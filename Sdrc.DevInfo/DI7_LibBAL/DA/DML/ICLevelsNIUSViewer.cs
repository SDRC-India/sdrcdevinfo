using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class ICLevelsNIUSViewer
    {

        #region "-- Private  --"

        #region "-- Variables --"

        private int MaxLevel = 0;

        private DIConnection SourceDBConnection;
        private DIQueries SourceDBQueries;



        #endregion

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods  --"

        private DataTable GetICTableWLevel(ref System.Data.DataTable table)
        {
            DataTable RetVal;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;

            table.Constraints.Clear();
            //- Allow db null
            table.Columns[IndicatorClassifications.ICType].AllowDBNull = true;

            //Step 1: Set indicator classification levels
            table.Columns.Add(LanguageKeys.Level, System.Type.GetType("System.Int32"));
            this.SetIndicatorClassificationLevel(table, -1, 0);

            //Step 2: Get maximum level from table 
            table.DefaultView.Sort = LanguageKeys.Level + " DESC";
            if (table.Rows.Count > 0)
            {
                this.MaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][LanguageKeys.Level]);
            }
            else
            {
                this.MaxLevel = 1;
            }

            //Step 2: Create New Data Table to fill the list view
            RetVal = table.Clone();

            //insert columns for classification level info.
            for (Level = 1; Level <= this.MaxLevel; Level++)
            {
                RetVal.Columns.Add(LanguageKeys.Level + " " + Level);
            }

            //insert rows into new table
            foreach (DataRow Row in table.Rows)
            {
                Rows = RetVal.Select(IndicatorClassifications.ICNId + " =" + Row[IndicatorClassifications.ICParent_NId].ToString());

                NewRow = RetVal.NewRow();
                NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName].ToString();
                NewRow[IndicatorClassifications.ICGlobal] = Row[IndicatorClassifications.ICGlobal];
                NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];

                NewRow[LanguageKeys.Level] = Row[LanguageKeys.Level];
                NewRow[LanguageKeys.Level + " " + Convert.ToInt32(Row[LanguageKeys.Level])] = Row[IndicatorClassifications.ICName].ToString();
                RetVal.Rows.Add(NewRow);
            }

            RetVal.AcceptChanges();
            this.SetIndicatorClassificationLevelName(RetVal);
            RetVal.DefaultView.Sort = IndicatorClassifications.ICNId;

            return RetVal;

        }

        private void SetIndicatorClassificationLevel(DataTable table, int parentNid, int parentLevel)
        {
            foreach (DataRow Row in table.Select(IndicatorClassifications.ICParent_NId + "=" + parentNid))
            {
                Row[LanguageKeys.Level] = parentLevel + 1;
                this.SetIndicatorClassificationLevel(table,
                    Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                    Convert.ToInt32(Row[LanguageKeys.Level]));

            }
        }

        private void SetIndicatorClassificationLevelName(DataTable table)
        {
            DataRow[] Rows;
            for (int Level = 2; Level <= this.MaxLevel; Level++)
            {
                foreach (DataRow Row in table.Select(LanguageKeys.Level + "=" + Level))
                {
                    Rows = table.Select(IndicatorClassifications.ICNId + "=" + Row[IndicatorClassifications.ICParent_NId].ToString());
                    for (int i = 1; i <= Level - 1; i++)
                    {
                        Row[LanguageKeys.Level + " " + i] = Rows[0][LanguageKeys.Level + " " + i];
                        table.AcceptChanges();
                    }

                }
            }
        }

        private DataTable DeleteExtraRows(DataTable table)
        {
            DataTable RetVal;
            int IndicatorNid = 0;
            int UnitNid = 0;
            int SubgroupValNid = 0;

            string FilterString = string.Empty;
            string LevelColumnName = string.Empty;

            try
            {
                RetVal = table.Clone();


                for (int Level = this.MaxLevel; Level > 0; Level--)
                {

                    DataRow[] Rows = table.Select(LanguageKeys.Level + "=" + Level);

                    // Get records by level
                    foreach (DataRow Row in Rows)
                    {
                        IndicatorNid = Convert.ToInt32(Row[Indicator.IndicatorNId]);


                        LevelColumnName = LanguageKeys.Level + " " + Level;
                        FilterString = "[" + LevelColumnName + "]='" + DICommon.RemoveQuotes(Row[LevelColumnName].ToString()) + "' And ";
                        FilterString += " " + Indicator.IndicatorNId + "=" + IndicatorNid;

                        if (this._ShowIUS)
                        {
                            UnitNid = Convert.ToInt32(Row[Unit.UnitNId]);
                            SubgroupValNid = Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]);

                            FilterString += " and " + Unit.UnitNId + "=" + UnitNid;
                            FilterString += " and " + SubgroupVals.SubgroupValNId + "=" + SubgroupValNid;
                        }

                        if (RetVal.Select(FilterString).Length == 0)
                        {
                            RetVal.Rows.Add(Row.ItemArray);
                        }

                    }
                }
                RetVal.AcceptChanges();

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private string GetSortingString()
        {
            string RetVal = string.Empty;
            for (int Level = 1; Level <= this.MaxLevel; Level++)
            {
                RetVal += LanguageKeys.Level + " " + Level + ", ";
            }

            RetVal += Indicator.IndicatorName;
            if (this._ShowIUS)
            {
                RetVal += "," + Unit.UnitName + "," + SubgroupVals.SubgroupVal;
            }
            return RetVal;
        }

        private string GetSqlQueryForIC(string searchString, ICType classificationType)
        {
            string RetVal = string.Empty;

            RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, classificationType, FieldSelection.Heavy);

            return RetVal;
        }

        private void ProcessDataTable(ref System.Data.DataTable table, ICType classificationType, bool isMetadataRequired, bool includeOnlyName)
        {
            string SqlQuery = string.Empty;
            DataTable NewTable;
            DataTable TempTable;
            DataTable SectorTable;
            DataRow NewRow;

            int Level = 0;

            //Step 1: Get IC table with levels
            SectorTable = this.GetICTableWLevel(ref table);

            //Step 2: Create New Data Table to fill the list view
            NewTable = new DataTable();

            //insert columns for IC level information.
            for (Level = 1; Level <= this.MaxLevel; Level++)
            {
                NewTable.Columns.Add(LanguageKeys.Level + " " + Level);
            }

            NewTable.Columns.Add(IndicatorClassifications.ICNId, System.Type.GetType("System.Int32"));
            NewTable.Columns.Add(IndicatorClassifications.ICParent_NId, System.Type.GetType("System.Int32"));
            NewTable.Columns.Add(IndicatorClassifications.ICGlobal, System.Type.GetType("System.Boolean"));
            NewTable.Columns.Add(IndicatorClassifications.ICGId);
            NewTable.Columns.Add(Indicator.IndicatorNId, System.Type.GetType("System.Int32"));
            NewTable.Columns.Add(Indicator.IndicatorGId);
            NewTable.Columns.Add(Indicator.IndicatorGlobal);

            if (isMetadataRequired)
            {
                NewTable.Columns.Add(IndicatorClassifications.ICInfo);
                NewTable.Columns.Add(Indicator.IndicatorInfo);
            }

            NewTable.Columns.Add(LanguageKeys.Level, System.Type.GetType("System.Int32"));
            NewTable.Columns.Add(IndicatorClassifications.ICName);
            NewTable.Columns.Add(Indicator.IndicatorName);

            //add unit and subgroup columns only if show IUS is true.
            if (this._ShowIUS)
            {
                NewTable.Columns.Add(Unit.UnitNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Unit.UnitGId);
                NewTable.Columns.Add(Unit.UnitGlobal);

                NewTable.Columns.Add(SubgroupVals.SubgroupValNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(SubgroupVals.SubgroupValGId);
                NewTable.Columns.Add(SubgroupVals.SubgroupValGlobal);

                NewTable.Columns.Add(Indicator_Unit_Subgroup.IUSNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Unit.UnitName);
                NewTable.Columns.Add(SubgroupVals.SubgroupVal);
            }                      

            //insert rows into new table
            foreach (DataRow Row in SectorTable.Rows)//.Select( LanguageKeys.Level + "=" + this.MaxLevel))
            {
                //get Indicator or IUS on the basis of IC_ICNid
                if (this._ShowIUS)
                {
                    SqlQuery = this.SourceDBQueries.IUS.GetAllIUSByIC(classificationType, Row[IndicatorClassifications.ICNId].ToString(), FieldSelection.Heavy);
                }
                else
                {
                    SqlQuery = this.SourceDBQueries.Indicators.GetAllIndicatorByIC(classificationType, Convert.ToString(Row[IndicatorClassifications.ICNId]), FieldSelection.Heavy);
                }

                TempTable = this.SourceDBConnection.ExecuteDataTable(SqlQuery);

                string[] ColArr=new string[TempTable.Columns.Count];
                int Index=0;
                foreach (DataColumn Col in TempTable.Columns)
                {
                    ColArr[Index++] = Col.ColumnName;
                } 

                TempTable = TempTable.DefaultView.ToTable(true, ColArr);

                foreach (DataRow IndicatorRow in TempTable.Rows)
                {
                    NewRow = NewTable.NewRow();
                    NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                    NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                    NewRow[IndicatorClassifications.ICGlobal] = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];

                    if (isMetadataRequired)
                    {
                        NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                    }


                    NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName].ToString();
                    NewRow[LanguageKeys.Level] = Row[LanguageKeys.Level];


                    for (Level = 1; Level < Convert.ToInt32(Row[LanguageKeys.Level]); Level++)
                    {
                        NewRow[LanguageKeys.Level + " " + Level] = Row[LanguageKeys.Level + " " + Level].ToString();
                    }

                    NewRow[LanguageKeys.Level + " " + Level] = Row[IndicatorClassifications.ICName].ToString();


                    //add indicator, unit and subgroup
                    NewRow[Indicator.IndicatorName] = IndicatorRow[Indicator.IndicatorName];
                    NewRow[Indicator.IndicatorNId] = IndicatorRow[Indicator.IndicatorNId];
                    NewRow[Indicator.IndicatorGId] = IndicatorRow[Indicator.IndicatorGId];
                    NewRow[Indicator.IndicatorGlobal] = IndicatorRow[Indicator.IndicatorGlobal];


                    //get indicatorinfo from indicator table
                    if (isMetadataRequired)
                    {
                        DataRow IndicatorInfoRow = this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorRow[Indicator.IndicatorNId].ToString(), FieldSelection.Heavy)).Rows[0];

                        NewRow[Indicator.IndicatorInfo] = IndicatorInfoRow[Indicator.IndicatorInfo];
                    }

                    if (this._ShowIUS)
                    {
                        NewRow[Unit.UnitName] = IndicatorRow[Unit.UnitName];
                        NewRow[SubgroupVals.SubgroupVal] = IndicatorRow[SubgroupVals.SubgroupVal];

                        NewRow[Unit.UnitNId] = IndicatorRow[Unit.UnitNId];
                        NewRow[Unit.UnitGId] = IndicatorRow[Unit.UnitGId];
                        NewRow[Unit.UnitGlobal] = IndicatorRow[Unit.UnitGlobal];

                        NewRow[SubgroupVals.SubgroupValNId] = IndicatorRow[SubgroupVals.SubgroupValNId];
                        NewRow[SubgroupVals.SubgroupValGId] = IndicatorRow[SubgroupVals.SubgroupValGId];
                        NewRow[SubgroupVals.SubgroupValGlobal] = IndicatorRow[SubgroupVals.SubgroupValGlobal];
                        NewRow[Indicator_Unit_Subgroup.IUSNId] = IndicatorRow[Indicator_Unit_Subgroup.IUSNId];

                    }


                    NewTable.Rows.Add(NewRow);
                }
            }
            NewTable.AcceptChanges();

            //remove extra rows from table
            NewTable = this.DeleteExtraRows(NewTable);


            ////////Step 4: If search string is not empty then filter data table 
            //////if (!string.IsNullOrEmpty(this.SearchString))
            //////{
            //////    NewTable.DefaultView.RowFilter = Indicator.IndicatorName + " LIKE '%" + this.SearchString + "%'";

            //////    NewTable = NewTable.DefaultView.ToTable();

            //////}

            //Step 5: set sorting
            NewTable.DefaultView.Sort = this.GetSortingString();

            //Step 6: Replace the referenced table with the new data table
            table = NewTable.DefaultView.ToTable();


            if (includeOnlyName)
            {
                table.Columns.Remove(IndicatorClassifications.ICNId);
                table.Columns.Remove(IndicatorClassifications.ICParent_NId);
                table.Columns.Remove(IndicatorClassifications.ICGlobal);
                table.Columns.Remove(IndicatorClassifications.ICGId);
                table.Columns.Remove(Indicator.IndicatorNId);
                table.Columns.Remove(Indicator.IndicatorGId);
                table.Columns.Remove(Indicator.IndicatorGlobal);
                table.Columns.Remove(LanguageKeys.Level);
                if (this._ShowIUS)
                {
                    table.Columns.Remove(Unit.UnitNId);
                    table.Columns.Remove(Unit.UnitGId);
                    table.Columns.Remove(Unit.UnitGlobal);
                    table.Columns.Remove(SubgroupVals.SubgroupValNId);
                    table.Columns.Remove(SubgroupVals.SubgroupValGId);
                    table.Columns.Remove(SubgroupVals.SubgroupValGlobal);
                    table.Columns.Remove(Indicator_Unit_Subgroup.IUSNId);
                }
            }

        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        protected bool _ShowIUS = true;
        /// <summary>
        /// Set true/false. True to show indicator,unit and subgroup. Default is true
        /// </summary>
        public bool ShowIUS
        {
            get
            {
                return this._ShowIUS;
            }
            set
            {
                this._ShowIUS = value;
            }
        }

        #endregion

        #region "-- New/Dispose --"

        public ICLevelsNIUSViewer(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.SourceDBConnection = dbConnection;
            this.SourceDBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods  --"

        public DataTable GetDataTable(string searchString, ICType classificationType, bool isMetadataRequired, bool includeOnlyName)
        {
            string SqlQuery = string.Empty;
            DataTable SourceTable = null;

            try
            {
                //Step1: Get sqlquery 
                SqlQuery = this.GetSqlQueryForIC(searchString, classificationType);

                //Step2: Execute query and get datatable
                SourceTable = this.SourceDBConnection.ExecuteDataTable(SqlQuery);


                this.ProcessDataTable(ref SourceTable, classificationType, isMetadataRequired, includeOnlyName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return SourceTable;
        }

        #endregion

        #endregion

        #region "-- Inner class --"

        private class LanguageKeys
        {
            public const string Level = "Level";
        }

        #endregion

    }
}
