using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        //   private const string IndicatorClassificationsIUSTableName = "indicator_classifications_ius";
        private static string IndicatorClassificationsIUSTableName =  DITables.ICIUSTableName;
        private const string TableName = "indicator_classifications";
        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"



        /// <summary>
        /// Update indicator classification information 
        /// </summary>
        /// <param name="whereClause">Select criteria Like IC_Nid in(1,2,3) or IC_Parent_NId =3,...etc </param>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name">Name which may be empty</param>
        /// <param name="GId">GID which may be empty</param>
        /// <param name="isGlobal">May be null</param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo">ICInfo which may be empty</param>
        /// <param name="classificationType">May be null</param>
        /// <param name="ICNids"></param>
        /// <returns></returns>
        public static string UpdateIC(string whereClause, string dataPrefix, string languageCode, string name, string GId, Nullable<bool> isGlobal, int parentNid, string ICInfo, Nullable<ICType> classificationType, string ICNids)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET " + DIColumns.IndicatorClassifications.ICParent_NId + "=" + parentNid;

            if (!string.IsNullOrEmpty(GId))
            {
                RetVal += "," + DIColumns.IndicatorClassifications.ICGId + "='" + DIQueries.RemoveQuotesForSqlQuery(GId) + "'";
            }
            if (!string.IsNullOrEmpty(name))
            {
                RetVal += "," + DIColumns.IndicatorClassifications.ICName + "='" + DIQueries.RemoveQuotesForSqlQuery(name) + "'";
            }

            if (isGlobal != null)
            {
                if (isGlobal == true)
                {
                    RetVal += "," + DIColumns.IndicatorClassifications.ICGlobal + "=1";
                }
                else
                {
                    RetVal += "," + DIColumns.IndicatorClassifications.ICGlobal + "=0";
                }
            }

            if (!string.IsNullOrEmpty(ICInfo))
            {
                RetVal += "," + DIColumns.IndicatorClassifications.ICInfo + "='" + ICInfo + "'";
            }

            if (classificationType != null)
            {
                RetVal += "," + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[(DevInfo.Lib.DI_LibDAL.Queries.ICType)classificationType] + " ";
            }

            RetVal += " WHERE " + whereClause;

            return RetVal;
        }

        /// <summary>
        /// Update indicator classification information for the given ICNId
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name">Name which may be empty</param>
        /// <param name="GId">GID which may be empty</param>
        /// <param name="isGlobal">May be null</param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo">ICInfo which may be empty</param>
        /// <param name="classificationType"></param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateIC(string dataPrefix, string languageCode, string name, string GId, Nullable<bool> isGlobal, int parentNid, string ICInfo, Nullable<ICType> classificationType, int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = Update.UpdateIC(DIColumns.IndicatorClassifications.ICNId + "=" + ICNid, dataPrefix, languageCode, name, GId, isGlobal, parentNid, ICInfo, classificationType, ICNid.ToString());

            return RetVal;
        }

        /// <summary>
        /// Update indicator classification information
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name">Name which may be empty</param>
        /// <param name="GId">GID which may be empty</param>
        /// <param name="isGlobal"></param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo">ICInfo which may be empty</param>
        /// <param name="classificationType"></param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateICInfo(string dataPrefix, string languageCode, string ICInfo, ICType classificationType, int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET ";

            RetVal += DIColumns.IndicatorClassifications.ICInfo + "='" + ICInfo + "'";

            RetVal += "," + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]

            + " WHERE " + DIColumns.IndicatorClassifications.ICNId + "=" + ICNid;

            return RetVal;
        }


        /// <summary>
        /// Update Indicator Classification ISBN and Nature value
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="ISBN"></param>
        /// <param name="nature"></param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateICInfo(string dataPrefix, string languageCode, string ISBN, string nature, int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET ";

            RetVal += DIColumns.IndicatorClassifications.ISBN + "='" + DIQueries.RemoveQuotesForSqlQuery(ISBN) + "'";

            RetVal += "," + DIColumns.IndicatorClassifications.Nature + "='" + DIQueries.RemoveQuotesForSqlQuery(nature) + "'"

            + " WHERE " + DIColumns.IndicatorClassifications.ICNId + "=" + ICNid;

            return RetVal;
        }

        /// <summary>
        /// Update indicator classification information
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="name">Name which may be empty</param>
        /// <param name="GId">GID which may be empty</param>
        /// <param name="isGlobal"></param>
        /// <param name="parentNid"></param>
        /// <param name="ICInfo">ICInfo which may be empty</param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateICInfo(string dataPrefix, string languageCode, string ICInfo, int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET ";

            RetVal += DIColumns.IndicatorClassifications.ICInfo + "='" + DIQueries.RemoveQuotesForSqlQuery(ICInfo) + "'";

            RetVal += " WHERE " + DIColumns.IndicatorClassifications.ICNId + "=" + ICNid;

            return RetVal;
        }


        /// <summary>
        /// Update IC Order
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="order"></param>
        /// <param name="classificationType"></param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateICOrder(string dataPrefix, string languageCode, int order, int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.TableName + languageCode + " SET ";

            RetVal += DIColumns.IndicatorClassifications.ICOrder + "=" + order
            + " WHERE " + DIColumns.IndicatorClassifications.ICNId + "=" + ICNid;

            return RetVal;
        }

        /// <summary>
        /// Returns a query to set IC_IUS_Order =1 for recommended sources. 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string UdpateICIUSOrderValues(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.IndicatorClassificationsIUSTableName +
            "  Set   " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + "=1  where  " + DIColumns.IndicatorClassificationsIUS.RecommendedSource + "=true";

            return RetVal;
        }
        /// <summary>
        /// Returns a query to set IC_IUS_Order =1 for recommended sources. 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string UdpateICIUSOrder(string dataPrefix, int order, int icIUSNId)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.IndicatorClassificationsIUSTableName +
                "  Set   " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + "=" + order + " WHERE  " + DIColumns.IndicatorClassificationsIUS.ICIUSNId + "=" + icIUSNId;

            return RetVal;
        }

        /// <summary>
        /// Returns a query to set IC_IUS_Order =1 for recommended sources. 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string UdpateICIUSOrder(string dataPrefix, int order, string icIUSNId)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + dataPrefix + Update.IndicatorClassificationsIUSTableName +
                "  Set   " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + "=" + order + " WHERE  " + DIColumns.IndicatorClassificationsIUS.ICIUSNId + " IN (" + icIUSNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update missing language values into target table
        /// </summary>
        /// <param name="sourceTableName">Source table name like UT_Indicator_Classification_en</param>
        /// <param name="targetTableName">Target table name like UT_Indicator_Classification_fr</param>
        /// <returns></returns>
        public static string UpdateMissingLanguageValues(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;
            string TextPrefix = DIQueries.TextPrefix;

            RetVal = "INSERT INTO " + targetTableName + "( " + DIColumns.IndicatorClassifications.ICNId + " , " + DIColumns.IndicatorClassifications.ICParent_NId + " , " + DIColumns.IndicatorClassifications.ICGId + " ,  " + DIColumns.IndicatorClassifications.ICName + " ,  " + DIColumns.IndicatorClassifications.ICGlobal + " , " + DIColumns.IndicatorClassifications.ICInfo + " , " + DIColumns.IndicatorClassifications.ICType + " )"
                + " SELECT " + DIColumns.IndicatorClassifications.ICNId + ", " + DIColumns.IndicatorClassifications.ICParent_NId + " , " + DIColumns.IndicatorClassifications.ICGId + " ,'" + TextPrefix + "' &  " + DIColumns.IndicatorClassifications.ICName + " , " + DIColumns.IndicatorClassifications.ICGlobal + " , " + DIColumns.IndicatorClassifications.ICInfo + " , " + DIColumns.IndicatorClassifications.ICType + "  " + " FROM " + sourceTableName + " " +
                " WHERE " + DIColumns.IndicatorClassifications.ICNId + " not in (SELECT DISTINCT  " + DIColumns.IndicatorClassifications.ICNId + "  FROM " + targetTableName + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns a query to update xml text in CF_FlowChart table
        /// </summary>
        /// <param name="tableName">CF_FlowChart table name</param>
        /// <param name="xmltext"></param>
        /// <returns></returns>
        public static string UpateCFFlowChart(string tableName, string xmltext)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.CFFlowChart.CF_FlowChart + "='" + xmltext + "'";
            return RetVal;
        }

        #region "-- Publisher, Title & DIYear columns--"

        /// <summary>
        /// Returns sql query to update the publisher column
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string UpdatePublisherColumn(DIServerType serverType, string tableName)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;

                case DIServerType.MsAccess:
                    RetVal = " UPDATE " + tableName + " AS IC INNER JOIN " + tableName + " AS IC1 ON IC1." + DIColumns.IndicatorClassifications.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICParent_NId + " SET Ic." + DIColumns.IndicatorClassifications.Publisher + "=  ic1." + DIColumns.IndicatorClassifications.ICName + " WHERE (((IC." + DIColumns.IndicatorClassifications.ICType + ")=" + DIQueries.ICTypeText[ICType.Source] + " )) ";

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
        /// Returns sql query to update the year column 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public static string UpdateYearColumn(string tableName, int ICNid, string diYear)
        {
            string RetVal = string.Empty;

            RetVal = " Update  " + tableName + " AS IC set IC." + DIColumns.IndicatorClassifications.DIYear + "='" + diYear + "' WHERE IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source] + " and IC." + DIColumns.IndicatorClassifications.ICNId + "=" + ICNid.ToString();

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to update the year column
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string UpdateYearColumn(DIServerType serverType, string tableName)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;

                case DIServerType.MsAccess:
                    RetVal = " Update  " + tableName + " AS IC set IC." + DIColumns.IndicatorClassifications.DIYear + "= iif(  instr(len(ic." + DIColumns.IndicatorClassifications.Publisher + ") +2, Ic." + DIColumns.IndicatorClassifications.ICName + ",'_') >0,Mid(IC." + DIColumns.IndicatorClassifications.ICName + ",   instr(len(ic." + DIColumns.IndicatorClassifications.Publisher + ") +2, Ic." + DIColumns.IndicatorClassifications.ICName + ",'_') + 1 ),'') WHERE IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source] + " and IC." + DIColumns.IndicatorClassifications.ICParent_NId + "<>-1  ";

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
        /// Returns sql query to update the title column
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string UpdateTitleColumn(DIServerType serverType, string tableName)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                    break;

                case DIServerType.MsAccess:
                    RetVal = " Update  " + tableName + " AS IC Set IC." + DIColumns.IndicatorClassifications.Title + "=  Mid (IC." + DIColumns.IndicatorClassifications.ICName + ", len(ic." + DIColumns.IndicatorClassifications.Publisher + ") +2,  len(IC." + DIColumns.IndicatorClassifications.ICName + ") - ( len(ic." + DIColumns.IndicatorClassifications.Publisher + ") +   iif( len(ic." + DIColumns.IndicatorClassifications.DIYear + ") =0, 0, Len(ic." + DIColumns.IndicatorClassifications.DIYear + ")+2)  ) ) WHERE IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source] + " and IC." + DIColumns.IndicatorClassifications.ICParent_NId + "<>-1  and IC." + DIColumns.IndicatorClassifications.ICName + " like IC." + DIColumns.IndicatorClassifications.ICName + " &'_%'";
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
        /// Returns sql query to update publisher, title  & year columns into target language table
        /// </summary>
        /// <param name="sourceTableName"></param>
        /// <param name="targetTableName"></param>
        /// <returns></returns>
        public static string UpdatePublisherTitleYearInOtherLanguages(string sourceTableName, string targetTableName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + sourceTableName + " AS IC INNER JOIN " + targetTableName + " AS IC1 ON IC." + DIColumns.IndicatorClassifications.ICNId + " = IC1." + DIColumns.IndicatorClassifications.ICNId + " SET IC1." + DIColumns.IndicatorClassifications.Publisher + "= Ic." + DIColumns.IndicatorClassifications.Publisher + " , IC1." + DIColumns.IndicatorClassifications.Title + "=IC." + DIColumns.IndicatorClassifications.Title + ", IC1." + DIColumns.IndicatorClassifications.DIYear + "=IC." + DIColumns.IndicatorClassifications.DIYear + " WHERE (((IC." + DIColumns.IndicatorClassifications.ICType + ")=" + DIQueries.ICTypeText[ICType.Source] + ") AND ((IC." + DIColumns.IndicatorClassifications.ICParent_NId + ")<>-1))";

            return RetVal;
        }

        #endregion

        #endregion

        #endregion


    }
}
