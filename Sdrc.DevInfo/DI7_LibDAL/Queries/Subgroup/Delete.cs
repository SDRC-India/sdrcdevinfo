using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Subgroup
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        /// <summary>
        /// Returns a query to delete columns from subgroupVal table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string DeleteColumnForDI6(string tableName,SubgroupType columnName)
        {
            string RetVal=string.Empty;
            RetVal = "ALTER TABLE " + tableName + " DROP COLUMN ";
            switch (columnName)
            {
                case SubgroupType.Sex:
                    RetVal += DIColumns.SubgroupValRemovedColumns.SubgroupValSex;
                    break;
                case SubgroupType.Location:
                    RetVal += DIColumns.SubgroupValRemovedColumns.SubgroupValLocation;
                    break;
                case SubgroupType.Age:
                    RetVal += DIColumns.SubgroupValRemovedColumns.SubgroupValAge;
                    break;
                case SubgroupType.Others:
                    RetVal += DIColumns.SubgroupValRemovedColumns.SubgroupValOthers;
                    break;
                default:
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete subgroupVals
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="nids"></param>
        /// <returns></returns>
        public static string DeleteSubgroupVals(string TableName, string nids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.SubgroupVals.SubgroupValNId + " IN( " + nids + " )";

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete subgroups
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="nids"></param>
        /// <returns></returns>
        public static string DeleteSubgroups(string TableName, string nids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.Subgroup.SubgroupNId + " IN( " + nids + " )";

            return RetVal;
        }
        
    }
}
