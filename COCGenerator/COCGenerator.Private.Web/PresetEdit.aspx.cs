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
using System.Collections.ObjectModel;
using COCGenerator.BusinesObjects;
using TallComponents.Web.PDF;

namespace COCGenerator
{
    public partial class PresetEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (this.Request.QueryString["pid"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                        this.btnEdit.Visible = false;
                        return;
                    }

                    DataSet ds;
                    bizPreset biz = new bizPreset();
                    ds = biz.GetPreset(Convert.ToInt32(this.Request.QueryString["pid"]));

                    if (ds == null) return;

                    DataRow dr = ds.Tables[0].Rows[0];
                    this.ddlAssociations.SelectedValue = dr["AssociationID"].ToString();
                    this.ddlDocumentType.SelectedValue = dr["DocumentType"].ToString();
                    this.lblTemplateName.Text = dr["TemplateName"].ToString();
                    ShowThumbnail((byte[])dr["Template"]);

                    this.btnBack.PostBackUrl = "~/AssociationPresets.aspx?aid=" + dr["AssociationID"].ToString();
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false); 
            }
        }

        private void ShowThumbnail(byte[] pdf)
        {
            thumbnails.Controls.Clear();

            string sessionID = Session.SessionID;
            Session[sessionID] = pdf;

            int n = 1; //Thumbnail.GetPageCount(sr.BaseStream);

            for (int i = 1; i <= n; i++)
            {
                Thumbnail thumbnail = new Thumbnail();
                thumbnail.SessionKey = sessionID;
                thumbnail.Index = i;
                thumbnails.Controls.Add(thumbnail);
            }
        }

        private bool UIValidation()
        {
            this.ucMessanger1.UnmarkControls();

            if (this.ddlAssociations.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("You have to select Association.", Utilities.enMsgType.Err, "Associations", typeof(DropDownList), true);
                return false;
            }
            if (this.ddlDocumentType.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("You have to select Document Type.", Utilities.enMsgType.Err, "DocumentType", typeof(DropDownList), true);
                return false;
            }
            return true;
        }

        protected void btnThumbnail_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.fulTemplate.HasFile == false)
                {
                    this.ucMessanger1.ProcessMessage("No Template file has been selected.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                if (this.fulTemplate.PostedFile.ContentType != "application/pdf")
                {
                    this.ucMessanger1.ProcessMessage("Template file is not a PDF.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                int len = this.fulTemplate.PostedFile.ContentLength;
                byte[] template = new byte[len];
                this.fulTemplate.PostedFile.InputStream.Read(template, 0, len);

                if (Session["pdf"] == null)
                {
                    Session.Add("pdf", template);
                }
                else
                {
                    Session["pdf"] = template;
                }

                thumbnails.Controls.Clear();

                this.lblTemplateName.Text = this.fulTemplate.FileName;
                
                string sessionID = Session.SessionID;
                Session[sessionID] = template;

                int n = 1; // Thumbnail.GetPageCount(this.fulTemplate.FileContent);

                for (int i = 1; i <= n; i++)
                {
                    Thumbnail thumbnail = new Thumbnail();
                    thumbnail.SessionKey = sessionID;
                    thumbnail.Index = i;
                    thumbnails.Controls.Add(thumbnail);
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
            if (UIValidation() == false) return;

            bizPreset biz = new bizPreset();

            if (biz.ValidatePreset(Convert.ToInt32(this.ddlAssociations.SelectedValue),
                                   this.lblTemplateName.Text,
                                   this.ddlDocumentType.SelectedValue) == false)
            {
                this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                return;
            }

            if (Session["pdf"] != null)
            {
                if (biz.UpdatePreset(Convert.ToInt32(this.Request.QueryString["pid"]),
                                     Convert.ToInt32(this.ddlAssociations.SelectedValue),
                                     this.lblTemplateName.Text,
                                     (byte[])Session["pdf"],
                                     this.ddlDocumentType.SelectedValue) == true)
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
                if (biz.UpdatePreset(Convert.ToInt32(this.Request.QueryString["pid"]),
                                     Convert.ToInt32(this.ddlAssociations.SelectedValue),
                                     this.lblTemplateName.Text,
                                     new byte[0],
                                     this.ddlDocumentType.SelectedValue) == true)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                }
                else
                {
                    this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);
                }
            }
        }
    }
}
