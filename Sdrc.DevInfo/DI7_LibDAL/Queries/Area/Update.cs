using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Area
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- New / Dispose --"

        public Update(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns sql query to update area
        /// </summary>
        /// <param name="areaParentNid"></param>
        /// <param name="areaID"></param>
        /// <param name="areaName"></param>
        /// <param name="areaGid"></param>
        /// <param name="areaLevel"></param>
        /// <param name="areaMap"></param>
        /// <param name="areaBlock"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public string UpdateArea(int areaParentNid, string areaID, string areaName, string areaGid, int areaLevel, string areaMap, string areaBlock, bool isGlobal)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.Area + " SET "
                + DIColumns.Area.AreaParentNId + "=" + areaParentNid + ","
                + DIColumns.Area.AreaID + "='" + DIQueries.RemoveQuotesForSqlQuery(areaID) + "',"
                + DIColumns.Area.AreaName + "='" + DIQueries.RemoveQuotesForSqlQuery(areaName) + "',"
                + DIColumns.Area.AreaGId + "='" + DIQueries.RemoveQuotesForSqlQuery(areaGid) + "',"
                + DIColumns.Area.AreaLevel + "=" + areaLevel + ","
                + DIColumns.Area.AreaMap + "='" + areaMap + "',"
                + DIColumns.Area.AreaBlock + "='" + areaBlock + "',"
                + DIColumns.Area.AreaGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + " where " + DIColumns.Area.AreaID + " = '" + areaID + "' ";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update area by Area_NId
        /// </summary>
        /// <param name="areaParentNid"></param>
        /// <param name="areaID"></param>
        /// <param name="areaName"></param>
        /// <param name="areaGid"></param>
        /// <param name="areaLevel"></param>
        /// <param name="areaMap"></param>
        /// <param name="areaBlock"></param>
        /// <param name="isGlobal"></param>
        /// <param name="areaNId"></param>
        /// <returns></returns>
        public string UpdateAreaByAreaNId(int areaParentNid, string areaID, string areaName, string areaGid, int areaLevel, string areaMap, string areaBlock, bool isGlobal, int areaNId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.Area + " SET "
                + DIColumns.Area.AreaParentNId + "=" + areaParentNid + ","
                + DIColumns.Area.AreaID + "='" + DIQueries.RemoveQuotesForSqlQuery(areaID) + "',"
                + DIColumns.Area.AreaName + "='" + DIQueries.RemoveQuotesForSqlQuery(areaName) + "',"
                + DIColumns.Area.AreaGId + "='" + DIQueries.RemoveQuotesForSqlQuery(areaGid) + "',"
                + DIColumns.Area.AreaLevel + "=" + areaLevel + ","
                + DIColumns.Area.AreaMap + "='" + areaMap + "',"
                + DIColumns.Area.AreaBlock + "='" + areaBlock + "',"
                + DIColumns.Area.AreaGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + " where " + DIColumns.Area.AreaNId + " =" + areaNId;

            return RetVal;
        }

        /// <summary>
        /// Updates area level name by level nid
        /// </summary>
        /// <param name="areaLevelNid"></param>
        /// <param name="levelName"></param>
        /// <returns></returns>
        public string UpdateAreaLevelNameByNId(int areaLevelNid, string levelName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.AreaLevel + " SET "
                + DIColumns.Area_Level.AreaLevelName + "='" + DIQueries.RemoveQuotesForSqlQuery(levelName) + "'  where " + DIColumns.Area_Level.LevelNId + " =" + areaLevelNid;

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update metadata info for the given layername
        /// </summary>
        /// <param name="metadataInfo"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public string UpdateAreaMetadataInfo(string metadataInfo, string layerName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.AreaMapMetadata + " SET " + DIColumns.Area_Map_Metadata.MetadataText + "='" + metadataInfo + "' " + " WHERE "
                + DIColumns.Area_Map_Metadata.LayerName + "='" + layerName + "' ";

            return RetVal;
        }

        /// <summary>
        /// Returns query to update area block with new area nids
        /// </summary>
        /// <param name="areaBlock"></param>
        /// <param name="areaID"></param>
        /// <returns></returns>
        public string UpdateAreaBlocks(string areaBlock, string areaID)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + this.TablesName.Area + " SET " + DIColumns.Area.AreaBlock + "='" + areaBlock + "'"
                + " Where " + DIColumns.Area.AreaID + " ='" + areaID + "'";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Area_en</param>
        /// <param name="targetTableName">Target table name like UT_Area_fr</param>
        /// <returns></returns>
        public string UpdateMissingAreaLanguageValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.Area.AreaNId + " , " + DIColumns.Area.AreaParentNId + " , " + DIColumns.Area.AreaID + " , " + DIColumns.Area.AreaName + " , " + DIColumns.Area.AreaGId + " , " + DIColumns.Area.AreaLevel + " , " + DIColumns.Area.AreaMap + " , " + DIColumns.Area.AreaBlock + " , " + DIColumns.Area.AreaGlobal + " )" +
                " SELECT  " + DIColumns.Area.AreaNId + " , " + DIColumns.Area.AreaParentNId + " , " + DIColumns.Area.AreaID + " ,'" + TextPrefix + "' &  " + DIColumns.Area.AreaName + " , " + DIColumns.Area.AreaGId + " , " + DIColumns.Area.AreaLevel + " , " + DIColumns.Area.AreaMap + " , " + DIColumns.Area.AreaBlock + " , " + DIColumns.Area.AreaGlobal + " " + " FROM " + sourceTableName + " " + " WHERE  " + DIColumns.Area.AreaNId + "  not in (SELECT DISTINCT  " + DIColumns.Area.AreaNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Area_level_en</param>
        /// <param name="targetTableName">Target table name like UT_Area_level_fr</param>
        /// <returns></returns>
        public string UpdateMissingAreaLevelLanguageValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.Area_Level.LevelNId + " , " + DIColumns.Area_Level.AreaLevel + " ," + DIColumns.Area_Level.AreaLevelName + " )" +
                " SELECT  " + DIColumns.Area_Level.LevelNId + " ," + DIColumns.Area_Level.AreaLevel + " ,'" + TextPrefix + "' & " + DIColumns.Area_Level.AreaLevelName + "   FROM " + sourceTableName + " " +
                " WHERE  " + DIColumns.Area_Level.LevelNId + "  not in (SELECT DISTINCT  " + DIColumns.Area_Level.LevelNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Area_Map_Metadata_en</param>
        /// <param name="targetTableName">Target table name like UT_Area_Map_Metadata_fr</param>
        /// <returns></returns>
        public string UpdateMissingAreaMapMetadataLanguageValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + " ( " + DIColumns.Area_Map_Metadata.MetadataNId + " , " + DIColumns.Area_Map_Metadata.LayerNId + " , " + DIColumns.Area_Map_Metadata.MetadataText + " , " + DIColumns.Area_Map_Metadata.LayerName + " )" +
                " SELECT  " + DIColumns.Area_Map_Metadata.MetadataNId + " , " + DIColumns.Area_Map_Metadata.LayerNId + " , " + DIColumns.Area_Map_Metadata.MetadataText + " ,'" + TextPrefix + "' &  " + DIColumns.Area_Map_Metadata.LayerName + "  " + " FROM " + sourceTableName + " " +
                " WHERE  " + DIColumns.Area_Map_Metadata.MetadataNId + "  not in (SELECT DISTINCT  " + DIColumns.Area_Map_Metadata.MetadataNId + "  FROM " + targetTableName + ")";
            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Area_Feature_Type_en</param>
        /// <param name="targetTableName">Target table name like UT_Area_Feature_Type_fr</param>
        /// <returns></returns>
        public string UpdateMissingAreaFeatureLanguageValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.Area_Feature_Type.FeatureTypeNId + " ,  " + DIColumns.Area_Feature_Type.FeatureType + " )" +
                " SELECT  " + DIColumns.Area_Feature_Type.FeatureTypeNId + " ,'" + TextPrefix + "' &  " + DIColumns.Area_Feature_Type.FeatureType + "  " + " FROM " + sourceTableName + " " +
                " WHERE  " + DIColumns.Area_Feature_Type.FeatureTypeNId + "  not in (SELECT DISTINCT  " + DIColumns.Area_Feature_Type.FeatureTypeNId + "  FROM " + targetTableName + ")";
            return RetVal;
        }


        #region "-- DataExists --"

        /// <summary>
        /// Returns sql query to update the data exists value to false for all areas
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public string UpdateDataExistToFalse(DIServerType serverType)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;
                case DIServerType.MsAccess:
                    RetVal = "UPDATE " + this.TablesName.Area + " AS A SET A." + DIColumns.Area.DataExist + "=false;";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update the data exists values
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public string UpdateDataExistValues(DIServerType serverType)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;
                case DIServerType.MsAccess:
                    RetVal = "UPDATE " + this.TablesName.Area + " AS A   SET A." + DIColumns.Area.DataExist + "=true where Exists (select * from  " + this.TablesName.Data + " AS D where A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + ")";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update the data exists values in the given language form the default area language table
        /// </summary>
        /// <param name="targetLanguageTablesName"></param>
        /// <returns></returns>
        public string UpdateDataExistValuesInOtherLangauge(DITables targetLanguageTablesName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.Area + " AS A INNER JOIN " + targetLanguageTablesName.Area + " AS A1 ON A." + DIColumns.Area.AreaNId + " = A1." + DIColumns.Area.AreaNId + " SET A1." + DIColumns.Area.DataExist + " = A." + DIColumns.Area.DataExist + " ";

            return RetVal;
        }


        #endregion

        #endregion

        #endregion

    }
}