using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

namespace COCGenerator.BusinesObjects
{
    public class bizTimeZone
    {
        public Utilities.RTNMessages MSGS;

        public bizTimeZone()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public bool InsertTimeZone(string TimeZoneName)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                DataSet ds = ListTimeZones();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["TimeZoneName"].ToString() == TimeZoneName)
                    {
                        this.MSGS.AddMessage("Time Zone already exists.", Utilities.enMsgType.Err);
                        return false;
                    }
                }

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertTimeZone";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@TimeZoneName";
                p1.Value = TimeZoneName;
                com.Parameters.Add(p1);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("25 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ListTimeZones()
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListTimeZones";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("26 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteTimeZone(string TimeZoneName)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                bizInsured biz = new bizInsured();
                DataSet ds = biz.ListAllInsureds();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["TimeZone"].ToString() == TimeZoneName)
                    {
                        this.MSGS.AddMessage("Time Zone is in use. You can't delete it.", Utilities.enMsgType.Err);
                        return false;
                    }
                }

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spDeleteTimeZone";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@TimeZoneName";
                p1.Value = TimeZoneName;
                com.Parameters.Add(p1);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("27 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
