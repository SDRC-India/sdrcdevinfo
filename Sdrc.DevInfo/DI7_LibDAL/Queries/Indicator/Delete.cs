using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibDAL.Queries.Indicator
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        /// <summary>
        /// Returns query to delete indicators
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="indicatorNids"></param>
        /// <returns></returns>
        public static string DeleteIndicator(string TableName,string indicatorNids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName + " where " + DIColumns.Indicator.IndicatorNId + " IN( " + indicatorNids +" )";

            return RetVal;
        }
                
    }
}
