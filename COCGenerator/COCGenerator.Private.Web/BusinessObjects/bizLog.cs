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
using Utilities;

namespace COCGenerator.BusinesObjects
{
    public static class bizLog
    {
        public static DataSet ListLogs(int Filter)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListLogs";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@Filter";
                p1.Value = Filter;
                com.Parameters.Add(p1);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool InsertExceptionLog(String ShortDescription, String LongDescription)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertLog";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 255;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@ShortDescription";
                p1.Value = ShortDescription;
                com.Parameters.Add(p1);
                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Text;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@LongDescription";
                p2.Value = LongDescription;
                com.Parameters.Add(p2);
                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.DateTime;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@CreatedDate";
                p3.Value = DateTime.Now;
                com.Parameters.Add(p3);
                SqlParameter p4 = new SqlParameter();
                p4.SqlDbType = SqlDbType.VarChar;
                p4.Size = 50;
                p4.Direction = ParameterDirection.Input;
                p4.ParameterName = "@CreatedBy";
                p4.Value = User.GetCurrentUserWithoutDomain();
                com.Parameters.Add(p4);

                com.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                Log.LogEvent(ex.Message + "\n" + ex.StackTrace, enMsgType.Err, "COC Generator");
                throw new Exception(ex.Message);
            }
        }
    }
}
