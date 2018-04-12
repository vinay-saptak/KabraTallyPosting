using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
    public class BusWise
    {

        public decimal TotalFare { get; set; }
        public decimal BusComm { get; set; }
        public string BusNumber { get; set; }
        public string DocNumber { get; set; }
        public decimal GST { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal NetAmount { get; set; }
        public decimal OfflineAgentComm { get; set; }
        public decimal OnlineAgentComm { get; set; }
        public int AccsysLedgerId { get; set; }
        public int BusiD { get; set; }
        public DateTime JourneyDate { get; set; }
        public int CommLedgerId { get; set; }


    }
}
