using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.Entity
{
   public  class PostingJob
    {
        public int CompanyId { get; set; }
        public int JobId { get; set; }
        public DateTime JourneyDate { get; set; }
        public int StatusId { get; set; }
        public string Remarks { get; set; }
    }
}
