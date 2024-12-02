using KanaTomo.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace KanaTomo.API.APIEmailService;

public interface IApiEmailService
{    
    Task<bool> SendEmailAsync(string email, string subject, string message);
}


public class ApiEmailService : IApiEmailService
{
    private readonly IApiEmailRepository _repository;
    private readonly ILogger<ApiEmailService> _logger;
    private readonly bool _isEmailConfigured;

    public ApiEmailService(IApiEmailRepository repository, ILogger<ApiEmailService> logger)
    {
        _repository = repository;
        _logger = logger;
        _isEmailConfigured = CheckEmailConfiguration();
    }

    private bool CheckEmailConfiguration()
    {
        var requiredEnvVars = new[] { "EMAIL_SMTP_SERVER", "EMAIL_PORT", "EMAIL_USERNAME", "EMAIL_PASSWORD" };
        return requiredEnvVars.All(var => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(var)));
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        if (!_isEmailConfigured)
        {
            _logger.LogWarning("Email configuration is incomplete. Skipping email sending.");
            return false;
        }

        return await _repository.SendEmailAsync(to, subject, body);
    }
}