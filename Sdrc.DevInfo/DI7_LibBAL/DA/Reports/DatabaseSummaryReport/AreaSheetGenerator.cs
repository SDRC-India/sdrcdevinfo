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
    /// Used To Create Area Sheet of Summary Report
    /// </summary>
    internal class AreaSheetGenerator: SheetGenerator  
    {

        #region "-- private --"

        #region "-- Methods --"
        
        /// <summary>
        /// Carete TimePeriod Table.
        /// </summary>
        private DataTable CreateAreaTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Area.AreaID);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Area_Level.AreaLevel);
            RetVal.Columns.Add(Area_Level.AreaLevelName);
            RetVal.Columns.Add(Area.AreaGlobal);
            RetVal.Columns.Add(Area.AreaMap);
            RetVal.Columns.Add(Area_Map_Layer.StartDate);   //,Type.GetType("System.DateTime"));
            RetVal.Columns.Add(Area_Map_Layer.EndDate);     //, Type.GetType("System.DateTime"));

            if (this.CurrentSheetType == SummarySheetType.Detailed)
            {
                // -- Add Co,umns for Count and Sum 
                RetVal.Columns.Add(this.ColumnHeader[DSRColumnsHeader.COUNT]);
                RetVal.Columns.Add(this.ColumnHeader[DSRColumnsHeader.SUM]);
            }

            return RetVal;
        }

        /// <summary>
        /// Create TimePeriod Table
        /// </summary>
        /// <returns></returns>
        //private DataTable GetAreaTable()
        //{
        //    DataTable RetVal = this.CreateAreaTable();
        //    DataTable AreaMapLayerTable = null;
        //    DataView AreaTable = null ;
        //    string AreaNid = string.Empty;

        //    try
        //    {
        //        // -- Fill Subgroup TAble 
        //        AreaTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaWithLevelName()).DefaultView;
        //        // -- Sort by Area Name, area ID, Area Map.
        //        AreaTable.Sort = Area.AreaName + " Asc ," + Area.AreaID + " Asc," + Area.AreaMap + " Asc";

        //        AreaMapLayerTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, string.Empty, FieldSelection.Light)).DefaultView.ToTable(true,Area.AreaNId, Area_Map_Layer.StartDate, Area_Map_Layer.EndDate);
               
        //        if (AreaMapLayerTable.Rows.Count > 0)
        //        {
        //            foreach (DataRowView AreaRow in AreaTable)
        //            {
        //                AreaNid = AreaRow[Area.AreaNId].ToString();
        //                // -- Get Area Map Layer on the basis of AreaNid
        //                DataRow[] rows = AreaMapLayerTable.Select(Area.AreaNId + "=" + AreaNid);
        //                //AreaMapLayerTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, AreaNid, FieldSelection.Light)).DefaultView.ToTable(true, Area_Map_Layer.StartDate, Area_Map_Layer.EndDate).DefaultView;
                        
        //                // -- Set Area Map Info If data exist for an area.
        //                if (rows.GetUpperBound(0) > -1)
        //                {
        //                    foreach (DataRow Row in rows )
        //                    {
        //                        DataRow TempRow = RetVal.NewRow();
        //                        TempRow.BeginEdit();

        //                        TempRow[Area.AreaID] = AreaRow[Area.AreaID];
        //                        TempRow[Area.AreaName] = AreaRow[Area.AreaName];
        //                        TempRow[Area_Level.AreaLevel] = AreaRow[Area_Level.AreaLevel];
        //                        TempRow[Area_Level.AreaLevelName] = AreaRow[Area_Level.AreaLevelName];

        //                        string GlobalVal = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
        //                        TempRow[Area.AreaGlobal] = GlobalVal;

        //                        DateTime StartDate = Convert.ToDateTime(Information.IsDBNull(Row[Area_Map_Layer.StartDate]) ? string.Empty : Row[Area_Map_Layer.StartDate].ToString().Replace("#", ""));
        //                        TempRow[Area_Map_Layer.StartDate] = StartDate.ToString("dd-MMM-yyyy");

        //                        DateTime EndDate = Convert.ToDateTime(Information.IsDBNull(Row[Area_Map_Layer.EndDate]) ? string.Empty : Row[Area_Map_Layer.EndDate].ToString().Replace("#", ""));

        //                        TempRow[Area_Map_Layer.EndDate] = EndDate.ToString("dd-MMM-yyyy");

        //                        if (this.CurrentSheetType == SummarySheetType.Detailed)
        //                        {
        //                            TempRow[this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
        //                            TempRow[this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
        //                        }
        //                        TempRow.EndEdit();

        //                        RetVal.Rows.Add(TempRow);

        //                    }
        //                }

        //                else
        //                {
        //                    RetVal.ImportRow(AreaRow.Row);

        //                    RetVal.Rows[RetVal.Rows.Count - 1][Area.AreaGlobal] = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
        //                    if (this.CurrentSheetType == SummarySheetType.Detailed)
        //                    {
        //                        RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
        //                        RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            foreach (DataRowView AreaRow in AreaTable)
        //            {
        //                RetVal.ImportRow(AreaRow.Row);

        //                RetVal.Rows[RetVal.Rows.Count - 1][Area.AreaGlobal] = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
        //                if (this.CurrentSheetType == SummarySheetType.Detailed)
        //                {
        //                    RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
        //                    RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    //-- Rename Table
        //    this.RenameAreaTable(ref RetVal);
        //    return RetVal;

        //}



        private DataTable GetAreaTable()
        {
            DataTable RetVal = this.CreateAreaTable();
            DataTable AreaMapLayerTable = null;
            DataView AreaTable = null;
            string AreaNid = string.Empty;

            try
            {
                // -- Fill Subgroup TAble 
                AreaTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaWithLevelName()).DefaultView;
                // -- Sort by Area Name, area ID, Area Map.
                AreaTable.Sort = Area.AreaName + " Asc ," + Area.AreaID + " Asc," + Area.AreaMap + " Asc";

                AreaMapLayerTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, string.Empty, FieldSelection.Light)).DefaultView.ToTable(true, Area.AreaNId, Area_Map_Layer.StartDate, Area_Map_Layer.EndDate);

                if (AreaMapLayerTable.Rows.Count > 0)
                {
                    foreach (DataRowView AreaRow in AreaTable)
                    {
                        AreaNid = AreaRow[Area.AreaNId].ToString();
                        // -- Get Area Map Layer on the basis of AreaNid
                        DataRow[] rows = AreaMapLayerTable.Select(Area.AreaNId + "=" + AreaNid);
                        //AreaMapLayerTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, AreaNid, FieldSelection.Light)).DefaultView.ToTable(true, Area_Map_Layer.StartDate, Area_Map_Layer.EndDate).DefaultView;

                        // -- Set Area Map Info If data exist for an area.
                        if (rows.Length >0)
                        {
                            foreach (DataRow Row in rows)
                            {
                                DataRow TempRow = RetVal.NewRow();
                                TempRow.BeginEdit();

                                TempRow[Area.AreaID] = AreaRow[Area.AreaID];
                                TempRow[Area.AreaName] = AreaRow[Area.AreaName];
                                TempRow[Area_Level.AreaLevel] = AreaRow[Area_Level.AreaLevel];
                                TempRow[Area_Level.AreaLevelName] = AreaRow[Area_Level.AreaLevelName];

                                string GlobalVal = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
                                TempRow[Area.AreaGlobal] = GlobalVal;

                                DateTime StartDate = Convert.ToDateTime(Information.IsDBNull(Row[Area_Map_Layer.StartDate]) ? string.Empty : Row[Area_Map_Layer.StartDate].ToString().Replace("#", ""));
                                TempRow[Area_Map_Layer.StartDate] = StartDate.ToString("dd-MMM-yyyy");

                                DateTime EndDate = Convert.ToDateTime(Information.IsDBNull(Row[Area_Map_Layer.EndDate]) ? string.Empty : Row[Area_Map_Layer.EndDate].ToString().Replace("#", ""));

                                TempRow[Area_Map_Layer.EndDate] = EndDate.ToString("dd-MMM-yyyy");

                                if (this.CurrentSheetType == SummarySheetType.Detailed)
                                {
                                    TempRow[this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
                                    TempRow[this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
                                }
                                TempRow.EndEdit();

                                RetVal.Rows.Add(TempRow);

                            }
                        }

                        else
                        {
                            RetVal.ImportRow(AreaRow.Row);

                            RetVal.Rows[RetVal.Rows.Count - 1][Area.AreaGlobal] = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
                            if (this.CurrentSheetType == SummarySheetType.Detailed)
                            {
                                RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
                                RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRowView AreaRow in AreaTable)
                    {
                        RetVal.ImportRow(AreaRow.Row);

                        RetVal.Rows[RetVal.Rows.Count - 1][Area.AreaGlobal] = this.GetGlobalData(AreaRow[Area.AreaGlobal].ToString());
                        if (this.CurrentSheetType == SummarySheetType.Detailed)
                        {
                            RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.COUNT]] = this.GetDataValueCountForAreaNID((int)AreaRow[Area.AreaNId]);
                            RetVal.Rows[RetVal.Rows.Count - 1][this.ColumnHeader[DSRColumnsHeader.SUM]] = this.GetDataValueSumForAreaNID((int)AreaRow[Area.AreaNId]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            //-- Rename Table
            this.RenameAreaTable(ref RetVal);
            return RetVal;

        }

        /// <summary>
        /// Get Missing Area Table
        /// </summary>
        /// <returns></returns>
        private DataTable GetMissingArea()
        {
            DataTable RetVal = this.CreateMissingAreaTAble();

            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetMissingInfoArea(DatabaseSummaryReportGenerator.DBConnection.ConnectionStringParameters.ServerType)).DefaultView;
            // -- Sort by Map Name
            Table.Sort = Area_Map_Metadata.LayerName + " Asc";

            foreach (DataRowView RowView in Table)
            {
                RetVal.ImportRow(RowView.Row);
            }
            
            RetVal.Columns[Area_Map_Metadata.LayerName].ColumnName = this.ColumnHeader[DSRColumnsHeader.LAYERNAME];

            return RetVal;
        }

        /// <summary>
        /// Create Missing Area TAble
        /// </summary>
        /// <returns></returns>
        private DataTable CreateMissingAreaTAble()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Area_Map_Metadata.LayerName);

            return RetVal;
        }

        /// <summary>
        /// Rename Timeperiod Table
        /// </summary>
        /// <param name="table"></param>
        private void RenameAreaTable(ref DataTable table)
        {

            table.Columns[Area.AreaID].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREAID];
            table.Columns[Area.AreaName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREANAME];
            table.Columns[Area_Level.AreaLevel].ColumnName = this.ColumnHeader[DSRColumnsHeader.LEVEL];
            table.Columns[Area_Level.AreaLevelName].ColumnName = this.ColumnHeader[DSRColumnsHeader.LEVEL_NAME];
            table.Columns[Area.AreaGlobal].ColumnName = this.ColumnHeader[DSRColumnsHeader.GLOBAL];
            table.Columns[Area.AreaMap].ColumnName = this.ColumnHeader[DSRColumnsHeader.LAYERNAME];
            //table.Columns[Area_Map_Layer.StartDate].DataType = Type.GetType("System.String");
            //table.Columns[Area_Map_Layer.EndDate].DataType = Type.GetType("System.String");
            table.Columns[Area_Map_Layer.StartDate].ColumnName = this.ColumnHeader[DSRColumnsHeader.STARTDATE];

            table.Columns[Area_Map_Layer.EndDate].ColumnName = this.ColumnHeader[DSRColumnsHeader.ENDDATE];

            if (this.CurrentSheetType == SummarySheetType.Detailed)
            {
                // -- Set Column For Count and Sum Of DataVAlue
                table.Columns[this.ColumnHeader[DSRColumnsHeader.COUNT]].ColumnName = this.ColumnHeader[DSRColumnsHeader.COUNT] + " " + this.ColumnHeader[DSRColumnsHeader.OF] + " " + this.ColumnHeader[DSRColumnsHeader.DATAVALUE];
                table.Columns[this.ColumnHeader[DSRColumnsHeader.SUM]].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUM] + " " + this.ColumnHeader[DSRColumnsHeader.OF] + " " + this.ColumnHeader[DSRColumnsHeader.DATAVALUE];
            }
        }

        /// <summary>
        /// Get Count of DataValue against AreaNID
        /// </summary>
        /// <param name="areaNid"></param>
        /// <returns></returns>
        private int GetDataValueCountForAreaNID(int areaNid)
        {
            int RetVal;
            RetVal = Convert.ToInt32(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(DatabaseSummaryReportGenerator.DBQueries.TablesName.Data, Data.AreaNId + " = " + areaNid)));
            return RetVal;
        }

        /// <summary>
        /// Get DataValue Sum For an AreaNID
        /// </summary>
        /// <param name="areaNid"></param>
        /// <returns></returns>
        private decimal GetDataValueSumForAreaNID(int areaNid)
        {
            decimal RetVal = 0;

            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Data.GetDataByIUSTimePeriodAreaSource(string.Empty, string.Empty, areaNid.ToString(), string.Empty, FieldSelection.NId)); //SummaryReportQueries.GetDataValuesByAreaNID(areaNid));
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
        /// Get the Global value in Yes and No format.
        /// </summary>
        /// <param name="globalData"></param>
        /// <returns></returns>
        private string GetGlobalData(string globalData)
        {
            string RetVal = string.Empty;
            // Set Global VAlue to Yes or No
            if (!string.IsNullOrEmpty(globalData))
            {
                // -- IF Global value is True Change it TO Yes else No.
                if (globalData.ToUpper() == "TRUE")
                { RetVal = "Yes"; }
                else
                { RetVal = "No"; }
            }
            else
            { RetVal = "No"; }

            return RetVal;

        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create TimePeriod Sheet
        /// </summary>
        /// <param name="excelFile"></param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {

            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.AREA]);
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.AREA]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            excelFile.SetCellValue(sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.SummaryReport.AreaColValueIndex, ColumnHeader[DSRColumnsHeader.AREA]);
            excelFile.SetCellValue(sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.SummaryReport.AreaColValueIndex, ColumnHeader[DSRColumnsHeader.GLOBAL]);
            // -- Get Area Data TAble.
            Table = this.GetAreaTable();
         excelFile.SetColumnFormatType("H:H",sheetNo, SpreadsheetGear.NumberFormatType.Text);
            excelFile.SetColumnFormatType("I:I", sheetNo, SpreadsheetGear.NumberFormatType.Text);
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Area.AreaDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);
            
            int LastRow = Constants.Sheet.Area.AreaDetailsRowIndex + Table.Rows.Count;
                        
            LastRow += 2;
           
            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Area.AreaLastColIndex, true);

            // -- Set Missing Area Header
            excelFile.SetCellValue(sheetNo, LastRow, Constants.HeaderColIndex, this.ColumnHeader[DSRColumnsHeader.MISSINGINFORMATION]);
            //excelFile.GetCellFont(sheetNo, LastRow, Constants.HeaderColIndex).Bold = true;
            excelFile.GetCellFont(sheetNo, LastRow, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            LastRow += 1;
            DataTable MissingTable = null;
            MissingTable = this.GetMissingArea();
            excelFile.LoadDataTableIntoSheet(LastRow, Constants.Sheet.SummaryReport.AreaColIndex, MissingTable, sheetNo, false);

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, LastRow, Constants.HeaderColIndex, LastRow + MissingTable.Rows.Count + 1, Constants.Sheet.Area.AreaIDColIndex, true);

          
            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.OthersColumnWidth, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaIDColIndex, LastRow, Constants.Sheet.Area.AreaIDColIndex);
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.AreaNameColumnWidth, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaNameColIndex, LastRow, Constants.Sheet.Area.AreaNameColIndex);
            // -- autofit Map 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaStartDateColIndex, LastRow, Constants.Sheet.Area.AreaLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaNameColIndex, LastRow, Constants.Sheet.Area.AreaNameColIndex, true);
           

        }

        #endregion

        #endregion



        
    }
}
