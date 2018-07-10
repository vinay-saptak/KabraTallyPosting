using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace KabraTallyPosting.CRSAPI
{
    class CRSDAL
    {
        MySqlCommand cmd;
        string strConnStringMaster;
        string strConnStringSlave;
        public CRSDAL()
        {
            cmd = new MySqlCommand();
            //strConnStringMaster = Convert.ToString(ConfigurationManager.AppSettings["ConnectionStringMaster"]);
            //strConnStringSlave = Convert.ToString(ConfigurationManager.AppSettings["ConnectionStringSlave"]);
            strConnStringMaster = "server=13.228.94.164;User Id=root;database=crs2011;password=crs43211234src;pooling=false;";
            strConnStringSlave = "server=52.76.182.185;User Id=root;database=crs2011;password=crss43211234ssrc;pooling=true;";
        }

        public void AddParameter(string strParamName, decimal dclParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Decimal).Value = dclParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public void AddParameter(string strParamName, int intParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.Int32).Value = intParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }

        public void AddParameter(string strParamName, string strParamValue, int intParamSize, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.VarChar, intParamSize).Value = strParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }

        public void AddParameter(string strParamName, DateTime dtParamValue, ParameterDirection direction = ParameterDirection.Input)
        {
            cmd.Parameters.Add(strParamName, MySqlDbType.DateTime).Value = dtParamValue;
            cmd.Parameters[strParamName].Direction = direction;
        }
        public DataSet ExecuteSelect(string strSQL, CommandType cmdType, int intTimeout, ref string strErr, string strOpt = "p_ErrMsg", bool IsMaster = true, string strSlaveMode = "", bool IsWrite = true, bool IsUseMinSecond = false)
        {

            DataSet ds = new DataSet();
            MySqlDataAdapter adp = new MySqlDataAdapter();
            MySqlConnection conn;
            if (IsMaster)
            {
                conn = new MySqlConnection(strConnStringMaster);
            }
            else
            {
                conn = new MySqlConnection(strConnStringSlave);
            }

            strErr = "";
            try
            {
                cmd.CommandText = strSQL;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = intTimeout;
                conn.Open();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                adp.Fill(ds);

                try
                {
                    if (cmd.Parameters[strOpt] != null && cmd.Parameters[strOpt].Value != null)
                        strErr = cmd.Parameters[strOpt].Value.ToString();
                }
                catch { }

                cmd.Parameters.Clear();
                //conn.Close();
            }
            catch (System.Exception ex)
            {
                ds = null;
                strErr = ex.Message;
            }
            finally
            {
                adp.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            return ds;
        }

        public int ExecuteDML(string strSQL, CommandType cmdType, int intTimeout, ref string strErr)
        {
            int status = 0;
            MySqlConnection conn = new MySqlConnection(strConnStringMaster);
            strErr = "";
            try
            {
                cmd.CommandText = strSQL;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = intTimeout;
                conn.Open();
                cmd.Connection = conn;
                status = cmd.ExecuteNonQuery();

                try
                {
                    if (cmd.Parameters["p_ErrMsg"] != null && cmd.Parameters["p_ErrMsg"].Value != null)
                        strErr = cmd.Parameters["p_ErrMsg"].Value.ToString();
                }
                catch { }

                cmd.Parameters.Clear();
            }
            catch (System.Exception ex)
            {
                status = -1;
                strErr = ex.Message;

            }
            finally
            {
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                conn = null;
            }
            return status;
        }
        
        public Boolean SlaveStatus(ref int arrSlaveValue)
        {
            string strErr = "";
            DataSet ds = new DataSet();
            Boolean blnSuccess = false;
            MySqlDataAdapter adp = new MySqlDataAdapter();
            MySqlConnection conn = new MySqlConnection(strConnStringSlave);
            arrSlaveValue = 99999;
            strErr = "";
            try
            {
                cmd.CommandText = "SHOW SLAVE STATUS";
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 1;
                //conn.ConnectionTimeout = 1;

                conn.Open();
                cmd.Connection = conn;
                adp.SelectCommand = cmd;
                adp.Fill(ds);
                //conn.Close();
            }
            catch (System.Exception ex)
            {
                ds = null;
                strErr = ex.Message;
            }
            finally
            {
                adp.Dispose();
                cmd.Dispose();
                conn.Close();
                conn.Dispose();
                conn = null;
            }

            string strSecondsBehindMaster = "";

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strSecondsBehindMaster = Convert.ToString(ds.Tables[0].Rows[0]["Seconds_Behind_Master"]).ToUpper();
                
                if (strSecondsBehindMaster != "")
                    arrSlaveValue = Convert.ToInt32(strSecondsBehindMaster);

                if (strSecondsBehindMaster != "" && strSecondsBehindMaster != "NULL")
                {
                    if (ds.Tables[0].Rows[0]["slave_SQL_running"].ToString().ToUpper() == "YES")
                    {
                        blnSuccess = true;
                    }
                }
            }

            return blnSuccess;

        }
    }
}
