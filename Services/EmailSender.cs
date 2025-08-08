using MailKit.Net.Smtp;
using MimeKit;
using PosDailyMailer.Config;

namespace PosDailyMailer.Services
{
    public class EmailSender
    {
        public void SendReport(string attachmentFilePath)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("POS Sales Report", AppConfig.SmtpUser));
            message.To.Add(MailboxAddress.Parse(AppConfig.EmailTo));
            message.Subject = $"Daily Sales Report - {DateTime.Now:yyyy-MM-dd}";

            var builder = new BodyBuilder();
            builder.TextBody = "Hello,\n\nPlease find attached the daily sales report.\n\nRegards,\nPOS System";

            builder.Attachments.Add(attachmentFilePath);

            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(AppConfig.SmtpHost, AppConfig.SmtpPort, false);

            smtp.Authenticate(AppConfig.SmtpUser, AppConfig.SmtpPassword);

            smtp.Send(message);
            smtp.Disconnect(true);
        }
    }
}
