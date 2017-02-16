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
using System.Text.RegularExpressions;

namespace COCGenerator.BusinesObjects
{
    public class bizInsured
    {
        public Utilities.RTNMessages MSGS;

        public bizInsured()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public bool ValidateInsured(string InsuredName, 
                                    string AccountExecutive,
                                    string OAMPSEmail,
                                    int AssociationID,
                                    string Insurer,
                                    DateTime InsurancePeriodStart,
                                    DateTime InsurancePeriodEnd,
                                    string TimeZone,
                                    string Class,
                                    string PolicyNumber,
                                    decimal IndemnityLimit,
                                    string BusinessDescription)
        {
            try
            {
                if (InsurancePeriodEnd <= InsurancePeriodStart)
                {
                    this.MSGS.AddMessage("TO date cannot be less or equal than FROM date.", Utilities.enMsgType.Err);
                    return false;
                }
                if (IsValidEmail(OAMPSEmail) == false)
                {
                    this.MSGS.AddMessage("You have to enter valid Email.", Utilities.enMsgType.Err);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("10 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
        }

        private bool IsValidEmail(string Email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                              @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                              @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            return re.IsMatch(Email);
        }

        public bool InsertInsured(string InsuredName, 
                                 string AccountExecutive,
                                 string OAMPSEmail,
                                 int AssociationID,
                                 string Insurer,
                                 DateTime InsurancePeriodStart,
                                 DateTime InsurancePeriodEnd,
                                 string TimeZone,
                                 string Class,
                                 string PolicyNumber,
                                 decimal IndemnityLimit,
                                 string BusinessDescription)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertInsured";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@InsuredName";
                p1.Value = InsuredName;
                com.Parameters.Add(p1);
                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.VarChar;
                p2.Size = 50;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AccountExecutive";
                p2.Value = AccountExecutive;
                com.Parameters.Add(p2);
                SqlParameter p1a = new SqlParameter();
                p1a.SqlDbType = SqlDbType.VarChar;
                p1a.Size = 50;
                p1a.Direction = ParameterDirection.Input;
                p1a.ParameterName = "@OAMPSEmail";
                p1a.Value = OAMPSEmail;
                com.Parameters.Add(p1a);
                SqlParameter p1b = new SqlParameter();
                p1b.SqlDbType = SqlDbType.Int;
                p1b.Direction = ParameterDirection.Input;
                p1b.ParameterName = "@AssociationID";
                p1b.Value = AssociationID;
                com.Parameters.Add(p1b);
                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.VarChar;
                p3.Size = 100;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@Insurer";
                p3.Value = Insurer;
                com.Parameters.Add(p3);
                SqlParameter p4 = new SqlParameter();
                p4.SqlDbType = SqlDbType.DateTime;
                p4.Direction = ParameterDirection.Input;
                p4.ParameterName = "@InsurancePeriodStart";
                p4.Value = InsurancePeriodStart;
                com.Parameters.Add(p4);
                SqlParameter p5 = new SqlParameter();
                p5.SqlDbType = SqlDbType.DateTime;
                p5.Direction = ParameterDirection.Input;
                p5.ParameterName = "@InsurancePeriodEnd";
                p5.Value = InsurancePeriodEnd;
                com.Parameters.Add(p5);
                SqlParameter p5a = new SqlParameter();
                p5a.SqlDbType = SqlDbType.VarChar;
                p5a.Size = 100;
                p5a.Direction = ParameterDirection.Input;
                p5a.ParameterName = "@TimeZone";
                p5a.Value = TimeZone;
                com.Parameters.Add(p5a);
                SqlParameter p6 = new SqlParameter();
                p6.SqlDbType = SqlDbType.VarChar;
                p6.Size = 50;
                p6.Direction = ParameterDirection.Input;
                p6.ParameterName = "@Class";
                p6.Value = Class;
                com.Parameters.Add(p6);
                SqlParameter p7 = new SqlParameter();
                p7.SqlDbType = SqlDbType.VarChar;
                p6.Size = 50;
                p7.Direction = ParameterDirection.Input;
                p7.ParameterName = "@PolicyNumber";
                p7.Value = PolicyNumber;
                com.Parameters.Add(p7);
                SqlParameter p8 = new SqlParameter();
                p8.SqlDbType = SqlDbType.Money;
                p8.Direction = ParameterDirection.Input;
                p8.ParameterName = "@IndemnityLimit";
                p8.Value = IndemnityLimit;
                com.Parameters.Add(p8);
                SqlParameter p9 = new SqlParameter();
                p9.SqlDbType = SqlDbType.VarChar;
                p9.Size = 1000;
                p9.Direction = ParameterDirection.Input;
                p9.ParameterName = "@BusinessDescription";
                p9.Value = BusinessDescription;
                com.Parameters.Add(p9);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("11 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ListAllInsureds()
        {
            SqlConnection conn = new SqlConnection(); ;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListAllInsureds";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("12 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet GetInsured(int InsuredID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spGetInsured";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@InsuredID";
                p1.Value = InsuredID;
                com.Parameters.Add(p1);

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("13 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet ListInsuredsByAssociation(int AssociationID)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Int;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@AssociationID";
                p1.Value = AssociationID;

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spListInsuredsByAssociation";
                com.Parameters.Add(p1);

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = com;

                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("14 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool DeleteInsured(int InsuredID)
        {
            SqlConnection conn = new SqlConnection(); ;
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spDeleteInsured";

                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.Int;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@InsuredID";
                p2.Value = InsuredID;
                com.Parameters.Add(p2);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("16 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool UpdateInsured(int InsuredID,
                                  string InsuredName,
                                  string AccountExecutive,
                                  string OAMPSEmail,
                                  int AssociationID,
                                  string Insurer,
                                  DateTime InsurancePeriodStart,
                                  DateTime InsurancePeriodEnd,
                                  string TimeZone,
                                  string Class,
                                  string PolicyNumber,
                                  decimal IndemnityLimit,
                                  string BusinessDescription)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();
                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spUpdateInsured";

                SqlParameter p0 = new SqlParameter();
                p0.SqlDbType = SqlDbType.Int;
                p0.Direction = ParameterDirection.Input;
                p0.ParameterName = "@InsuredID";
                p0.Value = InsuredID;
                com.Parameters.Add(p0);
                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.VarChar;
                p1.Size = 100;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@InsuredName";
                p1.Value = InsuredName;
                com.Parameters.Add(p1);
                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.VarChar;
                p2.Size = 50;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@AccountExecutive";
                p2.Value = AccountExecutive;
                com.Parameters.Add(p2);
                SqlParameter p1a = new SqlParameter();
                p1a.SqlDbType = SqlDbType.VarChar;
                p1a.Direction = ParameterDirection.Input;
                p1a.ParameterName = "@OAMPSEmail";
                p1a.Value = OAMPSEmail;
                com.Parameters.Add(p1a);
                SqlParameter p1b = new SqlParameter();
                p1b.SqlDbType = SqlDbType.Int;
                p1b.Direction = ParameterDirection.Input;
                p1b.ParameterName = "@AssociationID";
                p1b.Value = AssociationID;
                com.Parameters.Add(p1b);
                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.VarChar;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@Insurer";
                p3.Value = Insurer;
                com.Parameters.Add(p3);
                SqlParameter p4 = new SqlParameter();
                p4.SqlDbType = SqlDbType.DateTime;
                p4.Direction = ParameterDirection.Input;
                p4.ParameterName = "@InsurancePeriodStart";
                p4.Value = InsurancePeriodStart;
                com.Parameters.Add(p4);
                SqlParameter p5 = new SqlParameter();
                p5.SqlDbType = SqlDbType.DateTime;
                p5.Direction = ParameterDirection.Input;
                p5.ParameterName = "@InsurancePeriodEnd";
                p5.Value = InsurancePeriodEnd;
                com.Parameters.Add(p5);
                SqlParameter p5a = new SqlParameter();
                p5a.SqlDbType = SqlDbType.VarChar;
                p5a.Direction = ParameterDirection.Input;
                p5a.ParameterName = "@TimeZone";
                p5a.Value = TimeZone;
                com.Parameters.Add(p5a);
                SqlParameter p6 = new SqlParameter();
                p6.SqlDbType = SqlDbType.VarChar;
                p6.Direction = ParameterDirection.Input;
                p6.ParameterName = "@Class";
                p6.Value = Class;
                com.Parameters.Add(p6);
                SqlParameter p7 = new SqlParameter();
                p7.SqlDbType = SqlDbType.VarChar;
                p7.Direction = ParameterDirection.Input;
                p7.ParameterName = "@PolicyNumber";
                p7.Value = PolicyNumber;
                com.Parameters.Add(p7);
                SqlParameter p8 = new SqlParameter();
                p8.SqlDbType = SqlDbType.Money;
                p8.Direction = ParameterDirection.Input;
                p8.ParameterName = "@IndemnityLimit";
                p8.Value = IndemnityLimit;
                com.Parameters.Add(p8);
                SqlParameter p9 = new SqlParameter();
                p9.SqlDbType = SqlDbType.VarChar;
                p9.Size = 1000;
                p9.Direction = ParameterDirection.Input;
                p9.ParameterName = "@BusinessDescription";
                p9.Value = BusinessDescription;
                com.Parameters.Add(p9);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("17 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
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
