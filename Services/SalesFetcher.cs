using MySql.Data.MySqlClient;
using PosDailyMailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosDailyMailer.Services
{
    public class SalesFetcher
    {
        private readonly string _connectionString;

        public SalesFetcher(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SalesDataModel> GetHeaderSales(DateTime date)
        {
            var list = new List<SalesDataModel>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();


            string query = @"SELECT cm_store, ih_inv_no, ih_inv_date, ih_total_amt, ih_tax_amt, ih_net_amt, ih_invoiced_on, ih_invoice_by
                             FROM gc_invoice_head
                             WHERE DATE(ih_inv_date) = @date";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SalesDataModel
                {
                    cm_store = reader.GetInt32("cm_store").ToString(),
                    ih_inv_no = reader.GetString("ih_inv_no"),
                    ih_inv_date = reader.GetDateTime("ih_inv_date"),
                    ih_total_amt = reader.GetDecimal("ih_total_amt"),
                    ih_tax_amt = reader.GetDecimal("ih_tax_amt"),
                    ih_net_amt = reader.GetDecimal("ih_net_amt"),
                    ih_invoiced_on = reader.GetDateTime("ih_invoiced_on"),
                    ih_invoice_by = reader.GetString("ih_invoice_by")
                });
            }

            return list;
        }

        public List<SalesDataDetailModel> GetItemWiseSales(DateTime date)
        {
            var list = new List<SalesDataDetailModel>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = @"SELECT id_item_code, id_barcode, id_name, 
                                    SUM(id_quantity) AS total_qty, 
                                    id_price, 
                                    SUM(id_tax_amt) AS total_tax, 
                                    SUM(id_net_total) AS total_net
                             FROM gc_invoice_detl
                             WHERE  Date(id_created_on) = @date
                             GROUP BY id_item_code, id_barcode, id_name, id_price";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new SalesDataDetailModel
                {
                    id_item_code = reader["id_item_code"].ToString(),
                    id_barcode = reader["id_barcode"].ToString(),
                    id_name = reader["id_name"].ToString(),
                    id_quantity = reader.GetInt32("total_qty"),
                    id_price = reader.GetDecimal("id_price"),
                    id_tax_amt = reader.GetDecimal("total_tax"),
                    id_net_total = reader.GetDecimal("total_net")
                });
            }

            return list;
        }

        // Fetch summary data
        public (int InvoiceCount, decimal TotalAmount, decimal TotalTax, decimal TotalNet) GetSummary(DateTime date)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            string query = @"SELECT COUNT(*) AS InvoiceCount,
                                    SUM(ih_total_amt) AS TotalAmount,
                                    SUM(ih_tax_amt) AS TotalTax,
                                    SUM(ih_net_amt) AS TotalNet
                             FROM gc_invoice_head
                             WHERE DATE(ih_inv_date) = @date";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            reader.Read();

            return (
                reader.GetInt32("InvoiceCount"),
                reader.GetDecimal("TotalAmount"),
                reader.GetDecimal("TotalTax"),
                reader.GetDecimal("TotalNet")
            );
        }


    }
}
