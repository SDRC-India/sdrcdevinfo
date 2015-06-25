using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Indicator for LookInWindow control
    /// </summary>
    public class IndicatorLookInSource : BaseLookInSource
    {
        #region "-- Private --"

        #region "-- Variables --"

        private int MaxLevel = 0;
        private string SearchString = string.Empty;

        #endregion

        #region"-- Methods --"

        private string GetSqlQueryForSectors(string searchString)
        {
            string RetVal = string.Empty;

            this.SearchString = searchString;
            RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Sector, FieldSelection.Heavy);

            return RetVal;
        }

        private void SetIndicatorClassificationLevel(DataTable table, int parentNid, int parentLevel)
        {
            foreach (DataRow Row in table.Select(IndicatorClassifications.ICParent_NId + "=" + parentNid))
            {
                Row[Constants.LanguageKeys.Level] = parentLevel + 1;
                this.SetIndicatorClassificationLevel(table,
                    Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                    Convert.ToInt32(Row[Constants.LanguageKeys.Level]));

            }
        }

        private void SetIndicatorClassificationLevelName(DataTable table)
        {
            DataRow[] Rows;
            for (int Level = 2; Level <= this.MaxLevel; Level++)
            {
                foreach (DataRow Row in table.Select(Constants.LanguageKeys.Level + "=" + Level))
                {
                    Rows = table.Select(IndicatorClassifications.ICNId + "=" + Row[IndicatorClassifications.ICParent_NId].ToString());
                    for (int i = 1; i <= Level - 1; i++)
                    {
                        Row[Constants.LanguageKeys.Level + " " + i] = Rows[0][Constants.LanguageKeys.Level + " " + i];
                        table.AcceptChanges();
                    }

                }
            }
        }

        private DataTable GetSectorTableWLevel(ref System.Data.DataTable table)
        {
            DataTable RetVal;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;

            //Step 1: Set indicator classification levels
            table.Columns[IndicatorClassifications.ICType].AllowDBNull = true;
            table.Columns.Add(Constants.LanguageKeys.Level, System.Type.GetType("System.Int32"));
            this.SetIndicatorClassificationLevel(table, -1, 0);

            //Step 2: Get maximum level from table 
            table.DefaultView.Sort = Constants.LanguageKeys.Level + " DESC";
            if (table.Rows.Count > 0)
            {
                this.MaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][Constants.LanguageKeys.Level]);
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
                RetVal.Columns.Add(Constants.LanguageKeys.Level + " " + Level);
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

                NewRow[Constants.LanguageKeys.Level] = Row[Constants.LanguageKeys.Level];
                NewRow[Constants.LanguageKeys.Level + " " + Convert.ToInt32(Row[Constants.LanguageKeys.Level])] = Row[IndicatorClassifications.ICName].ToString();
                RetVal.Rows.Add(NewRow);
            }

            RetVal.AcceptChanges();
            this.SetIndicatorClassificationLevelName(RetVal);
            RetVal.DefaultView.Sort = IndicatorClassifications.ICNId;

            return RetVal;

        }

        private string GetSortingString()
        {
            string RetVal = string.Empty;
            for (int Level = 1; Level <= this.MaxLevel; Level++)
            {
                RetVal += Constants.LanguageKeys.Level + " " + Level + ", ";
            }

            RetVal += Indicator.IndicatorName;
            if (this._ShowIUS)
            {
                RetVal += "," + Unit.UnitName + "," + SubgroupVals.SubgroupVal;
            }
            return RetVal;
        }

        private void ImportSector(DataRow row)
        {
            IndicatorClassificationInfo SrcClassification;

            try
            {
                //get ic from source table
                SrcClassification = new IndicatorClassificationInfo();
                SrcClassification.Name = DICommon.RemoveQuotes(row[IndicatorClassifications.ICName].ToString());
                SrcClassification.GID = row[IndicatorClassifications.ICGId].ToString();
                SrcClassification.IsGlobal = Convert.ToBoolean(row[IndicatorClassifications.ICGlobal]);
                SrcClassification.Nid = Convert.ToInt32(row[IndicatorClassifications.ICNId]);
                if (!Information.IsDBNull(row[IndicatorClassifications.ICInfo]))
                {
                    SrcClassification.ClassificationInfo = DICommon.RemoveQuotes(row[IndicatorClassifications.ICInfo].ToString());
                }

                SrcClassification.Parent = new IndicatorClassificationInfo();
                SrcClassification.Parent.Nid = Convert.ToInt32(row[IndicatorClassifications.ICParent_NId]);
                SrcClassification.Type = ICType.Sector;

                //import into target database
                Utility.CreateClassificationChainFromExtDB(
                    SrcClassification.Nid,
                    SrcClassification.Parent.Nid,
                    SrcClassification.GID,
                    SrcClassification.Name,
                    SrcClassification.Type,
                    SrcClassification.ClassificationInfo,
                    SrcClassification.IsGlobal,
                    this.SourceDBQueries, this.SourceDBConnection, this._TargetDBQueries, this._TargetDBConnection);

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        private int ImportForIUS(DataRow row)
        {
            int RetVal = 0;
            IUSInfo iusInfo = new IUSInfo();
            IUSBuilder iusBuilder = new IUSBuilder(this._TargetDBConnection, this._TargetDBQueries);

            iusInfo.IndicatorInfo = this.GetIndicatorInfo(row);
            iusInfo.UnitInfo = this.GetUnitInfo(row);
            iusInfo.SubgroupValInfo = this.GetSubgroupValInfo(row);

            RetVal = iusBuilder.ImportIUS(iusInfo, this.SourceDBQueries, this.SourceDBConnection);

            bool ISDefault = false;
            if (!string.IsNullOrEmpty(Convert.ToString(row[Indicator_Unit_Subgroup.IsDefaultSubgroup])))
            {
                ISDefault = Convert.ToBoolean(row[Indicator_Unit_Subgroup.IsDefaultSubgroup]);
            }

            iusBuilder.UpdateIUSISDefaultSubgroup(RetVal.ToString(), ISDefault);

            iusBuilder.UpdateISDefaultSubgroup(iusInfo.IndicatorInfo.Nid, iusInfo.UnitInfo.Nid);
 
            //if (importSector)
            //{
            //    //create indicator classification
            //    this.ImportSector(row);
            //}

            return RetVal;
        }

        private DI6SubgroupValInfo GetSubgroupValInfo(DataRow row)
        {

            DI6SubgroupValInfo RetVal;
            DataTable TempTable;

            try
            {
                RetVal = new DI6SubgroupValInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[SubgroupVals.SubgroupVal].ToString());
                RetVal.GID = row[SubgroupVals.SubgroupValGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[SubgroupVals.SubgroupValGlobal]);
                RetVal.Nid = Convert.ToInt32(row[SubgroupVals.SubgroupValNId]);

            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        private UnitInfo GetUnitInfo(DataRow row)
        {

            UnitInfo RetVal;

            try
            {
                //get unit from source table
                RetVal = new UnitInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[Unit.UnitName].ToString());
                RetVal.GID = row[Unit.UnitGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[Unit.UnitGlobal]);
                RetVal.Nid = Convert.ToInt32(row[Unit.UnitNId]);

            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        private IndicatorInfo GetIndicatorInfo(DataRow row)
        {
            IndicatorInfo RetVal;

            try
            {
                //get unit from source table
                RetVal = new IndicatorInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[Indicator.IndicatorName].ToString());
                RetVal.GID = row[Indicator.IndicatorGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[Indicator.IndicatorGlobal]);
                RetVal.Info = DICommon.RemoveQuotes(Convert.ToString(row[Indicator.IndicatorInfo]));
                RetVal.Nid = Convert.ToInt32(row[Indicator.IndicatorNId]);
                RetVal.HighIsGood = Convert.ToBoolean(row[Indicator.HighIsGood]);
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region"-- Methods --"

        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (this._ShowSector)
            {
                RetVal = this.GetSqlQueryForSectors(searchString);
            }
            else
            {

                if (!string.IsNullOrEmpty(searchString))
                {

                    if (this._ShowIUS)
                    {
                        FilterString = " And I." + Indicator.IndicatorName + " LIKE '%" + searchString + "%' ";
                        RetVal = this.SourceDBQueries.IUS.GetIUS(FilterFieldType.Search, "" + FilterString + "", FieldSelection.Heavy);
                    }
                    else
                    {
                        FilterString = " " + Indicator.IndicatorName + " LIKE '%" + searchString + "%' ";
                        RetVal = this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.Search, "" + FilterString + "", FieldSelection.Heavy);
                    }
                }
                else
                {

                    if (this._ShowIUS)
                    {
                        RetVal = this.SourceDBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Heavy, true);
                        RetVal += " Order by I.Indicator_Name";
                    }
                    else
                    {
                        RetVal = this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Heavy);
                        RetVal += " Order by Indicator_Name";
                    }
                }
            }
            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            string SqlQuery = string.Empty;
            DataTable NewTable;
            DataTable TempTable;
            DataTable SectorTable;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;
            FieldSelection fieldSelection = FieldSelection.Light;

            if (this._ShowSector)
            {
                //Step 1: Get sector table with levels
                SectorTable = this.GetSectorTableWLevel(ref table);

                //Setp 2: Set Columns Info accordingly
                this.SetColumnsInfo();

                //Step 3: Create New Data Table to fill the list view
                NewTable = new DataTable();
                NewTable.Columns.Add(IndicatorClassifications.ICNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICParent_NId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Constants.LanguageKeys.Level, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICGlobal, System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(IndicatorClassifications.ICGId);
                NewTable.Columns.Add(IndicatorClassifications.ICInfo);
                NewTable.Columns.Add(IndicatorClassifications.ICName);

                NewTable.Columns.Add(Indicator.IndicatorNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Indicator.IndicatorName);
                NewTable.Columns.Add(Indicator.IndicatorGId);
                NewTable.Columns.Add(Indicator.IndicatorGlobal, System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(Indicator.IndicatorInfo);
                NewTable.Columns.Add(Indicator.HighIsGood, System.Type.GetType("System.Boolean"));

                //add unit and subgroup columns only if show IUS is true.
                if (this._ShowIUS)
                {
                    NewTable.Columns.Add(Unit.UnitNId, System.Type.GetType("System.Int32"));
                    NewTable.Columns.Add(Unit.UnitName);
                    NewTable.Columns.Add(Unit.UnitGId);
                    NewTable.Columns.Add(Unit.UnitGlobal, System.Type.GetType("System.Boolean"));

                    NewTable.Columns.Add(SubgroupVals.SubgroupValNId, System.Type.GetType("System.Int32"));
                    NewTable.Columns.Add(SubgroupVals.SubgroupVal);
                    NewTable.Columns.Add(SubgroupVals.SubgroupValGId);
                    NewTable.Columns.Add(SubgroupVals.SubgroupValGlobal, System.Type.GetType("System.Boolean"));

                    NewTable.Columns.Add(Indicator_Unit_Subgroup.IUSNId, System.Type.GetType("System.Int32"));
                    NewTable.Columns.Add(Indicator_Unit_Subgroup.IsDefaultSubgroup, System.Type.GetType("System.Boolean"));
                }

                //insert columns for IC level information.
                for (Level = 1; Level <= this.MaxLevel; Level++)
                {
                    NewTable.Columns.Add(Constants.LanguageKeys.Level + " " + Level);
                }

                //- Set Filter Selection = "Heavy", because Access Database can support DISTINCT Functions on Memo Fields (in some cases. if test length < 255).
                if (this.SourceDBConnection.ConnectionStringParameters.ServerType == DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess)
                {
                    fieldSelection = FieldSelection.Heavy;
                }

                //insert rows into new table
                foreach (DataRow Row in SectorTable.Rows)//.Select( Constants.LanguageKeys.Level + "=" + this.MaxLevel))
                {
                    //get Indicator or IUS on the basis of IC_ICNid
                    if (this._ShowIUS)
                    {
                        SqlQuery = this.SourceDBQueries.IUS.GetIUSByIC(ICType.Sector, Row[IndicatorClassifications.ICNId].ToString(), fieldSelection, true);
                    }
                    else
                    {
                        SqlQuery = this.SourceDBQueries.Indicators.GetIndicatorByIC(ICType.Sector, Convert.ToString(Row[IndicatorClassifications.ICNId]), fieldSelection);
                    }

                    TempTable = this.SourceDBConnection.ExecuteDataTable(SqlQuery);
                    foreach (DataRow IndicatorRow in TempTable.Rows)
                    {
                        NewRow = NewTable.NewRow();
                        NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                        NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                        NewRow[IndicatorClassifications.ICGlobal] = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                        NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];
                        NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                        NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName].ToString();
                        NewRow[Constants.LanguageKeys.Level] = Row[Constants.LanguageKeys.Level];

                        for (Level = 1; Level < Convert.ToInt32(Row[Constants.LanguageKeys.Level]); Level++)
                        {
                            NewRow[Constants.LanguageKeys.Level + " " + Level] = Row[Constants.LanguageKeys.Level + " " + Level].ToString();
                        }

                        NewRow[Constants.LanguageKeys.Level + " " + Level] = Row[IndicatorClassifications.ICName].ToString();

                        //add indicator, unit and subgroup
                        NewRow[Indicator.IndicatorName] = IndicatorRow[Indicator.IndicatorName];
                        NewRow[Indicator.IndicatorNId] = IndicatorRow[Indicator.IndicatorNId];
                        NewRow[Indicator.IndicatorGId] = IndicatorRow[Indicator.IndicatorGId];
                        NewRow[Indicator.IndicatorGlobal] = IndicatorRow[Indicator.IndicatorGlobal];
                        NewRow[Indicator.HighIsGood] = IndicatorRow[Indicator.HighIsGood];
                        //get indicatorinfo from indicator table
                        DataRow IndicatorInfoRow = this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorRow[Indicator.IndicatorNId].ToString(), FieldSelection.Heavy)).Rows[0];

                        if (TempTable.Columns.Contains(Indicator.IndicatorInfo))
                        {
                            NewRow[Indicator.IndicatorInfo] = IndicatorInfoRow[Indicator.IndicatorInfo];
                        }

                        if (this._ShowIUS)
                        {
                            NewRow[Unit.UnitName] = IndicatorRow[Unit.UnitName];
                            NewRow[Unit.UnitNId] = IndicatorRow[Unit.UnitNId];
                            NewRow[Unit.UnitGId] = IndicatorRow[Unit.UnitGId];
                            NewRow[Unit.UnitGlobal] = IndicatorRow[Unit.UnitGlobal];

                            NewRow[SubgroupVals.SubgroupVal] = IndicatorRow[SubgroupVals.SubgroupVal];
                            NewRow[SubgroupVals.SubgroupValNId] = IndicatorRow[SubgroupVals.SubgroupValNId];
                            NewRow[SubgroupVals.SubgroupValGId] = IndicatorRow[SubgroupVals.SubgroupValGId];
                            NewRow[SubgroupVals.SubgroupValGlobal] = IndicatorRow[SubgroupVals.SubgroupValGlobal];
                            NewRow[Indicator_Unit_Subgroup.IUSNId] = IndicatorRow[Indicator_Unit_Subgroup.IUSNId];
                            NewRow[Indicator_Unit_Subgroup.IsDefaultSubgroup] = IndicatorRow[Indicator_Unit_Subgroup.IsDefaultSubgroup];
                        }


                        NewTable.Rows.Add(NewRow);
                    }
                }
                NewTable.AcceptChanges();

                //remove extra rows from table
                NewTable = this.DeleteExtraRows(NewTable);


                //Step 4: If search string is not empty then filter data table 
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                    NewTable.DefaultView.RowFilter = Indicator.IndicatorName + " LIKE '%" + this.SearchString + "%'";

                    NewTable = NewTable.DefaultView.ToTable();

                }

                //Step 5: set sorting
                NewTable.DefaultView.Sort = this.GetSortingString();

                //Step 6: Replace the referenced table with the new data table
                table = NewTable.DefaultView.ToTable();
            }
        }

        //private void DeleteExtraRows(DataTable table)
        //{
        //    int IndicatorNid = 0;
        //    int UnitNid = 0;
        //    int SubgroupValNid = 0;
        //    int ICNid = 0;
        //    string FilterString = string.Empty;
        //    string LevelColumnName = string.Empty;

        //    List<ExtaRowInfo> ExtraRows = new List<ExtaRowInfo>();
        //    try
        //    {
        //        for (int Level = 1; Level < this.MaxLevel; Level++)
        //        {
        //            DataRow[] Rows = table.Select(Constants.LanguageKeys.Level + "=" + Level);

        //            // Get records where level is equal to 1
        //            foreach (DataRow Row in Rows)
        //            {
        //                ICNid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
        //                IndicatorNid = Convert.ToInt32(Row[Indicator.IndicatorNId]);

        //                //create filterstring
        //                FilterString = string.Empty;
        //                for (int i = 1; i <= Level; i++)
        //                {
        //                    LevelColumnName = Constants.LanguageKeys.Level + " " + i;
        //                    FilterString += "[" + LevelColumnName + "]='" + Row[LevelColumnName].ToString() + "' And ";
        //                }


        //                FilterString += " " + Indicator.IndicatorNId + "=" + IndicatorNid;


        //                if (this._ShowIUS)
        //                {
        //                    UnitNid = Convert.ToInt32(Row[Unit.UnitNId]);
        //                    SubgroupValNid = Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]);

        //                    FilterString += " and " + Unit.UnitNId + "=" + UnitNid;
        //                    FilterString += " and " + SubgroupVals.SubgroupValNId + "=" + SubgroupValNid;
        //                }



        //                //delete row if this is not the lowest IC level for the current IUS
        //                if (table.Select(FilterString).Length > 1)
        //                {
        //                    //ExtraRows.Add(new ExtaRowInfo(ICNid, IndicatorNid, UnitNid, SubgroupValNid));
        //                    Row.Delete();
        //                    table.AcceptChanges();
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(ex.ToString());
        //    }
        //}


        private DataTable DeleteExtraRows(DataTable table)
        {
            DataTable RetVal;
            int IndicatorNid = 0;
            int UnitNid = 0;
            int SubgroupValNid = 0;
            int ICNid = 0;
            string FilterString = string.Empty;
            string LevelColumnName = string.Empty;

            try
            {
                RetVal = table.Clone();
                DataRow NewRow;

                for (int Level = this.MaxLevel; Level > 0; Level--)
                {

                    DataRow[] Rows = table.Select(Constants.LanguageKeys.Level + "=" + Level);

                    // Get records by level
                    foreach (DataRow Row in Rows)
                    {
                        IndicatorNid = Convert.ToInt32(Row[Indicator.IndicatorNId]);


                        LevelColumnName = Constants.LanguageKeys.Level + " " + Level;
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

        #region -- Inner Class --

        internal class ExtaRowInfo
        {
            internal int ICNId;
            internal int IndicatorNid;
            internal int UnitNid;
            internal int SubgroupValNid;
            internal ExtaRowInfo(int icNId, int indicatorNid, int unitNid, int subgroupValNid)
            {
                this.ICNId = ICNId;
                this.IndicatorNid = indicatorNid;
                this.UnitNid = unitNid;
                this.SubgroupValNid = subgroupValNid;
            }

        }

        #endregion

        #endregion


        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        #region "-- Import  --"

        //private int ImportIndicator(DataRow row)
        //{
        //    int RetVal = -1;

        //    IndicatorInfo IndicatorRecord;
        //    IndicatorBuilder IndicatorBuilderObj;

        //    IndicatorRecord = this.GetIndicatorInfo(row);
        //    IndicatorBuilderObj = new IndicatorBuilder(this._TargetDBConnection, this._TargetDBQueries);
        //    RetVal = IndicatorBuilderObj.ImportIndicator(IndicatorRecord, IndicatorRecord.Nid, this.SourceDBQueries, this.SourceDBConnection);

        //    return RetVal;
        //}

        //private int ImportUnit(DataRow row)
        //{
        //    int RetVal = -1;

        //    UnitInfo UnitRecord;
        //    UnitBuilder UnitBuilderObj;

        //    UnitRecord = this.GetUnitInfo(row);

        //    UnitBuilderObj = new UnitBuilder(this._TargetDBConnection, this._TargetDBQueries);
        //    RetVal = UnitBuilderObj.ImportUnit(UnitRecord, UnitRecord.Nid, this.SourceDBQueries, this.SourceDBConnection);

        //    return RetVal;
        //}

        //private int ImportSubgroupVal(DataRow row)
        //{
        //    int RetVal = -1;

        //    DI6SubgroupValBuilder SubgroupValBuilderObj = new DI6SubgroupValBuilder(this._TargetDBConnection, this._TargetDBQueries);

        //    //import into target database
        //    RetVal = SubgroupValBuilderObj.ImportSubgroupVal(Convert.ToInt32(row[SubgroupVals.SubgroupValNId]), this.SourceDBQueries, this.SourceDBConnection);

        //    return RetVal;
        //}

        //private void ImportIUS(List<string> selectedNids, bool allSelected)
        //{
        //    int ProgressBarValue = 1;
        //    int IndicatorNId = -1;
        //    int UnitNId = -1;
        //    int SGValNId = -1;
        //    int MinValue=0;
        //    int MaxValue=0;

        //    DataRow[] Rows;
        //    DataTable TempTable;

        //    List<string> ImportedIndicatorNIDs = new List<string>();
        //    List<string> ImportedUnitNIDs = new List<string>();
        //    List<string> ImportedSGNIDs = new List<string>();

        //    IUSBuilder IusBuilderObj ;

        //    if (this.ShowIUS)
        //    {
        //        // Get selected rows
        //        if (allSelected)
        //        {
        //            Rows = this.SourceTable.Select("1=1");
        //        }
        //        else
        //        {
        //            Rows = this.SourceTable.Select(this.TagValueColumnName + " IN ( " + string.Join(",", selectedNids.ToArray()) + ")");
        //        }

        //        foreach (DataRow Row in Rows)
        //        {
        //            try
        //            {
        //                //import Indicator
        //                IndicatorNId = Convert.ToInt32(Row[Indicator.IndicatorNId]);
        //                if (!ImportedIndicatorNIDs.Contains(IndicatorNId.ToString()))
        //                {
        //                    ImportedIndicatorNIDs.Add(IndicatorNId.ToString());
        //                    IndicatorNId = this.ImportIndicator(Row);
        //                }


        //                    // import unit
        //                UnitNId = Convert.ToInt32(Row[Unit.UnitNId]);
        //                if (!ImportedIndicatorNIDs.Contains(UnitNId.ToString()))
        //                {
        //                   ImportedUnitNIDs.Add(UnitNId.ToString());
        //                    UnitNId =  this.ImportUnit(Row);
        //                }

        //                // import subgroupval
        //                SGValNId=Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]);
        //                if (!ImportedSGNIDs.Contains(SGValNId.ToString()))
        //                {
        //                    ImportedSGNIDs.Add(SGValNId.ToString());
        //                    SGValNId = this.ImportSubgroupVal(Row);
        //                }

        //                //import IUS 
        //                if (IndicatorNId > 0 && UnitNId > 0 && SGValNId > 0)
        //                {
        //                    //if(Convert.ToString(Row[Indicator_Unit_Subgroup.MaxValue]).Length>0)
        //                    //{
        //                    //MaxValue=   Convert.ToInt32(Row[Indicator_Unit_Subgroup.MaxValue]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    MaxValue=0;
        //                    //}

        //                    //if(Convert.ToString(Row[Indicator_Unit_Subgroup.MinValue]).Length>0)
        //                    //{
        //                    //    MinValue= Convert.ToInt32(Row[Indicator_Unit_Subgroup.MinValue]);
        //                    //}
        //                    //else
        //                    //{
        //                    //    MinValue=0;
        //                    //}


        //                    IusBuilderObj = new IUSBuilder( this._TargetDBConnection, this._TargetDBQueries);

        //                    IusBuilderObj.ImportIUS(IndicatorNId, UnitNId, SGValNId,                                                    MinValue, MaxValue , this.SourceDBQueries, this.SourceDBConnection);

        //                }

        //                if (this.ShowSector)
        //                {
        //                    this.ImportSector(Row);
        //                }
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //            this.RaiseIncrementProgessBarEvent(ProgressBarValue);
        //            ProgressBarValue++;
        //        }
        //    }
        //}

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            DataRow Row;
            IndicatorInfo IndicatorRecord = null;
            IndicatorBuilder IndicatorBuilderObj = null;
            DI7MetadataCategoryBuilder MetadataCategoryBuilderObj = null;
            int ProgressBarValue = 1;

            // import indicator metadata categories
            MetadataCategoryBuilderObj = new DI7MetadataCategoryBuilder(this._TargetDBConnection, this._TargetDBQueries);
            MetadataCategoryBuilderObj.ImportAllMetadataCategories(this.SourceDBConnection, this.SourceDBQueries, MetadataElementType.Indicator);

            // import IUS or indicator
            foreach (string Nid in selectedNids)
            {
                try
                {
                    Row = this.SourceTable.Select(this.TagValueColumnName + "=" + Nid)[0];

                    if (this._ShowIUS)
                    {
                        this.ImportForIUS(Row);
                    }
                    else
                    {

                        //import indicator 
                        IndicatorRecord = this.GetIndicatorInfo(Row);
                        IndicatorBuilderObj = new IndicatorBuilder(this._TargetDBConnection, this._TargetDBQueries);
                        IndicatorBuilderObj.ImportIndicator(IndicatorRecord, Convert.ToInt32(Nid), this.SourceDBQueries, this.SourceDBConnection);

                        if (this._ShowSector)
                        {
                            this.ImportSector(Row);
                        }

                    }

                }
                catch (Exception)
                {

                    throw;
                }
                this.RaiseIncrementProgessBarEvent(ProgressBarValue);
                ProgressBarValue++;
            }


            if (this._ShowIUS && this._ShowSector)
            {
                this.ImportSectors(selectedNids, allSelected);
            }
        }





        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        private void ImportSectors(List<string> selectedNids, bool allSelected)
        {
            int ProgressBarValue = 0;
            IndicatorClassificationInfo SrcClassification;
            DataRow Row;
            List<string> ImportedNids = new List<string>();

            foreach (string Nid in selectedNids)
            {
                try
                {

                    //get ic from source table
                    Row = this.SourceTable.Select(this.TagValueColumnName + "=" + Nid)[0];

                    if (!ImportedNids.Contains(Row[IndicatorClassifications.ICNId].ToString()))
                    {
                        SrcClassification = new IndicatorClassificationInfo();
                        SrcClassification.Name = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICName].ToString());
                        SrcClassification.GID = Row[IndicatorClassifications.ICGId].ToString();
                        SrcClassification.IsGlobal = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                        SrcClassification.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                        if (!Information.IsDBNull(Row[IndicatorClassifications.ICInfo]))
                        {
                            SrcClassification.ClassificationInfo = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICInfo].ToString());
                        }

                        SrcClassification.Parent = new IndicatorClassificationInfo();
                        SrcClassification.Parent.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]);
                        SrcClassification.Type = this._IndicatorClassificationType;

                        //import into target database
                        Utility.CreateClassificationChainFromExtDB(
                            SrcClassification.Nid,
                            SrcClassification.Parent.Nid,
                            SrcClassification.GID,
                            SrcClassification.Name,
                            SrcClassification.Type,
                            SrcClassification.ClassificationInfo,
                            SrcClassification.IsGlobal,
                            this.SourceDBQueries, this.SourceDBConnection, this._TargetDBQueries, this._TargetDBConnection);

                        ImportedNids.Add(Row[IndicatorClassifications.ICNId].ToString());
                    }
                    this.RaiseIncrementProgessBarEvent(ProgressBarValue);
                    ProgressBarValue++;
                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }

            }
        }



        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();
            if (this._ShowSector)
            {
                for (int Level = 1; Level <= this.MaxLevel; Level++)
                {
                    this.Columns.Add(Constants.LanguageKeys.Level + " " + Level, DILanguage.GetLanguageString(Constants.LanguageKeys.Level) + " " + Level);
                }
            }

            if (this._ShowIUS)
            {
                this.Columns.Add(Indicator.IndicatorName, DILanguage.GetLanguageString(Constants.LanguageKeys.Indicator));
                this.Columns.Add(Unit.UnitName, DILanguage.GetLanguageString(Constants.LanguageKeys.Unit));
                this.Columns.Add(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString(Constants.LanguageKeys.Subgroup));
                this.TagValueColumnName = Indicator_Unit_Subgroup.IUSNId;
            }
            else
            {
                this.Columns.Add(Indicator.IndicatorName, DILanguage.GetLanguageString(Constants.LanguageKeys.Indicator));
                this.TagValueColumnName = Indicator.IndicatorNId;
            }
            this.GlobalValueColumnName1 = Indicator.IndicatorGlobal;
            this.GlobalValueColumnName2 = Unit.UnitGlobal;
            this.GlobalValueColumnName3 = SubgroupVals.SubgroupValGlobal;
        }

        #endregion

        #endregion

        #endregion

    }
}
