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
   public class GSTBookingAPI
    {

        public static List<GSTBooking> GetBranchGSTBookings(int companyid,int intervalofdays)
        {
            List<GSTBooking> gstbookingsList = new List<GSTBooking>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_interval", intervalofdays, ParameterDirection.Input);
                DataSet dstOutPut = dal.ExecuteSelect("spJainTallyGet_GSTBookingDetails", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                                GSTBooking gb = new GSTBooking();

                                gb.BaseFare = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["TotalFare"].ToString());
                                gb.TotalFare = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["ActualTotalFare"].ToString());
                                gb.NetFare = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["NetTotalFare"].ToString());
                                gb.BookingId = Convert.ToInt64(dstOutPut.Tables[j].Rows[i]["BookingID"].ToString());
                                gb.GSTNumber = Convert.ToString(dstOutPut.Tables[0].Rows[0]["GSTNumber"].ToString());
                                gb.DebitLedgerId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["DebitLedgerid"].ToString());
                                gb.CreditLedgerId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["CreditLedgerId"].ToString());
                                gb.GST = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["GST"].ToString());
                                gb.IGST = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["IGST"].ToString());
                                gb.Comm = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["AgentComm"].ToString());
                                gb.CustomerName = Convert.ToString(dstOutPut.Tables[0].Rows[0]["CustomerName"].ToString());
                                gb.CustomerStateName = Convert.ToString(dstOutPut.Tables[0].Rows[0]["passengerstatename"].ToString());
                                gb.TicketNo = Convert.ToString(dstOutPut.Tables[j].Rows[i]["TicketNo"].ToString());
                                gb.DocNumber = Convert.ToString(dstOutPut.Tables[j].Rows[i]["docnumber"].ToString());
                                gb.DocType = Convert.ToString(dstOutPut.Tables[j].Rows[i]["doctype"].ToString());
                                gstbookingsList.Add(gb);
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Logger.WriteLog("BookingsAPI", "GetBranchCardBookings", ex.Message);
            }
            return gstbookingsList;


        }

        public static void CreateSaleForGSTBoookingCollection(int companyId, List<GSTBooking> gstbookingsList)
        {
            try
            {
                CRSDAL dal = null;
                string strErr = "";

                decimal roundoff = 0;


                for (int i = 0; i < gstbookingsList.Count; i++)
                {
                    dal = new CRSDAL();
                    GSTBooking gb = gstbookingsList[i];
                    string narration = AccountingUtil.GetNarration(gb);
                    string description = AccountingUtil.GetDescription(gb);
                    roundoff = gstbookingsList[i].TotalFare - gstbookingsList[i].NetFare;

                    dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                    dal.AddParameter("p_BookingDate", gstbookingsList[i].BookingDate, ParameterDirection.Input);
                    dal.AddParameter("p_debitledgerid", gstbookingsList[i].DebitLedgerId, ParameterDirection.Input);
                    dal.AddParameter("p_creditledgerid", gstbookingsList[i].CreditLedgerId, ParameterDirection.Input);
                    dal.AddParameter("p_basefare", gstbookingsList[i].BaseFare, ParameterDirection.Input);
                    dal.AddParameter("p_totalfare", gstbookingsList[i].NetFare, ParameterDirection.Input);
                    dal.AddParameter("p_GST", gstbookingsList[i].GST, ParameterDirection.Input);
                    dal.AddParameter("p_IGST", gstbookingsList[i].IGST, ParameterDirection.Input);
                    dal.AddParameter("p_comm", gstbookingsList[i].Comm, ParameterDirection.Input);
                    dal.AddParameter("p_narration", narration, 3000, ParameterDirection.Input);
                    dal.AddParameter("p_description", description, 3000, ParameterDirection.Input);
                    dal.AddParameter("p_docnumber", gstbookingsList[i].DocNumber, 3000, ParameterDirection.Input);
                    dal.AddParameter("p_doctype", gstbookingsList[i].DocType, 3000, ParameterDirection.Input);
                    dal.AddParameter("p_roundoff", roundoff, ParameterDirection.Input);

                    int status = dal.ExecuteDML("spTallySet_BranchGSTBooking", CommandType.StoredProcedure, 0, ref strErr);

                    EntryCounter.GetInstance().AddCount(1);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("UserWise", "CreateSaleForUserWiseCollection", ex.Message);
            }
        }
    }
}
