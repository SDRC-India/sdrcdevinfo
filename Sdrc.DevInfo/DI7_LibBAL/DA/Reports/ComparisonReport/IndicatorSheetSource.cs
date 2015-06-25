using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{

    /// <summary>
    /// Class Used To Create Indicator Sheet.
    /// </summary>
    internal class IndicatorSheetSource: SheetSource 
    {
        #region "-- Private --"
       
        #region "-- Method --"

        protected override void InitializeSheetVariables()
        {
            this.SheetName = DILanguage.GetLanguageString(Constants.SheetHeader.INDICATOR);
            this.NameColumnIndex = Constants.Sheet.Indicator.NameColIndex;
            this.LastColumnIndex = Constants.Sheet.Indicator.NameColIndex;
        }


        /// <summary>
        /// Rename Indicator DataTable Column for Required Column Heading
        /// </summary>
        /// <param name="IndicatorTable">Indicator Table</param>
        protected override void  RenameColumns(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = DILanguage.GetLanguageString(Constants.SheetHeader.INDICATOR);
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- New --"

        /// <summary>
        /// Constructor to Initilize Sheet Name
        /// </summary>
        internal IndicatorSheetSource()
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
            
            Query = ComparisonReportQuery.GetMissingRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator , Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Indicator, Indicator.IndicatorName, Indicator.IndicatorGId);
            
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

            Query = ComparisonReportQuery.GetAdditionalRecords(DataBaseComparisonReportGenerator.DBQueries.TablesName.Indicator, Constants.TempTables.TempTablePrefix + DataBaseComparisonReportGenerator.TargetDBQueries.TablesName.Indicator, Indicator.IndicatorName, Indicator.IndicatorGId);

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
