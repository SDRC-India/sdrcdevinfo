using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Utility;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;
using System.Collections;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.UI.UserPreference
{
    /// <summary>
    /// Represents UI pref which were previously stored in DI5UI.xml
    /// </summary>
    public class Adaptation
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
        /// </summary>
        public Adaptation()
        {

        }

        #endregion

        #region " -- Constants -- "

        /// <summary>
        /// Adaptation.xml
        /// </summary>
        public const string ADAPTATION_FILE_NAME = "Adaptation.xml";

        #endregion

        #region " -- Properties -- "

        private GeneralSettings _General = new GeneralSettings();
        /// <summary>
        /// Gets or sets the general settings.
        /// </summary>
        public GeneralSettings General
        {
            get
            {
                return this._General;
            }
            set
            {
                this._General = value;
            }
        }

        private ApplicationLevelSetting _ApplicationLevel = new ApplicationLevelSetting();
        /// <summary>
        /// Gets or sets the application level settings.
        /// </summary>
        public ApplicationLevelSetting ApplicationLevel
        {
            get
            {
                return this._ApplicationLevel;
            }
            set
            {
                this._ApplicationLevel = value;
            }
        }

        private CommandButtonsSettings _CommandButtons = new CommandButtonsSettings();
        /// <summary>
        /// Gets or sets the command button settings.
        /// </summary>
        public CommandButtonsSettings CommandButtons
        {
            get
            {
                return this._CommandButtons;
            }
            set
            {
                this._CommandButtons = value;
            }
        }

        private GraphicsSettings _Graphics = new GraphicsSettings();
        /// <summary>
        /// Gets or sets the graphics settings.
        /// </summary>
        public GraphicsSettings Graphics
        {
            get
            {
                return this._Graphics;
            }
            set
            {
                this._Graphics = value;
            }
        }

        private DirectorySettings _DirectoryLocation = new DirectorySettings();
        /// <summary>
        /// Gets or sets the director location settings.
        /// </summary>
        public DirectorySettings DirectoryLocation
        {
            get
            {
                return this._DirectoryLocation;
            }
            set
            {
                this._DirectoryLocation = value;
            }
        }

        private ContentSettings _Content = new ContentSettings();
        /// <summary>
        /// Gets or sets the file name settings.
        /// </summary>
        public ContentSettings Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                this._Content = value;
            }
        }


        private LiveUpdateSetting _LiveUpdate = new LiveUpdateSetting();
        /// <summary>
        /// Gets or sets the FTP settings.
        /// </summary>
        public LiveUpdateSetting LiveUpdate
        {
            get
            {
                return this._LiveUpdate;
            }
            set
            {
                this._LiveUpdate = value;
            }
        }

        private SMTPSettings _SMTP = new SMTPSettings();
        /// <summary>
        /// Gets or sets the SMTP setting
        /// </summary>
        /// <remarks>
        /// Used only in web application for sending mail for forgot password in login mode
        /// </remarks>
        public SMTPSettings SMTP
        {
            get { return this._SMTP; }
            set { this._SMTP = value; }
        }

        private PresentationSetting _Presentation = new PresentationSetting();
        /// <summary>
        /// Gets or sets the presentation settings.
        /// </summary>
        public PresentationSetting Presentation
        {
            get
            {
                return this._Presentation;
            }
            set
            {
                this._Presentation = value;
            }
        }

        private ReportsSetting _Reports = new ReportsSetting();
        /// <summary>
        /// Gets or sets the reports settings.
        /// </summary>
        public ReportsSetting Reports
        {
            get
            {
                return this._Reports;
            }
            set
            {
                this._Reports = value;
            }
        }

        private HomePageSetting _HomePage = new HomePageSetting();
        /// <summary>
        /// Gets or sets the home page settings
        /// </summary>
        public HomePageSetting HomePage
        {
            get
            {
                return this._HomePage;
            }
            set
            {
                this._HomePage = value;
            }
        }

        private diWizardSettings _diWizard = new diWizardSettings();
        /// <summary>
        /// Gets or sets the diWizard settings.
        /// </summary>
        public diWizardSettings diWizard
        {
            get
            {
                return this._diWizard;
            }
            set
            {
                this._diWizard = value;
            }
        }

        private BottomPanelSetting _BottomPanel = new BottomPanelSetting();
        /// <summary>
        /// Gets or sets bottom panel settings
        /// </summary>
        public BottomPanelSetting BottomPanel
        {
            get { return _BottomPanel; }
            set { _BottomPanel = value; }
        }

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Save the adaptation class in the form of XML.
        /// </summary>
        /// <param name="fileName">file name with path</param>
        public void Save(string filenameWithPath)
        {
            XmlSerializer AdaptationSerialize = new XmlSerializer(typeof(Adaptation));
            StreamWriter AdaptationWriter = new StreamWriter(filenameWithPath);
            AdaptationSerialize.Serialize(AdaptationWriter, this);
            AdaptationWriter.Close();
        }

        /// <summary>
        /// Load the deserialize XML file.
        /// </summary>
        /// <param name="fileName">file name with path</param>
        /// <returns></returns>
        public static Adaptation Load(string filenameWithPath)
        {
            Adaptation Retval;
            try
            {
                XmlSerializer AdaptationSerialize = new XmlSerializer(typeof(Adaptation));
                StreamReader AdaptationReader = new StreamReader(filenameWithPath);
                Retval = (Adaptation)AdaptationSerialize.Deserialize(AdaptationReader);
                AdaptationReader.Close();
            }
            catch (Exception ex)
            {
                Retval = null;
            }
            return Retval;
        }

        #endregion

        #endregion

        #region " -- Inner Classes -- "

        #region " -- Application configuration section -- "

        /// <summary>
        /// Represents the application config section of DI5UI.
        /// <remarks> Key : application </remarks>
        /// </summary>
        public class ApplicationLevelSetting
        {
            #region " -- Public -- "

            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public ApplicationLevelSetting()
            {
            }

            #endregion

            #region " -- Properties -- "

            private string _ApplicationName = "DevInfo";
            /// <summary>
            /// Get or sets the application name
            /// <remarks> Key : app_name </remarks>
            /// </summary>
            public string ApplicationName
            {
                get
                {
                    return this._ApplicationName;
                }
                set
                {
                    this._ApplicationName = value;
                }
            }

            private string _ApplicationVersion = "7.0";
            /// <summary>
            /// Get or sets the application version
            /// <remarks> Key : app_version </remarks>
            /// </summary>
            public string ApplicationVersion
            {
                get
                {
                    return this._ApplicationVersion;
                }
                set
                {
                    this._ApplicationVersion = value;
                }
            }

            private string _DefaultLanguage = "DI_English [en]";
            /// <summary>
            /// Get or sets the default language name
            /// <remarks> Key : def_languagename </remarks>
            /// </summary>
            public string DefaultLanguage
            {
                get
                {
                    return this._DefaultLanguage;
                }
                set
                {
                    this._DefaultLanguage = value;
                }
            }

            private string _UserFile = "users.xml";
            /// <summary>
            /// Contains user login id (email id) and encrypted password
            /// </summary>
            /// <remarks>Used only in Web Application</remarks>
            public string UserFile
            {
                get { return _UserFile; }
                set { this._UserFile = value; }
            }

            private string _AdaptationURL = string.Empty;
            /// <summary>
            /// URL of the adaptation
            /// </summary>
            /// <remarks>Used only in web application</remarks>
            public string AdaptationURL
            {
                get { return _AdaptationURL; }
                set { _AdaptationURL = value; }
            }

            private bool _EnableLogin = false;
            /// <summary>
            /// Boolean flag to determine wheter to run DevInfo in login mode or loginless mode
            /// </summary>
            /// <remarks>Used only in web application</remarks>
            public bool EnableLogin
            {
                get { return _EnableLogin; }
                set { _EnableLogin = value; }
            }

            private bool _AllowAdminToCreateUser = true;
            /// <summary>
            /// Allow user or administrator to create a user account.
            /// This will be applicable only if EnableLogin property is set to true
            /// If true then only admin can create profile else user can himself create his profile
            /// </summary>
            /// <remarks>Used only in web application</remarks>
            public bool AllowAdminToCreateUser
            {
                get { return _AllowAdminToCreateUser; }
                set { _AllowAdminToCreateUser = value; }
            }

            private bool _ShowHitCounter = true;
            /// <summary>
            /// To show the hit count for website for unique users
            /// </summary>
            /// <remarks>Used only in web application</remarks>
            public bool ShowHitCounter
            {
                get { return _ShowHitCounter; }
                set { _ShowHitCounter = value; }
            }


            private bool _CensusInfoFeature = false;
            /// <summary>
            /// To show / supress the CensusInfo Features
            /// </summary>
            /// <remarks>Used only in web application</remarks>
            public bool CensusInfoFeature
            {
                get { return _CensusInfoFeature; }
                set { _CensusInfoFeature = value; }
            }

            private string _DI_Database_Caption = "di Database";
            /// <summary>
            /// Set adaptation based caption. Like ci Database
            /// </summary>
            public string DI_Database_Caption
            {
                get { return _DI_Database_Caption; }
                set { _DI_Database_Caption = value; }
            }

            private string _DI_Gallery_Caption = "di Gallery";
            /// <summary>
            /// Set adaptation based caption. Like ci Gallery
            /// </summary>
            public string DI_Gallery_Caption
            {
                get { return _DI_Gallery_Caption; }
                set { _DI_Gallery_Caption = value; }
            }


            private string _DI_Reports_Caption = "di Reports";
            /// <summary>
            /// Set adaptation based caption. Like ci Reports
            /// </summary>
            public string DI_Reports_Caption
            {
                get { return _DI_Reports_Caption; }
                set { _DI_Reports_Caption = value; }
            }

            private string _DI_Worldwide_Caption = "di Worldwide";
            /// <summary>
            /// Gets or sets the DI_Worldwide_Caption
            /// </summary>
            public string DI_Worldwide_Caption
            {
                get 
                {
                    return this._DI_Worldwide_Caption; 
                }
                set 
                {
                    this._DI_Worldwide_Caption = value; 
                }
            }

            private string _DI_Book_Extension = "diBook(*.diBook)|*.diBook";
            /// <summary>
            /// Gets or sets the DI_Book_Extension
            /// </summary>
            public string DI_Book_Extension
            {
                get { return this._DI_Book_Extension; }
                set { this._DI_Book_Extension = value; }
            }

            private bool _WorldwideRegistration = true;
            /// <summary>
            /// gets or sets the WorldwideRegistration
            /// </summary>
            public bool WorldwideRegistration
            {
                get
                {
                    return this._WorldwideRegistration;
                }
                set
                {
                    this._WorldwideRegistration = value;
                }
            }

            private bool _SupressVersionHistory = false;
            /// <summary>
            /// Gets or sets the SupressVersionHistory
            /// </summary>
            public bool SupressVersionHistory
            {
                get 
                {
                    return this._SupressVersionHistory; 
                }
                set 
                {
                    this._SupressVersionHistory = value; 
                }
            }

            private bool _ApplyNativeLanguageOnButton = false;
            /// <summary>
            /// Apply Language On Button
            /// </summary>
            public bool ApplyNativeLanguageOnButton
            {
                get
                {
                    return this._ApplyNativeLanguageOnButton;
                }
                set
                {
                    this._ApplyNativeLanguageOnButton = value;
                }
            }

            #endregion

            #endregion
        }

        #endregion

        #region " -- diWizard section -- "

        /// <summary>
        /// Represents the diWizard section 
        /// </summary>
        public class diWizardSettings
        {
            #region " -- Public -- "

            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public diWizardSettings()
            {

            }

            #endregion

            #region " -- Properties -- "

            private string _diWizardTitle = "di Data Wizard";
            /// <summary>
            /// di Wizard Title
            /// </summary>
            public string diWizardTitle
            {
                get
                {
                    return this._diWizardTitle;
                }
                set
                {
                    this._diWizardTitle = value;
                }
            }

            private bool _ShowBanner = true;
            /// <summary>
            /// Top banner is to displayed or not. If banner is not displayed whole contenet shall move upwards
            /// </summary>
            public bool ShowBanner
            {
                get
                {
                    return this._ShowBanner;
                }
                set
                {
                    this._ShowBanner = value;
                }
            }

            private bool _ShowBottomPanel = true;
            /// <summary>
            /// Bottom Panel is to displayed or not.
            /// </summary>
            public bool ShowBottomPanel
            {
                get
                {
                    return this._ShowBottomPanel;
                }
                set
                {
                    this._ShowBottomPanel = value;
                }
            }

            private bool _ShowDBSelection = true;
            /// <summary>
            /// Wheteher to display database selection control or not
            /// </summary>
            public bool ShowDBSelection
            {
                get
                {
                    return this._ShowDBSelection;
                }
                set
                {
                    this._ShowDBSelection = value;
                }
            }

            private bool _ShowMap = true;
            /// <summary>
            /// Wheteher to display map tab in diwizard or not
            /// </summary>
            public bool ShowMap
            {
                get
                {
                    return this._ShowMap;
                }
                set
                {
                    this._ShowMap = value;
                }
            }

            private bool _ShowGallery = true;
            /// <summary>
            /// Wheteher to display gallery tab in diwizard or not
            /// </summary>
            public bool ShowGallery
            {
                get
                {
                    return this._ShowGallery;
                }
                set
                {
                    this._ShowGallery = value;
                }
            }

            private bool _ShowHelp = true;
            /// <summary>
            /// Show / Hide help link in diwizard home page
            /// </summary>
            public bool ShowHelp
            {
                get
                {
                    return this._ShowHelp;
                }
                set
                {
                    this._ShowHelp = value;
                }
            }

            private bool _ShowTour = true;
            /// <summary>
            /// Show / Hide tour link in diwizard home page
            /// </summary>
            public bool ShowTour
            {
                get
                {
                    return this._ShowTour;
                }
                set
                {
                    this._ShowTour = value;
                }
            }

            private string _TourExtension = "ppt";
            /// <summary>
            /// Tour File extension
            /// </summary>
            public string TourExtension
            {
                get
                {
                    return this._TourExtension;
                }
                set
                {
                    this._TourExtension = value;
                }
            }


            private bool _IndicatorPaging = true;
            /// <summary>
            /// Enable / Disbale Indicator paging in diwizard 
            /// </summary>
            public bool IndicatorPaging
            {
                get
                {
                    return this._IndicatorPaging;
                }
                set
                {
                    this._IndicatorPaging = value;
                }
            }

            private int _IndicatorPageSize = 50;
            /// <summary>
            /// Page Size for Indicator List in di Wizard
            /// </summary>
            public int IndicatorPageSize
            {
                get
                {
                    return this._IndicatorPageSize;
                }
                set
                {
                    this._IndicatorPageSize = value;
                }
            }

            private bool _AreaPaging = true;
            /// <summary>
            /// Enable / Disbale Area paging in diwizard 
            /// </summary>
            public bool AreaPaging
            {
                get
                {
                    return this._AreaPaging;
                }
                set
                {
                    this._AreaPaging = value;
                }
            }

            private int _AreaPageSize = 50;
            /// <summary>
            /// Page Size for Area List in di Wizard
            /// </summary>
            public int AreaPageSize
            {
                get
                {
                    return this._AreaPageSize;
                }
                set
                {
                    this._AreaPageSize = value;
                }
            }

            private bool _ShowMRDCheckBox = true;
            /// <summary>
            /// Wheteher to display MRD CheckBox in diwizard or not
            /// </summary>
            public bool ShowMRDCheckBox
            {
                get
                {
                    return this._ShowMRDCheckBox;
                }
                set
                {
                    this._ShowMRDCheckBox = value;
                }
            }


            private bool _ShowIndicatorListByDefault = true;
            /// <summary>
            /// Wheteher to display indicator list view by default , instead of tree view
            /// </summary>
            public bool ShowIndicatorListByDefault
            {
                get
                {
                    return this._ShowIndicatorListByDefault;
                }
                set
                {
                    this._ShowIndicatorListByDefault = value;
                }
            }

            private ICType _GroupICType = ICType.Sector;
            /// <summary>
            /// Defines the IC Grouping for Indicator ListView
            /// </summary>
            public ICType GroupICType
            {
                get { return _GroupICType; }
                set { _GroupICType = value; }
            }
	


            private bool _ShowAreaListByDefault = true;
            /// <summary>
            /// Wheteher to display ares list by default, instead of tree view
            /// </summary>
            public bool ShowAreaListByDefault
            {
                get
                {
                    return this._ShowAreaListByDefault;
                }
                set
                {
                    this._ShowAreaListByDefault = value;
                }
            }

            private bool _ShowIndicatorTree = true;
            /// <summary>
            /// Wheteher to display indicator tree view or not
            /// </summary>
            public bool ShowIndicatorTree
            {
                get
                {
                    return this._ShowIndicatorTree;
                }
                set
                {
                    this._ShowIndicatorTree = value;
                }
            }

            private bool _ShowAreaTree = true;
            /// <summary>
            /// Wheteher to display Area tree view or not
            /// </summary>
            public bool ShowAreaTree
            {
                get
                {
                    return this._ShowAreaTree;
                }
                set
                {
                    this._ShowAreaTree = value;
                }
            }

            private bool _ShowIndicatorAsHomePage = false;
            /// <summary>
            /// Wheteher to display indicator page as home page
            /// </summary>
            public bool ShowIndicatorAsHomePage
            {
                get
                {
                    return this._ShowIndicatorAsHomePage;
                }
                set
                {
                    this._ShowIndicatorAsHomePage = value;
                }
            }

            #endregion

            #endregion
        }

        #endregion

        #region " -- General section -- "

        /// <summary>
        /// Represents the general section of DI5UI.
        /// <remarks> Key : general </remarks>
        /// </summary>
        public class GeneralSettings
        {
            #region " -- Public -- "
 
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public GeneralSettings()
            {

            }

            #endregion

            #region " -- Enum -- "
            public enum MapInfo
            {
                /// <summary>
                ///  0 - Area Information form to be displayed as it is
                /// </summary>
                Info = 0,
                /// <summary>
                ///  1 - Area Information form to be displayed along with link for profile for Area selected. On clicking that link, profile file (.pdf) associated with selected Area will be opened
                /// </summary>
                Profile = 1,
                /// <summary>
                /// 2 - Profile file (.pdf) associated with selected Area will be opened directly. If file does not exists in folder for selected Area, then open Area Information form.
                /// </summary>
                InfoProfile = 2,
            }
            #endregion

            #region " -- Properties -- "

            private bool _ClassicViewAvailable = true;
            /// <summary>
            /// Get or sets the classic view
            /// <remarks> Key : classsicview_available </remarks>
            /// </summary>
            public bool ClassicViewAvailable
            {
                get
                {
                    return this._ClassicViewAvailable;
                }
                set
                {
                    this._ClassicViewAvailable = value;
                }
            }

            private bool _ShowClassicImages = false;
            /// <summary>
            /// Gets or sets the ShowClassicImages
            /// <remarks> Key : show_classic_images of Pref file </remarks>
            /// </summary>
            public bool ShowClassicImages
            {
                get
                {
                    return this._ShowClassicImages;
                }
                set
                {
                    this._ShowClassicImages = value;
                }
            }

            private bool _BigToolbar = true;
            /// <summary>
            /// Gets or sets the BigToolbar
            /// <remarks> Key : big_toolbar of Pref file </remarks>
            /// </summary>            
            public bool BigToolbar
            {
                get
                {
                    return this._BigToolbar;
                }
                set
                {
                    this._BigToolbar = value;
                }
            }

            private bool _ClearSpatialMapFolder = false;
            /// <summary>
            /// Gets or sets ClearSpatialMapFolder.
            /// </summary>
            /// <remarks>UI will removed map files from spatial map folder on closure of application</remarks>
            public bool ClearSpatialMapFolder
            {
                //Secured map will be encrypted and will be 
                get { return _ClearSpatialMapFolder; }
                set { _ClearSpatialMapFolder = value; }
            }

            private int _DefaultAreaLevel = 2;
            /// <summary>
            /// Used in diWizard (Web) for Default Area level in Area tab
            /// </summary>
            public int DefaultAreaLevel
            {
                get { return _DefaultAreaLevel; }
                set { _DefaultAreaLevel = value; }
            }

            private bool _ShowIndicatorInMap = true;
            /// <summary>
            /// Wheteher to display indicator selection dialog in map wizard or not. Used in di6 web
            /// </summary>
            public bool ShowIndicatorInMap
            {
                get
                {
                    return this._ShowIndicatorInMap;
                }
                set
                {
                    this._ShowIndicatorInMap = value;
                }
            }

            private bool _EnableOnlineDatabaseSearch = true;
            /// <summary>
            /// Wheteher to enable online database search in database selection panel. Used in di6 web
            /// </summary>
            public bool EnableOnlineDatabaseSearch
            {
                get
                {
                    return this._EnableOnlineDatabaseSearch;
                }
                set
                {
                    this._EnableOnlineDatabaseSearch = value;
                }
            }

            private string _ApplicationMode = "Normal";
            /// <summary>
            /// Set the runnig mode of the DI6 Web application.(e.g. modes may be - Wizard, Map etc.)
            /// </summary>
            public string ApplicationMode
            {
                get
                {
                    return this._ApplicationMode;
                }
                set
                {
                    this._ApplicationMode = value;
                }
            }

            private bool _DIBVisible = true;
            /// <summary>
            /// Gets or sets the value for visibility of Disputed International Boundaries Toolbar option in Map Wizard.
            /// </summary>
            public bool DIBVisible
            {
                get { return _DIBVisible; }
                set { _DIBVisible = value; }
            }


            private MapInfo _MapInformation = MapInfo.Info;
            /// <summary>
            /// This defines the type of information (Area Information, Profile) to be displayed when map is clicked with Information on
            /// </summary>
            public MapInfo MapInformation
            {
                get { return _MapInformation; }
                set { _MapInformation = value; }
            }



            #endregion



            #endregion
        }

        #endregion

        #region " -- Command Button Section -- "

        /// <summary>
        /// Represents the command buttons section of DI5UI.
        /// <remarks> Key : command_buttons </remarks>
        /// </summary>
        public class CommandButtonsSettings
        {

            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public CommandButtonsSettings()
            {
                //-- Create the instancd of commandbuttonstyle
                //-- Only font name, style and size is used for command buttons
                this._CommandButtonStyle = new FontSetting("Arial", FontStyle.Regular, 10, Color.Black, Color.Gray, StringAlignment.Center);
            }

            #endregion

            #region " -- Properties -- "

            private FontSetting _CommandButtonStyle;
            /// <summary>
            /// Gets or sets the command button style.
            /// Key : font, size, style
            /// </summary>
            public FontSetting CommandButtonStyle
            {
                get
                {
                    return this._CommandButtonStyle;
                }
                set
                {
                    this._CommandButtonStyle = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- Graphic Section -- "

        /// <summary>
        /// Represents the Adaptation section of DI5UI.
        /// <remarks> Key : graphics </remarks>
        /// </summary>
        public class GraphicsSettings
        {

            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public GraphicsSettings()
            {
                //-- Set the default gradiant color.
                this._GradiantColor = new List<string>();
                this._GradiantColor.Add("#DDDDDD");
                this._GradiantColor.Add("#FAFAFA");

                //-- Set the default gradiant table color.
                this._GradiantTableColor = new List<string>();
                this._GradiantTableColor.Add("#DCDCDC");
                this._GradiantTableColor.Add("#A9A9A9");
            }

            #endregion

            #region " -- Properties -- "

            private string _TopBannerBgColor = "#F5F5F5";
            /// <summary>
            /// Gets or sets the top banner background color
            /// <remarks> Key : top_banner_bg_color </remarks>
            /// </summary>
            public string TopBannerBgColor
            {
                get
                {
                    return this._TopBannerBgColor;
                }
                set
                {
                    this._TopBannerBgColor = value;
                }
            }

            private string _TopBannerBottomBgColor = "#ABABAB";
            /// <summary>
            /// Gets or sets the top banner bottom background color
            /// <remarks> Key : top_banner_bottom_bg_color </remarks>
            /// </summary>
            [Obsolete]
            public string TopBannerBottomBgColor
            {
                get
                {
                    return this._TopBannerBottomBgColor;
                }
                set
                {
                    this._TopBannerBottomBgColor = value;
                }
            }

            private string _GradiantBgColor = "#f5f5f5";
            /// <summary>
            /// Gets or sets the gradiant background color.
            /// <remarks> Key : gradcolor_bg </remarks>
            /// </summary>
            public string GradiantBgColor
            {
                get
                {
                    return this._GradiantBgColor;
                }
                set
                {
                    this._GradiantBgColor = value;
                }
            }

            private List<string> _GradiantColor;
            /// <summary>
            /// Gets or sets the gradiant color
            /// <remarks> Key : gradcolor_1, gradcolor_2 </remarks>
            /// </summary>
            public List<string> GradiantColor
            {
                get
                {
                    return this._GradiantColor;
                }
                set
                {
                    this._GradiantColor = value;
                }
            }

            private List<string> _GradiantTableColor;
            /// <summary>
            /// Gets or sets the gradiant table color.
            /// <remarks> gradcolor_tlb_1, gradcolor_tlb_2 </remarks>
            /// </summary>
            public List<string> GradiantTableColor
            {
                get
                {
                    return this._GradiantTableColor;
                }
                set
                {
                    this._GradiantTableColor = value;
                }
            }

            private string _ToolbarBgColor = "#eeeeee";
            /// <summary>
            /// Gets or sets the toolbar background color.
            /// <remarks> Key : toolbar_bg_clr </remarks>
            /// </summary>
            public string ToolbarBgColor
            {
                get
                {
                    return this._ToolbarBgColor;
                }
                set
                {
                    this._ToolbarBgColor = value;
                }
            }

            private string _SepratorColor = "#a9a9a9";
            /// <summary>
            /// Gets or sets the seprator color.
            /// <remarks> Key : seperatorcolor </remarks>
            /// </summary>
            public string SepratorColor
            {
                get
                {
                    return this._SepratorColor;
                }
                set
                {
                    this._SepratorColor = value;
                }
            }

            private string _SelectionListBgColor = "#eeeeee";
            /// <summary>
            /// Gets or sets the selection list background color.
            /// <remarks> Key : sellistbgcolor </remarks>
            /// </summary>
            public string SelectionListBgColor
            {
                get
                {
                    return this._SelectionListBgColor;
                }
                set
                {
                    this._SelectionListBgColor = value;
                }
            }

            private string _LinkColor = "#404040";
            /// <summary>
            /// Gets or sets the link color.
            /// </summary>
            public string LinkColor
            {
                get
                {
                    return this._LinkColor;
                }
                set
                {
                    this._LinkColor = value;
                }
            }

            private string _LinkSelectionColor = "#003399";
            /// <summary>
            /// Gets or sets the link selection color.
            /// <remarks> Key : linkselcolor </remarks>
            /// </summary>
            public string LinkSelectionColor
            {
                get
                {
                    return this._LinkSelectionColor;
                }
                set
                {
                    this._LinkSelectionColor = value;
                }
            }

            private string _GridHeaderColor = "#4F77AC";
            /// <summary>
            /// Gets or sets the grid header color.
            /// <remarks> Key : gridheadercolor </remarks>
            /// </summary>
            [Obsolete]
            public string GridHeaderColor
            {
                get
                {
                    return this._GridHeaderColor;
                }
                set
                {
                    this._GridHeaderColor = value;
                }
            }

            private string _GridColor = "#000000";
            /// <summary>
            /// gets or sets the grid foreground color.
            /// <remarks> Key : gridforecolor </remarks>
            /// </summary>
            [Obsolete]
            public string GridColor
            {
                get
                {
                    return this._GridColor;
                }
                set
                {
                    this._GridColor = value;
                }
            }

            private string _AreaMapCanvasColor = "#ffffff";
            /// <summary>
            /// Gets or sets the area map canvas color.
            /// <remarks> Key = area_map_canvas_clr </remarks>
            /// </summary>
            public string AreaMapCanvasColor
            {
                get
                {
                    return this._AreaMapCanvasColor;
                }
                set
                {
                    this._AreaMapCanvasColor = value;
                }
            }

            private string _AreaMapFillColor = "#F1F0EF";
            /// <summary>
            /// Gets or sets area map fill color.
            /// <remarks> Key : area_map_fill_clr </remarks>
            /// </summary>
            public string AreaMapFillColor
            {
                get
                {
                    return this._AreaMapFillColor;
                }
                set
                {
                    this._AreaMapFillColor = value;
                }
            }

            private string _AreaMapSelectColor = "#FFFFB9";
            /// <summary>
            /// Gets or sets the area map color
            /// <remarks> Key : area_map_select_clr </remarks>
            /// </summary>
            public string AreaMapSelectColor
            {
                get
                {
                    return this._AreaMapSelectColor;
                }
                set
                {
                    this._AreaMapSelectColor = value;
                }
            }

            private string _AreaMapBorderColor = "#000000";
            /// <summary>
            /// Gets or sets the area map border color.
            /// <remarks> Key : area_map_border_clr </remarks>
            /// </summary>
            public string AreaMapBorderColor
            {
                get
                {
                    return this._AreaMapBorderColor;
                }
                set
                {
                    this._AreaMapBorderColor = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- Directory Location Section -- "

        /// <summary>
        /// Represents the director location section of DI5UI.
        /// <remarks> Key : locations </remarks>
        /// </summary>
        public class DirectorySettings
        {
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public DirectorySettings()
            { }

            #endregion

            #region " -- Properties -- "

            private string _Adaptation = @"Bin\Graphics\Adaptation\";
            /// <summary>
            /// Gets or sets the adaptation path folder. Bin\Graphics\Adaptation\
            /// <remarks> Key : graphics_adaptation </remarks>
            /// </summary>
            public string Adaptation
            {
                get
                {
                    return this._Adaptation;
                }
                set
                {
                    this._Adaptation = value;
                }
            }

            private string _Common = @"Bin\Graphics\Common\";
            /// <summary>
            /// Gets or sets the common folder location. Bin\Graphics\Common\
            /// <remarks> Key : graphics_common </remarks>
            /// </summary>
            public string Common
            {
                get
                {
                    return this._Common;
                }
                set
                {
                    this._Common = value;
                }
            }

            private string _Language = @"Language\";
            /// <summary>
            /// gets or sets the language folder name. Language\
            /// <remarks> Key : lngfolder </remarks>
            /// </summary>
            public string Language
            {
                get
                {
                    return this._Language;
                }
                set
                {
                    this._Language = value;
                }
            }

            private string _Sounds = @"Bin\Sounds\";
            /// <summary>
            /// Gets or sets the sound folder location. Bin\Sounds\
            /// <remarks> Key : soundsfolder </remarks>
            /// </summary>
            public string Sounds
            {
                get
                {
                    return this._Sounds;
                }
                set
                {
                    this._Sounds = value;
                }
            }

            private string _Stock = @"Bin\Stock\";
            /// <summary>
            /// Gets or sets the stock folder location. Bin\Stock\
            /// <remarks> Key : stockfolder </remarks>
            /// </summary>
            public string Stock
            {
                get
                {
                    return this._Stock;
                }
                set
                {
                    this._Stock = value;
                }
            }

            private string _Content = @"Content\";
            /// <summary>
            /// Gets or sets the content folder location. Content\
            /// <remarks> Key : contentfolder </remarks>
            /// </summary>
            public string Content
            {
                get
                {
                    return this._Content;
                }
                set
                {
                    this._Content = value;
                }
            }

            private string _Reports = @"Reports\";
            /// <summary>
            /// Gets or sets the reports folder location. Reports\
            /// <remarks> Key : reportsfolder </remarks>
            /// </summary>
            public string Reports
            {
                get
                {
                    return this._Reports;
                }
                set
                {
                    this._Reports = value;
                }
            }

            private string _Data = @"Data\";
            /// <summary>
            /// gets or sets the data folder location. Data\
            /// <remarks> Key : dbfolder </remarks>
            /// </summary>
            public string Data
            {
                get
                {
                    return this._Data;
                }
                set
                {
                    this._Data = value;
                }
            }

            private string _Map = @"Maps\";
            /// <summary>
            /// Gets or sets the map folder location. Maps\
            /// <remarks> Key : mapfolder </remarks>
            /// </summary>
            public string Map
            {
                get
                {
                    return this._Map;
                }
                set
                {
                    this._Map = value;
                }
            }

            private string _Profile = @"Profiles\";
            /// <summary>
            /// gets or sets the profile folder location
            /// </summary>
            public string Profile
            {
                get { return _Profile; }
                set { _Profile = value; }
            }


            private string _Gallery = @"Gallery\Presentations\";
            /// <summary>
            /// Gets or sets the gallery folder location. Gallery\Presentations\
            /// <remarks> Key : galleryfolder </remarks>
            /// </summary>
            public string Gallery
            {
                get
                {
                    return this._Gallery;
                }
                set
                {
                    this._Gallery = value;
                }
            }

            private string _Template = @"Bin\Templates\";
            /// <summary>
            /// Gets or sets the template folder location. Bin\Templates\
            /// <remarks> Key : file_templates </remarks>
            /// </summary>
            public string Template
            {
                get
                {
                    return this._Template;
                }
                set
                {
                    this._Template = value;
                }
            }

            private string _SystemFolder = "SystemFiles";
            /// <summary>
            /// Gets or sets the system files. SystemFiles
            /// </summary>
            /// <remarks> This folder contains the pref.xml, Autocomplet.xml and utragridselection.xml files </remarks>
            public string SystemFolder
            {
                get
                {
                    return this._SystemFolder;
                }
                set
                {
                    this._SystemFolder = value;
                }
            }

            private string _Selections = "Selections";
            /// <summary>
            /// Gets or sets the selections
            /// </summary>
            /// <remarks> Selection folder contains the XmlExport.xml and ReportsFormula Xml files </remarks>
            public string Selections
            {
                get
                {
                    return this._Selections;
                }
                set
                {
                    this._Selections = value;
                }
            }

            private List<string> _GoogleEarthPath = new List<string>(new string[] { @"C:\Program Files\Google\Google Earth", @"C:\Program Files\Google\Google Earth\Client", @"C:\Program Files (x86)\Google\Google Earth" });
            /// <summary>
            /// Gets or sets list of Google Earth Paths
            /// </summary>
            public List<string> GoogleEarthPath
            {
                get
                {
                    return this._GoogleEarthPath;
                }
                set
                {
                    this._GoogleEarthPath = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- File Names and Extension Section -- "

        /// <summary>
        /// Represents the file name and extension section of DI5UI.
        /// <remarks> Key : file_names </remarks>
        /// </summary>
        public class ContentSettings
        {
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public ContentSettings()
            { }

            #endregion

            #region " -- Properties -- "

            private string _HelpFileName = "Help";
            /// <summary>
            /// Gets or sets the help file
            /// <remarks> Key : help_file_name </remarks>
            /// </summary>
            public string HelpFileName
            {
                get
                {
                    return this._HelpFileName;
                }
                set
                {
                    this._HelpFileName = value;
                }
            }

            private string _HelpFileExtension = "pdf";
            /// <summary>
            /// Gets or sets the help file extension.
            /// <remarks> Key : help_file_ext </remarks>
            /// </summary>
            public string HelpFileExtension
            {
                get
                {
                    return this._HelpFileExtension;
                }
                set
                {
                    this._HelpFileExtension = value;
                }
            }

            private string _QuickStartFileName = "QuickStart";
            /// <summary>
            /// Gets or sets the Quick Start FileName
            /// <remarks> Key : QuickStartFileName </remarks>
            /// </summary>
            public string QuickStartFileName
            {
                get
                {
                    return this._QuickStartFileName;
                }
                set
                {
                    this._QuickStartFileName = value;
                }
            }

            private string _QuickStartFileExtension = "pdf";
            /// <summary>
            /// Gets or sets the QuickStart file extension.
            /// <remarks> Key : QuickStartFileExtension </remarks>
            /// </summary>
            public string QuickStartFileExtension
            {
                get
                {
                    return this._QuickStartFileExtension;
                }
                set
                {
                    this._QuickStartFileExtension = value;
                }
            }

            private string _GettingStartedFileName = "GettingStarted";
            /// <summary>
            /// Gets or sets the Getting Started FileName
            /// <remarks> Key : GettingStartedFileName </remarks>
            /// </summary>
            public string GettingStartedFileName
            {
                get
                {
                    return this._GettingStartedFileName;
                }
                set
                {
                    this._GettingStartedFileName = value;
                }
            }

            private string _GettingStartedFileExtension = "pdf";
            /// <summary>
            /// Gets or sets the Getting Started file extension.
            /// <remarks> Key : GettingStartedFileExtension </remarks>
            /// </summary>
            public string GettingStartedFileExtension
            {
                get
                {
                    return this._GettingStartedFileExtension;
                }
                set
                {
                    this._GettingStartedFileExtension = value;
                }
            }

            private string _TourFileName = "Tour";
            /// <summary>
            /// Gets or sets the tour file name
            /// <remarks> Key : tour_file_name </remarks>
            /// </summary>
            public string TourFileName
            {
                get
                {
                    return this._TourFileName;
                }
                set
                {
                    this._TourFileName = value;
                }
            }

            private string _TourFileExtension = "pps";
            /// <summary>
            /// Gets or sets the tour file extension.
            /// <remarks> Key : tour_file_ext </remarks>
            /// </summary>
            public string TourFileExtension
            {
                get
                {
                    return this._TourFileExtension;
                }
                set
                {
                    this._TourFileExtension = value;
                }
            }

            private string _LinkFileName = "MoreInfo";
            /// <summary>
            /// Gets or sets the link file name.
            /// <remarks> Key : link_file_name </remarks>
            /// </summary>
            public string LinkFileName
            {
                get
                {
                    return this._LinkFileName;
                }
                set
                {
                    this._LinkFileName = value;
                }
            }

            private string _LinkFileExtension = "htm";
            /// <summary>
            /// Gets or sets the link file extension.
            /// <remarks> Key : link_file_ext </remarks>
            /// </summary>
            public string LinkFileExtension
            {
                get
                {
                    return this._LinkFileExtension;
                }
                set
                {
                    this._LinkFileExtension = value;
                }
            }

            private string _OrganizationFileName = "Organization";
            /// <summary>
            /// Gets or sets the organization file.
            /// <remarks> Key : organization_file_name </remarks>
            /// </summary>
            public string OrganizationFileName
            {
                get
                {
                    return this._OrganizationFileName;
                }
                set
                {
                    this._OrganizationFileName = value;
                }
            }

            private string _OrganizationFileExtension = "htm";
            /// <summary>
            /// Gets or sets the organization file extension.
            /// <remarks> Key : organization_file_ext </remarks>
            /// </summary>
            public string OrganizationFileExtension
            {
                get
                {
                    return this._OrganizationFileExtension;
                }
                set
                {
                    this._OrganizationFileExtension = value;
                }
            }

            private string _ProductFileName = "Product";
            /// <summary>
            /// Gets or sets the product file name
            /// <remarks> Key : product_file_name </remarks>
            /// </summary>
            public string ProductFileName
            {
                get
                {
                    return this._ProductFileName;
                }
                set
                {
                    this._ProductFileName = value;
                }
            }

            private string _ProductFileExtension = "htm";
            /// <summary>
            /// Gets or sets the product file extension.
            /// <remarks> Key : product_file_ext </remarks>
            /// </summary>
            public string ProductFileExtension
            {
                get
                {
                    return this._ProductFileExtension;
                }
                set
                {
                    this._ProductFileExtension = value;
                }
            }

            private string _VersionSite = @"http://www.devinfo.info/version.html";
            /// <summary>
            /// Gets or sets the version site.
            /// <remarks> Key : version_site </remarks>
            /// </summary>
            public string VersionSite
            {
                get
                {
                    return this._VersionSite;
                }
                set
                {
                    this._VersionSite = value;
                }
            }

            private string _SupportSite = @"http://www.devinfo.org";
            /// <summary>
            /// Gets or sets the support site.
            /// </summary>
            /// <remarks> Key : support_site </remarks>
            public string SupportSite
            {
                get
                {
                    return this._SupportSite;
                }
                set
                {
                    this._SupportSite = value;
                }
            }

            private string _SupportMail = "support@devinfo.info";
            /// <summary>
            /// Gets or sets the support site.
            /// </summary>
            public string SupportMail
            {
                get
                {
                    return this._SupportMail;
                }
                set
                {
                    this._SupportMail = value;
                }
            }
            #endregion

        }

        #endregion

        #region " -- FTP Section -- "        

        /// <summary>
        /// Represents the LiveUpdate section of User Preference
        /// </summary>
        public class LiveUpdateSetting
        {
            #region " -- Properties -- "

            private string _LiveUpdateKey = "DICILU";
            /// <summary>
            /// get n set Live update key [DILU- dev info and DICILU for- census info] //-- This code will passed to webservice to get FTP details as return
            /// </summary>
            public string LiveUpdateKey
            {
                get
                {
                    //by default Census infio live update
                    if (string.IsNullOrEmpty(this._LiveUpdateKey))
                        this._LiveUpdateKey = "DICILU";

                    return this._LiveUpdateKey;
                }
                set { this._LiveUpdateKey = value; }
            }

        //    //TODO Check the relevance of default FTP details before release. May set them to blank

        //    private string _FTPServerName = "61.12.1.180";
        //    /// <summary>
        //    /// Gets or sets the FTP Server Name
        //    /// </summary>
        //    public string FTPServerName
        //    {
        //        get
        //        {
        //            return this._FTPServerName;
        //        }
        //        set
        //        {
        //            this._FTPServerName = value;
        //        }
        //    }

        //    private string _FTPDirectory = "DevInfo Live Update Folder";
        //    /// <summary>
        //    /// Gets or sets the FTP Directory
        //    /// </summary>
        //    public string FTPDirectory
        //    {
        //        get
        //        {
        //            return this._FTPDirectory;
        //        }
        //        set
        //        {
        //            this._FTPDirectory = value;
        //        }
        //    }

        //    private string _FTPUserName = Encryption.Encrypt("testftpdevgrp");
        //    /// <summary>
        //    /// Gets or sets the FTP User Name
        //    /// </summary>
        //    /// <remarks>User name will be encrypted using md5 encryption</remarks>

        //    public string FTPUserName
        //    {
        //        get
        //        {
        //            return this._FTPUserName;
        //        }
        //        set
        //        {
        //            this._FTPUserName = value;
        //        }
        //    }

        //    private string _FTPPassword = Encryption.Encrypt("testftpdevgrp");
        //    /// <summary>
        //    /// Gets or sets the FTP Password. While Setting the password set the clear text. While retrieving the password use decrypt function
        //    /// </summary>
        //    /// <remarks>Password will be encrypted using md5 encryption</remarks>
        //    public string FTPPassword
        //    {
        //        get
        //        {
        //            return this._FTPPassword;
        //        }
        //        set
        //        {
        //            this._FTPPassword = value;
        //        }
        //    }


            #endregion
        }

        #endregion

        #region " -- Reports Section -- "

        /// <summary>
        /// Represents the reports section of pref.xml.
        /// <remarks> Key : report </remarks>
        /// </summary>
        public class ReportsSetting
        {
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public ReportsSetting()
            {
                this._DataFiles = new List<string>();
                //TODO: Add some files
                //dfid_mdg_formulas.xml,dfid_ind_selections.xml,dfid_cap_ind_selections.xml
            }

            #endregion

            #region " -- Properties -- "

            private List<string> _DataFiles;
            /// <summary>
            /// Gets or sets the data file list
            /// <remarks> Key : datafiles </remarks>
            /// </summary>
            public List<string> DataFiles
            {
                get
                {
                    return this._DataFiles;
                }
                set
                {
                    this._DataFiles = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- SMTP section -- "

        /// <summary>
        /// Represents the general section of DI5UI.
        /// <remarks> Key : general </remarks>
        /// </summary>
        public class SMTPSettings
        {
            #region " -- Public -- "

            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public SMTPSettings()
            {

            }

            #endregion

            #region " -- Properties -- "

            private string _Server = string.Empty;
            /// <summary>
            /// Get or sets the SMTP server name
            /// </summary>
            public string Server
            {
                get { return this._Server; }
                set { this._Server = value; }
            }

            private string _Port = string.Empty;
            /// <summary>
            /// Get or sets the Port number
            /// </summary>
            public string Port
            {
                get { return this._Port; }
                set { this._Port = value; }
            }

            private string _UserID = string.Empty;
            /// <summary>
            /// Get or sets the SMTP User ID
            /// </summary>
            public string UserID
            {
                get { return this._UserID; }
                set { this._UserID = value; }
            }

            private string _Password = string.Empty;
            /// <summary>
            /// Get or sets the SMTP password
            /// </summary>
            public string Password
            {
                get { return this._Password; }
                set { this._Password = value; }
            }

            private string _SenderMailID = string.Empty;
            /// <summary>
            /// Mail sender's email ID
            /// </summary>
            public string SenderMailID
            {
                get { return this._SenderMailID; }
                set { this._SenderMailID = value; }
            }


            #endregion

            #endregion
        }

        #endregion

        #region " -- Presentation Section -- "

        /// <summary>
        /// New section to store properties related to presentation
        /// </summary>
        public class PresentationSetting
        {
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public PresentationSetting()
            {

            }

            #endregion

            #region " -- Properties -- "


            private Size _ThumbnailImageSize = new Size(350, 350);
            /// <summary>
            /// Gets default thumbnail image size to be generated by presentations
            /// </summary>
            public Size ThumbnailImageSize
            {
                get { return _ThumbnailImageSize; }

                set { _ThumbnailImageSize = value; }
            }

            private Size _GalleryTileSize = new Size(150, 150);
            /// <summary>
            /// Gets default tiled image size for gallery
            /// </summary>
            public Size GalleryTileSize
            {
                get { return _GalleryTileSize; }

                set { _GalleryTileSize = value; }
            }


            private string _BackgroundMapAreaId = string.Empty;
            /// <summary>
            /// AreaId whose associated map is to be displayed. Introduced for MVPInfo 
            /// </summary>
            public string BackgroundMapAreaId
            {
                get { return _BackgroundMapAreaId; }
                set { _BackgroundMapAreaId = value; }
            }


            private string _DefaultMissingColor = "#808080"; //Color.Gray (128,128,128)
            /// <summary>
            /// Default missing color as hexadecimal value. Introduced for MVPInfo
            /// </summary>
            public string DefaultMissingColor
            {
                get { return _DefaultMissingColor; }
                set { _DefaultMissingColor = value; }
            }



            #endregion
        }

        #endregion

        #region " -- Home Page Section -- "

        /// <summary>
        /// Represents the Home apge section of pref.xml.
        /// <remarks> Key : report </remarks>
        /// </summary>
        public class HomePageSetting
        {
            #region " -- New / Dispose -- "

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public HomePageSetting()
            {

            }

            #endregion

            #region " -- Properties -- "

            private bool _ShowHomePageLink = false;
            /// <summary>
            /// Get or sets the hime page link
            /// <remarks> Key : show_link_home_page </remarks>
            /// </summary>
            public bool ShowHomePageLink
            {
                get
                {
                    return this._ShowHomePageLink;
                }
                set
                {
                    this._ShowHomePageLink = value;
                }
            }

            private bool _ShowHomePageBgImage = false;
            /// <summary>
            /// Get or sets the home page background image.
            /// <remarks> Key : home_page_bg_image </remarks>
            /// </summary>
            public bool ShowHomePageBgImage
            {
                get
                {
                    return this._ShowHomePageBgImage;
                }
                set
                {
                    this._ShowHomePageBgImage = value;
                }
            }

            private string _HomePageBgColor = "#ffffff";
            /// <summary>
            /// Get or sets the home page background color
            /// <remarks> Key : home_page_bg_color </remarks>
            /// </summary>
            public string HomePageBgColor
            {
                get
                {
                    return this._HomePageBgColor;
                }
                set
                {
                    this._HomePageBgColor = value;
                }
            }

            private string _HomePageTopBgColor = "#003399";
            /// <summary>
            /// Gets or sets the home page top background color.
            /// <remarks> Key : hometopbackcolor </remarks>
            /// </summary>
            public string HomePageTopBgColor
            {
                get
                {
                    return this._HomePageTopBgColor;
                }
                set
                {
                    this._HomePageTopBgColor = value;
                }
            }

            private string _HomePageMiddleBgColor = "#ffffff";
            /// <summary>
            /// Gets or sets the home page middle background color.
            /// <remarks> Key : homemiddlebackcolor </remarks>
            /// </summary>
            public string HomePageMiddleBgColor
            {
                get
                {
                    return this._HomePageMiddleBgColor;
                }
                set
                {
                    this._HomePageMiddleBgColor = value;
                }
            }

            private string _HomePageBottomBgColor = "#999999";
            /// <summary>
            /// Gets or sets the home page bottom background color.
            /// <remarks> Key : homebottombackcolor </remarks>
            /// </summary>
            public string HomePageBottomBgColor
            {
                get
                {
                    return this._HomePageBottomBgColor;
                }
                set
                {
                    this._HomePageBottomBgColor = value;
                }
            }

            private string _HomePageHeadColor = "#ffffff";
            /// <summary>
            /// Gets or sets the home page header fore ground color.
            /// <remarks> Key : homeheadforecolor </remarks>
            /// </summary>
            public string HomePageHeadColor
            {
                get
                {
                    return this._HomePageHeadColor;
                }
                set
                {
                    this._HomePageHeadColor = value;
                }
            }

            private string _HomePageHeadOptionColor = "#ffffff";
            /// <summary>
            /// Gets or sets the home page header fore ground color.
            /// <remarks> Key : homeheadoptionforecolor </remarks>
            /// </summary>
            public string HomePageHeadOptionColor
            {
                get
                {
                    return this._HomePageHeadOptionColor;
                }
                set
                {
                    this._HomePageHeadOptionColor = value;
                }
            }

            private string _HomePageBottomOptionColor = "#ffffff";
            /// <summary>
            /// Gets or sets the home page bottom color.
            /// <remarks> Key : homeheadoptionforecolor </remarks>
            /// </summary>
            public string HomePageBottomOptionColor
            {
                get
                {
                    return this._HomePageBottomOptionColor;
                }
                set
                {
                    this._HomePageBottomOptionColor = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- Bottom Panel -- "

        /// <summary>
        /// Class to for bottom panel information
        /// </summary>
        public class BottomPanelSetting
        {
            #region " -- Constructor -- "
            public BottomPanelSetting()
            {
                //////Add default buttons.
                ////BottomPanelButton bpButton1 = new BottomPanelButton("disetup", "diSetup", true, 1, "disetup_normal.png", "disetup_hover.png", BottomPanelButton.ActionType.Custom, "");
                ////this._BottomPanelButtons.Add(bpButton1);

                ////BottomPanelButton bpButton2 = new BottomPanelButton("diebook", "diEbook", true, 2, "diebook _normal.png", "diebook_hover.png", BottomPanelButton.ActionType.Exe, "osk.exe");
                ////this._BottomPanelButtons.Add(bpButton2);

                ////BottomPanelButton bpButton3 = new BottomPanelButton("diregistry", "diRegistry", true, 3, "diregistry_normal.png", "diregistry_hover.png", BottomPanelButton.ActionType.Exe, "mspaint.exe");
                ////this._BottomPanelButtons.Add(bpButton3);

                ////BottomPanelButton bpButton4 = new BottomPanelButton("dianalyzer", "diAnalyzer", true, 4, "dianalyzer_normal.png", "dianalyzer_hover.png", BottomPanelButton.ActionType.Exe, "notepad.exe");
                ////this._BottomPanelButtons.Add(bpButton4);

                ////BottomPanelButton bpButton5 = new BottomPanelButton("divideo", "diVideo", true, 5, "divideo_normal.png", "divideo_hover.png", BottomPanelButton.ActionType.Exe, "mshearts.exe");
                ////this._BottomPanelButtons.Add(bpButton5);


            }

            #endregion

            #region " -- Properties -- "

            private bool _DisplayTextAsLabel = false;
            /// <summary>
            /// Gets or sets boolean value to decide whether button text is to be displayed as label or tooltip
            /// </summary>
            public bool DisplayTextAsLabel
            {
                get { return _DisplayTextAsLabel; }
                set { _DisplayTextAsLabel = value; }
            }

            private GenericCollection<BottomPanelButton> _BottomPanelButtons = new GenericCollection<BottomPanelButton>();
            /// <summary>
            /// Bottom panel buttons collection
            /// </summary>
            public GenericCollection<BottomPanelButton> Buttons
            {
                get { return _BottomPanelButtons; }
                set { _BottomPanelButtons = value; }
            }

            #endregion

            #region " -- Methods -- "

            /// <summary>
            /// Get Button Id for new button so that it doesnt conflict with existing button ids
            /// Naming convention for new button id, shall be "BottomPanelButton#"
            /// </summary>
            /// <returns></returns>
            public string GetNewButtonId()
            {
                string RetVal = string.Empty;
                BottomPanelButton bpButton = null;
                int UnusedButtonIndex = 1;
                while (true)
                {
                    RetVal = "BottomPanelButton" + UnusedButtonIndex.ToString();
                    bpButton = this._BottomPanelButtons["ID", RetVal];
                    if (bpButton == null)
                    {
                        break;
                    }
                    UnusedButtonIndex += 1;
                }
                return RetVal;
            }

            /// <summary>
            ///  Get display order for new button, so that it is one greater than the maximum display order of existing buttons
            /// </summary>
            /// <returns></returns>
            public int GetNewDisplayOrder()
            {
                int RetVal = 0;
                foreach (BottomPanelButton bpButton in this._BottomPanelButtons)
                {
                    RetVal = Math.Max(RetVal, bpButton.DisplayOrder);
                }
                RetVal = RetVal + 1;
                return RetVal;
            }

            #endregion
        }


        /// <summary>
        /// TODO
        /// </summary>
        public class BottomPanelButton
        {

            /// <summary>
            /// Defines the action on button click. It may be running a satellite exe, invoking a url or opening dialog interface
            /// </summary>
            public enum ActionType
            {
                /// <summary>
                /// Run a satellite application or plugins
                /// </summary>
                Exe = 0,

                /// <summary>
                /// invoking a url in default browser
                /// </summary>
                Url = 1,

                /// <summary>
                /// Open dialog interface
                /// </summary>
                Custom
            }

            #region " -- Constructor -- "

            /// <summary>
            /// Default constructor for serialization
            /// </summary>
            public BottomPanelButton()
            {
            }

            /// <summary>
            /// Parameterized constructor for button creation
            /// </summary>
            /// <param name="iD">Button id</param>
            /// <param name="text">Button text</param>
            /// <param name="visible">Visible</param>
            /// <param name="displayOrder">Display order</param>
            /// <param name="normalImage">Normal image name with extension</param>
            /// <param name="hoverImage">Hover over image name with extension</param>
            /// <param name="actionType">Action type. Exe, url or custom</param>
            /// <param name="actionInfo">Action information. Exe name, url link or blank for custom action </param>
            public BottomPanelButton(string iD, string text, bool visible, int displayOrder, string normalImage, string hoverImage, ActionType actionType, string actionInfo)
            {
                this.ID = iD;
                this.Text = text;
                this.Visible = visible;
                this.DisplayOrder = displayOrder;
                this.NormalImage = normalImage;
                this.HoverImage = hoverImage;
                this.Action = actionType;
                this.ActionInfo = actionInfo;
            }

            #endregion

            #region " -- Properties -- "

            private string _ID = string.Empty;
            /// <summary>
            /// Gets or sets unique button id
            /// </summary>
            /// <remarks>While adding buttons to collection DA Customize module shall ensure the uniqueness of Button Ids</remarks>
            public string ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            private bool _Mandatory = false;
            /// <summary>
            /// Gets or sets the Mandatory status of button.
            /// Button like diSettings, diHelp etc will allways be part application and hence mandatory. Status of mandatory buttons will be set manually in master adaptaion.xml
            /// Custom buttons will be optional (non mandatory)
            /// </summary>
            /// <remarks>It will be set manually in master - Customize module to hide or display predefined buttons </remarks>
            public bool Mandatory
            {
                get { return _Mandatory; }
                set { _Mandatory = value; }
            }


            private string _Text = string.Empty;
            /// <summary>
            /// Gets or sets button text to be displayed as caption or tooltip.
            /// </summary>
            /// <remarks>
            /// This text will be language independent and set in DA - customize module.
            /// </remarks>
            public string Text
            {
                get { return _Text; }
                set { _Text = value; }
            }

            private bool _Visible = true;
            /// <summary>
            /// Gets or sets the visibility of button.
            /// </summary>
            /// <remarks>It will be set in DA - Customize module to hide or display predefined buttons </remarks>
            public bool Visible
            {
                get { return _Visible; }
                set { _Visible = value; }
            }

            private int _DisplayOrder;
            /// <summary>
            /// Gets or sets the display order of the button 
            /// </summary>
            /// <remarks>
            /// Display order should be considered only for all visible buttons in collection
            /// </remarks>
            public int DisplayOrder
            {
                get { return _DisplayOrder; }
                set { _DisplayOrder = value; }
            }

            private string _NormalImage = string.Empty;
            /// <summary>
            /// Gets or sets button normal image name with extension
            /// </summary>
            public string NormalImage
            {
                get { return _NormalImage; }
                set { _NormalImage = value; }
            }

            private string _HoverImage = string.Empty;
            /// <summary>
            /// Gets or sets button hover over image name with extension
            /// </summary>
            public string HoverImage
            {
                get { return _HoverImage; }
                set { _HoverImage = value; }
            }

            private ActionType _Action = ActionType.Custom;
            /// <summary>
            /// Gets or sets button action type. Exe, url or custom 
            /// </summary>
            public ActionType Action
            {
                get { return _Action; }
                set { _Action = value; }
            }

            private string _ActionInfo = string.Empty;
            /// <summary>
            /// Gets or sets action information which may be the exe name, url link or may be left blank for custom action (client application to decide action based on button id)
            /// </summary>
            public string ActionInfo
            {
                get { return _ActionInfo; }
                set { _ActionInfo = value; }
            }

            #endregion
        }



        #endregion

        #endregion
    }
}
