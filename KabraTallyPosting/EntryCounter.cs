using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting
{
   public class EntryCounter
    {

      
            private static int count = 0;
            private static EntryCounter entryCounter = new EntryCounter();
            private EntryCounter()
            { }

            public static EntryCounter GetInstance()
            {
                return entryCounter;
            }

            public void AddCount(int number)
            {
                count = count + number;
            }

            public int GetCount()
            {
                return count;
            }

            public void ResetCount()
            {
                count = 0;
            }
        
    }
}
