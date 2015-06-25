using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.LogFiles;
using System.Data;


namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// The base class which helps in database conversion
    /// </summary>
    public abstract class DBConverter
    {
        #region "-- Private --"



        #region "-- Methods --"

        #region "-- Log File --"


        //////private void AddNewSubgroupInfoIntoLog(DXLogFile logfile)
        //////{
        //////    // "New subgrop added for:"
        //////    if (this.SubgroupsAddedWNewSubgorup.Count > 0)
        //////    {
        //////        DataTable Table = new DataTable();
        //////        Table.Columns.Add("Subgroup");
        //////        Table.Columns.Add("Subgroup dimension value");
        //////        DataRow Row;

        //////        foreach (string key in this.SubgroupsAddedWNewSubgorup.Keys)
        //////        {
        //////            Row = Table.NewRow();
        //////            Row[0] = key;
        //////            Row[1] = this.SubgroupsAddedWNewSubgorup[key].ToString();
        //////            Table.Rows.Add(Row);
        //////        }

        //////        logfile.AddDataTable(Table, "New subgrop added for:");
        //////    }
        //////}

        //////private void AddMismatchSubgroupInfoIntoLog(DXLogFile logfile)
        //////{
        //////    if (this.MismatchSubgroups.Count > 0)
        //////    {
        //////        // "Mismatch subgrop:"
        //////        DataTable Table = new DataTable();
        //////        DataRow Row;


        //////        Table.Columns.Add("Subgroups");



        //////        foreach (string NewSubgroup in this.MismatchSubgroups)
        //////        {
        //////            Row = Table.NewRow();
        //////            Row[0] = NewSubgroup;
        //////            Table.Rows.Add(Row);
        //////        }

        //////        logfile.AddDataTable(Table, "Subgroups created under others :");
        //////    }
        //////}


        //////private void AddSubgroupInfoWhereTotalIsAddedIntoLog(DXLogFile logfile)
        //////{
        //////    if (this.SubgroupsAddedWithTotal.Count > 0)
        //////    {
        //////        // "Subgrop where relationship with total is added:"
        //////        DataTable Table = new DataTable();
        //////        DataRow Row;

        //////        Table.Columns.Add("Subgroup");

        //////        foreach (string SG in this.SubgroupsAddedWithTotal)
        //////        {
        //////            Row = Table.NewRow();
        //////            Row[0] = SG;
        //////            Table.Rows.Add(Row);
        //////        }


        //////        logfile.AddDataTable(Table, "Subgrop where relationship with total is added:");
        //////    }
        //////}

        #endregion

        #endregion

        #endregion

        #region -- public/internal --

        #region -- Variables / Properties --

       
        protected string _FilenameWPath;
        /// <summary>
        /// Gets or sets filename with path
        /// </summary>
        internal string FilenameWPath
        {
            get { return this._FilenameWPath; }
            set { this._FilenameWPath = value; }
        }

        protected string _TempFolderPath;
        /// <summary>
        /// Gets or sets temp folder path
        /// </summary>
        internal string TempFolderPath
        {
            get { return this._TempFolderPath; }
            set { this._TempFolderPath = value; }
        }

        internal string TempFilenameWPath;

        protected DIConnection _DBConnection;
        /// <summary>
        /// Gets or sets the instance of DIConnection
        /// </summary>
        internal DIConnection DBConnection
        {
            get { return this._DBConnection; }
            set { this._DBConnection = value; }
        }

        protected DIQueries _DBQueries;
        /// <summary>
        /// Gets or sets the instance of DIQueries.
        /// </summary>
        internal DIQueries DBQueries
        {
            get { return this._DBQueries; }
            set { this._DBQueries = value; }
        }

        private List<string> _SubgroupsAddedWithTotal = new List<string>();
        /// <summary>
        /// Gets or sets lists of subgroupval whose relationship with total subgroup is added into database
        /// </summary>
        internal List<string> SubgroupsAddedWithTotal
        {
            get
            {
                this._SubgroupsAddedWithTotal.Sort();
                return _SubgroupsAddedWithTotal;
            }
            set { _SubgroupsAddedWithTotal = value; }
        }

        #region -- Log -- 

        protected DBConverterLog _ConversionLogFile;
        /// <summary>
        /// Gets or sets conversion log file
        /// </summary>
        public DBConverterLog ConversionLogFile
        {
            get { return this._ConversionLogFile; }
            set { this._ConversionLogFile = value; }
        }
        private SortedDictionary<string, string> _SubgroupsAddedWNewSubgorup = new SortedDictionary<string, string>();
        /// <summary>
        /// Gets or sets lists of subgroupVal and subgroup (where new subgroup has been create into databse with total )
        /// </summary>
        internal SortedDictionary<string, string> SubgroupsAddedWNewSubgorup
        {
            get
            {

                return this._SubgroupsAddedWNewSubgorup;
            }
            set { this._SubgroupsAddedWNewSubgorup = value; }
        }

        private List<string> _MismatchSubgroups = new List<string>();
        /// <summary>
        /// Gets or sets mismatch subgroups.
        /// </summary>
        internal List<string> MismatchSubgroups
        {
            get
            {
                this._MismatchSubgroups.Sort();
                return this._MismatchSubgroups;
            }
            set { this._MismatchSubgroups = value; }
        }

        protected SortedDictionary<MetadataElementType,List<string>> _WrongMetadataElementList=new SortedDictionary<MetadataElementType,List<string>>();
        /// <summary>
        /// Gets or sets metadata elements list where metadata information is in incorrect format.
        /// </summary>
        internal SortedDictionary<MetadataElementType,List<string>> WrongMetadataElementList
        {
            get { return this._WrongMetadataElementList; }
            set { this._WrongMetadataElementList = value; }
        }

        protected List<string> _MissingMetadataCategoryTables = new List<string>();
        /// <summary>
        /// Gets or sets missing metadata category tables.
        /// </summary>
        internal List<string> MissingMetadataCategoryTables
        {
            get
            {
                return this._MissingMetadataCategoryTables;
            }
            set { this._MissingMetadataCategoryTables = value; }
        }
	
        #endregion

        protected bool _ConvertDatabase = true;
        /// <summary>
        /// Set true to convert database
        /// </summary>
        public bool ConvertDatabase
        {
            get { return this._ConvertDatabase; }
            set { this._ConvertDatabase = value; }
        }

        protected string _DBFilePostfix = Constants.DBFilePostFix.DI6_0_0_1;
        /// <summary>
        /// Gets database file post fix. Default is _v.6.1
        /// </summary>
        public string DBFilePostfix
        {
            get { return this._DBFilePostfix; }

        }
	

        #endregion

        #region -- New/Dispose --

        internal DBConverter(DIConnection dbConnection, DIQueries dbQueries)
        {
            this._DBConnection = dbConnection;
            this._DBQueries = dbQueries;
        }

        #region IDisposable Members

        internal void Dispose()
        {
            this._DBConnection.Dispose();

            if (!string.IsNullOrEmpty(this.TempFilenameWPath))
            {
                try
                {
                    //overwrite the original file with the temp one
                    File.Copy(this.TempFilenameWPath, this.FilenameWPath, true);
                    File.Delete(this.TempFilenameWPath);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        #endregion

        #endregion

        #region "-- Events --"

        /// <summary>
        /// Fires when conversion process is started.
        /// </summary>
        public event ProcessInfoDelegate ProcessStarted;

        /// <summary>
        /// Fires while doning conversion. Use this event to set progressbar value
        /// </summary>
        public event ProcessInfoDelegate ProcessInfo;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Returns true/false. True if database is in valid format otherwise false. 
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public abstract bool IsValidDB(bool forOnlineDB);

        /// <summary>
        /// Returns true/false. True if conversion complete sucessfully otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public abstract bool DoConversion(bool forOnlineDB);


        ///// <summary>
        ///// Generates Log file
        ///// </summary>
        ///// <param name="showLogFile"></param>
        ///// <param name="inputFileNameWPath"></param>
        //public void GenerateLogFile(bool showLogFile, string inputFileNameWPath)
        //{
        //    DXLogFile LogFile = new DXLogFile();
        //    List<string> InputFiles = new List<string>();
        //    string LogFileNameWPath = string.Empty;
        //    string CurrentDateTime = string.Empty;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(this._FilenameWPath))
        //        {
        //            CurrentDateTime = DateTime.Now.ToString().Replace("/", "");
        //            CurrentDateTime = CurrentDateTime.Replace("-", "");
        //            CurrentDateTime = CurrentDateTime.Replace(":", "").Trim();

        //            string FileName = LogFile.GenerateLogFileName("Conversion_LogFile_" + CurrentDateTime);

        //            LogFile.ShowTime = false;
        //            InputFiles.Add(inputFileNameWPath);
        //            LogFileNameWPath = Path.Combine(Path.GetDirectoryName(this._FilenameWPath), FileName);
        //            LogFile.Start(LogFileNameWPath, "Database conversion log file", InputFiles, this._FilenameWPath, this._DBConnection, this._DBQueries);

        //            this.AddNewSubgroupInfoIntoLog(LogFile);
        //            this.AddSubgroupInfoWhereTotalIsAddedIntoLog(LogFile);
        //            this.AddMismatchSubgroupInfoIntoLog(LogFile);
        //            LogFile.Close();

        //            if (showLogFile && File.Exists(LogFileNameWPath))
        //            {
        //                System.Diagnostics.Process.Start(LogFileNameWPath);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }


        //}


        #region "-- Raise Events --"

        internal void RaiseProcessStartedEvent(int totalSteps)
        {
            if (this.ProcessStarted != null)
            {
                this.ProcessStarted(totalSteps);
            }
        }

        internal void RaiseProcessInfoEvent(int number)
        {
            if (this.ProcessInfo != null)
            {
                this.ProcessInfo(number);
            }
        }

        #endregion

        #endregion

        #endregion

    }

}
