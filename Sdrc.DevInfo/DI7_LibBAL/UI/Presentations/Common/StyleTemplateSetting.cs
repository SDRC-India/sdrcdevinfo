using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;
using DevInfo.Lib.DI_LibBAL.Utility;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

using SpreadsheetGear;
using SpreadsheetGear.Charts;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common
{
    public class StyleTemplateSetting
    {

        #region "-- Public -- "

        #region "-- Enum -- "      

        /// <summary>
        /// Enum to define the locagtion type
        /// </summary>
        public enum LocationType
        {
            Bottom,
            Corner,
            Top,
            Left,
            Right
        }      

        #endregion

        #region "-- New / Dispose -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fontTemplate"></param>
        /// <param name="show"></param>
        /// <param name="lineColor"></param>
        /// <param name="lineDrawStyle"></param>
        /// <param name="lineWidth"></param>
        /// <param name="orientation"></param>
        /// <param name="orientationAngle"></param>
        /// <param name="flip"></param>
        /// <param name="elementPostion"></param>
        /// <param name="elementAlignment"></param>
        /// <param name="extent"></param>
        /// <param name="bgColor"></param>
        /// <param name="showLabel"></param>
        public StyleTemplateSetting(FontSetting fontTemplate, bool show, string lineColor, LineDrawStyle lineDrawStyle, int lineWidth, TextOrientation orientation, int orientationAngle, bool flip, LegendPosition elementPostion, StringAlignment elementAlignment, int extent, string bgColor, bool showLabel)
        {
            this._FontTemplate = fontTemplate;
            this._Show = show;
            this._LineColor = lineColor;
            this._LineStyle = lineDrawStyle;
            this._LineWidth = lineWidth;
            this._Orientation = orientation;
            this._OrientationAngle = orientationAngle;
            this._Flip = flip;
            this._ElementPosition = elementPostion;
            this._ElementAlignment = elementAlignment;
            this._Extent = extent;
            this._BgColor = bgColor;
            this._ShowLabel = showLabel;
        }

        /// <summary>
        /// Constuctor, Only for serialization purpose
        /// </summary>
        public StyleTemplateSetting()
        { 
            //Do Nothing
        }

        #endregion

        #region " -- Properties -- "

        private FontSetting _FontTemplate = new FontSetting("Arial", FontStyle.Regular, 6, Color.Black, Color.White, StringAlignment.Center);
        /// <summary>
        /// Gets or sets the font settings
        /// </summary>
        public FontSetting FontTemplate
        {
            get { return _FontTemplate; }
            set
            {
                _FontTemplate = value;
            }
        }       

        private bool _Show = false;
        /// <summary>
        /// Gets or sets the graph object visibility
        /// </summary>
        public bool Show
        {
            get { return _Show; }
            set { _Show = value; }
        }  

        private string _LineColor = "#E9E9E9";
        /// <summary>
        /// Gets or sets the graph object line color
        /// </summary>
        public string LineColor
        {
            get { return _LineColor; }
            set { _LineColor = value; }
        }

        private LineDrawStyle _LineStyle = LineDrawStyle.Solid;
        /// <summary>
        /// Gets or sets the graph object line style
        /// </summary>
        public LineDrawStyle LineStyle
        {
            get { return _LineStyle; }
            set { _LineStyle = value; }
        }

        private int _LineWidth = 1;
        /// <summary>
        /// Gets or sets the graph object line width
        /// </summary>
        public int LineWidth
        {
            get { return _LineWidth; }
            set { _LineWidth = value; }
        }

        private TextOrientation _Orientation = TextOrientation.Custom;
        /// <summary>
        /// Gets or sets the graph object orientation
        /// </summary>
        public TextOrientation Orientation
        {
            get { return _Orientation; }
            set { _Orientation = value; }
        }

        private int _OrientationAngle = 90;
        /// <summary>
        /// Gets or sets the graph object orientation angle
        /// </summary>
        public int OrientationAngle
        {
            get { return _OrientationAngle; }
            set { _OrientationAngle = value; }
        }

        private bool _Flip = false;
        /// <summary>
        /// Gets or sets the graph object flip
        /// </summary>
        public bool Flip
        {
            get { return _Flip; }
            set { _Flip = value; }
        }

        
        private SpreadsheetGear.Charts.LegendPosition _ElementPosition = LegendPosition.Top;
        /// <summary>
        /// Gets or sets the graph object legend location
        /// </summary>
        public LegendPosition ElementPosition 
        {
            get { return _ElementPosition; }
            set { _ElementPosition = value; }
        }

        private StringAlignment _ElementAlignment = StringAlignment.Center;
        /// <summary>
        /// Gets or sets the graph object alignment
        /// </summary>
        public StringAlignment ElementAlignment 
        {
            get { return _ElementAlignment; }
            set { _ElementAlignment = value; }
        }

        private int _Extent = 30;
        /// <summary>
        /// Gets or sets the extent 
        /// </summary>
        /// <remarks>Extent - distance of the Axis drawn from the Left Bottom edge </remarks>
        public int Extent
        {
            get { return _Extent; }
            set { _Extent = value; }
        }

        private string _BgColor;
        /// <summary>
        /// Gets or sets the background color
        /// </summary>
        public string BgColor
        {
            get { return _BgColor; }
            set { _BgColor = value; }
        }

        private bool _ShowLabel = false;
        /// <summary>
        /// Gets or sets the show label
        /// </summary>
        public bool ShowLabel
        {
            get { return _ShowLabel; }
            set { _ShowLabel = value; }
        }

        private bool _ShowCaption = false;
        /// <summary>
        /// Gets or sets the show caption
        /// </summary>
        public bool ShowCaption
        {
            get { return this._ShowCaption; }
            set { this._ShowCaption = value; }
        }

        private bool _ShowRange = true;
        /// <summary>
        /// Gets or sets the show range
        /// </summary>
        public bool ShowRange
        {
            get { return this._ShowRange; }
            set { this._ShowRange = value; }
        }

        private bool _ShowCount = false;
        /// <summary>
        /// Gets or sets value to show Count. Count refers to Number of areas with in a Legend range.
        /// </summary>
        public bool ShowCount 
        {
            get { return this._ShowCount; }
            set { this._ShowCount = value; }
        }

        private bool _ShowMissingLegend = true;
        /// <summary>
        /// true/False. Show Missing Data Legend.
        /// </summary>
        public bool ShowMissingLegend
        {
            get { return _ShowMissingLegend; }
            set { _ShowMissingLegend = value; }
        }
	

        private bool _AggregateAreaByParent = true;
        /// <summary>
        /// Gets or sets the AggregateAreabyParent
        /// </summary>
        public bool AggregateAreaByParent
        {
            get 
            {
                return this._AggregateAreaByParent; 
            }
            set 
            {
                this._AggregateAreaByParent = value; 
            }
        }
	
	
        #endregion  

        #endregion  

    }
}
