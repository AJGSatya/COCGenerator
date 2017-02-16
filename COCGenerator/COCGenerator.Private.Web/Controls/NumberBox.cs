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
  
[assembly: TagPrefix("OAMPS.JQS.WebControls", "oibit")]
namespace WebControls.NumericBox
{
	/// <summary>
	/// A text input control for display and entry of numeric values.
	/// </summary>
	[
	DefaultProperty("Amount"),
	DefaultEvent("AmountChanged"),
	ToolboxBitmap(typeof(NumberBox)),
	ToolboxData("<{0}:NumberBox runat=server></{0}:NumberBox>"),
    ValidationProperty("Amount")////Used for validation controls
	]
	public class NumberBox : NumericControl,	IPostBackDataHandler
	{

		#region .Declarations 

		// Constants
		private const string NUMBER_SCRIPTS = "NumberScripts";

		#endregion // Declarations

		#region .Constructor 

		/// <summary>
		/// Initialises a new instance of the NumberBox class.
		/// </summary>
		public NumberBox() : base()
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
				return Amount.ToString(string.Format("n{0}",Precision));
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
		public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			// Get the number value
			string number = postCollection[postDataKey];
			try
			{
				// Parse the number to a decimal
				decimal amount = decimal.Parse(number, NumberStyles.Number);
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
			string onFocus = "FormatNumberAsDecimal(this)";
			if (OnFocus != string.Empty)
			{
				onFocus += "," + OnFocus;
			}
			string onBlur = "FormatDecimalAsNumber(this)";
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
			RegisterFormatNumberScripts(Page);
			// Register the NumberToDecimal script
			Scripts.RegisterNumberToDecimalScript(Page);
			// Register the DecimalToNumber script
			Scripts.RegisterDecimalToNumberScript(Page);
			// Call the base method
			base.OnPreRender(e);
		}


		#endregion // Protected methods

		#region .Private methods 

		/// <summary>
		///  Registers the FormatDecimalAsNumber() and FormatNumberAsDecimal()
		///  script functions.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The FormatDecimalAsNumber() function formats the text in the control when
		/// it loses focus. It calls the DecimalToNumber function.
		/// The FormatNumberAsDecimal() function formats the text in the control when
		/// it receives focus. It calls the NumberToDecimal function.
		/// </remarks>
		private void RegisterFormatNumberScripts(Page page)
		{
			// Determine if the scripts have been registered
			if (!page.IsClientScriptBlockRegistered(NUMBER_SCRIPTS))
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
				// Declare the FormatDecimalAsNumber function
				script.Append(Environment.NewLine);
				script.Append("function FormatDecimalAsNumber(c)");
				script.Append(Environment.NewLine);
				script.Append("{");
				// Check if the value is within range
				script.Append("var max=new Number(c.getAttribute('maxAmount'));");
				script.Append("if(c.value>max){c.value=max}");
				script.Append("var min=new Number(c.getAttribute('minAmount'));");
				script.Append("if(c.value<min){c.value=min}");
				// Convert the controls text to a number string
				script.Append("c.value=DecimalToNumber(c.value,");
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
				// Declare the FormatNumberAsDecimal function
				script.Append(Environment.NewLine);
				script.Append("function FormatNumberAsDecimal(c)");
				script.Append(Environment.NewLine);
				script.Append("{");
				// Convert the controls text to a decimal
				script.Append("c.value=NumberToDecimal(c.value);");
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
				page.RegisterClientScriptBlock(NUMBER_SCRIPTS, script.ToString());
			}
		}


		#endregion // Private methods

	}
}
