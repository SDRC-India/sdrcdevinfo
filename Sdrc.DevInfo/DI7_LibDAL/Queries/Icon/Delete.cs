using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Icon
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "icons";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"


        /// <summary>
        /// Delete icon based on Element Type and Element NIds
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="elementType">'I', 'A','S', 'U', 'C', 'D', 'MI', 'MS', 'MA'</param>
        /// <param name="elementNIds">comma delimited element nids</param>
        /// <returns></returns>
        public static string DeleteIcon(string dataPrefix, string elementType, string elementNIds)
        {
            string RetVal = string.Empty;
            //-- handle quotes around elementType if they exists 'I', 'A','S', 'U', 'C', 'D', 'MI', 'MS', 'MA'
            RetVal = "DELETE FROM " + dataPrefix + TableName + " WHERE " + DIColumns.Icons.ElementType + " = '" + elementType.Replace("'", "") + "' AND " + DIColumns.Icons.ElementNId + " IN (" + elementNIds + ")"; 
            
            return RetVal;
        }

        /// <summary>
        /// Clear icons related to an element type
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="elementType">'I', 'A','S', 'U', 'C', 'D', 'MI', 'MS', 'MA'</param>
        /// <returns></returns>
        /// <remarks>Typically used to clear all icons related to map metadata</remarks>
        public static string ClearIcon(string dataPrefix, string elementType)
        {
            string RetVal = string.Empty;
            //-- handle quotes around elementType if they exists 'I', 'A','S', 'U', 'C', 'D', 'MI', 'MS', 'MA'
            RetVal = "DELETE FROM " + dataPrefix + TableName + " WHERE " + DIColumns.Icons.ElementType + " = '" + elementType.Replace("'", "") + "'";

            return RetVal;
        }


        #endregion

        #endregion
    }

}