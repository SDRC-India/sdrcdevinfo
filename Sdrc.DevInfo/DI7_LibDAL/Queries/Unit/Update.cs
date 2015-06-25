using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Unit
{

    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "unit";

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"
        
        /// <summary>
        /// Update Unit into Unit Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="unitName"></param>
        /// <param name="unitGID"></param>
        /// <param name="isGlobal"></param>
        /// <param name="unitNid"></param>
        /// <returns></returns>
        public static string UpdateByNid(string dataPrefix, string languageCode, string unitName, string unitGID, bool isGlobal,int unitNid)
        {
            string RetVal = string.Empty;
            RetVal= "UPDATE " + dataPrefix + Update.TableName + languageCode +" set "
                + DIColumns.Unit.UnitName + "='" + DIQueries.RemoveQuotesForSqlQuery(unitName) + "',  "
                + DIColumns.Unit.UnitGId + "='" + DIQueries.RemoveQuotesForSqlQuery(unitGID) + "', " 
                + DIColumns.Unit.UnitGlobal +"=" + DIConnection.GetBoolValue(isGlobal)
                + " WHERE " + DIColumns.Unit.UnitNId + "=" + unitNid ;            
            return RetVal;
        }

        /// <summary>
        /// Update Unit into Unit Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="unitName"></param>
        /// <param name="unitNid"></param>
        /// <returns></returns>
        public static string UpdateNameByNid(string dataPrefix, string languageCode, string unitName, int unitNid)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " set "
                + DIColumns.Unit.UnitName + "='" + DIQueries.RemoveQuotesForSqlQuery(unitName) + "',  "
                + " WHERE " + DIColumns.Unit.UnitNId + "=" + unitNid;
            return RetVal;
        }

        /// <summary>
        /// Update Unit name into Unit Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="unitName"></param>
        /// <param name="unitNid"></param>
        /// <returns></returns>
        public static string UpdateUnitNameByNid(string dataPrefix, string languageCode, string unitName, int unitNid)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " set "
                + DIColumns.Unit.UnitName + "='" + DIQueries.RemoveQuotesForSqlQuery(unitName) + "'"
                + " WHERE " + DIColumns.Unit.UnitNId + "=" + unitNid;
            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Unit_en</param>
        /// <param name="targetTableName">Target table name like UT_Unit_fr</param>
        /// <returns></returns>
        public static string UpdateMissingTextValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.Unit.UnitNId + " , " + DIColumns.Unit.UnitName + " , " + DIColumns.Unit.UnitGId + " , " + DIColumns.Unit.UnitGlobal + " )" +
                " SELECT  " + DIColumns.Unit.UnitNId + " ,'" + TextPrefix + "' &  " + DIColumns.Unit.UnitName + " , " + DIColumns.Unit.UnitGId + " , " + DIColumns.Unit.UnitGlobal + "  FROM " + sourceTableName + " AS U " +
                " WHERE U." + DIColumns.Unit.UnitNId + " not in (SELECT DISTINCT  " + DIColumns.Unit.UnitNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }


        #endregion

        #endregion

    }
}