using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Indicator
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "indicator";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"


        /// <summary>
        /// Update indicator into template or database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name"></param>
        /// <param name="GID"></param>
        /// <param name="isGlobal"></param>
        /// <param name="indicatorInfo"></param>
        /// <param name="nid"></param>
        /// <returns></returns>        
        public static string UpdateByNid(string dataPrefix, string languageCode, string name, string GID, bool isGlobal, string indicatorInfo, int nid)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " set "
                + DIColumns.Indicator.IndicatorName + "='" + DIQueries.RemoveQuotesForSqlQuery(name) + "',  "
                + DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(GID) + "', "
                + DIColumns.Indicator.IndicatorGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + ","
                + DIColumns.Indicator.IndicatorInfo + "='" + indicatorInfo + "' "
                + " WHERE " + DIColumns.Indicator.IndicatorNId + "=" + nid;
            return RetVal;
        }

        public static string UpdateByNid(string dataPrefix, string languageCode, string name, string GID, bool isGlobal, int nid)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " set "
                + DIColumns.Indicator.IndicatorName + "='" + DIQueries.RemoveQuotesForSqlQuery(name) + "',  "
                + DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(GID) + "', "
                + DIColumns.Indicator.IndicatorGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + " "

                + " WHERE " + DIColumns.Indicator.IndicatorNId + "=" + nid;
            return RetVal;
        }

        public static string UpdateByNid(string dataPrefix, string languageCode, string name, string GID, bool isGlobal, string indicatorInfo, int nid, bool highIsGood)
        {
            string RetVal = string.Empty;
            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " set "
                + DIColumns.Indicator.IndicatorName + "='" + DIQueries.RemoveQuotesForSqlQuery(name) + "',  "
                + DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(GID) + "', "
                + DIColumns.Indicator.IndicatorGlobal + "=" + DIConnection.GetBoolValue(isGlobal) + ","
                + DIColumns.Indicator.IndicatorInfo + "='" + indicatorInfo + "', "
                 + DIColumns.Indicator.HighIsGood + "=" + DIConnection.GetBoolValue(highIsGood)
                + " WHERE " + DIColumns.Indicator.IndicatorNId + "=" + nid;
            return RetVal;
        }

        /// <summary>
        /// Update indicator info on the basis of Nid or GId.
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="Info"></param>
        /// <param name="filterType">only NId or GId</param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public static string UpdateIndicatorInfo(string dataPrefix, string languageCode, string Info, FilterFieldType filterType, string filterString)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET " + DIColumns.Indicator.IndicatorInfo + "='" + Info + "' " + " WHERE ";

            switch (filterType)
            {
                case FilterFieldType.GId:
                    RetVal += DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(filterString) + "' ";
                    break;
                case FilterFieldType.NId:
                    RetVal += DIColumns.Indicator.IndicatorNId + "=" + filterString;
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Update indicator name on the basis of Nid.
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="indicatorName"></param>
        /// <param name="indicatorNId"></param>
        /// <returns></returns>
        public static string UpdateIndicatorName(string dataPrefix, string languageCode, string indicatorName, int indicatorNId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET " + DIColumns.Indicator.IndicatorName + "='" + DIQueries.RemoveQuotesForSqlQuery(indicatorName) + "' " + " WHERE ";

            RetVal += DIColumns.Indicator.IndicatorNId + "=" + indicatorNId;

            return RetVal;
        }


        /// <summary>
        /// Update indicator GId on the basis of Nid.
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="indicatorGId"></param>
        /// <param name="indicatorNId"></param>
        /// <returns></returns>
        public static string UpdateIndicatorGId(string dataPrefix, string languageCode, string indicatorGId, int indicatorNId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET " + DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(indicatorGId) + "' " + " WHERE ";

            RetVal += DIColumns.Indicator.IndicatorNId + "=" + indicatorNId;

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Indicator_en</param>
        /// <param name="targetTableName">Target table name like UT_Indicator_fr</param>
        /// <returns></returns>
        public static string UpdateMissingTextValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.Indicator.IndicatorNId + " , " + DIColumns.Indicator.IndicatorName + " , " + DIColumns.Indicator.IndicatorGId + " , " + DIColumns.Indicator.IndicatorInfo + " , " + DIColumns.Indicator.IndicatorGlobal + " )" +
                " SELECT  " + DIColumns.Indicator.IndicatorNId + " ,'" + TextPrefix + "' &  " + DIColumns.Indicator.IndicatorName + " , " + DIColumns.Indicator.IndicatorGId + " , " + DIColumns.Indicator.IndicatorInfo + " , " + DIColumns.Indicator.IndicatorGlobal + " " + " FROM " + sourceTableName + " AS I " +
                " WHERE I." + DIColumns.Indicator.IndicatorNId + "  not in (SELECT DISTINCT  " + DIColumns.Indicator.IndicatorNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }

        #region "-- DataExist and Order --"

        /// <summary>
        /// Returns sql query to update dataexist to false for all records
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string UpdateDataExistToFalse(DIServerType serverType, string tableName)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:

                    break;
                case DIServerType.MsAccess:
                    RetVal = "UPDATE " + tableName + " AS I SET I." + DIColumns.Indicator.DataExist + "=false";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update dataexist to true but where data exists
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tablesName"></param>
        /// <returns></returns>
        public static string UpdateDataExistValues(DIServerType serverType, DITables tablesName)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;
                case DIServerType.MsAccess:
                    RetVal = " UPDATE " + tablesName.Indicator + "  AS I  SET I." + DIColumns.Indicator.DataExist + "=True where Exists ( select * from " + tablesName.Data + " D where I." + DIColumns.Indicator.IndicatorNId + "=D." + DIColumns.Data.IndicatorNId + ") ";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update dataexist values into target table 
        /// </summary>
        /// <param name="sourceTableName"></param>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string UpdateDataExistValuesInOtherLanguage(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + sourceTableName + " AS I INNER JOIN " + targetTableName + " AS I1 ON I." + DIColumns.Indicator.IndicatorNId + " = I1." + DIColumns.Indicator.IndicatorNId + " SET  I1." + DIColumns.Indicator.DataExist + "= I." + DIColumns.Indicator.DataExist + " ";

            return RetVal;
        }

        /// <summary>
        /// Update HIgh is Good
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tableName"></param>
        /// <param name="IndicatorNid"></param>
        /// <param name="isHighGood"></param>
        /// <returns></returns>
        public static string UpdateISHighGood(DIServerType serverType, string tableName, int IndicatorNid, bool isHighGood)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.Oracle:
                case DIServerType.MySql:
                case DIServerType.SqlServerExpress:
                case DIServerType.SqlServer:
                    RetVal = "UPDATE " + tableName + " SET " + DIColumns.Indicator.HighIsGood + "=" + Convert.ToString(isHighGood ? 1 : 0);
                    break;
                case DIServerType.Excel:
                case DIServerType.MsAccess:
                    RetVal = "UPDATE " + tableName + " SET " + DIColumns.Indicator.HighIsGood + "=" + isHighGood.ToString();
                    break;
                default:
                    break;
            }

            RetVal += " WHERE " + DIColumns.Indicator.IndicatorNId + "=" + IndicatorNid.ToString();

            return RetVal;
        }
        #endregion

        #endregion

        #endregion

    }
}