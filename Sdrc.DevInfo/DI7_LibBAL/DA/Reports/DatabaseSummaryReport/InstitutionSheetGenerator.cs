using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate IC Institution Sheet of Summary Report
    /// </summary>
    internal class InstitutionSheetGenerator: SheetGenerator 
    {
        #region "-- Internal --"

        /// <summary>
        /// Create IC Institution Excelsheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = base.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.INSTITUTION]);
            // -- Generate Institution sheet contents 
            ICSheetGenerator ICSheet = new ICSheetGenerator();
            ICSheet.GenerateICSheet(ref excelFile, sheetNo, ICType.Institution);

        }

        #endregion
    }
}
