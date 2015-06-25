using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class SubgroupsDimensionValuesSource : TemplateMergingControlSource
    {

     
       #region "-- private/protected --"
        
        #region "-- Variables --"
               
        #endregion
        
        #region "-- Methods --"

        protected override void InitializeVaribles()
        {
            this.KeyColumnName = Subgroup.SubgroupNId;
            this._GlobalColumnName = Subgroup.SubgroupGlobal;

            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();

            this._DisplayColumnsInfo.Add(Subgroup.SubgroupName, DILanguage.GetLanguageString("SUBGROUP_DIMENSION"));

        }

            
        #endregion

        #endregion

        #region "-- Public / Friend --"

     
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public SubgroupsDimensionValuesSource()
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
                SqlQuery = this.ImportQueries.GetAvailableSubgroupDimValues();
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
                string SqlQuery = this.ImportQueries.GetUnmatchedSubgroupDimValues();
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
            DI6SubgroupBuilder SGBuilderObj = null;
            DI6SubgroupInfo SGInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;
            DI6SubgroupBuilder SrcSGBuilder=null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportSubgroupDimensionValues(selectedNids));

            //-- Step 2:Initialise DI6SubgroupBuilder Builder with Target DBConnection
            SGBuilderObj = new DI6SubgroupBuilder(this.TargetDBConnection, this.TargetDBQueries);
            
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


                    // get subgroup info
                    SrcSGBuilder = new DI6SubgroupBuilder(SourceDBConnection, SourceDBQueries);
                    SGInfoObj= SrcSGBuilder.GetSubgroupInfo(FilterFieldType.NId,Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]));
                    

                    SGBuilderObj.ImportSubgroup(SGInfoObj, Convert.ToInt32(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]), SourceDBQueries, SourceDBConnection);

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

            //if (this.UnmatchedTable.Select(Subgroup.SubgroupGId  + "='" + row[Subgroup.SubgroupGId] + "'").Length > 0)

            if ((this.UnmatchedTable.Select(Subgroup.SubgroupGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Subgroup.SubgroupGId] + "'").Length > 0) && (this.UnmatchedTable.Select(Subgroup.SubgroupGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Subgroup.SubgroupGId] + "'").Length > 0))
            {
                RetVal = true;
            }
            return RetVal;

        }

        #endregion

        #endregion
    }
}
