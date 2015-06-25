using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.ListViewBAL
{
    /// <summary>
    /// A delegate for InitializedIndicatorListViewSource,InitializedAreaListViewSource,InitializedDISourceListViewSource and InitializedTimeperiodListViewSource events
    /// </summary>
    public delegate void ListSourceInitializedDelegate();

    /// <summary>
    /// Helps in getting records for ListViewControl
    /// </summary>
    public abstract class BaseListViewSource
    {

        #region "-- Private --"

        private string GetIUSNIdsFrmDataTable()
        {
            string RetVal = string.Empty;
            string SqlQuery = string.Empty;
            DataTable Table;
            int SourceNids = 0;
            try
            {
                //get all IUSNID from data table
                SqlQuery = this.DBQueries.IUS.GetDistinctIUSNIdFrmDataTable();

                Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                foreach (DataRow Row in Table.Rows)
                {
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal += ",";
                    }
                    RetVal += Row[IndicatorClassificationsIUS.IUSNId].ToString();
                }

                Table.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private string GetAllIUSRecordsSqlQuery(string indicatorNIds)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;
            string NIds = string.Empty;

            //set filter string
            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                FilterString = " " + Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ";
            }
            else
            {
                FilterString = " 1 =1";
            }

            if (this._ShowIndicatorWithData)
            {
                //get IUSNIDs
                NIds = this.GetIUSNIdsFrmDataTable();

                RetVal = this.DBQueries.Indicators.GetIndicatorWithData(NIds, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);

            }
            else
            {
                RetVal = this.DBQueries.Indicators.GetIndicator(FilterFieldType.Search, FilterString, FieldSelection.Light);
            }

            return RetVal;
        }

        private string GetIUSAutoSelectRecordsSqlQuery(string indicatorNIds)
        {
            string RetVal = string.Empty;
            //get indicator,unit,subgroup (IUS)  where datatable.area=given_area and datatable.timperiod=given_timeperiod and datatable.source = given_source and indicatorNids In(indicatornids);

            RetVal = this.DBQueries.IUS.GetAutoSelectIUSByIndicator(
                this.DIUserPreference.UserSelection.TimePeriodNIds,
                this.DIUserPreference.UserSelection.AreaNIds,
                this.DIUserPreference.UserSelection.SourceNIds, FieldSelection.Light,indicatorNIds);


            return RetVal;
        }


        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"


        private ICType _ClassificationType= ICType.Sector;
        /// <summary>
        /// Gets or sets classification type. Default is sector
        /// </summary>
        public ICType ClassificationType
        {
            get { return _ClassificationType; }
            set { _ClassificationType = value; }
        }
	
        protected List<ColumnInfo> _Columns = new List<ColumnInfo>();
        /// <summary>
        /// Gets list of column
        /// </summary>
        public List<ColumnInfo> Columns
        {
            get
            {
                return this._Columns;
            }
        }

        protected string _TagValueColumnName = string.Empty;
        /// <summary>
        /// Gets tag value column name like AreaNid for arealist, IndicatorNid for indicator list, etc.
        /// </summary>
        public string TagValueColumnName
        {
            get
            {
                return this._TagValueColumnName;
            }
        }

        protected int _AreaLevel = 1;
        /// <summary>
        /// Sets area level for area list
        /// </summary>
        public int AreaLevel
        {
            set
            {
                this._AreaLevel = value;
            }
        }

        protected bool _ShowIndicatorWithData;
        /// <summary>
        /// Sets true/false. Set true to get indicator list with data only.
        /// </summary>
        public bool ShowIndicatorWithData
        {
            set
            {
                this._ShowIndicatorWithData = value;
            }
        }


        #endregion

        #region "-- Events --"

        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with IndicatorListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate InitializedIndicatorListViewSource;

        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with AreaListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate InitializedAreaListViewSource;

        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with DISourceListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate InitializedDISourceListViewSource;

        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with TimeperiodListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate
InitializedTimeperiodListViewSource;


        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with UnitListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate
InitializedUnitListViewSource;


        /// <summary>
        /// Fires when an instance of BaseListViewSource is initialized with SubgroupValListViewSource
        /// </summary>
        public event ListSourceInitializedDelegate
InitializedSubgroupValListViewSource;


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns the max area level
        /// </summary>
        /// <returns></returns>
        public int GetMaxAreaLevel()
        {
            int RetVal = 0;
            DataTable Table;
            try
            {

                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetMaxAreaLevelFrmAreaTable());

                
                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0][Area.AreaLevel]);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;

        }


        /// <summary>
        /// Get all records from database/template
        /// </summary>
        /// <param name="searchString">search string can be empty</param>
        /// <returns></returns>
        public DataTable GetAllRecords(string searchString)
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.GetAllRecordsSqlQuery(searchString));

            return RetVal;
        }

        /// <summary>
        /// Get auto select records only from database/template.
        /// </summary>
        /// <returns></returns>
        public DataTable GetAutoSelectRecords()
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.GetAutoSelectRecordsSqlQuery());

            return RetVal;
        }

        /// <summary>
        /// Get records by NIds.
        /// </summary>
        /// <returns></returns>
        public DataTable GetRecordsByNIds(string NIds)
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.GetRecordsByNIDsSqlQuery(NIds));

            return RetVal;
        }

        /// <summary>
        /// Retruns IndicatorNids from userpreference
        /// </summary>
        /// <returns></returns>
        public abstract string GetSelectedNids();


        /// <summary>
        /// Get IUS all records from database/template
        /// </summary>
        /// <param name="indicatorNIds">comma separated indicator nids which may be blank</param>
        /// <returns></returns>
        public DataTable GetAllIUSRecords(string indicatorNIds)
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.GetAllIUSRecordsSqlQuery(indicatorNIds));

            return RetVal;
        }

        /// <summary>
        /// Get IUS auto select records only from database/template.
        /// </summary>
        /// <param name="indicatorNIds">comma separated indicator nids which may be blank</param>
        /// <returns></returns>
        public DataTable GetIUSAutoSelectRecords(string indicatorNIds)
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.GetIUSAutoSelectRecordsSqlQuery(indicatorNIds));

            return RetVal;
        }

        /// <summary>
        /// Returns datatable of distinct indicators 
        /// </summary>
        /// <param name="IUSNids"></param>
        /// <returns></returns>
        public DataTable GetDistinctIndicatorByIUSNIDs(string IUSNids)
        {
            DataTable RetVal;

            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetDistinctIndicatorsByIUSNIds(IUSNids));

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables --"

        internal DIConnection DBConnection;
        internal DIQueries DBQueries;
        internal UserPreference DIUserPreference;

        #endregion

        #region "-- Methods --"

        #region "-- Methods: Raise Events --"

        internal void RaiseInitializedIndicatorListViewEvent()
        {
            if (this.InitializedIndicatorListViewSource != null)
            {
                this.InitializedIndicatorListViewSource();
            }
        }

        internal void RaiseInitializedAreaListViewEvent()
        {
            if (this.InitializedAreaListViewSource != null)
            {
                this.InitializedAreaListViewSource();
            }
        }

        internal void RaiseInitializedDISourceListViewEvent()
        {
            if (this.InitializedDISourceListViewSource != null)
            {
                this.InitializedDISourceListViewSource();
            }
        }

        internal void RaiseInitializedTimperiodListViewEvent()
        {
            if (this.InitializedTimeperiodListViewSource != null)
            {
                this.InitializedTimeperiodListViewSource();
            }
        }

        internal void RaiseInitializedUnitListViewEvent()
        {
            if (this.InitializedUnitListViewSource != null)
            {
                this.InitializedUnitListViewSource();
            }
        }

        internal void RaiseInitializedSubgroupValListViewEvent()
        {
            if (this.InitializedSubgroupValListViewSource != null)
            {
                this.InitializedSubgroupValListViewSource();
            }
        }

        #endregion

        internal abstract void SetColumnInfo();

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        protected abstract string GetAllRecordsSqlQuery(string searchString);

        protected abstract string GetRecordsByNIDsSqlQuery(string availableNids);

        protected abstract string GetAutoSelectRecordsSqlQuery();

        #endregion

        #endregion

    }


}
