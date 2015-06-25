using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{
    /// <summary>
    /// Constants of Images used in DI Data Wizard.
    /// </summary>
    public static class ImageFileName
    {
        #region " -- Constants / Read only Properties -- "

        /// <summary>
        /// 99
        /// </summary>
        public const int IMAGE_HEIGHT = 90;

        /// <summary>
        /// diWizard
        /// </summary>
        public const string DIWIXARD_FOLDER = @"diWizard\Panel";

        /// <summary>
        /// Icons
        /// </summary>
        public const string ICON_FOLDER = @"diWizard\Icons";

        #region " -- Icons --"

        private static string _GALLERY_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Gallery_Normal.png");
        /// <summary>
        /// Gallery_Normal.png
        /// </summary>
        public static string GALLERY_NORMAL_ICON
        {
            get
            {
                return _GALLERY_NORMAL_ICON;
            }
        }

        private static string _GALLERY_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Gallery_Selected.png");
        /// <summary>
        /// Gallery_Selected.png
        /// </summary>
        public static string GALLERY_SELECTED_ICON
        {
            get
            {
                return _GALLERY_SELECTED_ICON;
            }
        }

        private static string _GALLERY_HOVER_ICON = Path.Combine(ICON_FOLDER, "Gallery_Hover.png");
        /// <summary>
        /// Gallery_Hover.png
        /// </summary>
        public static string GALLERY_HOVER_ICON
        {
            get
            {
                return _GALLERY_HOVER_ICON;
            }
        }

        private static string _DATABASE_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Database_Normal.png");
        /// <summary>
        /// Database_Normal.png
        /// </summary>
        public static string DATABASE_NORMAL_ICON
        {
            get
            {
                return _DATABASE_NORMAL_ICON;
            }
        }

        private static string _DATABASE_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Database_Selected.png");
        /// <summary>
        /// Database_Selected.png
        /// </summary> 
        public static string DATABASE_SELECTED_ICON
        {
            get
            {
                return _DATABASE_SELECTED_ICON;
            }
        }

        private static string _DATABASE_HOVER_ICON = Path.Combine(ICON_FOLDER, "Database_Hover.png");
        /// <summary>
        /// Database_Hover.png
        /// </summary>
        public static string DATABASE_HOVER_ICON
        {
            get
            {
                return _DATABASE_HOVER_ICON;
            }
        }

        private static string _INDICATOR_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Indicator_Normal.png");
        /// <summary>
        /// Indicator_Normal.png
        /// </summary>
        public static string INDICATOR_NORMAL_ICON
        {
            get
            {
                return _INDICATOR_NORMAL_ICON;
            }
        }

        private static string _INDICATOR_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Indicator_Selected.png");
        /// <summary>
        /// Indicator_Selected.png
        /// </summary>
        public static string INDICATOR_SELECTED_ICON
        {
            get
            {
                return _INDICATOR_SELECTED_ICON;
            }
        }

        private static string _INDICATOR_HOVER_ICON = Path.Combine(ICON_FOLDER, "Indicator_Hover.png");
        /// <summary>
        /// Indicator_Hover.png
        /// </summary>
        public static string INDICATOR_HOVER_ICON
        {
            get
            {
                return _INDICATOR_HOVER_ICON;
            }
        }

        private static string _AREA_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Area_Normal.png");
        /// <summary>
        /// Indicator_Hover.png
        /// </summary>
        public static string AREA_NORMAL_ICON
        {
            get
            {
                return _AREA_NORMAL_ICON;
            }
        }

        private static string _AREA_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Area_Selected.png");
        /// <summary>
        /// Area_Selected.png
        /// </summary>
        public static string AREA_SELECTED_ICON
        {
            get
            {
                return _AREA_SELECTED_ICON;
            }
        }

        private static string _AREA_HOVER_ICON = Path.Combine(ICON_FOLDER, "Area_Hover.png");
        /// <summary>
        /// Area_Hover.png
        /// </summary>
        public static string AREA_HOVER_ICON
        {
            get
            {
                return _AREA_HOVER_ICON;
            }
        }

        private static string _TIMEPERIOD_NORMAL_ICON = Path.Combine(ICON_FOLDER, "TimePeriod_Normal.png");
        /// <summary>
        /// TimePeriod_Normal.png
        /// </summary>
        public static string TIMEPERIOD_NORMAL_ICON
        {
            get
            {
                return _TIMEPERIOD_NORMAL_ICON;
            }
        }

        private static string _TIMEPERIOD_SELECTED_ICON = Path.Combine(ICON_FOLDER, "TimePeriod_Selected.png");
        /// <summary>
        /// TimePeriod_Selected.png
        /// </summary>
        public static string TIMEPERIOD_SELECTED_ICON
        {
            get
            {
                return _TIMEPERIOD_SELECTED_ICON;
            }
        }

        private static string _TIMEPERIOD_HOVER_ICON = Path.Combine(ICON_FOLDER, "TimePeriod_Hover.png");
        /// <summary>
        /// TimePeriod_Hover.png
        /// </summary>
        public static string TIMEPERIOD_HOVER_ICON
        {
            get
            {
                return _TIMEPERIOD_HOVER_ICON;
            }
        }

        private static string _SOURCE_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Source_Normal.png");
        /// <summary>
        /// Source_Normal.png
        /// </summary>
        public static string SOURCE_NORMAL_ICON
        {
            get
            {
                return _SOURCE_NORMAL_ICON;
            }
        }

        private static string _SOURCE_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Source_Selected.png");
        /// <summary>
        /// Source_Selected.png
        /// </summary>
        public static string SOURCE_SELECTED_ICON
        {
            get
            {
                return _SOURCE_SELECTED_ICON;
            }
        }

        private static string _SOURCE_HOVER_ICON = Path.Combine(ICON_FOLDER, "Source_Hover.png");
        /// <summary>
        /// Source_Hover.png
        /// </summary>
        public static string SOURCE_HOVER_ICON
        {
            get
            {
                return _SOURCE_HOVER_ICON;
            }
        }

        private static string _RESET_NORMAL_IMAGE = Path.Combine(ICON_FOLDER, "Reset_normal.png");
        /// <summary>
        /// Reset_normal.png
        /// </summary>
        public static string RESET_NORMAL_IMAGE
        {
            get
            {
                return _RESET_NORMAL_IMAGE;
            }
        }

        private static string _RESET_SELECTED_IMAGE = Path.Combine(ICON_FOLDER, "Reset_selected.png");
        /// <summary>
        /// Reset_selected.png
        /// </summary>
        public static string RESET_SELECTED_IMAGE
        {
            get
            {
                return _RESET_SELECTED_IMAGE;
            }
        }

        private static string _RESET_HOVER_IMAGE = Path.Combine(ICON_FOLDER, "Reset_hover.png");
        /// <summary>
        /// Reset_hover.png
        /// </summary>
        public static string RESET_HOVER_IMAGE
        {
            get
            {
                return _RESET_HOVER_IMAGE;
            }
        }

        private static string _RESET_DISABLED_IMAGE = Path.Combine(ICON_FOLDER, "Reset_disabled.png");
        /// <summary>
        /// Reset_disabled.png
        /// </summary>
        public static string RESET_DISABLED_IMAGE
        {
            get
            {
                return _RESET_DISABLED_IMAGE;
            }
        }

        private static string _BACK_NORMAL_IMAGE = Path.Combine(ICON_FOLDER, "Back_normal.png");
        /// <summary>
        /// Back_normal.png
        /// </summary>
        public static string BACK_NORMAL_IMAGE
        {
            get
            {
                return _BACK_NORMAL_IMAGE;
            }
        }

        private static string _BACK_SELECTED_IMAGE = Path.Combine(ICON_FOLDER, "Back_selected.png");
        /// <summary>
        /// Back_selected.png
        /// </summary>
        public static string BACK_SELECTED_IMAGE
        {
            get
            {
                return _BACK_SELECTED_IMAGE;
            }
        }

        private static string _BACK_HOVER_IMAGE = Path.Combine(ICON_FOLDER, "Back_hover.png");
        /// <summary>
        /// Back_hover.png
        /// </summary>
        public static string BACK_HOVER_IMAGE
        {
            get
            {
                return _BACK_HOVER_IMAGE;
            }
        }

        private static string _BACK_DISABLED_IMAGE = Path.Combine(ICON_FOLDER, "Back_disabled.png");
        /// <summary>
        /// Back_disabled.png
        /// </summary>
        public static string BACK_DISABLED_IMAGE
        {
            get
            {
                return _BACK_DISABLED_IMAGE;
            }
        }

        private static string _NEXT_DISABLED_IMAGE = Path.Combine(ICON_FOLDER, "Next_Disabled.png");
        /// <summary>
        /// Next_Disabled.png
        /// </summary>
        public static string NEXT_DISABLED_IMAGE
        {
            get
            {
                return _NEXT_DISABLED_IMAGE;
            }
        }

        private static string _NEXT_NORMAL_IMAGE = Path.Combine(ICON_FOLDER, "Next_normal.png");
        /// <summary>
        /// Next_normal.png
        /// </summary>
        public static string NEXT_NORMAL_IMAGE
        {
            get
            {
                return _NEXT_NORMAL_IMAGE;
            }
        }

        private static string _NEXT_HOVER_IMAGE = Path.Combine(ICON_FOLDER, "Next_hover.png");
        /// <summary>
        /// Next_hover.png
        /// </summary>
        public static string NEXT_HOVER_IMAGE
        {
            get
            {
                return _NEXT_HOVER_IMAGE;
            }
        }

        private static string _NEXT_SELECTED_IMAGE = Path.Combine(ICON_FOLDER, "Next_selected.png");
        /// <summary>
        /// Next_selected.png
        /// </summary>
        public static string NEXT_SELECTED_IMAGE
        {
            get
            {
                return _NEXT_SELECTED_IMAGE;
            }
        }

        private static string _DATAVIEW_NORMAL_IMAGE = Path.Combine(ICON_FOLDER, "Dataview_normal.png");
        /// <summary>
        /// Dataview_normal.png
        /// </summary>
        public static string DATAVIEW_NORMAL_IMAGE
        {
            get
            {
                return _DATAVIEW_NORMAL_IMAGE;
            }
        }

        private static string _DATAVIEW_SELECTED_IMAGE = Path.Combine(ICON_FOLDER, "Dataview_selected.png");
        /// <summary>
        /// Dataview_selected.png
        /// </summary>
        public static string DATAVIEW_SELECTED_IMAGE
        {
            get
            {
                return _DATAVIEW_SELECTED_IMAGE;
            }
        }

        private static string _DATAVIEW_HOVER_IMAGE = Path.Combine(ICON_FOLDER, "Dataview_hover.png");
        /// <summary>
        /// Dataview_hover.png
        /// </summary>
        public static string DATAVIEW_HOVER_IMAGE
        {
            get
            {
                return _DATAVIEW_HOVER_IMAGE;
            }
        }

        private static string _DATAVIEW_DISABLED_IMAGE = Path.Combine(ICON_FOLDER, "Dataview_disabled.png");
        /// <summary>
        /// Dataview_disabled.png
        /// </summary>
        public static string DATAVIEW_DISABLED_IMAGE
        {
            get
            {
                return _DATAVIEW_DISABLED_IMAGE;
            }
        }

        private static string _LINK_NORMAL_ICON = Path.Combine(ICON_FOLDER, "LinkIcon_Nomal.png");
        /// <summary>
        /// LinkIcon_Nomal.png
        /// </summary>
        public static string LINK_NORMAL_ICON
        {
            get
            {
                return _LINK_NORMAL_ICON;
            }
        }

        private static string _LINK_SELECTED_ICON = Path.Combine(ICON_FOLDER, "LinkIcon_Selected.png");
        /// <summary>
        /// LinkIcon_Selected.png
        /// </summary>
        public static string LINK_SELECTED_ICON
        {
            get
            {
                return _LINK_SELECTED_ICON;
            }
        }

        private static string _LINK_HOVER_ICON = Path.Combine(ICON_FOLDER, "LinklIcon_Hover.png");
        /// <summary>
        /// LinklIcon_Hover.png
        /// </summary>
        public static string LINK_HOVER_ICON
        {
            get
            {
                return _LINK_HOVER_ICON;
            }
        }

        private static string _CANCEL_NORMAL_ICON = Path.Combine(ICON_FOLDER, "Cancel_Normal.png");
        /// <summary>
        /// Cancel_Normal.png
        /// </summary>
        public static string CANCEL_NORMAL_ICON
        {
            get
            {
                return _CANCEL_NORMAL_ICON;
            }
        }

        private static string _CANCEL_HOVER_ICON = Path.Combine(ICON_FOLDER, "Cancel_Hover.png");
        /// <summary>
        /// Cancel_Hover.png
        /// </summary>
        public static string CANCEL_HOVER_ICON
        {
            get
            {
                return _CANCEL_HOVER_ICON;
            }
        }

        private static string _CANCEL_SELECTED_ICON = Path.Combine(ICON_FOLDER, "Cancel_Selected.png");
        /// <summary>
        /// Cancel_Hover.png
        /// </summary>
        public static string CANCEL_SELECTED_ICON
        {
            get
            {
                return _CANCEL_SELECTED_ICON;
            }
        }

        #endregion

        #region " -- Panels -- "

        private static string _PANEL1_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel1_Normal.png");
        /// <summary>
        /// Panel1_Normal.png
        /// </summary>
        public static string PANEL1_NORMAL_IMAGE
        {
            get
            {
                return _PANEL1_NORMAL_IMAGE;
            }
        }

        private static string _PANEL1_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel1_Selected.png");
        /// <summary>
        /// Panel1_Selected.png
        /// </summary>
        public static string PANEL1_SELECTED_IMAGE
        {
            get
            {
                return _PANEL1_SELECTED_IMAGE;
            }
        }

        private static string _PANEL1_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel1_Hover.png");
        /// <summary>
        /// Panel1_Hover.png
        /// </summary>
        public static string PANEL1_HOVER_IMAGE
        {
            get
            {
                return _PANEL1_HOVER_IMAGE;
            }
        }

        private static string _PANEL2_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel2_Normal.png");
        /// <summary>
        /// Panel2_Normal.png
        /// </summary>
        public static string PANEL2_NORMAL_IMAGE
        {
            get
            {
                return _PANEL2_NORMAL_IMAGE;
            }
        }

        private static string _PANEL2_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel2_Selected.png");
        /// <summary>
        /// Panel2_Selected.png
        /// </summary>
        public static string PANEL2_SELECTED_IMAGE
        {
            get
            {
                return _PANEL2_SELECTED_IMAGE;
            }
        }

        private static string _PANEL2_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel2_Hover.png");
        /// <summary>
        /// Panel2_Hover.png
        /// </summary>
        public static string PANEL2_HOVER_IMAGE
        {
            get
            {
                return _PANEL2_HOVER_IMAGE;
            }
        }

        private static string _PANEL3_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel3_Normal.png");
        /// <summary>
        /// Panel3_Normal.png
        /// </summary>
        public static string PANEL3_NORMAL_IMAGE
        {
            get
            {
                return _PANEL3_NORMAL_IMAGE;
            }
        }

        private static string _PANEL3_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel3_Selected.png");
        /// <summary>
        /// Panel3_Selected.png
        /// </summary>
        public static string PANEL3_SELECTED_IMAGE
        {
            get
            {
                return _PANEL3_SELECTED_IMAGE;
            }
        }

        private static string _PANEL3_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel3_Hover.png");
        /// <summary>
        /// Panel3_Hover.png
        /// </summary>
        public static string PANEL3_HOVER_IMAGE
        {
            get
            {
                return _PANEL3_HOVER_IMAGE;
            }
        }

        private static string _PANEL4_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel4_Normal.png");
        /// <summary>
        /// Panel4_Normal.png
        /// </summary>
        public static string PANEL4_NORMAL_IMAGE
        {
            get
            {
                return _PANEL4_NORMAL_IMAGE;
            }
        }

        private static string _PANEL4_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel4_Selected.png");
        /// <summary>
        /// Panel4_Selected.png
        /// </summary>
        public static string Panel4_SELECTED_IMAGE
        {
            get
            {
                return _PANEL4_SELECTED_IMAGE;
            }
        }

        private static string _Panel4_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel4_Hover.png");
        /// <summary>
        /// Panel4_Hover.png
        /// </summary>
        public static string Panel4_HOVER_IMAGE
        {
            get
            {
                return _Panel4_HOVER_IMAGE;
            }
        }

        private static string _Panel5_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel5_Normal.png");
        /// <summary>
        /// Panel5_Normal.png
        /// </summary>
        public static string Panel5_NORMAL_IMAGE
        {
            get
            {
                return _Panel5_NORMAL_IMAGE;
            }
        }

        private static string _Panel5_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel5_Selected.png");
        /// <summary>
        /// Panel5_Selected.png
        /// </summary>
        public static string Panel5_SELECTED_IMAGE
        {
            get
            {
                return _Panel5_SELECTED_IMAGE;
            }
        }

        private static string _Panel5_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel5_Hover.png");
        /// <summary>
        /// Panel5_Hover.png
        /// </summary>
        public static string Panel5_HOVER_IMAGE
        {
            get
            {
                return _Panel5_HOVER_IMAGE;
            }
        }

        private static string _Panel6_NORMAL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel6_Normal.png");
        /// <summary>
        /// Panel6_Normal.png
        /// </summary>
        public static string Panel6_NORMAL_IMAGE
        {
            get
            {
                return _Panel6_NORMAL_IMAGE;
            }
        }

        private static string _Panel6_SELECTED_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel6_Selected.png");
        /// <summary>
        /// Panel6_Selected.png
        /// </summary>
        public static string Panel6_SELECTED_IMAGE
        {
            get
            {
                return _Panel6_SELECTED_IMAGE;
            }
        }

        private static string _Panel6_HOVER_IMAGE = Path.Combine(DIWIXARD_FOLDER, "Panel6_Hover.png");
        /// <summary>
        /// SourcePanel_Hover.png
        /// </summary>
        public static string Panel6_HOVER_IMAGE
        {
            get
            {
                return _Panel6_HOVER_IMAGE;
            }
        }

        private static string _DATABASE_PANEL_IMAGE = Path.Combine(DIWIXARD_FOLDER, "bg_bottom.png");
        /// <summary>
        /// bg_middle.png
        /// </summary>
        public static string DATABASE_PANEL_IMAGE
        {
            get
            {
                return _DATABASE_PANEL_IMAGE;
            }
        }

        #endregion

        #endregion
    }

}
