using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CityDiscoverTourist.Business.Helper.EmailHelper;

public class EmailSender : IEmailSender
{
    private static string errorInfo, Errormsg, ErrorLocation, extype, exurl = null, Frommail, ToMail, Sub, HostAdd, EmailHead, EmailSing;

    public EmailSender()
    {
    }

    public void SendEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);

        //Send(emailMessage);
    }

    public async Task SendEmailAsync(Message message)
    {
        var mailMessage = CreateEmailMessage(message);

        await SendAsync(mailMessage);
    }

    public void SendMailWithMailGun(Message message)
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

    /*
    public void SendMailException(System.Exception ex)
    {
       try
       {
           const string newline = "<br/>";
           Debug.Assert(ex.StackTrace != null, "ex.StackTrace != null");
           errorInfo = ex.StackTrace;
           Errormsg = ex.GetType().Name;
           extype = ex.GetType().ToString();

           ErrorLocation = ex.Message;
           EmailHead = "<b>Dear Team,</b>" + "<br/>" + "An exception occurred in a Application Url" + " " + exurl + " " + "With following Details" + "<br/>" + "<br/>";
           EmailSing = newline + "Thanks and Regards" + newline + "    " + "     " + "<b>Application Admin </b>" + "</br>";
           Sub = "Exception occurred" + " " + "in Application" + " " + exurl;
           var errorContext = EmailHead + "<b>Log Written Date: </b>" + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + newline +
                              "<b>Error Line No :</b>" + " " + errorInfo + "\t\n" + " " + newline +
                              "<b>Error Message:</b>" + " " + Errormsg + newline +
                              "<b>Exception Type:</b>" + " " + extype + newline +
                              "<b>Error Details :</b>" + " " + ErrorLocation + newline +
                              "<b>Error Page Url:</b>" + " " + exurl + newline + newline + newline + newline + EmailSing;
           using (var mailMessage = new MimeMessage())
           {
               mailMessage.From.Add(new MailboxAddress("Admin",
                   _emailConfig.From));
               mailMessage.To.Add(new MailboxAddress("User", _emailConfig.AdminEmail));
               mailMessage.Subject = Sub;
               mailMessage.Body = new TextPart(TextFormat.Html)
               {
                   Text = errorContext
               };
               using (var client = new SmtpClient())
               {
                   client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                   client.Connect (_emailConfig.SmtpServer, _emailConfig.Port, false);
                   client.AuthenticationMechanisms.Remove ("XOAUTH2");
                   client.Authenticate (_emailConfig.UserName, _emailConfig.Password);

                   client.Send (mailMessage);
                   client.Disconnect (true);
               }
           }
       }
       catch (System.Exception em)
       {
           em.ToString();
       }
    }

    public void SendMailException(ProblemDetails pd)
    {
        try
        {
           const string newline = "<br/>";
           errorInfo = (string) pd.Extensions["trace"];
           Errormsg = pd.Title;
           extype = pd.Type;

           ErrorLocation = pd.Detail;
           EmailHead = "<b>Dear Team,</b>" + "<br/>" + "An exception occurred in a Application Url" + " " + exurl + " " + "With following Details" + "<br/>" + "<br/>";
           EmailSing = newline + "Thanks and Regards" + newline + "    " + "     " + "<b>Application Admin </b>" + "</br>";
           Sub = "Exception occurred" + " " + "in Application" + " " + exurl;
           var errorContext = EmailHead + "<b>Log Written Date: </b>" + " " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + newline +
                              "<b>Error Line No :</b>" + " " + errorInfo + "\t\n" + " " + newline +
                              "<b>Error Message:</b>" + " " + Errormsg + newline +
                              "<b>Exception Type:</b>" + " " + extype + newline +
                              "<b>Error Details :</b>" + " " + ErrorLocation + newline +
                              "<b>Error Page Url:</b>" + " " + exurl + newline + newline + newline + newline + EmailSing;
           using (var mailMessage = new MimeMessage())
           {
               mailMessage.From.Add(new MailboxAddress("Admin",
                   _emailConfig.From));
               mailMessage.To.Add(new MailboxAddress("User", _emailConfig.AdminEmail));
               mailMessage.Subject = Sub;
               mailMessage.Body = new TextPart(TextFormat.Html)
               {
                   Text = errorContext
               };
               using (var client = new SmtpClient())
               {
                   client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                   client.Connect (_emailConfig.SmtpServer, _emailConfig.Port, false);
                   client.AuthenticationMechanisms.Remove ("XOAUTH2");
                   client.Authenticate (_emailConfig.UserName, _emailConfig.Password);

                   client.Send (mailMessage);
                   client.Disconnect (true);
               }
           }
       }
       catch (System.Exception em)
       {
           em.ToString();
       }
    }
    */

    private MimeMessage CreateEmailMessage(Message message)
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

    /*private void Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }*/

    private async Task SendAsync(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync("dathaha2000@gmail.com", "fjnmrkytxqxmbefa");

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