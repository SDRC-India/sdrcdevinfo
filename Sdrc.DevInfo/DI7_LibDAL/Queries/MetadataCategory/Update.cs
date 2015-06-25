using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory
{
    public class Update
    {

        #region "-- private --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Update MetadataCategory Record
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNId"></param>
        /// <param name="categoryName"></param>
        /// <param name="categoryType"></param>
        /// <param name="categoryOrder"></param>
        /// <returns></returns>
        public static string UpdateMetadataCategory(string tableName, int categoryNId, string categoryName, string categoryType, string categoryGID, string categoryDescription, string parentNId, bool isMandatory, bool isPresentational)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.Metadata_Category.CategoryName + "='"
           + DIQueries.RemoveQuotesForSqlQuery(categoryName) + "'," + DIColumns.Metadata_Category.CategoryType + "='"
           + categoryType + "'," +
           DIColumns.Metadata_Category.ParentCategoryNId + "=" + parentNId + "," +
           DIColumns.Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(categoryGID) + "'," +
           DIColumns.Metadata_Category.CategoryDescription + "='" + DIQueries.RemoveQuotesForSqlQuery(categoryDescription) + "', " + DIColumns.Metadata_Category.IsMandatory + "=" + Convert.ToInt32(isMandatory).ToString() + "," +
           DIColumns.Metadata_Category.IsPresentational + "=" + Convert.ToInt32(isPresentational).ToString()
           + " WHERE " + DIColumns.Metadata_Category.CategoryNId + " = " + categoryNId;

            return RetVal;
        }

        public static string UpdateMetadataCategory(string tableName, int categoryNId, string categoryGID, string parentNId, bool isMandatory, bool isPresentational)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " +
           DIColumns.Metadata_Category.ParentCategoryNId + "=" + parentNId + "," +
           DIColumns.Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(categoryGID) + "'," +
 DIColumns.Metadata_Category.IsMandatory + "=" + Convert.ToInt32(isMandatory).ToString() + "," +
           DIColumns.Metadata_Category.IsPresentational + "=" + Convert.ToInt32(isPresentational).ToString()
           + " WHERE " + DIColumns.Metadata_Category.CategoryNId + " = " + categoryNId;

            return RetVal;
        }

        /// <summary>
        /// Update MetadataCategory Record
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNId"></param>
        /// <param name="categoryName"></param>
        /// <param name="categoryType"></param>
        /// <param name="categoryOrder"></param>
        /// <returns></returns>
        public static string UpdateMetadataCategory(string tableName, int categoryNId, string categoryName, string categoryType, int categoryOrder)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.Metadata_Category.CategoryName + "='"
           + DIQueries.RemoveQuotesForSqlQuery(categoryName) + "'," + DIColumns.Metadata_Category.CategoryType + "='"
           + categoryType + "'," + DIColumns.Metadata_Category.CategoryOrder + "=" + categoryOrder
           + " WHERE " + DIColumns.Metadata_Category.CategoryNId + " = " + categoryNId;

            return RetVal;
        }

        /// <summary>
        /// Updates category  gid
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNId"></param>
        /// <param name="categoryGId"></param>
        /// <returns></returns>
        public static string UpdateMetadataCategoryGID(string tableName, string categoryNId, string categoryGId)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(categoryGId) + "' WHERE " + DIColumns.Metadata_Category.CategoryNId + " = " + categoryNId;

            return RetVal;
        }

        /// <summary>
        /// Updates category parent nid
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="categoryNId"></param>
        /// <param name="parentNid"></param>
        /// <returns></returns>
        public static string UpdateMetadataCategoryParentNid(string tableName, string categoryNId, string parentNid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " + DIColumns.Metadata_Category.ParentCategoryNId + "=" + parentNid + " WHERE " + DIColumns.Metadata_Category.CategoryNId + " = " + categoryNId;

            return RetVal;
        }

        /// <summary>
        /// Update metadata category order
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="order"></param>
        /// <param name="mdCategoryNId"></param>
        /// <returns></returns>
        public static string UpdateMetadataCategoryOrder(string tableName, int order, int mdCategoryNId)
        {
            string RetVal = string.Empty;

            RetVal = RetVal = "UPDATE " + tableName + " SET ";

            RetVal += DIColumns.Metadata_Category.CategoryOrder + "=" + order
            + " WHERE " + DIColumns.Metadata_Category.CategoryNId + "=" + mdCategoryNId;

            return RetVal;
        }

        #endregion

        #endregion

    }
}
