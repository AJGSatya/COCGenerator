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
using System.IO;
using COCGenerator.BusinesObjects;

namespace COCGenerator
{
    public partial class AssociationEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (this.Request.QueryString["aid"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                        this.btnEdit.Visible = false;
                        return;
                    }

                    DataSet ds;
                    bizAssociation biz = new bizAssociation();
                    ds = biz.GetAssociation(Convert.ToInt32(this.Request.QueryString["aid"]));

                    if (ds == null) return;

                    DataRow dr = ds.Tables[0].Rows[0];
                    this.txtAssociationName.Text = dr["AssociationName"].ToString();
                    this.imgLogo.ImageUrl = "ImageHandler.ashx?aid=" + this.Request.QueryString["aid"];
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

                bizAssociation biz = new bizAssociation();

                if (this.fulAssociationLogo.PostedFile.FileName == "")
                {
                    if (biz.UpdateAssociation(Convert.ToInt32(this.Request.QueryString["aid"]),
                                              this.txtAssociationName.Text,
                                              new byte[0]) == true)
                    {
                        this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    }
                    else
                    {
                        this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);
                    }
                }
                else
                {
                    if (biz.UpdateAssociation(Convert.ToInt32(this.Request.QueryString["aid"]),
                                              this.txtAssociationName.Text,
                                              this.fulAssociationLogo.FileBytes) == true)
                    {
                        this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    }
                    else
                    {
                        this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);
                    }
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
            this.ucMessanger1.UnmarkControls();

            if (this.txtAssociationName.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Association Name.", Utilities.enMsgType.Err, "AssociationName", typeof(TextBox), true);
                return false;
            }

            return true;
        }
    }
}
