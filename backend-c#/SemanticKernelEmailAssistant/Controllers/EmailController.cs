using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SemanticKernelEmailAssistant.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SemanticKernelEmailAssistant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly OpenAISkill _openAISkill;
        private readonly ILogger<EmailController> _logger;
        private readonly EmailService _emailService;

        public EmailController(OpenAISkill openAISkill, ILogger<EmailController> logger, EmailService emailService)
        {
            _openAISkill = openAISkill;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("generate-email")]
        public async Task<IActionResult> GenerateEmailDraft([FromBody] EmailRequest request)
        {
            _logger.LogInformation("Received email generation request: Subject - {Subject}, Description - {Description}",
                request.Subject, request.Description);
            try
            {
                var emailDraft = await _openAISkill.GenerateEmailDraftAsync(request.Subject, request.Description);
               
                _logger.LogInformation("Generated email draft: {Draft}", emailDraft);
                return Ok(new { draft = emailDraft });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating email draft");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
         
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            _logger.LogInformation("Received email send request: Subject - {Subject}, Recipients - {Recipients}",
                request.Subject, string.Join(", ", request.Recipients));
            try
            {
                await _emailService.SendEmailAsync(
                    request.Subject,
                    request.Body,
                    request.Recipients
                );
               
                _logger.LogInformation("Email sent successfully to {Recipients}", 
                    string.Join(", ", request.Recipients));
                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipients}", 
                    string.Join(", ", request.Recipients));
                return StatusCode(500, new { error = "Failed to send email" });
            }
        }
    }

    public class EmailRequest
    {
        public required string Subject { get; set; }
        public required string Description { get; set; }
    }

    public class SendEmailRequest
    {
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public required List<string> Recipients { get; set; }
    }
}