using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.Area;

namespace DevInfo.Lib.DI_LibBAL.Controls.ListViewBAL
{
    /// <summary>
    /// Helps in getting areas for ListViewControl
    /// </summary>
    public class AreaListViewSource : BaseListViewSource
    {

        #region "-- Protected --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        protected override string GetAllRecordsSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            StringBuilder FilterText = new StringBuilder();

            //create filter text
            FilterText.Append(" 1=1 ");

            if (this._AreaLevel > 0)
            {
                FilterText.Append(" And " + Area.AreaLevel + "=" + this._AreaLevel);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                FilterText.Append(" And  " + Area.AreaName + " LIKE '%" + searchString + "%'");
            }

            //get query 
            RetVal = this.DBQueries.Area.GetArea(FilterFieldType.Search, FilterText.ToString(), Select.OrderBy.AreaName);

            return RetVal;
        }

        protected override string GetRecordsByNIDsSqlQuery(string availableNids)
        {
            string RetVal = string.Empty;

            //get query 
            RetVal = this.DBQueries.Area.GetArea(FilterFieldType.NId, availableNids, Select.OrderBy.AreaName);
            return RetVal;
        }

        protected override string GetAutoSelectRecordsSqlQuery()
        {
            string RetVal = string.Empty;
            

            //get query 
            RetVal = this.DBQueries.Area.GetAutoSelectAreas(this.DIUserPreference.UserSelection.IndicatorNIds,this.DIUserPreference.UserSelection.ShowIUS, this.DIUserPreference.UserSelection.TimePeriodNIds, this.DIUserPreference.UserSelection.SourceNIds, this._AreaLevel);

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Intenal --"

        #region "-- Methods --"

        internal override void SetColumnInfo()
        {
            this._Columns.Clear();

            //set columns info
            this._Columns.Add(new ColumnInfo(Area.AreaName, DILanguage.GetLanguageString("AREA"), Area.AreaGlobal));

            //set tag value column
            this._TagValueColumnName = Area.AreaNId;
        }

        #endregion

        #endregion

        #region "- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Retruns IndicatorNids from userpreference
        /// </summary>
        /// <returns></returns>
        public override string GetSelectedNids()
        {
            return this.DIUserPreference.UserSelection.AreaNIds;
        }

        #endregion

        #endregion
    }
}
