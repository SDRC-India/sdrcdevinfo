using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Unit
{

    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        /// <summary>
        /// Returns query to delete units
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="nids"></param>
        /// <returns></returns>
        public static string DeleteUnits(string TableName, string nids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.Unit.UnitNId + " IN( " + nids + " )";

            return RetVal;
        }
    }
}
