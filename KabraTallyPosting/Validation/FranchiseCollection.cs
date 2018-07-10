using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KabraTallyPosting.Entity;
using KabraTallyPosting.TallyAPI;
using System.Data;
using KabraTallyPosting.Util;
using KabraTallyPosting.CRSAPI;

namespace KabraTallyPosting.Validation
{
    class FranchiseCollection : IValidate
    {

        public ValidationResult Validate(int companyId, DateTime journeyDate)
        {
            ValidationResult vr = new ValidationResult();
            vr.Status = 1;
            try
            {
                List<Franchise> franchiseList = FranchiseAPI.GetFranchiseCollection(companyId, journeyDate);


                if (franchiseList != null && franchiseList.Count > 0)
                {
                    for (int i = 0; i < franchiseList.Count; i++)
                    {
                        if (franchiseList[i].DebitLedgerId == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Ledger Id does not exists for JourneyDate: " + franchiseList[i].JourneyDate + " and For FranchiseID: " + franchiseList[i].FranchiseId;
                            Logger.WriteLog("Ledger Id does not exists for JourneyDate: " + franchiseList[i].JourneyDate + " and For FranchiseID: " + franchiseList[i].FranchiseId);
                            break;
                        }
                        if (franchiseList[i].ClassID == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = franchiseList[i].classname + " does not exists for JourneyDate: " + franchiseList[i].JourneyDate + " and For FranchiseID: " + franchiseList[i].FranchiseId;
                            Logger.WriteLog(franchiseList[i].classname + " does not exists for JourneyDate: " + franchiseList[i].JourneyDate + " and For FranchiseID: " + franchiseList[i].FranchiseId);
                            break;
                        }

                    }
                }
                else
                {
                    vr.Status = 0;
                    vr.ErrorMessage = "No data exists for Franchise Booking";
                }
            }
            catch (Exception ex)
            {
                vr.Status = 0;
                vr.ErrorMessage = "Error while validating Franchise  Booking";
                Logger.WriteLog("FranchiseAPI", "GetFranchiseCollection", ex.Message);
            }
            return vr;
        }
    }
}
