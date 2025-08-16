using PosDailyMailer.Config;
using PosDailyMailer.Models;
using PosDailyMailer.Services;

class Program
{
    static void Main(string[] args)
    {
        var mailerDb = new MailerDbService();
        var fetcher = new SalesFetcher(AppConfig.GetConnectionString());
        var excelGen = new ExcelGenerator();
        var emailSender = new EmailSender();

        string connectionString = AppConfig.GetConnectionString();
        DateTime reportDate = DateTime.Today.AddDays(-1); // Yesterday’s sales
        DateTime startDate = mailerDb.GetLastSentDate()?.AddDays(1) ?? DateTime.Today.AddMonths(-1);
        //DateTime reportDate = new DateTime(2025, 6, 10);
        
        for (DateTime date = startDate; date <= reportDate; date = date.AddDays(1))
        {
            try
            {
                mailerDb.InsertPending(date);
                var summary = fetcher.GetSummary(date);
                var headers = fetcher.GetHeaderSales(date);
                var items = fetcher.GetItemWiseSales(date);

                string filePath = excelGen.GenerateDailyReport(date, summary, headers, items);
                Console.WriteLine($"Report generated for {date:yyyy-MM-dd}: {filePath}");

                emailSender.SendReport(filePath, date);
                mailerDb.MarkSent(date);
                Console.WriteLine($"Email sent successfully for {date:yyyy-MM-dd}");

            }
            catch (Exception ex) 
            {
                mailerDb.MarkFailed(date, ex.Message);
                Console.WriteLine($"Failed for {date:yyyy-MM-dd}: {ex.Message}");
            }
        }

        //    var summary = fetcher.GetSummary(reportDate);
        //var headers = fetcher.GetHeaderSales(reportDate);
        //var items = fetcher.GetItemWiseSales(reportDate);

        //var excelGen = new ExcelGenerator();
        //string filePath = excelGen.GenerateDailyReport(reportDate, summary, headers, items);

        //Console.WriteLine($"Report generated: {filePath}");

        //// Send email
        //var emailSender = new EmailSender();
        //emailSender.SendReport(filePath);

        Console.WriteLine("All pending reports processed.");
    }
}
