using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.QDS
{
    public static class QDSConstants
    {
        public static class FolderName
        {
            public static class Codelists
            {
                public const string IUS = "ius";
                public const string IC = "ic";
                public const string IC_IUS = "ic_ius";
                public const string Area = "area";
                public const string TP = "tp";
                public const string Metadata = "metadata";
                public const string Footnotes = "footnotes";
            }
        }        

        #region "-- Table and its column name --"

        public static class QDSTables
        {
            public static class SplittedList
            {
                public const string TableName = "TmpDI7SplittedList";

                public static class Columns
                {
                    public const string Value = "Value";
                }
            }

            public static class SearchAreas
            {
                public const string TableName = "TmpDI7SearchAreas";

                public static class Columns
                {
                    public const string Id = "Id";
                    public const string AreaNId = "AreaNId";
                    public const string Area = "Area";
                }
            }

            public static class SearchIndicators
            {
                public const string TableName = "TmpDI7SearchIndicators";

                public static class Columns
                {
                    public const string Id = "Id";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string IndicatorName = "IndicatorName";
                    public const string ICName = "ICName";
                }
            }

            public static class TempMRDRecords
            {
                public const string TableName = "TmpDI7MRDRecords";

                public static class Columns
                {
                    public const string IUSNId = "IUSNId";
                    public const string IndicatorNId = "Indicator_NId";
                    public const string UnitNId = "Unit_NId";
                    public const string SubgroupValNId = "Subgroup_Val_nid";
                    public const string AreaNId = "Area_NId";
                    public const string Timeperiod = "Timeperiod";
                    public const string DVCount = "DVCount";
                    public const string DV = "DV";
                }
            }

            public static class DISearchResult
            {
                public const string TableName = "TmpDI7SearchResults";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class ParentTable
            {
                public const string TableName = "TmpDI7ParentTbl";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class NewParentTable
            {
                public const string TableName = "TmpDI7NewParentTbl";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class ChildTable
            {
                public const string TableName = "TmpDI7ChildTbl";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class ChildAreaTable
            {
                public const string TableName = "TmpDI7ChildArea";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class BlockAreaResults
            {
                public const string TableName = "TmpDI7BlockAreaResults";

                public static class Columns
                {
                    public const string NId = "NId";
                    public const string SearchLanguage = "SearchLanguage";
                    public const string IndicatorNId = "IndicatorNId";
                    public const string UnitNId = "UnitNId";
                    public const string AreaNId = "AreaNId";
                    public const string IsAreaNumeric = "IsAreaNumeric";
                    public const string IndicatorName = "IndicatorName";
                    public const string Unit = "Unit";
                    public const string Area = "Area";
                    public const string DefaultSG = "DefaultSG";
                    public const string MRDTP = "MRDTP";
                    public const string MRD = "MRD";
                    public const string AreaCount = "AreaCount";
                    public const string SGCount = "SGCount";
                    public const string SourceCount = "SourceCount";
                    public const string TPCount = "TPCount";
                    public const string DVCount = "DVCount";
                    public const string AreaNIds = "AreaNIds";
                    public const string SGNIds = "SGNIds";
                    public const string SourceNIds = "SourceNIds";
                    public const string TPNIds = "TPNIds";
                    public const string DVNIds = "DVNIds";
                    public const string DVSeries = "DVSeries";
                    public const string Dimensions = "Dimensions";
                    public const string BlockAreaParentNId = "BlockAreaParentNId";
                    public const string IUSNId = "IUSNId";
                    public const string AreaParentNId = "AreaParentNId";
                    public const string IsBlockAreaRecord = "IsBlockAreaRecord";
                }
            }

            public static class Data
            {  
                public static class Columns
                {                    
                    public const string TimePeriod = "Timeperiod";
                    public const string ISDefaultSG = "ISDefaultSG";
                    public const string DVCount = "DVCount";
                }
            }

            public static class Area
            {  
                public static class Columns
                {
                    public const string TempColumn = "TempColumn";                    
                }
            }
        }       

        #endregion
    }
}
