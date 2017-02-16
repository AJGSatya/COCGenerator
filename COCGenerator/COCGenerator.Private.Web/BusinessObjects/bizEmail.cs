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
    public static class bizEmail
    {
        public static string GetEmailValue(string Code)
        {
            SqlConnection conn = new SqlConnection(); ;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spGetEmailValue";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 50;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@Code";
                p1.Value = Code;
                com.Parameters.Add(p1);
                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.VarChar;
                p2.Size = -1;
                p2.Direction = ParameterDirection.Output;
                p2.ParameterName = "@Value";
                com.Parameters.Add(p2);

                com.ExecuteNonQuery();
                return com.Parameters["@Value"].Value.ToString();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
