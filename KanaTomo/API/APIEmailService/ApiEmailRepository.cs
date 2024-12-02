using KanaTomo.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace KanaTomo.API.APIEmailService;

public interface IApiEmailRepository
{
    Task<bool> SendEmailAsync(string email, string subject, string message);   
}

public class ApiEmailRepository : IApiEmailRepository
{

    private readonly EmailConfiguration _emailConfig;
    
    public ApiEmailRepository(IOptions<EmailConfiguration> emailConfig)
    {
        _emailConfig = emailConfig.Value;
    }
    
    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("KanaTomo", _emailConfig.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
        
        return true;
    }
}