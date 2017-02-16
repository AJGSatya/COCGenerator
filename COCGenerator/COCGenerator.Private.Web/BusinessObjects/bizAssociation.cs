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
    public class bizAssociation
    {
        public Utilities.RTNMessages MSGS;

        public bizAssociation()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public bool InsertAssociation(string AssociationName, byte[] AssociationLogo)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertAssociation";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationName";
                p1.Value = AssociationName;
                com.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Image;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AssociationLogo";
                p2.Value = AssociationLogo;
                com.Parameters.Add(p2);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("3 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet GetAssociation(int AssociationID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spGetAssociation";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationID";
                p1.Value = AssociationID;
                com.Parameters.Add(p1);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("4 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public byte[] GetAssociationLogo(int AssociationID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spGetAssociation";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationID";
                p1.Value = AssociationID;
                com.Parameters.Add(p1);

                SqlDataReader dr = com.ExecuteReader();
                dr.Read();
                return (byte[])dr["AssociationLogo"];
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("5 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ListAssociations()
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListAssociations";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("6 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteAssociation(int AssociationID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                bizPreset biz = new bizPreset();
                DataSet ds;
                ds = biz.ListPresetsByAssociation(AssociationID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.MSGS.AddMessage("Association is in use. You can't delete it.", Utilities.enMsgType.Err);
                    return false;
                }

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spDeleteAssociation";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationID";
                p1.Value = AssociationID;
                com.Parameters.Add(p1);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("7 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool UpdateAssociation(int AssociationID, string AssociationName, byte[] AssociationLogo)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                DataSet ds;
                ds = this.GetAssociation(AssociationID);
                DataRow dr = ds.Tables[0].Rows[0];

                if (dr["AssociationName"].ToString() == AssociationName && (byte[])dr["AssociationLogo"] == AssociationLogo)
                {
                    this.MSGS.AddMessage("Nothing has been changed.", Utilities.enMsgType.Warn);
                    return false;
                }

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spUpdateAssociation";

                SqlParameter p0 = new SqlParameter();
                p0.SqlDbType = SqlDbType.Int;
                p0.Direction = ParameterDirection.Input;
                p0.ParameterName = "@AssociationID";
                p0.Value = AssociationID;
                com.Parameters.Add(p0);

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationName";
                p1.Value = AssociationName;
                com.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Image;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AssociationLogo";
                if (AssociationLogo.Length == 0)
                    p2.Value = (byte[])dr["AssociationLogo"];
                else
                    p2.Value = AssociationLogo;
                com.Parameters.Add(p2);

                com.ExecuteNonQuery();
                this.MSGS.AddMessage("Association has been succesfully updated.", Utilities.enMsgType.OK);
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("9 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
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
