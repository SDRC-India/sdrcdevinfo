using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;

using SpreadsheetGear;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Log;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Globalization;
using System.Threading;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;


namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.DES
{
    /// <summary>
    /// Helps in importing records from data entry spreadsheets
    /// </summary>
    public class DataEntrySpreadsheets : DIData
    {
        #region "-- Private --"

        #region "-- Variables --"

        private int WorksheetsCount = 0;
        private string TotalTimeRequired = string.Empty;
        private TimeSpan AvgProcessTime = new TimeSpan();
        private TimeSpan TotalProcessTime = new TimeSpan();
        private DICulture DICultureObject = new DICulture();

        private string DesFileNamePath = string.Empty;
        private DIConnection TargetDBConnection = null;
        private DIQueries TargetDBQueries = null;

        #endregion

        #region "-- Methods --"

        #region "-- Import Excel into Database --"

        /// <summary>
        /// Imports spreadsheetSheet Data directly into TargetDatabase table.
        /// It makes connection with spreadsheet, considering it as another database, and inserting data into another Database's table.
        /// </summary>
        /// <param name="excelFilePath">Excel file Path whose Data is to be imported.</param>
        /// <param name="sheetName">Perticular sheetname of the excel filename.</param>
        private void ImportSheetIntoDatabase(string excelFilePath, string sheetName)
        {
            OleDbConnection Connection;
            OleDbCommand Command;
            string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + @"Data Source=" + excelFilePath + ";" + "Extended Properties=Excel 8.0;";

            try
            {
                Connection = new OleDbConnection(ConnectionString);
                Command = new OleDbCommand();
                Command.Connection = Connection;
                Connection.Open();
                Command.CommandText = @"INSERT INTO [MS Access;Database=" + this.TempTargetFile + ";pwd=" + DAImportCommon.Constants.DBPassword + ";].[" + DAImportCommon.Constants.TempSheetTableName + "](" + Timeperiods.TimePeriod + ", " + Area.AreaID + ", " + Area.AreaName + ", " + Data.DataValue + ", " + SubgroupVals.SubgroupVal + ", " + DAImportCommon.Constants.SourceColumnName + ", " + FootNotes.FootNote + ", " + Data.DataDenominator + ", " + SubgroupVals.SubgroupValGId + ") SELECT Trim(F14) , F2, IIF(LEN(F3)>60, MID(F3,1,60), F3 ), F15, F5, F6, F7,  IIF(LEN(Trim(F8))=0, 0, F8 ), F12 FROM [" + "TempDES" + "$]"; // Time,[Area ID],[Area Name],[Data Value],[Subgroup],[Source],[Footnotes],[Denominator]

                Command.ExecuteNonQuery();
                Connection.Close();

            }
            catch (Exception ex)
            {
                this.UpdateSkippedFileLog(excelFilePath, sheetName, "Invalid Data format (" + ex.Message + ")");

                //ExceptionFacade.ThrowException(ex);
            }

        }



        /// <summary>
        /// Process Source Spreadsheet , and inserts data from each sheet into Database temp Table.
        /// </summary>
        /// <param name="fileNameWPath">Excel fileName with Path.</param>
        private void ImportFrmDESFile(string fileNameWPath)
        {

            DIExcel ExcelFile = null;
            int MaxValueForProgressBar = 0;
            int ProgressBarValue = 0;
            int TotalSheetCount = 0;
            int Index = -1;
            string TimeLeft = string.Empty;
            TimeSpan StartTime;

            try
            {
                // set culture to invariant
                this.DICultureObject.SetInvariantCulture();

                ExcelFile = new DIExcel(fileNameWPath);
                StartTime = DateTime.Now.TimeOfDay;

                //Set progress bar and process info
                MaxValueForProgressBar = ExcelFile.AvailableWorksheetsCount + 4;
                this.HideCommonProcessInfo();
                this.RaiseProgressBarInitializeEvent(MaxValueForProgressBar);
                this.RaiseProcessSourceFileNameEvent(Path.GetFileName(fileNameWPath));

                //create temp sheet table into target file(database/template)
                this.DBConnection.ExecuteNonQuery(this.DAQuery.CreateTempSheetTable());
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                TotalSheetCount = ExcelFile.AvailableWorksheetsCount;
                //Process and import each sheet
                for (int SheetIndex = 0; SheetIndex < TotalSheetCount; SheetIndex++)
                {
                    try
                    {
                        //set process info
                        this.RaiseProcessSheetnameEvent(ExcelFile.GetSheetName(SheetIndex));
                        this.RaiseProcessFileOrSheetNoEvent(SheetIndex, TotalSheetCount);
                        //raise event to display processing time info       
                        this.DisplayProcessingTimeInfo(ref TimeLeft);

                        // Process Spreadsheet and insert desired records into Database
                        //ExcelFile.Close();
                        //ExcelFile = new DIExcel(fileNameWPath);
                        this.ProcessSpreadsheet(ExcelFile, SheetIndex, fileNameWPath);



                        //set progress bar 
                        ProgressBarValue += 1;
                        this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                        //calculate avgProcessTime and totalrequired time
                        this.CalculateTotalRequiredTime(ref StartTime);

                    }
                    catch (Exception ex)
                    {
                        ExceptionFacade.ThrowException(ex);
                    }
                }

                // Drop Tempsheet Table
                this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempSheetTable());

                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                //set process info :hide sheetname, sheet no, indicator Name, unit name
                this.RaiseHideSheetNameEvent();
                this.RaiseHideSheetOrFileNoEvent();
                this.RaiseHideIndicatorNameEvent();
                this.RaiseHideUnitNameEvent();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (ExcelFile != null)
                {
                    ExcelFile.Close();
                }

                // Restore rriginal culture
                this.DICultureObject.RestoreOriginalCulture();
            }
        }

        private void DisplayProcessingTimeInfo(ref string TimeLeft)
        {
            int Index = -1;
            if (!string.IsNullOrEmpty(this.TotalTimeRequired))
            {
                this.TotalProcessTime = this.TotalProcessTime.Subtract(this.AvgProcessTime);
                Index = this.TotalProcessTime.ToString().IndexOf(".");


                if (Index > 0)
                {
                    TimeLeft = LanguageStrings.TimeRemaining + "- " + this.TotalProcessTime.ToString().Substring(0, Index);
                }
                else
                {
                    TimeLeft = LanguageStrings.TimeRemaining + "- " + this.TotalProcessTime.ToString();
                }


                this.RaiseProcessTimeEvent(TimeLeft, this.TotalTimeRequired);
            }

        }

        private void CalculateTotalRequiredTime(ref TimeSpan StartTime)
        {
            int Index = -1;
            if (string.IsNullOrEmpty(this.TotalTimeRequired))
            {
                this.AvgProcessTime = DateTime.Now.TimeOfDay.Subtract(StartTime);
                for (int i = 0; i < this.WorksheetsCount; i++)
                {
                    this.TotalProcessTime = this.TotalProcessTime.Add(this.AvgProcessTime);
                }
                Index = this.TotalProcessTime.ToString().IndexOf(".");
                if (Index > 0)
                {
                    this.TotalTimeRequired = this.TotalProcessTime.ToString().Substring(0, Index);
                }
                else
                {
                    this.TotalTimeRequired = this.TotalProcessTime.ToString();
                }

                //this.TotalTimeRequired = this.TotalProcessTime.ToString().Substring(0, );//this.TotalProcessTime.TotalMinutes.ToString();

            }
        }

        /// <summary>
        /// It reads specified sheetIndex of specified spreadsheet, and inserts Data into TempSheetTable, updates IndicatorName, IndGid, Unitname, UnitGid 
        /// </summary>
        /// <param name="excelFile">ExcelFile to read</param>
        /// <param name="sheetIndex">sheetindex of sheet to read</param>
        /// <param name="fileNameWPath">Excel fineName with path.</param>
        private void ProcessSpreadsheet(DIExcel excelFile, int sheetIndex, string fileNameWPath)
        {
            string IndicatorName = string.Empty;
            string UnitName = string.Empty;
            string SubgroupValName = string.Empty;
            string IndicatorGID = string.Empty;
            string UnitGID = string.Empty;
            string SubgroupValGID = string.Empty;
            string Decimals = string.Empty;
            int DecimalPlace = 0;

            try
            {

                IndicatorName = excelFile.GetCellValue(sheetIndex, DESCellAddress.IndicatorName);
                UnitName = excelFile.GetCellValue(sheetIndex, DESCellAddress.UnitName);
                IndicatorGID = excelFile.GetCellValue(sheetIndex, DESCellAddress.IndicatorGID);
                UnitGID = excelFile.GetCellValue(sheetIndex, DESCellAddress.UnitGID);
                Decimals = excelFile.GetCellValue(sheetIndex, DESCellAddress.Decimals);

                //raise event to display processing information
                this.RaiseProcessIndicatorNameEvent(IndicatorName);
                this.RaiseProcessUnitNameEvent(UnitName);

                if (this.IsValidSpreadsheet(fileNameWPath, excelFile.GetSheetName(sheetIndex), IndicatorName, UnitName, IndicatorGID, UnitGID))
                {
                    //Delete first ten rows
                    DataEntrySpreadsheets.DeleteFirstTenRow(excelFile, sheetIndex);

                    DataEntrySpreadsheets.ProcessTimeperiodInWorksheet(excelFile.GetWorksheet(sheetIndex));
                    ////excelFile.Save();
                    if (string.IsNullOrEmpty(Decimals))
                    {
                        // if decimal value is not available inthe DES, then pass -1 
                        DataEntrySpreadsheets.ProcessDataValueColumnInWorksheet(excelFile.GetWorksheet(sheetIndex), -1);
                    }
                    else
                    {
                        Decimal DecimalsVal = default(Decimal);
                        if (Decimal.TryParse(Decimals, out DecimalsVal))
                        {
                            DataEntrySpreadsheets.ProcessDataValueColumnInWorksheet(excelFile.GetWorksheet(sheetIndex), Convert.ToInt32(Decimals));
                        }
                    }

                    DataEntrySpreadsheets.InsertColumnNameRow(excelFile, sheetIndex);
                    excelFile.Save();

                    // get data table from excel file and insert it into a new temp excel file
                    // which will be used in bulk import process
                    DataTable DESData = excelFile.GetDataTableFromSheet(excelFile.GetSheetName(sheetIndex));
                    for (int ColumnIndex = 0; ColumnIndex < DESData.Columns.Count; ColumnIndex++)
                    {
                        DESData.Columns[ColumnIndex].ColumnName = "F" + (ColumnIndex + 1);

                    }

                    DIExcel TempExcelFile = new DIExcel();
                    string TempDESFileNameWPath = string.Empty;
                    try
                    {

                        TempExcelFile.CreateWorksheet("TempDES");
                        TempExcelFile.LoadDataTableIntoSheet(DESData, "TempDES");
                        TempDESFileNameWPath = Path.Combine(Path.GetDirectoryName(fileNameWPath), "TempDES4Import.xls");
                        TempExcelFile.SaveAs(TempDESFileNameWPath);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        TempExcelFile.Close();
                    }


                    // Close DB connection so that Worksheet can be import by other object.
                    this.DBConnection.Dispose();
                    this.DBQueries = null;

                    //Insert worksheet into tempsheet table                  
                    this.ImportSheetIntoDatabase(TempDESFileNameWPath, excelFile.GetSheetName(sheetIndex));

                    // Connect again Dbconnection.
                    this.ConnectToDatabase();


                    //- Update DataValue column to blank where DataValue is '-' (Dash/Hyphen).
                    // This is done in order to skip '-' Datavalues later
                    if (!string.IsNullOrEmpty(DICommon.DESImportBlankDataValueSymbol))
                    {
                        this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateDataValuesToNullInTempSheet(DICommon.DESImportBlankDataValueSymbol));
                    }

                    // Remove rows which are completely blank i.e. have no data in any column.
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.DeleteBlankRowsFromTempSheet());

                    // get min value of NId column 
                    int MinNIDfound = Convert.ToInt32(Microsoft.VisualBasic.Conversion.Val(Convert.ToString(this.DBConnection.ExecuteScalarSqlQuery("Select Min(NID) from " + DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempSheetTableName))));


                    // Update ExcelFileName[SheetName] in Tempsheet table
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateSourceFileNameInTempSheetTable(Path.GetFileName(fileNameWPath) + "[" + excelFile.GetSheetName(sheetIndex) + "] [", "11 + Nid - " + MinNIDfound));

                    // Removes apostophe from Indicator/ Unit/ Decimals
                    IndicatorName = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(IndicatorName);
                    UnitName = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(UnitName);
                    Decimals = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Decimals);

                    //update indicator, unit and decimal information into TempSheet Table.
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateValuesInTempSheetTable(IndicatorName, UnitName, IndicatorGID, UnitGID, Decimals));


                    //////// Round Data_Value in TempSheetTable upto Decimal places specified.
                    //////if ((!string.IsNullOrEmpty(Decimals)) & (int.TryParse(Decimals, out DecimalPlace)) & (DecimalPlace >= 0))
                    //////{
                    //////    this._DBConnection.ExecuteNonQuery(this.DAQuery.UpdateDataValueToRightDecimalInTempSheetTable(int.Parse(Decimals)));
                    //////}

                    // Insert all rows from TempSheet table into Temp_Data table.
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertTempSheetDataIntoTempdata());

                    // Clear all records of TempsheetTable
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.DeleteFromTempSheetTable());
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }



        #region "-- Spreadhsheet gear --"

        //private void ProcessTimeperiodInWorksheet(IWorksheet worksheet)
        //{
        //    try
        //    {
        //        //copy timeperiods into "N" column  with space 
        //        worksheet.WorkbookSet.Calculation = Calculation.Automatic;

        //        // import N column 
        //        worksheet.Range["N:N"].NumberFormat = "General";
        //        worksheet.Range["N2"].Select();
        //        worksheet.WindowInfo.ActiveCell.FormulaR1C1 = "=RC[-13] & \" \"";
        //        worksheet.Range["N2:N" + Convert.ToString(worksheet.UsedRange.Rows.Count + 1)].CopyDown();
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionFacade.ThrowException(ex);
        //    }

        //}

        /////// <summary>
        /////// It copies all column values from "D" column (DataValue) to "O" column as Text..
        /////// </summary>
        /////// <param name="iWorksheet">zero based sheet index</param>
        ////private void ProcessDataValueColumnInWorksheet(IWorksheet worksheet)
        ////{
        ////    try
        ////    {
        ////        //copy DataValue column into "O" column  with space 
        ////        worksheet.WorkbookSet.Calculation = Calculation.Automatic;

        ////        // import N column 
        ////        worksheet.Range["O:O"].NumberFormat = "General";
        ////        worksheet.Range["O2"].Select();
        ////        worksheet.WindowInfo.ActiveCell.FormulaR1C1 = "=RC[-11] & \" \"";
        ////        worksheet.Range["O2:O" + Convert.ToString(worksheet.UsedRange.Rows.Count +1)].CopyDown();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionFacade.ThrowException(ex);
        ////    }
        ////}

        #endregion

        /// <summary>
        /// It validates that whether sheet is valid as standard DES for DevInfo Database.
        /// It should contain valid Indicatorname, IndicatorGid, Unit name, UnitGid.
        /// </summary>
        private bool IsValidSpreadsheet(string fileName, string sheetName, string indicatorName, string unitName, string indicatorGID, string unitGID)
        {
            bool RetVal = false;
            try
            {
                if ((string.IsNullOrEmpty(indicatorName) & string.IsNullOrEmpty(indicatorGID)))
                {
                    this.UpdateSkippedFileLog(fileName, sheetName, "Indicator Not Found");
                    RetVal = false;
                }

                else if (string.IsNullOrEmpty(unitName) & string.IsNullOrEmpty(unitGID))
                {
                    this.UpdateSkippedFileLog(fileName, sheetName, "Unit Not Found");
                    RetVal = false;
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        ////private void DeleteFirstTenRow(DIExcel excelFile, int sheetIndex)
        ////{
        ////    try
        ////    {
        ////        for (int RowIndex = 1; RowIndex <= 9; RowIndex++)
        ////        {

        ////            excelFile.DeleteRowAt(sheetIndex, 1);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionFacade.ThrowException(ex);
        ////    }
        ////}

        ////private void InsertColumnNameRow(DIExcel excelFile, int sheetIndex)
        ////{
        ////    try
        ////    {
        ////        excelFile.SetCellValue(sheetIndex, 0, 0, "F1");
        ////        excelFile.SetCellValue(sheetIndex, 0, 1, "F2");
        ////        excelFile.SetCellValue(sheetIndex, 0, 2, "F3");
        ////        excelFile.SetCellValue(sheetIndex, 0, 3, "F4");
        ////        excelFile.SetCellValue(sheetIndex, 0, 4, "F5");
        ////        excelFile.SetCellValue(sheetIndex, 0, 5, "F6");
        ////        excelFile.SetCellValue(sheetIndex, 0, 6, "F7");
        ////        excelFile.SetCellValue(sheetIndex, 0, 7, "F8");
        ////        excelFile.SetCellValue(sheetIndex, 0, 8, "F9");
        ////        excelFile.SetCellValue(sheetIndex, 0, 9, "F10");
        ////        excelFile.SetCellValue(sheetIndex, 0, 10, "F11");
        ////        excelFile.SetCellValue(sheetIndex, 0, 11, "F12");
        ////        excelFile.SetCellValue(sheetIndex, 0, 12, "F13");
        ////        excelFile.SetCellValue(sheetIndex, 0, 13, "F14");

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ExceptionFacade.ThrowException(ex);
        ////    }
        ////}

        #endregion

        #endregion

        #endregion

        #region "-- Internal --"

        #region"-- New/Dispose --"

        internal DataEntrySpreadsheets(List<string> dataEntrySpreadsheetFiles, string targetFileNameWPath, string tempFolderPath, string htmlLogOutputPath)
        {
            this.SetBasicPropertiesAndProcessValues(dataEntrySpreadsheetFiles, targetFileNameWPath, tempFolderPath, htmlLogOutputPath);

        }

        internal DataEntrySpreadsheets(string desFileNameWPath, string tempFolder, DIConnection dbConnection, DIQueries dbQueries)
        {
            this.TempFolderPath = tempFolder;
            this.DesFileNamePath = desFileNameWPath;
            this.TargetDBConnection = dbConnection;
            this.TargetDBQueries = dbQueries;
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        /// <summary>
        /// It reads Source DES files one by one, and reads Data from each workbook and inserts all sheet's Data into TempDataTable.
        /// </summary>
        protected override void ProcessTempDataTable()
        {
            int TotalFileCount = this.TempFiles.Count;
            int CurrentFileNo = 1;

            //step1: Count total worksheets to display processing time information. 
            this.CountTotalWorksheets();

            //step2: Process and import data from excel file into target database/template.
            foreach (string DESFileNameWPath in this.TempFiles)
            {
                //raise event  to display processing information
                this.RaiseProcessFileNoEvent(CurrentFileNo, TotalFileCount);
                CurrentFileNo += 1;

                //import file
                this.ImportFrmDESFile(DESFileNameWPath);

            }

            //hide file no information 
            this.RaiseHideFileNoEvent();

            // get skipped records 
            this.DataSkippedTable = this.GetSkippedRecordsDataTableForLog(Data.DataValue);

            this.AreaSkippedTable = this.GetSkippedRecordsDataTableForLog(Area.AreaID);

            this.SubgroupSkippedTable = this.GetSkippedRecordsDataTableForLog(SubgroupVals.SubgroupVal);
            this.SourceSkippedTable = this.GetSkippedRecordsDataTableForLog(Common.Constants.Log.DuplicateSourceColumnName);


            // Step 3: Remove all rows from Temp_Data table where Data Value is null or blank, or I, U, S are blank.
            this.DBConnection.ExecuteNonQuery(DAQuery.RemoveNullRecordsFromTempDataTable());

            //Get all Invalid timeperiod
            try
            {
                // Step 4: Save all invalid timeperiods into InvalidTimeperiodTable
                this.InvalidTimeperiodTable = this.DBConnection.ExecuteDataTable(DAQuery.GetInvalidTimeperiods());

                // Step 5: Delete all records where  timeperiod is null
                this.DBConnection.ExecuteNonQuery(DAQuery.DeleteInvalidTimeperiods());

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            this.RaiseProcessTimeEvent(string.Empty, string.Empty);

            //Get all Invalid Source
            try
            {
                // Step 6: Save all invalid source into InvalidSourceTable
                this.InvalidSourceTable = this.DBConnection.ExecuteDataTable(DAQuery.GetInvalidSource(Common.Constants.PublisherMaxLength));

                // Step 7: Delete all records where Source length is greater than 100
                this.DBConnection.ExecuteNonQuery(DAQuery.UpdateInvalidSourceWithNull(Common.Constants.PublisherMaxLength));

            }
            catch (Exception ex)
            {
                
            }

            this.RaiseProcessTimeEvent(string.Empty, string.Empty);

        }

        private DataTable GetSkippedRecordsDataTableForLog(string columnName)
        {
            DataTable RetVal = null;

            try
            {
                RetVal = this.DBConnection.ExecuteDataTable("Select " + DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.Log.SkippedSourceFileColumnName + " FROM " + Common.Constants.TempDataTableName + " WHERE " + columnName + " Is Null OR " + columnName + " = ''");
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private void CountTotalWorksheets()
        {
            foreach (string FileNameWPath in this.TempFiles)
            {
                DIExcel ExcelFile = null;
                try
                {
                    ExcelFile = new DIExcel(FileNameWPath);
                    this.WorksheetsCount += ExcelFile.AvailableWorksheetsCount;
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (ExcelFile != null)
                        ExcelFile.Close();
                }


            }
        }

        protected override void AddNotesAssistants()
        {
            // Do Nothing in case of spreadsheet

        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Upload DES to Database --"

        protected override bool UploadDatabase()
        {
            bool RetVal = false;

            DataTable DESTable = null;

            IndicatorBuilder IndicatorBuilderObj = null;
            IUSBuilder IUSBuilderObj = null;
            AreaBuilder AreaBuilderObj = null;
            SourceBuilder SourceBuilderObj = null;
            TimeperiodBuilder TimeperiodBuilderObj = null;
            FootnoteBuilder FootnoteBuilderObj = null;

            Dictionary<string, int> AreaList = new Dictionary<string, int>();
            Dictionary<string, int> SourceList = new Dictionary<string, int>();
            Dictionary<string, int> TimeperiodList = new Dictionary<string, int>();
            Dictionary<string, int> IUSList = new Dictionary<string, int>();

            int AreaNId = 0;
            int SourceNId = 0;
            int TimeperiodNId = 0;
            int IUSNId = 0;
            string TempExcelFileNameWithPath = string.Empty;
            string IndicatorGID = string.Empty;
            string UnitGID = string.Empty;
            string SubgroupGID = string.Empty;
            string IndicatorName = string.Empty;
            string UnitName = string.Empty;
            string SubgroupName = string.Empty;
            string IUSString = string.Empty;
            string Datavalue = string.Empty;
            int FootNotesNId = -1;
            string DataDenominator = string.Empty;
            int TotalSheetCount = 0;
            int CurrentSheetNo = 0;
            int Counter = 0;

            try
            {
                DIExcel ExcelFile = new DIExcel(this.DesFileNamePath);
                TotalSheetCount = ExcelFile.GetAllSheetsName().Count;
                ExcelFile.Close();

                //-- Copy File INto Temp Location
                TempExcelFileNameWithPath = this.CopyExcelFileTOTempFolder(this.DesFileNamePath);

                //-- GET DataTable from DES File
                DESTable = DevInfo.Lib.DI_LibBAL.Import.DAImport.DES.DataEntrySpreadsheets.GetDataTableForAllDESSheets(TempExcelFileNameWithPath);

                DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase DIDatabaseObj = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(TargetDBConnection, TargetDBQueries);

                //-- Initialize Progress
                this.RaiseInitilizeProcessEvent(Path.GetFileName(this.DesFileNamePath), TotalSheetCount, 100);

                if (DESTable.Rows.Count > 0)
                {
                    IndicatorBuilderObj = new IndicatorBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    IUSBuilderObj = new IUSBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    AreaBuilderObj = new AreaBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    SourceBuilderObj = new SourceBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    TimeperiodBuilderObj = new TimeperiodBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    FootnoteBuilderObj = new FootnoteBuilder(this.TargetDBConnection, this.TargetDBQueries);
                }

                foreach (DataRow Row in DESTable.Rows)
                {
                    //-- GET AREANID
                    if (AreaList.ContainsKey(Convert.ToString(Row[Area.AreaID])))
                    {
                        AreaNId = AreaList[Convert.ToString(Row[Area.AreaID])];
                    }
                    else
                    {
                        AreaNId = AreaBuilderObj.GetAreaNidByAreaID(Convert.ToString(Row[Area.AreaID]));
                        AreaList.Add(Convert.ToString(Row[Area.AreaID]), AreaNId);
                    }

                    if (AreaNId > 0)
                    {
                        //-- GET SourceNID
                        if (SourceList.ContainsKey(Convert.ToString(Row[IndicatorClassifications.ICName])))
                        {
                            SourceNId = SourceList[Convert.ToString(Row[IndicatorClassifications.ICName])];
                        }
                        else
                        {
                            SourceNId = SourceBuilderObj.CheckNCreateSource(Convert.ToString(Row[IndicatorClassifications.ICName]));
                            SourceList.Add(Convert.ToString(Row[IndicatorClassifications.ICName]), SourceNId);
                        }

                        //-- GET TIMEPERIOD NID
                        if (TimeperiodList.ContainsKey(Convert.ToString(Row[Timeperiods.TimePeriod])))
                        {
                            TimeperiodNId = TimeperiodList[Convert.ToString(Row[Timeperiods.TimePeriod])];
                        }
                        else
                        {
                            TimeperiodNId = TimeperiodBuilderObj.CheckNCreateTimeperiod(Convert.ToString(Row[Timeperiods.TimePeriod]));
                            TimeperiodList.Add(Convert.ToString(Row[Timeperiods.TimePeriod]), TimeperiodNId);
                        }

                        if (IndicatorName != Convert.ToString(Row[Indicator.IndicatorName]))
                        {
                            CurrentSheetNo++;
                            this.RaiseSetProcessEvent(Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Unit.UnitName]), CurrentSheetNo);
                        }

                        //-- GET IUS Details
                        IndicatorGID = Convert.ToString(Row[Indicator.IndicatorGId]);
                        UnitGID = Convert.ToString(Row[Unit.UnitGId]);
                        SubgroupGID = Convert.ToString(Row[SubgroupVals.SubgroupValGId]);
                        IndicatorName = Convert.ToString(Row[Indicator.IndicatorName]);
                        UnitName = Convert.ToString(Row[Unit.UnitName]);
                        SubgroupName = Convert.ToString(Row[SubgroupVals.SubgroupVal]);

                        IUSString = IndicatorGID + "~" + UnitGID + "~" + SubgroupGID + "~" + IndicatorName + "~" + UnitName + "~" + SubgroupName;
                        //-- Get IUSNID from Database
                        if (IUSList.ContainsKey(IUSString))
                        {
                            IUSNId = IUSList[IUSString];
                        }
                        else
                        {
                            IUSNId = IUSBuilderObj.GetIUSNid(IndicatorGID, UnitGID, SubgroupGID, IndicatorName, UnitName, SubgroupName);
                            if (IUSNId <= 0)
                            {
                                this.RaiseSkippedIUSEvent(IndicatorName, UnitName, SubgroupName, IndicatorGID, UnitGID, SubgroupGID);
                            }

                            IUSList.Add(IUSString, IUSNId);
                        }

                        if (SourceNId > 0 && TimeperiodNId > 0 && IUSNId > 0)
                        {
                            //-- Getv Data Value
                            Datavalue = Convert.ToString(Row[Data.DataValue]);

                            //-- Check n Create Footnote
                            FootNotesNId = FootnoteBuilderObj.CheckNCreateFoonote(Convert.ToString(Row[FootNotes.FootNote]));
                            DataDenominator = Convert.ToString(Row[Data.DataDenominator]);
                            DataDenominator = string.IsNullOrEmpty(DataDenominator) ? "0" : DataDenominator;

                            if (!string.IsNullOrEmpty(Datavalue))
                            {
                                //-- Check and Update Data Value
                                DIDatabaseObj.CheckNCreateData(AreaNId, IUSNId, SourceNId, TimeperiodNId, Datavalue, FootNotesNId, Convert.ToInt32(DataDenominator));
                            }
                        }
                    }

                    Counter++;
                    this.RaiseProgressBarIncrement(Convert.ToInt32((Counter * 100) / DESTable.Rows.Count));
                    RetVal = true;
                }
                DIDatabaseObj.UpdateIndicatorUnitSubgroupNIDsInData();
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }


            return RetVal;
        }

        /// <summary>
        /// Upload DES to Database
        /// </summary>
        /// <param name="desFileNamePath">USE Temp Excel File</param>
        /// <param name="targetDBConnection"></param>
        /// <param name="targetDBQueries"></param>
        /// <returns></returns>
        internal bool UploadDESToDatabase(string desFileNamePath, DIConnection targetDBConnection, DIQueries targetDBQueries, string tempFolderPath)
        {
            bool RetVal = false;

            //DataTable DESTable = null;

            //IndicatorBuilder IndicatorBuilderObj = null;
            //IUSBuilder IUSBuilderObj = null;
            //AreaBuilder AreaBuilderObj = null;
            //SourceBuilder SourceBuilderObj = null;
            //TimeperiodBuilder TimeperiodBuilderObj = null;
            //FootnoteBuilder FootnoteBuilderObj = null;

            //Dictionary<string, int> AreaList = new Dictionary<string, int>();
            //Dictionary<string, int> SourceList = new Dictionary<string, int>();
            //Dictionary<string, int> TimeperiodList = new Dictionary<string, int>();
            //Dictionary<string, int> IUSList = new Dictionary<string, int>();

            //int AreaNId = 0;
            //int SourceNId = 0;
            //int TimeperiodNId = 0;
            //int IUSNId = 0;
            //string TempExcelFileNameWithPath = string.Empty;
            //string IndicatorGID = string.Empty;
            //string UnitGID = string.Empty;
            //string SubgroupGID = string.Empty;
            //string IndicatorName = string.Empty;
            //string UnitName = string.Empty;
            //string SubgroupName = string.Empty;
            //string IUSString = string.Empty;
            //string Datavalue = string.Empty;
            //int FootNotesNId = -1;
            //string DataDenominator = string.Empty;
            //int TotalSheetCount = 0;
            //int CurrentSheetNo = 0;
            //int Counter = 0;

            //try
            //{
            //    DIExcel ExcelFile = new DIExcel(desFileNamePath);
            //    TotalSheetCount = ExcelFile.GetAllSheetsName().Count;
            //    ExcelFile.Close();

            //    //-- Copy File INto Temp Location
            //    TempExcelFileNameWithPath = this.CopyExcelFileTOTempFolder(desFileNamePath);

            //    //-- GET DataTable from DES File
            //    DESTable = DevInfo.Lib.DI_LibBAL.Import.DAImport.DES.DataEntrySpreadsheets.GetDataTableForAllDESSheets(TempExcelFileNameWithPath);

            //    DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase DIDatabaseObj = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(targetDBConnection, targetDBQueries);

            //    //-- Initialize Progress
            //    this.RaiseInitilizeProcessEvent(Path.GetFileName(desFileNamePath), TotalSheetCount, 100);

            //    if (DESTable.Rows.Count > 0)
            //    {
            //        IndicatorBuilderObj = new IndicatorBuilder(targetDBConnection, targetDBQueries);
            //        IUSBuilderObj = new IUSBuilder(targetDBConnection, targetDBQueries);
            //        AreaBuilderObj = new AreaBuilder(targetDBConnection, targetDBQueries);
            //        SourceBuilderObj = new SourceBuilder(targetDBConnection, targetDBQueries);
            //        TimeperiodBuilderObj = new TimeperiodBuilder(targetDBConnection, targetDBQueries);
            //        FootnoteBuilderObj = new FootnoteBuilder(targetDBConnection, targetDBQueries);
            //    }

            //    foreach (DataRow Row in DESTable.Rows)
            //    {
            //        //-- GET AREANID
            //        if (AreaList.ContainsKey(Convert.ToString(Row[Area.AreaID])))
            //        {
            //            AreaNId = AreaList[Convert.ToString(Row[Area.AreaID])];
            //        }
            //        else
            //        {
            //            AreaNId = AreaBuilderObj.GetAreaNidByAreaID(Convert.ToString(Row[Area.AreaID]));
            //            AreaList.Add(Convert.ToString(Row[Area.AreaID]), AreaNId);
            //        }

            //        if (AreaNId > 0)
            //        {
            //            //-- GET SourceNID
            //            if (SourceList.ContainsKey(Convert.ToString(Row[IndicatorClassifications.ICName])))
            //            {
            //                SourceNId = SourceList[Convert.ToString(Row[IndicatorClassifications.ICName])];
            //            }
            //            else
            //            {
            //                SourceNId = SourceBuilderObj.CheckNCreateSource(Convert.ToString(Row[IndicatorClassifications.ICName]));
            //                SourceList.Add(Convert.ToString(Row[IndicatorClassifications.ICName]), SourceNId);
            //            }

            //            //-- GET TIMEPERIOD NID
            //            if (TimeperiodList.ContainsKey(Convert.ToString(Row[Timeperiods.TimePeriod])))
            //            {
            //                TimeperiodNId = TimeperiodList[Convert.ToString(Row[Timeperiods.TimePeriod])];
            //            }
            //            else
            //            {
            //                TimeperiodNId = TimeperiodBuilderObj.CheckNCreateTimeperiod(Convert.ToString(Row[Timeperiods.TimePeriod]));
            //                TimeperiodList.Add(Convert.ToString(Row[Timeperiods.TimePeriod]), TimeperiodNId);
            //            }

            //            if (IndicatorName != Convert.ToString(Row[Indicator.IndicatorName]))
            //            {
            //                CurrentSheetNo++;
            //                this.RaiseSetProcessEvent(Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Unit.UnitName]), CurrentSheetNo);
            //            }

            //            //-- GET IUS Details
            //            IndicatorGID = Convert.ToString(Row[Indicator.IndicatorGId]);
            //            UnitGID = Convert.ToString(Row[Unit.UnitGId]);
            //            SubgroupGID = Convert.ToString(Row[SubgroupVals.SubgroupValGId]);
            //            IndicatorName = Convert.ToString(Row[Indicator.IndicatorName]);
            //            UnitName = Convert.ToString(Row[Unit.UnitName]);
            //            SubgroupName = Convert.ToString(Row[SubgroupVals.SubgroupVal]);

            //            IUSString = IndicatorGID + "~" + UnitGID + "~" + SubgroupGID + "~" + IndicatorName + "~" + UnitName + "~" + SubgroupName;
            //            //-- Get IUSNID from Database
            //            if (IUSList.ContainsKey(IUSString))
            //            {
            //                IUSNId = IUSList[IUSString];
            //            }
            //            else
            //            {
            //                IUSNId = IUSBuilderObj.GetIUSNid(IndicatorGID, UnitGID, SubgroupGID, IndicatorName, UnitName, SubgroupName);
            //                IUSList.Add(IUSString, IUSNId);
            //            }

            //            if (SourceNId > 0 && TimeperiodNId > 0 && IUSNId > 0)
            //            {
            //                //-- Getv Data Value
            //                Datavalue = Convert.ToString(Row[Data.DataValue]);

            //                //-- Check n Create Footnote
            //                FootNotesNId = FootnoteBuilderObj.CheckNCreateFoonote(Convert.ToString(Row[FootNotes.FootNote]));
            //                DataDenominator = Convert.ToString(Row[Data.DataDenominator]);
            //                DataDenominator = string.IsNullOrEmpty(DataDenominator) ? "0" : DataDenominator;

            //                if (!string.IsNullOrEmpty(Datavalue))
            //                {
            //                    //-- Check and Update Data Value
            //                    DIDatabaseObj.CheckNCreateData(AreaNId, IUSNId, SourceNId, TimeperiodNId, Datavalue, FootNotesNId, Convert.ToInt32(DataDenominator));
            //                }
            //            }
            //        }

            //        Counter++;
            //        this.RaiseProgressBarIncrement(Convert.ToInt32((Counter * 100) / DESTable.Rows.Count));
            //        RetVal = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    RetVal = false;
            //    ExceptionFacade.ThrowException(ex);
            //}


            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Static --"

        #region "-- Private Static --"

        private static DataTable GetDESDatatable(DIExcel excelFile, int sheetIndex)
        {
            DataTable RetVal = new DataTable();
            DataTable TempDataTable = null;
            string IndicatorName = string.Empty;
            string UnitName = string.Empty;
            string SubgroupValName = string.Empty;
            string IndicatorGID = string.Empty;
            string UnitGID = string.Empty;
            string SubgroupValGID = string.Empty;
            string Decimals = string.Empty;
            int DecimalPlace = 0;

            try
            {
                IndicatorName = excelFile.GetCellValue(sheetIndex, DESCellAddress.IndicatorName);
                UnitName = excelFile.GetCellValue(sheetIndex, DESCellAddress.UnitName);
                IndicatorGID = excelFile.GetCellValue(sheetIndex, DESCellAddress.IndicatorGID);
                UnitGID = excelFile.GetCellValue(sheetIndex, DESCellAddress.UnitGID);
                Decimals = excelFile.GetCellValue(sheetIndex, DESCellAddress.Decimals);


                if (DataEntrySpreadsheets.IsValidDES(excelFile.GetSheetName(sheetIndex), IndicatorName, UnitName, IndicatorGID, UnitGID))
                {
                    //Delete first ten rows
                    DataEntrySpreadsheets.DeleteFirstTenRow(excelFile, sheetIndex);

                    DataEntrySpreadsheets.ProcessTimeperiodInWorksheet(excelFile.GetWorksheet(sheetIndex));
                    excelFile.Save();

                    if (string.IsNullOrEmpty(Decimals))
                    {
                        DataEntrySpreadsheets.ProcessDataValueColumnInWorksheet(excelFile.GetWorksheet
(sheetIndex), -1);
                    }
                    else
                    {
                        DataEntrySpreadsheets.ProcessDataValueColumnInWorksheet(excelFile.GetWorksheet
   (sheetIndex), Convert.ToInt32(Decimals));
                    }

                    DataEntrySpreadsheets.InsertColumnNameRow(excelFile, sheetIndex);
                    excelFile.Save();

                    // Removes apostophe from Indicator/ Unit/ Decimals
                    IndicatorName = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(IndicatorName);
                    UnitName = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(UnitName);
                    Decimals = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Decimals);


                    TempDataTable = excelFile.GetDataTableFromSheet(excelFile.GetSheetName(sheetIndex));

                    if (TempDataTable != null)
                    {
                        RetVal.Columns.Add(Indicator.IndicatorName);
                        RetVal.Columns[Indicator.IndicatorName].DefaultValue = IndicatorName;

                        RetVal.Columns.Add(Indicator.IndicatorGId);
                        RetVal.Columns[Indicator.IndicatorGId].DefaultValue = IndicatorGID;

                        RetVal.Columns.Add(Unit.UnitName);
                        RetVal.Columns[Unit.UnitName].DefaultValue = UnitName;

                        RetVal.Columns.Add(Unit.UnitGId);
                        RetVal.Columns[Unit.UnitGId].DefaultValue = UnitGID;

                        TempDataTable.Columns[0].ColumnName = Timeperiods.TimePeriod;
                        TempDataTable.Columns[1].ColumnName = Area.AreaID;
                        TempDataTable.Columns[2].ColumnName = Area.AreaName;
                        TempDataTable.Columns[3].ColumnName = Data.DataValue;
                        TempDataTable.Columns[4].ColumnName = SubgroupVals.SubgroupVal;
                        TempDataTable.Columns[5].ColumnName = IndicatorClassifications.ICName;
                        TempDataTable.Columns[6].ColumnName = FootNotes.FootNote;
                        TempDataTable.Columns[7].ColumnName = Data.DataDenominator;

                        // delete f9,f10,f11
                        TempDataTable.Columns.Remove("F9");
                        TempDataTable.Columns.Remove("F10");
                        TempDataTable.Columns.Remove("F11");
                        TempDataTable.Columns["F12"].ColumnName = SubgroupVals.SubgroupValGId;
                        TempDataTable.Columns.Remove("F13");
                        TempDataTable.Columns.Remove("F14");
                        TempDataTable.Columns.RemoveAt(9);

                        RetVal.Merge(TempDataTable);
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        #region "-- Spreadhsheet gear --"

        private static void ProcessTimeperiodInWorksheet(IWorksheet worksheet)
        {
            try
            {
                //copy timeperiods into "N" column  with space 
                worksheet.WorkbookSet.Calculation = Calculation.Automatic;

                // import N column 
                worksheet.Range["N:N"].NumberFormat = "General";
                worksheet.Range["N2"].Select();
                worksheet.WindowInfo.ActiveCell.FormulaR1C1 = "=RC[-13] & \" \"";
                worksheet.Range["N2:N" + Convert.ToString(worksheet.UsedRange.Rows.Count + 1)].CopyDown();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// It copies all column values from "D" column (DataValue) to "O" column as Text..
        /// </summary>
        /// <param name="iWorksheet">zero based sheet index</param>
        private static void ProcessDataValueColumnInWorksheet(IWorksheet worksheet, int decimalValue)
        {
            try
            {
                //copy DataValue column into "O" column  with space 
                worksheet.WorkbookSet.Calculation = Calculation.Automatic;

                // import N column 
                worksheet.Range["O:O"].NumberFormat = "General";
                worksheet.Range["O2"].Select();

                if (decimalValue >= 0)
                {
                    // only when decimal value is available in DES
                    worksheet.WindowInfo.ActiveCell.FormulaR1C1 = "=IF(ISBLANK(RC[-11]),\"\",IF(ISNUMBER(RC[-11] + 0),ROUND(RC[-11]," + decimalValue + "),RC[-11])) & \" \"";
                }
                else
                {
                    // if decimal value is blank in DES
                    worksheet.WindowInfo.ActiveCell.FormulaR1C1 = "=TRIM(RC[-11] & \" \")";
                }
                worksheet.Range["O2:O" + Convert.ToString(worksheet.UsedRange.Rows.Count + 1)].CopyDown();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        /// <summary>
        /// It validates that whether sheet is valid as standard DES for DevInfo Database.
        /// It should contain valid Indicatorname, IndicatorGid, Unit name, UnitGid.
        /// </summary>
        private static bool IsValidDES(string sheetName, string indicatorName, string unitName, string indicatorGID, string unitGID)
        {
            bool RetVal = false;
            try
            {
                if ((string.IsNullOrEmpty(indicatorName) & string.IsNullOrEmpty(indicatorGID)))
                {
                    RetVal = false;
                }

                else if (string.IsNullOrEmpty(unitName) & string.IsNullOrEmpty(unitGID))
                {
                    RetVal = false;
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        private static void DeleteFirstTenRow(DIExcel excelFile, int sheetIndex)
        {
            try
            {
                for (int RowIndex = 1; RowIndex <= 9; RowIndex++)
                {

                    excelFile.DeleteRowAt(sheetIndex, 1);
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private static void InsertColumnNameRow(DIExcel excelFile, int sheetIndex)
        {
            try
            {
                excelFile.SetCellValue(sheetIndex, 0, 0, "F1");
                excelFile.SetCellValue(sheetIndex, 0, 1, "F2");
                excelFile.SetCellValue(sheetIndex, 0, 2, "F3");
                excelFile.SetCellValue(sheetIndex, 0, 3, "F4");
                excelFile.SetCellValue(sheetIndex, 0, 4, "F5");
                excelFile.SetCellValue(sheetIndex, 0, 5, "F6");
                excelFile.SetCellValue(sheetIndex, 0, 6, "F7");
                excelFile.SetCellValue(sheetIndex, 0, 7, "F8");
                excelFile.SetCellValue(sheetIndex, 0, 8, "F9");
                excelFile.SetCellValue(sheetIndex, 0, 9, "F10");
                excelFile.SetCellValue(sheetIndex, 0, 10, "F11");
                excelFile.SetCellValue(sheetIndex, 0, 11, "F12");
                excelFile.SetCellValue(sheetIndex, 0, 12, "F13");
                excelFile.SetCellValue(sheetIndex, 0, 13, "F14");
                excelFile.SetCellValue(sheetIndex, 0, 14, "F15");

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #region "-- DataTable from DES file --"
        /// <summary>
        /// Returns data table for all DES sheets
        /// </summary>
        /// <param name="fileNameWPath">Excel fileName with Path.</param>
        /// <param name="outputFileNameWPath"></param>
        public static DataTable GetDataTableForAllDESSheets(string fileNameWPath)
        {

            DataTable RetVal = null;
            DIExcel ExcelFile;
            DataTable TempDataTable;
            int TotalSheetCount = 0;
            DICulture DICultureInfo = new DICulture();
            try
            {
                DICultureInfo.SetInvariantCulture();
                ExcelFile = new DIExcel(fileNameWPath);

                // get total sheets count
                TotalSheetCount = ExcelFile.AvailableWorksheetsCount;

                //Process and import each sheet
                for (int SheetIndex = 0; SheetIndex < TotalSheetCount; SheetIndex++)
                {
                    try
                    {
                        // Process Spreadsheet and insert desired records 
                        TempDataTable = DataEntrySpreadsheets.GetDESDatatable(ExcelFile, SheetIndex);

                        if (TempDataTable != null)
                        {
                            if (RetVal != null)
                            {
                                RetVal.Merge(TempDataTable);
                            }
                            else
                            {
                                RetVal = TempDataTable;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionFacade.ThrowException(ex);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DICultureInfo.SetInvariantCulture();
            }

            return RetVal;
        }

        #endregion



        #endregion


    }
}
