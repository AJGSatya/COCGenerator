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
    public partial class InsuredAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["aid"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                    this.btnAdd.Visible = false;
                    return;
                }

                this.ddlStartHour.SelectedValue = "04";
                this.ddlStartAMPM.SelectedValue = "PM";
                this.ddlEndHour.SelectedValue = "04";
                this.ddlEndAMPM.SelectedValue = "PM";

                this.btnBack.PostBackUrl = "~/AssociationInsureds.aspx?aid=" + Request.QueryString["aid"];
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.UnmarkControls();

                if (UIValidation() == false)
                {
                    return;
                }

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

                if (biz.InsertInsured(this.txtInsuredName.Text, 
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
                                     this.txtBusinessDescription.Text) == true)
                {
                    this.ucMessanger1.ProcessMessage("Insured has been succesfully inserted.", Utilities.enMsgType.OK, "", null, true);
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtAccountExecutive.Text = "";
                this.txtClass.Text = "";
                this.txtIndemnityLimit.Amount = 0;
                this.txtInsuredName.Text = "";
                this.txtInsurer.Text = "";
                this.txtOAMPSEmail.Text = "";
                this.txtPolicyNumber.Text = "";

                if (this.ddlTimeZone.Items.Count >= 1) this.ddlTimeZone.SelectedIndex = 0;

                this.txtStart.Text = "";
                this.txtEnd.Text = "";
                this.ddlStartHour.SelectedValue = "04";
                this.ddlStartAMPM.SelectedValue = "PM";
                this.ddlEndHour.SelectedValue = "04";
                this.ddlEndAMPM.SelectedValue = "PM";
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }
    }
}
