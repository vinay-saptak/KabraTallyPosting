using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using KabraTallyPosting.CRSAPI;
using System.Configuration;
using KabraTallyPosting.Util;
using KabraTallyPosting.TallyAPI;
using KabraTallyPosting.Entity;
using KabraTallyPosting.ExportAPI;




namespace KabraTallyPosting
{
    class Program
    {
        static void Main(string[] args)
        {

            int companyId = 0;
            int IntervalOfDays = 0;

            if (!Int32.TryParse(ConfigurationManager.AppSettings["CompanyId"].ToString(), out companyId) || companyId == 0)
            {
                Logger.WriteLog("Program", "Main", "Company Not Defined.");
                return;
            }

            if (!Int32.TryParse(ConfigurationManager.AppSettings["IntervalOfDays"].ToString(), out IntervalOfDays) || IntervalOfDays == 0)
            {
                Logger.WriteLog("Program", "Main", "Company Not Defined.");
                return;
            }
            string tallyCompanyName = "";

            tallyCompanyName = ConfigurationManager.AppSettings["TallyCompanyName"].ToString();

            if (tallyCompanyName.Trim() == "")
            {
                Logger.WriteLog("Program", "Main", "Company Not Defined.");
                return;
            }

            Boolean isInsertPendingJobs = Convert.ToBoolean(ConfigurationManager.AppSettings["InsertPendingJobs"].ToString());
            if (isInsertPendingJobs)
            {
                List<PostingJob> postingJobList = PostingAPI.GetPendingPostingJobsForInputDate(companyId, IntervalOfDays);

                if (postingJobList != null && postingJobList.Count > 0)
                {
                    PostingAPI.InsertPostingStatus(companyId, postingJobList);
                }

            }

           

            //if (!BranchPosting.IsConnectingIPAddress())
            //{
            //    var Remarks = "Unable to connect to Server";
            //    Logger.WriteLog("Connection Failed :" + Remarks);
            //    //string msg = "Connection Failed :" + Remarks;
            //    //Email.SendMail(msg);
            //}

            try
            {
                Boolean isExportEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ExportEnabled"].ToString());
                if (isExportEnabled)
                {
                    TallyExporter.ExportLedgersFromTally(companyId);
                }

                Boolean isExportCostCentreEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["ExportCostCentreEnabled"].ToString());
                if (isExportCostCentreEnabled)
                {
                    TallyExporter.ExportCostCentersFromTally(companyId);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("Program", "Main", ex.Message);
            }

            Boolean isTestMode = Convert.ToBoolean(ConfigurationManager.AppSettings["TestMode"].ToString());
            if (isTestMode)
            {
                string journeyDateStr = ConfigurationManager.AppSettings["JourneyDate"].ToString();
                DateTime journeyDate = Convert.ToDateTime(journeyDateStr);

                Logger.WriteLog("Journey Date is :" + journeyDate.ToString("yyyy-MM-dd"));
                //Post Data to Tally
                BranchPosting.PostBranchData(companyId, journeyDate);
               // BranchPosting.PostBranchData(companyId, IntervalOfDays);

            }
            else
            {
                Logger.WriteLog("Started Job : First Checking Critical Log File Existence and then validation");
                string logpath = ConfigurationManager.AppSettings["LogPath"];
                if (File.Exists(@logpath + "Alertfile" + ".txt"))
                {

                    Logger.WriteLog("Validation Failed : Due to Existence of Critical File");
                    string msg = "Validation Failed :" + "Due to Existence of Critical File";
                    Email.SendMail(msg);
                }
                else
                {
                    List<PostingJob> pendingJobList = PostingAPI.GetPendingPostingJobsList(companyId);
                    Logger.WriteLog("No Of Pending Jobs: " + pendingJobList.Count);
                    if (pendingJobList != null && pendingJobList.Count > 0)
                    {
                        for (int i = 0; i < pendingJobList.Count; i++)
                        {

                            ValidationResult vr = BranchPosting.ValidateData(companyId, pendingJobList[i]);

                            if (vr.Status == 0)
                            {
                                // StatusId - 2 - Validation Failed
                                pendingJobList[i].StatusId = 2;
                                pendingJobList[i].Remarks = vr.ErrorMessage;
                                Logger.WriteLogAlert("Running status to: " + "Validation Failed :" + pendingJobList[i].Remarks);
                                Logger.WriteLog("Updating status to: " + pendingJobList[i].StatusId + " Remarks: " + pendingJobList[i].Remarks);
                                PostingAPI.UpdatePostingStatus(pendingJobList[i]);
                            }
                            else if (vr.Status == 1)
                            {

                                if (!TallyPostingAPI.IsTallyInstanceRunning())
                                {
                                    // Mark status as Cannot connect to Tally
                                    pendingJobList[i].StatusId = 3;
                                    pendingJobList[i].Remarks = "Unable to connect to Tally";
                                    Logger.WriteLog("Updating status to: " + pendingJobList[i].StatusId + " Remarks: " + pendingJobList[i].Remarks);
                                    PostingAPI.UpdatePostingStatus(pendingJobList[i]);
                                }

                                else
                                {
                                    // Mark status as In-Progress
                                    pendingJobList[i].StatusId = 4;
                                    Logger.WriteLog("Updating status to: " + pendingJobList[i].StatusId);
                                    PostingAPI.UpdatePostingStatus(pendingJobList[i]);


                                    BranchPosting.PostBranchData(companyId, pendingJobList[i].JourneyDate);

                                    //Mark status as Completed
                                    pendingJobList[i].StatusId = 5;
                                    Logger.WriteLog("Updating status to: " + pendingJobList[i].StatusId);
                                    PostingAPI.UpdatePostingStatus(pendingJobList[i]);
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
