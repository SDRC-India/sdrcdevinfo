using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;

namespace DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL
{
    /// <summary>
    ///  Helps in getting Timeperiod records for DISelectionControl
    /// </summary>
    public class TimeperiodSelectionSource : BaseSelection
    {

        #region "-- Protected --"

        #region "-- Method --"

        internal protected override DataView processDataView(DataView dv)
        {
            DataView RetVal;
            RetVal = this.SetStartDateEndDate(dv.Table).DefaultView;
            return dv;
        }

        protected override void SetColumnHeaders()
        {
            this._TagValueColumnName = Timeperiods.TimePeriodNId;
            this._FirstColumnName = Timeperiods.TimePeriod;
            this._SecondColumnName = Data.StartDate;
            this._ThirdColumnName = Data.EndDate;

        }

        protected override void SetColumnNames()
        {
            //Get header string from language file and set column headers string 
            this._FirstColumnHeader = DILanguage.GetLanguageString("TIMEPERIOD");
            this._SecondColumnHeader = DILanguage.GetLanguageString("STARTDATE");
            this._ThirdColumnHeader = DILanguage.GetLanguageString("ENDDATE");

        }

        /// <summary>
        /// Get Qquey for Timeperiod for Given Timeperiod_NID
        /// </summary>
        /// <param name="nids">Timeperiod_NIDs</param>
        /// <returns>Query String</returns>
        protected override string GetRecordsForSelectedNids(string nids)
        {
            string RetVal = string.Empty;
            RetVal = this.SqlQueries.Timeperiod.GetTimePeriod(nids);

            //dont implement and delete this method
            return RetVal;
        }

        /// <summary>
        /// Get SQL Query For All Timeperiods
        /// </summary>
        /// <returns></returns>
        protected override string GetSelectAllQuery()
        {

            string RetVal = string.Empty;

            RetVal = this.SqlQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty);

            return RetVal;
        }


        protected override string GetAssocicatedRecordsQuery(int selectedNid, int selectedParentNid)
        {
            // Dont implement and delete this method
            return string.Empty;
        }

        public override void UpdataDataTableBeforeCreatingListviewItem(DataTable iuTable)
        {
            //-- Do Nothing
        }

        public override List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon)
        {
            return new List<string>();
        }

        protected override string GetAssocicatedRecordsQuery(string selectedNids)
        {
            //Do nothing
            return string.Empty;
        }

        #endregion

        #endregion

        #region "-- Public --"

        /// <summary>
        /// N/A
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsForTreeAutoSelect()
        {
            //TODO dont implement and delete this method
            return null;
        }

        /// <summary>
        /// Get Query For AutoSelect Timeperiod
        /// </summary>
        /// <param name="availableItemsNid"></param>
        /// <returns></returns>
        public override DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid)
        {
            DataTable RetVal = null;

            try
            {
                RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.Timeperiod.GetAutoSelectTimeperiod(this.UserPrefences.UserSelection.IndicatorNIds, this.UserPrefences.UserSelection.ShowIUS, this.UserPrefences.UserSelection.AreaNIds, this.UserPrefences.UserSelection.SourceNIds));
                RetVal = this.SetStartDateEndDate(RetVal);
            }
            catch { }

            return RetVal;
        }

        /// <summary>
        /// Returns all records 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsTable()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this._DBConnection.ExecuteDataTable(this.GetSelectAllQuery());
                RetVal = this.SetStartDateEndDate(RetVal);

            }
            catch (Exception)
            {
                // do nothing
            }
            return RetVal;
        }

        public override List<string> GetSubgroupDimensions(string iuNId, string IUSNIds)
        {
            return new List<string>();
        }

        public override List<string> GetSubgroupDimensionsWithIU(string iuNId, string IUSNIds)
        {
            return new List<string>();
        }

        /// <summary>
        /// Set StartDate and EndDate columns to datatable and update their values
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private DataTable SetStartDateEndDate(DataTable dataTable)
        {
            DataTable RetVal = dataTable;
            DateTime StartDate;
            DateTime EndDate;
            try
            {
                if (dataTable != null)
                {
                    dataTable.Columns.Add(Data.StartDate, typeof(string));
                    dataTable.Columns.Add(Data.EndDate, typeof(string));
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        StartDate = DateTime.MinValue;
                        EndDate = DateTime.MinValue;
                        TimePeriodFacade.SetStartDateEndDate(dr[Timeperiods.TimePeriod].ToString(), ref StartDate, ref EndDate);

                        if (StartDate.CompareTo(DateTime.MinValue) == 0)
                        {
                            dr[Data.StartDate] = string.Empty;
                        }
                        else
                        {
                            dr[Data.StartDate] = StartDate.ToString("yyyy.MM.dd");
                        }

                        if (EndDate.CompareTo(DateTime.MinValue) == 0)
                        {
                            dr[Data.EndDate] = string.Empty;
                        }
                        else
                        {
                            dr[Data.EndDate] = EndDate.ToString("yyyy.MM.dd");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            return RetVal;
        }

        public override void UpdateIndicatorSelectedDetails(int indicatorNId, int unitNId, string selectionDetails, bool addNewSelection)
        {
            //Do nothing
        }

        public override string GetIndicatorSelectionDetails(int indicatorNId, int unitNId)
        {
            return string.Empty;
        }


        #endregion

    }
}
