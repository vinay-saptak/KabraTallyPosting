using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
    public class JournalDetail
    {
        public int JournalId { get; set; }
        public int LedgerId { get; set; }
        public string LedgerName { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
       
       
        public short IsDebit { get; set; }
        public decimal Amount { get; set; }
        public int DivisionId { get; set; }
        public int ClassId { get; set; }
        public string BillType { get; set; }
        public int SrNo { get; set; }
    }
}
