using System;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Xml.Serialization;
//**************************************************************************************************
//Change No     Date                    Description
// C1           10-June-2008            Legend.Caption property is added. While drawing Legend, Legend.Caption is concatenated with Legend.title (e.g: Low -(21.2 - 45.6)  )
//**************************************************************************************************

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Legend
    {

        # region " Variables "
        //Caption for the range
        private string m_Title;
        private decimal m_From;
        private decimal m_To;
        //give the count of shapes in the legend
        private int m_ShapeCount;
        private Color m_Color;
        private FillStyle m_FillStyle = FillStyle.Solid;

        //*** Variables used for Point Type Color Theme and Symbol Theme
        //*** For Symbol Theme if SymbolTheme = "" then marker shall be drawn else image shall be drawn '*** Supported Image Type are gif, jpeg, png, ico
        private MarkerStyle m_MarkerType = MarkerStyle.Circle;       //MarkerType.Circle;

        private int m_MarkerSize = 5;
        //*** This font should exist on server / desktop
        private Font m_MarkerFont = new Font("Webdings", 5);
        //MarkerFont and MarkerChar are only valid with MatkerStyle = Custom
        private char m_MarkerChar = "H"[0];
        private string m_SymbolImage = "";


        //*** Variable used for Label Theme
        private bool mbLabelVisible = true;
        //0-AreaId, 1-AreaName
        private string msLabelField = "1";
        private bool mbLabelMultiRow;
        private bool mbLabelIndented;


        #  endregion

        # region " Properties "

        //-- Change: C1
        private string _Caption;
        /// <summary>
        /// Gets or sets Legend Caption.
        /// </summary>
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }
	
        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public decimal RangeFrom
        {
            get { return m_From; }
            set { m_From = value; }
        }

        public decimal RangeTo
        {
            get { return m_To; }
            set { m_To = value; }
        }

        public int ShapeCount
        {
            get { return m_ShapeCount; }
            set { m_ShapeCount = value; }
        }
        [XmlIgnore()]
        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
 
        [XmlElement("Color")]
        public int XmlColorType
        {
            get
            {
                return this.m_Color.ToArgb();
            }
            set
            {
                this.m_Color = Color.FromArgb(value);
            }
        }
 
        
        public FillStyle FillStyle
        {
            get { return m_FillStyle; }
            set { m_FillStyle = value; }
        }

        public MarkerStyle MarkerType
        {
            get { return m_MarkerType; }
            set { m_MarkerType = value; }
        }

        
        public int MarkerSize
        {
            get { return m_MarkerSize; }
            set
            {
                m_MarkerSize = value;
                if (m_MarkerType == MarkerStyle.Custom)
                {
                    m_MarkerFont = new Font(m_MarkerFont.FontFamily, value);
                }
            }
        }

        [XmlIgnore()]
        public Font MarkerFont
        {
            get { return m_MarkerFont; }
            set { m_MarkerFont = value; }
        }


        //private string _XmlFont;
        [XmlElement("MarkerFont")]
        public string XmlMarkerFont
        {
            get
            {
                //string Font = m_MarkerFont.Name + "," + m_MarkerFont.Size + "," + m_MarkerFont.Style;
                return m_MarkerFont.Name + "," + m_MarkerFont.Size + "," +  m_MarkerFont.Style.ToString();
            }
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.m_MarkerFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.m_MarkerFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }


        public char MarkerChar
        {
            get { return m_MarkerChar; }
            set { m_MarkerChar = value; }
        }

        public string SymbolImage
        {
            get { return m_SymbolImage; }
            set { m_SymbolImage = value; }
        }

        public bool LabelVisible
        {
            get { return mbLabelVisible; }
            set { mbLabelVisible = value; }
        }

        public string LabelField
        {
            get { return msLabelField; }
            set { msLabelField = value; }
        }

        public bool LabelMultiRow
        {
            get { return mbLabelMultiRow; }
            set { mbLabelMultiRow = value; }
        }

        public bool LabelIndented
        {
            get { return mbLabelIndented; }
            set { mbLabelIndented = value; }
        }

        private bool _Visible = true;
        /// <summary>
        /// legend Visibility.
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }
	

        # endregion

    }
}