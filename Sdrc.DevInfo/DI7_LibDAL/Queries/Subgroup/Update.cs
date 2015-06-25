using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Subgroup
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "subgroup";
        private const string SubgroupValTableName = "subgroup_vals";

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"



        /// <summary>
        /// Returns a query to alter the column's data type of "Type" column in Subgroup table to Integer
        /// </summary>
        /// <param name="tableName">Subgroup table name</param>
        /// <param name="forOnlineDB"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string UpdateSubgroupTypeDataType(string tableName, bool forOnlineDB,  DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {

                if (serverType == DIServerType.MySql)
                {
                    RetVal = "ALTER TABLE " + tableName + " ALTER " + DIColumns.Subgroup.SubgroupType + "  int(4) ";
                                        
                }
                else
                {
                    RetVal = "ALTER TABLE " + tableName + " ALTER " + DIColumns.Subgroup.SubgroupType + "  int ";                    
                }
            }
            else
            {
                RetVal = "ALTER TABLE " + tableName + " ALTER " + DIColumns.Subgroup.SubgroupType + "  Long ";

                
            }
            return RetVal;

        }
         

        /// <summary>
        /// Update Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <param name="Nid"> Subgorup nid</param>
        /// <returns></returns>
        public static string UpdateSubgroupByNid(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, int subgroupType, int Nid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
                + DIColumns.Subgroup.SubgroupName + "= '" + DIQueries.RemoveQuotesForSqlQuery(  subgroupName) + "',"
                + DIColumns.Subgroup.SubgroupGId + "='" + DIQueries.RemoveQuotesForSqlQuery( subgroupGID) + "',"
                + DIColumns.Subgroup.SubgroupGlobal + "=" + isSubgroupGlobal + ","
           + DIColumns.Subgroup.SubgroupType + "=" + subgroupType + " WHERE " + DIColumns.Subgroup.SubgroupNId + "=" + Nid;

            return RetVal;
        }


        /// <summary>
        /// Update Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <param name="Nid"> Subgorup nid</param>
        /// <returns></returns>
        public static string UpdateSubgroupByNid(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, SubgroupType subgroupType, int Nid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
                + DIColumns.Subgroup.SubgroupName + "= '" + DIQueries.RemoveQuotesForSqlQuery( subgroupName) + "',"
                + DIColumns.Subgroup.SubgroupGId + "='" +DIQueries.RemoveQuotesForSqlQuery(  subgroupGID )+ "'," 
                +DIColumns.Subgroup.SubgroupGlobal +"="+ isSubgroupGlobal + ","
           + DIColumns.Subgroup.SubgroupType +"="+ subgroupType + " WHERE " + DIColumns.Subgroup.SubgroupNId + "=" + Nid;
           
            return RetVal;
        }

        /// <summary>
        /// Update Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <param name="Nid"> Subgorup nid</param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string UpdateSubgroupByNid(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, int subgroupType, int Nid,int order)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
                + DIColumns.Subgroup.SubgroupName + "= '" + DIQueries.RemoveQuotesForSqlQuery(subgroupName) + "',"
                + DIColumns.Subgroup.SubgroupGId + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupGID) + "',"
                + DIColumns.Subgroup.SubgroupGlobal + "=" + isSubgroupGlobal + ","
                + DIColumns.Subgroup.SubgroupType + "=" + subgroupType + ","
                + DIColumns.Subgroup.SubgroupOrder + "=" + order
                + " WHERE " + DIColumns.Subgroup.SubgroupNId + "=" + Nid;

            return RetVal;
        }

        /////// <summary>
        /////// Update subgroupVal record into subgroupVal Table
        /////// </summary>
        /////// <param name="dataPrefix">Data prefix like UT_</param>
        /////// <param name="languageCode">Language code like _en </param>
        /////// <param name="subgroupVal">Subgroup val name</param>
        /////// <param name="subgroupValGid">SubgroupVal GId </param>
        /////// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
        /////// <param name="subgroupValAge">Nid of Age subgroup</param>
        /////// <param name="subgroupValSex">subgroupVal sex</param>
        /////// <param name="location">subgroupVal Location</param>
        /////// <param name="subgothers">subgroupVal Others</param>
        /////// <param name="subgroupValNid">SubgroupVal Nid</param>
        /////// <returns></returns>        
        ////public static string UpdateSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, int subgroupValAge, int subgroupValSex, int location, int subgothers, int subgroupValNid)
        ////{
        ////    string RetVal = string.Empty;

        ////    RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
        ////        + DIColumns.SubgroupVals.SubgroupVal + "='" + subgroupVal + "',"
        ////        + DIColumns.SubgroupVals.SubgroupValGId + "='" + subgroupValGid + "',"
        ////        + DIColumns.SubgroupVals.SubgroupValGlobal + "=" + isGlobal + ","
        ////        + DIColumns.SubgroupVals.SubgroupValAge + subgroupValAge + ","
        ////        + DIColumns.SubgroupVals.SubgroupValSex + subgroupValSex + ","
        ////        + DIColumns.SubgroupVals.SubgroupValLocation + location + ","
        ////        + DIColumns.SubgroupVals.SubgroupValOthers + subgothers
        ////        + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;
            
        ////    return RetVal;
        ////}


        /// <summary>
        /// Update subgroupVal record into subgroupVal Table
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="subgroupVal">Subgroup val name</param>
        /// <param name="subgroupValGid">SubgroupVal GId </param>
        /// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
        /// <param name="subgroupValNid">SubgroupVal Nid</param>
        /// <returns></returns>        
        public static string UpdateSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, int subgroupValNid)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
                + DIColumns.SubgroupVals.SubgroupVal + "='" + DIQueries.RemoveQuotesForSqlQuery( subgroupVal) + "',"
                + DIColumns.SubgroupVals.SubgroupValGId + "='" + DIQueries.RemoveQuotesForSqlQuery( subgroupValGid)+ "',"
                + DIColumns.SubgroupVals.SubgroupValGlobal + "=" + isGlobal 
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Subgroup_en</param>
        /// <param name="targetTableName">Target table name like UT_Subgroup_fr</param>
        /// <returns></returns>
        public static string UpdateMissingTextValuesForSubgroup(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( "
                + DIColumns.Subgroup.SubgroupNId + " , "
                + DIColumns.Subgroup.SubgroupName + " , "
                + DIColumns.Subgroup.SubgroupGId + " , "
                + DIColumns.Subgroup.SubgroupGlobal + " , "
                + DIColumns.Subgroup.SubgroupType + " )" +
                " SELECT  "
                + DIColumns.Subgroup.SubgroupNId + " ,'" + TextPrefix + "' &  "
                + DIColumns.Subgroup.SubgroupName + " , "
                + DIColumns.Subgroup.SubgroupGId + " , "
                + DIColumns.Subgroup.SubgroupGlobal + " , "
                + DIColumns.Subgroup.SubgroupType + "  " + " FROM " + sourceTableName + " " +
                " WHERE  "
                + DIColumns.Subgroup.SubgroupNId + "  not in (SELECT DISTINCT  "
                + DIColumns.Subgroup.SubgroupNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Subgroup_vals_en</param>
        /// <param name="targetTableName">Target table name like UT_Subgroup_vals_fr</param>
        /// <returns></returns>
        public static string UpdateMissingTextValuesForSubgroupVal(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( "
                + DIColumns.SubgroupVals.SubgroupValNId + " , "
                + DIColumns.SubgroupVals.SubgroupVal + " , "
                + DIColumns.SubgroupVals.SubgroupValGId + " , "
                + DIColumns.SubgroupVals.SubgroupValGlobal + ") "+
                " SELECT  "
                + DIColumns.SubgroupVals.SubgroupValNId + " ,'" + TextPrefix + "' &  "
                + DIColumns.SubgroupVals.SubgroupVal + " , "
                + DIColumns.SubgroupVals.SubgroupValGId + " , "
                + DIColumns.SubgroupVals.SubgroupValGlobal + "  "
                + " FROM " + sourceTableName + " " +
                " WHERE  " + DIColumns.SubgroupVals.SubgroupValNId + "  not in (SELECT DISTINCT  "
                + DIColumns.SubgroupVals.SubgroupValNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns query to Update OtherTypes which is not in Any type from Subgroup table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="ageNId"></param>
        /// <param name="sexNId"></param>
        /// <param name="locationNId"></param>
        /// <param name="othersNId"></param>
        /// <returns></returns>
        /// <remarks>Used in DI6 to DI5 conversion process</remarks>
        public static string UpdateOtherTypesInSubgroupTable(string dataPrefix, string languageCode,int ageNId, int sexNId, int locationNId, int othersNId)
        {
            string RetVal = string.Empty;
            RetVal = "Update  " + dataPrefix + Update.TableName + languageCode + 
                "  set " + DIColumns.Subgroup.SubgroupType + " = " + othersNId.ToString() +
                " where " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupType +
                "  Not In (" + ageNId.ToString() + "  ,  " + sexNId.ToString() + " , " + locationNId.ToString() + "  )";
            return RetVal;
        }
        


        /// <summary>
        /// Returns query to Update Subgrouptypes in Subgroup table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="oldTypeValue"></param>
        /// <param name="newTypeValue"></param>
        /// <returns></returns>
        /// <remarks>Used in DI6 to DI5 conversion process</remarks>
        public static string UpdateSubgroupTypeInSubgroupTable(string dataPrefix, string languageCode, int oldTypeValue, int newTypeValue)
        {
            string RetVal = string.Empty;

            RetVal = "Update  " + dataPrefix + Update.TableName + languageCode + 
                "  set " + DIColumns.Subgroup.SubgroupType + " = " +
                newTypeValue.ToString() + " where " + DIColumns.Subgroup.SubgroupType +
                "  =" + oldTypeValue.ToString();

            return RetVal;
        }

        /// <summary>
        /// Update Subgroup Order By SubgroupNId
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="subgroupNid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string UpdateSubgroupOrderByNId(string dataPrefix, string languageCode, int subgroupNid, int order)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
                + DIColumns.Subgroup.SubgroupOrder + "=" + order
                + " WHERE " + DIColumns.Subgroup.SubgroupNId + "=" + subgroupNid;

            return RetVal;
        }

        #endregion

        #endregion
    }
}
