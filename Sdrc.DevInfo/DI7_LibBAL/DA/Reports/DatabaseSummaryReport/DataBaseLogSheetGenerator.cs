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
    /// Used To Generate DataBase Log Sheet of Summary Report
    /// </summary>
    internal class DataBaseLogSheetGenerator: SheetGenerator
    {
        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get DataBaseLog DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetDataBaseLogTable()
        {
            DataTable RetVal = this.createDataBaseLogTable();
            // -- Fill Subgroup TAble 
           
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.DatabaseLog.GetDatabaseLog()).DefaultView;
            Table.Sort = DatabaseLog.DBTimeStamp + " Asc";

            foreach (DataRow row in Table.Table.Rows)
            {
                RetVal.ImportRow(row);
            }
            //-- Rename Table
            this.RenameDataBaseLogTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Create DataBaseLog DataTable For IUS SHEET
        /// </summary>
        ///<returns>Data Table</returns>
        private DataTable createDataBaseLogTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(DatabaseLog.DBName);
            RetVal.Columns.Add(DatabaseLog.DBTimeStamp);
            RetVal.Columns.Add(DatabaseLog.DBAction);
            RetVal.Columns.Add(DatabaseLog.DBUser);

            return RetVal;
        }

        /// <summary>
        /// Rename DataBaseLog DataTAble
        /// </summary>
        private void RenameDataBaseLogTable(ref DataTable table)
        {
            table.Columns[DatabaseLog.DBName].ColumnName = this.ColumnHeader[DSRColumnsHeader.FILENAME];
            table.Columns[DatabaseLog.DBAction].ColumnName = this.ColumnHeader[DSRColumnsHeader.ACTION];
            table.Columns[DatabaseLog.DBTimeStamp].ColumnName = this.ColumnHeader[DSRColumnsHeader.DATE_TIME];
            table.Columns[DatabaseLog.DBUser].ColumnName = this.ColumnHeader[DSRColumnsHeader.USER];
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"
        
        /// <summary>
        /// Create DataBase Log Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            DataTable Table = null;
            // -- Create Timeperiod Sheet
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.DATABASE_LOG]);

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.DATABASE_LOG]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            Table = this.GetDataBaseLogTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.DBLog.DataBaseLogDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.DBLog.DataBaseLogDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.DBLog.DataBaseLogDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.DBLog.DataBaseLogLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.DBLog.DataBaseLogDetailsRowIndex, Constants.Sheet.DBLog.DataBaseLogNameColIndex, LastRow, Constants.Sheet.DBLog.DataBaseLogNameColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.DBLog.DataBaseLogDetailsRowIndex, Constants.Sheet.DBLog.DataBaseLogNameColIndex, LastRow, Constants.Sheet.DBLog.DataBaseLogNameColIndex, true);
          
  
        }

        #endregion

        #endregion

       
    }
}
