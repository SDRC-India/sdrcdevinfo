using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "subgroup_type";
        private const string SubgroupValTableName = "subgroup_vals";

        #endregion

         #region "-- Methods --"

        /// <summary>
        /// Update Subgroup Type Row Into Subgroup TypeTable
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupTypeName">Subgroup type Name</param>
        /// <param name="GID">Subgroup Gid</param>
        /// <param name="isGlobal">true/false .True if it is global </param>
        /// <param name="order">order </param>
        /// <param name="Nid"> Subgorup Type nid</param>
        /// <returns></returns>
        public static string UpdateSubgroupTypeByNid(string dataPrefix, string languageCode, string subgroupTypeName, string GID, bool isGlobal, int order, int Nid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
                + DIColumns.SubgroupTypes.SubgroupTypeName + "= '" + DIQueries.RemoveQuotesForSqlQuery(subgroupTypeName) + "',"
                + DIColumns.SubgroupTypes.SubgroupTypeGID + "='" + DIQueries.RemoveQuotesForSqlQuery(GID) + "',"
                + DIColumns.SubgroupTypes.SubgroupTypeGlobal+ "=" + isGlobal + ","
           + DIColumns.SubgroupTypes.SubgroupTypeOrder + "=" + order + " WHERE " + DIColumns.SubgroupTypes.SubgroupTypeNId + "=" + Nid;

            return RetVal;
        }

        /// <summary>
        /// Update Subgroup Dimension Order Bu SubgroupType_NId
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="NId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string UpdateSubgroupTypeOrderByNid(string dataPrefix, string languageCode, int NId, int order)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET "
              + DIColumns.SubgroupTypes.SubgroupTypeOrder + "=" + order + " WHERE " + DIColumns.SubgroupTypes.SubgroupTypeNId + "=" + NId;

            return RetVal;
        }

        
        #endregion

        #endregion

  
    }
}

