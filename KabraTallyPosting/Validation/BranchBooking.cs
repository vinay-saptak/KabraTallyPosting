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
    public class BranchBooking : IValidate
    {

        public ValidationResult Validate(int companyId, DateTime journeyDate)
        {
            ValidationResult vr = new ValidationResult();
            vr.Status = 1;
            try
            {
                List<Booking> bookingsList = BookingsAPI.GetBranchBookings(companyId, journeyDate);


                if (bookingsList != null && bookingsList.Count > 0)
                {
                    for (int i = 0; i < bookingsList.Count; i++)
                    {
                        if (bookingsList[i].DebitLedgerId == 0 )
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "Ledger Id does not exists for JourneyDate: " + bookingsList[i].JourneyDate + " and For BranchId: " + bookingsList[i].BranchId ;
                            Logger.WriteLog("Ledger Id does not exists for JourneyDate: " + bookingsList[i].JourneyDate + " and For BranchId: " + bookingsList[i].BranchId);
                            break;
                        }
                        if (bookingsList[i].ClassId == 0)
                        {
                            vr.Status = 0;
                            vr.ErrorMessage = "ClassId does not exists for JourneyDate: " + bookingsList[i].JourneyDate + " and For BranchId: " + bookingsList[i].BranchId;
                            Logger.WriteLog("ClassId does not exists for JourneyDate: " + bookingsList[i].JourneyDate + " and For BranchId: " + bookingsList[i].BranchId);
                            break;
                        }

                    }
                }
                else
                {
                    vr.Status = 0;
                    vr.ErrorMessage = "No data exists for Branch  Booking";
                }
            }
            catch (Exception ex)
            {
                vr.Status = 0;
                vr.ErrorMessage = "Error while validating Branch  Booking";
                Logger.WriteLog("BookingsAPI", "GetBranchBookings", ex.Message);
            }
            return vr;
        }
    }
}

