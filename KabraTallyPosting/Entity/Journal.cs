using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
   public class Journal
    {
        public int JournalId { get; set; }
        public int MyProperty { get; set; }
        public string DocType { get; set; }
        public string DocSubtype { get; set; }
        public string DocNumber { get; set; }
        public string DocFormattedNumber { get; set; }
        public string AccSysId { get; set; }
        public DateTime AccSysPostDateTime { get; set; }
        public DateTime JournalEntryDateTime { get; set; }
        public DateTime JournalDateTime { get; set; }
        public int CurrencyId { get; set; }
        public string Narration { get; set; }
        public string Type { get; set; }
        public long BookingId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
    }
}
