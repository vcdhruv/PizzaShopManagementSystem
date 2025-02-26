using System.Net;
using System.Net.Mail;
using Pizza_Shop_Management_System.Services.Interfaces;
using DAL.ViewModels;
using Microsoft.Extensions.Configuration;

namespace Pizza_Shop_Management_System.Services;

public class EmailService
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendEmail(string toEmail , string subject , string body , bool isBodyHtml = false)
    {
        // Retrieve the sender email address from the configuration.
        string? email_ = configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
        
        // Retrieve the sender email password from the configuration.
        string? password = configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
        
        // Retrieve the mail server (SMTP host) from the configuration.
        string? host = configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");

        // Retrieve the sender's display name from the configuration.
        string? senderName = configuration.GetValue<string>("EMAIL_CONFIGURATION:SENDERNAME");

        // Retrieve the SMTP port number from the configuration.
        int port = configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

        var smtpClient = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(email_, password),

            EnableSsl = true,
            UseDefaultCredentials = true,
        };

        // Create a MailAddress object with the sender's email and display name.
        MailAddress fromAddress = new MailAddress(email_, senderName);

        MailMessage message = new MailMessage()
        {
            From = fromAddress,
            Subject = subject,
            Body = body,
            IsBodyHtml = isBodyHtml
        };


        // Add the recipient's email address to the message.
        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }
}