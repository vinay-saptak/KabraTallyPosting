using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using KabraTallyPosting.Entity;
using KabraTallyPosting.Util;

namespace KabraTallyPosting.TallyAPI
{
    public class TallyMessageCreator
    {
        

        public static TallyResponse GetStatusFromResponseXML(string responseXML)
        {
            TallyResponse tr = new TallyResponse();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseXML);
                tr.Status = xmlDoc.SelectSingleNode("RESPONSE/CREATED").InnerText;
                tr.EntityId = xmlDoc.SelectSingleNode("RESPONSE/LASTVCHID").InnerText;
                tr.StatusMessage = xmlDoc.SelectSingleNode("RESPONSE/ERRORS").InnerText;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyMessageCreator", "GetStatusFromResponseXML", "Exception Message : " + ex.Message + "ResponseXML :" + responseXML);
            }
            return tr;
        }

        public static string CreateExportLedgersRequestMessage()
        {
            string xmlmessage = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string templateDirectory = ConfigurationManager.AppSettings["XMLTemplateFilesPath"].ToString();
                string fileName = "ExportLedgers.xml";
                xmlDoc.Load(templateDirectory + fileName);
                string tallyCompanyName = ConfigurationManager.AppSettings["TallyCompanyName"].ToString();

                XmlNode companyNameNode = xmlDoc.SelectSingleNode("/ENVELOPE/BODY/DESC/STATICVARIABLES/SVCURRENTCOMPANY");
                companyNameNode.InnerText = tallyCompanyName;
                xmlmessage = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyMessageCreator", "CreateExportLedgersRequestMessage", ex.Message);
            }
            return xmlmessage;
        }


        public static string CreateExportCostCentreRequestMessage()
        {
            string xmlmessage = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string templateDirectory = ConfigurationManager.AppSettings["XMLTemplateFilesPath"].ToString();
                string fileName = "ExportCostCenters.xml";
                xmlDoc.Load(templateDirectory + fileName);
                xmlmessage = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyMessageCreator", "CreateExportCostCentreRequestMessage", ex.Message);
            }
            return xmlmessage;
        }

        public static string CreateSingleLedgerDetailRequestMessage()
        {
            string xmlmessage = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string templateDirectory = ConfigurationManager.AppSettings["XMLTemplateFilesPath"].ToString();
                string fileName = "ExportSingleLedger.xml";
                xmlDoc.Load(templateDirectory + fileName);
                string tallyCompanyName = ConfigurationManager.AppSettings["TallyCompanyName"].ToString();
               
                XmlNode companyNameNode = xmlDoc.SelectSingleNode("/ENVELOPE/BODY/DESC/STATICVARIABLES/SVCURRENTCOMPANY");
                companyNameNode.InnerText = tallyCompanyName;
                xmlmessage = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyMessageCreator", "CreateSingleLedgerDetailRequestMessage", ex.Message);
            }
            return xmlmessage;
        }


        public static string CreateJournalXML(Journal jl, List<JournalDetail> jdList)
        {
            string xmlmessage = "";
            string fileName = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                string templateDirectory = ConfigurationManager.AppSettings["XMLTemplateFilesPath"].ToString();
              
                fileName = "BranchJournal.xml";
             
                xmlDoc.Load(templateDirectory + fileName);

                string tallyCompanyName = ConfigurationManager.AppSettings["TallyCompanyName"].ToString();
               // string CostcategoryName = ConfigurationManager.AppSettings["CostCategoryName"].ToString();

                XmlNode companyNameNode = xmlDoc.SelectSingleNode("/ENVELOPE/BODY/IMPORTDATA/REQUESTDESC/STATICVARIABLES/SVCURRENTCOMPANY");
                companyNameNode.InnerText = tallyCompanyName;

              

                xmlDoc.SelectSingleNode("/ENVELOPE/BODY/IMPORTDATA/REQUESTDATA/TALLYMESSAGE/VOUCHER/NARRATION").InnerText = jl.Narration;
               
               
               // xmlDoc.SelectSingleNode("/ENVELOPE/BODY/IMPORTDATA/REQUESTDATA/TALLYMESSAGE/VOUCHER/REFERENCE").InnerText = Convert.ToString(jl.DocNumber);
                xmlDoc.SelectSingleNode("/ENVELOPE/BODY/IMPORTDATA/REQUESTDATA/TALLYMESSAGE/VOUCHER/VOUCHERNUMBER").InnerText = Convert.ToString(jl.DocFormattedNumber);
               

                XmlNode voucherNode = xmlDoc.SelectSingleNode("/ENVELOPE/BODY/IMPORTDATA/REQUESTDATA/TALLYMESSAGE/VOUCHER");
                // XmlNodeList ledgerList = voucherNode.SelectNodes("ALLLEDGERENTRIES.LIST");

                if (jdList != null && jdList.Count > 1)
                {
                    for (int i = 0; i < jdList.Count; i++)
                    {


                        XmlNode ledgerNode = xmlDoc.CreateElement("ALLLEDGERENTRIES.LIST");

                        XmlNode isdeemedpositive = xmlDoc.CreateElement("ISDEEMEDPOSITIVE");
                        XmlNode amountNode = xmlDoc.CreateElement("AMOUNT");
                        XmlNode ledgerNameNode = xmlDoc.CreateElement("LEDGERNAME");
                        ledgerNameNode.InnerText = jdList[i].LedgerName;

                        XmlNode categorynode = xmlDoc.CreateElement("CATEGORYALLOCATIONS.LIST");
                        //XmlNode category = xmlDoc.CreateElement("CATEGORY");
                        //category.InnerText = CostcategoryName;
                        XmlNode isdeemedpositive1 = xmlDoc.CreateElement("ISDEEMEDPOSITIVE");
                        XmlNode CostCentreNode = xmlDoc.CreateElement("COSTCENTREALLOCATIONS.LIST");
                        XmlNode CostCentreName = xmlDoc.CreateElement("Name");
                        CostCentreName.InnerText = jdList[i].ClassName;
                        XmlNode Costamountnode = xmlDoc.CreateElement("AMOUNT");


                        if (jdList[i].IsDebit == 0)
                        {
                            isdeemedpositive.InnerText = "No";
                            amountNode.InnerText = jdList[i].Amount.ToString();
                            isdeemedpositive1.InnerText = "No";
                            Costamountnode.InnerText = jdList[i].Amount.ToString();

                           


                        }
                        else if (jdList[i].IsDebit == 1)
                        {

                            isdeemedpositive.InnerText = "Yes";
                            amountNode.InnerText = (-1 * jdList[i].Amount).ToString();

                            isdeemedpositive1.InnerText = "Yes";
                            Costamountnode.InnerText = (-1 * jdList[i].Amount).ToString();
                        }

                        ledgerNode.AppendChild(isdeemedpositive);
                        ledgerNode.AppendChild(ledgerNameNode);
                        ledgerNode.AppendChild(amountNode);

                        //categorynode.AppendChild(category);
                        categorynode.AppendChild(isdeemedpositive1);

                        CostCentreNode.AppendChild(CostCentreName);
                        CostCentreNode.AppendChild(Costamountnode);

                        categorynode.AppendChild(CostCentreNode);
                        ledgerNode.AppendChild(categorynode);

                        //if (jdList[i].Description.Trim().Length > 0)
                        //{
                        //    XmlNode narrationNode = xmlDoc.CreateElement("NARRATION");
                        //    narrationNode.InnerText = jdList[i].Description;
                        //    ledgerNode.AppendChild(narrationNode);
                        //}

                        voucherNode.AppendChild(ledgerNode);
                    }
                }


                string env = ConfigurationManager.AppSettings["Env"].ToString();
                string strDate = "";

                if (env == "Dev")
                {
                    strDate = "20180301";
                }
                else if (env == "Prod")
                {
                    strDate = jl.JournalDateTime.ToString("yyyyMMdd");
                }

                //Date
                voucherNode.SelectSingleNode("DATE").InnerText = strDate;
                voucherNode.SelectSingleNode("EFFECTIVEDATE").InnerText = strDate;
                voucherNode.Attributes["DATE"].InnerText = strDate;

                xmlmessage = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("TallyMessageCreator", "CreateJournalXML", "JournalId: " + jl.JournalId + " " + ex.Message);
            }

            return xmlmessage;
        }
    }
}
