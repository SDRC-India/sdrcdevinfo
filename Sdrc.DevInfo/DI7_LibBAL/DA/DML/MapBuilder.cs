using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Map;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// MapBuilder class manages all DML opertations related to Map layers.
    /// It contains methods for SELECT, INSERT and DELETE
    /// It affects table Area_Map, Area_Map_Layers, Area_Map_Metadata and Area_Feature_Type.
    /// It manages the database integrity while performing DML operations
    /// </summary>
    public class MapBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"
        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Constructor --"

        public MapBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }
        #endregion

        #region "-- Methods --"

        #region "-- SELECT --"

        public MapInfo GetMapInfo(string areaID)
        {
            return new MapInfo();
        }

        #endregion

        #region "-- INSERT --"

        /// <summary>
        /// Insert Map into AreaMapLayer table and MapFile name in AreaMapMetadata for all language tables
        /// </summary>
        /// <returns>Layer NId. If return value = -1 it implies that layer could not be inserted for some reasons</returns>
        public int InsertMap(MapInfo mapInfo)
        {
            int RetVal = -1;

            DbCommand Command = this.DBConnection.GetCurrentDBProvider().CreateCommand();
            DbParameter ParameterShp;
            DbParameter ParameterShx;
            DbParameter ParameterDbf;

            try
            {
                if (!string.IsNullOrEmpty(mapInfo.MapFilePath) && File.Exists(mapInfo.MapFilePath))
                {
                    string sFilePath = Path.GetDirectoryName(mapInfo.MapFilePath);
                    string sFileName = Path.GetFileNameWithoutExtension(mapInfo.MapFilePath);
                    ShapeInfo shapeInfo = ShapeFileReader.GetShapeInfo(sFilePath, sFileName);

                    //-- Validate that shape file is valid and devinfo compliant format
                    if (shapeInfo != null)
                    {
                        //-- Get shp file byte array
                        FileStream Shp = new FileStream(sFilePath + "\\" + sFileName + ".shp", FileMode.Open);
                        byte[] ShpBuffer = new byte[(int)Shp.Length - 1 + 1];
                        Shp.Read(ShpBuffer, 0, (int)Shp.Length);
                        Shp.Close();

                        //-- Get shx file byte array
                        FileStream Shx = new FileStream(sFilePath + "\\" + sFileName + ".shx", FileMode.Open);
                        byte[] ShxBuffer = new byte[(int)Shx.Length - 1 + 1];
                        Shx.Read(ShxBuffer, 0, (int)Shx.Length);
                        Shx.Close();

                        //-- Get dbf file byte array
                        FileStream Dbf = new FileStream(sFilePath + "\\" + sFileName + ".dbf", FileMode.Open);
                        byte[] DbfBuffer = new byte[(int)Dbf.Length - 1 + 1];
                        Dbf.Read(DbfBuffer, 0, (int)Dbf.Length);
                        Dbf.Close();

                        //-- Update mapInfo
                        mapInfo.LayerSize = ShpBuffer.Length + "," + ShxBuffer.Length + "," + DbfBuffer.Length;
                        if (mapInfo.IsFeatureLayer)
                        {
                            mapInfo.LayerType = (ShapeType)((int)shapeInfo.ShapeType - 1);
                        }
                        else
                        {
                            mapInfo.LayerType = (ShapeType)shapeInfo.ShapeType;
                        }
                        mapInfo.BoundingBox = shapeInfo.Extent;
                        mapInfo.UpdateTimestamp = DateTime.Now;


                        string sSql = DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertLayerWParamName(this.DBQueries.DataPrefix, mapInfo.LayerSize, (int)mapInfo.LayerType, mapInfo.BoundingBox.Left, mapInfo.BoundingBox.Top, mapInfo.BoundingBox.Right, mapInfo.BoundingBox.Bottom, mapInfo.StartDate, mapInfo.EndDate, mapInfo.UpdateTimestamp);
                        Command.Connection = this.DBConnection.GetConnection();
                        Command.CommandText = sSql;
                        Command.CommandType = CommandType.Text;
                        ParameterShp = this.DBConnection.GetCurrentDBProvider().CreateParameter();
                        ParameterShx = this.DBConnection.GetCurrentDBProvider().CreateParameter();
                        ParameterDbf = this.DBConnection.GetCurrentDBProvider().CreateParameter();

                        //-- Set the data type for parameter
                        ParameterShp.DbType = DbType.Binary;
                        ParameterShx.DbType = DbType.Binary;
                        ParameterDbf.DbType = DbType.Binary;

                        //-- Assign the contents of the buffer to the value of the parameter 
                        ParameterShp.ParameterName = "@" + Area_Map_Layer.LayerShp;
                        ParameterShp.Value = ShpBuffer;

                        ParameterShx.ParameterName = "@" + Area_Map_Layer.LayerShx;
                        ParameterShx.Value = ShxBuffer;

                        ParameterDbf.ParameterName = "@" + Area_Map_Layer.Layerdbf;
                        ParameterDbf.Value = DbfBuffer;
                        

                        //-- Add the parameter to the command 
                        Command.Parameters.Add(ParameterShp);
                        Command.Parameters.Add(ParameterShx);
                        Command.Parameters.Add(ParameterDbf);

                        //-- Insert the layer record
                        Command.ExecuteNonQuery();

                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

                        //-- Append record in Area_Map_Metadata table and set Layer_Name
                        //-- Insert record in Area_Map_Metadata table and set Layer_Name for all languages
                        string DefaultLanguageCode = this.DBQueries.LanguageCode;
                        string MapName =   mapInfo.MapName;
                        string LanguageCode = string.Empty;
                        foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                        {
                            LanguageCode = languageRow[Language.LanguageCode].ToString();
                            MapName = mapInfo.MapName;

                            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMapMetadata(this.DBQueries.DataPrefix, "_" + LanguageCode, string.Empty, RetVal, Utility.DICommon.RemoveQuotes(MapName)));
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.Print(ex.Message);
                //throw new ApplicationException(ex.Message);
            }
            finally
            {
                if ((Command != null))
                {
                    Command.Dispose();
                }
            }

            return RetVal;

        }

        /// <summary>
        /// Insert Area-Map relationship in AreaMap table 
        /// </summary>
        /// <param name="areaNId"></param>
        /// <param name="featureLayer"></param>
        /// <param name="featureTyepNId"></param>
        /// <param name="layerNId"></param>
        public void InsertAreaMap(int areaNId, bool featureLayer, int featureTyepNId, int layerNId)
        {
            string sSql = DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertAreaMap(this.DBQueries.DataPrefix, string.Empty, areaNId, featureLayer, featureTyepNId, layerNId);
            this.DBConnection.ExecuteNonQuery(sSql);
        }

        #endregion

        #region "-- DELETE --"

        /// <summary>
        /// Delete metatdata associated with a layer from all language tables
        /// </summary>
        /// <param name="layerNIds"></param>
        public void DeleteAreaMapMetaData(string layerNIds)
        {
            DITables TableNames;
            foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
            {
                TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Delete.DeleteAreaMapMetaData(TableNames.AreaMapMetadata, layerNIds.ToString()));
            }
        }

        /// <summary>
        /// Delete Map layer and all associated dependencies & relationships
        /// </summary>
        /// <param name="layerNId"></param>
        public void DeleteMap(string layerNIds)
        {
            DevInfo.Lib.DI_LibDAL.Queries.Area.Delete AreaDelete = new DevInfo.Lib.DI_LibDAL.Queries.Area.Delete(new DITables(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode));

            //-- Delete all associated metadata icons
            DIIcons.DeleteIcon(this.DBConnection, this.DBQueries.DataPrefix, layerNIds, IconElementType.MetadataArea);

            //-- Delete all associated record from Metdata table (languages based tables)
            this.DeleteAreaMapMetaData(layerNIds);

            //-- Delete all associated record from AreaMap table
            this.DBConnection.ExecuteNonQuery(AreaDelete.DeleteAreaMap(layerNIds.ToString()));

            // TODO Area feature type - Not to be implemented now as there is no interface to enter records into Area_Feature_Type table
            // When a record is deleted in AreaMap table, get Area_Map.Feature_Type_NId 
            // If Area_Map.Feature_Type_NId not associated to any other layer then delete record in Area_Feature_Type.Feature_Type_NId (language based table)

            //-- Delete layer record from AreaMapLayer master table
            this.DBConnection.ExecuteNonQuery(AreaDelete.DeleteAreaMapLayer(layerNIds.ToString()));

        }

        /// <summary>
        /// Delete map layers based on selected area nids
        /// </summary>
        /// <param name="areaNIds"></param>
        public void DeleteMapByAreaNIds(string areaNIds)
        {
            //-- Validate for valid argument value
            if (!string.IsNullOrEmpty(areaNIds))
            {
                DevInfo.Lib.DI_LibDAL.Queries.Area.Delete AreaDelete = new DevInfo.Lib.DI_LibDAL.Queries.Area.Delete(new DITables(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode));
                string[] ArrAreaNId = areaNIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string sSql = string.Empty;
                DataView dvAreaMapInfo;
                DataView dvAreaMapInfo2;
                int LayerNId = -1;

                //-- Iterate for each AreaNId
                for (int i = 0; i < ArrAreaNId.Length; i++)
                {
                    //-- Get all layer associated with AreaNId
                    sSql = this.DBQueries.Area.GetAreaMapByAreaNIds(ArrAreaNId[i], true);
                    dvAreaMapInfo = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                    //-- Iterate each layer associated with Area
                    foreach (DataRowView drv in dvAreaMapInfo)
                    {
                        LayerNId = (int)drv[Area_Map_Layer.LayerNId];
                        //-- Check whether this layer is associated with any other area

                        sSql = this.DBQueries.Area.GetAreaMapByLayerNIds(LayerNId.ToString(), true);
                        dvAreaMapInfo2 = this.DBConnection.ExecuteDataTable(sSql).DefaultView;
                        if (dvAreaMapInfo2.Count > 1)
                        {
                            //-- If this layer is associated with any other area then just drop the realtionship between Area and Layer
                            this.DBConnection.ExecuteNonQuery(AreaDelete.DeleteAreaMapByAreaNIds(ArrAreaNId[i]));
                        }
                        else
                        {
                            //-- If this layer is not associated with any other area then drop this layer along with other dependencies and relationships
                            this.DeleteMap(LayerNId.ToString());
                        }



                    }


                }




            }

        }

        /// <summary>
        /// Deletes area map associated for the given layerNid and areaNid
        /// </summary>
        /// <param name="layerNId"></param>
        /// <param name="areaNId"></param>
        public void DeleteMap(int layerNId,int areaNId)
        {
            string SqlQuery = string.Empty;
            DataView AreaMapDataView;
            DevInfo.Lib.DI_LibDAL.Queries.Area.Delete AreaDelete = new DevInfo.Lib.DI_LibDAL.Queries.Area.Delete(new DITables(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode));

            // step1: remove area and layer association
           SqlQuery= AreaDelete.DeleteAreaMap(layerNId, areaNId);
           this.DBConnection.ExecuteNonQuery(SqlQuery);

            // step2: delete map only if layer is not associated with other areas

           SqlQuery = this.DBQueries.Area.GetAreaMapByLayerNIds(layerNId.ToString(), true);
            AreaMapDataView= this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;

            if (AreaMapDataView.Count ==0)
            {                
                //-- If this layer is not associated with any other area then drop this layer along with other dependencies and relationships
                this.DeleteMap(layerNId.ToString());
            }

        }

        /// <summary>
        /// Delete all metatdata associated with all layer from all language tables
        /// </summary>
        public void ClearAreaMapMetadata()
        {
            DITables TableNames;
            foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
            {
                TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Delete.ClearAreaMapMetadata(TableNames.AreaMapMetadata));
            }

        }

        /// <summary>
        /// Clear all map and their dependencies & relationship from database
        /// </summary>
        public void ClearMap()
        {
            DevInfo.Lib.DI_LibDAL.Queries.Area.Delete AreaDelete = new DevInfo.Lib.DI_LibDAL.Queries.Area.Delete(new DITables(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode));

            //-- Clear all associated metadata icons
            DIIcons.ClearIcon(this.DBConnection, this.DBQueries.DataPrefix, IconElementType.MetadataArea);

            //-- Clear all record from Area_Map_Metadata table (languages based tables)
            this.ClearAreaMapMetadata();

            //-- Clear all associated record from Area_Map table
            this.DBConnection.ExecuteNonQuery(AreaDelete.ClearAreaMap());

            //-- Clear all records from Area_Feature_Type table 
            this.DBConnection.ExecuteNonQuery(AreaDelete.ClearAreaFeatureType());

            //-- Clear layer record from Area_Map_Layer master table
            this.DBConnection.ExecuteNonQuery(AreaDelete.ClearAreaMapLayer());
        }

        #endregion

        #endregion

        #endregion
    }
}
