using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory
{
    public class Select
    {        
        #region "-- private --"
     
        #region "-- Variables --"

        private DITables TablesName;

        #endregion        
              
        #endregion

        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"

        internal Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }
        
        #endregion

        #region "-- Methods --"

        /// <summary>
        /// return records form Metadata_Category table 
        /// </summary>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public string GetMetadataCategories(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();

            //sbQuery.Append("SELECT " + DIColumns.Metadata_Category.CategoryNId + "," + DIColumns.Metadata_Category.CategoryName + "," + DIColumns.Metadata_Category.CategoryType + "," + DIColumns.Metadata_Category.CategoryOrder + " From " + this.TablesName.MetadataCategory);


            sbQuery.Append("SELECT * From " + this.TablesName.MetadataCategory);
           

            if (filterText.Length > 0)
            {
                sbQuery.Append(" WHERE 1=1 ");

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryName + " IN (" + DIQueries.RemoveQuotesForSqlQuery( filterText) + ")");
                        break;
                    case FilterFieldType.Type:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryType + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(filterText) + "'");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append( " AND " + filterText);
                        break;
                    default:
                        break;
                }
            }

            sbQuery.Append(" order by " + DIColumns.Metadata_Category.CategoryOrder);
            RetVal = sbQuery.ToString();
            return RetVal;        
        }

        /// <summary>
        /// return records form Metadata_Category table 
        /// </summary>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public string GetMetadataCategoriesForEditor(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * From " + this.TablesName.MetadataCategory);


            if (filterText.Length > 0)
            {
                sbQuery.Append(" WHERE  "+ DIColumns.Metadata_Category.IsPresentational +"=0   ");

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryName + " IN (" + DIQueries.RemoveQuotesForSqlQuery(filterText) + ")");
                        break;
                    case FilterFieldType.Type:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryType + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(filterText) + "'");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(" AND " + filterText);
                        break;
                    default:
                        break;
                }
            }

            sbQuery.Append(" order by " + DIColumns.Metadata_Category.CategoryOrder);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// return records form Metadata_Category table 
        /// </summary>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public string GetMetadataCategories(FilterFieldType filterFieldType, string filterText,string parentNid)
        {
            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * From " + this.TablesName.MetadataCategory);


            if (filterText.Length > 0)
            {
                sbQuery.Append(" WHERE "+ DIColumns.Metadata_Category.ParentCategoryNId+"="+ parentNid  +" ");

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Type:
                        sbQuery.Append(" AND " + DIColumns.Metadata_Category.CategoryType + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(" AND " + filterText);
                        break;
                    default:
                        break;
                }
            }

            sbQuery.Append(" order by " + DIColumns.Metadata_Category.CategoryOrder);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        ///  Returns sql query to get max category order for the given category type and within the given cateogry
        /// </summary>
        /// <param name="categoryType"></param>
        /// <returns></returns>
        public string GetMaxMetadataCategoryOrder(string categoryType)
        {
            string RetVal = string.Empty;

            RetVal = "Select max(" + DIColumns.Metadata_Category.CategoryOrder + ") FROM " + this.TablesName.MetadataCategory + " WHERE " + DIColumns.Metadata_Category.CategoryType + "='" + categoryType + "' ";

            return RetVal;
        }

        /// <summary>
        ///  Returns sql query to get max category order for the given category type and within the given cateogry
        /// </summary>
        /// <param name="categoryType"></param>
        /// <returns></returns>
        public string GetMaxMetadataCategoryOrder(string categoryType,string parentNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select max(" + DIColumns.Metadata_Category.CategoryOrder + ") FROM " + this.TablesName.MetadataCategory+ " WHERE " + DIColumns.Metadata_Category.CategoryType + "=" + categoryType +"  AND "+
                DIColumns.Metadata_Category.ParentCategoryNId+"="+ parentNid;

            return RetVal;
        }
        
        /// <summary>
        /// Returns query to get metadata category for the given category type where name & parent nid are equal to the given name & parent nid
        /// </summary>
        /// <param name="categoryType"></param>
        /// <param name="categoryName"></param>
        /// <param name="parentNid"></param>
        /// <returns></returns>
        public string GetMetadataCategory(string categoryType,string categoryName, string parentNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select * FROM " + this.TablesName.MetadataCategory + " WHERE " + DIColumns.Metadata_Category.CategoryType + "='" + categoryType + "'  AND " +
                DIColumns.Metadata_Category.ParentCategoryNId + "=" + parentNid + " AND " +
                DIColumns.Metadata_Category.CategoryName + "='" + categoryName + "'";
            return RetVal;
        }

        /// <summary>
        /// return records form Metadata_Category table by category name
        /// </summary>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public string GetMetadataCategoriesByCategoryname(string categoryName, string categoryType)
        {
            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Metadata_Category.CategoryNId + "," + DIColumns.Metadata_Category.CategoryName + "," + DIColumns.Metadata_Category.CategoryType + "," + DIColumns.Metadata_Category.CategoryOrder + " From " + this.TablesName.MetadataCategory);

            sbQuery.Append(" WHERE 1=1 ");

            if (categoryName.Length > 0)
            {
                sbQuery.Append(" and " + DIColumns.Metadata_Category.CategoryName + "='" + categoryName + "'");
            }
            if (categoryType.Length > 0)
            {
                sbQuery.Append(" and " + DIColumns.Metadata_Category.CategoryType + "='" + categoryType + "'");
            }

            sbQuery.Append(" order by " + DIColumns.Metadata_Category.CategoryOrder);
            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// return records form Metadata_Category table by category name
        /// </summary>
        /// <param name="categoryNIds"></param>
        /// <returns></returns>
        public string GetMetadataCategoriesByCategoryname(string categoryName, string categoryType,string parentNid)
        {
            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Metadata_Category.CategoryNId + "," + DIColumns.Metadata_Category.CategoryName + "," + DIColumns.Metadata_Category.CategoryType + "," + DIColumns.Metadata_Category.CategoryOrder + " From " + this.TablesName.MetadataCategory);

            sbQuery.Append(" WHERE "+DIColumns.Metadata_Category.ParentCategoryNId +"= " +parentNid +" ");

            if (categoryName.Length > 0)
            {
                sbQuery.Append(" and " + DIColumns.Metadata_Category.CategoryName + "='" + categoryName + "'");
            }
            if (categoryType.Length > 0)
            {
                // if category type already contains single quotes, then dont add quote
                if (categoryType.StartsWith("'") && categoryType.EndsWith("'"))
                {
                    sbQuery.Append(" and " + DIColumns.Metadata_Category.CategoryType + "=" + categoryType );
                }
                else
                {
                    sbQuery.Append(" and " + DIColumns.Metadata_Category.CategoryType + "='" + categoryType + "'");
                }
            }

            sbQuery.Append(" order by " + DIColumns.Metadata_Category.CategoryOrder);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns sql query to get category nid 
        /// </summary>
        /// <param name="GId"></param>
        /// <param name="categoryType"></param>
        /// <returns></returns>
        public string GetMetadataCategoryNIdByGID(string GId, string categoryType)
        {
            string RetVal = string.Empty;

            RetVal="SELECT "+ DIColumns.Metadata_Category.CategoryNId +" FROM "+ this.TablesName.MetadataCategory +" WHERE "+ DIColumns.Metadata_Category.CategoryGId+"='"+ GId+"' AND "+ DIColumns.Metadata_Category.CategoryType+"="+ categoryType;

            return RetVal;
        }
        public string GetInValidSdmxCompliantMetadataCategoryGid()
        {
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.Metadata_Category.CategoryGId + "," + DIColumns.Metadata_Category.CategoryName  + "," + DIColumns.Metadata_Category.CategoryNId);
            SqlQuery.Append(" FROM " + TablesName.MetadataCategory);
            

            return SqlQuery.ToString();
        }
        #endregion


        #endregion
    }

}
