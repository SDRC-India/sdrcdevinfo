using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.DES
{
    /// <summary>
    /// Static class having static fields representing fixed Cell address in DevInfo DES.
    /// </summary>
    internal static class DESCellAddress
    {
        internal const string IndicatorName = "B5";
        internal const string UnitName = "B7";
        internal const string IndicatorGID = "L5";
        internal const string UnitGID = "L7";
        internal const string Decimals = "H7";
        internal const string Sector = "B3";
        internal const string Class = "B4";
        internal const string DataStartingCell = "A11";     // Cell address where DES data rows starts.

        #region "-- Labels --"
        // Below variables represnts cell address for labels present in standard DES.
        internal const string DESLable = "A1";
        internal const string SectorLable = "A3";
        internal const string ClassLable = "A4";
        internal const string IndicatorLable = "A5";
        internal const string UnitLable = "A7";
        internal const string TimeLable = "A9";
        internal const string AreaIDLable = "B9";
        internal const string AreaNameLable = "C9";
        internal const string DataValueLable = "D9";
        internal const string SubgroupLable = "E9";
        internal const string SourceLable = "F9";
        internal const string FootnoteLable = "G9";
        internal const string DenominatorLable = "H9";
        internal const string DecimalLable = "G7";


        #endregion
    }
}
