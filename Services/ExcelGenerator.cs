using ClosedXML.Excel;
using PosDailyMailer.Models;

namespace PosDailyMailer.Services
{
    public class ExcelGenerator
    {
        public string GenerateDailyReport(
            DateTime date,
            (int InvoiceCount, decimal TotalAmount, decimal TotalTax, decimal TotalNet) summary,
            List<SalesDataModel> headerSales,
            List<SalesDataDetailModel> itemSales)
        {
            string folder = @"C:\Reports";
            Directory.CreateDirectory(folder); 
            string filePath = Path.Combine(folder, $"SalesReport_{date:yyyyMMdd}.xlsx");

            using var workbook = new XLWorkbook();

            // Sheet 1
            var summaryData = new List<(string Label, object Value)>()
            {
                ("Date", date.ToShortDateString()),
                ("Invoice Count", summary.InvoiceCount),
                ("Total Gross Sales", summary.TotalAmount),
                ("Total Tax", summary.TotalTax),
                ("Total Net Sales", summary.TotalNet)
            };

            var wsSummary = workbook.Worksheets.Add("Total Sales Summary");
            wsSummary.Cell(1, 1).InsertTable(summaryData, "SummaryTable", true);

            // Sheet 2
            var wsHeader = workbook.Worksheets.Add("Header Sales");
            wsHeader.Cell(1, 1).InsertTable(headerSales);

            // Sheet 3
            var wsItems = workbook.Worksheets.Add("Item Sales");
            wsItems.Cell(1, 1).InsertTable(itemSales);

            workbook.SaveAs(filePath);

            return filePath;
        }
    }
}
