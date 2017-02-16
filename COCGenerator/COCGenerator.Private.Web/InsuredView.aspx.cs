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
    public partial class InsuredView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.Request.QueryString["iid"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                this.btnBack.PostBackUrl = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query;

                bizInsured bizI = new bizInsured();
                DataSet ds;
                ds = bizI.GetInsured(Convert.ToInt32(this.Request.QueryString["iid"]));
                this.ucMessanger1.ProcessMessages(bizI.MSGS, true);

                if (ds == null) return;

                DataRow dr = ds.Tables[0].Rows[0];
                this.lblInsuredName.Text = dr["InsuredName"].ToString();
                this.lblAccountExecutive.Text = dr["AccountExecutive"].ToString();
                this.lblOAMPSEmail.Text = dr["OAMPSEmail"].ToString();
                this.lblInsurer.Text = dr["Insurer"].ToString();
                this.lblInsurancePeriodStart.Text = String.Format("{0:dd/MM/yyyy hh tt}", dr["InsurancePeriodStart"]);
                this.lblInsurancePeriodEnd.Text = String.Format("{0:dd/MM/yyyy hh tt}", dr["InsurancePeriodEnd"]);
                this.lblTimeZoneStart.Text = dr["TimeZone"].ToString();
                this.lblTimeZoneEnd.Text = dr["TimeZone"].ToString();
                this.lblClass.Text = dr["Class"].ToString();
                this.lblPolicyNumber.Text = dr["PolicyNumber"].ToString();
                this.lblIndemnityLimit.Text = String.Format("{0:C}", dr["IndemnityLimit"]);
                this.lblBusinessDescription.Text = dr["BusinessDescription"].ToString().Replace("\n", "<br />");

                DataSet ads;
                bizAssociation bizA = new bizAssociation();
                ads = bizA.GetAssociation(Convert.ToInt32(dr["AssociationId"]));
                this.ucMessanger1.ProcessMessages(bizA.MSGS, true);

                if (ads == null) return;

                if (ads.Tables[0].Rows.Count > 0) this.lblAssociation.Text = ads.Tables[0].Rows[0]["AssociationName"].ToString();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
