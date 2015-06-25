using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Unit
{

    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "unit";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"
        
        /// <summary>
        /// Insert Unit into Unit Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="unitName"></param>
        /// <param name="unitGID"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public static string InsertUnit(string dataPrefix, string languageCode, string unitName, string unitGID, bool isGlobal)
        {
            string RetVal = string.Empty;

            RetVal = InsertUnit(dataPrefix, languageCode, unitName, unitGID, isGlobal, DIServerType.MsAccess);

            return RetVal;
        }

        /// <summary>
        /// Insert Unit into Unit Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="unitName"></param>
        /// <param name="unitGID"></param>
        /// <param name="isGlobal"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertUnit(string dataPrefix, string languageCode, string unitName, string unitGID, bool isGlobal, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.Unit.UnitName + ","
            + DIColumns.Unit.UnitGId + "," + DIColumns.Unit.UnitGlobal + ") "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(unitName) + "','" + DIQueries.RemoveQuotesForSqlQuery(unitGID) + "',";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.MySql:
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
