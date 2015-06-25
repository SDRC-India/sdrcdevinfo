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
    /// Helps in getting indicators for ListViewControl
    /// </summary>
    public class IndicatorListViewSource : BaseListViewSource
    {
        #region "-- Private -- "

        #region "-- Methods --"
                
        private string GetIUSNIdsFrmDataTable()
        {
            string RetVal = string.Empty;
            string SqlQuery = string.Empty;
            DataTable Table;
            int SourceNids = 0;
            try
            {
                //get all IUSNID from data table
                SqlQuery=this.DBQueries.IUS.GetDistinctIUSNIdFrmDataTable();
                
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

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        protected override string GetAllRecordsSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;
            string NIds = string.Empty;

            //set filter string
            if (!string.IsNullOrEmpty(searchString))
            {
                FilterString = " " + Indicator.IndicatorName + " LIKE '%" + searchString + "%' ";
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

            RetVal += " ORDER BY " + Indicator.IndicatorName;
            return RetVal;
        }


        protected override string GetRecordsByNIDsSqlQuery(string availableNids)
        {
            string RetVal = string.Empty;
            
            if (!string.IsNullOrEmpty(availableNids))
            {
                RetVal = this.DBQueries.Indicators.GetIndicator(FilterFieldType.NId, availableNids, FieldSelection.Light);
            }
            else
            {
                RetVal = this.DBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light);
            }

            RetVal += " ORDER BY " + Indicator.IndicatorName;
            return RetVal;
        }




        protected override string GetAutoSelectRecordsSqlQuery()
        {
            string RetVal = string.Empty;
            string Query=string.Empty;
            string IndicatorNIds=string.Empty;

            DataTable TempIndicatorTable = null;

            //get indicator  where datatable.area=given_area and datatable.timperiod=given_timeperiod and datatable.source = given_source;
            Query = this.DBQueries.Indicators.GetAutoSelectIndicatorByTimePeriodAreaSource(this.DIUserPreference.UserSelection.TimePeriodNIds,this.DIUserPreference.UserSelection.AreaNIds,
                    this.DIUserPreference.UserSelection.SourceNIds);
            TempIndicatorTable= this.DBConnection.ExecuteDataTable(Query);
            

            // get comma separated values from indicator table
            IndicatorNIds= DevInfo.Lib.DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromDataTable(TempIndicatorTable, Data.IndicatorNId);

// get indicators
            if (string.IsNullOrEmpty(IndicatorNIds))
            {
                IndicatorNIds = "0";
            }

            RetVal = this.DBQueries.Indicators.GetIndicator(FilterFieldType.NId,  IndicatorNIds, FieldSelection.Light );
            RetVal += " ORDER BY " + Indicator.IndicatorName;

                //RetVal = this.DBQueries.Indicators.GetAutoSelectIndicator(
                //    this.DIUserPreference.UserSelection.TimePeriodNIds,
                //    this.DIUserPreference.UserSelection.AreaNIds,
                //    this.DIUserPreference.UserSelection.SourceNIds, FieldSelection.Light);

           
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
            this._Columns.Add(new ColumnInfo(Indicator.IndicatorName, DILanguage.GetLanguageString("INDICATOR"), Indicator.IndicatorGlobal));

            //set tag value column
            this._TagValueColumnName = Indicator.IndicatorNId;
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
            string RetVal = string.Empty;

            if (this.DIUserPreference.UserSelection.ShowIUS)
            {
                RetVal = this.DIUserPreference.UserSelection.IndicatorNIds;
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
