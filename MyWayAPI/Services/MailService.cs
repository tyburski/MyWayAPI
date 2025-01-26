using MimeKit;
using MailKit.Net.Smtp;

namespace MyWayAPI.Services
{
    public interface IMailService
    {
        public void SendEmail(string sendTo, string subject, string fileName);
    }
    public class MailService: IMailService
    {

        public void SendEmail(string sendTo, string subject, string fileName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MyWay System", "tyburski@post.pl"));
            message.To.Add(new MailboxAddress(sendTo, sendTo));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Szanowni Państwo,\nW załączniku znajduje się raport z trasy przedstawionej w temacie wiadomości.\n\nZ poważaniem,\nMyWay System";
            bodyBuilder.Attachments.Add($"./{fileName}");
            message.Body = bodyBuilder.ToMessageBody();


            using (var client = new SmtpClient())
            {
                client.Connect("post.pl", 587, false);

                client.Authenticate("tyburski@post.pl", "VaFYy8Wj");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
