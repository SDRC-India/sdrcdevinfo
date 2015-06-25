using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Data
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

        #region "-- Methods --"

        /// <summary>
        /// Get generic SELECT-FROM-WHERE sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericSelectFromWhereClause()
        {
            string RetVal = this.GetGenericSelectClause() + this.GetGenericFromClause() + this.GetGenericWhereClause();
            return RetVal;
        }

        /// <summary>
        /// Get generic SELECT sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericSelectClause()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            sbQuery.Append(" SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.FootNoteNId + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.DataDenominator + ",D." + DIColumns.Data.ICIUSOrder);
            sbQuery.Append(",I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal);
            sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);
            sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder);
            //sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValAge + ",SGV." + DIColumns.SubgroupVals.SubgroupValSex + ",SGV." + DIColumns.SubgroupVals.SubgroupValLocation + ",SGV." + DIColumns.SubgroupVals.SubgroupValOthers);
            sbQuery.Append(",T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod);
            sbQuery.Append(",A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaParentNId + ",A." + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaLevel + ",A." + DIColumns.Area.AreaGlobal);
            sbQuery.Append(",IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICOrder);

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get generic FROM sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericFromClause()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");
            sbQuery.Append("," + this.TablesName.TimePeriod + " AS T," + this.TablesName.Area + " AS A");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get generic WHERE sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericWhereClause()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);
            sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);
            sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            RetVal = sbQuery.ToString();

            return RetVal;
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
        /// Get the Query for Duplicate DataValues Records
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateDataValues()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT D." + DIColumns.Data.DataNId + ", I." + DIColumns.Indicator.IndicatorName + ", U."
                + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ", D." + DIColumns.Data.DataValue
                + ", IC." + DIColumns.IndicatorClassifications.ICName + ", T." + DIColumns.Timeperiods.TimePeriod + ", A."
                + DIColumns.Area.AreaName
                + " FROM " + this.TablesName.Data + " AS D, "
                + "(SELECT " + DIColumns.Data.IUSNId + "," + DIColumns.Data.TimePeriodNId + "," + DIColumns.Data.SourceNId + ","
                + DIColumns.Data.AreaNId + " FROM " + this.TablesName.Data + " GROUP BY " + DIColumns.Data.IUSNId + ","
                + DIColumns.Data.TimePeriodNId + "," + DIColumns.Data.SourceNId + "," + DIColumns.Data.AreaNId
                + " HAVING( COUNT(*) > 1 ) ) as D1,"
                + this.TablesName.IndicatorClassifications + " AS IC,"
                + this.TablesName.IndicatorUnitSubgroup + " AS IUS, "
                + this.TablesName.Indicator + " AS I, "
                + this.TablesName.Unit + " AS U,"
                + this.TablesName.SubgroupVals + " AS SGV, "
                + this.TablesName.TimePeriod + " AS T, "
                + this.TablesName.Area + " AS A"
                + " WHERE 1=1 AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId
                + " AND D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.UnitNId + "= U." + DIColumns.Unit.UnitNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "= SGV." + DIColumns.SubgroupVals.SubgroupValNId + " AND D."
                + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId
                + " = A." + DIColumns.Area.AreaNId
                + " AND D." + DIColumns.Data.IUSNId + " =D1." + DIColumns.Data.IUSNId + " AND D."
                + DIColumns.Data.AreaNId + "=D1." + DIColumns.Data.AreaNId + " AND D."
                + DIColumns.Data.SourceNId + "= D1." + DIColumns.Data.SourceNId
                + " ORDER BY D." + DIColumns.Data.DataValue + ", I." + DIColumns.Indicator.IndicatorName + ", U."
                + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal;

            return RetVal;
        }


        /// <summary>
        /// Gets records form data table based on given IUSNId, TimePeriodNId, AreaNId, SourceNId
        /// <para>Atleast one filter criteria must be set. Others may be set blank</para>
        /// </summary>
        /// <param name="IUSNIds">comma delimited IUSNIds which may be blank</param>
        /// <param name="timePeriodNIds">comma delimited TimePeriodNIds which may be blank</param>
        /// <param name="areaNIds">comma delimited AreaNIds which may be blank</param>
        /// <param name="sourceNIds">comma delimited SourceNIds which may be blank</param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <returns></returns>
        public string GetDataByIUSTimePeriodAreaSource(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            RetVal = GetDataNIDs(IUSNIds, timePeriodNIds, areaNIds, sourceNIds, true, fieldSelection, false);
            return RetVal;
        }

        public string GetDataByIUSTimePeriodAreaSource(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, FieldSelection fieldSelection, bool isPlanedValue)
        {
            string RetVal = string.Empty;
            RetVal = GetDataNIDs(IUSNIds, timePeriodNIds, areaNIds, sourceNIds, true, fieldSelection, false, isPlanedValue);
            return RetVal;
        }
        /// <summary>
        /// Gets records form data table based on given IUSNId, TimePeriodNId, AreaNId, SourceNId
        /// <para>Atleast one filter criteria must be set. Others may be set blank</para>
        /// </summary>
        /// <param name="IUSNIds">comma delimited IUSNIds which may be blank</param>
        /// <param name="timePeriodNIds">comma delimited TimePeriodNIds which may be blank</param>
        /// <param name="areaNIds">comma delimited AreaNIds which may be blank</param>
        /// <param name="sourceNIds">comma delimited SourceNIds which may be blank</param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <returns></returns>
        public string GetDataByIUSTimePeriodAreaSourceByRule(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, FieldSelection fieldSelection, int areaLevel)
        {
            string RetVal = string.Empty;

            RetVal = GetDataNIDs(IUSNIds, timePeriodNIds, areaNIds, sourceNIds, true, fieldSelection, false, areaLevel);
            return RetVal;
        }
        /// <summary>
        /// Gets records form data table based on given IUSNId, TimePeriodNId, AreaNId, SourceNId
        /// <para>Atleast one filter criteria must be set. Others may be set blank</para>
        /// </summary>
        /// <param name="IUSNIds">comma delimited IUSNIds which may be blank</param>
        /// <param name="timePeriodNIds">comma delimited TimePeriodNIds which may be blank</param>
        /// <param name="areaNIds">comma delimited AreaNIds which may be blank</param>
        /// <param name="sourceNIds">comma delimited SourceNIds which may be blank</param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <returns></returns>
        public string GetDataNIdByIUSTimePeriodAreaSource(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds)
        {
            string RetVal = string.Empty;
            RetVal = GetDataNIDs(IUSNIds, timePeriodNIds, areaNIds, sourceNIds, true, FieldSelection.NId, true);
            return RetVal;
        }

        /// <summary>
        /// Gets records form data table based on User selection
        /// </summary>
        /// <param name="UserSelection">an instance of User selection object</param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <returns></returns>
        public string GetDataByIUSTimePeriodAreaSource(UserSelection.UserSelection UserSelection, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            RetVal = GetDataNIDs(UserSelection.IndicatorNIds, UserSelection.TimePeriodNIds, UserSelection.AreaNIds, UserSelection.SourceNIds, UserSelection.ShowIUS, fieldSelection, false);
            return RetVal;
        }

        /// <summary>
        /// Gets records form data table based on User selection
        /// </summary>
        /// <param name="UserSelection">an instance of User selection object</param>
        /// <param name="DIServerType">Server type required for database specific syntax like concat etc.</param>
        /// <param name="dataNIds">
        /// Comma delimited datanids which may be blank. 
        /// If datanid is provided then filter is applied on the basis datanids instead of indicatornid + timeperiodnids + areanids
        /// </param>
        /// <returns>Sql query text</returns>
        public string GetDataNIDByIUSTimePeriodAreaSource(UserSelection.UserSelection UserSelection, DI_LibDAL.Connection.DIServerType DIServerType, string dataNIds)
        {
            string RetVal = string.Empty;
            RetVal = GetDataIUSSourceNIDByUserSelection(UserSelection, FieldSelection.NId, DIServerType, dataNIds);
            return RetVal;
        }

        /// <summary>
        /// Gets records form data table based on User selection
        /// </summary>
        ///<param name="UserSelection">an instance of User selection object</param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <param name="DIServerType">Server type required for database specific syntax like concat etc.</param>
        /// <param name="dataNIds">
        /// Comma delimited datanids which may be blank. 
        /// If datanid is provided then filter is applied on the basis datanids instead of indicatornid + timeperiodnids + areanids
        /// </param>
        /// <returns></returns>
        private string GetDataIUSSourceNIDByUserSelection(UserSelection.UserSelection UserSelection, FieldSelection fieldSelection, DI_LibDAL.Connection.DIServerType DIServerType, string dataNIds)
        {

            // 
            // --------- sDataNIDs (Length > 0 then apply Filters) ---------
            // 
            // -- sDataNIDs will have comma seperated Data NIDs only when this function is being used to Apply Filters on the already created Data View
            // -- In such a case sDataNIDs will hold all the DataNIDs of the created DataView
            // 
            // -- 1. Will be used with Filters only
            // -- 2. If sDataNIDs is not blank then it means that this function is being used to set Filters
            // -- 3. TIME PERIOD Table - Will require a Check for MRD filter also - In this Case Time Period Table will be used
            // -- 4. In this Case no Filters on Area, Time and Indicator
            // 
            // -- SPECIAL CASE
            // -- With too many DataNIDs, the Query might fail - Maximum limit set to 60,000 length
            // -- In such a scenario, sDataNIDs filter will not be used and the Filters for Area, Time and Indicator will be used
            // -- Also MRD will be applied if applicable
            // 

            string RetVal = string.Empty;

            string indicatorNIds = UserSelection.IndicatorNIds;
            string timePeriodNIds = UserSelection.TimePeriodNIds;
            string areaNIds = UserSelection.AreaNIds;
            string sourceNIds = UserSelection.SourceNIds;
            bool showIUS = UserSelection.ShowIUS;
            bool UseIUSTable = false;
            int iDataNIDLen = dataNIds.Length;
            int iMaxDataNIdLen = 60000;

            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ", D." + DIColumns.Data.IUSNId + ", D." + DIColumns.Data.TimePeriodNId + ", D." + DIColumns.Data.AreaNId + ", D." + DIColumns.Data.SourceNId);

                    // Use TimePeriod Table (Check comments on top - MRD case)
                    if (iDataNIDLen > 0 && UserSelection.DataViewFilters.MostRecentData)
                    {
                        // Check comments on top - MRD case
                        sbQuery.Append(", T." + DIColumns.Timeperiods.TimePeriod);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                    sbQuery.Append("SELECT IUS." + DIColumns.Data.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICNId + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericSelectClause());
                    break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");


                    // Filters on - Unit and Subgroup
                    if (UserSelection.DataViewFilters.DeletedSubgroupNIds.Length > 0 || UserSelection.DataViewFilters.DeletedUnitNIds.Length > 0)
                    {
                        // -- DataView - If Filtered by Unit or Subgroup
                        // -- Use IUS Table in this case 
                        UseIUSTable = true;
                    }

                    // Use IUS Table
                    if (UseIUSTable)
                    {
                        sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
                    }
                    // Use Time Period Table
                    if (iDataNIDLen > 0 && UserSelection.DataViewFilters.MostRecentData)
                    {
                        // Check comments on top
                        sbQuery.Append(", " + this.TablesName.TimePeriod + " AS T ");
                    }

                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");

                    if (UseIUSTable)
                    {
                        // Join between IUS and Data Table
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                    }

                    // Use Time Period Table - MRD Filter
                    if (iDataNIDLen > 0 && UserSelection.DataViewFilters.MostRecentData)
                    {
                        // Check comments on top  - MRD Filter
                        sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + "=T." + DIColumns.Timeperiods.TimePeriodNId);
                    }


                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    break;
                default:
                    break;
            }

            // Filters on Area, Indicator and Time Period
            if (iDataNIDLen == 0 || iDataNIDLen > iMaxDataNIdLen)
            {
                // Area
                if (areaNIds.Trim().Length > 0)
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");
                }

                // Indicator
                if (indicatorNIds.Trim().Length > 0)
                {
                    if (showIUS == false)
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNIds + ") ");
                    }
                    else
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + indicatorNIds + ")");
                    }
                }

                // Time Period
                if (timePeriodNIds.Trim().Length > 0)
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");
                }

                // Source
                if (sourceNIds.Trim().Length > 0)
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");
                }
            }
            else
            {
                // IF the length of sDataNIDs > 0 then apply filter on DataNIDs
                sbQuery.Append(" AND D." + DIColumns.Data.DataNId + " IN (" + dataNIds + ")");
            }


            // --------------------------------------------- 
            // --------- Filters - DataView ---------  Exclude Deleted Data Point filters as they will be part of main dataview
            // --------------------------------------------- 
            if (UserSelection.DataViewFilters.MostRecentData == true)
            {
                // For MRD case Deleted records should be discarded ie filter clause for deleted record should be considered
                sbQuery.Append(UserSelection.DataViewFilters.SQL_GetDataViewFilters(DIServerType, false));
            }
            else
            {
                // For other case Deleted records should be part of dataview ie filter clause for deleted record should be excluded
                sbQuery.Append(UserSelection.DataViewFilters.SQL_GetDataViewFilters(DIServerType, true));
            }


            RetVal = sbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Gets DataNIds form data table based on Indiactor, Timeperiod, Area and Source selection
        /// </summary>
        /// <param name="IUSNIds">Comma deleimited IUSNIds / IndicatorNIds based on ShowIUS parameter</param>
        /// <param name="timePeriodNIds">Comma deleimited TimePeriodNIds</param>
        /// <param name="areaNIds">Comma deleimited AreaNIds</param>
        /// <param name="sourceNIds">Comma deleimited SourceNIds</param>
        /// <param name="ShowIUS"></param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <param name="dataNIdOnly">select DataNId, IUSNId, SourceNId columns from data table only</param>
        /// <returns></returns>
        public string GetDataNIDs(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, bool showIUS, FieldSelection fieldSelection, bool dataNIdOnly)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    if (dataNIdOnly)
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.SourceNId);
                    }
                    else
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                    sbQuery.Append("SELECT D." + DIColumns.Data.IUSNId + ",IUS." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",D." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder);
                    break;
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericSelectClause());
                    break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
                    if (showIUS == false)
                    {
                        // -- Use IUS Table in this case 
                        sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
                    }
                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");
                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    break;
                default:
                    break;
            }

            // Area
            if (areaNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");

            // Indicator
            if (IUSNIds.Trim().Length > 0)
            {
                if (showIUS == false)
                {
                    // -- Use IUS Table in this case 
                    if (fieldSelection == FieldSelection.NId)
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                    }
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IUSNIds + ") ");
                }
                else
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ")");
                }
            }

            // Time Period
            if (timePeriodNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");

            // Source
            if (sourceNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;

        }


        /// <summary>
        /// Gets DataNIds form data table based on Indiactor, Timeperiod, Area and Source selection
        /// </summary>
        /// <param name="IUSNIds">Comma deleimited IUSNIds / IndicatorNIds based on ShowIUS parameter</param>
        /// <param name="timePeriodNIds">Comma deleimited TimePeriodNIds</param>
        /// <param name="areaNIds">Comma deleimited AreaNIds</param>
        /// <param name="sourceNIds">Comma deleimited SourceNIds</param>
        /// <param name="ShowIUS"></param>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <param name="dataNIdOnly">select DataNId, IUSNId, SourceNId columns from data table only</param>
        /// <param name="isPlanedValue"></param>
        /// <returns></returns>
        public string GetDataNIDs(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, bool showIUS, FieldSelection fieldSelection, bool dataNIdOnly, bool isPlanedValue)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    if (dataNIdOnly)
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.SourceNId);
                    }
                    else
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                    sbQuery.Append("SELECT D." + DIColumns.Data.IUSNId + ",IUS." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",D." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder);
                    break;
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericSelectClause());
                    break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
                    if (showIUS == false)
                    {
                        // -- Use IUS Table in this case 
                        sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
                    }
                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");
                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    break;
                default:
                    break;
            }

            // Area
            if (areaNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");

            // Indicator
            if (IUSNIds.Trim().Length > 0)
            {
                if (showIUS == false)
                {
                    // -- Use IUS Table in this case 
                    if (fieldSelection == FieldSelection.NId)
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                    }
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IUSNIds + ") ");
                }
                else
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ")");
                }
            }

            // Time Period
            if (timePeriodNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");

            // Source
            if (sourceNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");

            sbQuery.Append(" AND D." + DIColumns.Data.IsPlannedValue + "=" + Convert.ToInt16(isPlanedValue));

            RetVal = sbQuery.ToString();

            return RetVal;

        }
        public string GetDataNIDs(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, bool showIUS, FieldSelection fieldSelection, bool dataNIdOnly, int areaLevel)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            sbQuery.Append("SELECT ");

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    if (dataNIdOnly)
                    {
                        sbQuery.Append(" D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.SourceNId);
                    }
                    else
                    {
                        sbQuery.Append(" D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.DataValue + " as " + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append(" I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.DataValue + " as " + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                    sbQuery.Append(" D." + DIColumns.Data.IUSNId + ",IUS." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",D." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.DataValue + " as " + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder);
                    break;
                case FieldSelection.Heavy:
                    sbQuery.Remove(0, sbQuery.Length);
                    sbQuery.Append(this.GetGenericSelectClause());
                    break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
                    if (showIUS == false)
                    {
                        // -- Use IUS Table in this case 
                        sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
                    }
                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");
                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    break;
                default:
                    break;
            }

            // Area
            if (areaNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");

            // Indicator
            if (IUSNIds.Trim().Length > 0)
            {
                if (showIUS == false)
                {
                    // -- Use IUS Table in this case 
                    if (fieldSelection == FieldSelection.NId)
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                    }
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IUSNIds + ") ");
                }
                else
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ")");
                }
            }

            // Time Period
            if (timePeriodNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");

            // Source
            if (sourceNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");

            //specify area level
            if (areaLevel > 0)
                sbQuery.Append(" AND A." + DIColumns.Area.AreaLevel + "=" + areaLevel);

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        public string GetDataNIDsWFootNotes(string IUSNIds, string timePeriodNIds, string areaNIds, string sourceNIds, bool showIUS, FieldSelection fieldSelection, bool dataNIdOnly)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    if (dataNIdOnly)
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.SourceNId);
                    }
                    else
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                    sbQuery.Append("SELECT D." + DIColumns.Data.IUSNId + ",IUS." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",D." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName + ",A." + DIColumns.Area.AreaID + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder + ",F." + DIColumns.FootNotes.FootNote);
                    break;
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericSelectClause());
                    sbQuery.Append(",F." + DIColumns.FootNotes.FootNote);
                    break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
                    if (showIUS == false)
                    {
                        // -- Use IUS Table in this case 
                        sbQuery.Append(", " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ");
                    }
                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    sbQuery.Append("," + this.TablesName.FootNote + " AS F");
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");
                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    sbQuery.Append(" AND D." + DIColumns.Data.FootNoteNId + " = F." + DIColumns.FootNotes.FootNoteNId);
                    break;
                default:
                    break;
            }

            // Area
            if (areaNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");

            // Indicator
            if (IUSNIds.Trim().Length > 0)
            {
                if (showIUS == false)
                {
                    // -- Use IUS Table in this case 
                    if (fieldSelection == FieldSelection.NId)
                    {
                        sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                    }
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IUSNIds + ") ");
                }
                else
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ")");
                }
            }

            // Time Period
            if (timePeriodNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");

            // Source
            if (sourceNIds.Trim().Length > 0)
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Use DIDataView class to get data
        /// Returns SQL Query statement for generating presentation data based on user selection.
        /// </summary>
        /// <param name="UserSelection">an instance of User selection object</param>
        /// <param name="DIServerType">Server type required for database specific syntax like concat etc.</param>
        /// <returns></returns>
        [Obsolete]
        public string GetPresentationData(UserSelection.UserSelection UserSelection, DI_LibDAL.Connection.DIServerType DIServerType)
        {


            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            string sDataViewBasicQuery = string.Empty;

            // Get DataView Basic Query
            sDataViewBasicQuery = GetGenericSelectFromWhereClause();


            // Indicator Filters
            if (UserSelection.IndicatorNIds.Length > 0)
            {
                if (UserSelection.ShowIUS == true)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + UserSelection.IndicatorNIds + ")");
                }
                else
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + UserSelection.IndicatorNIds + ") ");
                }
            }
            // Time Period Filters
            if (UserSelection.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + UserSelection.TimePeriodNIds + ")");
            }
            // Area Filters
            if (UserSelection.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " IN (" + UserSelection.AreaNIds + ")");
            }

            // Dataview Filters
            sbQuery.Append(UserSelection.DataViewFilters.SQL_GetDataViewFilters(DIServerType, false));

            RetVal = sDataViewBasicQuery + sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Gets records form data table based on dataNIds
        /// </summary>
        /// <param name="dataNIds">Comma delimited datanids</param>
        /// <returns></returns>
        public string GetDataViewDataByDataNIDs(string dataNIds)
        {
            // Get DataView Basic Query
            string RetVal = GetGenericSelectFromWhereClause();

            //Set DataNId Filter clause
            if (dataNIds.Length > 0)
            {
                RetVal += " AND D." + DIColumns.Data.DataNId + " IN (" + dataNIds + ")";
            }

            return RetVal;
        }


        /// <summary>
        /// Get DataNIds based on GIDs / Textual values for IndicatorGId,UnitGId,SubgroupValGId,TimePeriod,AreaID,ICName
        /// </summary>
        /// <param name="dataGIds">IndicatorGId,UnitGId,SubgroupValGId,TimePeriod,AreaID,ICName</param>
        /// <param name="DIServerType">Server type required for database specific syntax like concat etc.</param>
        /// <returns></returns>
        internal string GetDataPointNIds(string dataGIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause
            sbQuery.Append("SELECT " + DIColumns.Data.DataNId);

            // FROM Clause
            sbQuery.Append(this.GetGenericFromClause());

            // WHERE Clause
            sbQuery.Append(this.GetGenericWhereClause());


            //-- Filters
            sbQuery.Append(" AND (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaID + ",IC." + DIColumns.IndicatorClassifications.ICName, ",", Delimiter.TEXT_SEPARATOR, DIServerType) + ") IN (" + dataGIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns a query to get DataNid for unmatched IUS.
        /// </summary>
        /// <returns></returns>
        public string GetDataNidForUnmatchedIUS()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + DIColumns.Data.DataNId + " FROM " + this.TablesName.Data + " where " + DIColumns.Data.DataNId + " not in "
                + " ( SELECT " + DIColumns.Data.DataNId + " FROM " + this.TablesName.Data + " AS D INNER JOIN "
                + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns a query to get DataNid for unmatched areas
        /// </summary>
        /// <returns></returns>
        public string GetDataNidForUnmatchedArea()
        {
            string RetVal = string.Empty;

            RetVal = "Select Distinct " + DIColumns.Data.DataNId + "  FROM " + this.TablesName.Data + " where " + DIColumns.Data.DataNId + "  not in "
                + " ( SELECT " + DIColumns.Data.DataNId + "  FROM " + this.TablesName.Data + " AS D INNER JOIN "
                + this.TablesName.Area + " AS A ON D." + DIColumns.Area.AreaNId + "  = A." + DIColumns.Area.AreaNId + ")";

            return RetVal;

        }

        /// <summary>
        /// Return Query with Fields IndicatorClassificationsIUS.IUSNId, Indicator.IndicatorName ,Unit.UnitName, SubgroupVals.SubgroupVal,Indicator_Unit_Subgroup.MinValue, Indicator_Unit_Subgroup.MaxValue,IndicatorClassifications.ICName,DIColumns.Data.DataNId,Data.DataValue,Area.AreaID,Area.AreaName,Timeperiods.TimePeriod
        /// </summary>
        /// <returns></returns>
        public string GetValuesRangeCheck()
        {
            string RetVal = String.Empty;

            RetVal = "SELECT IUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + ", I."
    + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S."
    + DIColumns.SubgroupVals.SubgroupVal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS."
    + DIColumns.Indicator_Unit_Subgroup.MaxValue + ", IC." + DIColumns.IndicatorClassifications.ICName + ",  D."
    + DIColumns.Data.DataNId + ", D." + DIColumns.Data.DataValue + ",A."
    + DIColumns.Area.AreaID + ",A." + DIColumns.Area.AreaName + ",T." + DIColumns.Timeperiods.TimePeriod;

            RetVal += " FROM ( ((((( " + this.TablesName.Data + " AS D";

            RetVal += " INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON D."
     + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " )";

            RetVal += " INNER JOIN " + this.TablesName.Indicator + " AS I ON IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + ")";

            RetVal += " INNER JOIN " + this.TablesName.Unit + " AS U ON IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + ")";

            RetVal += " INNER JOIN " + this.TablesName.SubgroupVals + " AS S ON IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "= S." + DIColumns.SubgroupVals.SubgroupValNId + ") ";

            RetVal += " INNER JOIN " + this.TablesName.IndicatorClassifications + " AS IC ON D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId + ")";

            RetVal += " INNER JOIN " + this.TablesName.Area + " AS A ON  D." + DIColumns.Data.AreaNId + "= A." + DIColumns.Area.AreaNId + ")";

            RetVal += " INNER JOIN " + this.TablesName.TimePeriod + " T ON D." + DIColumns.Data.TimePeriodNId + "= T." + DIColumns.Timeperiods.TimePeriodNId;

            //--  selection criteria has been changed
            RetVal += " WHERE ((((Val([" + DIColumns.Data.DataValue + "])) >= IIf(IsNull([IUS]![" + DIColumns.Indicator_Unit_Subgroup.MinValue + "]), Val([" + DIColumns.Data.DataValue + "]), [IUS]![" + DIColumns.Indicator_Unit_Subgroup.MinValue + "]) And (Val([" + DIColumns.Data.DataValue + "])) <= IIf(IsNull([IUS]![" + DIColumns.Indicator_Unit_Subgroup.MaxValue + "]), Val([" + DIColumns.Data.DataValue + "]) , [IUS]![" + DIColumns.Indicator_Unit_Subgroup.MaxValue + "])) = False))";

            RetVal += " AND NOT ( IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + "=0 AND IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue + " = 0) ";
            RetVal += " ORDER BY IUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + ", T." + DIColumns.Timeperiods.TimePeriod;

            return RetVal;
        }



        /// <summary>
        /// Get DataValue List
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetDataValuesByAreaLevel(int level)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT D." + DIColumns.Data.DataValue + " FROM "
                + this.TablesName.Data + " D, "
                + this.TablesName.Area + " A "
                + " WHERE A." + DIColumns.Area.AreaNId + "= D." + DIColumns.Data.AreaNId
                + " AND A." + DIColumns.Area.AreaLevel + " = " + level
                + " AND D." + DIColumns.Data.DataValue + " IS NOT NULL";

            return RetVal;
        }

        /// <summary>
        /// Get Query To Return DataValues for Selected NIDs
        /// </summary>
        /// <param name="indicatorNids"></param>
        /// <param name="areaID"></param>
        /// <param name="timeNid"></param>
        /// <param name="sourceNid"></param>
        /// <param name="showIUS"></param>
        /// <param name="sort"></param>
        /// <returns> String </returns>
        /// <remarks>Userd In DataEntry Module</remarks>
        public string GetSelectedDataValues(string indicatorNids, string areaID, string timeNid, string sourceNid, bool showIUS, string sort) // ERROR: Unsupported modifier : In, Optional string sSort) 
        {

            string RetVal = string.Empty;

            RetVal = "Select Distinct T." + DIColumns.Timeperiods.TimePeriod + ", A." + DIColumns.Area.AreaID + ", A."
                + DIColumns.Area.AreaName + " ,SG." + DIColumns.SubgroupVals.SubgroupVal + " , IC."
                + DIColumns.IndicatorClassifications.ICName + "," + "I." + DIColumns.Indicator.IndicatorName
                + ",U." + DIColumns.Unit.UnitName + ", T." + DIColumns.Timeperiods.TimePeriodNId + ", A." + DIColumns.Area.AreaNId
                + " , I." + DIColumns.Indicator.IndicatorNId + "," + DIColumns.IndicatorClassifications.ICNId + " , IUG."
                + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", U." + DIColumns.Unit.UnitGId + " ," + " SG."
                + DIColumns.SubgroupVals.SubgroupValGId + ", I." + DIColumns.Indicator.IndicatorGId + ",I."
                + DIColumns.Indicator.IndicatorGlobal + ",U." + DIColumns.Unit.UnitGlobal + ",SG." + DIColumns.SubgroupVals.SubgroupValGlobal
                + ",A." + DIColumns.Area.AreaGlobal + ",IUG." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUG."
                + DIColumns.Indicator_Unit_Subgroup.MaxValue

                + " From " + this.TablesName.Indicator + " I, " + this.TablesName.Unit + " U, " + this.TablesName.SubgroupVals + " SG, "
                + this.TablesName.IndicatorUnitSubgroup + " IUG, " + this.TablesName.Area + " A, " + this.TablesName.TimePeriod + " T, "
                + this.TablesName.IndicatorClassifications + " IC" + " WHERE ";

            if (showIUS)
            {
                RetVal += " IUG." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in(" + indicatorNids + ")";
            }
            else
            {
                RetVal += " I." + DIColumns.Indicator.IndicatorNId + " in(" + indicatorNids + ")";
            }

            RetVal += " AND IUG." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + " and U." + DIColumns.Unit.UnitNId + "= IUG." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " AND "
            + " SG." + DIColumns.SubgroupVals.SubgroupValNId + " = IUG." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " AND A." + DIColumns.Area.AreaNId + " in (" + areaID + ") and "
            + " T." + DIColumns.Timeperiods.TimePeriodNId + " in(" + timeNid + ") and IC." + DIColumns.IndicatorClassifications.ICNId + " in (" + sourceNid + ")";

            //if( sort.Length == 0 )
            //    RetVal += "Order by I." + DIColumns.Indicator.IndicatorName;
            //else
            //    RetVal += "Order by " + sort;


            return RetVal;

        }

        /// <summary>
        /// Return Query For Get DataValue, Denominator for IUSNid, Areanid,timenids,sourcesnids
        /// </summary>
        /// <param name="iusNids"></param>
        /// <param name="areaNids"></param>
        /// <param name="timeNids"></param>
        /// <param name="sourceNids"></param>
        /// <param name="showIUS"></param>
        /// <returns>string</returns>
        /// <remarks>Userd In DataEntry Module</remarks>
        public string GetDataWNidsForIndicatorAreaTimePeriodSources(string iusNids, string areaNids, string timeNids, string sourceNids, bool showIUS, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (showIUS)
            {
                RetVal = "Select Distinct ";

                if (serverType == DIServerType.MsAccess)
                {
                    RetVal += DIColumns.Data.DataValue;
                }
                else
                {
                    RetVal += DIColumns.Data.DataValue + "," + DIColumns.Data.TextualDataValue + "," + DIColumns.Data.IsTextualData;
                }

                RetVal += "," + DIColumns.Data.DataDenominator + "," + DIColumns.Data.IUSNId
                    + "," + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Area.AreaNId + "," + DIColumns.FootNotes.FootNoteNId + ","
                    + DIColumns.Data.SourceNId + "," + DIColumns.Data.DataNId
                    + "," + DIColumns.Data.IsPlannedValue + "," + DIColumns.Data.ConfidenceIntervalUpper + "," + DIColumns.Data.ConfidenceIntervalLower
                    + " From " + this.TablesName.Data
                    + " Where " + DIColumns.Data.IUSNId + " in(" + iusNids + ") and " + DIColumns.Data.TimePeriodNId
                    + " in(" + timeNids + ") and " + DIColumns.Data.AreaNId + " in(" + areaNids + ") and " + DIColumns.Data.SourceNId
                    + " in(" + sourceNids + ")";
            }
            else
            {
                RetVal = "Select Distinct ";

                if (serverType == DIServerType.MsAccess)
                {
                    RetVal += DIColumns.Data.DataValue;
                }
                else
                {
                    RetVal += DIColumns.Data.DataValue + "," + DIColumns.Data.TextualDataValue + "," + DIColumns.Data.IsTextualData;
                }

                RetVal += "," + DIColumns.Data.DataDenominator + ", D."
                    + DIColumns.Data.IUSNId + "," + DIColumns.Data.TimePeriodNId + "," + DIColumns.Data.AreaNId + ","
                    + DIColumns.Data.FootNoteNId + "," + DIColumns.Data.SourceNId + "," + DIColumns.Data.DataNId
                     + "," + DIColumns.Data.IsPlannedValue + "," + DIColumns.Data.ConfidenceIntervalUpper + "," + DIColumns.Data.ConfidenceIntervalLower
                    + " From "
                    + this.TablesName.Data + " D, " + this.TablesName.IndicatorUnitSubgroup + " IUS"
                    + " Where IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + iusNids + ") AND " + " IUS."
                    + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND " + DIColumns.Data.TimePeriodNId
                    + " in(" + timeNids + ") AND " + DIColumns.Data.AreaNId + " in(" + areaNids + ") AND " + DIColumns.Data.SourceNId
                    + " in(" + sourceNids + ")";
            }

            return RetVal;

        }

        /// <summary>
        /// Return query for Distinct IUS and Indicator NIds
        /// </summary>
        /// <returns></returns>
        public string GetDistinctNIds()
        {
            string Retval = string.Empty;
            try
            {
                Retval = "SELECT DISTINCT D." + DIColumns.Data.IUSNId + ", D." + DIColumns.Data.IndicatorNId + " FROM " + this.TablesName.Data + " D";
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Return Query for auto selected NIds 
        /// </summary>
        /// <param name="showIUS"></param>
        /// <param name="indicatorNIds"></param>
        /// <param name="areaNIds"></param>
        /// <param name="timePeriodsNIds"></param>
        /// <param name="sourceNIds"></param>
        /// <returns></returns>
        public string GetDataWAutoSelectedNIDs(bool showIUS, string indicatorNIds, string areaNIds, string timePeriodsNIds, string sourceNIds)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                StringBuilder sbWhereClause = new StringBuilder();

                sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + " ,D." + DIColumns.Data.DataValue + " ,D." + DIColumns.Data.IndicatorNId + " ,D." + DIColumns.Data.AreaNId + " ,D." + DIColumns.Data.TimePeriodNId + " ,D." + DIColumns.Data.SourceNId + " ,D." + DIColumns.Data.IUSNId + " ,A." + DIColumns.Area.AreaLevel);
                sbQuery.Append(" FROM " + this.TablesName.Data + " D," + this.TablesName.Area + " A");



                if (showIUS && !string.IsNullOrEmpty(indicatorNIds))
                {
                    if (sbWhereClause.Length > 0)
                    {
                        sbWhereClause.Append(" AND ");
                    }
                    sbWhereClause.Append(" D." + DIColumns.Data.IUSNId + " IN (" + indicatorNIds + ") ");
                }

                if (!showIUS && !string.IsNullOrEmpty(indicatorNIds))
                {
                    if (sbWhereClause.Length > 0)
                    {
                        sbWhereClause.Append(" AND ");
                    }
                    sbWhereClause.Append("  D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNIds + ") ");
                }

                if (!string.IsNullOrEmpty(areaNIds))
                {
                    if (sbWhereClause.Length > 0)
                    {
                        sbWhereClause.Append(" AND ");
                    }
                    sbWhereClause.Append(" D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ");
                }

                if (!string.IsNullOrEmpty(timePeriodsNIds))
                {
                    if (sbWhereClause.Length > 0)
                    {
                        sbWhereClause.Append(" AND ");
                    }
                    sbWhereClause.Append("  D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodsNIds + ") ");
                }

                if (!string.IsNullOrEmpty(sourceNIds))
                {
                    if (sbWhereClause.Length > 0)
                    {
                        sbWhereClause.Append(" AND ");
                    }
                    sbWhereClause.Append(" D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ") ");
                }

                sbQuery.Append(" WHERE A." + DIColumns.Area.AreaNId + "=D." + DIColumns.Data.AreaNId);

                if (sbWhereClause.Length > 0)
                {
                    sbQuery.Append(" AND " + sbWhereClause.ToString());
                }

                Retval = sbQuery.ToString();

            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }


        /// <summary>
        /// Query return number of records in the data table.
        /// </summary>
        /// <returns></returns>
        public string GetRecordCount()
        {
            string Retval = string.Empty;
            try
            {
                Retval = "SELECT COUNT(" + DIColumns.Data.DataNId + ") FROM " + this.TablesName.Data;
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Get auto selected AreaNId, DataNId, IUSNId, TimeperiodNId and TimePeriod
        /// </summary>
        /// <param name="userSelection"></param>
        /// <returns></returns>
        public string GetAutoSelectedIUSTimePeriodArea(UserSelection.UserSelection userSelection)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.TimePeriodNId;
                RetVal += ",T." + DIColumns.Timeperiods.TimePeriod;

                RetVal += " FROM " + this.TablesName.Data + " D," + this.TablesName.TimePeriod + " T";
                RetVal += " WHERE D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId;

                if (!string.IsNullOrEmpty(userSelection.IndicatorNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.IUSNId + " IN (" + userSelection.IndicatorNIds + ")";
                }

                if (!string.IsNullOrEmpty(userSelection.IndicatorNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.IUSNId + " IN (" + userSelection.IndicatorNIds + ")";
                }

                if (!string.IsNullOrEmpty(userSelection.AreaNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.AreaNId + " IN (" + userSelection.AreaNIds + ")";
                }

                if (!string.IsNullOrEmpty(userSelection.TimePeriodNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN (" + userSelection.TimePeriodNIds + ")";
                }

                if (!string.IsNullOrEmpty(userSelection.SourceNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.SourceNId + " IN (" + userSelection.SourceNIds + ")";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }


        /// <summary>
        /// Get Unmatched Areas i.e Area Exist In Area Table But Not IN Data Table
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedAreas()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT " + DIColumns.Data.AreaNId + " FROM " + this.TablesName.Data + " D ");

            RetVal.Append(" WHERE NOT EXISTS(SELECT * FROM " + this.TablesName.Area + " A ");

            RetVal.Append(" WHERE A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + ")");

            return RetVal.ToString();
        }

        /// <summary>
        /// RelationShip Between Source And IUSNID By Data->SourceNid and ICIUS->IC_Nid
        /// </summary>
        /// <returns></returns> 
        public string GetUnmatchedDataBySourceNid()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT Distinct( " + DIColumns.Data.SourceNId + ") FROM " + this.TablesName.Data + " AS D WHERE NOT EXISTS( SELECT * FROM ");

            RetVal.Append(this.TablesName.IndicatorClassifications + " AS IC ");

            RetVal.Append(" WHERE  IC." + DIColumns.IndicatorClassifications.ICNId + " = D." + DIColumns.Data.SourceNId + ")");

            return RetVal.ToString();
        }

        /// <summary>
        /// IUSNId Exist In Data but not in IC_IUS Table
        /// RelationShip Between Source And IUSNID By Data->IUSNID and ICIUS->IUSNId
        /// </summary>
        /// <returns></returns>
        public string GetUnMatchedDataByIUSNIDWICIUS()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT DISTINCT " + DIColumns.Data.SourceNId + ", " + DIColumns.Data.IUSNId + " FROM "
                + this.TablesName.Data + " AS D  WHERE NOT EXISTS ");

            RetVal.Append(" ( SELECT * FROM " + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS  WHERE ");
            RetVal.Append("  ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " = D." + DIColumns.Data.IUSNId);
            RetVal.Append(" AND  ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = D." + DIColumns.Data.SourceNId + ")");

            return RetVal.ToString();

        }

        /// <summary>
        /// Footnotes exist in Data but not in Footnote table
        /// </summary>
        /// <returns></returns>
        public string GetUnMatchedDataByFootnotes()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT DISTINCT(" + DIColumns.Data.FootNoteNId + ")  FROM " + this.TablesName.Data + " AS D WHERE NOT EXISTS( SELECT * FROM ");

            RetVal.Append(this.TablesName.FootNote + " AS F  WHERE  F." + DIColumns.FootNotes.FootNoteNId + " = D." + DIColumns.Data.FootNoteNId + ")");

            return RetVal.ToString();

        }

        /// <summary>
        /// IUSNId exist in Data but not in IUS table
        /// </summary>
        /// <returns></returns>
        public string GetUnMatchedDataByIUSNId()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT DISTINCT(" + DIColumns.Data.IUSNId + ")  FROM " + this.TablesName.Data + " AS D WHERE NOT EXISTS( SELECT * FROM ");

            RetVal.Append(this.TablesName.IndicatorUnitSubgroup + " AS IUS  WHERE  IUS.");

            RetVal.Append(DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + ")");

            return RetVal.ToString();

        }

        /// <summary>
        /// Get SourceNid where IUSNId exist in Data but not in IUS table
        /// </summary>
        /// <returns></returns>
        public string GetUnMatchedDataSourceNidByIUSNId()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT DISTINCT(" + DIColumns.Data.SourceNId + ")  FROM " + this.TablesName.Data + " AS D WHERE NOT EXISTS( SELECT * FROM ");

            RetVal.Append(this.TablesName.IndicatorUnitSubgroup + " AS IUS  WHERE  IUS.");

            RetVal.Append(DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + ")");

            return RetVal.ToString();

        }


        /// <summary>
        /// Timeperiod exist in Data but not in Timeperiod table
        /// </summary>
        /// <returns></returns>
        public string GetUnMatchedDataByTimeperiod()
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT DISTINCT(" + DIColumns.Data.TimePeriodNId + ")  FROM " + this.TablesName.Data + " AS D WHERE NOT EXISTS( SELECT * FROM ");

            RetVal.Append(this.TablesName.TimePeriod + " AS T WHERE  T.");

            RetVal.Append(DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId + ")");

            return RetVal.ToString();

        }

        /// <summary>
        /// Get Minimum Timeperiod as MinTimeperiod and Maximum Timeperiod as MaxTimeperiod For IUSNIds
        /// </summary>
        /// <param name="iusNIds"></param>
        /// <returns></returns>
        public string GetMinMaxTimeperiodByIUS(string iusNIds)
        {
            StringBuilder RetVal = new StringBuilder();


            RetVal.Append("SELECT MIN(T." + DIColumns.Timeperiods.TimePeriod + ")AS MinTimeperiod,MAX(T." + DIColumns.Timeperiods.TimePeriod + ") AS MaxTimeperiod ");

            RetVal.Append(" FROM " + this.TablesName.Data + " AS D INNER JOIN " + this.TablesName.TimePeriod + " AS T ");

            RetVal.Append(" ON D." + DIColumns.Data.TimePeriodNId + "= T." + DIColumns.Timeperiods.TimePeriodNId);

            RetVal.Append(" WHERE D." + DIColumns.Data.IUSNId + " IN (" + iusNIds + ")");


            return RetVal.ToString();
        }

        /// <summary>
        /// Get the Duplicate Data Values By Nid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateDataValuesByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Data.DataNId + "," + DIColumns.Data.DataValue);

            SqlQuery.Append(" FROM " + this.TablesName.Data + " WHERE " + DIColumns.Data.DataNId + " IN(");

            SqlQuery.Append("SELECT " + DIColumns.Data.DataNId);

            SqlQuery.Append(" FROM " + this.TablesName.Data + " GROUP BY ");

            SqlQuery.Append(DIColumns.Data.DataNId + " HAVING COUNT(*) >1 )");
            return SqlQuery.ToString();
        }

        /// <summary>
        /// Query to get those ius which contains data
        /// </summary>
        /// <param name="iusNids"></param>
        /// <returns></returns>
        public string GetIUSNidWhereDataExistsForIUS(string iusNids)
        {
            string RetVal = string.Empty;

            RetVal = "select distinct " + DIColumns.Data.IUSNId + " from " + this.TablesName.Data + " where " + DIColumns.Data.IUSNId + " in(" + iusNids + ") and " + DIColumns.Data.DataValue + " is not null";

            return RetVal;
        }

        #endregion

        #endregion
    }
}
