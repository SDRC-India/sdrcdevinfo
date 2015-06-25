using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Data;


namespace DevInfo.Lib.DI_LibBAL.LogFiles
{

    public class DXLogFile
    {

        #region "--Public--"

        private DIConnection _DBConnection = null;
        /// <summary>
        /// Gets or sets instance of DBConnection
        /// </summary>
        public DIConnection DBConnection
        {
            get
            {
                return this._DBConnection;
            }
            set
            {
                this._DBConnection = value;
            }
        }

        private DIQueries _DBQueries = null;
        /// <summary>
        /// Gets or sets instance of DBQueries
        /// </summary>
        public DIQueries DBQueries
        {
            get
            {
                return this._DBQueries;
            }
            set
            {
                this._DBQueries = value;
            }
        }

        private bool ShowForMergeTemplate = false;

        private bool _ShowTime = true;
        /// <summary>
        /// Gets or sets true/false. If false then start and end time will not come in the log file. Default is true.
        /// </summary>
        public bool ShowTime
        {
            get { return this._ShowTime; }
            set { this._ShowTime = value; }
        }

        #region"--METHODS--"

        public void Start(string logFileNameWPath, string applicationName, List<string> inputFile, string outputFileNameWPath)
        {

            FileInfo LogFile;

            this.LogFilePath = logFileNameWPath;
            LogFile = new FileInfo(this.LogFilePath);
            this.StringWriter = LogFile.CreateText();

            this.ApplicationName = applicationName;
            this.InputFiles = inputFile;
            this.OutputFileNameWPath = outputFileNameWPath;
            this.Writer = new HtmlTextWriter(StringWriter);

            // add filename and datetime
            this.AddHeaderWFileNameAndTime();

            // end Table tag
            this.EndTag();
        }

        /// <summary>
        /// Start Log file with DBInfo
        /// </summary>
        /// <param name="logFileNameWPath"></param>
        /// <param name="applicationName"></param>
        /// <param name="inputFile"></param>
        /// <param name="outputFileNameWPath"></param>
        /// <param name="includeDBInfo"></param>
        public void Start(string logFileNameWPath, string applicationName, List<string> inputFile, string outputFileNameWPath, bool includeDBInfo, DIConnection dbConnection, DIQueries dbQueries)
        {

            FileInfo LogFile;

            this.LogFilePath = logFileNameWPath;
            LogFile = new FileInfo(this.LogFilePath);
            this.StringWriter = LogFile.CreateText();

            this.IncludeDBInfo = includeDBInfo;
            this.ApplicationName = applicationName;
            this.InputFiles = inputFile;
            this.OutputFileNameWPath = outputFileNameWPath;
            this.Writer = new HtmlTextWriter(StringWriter);

            // add filename and datetime
            this.AddHeaderWFileNameAndTime();

            // connect to database
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
            this.IncludeDBInfo = includeDBInfo;

            //// add database information
            //if (includeDBInfo)
            //{
            //    this.AddDataBaseInfo();
            //}

            // end Table tag
            this.EndTag();
        }

        /// <summary>
        /// Start Log file with DBInfo
        /// </summary>
        /// <param name="logFileNameWPath"></param>
        /// <param name="applicationName"></param>
        /// <param name="inputFile"></param>
        /// <param name="outputFileNameWPath"></param>
        /// <param name="includeDBInfo"></param>
        public void Start(string logFileNameWPath, string applicationName, List<string> inputFile, string outputFileNameWPath, bool includeDBInfo, bool showForMergetTemplate, DIConnection dbConnection, DIQueries dbQueries)
        {

            FileInfo LogFile;
            this.ShowForMergeTemplate = showForMergetTemplate;
            this.LogFilePath = logFileNameWPath;
            LogFile = new FileInfo(this.LogFilePath);
            this.StringWriter = LogFile.CreateText();

            this.IncludeDBInfo = includeDBInfo;
            this.ApplicationName = applicationName;
            this.InputFiles = inputFile;
            this.OutputFileNameWPath = outputFileNameWPath;
            this.Writer = new HtmlTextWriter(StringWriter);

            // add filename and datetime
            this.AddHeaderWFileNameAndTime();

            // connect to database
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
            this.IncludeDBInfo = includeDBInfo;

            //// add database information
            //if (includeDBInfo)
            //{
            //    this.AddDataBaseInfo();
            //}

            // end Table tag
            this.EndTag();
        }

        /// <summary>
        /// Start log file without DBINfo
        /// </summary>
        /// <param name="logFileNameWPath"></param>
        /// <param name="applicationName"></param>
        /// <param name="inputFiles"></param>
        /// <param name="outputFileNameWPath"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public void Start(string logFileNameWPath, string applicationName, List<string> inputFiles, string outputFileNameWPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            this.Start(logFileNameWPath, applicationName, inputFiles, outputFileNameWPath, true, dbConnection, dbQueries);
        }


        /// <summary>
        /// Start Log file with DBInfo with Multi DataTable Values
        /// </summary>
        /// <param name="logFileNameWPath"></param>
        /// <param name="applicationName"></param>
        /// <param name="inputFile"></param>
        /// <param name="outputFileNameWPath"></param>
        /// <param name="includeDBInfo"></param>
        /// <param name="tablesCollection">DataTAble Collection With Table Header</param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public void Start(string logFileNameWPath, string applicationName, List<string> inputFile, string outputFileNameWPath, bool includeDBInfo, Dictionary<string, DataTable> tablesCollection, DIConnection dbConnection, DIQueries dbQueries)
        {

            FileInfo LogFile;

            this.LogFilePath = logFileNameWPath;
            LogFile = new FileInfo(this.LogFilePath);
            this.StringWriter = LogFile.CreateText();

            this.IncludeDBInfo = includeDBInfo;
            this.ApplicationName = applicationName;
            this.InputFiles = inputFile;
            this.OutputFileNameWPath = outputFileNameWPath;
            this.Writer = new HtmlTextWriter(StringWriter);

            // add filename and datetime
            this.AddHeaderWFileNameAndTime();

            // connect to database
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
            this.IncludeDBInfo = includeDBInfo;

            foreach (string tableHeader in tablesCollection.Keys)
            {
                this.AddDataTable(tablesCollection[tableHeader], tableHeader);
            }

            // end Table tag
            this.EndTag();
        }


        public void AddDataTable(DataTable table, string tableHeader)
        {
            this.InsertHorizontalRegion(string.Empty, "2");

            this.InsertStartTableTag();
            // database
            this.InsertStartRowTag();
            this.InsertHeadingWAttr(tableHeader + ": ");
            this.InsertBlankRows();
            this.InsertStartCellTag(Constants.LogFileFontTypes[LogFileFontType.H3]);

            this.InsertStartTableTag();

            this.InsertStartRowTag();
            foreach (DataColumn Column in table.Columns)
            {
                this.InsertCell(Column.ColumnName, Constants.LogFileFontTypes[LogFileFontType.H3]);
            }
            this.InsertEndRowTag();

            foreach (DataRow Row in table.Rows)
            {
                this.InsertStartRowTag();
                foreach (DataColumn Column in table.Columns)
                {
                    this.InsertCell(Convert.ToString(Row[Column]), Constants.LogFileFontTypes[LogFileFontType.H4]);
                }
                this.InsertEndRowTag();
            }

            this.InsertEndTableTag();
            this.InsertEndCellTag();
            this.InsertEndRowTag();
            this.InsertEndTableTag();
            this.Writer.WriteBreak();
            this.Writer.WriteBreak();
        }

        /// <summary>
        /// Finally Close Logfile
        /// </summary>
        public virtual void Close()
        {

            // add database information
            if (this.IncludeDBInfo)
            {
                if (this.ShowForMergeTemplate)
                    this.AddDataBaseInfo(true);
                else
                    this.AddDataBaseInfo();
            }


            DateTime EndDateTime = DateTime.Now;
            this.InsertHorizontalRegion("red", "4");
            this.Writer.Close();
            this.Writer.Dispose();
            this.StringWriter.Close();

            if (this.ShowTime)
            {
                this.CheckNReplace(this.EndDateTime, Constants.ChangedInLast);
            }

            this.ReplaceOutputFileName(this.OutputFileNameWPath, Constants.OutputFileWPathTag);

            if (!string.IsNullOrEmpty(this.StartDateTime) & this.ShowTime)
            { this.CheckNReplace(this.StartDateTime, this.DefaultStartDateTime); }

            this.AppendHTMLMetaTag();
        }

        public void SetOutputFileName(string outputFileNameWPath)
        {
            this.OutputFileNameWPath = outputFileNameWPath;

        }

        /// <summary>
        /// Generates log file name
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public string GenerateLogFileName(string applicationName)
        {
            string RetVal = DateTime.Now.ToString();
            RetVal = RetVal.Replace(":", "_");
            RetVal = RetVal.Replace("/", "_");
            RetVal = RetVal.Replace("\\", "_");
            RetVal = "Log File_" + applicationName + "_" + RetVal + DICommon.FileExtension.HTML;

            //  RetVal= DICommon.GenerateUniqueFileName(RetVal, string.Empty);
            return RetVal;
        }


        /// <summary>
        /// Set Start Time of Logs
        /// </summary>
        /// <param name="startTime"></param>
        public void SetStartTime(string startTime)
        {
            this.StartDateTime = startTime;
        }

        /// <summary>
        /// Set endTime of Logs
        /// </summary>
        /// <param name="endTime"></param>
        public void SetEndTime(string endTime)
        {
            this.EndDateTime = endTime;

        }

        public void InsertHeadingWValue(string heading, string value)
        {
            this.InsertStartTableTag();
            this.InsertStartRowTag();

            this.InsertCell(heading, Constants.LogFileFontTypes[LogFileFontType.H3]);
            this.InsertBlankCell();
            this.InsertCell(value, Constants.LogFileFontTypes[LogFileFontType.H3]);

            this.InsertEndRowTag();
            this.InsertEndTableTag();
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Variables --"
        //Htmltextwriter Object for writing in html file
        protected HtmlTextWriter Writer;
        //Variable for getting information from Db or not 
        protected bool IncludeDBInfo = false;

        #endregion

        #region"-- Methods--"

        protected void InsertHorizontalRegion(string color, string size)
        {
            if (string.IsNullOrEmpty(color))
            {
                color = "lime";
            }

            if (string.IsNullOrEmpty(size))
            {
                color = "2";
            }

            this.Writer.WriteFullBeginTag("HR style=color:" + color + ";size:" + size + "2 ;");
        }

        protected void InsertStartTableTag()
        {
            this.Writer.WriteBeginTag("TABLE ");
            this.Writer.WriteAttribute("Width", "100%");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
        }

        protected void InsertEndTableTag()
        {
            this.Writer.WriteEndTag("TABLE ");
        }

        protected void InsertStartRowTag()
        {
            this.Writer.WriteBeginTag("TR ");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
        }

        protected void InsertEndRowTag()
        {
            this.Writer.WriteEndTag("TR ");
        }

        protected void InsertStartCellTag(string fontSize)
        {
            this.InsertStartCellTag(fontSize, string.Empty);
        }

        protected void InsertStartCellTag(string fontSize, string cellWidth)
        {
            this.Writer.WriteBeginTag("TD " + fontSize);

            if (!string.IsNullOrEmpty(cellWidth))
            {
                this.Writer.WriteAttribute("width", cellWidth);
            }

            this.Writer.WriteAttribute("valign", "top");

            this.Writer.Write(HtmlTextWriter.TagRightChar);
        }

        protected void InsertStartCellTagWAttr(string fontSize, string alignment)
        {

            this.Writer.WriteBeginTag("TD valign='top' " + fontSize);
            if (!string.IsNullOrEmpty(alignment))
            {
                this.Writer.WriteAttribute("style", "text-align:center;");
            }

            this.Writer.Write(HtmlTextWriter.TagRightChar);
        }


        protected void InsertStartCellTagWAttr(string fontSize)
        {
            this.InsertStartCellTagWAttr(fontSize, string.Empty);

        }

        protected void InsertEndCellTag()
        {
            this.Writer.WriteEndTag("TD");
        }


        protected void InsertTableRow(string heading, string value)
        {
            this.InsertStartRowTag();

            this.InsertMainHeading(heading);
            this.InsertMainHeadingValue(value);

            this.InsertEndRowTag();
        }



        protected void InsertMainHeading(string heading)
        {
            this.InsertCell(heading, Constants.LogFileFontTypes[LogFileFontType.H1]);
        }

        protected void InsertMainHeading(string heading, string cellwidth)
        {
            this.InsertCell(heading, Constants.LogFileFontTypes[LogFileFontType.H1], cellwidth);
        }


        protected void InsertMainHeadingValue(string value)
        {
            this.InsertCell(value, Constants.LogFileFontTypes[LogFileFontType.H2]);
        }

        protected void InsertHeading(string heading)
        {
            this.InsertHeading(heading, string.Empty);
        }

        protected void InsertHeading(string heading, string cellWidth)
        {
            this.InsertCell(heading, Constants.LogFileFontTypes[LogFileFontType.H3], cellWidth);
        }

        protected void InsertHeadingWAttr(string heading)
        {
            this.InsertCellWAttr(heading, Constants.LogFileFontTypes[LogFileFontType.H1]);
        }

        protected void InsertHeadingValue(string value)
        {
            this.InsertHeadingValue(value, string.Empty);
        }

        protected void InsertHeadingValue(string value, string cellWidth)
        {
            this.InsertCell(value, Constants.LogFileFontTypes[LogFileFontType.H4], cellWidth);
        }

        protected void InsertBlankCell()
        {
            this.InsertMainHeadingValue(string.Empty);
        }

        protected void InsertBlankRows()
        {
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
            this.BlankRow();
        }


        protected void InsertStartTableTagWAttr()
        {
            this.Writer.WriteBeginTag("TABLE ");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
        }

        //Insert span with text and size
        protected void InsertSpan(string Header, string fontSize)
        {
            this.Writer.WriteBeginTag("Span " + fontSize);
            this.Writer.Write(HtmlTextWriter.TagRightChar);
            this.Writer.Write(Header);
            this.Writer.WriteEndTag("Span");

        }

        /// <summary>
        /// Add DataBase Information in Log File
        /// </summary>
        protected void AddDataBaseInfo()
        {
            string SqlQuery = string.Empty;

            this.InsertHorizontalRegion(string.Empty, "2");

            this.InsertStartTableTag();
            // database
            this.InsertStartRowTag();
            //this.InsertStartCellTag(Constants.LogFileFontTypes[LogFileFontType.H2]);
            this.InsertHeadingWAttr("DataBase :");
            // this.InsertEndCellTag();
            // this.InsertStartRowTag();
            this.InsertStartCellTag(Constants.LogFileFontTypes[LogFileFontType.H2]);
            this.InsertStartTableTag();
            //this.InsertEndRowTag();                       

            // counts
            // indicator
            this.InsertRowsCount(DILanguage.GetLanguageString(Constants.Indicator), this.DBQueries.TablesName.Indicator);

            // Unit
            this.InsertRowsCount(Constants.Unit, this.DBQueries.TablesName.Unit);

            // Subgroup
            this.InsertRowsCount(Constants.Subgroup, this.DBQueries.TablesName.SubgroupVals);

            //  area
            this.InsertRowsCount(Constants.Area, this.DBQueries.TablesName.Area);

            //  source
            this.InsertRowsCount(Constants.Source, this.DBQueries.TablesName.IndicatorClassifications);

            // Timeperiod
            this.InsertRowsCount(Constants.Time, this.DBQueries.TablesName.TimePeriod);

            //  data 
            this.InsertRowsCount(Constants.Data, this.DBQueries.TablesName.Data);



            this.InsertEndTableTag();
            this.InsertEndCellTag();
            this.InsertEndRowTag();
            this.InsertEndTableTag();
            this.Writer.WriteBreak();
            this.Writer.WriteBreak();
        }

        /// <summary>
        /// Add DataBase Information in Log File
        /// </summary>
        protected void AddDataBaseInfo(bool showForMergeTemplate)
        {
            string SqlQuery = string.Empty;

            this.InsertHorizontalRegion(string.Empty, "2");

            this.InsertStartTableTag();
            // database
            this.InsertStartRowTag();
            //this.InsertStartCellTag(Constants.LogFileFontTypes[LogFileFontType.H2]);
            this.InsertHeadingWAttr("DataBase :");
            // this.InsertEndCellTag();
            // this.InsertStartRowTag();
            this.InsertStartCellTag(Constants.LogFileFontTypes[LogFileFontType.H2]);
            this.InsertStartTableTag();
            //this.InsertEndRowTag();                       

            // counts
            // indicator
            this.InsertRowsCount(DILanguage.GetLanguageString(Constants.Indicator), this.DBQueries.TablesName.Indicator);

            // Unit
            this.InsertRowsCount(Constants.Unit, this.DBQueries.TablesName.Unit);

            // Subgroup
            this.InsertRowsCount(Constants.Subgroup, this.DBQueries.TablesName.SubgroupVals);

            //  IUS
            this.InsertRowsCount(Constants.IUS, this.DBQueries.TablesName.IndicatorUnitSubgroup);

            // IC without Source
            InsertRowsCountForICWithOutSource(Constants.IndicatorClassification, this.DBQueries.TablesName.IndicatorClassifications);

            //  area
            this.InsertRowsCount(Constants.Area, this.DBQueries.TablesName.Area);


            if (!showForMergeTemplate)
            {
                //  source
                this.InsertRowsCount(Constants.Source, this.DBQueries.TablesName.IndicatorClassifications);

                // Timeperiod
                this.InsertRowsCount(Constants.Time, this.DBQueries.TablesName.TimePeriod);

                //  data 
                this.InsertRowsCount(Constants.Data, this.DBQueries.TablesName.Data);

            }

            this.InsertEndTableTag();
            this.InsertEndCellTag();
            this.InsertEndRowTag();
            this.InsertEndTableTag();
            this.Writer.WriteBreak();
            this.Writer.WriteBreak();
        }

        #endregion


        #endregion

        #region "--Private--"

        #region"--VARIABLES--"

        private string ApplicationName = string.Empty;
        private List<string> InputFiles;
        private string OutputFileNameWPath = string.Empty;



        private StreamWriter StringWriter;
        private string LogFilePath = string.Empty;
        private string DefaultStartDateTime = string.Empty;
        private string StartDateTime = string.Empty;
        private string EndDateTime = string.Empty;

        #endregion

        #region "--Methods--"

        /// <summary>
        ///  Insert cell with setting fontsize 
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="fontSize"></param>
        private void InsertCell(string cellValue, string fontSize)
        {
            this.InsertCell(cellValue, fontSize, string.Empty);
        }

        /// <summary>
        /// Insert cell with setting fontsize and cellwidth
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="fontSize"></param>
        /// <param name="cellWidth">May be null or empty</param>
        private void InsertCell(string cellValue, string fontSize, string cellWidth)
        {
            this.InsertStartCellTag(fontSize, cellWidth);

            this.Writer.Write(cellValue);


            this.InsertEndCellTag();
        }

        /// <summary>
        /// Insert cell with attribute valign top
        /// </summary>
        /// <param name="cellValue"></param>
        /// <param name="fontSize"></param>
        private void InsertCellWAttr(string cellValue, string fontSize)
        {
            this.InsertStartCellTagWAttr(fontSize);
            this.Writer.Write(cellValue);
            this.InsertEndCellTag();
        }

        //Insert rows count in DB
        private void InsertRowsCount(string heading, string tableName)
        {
            string SqlQuery = string.Empty;
            string Count = "0";

            // get rows count
            if (tableName == this.DBQueries.TablesName.IndicatorClassifications)
            {
                SqlQuery = this.DBQueries.IndicatorClassification.GetsSourcesWithoutPublishers();
                Count = this.DBConnection.ExecuteDataTable(SqlQuery).Rows.Count.ToString();
            }
            else
            {
                SqlQuery = DIQueries.GetTableRecordsCount(tableName, string.Empty);
                Count = Convert.ToString(DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }



            // write heading and count
            this.InsertStartRowTag();
            this.InsertBlankCell();

            this.InsertHeading(heading);
            this.InsertHeadingValue(Count);
            this.InsertEndRowTag();
        }

        //Insert rows count in DB
        private void InsertRowsCountForICWithOutSource(string heading, string tableName)
        {
            string SqlQuery = string.Empty;
            string Count = "0";
            DataView DV = null;

            // get rows count
            if (tableName == this.DBQueries.TablesName.IndicatorClassifications)
            {
                SqlQuery = DIQueries.GetTableRecordsCount(tableName, IndicatorClassifications.ICType + " <> " + DIQueries.ICTypeText[ICType.Source]);
                Count = Convert.ToString(DBConnection.ExecuteScalarSqlQuery(SqlQuery));

            }



            // write heading and count
            this.InsertStartRowTag();
            this.InsertBlankCell();

            this.InsertHeading(heading);
            this.InsertHeadingValue(Count);
            this.InsertEndRowTag();
        }



        //add Header with filename and Time
        private void AddHeaderWFileNameAndTime()
        {
            bool IsFirstFileIncluded = false;
            string Header = string.Empty;


            //Write Header

            Header = ApplicationName + "  " + DILanguage.GetLanguageString("LOG_FILE");

            this.InsertSpan(Header, Constants.LogFileFontTypes[LogFileFontType.ApplicationName]);


            this.InsertHorizontalRegion("red", "2");

            this.InsertHorizontalRegion(string.Empty, "2");


            //Write InputFiles
            this.InsertStartTableTagWAttr();

            this.InsertStartRowTag();

            this.InsertMainHeading(DILanguage.GetLanguageString("INPUT_FILES"));

            foreach (string FileName in this.InputFiles)
            {
                if (!IsFirstFileIncluded)
                {
                    // write FileName
                    this.InsertMainHeadingValue(FileName);

                    this.Writer.WriteEndTag("TR ");
                    IsFirstFileIncluded = true;

                }
                else
                {
                    this.InsertTableRow(string.Empty, FileName);
                }
            }

            //Write BLANK ROWS
            this.InsertBlankRows();

            //Write Output Files
            this.InsertTableRow(DILanguage.GetLanguageString("OUTPUT_FILE"), string.IsNullOrEmpty(this.OutputFileNameWPath) ? Constants.OutputFileWPathTag : this.OutputFileNameWPath);

            //Write BLANK ROWS
            this.InsertBlankRows();


            if (this.ShowTime)
            {
                this.DefaultStartDateTime = DateTime.Now.ToString();


                //Write Date,StartTime
                this.InsertTableRow(DILanguage.GetLanguageString("START_DATETIME"), string.IsNullOrEmpty(StartDateTime) ? DefaultStartDateTime : StartDateTime);

                //Write Date,EndTime
                this.InsertTableRow(DILanguage.GetLanguageString("END_DATETIME"), Constants.ChangedInLast);
            }

            //Insert Blank Row
            this.InsertBlankRows();
        }



        //Insert End Tag
        private void EndTag()
        {
            this.Writer.WriteEndTag("TABLE ");
        }

        //Insert Blank Row
        private void BlankRow()
        {
            this.Writer.WriteBeginTag("TR ");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
            this.Writer.WriteBeginTag("TD ");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
            this.Writer.WriteEndTag("TD");
            this.Writer.WriteBeginTag("TD ");
            this.Writer.Write(HtmlTextWriter.TagRightChar);
            this.Writer.WriteEndTag("TD");
            this.Writer.WriteEndTag("TR ");
        }

        #region"---Replace Start and End Time--"

        //Replace End Processing Time 
        private void ReplaceEndTime(string newValue, string oldValue)
        {


            //Enter some text that you want to search and replace
            //  DateTime EndDate = "1/1/0001 12:00:00 AM";
            //string find = EndDate.ToString();
            //string find = Constants.ChangedInLast;

            int replaced = 0;

            StreamReader sr = new StreamReader(this.LogFilePath);
            string content = sr.ReadToEnd();
            sr.Close();

            if (ReplaceText(ref content, oldValue, newValue, ref replaced))
            {
                StreamWriter sw = new StreamWriter(LogFilePath);
                sw.Write(content);
                sw.Flush();
                sw.Close();
                //TODO: Add the files to a collection that were affected.
            }

        }

        private void AppendHTMLMetaTag()
        {          

            StreamReader sr = new StreamReader(this.LogFilePath);
            string content = sr.ReadToEnd();
            sr.Close();

            content = "<HTML><meta http-equiv=\"content-type\" content=\"text/html;charset=utf-8\" /><body>" + content;
            content = content + "</body></HTML>";
        
            StreamWriter sw = new StreamWriter(LogFilePath);
            sw.Write(content);
            sw.Flush();
            sw.Close();


        }


        //Used For searching Index where we have to replace End date
        private bool ReplaceText(ref string content, string oldValue, string newValue, ref int replaced)
        {
            Boolean isReplaced = false;
            int startIndex = 0;


            if (oldValue != newValue)
            {
                while (startIndex != -1)
                {
                    startIndex = content.IndexOf(oldValue, startIndex);
                    if (startIndex != -1)
                    {
                        content = content.Remove(startIndex, oldValue.Length);
                        content = content.Insert(startIndex, newValue);
                        replaced += 1;
                        isReplaced = true;
                    }
                }
            }


            return isReplaced;
        }

        private void ReplaceOutputFileName(string newValue, string oldValue)
        {
            int replaced = 0;

            StreamReader sr = new StreamReader(this.LogFilePath);
            string content = sr.ReadToEnd();
            sr.Close();

            if (ReplaceText(ref content, oldValue, newValue, ref replaced))
            {
                StreamWriter sw = new StreamWriter(LogFilePath);
                sw.Write(content);
                sw.Flush();
                sw.Close();

            }

        }



        /// <summary>
        /// Check and replace Time of file
        /// </summary>
        /// <param name="dateTime"></param>
        private void CheckNReplace(string dateTime, string oldvalue)
        {

            DateTime updatedDate = DateTime.Now;
            string NewValue = updatedDate.ToString();

            if (!string.IsNullOrEmpty(dateTime))
            { NewValue = dateTime; }

            string OldValue = Constants.ChangedInLast;
            this.ReplaceEndTime(NewValue, oldvalue);



        }


        #endregion





        #endregion




        #endregion



    }













}
