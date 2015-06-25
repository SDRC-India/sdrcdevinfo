using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;


namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    /// <summary>
    /// Base Source Class
    /// </summary>
    public abstract class TemplateMergingControlSource
    {

        #region "-- private --"

        #region "-- Variables --"

        protected MergeTemplateQueries ImportQueries = null;

        #endregion

        #region "-- Methods --"

        #region "-- Methods --"

        protected abstract void InitializeVaribles();

        protected void DeleteRecord(string unmatchedNId)
        {
            try
            {
                foreach (DataRow row in this.UnmatchedTable.Rows)
                {
                    if (Convert.ToString(row[this.KeyColumnName]) == unmatchedNId)
                    {
                        row.Delete();
                        this._UnmatchedTable.AcceptChanges();
                        break;
                    }
                }

            }
            catch (Exception ex) { throw ex; }
        }

        public void SetForMappedTable()
        {
            string UnMatchedNId=string.Empty;
           
            foreach (DataRow Row in this.MappedTable.MappedTable.Rows)
            {
                UnMatchedNId =Convert.ToString(Row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + this._KeyColumnName]);
                this.DeleteRecord(UnMatchedNId);
            }

        }

        #endregion


        #endregion

        #endregion

        #region "-- Public / Friend --"

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
        
        protected List<string> _SourceFilesWPath;

        public List<string> SourceFilesWPath
        {
            get { return this._SourceFilesWPath; }
            set { this._SourceFilesWPath = value; }
        }
	      
        internal bool _IsAllowMapping=true;
        /// <summary>
        /// Gets ture/false. IF false then dont show mapping grids.
        /// </summary>
        public bool IsAllowMapping
        {
            get { return this._IsAllowMapping; }            
        }

        internal bool _ISValidationReqOnMapClick=false;
        /// <summary>
        /// Gets true/false. IF true then do validation on Map button_click()
        /// </summary>
        public bool ISValidationReqOnMapClick
        {
            get { return this._ISValidationReqOnMapClick; }
            
        }
	

        private ApplicationType _SourceApplicationType= ApplicationType.MergeTemplateType;
        /// <summary>
        /// Gets or sets Application type
        /// </summary>
        public ApplicationType SourceApplicationType
        {
            get { return this._SourceApplicationType; }
            set { this._SourceApplicationType = value; }
        }
	

        protected string _CurrentTemplateFileNameWPath = string.Empty;
        /// <summary>
        /// Set Current TemplateFileName
        /// </summary>
        public string CurrentTemplateFileNameWPath
        {
            get { return this._CurrentTemplateFileNameWPath; }
            set { this._CurrentTemplateFileNameWPath = value; }
        }

        protected ICType _CurrentICType;
        /// <summary>
        /// Get or Set ICType
        /// </summary>
        public ICType CurrentICType
        {
            get { return this._CurrentICType; }
            set { this._CurrentICType = value; }
        }


        protected Dictionary<string, string> _DisplayColumnsInfo;
        /// <summary>
        /// Gets or sets display column info. Key is actual column name and value is display string
        /// </summary>
        public Dictionary<string, string> DisplayColumnsInfo
        {
            get { return this._DisplayColumnsInfo; }
            set { this._DisplayColumnsInfo = value; }
        }

        protected string _KeyColumnName;
        /// <summary>
        /// Gets or sets Key column name like for Indicator table- INDICATOR_NID
        /// </summary>
        public string KeyColumnName
        {
            get { return this._KeyColumnName; }
            set { this._KeyColumnName = value; }
        }

        protected string _GlobalColumnName;
        /// <summary>
        /// Gets or sets Global column name like for Indicator table- INDICATOR_Global
        /// </summary>
        public string GlobalColumnName
        {
            get { return this._GlobalColumnName; }
            set { this._GlobalColumnName = value; }
        }

        protected DIConnection _TargetDBConnection;
        /// <summary>
        /// Target Database DIConnection
        /// </summary>
        public DIConnection TargetDBConnection
        {
            get { return this._TargetDBConnection; }
            set { this._TargetDBConnection = value; }
        }

        protected DIQueries _TargetDBQueries;
        /// <summary>
        /// Target Database DIQueries
        /// </summary>
        public DIQueries TargetDBQueries
        {
            get { return this._TargetDBQueries; }
            set
            {
                this._TargetDBQueries = value;
                this.ImportQueries = new MergeTemplateQueries(this._TargetDBQueries.DataPrefix, this._TargetDBQueries.LanguageCode);
            }
        }


        protected MappedTableInfo _MappedTable;
        /// <summary>
        /// Gets or Set MappedTable
        /// </summary>
        public MappedTableInfo MappedTable
        {
            get
            {
                if (this._MappedTable == null)
                    this._MappedTable = this.InitializeMappedTableInfo();

                return this._MappedTable;
            }
            set
            {
                this._MappedTable = value;
                this.SetForMappedTable();
            }
        }

        protected DataTable _UnmatchedTable;
        /// <summary>
        /// Get Unmatched Table
        /// </summary>
        public DataTable UnmatchedTable
        {
            get
            {
                if (this._UnmatchedTable == null)
                    this._UnmatchedTable = this.GetUnmatchedTable();
                return this._UnmatchedTable;
            }
            set
            {
                this._UnmatchedTable = value;
            }

        }


        protected DataTable _AvailableTable;
        /// <summary>
        /// Get Aavailable Table
        /// </summary>
        public DataTable AvailableTable
        {
            get
            {
                if (this._AvailableTable == null)
                    this._AvailableTable = this.GetAvailableTable();
                return this._AvailableTable;
            }
            set
            {
                this._AvailableTable = value;
            }

        }

        #endregion

        #region "-- Method --"

        protected abstract bool IsElementAlreadyExists(DataRow unmatchedRow, DataRow availableRow);


        public abstract DataTable GetAvailableTable();

        public abstract DataTable GetUnmatchedTable();

        #region "-- Mapped Tables --"
        /// <summary>
        /// Initialize MappTableInfo Object and Set Columns
        /// </summary>
        /// <returns></returns>
        public virtual MappedTableInfo InitializeMappedTableInfo()
        {
            MappedTableInfo RetVal;
            try
            {
                RetVal = new MappedTableInfo();

                RetVal.MappedRows = new Dictionary<string, MappedRowInfo>();

                //-- Set Columns
                RetVal.TableColumnsInfo = new List<string>();

                RetVal.TableColumnsInfo.Add(this.KeyColumnName);

                foreach (DataColumn ACol in this.AvailableTable.Columns)
                {
                    RetVal.TableColumnsInfo.Add(MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + ACol);
                }

                foreach (DataColumn UCol in this.UnmatchedTable.Columns)
                {
                    RetVal.TableColumnsInfo.Add(MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + UCol);
                }



            }
            catch (Exception ex) { throw ex; }

            return RetVal;
        }

        public virtual void MapRecord(string unmatchedKey, string availableKey)
        {
            MappedRowInfo RowInfo = null;

            try
            {
                //-- Initialize  MappedTable if not Initialized
                if (this._MappedTable.MappedRows == null)
                    this._MappedTable.MappedRows = new Dictionary<string, MappedRowInfo>();

                //-- Check Key value Exist or Not
                if (!this._MappedTable.MappedRows.ContainsKey(availableKey))
                {
                    RowInfo = new MappedRowInfo();

                    //-- Get Unmatched and Available Row
                    RowInfo.AvailableRow = this._AvailableTable.Copy().Select(this.KeyColumnName + "='" + DICommon.RemoveQuotes(availableKey) + "'")[0];
                    RowInfo.UnmatchedRow = this._UnmatchedTable.Copy().Select(this.KeyColumnName + "='" + DICommon.RemoveQuotes(unmatchedKey) + "'")[0];
                    RowInfo.KeyValue = availableKey;

                    this._MappedTable.MappedRows.Add(availableKey, RowInfo);
                    this.DeleteRecord(unmatchedKey);
                }
            }
            catch (Exception ex) { throw ex; }


        }

        public virtual void RemoveMapRecord(string keyNId)
        {
            string UnmatchedNid = string.Empty;

            if (this._MappedTable.MappedRows.ContainsKey(keyNId))
            {
                this._MappedTable.MappedRows.Remove(keyNId);
            }
            this._UnmatchedTable = this.GetUnmatchedTable();
            this._AvailableTable = this.GetAvailableTable();

            foreach (string MapKey in this._MappedTable.MappedRows.Keys)
            {
                //-- Get Unmatched Nids
                UnmatchedNid = Convert.ToString(this._MappedTable.MappedRows[MapKey].UnmatchedRow[this._KeyColumnName]);
                this.DeleteRecord(UnmatchedNid);
            }
        }

        public virtual bool ISMappedValueExist(DataRow row)
        {
            bool RetVal = false;

            //if ((this.UnmatchedTable.Select(this.KeyColumnName + "='" + row[Constants.Columns.UNMATCHED_COL_Prefix + this.KeyColumnName] + "'").Length > 0) && (this.UnmatchedTable.Select(this.KeyColumnName + "='" + row[Constants.Columns.AVAILABLE_COL_Prefix + this.KeyColumnName] + "'").Length > 0))  
            //    RetVal = true;

            return RetVal;

        }

        #endregion


        public abstract void Import(string selectedGIds);

        /// <summary>
        /// Returns ture if value already exists in target database.
        /// </summary>
        /// <returns></returns>
        public bool ISAlreadyExists(DataRow unmatchedRow, DataRow availableRow)
        {
            bool RetVal = false;

            // Note: pls check factory class to see the value of ISValidationReqOnMapClick variable
            // Check Only if ISValidationReqOnMapClick is true
            if (this.ISValidationReqOnMapClick)
            {
                RetVal = this.IsElementAlreadyExists(unmatchedRow,availableRow);
            }

            return RetVal;
        }

        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise ProgressBar Increment Event
        /// </summary>
        protected  void RaiseProgressBarIncrement(int value)
        {
            // -- Set ProgressBar Value If it is initialized.
            if (this.ProgressBar_Increment != null)
                this.ProgressBar_Increment(value);
        }

        /// <summary>
        /// To Initialize ProgressBar 
        /// </summary>
        protected void RaiseProgressBarInitialize(int maximumValue)
        {
            // --Initilize progressbar and Set ProgressBar Maximum Value to its maximum value.
            if (this.ProgressBar_Initialize != null)
                this.ProgressBar_Initialize(maximumValue);
        }

        /// <summary>
        /// Raise the progressbar`s close event.
        /// </summary>
        protected  void RaiseProgressBarClose()
        {
            // -- Close the progressbar
            if (this.ProgressBar_Close != null)
                this.ProgressBar_Close();
        }

        #endregion

        #endregion

        #endregion

    }
}
