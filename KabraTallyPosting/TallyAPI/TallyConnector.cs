using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KabraTallyPosting.TallyAPI
{
    public class TallyConnector
    {
        public static string PostDataToTally(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
            httpReq.Method = "POST";
            httpReq.ContentLength = bytes.Length;
            httpReq.ContentType = "application/x-www-form-urlencoded";
            httpReq.GetRequestStream().Write(bytes, 0, bytes.Length);
            httpReq.Timeout = -1;
            HttpWebResponse httpresp = (HttpWebResponse)httpReq.GetResponse();
            StreamReader sr = new StreamReader(httpresp.GetResponseStream());
            string str = sr.ReadToEnd();
            httpresp.Close();
            return str;
        }

        public static string SendRequestToTally(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://localhost:9000");
            httpReq.Method = "POST";
            httpReq.ContentLength = bytes.Length;
            httpReq.ContentType = "application/x-www-form-urlencoded";
            httpReq.GetRequestStream().Write(bytes, 0, bytes.Length);
            httpReq.Timeout = -1;
            HttpWebResponse httpresp = (HttpWebResponse)httpReq.GetResponse();
            StreamReader sr = new StreamReader(httpresp.GetResponseStream());
            string str = sr.ReadToEnd();
            httpresp.Close();
            return str;
        }
    }
}
