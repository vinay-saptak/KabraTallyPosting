using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.TallyAPI;
using System.Data;
using KabraTallyPosting.Util;

namespace KabraTallyPosting.CRSAPI
{
    public class PostingAPI
    {

        public static void InsertPostingStatus(int companyId, List<PostingJob>postinglist)
        {
            try
            {
                CRSDAL dal = new CRSDAL();
                string strErr = "";
               // DateTime BookingDate = Convert.ToDateTime(BookingDateTo);

                for (int i = 0; i < postinglist.Count; i++)
                {
                    PostingJob p = postinglist[i];

                    try
                    {
                        dal = new CRSDAL();
                        dal.AddParameter("p_CompanyId", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_JourneyDate", postinglist[i].JourneyDate, ParameterDirection.Input);
                        dal.AddParameter("p_JobId", postinglist[i].JobId, ParameterDirection.Input);
                        dal.AddParameter("p_StatusId", postinglist[i].StatusId, ParameterDirection.Input);
                        dal.AddParameter("p_Remarks", postinglist[i].Remarks,200, ParameterDirection.Input);
                        dal.AddParameter("p_UserId", 0, ParameterDirection.Input);
                        dal.AddParameter("p_ErrMsg", "", 100, ParameterDirection.Output);


                        int status = dal.ExecuteDML("spTallySet_PostingStatus", CommandType.StoredProcedure, 0, ref strErr);

                        if (strErr != "")
                        {
                            Logger.WriteLog("", "", "Error while upating posting status for CompanyId: " + companyId + "JourneyDate: " + postinglist[i].JourneyDate + " JobId: " + postinglist[i].JobId + " StatusId: " + postinglist[i].StatusId + " Error Is: " + strErr);
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("BookingAPI", "InsertPostingStatus", " Error for JourneyDate: " + postinglist[i].JourneyDate + " " + ex.Message);
                        Logger.WriteLogAlert("BookingAPI " + "Error:InsertPostingStatus For JourneyDate: " + postinglist[i].JourneyDate + " " + ex.Message);
                    }


                }

            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostingStatusAPI", "InsertPostingStatus", ex.Message);
            }
        }

        public static void UpdatePostingStatus(PostingJob pJob)
        {
            try
            {
                CRSDAL dal = new CRSDAL();
                string strErr = "";
                dal.AddParameter("p_CompanyId", pJob.CompanyId, ParameterDirection.Input);
                dal.AddParameter("p_JourneyDate", pJob.JourneyDate, ParameterDirection.Input);
                dal.AddParameter("p_JobId", pJob.JobId, ParameterDirection.Input);
                dal.AddParameter("p_StatusId", pJob.StatusId, ParameterDirection.Input);
                dal.AddParameter("p_Remarks", pJob.Remarks, 200, ParameterDirection.Input);
                dal.AddParameter("p_UserId", 0, ParameterDirection.Input);
                dal.AddParameter("p_ErrMsg", "", 100, ParameterDirection.Output);
                int status = dal.ExecuteDML("spTallySet_PostingStatus", CommandType.StoredProcedure, 0, ref strErr);
                if (strErr != "")
                {
                    Logger.WriteLog("", "", "Error while upating posting status for CompanyId: " + pJob.CompanyId + "JourneyDate: " + pJob.JourneyDate.ToString("yyyy-MM-dd") + " JobId: " + pJob.JobId + " StatusId: " + pJob.StatusId + " Error Is: " + strErr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostingStatusAPI", "UpdatePostingStatus", ex.Message);
            }
        }

        public static List<PostingJob> GetPendingPostingJobsForInputDate(int companyId, int intervalofdays)
        {
            List<PostingJob> pendingJobList = new List<PostingJob>();
            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                dal.AddParameter("p_interval", intervalofdays, ParameterDirection.Input);
                DataSet dstOutPut = dal.ExecuteSelect("spJainTallyGet_PostingStatus", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);
                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                                PostingJob pJob = new PostingJob();
                                pJob.CompanyId = companyId;
                               // pJob.JobId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["jobid"].ToString());
                               pJob.JourneyDate = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["chartdate"].ToString());
                                pJob.StatusId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["StatusId"].ToString());
                                pendingJobList.Add(pJob);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostingStatusAPI", "GetPendingPostingJobs", ex.Message);
            }
            return pendingJobList;
        }

        //public static List<PostingJob> GetPendingPostingJobs(int companyId)
        //{
        //    List<PostingJob> pendingJobList = new List<PostingJob>();
        //    try
        //    {
        //        string strErr = "";
        //        CRSDAL dal = new CRSDAL();
        //        dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
        //        dal.AddParameter("p_StatusId", 1, ParameterDirection.Input);
        //        DataSet dstOutPut = dal.ExecuteSelect("spJainTallyGet_PostingStatus", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);
        //        if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
        //        {
        //            for (int j = 0; j < dstOutPut.Tables.Count; j++)
        //            {
        //                if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
        //                    {
        //                        PostingJob pJob = new PostingJob();
        //                        pJob.CompanyId = companyId;
        //                        pJob.JobId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["jobid"].ToString());
        //                        pJob.JourneyDate = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["journeydate"].ToString());
        //                        pendingJobList.Add(pJob);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog("PostingStatusAPI", "GetPendingPostingJobs", ex.Message);
        //    }
        //    return pendingJobList;
        //}

        public static List<PostingJob> GetPendingPostingJobsList(int companyId)
        {
            List<PostingJob> pendingJobList = new List<PostingJob>();
            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                dal.AddParameter("p_StatusId", 1, ParameterDirection.Input);
                DataSet dstOutPut = dal.ExecuteSelect("spTallyGet_PostingStatus_20180130", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);
                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                                PostingJob pJob = new PostingJob();
                                pJob.CompanyId = companyId;
                                pJob.JobId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["jobid"].ToString());
                                pJob.JourneyDate = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["journeydate"].ToString());
                                pendingJobList.Add(pJob);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostingStatusAPI", "GetPendingPostingJobs", ex.Message);
            }
            return pendingJobList;
        }

        public static void UpdatePostingStatusForException( int companyid ,DateTime journeydate,int statusid)
        {
            try
            {
                CRSDAL dal = new CRSDAL();
                string strErr = "";
                dal.AddParameter("p_CompanyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_JourneyDate", journeydate, ParameterDirection.Input);
                dal.AddParameter("p_JobId", 1, ParameterDirection.Input);
                dal.AddParameter("p_StatusId", statusid, ParameterDirection.Input);
                dal.AddParameter("p_Remarks", "Exception Occured In Creation", 200, ParameterDirection.Input);
                dal.AddParameter("p_UserId", 0, ParameterDirection.Input);
                dal.AddParameter("p_ErrMsg", "", 100, ParameterDirection.Output);
                int status = dal.ExecuteDML("spTallySet_PostingStatus", CommandType.StoredProcedure, 0, ref strErr);
                if (strErr != "")
                {
                    Logger.WriteLog("", "", "Error while upating posting status for CompanyId: " + companyid + "JourneyDate: " + journeydate.ToString("yyyy-MM-dd") +  " Error Is: " + strErr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("PostingStatusAPI", "UpdatePostingStatus", ex.Message);
            }
        }



    }
}
