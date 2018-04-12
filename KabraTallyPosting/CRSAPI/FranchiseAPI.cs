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
        public static List<Franchise> GetFranchiseCollection(int companyid,int intervalofdays)
        {
            List<Franchise> franchisecollectionList = new List<Franchise>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_interval", intervalofdays, ParameterDirection.Input);
                DataSet dsOutPut = dal.ExecuteSelect("spJainTallyGet_FranchiseCollection", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

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
                               // fc.FranchiseName = dr["BranchName"].ToString();
                                fc.Bookingdate =Convert.ToDateTime(dr["BookingDate"].ToString());
                                 fc.DebitLedgerId = Convert.ToInt32(dr["DebitLedgerId"].ToString());
                                 fc.CreditLedgerId = Convert.ToInt32(dr["CreditLedgerId"].ToString());
                                fc.FranchiseName = dr["DebitLedgerName"].ToString();
                                fc.NetAmount = Convert.ToDecimal(dr["NetAmt"].ToString());
                                fc.TotalAmt = Convert.ToDecimal(dr["TotalAmt"].ToString());
                                 fc.ServiceTax = Convert.ToDecimal(dr["STax"].ToString());
                               fc.DocNumber = Convert.ToString(dr["docnumber"].ToString());
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

                   
                    Franchise fv = FranchiseCollectionList[i];
                    string narration = AccountingUtil.GetNarration(fv);
                    try
                    {
                        dal = new CRSDAL();
                        dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_BookingDate", FranchiseCollectionList[i].Bookingdate, ParameterDirection.Input);
                        dal.AddParameter("p_DebitLedgerId", FranchiseCollectionList[i].DebitLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_CreditLedgerId", FranchiseCollectionList[i].CreditLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_NetAmount", FranchiseCollectionList[i].NetAmount, ParameterDirection.Input);
                        dal.AddParameter("p_TotalAmount", FranchiseCollectionList[i].TotalAmt, ParameterDirection.Input);
                        dal.AddParameter("p_ServiceTax", FranchiseCollectionList[i].ServiceTax, ParameterDirection.Input);
                        dal.AddParameter("p_narration", narration,3000, ParameterDirection.Input);
                        dal.AddParameter("p_docnumber", FranchiseCollectionList[i].DocNumber, 300, ParameterDirection.Input);

                        int status = dal.ExecuteDML("spTallySet_FranchiseCollectionJain", CommandType.StoredProcedure, 0, ref strErr);

                        
                        EntryCounter.GetInstance().AddCount(1);
                       
                       
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("FranchiseCollection", "CreateSaleForFranchiseCollection", " Error for Branch: " + FranchiseCollectionList[i].FranchiseId + " " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("FranchiseCollection", "CreateSaleForFranchiseCollection", ex.Message);
            }
        }
    }
}
