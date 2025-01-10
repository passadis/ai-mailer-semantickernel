using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class EmbeddingController : ControllerBase
{
    private readonly EmbeddingService _embeddingService;
    private readonly ILogger<EmbeddingController> _logger;

    public EmbeddingController(EmbeddingService embeddingService, ILogger<EmbeddingController> logger)
    {
        _embeddingService = embeddingService;
        _logger = logger;
    }

    [HttpPost("generate-and-store")]
    public async Task<IActionResult> GenerateAndStoreEmbedding([FromBody] EmbeddingRequest request)
    {
        _logger.LogInformation("Received embedding request: {Input}", request.Input);
        try
        {
            // Generate the embedding
            var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Input);

            // Store the embedding in the database
            await _embeddingService.StoreEmbeddingAsync(request.Input, embedding, "defaultType");

            _logger.LogInformation("Embedding successfully generated and stored for input: {Input}", request.Input);
            return Ok(new { message = "Embedding generated and stored successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating or storing embedding for input: {Input}", request.Input);
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    public class EmbeddingRequest
    {
        public required string Input { get; set; }
    }
    
    [HttpPost("search")]
        public async Task<IActionResult> SearchEmbeddings([FromBody] SearchRequest request)
        {
            var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(request.Input);
            var results = await _embeddingService.SearchSimilarEmbeddingsAsync(queryEmbedding, request.Limit);
            return Ok(results);
        }

        public class SearchRequest
        {
            public required string Input { get; set; }
            public int Limit { get; set; } = 5;
        }
}
