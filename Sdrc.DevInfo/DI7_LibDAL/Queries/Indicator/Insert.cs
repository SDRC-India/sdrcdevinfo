using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;


namespace DevInfo.Lib.DI_LibDAL.Queries.Indicator
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "indicator";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Insert Indicator into Indicator Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="IndicatorName">Indicator Name</param>
        /// <param name="indicatorGID">Indicator Gid</param>
        /// <param name="Indicatorinfo">Indicator Info</param>
        /// <param name="isIndicatorGlobal"> Indicator is Global</param>
        /// <returns></returns>
        public static string InsertIndicator(string dataPrefix, string languageCode, string IndicatorName, string indicatorGID, string indicatorInfo, bool isIndicatorGlobal)
        {

            return InsertIndicator(dataPrefix, languageCode, IndicatorName, indicatorGID, indicatorInfo, isIndicatorGlobal, DIServerType.MsAccess);
        }

        /// <summary>
        /// Insert Indicator into Indicator Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="IndicatorName">Indicator Name</param>
        /// <param name="indicatorGID">Indicator Gid</param>
        /// <param name="Indicatorinfo">Indicator Info</param>
        /// <param name="isIndicatorGlobal"> Indicator is Global</param>
        /// <returns></returns>
        public static string InsertIndicator(string dataPrefix, string languageCode, string IndicatorName, string indicatorGID, string indicatorInfo, bool isIndicatorGlobal, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.Indicator.IndicatorName + ","
            + DIColumns.Indicator.IndicatorGId + "," + DIColumns.Indicator.IndicatorInfo + ","
            + DIColumns.Indicator.IndicatorGlobal + ") "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(IndicatorName) + "','" + DIQueries.RemoveQuotes(indicatorGID) + "','"
            + indicatorInfo + "',";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    if (isIndicatorGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isIndicatorGlobal + " ) ";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    if (isIndicatorGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                case DIServerType.Sqlite:
                    if (isIndicatorGlobal)
                    {
                        RetVal += " 1 ) ";
                    }
                    else
                    {
                        RetVal += " 0 ) ";
                    }
                    break;
                default:
                    RetVal += isIndicatorGlobal + " ) ";
                    break;
            }


            return RetVal;
        }

        /// <summary>
        /// Insert Indicator into Indicator Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="IndicatorName">Indicator Name</param>
        /// <param name="indicatorGID">Indicator Gid</param>
        /// <param name="Indicatorinfo">Indicator Info</param>
        /// <param name="isIndicatorGlobal"> Indicator is Global</param>
        /// <returns></returns>
        public static string InsertIndicator(string dataPrefix, string languageCode, string IndicatorName, string indicatorGID, string indicatorInfo, bool isIndicatorGlobal, bool highIsGood, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.Indicator.IndicatorName + ","
            + DIColumns.Indicator.IndicatorGId + "," + DIColumns.Indicator.IndicatorInfo + ","
            + DIColumns.Indicator.HighIsGood + "," + DIColumns.Indicator.IndicatorGlobal + ") "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(IndicatorName) + "','" + DIQueries.RemoveQuotes(indicatorGID) + "','"
            + indicatorInfo + "'," + DIConnection.GetBoolValue(highIsGood) + "," + DIConnection.GetBoolValue(isIndicatorGlobal) + ")";
            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert Short_Name column into UT_Indicator_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertShortNameColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Indicator + " ADD COLUMN  " + DIColumns.Indicator.ShortName + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(50) ";
                }
                else
                {
                    RetVal += " varchar(50) ";
                }
            }
            else
            {
                RetVal += " Text(50) ";
            }

            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert Keywords column into UT_Indicator_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertKeywordsColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Indicator + " ADD COLUMN  " + DIColumns.Indicator.Keywords + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " varchar(255) ";
                }
            }
            else
            {
                RetVal += " Text(255) ";
            }

            return RetVal;
        }
        /// <summary>
        /// Returns qurey to insert indicator_Order column into UT_Indicator_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIndicatorOrderColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Indicator + " ADD COLUMN  " + DIColumns.Indicator.IndicatorOrder + " ";

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
                RetVal += " number ";
            }

            return RetVal;
        }
        /// <summary>
        /// Returns qurey to insert Data_Exist column into UT_Indicator_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDataExistColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Indicator + " ADD COLUMN  " + DIColumns.Indicator.DataExist + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }
        /// <summary>
        /// Returns qurey to insert HighIsGood column into UT_Indicator_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertHighIsGoodColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.Indicator + " ADD COLUMN  " + DIColumns.Indicator.HighIsGood + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }

        #endregion

        #endregion
    }

}
