using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;


namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting timeperiod for LookInWindow control
    /// </summary>
    public class TimeperiodLookInSource:BaseLookInSource
    {
        #region"--Protected--"

        #region"--Method--"

      
        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString = Timeperiods.TimePeriod + " like '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.Timeperiod.GetTimePeriod(FilterFieldType.Search, FilterString);
            }
            else
            {
                RetVal = this.SourceDBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty);
            }


            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //Dont implement this
        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> SelectedNids, bool allSelected)
        {
            int ProgressBarValue = 0;
            DataRow[] Rows;
            TimeperiodBuilder DBTimperiodBuilder = new TimeperiodBuilder(this._TargetDBConnection, this._TargetDBQueries);
            

            foreach (string Nid in SelectedNids)
            {
                //get timeperiod
                Rows=this.SourceTable.Select(this.TagValueColumnName + "=" + Nid);
                //import timeperiod
               DBTimperiodBuilder.CheckNCreateTimeperiod(Rows[0][Timeperiods.TimePeriod].ToString());

               this.RaiseIncrementProgessBarEvent(ProgressBarValue);
               ProgressBarValue++;
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();
            this.Columns.Add(Timeperiods.TimePeriod, DevInfo.Lib.DI_LibBAL.Utility.DILanguage.GetLanguageString(Constants.LanguageKeys.Timeperiod));
            this.TagValueColumnName = Timeperiods.TimePeriodNId;
            this.GlobalValueColumnName1 = string.Empty;
        }

        #endregion

        #endregion

        #endregion
    }
}