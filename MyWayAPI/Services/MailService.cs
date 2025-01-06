using MimeKit;
using MailKit.Net.Smtp;

namespace MyWayAPI.Services
{
    public interface IMailService
    {
        public void SendEmail(string sendTo, string subject, string body);
    }
    public class MailService: IMailService
    {

        public void SendEmail(string sendTo, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MyWay System", "support@mw.com"));
            message.To.Add(new MailboxAddress(sendTo, sendTo));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @$"{body}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp-relay.gmail.com", 587, false);

                client.Authenticate("suport@mw.com", "xqro ttlg uxvc ccbv");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
