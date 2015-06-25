using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class IndicatorSource: TemplateMergingControlSource
    {


        #region "-- private/protected --"
        
        #region "-- Variables --"
               
        #endregion
        
        #region "-- Methods --"
        
        protected override void InitializeVaribles()
        {
            this.KeyColumnName = Indicator.IndicatorNId;
            this._GlobalColumnName = Indicator.IndicatorGlobal;
            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();
            this._DisplayColumnsInfo.Add(Indicator.IndicatorName, DILanguage.GetLanguageString("INDICATORNAME"));
        }

       

        #endregion

        #endregion

        #region "-- Public / Friend --"
             
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public IndicatorSource()
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
                SqlQuery = this.ImportQueries.GetAvailableIndicator();
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
                string SqlQuery = this.ImportQueries.GetUnmatchedIndicator();
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            { throw ex; }

            return RetVal;
        }

        #endregion
                
        #region "-- Mapped Tables --"

        //public override MappedTableInfo InitializeMappedTableInfo()
        //{
        //    MappedTableInfo RetVal;
        //    try
        //    {
        //        RetVal = new MappedTableInfo();

        //        RetVal.MappedRows = new Dictionary<string, MappedRowInfo>();
                
        //        //-- Set Columns
        //        RetVal.TableColumnsInfo = new List<string>();
        //        RetVal.TableColumnsInfo.Add(this.KeyColumnName);

        //        foreach (DataColumn UCol in this.UnmatchedTable.Columns)
        //        {
        //            RetVal.TableColumnsInfo.Add(Constants.Columns.UNMATCHED_COL_Prefix + UCol);
        //        }

        //        foreach (DataColumn ACol in this.AvailableTable.Columns)
        //        {
        //            RetVal.TableColumnsInfo.Add(Constants.Columns.AVAILABLE_COL_Prefix + ACol);
        //        }

        //    }
        //    catch (Exception ex) { throw ex; }
            
        //    return RetVal;
        //}

        //public override void MapRecord(string unmatchedNId, string availableNId)
        //{
        //    MappedRowInfo RowInfo = null;

        //    try
        //    {
        //        //-- Initialize  MappedTable if not Initialized
        //        if (this._MappedTable.MappedRows == null)
        //            this._MappedTable.MappedRows = new Dictionary<string, MappedRowInfo>();

        //        //-- Check Key value Exist or Not
        //        if (!this._MappedTable.MappedRows.ContainsKey(availableNId))
        //        {
        //            RowInfo = new MappedRowInfo();

        //            //-- Get Unmatched and Available Row
        //            RowInfo.AvailableRow = this.AvailableTable.Copy().Select(this.KeyColumnName + "=" + availableNId)[0];
        //            RowInfo.UnmatchedRow = this.UnmatchedTable.Copy().Select(this.KeyColumnName + "=" + unmatchedNId)[0];
        //            RowInfo.KeyValue = availableNId;

        //            this._MappedTable.MappedRows.Add(availableNId, RowInfo);
        //            this.DeleteRecord(unmatchedNId, availableNId);
        //        }
        //    }
        //    catch (Exception ex) { throw ex; }


        //}

        //public override void RemoveMapRecord(string keyNId)
        //{
        //    string UnmatchedNid = string.Empty;

        //    if (this._MappedTable.MappedRows.ContainsKey(keyNId))
        //    {
        //        this._MappedTable.MappedRows.Remove(keyNId);
        //    }
        //    this._UnmatchedTable = this.GetUnmatchedTable();
        //    this._AvailableTable = this.GetAvailableTable();

        //    foreach (string MapKey in this._MappedTable.MappedRows.Keys)
        //    {
        //        //-- Get Unmatched Nids
        //        UnmatchedNid = Convert.ToString(this._MappedTable.MappedRows[MapKey].UnmatchedRow[Constants.Columns.UNMATCHED_COL_Prefix + this._KeyColumnName]);
        //        this.DeleteRecord(UnmatchedNid, this._MappedTable.MappedRows[MapKey].KeyValue);
        //    }
        //}
        
        #endregion

        #region "-- Import --"

        protected override bool IsElementAlreadyExists(DataRow unmatchedRow, DataRow availableRow)
        {
            bool RetVal = false;

            //do nothing
            return RetVal;
        }

        public override void Import(string selectedGIds)
        {
            int ProgressCounter = 0;
            DataTable Table = null;
            string selectedNids = string.Empty;
            selectedNids = selectedGIds;

            // Initialize progress bar
            this.RaiseProgressBarInitialize( selectedGIds.Split(',').GetUpperBound(0) + 1);
            
            IndicatorBuilder TrgIndicatorBuilderObj = null;
            IndicatorBuilder SourceIndicatorBuilder = null;
            IndicatorInfo IndicatorInfoObj = null; 
            Dictionary<string, DataRow> FileWithNids=new Dictionary<string,DataRow>();

            DIConnection SourceDBConnection = null ;
            DIQueries SourceDBQueries = null ;

            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportIndicator(selectedNids));
            
            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            TrgIndicatorBuilderObj = new IndicatorBuilder(this.TargetDBConnection, this.TargetDBQueries);

            //-- Step 3: Import Nids for each SourceFile
            foreach (DataRow  Row in Table.Copy().Rows )
            {
                try
                {
                    
                    string SourceFileWPath = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SourceFileWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // Get Source Indicator Info
                    SourceIndicatorBuilder = new IndicatorBuilder(SourceDBConnection, SourceDBQueries);
                    IndicatorInfoObj = SourceIndicatorBuilder.GetIndicatorInfo(FilterFieldType.NId,Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]),FieldSelection.Light);

                    // Import INdicator By Nid
                    TrgIndicatorBuilderObj.ImportIndicator(IndicatorInfoObj, Convert.ToInt32(Row[MergetTemplateConstants.Columns.COLUMN_SRCNID]), SourceDBQueries, SourceDBConnection);
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

        public override bool ISMappedValueExist(DataRow row)
        {
            bool RetVal = false;

           // if (this.UnmatchedTable.Select(Indicator.IndicatorGId + "='" + row[Indicator.IndicatorGId] + "'").Length > 0)
            if ((this.UnmatchedTable.Select(Indicator.IndicatorGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Indicator.IndicatorGId] + "'").Length > 0) && (this.UnmatchedTable.Select(Indicator.IndicatorGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Indicator.IndicatorGId] + "'").Length > 0))  
            RetVal = true;

            return RetVal;

        }
        #endregion

        #endregion

    }
}
