using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;


namespace DevInfo.Lib.DI_LibBAL.Export
{
    internal class ExportDatabase
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string MARK_COLUMN = "Mark";

        //-- Varibles to store NIDs from userSelections.
        string IndicatorNIDs = string.Empty;
        string IUSNIDs = string.Empty;
        string UnitNIDs = string.Empty;
        string SubgroupValNIDs = string.Empty;
        string AreaNIDs = string.Empty;
        string TimePeriodNIDs = string.Empty;
        string SourceNIDs = string.Empty;
        string ICNIDs = string.Empty;
        private bool ApplyMRD = false;
        private DataViewFilters DataViewFilters = null;

        //-- Souce Database DIConnection object
        DIConnection SourceDBConnection = null;
        DIConnection DestDBConnection = null;
        DIQueries DestDBQueries = null;
        DITables DBTableNames = null;
        System.Data.DataTable LanguageTable = null;
        #endregion

        #region "-- Methods --"
        private void DeleteFromIUSTable()
        {
            this.AddTempColumn(this.DestDBQueries.TablesName.IndicatorUnitSubgroup);
            ////-- Set Mark true IndicatorNids
            //this.MarkRecords(this.DestDBQueries.TablesName.Indicator, Indicator.IndicatorNId, this.IndicatorNIDs);
            ////-- Set Mark true Units
            //this.MarkRecords(this.DestDBQueries.TablesName.Unit, Unit.UnitNId, this.UnitNIDs);
            ////-- Set Mark true Subgroupvals
            //this.MarkRecords(this.DestDBQueries.TablesName.SubgroupVals, SubgroupVals.SubgroupValNId, this.SubgroupValNIDs);
            ////-- Set Mark true IUSNIDs

            this.MarkRecords(this.DestDBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.IUSNId, this.IUSNIDs);
            //-- Delete unmarkded record from IUSTable
            this.DeleteMarkedRecords(this.DestDBQueries.TablesName.IndicatorUnitSubgroup);
            //-- Remove mark column
            this.RemoveMarkColumns(this.DestDBQueries.TablesName.IndicatorUnitSubgroup);
        }

        private void DeleteFromICTables()
        {
           // this.DeleteBulkRecords(this.DBTableNames.IndicatorClassifications, IndicatorClassifications.ICNId, this.ICNIDs, true, " AND " + IndicatorClassifications.ICType + " <> " + DIQueries.ICTypeText[ICType.Source]);

            this.DeleteBulkRecords(this.DBTableNames.IndicatorClassifications, IndicatorClassifications.ICNId, this.ICNIDs, true);

        }

        private void DeleteFromICIUSTable()
        {
            //***********Table:  UT_IndicatorClassification_IUS *************

            this.DeleteBulkRecords(this.DBTableNames.IndicatorClassificationsIUS, IndicatorClassificationsIUS.ICNId, this.ICNIDs, false, string.Empty);

            this.DeleteBulkRecords(this.DBTableNames.IndicatorClassificationsIUS, IndicatorClassificationsIUS.IUSNId, this.IUSNIDs, false, string.Empty);

        }

        private void DeleteSourcesFromICTable()
        {
            DataTable SourceTable = null;
            string TempICNIds = string.Empty;
            string LangCode = string.Empty;
            string strQuery = string.Empty;

            //- Get sources only if sourceNIds are blank
            if (string.IsNullOrEmpty(this.SourceNIDs))
            {
                //-- Get Auto select source (Sources left in UT_Data table)
                SourceTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Source.GetAutoDistinctSources());

                this.SourceNIDs = Common.GetCommaSeperatedString(SourceTable, Data.SourceNId);
            }
            //- get Source parents
            this.SourceNIDs = this.GetICParentNIds(this.SourceNIDs);

            //- Delete form IC table (apply Filter of sources 'SR')
            foreach (System.Data.DataRow row in LanguageTable.Rows)
            {
                LangCode = row[Language.LanguageCode].ToString();

                this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

                //-- Delete IndicatorClassification from UT_IndicatorClassification_en 
                strQuery = "Delete from " + this.DBTableNames.IndicatorClassifications + " WHERE " + IndicatorClassifications.ICType + " = 'SR'";

                if (string.IsNullOrEmpty(this.SourceNIDs) == false)
                {
                    strQuery += " AND " + IndicatorClassifications.ICNId + " NOT IN (" + this.SourceNIDs + ")";
                }

                this.DestDBConnection.ExecuteNonQuery(strQuery);
            }

            //-- Delete from IC_IUS table
            strQuery = "Delete from " + this.DBTableNames.IndicatorClassificationsIUS + " As IC_IUS WHERE NOT EXISTS (Select * from " + this.DBTableNames.IndicatorClassifications + " As IC where IC." + IndicatorClassificationsIUS.ICNId + " = IC_IUS." + IndicatorClassifications.ICNId + ")";

            this.DestDBConnection.ExecuteNonQuery(strQuery);

        }

        private void DeleteFromAreaTables()
        {
            string LangCode = string.Empty;
            string Query = string.Empty;
            string FilterNIDs = string.Empty;
            string AllAreaNIds = string.Empty;

            if (!String.IsNullOrEmpty(this.AreaNIDs))
            {
                //foreach (System.Data.DataRow row in this.LanguageTable.Rows)
                //{
                //    LangCode = row[Language.LanguageCode].ToString();

                this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), this.DestDBQueries.LanguageCode.Trim('_'));

                // DO NOT delete parent Areas
                // Get Parent NIDs and merge into given AreaNIds
                AllAreaNIds = this.GetAreaParentsNIds(this.AreaNIDs);


                //--Delete from UT_Area_en where AreaNIDs NOT in (given AreaNids)
                //this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Area, Area.AreaNId, AllAreaNIds, true), AllAreaNIds);
                this.DeleteBulkRecords(this.DBTableNames.Area, Area.AreaNId, AllAreaNIds, true);


                //--Delete from UT_Area_Map where AreaNIDs NOT IN (given AreaNids)
                //this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.AreaMap, Area_Map.AreaNId, AllAreaNIds, true), AllAreaNIds);
                this.DeleteBulkRecords(this.DBTableNames.AreaMap, Area_Map.AreaNId, AllAreaNIds, false);

                //--Delete from UT_Area_level_en where Area_Level NOT IN (Select Area_Level from UT_Area_en )
                FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaLevel(AllAreaNIds)), Area_Level.AreaLevel);
                Query = DIQueries.DeleteRecords(this.DBTableNames.AreaLevel, Area_Level.AreaLevel, FilterNIDs, true);
                this.ExecuteDelete(Query, FilterNIDs);

                // Get Layer_NID from UT_Area_Map for given AreaNIds
                System.Data.DataTable TempTable = this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, AllAreaNIds, FieldSelection.NId));
                FilterNIDs = Common.GetCommaSeperatedString(TempTable, Area_Map_Layer.LayerNId);
                //--Delete from UT_Area_Map_Layer_en where Layer_NID NOT IN (Select Layer_NID from UT_Area_Map )
                //Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Map_Layer.LayerNId, FilterNIDs, true);
                //this.ExecuteDelete(Query, FilterNIDs);
                this.DeleteBulkRecords(this.DBTableNames.AreaMapLayer, Area_Map_Layer.LayerNId, FilterNIDs, false);

                // Get MetaDataNIDs for given Layer_NIDs
                FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaMapMetadata(FilterNIDs)), Area_Map_Metadata.MetadataNId);
                // Delete from UT_Area_Map_Metadata_en where Metadata_NID NOT IN (given MetaDataNIDs)
                //Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapMetadata, Area_Map_Metadata.MetadataNId, FilterNIDs, true);
                //this.ExecuteDelete(Query, FilterNIDs);
                this.DeleteBulkRecords(this.DBTableNames.AreaMapMetadata, Area_Map_Metadata.MetadataNId, FilterNIDs, true);

                //Get Feature_Type_NID for given AreaNIds
                if (!String.IsNullOrEmpty(this.AreaNIDs))
                {
                    FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaFeatureByAreaNid(AllAreaNIds)), Area_Feature_Type.FeatureTypeNId);
                    //--Delete from UT_Area_Feature_Type_en where Feature_Type_NId NOT in (given Feature_Type_NID)
                    // Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Feature_Type.FeatureTypeNId, FilterNIDs, true);
                    //this.ExecuteDelete(Query, FilterNIDs);
                    this.DeleteBulkRecords(this.DBTableNames.AreaFeatureType, Area_Feature_Type.FeatureTypeNId, FilterNIDs, true);
                }
                //}

                //--Delete from UT_Data where AreaNIDs NOT in (given AreaNids)
                // this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Data, Area.AreaNId, this.AreaNIDs, true), this.AreaNIDs);
                this.DeleteBulkRecords(this.DBTableNames.Data, Area.AreaNId, this.AreaNIDs, false);
            }
        }

        //private void AutoDeleteAreas()
        //{
        //    //--Deletes all Areas from Area tables, in case when no AreaNId is present in UT_Data table.
        //    DataTable AreaTable = null;
        //    string LangCode = string.Empty;
        //    string Query = string.Empty;
        //    string FilterNIDs = string.Empty;
        //    string NIdsLeft = string.Empty;
        //    string AllAreaNIds = string.Empty;

        //    // Get Area left in data Tables
        //    AreaTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.AutoSelectArea(string.Empty, string.Empty, string.Empty, string.Empty));
        //    NIdsLeft = Utility.GetCommaSeperatedString(AreaTable, Data.AreaNId);

        //    if (String.IsNullOrEmpty(NIdsLeft))
        //    {

        //        this.AreaNIDs = Utility.MergeNIds(NIdsLeft, this.AreaNIDs);

        //        //-- if no AreaNID left in DataTable, then delete all Areas.
        //        if (String.IsNullOrEmpty(this.AreaNIDs))
        //        {
        //            foreach (System.Data.DataRow row in this.LanguageTable.Rows)
        //            {
        //                LangCode = row[Language.LanguageCode].ToString();

        //                this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

        //                //--Delete from UT_Area_en where AreaNid NOT Exists in UT_data
        //                this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Area, Area.AreaNId, AllAreaNIds, true), AllAreaNIds);

        //                //--Delete from UT_Area_Map where AreaNIDs NOT IN (given AreaNids)
        //                this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.AreaMap, Area_Map.AreaNId, AllAreaNIds, true), AllAreaNIds);


        //                //--Delete from UT_Area_level_en where Area_Level NOT IN (Select Area_Level from UT_Area_en )
        //                FilterNIDs = Utility.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaLevel(AllAreaNIds)), Area_Level.AreaLevel);
        //                Query = DIQueries.DeleteRecords(this.DBTableNames.AreaLevel, Area_Level.AreaLevel, FilterNIDs, true);
        //                this.ExecuteDelete(Query, FilterNIDs);

        //                // Get Layer_NID from UT_Area_Map for given AreaNIds
        //                System.Data.DataTable TempTable = this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, AllAreaNIds, FieldSelection.NId));
        //                FilterNIDs = Utility.GetCommaSeperatedString(TempTable, Area_Map_Layer.LayerNId);
        //                //--Delete from UT_Area_Map_Layer_en where Layer_NID NOT IN (Select Layer_NID from UT_Area_Map )
        //                Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Map_Layer.LayerNId, FilterNIDs, true);
        //                this.ExecuteDelete(Query, FilterNIDs);

        //                // Get MetaDataNIDs for given Layer_NIDs
        //                FilterNIDs = Utility.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaMapMetadata(FilterNIDs)), Area_Map_Metadata.MetadataNId);
        //                // Delete from UT_Area_Map_Metadata_en where Metadata_NID NOT IN (given MetaDataNIDs)
        //                Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapMetadata, Area_Map_Metadata.MetadataNId, FilterNIDs, true);
        //                this.ExecuteDelete(Query, FilterNIDs);

        //                //Get Feature_Type_NID for given AreaNIds
        //                if (String.IsNullOrEmpty(this.AreaNIDs) == false)
        //                {
        //                    FilterNIDs = Utility.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaFeatureByAreaNid(AllAreaNIds)), Area_Feature_Type.FeatureTypeNId);
        //                    //--Delete from UT_Area_Feature_Type_en where Feature_Type_NId NOT in (given Feature_Type_NID)
        //                    Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Feature_Type.FeatureTypeNId, FilterNIDs, true);
        //                    this.ExecuteDelete(Query, FilterNIDs);
        //                }
        //            }

        //            //--Delete from UT_Data where AreaNIDs NOT in (given AreaNids)
        //            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Data, Area.AreaNId, this.AreaNIDs, true), this.AreaNIDs);

        //        }
        //    }
        //    else
        //    {
        //        this.AreaNIDs = NIdsLeft;
        //        this.DeleteFromAreaTables();
        //    }

        //}

        private void DeleteFromAreaByAreaLevel()
        {
            string AreaNIdsTemp = this.AreaNIDs;
            DataTable AreaTable = null;
            string AreaLevels = string.Empty;
            if (this.AreaLevel != -1)
            {
                for (int i = 1; i <= this.AreaLevel; i++)
                {
                    if (!string.IsNullOrEmpty(AreaLevels))
                    {
                        AreaLevels += ",";
                    }

                    AreaLevels += i.ToString();
                }

                //-- get AreaNID on the baslis of Arealevel
                //AreaTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaByAreaLevel(this.AreaLevel.ToString()));
                AreaTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaByAreaLevel(AreaLevels));

                this.AreaNIDs = Common.GetCommaSeperatedString(AreaTable, Area.AreaNId);

                //-- delete from Area Table on the basis of AreaNids
                this.DeleteFromAreaTables();

                //-- Set back original AreaNId

                this.AreaNIDs = AreaNIdsTemp;
            }
        }

        private void DeleteFromDataTable()
        {
            string Query = string.Empty;

            //-- Delete from Data Table WHERE IndicatorNId NOT in (given IndicatorNId)
            if (!string.IsNullOrEmpty(this.IndicatorNIDs))
            {
                //-- Delete records where IndicatorNId NOT in (given Indicator NID)
                //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IndicatorNId, this.IndicatorNIDs, true);
                //this.DestDBConnection.ExecuteNonQuery(Query);
                this.DeleteBulkRecords(DBTableNames.Data, Data.IndicatorNId, this.IndicatorNIDs, false);
            }

            //-- Delete from Data Table WHERE UnitNId NOT in (given UnitNId)
            if (!string.IsNullOrEmpty(this.UnitNIDs))
            {
                //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.UnitNId, this.UnitNIDs, true);
                //this.DestDBConnection.ExecuteNonQuery(Query);
                this.DeleteBulkRecords(DBTableNames.Data, Data.UnitNId, this.UnitNIDs, false);
            }

            //-- Delete from Data Table WHERE IndicatorNId NOT in (given IndicatorNId)
            if (!string.IsNullOrEmpty(this.SubgroupValNIDs))
            {
                //-- Delete records where SubgroupValNIDs NOT in (given SubgroupValNIDs )
                //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SubgroupValNId, this.SubgroupValNIDs, true);
                //this.DestDBConnection.ExecuteNonQuery(Query);
                this.DeleteBulkRecords(DBTableNames.Data, Data.SubgroupValNId, this.SubgroupValNIDs, false);
            }


            if (!string.IsNullOrEmpty(this.IUSNIDs))
            {
                //-- Delete records where IUSNid NOT in (IUSNId left)
                //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IUSNId, this.IUSNIDs, true);
                //this.DestDBConnection.ExecuteNonQuery(Query);
                this.DeleteBulkRecords(DBTableNames.Data, Data.IUSNId, this.IUSNIDs, false);
            }



            //-- Delete records where Data NOT EXISTS for given IC_NID .
            if (string.IsNullOrEmpty(this.ICNIDs) == false)
            {
                Query = "DELETE FROM " + DBTableNames.Data + " AS D " +
                    " WHERE NOT EXISTS (Select * from " + DBTableNames.IndicatorUnitSubgroup + " AS IUS " +
                    " WHERE EXISTS (Select * from " + DBTableNames.IndicatorClassificationsIUS + " as IC_IUS " +
                    " WHERE EXISTS (Select * from " + DBTableNames.IndicatorClassifications + " AS IC " +
                    " WHERE IC_NID in (" + this.ICNIDs + ") AND IC." + IndicatorClassifications.ICNId + " = IC_IUS." + IndicatorClassificationsIUS.ICNId +
                    ") AND IC_IUS." + IndicatorClassificationsIUS.IUSNId + "= IUS." + Indicator_Unit_Subgroup.IUSNId +
                    ") AND IUS." + Indicator_Unit_Subgroup.IUSNId + "= D." + Data.IUSNId + ")";
                this.DestDBConnection.ExecuteNonQuery(Query);
            }

            //--Delete records where AreaNid NOT in (given AreaNid)
            if (!string.IsNullOrEmpty(this.AreaNIDs))
            {
                //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.AreaNId, this.AreaNIDs, true);
                //this.ExecuteDelete(Query, AreaNIDs);
                this.DeleteBulkRecords(DBTableNames.Data, Data.AreaNId, this.AreaNIDs, false);
            }


            //--Delete records where TimePeriodNid NOT in (given TimePeriodNid)
            if (!string.IsNullOrEmpty(this.TimePeriodNIDs))
            {

                Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.TimePeriodNId, this.TimePeriodNIDs, true);
                this.ExecuteDelete(Query, TimePeriodNIDs);
            }

            //--Delete records where SourceNid NOT in (given SourceNId)
            if (!string.IsNullOrEmpty(this.SourceNIDs))
            {
                Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SourceNId, this.SourceNIDs, true);
                this.ExecuteDelete(Query, SourceNIDs);
            }

            //- Delete Records if DataViewfilters are present. 
            this.DeleteFromDataByDataFilters();
        }

        #region "DataView Filters"

        private void DeleteFromDataByDataFilters()
        {
            string Query = string.Empty;
            try
            {

                if (this.DataViewFilters != null)
                {
                    //- Delete from UT_data for following filters

                    // MRD Filter
                    if (this.DataViewFilters.MostRecentData)
                    {
                        this.DeleteFromDataTableByMRD();
                    }

                    //- IndicatorNId filter
                    if (string.IsNullOrEmpty(this.DataViewFilters.DeletedSourceNIds) == false)
                    {
                        //- If ShowSourceByIUS is true, 
                        // then NIds are in format:- IUSNId_SourceNId
                        if (this.DataViewFilters.ShowSourceByIUS)
                        {
                            string[] IUSNId_SourceNId = Common.GetDelimtedNIdsArray(this.DataViewFilters.DeletedSourceNIds, '_');

                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IUSNId, IUSNId_SourceNId[0], false);
                            Query += " AND " + Data.SourceNId + " IN (" + IUSNId_SourceNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- SourceNId
                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SourceNId, this.DataViewFilters.DeletedSourceNIds, false);
                        }

                        this.ExecuteDelete(Query, this.DataViewFilters.DeletedSourceNIds);
                    }

                    //- Unit filter
                    if (string.IsNullOrEmpty(this.DataViewFilters.DeletedUnitNIds) == false)
                    {
                        //- If ShowUnitByIndicator is true, 
                        // then NIds are in format:- IndNId_UnitNId
                        if (this.DataViewFilters.ShowUnitByIndicator)
                        {
                            string[] IndNId_UnitNId = Common.GetDelimtedNIdsArray(this.DataViewFilters.DeletedUnitNIds, '_');

                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IndicatorNId, IndNId_UnitNId[0], false);
                            Query += " AND " + Data.UnitNId + " IN (" + IndNId_UnitNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- UnitNId
                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.UnitNId, this.DataViewFilters.DeletedUnitNIds, false);
                        }

                        this.ExecuteDelete(Query, this.DataViewFilters.DeletedUnitNIds);
                    }

                    //- Subgroup Filter
                    if (string.IsNullOrEmpty(this.DataViewFilters.DeletedSubgroupNIds) == false)
                    {
                        //- If ShowSubgroupByIndicator is true, 
                        // then NIds are in format:- IndNId_SubgroupNId
                        if (this.DataViewFilters.ShowSubgroupByIndicator)
                        {
                            string[] IndNId_SubgroupNId = Common.GetDelimtedNIdsArray(this.DataViewFilters.DeletedSubgroupNIds, '_');

                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IndicatorNId, IndNId_SubgroupNId[0], false);
                            Query += " AND " + Data.SubgroupValNId + " IN (" + IndNId_SubgroupNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- SubgroupNIds
                            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SubgroupValNId, this.DataViewFilters.DeletedSubgroupNIds, false);
                        }

                        this.ExecuteDelete(Query, this.DataViewFilters.DeletedSubgroupNIds);
                    }

                    //- IUS-DataValue Filter
                    if (this.DataViewFilters.IndicatorDataValueFilters != null && this.DataViewFilters.IndicatorDataValueFilters.Count > 0)
                    {
                        string IndNId = this.DataViewFilters.IndicatorDataValueFilters.SQL_GetIndicatorDataValueFilters(this.SourceDBConnection.ConnectionStringParameters.ServerType);

                        if (string.IsNullOrEmpty(IndNId) == false)
                        {
                            Query = "DELETE FROM " + this.DBTableNames.Data + " AS D2 WHERE NOT EXISTS " +
                                "(Select * from " + this.DBTableNames.Data + " AS D WHERE D." + Data.DataNId + "=D2." + Data.DataNId + " " + IndNId + ")";

                            this.ExecuteDelete(Query, IndNId);
                        }
                    }

                    // DataView Range Filter
                    if (this.DataViewFilters.DataValueFilter != null && this.DataViewFilters.DataValueFilter.OpertorType != OpertorType.None)
                    {
                        //- Set Delete Query
                        Query = DIQueries.DeleteRecords(DBTableNames.Data, string.Empty, string.Empty, false);

                        Query = Query + Common.GetDataValueRangeFilderString(this.DataViewFilters.DataValueFilter);

                        this.ExecuteDelete(Query, "-1");
                    }
                }
            }
            catch
            {
            }
        }

        private void DeleteFromDataTableByMRD()
        {
            //- Delete Records filtering Most Recent Data.

            //- Get Records left in UT_data
            DataTable DTData = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Data.GetDataByIUSTimePeriodAreaSource(string.Empty, string.Empty, string.Empty, string.Empty, FieldSelection.Heavy));

            string sAreaNId = string.Empty;
            string sIUSNId = string.Empty;
            StringBuilder sbMRDDataNIDs = new StringBuilder();

            // Most Recent Data
            //sort dataview in decending order of timeperiod so that latest record can be obtained
            DTData.DefaultView.Sort = Indicator_Unit_Subgroup.IUSNId + "," + Area.AreaNId + "," + Timeperiods.TimePeriod + " DESC";
            foreach (DataRow DRowParentTable in DTData.DefaultView.ToTable().Rows)
            {
                // Get the record for latest timeperiod.
                if (sAreaNId != DRowParentTable[Area.AreaNId].ToString() || sIUSNId != DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString())
                {
                    sAreaNId = DRowParentTable[Area.AreaNId].ToString();
                    sIUSNId = DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString();
                    sbMRDDataNIDs.Append("," + DRowParentTable[Data.DataNId].ToString());
                }
            }
            if (sbMRDDataNIDs.Length > 0)
            {
                string sMRDDataNIDs = sbMRDDataNIDs.ToString().Substring(1);

                //- Delete Records from Data Where DataNIDs NOT IN (MRD_DataNIds)
                this.DestDBConnection.ExecuteNonQuery(DIQueries.DeleteRecords(this.DestDBQueries.TablesName.Data, Data.DataNId, sMRDDataNIDs, true));
            }
        }

        #endregion

        private void DeleteFromIndicatorTable()
        {
            this.DeleteBulkRecords(this.DestDBQueries.TablesName.Indicator, Indicator.IndicatorNId, this.IndicatorNIDs, true, string.Empty);
        }

        private void DeleteFromUnitTable()
        {
            this.DeleteBulkRecords(this.DestDBQueries.TablesName.Unit, Unit.UnitNId, this.UnitNIDs, true, string.Empty);
        }

        private void DeleteFromSubgroupTables()
        {
            this.DeleteBulkRecords(this.DestDBQueries.TablesName.SubgroupVals, SubgroupVals.SubgroupValNId, this.SubgroupValNIDs, true, string.Empty);
        }

        private void DeleteFromFootNoteTable()
        {
            string Query = string.Empty;
            string FilterNIDs = string.Empty;

            //******************** Tables: FootNotes ******************** 
            string LangCode = string.Empty;
            foreach (System.Data.DataRow row in this.LanguageTable.Rows)
            {
                LangCode = row[Language.LanguageCode].ToString();
                this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

                // --Get FootNoteNIDs NOT in UT_Data
                //TODO move in DAL
                Query = "Select F.Footnote_nid from " + this.DBTableNames.FootNote + " AS F left Join " + this.DBTableNames.Data + " AS D ON " +
                 " F.footnote_nid  = D.FootNote_NID " +
                 " where D.Data_NId is NULL";

                FilterNIDs = Common.GetCommaSeperatedString(DestDBConnection.ExecuteDataTable(Query), Data.FootNoteNId);
                //FilterNIDs = Utility.GetCommaSeperatedString(DestDBConnection.ExecuteDataTable(DestDBQueries.Footnote. GetFootnoteFromDataNId("")), Data.FootNoteNId);

                // Delete footnotes where NID in (footNote NOT required)
                Query = DIQueries.DeleteRecords(DBTableNames.FootNote, FootNotes.FootNoteNId, FilterNIDs, false);
                this.ExecuteDelete(Query, FilterNIDs);
            }
        }

        private void DeleteFromTimePeriodTable()
        {
            string Query = string.Empty;
            string FilterNIDs = string.Empty;
            //*********** TimePeriod ********************

            //--Delete records where TimePeriodNid NOT in (given TimePeriodNid)
            Query = DIQueries.DeleteRecords(this.DBTableNames.TimePeriod, Data.TimePeriodNId, this.TimePeriodNIDs, true);
            this.ExecuteDelete(Query, this.TimePeriodNIDs);

            // Get TimePeriodNid left in UT_Data
            FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Timeperiod.GetAutoSelectTimeperiod("", false, "", "")), Timeperiods.TimePeriodNId);

            // Delete Timeperiod where not in (left TimePeriodNid)
            Query = DIQueries.DeleteRecords(this.DBTableNames.TimePeriod, Data.TimePeriodNId, FilterNIDs, true);
            this.DestDBConnection.ExecuteNonQuery(Query);

        }

        private void DeleteFromNotesTables()
        {
            string FilterNIDs = string.Empty;
            DataTable TempTable = null;
            string NotesNIdsToDelete = string.Empty;
            string LangCode = string.Empty;
            string Query = string.Empty;

            // Get DataNIds, Notes_Nid which are NOT in UT_Notes_Data
            // TODO Add query in DAL 
            Query = "Select N.Data_NId, N.Notes_NID from " + this.DBTableNames.NotesData + " AS N left Join " + this.DBTableNames.Data + " AS D ON " +
            " N.Data_NId  = D.Data_NID " +
            " where D.Data_NId IS NULL";

            TempTable = this.DestDBConnection.ExecuteDataTable(Query);

            //-- Get Unwanted Data_NID that are NOT required in Database.
            FilterNIDs = Common.GetCommaSeperatedString(TempTable, Notes_Data.DataNId);

            //-- Get Unwanted NotesNId from UT_Notes_Data
            NotesNIdsToDelete = Common.GetCommaSeperatedString(TempTable, Notes_Data.NotesNId);

            // Delete from UT_Notes_Data where Data_Nid in (unwanted Data_NID)
            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.NotesData, Notes_Data.DataNId, FilterNIDs, false), FilterNIDs);

            foreach (System.Data.DataRow row in LanguageTable.Rows)
            {
                LangCode = row[Language.LanguageCode].ToString();

                this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

                //if (string.IsNullOrEmpty(FilterNIDs) == false)
                //{
                //    // Get Notes_Nid left in UT_Notes_Data
                //    FilterNIDs = Utility.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Notes.GetDistinctNotesNidFromNotesData(FilterNIDs)), Notes_Data.NotesNId);
                //}

                //- Delete from UT_Notes_en where Notes_NID in (unwanted NotesNIDs)
                this.DestDBConnection.ExecuteNonQuery(DIQueries.DeleteRecords(this.DBTableNames.Notes, Notes.NotesNId, NotesNIdsToDelete, false));
            }

            //-- Get ProfileNID left in UT_Notes_en
            FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable("Select " + Notes.ProfileNId + " from " + this.DBTableNames.Notes), Notes.ProfileNId);

            //-Delete from UT_Notes_Profile where ProfileNID NOT IN (ProfileNID in UT_Notes_Data)
            this.DestDBConnection.ExecuteNonQuery(DIQueries.DeleteRecords(this.DBTableNames.NotesProfile, Notes_Profile.ProfileNId, FilterNIDs, true));
        }

        private void DeleteFromAssistantTables()
        {
            string FilterNIDs = string.Empty;

            if (string.IsNullOrEmpty(this.IndicatorNIDs) == false)
            {
                // Get IndicatorGIDs left in UT_Indicator
                System.Data.DataTable TempTable = this.DestDBConnection.ExecuteDataTable(DestDBQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorNIDs, FieldSelection.Light));

                FilterNIDs = Common.GetCommaSeperatedString(TempTable, Indicator.IndicatorGId);

                // Delete Assistant records on the basis of remaining IndicatorGID.
                this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.AssistantTopic, Assistant_Topic.IndicatorGId, FilterNIDs, true), FilterNIDs);
            }
        }

        private void DeleteFromXSLTTables()
        {
            string FilterNIDs = string.Empty;
            string LangCode = string.Empty;
            string AllAreaNIds = string.Empty;

            this.AddTempColumn(this.DBTableNames.ElementXSLT);

            //-- Set Mark true IndicatorNids
            this.MarkRecords(this.DBTableNames.ElementXSLT, EelementXSLT.ElementNId, this.IndicatorNIDs, " AND " + EelementXSLT.ElementType + " = 'I'");


            // Get Parent NIDs and merge into given AreaNIds
            AllAreaNIds = this.GetAreaParentsNIds(this.AreaNIDs);
            //-- Set Mark true Units
            this.MarkRecords(this.DBTableNames.ElementXSLT, EelementXSLT.ElementNId, AllAreaNIds, " AND " + EelementXSLT.ElementType
                + " = 'A'");

            //-- Set Mark true Subgroupvals
            this.MarkRecords(this.DBTableNames.ElementXSLT, EelementXSLT.ElementNId, this.SourceNIDs, " AND " + EelementXSLT.ElementType + " = 'S'");

            //-- Delete unmarkded record from IUSTable
            this.DeleteMarkedRecords(this.DBTableNames.ElementXSLT);
            //-- Remove mark column
            this.RemoveMarkColumns(this.DBTableNames.ElementXSLT);

            //- Delete from UT_Element_XSLT where Element_ID NOT in (given Indicator_NId)           
            // this.DeleteBulkRecords(this.DBTableNames.ElementXSLT, EelementXSLT.ElementNId, this.IndicatorNIDs, false," AND " + EelementXSLT.ElementType + " = 'I'");

            // // Get Parent NIDs and merge into given AreaNIds
            // AllAreaNIds = this.GetAreaParentsNIds(this.AreaNIDs);

            // //- Delete from UT_Element_XSLT where Element_ID NOT in (given Area_NId) 
            //// this.ExecuteDelete("Delete from " + this.DBTableNames.ElementXSLT + " where " + EelementXSLT.ElementNId + " NOT IN (" + AllAreaNIds + ") AND " + EelementXSLT.ElementType + " = 'A'", AllAreaNIds);

            // this.DeleteBulkRecords(this.DBTableNames.ElementXSLT, EelementXSLT.ElementNId, AllAreaNIds, false, " AND " + EelementXSLT.ElementType 
            //     + " = 'A'");

            // //- Delete from UT_Element_XSLT where Element_ID NOT in (given .Source_NIds)
            // this.ExecuteDelete("Delete from " + this.DBTableNames.ElementXSLT + " where " + EelementXSLT.ElementNId + " NOT IN (" + this.SourceNIDs + ") AND " + EelementXSLT.ElementType + " = 'S'", this.SourceNIDs);          

            //-- Get XSLT NIDs left in UT_Element_XSLT
            FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Xslt.GetXSLT(FilterFieldType.None, "")), XSLT.XSLTNId);

            //- Delete from UT_XSLT where XSLT_Nid NOT In UT_Element_XSLT.
            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.XSLT, XSLT.XSLTNId, FilterNIDs, true), FilterNIDs);

        }

        /// <summary>
        /// Get all IC parent NIds fro given icNIds
        /// </summary>
        /// <param name="icNIds"></param>
        private string GetICParentNIds(string icNIds)
        {
            string RetVal = string.Empty;
            string Query = string.Empty;
            string ParentNIds = string.Empty;

            if (!(string.IsNullOrEmpty(icNIds)))
            {
                RetVal = icNIds;

                //-- SQl query to get parent NIds
                Query = this.DestDBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, icNIds, FieldSelection.Light);

                //- get ParentNids in a string.
                ParentNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(Query), IndicatorClassifications.ICParent_NId);

                if (string.IsNullOrEmpty(ParentNIds) == false)
                {
                    ParentNIds = this.GetICParentNIds(ParentNIds);

                    RetVal = Common.MergeNIds(RetVal, ParentNIds);
                }
            }

            return RetVal;
        }

        private string GetAreaParentsNIds(string areaNIds)
        {
            string RetVal = string.Empty;
            string Query = string.Empty;
            string ParentNIds = string.Empty;

            if (!(string.IsNullOrEmpty(areaNIds)))
            {
                RetVal = areaNIds;

                //-- SQl query to get parent NIds
                Query = this.DestDBQueries.Area.GetArea(FilterFieldType.NId, areaNIds);

                //- get ParentNids in a string.
                ParentNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(Query), Area.AreaParentNId);

                if (string.IsNullOrEmpty(ParentNIds) == false)
                {
                    ParentNIds = this.GetAreaParentsNIds(ParentNIds);

                    RetVal = Common.MergeNIds(RetVal, ParentNIds);
                }
            }

            return RetVal;
        }

        private void ExecuteDelete(string query, string filterNID)
        {
            if (string.IsNullOrEmpty(filterNID) == false)
            {
                this.DestDBConnection.ExecuteNonQuery(query);
            }
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- New / Displose --"
        internal ExportDatabase(UserSelection userSelection, DIConnection sourceDBConnection)
        {
            if (sourceDBConnection != null && userSelection != null)
            {
                //--  Assign NIDs in userSelection.
                if (userSelection.ShowIUS)
                {
                    this.IUSNIDs = userSelection.IndicatorNIds;
                }
                else
                {
                    this.IndicatorNIDs = userSelection.IndicatorNIds;
                }
                this.UnitNIDs = userSelection.UnitNIds;
                this.SubgroupValNIDs = userSelection.SubgroupValNIds;
                this.AreaNIDs = userSelection.AreaNIds;
                this.TimePeriodNIDs = userSelection.TimePeriodNIds;
                this.SourceNIDs = userSelection.SourceNIds;
                this.ICNIDs = userSelection.ICNIds;
                this.ApplyMRD = userSelection.DataViewFilters.MostRecentData;
                this.DataViewFilters = userSelection.DataViewFilters;

                this.SourceDBConnection = sourceDBConnection;
            }
        }

        #endregion

        private int _AreaLevel = -1;
        /// <summary>
        /// gets or sets the area Level of selected areas.
        /// </summary>
        public int AreaLevel
        {
            get { return _AreaLevel; }
            set { _AreaLevel = value; }
        }


        #region "-- Methods --"

        internal bool ExportMDB(bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath)
        {
            bool RetVal = false;
            string TempDatabase = string.Empty;
            RecommendedSourcesBuilder RecommendedSrcBuilder;

            //-- Validate source DBConnection object, destination database name
            if (this.SourceDBConnection != null && string.IsNullOrEmpty(destinationDBNameWPath) == false)
            {
                try
                {
                    TempDatabase = Path.Combine(tempFolderPath, DateTime.Now.Ticks.ToString());

                    if (File.Exists(TempDatabase))
                    {
                        File.Delete(TempDatabase);
                    }

                    //-- Copy source Database in temp folder
                    File.Copy(SourceDBConnection.ConnectionStringParameters.DbName, TempDatabase);
                    File.SetAttributes(TempDatabase, FileAttributes.Normal);

                    //-- Create DIConnection, DIQueries object for temp Database.
                    this.DestDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TempDatabase, string.Empty, this.SourceDBConnection.ConnectionStringParameters.Password);
                    this.DestDBQueries = new DIQueries(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                    this.DBTableNames = new DITables(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                    //-- Get LanguageTable
                    this.LanguageTable = this.DestDBConnection.DILanguages(this.DestDBConnection.DIDataSetDefault());

                    //*************************************************************
                    //-------Start Deletion Process---------

                    //-- if maintainTemplate is true, then  Delete records from UT_Data & Notes Tables only
                    //-- else delete from all associated tables.
                    if (maintainTemplate)
                    {
                        this.DeleteFromAreaByAreaLevel();
                        this.DeleteAndMaintainTemplate();
                    }
                    else
                    {
                        this.UpdateNIdsByUserSelection();
                        this.DeleteFromAreaByAreaLevel();
                        this.DeleteFromALLTables();
                    }


                    // Delete extra rows from RecommendedSources table
                    RecommendedSrcBuilder = new RecommendedSourcesBuilder(this.DestDBConnection, this.DestDBQueries);
                    RecommendedSrcBuilder.DeleteExtraRowsFrmRecommendedSources();


                    //-----------Deletion ends--------------


                    ////-- Close DBConnection
                    //DestDBConnection.Dispose();
                    //DestDBQueries = null;

                    //-- Restrict FileName to 245 characters, as upper limit of windows filename.
                    if (destinationDBNameWPath.Length > 245)
                    {
                        destinationDBNameWPath.Substring(0, 245);
                    }

                    //-- Compact database and saves in destination path specified.
                    //////DA.DML.DIDatabase.CompactDataBase(TempDatabase, this.SourceDBConnection.ConnectionStringParameters, destinationDBNameWPath);

                    DA.DML.DIDatabase.CompactDataBase(ref this.DestDBConnection, this.DestDBQueries, destinationDBNameWPath, true);
                    DestDBQueries = null;
                    //-- Delete temp Database.
                    try
                    {
                        File.Delete(TempDatabase);
                    }
                    catch
                    {

                    }
                    RetVal = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionFacade.ThrowException(ex);
                }
                finally
                {
                    //-- Close DBConnection
                    if (DestDBConnection != null)
                    {
                        DestDBConnection.Dispose();
                        DestDBQueries = null;
                    }
                }
            }
            return RetVal;
        }

        private void UpdateNIdsByUserSelection()
        {
            DataTable Table = null;

            if (string.IsNullOrEmpty(this.IUSNIDs) && !string.IsNullOrEmpty(this.IndicatorNIDs))
            {
                Table = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUSByI_U_S(this.IndicatorNIDs, string.Empty, string.Empty));
                this.IUSNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator_Unit_Subgroup.IUSNId);
                this.UnitNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator_Unit_Subgroup.UnitNId);
                this.SubgroupValNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator_Unit_Subgroup.SubgroupValNId);
            }

            Table = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Data.GetDataByIUSTimePeriodAreaSource(this.IUSNIDs, this.TimePeriodNIDs, this.AreaNIDs, this.SourceNIDs, FieldSelection.Light));

            if (string.IsNullOrEmpty(this.IndicatorNIDs))
            {
                this.IndicatorNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator_Unit_Subgroup.IndicatorNId);
            }

            if (string.IsNullOrEmpty(this.IUSNIDs))
            {
                this.IUSNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator_Unit_Subgroup.IUSNId);
            }
            if (string.IsNullOrEmpty(this.AreaNIDs))
            {
                this.AreaNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.AreaNId);
            }
            if (string.IsNullOrEmpty(this.TimePeriodNIDs))
            {
                this.TimePeriodNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.TimePeriodNId);
            }
            if (string.IsNullOrEmpty(this.SourceNIDs))
            {
                this.SourceNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.SourceNId);
            }
            this.UnitNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.UnitNId);
            this.SubgroupValNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.SubgroupValNId);
        }

        private void DeleteAndMaintainTemplate()
        {
            string NewIUSNIds = string.Empty;

            string NewICNIds = string.Empty;

            string LangCode = string.Empty;
            string FilterNIDs = string.Empty;
            string DeleteQuery = string.Empty;

            //-- Get IUSNId for given IndicatorNIds, UnitNIds, subgroupNids
            if (string.IsNullOrEmpty(this.IndicatorNIDs) == false && string.IsNullOrEmpty(this.UnitNIDs) == false && string.IsNullOrEmpty(this.SubgroupValNIDs) == false)
            {
                NewIUSNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUSByI_U_S(this.IndicatorNIDs, this.UnitNIDs, this.SubgroupValNIDs)), Indicator_Unit_Subgroup.IUSNId);

                //-- Merge NewIUS with given IUS.
                this.IUSNIDs = Common.MergeNIds(this.IUSNIDs, NewIUSNIds);
            }

            //-- Delete records from UT_Data & Notes Tables only
            this.DeleteFromDataTable();

            this.DeleteFromNotesTables();

            this.DeleteFromFootNoteTable();

            if (string.IsNullOrEmpty(this.TimePeriodNIDs) == false)
            {
                this.DeleteFromTimePeriodTable();
            }

            ////-- delete from IC table for given sourceIDs
            //this.SourceNIDs = this.GetICParentNIds(this.SourceNIDs);
            //if (string.IsNullOrEmpty(this.SourceNIDs) == false)
            //{
            //    this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.IndicatorClassifications, IndicatorClassifications.ICNId, this.SourceNIDs, true), this.SourceNIDs);
            //}

            this.DeleteSourcesFromICTable();
        }

        private void DeleteFromALLTables()
        {
            // IMPORTANT:
            // Deletion order will be different for following cases 1 to 6:

            // 1) Multple Inputs (At least 2)- IUSNID, TimePeriodNId, AreaNID, SourceNIds 
            if ((!(string.IsNullOrEmpty(this.AreaNIDs)) && !(string.IsNullOrEmpty(this.IUSNIDs))) ||
            ((!(string.IsNullOrEmpty(this.AreaNIDs)) && !(string.IsNullOrEmpty(this.IndicatorNIDs)))) ||
            (!(string.IsNullOrEmpty(this.TimePeriodNIDs)) && !(string.IsNullOrEmpty(this.IUSNIDs))) ||
            (!(string.IsNullOrEmpty(this.TimePeriodNIDs)) && !(string.IsNullOrEmpty(this.IndicatorNIDs))))
            {
                this.StartDeleteFromALL();
            }

            // 2) Single Input - AreaNID only
            else if (!(string.IsNullOrEmpty(this.AreaNIDs)))
            {
                this.StartDeletionFromArea();
            }

            // 3) Single Input - TimePeriodNIDs only
            else if (!(string.IsNullOrEmpty(this.TimePeriodNIDs)))
            {
                this.StartDeletionFromTimePeriod();
            }

            // 4) Single Input - IndicatorNIDs OR UnitNids OR SubgroupNIDs only
            else if (!(string.IsNullOrEmpty(this.IndicatorNIDs)) || !(string.IsNullOrEmpty(this.UnitNIDs)) || !(string.IsNullOrEmpty(this.SubgroupValNIDs)))
            {
                this.StartDeletionFromI_U_S();
            }

            // 5) Single Input - IUSNIDs only
            else if (!(string.IsNullOrEmpty(this.IUSNIDs)))
            {
                this.StartDeletionFromIUS();
            }

            // 6) Single Input - ICNIDs only
            else if (!(string.IsNullOrEmpty(this.ICNIDs)))
            {
                this.StartDeletionFromIC();
            }
            else if (!(string.IsNullOrEmpty(this.SourceNIDs)))
            {
                this.StartDeletionFromSource();
            }
        }

        private void StartDeleteFromALL()
        {
            string NewIUSNIds = string.Empty;
            DataTable TempTable = null;

            if ((string.IsNullOrEmpty(this.ICNIDs)))
            {
                //-- Delete from UT_indicators for given IndicatorNIDs
                this.DeleteFromIndicatorTable();

                //-- Delete from UT_Units for given UnitNIDs
                this.DeleteFromUnitTable();

                //-- Delete from UT_SubgroupVal for given SubgroupValNIDs
                this.DeleteFromSubgroupTables();

                //-- Delete from IUS where indicator in (given Indicator) AND Unit in (given) AND SubgorupVal in (Given) AND IUSNID in (given iusNId)
                this.DeleteFromIUSTable();

                //-- Get new IUSNid left in IUS Table
                NewIUSNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.NId)), Indicator_Unit_Subgroup.IUSNId);

                // Merge into given IUSNIDs
                this.IUSNIDs = NewIUSNIds;  // Common.MergeNIds(this.IUSNIDs, NewIUSNIds);

                //-- Get all IC_nids where IUSnids are available in IUS table.
                //  this.ICNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, string.Empty, this.IUSNIDs, FieldSelection.NId)), IndicatorClassifications.ICNId);
                this.ICNIDs = GetICNidsForAvailableIUS();

                // DO NOT delete parent ICs
                //--Get parent ICNids and merge
                // this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

                //--- UT_IndicatorClassification_IUS
                this.DeleteFromICIUSTable();

                //--- UT_IndicatorClassification
                this.DeleteFromICTables();

            }
            else
            {
                //--Get parent ICNids and merge with given ICNIds
                this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

                this.DeleteFromICTables();
                this.DeleteFromICIUSTable();

                ////-- Get IUSNId from IC_IUS table.
                //NewIUSNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable("Select DISTINCT " + Indicator_Unit_Subgroup.IUSNId + " from " + DBTableNames.IndicatorClassificationsIUS), Indicator_Unit_Subgroup.IUSNId);
                ////-- Delete from IUSTable for newIUSNIds
                //string Query = DIQueries.DeleteRecords(this.DBTableNames.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.IUSNId, NewIUSNIds, true);
                //this.ExecuteDelete(Query, NewIUSNIds);

                //-- Get IUSNId from IC_IUS table.
                //-- Delete from IUSTable for newIUSNIds
                string Query = "DELETE FROM " + this.DBTableNames.IndicatorUnitSubgroup + " As IUS WHERE NOT EXISTS( SELECT * FROM " + this.DBTableNames.IndicatorClassificationsIUS + " AS ICIUS WHERE IUS." + Indicator_Unit_Subgroup.IUSNId + "=ICIUS." + IndicatorClassificationsIUS.IUSNId + ")";
                this.DestDBConnection.ExecuteNonQuery(Query);

                //-- Delete records from IUS Table for given IndicatorNID, UnitNIDs, SubgroupNIds, IUSNids

                this.DeleteFromIUSTable();

                this.DeleteFromIndicatorTable();
                this.DeleteFromUnitTable();
                this.DeleteFromSubgroupTables();

            }

            // get IUSNid, IndicatorNIds, UnitNIds, SubgroupvalNids left in IUS Table
            // and merge them in given NIds
            TempTable = DestDBConnection.ExecuteDataTable(DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

            this.IUSNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IUSNId);

            //-merge in Given IndicatorNIDs
            this.IndicatorNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IndicatorNId);

            //- merge in Given UnitNIDs
            this.UnitNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.UnitNId);

            //- merge in Given SubgroupValNIDs
            this.SubgroupValNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.SubgroupValNId);

            //-- Delete again from Tables: Indicator, Unit , SubgroupVal
            this.DeleteFromIndicatorTable();
            this.DeleteFromUnitTable();
            this.DeleteFromSubgroupTables();

            this.DeleteFromDataTable();
            this.DeleteFromAreaTables();

            this.DeleteFromNotesTables();

            this.DeleteFromAssistantTables();

            this.DeleteFromXSLTTables();

            //-- Delete records from Tables: timePeriod, Footnotes, XSLT
            this.DeleteFromFootNoteTable();
            this.DeleteFromTimePeriodTable();

            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();
        }

        private void StartDeletionFromI_U_S()
        {
            string NewIUSNIds = string.Empty;

            string NIDsLeft = string.Empty;

            string LangCode = string.Empty;
            string FilterNIDs = string.Empty;
            string DeleteQuery = string.Empty;
            DataTable TempTable = null;

            //-- Delete from UT_indicators for given IndicatorNIDs
            this.DeleteFromIndicatorTable();

            //-- Delete from UT_Units for given UnitNIDs
            this.DeleteFromUnitTable();

            //-- Delete from UT_SubgroupVal for given SubgroupValNIDs
            this.DeleteFromSubgroupTables();

            //-- Delete from IUS where indicator in (given Indicator) AND Unit in (given) AND SubgorupVal in (Given) AND IUSNID in (given iusNId)
            this.DeleteFromIUSTable();

            //-- Get new IUSNid left in IUS Table
            NewIUSNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.NId)), Indicator_Unit_Subgroup.IUSNId);

            // Merge into given IUSNIDs
            this.IUSNIDs = Common.MergeNIds(this.IUSNIDs, NewIUSNIds);

            //-- Get all IC_nids where IUSnids are available in IUS table.
            this.ICNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, string.Empty, this.IUSNIDs, FieldSelection.NId)), IndicatorClassifications.ICNId);

            // DO NOT delete parent ICs
            //--Get parent ICNids and merge
            this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

            //--- UT_IndicatorClassification_IUS
            this.DeleteFromICIUSTable();

            //--- UT_IndicatorClassification
            this.DeleteFromICTables();

            //-------------------------------


            // get IUSNid, IndicatorNIds, UnitNIds, SubgroupvalNids left in IUS Table
            // and merge them in given NIds
            TempTable = DestDBConnection.ExecuteDataTable(DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

            this.IUSNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IUSNId);

            //-merge in Given IndicatorNIDs
            this.IndicatorNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IndicatorNId);

            //- merge in Given UnitNIDs
            this.UnitNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.UnitNId);

            //- merge in Given SubgroupValNIDs
            this.SubgroupValNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.SubgroupValNId);

            //-- Delete again from Tables: Indicator, Unit , SubgroupVal
            this.DeleteFromIndicatorTable();
            this.DeleteFromUnitTable();
            this.DeleteFromSubgroupTables();

            this.DeleteFromDataTable();

            //-- Get AreaNIds left in DATA table
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.AutoSelectArea(string.Empty, string.Empty, string.Empty, string.Empty));
            NIDsLeft = Common.GetCommaSeperatedString(TempTable, Data.AreaNId);
            this.AreaNIDs = NIDsLeft;
            this.DeleteFromAreaTables();

            this.DeleteFromNotesTables();

            this.DeleteFromAssistantTables();

            this.DeleteFromXSLTTables();

            //-- Delete records from Tables: timePeriod, Footnotes, XSLT
            this.DeleteFromFootNoteTable();
            this.DeleteFromTimePeriodTable();
            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();
        }

        private void StartDeletionFromArea()
        {
            string Query = string.Empty;

            //- Delete from Area for given AreaNID
            this.DeleteFromAreaTables();

            //-- delete from DATA table
            //--Delete records where AreaNid NOT in (given AreaNid)
            //Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.AreaNId, this.AreaNIDs, true);
            //this.ExecuteDelete(Query, this.AreaNIDs);

            this.DeleteBulkRecords(DBTableNames.Data, Data.AreaNId, this.AreaNIDs, false);

            // Delete from TimePeriod
            this.DeleteFromTimePeriodTable();

            this.DeleteRecordsForArea();
            //this.DeleteProcessCommonToAreaTimeperiod();
        }

        private void StartDeletionFromTimePeriod()
        {
            DataTable TempTable = null;
            string Query = string.Empty;
            string NIdsLeft = string.Empty;

            //--Delete records where TimePeriodNid NOT in (given TimePeriodNid)
            Query = DIQueries.DeleteRecords(this.DBTableNames.TimePeriod, Data.TimePeriodNId, this.TimePeriodNIDs, true);
            this.ExecuteDelete(Query, this.TimePeriodNIDs);


            //-- delete from DATA table
            //--Delete records where TimePeriodNid NOT in (given TimePeriodNid)
            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.TimePeriodNId, this.TimePeriodNIDs, true);
            this.ExecuteDelete(Query, TimePeriodNIDs);

            // Get Area left in data Tables
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.AutoSelectArea(string.Empty, string.Empty, string.Empty, string.Empty));
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Data.AreaNId);
            this.AreaNIDs = Common.MergeNIds(NIdsLeft, this.AreaNIDs);

            //- Delete from Area for given AreaNID
            this.DeleteFromAreaTables();

            this.DeleteProcessCommonToAreaTimeperiod();
        }

        private void DeleteProcessCommonToAreaTimeperiod()
        {
            string NIdsLeft = string.Empty;

            string NewICNIds = string.Empty;

            DataTable TempTable = null;

            // Get IUSNID left in DATA Table
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetDistinctIUSNIdFrmDataTable());
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IUSNId);
            this.IUSNIDs = Common.MergeNIds(NIdsLeft, this.IUSNIDs);

            // Delete from IUS table
            this.DeleteFromIUSTable();

            // get IndicatorNId left in IUS
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.NId));
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Indicator.IndicatorNId);
            this.IndicatorNIDs = Common.MergeNIds(NIdsLeft, this.IndicatorNIDs);

            // Delete from indicator
            this.DeleteFromIndicatorTable();

            // get unitNID left in IUS
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Unit.UnitNId);
            this.UnitNIDs = Common.MergeNIds(NIdsLeft, this.UnitNIDs);

            // delete from UNit
            this.DeleteFromUnitTable();

            // Get SubgroupNId left in IUS
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, SubgroupVals.SubgroupValNId);
            this.SubgroupValNIDs = Common.MergeNIds(NIdsLeft, this.SubgroupValNIDs);

            // Delete from subgroup
            this.DeleteFromSubgroupTables();

            // delete from IC_IUS
            this.DeleteFromICIUSTable();

            //-- Get all IC_nids where IUSnids are available in IUS table.
            // this.ICNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, string.Empty, this.IUSNIDs, FieldSelection.NId)), IndicatorClassifications.ICNId);
            this.ICNIDs = this.GetICNidsForAvailableIUS();
            // DO NOT delete parent ICs
            //--Get parent ICNids and merge
            this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

            // delete from IC table
            this.DeleteFromICTables();

            // Delete from other rest tables
            this.DeleteFromNotesTables();

            this.DeleteFromAssistantTables();

            this.DeleteFromXSLTTables();

            //-- Delete records from Tables: timePeriod, Footnotes, XSLT
            this.DeleteFromFootNoteTable();

            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();
        }

        private void StartDeletionFromIUS()
        {
            string NIdsLeft = string.Empty;

            DataTable TempTable = null;

            //-- Delete from IUS where indicator in (given Indicator) AND Unit in (given) AND SubgorupVal in (Given) AND IUSNID in (given iusNId)
            this.DeleteFromIUSTable();

            // get IndicatorNId left in IUS
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.NId));
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Indicator.IndicatorNId);
            this.IndicatorNIDs = Common.MergeNIds(NIdsLeft, this.IndicatorNIDs);

            // get unitNID left in IUS
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Unit.UnitNId);
            this.UnitNIDs = Common.MergeNIds(NIdsLeft, this.UnitNIDs);

            // Get SubgroupNId left in IUS
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, SubgroupVals.SubgroupValNId);
            this.SubgroupValNIDs = Common.MergeNIds(NIdsLeft, this.SubgroupValNIDs);

            //-- Delete from UT_indicators for given IndicatorNIDs
            this.DeleteFromIndicatorTable();

            //-- Delete from UT_Units for given UnitNIDs
            this.DeleteFromUnitTable();

            //-- Delete from UT_SubgroupVal for given SubgroupValNIDs
            this.DeleteFromSubgroupTables();

            //-- Get all IC_nids where IUSnids are available in IUS table.
            this.ICNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, string.Empty, this.IUSNIDs, FieldSelection.NId)), IndicatorClassifications.ICNId);

            // DO NOT delete parent ICs
            //--Get parent ICNids and merge
            this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

            //--- UT_IndicatorClassification_IUS
            this.DeleteFromICIUSTable();

            //--- UT_IndicatorClassification
            this.DeleteFromICTables();


            this.DeleteFromDataTable();

            // Get Area left in data Tables
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.AutoSelectArea(string.Empty, string.Empty, string.Empty, string.Empty));
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Data.AreaNId);
            this.AreaNIDs = Common.MergeNIds(NIdsLeft, this.AreaNIDs);

            this.DeleteFromAreaTables();

            this.DeleteFromNotesTables();

            this.DeleteFromAssistantTables();

            this.DeleteFromXSLTTables();

            //-- Delete records from Tables: timePeriod, Footnotes, XSLT
            this.DeleteFromFootNoteTable();
            this.DeleteFromTimePeriodTable();

            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();
        }

        private void StartDeletionFromIC()
        {
            string NewIUSNIds = string.Empty;

            string NewICNIds = string.Empty;

            string LangCode = string.Empty;
            string FilterNIDs = string.Empty;
            string DeleteQuery = string.Empty;

            string TempICNids = string.Empty;

            DataTable TempTable = null;


            //--Get parent ICNids and merge with given ICNIds
            this.ICNIDs = this.GetICParentNIds(this.ICNIDs);

            // dont delete source 
            this.DeleteFromICTables();

            // get all icnids from indicator classifications 
            TempICNids = this.ICNIDs;

            this.ICNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable("Select DISTINCT " + IndicatorClassifications.ICNId + " from " + DBTableNames.IndicatorClassifications), IndicatorClassifications.ICNId);

            this.DeleteFromICIUSTable();

            this.ICNIDs = TempICNids;
            //-- Get IUSNId from IC_IUS table.
            NewIUSNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable("Select DISTINCT " + Indicator_Unit_Subgroup.IUSNId + " from " + DBTableNames.IndicatorClassificationsIUS + " WHERE " + IndicatorClassifications.ICNId + " IN (" + this.ICNIDs + ")"), Indicator_Unit_Subgroup.IUSNId);

            //-- Delete from IUSTable for newIUSNIds
            string Query = DIQueries.DeleteRecords(this.DBTableNames.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.IUSNId, NewIUSNIds, true);
            this.ExecuteDelete(Query, NewIUSNIds);

            //-- Delete records from IUS Table for given IndicatorNID, UnitNIDs, SubgroupNIds, IUSNids

            this.DeleteFromIUSTable();

            this.DeleteFromIndicatorTable();
            this.DeleteFromUnitTable();
            this.DeleteFromSubgroupTables();
            //----------------------------------------

            // get IUSNid, IndicatorNIds, UnitNIds, SubgroupvalNids left in IUS Table
            // and merge them in given NIds
            TempTable = DestDBConnection.ExecuteDataTable(DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

            this.IUSNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IUSNId);

            //-merge in Given IndicatorNIDs
            this.IndicatorNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IndicatorNId);

            //- merge in Given UnitNIDs
            this.UnitNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.UnitNId);

            //- merge in Given SubgroupValNIDs
            this.SubgroupValNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.SubgroupValNId);

            //-- Delete again from Tables: Indicator, Unit , SubgroupVal
            this.DeleteFromIndicatorTable();
            this.DeleteFromUnitTable();
            this.DeleteFromSubgroupTables();

            this.DeleteFromDataTable();
            this.DeleteFromAreaTables();

            this.DeleteFromNotesTables();

            this.DeleteFromAssistantTables();

            this.DeleteFromXSLTTables();

            //-- Delete records from Tables: timePeriod, Footnotes, XSLT
            this.DeleteFromFootNoteTable();
            this.DeleteFromTimePeriodTable();

            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();

            // get IUSNid, IndicatorNIds, UnitNIds, SubgroupvalNids left in IUS Table
            // and merge them in given NIds
            TempTable = DestDBConnection.ExecuteDataTable(DestDBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

            this.IUSNIDs = Common.GetCommaSeperatedString(TempTable, Indicator_Unit_Subgroup.IUSNId);

            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.IndicatorClassificationsIUS, IndicatorClassificationsIUS.IUSNId, this.IUSNIDs, true), this.IUSNIDs);

        }

        private void StartDeletionFromSource()
        {
            DataTable TempTable = null;
            string Query = string.Empty;
            string NIdsLeft = string.Empty;

            //-- Delete sources from ICTable
            this.DeleteSourcesFromICTable();

            //-- delete from DATA table
            //--Delete records where SourceNId NOT in (given SourceNID)
            Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SourceNId, this.SourceNIDs, true);
            this.ExecuteDelete(Query, this.SourceNIDs);

            // Get Area left in data Tables
            TempTable = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.AutoSelectArea(string.Empty, string.Empty, string.Empty, string.Empty));
            NIdsLeft = Common.GetCommaSeperatedString(TempTable, Data.AreaNId);
            this.AreaNIDs = Common.MergeNIds(NIdsLeft, this.AreaNIDs);

            //- Delete from Area for given AreaNID
            this.DeleteFromAreaTables();

            //-- Delete from TimePeriod
            this.DeleteFromTimePeriodTable();

            this.DeleteProcessCommonToAreaTimeperiod();
        }

        #region "-- Old Split Database Methods --"
        //private void DeleteFromXSLTTables()
        //{
        //    string FilterNIDs = string.Empty;
        //    string LangCode = string.Empty;
        //    string AllAreaNIds = string.Empty;
        //    //- Delete from UT_Element_XSLT where Element_ID NOT in (given Indicator_NId)
        //    this.ExecuteDelete("Delete from " + this.DBTableNames.ElementXSLT + " where " + EelementXSLT.ElementNId + " NOT IN (" + this.IndicatorNIDs + ") AND " + EelementXSLT.ElementType + " = 'I'", this.IndicatorNIDs);

        //    // Get Parent NIDs and merge into given AreaNIds
        //    AllAreaNIds = this.GetAreaParentsNIds(this.AreaNIDs);

        //    //- Delete from UT_Element_XSLT where Element_ID NOT in (given Area_NId) 
        //     this.ExecuteDelete("Delete from " + this.DBTableNames.ElementXSLT + " where " + EelementXSLT.ElementNId + " NOT IN (" + AllAreaNIds + ") AND " + EelementXSLT.ElementType + " = 'A'", AllAreaNIds);


        //    //- Delete from UT_Element_XSLT where Element_ID NOT in (given .Source_NIds)
        //    this.ExecuteDelete("Delete from " + this.DBTableNames.ElementXSLT + " where " + EelementXSLT.ElementNId + " NOT IN (" + this.SourceNIDs + ") AND " + EelementXSLT.ElementType + " = 'S'", this.SourceNIDs);

        //    //-- Get XSLT NIDs left in UT_Element_XSLT
        //    FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Xslt.GetXSLT(FilterFieldType.None, "")), XSLT.XSLTNId);

        //    //- Delete from UT_XSLT where XSLT_Nid NOT In UT_Element_XSLT.
        //    this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.XSLT, XSLT.XSLTNId, FilterNIDs, true), FilterNIDs);

        //}
        //private void DeleteFromDataTable()
        //{
        //    string Query = string.Empty;

        //    //-- Delete from Data Table WHERE IndicatorNId NOT in (given IndicatorNId)
        //    if (!string.IsNullOrEmpty(this.IndicatorNIDs))
        //    {
        //        //-- Delete records where IndicatorNId NOT in (given Indicator NID)
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IndicatorNId, this.IndicatorNIDs, true);
        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }

        //    //-- Delete from Data Table WHERE UnitNId NOT in (given UnitNId)
        //    if (!string.IsNullOrEmpty(this.UnitNIDs))
        //    {
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.UnitNId, this.UnitNIDs, true);
        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }

        //    //-- Delete from Data Table WHERE IndicatorNId NOT in (given IndicatorNId)
        //    if (!string.IsNullOrEmpty(this.SubgroupValNIDs))
        //    {
        //        //-- Delete records where SubgroupValNIDs NOT in (given SubgroupValNIDs )
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SubgroupValNId, this.SubgroupValNIDs, true);
        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }


        //    if (!string.IsNullOrEmpty(this.IUSNIDs))
        //    {
        //        //-- Delete records where IUSNid NOT in (IUSNId left)
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.IUSNId, this.IUSNIDs, true);
        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }



        //    //-- Delete records where Data NOT EXISTS for given IC_NID .
        //    if (string.IsNullOrEmpty(this.ICNIDs) == false)
        //    {
        //        Query = "DELETE FROM " + DBTableNames.Data + " AS D " +
        //            " WHERE NOT EXISTS (Select * from " + DBTableNames.IndicatorUnitSubgroup + " AS IUS " +
        //            " WHERE EXISTS (Select * from " + DBTableNames.IndicatorClassificationsIUS + " as IC_IUS " +
        //            " WHERE EXISTS (Select * from " + DBTableNames.IndicatorClassifications + " AS IC " +
        //            " WHERE IC_NID in (" + this.ICNIDs + ") AND IC." + IndicatorClassifications.ICNId + " = IC_IUS." + IndicatorClassificationsIUS.ICNId +
        //            ") AND IC_IUS." + IndicatorClassificationsIUS.IUSNId + "= IUS." + Indicator_Unit_Subgroup.IUSNId +
        //            ") AND IUS." + Indicator_Unit_Subgroup.IUSNId + "= D." + Data.IUSNId + ")";
        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }

        //    //--Delete records where AreaNid NOT in (given AreaNid)
        //    if (!string.IsNullOrEmpty(this.AreaNIDs))
        //    {
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.AreaNId, this.AreaNIDs, true);
        //        this.ExecuteDelete(Query, AreaNIDs);
        //    }


        //    //--Delete records where TimePeriodNid NOT in (given TimePeriodNid)
        //    if (!string.IsNullOrEmpty(this.TimePeriodNIDs))
        //    {

        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.TimePeriodNId, this.TimePeriodNIDs, true);
        //        this.ExecuteDelete(Query, TimePeriodNIDs);
        //    }

        //    //--Delete records where SourceNid NOT in (given SourceNId)
        //    if (!string.IsNullOrEmpty(this.SourceNIDs))
        //    {
        //        Query = DIQueries.DeleteRecords(DBTableNames.Data, Data.SourceNId, this.SourceNIDs, true);
        //        this.ExecuteDelete(Query, SourceNIDs);
        //    }

        //    //- Delete Records if DataViewfilters are present. 
        //    this.DeleteFromDataByDataFilters();
        //}
        //private void DeleteFromIUSTable()
        //{
        //    string NotInFilter = string.Empty;
        //    string Query = string.Empty;

        //    // Delete from IUS where IndicatorNid not in (givenIndicatorNid) and UnitNid not in(given unit) and sg_nid not in(given sg nid) and iusnid not in(given iusnid)
        //    if (!(string.IsNullOrEmpty(this.IndicatorNIDs)) || !(string.IsNullOrEmpty(this.UnitNIDs)) || !(string.IsNullOrEmpty(this.SubgroupValNIDs)) || !(string.IsNullOrEmpty(this.IUSNIDs)))
        //    {
        //        Query = DevInfo.Lib.DI_LibDAL.Queries.IUS.Delete.DeleteIUS(this.DBTableNames.IndicatorUnitSubgroup, this.IndicatorNIDs, this.UnitNIDs, this.SubgroupValNIDs, this.IUSNIDs, true);

        //        this.DestDBConnection.ExecuteNonQuery(Query);
        //    }

        //}

        //private void DeleteFromIndicatorTable()
        //{
        //    string LangCode = string.Empty;
        //    foreach (System.Data.DataRow row in this.LanguageTable.Rows)
        //    {
        //        LangCode = row[Language.LanguageCode].ToString();

        //        this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

        //        //-- delete Indicators
        //        //TempDBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Delete.DeleteIndicator(DBTableNames.Indicator, IndicatorNIDs));
        //        this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Indicator, Indicator.IndicatorNId, this.IndicatorNIDs, true), this.IndicatorNIDs);

        //    }
        //}

        //private void DeleteFromUnitTable()
        //{
        //    string LangCode = string.Empty;
        //    foreach (System.Data.DataRow row in this.LanguageTable.Rows)
        //    {
        //        LangCode = row[Language.LanguageCode].ToString();

        //        this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

        //        //-- delete Units
        //        this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Unit, Unit.UnitNId, this.UnitNIDs, true), this.UnitNIDs);

        //    }
        //}

        //private void DeleteFromSubgroupTables()
        //{
        //    string LangCode = string.Empty;
        //    foreach (System.Data.DataRow row in this.LanguageTable.Rows)
        //    {
        //        LangCode = row[Language.LanguageCode].ToString();

        //        this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

        //        //-- delete SubgroupVals
        //        //TODO TempDBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup.Delete.
        //        this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.SubgroupVals, SubgroupVals.SubgroupValNId, this.SubgroupValNIDs, true), this.SubgroupValNIDs);

        //        //-- Delete Subgroup

        //    }
        //}
        //private void DeleteFromAreaTables()
        //{
        //    string LangCode = string.Empty;
        //    string Query = string.Empty;
        //    string FilterNIDs = string.Empty;
        //    string AllAreaNIds = string.Empty;

        //    if (String.IsNullOrEmpty(this.AreaNIDs) == false)
        //    {
        //        foreach (System.Data.DataRow row in this.LanguageTable.Rows)
        //        {
        //            LangCode = row[Language.LanguageCode].ToString();

        //            this.DBTableNames = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

        //            // DO NOT delete parent Areas
        //            // Get Parent NIDs and merge into given AreaNIds
        //            AllAreaNIds = this.GetAreaParentsNIds(this.AreaNIDs);

        //            //--Delete from UT_Area_en where AreaNIDs NOT in (given AreaNids)
        //            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Area, Area.AreaNId, AllAreaNIds, true), AllAreaNIds);

        //            //--Delete from UT_Area_Map where AreaNIDs NOT IN (given AreaNids)
        //            this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.AreaMap, Area_Map.AreaNId, AllAreaNIds, true), AllAreaNIds);


        //            //--Delete from UT_Area_level_en where Area_Level NOT IN (Select Area_Level from UT_Area_en )
        //            FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaLevel(AllAreaNIds)), Area_Level.AreaLevel);
        //            Query = DIQueries.DeleteRecords(this.DBTableNames.AreaLevel, Area_Level.AreaLevel, FilterNIDs, true);
        //            this.ExecuteDelete(Query, FilterNIDs);

        //            // Get Layer_NID from UT_Area_Map for given AreaNIds
        //            System.Data.DataTable TempTable = this.DestDBConnection.ExecuteDataTable(DestDBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, AllAreaNIds, FieldSelection.NId));
        //            FilterNIDs = Common.GetCommaSeperatedString(TempTable, Area_Map_Layer.LayerNId);
        //            //--Delete from UT_Area_Map_Layer_en where Layer_NID NOT IN (Select Layer_NID from UT_Area_Map )
        //            Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Map_Layer.LayerNId, FilterNIDs, true);
        //            this.ExecuteDelete(Query, FilterNIDs);

        //            // Get MetaDataNIDs for given Layer_NIDs
        //            FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaMapMetadata(FilterNIDs)), Area_Map_Metadata.MetadataNId);
        //            // Delete from UT_Area_Map_Metadata_en where Metadata_NID NOT IN (given MetaDataNIDs)
        //            Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapMetadata, Area_Map_Metadata.MetadataNId, FilterNIDs, true);
        //            this.ExecuteDelete(Query, FilterNIDs);

        //            //Get Feature_Type_NID for given AreaNIds
        //            if (String.IsNullOrEmpty(this.AreaNIDs) == false)
        //            {
        //                FilterNIDs = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Area.GetAreaFeatureByAreaNid(AllAreaNIds)), Area_Feature_Type.FeatureTypeNId);
        //                //--Delete from UT_Area_Feature_Type_en where Feature_Type_NId NOT in (given Feature_Type_NID)
        //                Query = DIQueries.DeleteRecords(this.DBTableNames.AreaMapLayer, Area_Feature_Type.FeatureTypeNId, FilterNIDs, true);
        //                this.ExecuteDelete(Query, FilterNIDs);
        //            }
        //        }

        //        //--Delete from UT_Data where AreaNIDs NOT in (given AreaNids)
        //        this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.Data, Area.AreaNId, this.AreaNIDs, true), this.AreaNIDs);

        //    }
        //}

        //private void DeleteFromICIUSTable()
        //{
        //    //***********Table:  UT_IndicatorClassification_IUS *************
        //    this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.IndicatorClassificationsIUS, IndicatorClassificationsIUS.ICNId, this.ICNIDs, true), this.ICNIDs);
        //    this.ExecuteDelete(DIQueries.DeleteRecords(this.DBTableNames.IndicatorClassificationsIUS, IndicatorClassificationsIUS.IUSNId, this.IUSNIDs, true), this.IUSNIDs);
        //}
        #endregion


        #region "-- New Split Database Methods --"

        private void DeleteBulkRecords(string tableName, string NidColumn, string nids, bool isLanguageBased)
        {
            this.DeleteBulkRecords(tableName, NidColumn, nids, isLanguageBased, string.Empty);
        }

        private void DeleteBulkRecords(string tableName, string NidColumn, string nids, bool isLanguageBased, string whereClause)
        {
            string SqlQuery = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(nids))
                {
                    //-- Add Temp Column Mark
                    this.AddTempColumn(tableName);

                    //-- Set Mark field true which are not to delete
                    this.MarkRecords(tableName, NidColumn, nids, whereClause);

                    this.DeleteMarkedRecords(tableName);

                    //-- Remove Temp column Mark
                    this.RemoveMarkColumns(tableName);

                    if (isLanguageBased)
                    {
                        //if (tableName.ToLower().Contains(this.DestDBQueries.LanguageCode.ToLower()))
                        //{
                        //-- Delete From other language table
                        foreach (System.Data.DataRow row in this.LanguageTable.Rows)
                        {
                            string LangCode = Convert.ToString(row[Language.LanguageCode]);

                            if (LangCode.ToLower() != this.DestDBQueries.LanguageCode.Trim('_').ToLower())
                            {
                                string TargetTableName = tableName.Replace(this.DestDBQueries.LanguageCode, "_" + LangCode);
                                SqlQuery = "DELETE FROM " + TargetTableName + " Trg WHERE NOT EXISTS(SELECT * FROM " + tableName + " Src WHERE Trg." + NidColumn + "=Src." + NidColumn + ")";
                                this.DestDBConnection.ExecuteNonQuery(SqlQuery);
                            }
                        }
                        //}
                    }
                }
                else
                {
                    this.DestDBConnection.ExecuteNonQuery("DELETE FROM " + tableName);
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddTempColumn(string tableName)
        {
            string SqlQuery = string.Empty;
            //-- Add Mark Column
            SqlQuery = "Alter Table " + tableName + " Add Column " + MARK_COLUMN + " Bit";

            this.DestDBConnection.ExecuteNonQuery(SqlQuery);

        }

        private void MarkRecords(string tableName, string NidColumn, string nids)
        {
            this.MarkRecords(tableName, NidColumn, nids, string.Empty);
        }

        private void MarkRecords(string tableName, string NidColumn, string nids, string whereclause)
        {
            //-- Mark Not deletable records
            string SqlQuery = string.Empty;
            string[] NIdArray = nids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string TempNIds = string.Empty;
            int Index = 0;

            for (Index = 0; Index + 200 < NIdArray.Length; Index += 200)
            {
                TempNIds = string.Join(",", NIdArray, Index, 200);

                SqlQuery = "UPDATE " + tableName + " SET " + MARK_COLUMN + " = " + DIConnection.GetBoolValue(true) + " WHERE " + NidColumn + " IN (" + TempNIds + ")";
                if (!string.IsNullOrEmpty(whereclause))
                {
                    SqlQuery += whereclause;
                }

                this.DestDBConnection.ExecuteNonQuery(SqlQuery);
            }

            if (Index < NIdArray.Length)
            {
                TempNIds = string.Join(",", NIdArray, Index, NIdArray.Length - Index);
                SqlQuery = "UPDATE " + tableName + " SET " + MARK_COLUMN + " = " + DIConnection.GetBoolValue(true) + " WHERE " + NidColumn + " IN (" + TempNIds + ")";
                if (!string.IsNullOrEmpty(whereclause))
                {
                    SqlQuery += whereclause;
                }
                this.DestDBConnection.ExecuteNonQuery(SqlQuery);
            }

        }

        private void DeleteMarkedRecords(string tableName)
        {
            string SqlQuery = string.Empty;
            //-- Delete Records where Mark is False
            SqlQuery = "DELETE FROM " + tableName + " WHERE " + MARK_COLUMN + " =" + DIConnection.GetBoolValue(false);

            this.DestDBConnection.ExecuteNonQuery(SqlQuery);
        }

        private void RemoveMarkColumns(string tableName)
        {
            string SqlQuery = string.Empty;

            SqlQuery = "ALTER TABLE " + tableName + " DROP COLUMN " + MARK_COLUMN;

            this.DestDBConnection.ExecuteNonQuery(SqlQuery);
        }

        private string GetICNidsForAvailableIUS()
        {
            string RetVal = string.Empty;
            string SqlQuery = string.Empty;
            DataTable Table = null;

            SqlQuery = "SELECT ICIUS." + IndicatorClassificationsIUS.ICNId + " FROM " + this.DestDBQueries.TablesName.IndicatorClassificationsIUS
                + " AS ICIUS," + this.DestDBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE ICIUS."
                + IndicatorClassificationsIUS.IUSNId + "=IUS." + Indicator_Unit_Subgroup.IUSNId;

            Table = this.DestDBConnection.ExecuteDataTable(SqlQuery);

            //-- Get comma separated ICNIds
            RetVal = DIConnection.GetDelimitedValuesFromDataTable(Table, IndicatorClassificationsIUS.ICNId);

            return RetVal;
        }



        private void DeleteRecordsForArea()
        {
            DITables TempTables;
            string LangCode = string.Empty;

            try
            {
                // Delete from IUS where not exists (select * from Data where IUS.IUSNID =Data.IUSNID)
                this.DestDBConnection.ExecuteNonQuery("Delete from " + this.DestDBQueries.TablesName.IndicatorUnitSubgroup
                    + " as IUS where not exists (select * from " + this.DestDBQueries.TablesName.Data + " as D where IUS." + Indicator_Unit_Subgroup.IUSNId
                    + " =D." + Data.IUSNId + ") ");

                // delete records from IC_IUS table 
                // Delete from IC_IUS where not exists ( select * from IUS where IUS.IUSNID =ICIUS.IUSNID)
                this.DestDBConnection.ExecuteNonQuery("Delete from " + this.DestDBQueries.TablesName.IndicatorClassificationsIUS
                    + " as ICIUS where not exists ( select * from " + this.DestDBQueries.TablesName.IndicatorUnitSubgroup + " as IUS " +
                    "where IUS." + Indicator_Unit_Subgroup.IUSNId + " =ICIUS." + IndicatorClassificationsIUS.IUSNId + ")");



                // delete IC, indicator,unit & subgroups from all langauges

                foreach (DataRow row in LanguageTable.Rows)
                {
                    LangCode = row[Language.LanguageCode].ToString();
                    TempTables = new DITables(this.DestDBConnection.DIDataSetDefault(), LangCode);

                    // Delete from IC where not exists ( select * from ICIUS where ICIUS.ICNID =IC.ICNID)
                    this.DestDBConnection.ExecuteNonQuery("Delete from " + TempTables.IndicatorClassifications
                        + " as IC where not exists ( select * from " + TempTables.IndicatorClassificationsIUS + " as ICIUS " +
                        "where ICIUS." + IndicatorClassificationsIUS.ICNId + " =IC." + IndicatorClassifications.ICNId + ")");


                    // Delete from indicator where not exists ( select * from IUS where IUS.Indicator_nid =I.Indicator_Nid)
                    this.DestDBConnection.ExecuteNonQuery("Delete from " + TempTables.Indicator
                        + " as I where not exists ( select * from " + TempTables.IndicatorUnitSubgroup + " as IUS " +
                        "where IUS." + Indicator.IndicatorNId + " =I." + Indicator.IndicatorNId + ")");

                    // Delete from unit where not exists ( select * from IUS where IUS.unit_nid =U.unit_Nid)
                    this.DestDBConnection.ExecuteNonQuery("Delete from " + TempTables.Unit
                        + " as U where not exists ( select * from " + TempTables.IndicatorUnitSubgroup + " as IUS " +
                        "where IUS." + Unit.UnitNId + " =U." + Unit.UnitNId + ")");

                    // Delete from subgroupVals where not exists ( select * from IUS where IUS.subgroup_Val_nid =U.subgroup_Val_Nid)
                    this.DestDBConnection.ExecuteNonQuery("Delete from " + TempTables.SubgroupVals
                        + " as SG where not exists ( select * from " + TempTables.IndicatorUnitSubgroup + " as IUS " +
                       "where IUS." + SubgroupVals.SubgroupValNId + " =SG." + SubgroupVals.SubgroupValNId + ")");

                }

                //todo: xslt logic 
                this.DeleteFromXSLTTables();

                //-- Delete records from Tables: timePeriod, Footnotes, XSLT
                this.DeleteFromFootNoteTable();
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }


        #endregion

        #endregion

        #endregion

    }
}
