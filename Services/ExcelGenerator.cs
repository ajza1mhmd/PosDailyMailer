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
            var wsSummary = workbook.Worksheets.Add("Total Sales Summary");
            wsSummary.Cell(1, 1).Value = "Date";
            wsSummary.Cell(1, 2).Value = date.ToShortDateString();
            wsSummary.Cell(2, 1).Value = "Invoice Count";
            wsSummary.Cell(2, 2).Value = summary.InvoiceCount;
            wsSummary.Cell(3, 1).Value = "Total Gross Sales";
            wsSummary.Cell(3, 2).Value = summary.TotalAmount;
            wsSummary.Cell(4, 1).Value = "Total Tax";
            wsSummary.Cell(4, 2).Value = summary.TotalTax;
            wsSummary.Cell(5, 1).Value = "Total Net Sales";
            wsSummary.Cell(5, 2).Value = summary.TotalNet;

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
