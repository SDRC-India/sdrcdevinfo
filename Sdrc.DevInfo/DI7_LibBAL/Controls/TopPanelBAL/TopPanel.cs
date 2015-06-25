using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibBAL.Controls.TopPanelBAL
{
    #region "-- Enums --"

    /// <summary>
    /// Enum to define availabe button types in top panel control
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 0
        /// </summary>
        Logo = 0,

        /// <summary>
        /// 1
        /// </summary>
        Indicator = 1,

        /// <summary>
        /// 2
        /// </summary>
        Area = 2,

        /// <summary>
        /// 3
        /// </summary>
        TimePeriod = 3,

        /// <summary>
        /// 4
        /// </summary>
        Source = 4,

        /// <summary>
        /// 5
        /// </summary>
        DataView = 5,

        /// <summary>
        /// 6
        /// </summary>
        Gallery = 6,

        /// <summary>
        /// 7
        /// </summary>
        Report = 7,

        /// <summary>
        /// 8
        /// </summary>
        Help = 8
    }

    #endregion

    #region "-- Delegate --"

    /// <summary>
    /// A delegate to rasie ButtonClicked event
    /// </summary>
    public delegate void ButtonClickedDelegate(ButtonType buttonType);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="databaseInfo"></param>
    /// <param name="databaseName"></param>
    public delegate void DatabaseSelectedDelegate(string databaseInfo, string databaseName);

    /// <summary>
    /// TODO
    /// </summary>
    /// <param name="indicatorNIds"></param>
    /// <param name="areaNIds"></param>
    /// <param name="timePeriodNIds"></param>
    public delegate void FreeTextSearchChangedDelegate(string indicatorNIds, string areaNIds, string timePeriodNIds, string sourceNIds);
    
    /// <summary>
    /// A delegate to raise more option clicked inside database combo(cbodatabase)
    /// </summary>
    public delegate void MoreClickedDelegate();


    #endregion

    #region "-- Class --"

    public class TopPanel
    {
        #region "-- Constants --"

        /// <summary>
        /// This will be set in consultation with graphics team based on background image height
        /// </summary>
        public const int LOGO_HEIGHT = 70;

        /// <summary>
        /// TopPanel
        /// </summary>
        public const string TOP_PANEL_FOLDER = "TopPanel";

        /// <summary>
        /// Collection of constants for all image file names
        /// </summary>
        public class ImageFileNames
        {
            /// <summary>
            /// toppanel_background.png
            /// </summary>
            public const string TOPPANEL_BACKGROUND = "toppanel_background.png";

            /// <summary>
            /// datapanel_background.png
            /// </summary>
            public const string DATAPANEL_BACKGROUND = "datapanel_background.png";

            /// <summary>
            /// adaptation_logo.png
            /// </summary>
            public const string ADAPTATION_LOGO = "adaptation_logo.png";

            /// <summary>
            /// indicator_normal.png
            /// </summary>
            public const string INDICATOR_NORMAL = "indicator_normal.png";


            /// <summary>
            /// indicator_disabled.png
            /// </summary>
            public const string INDICATOR_DISABLED = "indicator_disabled.png";

            /// <summary>
            /// indicator_hover.png
            /// </summary>
            public const string INDICATOR_HOVER = "indicator_hover.png";

            /// <summary>
            /// indicator_selected.png
            /// </summary>
            public const string INDICATOR_SELECTED = "indicator_selected.png";

            /// <summary>
            /// area_normal.png
            /// </summary>
            public const string AREA_NORMAL = "area_normal.png";

            /// <summary>
            /// area_disabled.png
            /// </summary>
            public const string AREA_DISABLED = "area_disabled.png";

            /// <summary>
            /// area_hover.png
            /// </summary>
            public const string AREA_HOVER = "area_hover.png";

            /// <summary>
            /// area_selected.png
            /// </summary>
            public const string AREA_SELECTED = "area_selected.png";

            /// <summary>
            /// timeperiod_normal.png
            /// </summary>
            public const string TIMEPERIOD_NORMAL = "timeperiod_normal.png";

            /// <summary>
            /// timeperiod_disabled.png
            /// </summary>
            public const string TIMEPERIOD_DISABLED = "timeperiod_disabled.png";


            /// <summary>
            /// timeperiod_hover.png
            /// </summary>
            public const string TIMEPERIOD_HOVER = "timeperiod_hover.png";

            /// <summary>
            /// timeperiod_selected.png
            /// </summary>
            public const string TIMEPERIOD_SELECTED = "timeperiod_selected.png";

            /// <summary>
            /// source_normal.png
            /// </summary>
            public const string SOURCE_NORMAL = "source_normal.png";

            /// <summary>
            /// source_disabled.png
            /// </summary>
            public const string SOURCE_DISABLED = "source_disabled.png";

            /// <summary>
            /// source_hover.png
            /// </summary>
            public const string SOURCE_HOVER = "source_hover.png";

            /// <summary>
            /// source_selected.png
            /// </summary>
            public const string SOURCE_SELECTED = "source_selected.png";

            /// <summary>
            /// dataview_normal.png
            /// </summary>
            public const string DATAVIEW_NORMAL = "dataview_normal.png";

            /// <summary>
            /// dataview_disabled.png
            /// </summary>
            public const string DATAVIEW_DISABLED = "dataview_disabled.png";

            /// <summary>
            /// dataview_hover.png
            /// </summary>
            public const string DATAVIEW_HOVER = "dataview_hover.png";

            /// <summary>
            /// dataview_selected.png
            /// </summary>
            public const string DATAVIEW_SELECTED = "dataview_selected.png";

            /// <summary>
            /// gallery_normal.png
            /// </summary>
            public const string GALLERY_NORMAL = "gallery_normal.png";

            /// <summary>
            /// gallery_hover.png
            /// </summary>
            public const string GALLERY_HOVER = "gallery_hover.png";

            /// <summary>
            /// report_normal.png
            /// </summary>
            public const string REPORT_NORMAL = "report_normal.png";

            /// <summary>
            /// report_hover.png
            /// </summary>
            public const string REPORT_HOVER = "report_hover.png";

            /// <summary>
            /// help_normal.png
            /// </summary>
            public const string HELP_NORMAL = "help_normal.png";

            /// <summary>
            /// help_hover.png
            /// </summary>
            public const string HELP_HOVER = "help_hover.png";

            /// <summary>
            /// help_disabled.png
            /// </summary>
            public const string HELP_DISABLED = "help_disabled.png";

            /// <summary>
            /// help_selected.png
            /// </summary>
            public const string HELP_SELECTED = "help_selected.png";
        }

        #endregion


    }

    #endregion
}
