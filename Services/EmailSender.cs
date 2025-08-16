using MailKit.Net.Smtp;
using MimeKit;
using PosDailyMailer.Config;

namespace PosDailyMailer.Services
{
    public class EmailSender
    {
        public void SendReport(string attachmentFilePath, DateTime businessDate)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("POS Sales Report", AppConfig.SmtpUser));
            message.To.Add(MailboxAddress.Parse(AppConfig.EmailTo));
            message.Subject = $"Daily Sales Report - {businessDate:yyyy-MM-dd}";

            var builder = new BodyBuilder();
            //builder.TextBody = $"Hello,\n\nPlease find attached the daily sales report.\n\nRegards,\nPOS System\n\nReport Generated on - {DateTime.Now:yyyy-MM-dd}";
            builder.HtmlBody = $@"<html>
                                    <head>
                                        <style>
                                            body {{
                                                font-family: Arial, sans-serif;
                                                color: #333333;
                                            }}
                                            .header {{
                                                background-color: #007bff;
                                                color: white;
                                                padding: 10px;
                                                font-size: 18px;
                                                text-align: center;
                                            }}
                                            .content {{
                                                margin: 20px;
                                                font-size: 14px;
                                            }}
                                            .footer {{
                                                margin-top: 30px;
                                                font-size: 12px;
                                                color: #777777;
                                                text-align: center;
                                            }}
                                        </style>
                                    </head>
                                    <body>
                                        <div class='header'>POS Daily Sales Report</div>
                                        <div class='content'>
                                            <p>Hello,</p>
                                            <p>Please find attached the daily sales report for 
                                                <strong>{businessDate:yyyy-MM-dd}</strong>.</p>
                                            <p>Regards,<br>DKMCC POS System</p>
                                        </div>
                                        <div class='footer'>
                                            Report generated on {DateTime.Now:yyyy-MM-dd}
                                        </div>
                                    </body>
                                </html>";
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
