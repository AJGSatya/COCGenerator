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
    public partial class AdminHome : System.Web.UI.Page
    {

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //if (DNN.IsUserLoggedIn(bizEmail.GetEmailValue("DNNApp"), bizEmail.GetEmailValue("DNNAcc")) == false)
                //{
                //    Response.Redirect("~/NotAuthorised.aspx", false);
                //}
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }

        protected void grvAssociation_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (this.grvAssociation.Rows.Count == 0)
                {
                    this.lblAssociationMessage.Visible = true;
                }
                else
                {
                    this.lblAssociationMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvTimeZone_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (this.grvTimeZone.Rows.Count == 0)
                {
                    this.lblTimeZoneMessage.Visible = true;
                }
                else
                {
                    this.lblTimeZoneMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        #endregion 

        #region Insert

        protected void btnAddAssociation_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.UnmarkControls();
                if (this.txtAddAssociation.Text == "")
                {
                    this.ucMessanger1.ProcessMessage("You have to enter Association Name.", Utilities.enMsgType.Err, "AddAssociation", typeof(TextBox), true);
                    return;
                }

                if (this.fulAddLogo.PostedFile.FileName == "")
                {
                    this.ucMessanger1.ProcessMessage("You have to select Association Logo.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                int len = this.fulAddLogo.PostedFile.ContentLength;
                byte[] pic = new byte[len];
                this.fulAddLogo.PostedFile.InputStream.Read (pic, 0, len);

                bizAssociation biz = new bizAssociation();
                if (biz.InsertAssociation(this.txtAddAssociation.Text, pic) == true)
                {
                    this.txtAddAssociation.Text = "";
                    this.ucMessanger1.ProcessMessage("Association has been succesfully inserted.", Utilities.enMsgType.OK, "", null, true);
                    this.grvAssociation.DataBind();
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
     
        protected void btnAddTZ_Click(object sender, EventArgs e)
        {
            try
            {
                this.ucMessanger1.UnmarkControls();

                if (this.txtAddTimeZone.Text == "")
                {
                    this.ucMessanger1.ProcessMessage("You have to enter Time Zone.", Utilities.enMsgType.Err, "AddTimeZone", typeof(TextBox), true);
                    return;
                }

                bizTimeZone biz = new bizTimeZone();
                if (biz.InsertTimeZone(this.txtAddTimeZone.Text) == true)
                {
                    this.txtAddTimeZone.Text = "";
                    this.ucMessanger1.ProcessMessage("Time Zone has been succesfully inserted.", Utilities.enMsgType.OK, "", null, true);
                    this.grvTimeZone.DataBind();
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

        #endregion

        #region Delete

        protected void grvTimeZone_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                this.ucMessanger1.UnmarkControls();

                bizTimeZone biz = new bizTimeZone();
                if (biz.DeleteTimeZone(this.grvTimeZone.DataKeys[e.RowIndex].Value.ToString()) == true)
                {
                    this.ucMessanger1.ProcessMessage("Time Zone has been succesfully deleted.", Utilities.enMsgType.OK, "", null, true);
                }
                else
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                }

                e.Cancel = true;
                this.grvTimeZone.EditIndex = -1;
                this.grvTimeZone.DataBind();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvAssociation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                this.ucMessanger1.UnmarkControls();

                bizAssociation biz = new bizAssociation();
                if (biz.DeleteAssociation(Convert.ToInt32(this.grvAssociation.DataKeys[e.RowIndex].Value)) == true)
                {
                    this.ucMessanger1.ProcessMessage("Association has been succesfully deleted.", Utilities.enMsgType.OK, "", null, true);
                }
                else
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                }

                e.Cancel = true;
                this.grvAssociation.EditIndex = -1;
                this.grvAssociation.DataBind();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
        
        #endregion

    }
}
