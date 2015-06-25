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
    /// Used To Generate TemplateLog Sheet of Summary Report
    /// </summary>
    internal class TemplateLogSheetGenerator: SheetGenerator
    {
        
        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get Template Log DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetTemplateLogTable()
        {
            DataTable RetVal = this.createTemplateLogTable();
            // -- Fill Subgroup TAble 
           
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.TemplateLog.GetTemplateLog()).DefaultView;

            foreach (DataRow row in Table.Table.Rows)
            {
                RetVal.ImportRow(row);
            }
            //-- Rename Table
            this.RenameTemplateLogTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Create TemplateLog DataTable For IUS SHEET
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable createTemplateLogTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(TemplateLog.TPLName);
            RetVal.Columns.Add(TemplateLog.TPLTimeStamp);
            RetVal.Columns.Add(TemplateLog.TPLAction);
            RetVal.Columns.Add(TemplateLog.TPLUser);

            return RetVal;
        }

        /// <summary>
        /// Rename TemplateLog DataTAble
        /// </summary>
        private void RenameTemplateLogTable(ref DataTable table)
        {
            // TPL_Name, TPL_TimeStamp, TPL_Action, TPL_User
            table.Columns[TemplateLog.TPLName].ColumnName = this.ColumnHeader[DSRColumnsHeader.FILENAME];
            table.Columns[TemplateLog.TPLAction].ColumnName = this.ColumnHeader[DSRColumnsHeader.ACTION];
            table.Columns[TemplateLog.TPLTimeStamp].ColumnName = this.ColumnHeader[DSRColumnsHeader.DATE_TIME];
            table.Columns[TemplateLog.TPLUser].ColumnName = this.ColumnHeader[DSRColumnsHeader.USER];
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create TemplateLog Log Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            // -- Create Timeperiod Sheet
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.TEMPLATE_LOG]);

            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.TEMPLATE_LOG]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

           Table = this.GetTemplateLogTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.TemplateLog.TemplateLogDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.TemplateLog.TemplateLogDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.TemplateLog.TemplateLogDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.TemplateLog.TemplateLogLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.TemplateLog.TemplateLogDetailsRowIndex, Constants.Sheet.TemplateLog.TemplateLogNameColIndex, LastRow, Constants.Sheet.TemplateLog.TemplateLogNameColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.TemplateLog.TemplateLogDetailsRowIndex, Constants.Sheet.TemplateLog.TemplateLogNameColIndex, LastRow, Constants.Sheet.TemplateLog.TemplateLogNameColIndex, true);
           
        }

        #endregion

        #endregion

    }
}
