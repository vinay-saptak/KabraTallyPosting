using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
   public class OnlineAgentDetails
    {


        public decimal TotalFare { get; set; }
        public decimal AgentComm { get; set; }
        public string AgentName { get; set; }
        public string Docnumber { get; set; }
        public decimal GST { get; set; }
        public decimal Discount { get; set; }
        public decimal IGST { get; set; }
        public decimal NetAmount { get; set; }
        public int DebitLedgerId { get; set; }
        public int AgentId { get; set; }
        public int BusId { get; set; }
        public DateTime bookingDate { get; set; }
        public DateTime JourneyDate { get; set; }
        public int CreditLedgerId { get; set; }
        public string ClassName { get; set; }
        public int ClassID { get; set; }

    }
}
