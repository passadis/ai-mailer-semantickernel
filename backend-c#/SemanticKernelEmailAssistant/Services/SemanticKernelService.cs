using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernelEmailAssistant.Services
{
public class SemanticKernelService
{
    private readonly Kernel _kernel;
    
    public SemanticKernelService(IConfiguration configuration)
    {
        var azureOpenAIEndpoint = configuration["AzureOpenAI:Endpoint"];
        var azureOpenAIKey = configuration["AzureOpenAI:ApiKey"];
        var deploymentName = configuration["AzureOpenAI:DeploymentName"];
        
        if (string.IsNullOrEmpty(deploymentName))
            throw new ArgumentNullException(nameof(deploymentName));
        if (string.IsNullOrEmpty(azureOpenAIEndpoint))
            throw new ArgumentNullException(nameof(azureOpenAIEndpoint));
        if (string.IsNullOrEmpty(azureOpenAIKey))
            throw new ArgumentNullException(nameof(azureOpenAIKey));

        _kernel = Kernel.CreateBuilder()
            .AddAzureOpenAIChatCompletion(
                deploymentName: deploymentName,
                endpoint: azureOpenAIEndpoint,
                apiKey: azureOpenAIKey,
                modelId: "gpt-4"  // Note: Changed from "gpt4" to "gpt-4"
            )
            .Build();
    }

    public Kernel GetKernel() => _kernel;
}
}