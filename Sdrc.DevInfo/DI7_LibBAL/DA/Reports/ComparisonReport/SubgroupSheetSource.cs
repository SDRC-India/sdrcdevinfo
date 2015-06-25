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
    /// Generate Subgroup Sheet
    /// </summary>
    internal class SubgroupSheetSource : SheetSource
    {

        #region "-- Private --"

        #region "-- Method --"

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.SUBGROUP);
        }

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.SUBGROUP);
            this.NameColumnIndex = Constants.Sheet.Subgroup.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Subgroup.NameColIndex;   
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region " -- New --"
        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal SubgroupSheetSource()
        {
            this.InitializeSheetVariables();
            
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

            Query = ComparisonReportQuery.GetMissingRecords(this.DBQueries.TablesName.SubgroupVals, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.SubgroupVals, SubgroupVals.SubgroupVal, SubgroupVals.SubgroupValGId);

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

            Query = ComparisonReportQuery.GetAdditionalRecords(this.DBQueries.TablesName.SubgroupVals, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.SubgroupVals, SubgroupVals.SubgroupVal, SubgroupVals.SubgroupValGId);
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
