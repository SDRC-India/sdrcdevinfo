using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Area
{

    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public class Delete
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string AreaTableName = "Area";
        private DITables TablesName;

        #endregion

        #endregion

        #region "-- public --"

        #region "-- New / Dispose --"

        public Delete(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        #region -- Area --

        /// <summary>
        /// Returns query to delete records from area table
        /// </summary>
        /// <param name="areaNids"></param>
        /// <returns></returns>
        public string DeleteArea(string areaNids)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM  " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaNId + " IN( " + areaNids + ") ";
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from area table for given Area Table
        /// </summary>
        /// <param name="tableName">Area Table Name with DataPrefix and LanguageCode</param>
        /// <param name="areaNids">AreaNids</param>
        /// <returns>string</returns>
        public static string DeleteArea(string tableName, string areaNids)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM  " + tableName + " WHERE " + DIColumns.Area.AreaNId + " IN( " + areaNids + ") ";
            return RetVal;
        }

        #endregion

        #region -- Area_Map --

        /// <summary>
        /// Delete record from Area_Map table based on  LayerNIds
        /// </summary>
        /// <param name="layerNIds">comma delimited LayerNIds</param>
        /// <returns></returns>
        public  string DeleteAreaMap(string layerNIds)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaMap + " WHERE " + DIColumns.Area_Map.LayerNId + " IN (" + layerNIds + ")";
            return RetVal;
        }

        /// <summary>
        /// Delete record from Area_Map table for the given areaNid and layer Nid
        /// </summary>
        /// <param name="layerNId"></param>
        /// <param name="areaNId"></param>
        /// <returns></returns>
        public string DeleteAreaMap(int layerNId,int areaNId)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE  FROM " + this.TablesName.AreaMap + " WHERE " + DIColumns.Area_Map.LayerNId + " =" + layerNId + " AND "+  DIColumns.Area_Map.AreaNId +"="+ areaNId ;
            return RetVal;
        }


        /// <summary>
        /// Delete record from Area_Map table based on  Area NIds
        /// </summary>
        /// <param name="areaNIds">comma delimited Area NIds</param>
        /// <returns></returns>
        public  string DeleteAreaMapByAreaNIds(string areaNIds)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaMap + " WHERE " + DIColumns.Area_Map.AreaNId + " IN (" + areaNIds + ")" ;
            return RetVal;
        }

        /// <summary>
        /// Delete all records from Area_Map table
        /// </summary>
        /// <returns></returns>
        public  string ClearAreaMap()
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaMap ;
            return RetVal;
        }

        /// <summary>
        /// Delete record from Area_Map table based on  Area NIds and featureNids??
        /// </summary>
        /// <param name="areaNIds">comma delimited Area NIds</param>
        /// <param name="featureNIds">comma delimited Feature NIds</param>
        /// <returns></returns>
        public  string DeleteAreaMap(string areaNIds, string featureNIds)
        {
            //TODO - Used in AreaBuilder. Check for its usage
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaMap + " WHERE " + DIColumns.Area_Map.AreaNId + " IN (" + areaNIds + ") AND " + DIColumns.Area_Map.FeatureTypeNId + " IN(" + featureNIds + ")";
            return RetVal;
        }

        #endregion

        #region -- Area_Feature_Type --

        /// <summary>
        /// Delete all records from Area_Feature_Type table
        /// </summary>
        /// <returns></returns>
        public string ClearAreaFeatureType()
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaFeatureType;
            return RetVal;
        }

        #endregion

        #region -- Area_Map_Layer --

        /// <summary>
        ///  Delete record from Area_Map_Layer table based on  LayerNIds
        /// </summary>
        /// <param name="layerNIds">comma delimited LayerNIds</param>
        /// <returns></returns>
        public string DeleteAreaMapLayer(string layerNIds)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.AreaMapLayer + " WHERE " + DIColumns.Area_Map_Layer.LayerNId + " IN (" + layerNIds + ")";
            return RetVal;
        }

        /// <summary>
        /// Delete all records from Area_Map_Layer table
        /// </summary>
        /// <returns></returns>
        public string ClearAreaMapLayer()
        {

            string RetVal = string.Empty;
            RetVal = "DELETE  FROM " + this.TablesName.AreaMapLayer ;
            return RetVal;
        }

        #endregion

        #region -- Area_Map_Metadata --

        /// <summary>
        /// Delete record from Area_Map_Metadata table based on  LayerNIds
        /// </summary>
        /// <param name="tableName">language based table name</param>
        /// <param name="layerNIds">comma delimited LayerNIds</param>
        /// <returns></returns>
        public static string DeleteAreaMapMetaData(string tableName, string layerNIds)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + tableName + " WHERE " + DIColumns.Area_Map_Metadata.LayerNId + " IN (" + layerNIds + ")";
            return RetVal;
        }

        /// <summary>
        /// Delete all records from Area_Map_Metadata table
        /// </summary>
        /// <param name="tableName">language based table name</param>
        /// <returns></returns>
        public static string ClearAreaMapMetadata(string tableName)
        {
            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + tableName;
            return RetVal;
        }
        #endregion

        #endregion

        #endregion
    }
}
