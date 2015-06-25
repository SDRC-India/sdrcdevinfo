using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {

        #region "-- Private --"

        #region "-- Variables --"
               
        #endregion

        #endregion


        #region "-- Public --"

        /// <summary>
        /// To update Recommended Source value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="dataNid"></param>
        /// <param name="icIUSRank"></param>
        /// <param name="icIUSLabel"></param>
        /// <returns>sql query</returns>
        public static string UpdateRecommendedSourceValue(string tableName, int dataNid, string icIUSLabel)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.RecommendedSources.ICIUSLabel + "='"
           + icIUSLabel + "' WHERE " + DIColumns.RecommendedSources.DataNId + " = " + dataNid;

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_RecommendedSources_en</param>
        /// <param name="targetTableName">Target table name like UT_RecommendedSources_fr</param>
        /// <returns></returns>
        public static string UpdateMissingTextValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( "
                + DIColumns.RecommendedSources.NId+ " , "
                + DIColumns.RecommendedSources.DataNId+ " , "
                + DIColumns.RecommendedSources.ICIUSLabel + " )" +
                " SELECT  "
                + DIColumns.RecommendedSources.NId + ","
                + DIColumns.RecommendedSources.DataNId 
                + " ,'" + TextPrefix + "' &  "
                + DIColumns.RecommendedSources.ICIUSLabel 
                + "  " + " FROM " + sourceTableName + " As T " 
                + " WHERE  "
                + "  NOT EXISTS (SELECT "
                + DIColumns.RecommendedSources.NId+ "  FROM " + targetTableName + " as T1 WHERE T1." + DIColumns.RecommendedSources.NId + " = T." + DIColumns.RecommendedSources.NId + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to change data type of Data_NID column to LongInteger
        /// </summary>
        /// <param name="tableName">RecommendedSource Tablename</param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string UpdateTypeofDataNIdColumn(string tableName, bool forOnlineDB, DevInfo.Lib.DI_LibDAL.Connection.DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tableName + " ALTER COLUMN  " + DIColumns.RecommendedSources.DataNId+ " ";

            if (forOnlineDB)
            {
               // do nothing
            }
            else
            {
                RetVal += " Long ";
            }

            return RetVal;
        }

        #endregion

    }
}
