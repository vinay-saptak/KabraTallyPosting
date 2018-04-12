using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KabraTallyPosting.Entity;
using KabraTallyPosting.TallyAPI;
using KabraTallyPosting.Util;
using KabraTallyPosting.CRSAPI;
using System.Xml;

namespace KabraTallyPosting.ExportAPI
{
    public class TallyExporter
    {
        public static void ExportLedgersFromTally(int companyId)
        {
           
            try
            {
                string tallyRequestMessage = TallyMessageCreator.CreateExportLedgersRequestMessage();
                string tallyResponse = TallyConnector.SendRequestToTally(tallyRequestMessage);
               
                List<Ledger> ledgerList = ParseTallyResponseForLedgers(tallyResponse);
                // string jsonconvertedFromDataset = JsonConvert.SerializeObject(ledgerList);
                MastersAPI.InsertUpdateLedgersInCRS(ledgerList, companyId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyExporter", "ExportLedgersFromTally", ex.Message);
            }
        }

        private static List<Ledger> ParseTallyResponseForLedgers(string tallyResponse)
        {
            List<Ledger> ledgerList = new List<Ledger>();
            try
            {
                Logger.WriteLog("Tally Response: " + tallyResponse);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tallyResponse);
                XmlNode listOfLedgers = xmlDoc.SelectSingleNode("LISTOFLEDGERS");
                if (listOfLedgers != null && listOfLedgers.HasChildNodes)
                {
                    for (int i = 0; i < listOfLedgers.ChildNodes.Count; i++)
                    {
                        Ledger l = new Ledger();
                        l.LedgerName = listOfLedgers.ChildNodes[i].InnerXml;
                        i++;
                        l.LedgerMasterID = listOfLedgers.ChildNodes[i].InnerXml;
                        i++;
                        l.LedgerParentName = listOfLedgers.ChildNodes[i].InnerXml;
                        ledgerList.Add(l);

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyExporter", "ParseTallyResponseForLedgers", ex.Message);
            }
            return ledgerList;
        }

        public static void ExportCostCentersFromTally(int companyId)
        {
            try
            {
                string tallyRequestMessage = TallyMessageCreator.CreateExportCostCentreRequestMessage();
                string tallyResponse = TallyConnector.SendRequestToTally(tallyRequestMessage);
                List<Ledger> CostcenterList = ParseTallyResponseForCostCenters(tallyResponse);
                MastersAPI.InsertUpdateCostCentresInCRS(CostcenterList, companyId);
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyExporter", "ExportCostCentersFromTally", ex.Message);
            }
        }

        private static List<Ledger> ParseTallyResponseForCostCenters(string tallyResponse)
        {
            List<Ledger> ledgerList = new List<Ledger>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tallyResponse);
                XmlNode listOfLedgers = xmlDoc.SelectSingleNode("LISTOFCOSTCENTRES");
                if (listOfLedgers != null && listOfLedgers.HasChildNodes)
                {
                    for (int i = 0; i < listOfLedgers.ChildNodes.Count; i++)
                    {
                        Ledger l = new Ledger();
                        l.LedgerName = listOfLedgers.ChildNodes[i].InnerText;
                        i++;
                        l.LedgerMasterID = listOfLedgers.ChildNodes[i].InnerText;
                        i++;
                        l.LedgerParentName = listOfLedgers.ChildNodes[i].InnerText;

                        ledgerList.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyExporter", "ParseTallyResponseForCostCenters", ex.Message);
            }
            return ledgerList;
        }
    }
}
