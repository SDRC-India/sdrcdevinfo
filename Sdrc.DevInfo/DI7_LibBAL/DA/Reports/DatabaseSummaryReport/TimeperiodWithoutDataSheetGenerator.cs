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
    /// Used To Generate "Timeperiod Without Data" Sheet of Summary Report
    /// </summary>
    internal class TimeperiodWithoutDataSheetGenerator: SheetGenerator 
    {
      
        #region "-- private --"
        
        #region "-- Methods --"
        
        /// <summary>
        /// Get DataTable for TimePeriod Missing DataValue 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetTimeperiodWithoutDataTable()
        {
            DataTable RetVal = this.CreateTimePeriodTable();
            // -- Get IUS witout DataValues Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Timeperiod.GetTimePeriodsWithoutData()).DefaultView;

            // -- Sort By Timeperiod
            Table.Sort = Timeperiods.TimePeriod + " Asc";

            foreach (DataRowView RowView in Table)
            {
                RetVal.ImportRow(RowView.Row);
            }

            // -- Rename Columns
            RetVal.Columns[Timeperiods.TimePeriod].ColumnName = this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD];

            return RetVal;
        }

        /// <summary>
        /// Carete TimePeriod Table.
        /// </summary>
        ///<returns>DataTable</returns>
        private DataTable CreateTimePeriodTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Timeperiods.TimePeriod);

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"
        
        /// <summary>
        /// Create TimePeriod without DataValue Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile, Constants.SheetsNames.TimeperiodsWithoutData);
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, Constants.SheetsNames.TimeperiodsWithoutData);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get TimePeriod Data TAble.
            Table = this.GetTimeperiodWithoutDataTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex);
            // -- autofit Map 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.Sheet.Timeperiod.TimePeriodLastColIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, true);
           

        }

        #endregion

        #endregion

        
    }
}
