using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Queries.Icon
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "icons";

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"

       
        /// <summary>
        /// Returns a query to update the field size of ElementType column in UT_ICon table
        /// </summary>
        /// <param name="tableName"> UT_Icon table name</param>
        /// <returns></returns>
        public static string UpdateElementTypeColumnFiledSize(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tableName + " ALTER "+ DIColumns.Icons.ElementType+"  TEXT(2)";

            return RetVal;
        }

        /// <summary>
        /// Returns a query to update ICON information
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="iconType"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="elementType"></param>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public static string UpdateIcon(string dataPrefix,string iconType, int width, int height, string elementType, string elementNid)
        {
            string RetVal = string.Empty;

            if (iconType == "")
            {
                RetVal = "UPDATE " + dataPrefix + TableName + " set " + Icons.ElementIcon + " = @" + Icons.ElementIcon + " where " + Icons.ElementType + " ='" + elementType + "' and " + Icons.ElementNId + " =" + elementNid;
            }
            else
            {
                RetVal = "UPDATE " + dataPrefix + TableName + " set Icon_Type ='" + iconType + "', " + Icons.IconDimW + "=" + width + " ," + Icons.IconDimH + "=" + height + ",Element_Icon = ? where " + Icons.ElementType + " ='" + elementType + "' and " + Icons.ElementNId + " =" + elementNid;
            } 

            return RetVal;
        }


        #endregion

        #endregion
    }
}
