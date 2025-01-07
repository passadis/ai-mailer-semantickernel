using Microsoft.SemanticKernel;
using SemanticKernelEmailAssistant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
var configuration = builder.Configuration;

// Add Semantic Kernel services
builder.Services.AddSingleton<Kernel>(sp =>
{
    return Kernel.CreateBuilder()
        .AddAzureOpenAIChatCompletion(
            deploymentName: configuration["AzureOpenAI:DeploymentName"]!,
            endpoint: configuration["AzureOpenAI:Endpoint"]!,
            apiKey: configuration["AzureOpenAI:ApiKey"]!,
            modelId: "gpt-4"  // or your specific model
        )
        .Build();
});

// Register your services
builder.Services.AddSingleton<SemanticKernelService>();
builder.Services.AddScoped<OpenAISkill>();
builder.Services.AddSingleton<EmailService>();

// Add other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();