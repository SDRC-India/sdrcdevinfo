using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Diagnostics;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Class Used To Generate Comparison Report
    /// </summary>
    public class DataBaseComparisonReportGenerator
    {

        #region "-- private --"

        #region "-- Variables/ Properties --"

        private string TempFolderPath = string.Empty;   // -- Store Temporary Folder Path
        private string RefDBLanguageCode = string.Empty;    // -- Store Reference Database Language Code 
        private string _ReferenceDatabaseNameWPath = string.Empty;  // -- Store Reference Database File Name 
        /// <summary>
        /// Set Reference Database Connection
        /// </summary>
        private string ReferenceDatabaseNameWPath
        {
           set
            {
                this._ReferenceDatabaseNameWPath = value;

                //--Dispose Connection if it is not null
                if (this.ReferenceDBConnection != null)
                {
                    this.ReferenceDBConnection.Dispose();
                }
                //-- Connect TO Database If value is not null.
                if (!String.IsNullOrEmpty(value))
                {
                    this.ConnectToReferenceDataBase();
                }
            }
        }

        private string _TargetDatabaseNameWPath = string.Empty;
        /// <summary>
        /// Set Target Database Connection 
        /// </summary>
        private string TargetDatabaseNameWPath
        {
            set
            {
                this._TargetDatabaseNameWPath = value;
                //--Dispose Connection if it is not null
                if (this.TargetDBConnection != null)
                {
                    this.TargetDBConnection.Dispose();
                }               
                //-- Connect TO Database If value is not null.
                if (!String.IsNullOrEmpty(value))
                {
                    this.ConnectToTargetDataBase();
                }
            }
        }

        #endregion

        #region "-- Methods --"

        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise ProgressBar Increment Event
        /// </summary>
        private void RaiseProgressBarIncrement(int value)
        {
            // -- Set ProgressBar Value If it is initialized.
            if (this.ProgressBar_Increment != null)
                this.ProgressBar_Increment(value);
        }

        /// <summary>
        /// To Initialize ProgressBar 
        /// </summary>
        private void RaiseProgressBarInitialize(int maximumValue)
        {
            // --Initilize progressbar and Set ProgressBar Maximum Value to its maximum value.
            if (this.ProgressBar_Initialize != null)
                this.ProgressBar_Initialize(maximumValue);
        }

        /// <summary>
        /// Raise the progressbar`s close event.
        /// </summary>
        private void RaiseProgressBarClose()
        {
            // -- Close the progressbar
            if (this.ProgressBar_Close != null)
                this.ProgressBar_Close();
        }

          #endregion

        #region "-- Database Connection --"
        
        /// <summary>
        /// Connect To Reference Database
        /// </summary>
        private void ConnectToReferenceDataBase()
        {
            string DatasetPrefix = string.Empty;
            string LanguageCode = string.Empty;

            //-- Open and Connect to database  If Database file is Selected
            if (!String.IsNullOrEmpty(this._ReferenceDatabaseNameWPath))
            {
              
                this.ReferenceDBConnection  = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, String.Empty, String.Empty, this._ReferenceDatabaseNameWPath, String.Empty, String.Empty));
                
                DatasetPrefix = this.ReferenceDBConnection.DIDataSetDefault();
                LanguageCode = "_" + this.RefDBLanguageCode;
                // -- Set Default Language Code If Language Is Not Valid
                if (this.ReferenceDBConnection.IsValidDILanguage(DatasetPrefix,LanguageCode )==false )
                {
                    LanguageCode = this.ReferenceDBConnection.DILanguageCodeDefault(DatasetPrefix);
                }

                DataBaseComparisonReportGenerator.DBQueries = new DIQueries(DatasetPrefix, LanguageCode);

            }
        }

        /// <summary>
        /// Connect To Target Database
        /// </summary>
        private void ConnectToTargetDataBase()
        {
            string DatasetPrefix = string.Empty;
            string LanguageCode = string.Empty;

            //-- Open and Connect to database  If Database file is Selected
            if (!String.IsNullOrEmpty(this._TargetDatabaseNameWPath))
            {
                this.TargetDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, String.Empty, String.Empty, this._TargetDatabaseNameWPath, String.Empty, String.Empty));

                DatasetPrefix = this.TargetDBConnection.DIDataSetDefault();
                LanguageCode = "_" + this.RefDBLanguageCode;
                // -- Set Default Language Code If Language Is Not Valid
                if (this.TargetDBConnection.IsValidDILanguage(DatasetPrefix, LanguageCode) == false)
                {
                    LanguageCode = this.TargetDBConnection.DILanguageCodeDefault(DatasetPrefix);
                }
                DataBaseComparisonReportGenerator.TargetDBQueries = new DIQueries(DatasetPrefix, LanguageCode);

            }
        }

        #endregion

        #region "-- Report Method --"

        //-- Set LanguageName For Selected Language Code
        private void SetLanguage()
        {
            //--check for lang in source db 
            DataView DV = this.ReferenceDBConnection.ExecuteDataTable(DIQueries.GetLangauges(DataBaseComparisonReportGenerator.DBQueries.DataPrefix)).DefaultView;

            DataRow[] oDTRow;
            // -- Get Record OF Current Language
            if (DV.Table.Select(Language.LanguageCode + " = '" + this.RefDBLanguageCode + "'").Length  == 0)
            {
                // -- set the default language from the database 
                oDTRow = DV.Table.Select(Language.LanguageDefault + " = true");
                if (oDTRow.Length > 0)
                {
                    this.ReferenceDBLanguage = oDTRow[0][Language.LanguageName].ToString();
                }
            }
            else
            {
                oDTRow = DV.Table.Select(Language.LanguageCode + " = '" +  DataBaseComparisonReportGenerator.DBQueries.LanguageCode.Replace("_", "") + "'");
               
                 this.ReferenceDBLanguage = oDTRow[0][Language.LanguageName].ToString();
            }

            //--check for lang in target db 
            if (DV.Table.Select(Language.LanguageCode + " = '" + DataBaseComparisonReportGenerator.TargetDBQueries.LanguageCode.Replace("_", "") + "'").Length == 0)
            {
                // -- set the default language from the database 
                oDTRow = DV.Table.Select(Language.LanguageDefault + " = true");
                if (oDTRow.Length > 0)
                {
                    this.TargetDBLanguage =  oDTRow[0][Language.LanguageName].ToString();
                }
            }
            else
            {
                oDTRow = DV.Table.Select(Language.LanguageCode + " = '" + DataBaseComparisonReportGenerator.TargetDBQueries.LanguageCode.Replace("_", "") + "'");
                //-- Set Default Languagee
                this.TargetDBLanguage = oDTRow[0][Language.LanguageName].ToString();
            } 
        }

        /// <summary>
        /// Copy Reference Database file Into Temp Folder
        /// </summary>
        private void CopyReferenceDatabaseIntoTemp()
        {
            // -- Copy Reference DB into TempFolder if it is not null 
            if (!string.IsNullOrEmpty(this._ReferenceDatabaseNameWPath))
            {
                string TempDBPath = this.TempFolderPath + "\\"+  Path.GetFileName(this._ReferenceDatabaseNameWPath);
                try
                {
                    // -- Delete File If already Exist.
                    if (File.Exists(TempDBPath ))
                    {
                        File.Delete(TempDBPath);
                    }
                }
                catch //(Exception)
                {}
                // -- Copy Reference Database File Into TempFolder
                File.Copy(this._ReferenceDatabaseNameWPath,TempDBPath);
                this._ReferenceDatabaseNameWPath = TempDBPath;
            }
        }

        /// <summary>
        /// Connect To Both DataBase
        /// </summary>
        private void ConnectToDataBases()
        {
            //-- Connect To Database
            this.ReferenceDatabaseNameWPath = this._ReferenceDatabaseNameWPath;
            this.TargetDatabaseNameWPath = this._TargetDatabaseNameWPath;
        
        }

        /// <summary>
        /// Import Target DataBase Table Into Reference Database
        /// </summary>
        private void ImportTableIntoRefDatabase()
        {
            
            DbCommand Command;
            try
            {
                DbConnection DBConnection = this.TargetDBConnection.GetCurrentDBProvider().CreateConnection();
                DBConnection.ConnectionString = this.TargetDBConnection.GetConnection().ConnectionString;
                DBConnection.Open();

                //Command = this.TargetDBConnection.GetCurrentDBProvider().CreateCommand();
                //Command.Connection =this.TargetDBConnection.GetCurrentDBProvider().CreateConnection();
                //Command.Connection.ConnectionString = this.TargetDBConnection.GetConnection().ConnectionString;
                //Command.Connection.Open();

                // -- Create TAble Schema For DataBase.
                DataTable SchemaTable = DBConnection.GetSchema(Constants.DBSchema.TABLECOLLECTION );

                foreach (DataRow Row in SchemaTable.Select( Constants.DBSchema.TABLETYPE + " = '" + Constants.DBSchema.TABLE +"'"))
	            {
                    try
                    {
                    // -- Drop Table If already Exist
                     this.ReferenceDBConnection.ExecuteNonQuery( @"DROP TABLE " + Constants.TempTables.TempTablePrefix + Row[Constants.DBSchema.TABLENAME] + "]");

                    //Command.ExecuteNonQuery();
                    }
                    catch {}
                    // -- Import Table from Target DB To Reference DB with prefix Temp_.
                   this.TargetDBConnection.ExecuteNonQuery( @"SELECT *, false as Mapped INTO [MS Access;Database=" + this._ReferenceDatabaseNameWPath + ";pwd=" + Constants.DBSchema.DBPassword  + ";].[" + Constants.TempTables.TempTablePrefix + Row[Constants.DBSchema.TABLENAME] + "]" + " FROM " + Row[Constants.DBSchema.TABLENAME]);

                   // Command.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                //ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Save Summary Report Excel WorkBook.
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        private string SaveComparisonReport(ref DIExcel excelFile)
        {
            string RetVal = string.Empty;
            string FilePath = string.Empty;
            try
            {
                excelFile.ActiveSheetIndex = 0;
                // -- Get File name from Temp folder
                FilePath = Path.Combine(DICommon.DefaultFolder.DefaultSpreadSheetsFolder, Path.GetFileNameWithoutExtension(this.ReferenceDBConnection.ConnectionStringParameters.DbName)) + DICommon.FileExtension.Excel;

                excelFile.SaveAs(FilePath);
                RetVal = FilePath;
                //OpenComparisonReport(FilePath);
            }
            catch { }
            return RetVal;
        }

        /// <summary>
        /// Open Saved Comparison Report Workbook
        /// </summary>      
        private void OpenComparisonReport(string destinationFilePath)
        {
            try
            {
                // -- Open File if exist
                if (File.Exists(destinationFilePath))
                {
                    Process ReportProcess = new Process();
                    ProcessStartInfo StartInfo = new ProcessStartInfo(destinationFilePath);
                    StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    ReportProcess.StartInfo = StartInfo;
                    ReportProcess.Start();
                }
            }
            catch {}
           
        }

        /// <summary>
        /// USed To Generate Sheet of Comparison Report
        /// </summary>
        /// <param name="sheetGenerator">Base Class Object SheetGenerator</param>
        /// <param name="excelFile">Excel File</param>
        /// <param name="progressCounter"> ProgressCounter for Progressbar Increment </param>
        private void GenerateSheet(SheetType sheetType, ref DIExcel excelFile, ref int progressCounter)
        {

            SheetGenerator Generator = new SheetGenerator();
      

            Generator.GenerateSheet(ref excelFile,this.ReferenceDBConnection,DataBaseComparisonReportGenerator.DBQueries,sheetType);
            progressCounter += 1;
            this.RaiseProgressBarIncrement(progressCounter);
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public / Internal --"

        #region "-- Events --"

        /// <summary>
        /// Fires when value of prgressbar is changed.
        /// </summary>
        public event IncrementProgressBar ProgressBar_Increment;

        /// <summary>
        /// Fires when process started to initialize progress bar.
        /// </summary>
        public event InitializeProgressBar ProgressBar_Initialize;

        /// <summary>
        /// Raise when process stop.
        /// </summary>
        public event CloseProgressBar ProgressBar_Close;

        #endregion

        #region "-- Variables and Properties --"

        internal  DIConnection ReferenceDBConnection = null;       //-- Used For Database Connection
        internal  DIConnection TargetDBConnection = null;       //-- Used For Database Connection
        internal static DIQueries DBQueries = null;             //-- Used For Accesing Queries from DAL
        internal static DIQueries TargetDBQueries = null;             //-- Used For Accesing Queries from DAL
        internal  string TargetDBLanguage = string.Empty;         // -- Store TArget Databse Language Name
        internal  string ReferenceDBLanguage = string.Empty;      // -- Store Reference Databse Language Name
        internal  string ReferenceDataBaseFile = string.Empty; // -- Used To Keep Origal Name Of ReferenceDB except TempPAthDB

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor To Initilize Databases
        /// </summary>
        /// <param name="refDatabase">Reference DataBase File path</param>
        /// <param name="targetDatabase">Target Database file path</param>
        public DataBaseComparisonReportGenerator(string refDatabase,string targetDatabase,string tempFolderPath,string languageCode)
        {
            this.ReferenceDataBaseFile = refDatabase;
            this._ReferenceDatabaseNameWPath = refDatabase;
            this._TargetDatabaseNameWPath = targetDatabase;
            this.TempFolderPath = tempFolderPath;
            this.RefDBLanguageCode = languageCode;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Generate Comparison Report Sheet.
        /// </summary>
        /// <returns></returns>
        public bool GenerateComparisonReport()
        {
            bool RetVal = false;
            int ProgressCounter = 0;
          
            // -- Create DIExcel Objects 
            DIExcel ExcelFile = new DIExcel();
            try
            {
                // Initialize progress bar
                this.RaiseProgressBarInitialize(Constants.SheetsLayout.TotalComparisonReportCount);
                ProgressCounter += 1;
                this.RaiseProgressBarIncrement(ProgressCounter);
                // -- Copy Reference Db File Into Temp Folder
                this.CopyReferenceDatabaseIntoTemp();
                // -- Connect To Databases
                this.ConnectToDataBases();

                // -- Import All table from Target DB to Reference Database.
                this.ImportTableIntoRefDatabase();

                // -- Connect To Databases
                this.ConnectToDataBases();

                // -- Save Report TO skip Creation of Book1 file
                this.SaveComparisonReport(ref ExcelFile);

                // -- Generate Summary Sheet
                DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport.SummarySheetGenerator SummaryCreator = new DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport.SummarySheetGenerator(true, ref this.ReferenceDBConnection, ref this.TargetDBConnection, this._TargetDatabaseNameWPath, this.ReferenceDataBaseFile);
                SummaryCreator.GenerateSheet(ref ExcelFile);
                ProgressCounter += 1;
                this.RaiseProgressBarIncrement(ProgressCounter);

                //-- Generate Indicator Sheet
                this.GenerateSheet( SheetType.INDICATOR, ref ExcelFile, ref ProgressCounter);
                // -- Generate Unit Sheet
                this.GenerateSheet( SheetType.UNIT, ref ExcelFile, ref ProgressCounter);

                // -- Generate Subgroup Sheet
                this.GenerateSheet( SheetType.SUBGROUP, ref ExcelFile, ref ProgressCounter);

                // -- Generate IUS Sheet
                this.GenerateSheet( SheetType.IUS, ref ExcelFile, ref ProgressCounter);

                // -- Generate Timeperiod Sheet
                this.GenerateSheet( SheetType.TIMEPERIOD, ref ExcelFile, ref ProgressCounter);

                // -- Generate AREA Sheet
                this.GenerateSheet( SheetType.AREA, ref ExcelFile, ref ProgressCounter);

                // -- Generate IC Sector Sheet
                this.GenerateSheet( SheetType.SECTOR, ref ExcelFile, ref ProgressCounter);
                // -- Generate IC Goal Sheet
                this.GenerateSheet( SheetType.GOAL, ref ExcelFile, ref ProgressCounter);
                // -- Generate IC Institution Sheet
                this.GenerateSheet( SheetType.INSTITUTION , ref ExcelFile, ref ProgressCounter);
                // -- Generate IC Theme Sheet
                this.GenerateSheet( SheetType.THEME, ref ExcelFile, ref ProgressCounter);
                // -- Generate IC Convention Sheet
                this.GenerateSheet( SheetType.CONVENTION, ref ExcelFile, ref ProgressCounter);
                // -- Generate IC Framework Sheet
                this.GenerateSheet(SheetType.CF, ref ExcelFile, ref ProgressCounter);

                // -- Generate Data Sheet
                 this.GenerateSheet( SheetType.DATA, ref ExcelFile, ref ProgressCounter);

                this.OpenComparisonReport( this.SaveComparisonReport(ref ExcelFile));
                RetVal = true;
            }
            catch (Exception ex)
            {
                this.RaiseProgressBarClose();
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (this.ReferenceDBConnection != null) this.ReferenceDBConnection.Dispose();
               if (this.TargetDBConnection != null) this.TargetDBConnection.Dispose();

            }
            try
            {
                //-- Delete Temp File IF File exist.
                if (File.Exists(this._ReferenceDatabaseNameWPath))
                {
                    File.Delete(this._ReferenceDatabaseNameWPath);
                }
            }
            catch {}
           
            this.RaiseProgressBarClose();
            return RetVal;
        }

        #endregion

        #endregion

    }
}



    










