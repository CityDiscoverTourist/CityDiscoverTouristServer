using Microsoft.AspNetCore.Http;
using MimeKit;

namespace CityDiscoverTourist.Business.Helper.EmailHelper;

public class Message
{
    public Message(IEnumerable<string> to, string subject, string context, IFormFileCollection attachments)
    {
        To = new List<MailboxAddress>();

        To.AddRange(to.Select(x => new MailboxAddress("", x)));

        Subject = subject;
        Content = context;
        Attachments = attachments;
    }

    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
    public IFormFileCollection Attachments { get; set; }
}