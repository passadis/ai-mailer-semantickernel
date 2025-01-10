using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#pragma warning disable SKEXP0010
public class EmbeddingService
{
    private readonly AzureOpenAITextEmbeddingGenerationService _embeddingService;
    private readonly string _connectionString;

    // Constructor with Dependency Injection
    public EmbeddingService(AzureOpenAITextEmbeddingGenerationService embeddingService, IConfiguration configuration)
    {
        _embeddingService = embeddingService;
        
        // Initialize _connectionString from configuration
        _connectionString = configuration["Neon:ConnectionString"]
            ?? throw new ArgumentNullException("Neon:ConnectionString", "Database connection string not found in configuration.");
    }

    public async Task<List<float>> GenerateEmbeddingAsync(string input)
    {
        // Generate embeddings for the input text
        var embeddings = await _embeddingService.GenerateEmbeddingsAsync(new List<string> { input });

        // Return the first embedding as a list of floats
        return embeddings.Count > 0 ? embeddings.First().ToArray().ToList() : new List<float>();
    }

    public async Task StoreEmbeddingAsync(string inputText, List<float> embedding, string type)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            INSERT INTO embeddings (input_text, embedding, type)
            VALUES (@inputText, @embedding, @type)";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("inputText", inputText);
        cmd.Parameters.AddWithValue("embedding", embedding.ToArray());
        cmd.Parameters.AddWithValue("type", type);

        await cmd.ExecuteNonQueryAsync();
    }
    
    public async Task<List<SearchResult>> SearchSimilarEmbeddingsAsync(List<float> queryEmbedding, int limit)
    {
        var results = new List<SearchResult>();

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"
            SELECT 
                input_text,
                1 - (embedding <=> @queryEmbedding::vector) as similarity
            FROM embeddings
            ORDER BY embedding <=> @queryEmbedding::vector
            LIMIT @limit";

        using var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters.AddWithValue("queryEmbedding", NpgsqlTypes.NpgsqlDbType.Real | NpgsqlTypes.NpgsqlDbType.Array, queryEmbedding.ToArray());
        cmd.Parameters.AddWithValue("limit", limit);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new SearchResult
            {
                InputText = reader.GetString(0),
                Similarity = reader.GetDouble(1),
            });
        }

        return results;
    }

public class SearchResult
{
    public string? InputText { get; set; }
    public double Similarity { get; set; }
}
}





        
