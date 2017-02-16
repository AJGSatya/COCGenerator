using System;
using System.Text;


namespace WebControls.NumericBox
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class ScriptHelper
	{
		/// <summary>
		/// Constructs javascript code to declare a function
		/// </summary>
		/// <param name="name">
		/// The name of the function
		/// </param>
		/// <param name="parameters">
		/// The parameters accepted by the function
		/// </param>
		/// <returns>
		/// A string defining the start of javascript function
		/// </returns>
		/// <remarks>
		/// Use in conjuction with EndScriptFunction
		/// </remarks>
		static public string BeginScriptFunction(string name, params string[] parameters)
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Declare the script
			script.Append(Environment.NewLine);
			script.Append("<script language=javascript>");
			// Hide script from non-compliant browsers
			script.Append(Environment.NewLine);
			script.Append("<!--");
			// Declare the function
			script.Append(Environment.NewLine);
			script.Append("function ");
			script.Append(name);
			// Add parameters
			script.Append("(");
			for (int i = 0; i < parameters.Length; i++)
			{
				if (i > 0)
				{
					script.Append(",");
				}
				script.Append(parameters[i]);
			}
			script.Append(")");
			script.Append(Environment.NewLine);
			script.Append("{");
			// Return the script block
			return script.ToString();
		}

		/// <summary>
		/// Constructs javascript code to declare the end of a function
		/// </summary>
		/// <returns>
		/// A string defining the end of javascript function
		/// </returns>
		/// <remarks>
		/// Use in conjuction with BeginScriptFunction
		/// </remarks>
		static public string EndScriptFunction()
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Add the closing script
			script.Append("}");
			script.Append(Environment.NewLine);
			// End hiding script from non-compliant browsers
			script.Append("// -->");
			script.Append(Environment.NewLine);
			script.Append("</script>");
			// Return the script block
			return script.ToString();
		}

		/// <summary>
		/// Constructs javascript code to declare an array
		/// </summary>
		/// <param name="name">
		/// The name or the array to declare
		/// </param>
		/// <param name="values"></param>
		/// The elements of the array
		/// <returns>
		/// A string defining a javascript array
		/// </returns>
		static public string DeclareArray(string name, params object[] values)
		{
			// Initialise a string builder
			StringBuilder script = new StringBuilder();
			// Declare the array
			script.Append("var ");
			script.Append(name);
			script.Append("=new Array(");
			// Add each element
			for (int i=0; i < values.Length; i++)
			{
				if (i > 0)
				{
					script.Append(",");
				}
				// Check if its a number
				if (IsNumeric( values[i]))
				{
					script.Append(values[i].ToString());
				}
				else
				{
					//Quote the string
					script.Append("'");
					script.Append(values[i].ToString());
					script.Append("'");
				}
			}
			script.Append(");");
			// Return the script block
			return script.ToString();
		}

		/// <summary>
		/// Determines if the expresion is a number
		/// </summary>
		/// <param name="expression">
		/// An object to be tested
		/// </param>
		/// <returns>
		/// True if the expression is a number
		/// </returns>
		static private bool IsNumeric(object expression)
		{
			//TODO: Add test for other numeric types (byte, short, long etc)
			if (expression is int)
				return true;
			else if (expression is double)
				return true;
			else
				return false;
		}
	}
}
