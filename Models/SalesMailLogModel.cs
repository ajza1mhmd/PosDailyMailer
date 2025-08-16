using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PosDailyMailer.Models
{
    public class SalesMailLogModel
    {
        public DateTime MailDate { get; set; }
        public string Status { get; set; }
        public DateTime? SentAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
