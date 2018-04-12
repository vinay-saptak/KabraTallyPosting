using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.TallyAPI;
using System.Data;
using KabraTallyPosting.Util;
using KabraTallyPosting.CRSAPI;

namespace KabraTallyPosting.Validation
{
   public class OfflineAgentCollection : IValidate
    {

        public ValidationResult Validate(int companyId, DateTime journeyDate)
        {
            ValidationResult vr = new ValidationResult();
            vr.Status = 1;
            try
            {
                List<OfflineAgentDetails> OFFlineAgentCollectionList = OfflineAgentAPI.GetOfflineAgentCollection(companyId, journeyDate);


                if (OFFlineAgentCollectionList != null && OFFlineAgentCollectionList.Count > 0)
                {
                    for (int i = 0; i < OFFlineAgentCollectionList.Count; i++)
                    {
                        if (OFFlineAgentCollectionList[i].DebitLedgerId == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Ledger Id does not exists for AgentName: " + OFFlineAgentCollectionList[i].AgentName + " and For AgentId: " + OFFlineAgentCollectionList[i].AgentId + "JourneyDate: " + OFFlineAgentCollectionList[i].JourneyDate;
                            break;
                        }
                        if (OFFlineAgentCollectionList[i].ClassID == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Class Id does not exists for BusId: " + OFFlineAgentCollectionList[i].BusId + " and For AgentId: " + OFFlineAgentCollectionList[i].AgentId + "JourneyDate: " + OFFlineAgentCollectionList[i].JourneyDate;
                            break;
                        }

                    }
                }
                else
                {
                    vr.Status = 0;
                    //vr.ErrorMessage = "No data exists for Offline Agent Collection";
                }
            }
            catch (Exception ex)
            {
                vr.Status = 0;
                vr.ErrorMessage = "Error while validating Offline Agent Collection";
                Logger.WriteLog("OfflineAgentAPI", "GetOfflineAgentCollection", ex.Message);
            }
            return vr;
        }
    }
}
