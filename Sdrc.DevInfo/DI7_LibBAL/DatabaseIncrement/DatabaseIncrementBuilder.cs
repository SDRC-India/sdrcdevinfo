using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DIColumn = DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data.Common;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DatabaseIncrement
{
    public class DatabaseIncrementBuilder : IDisposable
    {
        #region "-- Private --"

        #region "Variables"

        private string SourceDbFile = string.Empty;
        private string TargetDbFile = string.Empty;

        private Dictionary<TableName, DataTable> UpdatedDataTable = new Dictionary<TableName, DataTable>();
        private Dictionary<TableName, DataTable> InsertedDataTable = new Dictionary<TableName, DataTable>();

        #endregion

        #region "-- Enum --"

        enum TableName
        {
            Indicator,
            Unit,
            AreaLevel,
            FootNote,
            TimePeriod,
            AreaFeatureType,
            AreaMapLayer,
            AreaMapMetadata,
            Area,
            AreaMap,
            IC,
            SubgroupType,
            Subgroup,
            SubgroupVals
        }

        #endregion

        #region "Methods"

        #region "-- Merge --"

        private bool Merge()
        {
            bool RetVal = false;
            string SqlString = string.Empty;
            DataTable RecordTable = new DataTable();

            try
            {
                //merge steps
                //Step1: For All Tables  
                this.ProcessAllTables();

                //Step2 Subgroups Pending
                this.ProcessSubgroups();

                //Step3 
                this.ProcessIUS();

            }
            catch (Exception ex)
            {
                //to do uncomment throw
                throw ex;
            }

            return RetVal;
        }

        #endregion

        #region "-- Process tables --"

        private void ProcessAllTables()
        {
            //GID based Column            
            this.ProcessTables(TargetQueries.TablesName.Indicator, TableName.Indicator, DIColumn.Indicator.IndicatorNId, DIColumn.Indicator.IndicatorGId, DIColumn.Indicator.IndicatorName);
            this.ProcessTables(TargetQueries.TablesName.Unit, TableName.Unit, DIColumn.Unit.UnitNId, DIColumn.Unit.UnitGId, DIColumn.Unit.UnitName);

            this.ProcessTables(TargetQueries.TablesName.FootNote, TableName.FootNote, DIColumn.FootNotes.FootNoteNId, DIColumn.FootNotes.FootNoteGId, DIColumn.FootNotes.FootNote);
            this.ProcessTables(TargetQueries.TablesName.TimePeriod, TableName.TimePeriod, DIColumn.Timeperiods.TimePeriodNId, string.Empty, DIColumn.Timeperiods.TimePeriod);

            //To Check Parenid existance of IC if not exist then create else create -1 level records first
            this.ProcessAreas();
            this.ProcessICTable();
        }

        private void ProcessIndicator()
        {

        }

        private void ProcessSubgroups()
        {
            this.ProcessTables(TargetQueries.TablesName.SubgroupType, TableName.SubgroupType, DIColumn.SubgroupTypes.SubgroupTypeNId, DIColumn.SubgroupTypes.SubgroupTypeGID, DIColumn.SubgroupTypes.SubgroupTypeName);
            this.ProcessTables(TargetQueries.TablesName.Subgroup, TableName.Subgroup, DIColumn.Subgroup.SubgroupNId, DIColumn.Subgroup.SubgroupGId, DIColumn.Subgroup.SubgroupName);

            //to check NIDs of old
            this.ProcessTables(TargetQueries.TablesName.SubgroupVals, TableName.SubgroupVals, DIColumn.SubgroupVals.SubgroupValNId, DIColumn.SubgroupVals.SubgroupValGId, DIColumn.SubgroupVals.SubgroupVal);
        }

        private void ProcessIUS()
        {
            DataTable IUSinformation = null;
            string IndicatorNids, UnitNids, SubgroupValNids = string.Empty;

            //IndicatorNids=string.Join(',',this.InsertedNids[TableName.Indicator]);

            //foreach (KeyValuePair<TableName, string> NewIusInformation in this.InsertedNids)
            foreach (DataRow row in this.InsertedDataTable[TableName.Indicator].Rows)
            {
                //select IUS combination for
                IUSinformation = SourceConnection.ExecuteDataTable("select * from ");


                //// create new ius logic (indicatornid, unitnid & newsgnid)
                //NewIUSNid = IUSBuilder.InsertIUS(DIMonitoringIndicatorNid, DIMonitoringUnitNid, NewSGValNid, 0, 0);

                ////link new IUS with linkedICNid and update ICIUS table (devinfo database) LinkedICNids
                //ICBuilder.AddNUpdateICIUSRelation(LinkedICNid, NewIUSNid, false, ICIUSOrder);

                //NewIUSNids.Add(NewIUSNid);
            }
        }

        private void ProcessAreas()
        {
            this.ProcessTables(TargetQueries.TablesName.AreaLevel, TableName.AreaLevel, DIColumn.Area_Level.LevelNId, DIColumn.Area_Level.AreaLevel, DIColumn.Area_Level.AreaLevelName);
            this.ProcessTables(TargetQueries.TablesName.AreaFeatureType, TableName.AreaFeatureType, DIColumn.Area_Feature_Type.FeatureTypeNId, string.Empty, DIColumn.Area_Feature_Type.FeatureType);

            //AreaMapLayer, AreaMapMetadata
            this.ProcessLayersTable();

            //get and fit parent nid DIColumn.Area.AreaParentNId  
            //To Check Parenid existance of IC if not exist then create else create -1 level records first
            //this.ProcessTables(TargetQueries.TablesName.Area, TableName.Area, DIColumn.Area.AreaNId, DIColumn.Area.AreaGId, DIColumn.Area.AreaID);
            this.ProcessTablesForArea();

            this.ProcessTablesForAreaMapTable();
        }

        private void ProcessLayersTable()
        {
            //AreaMapLayer, AreaMapMetadata
            int nid = -1;
            DataTable GetLayerTable = null;
            List<string> ColumnList = null;
            string LayerNids = string.Empty;
            int NewLayerNid = 0;
            string Layername = string.Empty;
            string InsertedNids = string.Empty, UpdatedNids = string.Empty;

            //Step1: Check and create layers existance by layer name in area_map_metadata
            this.ProcessTables(TargetQueries.TablesName.AreaMapMetadata, TableName.AreaMapMetadata, DIColumn.Area_Map_Metadata.MetadataNId, DIColumn.Area_Map_Metadata.LayerName, string.Empty);

            //to do set column names for area_layers
            GetLayerTable = TargetConnection.ExecuteDataTable("select * from " + TargetQueries.TablesName.AreaMapLayer + "  where 1=0");
            ColumnList = this.GetColumns(GetLayerTable, DIColumn.Area_Map_Layer.LayerNId);

            //Step2: Get layerRecord from Layer UT_area_map_layer
            InsertedNids = string.Join(",", DICommon.GetCommaSeperatedListOfGivenColumn(this.InsertedDataTable[TableName.AreaMapMetadata], DIColumn.Area_Map_Layer.LayerNId, false, string.Empty).ToArray());
            UpdatedNids = string.Join(",", DICommon.GetCommaSeperatedListOfGivenColumn(this.UpdatedDataTable[TableName.AreaMapMetadata], DIColumn.Area_Map_Layer.LayerNId, false, string.Empty).ToArray());

            LayerNids = InsertedNids + ((string.IsNullOrEmpty(InsertedNids)) ? "" : ",") + UpdatedNids;

            GetLayerTable = TargetConnection.ExecuteDataTable(TargetQueries.Area.GetAreaMapLayerByNid(LayerNids.Trim(','), FieldSelection.Heavy));

            //Step2: Insert and Update together Use to Insert into Layer UT_area_map_layer
            foreach (DataRow rowLayer in GetLayerTable.Rows)
            {
                nid = Convert.ToInt32(rowLayer[DIColumn.Area_Map_Layer.LayerNId]);
                Layername = SourceConnection.ExecuteScalarSqlQuery("select " + DIColumn.Area_Map_Metadata.LayerName + " from " + SourceQueries.TablesName.AreaMapMetadata + " where " + DIColumn.Area_Map_Layer.LayerNId + "=" + nid).ToString();

                //insert
                NewLayerNid = this.InsertAreaMap(nid, SourceQueries.TablesName.AreaMapLayer, rowLayer[DIColumn.Area_Map_Layer.LayerSize].ToString(), rowLayer[DIColumn.Area_Map_Layer.LayerShp], rowLayer[DIColumn.Area_Map_Layer.LayerShx], rowLayer[DIColumn.Area_Map_Layer.Layerdbf], rowLayer[DIColumn.Area_Map_Layer.LayerType].ToString(), rowLayer[DIColumn.Area_Map_Layer.MinX].ToString(), rowLayer[DIColumn.Area_Map_Layer.MinY].ToString(), rowLayer[DIColumn.Area_Map_Layer.MaxX].ToString(), rowLayer[DIColumn.Area_Map_Layer.MaxY].ToString(), rowLayer[DIColumn.Area_Map_Layer.StartDate].ToString(), rowLayer[DIColumn.Area_Map_Layer.EndDate].ToString(), rowLayer[DIColumn.Area_Map_Layer.UpdateTimestamp].ToString());

                //update nid of metadata
                TargetConnection.ExecuteNonQuery("update " + SourceQueries.TablesName.AreaMapLayer + " set " + DIColumn.Area_Map_Layer.LayerNId + "=" + nid + " where " + DIColumn.Area_Map_Layer.LayerNId + "=" + NewLayerNid);
                //TargetConnection.ExecuteNonQuery("update " + SourceQueries.TablesName.AreaMapMetadata + " set " + DIColumn.Area_Map_Layer.LayerNId + "=" + NewLayerNid + " where upper(" + DIColumn.Area_Map_Metadata.LayerName + ")=" + Layername.ToUpper());
            }

            GetLayerTable.DefaultView.RowFilter = DIColumn.Area_Map_Layer.LayerNId + " in(" + InsertedNids + ")";
            this.InsertedDataTable.Add(TableName.AreaMapLayer, GetLayerTable.DefaultView.ToTable().Clone());

            GetLayerTable.DefaultView.RowFilter = DIColumn.Area_Map_Layer.LayerNId + " in(" + UpdatedNids + ")";
            this.UpdatedDataTable.Add(TableName.AreaMapLayer, GetLayerTable.DefaultView.ToTable().Clone());

            GetLayerTable.Dispose();
        }

        private void ProcessICTable()
        {
            //get and fit parent nid DIColumn.Area.AreaParentNId            
            this.ProcessTables(TargetQueries.TablesName.IndicatorClassifications, TableName.IC, DIColumn.IndicatorClassifications.ICNId, DIColumn.IndicatorClassifications.ICGId, DIColumn.IndicatorClassifications.ICName);
        }

        /// <summary>
        /// process table and insert records
        /// </summary>
        /// <param name="TableNameDefaultLang">language based table name like ut_indicator_en, ut_Unit_en</param>
        /// <param name="tableName">to append table with Gid, name like Indicator_GId, Unit_GId</param>
        private void ProcessTables(string TableNameDefaultLang, TableName tableName, string NIdColumn, string GIDColumn, string NameColumn)
        {
            DataTable ToUpdateRecord, ToInsertRecord = null;

            List<string> ColumnList = new List<string>();
            int nid = -1;

            try
            {
                //Step1: Add columns [Mapped_Gid, Mapped_Name] Set Default False 
                SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TableNameDefaultLang, Constants.Mapped_GIdColumn, "bit default False"));
                SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TableNameDefaultLang, Constants.Mapped_NameColumn, "bit default False"));

                //Step2: Update column values Set to true for both GID and Name
                if (!string.IsNullOrEmpty(GIDColumn))
                    SourceConnection.ExecuteNonQuery("Update " + TableNameDefaultLang + " set " + Constants.Mapped_GIdColumn + "=True where " + GIDColumn + " in(-2,(select Source." + GIDColumn + " from " + TableNameDefaultLang + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TableNameDefaultLang + "] target where source." + GIDColumn + "=target." + GIDColumn + "))");

                if (!string.IsNullOrEmpty(NameColumn))
                    SourceConnection.ExecuteNonQuery("Update " + TableNameDefaultLang + " set " + Constants.Mapped_NameColumn + "=True where " + NameColumn + " in(-2,(select Source." + NameColumn + " from " + TableNameDefaultLang + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TableNameDefaultLang + "] target where source." + NameColumn + "=target." + NameColumn + "))");

                ////get record to Update and insert into Database
                ToInsertRecord = SourceConnection.ExecuteDataTable("select * from " + TableNameDefaultLang + " where " + Constants.Mapped_GIdColumn + "=False and " + Constants.Mapped_NameColumn + "=False");
                ToUpdateRecord = SourceConnection.ExecuteDataTable("select * from " + TableNameDefaultLang + " where " + Constants.Mapped_GIdColumn + "=True and " + Constants.Mapped_NameColumn + "=False");

                ColumnList = this.GetColumns(ToInsertRecord, NIdColumn);

                //Step4: Insert Records New or [Update Language based tables] Update
                //to do get columns 
                foreach (DataRow row in ToInsertRecord.Rows)
                {
                    nid = -1;
                    TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TableNameDefaultLang, string.Join(",", ColumnList.ToArray()), NIdColumn, nid));
                    nid = Convert.ToInt32(this.GetCurrentInsertedID());

                    //update old nid  to new in datatable
                    //row[NIdColumn] = nid;
                    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                    {
                        TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TableNameDefaultLang, string.Join(",", ColumnList.ToArray()), NIdColumn, nid));
                    }
                }

                //Step5: Get and Set list of nids which is inserted and updated
                foreach (DataRow row in ToUpdateRecord.Rows)
                {
                    nid = Convert.ToInt32(row[NIdColumn]);
                    TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TableNameDefaultLang, NIdColumn, nid));

                    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                    {
                        TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TableNameDefaultLang, NIdColumn, nid));
                    }
                }

                UpdatedDataTable.Add(tableName, ToUpdateRecord);
                InsertedDataTable.Add(tableName, ToInsertRecord);
            }
            catch (Exception ex)
            {

            }
        }

        private void ProcessTablesForArea()
        {
            DataTable ToUpdateRecord, ToInsertRecord = null;

            List<string> ColumnList = new List<string>();
            int nid = -1;

            try
            {
                //Step1: Add columns [Mapped_Gid, Mapped_Name] Set Default False 
                SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TargetQueries.TablesName.Area, Constants.Mapped_GIdColumn, "bit default False"));
                SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TargetQueries.TablesName.Area, Constants.Mapped_NameColumn, "bit default False"));

                //Step2: Update column values Set to true for both GID and Name
                if (!string.IsNullOrEmpty(DIColumn.Area.AreaGId))
                    SourceConnection.ExecuteNonQuery("Update " + TargetQueries.TablesName.Area + " set " + Constants.Mapped_GIdColumn + "=True where " + DIColumn.Area.AreaGId + " in(-2,(select Source." + DIColumn.Area.AreaGId + " from " + TargetQueries.TablesName.Area + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TargetQueries.TablesName.Area + "] target where source." + DIColumn.Area.AreaGId + "=target." + DIColumn.Area.AreaGId + "))");

                if (!string.IsNullOrEmpty(DIColumn.Area.AreaID))
                    SourceConnection.ExecuteNonQuery("Update " + TargetQueries.TablesName.Area + " set " + Constants.Mapped_NameColumn + "=True where " + DIColumn.Area.AreaID + " in(-2,(select Source." + DIColumn.Area.AreaID + " from " + TargetQueries.TablesName.Area + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TargetQueries.TablesName.Area + "] target where source." + DIColumn.Area.AreaID + "=target." + DIColumn.Area.AreaID + "))");

                ////get record to Update and insert into Database
                ToInsertRecord = SourceConnection.ExecuteDataTable("select * from " + TargetQueries.TablesName.Area + " where " + Constants.Mapped_GIdColumn + "=False and " + Constants.Mapped_NameColumn + "=False");
                ToUpdateRecord = SourceConnection.ExecuteDataTable("select * from " + TargetQueries.TablesName.Area + " where " + Constants.Mapped_GIdColumn + "=True and " + Constants.Mapped_NameColumn + "=False");

                ColumnList = this.GetColumns(ToInsertRecord, DIColumn.Area.AreaNId);

                //Step4: Insert Records New or [Update Language based tables] Update
                //to do get columns 
                foreach (DataRow row in ToInsertRecord.Rows)
                {
                    nid = -1;
                    TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TargetQueries.TablesName.Area, string.Join(",", ColumnList.ToArray()), DIColumn.Area.AreaNId, nid));
                    nid = Convert.ToInt32(this.GetCurrentInsertedID());

                    //update old nid  to new in datatable
                    //row[DIColumn.Area.AreaNId] = nid;
                    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                    {
                        TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TargetQueries.TablesName.Area, string.Join(",", ColumnList.ToArray()), DIColumn.Area.AreaNId, nid));
                    }
                }

                //Step5: Get and Set list of nids which is inserted and updated
                foreach (DataRow row in ToUpdateRecord.Rows)
                {
                    nid = Convert.ToInt32(row[DIColumn.Area.AreaNId]);
                    TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TargetQueries.TablesName.Area, DIColumn.Area.AreaNId, nid));

                    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                    {
                        TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TargetQueries.TablesName.Area, DIColumn.Area.AreaNId, nid));
                    }
                }

                UpdatedDataTable.Add(TableName.Area, ToUpdateRecord);
                InsertedDataTable.Add(TableName.Area, ToInsertRecord);
            }
            catch (Exception ex)
            {

            }
        }

        private void ProcessTablesForAreaMapTable()
        {
            DataTable ToUpdateRecord, ToInsertRecord = null;

            List<string> ColumnList = new List<string>();
            int nid = -1;

            try
            {
                //Step4: Insert Records check is any layer added [TODO and think for new association]
                foreach (DataRow row in this.InsertedDataTable[TableName.Area].Rows)
                {
                    TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TargetQueries.TablesName.AreaMap, string.Join(",", ColumnList.ToArray()), DIColumn.Area.AreaNId, nid));
                }

                ////Step1: Add columns [Mapped_Gid, Mapped_Name] Set Default False 
                //SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TargetQueries.TablesName.AreaMap, Constants.Mapped_GIdColumn, "bit default False"));
                //SourceConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement.Create.CreateColumn(TargetQueries.TablesName.AreaMap, Constants.Mapped_NameColumn, "bit default False"));

                ////Step2: Update column values Set to true for both GID and Name
                //if (!string.IsNullOrEmpty(DIColumn.Area.AreaGId))
                //    SourceConnection.ExecuteNonQuery("Update " + TargetQueries.TablesName.AreaMap + " set " + Constants.Mapped_GIdColumn + "=True where " + DIColumn.Area_Map.AreaNId + " in(-2,(select Source." + DIColumn.Area_Map.AreaNId + " from " + TargetQueries.TablesName.Area + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TargetQueries.TablesName.AreaMap + "] target where source." + DIColumn.Area_Map.AreaNId + "=target." + DIColumn.Area_Map.AreaNId + "))");

                //if (!string.IsNullOrEmpty(DIColumn.Area.AreaID))
                //    SourceConnection.ExecuteNonQuery("Update " + TargetQueries.TablesName.AreaMap + " set " + Constants.Mapped_NameColumn + "=True where " + DIColumn.Area_Map.LayerNId + " in(-2,(select Source." + DIColumn.Area_Map.LayerNId + " from " + TargetQueries.TablesName.AreaMap + " source, [MS Access;Database=" + this.TargetDbFile + ";pwd=" + Constants.DBPassword + ";].[" + TargetQueries.TablesName.AreaMap + "] target where source." + DIColumn.Area_Map.LayerNId + "=target." + DIColumn.Area_Map.LayerNId + "))");

                //////get record to Update and insert into Database
                //ToInsertRecord = SourceConnection.ExecuteDataTable("select * from " + TargetQueries.TablesName.AreaMap + " where " + Constants.Mapped_GIdColumn + "=False and " + Constants.Mapped_NameColumn + "=False");
                //ToUpdateRecord = SourceConnection.ExecuteDataTable("select * from " + TargetQueries.TablesName.AreaMap + " where " + Constants.Mapped_GIdColumn + "=True and " + Constants.Mapped_NameColumn + "=False");

                //ColumnList = this.GetColumns(ToInsertRecord, DIColumn.Area.AreaNId);                

                ////to do get columns 
                //foreach (DataRow row in ToInsertRecord.Rows)
                //{
                //    nid = -1;
                //    TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TargetQueries.TablesName.AreaMap, string.Join(",", ColumnList.ToArray()), DIColumn.Area.AreaNId, nid));
                //    nid = Convert.ToInt32(this.GetCurrentInsertedID());

                //    //update old nid  to new in datatable
                //    //row[DIColumn.Area.AreaNId] = nid;
                //    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                //    {
                //        TargetConnection.ExecuteNonQuery(this.CreateInsertQueryForAll(row, TargetQueries.TablesName.AreaMap, string.Join(",", ColumnList.ToArray()), DIColumn.Area.AreaNId, nid));
                //    }
                //}

                ////Step5: Get and Set list of nids which is inserted and updated
                //foreach (DataRow row in ToUpdateRecord.Rows)
                //{
                //    nid = Convert.ToInt32(row[DIColumn.Area.AreaNId]);
                //    TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TargetQueries.TablesName.Area, DIColumn.Area.AreaNId, nid));

                //    foreach (DataRow LangRow in TargetConnection.DILanguages(TargetConnection.DIDataSetDefault()).Rows)
                //    {
                //        TargetConnection.ExecuteNonQuery(this.CreateUpdateQueryForAll(row, TargetQueries.TablesName.Area, DIColumn.Area.AreaNId, nid));
                //    }
                //}

                //UpdatedDataTable.Add(TableName.AreaMap, ToUpdateRecord);
                InsertedDataTable.Add(TableName.AreaMap, ToInsertRecord);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region "-- Query --"

        public string GetCurrentInsertedID()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT @@identity ";

            return RetVal;
        }

        private int InsertAreaMap(int layerNid, string tableName, string LayerSize, object LayerShpbuffer, object LayerShxbuffer, object Layerdbfbuffer, string LayerType, string MinX, string MinY, string MaxX, string MaxY, string StartDate, string EndDate, string UpdateTimestamp)
        {
            int RetVal = 0;

            string SqlQuery = string.Empty;
            DbCommand Command = TargetConnection.GetCurrentDBProvider().CreateCommand();
            DbParameter ParameterLayerShp;
            DbParameter ParameterLayerShx;
            DbParameter ParameterLayerdbf;

            DbParameter LayerSizeParameter;
            DbParameter LayerTypeParameter;
            DbParameter MinXParameter;
            DbParameter MinYParameter;
            DbParameter MaxXParameter;
            DbParameter MaxYParameter;
            DbParameter StartDateParameter;
            DbParameter EndDateParameter;
            DbParameter UpdateTimestampParameter;

            try
            {
                //delete first then insert
                if (layerNid > 0)
                {
                    TargetConnection.ExecuteNonQuery("delete from " + tableName + " where " + DIColumn.Area_Map_Layer.LayerNId + "=" + layerNid);
                }

                SqlQuery = "INSERT INTO " + tableName + "(" + DIColumn.Area_Map_Layer.LayerSize + "," + DIColumn.Area_Map_Layer.LayerShp + "," + DIColumn.Area_Map_Layer.LayerShx + "," + DIColumn.Area_Map_Layer.Layerdbf + "," + DIColumn.Area_Map_Layer.LayerType + "," + DIColumn.Area_Map_Layer.MinX + "," + DIColumn.Area_Map_Layer.MinY + "," + DIColumn.Area_Map_Layer.MaxX
                    + "," + DIColumn.Area_Map_Layer.MaxY + "," + DIColumn.Area_Map_Layer.StartDate + "," + DIColumn.Area_Map_Layer.EndDate + "," + DIColumn.Area_Map_Layer.UpdateTimestamp + ")"
                    + " VALUES(@LayerSize,@LayerShp,@LayerShx,@Layerdbf,@LayerType,@MinX,@MinY,@MaxX,@MaxY,@StartDate,@EndDate,@UpdateTimestamp)";

                //-- Change for Online Database
                //  SqlQuery = SqlQuery.Replace("?,?,?", "@LayerShp,@LayerShx,@Layerdbf");

                Command.Connection = TargetConnection.GetConnection();
                Command.CommandText = SqlQuery;
                Command.CommandType = CommandType.Text;

                ParameterLayerShp = TargetConnection.GetCurrentDBProvider().CreateParameter();

                LayerSizeParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                LayerTypeParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                MinXParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                MinYParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                MaxXParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                MaxYParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                StartDateParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                EndDateParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();
                UpdateTimestampParameter = TargetConnection.GetCurrentDBProvider().CreateParameter();

                //LayerSize
                LayerSizeParameter.ParameterName = "@LayerSize";
                LayerSizeParameter.DbType = DbType.String;
                LayerSizeParameter.Value = LayerSize.ToString();
                Command.Parameters.Add(LayerSizeParameter);

                ParameterLayerShp.ParameterName = "@LayerShp";
                //the name used in the query for the parameter 
                ParameterLayerShp.DbType = DbType.Binary;
                //set the database type 
                ParameterLayerShp.Value = LayerShpbuffer;
                ParameterLayerShp.Size = ((byte[])LayerShpbuffer).Length;
                Command.Parameters.Add(ParameterLayerShp);

                ParameterLayerShx = TargetConnection.GetCurrentDBProvider().CreateParameter();
                ParameterLayerShx.ParameterName = "@LayerShx";
                ParameterLayerShx.DbType = DbType.Binary;
                ParameterLayerShx.Value = LayerShxbuffer;
                ParameterLayerShx.Size = ((byte[])LayerShxbuffer).Length;
                Command.Parameters.Add(ParameterLayerShx);

                ParameterLayerdbf = TargetConnection.GetCurrentDBProvider().CreateParameter();
                ParameterLayerdbf.ParameterName = "@Layerdbf";
                ParameterLayerdbf.DbType = DbType.Binary;
                ParameterLayerdbf.Value = Layerdbfbuffer;
                ParameterLayerdbf.Size = ((byte[])Layerdbfbuffer).Length;
                Command.Parameters.Add(ParameterLayerdbf);

                //LayerType
                LayerTypeParameter.ParameterName = "@LayerType";
                LayerTypeParameter.DbType = DbType.Int32;
                LayerTypeParameter.Value = Convert.ToInt32(LayerType);
                Command.Parameters.Add(LayerTypeParameter);

                //MinX
                MinXParameter.ParameterName = "@MinX";
                MinXParameter.DbType = DbType.Double;
                MinXParameter.Value = Convert.ToDouble(MinX);
                Command.Parameters.Add(MinXParameter);
                //MinY
                MinYParameter.ParameterName = "@MinY";
                MinYParameter.DbType = DbType.Double;
                MinYParameter.Value = Convert.ToDouble(MinY);
                Command.Parameters.Add(MinYParameter);

                //MaxX
                MaxXParameter.ParameterName = "@MaxX";
                MaxXParameter.DbType = DbType.Double;
                MaxXParameter.Value = Convert.ToDouble(MaxX);
                Command.Parameters.Add(MaxXParameter);
                //MaxY
                MaxYParameter.ParameterName = "@MaxY";
                MaxYParameter.DbType = DbType.Double;
                MaxYParameter.Value = Convert.ToDouble(MaxY);
                Command.Parameters.Add(MaxYParameter);

                StartDateParameter.ParameterName = "@StartDate";
                StartDateParameter.DbType = DbType.DateTime;
                StartDateParameter.Value = Convert.ToDateTime(StartDate);
                Command.Parameters.Add(StartDateParameter);

                EndDateParameter.ParameterName = "@EndDate";
                EndDateParameter.DbType = DbType.DateTime;
                EndDateParameter.Value = Convert.ToDateTime(EndDate);
                Command.Parameters.Add(EndDateParameter);

                UpdateTimestampParameter.ParameterName = "@UpdateTimestamp";
                UpdateTimestampParameter.DbType = DbType.DateTime;
                UpdateTimestampParameter.Value = Convert.ToDateTime(UpdateTimestamp);
                Command.Parameters.Add(UpdateTimestampParameter);


                //-- add the parameter to the command 
                //cmd.ExecuteNonQuery();
                int AffectedRow = Command.ExecuteNonQuery();

                //-- this saves the image to the database 
                RetVal = Convert.ToInt32(TargetConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

            }
            catch (Exception ex)
            {
                // return -111 , when database/template file size exceeeds 1.90 GB
                RetVal = -111;
            }
            finally
            {
                if (Command != null)
                {
                    Command.Dispose();
                }
            }

            return RetVal;
        }

        private string CreateInsertQueryForAll(DataRow row, string tableName, string Columns, string NidColumn, int nid)
        {
            Type ColumnType = null;
            string ColumnName = string.Empty;
            StringBuilder RetVal = new StringBuilder();
            string Query = string.Empty;

            DataTable DataToExportTable = row.Table;

            RetVal.Append("INSERT INTO " + tableName);
            RetVal.Append("(" + Columns + ")" + " ");

            RetVal.Append("VALUES(");

            //add values of insert statement into Query
            for (int colIndex = 0; colIndex < DataToExportTable.Columns.Count; colIndex++)
            {
                ColumnName = DataToExportTable.Columns[colIndex].ColumnName;
                if (ColumnName.ToUpper() != NidColumn && nid <= 0)
                {
                    continue;
                }

                ColumnType = DataToExportTable.Columns[colIndex].DataType;

                if (ColumnType == typeof(string)) //for  type string
                {
                    RetVal.Append("'" + DI_LibBAL.Utility.DICommon.RemoveQuotes(row[colIndex].ToString()) + "',");
                    continue;
                }
                else if (ColumnType == typeof(Int16))        //sqlite Boolean column datatype is not bool but Int16 
                {
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append("" + Convert.ToBoolean(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append("" + Convert.ToBoolean(row[colIndex]).ToString() + ",");
                    }
                    continue;
                }
                else if (ColumnType == typeof(object)) //In sqlite Number datatype return as object pass it ass integer
                {
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append("" + Convert.ToString(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append("" + row[colIndex].ToString() + ",");
                    }

                }
                else if (ColumnType == typeof(DateTime)) //for  type Date
                {
                    RetVal.Append("'" + row[colIndex].ToString() + "',");
                }
                else
                {
                    //In Last Numbers
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append(Convert.ToString(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append(row[colIndex].ToString() + ",");
                    }
                }
            }

            Query = RetVal.ToString().Trim(',');
            RetVal = RetVal.Remove(0, RetVal.Length);
            RetVal.Append(Query);

            RetVal.Append(")");

            return RetVal.ToString();
        }

        private string CreateUpdateQueryForAll(DataRow row, string tableName, string NidColumn, int nid)
        {
            Type ColumnType = null;
            string ColumnName = string.Empty;
            StringBuilder RetVal = new StringBuilder();
            string Query = string.Empty;

            DataTable DataToExportTable = row.Table;

            RetVal.Append("Update " + tableName + " SET");

            //add values of insert statement into Query
            for (int colIndex = 0; colIndex < DataToExportTable.Columns.Count; colIndex++)
            {
                ColumnName = DataToExportTable.Columns[colIndex].ColumnName;
                if (ColumnName.ToUpper() == NidColumn.ToUpper())
                {
                    continue;
                }

                ColumnType = DataToExportTable.Columns[colIndex].DataType;

                RetVal.Append(" " + ColumnName + "=");

                if (ColumnType == typeof(string)) //for  type string
                {
                    RetVal.Append("'" + DI_LibBAL.Utility.DICommon.RemoveQuotes(row[colIndex].ToString()) + "',");
                    continue;
                }
                else if (ColumnType == typeof(Int16))        //sqlite Boolean column datatype is not bool but Int16 
                {
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append("" + Convert.ToBoolean(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append("" + Convert.ToBoolean(row[colIndex]).ToString() + ",");
                    }
                    continue;
                }
                else if (ColumnType == typeof(object)) //In sqlite Number datatype return as object pass it ass integer
                {
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append("" + Convert.ToString(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append("" + row[colIndex].ToString() + ",");
                    }

                }
                else if (ColumnType == typeof(DateTime)) //for  type Date
                {
                    RetVal.Append("'" + row[colIndex].ToString() + "',");
                }
                else
                {
                    //In Last Numbers
                    if (row[colIndex].ToString() == "")
                    {
                        RetVal.Append(Convert.ToString(0).ToString() + ",");
                    }
                    else
                    {
                        RetVal.Append(row[colIndex].ToString() + ",");
                    }
                }
            }


            Query = RetVal.ToString().Trim(',');
            RetVal = RetVal.Remove(0, RetVal.Length);
            RetVal.Append(Query);

            RetVal.Append(" where " + NidColumn + "=" + nid);


            return RetVal.ToString();
        }

        #endregion

        private List<string> GetColumns(DataTable ToInsertRecord, string NIdColumn)
        {
            List<string> RetVal = new List<string>();

            foreach (DataColumn col in ToInsertRecord.Columns)
            {
                if (col.ColumnName.ToUpper() != NIdColumn.ToUpper())
                    RetVal.Add(col.ColumnName);
            }

            return RetVal;
        }

        private void AddTableToCollection()
        {

        }

        #endregion

        #endregion

        #region "-- public --"

        #region "Properties"

        private DIConnection _SourceConnection;

        public DIConnection SourceConnection
        {
            get { return _SourceConnection; }
            set { _SourceConnection = value; }
        }

        private DIQueries _SourceQueries;

        public DIQueries SourceQueries
        {
            get { return _SourceQueries; }
            set { _SourceQueries = value; }
        }

        private DIConnection _TargetConnection;

        public DIConnection TargetConnection
        {
            get { return _TargetConnection; }
            set { _TargetConnection = value; }
        }

        private DIQueries _TargetQueries;

        public DIQueries TargetQueries
        {
            get { return _TargetQueries; }
            set { _TargetQueries = value; }
        }


        #endregion

        #region "New/Dispose"

        public DatabaseIncrementBuilder(string sourceDBLocation, string TargetDBLocation)
        {
            string DataPrefix, LangCode = string.Empty;
            SourceConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, sourceDBLocation, string.Empty, "unitednations2000");
            TargetConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TargetDBLocation, string.Empty, "unitednations2000");

            DataPrefix = SourceConnection.DIDataSetDefault();
            LangCode = SourceConnection.DILanguageCodeDefault(DataPrefix);
            SourceQueries = new DIQueries(DataPrefix, LangCode);

            DataPrefix = TargetConnection.DIDataSetDefault();
            LangCode = TargetConnection.DILanguageCodeDefault(DataPrefix);
            TargetQueries = new DIQueries(DataPrefix, LangCode);

            this.SourceDbFile = sourceDBLocation;
            this.TargetDbFile = TargetDBLocation;
        }

        public void Dispose()
        {
            if (this._SourceConnection != null)
                this._SourceConnection.Dispose();

            if (this._TargetConnection != null)
                this._TargetConnection.Dispose();
        }

        #endregion

        #region "Methods"

        public bool MergeDatabase()
        {
            bool RetVal = false;

            try
            {
                RetVal = this.Merge();
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
