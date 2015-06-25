using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Generate Timeperiod Sheet for Comparison Report
    /// </summary>
    internal class TimeperiodSheetSource : SheetSource
    {

        #region "-- Private --"

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.TIMEPERIOD);
            this.NameColumnIndex = Constants.Sheet.Timeperiods.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Timeperiods.NameColIndex;
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[Timeperiods.TimePeriod].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.TIMEPERIOD);
        }

        #endregion

        #endregion

        #region "-- Internal --"
        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal TimeperiodSheetSource()
        {
            this.InitializeSheetVariables();
        }

        /// <summary>
        /// Get Missing Records Tables
        /// </summary>
        /// <returns></returns>
        internal override DataTable GetMissingRecordsTable()
        {
            string Query = string.Empty;
            DataTable RetVal = null;

           Query = ComparisonReportQuery.GetMissingTimeperiod();

            // -- Get DataTable From Query
            if (!string.IsNullOrEmpty(Query))
            {
                RetVal = this.DBConnection.ExecuteDataTable(Query);
                // -- Set S No. into Table 
                RetVal = this.GetTableWithSno(RetVal);

                this.RenameColumns(ref RetVal);
            }
            return RetVal;

        }

        /// <summary>
        /// Get Additional Records Tables
        /// </summary>
        /// <returns></returns>
        internal override DataTable GetAdditionalRecordsTable()
        {
            string Query = string.Empty;
            DataTable RetVal = null;

            Query = ComparisonReportQuery.GetAdditionalTimeperiod();
            // -- Get DataTable From Query
            if (!string.IsNullOrEmpty(Query))
            {
                RetVal = this.DBConnection.ExecuteDataTable(Query);
                // -- Set S No. into Table 
                RetVal = this.GetTableWithSno(RetVal);

                this.RenameColumns(ref RetVal);
            }
            return RetVal;
        }

        #endregion
              
    }
}
