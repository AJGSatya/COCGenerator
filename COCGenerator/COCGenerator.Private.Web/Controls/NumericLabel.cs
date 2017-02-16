using System;
using System.Collections.Generic;
using System.Text;

namespace WebControls.NumericBox
{
   
    public class NumericLabel: System.Web.UI.WebControls.Label
    {  
        public enum Type
        { 
            Currency,
            Percentage
        }
        public Type DisplayType
        {
            get
            {
                if (ViewState["DisplayType"] != null)
                    return (Type)ViewState["DisplayType"];
                return Type.Currency;
            }
            set { ViewState["DisplayType"] = value; }
        
        }
        public decimal Amount
        {
            get
            {
                if (this.Text.Trim().Length == 0)
                    return 0;
                decimal amount = Convert.ToDecimal(base.Text);
                return amount;
            }
            set { base.Text = value.ToString(); }
        
        }
        public override string Text
        {
            get
            {
                decimal amount = 0;
                string returnText = string.Empty;
                if (base.Text.Trim().Length > 0)
                    amount = Convert.ToDecimal(base.Text);
                switch (this.DisplayType)
                {
                    case Type.Currency:
                        returnText = string.Format("{0:C}", amount); 
                        break;
                    case Type.Percentage:
                        returnText = string.Format("{0:P}", amount);
                        break;
                }
                return returnText;
            }
        }
        //public bool IsBold
        //{
        //    get
        //    {
        //        if (ViewState["IsBold"] != null)
        //            return (bool)ViewState["IsBold"];
        //        return false;
        //    }
        //    set
        //    {
        //        ViewState["IsBold"] = value;
        //    }
        //}
    }
}
