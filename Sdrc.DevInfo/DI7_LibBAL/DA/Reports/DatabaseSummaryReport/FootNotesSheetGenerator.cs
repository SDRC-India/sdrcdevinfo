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
    /// Used To Generate Footnotes Sheet of Summary Report
    /// </summary>
    internal class FootNotesSheetGenerator: SheetGenerator
    {

        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get Footnotes DataTable
        /// </summary>
        /// <returns>Data Table</returns>
        private DataTable GetFootNoteTable()
        {
            DataTable RetVal = this.CreateFootNoteTable();

            // -- Get Duplicate Values Data Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Footnote.GetFootNotesWithDataValue()).DefaultView;
            int i = 0;
            // -- Sort By Indicator, Unit, Subgroup
            Table.Sort = Indicator.IndicatorName + " ASC," + Unit.UnitName + " ASC," + SubgroupVals.SubgroupVal + " ASC";

            foreach (DataRowView RowView in Table)
            {
                // -- Stop Importindata if rocord exceed from maximum defined sheet rows
                if (i > RangeCheckReport.RangeCheckCustomizationInfo.MAX_EXCEL_ROWS)
                    break;

                RetVal.ImportRow(RowView.Row);
                i += 1;
            }
            // -- Rename Columns
            this.RenameFootNoteTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create Table for FootNotes
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateFootNoteTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Timeperiods.TimePeriod);
            RetVal.Columns.Add(IndicatorClassifications.ICName);
            RetVal.Columns.Add(Data.DataValue);
            RetVal.Columns.Add(FootNotes.FootNote);


            return RetVal;
        }

        /// <summary>
        /// Rename Table OF FootNotes with Language based Column name
        /// </summary>
        /// <param name="table">DataTable</param>
        private void RenameFootNoteTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUBGROUP];
            table.Columns[Timeperiods.TimePeriod].ColumnName = this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD];
            table.Columns[IndicatorClassifications.ICName].ColumnName = this.ColumnHeader[DSRColumnsHeader.SOURCE];
            table.Columns[Area.AreaName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREA];
            table.Columns[Data.DataValue].ColumnName = this.ColumnHeader[DSRColumnsHeader.DATA];
        }

        #endregion

        #endregion

        #region "-- Internal --"
        
        #region "-- Methods --"
        
        /// <summary>
        ///  Create FootNote Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            DataTable Table = null;
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.FOOTNOTES]);
            
            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.Sheet.Footnotes.FootNotesHeaderRowIndex, Constants.Sheet.Footnotes.FootNotesHeaderColIndex, this.ColumnHeader[DSRColumnsHeader.FOOTNOTES]);
            excelFile.GetCellFont(sheetNo, Constants.Sheet.Footnotes.FootNotesHeaderRowIndex, Constants.Sheet.Footnotes.FootNotesHeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get FootNotes Data TAble.
            Table = this.GetFootNoteTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Footnotes.FootNotesDetailsRowIndex, Constants.Sheet.Footnotes.FootNotesHeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Footnotes.FootNotesDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Footnotes.FootNotesDetailsRowIndex, Constants.Sheet.Footnotes.FootNotesHeaderColIndex, LastRow, Constants.Sheet.Footnotes.FootNotesLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Footnotes.FootNotesNameColIndex, Constants.Sheet.Footnotes.FootNotesNameColIndex, LastRow, Constants.Sheet.Footnotes.FootNotesNameColIndex);
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Footnotes.FootNotesDetailsRowIndex, Constants.Sheet.Footnotes.FootNotesLastColIndex, LastRow, Constants.Sheet.Footnotes.FootNotesLastColIndex);
            
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.Footnotes.FootNotesHeaderRowIndex, Constants.Sheet.Footnotes.FootNotesNameColIndex, LastRow, Constants.Sheet.Footnotes.FootNotesNameColIndex, true);
            excelFile.WrapText(sheetNo, Constants.Sheet.Footnotes.FootNotesHeaderRowIndex, Constants.Sheet.Footnotes.FootNotesLastColIndex, LastRow, Constants.Sheet.Footnotes.FootNotesLastColIndex, true);
            // -- Set Cell Borders
           
        }
        

        #endregion

        #endregion

       
    }
}
