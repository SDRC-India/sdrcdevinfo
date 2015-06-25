using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class AreasSource:TemplateMergingControlSource 
    {
             
        #region "-- private/protected --"
        
        #region "-- Variables --"

        private int MaxLevel = 0;
        //private bool IsAreaMapListView = false;
        private string SearchString = string.Empty;
 
        #endregion
        
        #region "-- Methods --"

        protected override void InitializeVaribles()
        {
            this.KeyColumnName = Area.AreaNId ;
            this._GlobalColumnName = Area.AreaGlobal;

            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();
            this._DisplayColumnsInfo.Add(Area.AreaName, DILanguage.GetLanguageString("AREANAME"));
            this._DisplayColumnsInfo.Add(Area.AreaLevel, DILanguage.GetLanguageString("AREALEVEL"));
        }

        private string GetColumnName(int level)
        {
            string RetVal = string.Empty;
            RetVal = MergetTemplateConstants.Columns.Level + " " + level;
            return RetVal;
        }

        private void SetLevelName(DataTable table)
        {
            DataRow[] Rows;
            try
            {
                for (int Level = 2; Level <= this.MaxLevel; Level++)
                {
                    foreach (DataRow Row in table.Select(Area.AreaLevel + "=" + Level))
                    {
                        Rows = table.Select(Area.AreaNId + "=" + Row[Area.AreaParentNId].ToString());
                        for (int i = 1; i <= Level - 1; i++)
                        {
                            if (Rows.Length > 0)
                            {
                                Row[MergetTemplateConstants.Columns.Level + " " + i] = Rows[0][MergetTemplateConstants.Columns.Level + " " + i];
                                table.AcceptChanges();
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected  string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;

            //save the searach string and do the filtering of records in processDataTable()
            this.SearchString = searchString;

            RetVal = this.TargetDBQueries.Area.GetArea(FilterFieldType.None, string.Empty);
            return RetVal;
        }

        protected  void ProcessDataTable(ref System.Data.DataTable table)
        {
            DataTable NewTable;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;

            try
            {
                //Step 1: Get maximum level from table 
                table.DefaultView.Sort = Area.AreaLevel + " DESC";
                if (table.Rows.Count > 0)
                {
                    this.MaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][Area.AreaLevel]);

                }
                else
                {
                    this.MaxLevel = 1;
                }

                //Setp 2: Set Columns Info accordingly
                this.SetColumnsInfo();

                //Step 3: Create New Data Table to fill the list view
                NewTable = new DataTable();
                NewTable.Columns.Add(Area.AreaNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Area.AreaParentNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Area.AreaLevel, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Area.AreaID);
                NewTable.Columns.Add(Area.AreaName);
                NewTable.Columns.Add(Area.AreaGlobal);
                try
                {
                    NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMN_UNMATCHED);
                }
                catch { }

                //insert columns for area level info.
                for (Level = 1; Level <= this.MaxLevel; Level++)
                {
                    NewTable.Columns.Add(this.GetColumnName(Level));
                }

                //insert rows into new table
                foreach (DataRow Row in table.Rows)
                {
                    Rows = NewTable.Select(Area.AreaNId + " =" + Row[Area.AreaParentNId].ToString());

                    NewRow = NewTable.NewRow();
                    NewRow[Area.AreaNId] = Row[Area.AreaNId];
                    NewRow[Area.AreaParentNId] = Row[Area.AreaParentNId];
                    NewRow[Area.AreaID] = Row[Area.AreaID];
                    NewRow[Area.AreaName] = Row[Area.AreaName].ToString();
                    NewRow[Area.AreaGlobal] = Row[Area.AreaGlobal];
                    NewRow[Area.AreaLevel] = Row[Area.AreaLevel];

                    NewRow[this.GetColumnName(Convert.ToInt32(NewRow[Area.AreaLevel]))] = Row[Area.AreaName].ToString();

                    try
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMN_UNMATCHED] = Row[MergetTemplateConstants.Columns.COLUMN_UNMATCHED];
                        
                    }
                    catch { }
                    NewTable.Rows.Add(NewRow);

                   
                }

                NewTable.AcceptChanges();
                this.SetLevelName(NewTable);
                NewTable.DefaultView.Sort = Area.AreaID;

                //Step 4: If search string is not empty then filter data table 
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    NewTable.DefaultView.RowFilter = Area.AreaName + " LIKE '%" + this.SearchString + "%'";

                    NewTable = NewTable.DefaultView.ToTable();

                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }

            //Step 5: do the sorting on the basis of available levels
            string SortString = string.Empty;
            for (int i = 1; i <= MaxLevel; i++)
            {
                if (!string.IsNullOrEmpty(SortString))
                {
                    SortString += " ,";
                }
                SortString += MergetTemplateConstants.Columns.Level + " " + i + " ASC ";
            }

            NewTable.DefaultView.Sort = SortString;

            //Step 6: Replace the referenced table with the new data table
            table = NewTable.DefaultView.ToTable();
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public void SetColumnsInfo()
        {
            this.DisplayColumnsInfo.Clear();

            for (int Level = 1; Level <= this.MaxLevel; Level++)
            {
                //key is actual column name and value is display string(header name)
                this.DisplayColumnsInfo.Add(this.GetColumnName(Level), DevInfo.Lib.DI_LibBAL.Utility.DILanguage.GetLanguageString(MergetTemplateConstants.Columns.Level) + " " + Level);
            }

            this.KeyColumnName = Area.AreaNId;
        }

        #endregion

        #endregion

        #region "-- Public / Friend --"

     
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public AreasSource()
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
                SqlQuery = this.TargetDBQueries.Area.GetArea(FilterFieldType.None, string.Empty);
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);
                this.ProcessDataTable(ref RetVal);
            }
            catch (Exception ex)
            { throw ex; }
            return RetVal;
        }
              
        public override DataTable GetUnmatchedTable()
        {
            DataTable RetVal = null;
            DataView DVTable = null;
            string SqlQuery = string.Empty;

            try
            {

                //Step1:Set Unmatched Value to false
                SqlQuery = this.ImportQueries.ClearAllUnmatchedAreas();

                //Step2: Execute query 
                this.TargetDBConnection.ExecuteNonQuery(SqlQuery);
                              
                //Step1:Get Unmatched Query for Area
                SqlQuery = this.ImportQueries.UpdateUnmatchedAreas( this._CurrentTemplateFileNameWPath);

                //Step2: Execute query and get datatable
                this.TargetDBConnection.ExecuteNonQuery(SqlQuery);

                //Step1:Get Unmatched Query for Area
                SqlQuery = this.ImportQueries.GetUnmatchedAreas( this._CurrentTemplateFileNameWPath);

                //Step2: Execute query and get datatable
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);

                this.ProcessDataTable(ref RetVal);

                DVTable = RetVal.DefaultView;
                DVTable.RowFilter = MergetTemplateConstants.Columns.COLUMN_UNMATCHED + "= 1";
                RetVal = DVTable.ToTable();

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
            AreaBuilder AreaBuilderObj = null;
            AreaInfo AreaInfoObj = null;
            Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            // Initialize progress bar
            this.RaiseProgressBarInitialize(selectedNids.Split(',').GetUpperBound(0) + 1);


            //////-- Step 1: Get TempTable with Sorted SourceFileName
            ////Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportAreas(this._CurrentTemplateFileNameWPath,selectedNids));

            //-- Step 2:Initialise Indicator Builder with Target DBConnection
            AreaBuilderObj = new AreaBuilder(this.TargetDBConnection, this.TargetDBQueries);

            ////-- Step 3: Import Nids for each SourceFile
            //foreach (DataRow Row in Table.Copy().Rows)
            //{
                try
                {


                    SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, this._CurrentTemplateFileNameWPath, String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);

                    // AreaInfoObj = this.GetIndicatorInfo(Row);

                    //AreaBuilderObj.ImportArea(selectedNids, 1, SourceDBConnection, SourceDBQueries);
                    //AreaBuilderObj.ImportAreaMaps(selectedNids, 1, SourceDBConnection, SourceDBQueries);

                    AreaBuilderObj.ImportArea(selectedNids, DICommon.SplitString(selectedNids, ",").Length, SourceDBConnection, SourceDBQueries, true);
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
            //}
            this._UnmatchedTable = this.GetUnmatchedTable();
            this._AvailableTable = this.GetAvailableTable();
            // Close ProgressBar
            this.RaiseProgressBarClose();
        }

        #endregion

        protected override bool IsElementAlreadyExists(DataRow unmatchedRow, DataRow availableRow)
        {
            bool RetVal = false;
            DataTable Table;
            string SelectionCriteria = string.Empty;

            string SourceFileName = string.Empty;

            try
            {
             

                // 1. check by Area ID
                Table = this.TargetDBConnection.ExecuteDataTable(this.TargetDBQueries.Area.GetArea(FilterFieldType.ID,"'"+ DICommon.RemoveQuotes(Convert.ToString(unmatchedRow[Area.AreaID])) +"'"));
                if (Table != null && Table.Rows.Count > 0)
                {
                    RetVal = true;
                }


                // 2. check src area_Name exists under parent of target area
                if (RetVal == false)
                {
                    SelectionCriteria = " " + Area.AreaParentNId + "=" + Convert.ToInt32(availableRow[Area.AreaParentNId]) + " AND " +
                        Area.AreaName + "='" + Convert.ToString(unmatchedRow[Area.AreaName]) + "' ";

                    Table = this.TargetDBConnection.ExecuteDataTable(this.TargetDBQueries.Area.GetArea(FilterFieldType.Search, SelectionCriteria));
                    if (Table != null && Table.Rows.Count > 0)
                    {
                        RetVal = true;
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

            return RetVal;
        }


        public override bool ISMappedValueExist(DataRow row)
        {
            bool RetVal = false;

            //if (this.UnmatchedTable.Select(Area.AreaGId + "='" + row[Area.AreaGId] + "'").Length > 0)
            if ((this.UnmatchedTable.Select(Area.AreaGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + Area.AreaGId] + "'").Length > 0) && (this.UnmatchedTable.Select(Area.AreaGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + Area.AreaGId] + "'").Length > 0))
                RetVal = true;

            return RetVal;

        }

        #endregion

        #endregion

    }
}
