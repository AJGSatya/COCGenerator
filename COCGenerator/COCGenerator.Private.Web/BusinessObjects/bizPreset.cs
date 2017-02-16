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
using System.IO;

namespace COCGenerator.BusinesObjects
{
    public class bizPreset
    {
        public Utilities.RTNMessages MSGS;

        public bizPreset()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public bool ValidatePreset(int AssociationID,
                                   string Template,
                                   string DocumentType)
        {
            try
            {
                if (Path.GetExtension(Template) != ".pdf")
                {
                    this.MSGS.AddMessage("You have to select PDF file as a template.", Utilities.enMsgType.Err);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("19 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
        }

        public bool InsertPreset(int AssociationID,
                                 string TemplateName,
                                 byte[] Template,
                                 string DocumentType)
        {
            SqlConnection conn = new SqlConnection();;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertPreset";

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Int;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AssociationID";
                p2.Value = AssociationID;
                com.Parameters.Add(p2);
                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@TemplateName";
                p1.Value = TemplateName;
                com.Parameters.Add(p1);
                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.Image;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@Template";
                p3.Value = Template;
                com.Parameters.Add(p3);
                SqlParameter p1a = new SqlParameter();
                p1a.SqlDbType = SqlDbType.VarChar;
                p1a.Size = 50;
                p1a.Direction = ParameterDirection.Input;
                p1a.ParameterName = "@DocumentType";
                p1a.Value = DocumentType;
                com.Parameters.Add(p1a);
                                
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("20 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        
        public DataSet ListPresetsByAssociation(int AssociationID)
        {
            SqlConnection conn = new SqlConnection(); ;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListPresetsByAssociation";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationID";
                p1.Value = AssociationID;
                com.Parameters.Add(p1);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("21 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet GetPreset(int PresetID)
        {
            SqlConnection conn = new SqlConnection(); ;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spGetPreset";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@PresetID";
                p1.Value = PresetID;
                com.Parameters.Add(p1);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("22 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeletePreset(int PresetID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spDeletePreset";

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Int;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@PresetID";
                p2.Value = PresetID;
                com.Parameters.Add(p2);
               
                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("23 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool UpdatePreset(int PresetID, 
                                 int AssociationID, 
                                 string TemplateName, 
                                 byte[] Template, 
                                 string DocumentType)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                DataSet ds;
                ds = this.GetPreset(PresetID);
                DataRow dr = ds.Tables[0].Rows[0];

                if (AssociationID == Convert.ToInt32(dr["AssociationID"]) && TemplateName == dr["TemplateName"].ToString() && DocumentType == dr["DocumentType"].ToString())
                {
                    this.MSGS.AddMessage("Nothing has been changed.", Utilities.enMsgType.Warn);
                    return true;
                }

                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spUpdatePreset";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@PresetID";
                p1.Value = PresetID;
                com.Parameters.Add(p1);

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Int;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AssociationID";
                p2.Value = AssociationID;
                com.Parameters.Add(p2);

                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.VarChar;
                p3.Size = 100;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@TemplateName";
                p3.Value = TemplateName;
                com.Parameters.Add(p3);

                SqlParameter p4 = new SqlParameter();
                p4.SqlDbType = SqlDbType.Image;
                p4.Direction = ParameterDirection.Input;
                p4.ParameterName = "@Template";
                if (Template.Length == 0)
                    p4.Value = (byte[])dr["Template"];
                else
                    p4.Value = Template;
                com.Parameters.Add(p4);

                SqlParameter p5 = new SqlParameter();
                p5.SqlDbType = SqlDbType.VarChar;
                p5.Size = 50;
                p5.Direction = ParameterDirection.Input;
                p5.ParameterName = "@DocumentType";
                p5.Value = DocumentType;
                com.Parameters.Add(p5);

                com.ExecuteNonQuery();
                this.MSGS.AddMessage("Preset has been succesfully updated.", Utilities.enMsgType.OK);
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("24 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
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
