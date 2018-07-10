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
    class BookingsAPI
    {

       

        public static List<Booking> GetBranchBookings(int companyid, DateTime journeyDate)
        {
            List<Booking> bookingsList = new List<Booking>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_journeyDate", journeyDate, ParameterDirection.Input);
                DataSet dstOutPut = dal.ExecuteSelect("spKabraTallyGet_BranchBooking_test", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                       // int table = dstOutPut.Tables.Count;
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                               // int row = dstOutPut.Tables[table - 1].Rows.Count;
                                Booking b = new Booking();

                                b.BranchId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["branchid"].ToString());
                                b.BusID = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["busid"].ToString());
                                b.BranchName = Convert.ToString(dstOutPut.Tables[j].Rows[i]["branchname"].ToString());
                                b.JourneyDate = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["chartdate"].ToString());
                                b.DebitLedgerId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["Debitledgerid"].ToString());
                                b.CreditLedgerId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["Creditledgerid"].ToString());
                                b.TotalAmount = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["TotalFare"].ToString());
                                b.GST = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["GST"].ToString());
                                b.NetAmount = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["NetAmt"].ToString());
                                b.ClassId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["ClassID"].ToString());
                                b.ClassName = Convert.ToString(dstOutPut.Tables[j].Rows[i]["classname"].ToString());
                                b.DocNumber = Convert.ToString(dstOutPut.Tables[j].Rows[i]["docnumber"].ToString());
                                //b.NetAmount = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["NetAmt"].ToString());
                                bookingsList.Add(b);
                            }
                        }
                    }

                }
            
            }
            catch (Exception ex)
            {

                Logger.WriteLog("BookingsAPI", "GetBranchBookings", ex.Message);
            }
            return bookingsList;


        }





        public static void CreateJournalForUserWiseCollection(int companyId,List<Booking> bookingsList)
        {
            try
            {
                CRSDAL dal = new CRSDAL();
                string strErr = "";

                
               
                for (int i = 0; i < bookingsList.Count; i++)
                {
                    Booking b = bookingsList[i];
                    if (strErr == "")
                    {
                        try
                        {
                            dal = new CRSDAL();
                            dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                            dal.AddParameter("p_JourneyDate", bookingsList[i].JourneyDate, ParameterDirection.Input);
                            dal.AddParameter("p_DebitLedgerId", bookingsList[i].DebitLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_CreditLedgerId", bookingsList[i].CreditLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_ClassID", bookingsList[i].ClassId, ParameterDirection.Input);
                            dal.AddParameter("p_TotalAmount", bookingsList[i].TotalAmount, ParameterDirection.Input);
                            dal.AddParameter("p_NetAmount", bookingsList[i].NetAmount, ParameterDirection.Input);
                            dal.AddParameter("p_GST", bookingsList[i].GST, ParameterDirection.Input);
                            dal.AddParameter("p_docnumber", bookingsList[i].DocNumber, 300, ParameterDirection.Input);

                            int status = dal.ExecuteDML("spKabraTallySet_BranchCollection_Test", CommandType.StoredProcedure, 0, ref strErr);
                            EntryCounter.GetInstance().AddCount(1);
                            if (strErr != "")
                            {
                                throw new Exception();
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("BookingAPI", "CreateJournalForUserWiseCollection", " Error for Branch: " + bookingsList[i].BranchName + " " + ex.Message);
                            Logger.WriteLogAlert2("BookingAPI " + "Error:CreateJournalForUserWiseCollection For JournalID: " + bookingsList[i].BranchName + " " + ex.Message);
                            PostingAPI.UpdatePostingStatusForException(companyId, bookingsList[i].JourneyDate, 9);

                        }
                    }


                }
              
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BookingAPI", "CreateJournalForUserWiseCollection", ex.Message);
                Logger.WriteLogAlert("BookingAPI" + "CreateJournalForUserWiseCollection" + ex.Message);

            }
        }




       



    }
}
