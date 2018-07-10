using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Util;
using KabraTallyPosting.Entity;

namespace KabraTallyPosting.CRSAPI
{
    public class AccountingUtil
    {

        public static string GetNarration(OfflineAgentDetails av)
        {
            string narration = "";
            try
            {
                narration = "BUS TICKET COLLECTION ON DT. " + av.bookingDate.ToString("dd-MM-yyyy") + " THROUGH: " + av.AgentName;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for AGENTID : " + av.AgentId + "Error: " + ex.Message);
            }
            return narration;
        }


        public static string GetNarration(Franchise fv)
        {
            string narration = "";
            try
            {
                narration = "FRANCHISE COLLECTION FOR. " + fv.JourneyDate.ToString("dd-MM-yyyy") + " THROUGH: " + fv.FranchiseName;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for FranchiseID : " + fv.FranchiseId + "Error: " + ex.Message);
            }
            return narration;
        }



        public static string GetNarration(GSTBooking gb)
        {
            string narration = "";
            try
            {
                narration = "TICKET NO. " + gb.TicketNo;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for GSTBOOKINGID : " + gb.BookingId + "Error: " + ex.Message);
            }
            return narration;
        }


        public static string GetDescription(GSTBooking gb)
        {
            string description = "";
            try
            {
                description = gb.CustomerName + "_" + gb.GSTNumber + '_' + gb.CustomerStateName;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for GSTBOOKINGID : " + gb.BookingId + "Error: " + ex.Message);
            }
            return description;
        }


        public static string GetDescription(Cancellation c)
        {
            string description = "";
            try
            {
                if (c.GSTNumber != "")
                {
                    description = c.CustomerName + "_" + c.GSTNumber + '_' + c.CustomerStateName;
                }
               
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for GSTBOOKINGID : " + c.BookingId + "Error: " + ex.Message);
            }
            return description;
        }


        //public static string GetNarration(Booking b)
        //{
        //    string narration = "";
        //    try
        //    {
        //        if (b.TicketNos != "")
        //        { narration = "TICKET NUMBERS: " + b.TicketNos; }
                   
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("AccountingUtil", "GetNarration", "Failed for Bookingdate : " + b.BookingDate + "Error: " + ex.Message);
        //    }
        //    return narration;
        //}
    }
}
