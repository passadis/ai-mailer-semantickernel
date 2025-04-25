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
        private readonly EmbeddingService _embeddingService;

        public EmailController(OpenAISkill openAISkill, ILogger<EmailController> logger, EmailService emailService, EmbeddingService embeddingService)
        {
            _openAISkill = openAISkill;
            _logger = logger;
            _emailService = emailService;
            _embeddingService = embeddingService;
        }

        [HttpPost("generate-email")]
        public async Task<IActionResult> GenerateEmailDraft([FromBody] EmailRequest request)
        {
            _logger.LogInformation("Received email generation request"),
                request.Subject, request.Description);

            try
            {
                // Generate draft
                var emailDraft = await _openAISkill.GenerateEmailDraftAsync(request.Subject, request.Description);

                // Generate embedding
                var embedding = await _embeddingService.GenerateEmbeddingAsync(emailDraft);

                // Store draft embedding
                await _embeddingService.StoreEmbeddingAsync(emailDraft, embedding, "draft");

                _logger.LogInformation("Generated and stored draft embedding");
                return Ok(new { draft = emailDraft });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating or storing draft embedding");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

         
        [HttpPost("send")]
        var sanitizedSubject = request.Subject.Replace("\n", "").Replace("\r", "");
        var sanitizedRecipients = string.Join(", ", request.Recipients).Replace("\n", "").Replace("\r", "");



        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            logger.LogInformation("Email Subject: {Subject}, Recipients: {Recipients}", sanitizedSubject, sanitizedRecipients);

            try
            {
                // Send email
                await _emailService.SendEmailAsync(request.Subject, request.Body, request.Recipients);

                // Generate embedding
                var embedding = await _embeddingService.GenerateEmbeddingAsync(request.Body);

                // Store sent email embedding
                await _embeddingService.StoreEmbeddingAsync(request.Body, embedding, "sent");

                _logger.LogInformation("Sent email and stored embedding");
                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email and store embedding");
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
