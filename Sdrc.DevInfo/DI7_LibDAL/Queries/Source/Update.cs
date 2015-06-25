using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibDAL.Queries.Source
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public class Update
    {
        #region "-- Private --"

        private const string ICTableName = "Indicator_Classifications";
        private const string IndicatorClassificationIUSTable = "Indicator_Classifications_IUS";
        #endregion

        #region "-- Public --"
        /// <summary>
        /// Update Source
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="sourceName"></param>
        /// <param name="sourceNid"></param>
        /// <returns></returns>
        public static string UpdateSource(string dataPrefix, string languageCode, string sourceName, int sourceNid)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.ICTableName + languageCode
                    + " Set " + DIColumns.IndicatorClassifications.ICName + " = '" + DIQueries.RemoveQuotesForSqlQuery(sourceName)
                    + "' WHERE " + DIColumns.IndicatorClassifications.ICNId + "= " + sourceNid;

            return RetVal;
        }

        /// <summary>
        /// update Source
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="sourceName"></param>
        /// <param name="sourceNid"></param>
        /// <param name="ISBN"></param>
        /// <param name="nature"></param>
        /// <returns></returns>
        public static string UpdateSource(string dataPrefix, string languageCode, string sourceName, int sourceNid, string ISBN, string nature)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataPrefix + Update.ICTableName + languageCode
                    + " Set " + DIColumns.IndicatorClassifications.ICName + " = '" + DIQueries.RemoveQuotesForSqlQuery(sourceName)
                    + "'," +  DIColumns.IndicatorClassifications.ISBN + " = '" + DIQueries.RemoveQuotesForSqlQuery(ISBN)
                    + "'," + DIColumns.IndicatorClassifications.Nature + " = '" + DIQueries.RemoveQuotesForSqlQuery(nature)
                    + "' WHERE " + DIColumns.IndicatorClassifications.ICNId + "= " + sourceNid;

            return RetVal;
        }



        /// <summary>
        /// Returns update query to update label and Order in IndicatorClassifications_IUS table.
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="LabelVal"></param>
        /// <param name="OrderVal"></param>
        /// <param name="ICIUSNid"></param>
        /// <returns></returns>
        public static string UpdateRecommendedSources(string dataPrefix, string LabelVal, string OrderVal, string ICIUSNid)
        {
            string RetVal = string.Empty;
            int RecSource = 0;

            // Make Recommended Source True if Label or Order is Set
            if (!String.IsNullOrEmpty(LabelVal) || !String.IsNullOrEmpty(OrderVal))
            {
                RecSource = 1;
            }

            //--  update Label and Order into IndicatorClassification_IUS
            RetVal = "UPDATE " + dataPrefix + IndicatorClassificationIUSTable + " SET " + DIColumns.IndicatorClassificationsIUS.ICIUSLabel + "= '" + LabelVal + "', " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + " = " + OrderVal + ", " + DIColumns.IndicatorClassificationsIUS.RecommendedSource + " = " + RecSource + " WHERE " + DIColumns.IndicatorClassificationsIUS.ICIUSNId + "= " + ICIUSNid;

            return RetVal;

        }

        #endregion

    }
}
