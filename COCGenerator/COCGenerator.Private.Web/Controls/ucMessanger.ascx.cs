﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;

namespace Web.Controls
{
    public partial class ucMessanger : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ClearMessages()
        {
            this.Controls.Clear();
        }

        public void ProcessMessage(string message, Utilities.enMsgType messageType, string controlName, Type controlType, bool clearPrevious)
        {
            if (clearPrevious == true)
            {
                this.Controls.Clear();
            }

            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            Label lbl = new Label();
            HtmlGenericControl br = new HtmlGenericControl("BR");
            img.ImageAlign = ImageAlign.TextTop;
            lbl.Text = " " + message;
            switch (messageType)
            {
                case Utilities.enMsgType.OK:
                    img.ImageUrl = "~/Content/media/info16x16.jpg";
                    lbl.ForeColor = Color.Green;
                    break;
                case Utilities.enMsgType.Warn:
                    img.ImageUrl = "~/Content/media/warning16x16.jpg";
                    lbl.ForeColor = Color.Orange;
                    break;
                case Utilities.enMsgType.Err:
                    img.ImageUrl = "~/Content/media/error16x16.jpg";
                    lbl.ForeColor = Color.Red;
                    break;
            }
            this.Controls.Add(img);
            this.Controls.Add(lbl);
            this.Controls.Add(br);

            if (controlName != "")
            {
                MarkControl(controlName, controlType, messageType);
            }
        }

        public void ProcessMessages(Utilities.RTNMessages MSGS, bool clearPrevious)
        {
            if (clearPrevious == true)
            {
                this.Controls.Clear();
            }

            foreach (Utilities.RTNMessage MSG in MSGS)
            {
                System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                Label lbl = new Label();
                HtmlGenericControl br = new HtmlGenericControl("BR");
                img.ImageAlign = ImageAlign.TextTop;
                lbl.Text = " " + MSG.Message;
                switch (MSG.MessageType)
                {
                    case Utilities.enMsgType.OK:
                        img.ImageUrl = "~/Content/media/info16x16.jpg";
                        lbl.ForeColor = Color.Green;
                        break;
                    case Utilities.enMsgType.Warn:
                        img.ImageUrl = "~/Content/media/warning16x16.jpg";
                        lbl.ForeColor = Color.Orange;
                        break;
                    case Utilities.enMsgType.Err:
                        img.ImageUrl = "~/Content/media/error16x16.jpg";
                        lbl.ForeColor = Color.Red;
                        break;
                }
                this.Controls.Add(img);
                this.Controls.Add(lbl);
                this.Controls.Add(br);

                if (MSG.LinkedControlName != "")
                {
                    MarkControl(MSG.LinkedControlName, MSG.LinkedControlType, MSG.MessageType);
                }
            }
        }

        private void MarkControl(string controlName, Type controlType, Utilities.enMsgType messageType)
        {
            switch (controlType.Name)
            {
                case "TextBox":
                    TextBox txt;
                    txt = ((TextBox)this.Parent.FindControl("txt" + controlName));
                    if (messageType == Utilities.enMsgType.Err)
                    {
                        txt.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        txt.CssClass = "validation-warn";
                    }
                    break;
                case "DropDownList":
                    DropDownList ddl;
                    ddl = ((DropDownList)this.Parent.FindControl("ddl" + controlName));
                    if (messageType == Utilities.enMsgType.Err)
                    {
                        ddl.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        ddl.CssClass = "validation-warn";
                    }
                    break;
                case "RadioButtonList":
                    RadioButtonList rbl;
                    rbl = ((RadioButtonList)this.Parent.FindControl("rbl" + controlName));
                    if (messageType == Utilities.enMsgType.Err)
                    {
                        rbl.CssClass = "validation-err";
                    }
                    else // warning
                    {
                        rbl.CssClass = "validation-warn";
                    }
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        public void UnmarkControls()
        {
            foreach (Control c in this.Parent.Controls)
            {
                switch (c.GetType().Name)
                {
                    case "TextBox":
                        TextBox txt;
                        txt = (TextBox)c;
                        txt.CssClass = "";
                        break;
                    case "DropDownList":
                        DropDownList ddl;
                        ddl = (DropDownList)c;
                        ddl.CssClass = "";
                        break;
                    case "RadioButtonList":
                        RadioButtonList rbl;
                        rbl = (RadioButtonList)c;
                        rbl.CssClass = "";
                        break;
                }
            }
        }

    }
}