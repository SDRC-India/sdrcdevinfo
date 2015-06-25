using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    [XmlInclude(typeof(PointF))]
    public class CustomLabel
    {
        //*** A collection of instances of this class shall be preserved inside Layer Class

        # region " Enumerator"
        public enum EnumLabelType
        {
            Simple = 0,
            Box = 1,
            Line = 2,
            Callout = 3
        }
        # endregion


        # region " Variables"
        //*** AreaId of Modified label
        private string msAreaId;
        private EnumLabelType meType = EnumLabelType.Simple;
        private bool mbVisible = true;
        //*** Modified Caption
        private string msCaption;
        //0-AreaId, 1-AreaName
        private string msLabelField = "1";
        private bool mbMultiRow;
        private bool mbIndent;
        private Font moFont = new Font("Arial", 8);
        private Color moForeColor = Color.Black;

        private bool mbLeaderVisible;
        private Color moLeaderColor = Color.Gray;
        private int miLeaderWidth = 1;
        private DashStyle meLeaderStyle = DashStyle.Solid;

        //*** Top Right Corner
        private PointF moDrawPoint = new PointF();
        //*** georeferenced box
        private RectangleF moPlaceHolder = new RectangleF();

        # endregion

        # region " Properties"

        public string AreaId
        {
            get { return msAreaId; }
            set { msAreaId = value; }
        }
        public EnumLabelType Type
        {
            get { return meType; }
            set { meType = value; }
        }
        public bool Visible
        {
            get { return mbVisible; }
            set { mbVisible = value; }
        }
        public string Caption
        {
            get { return msCaption; }
            set { msCaption = value; }
        }
        public string LabelField
        {
            get { return msLabelField; }
            set { msLabelField = value; }
        }
        public bool MultiRow
        {
            get { return mbMultiRow; }
            set { mbMultiRow = value; }
        }
        public bool Indent
        {
            get { return mbIndent; }
            set { mbIndent = value; }
        }

        [XmlIgnore()]
        public Font LabelFont
        {
            get { return moFont; }
            set { moFont = value; }
        }

        //This below property is only used for Font property "LabelFont" to get serialized.
        [XmlElement("LabelFont")]
        public string XmlLabelFont
        {
            //At the time of serialization, moFont variable gets serialized into string (Font Name + Font Size)
            get
            {
                return moFont.Name + "," + moFont.Size + "," + moFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into moFont
            set
            {
                string[] _FontSettings = new string[3];
                try
                {
                    _FontSettings = value.Split(",".ToCharArray());
                    if (_FontSettings.Length == 3 && _FontSettings[2] != "")
                    {
                        this.moFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                    }
                    else
                    {
                        this.moFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                    }
                }
                catch
                {
                    this.moFont = new Font("Arial", 8F);
                }
            }
        }

        [XmlIgnore()]
        public Color ForeColor
        {
            get { return moForeColor; }
            set { moForeColor = value; }
        }

        //This property is used for ForeColor variable to get xml serialized.
        [XmlElement("ForeColor")]
        public int XmlForeColor
        {
            //At the time of serialization, moForeColor variable gets serialized in form of Color Name.
            get
            {
                return this.moForeColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into moForeColor
            set
            {
                this.moForeColor = Color.FromArgb(value);
            }
        }

        private LabelCase _LabelCase = LabelCase.Regular;
        /// <summary>
        /// Gets or sets the Label Case (UperCase, LowerCase & Regular)
        /// </summary>
        public LabelCase LabelCase
        {
            get { return _LabelCase; }
            set { _LabelCase = value; }
        }

        private int _LabelCharacterSpacing = 0;
        /// <summary>
        /// Gets or sets Label's Character Spacing (in pixels).
        /// </summary>
        public int LabelCharacterSpacing
        {
            get { return _LabelCharacterSpacing; }
            set { _LabelCharacterSpacing = value; }
        }

        public bool LeaderVisible
        {
            get { return mbLeaderVisible; }
            set { mbLeaderVisible = value; }
        }

        [XmlIgnore()]
        public Color LeaderColor
        {
            get { return moLeaderColor; }
            set { moLeaderColor = value; }
        }

        //This property is used for LeaderColor variable to get xml serialized.
        [XmlElement("LeaderColor")]
        public int XmlLeaderColor
        {
            //At the time of serialization, moLeaderColor variable gets serialized in form of color name.
            get
            {
                return this.moLeaderColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into moLeaderColor
            set
            {
                this.moLeaderColor = Color.FromArgb(value);
            }
        }

        public int LeaderWidth
        {
            get { return miLeaderWidth; }
            set { miLeaderWidth = value; }
        }
        public DashStyle LeaderStyle
        {
            get { return meLeaderStyle; }
            set { meLeaderStyle = value; }
        }

        public PointF DrawPoint
        {
            get { return moDrawPoint; }
            set { moDrawPoint = value; }
        }
        public RectangleF PlaceHolder
        {
            get { return moPlaceHolder; }
            set { moPlaceHolder = value; }
        }
        # endregion


    }
}
