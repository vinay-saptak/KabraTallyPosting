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
   public class AccountingAPI
    {

        public static List<Journal> GetJournals(int companyId)
        {
            List<Journal> journalList = new List<Journal>();
            try
            {
                journalList = GetJournals(companyId, "spKabraTallyGet_Journals");
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingAPI", "GetJournals", ex.Message);
            }
            return journalList;
        }



        private static List<Journal> GetJournals(int companyId, string spName)
        {
            List<Journal> journalList = new List<Journal>();
            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_companyId", companyId, ParameterDirection.Input);
                DataSet dstOutPut = dal.ExecuteSelect(spName, CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);

                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                                Journal jl = new Journal();
                                jl.Action = dstOutPut.Tables[j].Rows[i]["action"].ToString();
                                jl.AccSysId = dstOutPut.Tables[j].Rows[i]["accsysid"].ToString();
                                jl.AccSysPostDateTime = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["accsyspostdatetime"].ToString());
                                jl.BookingId = Convert.ToInt64(dstOutPut.Tables[j].Rows[i]["bookingid"].ToString());
                                jl.CompanyId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["companyid"].ToString());
                                jl.CurrencyId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["currencyid"].ToString());
                                jl.DocFormattedNumber = dstOutPut.Tables[j].Rows[i]["docformattednumber"].ToString();
                                jl.DocNumber = dstOutPut.Tables[j].Rows[i]["docnumber"].ToString();
                                jl.DocSubtype = dstOutPut.Tables[j].Rows[i]["docsubtype"].ToString();
                                jl.DocType = dstOutPut.Tables[j].Rows[i]["doctype"].ToString();
                                jl.Narration = dstOutPut.Tables[j].Rows[i]["narration"].ToString();
                                jl.JournalDateTime = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["journaldatetime"].ToString());
                                jl.JournalEntryDateTime = Convert.ToDateTime(dstOutPut.Tables[j].Rows[i]["journalentrydatetime"].ToString());
                                jl.JournalId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["voucherjournalid"].ToString());
                                jl.Type = dstOutPut.Tables[j].Rows[i]["type"].ToString();
                                jl.UserId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["userid"].ToString());

                                journalList.Add(jl);
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }

            return journalList;
        }


        public static List<JournalDetail> GetJournalDetail(int JournalID,string type)
        {
            List<JournalDetail> jdList = new List<JournalDetail>();
            try
            {
                string strErr = "";
                CRSDAL dal = new CRSDAL();
                dal.AddParameter("p_JournalId", JournalID, ParameterDirection.Input);
                
                DataSet dstOutPut = dal.ExecuteSelect("spTallyGet_JournalDetail_Kabra", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", false, "", false);
             
                if (dstOutPut != null && dstOutPut.Tables != null && dstOutPut.Tables.Count > 0)
                {
                    for (int j = 0; j < dstOutPut.Tables.Count; j++)
                    {
                        if (dstOutPut.Tables[j] != null && dstOutPut.Tables[j].Rows != null && dstOutPut.Tables[j].Rows.Count > 0)
                        {
                            for (int i = 0; i < dstOutPut.Tables[j].Rows.Count; i++)
                            {
                                JournalDetail jd = new JournalDetail();
                                jd.Amount = Convert.ToDecimal(dstOutPut.Tables[j].Rows[i]["amount"].ToString());
                                jd.Description = dstOutPut.Tables[j].Rows[i]["description"].ToString();
                                jd.LedgerName = dstOutPut.Tables[j].Rows[i]["ledgername"].ToString();
                                jd.ClassName = dstOutPut.Tables[j].Rows[i]["classname"].ToString();
                                jd.IsDebit = Convert.ToInt16(dstOutPut.Tables[j].Rows[i]["isdebit"].ToString());
                                jd.LedgerId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["accsysledgerid"].ToString());
                                jd.ClassId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["accsysclassid"].ToString());
                                jd.JournalId = Convert.ToInt32(dstOutPut.Tables[j].Rows[i]["voucherjournalid"].ToString());

                                jdList.Add(jd);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("AccountingAPI", "GetJournalDetail", ex.Message);
            }
            return jdList;
        }


        public static void UpdateTallyJournalIdInCRS(int journalId, string tallyId)
        {

            string strResult = "";
            try
            {
                CRSDAL dal = new CRSDAL();
                string strErr = "";
                dal.AddParameter("p_journalId", journalId, ParameterDirection.Input);
                dal.AddParameter("p_accsysid", tallyId, 100, ParameterDirection.Input);
                //dal.AddParameter("p_ErrMsg", strErr, strErr.Length, ParameterDirection.Output);
                 DataSet dstoutput = dal.ExecuteSelect("spKabraTallySet_JournalId", CommandType.StoredProcedure, 0, ref strErr, "p_ErrMessage", true, "", false);

                if (Convert.ToInt32(dstoutput.Tables[0].Rows[0]["countrow"].ToString()) == 0 || Convert.ToInt32(dstoutput.Tables[0].Rows[0]["countrow"].ToString()) > 1 || strErr != "")
                {
                    Logger.WriteLogAlert("AccountingAPI " + "Error In UpdateTallyJournalIdInCRS" + "For JournalId :" + journalId + " " + strErr);

                }

            }
            catch (Exception ex)
            {
                Logger.WriteLogAlert("AccountingAPI " + "Error In UpdateTallyJournalIdInCRS" + "For JournalId :" + journalId + " " + ex);

            }
        }

    }
}
