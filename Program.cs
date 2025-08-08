using PosDailyMailer.Config;
using PosDailyMailer.Models;
using PosDailyMailer.Services;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = AppConfig.GetConnectionString();
        //DateTime reportDate = DateTime.Today.AddDays(-1); // Yesterday’s sales
        DateTime reportDate = new DateTime(2025, 6, 10);

        var fetcher = new SalesFetcher(connectionString);

        var summary = fetcher.GetSummary(reportDate);
        var headers = fetcher.GetHeaderSales(reportDate);
        var items = fetcher.GetItemWiseSales(reportDate);

        var excelGen = new ExcelGenerator();
        string filePath = excelGen.GenerateDailyReport(reportDate, summary, headers, items);

        Console.WriteLine($"Report generated: {filePath}");

        // Send email
        var emailSender = new EmailSender();
        emailSender.SendReport(filePath);

        Console.WriteLine("Email sent successfully!");
    }
}
