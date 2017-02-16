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
    public partial class AssociationPresets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["aid"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                    this.btnAddPreset.Visible = false;
                    return;
                }

                bizAssociation biz = new bizAssociation();
                DataSet ds;
                ds = biz.GetAssociation(Convert.ToInt32(Request.QueryString["aid"]));

                if (ds == null) return;

                this.lblAssociation.Text = ds.Tables[0].Rows[0]["AssociationName"].ToString();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvPresets_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (this.grvPresets.Rows.Count == 0)
                {
                    this.lblPresetMessage.Visible = true;
                }
                else
                {
                    this.lblPresetMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnAddPreset_Click(object sender, EventArgs e)
        {
            try
            {
                HttpPostedFile pf;
                pf = this.fulAddTemplate.PostedFile;

                if (pf.FileName == "")
                {
                    this.ucMessanger1.ProcessMessage("No Template file has been selected.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                if (pf.ContentType != "application/pdf")
                {
                    this.ucMessanger1.ProcessMessage("You have to select PDF file as a template.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                string fileName = Path.GetFileName(pf.FileName);

                int len = this.fulAddTemplate.PostedFile.ContentLength;
                byte[] template = new byte[len];
                this.fulAddTemplate.PostedFile.InputStream.Read(template, 0, len);

                bizPreset biz = new bizPreset();

                if (biz.ValidatePreset(Convert.ToInt32(Request.QueryString["aid"]),
                                       fileName,
                                       this.ddlAddDocumentType.SelectedValue) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, false);
                    return;
                }

                if (biz.InsertPreset(Convert.ToInt32(Request.QueryString["aid"]),
                                     fileName,
                                     template,
                                     this.ddlAddDocumentType.SelectedValue) == true)
                {
                    this.ucMessanger1.ProcessMessage("Preset has been succesfully inserted.", Utilities.enMsgType.OK, "", null, false);
                    this.grvPresets.DataBind();
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

        protected void grvPresets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                bizPreset biz = new bizPreset();
                if (biz.DeletePreset(Convert.ToInt32(this.grvPresets.DataKeys[e.RowIndex].Value)) == true)
                {
                    this.ucMessanger1.ProcessMessage("Preset has been succesfully deleted.", Utilities.enMsgType.OK, "", null, true);
                    this.ucMessanger1.ProcessMessage("If there is existing link to this Preset, it will be broken.", Utilities.enMsgType.Warn, "", null, false);
                }
                else
                {
                    this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);
                }

                e.Cancel = true;
                this.grvPresets.EditIndex = -1;
                this.grvPresets.DataBind();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
