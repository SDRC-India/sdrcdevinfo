using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate Sources Without Data Sheet of Summary Report
    /// </summary>
    internal class SourcesWithoutDataSheetGenerator: SheetGenerator 
    {

        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get DataTable for IUS Missing DataValue 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetSourcesWithoutDataTable()
        {
            DataTable RetVal = this.CreateSourcesWithoutDataTable();
            // -- Get Surces witout DataValues Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Source.GetSourcesWithoutData()).DefaultView;
            
            // -- Sort by Name
            Table.Sort = IndicatorClassifications.ICName + " Asc";

            foreach (DataRowView RowView in Table)
            {
                RetVal.ImportRow(RowView.Row);
            }

            RetVal.Columns[IndicatorClassifications.ICName].ColumnName = this.ColumnHeader[DSRColumnsHeader.SOURCE];

            return RetVal;
        }

        /// <summary>
        /// Get Sources Without Data TAble
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateSourcesWithoutDataTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(IndicatorClassifications.ICName);

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"
        
        /// <summary>
        /// Create Sources without DataValue Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile, DILanguage.GetLanguageString("SOURCES_WITHOUT_DATA"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("SOURCES_WITHOUT_DATA"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get TimePeriod Data TAble.
            Table = this.GetSourcesWithoutDataTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, true);
          
            
        }

        #endregion

        #endregion

    }
}
