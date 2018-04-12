using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
    public class Cancellation
    {
        public long BookingId { get; set; }
        public string PassengerName { get; set; }
        public decimal BaseFare { get; set; }
         public decimal RefundAmount { get; set; }
        public decimal Comm { get; set; }
        public DateTime CancelDate { get; set; }
        public DateTime JourneyDate { get; set; }
        public string CustomerName { get; set; }
        public int DebitLedgerId { get; set; }
        public int CreditLedgerId { get; set; }
        public string TicketNo { get; set; }
        public string VoucherNo { get; set; }
        public string Action { get; set; }
        public string Docnumber { get; set; }
        public string DocType { get; set; }
        public string CustomerStateName { get; set; }
        public int CompanyId { get; set; }
        public string GSTNumber { get; set; }
        public decimal GST { get; set; }

        public decimal IGST { get; set; }



    }
}
