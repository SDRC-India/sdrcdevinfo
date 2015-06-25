using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Gallery.Search.Search;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{      
    /// <summary>
    /// Class builds and maintain the collection of wizard panel, maintains the history.
    /// </summary>
    public class DIWizard
    {

        #region " -- Enum -- "

        /// <summary>
        /// Enum to define the type DataWizard to be used.
        /// </summary>
        public enum DIDataWizardType
        {
            HomePage,
            SelectionPage,
            Filters,
            Dataview
        }

        /// <summary>
        /// Enum to define the page to be navigated while generating the dataview.
        /// </summary>
        public enum NavigationPage
        {
            Indicator,
            Area,
            TimePeriod,
            Source,
            IUSFilter,
            TimePeriodFilter,
            SourceFilter
        }

        /// <summary>
        /// Enum to define the filter type.
        /// </summary>
        public enum Filters
        { 
            IUSFilter,            
            SourceFilter,
            TimePeriodFilter
        }

        #endregion
  
        #region " -- Language Strings -- "

        /// <summary>
        /// contains the constants of language strings
        /// </summary>
        private static class LanguageStrings
        {
            /// <summary>
            /// GALLERIES
            /// </summary>
            public static string Galleries = "GALLERIES";

            /// <summary>
            /// PRESENTATIONS
            /// </summary>
            public static string Presentations = "PRESENTATIONS";

            /// <summary>
            /// COUNT
            /// </summary>
            public static string Count = "COUNT";

            /// <summary>
            /// DATABASE
            /// </summary>
            public static string Database = "DATA";

            /// <summary>
            /// SELECTED
            /// </summary>
            public static string Selected = "SELECTED";

            /// <summary>
            /// WHERE DATA EXISTS
            /// </summary>
            public static string Home_Page_Selected_Caption = "WHERE DATA EXISTS";

            /// <summary>
            /// TABLES
            /// </summary>
            public static string Tables = "TABLES";

            /// <summary>
            /// GRAPHS
            /// </summary>
            public static string Graphs = "GRAPHS";

            /// <summary>
            /// MAPS
            /// </summary>
            public static string Maps = "MAPS";

            /// <summary>
            /// VIEW GALLERY
            /// </summary>
            public static string ViewGallery = "VIEW GALLERY";

            /// <summary>
            /// DATA
            /// </summary>
            public static string ViewData = "VIEW DATA";

            /// <summary>
            /// INDICATORS
            /// </summary>
            public static string Indicators = "SELECT INDICATORS";

            /// <summary>
            /// AREAS
            /// </summary>
            public static string Areas = "SELECT AREAS";

            /// <summary>
            /// TIME PERIODS
            /// </summary>
            public static string TimePeriods = "SELECT TIME PERIODS";

            /// <summary>
            /// SOURCES
            /// </summary>
            public static string Sources = "SELECT SOURCES";

            /// <summary>
            /// Available
            /// </summary>
            public static string Available = "Available";

            /// <summary>
            /// Selected – Most Recent Data
            /// </summary>
            public static string Selected_MRD = "Selected – Most Recent Data";

            /// <summary>
            /// Indicator
            /// </summary>
            public static string Indicator = "Indicator";

            /// <summary>
            /// TimePeriod
            /// </summary>
            public static string TimePeriod = "TimePeriod";

            /// <summary>
            /// Area
            /// </summary>
            public static string Area = "Area";

            /// <summary>
            /// Source
            /// </summary>
            public static string Source = "Source";

        }


        #endregion
                
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="pref"></param>
        /// <param name="stockFolderPath"></param>
        public DIWizard(DIConnection dIConnection, DIQueries dIQueries, ref UserPreference pref,string stockFolderPath)
        {
            // -- Set DAL connection object
            this.DbConnection = dIConnection;

            // -- Set DAL query object
            this.DbQueries = dIQueries;

            // -- Set the user preference object
            this._UserPreference = pref;

            // -- Create the instance of panel collection 
            this._DIWizardPanels = new DIWizardPanels();

            // -- Intialize the panels.
            this.IntializePanel();

            // -- Intialize the navigation history property.
            this._NavigationHistory = new List<string>();

            // -- Intialize the selection order history.
            this.SelectionOrder = new List<string>();

            this.StockFolderPath = stockFolderPath;

            //-- Create the selection list.
            this.BuildSelectionList();

            //-- Create the selection count list.
            this.BuildSelectionCountList();

            if (!Directory.Exists(Path.Combine(this.StockFolderPath, Constants.CACHE)))
            {
                // -- Create the directory, if not exists
                Directory.CreateDirectory(Path.Combine(this.StockFolderPath, Constants.CACHE));
            }

            // -- Create the database cache folder.
            this.CreateDbFolder();

            // -- check for the existence of Diwizard.xml
            if (!File.Exists(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME)))
            {
                // -- If not found create the file
                this.SaveRecordCount();
            }
            else
            {
                // -- If found, retrieve the record
                this.AvailableNIdCache = CacheInfo.Load(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME));
                if (this.AvailableNIdCache == null)
                {
                    this.AvailableNIdCache = new CacheInfo();
                    // -- If the file is not properly saved.
                    this.SaveRecordCount();
                }
            }

            //if (File.Exists(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME)))
            //{
            //    //-- get the avialabel NIds
            //    this.GetAvailableRecords(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME));
            //}

        }

        #endregion

        #region " -- Properties / Variables -- "

        private DIWizardPanels _DIWizardPanels;
        /// <summary>
        /// Gets or sets the diwizardpanel
        /// </summary>
        public DIWizardPanels DIWizardPanels
        {
            get
            {
                return this._DIWizardPanels;
            }
            set
            {
                this._DIWizardPanels = value;
            }
        }     

        private string _LanguageFilePath = string.Empty;
        /// <summary>
        /// Gets or sets the language file path.
        /// </summary>
        public string LanguageFilePath
        {
            get
            {
                return this._LanguageFilePath;
            }
            set
            {
                this._LanguageFilePath = value;
                this.ApplyLanguageSettings();
            }
        }

        private string _DataFolderPath = string.Empty;
        /// <summary>
        /// Gets or sets the data folder path.
        /// </summary>
        public string DataFolderPath
        {
            get
            {
                return this._DataFolderPath;
            }
            set
            {
                this._DataFolderPath = value;
            }
        }

        private UserPreference _UserPreference;
        /// <summary>
        /// Gets or sets the user pref object
        /// </summary>
        public UserPreference UserPreference
        {
            get 
            {
                return this._UserPreference; 
            }
            set 
            {
                this._UserPreference = value; 
            }
        }	

        private string _PresentationFolderPath = string.Empty;
        /// <summary>
        /// Gets or sets the presentation folder path
        /// </summary>
        public string PresentationFolderPath
        {
            get
            {
                return this._PresentationFolderPath;
            }
            set
            {
                this._PresentationFolderPath = value;
            }
        } 
	
        private DIWizardPanel.PanelType _SelectedPage;
        /// <summary>
        /// Gets or sets the selected wizard panel.
        /// </summary>
        public DIWizardPanel.PanelType SelectedPage
        {
            get 
            {
                return this._SelectedPage; 
            }
            set 
            {
                this._SelectedPage = value;
            }
        }

        private List<string> _NavigationHistory;
        /// <summary>
        /// Gets or sets the navigation history.
        /// </summary>
        public List<string> NavigationHistory
        {
            get 
            {
                return this._NavigationHistory; 
            }
            set 
            {
                this._NavigationHistory = value; 
            }
        }

        private List<string> _SelectionOrder;
        /// <summary>
        /// Collection to maintain the selection order of navigation.
        /// </summary>
        public List<string> SelectionOrder
        {
            get
            {
                return this._SelectionOrder; 
            }
            set 
            {
                this._SelectionOrder = value; 
            }
        }

        private bool _IsFilterPage=false;
        /// <summary>
        /// Gets or sets the filter page.
        /// </summary>
        public bool IsFilterPage
        {
            get 
            {
                return this._IsFilterPage; 
            }
            set 
            {
                this._IsFilterPage = value; 
            }
        }

        private Filters _SelectedFilter;
        /// <summary>
        /// Gets or sets the selected filter.
        /// </summary>
        public Filters SelectedFilter
        {
            get 
            { 
                return this._SelectedFilter; 
            }
            set 
            { 
                this._SelectedFilter = value; 
            }
        }

		private Dictionary<DIWizardPanel.PanelType, string> _AutoSelectRecords = new Dictionary<DIWizardPanel.PanelType, string>();
        /// <summary>
        /// Contains the auto select NIds of panels
        /// </summary>
        public Dictionary<DIWizardPanel.PanelType, string> AutoSelectRecords
        {
            get 
            {
                return this._AutoSelectRecords; 
            }
            set 
            {
                this._AutoSelectRecords = value; 
            }
        }

        //private Dictionary<DIWizardPanel.PanelType, string> _AvailableNIds;
        ///// <summary>   
        ///// Contains the availabe NIds of the panel
        ///// </summary>
        //public Dictionary<DIWizardPanel.PanelType, string> AvailableNIds
        //{
        //    get
        //    {
        //        return this._AvailableNIds;
        //    }
        //    set
        //    {
        //        this._AvailableNIds = value;
        //    }
        //}

        private Dictionary<DIWizardPanel.PanelType, bool> _SelectionStatus;
        /// <summary>
        /// Get or sets the selection panel status.
        /// </summary>
        /// <remarks> If the value is true, then the selection panel list is repopulated. </remarks>
        public Dictionary<DIWizardPanel.PanelType, bool> SelectionStatus
        {
            get
            {
                return this._SelectionStatus;
            }
            set
            {
                this._SelectionStatus = value;
            }
        }

        private Dictionary<DIWizardPanel.PanelType, string> _SelectionCount = new Dictionary<DIWizardPanel.PanelType, string>();
        /// <summary>
        /// Contains the NIds count of panels
        /// </summary>
        public Dictionary<DIWizardPanel.PanelType, string> SelectionCount
        {
            get
            {
                return this._SelectionCount;
            }
            set
            {
                this._SelectionCount = value;
            }
        }

        private bool _IsValidIUSFilter = true;
        /// <summary>
        /// Gets or sets the validity of IUS filter
        /// </summary>
        public bool IsValidIUSFilter
        {
            get
            {
                return this._IsValidIUSFilter;
            }
            set
            {
                this._IsValidIUSFilter = value;
            }
        }

        private bool _IsValidSourceFilter = true;
        /// <summary>
        /// Gets or sets the validity of source filter.
        /// </summary>
        public bool IsValidSourceFilter
        {
            get 
            {
                return this._IsValidSourceFilter; 
            }
            set 
            {
                this._IsValidSourceFilter = value; 
            }
        }

        private int _SelectedAreaLevel = 2;
        /// <summary>
        /// Gets or sets the selected area level
        /// </summary>
        public int SelectedAreaLevel
        {
            get 
            {
                return this._SelectedAreaLevel; 
            }
            set 
            {
                this._SelectedAreaLevel = value; 
            }
        }

        private string _GalleryKeywords = string.Empty;
        /// <summary>
        /// Get the Gallery Keywords
        /// </summary>
        public string GalleryKeywords
        {
            get
            {
                return this._GalleryKeywords;
            }
        }

        private string _GalleryDatabaseFileWithPath = string.Empty;
        /// <summary>
        /// Set the gallery database file with path
        /// </summary>
        public string GalleryDatabaseFileWithPath
        {
            set 
            { 
                this._GalleryDatabaseFileWithPath = value; 
            }
        }	

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Set the DWPanel captions.
        /// </summary>
        public void SetDataWizardPanels()
        {
            //-- TODO use iformatprovider with tostring()
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            #region " -- Gallery -- "

           
            #endregion

            this.SetSelectedIndicatorPanel();

            this.SetSelectedAreaPanel();

            this.SetSelectedTimePeriodPanel();

            this.SetSelectedSourcePanel();

            //// -- Set the indicator panel properties.
            //this.SetIndicatorPanel(this.DIWizardPanels[DIWizardPanel.PanelType.Indicator]);

            //// -- Set the area panel properties.
            //this.SetAreaPanel(this.DIWizardPanels[DIWizardPanel.PanelType.Area]);

            //// -- Set the time period panel properties.
            //this.SetTimePeriodPanel(this.DIWizardPanels[DIWizardPanel.PanelType.TimePeriod]);

            //// -- Set the source panel properties.
            //this.SetSourcePanel(this.DIWizardPanels[DIWizardPanel.PanelType.Source]);

            // -- Set the Database panel properties.
            this.SetDatabasePanel(this.DIWizardPanels[DIWizardPanel.PanelType.Database]);

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }
     
        /// <summary>
        /// Maintain the history of navigation.
        /// </summary>
        public void History()
        {
            if (!this._IsFilterPage)
            {

                //this.UpdateHistory();

                // -- DW option is displayed.
                switch (this._SelectedPage)
                {
                    case DIWizardPanel.PanelType.Indicator:
                        if (this._UserPreference.UserSelection.IndicatorNIds.Length != 0)
                        {
                            // -- Add the selected page in the history.
                            this.AddToHistory(this._SelectedPage.ToString());
                            // -- Maintain the navigation order of the user.
                            this.NavigationOrder();
                        }
                        break;
                    case DIWizardPanel.PanelType.Area:
                        if (this._UserPreference.UserSelection.AreaNIds.Length != 0)
                        {
                            // -- Add the selected page in the history.
                            this.AddToHistory(this._SelectedPage.ToString());
                            // -- Maintain the navigation order of the user.
                            this.NavigationOrder();
                        }
                        break;
                    case DIWizardPanel.PanelType.TimePeriod:
                        if (this._UserPreference.UserSelection.TimePeriodNIds.Length != 0 || this._UserPreference.UserSelection.DataViewFilters.MostRecentData)
                        {
                            // -- Add the selected page in the history.
                            this.AddToHistory(this._SelectedPage.ToString());
                            // -- Maintain the navigation order of the user.
                            this.NavigationOrder();
                        }
                        break;
                    case DIWizardPanel.PanelType.Source:
                        if (this._UserPreference.UserSelection.SourceNIds.Length != 0)
                        {
                            // -- Add the selected page in the history.
                            this.AddToHistory(this._SelectedPage.ToString());
                            // -- Maintain the navigation order of the user.
                            this.NavigationOrder();
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (this._IsFilterPage)
            {
                switch (this._SelectedFilter)
                {
                    //case Filters.IUSFilter:
                    //    if (this._UserPreference.UserSelection.IndicatorNIds.Length != 0 && this._IsValidIUSFilter)
                    //    {
                    //        // -- Add the selected filter in the history.
                    //        this.AddToHistory(this._SelectedFilter.ToString());
                    //        // -- Maintain the navigation order of the user.
                    //        this.NavigationOrder();
                    //    }
                    //    break;
                    //case Filters.TimePeriodFilter:
                    //    if (this._UserPreference.UserSelection.TimePeriodNIds.Length != 0)
                    //    {
                    //        // -- Add the selected filter in the history.
                    //        this.AddToHistory(this._SelectedFilter.ToString());
                    //        // -- Maintain the navigation order of the user.
                    //        this.NavigationOrder();
                    //    }
                    //    break;
                    case Filters.SourceFilter:
                        if (this._IsValidSourceFilter)
                        {
                            // -- Add the selected filter in the history.
                            this.AddToHistory(this._SelectedFilter.ToString());
                            // -- Maintain the navigation order of the user.
                            this.NavigationOrder();
                        }
                        break;
                    default:
                        break;
                }
            }            
        } 

        /// <summary>
        /// Get the most recent history. 
        /// </summary>
        /// <param name="wizardType"></param>
        /// <param name="filterPage"></param>
        /// <returns></returns>
        public string GetHistoryPage(DIDataWizardType wizardType, bool filterPage)
        {
            string Retval = string.Empty;
            try
            {
                //if (this.IsFilterDeletedFromHistory)
                //{
                //    // -- Deleted last entry from the history
                //    this._NavigationHistory.RemoveAt(this._NavigationHistory.Count - 1);
                //    this.IsFilterDeletedFromHistory = false;
                //    try
                //    {
                //        DIWizardPanel.PanelType PanelPage = (DIWizardPanel.PanelType)Enum.Parse(typeof(DIWizardPanel.PanelType), this._NavigationHistory[this._NavigationHistory.Count - 1]);
                //        this._IsFilterPage = false;
                //        filterPage = false;
                //    }
                //    catch (Exception)
                //    {                       
                //    }
                //}

                // -- Get the most recent history.
                if (this._NavigationHistory.Count > 0)
                {
                    Retval = this._NavigationHistory[this._NavigationHistory.Count - 1];
                }
                else if (this._NavigationHistory.Count == 0)
                {
                    this._IsFilterPage = false;
                    Retval = Constants.DWOPTIONPAGE;
                }

                if (wizardType == DIDataWizardType.Filters && this._IsValidSourceFilter)
                {
                    try
                    {
                        //-- Reset the setting, so that DW option page will be displayed.
                        DIWizardPanel.PanelType PanelPage = (DIWizardPanel.PanelType)Enum.Parse(typeof(DIWizardPanel.PanelType), this._NavigationHistory[this._NavigationHistory.Count - 1]);
                        this._IsFilterPage = false;
                        wizardType = DIDataWizardType.SelectionPage;
                        filterPage = false;
                    }
                    catch (Exception)
                    {
                    }                   
                }

                if (!filterPage)
                {
                    // -- Convert the panel type enum into the array list.
                    ArrayList PanelType = this.EnumToArray();

                    // -- current page is not the DW option page
                    foreach (string Fields in PanelType)
                    {
                        if (Fields.ToLower() == Retval.ToLower())
                        {
                            if (wizardType == DIDataWizardType.SelectionPage)
                            {
                                Retval = Constants.DWOPTIONPAGE;
                                break;
                            }
                        }
                    }
                    //if (!string.IsNullOrEmpty(this.IndicatorNIds))
                    //{
                    //    this._UserPreference.UserSelection.IndicatorNIds = this.IndicatorNIds;
                    //    this._UserPreference.UserSelection.ShowIUS = false;
                    //    this.IndicatorNIds = string.Empty;
                    //}
                }

                if (this._NavigationHistory.Count > 0 && Retval != Constants.DWOPTIONPAGE)
                {
                    this._NavigationHistory.RemoveAt(this._NavigationHistory.Count - 1);
                }

                try
                {
                    //-- Update the selection status list
                    this.UpdateSelectionList((DIWizardPanel.PanelType)Enum.Parse(typeof(DIWizardPanel.PanelType), Retval));
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;

        }

        /// <summary>
        /// Reset the history and user selection.
        /// </summary>
        public void Reset()
        {
            // -- Reset the history of navigation.
            this._NavigationHistory = new List<string>();

            // -- Reset the selection order.
            this.SelectionOrder = new List<string>();

            // -- Reset the selections.
            this._UserPreference.UserSelection.Reset();
            this._UserPreference.UserSelection.DataViewFilters.MostRecentData = true;

            //-- Create the selection list.
            this.BuildSelectionList();

            //-- Create the selection count list.
            this.BuildSelectionCountList();

            // -- Create the instance of panel collection 
            this._DIWizardPanels = new DIWizardPanels();

            // -- Intialize the panels.
            this.IntializePanel();

            // -- Create the database cache folder.
            this.CreateDbFolder();

            this._SelectionOrder.Clear();

            this._IsValidSourceFilter = true;

            this.IndicatorNIds = string.Empty;

            this._GalleryKeywords = string.Empty;

            this._SelectedAreaLevel = 2;
        }
     
        /// <summary>
        /// Render the panels after the selections.
        /// </summary>
        public void RenderPanels()
        {
            if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds) || !string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds) || !string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) || !string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
            {
                this.SaveAutoSelect();
            }
            else
            {
                this.AutoSelectRecordCount = new Dictionary<DIWizardPanel.PanelType, int>();
                this._AutoSelectRecords = new Dictionary<DIWizardPanel.PanelType, string>();
            }

            // -- filter entry will not be deleted from the history.
            this.IsFilterDeletedFromHistory = false;
            // -- Set the wizard panel captions properties
            this.SetDataWizardPanels();

            int Order = 0;

            foreach (string Panels in this.SelectionOrder)
            {
                switch (Panels)
                {
                    case INDICATOR:
                        //if (this.UserPreference.UserSelection.IndicatorNIds.Length != 0)
                        //{
                            this.DIWizardPanels[DIWizardPanel.PanelType.Indicator].Selected = true;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Indicator].Order = Order;
                            Order += 1;
                        //}

                        break;
                    case AREA:
                        //if (this.UserPreference.UserSelection.AreaNIds.Length != 0)
                        //{
                            this.DIWizardPanels[DIWizardPanel.PanelType.Area].Selected = true;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Area].Order = Order;
                            Order += 1;
                        //}
                        break;
                    case TIMEPERIOD:
                        //if (this.UserPreference.UserSelection.TimePeriodNIds.Length != 0)
                        //{
                            this.DIWizardPanels[DIWizardPanel.PanelType.TimePeriod].Selected = true;
                            this.DIWizardPanels[DIWizardPanel.PanelType.TimePeriod].Order = Order;
                            Order += 1;
                        //}
                        break;
                    case SOURCE:
                        //if (this.UserPreference.UserSelection.SourceNIds.Length != 0)
                        //{
                            this.DIWizardPanels[DIWizardPanel.PanelType.Source].Selected = true;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Source].Order = Order;
                            Order += 1;
                        //}
                        break;
                    default:
                        break;
                }                
            }

            // -- Update the selection order on the basis of selection done by the user
            Order = this.UpdateSelectionOrder(Order);

            // -- Render the unselected panels
            this.UnSelectedPanel(Order);

            // -- Set the background image of the panel
            this.BackgroundImage(null);
        
        }

        /// <summary>
        /// True, if history is available.
        /// </summary>
        /// <returns></returns>
        public bool IsHistoryAvailable(DIDataWizardType wizardType)
        {
            bool Retval = true;
            try
            {
                if (wizardType == DIDataWizardType.HomePage && this._NavigationHistory.Count == 0)
                {
                    Retval = false;
                }
                else
                {
                    Retval = true;
                }
            }
            catch (Exception)
            {
                Retval = true;
            }
            return Retval;
        }

        /// <summary>
        /// Convert enum into array list.
        /// </summary>
        /// <returns></returns>
        public ArrayList EnumToArray()
        {
            ArrayList Retval = new ArrayList();
            try
            {
                System.Reflection.FieldInfo[] FieldInfos = typeof(DIWizardPanel.PanelType).GetFields();
                foreach (System.Reflection.FieldInfo Field in FieldInfos)
                {
                    Retval.Add(Field.Name);
                }
                Retval.RemoveAt(0);
            }
            catch (Exception)
            {
                Retval = null;
            }
            return Retval;
        }

        /// <summary>
        /// Get the next valid filter page.
        /// </summary>
        /// <returns></returns>
        public string ValidFilterPage()
        {
            string Retval = string.Empty;
            try
            {
                Array FiltersArray = Enum.GetValues(typeof(Filters));
                for (int FilterIndex = 0; FilterIndex < FiltersArray.Length && string.IsNullOrEmpty(Retval) ; FilterIndex++)
                {
                    switch (FilterIndex)
                    {
                        //case (int)Filters.IUSFilter:
                        //    if (this._NavigationHistory.IndexOf(Filters.IUSFilter.ToString()) == -1)
                        //    {
                        //        if (this._IsValidIUSFilter && this.ValidIUSFilter())
                        //        {
                        //            Retval = Filters.IUSFilter.ToString();
                        //            this.IsFilterDeletedFromHistory = true;
                        //        }
                        //        else
                        //        {
                        //            if (!this.UserPreference.UserSelection.ShowIUS)
                        //            {
                        //                this.ValidIUSFilter();
                        //            }
                        //        }
                        //    }
                        //    break;
                        //case (int)Filters.TimePeriodFilter:
                        //    if (this._NavigationHistory.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString()) == -1 && this._NavigationHistory.IndexOf(Filters.TimePeriodFilter.ToString()) == -1)
                        //    {
                        //        StringBuilder TimePeriods = new StringBuilder();
                        //        int RecordCount = 0;
                        //        this._UserPreference.UserSelection.TimePeriodNIds = string.Empty;
                        //        IDataReader TimePeriodReader = this.DbConnection.ExecuteReader(this.DbQueries.Timeperiod.GetAutoSelectTimeperiod(this._UserPreference.UserSelection.IndicatorNIds, this._UserPreference.UserSelection.ShowIUS, this._UserPreference.UserSelection.AreaNIds, this._UserPreference.UserSelection.TimePeriodNIds));

                        //        while (TimePeriodReader.Read())
                        //        {
                        //            TimePeriods.Append("," + TimePeriodReader[Timeperiods.TimePeriodNId].ToString());
                        //            RecordCount++;
                        //        }

                        //        TimePeriodReader.Close();

                        //        if (TimePeriods.Length > 0)
                        //        {
                        //            this._UserPreference.UserSelection.TimePeriodNIds = TimePeriods.ToString().Substring(1);
                        //        }
                        //        else
                        //        {
                        //            this._UserPreference.UserSelection.TimePeriodNIds = string.Empty;
                        //        }

                        //        if (RecordCount > 1)
                        //        {
                        //            Retval = Filters.TimePeriodFilter.ToString();
                        //            this.IsFilterDeletedFromHistory = true;
                        //        }
                        //    }
                        //    break;
                        case (int)Filters.SourceFilter:
                            if (this._NavigationHistory.IndexOf(DIWizardPanel.PanelType.Source.ToString()) == -1 && this._NavigationHistory.IndexOf(Filters.SourceFilter.ToString()) == -1 && this._IsValidSourceFilter)
                            {
                                StringBuilder Sources = new StringBuilder();
                                int RecordCount = 0;

                                IDataReader SourceReader = this.DbConnection.ExecuteReader(this.DbQueries.Source.GetAutoSelectSource(this._UserPreference.UserSelection.IndicatorNIds, this._UserPreference.UserSelection.ShowIUS, this._UserPreference.UserSelection.AreaNIds, this._UserPreference.UserSelection.TimePeriodNIds));
                                while (SourceReader.Read())
                                {
                                    Sources.Append("," + SourceReader[IndicatorClassifications.ICNId].ToString());
                                    RecordCount++;
                                }

                                SourceReader.Close();

                                if (Sources.Length > 0)
                                {
                                    this._UserPreference.UserSelection.SourceNIds = Sources.ToString().Substring(1);
                                }
                                else
                                {
                                    this._UserPreference.UserSelection.SourceNIds = string.Empty;
                                }

                                if (RecordCount > 1)
                                {
                                    Retval = Filters.SourceFilter.ToString();
                                    this.IsFilterDeletedFromHistory = true;
                                }
                                else
                                {
                                    this._IsValidSourceFilter = false;
                                }
                            }                           
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Update the selection status list.
        /// </summary>
        /// <param name="panelType"></param>
        public void UpdateSelectionList(DIWizardPanel.PanelType panelType)
        {
            int SelectedOrder = this.DIWizardPanels[panelType].Order;
            ArrayList PanelTypes = new ArrayList();
            PanelTypes = this.EnumToArray();
            for (int PanelIndex = 0; PanelIndex < PanelTypes.Count - 2; PanelIndex++)
            {
                if (this.DIWizardPanels[PanelIndex].Order > SelectedOrder)
                {
                    this._SelectionStatus[(DIWizardPanel.PanelType)PanelIndex] = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelType"></param>
        /// <returns></returns>
        public string GetNids(DIWizardPanel.PanelType panelType)
        {
            string Retval = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds) && string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds) && string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
                {
                    switch (panelType)
                    {
                        case DIWizardPanel.PanelType.Database:
                            break;
                        case DIWizardPanel.PanelType.Indicator:
                            Retval = this.AvailableNIdCache.IndicatorNIds;
                            break;
                        case DIWizardPanel.PanelType.Area:
                            Retval = this.AvailableNIdCache.AreaNIds;
                            break;
                        case DIWizardPanel.PanelType.TimePeriod:
                            Retval = this.AvailableNIdCache.TimeperiodNIds;
                            break;
                        case DIWizardPanel.PanelType.Source:
                            Retval = this.AvailableNIdCache.SourceNIds;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Retval=this._AutoSelectRecords[panelType];
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Update the selected Area NId on the basis of selected area level.
        /// </summary>
        public void UpdateAreaNIds()
        {            
            if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.AreaNIds))
            {
                IDataReader AreaDataReader;
                AreaDataReader = this.DbConnection.ExecuteReader(this.DbQueries.Area.GetArea(FilterFieldType.NId, this.UserPreference.UserSelection.AreaNIds));
                this.UserPreference.UserSelection.AreaNIds = string.Empty;
                while (AreaDataReader.Read())
                {
                    if (Convert.ToInt32(AreaDataReader[Area.AreaLevel]) == this._SelectedAreaLevel)
                    {
                        this.UserPreference.UserSelection.AreaNIds += "," + AreaDataReader[Area.AreaNId].ToString();
                    }                   
                }
                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.AreaNIds))
                {
                    this.UserPreference.UserSelection.AreaNIds = this.UserPreference.UserSelection.AreaNIds.Substring(1);
                }
                AreaDataReader.Close();
            }
        }


        /// <summary>
        /// Convert IUS NIds to Indicator NIds
        /// </summary>
        //public void IndicatorNidsFromIUSNIds()
        //{
        //    // -- convert IUS NIds into Indicator NIds
        //    if (this.UserPreference.UserSelection.ShowIUS && !string.IsNullOrEmpty(this.UserPreference.UserSelection.IndicatorNIds))
        //    {
        //        DataTable IndicatorDt = this.GetIUS();
        //        this.UserPreference.UserSelection.IndicatorNIds = string.Empty;

        //        foreach (DataRow Row in IndicatorDt.Rows)
        //        {
        //            this.UserPreference.UserSelection.IndicatorNIds += "," + Row[Data.IndicatorNId].ToString();
        //        }
        //        this.UserPreference.UserSelection.IndicatorNIds = this.UserPreference.UserSelection.IndicatorNIds.Substring(1);
        //        this.UserPreference.UserSelection.ShowIUS = false;
        //    }
        //    else if (this.UserPreference.UserSelection.ShowIUS && string.IsNullOrEmpty(this.UserPreference.UserSelection.IndicatorNIds))
        //    {
        //        this.UserPreference.UserSelection.ShowIUS = false;
        //    }
        //}



        public DIWizardPanel.PanelType GetNextPanel(DIWizardPanel.PanelType panelType, bool mrd, bool recommendedSources, DIWizardPanel.PanelType firstAccessedPage)
        {
            DIWizardPanel.PanelType Retval = DIWizardPanel.PanelType.Indicator;
            try
            {
                switch (panelType)
                {
                    case DIWizardPanel.PanelType.Indicator:

                        if (!this.IsThirdValidPage())
                        {
                            if (firstAccessedPage == DIWizardPanel.PanelType.Indicator)
                            {
                                //-- Set the next page to Area, if user first go to the indicator page
                                Retval = DIWizardPanel.PanelType.Area;
                            }
                            else
                            {
                                //-- Set the next page to Indicator, if user first go to the area page
                                Retval = DIWizardPanel.PanelType.Indicator;
                            }
                        }
                        else
                        {
                            Retval = DIWizardPanel.PanelType.Database;
                            //Retval = GetThirdPage(mrd, recommendedSources, Retval);
                        }
                        break;

                    case DIWizardPanel.PanelType.Area:
                        if (!this.IsThirdValidPage())
                        {
                            if (firstAccessedPage == DIWizardPanel.PanelType.Indicator)
                            {
                                //-- Set the next page to Area, if user first go to the indicator page
                                Retval = DIWizardPanel.PanelType.Area;
                            }
                            else
                            {
                                //-- Set the next page to Indicator, if user first go to the area page
                                Retval = DIWizardPanel.PanelType.Indicator;
                            }
                        }
                        else
                        {
                            Retval = DIWizardPanel.PanelType.Database;
                            //Retval = GetThirdPage(mrd, recommendedSources, Retval);
                        }
                        break;

                    case DIWizardPanel.PanelType.TimePeriod:
                        if (!recommendedSources)
                        {
                            Retval = DIWizardPanel.PanelType.Source;
                        }
                        else
                        {
                            Retval = DIWizardPanel.PanelType.Database;
                        }
                        break;

                    case DIWizardPanel.PanelType.Source:
                        Retval = DIWizardPanel.PanelType.Database;
                        break;

                    case DIWizardPanel.PanelType.Database:                       
                        break;
                    case DIWizardPanel.PanelType.Gallery:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }


        private bool IsThirdValidPage()
        {
            bool Retval = false;
            try
            {
                if (this.SelectionOrder.Contains(DIWizardPanel.PanelType.Indicator.ToString()) && this.SelectionOrder.Contains(DIWizardPanel.PanelType.Area.ToString()))
                {
                    Retval = true;
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        private DIWizardPanel.PanelType GetThirdPage(bool mrd, bool recommendedSources, DIWizardPanel.PanelType Retval)
        {           
            if (!mrd)
            {
                Retval = DIWizardPanel.PanelType.TimePeriod;
            }
            else
            {
                if (recommendedSources)
                {
                    Retval = DIWizardPanel.PanelType.Database;
                }
                else
                {
                    Retval = DIWizardPanel.PanelType.Source;
                }
            }
            return Retval;
        }

        public DIWizardPanel.PanelType GetPreviousPanel(DIWizardPanel.PanelType panelType, DIWizardPanel.PanelType firstAccessedPage)
        {
            DIWizardPanel.PanelType Retval = DIWizardPanel.PanelType.Indicator;
            try
            {
                switch (panelType)
                {
                    case DIWizardPanel.PanelType.Indicator:
                        if (firstAccessedPage == DIWizardPanel.PanelType.Indicator)
                        {
                            //-- Do nothing
                        }
                        else
                        {
                            Retval = DIWizardPanel.PanelType.Area;
                        }
                        break;

                    case DIWizardPanel.PanelType.Area:
                        if (firstAccessedPage == DIWizardPanel.PanelType.Area)
                        {
                            //-- Do nothing
                        }
                        else
                        {
                            Retval = DIWizardPanel.PanelType.Indicator;
                        }
                        break;

                    case DIWizardPanel.PanelType.TimePeriod:
                        Retval = DIWizardPanel.PanelType.Area;
                        break;

                    case DIWizardPanel.PanelType.Source:
                        Retval = DIWizardPanel.PanelType.TimePeriod;
                        break;

                    case DIWizardPanel.PanelType.Database:
                        Retval = DIWizardPanel.PanelType.Indicator;
                        break;

                    case DIWizardPanel.PanelType.Gallery:
                        break;
                    default:
                        break;
                }
                if (firstAccessedPage == DIWizardPanel.PanelType.Indicator)
                {
                    this.RemoveFromHistory(Retval, true);
                }
                else
                {
                    this.RemoveFromHistory(Retval, false);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Set the navigation oder of the wizard.
        /// </summary>
        public void NavigationOrder()
        {
            if (this.SelectionOrder.IndexOf(this._SelectedPage.ToString()) == -1)
            {
                // -- If not found, Insert the selection order on the bottom 
                this.SelectionOrder.Insert(this.SelectionOrder.Count, this._SelectedPage.ToString());
            }
        }

        public void RemoveFromHistory(DIWizardPanel.PanelType panelType, bool isIndicatorFirstPage)
        {
            try
            {
                if (this._SelectionOrder.Count >= 2)
                {
                    this._SelectionOrder.RemoveAt(this._SelectionOrder.Count - 1);
                }
            }
            catch (Exception)
            {
            }

            ////switch (panelType)
            ////{
            ////    case DIWizardPanel.PanelType.Indicator:
            ////        if (this._SelectionOrder.Count >= 2)
            ////        {
            ////            if (isIndicatorFirstPage)
            ////            {
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Indicator.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Indicator.ToString());
            ////                }
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Area.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Area.ToString());
            ////                }
            ////            }
            ////            else
            ////            {
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Indicator.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Indicator.ToString());
            ////                }
            ////            }
            ////        }
            ////        break;
            ////    case DIWizardPanel.PanelType.Area:
            ////        if (this._SelectionOrder.Count >= 2)
            ////        {
            ////            if (!isIndicatorFirstPage)
            ////            {
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Indicator.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Indicator.ToString());
            ////                }
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Area.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Area.ToString());
            ////                }
            ////            }
            ////            else
            ////            {
            ////                if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Area.ToString()))
            ////                {
            ////                    this._SelectionOrder.Remove(DIWizardPanel.PanelType.Area.ToString());
            ////                }
            ////            }
            ////        }
            ////        break;
            ////    case DIWizardPanel.PanelType.TimePeriod:
            ////        if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.TimePeriod.ToString()))
            ////        {
            ////            this._SelectionOrder.Remove(DIWizardPanel.PanelType.TimePeriod.ToString());
            ////        }
            ////        break;
            ////    case DIWizardPanel.PanelType.Source:
            ////        if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Source.ToString()))
            ////        {
            ////            this._SelectionOrder.Remove(DIWizardPanel.PanelType.Source.ToString());
            ////        }
            ////        break;
            ////    case DIWizardPanel.PanelType.Database:
            ////        if (this._SelectionOrder.Contains(DIWizardPanel.PanelType.Database.ToString()))
            ////        {
            ////            this._SelectionOrder.Remove(DIWizardPanel.PanelType.Database.ToString());
            ////        }
            ////        break;
            ////    case DIWizardPanel.PanelType.Gallery:
            ////        break;
            ////    default:
            ////        break;
            ////}            
        }


        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Constants -- "

        /// <summary>
        /// -
        /// </summary>
        private const string SEPRATOR = "-";

        /// <summary>
        /// |
        /// </summary>
        private const string GALLERY_SEPRATOR = "|";

        /// <summary>
        /// ,
        /// </summary>
        private const string PRESENTATION_SEPRATOR = ",";

        /// <summary>
        /// :
        /// </summary>
        private const string VALUE_SEPRATOR = ":";

        /// <summary>
        /// /
        /// </summary>
        private const string DATABASE_SEPRATOR = "/";

        /// <summary>
        /// (
        /// </summary>
        private const string DATAVALUE_START = "(";

        /// <summary>
        /// )
        /// </summary>
        private const string DATAVALUE_END = ")";

        /// <summary>
        /// Indicator
        /// </summary>
        private const string INDICATOR = "Indicator";

        /// <summary>
        /// Area
        /// </summary>
        private const string AREA = "Area";

        /// <summary>
        /// TimePeriod
        /// </summary>
        private const string TIMEPERIOD = "TimePeriod";

        /// <summary>
        /// Source
        /// </summary>
        private const string SOURCE = "Source";

        #endregion

        #region " -- Variables -- "

        /// <summary>
        /// DAL Connection object
        /// </summary>
        private DIConnection DbConnection;

        /// <summary>
        /// DAL query object
        /// </summary>
        private DIQueries DbQueries;       

        /// <summary>
        /// Database record count
        /// </summary>
        //private int DatabaseRecordCount = 0;

        /// <summary>
        /// Indicator record count
        /// </summary>
        //private int IndicatorRecordCount = 0;

        /// <summary>
        /// Area record count
        /// </summary>
        //private int AreaRecordCount = 0;

        /// <summary>
        /// time period record count
        /// </summary>
        //private int TimePeriodRecordCount = 0;

        /// <summary>
        /// source record count
        /// </summary>
        //private int SourceRecordCount = 0;

        /// <summary>
        /// true, Filter entry is deleted from the history.
        /// </summary>
        private bool IsFilterDeletedFromHistory = false;

        /// <summary>
        /// False, will not convert IUS NIDs to Indicator NIds
        /// </summary>
        //private bool IsIUSFilterApply = false;

        /// <summary>
        /// Database cache folder path
        /// </summary>
        private string DbNamePath = string.Empty;

        /// <summary>
        /// Contains the auto select record count of panels
        /// </summary>
        private Dictionary<DIWizardPanel.PanelType, int> AutoSelectRecordCount = new Dictionary<DIWizardPanel.PanelType, int>();

        /// <summary>
        /// Store the Selected Indicator NIds
        /// </summary>
        private string IndicatorNIds = string.Empty;

        /// <summary>
        /// Constains the stock folder path
        /// </summary>
        private string StockFolderPath = string.Empty;

        /// <summary>
        /// Object of cache class. Contains availabe NIDs and counts of the D,I,A,T,S.
        /// </summary>
        private CacheInfo AvailableNIdCache = new CacheInfo();

        /// <summary>
        /// Object of cache class. Contains auto selected NIDs and counts of the D,I,A,T,S.
        /// </summary>
        private CacheInfo AutoSelectNIdCache = new CacheInfo();

        #endregion

        #region " -- Method -- "

        /// <summary>
        /// Intilaize the panel
        /// </summary>
        private void IntializePanel()
        {
            DIWizardPanel WizardPanel;

            #region " -- Gallery -- "

            //WizardPanel = new DIWizardPanel();
            //WizardPanel.Type = DIWizardPanel.PanelType.Gallery;

            //// -- Set the gallery panel image.
            //this.SetPanel(WizardPanel);

            //WizardPanel.Selected = true;
            //WizardPanel.Order = 0;
            //WizardPanel.Caption = string.Empty;
            //WizardPanel.StatusCaption = string.Empty;
            //WizardPanel.LinkCaption = LanguageStrings.ViewGallery;

            //// -- Add the gallery panel in the collection.
            //this._DIWizardPanels.Add(WizardPanel);

            #endregion     

            #region " -- Indicator -- "

            WizardPanel = new DIWizardPanel();
            WizardPanel.Type = DIWizardPanel.PanelType.Indicator;

            // -- Set the indicator panel image.
            this.SetPanel(WizardPanel);

            WizardPanel.Selected = false;
            WizardPanel.Order = 0;
            WizardPanel.Caption = string.Empty;
            WizardPanel.StatusCaption = string.Empty;
            WizardPanel.LinkCaption = string.Empty;

            // -- Add the indicator panel in the collection.
            this._DIWizardPanels.Add(WizardPanel);

            #endregion

            #region " -- Area -- "

            WizardPanel = new DIWizardPanel();
            WizardPanel.Type = DIWizardPanel.PanelType.Area;

            // -- Set the area panel image.
            this.SetPanel(WizardPanel);

            WizardPanel.Selected = false;
            WizardPanel.Order = 1;
            WizardPanel.Caption = string.Empty;
            WizardPanel.StatusCaption = string.Empty;
            WizardPanel.LinkCaption = string.Empty;

            // -- Add the area panel in the collection.
            this._DIWizardPanels.Add(WizardPanel);

            #endregion

            #region " -- TimePeriod -- "

            WizardPanel = new DIWizardPanel();
            WizardPanel.Type = DIWizardPanel.PanelType.TimePeriod;

            // -- Set the time period panel image.
            this.SetPanel(WizardPanel);

            WizardPanel.Selected = false;
            WizardPanel.Order = 2;
            WizardPanel.Caption = string.Empty;
            WizardPanel.StatusCaption = string.Empty;
            WizardPanel.LinkCaption = string.Empty;

            // -- Add the Time period panel in the collection.
            this._DIWizardPanels.Add(WizardPanel);

            #endregion

            #region " -- Source -- "

            WizardPanel = new DIWizardPanel();
            WizardPanel.Type = DIWizardPanel.PanelType.Source;


            // -- Set the source panel image.
            this.SetPanel(WizardPanel);

            WizardPanel.Selected = false;
            WizardPanel.Order = 3;
            WizardPanel.Caption = string.Empty;
            WizardPanel.StatusCaption = string.Empty;
            WizardPanel.LinkCaption = string.Empty;

            // -- Add the source panel in the collection.
            this._DIWizardPanels.Add(WizardPanel);

            #endregion

            #region " -- Database -- "

            WizardPanel = new DIWizardPanel();
            WizardPanel.Type = DIWizardPanel.PanelType.Database;

            // -- Set the database panel image.
            this.SetPanel(WizardPanel);

            WizardPanel.Selected = false;
            WizardPanel.Order = 4;
            WizardPanel.Caption = string.Empty;
            WizardPanel.StatusCaption = string.Empty;
            WizardPanel.LinkCaption = LanguageStrings.ViewData;

            // -- Add the database panel in the collection.
            this._DIWizardPanels.Add(WizardPanel);

            #endregion

            // -- Set the background image of the panel
            this.BackgroundImage(null);
        }

        /// <summary>
        /// Set the panel BGimages,Icons, and Link Icon on the basis of panel type.
        /// </summary>
        /// <param name="dIWizardPanel"></param>
        private void SetPanel(DIWizardPanel dIWizardPanel)
        {
            switch (dIWizardPanel.Type)
            {
                //case DIWizardPanel.PanelType.Gallery:
                    //dIWizardPanel.BGImageNormal = ImageFileName.PANEL1_NORMAL_IMAGE;
                    //dIWizardPanel.BGImageSelected = ImageFileName.PANEL1_SELECTED_IMAGE;
                    //dIWizardPanel.BGImageHover = ImageFileName.PANEL1_HOVER_IMAGE;

                    //dIWizardPanel.IconNormal = ImageFileName.GALLERY_NORMAL_ICON;
                    //dIWizardPanel.IconSelected = ImageFileName.GALLERY_SELECTED_ICON;
                    //dIWizardPanel.IconHover = ImageFileName.GALLERY_HOVER_ICON;

                    //dIWizardPanel.LinkIconNormal = ImageFileName.LINK_NORMAL_ICON;
                    //dIWizardPanel.LinkIconSelected = ImageFileName.LINK_SELECTED_ICON;
                    //dIWizardPanel.LinkIconHover = ImageFileName.LINK_HOVER_ICON;
                    //break;
                case DIWizardPanel.PanelType.Database:
                    dIWizardPanel.BGImageNormal = string.Empty;
                    dIWizardPanel.BGImageSelected = string.Empty;
                    dIWizardPanel.BGImageHover = string.Empty;

                    dIWizardPanel.IconNormal = ImageFileName.DATABASE_NORMAL_ICON;
                    dIWizardPanel.IconSelected = ImageFileName.DATABASE_SELECTED_ICON;
                    dIWizardPanel.IconHover = ImageFileName.DATABASE_HOVER_ICON;

                    dIWizardPanel.LinkIconNormal = string.Empty;
                    dIWizardPanel.LinkIconSelected = string.Empty;
                    dIWizardPanel.LinkIconHover = string.Empty;

                    dIWizardPanel.BGImageNormal = ImageFileName.DATABASE_PANEL_IMAGE;
                    dIWizardPanel.BGImageSelected = ImageFileName.DATABASE_PANEL_IMAGE;
                    dIWizardPanel.BGImageHover = ImageFileName.DATABASE_PANEL_IMAGE;

                    //dIWizardPanel.IconNormal = ImageFileName.DATABASE_NORMAL_ICON;
                    //dIWizardPanel.IconSelected = ImageFileName.DATABASE_SELECTED_ICON;
                    //dIWizardPanel.IconHover = ImageFileName.DATABASE_HOVER_ICON;

                    //dIWizardPanel.LinkIconNormal = ImageFileName.LINK_NORMAL_ICON;
                    //dIWizardPanel.LinkIconSelected = ImageFileName.LINK_SELECTED_ICON;
                    //dIWizardPanel.LinkIconHover = ImageFileName.LINK_HOVER_ICON;
                    break;
                case DIWizardPanel.PanelType.Indicator:

                    // -- Set the Indicator panel icon
                    dIWizardPanel.IconNormal = ImageFileName.INDICATOR_NORMAL_ICON;
                    dIWizardPanel.IconSelected = ImageFileName.INDICATOR_SELECTED_ICON;
                    dIWizardPanel.IconHover = ImageFileName.INDICATOR_HOVER_ICON;

                    dIWizardPanel.LinkIconNormal = string.Empty;
                    dIWizardPanel.LinkIconSelected = string.Empty;
                    dIWizardPanel.LinkIconHover = string.Empty;
                    break;
                case DIWizardPanel.PanelType.Area:

                    // -- Set the Area panel icon
                    dIWizardPanel.IconNormal = ImageFileName.AREA_NORMAL_ICON;
                    dIWizardPanel.IconSelected = ImageFileName.AREA_SELECTED_ICON;
                    dIWizardPanel.IconHover = ImageFileName.AREA_HOVER_ICON;

                    dIWizardPanel.LinkIconNormal = string.Empty;
                    dIWizardPanel.LinkIconSelected = string.Empty;
                    dIWizardPanel.LinkIconHover = string.Empty;
                    break;
                case DIWizardPanel.PanelType.TimePeriod:
                    // -- Set the Time period panel icon
                    dIWizardPanel.IconNormal = ImageFileName.TIMEPERIOD_NORMAL_ICON;
                    dIWizardPanel.IconSelected = ImageFileName.TIMEPERIOD_SELECTED_ICON;
                    dIWizardPanel.IconHover = ImageFileName.TIMEPERIOD_HOVER_ICON;

                    dIWizardPanel.LinkIconNormal = string.Empty;
                    dIWizardPanel.LinkIconSelected = string.Empty;
                    dIWizardPanel.LinkIconHover = string.Empty;
                    break;
                case DIWizardPanel.PanelType.Source:
                    // -- Set the source panel icon
                    dIWizardPanel.IconNormal = ImageFileName.SOURCE_NORMAL_ICON;
                    dIWizardPanel.IconSelected = ImageFileName.SOURCE_SELECTED_ICON;
                    dIWizardPanel.IconHover = ImageFileName.SOURCE_HOVER_ICON;

                    dIWizardPanel.LinkIconNormal = string.Empty;
                    dIWizardPanel.LinkIconSelected = string.Empty;
                    dIWizardPanel.LinkIconHover = string.Empty;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set the background images of the panel.
        /// </summary>
        /// <param name="dIWizardPanel"></param>
        private void BackgroundImage(DIWizardPanel dIWizardPanel)
        {
            for (int PanelIndex = 0; PanelIndex < 4; PanelIndex++)
            {
                switch (this._DIWizardPanels[PanelIndex].Order) // dIWizardPanel.Order)
                {
                    case 0:
                        this._DIWizardPanels[PanelIndex].BGImageNormal = ImageFileName.PANEL1_NORMAL_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageSelected = ImageFileName.PANEL1_SELECTED_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageHover = ImageFileName.PANEL1_HOVER_IMAGE;
                        break;

                    case 1:
                        this._DIWizardPanels[PanelIndex].BGImageNormal = ImageFileName.PANEL2_NORMAL_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageSelected = ImageFileName.PANEL2_SELECTED_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageHover = ImageFileName.PANEL2_HOVER_IMAGE;
                        break;

                    case 2:
                        this._DIWizardPanels[PanelIndex].BGImageNormal = ImageFileName.PANEL3_NORMAL_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageSelected = ImageFileName.PANEL3_SELECTED_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageHover = ImageFileName.PANEL3_HOVER_IMAGE;
                        break;

                    case 3:
                        this._DIWizardPanels[PanelIndex].BGImageNormal = ImageFileName.PANEL4_NORMAL_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageSelected = ImageFileName.Panel4_SELECTED_IMAGE;
                        this._DIWizardPanels[PanelIndex].BGImageHover = ImageFileName.Panel4_HOVER_IMAGE;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SetGallery(DIWizardPanel dIWizardPanel)
        {
            dIWizardPanel.GalleryCaption = LanguageStrings.Galleries;
            //TODO: remove hardcoding.
            GallerySearch Gallery = new GallerySearch(this._GalleryDatabaseFileWithPath);

            //-- Get the Gallery count.
            int GalleryCount = Gallery.GetPresentaionCount(-1);

            if (string.IsNullOrEmpty(this.UserPreference.UserSelection.IndicatorNIds) && string.IsNullOrEmpty(this.UserPreference.UserSelection.AreaNIds) & string.IsNullOrEmpty(this.UserPreference.UserSelection.TimePeriodNIds) & string.IsNullOrEmpty(this.UserPreference.UserSelection.SourceNIds))
            {
                dIWizardPanel.GalleryStatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + GalleryCount + " " + DATABASE_SEPRATOR + " " + GalleryCount;
                if (GalleryCount > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Gallery] = GalleryCount.ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Gallery] = string.Empty;
                }
            }
            else
            {
                int SelectedCount = Gallery.GetSearchResultCount(this.GenerateKeywords(), string.Empty, GallerySearch.SearchType.All, -1);
                dIWizardPanel.GalleryStatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + SelectedCount.ToString() + " " + DATABASE_SEPRATOR + " " + GalleryCount.ToString();
                if (SelectedCount > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Gallery] = SelectedCount.ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Gallery] = string.Empty;
                }
            }
        }       

        /// <summary>
        /// Set the Database panel properties
        /// </summary>
        private void SetDatabasePanel(DIWizardPanel dIWizardPanel)
        {
            this._DIWizardPanels[4].Caption = LanguageStrings.Database;
            this._DIWizardPanels[4].StatusCaption = string.Empty;

            //////if ((string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && this._UserPreference.UserSelection.DataViewFilters.MostRecentData && this.SelectionOrder.Count == 1) || (this.SelectionOrder.Count == 0))
            //////{
            //////    // -- Set the database panel status caption.
            //////    this._DIWizardPanels[4].StatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
            //////    this._SelectionCount[DIWizardPanel.PanelType.Database] = String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
            //////}
            //////else 

            if (this.SelectionOrder.Count > 0)
            {
                if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Database))
                {
                    // -- Set the database panel status caption.
                    this._DIWizardPanels[4].StatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + String.Format("{0:n}", Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database])).Replace(".00", "") + " / " + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
                    this._SelectionCount[DIWizardPanel.PanelType.Database] = String.Format("{0:n}", Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database])).Replace(".00", "");
                }
                else
                {
                    // -- Set the database panel status caption from cache
                    this._DIWizardPanels[4].StatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
                    this._SelectionCount[DIWizardPanel.PanelType.Database] = String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
                }
            }
            else
            {
                // -- Set the database panel status caption.
                this._DIWizardPanels[4].StatusCaption = LanguageStrings.Count + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
                this._SelectionCount[DIWizardPanel.PanelType.Database] = String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "");
            }

            this.SetGallery(dIWizardPanel);

            //string[] DBFiles = new string[0];
            //int DatabaseCount = 0;

            //if (Directory.Exists(this._DataFolderPath))
            //{
            //    // -- MDB files in the data folder.
            //    DBFiles = Directory.GetFiles(this._DataFolderPath, "*" + DICommon.FileExtension.Database);
            //    DatabaseCount = DBFiles.Length;
            //    // -- MDF files in the data folder.
            //    DBFiles = Directory.GetFiles(this._DataFolderPath, "*" + DICommon.FileExtension.MDFDatabase);
            //    DatabaseCount += DBFiles.Length;

            //    // -- Online connections count.
            //    DatabaseCount += this.UserPreference.Database.OnlineDatabaseDetails.Count;

            //    // -- Set the database panel caption.
            //    this._DIWizardPanels[1].Caption = LanguageStrings.Database + " " + DIWizard.SEPRATOR + " 1 " + DATABASE_SEPRATOR + " " + DatabaseCount.ToString() + " " + LanguageStrings.Selected.ToLower();

            //    if ((string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && this._UserPreference.UserSelection.DataViewFilters.MostRecentData && this.SelectionOrder.Count == 1) || (this.SelectionOrder.Count == 0))
            //    {
            //        // -- Set the database panel status caption.
            //        this._DIWizardPanels[1].StatusCaption = Path.GetFileName(DbConnection.ConnectionStringParameters.DbName) + " " + DATAVALUE_START + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "") + " " + LanguageStrings.Count + DATAVALUE_END;
            //    }
            //    else if (this.SelectionOrder.Count > 0)
            //    {
            //        if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Database))
            //        {
            //            // -- Set the database panel status caption.
            //            this._DIWizardPanels[1].StatusCaption = Path.GetFileName(DbConnection.ConnectionStringParameters.DbName) + " " + DATAVALUE_START + String.Format("{0:n}", Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database])).Replace(".00", "") + " / " + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "") + " " + LanguageStrings.Count + DATAVALUE_END;
            //        }
            //        else
            //        {
            //            // -- Set the database panel status caption from cache
            //            this._DIWizardPanels[1].StatusCaption = Path.GetFileName(DbConnection.ConnectionStringParameters.DbName) + " " + DATAVALUE_START + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "") + " " + LanguageStrings.Count + DATAVALUE_END;
            //        }
            //    }
            //    //else
            //    //{
            //    //    // -- Set the database panel status caption.
            //    //    this._DIWizardPanels[1].StatusCaption = Path.GetFileName(DbConnection.ConnectionStringParameters.DbName) + " " + DATAVALUE_START + String.Format("{0:n}", this.AvailableNIdCache.DataCount).Replace(".00", "") + " " + LanguageStrings.Count + DATAVALUE_END;
            //    //}
            //}
            //else
            //{
                //this._DIWizardPanels[1].Caption = LanguageStrings.Database + " " + SEPRATOR + " 0 " + DATABASE_SEPRATOR + " " + 0 + " " + LanguageStrings.Selected;
                //this._DIWizardPanels[1].StatusCaption = string.Empty;
            //}
        }

        /// <summary>
        /// Set the selected count of indicator nids
        /// </summary>
        private void SetSelectedIndicatorPanel()
        {
            if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
            {
                string[] IndicatorNids = new string[0];
                IndicatorNids = DICommon.SplitString(this._UserPreference.UserSelection.IndicatorNIds, ",");
                if (IndicatorNids.Length > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Indicator] = IndicatorNids.Length.ToString();
                }               
            }
            else
            {
                if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Indicator] = this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator].ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Indicator] = string.Empty;
                }
            }
        }

        /// <summary>
        /// Set the selected count of area nids
        /// </summary>
        private void SetSelectedAreaPanel()
        {
            if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
            {
                string[] AreaNids = new string[0];
                AreaNids = DICommon.SplitString(this._UserPreference.UserSelection.AreaNIds, ",");
                if (AreaNids.Length > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Area] = AreaNids.Length.ToString();
                }               
            }
            else
            {
                if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Area] = this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area].ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Area] = string.Empty;
                }
            }
        }

        /// <summary>
        /// Set the selected count of timeperiod nids
        /// </summary>
        private void SetSelectedTimePeriodPanel()
        {
            if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
            {
                string[] TimePeriodNids = new string[0];
                TimePeriodNids = DICommon.SplitString(this._UserPreference.UserSelection.TimePeriodNIds, ",");
                if (TimePeriodNids.Length > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = TimePeriodNids.Length.ToString();
                }               
            }
            else
            {
                if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                {
                    this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod].ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = string.Empty;
                }                
            }
        }

        /// <summary>
        /// Set the selected count of source nids
        /// </summary>
        private void SetSelectedSourcePanel()
        {
            if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
            {
                string[] SourceNids = new string[0];
                SourceNids = DICommon.SplitString(this._UserPreference.UserSelection.SourceNIds, ",");
                if (SourceNids.Length > 0)
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Source] = SourceNids.Length.ToString();
                }                
            }
            else
            {
                if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Source] = this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source].ToString();
                }
                else
                {
                    this._SelectionCount[DIWizardPanel.PanelType.Source] = string.Empty;
                }                
            }
        }


        /// <summary>
        /// Set the indicator panel properties
        /// </summary>
        private void SetIndicatorPanel(DIWizardPanel dIWizardPanel)
        {
            if ((string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && this._UserPreference.UserSelection.DataViewFilters.MostRecentData && this.SelectionOrder.Count == 1) || (this.SelectionOrder.Count == 0))
            {
                // -- Indicator panel is updated with total number of record.
                //this._DIWizardPanels[0].Caption = LanguageStrings.Indicators;
                this._DIWizardPanels[0].StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.IndicatorCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                this._SelectionCount[DIWizardPanel.PanelType.Indicator] = String.Format("{0:n}", this.AvailableNIdCache.IndicatorCount).Replace(".00", "");
            }
            else if (this.SelectionOrder.Count > 0)
            {
                // -- Selected panel index in the selection order.
                int SelectedIndex = this.SelectionOrder.IndexOf(this.SelectedPage.ToString());
                // -- indicator panel index in the selection order
                int IndicatorPanelIndex = this.SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString());

                if (IndicatorPanelIndex >= SelectedIndex || IndicatorPanelIndex == -1)
                {
                    int AutoSelectIndicatorCount = 0;

                    if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
                    {
                        // -- Get the auto select indicator count from cache.
                        AutoSelectIndicatorCount = Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator]);
                    }

                    

                    if (IndicatorPanelIndex > SelectedIndex)
                    {
                        //// -- Update the Indicator Nids with the new selection NIds
                        //IDataReader SelectionReader;
                        //SelectionReader = this.DbConnection.ExecuteReader(this.DbQueries.Indicators.GetAutoSelectIndicator(this.UserPreference.UserSelection.TimePeriodNIds, this.UserPreference.UserSelection.AreaNIds, this.UserPreference.UserSelection.SourceNIds, FieldSelection.Light));

                        if (this._AutoSelectRecords != null && this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
                        {
                            this._UserPreference.UserSelection.IndicatorNIds = this.UpdateSelections(this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator], this._UserPreference.UserSelection.IndicatorNIds);
                        }

                        //SelectionReader.Close();
                        if (string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
                        {
                            this.SelectionOrder.Remove(dIWizardPanel.Type.ToString());
                            dIWizardPanel.Selected = false;
                        }
                        // -- Update the indicator panel caption.
                        IndicatorPanelIndex = 0;
                    }

                    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds) && IndicatorPanelIndex != -1)
                    {
                        // -- Get the selected indicator NId on the basis of IUS NId.
                        //DataTable dtIndicator;
                        //dtIndicator = this.DbConnection.ExecuteDataTable("SELECT DISTINCT IUS." + Indicator_Unit_Subgroup.IndicatorNId + " FROM UT_Indicator_Unit_Subgroup IUS WHERE IUS." + Indicator_Unit_Subgroup.IUSNId + " IN (" + this.UserPreference.UserSelection.IndicatorNIds + ")");

                        // -- Indicator panel is updated with the total number of Indicator selected.
                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + this.GetIndicatorFromIUS() + " / " + String.Format("{0:n}", this.AvailableNIdCache.IndicatorCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.Indicator] = this.GetIndicatorFromIUS().ToString();
                    }
                    else if (AutoSelectIndicatorCount == 1)
                    {
                        if (this._AutoSelectRecords != null && this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
                        {
                            this._UserPreference.UserSelection.IndicatorNIds = this.UpdateSelections(this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator], this._UserPreference.UserSelection.IndicatorNIds);
                        }

                        // -- Indicator panel is updated, if 1 record is left after auto select.
                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " 1 / " + String.Format("{0:n}", this.AvailableNIdCache.IndicatorCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.Indicator] = "1";
                        dIWizardPanel.Order = -1;
                    }
                    else
                    {
                        this._UserPreference.UserSelection.IndicatorNIds = string.Empty;

                        // -- Indicator panel is updated with total number of record left after the current selection.
                        dIWizardPanel.StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", AutoSelectIndicatorCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                        this._SelectionCount[DIWizardPanel.PanelType.Indicator] = String.Format("{0:n}", AutoSelectIndicatorCount).Replace(".00", "");
                        //this.RemoveSelection(dIWizardPanel);
                    }
                }
            }

            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
            {
                dIWizardPanel.Caption = LanguageStrings.Indicators;
            }
            else
            {
                dIWizardPanel.Caption = LanguageStrings.Indicator;
            }
        }

        /// <summary>
        /// Get the indicator from the IUS
        /// </summary>
        /// <returns></returns>
        private int GetIndicatorFromIUS()
        {
            int Retval = 0;
            try
            {
                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.IndicatorNIds))
                {
                    IDataReader IndicatorReader;
                    this.IndicatorNIds = string.Empty;

                    IndicatorReader = this.DbConnection.ExecuteReader(this.DbQueries.Indicators.GetIndicators(this.UserPreference.UserSelection.IndicatorNIds, true));
                    while (IndicatorReader.Read())
                    {
                        if (this.IndicatorNIds.IndexOf(IndicatorReader[Indicator.IndicatorNId].ToString()) == -1)
                        {
                            this.IndicatorNIds += "," + IndicatorReader[Indicator.IndicatorNId].ToString();
                            Retval += 1;
                        }
                    }
                    if (!string.IsNullOrEmpty(this.IndicatorNIds))
                    {
                        this.IndicatorNIds = this.IndicatorNIds.Substring(1);
                    }
                    IndicatorReader.Close();
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Set the area panel properties
        /// </summary>
        private void SetAreaPanel(DIWizardPanel dIWizardPanel)
        {
            if ((string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && this._UserPreference.UserSelection.DataViewFilters.MostRecentData && this.SelectionOrder.Count == 1) || (this.SelectionOrder.Count == 0))
            {
                // -- Area panel is updated with total number of record.
                //this._DIWizardPanels[1].Caption = LanguageStrings.Areas;
                this._DIWizardPanels[1].StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.AreaCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                this._SelectionCount[DIWizardPanel.PanelType.Area] = String.Format("{0:n}", this.AvailableNIdCache.AreaCount).Replace(".00", "");
            }
            else if (this.SelectionOrder.Count > 0)
            {
                // -- Selected panel index in the selection order.
                int SelectedIndex = this.SelectionOrder.IndexOf(this.SelectedPage.ToString());
                // -- Area panel index in the selection order
                int AreaPanelIndex = this.SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString());

                if (AreaPanelIndex >= SelectedIndex || AreaPanelIndex == -1)
                {
                    int AutoSelectAreaCount = 0;

                    if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
                    {
                        // -- Get the auto select area count.
                        AutoSelectAreaCount = Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area]);
                    }

                    //// -- Get the auto select area count.
                    //AutoSelectAreaCount = this.GetAreaAutoSelectCount();

                    if (AreaPanelIndex > SelectedIndex)
                    {
                        //// -- Update the Area Nids with the new selection NIds

                        if (this._AutoSelectRecords != null && this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
                        {
                            this._UserPreference.UserSelection.AreaNIds = this.UpdateSelections(this._AutoSelectRecords[DIWizardPanel.PanelType.Area], this._UserPreference.UserSelection.AreaNIds);
                        }

                        if (string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
                        {
                            this.SelectionOrder.Remove(dIWizardPanel.Type.ToString());
                            dIWizardPanel.Selected = false;
                        }

                        // -- Update the area panel caption.
                        AreaPanelIndex = 0;
                    }

                    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds) && AreaPanelIndex != -1)
                    {
                        // -- Area panel is updated with the total number of record selected.
                        string[] AreaNIds = new string[0];
                        AreaNIds = DICommon.SplitString(this._UserPreference.UserSelection.AreaNIds, ",");

                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + AreaNIds.Length.ToString() + " / " + String.Format("{0:n}", this.AvailableNIdCache.AreaCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.Area] = AreaNIds.Length.ToString();
                    }
                    else if (AutoSelectAreaCount == 1)
                    {
                        //-- Get the single area NId and stored it in the user selection.
                        this._UserPreference.UserSelection.AreaNIds = this._AutoSelectRecords[DIWizardPanel.PanelType.Area];

                        // -- Area panel is updated, if 1 record is left after auto select.
                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + "1 / " + String.Format("{0:n}", this.AvailableNIdCache.AreaCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.Area] = "1";
                        dIWizardPanel.Order = -1;
                    }
                    else
                    {
                        this._UserPreference.UserSelection.AreaNIds = string.Empty;
                        // -- Area panel is updated with total number of record left after the current selection.
                        dIWizardPanel.StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", AutoSelectAreaCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                        this._SelectionCount[DIWizardPanel.PanelType.Area] = String.Format("{0:n}", AutoSelectAreaCount).Replace(".00", "");
                        // -- Uncheck all the selected records and move next
                        //this.RemoveSelection(dIWizardPanel);
                    }
                }
            }
            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
            {
                dIWizardPanel.Caption = LanguageStrings.Areas;
            }
            else
            {
                dIWizardPanel.Caption = LanguageStrings.Area;
            }
        }

        /// <summary>
        /// Set the time period panel properties
        /// </summary>
        private void SetTimePeriodPanel(DIWizardPanel dIWizardPanel)
        {
            //!this.UserPreference.UserSelection.DataViewFilters.MostRecentData && 
            if (this.SelectionOrder.Count > 0)
            {
                // -- Selected panel index in the selection order.
                int SelectedIndex = this.SelectionOrder.IndexOf(this.SelectedPage.ToString());
                // -- Time period panel index in the selection order
                int TimePeriodPanelIndex = this.SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString());
                // -- Get the auto select time period count.

                if (TimePeriodPanelIndex >= SelectedIndex || TimePeriodPanelIndex == -1)
                {
                    int AutoSelectTimePeriodCount = 0;

                    if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                    {
                        // -- Get the auto select area count.
                        AutoSelectTimePeriodCount = Convert.ToInt32(this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod]);
                    }

                    if (TimePeriodPanelIndex > SelectedIndex)
                    {
                        // -- Update the Time period Nids with the new selection NIds

                        if (this._AutoSelectRecords != null && this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                        {
                            this._UserPreference.UserSelection.TimePeriodNIds = this.UpdateSelections(this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod], this._UserPreference.UserSelection.TimePeriodNIds);
                        }

                        if (string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
                        {
                            this.SelectionOrder.Remove(dIWizardPanel.Type.ToString());
                            dIWizardPanel.Selected = false;
                        }

                        // -- Update the Time period panel caption.
                        TimePeriodPanelIndex = 0;
                    }

                    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && TimePeriodPanelIndex != -1)
                    {
                        // -- time period panel is updated with the total number of record selected.
                        string[] TimePeriodsNIds = new string[0];
                        TimePeriodsNIds = DICommon.SplitString(this._UserPreference.UserSelection.TimePeriodNIds, ",");

                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + TimePeriodsNIds.Length.ToString() + " / " + String.Format("{0:n}", this.AvailableNIdCache.TimeperiodCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = TimePeriodsNIds.Length.ToString();
                    }
                    else if (AutoSelectTimePeriodCount == 1)
                    {
                        //-- Get the single time period NId and stored it in the user selection.                       

                        this._UserPreference.UserSelection.TimePeriodNIds = this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod];
                        // -- Timeperiod panel is updated, if 1 record is left after auto select.
                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + "1 / " + String.Format("{0:n}", this.AvailableNIdCache.TimeperiodCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = "1";
                        dIWizardPanel.Order = -1;
                    }
                    else
                    {
                        this._UserPreference.UserSelection.TimePeriodNIds = string.Empty;
                        // -- Timeperiod panel is updated with total number of record left after the current selection.
                        dIWizardPanel.StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", AutoSelectTimePeriodCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                        this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = String.Format("{0:n}", AutoSelectTimePeriodCount).Replace(".00", "");
                        //this.RemoveSelection(dIWizardPanel);
                    }
                }
            }
            //else if (this.UserPreference.UserSelection.DataViewFilters.MostRecentData)
            //{
            //    dIWizardPanel.StatusCaption = LanguageStrings.Selected_MRD;
            //    this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = "MRD";
            //}
            else if (this.SelectionOrder.Count == 0)
            {
                // -- Timeperiod panel is updated with total number of record.
                //this._DIWizardPanels[2].Caption = LanguageStrings.TimePeriods;
                this._DIWizardPanels[2].StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.TimeperiodCount.ToString()).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                this._SelectionCount[DIWizardPanel.PanelType.TimePeriod] = String.Format("{0:n}", this.AvailableNIdCache.TimeperiodCount.ToString()).Replace(".00", "");
            }

            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
            {
                dIWizardPanel.Caption = LanguageStrings.TimePeriods;
            }
            else
            {
                dIWizardPanel.Caption = LanguageStrings.TimePeriod;
            }
        }

        /// <summary>
        /// Set the source panel properties
        /// </summary>
        private void SetSourcePanel(DIWizardPanel dIWizardPanel)
        {
            if ((string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && this._UserPreference.UserSelection.DataViewFilters.MostRecentData && this.SelectionOrder.Count == 1) || (this.SelectionOrder.Count == 0))
            {
                // -- Source panel is updated with total number of record.
                //this._DIWizardPanels[3].Caption = LanguageStrings.Sources;
                this._DIWizardPanels[3].StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", this.AvailableNIdCache.SourceCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                this._SelectionCount[DIWizardPanel.PanelType.Source] = String.Format("{0:n}", this.AvailableNIdCache.SourceCount).Replace(".00", "");
            }
            else if (this.SelectionOrder.Count > 0)
            {
                // -- Selected panel index in the selection order.
                int SelectedIndex = this.SelectionOrder.IndexOf(this.SelectedPage.ToString());
                // -- Source panel index in the selection order
                int SourcePanelIndex = this.SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString());

                if (SourcePanelIndex >= SelectedIndex || SourcePanelIndex == -1)
                {
                    int AutoSelectSourceCount = 0;

                    if (this.AutoSelectRecordCount != null && this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
                    {
                        // -- Get the auto select source count.
                        AutoSelectSourceCount = this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source];
                    }

                    //// -- Get the auto select source count.
                    //AutoSelectSourceCount = this.GetSourceAutoSelectCount();

                    if (SourcePanelIndex > SelectedIndex)
                    {
                        //// -- Update the Time period Nids with the new selection NIds
                        if (this._AutoSelectRecords != null && this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
                        {
                            this._UserPreference.UserSelection.SourceNIds = this.UpdateSelections(this._AutoSelectRecords[DIWizardPanel.PanelType.Source], this._UserPreference.UserSelection.SourceNIds);
                        }

                        if (string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
                        {
                            this.SelectionOrder.Remove(dIWizardPanel.Type.ToString());
                            dIWizardPanel.Selected = false;
                        }


                        // -- Update the Time period panel caption.
                        SourcePanelIndex = 0;
                    }

                    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds) && SourcePanelIndex != -1)
                    {
                        // -- Source panel is updated with the total number of record selected.
                        string[] SourceNIds = new string[0];
                        SourceNIds = DICommon.SplitString(this._UserPreference.UserSelection.SourceNIds, ",");

                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + SourceNIds.Length.ToString() + " / " + String.Format("{0:n}", this.AvailableNIdCache.SourceCount).Replace(".00", "");
                    }
                    else if (AutoSelectSourceCount == 1)
                    {
                        //-- Get the single time period NId and stored it in the user selection.
                        this._UserPreference.UserSelection.SourceNIds = this._AutoSelectRecords[DIWizardPanel.PanelType.Source];

                        // -- Source panel is updated, if 1 record is left after auto select.
                        dIWizardPanel.StatusCaption = LanguageStrings.Selected + " " + SEPRATOR + " " + "1 / " + String.Format("{0:n}", this.AvailableNIdCache.SourceCount).Replace(".00", "");
                        this._SelectionCount[DIWizardPanel.PanelType.Source] = "1";
                        dIWizardPanel.Order = -1;
                    }
                    else
                    {
                        this._UserPreference.UserSelection.SourceNIds = string.Empty;
                        // -- Source panel is updated with total number of record left after the current selection.
                        dIWizardPanel.StatusCaption = LanguageStrings.Available + " " + SEPRATOR + " " + String.Format("{0:n}", AutoSelectSourceCount).Replace(".00", "") + "   " + LanguageStrings.Home_Page_Selected_Caption;
                        this._SelectionCount[DIWizardPanel.PanelType.Source] = String.Format("{0:n}", AutoSelectSourceCount).Replace(".00", "");
                        //this.RemoveSelection(dIWizardPanel);
                    }
                }
            }
            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
            {
                dIWizardPanel.Caption = LanguageStrings.Sources;
            }
            else
            {
                dIWizardPanel.Caption = LanguageStrings.Source;
            }
        }

        /// <summary>
        /// Generate the keywords using the user selection.
        /// </summary>
        /// <returns></returns>
        private string GenerateKeywords()
        {
            string Retval = string.Empty;
            StringBuilder Keywords = new StringBuilder();
            try
            {
                IDataReader KeywordReader;

                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.IndicatorNIds) && !string.IsNullOrEmpty(this.IndicatorNIds))
                {
                    KeywordReader = this.DbConnection.ExecuteReader(this.DbQueries.Indicators.GetIndicator(FilterFieldType.NId, this.IndicatorNIds, FieldSelection.Light));
                    while (KeywordReader.Read())
                    {
                        Keywords.Append(KeywordReader[Indicator.IndicatorName].ToString() + " ");
                    }
                    KeywordReader.Close();
                }

                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.AreaNIds))
                {
                    KeywordReader = this.DbConnection.ExecuteReader(this.DbQueries.Area.GetArea(FilterFieldType.NId, this.UserPreference.UserSelection.AreaNIds));
                    while (KeywordReader.Read())
                    {
                        Keywords.Append(KeywordReader[Area.AreaName].ToString() + " ");
                    }
                    KeywordReader.Close();
                }

                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.TimePeriodNIds))
                {
                    KeywordReader = this.DbConnection.ExecuteReader(this.DbQueries.Timeperiod.GetTimePeriod(this.UserPreference.UserSelection.TimePeriodNIds));
                    while (KeywordReader.Read())
                    {
                        Keywords.Append(KeywordReader[Timeperiods.TimePeriod].ToString() + " ");
                    }
                    KeywordReader.Close();
                }

                if (!string.IsNullOrEmpty(this.UserPreference.UserSelection.SourceNIds))
                {
                    KeywordReader = this.DbConnection.ExecuteReader(this.DbQueries.Source.GetSource(FilterFieldType.NId, this.UserPreference.UserSelection.SourceNIds, FieldSelection.Light, false));
                    while (KeywordReader.Read())
                    {
                        Keywords.Append(KeywordReader[IndicatorClassifications.ICName].ToString() + " ");
                    }
                    KeywordReader.Close();
                }

                Retval = Keywords.ToString();
                this._GalleryKeywords = Retval;
            }
            catch (Exception)
            {
                Retval = string.Empty;
                this._GalleryKeywords = string.Empty;
            }
            return Retval;
        }

        private void ApplyLanguageSettings()
        {
            try
            {
                DILanguage.Open(this._LanguageFilePath);
                LanguageStrings.Presentations = DILanguage.GetLanguageString("PRESENTATIONS");
                LanguageStrings.Galleries = DILanguage.GetLanguageString("GALLERY");
                LanguageStrings.Database = DILanguage.GetLanguageString("DATA");
                LanguageStrings.Selected = DILanguage.GetLanguageString("SELECTED");
                LanguageStrings.Count = DILanguage.GetLanguageString("COUNT");
                LanguageStrings.Home_Page_Selected_Caption = DILanguage.GetLanguageString("WHERE_DATA_EXISTS").ToLower();
                LanguageStrings.Available = DILanguage.GetLanguageString("AVAILABLE");
                LanguageStrings.Tables = DILanguage.GetLanguageString("Table");
                LanguageStrings.Graphs = DILanguage.GetLanguageString("Graph");
                LanguageStrings.Maps = DILanguage.GetLanguageString("Map");
                LanguageStrings.ViewGallery = DILanguage.GetLanguageString("VIEW");
                LanguageStrings.ViewData = DILanguage.GetLanguageString("VIEW_DATA");
                LanguageStrings.Indicators = DILanguage.GetLanguageString("MSG_26");
                LanguageStrings.Areas = DILanguage.GetLanguageString("MSG_28");
                LanguageStrings.TimePeriods = DILanguage.GetLanguageString("MSG_27");
                LanguageStrings.Sources = DILanguage.GetLanguageString("MSG_29");
                LanguageStrings.Indicator = DILanguage.GetLanguageString("INDICATOR");
                LanguageStrings.Area = DILanguage.GetLanguageString("AREA");
                LanguageStrings.TimePeriod = DILanguage.GetLanguageString("TIMEPERIOD");
                LanguageStrings.Source = DILanguage.GetLanguageString("SOURCECOMMON");
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Set the oder and selected property of wizard panel
        /// </summary>
        /// <param name="order"></param>
        private void UnSelectedPanel(int order)
        {
            // -- convert paneltype enum into array list
            ArrayList SelectedPanels = this.EnumToArray();

            foreach (string Panels in SelectedPanels)
            {
                if (this.SelectionOrder.IndexOf(Panels) == -1)
                {
                    switch (Panels)
                    {
                        case INDICATOR:
                            this.DIWizardPanels[DIWizardPanel.PanelType.Indicator].Selected = false;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Indicator].Order = order;
                            order += 1;
                            break;
                        case AREA:
                            this.DIWizardPanels[DIWizardPanel.PanelType.Area].Selected = false;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Area].Order = order;
                            order += 1;
                            break;
                        case TIMEPERIOD:
                            this.DIWizardPanels[DIWizardPanel.PanelType.TimePeriod].Selected = false;
                            this.DIWizardPanels[DIWizardPanel.PanelType.TimePeriod].Order = order;
                            order += 1;
                            break;
                        case SOURCE:
                            this.DIWizardPanels[DIWizardPanel.PanelType.Source].Selected = false;
                            this.DIWizardPanels[DIWizardPanel.PanelType.Source].Order = order;
                            order += 1;
                            break;
                        default:
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// Remove the selection, history of the panel.
        /// </summary>
        /// <param name="dIWizardPanel"></param>
        private void RemoveSelection(DIWizardPanel dIWizardPanel)
        {
            dIWizardPanel.Selected = false;
            dIWizardPanel.Order = 0;
            if (this._NavigationHistory.IndexOf(dIWizardPanel.Type.ToString()) != -1)
            {
                this._NavigationHistory.Remove(dIWizardPanel.Type.ToString());
            }
            if (this.SelectionOrder.IndexOf(dIWizardPanel.Type.ToString()) != -1)
            {
                this.SelectionOrder.Remove(dIWizardPanel.Type.ToString());
            }
        }

        /// <summary>
        /// Maintain the history of DI wizard.
        /// </summary>
        /// <param name="page"></param>
        private void AddToHistory(string page)
        {
            if (this._NavigationHistory.IndexOf(page) != -1)
            {
                // -- Remove the item, if already stored in the history.
                this._NavigationHistory.Remove(page);
            }
            // -- Add the new selection in the history.
            this._NavigationHistory.Add(page);
        }

        /// <summary>
        /// Delete the lower order selections from the history and empty its corresponding user selection items.
        /// </summary>
        private void UpdateHistory()
        {
            if (this._NavigationHistory.Count > 0 && this._NavigationHistory.IndexOf(this._SelectedPage.ToString()) != -1)
            {

                int SelectedPageIndex = this._NavigationHistory.IndexOf(this._SelectedPage.ToString());
                int HistoryCount = this._NavigationHistory.Count - 1;

                for (int i = HistoryCount; i > SelectedPageIndex; i--)
                {
                    switch (this._NavigationHistory[i])
                    {
                        case INDICATOR:
                            this._UserPreference.UserSelection.IndicatorNIds = string.Empty;
                            break;

                        case AREA:
                            this._UserPreference.UserSelection.AreaNIds = string.Empty;
                            break;

                        case TIMEPERIOD:
                            this._UserPreference.UserSelection.TimePeriodNIds = string.Empty;
                            break;

                        case SOURCE:
                            this._UserPreference.UserSelection.SourceNIds = string.Empty;
                            break;

                        default:
                            break;
                    }
                    this._NavigationHistory.RemoveAt(i);
                }
            }

            if (this._SelectionOrder.Count > 0 && this._SelectionOrder.IndexOf(this._SelectedPage.ToString()) != -1)
            {

                int SelectedPageIndex = this._SelectionOrder.IndexOf(this._SelectedPage.ToString());
                int HistoryCount = this._SelectionOrder.Count - 1;

                for (int i = HistoryCount; i > SelectedPageIndex; i--)
                {
                    switch (this._SelectionOrder[i])
                    {
                        case INDICATOR:
                            this._UserPreference.UserSelection.IndicatorNIds = string.Empty;
                            break;

                        case AREA:
                            this._UserPreference.UserSelection.AreaNIds = string.Empty;
                            break;

                        case TIMEPERIOD:
                            this._UserPreference.UserSelection.TimePeriodNIds = string.Empty;
                            break;

                        case SOURCE:
                            this._UserPreference.UserSelection.SourceNIds = string.Empty;
                            break;

                        default:
                            break;
                    }
                    this._SelectionOrder.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Update the old selection with the new selections
        /// </summary>
        /// <param name="autoSelectReader">Nid data reader</param>
        /// <param name="selectedNIds">old selections</param>
        /// <param name="idColumnName">Nid column name</param>
        /// <returns></returns>
        private string UpdateSelections(IDataReader autoSelectReader, string selectedNIds, string idColumnName)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder NewSelection = new StringBuilder();

                string[] SelectionID = new string[0];
                SelectionID = DICommon.SplitString(selectedNIds, ",");
                while (autoSelectReader.Read())
                {
                    foreach (string Ids in SelectionID)
                    {
                        // -- Match the selected NId with the new NIds
                        if (Convert.ToInt32(Ids) == Convert.ToInt32(autoSelectReader[idColumnName]))
                        {
                            NewSelection.Append("," + Ids);
                            break;
                        }
                    }
                }
                // -- Remove the trailing comma
                if (NewSelection.Length > 0)
                {
                    Retval = NewSelection.ToString().Substring(1);
                }
                else
                {
                    Retval = string.Empty;
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Update the old selection with the new selections
        /// </summary>
        /// <param name="autoSelectNids">Nid</param>
        /// <param name="selectedNids">old selections</param>
        /// <param name="idColumnName">Nid column name</param>
        /// <returns></returns>
        private string UpdateSelections(string autoSelectNids, string selectedNids)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder NewSelection = new StringBuilder();
                string[] SelectionID = new string[0];
                string[] AutoSelect = new string[0];

                SelectionID = DICommon.SplitString(selectedNids, ",");
                AutoSelect = DICommon.SplitString(autoSelectNids, ",");

                foreach (string Nid in AutoSelect)
                {
                    foreach (string Ids in SelectionID)
                    {
                        // -- Match the selected NId with the new NIds
                        if (Convert.ToInt32(Ids) == Convert.ToInt32(Nid))
                        {
                            NewSelection.Append("," + Ids);
                            break;
                        }
                    } 
                }

                if (NewSelection.Length > 0)
                {
                    // -- Remove the trailing comma
                    Retval = NewSelection.ToString().Substring(1);
                }
                else
                {
                    Retval = string.Empty;
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Update the selection order on the basis of selection done by the user
        /// </summary>
        /// <returns></returns>
        private int UpdateSelectionOrder(int lastSelectionIndex)
        {
            int Retval = 0;
            try
            {
                for (int PanelIndex = 0; PanelIndex < 4; PanelIndex++)
                {
                    if (this.DIWizardPanels[PanelIndex].Order == -1)
                    {
                        //-- Set the panel type as selected
                        this.DIWizardPanels[PanelIndex].Selected = true;
                        this.DIWizardPanels[PanelIndex].Order = lastSelectionIndex;
                        lastSelectionIndex += 1;

                        if (this.SelectionOrder.IndexOf(Convert.ToString((DIWizardPanel.PanelType)PanelIndex)) == -1)
                        {
                            // -- If not found, Insert the selection order on the bottom 
                            this.SelectionOrder.Insert(this.SelectionOrder.Count, Convert.ToString((DIWizardPanel.PanelType)PanelIndex));
                        }
                    }
                }
                Retval = lastSelectionIndex;
            }
            catch (Exception)
            {
                Retval = 0;
            }
            return Retval;
        }

        ///// <summary>
        ///// Check for the validity of IUS filter.
        ///// </summary>
        ///// <returns></returns>
        //private bool ValidIUSFilter()
        //{
        //    bool Retval = false;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
        //        {
        //            StringBuilder IUSNIds = new StringBuilder();
        //            // -- IUS NIds from Indicator NIds
        //            DataTable IUSDt = this.GetIUSNids();

        //            if (IUSDt.Rows.Count > 1)
        //            {
        //                // -- If multiple IUS exists.
        //                Retval = true;
        //            }

        //            // -- Get the comma seprated IUSNIDs from IUSDt
        //            foreach (DataRow Row in IUSDt.Rows)
        //            {
        //                IUSNIds.Append("," + Row[Data.IUSNId]);
        //            }

        //            // -- Store the indicator Nids.
        //            this.IndicatorNIds = this._UserPreference.UserSelection.IndicatorNIds;

        //            if (IUSNIds.Length > 0)
        //            {
        //                // -- Store the IUS NIds in user selection and set SHOW IUS=true
        //                this._UserPreference.UserSelection.IndicatorNIds = IUSNIds.ToString().Substring(1);
        //            }
        //            else
        //            {
        //                this._UserPreference.UserSelection.IndicatorNIds = string.Empty;
        //            }
        //        }
        //        this._UserPreference.UserSelection.ShowIUS = true;
        //        this._IsValidIUSFilter = Retval;
        //    }
        //    catch (Exception)
        //    {
        //        Retval = false;
        //    }
        //    return Retval;
        //}

        /// <summary>
        /// Save the record count of table in the XML
        /// </summary>
        private void SaveRecordCount()
        {
            DataView dvRecords;
            DataTable dtRecords;
            StringBuilder sbNids = new StringBuilder();
            string[] DistinctArray = new string[1];


            #region " -- Database -- "

            // -- Get the Data table Record count
            this.AvailableNIdCache.DataCount = this.GetDatabaseRecordCount();

            #endregion

            #region " -- Indicator -- "

            //// -- Get the Indicator table
            //dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Indicators.GetAutoSelectIndicator(string.Empty, string.Empty, string.Empty, FieldSelection.Light)).DefaultView;
            //// -- Store the record count in XML
            //this.AvailableNIdCache.IndicatorCount = dvRecords.Count;

            //// -- Sort it on the indicator_NId
            //dvRecords.Sort = Indicator.IndicatorNId;
            //// -- comma seprated string
            //foreach (DataRowView Row in dvRecords)
            //{
            //    sbNids.Append("," + Row[Indicator.IndicatorNId].ToString());
            //}


            // -- Get the IUS NIDs
            dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Data.GetDistinctNIds()).DefaultView;
            // -- Sort it on the indicator_NId
            dvRecords.Sort = Data.IUSNId;
            // -- comma seprated IUS NIds
            foreach (DataRowView Row in dvRecords)
            {
                sbNids.Append("," + Row[Data.IUSNId].ToString());
            }

            //-- Get the distinct Indicator count
            DistinctArray[0] = Data.IndicatorNId;
            dtRecords = dvRecords.ToTable(true, DistinctArray);

            // -- Store the record count in XML
            this.AvailableNIdCache.IndicatorCount = dtRecords.Rows.Count;

            if (sbNids.Length > 0)
            {
                // -- Remove the leading comma.
                this.AvailableNIdCache.IndicatorNIds = sbNids.ToString().Substring(1);
            }
            else
            {
                this.AvailableNIdCache.IndicatorNIds = string.Empty;
            }
            // -- reset the string builder
            sbNids.Length = 0;

            #endregion

            #region " -- Area -- "

            // -- Get the Area table
            dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Area.GetAutoSelectAreas(string.Empty,false ,string.Empty, string.Empty,-1)).DefaultView;
            // -- Store the record count in XML
            this.AvailableNIdCache.AreaCount = dvRecords.Count;

            // -- Get the Area level 2 table
            dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Area.GetAutoSelectAreas(string.Empty,false, string.Empty, string.Empty, this._SelectedAreaLevel)).DefaultView;
            // -- Sort it on the Area_Nid
            dvRecords.Sort = Area.AreaNId;
            // -- comma seprated string
            foreach (DataRowView Row in dvRecords)
            {
                sbNids.Append("," + Row[Area.AreaNId].ToString());
            }

            if (sbNids.Length > 0)
            {
                // -- Remove the leading comma.
                this.AvailableNIdCache.AreaNIds = sbNids.ToString().Substring(1);
            }
            else
            {
                this.AvailableNIdCache.AreaNIds = string.Empty;
            }

            // -- reset the string builder
            sbNids.Length = 0;

            #endregion

            #region " -- Time Period -- "

            // -- Get the Time period table
            dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Timeperiod.GetAutoSelectTimeperiod(string.Empty, false, string.Empty, string.Empty)).DefaultView;
            // -- Store the record count in XML
            this.AvailableNIdCache.TimeperiodCount = dvRecords.Count;

            // -- Sort it on the timeperiod_NId
            dvRecords.Sort = Timeperiods.TimePeriodNId;
            // -- comma seprated string
            foreach (DataRowView Row in dvRecords)
            {
                sbNids.Append("," + Row[Timeperiods.TimePeriodNId].ToString());
            }

            if (sbNids.Length > 0)
            {
                // -- Remove the leading comma.            
                this.AvailableNIdCache.TimeperiodNIds = sbNids.ToString().Substring(1);
            }
            else
            {
                this.AvailableNIdCache.TimeperiodNIds = string.Empty;
            }
            // -- reset the string builder
            sbNids.Length = 0;

            #endregion

            #region " -- Source -- "

            // -- Get the Source table
            dvRecords = this.DbConnection.ExecuteDataTable(this.DbQueries.Source.GetAutoSelectSource(string.Empty,false, string.Empty, string.Empty)).DefaultView;
            // -- Store the record count in XML
            this.AvailableNIdCache.SourceCount = dvRecords.Count;

            // -- Sort it on the IC_Nid
            dvRecords.Sort = IndicatorClassifications.ICNId;
            // -- comma seprated string
            foreach (DataRowView Row in dvRecords)
            {
                sbNids.Append("," + Row[IndicatorClassifications.ICNId].ToString());
            }

            if (sbNids.Length > 0)
            {
                // -- Remove the leading comma.
                this.AvailableNIdCache.SourceNIds = sbNids.ToString().Substring(1);
            }
            else
            {
                this.AvailableNIdCache.SourceNIds = string.Empty;
            }

            // -- reset the string builder
            sbNids.Length = 0;

            #endregion

            // -- Save the count and NIds in the file.
            try
            {
                this.AvailableNIdCache.Save(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME));
            }
            catch (Exception)
            {
            }
        }

        ///// <summary>
        ///// Retrieve the record count of the selections.
        ///// </summary>
        //private void GetRecordCount()
        //{
        //    XmlDocument XmlDocument = new XmlDocument();

        //    XmlDocument.Load(Path.Combine(this.DbNamePath, Constants.DB_CACHE_FILENAME));

        //    XmlNodeList XmlNodeLst = XmlDocument.SelectNodes("Wizard/Count");

        //    for (int Count = 0; Count < XmlNodeLst[0].ChildNodes.Count; Count++)
        //    {
        //        switch (Count)
        //        {
        //            case 0:
        //                this.AvailableNIdCache.DataCount = Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case 1:
        //                this.AvailableNIdCache.IndicatorCount = Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case 2:
        //                this.AvailableNIdCache.AreaCount = Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case 3:
        //                this.AvailableNIdCache.TimeperiodCount = Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case 4:
        //                this.AvailableNIdCache.SourceCount = Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //}

        /// <summary>
        /// Create the selected database directory in the cache folder, if not eixsts
        /// </summary>
        private void CreateDbFolder()
        {
            try
            {
                string DbName = Path.GetFileNameWithoutExtension(this._UserPreference.Database.SelectedDatasetName) + "_" + File.GetLastWriteTime(this._UserPreference.Database.SelectedConnectionDetail.DbName).ToString().Replace(@"/", "").Replace(":", "");
                this.DbNamePath = Path.Combine(Path.Combine(this.StockFolderPath, Constants.CACHE), DbName);
                if (!Directory.Exists(this.DbNamePath))
                {
                    // -- If not found create the file
                    Directory.CreateDirectory(DbNamePath);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Save the auto select cache file
        /// </summary>
        private void SaveAutoSelect()
        {
            string FileName = string.Empty;

            if (!this._IsFilterPage && this._SelectionOrder.Count == 1)
            {
                switch (this._SelectedPage)
                {
                    //case DIWizardPanel.PanelType.Gallery:
                    //    break;
                    case DIWizardPanel.PanelType.Database:
                        break;
                    case DIWizardPanel.PanelType.Indicator:
                        FileName = this.SelectionsFileName(this._UserPreference.UserSelection.IndicatorNIds, "I");
                        break;
                    case DIWizardPanel.PanelType.Area:
                        FileName = this.SelectionsFileName(this._UserPreference.UserSelection.AreaNIds, "A");
                        break;
                    case DIWizardPanel.PanelType.TimePeriod:
                        FileName = this.SelectionsFileName(this._UserPreference.UserSelection.TimePeriodNIds, "T");
                        break;
                    case DIWizardPanel.PanelType.Source:
                        FileName = this.SelectionsFileName(this._UserPreference.UserSelection.SourceNIds, "S");
                        break;
                    default:
                        break;
                }


                if (!File.Exists(Path.Combine(this.DbNamePath, FileName)))
                {
                    // -- Save the record count and availabe Nids
                    this.SaveAutoSelectNidsCount(FileName);
                }
                else
                {
                    // -- Get the records from the cache file.
                    this.GetAutoSelectRecords(FileName);
                }
            }
            else
            {
                // -- Get the record count and availabe Nids
                this.SaveAutoSelectNidsCount(FileName);
            }
        }

        /// <summary>
        /// Save the cache file with auto select record count and Nids.
        /// </summary>
        /// <param name="filenamePath"></param>
        private void SaveAutoSelectNidsCount(string filename)
        {
            try
            {
                //string CountXML = "<Selection><Count>";
                //string NIDsXMLList = "<AVLList>";
                StringBuilder NIds = new StringBuilder();
                string[] DistinctArray = new string[1];
                string AutoSelectedNids = string.Empty;

                // -- auto seelct Dataview
                DataView AutoSelectDv = this.GetAutoSelectedNIDs().DefaultView;

                foreach (DIWizardPanel Panel in DIWizardPanels)
                {
                    NIds.Length = 0;
                    switch (Panel.Type)
                    {
                        //case DIWizardPanel.PanelType.Gallery:
                        //    break;
                        case DIWizardPanel.PanelType.Database:

                            string FilterString = this.GetFilterString();
                            if (!string.IsNullOrEmpty(FilterString))
                            {
                                AutoSelectDv.RowFilter = FilterString;
                            }

                            if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Database))
                            {
                                this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Database, AutoSelectDv.Count);
                            }
                            else
                            {
                                this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database] = AutoSelectDv.Count;
                            }

                            // -- If file name exist, save the auto selection in the file.
                            if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                            {
                                this.AutoSelectNIdCache.DataCount = AutoSelectDv.Count;
                            }
                            AutoSelectDv.RowFilter = string.Empty;
                            break;
                        case DIWizardPanel.PanelType.Indicator:
                            //-- Nids of higher index will be updated
                            //if (this._SelectionStatus[DIWizardPanel.PanelType.Indicator] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
                            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
                            {
                                AutoSelectDv.Sort = Data.IUSNId;
                                // -- Get the distinct Indicator Nids
                                DistinctArray[0] = Data.IUSNId;
                                DataTable IndicatorDt = AutoSelectDv.ToTable(true, DistinctArray);

                                // -- Comma seprated indicator Nids
                                foreach (DataRow Row in IndicatorDt.Rows)
                                {
                                    NIds.Append("," + Row[Data.IUSNId].ToString());
                                }

                                //-- Get the distinct Indicator NIds, to get the indicator NIds count.
                                DistinctArray[0] = Data.IndicatorNId;
                                IndicatorDt = AutoSelectDv.ToTable(true, DistinctArray);


                                // -- If file name exist, save the auto selection in the file.
                                if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                                {
                                    this.AutoSelectNIdCache.IndicatorCount = IndicatorDt.Rows.Count;
                                    if (NIds.Length > 0)
                                    {
                                        this.AutoSelectNIdCache.IndicatorNIds = NIds.ToString().Substring(1);
                                    }
                                    else
                                    {
                                        this.AutoSelectNIdCache.IndicatorNIds = string.Empty;
                                    }
                                }

                                // -- Store the selection.
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, IndicatorDt.Rows.Count);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator] = IndicatorDt.Rows.Count;
                                }

                                if (NIds.Length > 0)
                                {
                                    AutoSelectedNids = NIds.ToString().Substring(1);
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, AutoSelectedNids);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator] = AutoSelectedNids;
                                }
                            }

                            else if (!this._SelectionStatus[DIWizardPanel.PanelType.Indicator] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
                            {
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, this.AvailableNIdCache.IndicatorCount);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator] = this.AvailableNIdCache.IndicatorCount;
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, this.AvailableNIdCache.IndicatorNIds);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator] = this.AvailableNIdCache.IndicatorNIds;
                                }
                                this.AutoSelectNIdCache.IndicatorCount = this.AvailableNIdCache.IndicatorCount;
                                this.AutoSelectNIdCache.IndicatorNIds = this.AvailableNIdCache.IndicatorNIds;
                            }

                            break;
                        case DIWizardPanel.PanelType.Area:
                            //if (this._SelectionStatus[DIWizardPanel.PanelType.Area] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
                            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
                            {
                                AutoSelectDv.RowFilter = Area.AreaLevel + " = " + this._SelectedAreaLevel;
                                AutoSelectDv.Sort = Data.AreaNId;
                                // -- Get the distinct Area Nids
                                DistinctArray[0] = Data.AreaNId;
                                DataTable AreaDt = AutoSelectDv.ToTable(true, DistinctArray);

                                // -- Comma seprated area Nids
                                foreach (DataRow Row in AreaDt.Rows)
                                {
                                    NIds.Append("," + Row[Data.AreaNId].ToString());
                                }

                                // -- Count of distinct area of all levels
                                AreaDt = AutoSelectDv.ToTable(true, DistinctArray);

                                // -- If file name exist, save the auto selection in the file.
                                if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                                {
                                    this.AutoSelectNIdCache.AreaCount = AreaDt.Rows.Count;
                                    if (NIds.Length > 0)
                                    {
                                        this.AutoSelectNIdCache.AreaNIds = NIds.ToString().Substring(1);
                                    }
                                    else
                                    {
                                        this.AutoSelectNIdCache.AreaNIds = string.Empty;
                                    }
                                }


                                // -- Store the selection.
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, AreaDt.Rows.Count);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area] = AreaDt.Rows.Count;
                                }

                                if (NIds.Length > 0)
                                {
                                    AutoSelectedNids = NIds.ToString().Substring(1);
                                }
                                else
                                {
                                    AutoSelectedNids = string.Empty;
                                }


                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, AutoSelectedNids);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Area] = AutoSelectedNids;
                                }

                                AutoSelectDv.RowFilter = string.Empty;
                            }

                            else if (!this._SelectionStatus[DIWizardPanel.PanelType.Area] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
                            {
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, this.AvailableNIdCache.AreaCount);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area] = this.AvailableNIdCache.AreaCount;
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, this.AvailableNIdCache.AreaNIds);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Area] = this.AvailableNIdCache.AreaNIds;
                                }
                                this.AutoSelectNIdCache.AreaCount = this.AvailableNIdCache.AreaCount;
                                this.AutoSelectNIdCache.AreaNIds = this.AvailableNIdCache.AreaNIds;
                            }
                            break;
                        case DIWizardPanel.PanelType.TimePeriod:
                            //if (this._SelectionStatus[DIWizardPanel.PanelType.TimePeriod] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
                            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
                            {
                                AutoSelectDv.Sort = Data.TimePeriodNId;
                                // -- Get the distinct time period Nids
                                DistinctArray[0] = Data.TimePeriodNId;
                                DataTable TimePeriodDt = AutoSelectDv.ToTable(true, DistinctArray);

                                foreach (DataRow Row in TimePeriodDt.Rows)
                                {
                                    NIds.Append("," + Row[Data.TimePeriodNId].ToString());
                                }

                                // -- If file name exist, save the auto selection in the file.
                                if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                                {
                                    this.AutoSelectNIdCache.TimeperiodCount = TimePeriodDt.Rows.Count;
                                    if (NIds.Length > 0)
                                    {
                                        this.AutoSelectNIdCache.TimeperiodNIds = NIds.ToString().Substring(1);
                                    }
                                    else
                                    {
                                        this.AutoSelectNIdCache.TimeperiodNIds = string.Empty;
                                    }
                                }

                                // -- Store the selection.
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, TimePeriodDt.Rows.Count);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod] = TimePeriodDt.Rows.Count;
                                }

                                if (NIds.Length > 0)
                                {
                                    AutoSelectedNids = NIds.ToString().Substring(1);
                                }
                                else
                                {
                                    AutoSelectedNids = string.Empty;
                                }


                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, AutoSelectedNids);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod] = AutoSelectedNids;
                                }
                            }
                            else if (!this._SelectionStatus[DIWizardPanel.PanelType.TimePeriod] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                            {
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, this.AvailableNIdCache.TimeperiodCount);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod] = this.AvailableNIdCache.TimeperiodCount;
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, this.AvailableNIdCache.TimeperiodNIds);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod] = this.AvailableNIdCache.TimeperiodNIds;
                                }
                                this.AutoSelectNIdCache.TimeperiodCount = this.AvailableNIdCache.TimeperiodCount;
                                this.AutoSelectNIdCache.TimeperiodNIds = this.AvailableNIdCache.TimeperiodNIds;
                            }
                            break;
                        case DIWizardPanel.PanelType.Source:
                            //if (this._SelectionStatus[DIWizardPanel.PanelType.Source] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
                            if (string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
                            {
                                //TODO Implement this
                                //AutoSelectDv.RowFilter = Indicator.IndicatorNId + " IN (" + this.UserPreference.UserSelection.IndicatorNIds + ")" + " AND " + Timeperiods.TimePeriodNId + " IN (" + this.UserPreference.UserSelection.TimePeriodNIds + ")" + " AND " + Area.AreaNId + " IN (" + this.UserPreference.UserSelection.AreaNIds + ")";
                                AutoSelectDv.Sort = Data.SourceNId;
                                // -- Get the distinct source Nids
                                DistinctArray[0] = Data.SourceNId;
                                DataTable SourceDt = AutoSelectDv.ToTable(true, DistinctArray);
                                string[] ICSources = new string[0];
                                List<string> DeletedSources = new List<string>();
                                int DistinctCount = 0;

                                // -- Comma seprated source Nids
                                foreach (DataRow Row in SourceDt.Rows)
                                {
                                    NIds.Append("," + Row[Data.SourceNId].ToString());
                                }

                                // -- If file name exist, save the auto selection in the file.
                                if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                                {
                                    this.AutoSelectNIdCache.SourceCount = SourceDt.Rows.Count;
                                    if (NIds.Length > 0)
                                    {
                                        this.AutoSelectNIdCache.SourceNIds = NIds.ToString().Substring(1);
                                    }
                                    else
                                    {
                                        this.AutoSelectNIdCache.SourceNIds = string.Empty;
                                    }
                                }

                                ICSources = DICommon.SplitString(this._UserPreference.UserSelection.DataViewFilters.DeletedSourceNIds, Delimiter.NUMERIC_DELIMITER);
                                foreach (string Sources in ICSources)
                                {
                                    string[] Source = new string[0];
                                    Source = DICommon.SplitString(Sources, Delimiter.NUMERIC_SEPARATOR);
                                    DeletedSources.Add(Source[1].Substring(0, Source[1].Length - 1));
                                }

                                DistinctCount = SourceDt.Rows.Count;

                                foreach (DataRow Row in SourceDt.Rows)
                                {
                                    foreach (string DeletedSource in DeletedSources)
                                    {
                                        if (Convert.ToInt32(Row[Data.SourceNId]) == Convert.ToInt32(DeletedSource))
                                        {
                                            DistinctCount -= 1;
                                            break;
                                        }
                                    }
                                }

                                // -- Store the selection.
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, DistinctCount);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source] = DistinctCount;
                                }

                                if (NIds.Length > 0)
                                {
                                    AutoSelectedNids = NIds.ToString().Substring(1);
                                }
                                else
                                {
                                    AutoSelectedNids = string.Empty;
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, AutoSelectedNids);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Source] = AutoSelectedNids;
                                }
                                AutoSelectDv.RowFilter = "";
                            }
                            else if (!this._SelectionStatus[DIWizardPanel.PanelType.Source] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
                            {
                                if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
                                {
                                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, this.AvailableNIdCache.SourceCount);
                                }
                                else
                                {
                                    this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source] = this.AvailableNIdCache.SourceCount;
                                }

                                if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
                                {
                                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, this.AvailableNIdCache.SourceNIds);
                                }
                                else
                                {
                                    this._AutoSelectRecords[DIWizardPanel.PanelType.Source] = this.AvailableNIdCache.SourceNIds;
                                }
                                this.AutoSelectNIdCache.SourceNIds = this.AvailableNIdCache.SourceNIds;
                                this.AutoSelectNIdCache.SourceCount = this.AvailableNIdCache.SourceCount;
                            }

                            break;
                        default:
                            break;
                    }
                }

                // -- If file name exist, save the auto selection in the file.
                if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
                {
                    this.AutoSelectNIdCache.Save(Path.Combine(this.DbNamePath, filename));
                }
            }
            catch (Exception ex)
            {
            }
        }


        ///// <summary>
        ///// Save the cache file with auto select record count and Nids.
        ///// </summary>
        ///// <param name="filenamePath"></param>
        //private void SaveAutoSelectNidsCount(string filename)
        //{
        //    try
        //    {
        //        //string CountXML = "<Selection><Count>";
        //        //string NIDsXMLList = "<AVLList>";
        //        StringBuilder NIds = new StringBuilder();
        //        string[] DistinctArray = new string[1];
        //        string AutoSelectedNids = string.Empty;

        //        // -- auto seelct Dataview
        //        DataView AutoSelectDv = this.GetAutoSelectedNIDs().DefaultView;

        //        foreach (DIWizardPanel Panel in DIWizardPanels)
        //        {
        //            NIds.Length = 0;
        //            switch (Panel.Type)
        //            {
        //                //case DIWizardPanel.PanelType.Gallery:
        //                //    break;
        //                case DIWizardPanel.PanelType.Database:

        //                    string FilterString = this.GetFilterString();
        //                    if (!string.IsNullOrEmpty(FilterString))
        //                    {
        //                        AutoSelectDv.RowFilter = FilterString;
        //                    }

        //                    if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Database))
        //                    {
        //                        this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Database, AutoSelectDv.Count);
        //                    }
        //                    else
        //                    {
        //                        this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database] = AutoSelectDv.Count;
        //                    }

        //                    // -- If file name exist, save the auto selection in the file.
        //                    if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //                    {
        //                        this.AutoSelectNIdCache.DataCount = AutoSelectDv.Count;
        //                    }
        //                    AutoSelectDv.RowFilter = string.Empty;
        //                    break;
        //                case DIWizardPanel.PanelType.Indicator:
        //                    //-- Nids of higher index will be updated
        //                    if (this._SelectionStatus[DIWizardPanel.PanelType.Indicator] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString())==-1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
        //                    {
        //                        AutoSelectDv.Sort = Data.IUSNId;
        //                        // -- Get the distinct Indicator Nids
        //                        DistinctArray[0] = Data.IUSNId;
        //                        DataTable IndicatorDt = AutoSelectDv.ToTable(true, DistinctArray);

        //                        // -- Comma seprated indicator Nids
        //                        foreach (DataRow Row in IndicatorDt.Rows)
        //                        {
        //                            NIds.Append("," + Row[Data.IUSNId].ToString());
        //                        }

        //                        //-- Get the distinct Indicator NIds, to get the indicator NIds count.
        //                        DistinctArray[0] = Data.IndicatorNId;
        //                        IndicatorDt = AutoSelectDv.ToTable(true, DistinctArray);


        //                        // -- If file name exist, save the auto selection in the file.
        //                        if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //                        {
        //                            this.AutoSelectNIdCache.IndicatorCount = IndicatorDt.Rows.Count;
        //                            if (NIds.Length > 0)
        //                            {
        //                                this.AutoSelectNIdCache.IndicatorNIds = NIds.ToString().Substring(1);
        //                            }
        //                            else
        //                            {
        //                                this.AutoSelectNIdCache.IndicatorNIds = string.Empty;
        //                            }
        //                        }

        //                        // -- Store the selection.
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, IndicatorDt.Rows.Count);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator] = IndicatorDt.Rows.Count;
        //                        }

        //                        if (NIds.Length > 0)
        //                        {
        //                            AutoSelectedNids = NIds.ToString().Substring(1);
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, AutoSelectedNids);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator] = AutoSelectedNids;
        //                        }
        //                    }

        //                    else if (!this._SelectionStatus[DIWizardPanel.PanelType.Indicator] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
        //                    {
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, this.AvailableNIdCache.IndicatorCount);                                    
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator] = this.AvailableNIdCache.IndicatorCount;
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, this.AvailableNIdCache.IndicatorNIds);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator] = this.AvailableNIdCache.IndicatorNIds;
        //                        }
        //                        this.AutoSelectNIdCache.IndicatorCount = this.AvailableNIdCache.IndicatorCount;
        //                        this.AutoSelectNIdCache.IndicatorNIds = this.AvailableNIdCache.IndicatorNIds;
        //                    }

        //                    break;
        //                case DIWizardPanel.PanelType.Area:
        //                    if (this._SelectionStatus[DIWizardPanel.PanelType.Area] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
        //                    {
        //                        AutoSelectDv.RowFilter = Area.AreaLevel + " = " + this._SelectedAreaLevel;
        //                        AutoSelectDv.Sort = Data.AreaNId;
        //                        // -- Get the distinct Area Nids
        //                        DistinctArray[0] = Data.AreaNId;
        //                        DataTable AreaDt = AutoSelectDv.ToTable(true, DistinctArray);

        //                        // -- Comma seprated area Nids
        //                        foreach (DataRow Row in AreaDt.Rows)
        //                        {
        //                            NIds.Append("," + Row[Data.AreaNId].ToString());
        //                        }
                               
        //                        // -- Count of distinct area of all levels
        //                        AreaDt = AutoSelectDv.ToTable(true, DistinctArray);

        //                        // -- If file name exist, save the auto selection in the file.
        //                        if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //                        {
        //                            this.AutoSelectNIdCache.AreaCount = AreaDt.Rows.Count;
        //                            if (NIds.Length > 0)
        //                            {
        //                                this.AutoSelectNIdCache.AreaNIds = NIds.ToString().Substring(1);
        //                            }
        //                            else
        //                            {
        //                                this.AutoSelectNIdCache.AreaNIds = string.Empty;
        //                            }
        //                        }                              


        //                        // -- Store the selection.
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, AreaDt.Rows.Count);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area] = AreaDt.Rows.Count;
        //                        }

        //                        if (NIds.Length > 0)
        //                        {
        //                            AutoSelectedNids = NIds.ToString().Substring(1);
        //                        }
        //                        else
        //                        {
        //                            AutoSelectedNids = string.Empty;
        //                        }


        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, AutoSelectedNids);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Area] = AutoSelectedNids;
        //                        }

        //                        AutoSelectDv.RowFilter = string.Empty;
        //                    }

        //                    else if (!this._SelectionStatus[DIWizardPanel.PanelType.Area] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
        //                    {
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, this.AvailableNIdCache.AreaCount);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area] = this.AvailableNIdCache.AreaCount;
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, this.AvailableNIdCache.AreaNIds);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Area] = this.AvailableNIdCache.AreaNIds;
        //                        }
        //                        this.AutoSelectNIdCache.AreaCount = this.AvailableNIdCache.AreaCount;
        //                        this.AutoSelectNIdCache.AreaNIds = this.AvailableNIdCache.AreaNIds;
        //                    }
        //                    break;
        //                case DIWizardPanel.PanelType.TimePeriod:
        //                    if (this._SelectionStatus[DIWizardPanel.PanelType.TimePeriod] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
        //                    {
        //                        AutoSelectDv.Sort = Data.TimePeriodNId;
        //                        // -- Get the distinct time period Nids
        //                        DistinctArray[0] = Data.TimePeriodNId;
        //                        DataTable TimePeriodDt = AutoSelectDv.ToTable(true, DistinctArray);

        //                        foreach (DataRow Row in TimePeriodDt.Rows)
        //                        {
        //                            NIds.Append("," + Row[Data.TimePeriodNId].ToString());
        //                        }

        //                        // -- If file name exist, save the auto selection in the file.
        //                        if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //                        {
        //                            this.AutoSelectNIdCache.TimeperiodCount = TimePeriodDt.Rows.Count;
        //                            if (NIds.Length > 0)
        //                            {
        //                                this.AutoSelectNIdCache.TimeperiodNIds = NIds.ToString().Substring(1);
        //                            }
        //                            else
        //                            {
        //                                this.AutoSelectNIdCache.TimeperiodNIds = string.Empty;
        //                            }
        //                        }

        //                        // -- Store the selection.
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, TimePeriodDt.Rows.Count);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod] = TimePeriodDt.Rows.Count;
        //                        }

        //                        if (NIds.Length > 0)
        //                        {
        //                            AutoSelectedNids = NIds.ToString().Substring(1);
        //                        }
        //                        else
        //                        {
        //                            AutoSelectedNids = string.Empty;
        //                        }


        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, AutoSelectedNids);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod] = AutoSelectedNids;
        //                        }
        //                    }
        //                    else if (!this._SelectionStatus[DIWizardPanel.PanelType.TimePeriod] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
        //                    {
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, this.AvailableNIdCache.TimeperiodCount);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod] = this.AvailableNIdCache.TimeperiodCount;
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, this.AvailableNIdCache.TimeperiodNIds);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod] = this.AvailableNIdCache.TimeperiodNIds;
        //                        }
        //                        this.AutoSelectNIdCache.TimeperiodCount = this.AvailableNIdCache.TimeperiodCount;
        //                        this.AutoSelectNIdCache.TimeperiodNIds = this.AvailableNIdCache.TimeperiodNIds;
        //                    }                            
        //                    break;
        //                case DIWizardPanel.PanelType.Source:
        //                    if (this._SelectionStatus[DIWizardPanel.PanelType.Source] & (this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString()) == -1 || this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString()) > this._SelectionOrder.IndexOf(this.SelectedPage.ToString())))
        //                    {
        //                        //TODO Implement this
        //                        //AutoSelectDv.RowFilter = Indicator.IndicatorNId + " IN (" + this.UserPreference.UserSelection.IndicatorNIds + ")" + " AND " + Timeperiods.TimePeriodNId + " IN (" + this.UserPreference.UserSelection.TimePeriodNIds + ")" + " AND " + Area.AreaNId + " IN (" + this.UserPreference.UserSelection.AreaNIds + ")";
        //                        AutoSelectDv.Sort = Data.SourceNId;
        //                        // -- Get the distinct source Nids
        //                        DistinctArray[0] = Data.SourceNId;
        //                        DataTable SourceDt = AutoSelectDv.ToTable(true, DistinctArray);

        //                        // -- Comma seprated source Nids
        //                        foreach (DataRow Row in SourceDt.Rows)
        //                        {
        //                            NIds.Append("," + Row[Data.SourceNId].ToString());
        //                        }

        //                        // -- If file name exist, save the auto selection in the file.
        //                        if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //                        {
        //                            this.AutoSelectNIdCache.SourceCount = SourceDt.Rows.Count;
        //                            if (NIds.Length > 0)
        //                            {
        //                                this.AutoSelectNIdCache.SourceNIds = NIds.ToString().Substring(1);
        //                            }
        //                            else
        //                            {
        //                                this.AutoSelectNIdCache.SourceNIds = string.Empty;
        //                            }
        //                        }

        //                        // -- Store the selection.
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, SourceDt.Rows.Count);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source] = SourceDt.Rows.Count;
        //                        }

        //                        if (NIds.Length > 0)
        //                        {
        //                            AutoSelectedNids = NIds.ToString().Substring(1);
        //                        }
        //                        else
        //                        {
        //                            AutoSelectedNids = string.Empty;
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, AutoSelectedNids);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Source] = AutoSelectedNids;
        //                        }
        //                        AutoSelectDv.RowFilter = "";
        //                    }
        //                    else if (!this._SelectionStatus[DIWizardPanel.PanelType.Source] && !this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
        //                    {
        //                        if (!this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
        //                        {
        //                            this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, this.AvailableNIdCache.SourceCount);
        //                        }
        //                        else
        //                        {
        //                            this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source] = this.AvailableNIdCache.SourceCount;
        //                        }

        //                        if (!this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
        //                        {
        //                            this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, this.AvailableNIdCache.SourceNIds);
        //                        }
        //                        else
        //                        {
        //                            this._AutoSelectRecords[DIWizardPanel.PanelType.Source] = this.AvailableNIdCache.SourceNIds;
        //                        }
        //                        this.AutoSelectNIdCache.SourceNIds = this.AvailableNIdCache.SourceNIds;
        //                        this.AutoSelectNIdCache.SourceCount = this.AvailableNIdCache.SourceCount;
        //                    }

        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
                
        //        // -- If file name exist, save the auto selection in the file.
        //        if (!string.IsNullOrEmpty(filename) && Path.Combine(this.DbNamePath, filename).Length < 255)
        //        {
        //            this.AutoSelectNIdCache.Save(Path.Combine(this.DbNamePath, filename));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        /// <summary>
        /// Generates the file name.
        /// </summary>
        /// <param name="selections"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string SelectionsFileName(string selections, string prefix)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(selections))
                {
                    StringBuilder FileName = new StringBuilder();
                    List<int> SelectedNIds = new List<int>();
                    string[] NIds = new string[0];
                    // -- Split the selected nids
                    NIds = DICommon.SplitString(selections, ",");

                    // -- sort the selected Nids
                    foreach (string Nid in NIds)
                    {
                        SelectedNIds.Add(Convert.ToInt32(Nid));
                    }
                    SelectedNIds.Sort();

                    // -- Build the file name
                    FileName.Append(prefix);
                    foreach (int Nid in SelectedNIds)
                    {
                        FileName.Append(Nid + "_");
                    }
                    if (FileName.Length > 0)
                    {
                        Retval = FileName.ToString().Substring(0, FileName.Length - 1) + DICommon.FileExtension.XML;
                    }
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Get the auto select record count and its Nids
        /// </summary>
        /// <param name="filename"></param>
        private void GetAutoSelectRecords(string filename)
        {
            this.AutoSelectNIdCache = CacheInfo.Load(Path.Combine(this.DbNamePath, filename));
            if (this.AutoSelectNIdCache != null)
            {
				//-- Database
				if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Database))
				{
					this.AutoSelectRecordCount[DIWizardPanel.PanelType.Database]= this.AutoSelectNIdCache.DataCount;
				}
				else
				{
					this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Database, this.AutoSelectNIdCache.DataCount);
				}

				//-- Indicator
				if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Indicator))
				{
					this.AutoSelectRecordCount[DIWizardPanel.PanelType.Indicator]= this.AutoSelectNIdCache.IndicatorCount;
				}
				else
				{
					this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, this.AutoSelectNIdCache.IndicatorCount);
				}

				// -- Area
				if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Area))
				{
					this.AutoSelectRecordCount[DIWizardPanel.PanelType.Area]= this.AutoSelectNIdCache.AreaCount;
				}
				else
				{
					this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, this.AutoSelectNIdCache.AreaCount);
				}

				//-- Timr period
				if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
				{
					this.AutoSelectRecordCount[DIWizardPanel.PanelType.TimePeriod]= this.AutoSelectNIdCache.TimeperiodCount;
				}
				else
				{
					this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, this.AutoSelectNIdCache.TimeperiodCount);
				}

				//-- source
				if (this.AutoSelectRecordCount.ContainsKey(DIWizardPanel.PanelType.Source))
				{
					this.AutoSelectRecordCount[DIWizardPanel.PanelType.Source]= this.AutoSelectNIdCache.SourceCount;
				}
				else
				{
					this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, this.AutoSelectNIdCache.SourceCount);
				}


				// -- Indicator NIds
				if (this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Indicator))
				{
					this._AutoSelectRecords[DIWizardPanel.PanelType.Indicator]=this.AutoSelectNIdCache.IndicatorNIds;
				}
				else
				{
					this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, this.AutoSelectNIdCache.IndicatorNIds);
				}

				//-- Area Nids
				if (this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Area))
				{
					this._AutoSelectRecords[DIWizardPanel.PanelType.Area]=this.AutoSelectNIdCache.AreaNIds;
				}
				else
				{
					this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, this.AutoSelectNIdCache.AreaNIds);
				}

				// -- Time period NIds
				if (this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.TimePeriod))
				{
					this._AutoSelectRecords[DIWizardPanel.PanelType.TimePeriod]= this.AutoSelectNIdCache.TimeperiodNIds;
				}
				else
				{
					this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, this.AutoSelectNIdCache.TimeperiodNIds);
				}

				// -- Source
				if (this._AutoSelectRecords.ContainsKey(DIWizardPanel.PanelType.Source))
				{
					this._AutoSelectRecords[DIWizardPanel.PanelType.Source]= this.AutoSelectNIdCache.SourceNIds;
				}
				else
				{																	  
					this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, this.AutoSelectNIdCache.SourceNIds);
				}
            }
            else
            {
                this.AutoSelectNIdCache = new CacheInfo();
                this.SaveAutoSelectNidsCount(filename);
            }

            //this.AutoSelectRecordCount = new Dictionary<DIWizardPanel.PanelType, int>();
            //this._AutoSelectRecords = new Dictionary<DIWizardPanel.PanelType, string>();

            //XmlDocument XmlDocument = new XmlDocument();

            //XmlDocument.Load(Path.Combine(this.DbNamePath, filename));

            //XmlNodeList XmlNodeLst = XmlDocument.SelectNodes("Selection");

            //for (int Count = 0; Count < XmlNodeLst[0].ChildNodes.Count; Count++)
            //{
            //    // -- loop to store the auto select record count and its NIds
            //    for (int ChildCount = 0; ChildCount < XmlNodeLst[0].ChildNodes[Count].ChildNodes.Count; ChildCount++)
            //    {
            //        switch (XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].Name.ToLower())
            //        {
            //            case "dv":
            //                if (XmlNodeLst[0].ChildNodes[Count].Name.ToLower() == "count")
            //                {
            //                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Database, Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText));
            //                }
            //                break;
            //            case "i":
            //                if (XmlNodeLst[0].ChildNodes[Count].Name.ToLower() == "count")
            //                {
            //                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Indicator, Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText));
            //                }
            //                else
            //                {
            //                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Indicator, XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText);
            //                }
            //                break;
            //            case "a":
            //                if (XmlNodeLst[0].ChildNodes[Count].Name.ToLower() == "count")
            //                {
            //                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Area, Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText));
            //                }
            //                else
            //                {
            //                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Area, XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText);
            //                }
            //                break;
            //            case "t":
            //                if (XmlNodeLst[0].ChildNodes[Count].Name.ToLower() == "count")
            //                {
            //                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.TimePeriod, Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText));
            //                }
            //                else
            //                {
            //                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.TimePeriod, XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText);
            //                }
            //                break;
            //            case "s":
            //                if (XmlNodeLst[0].ChildNodes[Count].Name.ToLower() == "count")
            //                {
            //                    this.AutoSelectRecordCount.Add(DIWizardPanel.PanelType.Source, Convert.ToInt32(XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText));
            //                }
            //                else
            //                {
            //                    this._AutoSelectRecords.Add(DIWizardPanel.PanelType.Source, XmlNodeLst[0].ChildNodes[Count].ChildNodes[ChildCount].InnerText);
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
        }

        ///// <summary>
        /////  Get the available Nids from the cache
        ///// </summary>
        ///// <param name="filename"></param>
        //private void GetAvailableRecords(string filename)
        //{
        //    this._AvailableNIds = new Dictionary<DIWizardPanel.PanelType, string>();

        //    XmlDocument XmlDocument = new XmlDocument();

        //    XmlDocument.Load(Path.Combine(this.DbNamePath, filename));

        //    XmlNodeList XmlNodeLst = XmlDocument.SelectNodes("Wizard/AVLList");

        //    for (int Count = 0; Count < XmlNodeLst[0].ChildNodes.Count; Count++)
        //    {

        //        switch (XmlNodeLst[0].ChildNodes[Count].Name.ToLower())
        //        {
        //            case "i":

        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Indicator, XmlNodeLst[0].ChildNodes[Count].InnerText);

        //                break;
        //            case "a":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Area, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case "tp":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.TimePeriod, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case "s":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Source, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            default:
        //                break;
        //        }

        //    }
        //}

        /// <summary>
        /// Build the selection status list.
        /// </summary>
        private void BuildSelectionList()
        {
            this._SelectionStatus = new Dictionary<DIWizardPanel.PanelType, bool>();

            ArrayList PanelTypes = new ArrayList();
            PanelTypes = this.EnumToArray();

            for (int PanelIndex = 0; PanelIndex < PanelTypes.Count - 1; PanelIndex++)
            {
                this._SelectionStatus.Add((DIWizardPanel.PanelType)PanelIndex, true);
            }
        }

        ///// <summary>
        /////  Get the available Nids from the cache
        ///// </summary>
        ///// <param name="filename"></param>
        //private void GetAvailableRecords(string filename)
        //{
        //    this._AvailableNIds = new Dictionary<DIWizardPanel.PanelType, string>();

        //    XmlDocument XmlDocument = new XmlDocument();

        //    XmlDocument.Load(Path.Combine(this.DbNamePath, filename));

        //    XmlNodeList XmlNodeLst = XmlDocument.SelectNodes("Wizard/AVLList");

        //    for (int Count = 0; Count < XmlNodeLst[0].ChildNodes.Count; Count++)
        //    {

        //        switch (XmlNodeLst[0].ChildNodes[Count].Name.ToLower())
        //        {
        //            case "i":

        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Indicator, XmlNodeLst[0].ChildNodes[Count].InnerText);

        //                break;
        //            case "a":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Area, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case "tp":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.TimePeriod, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            case "s":
        //                this._AvailableNIds.Add(DIWizardPanel.PanelType.Source, XmlNodeLst[0].ChildNodes[Count].InnerText);
        //                break;
        //            default:
        //                break;
        //        }

        //    }
        //}

        /// <summary>
        /// Build the selection Count list.
        /// </summary>
        private void BuildSelectionCountList()
        {
            this._SelectionCount = new Dictionary<DIWizardPanel.PanelType, string>();

            ArrayList PanelTypes = new ArrayList();
            PanelTypes = this.EnumToArray();

            for (int PanelIndex = 0; PanelIndex < PanelTypes.Count - 1; PanelIndex++)
            {
                this._SelectionCount.Add((DIWizardPanel.PanelType)PanelIndex, "0");
            }
        }

        /// <summary>
        /// Get the filter string to filter down the auto select data table
        /// </summary>
        /// <returns></returns>
        private string GetFilterString()
        {
            string Retval = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
                {
                    Retval = Data.IUSNId + " IN (" + this._UserPreference.UserSelection.IndicatorNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
                {
                    if (!string.IsNullOrEmpty(Retval))
                    {
                        Retval += " AND " + Data.AreaNId + " IN (" + this._UserPreference.UserSelection.AreaNIds + ")";
                    }
                    else
                    {
                        Retval = Data.AreaNId + " IN (" + this._UserPreference.UserSelection.AreaNIds + ")";
                    }

                }

                if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
                {
                    if (!string.IsNullOrEmpty(Retval))
                    {
                        Retval += " AND " + Data.TimePeriodNId + " IN (" + this._UserPreference.UserSelection.TimePeriodNIds + ")";
                    }
                    else
                    {
                        Retval = Data.TimePeriodNId + " IN (" + this._UserPreference.UserSelection.TimePeriodNIds + ")";
                    }
                }

                if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
                {
                    if (!string.IsNullOrEmpty(Retval))
                    {
                        Retval += " AND " + Data.SourceNId + " IN (" + this._UserPreference.UserSelection.SourceNIds + ")";
                    }
                    else
                    {
                        Retval = Data.SourceNId + " IN (" + this._UserPreference.UserSelection.SourceNIds + ")";
                    }
                }


                //if (this._SelectionOrder.IndexOf(this._SelectedPage.ToString()) < this._SelectionOrder.Count - 1)
                //{
                //    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds))
                //    {
                //        Retval = Data.IUSNId + " IN (" + this._UserPreference.UserSelection.IndicatorNIds + ")";
                //    }

                //    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds))
                //    {
                //        if (!string.IsNullOrEmpty(Retval))
                //        {
                //            Retval += " AND " + Data.AreaNId + " IN (" + this._UserPreference.UserSelection.AreaNIds + ")";
                //        }
                //        else
                //        {
                //            Retval = Data.AreaNId + " IN (" + this._UserPreference.UserSelection.AreaNIds + ")";
                //        }
                        
                //    }

                //    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds))
                //    {
                //        if (!string.IsNullOrEmpty(Retval))
                //        {
                //            Retval += " AND " + Data.TimePeriodNId + " IN (" + this._UserPreference.UserSelection.TimePeriodNIds + ")";
                //        }
                //        else
                //        {
                //            Retval = Data.TimePeriodNId + " IN (" + this._UserPreference.UserSelection.TimePeriodNIds + ")";
                //        }
                //    }

                //    if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds))
                //    {
                //        if (!string.IsNullOrEmpty(Retval))
                //        {
                //            Retval += " AND " + Data.SourceNId + " IN (" + this._UserPreference.UserSelection.SourceNIds + ")";
                //        }
                //        else
                //        {
                //            Retval = Data.SourceNId + " IN (" + this._UserPreference.UserSelection.SourceNIds + ")";
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        private Int32 GetDatabaseRecordCount()
        {
            Int32 Retval = 0;
            try
            {
                // -- Get the number of records in the data table
                object RecordCount = this.DbConnection.ExecuteScalarSqlQuery(this.DbQueries.Data.GetRecordCount());  //"SELECT COUNT(" + Data.DataNId + ") FROM UT_Data");

                Retval = Convert.ToInt32(RecordCount);
            }
            catch (Exception)
            {
                Retval = 0;
            }
            return Retval;
        }

        //private Int32 GetIndicatorRecordCount()
        //{
        //    Int32 Retval = 0;
        //    try
        //    {
        //        // -- Get the number of records in the data table
        //        object RecordCount = this.DbConnection.ExecuteScalarSqlQuery("SELECT COUNT(" + Indicator.IndicatorNId + ") FROM UT_Indicator_en");

        //        Retval = Convert.ToInt32(RecordCount);
        //    }
        //    catch (Exception)
        //    {
        //        Retval = 0;
        //    }
        //    return Retval;
        //}

        //private Int32 GetAreaRecordCount()
        //{
        //    Int32 Retval = 0;
        //    try
        //    {
        //        // -- Get the number of records in the data table
        //        object RecordCount = this.DbConnection.ExecuteScalarSqlQuery("SELECT COUNT(" + Area.AreaNId + ") FROM UT_Area_en");

        //        Retval = Convert.ToInt32(RecordCount);
        //    }
        //    catch (Exception)
        //    {
        //        Retval = 0;
        //    }
        //    return Retval;
        //}

        //private Int32 GetTimePeriodRecordCount()
        //{
        //    Int32 Retval = 0;
        //    try
        //    {
        //        // -- Get the number of records in the data table
        //        Object RecordCount = this.DbConnection.ExecuteScalarSqlQuery("SELECT COUNT(" + IndicatorClassifications.ICNId + ") FROM UT_Indicator_Classifications_en WHERE " + IndicatorClassifications.ICType + " = 'SR'");

        //        Retval = Convert.ToInt32(RecordCount);
        //    }
        //    catch (Exception)
        //    {
        //        Retval = 0;
        //    }
        //    return Retval;
        //}

        //private Int32 GetSourceRecordCount()
        //{
        //    Int32 Retval = 0;
        //    try
        //    {
        //        // -- Get the number of records in the data table
        //        Object RecordCount = this.DbConnection.ExecuteScalarSqlQuery("SELECT COUNT(" + Timeperiods.TimePeriodNId + ") FROM UT_TimePeriod");

        //        Retval = Convert.ToInt32(RecordCount);
        //    }
        //    catch (Exception)
        //    {
        //        Retval = 0;
        //    }
        //    return Retval;
        //}

        private DataTable GetAutoSelectedNIDs()
        {
            DataTable Retval=new DataTable();
            try
            {
                string IndicatorNIds = string.Empty;
                string AreaNIds = string.Empty;
                string TimePeriodNIds = string.Empty;
                string SourceNIds = string.Empty;

                int SelectedPanelIndex = this._SelectionOrder.IndexOf(this._SelectedPage.ToString());
                int PanelIndex = 0;


                AreaNIds = this._UserPreference.UserSelection.AreaNIds;
                SourceNIds = this._UserPreference.UserSelection.SourceNIds;
                IndicatorNIds = this._UserPreference.UserSelection.IndicatorNIds;
                TimePeriodNIds = this._UserPreference.UserSelection.TimePeriodNIds;


                //PanelIndex = this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Area.ToString());
                //if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.AreaNIds) && (PanelIndex == -1 || PanelIndex <= SelectedPanelIndex))
                //{
                //    AreaNIds = this._UserPreference.UserSelection.AreaNIds;
                //}

                //PanelIndex = this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Source.ToString());
                //if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.SourceNIds) && (PanelIndex == -1 || PanelIndex <= SelectedPanelIndex))
                //{
                //    SourceNIds = this._UserPreference.UserSelection.SourceNIds;
                //}

                //PanelIndex = this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString());
                //if (this._UserPreference.UserSelection.ShowIUS && !string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds) && (PanelIndex == -1 || PanelIndex <= SelectedPanelIndex))
                //{
                //    IndicatorNIds = this._UserPreference.UserSelection.IndicatorNIds;
                //}

                //PanelIndex = this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.Indicator.ToString());
                //if (!this._UserPreference.UserSelection.ShowIUS && !string.IsNullOrEmpty(this._UserPreference.UserSelection.IndicatorNIds) && (PanelIndex == -1 || PanelIndex <= SelectedPanelIndex))
                //{
                //    IndicatorNIds = this._UserPreference.UserSelection.IndicatorNIds;
                //}

                //PanelIndex = this._SelectionOrder.IndexOf(DIWizardPanel.PanelType.TimePeriod.ToString());
                //if (!string.IsNullOrEmpty(this._UserPreference.UserSelection.TimePeriodNIds) && (PanelIndex == -1 || PanelIndex <= SelectedPanelIndex))
                //{
                //    TimePeriodNIds = this._UserPreference.UserSelection.TimePeriodNIds;
                //}

                Retval = this.DbConnection.ExecuteDataTable(this.DbQueries.Data.GetDataWAutoSelectedNIDs(this._UserPreference.UserSelection.ShowIUS, IndicatorNIds, AreaNIds, TimePeriodNIds, SourceNIds));

            }
            catch (Exception)   
            {

                Retval = null;
            }
            return Retval;

        }       
   
        #endregion

        #endregion

    }
}