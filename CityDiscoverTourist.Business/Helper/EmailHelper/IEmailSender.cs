using TutorHelper_v2.Business.Helper.EmailHelper;

namespace CityDiscoverTourist.Business.Helper.EmailHelper;

public interface IEmailSender
{
    public void SendEmail(Message message);
    Task SendEmailAsync(Message message);
    public void SendMailWithMailGun(Message message);
    public void SendMailException(System.Exception ex);
    //void SendMailException(ProblemDetails pd);
}