using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Controls.ListViewBAL
{
    /// <summary>
    /// Helps in getting timeperiods for ListViewControl
    /// </summary>
    public class TimeperiodListViewSource : BaseListViewSource
    {
        #region "-- Protected --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        protected override string GetAllRecordsSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString = Timeperiods.TimePeriod + " like '%" + searchString + "%' ";
                RetVal = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.Search, FilterString);
            }
            else
            {
                RetVal = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty);

                if (!RetVal.ToUpper().Contains("ORDER BY"))
                {
                    RetVal += " ORDER BY " + Timeperiods.TimePeriod + " DESC ";
                }
            }

            return RetVal;
        }



        protected override string GetRecordsByNIDsSqlQuery(string availableNids)
        {
            string RetVal = string.Empty;

            if (!string.IsNullOrEmpty(availableNids))
            {
                RetVal = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, availableNids, Timeperiods.TimePeriod +" DESC ");
            }
            else
            {
                RetVal = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty, Timeperiods.TimePeriod + " DESC ");
            }

            return RetVal;
        }

        protected override string GetAutoSelectRecordsSqlQuery()
        {
            string RetVal = string.Empty;
           

            RetVal = this.DBQueries.Timeperiod.GetAutoSelectTimeperiod(this.DIUserPreference.UserSelection.IndicatorNIds,this.DIUserPreference.UserSelection.ShowIUS,
                 this.DIUserPreference.UserSelection.AreaNIds, this.DIUserPreference.UserSelection.SourceNIds);

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
            this._Columns.Add(new ColumnInfo(Timeperiods.TimePeriod, DILanguage.GetLanguageString("TIMEPERIOD"), string.Empty));

            //set tag value column
            this._TagValueColumnName = Timeperiods.TimePeriodNId;
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
            return this.DIUserPreference.UserSelection.TimePeriodNIds;
        }

        #endregion

        #endregion
    }
}
