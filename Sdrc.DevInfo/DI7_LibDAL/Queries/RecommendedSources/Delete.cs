using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {

        /// <summary>
        /// Returns sql query to delete records from UT_recommendsources_en table for the given dataNIds
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataNIds"></param>
        /// <returns></returns>
        public static string DeleteRecommendedSources(string tableName, string dataNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Delete FROM " + tableName;

            if (!string.IsNullOrEmpty(dataNIds))
            {
                RetVal += " WHERE " + DIColumns.RecommendedSources.DataNId + " IN(" + dataNIds + ")";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to delete extra records from recommended sources
        /// </summary>
        /// <param name="recommendedSourcetableName"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static string DeleteExtraRecords(string recommendedSourcetableName, string dataTableName)
        {
            string RetVal = string.Empty;

            RetVal = "Delete  FROM " + recommendedSourcetableName + " WHERE ( not Exists (Select * from  " + dataTableName + " AS D where " + recommendedSourcetableName + "." + DIColumns.RecommendedSources.DataNId + "=D." + DIColumns.Data.DataNId + ") )";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to delete records from recommended sources where ICIUS Label is empty
        /// </summary>
        /// <param name="recommendedSourcetableName"></param>
        /// <param name="dataTableName"></param>
        /// <returns></returns>
        public static string DeleteRecordsWhereICIUSIsEmpty(string recommendedSourcetableName, string dataTableName)
        {
            string RetVal = string.Empty;

            RetVal = "Delete  FROM " + recommendedSourcetableName + " AS R WHERE R." + DIColumns.RecommendedSources.ICIUSLabel + " is null OR R." + DIColumns.RecommendedSources.ICIUSLabel + " = \"\"";

            return RetVal;
        }

    }
}
