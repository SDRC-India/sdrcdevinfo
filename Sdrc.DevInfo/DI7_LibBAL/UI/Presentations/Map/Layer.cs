using System;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Collections.Generic;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    [XmlInclude(typeof(Shape))]
    [XmlInclude(typeof(CustomLabel))]
    public class Layer
    {

        # region " Variables"

        private bool m_Visible = true;
        //Specify the color of custom layers added by the user
        private System.Drawing.Color m_FillColor = Color.FromArgb((int)(VBMath.Rnd() * 255), (int)(VBMath.Rnd() * 255), (int)(VBMath.Rnd() * 255));
        //Polygon Custom Layer
        private FillStyle m_FillStyle = FillStyle.Solid;

        //point type custom layer
        private MarkerStyle m_MarkerStyle = MarkerStyle.Circle;
        //point type custom layer
        private int m_MarkerSize = 10;
        //*** This font should exist on server / desktop
        private Font m_MarkerFont = new Font("Webdings", 10, FontStyle.Regular);
        //MarkerFont and MarkerChar are only valid with MatkerStyle = Custom
        private char m_MarkerChar = "H"[0];

        private float m_BorderSize = 0.01F;
        private DashStyle m_BorderStyle = DashStyle.Solid;
        private Color m_BorderColor = Color.LightGray;

        private bool m_LabelVisible = false;
        //0-AreaId, 1-AreaName
        private string m_LabelField = "1";
        private Font m_LabelFont = new Font("Arial", 8, FontStyle.Regular);
        private Color m_LabelColor = Color.Black;
        private bool m_LabelMultirow;
        private bool m_LabelIndented;
        private RectangleF m_MinVisibleExtent;
        private RectangleF m_MaxVisibleExtent;

        //These Informations to be set while serialization using ShapeFileReader class
        private ShapeType m_LayerType;
        private RectangleF m_Extent = new RectangleF();
        private int m_RecordCount;
        private DateTime m_StartDate;
        private DateTime m_EndDate;

        //These Informations to be set during AddLayer
        //LayerName / ShapeFileName
        private string m_Id;
        private string m_LayerName = "";
        //Will contain the only direcory path
        private string m_LayerPath = "";
        private int m_Area_Level;
        private SourceType m_SourceType;
        //AreaId(Key)-AreaName(Value) 'These area names are picked from Language Table inside database. AreaId doesn't exist inside Database then Name is picked from shape file
        private Hashtable m_AreaNames = new Hashtable();

        //Valid Only for buffer layer
        private string m_BufferTargetLayerId;

        //Collections
        [NonSerialized()]
        //AreaId(Key)- Shape Object(Value)
        private Hashtable m_Records = new System.Collections.Hashtable();
        //AreaId(Key)-
        private StringCollection m_SelectedArea = new StringCollection();
        //AreaId(Key)- Custom Label Object(Value)
        private Hashtable m_ModifiedLabels = new Hashtable();
        # endregion

        # region " Properties"

        //[XmlIgnore()]   //Ignored because LayerPath will be same as SpacialMapPath after Map DeSerialization.
        public string LayerPath
        {
            get { return m_LayerPath; }
            set { m_LayerPath = value; }
        }

        public string BufferTargetLayerId
        {
            get { return m_BufferTargetLayerId; }
            set { m_BufferTargetLayerId = value; }
        }

        [XmlIgnore()]
        public Hashtable ModifiedLabels
        {
            get { return m_ModifiedLabels; }
            set { m_ModifiedLabels = value; }
        }

        /// <summary>
        /// Gets or sets Proxy for ModifiedLabel property
        /// </summary>
        [XmlElement("ModifiedLabels")]
        public HashtableSerializationProxy XmlModifiedLabels
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                return new HashtableSerializationProxy(this.m_ModifiedLabels);
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                this.m_ModifiedLabels = value._hashTable;
            }
        }

        [XmlIgnore()]
        public Hashtable AreaNames
        {
            get { return (m_AreaNames); }
        }

        //This property will serialize Hashtable object "AreaNames" using customised class "HashtableSerializationProxy" which can be serialized.
        [XmlElement("AreaNames")]        
        public HashtableSerializationProxy XmlAreaNames
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                return new HashtableSerializationProxy(m_AreaNames);
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                m_AreaNames = value._hashTable;
            }
        }

        public DateTime StartDate
        {
            get { return m_StartDate; }
            set { m_StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return m_EndDate; }
            set { m_EndDate = value; }
        }

        //[XmlIgnore()]
        public StringCollection SelectedArea
        {
            get { return m_SelectedArea; }
            set { m_SelectedArea = value; }
        }

        public int Area_Level
        {

            get { return m_Area_Level; }
            set { m_Area_Level = value; }
        }

        public string LayerName
        {
            get { return m_LayerName; }
            set { m_LayerName = value; }
        }
        public int RecordCount
        {
            get { return m_RecordCount; }
            set { m_RecordCount = value; }
        }

        public bool LabelMultirow
        {
            get { return m_LabelMultirow; }
            set { m_LabelMultirow = value; }
        }
        public bool LabelIndented
        {
            get { return m_LabelIndented; }
            set { m_LabelIndented = value; }
        }
        public RectangleF MinVisibleExtent
        {
            get { return m_MinVisibleExtent; }
            set { m_MinVisibleExtent = value; }
        }
        public RectangleF MaxVisibleExtent
        {
            get { return m_MaxVisibleExtent; }
            set { m_MaxVisibleExtent = value; }
        } 

        [DescriptionAttribute("get/set the unique ID")]
        public string ID
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public ShapeType LayerType
        {
            get { return m_LayerType; }
            set { m_LayerType = value; }
        }

        private bool _IsDIB = false;
        /// <summary>
        /// Is Disputed International boundaries
        /// </summary>
        public bool IsDIB
        {
            get { return _IsDIB; }
            set { _IsDIB = value; }
        }
	

        public SourceType SourceType
        {
            get { return m_SourceType; }
            set { m_SourceType = value; }
        }


        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }

        [XmlIgnore()]
        public Color FillColor
        {
            get { return m_FillColor; }
            set { m_FillColor = value; }
        }

        //This property is used for m_FillColor variable to get xml serialized.
        [XmlElement("FillColor")]
        public int XmlFillColor
        {
            //At the time of serialization, m_FillColor variable gets serialized in form of color name.
            get
            {
                return this.m_FillColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_FillColor
            set
            {
                this.m_FillColor = Color.FromArgb(value);
            }
        }

        public FillStyle FillStyle
        {
            get { return m_FillStyle; }
            set { m_FillStyle = value; }
        }

        public MarkerStyle MarkerStyle
        {
            get { return m_MarkerStyle; }
            set { m_MarkerStyle = value; }
        }

        public int MarkerSize
        {
            get { return m_MarkerSize; }
            set { m_MarkerSize = value; }
        }

        [XmlIgnore()]
        public Font MarkerFont
        {
            get { return m_MarkerFont; }
            set { m_MarkerFont = value; }
        }

        //This below property is only used for Font property "MarkerFont" to get serialized.
        [XmlElement("MarkerFont")]
        public string XmlMarkerFont
        {
            //At the time of serialization, m_MarkerFont variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                return m_MarkerFont.Name + "," + m_MarkerFont.Size + "," + m_MarkerFont.Style.ToString();
            }
            //At time of Deserialzation, value(string) is split into Font Name, Font Size, Font Style. and restored into m_MarkerFont
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



        public float BorderSize
        {
            get { return m_BorderSize; }
            set { m_BorderSize = value; }
        }

        public DashStyle BorderStyle
        {
            get { return m_BorderStyle; }
            set { m_BorderStyle = value; }
        }

        [XmlIgnore()]
        public Color BorderColor
        {
            get { return m_BorderColor; }
            set { m_BorderColor = value; }
        }

        //This property is used for m_BorderColor variable to get xml serialized.
        [XmlElement("BorderColor")]
        public int XmlBorderColor
        {
            //At the time of serialization, m_BorderColor variable gets serialized after in form of color name.
            get
            {
                return this.m_BorderColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_BorderColor
            set
            {
                this.m_BorderColor = Color.FromArgb(value);
            }
        }

        public bool LabelVisible
        {
            get { return m_LabelVisible; }
            set { m_LabelVisible = value; }
        }

        public string LabelField
        {
            get { return m_LabelField; }
            set { m_LabelField = value; }
        }

        [XmlIgnore()]
        public Font LabelFont
        {
            get { return m_LabelFont; }
            set { m_LabelFont = value; }
        }

        [XmlElement("LabelFont")]
        public string XmlLabelFont
        {
            //At the time of serialization, m_LabelFont variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                return m_LabelFont.Name + "," + m_LabelFont.Size + "," + m_LabelFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into m_LabelFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.m_LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.m_LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        [XmlIgnore()]
        public Color LabelColor
        {
            get { return m_LabelColor; }
            set { m_LabelColor = value; }
        }

        //This property is used for m_LabelColor variable to get xml serialized.
        [XmlElement("LabelColor")]
        public int XmlLabelColor
        {
            //At the time of serialization, m_LabelColor variable gets serialized in form of color name.
            get
            {
                return this.m_LabelColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_LabelColor
            set
            {
                this.m_LabelColor = Color.FromArgb(value);
            }
        }

        private bool _ShowLabelEffects = false;
        /// <summary>
        /// Gets or sets. True if Label effects to be applied.
        /// </summary>
        public bool ShowLabelEffects
        {
            get { return _ShowLabelEffects; }
            set { _ShowLabelEffects = value; }
        }

        private LabelEffectSetting _LabelEffectSetting = null;
        /// <summary>
        /// Gets or sets the Label Effects Settings.
        /// </summary>
        public LabelEffectSetting LabelEffectSettings
        {
            get { return _LabelEffectSetting; }
            set { _LabelEffectSetting = value; }
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

        [DescriptionAttribute("Layer extent")]
        public RectangleF Extent
        {
            get { return m_Extent; }
            set { m_Extent = value; }
        }

        public Hashtable GetRecords(string p_FileName)
        {

            if ((m_Records == null) || m_Records.Count <= 0)
            {
                if (File.Exists(p_FileName + ".shp"))
                {
                    ShapeInfo _ShapeInfo;
                    ShapeFileReader sfr = new ShapeFileReader();
                    _ShapeInfo = ShapeFileReader.GetShapeInfo(Path.GetDirectoryName(p_FileName), Path.GetFileName(p_FileName));
                    sfr = null;

                    m_Records = _ShapeInfo.Records;

                }
                else
                {
                    m_Records = new Hashtable();
                }
                //ShapeInfo.SetLayerInfo(Me, p_FileName)
            }
            return m_Records;
        }

        [XmlIgnore()]       //Ignored because records will be fetched from shapefiles after Deserialization.
        public Hashtable Records
        {
            get { return m_Records; }
            set { m_Records = value; }
        }

        private List<string> _DrilledDownLayers;
        /// <summary>
        /// Gets or sets the list of IDs of Drill down Layers associated with this Layers.
        /// </summary>
        public List<string> DrilledDownLayers
        {
            get { return _DrilledDownLayers; }
            set { _DrilledDownLayers = value; }
        }

        private bool _IsDrilledDown;
        /// <summary>
        /// True if Layer is being drilled down.
        /// </summary>
        public bool IsDrilledDown
        {
            get { return _IsDrilledDown; }
            set { _IsDrilledDown = value; }
        }
	
        # endregion

        #  region " Methods"
        public string GetArea(PointF p_Point)
        {
            Shape _Shape;
            int i;
            GraphicsPath gp = new GraphicsPath();
            IDictionaryEnumerator dicEnumerator = Records.GetEnumerator();
            //*** use property instead of private variable for web
            if (m_LayerType == ShapeType.Polygon)
            {
                while (dicEnumerator.MoveNext())
                {
                    //Traverse Shapes
                    _Shape = (Shape)dicEnumerator.Value;
                    gp.Reset();
                    for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                    {
                        gp.AddPolygon((PointF[])_Shape.Parts[i]);
                    }
                    if (gp.IsVisible(p_Point))
                    {
                        return _Shape.AreaName;
                    }
                }
            }
            return "";
        }
        # endregion

    }

}