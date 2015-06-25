using System;
//using Devinfo.Userinterface.Queries;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;
using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;
using ICSharpCode.SharpZipLib.Zip;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;

//OPT - Remove references to Microsoft.VisualBasic if possible
//OPT - Implement logic of Image buffering
//OPT - Update the Dot Density logic for random distribution
//OPT - Dissolving internal boundries of grouped map
//OPT - Use of image to avoid unnecessary redraws

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    /// <summary>
    /// Mapping Component that can be used for Desktop as well as Web Projects
    /// </summary>
    /// <remarks>Few classes and features like Themes and Insets are Specific to DevInfo Project</remarks>
    [Serializable()]
    [XmlInclude(typeof(Themes))]
    [XmlInclude(typeof(Legends))]
    [XmlInclude(typeof(Insets))]
    [XmlInclude(typeof(CustomFeature))]
    [XmlInclude(typeof(PointF))]
    [XmlInclude(typeof(AreaInfo))]
    public class Map : IDisposable
    {

        #region "Constants"
        //OPT Derive Precise Value based on current bounds
        [XmlIgnore()]
        const float SCALE_FACTOR = 0.0045F;

        /// <summary>
        /// Default start date "1/1/1800" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
        /// </summary>
        public const string DEFAULT_START_DATE = "1/1/1800";

        /// <summary>
        /// Default end date "12/31/3000" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
        /// </summary>
        public const string DEFAULT_END_DATE = "12/31/3000"; //12/31/2200 'thai era exceeds 2200 years so extended to 3000

        #endregion

        #region "Variables "

        // Thread worker to get AreaNames in each Layers (Layer.AreaNames.Add(AreaID, AreaName))
        [NonSerialized()]
        private Thread FillAreaWorker;

        private Random _random = new Random();  //To get random number varing from int to float 

        private System.Drawing.Color m_CanvasColor = Color.Transparent;
        private float m_Width;
        private float m_Height;
        private System.Drawing.Font m_Font;
        //Readonly
        private RectangleF m_FullExtent;
        private RectangleF m_CurrentExtent;

        private string m_Title = "";
        //private Font m_TitleFont = new Font("Microsoft Sans Serif", 14);
        private string m_TitleColor = Color.Black.Name;
        private string m_Subtitle = "";
        //private Font m_SubtitleFont = new Font("Microsoft Sans Serif", 12);
        private string m_SubtitleColor = Color.Black.Name;
        private string m_Disclaimer = "Note: The boundaries and the names shown and the designations used on these maps " + "do not imply official endorsement or acceptance by the United Nations.";
        //private Font m_DisclaimerFont = new Font("Microsoft Sans Serif", 8.25F);
        private LayoutType m_LayoutType;
        private string m_WaterMarkText = "DevInfo5";
        // Keep clearing images from this folder Default value=stock\users\user_id\map\images
        private string m_ImagePath = "";
        //Custom Feature Layer Folder
        private string m_CFLPath = "";
        private bool m_Footnote;
        private bool m_Scale;
        private string m_ScaleUnitText = "KM";

        private string m_FirstColor = "#FDF5E6";
        private string m_SecondColor = "#FED7AC";
        private string m_ThirdColor = "#FED7AC";
        private string m_FourthColor = "#FF9B37";

        private Themes m_Themes = new Themes();
        private Layers m_Layers = new Layers();
        //*** Custom Feature Layer Collection
        //private System.Collections.ArrayList m_CFLCol = new ArrayList();

        private System.Collections.Generic.List<CustomFeature> m_CFLCol = new System.Collections.Generic.List<CustomFeature>();

        private Insets m_Insets = new Insets();


        //System.Drawing.Drawing2D.Matrix is not serializable
        //The elements m11, m12, m21, m22, dx, dy of the Matrix object are represented by the values in the array in that order.
        private float[] m_TransMatrix = new float[6];
        [NonSerialized()]
        //private QueryBase m_QueryBase;
        private static string m_MapFolder = "";
        private bool m_PointShapeIncluded;
        private string m_SpatialMapFolder;

        //*** North Symbol
        //OPT: Map-Make a genric class for symbol which may be later used to draw symbols like north symbol font based symbol, icon/bmp based symbols
        private bool m_NorthSymbol;
        private PointF m_NorthSymbolPosition;
        private int m_NorthSymbolSize = 40;
        private Color m_NorthSymbolColor = Color.Black;

        //*** Selection
        private Color m_SelectionColor = Color.FromArgb(200, 255, 255, 0);

        // -- MAPPING
        private string m_MissingValue = "";

        //-- AreaID to highlight with a different border in DrawMap()
        private string AreaIDToHighlight = string.Empty;
        private Layer PreviousLayerHighlighted = null;


        /// <summary>
        /// Load the Connection object in the constructor
        /// </summary>
        /// <remarks>TODO Made public only for web later when xml serilaization is achieved in web make it private</remarks>
        [XmlIgnore()]
        [NonSerialized()]
        public DIConnection DIConnection = null;

        /// <summary>
        /// Load the DIQueries object in the constructor
        /// </summary>
        /// <remarks>TODO Made public only for web later when xml serilaization is achieved in web make it private</remarks>
        [XmlIgnore()]
        [NonSerialized()]
        public DIQueries DIQueries = null;
        private StringBuilder sbLayerNIDs;      // StringBuilder is used for data passing to seperate thread (thread - FillAreaWorker_DoWork) 
        private StringBuilder sbLayerNames;     // '' same ---

        #endregion

        #region "Properties "

        [NonSerialized()]
        private DIDataView _DIDataView;
        [XmlIgnore()]
        public DIDataView DIDataView
        {
            get
            {
                return this._DIDataView;
            }
            set
            {
                this._DIDataView = value;

                //-- If DIDataView is set, 
                //-- this.PresentationData also needs to be set
                if (this._DIDataView != null)
                {
                    this._PresentationData = this._DIDataView.GetAllDataByUserSelection();

                }
            }
        }

        [NonSerialized()]
        private DataView _PresentationData;
        private DataView PresentationData
        {
            get
            {
                if (this._PresentationData == null)
                {
                    this._PresentationData = this._DIDataView.GetPresentationData();

                }
                return _PresentationData;
            }
        }

        [NonSerialized()]
        private DataView _MRDData;
        private DataView MRDData
        {
            get
            {
                if (_MRDData == null)
                {
                    _MRDData = this._DIDataView.GetMostRecentData(false);
                }
                return _MRDData;
            }
        }

        public string MissingValue
        {
            get { return m_MissingValue; }
            set { m_MissingValue = value; }
        }

        private StyleTemplate _TemplateStyle = new StyleTemplate(Presentation.PresentationType.Map, false);
        /// <summary>
        /// Gets or sets the style template (FontName, FontSize, color) covering Map Title, Subtitle, Label, Disclaimer, LegendTitle, LegendBody.
        /// </summary>
        public StyleTemplate TemplateStyle
        {
            get { return _TemplateStyle; }
            set { _TemplateStyle = value; }
        }

        private string _TemplateStyleName = string.Empty;
        /// <summary>
        /// Gets or sets the Name of styleTemplate object used in Map.
        /// </summary>
        public string TemplateStyleName
        {
            get { return _TemplateStyleName; }
            set { _TemplateStyleName = value; }
        }


        public string TitleColor
        {
            get { return m_TitleColor; }
            set { m_TitleColor = value; }
        }

        public string SubtitleColor
        {
            get { return m_SubtitleColor; }
            set { m_SubtitleColor = value; }
        }

        ////[XmlIgnore()]
        ////public Font TitleFont
        ////{
        ////    get { return m_TitleFont; }
        ////    set { m_TitleFont = value; }
        ////}

        ////This below property is only used for Font property "TitleFont" to get serialized.
        //[XmlElement("TitleFont")]
        //public string XmlTitleFont
        //{
        //    //At the time of serialization, m_TitleFont variable gets serialized into string (Font Name + Font Size + FontStyle)
        //    get
        //    {
        //        return m_TitleFont.Name + "," + m_TitleFont.Size.ToString() + "," + m_TitleFont.Style.ToString();
        //    }
        //    //At time of Deserialzation, value is split into FontName & FontSize & FontStyle and restored into m_TitleFont
        //    set
        //    {
        //        string[] _FontSettings = new string[3];
        //        _FontSettings = value.Split(",".ToCharArray());
        //        if (_FontSettings.Length == 3 & _FontSettings[2] != "")
        //        {
        //            this.m_TitleFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)GetFontStyleInteger(_FontSettings[2]));
        //        }
        //        else
        //        {
        //            this.m_TitleFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
        //        }
        //    }
        //}

        ////[XmlIgnore()]
        ////public Font SubtitleFont
        ////{
        ////    get { return m_SubtitleFont; }
        ////    set { m_SubtitleFont = value; }
        ////}

        //This below property is only used for Font property "SubtitleFont" to get serialized.
        ////[XmlElement("SubtitleFont")]
        ////public string XmlSubtitleFont
        ////{
        ////    //At the time of serialization, m_SubtitleFont variable gets serialized into string (Font Name + Font Size + Font Style)
        ////    get
        ////    {
        ////        return m_SubtitleFont.Name + "," + m_SubtitleFont.Size.ToString() + "," + m_SubtitleFont.Style.ToString();
        ////    }
        ////    //At time of Deserialzation, value is split into Font Name & Font Size and restored into m_SubtitleFont
        ////    set
        ////    {
        ////        string[] _FontSettings = new string[3];
        ////        _FontSettings = value.Split(",".ToCharArray());
        ////        if (_FontSettings.Length == 3 & _FontSettings[2] != "")
        ////        {
        ////            this.m_SubtitleFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)GetFontStyleInteger(_FontSettings[2]));
        ////        }
        ////        else
        ////        {
        ////            this.m_SubtitleFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
        ////        }
        ////    }
        ////}

        private bool _DisplayMapTitle;
        /// <summary>
        /// Gets or sets bool value indicating whether Map title to be displayed on Map drawn.
        /// </summary>
        public bool DisplayMapTitle
        {
            get { return _DisplayMapTitle; }
            set { _DisplayMapTitle = value; }
        }


        ////[XmlIgnore()]
        ////public Font DisclaimerFont
        ////{
        ////    get { return m_DisclaimerFont; }
        ////    set { m_DisclaimerFont = value; }
        ////}

        ////This below property is only used for Font property "DisclaimerFont" to get serialized.
        //[XmlElement("DisclaimerFont")]
        //public string XmlDisclaimerFont
        //{
        //    //At the time of serialization, m_DisclaimerFont variable gets serialized into string (Font Name + Font Size)
        //    get
        //    {
        //        return m_DisclaimerFont.Name + "," + m_DisclaimerFont.Size.ToString() + "," + m_DisclaimerFont.Style.ToString();
        //    }
        //    //At time of Deserialzation, value is split into Font Name & Font Size and restored into m_DisclaimerFont
        //    set
        //    {
        //        string[] _FontSettings = new string[3];
        //        _FontSettings = value.Split(",".ToCharArray());
        //        if (_FontSettings.Length == 3 & _FontSettings[2] != "")
        //        {
        //            this.m_DisclaimerFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)GetFontStyleInteger(_FontSettings[2]));
        //        }
        //        else
        //        {
        //            this.m_DisclaimerFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
        //        }
        //    }
        //}

        public bool Footnote
        {
            get { return m_Footnote; }
            set { m_Footnote = value; }
        }

        public bool Scale
        {
            get { return m_Scale; }
            set { m_Scale = value; }
        }

        public string ScaleUnitText
        {
            get { return m_ScaleUnitText; }
            set { m_ScaleUnitText = value; }
        }

        [XmlIgnore()]
        public Color FirstColor
        {
            get { return ColorTranslator.FromHtml(m_FirstColor); }
            set { m_FirstColor = ColorTranslator.ToHtml(value); }
        }


        public string FirstColorString
        {
            get { return m_FirstColor; }
            set { m_FirstColor = value; }
        }

        [XmlIgnore()]
        public Color SecondColor
        {
            get { return ColorTranslator.FromHtml(m_SecondColor); }
            set { m_SecondColor = ColorTranslator.ToHtml(value); }
        }

        public string SecondColorString
        {
            get { return m_SecondColor; }
            set { m_SecondColor = value; }
        }

        [XmlIgnore()]
        public Color ThirdColor
        {
            get { return ColorTranslator.FromHtml(m_ThirdColor); }
            set { m_ThirdColor = ColorTranslator.ToHtml(value); }
        }

        public string ThirdColorString
        {
            get { return m_ThirdColor; }
            set { m_ThirdColor = value; }
        }

        [XmlIgnore()]
        public Color FourthColor
        {
            get { return ColorTranslator.FromHtml(m_FourthColor); }
            set { m_FourthColor = ColorTranslator.ToHtml(value); }
        }

        public string FourthColorString
        {
            get { return m_FourthColor; }
            set { m_FourthColor = value; }
        }

        private Color _MissingColor = Color.Gray;
        [XmlIgnore()]
        public Color MissingColor
        {
            get { return _MissingColor; }
            set
            {
                _MissingColor = value;

                try
                {
                    foreach (Theme th in this.m_Themes)
                    {
                        th.MissingColor = this._MissingColor;
                    }
                }
                catch
                { }
            }
        }

        public int MissingColorArgbValue
        {
            //At the time of serialization, m_MissingColor variable gets serialized in form of color string name.
            get
            {
                return this._MissingColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_MissingColor
            set
            {
                this._MissingColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Color SelectionColor
        {
            get { return m_SelectionColor; }
            set { m_SelectionColor = value; }
        }

        //This property is used for SelectionColor variable to get xml serialized.
        [XmlElement("SelectionColor")]
        public int XmlSelectionColor
        {
            //At the time of serialization, m_SelectionColor variable gets serialized after color transaltion.
            get
            {
                return this.m_SelectionColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_SelectionColor
            set
            {
                this.m_SelectionColor = Color.FromArgb(value);
            }
        }

        public float[] TransMatrix
        {
            get { return m_TransMatrix; }

            //Set is only used for Desrialization of property, otherwise it is treated as Readonly in Hosting application
            set { m_TransMatrix = (float[])value; }
        }

        public bool NorthSymbol
        {
            get { return m_NorthSymbol; }
            set { m_NorthSymbol = value; }
        }

        public PointF NorthSymbolPosition
        {
            get { return m_NorthSymbolPosition; }
            set { m_NorthSymbolPosition = value; }
        }

        public int NorthSymbolSize
        {
            get { return m_NorthSymbolSize; }
            set { m_NorthSymbolSize = value; }
        }

        [XmlIgnore()]
        public Color NorthSymbolColor
        {
            get { return m_NorthSymbolColor; }
            set { m_NorthSymbolColor = value; }
        }
        //This property is used for NorthSymbolColor variable to get xml serialized.
        [XmlElement("NorthSymbolColor")]
        public int XmlNorthSymbolColor
        {
            //At the time of serialization, m_NorthSymbolColor variable gets serialized after color transaltion.
            get
            {
                return this.m_NorthSymbolColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_NorthSymbolColor
            set
            {
                this.m_NorthSymbolColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]

        //Used only by web ??
        public DataView GetData
        {

            get
            {
                //TODO Check that PresentationData contains all fields
                return this.PresentationData;
                //return m_QueryBase.AvailableDataView_Fast(true); 
            }
        }

        public string SpatialMapFolder
        {
            get { return m_SpatialMapFolder; }
            set { m_SpatialMapFolder = value; }
        }

        private string _DIBFolderPath = string.Empty;
        /// <summary>
        /// Folder path of disputed International Boundaries.
        /// </summary>
        public string DIBFolderPath
        {
            get { return _DIBFolderPath; }
            set { _DIBFolderPath = value; }
        }

        private bool _ShowDIBLayers = false;
        /// <summary>
        /// Gets or sets value to make Disputed Boundaries Visible/ Invisible.
        /// </summary>
        public bool ShowDIBLayers
        {
            get
            {
                return _ShowDIBLayers;
            }
            set
            {
                _ShowDIBLayers = value;

                //- Set visibility of DIB layers
                try
                {
                    if (_ShowDIBLayers)
                    {
                        if (this.ContainsDIBLayers() == false)
                        {
                            // Add DIB layers 
                            this.AddDisputedInternationalBoundaries(this._DIBFolderPath);
                        }
                    }

                    foreach (Layer Lyr in this.m_Layers)
                    {
                        if (Lyr.IsDIB)
                        {
                            Lyr.Visible = this._ShowDIBLayers;
                        }
                    }
                }
                catch
                {
                }
            }
        }


        //Property to store path for Custom Feature Layer.
        public string CFLPath
        {
            get
            {
                return m_CFLPath;
            }
            set
            {
                m_CFLPath = value;
            }
        }

        //Property to store path for Images extracted.
        public string ImagePath
        {
            get
            {
                return m_ImagePath;
            }
            set
            {
                m_ImagePath = value;
            }
        }

        public bool PointShapeIncluded
        {
            get { return m_PointShapeIncluded; }
            set
            {
                //Set is only used for Desrialization of property, otherwise it is treated as Readonly in Hosting application
                m_PointShapeIncluded = value;
            }
        }

        public static string MapFolder
        {
            get { return m_MapFolder; }
            set { m_MapFolder = value; }
        }

        [XmlIgnore()]
        public Color CanvasColor
        {
            get { return m_CanvasColor; }
            set { m_CanvasColor = value; }
        }

        //This property is used for CanvasColor property to get xml serialized.
        [XmlElement("CanvasColor")]
        public int XmlCanvasColor
        {
            //At the time of serialization, m_CanvasColor variable gets serialized after color transaltion.
            get
            {
                return this.m_CanvasColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into m_BorderColor
            set
            {
                this.m_CanvasColor = Color.FromArgb(value);
            }
        }

        public float Width
        {
            get { return (m_Width); }
            set { m_Width = (float)value; }
        }

        public float Height
        {
            get { return m_Height; }
            set { m_Height = (float)value; }
        }

        [XmlIgnore()]
        public Font MapFont
        {
            get { return m_Font; }
            set { m_Font = value; }
        }

        //This below property is only used for Font property "MapFont" to get serialized.
        [XmlElement("MapFont")]
        public string XmlMapFont
        {
            //At the time of serialization, m_Font variable gets serialized into string (Font Name + Font Size + Font Style)
            get
            {
                if (m_Font == null)   //Checking if m_Font is nothing, then return null
                {
                    return "";
                }
                else
                {
                    return m_Font.Name + "," + m_Font.Size + "," + m_Font.Style.ToString();
                }
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into m_Font
            set
            {
                string[] _FontSettings = new string[3];
                if (value == "")
                {
                    this.m_Font = null;
                }
                else
                {
                    _FontSettings = value.Split(",".ToCharArray());
                    if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                    {
                        this.m_Font = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)GetFontStyleInteger(_FontSettings[2]));
                    }
                    else
                    {
                        this.m_Font = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                    }
                }
            }
        }

        public RectangleF FullExtent
        {
            get { return m_FullExtent; }
            set { m_FullExtent = value; }
        }

        public RectangleF CurrentExtent
        {
            get { return m_CurrentExtent; }
            set { m_CurrentExtent = value; }
        }

        public string Disclaimer
        {
            get { return m_Disclaimer; }
            set { m_Disclaimer = value; }
        }

        public Layers Layers
        {
            get { return m_Layers; }
            set { m_Layers = value; }
        }

        //[XmlIgnore()]
        public System.Collections.Generic.List<CustomFeature> CFL
        {
            get { return m_CFLCol; }
            set { m_CFLCol = value; }
        }

        public Themes Themes
        {
            get { return m_Themes; }
            //Set is only used for Desrialization of property, otherwise it is treated as Readonly in Hosting application                    
            set
            {
                m_Themes = value;
            }
        }

        public Insets Insets
        {
            get { return m_Insets; }
            //Set is only used for Desrialization of property, otherwise it is treated as Readonly in Hosting application                    
            set
            {
                m_Insets = value;
            }
        }

        public string Title
        {
            get { return m_Title; }
            set { m_Title = value; }
        }

        public string Subtitle
        {
            get { return m_Subtitle; }
            set { m_Subtitle = value; }
        }

        public LayoutType LayoutType
        {
            get { return m_LayoutType; }
            set { m_LayoutType = value; }
        }

        //[NonSerialized()]
        //private DevInfo.Lib.DI_LibDAL.UserSelection.UserSelection _UserSelection;
        ///// <summary>
        ///// Gets or sets the user selection.
        ///// </summary>
        ///// <remarks>This property will only be used at the time of Serialization & Deserialization of Map object.</remarks>

        //public DevInfo.Lib.DI_LibDAL.UserSelection.UserSelection UserSelection
        //{
        //    get
        //    {
        //        return this._UserSelection;
        //    }
        //    set
        //    {
        //        this._UserSelection = value;
        //    }
        //}

        private UserPreference.UserPreference _UserPreference;
        /// <summary>
        /// Gets or sets DevInfo.Lib.DI_LibBAL.UI.UserPreference.UserPrefrence object
        /// </summary>
        public UserPreference.UserPreference UserPreference
        {
            get { return _UserPreference; }
            set { _UserPreference = value; }
        }

        private List<string> _DrillDownAreaNIds = new List<string>();
        /// <summary>
        /// Gets or sets the NIds of Drill Down Areas.
        /// </summary>
        public List<string> DrillDownAreaNIds
        {
            get { return _DrillDownAreaNIds; }
            set { _DrillDownAreaNIds = value; }
        }

        private List<string> _DrillDownAreaIDs = new List<string>();
        /// <summary>
        /// Gets or sets the IDs of Drill Down Areas.
        /// </summary>
        public List<string> DrillDownAreaIDs
        {
            get { return _DrillDownAreaIDs; }
            set { _DrillDownAreaIDs = value; }
        }

        private bool _showTimePeriods;

        /// <summary>
        /// Gets or sets the bool value "showTimePeriods" indicating wheather Time Periods to be shown or not.
        /// </summary>
        public bool ShowTimePeriods
        {
            get { return this._showTimePeriods; }
            set { this._showTimePeriods = value; }
        }

        private bool _showAreaLevels;

        /// <summary>
        /// Gets or sets the bool value "showAreaLevels" indicating wheather AreaLevels to be shown or not.
        /// </summary>
        public bool ShowAreaLevels
        {
            get { return this._showAreaLevels; }
            set { this._showAreaLevels = value; }
        }

        private bool _ShowLabelWhereDataExists = false;
        /// <summary>
        /// (Default False). Gets or sets value whether to show Label only for Areas having DataValue.
        /// </summary>
        public bool ShowLabelWhereDataExists
        {
            get { return _ShowLabelWhereDataExists; }
            set { _ShowLabelWhereDataExists = value; }
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

        private string _SelectedThemeID = string.Empty;
        /// <summary>
        /// Gets or sets selected Theme ID. (User may set ThemeID explicitly if a perticular theme's label setting is edited in UI)
        /// </summary>
        public string SelectedThemeID
        {
            get
            {
                bool ThemePresent = false;

                //-- Check if ThemeID is present in Collection.
                foreach (Theme _Theme in this.m_Themes)
                {
                    if (string.Compare(_Theme.ID, _SelectedThemeID, true) == 0)
                    {
                        ThemePresent = true;
                        break;
                    }
                }

                //-- If Theme is NOT present, the clear ThemeID
                if (ThemePresent == false)
                {
                    this._SelectedThemeID = string.Empty;
                }

                return _SelectedThemeID;
            }
            set { _SelectedThemeID = value; }
        }

        private string[] _MDColumns;
        /// <summary>
        /// This property is kept private and will be used internally to fill MDfields in Theme.AreaIndexes, Theme.MetaDataKeys
        /// </summary>
        private string[] MDColumns
        {
            get
            {
                if (this._MDColumns == null)
                {
                    string MDColumnsTemp = ",";

                    if (this._MDIndicatorFields.Length > 0)
                    {
                        MDColumnsTemp += this._MDIndicatorFields;
                    }

                    if (this._MDAreaFields.Length > 0)
                    {
                        MDColumnsTemp += "," + this._MDAreaFields;
                    }
                    if (this._MDSourceFields.Length > 0)
                    {
                        MDColumnsTemp += "," + this._MDSourceFields;
                    }

                    // SET MDColumns in variable
                    _MDColumns = MDColumnsTemp.Substring(1).Split(',');
                }
                return _MDColumns;
            }
            set
            {
                _MDColumns = value;
            }
        }


        private string _MDIndicatorFields = string.Empty; //Comma delimited fields for indicator metadata "MD_IND_1, MD_IND_3.."
        /// <summary>
        /// Gets MetaData Indicators Fields presents in UserPreferences
        /// </summary>
        public string MDIndicatorFields // readonly
        {
            get
            {
                return this._MDIndicatorFields;
            }

        }

        private string _MDAreaFields = string.Empty;
        /// <summary>
        /// Gets MetaData Areas Fields presents in UserPreferences
        /// </summary>
        public string MDAreaFields      // readonly
        {
            get
            {
                return this._MDAreaFields;
            }
            set
            {
                this._MDAreaFields = value;
            }
        }

        private string _MDSourceFields = string.Empty;
        /// <summary>
        /// Gets MetaData Source Fields presents in UserPreferences
        /// </summary>
        public string MDSourceFields // readonly
        {
            get
            {
                return this._MDSourceFields;
            }
            set
            {
                this._MDSourceFields = value;
            }
        }

        private string _VisibleColorThemeIDWhenSerialized = string.Empty;
        /// <summary>
        /// Gets or sets the ID of visible Color theme from Collection at the time of Themes serialization.
        /// This property will be used to reset Color theme visibilty after Deserialization.
        /// </summary>
        public string VisibleColorThemeIDWhenSerialized
        {
            get
            {
                if (string.IsNullOrEmpty(this._VisibleColorThemeIDWhenSerialized))
                {
                    foreach (Theme _Theme in this.m_Themes)
                    {
                        if (_Theme.Type == ThemeType.Color && _Theme.Visible)
                        {
                            this._VisibleColorThemeIDWhenSerialized = _Theme.ID;
                        }
                    }
                }

                return _VisibleColorThemeIDWhenSerialized;
            }
            set { _VisibleColorThemeIDWhenSerialized = value; }
        }

        private string _BackgroundMapAreaID = string.Empty;
        /// <summary>
        /// Gets or sets the AreaId of AreaLayer to be shown in current Map background.
        /// </summary>
        public string BackgroundMapAreaID
        {
            get { return _BackgroundMapAreaID; }
            set
            {
                this._BackgroundMapAreaID = value;
            }
        }

        #region " -- Print -- "

        private string _PageSize = "A4";
        /// <summary>
        /// Gets or sets the selected page size for Printing.
        /// </summary>
        public string PageSize
        {
            get
            {
                return this._PageSize;
            }
            set
            {
                this._PageSize = value;
            }
        }

        private Orientation _Orientation = Orientation.Horizontal;
        /// <summary>
        /// Gets or sets the orientation of document to print.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this._Orientation;
            }
            set
            {
                this._Orientation = value;
            }
        }

        private bool _FitToPage = false;
        /// <summary>
        /// Gets or sets the value indicating document to fit to page.
        /// </summary>
        public bool FitToPage
        {
            get
            {
                return this._FitToPage;
            }
            set
            {
                this._FitToPage = value;
            }
        }

        #endregion

        #endregion

        #region "Methods"

        #region " Initialization"


        /// <summary>
        ///  Initializes the Map object and setting its properties with the parameters passed. DIConnection + DIQueries (for Database operations)
        /// </summary>
        /// <param name="presentationData">DataView containing Most recent Data</param>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <param name="UserSelection">UserSelection</param>
        public Map(DIDataView DIDataView, DIConnection DIConnection, DIQueries DIQueries, UserPreference.UserPreference userPreference)
        {

            this._DIDataView = DIDataView;

            //Sets the private variable to connection  object.
            this.DIConnection = DIConnection;

            //Sets the private variable to DiQueries object.
            this.DIQueries = DIQueries;

            //Set the userPreference & userSelection.
            this._UserPreference = userPreference;      // Property is used when deserializing Map object and again re-creating DIDataView.

            //Set Presentation data
            this._PresentationData = DIDataView.GetPresentationData();

            // sets MetaData columns in its properties. (this._MDindicators, this._MDArea, this._MDSource are used in GeneratePresentation() where columns are needed to be displayed in excel sheet)
            this.SetMetadataInfo();

        }

        public void Initialize(string p_ImageFolder, string p_SpatialMapFolder)
        {
            string p_CFLFolder = "";
            Initialize(p_ImageFolder, p_SpatialMapFolder, p_CFLFolder, false);
        }

        /// <summary>
        /// It initializes Map by adding layers in it and creating Theme on the basis of current userSelections & PresentationDataView.
        /// </summary>
        public void Initialize(string p_ImageFolder, string spatialMapFolder, string CFLFolder)
        {
            //-- Pass false in preserveLegends as default.
            this.Initialize(p_ImageFolder, spatialMapFolder, CFLFolder, false);
        }

        /// <summary>
        /// It initializes Map by adding layers in it and creating Theme on the basis of current userSelections & PresentationDataView.
        /// </summary>
        /// <param name="preserveLegendsRanges">true, if Map's existing Legend's settings are required to preserve in new Legends when Map will initialize again.</param>
        public void Initialize(string p_ImageFolder, string spatialMapFolder, string CFLFolder, bool preserveLegendsRanges)
        {
            this.m_ImagePath = p_ImageFolder.Replace("\\", @"\");
            this.m_SpatialMapFolder = spatialMapFolder;
            this.m_CFLPath = CFLFolder;
            string ExistingPolygonLayers = string.Empty;
            DataView dvAreaMapLayerInfo;

            DateTime dtStartDate;
            DateTime dtEndDate;
            DateTime dtTempStartDate = DateTime.Parse(DEFAULT_START_DATE).Date;
            DateTime dtTempEndDate = DateTime.Parse(DEFAULT_START_DATE).Date;
            CultureInfo ociEnUS = new CultureInfo("en-US", false);
            this.sbLayerNIDs = new StringBuilder();
            this.sbLayerNames = new StringBuilder();

            //-- Get the date range for which shapefiles are to be considered
            //-- handle different Reginal Settings - Thai, Arabic etc
            dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
            dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;

            //-- CHECK if PresentationData has any record in it. 
            //-- If NO Record found, then clear Themes and Layers.
            if (this.PresentationData != null && this.PresentationData.Count > 0)
            {
                string sSql = string.Empty;
                if (this.UserPreference.UserSelection.TimePeriodNIds.Length > 0)
                {
                    // Get TimePeriod from Userselection.
                    sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, this.UserPreference.UserSelection.TimePeriodNIds);
                }
                else
                {
                    // Get TimePeriod from PresentationData.
                    DataView dvTimeperiods = this.GetTimePeriods();
                    string TimeNids = string.Empty;
                    for (int k = 0; k < dvTimeperiods.Count; k++)
                    {
                        if (TimeNids.Length > 0)
                        {
                            TimeNids += ",";
                        }
                        TimeNids += dvTimeperiods[k][Timeperiods.TimePeriodNId].ToString();
                    }
                    sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, TimeNids);
                }

                //string sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, "");
                System.Data.IDataReader dr = DIConnection.ExecuteReader(sSql);

                int i = 0;
                while (dr.Read())
                {
                    dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
                    dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;

                    TimePeriodFacade.SetStartDateEndDate(dr[Timeperiods.TimePeriod].ToString(), ref dtTempStartDate, ref dtTempEndDate);

                    if (i == 0) //For first time set dtStartDate dtEndDate
                    {
                        dtStartDate = dtTempStartDate;
                        dtEndDate = dtTempEndDate;
                    }
                    else
                    {
                        if (System.DateTime.Compare(dtTempStartDate, dtStartDate) < 0)
                            dtStartDate = dtTempStartDate;

                        if (System.DateTime.Compare(dtTempEndDate, dtEndDate) > 0)
                            dtEndDate = dtTempEndDate;
                    }
                    i++;
                }
                dr.Close();
                dr.Dispose();


                //*** Add Layer to Layers collection
                //-- get Areas present in DataView.
                string sAreaNIds = GetAreaNIds();

                //-- Get & Merge Areas from DrillDown Collection.
                if (this._DrillDownAreaNIds.Count > 0)
                {
                    sAreaNIds += "," + string.Join(",", this._DrillDownAreaNIds.ToArray());
                }

                List<string> AreaNIds = new List<string>(sAreaNIds.Split(','));

                /// Very Important: If total Area selected is more than 1000 then the 
                /// "Query for All Areas and Area Map relation with complete info of Area Map Layer without SHP, SHX and DBF"
                /// takes a lot of time to execute.
                /// This also effects the loop below
                /// Query for All Areas and Area Map relation with complete info of Area Map Layer without SHP, SHX and DBF
                if (AreaNIds.Count > 1000)
                {
                    /// TODO: Database based: One Time Process - Could be moved to some better location 
                    sSql = this.DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, "", FieldSelection.Light);
                }
                else
                {
                    sSql = this.DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, sAreaNIds, FieldSelection.Light);
                }
                dvAreaMapLayerInfo = this.DIConnection.ExecuteDataTable(sSql).DefaultView;

                // Filter on Selected Area NIDs
                //dvAreaMapLayerInfo.RowFilter = Area.AreaNId + " in(" + sAreaNIds + ")";
                //StringBuilder sbSortedAreaNIDs = new StringBuilder();
                //sAreaNIds = "," + sAreaNIds + ",";
                //AreaNIds = DIConnection.GetDelimitedValuesFromDataTable(dvAreaMapLayerInfo.Table, Area.AreaNId).Split(',');

                // Query: Get all Shapes from the database (Layer_NId, SHP, SHX and DBF)
                // Prepare a DataView for that - LayerDataView
                // TODO: Database based: One Time Process - Could be moved to some better location 
                //--sSql = this.DIQueries.Area.GetAreaMapLayerByNid("", FieldSelection.Light);
                //--DataTable dtLayerDataTable = this.DIConnection.ExecuteDataTable(sSql);


                //-- Get all Polygon layers (if existed in previous Layers Collection).
                //-- Important DO NOT Delete custom & Feature Layers as not required to be deleted in case of Gallery refresh.
                for (int p = 0; p < this.Layers.Count; p++)
                {
                    if (this.Layers[p].LayerType == ShapeType.Polygon)
                    {
                        ExistingPolygonLayers += this.Layers[p].ID + "^";
                    }
                }

                //-- remove polygon layers which are NOT present in new Layer DataView
                foreach (string lyrId in ExistingPolygonLayers.Split('^'))
                {
                    if (this.m_Layers.LayerIndex(lyrId.Trim()) >= 0)
                    {
                        // If DataView has a row for perticular LayerName, then delete it from Layers Collection
                        if (dvAreaMapLayerInfo.Table.Select(Area_Map_Metadata.LayerName + " ='" + DICommon.RemoveQuotes(lyrId.Trim()) + "'").Length == 0)
                        {
                            this.m_Layers.RemoveAt(this.m_Layers.LayerIndex(lyrId.Trim()));
                        }
                    }
                }

                dvAreaMapLayerInfo.Sort = Area.AreaID + "," + Area_Map_Layer.EndDate;

                //get distinct layers by max end date
                dvAreaMapLayerInfo = this.GetDistinctLayersByMaxEndDate(dvAreaMapLayerInfo.Table).DefaultView;

                // Loop through the SELECTED Areas OR ALL AREAS of the database depending on the number of Areas selected. See above for more details
                //TODO preserve order of adding feature layer while adding to layer collection
                //foreach (string AreaNId in AreaNIds)
                //{

                //// Make _drv for selected AreaNId
                //DataRow[] AreaMapRows = dvAreaMapLayerInfo.Table.Select(Area.AreaNId + " = " + AreaNId);

                //-- Loop each layer associated with this AreaNid, & add that layer in collection
                //foreach (DataRow drAreaMap in AreaMapRows)
                foreach (DataRow drAreaMap in dvAreaMapLayerInfo.Table.Rows)
                {

                    try
                    {
                        if (AreaNIds.Contains(drAreaMap[Area.AreaNId].ToString()))
                        {
                            //Extarct Shape File from database
                            // Get DataRow from the LayerDataView on the basis of the Layer NID available in the Loop
                            //DataRow drLayer = dtLayerDataTable.Select(Area_Map_Layer.LayerNId + " = " + drAreaMap[Area_Map_Layer.LayerNId])[0];

                            string sShapeFileNamewoExtension = Path.Combine(spatialMapFolder, drAreaMap[Area_Map_Metadata.LayerName].ToString());
                            DateTime dUpdateTimeStamp = (DateTime)drAreaMap[Area_Map_Layer.UpdateTimestamp];

                            //*** Extract Files from database only if they doesn't exist in SpatialMap Folder or database contains a newer version of shape file 
                            if (!(File.Exists(sShapeFileNamewoExtension + ".shp") == true & System.DateTime.Compare(dUpdateTimeStamp, File.GetCreationTime(sShapeFileNamewoExtension + ".shp")) <= 0))
                            {
                                Map.ExtractShapeFileByLayerId(drAreaMap[Area_Map_Layer.LayerNId].ToString(), this.SpatialMapFolder, this.DIConnection, this.DIQueries);
                            }


                            //Checking if same Layer ID already exists in Layers Collection, if yes then skip.. 
                            if (Layers[drAreaMap[Area_Map_Metadata.LayerName].ToString()] == null)  //"Layer_Name"
                            {

                                #region "Old"

                                ////*** BugFix 01 Feb 2006 Problem with different Reginal Settings - Thai, Arabic etc
                                //dtTempStartDate = (System.DateTime)(Information.IsDBNull(drAreaMap[Area_Map_Layer.StartDate]) ? DateTime.Parse(DEFAULT_START_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Year, ociEnUS));
                                //dtTempEndDate = (System.DateTime)(Information.IsDBNull(drAreaMap[Area_Map_Layer.EndDate]) ? DateTime.Parse(DEFAULT_END_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Year, ociEnUS));

                                ////Get only those map files whose start date and end date are between Selected TimePeriods / Presentation data time periods
                                //if (System.DateTime.Compare(dtTempEndDate, dtStartDate) < 0 | System.DateTime.Compare(dtTempStartDate, dtEndDate) > 0)
                                //{
                                //    //--- Do nothing
                                //    Console.Write("");

                                //}
                                //else
                                //{

                                //}

                                #endregion

                                ////Add Layer to Layers collection
                                Layer _Layer = m_Layers.AddSpatialLayer(spatialMapFolder, drAreaMap[Area_Map_Metadata.LayerName].ToString());

                                if ((_Layer != null)) //if error while adding layer
                                {
                                    // Store the Layer NIDs in the StringBuilder
                                    this.sbLayerNIDs.Append("," + drAreaMap[Area_Map_Layer.LayerNId]);
                                    this.sbLayerNames.Append("," + drAreaMap[Area_Map_Metadata.LayerName]);

                                    //////Set AreaNames for Layer
                                    ////sSql = this.DIQueries.Area.GetAreaByLayer(drAreaMap[Area_Map_Layer.LayerNId].ToString());
                                    //////foreach (DataRowView _DRVL in m_QueryBase.Map_GetLayerNames(drAreaMap[Area_Map_Layer.LayerNId].ToString()))
                                    ////foreach (DataRowView _DRVL in this.DIConnection.ExecuteDataTable(sSql).DefaultView)
                                    ////{
                                    ////    _Layer.AreaNames.Add(_DRVL["Area_ID"].ToString(), _DRVL["Area_Name"].ToString());
                                    ////}

                                    //Set Layer properties
                                    _Layer.LayerName = drAreaMap[Area_Map_Metadata.LayerName].ToString();
                                    _Layer.Area_Level = int.Parse(drAreaMap[Area.AreaLevel].ToString());
                                    _Layer.LayerType = (ShapeType)(int)drAreaMap[Area_Map_Layer.LayerType];
                                    _Layer.StartDate = dtTempStartDate;
                                    _Layer.EndDate = dtTempEndDate;

                                    //Set property that point layer exists
                                    if (_Layer.LayerType == ShapeType.Point)
                                        this.m_PointShapeIncluded = true;

                                    //Set visibility off for feature layers by default
                                    switch (_Layer.LayerType)
                                    {
                                        case ShapeType.PointFeature:
                                        case ShapeType.PolygonFeature:
                                        case ShapeType.PolyLineFeature:
                                            _Layer.Visible = false;
                                            break;
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                }




                // Get Area Names for all the Layer NIDs used - Background Process
                //FillAreaWorker = new Thread(new ThreadStart(this.FillAreaWorker_DoWork));
                //FillAreaWorker.Start();
                //-- Threading approch discared as it results into reader alreday open error sometimes
                this.FillAreaWorker_DoWork();

                this.AddCustomFeatureLayers();

                //Set Full Extent by default
                SetFullExtent();


                //Create default theme implicitly for first time
                //In case of map loading from report need not to create this default theme
                if (this.m_Themes.Count == 0)
                {
                    this.CreateTheme("-1", "-1", "-1", ThemeType.Color);   // for default Theme, use "-1" as NIDs
                }
                else
                {
                    // If Themes are already present then preserve IUS and ThemeType , and re-create Theme again.
                    // New Theme is re-created on the basis of new PresentationData , new userSelection, and new Layers added..
                    // So Old Theme's settings must be restored in New Theme.
                    Theme TempTheme = new Theme();
                    Theme NewTheme = null;
                    string[] ThemeIdSettings;
                    string ThemeId;
                    string[] I_U_S_NIds = null;     //-- NIds of I, U, S used in  Theme

                    for (int c = 0; c < this.Themes.Count; c++)
                    {

                        //_Theme.ID = p_IndicatorNId + "_" + p_UnitNId + "_" + p_SubgroupValNId + "_" + (int)p_ThemeType;

                        TempTheme = this.Themes[c];
                        ThemeId = TempTheme.ID;
                        TempTheme.ID = "TempThemeID";   // ID changed temperarily.
                        TempTheme = (Theme)(TempTheme.Clone());

                        int TempThemeIndex = this.Themes.ItemIndex(ThemeId);

                        ThemeIdSettings = ThemeId.Split('_');

                        //-- Get Nids of Indicator, Unit, Subgroupval used in Theme. on the basis of their GIDs
                        I_U_S_NIds = this.GetI_U_S_NidsByGIDs(this._DIDataView.MainDataTable.DefaultView, TempTheme.I_U_S_GIDs[0], TempTheme.I_U_S_GIDs[1], TempTheme.I_U_S_GIDs[2]);

                        //--Create Theme again and add it in same i th position in collection.
                        try
                        {
                            // Check whather there is only one IUS in userSelection
                            // if one, then IUS must have been changed outside(in Dynamic reports). so, createTheme for default.
                            // if more than one, then createTheme for previus IUS
                            if (this.UserPreference.UserSelection.ShowIUS)
                            {
                                if (this.UserPreference.UserSelection.IndicatorNIds.Split(',').Length == 1)
                                {
                                    //if Single IUS, then create Default theme.
                                    if (TempTheme.Type == ThemeType.Chart)
                                    {
                                        NewTheme = this.CreateTheme("-1", "-1", "-1", (ThemeType)int.Parse(ThemeIdSettings[3]), TempTheme.ChartType, TempTheme.ChartSeriestype, c);
                                    }
                                    else
                                    {
                                        NewTheme = this.CreateTheme("-1", "-1", "-1", (ThemeType)int.Parse(ThemeIdSettings[3]), c);
                                    }
                                }
                                else
                                {
                                    // Multiple IUS indicates that IUS are same as previous
                                    // so, UpdateTheme for same IUS
                                    if (TempTheme.Type == ThemeType.Chart)
                                    {
                                        NewTheme = this.CreateTheme(I_U_S_NIds[0], I_U_S_NIds[1], I_U_S_NIds[2], (ThemeType)int.Parse(ThemeIdSettings[3]), TempTheme.ChartType, TempTheme.ChartSeriestype, c);
                                    }
                                    else
                                    {
                                        NewTheme = this.CreateTheme(I_U_S_NIds[0], I_U_S_NIds[1], I_U_S_NIds[2], (ThemeType)int.Parse(ThemeIdSettings[3]), c);
                                    }

                                    //-- Set Theme Name same as Previous
                                    NewTheme.Name = TempTheme.Name;
                                }
                            }
                            else
                            {
                                if (this.UserPreference.UserSelection.IndicatorNIds.Split(',').Length == 1 && this.UserPreference.UserSelection.UnitNIds.Split(',').Length == 1 && this.UserPreference.UserSelection.SubgroupValNIds.Split(',').Length == 1)
                                {
                                    //if Single IUS, then create Default theme.
                                    NewTheme = this.CreateTheme("-1", "-1", "-1", (ThemeType)int.Parse(ThemeIdSettings[3]), c);
                                }
                                else
                                {
                                    // Multiple IUS indicates that IUS is same as previous IUS in current Theme
                                    // so, UpdateTheme for prevoius IUS 
                                    NewTheme = this.CreateTheme(I_U_S_NIds[0], I_U_S_NIds[1], I_U_S_NIds[2], (ThemeType)int.Parse(ThemeIdSettings[3]), c);

                                    //-- Set Theme Name same as Previous
                                    NewTheme.Name = TempTheme.Name;
                                }
                            }
                            // Update new Theme with Old Theme's properties
                            this.UpdateThemeProperties(TempTheme, NewTheme, preserveLegendsRanges, true);

                            // remove old Theme
                            this.Themes.Remove(this.Themes.ItemIndex("TempThemeID"));

                        }
                        catch (Exception ex)
                        {
                            //If theme already exists exception will be thrown ApplicationException("3")
                            //If ThemeData.Count == 0 exception will be thrown ApplicationException("5")
                            // Exception to be handled in Desktop web and reporting panel which call Initialize method
                            throw ex;
                        }
                    }
                    int RecordCount = m_Layers.RecordCounts(m_SpatialMapFolder);

                    //-- Set visibility of color theme which was earlier set true when Map Serialized.
                    this.ResetColorThemeVisibility();

                    //- Clear inset images of previous Themes (if any)
                    try
                    {
                        foreach (Inset inset in this.m_Insets)
                        {
                            if (inset.InsetImage != null)
                            {
                                inset.InsetImage.Dispose();
                                inset.InsetImage = null;
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                this.SetMapTitle();
            }
            else   // If No Record Found in Presentation Data, then Clear Themes & Layers collection.
            {
                this.Themes.Clear();
                this.Layers.Clear();
            }
        }

        /// <summary>
        /// Get distinct layers by maximum end date like if 2 layers exists for UP then it will pick only one which has max end date
        /// some area name displayed twice at map.(Peter Leth)
        /// </summary>
        /// <param name="dvAreaMapLayerInfo"></param>
        /// <returns></returns>
        private DataTable GetDistinctLayersByMaxEndDate(DataTable dvAreaMapLayerInfo)
        {
            DataTable RetVal = null;
            DataTable TempTable = null;
            DataRow[] tempRows = null;

            List<DateTime> EndDatetimeList = new List<DateTime>();
            List<string> selectedTimePeriod = new List<string>();

            DateTime MaxDatetime = new DateTime();
            string LayerName = string.Empty;
            DataRow[] tempRowsByArea = null;
            int TotalLength = 0;
            Dictionary<DateTime, string> LayerList = new Dictionary<DateTime, string>();
            DataTable DistinctTimePeriodAndArea;

            DateTime dtStartDate = new DateTime();
            DateTime dtEndDate = new DateTime();
            DateTime dtTempStartDate = DateTime.Parse(DEFAULT_START_DATE).Date;
            DateTime dtTempEndDate = DateTime.Parse(DEFAULT_START_DATE).Date;
            DataRow[] TempDatetimeRows;
            string DeletedFilterText = string.Empty;

            Dictionary<string, DateTime> LayerWithDate = new Dictionary<string, DateTime>();
            List<DateTime> LayerWithDateList = new List<DateTime>();

            try
            {
                DistinctTimePeriodAndArea = this.GetDistinctTimePeriodsAndArea();

                TempTable = dvAreaMapLayerInfo.DefaultView.ToTable(true, Area_Map_Metadata.LayerName, Area.AreaID, Area_Map_Layer.EndDate);

                if (UserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Length > 0)
                {
                    DeletedFilterText = Data.DataNId + " not in(" + UserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")";
                }

                selectedTimePeriod = DICommon.GetCommaSeperatedListOfGivenColumn(this.PresentationData.ToTable(), Timeperiods.TimePeriod, false, DeletedFilterText);

                foreach (DataRow AreaRow in TempTable.DefaultView.ToTable(true, Area.AreaID).Rows)
                {
                    tempRows = null;
                    tempRows = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "'");
                    DistinctTimePeriodAndArea.DefaultView.RowFilter = Area.AreaID + "='" + AreaRow[Area.AreaID] + "'";
                    DistinctTimePeriodAndArea.DefaultView.Sort = Timeperiods.TimePeriod;

                    //if more than one row exist then get layer of max end date otherwise delete that record
                    if (tempRows.Length > 0)
                    {
                        EndDatetimeList.Clear();
                        LayerList.Clear();
                        LayerWithDate.Clear();
                        LayerWithDateList.Clear();

                        //1. get max end date                    
                        foreach (DataRow row in tempRows)
                        {
                            TempDatetimeRows = DistinctTimePeriodAndArea.DefaultView.ToTable().Select();
                            LayerName = Convert.ToString(row[Area_Map_Metadata.LayerName]);

                            dtStartDate = Convert.ToDateTime(row[Area_Map_Layer.StartDate]);
                            dtEndDate = Convert.ToDateTime(row[Area_Map_Layer.EndDate]);

                            if (!EndDatetimeList.Contains(dtEndDate))
                                EndDatetimeList.Add(dtEndDate);

                            if (!LayerList.ContainsKey(dtEndDate))
                            {
                                DataRow[] layerRow = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "' and " + Area_Map_Layer.EndDate + "='" + dtEndDate + "'");
                                if (layerRow.Length > 0)
                                {
                                    LayerName = Convert.ToString(layerRow[0][Area_Map_Metadata.LayerName]);
                                }

                                LayerList.Add(dtEndDate, LayerName);
                            }
                        }

                        EndDatetimeList.Sort();
                        selectedTimePeriod.Sort();

                        //get layer by timeperiod in multiple layer case
                        if (selectedTimePeriod.Count > 0)
                        {
                            TimePeriodFacade.SetStartDateEndDate(selectedTimePeriod[selectedTimePeriod.Count - 1], ref dtTempStartDate, ref dtTempEndDate);
                            for (int i = 0; i < EndDatetimeList.Count; i++)
                            {
                                if (dtTempEndDate.CompareTo(EndDatetimeList[i]) <= 0)
                                {
                                    MaxDatetime = EndDatetimeList[i];
                                    break;
                                }
                            }
                        }

                        if (!LayerList.ContainsKey(MaxDatetime))
                            MaxDatetime = EndDatetimeList[EndDatetimeList.Count - 1];

                        LayerName = LayerList[MaxDatetime];

                        //2. filter area and max timeperiod from table
                        tempRowsByArea = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "' and " + Area_Map_Metadata.LayerName + "<>'" + LayerName + "'");

                        TotalLength = tempRowsByArea.Length;

                        for (int i = 0; i < TotalLength; i++)
                        {
                            tempRowsByArea[i].Delete();
                        }

                        dvAreaMapLayerInfo.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            RetVal = dvAreaMapLayerInfo;

            return RetVal;
        }

        private void SetMapTitle()
        {
            if (this.m_Themes.Count > 0)
            {
                m_Title = m_Themes[0].IndicatorName[0];
                m_Subtitle = m_Themes[0].UnitName;
                if (m_Themes[0].Type != ThemeType.Chart && m_Themes[0].SubgroupName.Length > 0)
                {
                    m_Subtitle += " " + m_Themes[0].SubgroupName[0];
                }

                if (this.MapTitleChanged != null)
                {
                    this.MapTitleChanged(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Sets back the visibility of color theme which was earlier set true when Map Serialized.
        /// </summary>
        internal void ResetColorThemeVisibility()
        {
            try
            {
                if (string.IsNullOrEmpty(this._VisibleColorThemeIDWhenSerialized) == false)
                {
                    if (this.m_Themes.ItemIndex(this._VisibleColorThemeIDWhenSerialized) >= 0)
                    {
                        //-- Set all Color themes Visiblilty false
                        foreach (Theme _Theme in this.m_Themes)
                        {
                            if (_Theme.Type == ThemeType.Color)
                            {
                                if (_Theme.ID == this._VisibleColorThemeIDWhenSerialized)
                                {
                                    //-- Set Visibility true of Color theme when Themes Serialized
                                    _Theme.Visible = true;
                                }
                                else
                                {
                                    _Theme.Visible = false;
                                }
                            }
                        }

                        //- Clear variable
                        this._VisibleColorThemeIDWhenSerialized = string.Empty;
                    }
                }
            }
            catch
            {
            }
        }

        //*** Constructor and Destructor
        public Map() //TODO make private check its usage + confirm for xml serialization
        {
            //DO Nothing just implemented for the serialized object to be deserialized properly
        }

        public void Dispose()
        {
            //if ((m_QueryBase != null))
            //    m_QueryBase.Dispose();
            try
            {
                //-- Clearing all variable  of Map object                

                this.UserPreference.Dispose();
                this._DIDataView = null;
                this._PresentationData.Dispose();
                this._MRDData.Dispose();
                this.m_Layers.Clear();
                this.m_Themes.Clear();
                this.m_Insets.Clear();
                this.m_CFLCol.Clear();

            }
            catch
            {

            }

        }

        #endregion

        #region "-- Load & Save --"

        /// <summary>
        /// Loads the Map object from serialized xml Map file.
        /// </summary>
        /// <param name="serializeFileName">file Path of xml serialized Map object.</param>
        /// <returns>Map object.</returns>
        public static Map Load(string serializeFileName)
        {
            Map RetVal = null;
            FileStream _IO = null;
            if (File.Exists(serializeFileName))
            {
                try
                {
                    _IO = new FileStream(serializeFileName, FileMode.Open);
                    XmlSerializer _SRZFrmt = new XmlSerializer(typeof(Map));
                    RetVal = (Map)_SRZFrmt.Deserialize(_IO);
                    _IO.Flush();
                    _IO.Close();
                }
                catch (System.Runtime.Serialization.SerializationException ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                    if (_IO != null)
                    {
                        _IO.Close();
                    }
                }
            }

            return RetVal;
        }


        /// <summary>
        /// Map Object loads (DeSerialized) using Serialized XML fileName.
        /// <para>List of Properties that were ignored in Xml Serialisation:- 
        ///      Map.CFL
        ///      Map.GetData
        ///      Map.MapFolder 
        ///      Map.PresentationData
        ///      Layer.Records
        ///      Layer.ModifiedLabel
        ///      _________
        /// List of Functionalities that will be affected (will not work) in case of Dynamic Report in UI Hosting application.
        ///      CFL (If any) will not be visible.
        ///      Buffer (if any) will not be visible.
        ///      Insets (if any) will not be visible
        ///      When Base Layer changed , or new Layer added from database, then old layer will be visible instead of new Layer. If layer is browsed through custom location it shall work.
        ///      Multilegend if turned on will be reset to default theme for color themes
        /// </para>
        /// </summary>
        public static Map Load(DIConnection dIConnection, DIQueries dIQueries, string serializeFileName, string maskedFileWPath)
        {

            Map _Map = null;
            if (File.Exists(serializeFileName))
            {
                try
                {
                    FileStream _IO = new FileStream(serializeFileName, FileMode.Open);
                    XmlSerializer _SRZFrmt = new XmlSerializer(typeof(Map));
                    _Map = (Map)_SRZFrmt.Deserialize(_IO);
                    _IO.Flush();
                    _IO.Close();

                    //-- Get UserSelection object and preserve it
                    UserSelection userSelectionOld = (UserSelection)(_Map.UserPreference.UserSelection.Clone());

                    // update userSelection's GID
                    _Map.UserPreference.UserSelection.UpdateNIdsFromGIds(dIConnection, dIQueries);

                    //-- Compare new userSelection after updating NID from GID with old userSelection .
                    if (UserSelection.CompareIndicatorTimePeriodArea(userSelectionOld, _Map.UserPreference.UserSelection))
                    {
                        //-- If true, then there must be some missing IUS, A, Timeperiod in new UserSelection
                        //-- In order to enable UI to display NO record found message, Change TimePeriod NId = -1, 
                        //-- so that DataView will have ZERO Records.
                        _Map.UserPreference.UserSelection.TimePeriodNIds = "-10";
                    }

                    // TODO set DIDataView UserPreference ?
                    // -- Get DIDataView  based on the deserialized Map.userPreference.
                    _Map._DIDataView = new DIDataView(_Map._UserPreference, dIConnection, dIQueries, maskedFileWPath, "");

                    // DIDataView.GetAllDataByUserSelection() generates and returns PresentationData required in Map.
                    _Map._PresentationData = _Map._DIDataView.GetAllDataByUserSelection();

                    _Map.DIConnection = dIConnection;
                    _Map.DIQueries = dIQueries;


                    //Set Metadata columns info
                    _Map.SetMetadataInfo();

                    //--Merge dataValue of DrilledDown Areas also.
                    Map.MergeDataOfDrilledDownAreas(string.Join(",", _Map._DrillDownAreaNIds.ToArray()), string.Join(",", _Map._DrillDownAreaIDs.ToArray()), _Map);

                }
                catch (System.Runtime.Serialization.SerializationException ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }

            return _Map;
        }

        public static Map LoadFromSerializeText(DIConnection dIConnection, DIQueries dIQueries, string SerializeText, string maskedFileWPath, string languageFileWPath)
        {
            Map RetVal;
            try
            {
                XmlSerializer MapSerialize = new XmlSerializer(typeof(Map));
                MemoryStream MemoryStream = new MemoryStream(DICommon.StringToUTF8ByteArray(SerializeText));
                RetVal = (Map)MapSerialize.Deserialize(MemoryStream);

                RetVal.UserPreference.UserSelection.UpdateNIdsFromGIds(dIConnection, dIQueries);

                // TODO set DIDataView UserPreference ?
                // -- Get DIDataView  based on the deserialized Map.userPreference.
                RetVal._DIDataView = new DIDataView(RetVal._UserPreference, dIConnection, dIQueries, maskedFileWPath, "");
                //-- Set _PresentationData
                RetVal._PresentationData = RetVal._DIDataView.GetAllDataByUserSelection();



                RetVal.DIConnection = dIConnection;
                RetVal.DIQueries = dIQueries;

                //--Merge dataValue of DrilledDown Areas also.
                Map.MergeDataOfDrilledDownAreas(string.Join(",", RetVal._DrillDownAreaNIds.ToArray()), string.Join(",", RetVal._DrillDownAreaIDs.ToArray()), RetVal);

                //Set Metadata columns info
                RetVal.SetMetadataInfo();


                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Load Map form the deserialized XML text.
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        public static Map LoadFromSerializeText(string SerializeText, DIConnection dIConnection, DIQueries dIQueries)
        {
            Map RetVal;
            try
            {
                XmlSerializer MapSerialize = new XmlSerializer(typeof(Map));
                MemoryStream MemoryStream = new MemoryStream(DICommon.StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (Map)MapSerialize.Deserialize(MemoryStream);
                RetVal.UserPreference.UserSelection.UpdateNIdsFromGIds(dIConnection, dIQueries);

                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load Map form the deserialized XML text.
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        public static Map LoadFromSerializeText(string SerializeText)
        {
            Map RetVal;
            try
            {
                XmlSerializer MapSerialize = new XmlSerializer(typeof(Map));
                MemoryStream MemoryStream = new MemoryStream(DICommon.StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (Map)MapSerialize.Deserialize(MemoryStream);

                MemoryStream.Dispose();
            }
            catch
            {
                RetVal = null;
            }
            return RetVal;
        }

        public void SetDBConnection(DIConnection dIConnection, DIQueries dIQueries)
        {
            this.DIConnection = dIConnection;
            this.DIQueries = dIQueries;

            //TODO Update DIDataView instance
            //this.SetMRDData();

        }

        public void Save(string p_FileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(p_FileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(p_FileName));
            m_SpatialMapFolder = Path.GetDirectoryName(p_FileName);
            FileStream _IO = new FileStream(p_FileName, FileMode.Create);
            XmlSerializer _SRZFrmt = new XmlSerializer(typeof(Map));
            // BinaryFormatter _SRZFrmt = new BinaryFormatter();
            _SRZFrmt.Serialize(_IO, this);
            _IO.Flush();
            _IO.Close();
        }

        public void Save(string fileNameWpath, GoogleMapFileType mapFileType, bool includeLegend, string tempImageFolder)
        {
            string ImageFile = "DI.png";
            string ImageFileWPath = Path.Combine(tempImageFolder, ImageFile);
            string LegendFile = "Legend.png";
            string LegendFileWPath = Path.Combine(tempImageFolder, LegendFile);
            string MapTempFile = string.Empty;
            string XmlPath = string.Empty;
            const string KMZDocFileName = "doc.kml";

            try
            {
                //If MapImage top point latitude is more than 90 degree, OR  bottom point latitude is less than -90 degree,  
                //Then manipulate bonding box of map to generate image till valid latitude.

                float OrgMapHeight = this.m_Height;
                //Preserve Map's original height.
                float MapBoundWHRatio = this.FullExtent.Width / this.FullExtent.Height;
                //Calculating Map's bounding rectangle width to height ratio.
                double DesiredMapHeight = this.m_Width / MapBoundWHRatio;
                //Desired Height calculated is equivalent to Map.FullExtent - width to height Ratio.
                PointF PtTopLeft = this.PointToClient(0, 0);
                PointF PtBottomRight = this.PointToClient(this.m_Width, this.m_Height);
                if (PtTopLeft.Y > 90 | PtBottomRight.Y < -90)
                {
                    PtTopLeft.Y = this.FullExtent.Y;
                    PtBottomRight.Y = this.FullExtent.Y - this.FullExtent.Height;
                    this.m_Height = (int)DesiredMapHeight;
                    //Assign desired height for Map .
                }

                //Generate Map Image in Temp Folder
                ImageFileWPath = Path.Combine(tempImageFolder, ImageFile);
                this.GetMapImage(Path.GetDirectoryName(ImageFileWPath), Path.GetFileNameWithoutExtension(ImageFile), Path.GetExtension(ImageFile).Substring(1), false);


                //Restore original Map height.
                this.Height = OrgMapHeight;

                //-- Get Active Theme Name
                string ThemeName = string.Empty;
                foreach (Theme _Theme in this.Themes)
                {
                    if (_Theme.Type == ThemeType.Color & _Theme.Visible == true)
                    {
                        ThemeName = _Theme.Name;

                        //-Remove Special Characters like (< > " &)
                        ThemeName = ThemeName.Replace("&", "&amp;");
                        ThemeName = ThemeName.Replace("<", "&lt;");
                        ThemeName = ThemeName.Replace(">", "&gt;");
                        ThemeName = ThemeName.Replace("\"", "&quot;");
                        ThemeName = ThemeName.Replace("'", "&apos;");
                        break;
                    }
                }

                //-- Set xml file name
                if (mapFileType == GoogleMapFileType.KML)
                {
                    XmlPath = fileNameWpath;
                }
                else
                {
                    //--- For KMZ, xml filename is always doc.kml
                    XmlPath = Path.Combine(tempImageFolder, KMZDocFileName);
                }

                if (File.Exists(XmlPath))
                {
                    File.Delete(XmlPath);
                }

                //-- Generate KML file for GoogleEarth
                StreamWriter swGE = new StreamWriter(XmlPath);
                swGE.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                swGE.WriteLine("<kml xmlns=\"http://earth.google.com/kml/2.2\">");
                if (mapFileType == GoogleMapFileType.KMZ)
                {
                    swGE.WriteLine("<Folder>");
                }
                swGE.WriteLine("<GroundOverlay>");
                swGE.WriteLine("    <name>" + ThemeName + "</name>");
                swGE.WriteLine("    <color>bbffffff</color>");
                swGE.WriteLine("    <drawOrder>1</drawOrder>");
                swGE.WriteLine("    <Icon>");
                if (mapFileType == GoogleMapFileType.KML)
                {
                    swGE.WriteLine("        <href>" + ImageFileWPath + "</href>");
                }
                else if (mapFileType == GoogleMapFileType.KMZ)
                {
                    swGE.WriteLine("        <href>" + ImageFile + "</href>");
                }
                swGE.WriteLine("    </Icon>");
                swGE.WriteLine("    <LatLonBox>");
                swGE.WriteLine("        <north>" + PtTopLeft.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + "</north>");
                swGE.WriteLine("        <south>" + PtBottomRight.Y.ToString(System.Globalization.CultureInfo.InvariantCulture) + "</south>");
                swGE.WriteLine("        <east>" + PtBottomRight.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + "</east>");
                swGE.WriteLine("        <west>" + PtTopLeft.X.ToString(System.Globalization.CultureInfo.InvariantCulture) + "</west>");
                swGE.WriteLine("    </LatLonBox>");
                swGE.WriteLine("</GroundOverlay>");

                if (includeLegend)
                {
                    //Generate legend image for color theme
                    Theme ActiveTheme = this.Themes.GetActiveTheme();
                    if (ActiveTheme != null)
                    {
                        this.GetCompositeLegendImage(tempImageFolder, Path.GetFileNameWithoutExtension(LegendFile), Path.GetExtension(LegendFile).Substring(1), tempImageFolder);
                        //ActiveTheme.GetLegendImage(tempImageFolder, Path.GetFileNameWithoutExtension(LegendFile), Path.GetExtension(LegendFile).Substring(1), this.PointShapeIncluded, this.TemplateStyle.Legends.ShowCaption, this.TemplateStyle.Legends.ShowRange);
                    }

                    swGE.WriteLine("<ScreenOverlay>");
                    swGE.WriteLine("    <name>Legend</name>");
                    swGE.WriteLine("    <Icon>");
                    if (mapFileType == GoogleMapFileType.KML)
                    {
                        swGE.WriteLine("        <href>" + LegendFileWPath + "</href>");
                    }
                    else if (mapFileType == GoogleMapFileType.KMZ)
                    {
                        swGE.WriteLine("        <href>" + LegendFile + "</href>");
                    }
                    swGE.WriteLine("    </Icon>");
                    swGE.WriteLine("    <overlayXY x='0.01' y='0.99' xunits='fraction' yunits='fraction'/>");
                    swGE.WriteLine("    <screenXY x='0.01' y='0.99' xunits='fraction' yunits='fraction'/>");
                    swGE.WriteLine("    <size x='-1' y='-1' xunits='pixels' yunits='pixels'/>");
                    swGE.WriteLine("</ScreenOverlay>");
                }
                if (mapFileType == GoogleMapFileType.KMZ)
                {
                    swGE.WriteLine("</Folder>");
                }
                swGE.WriteLine("</kml>");
                swGE.Close();

                if (mapFileType == GoogleMapFileType.KMZ)
                {
                    ZipOutputStream zipOut = null;
                    ZipEntry entry = null;
                    FileStream sReader = null;
                    try
                    {
                        //-- Zip files (KML & Image) together to form KMZ.
                        string sZipFilePath = Path.GetDirectoryName(fileNameWpath) + @"\" + Path.GetFileNameWithoutExtension(fileNameWpath) + DICommon.FileExtension.KMZ;

                        // Create a Zip file in the stock folder and then provide a download link
                        zipOut = new ZipOutputStream(File.Create(sZipFilePath));

                        // add kml to the zip file.
                        System.IO.FileInfo fi = new System.IO.FileInfo(XmlPath);

                        entry = new ZipEntry(fi.Name);
                        sReader = File.OpenRead(XmlPath);
                        byte[] buff = new byte[Convert.ToInt32(sReader.Length)];
                        sReader.Read(buff, 0, (int)sReader.Length);
                        entry.DateTime = fi.LastWriteTime;
                        entry.Size = sReader.Length;
                        sReader.Close();
                        zipOut.PutNextEntry(entry);
                        zipOut.Write(buff, 0, buff.Length);

                        //-- zip images file
                        entry = new ZipEntry(Path.GetFileName(ImageFileWPath));
                        sReader = File.OpenRead(ImageFileWPath);
                        buff = new byte[Convert.ToInt32(sReader.Length)];
                        sReader.Read(buff, 0, buff.Length);
                        entry.Size = sReader.Length;
                        sReader.Close();
                        zipOut.PutNextEntry(entry);
                        zipOut.Write(buff, 0, buff.Length);

                        if (includeLegend)
                        {
                            //-- zip legend file
                            entry = new ZipEntry(Path.GetFileName(LegendFileWPath));
                            sReader = File.OpenRead(LegendFileWPath);
                            buff = new byte[Convert.ToInt32(sReader.Length)];
                            sReader.Read(buff, 0, buff.Length);
                            entry.Size = sReader.Length;
                            sReader.Close();
                            zipOut.PutNextEntry(entry);
                            zipOut.Write(buff, 0, buff.Length);
                        }

                        zipOut.Finish();
                        zipOut.Close();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (zipOut != null)
                        {
                            zipOut.Close();
                        }

                        if (sReader != null)
                        {
                            sReader.Close();
                            sReader = null;
                        }

                    }

                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Gets the xml serialized text of Map object.
        /// </summary>
        /// <param name="updateGId"></param>
        /// <returns></returns>
        public string GetSerializedText(bool updateGId)
        {
            string RetVal = string.Empty;
            if (updateGId)
            {
                this.UserPreference.UserSelection.UpdateNIdsFromGIds(this.DIConnection, this.DIQueries);
            }
            XmlSerializer MapSerializer = new XmlSerializer(typeof(Map));
            MemoryStream MemoryStream = new MemoryStream();
            try
            {
                MapSerializer.Serialize(MemoryStream, this);

            }
            catch (Exception ex)
            {

                throw;
            }
            MemoryStream.Position = 0;
            RetVal = new StreamReader(MemoryStream).ReadToEnd();
            MemoryStream.Close();
            return RetVal;
        }

        #endregion

        #region "-- Add / Extract Layer --"
        // Get AreaName in each Layer in this.Layers[]
        // Layer.AreaNames.Add(ID, AreaName)
        private void FillAreaWorker_DoWork()
        {
            try
            {
                if (this.sbLayerNIDs.Length > 0)
                {
                    string sLayerNIDs = sbLayerNIDs.ToString().Substring(1);
                    string[] arrLayerNIDs = sLayerNIDs.Split(',');
                    string[] arrLayerNames = this.sbLayerNames.ToString().Substring(1).Split(',');
                    DataRow[] dtRow;
                    // Query - Get All reas against the Layer NIDs
                    string sSql = this.DIQueries.Area.GetAreaByLayer(sLayerNIDs);
                    DataTable dtLayerArea = this.DIConnection.ExecuteDataTable(sSql);
                    for (int j = 0; j < arrLayerNIDs.Length; j++)
                    {
                        // Get Area Names for each Layer in the Stribguilder
                        dtRow = dtLayerArea.Select(Area_Map_Layer.LayerNId + " = " + arrLayerNIDs[j]);
                        foreach (DataRow dtRowArea in dtRow)
                        {
                            //Layers[sbLayerNames[i]]
                            try
                            {
                                m_Layers[arrLayerNames[j]].AreaNames.Add(dtRowArea[Area.AreaID].ToString(), dtRowArea[Area.AreaName].ToString());

                            }
                            catch (Exception ex)
                            {
                                //-- Cases when m_Layers.arrLayerNames[j] = null
                                System.Console.Write(ex.Message + arrLayerNames[j]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Sometime exception is thrown in case of online db Connection 
                // when two concurrent Query are exceuted in differetn Threads.
                System.Diagnostics.Debug.Print(ex.Message);
            }

        }

        public static void ExtractAllShapeFiles(DIConnection diConnection, DIQueries diQueries, string spatialMapFolder)
        {
            // Query: Get all Shapes from the database (Layer_NId, SHP, SHX and DBF)
            // Prepare a DataView for that - LayerDataView
            string sSql = diQueries.Area.GetAreaMapLayerByNid("", FieldSelection.Heavy);

            DataTable dtLayerDataTable = diConnection.ExecuteDataTable(sSql);

            // Loop through each layer in DataTable
            foreach (DataRow dRowLayerShape in dtLayerDataTable.Rows)
            {
                // Extract shapeFiles for this Layer
                Map.ExtractShapeFile(dRowLayerShape, spatialMapFolder);
            }
            dtLayerDataTable = null;
        }

        public bool ExtractShapeFile(string layerName, string spatialMapFolder)
        {
            bool RetVal = false;
            string sSql = this.DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.LayerName, "'" + DICommon.RemoveQuotes(layerName) + "'", FieldSelection.Heavy);
            DataView _Dv = this.DIConnection.ExecuteDataTable(sSql).DefaultView;
            foreach (DataRowView _DRv in _Dv)
            {
                RetVal = Map.ExtractShapeFile(_DRv, spatialMapFolder);
                break;
            }
            return RetVal;
        }

        public static bool ExtractShapeFileByLayerId(string layerNId, string spatialMapFolder, DIConnection DIConnection, DIQueries DIQueries)
        {
            bool RetVal = false;
            string sSql = DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.LayerNId, layerNId, FieldSelection.Heavy);
            DataView _Dv = DIConnection.ExecuteDataTable(sSql).DefaultView;
            foreach (DataRowView _DRv in _Dv)
            {
                RetVal = Map.ExtractShapeFile(_DRv, spatialMapFolder);
                break;
            }
            return RetVal;
        }

        public static bool ExtractShapeFile(string areaNId, string spatialMapFolder, DIConnection DBConnection, DIQueries DBQueries)
        {
            bool RetVal = false;

            //-- Get layers Associated with this areaNID
            string sSql = DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, areaNId, FieldSelection.Heavy);
            DataTable _Dv = DBConnection.ExecuteDataTable(sSql);

            //-- Extract Layers
            foreach (DataRow _DR in _Dv.Rows)
            {
                RetVal = Map.ExtractShapeFile(_DR, spatialMapFolder);
            }

            return RetVal;
        }

        private static bool ExtractShapeFile(DataRowView dataRowView, string spatialMapFolder)
        {
            return Map.ExtractShapeFile(dataRowView.Row, spatialMapFolder);
        }

        private static bool ExtractShapeFile(DataRow dataRow, string spatialMapFolder)
        {
            bool RetVal = false;
            string sShapeFileNamewoExtension = Path.Combine(spatialMapFolder, dataRow[Area_Map_Metadata.LayerName].ToString());
            DateTime dUpdateTimeStamp = (DateTime)dataRow[Area_Map_Layer.UpdateTimestamp];

            try
            {

                //*** Extract Files from database only if they doesn't exist in SpatialMap Folder or database contains a newer version of shape file 
                // Changed the DateTime Stamp Comparison from >0 to ==0 - This means the shape file will be extracted everytime if there's a change in the DateTimeStamp. Earlier it used to keep the Shape files with the latest DateTime Stamp as a result Databases with older shape files used to show maps with missing data
                if (File.Exists(sShapeFileNamewoExtension + ".shp") == true & System.DateTime.Compare(dUpdateTimeStamp, File.GetCreationTime(sShapeFileNamewoExtension + ".shp")) == 0)
                {
                    //Dont extract shapefile if it already exists in map folder and its time stamp is greater than that of database timestamp
                    //DateTime.Compare < 0 => t1 < t2. DateTime.Compare = 0 => t1 = t2. DateTime.Compare > 0 => t1 > t2. 
                    RetVal = true;
                }
                else
                {
                    Byte[] Buffer;
                    FileStream fs = null;
                    BinaryWriter bw = null;

                    //*Check if value is NOT blank
                    if (dataRow[Area_Map_Layer.LayerShp] != null)
                    {
                        //*** Create Shp File 
                        Buffer = (Byte[])dataRow[Area_Map_Layer.LayerShp];
                        fs = new FileStream(sShapeFileNamewoExtension + ".shp", FileMode.Create);
                        bw = new BinaryWriter(fs);
                        bw.Write(Buffer);
                        bw.Flush();
                        bw.Close();
                        fs.Close();

                        //*** Create Shx File 
                        Buffer = (Byte[])dataRow[Area_Map_Layer.LayerShx];
                        fs = new FileStream(sShapeFileNamewoExtension + ".shx", FileMode.Create);
                        bw = new BinaryWriter(fs);
                        bw.Write(Buffer);
                        bw.Flush();
                        bw.Close();
                        fs.Close();

                        //*** Create Dbf File 
                        Buffer = (Byte[])dataRow[Area_Map_Layer.Layerdbf];
                        fs = new FileStream(sShapeFileNamewoExtension + ".dbf", FileMode.Create);
                        bw = new BinaryWriter(fs);
                        bw.Write(Buffer);
                        bw.Flush();
                        bw.Close();
                        fs.Close();

                        //*** Set The creation time of file as that of Update_Timestamp field in database 
                        try
                        {
                            File.SetCreationTime(sShapeFileNamewoExtension + ".shp", dUpdateTimeStamp);
                            File.SetCreationTime(sShapeFileNamewoExtension + ".shx", dUpdateTimeStamp);
                            File.SetCreationTime(sShapeFileNamewoExtension + ".dbf", dUpdateTimeStamp);
                        }
                        catch (Exception ex)
                        {
                            //Console.Write(ex.Message) 
                        }

                        // Encrypt ShapeFile(.shp) header info in existing folder, so that any third party can't read shape file directly.
                        try
                        {
                            ShapeFileReader.EncryptDecryptShapeFile(true, sShapeFileNamewoExtension + ".shp");
                        }
                        catch (Exception ex)
                        {
                            // Do nothing
                        }

                        RetVal = true;
                    }
                    else
                    {
                        RetVal = false;
                    }
                }


            }
            catch (PathTooLongException)
            {

            }

            catch (Exception e)
            {

                //throw;
            }
            return RetVal;
        }

        private void AddCustomFeatureLayers()
        {
            if (Directory.Exists(m_CFLPath))
            {
                int i = 0;
                BinaryFormatter oBinaryFormatter = new BinaryFormatter();
                //XmlSerializer oXmlSerializer = new XmlSerializer(typeof(CustomFeature));
                DirectoryInfo oDirInfo = new DirectoryInfo(m_CFLPath);
                FileInfo[] oFileArr = oDirInfo.GetFiles("*.dif");
                //*** look only for DevInfoFeature Files

                //Deserilized map will already contain CFL. 
                //Preserve listing of cfl in deserialized map.
                //Clear all cfl loaded by deserializxation (as symbol images are not loaded through xml deserialization)
                //Add CFL from  CFL Folder. If It was part of deserialized map set its visibility to true
                Dictionary<string, bool> ArrCFL = new Dictionary<string, bool>();
                for (int j = 0; j < m_CFLCol.Count; j++)
                {
                    ArrCFL.Add(m_CFLCol[j].Name, m_CFLCol[j].Visible);
                }
                m_CFLCol.Clear();


                foreach (FileInfo oFileInfo in oFileArr)
                {
                    try
                    {
                        FileStream oFileStream = new FileStream(oFileInfo.FullName, FileMode.Open);
                        //m_CFLCol.Add((CustomFeature)oXmlSerializer.Deserialize(oFileStream));


                        m_CFLCol.Add((CustomFeature)oBinaryFormatter.Deserialize(oFileStream));


                        ((CustomFeature)m_CFLCol[i]).Name = oFileInfo.Name;
                        //*** Set the internal name to name of file if user has externally created a copy

                        if (ArrCFL.ContainsKey(oFileInfo.Name))
                        {
                            ((CustomFeature)m_CFLCol[i]).Visible = ArrCFL[oFileInfo.Name];
                        }
                        else
                        {
                            ((CustomFeature)m_CFLCol[i]).Visible = false;
                        }



                        oFileStream.Close();
                        i += 1;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Adds sub Areas or sub Nationals under specified Area. Hence theme's legend Ranges will alaso be changed as new Datavalues will be added.
        /// </summary>
        /// <param name="parentAreaID"></param>
        /// <param name="areaLayerID"></param>
        public void AddSubAreasByParentAreaID(string parentAreaID, string areaLayerID)
        {
            //Adds sub Areas or sub Nationals under specified Area. 
            //Hence theme's legend Ranges will alaso be changed as new Datavalues will be added.

            DataTable ChildAreasTable = null;
            string ParentAreaNID = null;
            DataTable ParentAreaTable = null;
            Theme NewTheme = null;
            string[] ThemeIDSettings;

            Layer ParentLayer = null;
            try
            {
                if (string.IsNullOrEmpty(parentAreaID) == false && string.IsNullOrEmpty(areaLayerID) == false)
                {
                    ParentLayer = this.m_Layers[areaLayerID];
                    ParentLayer.DrilledDownLayers = new List<string>();

                    //-- get AreaNId for given AreaID
                    ParentAreaTable = this.DIConnection.ExecuteDataTable(this.DIQueries.Area.GetArea(FilterFieldType.ID, "'" + parentAreaID + "'"));

                    if (ParentAreaTable != null && ParentAreaTable.Rows.Count > 0)
                    {
                        ParentAreaNID = DIConnection.GetDelimitedValuesFromDataTable(ParentAreaTable, Area.AreaNId);

                        //-- Get Sub Areas for given parent AreaIDs
                        ChildAreasTable = this.DIConnection.ExecuteDataTable(this.DIQueries.Area.GetArea(FilterFieldType.ParentNId, ParentAreaNID));

                        //-- Merge data of new ChiidAreas into existing Map's DataView.
                        Map.MergeDataOfDrilledDownAreas(DIConnection.GetDelimitedValuesFromDataTable(ChildAreasTable, Area.AreaNId), DIConnection.GetDelimitedValuesFromDataTable(ChildAreasTable, Area.AreaID), this);

                        //////-- Create new UserPreference with new areas but having same IUS, TimePeriod, Source.
                        ////UserPreference2 = (UI.UserPreference.UserPreference)(this._UserPreference.Clone());
                        ////UserPreference2.UserSelection.AreaIds = DIConnection.GetDelimitedValuesFromDataTable(ChildAreasTable, Area.AreaID);
                        ////UserPreference2.UserSelection.AreaNIds = DIConnection.GetDelimitedValuesFromDataTable(ChildAreasTable, Area.AreaNId);

                        //////-- Create new DIDataView with new UserPreference.
                        ////DIDataView2 = new DIDataView(UserPreference2, this.DIConnection, this.DIQueries, Path.Combine(this.UserPreference.General.AdaptationPath, @"Bin\Templates\Metadata\Mask"), string.Empty, string.Empty);
                        ////DIDataView2.GetAllDataByUserSelection();
                        ////MRDTable = DIDataView2.GetMostRecentData(false).Table;

                        ////if (DIDataView2.MainDataTable != null && DIDataView2.MainDataTable.Columns.Count > 0)
                        ////{
                        ////    //-- Merge new DataView with existing Map's DataView
                        ////    this._DIDataView.MainDataTable.Merge(DIDataView2.MainDataTable);

                        ////    //-- Re-assign PresentationData & MRDData
                        ////    this._PresentationData.Table.Merge(DIDataView2.MainDataTable);

                        ////    this.MRDData.Table.Merge(MRDTable);
                        ////}

                        //-- Add Layers associated with ChildAreas
                        foreach (DataRow areaRow in ChildAreasTable.Rows)
                        {
                            //-- Preserve ChildArea's NID + ID.
                            if (this._DrillDownAreaIDs.Contains(areaRow[Area.AreaID].ToString()) == false)
                            {
                                this._DrillDownAreaIDs.Add(areaRow[Area.AreaID].ToString());
                                this._DrillDownAreaNIds.Add(areaRow[Area.AreaNId].ToString());
                            }

                            //-- Get LayerID from areaNId
                            string sSql = this.DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, areaRow[Area.AreaNId].ToString(), FieldSelection.Heavy);
                            DataTable _Dt = this.DIConnection.ExecuteDataTable(sSql);
                            foreach (DataRow _DR in _Dt.Rows)
                            {
                                //-- Extract shape file
                                Map.ExtractShapeFile(_DR, this.SpatialMapFolder);

                                //Checking if same Layer ID already exists in Layers Collection, if yes then skip.. 
                                if (this.m_Layers[_DR[Area_Map_Metadata.LayerName].ToString()] == null)  //"Layer_Name"
                                {
                                    //dtTempStartDate = (System.DateTime)(Information.IsDBNull(_DR[Area_Map_Layer.StartDate]) ? DateTime.Parse(DEFAULT_START_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Year, ociEnUS));
                                    //dtTempEndDate = (System.DateTime)(Information.IsDBNull(_DR[Area_Map_Layer.EndDate]) ? DateTime.Parse(DEFAULT_END_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Year, ociEnUS));

                                    ////Get only those map files whose start date and end date are between Selected TimePeriods / Presentation data time periods
                                    //if (System.DateTime.Compare(dtTempEndDate, dtStartDate) < 0 | System.DateTime.Compare(dtTempStartDate, dtEndDate) > 0)
                                    //{
                                    //    //--- Do nothing
                                    //    Console.Write("");

                                    //}
                                    //else
                                    {
                                        //Add Layer to Layers collection
                                        Layer _Layer = m_Layers.AddSpatialLayer(this.SpatialMapFolder, _DR[Area_Map_Metadata.LayerName].ToString());

                                        if ((_Layer != null)) //if error while adding layer
                                        {
                                            //Set Layer properties
                                            _Layer.Area_Level = int.Parse(_DR[Area.AreaLevel].ToString());
                                            //_Layer.StartDate = dtTempStartDate;
                                            //_Layer.EndDate = dtTempEndDate;

                                            //Set property that point layer exists
                                            if (_Layer.LayerType == ShapeType.Point)
                                                this.m_PointShapeIncluded = true;

                                            //Set visibility off for feature layers by default
                                            switch (_Layer.LayerType)
                                            {
                                                case ShapeType.PointFeature:
                                                case ShapeType.PolygonFeature:
                                                case ShapeType.PolyLineFeature:
                                                    _Layer.Visible = false;
                                                    break;
                                            }

                                            //-- Add Layer's refrence in Parent layer.
                                            _Layer.IsDrilledDown = true;
                                            ParentLayer.DrilledDownLayers.Add(_Layer.ID);
                                        }
                                    }
                                }

                                break;
                            }
                        }

                        //-- Re-create Current theme with same IUS, Source, TimePeriod.
                        for (int c = 0; c < this.Themes.Count; c++)
                        {
                            Theme _Temptheme = this.Themes[c];
                            ThemeIDSettings = _Temptheme.ID.Split('_');     //-- Getting NIds of Indicator, Unit and Subgroupval & ThemeType
                            _Temptheme.ID = "TempID2";

                            _Temptheme = (Theme)(_Temptheme.Clone());

                            NewTheme = this.CreateTheme(ThemeIDSettings[0], ThemeIDSettings[1], ThemeIDSettings[2], (ThemeType)int.Parse(ThemeIDSettings[3]));

                            // Update new Theme with Old Theme's properties
                            //-- Passing false because Legend's ranges will be re constructed from new dataValues .
                            this.UpdateThemeProperties(_Temptheme, NewTheme, false, true);

                            // remove old Theme
                            this.Themes.Remove(this.Themes.ItemIndex(_Temptheme.ID));
                        }

                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// It updates the Existing dataView of Map object with new DataView for Drilled Down AreasNIds specified.
        /// </summary>
        private static void MergeDataOfDrilledDownAreas(string areaNIds, string areaIDs, Map map)
        {
            UI.UserPreference.UserPreference UserPreference2 = null;
            UI.DataViewPage.DIDataView DIDataView2 = null;
            DataTable MRDTable = null;
            string areaIDsWithQuotes = string.Empty;

            if (map != null && string.IsNullOrEmpty(areaNIds) == false && string.IsNullOrEmpty(areaIDs) == false)
            {
                //-- Create new UserPreference with new areas but having same IUS, TimePeriod, Source.
                UserPreference2 = (UI.UserPreference.UserPreference)(map._UserPreference.Clone());
                areaIDsWithQuotes = "'" + areaIDs.Replace(",", "','") + "'";
                UserPreference2.UserSelection.AreaIds = areaIDs;
                UserPreference2.UserSelection.AreaNIds = areaNIds;

                //-- Create new DIDataView with new UserPreference.
                DIDataView2 = new DIDataView(UserPreference2, map.DIConnection, map.DIQueries, Path.Combine(map.UserPreference.General.AdaptationPath, @"Bin\Templates\Metadata\Mask"), string.Empty);
                DIDataView2.GetAllDataByUserSelection();

                if (DIDataView2.MainDataTable != null && DIDataView2.MainDataTable.Columns.Count > 0)
                {
                    MRDTable = DIDataView2.GetMostRecentData(false).Table;

                    //-- Merge new DataView with existing Map's DataView
                    map._DIDataView.MainDataTable.Merge(DIDataView2.MainDataTable);

                    //-- Re-assign PresentationData & MRDData
                    map._PresentationData.Table.Merge(DIDataView2.MainDataTable);

                    map.MRDData.Table.Merge(MRDTable);
                }
            }
        }

        #endregion

        #region " DIB "

        /// <summary>
        /// Add the Disputed International Boundaries Layers into Layer Collection. (Only those layers will be added which have common or intersected boundaries with existing layers.)
        /// </summary>
        /// <param name="dIBFolderPath"></param>
        public void AddDisputedInternationalBoundaries(string dIBFolderPath)
        {
            Shape _Shape;
            GraphicsPath gp = new GraphicsPath();
            IDictionaryEnumerator dicEnumerator;
            Pen CheckPen = new Pen(Brushes.Black);
            Dictionary<string, ShapeInfo> DIBLayersToAdd = new Dictionary<string, ShapeInfo>();

            try
            {
                //- Extract DIB Shape Files form Resources into Specified Folder.
                this.ExtractDIBShapes(dIBFolderPath);

                //- Add DIB layers.
                if (string.IsNullOrEmpty(dIBFolderPath) == false && Directory.Exists(dIBFolderPath))
                {
                    foreach (string dibFilePath in Directory.GetFiles(dIBFolderPath, "*.shp"))
                    {
                        //- Load this layer & get its extent.
                        ShapeInfo DIbLayerInfo = ShapeFileReader.GetShapeInfo(dIBFolderPath, Path.GetFileNameWithoutExtension(dibFilePath));

                        if (DIbLayerInfo != null)
                        {
                            //- Check if this layer Coordinates coincides with any of the polygons of existing base Layers.
                            foreach (Layer Lyr in this.m_Layers)
                            {
                                if (Lyr.Extent.Contains(DIbLayerInfo.Extent) || Lyr.Extent.IntersectsWith(DIbLayerInfo.Extent))
                                {
                                    dicEnumerator = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID).GetEnumerator();

                                    while (dicEnumerator.MoveNext())
                                    {
                                        //Traverse Shapes
                                        _Shape = (Shape)dicEnumerator.Value;
                                        gp.Reset();
                                        for (int i = 0; i <= _Shape.Parts.Count - 1; i++)
                                        {
                                            gp.AddPolygon((PointF[])_Shape.Parts[i]);
                                        }

                                        //gp.CloseAllFigures();

                                        //- Check if any point of DIB Layer's Shape coincides with graphics Path of existing Polygons or not.
                                        foreach (Shape dibShape in DIbLayerInfo.Records.Values)
                                        {
                                            foreach (PointF[] DIBPoints in dibShape.Parts)
                                            {
                                                if (gp.IsOutlineVisible(DIBPoints[0], CheckPen) || gp.IsOutlineVisible(DIBPoints[DIBPoints.Length - 1], CheckPen)
                                                    || gp.IsOutlineVisible(DIBPoints[(int)(DIBPoints.Length / 2)], CheckPen))
                                                {
                                                    //- Add Into Collection
                                                    DIBLayersToAdd.Add(dibFilePath, DIbLayerInfo);
                                                    break;
                                                }

                                                if (DIBLayersToAdd.ContainsKey(dibFilePath))
                                                {
                                                    break;
                                                }
                                            }
                                            if (DIBLayersToAdd.ContainsKey(dibFilePath))
                                            {
                                                break;
                                            }
                                        }

                                        if (DIBLayersToAdd.ContainsKey(dibFilePath))
                                        {
                                            break;
                                        }

                                    }

                                    if (DIBLayersToAdd.ContainsKey(dibFilePath))
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                    }

                    //- Now Add DIBLayers which were found to be intersected with current polygons layers.
                    //- First Add polygon Layers
                    foreach (KeyValuePair<string, ShapeInfo> dibLayerEntry in DIBLayersToAdd)
                    {
                        switch (dibLayerEntry.Value.ShapeType)
                        {
                            case ShapeType.PolygonFeature:
                            case ShapeType.Polygon:
                            case ShapeType.PolygonCustom:
                            case ShapeType.PolygonBuffer:
                                Layer lyr = this.m_Layers.AddShapeFile(dIBFolderPath, Path.GetFileNameWithoutExtension(dibLayerEntry.Key));
                                lyr.IsDIB = true;

                                break;
                        }
                    }

                    //- Now Add polyline, Point Layers
                    foreach (KeyValuePair<string, ShapeInfo> dibLayerEntry in DIBLayersToAdd)
                    {
                        switch (dibLayerEntry.Value.ShapeType)
                        {
                            case ShapeType.PointFeature:
                            case ShapeType.Point:
                            case ShapeType.PointCustom:
                            case ShapeType.PolyLineCustom:
                            case ShapeType.PolyLineFeature:
                            case ShapeType.PolyLine:
                                Layer lyr = this.m_Layers.AddShapeFile(dIBFolderPath, Path.GetFileNameWithoutExtension(dibLayerEntry.Key));
                                lyr.IsDIB = true;
                                break;
                        }
                    }

                    //- Apply DIB Layers Settings
                    this.ApplyDIBSettings(Path.Combine(dIBFolderPath, DIBSettings.DIBSettingFileName));
                }
            }
            catch
            {

            }
            finally
            {
                CheckPen.Dispose();
                gp.Dispose();
            }
        }

        private void ExtractDIBShapes(string dIBFolderPath)
        {
            byte[] ShapeBytes = null;
            FileStream stream = null;
            string ShapeFileName = string.Empty;
            string OutputFilePath = string.Empty;

            //- Extracts DIB Shape Files from resources into specified Folder.
            try
            {
                System.Resources.ResourceSet rs = DIBShapes.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);

                IDictionaryEnumerator dicEnumerator = rs.GetEnumerator();

                while (dicEnumerator.MoveNext())
                {
                    ShapeFileName = Convert.ToString(dicEnumerator.Key);
                    if (ShapeFileName.EndsWith("_dbf") || ShapeFileName.EndsWith("_shp") || ShapeFileName.EndsWith("_shx"))
                    {
                        if (ShapeFileName.EndsWith("_dbf"))
                        {
                            ShapeFileName = ShapeFileName.Substring(0, ShapeFileName.Length - 4) + ".dbf";
                        }
                        if (ShapeFileName.EndsWith("_shp"))
                        {
                            ShapeFileName = ShapeFileName.Substring(0, ShapeFileName.Length - 4) + ".shp";
                        }
                        if (ShapeFileName.EndsWith("_shx"))
                        {
                            ShapeFileName = ShapeFileName.Substring(0, ShapeFileName.Length - 4) + ".shx";
                        }

                        OutputFilePath = Path.Combine(dIBFolderPath, ShapeFileName);

                        if (File.Exists(OutputFilePath) == false && dicEnumerator.Value != null)
                        {
                            ShapeBytes = (byte[])(dicEnumerator.Value);

                            //- Write Shape Bytes to a file.
                            stream = new FileStream(OutputFilePath, FileMode.Create);
                            stream.Write(ShapeBytes, 0, ShapeBytes.Length);
                            stream.Close();
                            stream = null;
                        }
                    }
                }

                //- Now Extract DIBCurrent.Xml & DIBDefault.xml
                OutputFilePath = Path.Combine(dIBFolderPath, DIBSettings.DIBDefaultFileName);
                if (File.Exists(OutputFilePath) == false)
                {
                    ShapeBytes = (byte[])(System.Text.Encoding.Default.GetBytes(DIBShapes.DIBDefault_xml));

                    //- Write Shape Bytes to a file.
                    stream = new FileStream(OutputFilePath, FileMode.Create);
                    stream.Write(ShapeBytes, 0, ShapeBytes.Length);
                    stream.Close();
                    stream = null;
                }

                OutputFilePath = Path.Combine(dIBFolderPath, DIBSettings.DIBSettingFileName);
                if (File.Exists(OutputFilePath) == false)
                {
                    ShapeBytes = (byte[])(System.Text.Encoding.Default.GetBytes(DIBShapes.DIBDefault_xml));

                    //- Write Shape Bytes to a file.
                    stream = new FileStream(OutputFilePath, FileMode.Create);
                    stream.Write(ShapeBytes, 0, ShapeBytes.Length);
                    stream.Close();
                    stream = null;
                }
            }
            catch
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }
        }

        /// <summary>
        /// Returns true, if Map contains Disputed Boundaries Layers in it.
        /// </summary>
        /// <returns></returns>
        public bool ContainsDIBLayers()
        {
            bool RetVal = false;

            if (this.m_Layers != null)
            {
                foreach (Layer Lyr in this.m_Layers)
                {
                    if (Lyr.IsDIB)
                    {
                        RetVal = true;
                        break;
                    }
                }
            }

            return RetVal;
        }

        public void ApplyDIBSettings(string DIBSettingsFilePath)
        {
            try
            {
                if (File.Exists(DIBSettingsFilePath))
                {
                    DIBSettings DIBSettingsObject = DIBSettings.LoadDIBSettings(DIBSettingsFilePath);

                    foreach (Layer Lyr in this.m_Layers)
                    {
                        if (Lyr.IsDIB)
                        {
                            //- Apply Layer Settings
                            this.ApplyDIBLayerSetting(Lyr, DIBSettingsObject);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveDIBSettings(string DIBSettingsFilePath)
        {
            DIBSettings DIBSettingsObject = null;
            DIBLayerSetting LayerSetting = null;
            try
            {
                if (File.Exists(DIBSettingsFilePath))
                {
                    //- Load Previous DIB Settings.
                    DIBSettingsObject = DIBSettings.LoadDIBSettings(DIBSettingsFilePath);
                }
                else
                {
                    //- create new Object
                    DIBSettingsObject = new DIBSettings();
                }

                //- Update new DIB Settings from DIB layers
                foreach (Layer Lyr in this.m_Layers)
                {
                    if (Lyr.IsDIB)
                    {
                        LayerSetting = DIBSettingsObject.GetDIBLayerSetting(Lyr.LayerName);

                        //- If Previous DIBSettings DO NOT contains settings for this Layer
                        if (LayerSetting == null)
                        {
                            //- Add Settings for this Layer.
                            LayerSetting = new DIBLayerSetting();
                            DIBSettingsObject.DIBLayersSetting.Add(LayerSetting);
                        }

                        LayerSetting.LayerName = Lyr.LayerName;
                        LayerSetting.FillColor = Lyr.FillColor;
                        LayerSetting.FillStyle = Lyr.FillStyle;
                        LayerSetting.BorderColor = Lyr.BorderColor;
                        LayerSetting.BorderStyle = Lyr.BorderStyle;
                        LayerSetting.BorderWidth = Lyr.BorderSize;
                    }
                }

                //- Save XML settings
                DIBSettingsObject.Save(DIBSettingsFilePath);
            }
            catch
            {
            }
        }

        public void ApplyDIBLayerSetting(Layer dibLayer, string DIBSettingsFileName)
        {
            try
            {
                if (File.Exists(DIBSettingsFileName))
                {
                    DIBSettings DIBSettingsObject = DIBSettings.LoadDIBSettings(DIBSettingsFileName);

                    this.ApplyDIBLayerSetting(dibLayer, DIBSettingsObject);
                }
            }
            catch
            {
            }
        }

        private void ApplyDIBLayerSetting(Layer dibLayer, DIBSettings DIBSettingsObject)
        {
            try
            {
                if (dibLayer != null && DIBSettingsObject != null)
                {
                    DIBLayerSetting setting = DIBSettingsObject.GetDIBLayerSetting(dibLayer.LayerName);

                    if (setting != null)
                    {
                        dibLayer.FillColor = setting.FillColor;
                        dibLayer.FillStyle = setting.FillStyle;
                        dibLayer.BorderColor = setting.BorderColor;
                        dibLayer.BorderStyle = setting.BorderStyle;
                        dibLayer.BorderSize = setting.BorderWidth;
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region " Theme"


        /// <summary>
        /// Set the layer visibility based on time period
        /// </summary>
        /// <param name="p_Theme"></param>
        /// <param name="p_ThemeData"></param>
        private void GenerateLayerInformation(ref Theme p_Theme, ref DataView p_ThemeData)
        {

            foreach (DataRowView _DRV in p_ThemeData)
            {
                Layer[] TimeLayers = GetTimeLayer(_DRV["TimePeriod"].ToString());
                if (TimeLayers != null)
                {
                    foreach (Layer _Layer in TimeLayers)
                    {
                        {
                            if (!p_Theme.LayerVisibility.ContainsKey(_Layer.ID))
                            {
                                switch (_Layer.LayerType)
                                {
                                    case ShapeType.Point:
                                    case ShapeType.Polygon:
                                    case ShapeType.PolyLine:
                                        p_Theme.LayerVisibility.Add(_Layer.ID, true);
                                        break;
                                    case ShapeType.PointFeature:
                                    case ShapeType.PolygonFeature:
                                    case ShapeType.PolyLineFeature:
                                        p_Theme.LayerVisibility.Add(_Layer.ID, false);
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            foreach (Layer _Layer in m_Layers)
            {
                if (!p_Theme.LayerVisibility.ContainsKey(_Layer.ID))
                {
                    p_Theme.LayerVisibility.Add(_Layer.ID, false);
                }
            }

        }

        private Layer[] GetTimeLayer(string p_Timeperiod)
        {
            //-- handle different regional setting - Thai,arabic calendar, date formats, seperator etc
            CultureInfo ociEnUS = new CultureInfo("en-US", false);
            DateTime dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
            DateTime dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;
            TimePeriodFacade.SetStartDateEndDate(p_Timeperiod, ref dtStartDate, ref dtEndDate);
            return Layers[dtStartDate];
        }


        public Theme GetTheme(int p_Index)
        {
            return m_Themes[(int)p_Index];
        }


        public Theme CreateTheme(string p_IndicatorNId, string p_UnitNId, string p_Subgroup_NId, ThemeType p_ThemeType)
        {
            int p_Position = -1;
            return CreateTheme(p_IndicatorNId, p_UnitNId, p_Subgroup_NId, p_ThemeType, p_Position);
        }

        public Theme CreateTheme(string p_IndicatorNId, string p_UnitNId, string p_SubgroupValNId, ThemeType p_ThemeType, int p_Position)
        {
            //-- by default, create Bar chart with multiple Subgroups
            return this.CreateTheme(p_IndicatorNId, p_UnitNId, p_SubgroupValNId, p_ThemeType, ChartType.Column, ChartSeriesType.Subgroup, p_Position);
        }

        public Theme CreateTheme(string p_IndicatorNId, string p_UnitNId, string p_SubgroupValNId, ThemeType p_ThemeType, ChartType chartType, ChartSeriesType chartGroupBy, int p_Position)
        {
            //*** Create New Theme. 
            Theme RetVal = null;
            string sSubgroupValNId = string.Empty;
            string sRowFilter = string.Empty;
            string DefaultIndicatorNId = string.Empty;
            string DefaultUnitNId = string.Empty;


            foreach (Theme _TempTheme in m_Themes)
            {
                if (_TempTheme.ID == p_IndicatorNId + "_" + p_UnitNId + "_" + p_SubgroupValNId + "_" + ((int)p_ThemeType).ToString())
                {
                    //If theme already exists exception will be thrown
                    throw new ApplicationException("3"); //TODO return null or some other approach
                }
            }


            Theme _Theme = new Theme();
            DataView _ThemeData;

            //-- If indicatorNID and UnitNId are -1 ,then Get default indicatorNId , UnitNIds from PresentationData's first row.
            //-- It is required because, for creating any theme (Chart or color) first time, we need some indicator, unit NIDs.
            if (p_IndicatorNId == "-1")
            {
                DefaultIndicatorNId = this.MRDData[0][Indicator.IndicatorNId].ToString();
                sRowFilter += Indicator.IndicatorNId + " IN (" + DefaultIndicatorNId + ")";
            }
            else
            {
                sRowFilter += Indicator.IndicatorNId + " IN (" + p_IndicatorNId + ")";
                DefaultIndicatorNId = p_IndicatorNId;
            }

            if (p_UnitNId == "-1")
            {
                DefaultUnitNId = this.MRDData[0][Unit.UnitNId].ToString();
                sRowFilter += " AND " + Unit.UnitNId + " IN (" + DefaultUnitNId + ")";
            }
            else
            {
                sRowFilter += " AND " + Unit.UnitNId + " IN (" + p_UnitNId + ")";
                DefaultUnitNId = p_UnitNId;
            }

            //DataTable SubGroupDataTable = DIConnection.ExecuteDataTable(DIQueries.IUS.GetSubgroupValByIU(Convert.ToInt32(DefaultIndicatorNId), Convert.ToInt32(DefaultUnitNId)));

            if (p_ThemeType == ThemeType.Chart)
            {
                string sSql = string.Empty;
                //foreach (DataRow dr in SubGroupDataTable.Rows)
                foreach (DataRow dr in this.GetSubgroupsNIDsBy_IndicatorUnit(DefaultIndicatorNId, DefaultUnitNId).Rows)
                {
                    if (sSubgroupValNId.Length > 0)
                    {
                        sSubgroupValNId += ",";
                    }
                    sSubgroupValNId += dr[SubgroupVals.SubgroupValNId].ToString();
                }
                if (!string.IsNullOrEmpty(sSubgroupValNId))
                {
                    //Get all records for Selected Indicator, Subgroups and Unit
                    sRowFilter = Indicator.IndicatorNId + " IN (" + DefaultIndicatorNId + ") AND " + Unit.UnitNId + " IN (" + DefaultUnitNId + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + sSubgroupValNId + ")";
                }
                else
                {
                    sRowFilter = Indicator.IndicatorNId + " IN (" + DefaultIndicatorNId + ") AND " + Unit.UnitNId + " IN (" + DefaultUnitNId + ")";
                }

                //////-- If Pie chart, then TimeSeries shud be OFF. Use MRD data
                ////if (chartType == ChartType.Pie)
                ////{
                ////    this.MRDData.RowFilter = sRowFilter;
                ////    _ThemeData = new DataView(this.MRDData.ToTable());
                ////    this.MRDData.RowFilter = "";
                ////}
                ////else
                ////{
                ////    this.PresentationData.RowFilter = sRowFilter;
                ////    _ThemeData = new DataView(this.PresentationData.ToTable());
                ////    this.PresentationData.RowFilter = "";
                ////}

                this.PresentationData.RowFilter = sRowFilter;
                _ThemeData = new DataView(this.PresentationData.ToTable());
                this.PresentationData.RowFilter = "";
            }
            else
            {
                //_ThemeData = m_QueryBase.Map_GetData(int.Parse(p_IndicatorNId), int.Parse(p_UnitNId), int.Parse(p_Subgroup_NId), 0, -1);
                //_ThemeData = this.MRDData;

                if (p_SubgroupValNId == "-1")
                {
                    //-- Set Subgroup filter only for new theme case.
                    //-- In case of Subgroup = Select all case discard subgroup filter
                    if (p_IndicatorNId == "-1" && p_UnitNId == "-1" && p_SubgroupValNId == "-1")
                    {
                        sRowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + MRDData[0][SubgroupVals.SubgroupValNId] + ")";
                    }
                    else
                    {
                        //-- SubgroupNid = -1 and IndicatorNid <> -1 , indicates that Subgroup = "Select ALL" case.
                        //-- then set bool variable of Theme.SubgroupSelectAll = true
                        _Theme.SubgroupSelectAll = true;
                    }
                }
                else
                {
                    sRowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + p_SubgroupValNId + ")";
                }
                this.MRDData.RowFilter = sRowFilter;
                _ThemeData = new DataView(this.MRDData.ToTable());
                this.MRDData.RowFilter = "";

            }

            //--Ckeck if Theme Data Has some record. Exit function if NO record Found.
            if (_ThemeData.Count > 0)
            {

                //*** Add Metadata Columns
                int i;

                if (_ThemeData.Count == 0)
                {
                    throw new ApplicationException("5");
                }

                //*** Update Metadata Column values if metadata column exists
                //UpdateMetadataInfo(ref _ThemeData);


                //GenerateLayerInformation(_Theme, _ThemeData)

                foreach (Layer _Layer in m_Layers)
                {
                    {
                        if (!_Theme.LayerVisibility.ContainsKey(_Layer.ID))
                        {
                            switch (_Layer.LayerType)
                            {
                                case ShapeType.Point:
                                case ShapeType.Polygon:
                                case ShapeType.PolyLine:
                                    _Theme.LayerVisibility.Add(_Layer.ID, true);
                                    break;
                                case ShapeType.PointFeature:
                                case ShapeType.PolygonFeature:
                                case ShapeType.PolyLineFeature:
                                    _Theme.LayerVisibility.Add(_Layer.ID, false);
                                    break;
                            }
                        }
                    }
                }
                // _Layer = null;



                //object[] oMDKeys = new object[MDKeys.Keys.Count];
                //MDKeys.Keys.CopyTo(oMDKeys, 0);
                object[] oMDKeys = this.MDColumns;
                _Theme.MetaDataKeys = oMDKeys;
                _Theme.Type = p_ThemeType;

                //_Theme.LegendTitle = _ThemeData[0][Unit.UnitName].ToString();;
                _Theme.SetLegendTitle(_ThemeData[0][Indicator.IndicatorName].ToString(), _ThemeData[0][Unit.UnitName].ToString(), _ThemeData[0][SubgroupVals.SubgroupVal].ToString());

                //*** Bugfix 26 Apr 2006 For the second map theme, the legend doesn’t automatically append a title…
                _Theme.StartColor = m_FirstColor;
                _Theme.EndColor = m_FourthColor;
                if ((m_Layers.RecordCounts() == 0) || m_Layers.RecordCounts() == 0)
                {
                    _Theme.ShapeCount = m_Layers.RecordCounts(m_SpatialMapFolder);
                }
                else
                {
                    _Theme.ShapeCount = m_Layers.RecordCounts();
                }

                _Theme.MissingColor = this._MissingColor;

                switch (_Theme.Type)
                {
                    case ThemeType.Color:
                        _Theme.SetRange(_ThemeData);
                        _Theme.Legends[0].Color = FirstColor;
                        _Theme.Legends[1].Color = SecondColor;
                        _Theme.Legends[2].Color = ThirdColor;
                        _Theme.Legends[3].Color = FourthColor;
                        _Theme.Legends[_Theme.Legends.Count - 1].Caption = m_MissingValue;
                        _Theme.BreakType = BreakType.Continuous;
                        //$$$ By Default create the theme on the basis of equal count and set it as continuous
                        break;
                    case ThemeType.Hatch:
                        _Theme.SetRange(_ThemeData);
                        _Theme.Legends[0].Color = Color.LightGray;
                        _Theme.Legends[1].Color = Color.LightGray;
                        _Theme.Legends[2].Color = Color.LightGray;
                        _Theme.Legends[3].Color = Color.LightGray;
                        _Theme.Legends[_Theme.Legends.Count - 1].Caption = m_MissingValue;
                        _Theme.BreakType = BreakType.Continuous;
                        //$$$ By Default create the theme on the basis of equal count and set it as continuous
                        break;
                    case ThemeType.Symbol:
                        _Theme.SetRange(_ThemeData);
                        for (i = 0; i <= _Theme.Legends.Count - 1; i++)
                        {
                            _Theme.Legends[i].MarkerType = MarkerStyle.Custom;
                            _Theme.Legends[i].MarkerChar = Strings.Chr(110);            //-- (65 + i)  - previous used
                            if (i <= 3)
                            {
                                //-- Default color is red for all with 50% transparency
                                _Theme.Legends[i].Color = Color.FromArgb(128, 255, 0, 0);
                                _Theme.Legends[i].MarkerFont = new Font("Webdings", 10 + i * 5);
                                _Theme.Legends[i].MarkerSize = 10 + i * 5;
                            }
                            else
                            {
                                //-- Missing legend info
                                _Theme.Legends[i].Color = Color.FromArgb(128, _Theme.MissingColor);
                                _Theme.Legends[i].MarkerFont = new Font("Webdings", 10);
                                _Theme.Legends[i].MarkerSize = 10;
                            }
                        }

                        _Theme.Legends[_Theme.Legends.Count - 1].Caption = m_MissingValue;
                        _Theme.BreakType = BreakType.Continuous;
                        //$$$ By Default create the theme on the basis of equal count and set it as continuous
                        break;
                    case ThemeType.Label:
                        _Theme.SetRange(_ThemeData);
                        Font fnt = new Font("Arial", 8);
                        for (i = 0; i <= _Theme.Legends.Count - 1; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    _Theme.Legends[i].Color = Color.FromArgb(255 - FirstColor.R, 255 - FirstColor.G, 255 - FirstColor.B);
                                    break;
                                case 1:
                                    _Theme.Legends[i].Color = Color.FromArgb(255 - SecondColor.R, 255 - SecondColor.G, 255 - SecondColor.B);
                                    break;
                                case 2:
                                    _Theme.Legends[i].Color = Color.FromArgb(255 - ThirdColor.R, 255 - ThirdColor.G, 255 - ThirdColor.B);
                                    break;
                                case 3:
                                    _Theme.Legends[i].Color = Color.FromArgb(255 - FourthColor.R, 255 - FourthColor.G, 255 - FourthColor.B);
                                    break;
                                default:    //Missing Data
                                    _Theme.Legends[i].Color = Color.LightYellow;
                                    break;
                            }
                            _Theme.Legends[i].MarkerFont = fnt;
                        }

                        _Theme.Legends[_Theme.Legends.Count - 1].Caption = m_MissingValue;
                        _Theme.BreakType = BreakType.Continuous;
                        //$$$ By Default create the theme on the basis of equal count and set it as continuous
                        break;
                    case ThemeType.DotDensity:
                        _Theme.DotSize = 3;
                        _Theme.SetThemeId(ref _ThemeData);
                        CalcDotDensity(_ThemeData, ref _Theme);
                        break;
                    case ThemeType.Chart:

                        //-- Chart dataValue are plotted against multiple SubgroupVal OR Sources.
                        _Theme.ChartSeriestype = chartGroupBy;
                        _Theme.ChartType = chartType;

                        //-- Get SubgroupValNIDs which are available in ThemeData
                        sSubgroupValNId = string.Empty;
                        // foreach (DataRow drow in SubGroupDataTable.Rows)
                        foreach (DataRow drow in _ThemeData.ToTable(true, SubgroupVals.SubgroupValNId).Rows)
                        {
                            if (sSubgroupValNId.Length == 0)
                            {
                                sSubgroupValNId = drow[SubgroupVals.SubgroupValNId].ToString();
                            }
                            else
                            {
                                sSubgroupValNId += "," + drow[SubgroupVals.SubgroupValNId].ToString();
                            }
                        }
                        string[] _Inds = Strings.Split(DefaultIndicatorNId, ",", -1, CompareMethod.Text);
                        string[] _SPs = Strings.Split(sSubgroupValNId, ",", -1, CompareMethod.Text);


                        //*** Bugfix 15 June 2006 Improper Max data value for Chart theme type
                        //float MinVal = 0;
                        //float MaxVal = 0;
                        //GetMinMaxDataValue(ref _ThemeData, ref MinVal, ref MaxVal);
                        //_Theme.Minimum = MinVal;
                        //_Theme.Maximum = MaxVal;

                        _ThemeData.Sort = DataExpressionColumns.DataType + " ASC," + DataExpressionColumns.NumericData + " ASC";
                        _Theme.Minimum = (decimal)_ThemeData[0][DataExpressionColumns.NumericData];
                        _Theme.Maximum = (decimal)_ThemeData[_ThemeData.Count - 1][DataExpressionColumns.NumericData];


                        if (chartGroupBy == ChartSeriesType.Subgroup)
                        {
                            //-- ChartGroupBy will be Subgroup by default.
                            MakeChartTheme(_Inds, _SPs, _ThemeData, _Theme);
                        }

                        if (_Theme.ChartType != ChartType.Line)
                        {
                            _Theme.DisplayChartMRD = true;      //-- default
                        }

                        //color array
                        string[] _IndColor = new string[_Inds.Length];
                        VBMath.Randomize();
                        for (i = 0; i <= _Inds.Length - 1; i++)
                        {
                            //Initially it was VBMath.rnd() function using for getting random no.
                            _IndColor[i] = Color.FromArgb((int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255)).Name;
                        }

                        string _INDName = "";
                        string _INDNid = "";
                        string _SPName = "";
                        string _SPNId = "";


                        string sSql = DIQueries.IUS.GetIUSNIdByI_U_S(DefaultIndicatorNId, "", "");
                        System.Data.IDataReader dr = DIConnection.ExecuteReader(sSql);
                        string sIUSNIds = string.Empty;
                        while (dr.Read())
                        {
                            if (sIUSNIds.Length > 0)
                                sIUSNIds += ",";
                            sIUSNIds += dr[Indicator_Unit_Subgroup.IUSNId].ToString();
                        }
                        dr.Close();
                        dr.Dispose();

                        sSql = DIQueries.IUS.GetIUS(FilterFieldType.NId, sIUSNIds, FieldSelection.Light);
                        DataView _Dv = DIConnection.ExecuteDataTable(sSql).DefaultView;

                        //DataView _Dv = m_QueryBase.Indicator_GetIUSSelections(p_IndicatorNId, sSubgroup_NId, -1);   //Default p_UnitNId = -1 is passed as defined in function signaure.
                        string[] SubGroupArr;
                        SubGroupArr = sSubgroupValNId.Split(',');
                        for (i = 0; i <= SubGroupArr.Length - 1; i++)
                        {
                            _Dv.RowFilter = "Subgroup_Val_NId = " + SubGroupArr[i];
                            if (_INDNid.IndexOf("{~}" + _Dv[0]["Indicator_NId"].ToString() + "@") == -1)
                            {
                                if (_INDName.Length > 0)
                                    _INDName += "{~}";
                                _INDNid += "{~}";
                                _INDName += _Dv[0]["Indicator_Name"].ToString();
                                _INDNid += _Dv[0]["Indicator_NId"].ToString() + "@";

                                _Theme.UnitName = _Dv[0]["Unit_Name"].ToString();

                            }

                            if (_SPNId.IndexOf("{~}" + _Dv[0]["Subgroup_Val_NId"].ToString() + "@") == -1)
                            {
                                if (_SPName.Length > 0)
                                    _SPName += "{~}";
                                _SPNId += "{~}";
                                _SPName += _Dv[0]["Subgroup_Val"].ToString();
                                _SPNId += _Dv[0]["Subgroup_Val_NId"].ToString() + "@";
                            }
                        }
                        _INDNid = _INDNid.Replace("@", "");
                        _SPNId = _SPNId.Replace("@", "");
                        _Theme.IndicatorName = Strings.Split(_INDName, "{~}", -1, CompareMethod.Text);
                        _Theme.IndicatorNId = Strings.Split(_INDNid, "{~}", -1, CompareMethod.Text);
                        _Theme.SubgroupName = Strings.Split(_SPName, "{~}", -1, CompareMethod.Text);
                        _Theme.SubgroupNId = Strings.Split(_SPNId, "{~}", -1, CompareMethod.Text);
                        _Theme.UnitNId = int.Parse(DefaultUnitNId);     //p_unitNid

                        _Theme.IndicatorColor = _IndColor;

                        p_IndicatorNId = DefaultIndicatorNId;
                        p_UnitNId = DefaultUnitNId;

                        string[] _SPFill = new string[_Theme.SubgroupName.Length];
                        string[] _SPVisible = new string[_Theme.SubgroupName.Length];
                        for (i = 0; i <= _Theme.SubgroupName.Length - 1; i++)
                        {
                            //Initiallly it was VBMath.rnd() function using for getting random no.
                            //-- Default transparency is 60% i.e 153 value.
                            _SPFill[i] = Color.FromArgb(153, (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255), (int)(_random.NextDouble() * 255)).ToArgb().ToString();
                            _SPVisible[i] = "1";
                        }

                        _Theme.SubgroupFillStyle = _SPFill;
                        _Theme.SubgroupVisible = _SPVisible;

                        //--If ChartGroupBy = source, then update Chart accordingly
                        if (chartGroupBy == ChartSeriesType.Source)
                        {
                            //-- Take default subgroupNId 
                            string SubgroupNIdForSource = p_SubgroupValNId;

                            if (SubgroupNIdForSource == "-1")
                            {
                                //-- Take first subgroup in collection
                                SubgroupNIdForSource = _Theme.SubgroupNId[0];
                            }
                            this.UpdateChartForMultipleSource(SubgroupNIdForSource, ref _Theme);
                        }
                        break;
                }

                if (p_IndicatorNId == "-1" & p_UnitNId == "-1" & p_SubgroupValNId == "-1")
                {
                }
                else
                {
                    if (p_UnitNId == "-1")
                    {
                        _Theme.UnitNId = -1;
                    }

                    if (p_SubgroupValNId == "-1" && _Theme.SubgroupNId.Length > 0 && _Theme.SubgroupNId[0] == "-1")
                    {
                        string[] SNID = new string[1];
                        SNID[0] = "-1";
                        _Theme.SubgroupNId = SNID;
                    }
                    _Theme.ID = p_IndicatorNId + "_" + p_UnitNId + "_" + p_SubgroupValNId + "_" + (int)p_ThemeType;
                    //$$$ Convention for Theme Id -> I_U_S_ThemeType
                }

                //-- Save GIDs of Indicator, Unit, Subgroups used in theme creation.
                _Theme.I_U_S_GIDs = this.GetI_U_S_GIDsByNids(this.DIDataView.MainDataTable.DefaultView, DefaultIndicatorNId, DefaultUnitNId, _Theme.SubgroupNId[0]);

                if (p_Position == -1)
                {
                    m_Themes.Add(_Theme);
                }
                else
                {
                    m_Themes.Insert(p_Position, _Theme);
                }
            }

            //- Apply Default setting of Map.TemplateStyle
            if (this.TemplateStyle != null)
            {
                _Theme.LegendFont = new Font(this.TemplateStyle.LegendTitle.FontTemplate.FontName, this.TemplateStyle.LegendTitle.FontTemplate.FontSize, this.TemplateStyle.LegendTitle.FontTemplate.FontStyle);
                _Theme.LegendColor = this.TemplateStyle.LegendTitle.FontTemplate.ForeColor;

                _Theme.LegendBodyFont = new Font(this.TemplateStyle.Legends.FontTemplate.FontName, this.TemplateStyle.Legends.FontTemplate.FontSize, this.TemplateStyle.Legends.FontTemplate.FontStyle);
                _Theme.LegendBodyColor = this.TemplateStyle.Legends.FontTemplate.ForeColor;
            }

            RetVal = _Theme;


            return RetVal;
        }

        /// <summary>
        /// It Updates specified Theme with new Indicator, Units, Subgroups. ThemeType will be same for new Theme.
        /// </summary>
        /// <param name="OldTheme">Old theme to update.</param>
        /// <param name="newIndicatorNid">new Indicator NID for new Theme</param>
        /// <param name="newUnitNid">new UNit NID for new Theme</param>
        /// <param name="subgroupValNid">new Subgroup NID for new Theme</param>
        /// <returns>true, if updated success.</returns>
        public bool UpdateTheme(Theme OldTheme, string newIndicatorNid, string newUnitNid, string newSubgroupValNid)
        {
            bool RetVal = false;
            Theme NewTheme = null;
            Theme OldThemeClone = null;

            try
            {
                if ((OldTheme != null) && string.IsNullOrEmpty(newIndicatorNid) == false && string.IsNullOrEmpty(newUnitNid) == false
                    && string.IsNullOrEmpty(newSubgroupValNid) == false)
                {
                    OldTheme.ID = "TempThemeID";

                    OldThemeClone = (Theme)(OldTheme.Clone());

                    //'- Adding new Theme.
                    NewTheme = this.CreateTheme(newIndicatorNid, newUnitNid, newSubgroupValNid, OldTheme.Type);

                    if ((NewTheme == null) == false)
                    {
                        //-- Update new Theme's properties with Old theme.
                        //-- Passing false because Legend's ranges and decimals will be re constructed from new dataValues fo new IUS
                        this.UpdateThemeProperties(OldThemeClone, NewTheme, false, false);

                        //-- Remove old theme from Collection.
                        this.Themes.Remove(this.Themes.ItemIndex(OldTheme.ID));
                    }

                    //-- update Map Title and Subtitle
                    this.SetMapTitle();

                    RetVal = true;
                }

            }
            catch (Exception)
            {

            }
            return RetVal;
        }

        /// <summary>
        /// It compares two themes and updates properties of oldTheme into NewTheme.
        /// <para>Following Theme properties are updated: BreakCount(LegendCount), Layer's Visibility, StartColor, EndColor, Theme.Visible, Legend's Color, Decimal, BorderColor, BorderStyle, BorderWidth, DotColor, DotFont, DotSize, LabelColor, LabelFont, LabelVisible.
        /// </para>
        /// </summary>
        /// <param name="oldTheme">Old Theme</param>
        /// <param name="newTheme">New Theme to retain properties of old theme.</param>
        /// <param name="preserveRoundingDecimals">true, if oldtheme's rounding Decmial to be preserved in new theme as well(applicable to Break based Themes)</param>
        private void UpdateThemeProperties(Theme oldTheme, Theme newTheme, bool preserveLegendsRanges, bool preserveRoundingDecimals)
        {
            IDictionaryEnumerator dicEnumerator = null;

            try
            {
                if (newTheme != null)
                {
                    // set Theme's common properties
                    foreach (object key in oldTheme.LayerVisibility.Keys)
                    {
                        if (newTheme.LayerVisibility.ContainsKey(key))
                        {
                            newTheme.LayerVisibility[key] = (bool)oldTheme.LayerVisibility[key];
                        }
                    }

                    //--  Set Theme Name
                    if (string.IsNullOrEmpty(newTheme.Name))
                    {
                        if (newTheme.IndicatorName[0] == oldTheme.IndicatorName[0])
                        {
                            newTheme.Name = oldTheme.Name;
                        }
                        else
                        {
                            if (newTheme.Type == ThemeType.Chart)
                            {
                                newTheme.Name = newTheme.IndicatorName[0] + " - " + newTheme.UnitName[0] + " - " + newTheme.SubgroupName[0];
                            }
                            else
                            {
                                newTheme.Name = newTheme.IndicatorName[0] + " - " + newTheme.UnitName[0];
                            }
                        }
                    }

                    newTheme.Visible = oldTheme.Visible;

                    newTheme.BorderColor = oldTheme.BorderColor;
                    newTheme.BorderStyle = oldTheme.BorderStyle;
                    newTheme.BorderWidth = oldTheme.BorderWidth;
                    newTheme.LabelColor = oldTheme.LabelColor;
                    newTheme.LabelFont = oldTheme.LabelFont;
                    newTheme.LabelVisible = oldTheme.LabelVisible;
                    newTheme.LabelField = oldTheme.LabelField;
                    newTheme.LabelMultirow = oldTheme.LabelMultirow;
                    newTheme.LabelIndented = oldTheme.LabelIndented;

                    //-- Set each Layer's Label visible properties
                    foreach (Layer layer in this.Layers)
                    {
                        if (oldTheme.LabelVisible)
                        {
                            layer.LabelVisible = oldTheme.LabelVisible;
                        }

                        //- If new Layers are added in collection (i.e case of Area Replication)
                        // then apply preserved Theme's Label Settings
                        if (oldTheme.LayerVisibility.ContainsKey(layer.ID) == false)
                        {
                            layer.LabelField = oldTheme.LabelField;
                            layer.LabelMultirow = oldTheme.LabelMultirow;
                            layer.LabelIndented = oldTheme.LabelIndented;

                            //////** Label Font
                            layer.LabelFont = oldTheme.LabelFont;
                            layer.LabelColor = oldTheme.LabelColor;
                        }
                    }

                    //--Layer's Visibility
                    dicEnumerator = oldTheme.LayerVisibility.GetEnumerator();
                    while (dicEnumerator.MoveNext())
                    {
                        //Theme.LayerVisibility[Key, Value]
                        //Key: LayerID
                        //Value: bool                    
                        if (newTheme.LayerVisibility.ContainsKey(dicEnumerator.Key))
                        {
                            newTheme.LayerVisibility[dicEnumerator.Key] = oldTheme.LayerVisibility[dicEnumerator.Key];
                        }
                    }

                    //-- Legend Settings
                    newTheme.StartColor = oldTheme.StartColor;
                    newTheme.EndColor = oldTheme.EndColor;
                    newTheme.BreakCount = oldTheme.BreakCount;
                    newTheme.BreakType = oldTheme.BreakType;
                    if (preserveRoundingDecimals)
                    {
                        newTheme.Decimals = oldTheme.Decimals;
                    }

                    newTheme.LegendBodyColor = oldTheme.LegendBodyColor;
                    newTheme.LegendBodyFont = oldTheme.LegendBodyFont;
                    newTheme.LegendColor = oldTheme.LegendColor;
                    newTheme.LegendFont = oldTheme.LegendFont;

                    newTheme.MultiLegendCriteria = oldTheme.MultiLegendCriteria;
                    newTheme.MultiLegend = oldTheme.MultiLegend;

                    if (newTheme.Type != ThemeType.DotDensity && newTheme.Type != ThemeType.Chart)
                    {
                        newTheme.Legends.Clear();

                        this.setThemeRange(this.Themes.ItemIndex(newTheme.ID));

                        //- Preserve OldTheme's Minimum & Maximum values and Legend Ranges, Caption, Title etc.
                        if (preserveLegendsRanges)
                        {
                            newTheme.Maximum = oldTheme.Maximum;
                            newTheme.Minimum = oldTheme.Minimum;
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Title = oldTheme.Legends[i].Title;
                                    newTheme.Legends[i].Caption = oldTheme.Legends[i].Caption;
                                    newTheme.Legends[i].RangeFrom = oldTheme.Legends[i].RangeFrom;
                                    newTheme.Legends[i].RangeTo = oldTheme.Legends[i].RangeTo;
                                }
                            }
                            newTheme.UpdateLegendBreakCount();
                        }
                    }

                    //-Set Legend Title only if OldTReme's  IUS is same as NewTheme's IUS
                    if (newTheme.I_U_S_GIDs.Length == oldTheme.I_U_S_GIDs.Length)
                    {
                        if (newTheme.I_U_S_GIDs[0] == oldTheme.I_U_S_GIDs[0] && newTheme.I_U_S_GIDs[0] == oldTheme.I_U_S_GIDs[0] && newTheme.I_U_S_GIDs[0] == oldTheme.I_U_S_GIDs[0])
                        {
                            newTheme.LegendTitle = oldTheme.LegendTitle;
                        }
                    }

                    //-- set Theme Type specific properties
                    switch (oldTheme.Type)
                    {
                        case ThemeType.Color:
                            // set Legend Colors
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                }
                            }

                            //MultiLegends  TODO confirm
                            if (oldTheme.MultiLegend == true)
                            {
                                newTheme.MultiLegend = oldTheme.MultiLegend;
                                newTheme.MultiLegendCriteria = oldTheme.MultiLegendCriteria;
                                newTheme.MultiLegendCol.Clear();
                                foreach (object m_Keys in oldTheme.MultiLegendCol.Keys)
                                {
                                    newTheme.MultiLegendCol.Add(m_Keys, oldTheme.MultiLegendCol[m_Keys]);
                                }
                            }

                            break;
                        case ThemeType.DotDensity:
                            newTheme.DotColor = oldTheme.DotColor;
                            newTheme.DotFont = oldTheme.DotFont;
                            newTheme.DotSize = oldTheme.DotSize;
                            newTheme.DotChar = oldTheme.DotChar;
                            newTheme.DotStyle = oldTheme.DotStyle;
                            newTheme.DotValue = oldTheme.DotValue;
                            break;
                        case ThemeType.Chart:
                            newTheme.Name = oldTheme.Name;
                            newTheme.ChartLeaderColor = oldTheme.ChartLeaderColor;
                            newTheme.ChartLeaderStyle = oldTheme.ChartLeaderStyle;
                            newTheme.ChartLeaderVisible = oldTheme.ChartLeaderVisible;
                            newTheme.ChartLeaderWidth = oldTheme.ChartLeaderWidth;
                            newTheme.RoundDecimals = oldTheme.RoundDecimals;
                            newTheme.ColumnsGap = oldTheme.ColumnsGap;
                            newTheme.ShowChartAxis = oldTheme.ShowChartAxis;
                            newTheme.ChartSize = oldTheme.ChartSize;
                            newTheme.ChartType = oldTheme.ChartType;
                            newTheme.ChartWidth = oldTheme.ChartWidth;
                            newTheme.DisplayChartData = oldTheme.DisplayChartData;
                            newTheme.ChartSeriestype = oldTheme.ChartSeriestype;
                            newTheme.DisplayChartMRD = oldTheme.DisplayChartMRD;
                            newTheme.PieAutoSize = oldTheme.PieAutoSize;
                            newTheme.PieSize = oldTheme.PieSize;
                            newTheme.PieAutoSizeFactor = oldTheme.PieAutoSizeFactor;
                            newTheme.ChartLineThickness = oldTheme.ChartLineThickness;
                            newTheme.ChartAxisLabelSettings = oldTheme.ChartAxisLabelSettings;
                            newTheme.ChartDataLabelSettings = oldTheme.ChartDataLabelSettings;

                            newTheme.ExcludeAreaIDs = oldTheme.ExcludeAreaIDs;

                            if (newTheme.SubgroupNId.Length == oldTheme.SubgroupNId.Length)
                            {
                                newTheme.SubgroupVisible = oldTheme.SubgroupVisible;
                                newTheme.SubgroupFillStyle = oldTheme.SubgroupFillStyle;
                            }

                            if (newTheme.SourceNIds != null && oldTheme.SourceNIds != null)
                            {
                                if (newTheme.SourceNIds.Length == oldTheme.SourceNIds.Length)
                                {
                                    newTheme.SourceVisible = oldTheme.SourceVisible;
                                }
                            }

                            for (int i = 0; i < Math.Min(newTheme.ChartTimePeriods.Count, oldTheme.ChartTimePeriods.Count); i++)
                            {
                                string timePeriodKey = oldTheme.ChartTimePeriods.Keys[i];
                                if (oldTheme.ChartTimePeriods.ContainsKey(timePeriodKey))
                                {
                                    newTheme.ChartTimePeriods[timePeriodKey] = oldTheme.ChartTimePeriods[timePeriodKey];
                                }
                            }
                            foreach (object key in oldTheme.ModifiedCharts.Keys)
                            {
                                if (newTheme.ModifiedCharts.ContainsKey(key) == false)
                                {
                                    newTheme.ModifiedCharts.Add(key, oldTheme.ModifiedCharts[key]);
                                }
                            }

                            break;
                        case ThemeType.Hatch:
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                newTheme.Legends[i].FillStyle = oldTheme.Legends[i].FillStyle;
                            }
                            break;
                        case ThemeType.Symbol:
                            // set Legend Symbols
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                    newTheme.Legends[i].SymbolImage = oldTheme.Legends[i].SymbolImage;
                                    newTheme.Legends[i].MarkerType = oldTheme.Legends[i].MarkerType;
                                    newTheme.Legends[i].MarkerSize = oldTheme.Legends[i].MarkerSize;
                                    newTheme.Legends[i].MarkerFont = oldTheme.Legends[i].MarkerFont;
                                    newTheme.Legends[i].MarkerChar = oldTheme.Legends[i].MarkerChar;
                                }
                            }

                            newTheme.X_Offset = oldTheme.X_Offset;
                            newTheme.Y_Offset = oldTheme.Y_Offset;
                            break;
                        case ThemeType.Label:
                            newTheme.LabelField = oldTheme.LabelField;
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                    newTheme.Legends[i].MarkerFont = oldTheme.Legends[i].MarkerFont;
                                }
                            }

                            newTheme.X_Offset = oldTheme.X_Offset;
                            newTheme.Y_Offset = oldTheme.Y_Offset;

                            break;
                        default:
                            break;
                    }

                }

            }
            catch
            {

            }


        }

        /// <summary>
        /// It updates Legend settings of oldTheme into NewTheme.
        /// <para>Following Theme properties are updated: BreakCount(LegendCount), Layer's Visibility, StartColor, EndColor, Theme.Visible, Legend's Color, Decimal, BorderColor, BorderStyle, BorderWidth.
        /// </para>
        /// </summary>
        /// <param name="oldTheme">Old Theme</param>
        /// <param name="newTheme">New Theme to retain properties of old theme.</param>
        /// <param name="preserveLegendsRanges">true, if oldtheme's Legend range values to be preserved in new theme as well(applicable to Break based Themes)</param>
        public void UpdateThemeLegendSettings(Theme oldTheme, Theme newTheme, bool preserveLegendsRanges)
        {
            try
            {
                if (newTheme != null)
                {
                    // set Theme's common properties

                    //--Layer's Visibility
                    foreach (object key in oldTheme.LayerVisibility.Keys)
                    {
                        //Theme.LayerVisibility[Key, Value]
                        //Key: LayerID
                        //Value: bool   
                        if (newTheme.LayerVisibility.ContainsKey(key))
                        {
                            newTheme.LayerVisibility[key] = (bool)oldTheme.LayerVisibility[key];
                        }
                    }

                    newTheme.Visible = oldTheme.Visible;

                    newTheme.BorderColor = oldTheme.BorderColor;
                    newTheme.BorderStyle = oldTheme.BorderStyle;
                    newTheme.BorderWidth = oldTheme.BorderWidth;
                    newTheme.LabelColor = oldTheme.LabelColor;
                    newTheme.LabelFont = oldTheme.LabelFont;
                    newTheme.LabelVisible = oldTheme.LabelVisible;
                    newTheme.LabelField = oldTheme.LabelField;
                    newTheme.LabelMultirow = oldTheme.LabelMultirow;
                    newTheme.LabelIndented = oldTheme.LabelIndented;


                    //-- Legend Settings
                    newTheme.StartColor = oldTheme.StartColor;
                    newTheme.EndColor = oldTheme.EndColor;
                    newTheme.BreakCount = oldTheme.BreakCount;
                    newTheme.BreakType = oldTheme.BreakType;
                    newTheme.Decimals = oldTheme.Decimals;


                    newTheme.LegendBodyColor = oldTheme.LegendBodyColor;
                    newTheme.LegendBodyFont = oldTheme.LegendBodyFont;
                    newTheme.LegendColor = oldTheme.LegendColor;
                    newTheme.LegendFont = oldTheme.LegendFont;

                    newTheme.MultiLegendCriteria = oldTheme.MultiLegendCriteria;
                    newTheme.MultiLegend = oldTheme.MultiLegend;

                    if (newTheme.Type != ThemeType.DotDensity && newTheme.Type != ThemeType.Chart)
                    {
                        newTheme.Legends.Clear();

                        this.setThemeRange(this.Themes.ItemIndex(newTheme.ID));

                        //- Preserve OldTheme's Minimum & Maximum values and Legend Ranges, Caption, Title etc.
                        if (preserveLegendsRanges)
                        {
                            newTheme.Maximum = oldTheme.Maximum;
                            newTheme.Minimum = oldTheme.Minimum;
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Title = oldTheme.Legends[i].Title;
                                    newTheme.Legends[i].Caption = oldTheme.Legends[i].Caption;
                                    newTheme.Legends[i].RangeFrom = oldTheme.Legends[i].RangeFrom;
                                    newTheme.Legends[i].RangeTo = oldTheme.Legends[i].RangeTo;
                                    newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                }
                            }
                            newTheme.UpdateLegendBreakCount();
                        }
                        else
                        {
                            //- update only Legend Colors
                            for (int i = 0; i < newTheme.Legends.Count; i++)
                            {
                                if (oldTheme.Legends[i] != null)
                                {
                                    newTheme.Legends[i].Color = oldTheme.Legends[i].Color;
                                }
                            }
                        }
                    }

                    switch (newTheme.Type)
                    {
                        case ThemeType.Symbol:
                            //*** Reset the Symbols legend
                            for (int i = 0; i <= newTheme.Legends.Count - 1; i++)
                            {
                                newTheme.Legends[i].MarkerType = MarkerStyle.Custom;
                                if (i < newTheme.Legends.Count - 1)
                                {
                                    newTheme.Legends[i].MarkerFont = new Font("Webdings", 10 + i * 5);
                                    newTheme.Legends[i].MarkerSize = 10 + i * 5;
                                }
                                else
                                {
                                    newTheme.Legends[i].MarkerFont = new Font("Webdings", 10);
                                    newTheme.Legends[i].MarkerSize = 10;
                                    newTheme.Legends[i].Color = Color.FromArgb(128, newTheme.MissingColor);
                                }

                                newTheme.Legends[i].MarkerChar = Strings.Chr(110);
                            }

                            break;
                        case ThemeType.Label:
                            //*** Reset the legend
                            Font fnt = new Font("Arial", 8);
                            for (int i = 0; i <= newTheme.Legends.Count - 1; i++)
                            {
                                newTheme.Legends[i].MarkerFont = fnt;
                            }

                            break;
                    }


                }

            }
            catch
            {

            }


        }


        public Theme setThemeRange(int p_Index)
        {
            BreakType _BreakType;
            DataView _DV;
            Theme _Theme = null;
            string TempValue = string.Empty;

            _Theme = m_Themes[p_Index];
            _Theme.StartColor = m_FirstColor;
            _Theme.EndColor = m_FourthColor;

            //- Round Min & max value to Decimal Places
            TempValue = DICommon.RoundDecimalValueTowardsZero(_Theme.Minimum.ToString(), _Theme.Decimals);
            _Theme.Minimum = decimal.Parse(TempValue);

            TempValue = DICommon.RoundDecimalValueAwayFromZero(_Theme.Maximum.ToString(), _Theme.Decimals);
            _Theme.Maximum = decimal.Parse(TempValue);

            this.MRDData.RowFilter = Indicator.IndicatorNId + " IN (" + _Theme.IndicatorNId[0] + ") AND " + Unit.UnitNId + " IN (" + _Theme.UnitNId.ToString() + ")";

            //--In case of Chart Theme, SubgroupVal is -1
            if (_Theme.SubgroupNId[0] != "-1")
            {
                this.MRDData.RowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + _Theme.SubgroupNId[0] + ")";
            }
            _DV = new DataView(this.MRDData.ToTable());
            this.MRDData.RowFilter = "";

            _BreakType = _Theme.BreakType;
            switch (_Theme.BreakType)
            {
                case BreakType.EqualCount:
                case BreakType.EqualSize:
                    _Theme.SetRange(_DV);
                    break;
                case BreakType.Continuous:
                case BreakType.Discontinuous:
                    if (_Theme.Legends.Count <= 0)
                    {
                        _Theme.BreakType = BreakType.EqualCount;
                        _Theme.SetRange(_DV);
                        _Theme.BreakType = _BreakType;
                    }
                    else
                    {
                        _Theme.SetRangeCount(_DV);
                    }

                    break;
            }

            // Set missing value text on Legends : ****Bugfix 18 Mar 08: dg7
            _Theme.Legends[_Theme.Legends.Count - 1].Caption = m_MissingValue;

            return _Theme;
        }

        /// <summary>
        /// It updates Chart Theme's AreaIndexes data Values for Multiple Sources against Single IUS.
        /// </summary>
        /// <param name="p_SubgroupNId">SubgroupNId to pair with existing Indicator & Unit in Current Theme.</param>
        /// <param name="p_Theme">Chart Theme object to edit.</param>
        public void UpdateChartForMultipleSource(string p_SubgroupNId, ref Theme p_Theme)
        {
            string RowFilter = string.Empty;
            string OriginalFilter = string.Empty;

            DataTable SourceNIdTable = null;
            DataView ThemeData = null;

            if (p_Theme != null && string.IsNullOrEmpty(p_SubgroupNId) == false)
            {
                try
                {
                    p_Theme.ChartSeriestype = ChartSeriesType.Source;

                    OriginalFilter = this.PresentationData.RowFilter;

                    //Get all SourceNIds for given Indicator, unit, Subgroups from Presentation data
                    RowFilter = Indicator.IndicatorNId + " IN(" + p_Theme.IndicatorNId[0] + ") AND " + Unit.UnitNId + " IN (" + p_Theme.UnitNId + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + p_SubgroupNId + ")";
                    this.PresentationData.RowFilter = RowFilter;


                    //Theme Data
                    ThemeData = this.PresentationData.ToTable().DefaultView;

                    //- Source Table
                    SourceNIdTable = ThemeData.ToTable(true, IndicatorClassifications.ICNId, IndicatorClassifications.ICName);

                    //- Set original Row Filter.
                    this.PresentationData.RowFilter = OriginalFilter;

                    //Initialize Theme.SouceNIds array.
                    p_Theme.SourceNIds = new string[SourceNIdTable.Rows.Count];
                    p_Theme.SourceName = new string[SourceNIdTable.Rows.Count];

                    for (int i = 0; i < SourceNIdTable.Rows.Count; i++)
                    {
                        p_Theme.SourceNIds[i] = SourceNIdTable.Rows[i][IndicatorClassifications.ICNId].ToString();
                        p_Theme.SourceName[i] = SourceNIdTable.Rows[i][IndicatorClassifications.ICName].ToString();
                    }

                    // Re create Chart Theme's AreaIndex. This time DataValue will be for Single IUS & Muultiple Sources
                    p_Theme.AreaIndexes = new Hashtable();
                    p_Theme.ChartSeriestype = ChartSeriesType.Source;

                    ThemeData.Sort = DataExpressionColumns.DataType + " ASC," + DataExpressionColumns.NumericData + " ASC";
                    p_Theme.Minimum = (decimal)ThemeData[0][DataExpressionColumns.NumericData];
                    p_Theme.Maximum = (decimal)ThemeData[ThemeData.Count - 1][DataExpressionColumns.NumericData];

                    //--Passing only one SubgroupNId for ChartSeries == Source
                    this.MakeChartTheme(p_Theme.IndicatorNId, new String[] { p_SubgroupNId }, ThemeData, p_Theme);

                    //- Update subgroupNID
                    //p_Theme.SubgroupNId = new string[] { p_SubgroupNId };

                    //- Update SourceFillStyle (Fill Colors)
                    p_Theme.SetChartSeriesVisibility();

                    //-- Update Theme ID = IndNid _ UnitNid _ SubgroupNid _ Type
                    p_Theme.ID = p_Theme.IndicatorNId[0] + "_" + p_Theme.UnitNId + "_" + p_SubgroupNId + "_" + (int)p_Theme.Type;

                    //-- Save GIDs of Indicator, Unit, Subgroups used in theme creation.
                    p_Theme.I_U_S_GIDs = this.GetI_U_S_GIDsByNids(this._DIDataView.MainDataTable.DefaultView, p_Theme.IndicatorNId[0], p_Theme.UnitNId.ToString(), p_SubgroupNId);
                }
                catch
                {
                    //- Set original Row Filter.
                    this.PresentationData.RowFilter = OriginalFilter;
                }
            }
        }

        public void UpdateChartForMultipleSubgroups(Theme p_Theme)
        {
            string RowFilter = string.Empty;
            string OriginalFilter = string.Empty;

            DataTable SubgroupNIdTable = null;
            DataView ThemeData = null;
            string[] _SPNID = null;
            string[] _SPName = null;
            if (p_Theme != null)
            {
                try
                {
                    p_Theme.ChartSeriestype = ChartSeriesType.Subgroup;

                    OriginalFilter = this.PresentationData.RowFilter;

                    //Get all SourceNIds for given Indicator, unit, Subgroups from Presentation data
                    RowFilter = Indicator.IndicatorNId + " IN(" + p_Theme.IndicatorNId[0] + ") AND " + Unit.UnitNId + " IN (" + p_Theme.UnitNId + ") ";
                    this.PresentationData.RowFilter = RowFilter;


                    //Theme Data
                    ThemeData = this.PresentationData.ToTable().DefaultView;

                    //- Source Table
                    SubgroupNIdTable = ThemeData.ToTable(true, SubgroupVals.SubgroupValNId, SubgroupVals.SubgroupVal);

                    //- Set original Row Filter.
                    this.PresentationData.RowFilter = OriginalFilter;

                    //Initialize Theme.SouceNIds array.
                    _SPNID = new string[SubgroupNIdTable.Rows.Count];
                    _SPName = new string[SubgroupNIdTable.Rows.Count];

                    for (int i = 0; i < SubgroupNIdTable.Rows.Count; i++)
                    {
                        _SPNID[i] = SubgroupNIdTable.Rows[i][SubgroupVals.SubgroupValNId].ToString();
                        _SPName[i] = SubgroupNIdTable.Rows[i][SubgroupVals.SubgroupVal].ToString();
                    }

                    p_Theme.SubgroupName = _SPName;
                    p_Theme.SubgroupNId = _SPNID;

                    // Re create Chart Theme's AreaIndex. This time DataValue will be for Single IUS & Muultiple Sources
                    p_Theme.AreaIndexes = new Hashtable();
                    p_Theme.ChartSeriestype = ChartSeriesType.Subgroup;

                    ThemeData.Sort = DataExpressionColumns.DataType + " ASC," + DataExpressionColumns.NumericData + " ASC";
                    p_Theme.Minimum = (decimal)ThemeData[0][DataExpressionColumns.NumericData];
                    p_Theme.Maximum = (decimal)ThemeData[ThemeData.Count - 1][DataExpressionColumns.NumericData];

                    this.MakeChartTheme(p_Theme.IndicatorNId, p_Theme.SubgroupNId, ThemeData, p_Theme);

                    //- Set SourceNIDs NULL
                    p_Theme.SourceNIds = new string[] { "" };

                    //- Update SourceFillStyle (Fill Colors)
                    p_Theme.SetChartSeriesVisibility();

                    //-- Update Theme ID = IndNid _ UnitNid _ SubgroupNid _ Type
                    p_Theme.ID = p_Theme.IndicatorNId[0] + "_" + p_Theme.UnitNId + "_-1_" + (int)p_Theme.Type;

                    //-- Save GIDs of Indicator, Unit & default SubgroupVal used in theme creation.
                    p_Theme.I_U_S_GIDs = this.GetI_U_S_GIDsByNids(this._DIDataView.MainDataTable.DefaultView, p_Theme.IndicatorNId[0], p_Theme.UnitNId.ToString(), p_Theme.SubgroupNId[0]);
                }
                catch
                {
                }
            }
        }

        private void MakeChartTheme(string[] p_indicator, string[] p_Subgroup, DataView p_ThemeData, Theme p_Theme)
        {
            string _Value = string.Empty;
            string ChartDataTimeKey = string.Empty; // Key represents TimePeriod.
            string _RowFilter = p_ThemeData.RowFilter;
            string _LocalFilter = "";
            string[] SubgroupArray = null;
            string ID = string.Empty;
            string[] ChartSeriesTypeNIDs = null;
            bool DataFoundForTimePeriod = false;
            string ChartMRD = string.Empty;

            //--IMP: some dataValue might have decimal,
            //--and in French Setting, decimals are treated as "," which was causing error while rendering ChartMap

            //-- Set Culture Info setting to US-English, and then reset original CultureSetting at the end.
            //-- Get the current culture.
            System.Globalization.CultureInfo OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //-- Reset the culture to english - US
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //-- set Chart Series NIds on the basis of Source(multiple Sources) OR Subgroup ( Multiple Subgroups)  
            if (p_Theme.ChartSeriestype == ChartSeriesType.Source)
            {
                ChartSeriesTypeNIDs = p_Theme.SourceNIds;
            }
            else
            {
                ChartSeriesTypeNIDs = p_Subgroup;
            }

            //object[] oMDKeys = new object[MDKeys.Keys.Count];
            //MDKeys.Keys.CopyTo(oMDKeys, 0);

            object[] oMDKeys = this.MDColumns;
            {
                //p_Theme.ChartVisibleTimePeriods.Clear();
                p_Theme.ChartTimePeriods.Clear();

                foreach (DataRowView _DRV in p_ThemeData)
                {
                    if (!p_Theme.AreaIndexes.ContainsKey((string)_DRV[Area.AreaID]))
                    {
                        ID = (string)_DRV[Area.AreaID];
                        _Value = "";
                        ChartMRD = "";
                        AreaInfo _AreaInfo = new AreaInfo();
                        _AreaInfo.ChartData = new Hashtable();
                        {
                            _AreaInfo.IndicatorGID = (string)_DRV[Indicator.IndicatorGId];
                            _AreaInfo.SubgroupGID = (string)_DRV[SubgroupVals.SubgroupValGId];
                            _AreaInfo.UnitGID = (string)_DRV[Unit.UnitGId];
                            _AreaInfo.Subgroup = (string)_DRV[SubgroupVals.SubgroupVal];
                            _AreaInfo.Time = (string)_DRV[Timeperiods.TimePeriod];
                            _AreaInfo.AreaName = (string)_DRV[Area.AreaName];
                            _AreaInfo.Source = (string)_DRV[IndicatorClassifications.ICName];

                            //*** Metadata
                            _AreaInfo.MDFldVal = new Hashtable();
                            for (int i = 0; i <= oMDKeys.Length - 1; i++)
                            {
                                if (p_ThemeData.Table.Columns.Contains(oMDKeys[i].ToString()))
                                {
                                    _AreaInfo.MDFldVal.Add(oMDKeys[i], _DRV[(string)oMDKeys[i]]);
                                }
                            }

                            _AreaInfo.DataValue = (decimal)Conversion.Val(_DRV[DataExpressionColumns.NumericData]);
                            //.RenderingInfo = CInt(Val(_DRV("Data_Value")) * _Count / p_Theme.Maximum)

                            //-- Loop each timeperiod and Get Delimieted dataValue for multiple Source/Subgroup
                            foreach (DataRowView drow in this.GetTimePeriods())
                            {
                                _Value = "";
                                ChartDataTimeKey = string.Empty;
                                DataFoundForTimePeriod = false;
                                foreach (string _Ind in p_indicator)
                                {
                                    foreach (string _SP in ChartSeriesTypeNIDs)
                                    {
                                        //-- Apply RowFilter 
                                        _LocalFilter = " Indicator_Nid=" + (string)_Ind + " AND Area_NId=" + _DRV["Area_NId"].ToString();

                                        //In Pie chart - Multiple Subgroups , No timePeriod, NO Sources
                                        if (p_Theme.ChartType == ChartType.Pie)
                                        {
                                            _LocalFilter += " AND Subgroup_Val_NId=" + (string)_SP;
                                        }
                                        else
                                        {
                                            //- In Bar/Line chart, timeSeries will be ON, with multiple Source/Subgroups.
                                            if (p_Theme.ChartSeriestype == ChartSeriesType.Source)
                                            {
                                                _LocalFilter += " AND Subgroup_Val_NId=" + (string)p_Subgroup[0];
                                                _LocalFilter += " AND " + IndicatorClassifications.ICNId + " = " + (string)_SP;
                                            }
                                            else if (p_Theme.ChartSeriestype == ChartSeriesType.Subgroup)
                                            {
                                                _LocalFilter += " AND Subgroup_Val_NId=" + (string)_SP;
                                            }
                                        }

                                        _LocalFilter += " AND " + Timeperiods.TimePeriod + " = '" + drow[Timeperiods.TimePeriod].ToString() + "'";

                                        if (_RowFilter.Length > 0)
                                        {
                                            p_ThemeData.RowFilter += " AND " + _LocalFilter;
                                        }
                                        else
                                        {
                                            p_ThemeData.RowFilter = _LocalFilter;
                                        }
                                        //-- Get chart Data
                                        if (_Value.Length > 0)
                                            _Value += ",";

                                        if (p_ThemeData.Count > 0)
                                        {
                                            DataFoundForTimePeriod = true;      // indicating that data is found for at least one Subgroup/Source
                                            _Value += (p_ThemeData[0][DataExpressionColumns.NumericData]).ToString();
                                            ChartDataTimeKey = p_ThemeData[0][Timeperiods.TimePeriod].ToString();

                                            //-Set TimePeriod visibilty true (default)
                                            if (p_Theme.ChartTimePeriods.Keys.Contains(ChartDataTimeKey) == false)
                                            {
                                                //p_Theme.ChartVisibleTimePeriods.Add(ChartDataTimeKey);
                                                p_Theme.ChartTimePeriods.Add(ChartDataTimeKey, true);
                                            }
                                        }
                                        else
                                        {
                                            if (ChartDataTimeKey.Length == 0)
                                            {
                                                ChartDataTimeKey = drow[Timeperiods.TimePeriod].ToString();
                                            }
                                            // then insert '{^}' so that Pie chart can sense the 0 value for given SubgroupVal
                                            _Value += "{^}";
                                        }

                                        p_ThemeData.RowFilter = _RowFilter;

                                    }
                                }

                                if (DataFoundForTimePeriod)
                                {
                                    _Value = _Value.Replace("{^}", "");

                                    //_AreaInfo.ChartData = _Value;
                                    _AreaInfo.ChartData.Add(ChartDataTimeKey, _Value);
                                }
                                ////if (p_Theme.ChartType == ChartType.Pie)
                                ////{
                                ////    break;  // Only one TimePeriod is needed.
                                ////}
                            }
                        }

                        //-- Get Most recent Data for IUS + Area + Souce
                        foreach (string _SGNId in ChartSeriesTypeNIDs)
                        {
                            if (p_Theme.ChartSeriestype == ChartSeriesType.Subgroup)
                            {
                                ChartMRD += "," + this.GetMRDForThemeArea(p_ThemeData, p_indicator[0], string.Empty, (string)_SGNId, _DRV[Area.AreaNId].ToString(), string.Empty);
                            }
                            else if (p_Theme.ChartSeriestype == ChartSeriesType.Source)
                            {
                                ChartMRD += "," + this.GetMRDForThemeArea(p_ThemeData, p_indicator[0], string.Empty, p_Subgroup[0], _DRV[Area.AreaNId].ToString(), (string)_SGNId);
                            }
                        }

                        _AreaInfo.ChartMostRecentData = ChartMRD.Substring(1);  //removing first comma
                        _AreaInfo.ChartMostRecentDataCopy = ChartMRD.Substring(1);

                        p_Theme.AreaIndexes.Add(ID, _AreaInfo);
                    }
                }
            }

            //-- Restore the culture.
            System.Threading.Thread.CurrentThread.CurrentCulture = OldCulture;
        }

        private void CalcDotDensity(DataView p_ThemeData, ref Theme p_Theme)
        {
            //*** Bugfix Feb 2006 Improper Dot Value logic

            decimal MinVal = 0;
            decimal MaxVal = 0;
            decimal DotValue;


            //*** Get Min and Max datavalues for current dataview
            //GetMinMaxDataValue(ref p_ThemeData, ref MinVal, ref MaxVal);
            p_ThemeData.Sort = DataExpressionColumns.DataType + " ASC," + DataExpressionColumns.NumericData + " ASC";
            MinVal = (decimal)p_ThemeData[0][DataExpressionColumns.NumericData];
            MaxVal = (decimal)p_ThemeData[p_ThemeData.Count - 1][DataExpressionColumns.NumericData];


            //*** Set the DotValues
            if (MaxVal > 0 & MaxVal < 1)
            {
                //For cases like Index datavalues
                DotValue = MinVal;
            }
            else if (MaxVal < 100)
            {
                //*** For cases like ALR where datavalues are less than 100
                DotValue = 1;
            }
            else
            {
                //-- Bug fix: When minValue = 0 and Max value > 100 , then max / min became infinity.
                if (MaxVal > 100 && MinVal == 0)
                {
                    MinVal = 1;
                }

                //*** For cases like population size where data values can be in lakhs
                if (MaxVal / MinVal > 500)
                {
                    //*** too much variation
                    DotValue = MaxVal / 500;
                }
                else
                {
                    DotValue = MinVal;
                }
            }

            p_Theme.Maximum = MaxVal;
            if (DotValue < 1)
            {
                p_Theme.DotValue = (double)Math.Round(DotValue, 2);
            }
            else if (DotValue < 10)
            {
                p_Theme.DotValue = (double)Math.Round(DotValue, 1);

            }
            else
            {
                p_Theme.DotValue = (double)Math.Round(DotValue, 0);
            }


            //object[] oMDKeys = new object[MDKeys.Keys.Count];
            //MDKeys.Keys.CopyTo(oMDKeys, 0);
            object[] oMDKeys = this.MDColumns;
            {
                foreach (DataRowView _DRV in p_ThemeData)
                {
                    if (!p_Theme.AreaIndexes.ContainsKey((string)_DRV[Area.AreaID]))            //"Area_ID"
                    {
                        AreaInfo _AreaInfo = new AreaInfo();
                        {
                            _AreaInfo.IndicatorGID = (string)_DRV[Indicator.IndicatorGId];      //"Indicator_GId"
                            _AreaInfo.UnitGID = (string)_DRV[Unit.UnitGId];                     //"Unit_GId"
                            _AreaInfo.SubgroupGID = (string)_DRV[SubgroupVals.SubgroupValGId];  //"Subgroup_Val_GId"
                            _AreaInfo.Subgroup = (string)_DRV[SubgroupVals.SubgroupVal];        //"Subgroup_Val"
                            _AreaInfo.Time = (string)_DRV[Timeperiods.TimePeriod];              //"TimePeriod"
                            _AreaInfo.AreaName = (string)_DRV[Area.AreaName];                   //"Area_Name"
                            _AreaInfo.Source = (string)_DRV[IndicatorClassifications.ICName];   //"IC_Name"

                            //*** Metadata
                            _AreaInfo.MDFldVal = new Hashtable();
                            for (int i = 0; i <= oMDKeys.Length - 1; i++)
                            {
                                if (p_ThemeData.Table.Columns.Contains(oMDKeys[i].ToString()))
                                {
                                    _AreaInfo.MDFldVal.Add(oMDKeys[i], _DRV[(string)oMDKeys[i]]);
                                }
                            }

                            _AreaInfo.DataValue = (decimal)Conversion.Val(_DRV[DataExpressionColumns.NumericData]); //"Data_Value"

                            //DisplayInfo stores textual DataValue (e.g. yes, no) and will be used as part map label
                            _AreaInfo.DisplayInfo = Convert.ToString(_DRV[Data.DataValue]);
                        }
                        //.RenderingInfo = CInt(Val(_DRV("Data_Value")) * DotValue / MaxVal)
                        p_Theme.AreaIndexes.Add((string)_DRV[Area.AreaID], _AreaInfo);      //"Area_ID"
                    }
                }
            }
        }


        //Used for Loading ans Saving legend settings
        public Theme GetTheme(string p_FileName, int p_Index, bool p_LoadColor, bool p_LoadRange, bool p_LoadLabels, bool p_LoadBreaks)
        {
            int i;
            Theme _Theme = null;
            Theme _LoadedTheme = Theme.Load(p_FileName);

            if (_LoadedTheme != null)
            {
                _Theme = GetTheme(p_Index);
                {
                    //-- check if breaks conditions is checked true,
                    //-- then assign Legend collection, Breaks & decimals as it is.
                    if (p_LoadBreaks)
                    {
                        _Theme.Minimum = _LoadedTheme.Minimum;
                        _Theme.Maximum = _LoadedTheme.Maximum;
                        _Theme.Decimals = _LoadedTheme.Decimals;
                        _Theme.BreakCount = _LoadedTheme.BreakCount;
                        _Theme.BreakType = _LoadedTheme.BreakType;
                        if (_Theme.Type == _LoadedTheme.Type)
                        {
                            //-- Set Legends Collection as it is if types of both are same.
                            _Theme.Legends = _LoadedTheme.Legends;
                        }
                        else
                        {
                            List<Legend> LegendsToRemove = new List<Legend>();

                            //- Remove Legends if LoadedTheme's Legends Count are less than Current theme's Legends
                            if (_LoadedTheme.Legends.Count < _Theme.Legends.Count)
                            {
                                for (i = _LoadedTheme.Legends.Count - 1; i < _Theme.Legends.Count - 1; i++)
                                {
                                    LegendsToRemove.Add(_Theme.Legends[i]);
                                }
                            }

                            //- Start Remove Legends
                            foreach (Legend Lg in LegendsToRemove)
                            {
                                _Theme.Legends.Remove(Lg);
                            }
                        }
                    }

                    for (i = 0; i <= _Theme.Legends.Count - 2; i++)
                    {
                        if (i > _LoadedTheme.Legends.Count - 2)
                            break;

                        try
                        {


                            if (p_LoadLabels)
                            {
                                //_Theme.Legends[i].Title = _LoadedTheme.Legends[i].Title;
                                _Theme.Legends[i].Caption = _LoadedTheme.Legends[i].Caption;
                            }
                            if (p_LoadRange)
                            {
                                _Theme.Legends[i].RangeFrom = _LoadedTheme.Legends[i].RangeFrom;
                                _Theme.Legends[i].RangeTo = _LoadedTheme.Legends[i].RangeTo;
                                _Theme.Legends[i].Title = _Theme.Legends[i].RangeFrom + " - " + _LoadedTheme.Legends[i].RangeTo;
                            }

                            //OPT can be extended for other Break based theme
                            if (p_LoadColor)
                            {
                                if (_Theme.MultiLegend == true)
                                {
                                    object[] _Keys = new object[_Theme.MultiLegendCol.Keys.Count];
                                    _Theme.MultiLegendCol.Keys.CopyTo(_Keys, 0);
                                    if (_LoadedTheme.MultiLegend == true)
                                    {
                                        object[] _LoadedKeys = new object[_LoadedTheme.MultiLegendCol.Keys.Count];
                                        _LoadedTheme.MultiLegendCol.Keys.CopyTo(_LoadedKeys, 0);
                                        for (int j = 0; j <= Math.Min(_Keys.Length, _LoadedKeys.Length); j++)
                                        {
                                            ((Legends)_Theme.MultiLegendCol[_Keys[j]])[i].Color = ((Legends)_LoadedTheme.MultiLegendCol[_LoadedKeys[j]])[i].Color;
                                        }
                                    }
                                    else
                                    {
                                        ((Legends)_Theme.MultiLegendCol[_Keys[0]])[i].Color = _LoadedTheme.Legends[i].Color;
                                    }
                                }
                                else
                                {
                                    if (_LoadedTheme.MultiLegend == true)
                                    {
                                        object[] _Keys = new object[_LoadedTheme.MultiLegendCol.Keys.Count];
                                        _LoadedTheme.MultiLegendCol.Keys.CopyTo(_Keys, 0);
                                        _Theme.Legends[i].Color = ((Legends)_LoadedTheme.MultiLegendCol[_Keys[0]])[i].Color;
                                        //Apply
                                    }
                                    else
                                    {
                                        _Theme.Legends[i].Color = _LoadedTheme.Legends[i].Color;
                                    }
                                }

                                //-- Set Hatch, Symbol, Label related setttings also
                                if (_LoadedTheme.Type == ThemeType.Hatch)
                                {
                                    _Theme.Legends[i].FillStyle = _LoadedTheme.Legends[i].FillStyle;
                                }

                                if (_LoadedTheme.Type == ThemeType.Symbol)
                                {
                                    _Theme.Legends[i].MarkerChar = _LoadedTheme.Legends[i].MarkerChar;
                                    _Theme.Legends[i].MarkerFont = _LoadedTheme.Legends[i].MarkerFont;
                                    _Theme.Legends[i].MarkerSize = _LoadedTheme.Legends[i].MarkerSize;
                                    _Theme.Legends[i].MarkerType = _LoadedTheme.Legends[i].MarkerType;
                                }
                                if (_LoadedTheme.Type == ThemeType.Label)
                                {
                                    _Theme.Legends[i].LabelField = _LoadedTheme.Legends[i].LabelField;
                                    _Theme.Legends[i].LabelVisible = _LoadedTheme.Legends[i].LabelVisible;
                                    _Theme.Legends[i].LabelIndented = _LoadedTheme.Legends[i].LabelIndented;
                                    _Theme.Legends[i].LabelMultiRow = _LoadedTheme.Legends[i].LabelMultiRow;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            //*** In case of legend count mismatch beteween _theme and _Loaded Theme handle error
                        }
                    }

                    #region "Missing Legend"

                    //-- Set settings of Missing Legend
                    if (p_LoadColor)
                    {
                        _Theme.Legends[_Theme.Legends.Count - 1].Color = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].Color;
                    }
                    if (p_LoadLabels)
                    {
                        _Theme.Legends[_Theme.Legends.Count - 1].Caption = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].Caption;

                        if (_LoadedTheme.Type == _Theme.Type)
                        {
                            if (_LoadedTheme.Type == ThemeType.Hatch)
                            {
                                _Theme.Legends[_Theme.Legends.Count - 1].FillStyle = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].FillStyle;
                            }

                            if (_LoadedTheme.Type == ThemeType.Symbol)
                            {
                                _Theme.Legends[_Theme.Legends.Count - 1].MarkerChar = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].MarkerChar;
                                _Theme.Legends[_Theme.Legends.Count - 1].MarkerFont = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].MarkerFont;
                                _Theme.Legends[_Theme.Legends.Count - 1].MarkerSize = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].MarkerSize;
                                _Theme.Legends[_Theme.Legends.Count - 1].MarkerType = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].MarkerType;
                            }
                            if (_LoadedTheme.Type == ThemeType.Label)
                            {
                                _Theme.Legends[_Theme.Legends.Count - 1].LabelField = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].LabelField;
                                _Theme.Legends[_Theme.Legends.Count - 1].LabelVisible = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].LabelVisible;
                                _Theme.Legends[_Theme.Legends.Count - 1].LabelIndented = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].LabelIndented;
                                _Theme.Legends[_Theme.Legends.Count - 1].LabelMultiRow = _LoadedTheme.Legends[_LoadedTheme.Legends.Count - 1].LabelMultiRow;
                            }
                        }
                    }

                    #endregion

                    string sFilter = Indicator.IndicatorNId + " = " + _Theme.IndicatorNId[0] + " AND " + Unit.UnitNId + " = " + _Theme.UnitNId + " AND " + SubgroupVals.SubgroupValNId + " = " + _Theme.SubgroupNId[0];
                    this.MRDData.RowFilter = sFilter;
                    _Theme.SetRangeCount(new DataView(this.MRDData.ToTable()));
                    this.MRDData.RowFilter = "";

                }
            }
            return _Theme;
        }

        /// <summary>
        /// It updates chart theme's basic properties with the chart theme saved in file specified (.plc)
        /// </summary>
        /// <param name="currentChartTheme">chart theme to update</param>
        /// <param name="savedChartThemeFileName">(serialized) saved chart theme file name path</param>
        public void UpdateChartSettings(ref Theme currentChartTheme, string savedChartThemeFileName)
        {
            Theme NewChartTheme = null;

            if (File.Exists(savedChartThemeFileName))
            {
                Theme LoadedTheme = Theme.Load(savedChartThemeFileName);
                if (LoadedTheme.Type == ThemeType.Chart)
                {
                    //- If ChartType is different from currentTheme, then re-create Chart theme
                    if (currentChartTheme.ChartType != LoadedTheme.ChartType)
                    {
                        //-- Create new Chart theme on the basis of ChartType
                        int ThemeIndex = this.m_Themes.ItemIndex(currentChartTheme.ID);
                        currentChartTheme.ID = "TempThemeID";

                        //-- Preserve Old Theme's Excluded Areas List and re-assign to new Theme
                        List<string> ChartThemeAreas = currentChartTheme.ExcludeAreaIDs;

                        //-Pass ChartType and Chart SeriesBy value (Subgroup OR Source)
                        NewChartTheme = this.CreateTheme(currentChartTheme.IndicatorNId[0], currentChartTheme.UnitNId.ToString(), currentChartTheme.SubgroupNId[0], currentChartTheme.Type, LoadedTheme.ChartType, LoadedTheme.ChartSeriestype, ThemeIndex);

                        //-- Assign Excluded Chart Areas List to new theme
                        NewChartTheme.ExcludeAreaIDs = ChartThemeAreas;
                        if (NewChartTheme.ExcludeAreaIDs == null)
                        {
                            NewChartTheme.ExcludeAreaIDs = new List<string>();
                        }

                        //- Remove OLD theme
                        this.m_Themes.Remove(this.m_Themes.ItemIndex("TempThemeID"));

                        NewChartTheme.Name = currentChartTheme.Name;
                        currentChartTheme = NewChartTheme;
                    }
                    else
                    {
                        //- If Chart SeriesBy (Subgroup OR Source) is different
                        if (currentChartTheme.ChartSeriestype != LoadedTheme.ChartSeriestype)
                        {
                            if (LoadedTheme.ChartSeriestype == ChartSeriesType.Source)
                            {
                                this.UpdateChartForMultipleSource(currentChartTheme.SubgroupNId[0].ToString(), ref currentChartTheme);
                            }
                            else if (LoadedTheme.ChartSeriestype == ChartSeriesType.Subgroup)
                            {
                                this.UpdateChartForMultipleSubgroups(currentChartTheme);
                            }
                        }
                    }

                    //- Update common Chart Settings 
                    currentChartTheme.ChartAxisColor = LoadedTheme.ChartAxisColor;
                    currentChartTheme.ChartAxisLabelSettings = LoadedTheme.ChartAxisLabelSettings;
                    currentChartTheme.ChartDataLabelSettings = LoadedTheme.ChartDataLabelSettings;

                    currentChartTheme.ChartLeaderColor = LoadedTheme.ChartLeaderColor;
                    currentChartTheme.ChartLeaderStyle = LoadedTheme.ChartLeaderStyle;
                    currentChartTheme.ChartLeaderVisible = LoadedTheme.ChartLeaderVisible;
                    currentChartTheme.ChartLeaderWidth = LoadedTheme.ChartLeaderWidth;
                    currentChartTheme.ChartLineThickness = LoadedTheme.ChartLineThickness;

                    currentChartTheme.RoundDecimals = LoadedTheme.RoundDecimals;
                    currentChartTheme.ChartSize = LoadedTheme.ChartSize;
                    currentChartTheme.ChartWidth = LoadedTheme.ChartWidth;
                    currentChartTheme.ColumnsGap = LoadedTheme.ColumnsGap;
                    currentChartTheme.DisplayChartData = LoadedTheme.DisplayChartData;
                    currentChartTheme.DisplayChartMRD = LoadedTheme.DisplayChartMRD;
                    currentChartTheme.PieAutoSize = LoadedTheme.PieAutoSize;
                    currentChartTheme.PieAutoSizeFactor = LoadedTheme.PieAutoSizeFactor;
                    currentChartTheme.PieSize = LoadedTheme.PieSize;
                    currentChartTheme.ShowChartAxis = LoadedTheme.ShowChartAxis;

                    //- Set Subgroup Fill Colors
                    string[] ColorArr = currentChartTheme.SubgroupFillStyle;

                    for (int i = 0; i < currentChartTheme.SubgroupFillStyle.Length; i++)
                    {
                        if (LoadedTheme.SubgroupFillStyle.Length > i)
                        {
                            ColorArr[i] = LoadedTheme.SubgroupFillStyle[i];
                        }
                    }
                    currentChartTheme.SubgroupFillStyle = ColorArr;

                }
            }
        }

        /// <summary>
        /// It updated Dot theme's basic properties with the theme saved in file name specified.
        /// </summary>
        /// <param name="currentDotTheme">Dot theme to update</param>
        /// <param name="savedChartThemeFileName">(serialized) saved chart theme file name path</param>
        public void UpdateDotThemeSettings(Theme currentDotTheme, string savedDotThemeFileName)
        {
            if (File.Exists(savedDotThemeFileName))
            {
                Theme LoadedTheme = Theme.Load(savedDotThemeFileName);

                if (LoadedTheme.Type == ThemeType.DotDensity)
                {
                    //- Update common Chart Settings 
                    currentDotTheme.DotChar = LoadedTheme.DotChar;
                    currentDotTheme.DotColor = LoadedTheme.DotColor;
                    currentDotTheme.DotFont = LoadedTheme.DotFont;
                    currentDotTheme.DotSize = LoadedTheme.DotSize;
                    currentDotTheme.DotStyle = LoadedTheme.DotStyle;
                }
            }
        }

        /// <summary>
        /// Gets Theme object as selected by user when Theme's label setting changed in UI.
        /// </summary>
        /// <returns></returns>
        public Theme GetSelectedTheme()
        {
            Theme RetVal = null;

            if (string.IsNullOrEmpty(this._SelectedThemeID) == false)
            {
                //-- Check if ThemeID is present in Collection.
                foreach (Theme _Theme in this.m_Themes)
                {
                    if (string.Compare(_Theme.ID, this._SelectedThemeID, true) == 0)
                    {
                        RetVal = _Theme;
                        break;
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the SubgroupNId used in specified Chart theme. Returns -1 if theme is not a Chart theme.
        /// </summary>
        /// <returns></returns>
        public string GetSubgroupNameUsedInChartTheme(Theme chartTheme)
        {
            string RetVal = string.Empty;

            try
            {
                if (chartTheme != null && chartTheme.Type == ThemeType.Chart)
                {
                    // Theme.I_U_S_GIDs array represents GID in form of :- 
                    // Theme.I_U_S_GIDs[0] - IndicatorGID
                    // Theme.I_U_S_GIDs[1] - UnitGID
                    // Theme.I_U_S_GIDs[2] - SubgroupValGID
                    if (chartTheme.I_U_S_GIDs != null && chartTheme.I_U_S_GIDs.Length == 3)
                    {
                        //- Get SubgroupVal Name from SubgroupValGID used in ChartTheme
                        DataRow[] DRows = this._DIDataView.SubgroupVals.Select(SubgroupVals.SubgroupValGId + "='" + chartTheme.I_U_S_GIDs[2] + "'");

                        if (DRows.Length > 0)
                        {
                            RetVal = Convert.ToString(DRows[0][SubgroupVals.SubgroupVal]);
                        }
                    }
                }
            }
            catch
            {
            }
            return RetVal;
        }

        #endregion

        #region " Extent Setting and Transformations"

        private void SetTransVar()
        {
            //*** Calculate the transformation values
            double ConX;
            double ConY;
            double MapL;
            double MapT;
            double ExtRat;
            double PicRat;
            double ScaleRat;
            try
            {
                ExtRat = m_CurrentExtent.Width / m_CurrentExtent.Height;
                PicRat = m_Width / m_Height;
                ScaleRat = ExtRat / PicRat;
                if (ExtRat >= PicRat)
                {
                    ConX = (m_Width) / m_CurrentExtent.Width;
                    ConY = (m_Height / ScaleRat) / m_CurrentExtent.Height;
                    MapL = m_CurrentExtent.Left;
                    MapT = m_CurrentExtent.Top + (((m_Height / ConY) - m_CurrentExtent.Height) / 2);
                }
                else
                {
                    ConX = (m_Width * ScaleRat) / m_CurrentExtent.Width;
                    ConY = m_Height / m_CurrentExtent.Height;
                    MapL = m_CurrentExtent.Left - (((m_Width / ConX) - m_CurrentExtent.Width) / 2);
                    MapT = m_CurrentExtent.Top;
                }

                //mTransMatrix.Reset(): 'mTransMatrix.Translate(-MapL, -MapT): 'mTransMatrix.Scale(ConX, -ConY, System.Drawing.Drawing2D.MatrixOrder.Append)
                m_TransMatrix[0] = (float)ConX;
                //m11
                m_TransMatrix[1] = 0;
                //m12
                m_TransMatrix[2] = 0;
                //m21
                m_TransMatrix[3] = (float)(-ConY);
                //m22
                m_TransMatrix[4] = (float)(-MapL * m_TransMatrix[0]);
                //dx
                m_TransMatrix[5] = (float)(-MapT * m_TransMatrix[3]);
                //dy
            }


            catch (System.OverflowException ex)
            {
                //Do something with the error or ignore it.
            }

        }

        private Matrix GetTransMatrix()
        {
            //*** Create the Transformation Matrix from Matrix Elemental Values
            return new Matrix(m_TransMatrix[0], m_TransMatrix[1], m_TransMatrix[2], m_TransMatrix[3], m_TransMatrix[4], m_TransMatrix[5]);
        }

        private RectangleF GetCurrentExtent()
        {
            RectangleF RetVal = new RectangleF();
            RetVal.X = 0 / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0]);
            RetVal.Y = (m_Height / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3]);
            RetVal.Width = Math.Abs(RetVal.X - (m_Width / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0])));
            RetVal.Height = Math.Abs(((0 / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3])) - RetVal.Y);
            return RetVal;
        }

        public void SetFullExtent()
        {
            //*** Full extent of map will always be reset when a layer is added or removed
            int i;
            RectangleF TempExt;
            float MinX = 0;
            float MinY = 0;
            float MaxX = 0;
            float MaxY = 0;
            for (i = 0; i <= Layers.Count - 1; i++)
            {
                TempExt = Layers[i].Extent;
                if (i == 0)
                {
                    MinX = TempExt.X;
                    MinY = TempExt.Top;
                    MaxX = TempExt.X + TempExt.Width;
                    MaxY = TempExt.Top + TempExt.Height;
                }
                else
                {
                    MinX = Math.Min(MinX, TempExt.X);
                    MinY = Math.Min(MinY, TempExt.Top);
                    MaxX = Math.Max(MaxX, TempExt.X + TempExt.Width);
                    MaxY = Math.Max(MaxY, TempExt.Top + TempExt.Height);
                }
            }

            m_FullExtent.X = MinX;
            m_FullExtent.Y = MaxY;
            m_FullExtent.Width = Math.Abs(MaxX - MinX);
            m_FullExtent.Height = Math.Abs(MaxY - MinY);

            if (this.Insets.SelIndex == -1)
            {
                m_CurrentExtent = m_FullExtent;
            }
            else
            {
                m_CurrentExtent = this.Insets[Insets.SelIndex].Extent;
            }


        }


        public PointF PointToClient(PointF Pt)
        {
            PointF RetVal = new PointF();
            RetVal.X = Pt.X / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0]);
            RetVal.Y = (Pt.Y / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3]);
            return RetVal;
        }

        public PointF PointToClient(float X, float Y)
        {
            PointF RetVal = new PointF();
            RetVal.X = X / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0]);
            RetVal.Y = (Y / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3]);
            return RetVal;
        }

        public double GetDistance(double dLat1, double dLong1, double dLat2, double dLong2)
        {
            double Cosd;
            double dMiles;
            double d;
            double Lat1;
            double Long1;
            double Lat2;
            double Long2;
            double cEarth = 3959;
            //StatuteMiles

            Lat1 = dLat1 * (Math.PI / 180);
            Long1 = dLong1 * (Math.PI / 180);
            Lat2 = dLat2 * (Math.PI / 180);
            Long2 = dLong2 * (Math.PI / 180);

            Cosd = Math.Sin(Lat1) * Math.Sin(Lat2) + Math.Cos(Lat1) * Math.Cos(Lat2) * Math.Cos(Long1 - Long2);

            d = Math.Acos(Cosd);
            dMiles = cEarth * d;
            double dStatuteMiles;
            double dKilometers;
            dStatuteMiles = Conversion.Int(dMiles * 100 + 0.5) / 100;
            dKilometers = dStatuteMiles * 1.6093470879;
            dKilometers = Conversion.Int(dKilometers * 100 + 0.5) / 100;
            return dKilometers;
        }
        #endregion

        #region " Zoom Pan FullExtent"

        public void zoom(int zX1, int zY1, int zX2, int zY2)
        {
            float MinX;
            float MinY;
            float MaxX;
            float MaxY;
            MinX = zX1 / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0]);
            MaxX = zX2 / m_TransMatrix[0] - (m_TransMatrix[4] / m_TransMatrix[0]);
            MinY = (zY2 / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3]);
            MaxY = (zY1 / m_TransMatrix[3]) - (m_TransMatrix[5] / m_TransMatrix[3]);

            //*** Set the map extent. Check for min width and height for . else preserve previous width and height
            //*** For web version this logic should be implemented on client side by java script
            if (Math.Abs(MaxX - MinX) > 0.002 & Math.Abs(MaxY - MinY) > 0.002)
            {
                m_CurrentExtent.X = Math.Min(MinX, MaxX);
                //To handle zoom bound formed by TL-BR drag or BR-TL drag
                m_CurrentExtent.Y = Math.Max(MaxY, MinY);
                m_CurrentExtent.Width = Math.Abs(MaxX - MinX);
                m_CurrentExtent.Height = Math.Abs(MaxY - MinY);
            }

        }

        public void zoom(float ZoomFactor)
        {
            zoom(ZoomFactor, 0, 0);
        }

        public void zoom(float ZoomFactor, float zX)
        {
            zoom(ZoomFactor, 0, 0);
        }

        public void zoom(float ZoomFactor, float zX, float zY)
        {
            double AddX;
            double AddY;
            AddX = m_CurrentExtent.Width * (1 / ZoomFactor - 1) / 2;
            AddY = m_CurrentExtent.Height * (1 / ZoomFactor - 1) / 2;
            m_CurrentExtent.X = m_CurrentExtent.Left - (float)AddX;
            m_CurrentExtent.Y = m_CurrentExtent.Top + (float)AddY;
            m_CurrentExtent.Width = m_CurrentExtent.Width + (float)AddX * 2;
            m_CurrentExtent.Height = m_CurrentExtent.Height + (float)AddY * 2;


            RectangleF InsetExtent;
            if (m_Insets.SelIndex == -1)
            {
                InsetExtent = m_FullExtent;
            }
            else
            {
                InsetExtent = m_Insets[Insets.SelIndex].Extent;
            }

            //*** Check that the new calculated extent is contained within full extent
            if ((m_CurrentExtent.Left) < InsetExtent.Left)
                m_CurrentExtent.X = InsetExtent.Left;
            if ((m_CurrentExtent.Top) > InsetExtent.Top)
                m_CurrentExtent.Y = InsetExtent.Top;
            if ((m_CurrentExtent.Width) > InsetExtent.Width)
                m_CurrentExtent.Width = InsetExtent.Width;
            if ((m_CurrentExtent.Height) > InsetExtent.Height)
                m_CurrentExtent.Height = InsetExtent.Height;
        }

        public void ZoomFullExtent()
        {
            SetFullExtent();
        }

        public void ZoomToLayer(int LayerIndex)
        {
            {
                m_CurrentExtent.X = m_Layers[LayerIndex].Extent.X;
                m_CurrentExtent.Y = m_Layers[LayerIndex].Extent.Bottom;
                m_CurrentExtent.Width = m_Layers[LayerIndex].Extent.Width;
                m_CurrentExtent.Height = m_Layers[LayerIndex].Extent.Height;
            }
        }

        public void ZoomToSelection()
        {
            int i;
            RectangleF ZoomExt = new RectangleF();
            RectangleF TempExt = new RectangleF();
            StringCollection _SelAreas;
            Shape _Shape;
            Hashtable _Records;

            SizeF PointSize = new SizeF(0, 0);
            PointSize.Width = PointToClient(0, 0).X;
            PointSize.Height = PointToClient(6, 0).X;
            PointSize.Width = Math.Abs(PointSize.Width - PointSize.Height);
            PointSize.Height = PointSize.Width;

            foreach (Layer _Layer in this.Layers)
            {
                _SelAreas = _Layer.SelectedArea;
                _Records = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID);

                if (_SelAreas.Count > 0)
                {
                    for (i = 0; i <= _SelAreas.Count - 1; i++)
                    {
                        _Shape = (Shape)_Records[_SelAreas[i]];
                        switch (_Layer.LayerType)
                        {
                            case ShapeType.Point:
                            case ShapeType.PointCustom:
                            case ShapeType.PointFeature:
                                TempExt.Location = (PointF)_Shape.Parts[0];
                                TempExt.Size = PointSize;
                                break;
                            case ShapeType.Polygon:
                            case ShapeType.PolygonCustom:
                            case ShapeType.PolygonBuffer:
                            case ShapeType.PolygonFeature:
                                TempExt = _Shape.Extent;
                                break;
                        }
                        if (ZoomExt.IsEmpty)
                        {
                            ZoomExt = TempExt;
                        }
                        else
                        {
                            ZoomExt = RectangleF.Union(ZoomExt, TempExt);
                        }
                    }
                }
            }

            //*** Set Map Extent to union extent of all maps
            if (!ZoomExt.IsEmpty)
            {
                m_CurrentExtent.X = ZoomExt.X;
                m_CurrentExtent.Y = ZoomExt.Bottom;
                m_CurrentExtent.Width = ZoomExt.Width;
                m_CurrentExtent.Height = ZoomExt.Height;
            }

            //*** If Inset Map is currently selected then limit the select shapes extent within Inset Map
            if (m_Insets.SelIndex > -1)
            {
                //*** Inset Map
                Inset _Inset = m_Insets[m_Insets.SelIndex];
                if (_Inset.Extent.X > m_CurrentExtent.X)
                    m_CurrentExtent.X = _Inset.Extent.X;
                if (_Inset.Extent.Y < m_CurrentExtent.Y)
                    m_CurrentExtent.Y = _Inset.Extent.Y;
                if (_Inset.Extent.Width < m_CurrentExtent.Width)
                    m_CurrentExtent.Width = _Inset.Extent.Width;
                if (_Inset.Extent.Height < m_CurrentExtent.Height)
                    m_CurrentExtent.Height = _Inset.Extent.Height;
                _Inset = null;
            }
        }

        public void Pan(int pX1, int pY1, int pX2, int pY2)
        {
            //*** pX1,pY1... are picture/image coordinates interms of pixel
            //*** Assuming flow from x1,y1 to x2,y2
            double dX;
            double dY;
            //d = Alt+235 d
            RectangleF InsetExtent;

            if (m_Insets.SelIndex == -1)
            {
                InsetExtent = m_FullExtent;
            }
            else
            {
                InsetExtent = m_Insets[Insets.SelIndex].Extent;
            }

            dX = (pX1 - pX2) / m_Width * m_CurrentExtent.Width;
            dY = (pY1 - pY2) / m_Height * m_CurrentExtent.Height;

            //*** Bugfix 09 Mar 2007 Pan does not works precisely
            double ExtRat;
            double PicRat;
            double ScaleRat;
            ExtRat = m_CurrentExtent.Width / m_CurrentExtent.Height;
            PicRat = m_Width / m_Height;
            ScaleRat = ExtRat / PicRat;
            if (ExtRat >= PicRat)
            {
                dY = dY * ScaleRat;
            }
            else
            {
                dX = dX / ScaleRat;
            }

            m_CurrentExtent.X = m_CurrentExtent.Left + (float)dX;
            m_CurrentExtent.Y = m_CurrentExtent.Top - (float)dY;

            //*** Check that Full Extent is not exceeded
            if (m_CurrentExtent.Left < InsetExtent.Left)
            {
                m_CurrentExtent.X = InsetExtent.Left;
            }
            else if (m_CurrentExtent.Left + m_CurrentExtent.Width > InsetExtent.Left + InsetExtent.Width)
            {
                m_CurrentExtent.X = InsetExtent.Left + InsetExtent.Width - m_CurrentExtent.Width;
            }

            if (m_CurrentExtent.Top > InsetExtent.Top)
            {
                m_CurrentExtent.Y = InsetExtent.Top;
            }
            else if (m_CurrentExtent.Top - m_CurrentExtent.Height < InsetExtent.Top - InsetExtent.Height)
            {
                m_CurrentExtent.Y = InsetExtent.Top - InsetExtent.Height + m_CurrentExtent.Height;
            }
        }
        #endregion

        #region "  Rendering Functions"



        /// <summary>
        /// This Updates theme legends in isolation, without affecting rendering information
        /// </summary>
        public void UpdateThemeLegend(bool p_CompositeLegend, int p_Area_Level, int themeIndex, bool preserveMinMax)
        {
            Theme _Theme = null;

            if (themeIndex != -1)
            {
                _Theme = m_Themes[themeIndex];
            }

            DataView _DvTheme = null;

            //-- Update legends only for break based themes
            if ((_Theme != null) && _Theme.Type != ThemeType.Chart && _Theme.Type != ThemeType.DotDensity)
            {
                // Set IUS + Area + Numeric data filter on presentation data / Mrd data
                string sRowFilter = string.Empty;

                sRowFilter = Indicator.IndicatorNId + " IN (" + _Theme.IndicatorNId[0].ToString() + ") AND " + Unit.UnitNId + " IN (" + _Theme.UnitNId.ToString() + ")";
                if (_Theme.SubgroupNId[0].ToString() != "-1")
                {
                    sRowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + _Theme.SubgroupNId[0].ToString() + ")";
                }

                // Filter for Area
                if (p_Area_Level != -1)
                {
                    sRowFilter += " AND " + Area.AreaLevel + "=" + p_Area_Level.ToString();
                }

                if (p_CompositeLegend == true) // TimeSeries is ON Consider all TimePeriod records for Theme IUS
                {
                    //-- Composite Legend
                    this.PresentationData.RowFilter = sRowFilter;
                    _DvTheme = new DataView(this.PresentationData.ToTable());
                    this.PresentationData.RowFilter = "";
                }
                else
                {
                    //-- MRD Legend - when timeSeries is OFF Consider only MRD records for Theme IUS
                    this.MRDData.RowFilter = sRowFilter;
                    _DvTheme = new DataView(this.MRDData.ToTable());
                    this.MRDData.RowFilter = "";

                }
                // Sort on Numeric DataValues
                _DvTheme.Sort = DataExpressionColumns.DataType + " ASC," + DataExpressionColumns.NumericData + " ASC";
                if (preserveMinMax)
                {
                    _Theme.UpdateThemeLegend(_DvTheme, _Theme.Minimum, _Theme.Maximum);

                }
                else
                {
                    _Theme.UpdateThemeLegend(_DvTheme, Convert.ToDecimal(_DvTheme[0][DataExpressionColumns.NumericData]), Convert.ToDecimal(_DvTheme[_DvTheme.Count - 1][DataExpressionColumns.NumericData]));

                }

            }


        }

        /// <summary>
        /// This updates rendering info in isolation without affecting theme legends.
        /// <para>(Updating RenderingInfo process : assigning Legend indexes to each Areas on the basis of the rendering dataValue.)</para>
        /// </summary>
        /// <param name="p_TimePeriod"></param>
        /// <param name="p_Area_Level"></param>
        public void UpdateRenderingInfo(int p_TimePeriod, int p_Area_Level, int themeIndex)
        {

            Theme _Theme = null;
            DataView _DvTheme = null;

            if (themeIndex != -1)
            {
                _Theme = m_Themes[themeIndex];
            }

            if (_Theme != null)
            {
                if (_Theme.Type != ThemeType.Chart)
                {
                    //*** Recreate Ranges based on selected AreaLevel and TimePeriod
                    string sRowFilter = string.Empty;

                    // Filter for IUS TODO Confirm if Subgroupval can be blank
                    sRowFilter = Indicator.IndicatorNId + " IN (" + _Theme.IndicatorNId[0].ToString() + ") AND " + Unit.UnitNId + " IN (" + _Theme.UnitNId.ToString() + ")";
                    if (_Theme.SubgroupNId[0].ToString() != "-1")
                    {
                        sRowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + _Theme.SubgroupNId[0].ToString() + ")";
                    }

                    // Filter for Area
                    if (p_Area_Level != -1)
                    {
                        sRowFilter += " AND " + Area.AreaLevel + "=" + p_Area_Level.ToString();
                    }

                    //Filter for TimePeriod
                    if (p_TimePeriod != -1)
                    {
                        sRowFilter += " AND " + Timeperiods.TimePeriodNId + "=" + p_TimePeriod.ToString();

                        //In case of Timeperiod use presentation data
                        this.PresentationData.RowFilter = sRowFilter;
                        _DvTheme = new DataView(this.PresentationData.ToTable());
                        this.PresentationData.RowFilter = "";
                    }
                    else
                    {
                        // use MRD data
                        this.MRDData.RowFilter = sRowFilter;
                        _DvTheme = new DataView(this.MRDData.ToTable());
                        this.MRDData.RowFilter = "";
                    }


                    //*** Set Visibilty of Base Layers based on selected TimePeriod and AreaLevel 
                    //_Theme.LayerVisibility.Clear();
                    //GenerateLayerInformation(ref _Theme, ref _DvTheme);
                    foreach (Layer _Layer in m_Layers)
                    {
                        if (_Layer.LayerType == ShapeType.Point || _Layer.LayerType == ShapeType.Polygon || _Layer.LayerType == ShapeType.PolyLine)
                        {
                            _Theme.LayerVisibility[_Layer.ID] = (p_Area_Level > -1 ? (_Layer.Area_Level == p_Area_Level) : true);//_Theme.LayerVisibility[_Layer.ID]
                        }
                    }


                    if (_DvTheme.Count > 0)
                    {
                        _Theme.UpdateRenderingInfo(_DvTheme);
                    }
                    else
                    {
                        object[] _Keys = new object[_Theme.AreaIndexes.Keys.Count];
                        _Theme.AreaIndexes.Keys.CopyTo(_Keys, 0);

                        foreach (string _Key in _Keys)
                        {
                            AreaInfo _AreaInfo = (AreaInfo)_Theme.AreaIndexes[_Key];
                            _AreaInfo.RenderingInfo = _Theme.Legends.Count - 1;
                            _AreaInfo.DataValue = 0;    //- Setting '0' so Dot Density could not draw Dot for zero Value.
                            _AreaInfo.DisplayInfo = string.Empty;
                            _Theme.AreaIndexes[_Key] = _AreaInfo;

                        }
                    }

                }
                else if (_Theme.Type == ThemeType.Chart)
                {
                    this.UpdateChartMostRecentData(_Theme, p_TimePeriod);
                }
            }
        }

        /// <summary>
        /// It updates the Chart Theme's MOST recent data information for specified TimePeriod.
        /// </summary>
        /// <param name="_Theme">Chart Theme instance</param>
        /// <param name="p_TimePeriod"></param>
        private void UpdateChartMostRecentData(Theme _Theme, int p_TimePeriod)
        {
            //-- Allow Updating Data Only for Column & Pie Chart
            // as Line Chart works only for Time Series
            if (_Theme.Type == ThemeType.Chart && _Theme.ChartType != ChartType.Line)
            {
                //Get TimePeriod by NId
                string TimePeriod = string.Empty;
                DataRow[] DRow = this._DIDataView.MainDataTable.Select(Timeperiods.TimePeriodNId + " = " + p_TimePeriod);
                if (DRow.Length > 0)
                {
                    TimePeriod = DRow[0][Timeperiods.TimePeriod].ToString();
                }

                object[] _Keys = new object[_Theme.AreaIndexes.Keys.Count];
                _Theme.AreaIndexes.Keys.CopyTo(_Keys, 0);


                foreach (string _Key in _Keys)
                {
                    AreaInfo _AreaInfo = (AreaInfo)_Theme.AreaIndexes[_Key];
                    if (string.IsNullOrEmpty(TimePeriod) == false)
                    {
                        // Update MostRecent Data with the Chart Data of specified TimePeriod
                        if (_AreaInfo.ChartData.ContainsKey(TimePeriod))
                        {
                            _AreaInfo.ChartMostRecentData = _AreaInfo.ChartData[TimePeriod].ToString();
                        }
                        else
                        {
                            _AreaInfo.ChartMostRecentData = string.Empty;
                        }
                    }
                    else
                    {
                        //-- Restore original Chart's MOST recent Data of every Area                        
                        _AreaInfo.ChartMostRecentData = _AreaInfo.ChartMostRecentDataCopy;
                    }

                    _Theme.AreaIndexes[_Key] = _AreaInfo;
                }
            }
        }

        public void DrawMap(Graphics g, string areaIDToHightlight)
        {
            this.AreaIDToHighlight = areaIDToHightlight;
            Layers PreservedLayeres = this.m_Layers;

            if (this.m_Layers != null)
            {
                try
                {
                    this.m_Layers = new Layers();

                    if (this.PreviousLayerHighlighted != null)
                    {
                        this.m_Layers.Add(PreviousLayerHighlighted);
                    }

                    //- get Layer for given AreaID
                    foreach (Layer Lyr in PreservedLayeres)
                    {
                        Shape _Shape = null;
                        Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                        IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                        if (Lyr.LayerType == ShapeType.Polygon)
                        {
                            while (dicEnumerator.MoveNext())
                            {
                                //Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;

                                if (_Shape.AreaId == areaIDToHightlight)
                                {
                                    if (this.m_Layers.LayerIndex(Lyr.ID) < 0)
                                    {
                                        this.m_Layers.Add(Lyr);

                                        //- Set Layer as previous Highlighted Area Layer.
                                        this.PreviousLayerHighlighted = Lyr;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    // Re-draw Map only for selected Layer associated with AreaID
                    this.DrawMap("", g);
                }
                catch
                { }
            }

            this.AreaIDToHighlight = string.Empty;
            this.m_Layers = PreservedLayeres;

            try
            {
                //*** Draw Label
                DrawLabel(ref g, this.GetTransMatrix(), this.GetCurrentExtent());
            }
            catch
            { }
        }

        public void DrawMap(string p_FileName, Graphics g)
        {
            try
            {
                //*** Create New Graphics object for web version, For Desktop version Graphics Object of a control shall be passed
                System.Drawing.Bitmap _BitMap;
                if (p_FileName.Length > 0)
                {
                    _BitMap = new System.Drawing.Bitmap((int)m_Width, (int)m_Height);
                    g = Graphics.FromImage(_BitMap);
                }
                else
                { _BitMap = null; }   // _BitMap is assigned some default values but has no relevance.

                //- Allow AntiAliasing for smooth Text Labels
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;


                //*** Set the background color of Graphics
                if (m_CanvasColor.ToArgb() != Color.Transparent.ToArgb())
                    g.Clear(m_CanvasColor);
                //*** Clearing with transparent color sets the backgolor to black in web

                //*** Set Transformation Variables
                SetTransVar();
                Matrix mTransMatrix = GetTransMatrix();
                RectangleF CurExt = GetCurrentExtent();

                //*** Draw Themes (Base layer + Feature Layer)
                Theme _Theme;
                _Theme = m_Themes.GetActiveTheme();
                //$$$ Assuming that at any point of time Only one Color Theme will be visible
                if ((_Theme == null))
                {
                    DrawBaseLayer(ref g, mTransMatrix, CurExt);
                    //*** Base Layers '*** If no color theme then draw boundaries of map
                }



                foreach (Theme _TempTheme in Themes)
                {
                    if (_TempTheme.Visible == true)
                    {
                        switch (_TempTheme.Type)
                        {
                            case ThemeType.Color:
                            case ThemeType.Hatch:
                                DrawColorTheme(ref g, _TempTheme, mTransMatrix, CurExt);
                                //*** Color Theme + Hatch Theme
                                break;
                            case ThemeType.DotDensity:
                                DrawDotDensity(ref g, _TempTheme, mTransMatrix, CurExt);
                                //*** Dot Density
                                break;
                            case ThemeType.Chart:
                                DrawChart(ref g, _TempTheme, mTransMatrix, CurExt);
                                //*** Chart Theme
                                break;
                            case ThemeType.Symbol:
                                DrawSymbolTheme(ref g, _TempTheme, mTransMatrix, CurExt);
                                //*** Symbol Theme
                                break;
                            case ThemeType.Label:
                                DrawLabelTheme(ref g, _TempTheme, mTransMatrix, CurExt);
                                //*** Label Theme
                                break;
                        }
                    }
                }




                //*** Draw Custom Feature Layers
                if (m_CFLCol.Count > 0)
                {
                    DrawCustomFeatureLayers(ref g, mTransMatrix, CurExt);
                }

                //*** Draw North Symbol only for Main Map not for Insets
                g.SmoothingMode = SmoothingMode.AntiAlias;
                if (m_NorthSymbol == true)
                {
                    Font _Font = new Font("Webdings", m_NorthSymbolSize);
                    SolidBrush _Brush = new SolidBrush(m_NorthSymbolColor);
                    //g.DrawString("l", _Font, _Brush, m_NorthSymbolPosition.X, m_NorthSymbolPosition.Y)
                    g.DrawString("l", _Font, _Brush, m_Width - 60, m_Height - 60);
                    _Font.Dispose();
                    _Brush.Dispose();
                }
                g.SmoothingMode = SmoothingMode.None;


                //*** Display Map Scale Enhancement 11 May 2006
                try
                {
                    if (m_Scale == true)
                    {
                        int iScaleWidth = (int)(m_Width / 2.5);
                        int iScaleHeight = 5;
                        int iBlockCount = 5;
                        int iBlockWidth = (int)iScaleWidth / iBlockCount;
                        int iOrgX = 10;
                        int iOrgY = (int)(m_Height - 20);
                        double dMapWidth;
                        double dBlockWidth;

                        {
                            dMapWidth = GetDistance(m_CurrentExtent.X, m_CurrentExtent.Y, m_CurrentExtent.X + m_CurrentExtent.Width, m_CurrentExtent.Y);
                        }

                        dBlockWidth = dMapWidth / m_Width * iBlockWidth;
                        if (dBlockWidth > 2)
                        {
                            dBlockWidth = (int)dBlockWidth;
                        }
                        else
                        {
                            dBlockWidth = Math.Round(dBlockWidth, 1);
                        }

                        //*** Create array of rectagle to be filled with alternate colors
                        Rectangle[] Rect1 = new Rectangle[3];
                        Rectangle[] Rect2 = new Rectangle[2];
                        Rect1[0] = new Rectangle(iOrgX, iOrgY, iBlockWidth, iScaleHeight);
                        Rect2[0] = new Rectangle(iOrgX + iBlockWidth, iOrgY, iBlockWidth, iScaleHeight);
                        Rect1[1] = new Rectangle(iOrgX + iBlockWidth * 2, iOrgY, iBlockWidth, iScaleHeight);
                        Rect2[1] = new Rectangle(iOrgX + iBlockWidth * 3, iOrgY, iBlockWidth, iScaleHeight);
                        Rect1[2] = new Rectangle(iOrgX + iBlockWidth * 4, iOrgY, iBlockWidth, iScaleHeight);

                        //*** Fill alternate band of rectangles
                        g.FillRectangles(Brushes.Red, Rect1);
                        g.FillRectangles(Brushes.Blue, Rect2);

                        //*** Draw Text for Scale measurements
                        Font oFont = new Font("Arial", 7);
                        StringFormat oStringFormat = new StringFormat();
                        oStringFormat.FormatFlags = StringFormatFlags.NoClip;
                        oStringFormat.Alignment = StringAlignment.Center;
                        g.DrawString(m_ScaleUnitText, oFont, Brushes.Black, iOrgX, iOrgY + iScaleHeight + 1, oStringFormat);
                        g.DrawString(dBlockWidth.ToString(), oFont, Brushes.Black, iOrgX + iBlockWidth, iOrgY + iScaleHeight + 1, oStringFormat);
                        g.DrawString((dBlockWidth * 2).ToString(), oFont, Brushes.Black, iOrgX + iBlockWidth * 2, iOrgY + iScaleHeight + 1, oStringFormat);
                        g.DrawString((dBlockWidth * 3).ToString(), oFont, Brushes.Black, iOrgX + iBlockWidth * 3, iOrgY + iScaleHeight + 1, oStringFormat);
                        g.DrawString((dBlockWidth * 4).ToString(), oFont, Brushes.Black, iOrgX + iBlockWidth * 4, iOrgY + iScaleHeight + 1, oStringFormat);
                        g.DrawString((dBlockWidth * 5).ToString(), oFont, Brushes.Black, iOrgX + iBlockWidth * 5, iOrgY + iScaleHeight + 1, oStringFormat);
                        oFont.Dispose();

                    }
                }
                catch (Exception ex)
                {

                }

                //*** Draw Label
                DrawLabel(ref g, mTransMatrix, CurExt);

                //*** For Web Version Save Image File to the specified path
                if (p_FileName.Length > 0)
                {
                    //_BitMap.MakeTransparent(Color.Transparent)
                    if (File.Exists(p_FileName))
                        File.Delete(p_FileName);
                    FileStream _File = new FileStream(p_FileName, FileMode.Create);
                    _BitMap.Save(_File, ImageFormat.Png);
                    _File.Flush();
                    _File.Close();
                    //_BitMap.Save(p_FileName, Imaging.ImageFormat.Png)
                    _BitMap.Dispose();
                    g.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        private void DrawBaseLayer(ref Graphics g, Matrix mTransMatrix, RectangleF CurExt)
        {

            Shape _Shape;
            //Dim PnLayer As New Pen(Color.LightGray)
            HatchBrush BrSelection = new HatchBrush(HatchStyle.Percent40, m_SelectionColor, Color.Transparent);
            Pen PnSelection = new Pen(m_SelectionColor);

            GraphicsPath gpShp = new GraphicsPath();
            GraphicsPath gpSelShp = new GraphicsPath();
            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            int j;
            foreach (Layer _Layer in Layers)
            {
                //Traverse Layers collection
                {
                    if (_Layer.Extent.IntersectsWith(CurExt) & _Layer.Visible == true)
                    {
                        // Render layer only if it lies within current map extent

                        Hashtable ht = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID);
                        IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                        switch (_Layer.LayerType)
                        {
                            case ShapeType.Polygon:
                            case ShapeType.PolygonFeature:
                            case ShapeType.PolygonCustom:
                            case ShapeType.PolygonBuffer:
                                Pen PnLayer = new Pen(_Layer.BorderColor, _Layer.BorderSize);
                                if (_Layer.BorderSize == 0)
                                    PnLayer.Color = Color.Transparent;

                                //??? Strange that setting Pen width = 0 gives effect of Pen width 1. So this workaround
                                PnLayer.DashStyle = _Layer.BorderStyle;

                                Brush BrLayer;
                                if (_Layer.FillStyle == FillStyle.Solid)
                                {
                                    BrLayer = new SolidBrush(_Layer.FillColor);
                                }
                                else if (_Layer.FillStyle == FillStyle.Transparent)
                                {
                                    BrLayer = new SolidBrush(Color.Transparent);
                                }
                                else
                                {
                                    BrLayer = new HatchBrush((HatchStyle)_Layer.FillStyle, _Layer.FillColor, Color.Transparent);
                                }


                                //While dicEnumerator.MoveNext()
                                // _Shape = dicEnumerator.Value
                                // If _Shape.Extent.IntersectsWith(CurExt) Then
                                // gpShp.Reset()
                                // For j = 0 To _Shape.Parts.Count - 1
                                // gpShp.AddPolygon(_Shape.Parts(j))
                                // Next
                                // gpShp.Transform(mTransMatrix)
                                // If _Layer.LayerType <> ShapeType.Polygon Then g.FillPath(BrLayer, gpShp)
                                // g.DrawPath(PnLayer, gpShp)
                                // If .SelectedArea.Contains(_Shape.AreaId) Then g.FillPath(BrSelection, gpShp)
                                // End If
                                //End While


                                gpShp.Reset();
                                gpShp.FillMode = FillMode.Winding;
                                gpSelShp.Reset();
                                gpSelShp.FillMode = FillMode.Winding;
                                while (dicEnumerator.MoveNext())
                                {
                                    _Shape = (Shape)dicEnumerator.Value;
                                    if (_Shape.Extent.IntersectsWith(CurExt))
                                    {
                                        for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                        {
                                            gpShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                            if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                                gpSelShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                        }
                                    }
                                }

                                gpShp.Transform(mTransMatrix);

                                if (_Layer.LayerType != ShapeType.Polygon)
                                {
                                    g.FillPath(BrLayer, gpShp);
                                }

                                try
                                {
                                    g.DrawPath(PnLayer, gpShp);
                                }
                                catch (Exception ex)
                                {
                                    Console.Write(ex.Message);
                                    //??? AFRERI Eritria problem
                                }

                                if (_Layer.SelectedArea.Count > 0)
                                {
                                    gpSelShp.Transform(mTransMatrix);
                                    g.FillPath(BrSelection, gpSelShp);
                                }


                                BrLayer.Dispose();
                                PnLayer.Dispose();
                                break;

                            case ShapeType.PolyLine:
                            case ShapeType.PolyLineFeature:
                            case ShapeType.PolyLineCustom:
                                gpShp.Reset();
                                while (dicEnumerator.MoveNext())
                                {
                                    _Shape = (Shape)dicEnumerator.Value;
                                    for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                    {
                                        gpShp.StartFigure();
                                        gpShp.AddLines((PointF[])_Shape.Parts[j]);
                                    }
                                }
                                PnLayer = null;
                                PnLayer = new Pen(_Layer.BorderColor, _Layer.BorderSize);
                                PnLayer.DashStyle = _Layer.BorderStyle;
                                gpShp.Transform(mTransMatrix);
                                g.DrawPath(PnLayer, gpShp);
                                PnLayer.Dispose();
                                break;

                            case ShapeType.Point:
                            case ShapeType.PointFeature:
                            case ShapeType.PointCustom:
                                g.SmoothingMode = SmoothingMode.AntiAlias;
                                PnLayer = new Pen(_Layer.FillColor, 0.01F);
                                BrLayer = new SolidBrush(_Layer.FillColor);
                                PointF[] Pt = new PointF[1];
                                int PtSize = _Layer.MarkerSize;
                                gpShp.Reset();
                                gpSelShp.Reset();
                                if (_Layer.MarkerStyle == MarkerStyle.Circle)
                                {
                                    while (dicEnumerator.MoveNext())
                                    {
                                        _Shape = (Shape)dicEnumerator.Value;
                                        Pt[0] = (PointF)_Shape.Parts[0];
                                        mTransMatrix.TransformPoints(Pt);
                                        if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                        {
                                            gpSelShp.AddEllipse((int)Pt[0].X - PtSize / 2, (int)Pt[0].Y - PtSize / 2, PtSize, PtSize);
                                        }
                                        else
                                        {
                                            gpShp.AddEllipse((int)Pt[0].X - PtSize / 2, (int)Pt[0].Y - PtSize / 2, PtSize, PtSize);
                                        }
                                    }
                                }
                                else if (_Layer.MarkerStyle == MarkerStyle.Square)
                                {
                                    while (dicEnumerator.MoveNext())
                                    {
                                        _Shape = (Shape)dicEnumerator.Value;
                                        Pt[0] = (PointF)_Shape.Parts[0];
                                        mTransMatrix.TransformPoints(Pt);
                                        if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                        {
                                            gpSelShp.AddRectangle(new RectangleF(Pt[0].X - PtSize / 2, Pt[0].Y - PtSize / 2, PtSize, PtSize));
                                        }
                                        else
                                        {
                                            gpShp.AddRectangle(new RectangleF(Pt[0].X - PtSize / 2, Pt[0].Y - PtSize / 2, PtSize, PtSize));
                                        }
                                    }
                                }
                                else if (_Layer.MarkerStyle == MarkerStyle.Triangle)
                                {
                                    //*** 1.Equilateral triangle with altitude = PtSize 2.Equilateral triangle with sides = PtSize
                                    PointF[] Vertex = new PointF[3];
                                    PtSize = (int)(Math.Sqrt(3) / 4 * PtSize);
                                    while (dicEnumerator.MoveNext())
                                    {
                                        _Shape = (Shape)dicEnumerator.Value;
                                        Pt[0] = (PointF)_Shape.Parts[0];
                                        mTransMatrix.TransformPoints(Pt);
                                        Vertex[0] = new PointF(Pt[0].X - PtSize, Pt[0].Y + PtSize);
                                        Vertex[1] = new PointF(Pt[0].X + PtSize, Pt[0].Y + PtSize);
                                        Vertex[2] = new PointF(Pt[0].X, Pt[0].Y - PtSize);
                                        if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                        {
                                            gpSelShp.AddPolygon(Vertex);
                                        }
                                        else
                                        {
                                            gpShp.AddPolygon(Vertex);
                                        }
                                    }
                                    Vertex = null;
                                }
                                else if (_Layer.MarkerStyle == MarkerStyle.Cross)
                                {
                                    while (dicEnumerator.MoveNext())
                                    {
                                        //*** Traverse Shapes
                                        _Shape = (Shape)dicEnumerator.Value;
                                        Pt[0] = (PointF)_Shape.Parts[0];
                                        mTransMatrix.TransformPoints(Pt);
                                        if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                        {
                                            gpSelShp.AddLine(new PointF(Pt[0].X - PtSize / 2, Pt[0].Y), new PointF(Pt[0].X + PtSize / 2, Pt[0].Y));
                                            gpSelShp.CloseFigure();
                                            gpSelShp.AddLine(new PointF(Pt[0].X, Pt[0].Y + PtSize / 2), new PointF(Pt[0].X, Pt[0].Y - PtSize / 2));
                                            gpSelShp.CloseFigure();
                                        }
                                        else
                                        {
                                            gpShp.AddLine(new PointF(Pt[0].X - PtSize / 2, Pt[0].Y), new PointF(Pt[0].X + PtSize / 2, Pt[0].Y));
                                            gpShp.CloseFigure();
                                            gpShp.AddLine(new PointF(Pt[0].X, Pt[0].Y + PtSize / 2), new PointF(Pt[0].X, Pt[0].Y - PtSize / 2));
                                            gpShp.CloseFigure();
                                        }
                                    }
                                }
                                else if (_Layer.MarkerStyle == MarkerStyle.Custom)
                                {
                                    Font _MFnt = new Font(_Layer.MarkerFont.FontFamily, PtSize);
                                    while (dicEnumerator.MoveNext())
                                    {
                                        //*** Traverse Shapes
                                        _Shape = (Shape)dicEnumerator.Value;
                                        Pt[0] = (PointF)_Shape.Parts[0];
                                        mTransMatrix.TransformPoints(Pt);
                                        if (_Layer.SelectedArea.Contains(_Shape.AreaId))
                                        {
                                            g.DrawString((string)_Layer.MarkerChar.ToString(), _MFnt, BrSelection, Pt[0].X, Pt[0].Y, _StringFormat);

                                        }
                                        else
                                        {
                                            g.DrawString(_Layer.MarkerChar.ToString(), _MFnt, BrLayer, Pt[0].X, Pt[0].Y, _StringFormat);
                                        }
                                    }
                                    _MFnt.Dispose();
                                }

                                g.FillPath(BrLayer, gpShp);
                                g.DrawPath(PnLayer, gpShp);
                                g.FillPath(BrSelection, gpSelShp);
                                g.DrawPath(PnSelection, gpSelShp);
                                Pt = null;
                                PnLayer.Dispose();
                                BrLayer.Dispose();
                                g.SmoothingMode = SmoothingMode.None;
                                break;
                        }
                    }
                }
            }
            gpShp.Dispose();
            gpSelShp.Dispose();
            _StringFormat.Dispose();
            BrSelection.Dispose();
            PnSelection.Dispose();
            // _Layer = null;
            _Shape = null;

        }
        private void DrawColorTheme(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt)
        {

            Legend _Legend;
            Shape _Shape;
            //Polygon : Polyline : Point
            int i;
            int j;
            GraphicsPath gpShp = new GraphicsPath();
            GraphicsPath gpSelShp = new GraphicsPath();
            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            bool BaseLyrVisibility = false;
            //*** Set an array of brush based on current theme legend items
            Brush[] BrLegend = new Brush[_Theme.Legends.Count];
            for (i = 0; i <= _Theme.Legends.Count - 1; i++)
            {
                _Legend = _Theme.Legends[i];
                if (_Legend.FillStyle == FillStyle.Solid)
                {
                    BrLegend[i] = new SolidBrush(_Legend.Color);
                }
                else if (_Legend.FillStyle == FillStyle.Transparent)
                {
                    BrLegend[i] = new SolidBrush(Color.Transparent);
                }
                else
                {
                    BrLegend[i] = new HatchBrush((HatchStyle)_Legend.FillStyle, _Legend.Color, Color.Transparent);
                }
            }

            Pen PnTheme = new Pen(_Theme.BorderColor, _Theme.BorderWidth);
            Pen BorderHighlight = new Pen(_Theme.BorderColor, _Theme.BorderWidth + 0.7F);
            BorderHighlight.Alignment = PenAlignment.Inset;
            BorderHighlight.DashStyle = _Theme.BorderStyle;

            if (_Theme.BorderWidth == 0)
                PnTheme.Color = Color.Transparent;
            //??? Strange that setting Pen width = 0 gives effect of Pen width 1. So this workaround
            PnTheme.DashStyle = _Theme.BorderStyle;

            HatchBrush BrSelection = new HatchBrush(HatchStyle.Percent40, m_SelectionColor, Color.Transparent);
            Pen PnSelection = new Pen(m_SelectionColor);

            //*** If lyr exists in theme.Layervisibility collection
            foreach (Layer Lyr in Layers)
            {
                //Traverse Layers collection
                {
                    if (Lyr.Extent.IntersectsWith(CurExt) || CurExt.Contains(Lyr.Extent) || Lyr.Extent.Contains(CurExt))
                    {
                        // Render layer only if it lies within current map extent
                        if (_Theme.Type == ThemeType.Color)
                        {
                            if (_Theme.LayerVisibility[Lyr.ID] == null)
                                BaseLyrVisibility = false;
                            else
                                BaseLyrVisibility = (bool)_Theme.LayerVisibility[Lyr.ID];
                        }
                        else if (_Theme.Type == ThemeType.Hatch)
                        {
                            BaseLyrVisibility = Lyr.Visible;
                        }

                        Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                        IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                        if (Lyr.LayerType == ShapeType.Polygon & BaseLyrVisibility == true)
                        {
                            while (dicEnumerator.MoveNext())
                            {
                                //Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                if (_Shape.Extent.IntersectsWith(CurExt))
                                {
                                    //Render shape only if it lies within current map extent
                                    gpShp.Reset();
                                    for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                    {
                                        gpShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                    }
                                    gpShp.Transform(mTransMatrix);
                                    if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                                    {
                                        if (_Theme.MultiLegend == true)
                                        {
                                            {
                                                switch (_Theme.MultiLegendCriteria)
                                                {
                                                    case "SRC":
                                                        if (_Theme.MultiLegendCol.ContainsKey(((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Source))
                                                        {
                                                            //TODO Optimize SolidBrush
                                                            g.FillPath(new SolidBrush(((Legends)_Theme.MultiLegendCol[((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Source])[(int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo].Color), gpShp);
                                                        }

                                                        break;
                                                    case "SGP":
                                                        if (_Theme.MultiLegendCol.ContainsKey(((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Subgroup))
                                                        {
                                                            g.FillPath(new SolidBrush(((Legends)_Theme.MultiLegendCol[((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Subgroup])[(int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo].Color), gpShp);
                                                        }

                                                        break;
                                                    default:
                                                        if (_Theme.MultiLegendCol.ContainsKey(((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).MDFldVal[_Theme.MultiLegendCriteria]))
                                                        {
                                                            g.FillPath(new SolidBrush(((Legends)_Theme.MultiLegendCol[((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).MDFldVal[_Theme.MultiLegendCriteria]])[(int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo].Color), gpShp);
                                                        }

                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            g.FillPath(BrLegend[(int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo], gpShp);
                                        }
                                    }

                                    else
                                    {
                                        g.FillPath(BrLegend[BrLegend.Length - 1], gpShp);
                                    }
                                    try
                                    {
                                        //- Draw Specified Area's polygon to hightlight with different border
                                        if (_Shape.AreaId == this.AreaIDToHighlight)
                                        {
                                            g.DrawPath(BorderHighlight, gpShp);
                                        }
                                        else
                                        {
                                            //- Draw Area polygon Path
                                            g.DrawPath(PnTheme, gpShp);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Console.Write(ex.Message) '??? AFRERI Eritria problem
                                    }

                                    //*** Draw Base layer selection
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                        g.FillPath(BrSelection, gpShp);

                                    //-Exist Drawing Function
                                    if (_Shape.AreaId == this.AreaIDToHighlight)
                                    {
                                        break;
                                    }
                                }
                            }
                            //Traverse Shapes
                        }
                        else if (((Lyr.LayerType == ShapeType.PolygonFeature & BaseLyrVisibility == true) | ((Lyr.LayerType == ShapeType.PolygonCustom | Lyr.LayerType == ShapeType.PolygonBuffer) & Lyr.Visible == true)) & _Theme.Type == ThemeType.Color)
                        {
                            Pen PnLayer = new Pen(Lyr.BorderColor, Lyr.BorderSize);
                            if (Lyr.BorderSize == 0)
                                PnLayer.Color = Color.Transparent;
                            //??? Strange that setting Pen width = 0 gives effect of Pen width 1. So this workaround
                            PnLayer.DashStyle = Lyr.BorderStyle;
                            Brush BrLayer;
                            if (Lyr.FillStyle == FillStyle.Solid)
                            {
                                BrLayer = new SolidBrush(Lyr.FillColor);
                            }
                            else if (Lyr.FillStyle == FillStyle.Transparent)
                            {
                                BrLayer = new SolidBrush(Color.Transparent);
                            }
                            else
                            {
                                BrLayer = new HatchBrush((HatchStyle)Lyr.FillStyle, Lyr.FillColor, Color.Transparent);
                            }
                            gpShp.Reset();
                            gpShp.FillMode = FillMode.Winding;
                            gpSelShp.Reset();
                            gpSelShp.FillMode = FillMode.Winding;
                            while (dicEnumerator.MoveNext())
                            {
                                _Shape = (Shape)dicEnumerator.Value;
                                if (_Shape.Extent.IntersectsWith(CurExt))
                                {
                                    for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                    {
                                        try
                                        {
                                            gpShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                            gpSelShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                    }
                                }
                            }
                            gpShp.Transform(mTransMatrix);
                            g.FillPath(BrLayer, gpShp);
                            try
                            {
                                g.DrawPath(PnLayer, gpShp);
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                                //??? AFRERI Eritria problem
                            }
                            if (Lyr.SelectedArea.Count > 0)
                            {
                                gpSelShp.Transform(mTransMatrix);
                                g.FillPath(BrSelection, gpSelShp);
                            }
                            BrLayer.Dispose();
                            PnLayer.Dispose();
                        }
                        else if (Lyr.LayerType == ShapeType.PolyLine & BaseLyrVisibility == true & _Theme.Type == ThemeType.Color)
                        {
                            Pen PnLegend = new Pen(Lyr.BorderColor, Lyr.BorderSize);
                            PnLegend.DashStyle = Lyr.BorderStyle;
                            while (dicEnumerator.MoveNext())
                            {
                                //*** Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                gpShp.Reset();
                                for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                {
                                    gpShp.StartFigure();
                                    gpShp.AddLines((PointF[])_Shape.Parts[j]);
                                }
                                gpShp.Transform(mTransMatrix);
                                PnLegend.Color = _Theme.Legends[(int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo].Color;
                                g.DrawPath(PnLegend, gpShp);
                            }
                            PnLegend.Dispose();
                        }
                        else if (((Lyr.LayerType == ShapeType.PolyLineFeature & BaseLyrVisibility == true) | (Lyr.LayerType == ShapeType.PolyLineCustom & Lyr.Visible == true)) & _Theme.Type == ThemeType.Color)
                        {
                            gpShp.Reset();
                            while (dicEnumerator.MoveNext())
                            {
                                //*** Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                {
                                    gpShp.StartFigure();
                                    gpShp.AddLines((PointF[])_Shape.Parts[j]);
                                }
                            }
                            Pen PnLayer = new Pen(Lyr.BorderColor, Lyr.BorderSize);
                            PnLayer.DashStyle = Lyr.BorderStyle;
                            gpShp.Transform(mTransMatrix);
                            g.DrawPath(PnLayer, gpShp);
                            PnLayer.Dispose();
                        }
                        else if (Lyr.LayerType == ShapeType.Point & BaseLyrVisibility == true)
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            {
                                PointF[] Pt = new PointF[1];
                                int LegendItemIndex;
                                int[] MarkerSize = new int[_Theme.Legends.Count];
                                //PtSize based on Legend item
                                char[] MarkerChar = new char[_Theme.Legends.Count];
                                Font[] MarkerFont = new Font[_Theme.Legends.Count];
                                Pen PnCross = (Pen)PnTheme.Clone();
                                for (i = 0; i <= _Theme.Legends.Count - 1; i++)
                                {
                                    _Legend = _Theme.Legends[i];
                                    MarkerSize[i] = _Legend.MarkerSize;
                                    MarkerChar[i] = _Legend.MarkerChar;
                                    MarkerFont[i] = _Legend.MarkerFont;
                                    if (_Legend.MarkerType == MarkerStyle.Cross)
                                        PnCross.Color = _Legend.Color;
                                }
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    //*** BugFix 04 July 2006 Base point layer missing data rendered with 1st legend information
                                    if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                                    {
                                        LegendItemIndex = (int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo;
                                    }
                                    else
                                    {
                                        LegendItemIndex = _Theme.Legends.Count - 1;
                                    }
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    switch (_Theme.Legends[LegendItemIndex].MarkerType)
                                    {
                                        case MarkerStyle.Circle:
                                            g.FillEllipse(BrLegend[LegendItemIndex], (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);
                                            g.DrawEllipse(PnTheme, (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);
                                            if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                                g.FillEllipse(BrSelection, (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);

                                            break;
                                        case MarkerStyle.Square:
                                            g.FillRectangle(BrLegend[LegendItemIndex], (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);
                                            g.DrawRectangle(PnTheme, (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);
                                            if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                                g.FillRectangle(BrSelection, (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2, MarkerSize[LegendItemIndex], MarkerSize[LegendItemIndex]);

                                            break;
                                        case MarkerStyle.Triangle:
                                            //*** 1.Equilateral triangle with altitude = MarkerSize(LegendItemIndex) 2.Equilateral triangle with sides = MarkerSize(LegendItemIndex)
                                            PointF[] Vertex = new PointF[3];
                                            int PtSize;
                                            PtSize = (int)(Math.Sqrt(3) / 4 * MarkerSize[LegendItemIndex]);
                                            Vertex[0] = new PointF(Pt[0].X - PtSize, Pt[0].Y + PtSize);
                                            //altitude = PtSize 'Vertex(0) = New PointF(pt(0).X - PtSize / 2, pt(0).Y + PtSize / 2) 'side = PtSize
                                            Vertex[1] = new PointF(Pt[0].X + PtSize, Pt[0].Y + PtSize);
                                            //altitude = PtSize 'Vertex(1) = New PointF(pt(0).X + PtSize / 2, pt(0).Y + PtSize / 2) 'side = PtSize
                                            Vertex[2] = new PointF(Pt[0].X, Pt[0].Y - PtSize);
                                            g.FillPolygon(BrLegend[LegendItemIndex], Vertex);
                                            g.DrawPolygon(PnTheme, Vertex);
                                            if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                                g.FillPolygon(BrSelection, Vertex);

                                            Vertex = null;
                                            break;
                                        case MarkerStyle.Cross:
                                            g.DrawLine(PnCross, (int)Pt[0].X - MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y, (int)Pt[0].X + MarkerSize[LegendItemIndex] / 2, (int)Pt[0].Y);
                                            g.DrawLine(PnCross, (int)Pt[0].X, (int)Pt[0].Y + MarkerSize[LegendItemIndex] / 2, (int)Pt[0].X, (int)Pt[0].Y - MarkerSize[LegendItemIndex] / 2);
                                            break;
                                        case MarkerStyle.Custom:
                                            g.DrawString(MarkerChar[LegendItemIndex].ToString(), MarkerFont[LegendItemIndex], BrLegend[LegendItemIndex], Pt[0].X, Pt[0].Y, _StringFormat);
                                            break;
                                    }
                                }
                                for (i = 0; i <= MarkerFont.Length - 1; i++)
                                {
                                    MarkerFont[i].Dispose();
                                }
                                Pt = null;
                                PnCross.Dispose();
                            }
                            g.SmoothingMode = SmoothingMode.None;
                        }
                        else if (((Lyr.LayerType == ShapeType.PointFeature & BaseLyrVisibility == true) | (Lyr.LayerType == ShapeType.PointCustom & Lyr.Visible == true)) & _Theme.Type == ThemeType.Color)
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            Pen PnLayer = new Pen(Lyr.FillColor, 0.01F);
                            Brush BrLayer = new SolidBrush(Lyr.FillColor);
                            PointF[] Pt = new PointF[1];
                            int PtSize = Lyr.MarkerSize;
                            // CInt((_Layer.MarkerSize * m_FullExtent.Width) / (8 * m_CurrentExtent.Width))
                            gpShp.Reset();
                            gpSelShp.Reset();
                            if (Lyr.MarkerStyle == MarkerStyle.Circle)
                            {
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                    {
                                        gpSelShp.AddEllipse((int)Pt[0].X - PtSize / 2, (int)Pt[0].Y - PtSize / 2, PtSize, PtSize);
                                    }
                                    else
                                    {
                                        gpShp.AddEllipse((int)Pt[0].X - PtSize / 2, (int)Pt[0].Y - PtSize / 2, PtSize, PtSize);
                                    }
                                }
                            }
                            else if (Lyr.MarkerStyle == MarkerStyle.Square)
                            {
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                    {
                                        gpSelShp.AddRectangle(new RectangleF(Pt[0].X - PtSize / 2, Pt[0].Y - PtSize / 2, PtSize, PtSize));
                                    }
                                    else
                                    {
                                        gpShp.AddRectangle(new RectangleF(Pt[0].X - PtSize / 2, Pt[0].Y - PtSize / 2, PtSize, PtSize));
                                    }
                                }
                            }
                            else if (Lyr.MarkerStyle == MarkerStyle.Triangle)
                            {
                                //*** 1.Equilateral triangle with altitude = PtSize 2.Equilateral triangle with sides = PtSize
                                PointF[] Vertex = new PointF[3];
                                PtSize = (int)(Math.Sqrt(3) / 4 * PtSize);
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    Vertex[0] = new PointF(Pt[0].X - PtSize, Pt[0].Y + PtSize);
                                    //altitude = PtSize 'Vertex(0) = New PointF(pt(0).X - PtSize / 2, pt(0).Y + PtSize / 2) 'side = PtSize
                                    Vertex[1] = new PointF(Pt[0].X + PtSize, Pt[0].Y + PtSize);
                                    //altitude = PtSize 'Vertex(1) = New PointF(pt(0).X + PtSize / 2, pt(0).Y + PtSize / 2) 'side = PtSize
                                    Vertex[2] = new PointF(Pt[0].X, Pt[0].Y - PtSize);
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                    {
                                        gpSelShp.AddPolygon(Vertex);
                                    }
                                    else
                                    {
                                        gpShp.AddPolygon(Vertex);
                                    }
                                }
                                Vertex = null;
                            }
                            else if (Lyr.MarkerStyle == MarkerStyle.Cross)
                            {
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                    {
                                        gpSelShp.AddLine(new PointF(Pt[0].X - PtSize / 2, Pt[0].Y), new PointF(Pt[0].X + PtSize / 2, Pt[0].Y));
                                        gpSelShp.CloseFigure();
                                        gpSelShp.AddLine(new PointF(Pt[0].X, Pt[0].Y + PtSize / 2), new PointF(Pt[0].X, Pt[0].Y - PtSize / 2));
                                        gpSelShp.CloseFigure();
                                    }
                                    else
                                    {
                                        gpShp.AddLine(new PointF(Pt[0].X - PtSize / 2, Pt[0].Y), new PointF(Pt[0].X + PtSize / 2, Pt[0].Y));
                                        gpShp.CloseFigure();
                                        gpShp.AddLine(new PointF(Pt[0].X, Pt[0].Y + PtSize / 2), new PointF(Pt[0].X, Pt[0].Y - PtSize / 2));
                                        gpShp.CloseFigure();
                                    }
                                }
                            }
                            else if (Lyr.MarkerStyle == MarkerStyle.Custom)
                            {
                                Font _MFnt = new Font(Lyr.MarkerFont.FontFamily, PtSize);
                                while (dicEnumerator.MoveNext())
                                {
                                    //*** Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    mTransMatrix.TransformPoints(Pt);
                                    if (Lyr.SelectedArea.Contains(_Shape.AreaId))
                                    {
                                        g.DrawString(Lyr.MarkerChar.ToString(), _MFnt, BrSelection, Pt[0].X, Pt[0].Y, _StringFormat);
                                    }
                                    else
                                    {
                                        g.DrawString(Lyr.MarkerChar.ToString(), _MFnt, BrLayer, Pt[0].X, Pt[0].Y, _StringFormat);
                                    }
                                }
                                _MFnt.Dispose();
                            }
                            g.FillPath(BrLayer, gpShp);
                            g.DrawPath(PnLayer, gpShp);
                            g.FillPath(BrSelection, gpSelShp);
                            g.DrawPath(PnSelection, gpSelShp);
                            Pt = null;
                            PnLayer.Dispose();
                            BrLayer.Dispose();
                            g.SmoothingMode = SmoothingMode.None;
                        }
                        ht = null;
                    }
                }
                //Lyr = null;
            }
            //Traverse Layers collection

            _Shape = null;
            for (i = 0; i <= BrLegend.Length - 1; i++)
            {
                BrLegend[i].Dispose();
            }
            PnTheme.Dispose();
            BorderHighlight.Dispose();
            BrSelection.Dispose();
            PnSelection.Dispose();
            _StringFormat.Dispose();
            gpSelShp.Dispose();
            gpShp.Dispose();
        }

        private void DrawDotDensity(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt)
        {
            int DotCount;
            int DotSize = (int)_Theme.DotSize;
            double DotValue = _Theme.DotValue;


            Brush BrDot = new SolidBrush(_Theme.DotColor);
            Pen PnDot = new Pen(_Theme.DotColor);
            PointF[] Pt = new PointF[1];

            int j;
            int TrialCount;

            Shape _Shape;
            GraphicsPath gpShp = new GraphicsPath();
            RectangleF RectRnd = new RectangleF();
            Matrix _Matrix = new Matrix();
            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            Theme _ATheme = m_Themes.GetActiveTheme();
            bool bLayerVisibility;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //*** Traverse Layers collection
            foreach (Layer _Layer in Layers)
            {
                if (DotValue == 0)
                    break;

                if ((_ATheme == null))
                {
                    bLayerVisibility = _Layer.Visible;
                }
                else
                {
                    bLayerVisibility = (bool)(_ATheme.LayerVisibility[_Layer.ID]);
                }
                //*** Consider polygon layers lying within current map extent and visibility is on
                if (_Layer.LayerType == ShapeType.Polygon & _Layer.Extent.IntersectsWith(CurExt) & bLayerVisibility)
                {
                    //Render layer only if it lies within current map extent
                    Hashtable ht = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID);
                    IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                    //*** Traverse each Shape of the layer
                    while (dicEnumerator.MoveNext())
                    {
                        _Shape = (Shape)dicEnumerator.Value;
                        //*** Consider shape only if it lies within current map extent
                        if (_Shape.Extent.IntersectsWith(CurExt))
                        {
                            if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                            {
                                DotCount = (int)((double)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).DataValue / DotValue);
                                Region Rgn = new Region();
                                RectangleF[] Rect;
                                PointF[] Vertex = new PointF[3];
                                //*** for triangle
                                object MarkerSize = Math.Sqrt(3) / 4 * DotSize;
                                //*** for triangle

                                if (DotCount > 0)
                                {
                                    gpShp.Reset();
                                    for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                                    {
                                        gpShp.AddPolygon((PointF[])_Shape.Parts[j]);
                                    }
                                    Rgn = new Region(gpShp);
                                    //Rgn.Intersect(CurExt)
                                    Rgn.Transform(mTransMatrix);
                                    Rect = Rgn.GetRegionScans(_Matrix);
                                    //http://www.dotnet247.com/247reference/a.aspx?u=http://www.bobpowell.net/gdiplus_faq.htm
                                    //Rect.Sort(Rect)
                                }
                                else
                                {
                                    Rect = new RectangleF[0];   // Rect[] length is set to 0 because Rect.length was required in If condition below.
                                }

                                //*** Draw random dots inside region
                                j = 1;
                                TrialCount = 1;

                                //*** Bugfix / Enhancement 19 Jun 2006 Distribution of Dots
                                while (j <= DotCount)
                                {
                                    if (DotCount == 0)
                                        break;

                                    if (Rect.Length == 0)
                                        break;

                                    VBMath.Randomize();

                                    try
                                    {
                                        if (Rect.Length / 2 > j + 1)
                                        {
                                            if ((double)j / 2 == (int)(j / 2))  //Math.Round((double)(j / 2)))
                                            {
                                                if ((double)j / 8 == Math.Round((double)(j / 8)))
                                                {
                                                    RectRnd = Rect[j];
                                                }
                                                else
                                                {
                                                    RectRnd = Rect[(int)(Rect.Length / 2) - j];
                                                }
                                            }
                                            else
                                            {
                                                if ((double)j / 6 == Math.Round((double)(j / 6)))
                                                {
                                                    RectRnd = Rect[Rect.Length - j];
                                                }
                                                else
                                                {
                                                    RectRnd = Rect[(int)(Rect.Length / 2) + j];
                                                }
                                            }
                                        }
                                        else
                                        {
                                            RectRnd = Rect[(int)(VBMath.Rnd() * (Rect.Length - 1))];
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }

                                    Pt[0].X = RectRnd.X + RectRnd.Width * (float)(0.6 * (float)VBMath.Rnd() + 0.2);
                                    Pt[0].Y = RectRnd.Y - RectRnd.Height / 2;
                                    //* Rnd()

                                    if (Rgn.IsVisible(Pt[0]))
                                    {
                                        switch (_Theme.DotStyle)
                                        {
                                            case MarkerStyle.Circle:
                                                g.FillEllipse(BrDot, (int)(Pt[0].X - DotSize / 2), (int)(Pt[0].Y - DotSize / 2), (int)DotSize, (int)DotSize);
                                                break;
                                            case MarkerStyle.Square:
                                                g.FillRectangle(BrDot, (int)(Pt[0].X - DotSize / 2), (int)(Pt[0].Y - DotSize / 2), (int)DotSize, (int)DotSize);
                                                break;
                                            case MarkerStyle.Triangle:
                                                Vertex[0] = new PointF(Pt[0].X - float.Parse(MarkerSize.ToString()), Pt[0].Y + float.Parse(MarkerSize.ToString()));
                                                Vertex[1] = new PointF(Pt[0].X + float.Parse(MarkerSize.ToString()), Pt[0].Y + float.Parse(MarkerSize.ToString()));
                                                Vertex[2] = new PointF(Pt[0].X, Pt[0].Y - float.Parse(MarkerSize.ToString()));
                                                g.FillPolygon(BrDot, Vertex);
                                                break;
                                            case MarkerStyle.Cross:
                                                g.DrawLine(PnDot, (int)(Pt[0].X - DotSize / 2), (int)Pt[0].Y, (int)(Pt[0].X + DotSize / 2), (int)Pt[0].Y);
                                                g.DrawLine(PnDot, (int)Pt[0].X, (int)(Pt[0].Y + DotSize / 2), (int)Pt[0].X, (int)(Pt[0].Y - DotSize / 2));
                                                break;
                                            case MarkerStyle.Custom:
                                                g.DrawString(_Theme.DotChar.ToString(), _Theme.DotFont, BrDot, Pt[0].X, Pt[0].Y, _StringFormat);
                                                break;
                                        }
                                        j = j + 1;
                                    }

                                    if (TrialCount > DotCount + 2)
                                        break;

                                    TrialCount += 1;
                                }
                                Rgn.Dispose();
                                Rect = null;
                            }
                        }
                    }
                    ht = null;
                    dicEnumerator = null;
                }
                // _Layer = null;
            }
            g.SmoothingMode = SmoothingMode.None;

            _Matrix = null;
            // RectRnd = null;
            Pt = null;
            // _Layer = null;
            _Shape = null;
            BrDot.Dispose();
            PnDot.Dispose();
            gpShp.Dispose();
            _StringFormat.Dispose();

        }

        private void DrawChart(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt)
        {
            bool bSearch = false;
            float PtX = 0;
            float PtY = 0;
            string LayerId = "";
            string AreaId = "";
            GraphicsPath gPath = null;
            DrawChart(ref g, _Theme, mTransMatrix, CurExt, bSearch, PtX, PtY, ref LayerId, ref AreaId, ref gPath);
        }

        private void DrawChart(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt, bool bSearch, float PtX, float PtY, ref string LayerId, ref string AreaId, ref GraphicsPath gPath)
        {
            #region "-- local Variables --"

            int i;

            Brush[] BrIS = new Brush[(_Theme.IndicatorColor.Length * _Theme.SubgroupFillStyle.Length)];

            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            for (i = 0; i <= _Theme.SubgroupFillStyle.Length - 1; i++)
            {
                BrIS[i] = new SolidBrush(Color.FromArgb(int.Parse(_Theme.SubgroupFillStyle[i])));
            }

            int VisibleTimePeriods = 0;
            int SpCnt = 0;                      // ChartSeriesNames (Subgroup / Source) visible count.
            int TotalBarColumns = 0;            // Totol Bar columns to draw for ONE Area
            int BarCtr = 0;
            int TimeVisibleCtr = 0;              // TimePeriod Counter for visible TimePeriods.
            int LineCtr = 0;
            float ColumnGap = 0;                // Distance between Column Charts of two consecutive TimePeriods
            double UnitWidth;
            decimal MaxData = _Theme.Maximum;
            float MapTop = m_FullExtent.Top;
            Font TextFont = null;               // maximum limit for Text Size of chart data value.
            Brush ChartDataLabelBrush = null;
            Font ChartAxisLabelFont = null;
            Brush ChartAxisLabelBrush = null;

            float TextFontSize = 6.2F;           // Text size for Chart data value.
            decimal DataValue = 0;
            PointF Pt = new PointF();
            PointF[] Pts = new PointF[] { Pt };
            PointF[] PtData = new PointF[1];
            PointF[] LeaderPts = new PointF[2];
            Pen PnLeader = new Pen(_Theme.ChartLeaderColor, _Theme.ChartLeaderWidth);
            PointF Centroid = new PointF(0, 0);
            GraphicsPath gpShp = new GraphicsPath();
            AreaInfo _AreaInfo;
            string[] DataArr;
            Hashtable ChartData = null; // Collection of TimePeriod wise delimited DataValue for multiple Subgroup/Source
            string ChartMRD = string.Empty;
            bool DataFoundForTimePeriod = false;

            //-- Pie variables
            float StartAngle = 0;     // Start angle for first Pie section , starting from 0 degree in clockwise direction
            float PieAngle = 0;
            float PieDiameter = 0;
            float PieMinDiameter = 0;
            float PieMaxDiameter = 0;
            decimal DataValueSum = 0;   // sum
            RectangleF ChartExtent = new RectangleF();  // Applicable for Pie & Line Chart


            // Line variables
            PointF LineStartPoint = new PointF();
            PointF LineEndPoint = new PointF();
            PointF[] LinePoints = new PointF[2];
            object[] LineSubgroupPoints = null; // Line points for visible Subgroup for any TimePeriod
            float LineChartYAxisLenght = 0F;
            float LineThickness = (float)_Theme.ChartLineThickness;
            bool AxisDrawn = false;         // indicates whether X, Y Axis are drawn one time.
            PointF AxisPoint1 = new PointF();
            PointF AxisPoint2 = new PointF();
            PointF[] AxisPoints = new PointF[2];
            decimal LineMaxDataValue = 0;

            PnLeader.DashStyle = _Theme.ChartLeaderStyle;
            #endregion

            //-- visible series - Subgroups / Sources
            string[] ChartVisibleSeries;

            if (_Theme.ChartSeriestype == ChartSeriesType.Subgroup)
            {
                ChartVisibleSeries = _Theme.SubgroupVisible;
            }
            else
            {
                ChartVisibleSeries = _Theme.SourceVisible;
            }

            //- Count Visible Series (either Subgroup Or Sources)
            for (i = 0; i <= ChartVisibleSeries.Length - 1; i++)
            {
                if (ChartVisibleSeries[i] == "1")
                    SpCnt += 1;
            }

            //-- Set TextFont Size on the basis of Map's Current Extent
            if (CurExt.Width < 40 && CurExt.Width >= 25)
            {
                TextFontSize = 7;
            }
            else if (CurExt.Width < 25)
            {
                TextFontSize = 8;
            }
            //TextFont = new Font(_Theme.LabelFont.Name, TextFontSize);

            //-- Set Font used to render Data Value & Chart Axis Labels
            CustomLabelSetting labelSetting = _Theme.ChartDataLabelSettings;
            if (labelSetting == null)
            {
                labelSetting = new CustomLabelSetting();
            }
            TextFont = labelSetting.LabelFont;
            ChartDataLabelBrush = new SolidBrush(labelSetting.LabelColor);

            labelSetting = _Theme.ChartAxisLabelSettings;
            if (labelSetting == null)
            {
                labelSetting = new CustomLabelSetting();
            }
            ChartAxisLabelFont = labelSetting.LabelFont;
            ChartAxisLabelBrush = new SolidBrush(labelSetting.LabelColor);

            //*** Bugfix / Enhancement 02 May 2006 Controling chart width
            //*** Bugfix / Enhancement 15 Jun 2006 Controling chart width
            //*** Set the unit width to 1/20th of the Screen width.
            //*** If there are multiple subgroup that should be accomodated then they should be accomodated within unit width
            //*** Set Width of Bars based on FullExtentWidth(/) + Subgroup Count(\) + Chart Size(/)
            UnitWidth = ((m_FullExtent.Width) / (SpCnt * 20)) * Math.Pow(1.5, _Theme.ChartWidth - 10);
            //UnitWidth = ((m_FullExtent.Width) / (50)) + ((float)_Theme.ChartWidth /  20);
            //Exponential Rise (_Theme.ChartWidth / 10) '
            //UnitWidth = (1 / SpCnt) * (_Theme.ChartWidth / 10) '*** Set Width of Bars based on FullExtentWidth(/) + Subgroup Count(\) + Chart Size(/)
            ColumnGap = (float)((UnitWidth * 0.5) * (_Theme.ColumnsGap));

            Theme _ATheme = m_Themes.GetActiveTheme();
            bool bLayerVisibility;



            Shape _Shape;
            //Polygon : Polyline : Point
            if (SpCnt > 0)
            {
                // and calculate the PieDiameter (Max & Min) on the basis of Map FullExtent.
                // Maximum PieDiameter is kept at 1/5 th of FullExtent.
                if (_Theme.ChartType != ChartType.Column)
                {

                    if ((PieMaxDiameter < m_FullExtent.Height / 5F) || (PieMaxDiameter < m_FullExtent.Width / 5F))
                    {
                        //- max
                        if (m_FullExtent.Height > m_FullExtent.Width)
                        {
                            PieMaxDiameter = (m_FullExtent.Width) / 5F;
                        }
                        else
                        {
                            PieMaxDiameter = (m_FullExtent.Height) / 5F;
                        }
                        //- Validate Maximum Piediameter shoukd be > 3F
                        if (PieMaxDiameter < 3F)
                        {
                            PieMaxDiameter = 3F;
                        }
                        //- Mimimum PieDiameter is Kept 1/3rd of Maximim.
                        PieMinDiameter = PieMaxDiameter * 0.3F;
                    }


                    //-- Maximum Diameter must be less than Total Extent / 5
                    if ((PieMaxDiameter > CurExt.Width / 4))
                    {
                        PieMaxDiameter = CurExt.Width / 4;
                        //- Mimimum PieDiameter is Kept 1/3rd of Maximim.
                        PieMinDiameter = PieMaxDiameter * 0.3F;
                    }
                }

                //*** If all subgroup are unchecked then no need of rendering
                foreach (Layer Lyr in Layers)
                {
                    //--Reset Chart variables
                    StartAngle = 0;
                    PieAngle = 0;
                    DataValueSum = 0;
                    AxisDrawn = false;
                    VisibleTimePeriods = 0;

                    //Traverse Layers collection
                    if ((_ATheme == null))
                    {
                        bLayerVisibility = Lyr.Visible;
                    }
                    else
                    {
                        if (_ATheme.LayerVisibility[Lyr.ID] == null)
                        {
                            bLayerVisibility = false;
                        }
                        else
                        {
                            bLayerVisibility = (bool)_ATheme.LayerVisibility[Lyr.ID];
                        }
                    }
                    if (Lyr.LayerType == ShapeType.Polygon & bLayerVisibility == true & Lyr.Extent.IntersectsWith(CurExt))
                    {
                        // Render layer only if it lies within current map extent
                        Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                        IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                        while (dicEnumerator.MoveNext())
                        {
                            //Traverse Shapes
                            _Shape = (Shape)dicEnumerator.Value;

                            //--Check AreaID exists in AreaIndexes. AND AreaID should NOT be excluded explicitly.
                            if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId) && _Theme.ExcludeAreaIDs.Contains(_Shape.AreaId) == false)
                            {
                                _AreaInfo = (AreaInfo)_Theme.AreaIndexes[_Shape.AreaId];

                                //-- get ChartData and ChartMRDData.
                                ChartData = _AreaInfo.ChartData;
                                ChartMRD = _AreaInfo.ChartMostRecentData;

                                //*** Get Shape Centroid
                                if (_Theme.ModifiedCharts.ContainsKey(Lyr.ID + "_" + _Shape.AreaId))
                                {
                                    //*** Get Modified Centroid for Nudged Chart
                                    Centroid = (PointF)_Theme.ModifiedCharts[Lyr.ID + "_" + _Shape.AreaId];

                                    //*** Draw Leader Line for Nudged Chart if atleast one subgroup bar is visible
                                    if (SpCnt > 0 & _Theme.ChartLeaderVisible == true)
                                    {
                                        LeaderPts[0] = Centroid;
                                        if (Lyr.LayerType == ShapeType.Polygon)
                                        {
                                            LeaderPts[1] = _Shape.Centroid;
                                        }
                                        else if (Lyr.LayerType == ShapeType.Point)
                                        {
                                            LeaderPts[1] = (PointF)_Shape.Parts[0];
                                        }
                                        mTransMatrix.TransformPoints(LeaderPts);
                                        g.DrawLine(PnLeader, LeaderPts[0], LeaderPts[1]);
                                    }
                                }
                                else
                                {
                                    //*** Get default Shape Centroid
                                    if (Lyr.LayerType == ShapeType.Polygon)
                                    {
                                        Centroid = _Shape.Centroid;
                                    }
                                    else if (Lyr.LayerType == ShapeType.Point)
                                    {
                                        Centroid = (PointF)_Shape.Parts[0];
                                    }
                                }

                                //--Reset Chart variables
                                AxisDrawn = false;
                                VisibleTimePeriods = 0;
                                DataValueSum = 0;
                                TimeVisibleCtr = 0;
                                StartAngle = 0;
                                PieAngle = 0;

                                #region "-- Set Pie / Line Chart Extent "

                                //-- Get X, Y position & Diameter of Pie Circle / Extent for Line Chart .
                                if (_Theme.ChartType == ChartType.Pie || _Theme.ChartType == ChartType.Line)
                                {
                                    //-- Pie drawn should be fitted in the Area.
                                    // so reduce the Area of Pie to 40% of Total Area Extent
                                    ChartExtent = _Shape.Extent;

                                    ChartExtent.Width = ChartExtent.Width * 0.5F;
                                    ChartExtent.Height = ChartExtent.Height * 0.5F;
                                    //-- Set the Pie Diameter according to layer's width and height
                                    if (_Theme.PieAutoSize)            //|| (!(_Theme.PieAutoSize) & _Theme.PieSize <= 0))
                                    {
                                        //-- If Shape Area is very small i.e. < Minumum Diameter set, 
                                        // then 
                                        if (ChartExtent.Width < PieMinDiameter || ChartExtent.Height < PieMinDiameter)
                                        {
                                            PieDiameter = PieMinDiameter;
                                        }
                                        else if (ChartExtent.Width > PieMaxDiameter && ChartExtent.Height > PieMaxDiameter)
                                        {
                                            //If PieDiameter is too high i.e. > Maximum Diameter set, 
                                            PieDiameter = PieMaxDiameter;
                                        }
                                        else if (ChartExtent.Height < ChartExtent.Width)
                                        {
                                            if (ChartExtent.Height * 0.9 < PieMinDiameter)
                                            {
                                                PieDiameter = PieMinDiameter;
                                            }
                                            else
                                            {
                                                PieDiameter = ChartExtent.Height * 0.9F;
                                                //PieDiameter = PieRectExtent.Height * 0.5F + (1 - PieRectExtent.Height / PieRectExtent.Width);
                                            }
                                        }
                                        else
                                        {
                                            if (ChartExtent.Width * 0.9 < PieMinDiameter)
                                            {
                                                PieDiameter = PieMinDiameter;
                                            }
                                            else
                                            {
                                                PieDiameter = ChartExtent.Width * 0.9F;
                                                //PieDiameter = PieRectExtent.Height * 0.5F + (1 - PieRectExtent.Height / PieRectExtent.Width);
                                            }
                                            //PieDiameter = PieRectExtent.Width * 0.5F + (1 - PieRectExtent.Width / PieRectExtent.Height);
                                        }

                                        //-- Multiply by Auto size Factor value.
                                        PieDiameter *= _Theme.PieAutoSizeFactor;
                                    }
                                    else if (_Theme.PieSize > 0)
                                    {
                                        //-- Fixed size of each pie is started at minium from curExtent / 12
                                        PieMaxDiameter = CurExt.Height / 12;
                                        //-- Then add (PieSize / 10) to it.
                                        PieDiameter = (PieMaxDiameter) * (1 + (float)_Theme.PieSize / 10);
                                    }


                                    ChartExtent.Width = PieDiameter;
                                    ChartExtent.Height = PieDiameter;


                                    if (_Theme.ModifiedCharts.ContainsKey(Lyr.ID + "_" + _Shape.AreaId))
                                    {
                                        //-- IF Chart is nudged then keep Pie center on Centroid.
                                        ChartExtent.X = Centroid.X - PieDiameter * 0.5F;
                                        ChartExtent.Y = Centroid.Y - PieDiameter * 0.5F;
                                    }
                                    else
                                    {
                                        //else, Keep pie center little displaced from Centroid towards downwards
                                        ChartExtent.X = Centroid.X - PieDiameter * 0.6F;
                                        ChartExtent.Y = Centroid.Y - PieDiameter * 0.7F;
                                    }

                                    if (_Theme.ChartType == ChartType.Line)
                                    {
                                        ChartExtent.Y = Centroid.Y - ChartExtent.Height * 0.1F;
                                        ChartExtent.Height = PieDiameter;
                                        ChartExtent.Width = ChartExtent.Height * 1.4F;
                                    }
                                }

                                #endregion


                                //- Get Visible TimePeriods (used in calculating X-Axis length)
                                foreach (string KeyTimePeriod in ChartData.Keys)
                                {
                                    if ((bool)_Theme.ChartTimePeriods[KeyTimePeriod] == true)
                                    {
                                        //- Count TimePeriods only if DataValue exists for visible Subgroups
                                        string[] TempDataArr = ChartData[KeyTimePeriod].ToString().Split(',');
                                        for (i = 0; i < TempDataArr.Length; i++)
                                        {
                                            if (string.IsNullOrEmpty(TempDataArr[i]) == false)
                                            {
                                                if (ChartVisibleSeries[i] == "1")
                                                {
                                                    VisibleTimePeriods++;
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                                if (VisibleTimePeriods == 0 || _Theme.DisplayChartMRD)
                                {
                                    VisibleTimePeriods = 1;
                                }

                                //--Total bar column to draw for single Area. (Total Subroups * Total TimePeriod)
                                TotalBarColumns = VisibleTimePeriods * SpCnt;
                                BarCtr = 0;
                                LineSubgroupPoints = new object[ChartVisibleSeries.Length]; // holds line Points drawn

                                //-- Loop TimePeriods  for Column OR Line chart. Column/ Line will be plotted for each TimePeriod 
                                foreach (string TimePeriodKey in _Theme.ChartTimePeriods.Keys)
                                {
                                    if (string.IsNullOrEmpty(TimePeriodKey.ToString()) == false)
                                    {
                                        //-- Check TimePeriod visibiliy and visibleTime < 2 for LineChart
                                        if (!(_Theme.ChartType == ChartType.Line && VisibleTimePeriods < 2))
                                        {
                                            //-- Check if TimePeriod visibilty is ON, And ChartData must have data against timePeriod
                                            if ((_Theme.ChartTimePeriods[TimePeriodKey.ToString()] == true && ChartData.ContainsKey(TimePeriodKey)) || _Theme.DisplayChartMRD)
                                            {
                                                if (TimeVisibleCtr < VisibleTimePeriods)
                                                {
                                                    TimeVisibleCtr++;
                                                }
                                                LineCtr = 0;
                                                DataValueSum = 0;
                                                DataFoundForTimePeriod = false;

                                                //-- If MRD for Chart is ON, Get mostRecent DataValues
                                                if (_Theme.DisplayChartMRD)
                                                {
                                                    DataArr = ChartMRD.Split(',');
                                                }
                                                else    //IF MRD is Off, get TimePeriodwise DataValues.
                                                {
                                                    DataArr = ChartData[TimePeriodKey].ToString().Split(',');
                                                }

                                                //-- Get sum of all dataValues, will be used in PieChart, LineChart
                                                // sum for only visible subgroups
                                                for (i = 0; i < DataArr.Length; i++)
                                                {
                                                    if (string.IsNullOrEmpty(DataArr[i]) == false)
                                                    {

                                                        decimal decVal = Convert.ToDecimal(DataArr[i], CultureInfo.InvariantCulture.NumberFormat);
                                                        if (decVal > 0 && ChartVisibleSeries[i] == "1")
                                                        {
                                                            DataFoundForTimePeriod = true;  //-- Indicates that some Data is found for visible Subgroup

                                                            //-- Round DataValues
                                                            decVal = Math.Round(decVal, _Theme.RoundDecimals);
                                                            DataArr[i] = decVal.ToString().Replace(",", ".");

                                                            DataValueSum += decVal;
                                                        }
                                                    }
                                                }

                                                // -- Get maximum DataValue for LineChart
                                                if (_Theme.ChartType == ChartType.Line)
                                                {
                                                    if (_Theme.DisplayChartMRD)
                                                    {
                                                        LineMaxDataValue = this.GetMaximumChartDataValue(DataArr);
                                                    }
                                                    else
                                                    {
                                                        LineMaxDataValue = this.GetMaximumChartDataValue(ChartData);
                                                    }
                                                    LineMaxDataValue *= 1.3M;
                                                }

                                                int j = 0;
                                                //*** Counter for Visible Series (subgroup / Sources)
                                                for (i = 0; i <= _Theme.SubgroupFillStyle.Length - 1; i++)
                                                {
                                                    if (ChartVisibleSeries[i] == "1" && DataFoundForTimePeriod)
                                                    {
                                                        BarCtr++;   //This is Nth Bar Column drawing (after increment)
                                                        try
                                                        {
                                                            if (DataArr[i] == "")
                                                            {
                                                                DataValue = 0;
                                                            }
                                                            else
                                                            {
                                                                DataValue = Convert.ToDecimal(Conversion.Val(DataArr[i]), CultureInfo.InvariantCulture.NumberFormat);
                                                                //DataValue = (decimal)Microsoft.VisualBasic.Conversion.Val(DataArr[i]);
                                                                //Cdbl
                                                            }

                                                            //-- If DataValue is NOT Zero, NOT Pie chart
                                                            if (DataArr[i] != "" && DataValueSum != 0)
                                                            {
                                                                #region "-- Column --"

                                                                if (_Theme.ChartType == ChartType.Column)
                                                                {
                                                                    Pt = Centroid;
                                                                    if (_Theme.ModifiedCharts.ContainsKey(Lyr.ID + "_" + _Shape.AreaId))
                                                                    {
                                                                        Pt.X = Pt.X + ((BarCtr - 1) * (float)UnitWidth) + (ColumnGap * (TimeVisibleCtr - 1));
                                                                    }
                                                                    else
                                                                    {
                                                                        //-Chart Mid point should coincide with the Centroid of Shape
                                                                        //- get Totol width of Column Chart , and get mid point of chart
                                                                        float ChartWidth = (float)(UnitWidth * TotalBarColumns) + ((VisibleTimePeriods - 1) * ColumnGap);

                                                                        Pt.X = Pt.X - ChartWidth / 2F + ((BarCtr - 1) * (float)UnitWidth) + (ColumnGap * (TimeVisibleCtr - 1));
                                                                        //Pt.Y = Pt.Y + (_Shape.Extent.Height) / 20;
                                                                        Pt.Y = Pt.Y - (CurExt.Height) / 40;
                                                                    }


                                                                    Pts = new PointF[4];
                                                                    Pts[0] = Pt;
                                                                    //1st

                                                                    Pt.X = Pt.X + (float)UnitWidth;
                                                                    Pts[1] = Pt;
                                                                    //2nd

                                                                    //*** Bugfix / Enhancement 15 Jun 2006 Controling Chart Height
                                                                    //*** Set Height of Bars based on FullExtentWidth(/) + DataValue(\) + Chart Size(/)
                                                                    Pt.Y = Pt.Y + (float)((m_FullExtent.Height / 2) * (float)(DataValue / MaxData) * Math.Pow(1.5, _Theme.ChartSize - 10));
                                                                    // exponential rise (_Theme.ChartSize / 10))


                                                                    //--AXIS X & Y
                                                                    //-- First Draw the X for Column Chart. 
                                                                    //-- Axis should be drawn only first time. 
                                                                    // Axis for Column to be drawn only for multiple TimePeriods.
                                                                    gpShp.Reset();
                                                                    if (AxisDrawn == false && _Theme.DisplayChartMRD == false)
                                                                    {
                                                                        //--Get Origin of Axis to be drawn, starting near first Column bar.
                                                                        //- Get coordinates of First Column bar
                                                                        AxisPoint1 = Centroid;
                                                                        if (_Theme.ModifiedCharts.ContainsKey(Lyr.ID + "_" + _Shape.AreaId))
                                                                        {
                                                                        }
                                                                        else
                                                                        {
                                                                            //-Chart Mid point should coincide with the Centroid of Shape
                                                                            //- get Totol width of Column Chart , and get mid point of chart
                                                                            float ChartWidth = (float)(UnitWidth * TotalBarColumns) + ((VisibleTimePeriods - 1) * ColumnGap);

                                                                            AxisPoint1.X = AxisPoint1.X - ChartWidth / 2F;
                                                                            AxisPoint1.Y = Pts[0].Y;
                                                                        }

                                                                        //-- Adding Axis buffer
                                                                        AxisPoint1.X = AxisPoint1.X - ((float)UnitWidth / 6F);
                                                                        AxisPoint1.Y = AxisPoint1.Y;

                                                                        AxisPoint2.X = AxisPoint1.X + ((float)UnitWidth * TotalBarColumns) + (ColumnGap * (VisibleTimePeriods - 1)) + ((float)UnitWidth / 3F);

                                                                        AxisPoint2.Y = AxisPoint1.Y;


                                                                        // X Axis
                                                                        if (_Theme.ShowChartAxis)
                                                                        {
                                                                            //gpShp.AddLine(AxisPoint1, AxisPoint2);
                                                                            AxisPoints[0] = AxisPoint1;
                                                                            AxisPoints[1] = AxisPoint2;
                                                                            mTransMatrix.TransformPoints(AxisPoints);
                                                                            g.DrawLine(new Pen(new SolidBrush(_Theme.ChartAxisColor), 1), AxisPoints[0], AxisPoints[1]);
                                                                        }
                                                                        AxisDrawn = true;
                                                                        AxisPoint1.X += (float)UnitWidth / 6F;
                                                                        AxisPoint2.X -= 0.1F;
                                                                    }

                                                                    if (Pt.Y > MapTop)
                                                                    {
                                                                        // ReDimStatement not supported in C#, so Pts array is copied into new array PtsTemp
                                                                        PointF[] PtsTemp = new PointF[7];      //PtsTemp[] is resized as desired
                                                                        Array.Copy(Pts, PtsTemp, Math.Min(Pts.Length, PtsTemp.Length));
                                                                        Pts = PtsTemp;      // PtsTemp elements are copied back into Pts[]

                                                                        Pt.Y = MapTop - 2 * (float)UnitWidth;
                                                                        Pts[2] = Pt;
                                                                        //3rd
                                                                        Pt.X = Pt.X - (float)UnitWidth / 4;
                                                                        Pts[3] = Pt;
                                                                        //4rd
                                                                        Pt.Y = Pt.Y - (float)UnitWidth / 4;
                                                                        Pt.X = Pt.X - (float)UnitWidth / 4;
                                                                        Pts[4] = Pt;
                                                                        //5th
                                                                        Pt.Y = MapTop - 2 * (float)UnitWidth;
                                                                        Pts[5] = Pt;
                                                                        //6th
                                                                        Pt.X = Pt.X - (float)UnitWidth / 2;
                                                                        Pts[6] = Pt;
                                                                        //7th
                                                                        //gpShp.Reset();
                                                                        gpShp.AddPolygon(Pts);

                                                                        //*** Set Bar Break
                                                                        Array.Clear(Pts, 0, 7);
                                                                        Pt.Y = Pt.Y + (float)UnitWidth / 4;
                                                                        Pts[0] = Pt;
                                                                        Pt.X = Pt.X + (float)UnitWidth / 2;
                                                                        Pts[1] = Pt;
                                                                        Pt.Y = Pt.Y + (float)UnitWidth / 4;
                                                                        Pt.X = Pt.X + (float)UnitWidth / 4;
                                                                        Pts[2] = Pt;
                                                                        Pt.Y = Pt.Y - (float)UnitWidth / 4;
                                                                        Pts[3] = Pt;
                                                                        Pt.X = Pt.X + (float)UnitWidth / 4;
                                                                        Pts[4] = Pt;
                                                                        Pt.Y = MapTop - (float)UnitWidth;
                                                                        Pts[5] = Pt;
                                                                        Pt.X = Pt.X - (float)UnitWidth;
                                                                        Pts[6] = Pt;
                                                                        //gpShp.AddPolygon(Pts);
                                                                    }
                                                                    else
                                                                    {
                                                                        Pts[2] = Pt;
                                                                        //3rd
                                                                        Pt.X = Pt.X - (float)UnitWidth;
                                                                        Pts[3] = Pt;
                                                                        //4th
                                                                        //gpShp.Reset();
                                                                        //gpShp.AddPolygon(Pts);
                                                                    }

                                                                    //-- Add Bar polygon
                                                                    gpShp.AddPolygon(Pts);
                                                                }

                                                                #endregion

                                                                #region "-- LINE --"

                                                                if (_Theme.ChartType == ChartType.Line)
                                                                {
                                                                    LineCtr++;

                                                                    //-- First Draw the X, Y axis for Line Chart.
                                                                    //-- Axis should be drawn only first time.
                                                                    gpShp.Reset();
                                                                    if (AxisDrawn == false && VisibleTimePeriods > 1)
                                                                    {

                                                                        // X Axis
                                                                        AxisPoint1 = new PointF(ChartExtent.Left, ChartExtent.Top);
                                                                        AxisPoint2 = new PointF(ChartExtent.Left + ChartExtent.Width, ChartExtent.Top);

                                                                        if (_Theme.ShowChartAxis)
                                                                        {
                                                                            //gpShp.AddLine(AxisPoint1, AxisPoint2);
                                                                            AxisPoints[0] = AxisPoint1;
                                                                            AxisPoints[1] = AxisPoint2;
                                                                            mTransMatrix.TransformPoints(AxisPoints);
                                                                            g.DrawLine(new Pen(new SolidBrush(_Theme.ChartAxisColor), 1), AxisPoints[0], AxisPoints[1]);
                                                                        }

                                                                        // Y Axis
                                                                        AxisPoint2 = new PointF(ChartExtent.Left, ChartExtent.Top + ChartExtent.Height);
                                                                        if (_Theme.ShowChartAxis)
                                                                        {
                                                                            //gpShp.AddLine(AxisPoint1, AxisPoint2);
                                                                            AxisPoints[0] = AxisPoint1;
                                                                            AxisPoints[1] = AxisPoint2;
                                                                            mTransMatrix.TransformPoints(AxisPoints);
                                                                            g.DrawLine(new Pen(new SolidBrush(_Theme.ChartAxisColor), 1), AxisPoints[0], AxisPoints[1]);
                                                                        }

                                                                        // Get Y Axis height
                                                                        LineChartYAxisLenght = ChartExtent.Height;

                                                                        // - Get horizontal distance between two consecutive points
                                                                        UnitWidth = ChartExtent.Width / (VisibleTimePeriods + 1);

                                                                        // -- Set Start Point same as 
                                                                        LineStartPoint = new PointF(ChartExtent.Left + (float)(UnitWidth * TimeVisibleCtr), ChartExtent.Top + (float)(DataValue / LineMaxDataValue) * LineChartYAxisLenght);

                                                                        AxisDrawn = true;
                                                                    }

                                                                    //-- Draw Points for given DataValue 
                                                                    LineEndPoint = new PointF(ChartExtent.Left + (float)(UnitWidth * TimeVisibleCtr), ChartExtent.Top + (float)(DataValue / LineMaxDataValue) * LineChartYAxisLenght);

                                                                    if (LineSubgroupPoints[i] != null)
                                                                    {
                                                                        //-- Join  Line from previous Point to New Point
                                                                        //-  In first loop, line will start from origin
                                                                        LineStartPoint = (PointF)LineSubgroupPoints[i];
                                                                        //PointF[] Line = { LineStartPoint, LineEndPoint, new PointF(LineEndPoint.X, LineEndPoint.Y - 0.02F), new PointF(LineStartPoint.X, LineStartPoint.Y - 0.02F) };
                                                                        //gpShp.AddPolygon(Line);
                                                                        LinePoints = new PointF[] { LineStartPoint, LineEndPoint };
                                                                        mTransMatrix.TransformPoints(LinePoints);
                                                                        g.DrawLine(new Pen(BrIS[i], LineThickness), LinePoints[0], LinePoints[1]);
                                                                    }
                                                                    //-- Draw a dot circle on Point
                                                                    float PointDiameter = CurExt.Height / 400;
                                                                    gpShp.AddEllipse(LineEndPoint.X - PointDiameter, LineEndPoint.Y - PointDiameter, PointDiameter * 2, PointDiameter * 2);

                                                                    //- Set End point as Starting point for next Point to draw.
                                                                    LineSubgroupPoints[i] = LineEndPoint;
                                                                }
                                                                #endregion

                                                                #region "-- PIE --"

                                                                else if (_Theme.ChartType == ChartType.Pie)
                                                                {
                                                                    if (!(string.IsNullOrEmpty(DataArr[i])) && DataValue > -1 && DataValueSum > 0)
                                                                    {
                                                                        //--Increment Start Angle for next Pie in loop
                                                                        StartAngle += PieAngle;

                                                                        if (j == SpCnt - 1)
                                                                        {
                                                                            PieAngle = 360 - StartAngle;
                                                                        }
                                                                        else
                                                                        {
                                                                            PieAngle = (int)(DataValue / DataValueSum * 360);    // (decimal.Parse(DataArr[i])
                                                                        }
                                                                        gpShp.Reset();
                                                                        //gpShp.AddPie((int)(PieRectExtent.X + PieRectExtent.Width * 0.5), (int)(PieRectExtent.Y + PieRectExtent.Height * 0.4), (int)(PieRectExtent.Height * 0.5), (int)(PieRectExtent.Height * 0.5), StartAngle, PieAngle);
                                                                        //gpShp.AddPie((int)(PieRectExtent.X), (int)PieRectExtent.Y, (int)PieRectExtent.Width , (int)PieRectExtent.Height, StartAngle, PieAngle);
                                                                        if (StartAngle == 0 && PieAngle == 360)
                                                                        {
                                                                            gpShp.AddEllipse(ChartExtent);
                                                                        }
                                                                        else
                                                                        {
                                                                            gpShp.AddPie(ChartExtent.X, ChartExtent.Y, ChartExtent.Width, ChartExtent.Height, StartAngle, PieAngle);
                                                                        }


                                                                    }
                                                                }

                                                                #endregion

                                                                #region "-- Draw Chart Shape --"

                                                                //*** Draw Column or Pie, or Line Chart
                                                                gpShp.Transform(mTransMatrix);

                                                                if (bSearch)
                                                                {
                                                                    if (gpShp.IsVisible(PtX, PtY))
                                                                    {
                                                                        AreaId = _Shape.AreaId;
                                                                        LayerId = Lyr.ID;
                                                                    }
                                                                    if (_Theme.ChartType == ChartType.Column)
                                                                    {
                                                                        gPath.AddPolygon(Pts);
                                                                    }
                                                                    else if (_Theme.ChartType == ChartType.Line)
                                                                    {
                                                                        gPath.AddRectangle(ChartExtent);
                                                                    }
                                                                    else if (_Theme.ChartType == ChartType.Pie)
                                                                    {
                                                                        gPath.AddPie(ChartExtent.X, ChartExtent.Y, ChartExtent.Width, ChartExtent.Height, StartAngle, PieAngle);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    g.SmoothingMode = SmoothingMode.AntiAlias;
                                                                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                                                    //PathGradientBrush LBrush = new PathGradientBrush(gpShp);
                                                                    //LBrush.SurroundColors = new Color[] { Color.Red, Color.Green, Color.Blue };
                                                                    // LBrush.CenterColor = Color.Gray;

                                                                    g.FillPath(BrIS[i], gpShp);  //LBrush

                                                                    //LBrush.Dispose();
                                                                    if (_Theme.ChartType == ChartType.Pie)
                                                                    {
                                                                        //-- In case of Pie, draw shape border with gray
                                                                        g.DrawPath(Pens.DarkGray, gpShp);
                                                                    }
                                                                    else
                                                                    {
                                                                        g.DrawPath(Pens.Black, gpShp);
                                                                    }


                                                                    //*** Draw Data Value 
                                                                    // Draw data Value if DataValueSum is >0 OR ( visible TimePeriods >= 2 in Line Chart)
                                                                    if (!(string.IsNullOrEmpty(DataArr[i])) && (_Theme.DisplayChartData == true && DataValueSum != 0) && (!(_Theme.ChartType == ChartType.Line && VisibleTimePeriods < 2)))
                                                                    {
                                                                        //--DataValue Label's X, Y position 
                                                                        switch (_Theme.ChartType)
                                                                        {
                                                                            case ChartType.Column:
                                                                                Pt.X = Pt.X + (float)UnitWidth / 2;
                                                                                Pt.Y = Pt.Y;        // + 0.4F;
                                                                                break;
                                                                            case ChartType.Pie:
                                                                                //--Label's X, Y position will be calculated 
                                                                                // on the basis of StartAngle & PieAngle of next subsequent Pie.
                                                                                if (!(string.IsNullOrEmpty(DataArr[i])) && DataValue > -1 && DataValueSum > 0)
                                                                                {
                                                                                    //- X
                                                                                    double angle;
                                                                                    angle = Math.PI * (StartAngle + (PieAngle / 2)) / 180.0;
                                                                                    double p = Math.Cos(angle);
                                                                                    Pt.X = (float)(((ChartExtent.Left + ChartExtent.Right) / 2) + (ChartExtent.Width / 1.7) * p);

                                                                                    //-Y
                                                                                    p = Math.Sin(angle);
                                                                                    Pt.Y = (float)(((ChartExtent.Top + ChartExtent.Bottom) / 2) + (ChartExtent.Height / 1.7) * p);
                                                                                }
                                                                                break;
                                                                            case ChartType.Line:
                                                                                Pt = LineEndPoint;
                                                                                Pt.Y += CurExt.Height / 100;    //- Drawing dataValue at some distance from actual plotted point.
                                                                                break;
                                                                        }

                                                                        PtData[0] = Pt;
                                                                        mTransMatrix.TransformPoints(PtData);
                                                                        //g.DrawString(DataValue.ToString(), TextFont, SystemBrushes.WindowText, PtData[0], _StringFormat);

                                                                        //-- Draw Data Value with orientation
                                                                        this.DrawString(g, DataValue.ToString(), true, TextFont, ChartDataLabelBrush, PtData[0], _Theme.ChartDataLabelSettings.YOffset, _StringFormat, _Theme.ChartDataLabelSettings.TextOrientationAngle);
                                                                    }

                                                                }
                                                                Pts = null;
                                                                #endregion
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            Console.Write(ex.Message);
                                                        }

                                                        j += 1;
                                                    }
                                                    //Subgroup Visibility
                                                }// Subgroups loop ends here.

                                                //BarCtr++;   // Bar counter increment to give gap between two timeSeries
                                            }



                                            #region "-- Draw TimePeriod in X Axis--"

                                            //-- Draw TimePeriod Text in X- Axis (Applicable in Column & Line)
                                            if (!(_Theme.DisplayChartMRD) && _Theme.ShowChartAxis && ChartData.Contains(TimePeriodKey) && _Theme.ChartTimePeriods[TimePeriodKey] == true && SpCnt > 0 && _Theme.ChartType != ChartType.Pie)
                                            {
                                                if (DataValueSum != 0)
                                                {
                                                    if (_Theme.ChartType == ChartType.Column)
                                                    {
                                                        //Pt.X = AxisPoint1.X + ((float)UnitWidth * SpCnt * TimeCtr) + (float)UnitWidth;
                                                        Pt.X = AxisPoint1.X + ((float)UnitWidth * (TimeVisibleCtr - 1) * SpCnt) + (ColumnGap * (TimeVisibleCtr - 1)) + (float)UnitWidth * 0.5F * SpCnt;
                                                        Pt.Y = AxisPoint1.Y - (CurExt.Height / 90);
                                                        PtData[0] = Pt;
                                                        mTransMatrix.TransformPoints(PtData);
                                                    }
                                                    else if (_Theme.ChartType == ChartType.Line && VisibleTimePeriods > 1)
                                                    {
                                                        PtData[0].X = LineEndPoint.X;
                                                        PtData[0].Y = AxisPoint1.Y - (CurExt.Height / 70);
                                                        mTransMatrix.TransformPoints(PtData);
                                                    }

                                                    //g.DrawString(TimePeriodKey.ToString(), ChartAxisLabelFont, ChartAxisLabelBrush, PtData[0], _StringFormat);

                                                    //-- Draw Data Value with orientation
                                                    this.DrawString(g, TimePeriodKey.ToString(), false, ChartAxisLabelFont, ChartAxisLabelBrush, PtData[0], _Theme.ChartAxisLabelSettings.YOffset, _StringFormat, _Theme.ChartAxisLabelSettings.TextOrientationAngle);
                                                }
                                            }

                                            if (((_Theme.DisplayChartMRD) && _Theme.ChartType == ChartType.Column) || (_Theme.ChartType == ChartType.Pie))
                                            {
                                                break;      // in MRD case, only Only Most recent dataValue for Subgroup/ Source series will be plotted in Column chart
                                            }
                                            #endregion

                                        }
                                    }


                                } // TimePeriod Loop ends

                                //For each subgroup
                                if (bSearch)
                                {
                                    if (AreaId != "")
                                    {
                                        gPath.Transform(mTransMatrix);
                                        return;
                                    }
                                    //TODO dispose objects
                                    //TODO case when same areaid might belong to multiple layers
                                    else
                                    {
                                        gPath.Reset();
                                    }
                                }
                            }
                            //Non Missing data Shapes
                        }
                        //Traverse Shapes
                    }
                    //Extent-Visibility-Type

                }
                //Traverse Layers collection

            }

            //*** Dispose
            //gPath = New GraphicsPath
            _Shape = null;
            // Pt = null;


            for (i = 0; i <= BrIS.Length - 1; i++)
            {
                BrIS[i].Dispose();
            }
            PnLeader.Dispose();

        }

        /// <summary>
        /// Draws label string in oriented way.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="labelCaption"></param>
        /// <param name="labelFont"></param>
        /// <param name="labelBrush"></param>
        /// <param name="yOffset">Y Offset value (in pixel)</param>
        /// <param name="xyLocation"></param>
        /// <param name="_stringFormat"></param>
        /// <param name="orientationAngle">-90 to +90 degrees</param>
        private void DrawString(Graphics g, string labelCaption, bool ShowLabelAboveChart, Font labelFont, Brush labelBrush, PointF xyLocation, int yOffset, StringFormat _stringFormat, int orientationAngle)
        {
            try
            {
                //-- Apply Y Offset Settings
                xyLocation.Y += (-1) * yOffset;

                //- Get Label's measurement
                SizeF labelSize = g.MeasureString(labelCaption, labelFont);

                if (orientationAngle != 0)
                {
                    orientationAngle = orientationAngle * -1;

                    //-- To draw orientated text label,
                    // draw string in new graphics object with Rotation applied,
                    // convert that into image object and then add image object into original graphics object

                    StringFormat NewStringFormat = (StringFormat)(_stringFormat.Clone());
                    NewStringFormat.Alignment = StringAlignment.Near;

                    //-- Get image rectangular Area considering Label's orientationAngle too (height should accomodate orientation)
                    Bitmap stringImg = new Bitmap((int)labelSize.Width + 10, (int)(labelSize.Width) + 10);

                    Graphics gbitmap = Graphics.FromImage(stringImg);

                    gbitmap.SmoothingMode = SmoothingMode.AntiAlias;
                    gbitmap.Clear(Color.Transparent);

                    PointF xyPos = new PointF();
                    xyPos.X = 0 - labelSize.Width / 2;
                    xyPos.Y = 0 - labelSize.Height / 2;

                    int TextOffsetX = (int)(labelSize.Height / 2);

                    //-- Calculate transformed distance caused due to rotation
                    float XTransformed = TextOffsetX + labelSize.Width / 2;
                    float YTransformed = TextOffsetX + labelSize.Width / 2;

                    gbitmap.TranslateTransform(XTransformed, YTransformed);
                    gbitmap.RotateTransform(orientationAngle);

                    gbitmap.DrawString(labelCaption, labelFont, labelBrush, xyPos, NewStringFormat);

                    //add image object into original graphics object
                    if (ShowLabelAboveChart)
                    {
                        //-- Show label above Chart (i.e Case of DataValue Label)
                        g.DrawImage(stringImg, xyLocation.X - (labelSize.Width / 2), xyLocation.Y - (labelSize.Width * 0.75F) - (labelSize.Width * 0.56F) * (float)(Math.Sin(Math.Abs(orientationAngle) * Math.PI / 180)));
                    }
                    else
                    {
                        //-- Show label below Chart (i.e Case of TimePeriod Label)
                        g.DrawImage(stringImg, xyLocation.X + 2 - (labelSize.Width / 2) - TextOffsetX / 2, xyLocation.Y - (labelSize.Height / 2) - TextOffsetX);
                    }

                    //dispose your bitmap and graphics objects at the end of onpaint
                    gbitmap.Dispose();
                    stringImg.Dispose();
                }
                else
                {
                    if (ShowLabelAboveChart)
                    {
                        xyLocation.Y = xyLocation.Y - (labelSize.Height * 0.5F);
                    }
                    g.DrawString(labelCaption, labelFont, labelBrush, xyLocation, _stringFormat);
                }
            }
            catch
            {
            }
        }

        private void DrawString(Graphics g, string labelCaption, Font labelFont, Brush labelBrush, PointF xyLocation, StringFormat stringFormat, LabelCase labelCase, int characterSpaceInPixels)
        {
            if (labelCase == LabelCase.LowerCase)
            {
                labelCaption = labelCaption.ToLower();
            }
            else if (labelCase == LabelCase.UpperCase)
            {
                labelCaption = labelCaption.ToUpper();
            }

            if (characterSpaceInPixels > 0)
            {
                StringFormat GenericTypoGraphicFormat = StringFormat.GenericTypographic;
                GenericTypoGraphicFormat.Alignment = StringAlignment.Near;
                GenericTypoGraphicFormat.LineAlignment = StringAlignment.Center;
                GenericTypoGraphicFormat.FormatFlags = StringFormatFlags.NoClip;

                //- Starting position of first Letter.
                float x = xyLocation.X;

                // Adjust starting position of first Letter 
                // so that central Letter comes at the centroid of Area.
                SizeF TotalLength = g.MeasureString(labelCaption, labelFont);
                TotalLength.Width += (labelCaption.Length - 1) * characterSpaceInPixels;
                x = x - (TotalLength.Width / 2);

                //- iterate each letter in Label and Draw letter with specified spacing.
                for (int i = 0; i < labelCaption.Length; i++)
                {
                    g.DrawString(labelCaption.Substring(i, 1), labelFont, labelBrush, new PointF(x, xyLocation.Y), GenericTypoGraphicFormat);
                    SizeF sf = g.MeasureString(labelCaption.Substring(i, 1), labelFont, new PointF(x, xyLocation.Y), GenericTypoGraphicFormat);
                    x += (float)(sf.Width + characterSpaceInPixels);
                }

                GenericTypoGraphicFormat.Dispose();
            }
            else
            {
                //- Draw String in Simple way.
                g.DrawString(labelCaption, labelFont, labelBrush, xyLocation, stringFormat);
            }
        }

        private void DrawSymbolTheme(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt)
        {

            Shape _Shape;
            //Polygon : Polyline : Point
            PointF[] Centroid = new PointF[1];
            Size Offset = new Size();
            Offset.Width = _Theme.X_Offset;
            Offset.Height = -(_Theme.Y_Offset);

            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;
            //object[] BrLegend = new object[_Theme.Legends.Count];
            SolidBrush[] SolidBrushArray = new SolidBrush[_Theme.Legends.Count];
            Image[] ImageArray = new Image[_Theme.Legends.Count];

            Legend _Legend;
            int LegendItemIndex;
            for (int i = 0; i <= _Theme.Legends.Count - 1; i++)
            {
                _Legend = _Theme.Legends[i];
                if (_Legend.SymbolImage == "" || File.Exists(_Legend.SymbolImage) == false)
                {
                    SolidBrushArray[i] = new SolidBrush(_Legend.Color);
                }
                else
                {
                    ImageArray[i] = Image.FromFile(_Legend.SymbolImage);
                }
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;        //AntiAlias
            foreach (Layer Lyr in Layers)
            {
                //Traverse Layers collection
                {
                    if ((Lyr.LayerType == ShapeType.Polygon | Lyr.LayerType == ShapeType.PolyLine | Lyr.LayerType == ShapeType.Point) & Lyr.Visible == true)
                    {
                        if (Lyr.Extent.IntersectsWith(CurExt) || CurExt.Contains(Lyr.Extent) || Lyr.Extent.Contains(CurExt))
                        {
                            // Render layer only if it lies within current map extent
                            Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                            IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                            while (dicEnumerator.MoveNext())
                            {
                                //Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                switch (Lyr.LayerType)
                                {
                                    case ShapeType.Point:
                                        Centroid[0] = (PointF)_Shape.Parts[0];
                                        break;
                                    case ShapeType.PolyLine:
                                        Centroid[0] = (PointF)_Shape.Parts[(int)_Shape.Parts.Count / 2 - 1];
                                        break;
                                    case ShapeType.Polygon:
                                        Centroid[0] = _Shape.Centroid;
                                        break;
                                }
                                if (CurExt.Contains(Centroid[0]))
                                {
                                    //Render shape only if it lies within current map extent
                                    mTransMatrix.TransformPoints(Centroid);
                                    //*** Apply Offset values
                                    Centroid[0] = PointF.Add(Centroid[0], Offset);
                                    {
                                        //TODO Handle MultiLegend case
                                        if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                                        {
                                            LegendItemIndex = (int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo;
                                        }
                                        else
                                        {
                                            LegendItemIndex = _Theme.Legends.Count - 1;
                                        }
                                        if (_Theme.Legends[LegendItemIndex].SymbolImage == "" || File.Exists(_Theme.Legends[LegendItemIndex].SymbolImage) == false)
                                        {
                                            g.DrawString(_Theme.Legends[LegendItemIndex].MarkerChar.ToString(), _Theme.Legends[LegendItemIndex].MarkerFont, SolidBrushArray[LegendItemIndex], Centroid[0].X, Centroid[0].Y, _StringFormat);
                                        }
                                        else
                                        {
                                            Centroid[0].X = Centroid[0].X - (ImageArray[LegendItemIndex]).Width / 2;
                                            Centroid[0].Y = Centroid[0].Y - (ImageArray[LegendItemIndex]).Height / 2;
                                            g.DrawImage(ImageArray[LegendItemIndex], Centroid[0]);
                                        }
                                    }
                                }
                            }
                            //Traverse Shapes
                            ht = null;
                        }
                    }
                }

            }
            //Traverse Layers collection
            g.SmoothingMode = SmoothingMode.None;

            for (int i = 0; i <= _Theme.Legends.Count - 1; i++)
            {
                if (SolidBrushArray[i] != null)
                {
                    SolidBrushArray[i].Dispose();
                }

                if (ImageArray[i] != null)
                {
                    ImageArray[i].Dispose();
                }
            }
            _StringFormat.Dispose();
            _Shape = null;
        }

        private void DrawLabelTheme(ref Graphics g, Theme _Theme, Matrix mTransMatrix, RectangleF CurExt)
        {

            string Label = "";
            Shape _Shape;
            //Polygon : Polyline : Point
            PointF[] Centroid = new PointF[1];
            Size Offset = new Size(0, 0);
            Offset.Width = _Theme.X_Offset;
            Offset.Height = -(_Theme.Y_Offset);

            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            SolidBrush[] BrLegend = new SolidBrush[_Theme.Legends.Count];
            Legend _Legend;
            int LegendItemIndex;
            for (int i = 0; i <= _Theme.Legends.Count - 1; i++)
            {
                _Legend = _Theme.Legends[i];
                BrLegend[i] = new SolidBrush(_Legend.Color);
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            foreach (Layer Lyr in Layers)
            {
                //Traverse Layers collection
                {
                    if ((Lyr.LayerType == ShapeType.Polygon | Lyr.LayerType == ShapeType.PolyLine | Lyr.LayerType == ShapeType.Point) & Lyr.Visible == true)
                    {
                        if (Lyr.Extent.IntersectsWith(CurExt) || CurExt.Contains(Lyr.Extent) || Lyr.Extent.Contains(CurExt))
                        {
                            // Render layer only if it lies within current map extent
                            Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                            IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                            while (dicEnumerator.MoveNext())
                            {
                                //Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                switch (Lyr.LayerType)
                                {
                                    case ShapeType.Point:
                                        Centroid[0] = (PointF)_Shape.Parts[0];
                                        break;
                                    case ShapeType.PolyLine:
                                        Centroid[0] = (PointF)_Shape.Parts[(int)_Shape.Parts.Count / 2 - 1];
                                        break;
                                    case ShapeType.Polygon:
                                        Centroid[0] = _Shape.Centroid;
                                        break;
                                }
                                if (CurExt.Contains(Centroid[0]))
                                {
                                    //Render shape only if it lies within current map extent
                                    mTransMatrix.TransformPoints(Centroid);
                                    //*** Apply Offset values
                                    Centroid[0] = PointF.Add(Centroid[0], Offset);
                                    {
                                        if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                                        {
                                            LegendItemIndex = (int)((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).RenderingInfo;
                                        }
                                        else
                                        {
                                            LegendItemIndex = _Theme.Legends.Count - 1;
                                        }
                                        _Legend = _Theme.Legends[LegendItemIndex];
                                        if (_Legend.LabelVisible == true)
                                        {
                                            Label = GetLabel(Lyr, ref _Shape, ref _Theme, null, ref _Legend);
                                            if (_Legend.LabelMultiRow == true)
                                                Centroid[0].X = Centroid[0].X - g.MeasureString(Label, _Legend.MarkerFont).Width / 4;
                                            if (_Legend.LabelMultiRow == true)
                                            {
                                                _StringFormat.Alignment = StringAlignment.Near;
                                            }
                                            else
                                            {
                                                _StringFormat.Alignment = StringAlignment.Center;
                                            }
                                            g.DrawString(Label, _Legend.MarkerFont, BrLegend[LegendItemIndex], Centroid[0].X, Centroid[0].Y, _StringFormat);
                                        }
                                    }
                                }
                            }
                            //Traverse Shapes
                            ht = null;
                        }
                    }
                }
                // Lyr = null;
            }
            //Traverse Layers collection
            g.SmoothingMode = SmoothingMode.None;

            for (int i = 0; i <= BrLegend.Length - 1; i++)
            {
                BrLegend[i].Dispose();
            }
            _StringFormat.Dispose();
            _Shape = null;
        }

        private void DrawCustomFeatureLayers(ref Graphics g, Matrix mTransMatrix, RectangleF CurExt)
        {

            Shape _Shape;
            //Polygon : Polyline : Point
            AreaInfo _AreaInfo;
            Theme _Theme;
            string sFIKey = "";
            PointF[] Centroid = new PointF[1];
            ArrayList oFICol = new ArrayList();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int i;
            int j;
            foreach (Layer Lyr in Layers)
            {
                //Traverse Layers collection
                {
                    if ((Lyr.LayerType == ShapeType.Polygon | Lyr.LayerType == ShapeType.PolyLine | Lyr.LayerType == ShapeType.Point) & Lyr.Visible == true)
                    {
                        if (Lyr.Extent.IntersectsWith(CurExt) || CurExt.Contains(Lyr.Extent) || Lyr.Extent.Contains(CurExt))
                        {
                            // Render layer only if it lies within current map extent
                            Hashtable ht = Lyr.GetRecords(Lyr.LayerPath + "\\" + Lyr.ID);
                            IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                            while (dicEnumerator.MoveNext())
                            {
                                //Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;
                                oFICol.Clear();
                                for (i = 0; i <= m_Themes.Count - 1; i++)
                                {
                                    //Traverse Themes Handle Multiple Theme
                                    _Theme = m_Themes[i];
                                    if (_Theme.Visible == true)
                                    {
                                        if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                                        {
                                            _AreaInfo = (AreaInfo)_Theme.AreaIndexes[_Shape.AreaId];
                                            switch (Lyr.LayerType)
                                            {
                                                case ShapeType.Point:
                                                    Centroid[0] = (PointF)_Shape.Parts[0];
                                                    break;
                                                case ShapeType.PolyLine:
                                                    Centroid[0] = (PointF)_Shape.Parts[(int)_Shape.Parts.Count / 2 - 1];
                                                    break;
                                                case ShapeType.Polygon:
                                                    Centroid[0] = _Shape.Centroid;
                                                    break;
                                            }
                                            if (CurExt.Contains(Centroid[0]))
                                            {
                                                //Render shape only if it lies within current map extent
                                                mTransMatrix.TransformPoints(Centroid);

                                                //*** Draw Custom Feature Symbols
                                                CustomFeature oCFL;
                                                FeatureInfo oFI;
                                                for (j = m_CFLCol.Count - 1; j >= 0; j += -1)
                                                {
                                                    //Traverse Custom Feature Layers
                                                    try
                                                    {
                                                        oCFL = (CustomFeature)m_CFLCol[j];
                                                        if (oCFL.Visible == true)
                                                        {
                                                            //*** Hashtable keys are case sensitive so use lower case (while creation and comparision)
                                                            sFIKey = "";
                                                            switch (oCFL.FeatureType)
                                                            {
                                                                case CustomFeature.FeatureField.Source:
                                                                    sFIKey = _AreaInfo.Source.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.Area:
                                                                    sFIKey = _Shape.AreaId.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.Indicator:
                                                                    sFIKey = _AreaInfo.IndicatorGID.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.Unit:
                                                                    sFIKey = _AreaInfo.UnitGID.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.Subgroup:
                                                                    sFIKey = _AreaInfo.SubgroupGID.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.TimePeriod:
                                                                    sFIKey = _AreaInfo.Time.ToLower();
                                                                    break;
                                                                case CustomFeature.FeatureField.IndicatorClassification:
                                                                    break;
                                                                case CustomFeature.FeatureField.Metadata:
                                                                    sFIKey = _AreaInfo.MDFldVal[oCFL.MetadataField].ToString().ToLower();
                                                                    break;
                                                            }
                                                            if (oCFL.FeatureCol.ContainsKey(sFIKey))
                                                            {
                                                                oFI = (FeatureInfo)oCFL.FeatureCol[sFIKey];
                                                                //*** add feature info to collection only if it doesn't exist already
                                                                if (oFICol.Contains(oFI) == false)
                                                                {
                                                                    oFICol.Add(oFI);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Debug.Write(ex.Message);
                                                    }
                                                }
                                                //Traverse Custom Feature Layers
                                            }
                                        }
                                        else
                                        {
                                            //If Theme does not contain area Index then handle missing data Area for Area CFL

                                            switch (Lyr.LayerType)
                                            {
                                                case ShapeType.Point:
                                                    Centroid[0] = (PointF)_Shape.Parts[0];
                                                    break;
                                                case ShapeType.PolyLine:
                                                    Centroid[0] = (PointF)_Shape.Parts[(int)_Shape.Parts.Count / 2 - 1];
                                                    break;
                                                case ShapeType.Polygon:
                                                    Centroid[0] = _Shape.Centroid;
                                                    break;
                                            }
                                            if (CurExt.Contains(Centroid[0]))
                                            {
                                                //Render shape only if it lies within current map extent
                                                mTransMatrix.TransformPoints(Centroid);

                                                //*** Draw Custom Feature Symbols
                                                CustomFeature oCFL;
                                                FeatureInfo oFI;
                                                for (j = m_CFLCol.Count - 1; j >= 0; j += -1)
                                                {
                                                    //Traverse Custom Feature Layers
                                                    try
                                                    {
                                                        oCFL = (CustomFeature)m_CFLCol[j];
                                                        if (oCFL.Visible == true)
                                                        {
                                                            //TODO Handle all cases
                                                            //*** Hashtable keys are case sensitive so use lower case (while creation and comparision)
                                                            sFIKey = "";
                                                            switch (oCFL.FeatureType)
                                                            {
                                                                case CustomFeature.FeatureField.Area:
                                                                    sFIKey = _Shape.AreaId.ToLower();
                                                                    break;
                                                            }
                                                            if (oCFL.FeatureCol.ContainsKey(sFIKey))
                                                            {
                                                                oFI = (FeatureInfo)oCFL.FeatureCol[sFIKey];
                                                                //*** add feature info to collection only if it doesn't exist already
                                                                if (oFICol.Contains(oFI) == false)
                                                                {
                                                                    oFICol.Add(oFI);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Debug.Write(ex.Message);
                                                    }
                                                }
                                                //Traverse Custom Feature Layers
                                            }
                                        }
                                    }
                                }
                                //Traverse Themes
                                DrawCutomFeature(ref g, ref mTransMatrix, _Shape, ref oFICol, Centroid[0]);
                            }
                            //Traverse Shapes
                            ht = null;
                        }
                    }
                }

            }
            //Traverse Layers collection
            g.SmoothingMode = SmoothingMode.None;

            _Shape = null;
        }

        private void DrawCutomFeature(ref Graphics g, ref Matrix mTransMatrix, Shape _Shape, ref ArrayList oFICol, PointF DrawPoint)
        {
            int i;
            int j;
            int iSpacer = 2;
            GraphicsPath gpShp = new GraphicsPath();
            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            FeatureInfo ofi;
            float iSymbolWidth = 0;
            float iSymbolOffset;
            PointF SymPoint = DrawPoint;
            float iLabelOffset = 0;
            SizeF szChar;
            for (i = 0; i <= oFICol.Count - 1; i++)
            {
                ofi = (FeatureInfo)oFICol[i];
                if (ofi.SymbolSet == true)
                {
                    if ((ofi.SymbolImage == null))
                    {
                        szChar = g.MeasureString(ofi.SymbolChar.ToString(), ofi.SymbolFont);
                        iSymbolWidth += szChar.Width + iSpacer;
                        iLabelOffset = Math.Max(iLabelOffset, szChar.Height / 2);
                    }
                    else
                    {
                        iSymbolWidth += ofi.SymbolImage.Width + iSpacer;
                        iLabelOffset = Math.Max(iLabelOffset, ofi.SymbolImage.Height / 2);
                    }
                }
            }
            iSymbolOffset = iSymbolWidth / 2;
            SymPoint.X = SymPoint.X - iSymbolOffset;

            for (i = 0; i <= oFICol.Count - 1; i++)
            {
                ofi = (FeatureInfo)oFICol[i];
                //*** Render Symbol
                if (ofi.SymbolSet == true)
                {
                    _StringFormat.Alignment = StringAlignment.Near;
                    _StringFormat.LineAlignment = StringAlignment.Center;
                    if ((ofi.SymbolImage == null))
                    {
                        SymPoint.Y = DrawPoint.Y;
                        g.DrawString(ofi.SymbolChar.ToString(), ofi.SymbolFont, new SolidBrush(ofi.SymbolColor), SymPoint.X, SymPoint.Y, _StringFormat);
                        SymPoint.X += g.MeasureString(ofi.SymbolChar.ToString(), ofi.SymbolFont).Width + iSpacer;
                    }
                    else
                    {
                        SymPoint.Y = DrawPoint.Y - ofi.SymbolImage.Height / 2;
                        g.DrawImage(ofi.SymbolImage, SymPoint);
                        SymPoint.X += ofi.SymbolImage.Width + iSpacer;
                    }
                }
                //*** Render Label
                if (ofi.LabelSet == true)
                {
                    //*** Place label on top of images
                    _StringFormat.Alignment = StringAlignment.Center;
                    _StringFormat.LineAlignment = StringAlignment.Far;
                    g.DrawString(ofi.Caption, ofi.LabelFont, new SolidBrush(ofi.LabelColor), DrawPoint.X, DrawPoint.Y - iLabelOffset, _StringFormat);
                }
                //*** Render Fill Style
                if (ofi.FillSet == true)
                {
                    gpShp.Reset();
                    for (j = 0; j <= _Shape.Parts.Count - 1; j++)
                    {
                        gpShp.AddPolygon((PointF[])_Shape.Parts[j]);
                    }
                    Pen PnFill = new Pen(ofi.BorderColor, ofi.BorderSize);
                    Brush BrFill;
                    if (ofi.BorderSize == 0)
                        PnFill.Color = Color.Transparent;
                    //??? Strange that setting Pen width = 0 gives effect of Pen width 1. So this workaround
                    PnFill.DashStyle = ofi.BorderStyle;

                    if (ofi.FillStyle == FillStyle.Solid)
                    {
                        BrFill = new SolidBrush(ofi.FillColor);
                    }
                    else if (ofi.FillStyle == FillStyle.Transparent)
                    {
                        BrFill = new SolidBrush(Color.Transparent);
                    }
                    else
                    {
                        BrFill = new HatchBrush((HatchStyle)ofi.FillStyle, ofi.FillColor, Color.Transparent);
                    }
                    gpShp.Transform(mTransMatrix);
                    g.FillPath(BrFill, gpShp);
                    try
                    {
                        g.DrawPath(PnFill, gpShp);
                    }
                    catch (Exception ex)
                    {
                        Debug.Write(ex.Message);
                        //*** Gives error sometimes when Pen border size > 1
                        //http://msdn2.microsoft.com/en-us/library/ms969928.aspx
                    }

                    PnFill.Dispose();
                    PnFill = null;
                    BrFill.Dispose();
                    BrFill = null;
                }
            }
            _StringFormat.Dispose();
            gpShp.Dispose();

        }

        private void DrawLabel(ref Graphics g, Matrix mTransMatrix, RectangleF CurExt)
        {
            bool RectSearch = false;
            float X = 0;
            float Y = 0;
            string LayerId = "";
            string AreaId = "";
            string Caption = "";
            string Rect = "";
            string FontName = "Arial";
            int FontSize = 8;
            int FontStyle = 0;
            int ForeColor = 0;
            DrawLabel(ref g, mTransMatrix, CurExt, RectSearch, X, Y, ref LayerId, ref AreaId, ref Caption, ref Rect, ref FontName, ref FontSize, ref FontStyle, ref ForeColor);
        }

        private void DrawLabel(ref Graphics g, Matrix mTransMatrix, RectangleF CurExt, bool RectSearch, float X, float Y, ref string LayerId, ref string AreaId, ref string Caption, ref string Rect, ref string FontName, ref int FontSize, ref int FontStyle, ref int ForeColor)
        {
            //*** BugFix 02 Feb 2006 Loss of font style on label nudge. ByVal parameter instead of ByRef
            string Label;
            Theme _Theme;
            //Layer _Layer;
            Shape _Shape;
            StringFormat _StringFormat = new StringFormat();
            _StringFormat.Alignment = StringAlignment.Center;
            _StringFormat.LineAlignment = StringAlignment.Center;
            _StringFormat.FormatFlags = StringFormatFlags.NoClip;

            PointF PtCentroid = new PointF();
            PointF[] Centroid = new PointF[1];
            char[] Fields;
            RectangleF LabelRect = new RectangleF();
            PointF SearchPoint = new PointF(X, Y);
            GraphicsPath gpShp = new GraphicsPath();
            Pen PnLeader = new Pen(Color.Black, 1);
            float xOffset;

            //-- Try to get Active Color theme
            _Theme = m_Themes.GetActiveTheme();

            //- if Active theme is null, then get Selected theme
            if (_Theme == null)
            {
                _Theme = this.GetSelectedTheme();
            }

            //*** Custom Layers
            bool bLayerVisible;

            foreach (Layer _Layer in Layers)
            {
                //*** Traverse Layers collection

                switch (_Layer.LayerType)
                {
                    case ShapeType.PointCustom:
                    case ShapeType.PolyLineCustom:
                    case ShapeType.PolygonCustom:
                    case ShapeType.PolygonBuffer:
                        bLayerVisible = _Layer.Visible;
                        break;
                    default:
                        if ((_Theme == null))
                        {
                            bLayerVisible = _Layer.Visible;
                        }
                        else
                        {
                            bLayerVisible = (bool)_Theme.LayerVisibility[_Layer.ID];
                        }

                        break;
                }

                if (_Layer.Extent.IntersectsWith(CurExt) & bLayerVisible == true)
                {
                    SolidBrush BrLabel = new SolidBrush(_Layer.LabelColor);
                    Font FontLabel = _Layer.LabelFont;
                    Fields = _Layer.LabelField.Replace(",", "").ToCharArray();
                    _StringFormat.Alignment = StringAlignment.Center;
                    if (_Layer.LabelMultirow == true)
                    {
                        _StringFormat.Alignment = StringAlignment.Near;
                    }

                    Hashtable ht = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID);
                    IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                    switch (_Layer.LayerType)
                    {
                        case ShapeType.Point:
                        case ShapeType.PointFeature:
                        case ShapeType.PointCustom:
                        case ShapeType.Polygon:
                        case ShapeType.PolygonFeature:
                        case ShapeType.PolygonCustom:
                        case ShapeType.PolygonBuffer:
                            while (dicEnumerator.MoveNext())
                            {
                                //*** Traverse Shapes
                                _Shape = (Shape)dicEnumerator.Value;

                                if (_Layer.ModifiedLabels.ContainsKey(_Shape.AreaId))
                                {
                                    //*** Handling Modified Label
                                    {
                                        if (((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).Visible == true)
                                        {
                                            switch (_Layer.LayerType)
                                            {
                                                case ShapeType.Point:
                                                case ShapeType.Polygon:
                                                    Label = GetLabel(_Layer, ref _Shape, ref _Theme, (CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]);
                                                    //.Caption
                                                    break;
                                                default:
                                                    Theme _TempTheme = null;   // Temp Theme is created for passing it to GetLabel function below, as C# dont allow to pass null as ref parameter.
                                                    Label = GetLabel(_Layer, ref _Shape, ref _TempTheme, (CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]);
                                                    //.Caption
                                                    break;
                                            }
                                            Centroid[0] = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).DrawPoint;
                                            mTransMatrix.TransformPoints(Centroid);
                                            if (_Layer.LabelMultirow == true)
                                                Centroid[0].X = Centroid[0].X - g.MeasureString(Label, FontLabel).Width / 4;
                                            //*** BugFix 12 Apr 2006 Supress Leader line for nudged label if label is empty
                                            if (RectSearch == false & Label.Trim() != "")
                                            {
                                                //*** Leader Line
                                                if (((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LeaderVisible == true)
                                                {
                                                    PnLeader.Color = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LeaderColor;
                                                    PnLeader.Width = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LeaderWidth;
                                                    PnLeader.DashStyle = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LeaderStyle;

                                                    PointF[] ShpCentroid = new PointF[1];
                                                    mTransMatrix.TransformPoints(ShpCentroid);
                                                    switch (_Layer.LayerType)
                                                    {
                                                        case ShapeType.Polygon:
                                                        case ShapeType.PolygonFeature:
                                                        case ShapeType.PolygonCustom:
                                                        case ShapeType.PolygonBuffer:
                                                            ShpCentroid[0] = _Shape.Centroid;
                                                            break;
                                                        case ShapeType.Point:
                                                        case ShapeType.PointFeature:
                                                        case ShapeType.PointCustom:
                                                            ShpCentroid[0] = (PointF)_Shape.Parts[0];
                                                            break;
                                                    }
                                                    mTransMatrix.TransformPoints(ShpCentroid);
                                                    g.DrawLine(PnLeader, ShpCentroid[0], Centroid[0]);
                                                }

                                                //*** Draw Modified Label
                                                BrLabel.Color = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).ForeColor;
                                                if (_Layer.ShowLabelEffects)
                                                {
                                                    this.DrawLabelWithEffects(_Layer.LabelEffectSettings, g, Label, BrLabel, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont, Centroid[0], _StringFormat, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelCase, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelCharacterSpacing);
                                                }
                                                else
                                                {
                                                    this.DrawString(g, Label, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont, BrLabel, Centroid[0], _StringFormat, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelCase, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelCharacterSpacing);
                                                }
                                                //g.DrawString(Label, ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont, BrLabel, Centroid[0], _StringFormat);
                                                BrLabel.Color = _Layer.LabelColor;
                                            }

                                            else if (RectSearch == true)
                                            {
                                                SearchPoint = PointToClient(X, Y);
                                                if (((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).PlaceHolder.Contains(SearchPoint))
                                                {
                                                    LayerId = _Layer.ID;
                                                    AreaId = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).AreaId;
                                                    switch (_Layer.LayerType)
                                                    {
                                                        case ShapeType.Point:
                                                        case ShapeType.Polygon:
                                                            Caption = GetLabel(_Layer, ref _Shape, ref _Theme, (CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]);
                                                            //.Caption
                                                            break;
                                                        default:
                                                            Theme _TempTheme = null;   // Temp Theme is created for passing it to GetLabel function below, as C# dont allow to pass null as ref parameter.
                                                            Caption = GetLabel(_Layer, ref _Shape, ref _TempTheme, (CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]);
                                                            //.Caption
                                                            break;
                                                    }
                                                    LabelRect.Size = g.MeasureString(((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).Caption, FontLabel, Centroid[0], _StringFormat);
                                                    LabelRect.X = Centroid[0].X - LabelRect.Size.Width / 2;
                                                    LabelRect.Y = Centroid[0].Y - LabelRect.Size.Height / 2;
                                                    Rect = LabelRect.X + "," + LabelRect.Y + "," + LabelRect.Width + "," + LabelRect.Height;
                                                    FontName = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont.Name;
                                                    FontSize = (int)((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont.Size;
                                                    FontStyle = (int)((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).LabelFont.Style;
                                                    ForeColor = ((CustomLabel)_Layer.ModifiedLabels[_Shape.AreaId]).ForeColor.ToArgb();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }

                                else if (_Layer.LabelVisible == true)
                                {

                                    //*** Handling Normal Labels
                                    switch (_Layer.LayerType)
                                    {
                                        case ShapeType.Point:
                                        case ShapeType.PointCustom:
                                        case ShapeType.PointFeature:
                                            PtCentroid = (PointF)_Shape.Parts[0];
                                            Centroid[0] = (PointF)_Shape.Parts[0];
                                            break;
                                        case ShapeType.Polygon:
                                        case ShapeType.PolygonCustom:
                                        case ShapeType.PolygonBuffer:
                                        case ShapeType.PolygonFeature:
                                            PtCentroid = _Shape.Centroid;
                                            Centroid[0] = _Shape.Centroid;
                                            break;
                                    }

                                    if (CurExt.Contains(Centroid[0]))
                                    {
                                        //If _Shape.Extent.IntersectsWith(CurExt) Then '??? point type extent
                                        mTransMatrix.TransformPoints(Centroid);
                                        switch (_Layer.LayerType)
                                        {
                                            case ShapeType.Point:
                                            case ShapeType.Polygon:
                                                Label = GetLabel(_Layer, ref _Shape, ref _Theme);
                                                break;
                                            default:
                                                Label = GetLabel(_Layer, ref _Shape);
                                                break;
                                        }
                                        if (_Layer.LabelMultirow == true)
                                            Centroid[0].X = Centroid[0].X - g.MeasureString(Label, FontLabel).Width / 4;

                                        //*** Logic for Shifting label going partially beyond Map boundries
                                        if (CurExt.Contains(PtCentroid))
                                        {
                                            xOffset = Centroid[0].X - g.MeasureString(Label, FontLabel, Centroid[0], _StringFormat).Width / 2;
                                            if (xOffset < 0)
                                            {
                                                Centroid[0].X = Centroid[0].X - xOffset;
                                            }
                                            else
                                            {
                                                xOffset = Centroid[0].X + g.MeasureString(Label, FontLabel, Centroid[0], _StringFormat).Width / 2;
                                                if (xOffset > m_Width & CurExt.Contains(PtCentroid))
                                                {
                                                    Centroid[0].X = Centroid[0].X - (xOffset - m_Width);
                                                }
                                            }
                                        }

                                        if (RectSearch == false)
                                        {
                                            if (CurExt.Contains(PtCentroid))
                                            {
                                                if (this.ShowLabelEffects)
                                                {
                                                    this.DrawLabelWithEffects(this.LabelEffectSettings, g, Label, BrLabel, FontLabel, Centroid[0], _StringFormat, _Layer.LabelCase, _Layer.LabelCharacterSpacing);
                                                }
                                                else if (_Layer.ShowLabelEffects)
                                                {
                                                    this.DrawLabelWithEffects(_Layer.LabelEffectSettings, g, Label, BrLabel, FontLabel, Centroid[0], _StringFormat, _Layer.LabelCase, _Layer.LabelCharacterSpacing);
                                                }
                                                else
                                                {
                                                    //- Draw Label with Character Spacing.
                                                    this.DrawString(g, Label, FontLabel, BrLabel, Centroid[0], _StringFormat, _Layer.LabelCase, _Layer.LabelCharacterSpacing);

                                                    //g.DrawString(Label, FontLabel, BrLabel, Centroid[0], _StringFormat);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            LabelRect.Size = g.MeasureString(Label, FontLabel, Centroid[0], _StringFormat);
                                            LabelRect.X = Centroid[0].X - LabelRect.Size.Width / 2;
                                            LabelRect.Y = Centroid[0].Y - LabelRect.Size.Height / 2;
                                            if (_Layer.LabelMultirow == true)
                                            {
                                                SearchPoint.X = X - g.MeasureString(Label, FontLabel).Width / 2;
                                            }
                                            else
                                            {
                                                SearchPoint.X = X;
                                            }
                                            SearchPoint.Y = Y;
                                            if (LabelRect.Contains(SearchPoint))
                                            {
                                                LayerId = _Layer.ID;
                                                AreaId = _Shape.AreaId;
                                                Caption = Label;
                                                Rect = LabelRect.X + "," + LabelRect.Y + "," + LabelRect.Width + "," + LabelRect.Height;
                                                FontName = FontLabel.FontFamily.Name;
                                                FontSize = (int)FontLabel.Size;
                                                FontStyle = (int)FontLabel.Style;
                                                ForeColor = (int)_Layer.LabelColor.ToArgb();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case ShapeType.PolyLine:
                        case ShapeType.PolyLineCustom:
                        case ShapeType.PolyLineFeature:
                            if (_Layer.LabelVisible == true)
                            {
                                PointF[] Pts;
                                PointF[] LabelPt = new PointF[1];
                                while (dicEnumerator.MoveNext())
                                {
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Label = GetLabel(_Layer, ref _Shape);
                                    Pts = (PointF[])(((PointF[])_Shape.Parts[0]).Clone());
                                    LabelPt[0] = Pts[(int)Pts.Length / 2];
                                    mTransMatrix.TransformPoints(LabelPt);
                                    this.DrawString(g, Label, FontLabel, BrLabel, LabelPt[0], _StringFormat, _Layer.LabelCase, _Layer.LabelCharacterSpacing);
                                    //g.DrawString(Label, FontLabel, BrLabel, LabelPt[0], _StringFormat);
                                }
                            }

                            break;

                        //TODO: Map-Draw string along the line ???
                        //http://www.codeproject.com/csharp/Drawing_Text_Strings.asp

                        //Try
                        // Dim Pts() As PointF
                        // Dim StartPoint, EndPoint As Integer
                        // Dim Rotation As Integer
                        // While dicEnumerator.MoveNext() '*** Traverse Shapes
                        // _Shape = dicEnumerator.Value
                        // Label = _Shape.AreaId 'GetLabel(_Layer, _Shape)
                        // Pts = CType(_Shape.Parts(0), PointF()).Clone
                        // mTransMatrix.TransformPoints(Pts)
                        // If (Pts.Length + 4) > Label.Length Then
                        // StartPoint = (Pts.Length - Label.Length) / 2
                        // EndPoint = StartPoint + Label.Length - 1
                        // For i = StartPoint To EndPoint
                        // g.DrawString(Label.Chars(i - StartPoint), FontLabel, BrLabel, Pts(i))
                        // g.ResetTransform()
                        // Next
                        // End If
                        // End While
                        // Pts = Nothing
                        //Catch ex As Exception
                        // Console.Write(ex.Message)
                        //End Try

                    }
                    BrLabel.Dispose();
                    //FontLabel.Dispose()
                }
            }
            _Theme = null;

            PnLeader.Dispose();
            gpShp = null;
            //  _Layer = null;
            _Shape = null;
            _StringFormat.Dispose();
        }

        public void DrawLabelWithEffects(LabelEffectSetting effectSettings, Graphics g, string labelText, SolidBrush orginalBrush, Font FontLabel, PointF xyCoordinates, StringFormat _StringFormat, LabelCase labelCase, int labelCharacterSpaceInPixels)
        {
            SolidBrush brush = null;
            LinearGradientBrush LinearGBrush = null;

            if (effectSettings != null && string.IsNullOrEmpty(labelText) == false)
            {
                brush = new SolidBrush(effectSettings.EffectColor);

                switch (effectSettings.Effect)
                {
                    case LabelEffect.Shadow:
                        //g.DrawString(labelText, FontLabel, brush, xyCoordinates.X + effectSettings.Depth, xyCoordinates.Y + effectSettings.Depth, _StringFormat);
                        //g.DrawString(labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat);

                        this.DrawString(g, labelText, FontLabel, brush, new PointF(xyCoordinates.X + effectSettings.Depth, xyCoordinates.Y + effectSettings.Depth), _StringFormat, labelCase, labelCharacterSpaceInPixels);
                        this.DrawString(g, labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat, labelCase, labelCharacterSpaceInPixels);

                        break;
                    case LabelEffect.Embossed:
                        //g.DrawString(labelText, FontLabel, brush, xyCoordinates.X + effectSettings.Depth, xyCoordinates.Y + effectSettings.Depth, _StringFormat);
                        //g.DrawString(labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat);

                        this.DrawString(g, labelText, FontLabel, brush, new PointF(xyCoordinates.X + effectSettings.Depth, xyCoordinates.Y + effectSettings.Depth), _StringFormat, labelCase, labelCharacterSpaceInPixels);
                        this.DrawString(g, labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat, labelCase, labelCharacterSpaceInPixels);

                        break;
                    case LabelEffect.Block:
                        for (int i = effectSettings.Depth; i >= 0; i--)
                        {
                            //g.DrawString(labelText, FontLabel, brush, xyCoordinates.X - i, xyCoordinates.Y + i, _StringFormat);
                            this.DrawString(g, labelText, FontLabel, brush, new PointF(xyCoordinates.X - i, xyCoordinates.Y + i), _StringFormat, labelCase, labelCharacterSpaceInPixels);
                        }

                        //g.DrawString(labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat);
                        this.DrawString(g, labelText, FontLabel, orginalBrush, xyCoordinates, _StringFormat, labelCase, labelCharacterSpaceInPixels);

                        break;
                    case LabelEffect.Gradient:

                        //Find the Size required to draw the Sample Text.
                        SizeF textSize = g.MeasureString(labelText, FontLabel);


                        //Create a Diagonal Gradient LinearGradientBrush. 
                        RectangleF gradientRectangle = new RectangleF(new PointF(0, 0), textSize);
                        LinearGBrush = new LinearGradientBrush(gradientRectangle, effectSettings.EffectColor, orginalBrush.Color, LinearGradientMode.ForwardDiagonal);

                        //g.DrawString(labelText, FontLabel, LinearGBrush, xyCoordinates, _StringFormat);
                        this.DrawString(g, labelText, FontLabel, LinearGBrush, xyCoordinates, _StringFormat, labelCase, labelCharacterSpaceInPixels);

                        break;
                    case LabelEffect.Reflect:
                        //Find the Size required to draw the Sample Text.
                        textSize = g.MeasureString(labelText, FontLabel);

                        // Because we will be scaling, and scaling effects the ENTIRE
                        //   graphics object, not just the text, we need to reposition
                        //   the Origin of the Graphics object (0,0) to the (xLocation,
                        //   yLocation) point. If we don't, when we attempt to flip 
                        //   the text with a scaling transform, it will merely draw the
                        //   reflected text at (xLocation, -yLocation) which is outside
                        //   the viewable area.
                        g.TranslateTransform(xyCoordinates.X, xyCoordinates.Y);

                        // Reflecting around the Origin still poses problems. The
                        //   origin represents the upper left corner of the rectangle.
                        //   This means the reflection will occur at the TOP of the 
                        //   original drawing. This is not how people are used to 
                        //   seing reflected text. Thus, we need to determine where to
                        //   draw the text. This can be done only when we have calculated
                        //   the height required by the Drawing.
                        // This is not as simple as it may seem. The Height returned 
                        //   from the MeasureString method includes some extra spacing 
                        //   for descenders and whitespace. We want ONLY the height from
                        //   the BASELINE (which is the line which all caps sit). Any
                        //   characters with descenders drop below the baseline. To 
                        //   calculate the height above the baseline, use the 
                        //   GetCellAscent method. Since GetCellAscent returns a Design
                        //   Metric value it must be converted to pixels, and scaled for
                        //   the font size.
                        // Note: this looks best with characters that can be reflected
                        //   over the baseline nicely -- like caps. Characters with descenders
                        //   look odd. To fix that uncomment the two lines below, which
                        //   then reflect across the lowest descender height.

                        int lineAscent = 0;
                        int lineSpacing = 0;
                        float lineHeight = 0;
                        float textHeight = 0;

                        lineAscent = FontLabel.FontFamily.GetCellAscent(FontLabel.Style);
                        lineSpacing = FontLabel.FontFamily.GetLineSpacing(FontLabel.Style);
                        lineHeight = FontLabel.GetHeight(g);
                        textHeight = lineHeight * lineAscent / lineSpacing;

                        //' Uncomment these lines to reflect over lowest portion
                        //'   of the characters.
                        //Dim lineDescent As Integer ' used for reflecting descending characters
                        //lineDescent = myFont.FontFamily.GetCellDescent(myFont.Style)
                        //textHeight = lineHeight * (lineAscent + lineDescent) / lineSpacing


                        // Draw the reflected one first. The only reason to draw the
                        //   Reflected one first is to demonstrate the use of the
                        //   GraphicsState object. 
                        // A GraphicsState object maintains the state of the Graphics
                        //   object as it currently stands. You can then scale, resize and
                        //   otherwise transform the Graphics object. You can 
                        //   immediately go back to a previous state using the Restore
                        //   method of the Graphics object.
                        // Had we drawn the main one first, we would not have needed the 
                        //   Restore method or the GraphicsState object.

                        // First Save the graphics state
                        GraphicsState myState = g.Save();

                        // To draw the reflection, use the ScaleTransform with a negative
                        //   value. Using -1 will reflect the Text with no distortion.
                        // Remember to account for the fact that the origin has been reset.
                        g.ScaleTransform(1, -1f);

                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;

                        // Only reflecting in the Y direction
                        if (labelCharacterSpaceInPixels > 0)
                        {
                            this.DrawString(g, labelText, FontLabel, brush, new PointF(0, -(textHeight + 1 - 2 - textHeight / 2)), _StringFormat, labelCase, labelCharacterSpaceInPixels);
                        }
                        else
                        {
                            g.DrawString(labelText, FontLabel, brush, 0, -(textHeight + 1), sf);
                        }

                        // Reset the graphics state to before the transform
                        g.Restore(myState);

                        // Draw the main text
                        if (labelCharacterSpaceInPixels > 0)
                        {
                            this.DrawString(g, labelText, FontLabel, orginalBrush, new PointF(0, -(textHeight - 2 - textHeight / 2)), _StringFormat, labelCase, labelCharacterSpaceInPixels);
                        }
                        else
                        {
                            g.DrawString(labelText, FontLabel, orginalBrush, 0, -textHeight, sf);
                        }

                        g.TranslateTransform(-xyCoordinates.X, -xyCoordinates.Y);

                        sf.Dispose();
                        break;
                    default:
                        break;
                }
            }
        }

        public string GetLabel(Layer _Layer, ref Shape _Shape)
        {
            Theme _Theme = null;
            CustomLabel _CustomLabel = null;
            Legend _Legend = null;
            return GetLabel(_Layer, ref _Shape, ref _Theme, _CustomLabel, ref _Legend);
        }

        public string GetLabel(Layer _Layer, ref Shape _Shape, ref Theme _Theme)
        {
            CustomLabel _CustomLabel = null;
            Legend _Legend = null;
            return GetLabel(_Layer, ref _Shape, ref _Theme, _CustomLabel, ref _Legend);
        }

        public string GetLabel(Layer _Layer, ref Shape _Shape, ref Theme _Theme, CustomLabel _CustomLabel)
        {
            Legend _Legend = null;
            return GetLabel(_Layer, ref _Shape, ref _Theme, _CustomLabel, ref _Legend);
        }

        public string GetLabel(Layer _Layer, ref Shape _Shape, ref Theme _Theme, CustomLabel _CustomLabel, ref Legend _Legend)
        {
            int i;
            int j;
            string Label = "";
            //*** BugFix 06 Feb 2006 No label field selected
            string Delimiter = " ";
            int IndentSpace = 3;
            char[] Fields;
            bool Indented;

            if ((_Theme == null))
            {
                //*** Custom and Feature Layers
                if (_CustomLabel == null)
                {
                    if (_Layer.LabelMultirow == true)
                        Delimiter = "\r\n";  //initially it was: ControlChars.CrLf (line feed)
                    Fields = _Layer.LabelField.Replace(",", "").ToCharArray();
                }
                else
                {
                    if (_CustomLabel.MultiRow == true)
                        Delimiter = "\r\n";  //initially it was: ControlChars.CrLf (line feed)
                    Fields = _CustomLabel.LabelField.Replace(",", "").ToCharArray();
                }

                if (Array.IndexOf(Fields, "0"[0]) > -1)
                    Label = _Shape.AreaId + Delimiter;
                if (Array.IndexOf(Fields, char.Parse("1")) > -1)
                {
                    if (_Layer.LabelMultirow == true)
                    {
                        if (Label == "")
                        {
                            Label += _Shape.AreaName;
                        }
                        else
                        {
                            if (_Layer.LabelIndented == true)
                                Label += new string(' ', IndentSpace);
                            Label += _Shape.AreaName;
                        }
                    }
                    else
                    {
                        Label += _Shape.AreaName;
                    }
                }
            }
            else
            {
                //*** Base Layers
                if (_CustomLabel == null)
                {
                    if (_Legend == null)
                    {
                        if (_Layer.LabelMultirow == true)
                            Delimiter = "\r\n";  //initially it was: ControlChars.CrLf (line feed)
                        Fields = _Layer.LabelField.Replace(",", "").ToCharArray();
                        Indented = _Layer.LabelIndented;
                    }
                    else
                    {
                        if (_Legend.LabelMultiRow == true)
                            Delimiter = "\r\n";  //initially it was: ControlChars.CrLf (line feed)
                        Fields = _Legend.LabelField.Replace(",", "").ToCharArray();
                        Indented = _Legend.LabelIndented;
                    }
                }
                else
                {
                    if (_CustomLabel.MultiRow == true)
                        Delimiter = "\r\n";  //initially it was: ControlChars.CrLf (line feed)
                    Fields = _CustomLabel.LabelField.Replace(",", "").ToCharArray();
                    Indented = _CustomLabel.Indent;
                }
                if (_Theme.AreaIndexes.ContainsKey(_Shape.AreaId))
                {
                    j = 0;
                    for (i = 0; i <= Fields.Length - 1; i++)
                    {
                        switch (Fields[i])
                        {
                            case '0':
                                Label += _Shape.AreaId + Delimiter;
                                j = j + 1;
                                break;
                            case '1':
                                if (Indented == true)
                                    Label += new string(' ', j * IndentSpace);

                                Label += ((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).AreaName + Delimiter;
                                j = j + 1;
                                break;
                            case '2':
                                if (Indented == true)
                                    Label += new string(' ', j * IndentSpace);

                                //Label += ((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).DataValue + Delimiter;
                                //Textual dataValue is also displayed on Map Label, so property AreaInfo.DisplayInfo is used.
                                Label += ((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).DisplayInfo + Delimiter;
                                j = j + 1;
                                break;
                            case '3':
                                if (Indented == true)
                                    Label += new string(' ', j * IndentSpace);

                                Label += _Theme.UnitName + Delimiter;
                                j = j + 1;
                                break;
                            case '4':
                                if (Indented == true)
                                    Label += new string(' ', j * IndentSpace);

                                //Label += _Theme.SubgroupName(0) & Delimiter
                                //*** BugFix 21 Sep 2006 Subgroup label error
                                Label += ((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Subgroup + Delimiter;
                                j = j + 1;
                                break;
                            case '5':
                                if (Indented == true)
                                    Label += new string(' ', j * IndentSpace);

                                Label += ((AreaInfo)_Theme.AreaIndexes[_Shape.AreaId]).Time + Delimiter;
                                break;
                        }
                    }
                    Label = Label.Trim(Delimiter.ToCharArray());
                }
                else
                {
                    if (this._ShowLabelWhereDataExists)
                    {
                        //- If label is not required for Areas NOT having Data,
                        // then set blank label.
                        Label = string.Empty;
                    }
                    else
                    {
                        Hashtable AreaNames = _Layer.AreaNames;
                        string AreaName;
                        if (Array.IndexOf(Fields, "0"[0]) > -1)
                            Label = _Shape.AreaId + Delimiter;
                        if (Array.IndexOf(Fields, char.Parse("1")) > -1)
                        {
                            if (AreaNames.ContainsKey(_Shape.AreaId))
                            {
                                //Get language specific AreaName from Database if it exists
                                AreaName = (string)AreaNames[_Shape.AreaId];
                            }
                            else
                            {
                                //Get Shapefile Area Name
                                AreaName = _Shape.AreaName;
                            }
                            if (_Layer.LabelMultirow == true)
                            {
                                if (Label == "")
                                {
                                    Label += AreaName;
                                }
                                else
                                {
                                    if (_Layer.LabelIndented == true)
                                        Label += new string(' ', IndentSpace);
                                    Label += AreaName;
                                }
                            }
                            else
                            {
                                Label += AreaName;
                            }
                        }
                    }
                }
            }
            return Label;
        }

        #endregion

        #region " Nudging Functions"

        public RectangleF GetLabelRect(Graphics g, int x, int y, ref string LayerId, ref string AreaId, ref string Caption, ref Font Font, ref Color ForeColor)
        {
            RectangleF RetVal = new RectangleF();
            Matrix mTransMatrix = GetTransMatrix();
            RectangleF CurExt = GetCurrentExtent();
            string Rect = "";
            string FontName = "Arial";
            int FontSize = 8;
            int iFontStyle = 0;
            int iForeColor = 0;
            DrawLabel(ref g, mTransMatrix, CurExt, true, x, y, ref LayerId, ref AreaId, ref Caption, ref Rect, ref FontName, ref FontSize, ref iFontStyle, ref iForeColor);
            if (Rect != "")
            {
                Font = new Font(FontName, FontSize, (FontStyle)iFontStyle);
                ForeColor = Color.FromArgb(iForeColor);
                string[] sRect = Rect.Split(',');
                RetVal.X = float.Parse(sRect[0]);
                RetVal.Y = float.Parse(sRect[1]);
                RetVal.Width = float.Parse(sRect[2]);
                RetVal.Height = float.Parse(sRect[3]);
            }
            return RetVal;
        }

        public RectangleF GetChartBound(Graphics g, int x, int y, ref string ThemeId, ref string LayerId, ref string AreaId, ref GraphicsPath gPath)
        {
            RectangleF RetVal = new RectangleF();
            Matrix mTransMatrix = GetTransMatrix();
            RectangleF CurExt = GetCurrentExtent();

            foreach (Theme _Theme in Themes)
            {
                if (_Theme.Visible == true)
                {
                    if (_Theme.Type == ThemeType.Chart)
                    {
                        DrawChart(ref g, _Theme, mTransMatrix, CurExt, true, x, y, ref LayerId, ref AreaId, ref gPath);

                        //*** Chart Theme
                        if ((gPath != null))
                        {
                            ThemeId = _Theme.ID;
                            RetVal = gPath.GetBounds();
                            break;
                        }
                    }
                }
            }

            return RetVal;
        }
        #endregion

        #region " Selection"

        public void SetSelection(string LayerId, string AreaId, bool bAdd)
        {
            if (LayerId == "" | AreaId == "")
                return;

            int i;
            Layer _Layer = this.Layers[LayerId];
            string[] _AreaId = AreaId.Split(',');
            {
                for (i = 0; i <= _AreaId.Length - 1; i++)
                {
                    if (bAdd)
                    {
                        if (!_Layer.SelectedArea.Contains(_AreaId[i]))
                        {
                            _Layer.SelectedArea.Add(_AreaId[i]);
                        }
                    }
                    else
                    {
                        if (_Layer.SelectedArea.Contains(_AreaId[i]))
                        {
                            _Layer.SelectedArea.Remove(_AreaId[i]);
                        }
                    }
                }
            }
        }

        public bool SetSelection(string LayerId, string AreaId)
        {
            if (LayerId == "" | AreaId == "")
                return false;

            int i;
            Layer _Layer = this.Layers[LayerId];
            bool RetVal = false;
            {
                if (_Layer.LayerType == ShapeType.PolygonBuffer)
                {
                    //*** Buffer Type layer polygons willl act as a single unit rather than individual polygons
                    if (_Layer.SelectedArea.Count == 0)
                    {
                        IDictionaryEnumerator dicEnumerator = _Layer.Records.GetEnumerator();
                        Shape _Shape;
                        while (dicEnumerator.MoveNext())
                        {
                            _Shape = (Shape)dicEnumerator.Value;
                            _Layer.SelectedArea.Add(_Shape.AreaId);
                        }
                        RetVal = true;
                        return RetVal;
                        //*** Bugfix Feb 2006 buffer item selection as a group
                    }
                    else
                    {
                        _Layer.SelectedArea.Clear();
                        RetVal = false;
                        return RetVal;
                    }
                }

                else
                {
                    string[] _AreaId = AreaId.Split(',');
                    for (i = 0; i <= _AreaId.Length - 1; i++)
                    {
                        if (_Layer.SelectedArea.Contains(_AreaId[i]))
                        {
                            _Layer.SelectedArea.Remove(_AreaId[i]);
                            RetVal = false;
                            break;
                        }
                        else
                        {
                            _Layer.SelectedArea.Add(_AreaId[i]);
                            RetVal = true;
                            break;
                        }
                    }
                }
            }
            return RetVal;

        }

        public bool SetSelection(int x, int y)
        {
            string LayerId = "";
            return SetSelection(x, y, LayerId);
        }

        public bool SetSelection(int x, int y, string LayerId)
        {
            bool RetVal = false;
            string[] Layer_Area;
            Layer_Area = GetArea(x, y, LayerId);
            if (!(Layer_Area[0] == "" | Layer_Area[1] == ""))
            {
                RetVal = SetSelection(Layer_Area[0], Layer_Area[1]);
            }
            return RetVal;
        }

        public void ClearSelection()
        {
            foreach (Layer Lyr in this.Layers)
            {
                Lyr.SelectedArea.Clear();
            }
        }

        public string[] GetArea(int x, int y)
        {
            string LayerId = "";
            return GetArea(x, y, LayerId);
        }

        public string[] GetArea(int x, int y, string LayerId)
        {
            string[] RetVal = new string[3];
            RetVal[0] = "";
            RetVal[1] = "";
            RetVal[2] = "";

            int i;
            int j;
            Layer _Layer;
            Shape _Shape;
            GraphicsPath gp = new GraphicsPath();
            IDictionaryEnumerator dicEnumerator;
            Matrix TransMatrix = new Matrix(m_TransMatrix[0], m_TransMatrix[1], m_TransMatrix[2], m_TransMatrix[3], m_TransMatrix[4], m_TransMatrix[5]);

            bool bLayerVisible;
            Theme _Theme = m_Themes.GetActiveTheme();
            if (LayerId == "")
            {
                for (j = this.Layers.Count - 1; j >= 0; j += -1)
                {
                    _Layer = this.Layers[j];

                    switch (_Layer.LayerType)
                    {
                        case ShapeType.PointCustom:
                        case ShapeType.PolyLineCustom:
                        case ShapeType.PolygonCustom:
                        case ShapeType.PolygonBuffer:
                            bLayerVisible = _Layer.Visible;
                            break;
                        default:
                            if ((_Theme == null))
                            {
                                bLayerVisible = _Layer.Visible;
                            }
                            else
                            {
                                bLayerVisible = (bool)_Theme.LayerVisibility[_Layer.ID];
                            }

                            break;
                    }

                    if (bLayerVisible == true)
                    {
                        dicEnumerator = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID).GetEnumerator();

                        switch (_Layer.LayerType)
                        {
                            case ShapeType.Polygon:
                            case ShapeType.PolygonFeature:
                            case ShapeType.PolygonCustom:
                            case ShapeType.PolygonBuffer:
                                while (dicEnumerator.MoveNext())
                                {
                                    //Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    gp.Reset();
                                    for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                                    {
                                        gp.AddPolygon((PointF[])_Shape.Parts[i]);
                                    }
                                    gp.Transform(TransMatrix);
                                    if (gp.IsVisible(x, y))
                                    {
                                        RetVal[0] = _Layer.ID;
                                        RetVal[1] = _Shape.AreaId;
                                        if (_Layer.AreaNames.ContainsKey(_Shape.AreaId))
                                        {
                                            //Get language specific AreaName from Database if it exists
                                            RetVal[2] = _Layer.AreaNames[_Shape.AreaId].ToString();
                                        }
                                        else
                                        {
                                            //Get Shapefile Area Name
                                            RetVal[2] = _Shape.AreaName;
                                        }
                                        return (RetVal);
                                    }
                                }

                                break;
                            case ShapeType.PolyLine:
                            case ShapeType.PolyLineFeature:
                            case ShapeType.PolyLineCustom:
                                break;
                            //OPT
                            case ShapeType.Point:
                            case ShapeType.PointFeature:
                            case ShapeType.PointCustom:
                                PointF[] Pt = new PointF[1];
                                while (dicEnumerator.MoveNext())
                                {
                                    //Traverse Shapes
                                    _Shape = (Shape)dicEnumerator.Value;
                                    Pt[0] = (PointF)_Shape.Parts[0];
                                    TransMatrix.TransformPoints(Pt);
                                    gp.Reset();
                                    gp.AddEllipse(Pt[0].X - 3, Pt[0].Y - 3, 6, 6);
                                    if (gp.IsVisible(x, y))
                                    {
                                        RetVal[0] = _Layer.ID;
                                        RetVal[1] = _Shape.AreaId;
                                        if (_Layer.AreaNames.ContainsKey(_Shape.AreaId))
                                        {
                                            //Get language specific AreaName from Database if it exists
                                            RetVal[2] = (string)_Layer.AreaNames[_Shape.AreaId];
                                        }
                                        else
                                        {
                                            //Get Shapefile Area Name
                                            RetVal[2] = _Shape.AreaName;
                                        }
                                        return (RetVal);
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            else
            {
                _Layer = this.Layers[LayerId];
                dicEnumerator = _Layer.GetRecords(_Layer.LayerPath + "\\" + _Layer.ID).GetEnumerator();
                switch (_Layer.LayerType)
                {
                    case ShapeType.Polygon:
                    case ShapeType.PolygonFeature:
                    case ShapeType.PolygonCustom:
                    case ShapeType.PolygonBuffer:
                        while (dicEnumerator.MoveNext())
                        {
                            //Traverse Shapes
                            _Shape = (Shape)dicEnumerator.Value;
                            gp.Reset();
                            for (i = 0; i <= _Shape.Parts.Count - 1; i++)
                            {
                                gp.AddPolygon((PointF[])_Shape.Parts[i]);
                            }
                            gp.Transform(TransMatrix);
                            if (gp.IsVisible(x, y))
                            {
                                RetVal[0] = _Layer.ID;
                                RetVal[1] = _Shape.AreaId;
                                RetVal[2] = _Shape.AreaName;
                                return (RetVal);
                            }
                        }

                        break;
                    case ShapeType.PolyLine:
                    case ShapeType.PolyLineFeature:
                    case ShapeType.PolyLineCustom:
                        break;
                    //OPT
                    case ShapeType.Point:
                    case ShapeType.PointFeature:
                    case ShapeType.PointCustom:
                        PointF[] Pt = new PointF[1];
                        while (dicEnumerator.MoveNext())
                        {
                            //Traverse Shapes
                            _Shape = (Shape)dicEnumerator.Value;
                            Pt[0] = (PointF)_Shape.Parts[0];
                            TransMatrix.TransformPoints(Pt);
                            gp.Reset();
                            gp.AddEllipse(Pt[0].X - 3, Pt[0].Y - 3, 6, 6);
                            if (gp.IsVisible(x, y))
                            {
                                RetVal[0] = _Layer.ID;
                                RetVal[1] = _Shape.AreaId;
                                RetVal[2] = _Shape.AreaName;
                                return (RetVal);
                            }
                        }

                        break;
                }
            }

            return RetVal;
        }


        #endregion

        #region " Image Functions"

        public void GetMapImage(string p_Path, string p_FileName)
        {
            string p_FileExt = "png";
            bool DefaultView = true;
            GetMapImage(p_Path, p_FileName, p_FileExt, DefaultView);
        }

        public void GetMapImage(string p_Path, string p_FileName, string p_FileExt)
        {
            bool DefaultView = true;
            GetMapImage(p_Path, p_FileName, p_FileExt, DefaultView);
        }

        public void GetMapImage(string p_Path, string p_FileName, string p_FileExt, bool DefaultView)
        {
            //*** Preserve Original Settings In case some inset is being shown currently
            int SelIndex = Insets.SelIndex;
            RectangleF CurExtent = m_CurrentExtent;

            if (SelIndex > -1 & DefaultView == true)
            {
                Insets.SelIndex = -1;
                SetFullExtent();
            }

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap((int)m_Width, (int)m_Height);

            Graphics g = Graphics.FromImage(bmp);

            switch (p_FileExt.ToLower())
            {
                case "emf":
                    IntPtr hRefDC = g.GetHdc();
                    //Dim m As Imaging.Metafile = New Imaging.Metafile(p_Path & "\" & p_FileName & "." & p_FileExt, hRefDC, New Rectangle(0, 0, m_Width, m_Height), MetafileFrameUnit.Pixel)
                    Metafile m = new Metafile(p_Path + "\\" + p_FileName + "." + p_FileExt, hRefDC, new Rectangle(0, 0, (int)m_Width, (int)m_Height), MetafileFrameUnit.Pixel, EmfType.EmfPlusDual); //MetafileFrameUnit.Point
                    g.ReleaseHdc(hRefDC);
                    Graphics gMeta = Graphics.FromImage(m);
                    DrawMap("", gMeta);
                    m.Dispose();
                    gMeta.Dispose();
                    break;
                case "png":
                    DrawMap("", g);
                    bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                    break;
                case "jpg":
                    Color TempColor = this.CanvasColor;    //Storing Canvas Color in Tempory variable
                    this.CanvasColor = Color.White;         //Specifying White Canvas Color as Background color of JPEG image                    
                    DrawMap("", g);
                    this.CanvasColor = TempColor;           //Storing back original Canvas Color.

                    //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                    Map.SaveAsHightQualityJPEG(bmp, p_Path + "\\" + p_FileName + ".jpg");
                    break;
                case "bmp":
                    DrawMap("", g);
                    bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                    break;
                case "gif":
                    DrawMap("", g);
                    bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                    break;
                case "tiff":
                    DrawMap("", g);
                    bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                    break;
                case "ico":
                    DrawMap("", g);
                    bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                    break;
            }
            bmp.Dispose();
            bmp = null;
            g.Dispose();

            if (SelIndex > -1 & DefaultView == true)
            {
                Insets.SelIndex = SelIndex;
                m_CurrentExtent = CurExtent;
            }
        }

        public Image GetMapOverviewImage(int imageWidth, int imageHeight)
        {
            Image RetVal = null;

            //-- Preserve Current Extent
            RectangleF CurrExtent = this.m_CurrentExtent;
            Dictionary<string, bool> LayerLabelVisibilty = new Dictionary<string, bool>();

            foreach (Layer Lyr in this.m_Layers)
            {
                if (LayerLabelVisibilty.ContainsKey(Lyr.ID) == false)
                {
                    LayerLabelVisibilty.Add(Lyr.ID, Lyr.LabelVisible);

                    Lyr.LabelVisible = false;
                }
            }

            MemoryStream tempStream = new MemoryStream();
            Bitmap _BitMap = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(_BitMap);
            this.m_Width = imageWidth;
            this.m_Height = imageHeight;

            this.SetFullExtent();

            Themes ThemesCol = this.m_Themes;
            this.m_Themes = new Themes();       //-- Clear Themes collection, in order to draw only Base Layers
            this.DrawMap("", g);
            this.m_Themes = ThemesCol;          //-- reset themes.

            _BitMap.Save(tempStream, ImageFormat.Png);
            RetVal = Image.FromStream(tempStream);
            _BitMap.Dispose();
            g.Dispose();

            //-- Reset back current Extent
            this.m_CurrentExtent = CurrExtent;

            foreach (Layer Lyr in this.m_Layers)
            {
                if (LayerLabelVisibilty.ContainsKey(Lyr.ID))
                {
                    Lyr.LabelVisible = LayerLabelVisibilty[Lyr.ID];
                }
            }

            return RetVal;
        }

        public static Image GetCompositeMapImage(string xlsPresentationFilePath, string tempLocation, string fileExtension)
        {
            return Map.GetCompositeMapImage(xlsPresentationFilePath, tempLocation, fileExtension, true, true);
        }

        public static Image GetCompositeMapImage(string xlsPresentationFilePath, string tempLocation, string fileExtension, bool includeLegend, bool includeTitle)
        {
            return Map.GetCompositeMapImage(xlsPresentationFilePath, tempLocation, fileExtension, includeLegend, includeTitle, -1, -1);
        }


        public static Image GetCompositeMapImage(string xlsPresentationFilePath, string tempLocation, string fileExtension, bool includeLegend, bool includeTitle, int customWidth, int customHeight)
        {
            Image RetVal = null;

            string TempImageFile = string.Empty;
            string MaskFolderPath = string.Empty;
            Map _Map = null;

            try
            {
                if (File.Exists(xlsPresentationFilePath))
                {
                    if (Directory.Exists(tempLocation))
                    {
                        //-- Load Map object from xls presentation
                        string xmlSerializedText = Presentation.GetSerializedPresentationText(xlsPresentationFilePath, Presentation.PresentationType.Map);

                        _Map = Map.LoadFromSerializeText(xmlSerializedText);

                        if (_Map != null)
                        {
                            //-- Initialize Map
                            _Map.DIConnection = new DIConnection(_Map.UserPreference.Database.SelectedConnectionDetail);

                            _Map.DIQueries = new DIQueries(_Map.UserPreference.Database.SelectedDatasetPrefix, _Map.UserPreference.Database.DatabaseLanguage);

                            MaskFolderPath = Path.Combine(_Map.UserPreference.General.AdaptationPath, @"Bin\Templates\Metadata\Mask");

                            _Map._DIDataView = new DIDataView(_Map._UserPreference, _Map.DIConnection, _Map.DIQueries, MaskFolderPath, "");

                            // DIDataView.GetAllDataByUserSelection() generates and returns PresentationData required in Map.
                            _Map._PresentationData = _Map._DIDataView.GetAllDataByUserSelection();

                            //-- Initialize Map object keeping Lengend ranges preserved
                            _Map.Initialize(tempLocation, _Map.SpatialMapFolder, _Map.CFLPath, true);

                            //-- Now generate composote Image
                            TempImageFile = Path.Combine(tempLocation, DateTime.Now.Ticks.ToString());

                            if (customWidth > 0 & customHeight > 0)
                            {
                                _Map.Height = customHeight;
                                _Map.Width = customWidth;
                            }

                            _Map.GetCompositeMapImage(tempLocation, Path.GetFileName(TempImageFile), fileExtension, tempLocation, true, string.Empty, includeLegend, includeTitle);


                            //-- Get image object from temp Image file generated.
                            TempImageFile += "." + fileExtension;
                            if (File.Exists(TempImageFile))
                            {
                                RetVal = Image.FromFile(TempImageFile);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (_Map != null)
                {
                    if (_Map.DIConnection != null)
                    {
                        _Map.DIConnection.Dispose();
                    }

                }
            }

            return RetVal;
        }

        public void GetCompositeMapImage(string p_Path, string p_FileName, string p_FileExt, string tempFolder, bool DefaultView)
        {
            this.GetCompositeMapImage(p_Path, p_FileName, p_FileExt, tempFolder, DefaultView, string.Empty); //-- Pass timePeriod Label as blank
        }

        /// <summary>
        /// Gets Map Composite Image (By default, Legends & Title will be included)
        /// </summary>
        public void GetCompositeMapImage(string p_Path, string p_FileName, string p_FileExt, string tempFolder, bool DefaultView, string timePeriodLabel)
        {
            this.GetCompositeMapImage(p_Path, p_FileName, p_FileExt, tempFolder, DefaultView, timePeriodLabel, true, true);
        }

        public void GetCompositeMapImage(string p_Path, string p_FileName, string p_FileExt, string tempFolder, bool DefaultView, string timePeriodLabel, bool includeLegend, bool includeTitle)
        {
            //--Process:
            //-- 1. Extract Images for Map, Legends & Titles seperatly in temp location.
            //-- 2. Add those images together in single g object.
            //-- 3. Save final graphics object.

            string FilePostfix = DateTime.Now.Ticks.ToString();
            string TitleImgFilePath = "Title" + FilePostfix;
            string LegendImgFilePath = "Legend" + FilePostfix;
            string MapImgPath = "Map" + FilePostfix;

            //-- Preserve Map's Original Size
            float MapOriginalHeight = this.m_Height;
            float MapOriginalWidth = this.m_Width;

            Image MapImg = null;
            Image TitleImg = null;
            Image LegendImg = null;
            Size LegendImgSize = new Size();
            Size TitleImgSize = new Size();
            SizeF timePeriodSize = new SizeF();
            Font timePeriodFnt = null;
            Graphics g = null;
            Metafile mFile = null;
            System.Drawing.Bitmap bmp = null;

            try
            {
                //-- Get Legend Image in temp location
                if (includeLegend)
                {
                    this.m_Themes[0].GetLegendImage(tempFolder, LegendImgFilePath, p_FileExt, false, this.TemplateStyle.Legends.ShowCaption, this.TemplateStyle.Legends.ShowRange, this.TemplateStyle.Legends.ShowCount, this.TemplateStyle.Legends.ShowMissingLegend);
                    LegendImg = Image.FromFile(Path.Combine(tempFolder, LegendImgFilePath + "." + p_FileExt));
                    LegendImgSize = LegendImg.Size;
                }

                //-- Get title image in Temp location
                if (includeTitle)
                {
                    this.GetTitleImage(tempFolder, TitleImgFilePath, p_FileExt);
                    TitleImg = Image.FromFile(Path.Combine(tempFolder, TitleImgFilePath + "." + p_FileExt));
                    TitleImgSize = TitleImg.Size;
                }

                bmp = new System.Drawing.Bitmap((int)MapOriginalWidth, (int)MapOriginalHeight);

                //- get new graphics object 
                g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                if (string.IsNullOrEmpty(timePeriodLabel) == false)
                {
                    timePeriodFnt = new Font("Arial", 24, FontStyle.Bold);
                    timePeriodSize = g.MeasureString(timePeriodLabel, timePeriodFnt);
                }

                if (p_FileExt.ToLower() == "emf")
                {
                    IntPtr hRefDC = g.GetHdc();
                    mFile = new Metafile(p_Path + "\\" + p_FileName + "." + p_FileExt, hRefDC, new Rectangle(0, 0, (int)m_Width + LegendImgSize.Width + 10, (int)m_Height + TitleImgSize.Height + 5), MetafileFrameUnit.Pixel, EmfType.EmfPlusDual); //MetafileFrameUnit.Point

                    g.Dispose();
                    g = Graphics.FromImage(mFile);
                    //g.ReleaseHdc(hRefDC);
                }

                //-- Decrease Map width & height to accomodate Legend and title images with in total size.
                this.m_Height -= TitleImgSize.Height + 5 + timePeriodSize.Height;
                this.m_Width -= LegendImgSize.Width + 5;

                //-- Get Map Image in temp location
                this.GetMapImage(tempFolder, MapImgPath, p_FileExt, DefaultView);

                //-- Load Map Image object
                MapImg = Image.FromFile(Path.Combine(tempFolder, MapImgPath + "." + p_FileExt));

                //-- Draw Map image as new graphics, giving spaces for Legend and title 
                g.DrawImage(MapImg, LegendImgSize.Width + 2, TitleImgSize.Height + 2);

                //-- Add Title image into top of graphics object.
                if (includeTitle)
                {
                    g.DrawImage(TitleImg, 2, 3);
                }

                //-- Add Legend image in Left side of Map 
                if (includeLegend)
                {
                    g.DrawImage(LegendImg, 2, MapOriginalHeight - LegendImgSize.Height - 5 - timePeriodSize.Height);
                }

                //-- Draw TimePeriod in bottom right corner of Map Image.
                if (string.IsNullOrEmpty(timePeriodLabel) == false)
                {
                    g.DrawString(timePeriodLabel, timePeriodFnt, Brushes.Gray, bmp.Width - timePeriodSize.Width - 3, bmp.Height - timePeriodSize.Height - 3);
                }

                switch (p_FileExt.ToLower())
                {
                    case "emf":
                        mFile.Dispose();
                        //gMeta.Dispose();
                        break;
                    case "png":
                        bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                        break;
                    case "jpg":
                        //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                        Map.SaveAsHightQualityJPEG(bmp, p_Path + "\\" + p_FileName + ".jpg");
                        break;
                    case "bmp":
                        bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                        break;
                    case "gif":
                        bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                        break;
                    case "tiff":
                        bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                        break;
                    case "ico":
                        bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                        break;
                }

            }
            catch
            {
            }
            finally
            {
                //-- Restore Map's original Size
                this.m_Height = MapOriginalHeight;
                this.m_Width = MapOriginalWidth;

                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                if (g != null)
                {
                    g.Dispose();
                }
                if (LegendImg != null)
                {
                    LegendImg.Dispose();
                }
                if (MapImg != null)
                {
                    MapImg.Dispose();
                }
                if (TitleImg != null)
                {
                    TitleImg.Dispose();
                }
            }
        }

        public void GetCompositeLegendImage(string outputFilePath, string fileName, string p_FileExt, string tempFolder)
        {
            this.GetCompositeLegendImage(outputFilePath, fileName, p_FileExt, tempFolder, LegendsSequenceOrder.SingleColumn, CompositeLegendImageOrder.Horizontal);
        }

        /// <summary>
        /// Get composite legend image
        /// </summary>
        /// <param name="outputFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="p_FileExt"></param>
        /// <param name="tempFolder"></param>
        /// <param name="legendsSequenceOrder"></param>
        /// <param name="compositeLegendImageOrder"></param>
        public void GetCompositeLegendImage(string outputPath, string fileName, string p_FileExt, string tempFolder, LegendsSequenceOrder legendsSequenceOrder, CompositeLegendImageOrder compositeLegendImageOrder)
        {
            string FilePostfix = DateTime.Now.Ticks.ToString();
            string LegendImgFile = string.Empty;
            //string CompositeLegendImage = "Legend" + FilePostfix;
            Image[] LegendImages = null;

            Size LegendImgSize = new Size();
            Graphics g = null;
            Metafile mFile = null;
            System.Drawing.Bitmap bmp = null;
            try
            {
                if (this.m_Themes.Count > 0)
                {
                    LegendImages = new Image[this.m_Themes.Count];

                    //- Get Legend Images in Image Array
                    for (int i = 0; i < this.m_Themes.Count; i++)
                    {
                        Theme theme = this.m_Themes[i];
                        if (theme.Visible)
                        {

                            LegendImgFile = "Legend_" + i + FilePostfix;

                            //set legend Maximum width according to map width
                            theme.LegendMaxWidth = this.m_Width - 2;
                            theme.GetLegendImage(tempFolder, LegendImgFile, p_FileExt, false, this._TemplateStyle.Legends.ShowCaption, this._TemplateStyle.Legends.ShowRange, this._TemplateStyle.Legends.ShowCount, this._TemplateStyle.Legends.ShowMissingLegend, legendsSequenceOrder);

                            LegendImages[i] = Image.FromFile(Path.Combine(tempFolder, LegendImgFile + "." + p_FileExt));
                        }
                    }

                    //- Start adding Legend Images in a single composite Legend image.
                    foreach (object img in LegendImages)
                    {
                        //- Calculate composite Legend Image Size
                        if (img != null)
                        {
                            if (compositeLegendImageOrder == CompositeLegendImageOrder.Horizontal)
                            {
                                //- if all legend Image are horizontally positioned
                                LegendImgSize.Width += ((Image)img).Width;
                                LegendImgSize.Height = Math.Max(LegendImgSize.Height, ((Image)img).Height);
                            }
                            else
                            {
                                //- if all legend Images are vertically positioned
                                LegendImgSize.Width = Math.Max(LegendImgSize.Width, ((Image)img).Width);
                                LegendImgSize.Height += ((Image)img).Height;
                            }
                        }
                    }

                    //- Get new graphics object 
                    bmp = new System.Drawing.Bitmap(LegendImgSize.Width, LegendImgSize.Height);
                    g = Graphics.FromImage(bmp);
                    g.Clear(Color.White);

                    if (p_FileExt.ToLower() == "emf")
                    {
                        IntPtr hRefDC = g.GetHdc();
                        //mFile = new Metafile(outputPath + "\\" + fileName + "." + p_FileExt, hRefDC, new Rectangle(0, 0, LegendImgSize.Width, LegendImgSize.Height, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual)); //MetafileFrameUnit.Point

                        g.Dispose();
                        g = Graphics.FromImage(mFile);
                    }

                    int leftPos = 0;
                    int topPos = 0;
                    foreach (object img in LegendImages)
                    {
                        if (img != null)
                        {
                            g.DrawImage(((Image)img), leftPos, topPos);

                            if (compositeLegendImageOrder == CompositeLegendImageOrder.Horizontal)
                            {
                                //- if all legend Image are horizontally positioned
                                leftPos += ((Image)img).Width;
                            }
                            else
                            {
                                //- if all legend Images are vertically positioned
                                topPos += ((Image)img).Height;
                            }
                        }
                    }

                    switch (p_FileExt.ToLower())
                    {
                        case "emf":
                            mFile.Dispose();
                            break;
                        case "png":
                            bmp.Save(outputPath + "\\" + fileName + ".png", ImageFormat.Png);
                            break;
                        case "jpg":
                            //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                            Map.SaveAsHightQualityJPEG(bmp, outputPath + "\\" + fileName + ".jpg");
                            break;
                        case "bmp":
                            bmp.Save(outputPath + "\\" + fileName + ".bmp", ImageFormat.Bmp);
                            break;
                        case "gif":
                            bmp.Save(outputPath + "\\" + fileName + ".gif", ImageFormat.Gif);
                            break;
                        case "tiff":
                            bmp.Save(outputPath + "\\" + fileName + ".tiff", ImageFormat.Tiff);
                            break;
                        case "ico":
                            bmp.Save(outputPath + "\\" + fileName + ".ico", ImageFormat.Icon);
                            break;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
                if (g != null)
                {
                    g.Dispose();
                }
                foreach (Image img in LegendImages)
                {
                    if (img != null)
                    {
                        img.Dispose();
                    }
                }
            }

        }


        public void GetInsetImage(string p_Path, string p_FileExt, int p_Width, int p_Height, int p_Index)
        {
            string p_FileName = "";
            GetInsetImage(p_Path, p_FileExt, p_Width, p_Height, p_Index, p_FileName);
        }

        public void GetInsetImage(string p_Path, string p_FileExt, int p_Width, int p_Height, int p_Index, string p_FileName)
        {
            int i;
            string ImgFileName = string.Empty;
            //*** Preserve Original Settings
            int SelIndex = Insets.SelIndex;
            int StartIndex = 0;
            int EndIndex = Insets.Count - 1;
            Color CurCanvasColor = this.CanvasColor;
            float CurWidth = m_Width;
            float CurHeight = m_Height;
            RectangleF CurExtent = m_CurrentExtent;
            bool bNorthSymbol = m_NorthSymbol;
            m_NorthSymbol = false;
            //Supress north symbol on Inset Images

            MemoryStream ms = null;

            m_Width = p_Width;
            m_Height = p_Height;
            this.CanvasColor = Color.White;
            if (p_Index > -1)
            {
                StartIndex = p_Index;
                EndIndex = p_Index;
            }
            for (i = StartIndex; i <= EndIndex; i++)
            {
                Insets.SelIndex = i;
                SetFullExtent();
                if (p_FileName == "")
                {
                    p_FileName = "Inset" + i;
                }
                else
                {
                    p_FileName += i;
                }

                ImgFileName = p_Path + "\\" + p_FileName + "." + p_FileExt;

                if (File.Exists(ImgFileName))
                {
                    try
                    {
                        File.Delete(ImgFileName);
                    }
                    catch
                    {
                    }
                }

                Bitmap bmp = new Bitmap(p_Width, p_Height);
                Graphics g = Graphics.FromImage(bmp);
                ms = new MemoryStream(1000);
                switch (p_FileExt.ToLower())
                {
                    case "emf":
                        IntPtr hRefDC = g.GetHdc();
                        Metafile m = new Metafile(p_Path + "\\" + p_FileName + "." + p_FileExt, hRefDC, new Rectangle(0, 0, p_Width, p_Height), MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
                        g.ReleaseHdc(hRefDC);
                        Graphics gMeta = Graphics.FromImage(m);
                        DrawMap("", gMeta);
                        m.Dispose();
                        gMeta.Dispose();

                        DrawMap("", g);
                        bmp.Save(ms, ImageFormat.Png);
                        break;
                    case "png":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                        bmp.Save(ms, ImageFormat.Png);
                        break;
                    case "jpg":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                        bmp.Save(ms, ImageFormat.Jpeg);
                        break;
                    case "bmp":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                        bmp.Save(ms, ImageFormat.Bmp);
                        break;
                    case "gif":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                        bmp.Save(ms, ImageFormat.Gif);
                        break;
                    case "tiff":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                        bmp.Save(ms, ImageFormat.Tiff);
                        break;
                    case "ico":
                        DrawMap("", g);
                        bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                        bmp.Save(ms, ImageFormat.Icon);
                        break;
                }
                bmp.Dispose();
                bmp = null;
                g.Dispose();

                //-- Get inset Image in Inset.InsetImage property.
                if (File.Exists(ImgFileName))
                {
                    try
                    {
                        this.m_Insets[i].InsetImage = Image.FromStream(ms);
                        //ms.Close();
                    }
                    catch (Exception ex)
                    {

                    }

                }
                ms.Close();
            }

            m_CurrentExtent = CurExtent;
            Insets.SelIndex = SelIndex;
            m_Width = CurWidth;
            m_Height = CurHeight;
            m_NorthSymbol = bNorthSymbol;
            this.CanvasColor = CurCanvasColor;
        }

        /// <summary>
        /// Get Map Image stream in Png format (default)
        /// </summary>
        public Stream GetMapStream()
        {
            return this.GetMapStream(ImageFormat.Png);
        }

        public Stream GetMapStream(ImageFormat imageFormat)
        {
            System.Drawing.Bitmap _BitMap = null;
            if (this.m_Width == 0 || this.m_Height == 0)
            {
                _BitMap = new System.Drawing.Bitmap(100, 80, PixelFormat.Format24bppRgb);
            }
            else
            {
                _BitMap = new System.Drawing.Bitmap((int)m_Width, (int)m_Height, PixelFormat.Format24bppRgb);
            }
            Graphics g = Graphics.FromImage(_BitMap);
            g.Clear(Color.White);
            DrawMap("", g);
            Stream oStream = new MemoryStream();
            _BitMap.Save(oStream, imageFormat);
            _BitMap.Dispose();
            _BitMap = null;
            g.Dispose();
            System.GC.WaitForPendingFinalizers();
            return oStream;
        }

        public Size GetTitleImage(string p_Path, string p_FileName)
        {
            string p_FileExt = "emf";
            return GetTitleImage(p_Path, p_FileName, p_FileExt);
        }

        public Size GetTitleImage(string p_Path, string p_FileName, string p_FileExt)
        {
            Size TitleSize;
            Size SubTitleSize;
            Size ImageSize = new Size(0, 0);
            Font font = null;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //*** Get maximum width of Title / Subtitle
            font = new Font(this._TemplateStyle.TitleSetting.FontTemplate.FontName, this._TemplateStyle.TitleSetting.FontTemplate.FontSize, this._TemplateStyle.TitleSetting.FontTemplate.FontStyle);
            TitleSize = g.MeasureString(m_Title, font).ToSize();      ////m_TitleFont

            font = new Font(this._TemplateStyle.SubTitleSetting.FontTemplate.FontName, this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize, this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle);
            SubTitleSize = g.MeasureString(m_Subtitle, font).ToSize();    //m_SubtitleFont

            ImageSize.Width = Math.Max(TitleSize.Width, SubTitleSize.Width) + 20;
            ImageSize.Height = TitleSize.Height + SubTitleSize.Height + 20;

            bmp = new System.Drawing.Bitmap(ImageSize.Width, ImageSize.Height);
            g = Graphics.FromImage(bmp);
            switch (p_FileExt.ToLower())
            {
                case "emf":
                    IntPtr hRefDC = g.GetHdc();
                    Metafile m = null;
                    try
                    {
                        m = new Metafile(p_Path + "\\" + p_FileName + ".emf", hRefDC, new Rectangle(0, 0, ImageSize.Width, ImageSize.Height), MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);

                    }
                    catch (Exception ex)
                    {

                        Console.Write(ex.Message);
                    }
                    g.ReleaseHdc(hRefDC);
                    Graphics gMeta = Graphics.FromImage(m);
                    DrawTitleText(gMeta, TitleSize.Height);

                    m.Dispose();
                    gMeta.Dispose();
                    break;
                case "png":
                    DrawTitleText(g, TitleSize.Height);
                    bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                    break;
                case "jpg":
                    g.Clear(Color.White);
                    DrawTitleText(g, TitleSize.Height);
                    //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                    Map.SaveAsHightQualityJPEG(bmp, p_Path + "\\" + p_FileName + ".jpg");
                    break;
                case "bmp":
                    g.Clear(Color.White);
                    DrawTitleText(g, TitleSize.Height);
                    bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                    break;
                case "gif":
                    g.Clear(Color.White);
                    DrawTitleText(g, TitleSize.Height);
                    bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                    break;
                case "tiff":
                    DrawTitleText(g, TitleSize.Height);
                    bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                    break;
                case "ico":
                    DrawTitleText(g, TitleSize.Height);
                    bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                    break;
            }

            bmp.Dispose();
            bmp = null;
            g.Dispose();

            return ImageSize;
        }

        private void DrawTitleText(Graphics g, int TitleHeight)
        {
            SolidBrush brTitle = new SolidBrush(Color.Black);
            Font MapFont = null;

            MapFont = new Font(this._TemplateStyle.TitleSetting.FontTemplate.FontName, this._TemplateStyle.TitleSetting.FontTemplate.FontSize, this._TemplateStyle.TitleSetting.FontTemplate.FontStyle);
            g.DrawString(m_Title, MapFont, brTitle, 10, 10);

            MapFont = new Font(this._TemplateStyle.SubTitleSetting.FontTemplate.FontName, this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize, this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle);
            g.DrawString(m_Subtitle, MapFont, brTitle, 10, 10 + TitleHeight);
            brTitle.Dispose();
        }

        private void DrawText(Graphics g, string textToDraw, Font TxtFont, PointF textPosition)
        {
            SolidBrush brTitle = new SolidBrush(Color.Black);
            g.DrawString(textToDraw, TxtFont, brTitle, textPosition.X, textPosition.Y);
            //g.DrawString(m_Subtitle, m_SubtitleFont, brTitle, 10, 10 + TxtHeight);
            brTitle.Dispose();
        }

        public Size GetDisclaimerImage(string p_Path, string p_FileName)
        {
            string p_FileExt = "emf";
            return GetDisclaimerImage(p_Path, p_FileName, p_FileExt);
        }

        public Size GetDisclaimerImage(string p_Path, string p_FileName, string p_FileExt)
        {
            return GetDisclaimerImage(p_Path, p_FileName, p_FileExt, -1);
        }

        public Size GetDisclaimerImage(string p_Path, string p_FileName, string p_FileExt, int imageMaxWidthInPixel)
        {
            Size DisclaimerSize;
            string DisclaimerText = this.m_Disclaimer;
            Size ImageSize = new Size(0, 0);
            SolidBrush brDisclaimer = new SolidBrush(Color.Black);
            Font DisclaimerFont = new Font(this._TemplateStyle.DisclaimerFont.FontTemplate.FontName, this._TemplateStyle.DisclaimerFont.FontTemplate.FontSize, this._TemplateStyle.DisclaimerFont.FontTemplate.FontStyle);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //*** Get maximum width of DisclaimerText
            DisclaimerSize = g.MeasureString(DisclaimerText, DisclaimerFont).ToSize();

            if (imageMaxWidthInPixel > 5)
            {
                if (DisclaimerSize.Width > imageMaxWidthInPixel)
                {
                    //- Wrap Disclaimer text in multiline 
                    // so that total width of disclaimer image limits to Maximum Image width specified.
                    string[] DisclaimerTextBlocks = DisclaimerText.Split(' ');
                    int TotalSize = 0;
                    for (int k = 0; k < DisclaimerTextBlocks.Length; k++)
                    {
                        string DisclaimerBlock = DisclaimerTextBlocks[k];

                        TotalSize += g.MeasureString(DisclaimerBlock, DisclaimerFont).ToSize().Width;

                        //- If total disclaimer size exceeds limit, 
                        // then add line break to get wrapping.
                        if (TotalSize >= imageMaxWidthInPixel && k > 0)
                        {
                            DisclaimerTextBlocks[k - 1] = DisclaimerTextBlocks[k - 1] + Environment.NewLine + DisclaimerTextBlocks[k];
                            DisclaimerTextBlocks[k] = string.Empty;
                            TotalSize = 0;
                        }
                    }

                    //- get wapped disclaimer Text from new array
                    DisclaimerText = string.Join(" ", DisclaimerTextBlocks);

                    //- Get again maximum width of DisclaimerText image
                    DisclaimerSize = g.MeasureString(DisclaimerText, DisclaimerFont).ToSize();
                }
            }

            ImageSize.Width = DisclaimerSize.Width + 20;
            ImageSize.Height = DisclaimerSize.Height + 20;

            bmp = new System.Drawing.Bitmap(ImageSize.Width, ImageSize.Height);
            g = Graphics.FromImage(bmp);
            switch (p_FileExt.ToLower())
            {
                case "emf":
                    IntPtr hRefDC = g.GetHdc();
                    Metafile m = new Metafile(p_Path + "\\" + p_FileName + ".emf", hRefDC, EmfType.EmfPlusOnly);
                    g.ReleaseHdc(hRefDC);
                    Graphics gMeta = Graphics.FromImage(m);
                    gMeta.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    m.Dispose();
                    gMeta.Dispose();
                    break;
                case "png":
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".png", ImageFormat.Png);
                    break;
                case "jpg":
                    g.Clear(Color.White);
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    //bmp.Save(p_Path + "\\" + p_FileName + ".jpg", ImageFormat.Jpeg);
                    Map.SaveAsHightQualityJPEG(bmp, p_Path + "\\" + p_FileName + ".jpg");
                    break;
                case "bmp":
                    g.Clear(Color.White);
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".bmp", ImageFormat.Bmp);
                    break;
                case "gif":
                    g.Clear(Color.White);
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".gif", ImageFormat.Gif);
                    break;
                case "tiff":
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".tiff", ImageFormat.Tiff);
                    break;
                case "ico":
                    g.DrawString(DisclaimerText, DisclaimerFont, brDisclaimer, 10, 10);
                    bmp.Save(p_Path + "\\" + p_FileName + ".ico", ImageFormat.Icon);
                    break;
            }

            bmp.Dispose();
            bmp = null;
            g.Dispose();

            brDisclaimer.Dispose();
            return ImageSize;
        }

        public static void SaveAsHightQualityJPEG(Bitmap bmp, string fullFileNamePath)
        {
            ImageCodecInfo myImageCodecInfo = null;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            if (bmp != null && string.IsNullOrEmpty(fullFileNamePath) == false)
            {
                // Get an ImageCodecInfo object that represents the JPEG codec.
                ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
                for (int j = 0; j < encoders.Length; ++j)
                {
                    if (encoders[j].MimeType == "image/jpeg")
                    {
                        myImageCodecInfo = encoders[j];
                        break;
                    }
                }

                // Create an Encoder object based on the GUID
                // for the Quality parameter category.
                myEncoder = System.Drawing.Imaging.Encoder.Quality;

                // Create an EncoderParameters object.
                // An EncoderParameters object has an array of EncoderParameter
                // objects. In this case, there is only one
                // EncoderParameter object in the array.
                myEncoderParameters = new EncoderParameters(1);

                // Save the bitmap as a JPEG file with quality level 25.
                myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                //--Save Bmp as JPEG
                //bmp.SetResolution(120F, 120F);

                bmp.Save(fullFileNamePath, myImageCodecInfo, myEncoderParameters);
                //bmp.SetResolution(96F, 96F);
                myEncoderParameters.Dispose();
            }
        }

        #endregion

        #region " Excel Presentation Generation "

        /// <summary>
        /// Generates the .XLS presentation file in specified folder on the basis of current Map object settings.
        /// </summary>
        /// <param name="presentationOutputFolderPath">Folder Path where .xls presentation is created.</param>
        /// <returns>Returns the full file path of .xls presentation generated.</returns>	
        public string GeneratePresentation(string presentationOutputFolderPath)
        {
            return GeneratePresentation(presentationOutputFolderPath, -1, -1);
        }

        /// <summary>
        /// Generates the .XLS presentation file in specified folder on the basis of current Map object settings.
        /// </summary>
        /// <param name="presentationOutputFolderPath">Folder Path where .xls presentation is created.</param>
        /// <param name="maxHeightInPixelToFitAll">(optional) maximun width (in pixel) for presentation composite content to fit in a single page. (-1 default)</param>
        /// <param name="maxWidthInPixelToFitAll">(optional) maximun height (in pixel) for presentation composite content to fit in a single page. (-1 default)</param>
        /// <returns>Returns the full file path of .xls presentation generated.</returns>	
        public string GeneratePresentation(string presentationOutputFolderPath, int maxWidthInPixelToFitAll, int maxHeightInPixelToFitAll)
        {
            string RetVal = string.Empty;
            if (!(string.IsNullOrEmpty(presentationOutputFolderPath)))
            {
                float MapOriginalWidth = this.m_Width;
                float MapOriginalHeight = this.m_Height;

                // Getting the xls sheet names in desired language.
                string MapSheetName = DILanguage.GetLanguageString("MAP");
                string MapDataSheetName = DILanguage.GetLanguageString("DATA");
                string SourceSheetName = DILanguage.GetLanguageString("SOURCECOMMON");

                // Set path for presentation and png images
                string FileSuffix = DateTime.Now.Ticks.ToString();

                // Set Image file extension(.png or .emf) on he basis of UserPreference property.
                string ImageExtention = string.Empty;
                if (this.UserPreference.General.ShowExcel)
                {
                    //If ShowExcel is True, then User can see presentation in MS Excel object
                    ImageExtention = "emf";
                }
                else
                {
                    // Png format is supported in SpreadsheetGear (.Emf NOT supported)
                    //If ShowExcel is False, then presentation will be opened in Spreadsheet Gear, so Use Png format.
                    ImageExtention = "png";
                }

                string PresentationPath = presentationOutputFolderPath + @"\Map" + FileSuffix + ".xls";
                string TitleImgPath = Path.Combine(presentationOutputFolderPath, "Title" + FileSuffix + "." + ImageExtention);  //"Title"
                string MapImgPath = Path.Combine(presentationOutputFolderPath, "Map" + FileSuffix + "." + ImageExtention);
                string DisclaimerImgPath = Path.Combine(presentationOutputFolderPath, "Disclaimer" + FileSuffix + "." + ImageExtention);
                string InsetFilePrefix = string.Empty;
                DIExcel ExcelApp = new DIExcel();
                Image img;

                //- Calculate pixelToPoint factor as SpreadsheetGear uses point unit instead of Pixel unit for measuring Length and width
                Graphics graphics = Graphics.FromImage(new System.Drawing.Bitmap(1, 1));
                double PixcelToPointFactor = 72 / graphics.DpiX;  //1 inch  = 72 points

                Size TitleSize = new Size();
                Size LegendSize = new Size();

                int MaxLegendHeight = 20;
                int MaxLegendWidth = 30;

                int RowIndex = 0;
                int SheetIndex;
                double CellHeight = ExcelApp.GetCellHeight(0, 0, 0);     // default cell height from default sheet.
                double CellWidth = ExcelApp.GetColumnWidth(0, 0, 0, 0, 1);  // Default cell width from default sheet.
                int SingleCellWidthInPixel = (int)(ExcelApp.GetWorksheet(0).Cells[0, 0].Width / PixcelToPointFactor);

                if (maxWidthInPixelToFitAll > 0 && maxHeightInPixelToFitAll > 0)
                {
                    //- Offset page Margins (default left margin -> 0.7" & right margin -> 1.4") and (default Top & bottom margin -> 0.75")
                    maxWidthInPixelToFitAll = maxWidthInPixelToFitAll - (int)(2.1 * graphics.DpiX);
                    maxHeightInPixelToFitAll = maxHeightInPixelToFitAll - (int)(1.5 * graphics.DpiY);

                    //- Calculating maximum Height & width of presentation's composite contents in Points
                    //maxWidthInPixelToFitAll = (int)(maxWidthInPixelToFitAll * PixcelToPointFactor);
                    //maxHeightInPixelToFitAll = (int)(maxHeightInPixelToFitAll * PixcelToPointFactor);

                    // Offset margin of first column width. 

                    maxWidthInPixelToFitAll -= SingleCellWidthInPixel;

                    //- Set final Map width so that Map can be accomodated within maximum width specified.
                    this.m_Width = (float)maxWidthInPixelToFitAll - 2;

                    //- offset width of Inset images (if present)/
                    //- generally inset image is 15% of total Map and starts after a gap of 1/2 column width.
                    if (this.Insets.Count > 0)
                    {
                        float InsetImageWidth = MapOriginalWidth; //- Default
                        if (this.Insets[0].InsetImage != null)
                        {
                            InsetImageWidth = (float)(this.Insets[0].InsetImage.Width);
                        }

                        this.m_Width -= (InsetImageWidth * 0.1F) + (float)(SingleCellWidthInPixel);
                    }

                    //- Adjusting Map height in proportion with CurrentExtent's Width to height ratio.
                    this.m_Height = this.m_Width * Math.Min(4 / 3, (this.m_CurrentExtent.Height / this.m_CurrentExtent.Width)) + 3;
                }

                try
                {
                    if (File.Exists(PresentationPath))
                    {
                        File.Delete(PresentationPath);
                    }
                    ExcelApp.SaveAs(PresentationPath);
                    ExcelApp.RenameWorkSheet(0, MapSheetName);   //language handling Rename first(default) sheet
                    ExcelApp.Save();


                    //*********************************  WorkSheet 1 - Map Images**************************************************
                    this.ClearSelection();          //-- Clear any Layer selection (if any)

                    //Generate the first sheet which is the Map Image for MRD data.

                    //*** generate title image in temp folder

                    TitleSize = this.GetTitleImage(Path.GetDirectoryName(TitleImgPath), Path.GetFileNameWithoutExtension(TitleImgPath), ImageExtention);

                    //*** Generate Themes Legend Image in temp folder
                    foreach (Theme _ThemeTemp in this.Themes)
                    {
                        if (_ThemeTemp.Visible == true)
                        {
                            //Generate the Legend image, and calculating image maximum height & width.
                            LegendSize = _ThemeTemp.GetLegendImage(presentationOutputFolderPath, _ThemeTemp.ID + FileSuffix, ImageExtention, this.PointShapeIncluded, this.TemplateStyle.Legends.ShowCaption, this.TemplateStyle.Legends.ShowRange, this.TemplateStyle.Legends.ShowCount, this.TemplateStyle.Legends.ShowMissingLegend);
                            MaxLegendWidth = Math.Max(MaxLegendWidth, (int)(LegendSize.Width * PixcelToPointFactor));
                            MaxLegendHeight = Math.Max(MaxLegendHeight, (int)(LegendSize.Height * PixcelToPointFactor));
                        }
                    }

                    //*** generate discalimer (common) image in temp folder
                    this.GetDisclaimerImage(Path.GetDirectoryName(DisclaimerImgPath), Path.GetFileNameWithoutExtension(DisclaimerImgPath), ImageExtention, maxWidthInPixelToFitAll);


                    int ImageRowPosition = 0;   //Holds the row position for Image to be pasted.

                    //---Insert Sheet for Map of most recent data.
                    SheetIndex = ExcelApp.GetSheetIndex(MapSheetName);
                    ExcelApp.ActivateSheet(SheetIndex);
                    ExcelApp.ShowWorkSheetGridLine(SheetIndex, false);

                    if (this._showTimePeriods || this._showAreaLevels)
                    {
                        //*** Set map rendering info for most recent time period for all level
                        for (int iThemeIndex = 0; iThemeIndex <= this.m_Themes.Count - 1; iThemeIndex++)
                        {
                            this.UpdateRenderingInfo(-1, -1, iThemeIndex);
                        }
                        this.GetMapImage(Path.GetDirectoryName(MapImgPath), Path.GetFileNameWithoutExtension(MapImgPath), ImageExtention, true);
                    }
                    else
                    {
                        this.GetMapImage(Path.GetDirectoryName(MapImgPath), Path.GetFileNameWithoutExtension(MapImgPath), ImageExtention, true);
                    }
                    ExcelApp.Save();


                    // Set Title Image
                    ImageRowPosition = 0;
                    if (this.Title != "")
                    {
                        img = Image.FromFile(TitleImgPath);
                        // Converting Image size from Pixel to Points while pasting.
                        ExcelApp.PasteImage(SheetIndex, TitleImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor);
                        ImageRowPosition += (int)(img.Height * PixcelToPointFactor) / (int)CellHeight + 1;     //Increment row position by rows covered by Image height.
                    }

                    // Set Map Image
                    ExcelApp.PasteImage(SheetIndex, MapImgPath, 1, ImageRowPosition, this.m_Width * PixcelToPointFactor, this.m_Height * PixcelToPointFactor);


                    // Set Insets Image
                    int InsetCtr;
                    CellWidth = 70;     //TODO: Remove hardcoding calculate column width right.
                    int RowOffset = ImageRowPosition;
                    int ColOffset = (int)(this.Width / CellWidth) + 2;

                    InsetFilePrefix = "Inset" + FileSuffix;

                    for (InsetCtr = 0; InsetCtr <= this.Insets.Count - 1; InsetCtr++)
                    {
                        Inset TempInset = this.Insets[InsetCtr];
                        string InsetImgFilePath = Path.Combine(presentationOutputFolderPath, InsetFilePrefix + InsetCtr + "." + ImageExtention);

                        if (TempInset.InsetImage != null)
                        {
                            TempInset.InsetImage.Save(InsetImgFilePath);
                        }
                        else
                        {
                            //Extract new "png" image of inset in temp folder.
                            this.GetInsetImage(presentationOutputFolderPath, ImageExtention, (int)this.Width, (int)this.Height, InsetCtr, InsetFilePrefix);
                        }

                        if (TempInset.Visible)
                        {
                            //*** Extract InsetName png.
                            TempInset.GetInsetName(presentationOutputFolderPath, "InsetName" + TempInset.Name + FileSuffix, ImageExtention);
                            img = Image.FromFile(Path.Combine(presentationOutputFolderPath, "InsetName" + TempInset.Name + FileSuffix + "." + ImageExtention));
                            ExcelApp.PasteImage(SheetIndex, Path.Combine(presentationOutputFolderPath, "InsetName" + TempInset.Name + FileSuffix + "." + ImageExtention), ColOffset, RowOffset, img.Width, img.Height);

                            //*** use previously generated inset images in temp folder
                            RowOffset += img.Height / (int)CellHeight;
                            img = Image.FromFile(InsetImgFilePath);
                            ExcelApp.PasteImage(SheetIndex, InsetImgFilePath, ColOffset, RowOffset, (img.Width * 0.09), (img.Height * 0.09));

                            RowOffset += (int)(img.Width * 0.09) / (int)CellHeight + 2;

                            img.Dispose();
                        }
                    }

                    //Set Theme Legend Images
                    ImageRowPosition += (int)((this.m_Height * PixcelToPointFactor) / (int)CellHeight) + 2;      //Increment row position by rows covered by Image height.
                    int p = 1;    //p is the column position of the Legend Images (x - axis).
                    foreach (Theme _ThemeTemp in this.Themes)
                    {
                        if (_ThemeTemp.Visible == true)
                        {
                            img = Image.FromFile(presentationOutputFolderPath + @"\" + _ThemeTemp.ID + FileSuffix + "." + ImageExtention);
                            ExcelApp.PasteImage(SheetIndex, presentationOutputFolderPath + @"\" + _ThemeTemp.ID + FileSuffix + "." + ImageExtention, p, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor); //(double)LegendSize.Width, (double)LegendSize.Height);
                            p += (int)(img.Width / CellWidth) + 2;                               // Increment column pos. by Legend's width + 2
                        }
                    }

                    // Set Sources               
                    ImageRowPosition += MaxLegendHeight / (int)CellHeight + 1;
                    RowIndex = ImageRowPosition + 1;
                    ExcelApp.SetCellValue(SheetIndex, RowIndex, 1, "Sources");
                    DataView _DVSources = this.GetUniqueSourceList();   //getting unique sources in DataView
                    for (int ctr = 0; ctr < _DVSources.Count; ctr++)
                    {
                        ExcelApp.SetCellValue(SheetIndex, RowIndex + ctr + 1, 1, _DVSources[ctr]["IC_Name"]);

                        //- Wrap Cell for source
                        int EndingColumnToMerge = maxWidthInPixelToFitAll / SingleCellWidthInPixel;
                        ExcelApp.MergeCells(SheetIndex, RowIndex + ctr + 1, 1, RowIndex + ctr + 1, EndingColumnToMerge);
                        ExcelApp.WrapText(SheetIndex, RowIndex + ctr + 1, 1, RowIndex + ctr + 1, EndingColumnToMerge, true);
                    }

                    // Set Disclaimer Image
                    ImageRowPosition += _DVSources.Count + 3;
                    img = Image.FromFile(DisclaimerImgPath);
                    ExcelApp.PasteImage(SheetIndex, DisclaimerImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)(img.Height * PixcelToPointFactor));


                    ExcelApp.Save();

                    //--Now, generate sheets for each timePeriod selected.
                    if (this._showTimePeriods)
                    {

                        DataView DVTimePeriods = this.GetTimePeriodsForThemes();
                        for (byte i = 0; i <= DVTimePeriods.Count - 1; i++)
                        {
                            //Restrict Excel Worksheet name to 31 characters
                            string sheetName = DVTimePeriods[i][Timeperiods.TimePeriod].ToString();
                            if (sheetName.Length > 31)
                            {
                                sheetName.Substring(0, 31);
                            }

                            //---Insert Sheet for Map of specified TimePeriod.
                            ExcelApp.InsertWorkSheet(sheetName);
                            SheetIndex = ExcelApp.GetSheetIndex(sheetName);
                            ExcelApp.ActivateSheet(SheetIndex);
                            ExcelApp.ShowWorkSheetGridLine(SheetIndex, false);

                            // Draw  Map for specifoed TimePeriod.
                            MapImgPath = Path.Combine(presentationOutputFolderPath, "Map" + FileSuffix + i.ToString() + "." + ImageExtention);

                            for (int iThemeIndex = 0; iThemeIndex <= this.m_Themes.Count - 1; iThemeIndex++)
                            {
                                this.UpdateRenderingInfo((int)(DVTimePeriods[i][Timeperiods.TimePeriodNId]), -1, iThemeIndex);
                            }
                            this.GetMapImage(Path.GetDirectoryName(MapImgPath), Path.GetFileNameWithoutExtension(MapImgPath), ImageExtention, true);

                            // Insert Title Image
                            ImageRowPosition = 0;
                            if (this.Title != "")
                            {
                                img = Image.FromFile(TitleImgPath);
                                ExcelApp.PasteImage(SheetIndex, TitleImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor);
                                ImageRowPosition += (int)(img.Height * PixcelToPointFactor) / (int)CellHeight + 1;     //Increment row position by rows covered by Image height.
                            }

                            // Insert Map Image
                            ExcelApp.PasteImage(SheetIndex, MapImgPath, 1, ImageRowPosition, this.m_Width * PixcelToPointFactor, this.m_Height * PixcelToPointFactor);

                            //Set Theme Legend Images
                            ImageRowPosition += (int)(this.m_Height * PixcelToPointFactor) / (int)CellHeight + 2;      //Increment row position by rows covered by Image height.
                            p = 1;    //p is the column position of the Legend Image (x - axis).
                            foreach (Theme _ThemeTemp in this.Themes)
                            {
                                if (_ThemeTemp.Visible == true)
                                {
                                    img = Image.FromFile(presentationOutputFolderPath + @"\" + _ThemeTemp.ID.ToString() + FileSuffix + "." + ImageExtention);
                                    ExcelApp.PasteImage(SheetIndex, presentationOutputFolderPath + @"\" + _ThemeTemp.ID.ToString() + FileSuffix + "." + ImageExtention, p, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor);
                                    p += (int)(img.Width / CellWidth) + 2;                               // Increment column pos. by Legend's width + 2
                                }
                            }

                            // Set Sources.
                            ImageRowPosition += MaxLegendHeight / (int)CellHeight + 1;
                            RowIndex = ImageRowPosition + 1;
                            ExcelApp.SetCellValue(SheetIndex, RowIndex, 1, "Sources");
                            for (int ctr = 0; ctr < _DVSources.Count; ctr++)
                            {
                                //*** Add Sources
                                ExcelApp.SetCellValue(SheetIndex, RowIndex + ctr + 1, 1, _DVSources[ctr]["IC_Name"]);
                            }

                            // Insert Disclaimer Image.
                            ImageRowPosition += _DVSources.Count + 3;
                            img = Image.FromFile(DisclaimerImgPath);
                            ExcelApp.PasteImage(SheetIndex, DisclaimerImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)(img.Height * PixcelToPointFactor));
                        }
                    }
                    else if (this.ShowAreaLevels)   //If only AreaLevel is selected.
                    {
                        DataView DVAreaLevels = this.GetAreaLevels();
                        for (byte i = 0; i <= DVAreaLevels.Count - 1; i++)
                        {
                            //---Insert Sheet for Map of specified TimePeriod.
                            ExcelApp.InsertWorkSheet(DVAreaLevels[i][Area_Level.AreaLevelName].ToString());
                            SheetIndex = ExcelApp.GetSheetIndex(DVAreaLevels[i][Area_Level.AreaLevelName].ToString());
                            ExcelApp.ActivateSheet(SheetIndex);
                            ExcelApp.ShowWorkSheetGridLine(SheetIndex, false);

                            // Draw  Map for specifoed TimePeriod.
                            MapImgPath = Path.Combine(presentationOutputFolderPath, "Map" + FileSuffix + i.ToString() + "." + ImageExtention);

                            for (int iThemeIndex = 0; iThemeIndex <= this.m_Themes.Count - 1; iThemeIndex++)
                            {
                                this.UpdateRenderingInfo(-1, (int)(DVAreaLevels[i][Area_Level.AreaLevel]), iThemeIndex);
                            }

                            this.GetMapImage(Path.GetDirectoryName(MapImgPath), Path.GetFileNameWithoutExtension(MapImgPath), ImageExtention, true);

                            // Insert Title Image
                            ImageRowPosition = 0;
                            if (this.Title != "")
                            {
                                img = Image.FromFile(TitleImgPath);
                                ExcelApp.PasteImage(SheetIndex, TitleImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor);
                                ImageRowPosition += (int)(img.Height * PixcelToPointFactor) / (int)CellHeight + 1;     //Increment row position by rows covered by Image height.
                            }

                            // Insert Map Image
                            ExcelApp.PasteImage(SheetIndex, MapImgPath, 1, ImageRowPosition, this.m_Width * PixcelToPointFactor, this.m_Height * PixcelToPointFactor);

                            //Set Theme Legend Images
                            ImageRowPosition += (int)(this.m_Height * PixcelToPointFactor) / (int)CellHeight + 2;      //Increment row position by rows covered by Image height.
                            p = 1;    //p is the column position of the Legend Image (x - axis).
                            foreach (Theme _ThemeTemp in this.Themes)
                            {
                                if (_ThemeTemp.Visible == true)
                                {
                                    img = Image.FromFile(presentationOutputFolderPath + @"\" + _ThemeTemp.ID.ToString() + FileSuffix + "." + ImageExtention);
                                    ExcelApp.PasteImage(SheetIndex, presentationOutputFolderPath + @"\" + _ThemeTemp.ID.ToString() + FileSuffix + "." + ImageExtention, p, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)img.Height * PixcelToPointFactor);
                                    p += (int)(img.Width / CellWidth) + 2;                               // Increment column pos. by Legend's width + 2
                                }
                            }

                            // Set Sources.
                            ImageRowPosition += MaxLegendHeight / (int)CellHeight + 1;
                            RowIndex = ImageRowPosition + 1;
                            ExcelApp.SetCellValue(SheetIndex, RowIndex, 1, SourceSheetName);
                            for (int ctr = 0; ctr < _DVSources.Count; ctr++)
                            {
                                //Adding sources from Dv into cell.
                                ExcelApp.SetCellValue(SheetIndex, RowIndex + ctr + 1, 1, _DVSources[ctr]["IC_Name"]);
                            }

                            // Insert Disclaimer Image.
                            ImageRowPosition += _DVSources.Count + 3;
                            img = Image.FromFile(DisclaimerImgPath);
                            ExcelApp.PasteImage(SheetIndex, DisclaimerImgPath, 1, ImageRowPosition, (double)img.Width * PixcelToPointFactor, (double)(img.Height * PixcelToPointFactor));
                        }

                        ExcelApp.Save();
                    }

                    //*********************************  WorkSheet - Map Data*********************************************
                    //TODO Use TablePresentation object logic
                    ExcelApp.InsertWorkSheet(MapDataSheetName);                 //insert DataSheet.
                    SheetIndex = ExcelApp.GetSheetIndex(MapDataSheetName);
                    ExcelApp.ActivateSheet(SheetIndex);
                    DataTable _DTDataSheet = GetDataSheetView(this.PresentationData);    //Getting Map Data or DataSheet.
                    string[,] arrDataView = new string[_DTDataSheet.Rows.Count, _DTDataSheet.Columns.Count - 1];    //Array Columns will be 1 less than dv, to ignore extra footNote column

                    //  --Adding Column names in desired language , only one time, at row 1.
                    ExcelApp.SetCellValue(SheetIndex, 1, 0, DILanguage.GetLanguageString("TIMEPERIOD"));            //Timeperiods.TimePeriod
                    ExcelApp.SetCellValue(SheetIndex, 1, 1, DILanguage.GetLanguageString("AREAID"));                //Area.AreaID
                    ExcelApp.SetCellValue(SheetIndex, 1, 2, DILanguage.GetLanguageString("AREANAME"));              //Area.AreaName
                    ExcelApp.SetCellValue(SheetIndex, 1, 3, DILanguage.GetLanguageString("INDICATOR"));             //Indicator.IndicatorName
                    ExcelApp.SetCellValue(SheetIndex, 1, 4, DILanguage.GetLanguageString("DATA"));                  //Data.DataValue
                    ExcelApp.SetCellValue(SheetIndex, 1, 5, DILanguage.GetLanguageString("UNIT"));                  //Unit.UnitName
                    ExcelApp.SetCellValue(SheetIndex, 1, 6, DILanguage.GetLanguageString("SUBGROUP"));              //SubgroupVals.SubgroupVal
                    ExcelApp.SetCellValue(SheetIndex, 1, 7, DILanguage.GetLanguageString("SOURCECOMMON"));           //IndicatorClassifications.ICName
                    int col = 8;  //Columnn number 8th
                    foreach (string MDColumn in this._MDIndicatorFields.Split(','))
                    {
                        if (MDColumn.Length > 0)
                        {
                            //Adding column name at "col" position.
                            ExcelApp.SetCellValue(SheetIndex, 1, col, this._DIDataView.MetadataIndicator.Columns[MDColumn].Caption);
                            col++;
                        }
                    }
                    foreach (string MDColumn in this._MDAreaFields.Split(','))
                    {
                        if (MDColumn.Length > 0)
                        {
                            //Adding column name at "col" position.
                            ExcelApp.SetCellValue(SheetIndex, 1, col, this._DIDataView.MetadataArea.Columns[MDColumn].Caption);
                            col++;
                        }
                    }
                    foreach (string MDColumn in this._MDSourceFields.Split(','))
                    {
                        if (MDColumn.Length > 0)
                        {
                            //Adding column name at "col" position.
                            ExcelApp.SetCellValue(SheetIndex, 1, col, this._DIDataView.MetadataSource.Columns[MDColumn].Caption);
                            col++;
                        }
                    }

                    // ---Columns added...

                    RowIndex = 2;          //Start displaying Map Data from Row - 2nd
                    foreach (DataRow DRowDTDataSheet in _DTDataSheet.Rows)
                    {
                        for (int Columns = 0; Columns < _DTDataSheet.Columns.Count; Columns++)
                        {
                            //Assign cellvalue with data in DataTable.
                            ExcelApp.SetCellValue(SheetIndex, RowIndex, Columns, DRowDTDataSheet[Columns].ToString());
                            if (_DTDataSheet.Columns[Columns].ColumnName == Data.DataValue)
                            {
                                //If Column id = "DataValue" then add footNotes As-Comment if present.
                                if (DRowDTDataSheet[FootNotes.FootNote].ToString() != "")
                                {
                                    ExcelApp.AddComment(SheetIndex, RowIndex, Columns, DRowDTDataSheet[FootNotes.FootNote].ToString(), true);
                                }
                            }
                        }
                        RowIndex++;
                    }
                    ExcelApp.AutoFitColumns(SheetIndex, 1, 0, _DTDataSheet.Rows.Count - 1, _DTDataSheet.Columns.Count - 1);   //Auto fitting columns width.

                    ExcelApp.Save();

                    //*********************************  WorkSheet - Sources**************************************************
                    ExcelApp.InsertWorkSheet(SourceSheetName);

                    SheetIndex = ExcelApp.GetSheetIndex(SourceSheetName);
                    ExcelApp.ActivateSheet(SheetIndex);
                    ExcelApp.SetCellValue(SheetIndex, 0, 0, SourceSheetName);      //Cell value = "Source" in desired language.
                    ExcelApp.SetCellValue(SheetIndex, 0, 2, DIConnection.ConnectionStringParameters.DbName);
                    for (byte i = 0; i <= _DVSources.Count - 1; i++)
                    {
                        //Adding sources from Dv into cell.
                        ExcelApp.SetCellValue(SheetIndex, i + 3, 0, _DVSources[i]["IC_Name"]);
                        ExcelApp.AutoFitColumn(SheetIndex, i);
                    }
                    ExcelApp.Save();

                    //*********************************  WorkSheet - Keyword**************************************************
                    ExcelApp.InsertWorkSheet(Presentation.KEYWORD_WORKSHEET_NAME);

                    ExcelApp.ActivateSheet(ExcelApp.GetSheetIndex(MapSheetName));   //Set first sheet as ACTIVE sheet..
                    ExcelApp.Save();

                    RetVal = PresentationPath;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                finally
                {
                    ExcelApp.Close();
                    if (graphics != null)
                    {
                        graphics.Dispose();
                    }

                    this.m_Width = MapOriginalWidth;
                    this.m_Height = MapOriginalHeight;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// It extracts Map data for xls Sheet(Map Data) from presentationData.
        /// </summary>
        /// <param name="dvPresentationData">DataView PresentationData on the basis of UserSelections.</param>
        /// <returns>Returns DataTable containing Map Data.</returns>
        private DataTable GetDataSheetView(DataView dvPresentationData)
        {
            DataTable RetVal = null;
            string[] MDIndicatorColumns = this._MDIndicatorFields.Split(',');
            string[] MDAreaColumns = this._MDAreaFields.Split(',');
            string[] MDSourceColumns = this._MDSourceFields.Split(',');

            DataTable DTDataSheet = new DataTable();
            //Adding columns in Datatable, picking names from classes in DAL.
            DTDataSheet.Columns.Add(Timeperiods.TimePeriod);
            DTDataSheet.Columns.Add(Area.AreaID);
            DTDataSheet.Columns.Add(Area.AreaName);
            DTDataSheet.Columns.Add(Indicator.IndicatorName);
            DTDataSheet.Columns.Add(Data.DataValue);
            DTDataSheet.Columns.Add(Unit.UnitName);
            DTDataSheet.Columns.Add(SubgroupVals.SubgroupVal);
            DTDataSheet.Columns.Add(IndicatorClassifications.ICName);

            foreach (string MDColumn in MDIndicatorColumns)
            {
                if (this._DIDataView.MetadataIndicator.Columns.Contains(MDColumn))
                {
                    DTDataSheet.Columns.Add(this._DIDataView.MetadataIndicator.Columns[MDColumn].Caption);
                }
            }
            foreach (string MDColumn in MDAreaColumns)
            {
                if (this._DIDataView.MetadataArea.Columns.Contains(MDColumn))
                {
                    DTDataSheet.Columns.Add(this._DIDataView.MetadataArea.Columns[MDColumn].Caption);
                }
            }
            foreach (string MDColumn in MDSourceColumns)
            {
                if (this._DIDataView.MetadataSource.Columns.Contains(MDColumn))
                {
                    DTDataSheet.Columns.Add(this._DIDataView.MetadataSource.Columns[MDColumn].Caption);
                }
            }

            DTDataSheet.Columns.Add(FootNotes.FootNote);

            DataRow RowDTDataSheet;
            DataRow[] MDDataRows;

            //Make a loop for each Record in the PresentationData
            foreach (DataRow DRVPresentationData in dvPresentationData.ToTable().Rows)
            {
                //-----------------Fill DataTable for DataSheet---------------
                RowDTDataSheet = DTDataSheet.NewRow();
                RowDTDataSheet[Timeperiods.TimePeriod] = DRVPresentationData[Timeperiods.TimePeriod];
                RowDTDataSheet[Area.AreaID] = DRVPresentationData[Area.AreaID];
                RowDTDataSheet[Area.AreaName] = DRVPresentationData[Area.AreaName];
                RowDTDataSheet[Indicator.IndicatorName] = DRVPresentationData[Indicator.IndicatorName];
                RowDTDataSheet[Data.DataValue] = DRVPresentationData[Data.DataValue];
                RowDTDataSheet[Unit.UnitName] = DRVPresentationData[Unit.UnitName];
                RowDTDataSheet[SubgroupVals.SubgroupVal] = DRVPresentationData[SubgroupVals.SubgroupVal];
                RowDTDataSheet[IndicatorClassifications.ICName] = DRVPresentationData[IndicatorClassifications.ICName];

                //If footnotes are checked, then add footnotes in dv.
                if (this.Footnote == true)
                {
                    if (DRVPresentationData[FootNotes.FootNoteNId].ToString() != "-1")
                    {
                        string str = DIQueries.Footnote.GetFootnote(FilterFieldType.NId, DRVPresentationData[FootNotes.FootNoteNId].ToString());
                        IDataReader dr = DIConnection.ExecuteReader(str);
                        while (dr.Read())
                        {
                            RowDTDataSheet[FootNotes.FootNote] = dr[FootNotes.FootNote];
                            break;
                        }

                        dr.Close();
                    }
                    else
                    {
                        RowDTDataSheet[FootNotes.FootNote] = "";
                    }
                }
                //Add corresponding MetaData Indicators in DataTable
                if (MDIndicatorColumns.Length > 0 && this._DIDataView.MetadataIndicator.Columns.Count > 0)
                {
                    MDDataRows = this._DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + DRVPresentationData[Indicator.IndicatorNId].ToString());
                    if (MDDataRows.Length > 0)
                    {
                        foreach (string MDColumn in MDIndicatorColumns)
                        {
                            RowDTDataSheet[this._DIDataView.MetadataIndicator.Columns[MDColumn].Caption] = MDDataRows[0][MDColumn].ToString();
                        }
                    }
                }

                //Add corresponding MetaData Area in DataTable
                if (MDAreaColumns.Length > 0 && this._DIDataView.MetadataArea.Columns.Count > 0)
                {
                    MDDataRows = this._DIDataView.MetadataArea.Select(Area.AreaNId + " = " + DRVPresentationData[Area.AreaNId].ToString());
                    if (MDDataRows.Length > 0)
                    {
                        foreach (string MDColumn in MDAreaColumns)
                        {
                            RowDTDataSheet[this._DIDataView.MetadataArea.Columns[MDColumn].Caption] = MDDataRows[0][MDColumn].ToString();
                        }
                    }
                }

                //Add corresponding MetaData Sources in DataTable
                if (MDSourceColumns.Length > 0 && this._DIDataView.MetadataSource.Columns.Count > 0)
                {
                    MDDataRows = this._DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + DRVPresentationData[IndicatorClassifications.ICNId].ToString());
                    if (MDDataRows.Length > 0)
                    {
                        foreach (string MDColumn in MDSourceColumns)
                        {
                            RowDTDataSheet[this._DIDataView.MetadataSource.Columns[MDColumn].Caption] = MDDataRows[0][MDColumn].ToString();
                        }
                    }
                }

                DTDataSheet.Rows.Add(RowDTDataSheet);
            }
            return DTDataSheet;

        }

        /// <summary>
        /// It replaces vertor Map image (.emf), present in xls presentation, with bitmap image like .png, .jpeg.
        /// </summary>
        /// <param name="mapPresentationFilePath"></param>
        public static bool ReplaceVectorMapImageWithBitmap(string mapPresentationFilePath, ImageFormat desiredBitmapFormat, string resultantOutputFilePath)
        {
            bool RetVal = false;
            SpreadsheetGear.IWorkbook workbook = null;
            SpreadsheetGear.IWorksheet worksheet = null;
            SpreadsheetGear.Shapes.IShapes ImagesShapes = null;
            SpreadsheetGear.Shapes.IShape OriginalMapShape = null;
            byte[] MapImageBytes = null;

            try
            {
                if (string.IsNullOrEmpty(mapPresentationFilePath) == false)
                {
                    if (File.Exists(mapPresentationFilePath))
                    {
                        //- Load xls.
                        workbook = SpreadsheetGear.Factory.GetWorkbook(mapPresentationFilePath);
                        worksheet = workbook.Worksheets[0];

                        //- Get all shapes present in WorkSheet 1
                        ImagesShapes = worksheet.Shapes;

                        if (ImagesShapes != null)
                        {
                            //- Load Map Image (Generally 2nd Shape in Sheet 1 represents Map image.)
                            foreach (SpreadsheetGear.Shapes.IShape imgShape in ImagesShapes)
                            {
                                if (imgShape.Type == SpreadsheetGear.Shapes.ShapeType.Picture && imgShape.Name == "Picture 2")
                                {
                                    OriginalMapShape = imgShape;

                                    //- Load Image into memory
                                    SpreadsheetGear.Drawing.Image MapImage = new SpreadsheetGear.Drawing.Image(imgShape);
                                    System.Drawing.Bitmap MapBitmap = MapImage.GetBitmap();
                                    MemoryStream ms = new MemoryStream();
                                    ms.Position = 0;

                                    //- Convert Map to png format 
                                    MapBitmap.Save(ms, desiredBitmapFormat);
                                    MapImageBytes = ms.ToArray();
                                    break;
                                }
                            }

                            //- Save and close Workbook.
                            if (MapImageBytes != null)
                            {
                                // overwrite on previous Map shape.
                                worksheet.Shapes.AddPicture(MapImageBytes, OriginalMapShape.Left, OriginalMapShape.Top, OriginalMapShape.Width, OriginalMapShape.Height);

                                //- Delete Previous Image
                                OriginalMapShape.Delete();

                                if (File.Exists(resultantOutputFilePath))
                                {
                                    try
                                    {
                                        File.Delete(resultantOutputFilePath);
                                    }
                                    catch
                                    { }
                                }

                                workbook.SaveAs(resultantOutputFilePath, SpreadsheetGear.FileFormat.XLS97);

                                RetVal = true;
                                workbook.Close();
                                workbook = null;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (workbook != null)
                {
                    workbook.Close();
                }
            }

            return RetVal;
        }

        # endregion


        #region " Buffer"
        public Layer CreateBufferLayer(Layer SrcLayer, float BufferRadius)
        {
            int i;
            Layer BufferLayer = new Layer();
            Shape SrcShape;
            IDictionaryEnumerator dicEnumerator = SrcLayer.GetRecords(SrcLayer.LayerPath + "\\" + SrcLayer.ID).GetEnumerator();
            switch (SrcLayer.LayerType)
            {
                case ShapeType.Point:
                case ShapeType.PointCustom:
                case ShapeType.PointFeature:
                    GraphicsPath TempGp = new GraphicsPath();
                    PointF Pt;
                    while (dicEnumerator.MoveNext())
                    {
                        Shape TempShp = new Shape();
                        SrcShape = (Shape)dicEnumerator.Value;
                        Pt = (PointF)SrcShape.Parts[0];
                        TempShp.AreaId = SrcShape.AreaId;
                        TempShp.AreaName = SrcShape.AreaName;
                        TempShp.Centroid = Pt;
                        PointF[] Pts = GetPointBuffer(Pt, BufferRadius);

                        TempGp.Reset();
                        TempGp.AddPolygon(Pts);
                        TempShp.Extent = TempGp.GetBounds();

                        TempShp.Parts.Add(Pts);
                        BufferLayer.Records.Add(TempShp.AreaId, TempShp);

                        if (BufferLayer.Extent.IsEmpty)
                        {
                            BufferLayer.Extent = TempShp.Extent;
                        }
                        else
                        {
                            BufferLayer.Extent = RectangleF.Union(BufferLayer.Extent, TempShp.Extent);
                        }

                        TempShp = null;
                    }

                    TempGp.Dispose();
                    break;

                case ShapeType.PolyLine:
                case ShapeType.PolyLineCustom:
                case ShapeType.PolyLineFeature:
                    TempGp = new GraphicsPath();
                    while (dicEnumerator.MoveNext())
                    {
                        Shape TempShp = new Shape();
                        SrcShape = (Shape)dicEnumerator.Value;
                        TempShp.AreaId = SrcShape.AreaId;
                        TempShp.AreaName = SrcShape.AreaName;
                        //TempShp.Centroid = SrcShape.Parts(j)
                        for (i = 0; i <= SrcShape.Parts.Count - 1; i++)
                        {
                            PointF[] Pts = (PointF[])SrcShape.Parts[i];
                            PointF[] PolyPts = GetLineBuffer(Pts, BufferRadius);
                            TempShp.Centroid = Pts[(int)Pts.Length / 2];
                            TempShp.Parts.Add(PolyPts);

                            TempGp.Reset();
                            TempGp.AddPolygon(PolyPts);

                            if (TempShp.Extent.IsEmpty)
                            {
                                TempShp.Extent = TempGp.GetBounds();
                            }
                            else
                            {
                                TempShp.Extent = RectangleF.Union(TempShp.Extent, TempGp.GetBounds());
                            }

                            Pts = null;
                        }

                        BufferLayer.Records.Add(TempShp.AreaId, TempShp);

                        if (BufferLayer.Extent.IsEmpty)
                        {
                            BufferLayer.Extent = TempShp.Extent;
                        }
                        else
                        {
                            BufferLayer.Extent = RectangleF.Union(BufferLayer.Extent, TempShp.Extent);
                        }

                        TempShp = null;
                    }

                    TempGp.Dispose();
                    break;
            }
            BufferLayer.LayerType = ShapeType.PolygonBuffer;
            BufferLayer.FillColor = Color.FromArgb(40, 255, 0, 0);
            BufferLayer.BorderColor = Color.Transparent;
            return BufferLayer;
        }

        public Layer CreateBufferLayer(PointF[] moBufferPts, float BufferRadius, BufferStyle eBufferStyle, string BufferName)
        {
            int i;

            //*** BugFix 11 Feb 2006 Error on Double clicking on same point
            if (moBufferPts.Length > 1)
            {
                if (moBufferPts[moBufferPts.Length - 1].Equals(moBufferPts[moBufferPts.Length - 2]))
                {
                    // ERROR: Not supported in C#: ReDimStatement
                }
            }

            //*** BugFix 02 Feb 2006 If user double click on first point itself for line buffer then create point buffer
            if (eBufferStyle == BufferStyle.Line & moBufferPts.Length == 1)
                eBufferStyle = BufferStyle.Point;

            //*** Transform Buffer Points to Latitude and Longitude
            for (i = 0; i <= moBufferPts.Length - 1; i++)
            {
                moBufferPts[i] = PointToClient(moBufferPts[i]);
            }

            //*** Create buffer
            Shape TempShp = new Shape();
            Layer BufferLayer = new Layer();
            GraphicsPath TempGp = new GraphicsPath();

            PointF[] PolyPts;
            if (eBufferStyle == BufferStyle.Point)
            {
                //*** Point Buffer
                GraphicsPath LyrGp = new GraphicsPath();
                for (i = 0; i <= moBufferPts.Length - 1; i++)
                {
                    TempShp = new Shape();
                    TempShp.AreaId = BufferName + "_" + i + 1;
                    TempShp.AreaName = BufferName + "_" + i + 1;
                    TempShp.Centroid = moBufferPts[i];
                    PolyPts = GetPointBuffer(moBufferPts[i], BufferRadius);
                    TempShp.Parts.Add(PolyPts);
                    TempGp.Reset();
                    TempGp.AddPolygon(PolyPts);
                    TempShp.Extent = TempGp.GetBounds();
                    BufferLayer.Records.Add(TempShp.AreaId, TempShp);
                    LyrGp.AddPolygon(PolyPts);
                }
                BufferLayer.Extent = LyrGp.GetBounds();
                LyrGp.Dispose();
            }
            else
            {
                //*** Line Buffer
                TempShp.AreaId = BufferName;
                TempShp.AreaName = BufferName;
                TempShp.Centroid = moBufferPts[(int)moBufferPts.Length / 2];
                PolyPts = GetLineBuffer(moBufferPts, BufferRadius);
                TempShp.Parts.Add(PolyPts);
                TempGp.AddPolygon(PolyPts);
                TempShp.Extent = TempGp.GetBounds();
                BufferLayer.Extent = TempGp.GetBounds();
                BufferLayer.Records.Add(TempShp.AreaId, TempShp);
            }

            TempGp.Dispose();
            TempShp = null;

            BufferLayer.LayerType = ShapeType.PolygonBuffer;
            BufferLayer.FillColor = Color.FromArgb(40, 255, 0, 0);
            BufferLayer.BorderColor = Color.Transparent;

            return BufferLayer;
        }

        public PointF[] GetPointBuffer(PointF Pt, float BufferRadius)
        {
            int i = 0;
            int Deg;
            PointF[] Pts = new PointF[37];
            for (Deg = 0; Deg <= 360; Deg += 10)
            {
                Pts[i].X = Pt.X + ((float)((double)BufferRadius * Math.Cos(Deg * Math.PI / 180) * (SCALE_FACTOR)));
                Pts[i].Y = Pt.Y - ((float)((double)BufferRadius * Math.Sin(Deg * Math.PI / 180) * (SCALE_FACTOR)));
                i += 1;
            }
            return Pts;
        }

        public PointF[] GetLineBuffer(PointF[] Pts, float BufferRadius)
        {
            //*** This Fuction takes coordinates of line as point array and returns buffer polygon as point array
            int i;

            PointF[] PolyPts = new PointF[Pts.Length * 3 - 2];
            object[] RectCol = new object[Pts.Length - 1];
            int FwdCtr = 0;
            int BackCtr = 0;
            float x1;
            float y1;
            float x2;
            float y2;
            float x3;
            float y3;
            float x4;
            float y4;
            float Ua;
            float Ub;
            float Den;

            float Area;
            float BufferShift = BufferRadius * SCALE_FACTOR;
            float Theta;
            bool Anticlockwise;

            List<int> NaNValueIndex = new List<int>();  //-- This stores the index number of those values which are calculated "NaN".. For.eg: float x = NaN

            for (i = 0; i <= Pts.Length - 2; i++)
            {
                Theta = (float)Math.Tanh((Pts[i].Y - Pts[i + 1].Y) / (Pts[i].X - Pts[i + 1].X));
                if (float.IsNaN(Theta))
                {
                    Theta = 0;
                }

                //* 180 / Math.PI
                float XDiff;
                float YDiff;
                PointF[] Poly = new PointF[4];

                XDiff = BufferShift * (float)Math.Sin(Theta);
                YDiff = BufferShift * (float)Math.Cos(Theta);

                Poly[0] = new PointF(Pts[i].X - XDiff, Pts[i].Y + YDiff);
                Poly[1] = new PointF(Pts[i].X + XDiff, Pts[i].Y - YDiff);
                Poly[2] = new PointF(Pts[i + 1].X + XDiff, Pts[i + 1].Y - YDiff);
                Poly[3] = new PointF(Pts[i + 1].X - XDiff, Pts[i + 1].Y + YDiff);

                //Area = Area + (x2 - x1) * (y2 + y1) / 2
                Area = 0;
                Area = Area + (Poly[0].X - Poly[3].X) * (Poly[0].Y + Poly[3].Y) / 2;
                Area = Area + (Poly[1].X - Poly[0].X) * (Poly[1].Y + Poly[0].Y) / 2;
                Area = Area + (Poly[2].X - Poly[1].X) * (Poly[2].Y + Poly[1].Y) / 2;
                Area = Area + (Poly[3].X - Poly[2].X) * (Poly[3].Y + Poly[2].Y) / 2;

                //Change the direction if its clockwise
                if (Area > 0)
                {
                    Poly[1] = new PointF(Pts[i].X - XDiff, Pts[i].Y + YDiff);
                    Poly[0] = new PointF(Pts[i].X + XDiff, Pts[i].Y - YDiff);
                    Poly[3] = new PointF(Pts[i + 1].X + XDiff, Pts[i + 1].Y - YDiff);
                    Poly[2] = new PointF(Pts[i + 1].X - XDiff, Pts[i + 1].Y + YDiff);
                }

                RectCol[i] = Poly;
            }

            //*** BugFix 11 Feb 2006 Drawing line buffer with two points only
            if (RectCol.Length == 1)
            {
                PolyPts = (PointF[])RectCol[0];
            }
            else
            {

                for (i = 0; i <= RectCol.Length - 1; i++)
                {
                    PointF[] RectPts1 = new PointF[4];
                    PointF[] RectPts2 = new PointF[4];
                    if (i == 0)
                    {
                        RectPts1 = (PointF[])RectCol[i];
                        PolyPts[0] = RectPts1[1];
                        //-- Check assigned float value is NaN
                        if (this.IsNaN_Or_Infinity(PolyPts[0]))
                        {
                            //-- preserve index value for future correction.
                            NaNValueIndex.Add(0);
                        }

                        PolyPts[PolyPts.Length - 1] = RectPts1[0];
                        //-- Check assigned float value is NaN
                        if (this.IsNaN_Or_Infinity(PolyPts[PolyPts.Length - 1]))
                        {
                            //-- preserve index value for future correction.
                            NaNValueIndex.Add(PolyPts.Length - 1);
                        }

                        FwdCtr = 0;
                        BackCtr = PolyPts.Length - 1;
                    }
                    else
                    {
                        RectPts1 = (PointF[])RectCol[i - 1];
                        RectPts2 = (PointF[])RectCol[i];

                        Anticlockwise = CounterClockDirection(Pts[i - 1], Pts[i], Pts[i + 1]);

                        if (Anticlockwise == false)
                        {
                            PolyPts[FwdCtr + 1] = RectPts1[2];

                            //-- Check assigned float value is NaN
                            if (this.IsNaN_Or_Infinity(PolyPts[FwdCtr + 1]))
                            {
                                //-- preserve index value for future correction.
                                NaNValueIndex.Add(FwdCtr + 1);
                            }

                            PolyPts[FwdCtr + 2] = RectPts2[1];

                            //-- Check assigned float value is NaN
                            if (this.IsNaN_Or_Infinity(PolyPts[FwdCtr + 2]))
                            {
                                //-- preserve index value for future correction.
                                NaNValueIndex.Add(FwdCtr + 2);
                            }

                            FwdCtr += 2;

                            x1 = RectPts1[0].X;
                            y1 = RectPts1[0].Y;
                            x2 = RectPts1[3].X;
                            y2 = RectPts1[3].Y;
                            x3 = RectPts2[3].X;
                            y3 = RectPts2[3].Y;
                            x4 = RectPts2[0].X;
                            y4 = RectPts2[0].Y;

                            Den = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
                            Ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / Den;
                            Ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / Den;
                            PolyPts[BackCtr - 1].X = x1 + Ua * (x2 - x1);
                            PolyPts[BackCtr - 1].Y = y1 + Ua * (y2 - y1);

                            //-- Check assigned float value is NaN
                            if (this.IsNaN_Or_Infinity(PolyPts[BackCtr - 1]))
                            {
                                //-- preserve index value for future correction.
                                NaNValueIndex.Add(BackCtr - 1);
                            }

                            BackCtr -= 1;
                        }
                        else
                        {
                            x1 = RectPts1[1].X;
                            y1 = RectPts1[1].Y;
                            x2 = RectPts1[2].X;
                            y2 = RectPts1[2].Y;
                            x3 = RectPts2[2].X;
                            y3 = RectPts2[2].Y;
                            x4 = RectPts2[1].X;
                            y4 = RectPts2[1].Y;

                            Den = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
                            Ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / Den;
                            Ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / Den;

                            PolyPts[FwdCtr + 1].X = x1 + Ua * (x2 - x1);
                            PolyPts[FwdCtr + 1].Y = y1 + Ua * (y2 - y1);
                            //-- Check assigned float value is NaN
                            if (this.IsNaN_Or_Infinity(PolyPts[FwdCtr + 1]))
                            {
                                //-- preserve index value for future correction.
                                NaNValueIndex.Add(FwdCtr + 1);
                            }
                            FwdCtr += 1;

                            PolyPts[BackCtr - 1] = RectPts1[3];
                            PolyPts[BackCtr - 2] = RectPts2[0];
                            //-- Check assigned float value is NaN
                            if (this.IsNaN_Or_Infinity(PolyPts[BackCtr - 2]))
                            {
                                //-- preserve index value for future correction.
                                NaNValueIndex.Add(BackCtr - 2);
                            }
                            BackCtr -= 2;
                        }
                    }
                    if (i == RectCol.Length - 1)
                    {
                        PolyPts[FwdCtr + 1] = RectPts2[2];
                        //-- Check assigned float value is NaN
                        if (this.IsNaN_Or_Infinity(PolyPts[FwdCtr + 1]))
                        {
                            //-- preserve index value for future correction.
                            NaNValueIndex.Add(FwdCtr + 1);
                        }

                        PolyPts[FwdCtr + 2] = RectPts2[3];
                        //-- Check assigned float value is NaN
                        if (this.IsNaN_Or_Infinity(PolyPts[FwdCtr + 2]))
                        {
                            //-- preserve index value for future correction.
                            NaNValueIndex.Add(FwdCtr + 2);
                        }
                    }
                }

            }

            //-- IMP: Now remove NaN float values if coordinates values previously calulated as NaN.
            //-- to remove NaN, simply re-assign value again with adjacent available point, either N-1 or N+1
            foreach (int iNaN in NaNValueIndex)
            {
                if (iNaN == 0)
                {
                    //-- Loop through all pointF from 0 to N, 
                    // if found NOT NaN, then assign that value.
                    for (int p = 0; p < PolyPts.Length; p++)
                    {
                        if (!(float.IsNaN(PolyPts[p].X) && float.IsNaN(PolyPts[p].Y)))
                        {
                            PolyPts[iNaN] = PolyPts[p];
                            break;
                        }
                    }
                }
                else if (iNaN > 0)
                {
                    //-- Loop through all pointF from N to 0, 
                    // if found NOT NaN, then assign that value.
                    for (int p = iNaN; p >= 0; p--)
                    {
                        if (!(this.IsNaN_Or_Infinity(PolyPts[p])))
                        {
                            PolyPts[iNaN] = PolyPts[p];
                            break;
                        }
                    }
                }
            }

            return PolyPts;
        }

        private bool CounterClockDirection(PointF PtA, PointF PtB, PointF PtC)
        {
            bool functionReturnValue = false;
            float E1x;
            float E1y;
            float E2x;
            float E2y;
            E1x = PtA.X - PtB.X;
            E1y = PtA.Y - PtB.Y;
            E2x = PtC.X - PtB.X;
            E2y = PtC.Y - PtB.Y;
            if ((E1x * E2y - E1y * E2x) >= 0)
                functionReturnValue = true;
            return functionReturnValue;
        }

        private bool IsNaN_Or_Infinity(PointF point)
        {
            // Checks if found NOT NaN or infinity values in X or Y

            bool RetVal = false;

            if (float.IsNaN(point.X) || float.IsNaN(point.Y) || point.X == float.PositiveInfinity || point.X == float.NegativeInfinity || point.Y == float.PositiveInfinity || point.Y == float.NegativeInfinity)
            {
                RetVal = true;
            }

            return RetVal;
        }

        #endregion

        #region " Metadata Info"


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="MD_IND_Fields">Comma delimited indicator metadata fields. MD_IND_1,MD_IND_2,MD_IND_5 (userPreference.DataView.MetadataIndicatorField)</param>
        ///// <param name="MD_AREA_Fields"></param>
        ///// <param name="MD_SRC_Fields"></param>
        //public void SetMetadataInfo(string MD_IND_Fields, string MD_AREA_Fields, string MD_SRC_Fields)
        //{
        //    this._MDIndicatorFields = this.UserPreference.DataView.MetadataIndicatorField;
        //    this._MDAreaFields = MD_AREA_Fields;
        //    this._MDSourceFields = MD_SRC_Fields;
        //}

        /// <summary>
        /// Sets Metadata columns properties using Map.Userpreference
        /// </summary>
        private void SetMetadataInfo()
        {
            this._MDIndicatorFields = this.UserPreference.DataView.MetadataIndicatorField;
            this._MDAreaFields = this.UserPreference.DataView.MetadataAreaField; ;
            this._MDSourceFields = this.UserPreference.DataView.MetadataSourceField; ;
        }

        #endregion

        #region " Helper Functions"
        public void Print()
        {

        }

        public void Export()
        {
        }

        private decimal GetMaximumChartDataValue(Hashtable chartData)
        {
            decimal RetVal = 0;
            string[] DataArray = null;
            decimal value = 0;
            if (chartData != null)
            {
                foreach (string Key in chartData.Keys)
                {
                    if (string.IsNullOrEmpty(chartData[Key].ToString()) == false)
                    {
                        DataArray = chartData[Key].ToString().Split(',');
                        foreach (string valueStr in DataArray)
                        {
                            value = (decimal)Microsoft.VisualBasic.Conversion.Val(valueStr);
                            if (value > RetVal)
                            {
                                RetVal = value;
                            }
                        }
                    }
                }
            }
            return RetVal;
        }

        private decimal GetMaximumChartDataValue(string[] dataArray)
        {
            decimal RetVal = 0;
            decimal value = 0;
            foreach (string valueStr in dataArray)
            {
                if (string.IsNullOrEmpty(valueStr) == false)
                {
                    value = (decimal)Microsoft.VisualBasic.Conversion.Val(valueStr);
                    if (value > RetVal)
                    {
                        RetVal = value;
                    }
                }
            }
            return RetVal;
        }

        private string GetMRDForThemeArea(DataView _themeData, string indicatorNId, string unitNId, string subgroupValNId, string areaNId)
        {
            return this.GetMRDForThemeArea(_themeData, indicatorNId, unitNId, subgroupValNId, areaNId, string.Empty);
        }

        /// <summary>
        /// Gets Most recent data for sopecified indicator, unit, subgroup, Area , source. from databview.
        /// </summary>
        private string GetMRDForThemeArea(DataView _themeData, string indicatorNId, string unitNId, string subgroupValNId, string areaNId, string sourceNId)
        {
            string RetVal = string.Empty;   //Most recent dataValue
            DataView TempDataView = null;
            string RowFilter = string.Empty;
            string OriginalFilter = string.Empty;
            string OriginalSort = string.Empty;
            string sort = Timeperiods.TimePeriod + " DESC";
            try
            {
                if (_themeData != null && _themeData.Count > 0)
                {
                    OriginalFilter = _themeData.RowFilter;
                    OriginalSort = _themeData.Sort;

                    //-- Apply RowFilter of Indicator , Unit, Subgroup, Area
                    RowFilter = Indicator.IndicatorNId + " IN (" + indicatorNId + ")";

                    if (string.IsNullOrEmpty(unitNId) == false)
                    {
                        RowFilter += " AND " + Unit.UnitNId + " IN (" + unitNId + ") ";
                    }


                    RowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + subgroupValNId + ") AND " + Area.AreaNId + " IN(" + areaNId + ")";

                    if (string.IsNullOrEmpty(sourceNId) == false)
                    {
                        RowFilter += " AND " + IndicatorClassifications.ICNId + " IN (" + sourceNId + ")";
                    }

                    _themeData.RowFilter = RowFilter;

                    if (_themeData.Sort.Length > 0)
                    {
                        sort = sort + ", " + _themeData.Sort;
                    }
                    _themeData.Sort = sort;

                    if (_themeData.Count > 0)
                    {
                        //- get dataValue at first row (after sorting on the basis of TimePeriod)
                        RetVal = Convert.ToDecimal(_themeData[0][DataExpressionColumns.NumericData]).ToString();
                    }

                    _themeData.RowFilter = OriginalFilter;
                    _themeData.Sort = OriginalSort;
                }
            }

            catch (Exception e)
            {
                _themeData.RowFilter = OriginalFilter;
                _themeData.Sort = OriginalSort;
            }
            return RetVal;
        }

        /// <summary>
        /// Gets the array of GIDs of Indicator, Unit, SubgroupVal by their NIDs specified.
        /// </summary>
        private string[] GetI_U_S_GIDsByNids(DataView _themeData, string ind_NId, string unit_Nid, string subgroup_val_Nid)
        {
            string[] RetVal = new string[3];

            string OrgRowFilter = string.Empty;

            if (_themeData != null)
            {
                //-- Preserve Original row Filter
                OrgRowFilter = _themeData.RowFilter;


                //-- Get Indicator GID
                _themeData.RowFilter = Indicator.IndicatorNId + "= " + ind_NId;

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(Indicator.IndicatorGId))
                {
                    RetVal[0] = _themeData[0][Indicator.IndicatorGId].ToString();
                }

                //-- Get Unitn GID
                _themeData.RowFilter = Unit.UnitNId + "= " + unit_Nid;

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(Unit.UnitGId))
                {
                    RetVal[1] = _themeData[0][Unit.UnitGId].ToString();
                }

                //-- Get SubgroupVal GID
                _themeData.RowFilter = SubgroupVals.SubgroupValNId + "=" + subgroup_val_Nid;

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                {
                    RetVal[2] = _themeData[0][SubgroupVals.SubgroupValGId].ToString();
                }


                //-- Set RowFilter
                _themeData.RowFilter = OrgRowFilter;
            }


            return RetVal;
        }

        private string[] GetI_U_S_NidsByGIDs(DataView _themeData, string ind_GID, string unit_GID, string subgroup_val_GID)
        {
            string[] RetVal = new string[3];

            string OrgRowFilter = string.Empty;

            if (_themeData != null)
            {
                //-- Preserve Original row Filter
                OrgRowFilter = _themeData.RowFilter;


                //-- Get Indicator GID
                _themeData.RowFilter = Indicator.IndicatorGId + "='" + ind_GID + "'";

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(Indicator.IndicatorNId))
                {
                    RetVal[0] = _themeData[0][Indicator.IndicatorNId].ToString();
                }

                //-- Get Unitn GID
                _themeData.RowFilter = Unit.UnitGId + "='" + unit_GID + "'";

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(Unit.UnitNId))
                {
                    RetVal[1] = _themeData[0][Unit.UnitNId].ToString();
                }

                //-- Get SubgroupVal GID
                _themeData.RowFilter = SubgroupVals.SubgroupValGId + "='" + subgroup_val_GID + "'";

                if (_themeData.Count > 0 && _themeData.Table.Columns.Contains(SubgroupVals.SubgroupValNId))
                {
                    RetVal[2] = _themeData[0][SubgroupVals.SubgroupValNId].ToString();
                }


                //-- Set RowFilter
                _themeData.RowFilter = OrgRowFilter;
            }


            return RetVal;
        }


        public DataView GetAreaLevels()
        {
            return GetAreaLevels(false);
        }

        public DataView GetAreaLevels(bool p_ShowAll)
        {
            DataView RetVal = null;
            string sSql = string.Empty;
            if (p_ShowAll == true) //Get all AreaLevels
            {
                sSql = DIQueries.Area.GetAreaLevel(FilterFieldType.None, "");
            }
            else                //Get AreaLevels based on Presentataion Data
            {
                string sAreaNIds = GetAreaNIds();
                sSql = DIQueries.Area.GetAreaLevel(sAreaNIds);
            }

            try
            {
                RetVal = DIConnection.ExecuteDataTable(sSql).DefaultView;

            }
            catch (Exception ex)
            {
                //In case of online database some times error occurs
                System.Diagnostics.Debug.Print(ex.Message);
            }

            //return m_QueryBase.Area_GetLevels(p_ShowAll);
            return RetVal;
        }

        /// <summary>
        /// Returns Comma delimited AreaNIds. AreaNIds are picked from userselection if they exists or else distinct AreaNIds are picked from presentation data
        /// </summary>
        /// <returns></returns>
        private string GetAreaNIds()
        {
            string RetVal = this.UserPreference.UserSelection.AreaNIds;

            if (RetVal.Length == 0)
            {
                // If user selection does not contains AreaNIds then get distinct AreaNIds from PresentationData
                string[] _DistinctColumn = new string[1];
                _DistinctColumn[0] = Area.AreaNId;
                DataTable dtArea = this.PresentationData.ToTable(true, _DistinctColumn);
                for (int i = 0; i < dtArea.Rows.Count; i++)
                {
                    if (RetVal.Length > 0)
                    {
                        RetVal += ",";
                    }
                    RetVal += dtArea.Rows[i][Area.AreaNId].ToString();
                }
            }

            try
            {
                //- Add AreaNId of BackgroudMapAreaID (if any)
                if (string.IsNullOrEmpty(this._BackgroundMapAreaID) == false)
                {
                    //- 1. Get AreaNId for given AreaID
                    DataTable DTArea = this.DIConnection.ExecuteDataTable(this.DIQueries.Area.GetArea(FilterFieldType.ID, "'" + DIQueries.RemoveQuotesForSqlQuery(this._BackgroundMapAreaID) + "'"));

                    if (DTArea != null && DTArea.Rows.Count > 0)
                    {
                        //- Concatenate AreaNId with final AreaNIds
                        // so that this Area's Layer will be automatically included in Map.
                        RetVal += "," + Convert.ToString(DTArea.Rows[0][Area.AreaNId]);
                        RetVal = RetVal.Trim(',');
                    }
                }
            }
            catch
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Gets Distinct Indicator NID, Unit Nid, SubGroup Nid, Indicator Name, unit Name, SubGroup Name from PresenationData.
        /// </summary>
        /// <returns>DataView, containing distinct IUS Nids and Names.</returns>
        /// <remarks>This is required when user adds theme in case Map is loaded from serialized.</remarks>
        public DataView GetIUSDataView()
        {
            string[] _DistinctColumn = new string[6];
            _DistinctColumn[0] = Indicator.IndicatorNId;
            _DistinctColumn[1] = Unit.UnitNId;
            _DistinctColumn[2] = SubgroupVals.SubgroupValNId;
            _DistinctColumn[3] = Indicator.IndicatorName;
            _DistinctColumn[4] = Unit.UnitName;
            _DistinctColumn[5] = SubgroupVals.SubgroupVal;
            DataTable dt = this.MRDData.Table.Clone();
            dt.Merge(this.MRDData.Table);
            dt = dt.DefaultView.ToTable(true, _DistinctColumn);
            return dt.DefaultView;
        }

        /// <summary>
        /// Get distinct timeperiods from presentation data
        /// </summary>
        /// <returns></returns>
        public DataView GetTimePeriods()
        {
            string[] _DistinctColumn = new string[2];
            _DistinctColumn[0] = Timeperiods.TimePeriodNId;
            _DistinctColumn[1] = Timeperiods.TimePeriod;

            DataTable dt = this.PresentationData.ToTable(true, _DistinctColumn);
            return dt.DefaultView;

            //return m_QueryBase.Time_GetTimePeriods();
        }

        /// <summary>
        /// Get distinct timeperiods and area from presentation data
        /// </summary>
        /// <returns></returns>
        public DataTable GetDistinctTimePeriodsAndArea()
        {
            DataTable RetVal = null;

            string[] _DistinctColumn = new string[2];
            _DistinctColumn[0] = Timeperiods.TimePeriod;
            _DistinctColumn[1] = Area.AreaID;

            RetVal = this.PresentationData.ToTable(true, _DistinctColumn);

            return RetVal;

            //return m_QueryBase.Time_GetTimePeriods();
        }

        /// <summary>
        /// Get Distinct TimePeriods for all themes in collection
        /// </summary>
        /// <returns></returns>
        public DataView GetTimePeriodsForThemes()
        {
            DataTable dt = new DataTable();
            string RowFilter = string.Empty;
            string IndicatorNIds = string.Empty;    //Holds all theme's IndicatorNids.
            string UnitNIds = string.Empty;         //Holds all theme's UnitNids.
            string SubgroupValNIds = string.Empty;  //Holds all theme's SubgroupValNids.
            string[] _DistinctColumn = new string[2];
            _DistinctColumn[0] = Timeperiods.TimePeriodNId;
            _DistinctColumn[1] = Timeperiods.TimePeriod;

            string OriginalSort = string.Empty;

            //Set row filter based on all themes in theme collection 
            foreach (Theme Th in this.Themes)
            {
                if (Th.Visible == true)
                {
                    if (IndicatorNIds.Length != 0)
                    {
                        IndicatorNIds += ",";
                    }
                    IndicatorNIds += string.Join(",", Th.IndicatorNId);

                    if (UnitNIds.Length != 0)
                    {
                        UnitNIds += ",";
                    }
                    UnitNIds += Th.UnitNId;

                    if (SubgroupValNIds.Length != 0)
                    {
                        SubgroupValNIds += ",";
                    }
                    SubgroupValNIds += string.Join(",", Th.SubgroupNId);

                }
            }

            RowFilter += Indicator.IndicatorNId + " IN (" + IndicatorNIds + ") AND " + Unit.UnitNId + " IN (" + UnitNIds + ")";

            if (SubgroupValNIds != "-1")    // In MultiSelect case, subgroupValNId could be -1 .
            {
                RowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + SubgroupValNIds + ")";
            }

            this.PresentationData.RowFilter = RowFilter;

            //-- Apply TimePeriod Sorting
            OriginalSort = this.PresentationData.Sort;
            this.PresentationData.Sort = Timeperiods.TimePeriod + " ASC";

            dt = this.PresentationData.ToTable(true, _DistinctColumn);
            this.PresentationData.RowFilter = "";
            this.PresentationData.Sort = OriginalSort;

            return dt.DefaultView;
        }

        public DataView GetUniqueSourceList()
        {
            return GetUniqueSourceList(false);
        }

        public DataView GetUniqueSourceList(bool bThemeBased)
        {
            if (bThemeBased)
            {
                //***BugFix / Enhancement 27 Apr 2006 Unique Source list based on map themes

                DataTable _DT = new DataTable();
                DataColumn[] _Col = new DataColumn[1];
                int i;
                _Col[0] = _DT.Columns.Add("IC_Name");
                _DT.PrimaryKey = _Col;

                foreach (Theme _Theme in m_Themes)
                {
                    //TODO Check this case                      

                    //_DV = m_QueryBase.GetUniqueSourceList(_Theme.IndicatorNId[0], _Theme.SubgroupNId[0], _Theme.UnitNId.ToString());

                    string sRowFilter = Indicator.IndicatorNId + " IN (" + _Theme.IndicatorNId[0] + ") AND " + Unit.UnitNId + " IN (" + _Theme.UnitNId.ToString() + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + _Theme.SubgroupNId[0] + ")";
                    this.MRDData.RowFilter = sRowFilter;
                    DataView _DV = new DataView(this.MRDData.ToTable());
                    this.MRDData.RowFilter = "";

                    for (i = 0; i <= _DV.Count - 1; i++)
                    {
                        object[] rowVals = new object[1];
                        rowVals[0] = _DV[i]["IC_Name"];
                        try
                        {
                            _DT.Rows.Add(rowVals);
                        }
                        catch (Exception ex)
                        {
                            //Console.Write("Add only unique values")
                        }
                    }
                }
                return _DT.DefaultView;
            }
            else
            {
                //-- Bugfix 2007-07-24 Consider NotesFilter while refering current dataview
                //return m_QueryBase.Table_GetUniqueList((int)QueryBase.ColumnType.Source);
                string[] _DistinctColumn = new string[1];
                _DistinctColumn[0] = IndicatorClassifications.ICName;
                DataTable dt = this.PresentationData.ToTable(true, _DistinctColumn);
                dt.Columns[0].ColumnName = "IC_Name";
                return dt.DefaultView;
            }
        }

        /// <summary>
        /// Gets all SubgroupValNIDs present in presentationData for given Indicator, Unit.
        /// </summary>
        public DataTable GetSubgroupsNIDsBy_IndicatorUnit(string indicatorNIds, string unitNIds)
        {
            DataTable RetVal = null;    // new string[] { "" };
            string OriginalFilter = string.Empty;

            if (!(string.IsNullOrEmpty(indicatorNIds) && string.IsNullOrEmpty(unitNIds)))
            {
                DataTable SubgroupNIdTable = null;
                string RowFilter = string.Empty;

                //Get all SubgroupsNIDs for given Indicator, unit from Presentation data
                OriginalFilter = this.PresentationData.RowFilter;
                RowFilter = Indicator.IndicatorNId + " IN (" + indicatorNIds + ") AND " + Unit.UnitNId + " IN (" + unitNIds + ")";
                this.PresentationData.RowFilter = RowFilter;

                SubgroupNIdTable = this.PresentationData.ToTable(true, SubgroupVals.SubgroupValNId, SubgroupVals.SubgroupVal);

                //RetVal = new string[SubgroupNIdTable.Rows.Count];

                ////for (int i = 0; i < SubgroupNIdTable.Rows.Count; i++)
                ////{
                ////    RetVal[i] = SubgroupNIdTable.Rows[i][SubgroupVals.SubgroupValNId].ToString();
                ////}

                RetVal = SubgroupNIdTable;
            }
            this.PresentationData.RowFilter = OriginalFilter;

            return RetVal;
        }

        /// <summary>
        /// Gets Possible SourceNIDs present in presentationData for given Indicator, Unit, Subgroups.
        /// </summary>
        public string[] GetSourceNIDsBy_I_U_S(string indicatorNIds, string unitNIds, string SubgroupNIds)
        {
            string[] RetVal = new string[] { "" };
            string OriginalFilter = string.Empty;

            if (!(string.IsNullOrEmpty(indicatorNIds) && string.IsNullOrEmpty(unitNIds) && string.IsNullOrEmpty(SubgroupNIds)))
            {
                DataTable SubgroupNIdTable = null;
                string RowFilter = string.Empty;

                //Get all SubgroupsNIDs for given Indicator, unit from Presentation data
                OriginalFilter = this.PresentationData.RowFilter;
                RowFilter = Indicator.IndicatorNId + " IN(" + indicatorNIds + ") AND " + Unit.UnitNId + " IN (" + unitNIds + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + SubgroupNIds + ")";
                this.PresentationData.RowFilter = RowFilter;

                SubgroupNIdTable = this.PresentationData.ToTable(true, SubgroupVals.SubgroupValNId);

                RetVal = new string[SubgroupNIdTable.Rows.Count];

                for (int i = 0; i < SubgroupNIdTable.Rows.Count; i++)
                {
                    RetVal[i] = SubgroupNIdTable.Rows[i][SubgroupVals.SubgroupValNId].ToString();
                }
            }
            this.PresentationData.RowFilter = OriginalFilter;

            return RetVal;
        }

        public void ReplaceBaseLayer(SourceType p_SourceType, string p_OldLayerID, string p_NewLayerID)
        {
            ReplaceBaseLayer(p_SourceType, p_OldLayerID, p_NewLayerID, "");
        }

        public void ReplaceBaseLayer(SourceType p_SourceType, string p_OldLayerID, string p_NewLayerID, string p_SourcePath)
        {
            {
                try
                {
                    Layers[p_OldLayerID].Records.Clear();
                }
                catch (Exception)
                {

                }

                switch (p_SourceType)
                {
                    case SourceType.Database:
                        //DataView _Dv = m_QueryBase.Map_GetLayerInfo(m_SpatialMapFolder, p_NewLayerID);
                        //string sSql = this.DIQueries.
                        string sSql = this.DIQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.LayerName, "'" + DICommon.RemoveQuotes(p_NewLayerID) + "'", FieldSelection.Heavy);
                        DataView _Dv = this.DIConnection.ExecuteDataTable(sSql).DefaultView;
                        foreach (DataRowView _DRv in _Dv)
                        {
                            ExtractShapeFile(_DRv, this.SpatialMapFolder);
                            break;
                        }
                        Layers[p_OldLayerID].StartDate = (System.DateTime)_Dv[0]["Start_Date"];
                        Layers[p_OldLayerID].EndDate = (System.DateTime)_Dv[0]["End_Date"];
                        break;
                    case SourceType.Shapefile:
                        if (!File.Exists(m_SpatialMapFolder + "/" + p_NewLayerID + ".shp"))
                        {
                            File.Copy(p_SourcePath + "/" + p_NewLayerID + ".shp", m_SpatialMapFolder + "/" + p_NewLayerID + ".shp");
                            File.Copy(p_SourcePath + "/" + p_NewLayerID + ".shx", m_SpatialMapFolder + "/" + p_NewLayerID + ".shx");
                            File.Copy(p_SourcePath + "/" + p_NewLayerID + ".dbf", m_SpatialMapFolder + "/" + p_NewLayerID + ".dbf");
                        }

                        break;
                }
                ShapeInfo _ShapeFile = ShapeFileReader.GetShapeInfo(m_SpatialMapFolder, p_NewLayerID);
                Layers[p_OldLayerID].Extent = _ShapeFile.Extent;
                Layers[p_OldLayerID].RecordCount = _ShapeFile.RecordCount;
                Layers[p_OldLayerID].LayerName = p_NewLayerID;
                Layers[p_OldLayerID].LayerType = _ShapeFile.ShapeType;
                foreach (Theme _theme in m_Themes)
                {
                    {
                        if (_theme.LayerVisibility.ContainsKey(p_OldLayerID))
                        {
                            if (_theme.LayerVisibility.ContainsKey(p_NewLayerID) == false)
                                _theme.LayerVisibility.Add(p_NewLayerID, _theme.LayerVisibility[p_OldLayerID]);
                            _theme.LayerVisibility.Remove(p_OldLayerID);
                        }
                    }
                }
                Layers[p_OldLayerID].ID = p_NewLayerID;
            }
        }
        #endregion

        public static int GetFontStyleInteger(string fontStyle)
        {
            int RetVal = 0;     //Default 0 for Regular Font Style.
            switch (fontStyle)
            {
                case "Regular":
                    RetVal = 0;
                    break;
                case "Bold":
                    RetVal = 1;
                    break;
                case "Italic":
                    RetVal = 2;
                    break;
                case "Underline":
                    RetVal = 4;
                    break;
                case "Strikeout":
                    RetVal = 8;
                    break;
            }
            return RetVal;
        }

        # endregion

        #region "-- Events --"

        /// <summary>
        /// Event for map title & Subtitle changed.
        /// </summary>
        public event EventHandler MapTitleChanged;

        #endregion
    }


}