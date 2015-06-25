using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "subgroup";
        private const string SubgroupValTableName = "subgroup_vals";

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns qurey to insert SubgroupValOrder column into SubgroupVal table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AddOrderColumn(string dataPrefix,string languageCode, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, languageCode).SubgroupVals + " ADD COLUMN  [" + DIColumns.SubgroupVals.SubgroupValOrder + "] ";
            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " Long ";
            }

            return RetVal;
        }


        /// <summary>
        /// Insert subgroupVal record into subgroupVal Table
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="subgroupVal">Subgroup val name</param>
        /// <param name="subgroupValGid">SubgroupVal GId </param>
        /// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
         /// <returns></returns>        
        public static string InsertSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal)
        {
            return Insert.InsertSubgroupVal(dataPrefix, languageCode, subgroupVal, subgroupValGid, isGlobal, DIServerType.MsAccess);            
        }

        /// <summary>
        /// Insert subgroupVal record into subgroupVal Table
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="subgroupVal">Subgroup val name</param>
        /// <param name="subgroupValGid">SubgroupVal GId </param>
        /// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
        /// <returns></returns>        
        public static string InsertSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.SubgroupValTableName + languageCode + "(" + DIColumns.SubgroupVals.SubgroupVal + ","
            + DIColumns.SubgroupVals.SubgroupValGId + ","
            + DIColumns.SubgroupVals.SubgroupValGlobal + ")"
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(subgroupVal) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupValGid) + "', ";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.Sqlite:
                    if (isGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal +=  " 0 ) ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isGlobal + " ) ";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    if (isGlobal)
                    {
                        RetVal += " 1 )";
                    }
                    else
                    {
                        RetVal += " 0 )";
                    }
                    break;
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
        /// Insert subgroupVal record into subgroupVal Table with 
        /// </summary>
        /// <param name="dataPrefix">Data prefix like UT_</param>
        /// <param name="languageCode">Language code like _en </param>
        /// <param name="subgroupVal">Subgroup val name</param>
        /// <param name="subgroupValGid">SubgroupVal GId </param>
        /// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
        /// <param name="serverType"></param>
        /// <param name="order"></param>
        /// <returns></returns>        
        public static string InsertSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, DIServerType serverType,int order)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.SubgroupValTableName + languageCode + "(" + DIColumns.SubgroupVals.SubgroupVal + ","
            + DIColumns.SubgroupVals.SubgroupValGId + ","
            + DIColumns.SubgroupVals.SubgroupValOrder + ","
            + DIColumns.SubgroupVals.SubgroupValGlobal + ")"
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(subgroupVal) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupValGid) + "', " + order + ", ";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.Sqlite:
                    if (isGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isGlobal + " ) ";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    if (isGlobal)
                    {
                        RetVal += " 1 )";
                    }
                    else
                    {
                        RetVal += " 0 )";
                    }
                    break;
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
        
        #endregion

        #endregion
    }
}
