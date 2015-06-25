using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Genearate IUS Sheet For Comparison Report
    /// </summary>
    internal class IUSSheetSource: SheetSource 
    {
        #region "-- Private --"

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.IUS);
            this.NameColumnIndex = Constants.Sheet.IUS.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.IUS.LastColIndex;
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.INDICATOR);
            table.Columns[Unit.UnitName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.UNIT);
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.SUBGROUP);
        }

        private void InitializeIUSTables()
        {
            try
            {
                this.DBConnection.ExecuteNonQuery("DROP TABLE " + Constants.TempTables.TempIUSTable);
            }
            catch{}

            // -- Create and Update _IUS Table
            this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.CreateTempIUSTable());
            this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.InsertIntoIUSTable());
            this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.UpdateIUSTable());
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region " -- New --"
        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal IUSSheetSource()
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
            this.InitializeIUSTables();
            Query = ComparisonReportQuery.SelectMissingIUS();

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

            Query =ComparisonReportQuery.SelectAdditionalIUS();
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
