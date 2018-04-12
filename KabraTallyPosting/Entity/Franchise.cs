using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
   public class Franchise
    {

        public int FranchiseId { get; set; }
        public string FranchiseName { get; set; }
        public string DocNumber { get; set; }
        public int DebitLedgerId { get; set; }
        public int CreditLedgerId { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal NetAmount { get; set; }
        public decimal ServiceTax { get; set; }
        public DateTime Bookingdate { get; set; }
        

    }
}
