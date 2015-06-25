using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.IO;
using DevInfo.Lib.DI_LibBAL.Utility.MRU;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// A delegate for ProcessStarted and ProcessStopped events
    /// </summary>
    public delegate void Process();


    /// <summary>
    /// A delegate for InitializeProgressBar event
    /// </summary>
    /// <param name="value"></param>
    public delegate void InitializeProgressBarDelegate(int value);

    /// <summary>
    /// Helps in LookInWindowControl.It provides methods for getting and importing records 
    /// </summary>
    public abstract class BaseLookInSource:IDisposable  
    {
        #region "-- Private --"

        #region "-- Variables --"

        private string DataPrefix = string.Empty;
        private string LanguageCode = string.Empty;

        #endregion

          #endregion

        #region "-- Protected --"

        #region "-- Variables --"

       
        
        #endregion

        #region "-- Methods --"

       protected abstract string GetSqlQuery(string searchString);
        protected abstract void ProcessDataTable(ref DataTable table);

 
        /// <summary> 
        /// Reads a file and returns the text 
        /// </summary> 
        /// <param name="filePath">Path of the file</param> 
        /// <returns>Content of the file</returns> 
        protected string GetFileText(string filePath)
        {
            string RetVal = string.Empty;
            StreamReader Reader;

            if (File.Exists(filePath))
            {
               Reader = new StreamReader(filePath);
                RetVal = Reader.ReadToEnd();
                RetVal = RetVal.Trim();
                Reader.Close();
                Reader.Dispose();
                Reader = null;
            }

            return RetVal;
        }

       

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables/Properties --"

        private bool _ShowDESButton = false;
        /// <summary>
        /// Gets or sets true or false. Ture to display ShowDESButton button which allows user to import data/metadata from excel, rtf or xml. 
        /// </summary>
        public bool ShowDESButton
        {
            get { return this._ShowDESButton; }
            set { this._ShowDESButton = value; }
        }

        private bool _IsEnableSearching = true;
        /// <summary>
        /// Gets or sets true or false. True to enable search option otherwise false. True is by default.
        /// </summary>
        public bool IsEnableSearching
        {
            get { return this._IsEnableSearching; }
            set
            {
                this._IsEnableSearching = value;
            }
        }


        protected string _XsltFolderPath;
        /// <summary>
        /// Gets or sets the Xslt folder path
        /// </summary>
        /// <remarks>TemplateFolderPath + "\Metadata\XSLT\" </remarks>
        public string XsltFolderPath
        {
            get
            {
                return this._XsltFolderPath;
            }
            set
            {
                this._XsltFolderPath = value;
            }
        }

        protected List<string> _SelectedFiles;
        /// <summary>
        /// Gets or sets selected xml, excel or rtf files
        /// </summary>
        public List<string> SelectedFiles
        {
            get
            {
                return this._SelectedFiles;
            }
            set
            {
                this._SelectedFiles = value;
            }
        }

        private DataSourceType _InputFileType;
        /// <summary>
        /// Gets or Sets input data source file type
        /// </summary>
        public DataSourceType InputFileType
        {
            get
            {
                return this._InputFileType;
            }
            set
            {
                this._InputFileType = value;
            }
        }

        private DataSourceType _ImportFileType;
        /// <summary>
        /// Gets or Sets import file type. This property is used for getting metadata and data from excel, rtf or xml file.
        /// </summary>
        public DataSourceType ImportFileType
        {
            get
            {
                return this._ImportFileType;
            }
            set
            {
                this._ImportFileType = value;
            }
        }

        private int _SelectedNidInTrgDatabase;
        /// <summary>
        /// Gets or set selected nid in target database only when to import data from excel, xml or rtf file.
        /// </summary>
        public int SelectedNidInTrgDatabase
        {
            get { return this._SelectedNidInTrgDatabase; }
            set { this._SelectedNidInTrgDatabase = value; }
        }


        protected ICType _IndicatorClassificationType;
        /// <summary>
        /// Gets or sets the indicator classification type
        /// </summary>
        public ICType IndicatorClassificationType
        {
            get
            {
                return this._IndicatorClassificationType;
            }
            set
            {
                this._IndicatorClassificationType = value;
            }
        }

        private string _FileSelectionFilterString = string.Empty;
        /// <summary>
        /// Gets or sets filter string for file selection
        /// </summary>
        public string FileSelectionFilterString
        {
            get { return this._FileSelectionFilterString; }
            set { this._FileSelectionFilterString = value; }
        }


        protected MRUKey _DefaultFileType = MRUKey.MRU_DATABASES;
        /// <summary>
        /// Gets or Sets default File type which will be used to save and display last browsed location.
        /// </summary>
        public MRUKey DefaultFileType
        {
            get
            {
                return this._DefaultFileType;
            }
            set
            {
                this._DefaultFileType = value;
            }
        }

       

        protected DIConnection _TargetDBConnection;
        /// <summary>
        /// Sets existing instance of DIConnection class.
        /// </summary>
        public DIConnection TargetDBConnection
        {
            set
            {
                this._TargetDBConnection = value;

            }
        }

        protected DIQueries _TargetDBQueries;
        /// <summary>
        /// Gets or sets instance of DIQueries
        /// </summary>
        public DIQueries TargetDBQueries
        {
            get
            {
                return this._TargetDBQueries;
            }
            set
            {
                this._TargetDBQueries = value;
            }
        }

 

        protected bool _ShowIUS;
        /// <summary>
        /// Set true/false. True to show indicator,unit and subgroup
        /// </summary>
        public bool ShowIUS
        {
            get 
            {
                return this._ShowIUS;
            }
            set
            {
                this._ShowIUS = value;
            }
        }

        protected bool _ShowSector;
        /// <summary>
        /// Set true/false. True to show Sector.
        /// </summary>
        public bool ShowSector
        {
            get
            {
                return this._ShowSector;
            }
            set
            {
                this._ShowSector = value;
            }
        }

        protected int _ImageIndex;
        /// <summary>
        /// Gets or sets image index for list view items
        /// </summary>
        public int ImageIndex
        {
            get
            {
                return this._ImageIndex;
            }
            set
            {
                this._ImageIndex = value;
            }
        }
       
               private Dictionary<string,string> _Columns= new Dictionary<string, string>();
               /// <summary>
               /// Gets or sets columns details. Keyis actual column name and value is display name
               /// </summary>
               public Dictionary<string,string> Columns
               {
                   get { return this._Columns; }
                   set { this._Columns = value; }
               }

               private string _TagValueColumnName=string.Empty;
        /// <summary>
        /// Gets or sets tag value column name
        /// </summary>
               public string TagValueColumnName
               {
                   get { return this._TagValueColumnName; }
                   set { this._TagValueColumnName = value; }
               }

               private string _GlobalValueColumnName1=string.Empty;
        /// <summary>
        /// Gets or sets global value column name1
        /// </summary>
               public string GlobalValueColumnName1
               {
                   get { return this._GlobalValueColumnName1; }
                   set { this._GlobalValueColumnName1 = value; }
               }

        private string _GlobalValueColumnName2 = string.Empty;
               /// <summary>
               /// Gets or sets global value column name1
               /// </summary>
               public string GlobalValueColumnName2
               {
                   get { return this._GlobalValueColumnName2; }
                   set { this._GlobalValueColumnName2 = value; }
               }

        private string _GlobalValueColumnName3 = string.Empty;
               /// <summary>
               /// Gets or sets global value column name1
               /// </summary>
               public string GlobalValueColumnName3
               {
                   get { return this._GlobalValueColumnName3; }
                   set { this._GlobalValueColumnName3 = value; }
               }

               private DIConnection _SourceDBConnection=null;
        /// <summary>
        /// Gets or sets source database connection object
        /// </summary>
               public DIConnection SourceDBConnection
               {
                   get { return this._SourceDBConnection; }
                   set { this._SourceDBConnection = value; }
               }

               private DIQueries _SourceDBQueries;
        /// <summary>
        /// Gets or sets Source database queries object
        /// </summary>
               public DIQueries SourceDBQueries
               {
                   get { return this._SourceDBQueries; }
                   set { this._SourceDBQueries = value; }
               }

               private DataTable _SourceTable=null;
        /// <summary>
        /// Gets or sets source data table
        /// </summary>
               public DataTable SourceTable
               {
                   get { return this._SourceTable; }
                   set { this._SourceTable = value; }
               }

               private int _SelectedAreaLevel;
        /// <summary>
        /// Gets or sets selected area level for area tree
        /// </summary>
               public int SelectedAreaLevel
               {
                   get { return this._SelectedAreaLevel; }
                   set { this._SelectedAreaLevel = value; }
               }
               
			

               private bool _IncludeMap=false;
        /// <summary>
        /// Gets or sets true/false. Set true to import maps 
        /// </summary>
               public bool IncludeMap
               {
                   get 
                   {
                       return this._IncludeMap; 
                   }
                   set
                   {
                       this._IncludeMap = value; 
                   }
               }


               private bool _IncludeArea=false;
               /// <summary>
               /// Gets or sets true/false. Set true to import Area 
               /// </summary>
        public bool IncludeArea
               {
                   get
                   {
                       return this._IncludeArea;
                   }
                   set
                   {
                       this._IncludeArea = value;
                   }
               }
			
			
        
       #endregion

        #region "-- Events --"
        /// <summary>
        /// Fires before starting the process
        /// </summary>
        public event Process ProcessStarted;

        /// <summary>
        /// Fires after process is completed or stopped
        /// </summary>
        public event Process ProcessStopped;

        /// <summary>
        /// Fires to increment progress bar
        /// </summary>
        public event InitializeProgressBarDelegate IncrementProgessBar;

        /// <summary>
        /// Fires to initialize progress bar
        /// </summary>
        public event InitializeProgressBarDelegate InitializeProgressBar;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public abstract void ImportValues(List<string> selectedNids,bool allSelected);

        /// <summary>
        /// Returns data table on the basis of source type
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string searchString)
        {
            string SqlQuery = string.Empty;
            DataTable SourceTable=null;

            try
            {
                //Step1: Get sqlquery 
                SqlQuery = this.GetSqlQuery(searchString);

                //Step2: Execute query and get datatable
                SourceTable = this.SourceDBConnection.ExecuteDataTable(SqlQuery);


                this.ProcessDataTable(ref SourceTable);
            }
            catch (Exception ex)
            {                
            }

            return SourceTable;
        }

        /// <summary>
        /// Returns the max area level
        /// </summary>
        /// <returns></returns>
        public int GetMaxAreaLevel()
        {
            int RetVal = 0;
          
            try
            {

              Int32.TryParse(Convert.ToString( this.SourceDBConnection.ExecuteScalarSqlQuery(this.SourceDBQueries.Area.GetMaxAreaLevelFrmAreaTable())),out RetVal);
           
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
               
        }

        /// <summary>
        /// Creates the connection with  source database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void ConnectToSourceDatabase(string fileNameWPath)
        {

            this.SourceDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, "unitednations2000"));
            this.DataPrefix = this.SourceDBConnection.DIDataSetDefault();
            this.LanguageCode = this.TargetDBQueries.LanguageCode;
            this.SourceDBQueries = new DIQueries(this.DataPrefix, this.LanguageCode);
        }

        /// <summary>
        /// Creates the connection with  source database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void ConnectToSQLDatabase(DIConnectionDetails connectionDetails)
        {
            this.SourceDBConnection = new DIConnection(connectionDetails);
            this.DataPrefix = this.SourceDBConnection.DIDataSetDefault();
            this.LanguageCode = this.TargetDBQueries.LanguageCode;
            this.SourceDBQueries = new DIQueries(this.DataPrefix, this.LanguageCode);
        }
    
        #region "-- Raise Event --"
        /// <summary>
        /// Raises the ProcessStartedEvent
        /// </summary>
        public void RaiseProcessStartedEvent()
        {
            if (this.ProcessStarted != null)
            {
                this.ProcessStarted();
            }
        }

        /// <summary>
        /// Raises the ProcessStoppeddEvent
        /// </summary>
        public void RaiseProcessStoppedEvent()
        {
            if (this.ProcessStopped != null)
            {
                this.ProcessStopped();
            }
        }

        /// <summary>
        /// Raises the IncrementProcessBarEvent
        /// </summary>
        public void RaiseIncrementProgessBarEvent(int value)
        {
            if (this.IncrementProgessBar != null)
                this.IncrementProgessBar(value);
        }

        /// <summary>
        /// Raises the InitializeProcessBarEvent
        /// </summary>
        public void RaiseInitializeProgessBarEvent(int maxValue)
        {
            if (this.InitializeProgressBar != null)
                this.InitializeProgressBar(maxValue);
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public abstract void SetColumnsInfo();

        #region IDisposable Members
        /// <summary>
        /// Disposes the source database connection
        /// </summary>
        public void Dispose()
        {
            if (this.SourceDBConnection != null)
            {
                this.SourceDBConnection.Dispose();
            }
        }

        #endregion

       #endregion

        #endregion

        #endregion


    }
}


