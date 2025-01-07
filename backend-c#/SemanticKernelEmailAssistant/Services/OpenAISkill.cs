using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace SemanticKernelEmailAssistant.Services
{
public class OpenAISkill
{
    private readonly Kernel _kernel;

    public OpenAISkill(SemanticKernelService semanticKernelService)
    {
        _kernel = semanticKernelService.GetKernel();
    }

    public async Task<string> GenerateEmailDraftAsync(string subject, string description)
    {
        try
        {
            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            
            var message = new ChatMessageContent(
                AuthorRole.User,
                $"Draft a professional email with the following details:\nSubject: {subject}\nDescription: {description}"
            );

            var result = await chatCompletionService.GetChatMessageContentAsync(message.Content ?? string.Empty);
            return result?.Content ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error generating email draft: {ex.Message}", ex);
        }
    }
}
}