using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting area for LookInWindow control
    /// </summary>
    public class AreaLookInSource : BaseLookInSource
    {
        #region"--Private--"

        #region "-- Variables --"

        private int MaxLevel = 0;
        private bool IsAreaMapListView = false;
        private string SearchString = string.Empty;

        #endregion

        #region"--Method--"

        private string GetColumnName(int level)
        {
            string RetVal = string.Empty;
            RetVal = Constants.LanguageKeys.Level + " " + level;
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
                                Row[Constants.LanguageKeys.Level + " " + i] = Rows[0][Constants.LanguageKeys.Level + " " + i];
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



        private void AreaBuilderObj_BeforeProcess(int recordsFound)
        {
            this.RaiseInitializeProgessBarEvent(recordsFound);
        }

        private void AreaBuilderObj_ProcessInfo(int currentRecordNumber)
        {
            this.RaiseIncrementProgessBarEvent(currentRecordNumber);

        }


        #endregion

        #endregion

        #region"--Protected / Public --"

        #region"--Method--"

        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;

            //save the searach string and do the filtering of records in processDataTable()
            this.SearchString = searchString;

            RetVal = this.SourceDBQueries.Area.GetArea(FilterFieldType.None, string.Empty);
            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
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
                    NewRow[Area.AreaLevel] = Row[Area.AreaLevel];
                    NewRow[this.GetColumnName(Convert.ToInt32(NewRow[Area.AreaLevel]))] = Row[Area.AreaName].ToString();
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
                SortString += Constants.LanguageKeys.Level + " " + i + " ASC ";
            }

            NewTable.DefaultView.Sort = SortString;

            //Step 6: Replace the referenced table with the new data table
            table = NewTable.DefaultView.ToTable();
        }

        #endregion

        #endregion


        #region "-- public  --"

        #region "-- New/Dispose --"

        public AreaLookInSource(bool isAreaMapListView)
        {
            this.IsAreaMapListView = isAreaMapListView;
        }

        #endregion

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> SelectedNids, bool allSelected)
        {
            AreaBuilder AreaBuilderObj = new AreaBuilder(this._TargetDBConnection, this._TargetDBQueries);
            DI7MetadataCategoryBuilder AreaMetadataCategoryBuilder = null;
            int SelectedCount = SelectedNids.Count;

            try
            {

                // 1. import all area metadata categories
                if (this.IncludeMap)
                {
                    AreaMetadataCategoryBuilder = new DI7MetadataCategoryBuilder(this._TargetDBConnection, this._TargetDBQueries);
                    AreaMetadataCategoryBuilder.ImportAllMetadataCategories(this.SourceDBConnection, this.SourceDBQueries, MetadataElementType.Area);
                }


                // 2. add event handlers
                AreaBuilderObj.ProcessInfo += new ProcessInfoDelegate(AreaBuilderObj_ProcessInfo);
                AreaBuilderObj.BeforeProcess += new ProcessInfoDelegate(AreaBuilderObj_BeforeProcess);

                // 3. updated selected count variable
                if (allSelected)
                    SelectedCount = -1;

                // 4. import selected areas
                if (this.IncludeArea & this.IncludeMap)
                {
                    // import area and maps

                    AreaBuilderObj.ImportArea(string.Join(",", SelectedNids.ToArray()), SelectedCount, this.SourceDBConnection, this.SourceDBQueries, this.SelectedAreaLevel, true);
                }
                else if (this.IncludeArea)
                {
                    // import area
                    AreaBuilderObj.ImportArea(string.Join(",", SelectedNids.ToArray()), SelectedCount, this.SourceDBConnection, this.SourceDBQueries, this.SelectedAreaLevel);
                }
                else if (this.IncludeMap)
                {
                    // import area maps
                    AreaBuilderObj.ImportAreaMaps(string.Join(",", SelectedNids.ToArray()), SelectedCount, this.SourceDBConnection, this.SourceDBQueries, this.SelectedAreaLevel);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();

            for (int Level = 1; Level <= this.MaxLevel; Level++)
            {
                //key is actual column name and value is display string(header name)
                this.Columns.Add(this.GetColumnName(Level), DevInfo.Lib.DI_LibBAL.Utility.DILanguage.GetLanguageString(Constants.LanguageKeys.Level) + " " + Level);
            }

            this.TagValueColumnName = Area.AreaNId;
        }


        #endregion

        #endregion

        #endregion

    }


}
