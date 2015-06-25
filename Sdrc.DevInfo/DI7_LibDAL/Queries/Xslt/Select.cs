using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Xslt
{
    /// <summary>
    /// Provides sql queries to get records
    /// </summary>
    public class Select
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion


        #region "-- New / Dispose --"

        private Select()
        {
            // don't implement this method
        }

        #endregion

        #endregion

        #region "-- Public / Internal --"

        #region "-- New / Dispose --"


        internal Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"


        /// <summary>
        /// Returns sql query to get ElementXSLT table for a given  elementNId and ElementType
        /// </summary>
        /// <param name="elementNId">elementNId </param>
        /// <param name="metadataElementType">Indicator / Area / Source</param>
        /// <returns></returns>
        public string GetElementXSLTTable(string elementNId, MetadataElementType metadataElementType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * ");
            sbQuery.Append(" FROM " + this.TablesName.ElementXSLT + " ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(DIColumns.EelementXSLT.ElementType + " = " + DIQueries.MetadataElementTypeText[metadataElementType]);
            sbQuery.Append(" AND " + DIColumns.EelementXSLT.ElementNId + " IN (" + elementNId + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get StyleSheet details from XSLT table for a given  elementNId and ElementType
        /// </summary>
        /// <param name="elementNId">elementNId for which icon is to be extracted</param>
        /// <param name="metadataElementType">Indicator / Area / Source</param>
        /// <returns></returns>
        public string GetXSLT(string elementNId, MetadataElementType metadataElementType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT X." + DIColumns.XSLT.XSLTNId + ",EX." + DIColumns.EelementXSLT.ElementNId + ",X." + DIColumns.XSLT.XSLTText + ",X." + DIColumns.XSLT.XSLTFile);
            sbQuery.Append(" FROM " + this.TablesName.XSLT + " AS X," + this.TablesName.ElementXSLT + " AS EX");

            sbQuery.Append(" WHERE X." + DIColumns.XSLT.XSLTNId + " = EX." + DIColumns.EelementXSLT.XSLTNId);
            sbQuery.Append(" AND EX." + DIColumns.EelementXSLT.ElementType + " = " + DIQueries.MetadataElementTypeText[metadataElementType]);

            if (elementNId.Trim().Length >= 0)
            {
                sbQuery.Append(" AND EX." + DIColumns.EelementXSLT.ElementNId + " IN (" + elementNId + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Text and FileName from XSLT table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be XSLT.File LIKE '%DI_Source%' </param>
        /// <returns>Sql query string</returns>
        public string GetXSLT(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT " + DIColumns.XSLT.XSLTNId + "," + DIColumns.XSLT.XSLTText + "," + DIColumns.XSLT.XSLTFile);    //FieldSelection.NId

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.XSLT);

            //   WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.XSLT.XSLTNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(" AND " + DIColumns.XSLT.XSLTFile + " ='" + filterText + "' ");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(" AND " + filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(" AND " + DIColumns.XSLT.XSLTNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(" AND " + DIColumns.XSLT.XSLTFile + " <>'" + filterText + "' ");
                        break;
                    default:
                        break;
                }
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetXSLT()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.XSLT.XSLTNId + "," + DIColumns.XSLT.XSLTText + "," + DIColumns.XSLT.XSLTFile);
            SqlQuery.Append(" FROM " + TablesName.XSLT);
            return SqlQuery.ToString();
        }
        public string UpdateXSLT(string xsltnid, string correctxslt)
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("Update " + TablesName.XSLT + " SET " + DIColumns.XSLT.XSLTText + " = '" + correctxslt +"'");
           
            SqlQuery.Append(" WHERE " + DIColumns.XSLT.XSLTNId + " IN ( ");
            SqlQuery.Append(xsltnid + " )");

            return SqlQuery.ToString();

        }
        #endregion

        #endregion
    }
}
