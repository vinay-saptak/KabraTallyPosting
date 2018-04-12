using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.Util;
using System.Data;

namespace KabraTallyPosting.CRSAPI
{
   public class CancellationAPI
    {

        public static List<Cancellation> GetCancellation(int companyid,int intervalofdays)
        {
            List<Cancellation> CancellationList = new List<Cancellation>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_interval", intervalofdays, ParameterDirection.Input);
                DataSet dsOutPut = dal.ExecuteSelect("spJainTallyGet_CancellationData", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dsOutPut != null && dsOutPut.Tables != null && dsOutPut.Tables.Count > 0)
                {
                    DataTable dtOutput = dsOutPut.Tables[0];
                    if (dtOutput != null && dtOutput.Rows != null && dtOutput.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtOutput.Rows.Count; i++)
                        {
                            DataRow dr = dtOutput.Rows[i];
                            if (Convert.ToInt32(dr["DebitLedgerId"].ToString()) > 0 && Convert.ToInt32(dr["CreditLedgerId"].ToString()) > 0)
                            {
                                Cancellation c = new Cancellation();

                                c.BookingId = Convert.ToInt32(dr["bookingid"].ToString());
                                c.DebitLedgerId = Convert.ToInt32(dr["DebitLedgerId"].ToString());
                                c.CreditLedgerId = Convert.ToInt32(dr["CreditLedgerId"].ToString());
                                c.BaseFare = Convert.ToDecimal(dr["ActualTotalFare"].ToString());
                                c.GST = Convert.ToDecimal(dr["GST"].ToString());
                                c.IGST = Convert.ToDecimal(dr["IGST"].ToString());
                                c.RefundAmount = Convert.ToDecimal(dr["RefundAmt"].ToString());
                                c.Comm = Convert.ToDecimal(dr["AgentComm"].ToString());
                                c.CancelDate = Convert.ToDateTime(dr["CDate"].ToString());
                                c.CustomerName = Convert.ToString(dr["CustomerName"].ToString());
                                c.GSTNumber = Convert.ToString(dr["GSTNumber"].ToString());
                                c.CustomerStateName = Convert.ToString(dr["passengerstatename"].ToString());
                                c.TicketNo = Convert.ToString(dr["TicketNo"].ToString());
                                c.Docnumber = Convert.ToString(dr["docnumber"].ToString());
                                c.DocType = Convert.ToString(dr["doctype"].ToString());
                                CancellationList.Add(c);
                            }
                           

                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Logger.WriteLog("CancellationAPI", "GetCancellation", ex.Message);
               // Logger.WriteLogAlert("Cancellation " + " Error for Bookigid: " + CancellationList[i].BookingId + " " + ex.Message);
            }
            return CancellationList;


        }



        public static void CreateCreditForCancellationdata(int companyId, List<Cancellation> CancellationList)
        {
            CRSDAL dal = null;
            try
            {

                string strErr = "";
               

                for (int i = 0; i < CancellationList.Count; i++)
                {
                    if (CancellationList[i].DebitLedgerId > 0 && CancellationList[i].CreditLedgerId > 0)
                    {

                        Cancellation c = CancellationList[i];

                        string description = AccountingUtil.GetDescription(c);
                       
                        try
                        {
                            dal = new CRSDAL();
                            dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                            dal.AddParameter("p_CancelDate", CancellationList[i].CancelDate, ParameterDirection.Input);
                            dal.AddParameter("p_DebitLedgerId", CancellationList[i].DebitLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_CreditLedgerId", CancellationList[i].CreditLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_RefundAmt", CancellationList[i].RefundAmount, ParameterDirection.Input);
                            dal.AddParameter("p_TotalAmount", CancellationList[i].BaseFare, ParameterDirection.Input);
                            dal.AddParameter("p_GST", CancellationList[i].GST, ParameterDirection.Input);
                            dal.AddParameter("p_IGST", CancellationList[i].IGST, ParameterDirection.Input);
                            dal.AddParameter("p_Comm", CancellationList[i].Comm, ParameterDirection.Input);
                            dal.AddParameter("p_BookingId", CancellationList[i].BookingId, ParameterDirection.Input);
                            dal.AddParameter("p_TicketNo", CancellationList[i].TicketNo, 3000, ParameterDirection.Input);
                            dal.AddParameter("p_description", description, 3000, ParameterDirection.Input);
                            dal.AddParameter("p_docnumber", CancellationList[i].Docnumber, 3000, ParameterDirection.Input);
                            dal.AddParameter("p_doctype", CancellationList[i].DocType, 3000, ParameterDirection.Input);

                            int status = dal.ExecuteDML("spTallySet_CancellationJain", CommandType.StoredProcedure, 0, ref strErr);
                            EntryCounter.GetInstance().AddCount(1);
                            //if (strErr != "")
                            //{
                            //    Logger.WriteLogAlert("Cancellation " + " Error for Bookigid: " + CancellationList[i].BookingId );
                            //    break;
                            //}

                        }



                        catch (Exception ex)
                        {
                            Logger.WriteLog("Cancellation", "CreateCreditForCancellationdata", " Error for Bookigid: " + CancellationList[i].BookingId + " " + ex.Message);
                            Logger.WriteLogAlert("Cancellation " + " Error for Bookigid: " + CancellationList[i].BookingId + " " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Cancellation", "CreateCreditForCancellationdata", ex.Message);
            }
        }

    }


}
