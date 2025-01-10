using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SemanticKernelEmailAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidatePinController : ControllerBase
    {
        private readonly SecretClient _secretClient;

        public ValidatePinController(IConfiguration configuration)
        {
            var keyVaultUri = configuration["KeyVault:Uri"];
            if (string.IsNullOrEmpty(keyVaultUri))
            {
                throw new ArgumentNullException(nameof(keyVaultUri), "KeyVault Uri cannot be null or empty.");
            }
            _secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        [HttpPost("validate-pin")]
        public async Task<IActionResult> ValidatePin([FromBody] PinRequest request)
        {
            try
            {
                // Fetch valid PINs from Key Vault
                var secret = await _secretClient.GetSecretAsync("ValidPins");
                var validPins = secret.Value.Value.Split(',');

                // Check if the submitted PIN matches any valid PIN
                if (validPins.Contains(request.Pin))
                {
                    return Ok();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class PinRequest
    {
        public string? Pin { get; set; }
    }
}