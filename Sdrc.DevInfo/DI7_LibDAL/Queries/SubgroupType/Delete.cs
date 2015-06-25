using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes
{
        /// <summary>
        /// Provides sql queries to delete type records
        /// </summary>
        public static class Delete
        {            
            /// <summary>
            /// Returns query to delete subgroup type
            /// </summary>
            /// <param name="TableName"></param>
            /// <param name="nids"></param>
            /// <returns></returns>
            public static string DeleteSubgroupType(string TableName, string nids)
            {
                string RetVal = string.Empty;

                RetVal = "DELETE FROM " + TableName + " where " + DIColumns.SubgroupTypes.SubgroupTypeNId + " IN( " + nids + " )";

                return RetVal;
            }

        }
   
}
