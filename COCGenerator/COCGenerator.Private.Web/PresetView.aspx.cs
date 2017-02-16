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
using TallComponents.Web.PDF;

namespace COCGenerator
{
    public partial class PresetView : System.Web.UI.Page
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
                        return;
                    }

                    this.btnBack.PostBackUrl = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query;

                    DataSet ds;
                    bizPreset biz = new bizPreset();
                    ds = biz.GetPreset(Convert.ToInt32(this.Request.QueryString["pid"]));
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    
                    if (ds == null) return;

                    DataRow dr = ds.Tables[0].Rows[0];
                    this.lblPresetID.Text = dr["PresetID"].ToString();
                    this.lblTemplateName.Text = dr["TemplateName"].ToString();
                    ShowThumbnail((byte[])dr["Template"]);
                    this.lblDocumentType.Text = dr["DocumentType"].ToString();

                    DataSet ads;
                    bizAssociation bizA = new bizAssociation();
                    ads = bizA.GetAssociation(Convert.ToInt32(dr["AssociationId"]));
                    this.ucMessanger1.ProcessMessages(bizA.MSGS, true);

                    if (ads == null) return;

                    if (ads.Tables[0].Rows.Count > 0) this.lblAssociation.Text = ads.Tables[0].Rows[0]["AssociationName"].ToString();
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
    }
}
