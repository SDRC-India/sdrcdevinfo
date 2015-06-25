using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Create
    {
        #region "-- Private --"

        #region "-- Variables --"


        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        public static string CreateColumn(string tableName, string columnName, string columnType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tableName + " ADD COLUMN  " + columnName + " ";
            RetVal += " " + columnType + " ";

            return RetVal;
        }

        #endregion

        #endregion
    }
}
