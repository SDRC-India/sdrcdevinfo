using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class ICSource : TemplateMergingControlSource
    {

        #region "-- private/protected --"

        #region "-- Variables --"

        private int MaxLevel = 0;
        private int CurrentMaxLevel = 0;
        private string SearchString = string.Empty;

        #endregion

        #region "-- Methods --"

        protected override void InitializeVaribles()
        {
            this.KeyColumnName =  IndicatorClassifications.ICNId;
            this._GlobalColumnName = IndicatorClassifications.ICGlobal;

            if (this._DisplayColumnsInfo == null)
                this._DisplayColumnsInfo = new Dictionary<string, string>();
            this._DisplayColumnsInfo.Add(IndicatorClassifications.ICName, DILanguage.GetLanguageString("INDICATORCLASSIFICATION"));
        }

        #region "-- Process IC --"

        private void SetIndicatorClassificationLevelName(DataTable table)
        {
            DataRow[] Rows;
            for (int Level = 2; Level <= this.CurrentMaxLevel; Level++)
            {
                foreach (DataRow Row in table.Select(MergetTemplateConstants.Columns.Level + "=" + Level))
                {
                    Rows = table.Select(IndicatorClassifications.ICNId + "=" + Row[IndicatorClassifications.ICParent_NId].ToString());
                    for (int i = 1; i <= Level - 1; i++)
                    {
                        Row[MergetTemplateConstants.Columns.Level + " " + i] = Rows[0][MergetTemplateConstants.Columns.Level + " " + i];
                        table.AcceptChanges();
                    }

                }
            }
        }

        private void SetIndicatorClassificationLevel(DataTable table, int parentNid, int parentLevel)
        {
            foreach (DataRow Row in table.Select(IndicatorClassifications.ICParent_NId + "=" + parentNid))
            {
                Row[MergetTemplateConstants.Columns.Level] = parentLevel + 1;
                this.SetIndicatorClassificationLevel(table,
                    Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                    Convert.ToInt32(Row[MergetTemplateConstants.Columns.Level]));

            }
        }


        private void ProcessDataTable(ref System.Data.DataTable table)
        {
            DataTable NewTable;
            DataRow NewRow;
            int Level = 0;

            try
            {

                //Step 1: Set indicator classification levels
                table.Columns.Add(MergetTemplateConstants.Columns.Level, System.Type.GetType("System.Int32"));
                this.SetIndicatorClassificationLevel(table, -1, 0);

                //Step 2: Get maximum level from table 
                table.DefaultView.Sort = MergetTemplateConstants.Columns.Level + " DESC";
                if (table.Rows.Count > 0)
                {
                    this.CurrentMaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][MergetTemplateConstants.Columns.Level]);
                }
                else
                {
                    this.CurrentMaxLevel = 1;
                }

                //Setp 2: Set Columns Info accordingly
                this.SetColumnsInfo(this.CurrentMaxLevel);

                // Set Maximum Column Level 
                if (this.MaxLevel < this.CurrentMaxLevel)
                    this.MaxLevel = this.CurrentMaxLevel;

                //Step 3: Create New Data Table to fill the list view
                NewTable = new DataTable();
                NewTable.Columns.Add(IndicatorClassifications.ICNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICParent_NId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(MergetTemplateConstants.Columns.Level, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICGlobal, System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(IndicatorClassifications.ICGId);
                NewTable.Columns.Add(IndicatorClassifications.ICInfo);
                NewTable.Columns.Add(IndicatorClassifications.ICName);
                NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME);

                NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMNS_NEWICNID, System.Type.GetType("System.Int32"));

                try
                {
                    NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMN_UNMATCHED);
                }
                catch { }
                //insert columns for area level info.
                for (Level = 1; Level <= this.CurrentMaxLevel; Level++)
                {
                    NewTable.Columns.Add(MergetTemplateConstants.Columns.Level + " " + Level);
                }

                //insert rows into new table
                foreach (DataRow Row in table.Rows)
                {
                    NewRow = NewTable.NewRow();
                    NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                    NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                    NewRow[IndicatorClassifications.ICGlobal] = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];
                    NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                    NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName];
                    NewRow[MergetTemplateConstants.Columns.Level] = Row[MergetTemplateConstants.Columns.Level];
                    if(Row.Table.Columns.Contains(MergetTemplateConstants.Columns.COLUMNS_NEWICNID))
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMNS_NEWICNID] = Row[MergetTemplateConstants.Columns.COLUMNS_NEWICNID];
                    }

                    if (Row.Table.Columns.Contains(MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME))
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME] = Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME];
                    }

                    try
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMN_UNMATCHED] = Row[MergetTemplateConstants.Columns.COLUMN_UNMATCHED];
                    }
                    catch { }
                    try
                    {
                        NewRow[MergetTemplateConstants.Columns.Level + " " + Convert.ToString(Row[MergetTemplateConstants.Columns.Level])] = Row[IndicatorClassifications.ICName].ToString();
                        NewTable.Rows.Add(NewRow);
                    }
                    catch { }

                }

                NewTable.AcceptChanges();
                this.SetIndicatorClassificationLevelName(NewTable);
                NewTable.DefaultView.Sort = IndicatorClassifications.ICNId;

                //Step 4: If search string is not empty then filter data table 
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    NewTable.DefaultView.RowFilter = IndicatorClassifications.ICName + " LIKE '%" + this.SearchString + "%'";

                    NewTable = NewTable.DefaultView.ToTable();

                }

                //Step 5: do the sorting on the basis of available levels
                string SortString = string.Empty;
                for (int i = 1; i <= CurrentMaxLevel; i++)
                {
                    if (!string.IsNullOrEmpty(SortString))
                    {
                        SortString += " ,";
                    }
                    SortString += MergetTemplateConstants.Columns.Level + " " + i;
                }

                NewTable.DefaultView.Sort = SortString;


                //Step 6: Replace the referenced table with the new data table
                table = NewTable.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void ProcessMultiDataTable(ref System.Data.DataTable table)
        {
            DataTable NewTable;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;

            try
            {

                //Step 1: Set indicator classification levels
                table.Columns.Add(MergetTemplateConstants.Columns.Level, System.Type.GetType("System.Int32"));
                this.SetIndicatorClassificationLevel(table, -1, 0);

                //Step 2: Get maximum level from table 
                table.DefaultView.Sort = MergetTemplateConstants.Columns.Level + " DESC";
                if (table.Rows.Count > 0)
                {
                    this.CurrentMaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][MergetTemplateConstants.Columns.Level]);
                }
                else
                {
                    this.CurrentMaxLevel = 1;
                }

                //Setp 2: Set Columns Info accordingly
                this.SetColumnsInfo(this.CurrentMaxLevel);

                //Step 3: Create New Data Table to fill the list view
                NewTable = new DataTable();
                NewTable.Columns.Add(IndicatorClassifications.ICNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICParent_NId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(MergetTemplateConstants.Columns.Level, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICGlobal, System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(IndicatorClassifications.ICGId);
                NewTable.Columns.Add(IndicatorClassifications.ICInfo);
                NewTable.Columns.Add(IndicatorClassifications.ICName);
                NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME);
                try
                {
                    NewTable.Columns.Add(MergetTemplateConstants.Columns.COLUMN_UNMATCHED);
                }
                catch { }
                //insert columns for area level info.
                for (Level = 1; Level <= this.CurrentMaxLevel; Level++)
                {
                    NewTable.Columns.Add(MergetTemplateConstants.Columns.Level + " " + Level);
                }

                //insert rows into new table
                foreach (DataRow Row in table.Rows)
                {
                    NewRow = NewTable.NewRow();
                    NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                    NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                    NewRow[IndicatorClassifications.ICGlobal] = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];
                    NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                    NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName];
                    NewRow[MergetTemplateConstants.Columns.Level] = Row[MergetTemplateConstants.Columns.Level];

                    if (Row.Table.Columns.Contains(MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME))
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME] = Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME];
                    }

                    try
                    {
                        NewRow[MergetTemplateConstants.Columns.COLUMN_UNMATCHED] = Row[MergetTemplateConstants.Columns.COLUMN_UNMATCHED];
                    }
                    catch { }
                    try
                    {
                        NewRow[MergetTemplateConstants.Columns.Level + " " + Convert.ToString(Row[MergetTemplateConstants.Columns.Level])] = Row[IndicatorClassifications.ICName].ToString();
                        NewTable.Rows.Add(NewRow);
                    }
                    catch { }

                }

                NewTable.AcceptChanges();
                this.SetIndicatorClassificationLevelName(NewTable);
                NewTable.DefaultView.Sort = IndicatorClassifications.ICNId;

                //Step 4: If search string is not empty then filter data table 
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    NewTable.DefaultView.RowFilter = IndicatorClassifications.ICName + " LIKE '%" + this.SearchString + "%'";

                    NewTable = NewTable.DefaultView.ToTable();

                }

                //Step 5: do the sorting on the basis of available levels
                string SortString = string.Empty;
                for (int i = 1; i <= CurrentMaxLevel; i++)
                {
                    if (!string.IsNullOrEmpty(SortString))
                    {
                        SortString += " ,";
                    }
                    SortString += MergetTemplateConstants.Columns.Level + " " + i;
                }

                NewTable.DefaultView.Sort = SortString;


                //Step 6: Replace the referenced table with the new data table
                table = NewTable.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public / Friend --"
        
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/ Dispose --"

        public ICSource()
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

                //Step1:Get Unmatched Query for ICType
                SqlQuery = this.TargetDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, this._CurrentICType, FieldSelection.Heavy);
                //Step2: Execute query and get datatable
                RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);

                this.ProcessDataTable(ref RetVal);
            }
            catch (Exception ex)
            { throw ex; }
            return RetVal;
        }

        //public override DataTable GetUnmatchedTable()
        //{
        //    DataTable RetVal = null;
        //    DataView DVTable = null;
        //    string SqlQuery = string.Empty;
        //    try
        //    {

        //        //Step1:Set Unmatched Value to false
        //        SqlQuery = this.ImportQueries.ClearAllUnmatchedIC();

        //        //Step2: Execute query 
        //        this.TargetDBConnection.ExecuteNonQuery(SqlQuery);

        //        //Step1:Get Unmatched Query for ICType
        //        SqlQuery = this.ImportQueries.UpdateUnmatchedIC(this._CurrentICType, this._CurrentTemplateFileNameWPath);

        //        //Step2: Execute query 
        //        this.TargetDBConnection.ExecuteNonQuery(SqlQuery);

        //        //Step1:Get Unmatched Query for ICType
        //        SqlQuery = this.ImportQueries.GetUnmatchedIC(this._CurrentICType, this._CurrentTemplateFileNameWPath);

        //        //Step2: Execute query and get datatable
        //        RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);

        //        this.ProcessDataTable(ref RetVal);

        //        DVTable = RetVal.Copy().DefaultView;
        //        DVTable.RowFilter = MergetTemplateConstants.Columns.COLUMN_UNMATCHED + " =1";
        //        RetVal.Dispose();
        //        RetVal = DVTable.ToTable();

        //    }
        //    catch (Exception ex)
        //    { throw ex; }


        //    return RetVal;
        //}


        public override DataTable GetUnmatchedTable()
        {
            DataTable RetVal = null;
            DataView DVTable = null;
            string SqlQuery = string.Empty;
            DataTable TempTable=null;
            DataTable OtherTable = null;
            string SortString=string.Empty;
            this.MaxLevel = 0;

            // Get UnMatched DataTable from All Source Files
            foreach (string FileNameWPath in this._SourceFilesWPath)
            {
                try
                {    
                
                    // Step1:Set Unmatched Value to false
                    SqlQuery = this.ImportQueries.ClearAllUnmatchedIC();

                    // Step2: Execute query 
                    this.TargetDBConnection.ExecuteNonQuery(SqlQuery);

                    // Step1:Get Unmatched Query for ICType
                    SqlQuery = this.ImportQueries.UpdateUnmatchedIC(this._CurrentICType, FileNameWPath);

                    // Step2: Execute query 
                    this.TargetDBConnection.ExecuteNonQuery(SqlQuery);

                    // Step1:Get Unmatched Query for ICType
                    SqlQuery = this.ImportQueries.GetUnmatchedIC(this._CurrentICType, FileNameWPath);

                    // Step2: Execute query and get datatable
                    RetVal = this.TargetDBConnection.ExecuteDataTable(SqlQuery);

                    this.ProcessDataTable(ref RetVal);

                    DVTable = RetVal.Copy().DefaultView;
                    DVTable.RowFilter = MergetTemplateConstants.Columns.COLUMN_UNMATCHED + " =1";
                   
                    TempTable = DVTable.ToTable();
                    
                    // Merge Table 
                    if (OtherTable != null)
                    {
                        if (OtherTable.Columns.Count > TempTable.Columns.Count)
                            OtherTable.Merge(TempTable);
                        else
                        {
                            TempTable.Merge(OtherTable);
                            OtherTable = TempTable.Copy();
                        }
                    }
                    else
                    {
                        OtherTable = TempTable.Copy();
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }

            SetColumnsInfo(this.MaxLevel);

            foreach (string  Col in this.DisplayColumnsInfo.Keys)
	        {
                if(string.IsNullOrEmpty(SortString))
                   SortString = Col + " Asc ";
                else
                    SortString += "," + Col + " Asc ";
	        }

            DVTable = OtherTable.DefaultView;
            DVTable.Sort = MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME + " Asc, " + SortString;

            RetVal = DVTable.ToTable();
            return RetVal;
        }

        /// <summary>
        /// Sets the columns info like name etc
        /// </summary>
        public void SetColumnsInfo(int currentMaxLevel)
        {
            
            this.DisplayColumnsInfo.Clear();
            for (int Level = 1; Level <= currentMaxLevel; Level++)
            {
                this.DisplayColumnsInfo.Add(MergetTemplateConstants.Columns.Level + " " + Level, DILanguage.GetLanguageString(MergetTemplateConstants.Columns.Level) + " " + Level);
            }
         
            
        }

        #endregion

        #region "-- Import --"

        private IndicatorClassificationInfo GetICInfo(DataRow row)
        {
            IndicatorClassificationInfo RetVal;

            try
            {
                //get unit from source table
                RetVal = new IndicatorClassificationInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[IndicatorClassifications.ICName].ToString());
                RetVal.GID = row[IndicatorClassifications.ICGId].ToString();
                RetVal.IsGlobal = Convert.ToBoolean(row[IndicatorClassifications.ICGlobal]);
                RetVal.ClassificationInfo = DICommon.RemoveQuotes(Convert.ToString(row[IndicatorClassifications.ICInfo]));
                RetVal.Nid = Convert.ToInt32(row[IndicatorClassifications.ICNId]);
                RetVal.Parent = new IndicatorClassificationInfo();
                RetVal.Parent.Nid = Convert.ToInt32(row[IndicatorClassifications.ICParent_NId]);
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        public override void Import(string selectedNIds)
        {

            IndicatorClassificationInfo SrcClassification;
            string SrcFileName = string.Empty;
            DataTable Table = null;
            int ProgressCounter = 0;
            //IndicatorClassificationBuilder  ICBuilder = null;
            //Dictionary<string, DataRow> FileWithNids = new Dictionary<string, DataRow>();

            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;

            // Initialize progress bar
            this.RaiseProgressBarInitialize(selectedNIds.Split(',').GetUpperBound(0) + 1);


            //-- Step 1: Get TempTable with Sorted SourceFileName
            Table = this._TargetDBConnection.ExecuteDataTable(this.ImportQueries.GetImportICs( selectedNIds));

            ////-- Step 2:Initialise Indicator Builder with Target DBConnection
            //ICBuilder = new IndicatorClassificationBuilder(this.TargetDBConnection, this.TargetDBQueries);

            try
            {


                //-- Step 3: Import Nids for each SourceFile
                foreach (DataRow Row in Table.Copy().Rows)
                {
                    try
                    {
                        if (SrcFileName != Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]))
                        {
                            SrcFileName = Convert.ToString(Row[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                            //-- Step 2:Initialise DBConnection
                            if (SourceDBConnection != null)
                            {
                                SourceDBConnection.Dispose();
                            }

                            SourceDBConnection = new DIConnection(DIServerType.MsAccess, String.Empty, String.Empty, SrcFileName, String.Empty, MergetTemplateConstants.DBPassword);
                            SourceDBQueries = DataExchange.GetDBQueries(SourceDBConnection);
                        }

                        SrcClassification = new IndicatorClassificationInfo();
                        SrcClassification.Name = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICName].ToString());
                        SrcClassification.GID = Row[IndicatorClassifications.ICGId].ToString();
                        SrcClassification.IsGlobal = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                        SrcClassification.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                        if (!string.IsNullOrEmpty(Convert.ToString((Row[IndicatorClassifications.ICInfo]))))
                        {
                            SrcClassification.ClassificationInfo = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICInfo].ToString());
                        }

                        SrcClassification.Parent = new IndicatorClassificationInfo();
                        SrcClassification.Parent.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]);
                        SrcClassification.Type = this._CurrentICType;

                        //import into target database
                        this.CreateClassificationChainFromExtDB(
                            SrcClassification.Nid,
                            SrcClassification.Parent.Nid,
                            SrcClassification.GID,
                            SrcClassification.Name,
                            SrcClassification.Type,
                            SrcClassification.ClassificationInfo,
                            SrcClassification.IsGlobal,
                           SourceDBQueries, SourceDBConnection, this._TargetDBQueries, this._TargetDBConnection);

                        ProgressCounter += 1;
                        this.RaiseProgressBarIncrement(ProgressCounter);

                    }
                    catch (Exception ex)
                    {
                        ExceptionFacade.ThrowException(ex);
                    }
                    this._UnmatchedTable = this.GetUnmatchedTable();
                    this._AvailableTable = this.GetAvailableTable();

                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (SourceDBConnection != null)
                {
                    SourceDBConnection.Dispose();
                }
            }

            // Close ProgressBar
            this.RaiseProgressBarClose();
        }
        
       

        internal int CreateClassificationChainFromExtDB(int srcICNId, int srcParentNId, string srcICGid, string srcICName, ICType srcICType, string srcICInfo, bool isGlobal, DIQueries srcQueries, DIConnection srcDBConnection, DIQueries targetDBQueries, DIConnection targetDBConnection)
        {

            int RetVal;
            //int TrgParentNId; 
            //string TrgParentName; 
            int NewParentNId;
            DataTable TempTable;
            IndicatorClassificationInfo ICInfo;
            IndicatorClassificationBuilder ClassificationBuilder = new IndicatorClassificationBuilder(targetDBConnection, targetDBQueries);


            // -- STEP 1: If the Parent NID is -1 then create the Classification at the root 
            if (srcParentNId == -1)
            {
                // -- Create the Classification 

                // -------------------------------------------------------------- 
                // While importing the Classifications, if the NId of the Source Classification is _ 
                // the same as that of the one created, then the Duplicate check fails and a duplicate 
                // classification getscreated. PASS -99 as the first parameter to the calling function 
                // -------------------------------------------------------------- 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = srcParentNId;
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }



            else
            {
                // -- STEP 2: If the Parent is not -1 then check for the existence of the Parent and then create the Classification 
                // Classification can only be created if the parent exists 
                // -- STEP 2.1: If the Parent Exists then create the Classification under that parent 
                // -- STEP 2.2: If the Parent does not Exist then create the Parent first and then the Classification under that parent 

                // -- STEP 2: Check the existence of the Parent in the Target Database 
                // -- get the parent from the source database 

                TempTable = srcDBConnection.ExecuteDataTable(srcQueries.IndicatorClassification.GetIC(FilterFieldType.NId, srcParentNId.ToString(), srcICType, FieldSelection.Heavy));
                {

                    // -------------------------------------------------------------- 
                    // While importing the Classifications, if the NId of the Source Classification is _ 
                    // the same as that of the one created, then the Duplicate check fails and a duplicate 
                    // classification getscreated. PASS -99 as the first parameter to the calling function 
                    // -------------------------------------------------------------- 
                    DataRow Row;
                    string ClassificationInfo = string.Empty;
                    Row = TempTable.Rows[0];
                    ClassificationInfo = Convert.ToString(Row[IndicatorClassifications.ICInfo]);

                    NewParentNId = CreateClassificationChainFromExtDB(
                       Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                       Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]),
                        Row[IndicatorClassifications.ICGId].ToString(),
                        Row[IndicatorClassifications.ICName].ToString(),
                        srcICType,
                        ClassificationInfo, Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]), srcQueries, srcDBConnection, targetDBQueries, targetDBConnection); ;
                }




                // -- Create the Child Now 
                ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();
                ICInfo.Parent.Nid = NewParentNId;       // set new parent nid
                ICInfo.Nid = srcICNId;
                ICInfo.Name = srcICName;
                ICInfo.ClassificationInfo = srcICInfo;
                ICInfo.GID = srcICGid;
                ICInfo.IsGlobal = isGlobal;
                ICInfo.Type = srcICType;

                RetVal = ClassificationBuilder.ImportIndicatorClassification(ICInfo, srcICNId, srcQueries, srcDBConnection);

            }

            //import ic and ius relationship into indicator_classification_IUS table
            ClassificationBuilder.ImportICAndIUSRelations(srcICNId, RetVal, ICInfo.Type, srcQueries, srcDBConnection);

            return RetVal;
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
                //get sourcefiilename
                SourceFileName = Convert.ToString(unmatchedRow[MergetTemplateConstants.Columns.COLUMN_SOURCEFILENAME]);

                
                // 1. check by GID
                Table = this.TargetDBConnection.ExecuteDataTable(this.TargetDBQueries.IndicatorClassification.GetIC(FilterFieldType.GId, "'"+ DICommon.RemoveQuotes(Convert.ToString(unmatchedRow[IndicatorClassifications.ICGId]))+ "'",   FieldSelection.Light  ));
                if (Table != null && Table.Rows.Count > 0)
                {
                    RetVal = true;
                }


                // 2. check src IC_Name exists under parent of target IC
                if (RetVal == false)
                {
                    SelectionCriteria = " " + IndicatorClassifications.ICParent_NId + "=" + Convert.ToInt32(availableRow[IndicatorClassifications.ICParent_NId]) + " AND " +
                        IndicatorClassifications.ICName + "='" + Convert.ToString(unmatchedRow[IndicatorClassifications.ICName]) + "' ";

                    Table = this.TargetDBConnection.ExecuteDataTable(this.TargetDBQueries.IndicatorClassification.GetIC(FilterFieldType.Search, SelectionCriteria,FieldSelection.Light));
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

            // if (this.UnmatchedTable.Select(IndicatorClassifications.ICGId + "='" + row[IndicatorClassifications.ICGId] + "'").Length > 0)
            if ((this.UnmatchedTable.Select(IndicatorClassifications.ICGId + "='" + row[MergetTemplateConstants.Columns.UNMATCHED_COL_Prefix + IndicatorClassifications.ICGId] + "'").Length > 0) && (this.UnmatchedTable.Select(IndicatorClassifications.ICGId + "='" + row[MergetTemplateConstants.Columns.AVAILABLE_COL_Prefix + IndicatorClassifications.ICGId] + "'").Length > 0))
                RetVal = true;

            return RetVal;

        }


        #endregion

        #endregion
    }
}
