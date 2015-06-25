using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes
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
        /// Get SELECT clause for SubgroupTypes table
        /// </summary>
        /// <param name="distinct">defines wheteher to add distinct keyword to select statement</param>
        /// <returns></returns>
        private string GetSelectClause(bool distinct)
        {
            string RetVal = "SELECT " ;
            if (distinct)
            {
                RetVal += " DISTINCT ";
            }
            RetVal += DIColumns.SubgroupTypes.SubgroupTypeNId + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeOrder + "," + DIColumns.SubgroupTypes.SubgroupTypeGlobal + "," + DIColumns.SubgroupTypes.SubgroupTypeGID;
            return RetVal;
        }


        /// <summary>
        /// Returns sql query to get subgroup types
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="filterString"></param>
        /// <returns></returns>
        public string GetSubgroupTypes(FilterFieldType filterType, string filterString)
        {

            StringBuilder RetVal = new StringBuilder();

            RetVal.Append(this.GetSelectClause(false));
            RetVal.Append(" FROM " + this.TablesName.SubgroupType);

            if (!string.IsNullOrEmpty(filterString))
            {
                RetVal.Append(" WHERE 1=1 ");

                switch (filterType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        RetVal.Append(" AND " + DIColumns.SubgroupTypes.SubgroupTypeNId + " IN (" + filterString + ")");
                        break;

                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;

                    case FilterFieldType.GId:
                        RetVal.Append(" AND " + DIColumns.SubgroupTypes.SubgroupTypeGID + " IN (" + filterString + ")");
                        break;
                    case FilterFieldType.Name:
                        RetVal.Append(" AND " + DIColumns.SubgroupTypes.SubgroupTypeName + " IN (" + filterString + ")");
                        break;
                    case FilterFieldType.Type:
                        break;
                    case FilterFieldType.Search:
                        RetVal.Append(filterString);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        RetVal.Append(" AND " + DIColumns.SubgroupTypes.SubgroupTypeNId + " NOT IN (" + filterString + ")");
                        break;                       
                    case FilterFieldType.NameNotIn:
                        break;
                    case FilterFieldType.Level:
                        break;
                    case FilterFieldType.LayerNid:
                        break;
                    default:
                        break;
                }

            }
            RetVal.Append(" ORDER BY " + DIColumns.SubgroupTypes.SubgroupTypeOrder + " ");
            return RetVal.ToString();
        }

        /// <summary>
        /// Get Distinct SubgroupTypes based on SubgroupValNId
        /// </summary>
        /// <param name="SubgroupValNId">Comma delimited SubgroupValNId which may be null or empty</param>
        /// <returns></returns>
        public string GetSubgroupTypes(string SubgroupValNId)
        {
            string RetVal = string.Empty;
            RetVal = this.GetSelectClause(true);
            RetVal += " FROM " + this.TablesName.SubgroupType + " AS ST, " + this.TablesName.SubgroupValsSubgroup + " AS SGVS, " + this.TablesName.Subgroup + " AS SG";
            RetVal += " WHERE SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " = " + "SG." + DIColumns.Subgroup.SubgroupNId;
            RetVal += " AND SG." + DIColumns.Subgroup.SubgroupType + " = " + "ST." + DIColumns.SubgroupTypes.SubgroupTypeNId;
            if (!string.IsNullOrEmpty(SubgroupValNId))
            {
                RetVal += " AND SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN (" + SubgroupValNId + ")";
            }
            RetVal += " ORDER BY ST." + DIColumns.SubgroupTypes.SubgroupTypeOrder;
            return RetVal;
        }

        /// <summary>
        /// Get Nid and Name where two language table have different Gid with same Nid
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string GetUnmatchedGidForLanguage(string dataPrefix, string langCode)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            DITables table = new DITables(dataPrefix, langCode);

            sbQuery.Append("SELECT S." + DIColumns.SubgroupTypes.SubgroupTypeNId + " AS " + DIColumns.SubgroupTypes.SubgroupTypeNId + ",S." + DIColumns.SubgroupTypes.SubgroupTypeName + " AS " + DIColumns.SubgroupTypes.SubgroupTypeName + ",S." + DIColumns.SubgroupTypes.SubgroupTypeGID + " AS " + DIColumns.SubgroupTypes.SubgroupTypeGID);
            sbQuery.Append(",S1." + DIColumns.SubgroupTypes.SubgroupTypeNId + ",S1." + DIColumns.SubgroupTypes.SubgroupTypeName + ",S1." + DIColumns.SubgroupTypes.SubgroupTypeGID + " FROM " + this.TablesName.SubgroupType + " S," + table.SubgroupType + " S1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" S." + DIColumns.SubgroupTypes.SubgroupTypeNId + "= S1." + DIColumns.SubgroupTypes.SubgroupTypeNId );
            sbQuery.Append(" AND S." + DIColumns.SubgroupTypes.SubgroupTypeGID + "<> S1." + DIColumns.SubgroupTypes.SubgroupTypeGID);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        ///  Returns sql query to get max subgroup type order
        /// </summary>
        /// <returns></returns>
        public string GetMaxSubgroupTypeOrder()
        {
            string RetVal = string.Empty;

            RetVal = "Select max(" + DIColumns.SubgroupTypes.SubgroupTypeOrder + ") FROM " + this.TablesName.SubgroupType;

            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Records of SubGroup Type By NId
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubGroupTypeByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeNId + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID);
            SqlQuery.Append(" FROM " + this.TablesName.SubgroupType);
            SqlQuery.Append(" WHERE " + DIColumns.SubgroupTypes.SubgroupTypeNId + " IN(");

            SqlQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeNId);
            SqlQuery.Append(" FROM " + this.TablesName.SubgroupType + " GROUP BY ");
            SqlQuery.Append(DIColumns.SubgroupTypes.SubgroupTypeNId + " HAVING COUNT(*) >1 )");

            return SqlQuery.ToString();
        }

        public string GetInValidSdmxCompliantSubgroupTypeGid()
        {
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeGID + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeNId);
            SqlQuery.Append(" FROM " + TablesName.SubgroupType);
            

            return SqlQuery.ToString();
        }
        #region "-- Duplicate Records --"

        /// <summary>
        /// Get Duplicate SubgroupType By Name and Gids
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupType()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();
            
            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeNId + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID );
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType);
            SbQuery.Append(" WHERE EXISTS (");

            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID );
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType + " GROUP BY ");
            SbQuery.Append(DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupTypeName
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupTypeByName()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeNId + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType + " WHERE " + DIColumns.SubgroupTypes.SubgroupTypeName + " IN(");
            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeName );
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType + " GROUP BY ");
            SbQuery.Append(DIColumns.SubgroupTypes.SubgroupTypeName + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupTypeGid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupTypeByGid()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();
            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeNId + "," + DIColumns.SubgroupTypes.SubgroupTypeName + "," + DIColumns.SubgroupTypes.SubgroupTypeGID);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType );
            SbQuery.Append(" WHERE " + DIColumns.SubgroupTypes.SubgroupTypeGID + " IN(");

            SbQuery.Append("SELECT " + DIColumns.SubgroupTypes.SubgroupTypeGID);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupType + " GROUP BY ");
            SbQuery.Append( DIColumns.SubgroupTypes.SubgroupTypeGID + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }
               

        #endregion

        #endregion

        #endregion

    }
}
