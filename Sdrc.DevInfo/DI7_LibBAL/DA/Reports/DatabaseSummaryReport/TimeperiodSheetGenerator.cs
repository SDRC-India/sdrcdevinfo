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
    /// Used To Generate Timeperiod Sheet of Summary Report
    /// </summary>
    internal class TimeperiodSheetGenerator:    SheetGenerator 
    {
        
        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Create TimePeriod Table.
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateTimePeriodTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Timeperiods.TimePeriod);

            return RetVal;
        }

        /// <summary>
        /// Create TimePeriod Table
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetTimePeriodTable()
        {
            DataTable RetVal = this.CreateTimePeriodTable();
            // -- Fill Subgroup TAble 
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty, Timeperiods.TimePeriod)).DefaultView;
            Table.Sort = Timeperiods.TimePeriod + " Asc";

            foreach (DataRowView row in Table)
            {
                RetVal.ImportRow(row.Row);
            }
            //-- Rename Table
            this.RenameTimePeriodTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Rename Timeperiod Table
        /// </summary>
        /// <param name="table">DataTable of TimePeriod</param>
        private void RenameTimePeriodTable(ref DataTable table)
        {
            table.Columns[Timeperiods.TimePeriod].ColumnName = this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD];

        }

        #endregion
        
        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create TimePeriod Sheet of Summary Report
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            // -- Create Timeperiod Sheet
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD]);

            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.TIMEPERIOD]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            Table = this.GetTimePeriodTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.TimePeriodColWidth, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex);
            excelFile.SetHorizontalAlignment(sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, SpreadsheetGear.HAlign.Left);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.Timeperiod.TimePeriodDetailsRowIndex, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, LastRow, Constants.Sheet.Timeperiod.TimePeriodColValueIndex, true);
            
           
        }

        #endregion

        #endregion

        
    }
}
