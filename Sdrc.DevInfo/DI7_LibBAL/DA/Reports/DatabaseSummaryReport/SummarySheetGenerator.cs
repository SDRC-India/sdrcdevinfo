using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate SummaryReport Sheet of Summary Report
    /// </summary>
    internal class SummarySheetGenerator : SheetGenerator
    {

        #region "-- Private --"

        #region "-- Varibles --"

        private bool ForComparisonReport = false;  // -- Used For Comparison Report
        private int SheetLastRowIndex = 0;
        private DIConnection RefDBConnection;
        private DIConnection TargetDBConnection;
        private string ReferenceDataBaseFile = string.Empty;

        #endregion
        
        #region "-- Method --"

        ///<summary>
        /// Set Summary Report Record Header and Its Value.
        /// </summary>
        /// <param name="ExcelSheet">Excel File</param>
        /// <param name="SheetNo">Sheet No</param>
        private void FeedSummaryReportValues(ref DIExcel excelFile,int sheetNo, int startRowPosition) 
        {
            // -- count - total Records in the Database 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.CountRowIndex, Constants.Sheet.SummaryReport.CountColIndex, Constants.Sheet.SummaryReport.CountColValueIndex, DSRColumnsHeader.COUNT, DatabaseSummaryReportGenerator.DBQueries.TablesName.Data, startRowPosition);

           // -- Total indicators 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.IndicatorRowIndex, Constants.Sheet.SummaryReport.IndicatorColIndex, Constants.Sheet.SummaryReport.IndicatorColValueIndex, DSRColumnsHeader.INDICATOR, DatabaseSummaryReportGenerator.DBQueries.TablesName.Indicator, startRowPosition);

           // -- Total units 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.UnitRowIndex, Constants.Sheet.SummaryReport.UnitColIndex, Constants.Sheet.SummaryReport.UnitColValueIndex, DSRColumnsHeader.UNIT, DatabaseSummaryReportGenerator.DBQueries.TablesName.Unit, startRowPosition);

            // -- Total subgroups 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.SubgroupRowIndex, Constants.Sheet.SummaryReport.TemplateColIndex, Constants.Sheet.SummaryReport.SubgroupColValueIndex, DSRColumnsHeader.SUBGROUP, DatabaseSummaryReportGenerator.DBQueries.TablesName.SubgroupVals, startRowPosition);

            // -- Add Subgroup Type and Subgroup Dimension Info For DI6 Database.
            if (DICommon.ISDevInfo6Database(DatabaseSummaryReportGenerator.SourceDatabaseNameWPath))
            {
                // -- Total subgroups Type
                this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.SubgroupTypeRowIndex, Constants.Sheet.SummaryReport.TemplateColIndex, Constants.Sheet.SummaryReport.SubgroupColValueIndex, DSRColumnsHeader.SUBGROUPTYPE, DatabaseSummaryReportGenerator.DBQueries.TablesName.SubgroupType, startRowPosition);

                // -- Total subgroups Dimension
                this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.SubgroupDimensionRowIndex, Constants.Sheet.SummaryReport.TemplateColIndex, Constants.Sheet.SummaryReport.SubgroupColValueIndex, DSRColumnsHeader.SUBGROUPDIMENSION, DatabaseSummaryReportGenerator.DBQueries.TablesName.Subgroup, startRowPosition);
            
            }

            // -- Total IUS 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.IUSRowIndex, Constants.Sheet.SummaryReport.IUSColIndex, Constants.Sheet.SummaryReport.IUSColValueIndex, DSRColumnsHeader.IUS, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup, startRowPosition);

            // -- Area 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaRowIndex, Constants.Sheet.SummaryReport.AreaColIndex, Constants.Sheet.SummaryReport.AreaColValueIndex, DSRColumnsHeader.AREA, DatabaseSummaryReportGenerator.DBQueries.TablesName.Area, startRowPosition);

            // -- Area total levels 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaRowIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex, DSRColumnsHeader.LEVEL, DatabaseSummaryReportGenerator.DBQueries.TablesName.Area, startRowPosition);

            // --Set TimePeriod 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.TimeRowIndex, Constants.Sheet.SummaryReport.TimeColIndex, Constants.Sheet.SummaryReport.TimeColValueIndex, DSRColumnsHeader.TIMEPERIOD, DatabaseSummaryReportGenerator.DBQueries.TablesName.TimePeriod, startRowPosition);

            // -- Set Minimum Timeperiod 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.TimeRowIndex, Constants.Sheet.SummaryReport.TimeMinMaxColIndex, Constants.Sheet.SummaryReport.TimeMinMaxColValueIndex, DSRColumnsHeader.MIN, DatabaseSummaryReportGenerator.DBQueries.TablesName.TimePeriod, startRowPosition);

            // -- Maximum Timeperiod 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.MaxTimeRowIndex, Constants.Sheet.SummaryReport.TimeMinMaxColIndex, Constants.Sheet.SummaryReport.TimeMinMaxColValueIndex, DSRColumnsHeader.MAX, DatabaseSummaryReportGenerator.DBQueries.TablesName.TimePeriod, startRowPosition);

            // -- Total sources 
            int SourceRowindex = startRowPosition + Constants.Sheet.SummaryReport.SourceRowIndex;
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref SourceRowindex, Constants.Sheet.SummaryReport.SourceColIndex, Constants.Sheet.SummaryReport.SourceColValueIndex, DSRColumnsHeader.SOURCE, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Source);

            // -- Set language  
            this.SetLanguageValues(ref excelFile, sheetNo, startRowPosition + Constants.Sheet.SummaryReport.ICLanguageDefaultIndex, Constants.Sheet.SummaryReport.LanguageColIndex, Constants.Sheet.SummaryReport.LanguageNameColValueIndex, DSRColumnsHeader.LANGUAGE, DatabaseSummaryReportGenerator.DBQueries.TablesName.Language);

            // -- Set Indicator Classification
            this.SetICTotalValues(ref excelFile, sheetNo, startRowPosition);

            try
            {
            // -- Set Column Font Bold
                excelFile.GetRangeFont(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaRowIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex, startRowPosition + Constants.Sheet.SummaryReport.ICLanguageDefaultIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex).Bold = true;
                excelFile.AutoFitColumns(sheetNo, startRowPosition + Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex, startRowPosition + Constants.Sheet.SummaryReport.ICLanguageDefaultIndex, Constants.Sheet.SummaryReport.AreaLevelColIndex); 
            }
            catch
            {}
           

        }

        /// <summary>
        /// Set Language Value into Sheet
        /// </summary>
        /// <param name="ExcelSheet">worksheet in use</param>
        /// <param name="SheetNo">Sheet number</param>
        /// <param name="RowIndex">Row Index</param>
        /// <param name="ColHeaderIndex">Header Column Index</param>
        /// <param name="ColValueIndex">Column Value Index</param>
        /// <param name="HeaderType">Enum for ColumnHeader</param>
        /// <param name="TableName">Table Name</param>
        private void SetLanguageValues(ref DIExcel excelFile, int sheetNo,int rowIndex, int colHeaderIndex, int colValueIndex,DSRColumnsHeader headerType,string tableName)
        {
            int Counter=0;

            //-- Set Record First Value
            excelFile.SetCellValue(sheetNo, rowIndex, colHeaderIndex, base.ColumnHeader[headerType]);

            DataView ReportTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DIQueries.GetLangauges(DatabaseSummaryReportGenerator.DBQueries.DataPrefix)).DefaultView;

            for ( Counter = 0; Counter <= ReportTable.Count - 1; Counter++)
            {
                // -- Make default language font Bold
                if (Convert.ToBoolean(ReportTable[Counter][Language.LanguageDefault]))
                {
                    excelFile.GetCellFont(sheetNo, rowIndex, colValueIndex).Bold = true;
                }
                excelFile.SetCellValue(sheetNo, rowIndex + Counter,colValueIndex,ReportTable[Counter][Language.LanguageName]);
            }
            // -- language count 
            excelFile.SetCellValue(sheetNo, rowIndex, Constants.Sheet.SummaryReport.LanguageColCountValueIndex, ReportTable.Count);
        }

       /// <summary>
       /// Set Indicator Classification SummaryReport Total Values. 
       /// </summary>
       /// <param name="excelFile">Object of Excel Worksheet</param>
       /// <param name="sheetNo">Sheet Number of Current Worksheet</param>
       private void SetICTotalValues(ref DIExcel excelFile, int sheetNo,int startRowIndex)
        {
            int ICRowIndex = 0;

            // -- Count Total Language.
            int LangCount = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DIQueries.GetLangauges(DatabaseSummaryReportGenerator.DBQueries.DataPrefix)).Rows.Count;
            //-- Set Indicator Classification Row Index
            ICRowIndex = startRowIndex + Constants.Sheet.SummaryReport.ICLanguageDefaultIndex + LangCount + 2;
            // -- sector 
            this.SetReportsHeaderValues(ref excelFile, sheetNo,ref ICRowIndex, Constants.Sheet.SummaryReport.LanguageColIndex, Constants.Sheet.SummaryReport.LanguageColCountValueIndex, DSRColumnsHeader.SECTOR, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Sector);
            // -- goal 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref  ICRowIndex, Constants.Sheet.SummaryReport.ICTypeColIndex, Constants.Sheet.SummaryReport.ICTypeColCountValueIndex, DSRColumnsHeader.GOAL, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Goal);
            // -- cf 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref ICRowIndex, Constants.Sheet.SummaryReport.ICTypeColIndex, Constants.Sheet.SummaryReport.ICTypeColCountValueIndex, DSRColumnsHeader.CF, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.CF);
            // -- institution
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref ICRowIndex, Constants.Sheet.SummaryReport.ICTypeColIndex, Constants.Sheet.SummaryReport.ICTypeColCountValueIndex, DSRColumnsHeader.INSTITUTION, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Institution);
            // -- theme 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref ICRowIndex, Constants.Sheet.SummaryReport.ICTypeColIndex, Constants.Sheet.SummaryReport.ICTypeColCountValueIndex, DSRColumnsHeader.THEME, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Theme);
            // -- convention 
            this.SetReportsHeaderValues(ref excelFile, sheetNo, ref ICRowIndex, Constants.Sheet.SummaryReport.ICTypeColIndex, Constants.Sheet.SummaryReport.ICTypeColCountValueIndex, DSRColumnsHeader.CONVENTION, DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications, ICType.Convention);

           // -- Set Last Row Index of Summary Sheet
            this.SheetLastRowIndex = ICRowIndex;
            // -- Apply Font Settings 
            this.SetSummaryReportFontSettings(ref excelFile, sheetNo, ICRowIndex, startRowIndex);
            // -- autofit columns 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.SummaryReport.ICHeaderCellIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex, ICRowIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex);

        }

        /// <summary>
        /// Apply Font Setting for Summary Report Sheet.
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetNo"></param>
        /// <param name="icRow">Last Record RowIndex Of sheet</param>
        private void SetSummaryReportFontSettings(ref DIExcel excelFile, int sheetNo, int icRow, int startRowIndex)
        {
            // -- Apply font settings 
           
            excelFile.GetRangeFont(sheetNo,startRowIndex+ Constants.Sheet.SummaryReport.ICHeaderCellIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex,icRow,Constants.Sheet.SummaryReport.ICHeaderCellIndex ).Bold = true;
            if (DatabaseSummaryReportGenerator.FontName != null)
            {
                excelFile.GetRangeFont(sheetNo, Constants.Sheet.SummaryReport.ICHeaderColValueIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex, icRow, Constants.Sheet.SummaryReport.ICHeaderLastColIndex).Name = DatabaseSummaryReportGenerator.FontName;
                excelFile.GetRangeFont(sheetNo, Constants.Sheet.SummaryReport.ICHeaderColValueIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex, icRow, Constants.Sheet.SummaryReport.ICHeaderLastColIndex).Size = (double)DatabaseSummaryReportGenerator.FontSize;
            }

        }

       /// <summary>
       /// Set Indicator Classification Value For Summary Report
       /// </summary>
       /// <param name="excelFile"></param>
       /// <param name="sheetNo"></param>
       /// <param name="rowIndex"></param>
       /// <param name="colHeaderIndex"></param>
       /// <param name="colValueIndex"></param>
       /// <param name="headerType">Enum for Summary Report Column Header</param>
       /// <param name="tableName"></param>
       /// <param name="icType">Enum Classification Type(ICType)</param>
        private void SetReportsHeaderValues(ref DIExcel excelFile, int sheetNo,ref int rowIndex, int colHeaderIndex, int colValueIndex, DSRColumnsHeader headerType, string tableName, ICType icType)
        {
            string SqlQuery;

            //-- Set Record Header Value
            excelFile.SetCellValue(sheetNo, rowIndex, colHeaderIndex, base.ColumnHeader[headerType]);
            try
            {
                switch (headerType)
                {
                    case DSRColumnsHeader.SECTOR:           //-- Set value for ICs.
                    case DSRColumnsHeader.THEME:
                    case DSRColumnsHeader.SOURCE:
                    case DSRColumnsHeader.GOAL:
                    case DSRColumnsHeader.INSTITUTION:
                    case DSRColumnsHeader.CF:
                    case DSRColumnsHeader.CONVENTION:
                        //--Query is like SELECT count(*) from " + msDBPrefix + "Indicator_Classifications" + msDBLngCode + " WHERE IC_Type='SC'"
                        SqlQuery = DIQueries.GetTableRecordsCount(tableName, IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[icType]);
                        excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery)));
                        colValueIndex += 1;
                        this.SetICLevel(ref excelFile, sheetNo, icType, rowIndex, colValueIndex);
                        break;
                }
                rowIndex += 2;
            }
            catch { }//(Exception ex) throw ex }
        }

        /// <summary>
        /// Set IC Level
        /// </summary>
        /// <param name="eicType">Enum ICTYpe</param>
        private void SetICLevel(ref DIExcel excelSheet, int sheetNo, ICType eicType, int rowIndex, int colValueIndex)
        {
            DataRow[] objRows = null;
            DataRow NewRow = null;
            string[] Temp = null;
            Int32 MaxLevel = 0;

            DataTable Table = base.GetTempTable();
            // -- Get Indicator Classification Value
            DataView TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, eicType, FieldSelection.Light)).DefaultView;
            TempTable.Sort = IndicatorClassifications.ICParent_NId + " ASC, " + IndicatorClassifications.ICName;

            DataTable dt = new DataTable();

            foreach (DataRowView ROW in TempTable)
            {

                NewRow = Table.NewRow();
                // -- Get Records for IC ParentNID
                objRows = Table.Select(Constants.ICTempTableColumns.ID + "=" + ROW[IndicatorClassifications.ICParent_NId].ToString());

                NewRow[Constants.ICTempTableColumns.ID] = ROW[IndicatorClassifications.ICNId];

                NewRow[Constants.ICTempTableColumns.PARENT_ID] = ROW[IndicatorClassifications.ICParent_NId];
                NewRow[Constants.ICTempTableColumns.ACT_LABEL] = ROW[IndicatorClassifications.ICName].ToString();
                // -- Get Level If Any NidID have Parent_NID 
                if (objRows.Length > 0)
                {
                    string[] tempVal = new string[] { Constants.ICTempTableColumns.ICLEVEL_SEPERATOR };
                    Temp = objRows[0][Constants.ICTempTableColumns.LABEL].ToString().Split(tempVal, StringSplitOptions.RemoveEmptyEntries);
                    // -- Set Maximum level if array lengh is greater than MaxLength.
                    if (MaxLevel < Temp.Length)
                    { MaxLevel = Temp.Length; }
                    NewRow[Constants.ICTempTableColumns.LABEL] = objRows[0][Constants.ICTempTableColumns.LABEL].ToString() + Constants.ICTempTableColumns.ICLEVEL_SEPERATOR + ROW[IndicatorClassifications.ICName].ToString();
                }
                else
                { NewRow[Constants.ICTempTableColumns.LABEL] = ROW[IndicatorClassifications.ICName]; }

                Table.Rows.Add(NewRow);
            }

            Table.AcceptChanges();
            int CountICByLevel = 0;
            // -- Set Level  IF Table has records
            if (Table.Rows.Count > 0)
            {
                excelSheet.SetCellValue(sheetNo, rowIndex, colValueIndex, base.ColumnHeader[DSRColumnsHeader.LEVEL]);
                excelSheet.GetRangeFont(sheetNo, rowIndex, colValueIndex, rowIndex, colValueIndex).Bold = true;
                for (int i = 0; i <= MaxLevel; i++)
                {
                    // -- Increase Column Index
                    colValueIndex += 1;
                    excelSheet.SetCellValue(sheetNo, rowIndex, colValueIndex, (i + 1));
                    // -- Increase Row Index for Value
                    rowIndex += 1;
                    CountICByLevel = this.GetCountICLavel(Table, i);
                    // -- Set Lavel Value
                    excelSheet.SetCellValue(sheetNo, rowIndex, colValueIndex, CountICByLevel);
                    // -- Decrease rowindex for next Lavel.
                    rowIndex -= 1;
                }
            }
        }

        /// <summary>
        /// Get Count of Classification Value
        /// </summary>
        /// <param name="table"></param>
        /// <param name="maxLevel">Indicator Classification Maximum Level</param>
        /// <returns></returns>
        private int GetCountICLavel(DataTable table, int maxLevel)
        {
            int RetVal = 0;
            string[] delimiter = new string[] { Constants.ICTempTableColumns.ICLEVEL_SEPERATOR };

            foreach (DataRow row in table.Rows)
            {
                // -- Increase Counter if Label is in supplied level value.
                if (row[Constants.ICTempTableColumns.LABEL].ToString().Split(delimiter, StringSplitOptions.RemoveEmptyEntries).GetUpperBound(0) == maxLevel)
                {
                    RetVal += 1;
                }
            }
            return RetVal;
        }

       /// <summary>
       /// Set Summary Report Count Value Into "Summary Report" WorkSheet 
       /// </summary>
       /// <param name="ExcelSheet"></param>
       /// <param name="SheetNo"></param>
       /// <param name="RowIndex"></param> 
       /// <param name="ColHeaderIndex"></param>
       /// <param name="ColValueIndex"></param>
       /// <param name="HeaderType"></param>
       /// <param name="TableName"></param>
        private void SetReportsHeaderValues(ref DIExcel excelFile,int sheetNo,int rowIndex, int colHeaderIndex, int colValueIndex,DSRColumnsHeader headerType,string tableName,int startRowPosition) 
        {
            string SqlQuery;

            //-- Set Record Header Value
            excelFile.SetCellValue(sheetNo, rowIndex, colHeaderIndex, base.ColumnHeader[headerType]);
            try
            {
                switch (headerType )
                {
                    case DSRColumnsHeader.IUS :

                        SqlQuery = DatabaseSummaryReportGenerator.DBQueries.IUS.GetIUS(FilterFieldType.None,string.Empty,FieldSelection.NId);

                        excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(SqlQuery).Rows.Count));
                        break;
                    case DSRColumnsHeader.LEVEL:

                        SqlQuery = DatabaseSummaryReportGenerator.DBQueries.Area.GetMaxAreaLevelFrmAreaTable();
                        int MaxVal = (int)DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery);
                        int Counter;

                        if (this.CurrentSheetType == SummarySheetType.Detailed)
                        {
                            //-- Set "Count OF DATAVALUE" Cell Value 
                            excelFile.SetCellValue(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelCountDataRowIndex, colHeaderIndex, base.ColumnHeader[DSRColumnsHeader.COUNT] + " " + base.ColumnHeader[DSRColumnsHeader.OF] + " " + base.ColumnHeader[DSRColumnsHeader.DATAVALUE]);

                            //-- Set "SUM OF DATAVALUE" Cell Value 
                            excelFile.SetCellValue(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelSumDataRowIndex, colHeaderIndex, base.ColumnHeader[DSRColumnsHeader.SUM] + " " + base.ColumnHeader[DSRColumnsHeader.OF] + " " + base.ColumnHeader[DSRColumnsHeader.DATAVALUE]);
                        
                        try
                        {
                            excelFile.GetRangeFont(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelCountDataRowIndex, colHeaderIndex, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelCountDataRowIndex, colHeaderIndex).Bold = true;
                            excelFile.GetRangeFont(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelSumDataRowIndex, colHeaderIndex, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelSumDataRowIndex, colHeaderIndex).Bold = true;
                        }
                        catch {}
                       }
                        for (Counter = 1; Counter <= MaxVal; Counter++)
                        {
                            excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex + Counter, Counter);
                            SqlQuery = DIQueries.GetTableRecordsCount(tableName, Convert.ToString(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaLevel + "=" + Counter));
                            excelFile.SetCellValue(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelRowValueIndex, colValueIndex + Counter, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery)));
                            if (this.CurrentSheetType == SummarySheetType.Detailed)
                            {
                                //-- Set Count OF Data VAlue
                                excelFile.SetCellValue(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelCountDataRowIndex, colValueIndex + Counter, this.GetCountOFDataBYAreaLevel(Counter));
                                excelFile.SetCellValue(sheetNo, startRowPosition + Constants.Sheet.SummaryReport.AreaLevelSumDataRowIndex, colValueIndex + Counter, this.GetSumOFDataBYAreaLevel(Counter));
                            }
                         }

                        break;
                    case DSRColumnsHeader.MIN :     //-- Set Value for Minimum Timeperiod.

                        SqlQuery = DIQueries.GetMinValue(tableName,Convert.ToString( DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod),string.Empty);
                        excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery)));
                        break;
                    case DSRColumnsHeader.MAX :     //-- Set Value for Minimum Timeperiod.

                        SqlQuery = DIQueries.GetMaxValue(tableName, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod,string.Empty);
                        excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery)));
                        break;


                    default ://-- Default Process

                        SqlQuery = DIQueries.GetTableRecordsCount(tableName,string.Empty);
                        excelFile.SetCellValue(sheetNo, rowIndex, colValueIndex, Convert.ToString(DatabaseSummaryReportGenerator.DBConnection.ExecuteScalarSqlQuery(SqlQuery)));
                        break;
                }
            }
            catch //(Exception ex)
            {
                //    throw ex;
            }

        }

        /// <summary>
        /// Get Count of Data by Area Level
        /// </summary>
        /// <returns></returns>
        private int GetCountOFDataBYAreaLevel(int level)
        {
            int RetVal = 0;

            try
            {
                RetVal = (int)DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Data.GetDataValuesByAreaLevel(level)).Rows.Count;

            }
            catch (Exception)
            {
                RetVal = 0;
            }
           
            return RetVal;
        }

        /// <summary>
        /// Get Sum Of DataValues by Area Level
        /// </summary>
        /// <param name="level">Area Level</param>
        /// <returns></returns>
        private decimal GetSumOFDataBYAreaLevel(int level)
        {
            decimal RetVal = 0;
            DataTable TempTable = null;

            TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Data.GetDataValuesByAreaLevel(level));
            foreach (DataRow ROW in TempTable.Rows)
            {
                double DataVal;
                //--Add Data Value IF Datavalue is Double
                if (double.TryParse(ROW[Data.DataValue].ToString(), out DataVal))
                {
                    RetVal += (decimal)DataVal;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Generate Summary Sheet For Summary Report
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetNo"></param>
        private void GenerateForSummaryReport(ref DIExcel excelFile,int sheetNo)
        {
            try
            {
                // -- database name
                excelFile.SetCellValue(sheetNo, Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseNameColIndex, base.ColumnHeader[DSRColumnsHeader.DATABASENAME]);
               
                    excelFile.SetCellValue(sheetNo, Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseValueColIndex, DatabaseSummaryReportGenerator.DBConnection.ConnectionStringParameters.DbName );
                               
                // -- Feed Data into Summary Report sheet
                this.FeedSummaryReportValues(ref excelFile, sheetNo, 0);
                 
                //-- Set Header Name 
                excelFile.SetCellValue(sheetNo, Constants.Sheet.SummaryReport.ICHeaderCellIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex, base.ColumnHeader[DSRColumnsHeader.SUMMARY_REPORT] + ":" + Convert.ToString(this.CurrentSheetType==SummarySheetType.Basic? DILanguage.GetLanguageString("BASIC") : DILanguage.GetLanguageString("DETAILED")));
                // -- Heaser Set Font Settings
                excelFile.GetCellFont(sheetNo, Constants.Sheet.SummaryReport.ICHeaderCellIndex, Constants.Sheet.SummaryReport.ICHeaderCellIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            }
            catch //(Exception ex)
            {
                //-- Throw ex;
            } 
        }

        /// <summary>
        /// Generate Summary Sheet For Comparison Report
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetNo"></param>
        private void GenerateForComparisonReport(ref DIExcel excelFile, int sheetNo)
        {
            try
            {
                //-- Set Header Name 
                excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("COMPARISONREPORT"));
              
                // --Set Reference Database name
                DatabaseSummaryReportGenerator.SourceDatabaseNameWPath = this.RefDBConnection.ConnectionStringParameters.DbName;
                
                // -- Set Header Name and Value
                excelFile.SetCellValue(sheetNo, Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseNameColIndex, (base.ColumnHeader[DSRColumnsHeader.REFERENCE] + " " + base.ColumnHeader[DSRColumnsHeader.DATABASENAME]));
                
                excelFile.SetCellValue(sheetNo, Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseValueColIndex, this.ReferenceDataBaseFile);

                // -- Feed Data into Summary Report sheet
                this.FeedSummaryReportValues(ref excelFile, sheetNo, 0);

                // -- Set TArget Database Summary
                DatabaseSummaryReportGenerator.SourceDatabaseNameWPath = this.TargetDBConnection.ConnectionStringParameters.DbName;
                // -- Set Header Name and Value
                excelFile.SetCellValue(sheetNo,this.SheetLastRowIndex + Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseNameColIndex,( base.ColumnHeader[DSRColumnsHeader.TARGET] + " " + base.ColumnHeader[DSRColumnsHeader.DATABASENAME]));
                excelFile.SetCellValue(sheetNo, this.SheetLastRowIndex + Constants.Sheet.SummaryReport.DataBaseNameRowIndex, Constants.Sheet.SummaryReport.DataBaseValueColIndex, DatabaseSummaryReportGenerator.SourceDatabaseNameWPath );

                // -- Feed Data into Summary Report sheet
                this.FeedSummaryReportValues(ref excelFile, sheetNo, this.SheetLastRowIndex);

            }
            catch //(Exception ex)
            {
                //-- Throw ex;
            } 
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Varible --"
        
        /// <summary>
        /// Initilize For Summry OR Comparison Report.
        /// </summary>
        /// <param name="isComparison"></param>
        internal SummarySheetGenerator(bool isComparison)
        {
            this.ForComparisonReport = isComparison;
        }
        internal SummarySheetGenerator(bool isComparison,ref DIConnection dbConnection,ref DIConnection targetDBConnection,string targetFile,string sourceDB)
        {
            this.ForComparisonReport = isComparison;
            this.ReferenceDataBaseFile = sourceDB;
            this.RefDBConnection = dbConnection;
            this.TargetDBConnection = targetDBConnection;
            

        }

        #endregion

        #region "-- Method --"
        
        /// <summary>
        /// GENERATE Summary Report Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            int SheetNo = 0;

            // -- Count Available Sheets in Excel WorkBook
            int Count = excelFile.AvailableWorksheetsCount;
            // -- Rename or Create First existing sheet to "Summary Report"
            if (Count > 0)
            {
               excelFile.RenameWorkSheet(Constants.Sheet.SummaryReport.SummaryReportSheetIndex, base.ColumnHeader[DSRColumnsHeader.SUMMARY]);
            }
            else
            { excelFile.CreateWorksheet(base.ColumnHeader[DSRColumnsHeader.SUMMARY]); }

            //--Get Sheet No of SummaryReport Sheet
            SheetNo = excelFile.GetSheetIndex(base.ColumnHeader[DSRColumnsHeader.SUMMARY]);
            
            // -- Generate Summary Report For Both Comparison and Summary Report as per selection 
            if (ForComparisonReport)
                this.GenerateForComparisonReport(ref excelFile, SheetNo);
            else 
                this.GenerateForSummaryReport(ref excelFile, SheetNo);
           
        }

        #endregion

        #endregion
       
       
    }
}
