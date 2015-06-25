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
    /// Helps in getting Unit for ListViewControl
    /// </summary>
    public class SubgroupValListViewSource : BaseListViewSource
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
                FilterString = " '%" + searchString + "%' ";
                RetVal = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.Search, FilterString);
            }
            else
            {
                RetVal = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None, string.Empty);
            }

            RetVal += " ORDER BY " + SubgroupVals.SubgroupVal;
            return RetVal;
        }

        protected override string GetRecordsByNIDsSqlQuery(string availableNids)
        {
            string RetVal = string.Empty;


            if (!string.IsNullOrEmpty(availableNids))
            {
                RetVal = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.NId, availableNids);
            }
            else
            {
                RetVal = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None, string.Empty);
            }

            return RetVal;
        }

        protected override string GetAutoSelectRecordsSqlQuery()
        {
            string RetVal = string.Empty;

            RetVal = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None, string.Empty);

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
            this._Columns.Add(new ColumnInfo(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString("SUBGROUP"), SubgroupVals.SubgroupValGlobal));

            //set tag value column
            this._TagValueColumnName = SubgroupVals.SubgroupValNId;
        }

        #endregion

        #endregion

        #region "- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Retruns subgroupVals Nid from userpreference
        /// </summary>
        /// <returns></returns>
        public override string GetSelectedNids()
        {
            return this.DIUserPreference.UserSelection.SubgroupValNIds;
        }

        #endregion

        #endregion
    }
}
