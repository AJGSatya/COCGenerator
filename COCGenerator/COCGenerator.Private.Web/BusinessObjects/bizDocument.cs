using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace COCGenerator.BusinesObjects
{
    public class bizDocument
    {
        public Utilities.RTNMessages MSGS;

        public bizDocument()
        {
            MSGS = new Utilities.RTNMessages();
        }

        public bool ValidateRequest(bool InterestedParty,
                                   string InterestedPartyName,
                                   string RequestedBy,
                                   string Email,
                                   string PositionAtInsured)
        {
            try
            {
                if (InterestedParty == true && InterestedPartyName == "")
                {
                    this.MSGS.AddMessage("You have to enter Interested Party Name.", Utilities.enMsgType.Err);
                    return false;
                }
                if (IsValidEmail(Email) == false)
                {
                    this.MSGS.AddMessage("You have to enter valid Email.", Utilities.enMsgType.Err);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("2 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
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

        public bool ProcessRequest(string AssociationName,
                                  bool InterestedParty,
                                  string InterestedPartyName,
                                  string RequestedBy,
                                  string Email,
                                  string PositionAtInsured,
                                  string TemplateName,
                                  byte[] Template,
                                  string DocumentType,
                                  string InsuredName,
                                  string AccountExecutive,
                                  string Insurer,
                                  DateTime InsurancePeriodStart,
                                  DateTime InsurancePeriodEnd,
                                  string TimeZone,
                                  string Class,
                                  string PolicyNumber,
                                  Decimal IndemnityLimit,
                                  string OAMPSEmail,
                                  string BusinessDescription)
{
            try
            {
                // Check for override values in web.config - if specified, send e-mails to the override values instead of the intended recipients
                /// Useful 
                if (ConfigurationManager.AppSettings.Get("OverrideInternalEmail") != null 
                    && ConfigurationManager.AppSettings.Get("OverrideInternalEmail") != "") OAMPSEmail = ConfigurationManager.AppSettings.Get("OverrideInternalEmail");
                if (ConfigurationManager.AppSettings.Get("OverrideRequestorEmail") != null
                    && ConfigurationManager.AppSettings.Get("OverrideRequestorEmail") != "") Email = ConfigurationManager.AppSettings.Get("OverrideRequestorEmail");

            }
            catch { }

            SqlConnection conn = new SqlConnection();
            try
            {
                // Populate Form Fields
                PdfReader r = new PdfReader(Template);
                string populatedFile = HostingEnvironment.MapPath(@"~/templates/" + string.Format("{0:yyyyMMddhhmmssff}", DateTime.Now) + ".pdf");
                //PdfStamper s = new PdfStamper(r, new FileStream(populatedFile, FileMode.Create));
                PdfStamper s = new PdfStamper(r, new FileStream(populatedFile, FileMode.Create));
                AcroFields pdfFormFields = s.AcroFields;
                pdfFormFields.SetField("Association", AssociationName);
                pdfFormFields.SetField("Insured", InsuredName);
                pdfFormFields.SetField("Insurer", Insurer);
                pdfFormFields.SetField("InsurancePeriodStart", string.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodStart) + " " + TimeZone);
                pdfFormFields.SetField("InsurancePeriodEnd", string.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodEnd) + " " + TimeZone);
                pdfFormFields.SetField("PolicyNumber", PolicyNumber);
                pdfFormFields.SetField("Class", Class);
                pdfFormFields.SetField("PolicyNumber", PolicyNumber);
                pdfFormFields.SetField("IndemnityLimit", string.Format("{0:C}", IndemnityLimit));
                pdfFormFields.SetField("BusinessDescription", BusinessDescription);
                if (InterestedParty == false)
                {
                    pdfFormFields.SetField("InterestedPartyLabel", "");
                    pdfFormFields.SetField("InterestedPartyName", "");
                    //pdfFormFields.SetFieldProperty("InterestedPartyName", "flags", PdfAnnotation.FLAGS_INVISIBLE, null);
                    //pdfFormFields.RegenerateField("InterestedPartyName");
                }
                else
                {
                    pdfFormFields.SetField("InterestedPartyLabel", "Interested Third Party");
                    pdfFormFields.SetField("InterestedPartyName", InterestedPartyName);
                    //pdfFormFields.SetFieldProperty("InterestedPartyName", "flags", PdfAnnotation.FLAGS_PRINT, null);
                    //pdfFormFields.RegenerateField("InterestedPartyName");
                }
                pdfFormFields.SetField("OnBehalfInsurer", Insurer);
                s.FormFlattening = true;
                s.Close();
                r.Close();

                // Send Email
                if (InterestedParty == false)
                {
                    // to requestor
                    StringBuilder body1 = new StringBuilder(bizEmail.GetEmailValue("NonInterestedPartyRequestorMessage"));
                    body1.Replace("<<RequestedBy>>", RequestedBy);
                    body1.Replace("<<DocumentType>>", DocumentType);
                    body1.Replace("<<AccountExecutive>>", AccountExecutive);

                    StringBuilder sub1 = new StringBuilder(bizEmail.GetEmailValue("RequestorSubject"));
                    sub1.Replace("<<DocumentType>>", DocumentType);

                    Attachment att1 = new Attachment(populatedFile);
                    att1.Name = DocumentType + " - " + InsuredName + ".pdf";

                    MailMessage msg1 = new MailMessage(bizEmail.GetEmailValue("MailFromNoReply"), Email);
                    msg1.ReplyToList.Add(new MailAddress(OAMPSEmail));
                    msg1.Subject = sub1.ToString();
                    msg1.IsBodyHtml = true;
                    msg1.Body = body1.ToString();
                    msg1.Attachments.Add(att1);

                    SmtpClient emailClient1 = new SmtpClient(bizEmail.GetEmailValue("SMTPServer"));
                    emailClient1.Credentials = new NetworkCredential(bizEmail.GetEmailValue("UserName"), bizEmail.GetEmailValue("Password")); 
                    //emailClient1.UseDefaultCredentials = true;
                    emailClient1.Send(msg1);
                    msg1.Attachments.Dispose();
                    msg1.Dispose();


                    // to account executive
                    StringBuilder sub2 = new StringBuilder(bizEmail.GetEmailValue("ExecutiveSubject"));
                    sub2.Replace("<<DocumentType>>", DocumentType);
                    sub2.Replace("<<Insured>>", InsuredName);

                    StringBuilder body2 = new StringBuilder(bizEmail.GetEmailValue("NonInterestedPartyExecutiveMessage"));
                    body2.Replace("<<AccountExecutive>>", AccountExecutive);
                    body2.Replace("<<Insured>>", InsuredName);
                    body2.Replace("<<DocumentType>>", DocumentType);
                    body2.Replace("<<Insurer>>", Insurer);
                    body2.Replace("<<InsurancePeriodStart>>", String.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodStart));
                    body2.Replace("<<InsurancePeriodEnd>>", String.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodEnd));
                    //body2.Replace("<<InterestedPartyName>>", InterestedPartyName);
                    body2.Replace("<<Class>>", Class);
                    body2.Replace("<<PolicyNumber>>", PolicyNumber);
                    body2.Replace("<<IndemnityLimit>>", String.Format("{0:C}", IndemnityLimit));
                    body2.Replace("<<BusinessDescription>>", "<br />" + BusinessDescription.Replace("\n", "<br />"));
                    body2.Replace("<<RequestedBy>>", RequestedBy);
                    body2.Replace("<<Email>>", Email);
                    body2.Replace("<<PositionAtInsured>>", PositionAtInsured);

                    MailMessage msg2 = new MailMessage(bizEmail.GetEmailValue("MailFromNoReply"), OAMPSEmail);
                    msg2.IsBodyHtml = true;
                    msg2.Subject = sub2.ToString();
                    msg2.Body = body2.ToString();

                    SmtpClient emailClient2 = new SmtpClient(bizEmail.GetEmailValue("SMTPServer"));
                    emailClient2.Credentials = new NetworkCredential(bizEmail.GetEmailValue("UserName"), bizEmail.GetEmailValue("Password")); 
                    //emailClient2.UseDefaultCredentials = true;
                    emailClient2.Send(msg2);
                    msg2.Attachments.Dispose();
                    msg2.Dispose();
                }
                else
                {
                    // to requestor
                    StringBuilder sub3 = new StringBuilder(bizEmail.GetEmailValue("RequestorSubject"));
                    sub3.Replace("<<DocumentType>>", DocumentType);

                    StringBuilder body3 = new StringBuilder(bizEmail.GetEmailValue("InterestedPartyRequestorMessage"));
                    body3.Replace("<<RequestedBy>>", RequestedBy);
                    body3.Replace("<<DocumentType>>", DocumentType);
                    body3.Replace("<<AccountExecutive>>", AccountExecutive);

                    MailMessage msg3 = new MailMessage(bizEmail.GetEmailValue("MailFromNoReply"), Email);
                    msg3.ReplyToList.Add(new MailAddress(OAMPSEmail));
                    msg3.Subject = sub3.ToString();
                    msg3.IsBodyHtml = true;
                    msg3.Body = body3.ToString();

                    SmtpClient emailClient3 = new SmtpClient(bizEmail.GetEmailValue("SMTPServer"));
                    emailClient3.Credentials = new NetworkCredential(bizEmail.GetEmailValue("UserName"), bizEmail.GetEmailValue("Password")); 
                    //emailClient3.UseDefaultCredentials = true;
                    emailClient3.Send(msg3);
                    msg3.Attachments.Dispose();
                    msg3.Dispose();

                    // to account executive
                    StringBuilder sub4 = new StringBuilder(bizEmail.GetEmailValue("ExecutiveSubject"));
                    sub4.Replace("<<DocumentType>>", DocumentType);
                    sub4.Replace("<<Insured>>", InsuredName);

                    StringBuilder body4 = new StringBuilder(bizEmail.GetEmailValue("InterestedPartyExecutiveMessage"));
                    body4.Replace("<<AccountExecutive>>", AccountExecutive);
                    body4.Replace("<<Insured>>", InsuredName);
                    body4.Replace("<<DocumentType>>", DocumentType);
                    body4.Replace("<<Insurer>>", Insurer);
                    body4.Replace("<<InsurancePeriodStart>>", String.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodStart));
                    body4.Replace("<<InsurancePeriodEnd>>", String.Format("{0:dd/MM/yyyy hh tt}", InsurancePeriodEnd));
                    body4.Replace("<<InterestedPartyName>>", InterestedPartyName);
                    body4.Replace("<<Class>>", Class);
                    body4.Replace("<<PolicyNumber>>", PolicyNumber);
                    body4.Replace("<<IndemnityLimit>>", String.Format("{0:C}", IndemnityLimit));
                    body4.Replace("<<BusinessDescription>>", "<br />" + BusinessDescription.Replace("\n", "<br />"));
                    body4.Replace("<<RequestedBy>>", RequestedBy);
                    body4.Replace("<<Email>>", Email);
                    body4.Replace("<<PositionAtInsured>>", PositionAtInsured);

                    Attachment att = new Attachment(populatedFile);
                    att.Name = DocumentType + " - " + InsuredName + ".pdf";

                    MailMessage msg4 = new MailMessage(bizEmail.GetEmailValue("MailFromNoReply"), OAMPSEmail);
                    msg4.Subject = sub4.ToString();
                    msg4.IsBodyHtml = true;
                    msg4.Body = body4.ToString();
                    msg4.Attachments.Add(att);

                    SmtpClient emailClient4 = new SmtpClient(bizEmail.GetEmailValue("SMTPServer"));
                    emailClient4.Credentials = new NetworkCredential(bizEmail.GetEmailValue("UserName"), bizEmail.GetEmailValue("Password")); 
                    //emailClient4.UseDefaultCredentials = true;
                    emailClient4.Send(msg4);
                    msg4.Attachments.Dispose();
                    msg4.Dispose();
                }

                // Delete Document
                File.Delete(populatedFile);

                // Insert Document Data into Database
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["COC-Conn"].ConnectionString;
                conn.Open();

                SqlCommand com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;
                com.CommandText = "spInsertDocument";

                SqlParameter p1 = new SqlParameter();
                p1.SqlDbType = SqlDbType.Bit;
                p1.Direction = ParameterDirection.Input;
                p1.ParameterName = "@InterestedParty";
                p1.Value = InterestedParty;
                com.Parameters.Add(p1);
                SqlParameter p2 = new SqlParameter();
                p2.SqlDbType = SqlDbType.VarChar;
                p2.Direction = ParameterDirection.Input;
                p2.ParameterName = "@InterestedPartyName";
                p2.Value = InterestedPartyName;
                com.Parameters.Add(p2);
                SqlParameter p3 = new SqlParameter();
                p3.SqlDbType = SqlDbType.VarChar;
                p3.Direction = ParameterDirection.Input;
                p3.ParameterName = "@RequestedBy";
                p3.Value = RequestedBy;
                com.Parameters.Add(p3);
                SqlParameter p4 = new SqlParameter();
                p4.SqlDbType = SqlDbType.VarChar;
                p4.Direction = ParameterDirection.Input;
                p4.ParameterName = "@Email";
                p4.Value = Email;
                com.Parameters.Add(p4);
                SqlParameter p5 = new SqlParameter();
                p5.SqlDbType = SqlDbType.VarChar;
                p5.Direction = ParameterDirection.Input;
                p5.ParameterName = "@PositionAtInsured";
                p5.Value = PositionAtInsured;
                com.Parameters.Add(p5);
                SqlParameter p5a = new SqlParameter();
                p5a.SqlDbType = SqlDbType.VarChar;
                p5a.Direction = ParameterDirection.Input;
                p5a.ParameterName = "@Template";
                p5a.Value = TemplateName;
                com.Parameters.Add(p5a);
                SqlParameter p6a = new SqlParameter();
                p6a.SqlDbType = SqlDbType.VarChar;
                p6a.Direction = ParameterDirection.Input;
                p6a.ParameterName = "@DocumentType";
                p6a.Value = DocumentType;
                com.Parameters.Add(p6a);
                SqlParameter p7 = new SqlParameter();
                p7.SqlDbType = SqlDbType.VarChar;
                p7.Direction = ParameterDirection.Input;
                p7.ParameterName = "@InsuredName";
                p7.Value = InsuredName;
                com.Parameters.Add(p7);
                SqlParameter p7a = new SqlParameter();
                p7a.SqlDbType = SqlDbType.VarChar;
                p7a.Direction = ParameterDirection.Input;
                p7a.ParameterName = "@AccountExecutive";
                p7a.Value = AccountExecutive;
                com.Parameters.Add(p7a);
                SqlParameter p8 = new SqlParameter();
                p8.SqlDbType = SqlDbType.VarChar;
                p8.Direction = ParameterDirection.Input;
                p8.ParameterName = "@Insurer";
                p8.Value = Insurer;
                com.Parameters.Add(p8);
                SqlParameter p9 = new SqlParameter();
                p9.SqlDbType = SqlDbType.DateTime;
                p9.Direction = ParameterDirection.Input;
                p9.ParameterName = "@InsurancePeriodStart";
                p9.Value = InsurancePeriodStart;
                com.Parameters.Add(p9);
                SqlParameter p10 = new SqlParameter();
                p10.SqlDbType = SqlDbType.DateTime;
                p10.Direction = ParameterDirection.Input;
                p10.ParameterName = "@InsurancePeriodEnd";
                p10.Value = InsurancePeriodEnd;
                com.Parameters.Add(p10);
                SqlParameter p11 = new SqlParameter();
                p11.SqlDbType = SqlDbType.VarChar;
                p11.Direction = ParameterDirection.Input;
                p11.ParameterName = "@Class";
                p11.Value = Class;
                com.Parameters.Add(p11);
                SqlParameter p12 = new SqlParameter();
                p12.SqlDbType = SqlDbType.VarChar;
                p12.Direction = ParameterDirection.Input;
                p12.ParameterName = "@PolicyNumber";
                p12.Value = PolicyNumber;
                com.Parameters.Add(p12);
                SqlParameter p13 = new SqlParameter();
                p13.SqlDbType = SqlDbType.VarChar;
                p13.Direction = ParameterDirection.Input;
                p13.ParameterName = "@IndemnityLimit";
                p13.Value = IndemnityLimit;
                com.Parameters.Add(p13);
                SqlParameter p14 = new SqlParameter();
                p14.SqlDbType = SqlDbType.VarChar;
                p14.Direction = ParameterDirection.Input;
                p14.ParameterName = "@BusinessDescription";
                p14.Value = BusinessDescription;
                com.Parameters.Add(p14);
                SqlParameter p15 = new SqlParameter();
                p15.SqlDbType = SqlDbType.VarChar;
                p15.Direction = ParameterDirection.Input;
                p15.ParameterName = "@OAMPSEmail";
                p15.Value = OAMPSEmail;
                com.Parameters.Add(p15);

                com.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                this.MSGS.AddMessage(string.Format("1 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err);
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
