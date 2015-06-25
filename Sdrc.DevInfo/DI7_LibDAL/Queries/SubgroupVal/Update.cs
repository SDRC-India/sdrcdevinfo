using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "subgroup";
        private const string SubgroupValTableName = "Subgroup_Vals";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Returns query to Update Age, Sex , location , Others in SubgroupVal
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="updateColumnName"></param>
        /// <param name="subgroupNID"></param>
        ///  <param name="subgroupValNId"></param>
        /// <returns></returns>
        public static string UpdateAgeSexLocationNOthersInSubgroupVal(string dataPrefix, string languageCode, string updateColumnName, int subgroupNID, int subgroupValNId)
        {
            string RetVal = string.Empty;

            RetVal = "update  " + dataPrefix + Update.SubgroupValTableName + languageCode + "  Set " + updateColumnName + "  =  " + subgroupNID.ToString()
                + "  where " + DIColumns.SubgroupVals.SubgroupValNId + "  =" + subgroupValNId.ToString();

            return RetVal;
        }

        /// <summary>
        /// Returns query to Update Age, Sex , location , Others in SubgroupVal
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="updateColumnName"></param>
        /// <param name="subgroupNID"></param>
        ///  <param name="subgroupValNId"></param>
        /// <returns></returns>
        public static string SetAgeSexLocationNOthersToZero(string dataPrefix, string languageCode, int subgroupValNId)
        {
            string RetVal = string.Empty;

            RetVal = "update  " + dataPrefix + Update.SubgroupValTableName + languageCode + "  Set " + DIColumns.SubgroupValRemovedColumns.SubgroupValAge + "  =  0 , " 
                + DIColumns.SubgroupValRemovedColumns.SubgroupValLocation + "  =  0 ,"
                    + DIColumns.SubgroupValRemovedColumns.SubgroupValOthers + "  =  0 ,"
                        + DIColumns.SubgroupValRemovedColumns.SubgroupValSex+ "  =  0 "
                + "  where " + DIColumns.SubgroupVals.SubgroupValNId + "  =" + subgroupValNId.ToString();

            return RetVal;
        }
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
                + DIColumns.SubgroupVals.SubgroupVal + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupVal) + "',"
                + DIColumns.SubgroupVals.SubgroupValGId + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupValGid) + "',"
                + DIColumns.SubgroupVals.SubgroupValGlobal + "=" + DIConnection.GetBoolValue(isGlobal) 
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }


        /// <summary>
        /// Update subgroupVal GID record into subgroupVal Table
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
       /// <param name="subgroupValGid">SubgroupVal GId </param>
        /// <param name="subgroupValNid">SubgroupVal Nid</param>
        /// <returns></returns>        
        public static string UpdateSubgroupValGID(string dataPrefix, string languageCode,  string subgroupValGid, int subgroupValNid)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
                + DIColumns.SubgroupVals.SubgroupValGId + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupValGid) + "' "
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }

        /// <summary>
        /// Update SubgroupVal with SubgroupOrder
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="subgroupVal"></param>
        /// <param name="subgroupValGid"></param>
        /// <param name="isGlobal"></param>
        /// <param name="subgroupValNid"></param>
        /// <param name="Order"></param>
        /// <returns></returns>
        public static string UpdateSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, int subgroupValNid,int Order)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
                + DIColumns.SubgroupVals.SubgroupVal + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupVal) + "',"
                + DIColumns.SubgroupVals.SubgroupValGId + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupValGid) + "',"
                + DIColumns.SubgroupVals.SubgroupValGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + ","
                + DIColumns.SubgroupVals.SubgroupValOrder + "=" + Order
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }

       /// <summary>
        /// Update SubgroupVal with SubgroupOrder
       /// </summary>
       /// <param name="dataPrefix"></param>
       /// <param name="languageCode"></param>
       /// <param name="subgroupValNid"></param>
       /// <param name="Order"></param>
       /// <returns></returns>
        public static string UpdateSubgroupValOrder(string dataPrefix, string languageCode, int subgroupValNid, int Order)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
                + DIColumns.SubgroupVals.SubgroupValOrder + "=" + Order
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }

        /// <summary>
        /// Update subgroupVal record into subgroupVal Table
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="subgroupVal">Subgroup val name</param>
        /// <param name="subgroupValNid">SubgroupVal Nid</param>
        /// <returns></returns>        
        public static string UpdateSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, int subgroupValNid)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.SubgroupValTableName + languageCode + " SET "
                + DIColumns.SubgroupVals.SubgroupVal + "='" + DIQueries.RemoveQuotesForSqlQuery(subgroupVal) + "'"
                + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + "=" + subgroupValNid;

            return RetVal;
        }


        #endregion

        #endregion
    }
}
