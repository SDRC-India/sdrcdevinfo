using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate Unit Sheet of Summary Report
    /// </summary>   
    internal class UnitSheetGenerator: SheetGenerator
    {

        #region "-- Private --"
              
        /// <summary>
        /// Create Unit Table
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateUnitTable()
        {
            DataTable RetVal = new DataTable();
            // -- Add Columns For Required Indicator Fields 
            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"), Type.GetType("System.Int32"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGlobal);
            // -- Return Indicator Table
            return RetVal;
        }

        /// <summary>
        /// Return DataTable for Unit Sheet
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetUnitTable()
        {

            DataTable RetVal = this.CreateUnitTable();

            DataView UnitTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Unit.GetUnit(FilterFieldType.None, string.Empty)).DefaultView;
            UnitTable.Sort = Unit.UnitName + " Asc";

            foreach (DataRowView row in UnitTable)
            {
                string GlobalVal = string.Empty;
                DataRow Temp = RetVal.NewRow();

                Temp.BeginEdit();
                // Set Global VAlue to Yes or No
                if (!string.IsNullOrEmpty(row[Unit.UnitGlobal].ToString()))
                {
                    // -- IF Global value is True Change it TO Yes else No. 
                    if (row[Unit.UnitGlobal].ToString().ToUpper() == "TRUE")
                    { GlobalVal = "Yes"; }
                    else
                    { GlobalVal = "No"; }
                }
                else
                { GlobalVal = "No"; }

                Temp[Unit.UnitName] = row[Unit.UnitName];
                Temp[Unit.UnitGlobal] = GlobalVal;
                Temp.EndEdit();
                // -- Add Row Into Table
                RetVal.Rows.Add(Temp);
            }

            RenameUnitTable(ref RetVal);

            return RetVal;

        }

        /// <summary>
        /// Rename Unit Table Columns
        /// </summary>
        /// <param name="table">DataTable</param>
        private void RenameUnitTable(ref DataTable table)
        {
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[Unit.UnitGlobal].ColumnName = this.ColumnHeader[DSRColumnsHeader.GLOBAL];
        }
        #endregion

        #region "-- Internal --"

        /// <summary>
        /// Create Unit Sheet of Summary Report
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            // -- Create Unit Sheet
            int SheetNo = base.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.UNIT]);
            DataTable UnitTable = null;

            // -- sheet content 
            excelFile.SetCellValue(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.UNIT]);
            excelFile.GetCellFont(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get Unit Table Filled
            UnitTable = this.GetUnitTable();
            // -- Set SNo into TAble
            this.SetSNoIntoTableColumn(ref UnitTable);
            // -- Set Value To Excel Sheet
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Unit.DetailsRowIndex, Constants.HeaderColIndex, UnitTable, SheetNo, false);

            int LastRow = Constants.Sheet.Unit.DetailsRowIndex + UnitTable.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, SheetNo, Constants.Sheet.Unit.DetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Unit.LastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(SheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Unit.DetailsRowIndex, Constants.Sheet.SummaryReport.UnitColValueIndex, LastRow, Constants.Sheet.SummaryReport.UnitColValueIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(SheetNo, Constants.Sheet.Unit.DetailsRowIndex, Constants.Sheet.SummaryReport.UnitColValueIndex, LastRow, Constants.Sheet.SummaryReport.UnitColValueIndex, true);
           

        }

        #endregion
        
        
    }
}
