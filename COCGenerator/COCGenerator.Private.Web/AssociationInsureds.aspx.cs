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
    public partial class AssociationInsureds : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["aid"] == null)
                {
                    this.ucMessanger1.ProcessMessage("Invalid link.", Utilities.enMsgType.Err, "", null, true);
                    return;
                }

                this.btnAdd.PostBackUrl = "~/InsuredAdd.aspx?aid=" + Request.QueryString["aid"];

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

        protected void grvInsured_DataBound(object sender, EventArgs e)
        {
            try
            {
                if (this.grvInsureds.Rows.Count == 0)
                {
                    this.lblInsuredMessage.Visible = true;
                }
                else
                {
                    this.lblInsuredMessage.Visible = false;
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvInsureds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    HyperLink hpl = (HyperLink)e.Row.Cells[0].FindControl("hplView");
                    hpl.NavigateUrl = "~/InsuredView.aspx?iid=" + ((DataRowView)e.Row.DataItem)["InsuredID"];
                }
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }

        protected void grvInsureds_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                bizInsured biz = new bizInsured();
                if (biz.DeleteInsured(Convert.ToInt32(this.grvInsureds.DataKeys[e.RowIndex].Value)) == true)
                {
                    this.ucMessanger1.ProcessMessage("Insured has been succesfully deleted.", Utilities.enMsgType.OK, "", null, false);
                }
                else
                {
                    this.ucMessanger1.ProcessMessage("An error has happened, please see the log.", Utilities.enMsgType.Err, "", null, true);
                }

                e.Cancel = true;
                this.grvInsureds.EditIndex = -1;
                this.grvInsureds.DataBind();
            }
            catch (Exception ex)
            {
                bizLog.InsertExceptionLog(ex.Message, ex.StackTrace);
                Response.Redirect("~/ErrorPage.aspx", false);
            }
        }
    }
}
