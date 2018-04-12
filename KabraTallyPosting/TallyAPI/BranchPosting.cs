using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using KabraTallyPosting.Util;
using KabraTallyPosting.CRSAPI;
using KabraTallyPosting.Entity;
using KabraTallyPosting.Validation;
using System.Net.NetworkInformation;

namespace KabraTallyPosting.TallyAPI
{
    class BranchPosting
    {

        public static ValidationResult ValidateData(int companyId, PostingJob pJob)
        {
            ValidationResult vr = new ValidationResult();
            vr.Status = 1;
            try
            {
                Logger.WriteLog("Validation Started...");

                List<IValidate> validationCollection = new List<IValidate>();
                validationCollection.Add(new BranchBooking());
                validationCollection.Add(new OnlineAgentCollection());
                validationCollection.Add(new OfflineAgentCollection());



                for (int i = 0; i < validationCollection.Count; i++)
                {
                    vr = validationCollection[i].Validate(companyId,pJob.JourneyDate);
                    if (vr.Status == 0)
                    {
                        break;
                    }
                }

                Logger.WriteLog("Validation Ended...");
            }
            catch (Exception ex)
            {
                vr.Status = 0;
                vr.ErrorMessage = vr.ErrorMessage + " Error in ValidateData";
                Logger.WriteLog("PostingCron", "ValidateData", ex.Message);
            }
            return vr;
        }

        public static bool IsConnectingIPAddress()
        {
            bool IsConnectingIPAddress = false;
            try
            {
                
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("52.76.182.185", 1000);
                if (reply != null)
                {
                    Logger.WriteLog("Status :  " + reply.Status + " \n Time : " + reply.RoundtripTime.ToString() + " \n Address : " + reply.Address);

                    IsConnectingIPAddress = true;
                }
               
            }
            catch
            {
                Logger.WriteLog("ERROR: You have Some TIMEOUT issue");
            }
            return IsConnectingIPAddress;
        }

        public static void PostBranchData(int companyid, DateTime journeyDate )
        {
            // Initialize entry counter to Zero
            EntryCounter.GetInstance().ResetCount();

            Boolean toPostBranchBookings = Convert.ToBoolean(ConfigurationManager.AppSettings["PostBranchBookings"].ToString());

            if (toPostBranchBookings)
            {
                #region Branch  Bookings 


                try
                {
                    Logger.WriteLog("Started Branch Booking...");
                    List<Booking> bookingsList = BookingsAPI.GetBranchBookings(companyid, journeyDate);
                   
                 
                    if (bookingsList != null && bookingsList.Count > 0)
                    {
                        Logger.WriteLog("Entry No 1 :  Branch Collection List : 1");
                        BookingsAPI.CreateJournalForUserWiseCollection(companyid, bookingsList);
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteLog("BranchPosting", "PostBranchBookings", "Exception in Posting Bookings: " + ex.Message);
                }


                #endregion
            }


           

            Boolean toPostOnlineAgentCollection = Convert.ToBoolean(ConfigurationManager.AppSettings["PostOnlineAgentCollection"].ToString());

            if (toPostOnlineAgentCollection)
            {
                #region Online Agent Collection 


                try
                {
                    Logger.WriteLog("Started Online Agent Collection...");
                    List<OnlineAgentDetails> AgentList = OnlineAgentAPI.GetOnlineAgentCollection(companyid, journeyDate);
                    Logger.WriteLog("Entry No 2: Online Agent Collection List: " + AgentList.Count);

                    if (AgentList != null && AgentList.Count > 0)
                    {
                        OnlineAgentAPI.CreateJournalForOnlineAgentCollection(companyid, AgentList);
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteLog("OnlineAgentPosting", "PostOnlineAgentCollection", "Exception in Posting AgentCollection: " + ex.Message);
                }


                #endregion
            }


            Boolean toPostOfflineAgentCollection = Convert.ToBoolean(ConfigurationManager.AppSettings["PostOfflineAgentCollection"].ToString());

            if (toPostOfflineAgentCollection)
            {
                #region Offline Agent Collection 


                try
                {
                    Logger.WriteLog("Started Offline Agent Collection...");
                    List<OfflineAgentDetails> AgentList = OfflineAgentAPI.GetOfflineAgentCollection(companyid, journeyDate);
                    Logger.WriteLog("Entry No 3: Offline Agent Collection List: " + AgentList.Count);

                    if (AgentList != null && AgentList.Count > 0)
                    {
                        OfflineAgentAPI.CreateJournalForOfflineAgentCollection(companyid, AgentList);
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteLog("OfflineAgentPosting", "PostOfflineAgentCollection", "Exception in Posting AgentCollection: " + ex.Message);
                }


                #endregion
            }




            Boolean toPostJournalEntry = Convert.ToBoolean(ConfigurationManager.AppSettings["PostJournalEntry"].ToString());
            if (toPostJournalEntry)
            {
                Logger.WriteLog("Journal Entry Creation and Posting");
                List<Journal> journalList = AccountingAPI.GetJournals(companyid);
                if (journalList != null && journalList.Count > 0)
                {
                    Logger.WriteLog("Journal list Count: " + journalList.Count);
                    for (int i = 0; i < journalList.Count; i++)
                    {
                        Journal jl = journalList[i];
                        List<JournalDetail> jdList = AccountingAPI.GetJournalDetail(jl.JournalId, jl.Type);
                        TallyResponse tr = TallyPostingAPI.PostJournal(jl, jdList);
                        if (tr != null && tr.Status == "1")
                        {
                            //AccountingAPI.UpdateTallyJournalIdInCRS(jl.JournalId, tr.EntityId);
                        }
                        else
                        {
                            Logger.WriteLogAlert("AccountingAPI " + "Error:PostingJournalIdInTally For JournalID: "  +  jl.JournalId);
                          
                        }
                    }

                    //if (EntryCounter.GetInstance().GetCount() != journalList.Count)
                    //{
                    //    Email.SendMail("Mismatch in No Of Entry Generated Vs. Posted for Journey Date: " + journalList[0].JournalDateTime + " No Of Entry Generated: " + EntryCounter.GetInstance().GetCount() + " No Of Entry Posted: " + journalList.Count);
                    //}
                }
            }



            
        }
    }
}
