using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    public enum DIExportOutputType
    {
        Spreadsheet = 1,
        XML=2,
        PDF = 3,
        HTML = 4,
        CSV = 5,
        /// <summary>
        /// DevInfo Entry SpreadSheet format.
        /// </summary>
        DES = 6,
        MDB = 7
        
    }

    internal enum WorkbookType
    {
        DataEntrySpreadsheet = 0,
        IndicatorEntrySpreadsheet = 1,
        AreaEntrySpreadsheet = 2,
        General = 3
    }
}
