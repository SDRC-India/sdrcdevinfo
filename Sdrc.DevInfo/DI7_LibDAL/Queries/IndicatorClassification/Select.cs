using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification
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

        #region "-- Methods --"

        /// <summary>
        /// Returns SELECT and FROM clause for IC table based on FieldSelection
        /// </summary>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        private string GetICSelectFromClause(FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICType + ",IC." + DIColumns.IndicatorClassifications.ICOrder +",IC."+ DIColumns.IndicatorClassifications.ISBN+",IC."+DIColumns.IndicatorClassifications.Nature;
            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += ",IC." + DIColumns.IndicatorClassifications.ICInfo;
            }
            RetVal += " FROM " + this.TablesName.IndicatorClassifications + " AS IC";
            return RetVal;
        }

        /// <summary>
        /// Returns SELECT and FROM clause for IC table based on FieldSelection
        /// </summary>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        private string GetICSelectFromClause(FieldSelection fieldSelection,bool includeISBNNature)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICType + ",IC." + DIColumns.IndicatorClassifications.ICOrder ;
            
            if (includeISBNNature)
            {
                RetVal += ",IC." + DIColumns.IndicatorClassifications.ISBN + ",IC." + DIColumns.IndicatorClassifications.Nature;
            }

            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += ",IC." + DIColumns.IndicatorClassifications.ICInfo;
            }

            RetVal += " FROM " + this.TablesName.IndicatorClassifications + " AS IC";
            return RetVal;
        }

        /// <summary>
        /// Returns Where clause based on FilterFieldType and FilterText
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <returns></returns>
        private string GetICWhereClause(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:

                        break;
                    case FilterFieldType.NId:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + filterText + ")";
                        break;
                    case FilterFieldType.GId:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICGId + " IN (" + filterText + ")";
                        break;
                    case FilterFieldType.Name:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICName + " IN (" + filterText + ")";
                        break;
                    case FilterFieldType.Search:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICName + " LIKE " + filterText;
                        break;
                    case FilterFieldType.ParentNId:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICParent_NId + " IN (" + filterText + ")";
                        break;
                    case FilterFieldType.NIdNotIn:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICNId + " NOT IN (" + filterText + ")";
                        break;
                    case FilterFieldType.NameNotIn:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICName + " NOT IN (" + filterText + ")";
                        break;
                    case FilterFieldType.Type:
                        RetVal = "IC." + DIColumns.IndicatorClassifications.ICType + " IN (" + filterText + ")";
                        break;

                    default:
                        break;
                }
            }
            return RetVal;
        }


        /// <summary>
        /// Returns SELECT and FROM clause for IC and ICIUSNId table based on FieldSelection
        /// </summary>
        /// <param name="fieldSelection">LIGHT: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, IUSNId
        /// <para>HEAVY: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, ICInfo, IUSNId</para>
        /// </param>        
        /// <returns></returns>
        private string GetICIUSSelectFromClause(FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT DISTINCT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICType;
            RetVal += ",ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId;
            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += "," + DIColumns.IndicatorClassifications.ICInfo;
            }

            // FROM Clause
            RetVal += " FROM " + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS";


            return RetVal;
        }

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
        /// Get list of Classification having No IUS. 
        /// </summary>
        /// <returns></returns>
        public string GetClassificationUnmatchedIUS()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + DIColumns.IndicatorClassifications.ICType + "," + DIColumns.IndicatorClassifications.ICName
                    + " FROM  " + this.TablesName.IndicatorClassifications
                    + " WHERE " + DIColumns.IndicatorClassifications.ICType + " <> " + DIQueries.ICTypeText[ICType.Source] + " AND " + DIColumns.IndicatorClassifications.ICNId
                    + " Not In (" + "SELECT ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " FROM "
                    + this.TablesName.IndicatorUnitSubgroup + " IUS,"
                    + this.TablesName.IndicatorClassificationsIUS + " ICIUS "
                    + " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + ") ORDER BY " + DIColumns.IndicatorClassifications.ICType;


            return RetVal;
        }
        

        /// <summary>
        /// Get IC Records for root parents (-1) for a given IC Type and Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">None = get all top level Classifications with ParentNId=-1.
        /// <para>Applicable for NId, Search, NameNotIn</para>
        /// </param>
        /// <param name="filterText"><para>blank will Filter only for ICParentNId=-1</para>
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="classificationType"></param>
        /// <returns>
        /// <para>NId, Name, GID, Global</para>
        /// </returns>
        public string GetICTopParents(FilterFieldType filterFieldType, string filterText, ICType classificationType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            sbQuery.Append("SELECT " + DIColumns.IndicatorClassifications.ICNId + "," + DIColumns.IndicatorClassifications.ICName + "," + DIColumns.IndicatorClassifications.ICGId + "," + DIColumns.IndicatorClassifications.ICGlobal);

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications);

            // WHERE Clause
            sbQuery.Append(" WHERE " + DIColumns.IndicatorClassifications.ICParent_NId + " = -1 ");
            sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.NId:
                        // Filter on NID
                        sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICNId + " IN(" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICName + " LIKE " + filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICName + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }

            // ORDER BY Clause
            sbQuery.Append(" ORDER BY " + DIColumns.IndicatorClassifications.ICName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IC Records for a given Filter Criteria and FieldSelection
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        public string GetIC(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection));
            string WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" WHERE " + WhereClause);
            }

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IC Records for a given IC Type and Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="classificationType"></param>
        /// <param name="fieldSelection">LIGHT: Returns - NId, PArentNId, Name, GID, Global
        /// <para>HEAVY: Returns - NId, PArentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetIC(FilterFieldType filterFieldType, string filterText, ICType classificationType, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string WhereClause = string.Empty;

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection,true));

            sbQuery.Append(" WHERE ");
            sbQuery.Append("IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);

            WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" AND " + WhereClause);
            }

            if (classificationType == ICType.Source)
            {
                sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            }
            else
            {
                sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICOrder);
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IC Records for a given Filter Criteria and FieldSelection
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="searchClause">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        public string GetICForSearch(FilterFieldType filterFieldType, string searchClause, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection));
            string WhereClause = searchClause;
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" WHERE " + WhereClause);
            }

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }
        

        /// <summary>
        /// Get Sources without publishers
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAllSourceColumnsWithoutPublishers()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("Select * from " + this.TablesName.IndicatorClassifications + " IC ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append("IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source]);

            sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICParent_NId + " NOT IN (-1)");

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }



        /// <summary>
        /// Get Source Records without publishers
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetsSourcesWithoutPublishers()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICSelectFromClause(FieldSelection.Light,true));

            sbQuery.Append(" WHERE ");
            sbQuery.Append("IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source]);

            sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICParent_NId + " NOT IN (-1)");

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get Indicator Classification records for a given IC Type and IndicatorNids
        /// </summary>
        /// <param name="classificationType">ICType enum value</param>
        /// <param name="IndicatorNIds">Comma delimited Indicator NIds</param>
        /// <param name="fieldSelection">LIGHT: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal,IndicatorNId
        /// <para>HEAVY: Returns - NId, PArentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetICForIndicators(ICType classificationType, string IndicatorNIds, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            sbQuery.Append("SELECT DISTINCT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal);
            sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IC." + DIColumns.IndicatorClassifications.ICOrder);
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.IndicatorClassifications.ICInfo);
            }

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);
            if (IndicatorNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IndicatorNIds + ")");
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Indicator Classification records  IndicatorNids
        /// </summary>        
        /// <param name="IndicatorNIds">Comma delimited Indicator NIds</param>
        /// <param name="fieldSelection">LIGHT: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal,IndicatorNId
        /// <para>HEAVY: Returns - NId, PArentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetICForIndicators(string IndicatorNIds, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            sbQuery.Append("SELECT DISTINCT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICType);
            sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.IndicatorClassifications.ICInfo);
            }

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            if (IndicatorNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IndicatorNIds + ")");
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Indicator Classification records for a given IUSNIds
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="IUSNIds">Comma delimited IUSNIds</param>
        /// <param name="fieldSelection">LIGHT: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, IUSNId
        /// <para>HEAVY: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, ICInfo, IUSNId</para>
        /// </param>        
        /// <returns></returns>
        public string GetICForIUSNId(FilterFieldType filterFieldType, string filterText, string IUSNIds, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICIUSSelectFromClause(fieldSelection));

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);
            if (IUSNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN (" + IUSNIds + ")");
            }

            string WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" AND " + WhereClause);
            }


            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Indicator Classification records for a given IC Type and IUSNId
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="IUSNIds">Comma delimited IUSNIds </param>
        /// <param name="classificationType">ICType enum value</param>
        /// <param name="fieldSelection">LIGHT: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, IUSNId
        /// <para>HEAVY: Returns - ICNId, ICParent_NId, ICName, ICGId, ICGlobal, ICType, ICInfo, IUSNId</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetICForIUSNId(FilterFieldType filterFieldType, string filterText, string IUSNIds, ICType classificationType, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICIUSSelectFromClause(fieldSelection));

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);
            if (IUSNIds.Trim().Length > 0)
            {
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN (" + IUSNIds + ")");
            }

            string WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" AND " + WhereClause);
            }


            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns records from UT_Indicator_Classifications_IUS table
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <returns></returns>
        public string GetICIUSNid(int ICNid, int IUSNid)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT * from " + this.TablesName.IndicatorClassificationsIUS + " where " + DIColumns.IndicatorClassificationsIUS.ICNId + "=" + ICNid + " and " + DIColumns.IndicatorClassificationsIUS.IUSNId + " = " + IUSNid + "";
            return RetVal;
        }

        /// <summary>
        /// Returns IC_IUSNid from UT_Indicator_Classifications_IUS table
        /// </summary>
        /// <param name="ICNids">Comma separated IC Nids</param>
        /// <param name="IUSNids">Comma separated IUS NIds which may be empty</param>
        /// <returns></returns>
        public string GetICIUSNid(string ICNids, string IUSNids)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT * FROM " + this.TablesName.IndicatorClassificationsIUS + " WHERE " + DIColumns.IndicatorClassificationsIUS.ICNId + " IN (" + ICNids + ") ";

            if (!string.IsNullOrEmpty(IUSNids))
            {
                RetVal += " AND " + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN ( " + IUSNids + ") ";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns CF_FlowChart from CF_FlowChart table
        /// </summary>
        /// <returns></returns>
        public string GetCFFlowCharts()
        {
            string SqlString = string.Empty;
            SqlString = "SELECT " + DIColumns.CFFlowChart.CF_FlowChart + " FROM " + this.TablesName.CFFlowChart;
            return SqlString;
        }

        /// <summary>
        /// Get ICs on the basis of parent NId and Ic Type
        /// </summary>
        /// <param name="icType"> Indicator Classifcation type </param>
        /// <param name="parentNId"> Parent Nid </param>
        /// <returns></returns>
        public string GetICForParentNIdAndICType(ICType icType, int parentNId)
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT IC." + DIColumns.IndicatorClassifications.ICNId + " ,IC." + DIColumns.IndicatorClassifications.ICName + " ,IC." + DIColumns.IndicatorClassifications.ICGId);
                RetVal.Append(" ,IC." + DIColumns.IndicatorClassifications.ICGlobal + " ,IC." + DIColumns.IndicatorClassifications.ICType + " ,IC." + DIColumns.IndicatorClassifications.ICParent_NId);
                RetVal.Append(" ,IC." + DIColumns.IndicatorClassifications.ICOrder);

                RetVal.Append(" FROM " + TablesName.IndicatorClassifications + " IC");

                RetVal.Append(" WHERE IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[icType]);
                RetVal.Append(" AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " = " + parentNId);

                RetVal.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }

        /// <summary>
        /// Select Duplicate ICNID and IUSNID In ICIUS Table
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateICIUSRecords()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + ", IC." + DIColumns.IndicatorClassifications.ICNId + " ,IC." + DIColumns.IndicatorClassifications.ICName + " ,ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);

            RetVal.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + TablesName.IndicatorClassificationsIUS + " ICIUS");

            RetVal.Append(" WHERE IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " AND EXISTS (");

            RetVal.Append("SELECT  " + DIColumns.IndicatorClassificationsIUS.ICNId + "," + DIColumns.IndicatorClassificationsIUS.IUSNId);
            RetVal.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS);
            RetVal.Append(" GROUP BY " + DIColumns.IndicatorClassificationsIUS.ICNId + "," + DIColumns.IndicatorClassificationsIUS.IUSNId);
            RetVal.Append(" HAVING COUNT(*)>1 )");

            return RetVal.ToString();
        }

        /// <summary>
        /// Get Unmatch IC_IUS based on IUSNID
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedICIUSByIUSNID()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT Distinct(" + DIColumns.IndicatorClassificationsIUS.IUSNId + ")   FROM " + this.TablesName.IndicatorClassificationsIUS);

            SbQuery.Append(" AS ICIUS WHERE NOT EXISTS (SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup);

            SbQuery.Append(" AS IUS WHERE ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " =  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ")");

            RetVal = SbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get Unmatch IC_IUS based on IC_NID
        /// </summary> 
        public string GetUnmatchedICIUSByICNID()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT Distinct(" + DIColumns.IndicatorClassificationsIUS.ICNId + ")  FROM " + this.TablesName.IndicatorClassificationsIUS);

            SbQuery.Append(" AS ICIUS WHERE NOT EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassifications);

            SbQuery.Append(" AS IC WHERE ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId + " )");

            RetVal = SbQuery.ToString();

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

            sbQuery.Append("SELECT IC." + DIColumns.IndicatorClassifications.ICNId + " AS " + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName + " AS " + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId + " AS " + DIColumns.IndicatorClassifications.ICGId);
            sbQuery.Append(",IC1." + DIColumns.IndicatorClassifications.ICNId + ",IC1." + DIColumns.IndicatorClassifications.ICName + ",IC1." + DIColumns.IndicatorClassifications.ICGId + " FROM " + this.TablesName.IndicatorClassifications + " IC," + table.IndicatorClassifications + " IC1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + "= IC1." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICGId + "<> IC1." + DIColumns.IndicatorClassifications.ICGId);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns sql query to get the max ic order with in the given parent nid
        /// </summary>
        /// <param name="parentNid"></param>
        /// <returns></returns>
        public string GetMaxICOrder(int parentNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select Max(" + DIColumns.IndicatorClassifications.ICOrder + ") FROM " + this.TablesName.IndicatorClassifications + " WHERE " + DIColumns.IndicatorClassifications.ICParent_NId + "=" + parentNid.ToString();

            return RetVal;
        }


        /// <summary>
        /// Returns sql query to get the max IC_IUS order with in the given IC nid
        /// </summary>
        /// <param name="ICNid"></param>
        /// <returns></returns>
        public string GetMaxICIUSOrder(int ICNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select Max(" + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + ") FROM " + this.TablesName.IndicatorClassificationsIUS + " WHERE " + DIColumns.IndicatorClassificationsIUS.ICNId + "=" + ICNid.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get IndicatorNids for Convention IC(Developement IC)
        /// </summary>
        /// <returns></returns>
        public string GetDevelopmentIndicatorNIds()
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId);

            Sb.Append(" FROM " + this.TablesName.IndicatorClassifications + " AS IC ," + this.TablesName.Indicator + " AS I, " + this.TablesName.IndicatorUnitSubgroup + " AS IUS, " + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS ");

            Sb.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);

            Sb.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);

            Sb.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);
            
            Sb.Append(" AND (IC." + DIColumns.IndicatorClassifications.ICType + "= " + DIQueries.ICTypeText[ICType.Convention] + ")");

            RetVal = Sb.ToString();

            return RetVal;
        }

       
        /// <summary>
        /// Get Indicator Classifications By IndicatorNids for Sector and ParentNId=-1  
        /// </summary>
        /// <param name="indicatorNIds"></param>
        /// <returns></returns>
        public string GetDevelopmentICByIndicators(string indicatorNIds)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT DISTINCT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICParent_NId + ", IC." + DIColumns.IndicatorClassifications.ICName + ", IC." + DIColumns.IndicatorClassifications.ICGId + ", IC." + DIColumns.IndicatorClassifications.ICGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ", IC." + DIColumns.IndicatorClassifications.ICOrder );
                        
           
            Sb.Append(" FROM " + this.TablesName.IndicatorClassifications + " AS IC ," + this.TablesName.IndicatorUnitSubgroup + " AS IUS, " + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS ");

           
            Sb.Append(" WHERE ");

            Sb.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);

            Sb.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);

            Sb.Append(" AND (IC." + DIColumns.IndicatorClassifications.ICType + "= " + DIQueries.ICTypeText[ICType.Sector] + ")");
            
            Sb.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN(" + indicatorNIds + ")");
            
            Sb.Append(" AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + "= -1");

            RetVal = Sb.ToString();

            return RetVal;

        }
        /// <summary>
        /// Getting Duplicate Records of Indicator Classifications By ICNid
        /// </summary>
        /// <returns></returns>
        public string GetDupliacteICRecordsByICNid()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGId);

                RetVal.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC");

                RetVal.Append(" WHERE EXISTS ( ");

                RetVal.Append("SELECT IC1." + DIColumns.IndicatorClassifications.ICNId);

                RetVal.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC1");

                RetVal.Append(" GROUP BY IC1." + DIColumns.IndicatorClassifications.ICNId + " HAVING COUNT(*)>1 )");
            }
            catch (Exception)
            {
                RetVal.Length = 0;
            }


            return RetVal.ToString();

        }
        public string GetInValidSdmxCompliantICGid()
        {

            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.IndicatorClassifications.ICGId + "," + DIColumns.IndicatorClassifications.ICName + "," + DIColumns.IndicatorClassifications.ICNId);
            SqlQuery.Append(" FROM " + TablesName.IndicatorClassifications);
            

            return SqlQuery.ToString();
        }
        /// <summary>
        /// Get Records of Blank IC Name from Indicator Classification Table
        /// </summary>
        /// <returns></returns>
        public string GetBlankICName()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.IndicatorClassifications.ICNId + "," + DIColumns.IndicatorClassifications.ICGId);
            SqlQuery.Append(" FROM " + TablesName.IndicatorClassifications);
            SqlQuery.Append(" WHERE " + DIColumns.IndicatorClassifications.ICName + " IS NULL");
            return SqlQuery.ToString();

        }
        #region "-- DI7 Changes --"

        /// <summary>
        /// Get IC Records for a given Filter Criteria and FieldSelection
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        public string GetIC(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection,bool includeISBNNature)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection, includeISBNNature));
            string WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" WHERE " + WhereClause);
            }

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IC Records for a given IC Type and Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="filterText">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="classificationType"></param>
        /// <param name="fieldSelection">LIGHT: Returns - NId, PArentNId, Name, GID, Global
        /// <para>HEAVY: Returns - NId, PArentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetIC(FilterFieldType filterFieldType, string filterText, ICType classificationType, FieldSelection fieldSelection, bool includeISBNNature)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string WhereClause = string.Empty;

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection, includeISBNNature));

            sbQuery.Append(" WHERE ");
            sbQuery.Append("IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[classificationType]);

            WhereClause = this.GetICWhereClause(filterFieldType, filterText);
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" AND " + WhereClause);
            }

            if (classificationType == ICType.Source)
            {
                sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            }
            else
            {
                sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICOrder);
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IC Records for a given Filter Criteria and FieldSelection
        /// </summary>
        /// <param name="filterFieldType">
        /// <para>Applicable for NId, GId, Name, Search, ParentNId, NIdNotIn, NameNotIn, Type</para>        
        /// </param>
        /// <param name="searchClause">
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%UNSD%' or '%UNSD'</para>
        /// </param>
        /// <param name="fieldSelection">
        /// <para>LIGHT: Returns - NId, ParentNId, Name, GID, Global</para>
        /// <para>HEAVY: Returns - NId, ParentNId, Name, GID, Global, Info</para>
        /// </param>
        /// <returns></returns>
        public string GetICForSearch(FilterFieldType filterFieldType, string searchClause, FieldSelection fieldSelection, bool includeISBNNature)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetICSelectFromClause(fieldSelection, includeISBNNature));
            string WhereClause = searchClause;
            if (WhereClause.Length > 0)
            {
                sbQuery.Append(" WHERE " + WhereClause);
            }

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        #endregion
        #endregion


        #endregion

    }
}

