using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosDailyMailer.Models
{
    public class SalesDataModel
    {
        public string cm_store { get; set; }
        public string ih_inv_no { get; set; }
        public DateTime ih_inv_date { get; set; }
        public decimal ih_total_amt { get; set; }
        public decimal ih_tax_amt { get; set; }
        public decimal ih_net_amt { get; set; }
        public DateTime ih_invoiced_on { get; set; }
        public string ih_invoice_by { get; set; }
    }
}
