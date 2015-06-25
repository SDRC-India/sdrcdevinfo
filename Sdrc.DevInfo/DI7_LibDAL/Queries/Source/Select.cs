using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Source
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
        /// Returns query to get records for entries of recommended sources 
        /// </summary>
        /// <returns></returns>
        public string GetForRecommendedSourceEntries(string IUSNIds)
        {
            string RetVal = string.Empty;

            StringBuilder sSql = new System.Text.StringBuilder();

            sSql.Append("SELECT  ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", IC." + DIColumns.IndicatorClassifications.ICName + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IC." + DIColumns.IndicatorClassifications.ICGlobal + ", ");
            sSql.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder  + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSLabel );

            sSql.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS, ");
            sSql.Append(this.TablesName.IndicatorUnitSubgroup + " IUS, ");
            sSql.Append(this.TablesName.Indicator + " I, ");
            sSql.Append(this.TablesName.Unit + " U, ");
            sSql.Append(this.TablesName.SubgroupVals + " S, ");
            sSql.Append(this.TablesName.IndicatorClassifications + " IC ");

            sSql.Append(" WHERE ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND ");
            sSql.Append(" U." + DIColumns.Unit.UnitNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " AND ");
            sSql.Append(" S." + DIColumns.SubgroupVals.SubgroupValNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " AND ");
            sSql.Append(" IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " AND ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " AND ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICType + " = 'SR' AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " <> -1 AND");
            sSql.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN(" + IUSNIds + ")");

            sSql.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName + " Asc, U." + DIColumns.Unit.UnitName + " Asc, S." + DIColumns.SubgroupVals.SubgroupVal + " Asc ");

            RetVal = sSql.ToString();

            return RetVal;
        }

        /// <summary>
        /// Returns query to get records for entries of recommended sources 
        /// </summary>
        /// <returns></returns>
        public string GetForRecommendedSourceEntries(string IUSNIds, string areaNIds, string timeperiodNIds)
        {
            string RetVal = string.Empty;

            StringBuilder sSql = new System.Text.StringBuilder();

            sSql.Append("SELECT DISTINCT  ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", IC." + DIColumns.IndicatorClassifications.ICName + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IC." + DIColumns.IndicatorClassifications.ICGlobal + ", ");
            sSql.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSLabel);

            sSql.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS, ");            
            sSql.Append(this.TablesName.Indicator + " I, ");
            sSql.Append(this.TablesName.Unit + " U, ");
            sSql.Append(this.TablesName.SubgroupVals + " S, ");
            sSql.Append(this.TablesName.IndicatorClassifications + " IC, ");
            sSql.Append(this.TablesName.Data + " D ");

            sSql.Append(" WHERE ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorNId + "= D." + DIColumns.Data.IndicatorNId + " AND ");
            sSql.Append(" U." + DIColumns.Unit.UnitNId + "= D." + DIColumns.Data.UnitNId + " AND ");
            sSql.Append(" S." + DIColumns.SubgroupVals.SubgroupValNId + "= D." + DIColumns.Data.SubgroupValNId + " AND ");            
            sSql.Append(" D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId + " AND ");
            sSql.Append(" D." + DIColumns.Data.IUSNId + "= ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " AND ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " AND ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICType + " = 'SR' AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " <> -1 AND");
           
            sSql.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " IN(" + IUSNIds + ") ");

            if (!string.IsNullOrEmpty(areaNIds))
            {
                sSql.Append(" AND D." + DIColumns.Data.AreaNId + " IN(" + areaNIds + ") ");
            }

            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                sSql.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN(" + timeperiodNIds + ") ");
            }

            sSql.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName + " Asc, U." + DIColumns.Unit.UnitName + " Asc, S." + DIColumns.SubgroupVals.SubgroupVal + " Asc ");

            RetVal = sSql.ToString();

            return RetVal;
        }


        /// <summary>
        /// Get Query To return Sources  where metadata is missing
        /// </summary>
        /// <returns></returns>
        public string GetMissingInfoSources(DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append( "SELECT " + DIColumns.IndicatorClassifications.ICName + " FROM " + this.TablesName.IndicatorClassifications);
            
            sbQuery.Append(  " WHERE ((" + DIColumns.IndicatorClassifications.ICInfo + " IS Null )");

            if (serverType == DIServerType.MsAccess)
            {

                sbQuery.Append(" OR RTrim(LTrim(" + DIColumns.IndicatorClassifications.ICInfo + ")) = '') ");
            }
            else
            {
                sbQuery.Append(" OR (" + DIColumns.IndicatorClassifications.ICInfo + " like  '') OR (" + DIColumns.IndicatorClassifications.ICInfo + " like  ' ')) ");
            }
             
            sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICType + "= " + DIQueries.ICTypeText[ICType.Source] + " AND " + DIColumns.IndicatorClassifications.ICParent_NId + " <>-1 ");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get query to return Sources missing Data.
        /// </summary>
        /// <returns></returns>
        public string GetSourcesWithoutData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + DIColumns.IndicatorClassifications.ICName
                    + " FROM " + this.TablesName.IndicatorClassifications
                    + " WHERE " + DIColumns.IndicatorClassifications.ICType + "= " + DIQueries.ICTypeText[ICType.Source]
                    + " AND " + DIColumns.IndicatorClassifications.ICParent_NId + " <> -1 " + " AND " + DIColumns.IndicatorClassifications.ICNId
                    + " NOT IN ( SELECT ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " FROM "
                    + this.TablesName.IndicatorClassificationsIUS + " ICIUS "
                    + " INNER JOIN " + this.TablesName.Data + " D "
                    + " ON ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "= D." + DIColumns.Data.IUSNId + ")";

            return RetVal;
        }

       
        /// <summary>
        /// Retrieves IndGId_UnitGid_SubgroupGIds for given IUSNId_SourceNIds
        /// </summary>
        /// <param name="IUSNId_SourceNIds">IUSNId_SourceNIds</param>
        /// <param name="DIServerType">Server Type information required for concat syntax</param>
        /// <returns></returns>
        public string GetIndGId_UnitGId_SubgroupGID_SoureNames(string IUSNId_SourceNIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",IC." + DIColumns.IndicatorClassifications.ICName, ",", Delimiter.TEXT_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND (" + DIQueries.SQL_GetConcatenatedValues("IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IC." + DIColumns.IndicatorClassifications.ICNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") IN (" + IUSNId_SourceNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetIUSNId_SourceNIds(string IndGId_UnitGId_SubgroupGID_SoureNames, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IC." + DIColumns.IndicatorClassifications.ICNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",IC." + DIColumns.IndicatorClassifications.ICName, ",", Delimiter.TEXT_SEPARATOR, DIServerType) + ") IN (" + IndGId_UnitGId_SubgroupGID_SoureNames + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetSource_Rec(string IUSNId_SourceNIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            string _Query = string.Empty;
            _Query = "SELECT " + DIColumns.IndicatorClassificationsIUS.IUSNId + ", " + DIColumns.IndicatorClassificationsIUS.ICNId + ", " + DIColumns.IndicatorClassificationsIUS.RecommendedSource + ", " + DIColumns.IndicatorClassificationsIUS.ICIUSOrder + ", " + DIColumns.IndicatorClassificationsIUS.ICIUSLabel + " FROM " + this.TablesName.IndicatorClassificationsIUS;
            if (!string.IsNullOrEmpty(IUSNId_SourceNIds))
            {
                _Query += " WHERE " + DIQueries.SQL_GetConcatenatedValues(DIColumns.IndicatorClassificationsIUS.IUSNId + "," + DIColumns.IndicatorClassificationsIUS.ICNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType) + " IN (" + IUSNId_SourceNIds + ")";
            }

            RetVal = _Query;
            return RetVal;
        }

        public string GetSource_RecOnly()
        {
            string RetVal = string.Empty;
            string _Query = string.Empty;
            _Query = "SELECT " + DIColumns.IndicatorClassificationsIUS.IUSNId + ", " + DIColumns.IndicatorClassificationsIUS.ICNId + ", " + DIColumns.IndicatorClassificationsIUS.RecommendedSource + " FROM " + this.TablesName.IndicatorClassificationsIUS;
            _Query += " WHERE " + DIColumns.IndicatorClassificationsIUS.RecommendedSource + " <> 0";

            RetVal = _Query;
            return RetVal;
        }

        public string GetSource(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection, bool onlyParent)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //   SELECT clause
            sbQuery.Append("SELECT " + DIColumns.IndicatorClassifications.ICNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.IndicatorClassifications.ICName + "," + DIColumns.IndicatorClassifications.ICGId + "," + DIColumns.IndicatorClassifications.ICGlobal+","+ DIColumns.IndicatorClassifications.ICParent_NId );
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.IndicatorClassifications.ICInfo);
            }

            //   FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications);

            //   WHERE Clause
            sbQuery.Append(" WHERE " + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType.Source]);

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {
                sbQuery.Append(" AND ");
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.IndicatorClassifications.ICNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.IndicatorClassifications.ICGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.IndicatorClassifications.ICName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.IndicatorClassifications.ICNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.IndicatorClassifications.ICName + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }

            if (onlyParent)
            {
                sbQuery.Append(" AND " + DIColumns.IndicatorClassifications.ICParent_NId + " =-1 ");
            }

            //   ORDER BY Clause
            sbQuery.Append(" ORDER BY " + DIColumns.IndicatorClassifications.ICName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// AutoSelect Source based on Indiactor, Area, and Timeperiod selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="timeperiodNids">commma delimited Source NIds which may be blank</param>
        /// <param name="IUSNIds">commma delimited IUSNIds which may be blank </param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIndicatorAreaTimeperiod(string indicatorNIds, string areaNIds, string timeperiodNIds, string IUSNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ", IC." + DIColumns.IndicatorClassifications.ICGlobal);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorClassifications + " AS IC");

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            }

            sbQuery.Append(" WHERE IC." + DIColumns.IndicatorClassifications.ICNId + " = D." + DIColumns.Data.SourceNId);

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ");
            }

            if (timeperiodNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timeperiodNIds + ") ");
            }

            if (IUSNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ") ");
            }

            sbQuery.Append(" ORDER BY IC." + DIColumns.IndicatorClassifications.ICName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// AutoSelect Source based on Indiactor, Area, and Timeperiod selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator/IUS NIds which may be blank</param>
        /// <param name="showIUS"></param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="timeperiodNIds">commma delimited Source NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectSource(string indicatorNIds, bool showIUS, string areaNIds, string timeperiodNIds)
        {
            string RetVal = string.Empty;

            RetVal = this.GetAutoSelectSource(indicatorNIds,showIUS, areaNIds, timeperiodNIds, string.Empty);

            return RetVal;
        }


        /// <summary>
        /// AutoSelect Source based on Indiactor, Area, and Timeperiod selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator/IUS NIds which may be blank</param>
        /// <param name="showIUS"></param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="timeperiodNIds">commma delimited Source NIds which may be blank</param>
        /// <param name="sourceParentNId">Source parent NId </param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectSource(string indicatorNIds,bool showIUS, string areaNIds, string timeperiodNIds,  string sourceParentNId)
        {
            string RetVal = string.Empty;

            // IC Table
            RetVal = "SELECT  IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ", IC." + DIColumns.IndicatorClassifications.ICGlobal 
                + ",IC." + DIColumns.IndicatorClassifications.ISBN + ",IC." + DIColumns.IndicatorClassifications.Nature
                + " FROM " + this.TablesName.IndicatorClassifications + " AS IC ";


            // Data table
            RetVal += " WHERE  EXISTS ( SELECT *  FROM  " + this.TablesName.Data + " AS D WHERE  IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId;


            //if indicator nids is given
            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                if (!showIUS)
                {
                    RetVal += " AND EXISTS ";

                    //Indicator
                    RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND EXISTS ";

                    RetVal += " ( SELECT * FROM " + this.TablesName.Indicator + " AS I  WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN(" + indicatorNIds + ") ";

                    RetVal += " )) ";
                }
                else
                {
                    RetVal += " AND EXISTS ";

                    //Indicator
                    RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN(" + indicatorNIds + ") ";

                    RetVal += " ) ";
                }
            }

            
            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timeperiodNIds + " ) ";
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }


            RetVal += ") "; //ORDER BY IC." + DIColumns.IndicatorClassifications.ICName;


            if (!string.IsNullOrEmpty(sourceParentNId))
            {
                RetVal += " AND " + DIColumns.IndicatorClassifications.ICParent_NId + "=" + sourceParentNId + " ";
            }

            RetVal += " ORDER BY IC." + DIColumns.IndicatorClassifications.ICName;

            return RetVal;
        }

        /// <summary>
        /// Auto Distinct SourceNIDs present in UT_Data table.
        /// </summary>
        public string GetAutoDistinctSources()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT D." + DIColumns.Data.SourceNId + ", IC." + DIColumns.IndicatorClassifications.ICNId + ", IC." + DIColumns.IndicatorClassifications.ICName + ", IC." + DIColumns.IndicatorClassifications.ICGlobal
                + " FROM " + this.TablesName.Data + " AS D INNER JOIN " + this.TablesName.IndicatorClassifications + " AS IC "
                         + "  ON IC." + DIColumns.IndicatorClassifications.ICNId + " = D." + DIColumns.Data.SourceNId;

            return RetVal;
        }

        /// <summary>
        /// Query return auto selected source and IUS
        /// </summary>
        /// <param name="showIUS"></param>
        /// <param name="indicatorNIds"></param>
        /// <param name="sourceNIds"></param>
        /// <returns></returns>
        public string GetDataWAutoSelectedSourcesWithIUS(bool showIUS, string indicatorNIds, string sourceNIds)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IUSNId + ", I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName);
                sbQuery.Append(", SGV." + DIColumns.SubgroupVals.SubgroupVal + " ,IC." + DIColumns.IndicatorClassifications.ICNId + " ,IC." + DIColumns.IndicatorClassifications.ICName);
                sbQuery.Append(", ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);

                sbQuery.Append(" FROM " + this.TablesName.Indicator + " I," + this.TablesName.SubgroupVals + " SGV," + this.TablesName.Unit + " U,");
                sbQuery.Append(this.TablesName.Data + " D," + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.IndicatorClassificationsIUS + " ICIUS ");

                sbQuery.Append(" WHERE ");
                sbQuery.Append(" U." + DIColumns.Unit.UnitNId + " = D." + DIColumns.Data.UnitNId + " AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = D." + DIColumns.Data.SubgroupValNId);
                sbQuery.Append(" AND  I." + DIColumns.Indicator.IndicatorNId + " = D." + DIColumns.Data.IndicatorNId + " AND IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = " + "IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " = D." + DIColumns.Data.IUSNId);

                if (showIUS && !string.IsNullOrEmpty(indicatorNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + indicatorNIds + ")");
                }
                else if (!string.IsNullOrEmpty(indicatorNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNIds + ")");
                }

                if (!string.IsNullOrEmpty(sourceNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");
                }

                Retval = sbQuery.ToString();
            }
            catch (Exception)
            {
                Retval = null;
            }
            return Retval;
        }

        /// <summary>
        /// Query to return the ICNId and IUSNId on the basis comma seprated of ICIUSNIds
        /// </summary>
        /// <param name="iciusNId"></param>
        /// <returns></returns>
        public string GetICNIdIUSNId(string iciusNId)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DISTINCT ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " ,");
                sbQuery.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId);

                sbQuery.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS");

                sbQuery.Append(" WHERE ");
                sbQuery.Append(" ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + " IN (" + iciusNId + ")");

                Retval = sbQuery.ToString();
            }
            catch (Exception)
            {
                Retval = null;
            }
            return Retval;
        }

        #endregion

        #endregion

    }
}
