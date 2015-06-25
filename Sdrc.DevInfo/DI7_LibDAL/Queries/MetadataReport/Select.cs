using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataReport
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
        /// Returns query  to get metadata reports for the given target nid
        /// </summary>
        /// <param name="targetNid"></param>
        /// <returns></returns>
        public string GetMetadataReportsByTargetNid(string targetNid, MetadataElementType categeoryType)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + this.TablesName.MetadataReport + " AS M INNER JOIN " + this.TablesName.MetadataCategory + " AS C ON C." + DIColumns.Metadata_Category.CategoryNId + " = M." + DIColumns.MetadataReport.CategoryNid + " WHERE C." + DIColumns.Metadata_Category.CategoryType + "=" + DIQueries.MetadataElementTypeText[categeoryType] + " and M." + DIColumns.MetadataReport.TargetNid + "=" + targetNid;


            return RetVal;
        }
        /// <summary>
        /// Get all the metadata records present in Metadata Table
        /// </summary>
        /// <returns></returns>
        public string GetMetadata()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.MetadataReport.MetadataReportNid + "," + DIColumns.MetadataReport.TargetNid + "," + DIColumns.MetadataReport.Metadata);
            SqlQuery.Append(" FROM " + TablesName.MetadataReport);
            SqlQuery.Append(" WHERE " + DIColumns.MetadataReport.Metadata + " LIKE '%DIIMG__%'");
            return SqlQuery.ToString();
        }

        /// <summary>
        /// Get all the metadata records present in Metadata Table
        /// </summary>
        /// <param name="categeoryType">Indicator,Area,Source or Pass null to get all records together</param>
        /// <returns></returns>
        public string GetAllMetadataReportsByCategoryType(MetadataElementType categeoryType)
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append(" SELECT MC." + DIColumns.Metadata_Category.CategoryNId + ", MC." + DIColumns.Metadata_Category.CategoryName + ", MC." + DIColumns.Metadata_Category.CategoryType + ", MR." + DIColumns.MetadataReport.TargetNid + ", MR." + DIColumns.MetadataReport.Metadata + " ");
            SqlQuery.Append(" FROM " + TablesName.MetadataCategory + " as MC ");
            SqlQuery.Append(" inner join " + TablesName.MetadataReport + " as MR on MC." + DIColumns.Metadata_Category.CategoryNId + "=MR.Category_NId ");

            if (categeoryType != null)
            {
                SqlQuery.Append(" where MC." + DIColumns.Metadata_Category.CategoryType + "=" + DIQueries.MetadataElementTypeText[categeoryType] + " ");
            }

            SqlQuery.Append(" order by MC." + DIColumns.Metadata_Category.CategoryType + ",MR." + DIColumns.MetadataReport.TargetNid + ",MC." + DIColumns.Metadata_Category.CategoryOrder + "");
            return SqlQuery.ToString();
        }

        public string GetDuplicateMetadataByNid()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT M." + DIColumns.MetadataReport.MetadataReportNid + ",M." + DIColumns.MetadataReport.Metadata);

                RetVal.Append(" FROM " + this.TablesName.MetadataReport + " M");

                RetVal.Append(" WHERE EXISTS ( ");

                RetVal.Append("SELECT M1." + DIColumns.MetadataReport.MetadataReportNid);

                RetVal.Append(" FROM " + this.TablesName.MetadataReport + " M1");

                RetVal.Append(" GROUP BY M1." + DIColumns.MetadataReport.MetadataReportNid + " HAVING COUNT(*)>1 )");
            }
            catch (Exception)
            {
                RetVal.Length = 0;
            }

            return RetVal.ToString();
        }
        #endregion


        #endregion
    }

}
