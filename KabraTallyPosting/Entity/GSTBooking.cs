using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
    public class GSTBooking
    {

        public long BookingId { get; set; }
        public string CustomerName { get; set; }
        public decimal BaseFare { get; set; }
        public decimal GST { get; set; }
        public decimal TotalFare { get; set; }
        public decimal NetFare { get; set; }
        public string CustomerStateName { get; set; }
        public DateTime BookingDate { get; set; }
        public string TicketNo { get; set; }
        public string GSTNumber { get; set; }
        public int DebitLedgerId { get; set; }
        public int CreditLedgerId { get; set; }
        public string DocNumber { get; set; }
        public decimal IGST { get; set; }
        public decimal Comm { get; set; }
        public string DocType { get; set; }



    }
}
