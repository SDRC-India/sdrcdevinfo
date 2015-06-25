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
    /// Used To Generate IUSWithoutData Sheet of Summary Report
    /// </summary>
    internal class IUSWithoutDataSheetGenerator: SheetGenerator
    {
        
        #region "-- private --"
        
        #region "-- Methods --"

        /// <summary>
        /// Return DataTable with data for IUS Missing DataValue 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetIUSWithoutDataTable()
        {
            DataTable RetVal = this.CreateIUSWithoutDataTable();
            // -- Get IUS witout DataValues Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IUS.GetIUSWithoutData()).DefaultView;
            // -- Sort By Indicator, Unit, Subgroup
            Table.Sort= Indicator.IndicatorName + " ASC," + Unit.UnitName + " ASC," +SubgroupVals.SubgroupVal + " ASC";

            foreach (DataRowView row in Table)
            {
                RetVal.ImportRow(row.Row);
            }
            // -- Rename Columns
            this.RenameIUSWithoutDataTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create IUS Missing Classification DataTable 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateIUSWithoutDataTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);

            return RetVal;
        }

        /// <summary>
        /// Rename Column of IUS Without Data Table
        /// </summary>
        /// <param name="table">DataTable</param>
        private void RenameIUSWithoutDataTable(ref DataTable table)
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
        /// Create IUS without Data Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel excelFile)
        {
            int sheetNo = base.CreateSheet(ref excelFile, DILanguage.GetLanguageString("IUS_WITHOUT_DATA"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("IUS_WITHOUT_DATA"));

            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get IUSLinked Data TAble.
            Table = this.GetIUSWithoutDataTable();
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
            
            
        }

        #endregion

        #endregion

        
    }
}
