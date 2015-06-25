using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Create Classification Missing IUS Sheet of Summary Report
    /// </summary>
    internal class ClassificationMissingIUSSheetGenerator:  SheetGenerator
    {

       #region "-- private --"
        
        #region "-- Methods --"
       
        /// <summary>
        /// Get Unmatched IUS Classification DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetUnmatchedIUSClassesTable()
        {
            string CurrentIC = string.Empty;
            int Counter = 0;
            DataTable RetVal = this.CreateUnmatchedIUSClassesTable();

            // -- Get TAble of Classification 
            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IndicatorClassification.GetClassificationUnmatchedIUS());
            Table.DefaultView.Sort = IndicatorClassifications.ICType;

            foreach (DataRow row in Table.Rows)
            {
                //-- If ICType Changed then Add ICType Into Record.
                if (CurrentIC != row[IndicatorClassifications.ICType].ToString())
                {
                    DataRow TempRow = null;
                    //-- Insert A Blank Record above the starting of new ICType Name.
                    if (!string.IsNullOrEmpty(CurrentIC)) { TempRow = RetVal.NewRow(); RetVal.Rows.Add(TempRow); }
                    CurrentIC = row[IndicatorClassifications.ICType].ToString();
                    TempRow = RetVal.NewRow();
                    TempRow[IndicatorClassifications.ICName] = base.GetICTypeName(CurrentIC);
                    RetVal.Rows.Add(TempRow);
                    Counter = 0;
                }
                RetVal.ImportRow(row);
                Counter += 1;
                RetVal.Rows[RetVal.Rows.Count - 1][DILanguage.GetLanguageString("SERIAL_NUMBER")] = Counter;
            }
            // -- Rename Columns
            this.RenameUnmatchedIUSClassesTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create Unmatched IUS Classification DataTable 
        /// </summary>
        /// <returns></returns>
        private DataTable CreateUnmatchedIUSClassesTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            //RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            //RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(IndicatorClassifications.ICName);

            return RetVal;
        }

        /// <summary>
        /// Rename Column of Unmatched IUS Classification
        /// </summary>
        /// <param name="table"></param>
        private void RenameUnmatchedIUSClassesTable(ref DataTable table)
        {
            table.Columns[IndicatorClassifications.ICName].ColumnName = this.ColumnHeader[DSRColumnsHeader.CLASSIFICATION_INDICATOR];
        }

        #endregion

        #endregion

       #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        ///  Create Unmatched IUS Classification Sheet
        /// </summary>
        /// <param name="excelFile"></param>
        internal override void GenerateSheet(ref DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile, DILanguage.GetLanguageString("CLASSIFICATION_MISSING_IUS"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("CLASSIFICATION_MISSING_IUS"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get UnmatchedIUSIC Data TAble.
            Table = this.GetUnmatchedIUSClassesTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.ICMissingIUS.UnmatchedIUSICDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.ICMissingIUS.UnmatchedIUSICDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.ICMissingIUS.UnmatchedIUSICDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.ICMissingIUS.UnmatchedIUSICLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.ICMissingIUS.UnmatchedIUSICColIndex, Constants.Sheet.ICMissingIUS.UnmatchedIUSICColIndex, LastRow, Constants.Sheet.ICMissingIUS.UnmatchedIUSICColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.ICMissingIUS.UnmatchedIUSICColIndex, LastRow, Constants.Sheet.ICMissingIUS.UnmatchedIUSICColIndex, true);
           
 
        }

        #endregion

        #endregion

        
    }
}
