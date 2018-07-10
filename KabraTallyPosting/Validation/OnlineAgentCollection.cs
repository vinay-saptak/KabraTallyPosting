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
    public class OnlineAgentCollection : IValidate
    {

        public ValidationResult Validate(int companyId, DateTime  journeyDate)
        {
            ValidationResult vr = new ValidationResult();
            vr.Status = 1;
            try
            {
                List<OnlineAgentDetails> OnlineAgentCollectionList = OnlineAgentAPI.GetOnlineAgentCollection(companyId, journeyDate);


                if (OnlineAgentCollectionList != null && OnlineAgentCollectionList.Count > 0)
                {
                    for (int i = 0; i < OnlineAgentCollectionList.Count; i++)
                    {
                        if (OnlineAgentCollectionList[i].DebitLedgerId == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Ledger Id does not exists for AgentName: " + OnlineAgentCollectionList[i].AgentName + " and For AgentId: " + OnlineAgentCollectionList[i].AgentId + "JourneyDate: " + OnlineAgentCollectionList[i].JourneyDate;
                            Logger.WriteLog("Ledger Id does not exists for AgentName: " + OnlineAgentCollectionList[i].AgentName + " and For AgentId: " + OnlineAgentCollectionList[i].AgentId + "JourneyDate: " + OnlineAgentCollectionList[i].JourneyDate);
                            break;
                        }

                        if (OnlineAgentCollectionList[i].ClassID == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Class Id does not exists for BusId: " + OnlineAgentCollectionList[i].BusId + " and For AgentId: " + OnlineAgentCollectionList[i].AgentId + "JourneyDate: " + OnlineAgentCollectionList[i].JourneyDate;
                            Logger.WriteLog("Class Id does not exists for AgentName: " + OnlineAgentCollectionList[i].AgentName + " and For AgentId: " + OnlineAgentCollectionList[i].AgentId + "JourneyDate: " + OnlineAgentCollectionList[i].JourneyDate);
                            break;
                        }
                    }
                }
                else
                {
                    vr.Status = 0;
                    vr.ErrorMessage = "No data exists for Offline Agent Collection";
                }
            }
            catch (Exception ex)
            {
                vr.Status = 0;
                vr.ErrorMessage = "Error while validating Online Agent Collection";
                Logger.WriteLog("OnlineAgentAPI", "GetOnlineAgentCollection", ex.Message);
            }
            return vr;
        }
    }
}
