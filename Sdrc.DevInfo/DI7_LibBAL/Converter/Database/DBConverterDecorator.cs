using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;


namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting DevInfo database into latest version.
    /// </summary>
    /// <remarks>
    /// STEP 1. Check if DB is DI6 compliant - existence of VERSION table
    /// STEP 2. If VERSION table does not exist then check for DI5SP2 tables
    /// STEP 2.1. If not DI5SP2 compliant then 
    /// STEP 2.1.1 convert the DB into DI5SP2
    /// STEP 2.2. Convert the DB into DI6 format
    /// 
    /// After updating the database schema,  do the following changes
    /// 1. Add a new class for new version (like DI6_0_0_2DBConverter)
    /// 2. Never forget to invoke base class construtor and methods
    /// 3. Initialize this.DatabaseConverter object with the latest version class in CreateDBConverter Method()
    /// </remarks>
    public class DBConverterDecorator : IDisposable
    {
        #region -- Private --

        #region -- Variables / Properties --

        private DBConverter DatabaseConverter;
        private bool IsLogFileRequired = false;


        #endregion

        #region -- Methods --

        private void CreateDBConverter(DIConnection dbConnection, DIQueries dbQueries)
        {
            #region "-- change here --"

            this.DatabaseConverter = new DI7_0_0_1DBConverter(dbConnection, dbQueries);

            #endregion

            // add event handlers
            this.DatabaseConverter.ProcessInfo += new DevInfo.Lib.DI_LibBAL.DA.DML.ProcessInfoDelegate(DatabaseConverter_ProcessInfo);
            this.DatabaseConverter.ProcessStarted += new DevInfo.Lib.DI_LibBAL.DA.DML.ProcessInfoDelegate(DatabaseConverter_ProcessStarted);
        }


        private void ConnectToDatabaseNCreateConverter(string fileNameWPath)
        {
            DIConnection DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);

            DIQueries DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

            this.CreateDBConverter(DBConnection, DBQueries);
        }


        #region "-- DatabaseConverter : Events --"

        private void DatabaseConverter_ProcessStarted(int number)
        {
            if (this.ProcessStarted != null)
            {
                this.ProcessStarted(number);
            }
        }

        private void DatabaseConverter_ProcessInfo(int number)
        {
            if (this.ProcessInfo != null)
            {
                this.ProcessInfo(number);
            }
        }

        #endregion

        #endregion

        #endregion

        #region -- Public --

        #region -- Variables / Properties --

        #region "-- Change here also --"

        private static string _LatestDBFilePostFix=Constants.DBFilePostFix.DI7_0_0_0;
        /// <summary>
        /// Gets latest DBFilePost fix string
        /// </summary>
        public static string LatestDBFilePostFix
        {
            get { return _LatestDBFilePostFix; }
        }
        
        

        #endregion

        protected bool _ConvertToLatestDatabase = true;
        /// <summary>
        /// Set false to stop convert database to DI7
        /// </summary>
        public bool ConvertToLatestDatabase
        {
            get { return this._ConvertToLatestDatabase; }
            set { this._ConvertToLatestDatabase = value; }
        }

        protected string _DBFilePostfix = string.Empty;
        /// <summary>
        /// Gets database file post fix. If database is in latest format then  DBFilePostfix will be empty/blank.
        /// </summary>
        public string DBFilePostfix
        {
            get { return this._DBFilePostfix; }

        }
	

       

        #endregion

        #region -- New/Dispose --

        private DBConverterDecorator()
        {
            //donoting
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public DBConverterDecorator(string fileNameWPath)
        {
            //create connection and queries object 
            this.ConnectToDatabaseNCreateConverter(fileNameWPath);
            this.DatabaseConverter.FilenameWPath = fileNameWPath;

            if (!string.IsNullOrEmpty(fileNameWPath))
            {
                this.IsLogFileRequired = true;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public DBConverterDecorator(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.CreateDBConverter(dbConnection, dbQueries);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="tempfolderPath"></param>
        public DBConverterDecorator(string fileNameWPath, string tempfolderPath)
        {
            string TempFileNameWPath = (tempfolderPath + "\\" + Path.GetFileName(fileNameWPath));
            //copy file into temp location
            File.Copy(fileNameWPath, TempFileNameWPath, true);

            this.ConnectToDatabaseNCreateConverter(TempFileNameWPath);
            this.DatabaseConverter.FilenameWPath = fileNameWPath;
            this.DatabaseConverter.TempFolderPath = tempfolderPath;
            this.DatabaseConverter.TempFilenameWPath = TempFileNameWPath;

            if (!string.IsNullOrEmpty(fileNameWPath))
            {
                this.IsLogFileRequired = true;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.DatabaseConverter != null)
                this.DatabaseConverter.Dispose();
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
        /// Returns ture if database is valid otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;
            RetVal = DatabaseConverter.IsValidDB(forOnlineDB);

            // reset DBFilePostFix
            if (RetVal)
            {
                this._DBFilePostfix = string.Empty;
            }
            else
            {
                this._DBFilePostfix = DatabaseConverter.DBFilePostfix;
            }

            return DatabaseConverter.IsValidDB(forOnlineDB);
        }

        /// <summary>
        /// Converts the database into latest format.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public bool DoConversion(bool forOnlineDB)
        {
            return this.DoConversion(forOnlineDB, false);
        }

        /// <summary>
        /// Converts the database into latest format.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public bool DoConversion(bool forOnlineDB, bool showLogFile)
        {
            return this.DoConversion(forOnlineDB, showLogFile, string.Empty);
        }

        /// <summary>
        /// Converts the database into latest format.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public bool DoConversion(bool forOnlineDB, bool showLogFile,string sourceFileName)
        {
            bool RetVal = false;
            string InputFileName = string.Empty;

            try
            {
                if (!this.DatabaseConverter.IsValidDB(forOnlineDB))
                {
                    InputFileName = this.DatabaseConverter.DBConnection.DIDataSets().Rows[0].ItemArray[1].ToString();
                    //-- Set to convert db to DI7 or not
                    this.DatabaseConverter.ConvertDatabase = this._ConvertToLatestDatabase;
                    RetVal = this.DatabaseConverter.DoConversion(forOnlineDB);

                    //update database name in avl_database table
                    this.DatabaseConverter.DBConnection.InsertNewDBFileName(this.DatabaseConverter.DBConnection.DIDataSetDefault(), Path.GetFileName(this.DatabaseConverter.DBConnection.ConnectionStringParameters.DbName));

                    if (string.IsNullOrEmpty(this.DatabaseConverter.FilenameWPath))
                    {
                        this.DatabaseConverter.FilenameWPath = this.DatabaseConverter.DBConnection.ConnectionStringParameters.DbName;
                    }

                    this.DatabaseConverter.ConversionLogFile = new DBConverterLog(this.DatabaseConverter.FilenameWPath, this.DatabaseConverter.DBConnection, this.DatabaseConverter.DBQueries);

                    if (string.IsNullOrEmpty(sourceFileName))
                    {                        
                        this.DatabaseConverter.ConversionLogFile.GenerateLogFile(showLogFile);
                    }
                    else
                    {                        
                        this.DatabaseConverter.ConversionLogFile.GenerateLogFile(showLogFile,sourceFileName);
                    }
                    // write information into log
                    this.DatabaseConverter.ConversionLogFile.AddMismatchSubgroupInfoIntoLog(this.DatabaseConverter.MismatchSubgroups);
                    this.DatabaseConverter.ConversionLogFile.AddSubgroupInfoWhereTotalIsAddedIntoLog(this.DatabaseConverter.SubgroupsAddedWithTotal);
                    this.DatabaseConverter.ConversionLogFile.AddNewSubgroupInfoIntoLog(this.DatabaseConverter.SubgroupsAddedWNewSubgorup);
                    this.DatabaseConverter.ConversionLogFile.AddWrongMetadataElementsInfo(this.DatabaseConverter.WrongMetadataElementList);
                    this.DatabaseConverter.ConversionLogFile.AddMissingMetadataCategoryTables(this.DatabaseConverter.MissingMetadataCategoryTables);

                    this.DatabaseConverter.ConversionLogFile.CloseLogFile();

                    //if (this.IsLogFileRequired)
                    //{
                    //    this.DatabaseConverter.GenerateLogFile(showLogFile, InputFileName);
                    //}

                    //else
                    //{
                    //    if (string.IsNullOrEmpty(this.DatabaseConverter.FilenameWPath))
                    //    {
                    //        this.DatabaseConverter.FilenameWPath = this.DatabaseConverter.DBConnection.ConnectionStringParameters.DbName;
                    //    }

                    //    this.DatabaseConverter.ConversionLogFile = new DBConverterLog(InputFileName);
                    //    this.DatabaseConverter.ConversionLogFile.GenerateLogFile(showLogFile);

                        
                    //}
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion
    }
}
