using System;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Data.Common;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;
using System.Diagnostics;


namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate Summary Report
    /// </summary>
   public class DatabaseSummaryReportGenerator: IDisposable  
    {
        #region "-- Private --"
        
        #region "-- Variables --"

        private string DestinationFilePath;    // -- Used for Destination File
        private SummarySheetType SheetType = SummarySheetType.Detailed ;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Save Summary Report Excel WorkBook.
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        private void SaveSummaryReport(ref DIExcel excelFile)
        {
            try
            {
                // -- If File exist delete file.
                if (System.IO.File.Exists(this.DestinationFilePath))
                {
                    System.IO.File.Delete(this.DestinationFilePath);
                }
            }
            catch { }
            excelFile.ActiveSheetIndex = 0;
            excelFile.SaveAs(this.DestinationFilePath);

        }

       /// <summary>
       /// USed To Generate Sheet of Summary Report
       /// </summary>
       /// <param name="sheetGenerator">Base Class Object SheetGenerator</param>
       /// <param name="excelFile">Excel File</param>
       /// <param name="progressCounter"> ProgressCounter for Progressbar Increment </param>
       private void GenerateSheet(SheetGenerator sheetGenerator,ref DIExcel excelFile, ref int progressCounter)
       {
           //-- Set Sheet Type For Light OR Heavy
           sheetGenerator.CurrentSheetType = this.SheetType;
           sheetGenerator.GenerateSheet(ref excelFile);
           progressCounter += 1;
           this.RaiseProgressBarIncrement(progressCounter);
       }

        /// <summary>
        /// Open Saved Summary Report Workbook
        /// </summary>      
        private void OpenSummaryReport()
        {
            try
            {
                Process ReportProcess = new Process();
                ProcessStartInfo StartInfo = new ProcessStartInfo(this.DestinationFilePath);
                StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                ReportProcess.StartInfo = StartInfo;
                ReportProcess.Start();               
            }
            catch {}

         
        }

        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise ProgressBar Increment
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
            // --INitilize progressbar and Set ProgressBar Maximum Value to its maximum value.
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

        #region "-- Common Methods --"

        /// <summary>
        /// Connect To Database
        /// </summary>
        private static void ConnectToDataBase()
        {
            string DatasetPrefix;
            string LanguageCode;

            //-- Open and Connect to database  If Database file is Selected
            if (!String.IsNullOrEmpty(DatabaseSummaryReportGenerator._SourceDatabaseNameWPath))
            {
                //--Dispose Connection if it is not null
                if (DatabaseSummaryReportGenerator.DBConnection != null)
                {
                    DatabaseSummaryReportGenerator.DBConnection.Dispose();
                }

                DatabaseSummaryReportGenerator.DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, String.Empty, String.Empty, _SourceDatabaseNameWPath, String.Empty, String.Empty));
                DatasetPrefix = DatabaseSummaryReportGenerator.DBConnection.DIDataSetDefault();
                LanguageCode = DatabaseSummaryReportGenerator.DBConnection.DILanguageCodeDefault(DatasetPrefix);
                DatabaseSummaryReportGenerator.DBQueries = new DIQueries(DatasetPrefix, LanguageCode);

            }
        }

        #endregion

        #endregion
       
        #endregion

        #region "-- Public / Internal--"

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

       internal static DIConnection DBConnection;       //-- Used For Database Connection
       internal static DIQueries DBQueries;             //-- Used For Accesing Queries from DAL
       
        private static  string _FontName;
       /// <summary>
        /// Get and Set Global Font Name for Summary Report
       /// </summary>
       internal static string FontName
        {
            get { return DatabaseSummaryReportGenerator._FontName; }
            set { DatabaseSummaryReportGenerator._FontName = value; }
        }

        /// <summary>
        /// Get and Set Font Size Applied in Summary Report Sheet
        /// </summary>
        private static double _FontSize;
        internal static double FontSize
        {
            get { return DatabaseSummaryReportGenerator._FontSize; }
            set { DatabaseSummaryReportGenerator._FontSize = value; }
        }

        private static string _SourceDatabaseNameWPath;
        /// <summary>
        /// Get Connected Database File path and Set Database Conection
        /// </summary>
        internal static string SourceDatabaseNameWPath
        {
            get
            {
                return DatabaseSummaryReportGenerator._SourceDatabaseNameWPath;
            }
            set
            {
                DatabaseSummaryReportGenerator._SourceDatabaseNameWPath = value;
                if (!String.IsNullOrEmpty(value))
                {
                    DatabaseSummaryReportGenerator.ConnectToDataBase();
                }
            }
        }

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor used to Initilize Summary Report by setting Source Database and Destination File
        /// </summary>
        /// <param name="sourceDatabasePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        public DatabaseSummaryReportGenerator(string sourceDatabasePath, string destinationFilePath, string fontName, double fontSize)
        {
            //-- Connect to Database File
            DatabaseSummaryReportGenerator.SourceDatabaseNameWPath = sourceDatabasePath;
            // -- Set Target File Path
            this.DestinationFilePath = destinationFilePath;
            // --  Set Font Settings
            DatabaseSummaryReportGenerator.FontName = fontName;
            DatabaseSummaryReportGenerator.FontSize = fontSize;
            this.SheetType = SummarySheetType.Detailed;
        }

        /// <summary>
        /// Constructor used to Initilize Summary Report by setting Source Database and Destination File
        /// </summary>
        /// <param name="sourceDatabasePath"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
       public DatabaseSummaryReportGenerator(string sourceDatabasePath, string destinationFilePath, string fontName, double fontSize, SummarySheetType sheetType)
        {
            //-- Connect to Database File
            DatabaseSummaryReportGenerator.SourceDatabaseNameWPath = sourceDatabasePath;
            // -- Set Target File Path
            this.DestinationFilePath = destinationFilePath;
            // --  Set Font Settings
            DatabaseSummaryReportGenerator.FontName = fontName;
            DatabaseSummaryReportGenerator.FontSize = fontSize;
            this.SheetType = sheetType;
        }
     
        /// <summary>
        /// Constructor used to Initilize Summary Report by setting Source Database and Destination File
        /// </summary>
        ///<param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="destinationFilePath"></param>
        /// <param name="fontName"></param>
        /// <param name="fontSize"></param>
        public DatabaseSummaryReportGenerator(DIConnection dbConnection,DIQueries dbQueries, string destinationFilePath, string fontName, double fontSize)
        {
            //-- Set connection objects
            DatabaseSummaryReportGenerator.DBConnection = dbConnection;
            DatabaseSummaryReportGenerator.DBQueries = dbQueries;
            
            // -- Set Target File Path
            this.DestinationFilePath = destinationFilePath;
            // --  Set Font Settings
            DatabaseSummaryReportGenerator.FontName = fontName;
            DatabaseSummaryReportGenerator.FontSize = fontSize;
            this.SheetType = SummarySheetType.Detailed;
        }

       /// <summary>
       /// Constructor used to Initilize Summary Report by setting Source Database and Destination File
       /// </summary>
       /// <param name="dbConnection"></param>
       /// <param name="dbQueries"></param>
       /// <param name="destinationFilePath"></param>
       /// <param name="fontName"></param>
       /// <param name="fontSize"></param>
        /// <param name="sheetType">Heavy with CheckSum/CheckCount OR Light without CheckSum/CheckCount</param>
       public DatabaseSummaryReportGenerator(DIConnection dbConnection, DIQueries dbQueries, string destinationFilePath, string fontName, double fontSize,SummarySheetType sheetType)
       {
           //-- Set connection objects
           DatabaseSummaryReportGenerator.DBConnection = dbConnection;
           DatabaseSummaryReportGenerator.DBQueries = dbQueries;

           // -- Set Target File Path
           this.DestinationFilePath = destinationFilePath;
           // --  Set Font Settings
           DatabaseSummaryReportGenerator.FontName = fontName;
           DatabaseSummaryReportGenerator.FontSize = fontSize;
           this.SheetType = sheetType;
       }


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Generate Excel Summary Report File
        /// </summary>
        public void GenerateSummaryReport()
        {
            int ProgressCounter = 0;
            
            //-- Change Current Culture to English 
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            
            // -- Create DIExcel Objects 
            DIExcel ExcelFile = new DIExcel();

            try
            {
                // Initialize progress bar
                this.RaiseProgressBarInitialize(Constants.SheetsLayout.TotalSummaryReportCount);
                Trace.Write(DateAndTime.Now + "\n");
                this.SaveSummaryReport(ref ExcelFile);
                // -- Create Summary Report Sheet
                this.GenerateSheet(new SummarySheetGenerator(false), ref ExcelFile, ref ProgressCounter);
                Trace.Write("Summary" + DateAndTime.Now + "\n");
                // -- Create Indicator Report Sheet.
                this.GenerateSheet(new IndicatorSheetGenerator(), ref ExcelFile, ref ProgressCounter);
                Trace.Write("IND" + DateAndTime.Now + "\n");
                //-- Create UNit Sheet
                this.GenerateSheet(new UnitSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                Trace.Write("UNit" + DateAndTime.Now + "\n");
                // -- Create SubGroup Sheet
                this.GenerateSheet(new SubgroupSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                Trace.Write("Subgroup" + DateAndTime.Now + "\n");
                // -- Create IUS Sheet
                this.GenerateSheet(new IUSSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                Trace.Write("IUS" + DateAndTime.Now + "\n");
                // -- Create DBVersion Sheet
                this.GenerateSheet(new TimeperiodSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                Trace.Write("TIME" + DateAndTime.Now + "\n");
                // -- Create Area Sheet
                this.GenerateSheet(new AreaSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                Trace.Write("Area" + DateAndTime.Now + "\n");
                DataAdmin.LoadPrefrences();
                // -- Create IC Sector Sheet
                if (DataAdmin.IsSectorVisible)
                {                   
                    this.GenerateSheet(new SectorSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("SECTOR" + DateAndTime.Now + "\n");
                }
                // -- Create Goal Sheet
                if (DataAdmin.IsGoalVisible)
                {
                    this.GenerateSheet(new GoalSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Goal" + DateAndTime.Now + "\n");
                }
                // -- Create FrameWork Sheet
                if (DataAdmin.IsCfVisible)
                {
                    this.GenerateSheet(new FrameworkSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Framwork" + DateAndTime.Now + "\n");
                }
                // -- Create Institution Sheet
                if (DataAdmin.IsInstitiutionVisible)
                {
                    this.GenerateSheet(new InstitutionSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Institute" + DateAndTime.Now + "\n");
                }
                if (DataAdmin.IsThemeVisible)
                {
                    // -- Create Theme Sheet
                    this.GenerateSheet(new ThemeSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Theme" + DateAndTime.Now + "\n");
                }
                // -- Create Convention Sheet 
                if (DataAdmin.IsConventionVisible)
                {
                    this.GenerateSheet(new ConventionSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Convention" + DateAndTime.Now + "\n");
                }

                // -- Create Source Sheet
                if (DataAdmin.IsSourceVisible)
                {
                    this.GenerateSheet(new SourceSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Source" + DateAndTime.Now + "\n");
                }

                if (this.SheetType == SummarySheetType.Detailed)
                {
                    // -- Create IUS Linked To Classes
                    this.GenerateSheet(new IUSLinkedClassesSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("IUSLinkedClassesSheetGenerator" + DateAndTime.Now + "\n");
                    // -- Create IUS Not Linked To Classes
                    this.GenerateSheet(new IUSMissingClassesSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("IUSMissingClassesSheetGenerator" + DateAndTime.Now + "\n");
                    // -- Create "IUSs" without data Sheet
                    this.GenerateSheet(new IUSWithoutDataSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("IUSWithoutDataSheet" + DateAndTime.Now + "\n");
                    // -- Create ICs not used for IUS.
                    this.GenerateSheet(new ClassificationMissingIUSSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("ClassificationMissingIUS" + DateAndTime.Now + "\n");
                    
                    // -- Create Duplicate Data Values
                    this.GenerateSheet(new DuplicateDataSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Duplicatedata" + DateAndTime.Now + "\n");
                               
                    // -- Create Footnotes Shet.
                    this.GenerateSheet(new FootNotesSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Footnote" + DateAndTime.Now + "\n");
                    // -- Create Comments Sheet
                    this.GenerateSheet(new CommentsSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("Comments" + DateAndTime.Now + "\n");
                    //// -- "Time periods" without data
                    //this.GenerateSheet(new TimeperiodSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    //Trace.Write("TimePeriod" + DateAndTime.Now);
                    // -- list of "Sources" without data
                    this.GenerateSheet(new SourcesWithoutDataSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("SourcesWithoutData" + DateAndTime.Now + "\n");
                    // -- "Areas" without data
                    this.GenerateSheet(new AreaWithoutDataSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                    Trace.Write("AreaWithoutData" + DateAndTime.Now + "\n");
                }

                if (DICommon.ISDevInfo6Database(DatabaseSummaryReportGenerator.SourceDatabaseNameWPath))
                {
                    // -- "DBVersion" Sheet
                    this.GenerateSheet(new DBVersionSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                }
                
                Trace.Write("DBVersion" + DateAndTime.Now + "\n");
                // -- Removed On Demand //////////////////////////////////////////////////////////////////
                // -- Create Log Sheet
                //this.GenerateSheet(new DataBaseLogSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                // -- Create Template Log
                //this.GenerateSheet(new TemplateLogSheetGenerator(), ref ExcelFile, ref  ProgressCounter);
                //////////////////////////////////////////////////////////////////////////////////////////

                // -- Save WorkBook 
                this.SaveSummaryReport(ref ExcelFile);

                // -- Close ProgressBar
                this.RaiseProgressBarClose();

                //-- Open Report.
                this.OpenSummaryReport();

            }
            catch (Exception ex)
            {
                this.RaiseProgressBarClose();
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            //--  Change Current Culture to Original
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
        }

       #endregion
       
       #region IDisposable Members

       public void Dispose()
       {
           // -- Dispose DataBase Connection
           if (DatabaseSummaryReportGenerator.DBConnection != null)
               DatabaseSummaryReportGenerator.DBConnection.Dispose();
           DatabaseSummaryReportGenerator.DBQueries = null;
       }

       #endregion

       #endregion


       
    }
}

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Generate DBVersion Sheet of Summary Report
    /// </summary>
    internal class DBVersionSheetGenerator : SheetGenerator
    {

        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Create DBVersion Table.
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateDBVersionTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(DBVersion.VersionNumber);
            RetVal.Columns.Add(DBVersion.VersionChangeDate);
            RetVal.Columns.Add(DBVersion.VersionComments);

            return RetVal;
        }

        /// <summary>
        /// Create DBVersion Table
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetDBVersionTable()
        {
            DataTable RetVal = this.CreateDBVersionTable();
            // -- Fill Subgroup TAble 
            string Query = "SELECT Version_Number, Version_Change_Date, Version_Comments FROM DB_Version" ;

            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(Query).DefaultView; 
            Table.Sort = DBVersion.VersionNumber + " Asc";

            foreach (DataRowView row in Table)
            {
                RetVal.ImportRow(row.Row);
            }
            //-- Rename Table
            this.RenameDBVersionTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Rename DBVersion Table
        /// </summary>
        /// <param name="table">DataTable of DBVersion</param>
        private void RenameDBVersionTable(ref DataTable table)
        {
            // table.Columns[DBVersion.VersionNumber].ColumnName = this.ColumnHeader[DSRColumnsHeader.DBVersion];

        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create DBVersion Sheet of Summary Report
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            // -- Create DBVersion Sheet
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.DATABASE] + " " + this.ColumnHeader[DSRColumnsHeader.VERSION]);

            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, this.ColumnHeader[DSRColumnsHeader.DATABASE] + " " + this.ColumnHeader[DSRColumnsHeader.VERSION]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            Table = this.GetDBVersionTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.DBVersion.DetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.DBVersion.DetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.DBVersion.DetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.DBVersion.LastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.TimePeriodColWidth , Constants.Sheet.DBVersion.DetailsRowIndex, Constants.Sheet.DBVersion.ColValueIndex, LastRow, Constants.Sheet.DBVersion.ColValueIndex);
            
            excelFile.SetHorizontalAlignment(sheetNo, Constants.Sheet.DBVersion.DetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.DBVersion.ColValueIndex, SpreadsheetGear.HAlign.Left);
           
           
        }

        #endregion

        #endregion


    }
}


