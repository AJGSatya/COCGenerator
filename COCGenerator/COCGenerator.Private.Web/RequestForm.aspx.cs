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
    public partial class RequestForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack || true == true)
                {
                    if (this.Request.QueryString["pid"] == null)
                    {
                        this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                        this.btnSubmit.Visible = false;
                        this.btnClear.Visible = false;
                        return;
                    }

                    bizPreset bizP = new bizPreset();

                    DataSet ds;
                    ds = bizP.GetPreset(Convert.ToInt32(this.Request.QueryString["pid"]));
                    this.ucMessanger1.ProcessMessages(bizP.MSGS, true);

                    if (ds == null) return;

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        this.btnSubmit.Visible = false;
                        this.btnClear.Visible = false;
                        this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                        return;
                    }

                    DataRow drOriginalPreset = ds.Tables[0].Rows[0];

                    string actualDocumentType = drOriginalPreset["DocumentType"].ToString();
                    if (rblInterestedParty.SelectedValue != null) actualDocumentType = rblInterestedParty.SelectedValue;


                    int aid = Convert.ToInt32(drOriginalPreset["AssociationID"].ToString());
                    DataSet dsAssociationPresets;
                    dsAssociationPresets = bizP.ListPresetsByAssociation(aid);
                    var interestedPartyPresets = dsAssociationPresets.Tables[0].AsEnumerable().Select(r => new {
                        PresetID = r.Field<int>("PresetID"),
                        AssociationName = r.Field<string>("AssociationName"),
                        TemplateName = r.Field<string>("TemplateName"),
                        DocumentType = r.Field<string>("DocumentType")
                    })
                        .Where(r => r.DocumentType == actualDocumentType)
                        .OrderByDescending(r => r.PresetID)
                        .GetEnumerator();

                    if (!interestedPartyPresets.MoveNext())
                    {
                        this.ucMessanger1.ProcessMessage("Invalid selection", Utilities.enMsgType.Err, "", null, true);
                        this.btnSubmit.Visible = false;
                        this.btnClear.Visible = false;
                        hidActualPreset.Value = this.Request.QueryString["pid"];
                    }
                    else
                    {
                        var thisPreset = interestedPartyPresets.Current;
                        hidActualPreset.Value = thisPreset.PresetID.ToString();
                    }

                    if (this.Request.QueryString["debug"] != null)
                    {
                        Response.Write("Actual Document type: " + actualDocumentType);
                        Response.Write("Actual preset: " + hidActualPreset.Value);

                    }


                    this.lblFormTitle.Text = "Request for " + actualDocumentType;
                    this.imgAssociationLogo.ImageUrl = "ImageHandler.ashx?aid=" + drOriginalPreset["AssociationID"].ToString();

                    DataSet ads;
                    bizAssociation bizA = new bizAssociation();
                    ads = bizA.GetAssociation(aid);

                    if (ads == null) return;

                    DataRow adr = ads.Tables[0].Rows[0];
                    this.lblHeaderTitle.Text = adr["AssociationName"].ToString();

                    DataSet ids;
                    bizInsured bizI = new bizInsured();
                    ids = bizI.ListInsuredsByAssociation(Convert.ToInt32(drOriginalPreset["AssociationID"]));
                    this.ucMessanger1.ProcessMessages(bizP.MSGS, false);

                    if (ids == null) return;

                    if (ids.Tables[0].Rows.Count == 0)
                    {
                        this.btnSubmit.Visible = false;
                        this.btnClear.Visible = false;
                        this.ucMessanger1.ProcessMessage("There are no Insureds for " + adr["AssociationName"].ToString() + ".", Utilities.enMsgType.Err, "", null, true);
                        return;
                    }
                    
                    //this.ddlInsured.Items.Add(new ListItem(" -- please select -- ", ""));
                    foreach (DataRow idr in ids.Tables[0].Rows)
                    {
                        this.lstInsured.Items.Add(new ListItem(idr["InsuredName"].ToString(), idr["InsuredID"].ToString()));
                        this.ddlInsured.Items.Add(new ListItem(idr["InsuredName"].ToString(), idr["InsuredID"].ToString()));
                    }
                    this.lstInsured.Width = new Unit("356px");
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (UIValidation() == false) return;

                DataSet pds;
                bizPreset bizP = new bizPreset();
                //pds = bizP.GetPreset(Convert.ToInt32(this.Request.QueryString["pid"]));
                pds = bizP.GetPreset(Convert.ToInt32(hidActualPreset.Value));

                if (pds == null) return;

                DataRow pdr = pds.Tables[0].Rows[0];

                DataSet ds;
                bizInsured bizI = new bizInsured();
                ds = bizI.GetInsured(Convert.ToInt32(this.lstInsured.SelectedValue));
                this.ucMessanger1.ProcessMessages(bizI.MSGS, true);

                if (ds == null) return;

                DataRow drSubmitPreset = ds.Tables[0].Rows[0];

                bizDocument biz = new bizDocument();

                if (biz.ValidateRequest(this.rblInterestedParty.SelectedValue == "Certificate of Currency 3rd Party" ? true : false,
                                        this.txtInterestedPartyName.Text,
                                        this.txtRequestedBy.Text,
                                        this.txtEmail.Text,
                                        this.txtPositionAtInsured.Text) == false)
                {
                    this.ucMessanger1.ProcessMessages(biz.MSGS, true);
                    return;
                }

                if (biz.ProcessRequest(this.lblHeaderTitle.Text, 
                                       this.rblInterestedParty.SelectedValue == "Certificate of Currency 3rd Party" ? true : false,
                                       this.txtInterestedPartyName.Text,
                                       this.txtRequestedBy.Text,
                                       this.txtEmail.Text,
                                       this.txtPositionAtInsured.Text,
                                       pdr["TemplateName"].ToString(),
                                       (byte[])pdr["Template"],
                                       pdr["DocumentType"].ToString(),
                                       this.lstInsured.SelectedItem.Text,
                                       drSubmitPreset["AccountExecutive"].ToString(),
                                       drSubmitPreset["Insurer"].ToString(),
                                       Convert.ToDateTime(drSubmitPreset["InsurancePeriodStart"]),
                                       Convert.ToDateTime(drSubmitPreset["InsurancePeriodEnd"]),
                                       drSubmitPreset["TimeZone"].ToString(),
                                       drSubmitPreset["Class"].ToString(),
                                       drSubmitPreset["PolicyNumber"].ToString(),
                                       Convert.ToDecimal(drSubmitPreset["IndemnityLimit"]),
                                       drSubmitPreset["OAMPSEmail"].ToString(),
                                       drSubmitPreset["BusinessDescription"].ToString()) == true)
                {
                    ClearData();
                    if (this.rblInterestedParty.SelectedValue == "Certificate of Currency 3rd Party")
                    {
                        this.ucMessanger1.ProcessMessage("Request has been succesfully submitted. Requested document has to be approved by Arthur J. Gallagher & Co. when an interested party is involved.", Utilities.enMsgType.OK, "", null, true);
                    }
                    else
                    {
                        this.ucMessanger1.ProcessMessage("Request has been succesfully submited. You will get the requested document in your e-mail shortly.", Utilities.enMsgType.OK, "", null, true);
                    }
                }
                else
                {
                    this.ucMessanger1.ProcessMessage(string.Format("27 {0}", bizEmail.GetEmailValue("ErrorMessage")), Utilities.enMsgType.Err, "", null, true);
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

            if (this.lstInsured.SelectedValue == "")
            {
                this.ucMessanger1.ProcessMessage("You have to select Insured.", Utilities.enMsgType.Err, "Insured", typeof(DropDownList), true);
                return false;
            }
            if (this.rblInterestedParty.SelectedValue == "Certificate of Currency 3rd Party" && this.txtInterestedPartyName.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Interested Party Name.", Utilities.enMsgType.Err, "InterestedPartyName", typeof(TextBox), true);
                return false;
            }
            if (this.txtRequestedBy.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Requested By.", Utilities.enMsgType.Err, "RequestedBy", typeof(TextBox), true);
                return false;
            }
            if (this.txtEmail.Text == "")
            {
                this.ucMessanger1.ProcessMessage("You have to enter Email.", Utilities.enMsgType.Err, "Email", typeof(TextBox), true);
                return false;
            }
            return true;
        }

        protected void rblInterestedParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.rblInterestedParty.SelectedValue == "Certificate of Currency 3rd Party")
                {
                    this.txtInterestedPartyName.Enabled = true;
                }
                else
                {
                    this.txtInterestedPartyName.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearData();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        private void ClearData()
        {
            this.ucMessanger1.ClearMessages();
            this.txtSearch.Value = "";
            if (this.lstInsured.Items.Count > 0) this.lstInsured.SelectedIndex = 0;
            this.rblInterestedParty.SelectedValue = "Certificate of Currency";
            this.txtInterestedPartyName.Text = "";
            this.txtInterestedPartyName.Enabled = false;
            this.txtRequestedBy.Text = "";
            this.txtEmail.Text = "";
            this.txtPositionAtInsured.Text = "";
        }

    }
}
