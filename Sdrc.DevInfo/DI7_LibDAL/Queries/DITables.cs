using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries
{
    /// <summary>
    /// Provides the name of tables including dataset and language code.
    /// </summary>
    /// <remarks>
    /// Ensure all table name are in lower case to handle case sensetive table names for mysql installed over linux
    /// </remarks>
    public class DITables
    {

        public static string ICIUSTableName = "ic_ius";
        public static string Old_ICIUSTableName = "indicator_classifications_ius";

        #region "-- Private --"

        #region "-- Variables --"

        private string DataPrefix;
        private string LanguageCode;

        #endregion

        #region "-- New / Dispose --"

        private DITables()
        {
            // don't implement this method
        }

        #endregion

        #region "-- Methods --"

        private void ProcessTablesName()
        {
            // Add Data Prefix and Language Code 
            // Language Code will suffixed only to tables with text fields (exclude relationship tables)

            // this._DBAvailableDatabases = this.AddDataPrefixOnly(this._DBAvailableDatabases);
            this._Language = this.AddDataPrefixOnly(this._Language);

            this._Indicator = this.AddDataPrefixNLanuageCode(this._Indicator);
            this._Unit = this.AddDataPrefixNLanuageCode(this._Unit);

            this._Subgroup = this.AddDataPrefixNLanuageCode(this._Subgroup);
            this._SubgroupVals = this.AddDataPrefixNLanuageCode(this._SubgroupVals);
            this._AgePeriod = this.AddDataPrefixNLanuageCode(this._AgePeriod);

            this._IndicatorUnitSubgroup = this.AddDataPrefixOnly(this._IndicatorUnitSubgroup);

            this._IndicatorClassifications = this.AddDataPrefixNLanuageCode(this._IndicatorClassifications);
            this._IndicatorClassificationsIUS = this.AddDataPrefixOnly(this._IndicatorClassificationsIUS);
            this._CFFlowChart = this.AddDataPrefixOnly(this._CFFlowChart);

            this._TimePeriod = this.AddDataPrefixOnly(this._TimePeriod);



            this._Area = this.AddDataPrefixNLanuageCode(this._Area);
            this._AreaMap = this.AddDataPrefixOnly(this._AreaMap);
            this._AreaLevel = this.AddDataPrefixNLanuageCode(this._AreaLevel);
            this._AreaFeatureType = this.AddDataPrefixNLanuageCode(this._AreaFeatureType);
            this._AreaMapLayer = this.AddDataPrefixOnly(this._AreaMapLayer);
            this._AreaMapMetadata = this.AddDataPrefixNLanuageCode(this._AreaMapMetadata);

            this._Data = this.AddDataPrefixOnly(this._Data);

            this._FootNote = this.AddDataPrefixNLanuageCode(this._FootNote);

            this._NotesClassification = this.AddDataPrefixNLanuageCode(this._NotesClassification);
            this._Notes = this.AddDataPrefixNLanuageCode(this._Notes);
            this._NotesData = this.AddDataPrefixOnly(this._NotesData);
            this._NotesProfile = this.AddDataPrefixOnly(this._NotesProfile);

            this._AssistanteBook = this.AddDataPrefixNLanuageCode(this._AssistanteBook);
            this._Assistant = this.AddDataPrefixNLanuageCode(this._Assistant);
            this._AssistantTopic = this.AddDataPrefixNLanuageCode(this._AssistantTopic);

            this._Icons = this.AddDataPrefixOnly(this._Icons);

            this._ElementXSLT = this.AddDataPrefixOnly(this._ElementXSLT);
            this._XSLT = this.AddDataPrefixOnly(this._XSLT);

            this._TemplateLog = this.AddDataPrefixOnly(this._TemplateLog);
            this._DatabaseLog = this.AddDataPrefixOnly(this._DatabaseLog);

            //DevInfo v.6.0.0.1
            this._SubgroupType = this.AddDataPrefixNLanuageCode(this._SubgroupType);
            this._SubgroupValsSubgroup = this.AddDataPrefixOnly(this._SubgroupValsSubgroup);

            // DevInfo v.6.0.0.2
            this._DBMetadata = this.AddDataPrefixNLanuageCode(this._DBMetadata);

            // DevInfo v.6.0.0.3
            this._RecommendedSources = this.AddDataPrefixNLanuageCode(this._RecommendedSources);

            // DevInfo v.6.0.0.5
            this._MetadataCategory = this.AddDataPrefixNLanuageCode(this._MetadataCategory);
            this._Document = this.AddDataPrefixOnly(this._Document);

            // Devinfo v.7.0.0.0
            this._MetadataReport = this.AddDataPrefixNLanuageCode(this._MetadataReport);

            this._SDMXUser = this.AddDataPrefixOnly(this._SDMXUser);

        }

        #endregion

        #endregion

        #region "-- public   --"

        #region "-- Variables / Properties --

        /// <summary>
        /// Gets data prefix
        /// </summary>
        public  string CurrentDataPrefix
        {
            get { return this.DataPrefix; }
            
        }

        
        /// <summary>
        /// Gets language code
        /// </summary>
        public string CurrentLanguageCode
        {
            get { return this.LanguageCode; }
            
        }
	
	

        #region "-- Language Independent Tables --"

        private string _DBUserAccess = "user_access";
        /// <summary>
        /// Get User_Access Table for Online
        /// </summary>
        public string DBUserAccess
        {
            get { return this._DBUserAccess; }

        }

        private string _DBUser = "user";
        /// <summary>
        /// Get User Table for Online
        /// </summary>
        public string DBUser
        {
            get { return this._DBUser; }

        }
        private string _DIUser = "di_user";
        /// <summary>
        /// Get DI_User Table for Online
        /// </summary>
        public string DIUser
        {
            get { return this._DIUser; }
        }
        private string _DBAvailableDatabases = "db_available_databases";
        /// <summary>
        /// Gets DBAvailableDatabase table
        /// </summary>
        public string DBAvailableDatabases
        {
            get
            {
                return this._DBAvailableDatabases;
            }
        }

        private string _Language = "language";
        /// <summary>
        /// Gets Language table
        /// </summary>
        public string Language
        {
            get
            {
                return this._Language;
            }
        }

        private string _IndicatorUnitSubgroup = "indicator_unit_subgroup";
        /// <summary>
        /// Gets Indicator Unit Subgroup table
        /// </summary>
        public string IndicatorUnitSubgroup
        {
            get
            {
                return this._IndicatorUnitSubgroup;
            }
        }

        private string _CFFlowChart = "cf_flowchart";
        /// <summary>
        /// Gets CF FlowChart table
        /// </summary>
        public string CFFlowChart
        {
            get
            {
                return this._CFFlowChart;
            }
        }

       
        //        private string _IndicatorClassificationsIUS = "indicator_classifications_ius";
        private string _IndicatorClassificationsIUS = DITables.ICIUSTableName;
        /// <summary>
        /// Gets IndicatorClassifications IUS table
        /// </summary>
        public string IndicatorClassificationsIUS
        {
            get
            {
                return this._IndicatorClassificationsIUS;
            }
        }

        private string _TimePeriod = "timeperiod";
        /// <summary>
        ///  Gets Timeperiod table
        /// </summary>
        public string TimePeriod
        {
            get
            {
                return this._TimePeriod;
            }
        }

        private string _AreaMap = "area_map";
        /// <summary>
        /// Gets AreapMap table
        /// </summary>
        public string AreaMap
        {
            get
            {
                return this._AreaMap;
            }
        }

        private string _AreaMapLayer = "area_map_layer";
        /// <summary>
        /// Gets AreaMapLayer table
        /// </summary>
        public string AreaMapLayer
        {
            get
            {
                return this._AreaMapLayer;
            }
        }

        private string _Data = "data";
        /// <summary>
        /// Gets Data table
        /// </summary>
        public string Data
        {
            get
            {
                return this._Data;
            }
        }

        private string _Icons = "icons";
        /// <summary>
        /// Gets Icons table
        /// </summary>
        public string Icons
        {
            get
            {
                return this._Icons;
            }
        }

        private string _NotesProfile = "notes_profile";
        /// <summary>
        /// Gets Notes Profile table
        /// </summary>
        public string NotesProfile
        {
            get
            {
                return this._NotesProfile;
            }
        }

        private string _NotesData = "notes_data";
        /// <summary>
        ///  Gets NotesData table
        /// </summary>
        public string NotesData
        {
            get
            {
                return this._NotesData;
            }
        }

        private string _XSLT = "xslt";
        /// <summary>
        /// Gets XSLT table
        /// </summary>
        public string XSLT
        {
            get
            {
                return this._XSLT;
            }
        }

        private string _ElementXSLT = "element_xslt";
        /// <summary>
        /// Gets Elements XSLT table
        /// </summary>
        public string ElementXSLT
        {
            get
            {
                return this._ElementXSLT;
            }
        }

        private string _TemplateLog = "template_log";
        /// <summary>
        /// Gets Template log table
        /// </summary>
        public string TemplateLog
        {
            get
            {
                return this._TemplateLog;
            }
        }

        private string _DatabaseLog = "database_log";
        /// <summary>
        /// Gets Database log table
        /// </summary>
        public string DatabaseLog
        {
            get
            {
                return this._DatabaseLog;
            }
        }

        private string _DBVersion = "db_version";
        /// <summary>
        /// Gets DB_Version table
        /// </summary>
        public string DBVersion
        {
            get
            {
                return this._DBVersion;
            }
        }

        private string _Document = "document";
        /// <summary>
        /// Gets Document table
        /// </summary>
        public string Document
        {
            get
            {
                return this._Document;
            }
        }

        private string _SubgroupValsSubgroup = "subgroup_vals_subgroup";
        /// <summary>
        /// Gets Subgroup_Vals_Subgroup table
        /// </summary>
        public string SubgroupValsSubgroup
        {
            get { return this._SubgroupValsSubgroup; }
            set { this._SubgroupValsSubgroup = value; }
        }


        #endregion

        #region "-- Language Tables --"

        private string _AgePeriod = "ageperiod";
        /// <summary>
        /// Gets AgePeriod table
        /// </summary>
        public string AgePeriod
        {
            get
            {
                return this._AgePeriod;
            }
        }

        private string _Area = "area";
        /// <summary>
        /// Gets Area table
        /// </summary>
        public string Area
        {
            get
            {
                return this._Area;
            }
        }

        private string _AreaFeatureType = "area_feature_type";
        /// <summary>
        /// Gets Area Feature type table
        /// </summary>
        public string AreaFeatureType
        {
            get
            {
                return this._AreaFeatureType;
            }
        }

        private string _AreaLevel = "area_level";
        /// <summary>
        /// Gets Area level table
        /// </summary>
        public string AreaLevel
        {
            get
            {
                return this._AreaLevel;
            }
        }

        private string _AreaMapMetadata = "area_map_metadata";
        /// <summary>
        ///  Gets AreaMap Metadata table
        /// </summary>
        public string AreaMapMetadata
        {
            get
            {
                return this._AreaMapMetadata;
            }
        }

        private string _AssistanteBook = "assistant_ebook";
        /// <summary>
        /// Gets Assistant eBook table
        /// </summary>
        public string AssistanteBook
        {
            get
            {
                return this._AssistanteBook;
            }
        }

        private string _Assistant = "assistant";
        /// <summary>
        /// Gets Assistant table
        /// </summary>
        public string Assistant
        {
            get
            {
                return this._Assistant;
            }
        }

        private string _AssistantTopic = "assistant_topic";
        /// <summary>
        /// Gets Assistant Topic table
        /// </summary>
        public string AssistantTopic
        {
            get
            {
                return this._AssistantTopic;
            }
        }

        private string _FootNote = "footnote";
        /// <summary>
        /// Gets Footnote table
        /// </summary>
        public string FootNote
        {
            get
            {
                return this._FootNote;
            }
        }

        private string _IndicatorClassifications = "indicator_classifications";
        /// <summary>
        /// Gets Indicator classification table
        /// </summary>
        public string IndicatorClassifications
        {
            get
            {
                return this._IndicatorClassifications;
            }
        }

        private string _Indicator = "indicator";
        /// <summary>
        /// Gets Indicator table
        /// </summary>
        public string Indicator
        {
            get
            {
                return this._Indicator;
            }
        }

        private string _NotesClassification = "notes_classification";
        /// <summary>
        /// Gets Notes Classification table
        /// </summary>
        public string NotesClassification
        {
            get
            {
                return this._NotesClassification;
            }
        }

        private string _Notes = "notes";
        /// <summary>
        /// Gets Notes table
        /// </summary>
        public string Notes
        {
            get
            {
                return this._Notes;
            }
        }

        private string _Subgroup = "subgroup";
        /// <summary>
        /// Gets Subgroup table
        /// </summary>
        public string Subgroup
        {
            get
            {
                return this._Subgroup;
            }
        }

        private string _SubgroupVals = "subgroup_vals";
        /// <summary>
        /// Gets SubgroupVals table
        /// </summary>
        public string SubgroupVals
        {
            get
            {
                return this._SubgroupVals;
            }
        }

        private string _Unit = "unit";
        /// <summary>
        /// Gets Unit table
        /// </summary>
        public string Unit
        {
            get
            {
                return this._Unit;
            }
        }

        private string _SubgroupType = "subgroup_type";
        /// <summary>
        ///Gets Subgroup_Type table
        /// </summary>
        public string SubgroupType
        {
            get { return this._SubgroupType; }
        }

        private string _DBMetadata = "dbmetadata";
        /// <summary>
        /// Gets DBMetadata table
        /// </summary>
        public string DBMetadata
        {
            get { return this._DBMetadata; }
        }



        private string _RecommendedSources = "recommendedsources";
        /// <summary>
        /// Gets RecommendedSources table
        /// </summary>
        public string RecommendedSources
        {
            get { return this._RecommendedSources; }
        }

        private string _MetadataCategory = "metadata_category";
        /// <summary>
        /// Gets Metadata_Category table
        /// </summary>
        public string MetadataCategory
        {
            get { return this._MetadataCategory; }
        }


        private string _MetadataReport = "metadatareport";
        /// <summary>
        /// Gets metadata_report table
        /// </summary>
        public string MetadataReport
        {
            get { return this._MetadataReport; }
        }

        private string _SDMXUser = "SDMXUser";
        /// <summary>
        /// Get and Set Sender Table
        /// </summary>
        public string SDMXUser
        {
            get { return this._SDMXUser; }
            set { this._SDMXUser = value; }
        }


        private List<string> _LanguageDependentTablesName = new List<string>();
        /// <summary>
        /// Gets language dependent tables name 
        /// </summary>
        public List<string> LanguageDependentTablesName
        {
            get { return this._LanguageDependentTablesName; }
        }


        #endregion

        #endregion

        #region "-- New / Dispose --"

        /// <summary>
        /// Constructor to create instance of DITables.
        /// </summary>
        /// <param name="dataPrefix">Dataset prefix like UT_</param>
        /// <param name="languageCode">Language code like _en</param>
        public DITables(string dataPrefix, string languageCode)
        {
            this.DataPrefix = dataPrefix.ToLower();

            if (!languageCode.StartsWith("_"))
                languageCode = "_" + languageCode;

            this.LanguageCode = languageCode.ToLower();
            this.ProcessTablesName();
        }

        #endregion




        #endregion

        #region "-- Protected --"

        #region"-- Methods --"

        protected string AddDataPrefixOnly(string tableName)
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return (this.DataPrefix + tableName.ToLower());
        }

        protected string AddDataPrefixNLanuageCode(string tableName)
        {
            string RetVal = string.Empty;
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            RetVal = this.DataPrefix + tableName.ToLower() + this.LanguageCode;

            // add into LangaugeDependentTablesName list
            if (this._LanguageDependentTablesName.Contains(RetVal) == false)
            {
                this._LanguageDependentTablesName.Add(RetVal);
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
