using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Area
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "area";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Returns query to insert record into area level table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="areaLevel"></param>
        /// <param name="areaLevelName"></param>
        /// <returns></returns>
        public static string InsertAreaLevel(string tableName, string areaLevel, string areaLevelName)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tableName + " (" + DIColumns.Area_Level.AreaLevel + ", " + DIColumns.Area_Level.AreaLevelName + ") VALUES(" + areaLevel + ",'" + areaLevelName + "')";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to insert area
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="areaParentNid"></param>
        /// <param name="areaID"></param>
        /// <param name="areaName"></param>
        /// <param name="areaGid"></param>
        /// <param name="areaLevel"></param>
        /// <param name="areaMap"></param>
        /// <param name="areaBlock"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public static string InsertArea(string dataPrefix, string languageCode, int areaParentNid, string areaID, string areaName, string areaGid, int areaLevel, string areaMap, string areaBlock, bool isGlobal)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.Area.AreaParentNId + ","
            + DIColumns.Area.AreaID + ","
            + DIColumns.Area.AreaName + ","
            + DIColumns.Area.AreaGId + ","
            + DIColumns.Area.AreaLevel + ","
            + DIColumns.Area.AreaMap + ","
            + DIColumns.Area.AreaBlock + ","
            + DIColumns.Area.AreaGlobal + ")"
            + "VALUES(" + areaParentNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(areaID) + "','" + DIQueries.RemoveQuotesForSqlQuery(areaName) + "','" + DIQueries.RemoveQuotesForSqlQuery(areaGid) + "',"
            + areaLevel + ",'" + areaMap + "','" + areaBlock + "'," + isGlobal + " ) ";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to insert area
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="areaParentNid"></param>
        /// <param name="areaID"></param>
        /// <param name="areaName"></param>
        /// <param name="areaGid"></param>
        /// <param name="areaLevel"></param>
        /// <param name="areaMap"></param>
        /// <param name="areaBlock"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public static string InsertArea(string dataPrefix, string languageCode, int areaParentNid, string areaID, string areaName, string areaGid, int areaLevel, string areaMap, string areaBlock, bool isGlobal, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.Area.AreaParentNId + ","
            + DIColumns.Area.AreaID + ","
            + DIColumns.Area.AreaName + ","
            + DIColumns.Area.AreaGId + ","
            + DIColumns.Area.AreaLevel + ","
            + DIColumns.Area.AreaMap + ","
            + DIColumns.Area.AreaBlock + ","
            + DIColumns.Area.AreaGlobal + ")"
            + "VALUES(" + areaParentNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(areaID) + "','" + DIQueries.RemoveQuotesForSqlQuery(areaName) + "','" + DIQueries.RemoveQuotesForSqlQuery(areaGid) + "',"
            + areaLevel + ",'" + areaMap + "','" + areaBlock + "',";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    if (isGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isGlobal + " ) ";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    if (isGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                case DIServerType.Sqlite:
                    if (isGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;
                default:
                    RetVal += isGlobal + " ) ";
                    break;
            }   

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to insert area map feature
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="featureType"></param>
        /// <returns></returns>
        public static string InsertAreaMapFeature(string dataPrefix, string languageCode, string featureType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + "Area_Feature_Type" + languageCode + " "
            + " (" + DIColumns.Area_Feature_Type.FeatureType + ") VALUES('" + featureType + "')";

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to insert area map
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="areaNId"></param>
        /// <param name="featureLayer"></param>
        /// <param name="featureTyepNId"></param>
        /// <param name="layerNId"></param>
        /// <returns></returns>
        public static string InsertAreaMap(string dataPrefix, string languageCode, int areaNId, bool featureLayer, int featureTyepNId, int layerNId)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + "Area_Map "
            + " (" + DIColumns.Area_Map.AreaNId + "," + DIColumns.Area_Map.FeatureLayer + "," + DIColumns.Area_Map.FeatureTypeNId + "," + DIColumns.Area_Map.LayerNId
            + ") VALUES(" + areaNId + "," + Convert.ToInt32(featureLayer) + "," + featureTyepNId + "," + layerNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns parameterized sql query to insert map (binary blob objects) alongwith other field values
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="layerSize"></param>
        /// <param name="layerType"></param>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="updateTimestamp"></param>
        /// <returns></returns>
        public static string InsertLayer(string dataPrefix, string layerSize, int layerType, float minX, float minY, float maxX, float maxY, DateTime startDate, DateTime endDate, DateTime updateTimestamp)
        {
            //-- Use CultureInfo.InvariantCulture to avoid decimal symbol being converted to comma in french regional settings  for minx... other wise wrong sql syntax with extra commas
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + "Area_Map_Layer"
            + " (" + DIColumns.Area_Map_Layer.LayerSize + "," + DIColumns.Area_Map_Layer.LayerType
            + "," + DIColumns.Area_Map_Layer.MinX + "," + DIColumns.Area_Map_Layer.MinY + "," + DIColumns.Area_Map_Layer.MaxX + "," + DIColumns.Area_Map_Layer.MaxY
            + "," + DIColumns.Area_Map_Layer.StartDate + "," + DIColumns.Area_Map_Layer.EndDate + "," + DIColumns.Area_Map_Layer.UpdateTimestamp
            + "," + DIColumns.Area_Map_Layer.LayerShp + "," + DIColumns.Area_Map_Layer.LayerShx + "," + DIColumns.Area_Map_Layer.Layerdbf
            + ") VALUES ('" + layerSize + "'," + layerType + "," + minX.ToString(CultureInfo.InvariantCulture) + "," + minY.ToString(CultureInfo.InvariantCulture) + "," + maxX.ToString(CultureInfo.InvariantCulture) + "," + maxY.ToString(CultureInfo.InvariantCulture) + ",'" + startDate + "','" + endDate + "','" + updateTimestamp + "',?,?,?)";

            return RetVal;

        }

        /// <summary>
        /// Returns parameterized sql query to insert map (binary blob objects) alongwith other field values
        /// with Param Name for Online database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="layerSize"></param>
        /// <param name="layerType"></param>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="updateTimestamp"></param>
        /// <returns></returns>
        public static string InsertLayerWParamName(string dataPrefix, string layerSize, int layerType, float minX, float minY, float maxX, float maxY, DateTime startDate, DateTime endDate, DateTime updateTimestamp)
        {
            //-- Use CultureInfo.InvariantCulture to avoid decimal symbol being converted to comma in french regional settings  for minx... other wise wrong sql syntax with extra commas
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + "Area_Map_Layer"
            + " (" + DIColumns.Area_Map_Layer.LayerSize + "," + DIColumns.Area_Map_Layer.LayerType
            + "," + DIColumns.Area_Map_Layer.MinX + "," + DIColumns.Area_Map_Layer.MinY + "," + DIColumns.Area_Map_Layer.MaxX + "," + DIColumns.Area_Map_Layer.MaxY
            + "," + DIColumns.Area_Map_Layer.StartDate + "," + DIColumns.Area_Map_Layer.EndDate + "," + DIColumns.Area_Map_Layer.UpdateTimestamp
            + "," + DIColumns.Area_Map_Layer.LayerShp + "," + DIColumns.Area_Map_Layer.LayerShx + "," + DIColumns.Area_Map_Layer.Layerdbf
            + ") VALUES ('" + layerSize + "'," + layerType + "," + minX.ToString(CultureInfo.InvariantCulture) + "," + minY.ToString(CultureInfo.InvariantCulture) + "," + maxX.ToString(CultureInfo.InvariantCulture) + "," + maxY.ToString(CultureInfo.InvariantCulture) + ",'" + startDate + "','" + endDate + "','" + updateTimestamp + "', @" + DIColumns.Area_Map_Layer.LayerShp + ",@" + DIColumns.Area_Map_Layer.LayerShx + ", @" + DIColumns.Area_Map_Layer.Layerdbf + ")";

            return RetVal;

        }

        /// <summary>
        /// Returns sql query to insert layer metadata
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="metaData"></param>
        /// <param name="layerNId"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public static string InsertAreaMapMetadata(string dataPrefix, string languageCode, string metaData, int layerNId, string layerName)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + "Area_Map_Metadata" + languageCode + " "
                + " (" + DIColumns.Area_Map_Metadata.LayerNId + ", " + DIColumns.Area_Map_Metadata.MetadataText + ", " + DIColumns.Area_Map_Metadata.LayerName + ") VALUES(" + layerNId + ",'" + metaData + "','" + layerName + "')";

            return RetVal;
        }



        /// <summary>
        /// Returns qurey to insert Data_Exist column into UT_Area_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDataExistColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Area + " ADD COLUMN  " + DIColumns.Area.DataExist + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert AreaShortName column into UT_Area_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AddAreaShortNameColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Area + " ADD COLUMN  " + DIColumns.Area.AreaShortName + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " nvarchar(255) ";
                }
            }
            else
            {
                RetVal += " Text(255) ";
            }

            return RetVal;
        }

  

        #endregion

        #endregion
    }
}
