using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Area
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

        #region "-- Enumerator--"

        /// <summary>
        /// Constants to define order by field
        /// </summary>
        public enum OrderBy
        {
            AreaId = 0,
            AreaName = 1,
            AreaNId = 2,
            AreaParentNId = 3
        }

        /// <summary>
        /// Constants to define map filter field type
        /// </summary>
        public enum MapFilterFieldType
        {
            None,
            LayerNId,
            LayerName,
            AreaNId,
            AreaLevel
        }


        #endregion

        #region "-- Methods --"

        #region "-- Area Table --"

        /// <summary>
        /// Get the string of missing area MetaDAtaText
        /// </summary>
        /// <returns></returns>
        public string GetMissingInfoArea(DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + DIColumns.Area_Map_Metadata.LayerName + " FROM " + this.TablesName.AreaMapMetadata;
            
            if (serverType == DIServerType.MsAccess)
            {
                RetVal  += " WHERE ((" + DIColumns.Area_Map_Metadata.MetadataText + " IS Null) or RTrim(LTrim(" + DIColumns.Area_Map_Metadata.MetadataText + ")) = '')";
            }
            else
            {
                RetVal += " WHERE (" + DIColumns.Area_Map_Metadata.MetadataText + " IS Null OR " + DIColumns.Area_Map_Metadata.MetadataText + " LIKE '')";
            }

            return RetVal;
        }

        /// <summary>
        /// Get Area missing Data.
        /// </summary>
        /// <returns></returns>
        public string GetAreasWithoutData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + DIColumns.Area.AreaParentNId + "," + DIColumns.Area.AreaID + "," + DIColumns.Area.AreaName + "," + DIColumns.Area.AreaLevel + "," + DIColumns.Area.AreaMap
                    + " FROM " + this.TablesName.Area
                    + " WHERE " + DIColumns.Area.AreaNId + " Not In (SELECT A." + DIColumns.Area.AreaNId
                    + " FROM " + this.TablesName.Data + " D "
                    + " Left JOIN " + this.TablesName.Area + " A ON D."
                    + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Get query to get AreaNID, ParentNId, AreaId,Name, AreaLevelName from Area_Level , AreaMap,AreaGlobal, AreaBlock from Area and Area_Level TAble
        /// </summary>
        /// <returns></returns>
        public string GetAreaWithLevelName()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT A." + DIColumns.Area.AreaNId + ", A." + DIColumns.Area.AreaParentNId + ", A." + DIColumns.Area.AreaID + ", A."
                + DIColumns.Area.AreaName + ", A." + DIColumns.Area.AreaGId + ", A." + DIColumns.Area.AreaLevel + ", A."
                + DIColumns.Area.AreaMap + ", A." + DIColumns.Area.AreaBlock + ", A." + DIColumns.Area.AreaGlobal + ", AL."
                + DIColumns.Area_Level.AreaLevelName + " FROM " + this.TablesName.Area + " A, " + this.TablesName.AreaLevel + " AL "
                + " WHERE AL." + DIColumns.Area_Level.AreaLevel + " = A." + DIColumns.Area.AreaLevel + " ORDER BY A." + DIColumns.Area.AreaID;

            return RetVal;
        }


        public string GetArea(FilterFieldType filterFieldType, string filterText, OrderBy OrderBy)
        {
            string RetVal = string.Empty;
            RetVal = this.GetArea(filterFieldType, filterText);
            RetVal += " ";
            switch (OrderBy)
            {
                case OrderBy.AreaId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaID;
                    break;
                case OrderBy.AreaName:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaName;
                    break;
                case OrderBy.AreaNId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaNId;
                    break;
                case OrderBy.AreaParentNId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaParentNId;
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Get  NId, ParentNId, Name, GID, Global,ID from Area for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">None = get all top level Classifications with ParentNId=-1.
        /// <para>Applicable for NId, ParentNId, ID, GId, Search, NIdNotIn, NameNotIn, Level</para>
        /// </param>
        /// <param name="filterText"><para>blank will Filter only for ICParentNId=-1</para>
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%Ind%' or '%Africa%'</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetArea(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area.AreaNId + "," + DIColumns.Area.AreaParentNId + "," + DIColumns.Area.AreaName + "," + DIColumns.Area.AreaGId + "," + DIColumns.Area.AreaGlobal + "," + DIColumns.Area.AreaID + "," + DIColumns.Area.AreaBlock + "," + DIColumns.Area.AreaMap + "," + DIColumns.Area.AreaLevel);
            sbQuery.Append(" FROM " + this.TablesName.Area);
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
                sbQuery.Append(" WHERE ");

            if (!string.IsNullOrEmpty(filterText))
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Area.AreaNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        sbQuery.Append(DIColumns.Area.AreaParentNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ID:
                        sbQuery.Append(DIColumns.Area.AreaID + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.Area.AreaGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Area.AreaName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        //sbQuery.Append( DIColumns.Area.AreaName + " LIKE " + FilterText );
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Area.AreaNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Area.AreaName + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Level:
                        sbQuery.Append(DIColumns.Area.AreaLevel + "=" + filterText);
                        break;
                    default:
                        break;
                }
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, ParentNId, Name, GID, Global,ID from Area for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">None = get all top level Classifications with ParentNId=-1.
        /// <para>Applicable for NId, ParentNId, ID, GId, Search, NIdNotIn, NameNotIn, Level,Dataexist</para>
        /// </param>
        /// <param name="filterText"><para>blank will Filter only for ICParentNId=-1</para>
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%Ind%' or '%Africa%'</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetAreasWithDataExist(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area.AreaNId + "," + DIColumns.Area.AreaParentNId + "," + DIColumns.Area.AreaName + "," + DIColumns.Area.AreaGId + "," + DIColumns.Area.AreaGlobal + "," + DIColumns.Area.AreaID + "," + DIColumns.Area.AreaBlock + "," + DIColumns.Area.AreaMap + "," + DIColumns.Area.AreaLevel + "," + DIColumns.Area.DataExist);
            sbQuery.Append(" FROM " + this.TablesName.Area);
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
                sbQuery.Append(" WHERE ");

            if (!string.IsNullOrEmpty(filterText))
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Area.AreaNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        sbQuery.Append(DIColumns.Area.AreaParentNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ID:
                        sbQuery.Append(DIColumns.Area.AreaID + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.Area.AreaGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Area.AreaName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        //sbQuery.Append( DIColumns.Area.AreaName + " LIKE " + FilterText );
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Area.AreaNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Area.AreaName + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Level:
                        sbQuery.Append(DIColumns.Area.AreaLevel + "=" + filterText);
                        break;
                    default:
                        break;
                }
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get area details by sorted AreaName,AreaID,AreaNID,AreaParentNId
        /// </summary>
        /// <param name="filterFieldType"></param>
        /// <param name="filterText"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public string GetAreasWithDataExist(FilterFieldType filterFieldType, string filterText, OrderBy OrderBy)
        {
            string RetVal = string.Empty;
            RetVal = this.GetAreasWithDataExist(filterFieldType, filterText);
            RetVal += " ";
            switch (OrderBy)
            {
                case OrderBy.AreaId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaID;
                    break;
                case OrderBy.AreaName:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaName;
                    break;
                case OrderBy.AreaNId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaNId;
                    break;
                case OrderBy.AreaParentNId:
                    RetVal += "ORDER BY " + DIColumns.Area.AreaParentNId;
                    break;
                default:
                    break;
            }

            return RetVal;
        }


        public string GetAreaByAreaLevel(string AreaLevel)
        {
            string RetVal = GetArea(FilterFieldType.None, "");

            RetVal += " WHERE " + DIColumns.Area.AreaLevel + " IN (" + AreaLevel + ") ORDER BY " + DIColumns.Area.AreaLevel + "," + DIColumns.Area.AreaName;

            return RetVal;
        }

        public string GetAreaByLayer(string layerNId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT AM." + DIColumns.Area_Map.LayerNId + ",A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaParentNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaGId + ",A." + DIColumns.Area.AreaGlobal + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaBlock + ",A." + DIColumns.Area.AreaLevel);
            sbQuery.Append(" FROM " + this.TablesName.Area + " AS A," + this.TablesName.AreaMap + " AS AM");
            sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + " = AM." + DIColumns.Area_Map.AreaNId + " AND AM." + DIColumns.Area_Map.LayerNId + " IN (" + layerNId + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect Area based on Indiactor and TimePeriod selection
        /// </summary>
        /// <param name="parentNId">Parent NIds which may be -1</param>
        /// <param name="currentLevel">Area level of selected area node which may be 0</param>
        /// <param name="requiredLevel">Area level till which area are to be considered which may be 0</param>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="timePeriodNIds">commma delimited TimePeriod NIds which may be blank</param>
        /// <param name="areaBlocksNIds">commma delimited area NIds stored in area block, which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIndicatorTimePeriod(int parentNId, int currentLevel, int requiredLevel, string indicatorNIds, string timePeriodNIds, string areaBlocksNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string _WhereClause = string.Empty;

            int _LevelDifference;
            int iCount;

            if (parentNId == -1)
            {
                currentLevel = 0;
            }

            _LevelDifference = Math.Abs(requiredLevel) - Math.Abs(currentLevel);

            if (_LevelDifference < 0)
            {
                _LevelDifference = 0;
            }
            else if (_LevelDifference > 0)
            {
                _LevelDifference -= 1;
            }

            //TODO include other fields in Select clause
            sbQuery.Append("SELECT DISTINCT A" + _LevelDifference + "." + DIColumns.Area.AreaNId + ",A" + _LevelDifference + "." + DIColumns.Area.AreaID + ",A" + _LevelDifference + "." + DIColumns.Area.AreaName);
            sbQuery.Append(",A" + _LevelDifference + "." + DIColumns.Area.AreaGlobal + ",A" + _LevelDifference + "." + DIColumns.Area.AreaLevel);

            sbQuery.Append(" FROM " + this.TablesName.Area + " AS A0," + this.TablesName.Data + " AS D");


            if (_LevelDifference > 0)
            {
                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    sbQuery.Append("," + this.TablesName.Area + " A" + iCount);
                }

                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    if (_WhereClause.Length > 0)
                        _WhereClause += " AND ";
                    _WhereClause += " A" + iCount.ToString() + "." + DIColumns.Area.AreaParentNId + " = A" + ((int)(iCount - 1)).ToString() + "." + DIColumns.Area.AreaNId;
                }
            }

            if (indicatorNIds.Length > 0)
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (timePeriodNIds.Length > 0)
                sbQuery.Append("," + this.TablesName.TimePeriod + " AS T");

            if (_WhereClause.Length > 0)
                _WhereClause += " AND ";

            _WhereClause += "A" + _LevelDifference.ToString() + "." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId;

            if (indicatorNIds.Length > 0)
                _WhereClause += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ")";

            if (timePeriodNIds.Length > 0)
                _WhereClause += " AND T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId + " AND T." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")";

            //_WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId.ToString();

            if (string.IsNullOrEmpty(areaBlocksNIds.Trim()))
            {
                if (currentLevel == -1 && requiredLevel == -1)
                {
                    _WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId;
                }
                else if (requiredLevel > 0)
                {
                    _WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId;
                }
            }
            else
            {
                _WhereClause += " AND A" + _LevelDifference + "." + DIColumns.Area.AreaNId + " IN (" + areaBlocksNIds + ")";
            }

            //TODO Handling for Area Block auto select. See DI5_Query->QueryBase->Area_GetAutoSelect

            RetVal = sbQuery.ToString();
            if (_WhereClause.Length > 0)
                RetVal += " WHERE " + _WhereClause;

            return RetVal;
        }

        /// <summary>
        /// AutoSelect TimePeriod based on IUS and Area selection
        /// </summary>
        /// <param name="parentNId">Parent NIds which may be -1</param>
        /// <param name="currentLevel">Area level of selected area node which may be 0</param>
        /// <param name="requiredLevel">Area level till which area are to be considered which may be 0</param>
        /// <param name="IUSNIds">commma delimited IUS NIds which may be blank</param>
        /// <param name="timePeriodNIds">commma delimited TimePeriod NIds which may be blank</param>
        /// <param name="areaBlocksNIds">commma delimited area NIds stored in area block, which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIUSTimePeriod(int parentNId, int currentLevel, int requiredLevel, string IUSNIds, string timePeriodNIds, string areaBlocksNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string _WhereClause = string.Empty;

            int _LevelDifference;
            int iCount;

            if (parentNId == -1)
                currentLevel = 0;

            _LevelDifference = Math.Abs(requiredLevel) - Math.Abs(currentLevel);

            if (_LevelDifference < 0)
                _LevelDifference = 0;

            if (_LevelDifference > 0)
                _LevelDifference -= 1;

            sbQuery.Append("SELECT DISTINCT A" + _LevelDifference + "." + DIColumns.Area.AreaNId + ",A" + _LevelDifference + "." + DIColumns.Area.AreaID + ",A" + _LevelDifference + "." + DIColumns.Area.AreaName);
            sbQuery.Append(",A" + _LevelDifference + "." + DIColumns.Area.AreaGlobal + ",A" + _LevelDifference + "." + DIColumns.Area.AreaLevel);

            sbQuery.Append(" FROM " + this.TablesName.Area + " AS A0," + this.TablesName.Data + " AS D");


            if (_LevelDifference > 0)
            {
                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    sbQuery.Append("," + this.TablesName.Area + " A" + iCount);
                }

                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    if (_WhereClause.Length > 0)
                        _WhereClause += " AND ";
                    _WhereClause += " A" + iCount.ToString() + "." + DIColumns.Area.AreaParentNId + " = A" + ((int)(iCount - 1)).ToString() + "." + DIColumns.Area.AreaNId;
                }
            }

            if (IUSNIds.Length > 0)
                sbQuery.Append("," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (timePeriodNIds.Length > 0)
                sbQuery.Append("," + this.TablesName.TimePeriod + " AS T");

            if (_WhereClause.Length > 0)
                _WhereClause += " AND ";

            _WhereClause += "A" + _LevelDifference.ToString() + "." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId;

            if (IUSNIds.Length > 0)
                _WhereClause += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + IUSNIds + ")";

            if (timePeriodNIds.Length > 0)
                _WhereClause += " AND T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId + " AND T." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")";

            //_WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId.ToString();

            if (string.IsNullOrEmpty(areaBlocksNIds.Trim()))
            {
                if (currentLevel == -1 && requiredLevel == -1)
                {
                    _WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId;
                }
                else if (requiredLevel > 0)
                {
                    _WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId;
                }
            }
            else
            {
                _WhereClause += " AND A" + _LevelDifference + "." + DIColumns.Area.AreaNId + " IN (" + areaBlocksNIds + ")";
            }


            //TODO Handling for Area Block auto select. See DI5_Query->QueryBase->Area_GetAutoSelect

            RetVal = sbQuery.ToString();
            if (_WhereClause.Length > 0)
                RetVal += " WHERE " + _WhereClause;

            return RetVal;
        }

        /// <summary>
        /// AutoSelect Area based on Indiactor, timperiod, and source selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="timeperiodNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNIds">commma delimited Source NIds which may be blank</param>
        /// <param name="IUSNIds">commma delimited IUSNIds which may be blank </param>
        /// <param name="requiredLevel">Required area level which may be -1 to get level's area</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIndicatorTimeperiodSource(string indicatorNIds, string timeperiodNIds, string sourceNIds, string IUSNIds, int requiredLevel)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaGlobal);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.Area + " AS A ");

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
            }

            sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId);

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            if (timeperiodNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timeperiodNIds + ") ");
            }

            if (sourceNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ") ");
            }

            if (IUSNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ") ");
            }

            if (requiredLevel > 0)
            {
                sbQuery.Append("  AND A." + DIColumns.Area.AreaLevel + " =" + requiredLevel + " ");
            }

            sbQuery.Append(" ORDER BY A." + DIColumns.Area.AreaName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect Area based on Indiactor, timperiod, and source selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator/IUS NIds which may be blank</param>
        /// <param name="showIUS"></param>
        /// <param name="timeperiodNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNIds">commma delimited Source NIds which may be blank</param>
        /// <param name="requiredLevel">Required area level which may be -1 to get level's area</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectAreas(string indicatorNIds, bool showIUS, string timeperiodNIds, string sourceNIds, int requiredLevel)
        {
            string RetVal = string.Empty;

            RetVal = this.GetAutoSelectAreas(indicatorNIds, showIUS, timeperiodNIds, sourceNIds, requiredLevel, string.Empty);

            return RetVal;
        }

        /// <summary>
        /// AutoSelect Area based on Indiactor, timperiod, and source selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="timeperiodNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNIds">commma delimited Source NIds which may be blank</param>
        /// <param name="IUSNIds">commma delimited IUSNIds which may be blank </param>
        /// <param name="requiredLevel">Required area level which may be -1 to get all levels area</param>
        /// <param name="availableAreaNIDs">Available Area NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectAreas(string indicatorNIds, bool showIUS, string timeperiodNIds, string sourceNIds, int requiredLevel, string availableAreaNIDs)
        {
            string RetVal = string.Empty;

            // AREA Table
            RetVal = "SELECT  A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaGlobal + ",A." + DIColumns.Area.AreaLevel + " FROM " + this.TablesName.Area + " AS A ";


            // Data table
            RetVal += " WHERE  ";

            if (requiredLevel > 0)
            {
                RetVal += "  A." + DIColumns.Area.AreaLevel + " =" + requiredLevel + " AND ";
            }


            RetVal += " EXISTS ( SELECT *  FROM  " + this.TablesName.Data + " AS D WHERE  A." + DIColumns.Area.AreaNId + "= D." + DIColumns.Data.AreaNId;


            //if indicator nids is given
            if (!string.IsNullOrEmpty(indicatorNIds))
            {

                if (!showIUS)
                {
                    //////RetVal += " AND EXISTS ";

                    ////////Indicator
                    //////RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND EXISTS ";

                    //////RetVal += " ( SELECT * FROM " + this.TablesName.Indicator + " AS I  WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN(" + indicatorNIds + ") ";

                    //////RetVal += " )) ";

                    RetVal += " AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNIds + ") ";
                }
                else
                {

                    ////RetVal += " AND EXISTS ";

                    //////IUS
                    ////RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN(" + indicatorNIds + ") ";

                    ////RetVal += " ) ";

                    RetVal += " AND D." + DIColumns.Data.IUSNId + " IN (" + indicatorNIds + ") ";
                }
            }


            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timeperiodNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNIds))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNIds + " ) ";
            }

            RetVal += ") ";

            // get auto select records by parent nid
            if (!string.IsNullOrEmpty(availableAreaNIDs))
            {
                RetVal += " AND A." + DIColumns.Area.AreaNId + " IN (" + availableAreaNIDs + ")";
            }

            RetVal += " ORDER BY A." + DIColumns.Area.AreaName;

            return RetVal;
        }

        /// <summary>
        /// Use this query to autoselect the Areas. Tables used: Data
        /// </summary>
        /// <param name="indicatorNIDs">Stores comma seperated Indicator NIDs - Can be blank</param>
        /// <param name="IUSNIDs">Stores comma seperated IUSNIDs - Can be blank</param>
        /// <param name="timePeriodsNIDs">Stores comma seperated TimePeriod NIDs - Can be blank</param>
        /// <param name="AreaNIDs">Stores comma seperated Area NIDs - Can be blank</param>
        /// <returns>Query - Select includes only the Area NID from the Data Table</returns>
        public string AutoSelectArea(string indicatorNIDs, string IUSNIDs, string timePeriodsNIDs, string AreaNIDs)
        {
            //TODO: AutoSelectArea - Change Query once the Indicator NID column is available in the Data Table

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string _WhereClause = string.Empty;

            sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.AreaNId);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");

            // Indicator NIDs (TO BE CHANGED - Check TODO)
            if (indicatorNIDs.Length > 0)
            {
                if (_WhereClause.Length > 0)
                {
                    _WhereClause += " AND ";
                }
                sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");

                _WhereClause += " I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId;
                _WhereClause += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId;

                //TODO Where Clause needs to be changed here once IndicatorNID column is part of the Data Table
                _WhereClause += " AND I." + DIColumns.Indicator.IndicatorNId + " in (" + indicatorNIDs + ")";
            }
            // IUSNIDs
            if (IUSNIDs.Length > 0)
            {
                if (_WhereClause.Length > 0)
                {
                    _WhereClause += " AND ";
                }
                _WhereClause += " D." + DIColumns.Data.IUSNId + " in (" + IUSNIDs + ")";
            }
            // TimePeriod NIDs
            if (timePeriodsNIDs.Length > 0)
            {
                if (_WhereClause.Length > 0)
                {
                    _WhereClause += " AND ";
                }
                _WhereClause += " D." + DIColumns.Data.TimePeriodNId + " in (" + timePeriodsNIDs + ")";
            }
            // Area NIDs
            if (AreaNIDs.Length > 0)
            {
                if (_WhereClause.Length > 0)
                {
                    _WhereClause += " AND ";
                }
                _WhereClause += " D." + DIColumns.Data.AreaNId + " in (" + AreaNIDs + ")";
            }

            // Append Where clause
            if (_WhereClause.Length > 0)
            {
                _WhereClause = " WHERE " + _WhereClause;
                sbQuery.Append(_WhereClause);
            }

            // return the Query
            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// AutoSelect Area based on Indiactor, timperiod,source selection and where area_parent_nid is equal to the given nid
        /// </summary>
        /// <param name="areaParentNId"></param>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="showIUS">Ture if indicatorNIds is for IUS otherwise false</param>
        /// <param name="timeperiodNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNIds">commma delimited Source NIds which may be blank</param>
        /// <param name="IUSNIds">commma delimited IUSNIds which may be blank </param>
        /// <param name="requiredLevel">Required area level which may be -1 to get all levels area</param>
        /// <param name="availableAreaNIDs">Available Area NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectAreasForParentArea(int areaParentNId, string indicatorNIds, bool showIUS, string timeperiodNIds, string sourceNIds)
        {
            string RetVal = string.Empty;

            // AREA Table
            RetVal = "SELECT  A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaGlobal + ",A." + DIColumns.Area.AreaLevel + " FROM " + this.TablesName.Area + " AS A ";


            // Data table
            RetVal += " WHERE  ";

            RetVal += "  A." + DIColumns.Area.AreaParentNId + " =" + areaParentNId.ToString() + " AND ";



            RetVal += " EXISTS ( SELECT *  FROM  " + this.TablesName.Data + " AS D WHERE  A." + DIColumns.Area.AreaNId + "= D." + DIColumns.Data.AreaNId;


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

                    //IUS
                    RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN(" + indicatorNIds + ") ";

                    RetVal += " ) ";
                }
            }


            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timeperiodNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNIds))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNIds + " ) ";
            }

            RetVal += ") ";


            RetVal += " ORDER BY A." + DIColumns.Area.AreaName;

            return RetVal;
        }

        /// <summary>
        /// Get the AreaNId, area Name, Area global and Area block on the basis of area parent NId.
        /// </summary>
        /// <param name="parent_nid">Comma delimited area parent Nids</param>
        /// <returns></returns>
        public string GetParentData(string parent_nid)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT A." + DIColumns.Area.AreaNId + ", A." + DIColumns.Area.AreaName + ", A." + DIColumns.Area.AreaGlobal);
            sbQuery.Append(", A." + DIColumns.Area.AreaBlock);

            sbQuery.Append(" FROM " + TablesName.Area + " A");

            if (string.IsNullOrEmpty(parent_nid))
            {
                sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + " = -1");
            }
            else
            {
                sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + " IN (" + parent_nid + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name from Area for a given Source and Filter Criteria
        /// </summary>
        /// <param name="filterFieldType"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        /// <remarks>Used in CCC Dashboard</remarks>
        public string GetAreaBySource(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT A." + DIColumns.Area.AreaNId + ", A." + DIColumns.Area.AreaName;
            RetVal += " FROM " + this.TablesName.Area + " A, " + this.TablesName.Data + " D, " + this.TablesName.IndicatorClassifications + " IC";
            RetVal += "  WHERE A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + " AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId + " AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType.Source];

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.NId:
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        break;
                    case FilterFieldType.Name:
                        break;
                    case FilterFieldType.Search:
                        RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICName + " LIKE " + filterText;
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        break;
                    case FilterFieldType.NameNotIn:
                        break;
                    default:
                        break;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Get concatinated path for desired area starting from root area
        /// </summary>
        /// <param name="areaNId">Area NId</param>
        /// <param name="areaLevel">Area Level</param>
        /// <param name="dIServerType">Server Type</param>
        /// <returns></returns>
        public string GetAreaChain(int areaNId, int areaLevel, DI_LibDAL.Connection.DIServerType dIServerType)
        {
            string RetVal = string.Empty;
            string FromClause = string.Empty;
            string WhereClause = string.Empty;
            string SelfJoinTablePrefix = string.Empty;
            int RequiredLevel = 1;
            int LevelDifference = LevelDifference = Math.Abs(RequiredLevel - areaLevel);
            if (LevelDifference > 0)
            {
                for (int i = 0; i <= LevelDifference; i++)
                {
                    if (i == 0)
                    {
                        SelfJoinTablePrefix = "A" + i + "." + DIColumns.Area.AreaName;
                        FromClause = " FROM " + this.TablesName.Area + " AS A" + i;
                        WhereClause = " WHERE A" + LevelDifference + "." + DIColumns.Area.AreaNId + " = " + areaNId.ToString();
                    }
                    else
                    {
                        SelfJoinTablePrefix += Delimiter.TEXT_DELIMITER + "A" + i + "." + DIColumns.Area.AreaName;
                        FromClause += "," + this.TablesName.Area + " AS A" + i;
                        WhereClause += " AND A" + i + "." + DIColumns.Area.AreaParentNId + " = A" + (i - 1) + "." + DIColumns.Area.AreaNId;
                    }
                }
                RetVal = "SELECT DISTINCT ";
                RetVal += DIQueries.SQL_GetConcatenatedValues(SelfJoinTablePrefix, Delimiter.TEXT_DELIMITER, " - ", dIServerType);
                RetVal += " AS AreaChain ";
                RetVal = RetVal + FromClause + WhereClause;
            }
            else
            {
                RetVal = "SELECT " + DIColumns.Area.AreaName + " FROM " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaNId + " = " + areaNId;
            }
            return RetVal;
        }

        /// <summary>
        /// Get Area NIds on the basis of timeperiod, Area and IUS NIds.
        /// </summary>
        /// <param name="timePeriodNIds">Comma seprated timeperiod NIds</param>
        /// <param name="areaNIds">Comma seprated area NIds</param>
        /// <param name="iusNIds">Comma seprated IUS NIds</param>
        /// <returns></returns>
        public string GetAreaNIdsByTimePeriodAreaIUS(string timePeriodNIds, string areaNIds, string iusNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT D." + DIColumns.Data.AreaNId);

            sbQuery.Append(" FROM " + TablesName.Data + " D ");

            sbQuery.Append(" WHERE  1=1 ");

            if (!string.IsNullOrEmpty(timePeriodNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");
            }

            if (!string.IsNullOrEmpty(areaNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");
            }

            if (!string.IsNullOrEmpty(iusNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + iusNIds + ")");
            }

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get the Area NId,Name ,level ,global ,Id on the basis of parent NId, current level, required level and area NIds
        /// </summary>
        /// <param name="parentNId"></param>
        /// <param name="currentLevel"></param>
        /// <param name="requiredLevel"></param>
        /// <param name="areaNIDs"></param>
        /// <returns></returns>
        public string GetAreaByAreaNIds(int parentNId, int currentLevel, int requiredLevel, string areaNIDs)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string _WhereClause = string.Empty;

            int _LevelDifference;
            int iCount;

            if (parentNId == -1)
            {
                currentLevel = 0;
            }

            _LevelDifference = Math.Abs(requiredLevel) - Math.Abs(currentLevel);

            if (_LevelDifference < 0)
            {
                _LevelDifference = 0;
            }
            else if (_LevelDifference > 0)
            {
                _LevelDifference -= 1;
            }

            //TODO include other fields in Select clause
            sbQuery.Append("SELECT DISTINCT A" + _LevelDifference + "." + DIColumns.Area.AreaNId + ",A" + _LevelDifference + "." + DIColumns.Area.AreaID + ",A" + _LevelDifference + "." + DIColumns.Area.AreaName);
            sbQuery.Append(",A" + _LevelDifference + "." + DIColumns.Area.AreaGlobal + ",A" + _LevelDifference + "." + DIColumns.Area.AreaLevel);

            sbQuery.Append(" FROM " + this.TablesName.Area + " AS A0");

            _WhereClause += "1=1 ";

            if (_LevelDifference > 0)
            {
                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    sbQuery.Append("," + this.TablesName.Area + " A" + iCount);
                }

                for (iCount = 1; iCount <= _LevelDifference; iCount++)
                {
                    if (_WhereClause.Length > 0)
                        _WhereClause += " AND ";
                    _WhereClause += " A" + iCount.ToString() + "." + DIColumns.Area.AreaParentNId + " = A" + ((int)(iCount - 1)).ToString() + "." + DIColumns.Area.AreaNId;
                }
            }

            _WhereClause += " AND A0." + DIColumns.Area.AreaParentNId + " = " + parentNId.ToString();

            if (!string.IsNullOrEmpty(areaNIDs))
            {
                _WhereClause += " AND A0." + DIColumns.Area.AreaNId + " IN (" + areaNIDs + ")";
            }

            sbQuery.Append(" WHERE " + _WhereClause);

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Query to validate area NId on the basis areaNId and area Level
        /// </summary>
        /// <param name="areaNId">Comma seprated area NId</param>
        /// <param name="areaLevel">Area level</param>
        /// <returns></returns>
        public string GetAreaNIdByAreaLevel(string areaNId,int areaLevel)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT A." + DIColumns.Area.AreaNId);
            sbQuery.Append(" FROM " + this.TablesName.Area + " A");
            sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + " IN (" + areaNId + ") AND A." + DIColumns.Area.AreaLevel + " = " + areaLevel);
         
            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get areas on the basis of levels
        /// </summary>
        /// <param name="levels"></param>
        /// <returns></returns>
        public string GetAreasByAreaLevels(string levels)
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {

                RetVal.Append("SELECT A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaParentNId + ",A." + DIColumns.Area.AreaName);

                RetVal.Append(",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaLevel);

                RetVal.Append(" FROM " + this.TablesName.Area + " A");

                RetVal.Append(" WHERE A." + DIColumns.Area.AreaLevel + " IN (" + levels + ")");
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }

        public string GetDuplicateAreasBYAreaID()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaName);

                RetVal.Append(" FROM " + this.TablesName.Area + " A");

                RetVal.Append(" WHERE EXISTS ( " );

                RetVal.Append("SELECT A1." + DIColumns.Area.AreaID );

                RetVal.Append(" FROM " + this.TablesName.Area + " A1");

                RetVal.Append(" GROUP BY A1." + DIColumns.Area.AreaID + " HAVING COUNT(*)>1 )");

            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
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

            DITables LangTable = new DITables(dataPrefix, langCode);

            sbQuery.Append("SELECT A." + DIColumns.Area.AreaNId + " AS " + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaID + " AS " + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaGId + " AS " + DIColumns.Area.AreaGId);
            sbQuery.Append(",A1." + DIColumns.Area.AreaNId + ",A1." + DIColumns.Area.AreaID + ",A1." + DIColumns.Area.AreaGId);
            sbQuery.Append (" FROM " + this.TablesName.Area + " A," + LangTable.Area + " A1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" A." + DIColumns.Area.AreaID + "= A1." + DIColumns.Area.AreaID);
            sbQuery.Append(" AND A." + DIColumns.Area.AreaGId + "<> A1." + DIColumns.Area.AreaGId);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get all the invalid sdmx compliant Area GID.
        /// </summary>
        /// <returns></returns>
        public string GetInValidSdmxCompliantGid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Area.AreaGId + "," + DIColumns.Area.AreaName + "," + DIColumns.Area.AreaNId);
            SqlQuery.Append(" FROM " + TablesName.Area);
            

            return SqlQuery.ToString();
        
        }
        /// <summary>
        /// Getting the missing Area Nid for which Map Exists.
        /// </summary>
        /// <returns></returns>
        public string GetMissingAreaNidByMapNid()
        {
            string RetVal = String.Empty;
            RetVal = "Select M.* from " + TablesName.AreaMap + " M  where NOT Exists (Select * from " + TablesName.Area + " As A where A." + DIColumns.Area.AreaNId + " = M." + DIColumns.Area_Map.AreaNId + " )";
            return RetVal;
        }
        public string GetDuplicateAreasByAreaNid()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaName);

                RetVal.Append(" FROM " + this.TablesName.Area + " A");

                RetVal.Append(" WHERE EXISTS ( ");

                RetVal.Append("SELECT A1." + DIColumns.Area.AreaNId);

                RetVal.Append(" FROM " + this.TablesName.Area + " A1");

                RetVal.Append(" GROUP BY A1." + DIColumns.Area.AreaNId + " HAVING COUNT(*)>1 )");
            }
            catch (Exception)
            {
                RetVal.Length = 0;
            }

            return RetVal.ToString();

        }

        #endregion

        #region "-- Area_Level Table --"

        /// <summary>
        /// Retruns query to get max area level from area table
        /// </summary>
        /// <returns></returns>
        public string GetMaxAreaLevelFrmAreaTable()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT MAX(" + DIColumns.Area.AreaLevel + ") as " + DIColumns.Area.AreaLevel + " FROM " + this.TablesName.Area;

            return RetVal;
        }

        /// <summary>
        /// Get  LevelNId, AreaLevel, AreaLevelName from AreaLevel for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Use None for all levels. Use ID filter for AreaLevel
        /// <para>Applicable for NId,ID(AreaLevel),Name,Search,NIdNotIn,NameNotIn</para>
        /// </param>
        /// <param name="filterText"><para>blank will get all AreaLevel</para>
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%Ind%' or '%Africa%'</para>
        /// </param>
        /// <returns>
        /// </returns>
        public string GetAreaLevel(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area_Level.LevelNId + "," + DIColumns.Area_Level.AreaLevel + "," + DIColumns.Area_Level.AreaLevelName);
            sbQuery.Append(" FROM " + this.TablesName.AreaLevel);
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {
                sbQuery.Append(" WHERE ");
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Area_Level.LevelNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        sbQuery.Append(DIColumns.Area_Level.AreaLevel + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.GId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Area_Level.AreaLevelName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        //sbQuery.Append( DIColumns.Area.AreaName + " LIKE " + FilterText );
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Area_Level.LevelNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Area_Level.AreaLevelName + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Level:
                        sbQuery.Append(DIColumns.Area_Level.AreaLevel + " IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }

            }


            RetVal = sbQuery.ToString();
            return RetVal;


        }

        /// <summary>
        /// Returns all areas for the given level and for the given parentNid
        /// </summary>
        /// <param name="parentNId"></param>
        /// <param name="parentLevel"></param>
        /// <param name="forLevel"></param>
        /// <returns></returns>
        public string GetAreaByParentNId(int parentNId,int parentLevel, int forLevel)
        {
            string RetVal = string.Empty;
            string SearchString = string.Empty;

            SearchString =  parentNId.ToString();
            for (int i = parentLevel+1; i < forLevel; i++)
            {
                SearchString = " SELECT " + DIColumns.Area.AreaNId + " FROM " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaParentNId + " IN ("  + SearchString +")";    
            }

            RetVal = "SELECT * FROM " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaParentNId + " IN ( " + SearchString + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns all areas for the given level and for the given childNid
        /// </summary>
        /// <param name="childNId"></param>
        /// <param name="childLevel"></param>
        /// <param name="forLevel"></param>
        /// <returns></returns>
        public string GetParentAreaByChildNId(int childNId, int childLevel, int forLevel)
        {
            string RetVal = string.Empty;
            string SearchString = string.Empty;

            SearchString = childNId.ToString();
            for (int i = forLevel  ; i < childLevel; i++)
            {
                SearchString = " SELECT " + DIColumns.Area.AreaParentNId + " FROM " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaNId + " IN (" + SearchString + ")";
            }

            RetVal = "SELECT * FROM " + this.TablesName.Area + " WHERE " + DIColumns.Area.AreaNId + " IN ( " + SearchString + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns all areas for the given level and for the given parentNid
        /// </summary>
        /// <param name="parentNId"></param>
        /// <param name="parentLevel"></param>
        /// <param name="forLevel"></param>
        /// <returns></returns>        
        public string GetAreaByParentNIdFrmMySql(int parentNId, int parentLevel, int forLevel)
        {
            string RetVal = string.Empty;
            string SearchString = string.Empty;

            SearchString = parentNId.ToString();

            SearchString = " SELECT *  FROM " + this.TablesName.Area + "  AS A" + parentLevel + " WHERE A" + parentLevel + "." + DIColumns.Area.AreaParentNId + " =" + parentNId.ToString() + " and A" + (parentLevel - 1) + "." + DIColumns.Area.AreaNId + "=A" + parentLevel + "." + DIColumns.Area.AreaNId;

            for (int i = parentLevel + 1; i < forLevel; i++)
            {
                SearchString = " SELECT *  FROM " + this.TablesName.Area + "  AS A" + i + " WHERE Exists (" + SearchString + ") and A" + (i - 1) + "." + DIColumns.Area.AreaNId + "=A" + i + "." + DIColumns.Area.AreaNId;
            }

            RetVal = " SELECT * FROM " + this.TablesName.Area + " AS A" + (parentLevel - 1) + "   WHERE   Exists ( " + SearchString + ")";

            return RetVal;
        }


        /// <summary>Get  LevelNId, AreaLevel, AreaLevelName from AreaLevel for a given  AreaIds</summary>
        /// <param name="areaNIds">comma delimited areaIds</param>
        /// <returns>Sql Statement</returns>
        public string GetAreaLevel(string areaNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT AL." + DIColumns.Area_Level.LevelNId + ",AL." + DIColumns.Area_Level.AreaLevel + ",AL." + DIColumns.Area_Level.AreaLevelName);
            sbQuery.Append(" FROM " + this.TablesName.AreaLevel + " AS AL");

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Area + " AS A");
                sbQuery.Append(" WHERE AL." + DIColumns.Area_Level.AreaLevel + " = A." + DIColumns.Area.AreaLevel + " AND A." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get only those area level against whom area is present in the area table.
        /// </summary>
        /// <returns></returns>
        public string GetAreaLevel()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT DISTINCT AL." + DIColumns.Area_Level.LevelNId + ",AL." + DIColumns.Area_Level.AreaLevel + ",AL." + DIColumns.Area_Level.AreaLevelName);

                RetVal.Append(" FROM " + TablesName.Area + " A," + TablesName.AreaLevel + " AL");

                RetVal.Append(" WHERE AL." + DIColumns.Area_Level.AreaLevel + " = A." + DIColumns.Area.AreaLevel);

                RetVal.Append(" ORDER BY AL." + DIColumns.Area_Level.AreaLevel + " DESC");
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }

        #endregion

        #region "-- Area_Feature_Type Table --"

        public string GetAreaFeature()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT F." + DIColumns.Area_Feature_Type.FeatureTypeNId + ", F." + DIColumns.Area_Feature_Type.FeatureType;
            RetVal += " FROM " + this.TablesName.AreaFeatureType + " F ";

            return RetVal;

        }

        public string GetAreaFeatureByAreaNid(string Nids)
        {
            string Retval = string.Empty;
            Retval = this.GetAreaFeature() + ", " + this.TablesName.AreaMap + " A_M ";
            Retval += " WHERE A_M." + DIColumns.Area_Map.AreaNId + " in(" + Nids + ") AND A_M." + DIColumns.Area_Map.FeatureTypeNId + "=F." + DIColumns.Area_Map.FeatureTypeNId;
            Retval += " ORDER BY F.Feature_Type ";
            return Retval;
        }

        public string GetAreaFeatureByNid(string Nids)
        {
            string Retval = string.Empty;
            Retval = this.GetAreaFeature() + " WHERE F." + DIColumns.Area_Feature_Type.FeatureTypeNId + " in(" + Nids + ")";
            Retval += " ORDER BY F.Feature_Type ";
            return Retval;
        }

        public string GetAreaFeatureByFeatureType(string featureType)
        {
            string Retval = string.Empty;
            Retval = this.GetAreaFeature() + " WHERE F." + DIColumns.Area_Feature_Type.FeatureType + " IN(" + featureType + ")";

            Retval += " ORDER BY F.Feature_Type ";
            return Retval;
        }


        public string GetDuplicateAreaFeature(int areaNId, int featureNId)
        {
            string Retval = string.Empty;

            //Check Existence of the Area and the Feature in the area_map table
            Retval = "SELECT " + DIColumns.Area_Map.AreaNId + ", " + DIColumns.Area_Map.FeatureTypeNId
            + " FROM " + this.TablesName.AreaMap
            + " Where " + DIColumns.Area_Map.AreaNId + "=" + areaNId + " AND " + DIColumns.Area_Map.FeatureTypeNId + "=" + featureNId;
            return Retval;
        }




        #endregion

        #region "-- Area_Map Table --"

        private string GetAreaMap()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT " + DIColumns.Area_Map.AreaMapNId + "," + DIColumns.Area_Map.AreaNId + "," + DIColumns.Area_Map.LayerNId);
            sbQuery.Append("," + DIColumns.Area_Map.FeatureTypeNId + "," + DIColumns.Area_Map.FeatureLayer);
            sbQuery.Append(" FROM " + this.TablesName.AreaMap);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetAreaMapByAreaNIds(string areaNIds, bool ConsiderFeatureLayers)
        {
            string RetVal = this.GetAreaMap();

            if (areaNIds.Length > 0)
            {
                RetVal += " WHERE " + DIColumns.Area_Map.AreaNId + " IN (" + areaNIds + ")";
            }


            if (ConsiderFeatureLayers == false)
            {
                if (areaNIds.Length > 0)
                {
                    RetVal += " AND " + DIColumns.Area_Map.FeatureLayer + " = 0";

                }
                else
                {
                    RetVal += " WHERE " + DIColumns.Area_Map.FeatureLayer + " = 0";
                }
            }
            return RetVal;
        }

        public string GetAreaMapByLayerNIds(string layerNIds, bool ConsiderFeatureLayers)
        {
            string RetVal = this.GetAreaMap();

            if (layerNIds.Length > 0)
            {
                RetVal += " WHERE " + DIColumns.Area_Map.LayerNId + " IN (" + layerNIds + ")";
            }

            if (ConsiderFeatureLayers == false)
            {
                if (layerNIds.Length > 0)
                {
                    RetVal += " AND " + DIColumns.Area_Map.FeatureLayer + " = 0";

                }
                else
                {
                    RetVal += " WHERE " + DIColumns.Area_Map.FeatureLayer + " = 0";
                }
            }
            return RetVal;
        }

        public string GetLayerNIDsExcludeArea(int areaNId, string layerNIDs)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + DIColumns.Area_Map.LayerNId + "  FROM " + this.TablesName.AreaMap + "  WHERE " + DIColumns.Area_Map.AreaNId + "<> " + areaNId + " AND " + DIColumns.Area_Map.LayerNId + " in(" + layerNIDs + ")";
            return RetVal;
        }



        #endregion

        #region "-- Area_Map_Layer Table --"

        /// <summary>
        /// Get  LayerName, LayerNId, LayerSize, LayerType, MinX, MinY, MaxX, MaxY,[LayerShp, LayerShx, LayerDbf] from AreaMapLayer for a given  Filter Criteria
        /// </summary>
        /// <param name="MapFilterFieldType">None will get all layers</param>
        /// <param name="filterText">blank will get all layers</param>
        /// <param name="fieldSelection">Light or Heavy. Heavy will include shp, shx and dbf</param>
        /// <returns></returns>
        public string GetAreaMapLayer(MapFilterFieldType mapFilterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //DISTINCT Clause not valid with memo or oledb (blob) fields
            sbQuery.Append("SELECT AMM." + DIColumns.Area_Map_Metadata.LayerName + ",AML." + DIColumns.Area_Map_Layer.LayerNId + ",AML." + DIColumns.Area_Map_Layer.LayerSize + ",AML." + DIColumns.Area_Map_Layer.LayerType);
            sbQuery.Append(",AML." + DIColumns.Area_Map_Layer.MinX + ",AML." + DIColumns.Area_Map_Layer.MinY + ",AML." + DIColumns.Area_Map_Layer.MaxX + ",AML." + DIColumns.Area_Map_Layer.MaxY);
            sbQuery.Append(",AML." + DIColumns.Area_Map_Layer.StartDate + ",AML." + DIColumns.Area_Map_Layer.EndDate + ",AML." + DIColumns.Area_Map_Layer.UpdateTimestamp);
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",AML." + DIColumns.Area_Map_Layer.LayerShp + ",AML." + DIColumns.Area_Map_Layer.LayerShx + ",AML." + DIColumns.Area_Map_Layer.Layerdbf);
            }
            if (mapFilterFieldType == MapFilterFieldType.AreaNId || mapFilterFieldType == MapFilterFieldType.AreaLevel)
            {
                sbQuery.Append(",A." + DIColumns.Area.AreaLevel + ", A." + DIColumns.Area.AreaNId + ", A." + DIColumns.Area.AreaID + ", A." + DIColumns.Area.AreaBlock);
            }

            sbQuery.Append(" FROM " + this.TablesName.AreaMapLayer + " AS AML," + this.TablesName.AreaMapMetadata + " AS AMM");
            if (mapFilterFieldType == MapFilterFieldType.AreaNId || mapFilterFieldType == MapFilterFieldType.AreaLevel)
            {
                sbQuery.Append("," + this.TablesName.AreaMap + " AS AM," + this.TablesName.Area + " AS A");
            }


            sbQuery.Append(" WHERE AML." + DIColumns.Area_Map_Layer.LayerNId + " = AMM." + DIColumns.Area_Map_Metadata.LayerNId);
            if (mapFilterFieldType == MapFilterFieldType.AreaNId || mapFilterFieldType == MapFilterFieldType.AreaLevel)
            {
                sbQuery.Append(" AND AML." + DIColumns.Area_Map_Layer.LayerNId + " = AM." + DIColumns.Area_Map.LayerNId);
                sbQuery.Append(" AND AM." + DIColumns.Area_Map.AreaNId + " = A." + DIColumns.Area.AreaNId);
            }

            if (filterText.Length > 0)
            {
                switch (mapFilterFieldType)
                {
                    case MapFilterFieldType.None:
                        break;
                    case MapFilterFieldType.LayerNId:
                        sbQuery.Append(" AND AML." + DIColumns.Area_Map_Layer.LayerNId + " IN (" + filterText + ")");
                        break;
                    case MapFilterFieldType.LayerName:
                        sbQuery.Append(" AND AMM." + DIColumns.Area_Map_Metadata.LayerName + " IN (" + filterText + ")");
                        break;
                    case MapFilterFieldType.AreaNId:
                        sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " IN (" + filterText + ")");
                        break;
                    case MapFilterFieldType.AreaLevel:
                        sbQuery.Append(" AND A." + DIColumns.Area.AreaLevel + " IN (" + filterText + ")");
                        break;
                }
            }

            //*** Polygon(5)->PolyLine(3)->Points(1) Country(1)->State(2)->District(3)
            sbQuery.Append(" ORDER BY AML." + DIColumns.Area_Map_Layer.LayerType + " DESC");
            if (mapFilterFieldType == MapFilterFieldType.AreaNId || mapFilterFieldType == MapFilterFieldType.AreaLevel)
            {
                sbQuery.Append(",A." + DIColumns.Area.AreaLevel);
            }


            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns query to get area map layer
        /// </summary>
        /// <param name="layerNid">Comma separated layer nids and to get all map layer can be blank</param>
        /// <param name="fieldSelection"></param>
        /// <returns></returns>
        public string GetAreaMapLayerByNid(string layerNids, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;

            // SELECT
            RetVal = "Select AML." + DIColumns.Area_Map_Layer.LayerNId + ", AML." + DIColumns.Area_Map_Layer.LayerSize + ", AML." + DIColumns.Area_Map_Layer.LayerType + ", AML." + DIColumns.Area_Map_Layer.MinX + ", AML." + DIColumns.Area_Map_Layer.MinY + ", AML." + DIColumns.Area_Map_Layer.MaxX + ", AML." + DIColumns.Area_Map_Layer.MaxY + ", AML." + DIColumns.Area_Map_Layer.StartDate + ", AML." + DIColumns.Area_Map_Layer.EndDate + ", AML." + DIColumns.Area_Map_Layer.UpdateTimestamp + ", AMM." + DIColumns.Area_Map_Metadata.LayerName;
            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += ", AML." + DIColumns.Area_Map_Layer.LayerShp + ", AML." + DIColumns.Area_Map_Layer.LayerShx + ", AML." + DIColumns.Area_Map_Layer.Layerdbf;
            }

            // FROM
            RetVal += " FROM " + this.TablesName.AreaMapLayer + " AML, " + this.TablesName.AreaMapMetadata + " AMM ";

            // WHERE
            RetVal += " WHERE AMM." + DIColumns.Area_Map_Layer.LayerNId + " = AML." + DIColumns.Area_Map_Layer.LayerNId;
            if (!string.IsNullOrEmpty(layerNids))
                RetVal += " AND AML.Layer_NId in(" + layerNids + ")";

            // Return
            return RetVal;
        }

        /// <summary>
        /// Returns query to get area map layer without layer name
        /// </summary>
        /// <param name="layerNid">Comma separated layer nids and to get all map layer can be blank</param>
        /// <param name="fieldSelection"></param>
        /// <returns></returns>
        public string GetAreaMapLayer(string layerNids, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;

            // SELECT
            RetVal = "Select AML." + DIColumns.Area_Map_Layer.LayerNId + ", AML." + DIColumns.Area_Map_Layer.LayerSize + ", AML." + DIColumns.Area_Map_Layer.LayerType + ", AML." + DIColumns.Area_Map_Layer.MinX + ", AML." + DIColumns.Area_Map_Layer.MinY + ", AML." + DIColumns.Area_Map_Layer.MaxX + ", AML." + DIColumns.Area_Map_Layer.MaxY + ", AML." + DIColumns.Area_Map_Layer.StartDate + ", AML." + DIColumns.Area_Map_Layer.EndDate + ", AML." + DIColumns.Area_Map_Layer.UpdateTimestamp; ;
            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += ", AML." + DIColumns.Area_Map_Layer.LayerShp + ", AML." + DIColumns.Area_Map_Layer.LayerShx + ", AML." + DIColumns.Area_Map_Layer.Layerdbf;
            }

            // FROM
            RetVal += " FROM " + this.TablesName.AreaMapLayer + " AML ";

            // WHERE
            if (!string.IsNullOrEmpty(layerNids))
            {
                RetVal += " WHERE AML.Layer_NId in(" + layerNids + ")";
            }

            // Return
            return RetVal;
        }

        /// <summary>
        /// Returns sql query to get area map layer information with area
        /// </summary>
        /// <param name="filterType"></param>
        /// <param name="filterString"></param>
        /// <param name="fieldSelection"></param>
        /// <returns></returns>
        public string GetAreaMapLayerInfo(FilterFieldType filterType, string filterString, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT A." + DIColumns.Area.AreaGId + " ";
            if (fieldSelection == FieldSelection.Heavy)
            {

                RetVal += ", A." + DIColumns.Area.AreaID + ", A." + DIColumns.Area.AreaName + ", AM." + DIColumns.Area_Map_Layer.LayerNId
                    + ", AM." + DIColumns.Area_Map.AreaMapNId + ", AM." + DIColumns.Area_Map.AreaNId + ", AM." + DIColumns.Area_Map.FeatureLayer + ", AM." + DIColumns.Area_Map.FeatureTypeNId + " ";
            }

            RetVal += ", MET." + DIColumns.Area_Map_Metadata.LayerName + " ";

            if (fieldSelection == FieldSelection.Heavy)
            {
                RetVal += ", AML." + DIColumns.Area_Map_Layer.MetadataNId + ", AML." + DIColumns.Area_Map_Layer.UpdateTimestamp + ", MET." + DIColumns.Area_Map_Metadata.MetadataText + " ";
            }

            RetVal += " FROM ((" + this.TablesName.AreaMap + " AS AM INNER JOIN " + this.TablesName.AreaMapLayer + " AS AML "
                + " ON AM." + DIColumns.Area_Map.LayerNId + " = AML." + DIColumns.Area_Map_Layer.LayerNId + ") " +
                " LEFT JOIN " + this.TablesName.AreaMapMetadata + " AS MET ON AML." + DIColumns.Area_Map_Layer.LayerNId + " = MET." + DIColumns.Area_Map_Metadata.LayerNId + ") "
                + " INNER JOIN " + this.TablesName.Area + " AS A ON AM." + DIColumns.Area_Map.AreaNId + "= A." + DIColumns.Area.AreaNId + " ";

            // Where clause
            RetVal += " WHERE 1=1 ";

            if (filterType == FilterFieldType.NId)
            {
                RetVal += " AND A." + DIColumns.Area.AreaNId + " in(" + filterString + ")";
            }
            else if (filterType == FilterFieldType.LayerNid)
            {
                RetVal += " AND AM." + DIColumns.Area_Map.LayerNId + " in(" + filterString + ")";
            }
            else if (filterType == FilterFieldType.Level)
            {
                RetVal += " AND A." + DIColumns.Area.AreaLevel + " =" + filterString + " ";
            }
            else if (filterType == FilterFieldType.Search)
            {
                RetVal += " AND " + filterString + " ";
            }

            // Order By
            RetVal += " ORDER BY AM.Layer_NId ";
            return (RetVal);
        }

        #endregion

        #region "-- Area_Map_Metadata Table --"
        /// <summary>
        /// Get  NId, ParentNId, Name, GID, Global,ID from Area for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">None - gets all records
        /// <para>Applicable for NId, Name, Search, NIdNotIn, NameNotIn</para>
        /// </param>
        /// <param name="filterString">blank - gets all records 
        /// <para>For FilterFieldType "Search" include wild characters. e.g. '%Ind%' or '%Africa%'</para>
        /// </param>
        /// <param name="fieldSelection">Heavy will include metadata text</param>
        /// <returns></returns>
        public string GetAreaMapMetadata(FilterFieldType filterFieldType, string filterString, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area_Map_Metadata.MetadataNId + "," + DIColumns.Area_Map_Metadata.LayerNId + "," + DIColumns.Area_Map_Metadata.LayerName);

            //-- Include Metadata text only for heavy case
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + DIColumns.Area_Map_Metadata.MetadataText);
            }

            sbQuery.Append(" FROM " + this.TablesName.AreaMapMetadata);


            if (filterString.Length > 0 && filterFieldType != FilterFieldType.None)
            {
                 sbQuery.Append(" WHERE ");
           }

            if (filterString.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Area_Map_Metadata.LayerNId + " IN (" + filterString + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Area_Map_Metadata.LayerName + " IN (" + filterString + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterString);
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Area_Map_Metadata.LayerNId + " NOT IN (" + filterString + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Area_Map_Metadata.LayerName + " NOT IN (" + filterString + ")");
                        break;
                }
            }

            RetVal = sbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Get metadata from AreaMapMetadata table for a given  layerNIds
        /// </summary>
        /// <param name="layerNIds">layerNIds for which metadata is to be extracted</param>
        /// <returns></returns>
        public string GetAreaMapMetadata(string layerNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area_Map_Metadata.MetadataNId + "," + DIColumns.Area_Map_Metadata.LayerNId);
            sbQuery.Append("," + DIColumns.Area_Map_Metadata.LayerName + "," + DIColumns.Area_Map_Metadata.MetadataText);

            sbQuery.Append(" FROM " + this.TablesName.AreaMapMetadata);

            if (layerNIds.Trim().Length > 0)
            {
                sbQuery.Append(" WHERE " + DIColumns.Area_Map_Metadata.LayerNId + " IN (" + layerNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get metadata from AreaMapMetadata table for a given  layerName
        /// </summary>
        /// <param name="layerName">Layer Name for which metadata is to be extracted</param>
        /// <returns></returns>
        public string GetAreaMapMetadataByName(string layerName)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Area_Map_Metadata.MetadataNId + "," + DIColumns.Area_Map_Metadata.LayerNId);
            sbQuery.Append("," + DIColumns.Area_Map_Metadata.LayerName + "," + DIColumns.Area_Map_Metadata.MetadataText);

            sbQuery.Append(" FROM " + this.TablesName.AreaMapMetadata);

            if (layerName.Trim().Length > 0)
            {
                sbQuery.Append(" WHERE " + DIColumns.Area_Map_Metadata.LayerName + " ='" + layerName + "'");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        #endregion

        #endregion

        #endregion

    }
}
