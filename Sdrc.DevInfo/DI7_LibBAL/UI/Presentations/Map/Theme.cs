using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
//using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualBasic;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

//**************************************************************************************************
//Change No     Date                    Description
// C1           10-June-2008            Legend.Caption property is added. While drawing Legend, Legend.Caption is concatenated with Legend.title (e.g: Low -(21.2 - 45.6)  )
//**************************************************************************************************
namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    [XmlInclude(typeof(Legends))]
    [XmlInclude(typeof(Legend))]
    [XmlInclude(typeof(PointF))]
    [XmlInclude(typeof(AreaInfo))]

    public class Theme : ICloneable
    {

        # region " Variables"

        //*** Stores I_U_S_Type as key for collection '$$$ Uniqueness of a theme is defined By Theme ID
        private string m_Id = "";
        private string m_Name = "";

        private ThemeType m_ThemeType;
        private string m_IndicatorNID;
        private string m_IndicatorName;
        private int m_UnitNID;
        private string m_UnitName;
        private string m_SubgroupNID;
        private string m_SubgroupName;

        private bool m_Visible = true;
        private int m_ShapeCount;
        //*** Used by interface to show legend in expanded or collapsed form
        private bool m_LegendIsExpanded = true;
        internal bool FirstBuild = true;

        //*** Properties for ThemeType = Symbol and Label
        private int m_X_Offset = 0;
        private int m_Y_Offset = 0;

        //*** Properties for ThemeType = DotDensity
        private double m_DotValue;
        private float m_DotSize = 3;
        private Color m_DotColor = Color.Black;
        private MarkerStyle m_DotStyle;
        //$$$ This font should exist on server / desktop
        private Font m_DotFont = new Font("Webdings", 6);
        //DotFont and DotChar are only valid with DotStyle = Custom
        private char m_DotChar = "H"[0];

        //*** Properties for ThemeType = Chart
        //*** Comma delimited Hex Color values for each Indicator
        private string m_IndicatorColor;
        //*** Comma delimited Color Int values for each Subgroup
        private string m_SubgroupFillStyle;
        //*** Comma delimited Visibility Boolean for each subgroup
        private string m_SubgroupVisible;
        private ChartType m_ChartType = ChartType.Column;
        //*** ChartSize.ShapeBased
        private int m_ChartSize = 8;
        //*** ChartSize.ShapeBased '*** Bugfix / Enhancement 02 May 2006 Controling chart width
        private int m_ChartWidth = 8;
        private bool m_DisplayChartData = true;
        //*** LayerId_AreaId(Key)- DrawPoint Pt structure (Value)
        private Hashtable m_ModifiedCharts = new Hashtable();
        private bool mbChartLeaderVisible;
        private Color moChartLeaderColor = Color.LightGray;
        private int miChartLeaderWidth = 1;
        private DashStyle meChartLeaderStyle = DashStyle.Solid;

        //*** Set ChartLegend Max Width According to Map Width
        private float m_LegendMaxWidth = 0;

        //*** Properties for ThemeType = Color
        private BreakType m_BreakType = BreakType.EqualCount;
        private int m_BreakCount = 4;
        private int m_Decimals;
        private decimal m_Minimum;
        private decimal m_Maximum;
        private Legends m_Legends = new Legends();

        private bool m_MultiLegend;
        //SRC,SGP,MD_IND_1,.... 'MultiLegendField = MultiLegendField.None
        private string m_MultiLegendField = "";
        //$$$ MultiLegendFieldValue (Key) - Value (Legends)
        private Hashtable m_MultiLegendCol = new Hashtable();

        private string m_DefaultStartColor;
        private string m_DefaultEndColor;
        private Color m_MissingColor = Color.Gray;

        private Color m_BorderColor = Color.LightGray;
        private float m_BorderWidth = 1;
        private DashStyle m_BorderStyle = DashStyle.Solid;

        //*** Properties for Legend Settings
        private string m_LegendTitle = "";
        private Font m_LegendFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        private Color m_LegendColor = Color.Black;
        private Font m_LegendBodyFont = new Font("Microsoft Sans Serif", 8);
        private Color m_LegendBodyColor = Color.Black;

        //*** Properties for Label
        private bool m_LabelVisible = false;
        //$$$ For Multiple Fields Concat respective Field Number 012345 0-AreaId, 1-AreaName, 2-DataValue, 3-Unit, 4-Subgroup, 5-TimePeriod
        private string m_LabelField = "1";
        private Font m_LabelFont = new Font("Arial", 8, FontStyle.Regular);
        private Color m_LabelColor = Color.Black;
        private bool m_LabelMultirow;
        private bool m_LabelIndented;

        //$$$ AreaID(Key) - Value (AreaInfo Structure)
        private Hashtable m_AreaIndex = new Hashtable();
        //
        [XmlIgnore()]
        public HashtableSerializationProxy _XmlAreaIndexes = new HashtableSerializationProxy();
        //$$$ LayerId(Key) - Layer visibility (Boolean Value)
        private Hashtable m_LayerVisibility = new Hashtable();

        private DataTable m_AreaIndexDT;
        /// <summary>
        /// Gets AreaID - DataValue mapping
        /// </summary>
        public DataTable AreaIndexDT
        {
            get
            {
                return this.m_AreaIndexDT;
            }
        }

        [NonSerialized()]
        private DataView m_ThemeData;
        private string m_ActualFilter = "";
        private string m_ActualSort = "";

        private object[] m_MDKeys;

        private bool ShowLegendCaption = false;
        private bool ShowLegendRange = true;
        private bool ShowAreaCount = false;

        # endregion

        # region " Properties"

        # region " Theme General"
        public string ID
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public ThemeType Type
        {
            get { return m_ThemeType; }
            set { m_ThemeType = value; }
        }

        private int _IUSNid;
        /// <summary>
        /// Gets the IUSNId of associated with current theme.
        /// </summary>
        /// <remarks> In case of Chart Theme, IUSnid = -1 , due to multiple Subgroups</remarks>
        public int IUSNid
        {
            get
            {
                if (this.Type == ThemeType.Chart)
                {
                    this._IUSNid = -1;
                }

                return this._IUSNid;
            }
        }

        private string[] _I_U_S_GIDs = new string[2];
        /// <summary>
        /// Gets the GIDs of Indicator, Unit & SubgroupVal used in this Theme.
        /// </summary>
        public string[] I_U_S_GIDs
        {
            get { return _I_U_S_GIDs; }
            set { _I_U_S_GIDs = value; }
        }
	

        public string[] IndicatorNId
        {

            get
            {
                //Microsoft.VisualBasic.Strings.RTrim

                return Strings.Split(m_IndicatorNID, ",", -1, CompareMethod.Text);
            }
            set
            {
                m_IndicatorNID = "";
                foreach (string _String in value)
                {
                    if (m_IndicatorNID.Length > 0)
                        m_IndicatorNID += ",";
                    m_IndicatorNID += _String;
                }
            }
        }

        public string[] IndicatorName
        {
            get { return Strings.Split(m_IndicatorName, "{~}", -1, CompareMethod.Text); }
            set
            {
                m_IndicatorName = "";
                foreach (string _String in value)
                {
                    if (m_IndicatorName.Length > 0)
                        m_IndicatorName += "{~}";
                    m_IndicatorName += _String;
                }
            }
        }

        public string[] SubgroupNId
        {
            get { return Strings.Split(m_SubgroupNID, ",", -1, CompareMethod.Text); }
            set
            {
                m_SubgroupNID = "";
                foreach (string _String in value)
                {
                    if (m_SubgroupNID.Length > 0)
                        m_SubgroupNID += ",";
                    m_SubgroupNID += _String;
                }
            }
        }

        public string[] SubgroupName
        {
            get { return Strings.Split(m_SubgroupName, "{~}", -1, CompareMethod.Text); }
            set
            {
                m_SubgroupName = "";
                foreach (string _String in value)
                {
                    if (m_SubgroupName.Length > 0)
                        m_SubgroupName += "{~}";
                    m_SubgroupName += _String;
                }
            }
        }
        public int UnitNId
        {
            get { return m_UnitNID; }
            set { m_UnitNID = value; }
        }
        public string UnitName
        {
            get { return m_UnitName; }
            set { m_UnitName = value; }
        }

        private string[] _SourceName;
        /// <summary>
        /// Gets or sets Source names string array. (Applicable for Chart dataValues, will be plotted for single IUS)
        /// </summary>
        public string[] SourceName
        {
            get { return _SourceName; }
            set { _SourceName = value; }
        }

        private string[] _SourceNIds;
        /// <summary>
        /// Gets or sets Source NIds string array. (Applicable for Chart dataValues, will be plotted for single IUS)
        /// </summary>
        public string[] SourceNIds
        {
            get { return _SourceNIds; }
            set { _SourceNIds = value; }
        }

        public bool Visible
        {
            get { return m_Visible; }
            set { m_Visible = value; }
        }
        public int ShapeCount
        {
            set { m_ShapeCount = value; }
        }
        public bool LegendIsExpanded
        {
            get { return m_LegendIsExpanded; }
            set { m_LegendIsExpanded = value; }
        }

        private bool _SubgroupSelectAll = false;
        /// <summary>
        /// true / false. Indicates whether All Subgroups are selected when Legend Based themes are created.
        /// </summary>
        public bool SubgroupSelectAll
        {
            get { return _SubgroupSelectAll; }
            set { _SubgroupSelectAll = value; }
        }
	
        # endregion

        # region " Symbol and Label"
        public int X_Offset
        {
            get { return m_X_Offset; }
            set { m_X_Offset = value; }
        }

        public int Y_Offset
        {
            get { return m_Y_Offset; }
            set { m_Y_Offset = value; }
        }
        # endregion

        # region " Dot Density"
        public double DotValue
        {
            get { return m_DotValue; }
            set { m_DotValue = value; }
        }
        public float DotSize
        {
            get { return m_DotSize; }
            set
            {
                m_DotSize = value;
                if (m_DotStyle == MarkerStyle.Custom)
                    m_DotFont = new Font(m_DotFont.FontFamily, value);
            }
        }

        [XmlIgnore()]
        public Color DotColor
        {
            get { return m_DotColor; }
            set { m_DotColor = value; }
        }

        //This property is used for m_DotColor variable to get xml serialized.
        [XmlElement("DotColor")]
        public int XmlDotColor
        {
            //At the time of serialization, m_DotColor variable gets serialized in form of color name string.
            get
            {
                return this.m_DotColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_DotColor
            set
            {
                this.m_DotColor = Color.FromArgb(value);
            }
        }

        public MarkerStyle DotStyle
        {
            get { return m_DotStyle; }
            set { m_DotStyle = value; }
        }

        [XmlIgnore()]
        public Font DotFont
        {
            get { return m_DotFont; }
            set { m_DotFont = value; }
        }

        //This below property is only used for Font property "DotFont" to get serialized.
        [XmlElement("DotFont")]
        public string XmlDotFont
        {
            //At the time of serialization, m_DotFont variable gets serialized into string (Font Name + Font Size)
            get
            {
                return m_DotFont.Name + "," + m_DotFont.Size;
            }
            //At time of Deserialzation, value(string) is split into Font Name & Font Size and restored into m_DotFont
            set
            {
                string[] _FontSettings = new string[2];
                _FontSettings = value.Split(",".ToCharArray());
                this.m_DotFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
            }
        }
        public char DotChar
        {
            get { return m_DotChar; }
            set { m_DotChar = value; }
        }
        # endregion

        # region " Chart"
        public string[] IndicatorColor
        {
            get { return Strings.Split(m_IndicatorColor, ",", -1, CompareMethod.Text); }
            set
            {
                m_IndicatorColor = "";
                foreach (string _String in value)
                {
                    if (m_IndicatorColor.Length > 0)
                        m_IndicatorColor += ",";
                    m_IndicatorColor += _String;
                }
            }
        }
        public string[] SubgroupVisible
        {
            get { return Strings.Split(m_SubgroupVisible, ",", -1, CompareMethod.Text); }
            set
            {
                m_SubgroupVisible = "";
                foreach (string _String in value)
                {
                    if (m_SubgroupVisible.Length > 0)
                        m_SubgroupVisible += ",";
                    m_SubgroupVisible += _String;
                }
            }
        }

        public string[] SubgroupFillStyle
        {
            get { return Strings.Split(m_SubgroupFillStyle, ",", -1, CompareMethod.Text); }
            set
            {
                m_SubgroupFillStyle = "";
                foreach (string _String in value)
                {
                    if (m_SubgroupFillStyle.Length > 0)
                        m_SubgroupFillStyle += ",";
                    m_SubgroupFillStyle += _String;
                }
            }
        }

        public ChartType ChartType
        {
            get { return m_ChartType; }
            set { m_ChartType = value; }
        }
        public int ChartSize
        {
            get { return m_ChartSize; }
            set { m_ChartSize = value; }
        }
        public int ChartWidth
        {
            get { return m_ChartWidth; }
            set { m_ChartWidth = value; }
        }

        /// <summary>
        /// Gets & Sets Legend Maximum width according to Map Width 
        /// </summary>
        public float LegendMaxWidth
        {
            get { return m_LegendMaxWidth; }
            set { m_LegendMaxWidth = value; }
        }

        private int _ColumnsGap = 1;
        /// <summary>
        ///  Gets or sets the gap between the column Charts of two consecutive Timeperiods.
        /// </summary>
        public int ColumnsGap
        {
            get { return _ColumnsGap; }
            set { _ColumnsGap = value; }
        }

        private bool _ShowChartAxis = true;
        /// <summary>
        /// gets or sets the bool value indicating Axis to visible or not. (Only in Column & Line Chart)
        /// </summary>
        public bool ShowChartAxis
        {
            get { return _ShowChartAxis; }
            set { _ShowChartAxis = value; }
        }

        private Color _ChartAxisColor = Color.Black;
        /// <summary>
        /// Gets or Sets the color of Axis in Column or Line Chart.
        /// </summary>
        [XmlIgnore()]
        public Color ChartAxisColor
        {
            get { return this._ChartAxisColor; }
            set { this._ChartAxisColor = value; }
        }

        //This property is used for _ChartAxisColor variable to get xml serialized.
        public int XmlChartAxisColor
        {
            //At the time of serialization, _ChartAxisColor variable gets serialized in form of color string name.
            get
            {
                return this._ChartAxisColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into _ChartAxisColor
            set
            {
                this._ChartAxisColor = Color.FromArgb(value);
            }
        }

        private CustomLabelSetting _ChartAxisLabelSettings = new CustomLabelSetting();
        /// <summary>
        /// Gets or sets the Chart's Axis label settings like Font, Color & Orientation.
        /// </summary>
        public CustomLabelSetting ChartAxisLabelSettings
        {
            get { return _ChartAxisLabelSettings; }
            set { _ChartAxisLabelSettings = value; }
        }

        private CustomLabelSetting _ChartDataLabelSettings = new CustomLabelSetting();
        /// <summary>
        /// Gets or sets the Chart's Data LabelSettings object (settings like Font, Color & Orientation).
        /// </summary>
        public CustomLabelSetting ChartDataLabelSettings
        {
            get { return _ChartDataLabelSettings; }
            set { _ChartDataLabelSettings = value; }
        }

        private int _ChartLineThickness = 1;    // Default = 1
        /// <summary>
        /// Gets or sets Line thickness width in Line Charts.
        /// </summary>
        public int ChartLineThickness
        {
            get { return this._ChartLineThickness; }
            set { this._ChartLineThickness = value; }
        }

        private int _RoundDecimals = 1; //Default
        /// <summary>
        /// Gets or sets the decimal places used to round off Chart data values.
        /// </summary>
        public int RoundDecimals
        {
            get { return this._RoundDecimals; }
            set { this._RoundDecimals = value; }
        }


        private ChartSeriesType _ChartSeriesType = ChartSeriesType.Subgroup;      // Default
        /// <summary>
        /// Gets or Sets ChartGroupBy Enum value.
        /// </summary>
        public ChartSeriesType ChartSeriestype
        {
            get { return _ChartSeriesType; }
            set { _ChartSeriesType = value; }
        }

        ////private List<> _ChartVisibleTimePeriods = new List<>();
        /////// <summary>
        /////// Gets or Sets the TimePeriods visibilty, used to plot dataValue in Chart Theme.
        /////// </summary>
        ////public List<> ChartVisibleTimePeriods
        ////{
        ////    get { return this._ChartVisibleTimePeriods; }
        ////    set { this._ChartVisibleTimePeriods = value; }
        ////}

        private bool _DisplayChartMRD = false;
        /// <summary>
        /// Gets or sets bool value indicating Chart data is displayed for Most recent TimePeriod or NOT.
        /// </summary>
        /// <remarks>For Line Chart, DisplayChartMRD is always False</remarks>
        public bool DisplayChartMRD
        {
            get { return _DisplayChartMRD; }
            set { _DisplayChartMRD = value; }
        }


        private SortedList<string, bool> _ChartTimePeriods = new SortedList<string, bool>();
        /// <summary>
        /// Gets the TimePeriods of Chart Theme.
        /// </summary>
        [XmlIgnore()]
        public SortedList<string, bool> ChartTimePeriods
        {
            get { return _ChartTimePeriods; }
            set { this._ChartTimePeriods = value; }
        }

        /// <summary>
        /// Chart TimePeriods in Comma delimited string format.
        /// </summary>
        /// <remarks>This property is used for xml serialization of this._ChartTimePeriod</remarks>
        [XmlElement("ChartTimePeriods")]
        public string[] XmlChartTimePeriods
        {
            get // Serialization
            {
                string[] RetVal = new string[1];
                //Get comma delimited string of TimePeriods in Chart theme
                if (this._ChartTimePeriods != null)
                {
                    RetVal = new string[this._ChartTimePeriods.Count];
                    for (int i = 0; i < this._ChartTimePeriods.Count; i++)
                    {
                        //-- Append timeperiod visibilty value with timeperiod string.
                        //-- For e.g: 2001^true
                        RetVal[i] = this._ChartTimePeriods.Keys[i].ToString() + "{^}" + this._ChartTimePeriods.Values[i].ToString();
                    }
                }
                return RetVal;
            }
            set     // Deserialization
            {
                if (value != null && value.Length > 0)
                {
                    this._ChartTimePeriods = new SortedList<string, bool>();
                    string[] TimeSplitted;
                    foreach (string timePeriodStr in value)
                    {
                        if (string.IsNullOrEmpty(timePeriodStr) == false)
                        {
                            TimeSplitted = DICommon.SplitString(timePeriodStr, "{^}");
                            if (TimeSplitted.Length == 2)
                            {
                                this._ChartTimePeriods.Add(TimeSplitted[0], Convert.ToBoolean(TimeSplitted[1]));
                            }
                            else
                            {
                                this._ChartTimePeriods.Add(timePeriodStr, true);
                            }
                        }
                    }
                }
            }
        }

        private string[] _SourceVisible;
        /// <summary>
        /// TODO
        /// </summary>
        public string[] SourceVisible
        {
            get { return _SourceVisible; }
            set { _SourceVisible = value; }
        }


        private int _PieSize = 1;
        /// <summary>
        /// Gets or sets Pie's diameter Size .
        /// </summary>
        public int PieSize
        {
            get { return _PieSize; }
            set { _PieSize = value; }
        }

        private bool _PieAutoSize = true;
        /// <summary>
        /// Gets or sets the Pie Auto Size status (true if Auto Size is required) 
        /// </summary>
        public bool PieAutoSize
        {
            get { return _PieAutoSize; }
            set { _PieAutoSize = value; }
        }

        private float _PieAutoSizeFactor = 1.0F;
        /// <summary>
        /// Factor (float value) by which Pie chart's auto Area will be multiplied.
        /// </summary>
        public float PieAutoSizeFactor
        {
            get { return _PieAutoSizeFactor; }
            set { _PieAutoSizeFactor = value; }
        }

        public bool DisplayChartData
        {
            get { return m_DisplayChartData; }
            set { m_DisplayChartData = value; }
        }
        [XmlIgnore()]
        public Hashtable ModifiedCharts
        {
            get { return m_ModifiedCharts; }
            set { m_ModifiedCharts = value; }
        }

        //This property will serialize Hashtable object "ModifiedCharts" using customised class "HashtableSerializationProxy" which can be serialized.
        [XmlElement("ModifiedCharts")]
        // [XmlIgnore()] 
        public HashtableSerializationProxy XmlModifiedCharts
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                return new HashtableSerializationProxy(m_ModifiedCharts);
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                m_ModifiedCharts = value._hashTable;
            }
        }

        public bool ChartLeaderVisible
        {
            get { return mbChartLeaderVisible; }
            set { mbChartLeaderVisible = value; }
        }
        [XmlIgnore()]
        public Color ChartLeaderColor
        {
            get { return moChartLeaderColor; }
            set { moChartLeaderColor = value; }
        }

        //This property is used for moChartLeaderColor variable to get xml serialized.
        [XmlElement("ChartLeaderColor")]
        public int XmlChartLeaderColor
        {
            //At the time of serialization, moChartLeaderColor variable gets serialized in form of color string name.
            get
            {
                return this.moChartLeaderColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into moChartLeaderColor
            set
            {
                this.moChartLeaderColor = Color.FromArgb(value);
            }
        }

        public int ChartLeaderWidth
        {
            get { return miChartLeaderWidth; }
            set { miChartLeaderWidth = value; }
        }

        public DashStyle ChartLeaderStyle
        {
            get { return meChartLeaderStyle; }
            set { meChartLeaderStyle = value; }
        }

        private List<string> _ExcludeAreaIDs = new List<string>();
        /// <summary>
        /// Gets or sets the List of AreaIDs which are NOT included in Chart Rendering.
        /// </summary>
        public List<string> ExcludeAreaIDs
        {
            get { return this._ExcludeAreaIDs; }
            set { this._ExcludeAreaIDs = value; }
        }

        # endregion

        # region " Color"
        public BreakType BreakType
        {
            get { return m_BreakType; }
            set { m_BreakType = value; }
        }
        public int BreakCount
        {
            get { return m_BreakCount; }
            set { m_BreakCount = value; }
        }
        public int Decimals
        {
            get { return m_Decimals; }
            set { m_Decimals = value; }
        }
        public decimal Minimum
        {
            get { return m_Minimum; }
            set { m_Minimum = value; }
        }
        public decimal Maximum
        {
            get { return m_Maximum; }
            set { m_Maximum = value; }
        }

        //[XmlArray("Legends")]
        public Legends Legends
        {
            get { return m_Legends; }
            set { m_Legends = value; }  //This line is added only for the property to get Deserialze.
        }

        public bool MultiLegend
        {
            get { return m_MultiLegend; }
            set { m_MultiLegend = value; }
        }

        public string MultiLegendCriteria
        {
            get { return m_MultiLegendField; }
            set { m_MultiLegendField = value; }
        }
        [XmlIgnore()]
        public Hashtable MultiLegendCol
        {
            get { return m_MultiLegendCol; }
        }

        //This property will serialize Hashtable object "MultiLegendCol" using customised class "HashtableSerializationProxy" which can be serialized.
        [XmlElement("MultiLegendCol")]
        public HashtableSerializationProxy XmlMultiLegendCol
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                return new HashtableSerializationProxy(m_MultiLegendCol);
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                m_MultiLegendCol = value._hashTable;
            }
        }

        public string StartColor
        {
            //Get is only used for Srialization of property, otherwise it is treated as Writeonly in Hosting application                    
            get { return m_DefaultStartColor; }
            set { m_DefaultStartColor = value; }

        }
        public string EndColor
        {
            //Get is only used for Srialization of property, otherwise it is treated as Writeonly in Hosting application                    
            get { return m_DefaultEndColor; }
            set { m_DefaultEndColor = value; }

        }

        [XmlIgnore()]
        public Color MissingColor
        {
            get { return m_MissingColor; }
            set { m_MissingColor = value; }
        }

        //This property is used for m_BorderColor variable to get xml serialized.
        [XmlElement("MissingColor")]
        public int XmlMissingColor
        {
            //At the time of serialization, m_MissingColor variable gets serialized in form of color string name.
            get
            {
                return this.m_MissingColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_MissingColor
            set
            {
                this.m_MissingColor = Color.FromArgb(value);
            }
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
            //At the time of serialization, m_BorderColor variable gets serialized in form of color string name.
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

        public float BorderWidth
        {
            get { return m_BorderWidth; }
            set { m_BorderWidth = value; }
        }
        public DashStyle BorderStyle
        {
            get { return m_BorderStyle; }
            set { m_BorderStyle = value; }
        }
        # endregion

        # region " Legend Settings"
        public string LegendTitle
        {
            get { return m_LegendTitle; }
            set { m_LegendTitle = value; }
        }

        [XmlIgnore()]
        public Font LegendFont
        {
            get { return m_LegendFont; }
            set { m_LegendFont = value; }
        }
        //This below property is only used for Font property "LegendFont" to get serialized.
        [XmlElement("LegendFont")]
        public string XmlLegendFont
        {
            //At the time of serialization, m_LegendFont variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                return m_LegendFont.Name + "," + m_LegendFont.Size + "," + m_LegendFont.Style.ToString();
            }
            //At time of Deserialzation, value(string) is split into Font Name & Font Size and restored into m_LegendFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.m_LegendFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.m_LegendFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        [XmlIgnore()]
        public Color LegendColor
        {
            get { return m_LegendColor; }
            set { m_LegendColor = value; }
        }
        //This property is used for m_LegendColor variable to get xml serialized.
        [XmlElement("LegendColor")]
        public int XmlLegendColor
        {
            //At the time of serialization, m_LegendColor variable gets serialized in form of color string name.
            get
            {
                return this.m_LegendColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_LegendColor
            set
            {
                this.m_LegendColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Font LegendBodyFont
        {
            get { return m_LegendBodyFont; }
            set { m_LegendBodyFont = value; }
        }

        //This below property is only used for Font property "LegendBodyFont" to get serialized.
        [XmlElement("LegendBodyFont")]
        public string XmlLegendBodyFont
        {
            //At the time of serialization, m_LegendBodyFont variable gets serialized into string (Font Name + Font Size)
            get
            {
                return m_LegendBodyFont.Name + "," + m_LegendBodyFont.Size + "," + m_LegendBodyFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into m_LegendBodyFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.m_LegendBodyFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.m_LegendBodyFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        [XmlIgnore()]
        public Color LegendBodyColor
        {
            get { return m_LegendBodyColor; }
            set { m_LegendBodyColor = value; }
        }

        //This property is only used for m_LegendBodyColor variable to get xml serialized.
        [XmlElement("LegendBodyColor")]
        public int XmlLegendBodyColor
        {
            //At the time of serialization, m_LegendBodyColor variable gets serialized after color translation.
            get
            {
                return this.m_LegendBodyColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_LegendBodyColor
            set
            {
                this.m_LegendBodyColor = Color.FromArgb(value);
            }
        }

        # endregion

        # region " Labels"
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
        //This below property is only used for Font property "LabelFont" to get serialized.
        [XmlElement("LabelFont")]
        public string XmlLabelFont
        {

            //At the time of serialization, m_LabelFont variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                return m_LabelFont.Name + "," + m_LabelFont.Size + "," + m_LabelFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size & style , and restored into m_LabelFont
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
            //At the time of serialization, m_LabelColor variable gets serialized in form of color string name..
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

        # endregion

        # region " Misc"

        [XmlIgnore()]
        public Hashtable AreaIndexes
        {
            get { return m_AreaIndex; }
            set { m_AreaIndex = value; }
        }

        //This property will serialize Hashtable object "AreaIndexes" using customised class "HashtableSerializationProxy" which can be serialized.
        [XmlElement("AreaIndexes")]
        //[XmlIgnore()]
        public HashtableSerializationProxy XmlAreaIndexes
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                _XmlAreaIndexes = new HashtableSerializationProxy(m_AreaIndex);
                return _XmlAreaIndexes;
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                m_AreaIndex = value._hashTable;
            }
        }

        [XmlIgnore()]
        public Hashtable LayerVisibility
        {
            get { return m_LayerVisibility; }
        }

        //This property will serialize Hashtable object "LayerVisibility" using customised class "HashtableSerializationProxy" which can be serialized.
        [XmlElement("LayerVisibility")]
        public HashtableSerializationProxy XmlLayerVisibility
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                return new HashtableSerializationProxy(m_LayerVisibility);
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                m_LayerVisibility = value._hashTable;
            }
        }

        # endregion

        # region " Metadata"

        [XmlIgnore()]
        public object[] MetaDataKeys  //MetaDataKeys holds Array of keys of MDCaption Hashtable present in Map class.
        {
            get { return m_MDKeys; }
            set { m_MDKeys = value; }
        }

        // Below property used for MetaDataKeys property to get xml-serialised in form of string[].
        [XmlElement("MetaDataKeys")]
        public string[] XmlMetaDataKeys
        {
            get
            {
                if (m_MDKeys != null)
                {
                    string[] _xmlMDKeys = new string[m_MDKeys.Length];
                    Array.Copy(m_MDKeys, _xmlMDKeys, m_MDKeys.Length);

                    return _xmlMDKeys;
                }
                else
                {
                    string[] _xmlMDKeys = new string[0];
                    return _xmlMDKeys;
                }
            }
            set
            {
                if (value == null)
                {
                    m_MDKeys = new object[0];
                }
                else
                {
                    m_MDKeys = value;
                }

            }
        }

        # endregion

        

        # endregion

        # region " Methods "

        # region " Save Load"
        public void Save(string p_FileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(p_FileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(p_FileName));
            if (File.Exists(p_FileName))
                throw new ApplicationException("File with this name already exist.");
            System.IO.FileStream _IO = new System.IO.FileStream(p_FileName, FileMode.Create);
            XmlSerializer _SRZFrmt = new XmlSerializer(typeof(Theme));
            _SRZFrmt.Serialize(_IO, this);
            _IO.Flush();
            _IO.Close();
        }

        public static Theme Load(string p_FileName)
        {
            Theme RetVal = null;

            if (File.Exists(p_FileName))
            {
                try
                {
                    System.IO.FileStream _IO = new FileStream(p_FileName, FileMode.Open);
                    XmlSerializer _SRZFrmt = new XmlSerializer(typeof(Theme));
                    RetVal = (Theme)_SRZFrmt.Deserialize(_IO);
                    _IO.Flush();
                    _IO.Close();
                }
                catch (Exception ex)
                { }
            }
            return RetVal;
        }


        public object Clone()
        {
            object RetVal = null;
            try
            {
                //*** Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter oBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Theme));
                MemoryStream oMemStream = new MemoryStream();
                oBinaryFormatter.Serialize(oMemStream, this);
                oMemStream.Position = 0;
                RetVal = (Theme)oBinaryFormatter.Deserialize(oMemStream);
                oMemStream.Close();
                oMemStream.Dispose();
                oMemStream = null;
            }
            catch
            {

            }
            return (Theme)RetVal;
        }

        # endregion

        # region " Color Ramp"

        public void Smooth(string p_StartColor, string p_EndColor)
        {
            string p_MultilegendKey = "";
            Smooth(p_StartColor, p_EndColor, p_MultilegendKey);
        }

        public void Smooth(string p_StartColor, string p_EndColor, string p_MultilegendKey)
        {
            Color _StartColor;
            Color _EndColor;
            _StartColor = Color.FromArgb(int.Parse(p_StartColor));
            _EndColor = Color.FromArgb(int.Parse(p_EndColor));

            if (p_MultilegendKey == "")
            {
                BuildRangeColor(_StartColor, _EndColor);
            }
            else
            {
                BuildRangeColor(_StartColor, _EndColor, (Legends)m_MultiLegendCol[p_MultilegendKey]);
            }
        }

        public void Smooth(Color p_StartColor, Color p_EndColor)
        {
            string p_MultilegendKey = "";
            Smooth(p_StartColor, p_EndColor, p_MultilegendKey);
        }

        public void Smooth(Color p_StartColor, Color p_EndColor, string p_MultilegendKey)
        {
            if (p_MultilegendKey == "")
            {
                BuildRangeColor(p_StartColor, p_EndColor);
            }
            else
            {
                BuildRangeColor(p_StartColor, p_EndColor, (Legends)m_MultiLegendCol[p_MultilegendKey]);
            }
        }

        public void Smooth()
        {
            if (m_MultiLegend == true)
            {
                IDictionaryEnumerator MultiLegendEnumerator = m_MultiLegendCol.GetEnumerator();
                while (MultiLegendEnumerator.MoveNext())
                {
                    {
                        BuildRangeColor(((Legends)MultiLegendEnumerator.Value)[0].Color, ((Legends)MultiLegendEnumerator.Value)[((Legends)MultiLegendEnumerator.Value).Count - 2].Color, (Legends)MultiLegendEnumerator.Value);
                    }

                }
            }
            else
            {
                BuildRangeColor(m_Legends[0].Color, m_Legends[m_BreakCount - 1].Color);
            }

        }

        private void BuildRangeColor(Color p_StartColor, Color p_EndColor)
        {
            Legends _Legends = null;
            BuildRangeColor(p_StartColor, p_EndColor, _Legends);
        }

        private void BuildRangeColor(Color p_StartColor, Color p_EndColor, Legends _Legends)
        {

            int iBreaks = m_BreakCount;
            int A1;
            int A2;

            int R1;
            int R2;

            int G1;
            int G2;

            int B1;
            int B2;

            int TA1;
            int TR1;
            int TG1;
            int TB1;

            float AInterval;
            float RInterval;
            float GInterval;
            float BInterval;
            int iCtr;

            if ((_Legends == null))
            {
                iBreaks = m_BreakCount;
            }
            else
            {
                iBreaks = _Legends.Count - 1;
            }

            A1 = p_StartColor.A;
            R1 = p_StartColor.R;
            G1 = p_StartColor.G;
            B1 = p_StartColor.B;
    
            A2 = p_EndColor.A;
            R2 = p_EndColor.R;
            G2 = p_EndColor.G;
            B2 = p_EndColor.B;

            //-- get the difference between the R, G, B and divide it by number of breaks to get the interval
            
            AInterval = Convert.ToSingle(Math.Abs((A1 - A2) / (iBreaks - 1)));
            RInterval = Convert.ToSingle(Math.Abs((R1 - R2) / (iBreaks - 1)));
            GInterval = Convert.ToSingle(Math.Abs((G1 - G2) / (iBreaks - 1)));
            BInterval = Convert.ToSingle(Math.Abs((B1 - B2) / (iBreaks - 1)));

            for (iCtr = 1; iCtr <= iBreaks; iCtr++)
            {
                if (iCtr == 1)
                {
                   //-- first color
                      TA1 = p_StartColor.A;
                   TR1 = p_StartColor.R;
                    TG1 = p_StartColor.G;
                    TB1 = p_StartColor.B;
                }
                else if (iCtr == iBreaks)
                {
                    //-- last color
                    TA1 = p_EndColor.A;
                    TR1 = p_EndColor.R;
                    TG1 = p_EndColor.G;
                    TB1 = p_EndColor.B;
                }
                else
                {

                    if (A1 > A2)
                    {
                        TA1 = Convert.ToInt16(A1 - (AInterval * (iCtr - 1)));
                    }
                    else
                    {
                        TA1 = Convert.ToInt16(A1 + (AInterval * (iCtr - 1)));
                    }

                    if (R1 > R2)
                    {
                        TR1 = Convert.ToInt16(R1 - (RInterval * (iCtr - 1)));
                    }
                    else
                    {
                        TR1 = Convert.ToInt16(R1 + (RInterval * (iCtr - 1)));
                    }
                    if (G1 > G2)
                    {
                        TG1 = Convert.ToInt16(G1 - (GInterval * (iCtr - 1)));
                    }
                    else
                    {
                        TG1 = Convert.ToInt16(G1 + (GInterval * (iCtr - 1)));
                    }

                    if (B1 > B2)
                    {
                        TB1 = Convert.ToInt16(B1 - (BInterval * (iCtr - 1)));
                    }
                    else
                    {
                        TB1 = Convert.ToInt16(B1 + (BInterval * (iCtr - 1)));
                    }

                }

                if ((_Legends == null))
                {
                    m_Legends[iCtr - 1].Color = Color.FromArgb(Math.Abs(TA1),Math.Abs(TR1), Math.Abs(TG1), Math.Abs(TB1));
                }
                else
                {
                    _Legends[iCtr - 1].Color = Color.FromArgb(Math.Abs(TA1), Math.Abs(TR1), Math.Abs(TG1), Math.Abs(TB1));
                }

            }
        }

        /// <summary>
        /// Set Marker size to all legends in the Theme's Legends Collection. (Applicable to Symbol theme and Point Layers)
        /// </summary>
        /// <param name="markerSize"></param>
        public void ApplyMarkerSizeOnAllLegends(int markerSize)
        {
            if (this.m_Legends != null)
            {
                foreach (Legend legend in this.m_Legends)
                {
                    legend.MarkerSize = markerSize;
                }
            }
        }

        # endregion

        # region " Legend Breaks"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ThemeDataView">May be Composite dataview or MRD dataview</param>
        /// <param name="p_Min"></param>
        /// <param name="p_Max"></param>
        internal void UpdateThemeLegend(DataView ThemeDataView, decimal p_Min, decimal p_Max)
        {
            m_Minimum = p_Min;
            m_Maximum = p_Max;

            // Preserve original Legends collection.
            Legends OrginalLegends = (Legends)(this.m_Legends.Clone());

            switch (m_BreakType)
            {
                case BreakType.EqualCount:
                case BreakType.Continuous:
                case BreakType.Discontinuous:
                    GenerateEqualCount(ThemeDataView);
                    break;
                case BreakType.EqualSize:
                    GenerateEqualSize(ThemeDataView);
                    break;
            }

            //UpdateLegendBreakCount();

            // Restore original Legend Color values and symbol settings into newly created Legends.
            for (int i = 0; i <= System.Math.Min(m_Legends.Count - 1, OrginalLegends.Count - 1); i++)
            {
                this.m_Legends[i].Caption = OrginalLegends[i].Caption;
                //-- Color
                this.m_Legends[i].Color = OrginalLegends[i].Color;

                //-- Hatch Settings
                this.m_Legends[i].FillStyle = OrginalLegends[i].FillStyle;

                //-- Symbol settings
                this.m_Legends[i].MarkerChar = OrginalLegends[i].MarkerChar;
                this.m_Legends[i].MarkerFont = OrginalLegends[i].MarkerFont;
                this.m_Legends[i].MarkerSize = OrginalLegends[i].MarkerSize;
                this.m_Legends[i].MarkerType = OrginalLegends[i].MarkerType;

                //TODO -- Label settings confirm
                this.m_Legends[i].LabelVisible = OrginalLegends[i].LabelVisible;
                this.m_Legends[i].LabelField = OrginalLegends[i].LabelField;
                this.m_Legends[i].LabelMultiRow = OrginalLegends[i].LabelMultiRow;
                this.m_Legends[i].LabelIndented = OrginalLegends[i].LabelIndented;
            }

        }

        /// <summary>
        /// 1. Set Theme dataview
        /// 2. SetDataForBreak (This utilizes theme data view)
        /// 3. Set Rendering Info for each area.
        /// </summary>
        internal void UpdateRenderingInfo(DataView p_data)
        {
            //-- Set Theme Data
            m_ThemeData = p_data;

            //-- Preserve Row Filter and Sort order
            m_ActualFilter = m_ThemeData.RowFilter;
            m_ActualSort = m_ThemeData.Sort;

            if (m_ThemeData.Count > 0)
            {
                //-- Build AreaInfo Collection (AreaId - AreaInfo (DataValue,...))
                this.m_AreaIndex.Clear();
                this.SetDataForBreak();

                //-- 
                this.UpdateLegendBreakCount();
            }

            this.SetRenderingInfo();

            //-- Restore Row Filter and Sort order
            m_ThemeData.Sort = m_ActualSort;
            m_ThemeData.RowFilter = m_ActualFilter;


        }

        //-- Bugfix 1848 2007-05-12 p_Min , p_Max set to double, otherwise large values set as 1.234556678 E+9
        public void SetRange(DataView p_data)
        {
            bool p_ChangeNow = true;
            decimal p_Min = -1;
            decimal p_Max = -1;
            SetRange(p_data, p_ChangeNow, p_Min, p_Max, null);

        }

        public void SetRange(DataView p_data, bool p_ChangeNow)
        {
            decimal p_Min = -1;
            decimal p_Max = -1;
            SetRange(p_data, p_ChangeNow, p_Min, p_Max, null);
        }

        public void SetRange(DataView p_data, bool p_ChangeNow, decimal p_Min)
        {
            decimal p_Max = -1;
            SetRange(p_data, p_ChangeNow, p_Min, p_Max, null);
        }



        internal void SetRange(DataView p_data, bool p_ChangeNow, decimal p_Min, decimal p_Max, DataView CompositeDataView)
        {            
            m_ThemeData = p_data;
            m_ActualFilter = m_ThemeData.RowFilter;
            m_ActualSort = m_ThemeData.Sort;
            m_ThemeData.Sort = DataExpressionColumns.NumericData + " ASC ";

            //-- If theme is being created for first time then 
            //-- Set default Theme Name, Theme ID and Legend Titles
            if (p_ChangeNow)
            {
                if (m_Name.Length <= 0)
                    m_Name = m_ThemeData[0][Indicator.IndicatorName].ToString() + " - " + m_ThemeData[0][Unit.UnitName].ToString() + " - " + m_ThemeData[0][SubgroupVals.SubgroupVal].ToString();

                if (m_LegendTitle.Length <= 0 && m_ThemeData.Count > 0)
                    //m_LegendTitle = m_ThemeData[0][Unit.UnitName].ToString();
                    this.SetLegendTitle(m_ThemeData[0][Indicator.IndicatorName].ToString(), m_ThemeData[0][Unit.UnitName].ToString(), m_ThemeData[0][SubgroupVals.SubgroupVal].ToString());

                if (m_ThemeData.Count > 0)
                    SetThemeId(ref p_data);
            }

            if (m_ThemeData.Count > 0)
            {
                this._IUSNid = Convert.ToInt32(m_ThemeData[0][Indicator_Unit_Subgroup.IUSNId]);

                this.SetDataForBreak();

                if (p_ChangeNow == false)
                {
                    //*** In case of TimeSeries or AreaSeries set the min and max value of theme to accomodate all years (dataview)

                    m_Minimum = p_Min;
                    m_Maximum = p_Max;

                    // Preserve original Legend color values.
                    Color[] LegendColors = new Color[m_Legends.Count - 1];
                    for (int i = 0; i < m_Legends.Count - 1; i++)
                    {
                        LegendColors[i] = this.m_Legends[i].Color;
                    }

                    switch (m_BreakType)
                    {
                        case BreakType.EqualCount:
                        case BreakType.Continuous:
                        case BreakType.Discontinuous:
                            GenerateEqualCount(CompositeDataView);
                            break;
                        case BreakType.EqualSize:
                            GenerateEqualSize(CompositeDataView);
                            break;
                    }

                    UpdateLegendBreakCount();

                    // Restore original Legend Color values into newly created Legends.
                    for (int i = 0; i < m_Legends.Count - 1; i++)
                    {
                        this.m_Legends[i].Color = LegendColors[i];
                    }
                }
                else
                {
                    switch (m_BreakType)
                    {
                        case BreakType.EqualCount:
                        case BreakType.Continuous:
                        case BreakType.Discontinuous:
                            GenerateEqualCount(m_AreaIndexDT.DefaultView);
                            break;
                        case BreakType.EqualSize:
                            GenerateEqualSize(m_AreaIndexDT.DefaultView);
                            break;
                    }
                }


            }

            switch (m_ThemeType)
            {
                case ThemeType.Chart:
                    this._IUSNid = -1;  //- Due to multiple subgroups, IUSNId is kept -1
                    break;
                case ThemeType.Color:
                    if (p_ChangeNow == true)
                        Smooth();

                    break;
                case ThemeType.DotDensity:
                    break;
                case ThemeType.Hatch:
                    int i;
                    for (i = 0; i <= m_Legends.Count - 2; i++)
                    {
                        m_Legends[i].Color = Color.White;
                    }

                    break;
            }
            SetRenderingInfo();
            m_ThemeData.Sort = m_ActualSort;
            m_ThemeData.RowFilter = m_ActualFilter;

        }

        public void UpdateLegendBreakCount()
        {
            string originalRowFilter = m_AreaIndexDT.DefaultView.RowFilter;
            int shapeCounter = 0;

            DataView _DV = m_AreaIndexDT.DefaultView;
            _DV.Sort = DataExpressionColumns.NumericData + " ASC ";
            Legend _Legend;
            for (int i = 0; i <= m_Legends.Count - 1; i++)
            {
                _Legend = m_Legends[i];
                if (i == m_Legends.Count - 1)
                {
                    m_AreaIndexDT.DefaultView.RowFilter = originalRowFilter;
                    _Legend.ShapeCount = m_ShapeCount - shapeCounter;
                    if (_Legend.ShapeCount < 0)
                        _Legend.ShapeCount = 0;
                }
                else
                {
                    _DV.RowFilter = DataExpressionColumns.NumericData + " >= " + _Legend.RangeFrom.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + _Legend.RangeTo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    _Legend.ShapeCount = _DV.Count;
                    shapeCounter += _DV.Count;
                }
            }

            m_AreaIndexDT.DefaultView.RowFilter = originalRowFilter;
            SetRenderingInfo();
        }

        internal void SetThemeId(ref DataView p_ThemeData)
        {
            m_IndicatorNID = p_ThemeData[0][Indicator.IndicatorNId].ToString();
            m_IndicatorName = p_ThemeData[0][Indicator.IndicatorName].ToString();
            m_UnitNID = int.Parse(p_ThemeData[0][Unit.UnitNId].ToString());
            m_UnitName = p_ThemeData[0][Unit.UnitName].ToString();

            //-- DO NOT assign SubgroupNId in case of Subgroup = "SelectAll" case
            if (this._SubgroupSelectAll == false)
            {
                m_SubgroupNID = p_ThemeData[0][SubgroupVals.SubgroupValNId].ToString();
                m_SubgroupName = p_ThemeData[0][SubgroupVals.SubgroupVal].ToString();
            }
            else
            {
                //-- Assign SubgroupNId = -1 in case of Subgroup = "SelectAll" case
                this.m_SubgroupNID = "-1";
                this.m_SubgroupName = string.Empty;
            }

            //$$$ Convention for Theme Id -> I_U_S_ThemeType
            m_Id = m_IndicatorNID + "_" + m_UnitNID + "_" + m_SubgroupNID + "_" + (int)m_ThemeType;
        }

        //Called only when break type is continuos
        public void SetRangeCount(DataView p_ThemeData)
        {
            m_ThemeData = p_ThemeData;
            SetDataForBreak();
            int i;
            DataView _DV = m_AreaIndexDT.DefaultView;
            _DV.Sort = DataExpressionColumns.NumericData + " ASC ";
            string _RowFilter = _DV.RowFilter;
            for (i = 0; i <= m_Legends.Count - 1; i++)
            {
                {
                    if (i == m_Legends.Count - 1)
                    {
                        m_Legends[i].ShapeCount = m_ShapeCount - _DV.Count;
                        //*** BugFix 20 Mar 2006 Negative Value for missing data. If no map is associated with map then Dv.Count > m_shape count
                        if (m_Legends[i].ShapeCount < 0)
                            m_Legends[i].ShapeCount = 0;
                    }
                    else
                    {
                        //*** BugFix 10 May 2006 Handling for French Settings
                        _DV.RowFilter = DataExpressionColumns.NumericData + " >= " + m_Legends[i].RangeFrom.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + m_Legends[i].RangeTo.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        m_Legends[i].ShapeCount = _DV.Count;
                        _DV.RowFilter = _RowFilter;
                    }
                }
            }
            SetRenderingInfo();
        }

        /// <summary>
        /// It updates  RangeFrom, RangeTo values of all legends in Collection.
        /// <para>This methods is generally used when user edited any legend's RangeTo value. </para>
        /// <para>SO client program explicitly specify editedLegendindex and value.</para>
        /// </summary>
        /// <param name="editedLegendIndex"> index position of edited Legend. </param>
        /// <param name="editedLegendRangeTo"> edited Legend's RangeTo value as desired</param>
        /// <param name="minValue">minvalue in deciaml</param>
        /// <param name="maxValue">maxValue in decimal</param>
        /// <param name="decimals">decimal places.</param>
        public void UpdateLegendRanges(int editedLegendIndex, decimal editedLegendRangeTo)
        {
            //IFormatProvider culture = System.Globalization.CultureInfo.

            int BreakCount = (this.m_Legends.Count - 1) - (editedLegendIndex + 1);

            if (this.m_Maximum >= editedLegendRangeTo && BreakCount > 0)
            {
                DataTable NewRangeDT = DevInfo.Lib.DI_LibBAL.Utility.DICommon.GenerateEqualCount(this.m_AreaIndexDT.DefaultView, BreakCount, this.m_Maximum, this.m_Decimals, editedLegendRangeTo);
                //DataTable NewRangeDT = DevInfo.Lib.DI_LibBAL.Utility.DICommon.GenerateEqualCount(this.m_AreaIndexDT.DefaultView, BreakCount - 1, newMinValue, maxValue, decimals);
                int i = editedLegendIndex + 1;
                // Loop each row and update Legends by filling RangeTo, RangeFrom, shapeCount after legend
                foreach (DataRow dr in NewRangeDT.Rows)
                {
                    // Table structure is :- Range_From | Range_To | Count

                    this.m_Legends[i].Title = Convert.ToString(dr[0], System.Globalization.CultureInfo.CurrentCulture) + " - " + Convert.ToString(dr[1], System.Globalization.CultureInfo.CurrentCulture);
                    this.m_Legends[i].RangeFrom = Convert.ToDecimal(dr[0]);
                    this.m_Legends[i].RangeTo = Convert.ToDecimal(dr[1]);

                    this.m_Legends[i].ShapeCount = (int)dr[2];
                    i += 1;
                }
            }

            //Now update All legend's shape Count.
            this.UpdateLegendBreakCount();
        }

        private void GenerateEqualCount(DataView _DV)
        {
            //IFormatProvider culture = new CultureInfo("fr-Fr", true);

            m_Legends.Clear();
            Legend legend = null;
            DataTable RangeTable = DICommon.GenerateEqualCount(_DV, this.m_BreakCount, this.m_Minimum, this.m_Maximum, this.m_Decimals);
            int i = 1;
            int TotalShapeCount = 0;
            // Loop each row and add Legends by filling RangeTo, RangeFrom, shapeCount in each legend
            foreach (DataRow dr in RangeTable.Rows)
            {
                // Table structure is :- Range_From | Range_To | Count
                legend = new Legend();

                legend.Title = Convert.ToString(dr[0], System.Globalization.CultureInfo.CurrentCulture) + " - " + Convert.ToString(dr[1], System.Globalization.CultureInfo.CurrentCulture);
                //--default Caption : Label 1, Label 2, label 3.... Label N
                legend.Caption = "Label " + i.ToString();
                legend.RangeFrom = Convert.ToDecimal(dr[0]);
                legend.RangeTo = Convert.ToDecimal(dr[1]);

                legend.ShapeCount = (int)dr[2];

                // Keep adding shapeCount.
                TotalShapeCount += (int)dr[2];

                switch (this.m_ThemeType)
                {
                    case ThemeType.Color:
                        legend.FillStyle = FillStyle.Solid;
                        break;
                    case ThemeType.Hatch:
                        legend.FillStyle = (FillStyle)i - 1;
                        break;
                }
                this.m_Legends.Add(legend);
                i += 1;
            }


            Legend _Legend = new Legend();
            {
                _Legend.Caption = "Missing Data";
                _Legend.Color = m_MissingColor;
                _Legend.ShapeCount = m_ShapeCount - TotalShapeCount;
                //*** BugFix 20 Mar 2006 Negative Value for missing data. If no map is associated with map then Dv.Count > m_shape count
                if (_Legend.ShapeCount < 0)
                    _Legend.ShapeCount = 0;

                switch (m_ThemeType)
                {
                    case ThemeType.Color:
                        _Legend.FillStyle = FillStyle.Solid;
                        break;
                    case ThemeType.Hatch:
                        _Legend.FillStyle = (FillStyle)i;
                        break;
                }
            }
            this.m_Legends.Add(_Legend);
        }

        private void GenerateEqualSize(DataView _DV)
        {
            //IFormatProvider culture = new CultureInfo("fr-Fr", true);

            m_Legends.Clear();

            Legend legend = null;
            DataTable RangeTable = DICommon.GenerateEqualSize(_DV, this.m_BreakCount, this.m_Minimum, this.m_Maximum, this.m_Decimals);
            int TotalShapeCount = 0;
            int i = 1;
            // Loop each row and add Legends by filling RangeTo, RangeFrom, shapeCount in each legend
            foreach (DataRow dr in RangeTable.Rows)
            {
                // Table structure is :- Range_From | Range_To | Count
                legend = new Legend();
                legend.Title = Convert.ToString(dr[0], System.Globalization.CultureInfo.CurrentCulture) + " - " + Convert.ToString(dr[1], System.Globalization.CultureInfo.CurrentCulture);
                //--default Caption : Label 1, Label 2, label 3.... Label N
                legend.Caption = "Label " + i.ToString();
                legend.RangeFrom = Convert.ToDecimal(dr[0]);
                legend.RangeTo = Convert.ToDecimal(dr[1]);

                legend.ShapeCount = (int)dr[2];

                // Increment Total shape count
                TotalShapeCount += (int)dr[2];
                switch (this.m_ThemeType)
                {
                    case ThemeType.Color:
                        legend.FillStyle = FillStyle.Solid;
                        break;
                    case ThemeType.Hatch:
                        legend.FillStyle = (FillStyle)i - 1;
                        break;
                }
                this.m_Legends.Add(legend);
                i += 1;
            }



            Legend _Legend = new Legend();
            {
                _Legend.Caption = "Missing Data";
                _Legend.Color = m_MissingColor;
                _Legend.ShapeCount = m_ShapeCount - TotalShapeCount;
                //*** BugFix 20 Mar 2006 Negative Value for missing data. If no map is associated with map then Dv.Count > m_shape count
                if (_Legend.ShapeCount < 0)
                    _Legend.ShapeCount = 0;
                switch (m_ThemeType)
                {
                    case ThemeType.Color:
                        _Legend.FillStyle = FillStyle.Solid;
                        break;
                    case ThemeType.Hatch:
                        _Legend.FillStyle = (FillStyle)i;
                        break;
                }
            }
            this.m_Legends.Add(_Legend);
        }

        /// <summary>
        /// Build AreaInfo Collection (AreaId - AreaInfo (DataValue,...))
        /// </summary>
        private void SetDataForBreak()
        {
            m_AreaIndexDT = new DataTable();
            m_AreaIndexDT.Locale = new System.Globalization.CultureInfo("", false);
            DataColumn[] _PK = new DataColumn[1];
            {
                {
                    m_AreaIndexDT.Columns.Add(Area.AreaID, typeof(string)).Unique = true;
                    m_AreaIndexDT.Columns.Add(DataExpressionColumns.NumericData, typeof(decimal));
                    _PK[0] = m_AreaIndexDT.Columns[Area.AreaID];

                }
                m_AreaIndexDT.PrimaryKey = _PK;
            }
            int _MaxLengthFound = 0;   // Defalut Value assigned as 0, because C# compliler was not permitting use of unassigned local variable.
            int _Length;
            string[] _Value;

            foreach (DataRowView _DRV in m_ThemeData)
            {
                try
                {
                    AreaInfo _AreaInfo = new AreaInfo();
                    if (!m_AreaIndex.ContainsKey((string)_DRV[Area.AreaID]))
                    {
                        {
                            _AreaInfo.IndicatorGID = _DRV[Indicator.IndicatorGId].ToString();
                            _AreaInfo.UnitGID = _DRV[Unit.UnitGId].ToString();
                            _AreaInfo.SubgroupGID = _DRV[SubgroupVals.SubgroupValGId].ToString();
                            _AreaInfo.Subgroup = _DRV[SubgroupVals.SubgroupVal].ToString();
                            _AreaInfo.Time = _DRV[Timeperiods.TimePeriod].ToString();
                            _AreaInfo.AreaName = _DRV[Area.AreaName].ToString();
                            _AreaInfo.Source = _DRV[IndicatorClassifications.ICName].ToString();

                            //*** Metadata
                            _AreaInfo.MDFldVal = new Hashtable();

                            for (int i = 0; i <= m_MDKeys.Length - 1; i++)
                            {
                                if (m_ThemeData.Table.Columns.Contains(m_MDKeys[i].ToString()))
                                {
                                    _AreaInfo.MDFldVal.Add(m_MDKeys[i], _DRV[(string)m_MDKeys[i]].ToString());
                                }
                            }


                            if (FirstBuild)
                            {
                                _AreaInfo.DataValue = (decimal)_DRV[DataExpressionColumns.NumericData];
                            }
                            else
                            {
                                _AreaInfo.DataValue = (decimal)_DRV[DataExpressionColumns.NumericData];
                            }
                            _AreaInfo.RenderingInfo = _AreaInfo.DataValue;

                            //DisplayInfo stores textual DataValue (e.g. yes, no) and will be used as part map label
                            _AreaInfo.DisplayInfo = _DRV[Data.DataValue].ToString();

                            m_AreaIndexDT.Rows.Add(new object[] { _DRV[Area.AreaID].ToString(), _AreaInfo.DataValue });
                        }
                        m_AreaIndex.Add(_DRV[Area.AreaID].ToString(), _AreaInfo);
                    }
                    else
                    {
                        //*** Bugfix 24 Aug 2006 Loss of data on decimal precision change
                        _AreaInfo = (AreaInfo)m_AreaIndex[_DRV[Area.AreaID]];
                        if (FirstBuild)
                        {
                            _AreaInfo.DataValue = (decimal)_DRV[DataExpressionColumns.NumericData];

                        }
                        else
                        {
                            _AreaInfo.DataValue = (decimal)_DRV[DataExpressionColumns.NumericData];
                        }
                        //-- Bugfix 1913 2007-05-19 When Area record repeats upadte RenderingInfo and AreaIndexDT value also
                        _AreaInfo.RenderingInfo = _AreaInfo.DataValue;

                        //DisplayInfo stores textual DataValue (e.g. yes, no) and will be used as part map label
                        _AreaInfo.DisplayInfo = _DRV[Data.DataValue].ToString();

                        m_AreaIndex[_DRV[Area.AreaID]] = _AreaInfo;
                        if (m_AreaIndexDT != null & m_AreaIndexDT.Rows.Count > 0)
                        {
                            m_AreaIndexDT.Rows.Find((string)_DRV[Area.AreaID])[DataExpressionColumns.NumericData] = _AreaInfo.DataValue;
                        }

                    }
                    _Value = Strings.Split(_AreaInfo.DataValue.ToString(), System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator.ToString(), -1, CompareMethod.Text);
                    if (_Value.Length > 1)
                    {
                        _Length = _Value[1].Length;
                    }
                    else
                    {
                        _Length = 0;
                    }
                    if (_Length > _MaxLengthFound)
                        _MaxLengthFound = _Length;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }


            //*** Take the filtered records and sort them on Indicator_NId and Subgroup_Nid
            switch (m_ThemeType)
            {
                case ThemeType.Chart:
                    break;

                default:
                    if (m_AreaIndexDT.Rows.Count <= 0)
                    {
                        foreach (string _Key in m_AreaIndex.Keys)
                        {
                            if (!(m_AreaIndexDT.Select(Area.AreaID + " = '" + _Key + "'").Length > 0))
                            {
                                {
                                    if (FirstBuild)
                                    {
                                        m_AreaIndexDT.Rows.Add(new object[] { _Key, ((AreaInfo)m_AreaIndex[_Key]).DataValue });
                                    }
                                    else
                                    {
                                        m_AreaIndexDT.Rows.Add(new object[] { _Key, Math.Round(((AreaInfo)m_AreaIndex[_Key]).DataValue, m_Decimals, MidpointRounding.AwayFromZero) });
                                    }
                                }

                            }
                        }
                    }

                    break;
            }

            DataView _Dv = m_AreaIndexDT.DefaultView;
            _Dv.Sort = DataExpressionColumns.NumericData + " ASC";
            if (FirstBuild)
            {
                m_Decimals = _MaxLengthFound;
                if (_Dv.Count > 0)
                {
                    m_Minimum = (decimal)_Dv[0][DataExpressionColumns.NumericData];
                    m_Maximum = (decimal)_Dv[_Dv.Count - 1][DataExpressionColumns.NumericData];
                }
                FirstBuild = false;
            }
        }
        private void SetRenderingInfo()
        {
            int i;
            decimal dValue ;
            bool _Found;
            object[] _Keys = new object[m_AreaIndex.Keys.Count];
            m_AreaIndex.Keys.CopyTo(_Keys, 0);
            foreach (string _ID in _Keys)
            {
                _Found = false;
                for (i = 0; i <= m_Legends.Count - 1; i++)
                {
                    decimal _DataValue = ((AreaInfo)m_AreaIndex[_ID]).DataValue;
                    if (m_Legends[i].RangeFrom <= Math.Round(_DataValue, m_Decimals) && m_Legends[i].RangeTo >= Math.Round(_DataValue, m_Decimals))
                    {
                        //If m_Legends.Item(i).RangeFrom <= _DataValue AndAlso m_Legends.Item(i).RangeTo >= _DataValue Then
                        AreaInfo _AreaInfo = (AreaInfo)m_AreaIndex[_ID];
                        _AreaInfo.RenderingInfo = i;

                        //- Update DisplayInfo (part of Map label) upto Decimal places
                        if (decimal.TryParse(_AreaInfo.DisplayInfo, out dValue))
                        {
                            _AreaInfo.DisplayInfo = Convert.ToString(Math.Round(dValue, m_Decimals));
                        }

                        m_AreaIndex[_ID] = _AreaInfo;
                        _Found = true;
                        break;
                    }
                }
                if (_Found == false)
                {
                    AreaInfo _AreaInfo = (AreaInfo)m_AreaIndex[_ID];
                    _AreaInfo.RenderingInfo = m_Legends.Count - 1;
                    m_AreaIndex[_ID] = _AreaInfo;
                }
            }
        }

        /// <summary>
        /// Set visibility of chartSeriesTypes to true (by default)
        /// </summary>
        public void SetChartSeriesVisibility()
        {
            string[] _SPFillStyle = null;
            string[] _SPVisible = null;
            Random _random = new Random();
            int ChartSeriesCount = 0;

            //-- Set visibility status on the basis of Group by clause (Source, Subgroups)
            if (this.ChartSeriestype == ChartSeriesType.Source)
            {
                ChartSeriesCount = this._SourceNIds.Length;
            }
            else
            {
                ChartSeriesCount = this.SubgroupNId.Length;
            }

            _SPVisible = new string[ChartSeriesCount];
            _SPFillStyle = new string[ChartSeriesCount];

            //- Set Subgroup Fill Colors
            if (this.SubgroupFillStyle.Length > 0 && string.IsNullOrEmpty(this.SubgroupFillStyle[0]) == false)
            {
                //--Preserve Old Subgroup Fill Colors
                for (int i = 0; i < _SPVisible.Length; i++)
                {
                    _SPVisible[i] = "1";
                    if (this.SubgroupFillStyle.Length > i)
                    {   
                        _SPFillStyle[i] = this.SubgroupFillStyle[i];
                    }
                    else
                    {
                        _SPFillStyle[i] = Color.FromArgb(153, (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255)).ToArgb().ToString();
                    }
                }
            }
            else
            {
                //-- Add new Subgroup Fill Colors
                for (int i = 0; i <= _SPVisible.Length - 1; i++)
                {
                    _SPVisible[i] = "1";
                    //-- Set random Color with 60% transparency .
                    _SPFillStyle[i] = Color.FromArgb(153, (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255)).ToArgb().ToString();
                }
            }

            if (this.ChartSeriestype == ChartSeriesType.Subgroup)
            {
                this.SubgroupVisible = _SPVisible;
            }
            else
            {
                this.SourceVisible = _SPVisible;
            }
            this.SubgroupFillStyle = _SPFillStyle;
        }

        /// <summary>
        /// Set ChartSeries Type visibility value for specified index. 
        /// <para>SubgroupVisible[ChartSeriesVisibilityIndex], OR SourceVisible[ChartSeriesVisibilityIndex]</para>
        /// </summary>
        public void SetChartSeriesVisibility(int ChartSeriesItemIndex, bool visible)
        {

            try
            {
                string value = string.Empty;
                if (visible)
                {
                    value = "1";
                }
                else
                {
                    value = "0";
                }

                if (ChartSeriesItemIndex >= 0)
                {
                    switch (this._ChartSeriesType)
                    {
                        case ChartSeriesType.Subgroup:
                            string[] arr = this.SubgroupVisible;
                            arr[ChartSeriesItemIndex] = value;
                            this.SubgroupVisible = arr; ;

                            break;
                        case ChartSeriesType.Source:
                            this._SourceVisible[ChartSeriesItemIndex] = value;
                            break;
                    }
                }
            }
            catch
            {

            }

        }

        public void SetLegendTitle(string indicatorName, string UnitName, string SubgroupName)
        {
            // Set Default Values for Legend Title
            switch (this.Type)
            {

                case ThemeType.Color:
                    //Case 1: Theme Type = Color ---- Title = Subgroup Value
                    if (this.MultiLegend == false)
                    {
                        this.LegendTitle = SubgroupName;
                    }
                    //Case 2: Theme Type = Color and Multiple Legend ---- Title = Indicator Value
                    else
                    {
                        this.LegendTitle = indicatorName;
                    }
                    break;
                case ThemeType.Chart:
                    //Case 3: Theme Type = Chart ---- Title = Indicator Value
                    this.LegendTitle = indicatorName;
                    break;
                case ThemeType.DotDensity:
                case ThemeType.Hatch:
                case ThemeType.Symbol:
                case ThemeType.Label:
                    //Case 4: Theme Type = Other than Color or Chart ---- Title = Subgroup Value
                    this.LegendTitle = SubgroupName;
                    break;
            }

        }

        #  endregion

        # region " Legend Image"

        public Size GetLegendImage(string p_Path, string p_FileName)
        {
            string p_FileExt = "png";
            bool ShowPointLegend = false;
            return GetLegendImage(p_Path, p_FileName, p_FileExt, ShowPointLegend, false, true, false, true);
        }

        public Size GetLegendImage(string p_Path, string p_FileName, string p_FileExt)
        {
            bool ShowPointLegend = false;
            return GetLegendImage(p_Path, p_FileName, p_FileExt, ShowPointLegend, false, true, false, true);
        }

        public Size GetLegendImage(string p_Path, string p_FileName, string p_FileExt, bool ShowPointLegend, bool showCaption, bool showRange, bool showAreaCount, bool showMissingDataInLegend)
        {
            return GetLegendImage(p_Path, p_FileName, p_FileExt, ShowPointLegend, showCaption, showRange, showAreaCount, showMissingDataInLegend, LegendsSequenceOrder.SingleColumn);
        }

        public Size GetLegendImage(string p_Path, string p_FileName, string p_FileExt, bool ShowPointLegend, bool showCaption, bool showRange, bool showAreaCount, bool showMissingDataInLegend, LegendsSequenceOrder legendSequenceOrder)
        {
            Size RetVal = new Size(0, 0);

            int ImageWidth;
            int ImageHeight = 0;
            int LegendWidth;
            int LegendHeight;
            int Padding;
            SizeF LegendSize;
            SizeF LegendTitleSize;
            int i = 0;

            this.ShowLegendRange = showRange;
            this.ShowLegendCaption = showCaption;
            this.ShowAreaCount = showAreaCount;
            
            if (m_MultiLegend == true)
            {
                LegendWidth = 20;
            }
            else
            {
                LegendWidth = 10; //40 di Profiles
            }
            LegendHeight = 5;    //20  di Profiles

            Padding = 10;
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            LegendTitleSize = g.MeasureString(m_LegendTitle, m_LegendFont);

            //*** Get maximum width of Legend Items
            ImageWidth = (int)LegendTitleSize.Width + Padding;
            

            switch (m_ThemeType)
            {
                case ThemeType.Color:
                case ThemeType.Hatch:
                case ThemeType.Symbol:
                    //-- calculate Legend Height (N Legend's Height)
                    
                    LegendHeight = (int)g.MeasureString(m_Legends[0].Title, m_LegendBodyFont).Height;
                    

                    if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                    {
                        //- Show all legends vertically in one column.
                        //- Get image height by summing all legend's height
                        ImageHeight = (m_Legends.Count - Convert.ToInt32(!(showMissingDataInLegend))) * LegendHeight + (m_MultiLegendCol.Count * LegendHeight / 2);

                        //- Add Legend title height
                        ImageHeight += (int)LegendTitleSize.Height + Padding;
                    }
                    else if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                    {
                        //- Show all legends horizontally in single row.
                        //- Get image height = Single Legend Height + LegendTitle height
                        ImageHeight = LegendHeight + (int)LegendTitleSize.Height + Padding;
                        
                    }

                    //*** one extra for legend title
                    int ColorBlockWidth;
                    if (m_MultiLegend == true)
                    {
                        ColorBlockWidth = Padding + (LegendWidth * m_MultiLegendCol.Count) + m_MultiLegendCol.Count + Padding;
                    }
                    else
                    {
                        ColorBlockWidth = Padding + LegendWidth + Padding;
                    }

                    //- Calculate Image width 
                    foreach (Legend _Legend in m_Legends)
                    {
                        i += 1;

                        //-- Legend Title displayed should be: Legend.Caption + (Legend.Title) "
                        string LegendCaption = string.Empty;
                        if (this.ShowLegendCaption)
                        {
                            LegendCaption = _Legend.Caption + " ";
                        }
                        else if (showMissingDataInLegend && i == m_Legends.Count)
                        {
                            //- Show MissingData for Last Legend if Caption is blank
                            LegendCaption = _Legend.Caption + " ";
                        }
                        

                        if (this.ShowLegendRange && (!(string.IsNullOrEmpty(_Legend.Title))))
                        {
                            LegendCaption += " " + _Legend.Title;
                        }
                        if (this.ShowAreaCount)
                        {
                            LegendCaption += " (" + Convert.ToString(_Legend.ShapeCount) + ")";
                        }


                        LegendSize = g.MeasureString(LegendCaption, m_LegendBodyFont);

                        if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                        {
                            //- if single column, then get maximum legend's width
                            ImageWidth = Math.Max(ImageWidth, ColorBlockWidth + (int)LegendSize.Width);
                        }
                        else if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                        {
 
                            if (i < m_Legends.Count || (showMissingDataInLegend && i == m_Legends.Count))
                            {
                                //- if single row, then get sum of all legend's width
                                ImageWidth += LegendWidth + Padding + (int)LegendSize.Width + Padding;
                            }
                        }
                    }

                    // In case of word wrap when single row legend exceeds map width, increase legend heigt to accomodate wraped text
                    if (legendSequenceOrder == LegendsSequenceOrder.SingleRow && this.m_LegendMaxWidth < ImageWidth)
                    {
                        ImageWidth = Convert.ToInt32(this.m_LegendMaxWidth.ToString());
                        ImageHeight = ImageHeight + ImageHeight;    //
                    }

                    break;

                case ThemeType.Label:

                    int LegendHeightSum = 0;
                    //-- Get sum of all Legends hights
                    // as in label theme, each label is set with its individual font.
                    foreach (Legend _Legend in m_Legends)
                    {
                        LegendHeightSum += (int)g.MeasureString(_Legend.Title, _Legend.MarkerFont).Height;
                    }

                    LegendHeight = LegendHeightSum / m_Legends.Count;

                    ImageHeight = LegendHeightSum + (m_MultiLegendCol.Count * LegendHeight / 2);
                    ImageHeight += (int)LegendTitleSize.Height + Padding;
 

                    //*** one extra for legend title
                    if (m_MultiLegend == true)
                    {
                        ColorBlockWidth = Padding + (LegendWidth * m_MultiLegendCol.Count) + m_MultiLegendCol.Count + Padding;
                    }
                    else
                    {
                        ColorBlockWidth = Padding + LegendWidth + Padding;
                    }

                    foreach (Legend _Legend in m_Legends)
                    {
                        i += 1;

                        //-- Legend Title displayed should be: Legend.Caption + (Legend.Title) "
                        string LegendCaption = string.Empty;
                        if (this.ShowLegendCaption)
                        {
                            LegendCaption = _Legend.Caption + " ";
                        }
                        else if (showMissingDataInLegend && i == m_Legends.Count)
                        {
                            //- Show MissingData for Last Legend if Caption is blank
                            LegendCaption = _Legend.Caption + " ";
                        }

                        if (this.ShowLegendRange && (!(string.IsNullOrEmpty(_Legend.Title))))
                        {
                            LegendCaption += " " + _Legend.Title;
                        }
                        if (this.ShowAreaCount)
                        {
                            LegendCaption += " (" + Convert.ToString(_Legend.ShapeCount) + ")";
                        }

                        LegendSize = g.MeasureString(LegendCaption, _Legend.MarkerFont);
                        ImageWidth = Math.Max(ImageWidth, ColorBlockWidth + (int)LegendSize.Width);
                    }
                    break;
                case ThemeType.DotDensity:
                    LegendSize = g.MeasureString(m_DotValue + " " + m_UnitName, m_LegendBodyFont);
                    ImageWidth = Math.Max((int)LegendTitleSize.Width + Padding, 20 + (int)LegendSize.Width);
                    ImageHeight = Math.Min(2 * (int)LegendTitleSize.Height, (int)LegendTitleSize.Height + (int)LegendSize.Height);
                    break;
                case ThemeType.Chart:
                    //get height of any one legend item based on legend font
                    LegendHeight = Math.Max(LegendHeight, (int)g.MeasureString(SubgroupName[0].ToString(), m_LegendBodyFont).Height);

                    if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                    {
                        //- Show all legends vertically in one column.
                        //ImageHeight = (SubgroupName.Length + 1) * LegendHeight;

                        // Set Image height of based on number of legend Item
                        ImageHeight = (SubgroupName.Length) * LegendHeight;

                        //- Add Legend title height to image height
                        ImageHeight += (int)(g.MeasureString(IndicatorName[0].ToString(), m_LegendBodyFont).Height)+Padding;
                    }
                    else if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                    {
                        //- Show all legends horizontally in single row.
                        ImageHeight = LegendHeight;
                    }

                    //ImageHeight += (int)LegendTitleSize.Height + Padding;
                    //*** one extra for legend title

                    foreach (string _Str in IndicatorName)
                    {
                        LegendSize = g.MeasureString(_Str, m_LegendBodyFont);
                        ImageWidth = Math.Max(ImageWidth, Padding + LegendWidth + Padding + (int)LegendSize.Width);
                    }
                                        
                    //- Calculate image width depending upon following cases
                    //- If all legends to be shown in single column.
                    // or all legends in single row.
                    if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                    {
                        //- if single column, then get maximum subgroup's width
                        foreach (string _Str in SubgroupName)
                        {
                            LegendSize = g.MeasureString(_Str, m_LegendBodyFont);

                            ImageWidth = Math.Max(ImageWidth, Padding + LegendWidth + Padding + (int)LegendSize.Width);
                        }
                    }
                    else if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                    {
                        int SubgroupCaptionWidth = 0;
                        foreach (string _Str in SubgroupName)
                        {
                            LegendSize = g.MeasureString(_Str, m_LegendBodyFont);
                            SubgroupCaptionWidth += Padding + LegendWidth + Padding + (int)LegendSize.Width;
                        }

                        ImageWidth = Math.Max(ImageWidth, SubgroupCaptionWidth);
                    }

                    //-- Now consider Legend Title width
                    ImageWidth = Math.Max((int)LegendTitleSize.Width + Padding, ImageWidth);

                    break;
            }
            bmp = new System.Drawing.Bitmap(ImageWidth + 2, ImageHeight);
            g = Graphics.FromImage(bmp);
            switch (p_FileExt.ToLower())
            {
                case "emf":
                    IntPtr hRefDC = g.GetHdc();
                    //Metafile m = new Metafile(p_Path + "\\" + p_FileName + ".emf", hRefDC);
                    Metafile m = new Metafile(p_Path + "\\" + p_FileName + ".emf", hRefDC, new Rectangle(0, 0, ImageWidth + 2, ImageHeight), MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
                    g.ReleaseHdc(hRefDC);
                    Graphics gMeta = Graphics.FromImage(m);
                    DrawLegend(ref gMeta, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    m.Dispose();
                    gMeta.Dispose();
                    break;
                case "png":
                    g.Clear(Color.White);
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                    break;
                case "jpg":
                    g.Clear(Color.White);
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                    Map.SaveAsHightQualityJPEG( bmp, p_Path + "\\" + p_FileName + ".jpg");
                    break;
                case "bmp":
                    g.Clear(Color.White);
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                    break;
                case "gif":
                    g.Clear(Color.White);
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                    break;
                case "tiff":
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                    break;
                case "ico":
                    DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, legendSequenceOrder);
                    bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                    break;
            }
           

            RetVal.Height = ImageHeight;
            RetVal.Width = ImageWidth;
            bmp.Dispose();
            bmp = null;
            g.Dispose();
            return RetVal;
        }

        private void DrawLegend(ref Graphics g, int LegendWidth, int LegendHeight)
        {
            bool ShowPointLegend = false;
            DrawLegend(ref g, LegendWidth, LegendHeight, ShowPointLegend, true, LegendsSequenceOrder.SingleColumn);
        }

        private void DrawLegend(ref Graphics g, int LegendWidth, int LegendHeight, bool ShowPointLegend, bool showMissingDataInLegend, LegendsSequenceOrder legendSequenceOrder)
        {

            //*** Legend Title
            SolidBrush BrTitle = new SolidBrush(m_LegendColor);
            SolidBrush BrLegendBody = new SolidBrush(m_LegendBodyColor);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawString(m_LegendTitle + " ", m_LegendFont, BrTitle, 10, 0);

            int LegendTitleHeight = (int)g.MeasureString(m_LegendTitle, m_LegendFont).Height;
            //*** Legend Body
            switch (m_ThemeType)
            {
                case ThemeType.Color:
                case ThemeType.Hatch:
                case ThemeType.Symbol:
                case ThemeType.Label:
                    //*** In case of multilegend draw element name (source/subgroup/metadata) at the bottom of legend
                    if (m_MultiLegend == true)
                    {
                        SolidBrush BrFooter = new SolidBrush(m_LegendColor);
                        Font FntFooter = new Font("Arial", 6);
                        IDictionaryEnumerator MultiLegendEnumerator = m_MultiLegendCol.GetEnumerator();
                        int MultiLegendIndex = 0;
                        while (MultiLegendEnumerator.MoveNext())
                        {
                            DrawMultiLegend(ref g, (Legends)MultiLegendEnumerator.Value, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, MultiLegendIndex, legendSequenceOrder);
                            //*** Draw Source/Subgroup/Metadata Text as Footer
                            g.DrawString(MultiLegendEnumerator.Key.ToString(), FntFooter, BrFooter, 10, ((((Legends)MultiLegendEnumerator.Value).Count + 1) * LegendHeight) + (MultiLegendIndex * LegendHeight / 2) + 2);
                            //Range Title
                            MultiLegendIndex += 1;
                        }
                        BrFooter.Dispose();
                        FntFooter.Dispose();
                    }
                    else
                    {
                        DrawMultiLegend(ref g, m_Legends, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, -1, legendSequenceOrder);
                    }

                    break;

                case ThemeType.DotDensity:
                    Font fnt = new Font("Webdings", m_DotSize, FontStyle.Regular);
                    SolidBrush BrDot = new SolidBrush(m_DotColor);
                    switch (m_DotStyle)
                    {
                        case MarkerStyle.Circle:
                            g.FillEllipse(BrDot, new RectangleF(10 - (m_DotSize / 2), (float)(1.5 * LegendTitleHeight) - m_DotSize, m_DotSize, m_DotSize));
                            break;
                        case MarkerStyle.Square:
                            g.FillRectangle(BrDot, new RectangleF(10 - (m_DotSize / 2), (float)(1.5 * LegendTitleHeight) - m_DotSize, m_DotSize, m_DotSize));
                            break;
                        case MarkerStyle.Triangle:
                            g.DrawString("5", fnt, BrDot, 10, LegendTitleHeight);
                            //Range Title
                            break;
                        case MarkerStyle.Cross:
                            fnt = new Font("Arial", m_DotSize, FontStyle.Regular);
                            g.DrawString("+", fnt, BrDot, 10, LegendTitleHeight);
                            //Range Title
                            break;
                        case MarkerStyle.Custom:
                            g.DrawString(m_DotChar.ToString(), m_DotFont, BrDot, 10, LegendTitleHeight);
                            //Range Title
                            break;
                    }
                    g.DrawString(m_DotValue + " " + m_UnitName, m_LegendBodyFont, BrLegendBody, 20, LegendTitleHeight);
                    //Range Title
                    fnt.Dispose();
                    BrDot.Dispose();
                    break;
                case ThemeType.Chart:
                    int k = 0;
                    string[] VisibleSeries = null;      // Either Source, Or SubgroupVal
                    string[] ChartSeriesName = null;

                    Brush BrLegend;
                    int LegendLeftPos = 0;
                    if (this._ChartSeriesType == ChartSeriesType.Source)
                    {
                        VisibleSeries = this._SourceVisible;
                        ChartSeriesName = this._SourceName;
                    }
                    else if (this._ChartSeriesType == ChartSeriesType.Subgroup)
                    {
                        VisibleSeries = this.SubgroupVisible;
                        ChartSeriesName = this.SubgroupName;
                    }
                    for (int i = 0; i <= ChartSeriesName.Length - 1; i++)
                    {
                        LegendHeight = Math.Max(LegendHeight, (int)g.MeasureString(ChartSeriesName[i].ToString(), m_LegendBodyFont).Height);

                        //*** Bugfix Feb 06 Show only those chart legend which are selected at step1
                        if (VisibleSeries[i] == "1")
                        {
                            BrLegend = new SolidBrush(Color.FromArgb(int.Parse(SubgroupFillStyle[i])));
                            if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                            {
                                //- Starting left position of Legend's Color box
                                LegendLeftPos += 10;

                                //-show color box adjacent (right) to previous Legend 
                                g.FillRectangle(BrLegend, LegendLeftPos, LegendTitleHeight, LegendWidth, LegendHeight - 2);

                                //-increment left position for cation to show
                                LegendLeftPos += LegendWidth;

                                g.DrawString(ChartSeriesName[i], m_LegendBodyFont, BrLegendBody, LegendLeftPos+5, LegendTitleHeight + 5);

                                //- For next Legend, increment left position by caption width.
                                LegendLeftPos += (int)(g.MeasureString(ChartSeriesName[i].Trim(), m_LegendBodyFont).Width)+ 5;
                            }
                            else
                            {
                                

                                g.FillRectangle(BrLegend, 10, LegendTitleHeight + (k * LegendHeight)+5, LegendWidth, LegendHeight - 2);
                                g.DrawString(ChartSeriesName[i], m_LegendBodyFont, BrLegendBody, LegendWidth + 20, LegendTitleHeight + (k * LegendHeight)+5);

                                ////////Color Box

                                ////////To Do Change //g.DrawString(ChartSeriesName[i], m_LegendBodyFont, BrLegendBody, LegendWidth + 20, LegendTitleHeight + 5 + (k * LegendHeight));
                                ////////Change 
                                //////g.DrawString(ChartSeriesName[i], m_LegendBodyFont, BrLegendBody, LegendTitleHeight + LegendHeight + 5, LegendTitleHeight + (k * LegendHeight));
                                ////////Change end

                            }
                            //Range Title
                            BrLegend.Dispose();
                            k += 1;
                        }
                    }

                    //if ((BrLegend != null))
                    //    BrLegend.Dispose();

                    break;
            }
            BrTitle.Dispose();
            BrLegendBody.Dispose();
        }

        private void DrawMultiLegend(ref Graphics g, Legends _Legends, int LegendWidth, int LegendHeight)
        {
            bool ShowPointLegend = false;
            int MultiLegendIndex = -1;
            DrawMultiLegend(ref g, _Legends, LegendWidth, LegendHeight, ShowPointLegend, true, MultiLegendIndex, LegendsSequenceOrder.SingleColumn);
        }

        private void DrawMultiLegend(ref Graphics g, Legends _Legends, int LegendWidth, int LegendHeight, bool ShowPointLegend, bool showMissingDataInLegend)
        {
            int MultiLegendIndex = -1;
            DrawMultiLegend(ref g, _Legends, LegendWidth, LegendHeight, ShowPointLegend, showMissingDataInLegend, MultiLegendIndex, LegendsSequenceOrder.SingleColumn);
        }

        private void DrawMultiLegend(ref Graphics g, Legends _Legends, int LegendWidth, int LegendHeight, bool ShowPointLegend, bool showMissingDataInLegend, int MultiLegendIndex, LegendsSequenceOrder legendSequenceOrder)
        {
            int i;
            SolidBrush BrTitle = new SolidBrush(m_LegendBodyColor);
            Legend _Legend;
            string LegendCaption = string.Empty;
            SizeF LegendTitleSize = g.MeasureString(m_LegendTitle, m_LegendFont);
            int LegendLeftPos = 0;

            float ImageRowHeight = LegendTitleSize.Height;  // image height set in single row when width exceeds then wrap in next row

            //change Increment 
            int SetOnce = 0;
            //endchange
            switch (m_ThemeType)
            {
                case ThemeType.Symbol:
                    object BrLegend;
                    StringFormat _StringFormat = new StringFormat();
                    _StringFormat.Alignment = StringAlignment.Center;
                    _StringFormat.LineAlignment = StringAlignment.Center;
                    _StringFormat.FormatFlags = StringFormatFlags.NoClip;
                    for (i = 0; i <= _Legends.Count - 1; i++)
                    {
                        bool DrawLegendRequired = true;

                        //- Skip MissingData Legend if not required
                        if (i == m_Legends.Count - 1 && showMissingDataInLegend == false)
                        {
                            DrawLegendRequired = false;
                        }

                        if (DrawLegendRequired)
                        {
                            _Legend = _Legends[i];

                            LegendCaption = string.Empty;

                            //*** Pick Legend Title from Source Legend - [Caption] Doublespace [Title]
                            if (this.ShowLegendCaption)
                            {
                                LegendCaption = _Legend.Caption + " ";
                            }
                            else if (showMissingDataInLegend && i == m_Legends.Count - 1)
                            {
                                //- Show MissingData for Last Legend if Caption is blank
                                LegendCaption = _Legend.Caption + " ";
                            }

                            if (this.ShowLegendRange && (!(string.IsNullOrEmpty(_Legend.Title))))
                            {
                                LegendCaption += _Legend.Title;
                            }
                            if (this.ShowAreaCount)
                            {
                                LegendCaption += " (" + Convert.ToString(_Legend.ShapeCount) + ")";
                            }

                            //-Draw Symbol marchar
                            if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                            {
                                _StringFormat.Alignment = StringAlignment.Near;

                                //- Show all symbols in single row (Horizontally)
                                //- Starting left position of Legend's Color box
                                LegendLeftPos += 10;

                                //-show Symbol box adjacent (right) to previous Legend 
                                if (_Legend.SymbolImage == "" || File.Exists(_Legend.SymbolImage) == false)
                                {
                                    BrLegend = new SolidBrush(_Legend.Color);
                                    g.DrawString(_Legend.MarkerChar.ToString(), _Legend.MarkerFont, (SolidBrush)BrLegend, LegendLeftPos, LegendTitleSize.Height + 5 + (_Legend.MarkerFont.Size / 3), _StringFormat);
                                    //_StringFormat

                                    LegendLeftPos += (int)(g.MeasureString(_Legend.MarkerChar.ToString(), _Legend.MarkerFont).Width);
                                }
                                else
                                {
                                    BrLegend = Image.FromFile(_Legend.SymbolImage);
                                    g.DrawImage((Image)BrLegend, LegendLeftPos, LegendTitleSize.Height + 5 + 6, Math.Min(16, ((Image)BrLegend).Width), Math.Min(13, ((Image)BrLegend).Height));

                                    LegendLeftPos += LegendWidth;
                                }
                            }
                            else
                            {
                                //-Show all Symbols in single column
                                if (_Legend.SymbolImage == "" || File.Exists(_Legend.SymbolImage) == false)
                                {
                                    BrLegend = new SolidBrush(_Legend.Color);
                                    g.DrawString(_Legend.MarkerChar.ToString(), _Legend.MarkerFont, (SolidBrush)BrLegend, 30, LegendTitleSize.Height + 5 + (_Legend.MarkerFont.Size / 2) + (i * LegendHeight), _StringFormat);
                                    //_StringFormat
                                }
                                else
                                {
                                    BrLegend = Image.FromFile(_Legend.SymbolImage);
                                    g.DrawImage((Image)BrLegend, 10, LegendTitleSize.Height + 5 + (i * LegendHeight) + 6, Math.Min(16, ((Image)BrLegend).Width), Math.Min(13, ((Image)BrLegend).Height));
                                }
                            }

                            if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                            {
                                //-Show Legend Caption or Range
                                g.DrawString(LegendCaption, m_LegendBodyFont, BrTitle, LegendWidth + 20, LegendTitleSize.Height + 5 + (i * LegendHeight));
                            }
                            else
                            {
                                //- if in single row, then draw caption next to Symbol Box
                                LegendLeftPos += 10;
                                g.DrawString(LegendCaption.Trim(), m_LegendBodyFont, BrTitle, LegendLeftPos, LegendTitleSize.Height + 5);

                                //- For next Legend, increment left position by caption width.
                                LegendLeftPos += (int)(g.MeasureString(LegendCaption.Trim(), m_LegendBodyFont).Width);
                            }

                            
                            //Range Title
                        }
                    }

                    BrLegend = new SolidBrush(Color.Black);      // BrLegend object is equalled to SolidBrush so that .Dispose() can be used. SolidBrush has no significance here.
                    if ((BrLegend != null))
                    {
                        ((SolidBrush)BrLegend).Dispose();
                    }

                    _StringFormat.Dispose();
                    break;
                //TODO Handle MultiColorTheme case
                case ThemeType.Label:
                    int PreviousLegendHeight = 0;
                    SizeF LegendSize;

                    BrLegend = new SolidBrush(Color.Black);
                    for (i = 0; i <= _Legends.Count - 1; i++)
                    {
                        _Legend = _Legends[i];

                        LegendCaption = string.Empty;

                        //*** Pick Legend Title from Source Legend - [Caption] Doublespace [Title]
                        if (this.ShowLegendCaption)
                        {
                            LegendCaption = _Legend.Caption + " ";
                        }
                        else if (showMissingDataInLegend && i == m_Legends.Count - 1)
                        {
                            //- Show MissingData for Last Legend if Caption is blank
                            LegendCaption = _Legend.Caption + " ";
                        }

                        if (this.ShowLegendRange && (!(string.IsNullOrEmpty(_Legend.Title))))
                        {
                            LegendCaption += _Legend.Title;
                        }
                        if (this.ShowAreaCount)
                        {
                            LegendCaption += " (" + Convert.ToString(_Legend.ShapeCount) + ")";
                        }


                        LegendSize = g.MeasureString(LegendCaption, _Legend.MarkerFont);
                        LegendHeight = (int)(LegendSize.Height);
                        LegendWidth = (int)(LegendSize.Width);

                        BrLegend = new SolidBrush(_Legend.Color);

                        //accomodate legend size to display large text
                        g.SetClip(new Rectangle(0, (int)LegendTitleSize.Height + PreviousLegendHeight + 5, (int)(15 + LegendWidth), LegendHeight));
                        g.DrawString(LegendCaption, _Legend.MarkerFont, (SolidBrush)BrLegend, 10, LegendTitleSize.Height + PreviousLegendHeight + 5);
                        
                        g.ResetClip();

                        PreviousLegendHeight += LegendHeight;
                    }

                    ((SolidBrush)BrLegend).Dispose();
                    break;
                //TODO Handle MultiColorTheme case
                case ThemeType.Color:
                case ThemeType.Hatch:

                    LegendLeftPos = 0;

                    for (i = 0; i <= _Legends.Count - 1; i++)
                    {
                        bool DrawLegendRequired = true;

                        //- Skip MissingData Legend if not required
                        if (i == m_Legends.Count - 1 && showMissingDataInLegend == false)
                        {
                            DrawLegendRequired = false;
                        }

                        if (DrawLegendRequired)
                        {

                            _Legend = _Legends[i];

                            LegendCaption = string.Empty;

                            //*** Pick Title from Source Legend
                            //--Start Change C1  - display - <Caption>Doublespace<Title>
                            if (this.ShowLegendCaption)
                            {
                                LegendCaption = _Legend.Caption + " ";
                            }
                            else if (showMissingDataInLegend && i == m_Legends.Count - 1)
                            {
                                //- Show MissingData for Last Legend if Caption is blank
                                LegendCaption = _Legend.Caption + " ";
                            }

                            if (this.ShowLegendRange && (!(string.IsNullOrEmpty(_Legend.Title))))
                            {
                                LegendCaption += _Legend.Title;
                            }
                            if (this.ShowAreaCount)
                            {
                                LegendCaption += " (" + Convert.ToString(_Legend.ShapeCount) + ")";
                            }

                            

                            if (_Legend.FillStyle == FillStyle.Solid)
                            {
                                BrLegend = new SolidBrush(_Legend.Color);
                            }
                            else if (_Legend.FillStyle == FillStyle.Transparent)
                            {
                                BrLegend = new SolidBrush(Color.Transparent);
                            }
                            else
                            {
                                BrLegend = new HatchBrush((HatchStyle)_Legend.FillStyle, _Legend.Color, Color.Transparent);
                            }

                            if (m_ThemeType == ThemeType.Color & ShowPointLegend == true)
                            {
                                g.FillRectangle((Brush)BrLegend, 10, LegendTitleSize.Height + (i * LegendHeight), (int)LegendWidth / 2, LegendHeight - 2);
                                //Half Color Box
                                switch (_Legend.MarkerType)
                                {
                                    case MarkerStyle.Circle:
                                        g.FillEllipse((Brush)BrLegend, 35, LegendTitleSize.Height + (i * LegendHeight) + 5, 10, 10);
                                        //Circle
                                        break;
                                    case MarkerStyle.Square:
                                        g.FillRectangle((Brush)BrLegend, 35, LegendTitleSize.Height + (i * LegendHeight) + 5, 10, 10);
                                        //Square
                                        break;
                                    case MarkerStyle.Triangle:
                                        PointF[] Vertex = new PointF[3];
                                        //Triangle
                                        Vertex[0] = new PointF(40, LegendTitleSize.Height + (i * LegendHeight) + 5);
                                        Vertex[1] = new PointF(35, LegendTitleSize.Height + (i * LegendHeight) + 15);
                                        Vertex[2] = new PointF(45, LegendTitleSize.Height + (i * LegendHeight) + 15);
                                        g.FillPolygon((Brush)BrLegend, Vertex);
                                        break;
                                    case MarkerStyle.Cross:
                                        Pen Pn = new Pen(_Legend.Color);
                                        //Cross
                                        g.DrawLine(Pn, 35, LegendTitleSize.Height + (i * LegendHeight) + 10, 45, LegendTitleSize.Height + (i * LegendHeight) + 10);
                                        g.DrawLine(Pn, 40, LegendTitleSize.Height + (i * LegendHeight) + 5, 40, LegendTitleSize.Height + (i * LegendHeight) + 15);
                                        Pn.Dispose();
                                        break;
                                }
                            }
                            else
                            {
                                if (m_MultiLegend == true)
                                {
                                    g.FillRectangle((Brush)BrLegend, 10 + (LegendWidth * MultiLegendIndex) + MultiLegendIndex, LegendTitleSize.Height + (i * LegendHeight) + 5, LegendWidth, LegendHeight - 2);
                                    //Color Box
                                }
                                else
                                {
                                    //If legend postion exceeds max Legend width then reset the left position and height to wrap text
                                    if (legendSequenceOrder == LegendsSequenceOrder.SingleRow && (m_LegendMaxWidth) <= ((g.MeasureString(LegendCaption.Trim(), m_LegendBodyFont).Width) + LegendLeftPos + LegendWidth))
                                    {
                                        LegendLeftPos = 0;
                                        LegendTitleSize.Height = LegendTitleSize.Height + ImageRowHeight;
                                    }

                                    //Color Box
                                    if (legendSequenceOrder == LegendsSequenceOrder.SingleRow)
                                    {
                                        //- Starting left position of Legend's Color box
                                        LegendLeftPos += 1; //10  di Profiles

                                        //-show color box adjacent (right) to previous Legend 
                                        g.FillRectangle((Brush)BrLegend, LegendLeftPos, LegendTitleSize.Height + 5, LegendWidth, LegendHeight - 2);
                                        
                                        LegendLeftPos += LegendWidth;
                                    }
                                    else
                                    {
                                        //-show color box below previous Legend 
                                        g.FillRectangle((Brush)BrLegend, 10, LegendTitleSize.Height + (i * LegendHeight) + 5, LegendWidth, LegendHeight - 2);
                                    }

                                   
                                }

                            }
                            if (m_MultiLegend == true)
                            {
                                if (MultiLegendIndex == m_MultiLegendCol.Count - 1)
                                {
                                    //Range Title
                                    g.DrawString(LegendCaption.Trim(), m_LegendBodyFont, BrTitle, (LegendWidth * m_MultiLegendCol.Count) + 20, LegendTitleSize.Height + 5 + (i * LegendHeight));
                                }
                            }
                            else
                            {
                                //Draw Range Title string
                                if (legendSequenceOrder == LegendsSequenceOrder.SingleColumn)
                                {
                                    g.DrawString(LegendCaption.Trim(), m_LegendBodyFont, BrTitle, LegendWidth + 20, LegendTitleSize.Height + 5 + (i * LegendHeight));
                                }
                                else
                                {
                                    //- if in single row, then draw caption next to Color Box
                                    LegendLeftPos += 1; //10  di Profiles
                                    g.DrawString(LegendCaption.Trim(), m_LegendBodyFont, BrTitle, LegendLeftPos, LegendTitleSize.Height + 5 );

                                    //- For next Legend, increment left position by caption width.
                                    LegendLeftPos += (int)(g.MeasureString(LegendCaption.Trim(), m_LegendBodyFont).Width);

                                }
                            }

                        }
                    }

                    //if ((BrLegend != null))
                    //    BrLegend = null;

                    break;
            }
            BrTitle.Dispose();
        }
        # endregion

        # endregion

    }

    /// <summary>
    /// This class defines custom settings for Label like Font, Color & Text Orientation.
    /// </summary>
    [Serializable()]
    public class CustomLabelSetting
    {
        public CustomLabelSetting()
        {
            
        }

        #region "Property"

        private Font _LabelFont = new Font("Arial", 7, FontStyle.Regular);      //- Default
        [XmlIgnore()]
        /// <summary>
        /// Gets or sets the Label's Font object.
        /// </summary>
        public Font LabelFont
        {
            get { return _LabelFont; }
            set { _LabelFont = value; }
        }

        //This below property is only used for Font property "_LabelFont" to get serialized.
        [XmlElement("LabelFont")]
        public string XmlLabelFont
        {

            //At the time of serialization, _LabelFont variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                return _LabelFont.Name + "," + _LabelFont.Size + "," + _LabelFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size & style , and restored into _LabelFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this._LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this._LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        private Color _LabelColor = Color.Black;
        [XmlIgnore()]
        /// <summary>
        /// Gets or sets the label Color.
        /// </summary>
        public Color LabelColor
        {
            get { return _LabelColor; }
            set { _LabelColor = value; }
        }

        //This property is used for _LabelColor variable to get xml serialized.
        [XmlElement("LabelColor")]
        public int XmlLabelColor
        {
            //At the time of serialization, _LabelColor variable gets serialized in form of color string name.
            get
            {
                return this._LabelColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into _LabelColor
            set
            {
                this._LabelColor = Color.FromArgb(value);
            }
        }

	

        private int _TextOrientationAngle = 0;
        /// <summary>
        /// Gets or sets the label's text orientation angle. (+90 to -90 Degree)
        /// </summary>
        public int TextOrientationAngle
        {
            get { return _TextOrientationAngle; }
            set { _TextOrientationAngle = value; }
        }

        private int _YOffset = 0;
        /// <summary>
        /// Gets or sets the Y offset value (in pixels)
        /// </summary>
        public int YOffset
        {
            get { return _YOffset; }
            set { _YOffset = value; }
        }
	
        
        #endregion

    }

    [Serializable()]
    public class LabelEffectSetting
    {
        public LabelEffectSetting()
        {

        }

        public LabelEffectSetting Clone()
        {
            LabelEffectSetting RetVal = new LabelEffectSetting();

            RetVal = new LabelEffectSetting();

            RetVal._Depth = this._Depth;
            RetVal._effect = this._effect;
            RetVal._effectColor = this._effectColor;

            return RetVal;
        }

        #region "Properties"

        private LabelEffect _effect = LabelEffect.Block;
        /// <summary>
        /// gets or sets the effect Name
        /// </summary>
        public LabelEffect Effect
        {
            get { return _effect; }
            set { _effect = value; }
        }

        private Color _effectColor = Color.Gray;
        /// <summary>
        /// Gets or sets the background effect Color
        /// </summary>
        [XmlIgnore()]
        public Color EffectColor
        {
            get { return _effectColor; }
            set { _effectColor = value; }
        }

        [XmlElement("EffectColor")]
        public int EffectColorArgb
        {
            //At the time of serialization, _effectColor variable gets serialized in form of color name.
            get
            {
                return this._effectColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into _effectColor
            set
            {
                this._effectColor = Color.FromArgb(value);
            }
        }

        private int _Depth = 1;
        /// <summary>
        /// Gets or sets depth of effect (+ve integer) 
        /// </summary>
        public int Depth
        {
            get { return _Depth; }
            set { _Depth = value; }
        }
	

        #endregion
    }

}