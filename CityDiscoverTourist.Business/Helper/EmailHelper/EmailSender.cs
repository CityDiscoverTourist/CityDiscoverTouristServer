using MailKit.Net.Smtp;
using MimeKit;

namespace CityDiscoverTourist.Business.Helper.EmailHelper;

public class EmailSender : IEmailSender
{
    private readonly EmailConfiguration _emailConfig;

    public EmailSender(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task SendEmailAsync(Message message)
    {
        var mailMessage = CreateEmailMessage(message);

        await SendAsync(mailMessage);
    }

    public async Task SendMailConfirmAsync(string email, string subject, string htmlMessage)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("City Tourist", _emailConfig.From));
        emailMessage.Subject = subject;
        emailMessage.To.Add(new MailboxAddress("", email));
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = string.Format("<html>{0}</html>", htmlMessage)
        };

        emailMessage.Body = bodyBuilder.ToMessageBody();
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailConfig.From, _emailConfig.Password);
            await client.SendAsync(emailMessage);
        }
        finally
        {
            await client.DisconnectAsync(true);
            client.Dispose();
        }
    }


    public static void SendMailWithMailGun(Message message)
    {
        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress("Admin",
            "postmaster@sandbox583bda9281f2472f83cbcfa4ddccf205.mailgun.org"));
        mailMessage.To.Add(new MailboxAddress("User", "dathaha2000@gmail.com"));
        mailMessage.Subject = message.Subject;
        mailMessage.Body = new TextPart("plain")
        {
            Text = message.Content
        };

        using (var client = new SmtpClient())
        {
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

            client.Connect ("smtp.mailgun.org", 587, false);
            client.AuthenticationMechanisms.Remove ("XOAUTH2");
            client.Authenticate ("postmaster@sandbox583bda9281f2472f83cbcfa4ddccf205.mailgun.org",
                "ff91ac5a481f75f1a76bc3c93d3adce8-cac494aa-4707a7e9");

            client.Send (mailMessage);
            client.Disconnect (true);
        }
    }

    private static MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("", "dathaha2000@gmail.com"));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
            { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

        /*if (message.Attachments.Any())
        {
            byte[] fileBytes;
            foreach (var attachment in message.Attachments)
            {
                using (var ms = new MemoryStream())
                {
                    attachment.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }

                bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
            }
        }*/

        emailMessage.Body = bodyBuilder.ToMessageBody();
        return emailMessage;
    }


    private async Task SendAsync(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.From, _emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}