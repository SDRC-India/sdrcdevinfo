using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap
{

    #region " -- Delegates --"

    /// <summary>
    /// Declare the delegate to update the legend.
    /// </summary>
    /// <param name="LegendIndex"></param>
    /// <param name="LegendPresentationData"></param>
    public delegate void SetLegendsDelegate(int LegendIndex);

   
    /// <summary>
    ///  Declare the delegate to update the caption of the legend.
    /// </summary>
    /// <param name="LegendIndex"></param>
    /// <returns></returns>
    public delegate void SetCaptionDelegate(int LegendIndex);

    /// <summary>
    ///  Declare the delegate to update the Color of the legend.
    /// </summary>
    /// <param name="LegendIndex"></param>
    /// <param name="LegendPresentationData"></param>
    public delegate void SetColorDelegate(int LegendIndex);

    #endregion

    
    /// <summary>
    /// Stores the information related to a range break
    /// </summary>
    public class Legend
    {
    
        # region " -- Private -- "

        #region " -- Raise Event -- "
        
        /// <summary>
        /// Raise the event to set the legends
        /// </summary>
        /// <param name="legendIndex"></param>        
        private void RaiseSetLegendsEvent(int legendIndex)
        {
            if (this.SetLegendsEvent != null)
            {
                this.SetLegendsEvent(legendIndex);
            }
        }

        /// <summary>
        /// Raise the event to set the caption
        /// </summary>
        /// <param name="legendIndex"></param>
        private void RaiseSetCaptionEvent(int legendIndex)
        {
            if (this.SetCaptionEvent != null)
            {
                this.SetCaptionEvent(legendIndex);
            }
        }

        /// <summary>
        /// Raise the event to set the Color
        /// </summary>
        /// <param name="legendIndex"></param>        
        private void RaiseSetColorEvent(int legendIndex)
        {
            if (this.SetColorEvent != null)
            {
                this.SetColorEvent(legendIndex);
            }
        }

        #endregion

        #region " -- Methods -- "

        #endregion

        # endregion

        # region " -- Public / Friend -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Constructor to create the legend.
        /// </summary>
        /// <param name="caption">Caption</param>
        /// <param name="fromRange">From Range</param>
        /// <param name="ToRange">To Range</param>
        /// <param name="count">Count</param>
        /// <param name="legendColor">Legend Color</param>
        /// <param name="legendIndex">Legend Index</param>
        /// <param name="presentationData">Presentation Data</param>
        public Legend(string caption, decimal fromRange, decimal ToRange, int count, Color legendColor, int legendIndex)
        {
            this._Caption = caption;
            string Range1 = fromRange.ToString();
            string Range2 = ToRange.ToString();

            //-- Check the decimal places and apply the group formatting
            if (fromRange.ToString().Contains("."))
            {
                Range1 = string.Format("{0:n}", fromRange); 
            }
            else
            {
                Range1 = string.Format("{0:n}", fromRange).Replace(".00", ""); 
            }

            if (ToRange.ToString().Contains("."))
            {
                Range2 = string.Format("{0:n}", ToRange); 
            }
            else
            {
                Range2 = string.Format("{0:n}", ToRange).Replace(".00", ""); 
            }

            this._Range = Range1 + " - " + Range2;
            this._RangeFrom = fromRange;
            this._RangeTo = ToRange;
            this._Count = count;
            this._Color = legendColor;            
            this._LegendIndex = legendIndex;            
        }     
        
        /// <summary>
        /// Private constructor is needed to serailize and deserialize the class.
        /// </summary>
        public Legend()
        { 
            // Do Nothing
        }

        #endregion

        # region " -- Properties -- "

        private string _Caption = string.Empty;
        /// <summary>
        /// Get or Sets the Caption
        /// </summary>
        public string Caption
        {
            get
            {
                return this._Caption;
            }
            set
            {
                this._Caption = value;
                this.RaiseSetCaptionEvent(this._LegendIndex);
            }
        }

        private string _Range = string.Empty;
        /// <summary>
        /// Gets or sets the range.
        /// </summary>
        /// <remarks> Contains FromValue + " - " + ToValue  </remarks>
        public string Range
        {
            get 
            {
                return this._Range; 
            }
            set 
            {
                this._Range = value; 
            }
        }
	

        private decimal _RangeFrom;
        /// <summary>
        /// Get or sets the from range
        /// <para>Hosting application will display this value according to the regional setting</para>
        /// </summary>
        public decimal RangeFrom
        {

            get
            {
                return this._RangeFrom;
            }
            set
            {
                this._RangeFrom = value;
                // -- raise the event in case of update the value from hosting application
                this.RaiseSetLegendsEvent(this._LegendIndex);
            }
        }

        private decimal _RangeTo;
        /// <summary>
        /// Get or sets the to range
        /// <para>Hosting application will display this value according to the regional setting</para>
        /// </summary>
        public decimal RangeTo
        {
            get
            {
                return this._RangeTo;
            }
            set
            {
                this._RangeTo = value;
                // -- raise the event in case of update the value from hosting application
                this.RaiseSetLegendsEvent(this._LegendIndex);
            }
        }

        private int _Count;
        /// <summary>
        /// Get or sets the count
        /// </summary>
        public int Count
        {
            get
            {
                return this._Count;
            }
            set
            {
                this._Count = value;
            }
        }

        private System.Drawing.Color _Color = Color.Gray ;
        /// <summary>
        /// Get or sets the Color
        /// <para>XmlSerialization does not serialize the Color</para>
        /// </summary>
        [XmlIgnore()]
        public System.Drawing.Color Color
        {
            get
            {
                return this._Color;
            }
            set
            {
                this._Color = value;
                // -- raise the event in case of update the color from the hosting application
                this.RaiseSetColorEvent(this._LegendIndex);
            }
        }

        private string _XmlColor;
        /// <summary>
        /// Only for XmlSerilization.
        /// </summary>
        /// <remarks> XmlSerilization can serialize the Public read/write properties and public fields of public classes.
        /// Color is the read only property, so XmlSerialization can't serialize it. 
        /// To serialize/desrialize the Color Property, we used this property.
        /// To serialize the color, We used the XmlColorType.
        /// </remarks>
        
        [XmlElement("Color")]
        public string XmlColor
        {
            get
            {
                return ColorTranslator.ToHtml(this._Color);
            }
            set
            {
                this.SetColor(ColorTranslator.FromHtml(value));
            }
        }

        private int _LegendIndex = -1;
        /// <summary>
        /// Get or set the Legend index
        /// <para>This will set at the time of changing the to range</para>
        /// </summary>
        public int LegendIndex
        {
            get
            { 
                return this._LegendIndex; 
            }
            set
            {
                this._LegendIndex = value; 
            }
        }
	
	
        # endregion

        #region " -- Methods -- "

        /// <summary>
        /// Set the RangeTo value
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="tovalue"></param>
        internal void SetTovalue(decimal tovalue)
        {
            this._RangeTo = tovalue;
        }

        /// <summary>
        /// Set the RangeFrom value
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="fromvalue"></param>
        internal void SetFromvalue(decimal fromvalue)
        {
            this._RangeFrom = fromvalue;
        }

        /// <summary>
        /// Set the Color
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="legendColor"></param>
        internal void SetColor(Color legendColor)
        {
            this._Color = legendColor;
        }

        /// <summary>
        /// Set the caption.
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="caption"></param>
        internal void SetCaption(string caption)
        {
            this._Caption = caption;
        }

        #endregion

        # region  " -- Events --"
        /// <summary>
        /// Declare the event to update the legends
        /// </summary>
        public event SetLegendsDelegate SetLegendsEvent;

        /// <summary>
        /// Declare the event to update the legend caption.
        /// <para>This will update the caption for all the themes </para>
        /// </summary>
        public event SetCaptionDelegate SetCaptionEvent;

        /// <summary>
        /// Declare the event to update the legend color.
        /// <para>This will update the color for all the themes </para>
        /// </summary>
        public event SetColorDelegate SetColorEvent;

        #endregion


        # endregion

    }
}
