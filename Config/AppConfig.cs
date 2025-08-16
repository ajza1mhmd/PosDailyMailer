using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosDailyMailer.Config
{
    public static class AppConfig
    {
        // Database
        public static string DbHost = "localhost";
        public static string DbName = "kmcc_dc_db";
        public static string DbUser = "grandChef";
        public static string DbPassword = "Login@123";

        // Email
        public static string SmtpHost = "sandbox.smtp.mailtrap.io";
        public static int SmtpPort = 587;
        public static string SmtpUser = "fb800be605a9c7";
        public static string SmtpPassword = "13e1c84fc18ed4";
        public static string EmailTo = "ajsal@ae.grandhyper.com";

        // FTP
        public static string FtpHost = "ftp.example.com";
        public static string FtpUser = "ftpuser";
        public static string FtpPassword = "ftppassword";

        public static string GetConnectionString()
        {
            return $"Server={DbHost};Database={DbName};User ID={DbUser};Password={DbPassword};";
        }
    }
    public static class MailerDbConfig
    {
        public static string DbHost = "localhost";
        public static string DbName = "pos_mailer_db";
        public static string DbUser = "grandChef";
        public static string DbPassword = "Login@123";

        public static string GetConnectionString()
        {
            return $"Server={DbHost};Database={DbName};User ID={DbUser};Password={DbPassword};";
        }
    }
}
