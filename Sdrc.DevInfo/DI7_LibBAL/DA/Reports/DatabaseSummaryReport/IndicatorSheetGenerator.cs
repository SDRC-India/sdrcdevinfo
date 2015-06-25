using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate Indicator Sheet of Summary Report
    /// </summary>
    internal class IndicatorSheetGenerator : SheetGenerator 
    {

         #region "-- Private --"

        /// <summary>
        /// Get Indicator DataTable with Fields Name S No, Indicator, Global, Information.
        /// </summary>
        /// <returns>Data Table</returns>
        private DataTable GetIndicatorDataTable()
        {
            DataTable RetVal = null;
            try
            {
                // -- Fill DataTable with Indicators
                DataView TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Heavy)).DefaultView;
               
                //-- Fill Table with Required Field Values
                DataTable IndicatorTable = this.FillIndicatorTable(TempTable.Table);
                // -- Rename Table Column Name
                this.RenameIndicatorTableFields(ref IndicatorTable,false);
                                
                //-- Copy Table to return.
                RetVal = IndicatorTable.Copy();

                TempTable.Dispose();
                IndicatorTable.Dispose();

            }
            catch (Exception ex) { throw ex; }

            return RetVal;
        }

        /// <summary>
        /// Get Missing Indicators Name
        /// </summary>
        /// <returns>Data Table</returns>
        private DataTable GetMissingIndicators(DataTable indTable)
        {
            DataTable RetVal = new DataTable();
            DataRow[] Rows=null;
            try
            {
                RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"), Type.GetType("System.Int32"));
                RetVal.Columns.Add(this.ColumnHeader[DSRColumnsHeader.INDICATOR]);

                if (indTable.Columns.Contains(this.ColumnHeader[DSRColumnsHeader.INFORMATION]))
                {
                    try
                    {
                        Rows = indTable.Select(this.ColumnHeader[DSRColumnsHeader.INFORMATION] + " = 'No'");
                    }
                    catch (Exception)
                    {
                        //-- Handle for Khmer font
                        indTable.Columns[3].ColumnName = "Information";                      
                        Rows = indTable.Select(indTable.Columns[3].ColumnName + " = 'No'"); 
                        indTable.Columns[3].ColumnName =this.ColumnHeader[DSRColumnsHeader.INFORMATION];
                    }                   
                }               
               
                
                foreach (DataRow  row in Rows  )
                {
                    RetVal.ImportRow(row);
                    
                }

                RetVal.AcceptChanges();
            }
            catch (Exception ex) { throw ex; }

            return RetVal;
        }

        /// <summary>
        ///  Create Indicator DataTable with Required Fields
        /// </summary>
        /// <param name="isMissingIndicator"> True In case OF Missing Indicators</param>
        /// <returns>DataTable</returns>
        private DataTable CreateIndicatorTable(bool isMissingIndicator)
        {
            DataTable RetVal = new DataTable();
            // -- Add Columns For Required Indicator Fields 
            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"), Type.GetType("System.Int32"));
            RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName);
            // -- Not Add Global and Infomation Column In case Of Missing Indicator
            if (isMissingIndicator == false )
            {
                RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGlobal, Type.GetType("System.String"));
                RetVal.Columns.Add(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorInfo);
            }        
            // -- Return Indicator Table
            return RetVal;
        }

        /// <summary>
        /// Fill Indicator Table from Existing DataTable
        /// </summary>
        /// <param name="indicatorTable">Indicator Table</param>
        /// <returns>DataTable</returns>
        private DataTable FillIndicatorTable(DataTable indicatorTable)
        {
            DataTable RetVal = null;

            // -- Get New DataTable for Indicator Required Columns
            RetVal = this.CreateIndicatorTable(false);
            
            if (indicatorTable.Columns.Count > 1)
            {
                indicatorTable.DefaultView.Sort = Indicator.IndicatorName + " Asc";

                //-- Import row into Indicator Table
                foreach (DataRowView Row in indicatorTable.DefaultView)
                {
                    string GlobalVal = string.Empty;
                    DataRow Temp = RetVal.NewRow();
                    Temp.BeginEdit();
                    // Set Global VAlue to Yes or No
                    if (!string.IsNullOrEmpty(Row[Indicator.IndicatorGlobal].ToString()))
                    {
                        // -- IF Global value is True Change it TO Yes else No. 
                        if (Row[Indicator.IndicatorGlobal].ToString().ToUpper() == "TRUE")
                        { GlobalVal = "Yes"; }
                        else
                        { GlobalVal = "No"; }
                    }
                    else
                    {
                        GlobalVal = "No";
                    }
                    Temp[Indicator.IndicatorGlobal] = GlobalVal;
                    // -- Make IndicatorInfo to Yes if Preasent otherwise No.
                    if (this.CheckMetadataofIndiacatorNID(Row[Indicator.IndicatorNId].ToString().Trim()))
                    { GlobalVal = "Yes"; }
                    else
                    {
                        GlobalVal = "No";
                    }
                    Temp[Indicator.IndicatorName] = Row[Indicator.IndicatorName];
                    Temp[Indicator.IndicatorInfo] = GlobalVal;
                    Temp.EndEdit();
                    // -- Import row
                    RetVal.Rows.Add(Temp);

                }
            }
            else
            {
                //-- Import row into Indicator Table
                foreach (DataRowView RowView in indicatorTable.DefaultView)
                {
                    RetVal.ImportRow(RowView.Row);
                }
            }

            return RetVal;
        }
        /// <summary>
        /// Checking weather IndicatorNid is present in MetadataReport or not
        /// </summary>
        /// <param name="targetindicatornid"></param>
        /// <returns></returns>
        private bool CheckMetadataofIndiacatorNID(string targetindicatornid)
        {
            Boolean RetVal = false;
            String SqlQuery = String.Empty;
            SqlQuery = DatabaseSummaryReportGenerator.DBQueries.Indicators.GetDistinctIndicatorfromMetadataReport();
            DataView TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;
            DataTable tempdatatable = TempTable.Table;
            foreach (DataRow  targetnid in tempdatatable.Rows )
            {
                if (targetindicatornid==targetnid[Indicator.IndicatorNId].ToString())
                {
                    RetVal = true;
                }
            }
            return RetVal;
        }
        /// <summary>
        /// Rename Indicator DataTable Column for Required Column Heading
        /// </summary>
        /// <param name="IndicatorTable">Indicator Table</param>
        private void RenameIndicatorTableFields(ref DataTable indicatorTable,bool isMissing)
        {
            indicatorTable.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            if (isMissing == false)
            {
                indicatorTable.Columns[Indicator.IndicatorGlobal].ColumnName = this.ColumnHeader[DSRColumnsHeader.GLOBAL];
                indicatorTable.Columns[Indicator.IndicatorInfo].ColumnName = this.ColumnHeader[DSRColumnsHeader.INFORMATION];
            }
        }

        #endregion

         #region "-- Internal --"

        /// <summary>
        /// Generate Indicator Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
          // -- Create Indicator Sheet
            int SheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.INDICATOR]);
            DataTable IndicatorTable = null;

            // -- sheet content 
            excelFile.SetCellValue(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.INDICATOR]);
            excelFile.GetCellFont(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get Indicator DataTable with Values
            IndicatorTable = this.GetIndicatorDataTable();
            
            this.SetSNoIntoTableColumn(ref IndicatorTable);
                       
            // -- Fill Indicator Excel Sheet with Data
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, IndicatorTable, SheetNo, false);
            
            // -- Apply Font Settings
            int RowCount = IndicatorTable.Rows.Count;
            this.ApplyFontSettings(ref excelFile, SheetNo, Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, Constants.Sheet.Indicator.DetailsRowIndex + RowCount, Constants.Sheet.Indicator.DetailsRowIndex, true);

            // -- Get Last Row 
            RowCount = Constants.Sheet.Indicator.DetailsRowIndex + RowCount;

            // -- Set Column Width
            excelFile.SetColumnWidth(SheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Indicator.DetailsRowIndex, Constants.Sheet.SummaryReport.IndicatorColValueIndex, RowCount, Constants.Sheet.SummaryReport.IndicatorColValueIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(SheetNo, Constants.Sheet.Indicator.DetailsRowIndex, Constants.Sheet.SummaryReport.IndicatorColValueIndex, RowCount, Constants.Sheet.SummaryReport.IndicatorColValueIndex, true);
            //// -- Set Cell Borders
            //excelFile.SetCellBorder(Constants.Sheet.Indicator.IndicatorDetailsRowIndex, Constants.Sheet.SummaryReport.IndicatorColIndex, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black);
            RowCount += 2;

            // -- Show Missing Info
            excelFile.SetCellValue(SheetNo, RowCount, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.MISSINGINFORMATION]);
            excelFile.GetCellFont(SheetNo, RowCount, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            DataTable MissingTable = null;
            MissingTable = GetMissingIndicators(IndicatorTable);
            this.SetSNoIntoTableColumn(ref MissingTable);

            RowCount += 1;
            // -- Fill Indicator Excel Sheet with Data
            excelFile.LoadDataTableIntoSheet(RowCount, Constants.HeaderColIndex, MissingTable, SheetNo, false);

            // -- Apply Font Settings
            int LastRow = MissingTable.Rows.Count + RowCount;
            this.ApplyFontSettings(ref excelFile, SheetNo, RowCount, Constants.HeaderColIndex, LastRow, Constants.Sheet.Indicator.NameColIndex, true);
  
        }

    	#endregion
        
    }
}
