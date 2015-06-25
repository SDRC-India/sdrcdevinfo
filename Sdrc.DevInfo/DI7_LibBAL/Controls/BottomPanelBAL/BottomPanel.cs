using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.BottomPanelBAL
{
    #region "-- Delegate --"

    /// <summary>
    /// A delegate to rasie ButtonClicked event
    /// </summary>
    public delegate void ButtonClickedDelegate(string buttonId);


    #endregion

    #region "-- Class --"

    public class BottomPanel
    {

        #region "-- Constants --"

        /// <summary>
        /// Background image height = 88
        /// </summary>
        public const int BOTTOM_PANEL_HEIGHT = 88; //Button Strip + Slider //103

        /// <summary>
        /// Button image width = 55
        /// </summary>
        public const int BUTTON_WIDTH = 55;

        /// <summary>
        /// Button image height = 55
        /// </summary>
        public const int BUTTON_HEIGHT = 55;

        /// <summary>
        /// Button margin = 15
        /// </summary>
        public const int BUTTON_MARGIN = 15;

        /// <summary>
        /// BottomPanel
        /// </summary>
        public const string BOTTOM_PANEL_FOLDER = "BottomPanel";

        /// <summary>
        /// Collection of constants for all image file names
        /// </summary>
        public class ImageFileNames
        {
            /// <summary>
            /// bottompanel_background.png
            /// </summary>
            public const string BOTTOMPANEL_BACKGROUND = "bottompanel_background.png";


            /// <summary>
            /// slider_expand_normal.png
            /// </summary>
            public const string SLIDER_EXPAND_NORMAL = "slider_expand_normal.png";

            /// <summary>
            /// slider_expand_hover.png
            /// </summary>
            public const string SLIDER_EXPAND_HOVER = "slider_expand_hover.png";

            /// <summary>
            /// slider_collapse_normal.png
            /// </summary>
            public const string SLIDER_COLLAPSE_NORMAL = "slider_collapse_normal.png";

            /// <summary>
            /// slider_collapse_hover.png
            /// </summary>
            public const string SLIDER_COLLAPSE_HOVER = "slider_collapse_hover.png";


            /// <summary>
            /// toppanel_background.png
            /// </summary>
            public const string SCROLL_LEFT_NORMAL = "scroll_left_normal.png";

            /// <summary>
            /// adaptation_logo.png
            /// </summary>
            public const string SCROLL_LEFT_HOVER = "scroll_left_hover.png";

            /// <summary>
            /// indicator_normal.png
            /// </summary>
            public const string SCROLL_RIGHT_NORMAL = "sroll_right_normal.png";

            /// <summary>
            /// indicator_hover.png
            /// </summary>
            public const string SCROLL_RIGHT_HOVER = "sroll_right_hover.png";
        }

        /// <summary>
        /// Collection of constants for Custom button Ids. Click on custom button will open up a client dialog
        /// </summary>
        public class CustomButtonIds
        {

            /// <summary>
            /// diAbout. Open About dialog
            /// </summary>
            public const string DIABOUT = "diAbout";

            /// <summary>
            /// diSettings. Open user preferrence dialog
            /// </summary>
            public const string DISETTINGS = "diSettings";


            /// <summary>
            /// diDataWizard. Open Data Wizard
            /// </summary>
            public const string DIDATAWIZRAD = "diDataWizard";

            /// <summary>
            /// diHelp. Open help file
            /// </summary>
            public const string DIHELP = "diHelp";

            /// <summary>
            /// diOrganization. 
            /// </summary>
            public const string DIORGANIZATION = "diOrganization";

            /// <summary>
            /// diProduct
            /// </summary>
            public const string DIPRODUCT = "diProduct";

            /// <summary>
            /// diTour
            /// </summary>
            public const string DITOUR = "diTour";

        }


        #endregion


    }

    #endregion
}
