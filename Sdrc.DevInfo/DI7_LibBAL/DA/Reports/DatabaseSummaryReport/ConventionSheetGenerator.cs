using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate IC Convention  Sheet of Summary Report
    /// </summary>
    internal class ConventionSheetGenerator:SheetGenerator
    {
        #region "-- Internal --"

        /// <summary>
        /// Create IC Convention excelFile
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override  void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = base.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.CONVENTION]);
            // -- Generate Convention sheet 
            ICSheetGenerator ICSheet = new ICSheetGenerator();
            ICSheet.GenerateICSheet(ref excelFile, sheetNo, ICType.Convention);
        }

        #endregion
    }
}
