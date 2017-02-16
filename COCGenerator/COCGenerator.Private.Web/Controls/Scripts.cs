using System;
using System.Text;
using System.Web.UI;
using System.Threading;
using System.Globalization;


namespace WebControls.NumericBox
{
	/// <summary>
	/// Contains the static methods for registering client side scripts
	/// used by controls in the SandTrap.WebControls namespace.
	/// </summary>
	/// <remarks>
	/// These scripts are in a separate class so that they can be called
	/// form other assemblies.
	/// </remarks>
	public class Scripts
	{

		#region .Declarations 

		// Constants
		private const string DECIMAL_TO_CURRENCY = "DecimalToCurrency";
		private const string CURRENCY_TO_DECIMAL = "CurrencyToDecimal";
		private const string DECIMAL_TO_PERCENT = "DecimalToPercent";
		private const string PERCENT_TO_DECIMAL = "PercentToDecimal";
		private const string ENSURE_NUMERIC = "EnsureNumeric";
		private const string DECIMAL_TO_NUMBER = "DecimalToNumber";
		private const string NUMBER_TO_DECIMAL = "NumberToDecimal";

		#endregion // Declarations

		#region .Static methods 

		/// <summary>
		/// Registers the CurrencyToDecimal() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a currency string to a decimal value.
		/// </remarks>
		public static void RegisterCurrencyToDecimalScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(CURRENCY_TO_DECIMAL))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(CURRENCY_TO_DECIMAL, "n"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Some languages (arabic) include the decimal separator in the currency symbol
				if (format.CurrencySymbol.IndexOf(format.CurrencyDecimalSeparator) >= 0)
				{
					// Delete the currency symbol
					script.Append("n=n.replace(/");
					script.Append(format.CurrencySymbol);
					script.Append("/,'');");
				}
				// Get the currency negative pattern
				int negPat = format.CurrencyNegativePattern;
				// If negative numbers use parenthesis, append the minus sign if appropriate
				if (negPat == 0||negPat == 4||negPat == 14||negPat == 15)
				{
					script.Append(@"if (n.match(/\(/)!=null) {n='\x");
					script.Append(minusSign);
					script.Append("'+n;}");
				}
				// Delete everything except numbers, the minus sign and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(minusSign);
				script.Append(@"\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Return the decimal value
				script.Append("return n;");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(CURRENCY_TO_DECIMAL, script.ToString());
			}
		}

		/// <summary>
		/// Registers the DecimalToCurrency() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a decimal value to a currency string.
		/// </remarks>
		public static void RegisterDecimalToCurrencyScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(DECIMAL_TO_CURRENCY))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(DECIMAL_TO_CURRENCY, "n", "p"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Set default for the precision
				script.Append("if(p==null){p=2;}");
				// Declare currency symbols
				script.Append("var sy=new Array(");
				script.Append(GetCurrencySymbols(format));
				script.Append(");");
				// Determine if its a negative value
				script.Append(@"var neg=(n.match(/\x");
				script.Append(minusSign);
				script.Append("/)!=null?true:false);");
				// Delete everything except numbers and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Get the components
				script.Append(@"var m=n.match(/(\d*)(\x");
				script.Append(separator);
				script.Append(@"*)(\d*)/);");
				// Get the cents
				script.Append("var f=m[3];");
				script.Append("if(f.length>p)");
				script.Append("{");
				// Divide and round cents to required precision
				script.Append("f=f/Math.pow(10,(f.length-p));");
				script.Append("f=Math.round(f);");
				script.Append("while(f.toString().length<p){f='0'+f};");
				script.Append("}else{");
				// Add zeros to required precision
				script.Append("while(f.toString().length<p){f+='0'};");
				script.Append("}");
				// Get the whole number
				script.Append("var w=new Number(m[1]);");
				script.Append("if(f==Math.pow(10,p)){w+=1;f=f.toString().substr(1);}");
				// Determine if there is grouping
				if (format.CurrencyGroupSizes[0] != 0)
				{
					// Convert whole number to a string
					script.Append("w=w.toString();");
					// Get the percent group separator (hex value)
					string group = ((int)format.CurrencyGroupSeparator[0]).ToString("X");
					// Determine the type of grouping
					if (format.PercentGroupSizes.Length ==1)
					{
						// Declare the size of the group
						script.Append("var s=");
						script.Append(format.CurrencyGroupSizes[0]);
						script.Append(";");
						// Set the location at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
					else
					{
						// Declare an array of currency group sizes
						script.Append("var g=new Array(");
						for (int i=0; i<format.CurrencyGroupSizes.Length; i++)
						{
							if (i>0) {script.Append(",");} // Separate array items
							script.Append(format.CurrencyGroupSizes[i]);
						}
						script.Append(");");
						// Get size of first group
						script.Append("var i=0;"); // array index
						script.Append("var s=g[i];"); // size of first group
						// Set the position at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Get the next size
						script.Append("if(i<g.length-1) {i++;}");
						script.Append("s=g[i];");
						// If the group size is 0, there is no more grouping
						script.Append("if (s==0) {break;}");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
				}
				// Return the currency string
				script.Append(@"if(p==0){m[2]='';f=''}else{m[2]='\x");
				script.Append(separator);
				script.Append("'}");
				script.Append("return (neg?");
				script.Append("sy[2]+w+m[2]+f+sy[3]:");
				script.Append("sy[0]+w+m[2]+f+sy[1]);");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(DECIMAL_TO_CURRENCY, script.ToString());
			}
		}

		/// <summary>
		/// Registers the DecimalToNumber() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a decimal value to a numeric string.
		/// </remarks>
		public static void RegisterDecimalToNumberScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(DECIMAL_TO_NUMBER))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(DECIMAL_TO_NUMBER, "n", "p"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Set default for the precision
				script.Append("if(p==null){p=2;}");
				// Declare negative number symbols
				script.Append("var sy=new Array(");
				script.Append(GetNumberSymbols(format));
				script.Append(");");
				// Determine if its a negative value
				script.Append(@"var neg=(n.match(/\x");
				script.Append(minusSign);
				script.Append("/)!=null?true:false);");
				// Delete everything except numbers and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Get the components
				script.Append(@"var m=n.match(/(\d*)(\x");
				script.Append(separator);
				script.Append(@"*)(\d*)/);");
				// Get the fraction
				script.Append("var f=m[3];");
				script.Append("if(f.length>p)");
				script.Append("{");
				// Divide and round fraction to required precision
				script.Append("f=f/Math.pow(10,(f.length-p));");
				script.Append("f=Math.round(f);");
				script.Append("while(f.toString().length<p){f='0'+f};");
				script.Append("}else{");
				// Add zeros to required precision
				script.Append("while(f.toString().length<p){f+='0'};");
				script.Append("}");
				// Get the whole number
				script.Append("var w=new Number(m[1]);");
				script.Append("if(f==Math.pow(10,p)){w+=1;f=f.toString().substr(1);}");
				// Determine if there is grouping
				if (format.NumberGroupSizes[0] != 0)
				{
					// Convert whole number to a string
					script.Append("w=w.toString();");
					// Get the number group separator (hex value)
					string group = ((int)format.NumberGroupSeparator[0]).ToString("X");
					// Determine the type of grouping
					if (format.NumberGroupSizes.Length ==1)
					{
						// Declare the size of the group
						script.Append("var s=");
						script.Append(format.NumberGroupSizes[0]);
						script.Append(";");
						// Set the location at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
					else
					{
						// Declare an array of number group sizes
						script.Append("var g=new Array(");
						for (int i=0; i<format.NumberGroupSizes.Length; i++)
						{
							if (i>0) {script.Append(",");} // Separate array items
							script.Append(format.NumberGroupSizes[i]);
						}
						script.Append(");");
						// Get size of first group
						script.Append("var i=0;"); // array index
						script.Append("var s=g[i];"); // size of first group
						// Set the position at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Get the next size
						script.Append("if(i<g.length-1) {i++;}");
						script.Append("s=g[i];");
						// If the group size is 0, there is no more grouping
						script.Append("if (s==0) {break;}");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
				}
				// Return the number string
				script.Append(@"if(p==0){m[2]='';f=''}else{m[2]='\x");
				script.Append(separator);
				script.Append("'}");
				script.Append("return (neg?");
				script.Append("sy[0]+w+m[2]+f+sy[1]:");
				script.Append("w+m[2]+f);");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(DECIMAL_TO_NUMBER, script.ToString());
			}
		}

		/// <summary>
		/// Registers the DecimalToPercent() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a decimal value to a percentage string.
		/// </remarks>
		public static void RegisterDecimalToPercentScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(DECIMAL_TO_PERCENT))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(DECIMAL_TO_PERCENT, "n", "p"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Set default for the precision
				script.Append("if(p==null){p=2;}");
				// Declare percent symbols
				script.Append("var sy=new Array(");
				script.Append(GetPercentSymbols(format));
				script.Append(");");
				// Determine if its a negative value
				script.Append(@"var neg=(n.match(/\x");
				script.Append(minusSign);
				script.Append("/)!=null?true:false);");
				// Delete everything except numbers and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Get the components
				script.Append(@"var m=n.match(/(\d*)(\x");
				script.Append(separator);
				script.Append(@"*)(\d*)/);");
				// Get the fraction
				script.Append("var f=m[3];");
				script.Append("if(f.length>p)");
				script.Append("{");
				// Divide and round fraction to required precision
				script.Append("f=f/Math.pow(10,(f.length-p));");
				script.Append("f=Math.round(f);");
				script.Append("while(f.toString().length<p){f='0'+f};");
				script.Append("}else{");
				// Add zeros to required precision
				script.Append("while(f.toString().length<p){f+='0'};");
				script.Append("}");
				// Get the whole number
				script.Append("var w=new Number(m[1]);");
				script.Append("if(f==Math.pow(10,p)){w+=1;f=f.toString().substr(1);}");
				// Determine if there is grouping
				if (format.PercentGroupSizes[0] != 0)
				{
					// Convert whole number to a string
					script.Append("w=w.toString();");
					// Get the percent group separator (hex value)
					string group = ((int)format.PercentGroupSeparator[0]).ToString("X");
					// Determine the type of grouping
					if (format.PercentGroupSizes.Length ==1)
					{
						// Declare the size of the group
						script.Append("var s=");
						script.Append(format.PercentGroupSizes[0]);
						script.Append(";");
						// Set the location at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
					else
					{
						// Declare an array of percent group sizes
						script.Append("var g=new Array(");
						for (int i=0; i<format.PercentGroupSizes.Length; i++)
						{
							if (i>0) {script.Append(",");} // Separate array items
							script.Append(format.PercentGroupSizes[i]);
						}
						script.Append(");");
						// Get size of first group
						script.Append("var i=0;"); // array index
						script.Append("var s=g[i];"); // size of first group
						// Set the position at the first separator
						script.Append("var l=w.length-s;");
						// While there are enough numbers to group
						script.Append("while(l>0)");
						script.Append("{");
						// Insert the separator
						script.Append(@"w=w.substr(0,l)+'\x");
						script.Append(group);
						script.Append("'+w.substr(l);");
						// Get the next size
						script.Append("if(i<g.length-1) {i++;}");
						script.Append("s=g[i];");
						// If the group size is 0, there is no more grouping
						script.Append("if (s==0) {break;}");
						// Move to the next position
						script.Append("l-=s;");
						script.Append("}");
					}
				}
				// Return the percent string
				script.Append(@"if(p==0){m[2]='';f=''}else{m[2]='\x");
				script.Append(separator);
				script.Append("'}");
				script.Append("return (neg?");
				script.Append("sy[2]+w+m[2]+f+sy[3]:");
				script.Append("sy[0]+w+m[2]+f+sy[1]);");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(DECIMAL_TO_PERCENT, script.ToString());
			}
		}

		/// <summary>
		/// Registers the EnsureNumeric() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script limits keyboard entry to valid numeric characters.
		/// </remarks>
		public static void RegisterEnsureNumericScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(ENSURE_NUMERIC))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the keycode value of the decimal separator character
				int separator = (int)format.NumberDecimalSeparator[0];
				// Get the keycode value of the negative sign
				int minusSign = (int)format.NegativeSign[0];
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(ENSURE_NUMERIC));
				// Get the keycode
				script.Append("var k=window.event.keyCode;");
				// Allow only numbers (48-57), the decimal separator and negative sign
				//TODO: Use keycodes for numbers? do they vary with languages?
				script.Append("if(!((k>47&&k<58)||k==");
				script.Append(separator.ToString());
				script.Append("||k==");
				script.Append(minusSign.ToString());
				script.Append("))");
				script.Append("{window.event.returnValue=false;}");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(ENSURE_NUMERIC, script.ToString());
			}
		}

		/// <summary>
		/// Registers the NumberToDecimal() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a number string to a decimal value.
		/// </remarks>

		public static void RegisterNumberToDecimalScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(NUMBER_TO_DECIMAL))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(NUMBER_TO_DECIMAL, "n"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Delete everything except numbers, the minus sign and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(minusSign);
				script.Append(@"\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Return the decimal value
				script.Append("return n;");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(NUMBER_TO_DECIMAL, script.ToString());
			}
		}

		/// <summary>
		/// Registers the PercentToDecimal() script function.
		/// </summary>
		/// <param name="page">
		/// The System.Web.UI.Page in which to register the script.
		/// </param>
		/// <remarks>
		/// The script converts a percentage string to a decimal value.
		/// </remarks>
		public static void RegisterPercentToDecimalScript(Page page)
		{
			// Determine if the script has been registered
			if (!page.IsClientScriptBlockRegistered(PERCENT_TO_DECIMAL))
			{
				// Get the current number format
				NumberFormatInfo format = Thread.CurrentThread.CurrentCulture.NumberFormat;
				// Get the decimal separator (hex value)
				string separator = ((int)format.NumberDecimalSeparator[0]).ToString("X");
				// Get the minus sign (hex value)
				string minusSign = ((int)format.NegativeSign[0]).ToString("X");
				// Initialise a string builder
				StringBuilder script = new StringBuilder();
				// Declare the script function
				script.Append(ScriptHelper.BeginScriptFunction(PERCENT_TO_DECIMAL, "n"));
				// Ensure its a string
				script.Append("n=n.toString();");
				// Delete everything except numbers, the minus sign and the decimal separator
				script.Append(@"n=n.replace(/[^\d\x");
				script.Append(minusSign);
				script.Append(@"\x");
				script.Append(separator);
				script.Append("]/g,'');");
				// Return the decimal value
				script.Append("return n;");
				// Complete the script
				script.Append(ScriptHelper.EndScriptFunction());
				// Register the script
				page.RegisterClientScriptBlock(PERCENT_TO_DECIMAL, script.ToString());
			}
		}


		#endregion // Static methods

		#region .Private methods 

		/// <summary>
		/// Creates an array containing the currrency prefix and suffix strings.
		/// </summary>
		/// <param name="culture">
		/// A NumberFormatInfo representing the current cultures number format.
		/// </param>
		/// <returns>
		/// A string containing the array.
		/// </returns>
		/// <remarks>
		/// The first element contains the prefix for a positive currency.
		/// The second element contains the suffix for a positive currency.
		/// The third element contains the prefix for a negative currency.
		/// The fourth element contains the suffix for a negative currency.
		/// </remarks>
		private static string GetCurrencySymbols(NumberFormatInfo format)
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Add the positive value prefix and suffix (elements 1 and 2)
			switch (format.CurrencyPositivePattern)
			{
				case 0: // $n
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append("','',");
					break;
				case 1: // n$
					script.Append("'','");
					script.Append(format.CurrencySymbol);
					script.Append("',");
					break;
				case 2: // $ n
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append(" ','',");
					break;
				case 3: // n $
					script.Append("'',' ");
					script.Append(format.CurrencySymbol);
					script.Append("',");
					break;
			}
			// Add the negative value prefix and suffix (elements 3 and 4)
			switch (format.CurrencyNegativePattern)
			{
				case 0: // ($n)
					script.Append("'(");
					script.Append(format.CurrencySymbol);
					script.Append("',')'");
					break;
				case 1:  // -$n
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append(format.CurrencySymbol);
					script.Append("',''");
					break;
				case 2: // $-n
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append(format.NegativeSign);
					script.Append("',''");
					break;
				case 3: // $n-
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append("','");
					script.Append(format.NegativeSign);
					script.Append("'");
					break;
				case 4: // (n$)
					script.Append("'(','");
					script.Append(format.CurrencySymbol);
					script.Append(")'");
					break;
				case 5: // -n$
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append("','");
					script.Append(format.CurrencySymbol);
					script.Append("'");
					break;
				case 6: // n-$
					script.Append("'','");
					script.Append(format.NegativeSign);
					script.Append(format.CurrencySymbol);
					script.Append("'");
					break;
				case 7: // n$-
					script.Append("'','");
					script.Append(format.CurrencySymbol);
					script.Append(format.NegativeSign);
					script.Append("'");
					break;
				case 8: // -n $
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append("',' ");
					script.Append(format.CurrencySymbol);
					script.Append("'");
					break;
				case 9: // -$ n
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append(format.CurrencySymbol);
					script.Append(" ',''");
					break;
				case 10: // n $-
					script.Append("'',' ");
					script.Append(format.CurrencySymbol);
					script.Append(format.NegativeSign);
					script.Append("'");
					break;
				case 11: // $ n-
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append(" ','");
					script.Append(format.NegativeSign);
					script.Append("'");
					break;
				case 12: // $ -n
					script.Append("'");
					script.Append(format.CurrencySymbol);
					script.Append(" ");
					script.Append(format.NegativeSign);
					script.Append("',''");
					break;
				case 13: // n- $
					script.Append("'','");
					script.Append(format.NegativeSign);
					script.Append(" ");
					script.Append(format.CurrencySymbol);
					script.Append("'");
					break;
				case 14: // ($ n)
					script.Append("'(");
					script.Append(format.CurrencySymbol);
					script.Append(" ',')'");
					break;
				case 15: // (n $)
					script.Append("'(',' ");
					script.Append(format.CurrencySymbol);
					script.Append(")'");
					break;
			}
			// Return the script
			return script.ToString();
		}

		/// <summary>
		/// Creates an array containing the negative number prefix and suffix strings.
		/// </summary>
		/// <param name="format">
		/// A NumberFormatInfo representing the current cultures number format.
		/// </param>
		/// <returns>
		/// A string containing the array.
		/// </returns>
		/// <remarks>
		/// The first element contains the prefix for a negative number.
		/// The second element contains the suffix for a negative number.
		/// </remarks>
		private static string GetNumberSymbols(NumberFormatInfo format)
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Add the negative value prefix and suffix
			switch (format.NumberNegativePattern)
			{
				case 0: // (n)
					script.Append("'(',')'");
					break;
				case 1: // -n
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append("',''");
					break;
				case 2: // - n
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append(" ',''");
					break;
				case 3: // n-
					script.Append("'','");
					script.Append("format.NegativeSign");
					script.Append("'");
					break;
				case 4: // n -
					script.Append("'',' ");
					script.Append("format.NegativeSign");
					script.Append("'");
					break;
			}
			// Return the script
			return script.ToString();
		}

		/// <summary>
		/// Creates an array containing the percent prefix and suffix strings.
		/// </summary>
		/// <param name="culture">
		/// A NumberFormatInfo representing the current cultures number format.
		/// </param>
		/// <returns>
		/// A string containing the array.
		/// </returns>
		/// <remarks>
		/// The first element contains the prefix for a positive percent.
		/// The second element contains the suffix for a positive percent.
		/// The third element contains the prefix for a negative percent.
		/// The fourth element contains the suffix for a negative percent.
		/// </remarks>
		private static string GetPercentSymbols(NumberFormatInfo format)
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Add the positive value prefix and suffix (elements 1 and 2)
			switch (format.PercentPositivePattern)
			{
				case 0: // n %
					script.Append("'',' ");
					script.Append(format.PercentSymbol);
					script.Append("',");
					break;
				case 1: // n%
					script.Append("'','");
					script.Append(format.PercentSymbol);
					script.Append("',");
					break;
				case 2: // %n
					script.Append("'");
					script.Append(format.PercentSymbol);
					script.Append("','',");
					break;
			}
			// Add the negative value prefix and suffix (elements 3 and 4)
			switch (format.PercentNegativePattern)
			{
				case 0: // -n %
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append("',' ");
					script.Append(format.PercentSymbol);
					script.Append("'");
					break;
				case 1:  // -n%
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append("','");
					script.Append(format.PercentSymbol);
					script.Append("'");
					break;
				case 2: // -%n
					script.Append("'");
					script.Append(format.NegativeSign);
					script.Append(format.PercentSymbol);
					script.Append("',''");
					break;
			}
			// Return the script
			return script.ToString();
		}


		#endregion // Private methods

	}
}
