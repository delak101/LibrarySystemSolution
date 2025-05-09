namespace LibrarySystemApp.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string content);

}