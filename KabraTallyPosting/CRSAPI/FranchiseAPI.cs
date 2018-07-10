using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using KabraTallyPosting.Entity;
using KabraTallyPosting.Util;


namespace KabraTallyPosting.CRSAPI
{
    public class FranchiseAPI
    {
        public static List<Franchise> GetFranchiseCollection(int companyid, DateTime journeyDate)
        {
            List<Franchise> franchisecollectionList = new List<Franchise>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_journeyDate", journeyDate, ParameterDirection.Input);
                DataSet dsOutPut = dal.ExecuteSelect("spKabraTallyGet_FranchiseBooking_test", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dsOutPut != null && dsOutPut.Tables != null && dsOutPut.Tables.Count > 0)
                {
                    DataTable dtOutput = dsOutPut.Tables[0];
                    if (dtOutput != null && dtOutput.Rows != null && dtOutput.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtOutput.Rows.Count; i++)
                        {
                            DataRow dr = dtOutput.Rows[i];
                            Franchise fc = new Franchise();
                            fc.FranchiseId = Convert.ToInt32(dr["BranchID"].ToString());
                            fc.FranchiseName = dr["BranchName"].ToString();
                            fc.JourneyDate = Convert.ToDateTime(dr["Journeydate"].ToString());
                            fc.DebitLedgerId = Convert.ToInt32(dr["DebitLedgerId"].ToString());
                            fc.CreditLedgerId = Convert.ToInt32(dr["CreditLedgerId"].ToString());
                            //fc.FranchiseName = dr["DebitLedgerName"].ToString();
                            fc.NetAmount = Convert.ToDecimal(dr["NetAmt"].ToString());
                            fc.TotalAmt = Convert.ToDecimal(dr["TotalFare"].ToString());
                            fc.ServiceTax = Convert.ToDecimal(dr["GST"].ToString());
                            fc.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                            fc.AggentComm = Convert.ToDecimal(dr["Commission"].ToString());
                            fc.DocNumber = Convert.ToString(dr["docnumber"].ToString());
                            fc.ClassID = Convert.ToInt32(dr["ClassID"].ToString());
                            fc.classname = Convert.ToString(dr["classname"].ToString());
                            franchisecollectionList.Add(fc);
                            
                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Logger.WriteLog("FranchiseAPI", "GetFranchiseCollection", ex.Message);
            }
            return franchisecollectionList;


        }


        public static void CreateSaleForFranchiseCollection(int companyId, List<Franchise> FranchiseCollectionList)
        {
            try
            {
                CRSDAL dal = null;
                string strErr = "";

                for (int i = 0; i < FranchiseCollectionList.Count; i++)
                {
                    if (strErr == "")
                    {
                        Franchise fv = FranchiseCollectionList[i];
                        string narration = AccountingUtil.GetNarration(fv);
                        try
                        {
                            dal = new CRSDAL();
                            dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                            dal.AddParameter("p_JourneyDate", FranchiseCollectionList[i].JourneyDate, ParameterDirection.Input);
                            dal.AddParameter("p_DebitLedgerId", FranchiseCollectionList[i].DebitLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_CreditLedgerId", FranchiseCollectionList[i].CreditLedgerId, ParameterDirection.Input);
                            dal.AddParameter("p_NetAmount", FranchiseCollectionList[i].NetAmount, ParameterDirection.Input);
                            dal.AddParameter("p_TotalAmount", FranchiseCollectionList[i].TotalAmt, ParameterDirection.Input);
                            dal.AddParameter("p_GST", FranchiseCollectionList[i].ServiceTax, ParameterDirection.Input);
                            dal.AddParameter("p_docnumber", FranchiseCollectionList[i].DocNumber, 300, ParameterDirection.Input);
                            dal.AddParameter("p_ClassID", FranchiseCollectionList[i].ClassID, ParameterDirection.Input);
                            dal.AddParameter("p_AgentComm", FranchiseCollectionList[i].AggentComm, ParameterDirection.Input);
                            dal.AddParameter("p_Discount", FranchiseCollectionList[i].Discount, ParameterDirection.Input);

                            int status = dal.ExecuteDML("spKabraTallySet_FranchiseBooking_Test", CommandType.StoredProcedure, 0, ref strErr);
                            if (strErr != "")
                            {
                                throw new Exception();
                            }

                            EntryCounter.GetInstance().AddCount(1);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLog("FranchiseCollection", "CreateSaleForFranchiseCollection", " Error for Franchise: " + FranchiseCollectionList[i].FranchiseName + " " + ex.Message);
                            Logger.WriteLogAlert2("FranchiseCollection" + "CreateSaleForFranchiseCollection" + " Error for Franchise: " + FranchiseCollectionList[i].FranchiseName + " " + ex.Message);
                            PostingAPI.UpdatePostingStatusForException(companyId, FranchiseCollectionList[i].JourneyDate, 9);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("FranchiseCollection", "CreateSaleForFranchiseCollection", ex.Message);
                Logger.WriteLogAlert("FranchiseCollection" + "CreateSaleForFranchiseCollection" + ex.Message);
            }
        }
    }
}
