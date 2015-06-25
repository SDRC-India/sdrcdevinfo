using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;

namespace DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL
{
    /// <summary>
    /// A delegate for ShowPagingInfo event.
    /// </summary>
    /// <param name="currentPageNo"></param>
    /// <param name="totalPageNo"></param>
    /// <param name="totalRecords"></param>
    public delegate void PagingInfoDeletegate(int currentPageNo, int totalPageNo, int totalRecords);


    /// <summary>
    /// Helps in getting records for TreeSelectionControl
    /// </summary>
    public abstract class BaseSelectionSource
    {
        #region "-- Private --"

        #region "-- Methods --"

        #region "-- Raise Events --"

        private void RaiseShowPagingInfoEvent(int totalRecords)
        {
            if (this.ShowPagingInfo != null)
                this.ShowPagingInfo(this._CurrentPageNo, this._TotalPages, totalRecords);
        }

        #endregion

        private DataTable PagedData(DataTable table)
        {
            DataTable RetVal = new DataTable();
            int FirstRecordNumber;
            int LastRecordNumber;

            //calculate total number of pages
            this._TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(table.Rows.Count / Convert.ToDouble(this._PageSize))));

            //raise event to show paging info event
            this.RaiseShowPagingInfoEvent(table.Rows.Count);

            if (this._TotalPages > 1)
            {
                //get clone of input table
                RetVal = table.Clone();

                FirstRecordNumber = (this._CurrentPageNo - 1) * this._PageSize;
                LastRecordNumber = (this._CurrentPageNo * this._PageSize) - 1;
                if (LastRecordNumber >= table.Rows.Count)
                    LastRecordNumber = table.Rows.Count - 1;


                for (int i = FirstRecordNumber; i <= LastRecordNumber; i++)
                {
                    RetVal.ImportRow(table.Rows[i]);
                }

                RetVal.AcceptChanges();
            }
            else
            {
                RetVal = table;
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region -- public --

        #region -- Variables / Properties --


        #region -- Database related --

        protected DIConnection _DBConnection;
        /// <summary>
        /// Sets reference of DIConnection object.
        /// </summary>
        public DIConnection DBConnection
        {
            set
            {
                this._DBConnection = value;
            }
        }


        private DIQueries _SqlQueries;
        /// <summary>
        /// Gets or sets instance of DIQueries 
        /// </summary>
        public DIQueries SqlQueries
        {
            get
            {
                return this._SqlQueries;
            }
            set
            {
                this._SqlQueries = value;
            }
        }

        private AreaTreeSortType _AreaTreeSortingType = AreaTreeSortType.ByAreaName;
        /// <summary>
        /// Gets or sets area tree sorting type. Default is AreaName
        /// </summary>
        public AreaTreeSortType AreaTreeSortingType
        {
            get { return this._AreaTreeSortingType; }
            set { this._AreaTreeSortingType = value; }
        }

        #endregion

        protected string DataPrefix = string.Empty;
        protected string DBLanguageCode = string.Empty;

        #region "-- Columns Info --"

        protected string _TagValueColumnName;
        /// <summary>
        /// Gets tag value column name like area_nid,ic_nid,etc.
        /// </summary>
        public string TagValueColumnName
        {
            get
            {
                return this._TagValueColumnName;
            }
        }

        protected string _GlobalValueColumnName1;
        /// <summary>
        /// Gets global value column name 1.
        /// </summary>
        public string GlobalValueColumnName1
        {
            get
            {
                return this._GlobalValueColumnName1;
            }
        }


        protected string _GlobalValueColumnName2;
        /// <summary>
        /// Gets global value column name 2.
        /// </summary>
        public string GlobalValueColumnName2
        {
            get
            {
                return this._GlobalValueColumnName2;
            }
        }

        protected string _GlobalValueColumnName3;
        /// <summary>
        /// Gets  global value column name 3.
        /// </summary>
        public string GlobalValueColumnName3
        {
            get
            {
                return this._GlobalValueColumnName3;
            }
        }

        protected string _FirstColumnName;
        /// <summary>
        /// Gets first column name;
        /// </summary>
        public string FirstColumnName
        {
            get
            {
                return this._FirstColumnName;
            }
        }

        protected string _SecondColumnName;
        /// <summary>
        /// Gets  second column name;
        /// </summary>
        public string SecondColumnName
        {
            get
            {
                return this._SecondColumnName;
            }
        }

        protected string _ThirdColumnName;
        /// <summary>
        /// Gets  third column name;
        /// </summary>
        public string ThirdColumnName
        {
            get
            {
                return this._ThirdColumnName;
            }
        }

        protected string _FirstColumnHeader;
        /// <summary>
        /// Gets or Sets first column header;
        /// </summary>
        public string FirstColumnHeader
        {
            get
            {
                return this._FirstColumnHeader;
            }
        }

        protected string _SecondColumnHeader;
        /// <summary>
        /// Gets  second column header;
        /// </summary>
        public string SecondColumnHeader
        {
            get
            {
                return this._SecondColumnHeader;
            }
        }

        protected string _ThirdColumnHeader;
        /// <summary>
        /// Gets  third column header;
        /// </summary>
        public string ThirdColumnHeader
        {
            get
            {
                return this._ThirdColumnHeader;
            }
        }

        private bool _ShowISBNNatureColumn=false;
        /// <summary>
        /// Set true to display ISBN and Source column
        /// </summary>
        public bool ShowISBNNatureColumn
        {
            get { return this._ShowISBNNatureColumn; }
            set { this._ShowISBNNatureColumn = value; }
        }

        #endregion

        protected DevInfo.Lib.DI_LibDAL.Queries.ICType _IndicatorClassificationType;
        /// <summary>
        /// Gets or sets Indicator classifications type
        /// </summary>
        public DevInfo.Lib.DI_LibDAL.Queries.ICType IndicatorClassificationType
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

        private bool _IsAutoFillForAvailableList = false;
        /// <summary>
        /// Gets or sets true/false. True to get records for available list using selections of user preferences
        /// </summary>
        public bool IsAutoFillForAvailableList
        {
            get { return this._IsAutoFillForAvailableList; }
            set { this._IsAutoFillForAvailableList = value; }
        }


        protected int _AreaSelectedLevel = 0;
        ///// <summary>
        ///// Gets or sets selected area level
        ///// </summary>
        public int AreaSelectedLevel
        {
            get
            { return _AreaSelectedLevel; }
            set { _AreaSelectedLevel = value; }
        }

        protected int _SelectedNid = 0;
        /// <summary>
        /// Sets selected area or indicator classification nid 
        /// </summary>
        public int SelectedNid
        {
            set
            {
                this._SelectedNid = value;
            }
        }

        protected int _AreaRequiredLevel = 0;
        /// <summary>
        /// Gets or sets required level for area selection
        /// </summary>
        public int AreaRequiredLevel
        {
            get
            {
                return this._AreaRequiredLevel;
            }
            set
            {
                this._AreaRequiredLevel = value;
            }
        }

        #region "-- Properties for select all and auto select options --"

        protected UserPreference _UserPrefences;
        /// <summary>
        /// Gets or sets userprefence instance
        /// </summary>
        public UserPreference UserPrefences
        {
            get
            {
                if (this._UserPrefences == null)
                {
                    this._UserPrefences = new DevInfo.Lib.DI_LibBAL.UI.UserPreference.UserPreference();
                    this._UserPrefences.UserSelection = new DevInfo.Lib.DI_LibDAL.UserSelection.UserSelection();
                    this._UserPrefences.General = new DevInfo.Lib.DI_LibBAL.UI.UserPreference.UserPreference.GeneralPreference();
                }

                return this._UserPrefences;
            }
            set { this._UserPrefences = value; }
        }

        protected bool _UseIndicator = false;
        /// <summary>
        /// Gets true/false.Ture means use indicator nids for autoselection.
        /// </summary>
        public bool UseIndicator
        {
            get
            {
                return this._UseIndicator;
            }
            set
            {
                this._UseIndicator = value;
            }
        }

        protected bool _UseArea = false;
        /// <summary>
        /// Gets true/false.Ture means use area nids for autoselection.
        /// </summary>        
        public bool UseArea
        {
            get { return this._UseArea; }
            set { this._UseArea = value; }
        }

        protected bool _UseTime = false;
        /// <summary>
        /// Gets true/false.Ture means use timeperiod nids for autoselection.
        /// </summary>        
        public bool UseTime
        {
            get { return this._UseTime; }
            set { this._UseTime = value; }
        }

        protected bool _UseSource = false;
        /// <summary>
        /// Gets true/false.Ture means use source nids for autoselection.
        /// </summary>        
        public bool UseSource
        {
            get { return this._UseSource; }
            set { this._UseSource = value; }
        }

        protected bool _Continue = false;
        /// <summary>
        /// Gets  true/false.True means do autoselection without using selected nids.
        /// </summary>        
        public bool Continue
        {
            get { return this._Continue; }
        }

        protected string _TotalNId = string.Empty;
        /// <summary>
        /// Gets or sets the TotalNId
        /// </summary>
        public string TotalNId
        {
            get
            {
                return this._TotalNId;
            }
            set
            {
                this._TotalNId = value;
            }
        }


        #endregion

        #region "-- Paging --"

        private string _PagingString = string.Empty;
        /// <summary>
        /// Gets paging information like "1 of 5"
        /// </summary>
        public string PagingString
        {
            get
            {
                return this._PagingString;
            }
        }


        private int _TotalPages = 1;
        /// <summary>
        /// Gets total numbers of pages
        /// </summary>
        public int TotalPages
        {
            get
            {
                return this._TotalPages;
            }
        }

        private int _PageSize = 50;
        //Gets or sets page size. Default is 50
        public int PageSize
        {
            get
            {
                return this._PageSize;
            }
            set
            {
                this._PageSize = value;
            }
        }


        private int _CurrentPageNo = 1;
        /// <summary>
        /// Gets or sets Current page number
        /// </summary>
        public int CurrentPageNo
        {
            get
            {
                return this._CurrentPageNo;
            }
            set
            {
                this._CurrentPageNo = value;
            }
        }

        private bool _Paging = false;
        /// <summary>
        /// Gets or sets true/false. Ture to show records in pages.
        /// </summary>
        public bool Paging
        {
            get
            {
                return this._Paging;
            }
            set
            {
                this._Paging = value;

                if (!this._Paging)
                {
                    this._CurrentPageNo = 1;
                    this._TotalPages = 1;
                }
            }
        }

        #endregion

        private bool _ShowAreaGroupInLists = true;
        /// <summary>
        /// Gets or sets true/false.False will not show the area group in available and selected lists. Default is true.
        /// </summary>
        public bool ShowAreaGroupInLists
        {
            get { return this._ShowAreaGroupInLists; }
            set { this._ShowAreaGroupInLists = value; }
        }

        #endregion

        #region "-- Events --"

        public event PagingInfoDeletegate ShowPagingInfo;

        #endregion

        #region -- Methods --

        #region -- Common --

        /// <summary>
        /// To reset columns info
        /// </summary>
        public void ResetColumnInfo()
        {
            //rest column names
            this.SetColumnNames();

            // reset list column header
            this.SetColumnHeaders();
        }


        /// <summary>
        /// To reset columns header
        /// </summary>
        public void ResetColumnHeader()
        {
            //rest columns header
            this.SetColumnHeaders();
        }

        /// <summary>
        /// To intialize properties and other values
        /// </summary>
        public void InitializeProperties()
        {
            // step1: set column names
            this.SetColumnNames();

            // step2: set column names
            this.SetColumnHeaders();

            // step2: intialize DIqueries object. 
            if (this.SqlQueries == null)
            {
                this.DataPrefix = this._DBConnection.DIDataSetDefault();
                this.DBLanguageCode = this._DBConnection.DILanguageCodeDefault(this.DataPrefix);
                this.SqlQueries = new DIQueries(this.DataPrefix, this.DBLanguageCode);
            }
            else
            {
                this.DBLanguageCode = this.SqlQueries.LanguageCode;
                this.DataPrefix = this.SqlQueries.DataPrefix;
            }

        }

        /// <summary>
        /// Returns records from database on the basis of selected nid and parent nid.
        /// </summary>
        /// <param name="selectedNid"></param>
        /// <param name="selectedParentNid"></param>
        /// <returns></returns>
        public DataTable GetAssociatedRecords(int selectedNid, int selectedParentNid)
        {
            DataTable RetVal = new DataTable();
            try
            {
                string SqlQuery = string.Empty;

                // Get sql query to fetch associated records from database
                SqlQuery = this.GetAssocicatedRecordsQuery(selectedNid, selectedParentNid);

                // Get records from database
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
                 
                if (this._Paging)
                {
                    RetVal = this.PagedData(RetVal);
                }
            }
            catch (Exception ex)
            {

            }
            return RetVal;
        }

        //private void WriteError(string ErrorString)
        //{
        //    //string fpath = @"D:\-- WEBSITES --\Devinfo.info\-- DI6 Web Adaptations --\CamInfov4.1\stock\ErrorLogs\Error.txt";
        //    string fpath = @"D:\CamInfo2011\stock\ErrorLogs\Error.txt";
        //    //fpath = System.IO.Path.Combine(""  & ".txt")
        //    if (System.IO.File.Exists(fpath) == false)
        //    {
        //        System.IO.File.WriteAllText(fpath, " ");
        //        WriteInErrorLogFile(fpath, ErrorString);
        //    }
        //    else
        //    {
        //        WriteInErrorLogFile(fpath, ErrorString);
        //    }
        //}

        //private void WriteInErrorLogFile(string fpath, string ErrorString)
        //{
        //    System.IO.StreamWriter wr;
        //    try
        //    {
        //        wr = new System.IO.StreamWriter(fpath, true);
        //        string str = string.Empty;
        //        str = System.DateTime.Now.ToString() + " >> " + ErrorString;
        //        str = str + System.Environment.NewLine;
        //        wr.WriteLine(str);
        //        wr.Flush();
        //        wr.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}




        /// <summary>
        /// Returns records from database on the basis of selected nid and parent nid.
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <returns></returns>
        public DataTable GetAssociatedRecords(string selectedNids)
        {
            DataTable RetVal = new DataTable();
            string SqlQuery = string.Empty;

            // Get sql query to fetch associated records from database
            SqlQuery = this.GetAssocicatedRecordsQuery(selectedNids);

            // Get records from database
            RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);

            if (this._Paging)
            {
                RetVal = this.PagedData(RetVal);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns records from database on the basis of selected nid and parent nid without paging the data.
        /// </summary>
        /// <param name="selectedNid"></param>
        /// <param name="selectedParentNid"></param>
        /// <returns></returns>
        public DataTable GetAssociatedRecordsWithoutPaging(int selectedNid, int selectedParentNid)
        {
            DataTable RetVal = new DataTable();
            string SqlQuery = string.Empty;

            // Get sql query to fetch associated records from database
            SqlQuery = this.GetAssocicatedRecordsQuery(selectedNid, selectedParentNid);

            // Get records from database
            RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);

            return RetVal;
        }


        /// <summary>
        /// Returns all records from database .
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllRecords()
        {
            DataTable RetVal = new DataTable();
            string SqlQuery = string.Empty;


            // Get sql query to fetch all records from database

            SqlQuery = this.GetSelectAllQuery();

            // Get records from database
            RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);



            return RetVal;
        }

        /// <summary>
        /// Returns records for available list auto select option .
        /// </summary>
        /// <param name="availableItemsNid"></param>
        /// <returns></returns>
        public DataTable GetAllRecordsForAvailableListAutoSelect(string availableItemsNid)
        {
            DataTable RetVal;
            // Get records for auto select option
            RetVal = this.GetAutoSelectRecordsForAvailableList(availableItemsNid);

            // set paging if auto fill is true
            if (this._IsAutoFillForAvailableList)
            {
                if (this._Paging)
                {
                    RetVal = this.PagedData(RetVal);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns max area level
        /// </summary>
        /// <returns></returns>
        public int GetMaxAreaLevel()
        {
            int RetVal = 0;
            string SqlString = string.Empty;
            DataTable TempTable = null;
            try
            {
                SqlString = this.SqlQueries.Area.GetMaxAreaLevelFrmAreaTable();
                TempTable = this._DBConnection.ExecuteDataTable(SqlString);
                if (TempTable.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(TempTable.Rows[0][Area_Level.AreaLevel]);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns dataview for the given nids
        /// </summary>
        /// <param name="nids">comma separated nids</param>
        /// <returns></returns>
        public DataView GetRecordsByNids(string nids)
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.GetRecordsForSelectedNids(nids);
                if (!string.IsNullOrEmpty(SqlQuery))
                {
                    RetVal = this._DBConnection.ExecuteDataTable(SqlQuery).DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Get the IC of the selected ius nids
        /// </summary>
        /// <param name="IUSNIds"></param>
        /// <returns></returns>
        public string GetICofIUS(string IUSNIds)
        {
            string RetVal = string.Empty;
            DataTable ICTable;
            DataRow[] Rows = new DataRow[0];
            try
            {
                if (!string.IsNullOrEmpty(IUSNIds))
                {
                    ICTable = this._DBConnection.ExecuteDataTable(this._SqlQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.NId, string.Empty, IUSNIds, this.IndicatorClassificationType, FieldSelection.Light));
                    //Rows = ICTable.Select(IndicatorClassifications.ICParent_NId + " <> -1");

                    foreach (DataRow Row in ICTable.Rows)
                    {
                        RetVal += "," + Row[IndicatorClassifications.ICNId].ToString();
                    }
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal = RetVal.Substring(1);
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #region "-- Abstract --"

        /// <summary>
        /// Returns records for  auto select option of available list.
        /// </summary>
        /// <param name="availableItemsNid">Comma separated nids of available items</param>
        /// <returns></returns>
        public abstract DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid);

        /// <summary>
        /// Returns records for  auto select option of treeview.
        /// </summary>
        /// <returns></returns>
        public abstract DataTable GetAllRecordsForTreeAutoSelect();

        /// <summary>
        /// Update the data table with IUNID and select subgroup columns.
        /// </summary>
        /// <param name="iuTable"></param>
        public abstract void UpdataDataTableBeforeCreatingListviewItem(DataTable iuTable);

        /// <summary>
        /// Get the IUSNIds on the basis of IUNId
        /// </summary>
        /// <param name="iuNIds"></param>
        /// <returns></returns>
        public abstract List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon);

        /// <summary>
        /// Get the subgroup information that need to be render on the IC list
        /// </summary>
        /// <param name="iuNIds"></param>
        /// <returns></returns>
        public abstract List<string> GetSubgroupDimensions(string iuNId, string IUSNIds);

        /// <summary>
        /// Get the subgroup information that need to be render on the IC list
        /// </summary>
        /// <param name="iuNIds">Comma seprated Indicator, Unit NId. Third NId is now obseleted which was previously used for "where data exists"</param>
        /// <param name="IUSNIds">Comma seprated selected IUS NIds</param>
        /// <returns></returns>
        public abstract List<string> GetSubgroupDimensionsWithIU(string iuNId, string IUSNIds);

        /// <summary>
        /// Update the user selection with IU selection details
        /// </summary>
        /// <param name="selectionDetails"></param>
        public abstract void UpdateIndicatorSelectedDetails(int indicatorNId, int unitNId, string selectionDetails, bool addNewSelection);

        /// <summary>
        /// Get the indicator, unit selection details
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public abstract string GetIndicatorSelectionDetails(int indicatorNId, int unitNId);

        #endregion

        /// <summary>
        /// Retruns the records for showIUS button.Invoke this function for ShowIUSButton's click event.
        /// </summary>
        /// <param name="selectedNids">If showIus is true then pass selected IusNid otherwise indicatorNids</param>
        /// <returns></returns>
        public virtual DataTable GetRecordsForShowIUSButton(string selectedNids)
        {
            DataTable RetVal;

            if (!this.UserPrefences.Indicator.ShowIUS)
            {
                RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetDistinctIndicatorUnit(selectedNids, this.UserPrefences.Indicator.ShowIUS));
            }
            else
            {
                string ICNId = this.GetICofIUS(selectedNids);
                if (!string.IsNullOrEmpty(ICNId))
                {
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetIndicatorUnit(selectedNids, ICNId, this.UserPrefences.Indicator.ShowIUS));
                }
                else
                {
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetIndicatorUnit(selectedNids, this.UserPrefences.Indicator.ShowIUS));
                }
            }
            return RetVal;
        }


        #endregion

        #endregion

        #region -- Protected --

        #region -- Methods --

        protected abstract void SetColumnNames();

        protected abstract void SetColumnHeaders();

        protected abstract string GetAssocicatedRecordsQuery(int selectedNid, int selectedParentNid);

        protected abstract string GetSelectAllQuery();

        protected abstract string GetRecordsForSelectedNids(string nids);

        protected DataTable GetDistinctRecordsTable(DataTable table, string[] distinctColumns)
        {
            DataTable RetVal = new DataTable();

            RetVal = table.DefaultView.ToTable(true, distinctColumns);

            return RetVal;
        }

        protected string GetCommaSeparatedValues(DataTable table, string columnName)
        {
            String RetVal = string.Empty;
            StringBuilder Value = new StringBuilder();

            foreach (DataRow Row in table.Rows)
            {
                Value.Append(",");
                Value.Append(Row[columnName].ToString());
            }

            if (Value.Length > 0)
            {
                RetVal = Value.ToString().Substring(1);
            }

            return RetVal;
        }

        protected abstract string GetAssocicatedRecordsQuery(string selectedNids);

        #endregion

        #endregion



    }
}
