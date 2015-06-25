using System;
using System.Collections.Generic;
using System.Text;


namespace DevInfo.Lib.DI_LibBAL.QDS.Utility
{
    public static class QdsResultSchema
    {
        public static class Columns
        {
            public const string Nid = "Nid";
            public const string SearchLanguage = "SearchLanguage";
            public const string IndicatorNid = "IndicatorNid";
            public const string UnitNid = "UnitNid";
            public const string AreaNid = "AreaNid";
            public const string IsAreaNumeric = "IsAreaNumeric";
            public const string Indicator = "IndicatorName";
            public const string Unit = "Unit";
            public const string Area = "Area";
            public const string DefaultSg = "DefaultSG";
            public const string MrdTp = "MRDTP";
            public const string Mrd = "MRD";
            public const string AreaCount = "AreaCount";
            public const string SGCount = "SGCount";
            public const string SourceCount = "SourceCount";
            public const string TpCount = "TPCount";
            public const string DvCount = "DVCount";
            public const string AreaNids = "AreaNids";
            public const string SgNids = "SGNids";
            public const string TpNids = "TPNids";
            public const string DvSeries = "DVSeries";
            public const string Dimensions = "Dimensions";
            public const string BlockAreaParentNid = "AreaParentNid";
            public const string IndicatorDesc = "IndicatorDescription";
            public const string IcName = "IC_Name";
            public const string IUSNId = "IUSNId";
            public const string PaddedArea = "PaddedArea";
            public const string PaddedDVSeries = "PaddedDVSeries";
        }
    }

    public static class DimensionSchema
    {
        public const string SgValNid = "Subgroup_Val_NId";
        public const string DimensionVal = "DimensionValue";
        public const string Dimension = "Dimension";
        public const string DimensionNid = "DimensionNId";
    }
}
