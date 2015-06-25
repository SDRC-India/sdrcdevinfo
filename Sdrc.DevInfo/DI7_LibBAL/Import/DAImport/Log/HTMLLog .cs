using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;


namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Log
{
    /// <summary>
    /// Helps in creating a html log file for import process
    /// </summary>
    internal class HTMLLog
    {
        #region "-- Private --"

        #region "-- Variables / Properties --"

        private int TotalTimeperiod = 0;
        private int TotalSource = 0;
        private int TotalData = 0;

        // DataTables for each (I, U, S, Area,  Duplicate & Skipped records of imported Data)
        private DataTable IndicatorLogInfoTable = null;
        private DataTable UnitLogInfoTable = null;
        private DataTable SubgroupLogInfoTable = null;
        private DataTable AreaLogInfoTable = null;
        private DataTable DuplicateRecordsInfoTable = null;
        private DataTable _SkippedIUSInfoTable = null;

        private DataTable SkippedFiles = null;

        private DataTable _IUSLogInfoTable = null;   //- IUS Log to be used in CSV inport

        #region "-- HTML log variables -- "

        //These varaibles have columnName used in XmlMappedLog file.
        // ColumnName for each Unmatched and Mapped elements. 

        private string MapIndicatorColumnName = "Map_Indicator_Name";
        private string UnmatchedIndicatorColumnName = "Indicator_Name";

        private string MapUnitColumnName = "Map_Unit_Name";
        private string UnmatchedUnitColumnName = "Unit_Name";

        private string MapSubgroupValColumnName = "Map_Subgroup_Val";
        private string UnmatchedSubgroupValColumnName = "Subgroup_Val";

        private string MapAreaColumnName = "Map_Area_Name";
        private string UnmatchedAreaColumnName = "Area_Name";

        private string DuplicateIndicatorColumnName = "Indicator_Name";
        private string DuplicateUnitColumnName = "Unit_Name";
        private string DuplicateSubgroupValColumnName = "Subgroup_Val";
        private string DuplicateTimeperiodColumnName = "TimePeriod";
        private string DuplicateAreaIDColumnName = "Area_ID";
        private string DuplicateSourceColumnName = "Source";

        private string SkippedIndicatorColumnName = "Indicator_name";
        private string SkippedUnitColumnName = "Unit_Name";
        private string SkippedSubgroupValColumnName = "Subgroup_Val";
        private string SkippedTimePeriodColumnName = "TimePeriod";
        private string SkippedSourceColumnName = "IC_Name";
        private string SkippedSourceFileColumnName = "SourceFile";


        //// columns for Skipped files
        //private const string SkippedSheetName = "SheetName";
        //private const string SkippedFileReason = "SkippedReason";


        #endregion

        #endregion

        #region "-- Methods --"

        private string GetLogFileNameWPath()
        {
            string RetVal = string.Empty;
            string OutputDirectoryPath = string.Empty;

            // Check the Directry
            OutputDirectoryPath = this.GetOutputDirectory();

            if (!string.IsNullOrEmpty(OutputDirectoryPath))
            {
                switch (this._ImportType)
                {
                    case DIImportFileType.DataEntrySpreasheet:
                        RetVal = "Spreadsheet";
                        break;
                    case DIImportFileType.Template:
                    case DIImportFileType.Database:
                        RetVal = "Database";
                        break;
                    default:
                        break;
                }

                RetVal = OutputDirectoryPath + "\\Log_Import -" + System.DateTime.Today.ToLongDateString() + "-" + Microsoft.VisualBasic.Strings.Replace(System.DateTime.Now.ToString(), ":", ".", 1, -1, Microsoft.VisualBasic.CompareMethod.Text) + ".html";

                // DELETE the file if it is already exists. 
                if (File.Exists(RetVal))
                {
                    System.IO.File.Delete(RetVal);
                }

            }
            return RetVal;
        }

        private string GetOutputDirectory()
        {
            string RetVal = string.Empty;

            if (Directory.Exists(this._SpreadsheetFolderPath))
            {
                RetVal = this._SpreadsheetFolderPath;
            }
            else if (Directory.Exists(@"C:\DevInfo"))
            {
                RetVal = @"C:\DevInfo\";
            }


            return RetVal;
        }

        protected void SetHTMLLogVariables(Import.DAImport.Common.DIImportFileType importType)
        {
            if (importType == DIImportFileType.SDMXXml)
            {
                this.UnmatchedIndicatorColumnName = "Indicator_GID";


                this.UnmatchedUnitColumnName = "Unit_GID";


                this.UnmatchedSubgroupValColumnName = "Subgroup_Val_GID";


                this.UnmatchedAreaColumnName = "Area_ID";

                this.DuplicateIndicatorColumnName = "Indicator_GID";
                this.DuplicateUnitColumnName = "Unit_GID";
                this.DuplicateSubgroupValColumnName = "Subgroup_Val_GID";




                this.SkippedIndicatorColumnName = "Indicator_GID";
                this.SkippedUnitColumnName = "Unit_GID";
                this.SkippedSubgroupValColumnName = "Subgroup_Val_GID";

            }
            else        // In case of ImportType = Spreadsheet, Database, template
            {
                // values are already Set by default..
                // no change
            }

        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables / Properties --"

        private string _SpreadsheetFolderPath;
        /// <summary>
        /// Sets spreadsheet folder path
        /// </summary>
        internal string SpreadsheetFolderPath
        {
            set
            {
                this._SpreadsheetFolderPath = value;
            }
        }

        private DIImportFileType _ImportType;
        /// <summary>
        /// Sets import type.
        /// </summary>
        internal DIImportFileType ImportType
        {
            set
            {
                this._ImportType = value;
                //Set html log varibale on the basis of importType.
                this.SetHTMLLogVariables(value);

            }
        }

        private string _FontName;
        /// <summary>
        /// Sets font name
        /// </summary>
        internal string FontName
        {
            set
            {
                this._FontName = value;
            }
        }

        private int _FontSize;
        /// <summary>
        /// Sets font size
        /// </summary>
        internal int FontSize
        {
            set
            {
                this._FontSize = value;
            }
        }

        private string _StartTime;
        /// <summary>
        /// Sets start time of import process
        /// </summary>
        internal string StartTime
        {
            set
            {
                this._StartTime = value;
            }
        }

        private string _SourceFolderPath;
        /// <summary>
        /// Sets source folder path
        /// </summary>
        internal string SourceFolderPath
        {
            set
            {
                this._SourceFolderPath = value;
            }
        }

        private string _TargetFilePath;
        /// <summary>
        /// Sets target file path. File name with path which is selected on Step2.
        /// </summary>
        internal string TargetFilePath
        {
            set
            {
                this._TargetFilePath = value;
            }
        }

        private DataTable _InvalidTimeperiodsTable = null;
        /// <summary>
        /// Sets invalid timeperiods table
        /// </summary>
        public DataTable InvalidTimeperiodsTable
        {
            get
            {
                return this._InvalidTimeperiodsTable;
            }
            set
            {
                this._InvalidTimeperiodsTable = value;
            }
        }

        private DataTable _InvalidSourceTable = null;
        /// <summary>
        /// Sets invalid source table
        /// </summary>
        public DataTable InvalidSourceTable
        {
            get
            {
                return this._InvalidSourceTable;
            }
            set
            {
                this._InvalidSourceTable = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DataTable SkippedIUSInfoTable
        {
            get
            {
                return this._SkippedIUSInfoTable;
            }
            set
            {
                this._SkippedIUSInfoTable = value;
            }
        }

        private DataTable _SkippedAreaTable = null;
        /// <summary>
        /// Sets skipped area table
        /// </summary>
        public DataTable SkippedAreaTable
        {
            get
            {
                return this._SkippedAreaTable;
            }
            set
            {
                this._SkippedAreaTable = value;
            }
        }

        private DataTable _SkippedSubgroupTable = null;
        /// <summary>
        /// Sets skipped subgroup table
        /// </summary>
        public DataTable SkippedSubgroupTable
        {
            get
            {
                return this._SkippedSubgroupTable;
            }
            set
            {
                this._SkippedSubgroupTable = value;
            }
        }
        private DataTable _SkippedSourceTable = null;
        /// <summary>
        /// Sets skipped Source table
        /// </summary>
        public DataTable SkippedSourceTable
        {
            get
            {
                return this._SkippedSourceTable;
            }
            set
            {
                this._SkippedSourceTable = value;
            }
        }

        private DataTable _SkippedDataTable = null;
        /// <summary>
        /// Sets skipped data table
        /// </summary>
        public DataTable SkippedDataTable
        {
            get
            {
                return this._SkippedDataTable;
            }
            set
            {
                this._SkippedDataTable = value;
            }
        }

        /// <summary>
        /// gets or sets the IUS Log file.
        /// </summary>
        public DataTable IUSLogInfoTable
        {
            get { return _IUSLogInfoTable; }
            set { _IUSLogInfoTable = value; }
        }

        private string _TargetFileVersionNumber = string.Empty;
        /// <summary>
        /// Gets or sets target file version number
        /// </summary>
        public string TargetFileVersionNumber
        {
            get { return this._TargetFileVersionNumber; }
            set { this._TargetFileVersionNumber = value; }
        }


        #endregion

        #region "-- New/Dispose --"

        internal HTMLLog()
        {
        }

        internal HTMLLog(Import.DAImport.Common.DIImportFileType importType, int totalTimperiod, int totalSource, int totalData,
            DataTable indicatorLogInfoTable, DataTable unitLogInfoTable, DataTable subgroupLogInfoTable,
            DataTable areaLogInfoTable, DataTable duplicateRecordsInfoTable, DataTable skippedIUSInfoTable, DataTable skippedFiles)
        {
            this.SetBasicValues(importType, totalTimperiod, totalSource, totalData,
    indicatorLogInfoTable, unitLogInfoTable, subgroupLogInfoTable,
    areaLogInfoTable, duplicateRecordsInfoTable, skippedIUSInfoTable, skippedFiles);
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Sets basic values which are required to generate Html log file
        /// </summary>
        /// <param name="totalTimperiod"></param>
        /// <param name="totalSource"></param>
        /// <param name="totalData"></param>
        /// <param name="indicatorLogInfoTable"></param>
        /// <param name="unitLogInfoTable"></param>
        /// <param name="subgroupLogInfoTable"></param>
        /// <param name="areaLogInfoTable"></param>
        /// <param name="duplicateRecordsInfoTable"></param>
        /// <param name="skippedIUSInfoTable"></param>
        internal void SetBasicValues(Import.DAImport.Common.DIImportFileType importType, int totalTimperiod, int totalSource, int totalData,
    DataTable indicatorLogInfoTable, DataTable unitLogInfoTable, DataTable subgroupLogInfoTable,
    DataTable areaLogInfoTable, DataTable duplicateRecordsInfoTable, DataTable skippedIUSInfoTable, DataTable skippedFiles)
        {
            this.TotalTimeperiod = totalTimperiod;
            this.TotalSource = totalSource;
            this.TotalData = totalData;

            this.IndicatorLogInfoTable = indicatorLogInfoTable;
            this.UnitLogInfoTable = unitLogInfoTable;
            this.SubgroupLogInfoTable = subgroupLogInfoTable;
            this.AreaLogInfoTable = areaLogInfoTable;
            this.DuplicateRecordsInfoTable = duplicateRecordsInfoTable;
            this.SkippedIUSInfoTable = skippedIUSInfoTable;

            this.SkippedFiles = skippedFiles;
            this._ImportType = importType;
            this.SetHTMLLogVariables(importType);
        }

        private string GetVersionDetails(string databaseFileName)
        {
            string RetVal = string.Empty;
            DIConnection TempDBConnection = null;

            try
            {
                if (File.Exists(databaseFileName))
                {
                    TempDBConnection = new DIConnection(DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess, string.Empty, string.Empty, databaseFileName, string.Empty, string.Empty);
                    RetVal = "<span style=\"color:gray;font-size:10px;\"> (v." + Convert.ToString(TempDBConnection.ExecuteScalarSqlQuery(DIQueries.GetDatabaseVersion())) + ") </span>";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (TempDBConnection != null)
                {
                    TempDBConnection.Dispose();
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Creates The Html Log file in specified folder.
        /// </summary>
        /// <param name="databaseFileName">Target Database file.</param>
        /// <param name="sourceFiles">List of Source files (Spreadsheet or Database)</param>
        /// <param name="htmlLogOutputPath">Output path where Html will be created.</param>
        /// <returns></returns>
        internal string CreateLog(string databaseFileName, List<string> sourceFiles, string htmlLogOutputPath)
        {

            string RetVal = string.Empty;

            StreamWriter output;
            string sBodyFont;
            string FileType = string.Empty;
            string sDir;
            int iRow = 0;
            int iValue;
            DataTable TempTable;
            string TargetDatabaseExtension = string.Empty;
            string TargetType = string.Empty;
            string VersionNumber = string.Empty;

            try
            {
                RetVal = Path.Combine(htmlLogOutputPath, "Log_Import -" + System.DateTime.Today.ToLongDateString() + "-" + System.DateTime.Now.ToString().Split(" ".ToCharArray())[1].ToString().Replace(":", ".") + ".html");

                TargetDatabaseExtension = Path.GetExtension(this._TargetFilePath);
                if (TargetDatabaseExtension == DICommon.FileExtension.Template)
                {
                    TargetType = "Template";
                }
                else if (TargetDatabaseExtension == DICommon.FileExtension.Database)
                {
                    TargetType = "Database";
                }

                // Create a Writer
                sBodyFont = "<font style=\"FONT-FAMILY:" + this._FontName + "\";FONT-SIZE:" + this._FontSize + "pt;\">";

                output = new StreamWriter(RetVal, false, UTF8Encoding.UTF8);

                sDir = "ltr";
                //if( gbRTL)
                //        {
                //            sDir = "rtl";
                //        }



                output.Write("<HTML dir='" + sDir + "'><HEAD><TITLE> Log </TITLE></BR>");
                output.Write("<Font face='Arial'>");
                output.Write("<a name='Top'></a>");

                output.Write("<H1>Database Administration Log ");

                // add application version number
                try
                {
                    VersionNumber = System.Windows.Forms.Application.ProductVersion.ToString();
                    output.Write(" <span style=\"color:gray;font-size:10px;\"> (v." + VersionNumber + ")</span>");
                    output.Write("</H1></Head><BR>");
                }
                catch (Exception) { }


                output.Write("<Body><B>Module</B> : Import " + this._ImportType.ToString() + " <br>");
                output.Write("<B>Destination Database</B> : " + databaseFileName);

                if (!string.IsNullOrEmpty(this.TargetFileVersionNumber))
                {
                    output.Write(" <span style=\"color:gray;font-size:10px;\"> (v." + this._TargetFileVersionNumber + ")</span>");
                }
                output.Write("<br>");


                switch (this._ImportType)
                {
                    case DIImportFileType.DataEntrySpreasheet:
                        output.Write("<B>Spreadsheet Folder</B> :");
                        FileType = "Spreadsheet";
                        break;

                    case DIImportFileType.Database:
                        output.Write("<B>Source Database</B> : ");
                        FileType = "Database";
                        break;

                    case DIImportFileType.Template:
                        output.Write("<B>Source Database</B> : ");
                        FileType = "Template";
                        break;
                    case DIImportFileType.SDMXXml:
                        output.Write("<B>SDMX Folder</B> : ");
                        FileType = "SDMX-ML";
                        break;
                    default:
                        break;

                }

                //Source folder location
                output.Write(Path.GetDirectoryName(sourceFiles[0]) + "<br>");

                // Today's Date
                output.Write("<B>Date</B> : " + System.DateTime.Today.ToShortDateString() + "<br>");
                // Start Time of the Process stored in global Variable
                output.Write("<B>Start Time</B> : " + this._StartTime.Split(" ".ToCharArray())[1] + "<BR>");
                // End Time of the Process
                output.Write("<B>End Time</B> : " + System.DateTime.Now.ToString().ToString().Split(" ".ToCharArray())[1] + "<BR>");

                VersionNumber = this.GetVersionDetails(this._TargetFilePath);

                // If Template selected for Import then its path        
                if (TargetDatabaseExtension == DICommon.FileExtension.Template)
                {
                    output.Write("<B>Template</B> :" + this._TargetFilePath + VersionNumber + " <BR>");
                }
                else
                {
                    output.Write("<B>Database</B> :" + this._TargetFilePath + VersionNumber + " <BR>");
                }
                output.Write("<Table width=20% Border=0 Cellspacing=0 Cellpadding=0>");

                // Number of Time, Source and Data Values Inserted into the Target DB
                // Getting Timeperiod and Source exists in the table
                output.Write("<tr><td>Time</td><td>" + this.TotalTimeperiod + "</td></tr>");
                output.Write("<tr><td>Source</td><td>" + this.TotalSource + "</td></tr>");
                output.Write("<tr><td>Data</td><td>" + this.TotalData + "</td></tr></Table>");

                // Log Hyperlinks
                output.Write("<h3><a href=#IUS_Log>IUS and Area Log</a><br>");
                output.Write("<a href=#Gen_Log>General Log</a><br>");
                output.Write("<a href=#IUSSkipped>IUS Skipped</a><br>");
                output.Write("<a href=#Spr_Log>" + FileType + "</a><br></h3><hr>");


                // IUS Log
                output.Write("<br><h3><a name='IUS_Log'></a>IUS and Area Log</h3>");
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");

                // Unmatch Indicators
                output.Write("<tr><td  width=70%><b>" + DILanguage.GetLanguageString("UNMATCHED_INDICATORS") + "</b></td><td><a href=#Top>Back to Top</a></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " " + DILanguage.GetLanguageString("INDICATOR") + "</b></td><td bgcolor=E0E0E0><b>" + TargetType + " " + DILanguage.GetLanguageString("INDICATOR") + "</b></td></tr>");
                foreach (DataRow Row in this.IndicatorLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedIndicatorColumnName] + "</td><td>" + Row[Constants.Log.MapIndicatorColumnName] + "</td></tr>");
                }
                output.Write("</Table>");

                // Unmatch Units
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<br><tr><td  width=70%><b>Unmatched Units</b></td><td><a href=#Top>Back to Top</a></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Units</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Units</b></td></tr>");
                foreach (DataRow Row in this.UnitLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedUnitColumnName] + "</td><td>" + Row[Constants.Log.MapUnitColumnName] + "</td></tr>");
                }
                output.Write("</Table>");


                // Unmatch Subpops
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<br><tr><td  width=70%><b>Unmatched Subgroup</b></td><td><a href=#Top>Back to Top</A></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Subgroups</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Subgroups</b></td></tr>");
                foreach (DataRow Row in this.SubgroupLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedSubgroupValColumnName] + "</td><td>" + Row[Constants.Log.MapSubgroupValColumnName] + "</td></tr>");
                }
                output.Write("</Table><br>");

                // Unmatch Area
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<tr><td  width=70%><b>Unmatched Area</b></td><td><a href=#Top>Back to Top</A></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Areas</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Areas</b></td></tr>");
                foreach (DataRow Row in this.AreaLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedAreaColumnName] + "</td><td>" + Row[Constants.Log.MapAreaColumnName] + "</td></tr>");
                }
                output.Write("</Table><br><hr>");

                // General Log
                output.Write("<h3><a name='Gen_Log'>General Log</h3>");


                // Duplicate Record

                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<tr><td><b>Duplicate Records</b><br><br></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0><b>" + DILanguage.GetLanguageString("INDICATOR") + "</b></td><td bgcolor=E0E0E0><b>Unit</b></td><td bgcolor=E0E0E0><b>Subgroup</b></td><td bgcolor=E0E0E0><b>Time</b></td><td bgcolor=E0E0E0><b>AreaID</b></td><td bgcolor=E0E0E0><b>Source</b></td><td bgcolor=E0E0E0><b>Source File</b></td></tr><tr><td></td></tr>");
                foreach (DataRow Row in this.DuplicateRecordsInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.DuplicateIndicatorColumnName] + "</td><td> " + Row[this.DuplicateUnitColumnName] + "</td><td>" + Row[this.DuplicateSubgroupValColumnName] + "</td><td>" + Row[Constants.Log.DuplicateTimeperiodColumnName] + " </td><td>" + Row[Constants.Log.DuplicateAreaIDColumnName] + "</td><td>" + Row[Constants.Log.DuplicateSourceColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                }
                output.Write("</Table><br><hr>");


                // IUS Skipped Log
                output.Write("<h3><a name='IUSSkipped'></a>IUS Skipped</h3><br>");
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>" + DILanguage.GetLanguageString("INDICATOR") + "</b></td><td bgcolor=E0E0E0><b>Unit</b></td><td bgcolor=E0E0E0><b>Subgroup</b></td><td bgcolor=E0E0E0><b>FileName</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                foreach (DataRow Row in this.SkippedIUSInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.SkippedIndicatorColumnName] + "</td><td> " + Row[this.SkippedUnitColumnName] + "</td><td>" + Row[this.SkippedSubgroupValColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                }

                output.Write("</Table><br><hr>");

                // Invalid timeperiods
                if (this._InvalidTimeperiodsTable != null && this._InvalidTimeperiodsTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Invalid Timeperiods'></a>Invalid Timeperiods</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Timeperiod</b></td><td bgcolor=E0E0E0><b>Filename</b></td></tr>");

                    // invalid timeperiod and sheetname
                    foreach (DataRow Row in this.InvalidTimeperiodsTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[this.SkippedTimePeriodColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }


                // Skipped area
                if (this._SkippedAreaTable != null && this._SkippedAreaTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Area Skipped'></a>Area Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedAreaTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }

                // Skipped subgroup
                if (this._SkippedSubgroupTable != null && this._SkippedSubgroupTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Subgroup Skipped'></a>Subgroup Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedSubgroupTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }


                // Skipped source
                if (this._SkippedSourceTable != null && this._SkippedSourceTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Source Skipped'></a>Source Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td></tr>");

                    // skipped area and sheetname
                    foreach (DataRow Row in this.SkippedSourceTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");
                }

                // Invalid Source
                if (this._InvalidSourceTable != null && this._InvalidSourceTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Invalid Source'></a>Invalid Source</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source</b></td><td bgcolor=E0E0E0><b>Filename</b></td></tr>");

                    // invalid source and sheetname
                    foreach (DataRow Row in this._InvalidSourceTable.Rows)
                    {
                        //output.Write("<tr><td>" + Row[this.SkippedSourceColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                        output.Write("<tr><td>" + Row[Constants.Log.DuplicateSourceColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");
                }

                // Skipped Data
                if (this._SkippedDataTable != null && this._SkippedDataTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Data Skipped'></a>Data Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedDataTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");
                }

                // Skipped Source files (sheets)
                output.Write("<h3><a name='SkippedFiles'></a>" + FileType + " Skipped</h3><br>");
                if (this.SkippedFiles != null)
                {
                    if (this._ImportType == DIImportFileType.DataEntrySpreasheet)
                    {
                        output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Skipped File Name</b></td><td bgcolor=E0E0E0><b>Worksheet Name</b></td><td bgcolor=E0E0E0><b>Reason</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                        foreach (DataRow Row in this.SkippedFiles.Rows)
                        {
                            output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td> " + Row[Constants.Log.SkippedSheetName].ToString() + "</td><td>" + Row[Constants.Log.SkippedFileReason].ToString() + "</td></tr>");
                        }
                    }
                    else
                    {
                        output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Skipped File Name</b></td><td bgcolor=E0E0E0><b>Reason</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                        foreach (DataRow Row in this.SkippedFiles.Rows)
                        {
                            output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.SkippedFileReason].ToString() + "</td></tr>");
                        }
                    }
                    output.Write("</Table>");
                }
                output.Write("<br><hr>");

                // Source  file name
                output.Write("<h3><a name='Spr_Log'></a>" + FileType + " Processed</h3><ol>");
                foreach (string SourceFile in sourceFiles)
                {
                    if (this._ImportType == DIImportFileType.Database || this._ImportType == DIImportFileType.Template)
                    {
                        VersionNumber = this.GetVersionDetails(SourceFile);
                        output.Write("<Li>" + SourceFile + VersionNumber + "<br>");
                    }
                    else
                    {
                        output.Write("<Li>" + SourceFile + "<br>");
                    }
                }

                output.Write("</ol></Body></HTML>");
                output.Close();
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        internal string CreateCSVLog(string databaseFileName, List<string> sourceFiles, string htmlLogOutputPath)
        {
            string RetVal = string.Empty;

            StreamWriter output;
            string sBodyFont;
            string FileType = string.Empty;
            string sDir;
            int iRow = 0;
            int iValue;
            DataTable TempTable;
            string TargetDatabaseExtension = string.Empty;
            string TargetType = string.Empty;
            string VersionNumber = string.Empty;

            try
            {
                RetVal = Path.Combine(htmlLogOutputPath, "Log_CSV_Import -" + System.DateTime.Today.ToLongDateString() + "-" + System.DateTime.Now.ToString().Split(" ".ToCharArray())[1].ToString().Replace(":", ".") + ".html");

                TargetDatabaseExtension = Path.GetExtension(this._TargetFilePath);
                if (TargetDatabaseExtension == DICommon.FileExtension.Template)
                {
                    TargetType = "Template";
                }
                else if (TargetDatabaseExtension == DICommon.FileExtension.Database)
                {
                    TargetType = "Database";
                }

                // Create a Writer
                sBodyFont = "<font style=\"FONT-FAMILY:" + this._FontName + "\";FONT-SIZE:" + this._FontSize + "pt;\">";

                output = new StreamWriter(RetVal, false, UTF8Encoding.UTF8);

                sDir = "ltr";

                output.Write("<HTML dir='" + sDir + "'><HEAD><TITLE> Log </TITLE></BR>");
                output.Write("<Font face='Arial'>");
                output.Write("<a name='Top'></a>");
                output.Write("<H1>CSV Census Import - Database Administration Log </H1></Head><BR>");
                output.Write("<Body><B>Module</B> : DX Import CSV Census <br>");

                VersionNumber = DICommon.GetDIVersionDetailsHTMLStringForHTMLLog(databaseFileName);
                output.Write("<B>Target Database</B> : " + Path.GetFileName(databaseFileName) + VersionNumber + "<br>");

                switch (this._ImportType)
                {
                    case DIImportFileType.CSV:
                        output.Write("<B>CSV Folder</B> : ");
                        FileType = "CSV";
                        break;
                    default:
                        break;

                }

                output.Write(Path.GetDirectoryName(sourceFiles[0]) + "<br>");


                // Today's Date
                output.Write("<B>Date</B> : " + System.DateTime.Today.ToShortDateString() + "<br>");
                // Start Time of the Process stored in global Variable
                output.Write("<B>Start Time</B> : " + this._StartTime.Split(" ".ToCharArray())[1] + "<BR>");
                // End Time of the Process
                output.Write("<B>End Time</B> : " + System.DateTime.Now.ToString().ToString().Split(" ".ToCharArray())[1] + "<BR>");

                // If Template selected for Import then its path        
                if (TargetDatabaseExtension == DICommon.FileExtension.Template)
                {
                    output.Write("<B>Template</B> :" + this._TargetFilePath + " <BR>");
                }
                else
                {
                    output.Write("<B>Database</B> :" + this._TargetFilePath + " <BR>");
                }
                output.Write("<Table width=20% Border=0 Cellspacing=0 Cellpadding=0>");



                // Number of Time, Source and Data Values Inserted into the Target DB
                // Getting Timeperiod and Source exists in the table
                output.Write("<tr><td>Time</td><td>" + this.TotalTimeperiod + "</td></tr>");
                output.Write("<tr><td>Source</td><td>" + this.TotalSource + "</td></tr>");
                output.Write("<tr><td>Data</td><td>" + this.TotalData + "</td></tr></Table>");

                // Log Hyperlinks
                output.Write("<h4><a href=#IUS_Log>IUS and Area Log</a><br>");
                output.Write("<a href=#Gen_Log>General Log</a><br>");
                output.Write("<a href=#IUSSkipped>IUS Skipped</a><br>");
                output.Write("<a href=#Spr_Log>" + FileType + "</a><br></h4><hr>");


                // IUS Log
                output.Write("<br><h3><a name='IUS_Log'></a>IUS and Area Log</h3>");
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=1>");

                // Matched Indicators_Unit_Subgroups
                output.Write("<tr><td  width=70%><b>IUS Imported</b></td><td><a href=#Top>Back to Top</a></td></tr>");
                output.Write("<tr bgcolor=E0E0E0><td><b>IUS GUID</b></td><td><b>" + DILanguage.GetLanguageString("INDICATOR") + "</b></td><td><b>Unit</b></td><td><b>Subgroup</b></td></tr>");
                if (this.IUSLogInfoTable != null)
                {
                    foreach (DataRow Row in this.IUSLogInfoTable.Rows)
                    {
                        output.Write("<tr>");
                        output.Write("<td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId] + "-" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId] + "-" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId] + "</td>");
                        output.Write("<td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName] + "</td>");
                        output.Write("<td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName] + "</td>");
                        output.Write("<td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal] + "</td>");
                        output.Write("</tr>");
                    }
                }
                output.Write("</Table>");

                output.Write("<br>");

                // New Areas
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<tr><td  width=30%><b>New Areas</b></td><td><a href=#Top>Back to Top</A></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>Area ID</b></td><td bgcolor=E0E0E0><b>Area Name</b></td><td bgcolor=E0E0E0><b>Source File</b></td></tr>");
                foreach (DataRow Row in this.AreaLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID] + "</td><td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                }
                output.Write("</Table><br><hr>");

                // General Log
                output.Write("<h3><a name='Gen_Log'>General Log</h3>");

                // Duplicate Record

                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<tr><td><b>Duplicate Records</b><br><br></td></tr>");
                if (this.DuplicateRecordsInfoTable != null && this.DuplicateRecordsInfoTable.Rows.Count > 0)
                {
                    output.Write("<tr><td bgcolor=E0E0E0><b>" + DILanguage.GetLanguageString("INDICATOR") + "</b></td><td bgcolor=E0E0E0><b>Unit</b></td><td bgcolor=E0E0E0><b>Subgroup</b></td><td bgcolor=E0E0E0><b>Time</b></td><td bgcolor=E0E0E0><b>AreaID</b></td><td bgcolor=E0E0E0><b>Source</b></td><td bgcolor=E0E0E0><b>Source File</b></td></tr><tr><td></td></tr>");
                    foreach (DataRow Row in this.DuplicateRecordsInfoTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[this.DuplicateIndicatorColumnName] + "</td><td> " + Row[this.DuplicateUnitColumnName] + "</td><td>" + Row[this.DuplicateSubgroupValColumnName] + "</td><td>" + Row[Constants.Log.DuplicateTimeperiodColumnName] + " </td><td>" + Row[Constants.Log.DuplicateAreaIDColumnName] + "</td><td>" + Row[Constants.Log.DuplicateSourceColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }
                }
                output.Write("</Table><br><hr>");


                ////// IUS Skipped Log
                ////output.Write("<h3><a name='IUSSkipped'></a>IUS Skipped</h3><br>");
                //if (this.SkippedIUSInfoTable != null && this.SkippedIUSInfoTable.Rows.Count > 0)
                ////{
                ////    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=1><tr><td bgcolor=E0E0E0><b>IUS</b></td><td bgcolor=E0E0E0><b>FileName</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                ////    foreach (DataRow Row in this.SkippedIUSInfoTable.Rows)
                ////    {
                ////        output.Write("<tr><td>" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId] + "-" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId] + "-" + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId] + "</td>" + "<td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                ////    }
                ////}

                ////output.Write("</Table><br><hr>");

                // Invalid timeperiods
                if (this._InvalidTimeperiodsTable != null && this._InvalidTimeperiodsTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Invalid Timeperiods'></a>Invalid Timeperiods</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Timeperiod</b></td><td bgcolor=E0E0E0><b>Filename</b></td><td bgcolor=E0E0E0><b>Row Number</b></td></tr>");

                    // invalid timeperiod and sheetname
                    foreach (DataRow Row in this.InvalidTimeperiodsTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[this.SkippedTimePeriodColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.CSVDataRowNoColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }


                // Skipped area
                if (this._SkippedAreaTable != null && this._SkippedAreaTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Area Skipped'></a>Area Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=1><tr><td bgcolor=E0E0E0><b>Source file</b></td><td bgcolor=E0E0E0><b>Row Number</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedAreaTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.CSVDataRowNoColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }



                // Skipped subgroup
                if (this._SkippedSubgroupTable != null && this._SkippedSubgroupTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Subgroup Skipped'></a>Subgroup Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedSubgroupTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }


                // Skipped source
                if (this._SkippedSourceTable != null && this._SkippedSourceTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Source Skipped'></a>Source Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Source file</b></td><td bgcolor=E0E0E0><b>Row Number</b></td></tr>");

                    // skipped area and sheetname
                    foreach (DataRow Row in this.SkippedSourceTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.CSVDataRowNoColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }

                // Skipped Data
                if (this._SkippedDataTable != null && this._SkippedDataTable.Rows.Count > 0)
                {
                    // Main header
                    output.Write("<h3><a name='Data Skipped'></a>Data Skipped</h3><br>");
                    // header
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=2><tr><td  width='35%' bgcolor=E0E0E0><b>Source file</b></td><td bgcolor=E0E0E0 style='halign:center'><b>Row Number</b></td><td bgcolor=E0E0E0 style='halign:center'><b>IUS</b></td></tr>");

                    // skipped sheetname
                    foreach (DataRow Row in this.SkippedDataTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.CSVDataRowNoColumnName] + "</td><td>" + Row[2] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");

                }

                // Skipped Source files (sheets)
                output.Write("<h3><a name='SkippedFiles'></a>" + FileType + " Skipped Files</h3><br>");
                if (this.SkippedFiles != null)
                {
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Skipped File Name</b></td><td bgcolor=E0E0E0><b>Reason</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                    foreach (DataRow Row in this.SkippedFiles.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td><td>" + Row[Constants.Log.SkippedFileReason].ToString() + "</td></tr>");
                    }

                    output.Write("</Table>");
                }
                output.Write("<br><hr>");
                //Skipped IUS
                output.Write("<h3><a name='IUSSkipped'></a> Skipped IUS</h3><br>");
                if (this.SkippedIUSInfoTable != null && this.SkippedIUSInfoTable.Rows.Count > 0)
                {
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=2><tr><td width='65%' bgcolor=E0E0E0><b>Indicator GID | Unit GID | SubGroup GID</b></td><td width='35%' bgcolor=E0E0E0><b> Reason </b></td></tr>");
                    foreach (DataRow Row in this.SkippedIUSInfoTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[Constants.SkippedIUSColumnName] + "</td><td>" + Row[Constants.SkippedIUSReason] + "</td></tr>");
                    }
                    output.Write("</Table>");
                }
                // Source  file name
                output.Write("<h3><a name='Spr_Log'></a>" + FileType + " Processed</h3><ol>");
                foreach (string SourceFile in sourceFiles)
                {
                    output.Write("<Li>" + SourceFile + "<br>");
                }
                output.Write("</ol></Body></HTML>");
                output.Close();

            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }



        #endregion

        #endregion
    }
}
