using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Helps in getting database columns name.
/// </summary>
namespace DevInfo.Lib.DI_LibDAL.Queries.DIColumns
{

    /// <summary>
    /// Returns  DBAvailableDatabases Columns 
    /// </summary>
    public static class DBAvailableDatabases
    {
        /// <summary>
        /// AvlDB_NId
        /// </summary>
        public const string AvlDBNId = "AvlDB_NId";

        /// <summary>
        /// AvlDB_Name
        /// </summary>
        public const string AvlDBName = "AvlDB_Name";
        /// <summary>
        /// AvlDB_Prefix
        /// </summary>
        public const string AvlDBPrefix = "AvlDB_Prefix";
        /// <summary>
        /// AvlDB_Default
        /// </summary>
        public const string AvlDBDefault = "AvlDB_Default";
        /// <summary>
        /// Usr_Name
        /// </summary>
        public const string UsrName = "Usr_Name";
        /// <summary>
        /// Usr_Pwd
        /// </summary>
        public const string UsrPwd = "Usr_Pwd";
        /// <summary>
        /// Usr_Protection
        /// </summary>
        public const string UsrProtection = "Usr_Protection";
    }

    /// <summary>
    /// Returns  Language Columns 
    /// </summary>
    public static class Language
    {
        /// <summary>
        /// Language_NId
        /// </summary>
        public const string LanguageNId = "Language_NId";
        /// <summary>
        /// Language_Name
        /// </summary>
        public const string LanguageName = "Language_Name";
        /// <summary>
        /// Language_Code
        /// </summary>
        public const string LanguageCode = "Language_Code";
        /// <summary>
        /// Language_Default
        /// </summary>
        public const string LanguageDefault = "Language_Default";
        /// <summary>
        /// Language_GlobalLock
        /// </summary>
        public const string LanguageGlobalLock = "Language_GlobalLock";
    }

    /// <summary>
    /// Returns Indicator Columns
    /// </summary>
    public static class Indicator
    {
        /// <summary>
        /// Indicator_NId
        /// </summary>
        public const string IndicatorNId = "Indicator_NId";

        /// <summary>
        /// Indicator_Name
        /// </summary>
        public const string IndicatorName = "Indicator_Name";

        /// <summary>
        /// Indicator_GId
        /// </summary>
        public const string IndicatorGId = "Indicator_GId";

        /// <summary>
        /// Indicator_Info
        /// </summary>
        public const string IndicatorInfo = "Indicator_Info";

        /// <summary>
        /// Indicator_Global
        /// </summary>
        public const string IndicatorGlobal = "Indicator_Global";

        /// <summary>
        /// Short_Name
        /// </summary>
        public const string ShortName = "Short_Name";

        /// <summary>
        /// Keywords
        /// </summary>
        public const string Keywords = "Keywords";

        /// <summary>
        /// Indicator_Order
        /// </summary>
        public const string IndicatorOrder = "Indicator_Order";

        /// <summary>
        /// Data_Exist
        /// </summary>
        public const string DataExist = "Data_Exist";

        /// <summary>
        /// HighIsGood
        /// </summary>
        public const string HighIsGood = "HighIsGood";

    }

    /// <summary>
    /// Returns Unit Columns 
    /// </summary>
    public static class Unit
    {
        /// <summary>
        /// Unit_NId
        /// </summary>
        public const string UnitNId = "Unit_NId";

        /// <summary>
        /// Unit_Name
        /// </summary>
        public const string UnitName = "Unit_Name";

        /// <summary>
        /// Unit_GId
        /// </summary>
        public const string UnitGId = "Unit_GId";

        /// <summary>
        /// Unit_Global
        /// </summary>
        public const string UnitGlobal = "Unit_Global";
    }

    /// <summary>
    /// Returns CF_FlowChart columns    
    /// </summary>
    public static class CFFlowChart
    {
        /// <summary>
        /// CF_FlowChart
        /// </summary>
        public const string CF_FlowChart = "CF_FlowChart";
    }

    /// <summary>
    /// Returns User_Access columns    
    /// </summary>
    public static class DBUserAccess
    {
        /// <summary>
        /// User_Access
        /// </summary>
        public const string UserAccessNId = "User_Access_NId";
        public const string UserNId = "User_NId";
        public const string DBPrefix = "DB_Prefix";
        public const string AccessTo = "Access_To";
        public const string LastLogin = "Last_Login";
        public const string LastLogout = "Last_Logout";
        public const string PermissionSource = "Permission_Source";
        public const string PermissionTimePeriod = "Permission_TimePeriod";
        public const string PermissionArea = "Permission_Area";
        public const string PermissionIC = "Permission_IC";
        public const string IsAdmin = "IsAdmin";
        public const string PermissionAreaDescription = "Permission_Area_Des";
        public const string PermissionICDescription = "Permission_IC_Des";
        
    }

    /// <summary>
    /// Returns User_Access columns    
    /// </summary>
    public static class DBUser
    {
        /// <summary>
        /// User_Access
        /// </summary>
        public const string UserNid = "User_NId";
        public const string UserName = "User_Name";
        public const string UserPWD = "User_PWD";
        public const string ISAdmin = "isAdmin";
        public const string ISloggedIn = "isloggedIn";
    }

    /// <summary>
    /// Returns DI_User Columns
    /// </summary>        
    public static class DIUser
    {
        public const string UserNId = "User_NId";
        public const string DBPrefix = "DB_Prefix";
        public const string UserName = "User_Name";
        public const string UserPWD = "User_PWD";
        public const string LastLogin = "Last_Login";
        public const string LastLogout = "Last_Logout";
        public const string PermissionSource = "Permission_Source";
        public const string PermissionTimePeriod = "Permission_TimePeriod";
        public const string PermissionArea = "Permission_Area";
        public const string PermissionIC = "Permission_IC";
        public const string IsAdmin = "IsAdmin";
        public const string AssocitedIC = "Associated_IC";
        public const string AssocitedICWithParent = "Associated_ICW_Parent";
        public const string AssociatedArea = "Associated_Area";
        public const string AssociatedAreaWithParent = "Associated_AreaW_Parent";
        public const string ISloggedIn = "IsLoggedin";

    }

    /// <summary>
    /// Returns Notes Columns 
    /// </summary>
    public static class AgePeriods
    {
        /// <summary>
        /// AgePeriod_NId
        /// </summary>
        public const string AgePeriodNId = "AgePeriod_NId";
        /// <summary>
        /// AgePeriod
        /// </summary>
        public const string AgePeriod = "AgePeriod";
    }

    /// <summary>
    /// Returns SubgroupVals Columns 
    /// </summary>
    public static class SubgroupVals
    {
        /// <summary>
        /// Subgroup_Val_NId
        /// </summary>
        public const string SubgroupValNId = "Subgroup_Val_NId";

        /// <summary>
        /// Subgroup_Val
        /// </summary>
        public const string SubgroupVal = "Subgroup_Val";

        /// <summary>
        /// Subgroup_Val_GId
        /// </summary>
        public const string SubgroupValGId = "Subgroup_Val_GId";

        /// <summary>
        /// Subgroup_Val_Global
        /// </summary>
        public const string SubgroupValGlobal = "Subgroup_Val_Global";

        /// <summary>
        /// Order
        /// </summary>
        public const string SubgroupValOrder = "Subgroup_Val_Order";


        /////// <summary>
        /////// Subgroup_Val_Age. Only used in DX Convert DI v5.0 to DI v6.0
        /////// </summary>
        ////[Obsolete]
        ////public const string SubgroupValAge = "Subgroup_Val_Age";

        /////// <summary>
        /////// Subgroup_Val_Sex. Only used in DX Convert DI v5.0 to DI v6.0
        /////// </summary>
        ////[Obsolete]
        ////public const string SubgroupValSex = "Subgroup_Val_Sex";

        /////// <summary>
        /////// Subgroup_Val_Location. Only used in DX Convert DI v5.0 to DI v6.0
        /////// </summary>
        ////[Obsolete]
        ////public const string SubgroupValLocation = "Subgroup_Val_Location";

        /////// <summary>
        /////// Subgroup_Val_Others. Only used in DX Convert DI v5.0 to DI v6.0
        /////// </summary>
        ////[Obsolete]
        ////public const string SubgroupValOthers = "Subgroup_Val_Others";
    }

    /// <summary>
    /// Returns removed subgroup val columns name
    /// </summary>
    public static class SubgroupValRemovedColumns
    {
        /// <summary>
        /// Subgroup_Val_Age. Only used in DX Convert DI v5.0 to DI v6.0
        /// </summary>
        public const string SubgroupValAge = "Subgroup_Val_Age";

        /// <summary>
        /// Subgroup_Val_Sex. Only used in DX Convert DI v5.0 to DI v6.0
        /// </summary>
        public const string SubgroupValSex = "Subgroup_Val_Sex";

        /// <summary>
        /// Subgroup_Val_Location. Only used in DX Convert DI v5.0 to DI v6.0
        /// </summary>

        public const string SubgroupValLocation = "Subgroup_Val_Location";

        /// <summary>
        /// Subgroup_Val_Others. Only used in DX Convert DI v5.0 to DI v6.0
        /// </summary>
        public const string SubgroupValOthers = "Subgroup_Val_Others";
    }

    /// <summary>
    /// Returns Subgroup Columns
    /// </summary>
    public static class Subgroup
    {
        /// <summary>
        /// Subgroup_NId
        /// </summary>
        public const string SubgroupNId = "Subgroup_NId";
        /// <summary>
        /// "Subgroup_Name
        /// </summary>
        public const string SubgroupName = "Subgroup_Name";
        /// <summary>
        /// "Subgroup_GId
        /// </summary>
        public const string SubgroupGId = "Subgroup_GId";
        /// <summary>
        /// "Subgroup_Global
        /// </summary>
        public const string SubgroupGlobal = "Subgroup_Global";
        /// <summary>
        /// Subgroup_Type
        /// </summary>
        public const string SubgroupType = "Subgroup_Type";

        /// <summary>
        /// Order
        /// </summary>
        public const string SubgroupOrder = "Subgroup_Order";
    }

    /// <summary>
    /// Return Dummy Column names for SubgroupInfo table in DIDataView class
    /// </summary>
    /// <remarks>
    /// No such table SubgroupInfoColumns actually exists in DevInfo database structure
    /// Constants declared for logical field names, for consistent use in DAL BAL and Hosting Applications
    /// Used in Presentationa Data for Table wizard
    /// </remarks>
    public struct SubgroupInfoColumns
    {
        /// <summary>
        /// NId
        /// </summary>
        public const string NId = "NId";

        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        /// GId
        /// </summary>
        public const string GId = "GId";

        /// <summary>
        /// Global
        /// </summary>
        public const string Global = "Global";

        /// <summary>
        /// Type
        /// </summary>
        public const string Type = "Type";

        /// <summary>
        /// Order
        /// </summary>
        public const string Order = "Order";

        /// <summary>
        /// Caption
        /// </summary>
        public const string Caption = "Caption";

        /// <summary>
        /// GIdValue
        /// </summary>
        public const string GIdValue = "GIdValue";

        /// <summary>
        /// OrderColName
        /// </summary>
        public const string OrderColName = "OrderColName";

    }


    /// <summary>
    /// Returns IUS Columns
    /// </summary>
    public static class Indicator_Unit_Subgroup
    {
        /// <summary>
        /// IUSNId
        /// </summary>
        public const string IUSNId = "IUSNId";
        /// <summary>
        /// Indicator_NId
        /// </summary>
        public const string IndicatorNId = "Indicator_NId";
        /// <summary>
        /// Unit_NId
        /// </summary>
        public const string UnitNId = "Unit_NId";
        /// <summary>
        /// Subgroup_Val_NId
        /// </summary>
        public const string SubgroupValNId = "Subgroup_Val_NId";
        /// <summary>
        /// Min_Value
        /// </summary>
        public const string MinValue = "Min_Value";
        /// <summary>
        /// Max_Value
        /// </summary>
        public const string MaxValue = "Max_Value";

        /// <summary>
        /// Subgroup_Nids
        /// </summary>
        public const string SubgroupNids = "Subgroup_Nids";

        /// <summary>
        /// Subgroup_Type_Nids
        /// </summary>
        public const string SubgroupTypeNids = "Subgroup_Type_Nids";

        /// <summary>
        /// Data_Exist
        /// </summary>
        public const string DataExist = "Data_Exist";

        #region "-- DI7 Columns --"

        /// <summary>
        /// IsDefaultSubgroup
        /// </summary>
        public const string IsDefaultSubgroup = "IsDefaultSubgroup";

        /// <summary>
        /// AvlMinDataValue 
        /// </summary>
        public const string AvlMinDataValue = "AvlMinDataValue";

        /// <summary>
        /// AvlMaxDataValue
        /// </summary>
        public const string AvlMaxDataValue = "AvlMaxDataValue";

        /// <summary>
        /// AvlMinTimePeriod
        /// </summary>
        public const string AvlMinTimePeriod = "AvlMinTimePeriod";

        /// <summary>
        /// AvlMaxTimePeriod
        /// </summary>
        public const string AvlMaxTimePeriod = "AvlMaxTimePeriod";

        #endregion

    }

    /// <summary>
    /// Returns IndicatorClassifications Columns 
    /// </summary>
    public static class IndicatorClassifications
    {
        /// <summary>
        /// IC_NId
        /// </summary>
        public const string ICNId = "IC_NId";
        /// <summary>
        /// IC_Parent_NId
        /// </summary>
        public const string ICParent_NId = "IC_Parent_NId";
        /// <summary>
        /// IC_GId
        /// </summary>
        public const string ICGId = "IC_GId";
        /// <summary>
        /// IC_Name
        /// </summary>
        public const string ICName = "IC_Name";
        /// <summary>
        /// IC_Global
        /// </summary>
        public const string ICGlobal = "IC_Global";
        /// <summary>
        /// IC_Info
        /// </summary>
        public const string ICInfo = "IC_Info";

        /// <summary>
        /// "SC" - Sector, "GL" - Goal, "CF" - CF, "IT" - Institution, "TH" - Theme, "SR" - Source, "CN" - Convention
        /// IC_Type
        /// </summary>
        public const string ICType = "IC_Type";

        /// <summary>
        /// Order
        /// </summary>
        public const string ICOrder = "IC_Order";

        /// <summary>
        /// IC_Short_Name
        /// </summary>
        public const string ICShortName = "IC_Short_Name";

        /// <summary>
        /// Publisher
        /// </summary>
        public const string Publisher = "Publisher";

        /// <summary>
        /// Title
        /// </summary>
        public const string Title = "Title";

        /// <summary>
        /// DIYear
        /// </summary>
        public const string DIYear = "DIYear";

        /// <summary>
        /// SourceLink1
        /// </summary>
        public const string SourceLink1 = "SourceLink1";

        /// <summary>
        /// SourceLink2
        /// </summary>
        public const string SourceLink2 = "SourceLink2";

        /// <summary>
        /// ISBN
        /// </summary>
        public const string ISBN = "ISBN";

        /// <summary>
        /// Nature
        /// </summary>
        public const string Nature = "Nature";

    }

    /// <summary>
    /// Returns IndicatorClassificationsIUS Columns 
    /// </summary>
    public static class IndicatorClassificationsIUS
    {
        /// <summary>
        /// IC_IUSNId
        /// </summary>
        public const string ICIUSNId = "IC_IUSNId";
        /// <summary>
        /// IC_NId
        /// </summary>
        public const string ICNId = "IC_NId";
        /// <summary>
        /// IUS_NId
        /// </summary>
        public const string IUSNId = "IUSNId";
        /// <summary>
        /// RecommendedSource
        /// </summary>
        public const string RecommendedSource = "RecommendedSource";
        /// <summary>
        /// IC_IUS_Order
        /// </summary>
        public const string ICIUSOrder = "IC_IUS_Order";
        /// <summary>
        /// IC_IUS_Label
        /// </summary>
        public const string ICIUSLabel = "IC_IUS_Label";
    }

    /// <summary>
    /// Returns Notes Columns 
    /// </summary>
    public static class Timeperiods
    {
        /// <summary>
        /// TimePeriod_NId
        /// </summary>
        public const string TimePeriodNId = "TimePeriod_NId";
        /// <summary>
        /// TimePeriod
        /// </summary>
        public const string TimePeriod = "TimePeriod";

        /// <summary>
        /// StartDate
        /// </summary>
        public const string StartDate = "StartDate";
        /// <summary>
        /// EndDate
        /// </summary>
        public const string EndDate = "EndDate";
        /// <summary>
        /// Perodicity
        /// </summary>
        public const string Periodicity = "Periodicity";
    }

    /// <summary>
    /// Returns Area Columns
    /// </summary>
    public static class Area
    {
        /// <summary>
        /// Area_NId
        /// </summary>
        public const string AreaNId = "Area_NId";
        /// <summary>
        /// Area_Parent_NId
        /// </summary>
        public const string AreaParentNId = "Area_Parent_NId";
        /// <summary>
        /// Area_ID
        /// </summary>
        public const string AreaID = "Area_ID";
        /// <summary>
        /// Area_Name
        /// </summary>
        public const string AreaName = "Area_Name";
        /// <summary>
        /// Area_GId
        /// </summary>
        public const string AreaGId = "Area_GId";
        /// <summary>
        /// Area_Level
        /// </summary>
        public const string AreaLevel = "Area_Level";
        /// <summary>
        /// Area_Map
        /// </summary>
        public const string AreaMap = "Area_Map";
        /// <summary>
        /// Area_Block
        /// </summary>
        public const string AreaBlock = "Area_Block";
        /// <summary>
        /// Area_Global
        /// </summary>
        public const string AreaGlobal = "Area_Global";

        /// <summary>
        /// Data_Exist
        /// </summary>
        public const string DataExist = "Data_Exist";

        /// <summary>
        /// AreaShortName
        /// </summary>
        public const string AreaShortName = "AreaShortName";

    }

    public static class Area_Feature_Type
    {
        /// <summary>
        /// Feature_Type_NId
        /// </summary>
        public const string FeatureTypeNId = "Feature_Type_NId";
        /// <summary>
        /// Feature_Type
        /// </summary>
        public const string FeatureType = "Feature_Type";
    }

    public static class Area_Level
    {
        /// <summary>
        /// Level_NId
        /// </summary>
        public const string LevelNId = "Level_NId";
        /// <summary>
        /// Area_Level
        /// </summary>
        public const string AreaLevel = "Area_Level";
        /// <summary>
        /// Area_Level_Name
        /// </summary>
        public const string AreaLevelName = "Area_Level_Name";
    }

    public static class Area_Map
    {
        /// <summary>
        /// Area_Map_NId
        /// </summary>
        public const string AreaMapNId = "Area_Map_NId";
        /// <summary>
        /// Area_NId
        /// </summary>
        public const string AreaNId = "Area_NId";
        /// <summary>
        /// Feature_Layer
        /// </summary>
        public const string FeatureLayer = "Feature_Layer";
        /// <summary>
        /// Feature_Type_NId
        /// </summary>
        public const string FeatureTypeNId = "Feature_Type_NId";
        /// <summary>
        /// Layer_NId
        /// </summary>
        public const string LayerNId = "Layer_NId";
    }

    public static class Area_Map_Layer
    {
        /// <summary>
        /// Layer_NId
        /// </summary>
        public const string LayerNId = "Layer_NId";
        /// <summary>
        /// Layer_Size
        /// </summary>
        public const string LayerSize = "Layer_Size";
        /// <summary>
        /// Layer_Shp
        /// </summary>
        public const string LayerShp = "Layer_Shp";
        /// <summary>
        /// Layer_Shx
        /// </summary>
        public const string LayerShx = "Layer_Shx";
        /// <summary>
        /// Layer_dbf
        /// </summary>
        public const string Layerdbf = "Layer_dbf";
        /// <summary>
        /// Layer_Type
        /// </summary>
        public const string LayerType = "Layer_Type";
        /// <summary>
        /// MinX
        /// </summary>
        public const string MinX = "MinX";
        /// <summary>
        /// MinY
        /// </summary>
        public const string MinY = "MinY";
        /// <summary>
        /// MaxX
        /// </summary>
        public const string MaxX = "MaxX";
        /// <summary>
        /// MaxY
        /// </summary>
        public const string MaxY = "MaxY";
        /// <summary>
        /// Start_Date
        /// </summary>
        public const string StartDate = "Start_Date";
        /// <summary>
        /// End_Date
        /// </summary>
        public const string EndDate = "End_Date";
        /// <summary>
        /// Metadata_NId
        /// </summary>
        public const string MetadataNId = "Metadata_NId";
        /// <summary>
        /// Update_Timestamp
        /// </summary>
        public const string UpdateTimestamp = "Update_Timestamp";
    }


    public static class Area_Map_Metadata
    {
        /// <summary>
        /// Metadata_NId
        /// </summary>
        public const string MetadataNId = "Metadata_NId";
        /// <summary>
        /// Layer_NId
        /// </summary>
        public const string LayerNId = "Layer_NId";
        /// <summary>
        /// Metadata_Text
        /// </summary>
        public const string MetadataText = "Metadata_Text";
        /// <summary>
        /// Layer_Name
        /// </summary>
        public const string LayerName = "Layer_Name";
    }



    /// <summary>
    /// Returns Data Columns 
    /// </summary>
    public static class Data
    {
        /// <summary>
        /// Data_NId
        /// </summary>
        public const string DataNId = "Data_NId";
        /// <summary>
        /// IUSNId
        /// </summary>
        public const string IUSNId = "IUSNId";
        /// <summary>
        /// TimePeriod_NId
        /// </summary>
        public const string TimePeriodNId = "TimePeriod_NId";
        /// <summary>
        /// Area_NId
        /// </summary>
        public const string AreaNId = "Area_NId";
        /// <summary>
        /// Data_Value
        /// </summary>
        public const string DataValue = "Data_Value";
        /// <summary>
        /// StartDate
        /// </summary>
        public const string StartDate = "Start_Date";
        /// <summary>
        /// End_Date
        /// </summary>
        public const string EndDate = "End_Date";
        /// <summary>
        /// Data_Denominator
        /// </summary>
        public const string DataDenominator = "Data_Denominator";
        /// <summary>
        /// FootNote_NId
        /// </summary>        
        public const string FootNoteNId = "FootNote_NId";
        /// <summary>
        /// Source_NId
        /// </summary>
        public const string SourceNId = "Source_NId";
        /// <summary>
        /// Indicator_NId
        /// </summary>
        public const string IndicatorNId = "Indicator_NId";
        /// <summary>
        /// Unit_NId
        /// </summary>
        public const string UnitNId = "Unit_NId";
        /// <summary>
        /// Subgroup_Val_NId
        /// </summary>
        public const string SubgroupValNId = "Subgroup_Val_NId";

        /// <summary>
        /// IC_IUS_Order for recommended source
        /// </summary>
        public const string ICIUSOrder = "IC_IUS_Order";

        #region "-- DI7 Columns --"

        /// <summary>
        /// Textual_Data_Value for recommended source
        /// </summary>
        public const string TextualDataValue = "Textual_Data_Value";

        /// <summary>
        /// IsTextualData
        /// </summary>
        public const string IsTextualData = "IsTextualData";

        /// <summary>
        /// IsMRD
        /// </summary>
        public const string IsMRD = "IsMRD";

        /// <summary>
        /// IsPlannedValue
        /// </summary>
        public const string IsPlannedValue = "IsPlannedValue";

        /// <summary>
        /// IUNId
        /// </summary>
        public const string IUNId = "IUNId";

        /// <summary>
        /// ConfidenceIntervalUpper
        /// </summary>
        public const string ConfidenceIntervalUpper = "ConfidenceIntervalUpper";

        /// <summary>
        /// ConfidenceIntervalLower
        /// </summary>
        public const string ConfidenceIntervalLower = "ConfidenceIntervalLower";

        /// <summary>
        /// MultipleSource
        /// </summary>
        public const string MultipleSource = "MultipleSource";

        #endregion
    }

    /// <summary>
    /// Describes dummy expression column names for data table
    /// </summary>
    /// <remarks>
    /// No such table DataExpression actually exists in DevInfo database structure
    /// Constants declared for logical expression field names, for consistent use in DAL BAL and Hosting
    /// Used for numeric sorting of textual DataValue
    /// </remarks>
    public struct DataExpressionColumns
    {
        /// <summary>
        /// Selected
        /// </summary>
        public const string Selected = "Selected";
        /// <summary>
        /// DataType
        /// </summary>
        public const string DataType = "DataType";
        /// <summary>
        /// NumericData
        /// </summary>
        public const string NumericData = "NumericData";
        /// <summary>
        /// TextualData
        /// </summary>
        public const string TextualData = "TextualData";
        /// <summary>
        /// TimePeriodStartDate
        /// </summary>
        public const string TimePeriodStartDate = "TimePeriodStartDate";
        /// <summary>
        /// TimePeriodEndDate
        /// </summary>
        public const string TimePeriodEndDate = "TimePeriodEndDate";
    }

    public struct StatsExpressionColumns
    {
        /// <summary>
        /// Count
        /// </summary>
        public const string Count = "Count";

        /// <summary>
        /// Minimum
        /// </summary>
        public const string Minimum = "Minimum";

        /// <summary>
        /// Maximum
        /// </summary>
        public const string Maximum = "Maximum";

        /// <summary>
        /// Sum
        /// </summary>
        public const string Sum = "Sum";

        /// <summary>
        /// Mean. Arithmatic Mean OR Average
        /// </summary>
        public const string Mean = "Mean";      //

        /// <summary>
        /// StandardDeviation
        /// </summary>
        public const string StandardDeviation = "StandardDeviation";

        /// <summary>
        /// Variance
        /// </summary>
        public const string Variance = "Variance";

        /// <summary>
        /// Histogram
        /// </summary>
        public const string Histogram = "Histogram";
    }

    /// <summary>
    /// Returns Footnote columns
    /// </summary>
    public static class FootNotes
    {
        /// <summary>
        /// Returns Footnote NId column
        /// </summary>
        public const string FootNoteNId = "FootNote_NId";

        /// <summary>
        /// Returns Footnote column
        /// </summary>
        public const string FootNote = "FootNote";

        /// <summary>
        /// Returns Footnote GId column
        /// </summary>
        public const string FootNoteGId = "FootNote_GId";

    }

    /// <summary>
    /// Returns Assistant_eBook Columns 
    /// </summary>
    public static class Assistant_eBook
    {
        /// <summary>
        /// Ebook_NId
        /// </summary>
        public const string EbookNId = "Ebook_NId";         //PK
        /// <summary>
        /// EBook
        /// </summary>
        public const string EBook = "EBook";
    }

    /// <summary>
    /// Returns Assistant_Topic Columns 
    /// </summary>
    public static class Assistant_Topic
    {
        /// <summary>
        /// Topic_NId
        /// </summary>
        public const string TopicNId = "Topic_NId";         //PK
        /// <summary>
        /// Topic_Name
        /// </summary>
        public const string TopicName = "Topic_Name";
        /// <summary>
        /// Indicator_GId
        /// </summary>
        public const string IndicatorGId = "Indicator_GId";
        /// <summary>
        /// Topic_Intro
        /// </summary>
        public const string TopicIntro = "Topic_Intro";
    }

    /// <summary>
    /// Returns Assistant Columns 
    /// </summary>
    public static class Assistant
    {
        /// <summary>
        /// Assistant_NId
        /// </summary>
        public const string AssistantNId = "Assistant_NId"; //PK
        /// <summary>
        /// Topic_NId
        /// </summary>
        public const string TopicNId = "Topic_NId";         //FK
        /// <summary>
        /// Assistant
        /// </summary>
        public const string AssistantText = "Assistant";

        /// <summary>
        /// Assistant_Type
        /// Text(3) I - Indicator, A - Area, T - Time, DV - Data View, DU - Data View Unit, DS - Data View Source, DG - Data View SubGroup, TW1 - Table Wizard Step 1..., GW1 - Graph Wizard Step 1..., MW1- Map Wizard Step 1
        /// </summary>
        public const string AssistantType = "Assistant_Type";

        /// <summary>
        /// Assistant_Order
        /// Number(Long Integer)
        /// </summary>
        public const string AssistantOrder = "Assistant_Order";
    }

    /// <summary>
    /// Returns Notes_Classification Columns 
    /// </summary>
    public static class Notes_Classification
    {
        /// <summary>
        /// Classification_NId
        /// </summary>
        public const string ClassificationNId = "Classification_NId";   //PK
        /// <summary>
        /// Classification_Name
        /// </summary>
        public const string ClassificationName = "Classification_Name";
    }

    /// <summary>
    /// Returns Notes_Profile Columns 
    /// </summary>
    public static class Notes_Profile
    {
        /// <summary>
        /// Profile_NId
        /// </summary>
        public const string ProfileNId = "Profile_NId";                 //PK
        /// <summary>
        /// Profile_Name
        /// </summary>
        public const string ProfileName = "Profile_Name";
        /// <summary>
        /// Profile_EMail
        /// </summary>
        public const string ProfileEMail = "Profile_EMail";
        /// <summary>
        /// Profile_Country
        /// </summary>
        public const string ProfileCountry = "Profile_Country";
        /// <summary>
        /// Profile_Org
        /// </summary>
        public const string ProfileOrg = "Profile_Org";
        /// <summary>
        /// Profile_Org_Type
        /// </summary>
        public const string ProfileOrgType = "Profile_Org_Type";
    }

    /// <summary>
    /// Returns Notes Columns 
    /// </summary>
    public static class Notes
    {
        /// <summary>
        /// Notes_NId
        /// </summary>
        public const string NotesNId = "Notes_NId";                   //PK
        /// <summary>
        /// Profile_NId
        /// </summary>
        public const string ProfileNId = "Profile_NId";               //FK
        /// <summary>
        /// Classification_NId
        /// </summary>
        public const string ClassificationNId = "Classification_NId"; //FK
        /// <summary>
        /// Notes
        /// </summary>
        public const string Note = "Notes";
        /// <summary>
        /// Notes_DateTime
        /// </summary>
        public const string NotesDateTime = "Notes_DateTime";
        /// <summary>
        /// Notes_Approved
        /// </summary>
        public const string NotesApproved = "Notes_Approved";
    }

    /// <summary>
    /// Returns Notes_Classification Columns 
    /// </summary>
    public static class Notes_Data
    {
        /// <summary>
        /// Notes_Data_NId
        /// </summary>
        public const string NotesDataNId = "Notes_Data_NId";          //PK
        /// <summary>
        /// Notes_NId
        /// </summary>
        public const string NotesNId = "Notes_NId";                   //FK
        /// <summary>
        /// Data_NId
        /// </summary>
        public const string DataNId = "Data_NId";                     //FK
    }

    /// <summary>
    /// Returns Icons Columns 
    /// </summary>
    public static class Icons
    {
        /// <summary>
        /// Icon_NId
        /// </summary>
        public const string IconNId = "Icon_NId";
        /// <summary>
        /// Icon_Type
        /// </summary>
        public const string IconType = "Icon_Type";
        /// <summary>
        /// Icon_Dim_W
        /// </summary>
        public const string IconDimW = "Icon_Dim_W";
        /// <summary>
        /// Icon_Dim_H
        /// </summary>
        public const string IconDimH = "Icon_Dim_H";
        /// <summary>
        /// Element_Type
        /// </summary>
        public const string ElementType = "Element_Type";
        /// <summary>
        /// Element_NId
        /// </summary>
        public const string ElementNId = "Element_NId";
        /// <summary>
        /// Element_Icon
        /// </summary>
        public const string ElementIcon = "Element_Icon";
    }

    /// <summary>
    /// Returns XSLT Columns 
    /// </summary>
    public static class XSLT
    {
        /// <summary>
        /// XSLT_NId
        /// </summary>
        public const string XSLTNId = "XSLT_NId";
        /// <summary>
        /// XSLT_Text
        /// </summary>
        public const string XSLTText = "XSLT_Text";
        /// <summary>
        /// XSLT_File
        /// </summary>
        public const string XSLTFile = "XSLT_File";
    }

    /// <summary>
    /// Returns XSLT Columns 
    /// </summary>
    public static class EelementXSLT
    {
        /// <summary>
        /// Element_XSLT_NId
        /// </summary>
        public const string ElementXSLTNId = "Element_XSLT_NId";
        /// <summary>
        /// Element_NId
        /// </summary>
        public const string ElementNId = "Element_NId";

        /// <summary>
        /// Element_Type
        /// Text(50) I-Indicator; A-Area; S-Source
        /// </summary>
        public const string ElementType = "Element_Type";

        /// <summary>
        /// XSLT_NId
        /// </summary>
        public const string XSLTNId = "XSLT_NId";
    }

    /// <summary>
    /// Returns DB_Version Columns 
    /// </summary>
    public static class DBVersion
    {
        /// <summary>
        /// Version_NId
        /// </summary>
        public const string VersionNId = "Version_NId";
        /// <summary>
        /// Version_Number
        /// </summary>
        public const string VersionNumber = "Version_Number";
        /// <summary>
        /// Version_Change_Date
        /// </summary>
        public const string VersionChangeDate = "Version_Change_Date";
        /// <summary>
        /// Version_Comments
        /// </summary>
        public const string VersionComments = "Version_Comments";
    }


    /// <summary>
    /// Returns UT_Subgroup_Vals_Subgroup Columns 
    /// </summary>
    public static class SubgroupValsSubgroup
    {
        /// <summary>
        /// Subgroup_Val_Subgroup_NId
        /// </summary>
        public const string SubgroupValSubgroupNId = "Subgroup_Val_Subgroup_NId";
        /// <summary>
        /// Subgroup_Val_NId
        /// </summary>
        public const string SubgroupValNId = "Subgroup_Val_NId";
        /// <summary>
        /// Subgroup_NId
        /// </summary>
        public const string SubgroupNId = "Subgroup_NId";
    }

    /// <summary>
    /// Returns UT_RecommendedSources_en Columns 
    /// </summary>
    public static class RecommendedSources
    {
        /// <summary>
        /// RSRC_NId
        /// </summary>
        public const string NId = "RSRC_NId";
        /// <summary>
        /// Data_NId
        /// </summary>
        public const string DataNId = "Data_NId";

        /// <summary>
        /// IC_IUS_Label
        /// </summary>
        public const string ICIUSLabel = "IC_IUS_Label";
    }

    /// <summary>
    /// Returns UT_Subgroup_Type_en Columns 
    /// </summary>
    public static class SubgroupTypes
    {
        /// <summary>
        /// Subgroup_Type_NId
        /// </summary>
        public const string SubgroupTypeNId = "Subgroup_Type_NId";
        /// <summary>
        /// Subgroup_Type_Name
        /// </summary>
        public const string SubgroupTypeName = "Subgroup_Type_Name";
        /// <summary>
        /// Subgroup_Type_GID
        /// </summary>
        public const string SubgroupTypeGID = "Subgroup_Type_GID";
        /// <summary>
        /// Subgroup_Type_Order
        /// </summary>
        public const string SubgroupTypeOrder = "Subgroup_Type_Order";
        /// <summary>
        /// Subgroup_Type_Global
        /// </summary>
        public const string SubgroupTypeGlobal = "Subgroup_Type_Global";
    }

    /// <summary>
    /// Returns Database Log table's column name
    /// </summary>
    public static class DatabaseLog
    {
        /// <summary>
        /// DB_NId
        /// </summary>
        public const string DBNId = "DB_NId";
        /// <summary>
        /// DB_TimeStamp
        /// </summary>
        public const string DBTimeStamp = "DB_TimeStamp";
        /// <summary>
        /// DB_Name
        /// </summary>
        public const string DBName = "DB_Name";
        /// <summary>
        /// DB_Action
        /// </summary>
        public const string DBAction = "DB_Action";
        /// <summary>
        /// DB_User
        /// </summary>
        public const string DBUser = "DB_User";
    }

    /// <summary>
    /// Returns Template Log table's column name
    /// </summary>
    public static class TemplateLog
    {
        /// <summary>
        /// TPL_NId
        /// </summary>
        public const string TPLNId = "TPL_NId";
        /// <summary>
        /// TPL_TimeStamp
        /// </summary>
        public const string TPLTimeStamp = "TPL_TimeStamp";
        /// <summary>
        /// TPL_Name
        /// </summary>
        public const string TPLName = "TPL_Name";
        /// <summary>
        /// TPL_Action
        /// </summary>
        public const string TPLAction = "TPL_Action";
        /// <summary>
        /// TPL_User
        /// </summary>
        public const string TPLUser = "TPL_User";
    }

    /// <summary>
    /// Returns DBMetadata table's columns name
    /// </summary>
    public static class DBMetaData
    {
        /// <summary>
        /// DBMtd_NId
        /// </summary>
        public const string NID = "DBMtd_NId";

        /// <summary>
        /// DBMtd_Desc
        /// </summary>
        public const string Description = "DBMtd_Desc";

        /// <summary>
        /// DBMtd_PubName
        /// </summary>
        public const string PublisherName = "DBMtd_PubName";

        /// <summary>
        /// DBMtd_PubDate
        /// </summary>
        public const string PublisherDate = "DBMtd_PubDate";

        /// <summary>
        /// DBMtd_PubCountry
        /// </summary>
        public const string PublisherCountry = "DBMtd_PubCountry";

        /// <summary>
        /// DBMtd_PubRegion
        /// </summary>
        public const string PublisherRegion = "DBMtd_PubRegion";

        /// <summary>
        /// DBMtd_PubOffice
        /// </summary>
        public const string PublisherOffice = "DBMtd_PubOffice";

        /// <summary>
        /// DBMtd_AreaCnt
        /// </summary>
        public const string AreaCount = "DBMtd_AreaCnt";

        /// <summary>
        /// DBMtd_IndCnt
        /// </summary>
        public const string IndicatorCount = "DBMtd_IndCnt";

        /// <summary>
        /// DBMtd_IUSCnt
        /// </summary>
        public const string IUSCount = "DBMtd_IUSCnt";

        /// <summary>
        /// DBMtd_TimeCnt
        /// </summary>
        public const string TimeperiodCount = "DBMtd_TimeCnt";

        /// <summary>
        /// DBMtd_SrcCnt
        /// </summary>
        public const string SourceCount = "DBMtd_SrcCnt";

        /// <summary>
        /// DBMtd_DataCnt
        /// </summary>
        public const string DataCount = "DBMtd_DataCnt";
    }

    /// <summary>
    /// Return Metadata_Category table's columns name
    /// </summary>
    public static class Metadata_Category
    {
        /// <summary>
        /// CategoryNId
        /// </summary>
        public const string CategoryNId = "CategoryNId";

        /// <summary>
        /// CategoryName
        /// </summary>
        public const string CategoryName = "CategoryName";
        /// <summary>
        /// CategoryType
        /// </summary>
        public const string CategoryType = "CategoryType";

        /// <summary>
        /// CategoryOrder
        /// </summary>
        public const string CategoryOrder = "CategoryOrder";

        #region "-- DI7 Columns --"

        /// <summary>
        /// ParentCategoryNId
        /// </summary>
        public const string ParentCategoryNId = "ParentCategoryNId";

        /// <summary>
        /// CategoryGId
        /// </summary>
        public const string CategoryGId = "CategoryGId";

        /// <summary>
        /// CategoryDescription
        /// </summary>
        public const string CategoryDescription = "CategoryDescription";

        /// <summary>
        /// IsPresentational
        /// </summary>
        public const string IsPresentational = "IsPresentational";

        /// <summary>
        /// IsMandatory
        /// </summary>
        public const string IsMandatory = "IsMandatory";

        #endregion
    }



    /// <summary>
    /// Return Document table's columns name
    /// </summary>
    public static class DIDocument
    {
        /// <summary>
        /// Document_Nid
        /// </summary>
        public const string DocumentNid = "Document_Nid";

        /// <summary>
        /// Document_Type
        /// </summary>
        public const string DocumentType = "Document_Type";

        /// <summary>
        /// Element_Type
        /// </summary>
        public const string ElementType = "Element_Type";

        /// <summary>
        /// Element_Nid
        /// </summary>
        public const string ElementNid = "Element_Nid";

        /// <summary>
        /// Element_Document
        /// </summary>
        public const string ElementDocument = "Element_Document";
    }

    /// <summary>
    /// Return MetadataReport table's columns name
    /// </summary>
    public static class MetadataReport
    {
        /// <summary>
        /// MetadataReport_Nid
        /// </summary>
        public const string MetadataReportNid = "MetadataReport_Nid";

        /// <summary>
        /// Target_Nid
        /// </summary>
        public const string TargetNid = "Target_Nid";
        /// <summary>
        /// Category_Nid
        /// </summary>
        public const string CategoryNid = "Category_Nid";

        /// <summary>
        /// Metadata
        /// </summary>
        public const string Metadata = "Metadata";

    }

    /// <summary>
    /// Get Header Table Column names
    /// </summary>
    public static class SDMXUser
    {
        /// <summary>
        /// Sender_NId
        /// </summary>
        public const string SenderNId = "Sender_NId";
        /// <summary>
        /// IsSender
        /// </summary>
        public const string IsSender = "IsSender";
        /// <summary>
        /// ID
        /// </summary>
        public const string ID = "ID";

        /// <summary>
        /// Name
        /// </summary>
        public const string Name = "Name";
        /// <summary>
        /// ContactName
        /// </summary>
        public const string ContactName = "ContactName";
        /// <summary>
        /// Department
        /// </summary>
        public const string Department = "Department";
        /// <summary>
        /// Email
        /// </summary>
        public const string Email = "Email";
        /// <summary>
        /// Role
        /// </summary>
        public const string Role = "Role";
        /// <summary>
        /// Telephone
        /// </summary>
        public const string Telephone = "Telephone";
        /// <summary>
        /// Fax
        /// </summary>
        public const string Fax = "Fax";

    }


}
