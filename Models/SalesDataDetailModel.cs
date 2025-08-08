using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosDailyMailer.Models
{
    public class SalesDataDetailModel
    {
        public string id_item_code { get; set; }
        public string id_barcode { get; set; }
        public string id_name { get; set; }
        public int id_quantity { get; set; }
        public decimal id_price { get; set; }
        public decimal id_tax_amt { get; set; }
        public decimal id_net_total { get; set; }
    }
}
