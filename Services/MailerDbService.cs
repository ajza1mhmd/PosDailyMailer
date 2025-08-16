using MySql.Data.MySqlClient;
using PosDailyMailer.Config;
using PosDailyMailer.Models;

namespace PosDailyMailer.Services
{
    public class MailerDbService
    {
        private readonly string _connectionString;

        public MailerDbService()
        {
            _connectionString = MailerDbConfig.GetConnectionString();
        }

        public DateTime? GetLastSentDate()
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "SELECT MAX(MailDate) FROM SalesMailLog WHERE Status='Sent';", conn);

            var result = cmd.ExecuteScalar();
            if (result != DBNull.Value)
                return Convert.ToDateTime(result);

            return null;
        }

        public void InsertPending(DateTime date)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "INSERT IGNORE INTO SalesMailLog (MailDate, Status) VALUES (@date, 'Pending');", conn);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.ExecuteNonQuery();
        }

        public void MarkSent(DateTime date)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE SalesMailLog SET Status='Sent', SentAt=NOW(), ErrorMessage=NULL WHERE MailDate=@date;", conn);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.ExecuteNonQuery();
        }

        public void MarkFailed(DateTime date, string error)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();
            var cmd = new MySqlCommand(
                "UPDATE SalesMailLog SET Status='Failed', ErrorMessage=@error WHERE MailDate=@date;", conn);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@error", error);
            cmd.ExecuteNonQuery();
        }
    }
}
