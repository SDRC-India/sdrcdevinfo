using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate Sector Sheet of Summary Report
    /// </summary>
    internal class SectorSheetGenerator: SheetGenerator
    {
        
        #region "-- Internal --"
        
        /// <summary>
        /// Create Sector sheet In Summary Report
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = base.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.SECTOR]);
            // -- Generator Sector sheet contents 
            ICSheetGenerator ICSheet = new ICSheetGenerator();
            ICSheet.GenerateICSheet(ref excelFile, sheetNo, ICType.Sector);
        }

        #endregion

    }
}
