using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    internal class AreaSheetSource: SheetSource
    {
        #region "-- Private --"

        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.AREA);
            this.NameColumnIndex = Constants.Sheet.Area.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Area.NameColIndex;
        }

        protected override void RenameColumns(ref DataTable table)
        {
            table.Columns[Area.AreaName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.AREA);
        }    
       
        #endregion

        #endregion

        #region "-- Internal --"
        
        #region "-- New --"

        /// <summary>
        /// Constructor to Initialize Sheet Variables
        /// </summary>
        internal AreaSheetSource()
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

            Query = ComparisonReportQuery.GetMissingRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.Area, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Area, Area.AreaName, Area.AreaID);

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

            Query = ComparisonReportQuery.GetAdditionalRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.Area, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Area, Area.AreaName, Area.AreaID);

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
