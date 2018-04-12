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
   public class AttachedBusAPI
    {
        public static List<BusWise> GetAttachedBusCollection(int companyid,int intervalofdays)
        {
            List<BusWise> BusWiseList = new List<BusWise>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_interval", intervalofdays, ParameterDirection.Input);
                DataSet dsOutPut = dal.ExecuteSelect("spJainTallyGet_QuickView", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dsOutPut != null && dsOutPut.Tables != null && dsOutPut.Tables.Count > 0)
                {
                    DataTable dtOutput = dsOutPut.Tables[0];
                    if (dtOutput != null && dtOutput.Rows != null && dtOutput.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtOutput.Rows.Count; i++)
                        {
                            DataRow dr = dtOutput.Rows[i];
                            BusWise bc = new BusWise();
                            bc.BusiD = Convert.ToInt32(dr["BusID"].ToString());
                            bc.BusNumber = dr["BusNumber"].ToString();
                            bc.JourneyDate = Convert.ToDateTime(dr["JourneyDate"].ToString());
                            bc.AccsysLedgerId = Convert.ToInt32(dr["accsysledgerid"].ToString());
                            bc.CommLedgerId = Convert.ToInt32(dr["CommLedgerid"].ToString());
                            bc.NetAmount = Convert.ToDecimal(dr["totalactualfare"].ToString());
                            bc.OfflineAgentComm = Convert.ToDecimal(dr["TotalOfflineAgentComm"].ToString());
                            bc.OnlineAgentComm = Convert.ToDecimal(dr["TotalOnlineAgentComm"].ToString());
                            bc.CGST = Convert.ToDecimal(dr["CGST"].ToString());
                            bc.SGST = Convert.ToDecimal(dr["CGST"].ToString());
                            bc.BusComm = Convert.ToDecimal(dr["Comm"].ToString());
                            bc.DocNumber = Convert.ToString(dr["docnumber"].ToString());
                            BusWiseList.Add(bc);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Logger.WriteLog("AttachedBusAPI", "GetAttachedBusCollection", ex.Message);
            }
            return BusWiseList;


        }




        public static void CreateSaleForBusWiseCollection(int companyId, List<BusWise> BusWiseCollectionList)
        {
            try
            {
                CRSDAL dal = null;
                string strErr = "";

                for (int i = 0; i < BusWiseCollectionList.Count; i++)
                {
                   

                    //BusWise bc = BusWiseCollectionList[i];
                    //string narration = AccountingUtil.GetNarration(bc);

                    decimal totalamount = 0;
                    decimal totalColl = 0;


                    totalamount = BusWiseCollectionList[i].NetAmount - (BusWiseCollectionList[i].SGST + BusWiseCollectionList[i].CGST + BusWiseCollectionList[i].BusComm);
                    totalColl = totalamount + BusWiseCollectionList[i].SGST + BusWiseCollectionList[i].CGST + BusWiseCollectionList[i].BusComm + BusWiseCollectionList[i].OfflineAgentComm +  BusWiseCollectionList[i].OnlineAgentComm;


                    try
                    {
                        dal = new CRSDAL();
                        dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_JourneyDate", BusWiseCollectionList[i].JourneyDate, ParameterDirection.Input);
                        dal.AddParameter("p_AccsysledgerId", BusWiseCollectionList[i].AccsysLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_CommLedgerId", BusWiseCollectionList[i].CommLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_NetAmount", totalamount, ParameterDirection.Input);
                        dal.AddParameter("p_TotalAmount", totalColl, ParameterDirection.Input);
                        dal.AddParameter("p_Comm", BusWiseCollectionList[i].BusComm, ParameterDirection.Input);
                        dal.AddParameter("p_GST", BusWiseCollectionList[i].CGST, ParameterDirection.Input);
                        dal.AddParameter("p_TotalOfflineAgentComm", BusWiseCollectionList[i].OfflineAgentComm, ParameterDirection.Input);
                        dal.AddParameter("p_TotalOnlineAgentComm", BusWiseCollectionList[i].OnlineAgentComm, ParameterDirection.Input);
                        dal.AddParameter("p_docnumber", BusWiseCollectionList[i].DocNumber,1000, ParameterDirection.Input);

                        // dal.AddParameter("p_narration", narration, 3000, ParameterDirection.Input);

                       int status = dal.ExecuteDML("spJainTallySet_BusCollection", CommandType.StoredProcedure, 0, ref strErr);

                       
                        EntryCounter.GetInstance().AddCount(1);
                      
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("BusWiseCollection", "CreateSaleForBusWiseCollection", " Error for Bus: " + BusWiseCollectionList[i].BusiD + " " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("BusWiseCollection", "CreateSaleForBusWiseCollection", ex.Message);
            }
        }

    }
}
