using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;
using System.IO;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common
{
    /// <summary>
    /// Class to set properties for template.
    /// </summary>
    public class FontSetting
    {
        #region " -- Private -- "

        #region " -- New / Dispose -- "

        private FontSetting()
        { 
        }

        #endregion

        #endregion

        #region " -- Public -- "
     
        #region " -- New / Dispose -- "

        public FontSetting(string fontName, FontStyle fontStyle, int size, Color foreColor, Color backColor, System.Drawing.StringAlignment textAlignment)
        {
            this._FontName = fontName;
            this._FontStyle = fontStyle;
            this._FontSize = size;
            this._ForeColor = foreColor;
            this._BackColor = backColor;
            this._TextAlignment = textAlignment;
            
        }

        public FontSetting(string fontName, FontStyle fontStyle, int size, Color foreColor, Color backColor, System.Drawing.StringAlignment textAlignment, bool show, bool inline, FootNoteDisplayStyle footnoteInLine)
        {
            this._FontName = fontName;
            this._FontStyle = fontStyle;
            this._FontSize = size;
            this._ForeColor = foreColor;
            this._BackColor = backColor;
            this._TextAlignment = textAlignment;
            this._Show = show;
            this._Inline = inline;
            this._TableFootnoteInLine = footnoteInLine;
        }

        public FontSetting(Font font, Color foreColor, Color backColor)
        {
            this._FontName = font.Name;
            this._FontStyle = font.Style;
            this._FontSize = Convert.ToInt32(font.Size);
            this._ForeColor = foreColor;
            this._BackColor = backColor;
        }

        public FontSetting(string fontName, FontStyle fontStyle, int size, Color foreColor, bool showBackColor, Color backColor, System.Drawing.StringAlignment textAlignment, bool show, bool inline, FootNoteDisplayStyle footnoteInLine, bool showAlternateColor, string alternateColor1, string alternateColor2, bool wordWrap, int columnWidth, bool formatDataValue)
        {
            this._FontName = fontName;
            this._FontStyle = fontStyle;
            this._FontSize = size;
            this._ForeColor = foreColor;
            this._BackColor = backColor;
            this._TextAlignment = textAlignment;
            this._Show = show;
            this._Inline = inline;
            this._TableFootnoteInLine = footnoteInLine;
            this._ShowAlternateColor = showAlternateColor;
            this._AlternateBackColor1 = alternateColor1;
            this._AlternateBackColor2 = alternateColor2;
            this._WordWrap = wordWrap;
            this._ColumnWidth = columnWidth;
            this._FormatDataValue = formatDataValue;
            this._ShowBackColor = showBackColor;
        }

        public FontSetting(string fontName, FontStyle fontStyle, int size, Color foreColor, bool showBackColor, Color backColor, System.Drawing.StringAlignment textAlignment, bool show, bool inline, FootNoteDisplayStyle footnoteInLine, bool showAlternateColor, string alternateColor1, string alternateColor2, bool wordWrap, int columnWidth, bool formatDataValue, CellBorderStyle borderStyle, string borderColor, bool roundDataValue, int decimalPlace)
        {
            this._FontName = fontName;
            this._FontStyle = fontStyle;
            this._FontSize = size;
            this._ForeColor = foreColor;
            this._BackColor = backColor;
            this._TextAlignment = textAlignment;
            this._Show = show;
            this._Inline = inline;
            this._TableFootnoteInLine = footnoteInLine;
            this._ShowAlternateColor = showAlternateColor;
            this._AlternateBackColor1 = alternateColor1;
            this._AlternateBackColor2 = alternateColor2;
            this._WordWrap = wordWrap;
            this._ColumnWidth = columnWidth;
            this._FormatDataValue = formatDataValue;
            this._ShowBackColor = showBackColor;
            this._BorderStyle = borderStyle;
            this._BorderColor = borderColor;
            this._DecimalPlace = decimalPlace;
            this._RoundDataValues = roundDataValue;
        }

        /// <summary>
        /// Return the font object 
        /// </summary>
        /// <returns>Font object</returns>
        public Font Font()
        {
            System.Drawing.Font Retval;
            try
            {
                Retval = new Font(this._FontName, (float)this._FontSize, this._FontStyle, GraphicsUnit.Pixel);
            }
            catch (Exception)
            {
                Retval = null;
            }
            return Retval;
        }

        #endregion

        #region " -- Enum -- "
        /// <summary>
        /// cell border style
        /// </summary>
        public enum CellBorderStyle
        {
            /// <summary>
            /// 0
            /// </summary>
            None = 0,

            /// <summary>
            /// 1
            /// </summary>
            Bottom = 1,

            /// <summary>
            /// 2
            /// </summary>
            Top = 2,
            /// <summary>
            /// 3
            /// </summary>
            Fill = 3
        }

        #endregion

        #region " -- Properties -- "

        private string _FontName = "Arial";
        /// <summary>
        /// Gets or set the fontname
        /// </summary>
        public string FontName
        {
            get
            {
                return this._FontName;
            }
            set
            {
                this._FontName = value;
            }
        }


        private FontStyle _FontStyle = FontStyle.Regular;
        /// <summary>
        /// Gets or sets the font style
        /// </summary>
        public FontStyle FontStyle
        {
            get
            {
                
                return this._FontStyle;
            }
            set
            {
                this._FontStyle = value;
            }
        }


        private int _FontSize = 8;
        /// <summary>
        /// Gets or sets the font size
        /// </summary>
        public int FontSize
        {
            get
            {
                return this._FontSize;
            }
            set
            {
                this._FontSize = value;
            }
        }

        private Color _ForeColor = Color.Black;
        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        [XmlIgnore()]
        public Color ForeColor
        {
            get
            {
                return this._ForeColor;
            }
            set
            {
                this._ForeColor = value;
            }
        }

        private string _XmlForeColor = "Black";
        /// <summary>
        /// Only for XmlSerialization.
        /// </summary>
        /// <remarks> XmlSerilization can serialize the Public read/write properties and public fields of public classes.
        /// Color is the read only property, so XmlSerialization can't serialize it. 
        /// To serialize/desrialize the Color Property, we used this property.
        /// To serialize the color, We used the XmlColorType.
        /// </remarks>
        [XmlElement("ForeColor")]
        public string XmlForeColor
        {
            get
            {
                return ColorTranslator.ToHtml(this._ForeColor);
            }
            set
            {
                this._ForeColor = ColorTranslator.FromHtml(value);
            }
        }

        private Color _BackColor = Color.White;
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        [XmlIgnore()]
        public Color BackColor
        {
            get
            {
                return this._BackColor;
            }
            set
            {
                this._BackColor = value;
            }
        }

        private string _XmlBackColor = "White";
        /// <summary>
        /// Only for XmlSerialization.
        /// </summary>
        /// <remarks> XmlSerilization can serialize the Public read/write properties and public fields of public classes.
        /// Color is the read only property, so XmlSerialization can't serialize it. 
        /// To serialize/desrialize the Color Property, we used this property.
        /// To serialize the color, We used the XmlColorType.
        /// </remarks>
        [XmlElement("BackColor")]
        public string XmlBackColor
        {
            get
            {
                return ColorTranslator.ToHtml(this._BackColor);
            }
            set
            {
                this._BackColor = ColorTranslator.FromHtml(value);
            }
        }

        private System.Drawing.StringAlignment _TextAlignment = StringAlignment.Center;
        /// <summary>
        /// Gets or sets the text alignment.
        /// <remarks> It is used in content settings. </remarks>
        /// </summary>
        public System.Drawing.StringAlignment TextAlignment
        {
            get 
            {
                return this._TextAlignment; 
            }
            set 
            {
                this._TextAlignment = value; 
            }
        }      

        private bool _Show = true;
        /// <summary>
        /// Used to show the comments, footnotes and denominator.
        /// </summary>
        public bool Show
        {
            get 
            { 
                return this._Show; 
            }
            set 
            { 
                this._Show = value; 
            }
        }

        private bool _Inline = false;
        /// <summary>
        /// Gets or sets the inline.
        /// </summary>
        /// <remarks> This property is used for to show the inline footnotes and comments </remarks>
        public bool Inline
        {
            get 
            { 
                return this._Inline; 
            }
            set 
            { 
                this._Inline = value; 
            }
        }

        private FootNoteDisplayStyle _TableFootnoteInLine = FootNoteDisplayStyle.Inline;
        /// <summary>
        /// Get or set the table FootnoteInLine
        /// </summary>
        public FootNoteDisplayStyle TableFootnoteInLine
        {
            get
            {
                return this._TableFootnoteInLine;
            }
            set
            {
                this._TableFootnoteInLine = value;
            }
        }

        private FootNoteDisplayStyle _GraphFootnoteInLine = FootNoteDisplayStyle.InlineWithData;
        /// <summary>
        /// Get or set the graph FootnoteInLine
        /// </summary>
        public FootNoteDisplayStyle GraphFootnoteInLine
        {
            get
            {
                return this._GraphFootnoteInLine;
            }
            set
            {
                this._GraphFootnoteInLine = value;
            }
        }

        private bool _ShowAlternateColor = false;
        /// <summary>
        /// Gets or sets the visibility of alternate color
        /// </summary>
        public bool ShowAlternateColor
        {
            get 
            {
                return this._ShowAlternateColor; 
            }
            set 
            {
                this._ShowAlternateColor = value; 
            }
        }

        private string _AlternateBackColor1 = ColorTranslator.ToHtml(Color.White);
        /// <summary>
        /// Gets or sets the alternate back color
        /// </summary>
        public string AlternateBackColor1
        {
            get 
            {
                return _AlternateBackColor1; 
            }
            set 
            {
                _AlternateBackColor1 = value; 
            }
        }

        private string _AlternateBackColor2 = ColorTranslator.ToHtml(Color.White);
        /// <summary>
        /// Gets or sets the alternate back color2
        /// </summary>
        public string AlternateBackColor2
        {
            get 
            {
                return _AlternateBackColor2; 
            }
            set 
            {
                _AlternateBackColor2 = value; 
            }
        }

        private bool _WordWrap = true;
        /// <summary>
        /// Gets or sets the word wrap
        /// </summary>
        public bool WordWrap
        {
            get 
            {
                return this._WordWrap; 
            }
            set 
            {
                this._WordWrap = value; 
            }
        }

        private int _ColumnWidth = 22;
        /// <summary>
        /// Gets or sets the column width
        /// </summary>
        public int ColumnWidth
        {
            get 
            {
                return this._ColumnWidth; 
            }
            set 
            {
                this._ColumnWidth = value; 
            }
        }

        private bool _FormatDataValue = false;
        /// <summary>
        /// Gets or sets the formatting of data value
        /// </summary>
        public bool FormatDataValue
        {
            get 
            {
                return this._FormatDataValue; 
            }
            set 
            {
                this._FormatDataValue = value; 
            }
        }

        private bool _RoundDataValues = false;
        /// <summary>
        /// Gets or sets the round decimal places
        /// </summary>
        public bool RoundDataValues
        {
            get { return this._RoundDataValues; }
            set { this._RoundDataValues = value; }
        }	

        private int _DecimalPlace = 0;
        /// <summary>
        /// Gets or sets the decimal places
        /// </summary>
        public int DecimalPlace
        {
            get { return _DecimalPlace; }
            set { _DecimalPlace = value; }
        }	

        private bool _ShowBackColor = true;
        /// <summary>
        /// Gets or sets the show back color
        /// </summary>
        public bool ShowBackColor
        {
            get 
            {
                return this._ShowBackColor; 
            }
            set 
            {
                this._ShowBackColor = value; 
            }
        }

        private int _BorderSize;
        /// <summary>
        /// TODO
        /// </summary>
        public int BorderSize
        {
            get { return _BorderSize; }
            set { _BorderSize = value; }
        }

        private string _BorderColor = ColorTranslator.ToHtml(Color.Gray);
        /// <summary>
        /// Gets or sets the alternate back color
        /// </summary>
        public string BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
            }
        }

        private CellBorderStyle _BorderStyle = CellBorderStyle.None;
        /// <summary>
        /// TODO
        /// </summary>
        public CellBorderStyle BorderStyle
        {
            get { return _BorderStyle; }
            set { _BorderStyle = value; }
        }

        private int _RowHeight = 12;
        /// <summary>
        /// Used in refernece of Area Level Font settings
        /// </summary>
        public int RowHeight
        {
            get { return _RowHeight; }
            set { _RowHeight = value; }
        }

      

        #endregion

        #endregion
    }
}
