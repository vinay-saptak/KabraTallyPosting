using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KabraTallyPosting.Util
{
    public class Logger
    {
        public static void WriteLog(string data)
        {
            try
            {
                int intServerMinsOffset = Convert.ToInt32(ConfigurationManager.AppSettings["ServerOffsetMins"]);
                StreamWriter log;
                string logpath = ConfigurationManager.AppSettings["LogPath"];
                if (!File.Exists(@logpath + "logfile_" + DateTime.Now.AddMinutes(intServerMinsOffset).ToString("dd-MMM-yyy") + ".txt"))
                {
                    log = new StreamWriter(@logpath + "logfile_" + DateTime.Now.AddMinutes(intServerMinsOffset).ToString("dd-MMM-yyy") + ".txt");
                }
                else
                {
                    log = File.AppendText(@logpath + "logfile_" + DateTime.Now.AddMinutes(intServerMinsOffset).ToString("dd-MMM-yyy") + ".txt");
                }

                string currenttime = Util.GetServerDateTime().ToString("dd-MMM-yyy HH:mm:ss.fff");
                if (data != "")
                    log.WriteLine(currenttime + " : " + data);

                log.Close();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
        public static void WriteLogAlert(string data)
        {
            try
            {
                int intServerMinsOffset = Convert.ToInt32(ConfigurationManager.AppSettings["ServerOffsetMins"]);
                StreamWriter log;
                string logpath = ConfigurationManager.AppSettings["LogPath"];
                if (!File.Exists(@logpath + "Alertfile"  + ".txt"))
                {
                    log = new StreamWriter(@logpath + "Alertfile" + ".txt");
                }
                else
                {
                    log = File.AppendText(@logpath + "Alertfile" + ".txt");
                }

                string currenttime = Util.GetServerDateTime().ToString("dd-MMM-yyy HH:mm:ss.fff");
                if (data != "")
                    log.WriteLine(currenttime + " : " + data);

                log.Close();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public static void WriteLogAlert2(string data)
        {
            try
            {
                int intServerMinsOffset = Convert.ToInt32(ConfigurationManager.AppSettings["ServerOffsetMins"]);
                StreamWriter log;
                string logpath = ConfigurationManager.AppSettings["LogPath"];
                if (!File.Exists(@logpath + "Alertfile2" + ".txt"))
                {
                    log = new StreamWriter(@logpath + "Alertfile2" + ".txt");
                }
                else
                {
                    log = File.AppendText(@logpath + "Alertfile2" + ".txt");
                }

                string currenttime = Util.GetServerDateTime().ToString("dd-MMM-yyy HH:mm:ss.fff");
                if (data != "")
                    log.WriteLine(currenttime + " : " + data);

                log.Close();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public static void WriteLog(string heading, string subheading, string data, bool toSendMail = true)
        {
            string msg = heading + "::" + subheading + "::" + data;
            WriteLog(msg);
            if(toSendMail)
            {
                Email.SendMail(msg);
            }
        }
    }
}
