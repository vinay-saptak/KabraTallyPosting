﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.Util;

namespace KabraTallyPosting.TallyAPI
{
    public class TallyPostingAPI
    {
        public static TallyResponse PostJournal(Journal jl, List<JournalDetail> jdList)
        {
            string JournalXML = "";
            string status = "";
            TallyResponse tr = null;
            if (jl.Action == "Create")
            {
                JournalXML = TallyMessageCreator.CreateJournalXML(jl, jdList);
                status = TallyConnector.PostDataToTally(JournalXML);
                Logger.WriteLog("Response for journal Id: " + jl.JournalId + ": " + status);
                tr = TallyMessageCreator.GetStatusFromResponseXML(status);
            }
            return tr;
        }

        public static bool IsTallyInstanceRunning()
        {
            bool isTallyInstanceRunning = false;
            try
            {
                //string ledgerXML = TallyMessageCreator.CreateSingleLedgerDetailRequestMessage();
                //TallyConnector.PostDataToTally(ledgerXML);

                string ledgerXML = TallyMessageCreator.CreateIsCompanyOpenRequestMessage();
                string response = TallyConnector.isCompanyOpen(ledgerXML);


                if (!response.Contains("Server company is not available!"))
                {
                    isTallyInstanceRunning = true;
                }
                else
                {
                    Logger.WriteLog("TallyPostingAPI", "IsTallyInstanceRunning", "Company is Not Selected In Tally");
                }
                    
            }
            catch(Exception ex)
            {
                Logger.WriteLog("TallyPostingAPI", "IsTallyInstanceRunning", ex.Message);
            }
            return isTallyInstanceRunning;
        }
    }
}
