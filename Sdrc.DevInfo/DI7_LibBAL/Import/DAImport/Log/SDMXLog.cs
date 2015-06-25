using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Log
{
    public class SDMXLog
    {
        #region "-- Private --"

        #region "-- Variables / Properties --"
        // DataTables for each (I, U, S, Area,  Duplicate & Skipped records of imported Data)
        private DataTable IndicatorLogInfoTable = null;
        private DataTable UnitLogInfoTable = null;
        private DataTable SubgroupLogInfoTable = null;
        private DataTable AreaLogInfoTable = null;
        private DataTable DuplicateRecordsInfoTable = null;
        private DataTable SkippedIUSInfoTable = null;

        private DataTable SkippedFiles = null;

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
        private string SkippedSourceFileColumnName = "SourceFile";


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
        public string StartTime
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


        #endregion

        #region "-- New/Dispose --"



        public SDMXLog(Import.DAImport.Common.DIImportFileType importType,
            DataTable indicatorLogInfoTable, DataTable unitLogInfoTable, DataTable subgroupLogInfoTable,
            DataTable areaLogInfoTable, DataTable duplicateRecordsInfoTable, DataTable skippedIUSInfoTable, DataTable skippedFiles)
        {
            this.SetBasicValues(importType,
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
        public void SetBasicValues(Import.DAImport.Common.DIImportFileType importType,
    DataTable indicatorLogInfoTable, DataTable unitLogInfoTable, DataTable subgroupLogInfoTable,
    DataTable areaLogInfoTable, DataTable duplicateRecordsInfoTable, DataTable skippedIUSInfoTable, DataTable skippedFiles)
        {

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

        /// <summary>
        /// Creates The Html Log file in specified folder.
        /// </summary>
        /// <param name="databaseFileName">Target Database file.</param>
        /// <param name="sourceFiles">List of Source files (Spreadsheet or Database)</param>
        /// <param name="htmlLogOutputPath">Output path where Html will be created.</param>
        /// <returns></returns>
        public string CreateLog(string databaseFileName, List<string> sourceFiles, string htmlLogOutputPath)
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
                output.Write("<H1>Database Administration Log </H1></Head><BR>");
                output.Write("<Body><B>Module</B> : Import " + this._ImportType.ToString() + " <br>");
                output.Write("<B>Destination Database</B> : " + databaseFileName + "<br>");

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

                output.Write(Path.GetDirectoryName(sourceFiles[0]) + "<br>");


                // Today's Date
                output.Write("<B>Date</B> : " + System.DateTime.Today.ToShortDateString() + "<br>");
                // Start Time of the Process stored in global Variable
                output.Write("<B>Start Time</B> : " + this._StartTime.Split(" ".ToCharArray())[1] + "<BR>");
                // End Time of the Process
                output.Write("<B>End Time</B> : " + System.DateTime.Now.ToString().ToString().Split(" ".ToCharArray())[1] + "<BR>");

                // If Template selected for Import then its path        

                output.Write("<B>Spreadsheet</B> :" + this._TargetFilePath + " <BR>");

                output.Write("<Table width=20% Border=0 Cellspacing=0 Cellpadding=0>");



                // Log Hyperlinks
                output.Write("<h3><a href=#IUS_Log>Indicator-Unit-Subgroup and Area Log</a><br>");
                output.Write("<a href=#Gen_Log>General Log</a><br>");
                output.Write("<a href=#IUSSkipped>Indicator-Unit-Subgroup Skipped</a><br>");
                output.Write("<a href=#Spr_Log>" + FileType + "</a><br></h3><hr>");


                // IUS Log
                output.Write("<br><h3><a name='IUS_Log'></a>Indicator-Unit-Subpop and Area Log</h3>");
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");

                // Unmatch Indicators
                output.Write("<tr><td  width=70%><b>Unmatched Indicators</b></td><td><a href=#Top>Back to Top</a></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Indicator</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Indicator</b></td></tr>");
                foreach (DataRow Row in this.IndicatorLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedIndicatorColumnName] + "</td><td>" + Row[1] + "</td></tr>");
                }
                output.Write("</Table>");

                // Unmatch Units
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<br><tr><td  width=70%><b>Unmatched Units</b></td><td><a href=#Top>Back to Top</a></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Units</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Units</b></td></tr>");
                foreach (DataRow Row in this.UnitLogInfoTable.Rows)
                {
                    //output.Write("<tr><td>" + Row[this.UnmatchedUnitColumnName] + "</td><td>" + Row[Constants.Log.MapUnitColumnName] + "</td></tr>");
                    output.Write("<tr><td>" + Row[this.UnmatchedUnitColumnName] + "</td><td>" + Row[1] + "</td></tr>");
                }
                output.Write("</Table>");


                // Unmatch Subpops
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<br><tr><td  width=70%><b>Unmatched Subpop</b></td><td><a href=#Top>Back to Top</A></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Subpops</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Subpops</b></td></tr>");
                foreach (DataRow Row in this.SubgroupLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedSubgroupValColumnName] + "</td><td>" + Row[1] + "</td></tr>");
                }
                output.Write("</Table><br>");

                // Unmatch Area
                output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0>");
                output.Write("<tr><td  width=70%><b>Unmatched Area</b></td><td><a href=#Top>Back to Top</A></td></tr>");
                output.Write("<tr><td bgcolor=E0E0E0 ><b>" + FileType + " Areas</b></td><td bgcolor=E0E0E0><b>" + TargetType + " Areas</b></td></tr>");
                foreach (DataRow Row in this.AreaLogInfoTable.Rows)
                {
                    output.Write("<tr><td>" + Row[this.UnmatchedAreaColumnName] + "</td><td>" + Row[1] + "</td></tr>");
                }
                output.Write("</Table><br><hr>");

                // General Log
                output.Write("<h3><a name='Gen_Log'>General Log</h3>");


                if (this.SkippedIUSInfoTable != null)
                {
                    // IUS Skipped Log
                    output.Write("<h3><a name='IUSSkipped'></a>Indicator-Unit-Subpop Skipped</h3><br>");
                    output.Write("<Table width=100% Border=0 Cellspacing=0 Cellpadding=0><tr><td bgcolor=E0E0E0><b>Indicator</b></td><td bgcolor=E0E0E0><b>Unit</b></td><td bgcolor=E0E0E0><b>Subpop</b></td><td bgcolor=E0E0E0><b>FileName</b></td></tr><tr><td><b>&nbsp</b></td></tr>");
                    foreach (DataRow Row in this.SkippedIUSInfoTable.Rows)
                    {
                        output.Write("<tr><td>" + Row[this.SkippedIndicatorColumnName] + "</td><td> " + Row[this.SkippedUnitColumnName] + "</td><td>" + Row[this.SkippedSubgroupValColumnName] + "</td><td>" + Row[Constants.Log.SkippedSourceFileColumnName] + "</td></tr>");
                    }

                    output.Write("</Table><br><hr>");
                }

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

        /// <summary>
        /// Prepares all required elements for Html log creation , and Creates Html log file.
        /// </summary>
        public void CreateHTMLLog()
        {

        }




        #endregion

        #endregion

    }
}
