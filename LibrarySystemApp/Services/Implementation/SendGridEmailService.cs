using LibrarySystemApp.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LibrarySystemApp.Services.Implementation;

public class SendGridEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly string _sendGridApiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;

    public SendGridEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _sendGridApiKey = _configuration["SendGrid:ApiKey"] 
                          ?? throw new ArgumentNullException("SendGrid:ApiKey");
        _fromEmail = _configuration["SendGrid:FromEmail"] 
                     ?? throw new ArgumentNullException("SendGrid:FromEmail");
        _fromName = _configuration["SendGrid:FromName"] 
                    ?? throw new ArgumentNullException("SendGrid:FromName");
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string content)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress(_fromEmail, _fromName);
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
        
        var response = await client.SendEmailAsync(msg);
        return response.IsSuccessStatusCode;
    }
  
}