using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Queries.Calculates
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

        #region " --Method -- "

        /// <summary>
        /// On the basis of server type cast value  to String
        /// </summary>
        /// <param name="value"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        private string TypeCastToString(string value, DIServerType serverType)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.SqlServerExpress:
                    RetVal = "cast(" + value + " as varchar (60))";
                    break;
                case DIServerType.MsAccess:
                    RetVal = "cstr(" + value + ")";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.MySql:
                    RetVal = "CAST(" + value + " AS CHAR)";
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }
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
        /// Get indicator and subgroup details based on userselection and show IUS
        /// </summary>
        /// <param name="showSubgroup"></param>
        /// <returns></returns>
        public String Indicator_Subgroup_Selected(Boolean showSubgroup, UserSelection.UserSelection userSelection, DIServerType serverType)
        {
            String RetVal;
            StringBuilder sbQuery = new StringBuilder();
            // If show subgroup, then get subgroup information
            if (showSubgroup)
            {
                // Select Ind NId,Name,Ind_Global,SubgroupValNId,SubgroupVal,subgroupVal_Global, IUSNID
                sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(", S." + DIColumns.SubgroupVals.SubgroupValNId + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
                sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
                sbQuery.Append(this.TablesName.SubgroupVals + " S," + this.TablesName.IndicatorUnitSubgroup + " IUS,");
                sbQuery.Append(this.TablesName.Indicator + " I, " + this.TablesName.Area + " A, " + this.TablesName.TimePeriod + " T");
                sbQuery.Append(" WHERE D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND  D." + DIColumns.Data.TimePeriodNId + "= T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId + " AND D." + DIColumns.Data.SourceNId + "=IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " =I." + DIColumns.Indicator.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " =S." + DIColumns.SubgroupVals.SubgroupValNId);

                // If Subgroup deleted
                if (userSelection.DataViewFilters.DeletedSubgroupNIds.Length > 0)
                {
                    if (userSelection.DataViewFilters.ShowSubgroupByIndicator)
                    {

                        sbQuery.Append(" AND " + TypeCastToString("IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId, serverType) + " +'_' + ");
                        sbQuery.Append(TypeCastToString("IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId, serverType));
                        sbQuery.Append(" NOT IN (" + userSelection.DataViewFilters.DeletedSubgroupNIds + ")");
                    }
                    else
                    {
                        sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " NOT IN (" + userSelection.DataViewFilters.DeletedSubgroupNIds + ")");
                    }
                }
            }
            else
            {
                sbQuery.Append("SELECT DISTINCT I.Indicator_NId,I.Indicator_Name, I.Indicator_Global");
                sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
                sbQuery.Append(this.TablesName.IndicatorUnitSubgroup + " IUS,");
                sbQuery.Append(this.TablesName.Indicator + " I, " + this.TablesName.Area + " A," + this.TablesName.TimePeriod + " T");
                sbQuery.Append(" WHERE D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND D." + DIColumns.Data.TimePeriodNId + " =T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId + " AND D." + DIColumns.Data.SourceNId + "=IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "=I." + DIColumns.Indicator.IndicatorNId);
            }

            //'*** Filters for dataview  Indicator-Time-Area

            //If user selection have indicator NId then include them in filter criteria of where clause
            if (userSelection.IndicatorNIds.Length > 0)
            {
                if (userSelection.ShowIUS)
                {
                    // Match  userselection indicatorNIds are matched with IUSNId
                    //_Query &= " AND D.IUSNID in (" & .Indicators & ")"
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " in (" + userSelection.IndicatorNIds + ")");
                }
                else
                {
                    // Match  userselection indicatorNIds are matched with IndicatorNId
                    //_Query &= " AND IUS.Indicator_Nid in (" & .Indicators & ") "		 
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + userSelection.IndicatorNIds + ") ");
                }
            }
            // Include selected Area in Search
            if (userSelection.AreaNIds.Length > 0)
            {
                // Match for selected NIds in Data table
                //_Query &= " AND D.Area_NId in (" & .Areas & ")"
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " in (" + userSelection.AreaNIds + ")");
            }
            if (userSelection.TimePeriodNIds.Length > 0)
            {
                //_Query &= " AND D.Timeperiod_NId in (" & .Time & ")"
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " in (" + userSelection.TimePeriodNIds + ")");
            }

            // -- Append order by clause
            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Indicator details(NId,Name,Global,GId)fitered on the bais of selections in user selection
        /// </summary>
        /// <param name="userselection"></param>
        /// <returns></returns>
        public string GetIndicatorList(UserSelection.UserSelection userselection)
        {
            string retval = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // Select Clause : select Ind NId, Name,GLobal and GId
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", I." + DIColumns.Indicator.IndicatorGId);

            // From Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
            sbQuery.Append(this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Indicator + " I,");
            sbQuery.Append(this.TablesName.Area + " A," + this.TablesName.TimePeriod + " T ");

            // Where Clause
            sbQuery.Append("WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "AND IC." + DIColumns.IndicatorClassifications.ICNId + " = D." + DIColumns.Data.SourceNId);
            sbQuery.Append("AND D." + DIColumns.Data.TimePeriodNId + "=T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId);

            // Adding user selection selections in where clause
            if (userselection.IndicatorNIds.Length > 0)
            {
                //If show IUS then match userselection selected nid  with IUSNIds
                if (userselection.ShowIUS)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in (" + userselection.IndicatorNIds + ")");
                }
                else
                {
                    //If show IUS then match userselection selected nid  with IndicatorNIds
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + userselection.IndicatorNIds + ") ");
                }
            }

            // If area in found in user selection include them in query where clause   
            if (userselection.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " in (" + userselection.AreaNIds + ")");
            }

            // If timeperiod in found in user selection include them in query where clause   
            if (userselection.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " in (" + userselection.TimePeriodNIds + ")");
            }

            //// If fiter is applied for notes. Consider notes filter
            //if ((userselection.DataViewFilters.CommentFilter) && userselection.DataViewFilters.CommentDataNIds.Length > 0)
            //{
            //    sbQuery.Append(" AND D.Data_NId IN (" + userselection.DataViewFilters.CommentDataNIds + ")");
            //}

            // Return query
            retval = sbQuery.ToString();
            return retval;
        }

        /// <summary>
        /// Get Unit details(NId,Name,Global,GId)fitered on the bais of selections in user selection
        /// </summary>
        /// <param name="userselection"></param>
        /// <returns></returns>
        public String GetUnitList(UserSelection.UserSelection userselection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT ");
            if (userselection.DataViewFilters.ShowUnitByIndicator)
            {

                // Include indicator columns                               
                sbQuery.Append("I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ",");
            }
            else
            {
                // Get unit Nid
                sbQuery.Append("U." + DIColumns.Unit.UnitNId);
            }

            //_Query &= ",U.Unit_Name, U.Unit_Global, U.Unit_GId"
            sbQuery.Append("U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal + ",U." + DIColumns.Unit.UnitGId);

            //From clause        
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
            sbQuery.Append(this.TablesName.SubgroupVals + " S," + this.TablesName.IndicatorUnitSubgroup + " IUS,");
            sbQuery.Append(this.TablesName.Unit + " U," + this.TablesName.Indicator + " I,");
            sbQuery.Append(this.TablesName.Area + " A," + this.TablesName.TimePeriod + " T ");

            // where clause
            sbQuery.Append("WHERE S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + " And ");
            sbQuery.Append("IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + " AND D.");
            sbQuery.Append(DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId);
            sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + "=T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId);
            // If indicator in found in user selection include them in query where clause
            if (userselection.IndicatorNIds.Length > 0)
            {
                if (userselection.ShowIUS)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in (" + userselection.IndicatorNIds + ")");
                }
                else
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + userselection.IndicatorNIds + ") ");
                }
            }

            // If area in found in user selection include them in query where clause   
            if (userselection.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " in (" + userselection.AreaNIds + ")");
            }

            // If timeperiod in found in user selection include them in query where clause   
            if (userselection.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " in (" + userselection.TimePeriodNIds + ")");
            }

            //// If fiter is applied for notes. Consider notes filter
            //if ((userselection.DataViewFilters.CommentFilter) && userselection.DataViewFilters.CommentDataNIds.Length > 0)
            //{
            //    sbQuery.Append(" AND D.Data_NId IN (" + userselection.DataViewFilters.CommentDataNIds + ")");
            //}
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get SubgroupVal details(NId,Name,Global,GId)fitered on the bais of selections in user selection
        /// </summary>
        /// <param name="userselection"></param>
        /// <returns></returns>
        public string GetSubgroupList(UserSelection.UserSelection userselection)
        {
            string retval = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT ");

            // if show subgroup by indicator
            if (userselection.DataViewFilters.ShowSubgroupByIndicator)
            {
                // Include indicator columns                
                sbQuery.Append("I." + DIColumns.Indicator.IndicatorNId + ", S." + DIColumns.SubgroupVals.SubgroupValNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ",");
            }
            else
            {
                sbQuery.Append("S." + DIColumns.SubgroupVals.SubgroupValNId + ",");
            }
            // Add subgroup val, Global and GId Columns in query 
            sbQuery.Append("S." + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.SubgroupVals.SubgroupValGlobal + ",S." + DIColumns.SubgroupVals.SubgroupValGId);

            //From Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
            sbQuery.Append(this.TablesName.SubgroupVals + " S," + this.TablesName.IndicatorUnitSubgroup + " IUS,");
            sbQuery.Append(this.TablesName.Unit + " U," + this.TablesName.Indicator + " I,");
            sbQuery.Append(this.TablesName.Area + " A," + this.TablesName.TimePeriod + " T ");

            //Where Clause
            sbQuery.Append("WHERE S." + SubgroupVals.SubgroupValNId + " = IUS." + Indicator_Unit_Subgroup.SubgroupValNId + " AND IUS." + Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + " And ");
            sbQuery.Append("IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + " AND D.");
            sbQuery.Append(DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId);
            sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + "=T." + Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId);

            // If indicator in found in user selection include them in query's where clause
            if (userselection.IndicatorNIds.Length > 0)
            {
                if (userselection.ShowIUS)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in (" + userselection.IndicatorNIds + ")");
                }
                else
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + userselection.IndicatorNIds + ") ");
                }
            }

            // If area in found in user selection include them in query where clause   
            if (userselection.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " in (" + userselection.AreaNIds + ")");
            }

            // If timeperiod in found in user selection include them in query where clause   
            if (userselection.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " in (" + userselection.TimePeriodNIds + ")");
            }

            //// If fiter is applied for notes. Consider notes filter
            //if ((userselection.DataViewFilters.CommentFilter) && userselection.DataViewFilters.CommentDataNIds.Length > 0)
            //{
            //    sbQuery.Append(" AND D.Data_NId IN (" + userselection.DataViewFilters.CommentDataNIds + ")");
            //}

            retval = sbQuery.ToString();
            return retval;
        }

        /// <summary>
        /// Get Source details(sourceNId,Name,Global,recomendedSource) fitered on the bais of selections in user selection
        /// If userselection contain ShowSourceByIUS then include indicator unit and subgroup columns
        /// </summary>
        /// <param name="userselection"></param>
        /// <returns></returns>
        public String GetSourceList(UserSelection.UserSelection userselection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT ");

            // if ShowSourceByIUS then include indicator , unit and subgroup
            if (userselection.DataViewFilters.ShowSourceByIUS)
            {
                // Include indicator,unit,subgroup columns                                                
                sbQuery.Append("D." + DIColumns.Data.IUSNId + ", D." + DIColumns.Data.SourceNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ",");
                sbQuery.Append(" U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            else
            {
                // Get Source Nid from data table                 
                sbQuery.Append("D." + DIColumns.Data.SourceNId);
            }
            // Add other source information from ic Table(ICName,global and recomended source)
            sbQuery.Append(",IC." + DIColumns.IndicatorClassifications.ICName + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource);

            // From Clause                  
            sbQuery.Append(" FROM " + this.TablesName.IndicatorClassifications + " IC," + this.TablesName.Data + " D,");
            sbQuery.Append(this.TablesName.SubgroupVals + " S," + this.TablesName.IndicatorUnitSubgroup + " IUS,");
            sbQuery.Append(this.TablesName.Unit + " U," + this.TablesName.Indicator + " I,");
            sbQuery.Append(this.TablesName.Area + " A," + this.TablesName.TimePeriod + " T,");
            sbQuery.Append(this.TablesName.IndicatorClassificationsIUS + " ICIUS ");

            //Where clause            
            sbQuery.Append("WHERE S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + " And ");
            sbQuery.Append(" IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + " AND D.");
            sbQuery.Append(DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "AND IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId);
            sbQuery.Append("AND D." + DIColumns.Data.TimePeriodNId + "=T." + DIColumns.Timeperiods.TimePeriodNId + " AND D." + DIColumns.Data.AreaNId + " =A." + DIColumns.Area.AreaNId);
            sbQuery.Append("AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + "=IC." + DIColumns.IndicatorClassificationsIUS.ICNId);


            // If indicator in found in user selection include them in query where clause
            if (userselection.IndicatorNIds.Length > 0)
            {
                if (userselection.ShowIUS)
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in (" + userselection.IndicatorNIds + ")");
                }
                else
                {
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " in (" + userselection.IndicatorNIds + ") ");
                }
            }

            // If area in found in user selection include them in query where clause   
            if (userselection.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " in (" + userselection.AreaNIds + ")");
            }

            // If timeperiod in found in user selection include them in query where clause   
            if (userselection.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " in (" + userselection.TimePeriodNIds + ")");
            }

            //// If fiter is applied for notes. Consider notes filter
            //if ((userselection.DataViewFilters.CommentFilter) && userselection.DataViewFilters.CommentDataNIds.Length > 0)
            //{
            //    sbQuery.Append(" AND D.Data_NId IN (" + userselection.DataViewFilters.CommentDataNIds + ")");
            //}

            // Return query
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Area Level using user selection's indicator,time and area nids 
        /// </summary>
        /// <param name="userSelection"></param>
        /// <param name="showAllAreaLevel">If True all area level will returned and user selection is not considered  </param>
        /// <returns></returns>
        public string GetAreaLevelByUserselection(UserSelection.UserSelection userSelection, bool showAllAreaLevel)
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            // If available area level will be returned . Indicator, timeperiod and area selection in userselection is not considered for filtering 
            if (showAllAreaLevel)
            {
                SbQuery.Append("SELECT DISTINCT AL." + Area_Level.AreaLevelName + ",AL." + Area_Level.AreaLevel + " FROM " + TablesName.AreaLevel + " AL");
            }
            else
            {
                // selected Area level considering selected area in user selection
                if (userSelection.AreaNIds.Length > 0)
                {
                    SbQuery.Append("SELECT DISTINCT AL." + Area_Level.AreaLevelName + ", AL." + Area_Level.AreaLevel + " FROM " + TablesName.AreaLevel + " AL, ");
                    SbQuery.Append(TablesName.Area + " A ");

                    // Where claues
                    SbQuery.Append(" WHERE AL." + Area_Level.AreaLevel + "=A." + DIColumns.Area.AreaLevel + " AND A." + DIColumns.Area.AreaNId + " in (" + userSelection.AreaNIds + ")");

                }
                // Selected area levels using area nid in data table
                else
                {
                    SbQuery.Append("SELECT DISTINCT AL." + Area_Level.AreaLevelName + ", AL." + Area_Level.AreaLevel + " FROM " + TablesName.AreaLevel + " AL,  ");
                    SbQuery.Append(TablesName.Area + " A, " + TablesName.Data + " D");

                    //-- Include Indicator_Unit_Subgroup in FROM clause in case  ShowIUS = false (Join on basis of indicatorNId)
                    if (userSelection.IndicatorNIds.Length > 0 && userSelection.ShowIUS == false)
                    {
                        SbQuery.Append(", " + TablesName.IndicatorUnitSubgroup + " IUS");
                    }

                    SbQuery.Append(" WHERE AL." + Area_Level.AreaLevel + "=A." + DIColumns.Area.AreaLevel + " AND D." + DIColumns.Data.AreaNId + "=A." + DIColumns.Area.AreaNId);
                    
                    // If user selection has indicator then filter out only those data in data table where IUSNid match with selected indicators     
                    if (userSelection.IndicatorNIds.Length > 0)
                    {
                        if (userSelection.ShowIUS)
                        {
                            SbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " in (" + userSelection.IndicatorNIds + ")");
                        }
                        else
                        {
                            SbQuery.Append(" AND IUS." + Indicator_Unit_Subgroup.IUSNId + "=D." + DIColumns.Data.IUSNId + " AND IUS." + Indicator_Unit_Subgroup.IndicatorNId + " in (" + userSelection.IndicatorNIds + ")");
                        }
                    }

                    if (userSelection.TimePeriodNIds.Length > 0)
                    {
                        SbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " in (" + userSelection.TimePeriodNIds + ")");
                    }
                }
            }
            RetVal = SbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Returns ICNId from Indicator_Classifications table on the basis of GID
        /// </summary>
        /// <param name="ICGId"></param>
        /// <returns></returns>
        public string GetICNIdByGId(string ICGId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT IC_NId FROM " + this.TablesName.IndicatorClassifications + " WHERE " + IndicatorClassifications.ICGId + " in (" + ICGId + " ) ";
            return RetVal;
        }

        /// <summary>
        /// Returns unitNId from Unit table on the basis of unit GID
        /// </summary>
        /// <param name="unitGId"></param>
        /// <returns></returns>
        public string GetUnitNIdByGId(String unitGId)
        {
            string RetVal = string.Empty;
            //"SELECT Unit_NId FROM " & sDB_Prefix & "Unit" & sLng_Suffix & " WHERE Unit_GId='" & moDESheetInfo.UnitGUID & "'"
            RetVal = "SELECT Unit_NId FROM " + this.TablesName.Unit + " WHERE " + DIColumns.Unit.UnitGId + " ='" + unitGId + "'";
            return RetVal;
        }

        /// <summary>
        /// Returns SubgroupValsNId from SubgroupVals table on the basis of SubgroupVals GID
        /// </summary>
        /// <param name="subgroupValGId"></param>
        /// <returns></returns>
        public string GetSubgroupValNIdByGId(String subgroupValGId)
        {
            string RetVal = string.Empty;
            //"SELECT Subgroup_Val_NId FROM " & sDB_Prefix & "Subgroup_Vals" & sLng_Suffix & " WHERE Subgroup_Val_GId='" & moDESheetInfo.SubgroupGUID(i) & "'"
            RetVal = "SELECT " + SubgroupVals.SubgroupValNId + " FROM " + this.TablesName.SubgroupVals + " WHERE " + DIColumns.SubgroupVals.SubgroupValGId + " ='" + subgroupValGId + "'";
            return RetVal;

        }

        /// <summary>
        /// Get IUS NId from IUS table using indicator,unit,subgroup NIds
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="SubgroupNId"></param>
        /// <returns></returns>
        public String GetIUSNIdByIUS(int indicatorNId, int unitNId, int SubgroupNId)
        {
            String RetVal = string.Empty;

            RetVal = "SELECT " + Indicator_Unit_Subgroup.IUSNId + " FROM " + this.TablesName.IndicatorUnitSubgroup + " WHERE " + Indicator_Unit_Subgroup.IndicatorNId +
            "=" + indicatorNId + " AND " + Indicator_Unit_Subgroup.UnitNId + " = " + unitNId + " AND " +
            Indicator_Unit_Subgroup.SubgroupValNId + "=" + SubgroupNId;

            return RetVal;
        }

        /// <summary>
        /// Get timeperiodNId using timeperiod
        /// </summary>
        /// <returns></returns>
        public String GetTimeperiodNIdByTimePeriod(String timePeriod)
        {
            String RetVal = String.Empty;
            RetVal = "SELECT " + Timeperiods.TimePeriodNId + " FROM " + this.TablesName.TimePeriod + " WHERE " +
                 Timeperiods.TimePeriod + "='" + timePeriod + "'";
            return RetVal;
        }

        /// <summary>
        /// Get AreaNId using AreaID
        /// </summary>
        /// <returns></returns>
        public String GetAreaNIdByAreaID(String areaID)
        {
            String RetVal = String.Empty;
            //sQry = "SELECT Area_NId FROM " & sDB_Prefix & "Area" & sLng_Suffix & " WHERE Area_ID='" & sAreaId & "'"
            RetVal = "SELECT " + DIColumns.Area.AreaNId + " FROM " + this.TablesName.Area + " WHERE " +
            DIColumns.Area.AreaID + "='" + areaID + "'";
            return RetVal;
        }

        /// <summary>
        /// Get ParentNId from indicator_Classification Table using icType and NIds
        /// </summary>
        /// <param name="iCNId"></param>
        /// <param name="ictype">String value like 'SR'</param>
        /// <returns></returns>
        public String GetICParentNIdByICNId(int iCNId, String ictype)
        {
            string RetVal = string.Empty;
            //sQry = "SELECT IC_Parent_NId FROM " & sDB_Prefix & "Indicator_Classifications" & sLng_Suffix & " WHERE IC_Type='SR' AND IC_NId=" & iSource_NId
            RetVal = "SELECT " + IndicatorClassifications.ICParent_NId + " FROM " + this.TablesName.IndicatorClassifications +
            " WHERE " + IndicatorClassifications.ICType + "=' " + ictype + "' AND " + IndicatorClassifications.ICNId + "=" + iCNId;
            return RetVal;
        }

        /// <summary>
        /// Get FootNoteNId uusing FootNote column value
        /// </summary>
        /// <param name="footNote"></param>
        /// <returns></returns>
        public String GetFootNoteNIdByFootNote(string footNote)
        {
            string RetVal = string.Empty;

            //"SELECT FootNote_NId FROM " & sDB_Prefix & "FootNote" & sLng_Suffix & " WHERE FootNote='" & sFootNote & "'"
            RetVal = "SELECT " + FootNotes.FootNoteNId + " FROM " + this.TablesName.FootNote + " WHERE " + FootNotes.FootNote + "='" + footNote + "'";

            return RetVal;
        }


        #region " --Insert -- "

        /// <summary>
        /// Insert record in subgroupVal Table
        /// </summary>
        /// <param name="subgroupName"></param>
        /// <param name="subgroupGID"></param>
        /// <param name="isSubgroupGlobal"></param>
        /// <returns></returns>
        public string InsertSubgroupVal(string subgroupName, string subgroupGID, bool isSubgroupGlobal)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + this.TablesName.SubgroupVals + " (Subgroup_Val,Subgroup_Val_GId,Subgroup_Val_Global)"
                 + " VALUES ('" + subgroupName + "','" + subgroupGID + "'," + isSubgroupGlobal + ")";

            return RetVal;
        }

        /// <summary>
        /// Insert indicator ,unit and subgroupNids in IUS table
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNid"></param>
        /// <param name="SubgroupNId"></param>
        /// <returns></returns>
        public string InsertNIdsInIUSTable(int indicatorNId, int unitNId, int SubgroupNId)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + this.TablesName.IndicatorUnitSubgroup + " (" + Indicator_Unit_Subgroup.IndicatorNId + ","
            + Indicator_Unit_Subgroup.UnitNId + "," + Indicator_Unit_Subgroup.SubgroupValNId + ")"
            + " VALUES (" + indicatorNId + "," + unitNId + "," + SubgroupNId + ")";

            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ICNId"></param>
        /// <param name="IUSNId"></param>
        /// <returns></returns>
        public string InsertIC_IUSRelation(int ICNId, int IUSNId)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + this.TablesName.IndicatorClassificationsIUS + " (" + IndicatorClassificationsIUS.ICNId + "," +
                IndicatorClassificationsIUS.IUSNId + ") VALUES (" + ICNId + "," + IUSNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Insert data into data table for calulates 
        /// </summary>
        /// <param name="IUSNId"></param>
        /// <param name="timePeriodNId"></param>
        /// <param name="areaNId"></param>
        /// <param name="dataValue"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="dataDenominator"></param>
        /// <param name="footNoteNId"></param>
        /// <param name="sourceNId"></param>
        /// <returns></returns>
        public string InsertDataForCalculate(int IUSNId, int timePeriodNId, int areaNId, string dataValue, string startDate, string endDate, string dataDenominator, int footNoteNId, int sourceNId)
        {
            string RetVal = string.Empty;

            // If dataDenominator value is present then include data denominator column in insert   
            if (dataDenominator.Length > 0)
            {
                RetVal = "INSERT INTO " + this.TablesName.Data + " (" + DIColumns.Data.IUSNId + "," +
                DIColumns.Data.TimePeriodNId + "," + DIColumns.Data.AreaNId + "," + DIColumns.Data.DataValue + ","
                 + DIColumns.Data.StartDate + "," + DIColumns.Data.EndDate +
                 "," + DIColumns.Data.DataDenominator + "," + DIColumns.Data.FootNoteNId + ","
                 + DIColumns.Data.SourceNId + ")  VALUES(" + IUSNId + "," + timePeriodNId + "," + areaNId + ",'" +
                 dataValue + "'," + startDate + "," + endDate + "," + dataDenominator + footNoteNId + "," +
                 sourceNId + ")";
            }

         // If dataDenominator value is not present do not include data denominator column in insert  
            else
            {
                RetVal = "INSERT INTO " + this.TablesName.Data + " (" + DIColumns.Data.IUSNId + "," +
                DIColumns.Data.TimePeriodNId + "," + DIColumns.Data.AreaNId + "," + DIColumns.Data.DataValue + ","
                + DIColumns.Data.StartDate + "," + DIColumns.Data.EndDate +
                ", " + DIColumns.Data.FootNoteNId + "," + DIColumns.Data.SourceNId + ")  VALUES("
                + IUSNId + "," + timePeriodNId + "," + areaNId + ",'" +
                dataValue + "'," + startDate + "," + endDate + "," + footNoteNId + "," + sourceNId + ")";
            }
            // retun query
            return RetVal;
        }

        #endregion

        #endregion

        #endregion

    }
}
