using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Data.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate IC Sheet of Summary Report
    /// </summary>
    internal class ICSheetGenerator: SheetGenerator 
    {

        #region "-- Private --"
        
        #region "-- Methods --"

        /// <summary>
        /// Get Table for DataCollection as per Level
        /// </summary>
        /// <param name="TotCols">Total Columns</param>
        /// <returns>Data Table</returns>
        protected DataTable GetCollectionTable(int TotalCols)
        {
            DataTable RetVal = new DataTable();

            for (int i = 1; i <= TotalCols; i++)
            {
                RetVal.Columns.Add(i.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Feed data into indicator Classification Sheet
        /// </summary>
        /// <param name="eICType">Enum of ICType</param>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetNo">Sheet No</param>
        internal void GenerateICSheet(ref DIExcel excelFile, int sheetNo, ICType eicType)
        {
            // -- Get Temp Table For ICs.
            DataRow[] objRows = null;
            DataRow NewRow = null;
            Int32 MaxLevel = 0;
            string[] Temp = null;
            int Counter = 0;
            int TotCols = 0;
            DataView TempTableView = null;
            DataView TempDV = null;
            // -- Get Temporary Table for Classification
            DataTable Table = this.GetTempTable();

            try
            {
                // -- Get Indicator Classification Value
                TempTableView = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IndicatorClassification.GetIC(FilterFieldType.None,string.Empty,eicType,FieldSelection.Heavy)).DefaultView;

                TempTableView.Sort = IndicatorClassifications.ICParent_NId + " ASC, " + IndicatorClassifications.ICName + " ASC";

                foreach (DataRowView RowView in TempTableView)
                {

                    NewRow = Table.NewRow();
                    // -- Get Records for IC ParentNID
                    objRows = Table.Select(Constants.ICTempTableColumns.ID + "=" + RowView[IndicatorClassifications.ICParent_NId].ToString());

                    NewRow[Constants.ICTempTableColumns.ID] = RowView[IndicatorClassifications.ICNId];

                    NewRow[Constants.ICTempTableColumns.PARENT_ID] = RowView[IndicatorClassifications.ICParent_NId];
                    NewRow[Constants.ICTempTableColumns.ACT_LABEL] = RowView[IndicatorClassifications.ICName].ToString();
                    //-- Set Label with Parent Level IF Parent Level Exist
                    if (objRows.Length > 0)
                    {
                        string[] tempVal = new string[] { Constants.ICTempTableColumns.ICLEVEL_SEPERATOR };
                        Temp = objRows[0][Constants.ICTempTableColumns.LABEL].ToString().Split(tempVal, StringSplitOptions.RemoveEmptyEntries);
                        if (MaxLevel < Temp.Length)
                        { MaxLevel = Temp.Length; }
                        NewRow[Constants.ICTempTableColumns.LABEL] = objRows[0][Constants.ICTempTableColumns.LABEL].ToString() + Constants.ICTempTableColumns.ICLEVEL_SEPERATOR + RowView[IndicatorClassifications.ICName].ToString();
                    }
                    else
                    {
                        NewRow[Constants.ICTempTableColumns.LABEL] = RowView[IndicatorClassifications.ICName];
                    }

                    Table.Rows.Add(NewRow);
                }
                Table.AcceptChanges();

                //-- update table 
                DataTable tempTbl = Table.Copy();
                foreach (DataRow dr in Table.Rows)
                {
                    foreach (DataRow drTmp in tempTbl.Rows)
                    {
                        // -- Set PArentID to "-99" IF HAve No Child
                        if (drTmp[Constants.ICTempTableColumns.LABEL].ToString().StartsWith(dr[Constants.ICTempTableColumns.LABEL].ToString() + Constants.ICTempTableColumns.ICLEVEL_SEPERATOR))
                        {
                            dr[Constants.ICTempTableColumns.PARENT_ID] = Constants.ICTempTableColumns.PARENTID_VALUE; // "-99"; 
                            Table.AcceptChanges();
                            break;
                        }
                    }
                }

                Table.DefaultView.Sort = Constants.ICTempTableColumns.LABEL;
                Temp = null;

                //-- Set Total Column Length.
                TotCols = MaxLevel + 1 + 3;
                int IndCol = MaxLevel + 1;
                int UnitCol = IndCol + 1;
                int SubCol = UnitCol + 1;

                // -- Fill IUS 
                int jj;
                DataTable TempTable =this.GetCollectionTable(TotCols);
                int LastRow;

                for (Counter = 0; Counter <= Table.Rows.Count - 1; Counter++)
                {
                    // -- if the Number of Elements in the DataRow is same as MaxLevel then get the IUS combination for this level 
                    string[] tempVal = new string[] { Constants.ICTempTableColumns.ICLEVEL_SEPERATOR };
                    Temp = Table.Rows[Counter][Constants.ICTempTableColumns.LABEL].ToString().Split(tempVal, StringSplitOptions.RemoveEmptyEntries);

                    // If Parent_NId is not -99 then Get IUS Record for Current ICNId.
                    if (Table.Rows[Counter][Constants.ICTempTableColumns.PARENT_ID].ToString() != Constants.ICTempTableColumns.PARENTID_VALUE)
                    {
                        // -- get the IUS for this DataRow 
                        try
                        {
                            TempDV = null;
                            TempDV = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IUS.GetDistinctIUSByIC(eicType, Table.Rows[Counter][Constants.ICTempTableColumns.ID].ToString(), FieldSelection.Light)).DefaultView;
                            // -- Sort By Indicator, Unit, Subgroup
                            TempDV.Sort = Indicator.IndicatorName + " Asc," + Unit.UnitName + " Asc," + SubgroupVals.SubgroupVal + " Asc";
                        }
                        catch (Exception)
                        {
                            // Interaction.MsgBox(ex.Message); 
                        }
                        // -- Set IUS for Each Level if ICNID have related IUS.
                        if (TempDV.Count > 0)
                        {
                            foreach (DataRowView DVRow in TempDV)
                            {
                                DataRow Drow = TempTable.NewRow();

                                for (jj = 0; jj <= Temp.GetUpperBound(0); jj++)
                                {
                                    Drow[jj] = Temp[jj];
                                }

                                Drow[IndCol] = DVRow[Indicator.IndicatorName].ToString();
                                Drow[UnitCol] = DVRow[Unit.UnitName].ToString();
                                Drow[SubCol] = DVRow[SubgroupVals.SubgroupVal].ToString();

                                TempTable.Rows.Add(Drow);
                            }
                        }
                        else
                        {
                            DataRow Row = TempTable.NewRow();
                            for (jj = 0; jj <= Temp.GetUpperBound(0); jj++)
                            {
                                Row[jj] = Temp[jj];
                            }
                            Row[jj] = "";
                            Row[jj + 1] = "";
                            Row[jj + 2] = "";

                            TempTable.Rows.Add(Row);
                        }
                        TempTable.AcceptChanges();
                    }
                }

                // -- Create DataArray 
                object[,] DataArray = new object[TempTable.Rows.Count + 1, TotCols + 1];
                try
                {

                    // Startng Column for Serial Number 
                    // -- header 
                    DataArray[0, 0] = DILanguage.GetLanguageString("SERIAL_NUMBER");
                    for (jj = 0; jj <= MaxLevel; jj++)
                    {
                        DataArray[0, jj + 1] = base.ColumnHeader[DSRColumnsHeader.LEVEL] + " " + (jj + 1);
                    }
                    DataArray[0, jj + 1] = base.ColumnHeader[DSRColumnsHeader.INDICATOR];
                    DataArray[0, jj + 2] = base.ColumnHeader[DSRColumnsHeader.UNIT];
                    DataArray[0, jj + 3] = base.ColumnHeader[DSRColumnsHeader.SUBGROUP];

                    // -- body 
                    for (Counter = 1; Counter <= TempTable.Rows.Count; Counter++)
                    {
                        // -- Exit to write data if Records exceed from MAximum defined Rows.
                        if (Counter > RangeCheckReport.RangeCheckCustomizationInfo.MAX_EXCEL_ROWS)
                        { break; }

                        DataArray[Counter, 0] = Counter;
                        for (jj = 0; jj <= TotCols - 1; jj++)
                        {
                            DataArray[Counter, jj + 1] = TempTable.Rows[Counter - 1][jj]; //[jj]; 
                        }
                    }
                }
                catch (Exception)
                {
                }
                // -- print the DataRaay into the Excel Sheet 
                excelFile.SetArrayValuesIntoSheet(sheetNo, Constants.Sheet.IC.ICDetailsRowIndex, Constants.Sheet.IC.ICHeaderColIndex, Constants.Sheet.IC.ICDetailsRowIndex + Counter + 1, TotCols + 1, DataArray);
                // excelFile.Range(excelFile.Cells(4, 1), excelFile.Cells(4 + Counter - 1, TotCols + 1)).Value = DataArray; 
                LastRow = Constants.Sheet.IC.ICDetailsRowIndex + Counter;
                // -- Apply Font Settings
                base.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.IC.ICDetailsRowIndex, Constants.Sheet.IC.ICHeaderColIndex, LastRow, TotCols, true);

                string sHeadVal = string.Empty;
                // -- header 
                switch (eicType)
                {
                    case ICType.Sector:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.SECTOR];
                        break;
                    case ICType.Goal:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.GOAL];
                        break;
                    case ICType.CF:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.CF];
                        break;
                    case ICType.Institution:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.INSTITUTION];
                        break;
                    case ICType.Theme:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.THEME];
                        break;
                    case ICType.Convention:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.CONVENTION];
                        break;
                    case ICType.Source:
                        sHeadVal = base.ColumnHeader[DSRColumnsHeader.SOURCE];
                        break;
                }

                // -- Set Header Value
                excelFile.SetCellValue(sheetNo, Constants.Sheet.IC.ICHeaderRowIndex, Constants.Sheet.IC.ICHeaderColIndex, sHeadVal);
                excelFile.GetCellFont(sheetNo, Constants.Sheet.IC.ICHeaderRowIndex, Constants.Sheet.IC.ICHeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

                // -- Show Missing Information Detail also 
                TempTable = this.GetCollectionTable(1); // new Collection(); 

                // -- Current IC Type is Source then Add Missing Information
                if (eicType == ICType.Source)
                {
                    // -- Show Missing Information Detail also 
                    TempDV = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Source.GetMissingInfoSources(DatabaseSummaryReportGenerator.DBConnection.ConnectionStringParameters.ServerType)).DefaultView;
                    // -- Sort by Name
                    TempDV.Sort = IndicatorClassifications.ICName + " Asc";
                    TotCols = 1;
                    if (TempDV.Count > 0)
                    {
                        foreach (DataRowView DVRow in TempDV)
                        {
                            // -- Data Array 
                            DataRow row = TempTable.NewRow();
                            row[0] = DVRow[IndicatorClassifications.ICName].ToString();
                            TempTable.Rows.Add(row);
                        }
                        DataArray = new object[TempTable.Rows.Count + 2, TotCols + 1];
                        // Startng Column for Serial Number 
                        DataArray[0, 0] = base.ColumnHeader[DSRColumnsHeader.MISSINGINFORMATION];
                        DataArray[1, 0] = DILanguage.GetLanguageString("SERIAL_NUMBER");
                        DataArray[1, 1] = base.ColumnHeader[DSRColumnsHeader.SOURCE];
                        // -- body 
                        for (Counter = 1; Counter <= TempTable.Rows.Count; Counter++)
                        {
                            DataArray[Counter + 1, 0] = Counter;
                            for (jj = 0; jj <= TotCols - 1; jj++)
                            {
                                DataArray[Counter + 1, 1] = TempTable.Rows[Counter - 1][0];
                            }
                        }
                        LastRow += 1;
                        // -- print the DataRaay into the Excel Sheet
                        excelFile.SetArrayValuesIntoSheet(sheetNo, LastRow, Constants.Sheet.IC.ICHeaderColIndex, LastRow + Counter + 1, TotCols + 1, DataArray);
                        // -- Apply Font Settings
                        base.ApplyFontSettings(ref excelFile, sheetNo, LastRow + 1, Constants.Sheet.IC.ICHeaderColIndex, LastRow + Counter + 1, TotCols, true);
                        base.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.IC.ICDetailsRowIndex, Constants.Sheet.IC.ICHeaderColIndex, LastRow + Counter + 1, TotCols, true);

                        // -- Set Missing Info Font and size
                        excelFile.GetCellFont(sheetNo, LastRow, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;
                    }
                }

                // -- Set Column Widths 
                for (jj = 0; jj <= MaxLevel; jj++)
                {
                    excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.ICLavelColWidth, Constants.Sheet.IC.ICDetailsRowIndex, jj+1, LastRow, jj+1);
                }
                    // -- Set indicator Column Width 40.
                    excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.ICNameColWidth, Constants.Sheet.IC.ICDetailsRowIndex, jj +1 , LastRow, jj +1);
                    // -- Set Auto col width for Unit and Subpop 
                    excelFile.AutoFitColumns(sheetNo, Constants.Sheet.IC.ICDetailsRowIndex, jj + 2, Constants.Sheet.IC.ICDetailsRowIndex + Counter, jj + 3);

                // -- Set S No Column Width
                excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.SNoColumnWidth, Constants.Sheet.IC.ICDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.HeaderColIndex );
            }

            catch (Exception)
            {}
            finally
            {
                //-- Close Data REader
                if ((TempTableView != null))
                    TempTableView.Dispose();
                // -- Dispose DataView
                if ((TempDV != null))
                    TempDV.Dispose();

            }

        }

        /// <summary>
        /// Not In Use
        /// </summary>
        /// <param name="excelFile"></param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            //--N/A
        }

        #endregion

        #endregion

       
    }
}
