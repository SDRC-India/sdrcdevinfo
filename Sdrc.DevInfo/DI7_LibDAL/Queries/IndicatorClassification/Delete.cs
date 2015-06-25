using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// DROP indicator_classifications_ius table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string DeleteOldICIUSTable(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = "DROP TABLE " + dataPrefix + DITables.Old_ICIUSTableName;

            return RetVal;
        }

        /// <summary>
        /// DROP indicator_classifications_ius table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string DeleteNewICIUSTable(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = "DROP TABLE " + dataPrefix + DITables.ICIUSTableName;

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Indicator_Classification_IUS table
        /// </summary>
        /// <param name="tableName"> Indicator_Classification_IUS table name</param>
        /// <param name="IUSNIds"></param>
        /// <param name="ICNIds"></param>
        /// <returns></returns>
        public static string DeleteICIUS(string tableName, string IUSNIds, string ICNIds)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + tableName + " Where 1=1 ";
            if (!string.IsNullOrEmpty(IUSNIds))
            {
                RetVal += " AND " + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN (" + IUSNIds + ") ";
            }

            if (!string.IsNullOrEmpty(ICNIds))
            {
                RetVal += " AND " + DIColumns.IndicatorClassificationsIUS.ICNId + " IN (" + ICNIds + ") ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Indicator_Classification_IUS table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="IUSNIds"></param>
        /// <returns></returns>
        public static string DeleteICIUS(string tableName, string IUSNIds)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + tableName + " Where " + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN (" + IUSNIds + ")";

            return RetVal;
        }

        /// <summary>
        /// Deelte ic IUS relation on the indicatorNId
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="iusTablename"></param>
        /// <param name="indicatorNId"></param>
        /// <returns></returns>
        public static string DeleteICIUSByIndicatorNId(string tableName, string iusTablename, string indicatorNId)
        {

            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + tableName + " Where " + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN (";
            RetVal += " SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " FROM " + iusTablename + " IUS ";
            RetVal += " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNId + "))";

            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Indicator_Classification_IUS table By ICIUSNIds
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ICIUSNIds"></param>
        /// <returns></returns>
        public static string DeleteICIUSByICIUSNId(string tableName, string ICIUSNIds)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + tableName + " Where " + DIColumns.IndicatorClassificationsIUS.ICIUSNId + " IN (" + ICIUSNIds + ")";

            return RetVal;
        }


        /// <summary>
        /// Returns a query to delete records from Indicator_classification table
        /// </summary>
        /// <param name="tableName">IndicatorClassification table name</param>
        /// <param name="nids"></param>
        /// <returns></returns>
        public static string DeleteIC(string tableName, string nids)
        {
            string RetVal = string.Empty;
            RetVal = " Delete FROM " + tableName + "  Where " + DIColumns.IndicatorClassifications.ICNId + " IN (" + nids + ")";
            return RetVal;
        }

        /// <summary>
        /// Returns a query to delete records from Indicator_classification table
        /// </summary>
        /// <param name="tableName">IndicatorClassification table name</param>
        /// <param name="nids"></param>
        /// <returns></returns>
        public static string DeleteSources(string tableName, string nids)
        {
            string RetVal = string.Empty;
            RetVal = " Delete FROM " + tableName + "  Where " + DIColumns.IndicatorClassifications.ICType + "= 'SR' ";

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " AND " + DIColumns.IndicatorClassifications.ICNId + " IN (" + nids + ")";
            }

            return RetVal;
        }

        #endregion

        #endregion
    }
}
