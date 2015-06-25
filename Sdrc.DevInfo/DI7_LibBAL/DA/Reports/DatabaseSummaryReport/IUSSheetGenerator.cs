using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate IUS Sheet of Summary Report
    /// </summary>
    internal class IUSSheetGenerator : SheetGenerator
    {

        #region "-- private --"

        #region "-- Methods --"
        /// <summary>
        /// Get IUS DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetIUSTable()
        {
            DataTable RetVal = this.createIUSTable();
            // -- Fill Subgroup TAble 
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light)).DefaultView;

            Table.Sort = Indicator.IndicatorName + " Asc, " + Unit.UnitName + " Asc, " + SubgroupVals.SubgroupVal + " Asc";

            foreach (DataRowView ROW in Table)
            {
                RetVal.ImportRow(ROW.Row);

                if (this.CurrentSheetType == SummarySheetType.Detailed)
                {
                    // -- Set value for Count OF DataVAlue present against an IUS.
                    string SqlQuery = DIQueries.GetTableRecordsCount(DatabaseSummaryReportGenerator.DBQueries.TablesName.Data, Data.IUSNId + "=" + ROW[Data.IUSNId]);
                    RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.COUNT]] = DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery).ToString();
                
                    // -- Set value for SUM OF DataVAlue present against an IUS.
                    RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetSumOFDataValuesForIUS((int)ROW[Data.IUSNId]);
                }
            }
            //-- Rename Table
            this.RenameIUSTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Get the Sum of DataValue for a IUSNID
        /// </summary>
        /// <param name="iusNid">IUSNid</param>
        /// <returns>decimal</returns>
        private decimal GetSumOFDataValuesForIUS(int iusNid)
        {
            decimal RetVal = 0;
            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Data.GetDataByIUSTimePeriodAreaSource(iusNid.ToString(), string.Empty, string.Empty, string.Empty, FieldSelection.Light));
            try
            {
                foreach (DataRow ROW in Table.Rows)
                {
                    double DataVal;
                    // -- IF current Cell have double datatype value then Add in aggregate value 
                    if (double.TryParse(ROW[Data.DataValue].ToString(), out DataVal))
                    {
                        RetVal += (decimal)DataVal;
                    }
                }
            }
            finally
            {
                Table.Dispose();
            }

            return RetVal;
        }

        /// <summary>
        /// Return IUS DataTable For IUS SHEET
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable createIUSTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);

            if (this.CurrentSheetType == SummarySheetType.Detailed)
            {
                RetVal.Columns.Add(this.ColumnHeader[DSRColumnsHeader.COUNT]);
                RetVal.Columns.Add(this.ColumnHeader[DSRColumnsHeader.SUM]);
            }

            return RetVal;
        }

        /// <summary>
        /// Rename IUS DataTable
        /// </summary>
        private void RenameIUSTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUBGROUP];
            if (this.CurrentSheetType == SummarySheetType.Detailed)
            {
                table.Columns[this.ColumnHeader[DSRColumnsHeader.COUNT]].ColumnName = this.ColumnHeader[DSRColumnsHeader.COUNT] + " " + this.ColumnHeader[DSRColumnsHeader.OF] + " " + this.ColumnHeader[DSRColumnsHeader.DATAVALUE];
                table.Columns[this.ColumnHeader[DSRColumnsHeader.SUM]].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUM] + " " + this.ColumnHeader[DSRColumnsHeader.OF] + " " + this.ColumnHeader[DSRColumnsHeader.DATAVALUE];
            }
        }

      
        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create IUS Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {

            // -- Create Timeperiod Sheet
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.IUS]);

            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.IUS]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            excelFile.SetCellValue(sheetNo, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.Sheet.SummaryReport.IUSColValueIndex, ColumnHeader[DSRColumnsHeader.IUS]);
            excelFile.SetCellValue(sheetNo, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.Sheet.SummaryReport.IUSColValueIndex, ColumnHeader[DSRColumnsHeader.GLOBAL]);

            Table = this.GetIUSTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.IUS.IUSDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.IUS.IUSLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.Sheet.IUS.IUSNameColIndex, LastRow, Constants.Sheet.IUS.IUSNameColIndex);
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.Sheet.IUS.IUSCountColINdex, LastRow, Constants.Sheet.IUS.IUSLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.Sheet.IUS.IUSNameColIndex, LastRow, Constants.Sheet.IUS.IUSNameColIndex, true);
            // -- Set Cell Borders
            excelFile.SetCellBorder(Constants.Sheet.IUS.IUSDetailsRowIndex, Constants.HeaderColIndex, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Medium, Color.Black);
        }

        #endregion

        #endregion


    }
}
