using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Defines constants
    /// </summary>
    public  static class Constants
    {
        /// <summary>
        /// Define constants for language strings
        /// </summary>
        public  static class LanguageKeys
        {

            public  const string IUS = "IUS";
            public  const string Import = "Import";
            public  const string Indicator="Indicator";
            public  const string Unit = "Unit";
            public  const string Subgroup = "Subgroup";
            public  const string SubgroupDimension = "SUBGROUP_DIMENSION";
            public  const string SubgroupType = "SUBGROUP_TYPE";
            public  const string SubgroupVal = "SUBGROUP_VALS";
            public  const string SubgroupSex = "Sex";
            public  const string SubgroupAge = "Age";
            public  const string Subgroupothers = "Others";
            public  const string SubgroupLocation = "Location";
            public  const string RetrieveUnitSubpop = "RETRIEVEUNITSUBPOP";
            public  const string IndicatorClassification = "IndicatorClassification"; //CLASSIFICATION_INDICATOR
            public  const string AreaName = "AreaName";
            public const string Area = "Area";
            public const string Map = "Map";
            public  const string AreaID = "AreaID";
            public  const string AreaLevel = "AreaLevel";
            public  const string Level = "Level";
            public  const string Timeperiod = "Timeperiod";
            public  const string Source = "Source";
            public  const string Sector = "Sector";
            public  const string Goal = "Goal";
            public  const string Framework = "Framework";
            public  const string Convention = "Convention";
            public  const string Institution = "Institution";
            public  const string Theme = "Theme";
            public  const string Layer = "Layer";
            public  const string Type = "Type";
            public const string Footnote = "Footnote";

            #region "-- For Selection list --"
            
            public  const string Available = "AVAILABLE";
            public  const string Selected = "SELECTED";
            public  const string Files = "FILES";
            public  const string RemoveAll = "REMOVEALL";
            public  const string RemoveHighLighted = "REMOVEHIGHLIGHTED";
            public  const string SelectALL = "SELECTALL";
            public  const string SelectHighLighted = "SELECTHIGHLIGHTED";

            #endregion

            public  const string Select = "SELECT";
            public  const string OK = "OK";
            public  const string Cancel = "CANCEL";
            public  const string Browse = "BROWSE";
            public  const string Apply = "Apply";
            public  const string DES = "DEVINFO_DATA_ENTRY_SPREADSHEET";
            public  const string XML = "XML";
            public  const string RTF = "RTF";
            public  const string Search= "SEARCH";
            public  const string UseRegistry = "USEREGISTRY";
            public const string UseMapServer = "USEMAPSERVER";
            public const string DigitalMapServer = "DIGITAL_MAP_SERVER";
            public const string UseRegistryUpdateRequired = "USEREGISTRY_UPDATEREQUIRED";
            public const string Invalid_Url = "INVALID_URL";
            
            //to make sdmx connect intutive
            public const string CONNECTING_TO_SDMX_REGISTRY = "CONNECTING_TO_SDMX_REGISTRY";
            public const string FETCHING_RECORDS_FROM_REGISTRY = "FETCHING_RECORDS_FROM_REGISTRY";
            public const string CREATING_LOCAL_FILE_FOR_FUTURE_REFERENCE = "CREATING_LOCAL_FILE_FOR_FUTURE_REFERENCE";
            public const string FETCHING_METADATA_FROM_REGISTRY = "FETCHING_METADATA_FROM_REGISTRY";
            public const string DISPLAYING_DEVINFO_ELEMENTS = "DISPLAYING_DEVINFO_ELEMENTS";
            public const string UNABLE_TO_GET_REGISTRY = "UNABLE_TO_GET_REGISTRY";
            public const string PLEASE_ENTER_VALID_SDMX_URL = "PLEASE_ENTER_VALID_SDMX_URL";
        }
        
        public const string TempDBFileNamePrefix = "TempLookIn_";
        public  const string FileNameDelimiter = "{[~]}";
    }
}
