using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Public --"

        #region "-- Methods --"

        
        /// <summary>
        /// Returns a query to create UT_Subgroup_Val_Subgorup table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateTable(bool forOnlineDB,string dataPrefix, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
             
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + new DITables(dataPrefix,string.Empty).SubgroupValsSubgroup + " (" + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + ")," + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " int(4), " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " int(4))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + new DITables(dataPrefix, string.Empty).SubgroupValsSubgroup + " (" + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + " int Identity(1,1) primary key," + DIColumns.SubgroupValsSubgroup.SubgroupValNId + "  int,  " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " int)";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + new DITables(dataPrefix, string.Empty).SubgroupValsSubgroup + " (" + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + " counter primary key," + DIColumns.SubgroupValsSubgroup.SubgroupValNId + "  Long, " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " Long)";
            }
            return RetVal;

        }

        /// <summary>
        /// Returns a query to insert relationship of subgroup and subgroupVal
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="subgroupValNid"></param>
        /// <param name="subgroupNid"></param>
        /// <returns></returns>
        public static string InsertSubgroupValRelation(string dataPrefix,  int subgroupValNid, int subgroupNid)
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO " + new DITables(dataPrefix,string.Empty).SubgroupValsSubgroup + "(" + 
                DIColumns.SubgroupValsSubgroup.SubgroupNId +"," + DIColumns.SubgroupValsSubgroup.SubgroupValNId
                + ") VAlues (" +
                subgroupNid + "," + subgroupValNid + ")";
            return RetVal;
        }

      

        #endregion

        #endregion
    }
}
