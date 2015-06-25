using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Source
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

        /// <summary>
        /// Inserts source into Indicator_Classifications table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en </param>
        /// <param name="source">Source name</param>
        /// <param name="parentNid">ParentNid. To insert source parent use -1 .</param>
        public static string InsertSource(string dataPrefix, string languageCode, string source, int parentNid, string GID, string info, bool isGlobal)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + " (" + DIColumns.IndicatorClassifications.ICParent_NId + ","
            + DIColumns.IndicatorClassifications.ICName + ","
            + DIColumns.IndicatorClassifications.ICInfo + ","
            + DIColumns.IndicatorClassifications.ICType + ","
            + DIColumns.IndicatorClassifications.ICGId + ","
            + DIColumns.IndicatorClassifications.ICGlobal + " )"
            + " Values(" + parentNid + ",'" + DIQueries.RemoveQuotesForSqlQuery(source) + "','";
            if (isGlobal == false)
            {
                RetVal += info + "','SR','" + GID + "',0)";
            }
            else
            {
                RetVal += info + "','SR','" + GID + "',1)";
            }



            return RetVal;
        }

        /// <summary>
        /// Inserts source into Indicator_Classifications table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en </param>
        /// <param name="source">Source name</param>
        /// <param name="parentNid">ParentNid. To insert source parent use -1 .</param>
        /// <param name="isGlobal"></param>
        /// <param name="GID"></param> 
        /// <param name="info"></param> 
        /// <param name="isbn">ISBN</param>
        /// <param name="nature">Nature</param>
        public static string InsertSource(string dataPrefix, string languageCode, string source, int parentNid, string GID, string info, bool isGlobal, string ISBN, string nature)
        {
            string RetVal = string.Empty;

            source = DIQueries.RemoveQuotesForSqlQuery(source);
            ISBN = DIQueries.RemoveQuotesForSqlQuery(ISBN);
            nature = DIQueries.RemoveQuotesForSqlQuery(nature);

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + " (" + DIColumns.IndicatorClassifications.ICParent_NId + ","
            + DIColumns.IndicatorClassifications.ICName + ","
            + DIColumns.IndicatorClassifications.ICInfo + ","
            + DIColumns.IndicatorClassifications.ICType + ","
            + DIColumns.IndicatorClassifications.ICGId + ","
            + DIColumns.IndicatorClassifications.ICGlobal + ","
            + DIColumns.IndicatorClassifications.ISBN + ","
            + DIColumns.IndicatorClassifications.Nature + " )"

            + " Values(" + parentNid + ",'" + source + "','";
            if (isGlobal == false)
            {
                RetVal += info + "','SR','" + GID + "',0,'" + ISBN + "','" + nature + "')";
            }
            else
            {
                RetVal += info + "','SR','" + GID + "',1,'" + ISBN + "','" + nature + "')";
            }



            return RetVal;
        }

        #endregion


    }
}
