using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate DuplicateData Sheet of Summary Report
    /// </summary>
    internal class DuplicateDataSheetGenerator: SheetGenerator
    {
     
        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get Duplicate DataValues DataTable
        /// </summary>
        /// <returns>Data Table</returns>
        private DataTable GetDuplicateDataTable()
        {
            DataTable RetVal = this.CreateDuplicateDataTable();
            string DataValue = string.Empty;

            // -- Get Duplicate Values Data Table
            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Data.GetDuplicateDataValues());
            //this.RemoveSingleInstanceData(ref Table);
            
            // -- Sort By Indicator, Unit, Subgroup
            Table.DefaultView.Sort = Indicator.IndicatorName + " ASC," + Unit.UnitName + " ASC," + SubgroupVals.SubgroupVal + " ASC";
            foreach (DataRowView RowView in Table.DefaultView)
            {
                if (DataValue != RowView[Data.DataValue].ToString())     //-- Set DataValue If DataValue Changed.
                {
                    if (!string.IsNullOrEmpty(DataValue))        //-- Insert Blank Row IF DataValue Changed. 
                    {
                        DataRow BlankRow = RetVal.NewRow();
                        RetVal.Rows.Add(BlankRow);
                    }
                    DataValue = Information.IsDBNull(RowView[Data.DataValue]) ? string.Empty : RowView[Data.DataValue].ToString();

                }
                RetVal.ImportRow(RowView.Row);
            }
            // -- Delete Extra row if Records are more than the excel worksheet limit.
            if (RetVal.Rows.Count > RangeCheckReport.RangeCheckCustomizationInfo.MAX_EXCEL_ROWS)
            {
                for (int i = RetVal.Rows.Count - 1; i > RangeCheckReport.RangeCheckCustomizationInfo.MAX_EXCEL_ROWS; i--)
                {
                    RetVal.Rows[i].Delete();
                }
            }
            RetVal.AcceptChanges();
            // -- Rename Columns
            this.RenameDuplicateDataTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Delete Single Instane DataValue
        /// </summary>
        /// <param name="table">Data Table of DuplicateData</param>
        private void RemoveSingleInstanceData(ref DataTable table)
        {
            int Counter = 0;
            string DataValue = string.Empty;

            int i = 0;
            while (i < table.Rows.Count)
            {
                //-- Set DataValue If DataValue Changed.
                if (DataValue != table.Rows[i][Data.DataValue].ToString())
                {
                    //-- Reinitialize Counter and delete record.
                    if (!string.IsNullOrEmpty(DataValue))
                    {
                        //-- Delete Record with Single Instance
                        if (Counter == 1)
                        {
                            table.Rows[i - 1].Delete();
                            table.AcceptChanges();
                            i--;
                        }
                        Counter = 0;
                    }
                    //Counter += 1;
                    DataValue = Information.IsDBNull(table.Rows[i][Data.DataValue]) ? string.Empty : table.Rows[i][Data.DataValue].ToString();
                }
                Counter += 1;
                i++;
            }
        }

        /// <summary>
        ///Create Table for Duplicate Data
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateDuplicateDataTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns.Add(Data.DataNId);
            RetVal.Columns.Add(Data.DataValue);
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Timeperiods.TimePeriod);
            RetVal.Columns.Add(IndicatorClassifications.ICName);

            return RetVal;
        }

        /// <summary>
        /// Rename Column OF DuplicateData TAble
        /// </summary>
        /// <param name="table">DataTable</param>
        private void RenameDuplicateDataTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUBGROUP];
            table.Columns[Timeperiods.TimePeriod].ColumnName = this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD];
            table.Columns[IndicatorClassifications.ICName].ColumnName = this.ColumnHeader[DSRColumnsHeader.SOURCE];
            table.Columns[Data.DataValue].ColumnName = this.ColumnHeader[DSRColumnsHeader.DATA];
            table.Columns[Area.AreaName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREA];
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create Duplicate DataValue Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile,DILanguage.GetLanguageString("DUPLICATE_DATAVALUES"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("DUPLICATE_DATAVALUES"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get DuplicateData Data TAble.
            Table = this.GetDuplicateDataTable();
            int Counter = 0;
            foreach (DataRow Row in Table.Rows)
            {
                // -- Set Serial No IF Row is not Blank
                if (Row[this.ColumnHeader[DSRColumnsHeader.DATA]].ToString() != string.Empty)
                {
                    Counter += 1;
                    Row[DILanguage.GetLanguageString("SERIAL_NUMBER")] = Counter;
                }
            }
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.DuplicateData.DuplicateDataDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.DuplicateData.DuplicateDataDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.DuplicateData.DuplicateDataDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.DuplicateData.DuplicateDataLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.DuplicateData.DuplicateDataNameColIndex, Constants.Sheet.DuplicateData.DuplicateDataNameColIndex, LastRow, Constants.Sheet.DuplicateData.DuplicateDataNameColIndex);
            //excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.OthersColumnWidth, Constants.Sheet.DuplicateData.DuplicateDataDetailsRowIndex, Constants.Sheet.DuplicateData.DuplicateDataOthersColIndex, LastRow, Constants.Sheet.DuplicateData.DuplicateDataLastColIndex);
            // -- autofit Map 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.DuplicateData.DuplicateDataDetailsRowIndex, Constants.Sheet.DuplicateData.DuplicateDataLastColIndex, LastRow, Constants.Sheet.DuplicateData.DuplicateDataLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.DuplicateData.DuplicateDataNameColIndex, LastRow, Constants.Sheet.DuplicateData.DuplicateDataNameColIndex, true);
           
   
        }

        #endregion

        #endregion
        
        
    }
}
