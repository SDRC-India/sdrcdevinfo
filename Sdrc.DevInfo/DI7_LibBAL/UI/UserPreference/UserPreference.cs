using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibBAL.UI.UserPreference
{

    #region " -- Delegates -- "

    /// <summary>
    /// Delegate for change in Data content
    /// </summary>
    public delegate void DataContentChangedDelegate();

    /// <summary>
    /// Delegate for change in Data Layout
    /// </summary>
    public delegate void DataLayoutChangedDelegate();

    /// <summary>
    /// Delegate for change in Area content
    /// </summary>
    public delegate void AreaContentChangedDelegate();

    /// <summary>
    /// Delegate for change in Indicator Content
    /// </summary>
    public delegate void IndicatorContentChangedDelegate();

    /// <summary>
    /// Delegate for change in Time period Content
    /// </summary>
    public delegate void TimePeriodContentChangedDelegate();

    /// <summary>
    /// Delegate for change in preview limit properties.
    /// </summary>
    public delegate void PreviewChangedDelegate(Presentation.PresentationType presentationType);

    #endregion

    /// <summary>
    /// Represents User Preferences which were previously stored in pref.xml
    /// </summary>
    public class UserPreference : ICloneable, IDisposable
    {
        #region " -- Events -- "

        /// <summary>
        /// Event for change in Data content
        /// </summary>
        public event DataContentChangedDelegate DataContentChangedEvent;

        /// <summary>
        /// Event for change in Data Layout
        /// </summary>
        public event DataLayoutChangedDelegate DataLayoutChangedEvent;

        /// <summary>
        /// Event for change in Area content
        /// </summary>
        public event AreaContentChangedDelegate AreaContentChangedEvent;

        /// <summary>
        /// Event for change in Indicator Content
        /// </summary>
        public event IndicatorContentChangedDelegate IndicatorContentChangedEvent;

        /// <summary>
        /// Event for change in Time Period Content
        /// </summary>
        public event TimePeriodContentChangedDelegate TimePeriodContentChangedEvent;

        /// <summary>
        /// Event for change in preview limit properties.
        /// </summary>
        public event PreviewChangedDelegate PreviewChangedEvent;


        #endregion

        #region " -- Raise Event -- "

        /// <summary>
        /// Raise the event for change in Data content
        /// </summary>
        public void RaiseDataContentChangedEvent()
        {
            if (this.DataContentChangedEvent != null)
            {
                this.DataContentChangedEvent();
            }
        }

        /// <summary>
        /// Raise the event for change in Data Layout
        /// </summary>
        public void RaiseDataLayoutChangedEvent()
        {
            if (this.DataLayoutChangedEvent != null)
            {
                this.DataLayoutChangedEvent();
            }
        }

        /// <summary>
        /// Raise the event for change in Area content
        /// </summary>
        public void RaiseAreaContentChangedEvent()
        {
            if (this.AreaContentChangedEvent != null)
            {
                this.AreaContentChangedEvent();
            }
        }

        /// <summary>
        /// Raise the event for change in time Period Content
        /// </summary>
        public void RaiseTimePeriodContentChangedEvent()
        {
            if (this.TimePeriodContentChangedEvent != null)
            {
                this.TimePeriodContentChangedEvent();
            }
        }

        /// <summary>
        /// Raise the event for change in Indicator Content
        /// </summary>
        public void RaiseIndicatorContentChangedEvent()
        {
            if (this.IndicatorContentChangedEvent != null)
            {
                this.IndicatorContentChangedEvent();
            }
        }

        /// <summary>
        /// Raise the event for change in preview limit properties.
        /// </summary>
        public void RaisePreviewChangedEvent(Presentation.PresentationType presentationType)
        {
            if (this.PreviewChangedEvent != null)
            {
                this.PreviewChangedEvent(presentationType);
            }
        }

        #endregion

        #region " -- Public -- "

        #region " -- New / Dispose -- "

        public UserPreference(string languageFolder)
        {
            // -- Create the instance of general preference class.
            this._General = new GeneralPreference();

            // -- Create the instance of language preference class.
            this._Language = new LanguagePreference();

            // -- Create the instance of indicator preference class.
            this._Indicator = new IndicatorPreference();

            // -- Create the instance of dataview preference class.
            this._DataView = new DataviewPreference(Path.Combine(languageFolder, this._Language.InterfaceLanguage + ".xml"));

            // -- Create the instance of chart preference class.
            this._Chart = new GraphPreference();

            // -- Create the instance of mapping preference class.
            this._Mapping = new MapPrefernce();

            //-- Vreate the instance of default legend colors and add the colors into it.
            this._Mapping.DefaultLegendColors = new List<string>();

            if (this._Mapping.DefaultLegendColors.Count == 0)
            {
                this._Mapping.DefaultLegendColors.Add("#DEE4F0");
                this._Mapping.DefaultLegendColors.Add("#94A9D3");
                this._Mapping.DefaultLegendColors.Add("#4A6EB6");
                this._Mapping.DefaultLegendColors.Add("#003399");
            }

            // -- Create the instance of sound preference class.
            this._Sound = new SoundPreference();

            // -- Create the instance of database preference class.
            this._Database = new DatabasePreference();

            // -- Create the instance of user selection.
            this._UserSelection = new UserSelection();

            // -- Create the instance of MRU preference class.
            this._MRU = new MRUPreference();

            //-- Event to update the language of fields in dataview.
            this._Language.InterfaceLanguageChanged += new LanguagePreference.InterfaceLanguageChangedDelegate(_Language_UpdateLanguageEvent);

            //-- Set the language folder path.
            LanguageFolderPath = languageFolder;
        }

        /// <summary>
        /// Constructor only for XML serialization.
        /// </summary>
        public UserPreference()
        { }

        #endregion

        #region " -- Enums -- "

        /// <summary>
        /// A bit field or flag enumeration of Classification Types
        /// </summary>
        [Flags]
        public enum ICTypes
        {
            None = 0,
            Sector = 1,
            Goal = 2,
            CF = 4,
            Theme = 8,
            Source = 16,
            Institution = 32,
            Convention = 64,
            All = Sector | Goal | CF | Theme | Source | Institution | Convention,
        }

        /// <summary>
        /// Enum for affected dataview
        /// </summary>
        public enum AffectedDataview
        {
            /// <summary>
            /// 
            /// </summary>
            ContentAffected,

            /// <summary>
            /// 
            /// </summary>
            LayoutAffected,

            /// <summary>
            /// 
            /// </summary>
            None
        }

        /// <summary>
        /// Specifies the alignments.
        /// </summary>
        public enum Alignment
        {
            Left = 1,
            Center = 2,
            Right = 3
        }

        #endregion

        #region " -- Constants -- "

        /// <summary>
        /// Pref.xml located at bin\stock\systemfiles in desktop app.
        /// </summary>
        public const string USER_PREF_FILE_NAME = "Pref.xml";

        #endregion


        #region " -- Properties -- "

        //TODO - Interface Language Changed Event Raise which will be handled by hosting application
        //TODO- Database Language Changed / Current database + dataset changed Event Raise which will be handled by hosting application

        private GeneralPreference _General;
        /// <summary>
        /// Gets or sets the GeneralPreference class.
        /// </summary>
        public GeneralPreference General
        {
            get
            {
                if (this._General == null)
                {
                    this._General = new GeneralPreference();
                }
                return this._General;
            }
            set
            {
                this._General = value;
            }
        }

        private LanguagePreference _Language;
        /// <summary>
        /// Gets or sets the LanguagePreference class.
        /// </summary>
        public LanguagePreference Language
        {
            get
            {
                if (this._Language == null)
                {
                    this._Language = new LanguagePreference();
                }
                return this._Language;
            }
            set
            {
                this._Language = value;
            }
        }

        private IndicatorPreference _Indicator;
        /// <summary>
        /// Gets or sets the indicator preference class
        /// </summary>
        public IndicatorPreference Indicator
        {
            get
            {
                if (this._Indicator == null)
                {
                    this._Indicator = new IndicatorPreference();
                }
                return this._Indicator;
            }
            set
            {
                this._Indicator = value;
            }
        }

        private DataviewPreference _DataView;
        /// <summary>
        /// Gets or sets the dataview preference
        /// </summary>
        public DataviewPreference DataView
        {
            get
            {
                if (this._DataView == null)
                {
                    this._DataView = new DataviewPreference();
                }
                return _DataView;
            }
            set
            {
                _DataView = value;
            }
        }

        private GraphPreference _Chart;
        /// <summary>
        /// Gets or sets the chart preference
        /// </summary>
        public GraphPreference Chart
        {
            get
            {
                if (this._Chart == null)
                {
                    this._Chart = new GraphPreference();
                }
                return this._Chart;
            }
            set
            {
                this._Chart = value;
            }
        }

        private MapPrefernce _Mapping;
        /// <summary>
        /// Gets or sets the mapping preference
        /// </summary>
        public MapPrefernce Mapping
        {
            get
            {
                if (this._Mapping == null)
                {
                    this._Mapping = new MapPrefernce();
                }
                return this._Mapping;
            }
            set
            {
                this._Mapping = value;
            }
        }

        private SoundPreference _Sound;
        /// <summary>
        /// Gets or sets the sound preference
        /// </summary>
        public SoundPreference Sound
        {
            get
            {
                if (this._Sound == null)
                {
                    this._Sound = new SoundPreference();
                }
                return this._Sound;
            }
            set
            {
                this._Sound = value;
            }
        }

        private DatabasePreference _Database;
        /// <summary>
        /// Gets or sets the database preference
        /// </summary>
        public DatabasePreference Database
        {
            get
            {
                if (this._Database == null)
                {
                    this._Database = new DatabasePreference();
                }
                return this._Database;
            }
            set
            {
                this._Database = value;
            }
        }

        private UserSelection _UserSelection;
        /// <summary>
        /// Gets or sets the user selection.
        /// </summary>
        public UserSelection UserSelection
        {
            get
            {
                if (this._UserSelection == null)
                {
                    this._UserSelection = new UserSelection();
                }
                return this._UserSelection;
            }
            set
            {
                this._UserSelection = value;
            }
        }

        private MRUPreference _MRU;
        /// <summary>
        /// Gets or sets the MRU preference.
        /// </summary>
        public MRUPreference MRU
        {
            get
            {
                if (this._MRU == null)
                {
                    this._MRU = new MRUPreference();
                }
                return _MRU;
            }
            set
            {
                _MRU = value;
            }
        }

        private AffectedDataview _UserPreferenceChanged = AffectedDataview.None;
        /// <summary>
        /// Gets or sets the user preference changed.
        /// </summary>
        /// <remarks> ContentAffected / LayoutAffected, if any property related to data has been changed. </remarks>
        public AffectedDataview UserPreferenceChanged
        {
            get
            {
                return this._UserPreferenceChanged;
            }
            set
            {
                this._UserPreferenceChanged = value;
            }
        }


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Save the user preference class in the form of XML.
        /// </summary>
        /// <param name="fileName">file name with path</param>
        public void Save(string fileName)
        {
            try
            {
                XmlSerializer UserPreferenceSerialize = new XmlSerializer(typeof(UserPreference));
                StreamWriter PreferenceWriter = new StreamWriter(fileName);
                UserPreferenceSerialize.Serialize(PreferenceWriter, this);
                PreferenceWriter.Close();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Load the deserialize XML file.
        /// </summary>
        /// <param name="fileName">file name with path</param>
        /// <returns></returns>
        public static UserPreference Load(string fileName, string languageFolder)
        {
            UserPreference Retval;
            StreamReader PreferenceReader = null;
            string ErroneousConventionFiledID = "CLS_CV_";
            try
            {
                //-- Event to update the language of fields in dataview.
                //_Language.InterfaceLanguageChanged += new LanguagePreference.InterfaceLanguageChangedDelegate(_Language_UpdateLanguageEvent);

                //-- Set the language folder path.
                LanguageFolderPath = languageFolder;

                XmlSerializer UserPreference = new XmlSerializer(typeof(UserPreference));
                PreferenceReader = new StreamReader(fileName);
                Retval = (UserPreference)UserPreference.Deserialize(PreferenceReader);

                // Modify pref to handle previous CLS_CV_ bug. Replace CLS_CV_ with CLS_CN_
                #region "Handling for CLS_CV_"
                try
                {
                    if (Retval._DataView.Fields.All.Exists(ErroneousConventionFiledID))
                    {
                        Retval._DataView.Fields.All[ErroneousConventionFiledID].FieldID = DataviewPreference.ICConvention;
                    }
                    if (Retval._DataView.Fields.Columns.Exists(ErroneousConventionFiledID))
                    {
                        Retval._DataView.Fields.Columns[ErroneousConventionFiledID].FieldID = DataviewPreference.ICConvention;
                    }
                    if (Retval._DataView.Fields.Rows.Exists(ErroneousConventionFiledID))
                    {
                        Retval._DataView.Fields.Rows[ErroneousConventionFiledID].FieldID = DataviewPreference.ICConvention;
                    }
                    if (Retval._DataView.Fields.Available.Exists(ErroneousConventionFiledID))
                    {
                        Retval._DataView.Fields.Available[ErroneousConventionFiledID].FieldID = DataviewPreference.ICConvention;
                    }
                    if (Retval._DataView.Fields.Sort.Exists(ErroneousConventionFiledID))
                    {
                        Retval._DataView.Fields.Sort[ErroneousConventionFiledID].FieldID = DataviewPreference.ICConvention;
                    }

                }
                catch (Exception)
                {


                }
                #endregion



                PreferenceReader.Close();
                PreferenceReader.Dispose();
            }
            catch (Exception ex)
            {
                Retval = null;
                if (PreferenceReader != null)
                {
                    PreferenceReader.Dispose();
                }
            }
            return Retval;
        }

        /// <summary>
        /// Check for changes in user pref properties, and fire the event accordingly.
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        public void UserPreferenceAffected(UserPreference tempUserPreference)
        {
            bool Success = false;
            try
            {
                //-- If there is any change regarding to the Data related porperty, its event will get fired.
                Success = this.DataviewAffected(tempUserPreference);
                //-- If there is any change regarding to the Indicator related porperty, its event will get fired.
                Success = this.IndicatorAffected(tempUserPreference);
                //-- If there is any change regarding to the preview limit porperty, its event will get fired.
                Success = this.PreviewAffected(tempUserPreference);
                //-- If there is any change regarding to the Timeperiod related porperty, its event will get fired.
                Success = this.TimePeriodAffected(tempUserPreference);
                //-- If there is any change regarding to the Area related porperty, its event will get fired.
                Success = this.AreaAffected(tempUserPreference);
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #endregion

        #region " -- Inner Classes -- "

        #region " -- General Preference Class -- "

        /// <summary>
        /// Represents the general section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : general </remarks>
        /// </summary>
        public class GeneralPreference
        {
            #region " -- Properties -- "

            private bool _ShowAutoSelectDialog = false;
            /// <summary>
            /// Gets or sets the UseAutoSelect
            /// </summary>
            public bool ShowAutoSelectDialog
            {
                get
                {
                    return this._ShowAutoSelectDialog;
                }
                set
                {
                    this._ShowAutoSelectDialog = value;
                }
            }

            private bool _ShowPresentationSaveDialog = true;
            /// <summary>
            /// Gets or sets to prompt user to save while closing a presentation
            /// </summary>
            public bool ShowPresentationSaveDialog
            {
                get
                {
                    return this._ShowPresentationSaveDialog;
                }
                set
                {
                    this._ShowPresentationSaveDialog = value;
                }
            }

            private bool _SortAreaTreeByAreaId = true;
            /// <summary>
            /// Gets or sets the AreaTreeByAreaId
            /// <remarks>Key : areatreebyareaid </remarks>
            /// </summary>
            public bool SortAreaTreeByAreaId
            {
                get
                {
                    return this._SortAreaTreeByAreaId;
                }
                set
                {
                    this._SortAreaTreeByAreaId = value;
                }
            }

            private bool _ShowGlobalColor = true;
            /// <summary>
            /// gets or sets the ShowGlobalColor
            /// <remarks> Key : show_global_color </remarks>
            /// </summary>
            public bool ShowGlobalColor
            {
                get
                {
                    return this._ShowGlobalColor;
                }
                set
                {
                    this._ShowGlobalColor = value;
                }
            }

            private string _GlobalColor = "#243E94";
            /// <summary>
            /// Gets or sets the GlobalColor
            /// <remarks> Key : global_color (#243E94) </remarks>
            /// </summary>
            public string GlobalColor
            {
                get
                {
                    return this._GlobalColor;
                }
                set
                {
                    this._GlobalColor = value;
                }
            }

            private int _PageSize = 1000;
            /// <summary>
            /// Gets or sets the pagesize
            /// <remarks> Key : page_size </remarks>
            /// </summary>
            public int PageSize
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

            private int _PageNumber = 1;
            /// <summary>
            /// Gets or sets the page number.
            /// </summary>
            public int PageNumber
            {
                get
                {
                    return this._PageNumber;
                }
                set
                {
                    this._PageNumber = value;
                }
            }

            private bool _ClassicView = false;
            /// <summary>
            /// Gets or sets the ClassicView
            /// <remarks> Key : classic_view </remarks>
            /// </summary>
            public bool ClassicView
            {
                get
                {
                    return this._ClassicView;
                }
                set
                {
                    this._ClassicView = value;
                }
            }

            private bool _ShowRecommendedSourceColor = false;
            /// <summary>
            /// Gets or sets the ShowRecommendedSourceColor
            /// <remarks> Key : show_rec_source_color </remarks>
            /// </summary>
            public bool ShowRecommendedSourceColor
            {
                get
                {
                    return this._ShowRecommendedSourceColor;
                }
                set
                {
                    this._ShowRecommendedSourceColor = value;
                }
            }

            private string _RecommendedSourceColor = "#D8E8FF";
            /// <summary>
            /// Gets or sets the RecommendedSourceColor
            /// <remarks> Key : rec_source_color (#D8E8FF) </remarks>
            /// </summary>
            public string RecommendedSourceColor
            {
                get
                {
                    return this._RecommendedSourceColor;
                }
                set
                {
                    this._RecommendedSourceColor = value;
                }
            }

            private bool _ShowComments = false;
            /// <summary>
            /// Gets or sets the ShowComments
            /// <remarks> Key : show_comments </remarks>
            /// </summary>
            public bool ShowComments
            {
                get
                {
                    return this._ShowComments;
                }
                set
                {
                    this._ShowComments = value;
                }
            }

            private bool _IndicatorDockHorizontal = false;
            /// <summary>
            /// Gets or sets the dock horizontal view status at indicator page.
            /// </summary>
            public bool IndicatorDockHorizontal
            {
                get
                {
                    return this._IndicatorDockHorizontal;
                }
                set
                {
                    this._IndicatorDockHorizontal = value;
                }
            }

            private bool _AreaDockHorizontal = false;
            /// <summary>
            /// Gets or sets the dock horizontal view status at area page.
            /// </summary>
            public bool AreaDockHorizontal
            {
                get
                {
                    return this._AreaDockHorizontal;
                }
                set
                {
                    this._AreaDockHorizontal = value;
                }
            }

            private bool _SourceDockHorizontal = false;
            /// <summary>
            /// Gets or sets the dock horizontal view status at source page.
            /// </summary>
            public bool SourceDockHorizontal
            {
                get
                {
                    return this._SourceDockHorizontal;
                }
                set
                {
                    this._SourceDockHorizontal = value;
                }
            }

            private string _LastGalleryLocation = string.Empty;
            /// <summary>
            /// Gets or sets the LastGalleryLocation
            /// <remarks> Key : gallery_last_location </remarks>
            /// </summary>
            public string LastGalleryLocation
            {
                get
                {
                    return this._LastGalleryLocation;
                }
                set
                {
                    this._LastGalleryLocation = value;
                }
            }

            private bool _ShowExcel = true;
            /// <summary>
            /// Gets or set the show excel.
            /// </summary>
            /// <remarks> If true, Presentations, will be shown in excel otherwise in spreadsheet gear. </remarks>
            public bool ShowExcel
            {
                get
                {
                    return this._ShowExcel;
                }
                set
                {
                    this._ShowExcel = value;
                }
            }

            private string _AdaptationPath = string.Empty;
            /// <summary>
            /// Gets or sets the adaptaion path
            /// </summary>
            public string AdaptationPath
            {
                get
                {
                    return this._AdaptationPath;
                }
                set
                {
                    this._AdaptationPath = value;
                }
            }

            private string _AgeGroupGId = "AGE";
            /// <summary>
            /// Get or set the Age groupGId
            /// </summary>
            /// <remarks>Used in Pyramid chart for identifying Age dimension</remarks>
            public string AgeGroupGId
            {
                get
                {
                    return this._AgeGroupGId;
                }
                set
                {
                    this._AgeGroupGId = value;
                }
            }

            private string _SexGroupGId = "SEX";
            /// <summary>
            /// Gets or sets the SexGroupGId
            /// </summary>
            public string SexGroupGId
            {
                get
                {
                    return this._SexGroupGId;
                }
                set
                {
                    this._SexGroupGId = value;
                }
            }

            private string _TotalGId = "TOTAL";
            /// <summary>
            /// Gets or sets the TotalGId
            /// </summary>
            public string TotalGId
            {
                get
                {
                    return this._TotalGId;
                }
                set
                {
                    this._TotalGId = value;
                }
            }


            #endregion
        }

        #endregion

        #region " -- Language Preference Class -- "

        /// <summary>
        /// Represents the language section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : language </remarks>
        /// </summary>
        public class LanguagePreference
        {
            #region " -- Delegate -- "

            /// <summary>
            /// Declare the delegate to update the language of fields in dataview.
            /// </summary>
            /// <param name="languageFile">language file name with path</param>
            public delegate void InterfaceLanguageChangedDelegate(string languageFile);

            #endregion

            #region " -- Event -- "

            /// <summary>
            /// Declare the event to update the language of fields in dataview.
            /// </summary>
            public event InterfaceLanguageChangedDelegate InterfaceLanguageChanged;

            #endregion

            #region " -- Properties -- "

            //TODO how to enforce a string pattern for a string property
            private string _InterfaceLanguage = "English [en]";
            /// <summary>
            /// Gets or sets the LanguageName
            /// <remarks> Key : languagename </remarks>
            /// </summary>
            public string InterfaceLanguage
            {
                get
                {
                    return this._InterfaceLanguage;
                }
                set
                {
                    this._InterfaceLanguage = value;
                    if (this.InterfaceLanguageChanged != null)
                    {
                        this.InterfaceLanguageChanged(this._InterfaceLanguage);
                    }

                    //-- Get the language key on the basis of language file given.
                    if (this._InterfaceLanguage.IndexOf(" [") > -1 && this._InterfaceLanguage.EndsWith("]") == true)
                    {
                        int CodeLength = this._InterfaceLanguage.LastIndexOf("]") - this._InterfaceLanguage.LastIndexOf(" [") - 2;
                        this._InterfaceLanguageCode = this._InterfaceLanguage.Substring(this._InterfaceLanguage.LastIndexOf(" [") + 2, CodeLength);
                    }
                }
            }


            private string _InterfaceLanguageCode = string.Empty;
            /// <summary>
            /// Read only LanguageCode based on current Interface Language
            /// </summary>
            public string InterfaceLanguageCode
            {
                get
                {
                    return _InterfaceLanguageCode;
                }
            }

            private string _FontName = "Arial";
            /// <summary>
            /// Gets or sets the FontName
            /// <remarks> Key : fontname </remarks>
            /// </summary>
            public string FontName
            {
                get
                {
                    return this._FontName;
                }
                set
                {
                    this._FontName = value;
                }
            }

            private FontStyle _FontStyle = FontStyle.Regular;
            /// <summary>
            /// Gets or sets the FontStyle
            /// <remarks> Key :fontstyle </remarks>
            /// </summary>
            public FontStyle FontStyle
            {
                get
                {
                    return this._FontStyle;
                }
                set
                {
                    this._FontStyle = value;
                }
            }

            private int _FontSize = 10;
            /// <summary>
            /// Gets or sets the FontSize
            /// <remarks> Key : fontsize </remarks>
            /// </summary>
            public int FontSize
            {
                get
                {
                    return this._FontSize;
                }
                set
                {
                    this._FontSize = value;
                }
            }

            #endregion

            #region " -- Private -- "

            #region " -- Variable -- "


            #endregion

            #region " -- Raise Event -- "

            /// <summary>
            /// Raise the event to update the language of fields in dataview.
            /// </summary>
            /// <param name="languageFile">language file name with path</param>
            private void RaiseUpdateLanguageEvent(string languageFile)
            {
                if (this.InterfaceLanguageChanged != null)
                {
                    this.InterfaceLanguageChanged(languageFile);
                }
            }

            #endregion

            #endregion
        }


        #endregion

        #region " -- Indicator Preference Class -- "

        /// <summary>
        /// Represents the indicator section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : indicator </remarks>
        /// </summary>
        public class IndicatorPreference
        {
            #region " -- Properties -- "

            private ICTypes _SelectedICType = ICTypes.Sector | ICTypes.Goal | ICTypes.CF | ICTypes.Theme | ICTypes.Source | ICTypes.Institution | ICTypes.Convention;
            /// <summary>
            /// Gets or sets the selected IC types.
            /// </summary>
            [XmlIgnore()]
            public ICTypes SelectedICType
            {
                get
                {
                    return this._SelectedICType;
                }
                set
                {
                    this._SelectedICType = value;
                }
            }

            /// <summary>
            /// Gets or sets the IC selected types.
            /// <remarks> Only for XML serialization. ICTypes is a bitwise enum and we select multiple values in the SelectedICTypes property. So, it can't be serialize. </remarks>
            /// </summary>
            [XmlElement("SelectedICType")]
            public int XMLSelectedICType
            {
                get
                {
                    return Convert.ToInt32(this._SelectedICType);
                }
                set
                {
                    this._SelectedICType = (ICTypes)value;
                }
            }

            private ICType _DefaultICType = ICType.Sector;
            /// <summary>
            /// Gets or sets the indicator selection.
            /// <remarks> Key : default_tree </remarks>
            /// </summary>
            public ICType DefaultICType
            {
                get
                {
                    return this._DefaultICType;
                }
                set
                {
                    this._DefaultICType = value;
                }
            }

            public static ICTypes GetICTypes(bool sector, bool goal, bool cf, bool theme, bool source, bool institution, bool convention)
            {
                ICTypes RetVal = ICTypes.None;
                int i = 0;
                if (sector)
                {
                    i += (int)ICTypes.Sector;
                }

                if (goal)
                {
                    i += (int)ICTypes.Goal;
                }

                if (cf)
                {
                    i += (int)ICTypes.CF;
                }

                if (theme)
                {
                    i += (int)ICTypes.Theme;
                }

                if (source)
                {
                    i += (int)ICTypes.Source;
                }

                if (institution)
                {
                    i += (int)ICTypes.Institution;
                }

                if (convention)
                {
                    i += (int)ICTypes.Convention;
                }

                RetVal = (ICTypes)i;

                return RetVal;
            }

            private bool _ShowIndicatorWithData = false;
            /// <summary>
            /// Get or set bool value to define that only those indicators are to be picked against which data is available
            /// </summary>
            public bool ShowIndicatorWithData
            {
                get { return _ShowIndicatorWithData; }
                set { _ShowIndicatorWithData = value; }
            }

            private bool _ShowIUS = true;
            /// <summary>
            /// Gets or sets the showIUS
            /// </summary>
            public bool ShowIUS
            {
                get
                {
                    return this._ShowIUS;
                }
                set
                {
                    this._ShowIUS = value;
                }
            }

            private bool _SelectSubgroupsbyAllCombinations = true;
            /// <summary>
            /// Gets or sets the Select_Subgroups_by_All_Combinations
            /// </summary>
            /// <remarks>For CensusInfo, it should be checked on and for DI, it should be checked off</remarks>
            public bool SelectSubgroupsbyAllCombinations
            {
                get
                {
                    return this._SelectSubgroupsbyAllCombinations;
                }
                set
                {
                    this._SelectSubgroupsbyAllCombinations = value;
                }
            }

            private bool _SpecialSubgroupHandling = false;
            /// <summary>
            /// Gets or sets the SpecialSubgroupHandling
            /// </summary>
            /// <remarks>
            /// SpecialSubgroupHandling identifies wheteher SelectAllSubgroups property is to be implemented or not
            /// By default it is set to be false, which means all subgroup val are to be shown
            /// </remarks>
            public bool SpecialSubgroupHandling
            {
                get
                {
                    return this._SpecialSubgroupHandling;
                }
                set
                {
                    this._SpecialSubgroupHandling = value;
                }
            }


            private bool _SelectAllSubgroups = false;
            /// <summary>
            /// Gets or sets the Select_All_Subgroups
            /// </summary>
            /// <remarks>
            /// It defines the behaviour of default subgroup selection, while selecting Indicator when ShowIUS = False
            /// For CensusInfo, it should be checked on and for DI, it should be checked off. 
            /// If its on all subgroup are selected, else Single / "TOTAL" / Unidimensional subgroup vals
            /// </remarks>
            public bool SelectAllSubgroups
            {
                get
                {
                    return this._SelectAllSubgroups;
                }
                set
                {
                    this._SelectAllSubgroups = value;
                }
            }



            private bool _ShowWhereDataExists = false;
            /// <summary>
            /// Gets or sets the ShowWhereDataExists
            /// </summary>
            /// <remarks>For CensusInfo, it should be checked on and for DI, it should be checked off</remarks>
            public bool ShowWhereDataExists
            {
                get
                {
                    return this._ShowWhereDataExists;
                }
                set
                {
                    this._ShowWhereDataExists = value;
                }
            }


            private bool _EnableAlphabeticalSort;
            /// <summary>
            /// Get or Set Enable Alphabetical Sort
            /// </summary>
            public bool EnableAlphabeticalSort
            {
                get { return this._EnableAlphabeticalSort; }
                set { this._EnableAlphabeticalSort = value; }
            }


            #endregion
        }

        #endregion

        #region " -- Data view Preference Class -- "

        /// <summary>
        /// Represents the dataview section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : dataview </remarks>
        /// </summary>
        public class DataviewPreference : ICloneable
        {
            #region " -- Private -- "

            #region " -- Constants -- "

            /// <summary>
            /// Constant for Language string of timeperiod used to set caption of field object.
            /// </summary>
            private const string TIMEPERIOD = "TIMEPERIOD";

            /// <summary>
            /// Constant for Language string of areaid used to set caption of field object.
            /// </summary>
            private const string AREAID = "AREAID";

            /// <summary>
            /// Constant for Language string of areaname used to set caption of field object.
            /// </summary>
            private const string AREANAME = "AREANAME";

            /// <summary>
            /// Constant for Language string of indicator used to set caption of field object.
            /// </summary>
            private const string INDICATOR = "INDICATOR";

            /// <summary>
            /// Constant for Language string of datavalue used to set caption of field object.
            /// </summary>
            private const string DATAVALUE = "DATAVALUE";

            /// <summary>
            /// Constant for Language string of unit used to set caption of field object.
            /// </summary>
            private const string UNIT = "UNIT";

            /// <summary>
            /// Constant for Language string of subgroup used to set caption of field object.
            /// </summary>
            private const string SUBGROUP = "SUBGROUP";

            /// <summary>
            /// Constant for Language string of source used to set caption of field object.
            /// </summary>
            private const string SOURCE = "SOURCE";

            /// <summary>
            /// Constant for Language string of metadata used to set caption of field object.
            /// </summary>
            private const string METADATA = "META_DATA";

            /// <summary>
            /// Constant for Language string of area used to set caption of field object.
            /// </summary>
            private const string AREA = "AREA";

            /// <summary>
            /// Constant for Language string of indicator classification used to set caption of field object.
            /// </summary>
            private const string INDICATOR_CLASSIFICATION = "CLASSIFICATION_INDICATOR";

            /// <summary>
            /// Constant for Language string of sector used to set caption of field object.
            /// </summary>
            private const string SECTOR = "SECTOR";

            /// <summary>
            /// Constant for Language string of goal used to set caption of field object.
            /// </summary>
            private const string GOAL = "GOAL";

            /// <summary>
            /// Constant for Language string of institution used to set caption of field object.
            /// </summary>
            private const string INSTITUTION = "INSTITUTION";

            /// <summary>
            /// Constant for Language string of theme used to set caption of field object.
            /// </summary>
            private const string THEME = "THEME";

            /// <summary>
            /// Constant for Language string of convention used to set caption of field object.
            /// </summary>
            private const string CONVENTION = "CONVENTION";

            /// <summary>
            /// Constant for Metadata indicator FieldId. FieldId is generated on the file like MD_IND_1 etc
            /// </summary>
            public const string MetadataIndicator = "MD_IND_";

            /// <summary>
            /// Constant for Metadata Area FieldId. FieldId is generated on the file like MD_AREA_1 etc
            /// </summary>
            public const string MetadataArea = "MD_AREA_";

            /// <summary>
            /// Constant for Metadata source FieldId. FieldId is generated on the file like MD_SRC_1 etc
            /// </summary>
            public const string MetadataSource = "MD_SRC_";

            /// <summary>
            /// Constant for metadata field
            /// </summary>
            public const string Metadata = "MD";

            /// <summary>
            /// constant for IC source fieldid. fieldid is generated on the file like CLS_SR_1 etc
            /// </summary>
            public const string ICSource = "CLS_SR_";

            /// <summary>
            /// constant for IC Goal area fieldid. fieldid is generated on the file like CLS_SC_1 etc
            /// </summary>
            public const string ICGoal = "CLS_GL_";

            /// <summary>
            /// constant for IC sector source fieldid. fieldid is generated on the file like CLS_SC_1 etc
            /// </summary>
            public const string ICSector = "CLS_SC_";

            /// <summary>
            /// constant for IC sector theme fieldid. fieldid is generated on the file like CLS_TH_1 etc
            /// </summary>
            public const string ICTheme = "CLS_TH_";

            /// <summary>
            /// constant for IC sector institution fieldid. fieldid is generated on the file like CLS_IT_1 etc
            /// </summary>
            public const string ICInstitution = "CLS_IT_";

            /// <summary>
            /// constant for IC sector convention fieldid. fieldid is generated on the file like CLS_CN_1 etc
            /// </summary>
            public const string ICConvention = "CLS_CN_";

            /// <summary>
            /// constant for ICs field
            /// </summary>
            public const string IC = "CLS";


            #endregion

            #region " -- Methods -- "

            /// <summary>
            /// Fill the Fields collection named ALL.
            /// <para>Will be used for filling up the Available & selected collection</para>
            /// </summary>
            private void FillAllFields(string LanguageFile)
            {
                try
                {
                    if (System.IO.File.Exists(LanguageFile))
                    {
                        Utility.DILanguage.Open(LanguageFile);
                    }
                }
                catch (Exception ex)
                {
                }
                string MetadataPrefix = Utility.DILanguage.GetLanguageString(METADATA);
                string IndicatorClassificationPrefix = Utility.DILanguage.GetLanguageString(INDICATOR_CLASSIFICATION);

                this._Fields.All = new FieldsCollection();

                this._Fields.All.Add(new Field(Timeperiods.TimePeriod, Utility.DILanguage.GetLanguageString(TIMEPERIOD)));//, FieldType.TimePeriod));
                this._Fields.All.Add(new Field(Area.AreaID, Utility.DILanguage.GetLanguageString(AREAID)));//, FieldType.AreaID));
                this._Fields.All.Add(new Field(Area.AreaName, Utility.DILanguage.GetLanguageString(AREANAME)));//, FieldType.AreaName));
                this._Fields.All.Add(new Field(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName, Utility.DILanguage.GetLanguageString(INDICATOR)));//, FieldType.Indicator));
                this._Fields.All.Add(new Field(Data.DataValue, Utility.DILanguage.GetLanguageString(DATAVALUE)));//, FieldType.DataValue));
                this._Fields.All.Add(new Field(Unit.UnitName, Utility.DILanguage.GetLanguageString(UNIT)));//, FieldType.Unit));
                this._Fields.All.Add(new Field(SubgroupVals.SubgroupVal, Utility.DILanguage.GetLanguageString(SUBGROUP)));//, FieldType.Subgroup));
                this._Fields.All.Add(new Field(IndicatorClassifications.ICName, Utility.DILanguage.GetLanguageString(SOURCE)));//, FieldType.Source));



                this._Fields.All.Add(new Field(DataviewPreference.MetadataIndicator, MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(INDICATOR)));//, FieldType.MetadataIndicator));
                this._Fields.All.Add(new Field(DataviewPreference.MetadataArea, MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(AREA)));//, FieldType.MetadataArea));
                this._Fields.All.Add(new Field(DataviewPreference.MetadataSource, MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(SOURCE)));//, FieldType.MetadataSource));

                this._Fields.All.Add(new Field(DataviewPreference.ICSector, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(SECTOR)));//, FieldType.ICSector));
                this._Fields.All.Add(new Field(DataviewPreference.ICGoal, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(GOAL)));//, FieldType.ICGoal));
                this._Fields.All.Add(new Field(DataviewPreference.ICInstitution, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(INSTITUTION)));//, FieldType.ICInstitute));
                this._Fields.All.Add(new Field(DataviewPreference.ICTheme, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(THEME)));//, FieldType.ICTheame));
                this._Fields.All.Add(new Field(DataviewPreference.ICSource, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(SOURCE)));//, FieldType.ICSource));
                this._Fields.All.Add(new Field(DataviewPreference.ICConvention, IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(CONVENTION)));//, FieldType.ICConvention));
            }

            /// <summary>
            /// Fill the selected list
            /// <remarks> Rows collection is equivalent to the selected collection. </remarks>
            /// </summary>
            private void FillSelectedList()
            {
                this._Fields.Rows.Add(this._Fields.All[Timeperiods.TimePeriod]);
                this._Fields.Rows.Add(this._Fields.All[Area.AreaID]);
                this._Fields.Rows.Add(this._Fields.All[Area.AreaName]);
                this._Fields.Rows.Add(this._Fields.All[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName]);
                this._Fields.Rows.Add(this._Fields.All[Data.DataValue]);
                this._Fields.Rows.Add(this._Fields.All[Unit.UnitName]);
                this._Fields.Rows.Add(this._Fields.All[SubgroupVals.SubgroupVal]);
                this._Fields.Rows.Add(this._Fields.All[IndicatorClassifications.ICName]);
            }

            /// <summary>
            /// Fill the available list
            /// <remarks> Column collection is equivalent to the available collection. </remarks>
            /// </summary>
            private void FillAvailableList()
            {
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.MetadataIndicator]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.MetadataArea]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.MetadataSource]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICSector]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICGoal]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICInstitution]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICTheme]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICSource]);
                this._Fields.Columns.Add(this._Fields.All[DataviewPreference.ICConvention]);
            }

            #endregion

            #endregion

            #region " -- Public -- "

            #region " -- New / Dispose -- "

            public DataviewPreference(string languageFileName)
            {
                this._Fields = new Fields();

                this.FillAllFields(languageFileName);
                this.FillAvailableList();
                this.FillSelectedList();
            }

            /// <summary>
            /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
            /// </summary>
            public DataviewPreference()
            {
            }

            #endregion

            #region " -- Properties -- "

            private string _SortFields = string.Empty;
            /// <summary>
            /// Gets or sets the sort fields.
            /// </summary>
            public string SortFields
            {
                get
                {
                    return this._SortFields;
                }
                set
                {
                    this._SortFields = value;
                }
            }

            private Fields _Fields;
            /// <summary>
            /// Gets or sets the fields
            /// <remarks> Key : dataview_cols </remarks>
            /// </summary>
            public Fields Fields
            {
                get
                {
                    return this._Fields;
                }
                set
                {
                    this._Fields = value;
                }
            }

            private bool _ShowDataviewAlternateColor = true;
            /// <summary>
            /// Gets or sets the dataviewalternateColor
            /// <remarks> Key : dataview_showaltclr </remarks>
            /// </summary>
            public bool ShowDataviewAlternateColor
            {
                get
                {
                    return this._ShowDataviewAlternateColor;
                }
                set
                {
                    this._ShowDataviewAlternateColor = value;
                }
            }

            private string _DataviewAlternateColor = "#ECECFF";
            /// <summary>
            /// gets or sets the dataviewAlternateColor
            /// <remarks> Key : dataview_altclr </remarks>
            /// </summary>
            public string DataviewAlternateColor
            {
                get
                {
                    return this._DataviewAlternateColor;
                }
                set
                {
                    this._DataviewAlternateColor = value;
                }
            }

            #region "-- Indicator Classification --"

            // -- Comma delimited IC GIds stores GIDS of selected values under the given head.

            private string _ICSectorGIds = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated IC sector GIds
            /// <remarks> Key : cls_sr_gids </remarks>
            /// </summary>
            public string ICSectorGIds
            {
                get
                {
                    return this._ICSectorGIds;
                }
                set
                {
                    this._ICSectorGIds = value;
                }
            }

            private string _ICGoalGIds = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated IC goal GIds
            /// <remarks> Key : cls_gl_gids </remarks>
            /// </summary>
            public string ICGoalGIds
            {
                get
                {
                    return this._ICGoalGIds;
                }
                set
                {
                    this._ICGoalGIds = value;
                }
            }

            private string _ICThemeGIds = string.Empty;
            /// <summary>
            /// Get or sets the comma seprated IC theme GIds
            /// <remarks> Key : cls_th_gids </remarks>
            /// </summary>
            public string ICThemeGIds
            {
                get
                {
                    return this._ICThemeGIds;
                }
                set
                {
                    this._ICThemeGIds = value;
                }
            }

            private string _ICSourceGIds = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated IC Source GIds
            /// <remarks> Key : cls_sc_gids </remarks>
            /// </summary>
            public string ICSourceGIds
            {
                get
                {
                    return this._ICSourceGIds;
                }
                set
                {
                    this._ICSourceGIds = value;
                }
            }

            private string _ICInstitutionalGIds = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated IC institutional GIds
            /// <remarks> Key : cls_it_gids </remarks>
            /// </summary>
            public string ICInstitutionalGIds
            {
                get
                {
                    return this._ICInstitutionalGIds;
                }
                set
                {
                    this._ICInstitutionalGIds = value;
                }
            }

            private string _ICConventionGIds = string.Empty;
            /// <summary>
            /// Get or sets the comma seprated IC convention GIds
            /// <remarks> Key : cls_cv_gids </remarks>
            /// </summary>
            public string ICConventionGIds
            {
                get
                {
                    return this._ICConventionGIds;
                }
                set
                {
                    this._ICConventionGIds = value;
                }
            }

            private string _ICFields = string.Empty;
            /// <summary>
            /// Gets or sets the selected IC fields
            /// </summary>
            public string ICFields
            {
                get
                {
                    return this._ICFields;
                }
                set
                {
                    this._ICFields = value;
                }
            }

            #endregion

            #region " -- Metadata -- "

            // -- Metadata fields stores the fields Id of selected values.

            private string _MetadataIndicatorField = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated metadata indicator fields.
            /// </summary>
            public string MetadataIndicatorField
            {
                get
                {
                    return _MetadataIndicatorField;
                }
                set
                {
                    _MetadataIndicatorField = value;
                }
            }

            private string _MetadataAreaField = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated metadata area fields.
            /// </summary>
            public string MetadataAreaField
            {
                get
                {
                    return this._MetadataAreaField;
                }
                set
                {
                    this._MetadataAreaField = value;
                }
            }

            private string _MetadataSourceField = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated metadata source fields.
            /// </summary>
            public string MetadataSourceField
            {
                get
                {
                    return this._MetadataSourceField;
                }
                set
                {
                    this._MetadataSourceField = value;
                }
            }

            #endregion

            private Alignment _DataValueAlignment = Alignment.Right;
            /// <summary>
            /// Gets or sets the data value alignment.
            /// <remarks> Key : datavalue_alignment </remarks>
            /// </summary>
            public Alignment DataValueAlignment
            {
                get
                {
                    return this._DataValueAlignment;
                }
                set
                {
                    this._DataValueAlignment = value;
                }
            }

            private bool _AllowRowFiltering = false;
            /// <summary>
            /// Gets or sets the boolean value to display autofilter option in data grid
            /// <remarks></remarks>
            /// </summary>
            public bool AllowRowFiltering
            {
                get { return this._AllowRowFiltering; }
                set { this._AllowRowFiltering = value; }
            }


            private int _DecimalPrecision = 2;
            /// <summary>
            /// Decimal precision for displaying values in statstical table
            /// </summary>
            public int DecimalPrecision
            {
                get { return _DecimalPrecision; }
                set { _DecimalPrecision = value; }
            }


            private bool _ShowPreview = true;
            /// <summary>
            /// Gets or sets the boolean value to turn preview option on or off
            /// <remarks></remarks>
            /// </summary>
            public bool ShowPreview
            {
                get
                {
                    return this._ShowPreview;
                }
                set
                {
                    this._ShowPreview = value;
                }
            }

            private int _PreviewHeight = 350;
            /// <summary>
            /// Decimal precision for displaying values in statstical table
            /// </summary>
            public int PreviewHeight
            {
                get { return _PreviewHeight; }
                set { _PreviewHeight = value; }
            }

            private int _TablePresentationLimit = 20000;
            /// <summary>
            /// Maximum records in datview that will be considered while generating table presentation or else "too many records" message shall be prompted
            /// </summary>
            public int TablePresentationLimit
            {
                get { return _TablePresentationLimit; }
                set { _TablePresentationLimit = value; }
            }

            private int _TablePreviewLimit = 1000;
            /// <summary>
            /// Maximum records in datview that will be considered while generating table preview or else dummy image shall be displayed
            /// </summary>
            public int TablePreviewLimit
            {
                get { return _TablePreviewLimit; }
                set { _TablePreviewLimit = value; }
            }

            private int _DIWizardTablePreviewLimit = 20000;
            /// <summary>
            /// Maximum records in diwizard that will be considered while generating table preview or else dummy image shall be displayed
            /// </summary>
            public int DIWizardTablePreviewLimit
            {
                get { return _DIWizardTablePreviewLimit; }
                set { _DIWizardTablePreviewLimit = value; }
            }

            private int _GraphPresentationLimit = 20000;
            /// <summary>
            /// Maximum records in datview that will be considered while generating graph presentation or else "too many records" message shall be prompted
            /// </summary>
            public int GraphPresentationLimit
            {
                get { return _GraphPresentationLimit; }
                set { _GraphPresentationLimit = value; }
            }

            private int _GraphPreviewLimit = 200;
            /// <summary>
            /// Maximum records in datview that will be considered while generating graph preview or else dummy image shall be displayed
            /// </summary>
            public int GraphPreviewLimit
            {
                get { return _GraphPreviewLimit; }
                set { _GraphPreviewLimit = value; }
            }

            private int _DIWizardGraphPreviewLimit = 200;
            /// <summary>
            /// Maximum records in diwizard that will be considered while generating graph preview or else dummy image shall be displayed
            /// </summary>
            public int DIWizardGraphPreviewLimit
            {
                get { return _DIWizardGraphPreviewLimit; }
                set { _DIWizardGraphPreviewLimit = value; }
            }


            #endregion

            #region " -- Methods -- "

            /// <summary>
            /// Fill the Fields collection named ALL.
            /// <para>Will be used for filling up the Available & selected collection</para>
            /// </summary>
            internal void UpdateAllFields(string LanguageFile)
            {
                try
                {
                    if (System.IO.File.Exists(LanguageFile))
                    {
                        Utility.DILanguage.Open(LanguageFile);
                    }
                }
                catch (Exception ex)
                {
                }

                string MetadataPrefix = Utility.DILanguage.GetLanguageString(METADATA);
                string IndicatorClassificationPrefix = Utility.DILanguage.GetLanguageString(INDICATOR_CLASSIFICATION);

                foreach (Field field in this._Fields.All)
                {
                    switch (field.FieldID)
                    {
                        case Timeperiods.TimePeriod:
                            field.Caption = Utility.DILanguage.GetLanguageString(TIMEPERIOD);
                            break;

                        case Area.AreaID:
                            field.Caption = Utility.DILanguage.GetLanguageString(AREAID);
                            break;

                        case Area.AreaName:
                            field.Caption = Utility.DILanguage.GetLanguageString(AREANAME);
                            break;

                        case DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName:
                            field.Caption = Utility.DILanguage.GetLanguageString(INDICATOR);
                            break;

                        case Data.DataValue:
                            field.Caption = Utility.DILanguage.GetLanguageString(DATAVALUE);
                            break;

                        case Unit.UnitName:
                            field.Caption = Utility.DILanguage.GetLanguageString(UNIT);
                            break;

                        case SubgroupVals.SubgroupVal:
                            field.Caption = Utility.DILanguage.GetLanguageString(SUBGROUP);
                            break;

                        case IndicatorClassifications.ICName:
                            field.Caption = Utility.DILanguage.GetLanguageString(SOURCE);
                            break;

                        case DataviewPreference.MetadataIndicator:
                            field.Caption = MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(INDICATOR);
                            break;

                        case DataviewPreference.MetadataArea:
                            field.Caption = MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(AREA);
                            break;

                        case DataviewPreference.MetadataSource:
                            field.Caption = MetadataPrefix + " - " + Utility.DILanguage.GetLanguageString(SOURCE);
                            break;

                        case DataviewPreference.ICSector:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(SECTOR);
                            break;

                        case DataviewPreference.ICGoal:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(GOAL);
                            break;

                        case DataviewPreference.ICInstitution:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(INSTITUTION);
                            break;

                        case DataviewPreference.ICTheme:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(THEME);
                            break;

                        case DataviewPreference.ICSource:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(SOURCE);
                            break;

                        case DataviewPreference.ICConvention:
                            field.Caption = IndicatorClassificationPrefix + " - " + Utility.DILanguage.GetLanguageString(CONVENTION);
                            break;

                        default:
                            break;
                    }
                }
            }

            /// <summary>
            /// Update the sort order of the available list.
            /// </summary>
            public void UpdateAvailableList()
            {
                try
                {
                    this._Fields.Columns.Clear();
                    foreach (Field Field in this._Fields.All)
                    {
                        if (this._Fields.Rows[Field.FieldID] == null)
                        {
                            this._Fields.Columns.Add(this._Fields.All[Field.FieldID]);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            #endregion

            #endregion

            #region ICloneable Members

            public object Clone()
            {
                object RetVal = null;
                try
                {
                    XmlSerializer XmlSerializer = new XmlSerializer(typeof(DataviewPreference));
                    MemoryStream MemoryStream = new MemoryStream();
                    XmlSerializer.Serialize(MemoryStream, this);
                    MemoryStream.Position = 0;
                    RetVal = (DataviewPreference)XmlSerializer.Deserialize(MemoryStream);
                    MemoryStream.Close();
                    MemoryStream.Dispose();
                }
                catch (Exception ex)
                {
                    RetVal = null;
                }
                return (DataviewPreference)RetVal;
            }


            #endregion
        }

        #endregion

        #region " -- Graph Preference Class -- "

        /// <summary>
        /// Represents the chart section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : chart </remarks>
        /// </summary>
        public class GraphPreference
        {
            #region " -- Properties -- "

            private int _ChartType = 51;
            /// <summary>
            /// Gets or sets the chart type.
            /// <remarks> Key : charttype </remarks>
            /// </summary>
            public int ChartType
            {
                get
                {
                    return this._ChartType;
                }
                set
                {
                    this._ChartType = value;
                }
            }

            #endregion
        }

        #endregion

        #region " -- Mapping Prefernece Class -- "

        /// <summary>
        /// Represents the mapping, map_symbol_recent section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : mapping, map_symbol_recent </remarks>
        /// </summary>
        public class MapPrefernce
        {
            #region " -- New / Dispose -- "

            public MapPrefernce()
            {
                ////-- Add the default mapping color.
                if (this._DefaultLegendColors == null)
                {
                    this._DefaultLegendColors = new List<string>();
                }

                //-- Create the instance of FontMapCollection
                this._RecentMapFonts = new MapFonts();

                //-- Create the instance of PictureMapCollection
                this._RecentMapPictures = new MapPictures();
            }

            #endregion

            #region " -- Properties -- "

            private List<string> _DefaultLegendColors;
            /// <summary>
            /// Get or sets the mapping color list of 4 items.
            /// <remarks> Key : map_clr1, map_clr2, map_clr3, map_clr4 </remarks>
            /// </summary>
            public List<string> DefaultLegendColors
            {
                get
                {
                    return this._DefaultLegendColors;
                }
                set
                {
                    this._DefaultLegendColors = value;
                }
            }

            private MapFonts _RecentMapFonts;
            /// <summary>
            /// Get or sets the recent map font 
            /// <remarks> Key : map_font_recent </remarks>
            /// </summary>
            public MapFonts RecentMapFonts
            {
                get
                {
                    return this._RecentMapFonts;
                }
                set
                {
                    this._RecentMapFonts = value;
                }
            }

            private MapPictures _RecentMapPictures;
            /// <summary>
            /// Gets or sets the recent map picture
            /// <remarks> Key : map_pic_recent </remarks>
            /// </summary>
            public MapPictures RecentMapPictures
            {
                get
                {
                    return this._RecentMapPictures;
                }
                set
                {
                    this._RecentMapPictures = value;
                }
            }

            private string _WorldWindPath = @"C:\Devinfo\World Wind 1.3";
            /// <summary>
            /// Gets or sets the WorldWindPath
            /// <remarks> Key : world_wind_path </remarks>
            /// </summary>
            public string WorldWindPath
            {
                get
                {
                    return this._WorldWindPath;
                }
                set
                {
                    this._WorldWindPath = value;
                }
            }

            private string _GoogleEarthPath = string.Empty;
            /// <summary>
            /// Gets or sets the GoogleEarth Path
            /// </summary>
            public string GoogleEarthPath
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

            #region " -- Inner Classes -- "

            /// <summary>
            /// Stores the font information
            /// </summary>
            public class MapFont
            {
                #region " -- New / Dispose -- "

                public MapFont(string fontName, int characterIndex, int fontColor, int fontSize)
                {
                    this._FontName = fontName;
                    this._CharacterIndex = characterIndex;
                    this._FontColor = fontColor;
                    this._FontSize = fontSize;
                }

                public MapFont()
                { }

                #endregion

                #region " -- Properties -- "

                private string _FontName = string.Empty;
                /// <summary>
                /// Gets or sets the font name
                /// </summary>
                public string FontName
                {
                    get
                    {
                        return this._FontName;
                    }
                    set
                    {
                        this._FontName = value;
                    }
                }

                private int _CharacterIndex;
                /// <summary>
                /// Get or sets the character index
                /// </summary>
                public int CharacterIndex
                {
                    get
                    {
                        return _CharacterIndex;
                    }
                    set
                    {
                        _CharacterIndex = value;
                    }
                }

                private int _FontColor = 0;
                /// <summary>
                /// Get or sets the font color name value.
                /// </summary>
                /// <remarks >
                /// Html value does not consider alpha value, therefore font name value is preserved. Use Color.FromName to retrieve color
                /// </remarks>
                public int FontColor
                {
                    get
                    {
                        return this._FontColor;
                    }
                    set
                    {
                        this._FontColor = value;
                    }
                }

                private int _FontSize;
                /// <summary>
                /// Get or sets the font size
                /// </summary>
                public int FontSize
                {
                    get
                    {
                        return this._FontSize;
                    }
                    set
                    {
                        this._FontSize = value;
                    }
                }

                #endregion

            }

            /// <summary>
            /// Stores the map font information into the collection.
            /// </summary>
            public class MapFonts : System.Collections.CollectionBase
            {
                #region " -- Public -- "

                #region " -- Methods -- "

                /// <summary>
                /// Add the details into the collection 
                /// </summary>
                /// <param name="map"></param>
                public void Add(MapFont map)
                {
                    bool NewField = true;
                    foreach (MapFont MapField in this.InnerList)
                    {
                        if (MapField.FontName == map.FontName && MapField.CharacterIndex == map.CharacterIndex && MapField.FontColor == map.FontColor && MapField.FontSize == map.FontSize)
                        {
                            NewField = false;
                            //-- If the item already exists, then it send that item onto the top
                            this.MoveToTop(MapField);
                            break;
                        }
                    }
                    // -- Font list add only 15 items. 1st item will remove if user want to add more items into the collection. It works on FIFO basis.
                    if (this.InnerList.Count > 15 && NewField)
                    {
                        this.InnerList.RemoveAt(0);
                        this.InnerList.Add(map);
                    }
                    else if (NewField)
                    {
                        this.InnerList.Add(map);
                    }
                }

                /// <summary>
                /// Get the item from the collection.
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public MapFont this[int index]
                {
                    get
                    {
                        MapFont Retval;
                        try
                        {
                            Retval = (MapFont)this.InnerList[index];
                        }
                        catch (Exception ex)
                        {
                            Retval = null;
                        }
                        return Retval;
                    }
                }

                /// <summary>
                /// Remove the item from the collection on the basis of index.
                /// </summary>
                /// <param name="index"></param>
                public void Remove(int index)
                {
                    this.InnerList.RemoveAt(index);
                }

                /// <summary>
                /// Remove the item from the collection on the basis of instance of the field.
                /// </summary>
                /// <param name="mapField"></param>
                public void Remove(MapFont mapField)
                {
                    this.InnerList.Remove(mapField);
                }

                /// <summary>
                /// Convert list into array.
                /// </summary>
                /// <returns></returns>
                public string[] ToArray()
                {
                    string[] Retval = new string[this.InnerList.Count];
                    try
                    {
                        for (int i = 0; i < this.InnerList.Count; i++)
                        {
                            Retval[i] = this.InnerList[i].ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return Retval;
                }

                #endregion

                #endregion

                #region " -- Private -- "

                #region " -- Methods --"

                /// <summary>
                /// Move the field on the top of the collection.
                /// </summary>
                /// <param name="mapField"></param>
                private void MoveToTop(MapFont mapField)
                {
                    this.InnerList.Remove(mapField);
                    this.InnerList.Insert(0, mapField);
                }

                #endregion

                #endregion
            }

            /// <summary>
            /// Stores the image information
            /// </summary>
            public class MapPicture
            {
                #region " -- New / Dispose -- "

                /// <summary>
                /// Constructor to add field in the collection.
                /// </summary>
                /// <param name="picture"></param>
                public MapPicture(string picture)
                {
                    this._RecentPicture = picture;
                }

                /// <summary>
                /// Only for XML serialization.
                /// </summary>
                public MapPicture()
                {
                }

                #endregion

                #region " -- Properties -- "

                private string _RecentPicture = string.Empty;
                /// <summary>
                /// Gets or sets the recent map picture                
                /// </summary>
                public string RecentPicture
                {
                    get
                    {
                        return this._RecentPicture;
                    }
                    set
                    {
                        this._RecentPicture = value;
                    }
                }

                #endregion
            }

            /// <summary>
            /// Stores the recent map picture information into the collection.
            /// </summary>
            public class MapPictures : System.Collections.CollectionBase
            {
                #region " -- Public -- "

                #region " -- Methods -- "

                /// <summary>
                /// Add the details into the collection 
                /// </summary>
                /// <param name="map"></param>
                public void Add(MapPicture picture)
                {
                    bool NewField = true;
                    foreach (MapPicture PicField in this.InnerList)
                    {
                        if (PicField.RecentPicture == picture.RecentPicture)
                        {
                            NewField = false;
                            //-- If the item already exists, then it send that item onto the top
                            this.MoveToTop(picture);
                            break;
                        }
                    }
                    // -- Font list add only 15 items. 1st item will remove if user want to add more items into the collection. It works on FIFO basis.
                    if (this.InnerList.Count > 15 && NewField)
                    {
                        this.InnerList.RemoveAt(0);
                        this.InnerList.Add(picture);
                    }
                    else if (NewField)
                    {
                        this.InnerList.Add(picture);
                    }
                }

                /// <summary>
                /// Get the item from the collection.
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public MapPicture this[int index]
                {
                    get
                    {
                        MapPicture Retval;
                        try
                        {
                            Retval = (MapPicture)this.InnerList[index];
                        }
                        catch (Exception ex)
                        {
                            Retval = null;
                        }
                        return Retval;
                    }
                }

                /// <summary>
                /// Remove the item from the collection on the basis of index.
                /// </summary>
                /// <param name="index"></param>
                public void Remove(int index)
                {
                    this.InnerList.RemoveAt(index);
                }

                /// <summary>
                /// Remove the item from the collection on the basis of instance of the field.
                /// </summary>
                /// <param name="mapField"></param>
                public void Remove(MapPicture picField)
                {
                    this.InnerList.Remove(picField);
                }

                /// <summary>
                /// Convert list into array.
                /// </summary>
                /// <returns></returns>
                public string[] ToArray()
                {
                    string[] Retval = new string[this.InnerList.Count];
                    try
                    {
                        for (int i = 0; i < this.InnerList.Count; i++)
                        {
                            Retval[i] = this.InnerList[i].ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    return Retval;
                }

                #endregion

                #endregion

                #region " -- Private -- "

                #region " -- Methods --"

                /// <summary>
                /// Move the field on the top of the collection.
                /// </summary>
                /// <param name="mapField"></param>
                private void MoveToTop(MapPicture mapField)
                {
                    this.InnerList.Remove(mapField);
                    this.InnerList.Insert(0, mapField);
                }

                #endregion

                #endregion
            }
            #endregion

        }

        #endregion

        #region " -- Sound Preference Class -- "

        /// <summary>
        /// Represents the sound section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : mapping </remarks>
        /// </summary>
        public class SoundPreference
        {
            #region " -- Properties -- "

            private string _SoundFile = "Default.wav";
            /// <summary>
            /// Get or sets the sound file
            /// <remarks> Key : defaultsoundfile </remarks>
            /// </summary>
            public string SoundFile
            {
                get
                {
                    return this._SoundFile;
                }
                set
                {
                    this._SoundFile = value;
                }
            }

            private bool _Mute = true;
            /// <summary>
            /// Get or sets the sound mute.
            /// <remarks> Key : soundmute </remarks>
            /// </summary>
            public bool Mute
            {
                get
                {
                    return this._Mute;
                }
                set
                {
                    this._Mute = value;
                }
            }

            #endregion

        }

        #endregion

        #region " -- Database Preference Class -- "

        /// <summary>
        /// Represents the onlinedb, onlinedataset section of User Preferences which were previously stored in pref.xml        
        /// <remarks> Key : onlinedataset, onlinedb </remarks>
        /// </summary>
        public class DatabasePreference
        {
            #region " -- Constants -- "

            private const string USERNAME = "admin";
            private const string PASSWORD = "unitednations2000";
            public const string DISPLAY_MEMBER = "DisplayMember";
            public const string VALUE_MEMBER = "ValueMember";
            public const string DATA = "Data";
            public const string DELIMITER = "{[]}";
            public const string SPACER = "---";

            #endregion

            #region " -- New / Dispose --"

            public DatabasePreference()
            {
                this._OnlineDatabaseDetails = new OnlineConnectionDetails();
            }

            #endregion

            #region " -- Properties -- "

            private DIConnectionDetails _SelectedConnectionDetail;
            /// <summary>
            /// Get or sets the selected database name and its connection details
            /// </summary>
            public DIConnectionDetails SelectedConnectionDetail
            {
                get
                {
                    return this._SelectedConnectionDetail;
                }
                set
                {
                    this._SelectedConnectionDetail = value;
                }
            }

            private string _SelectedDatasetName = string.Empty;
            /// <summary>
            /// Get or sets the selected dataset .
            /// </summary>
            public string SelectedDatasetName
            {
                get
                {
                    return this._SelectedDatasetName;
                }
                set
                {
                    this._SelectedDatasetName = value;
                }
            }

            private string _SelectedDatasetPrefix = string.Empty;
            /// <summary>
            /// Get or sets the selected dataset prefix.
            /// </summary>
            public string SelectedDatasetPrefix
            {
                get
                {
                    return this._SelectedDatasetPrefix;
                }
                set
                {
                    this._SelectedDatasetPrefix = value;
                }
            }

            private string _SelectedConnectionName = string.Empty;
            /// <summary>
            /// Gets or sets the selected connection name.
            /// </summary>
            public string SelectedConnectionName
            {
                get
                {
                    return this._SelectedConnectionName;
                }
                set
                {
                    this._SelectedConnectionName = value;
                }
            }

            private string _SelectedDbInfo = string.Empty;
            /// <summary>
            /// Gets or sets the selected db information.
            /// </summary>
            /// <remarks>In access database, Dbinfo contains ServerType{[]}Dbname with path.
            /// In other db, db contains, ServerType{[]}ConnectionIndex{[]}DatasetPrefix </remarks>
            public string SelectedDbInfo
            {
                get
                {
                    return this._SelectedDbInfo;
                }
                set
                {
                    this._SelectedDbInfo = value;
                }
            }

            //TODO Check the implication of preserving language suffix only
            private string _DatabaseLanguage = "_en";
            /// <summary>
            /// Gets or sets the DbLanguageName
            /// <remarks> Key : dblanguagename . Interface language is stored seprately </remarks>
            /// </summary>
            public string DatabaseLanguage
            {
                get
                {
                    return this._DatabaseLanguage;
                }
                set
                {
                    this._DatabaseLanguage = value;
                }
            }

            private OnlineConnectionDetails _OnlineDatabaseDetails;
            /// <summary>
            /// Get or sets the online as well as offline database connection details
            /// </summary>
            public OnlineConnectionDetails OnlineDatabaseDetails
            {
                get
                {
                    return this._OnlineDatabaseDetails;
                }
                set
                {
                    this._OnlineDatabaseDetails = value;

                }
            }

            private string _DefaultDbInfo = string.Empty;
            /// <summary>
            /// Gets or sets the default database name.
            /// </summary>
            /// <remarks>
            /// This will always be mdb file name (without path) set through DA - Customization
            /// If no database is found under SelectedConnectionDetail then DI will try to look default database into data folder and set it as selected database
            /// </remarks>
            public string DefaultDbInfo
            {
                get
                {
                    return this._DefaultDbInfo;
                }
                set
                {
                    this._DefaultDbInfo = value;
                }
            }

            private string _SqlExpressInstanceName = @"\sqlexpress";
            /// <summary>
            /// Gets or sets the sql server express instance name
            /// </summary>
            /// <remarks>Default named piped instance is \sqlexpress </remarks>
            public string SqlExpressInstanceName
            {
                get
                {
                    return this._SqlExpressInstanceName;
                }
                set
                {
                    this._SqlExpressInstanceName = value;
                }
            }


            #endregion

            #region " -- Methods -- "

            #region " -- Public -- "

            /// <summary>
            /// Check for the duplicacy of the connection on the basis of connection name and set the selected connection.
            /// </summary>
            /// <param name="connectionName"></param>
            /// <returns>True, if connection already exists and false for new connection.</returns>
            public bool IsDuplicate(string connectionName)
            {
                bool RetVal = false;
                try
                {
                    foreach (OnlineConnectionDetail ConnectionDetail in OnlineDatabaseDetails)
                    {
                        if (string.Compare(ConnectionDetail.Connection, connectionName, true) == 0)
                        {
                            RetVal = true;
                            this.SelectedConnectionDetail = ConnectionDetail.DIConnectionDetails;
                            this.SelectedDatasetName = string.Empty;
                            this.SelectedDatasetPrefix = string.Empty;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    RetVal = false;
                }
                return RetVal;
            }

            /// <summary>
            /// Data table contains the information of database and dataset.
            /// </summary>
            /// <param name="dataFolderPath"></param>
            /// <param name="clubOfflineDatabase">True, to club the access database. False, to show the dataset of access database.</param>
            /// <param name="dataLanguageString">Language string of "DATA". Used only in Dekstop.</param>
            /// <remarks> Online connection Db info contains : ServerType{[]}ConnectionIndex{[]}DatasetPrefix 
            /// Offline connection ServerType{[]}AcessdbPath (Only for Dekstop Application) </remarks>
            /// <returns>Data table contains the database and dataset information.</returns>
            public DataTable GetDatabaseList(string dataFolderPath, bool clubOfflineDatabase, string dataLanguageString)
            {
                DataTable Retval = new DataTable();
                try
                {
                    SortedList<string, int> SortedConnections = new SortedList<string, int>();
                    Retval.Columns.Add(DISPLAY_MEMBER, typeof(System.String));
                    Retval.Columns.Add(VALUE_MEMBER, typeof(System.String));
                    DataRow Row;

                    if (!clubOfflineDatabase)
                    {
                        //-- Add the access database in the collection.
                        this.OfflineDbCollection(dataFolderPath);
                        //-- Remove the deleted access database from the collection.
                        this.RemoveOfflineConnection();
                    }
                    else
                    {
                        //-- Add the lanuage specific string of "DATA"
                        Row = Retval.NewRow();
                        Row[DISPLAY_MEMBER] = dataLanguageString;
                        Row[VALUE_MEMBER] = string.Empty;
                        Retval.Rows.Add(Row);

                        //-- Get all the access db file from data folder.
                        string[] DbFiles = new string[0];
                        DbFiles = Directory.GetFiles(dataFolderPath, "*.mdb");
                        //-- Add access database for Dekstop
                        foreach (string AccessDb in DbFiles)
                        {
                            int ServerType = Convert.ToInt32(DIServerType.MsAccess);
                            Row = Retval.NewRow();
                            Row[DISPLAY_MEMBER] = SPACER + Path.GetFileName(AccessDb);
                            Row[VALUE_MEMBER] = ServerType.ToString() + DELIMITER + AccessDb;
                            Retval.Rows.Add(Row);
                        }
                        DbFiles = Directory.GetFiles(dataFolderPath, "*.mdf");
                        //-- Add Sql server express mdf database files for Dekstop
                        foreach (string ExpressDb in DbFiles)
                        {
                            int ServerType = Convert.ToInt32(DIServerType.SqlServerExpress);
                            Row = Retval.NewRow();
                            Row[DISPLAY_MEMBER] = SPACER + Path.GetFileName(ExpressDb);
                            Row[VALUE_MEMBER] = ServerType.ToString() + DELIMITER + ExpressDb;
                            Retval.Rows.Add(Row);
                        }
                    }

                    //-- Get the sorted connection list.
                    SortedConnections = this.GetSortedList();

                    foreach (KeyValuePair<string, int> Connection in SortedConnections)
                    {
                        //-- Add the connection name.
                        Row = Retval.NewRow();
                        Row[DISPLAY_MEMBER] = Connection.Key;
                        Row[VALUE_MEMBER] = string.Empty;
                        Retval.Rows.Add(Row);
                        //-- Add the dataset details for the connection.
                        foreach (OnlineConnectionDetail.OnlineDataSet DbDataSet in this._OnlineDatabaseDetails[Connection.Value].OnlineDatasetInfo)
                        {
                            int ServerType = Convert.ToInt32(this._OnlineDatabaseDetails[Connection.Value].DIConnectionDetails.ServerType);
                            Row = Retval.NewRow();
                            Row[DISPLAY_MEMBER] = SPACER + DbDataSet.DatasetName;
                            Row[VALUE_MEMBER] = ServerType.ToString() + DELIMITER + Connection.Value.ToString() + DELIMITER + DbDataSet.Prefix;
                            Retval.Rows.Add(Row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                return Retval;
            }

            /// <summary>
            /// Return the DIConnection on the database info.
            /// </summary>
            /// <param name="valueMemeber">ServerType{[]}ConnectionIndex{[]}DatasetPrefix</param>
            /// <returns>Database connection.</returns>
            public DIConnection GetDatabaseConnection(string valueMemeber)
            {
                DIConnection Retval = null;
                try
                {
                    string[] Values = new string[0];
                    Values = Utility.DICommon.SplitString(valueMemeber, DELIMITER);
                    if (Values.Length == 3)
                    {
                        //-- In case of Web, it return the connection for all the servert type.
                        //-- In case of Dekstop, it return the connection for all the servert type except access database.
                        Retval = new DIConnection(this._OnlineDatabaseDetails[Convert.ToInt32(Values[1])].DIConnectionDetails);
                    }
                    else if (Values.Length == 2)
                    {
                        //-- In case of Dekstop, it return the connection for access database only.
                        Retval = new DIConnection(DIServerType.MsAccess, "", "", Values[1], USERNAME, PASSWORD);
                    }
                }
                catch (Exception ex)
                {
                    Retval = null;
                }
                return Retval;
            }

            /// <summary>
            /// Set the default database information on the basis of connection name.
            /// </summary>
            /// <param name="connectionName">Connection Name</param>
            /// <remarks> Default database information contains ServerType{[]}ConnectionIndex{[]}DatasetPrefix </remarks>
            public void SetDefaultConnection(string connectionName)
            {
                //-- Get the Db info on the basis of connection name.
                this._DefaultDbInfo = this.DatabaseInfo(connectionName);
            }

            /// <summary>
            /// check for the default connection on the basis of connection name.
            /// </summary>
            /// <param name="connectionName">connection Name</param>
            /// <returns>True, If the current connection is default connection. False, If the current connection is not the default connection.</returns>
            public bool IsDefaultConnection(string connectionName)
            {
                bool Retval = false;
                try
                {
                    //-- Get the Db info on the basis of connection name.
                    string DbInfo = this.DatabaseInfo(connectionName);
                    if (DbInfo == this._DefaultDbInfo)
                    {
                        //-- If it matches with the defaulrDbinfo, return the true.
                        Retval = true;
                    }
                }
                catch (Exception ex)
                {
                    Retval = false;
                }
                return Retval;
            }

            /// <summary>
            /// Get the dataset information on the basis of database info.
            /// </summary>
            /// <param name="dbInfo">Db info contains : ServerType{[]}ConnectionIndex{[]}DatasetPrefix</param>
            /// <returns>Dataset name</returns>
            public string GetDatasetName(string dbInfo)
            {
                string Retval = string.Empty;
                try
                {
                    string[] Values = new string[0];
                    Values = Utility.DICommon.SplitString(dbInfo, DELIMITER);
                    if (Values.Length == 3)
                    {
                        //-- In case of Web, it return the connection for all the servert type.
                        OnlineConnectionDetail.OnlineDataSets DatasetDetails = this._OnlineDatabaseDetails[Convert.ToInt32(Values[1])].OnlineDatasetInfo;
                        foreach (OnlineConnectionDetail.OnlineDataSet DatasetInfo in DatasetDetails)
                        {
                            if (DatasetInfo.Prefix.ToLower() == Values[2].ToLower())
                            {
                                Retval = DatasetInfo.DatasetName;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Retval = string.Empty;
                }
                return Retval;
            }

            /// <summary>
            /// Set the selected connection details on the basis of DBinfo
            /// </summary>
            /// <param name="dbInfo"></param>
            /// <remarks> In case of online connection, Dbinfo contains : ServerType{[]}ConnectionIndex{[]}DatasetPrefix 
            /// In case of offline connection, ServerType{[]}AcessdbPath (Applicable for Dekstop application)  </remarks>
            public void SetSelectedConnectionDetail(string dbInfo)
            {
                DIConnection DIConnection;
                string[] Values = new string[0];
                Values = Utility.DICommon.SplitString(dbInfo, DELIMITER);
                if (Values.Length == 3)
                {
                    //-- In case of Web, it return the connection for all the servert type.
                    this.SelectedConnectionDetail = this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].DIConnectionDetails;
                    this.SelectedConnectionName = this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].Connection;
                    for (int i = 0; i <= this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].OnlineDatasetInfo.Count - 1; i++)
                    {
                        if (this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].OnlineDatasetInfo[i].Prefix.ToLower() == Values[2].ToLower())
                        {
                            //-- set the selected dataset name and its prefix.
                            this.SelectedDatasetName = this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].OnlineDatasetInfo[i].DatasetName;
                            this.SelectedDatasetPrefix = this.OnlineDatabaseDetails[Convert.ToInt32(Values[1])].OnlineDatasetInfo[i].Prefix + "_";
                            break;
                        }
                    }
                    this.SelectedDbInfo = dbInfo;
                    DIConnection = new DIConnection(this.SelectedConnectionDetail);
                    //-- Seleted dataset language                    
                    if (DIConnection.DILanguages(this.SelectedDatasetPrefix).Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode + " = '" + this._DatabaseLanguage.Substring(1) + "'").Length > 0)
                    {
                        //-- If the database language exists in the database
                        this.DatabaseLanguage = this._DatabaseLanguage;
                    }
                    else
                    {
                        //-- Set the default language as databaseLanguage, if the last selected language does not exists in the database.
                        this.DatabaseLanguage = DIConnection.DILanguageCodeDefault(this.SelectedDatasetPrefix);
                    }
                }
                else if (Values.Length == 2)
                {
                    //-- Set the connection detail of access db (only for dekstop)
                    this.SelectedConnectionDetail = new DIConnectionDetails(DIServerType.MsAccess, "", "", Values[1], USERNAME, PASSWORD);
                    this.SelectedConnectionName = string.Empty;
                    this.SelectedDatasetName = System.IO.Path.GetFileName(Values[1]);

                    DIConnection = new DIConnection(this.SelectedConnectionDetail);
                    this.SelectedDatasetPrefix = DIConnection.DIDataSetDefault();
                    this.SelectedDbInfo = dbInfo;
                    //-- Seleted dataset language
                    this.DatabaseLanguage = DIConnection.DILanguageCodeDefault(this.SelectedDatasetPrefix);
                }
            }

            /// <summary>
            /// Retrun the Db info on the basis of connection name.
            /// </summary>
            /// <param name="connectionName">Connection Name</param>
            /// <returns>database information contains ServerType{[]}ConnectionIndex{[]}DatasetPrefix</returns>
            public string DatabaseInfo(string connectionName)
            {
                string Retval = string.Empty;
                try
                {
                    int Index = 0;
                    foreach (DatabasePreference.OnlineConnectionDetail ConnectionDetail in this._OnlineDatabaseDetails)
                    {
                        //-- Set the default database info, if given connection name matches with loop connection name.
                        if (ConnectionDetail.Connection.ToLower() == connectionName.ToLower())
                        {
                            int ServerType = Convert.ToInt32(ConnectionDetail.DIConnectionDetails.ServerType);
                            DIConnection DIConnection = new DIConnection(ConnectionDetail.DIConnectionDetails);
                            string DefaultDataSet = DIConnection.DIDataSetDefault();
                            //-- remove the "_" from prefix.
                            DefaultDataSet = DefaultDataSet.Substring(0, DefaultDataSet.Length - 1);
                            Retval = ServerType.ToString() + DELIMITER + Index.ToString() + DELIMITER + DefaultDataSet;
                            break;
                        }
                        Index += 1;
                    }
                }
                catch (Exception ex)
                {
                    Retval = string.Empty;
                }
                return Retval;
            }



            #region " -- Methods -- "

            /// <summary>
            /// Save the Database Preferencee class in the form of XML.
            /// </summary>
            /// <param name="fileName">file name with path</param>
            public void Save(string fileName)
            {
                try
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(DatabasePreference));
                    StreamWriter PreferenceWriter = new StreamWriter(fileName);
                    oXmlSerializer.Serialize(PreferenceWriter, this);
                    PreferenceWriter.Close();
                }
                catch (Exception ex)
                {

                }
            }

            /// <summary>
            /// Load the deserialize XML file.
            /// </summary>
            /// <param name="fileName">file name with path</param>
            /// <returns></returns>
            public static DatabasePreference Load(string fileName)
            {
                DatabasePreference Retval;
                StreamReader oStreamReader = null;
                try
                {
                    XmlSerializer oXmlSerializer = new XmlSerializer(typeof(DatabasePreference));
                    oStreamReader = new StreamReader(fileName);
                    Retval = (DatabasePreference)oXmlSerializer.Deserialize(oStreamReader);
                    oStreamReader.Close();
                    oStreamReader.Dispose();
                }
                catch (Exception ex)
                {
                    Retval = null;
                    if (oStreamReader != null)
                    {
                        oStreamReader.Dispose();
                    }
                }
                return Retval;
            }


            #endregion

            #endregion

            #region " -- Private -- "

            /// <summary>
            /// Returns the sorted connection list.
            /// </summary>
            /// <returns></returns>
            private SortedList<string, int> GetSortedList()
            {
                SortedList<string, int> Retval = new SortedList<string, int>();
                try
                {
                    int Index = 0;
                    foreach (DatabasePreference.OnlineConnectionDetail ConnectionDetail in this.OnlineDatabaseDetails)
                    {
                        if (ConnectionDetail.DIConnectionDetails.ServerType == DIServerType.MsAccess)
                        {
                            //-- In case of access, add the dbname only.
                            Retval.Add(Path.GetFileName(ConnectionDetail.DIConnectionDetails.DbName), Index);
                        }
                        else
                        {
                            //-- In case of other Db, add the connection name.
                            Retval.Add(ConnectionDetail.Connection, Index);
                        }
                        Index += 1;
                    }
                }
                catch (Exception ex)
                {
                    Retval.Clear();
                }
                return Retval;
            }

            /// <summary>
            /// Add the access database in the collection
            /// </summary>
            /// <param name="dataFolderPath"></param>
            private void OfflineDbCollection(string dataFolderPath)
            {
                string[] DbFiles = new string[0];
                DbFiles = Directory.GetFiles(dataFolderPath, "*.mdb");
                foreach (string OfflineDatabase in DbFiles)
                {
                    //-- Offline database not exists in the collection.
                    if (!IsOfflineDatabaseExists(OfflineDatabase))
                    {
                        DIConnectionDetails OfflineConnectionDetail = new DIConnectionDetails(DIServerType.MsAccess, "", "", OfflineDatabase, USERNAME, PASSWORD);
                        //-- Set the connection
                        DIConnection DIConnection = new DIConnection(OfflineConnectionDetail);
                        //-- Get all the datasets.
                        DataTable DataSetTable = DIConnection.DIDataSets();
                        DatabasePreference.OnlineConnectionDetail.OnlineDataSets OfflineDataSets = new OnlineConnectionDetail.OnlineDataSets();
                        //-- add all datasets into the collection.
                        foreach (DataRow Row in DataSetTable.Rows)
                        {
                            OfflineDataSets.Add(new DatabasePreference.OnlineConnectionDetail.OnlineDataSet(Row[DBAvailableDatabases.AvlDBPrefix].ToString(), Row[DBAvailableDatabases.AvlDBName].ToString()));
                        }
                        DatabasePreference.OnlineConnectionDetail ConnectionDetail = new OnlineConnectionDetail(OfflineConnectionDetail, OfflineDataSets, Path.GetFileName(OfflineDatabase));
                        //-- add the offline database details.
                        this.OnlineDatabaseDetails.Add(ConnectionDetail);
                    }
                }
            }

            /// <summary>
            /// Remove the access database from the collection.
            /// </summary>
            private void RemoveOfflineConnection()
            {
                try
                {
                    int ConnectionCount = this._OnlineDatabaseDetails.Count;
                    for (int i = 0; i < ConnectionCount; i++)
                    {
                        //-- Remove Access database which are not in the data folder.
                        if (this._OnlineDatabaseDetails[i].DIConnectionDetails.ServerType == DIServerType.MsAccess && !File.Exists(this._OnlineDatabaseDetails[i].DIConnectionDetails.DbName))
                        {
                            if (this._SelectedConnectionName.ToLower() == this._OnlineDatabaseDetails[i].Connection.ToLower())
                            {
                                this.ResetDefaultConnection();
                            }
                            this._OnlineDatabaseDetails.RemoveAt(i);
                            ConnectionCount--;
                            i--;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            /// <summary>
            /// Reset the selected connection details.
            /// </summary>
            private void ResetDefaultConnection()
            {
                this._SelectedConnectionDetail = null;
                this._SelectedConnectionName = string.Empty;
                this._SelectedDatasetName = string.Empty;
            }

            /// <summary>
            /// Check for the existence of offline database in the collection.
            /// </summary>
            /// <param name="dbFileWithPath"></param>
            /// <returns></returns>
            private bool IsOfflineDatabaseExists(string dbFileWithPath)
            {
                bool Retval = false;
                try
                {
                    foreach (DatabasePreference.OnlineConnectionDetail ConnectionDetail in this.OnlineDatabaseDetails)
                    {
                        if (ConnectionDetail.DIConnectionDetails.ServerType == DIServerType.MsAccess && ConnectionDetail.DIConnectionDetails.DbName.ToLower() == dbFileWithPath.ToLower())
                        {
                            Retval = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Retval = false;
                }
                return Retval;
            }

            #endregion

            #endregion

            #region " -- Database connection string inner class -- "

            /// <summary>
            /// Stores the database information
            /// </summary>
            public class OnlineConnectionDetail
            {
                #region " -- New / Dispose -- "

                /// <summary>
                /// constructor to save the database information
                /// </summary>
                /// <param name="connectionDetails">Connection Details</param>
                /// <param name="datasetInfo">Dataset information</param>         
                public OnlineConnectionDetail(DIConnectionDetails connectionDetails, OnlineDataSets datasetInfo, string connection)
                {
                    this._DIConnectionDetails = connectionDetails;
                    this._OnlineDatasetInfo = datasetInfo;
                    this._Connection = connection;
                }

                /// <summary>
                /// Constructor only for XML serialization.
                /// </summary>
                public OnlineConnectionDetail()
                {
                }

                #endregion

                #region " -- Properties -- "

                private DIConnectionDetails _DIConnectionDetails;
                /// <summary>
                /// Get or sets the connection details.
                /// </summary>        
                public DIConnectionDetails DIConnectionDetails
                {
                    get
                    {
                        return this._DIConnectionDetails;
                    }
                    set
                    {
                        this._DIConnectionDetails = value;
                    }
                }

                private string _Connection;
                /// <summary>
                /// Gets or sets the connection name.
                /// </summary>
                public string Connection
                {
                    get
                    {
                        return this._Connection;
                    }
                    set
                    {
                        this._Connection = value;
                    }
                }

                private OnlineDataSets _OnlineDatasetInfo;
                /// <summary>
                /// Get or sets the dataset information.
                /// </summary>
                public OnlineDataSets OnlineDatasetInfo
                {
                    get
                    {
                        return this._OnlineDatasetInfo;
                    }
                    set
                    {
                        this._OnlineDatasetInfo = value;
                    }
                }

                #endregion

                #region "-- Dataset information inner class --"

                /// <summary>
                /// Class to store the information about the dataset.
                /// </summary>
                public class OnlineDataSet
                {

                    #region " -- New / Dispose -- "

                    public OnlineDataSet(string prefix, string datasetName)
                    {
                        this._DatasetName = datasetName;
                        this._Prefix = prefix;
                    }

                    /// <summary>
                    /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
                    /// </summary>
                    public OnlineDataSet()
                    { }

                    #endregion

                    #region " -- Properties -- "

                    private string _Prefix;
                    /// <summary>
                    /// Get or sets the dataset prefix
                    /// </summary>
                    public string Prefix
                    {
                        get
                        {
                            return this._Prefix;
                        }
                        set
                        {
                            this._Prefix = value;
                        }
                    }

                    private string _DatasetName;
                    /// <summary>
                    /// Get or sets the dataset name.
                    /// </summary>
                    public string DatasetName
                    {
                        get
                        {
                            return this._DatasetName;
                        }
                        set
                        {
                            this._DatasetName = value;
                        }
                    }



                    #endregion
                }

                /// <summary>
                /// Stores the dataset info into the collection.
                /// </summary>
                public class OnlineDataSets : System.Collections.CollectionBase
                {
                    #region " -- Methods -- "

                    /// <summary>
                    /// Add the details into the collection
                    /// </summary>
                    /// <param name="databaseInfo"></param>
                    public void Add(OnlineDataSet onlineDataSet)
                    {
                        this.InnerList.Add(onlineDataSet);
                    }

                    /// <summary>
                    /// Get the item from the collection.
                    /// </summary>
                    /// <param name="index"></param>
                    /// <returns></returns>
                    public OnlineDataSet this[int index]
                    {
                        get
                        {
                            OnlineDataSet Retval = null;
                            try
                            {
                                Retval = (OnlineDataSet)this.InnerList[index];
                            }
                            catch (Exception ex)
                            {
                                Retval = null;
                            }
                            return Retval;
                        }
                    }

                    /// <summary>
                    /// Remove the item from collection.
                    /// </summary>
                    /// <param name="index"></param>
                    public void Remove(int index)
                    {
                        this.InnerList.RemoveAt(index);
                    }

                    /// <summary>
                    /// Remove the item from collection
                    /// </summary>
                    /// <param name="onlineDataSet"></param>
                    public void Remove(OnlineDataSet onlineDataSet)
                    {
                        this.InnerList.Remove(onlineDataSet);
                    }

                    #endregion
                }

                #endregion
            }

            /// <summary>
            /// Stores the database info into the collection.
            /// </summary>
            public class OnlineConnectionDetails : System.Collections.CollectionBase
            {
                #region " -- Methods -- "

                /// <summary>
                /// Add the details into the collection
                /// </summary>
                /// <param name="databaseInfo"></param>
                public void Add(OnlineConnectionDetail onlineConnectionDetail)
                {
                    bool IsDuplicate = false;
                    foreach (OnlineConnectionDetail Connection in this.InnerList)
                    {
                        if (string.Compare(onlineConnectionDetail.Connection, Connection.Connection, true) == 0)
                        {
                            IsDuplicate = true;
                            break;
                        }
                    }
                    if (!IsDuplicate)
                    {
                        this.InnerList.Add(onlineConnectionDetail);
                    }
                }

                /// <summary>
                /// Get the item from the collection.
                /// </summary>
                /// <param name="index"></param>
                /// <returns></returns>
                public OnlineConnectionDetail this[int index]
                {
                    get
                    {
                        OnlineConnectionDetail Retval = null;
                        try
                        {
                            Retval = (OnlineConnectionDetail)this.InnerList[index];
                        }
                        catch (Exception ex)
                        {
                            Retval = null;
                        }
                        return Retval;
                    }
                }

                /// <summary>
                /// Insert the connection details at the specified location.
                /// </summary>
                /// <param name="index">Index</param>
                /// <param name="onlineConnectionDetail">Connection Details</param>
                public void Insert(int index, OnlineConnectionDetail onlineConnectionDetail)
                {
                    this.InnerList.Insert(index, onlineConnectionDetail);
                }

                /// <summary>
                /// Remove the item from collection.
                /// </summary>
                /// <param name="index"></param>
                public void Remove(int index)
                {
                    this.InnerList.RemoveAt(index);
                }

                /// <summary>
                /// Remove the item from collection
                /// </summary>
                /// <param name="onlineConnectionDetail"></param>
                public void Remove(OnlineConnectionDetail onlineConnectionDetail)
                {
                    this.InnerList.Remove(onlineConnectionDetail);
                }

                /// <summary>
                /// Remove the item from collection
                /// </summary>
                /// <param name="connectionName"></param>
                public void Remove(string connectionName)
                {
                    foreach (OnlineConnectionDetail Connection in this.InnerList)
                    {
                        if (string.Compare(Connection.Connection, connectionName, true) == 0)
                        {
                            this.InnerList.Remove(Connection);
                            break;
                        }
                    }
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region "-- MRU Classes --"

        /// <summary>
        /// Represents the MRU section of User Preferences which were previously stored in pref.xml        
        /// </summary>
        public class MRUPreference
        {
            #region "-- New / Dispose --"

            public MRUPreference()
            {
                this._MRUDatabase = new List<string>();
                this._MRULanguage = new List<string>();
            }

            #endregion

            #region "-- Properties --"

            private List<string> _MRUDatabase;
            /// <summary>
            /// Gets or sets the MRU database.
            /// <remarks> Key : mru_databases </remarks>
            /// </summary>
            /// TODO : CAP & MDG Specific, TO be removed if possible
            public List<string> MRUDatabase
            {
                get
                {
                    return this._MRUDatabase;
                }
                set
                {
                    this._MRUDatabase = value;
                }
            }

            private List<string> _MRULanguage;
            /// <summary>
            /// Gets or sets the MRU language.
            /// <remarks> Key :  </remarks>
            /// </summary>
            /// TODO : CAP & MDG Specific, TO be removed if possible
            public List<string> MRULanguage
            {
                get
                {
                    return this._MRULanguage;
                }
                set
                {
                    this._MRULanguage = value;
                }
            }

            #endregion

        }

        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Variable -- "

        private static string LanguageFolderPath = string.Empty;

        #endregion

        #region " -- Methods -- "

        private void _Language_UpdateLanguageEvent(string languageFile)
        {
            this._DataView.UpdateAllFields(Path.Combine(languageFile, LanguageFolderPath + ".xml"));
        }

        /// <summary>
        /// If there is any change regarding to the Data related porperty, its event will get fired.
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        private bool DataviewAffected(UserPreference tempUserPreference)
        {
            int Index = 0;

            #region " -- ContentAffected -- "

            //-- Paging
            if (this.General.PageSize != tempUserPreference.General.PageSize)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            //////// Table Preview Limit
            //////if (this.DataView.TablePreviewLimit != tempUserPreference.DataView.TablePreviewLimit)
            //////{
            //////    this._UserPreferenceChanged = AffectedDataview.ContentAffected;
            //////    this.RaiseDataContentChangedEvent();
            //////    return true;
            //////}

            //////// Graph Preview Limit
            //////if (this.DataView.GraphPreviewLimit != tempUserPreference.DataView.GraphPreviewLimit)
            //////{
            //////    this._UserPreferenceChanged = AffectedDataview.ContentAffected;
            //////    this.RaiseDataContentChangedEvent();
            //////    return true;
            //////}

            //-- DB language
            if (this.Database.DatabaseLanguage != tempUserPreference.Database.DatabaseLanguage)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            //-- Data
            if (this.DataView.Fields.Rows.Count != tempUserPreference.DataView.Fields.Rows.Count)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            if (this.General.ShowRecommendedSourceColor != tempUserPreference.General.ShowRecommendedSourceColor)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            //-- Existence of field.
            foreach (Field DvField in this.DataView.Fields.Rows)
            {
                if (tempUserPreference.DataView.Fields.Rows[DvField.FieldID] == null)
                {
                    this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                    this.RaiseDataContentChangedEvent();
                    return true;
                }
            }

            //-- Metadata fields
            if (this.DataView.MetadataIndicatorField != tempUserPreference.DataView.MetadataIndicatorField || this.DataView.MetadataAreaField != tempUserPreference.DataView.MetadataAreaField || this.DataView.MetadataSourceField != tempUserPreference.DataView.MetadataSourceField)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            //-- Ic fields
            if (this.DataView.ICSectorGIds != tempUserPreference.DataView.ICSectorGIds || this.DataView.ICSourceGIds != tempUserPreference.DataView.ICSourceGIds || this.DataView.ICInstitutionalGIds != tempUserPreference.DataView.ICInstitutionalGIds || this.DataView.ICThemeGIds != tempUserPreference.DataView.ICThemeGIds || this.DataView.ICConventionGIds != tempUserPreference.DataView.ICConventionGIds || this.DataView.ICGoalGIds != tempUserPreference.DataView.ICGoalGIds)
            {
                this._UserPreferenceChanged = AffectedDataview.ContentAffected;
                this.RaiseDataContentChangedEvent();
                return true;
            }

            #endregion

            #region " -- LayoutAffected -- "

            //-- global Color
            if (this.General.ShowGlobalColor != tempUserPreference.General.ShowGlobalColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            if (this.General.GlobalColor != tempUserPreference.General.GlobalColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            //-- Source
            if (this.General.ShowRecommendedSourceColor != tempUserPreference.General.ShowRecommendedSourceColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            if (this.General.RecommendedSourceColor != tempUserPreference.General.RecommendedSourceColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            //-- Comments
            if (this.General.ShowComments != tempUserPreference.General.ShowComments)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            //-- Interface Language
            if (this.Language.InterfaceLanguage != tempUserPreference.Language.InterfaceLanguage)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            //-- Field Order.
            for (Index = 0; Index < this.DataView.Fields.Rows.Count; Index++)
            {
                if (this.DataView.Fields.Rows[Index].FieldID != tempUserPreference.DataView.Fields.Rows[Index].FieldID)
                {
                    this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                    this.DataLayoutChangedEvent();
                    return true;
                }
            }

            //-- Alternate dataview color
            if (this.DataView.ShowDataviewAlternateColor != tempUserPreference.DataView.ShowDataviewAlternateColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            if (this.DataView.DataviewAlternateColor != tempUserPreference.DataView.DataviewAlternateColor)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            if (this.DataView.AllowRowFiltering != tempUserPreference.DataView.AllowRowFiltering)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            if (this.DataView.DataValueAlignment != tempUserPreference.DataView.DataValueAlignment)
            {
                this._UserPreferenceChanged = AffectedDataview.LayoutAffected;
                this.RaiseDataLayoutChangedEvent();
                return true;
            }

            #endregion

            this._UserPreferenceChanged = AffectedDataview.None;
            return false;
        }

        /// <summary>
        /// PreviewChangeEvent is fired, if there is any change in preview limit properties
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        private bool PreviewAffected(UserPreference tempUserPreference)
        {
            bool IsGraphChanged = false;    //-- Set the flag to true, so that table preview is redraw if there is any change in the property.


            //-- Show excel
            if (!IsGraphChanged && this._General.ShowExcel != tempUserPreference.General.ShowExcel)
            {
                this.RaisePreviewChangedEvent(Presentation.PresentationType.Graph);
                //-- Set the flag to true, so that table preview is redraw if there is any change in the property.
                IsGraphChanged = true;
            }


            if (!IsGraphChanged && this.DataView.GraphPreviewLimit != tempUserPreference.DataView.GraphPreviewLimit)
            {
                this.RaisePreviewChangedEvent(Presentation.PresentationType.Graph);
                IsGraphChanged = true;
            }

            if (!IsGraphChanged && this.DataView.DIWizardGraphPreviewLimit != tempUserPreference.DataView.DIWizardGraphPreviewLimit)
            {
                this.RaisePreviewChangedEvent(Presentation.PresentationType.Graph);
                IsGraphChanged = true;
            }

            //-- Preview limit change
            if (this.DataView.TablePreviewLimit != tempUserPreference.DataView.TablePreviewLimit)
            {
                this.RaisePreviewChangedEvent(Presentation.PresentationType.Table);
                return true;
            }

            if (this.DataView.DIWizardTablePreviewLimit != tempUserPreference.DataView.DIWizardTablePreviewLimit)
            {
                this.RaisePreviewChangedEvent(Presentation.PresentationType.Table);
                return true;
            }

            return false;
        }

        /// <summary>
        /// If there is any change regarding to the Indicator related porperty, its event will get fired.
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        private bool IndicatorAffected(UserPreference tempUserPreference)
        {
            //-- Page Size
            if (this.General.PageSize != tempUserPreference.General.PageSize)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- global Color
            if (this.General.ShowGlobalColor != tempUserPreference.General.ShowGlobalColor)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- global Color
            if (this.General.GlobalColor != tempUserPreference.General.GlobalColor)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- Interface lang.
            if (this.Language.InterfaceLanguage != tempUserPreference.Language.InterfaceLanguage)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- Database lang.
            if (this.Database.DatabaseLanguage != tempUserPreference.Database.DatabaseLanguage)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- Selectcted IC type
            if (this.Indicator.SelectedICType != tempUserPreference.Indicator.SelectedICType)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- Show IUS
            if (this.UserSelection.ShowIUS != tempUserPreference.UserSelection.ShowIUS)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            //-- Show IUS
            if (this.Indicator.ShowIndicatorWithData != tempUserPreference.Indicator.ShowIndicatorWithData)
            {
                this.RaiseIndicatorContentChangedEvent();
                return true;
            }

            return false;
        }

        /// <summary>
        /// If there is any change regarding to the Area related porperty, its event will get fired.
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        private bool AreaAffected(UserPreference tempUserPreference)
        {
            //-- sort by AreaId
            if (this.General.SortAreaTreeByAreaId != tempUserPreference.General.SortAreaTreeByAreaId)
            {
                this.RaiseAreaContentChangedEvent();
                return true;
            }

            //-- global Color
            if (this.General.ShowGlobalColor != tempUserPreference.General.ShowGlobalColor)
            {
                this.RaiseAreaContentChangedEvent();
                return true;
            }

            //-- global Color
            if (this.General.GlobalColor != tempUserPreference.General.GlobalColor)
            {
                this.RaiseAreaContentChangedEvent();
                return true;
            }

            //-- Interface lang.
            if (this.Language.InterfaceLanguage != tempUserPreference.Language.InterfaceLanguage)
            {
                this.RaiseAreaContentChangedEvent();
                return true;
            }

            //-- Database lang.
            if (this.Database.DatabaseLanguage != tempUserPreference.Database.DatabaseLanguage)
            {
                this.RaiseAreaContentChangedEvent();
                return true;
            }

            return false;
        }

        /// <summary>
        /// If there is any change regarding to the timeperiod related porperty, its event will get fired.
        /// </summary>
        /// <param name="tempUserPreference"></param>
        /// <returns></returns>
        private bool TimePeriodAffected(UserPreference tempUserPreference)
        {
            //-- Interface lang.
            if (this.Language.InterfaceLanguage != tempUserPreference.Language.InterfaceLanguage)
            {
                this.RaiseTimePeriodContentChangedEvent();
                return true;
            }

            //-- Database lang.
            if (this.Database.DatabaseLanguage != tempUserPreference.Database.DatabaseLanguage)
            {
                this.RaiseTimePeriodContentChangedEvent();
                return true;
            }

            return false;
        }

        #endregion

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            object RetVal = null;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(UserPreference));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (UserPreference)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return (UserPreference)RetVal;
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Dispose 
        /// </summary>
        public void Dispose()
        {

        }

        #endregion
    }
}
