using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
   public class Booking
    {
       
        public int BranchId { get; set; }
        public int BusID { get; set; }
        public String BranchName { get; set; }
        public int DebitLedgerId { get; set; }
        public int CreditLedgerId { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GST { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime JourneyDate { get; set; }
        public String DocNumber { get; set; }
        public String ClassName { get; set; }
        public int ClassId { get; set; }
    }
}
