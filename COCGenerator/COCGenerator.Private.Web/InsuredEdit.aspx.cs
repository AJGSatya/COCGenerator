using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using COCGenerator.BusinesObjects;

namespace COCGenerator
{
    public partial class InsuredEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (Request.QueryString["iid"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                        this.btnEdit.Visible = false;
                        return;
                    }

                    DataSet ds;
                    bizInsured biz = new bizInsured();
                    ds = biz.GetInsured(Convert.ToInt32(Request.QueryString["iid"]));

                    if (ds == null) return;

                    DataRow dr = ds.Tables[0].Rows[0];
                    this.txtInsuredName.Text = dr["InsuredName"].ToString();
                    this.ddlAssociations.SelectedValue = dr["AssociationID"].ToString();
                    this.txtAccountExecutive.Text = dr["AccountExecutive"].ToString();
                    this.txtInsurer.Text = dr["Insurer"].ToString();
                    this.txtStart.Text = String.Format("{0:dd/MM/yyyy}", dr["InsurancePeriodStart"]);
                    this.ddlStartHour.SelectedValue = String.Format("{0:hh}", dr["InsurancePeriodStart"]);
                    this.ddlStartAMPM.SelectedValue = String.Format("{0:tt}", dr["InsurancePeriodStart"]);
                    this.txtEnd.Text = String.Format("{0:dd/MM/yyyy}", dr["InsurancePeriodEnd"]);
                    this.ddlEndHour.SelectedValue = String.Format("{0:hh}", dr["InsurancePeriodEnd"]);
                    this.ddlEndAMPM.SelectedValue = String.Format("{0:tt}", dr["InsurancePeriodEnd"]);
                    this.ddlTimeZone.SelectedValue = dr["TimeZone"].ToString();
                    this.txtClass.Text = dr["Class"].ToString();
                    this.txtPolicyNumber.Text = dr["PolicyNumber"].ToString();
                    this.txtIndemnityLimit.Amount = Convert.ToDecimal(dr["IndemnityLimit"]);
                    this.txtOAMPSEmail.Text = dr["OAMPSEmail"].ToString();
                    this.txtBusinessDescription.Text = dr["BusinessDescription"].ToString();

                    this.btnBack.PostBackUrl = "~/AssociationInsureds.aspx?aid=" + dr["AssociationID"].ToString();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation() == false) return;

                String Start = this.txtStart.Text + " " + this.ddlStartHour.SelectedValue + " " + this.ddlStartAMPM.SelectedValue;
                String End = this.txtEnd.Text + " " + this.ddlEndHour.SelectedValue + " " + this.ddlEndAMPM.SelectedValue;

                bizInsured biz = new bizInsured();

                if (biz.ValidateInsured(this.txtInsuredName.Text,
                                     this.txtAccountExecutive.Text,
                                     this.txtOAMPSEmail.Text,
                                     Convert.ToInt32(Request.QueryString["aid"]),
                                     this.txtInsurer.Text,
                                     DateTime.Parse(Start),
                                     DateTime.Parse(End),
                                     this.ddlTimeZone.SelectedValue,
                                     this.txtClass.Text,
                                     this.txtPolicyNumber.Text,
                                     this.txtIndemnityLimit.Amount,
                                     this.txtBusinessDescription.Text) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                }

                if (biz.UpdateInsured(Convert.ToInt32(this.Request.QueryString["iid"]),
                                     this.txtInsuredName.Text,
                                     this.txtAccountExecutive.Text,
                                     this.txtOAMPSEmail.Text,
                                     Convert.ToInt32(this.ddlAssociations.SelectedValue),
                                     this.txtInsurer.Text,
                                     DateTime.Parse(Start),
                                     DateTime.Parse(End),
                                     this.ddlTimeZone.SelectedValue,
                                     this.txtClass.Text,
                                     this.txtPolicyNumber.Text,
                                     this.txtIndemnityLimit.Amount,
                                     this.txtBusinessDescription.Text) == true)
                {
                    this.ucMessanger1.ProcessMessage("Insured has been succesfully updated.", Utilities.enMsgType.OK, "", null, true);
                }
                else
                {
                    this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);

                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }

        private bool UIValidation()
        {
            if (this.txtInsuredName.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Insured Name.", Utilities.enMsgType.Err, "InsuredName", typeof(TextBox), true);
                return false;
            }
            if (this.ddlAssociations.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("You have to select Association.", Utilities.enMsgType.Err, "AccountExecutive", typeof(TextBox), true);
                return false;
            }
            if (this.txtAccountExecutive.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Account Executive.", Utilities.enMsgType.Err, "AccountExecutive", typeof(TextBox), true);
                return false;
            }
            if (this.txtInsurer.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Insurer.", Utilities.enMsgType.Err, "Insurer", typeof(TextBox), true);
                return false;
            }
            DateTime sresult;
            if (DateTime.TryParse(this.txtStart.Text, out sresult) == false)
            {
                this.ucMessanger1.ProcessMessage("You have to enter valid date.", Utilities.enMsgType.Err, "Start", typeof(TextBox), true);
                return false;
            }
            DateTime eresult;
            if (DateTime.TryParse(this.txtEnd.Text, out eresult) == false)
            {
                this.ucMessanger1.ProcessMessage("You have to enter valid date.", Utilities.enMsgType.Err, "End", typeof(TextBox), true);
                return false;
            }
            if (this.ddlTimeZone.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("You have to select Time Zone.", Utilities.enMsgType.Err, "TimeZone", typeof(DropDownList), true);
                return false;
            }
            if (this.txtClass.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Class.", Utilities.enMsgType.Err, "Class", typeof(TextBox), true);
                return false;
            }
            if (this.txtPolicyNumber.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Policy Number.", Utilities.enMsgType.Err, "PolicyNumber", typeof(TextBox), true);
                return false;
            }
            if (this.txtOAMPSEmail.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Arthur J. Gallagher Email.", Utilities.enMsgType.Err, "OAMPSEmail", typeof(TextBox), true);
                return false;
            }
            return true;
        }
    }
}
