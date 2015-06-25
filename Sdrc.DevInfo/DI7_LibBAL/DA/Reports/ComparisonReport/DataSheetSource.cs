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
    /// Generate Data Sheet For Comparison Report
    /// </summary>
    internal class DataSheetSource: SheetSource
    {

        #region "-- Private --"

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.DATA);
            this.NameColumnIndex = Constants.Sheet.Data.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Data.LastColIndex;
        }

        private void IntializeDataTable()
        {
                // -- Step 1: create table TargetDataWithGIDS with two new columns : NewICNid and Mapped
                this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.CreateTargetDataWithGIDSTable());

                // -- Step 2: create table ReferenceData_withGIDs with one new column named as Mapped.
                this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.CreateReferenceDataWithGIDTable());

                ////////// -- Step 3: update NewICNid in TargetDataWithGIDs table .
                ////////this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.UpdateTargetDataWithGIDsTable());
                
            // -- Step 4: Update Reference and target tables set mapped to true where record exists in both table
                this.DBConnection.ExecuteNonQuery(ComparisonReportQuery.UpdateRefTargetTable());
                            
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.INDICATOR);
            table.Columns[Unit.UnitName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.UNIT);
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.SUBGROUP);
            table.Columns[Timeperiods.TimePeriod ].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.TIMEPERIOD);
            table.Columns[Area.AreaID].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.AREAID);
            table.Columns[Area.AreaName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.AREANAME);
            table.Columns[Data.DataValue].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.DATAVALUE);
            table.Columns[IndicatorClassifications.ICName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.SOURCE);
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- New --"
        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal DataSheetSource()
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

            this.IntializeDataTable();
            Query = ComparisonReportQuery.GetMissingData();

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

            Query = ComparisonReportQuery.GetAdditionalData();
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
