using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{

    /// <summary>
    /// Generate Sheet For Indicator Classifications
    /// </summary>
    internal class ICSheetSource :  SheetSource
    {

        #region "-- Private --"

        #region "-- Variables --"

        private ICType EICType;

        #endregion

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.NameColumnIndex = Constants.Sheet.IC.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.IC.NameColIndex;
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[IndicatorClassifications.ICName].ColumnName = this.SheetName;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region " -- New --"

        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal ICSheetSource(ICType icType, string sheetName)
        {
            this.InitializeSheetVariables();
            this.EICType = icType;
            this.SheetName = sheetName;
        }

        #endregion
        /// <summary>
        /// Get Missing Records Tables
        /// </summary>
        /// <returns></returns>
        internal override DataTable GetMissingRecordsTable()
        {
            string Query = string.Empty;
            DataTable RetVal = null;

            Query = ComparisonReportQuery.GetMissingRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.IndicatorClassifications, IndicatorClassifications.ICName, IndicatorClassifications.ICGId, this.EICType);

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

            Query = ComparisonReportQuery.GetAdditionalRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.IndicatorClassifications, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.IndicatorClassifications, IndicatorClassifications.ICName, IndicatorClassifications.ICGId, this.EICType);

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
