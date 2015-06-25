using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.IO;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build area according to area information .if parent of area is not exist than first create parent of area and insert it into database.
    /// according to parent area information create area.
    /// </summary>
    public class AreaBuilder : ProcessEventCreator
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;
        private const string AreaTableName = "Area";


        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Insert area record Into database
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(AreaInfo areaInfo)
        {
            return this.InsertIntoDatabase(areaInfo.Name, areaInfo.ID, areaInfo.GID, areaInfo.Level, areaInfo.Parent.Nid, areaInfo.AreaMap, areaInfo.AreaBlock, areaInfo.IsGlobal);
        }



        /// <summary>
        /// check area record intocollection and database  
        /// </summary>
        /// <param name="areaID">Area ID </param>
        /// <returns>AreaNid</returns>

        private int CheckAreaExists(string areaID)
        {
            int RetVal = 0;


            //Step 1: check in area collection by areaID
            RetVal = this.CheckAreaInCollection(areaID);

            //Step 2: if doesnt exists, then check into DataBase
            if (RetVal <= 0)
            {
                RetVal = this.GetAreaNidByAreaID(areaID);
            }

            return RetVal;
        }
        /// <summary>
        /// check Area record into Collection  
        /// </summary>
        /// <param name="areaID">Area ID </param>
        /// <returns>Area Nid </returns>

        private int CheckAreaInCollection(string areaID)
        {
            int RetVal = 0;
            try
            {
                // check in area collection by areaID
                if (this.Areas.ContainsKey(areaID))
                {
                    //If Collection Contains Area ID Then set RetVal to AreaNid
                    RetVal = this.Areas[areaID].Nid;
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return RetVal;

        }
        /// <summary>
        ///  Create area if  not exist Into database 
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo</param>
        /// <returns>Area Nid</returns>

        private int CreateArea(AreaInfo areaInfo)
        {
            int RetVal = 0;
            int ParentNId = 0;
            int ParentLevel = 1;
            string areaparentID = string.Empty;
            string areanid = string.Empty;


            //Step1 :Check parent area exists or not by area id only if ParentNid<>-1 and ParentID is not empty or null
            if (string.IsNullOrEmpty(areaInfo.Parent.ID) & areaInfo.Nid != -1)
            {
                ParentNId = -1;
                areaInfo.Parent.Level = 0;
            }
            else
            {
                ParentNId = this.CheckAreaExists(areaInfo.Parent.ID);

                if (ParentNId <= 0)
                {
                    //set ParentLevel to 1 
                    if (areaInfo.Parent.Level == 0)
                    {
                        areaInfo.Parent.Level = ParentLevel;
                    }

                    // create gid for parent area
                    if (string.IsNullOrEmpty(areaInfo.Parent.GID))
                    {
                        areaInfo.Parent.GID = Guid.NewGuid().ToString();
                    }

                    //if parent doesnt exist then insert it into database .
                    if (this.InsertIntoDatabase(areaInfo.Parent.Name, areaInfo.Parent.ID, areaInfo.Parent.GID, areaInfo.Parent.Level, -1))
                    {
                        areaparentID = areaInfo.Parent.ID;
                        ParentNId = this.GetAreaNidByAreaID(areaparentID);
                    }
                }
            }

            //insert area into database
            if (ParentNId > 0 || ParentNId == -1)
            {
                //udpate parentNid in areaInfo object
                areaInfo.Parent.Nid = ParentNId;

                //get parent level if it is null or zero & parent nid >0
                if (areaInfo.Parent.Level == 0 & ParentNId > 0)
                {
                    //get parent area level and update areaRecord object
                    areaInfo.Parent.Level = this.GetAreaLevelByAreaID(areaInfo.Parent.ID);
                }

                //set area level 
                if (areaInfo.Level == 0)
                {
                    areaInfo.Level = areaInfo.Parent.Level + 1;
                }

                //create gid for area
                if (string.IsNullOrEmpty(areaInfo.GID))
                {
                    areaInfo.GID = Guid.NewGuid().ToString();
                }

                if (this.InsertIntoDatabase(areaInfo))
                {
                    areanid = areaInfo.ID;
                    RetVal = this.GetAreaNidByAreaID(areanid);
                }

            }

            //update areainfo object
            areaInfo.Nid = RetVal;

            return RetVal;
        }
        /// <summary>
        /// Add area record into collection.
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo </param>
        private void AddAreaIntoCollection(AreaInfo areaInfo)
        {
            if (!this.Areas.ContainsKey(areaInfo.ID))
            {
                this.Areas.Add(areaInfo.ID, areaInfo);
            }
        }

        /// <summary>
        /// Create area and parent ID
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo</param>
        private void CreateAreaIDNParentID(AreaInfo areaInfo)
        {
            //set area id to areaname, if empty
            if (string.IsNullOrEmpty(areaInfo.ID))
            {
                areaInfo.ID = areaInfo.Name;
            }

            //set parent area id to parent area name, if empty
            if (string.IsNullOrEmpty(areaInfo.Parent.ID))
            {
                areaInfo.Parent.ID = areaInfo.Parent.Name;
            }
        }

        private void RemoveAreaMapAssociations(int areaNId, List<int> NewAddedLayerNidList, bool removeFeaturesAlso)
        {
            string LayerNIds = string.Empty;
            string LayerNIds1 = string.Empty;
            DataTable Table;

            // -- Area_Map : DELETE Area from Area_Map - This will remove all it's features as well 
            // -- STEP 1 - Get all Layer_NIds from Area_Map which are not associated with any other Area _ 
            // to delete the Layers from Area_Map_Layer 
            // -- since 1 Area can have features associated, therefore there will be multiple Layers associated with the single Area 

            if (removeFeaturesAlso)
            {
                // -- Get All Layers for this Area 
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapByAreaNIds(areaNId.ToString(), true));
            }
            else
            {
                // -- Get Layer for this Area only. This Area might have features. In this case Layers associated with those features will not be retrieved 
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapByAreaNIds(areaNId.ToString(), false));
            }

            foreach (DataRow Row in Table.Rows)
            {
                if (!NewAddedLayerNidList.Contains(Convert.ToInt32(Row[Area_Map_Layer.LayerNId])))
                {
                    LayerNIds += "," + Row[Area_Map_Layer.LayerNId].ToString();
                }
            }
            Table.Dispose();
            Table = null;

            if (LayerNIds.Length > 0)
            {
                LayerNIds1 = LayerNIds + ",";
                LayerNIds = Strings.Mid(LayerNIds, 2);

                // -- Get Those Layer NIDs which do not have reference to any other areas 
                using (DataTable Table1 = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetLayerNIDsExcludeArea(areaNId, LayerNIds)))
                {
                    if (Table1.Rows.Count > 0)
                    {
                        foreach (DataRow Row in Table1.Rows)
                        {
                            LayerNIds1 = Microsoft.VisualBasic.Strings.Replace(LayerNIds1, "," + Row[Area_Map_Layer.LayerNId].ToString() + ",", ",", 1, -1, CompareMethod.Binary).ToString();
                        }

                        if (Strings.Mid(LayerNIds1, 1, 1) == ",")
                        {
                            LayerNIds = Strings.Mid(LayerNIds1, 2);
                        }

                        if (LayerNIds.Length > 0)
                        {
                            if (Strings.Mid(LayerNIds, LayerNIds.Length - 1, 1) == ",")
                                LayerNIds = Strings.Mid(LayerNIds, 1, LayerNIds.Length - 1);
                        }
                    }
                }
            }



            if (LayerNIds.Length > 0)
            {
                // -- STEP 2 - Delete Area from Area_Map 
                // -- STEP 3 - Delete Layers from Area_Map_Layer 
                // -- STEP 4 - Delete Layers from Area_Map_Metadata 
                MapBuilder oMapBuilder = new MapBuilder(this.DBConnection, this.DBQueries);
                oMapBuilder.DeleteMap(LayerNIds);
            }
        }




        #region "-- Import Process--"

        private int CreateAreaForImportProcess(AreaInfo areaInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();

            try
            {
                areaInfo.Name = DICommon.RemoveQuotes(areaInfo.Name);
                if (areaInfo.Parent != null)
                {
                    areaInfo.Parent.Name = DICommon.RemoveQuotes(areaInfo.Name);
                }


                //Step 1: check existence of area (areaID)
                RetVal = this.CheckAreaExists(areaInfo.ID);


                if (RetVal > 0)
                {
                    DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                    // update area info
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateArea(areaInfo.Parent.Nid, areaInfo.ID, areaInfo.Name, areaInfo.GID, areaInfo.Level, areaInfo.AreaMap, areaInfo.AreaBlock, areaInfo.IsGlobal));
                }
                else
                {
                    if (this.InsertIntoDatabase(areaInfo))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }


                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Area, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        private int CreateAreaChainFromExtDB(int sourceAreaNId, int sourceParentNId, string sourceAreaID, string sourceAreaName, string sourceAreaGId, int sourceAreaLevel, string sourceAreaMap, string sourceAreaBlock, bool isGlobal, DIQueries sourceDBQueries,
    DIConnection sourceDBConnection, Dictionary<string, int> alreadyImportedAreas, bool importMapAlso)
        {
            int RetVal;

            int NewParentNId;
            string TargetParent = string.Empty;
            string Map = string.Empty;
            string Block = string.Empty;
            DataRow Row;
            AreaInfo AreaInfoObject = new AreaInfo();

            try
            {
                // check area is already imported or not. If already then get area nid
                if (alreadyImportedAreas.ContainsKey(sourceAreaID))
                {
                    RetVal = alreadyImportedAreas[sourceAreaID];
                }
                else
                {

                    // -- STEP 1: If the Parent NID is -1 then create the area at the root 
                    if (sourceParentNId == -1)
                    {

                        // -- Create the Area 
                        AreaInfoObject.Parent = new AreaInfo();
                        AreaInfoObject.Parent.Nid = sourceParentNId;
                        AreaInfoObject.Nid = sourceAreaNId;
                        AreaInfoObject.Name = sourceAreaName;
                        AreaInfoObject.ID = sourceAreaID;
                        AreaInfoObject.GID = sourceAreaGId;
                        AreaInfoObject.IsGlobal = isGlobal;
                        AreaInfoObject.Level = sourceAreaLevel;
                        AreaInfoObject.AreaBlock = sourceAreaBlock;
                        AreaInfoObject.AreaMap = sourceAreaMap;
                        RetVal = this.CreateAreaForImportProcess(AreaInfoObject, sourceAreaNId, sourceDBQueries, sourceDBConnection);

                        //import area maps
                        if (importMapAlso)
                        {
                            this.ImportAreaMaps(sourceAreaNId.ToString(), 1, sourceDBConnection, sourceDBQueries);
                        }

                    }


                    else
                    {
                        // -- STEP 2: If the Parent is not -1 then check for the existence of the Parent and then create the Area 
                        // -- STEP 2.1: If the Parent Exists then create the Area under that parent 
                        // -- STEP 2.2: If the Parent does not Exist then create the Parent first and then the Area under that parent 

                        // -- get the parent from the source database 
                        using (DataTable TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetArea(FilterFieldType.NId, sourceParentNId.ToString())))
                        {
                            Row = TempTable.Rows[0];
                            {
                                Block = string.Empty;
                                Map = string.Empty;

                                if (!Information.IsDBNull(Row[Area.AreaMap]))
                                {
                                    Map = Convert.ToString(Row[Area.AreaMap]);
                                }
                                if (!Information.IsDBNull(Row[Area.AreaBlock]))
                                {
                                    Block = Convert.ToString(Row[Area.AreaBlock]);
                                }


                                NewParentNId = this.CreateAreaChainFromExtDB(
                                    Convert.ToInt32(Row[Area.AreaNId]),
                                    Convert.ToInt32(Row[Area.AreaParentNId]), Row[Area.AreaID].ToString(),
                                    Row[Area.AreaName].ToString(), Row[Area.AreaGId].ToString(),
                                    Convert.ToInt32(Row[Area.AreaLevel]), Map, Block,
                                    Convert.ToBoolean(Row[Area.AreaGlobal]), sourceDBQueries,
                                    sourceDBConnection, alreadyImportedAreas, importMapAlso);
                            }
                        }

                        // -- Create the Child Row 
                        AreaInfoObject.Parent = new AreaInfo();
                        AreaInfoObject.Parent.Nid = NewParentNId;
                        AreaInfoObject.Nid = sourceAreaNId;
                        AreaInfoObject.Name = sourceAreaName;
                        AreaInfoObject.ID = sourceAreaID;
                        AreaInfoObject.GID = sourceAreaGId;
                        AreaInfoObject.IsGlobal = isGlobal;
                        AreaInfoObject.Level = sourceAreaLevel;
                        AreaInfoObject.AreaBlock = sourceAreaBlock;
                        AreaInfoObject.AreaMap = sourceAreaMap;

                        RetVal = this.CreateAreaForImportProcess(AreaInfoObject, sourceAreaNId, sourceDBQueries, sourceDBConnection);

                        //import area maps
                        if (importMapAlso)
                        {
                            this.ImportAreaMaps(sourceAreaNId.ToString(), 1, sourceDBConnection, sourceDBQueries);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }


            // add new area_nid into ImportedArea list
            if (RetVal > 0)
            {
                if (!alreadyImportedAreas.ContainsKey(sourceAreaID))
                {
                    alreadyImportedAreas.Add(sourceAreaID, RetVal);
                }
            }

            return RetVal;
        }

        private void UpdateBlockAreas(string sourceAreaId, string sourceAreaBlock, DIQueries sourceDBQueries, DIConnection sourceDBConnection)
        {
            DataTable TempTable;
            string SourceAreaID = string.Empty;
            string AreaNIds = string.Empty;
            int Index;

            if (!string.IsNullOrEmpty(sourceAreaBlock.Trim()))
            {
                // -- STEP 1: Get the ID from the source Database against the Block IDs 
                TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetArea(FilterFieldType.NId, sourceAreaBlock));
                {
                    SourceAreaID = "";
                    for (Index = 0; Index <= TempTable.Rows.Count - 1; Index++)
                    {
                        if (Index == 0)
                        {
                            SourceAreaID = "'" + DICommon.RemoveQuotes(TempTable.Rows[Index][Area.AreaID].ToString()) + "'";
                        }
                        else
                        {
                            SourceAreaID = SourceAreaID + ",'" + DICommon.RemoveQuotes(TempTable.Rows[Index][Area.AreaID].ToString()) + "'";
                        }
                    }
                }

                // -- STEP 2: Get the Area NIds from the Target Database Against the Area ID Retrieved from the Source Database 
                if (SourceAreaID.Length > 0)
                {
                    TempTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetArea(FilterFieldType.ID, SourceAreaID));
                    {
                        AreaNIds = "";
                        for (Index = 0; Index <= TempTable.Rows.Count - 1; Index++)
                        {
                            if (Index == 0)
                            {
                                AreaNIds = TempTable.Rows[Index][Area.AreaNId].ToString();
                            }
                            else
                            {
                                AreaNIds = AreaNIds + "," + TempTable.Rows[Index][Area.AreaNId].ToString();
                            }
                        }
                    }

                    // -- STEP 3: Update the Target Database Area against the GID sent for the Block Areas 
                    if (AreaNIds.Length > 0)
                    {
                        // -- Update the Record in the Target Database with the 
                        this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateAreaBlocks(AreaNIds, sourceAreaId));
                    }
                }

                // -- Dispose the Data Table object 
                TempTable.Dispose();
            }
        }

        #endregion

        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns area colleciton in key,pair format. Key is area id  and value is object of AreaInfo.
        /// </summary>
        internal Dictionary<string, AreaInfo> Areas = new Dictionary<string, AreaInfo>();

        /// <summary>
        /// Returns list of inserted area levels. 
        /// </summary>
        internal List<int> InsertedAreaLevel = new List<int>();

        #endregion
        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public AreaBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        #region "-- Map Process --"

        public int ImportAreaFrmMappedArea(AreaInfo srcAreaInfo, int NidInSourceDB, int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = NidInTrgDB;
            Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();

            try
            {
                srcAreaInfo.Name = DICommon.RemoveQuotes(srcAreaInfo.Name);
                if (srcAreaInfo.Parent != null)
                {
                    srcAreaInfo.Parent.Name = DICommon.RemoveQuotes(srcAreaInfo.Name);
                }

                if (RetVal > 0)
                {
                    // update area info
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateArea(srcAreaInfo.Parent.Nid, srcAreaInfo.ID, srcAreaInfo.Name, srcAreaInfo.GID, srcAreaInfo.Level, srcAreaInfo.AreaMap, srcAreaInfo.AreaBlock, srcAreaInfo.IsGlobal));



                    //update/insert icon 
                    DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Area, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        #endregion

        /// <summary>
        /// Returns true/false. True if area is duplicate
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="areaID"></param>
        /// <param name="areaNId">if not available then send -99</param>
        /// <param name="areaParentNId"></param>
        /// <returns></returns>
        public bool IsDuplicateArea(string areaName, string areaID, int areaNId, int areaParentNId)
        {
            bool RetVal = false;
            int NIdInDB = -1;
            DataTable AreaTable;
            string SqlQuery = string.Empty;

            try
            {

                // check by area id 
                NIdInDB = this.GetAreaNidByAreaID(areaID);


                // if areaNId is lessthan zero 
                if (areaNId <= 0)
                {

                    // check by name
                    if (NIdInDB <= 0)
                    {
                        SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.Name, "'" + areaName + "'");
                        AreaTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                        if (AreaTable != null && AreaTable.Rows.Count > 0)
                        {
                            if (AreaTable.Select(Area.AreaParentNId + "=" + areaParentNId).Length > 0)
                            {
                                RetVal = true;
                            }
                        }
                    }
                    else
                    {
                        RetVal = true;
                    }
                }
                else
                {
                    // if new nid is different 
                    if (NIdInDB == 0)
                    {
                        RetVal = false;
                    }
                    else if (NIdInDB != areaNId)
                    {
                        RetVal = true;
                    }
                    else if (NIdInDB <= 0 || NIdInDB == areaNId)
                    {
                        SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.Name, "'" + areaName + "'");

                        AreaTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                        if (AreaTable != null && AreaTable.Rows.Count > 0)
                        {
                            foreach (DataRow Row in AreaTable.Select(Area.AreaParentNId + "=" + areaParentNId))
                            {
                                if (Convert.ToInt32(Row[Area.AreaNId]) != areaNId)
                                {
                                    RetVal = true;
                                    break;
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        ///  Check record into collection and database if not exist then create area . 
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo </param>
        /// <returns>return parent nid</returns>
        public int CheckNCreateArea(AreaInfo areaInfo)
        {
            int RetVal = 0;

            try
            {
                //Step 1: If area id and parent area id not exists then create it.
                this.CreateAreaIDNParentID(areaInfo);

                //Step 2: check existence of area (areaID)
                RetVal = this.CheckAreaExists(areaInfo.ID);

                //Step 3: if doesnt exists, then create it
                if (RetVal <= 0)
                {
                    //Create into database
                    RetVal = this.CreateArea(areaInfo);
                }

                //Step 4:Add into area collection..
                this.AddAreaIntoCollection(areaInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Import Area map,layer,etc from given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all availble maps</param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        public void ImportAreaMaps(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {
            this.ImportAreaMaps(selectedNIDs, selectionCount, sourceDBConnection, sourceDBQueries, -1);
        }




        /// <summary>
        /// Import Area map,layer,etc from given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all availble maps</param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="requiredAreaLevel">Send -1 to import all  levels area maps</param>
        public void ImportAreaMaps(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries, int requiredAreaLevel)
        {
            int LayerNId = -1;
            int TrgtLayerNId = -1;
            int AreaNId;
            int MetaDataNId;
            int FeatureNId = -1;
            int CurrentRecordIndex = 0;

            bool IsSameLayer = true;
            bool IsItFeatureLayer = false;


            string LayerName = string.Empty;
            string Metadata = string.Empty;
            string FeatureType = string.Empty;
            string SqlString = string.Empty;

            //DataTable AreaTable;
            DataTable LayerTable = new DataTable();
            DataTable TempTable;
            //DataTable TempFeatureTable;
            //DataTable TempXsltTable;
            //DbCommand Command;
            //DbDataAdapter adapter;
            //DbCommandBuilder CommandBuilder;
            //DataSet TempDataset;
            DataRow NewLayerRow;
            DataRow layerRow;
            DataRow TempFeatureTableRow;
            MetaDataBuilder MetaDataBuilderObj;

            List<int> NewAddedLayerNidList = null;
            string LangCode = string.Empty;

            try
            {
                // --------- MAPS --------- 
                // -- Multiple Areas can be associated with the same Layer. So get the Unique list of AreaGIDs and LayerNIds against the 
                // -- selected AreaNIds from the source Database. Loop through them and insert these Maps into Target Database 

                try
                {
                    this.RaiseStartProcessEvent();

                    if (selectionCount <= 0)
                    {
                        // -- GET ALL 
                        if (requiredAreaLevel <= 0)
                        {
                            // get all levels area
                            TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapLayerInfo(FilterFieldType.None, string.Empty, FieldSelection.Heavy));
                        }
                        else
                        {
                            // get required level areas
                            TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapLayerInfo(FilterFieldType.Search, " A." + Area.AreaLevel + " <= " + requiredAreaLevel.ToString() + " ", FieldSelection.Heavy));
                        }

                        LayerTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.None, string.Empty, FieldSelection.Heavy));
                    }


                    else
                    {
                        // -- GET SELECTED 
                        TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapLayerInfo(FilterFieldType.NId, selectedNIDs, FieldSelection.Heavy));

                        LayerTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.AreaNId, selectedNIDs, FieldSelection.Heavy));
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }


                //-- read records and insert it into oTempDT 
                {
                    this.RaiseBeforeProcessEvent(TempTable.Rows.Count);

                    NewAddedLayerNidList = new List<int>();

                    //for (Index = 0; Index <= TempTable.Rows.Count - 1; Index++)
                    foreach (DataRow TempTableRow in TempTable.Rows)
                    {
                        CurrentRecordIndex++;

                        if (LayerNId != Convert.ToInt32(TempTableRow[Area_Map_Layer.LayerNId]))
                        {
                            LayerNId = Convert.ToInt32(TempTableRow[Area_Map_Layer.LayerNId]);
                            IsSameLayer = false;
                        }
                        else
                        {
                            IsSameLayer = true;
                        }

                        // -- STEP 1: Check target AREA ID has any layer associated 
                        // -- Assumption: Source AreaId will be there in the Taget Database. 

                        //import maps by mapping AREA_ID 
                        using (DataTable AreaTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetArea(FilterFieldType.ID, "'" + TempTableRow[Area.AreaID].ToString() + "'")))
                        {
                            if (AreaTable.Rows.Count == 1)
                            {
                                AreaNId = Convert.ToInt32(AreaTable.Rows[0][Area.AreaNId]);
                            }
                            else
                            {
                                // -- This case will happen when 2 Areas with the same name are lying under the same Parent at the same level. 
                                // -- Because of that, only one Area would be created in the Target database 
                                AreaNId = -1;
                            }
                        }


                        if (!(AreaNId == -1))
                        {

                            // -- STEP 2: Remove the Layer Association and Add the Source Layer 
                            this.RemoveAreaMapAssociations(AreaNId,NewAddedLayerNidList, true);

                            // -- STEP 3: Add Layer, Feature Layer and MetaData 
                            if (!IsSameLayer)
                            {
                                #region -- When layer is not same --

                                //-- Get the Map Layer Information into the Target Database 

                                if (LayerTable.Select(Area_Map_Layer.LayerNId + "= " + TempTableRow[Area_Map_Layer.LayerNId]).Length > 0)
                                {
                                    layerRow = LayerTable.Select(Area_Map_Layer.LayerNId + "= " + TempTableRow[Area_Map_Layer.LayerNId])[0];
                                    LayerName = Convert.ToString(TempTableRow[Area_Map_Metadata.LayerName]);

                                    TrgtLayerNId = this.UpdateAreamap(this.DBQueries.TablesName.AreaMapLayer, Convert.ToString(layerRow[Area_Map_Layer.LayerSize]), layerRow[Area_Map_Layer.LayerShp], layerRow[Area_Map_Layer.LayerShx], layerRow[Area_Map_Layer.Layerdbf], Convert.ToString(layerRow[Area_Map_Layer.LayerType]), Convert.ToString(layerRow[Area_Map_Layer.MinX]), Convert.ToString(layerRow[Area_Map_Layer.MinY]), Convert.ToString(layerRow[Area_Map_Layer.MaxX]), Convert.ToString(layerRow[Area_Map_Layer.MaxY]), Convert.ToString(layerRow[Area_Map_Layer.StartDate]), Convert.ToString(layerRow[Area_Map_Layer.EndDate]), Convert.ToString(layerRow[Area_Map_Layer.UpdateTimestamp]));

                                    NewAddedLayerNidList.Add(TrgtLayerNId);

                                    ////if (TrgtLayerNId == -111)
                                    ////{
                                    ////    // when the size of temp database/template file exceeds 1.90GB then compact database and reimport layer
                                    ////    this.CompactDataBase(ref this.DBConnection);
                                    ////    TrgtLayerNId = this.UpdateAreamap(this.DBQueries.TablesName.AreaMapLayer, Convert.ToString(layerRow[Area_Map_Layer.LayerSize]), layerRow[Area_Map_Layer.LayerShp], layerRow[Area_Map_Layer.LayerShx], layerRow[Area_Map_Layer.Layerdbf], Convert.ToString(layerRow[Area_Map_Layer.LayerType]), Convert.ToString(layerRow[Area_Map_Layer.MinX]), Convert.ToString(layerRow[Area_Map_Layer.MinY]), Convert.ToString(layerRow[Area_Map_Layer.MaxX]), Convert.ToString(layerRow[Area_Map_Layer.MaxY]), Convert.ToString(layerRow[Area_Map_Layer.StartDate]), Convert.ToString(layerRow[Area_Map_Layer.EndDate]), Convert.ToString(layerRow[Area_Map_Layer.UpdateTimestamp]));

                                    ////}
                                }
                                else
                                {
                                    TrgtLayerNId = -1;
                                }


                                // -- Feature Layer 
                                IsItFeatureLayer = Convert.ToBoolean(TempTableRow[Area_Map.FeatureLayer]);
                                if (IsItFeatureLayer)
                                {
                                    // -- Add the Feature Layer Reference 

                                    // -- Get Feature Name from the Source Database 
                                    using (DataTable TempFeatureTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaFeatureByNid(TempTableRow[Area_Feature_Type.FeatureTypeNId].ToString())))
                                    {
                                        if (TempFeatureTable.Rows.Count > 0)
                                        {
                                            TempFeatureTableRow = TempFeatureTable.Rows[0];

                                            FeatureType = TempFeatureTableRow[Area_Feature_Type.FeatureType].ToString();
                                            TempFeatureTable.Dispose();

                                            // -- Check if this feature exists in the Target Database 
                                            using (DataTable TempFeatureTable1 = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaFeatureByFeatureType("'" + FeatureType + "'")))
                                            {

                                                if (TempFeatureTable1.Rows.Count > 0)
                                                {
                                                    TempFeatureTableRow = TempFeatureTable1.Rows[0];

                                                    // -- If it exists then pick its reference 
                                                    FeatureNId = Convert.ToInt32(TempFeatureTableRow[Area_Feature_Type.FeatureTypeNId]);

                                                    //-- Check If the Area already has the relation with this Feature 
                                                    using (DataTable TempFeatureTable2 = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetDuplicateAreaFeature(AreaNId, FeatureNId)))
                                                    {
                                                        if (TempFeatureTable2.Rows.Count > 0)
                                                        {
                                                            // -- Same relation already exists 
                                                            // -- remove this relation 
                                                            this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Area.DeleteAreaMap(AreaNId.ToString(), FeatureNId.ToString()));
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // -- if it does not then create a new feature 
                                                    FeatureType = DICommon.RemoveQuotes(FeatureType);
                                                    this.DBConnection.ExecuteScalarSqlQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMapFeature(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, FeatureType));
                                                    FeatureNId = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                                }
                                            }
                                            //    TempFeatureTable.Dispose();
                                            //    TempFeatureTable = null;
                                        }
                                    }
                                }

                                // -- Add MetaData Text 
                                MetaDataNId = -1;
                                Metadata = "";
                                if (!Information.IsDBNull(TempTableRow[Area_Map_Metadata.MetadataText]))
                                {
                                    if (!(TempTableRow[Area_Map_Metadata.MetadataText].ToString().Length == 0))
                                    {
                                        Metadata = TempTableRow[Area_Map_Metadata.MetadataText].ToString();
                                    }
                                }

                                if (TrgtLayerNId > 0)
                                {
                                    //add language based maps
                                    foreach (DataRow LangRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                                    {
                                        LangCode = "_" + (LangRow[Language.LanguageCode].ToString().Trim("-".ToCharArray()));

                                        // use removeQuotes method for sLayerName 
                                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMapMetadata(this.DBQueries.DataPrefix, LangCode, DICommon.RemoveQuotes(Metadata), TrgtLayerNId, DICommon.RemoveQuotes(LayerName)));
                                    }

                                    // -- STEP 4: Create the Area Layer Association 
                                    // -- Create a New Relation of the Area_Map with the LayerNId selected 
                                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMap(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, AreaNId, IsItFeatureLayer, FeatureNId, TrgtLayerNId));


                                    #region -- update area xslt info --

                                    //-- get info from source database 
                                    SqlString = sourceDBQueries.Xslt.GetXSLT(LayerNId.ToString(), MetadataElementType.Area);

                                    using (DataTable TempXsltTable = sourceDBConnection.ExecuteDataTable(SqlString))
                                    {
                                        // -- update xslt tables in the target database 
                                        if (TempXsltTable.Rows.Count > 0)
                                        {

                                            MetaDataBuilderObj = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                                            MetaDataBuilderObj.ImportTransformInfo(TempXsltTable.Rows[0][XSLT.XSLTText].ToString(), TempXsltTable.Rows[0][XSLT.XSLTFile].ToString(), TrgtLayerNId.ToString(), MetadataElementType.Area);
                                        }
                                    }

                                    #endregion
                                }

                                #endregion
                            }

                            else
                            {
                                #region -- if bSameLayer is false --

                                //"If more than 1 area is associated to the same map, then the map association happens only with one of the areas" 

                                // -- Feature Layer 
                                IsItFeatureLayer = Convert.ToBoolean(TempTableRow[Area_Map.FeatureLayer]);

                                // -- Get Feature Name from the Source Database 
                                using (DataTable TempFeatureTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaFeatureByNid(TempTableRow[Area_Feature_Type.FeatureTypeNId].ToString())))
                                {

                                    if (TempFeatureTable.Rows.Count > 0)
                                    {
                                        TempFeatureTableRow = TempFeatureTable.Rows[0];
                                        FeatureType = TempFeatureTableRow[Area_Feature_Type.FeatureType].ToString();
                                        TempFeatureTable.Dispose();

                                        // -- Check if this feature exists in the Target Database 
                                        using (DataTable TempFeatureTable1 = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaFeatureByFeatureType("'" + FeatureType + "'")))
                                        {
                                            if (TempFeatureTable1.Rows.Count > 0)
                                            {
                                                TempFeatureTableRow = TempFeatureTable1.Rows[0];

                                                // -- If it exists then pick its reference 
                                                FeatureNId = Convert.ToInt32(TempFeatureTableRow[Area_Feature_Type.FeatureTypeNId]);

                                                //-- Check If the Area already has the relation with this Feature 
                                                using (DataTable TempFeatureTable2 = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetDuplicateAreaFeature(AreaNId, FeatureNId)))
                                                {
                                                    if (TempFeatureTable2.Rows.Count > 0)
                                                    {
                                                        // -- Same relation already exists 
                                                        // -- remove this relation 
                                                        this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Area.DeleteAreaMap(AreaNId.ToString(), FeatureNId.ToString()));

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // -- if it does not then create a new feature 
                                                FeatureType = DICommon.RemoveQuotes(FeatureType);
                                                this.DBConnection.ExecuteScalarSqlQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMapFeature(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, FeatureType));
                                                FeatureNId = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                            }
                                        }
                                        //TempFeatureTable.Dispose();
                                        //TempFeatureTable = null;
                                    }



                                }

                                // -- STEP 4: Create the Area Layer Association 
                                // -- Create a New Relation of the Area_Map with the LayerNId selected 
                                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMap(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, AreaNId, IsItFeatureLayer, FeatureNId, TrgtLayerNId));
                                #endregion
                            }
                        }

                        this.RaiseProcessInfoEvent(CurrentRecordIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void UpdateAreaMapLayer(ref int TrgtLayerNId, ref string LayerName, DataTable LayerTable, DataRow TempTableRow, out DataRow NewLayerRow, out DataRow layerRow)
        {
            using (DbCommand Command = this.DBConnection.GetCurrentDBProvider().CreateCommand())
            {
                using (DbDataAdapter adapter = this.DBConnection.GetCurrentDBProvider().CreateDataAdapter())
                {
                    Command.Connection = this.DBConnection.GetConnection();
                    Command.CommandText = this.DBQueries.Area.GetAreaMapLayer(string.Empty, FieldSelection.Heavy);
                    Command.CommandType = CommandType.Text;
                    adapter.SelectCommand = Command;

                    using (DbCommandBuilder CommandBuilder = this.DBConnection.GetCurrentDBProvider().CreateCommandBuilder())
                    {
                        CommandBuilder.DataAdapter = adapter;

                        using (DataSet TempDataset = new DataSet(this.DBQueries.TablesName.AreaMapLayer))
                        {
                            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            adapter.Fill(TempDataset, this.DBQueries.TablesName.AreaMapLayer);

                            //Fill data adapter 
                            NewLayerRow = TempDataset.Tables[0].NewRow();
                            try
                            {

                                layerRow = LayerTable.Select(Area_Map_Layer.LayerNId + "= " + TempTableRow[Area_Map_Layer.LayerNId])[0];

                                LayerName = TempTableRow[Area_Map_Metadata.LayerName].ToString();
                                NewLayerRow[Area_Map_Layer.LayerSize] = layerRow[Area_Map_Layer.LayerSize];
                                NewLayerRow[Area_Map_Layer.LayerShp] = layerRow[Area_Map_Layer.LayerShp];

                                NewLayerRow[Area_Map_Layer.LayerShx] = layerRow[Area_Map_Layer.LayerShx];

                                NewLayerRow[Area_Map_Layer.Layerdbf] = layerRow[Area_Map_Layer.Layerdbf];

                                NewLayerRow[Area_Map_Layer.LayerType] = layerRow[Area_Map_Layer.LayerType];
                                NewLayerRow[Area_Map_Layer.MinX] = layerRow[Area_Map_Layer.MinX];
                                NewLayerRow[Area_Map_Layer.MinY] = layerRow[Area_Map_Layer.MinY];
                                NewLayerRow[Area_Map_Layer.MaxX] = layerRow[Area_Map_Layer.MaxX];
                                NewLayerRow[Area_Map_Layer.MaxY] = layerRow[Area_Map_Layer.MaxY];
                                NewLayerRow[Area_Map_Layer.StartDate] = layerRow[Area_Map_Layer.StartDate];
                                NewLayerRow[Area_Map_Layer.EndDate] = layerRow[Area_Map_Layer.EndDate];
                                NewLayerRow[Area_Map_Layer.UpdateTimestamp] = layerRow[Area_Map_Layer.UpdateTimestamp];

                                //byte[] buffer = NewLayerRow[Area_Map_Layer.LayerShp];

                                System.IO.MemoryStream str = new System.IO.MemoryStream();

                                byte[] buffer = new byte[System.Convert.ToInt32(str.Length)];
                                str.Position = 0;
                                str.Read(buffer, 0, System.Convert.ToInt32(str.Length));


                                //-- bug fixed on 5,May,2006: end                                                          NewLayerRow[Area_Map_Layer.Layerdbf]
                                TempDataset.Tables[0].Rows.Add(NewLayerRow);
                                adapter.Update(TempDataset, this.DBQueries.TablesName.AreaMapLayer);

                                //Save changes to the database 
                                //-- Return Layer NId 
                                TrgtLayerNId = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));


                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(ex.ToString());
                                TrgtLayerNId = -1;
                            }
                            finally
                            {
                                //if (adapter != null)
                                //{
                                //    adapter.Dispose();
                                //}
                                //if (CommandBuilder != null)
                                //{
                                //    CommandBuilder.Dispose();
                                //}
                                //if (Command != null)
                                //{
                                //    Command.Dispose();
                                //}

                            }
                        }
                    }
                }
            }
        }

        private int UpdateAreamap(string tableName, string LayerSize, object LayerShpbuffer, object LayerShxbuffer, object Layerdbfbuffer, string LayerType, string MinX, string MinY, string MaxX, string MaxY, string StartDate, string EndDate, string UpdateTimestamp)
        {
            int RetVal = 0;

            string SqlQuery = string.Empty;
            DbCommand Command = this.DBConnection.GetCurrentDBProvider().CreateCommand();
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
                SqlQuery = "INSERT INTO " + tableName + "(" + Area_Map_Layer.LayerSize + "," + Area_Map_Layer.LayerShp + "," + Area_Map_Layer.LayerShx + "," + Area_Map_Layer.Layerdbf + "," + Area_Map_Layer.LayerType + "," + Area_Map_Layer.MinX + "," + Area_Map_Layer.MinY + "," + Area_Map_Layer.MaxX
                    + "," + Area_Map_Layer.MaxY + "," + Area_Map_Layer.StartDate + "," + Area_Map_Layer.EndDate + "," + Area_Map_Layer.UpdateTimestamp + ")"
                    + " VALUES(@LayerSize,@LayerShp,@LayerShx,@Layerdbf,@LayerType,@MinX,@MinY,@MaxX,@MaxY,@StartDate,@EndDate,@UpdateTimestamp)";



                //-- Change for Online Database
                //  SqlQuery = SqlQuery.Replace("?,?,?", "@LayerShp,@LayerShx,@Layerdbf");

                Command.Connection = DBConnection.GetConnection();
                Command.CommandText = SqlQuery;
                Command.CommandType = CommandType.Text;

                ParameterLayerShp = DBConnection.GetCurrentDBProvider().CreateParameter();

                LayerSizeParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                LayerTypeParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                MinXParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                MinYParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                MaxXParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                MaxYParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                StartDateParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                EndDateParameter = DBConnection.GetCurrentDBProvider().CreateParameter();
                UpdateTimestampParameter = DBConnection.GetCurrentDBProvider().CreateParameter();

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

                ParameterLayerShx = DBConnection.GetCurrentDBProvider().CreateParameter();
                ParameterLayerShx.ParameterName = "@LayerShx";
                ParameterLayerShx.DbType = DbType.Binary;
                ParameterLayerShx.Value = LayerShxbuffer;
                ParameterLayerShx.Size = ((byte[])LayerShxbuffer).Length;
                Command.Parameters.Add(ParameterLayerShx);

                ParameterLayerdbf = DBConnection.GetCurrentDBProvider().CreateParameter();
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
                Command.ExecuteNonQuery();

                //-- this saves the image to the database 
                RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

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

        /// <summary>
        /// Compact the database mainly Access database(.mdb) and save as specified destination file.
        /// </summary>
        public bool CompactDataBase(ref DIConnection sourceDBConnection)
        {
            bool RetVal = false;
            string SourceFilePath = string.Empty;
            DIConnectionDetails SourceDBConnectionDetails;
            JRO.JetEngine je;
            string DataPrefix = string.Empty;


            try
            {
                if (sourceDBConnection != null)
                {
                    // dispose source database connection
                    SourceDBConnectionDetails = sourceDBConnection.ConnectionStringParameters;
                    SourceFilePath = SourceDBConnectionDetails.DbName;
                    sourceDBConnection.Dispose();

                    System.Threading.Thread.Sleep(10);
                    //try
                    //{
                    //    if (File.Exists(destFilePath))
                    //    {
                    //        File.SetAttributes(destFilePath, FileAttributes.Normal);
                    //        File.Delete(destFilePath);
                    //    }
                    //}
                    //catch { }

                    // Copy SourceFile to temp file 
                    string TempFile = DICommon.GetValidFileName(DateTime.Now.ToString()) + Path.GetExtension(SourceFilePath);
                    //   File.Copy(SourceDBNameWPath, TempFile, true);

                    // compacting the database
                    je = new JRO.JetEngine();
                    je.CompactDatabase("Data Source=\"" + SourceFilePath + "\";Jet OLEDB:Database Password=" + SourceDBConnectionDetails.Password, "Data Source=\"" + TempFile + "\";Jet OLEDB:Database Password=" + SourceDBConnectionDetails.Password);
                    je = null;

                    // copy temp file  to source file
                    System.IO.File.Copy(TempFile, SourceFilePath, true);

                    // reconnect to source database
                    sourceDBConnection = new DIConnection(SourceDBConnectionDetails);


                    RetVal = true;
                }

            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        private void ImportAreaMetadataCategories(DIConnection srcDBConn, DIQueries srcDBQueries)
        {
            MetadataCategoryInfo MDCatInfo = null;
            MetadataCategoryBuilder MDCatBuilder = new MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
            MetadataCategoryBuilder SrcMDCatBuilder = new MetadataCategoryBuilder(srcDBConn, srcDBQueries);
            //-- Get Categories from Source Table
            DataTable Table = SrcMDCatBuilder.GetAllRecordsFromMetadataCategory();

            DataRow[] Rows = Table.Select(Metadata_Category.CategoryType + "=" + "'A'");

            foreach (DataRow Row in Rows)
            {
                MDCatInfo = new MetadataCategoryInfo();
                MDCatInfo.CategoryName = Convert.ToString(Row[Metadata_Category.CategoryName]);
                MDCatInfo.CategoryType = Convert.ToString(Row[Metadata_Category.CategoryType]);
                // Add MetadataCategory Into all metdata category language tables
                MDCatBuilder.CheckNCreateMetadataCategory(MDCatInfo);
            }


        }

        /// <summary>
        /// Import Area from the given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all availble maps</param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        public void ImportArea(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {

            this.ImportArea(selectedNIDs, selectionCount, sourceDBConnection, sourceDBQueries, -1);
        }

        /// <summary>
        /// Import Area from the given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all availble maps</param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="importMapAlso">send true to import map also</param>
        public void ImportArea(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries, bool importMapAlso)
        {

            this.ImportArea(selectedNIDs, selectionCount, sourceDBConnection, sourceDBQueries, -1, importMapAlso);
        }


        /// <summary>
        /// Import Area from the given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all area </param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="requiredAreaLevel">Send -1 to import all levels </param>
        public void ImportArea(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries, int requiredAreaLevel)
        {
            this.ImportArea(selectedNIDs, selectionCount, sourceDBConnection, sourceDBQueries, requiredAreaLevel, false);
        }

        /// <summary>
        /// Import Area from the given source database or template into current databse/template
        /// </summary>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount">Send -1 to import all area </param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="requiredAreaLevel">Send -1 to import all levels </param>
        /// <param name="importMapAlso">Send true to import map also </param>
        public void ImportArea(string selectedNIDs, int selectionCount, DIConnection sourceDBConnection, DIQueries sourceDBQueries, int requiredAreaLevel, bool importMapAlso)
        {

            int CurrentRecordIndex = 0;
            string AreaId = string.Empty;
            string AreaName = string.Empty;
            string Map = string.Empty;
            string Block = string.Empty;
            int NewAreaNId = -1;

            // Key is area id and value is area_nid (target database)
            Dictionary<string, int> AlreadyImportedAreas = new Dictionary<string, int>();

            DataTable TempTable;
            try
            {
                this.RaiseStartProcessEvent();

                if (selectionCount <= 0)
                {
                    // -- GET ALL 
                    if (requiredAreaLevel <= 0)
                    {
                        // get all levels area
                        TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetArea(FilterFieldType.None, string.Empty));
                    }
                    else
                    {
                        // get requried level area
                        TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetArea(FilterFieldType.Search, " " + Area.AreaLevel + " <= " + requiredAreaLevel.ToString() + " "));
                    }
                }
                else
                {
                    // -- GET SELECTED 
                    TempTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetArea(FilterFieldType.NId, selectedNIDs));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            try
            {
                //import area 
                this.RaiseBeforeProcessEvent(TempTable.Rows.Count);
                //-- Import MetadataCategory
                this.ImportAreaMetadataCategories(sourceDBConnection, sourceDBQueries);

                foreach (DataRow Row in TempTable.Rows)
                {
                    CurrentRecordIndex++;

                    AreaName = Row[Area.AreaName].ToString();
                    AreaId = Row[Area.AreaID].ToString();

                    // import only if not imported
                    if (AlreadyImportedAreas.ContainsKey(AreaId) == false)
                    {
                        Block = string.Empty;
                        Map = string.Empty;

                        if (!Information.IsDBNull(Row[Area.AreaMap]))
                        {
                            Map = Convert.ToString(Row[Area.AreaMap]);
                        }
                        if (!Information.IsDBNull(Row[Area.AreaBlock]))
                        {
                            Block = Convert.ToString(Row[Area.AreaBlock]);
                        }

                        if (AreaName.Length > 58)
                            AreaName = AreaName.Substring(0, 58);

                        if (AreaId.Length > 253)
                            AreaId = AreaId.Substring(0, 253);


                        //Create Areas
                        NewAreaNId = this.CreateAreaChainFromExtDB(
                                Convert.ToInt32(Row[Area.AreaNId]),
                                Convert.ToInt32(Row[Area.AreaParentNId]), Row[Area.AreaID].ToString(),
                                Row[Area.AreaName].ToString(), Row[Area.AreaGId].ToString(),
                                Convert.ToInt32(Row[Area.AreaLevel]), Map, Block,
                                Convert.ToBoolean(Row[Area.AreaGlobal]), sourceDBQueries,
                                sourceDBConnection, AlreadyImportedAreas, importMapAlso);


                    }
                    this.RaiseProcessInfoEvent(CurrentRecordIndex);
                }

                //import area blocks
                this.RaiseBeforeProcessEvent(TempTable.Rows.Count);
                CurrentRecordIndex = 0;
                foreach (DataRow TempDataRow in TempTable.Rows)
                {
                    CurrentRecordIndex++;

                    this.UpdateBlockAreas(TempDataRow[Area.AreaID].ToString(), TempDataRow[Area.AreaBlock].ToString(), sourceDBQueries, sourceDBConnection);

                    this.RaiseProcessInfoEvent(CurrentRecordIndex);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// Insert Area record into database
        /// </summary>
        /// <param name="areaName">Area Name</param>
        /// <param name="areaID">Area ID</param>
        /// <param name="areaGID">Area GId</param>
        /// <param name="areaLevel">Area Level</param>
        /// <param name="parentNId">Area Parent Nid</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        public bool InsertIntoDatabase(string areaName, string areaID, string areaGID, int areaLevel, int parentNId)
        {
            return this.InsertIntoDatabase(areaName, areaID, areaGID, areaLevel, parentNId, string.Empty, string.Empty, false);
        }

        /// <summary>
        /// Get area Nid by AreaID
        /// </summary>
        /// <param name="areaID">Area ID </param>
        /// <returns>Area Nid</returns>
        public int GetAreaNidByAreaID(string areaID)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable AreaTable = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(areaID))
                {
                    SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.ID, "'" + areaID + "'");
                    AreaTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                    if (AreaTable.Rows.Count > 0)
                    {
                        RetVal = Convert.ToInt32(AreaTable.Rows[0][Area.AreaNId]);
                    }
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get area level  by AreaID 
        /// </summary>
        /// <param name="areaID">Area ID  </param>
        /// <returns>Area Level</returns>
        public int GetAreaLevelByAreaID(string areaID)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable AreaTable = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(areaID))
                {
                    SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.ID, "'" + areaID + "'");
                    AreaTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                    //check if Row Count Is greater Than Zero
                    if (AreaTable.Rows.Count > 0)
                    {
                        RetVal = Convert.ToInt32(AreaTable.Rows[0][Area.AreaLevel]);
                    }
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }


        /// <summary>
        /// Get AreaInfo based on filter criteria and filter string 
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, ParentNId, ID, GId, Search, NIdNotIn, NameNotIn, Level</param>
        /// <param name="filterText"></param>
        /// <returns>Returns null incase no record is found. If multiple records are available areaInfo of first record will be returned</returns>
        public AreaInfo GetAreaInfo(FilterFieldType filterFieldType, string filterText)
        {
            AreaInfo RetVal = null;

            string SqlQuery = string.Empty;
            DataTable AreaTable = new DataTable();
            try
            {
                if (!string.IsNullOrEmpty(filterText))
                {
                    SqlQuery = this.DBQueries.Area.GetArea(filterFieldType, filterText);
                    AreaTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                    //check if Row Count Is greater Than Zero
                    if (AreaTable.Rows.Count > 0)
                    {
                        RetVal = new AreaInfo();
                        RetVal.Nid = Convert.ToInt32(AreaTable.Rows[0][Area.AreaNId]);
                        RetVal.ParentNid = Convert.ToInt32(AreaTable.Rows[0][Area.AreaParentNId]);
                        RetVal.ID = Convert.ToString(AreaTable.Rows[0][Area.AreaID]);
                        RetVal.Name = Convert.ToString(AreaTable.Rows[0][Area.AreaName]);
                        RetVal.GID = Convert.ToString(AreaTable.Rows[0][Area.AreaGId]);
                        RetVal.Level = Convert.ToInt32(AreaTable.Rows[0][Area.AreaLevel]);
                        RetVal.AreaMap = Convert.ToString(AreaTable.Rows[0][Area.AreaMap]);
                        RetVal.AreaBlock = Convert.ToString(AreaTable.Rows[0][Area.AreaBlock]);
                        RetVal.IsGlobal = Convert.ToBoolean(AreaTable.Rows[0][Area.AreaGlobal]);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// Deletes areas from area and other associtated tables
        /// </summary>
        /// <param name="areaNIds"></param>
        public void DeleteAreas(string areaNIds)
        {
            DataTable TempDT = null;
            DataTable AreaTable = null;
            string LayerNids = string.Empty;
            string TableName = string.Empty;
            MetaDataBuilder MetadataBuilderObject;
            MapBuilder MapBuilderObj = new MapBuilder(this.DBConnection, this.DBQueries);

            try
            {

                LayerNids = DIConnection.GetDelimitedValuesFromDataTable(this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapByAreaNIds(areaNIds, true)), Area_Map.LayerNId);

                // Step 1: Delete records from area table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TableName = this.DBQueries.DataPrefix + AreaTableName + "_" + Row[Language.LanguageCode].ToString();

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Delete.DeleteArea(TableName, areaNIds));
                }

                // Step2: delete metadata
                MetadataBuilderObject = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                MetadataBuilderObject.DeleteMetadata(areaNIds, MetadataElementType.Area);

                if (!string.IsNullOrEmpty(LayerNids))
                {
                    //-- Remove Associated Table Records
                    MapBuilderObj.DeleteMap(LayerNids);
                }

                // -- STEP 3: Delete All First Level Child of this Area
                TempDT = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetArea(FilterFieldType.ParentNId, areaNIds));
                foreach (DataRow row in TempDT.Rows)
                {
                    this.DeleteAreas(row[Area.AreaNId].ToString());
                }


                // --  step 4 delete from data table
                new DIDatabase(this.DBConnection, this.DBQueries).DeleteByAreaNIds(areaNIds);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }
        /// <summary>
        /// Update AreaInfo
        /// </summary>
        /// <param name="areaInfo"></param>
        public void UpdateArea(AreaInfo areaInfo)
        {
            string SqlQuery = string.Empty;
            // -- Update the area
            SqlQuery = this.DBQueries.Update.Area.UpdateAreaByAreaNId(areaInfo.ParentNid, areaInfo.ID, areaInfo.Name, areaInfo.GID, areaInfo.Level, areaInfo.AreaMap, areaInfo.AreaBlock, areaInfo.IsGlobal, areaInfo.Nid);

            this.DBConnection.ExecuteNonQuery(SqlQuery);
        }

        /// <summary>
        /// Inserts area into database
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="areaID"></param>
        /// <param name="areaGID"></param>
        /// <param name="areaLevel"></param>
        /// <param name="parentNId"></param>
        /// <param name="areaMap"></param>
        /// <param name="areaBlock"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public bool InsertIntoDatabase(string areaName, string areaID, string areaGID, int areaLevel, int parentNId, string areaMap, string areaBlock, bool isGlobal)
        {
            bool RetVal = false;
            string AreaGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string AreaForDatabase = string.Empty;
            string AreaLevelNameForDatabase = string.Empty;
            DITables TablesName = null;
            DataTable Table = null;
            int AreaLevelInDB = -1;
            bool IsAreaLevelInstertionReq = true;

            try
            {
                #region "-- add all available area_levels into "inserted area level" list --"
                if (this.InsertedAreaLevel.Count == 0)
                {
                    try
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaLevel(FilterFieldType.None, string.Empty));

                        foreach (DataRow Row in Table.Rows)
                        {
                            AreaLevelInDB = Convert.ToInt32(Row[Area_Level.AreaLevel]);
                            if (!this.InsertedAreaLevel.Contains(AreaLevelInDB))
                            {
                                this.InsertedAreaLevel.Add(AreaLevelInDB);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.ExceptionFacade.ThrowException(ex);
                    }
                }
                #endregion

                // check area level instertion is required in database/template
                if (this.InsertedAreaLevel.Contains(areaLevel))
                {
                    IsAreaLevelInstertionReq = false;
                }

                // insert area & area level into database
                DefaultLanguageCode = this.DBQueries.LanguageCode;
                if (!string.IsNullOrEmpty(areaGID))
                {
                    AreaGId = areaGID;
                }
                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        AreaForDatabase = areaName;
                        AreaLevelNameForDatabase = DILanguage.GetLanguageString("LEVEL");
                    }
                    else
                    {
                        AreaForDatabase = Constants.PrefixForNewValue + areaName;
                        AreaLevelNameForDatabase = Constants.PrefixForNewValue + DILanguage.GetLanguageString("LEVEL");
                    }

                    // insert area level into area_level table 
                    if (IsAreaLevelInstertionReq)
                    {
                        AreaLevelNameForDatabase += " " + areaLevel.ToString();

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaLevel(TablesName.AreaLevel, areaLevel.ToString(), AreaLevelNameForDatabase));
                    }

                    // insert area info into area table
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertArea(this.DBQueries.DataPrefix, "_" + LanguageCode, parentNId, areaID, AreaForDatabase, AreaGId, areaLevel, areaMap, areaBlock, isGlobal, DBConnection.ConnectionStringParameters.ServerType));

                    RetVal = true;
                }


                if (IsAreaLevelInstertionReq)
                {
                    // add area level into inserted list
                    this.InsertedAreaLevel.Add(areaLevel);
                }

            }
            catch (Exception)
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Updates data exists values
        /// </summary>
        public void UpdateDataExistValues()
        {
            DIServerType ServerType = this.DBConnection.ConnectionStringParameters.ServerType;
            DITables TablesName;
            string LanguageCode = string.Empty;

            try
            {
                // 1. set all areas' data_exist value to false in default language table
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateDataExistToFalse(ServerType));

                // 2. set data_exist to true but where data exists
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateDataExistValues(ServerType));

                // 3. update other language tables 
                foreach (DataRow LanguageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = Convert.ToString(LanguageRow[Language.LanguageCode]);

                    // update all Language tables except default langauge table
                    if (("_" + LanguageCode) != this.DBQueries.LanguageCode)
                    {
                        TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                        this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateDataExistValuesInOtherLangauge(TablesName));
                    }
                }
            }
            catch (Exception ex)
            {
                DevInfo.Lib.DI_LibBAL.ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Import Area by Name, ID
        /// </summary>
        /// <param name="requiredAreaLevel"></param>
        /// <param name="areaName"></param>
        /// <param name="areaID"></param>
        /// <param name="parentAreaID"></param>
        /// <returns></returns>
        public int ImportArea(string areaName, string areaID, string parentAreaID, bool isGlobal)
        {
            int RetVal = 0;
            string Map = string.Empty;
            string Block = string.Empty;
            AreaInfo AreaInfoObj = new AreaInfo();

            try
            {
                RetVal = this.GetAreaNidByAreaID(areaID);

                AreaInfoObj.Name = areaName;
                AreaInfoObj.ID = areaID;
                AreaInfoObj.Parent = new AreaInfo();
                if (parentAreaID == "-1")
                {
                    AreaInfoObj.Parent.Nid = -1;
                }
                else
                {
                    AreaInfoObj.Parent.ID = parentAreaID;
                    AreaInfoObj.Parent.Nid = this.GetAreaNidByAreaID(parentAreaID);
                }
                AreaInfoObj.IsGlobal = isGlobal;

                if (RetVal <= 0)
                {

                    RetVal = this.CheckNCreateArea(AreaInfoObj);
                }
                else
                {
                    this.UpdateArea(AreaInfoObj);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

    }

}
