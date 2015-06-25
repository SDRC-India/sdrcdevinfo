using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class UnitSource : TemplateMergingControlSource
    {
      

        #region "-- private/protected --"
        
        #region "-- Variables --"
               
        #endregion
        
        #region "-- Methods --"

        protected override void InitializeVaribles()
        {
            this.KeyColumnName = Unit.UnitNId;
            this._GlobalColumnName = Unit.UnitGlobal;

            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();
            this._DisplayColumnsInfo.Add(Unit.UnitName, DILanguage.GetLanguageString("UNIT"));
        }

        #endregion

        #endregion

        #region "-- Public / Friend --"

     
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public UnitSource()
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
                SqlQuery = this.ImportQueries.GetAvailableUnit();
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
                string SqlQuery = this.ImportQueries.GetUnmatchedUnit();
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
            UnitBuilder TrgUnitBuilderObj = null;
            UnitBuilder SourceUnitBuilderObj = null;
            UnitInfo SrcUnitInfoObj = null;
          
            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportUnits(selectedNids));

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            TrgUnitBuilderObj = new UnitBuilder(this.TargetDBConnection, this.TargetDBQueries);

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

                    SourceUnitBuilderObj = new UnitBuilder(SourceDBConnection, SourceDBQueries);
                    SrcUnitInfoObj = SourceUnitBuilderObj.GetUnitInfo(FilterFieldType.NId,Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]));
             
                    // Import Unit from Source
                    TrgUnitBuilderObj.ImportUnit(SrcUnitInfoObj, Convert.ToInt32(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]), SourceDBQueries, SourceDBConnection);
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

            //if (this.UnmatchedTable.Select(Unit.UnitGId + "='" + row[Unit.UnitGId] + "'").Length > 0)
            if ((this.UnmatchedTable.Select(Unit.UnitGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Unit.UnitGId] + "'").Length > 0) && (this.UnmatchedTable.Select(Unit.UnitGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Unit.UnitGId] + "'").Length > 0)) 
            RetVal = true;

            return RetVal;

        }

        #endregion

        #endregion
    }
}
