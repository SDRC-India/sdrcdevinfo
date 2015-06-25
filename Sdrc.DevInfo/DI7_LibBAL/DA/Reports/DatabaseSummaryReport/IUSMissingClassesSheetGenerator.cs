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
    /// Used To Create IUS Missing ICs Sheet of Summary Report
    /// </summary>
    internal class IUSMissingClassesSheetGenerator: SheetGenerator 
    {
       
    #region "-- private --"
                
        #region "-- Methods --"

        /// <summary>
        /// Get IUS Missing ICs DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetIUSMissingICTable()
        {
            DataTable RetVal = this.CreateIUSMissingICTable();
            // -- Get IUS Linked Data Table
            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IUS.GetIUSUnmatchedIC());
            foreach (DataRow row in Table.Rows)
            {
                RetVal.ImportRow(row);
            }
            // -- Rename Columns
            this.RenameIUSMissingICTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create IUS Missing Classification DataTable 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateIUSMissingICTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);

            return RetVal;
        }

        /// <summary>
        /// Rename Column of IUS Not Linked TO IC Table
        /// </summary>
        /// <param name="table">DataTable</param>
        private void RenameIUSMissingICTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUBGROUP];
        }
        
        #endregion

    #endregion

    #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create IUS NOT Linked TO Classification Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile, DILanguage.GetLanguageString("IUS_MISSING_CLASSIFICATION"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("IUS_MISSING_CLASSIFICATION"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get IUSLinked Data TAble.
            Table = this.GetIUSMissingICTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex);
            // -- autofit Map 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, true);
            // -- Set Cell Borders
            excelFile.SetCellBorder(Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.HeaderColIndex, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black);
            
        }

        #endregion

    #endregion

    }
}
