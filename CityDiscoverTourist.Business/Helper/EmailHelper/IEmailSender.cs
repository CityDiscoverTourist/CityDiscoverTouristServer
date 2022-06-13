namespace CityDiscoverTourist.Business.Helper.EmailHelper;

public interface IEmailSender
{
    //public void SendEmail(Message message);
    Task SendEmailAsync(Message message);

    Task SendMailConfirmAsync(string email, string subject, string htmlMessage);
    //public void SendMailWithMailGun(Message message);
    //public void SendMailException(System.Exception ex);
    //void SendMailException(ProblemDetails pd);
}