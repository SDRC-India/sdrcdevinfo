using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Genearate Unit Sheet for Comparison Report
    /// </summary>
    internal class UnitSheetSource: SheetSource
    {

        #region "-- Private --"

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.UNIT);
            this.NameColumnIndex = Constants.Sheet.Unit.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Unit.NameColIndex;
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[Unit.UnitName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.UNIT);
        }

        #endregion

        #endregion

        #region "-- Internal --"

        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal UnitSheetSource()
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

            Query = ComparisonReportQuery.GetMissingRecords(this.DBQueries.TablesName.Unit, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Unit, Unit.UnitName, Unit.UnitGId);

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

            Query = ComparisonReportQuery.GetAdditionalRecords(this.DBQueries.TablesName.Unit, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Unit, Unit.UnitName, Unit.UnitGId);
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
