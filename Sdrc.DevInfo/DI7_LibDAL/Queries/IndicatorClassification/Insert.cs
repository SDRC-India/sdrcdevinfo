using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "indicator_classifications";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// careate ut_ic_ius table from ut_indicator_classification_IUC table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string CreateNewICIUSTableFromExisting(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "SELECT * INTO " + dataPrefix + DITables.ICIUSTableName
                + " FROM " + dataPrefix + DITables.Old_ICIUSTableName;

            return RetVal;
        }

        /// <summary>
        /// create ut_indicator_classification_IUS table from ut_ic_ius table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string CreateOldICIUSTableFromExisting(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "SELECT * INTO " + dataPrefix + DITables.Old_ICIUSTableName
                + " FROM " + dataPrefix + DITables.ICIUSTableName;

            return RetVal;
        }

        /// <summary>
        /// Insert record for IC and IUS relationship into database.
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="ICNid">IC Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="Recomendedsource">Recomended Source</param>
        /// <returns>Insert Sqlquery</returns>
        public static string InsertICAndIUSRelation(string dataPrefix, int ICNid, int IUSNid, bool Recomendedsource)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "INSERT INTO " + dataPrefix + DITables.ICIUSTableName + "(" + DIColumns.IndicatorClassificationsIUS.ICNId + ","
            + DIColumns.IndicatorClassificationsIUS.IUSNId + ","
            + DIColumns.IndicatorClassificationsIUS.RecommendedSource + ") "
            + " VALUES(" + ICNid + "," + IUSNid + "," + (Recomendedsource == true ? 1 : 0) + " ) ";

            return RetVal;
        }

        /// <summary>
        /// Insert record for IC and IUS relationship into database.
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="ICNid">IC Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="Recomendedsource">Recomended Source</param>
        /// <param name="icOrder">IC IUS Sort Order</param>
        /// <returns>Insert Sqlquery</returns>
        public static string InsertICAndIUSRelation(string dataPrefix, int ICNid, int IUSNid, bool Recomendedsource, int iusOrder)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "INSERT INTO " + dataPrefix + DITables.ICIUSTableName + "(" + DIColumns.IndicatorClassificationsIUS.ICNId + ","
            + DIColumns.IndicatorClassificationsIUS.IUSNId + ","
            + DIColumns.IndicatorClassificationsIUS.RecommendedSource + ","
            + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + ") "
            + " VALUES(" + ICNid + "," + IUSNid + "," + (Recomendedsource == true ? 1 : 0) + "," + iusOrder + " ) ";

            return RetVal;
        }


        /// <summary>
        /// Returns qurey to insert SubgroupValOrder column into SubgroupVal table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AddOrderColumn(string dataPrefix, string languageCode, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, languageCode).IndicatorClassifications + " ADD COLUMN  [" + DIColumns.IndicatorClassifications.ICOrder + "] ";
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
        /// Insert indicator classification into database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name"></param>
        /// <param name="GId"></param>
        /// <param name="isGlobal"></param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo"></param>
        /// <param name="classificationType"></param>
        /// <returns></returns>
        public static string InsertIC(string dataPrefix, string languageCode, string name, string GId, bool isGlobal, int parentNid, string ICInfo, ICType classificationType)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.IndicatorClassifications.ICParent_NId + ","
            + DIColumns.IndicatorClassifications.ICGId + ","
            + DIColumns.IndicatorClassifications.ICName + ","
            + DIColumns.IndicatorClassifications.ICGlobal + ","
            + DIColumns.IndicatorClassifications.ICInfo + ","
            + DIColumns.IndicatorClassifications.ICType + ") "
            + "  Values(" + parentNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(GId) + "','" + DIQueries.RemoveQuotesForSqlQuery(name) + "'," + (isGlobal == true ? 1 : 0) + ", '"
            + ICInfo + "'," + DIQueries.ICTypeText[classificationType] + ")";

            return RetVal;
        }



        /// <summary>
        /// Insert indicator classification into database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name"></param>
        /// <param name="GId"></param>
        /// <param name="isGlobal"></param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo"></param>
        /// <param name="classificationType"></param>
        /// <param name="ICOrder"></param>
        /// <returns></returns>
        public static string InsertIC(string dataPrefix, string languageCode, string name, string GId, bool isGlobal, int parentNid, string ICInfo, ICType classificationType, int ICOrder)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + "(" + DIColumns.IndicatorClassifications.ICParent_NId + ","
            + DIColumns.IndicatorClassifications.ICGId + ","
            + DIColumns.IndicatorClassifications.ICName + ","
            + DIColumns.IndicatorClassifications.ICGlobal + ","
            + DIColumns.IndicatorClassifications.ICInfo + ","
            + DIColumns.IndicatorClassifications.ICType + ","
            + DIColumns.IndicatorClassifications.ICOrder + ") "
            + "  Values(" + parentNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(GId) + "','" + DIQueries.RemoveQuotesForSqlQuery(name) + "'," + (isGlobal == true ? 1 : 0) + ", '"
            + ICInfo + "'," + DIQueries.ICTypeText[classificationType] + "," + ICOrder.ToString() + ")";

            return RetVal;
        }



        /// <summary>
        /// Returns qurey to insert IC_IUS_Order column into UT_Indicator_Classifications_IUS table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIC_IUS_OrderColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorClassificationsIUS + " ADD COLUMN  " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + " ";
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
        /// Returns qurey to insert IC_IUS_Label column into UT_Indicator_Classifications_IUS table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIC_IUS_LabelColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorClassificationsIUS + " ADD COLUMN  " + DIColumns.IndicatorClassificationsIUS.ICIUSLabel + " ";
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
        /// Returns qurey to insert IC_Short_Name column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertICShortNameColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.ICShortName + " ";

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
        /// Returns qurey to insert Publisher column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertPublisherColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.Publisher + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(100) ";
                }
                else
                {
                    RetVal += " varchar(100) ";
                }
            }
            else
            {
                RetVal += " Text(100) ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Title column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertTitleColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.Title + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " LongText ";
                }
                else
                {
                    RetVal += " nvarchar(4000) ";
                }
            }
            else
            {
                RetVal += " Memo ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert DIYear column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDIYearColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.DIYear + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(10) ";
                }
                else
                {
                    RetVal += " varchar(10) ";
                }
            }
            else
            {
                RetVal += " Text(10) ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert SourceLink1 column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertSourceLink1Column(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.SourceLink1 + " ";

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
        /// Returns qurey to insert SourceLink2 column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertSourceLink2Column(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.SourceLink2 + " ";

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
        /// Returns qurey to insert ISBN column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7ISBNColumns(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.ISBN + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " nvarchar(255) ";
                }
            }
            else
            {
                RetVal += " Text(255) ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Nature column into UT_Indicator_Classifications_en table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7NatureColumns(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorClassifications + " ADD COLUMN  " + DIColumns.IndicatorClassifications.Nature + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " nvarchar(255) ";
                }
            }
            else
            {
                RetVal += " Text(255) ";
            }

            return RetVal;
        }


        #endregion

        #endregion


    }
}
