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
   public class OnlineAgentAPI
    {
        public static List<OnlineAgentDetails> GetOnlineAgentCollection(int companyid, DateTime journeyDate)
        {
            List<OnlineAgentDetails> AgentList = new List<OnlineAgentDetails>();

            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyid, ParameterDirection.Input);
                dal.AddParameter("p_journeyDate", journeyDate, ParameterDirection.Input);
                DataSet dsOutPut = dal.ExecuteSelect("spKabraTallyGet_OnlineAgentCollection", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dsOutPut != null && dsOutPut.Tables != null && dsOutPut.Tables.Count > 0)
                {
                    DataTable dtOutput = dsOutPut.Tables[0];
                    if (dtOutput != null && dtOutput.Rows != null && dtOutput.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtOutput.Rows.Count; i++)
                        {
                            DataRow dr = dtOutput.Rows[i];
                            OnlineAgentDetails ac = new OnlineAgentDetails();

                            ac.AgentName = dr["AgentName"].ToString();
                            ac.AgentId = Convert.ToInt32(dr["agentid"].ToString());
                            ac.BusId = Convert.ToInt32(dr["busid"].ToString());
                            ac.DebitLedgerId = Convert.ToInt32(dr["DebitLedgerId"].ToString());
                            ac.CreditLedgerId = Convert.ToInt32(dr["CreditLedgerId"].ToString());
                            ac.NetAmount = Convert.ToDecimal(dr["NetAmt"].ToString());
                            ac.GST = Convert.ToDecimal(dr["GST"].ToString());
                            ac.Discount = Convert.ToDecimal(dr["Discount"].ToString());
                            ac.TotalFare = Convert.ToDecimal(dr["TotalFare"].ToString());
                            ac.AgentComm = Convert.ToDecimal(dr["AgentComm"].ToString());
                            ac.JourneyDate = Convert.ToDateTime(dr["journeydate"].ToString());
                            ac.Docnumber = Convert.ToString(dr["docnumber"].ToString());
                            ac.ClassName = dr["classname"].ToString();
                            ac.ClassID = Convert.ToInt32(dr["ClassID"].ToString());
                            AgentList.Add(ac);

                        }
                    }

                }

            }
            catch (Exception ex)
            {

                Logger.WriteLog("OnlineAgentAPI", "GetOnlineAgentCollection", ex.Message);
            }
            return AgentList;


        }


        public static void CreateJournalForOnlineAgentCollection(int companyId, List<OnlineAgentDetails> agentlist)
        {
            try
            {
                CRSDAL dal = null;
                string strErr = "";


                for (int i = 0; i < agentlist.Count; i++)
                {


                    OnlineAgentDetails al = agentlist[i];
                   // string narration = AccountingUtil.GetNarration(al);
                    try
                    {
                        dal = new CRSDAL();
                        dal.AddParameter("p_CompanyID", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_JourneyDate", agentlist[i].JourneyDate, ParameterDirection.Input);
                        dal.AddParameter("p_DebitLedgerId", agentlist[i].DebitLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_CreditLedgerId", agentlist[i].CreditLedgerId, ParameterDirection.Input);
                        dal.AddParameter("p_ClassID", agentlist[i].ClassID, ParameterDirection.Input);
                        dal.AddParameter("p_TotalAmount", agentlist[i].TotalFare, ParameterDirection.Input);
                        dal.AddParameter("p_docnumber", agentlist[i].Docnumber, 300, ParameterDirection.Input);
                        dal.AddParameter("p_AgentComm", agentlist[i].AgentComm, ParameterDirection.Input);
                         dal.AddParameter("p_NetAmount", agentlist[i].NetAmount, ParameterDirection.Input);
                        dal.AddParameter("p_GST", agentlist[i].GST, ParameterDirection.Input);
                        dal.AddParameter("p_Discount", agentlist[i].Discount,  ParameterDirection.Input);


                        int status = dal.ExecuteDML("spKabraTallySet_OnlineAgentCollection", CommandType.StoredProcedure, 0, ref strErr);


                        EntryCounter.GetInstance().AddCount(1);

                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLog("OnlineAgentAPI", "CreateJournalForOnlineAgentCollection", " Error for Agent: " + agentlist[i].AgentName + " " + ex.Message);
                        Logger.WriteLogAlert("OnlineAgentAPI" + "CreateJournalForOnlineAgentCollection" + " Error for Agent: " + agentlist[i].AgentName + " " + ex.Message);
                        PostingAPI.UpdatePostingStatusForException(companyId, agentlist[i].JourneyDate, 7);


                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("OnlineAgentAPI", "CreateJournalForOnlineAgentCollection", ex.Message);
                Logger.WriteLogAlert("OnlineAgentAPI" + "CreateJournalForOnlineAgentCollection" + ex.Message);

            }
        }

    }
}
