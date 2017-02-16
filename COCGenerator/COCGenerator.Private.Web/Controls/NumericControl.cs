using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;

[assembly: TagPrefix("OAMPS.JQS.WebControls", "oibit")]
namespace WebControls.NumericBox
{
    /// <summary>
    /// Serves as the Base class for all numeric controls in the 
    /// SandTrap.WebControls namespace.
    /// </summary>
    [ValidationProperty("Amount")]//Used for validation controls
    public abstract class NumericControl : TextBox
    {

        #region .Declarations

        // Properties
        TextAlign _Alignment;
        int _Precision;
        decimal _MinAmount;
        decimal _MaxAmount;
        // TODO: Use styles for positive/negative values?
        Color _NegativeColor;
        Color _PositiveColor;
        string _OnFocus;
        string _OnKeyPress;
        string _OnBlur;

        // Events
        [Browsable(true), Category("Action")]
        public event EventHandler AmountChanged;

        #endregion // Declarations

        #region .Constructor

        /// <summary>
        /// Initialises a new instance of the NumericControl class.
        /// </summary>
        protected NumericControl()
        {
            // Set default properties
            _Alignment = TextAlign.Right;
            _MinAmount = -100000000M;
            _MaxAmount = 100000000M;
            _Precision = 2;
            _NegativeColor = Color.Red;
            _OnFocus = string.Empty;
            _OnKeyPress = string.Empty;
            _OnBlur = string.Empty;
        }


        #endregion // Constructor

        #region .Properties

        /// <summary>
        /// Gets or sets the alignment of the text in the control.
        /// </summary>
        /// <value>
        /// One of the TextAlign values. The default is right.
        /// </value>
        [
        DefaultValue(typeof(TextAlign), "Right"),
        Category("Appearance"),
        Bindable(false),
        Description("The alignment of text in the control")
        ]
        public TextAlign Alignment
        {
            get
            {
                return _Alignment;
            }
            set
            {
                _Alignment = value;
            }
        }

        /// <summary>
        /// Gets of sets the amount.
        /// </summary>
        /// <value>
        /// A System.Decimal. The default is 0.
        /// </value>
        [
        DefaultValue(typeof(decimal), "0"),
        Category("Appearance"),
        Bindable(true),
        Description("The amount displayed in the control")
        ]
        public decimal Amount
        {
            get
            {
                // Check if a value has been assigned
                object amount = ViewState["amount"];
                if (amount == null)
                    // Return the default
                    return 0M;
                else
                    // Return the value
                    return (decimal)amount;
            }
            set
            {
                ViewState["amount"] = value;
                // Set the text colour
                if (value < 0)
                    base.ForeColor = NegativeColor;
                else
                    base.ForeColor = PositiveColor;
            }
        }

        /// <summary>
        /// Gets the colour of the text.
        /// </summary>
        /// <remarks>
        /// The implementation hides the base class ForeColor property and implements it 
        /// as readonly. It returns PositiveColor if Amount is positive or NegativeColor 
        /// if Amount is negative.
        /// </remarks>
        [
        Browsable(false)
        ]
        public new Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
        }

        /// <summary>
        /// Gets of sets the minumum allowable amount.
        /// </summary>
        /// <value>
        /// A System.Decimal. The default is -100,000,000.00
        /// </value>
        [
        DefaultValue(typeof(decimal), "-100000000"),
        Category("Appearance"),
        Bindable(true),
        Description("The minumum allowable amount")
        ]
        public decimal MinAmount
        {
            get
            {
                return _MinAmount;
            }
            set
            {
                _MinAmount = value;
            }
        }

        /// <summary>
        /// Gets of sets the maximum allowable amount.
        /// </summary>
        /// <value>
        /// A System.Decimal. The default is 100,000,000.00
        /// </value>
        [
        DefaultValue(typeof(decimal), "100000000"),
        Category("Appearance"),
        Bindable(true),
        Description("The maximum allowable amount")
        ]
        public decimal MaxAmount
        {
            get
            {
                return _MaxAmount;
            }
            set
            {
                _MaxAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the text color if the Amount is negative.
        /// </summary>
        /// <value>
        /// A System.Drawing.Color. The default is Color.Red.
        /// </value>
        /// <remarks>
        /// The property sets a user defined attribute (negativeColor). The attribute
        /// is read by the rendered HTMLInputText controls onblur event to set the text 
        /// colour if its value is negative.
        /// </remarks>
        [
        DefaultValue(typeof(Color), "Red"),
        Category("Appearance"),
        Bindable(true),
        Description("The colour of negative currency values")
        ]
        public Color NegativeColor
        {
            get
            {
                return _NegativeColor;
            }
            set
            {
                _NegativeColor = value;
                // Set the controls ForeColor if appropriate
                if (Amount < 0)
                    base.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the client script function(s) to call when the 
        /// control loses focus.
        /// </summary>
        /// <value>
        /// A System.String. The default is String.Empty.
        /// </value>
        /// <remarks>
        /// All controls inheriting from NumericControl assign a script function to the
        /// OnBlur event. This property allows additional script functions to be assigned
        /// to the HTML controls OnBlur event
        /// </remarks>
        [
        Category("Client Scripts"),
        Description("The name of the client script function(s) to call when the " +
            "control loses focus")
        ]
        public string OnBlur
        {
            get
            {
                return _OnBlur;
            }
            set
            {
                _OnBlur = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of a client script function to call when the 
        /// control gains focus.
        /// </summary>
        /// <value>
        /// A System.String. The default is String.Empty.
        /// </value>
        /// <remarks>
        /// All controls inheriting from NumericControl assign a script function to the
        /// OnBlur event. This property allows additional script functions to be assigned
        /// to the HTML controls OnFocus event
        /// </remarks>
        [
        Category("Client Scripts"),
        Description("The name of the client script function(s) to call when the " +
            "control gains focus")
        ]
        public string OnFocus
        {
            get
            {
                return _OnFocus;
            }
            set
            {
                _OnFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of a client script function to call when the 
        /// control receives key press events.
        /// </summary>
        /// <value>
        /// A System.String. The default is String.Empty.
        /// </value>
        /// <remarks>
        /// All controls inheriting from NumericControl assign a script function to the
        /// OnKeyPress event. This property allows additional script functions to be assigned
        /// to the HTML controls OnFocus event
        /// </remarks>

        [
        Category("Client Scripts"),
        Description("The name of the client script function(s) to call when the " +
            "control receives key press events")
        ]
        public string OnKeyPress
        {
            get
            {
                return _OnKeyPress;
            }
            set
            {
                _OnKeyPress = value;
            }
        }

        /// <summary>
        /// Gets or sets the text color if the Amount is positive.
        /// </summary>
        /// <value>
        /// A System.Drawing.Color. The default is Color.Empty.
        /// </value>
        /// <remarks>
        /// The property sets a user defined attribute (positiveColor). The attribute
        /// is read by the rendered HTMLInputText controls onblur event to set the text 
        /// colour if its value is positive.
        /// </remarks>
        [
        DefaultValue(typeof(Color), "Empty"),
        Category("Appearance"),
        Bindable(true),
        Description("The colour of negative currency values")
        ]
        public Color PositiveColor
        {
            get
            {
                return _PositiveColor;
            }
            set
            {
                _PositiveColor = value;
                // Set the controls ForeColor if appropriate
                if (Amount >= 0)
                {
                    base.ForeColor = value;
                }
            }
        }

        /// <summary>
        /// Gets of sets the number of decimal digits to display.
        /// </summary>
        /// <value>
        /// A System.Int32. The default is 2.
        /// </value>
        [
        DefaultValue(typeof(int), "2"),
        Category("Appearance"),
        Bindable(true),
        Description("The number of decimal digits to display")
        ]
        public int Precision
        {
            get
            {
                return _Precision;
            }
            set
            {
                _Precision = value;
            }
        }


        #endregion // Properties

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
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddStyleAttribute("text-align", Alignment.ToString());
            // Add user defined attributes to control colour formatting
            writer.AddAttribute("negativeColor", NegativeColor.Name);

          
            if (PositiveColor != Color.Empty)
            {
                writer.AddAttribute("positiveColor", PositiveColor.Name);
            }
            // Add user defined attributes to control minimum and maximum values
            writer.AddAttribute("minAmount", MinAmount.ToString());
            writer.AddAttribute("maxAmount", MaxAmount.ToString());
            // Add client side event handlers
            string onKeyPress = "EnsureNumeric()";
            if (OnKeyPress != string.Empty)
            {
                onKeyPress += "," + OnKeyPress;
            }
            writer.AddAttribute("onkeypress", onKeyPress);
        }

        /// <summary>
        /// Returns the System.Web.UI.HtmlTextWriterTag.
        /// </summary>
        /// <remarks>
        /// The method overrides the base class (Span tag) to return the 
        /// Input (TextField) tag.
        /// </remarks>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Input;
            }
        }

        /// <summary>
        /// Raises the AmountChanged event.
        /// </summary>
        /// <param name="e">
        /// A System.EventArgs that contains event information.
        /// </param>
        /// <remarks>
        /// The control must have view state enabled for the AmountChanged event to work 
        /// correctly.
        /// </remarks>
        protected virtual void OnAmountChanged(EventArgs e)
        {
            // Check for registered objects
            if (AmountChanged != null)
            {
                // Notify the object
                AmountChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">
        /// A System.EventArgs that contains event information.
        /// </param>
        /// <remarks>
        /// The method overrides the base class to register the EnsureNumeric script.
        /// </remarks>
        protected override void OnPreRender(System.EventArgs e)
        {
            // Register the EnsureNumeric script
            Scripts.RegisterEnsureNumericScript(Page);
            // Call the base method
            base.OnPreRender(e);
        }


        #endregion // Protected methods

    }
}
