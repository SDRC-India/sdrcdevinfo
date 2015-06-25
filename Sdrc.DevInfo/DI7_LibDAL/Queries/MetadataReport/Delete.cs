using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataReport
{
    public class Delete
    {

        #region "-- public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns sql query to delete records from UT_Metadata_Report_langcode table for the given CategoryNId
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public static string DeleteMetadataRecordsByCategories(string tableName, string categoryNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Delete FROM " + tableName;

            if (!string.IsNullOrEmpty(categoryNIds))
            {
                RetVal += " WHERE " + DIColumns.MetadataReport.CategoryNid + " IN(" + categoryNIds + ")";
            }

            return RetVal;
        }

        /// <summary>
        /// Delete records from metadata report table for the given targetnid and category nid 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="targetNid"></param>
        /// <param name="categoryNId"></param>
        /// <returns></returns>
        public static string DeleteMetadataReport(string tableName, string targetNid, string categoryNId)
        {
            string RetVal = string.Empty;

            RetVal = "Delete from " + tableName + " where " + DIColumns.MetadataReport.TargetNid + "=" + targetNid + " AND " + DIColumns.MetadataReport.CategoryNid + "=" + categoryNId;

            return RetVal;
        }

        /// <summary>
        /// Deletes records from metadata report table for the given category nid
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNId"></param>
        /// <returns></returns>
        public static string DeleteMetadataReportByCategory(string tableName, string categoryNId)
        {
            string RetVal = string.Empty;

            RetVal = "Delete from " + tableName + " where " + DIColumns.MetadataReport.CategoryNid + "=" + categoryNId;
            return RetVal;
        }

        public static string DeleteMetadataReportByCategory(DITables tables, MetadataElementType categoryType)
        {
            string RetVal=string.Empty;

            RetVal = "DELETE MR.* FROM " + tables.MetadataCategory + "  MC INNER JOIN " + tables.MetadataReport + " MR ON MC." + DIColumns.Metadata_Category.CategoryNId + " = MR." + DIColumns.MetadataReport.CategoryNid + " where  MC." + DIColumns.Metadata_Category.CategoryType + "=" + DIQueries.MetadataElementTypeText[categoryType] +";";

            return RetVal;
        }
        #endregion

        #endregion

    }
}
