using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.TallyAPI;
using KabraTallyPosting.Util;
using KabraTallyPosting.CRSAPI;
using System.Data;

namespace KabraTallyPosting.CRSAPI
{
    public class MastersAPI
    {
        public static void InsertUpdateLedgersInCRS(List<Ledger> ledgerList, int companyId)
        {
            try
            {
                if (ledgerList != null && ledgerList.Count > 0)
                {
                    for (int i = 0; i < ledgerList.Count; i++)
                    {
                        Logger.WriteLog("Ledger Name: " + ledgerList[i].LedgerName);
                        CRSDAL dal = new CRSDAL();
                        string strErr = "";
                        dal.AddParameter("p_ledgerid", 0, ParameterDirection.Input);
                        dal.AddParameter("p_ledgername", ledgerList[i].LedgerName, 200, ParameterDirection.Input);
                        dal.AddParameter("p_LedgerTypeId", 0, ParameterDirection.Input);
                        dal.AddParameter("p_LedgerSubTypeId", 0, ParameterDirection.Input);
                        dal.AddParameter("p_CompanyId", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_accsysid", ledgerList[i].LedgerMasterID, 50, ParameterDirection.Input);
                        dal.AddParameter("p_parentname", ledgerList[i].LedgerParentName, 200, ParameterDirection.Input);

                        int status = dal.ExecuteDML("spTallySet_Ledger_20171003_utkarsh", CommandType.StoredProcedure, 0, ref strErr);
                        if (strErr != "")
                        {
                            Logger.WriteLog("MastersAPI", "NoInsertUpdateLedgersInCRS", ledgerList[i].LedgerName);

                        }
                        else
                        {
                            //Logger.WriteLog("MastersAPI", "InsertUpdateLedgersInCRS", ledgerList[i].LedgerName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MastersAPI", "InsertUpdateLedgersInCRS", ex.Message);
            }
        }

        public static void InsertUpdateCostCentresInCRS(List<Ledger> CostcenterList, int companyId)
        {
            try
            {
                if (CostcenterList != null && CostcenterList.Count > 0)
                {
                    for (int i = 0; i < CostcenterList.Count; i++)
                    {
                        Logger.WriteLog("Ledger Name: " + CostcenterList[i].LedgerName);
                        CRSDAL dal = new CRSDAL();
                        string strErr = "";

                        dal.AddParameter("p_classname", CostcenterList[i].LedgerName, 200, ParameterDirection.Input);
                        dal.AddParameter("p_issubclass", 0, ParameterDirection.Input);
                        dal.AddParameter("p_parentclassid", 0, ParameterDirection.Input);
                        dal.AddParameter("p_CompanyId", companyId, ParameterDirection.Input);
                        dal.AddParameter("p_accsysid", CostcenterList[i].LedgerMasterID, 50, ParameterDirection.Input);
                        dal.AddParameter("p_parentname", "", 200, ParameterDirection.Input);

                        int status = dal.ExecuteDML("spTallySet_CostCentre", CommandType.StoredProcedure, 0, ref strErr);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("MastersAPI", "InsertUpdateCostCentresInCRS", ex.Message);
            }
        }
    }
}
