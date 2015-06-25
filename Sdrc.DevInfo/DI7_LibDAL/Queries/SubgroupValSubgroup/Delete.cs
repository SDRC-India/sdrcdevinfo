using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {

        /// <summary>
        /// Returns query to delete subgroup val relations
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="subgroupValNIds"></param>
        /// <returns></returns>
        public static string DeleteSubgroupValRelations(string TableName, string subgroupValNIds)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN( " + subgroupValNIds + " )";

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete subgroup val relations
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="subgroupValNID"></param>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public static string DeleteSubgroupRelationsFrmSubgroupVal(string TableName,int subgroupValNID  ,string subgroupNIds)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = " + subgroupValNID + " AND " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + 
                subgroupNIds +" )";

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete subgroup val Subgroup Records
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="subgroupValNIds"></param>
        /// <returns></returns>
        public static string DeleteSubgroupValSubgroup(string TableName, string subgroupValSubgroupNIds)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId  + " IN( " + subgroupValSubgroupNIds + " )";

            return RetVal;
        }


    }
}
