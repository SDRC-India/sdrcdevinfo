using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries
{
    /// <summary>
    /// Allows to get select queries.To get distinct indicators use : ObjectOfDIQueries.Indicator.Distinct() 
    /// </summary>
    public class DIQueries : IDisposable
    {

        #region "-- Public / Internal --"

        #region "-- Variables / Properties --"

        private string _DataPrefix;
        /// <summary>
        /// Prefix used to identefy a specific dataset among multiple datasets residing ina single database. e.g. "UT_" "RT_" 
        /// </summary>
        public string DataPrefix
        {
            get { return _DataPrefix; }
        }

        private string _LanguageCode;

        /// <summary>
        /// Language suffix for laguage tables. e.g. "_en" "_fr"
        /// </summary>
        public string LanguageCode
        {
            get { return _LanguageCode; }
        }

        private Area.Select _Area;
        /// <summary>
        /// Returns select queries for Area
        /// </summary>
        public Area.Select Area
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Area == null)
                {
                    this._Area = new Area.Select(this.TablesName);
                }

                return this._Area;
            }
        }

        private AgePeriod.Select _AgePeriod;
        /// <summary>
        /// Returns select queries for AgePeriod
        /// </summary>
        public AgePeriod.Select AgePeriod
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._AgePeriod == null)
                {
                    this._AgePeriod = new AgePeriod.Select(this.TablesName);
                }

                return this._AgePeriod;
            }
        }

        private Assistant.Select _Assistant;
        /// <summary>
        /// Returns select queries for Assistant 
        /// </summary>
        public Assistant.Select Assistant
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Assistant == null)
                {
                    this._Assistant = new Assistant.Select(this.TablesName);
                }

                return this._Assistant;
            }
        }

        private Data.Select _Data;
        /// <summary>
        ///  Returns select queries for Data
        /// </summary>
        public Data.Select Data
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Data == null)
                {
                    this._Data = new Data.Select(this.TablesName);
                }

                return this._Data;
            }
        }

        private Icon.Select _Icon;
        /// <summary>
        ///  Returns select queries for Icon
        /// </summary>
        public Icon.Select Icon
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Icon == null)
                {
                    this._Icon = new Icon.Select(this.TablesName);
                }

                return this._Icon;
            }
        }

        private Indicator.Select _Indicators;
        /// <summary>
        ///  Returns select queries for Indicator
        /// </summary>
        public Indicator.Select Indicators
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Indicators == null)
                {
                    this._Indicators = new Indicator.Select(this.TablesName);
                }

                return _Indicators;
            }
        }

        private MetadataReport.Select _MetadataReport;
        /// <summary>
        /// Gets or sets queries for MeatadataReport
        /// </summary>
        public MetadataReport.Select MetadataReport
        {
            get {
                if (this._MetadataReport == null)
                { this._MetadataReport = new MetadataReport.Select(this.TablesName); }

                return _MetadataReport; }
            
        }
	


        private IndicatorClassification.Select _IndicatorClassification;
        /// <summary>
        ///  Returns select queries for IndicatorClassification
        /// </summary>
        public IndicatorClassification.Select IndicatorClassification
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._IndicatorClassification == null)
                {
                    this._IndicatorClassification = new IndicatorClassification.Select(this.TablesName);
                }
                return this._IndicatorClassification;
            }
        }

        private IUS.Select _IUS;
        /// <summary>
        ///  Returns select queries for IUS
        /// </summary>
        public IUS.Select IUS
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._IUS == null)
                {
                    this._IUS = new IUS.Select(this.TablesName);
                }

                return this._IUS;
            }
        }

        private Notes.Select _Notes;
        /// <summary>
        ///  Returns select queries for Notes
        /// </summary>
        public Notes.Select Notes
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Notes == null)
                {
                    this._Notes = new Notes.Select(this.TablesName);
                }
                return this._Notes;
            }
        }

        private Footnote.Select _Footnote;
        /// <summary>
        ///  Returns select queries for Footnote
        /// </summary>
        public Footnote.Select Footnote
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Footnote == null)
                {
                    this._Footnote = new Footnote.Select(this.TablesName);
                }
                return this._Footnote;
            }
        }

        private Source.Select _Source;
        /// <summary>
        ///  Returns select queries for Source
        /// </summary>
        public Source.Select Source
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Source == null)
                {
                    this._Source = new Source.Select(this.TablesName);
                }
                return this._Source;
            }
        }


        private SubgroupVal.Select _SubgroupVals;
        /// <summary>
        ///  Returns select queries for SubgroupVals
        /// </summary>
        public SubgroupVal.Select SubgroupVals
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._SubgroupVals == null)
                {
                    this._SubgroupVals = new SubgroupVal.Select(this.TablesName);
                }
                return this._SubgroupVals;
            }
        }

        private Subgroup.Select _Subgroup;
        /// <summary>
        ///  Returns select queries for Subgroup
        /// </summary>
        public Subgroup.Select Subgroup
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Subgroup == null)
                {
                    this._Subgroup = new Subgroup.Select(this.TablesName);
                }
                return this._Subgroup;
            }
        }

        private Timeperiod.Select _Timeperiod;
        /// <summary>
        ///  Returns select queries for Timeperiod
        /// </summary>
        public Timeperiod.Select Timeperiod
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Timeperiod == null)
                {
                    this._Timeperiod = new Timeperiod.Select(this.TablesName);
                }
                return this._Timeperiod;
            }
        }

        private Unit.Select _Unit;
        /// <summary>
        ///  Returns select queries for Unit
        /// </summary>
        public Unit.Select Unit
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Unit == null)
                {
                    this._Unit = new Unit.Select(this.TablesName);
                }
                return this._Unit;
            }
        }


        private Xslt.Select _Xslt;
        /// <summary>
        ///  Returns select queries for Unit
        /// </summary>
        public Xslt.Select Xslt
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Xslt == null)
                {
                    this._Xslt = new Xslt.Select(this.TablesName);
                }
                return this._Xslt;
            }
        }



        private DBVersion.Select _DBVersion;
        /// <summary>
        ///  Returns select queries for DBVersion
        /// </summary>
        public DBVersion.Select DBVersion
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._DBVersion == null)
                {
                    this._DBVersion = new DBVersion.Select(this.TablesName);
                }
                return this._DBVersion;
            }
        }

        private Calculates.Select _Calculates;
        /// <summary>
        ///  Returns all queries for Calculate module
        /// </summary>
        public Calculates.Select Calculates
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Calculates == null)
                {
                    this._Calculates = new Calculates.Select(this.TablesName);
                }
                return this._Calculates;
            }
        }


        private SubgroupValSubgroup.Select _SubgroupValSubgroup;
        /// <summary>
        ///  Returns select queries for UT_Subgroup_Val_Subgroup
        /// </summary>
        public SubgroupValSubgroup.Select SubgroupValSubgroup
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._SubgroupValSubgroup == null)
                {
                    this._SubgroupValSubgroup = new SubgroupValSubgroup.Select(this.TablesName);
                }
                return this._SubgroupValSubgroup;
            }
        }


        private SubgroupTypes.Select _SubgroupTypes;
        /// <summary>
        ///  Returns select queries for UT_Subgroup_Types_en
        /// </summary>
        public SubgroupTypes.Select SubgroupTypes
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._SubgroupTypes == null)
                {
                    this._SubgroupTypes = new SubgroupTypes.Select(this.TablesName);
                }
                return this._SubgroupTypes;
            }
        }

        private MetadataCategory.Select _Metadata_Category;
        /// <summary>
        ///  Returns select queries for UT_Metadata_Category_en
        /// </summary>
        public MetadataCategory.Select Metadata_Category
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._Metadata_Category == null)
                {
                    this._Metadata_Category = new MetadataCategory.Select(this.TablesName);
                }
                return this._Metadata_Category;
            }
        }

        private DITables _TablesName;
        /// <summary>
        /// Gets tables name
        /// </summary>
        public DITables TablesName
        {
            get
            {
                return this._TablesName;
            }

        }

        private DeleteQueries _Delete;
        /// <summary>
        /// Gets queries to delete records
        /// </summary>
        public DeleteQueries Delete
        {

            get
            {
                if (this._Delete == null)
                {
                    this._Delete = new DeleteQueries(this._TablesName);
                }
                return this._Delete;
            }
        }

        private UpdateQueries _Update;
        /// <summary>
        /// Gets queries to update records
        /// </summary>
        public UpdateQueries Update
        {

            get
            {
                if (this._Update == null)
                {
                    this._Update = new UpdateQueries(this._TablesName);
                }
                return this._Update;
            }
        }

        private TemplateLog.Select _TemplateLog;
        /// <summary>
        ///  Returns select queries for TemplateLog
        /// </summary>
        public TemplateLog.Select TemplateLog
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._TemplateLog == null)
                {
                    this._TemplateLog = new TemplateLog.Select(this.TablesName);
                }
                return this._TemplateLog;
            }
        }

        private DatabaseLog.Select _DatabaseLog;
        /// <summary>
        ///  Returns select queries for DatabaseLog
        /// </summary>
        public DatabaseLog.Select DatabaseLog
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._DatabaseLog == null)
                {
                    this._DatabaseLog = new DatabaseLog.Select(this.TablesName);
                }
                return this._DatabaseLog;
            }
        }


        private DBUsers.Select _DBUser;
        /// <summary>
        /// Returns select queries for Area
        /// </summary>
        public DBUsers.Select DBUser
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._DBUser == null)
                {
                    this._DBUser = new DBUsers.Select(this.TablesName);
                }

                return this._DBUser;
            }
        }

        private DBUsers.Select _DIUser;
        /// <summary>
        /// Returns select queries for Area
        /// </summary>
        public DBUsers.Select DIUser
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._DIUser == null)
                {
                    this._DIUser = new DBUsers.Select(this.TablesName);
                }

                return this._DIUser;
            }
        }
        private DBMetadata.Select _DBMetadata;
        /// <summary>
        /// Returns select queries for DBMetadata
        /// </summary>
        public DBMetadata.Select DBMetadata
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._DBMetadata == null)
                {
                    this._DBMetadata = new DBMetadata.Select(this.TablesName);
                }

                return this._DBMetadata;
            }
        }

        private RecommendedSources.Select _RecommendedSources;
        /// <summary>
        /// Returns select queries for RecommendedSources
        /// </summary>
        public RecommendedSources.Select RecommendedSources
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._RecommendedSources == null)
                {
                    this._RecommendedSources = new RecommendedSources.Select(this.TablesName);
                }

                return this._RecommendedSources;
            }
        }

        private AutoFill.Select _AutoFill;
        /// <summary>
        /// Returns select queries for AutoFill Records (means records where data exists)
        /// </summary>
        public AutoFill.Select AutoFill
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._AutoFill == null)
                {
                    this._AutoFill = new AutoFill.Select(this.TablesName);
                }

                return this._AutoFill;
            }
        }


        private SDMXUser.Select _SDMXUser;
        /// <summary>
        /// Returns select queries for Sender
        /// </summary>
        public SDMXUser.Select SDMXUser
        {
            get
            {
                // CreateObjects object, if it is null
                if (this._SDMXUser == null)
                {
                    this._SDMXUser = new SDMXUser.Select(this.TablesName);
                }

                return this._SDMXUser;
            }
        }

        #endregion

        #region "-- New / Dispose --"

        /// <summary>
        /// Returns instance of DIQueries.
        /// </summary>
        internal DIQueries()
        {
            //dont implements this method
        }

        /// <summary>
        /// Returns instance of DIQueries.
        /// </summary>
        /// <param name="dataPrefix">Data prefix like "UT_"</param>
        /// <param name="languageCode">Language code like "_en"</param>
        public DIQueries(string dataPrefix, string languageCode)
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            this._DataPrefix = dataPrefix.ToLower();
            this._LanguageCode = languageCode.ToLower();

            // Setting DevInfo Tables
            this._TablesName = new DITables(this._DataPrefix, this._LanguageCode);
        }


        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        #endregion

        #region "-- Delete --"

        /// <summary>
        /// Provides queries to delete records .
        /// </summary>
        public class DeleteQueries
        {

            #region "--public/ Internal --"

            #region "-- Variables --"

            private DITables TableNames;

            private Area.Delete _Area;
            /// <summary>
            /// Gets instance of area delete class 
            /// </summary>
            public Area.Delete Area
            {
                get
                {
                    if (this._Area == null)
                    {
                        this._Area = new Queries.Area.Delete(this.TableNames);
                    }

                    return this._Area;
                }
            }


            private IUS.Delete _IUS;
            /// <summary>
            /// Gets instance of IUS delete class 
            /// </summary>
            public IUS.Delete IUS
            {
                get
                {
                    if (this._IUS == null)
                    {
                        this._IUS = new Queries.IUS.Delete(this.TableNames);
                    }

                    return this._IUS;
                }
            }


            private Data.Delete _Data;
            /// <summary>
            /// Gets instance of Data delete class 
            /// </summary>
            public Data.Delete Data
            {
                get
                {
                    if (this._Data == null)
                    {
                        this._Data = new Queries.Data.Delete(this.TableNames);
                    }

                    return this._Data;
                }
            }

            private Timeperiod.Delete _Timeperiod;
            /// <summary>
            /// Gets instance of Timeperiod delete class 
            /// </summary>
            public Timeperiod.Delete Timeperiod
            {
                get
                {
                    if (this._Timeperiod == null)
                    {
                        this._Timeperiod = new Queries.Timeperiod.Delete(this.TableNames);
                    }

                    return this._Timeperiod;
                }
            }

            private Xslt.Delete _Xslt;
            /// <summary>
            /// Gets instance of Xslt delete class 
            /// </summary>
            public Xslt.Delete Xslt
            {
                get
                {
                    if (this._Xslt == null)
                    {
                        this._Xslt = new Queries.Xslt.Delete(this.TableNames);
                    }

                    return this._Xslt;
                }
            }


            private DBVersion.Delete _Version;
            /// <summary>
            /// Gets instance of Version delete class 
            /// </summary>
            public DBVersion.Delete Version
            {
                get
                {
                    if (this._Version == null)
                    {
                        this._Version = new Queries.DBVersion.Delete(this.TableNames);
                    }

                    return this._Version;
                }
            }


            #endregion

            #region "-- New/ Dispose --"

            internal DeleteQueries(DITables tableNames)
            {
                this.TableNames = tableNames;
            }

            #endregion

            #endregion
        }

        #endregion

        #region "-- Update --"

        /// <summary>
        /// Provides queries to update records
        /// </summary>
        public class UpdateQueries
        {

            #region "-- Internal --"

            #region "-- Variables --"

            private DITables TableNames;

            private Area.Update _Area;
            /// <summary>
            /// Gets instance of area delete class 
            /// </summary>
            public Area.Update Area
            {
                get
                {
                    if (this._Area == null)
                    {
                        this._Area = new Queries.Area.Update(this.TableNames);
                    }

                    return this._Area;
                }
            }


            private IUS.Update _IUS;
            /// <summary>
            /// Gets instance of IUS update class 
            /// </summary>
            public IUS.Update IUS
            {
                get
                {
                    if (this._IUS == null)
                    {
                        this._IUS = new Queries.IUS.Update(this.TableNames);
                    }

                    return this._IUS;
                }
            }

            #endregion

            #region "-- New/ Dispose --"

            internal UpdateQueries(DITables tableNames)
            {
                this.TableNames = tableNames;
            }

            #endregion

            #endregion
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Sets the default language code
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="langaugeCode"></param>
        /// <returns></returns>
        public string SetDefaultLanguageCode(string langaugeCode)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.Language + " Set  " + DIColumns.Language.LanguageDefault + "=1 where " + DIColumns.Language.LanguageCode + "='" + DIQueries.RemoveQuotes(langaugeCode) + "'";

            return RetVal;
        }


        #endregion


        #endregion

        #region "-- Static --"

        #region"-- New / Dispose--"

        static DIQueries()
        {
            // add values in ICType collection
            _ICTypeText = new SortedList<ICType, string>();
            _ICTypeText.Add(ICType.CF, "'CF'");
            _ICTypeText.Add(ICType.Convention, "'CN'");
            _ICTypeText.Add(ICType.Goal, "'GL'");
            _ICTypeText.Add(ICType.Institution, "'IT'");
            _ICTypeText.Add(ICType.Sector, "'SC'");
            _ICTypeText.Add(ICType.Source, "'SR'");
            _ICTypeText.Add(ICType.Theme, "'TH'");

            // add values in IconElementType collection
            _IconElementTypeText = new SortedList<IconElementType, string>();
            _IconElementTypeText.Add(IconElementType.Indicator, "'I'");
            _IconElementTypeText.Add(IconElementType.Unit, "'U'");
            _IconElementTypeText.Add(IconElementType.SubgroupVals, "'S'");
            _IconElementTypeText.Add(IconElementType.Area, "'A'");
            _IconElementTypeText.Add(IconElementType.IndicatorClassification, "'C'");
            _IconElementTypeText.Add(IconElementType.Data, "'D'");
            _IconElementTypeText.Add(IconElementType.MetadataIndicator, "'MI'");
            _IconElementTypeText.Add(IconElementType.MetadataArea, "'MA'");
            _IconElementTypeText.Add(IconElementType.MetadataSource, "'MS'");

            // add values in MetadataElementType collection
            _MetadataElementTypeText = new SortedList<MetadataElementType, string>();
            _MetadataElementTypeText.Add(MetadataElementType.Indicator, "'I'");
            _MetadataElementTypeText.Add(MetadataElementType.Area, "'A'");
            _MetadataElementTypeText.Add(MetadataElementType.Source, "'S'");
        }

        #endregion

        #region"-- Properties --"

        /// <summary>
        /// Gets the TextPrefix.
        /// </summary>
        public static string TextPrefix = "#";

        /// <summary>
        /// Gets Metadata Xslt type.
        /// </summary>
        public static class MetadataXsltType
        {
            /// <summary>
            /// IND
            /// </summary>
            public const string Indicator = "IND";

            /// <summary>
            /// MAP
            /// </summary>
            public const string Area = "MAP";

            /// <summary>
            /// SRC
            /// </summary>
            public const string Source = "SRC";
        }

        private static SortedList<ICType, string> _ICTypeText;
        /// <summary>
        /// Get standard abbreviated strings for indicator classification based on ICType. Eg SC,GL,CF,IT,TH,SR,CN
        /// </summary>
        /// <remarks>
        /// <para>These strings are compliant to IC_Type column values of Indicator_Classifications table</para>
        /// <para>"SC" - Sector, "GL" - Goal, "CF" - CF, "IT" - Institution, "TH" - Theme, "SR" - Source, "CN" - Convention</para>
        /// </remarks>
        public static SortedList<ICType, string> ICTypeText
        {
            get
            {
                return _ICTypeText;
            }
        }

        private static SortedList<IconElementType, string> _IconElementTypeText;
        /// <summary>
        /// Get standard abbreviated strings for elments to which icon can be attached, based on IconElementType. Eg I,A,S,U,C,D,MI,MS,MA
        /// </summary>
        /// <remarks>
        /// <para>These strings are compliant to Element_Type column values of Icons table</para>
        /// <para>I-Indicator,A-Area,S-SubgroupVals,U-Unit,C-Classification,D-Data,MI-Metadata Indicator, MS-Metadata Source, MA-Metadata Area</para>
        /// </remarks>
        public static SortedList<IconElementType, string> IconElementTypeText
        {
            get
            {
                return _IconElementTypeText;
            }
        }

        private static SortedList<MetadataElementType, string> _MetadataElementTypeText;
        /// <summary>
        /// Get standard abbreviated strings for elments for which standard xslts are available, based on MetadataElementType. Eg I,A,S
        /// </summary>
        /// <remarks>
        /// <para>These strings are compliant to Element_Type column values of Element_XSLT table</para>
        /// <para>I-Indicator; A-Area; S-Source</para>
        /// </remarks>
        public static SortedList<MetadataElementType, string> MetadataElementTypeText
        {
            get
            {
                return _MetadataElementTypeText;
            }
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns query to get NID of last created record
        /// </summary>
        /// <returns></returns>
        public static string GetNewNID()
        {
            string RetVal = "SELECT @@IDENTITY";

            return RetVal;
        }

        /// <summary>
        /// Get Max value of a field of a table
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="ColName"></param>
        /// <param name="FilterTxt"></param>
        /// <returns></returns>
        public static string GetMaxValue(string tableName, string colName, string filterText)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT MAX(" + colName + ") FROM " + tableName;
            //--Add Condition if FilterText is not null.
            if (!string.IsNullOrEmpty(filterText))
            {
                RetVal += " WHERE " + filterText;
            }

            return RetVal;
        }

        /// <summary>
        /// Return Query to Get Minimum OF any Fields
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ColName"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public static string GetMinValue(string tableName, string ColName, string filterText)
        {
            string RetVal;

            RetVal = "SELECT MIN(" + ColName + ") FROM " + tableName;
            //--Add Condition if FilterText is not null.
            if (!string.IsNullOrEmpty(filterText))
            {
                RetVal += " WHERE " + filterText;
            }

            return RetVal;
        }

        /// <summary>
        /// Get Records Count of Supplied Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public static string GetTableRecordsCount(string tableName, string filterText)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT COUNT(*) FROM " + tableName;
            if (!string.IsNullOrEmpty(filterText))
            {
                RetVal += " WHERE " + filterText;
            }
            return RetVal;
        }

        /// <summary>
        /// Get all the records of the table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetTableRecords(string tableName)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT * FROM " + tableName;
            return RetVal;
        }


        /// <summary>
        /// Retruns query to get all languages
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <returns>string</returns>
        public static string GetLangauges(string dataPrefix)
        {
            return "Select * FROM " + dataPrefix.ToLower() + "Language".ToLower();
        }

        /// <summary>
        /// Retruns query to get language name for the given langauge code
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">like en</param>
        /// <returns>string</returns>
        public static string GetLangaugeName(string dataPrefix, string languageCode)
        {
            languageCode=languageCode.Replace("_", "");

            return "Select * FROM " + dataPrefix + "Language WHERE language_code='" + languageCode + "'";
        }


        /// <summary>
        /// Retruns query to get default language
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <returns></returns>
        public static string GetDefaultLangauge(string dataPrefix)
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return "Select * FROM " + dataPrefix.ToLower() + "Language where language_default <> 0 ".ToLower();
        }

        /// <summary>
        /// Returns query to get all devinfo dataset
        /// </summary>
        /// <returns>string</returns>
        internal static string GetAllDataset()
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return "SELECT * FROM DB_Available_Databases ".ToLower();
        }

        /// <summary>
        /// Returns query to get default dataset.
        /// </summary>
        /// <returns>string</returns>
        internal static string GetDefaultDataset()
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return "SELECT * FROM DB_Available_Databases where AvlDB_Default<> 0".ToLower(); ;
        }

        /// <summary>
        /// Returns query to check dataset exists or not.
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <returns>string</returns>
        internal static string CheckDatasetExists(string dataPrefix)
        {
            dataPrefix = dataPrefix.Replace("_", "");
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return "SELECT count(*) FROM DB_Available_Databases where AvlDB_Prefix='".ToLower() + dataPrefix.ToLower() + "'";
        }

        /// <summary>
        /// Returns query to check language exists or not.
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <param name="languageCode"> language code like en or _en.</param>
        /// <returns>string</returns>
        public static string CheckLanguageExists(string dataPrefix, string languageCode)
        {
            languageCode = languageCode.Replace("_", "");
            return "Select count(*) FROM " + dataPrefix + "Language where language_code='" + languageCode + "'";
        }

        /// <summary>
        /// Returns SQL syntax for concatination based on database type
        /// </summary>
        /// <param name="values">Comma Delimited Values that are to be concatenated</param>
        /// <param name="valuesDelimiter">Delimiter for values send</param>
        /// <param name="concatString">Text used as concatination separator</param>
        /// <param name="DIServerType">Database server type</param>
        /// <returns></returns>
        internal static string SQL_GetConcatenatedValues(string values, string valuesDelimiter, string concatString, DIServerType DIServerType)
        {
            string RetVal = string.Empty;

            string[] arrFields = GetSplittedValues(values, valuesDelimiter);
            for (int i = 0; i < arrFields.Length; i++)
            {
                switch (DIServerType)
                {
                    case DIServerType.SqlServer:
                    case DIServerType.SqlServerExpress:
                    case DIServerType.MsAccess:
                    case DIServerType.Oracle:
                        if (RetVal.Length == 0)
                        {
                            RetVal = SQL_TypeCastToString(arrFields[i], DIServerType);
                        }
                        else
                        {
                            RetVal += " + '" + concatString + "' + " + SQL_TypeCastToString(arrFields[i], DIServerType);
                        }
                        break;
                    case DIServerType.MySql:
                        if (RetVal.Length == 0)
                        {
                            RetVal = "CONCAT(" + SQL_TypeCastToString(arrFields[i], DIServerType);
                        }
                        else if (i == arrFields.Length - 1)
                        {
                            RetVal += ",'" + concatString + "'," + SQL_TypeCastToString(arrFields[i], DIServerType) + ")";
                        }
                        else
                        {
                            RetVal += ",'" + concatString + "'," + SQL_TypeCastToString(arrFields[i], DIServerType);
                        }
                        break;
                    case DIServerType.Excel:
                        break;
                    default:
                        break;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns SQL syntax for casting as string, based on database type
        /// </summary>
        /// <param name="Value">Value to be transformed with cast statement</param>
        /// <param name="DIServerType">Database Type</param>
        /// <returns></returns>
        /// <remarks>Required when field to be concatinated is numeric type (NId etc)</remarks>
        private static string SQL_TypeCastToString(string Value, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            switch (DIServerType)
            {
                case DIServerType.SqlServer:
                case DIServerType.SqlServerExpress:
                    // http://msdn2.microsoft.com/en-us/library/ms187928.aspx
                    // CAST ( expression AS data_type [ (length ) ])
                    // length Is an optional parameter of nchar, nvarchar, char, varchar, binary, or varbinary data types.
                    // if length is not specified, the default to 30 characters. (GIds are greater than 30 length)
                    RetVal = "cast(" + Value + " as varchar (60))";
                    break;
                case DIServerType.MsAccess:
                    RetVal = "cstr(" + Value + ")";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    RetVal = "CAST(" + Value + " AS CHAR)";
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }


            return RetVal;
        }

        /// <summary>
        /// Returns string array after splitting a delimited text based on given delimiter
        /// </summary>
        /// <param name="value">string value to be splitted</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>string[]</returns>
        internal static string[] GetSplittedValues(string value, string delimiter)
        {
            string[] RetVal;
            string[] Arr = new string[1];       //To get splitted values
            Arr[0] = delimiter;
            RetVal = value.Split(Arr, StringSplitOptions.None);
            return RetVal;
        }

        /// <summary>
        /// Returns insert query to insert recrod in langauge table.
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT</param>
        /// <param name="languageName">New langauge name</param>
        /// <param name="languageCode">New language code</param>
        /// <param name="languageDefault">True/False. True to make it default langauge otherwise false.</param>
        /// <param name="languageGlobalLock"></param>
        /// <returns>string </returns>
        public static string InsertLanguage(string dataPrefix, string languageName, string languageCode, bool languageDefault, bool languageGlobalLock)
        {

            string RetVal = string.Empty;

            RetVal = " INSERT INTO " + dataPrefix + "Language ( " + DIColumns.Language.LanguageName + ","
            + DIColumns.Language.LanguageCode + ","
            + DIColumns.Language.LanguageDefault + ","
            + DIColumns.Language.LanguageGlobalLock + ")"
            + " VALUES('" + languageName + "' , '" + languageCode + "'," + languageDefault + " , " + languageGlobalLock + " ) ";

            return RetVal;

        }


        /// <summary>
        /// Removes Quotes From the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveQuotesForSqlQuery(string value)
        {
            string RetVal = string.Empty;
            
            RetVal = value.Replace("''", "'");
            RetVal = RetVal.Replace("’’", "'");

            RetVal = RetVal.Replace("'", "''");
            RetVal = RetVal.Replace("’", "''");
            

            return RetVal;
        }


        /// <summary>
        /// Removes Quotes From the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string RemoveQuotes(string value)
        {
            string RetVal = string.Empty;

            RetVal = value.Replace("'", "''");
            RetVal = RetVal.Replace("’", "''");

            return RetVal;
        }

        /// <summary>
        /// Returns query to insert new database name
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="newDBName">Database filename without path</param>
        /// <returns></returns>
        internal static string InsertNewDBName(string dataPrefix, string newDBName)
        {
            string RetVal = string.Empty;
            //DITables TableNames = new DITables(dataPrefix,string.Empty);

            RetVal = "Update  DB_Available_Databases set " + DIColumns.DBAvailableDatabases.AvlDBName + "='" + DIQueries.RemoveQuotesForSqlQuery(newDBName) + "' where " + DIColumns.DBAvailableDatabases.AvlDBDefault + "=-1";

            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            return RetVal.ToLower();
        }

        /// <summary>
        /// Returns delete query
        /// </summary>
        /// <param name="TableName">From which records has to be deleted</param>
        /// <param name="expressionColumnName">Expression column namelike Indicator NId, indicator GId,.... May be blank to delete all records</param>
        /// <param name="filterString">like Nids, GIds, etc. May be blank to delete all records</param>
        /// <param name="ForNotInClause">Set true/false. True if "NOT IN" clause is required for filtering</param>
        /// <returns></returns>
        public static string DeleteRecords(string TableName, string expressionColumnName, string filterString, bool ForNotInClause)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName;

            if (!string.IsNullOrEmpty(filterString))
            {
                RetVal += " WHERE " + expressionColumnName;
                if (ForNotInClause)
                {
                    RetVal += " NOT IN ";
                }
                else
                {
                    RetVal += " IN ";
                }

                RetVal += " ( " + filterString + " )";
            }

            return RetVal;
        }


        /// <summary>
        /// Returns query to drop table
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string DropTable(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = "DROP TABLE " + tableName;

            return RetVal;
        }

        /// <summary>
        /// Returns query to Drop IndividualColumn Of Table on basis of passing columnname,tableName
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// </summary>
        public static string DropIndividualColumnOfTable(string tableName, string columnName)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER  TABLE " + tableName + " DROP COLUMN " + columnName;
            return RetVal;
        }

        /// <summary>
        /// Returns query to add column into existing table
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// <param name="dataType"></param>
        /// <param name="defaultValue">may be empty</param>
        /// </summary>
        public static string AddColumn(string tableName, string columnName, string dataType, string defaultValue)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tableName + " ADD COLUMN " + columnName + " " + dataType + " ";

            if (!string.IsNullOrEmpty(defaultValue))
            {
                RetVal += " DEFAULT " + defaultValue;
            }

            return RetVal;
        }

        /// <summary>
        /// Deletes language code from UT_Language table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="langaugeCode"></param>
        /// <returns></returns>
        public static string DeleteLanguageCode(string tableName, string langaugeCode)
        {
            string RetVal = string.Empty;

            RetVal = "Delete from  " + tableName + " where " + DIColumns.Language.LanguageCode + "='" + DIQueries.RemoveQuotes( langaugeCode) + "'";
            return RetVal;
        }

        /// <summary>
        /// Returns the database version
        /// </summary>
        /// <returns></returns>
        public static string GetDatabaseVersion()
        {
            // Convert to lower case to handle case sensetive table names for mysql installed over linux
            string RetVal = " SELECT max(Version_Number) FROM DB_Version".ToLower();

            return RetVal;
        }

        #region "-- Create/Drop Index --"

        public static string CreateIndex(string indexName, string indexOnTableName, string columnName)
        {
            string RetVal = string.Empty;

            //CREATE INDEX indexName ON ut_data(iusnid)
            RetVal = "Create index " + indexName + " on " + indexOnTableName + "(" + columnName + ")";

            return RetVal;
        }

        public static string DropIndex(string indexName, string indexOnTableName)
        {
            string RetVal = string.Empty;

            //DROP INDEX indexName ON ut_data
            RetVal = "DROP INDEX " + indexName + " ON " + indexOnTableName;

            return RetVal;
        }

        #endregion

        #endregion

        #endregion

    }

}
