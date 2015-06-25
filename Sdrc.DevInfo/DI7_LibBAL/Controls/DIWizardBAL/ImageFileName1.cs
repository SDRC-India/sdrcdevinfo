using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{
    /// <summary>
    /// Constants of Images used in DI Data Wizard.
    /// </summary>
    public static class ImageFileName1
    {
        #region " -- Constants / Read only Properties -- "

        #region " -- Locations -- "
        
        /// <summary>
        /// diWizard
        /// </summary>
        public const string DIWIZARD = "diWizard";

        /// <summary>
        /// diWizard\arrows
        /// </summary>
        public const string DIWIZARD_ARROWS = @"diWizard\arrows";

        /// <summary>
        /// diWizard\home
        /// </summary>
        public const string DIWIZARD_HOME = @"diWizard\home";

        /// <summary>
        /// diWizard\shadows
        /// </summary>
        public const string DIWIZARD_SHADOWS = @"diWizard\shadows";

        /// <summary>
        /// diWizard\tabs
        /// </summary>
        public const string DIWIZARD_TABS = @"diWizard\tabs";

        /// <summary>
        /// diWizard\tool bars
        /// </summary>
        public const string DIWIZARD_TOOLBAR = @"diWizard\tool bars";

        /// <summary>
        /// diWizard\Navigation
        /// </summary>
        public const string DIWIZARD_NAVIGATION = @"diWizard\Navigation";

        /// <summary>
        /// TopPanel
        /// </summary>
        public const string TOP_PANEL = @"TopPanel";

        #endregion

        #region " -- Tabs --"

        private static string _INDICATOR_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "indicator_normal.png");
        /// <summary>
        /// Indicator_Normal.png
        /// </summary>
        public static string INDICATOR_NORMAL_TAB
        {
            get
            {
                return _INDICATOR_NORMAL_TAB;
            }
        }

        private static string _INDICATOR_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "indicator_selected.png");
        /// <summary>
        /// indicator_selected.png
        /// </summary>
        public static string INDICATOR_SELECTED_TAB
        {
            get
            {
                return _INDICATOR_SELECTED_TAB;
            }
        }

        private static string _INDICATOR_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "indicator_hover.png");
        /// <summary>
        /// indicator_hover.png
        /// </summary>
        public static string INDICATOR_HOVER_TAB
        {
            get
            {
                return _INDICATOR_HOVER_TAB;
            }
        }

        private static string _AREA_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "area_normal.png");
        /// <summary>
        /// area_normal.png
        /// </summary>
        public static string AREA_NORMAL_TAB
        {
            get
            {
                return _AREA_NORMAL_TAB;
            }
        }

        private static string _AREA_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "area_selected.png");
        /// <summary>
        /// area_selected.png
        /// </summary>
        public static string AREA_SELECTED_TAB
        {
            get
            {
                return _AREA_SELECTED_TAB;
            }
        }

        private static string _AREA_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "area_hover.png");
        /// <summary>
        /// area_hover.png
        /// </summary>
        public static string AREA_HOVER_TAB
        {
            get
            {
                return _AREA_HOVER_TAB;
            }
        }

        private static string _TIMEPERIOD_TAB = Path.Combine(DIWIZARD_TABS, "timeperiod_up.png");
        /// <summary>
        /// source_up.png
        /// </summary>
        public static string TIMEPERIOD_TAB
        {
            get
            {
                return _TIMEPERIOD_TAB;
            }
        }        

        private static string _SOURCE_TAB = Path.Combine(DIWIZARD_TABS, "source_up.png");
        /// <summary>
        /// Source_Normal.png
        /// </summary>
        public static string SOURCE_TAB
        {
            get
            {
                return _SOURCE_TAB;
            }
        }       

        private static string _DATABASE_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "database_normal.png");
        /// <summary>
        /// database_normal.png
        /// </summary>
        public static string DATABASE_NORMAL_TAB
        {
            get
            {
                return _DATABASE_NORMAL_TAB;
            }
        }

        private static string _DATABASE_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "database_selected.png");
        /// <summary>
        /// database_selected.png
        /// </summary> 
        public static string DATABASE_SELECTED_TAB
        {
            get
            {
                return _DATABASE_SELECTED_TAB;
            }
        }

        private static string _DATABASE_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "database_hover.png");
        /// <summary>
        /// database_hover.png
        /// </summary>
        public static string DATABASE_HOVER_TAB
        {
            get
            {
                return _DATABASE_HOVER_TAB;
            }
        }

        private static string _GALLERY_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "gallery_normal.png");
        /// <summary>
        /// gallery_normal.png
        /// </summary>
        public static string GALLERY_NORMAL_TAB
        {
            get
            {
                return _GALLERY_NORMAL_TAB;
            }
        }

        private static string _GALLERY_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "gallery_selected.png");
        /// <summary>
        /// gallery_selected.png
        /// </summary>
        public static string GALLERY_SELECTED_TAB
        {
            get
            {
                return _GALLERY_SELECTED_TAB;
            }
        }

        private static string _GALLERY_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "gallery_hover.png");
        /// <summary>
        /// Gallery_Hover.png
        /// </summary>
        public static string GALLERY_HOVER_TAB
        {
            get
            {
                return _GALLERY_HOVER_TAB;
            }
        }

        private static string _REPORT_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "report_normal.png");
        /// <summary>
        /// report_normal.png
        /// </summary>
        public static string REPORT_NORMAL_TAB
        {
            get
            {
                return _REPORT_NORMAL_TAB;
            }
        }

        private static string _REPORT_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "report_selected.png");
        /// <summary>
        /// report_selected.png
        /// </summary>
        public static string REPORT_SELECTED_TAB
        {
            get
            {
                return _REPORT_SELECTED_TAB;
            }
        }

        private static string _REPORT_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "report_hover.png");
        /// <summary>
        /// report_hover.png
        /// </summary>
        public static string REPORT_HOVER_TAB
        {
            get
            {
                return _REPORT_HOVER_TAB;
            }
        }


        private static string _VIEWDATA_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "viewdata_normal.png");
        /// <summary>
        /// viewdata_normal.png
        /// </summary>
        public static string VIEWDATA_NORMAL_TAB
        {
            get
            {
                return _VIEWDATA_NORMAL_TAB;
            }
        }

        private static string _VIEWDATA_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "viewdata_selected.png");
        /// <summary>
        /// viewdata_selected.png
        /// </summary>
        public static string VIEWDATA_SELECTED_TAB
        {
            get
            {
                return _VIEWDATA_SELECTED_TAB;
            }
        }

        private static string _VIEWDATA_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "viewdata_hover.png");
        /// <summary>
        /// viewdata_hover.png
        /// </summary>
        public static string VIEWDATA_HOVER_TAB
        {
            get
            {
                return _VIEWDATA_HOVER_TAB;
            }
        }

        private static string _TABLE_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "table_normal.png");
        /// <summary>
        /// table_normal.png
        /// </summary>
        public static string TABLE_NORMAL_TAB
        {
            get
            {
                return _TABLE_NORMAL_TAB;
            }
        }

        private static string _TABLE_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "table_selected.png");
        /// <summary>
        /// table_selected.png
        /// </summary>
        public static string TABLE_SELECTED_TAB
        {
            get
            {
                return _TABLE_SELECTED_TAB;
            }
        }

        private static string _TABLE_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "table_hover.png");
        /// <summary>
        /// table_hover.png
        /// </summary>
        public static string TABLE_HOVER_TAB
        {
            get
            {
                return _TABLE_HOVER_TAB;
            }
        }

        private static string _GRAPH_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "graph_normal.png");
        /// <summary>
        /// graph_normal.png
        /// </summary>
        public static string GRAPH_NORMAL_TAB
        {
            get
            {
                return _GRAPH_NORMAL_TAB;
            }
        }

        private static string _GRAPH_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "graph_selected.png");
        /// <summary>
        /// graph_selected.png
        /// </summary>
        public static string GRAPH_SELECTED_TAB
        {
            get
            {
                return _GRAPH_SELECTED_TAB;
            }
        }

        private static string _GRAPH_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "graph_hover.png");
        /// <summary>
        /// graph_hover.png
        /// </summary>
        public static string GRAPH_HOVER_TAB
        {
            get
            {
                return _GRAPH_HOVER_TAB;
            }
        }

        private static string _MAP_NORMAL_TAB = Path.Combine(DIWIZARD_TABS, "map_normal.png");
        /// <summary>
        /// map_normal.png
        /// </summary>
        public static string MAP_NORMAL_TAB
        {
            get
            {
                return _MAP_NORMAL_TAB;
            }
        }

        private static string _MAP_SELECTED_TAB = Path.Combine(DIWIZARD_TABS, "map_selected.png");
        /// <summary>
        /// map_selected.png
        /// </summary>
        public static string MAP_SELECTED_TAB
        {
            get
            {
                return _MAP_SELECTED_TAB;
            }
        }

        private static string _MAP_HOVER_TAB = Path.Combine(DIWIZARD_TABS, "map_hover.png");
        /// <summary>
        /// map_hover.png
        /// </summary>
        public static string MAP_HOVER_TAB
        {
            get
            {
                return _MAP_HOVER_TAB;
            }
        }

        private static string _TAB_BACKGROUND = Path.Combine(DIWIZARD_TABS, "tabs_bg.png");
        /// <summary>
        /// tabs_bg.png
        /// </summary>
        public static string TAB_BACKGROUND
        {
            get
            {
                return _TAB_BACKGROUND;
            }
        }

        private static string _AREA_NORMAL_TAB_1 = Path.Combine(DIWIZARD_TABS, "area_normal_1.png");
        /// <summary>
        /// area_normal_1.png
        /// </summary>
        public static string AREA_NORMAL_TAB_1
        {
            get
            {
                return _AREA_NORMAL_TAB_1;
            }
        }

        private static string _AREA_HOVER_TAB_1 = Path.Combine(DIWIZARD_TABS, "area_hover_1.png");
        /// <summary>
        /// area_hover_1.png
        /// </summary>
        public static string AREA_HOVER_TAB_1
        {
            get
            {
                return _AREA_HOVER_TAB_1;
            }
        }

        private static string _AREA_SELECTED_TAB_1 = Path.Combine(DIWIZARD_TABS, "area_selected_1.png");
        /// <summary>
        /// area_selected_1.png
        /// </summary>
        public static string AREA_SELECTED_TAB_1
        {
            get
            {
                return _AREA_SELECTED_TAB_1;
            }
        }

        private static string _INDICATOR_NORMAL_TAB_2 = Path.Combine(DIWIZARD_TABS, "indicator_normal_2.png");
        /// <summary>
        /// indicator_normal_2.png
        /// </summary>
        public static string INDICATOR_NORMAL_TAB_2
        {
            get
            {
                return _INDICATOR_NORMAL_TAB_2;
            }
        }

        private static string _INDICATOR_HOVER_TAB_2 = Path.Combine(DIWIZARD_TABS, "indicator_hover_2.png");
        /// <summary>
        /// indicator_hover_2.png
        /// </summary>
        public static string INDICATOR_HOVER_TAB_2
        {
            get
            {
                return _INDICATOR_HOVER_TAB_2;
            }
        }

        private static string _INDICATOR_SELECTED_TAB_2 = Path.Combine(DIWIZARD_TABS, "indicator_selected_2.png");
        /// <summary>
        /// indicator_selected_2.png
        /// </summary>
        public static string INDICATOR_SELECTED_TAB_2
        {
            get
            {
                return _INDICATOR_SELECTED_TAB_2;
            }
        }

        #endregion

        #region " -- Navigation -- "

        private static string _BACK_NORMAL_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_back_normal.png");
        /// <summary>
        /// button_back_normal.png  
        /// </summary>
        public static string BACK_NORMAL_IMAGE
        {
            get
            {
                return _BACK_NORMAL_IMAGE;
            }
        }

        private static string _BACK_SELECTED_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_back_selected.png");
        /// <summary>
        /// button_back_selected.png
        /// </summary>
        public static string BACK_SELECTED_IMAGE
        {
            get
            {
                return _BACK_SELECTED_IMAGE;
            }
        }

        private static string _BACK_HOVER_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_back_hover.png");
        /// <summary>
        /// button_back_hover.png
        /// </summary>
        public static string BACK_HOVER_IMAGE
        {
            get
            {
                return _BACK_HOVER_IMAGE;
            }
        }

        private static string _NEXT_NORMAL_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_next_normal.png");
        /// <summary>
        /// button_next_normal.png
        /// </summary>
        public static string NEXT_NORMAL_IMAGE
        {
            get
            {
                return _NEXT_NORMAL_IMAGE;
            }
        }

        private static string _NEXT_HOVER_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_next_hover.png");
        /// <summary>
        /// button_next_hover.png
        /// </summary>
        public static string NEXT_HOVER_IMAGE
        {
            get
            {
                return _NEXT_HOVER_IMAGE;
            }
        }

        private static string _NEXT_SELECTED_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_next_selected.png");
        /// <summary>
        /// button_next_selected.png
        /// </summary>
        public static string NEXT_SELECTED_IMAGE
        {
            get
            {
                return _NEXT_SELECTED_IMAGE;
            }
        }

        private static string _NEXT_DISABLED_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_next_disabled.png");
        /// <summary>
        /// button_back_disabled.png
        /// </summary>
        public static string NEXT_DISABLED_IMAGE 
        {
            get
            {
                return _NEXT_DISABLED_IMAGE;
            }
        }

        private static string _BACK_DISABLED_IMAGE = Path.Combine(DIWIZARD_NAVIGATION, "button_back_disabled.png");
        /// <summary>
        /// button_next_disabled.png
        /// </summary>
        public static string BACK_DISABLED_IMAGE
        {
            get
            {
                return _BACK_DISABLED_IMAGE;
            }
        }

        #endregion

        #region " -- Home -- "

        private static string _DATABASE_BACKGROUND = Path.Combine(DIWIZARD_HOME, "database_bg.png");
        /// <summary>
        /// database_bg.png
        /// </summary>
        public static string DATABASE_BACKGROUND 
        {
            get
            {
                return _DATABASE_BACKGROUND;
            }
        }

        private static string _GALLERY_BACKGROUND = Path.Combine(DIWIZARD_HOME, "gallery_bg.png");
        /// <summary>
        /// gallery_bg.png
        /// </summary>
        public static string GALLERY_BACKGROUND
        {
            get
            {
                return _GALLERY_BACKGROUND;
            }
        }

        private static string _REPORT_BACKGROUND = Path.Combine(DIWIZARD_HOME, "reports_bg.png");
        /// <summary>
        /// reports_bg.png
        /// </summary>
        public static string REPORT_BACKGROUND
        {
            get
            {
                return _REPORT_BACKGROUND;
            }
        }

        private static string _GALLERY_GUIDE = Path.Combine(DIWIZARD_HOME, "gallery_guide_img.png");
        /// <summary>
        /// gallery_guide_img.png
        /// </summary>
        public static string GALLERY_GUIDE
        {
            get
            {
                return _GALLERY_GUIDE;
            }
        }

        private static string _REPORT_GUIDE = Path.Combine(DIWIZARD_HOME, "report_guide_img.png");
        /// <summary>
        /// report_guide_img.png
        /// </summary>
        public static string REPORT_GUIDE
        {
            get
            {
                return _REPORT_GUIDE;
            }
        }

        private static string _DATABASE_GUIDE = Path.Combine(DIWIZARD_HOME, "database_guide_img.png");
        /// <summary>
        /// database_guide_img.png
        /// </summary>
        public static string DATABASE_GUIDE
        {
            get
            {
                return _DATABASE_GUIDE;
            }
        }

        private static string _HEADER_BG = Path.Combine(DIWIZARD_HOME, "header_bg.png");
        /// <summary>
        /// header_bg.png
        /// </summary>
        public static string HEADER_BG
        {
            get
            {
                return _HEADER_BG;
            }
        }

        private static string _HEADER_LOGO = Path.Combine(DIWIZARD_HOME, "header_logo.png");
        /// <summary>
        /// header_logo.png
        /// </summary>
        public static string HEADER_LOGO
        {
            get
            {
                return _HEADER_LOGO;
            }
        }

        private static string _DI_PREVIEW = Path.Combine(DIWIZARD_HOME, "div60_preview.png");
        /// <summary>
        /// div60_preview.png
        /// </summary>
        public static string DI_PREVIEW
        {
            get
            {
                return _DI_PREVIEW;
            }
        }

        private static string _HOME_PAGE_BG = Path.Combine(DIWIZARD_HOME, "home_body_bg.png");
        /// <summary>
        /// home_body_bg.png
        /// </summary>
        public static string HOME_PAGE_BG
        {
            get
            {
                return _HOME_PAGE_BG;
            }
        }

        private static string _BOTTOM_BAR_BG = Path.Combine(DIWIZARD_HOME, "bottom_bar_bg.png");
        /// <summary>
        /// bottom_bar_bg.png
        /// </summary>
        public static string BOTTOM_BAR_BG
        {
            get
            {
                return _BOTTOM_BAR_BG;
            }
        }

        private static string _BOTTOM_BAR_DIVIDER = Path.Combine(DIWIZARD_HOME, "bottom_bar_divider.png");
        /// <summary>
        /// bottom_bar_divider.png
        /// </summary>
        public static string BOTTOM_BAR_DIVIDER
        {
            get
            {
                return _BOTTOM_BAR_DIVIDER;
            }
        }

        private static string _CONTEXT_BOX_CURVE_BOTTOM = Path.Combine(DIWIZARD_HOME, "content_box_curve_bottom_left.png");
        /// <summary>
        /// content_box_curve_bottom_left.png
        /// </summary>
        public static string CONTEXT_BOX_CURVE_BOTTOM
        {
            get
            {
                return _CONTEXT_BOX_CURVE_BOTTOM;
            }
        }

        private static string _CONTEXT_BOX_CURVE_RIGHT = Path.Combine(DIWIZARD_HOME, "content_box_curve_bottom_right.png");
        /// <summary>
        /// content_box_curve_bottom_right.png
        /// </summary>
        public static string CONTEXT_BOX_CURVE_RIGHT
        {
            get
            {
                return _CONTEXT_BOX_CURVE_RIGHT;
            }
        }

        private static string _CONTEXT_BOX_CURVE_TOP_LEFT = Path.Combine(DIWIZARD_HOME, "content_box_curve_top_left.png");
        /// <summary>
        /// content_box_curve_top_left.png
        /// </summary>
        public static string CONTEXT_BOX_CURVE_TOP_LEFT
        {
            get
            {
                return _CONTEXT_BOX_CURVE_TOP_LEFT;
            }
        }

        private static string _CONTEXT_BOX_CURVE_TOP_RIGHT = Path.Combine(DIWIZARD_HOME, "content_box_curve_top_right.png");
        /// <summary>
        /// content_box_curve_bottom_right.png
        /// </summary>
        public static string CONTEXT_BOX_CURVE_TOP_RIGHT
        {
            get
            {
                return _CONTEXT_BOX_CURVE_TOP_RIGHT;
            }
        }

        private static string _HEADING_BAR_BG = Path.Combine(DIWIZARD_HOME, "headings_bar_bg.png");
        /// <summary>
        /// headings_bar_bg.png
        /// </summary>
        public static string HEADING_BAR_BG
        {
            get
            {
                return _HEADING_BAR_BG;
            }
        }

        private static string _HEADING_BAR_LEFT = Path.Combine(DIWIZARD_HOME, "headings_bar_left.png");
        /// <summary>
        /// headings_bar_left.png
        /// </summary>
        public static string HEADING_BAR_LEFT
        {
            get
            {
                return _HEADING_BAR_LEFT;
            }
        }

        private static string _HEADING_BAR_RIGHT = Path.Combine(DIWIZARD_HOME, "headings_bar_right.png");
        /// <summary>
        /// headings_bar_right.png
        /// </summary>
        public static string HEADING_BAR_RIGHT
        {
            get
            {
                return _HEADING_BAR_RIGHT;
            }
        }

        private static string _MAIN_CONTENT_BOTTOM_CENTER_BG = Path.Combine(DIWIZARD_HOME, "main_content_area_bottom_center_bg.png");
        /// <summary>
        /// main_content_area_bottom_center_bg.png
        /// </summary>
        public static string MAIN_CONTENT_BOTTOM_CENTER_BG
        {
            get
            {
                return _MAIN_CONTENT_BOTTOM_CENTER_BG;
            }
        }

        private static string _MAIN_CONTENT_BOTTOM_LEFT_CENTER_CURVE = Path.Combine(DIWIZARD_HOME, "main_content_area_bottom_left_curve.png");
        /// <summary>
        /// main_content_area_bottom_left_curve.png
        /// </summary>
        public static string MAIN_CONTENT_BOTTOM_LEFT_CENTER_CURVE
        {
            get
            {
                return _MAIN_CONTENT_BOTTOM_LEFT_CENTER_CURVE;
            }
        }

        private static string _MAIN_CONTENT_BOTTOM_RIGHT_CURVE = Path.Combine(DIWIZARD_HOME, "main_content_area_bottom_right_curve.png");
        /// <summary>
        /// main_content_area_bottom_right_curve.png
        /// </summary>
        public static string MAIN_CONTENT_BOTTOM_RIGHT_CURVE
        {
            get
            {
                return _MAIN_CONTENT_BOTTOM_RIGHT_CURVE;
            }
        }

        private static string _SEARCH_BG_CENTER = Path.Combine(DIWIZARD_HOME, "search_bg_center.png");
        /// <summary>
        /// search_bg_center.png
        /// </summary>
        public static string SEARCH_BG_CENTER
        {
            get
            {
                return _SEARCH_BG_CENTER;
            }
        }

        private static string _TOUR_CONTENT_BG = Path.Combine(DIWIZARD_HOME, "tour_content_area_bg.png");
        /// <summary>
        /// tour_content_area_bg.png
        /// </summary>
        public static string TOUR_CONTENT_BG
        {
            get
            {
                return _TOUR_CONTENT_BG;
            }
        }

        private static string _TOUR_CONTENT_CURVED_BOTTOM = Path.Combine(DIWIZARD_HOME, "tour_content_area_curved_bottom.png");
        /// <summary>
        /// tour_content_area_curved_bottom.png
        /// </summary>
        public static string TOUR_CONTENT_CURVED_BOTTOM
        {
            get
            {
                return _TOUR_CONTENT_CURVED_BOTTOM;
            }
        }


        private static string _HOME_GALLERY_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "gallery_normal.png");
        /// <summary>
        /// gallery_normal.png
        /// </summary>
        public static string HOME_GALLERY_NORMAL_BUTTON
        {
            get
            {
                return _HOME_GALLERY_NORMAL_BUTTON;
            }
        }

        private static string _HOME_GALLERY_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "gallery_hover.png");
        /// <summary>
        /// gallery_hover.png
        /// </summary>
        public static string HOME_GALLERY_HOVER_BUTTON
        {
            get
            {
                return _HOME_GALLERY_HOVER_BUTTON;
            }
        }


        private static string _HOME_DATABASE_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "data_normal.png");
        /// <summary>
        /// data_normal.png
        /// </summary>
        public static string HOME_DATABASE_NORMAL_BUTTON
        {
            get
            {
                return _HOME_DATABASE_NORMAL_BUTTON;
            }
        }

        private static string _HOME_DATABASE_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "data_hover.png");
        /// <summary>
        /// data_hover.png
        /// </summary>
        public static string HOME_DATABASE_HOVER_BUTTON
        {
            get
            {
                return _HOME_DATABASE_HOVER_BUTTON;
            }
        }

        private static string _HOME_REPORT_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "report_normal.png");
        /// <summary>
        /// report_normal.png
        /// </summary>
        public static string HOME_REPORT_NORMAL_BUTTON
        {
            get
            {
                return _HOME_REPORT_NORMAL_BUTTON;
            }
        }

        private static string _HOME_REPORT_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "report_hover.png");
        /// <summary>
        /// report_hover.png
        /// </summary>
        public static string HOME_REPORT_HOVER_BUTTON
        {
            get
            {
                return _HOME_REPORT_HOVER_BUTTON;
            }
        }

        private static string _HOME_SELECTBY_INIDICATOR_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "indicator_button_normal.png");
        /// <summary>
        /// indicator_button_normal.png
        /// </summary>
        public static string HOME_SELECTBY_INIDICATOR_NORMAL_BUTTON
        {
            get
            {
                return _HOME_SELECTBY_INIDICATOR_NORMAL_BUTTON;
            }
        }

        private static string _HOME_SELECTBY_INIDICATOR_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "indicator_button_hover.png");
        /// <summary>
        /// indicator_button_hover.png
        /// </summary>
        public static string HOME_SELECTBY_INIDICATOR_HOVER_BUTTON
        {
            get
            {
                return _HOME_SELECTBY_INIDICATOR_HOVER_BUTTON;
            }
        }

        private static string _HOME_SELECTBY_AREA_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "area_button_normal.png");
        /// <summary>
        /// area_button_normal.png
        /// </summary>
        public static string HOME_SELECTBY_AREA_NORMAL_BUTTON
        {
            get
            {
                return _HOME_SELECTBY_AREA_NORMAL_BUTTON;
            }
        }

        private static string _HOME_SELECTBY_AREA_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "area_button_hover.png");
        /// <summary>
        /// area_button_hover.png
        /// </summary>
        public static string HOME_SELECTBY_AREA_HOVER_BUTTON
        {
            get
            {
                return _HOME_SELECTBY_AREA_HOVER_BUTTON;
            }
        }

        private static string _HOME_NEXT_ARROW_NORMAL_BUTTON = Path.Combine(DIWIZARD_HOME, "next_arrow_smal_normal.png");
        /// <summary>
        /// next_arrow_smal_normal.png
        /// </summary>
        public static string HOME_NEXT_ARROW_NORMAL_BUTTON
        {
            get
            {
                return _HOME_NEXT_ARROW_NORMAL_BUTTON;
            }
        }

        private static string _HOME_NEXT_ARROW_HOVER_BUTTON = Path.Combine(DIWIZARD_HOME, "next_arrow_small_hover.png");
        /// <summary>
        /// next_arrow_small_hover.png
        /// </summary>
        public static string HOME_NEXT_ARROW_HOVER_BUTTON
        {
            get
            {
                return _HOME_NEXT_ARROW_HOVER_BUTTON;
            }
        }

        private static string _FACT_YOU_DECIDE = Path.Combine(DIWIZARD_HOME, "header_facts_you_decide.png");
        /// <summary>
        /// header_facts_you_decide.png
        /// </summary>
        public static string FACT_YOU_DECIDE
        {
            get
            {
                return _FACT_YOU_DECIDE;
            }
        }

        private static string _TOUR_CONTENT_CURVE_BOTTOM = Path.Combine(DIWIZARD_HOME, "tour_content_area_curved_bottom_with_di_image.png");
        /// <summary>
        /// tour_content_area_curved_bottom_with_di_image.png
        /// </summary>
        public static string TOUR_CONTENT_CURVE_BOTTOM
        {
            get
            {
                return _TOUR_CONTENT_CURVE_BOTTOM;
            }
        }


        #endregion

        #region " -- Toolbars -- "

        private static string _AREA_TOOLBAR = Path.Combine(DIWIZARD_TOOLBAR, "area.png");
        /// <summary>
        /// area.png
        /// </summary>
        public static string AREA_TOOLBAR
        {
            get
            {
                return _AREA_TOOLBAR;
            }
        }

        private static string _INDICATOR_TOOLBAR = Path.Combine(DIWIZARD_TOOLBAR, "indicator.png");
        /// <summary>
        /// indicator.png
        /// </summary>
        public static string INDICATOR_TOOLBAR
        {
            get
            {
                return _INDICATOR_TOOLBAR;
            }
        }

        private static string _SOURCE_TOOLBAR = Path.Combine(DIWIZARD_TOOLBAR, "source.png");
        /// <summary>
        /// source.png
        /// </summary>
        public static string SOURCE_TOOLBAR 
        {
            get
            {
                return _SOURCE_TOOLBAR;
            }
        }

        private static string _TIMEPERIOD_TOOLBAR = Path.Combine(DIWIZARD_TOOLBAR, "timeperiod.png");
        /// <summary>
        /// timeperiod.png
        /// </summary>
        public static string TIMEPERIOD_TOOLBAR
        {
            get
            {
                return _TIMEPERIOD_TOOLBAR;
            }
        }

        #endregion

        #region " -- SHADOW -- "

        private static string _SHADOW_BOTTOM = Path.Combine(DIWIZARD_SHADOWS, "shadow_bottom.png");
        /// <summary>
        /// shadow_bottom.png
        /// </summary>
        public static string SHADOW_BOTTOM
        {
            get
            {
                return _SHADOW_BOTTOM;
            }
        }

        private static string _SHADOW_LEFT = Path.Combine(DIWIZARD_SHADOWS, "shadow_left.png");
        /// <summary>
        /// shadow_left.png
        /// </summary>
        public static string SHADOW_LEFT
        {
            get
            {
                return _SHADOW_LEFT;
            }
        }

        private static string _SHADOW_RIGHT = Path.Combine(DIWIZARD_SHADOWS, "shadow_right.png");
        /// <summary>
        /// shadow_right.png
        /// </summary>
        public static string SHADOW_RIGHT
        {
            get
            {
                return _SHADOW_RIGHT;
            }
        }

        private static string _SHADOW_TOP = Path.Combine(DIWIZARD_SHADOWS, "shadow_top.png");
        /// <summary>
        /// shadow_top.png
        /// </summary>
        public static string SHADOW_TOP
        {
            get
            {
                return _SHADOW_TOP;
            }
        }

        private static string _SHADOW_BOTTOM_LEFT = Path.Combine(DIWIZARD_SHADOWS, "shadow_bottom_left.png");
        /// <summary>
        /// shadow_bottom_left.png
        /// </summary>
        public static string SHADOW_BOTTOM_LEFT
        {
            get
            {
                return _SHADOW_BOTTOM_LEFT;
            }
        }

        private static string _SHADOW_BOTTOM_RIGHT = Path.Combine(DIWIZARD_SHADOWS, "shadow_bottom_right.png");
        /// <summary>
        /// shadow_bottom_right.png
        /// </summary>
        public static string SHADOW_BOTTOM_RIGHT
        {
            get
            {
                return _SHADOW_BOTTOM_RIGHT;
            }
        }

        private static string _SHADOW_LEFT_TOP = Path.Combine(DIWIZARD_SHADOWS, "shadow_left_top.png");
        /// <summary>
        /// shadow_left_top.png
        /// </summary>
        public static string SHADOW_LEFT_TOP
        {
            get
            {
                return _SHADOW_LEFT_TOP;
            }
        }

        private static string _SHADOW_RIGHT_TOP = Path.Combine(DIWIZARD_SHADOWS, "shadow_right_top.png");
        /// <summary>
        /// shadow_right_top.png
        /// </summary>
        public static string SHADOW_RIGHT_TOP
        {
            get
            {
                return _SHADOW_RIGHT_TOP;
            }
        }

        #endregion

        #region " -- ARROWS -- "

        private static string _ARROW_GREY_DOWN = Path.Combine(DIWIZARD_ARROWS, "arrow_grey_dw.png");
        /// <summary>
        /// arrow_grey_dw.png
        /// </summary>
        public static string ARROW_GREY_DOWN
        {
            get
            {
                return _ARROW_GREY_DOWN;
            }
        }

        private static string _ARROW_GREY_UP = Path.Combine(DIWIZARD_ARROWS, "arrow_grey_up.png");
        /// <summary>
        /// arrow_grey_up.png
        /// </summary>
        public static string ARROW_GREY_UP
        {
            get
            {
                return _ARROW_GREY_UP;
            }
        }

        //private static string _ARROW_WHITE_DOWN = Path.Combine(DIWIZARD_ARROWS, "arrow_white_dw.png");
        ///// <summary>
        ///// arrow_white_dw.png
        ///// </summary>
        //public static string ARROW_WHITE_DOWN 
        //{
        //    get
        //    {
        //        return _ARROW_WHITE_DOWN;
        //    }
        //}

        //private static string _ARROW_WHITE_UP = Path.Combine(DIWIZARD_ARROWS, "arrow_white_up.png");
        ///// <summary>
        ///// arrow_white_up.png
        ///// </summary>
        //public static string ARROW_WHITE_UP
        //{
        //    get
        //    {
        //        return _ARROW_WHITE_UP;
        //    }
        //}

        #endregion

        #region " -- IMAGES -- "

        private static string _FACTS_YOU_DECIDE = Path.Combine(DIWIZARD, "facts.you_decide.png");
        /// <summary>
        /// facts.you_decide.png
        /// </summary>
        public static string FACTS_YOU_DECIDE
        {
            get
            {
                return _FACTS_YOU_DECIDE;
            }
        }

        private static string _SELECTION_BACKGROUND = Path.Combine(DIWIZARD, "view_selectionbar_bg.png");
        /// <summary>
        /// view_selectionbar_bg.png
        /// </summary>
        public static string SELECTION_BACKGROUND
        {
            get
            {
                return _SELECTION_BACKGROUND;
            }
        }       

        #endregion

        #endregion
    }

}
