using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Text;
using System.Collections.Specialized;

namespace WebControls.NumericBox
{
	/// <summary>
	/// A text input control for display and entry of percentage values.
	/// </summary>
	///
	[
	DefaultProperty("Amount"),
	DefaultEvent("AmountChanged"),
	ToolboxBitmap(typeof(PercentBox)),
	ToolboxData("<{0}:PercentBox runat=server></{0}:PercentBox>"),
    //Used for validation controls
    ValidationProperty("Amount")
	]
	public class PercentBox : NumericControl,	IPostBackDataHandler
	{

		#region .Declarations 

		// Constants
		private const string PERCENT_SCRIPTS = "PercentScripts";

		#endregion // Declarations

		#region .Constructor 

		/// <summary>
		/// Initialises a new instance of the PercentBox class.
		/// </summary>
		public PercentBox() : base()
		{
		}


		#endregion // Constructor

		#region .Properties 

		/// <summary>
		/// Gets the amount as a string.
		/// </summary>
		/// <remarks>
		/// It returns a string representation of the Amount property formatted in 
		/// accordance with the current culture.
		/// </remarks>
		[
		Browsable(false)
		]
		public string Text
		{
			get
			{
				return Amount.ToString(string.Format("p{0}",Precision));
			}
		}


		#endregion //Properties

		#region .IPostBackDataHandler interface 

		/// <summary>
		/// Processes post back data for the control.
		/// </summary>
		/// <param name="postDataKey">
		/// The key identifier for the control.
		/// </param>
		/// <param name="postCollection">
		/// The collection of all incoming name values.
		/// </param>
		/// <returns>
		/// true if the Amount changes as a result of the post back; otherwise false.
		/// </returns>
		/// <remarks>
		/// The value (a formatted percent string) is first parsed to a decimal then
		/// compared with the Amount.  If the value cannot be parsed, the Amount is
		/// not updated.
		/// </remarks>
		public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			// Get the percent value
			string percent = postCollection[postDataKey];
			try
			{
				// Remove the percent symbol
				percent = percent.Replace
					(Thread.CurrentThread.CurrentCulture.NumberFormat.PercentSymbol, "");
				// Parse the percent to a decimal
				decimal amount = decimal.Parse(percent, NumberStyles.Number) / 100;
				// Compare with the current Amount
				if (amount != Amount)
				{
					Amount = amount;
					return true;
				}
				else
					return false;
			}
			catch
			{
				// Just in case
				return false;
			}
		}

		/// <summary>
		/// Notifies the control that the Amount has changed.
		/// </summary>
		public void RaisePostDataChangedEvent()
		{
			// Raise the AmountChanged event
			OnAmountChanged(new EventArgs());
		}


		#endregion // IPostBackDataHandler

		#region .Protected methods 

		/// <summary>
		/// Adds HTML attributes and styles that need to be rendered.
		/// </summary>
		/// <param name="writer">
		/// A System.Web.UI.HtmlTextWriter that represents the output stream to render 
		/// HTML content on the client
		/// </param>
		/// <remarks>
		/// The method overrides the base class to add the attributes necessary to 
		/// render the control as as text input.
		/// </remarks>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			// Add attributes necessary to display an text control
			writer.AddAttribute(HtmlTextWriterAttribute.Value, Text);
			// Add client side event handlers
			string onFocus = "FormatPercentAsDecimal(this)";
			if (OnFocus != string.Empty)
			{
				onFocus += "," + OnFocus;
			}
			string onBlur = "FormatDecimalAsPercent(this)";
			if (OnBlur != string.Empty)
			{
				onBlur += "," + OnBlur;
			}
			writer.AddAttribute("onfocus", onFocus);
			writer.AddAttribute("onblur", onBlur);
		}

		/// <summary>
		/// Raises the PreRender event.
		/// </summary>
		/// <param name="e">
		/// A System.EventArgs that contains event information.
		/// </param>
		/// <remarks>
		/// The method overrides the base class to register client side scripts and add 
		/// client side event handlers.
		/// </remarks>
		protected override void OnPreRender(EventArgs e)
		{
			// Register the controls formatting scripts
			RegisterFormatPercentScripts(Page);
			// Register the PercentToDecimal script
			Scripts.RegisterPercentToDecimalScript(Page);
			// Register the DecimalToPercent script
			Scripts.RegisterDecimalToPercentScript(Page);
			// Call the base method
			base.OnPreRender(e);
		}


		#endregion // Protected methods

		#region .Private methods 

		/// <summary>
		///  Registers the FormatDecimalAsPercent() and FormatPercentAsDecimal()
		///  script functions.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The FormatDecimalAsPercent() function formats the text in the control when
		/// it loses focus. It calls the DecimalToPercent function.
		/// The FormatPercentAsDecimal() function formats the text in the control when
		/// it receives focus. It calls the PercentToDecimal function.
		/// </remarks>
		private void RegisterFormatPercentScripts(Page page)
		{
			// Determine if the scripts have been registered
			if (!page.IsClientScriptBlockRegistered(PERCENT_SCRIPTS))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script
				script.Append(Environment.NewLine);
				script.Append("<script language=javascript>");
				// Hide script from non-compliant browsers
				script.Append(Environment.NewLine);
				script.Append("<!--");
				// Declare the FormatDecimalAsPercent function
				script.Append(Environment.NewLine);
				script.Append("function FormatDecimalAsPercent(c)");
				script.Append(Environment.NewLine);
				script.Append("{");
				// Check if the value is within range
				script.Append("var max=new Number(c.getAttribute('maxAmount'))*100;");
				script.Append("if(c.value>max){c.value=max}");
				script.Append("var min=new Number(c.getAttribute('minAmount'))*100;");
				script.Append("if(c.value<min){c.value=min}");
				// Convert the controls text to a percentage string
				script.Append("c.value=DecimalToPercent(c.value,");
				script.Append(Precision.ToString());
				script.Append(");");
				// Set the text colour
				script.Append("c.style.color=");
				script.Append(@"(c.value.match(/\x");
				script.Append(minusSign);
				script.Append("/)==null?");
				script.Append("c.getAttribute(\"positiveColor\"):");
				script.Append("c.getAttribute(\"negativeColor\"));");
				// End the function
				script.Append("}");
				// Declare the FormatDecimalAsPercent function
				script.Append(Environment.NewLine);
				script.Append("function FormatPercentAsDecimal(c)");
				script.Append(Environment.NewLine);
				script.Append("{");
				// Convert the controls text to a decimal
				script.Append("c.value=PercentToDecimal(c.value);");
				// Set the colour to black while editing (since we don't know yet if its
				// going to be positive or negative)
				script.Append("c.style.color=\"black\";");
				// Select the value (the default behaviour is over-type mode)
				script.Append("c.select();");
				// End the function
				script.Append("}");
				// End hiding script from non-compliant browsers
				script.Append(Environment.NewLine);
				script.Append("// -->");
				// End the script
				script.Append(Environment.NewLine);
				script.Append("</script>");
				// Register the script
				page.RegisterClientScriptBlock(PERCENT_SCRIPTS, script.ToString());
			}
		}


		#endregion // Private methods

	}
}
