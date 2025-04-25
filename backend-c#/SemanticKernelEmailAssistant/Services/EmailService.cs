using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Graph;
using Microsoft.Graph.Models;

public class EmailService
{
    private readonly GraphServiceClient _graphClient;
    private readonly string _senderEmail;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _logger = logger;
       
        // Get credentials from Key Vault
        var keyVaultUri = configuration["KeyVault:Uri"];
        var tenantId = configuration["AzureAd:TenantId"];
        var clientId = configuration["AzureAd:ClientId"];
       
        if (string.IsNullOrEmpty(keyVaultUri))
        {
            throw new ArgumentNullException(nameof(keyVaultUri), "KeyVault Uri cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(tenantId))
        {
            throw new ArgumentNullException(nameof(tenantId), "TenantId cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentNullException(nameof(clientId), "ClientId cannot be null or empty.");
        }

        var credential = new DefaultAzureCredential();
        var keyVaultClient = new SecretClient(new Uri(keyVaultUri), credential);
       
        // Get secrets from Key Vault
        var clientSecret = keyVaultClient.GetSecret("GraphClientSecret").Value.Value;
        _senderEmail = keyVaultClient.GetSecret("SenderEmail").Value.Value;

        if (string.IsNullOrEmpty(_senderEmail))
        {
            throw new InvalidOperationException("Sender email address not found in Key Vault.");
        }

        // Initialize Graph client
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        var clientSecretCredential = new ClientSecretCredential(
            tenantId, clientId, clientSecret, options);

        _graphClient = new GraphServiceClient(clientSecretCredential, scopes);
    }

    public async Task SendEmailAsync(string subject, string body, List<string> recipients)
    {
        try
        {
            var message = new Message
            {
                Subject = subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = body
                },
                ToRecipients = recipients.Select(r => new Recipient
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = r
                    }
                }).ToList()
            };

            await _graphClient.Users[_senderEmail]
                .SendMail
                .PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
                {
                    Message = message,
                    SaveToSentItems = true
                });

            _logger.LogInformation("Email sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            throw;
        }
    }
}
