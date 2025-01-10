using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using SemanticKernelEmailAssistant.Services;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var configuration = builder.Configuration;

// Add CORS policy
var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(',');
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedOrigins ?? new[] { "http://localhost:3000" }) // Default for development
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

// Add Semantic Kernel services
builder.Services.AddSingleton<Kernel>(sp =>
{
    return Kernel.CreateBuilder()
        .AddAzureOpenAIChatCompletion(
            deploymentName: configuration["AzureOpenAI:DeploymentName"]!,
            endpoint: configuration["AzureOpenAI:Endpoint"]!,
            apiKey: configuration["AzureOpenAI:ApiKey"]!,
            modelId: "gpt-4"
        )
        .Build();
});

#pragma warning disable SKEXP0010

// Register AzureOpenAITextEmbeddingGenerationService
builder.Services.AddSingleton(sp =>
{
    return new AzureOpenAITextEmbeddingGenerationService(
        deploymentName: configuration["AzureOpenAI:TextEmbeddingDeploymentName"]!, // Add your embedding deployment name here
        endpoint: configuration["AzureOpenAI:Endpoint"]!,                         // Your Azure OpenAI endpoint
        apiKey: configuration["AzureOpenAI:ApiKey"]!,                             // Your Azure OpenAI API key
        modelId: "text-embedding-ada-002",                                        // Your model name, adjust if different
        httpClient: new HttpClient(),                                             // Optional custom HttpClient
        dimensions: 1536                                                          // Embedding dimensions for text-embedding-ada-002
    );
});

// Register your embedding service
builder.Services.AddScoped<EmbeddingService>();

// Register your services
builder.Services.AddSingleton<SemanticKernelService>();
builder.Services.AddScoped<OpenAISkill>();
builder.Services.AddSingleton<EmailService>();

// Add API and Swagger services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Email Assistant API",
        Version = "v1",
        Description = "API for email drafting and management"
    });
});

// Set server port
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// Enable Swagger but not at the root
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email Assistant API v1");
    c.RoutePrefix = "docs"; // Swagger is now accessible at /docs
    c.DefaultModelsExpandDepth(-1); // Hide schemas section
});

// Required middleware
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

// Map endpoints
app.MapControllers();

// Root endpoint for frontend communication
app.MapGet("/", () => "Backend is running. Navigate to /docs for API documentation.");

// Run the application
app.Run();
