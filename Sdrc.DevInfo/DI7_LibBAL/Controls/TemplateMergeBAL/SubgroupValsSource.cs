using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class SubgroupValsSource : TemplateMergingControlSource
    {
      
        #region "-- private/protected --"
        
        #region "-- Variables --"
               
        #endregion
        
        #region "-- Methods --"

        protected override void InitializeVaribles()
        {
            this.KeyColumnName = SubgroupVals.SubgroupValNId ;
            this._GlobalColumnName = SubgroupVals.SubgroupValGlobal;

            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();
            this._DisplayColumnsInfo.Add(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString("SUBGROUP"));
        }

      

        #endregion

        #endregion

        #region "-- Public / Friend --"

     
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public SubgroupValsSource()
        {
            this.InitializeVaribles();
        } 

        #endregion

        #region "-- Methods --"

        #region "-- Available/Unmatched --"

        public override DataTable GetAvailableTable()
        {
            DataTable RetVal = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.ImportQueries.GetAvailableSubgroupVals();
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            { throw ex; }
            return RetVal;
        }

        public override DataTable GetUnmatchedTable()
        {
            DataTable RetVal = null;
            try
            {
                string SqlQuery = this.ImportQueries.GetUnmatchedSubgroupVals();
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            { throw ex; }

            return RetVal;
        }

        #endregion
                
       
        #region "-- Import --"

        public override void Import(string selectedNids)
        {
            DataTable Table = null;
            int ProgressCounter = 0;
            DI6SubgroupValBuilder SGBuilderObj = null;
            DI6SubgroupValInfo SGInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;
            DI6SubgroupValBuilder SourceSGValBuilder = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportSubgroupVals(selectedNids));

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            SGBuilderObj = new DI6SubgroupValBuilder(this.TargetDBConnection, this.TargetDBQueries);
          
            // Initialize progress bar
            this.RaiseProgressBarInitialize(selectedNids.Split(',').GetUpperBound(0) + 1);


            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow Row in Table.Copy().Rows)
            {
                try
                {
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // get subgroupval info from source file
                    SourceSGValBuilder = new DI6SubgroupValBuilder(SourceDBConnection, SourceDBQueries);
                 SGInfoObj =SourceSGValBuilder.GetSubgroupValInfo(FilterFieldType.NId,   Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]));

                    

                    SGBuilderObj.ImportSubgroupVal(Convert.ToInt32(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]), SourceDBQueries, SourceDBConnection);
                    ProgressCounter += 1;
                    this.RaiseProgressBarIncrement(ProgressCounter);

                }
                catch (Exception ex) { ExceptionFacade.ThrowException(ex); }
                finally
                {
                    if (SourceDBConnection != null)
                        SourceDBConnection.Dispose();
                    if (SourceDBQueries != null)
                        SourceDBQueries.Dispose();
                }
            }
            this._AvailableTable = this.GetAvailableTable();
            this._UnmatchedTable = this.GetUnmatchedTable();
            // Close ProgressBar
            this.RaiseProgressBarClose();
        }

        #endregion


        protected override bool IsElementAlreadyExists(DataRow unmatchedRow, DataRow availableRow)
        {
            bool RetVal = false;

            //do nothing
            return RetVal;
        }

        public override bool ISMappedValueExist(DataRow row)
        {
            bool RetVal = false;

            if ((this.UnmatchedTable.Select(SubgroupVals.SubgroupValGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + SubgroupVals.SubgroupValGId] + "'").Length > 0) && (this.UnmatchedTable.Select(SubgroupVals.SubgroupValGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + SubgroupVals.SubgroupValGId] + "'").Length > 0))
                RetVal = true;


            return RetVal;

        }

        #endregion

        #endregion
    }
}
