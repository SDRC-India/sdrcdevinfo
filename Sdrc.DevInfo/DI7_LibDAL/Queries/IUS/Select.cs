using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.IUS
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

        #region "-- Method --"

        private string GetGenericSelectClause(FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();

                //  SELECT Clause
                sbQuery.Append(" SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

                if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
                {
                    sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                    sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                    sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                }
                if (fieldSelection == FieldSelection.Heavy)
                {
                    sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
                }

                RetVal = sbQuery.ToString();
            }
            catch (Exception)
            {
            }
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
        /// Get IUS not related with Indicator Classification.
        /// </summary>
        /// <returns></returns>
        public string GetIUSUnmatchedIC()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal
                    + " FROM " + this.TablesName.SubgroupVals + " S, "
                    + this.TablesName.Unit + " U, "
                    + this.TablesName.Indicator + " I,"
                    + this.TablesName.IndicatorUnitSubgroup + " IUS "
                    + " WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId
                    + " AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId
                    + " AND S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId
                    + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId
                    + " Not In (" + " SELECT " + DIColumns.IndicatorClassificationsIUS.IUSNId
                    + " FROM " + this.TablesName.IndicatorClassifications + " IC,"
                    + this.TablesName.IndicatorClassificationsIUS + " ICIUS "
                    + " WHERE IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId
                    + " AND IC." + DIColumns.IndicatorClassifications.ICType + "<>" + DIQueries.ICTypeText[ICType.Source] + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns last level classifications
        /// </summary>
        /// <returns></returns>
        public string GetLastLevelICs()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT  * FROM " + this.TablesName.IndicatorClassifications + " AS IC WHERE NOT  EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassifications + " AS IC1 WHERE  IC." + DIColumns.IndicatorClassifications.ICNId + "=IC1." + DIColumns.IndicatorClassifications.ICParent_NId + " AND IC1." + DIColumns.IndicatorClassifications.ICParent_NId + " >0)";

            return RetVal;
        }


        /// <summary>
        /// Get IUS Linked To Classification
        /// </summary>
        /// <returns></returns>
        public string GetIUSLinkedTOClasses()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal
                + " FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS, " + this.TablesName.Indicator + " AS I,"
                + this.TablesName.Unit + " AS U, "
                + this.TablesName.SubgroupVals + " SGV, "
                + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS, "
                + this.TablesName.IndicatorClassifications + " AS IC "
                + " WHERE 1=1  AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId
                + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId
                + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "= SGV." + DIColumns.SubgroupVals.SubgroupValNId
                + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId
                + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId
                + " GROUP BY I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ", IC." + DIColumns.IndicatorClassifications.ICType
                + " HAVING (IC." + DIColumns.IndicatorClassifications.ICType + "<>" + DIQueries.ICTypeText[ICType.Source] + " AND count(*)>1 )";

            return RetVal;
        }

        /// <summary>
        /// Return query to Get IUS witout DATA.
        /// </summary>
        /// <returns></returns>
        public string GetIUSWithoutData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", U." + DIColumns.Unit.UnitName + ", I." + DIColumns.Indicator.IndicatorName
                    + ", S." + DIColumns.SubgroupVals.SubgroupVal
                    + " FROM " + this.TablesName.Unit + " AS U, "
                    + this.TablesName.SubgroupVals + " AS S, "
                    + this.TablesName.Indicator + " AS I, "
                    + this.TablesName.IndicatorUnitSubgroup + " AS IUS "
                    + " WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND S."
                    + DIColumns.SubgroupVals.SubgroupValNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " AND U."
                    + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " NOT IN ( SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId
                    + " FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS, "
                    + this.TablesName.Data + " D "
                    + " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId + ")";

            return RetVal;
        }


        /// <summary>
        /// Retruns sql query to get distinct indicator & unit where  data exists
        /// </summary>
        /// <returns></returns>
        public string GetDistinctIndicatorUnitFrmData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + " ,I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorInfo + ",I." + DIColumns.Indicator.IndicatorGlobal + ", " +
        "U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId
        + ",U." + DIColumns.Unit.UnitGlobal + "  FROM " + this.TablesName.Unit + " as U," + this.TablesName.Indicator + " as I,"
        + this.TablesName.IndicatorUnitSubgroup + " as IUS," + this.TablesName.Data + " AS D "
        + "WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=D." + DIColumns.Indicator_Unit_Subgroup.IUSNId
        + " AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId;

            return RetVal;
        }

        /// <summary>
        /// Get all IUS Items where comments are exists
        /// </summary>
        /// <returns></returns>
        public string GetAllIUSForComments()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ", T." + DIColumns.Timeperiods.TimePeriod + ", A."
                + DIColumns.Area.AreaName + ", D." + DIColumns.Data.DataValue + ", IC." + DIColumns.IndicatorClassifications.ICName + ",  N." + DIColumns.Notes_Data.DataNId
                + " FROM "
                + this.TablesName.IndicatorUnitSubgroup + " AS IUS, "
                + this.TablesName.Indicator + " AS I, "
                + this.TablesName.Unit + " AS U,"
                + this.TablesName.SubgroupVals + " AS SGV, "
                + this.TablesName.TimePeriod + " AS T, "
                + this.TablesName.Area + " AS A,"
                + this.TablesName.IndicatorClassifications + " AS IC, "
                + this.TablesName.Data + " AS D, "
                + this.TablesName.NotesData + " AS N"
                + " WHERE  D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.UnitNId + "= U." + DIColumns.Unit.UnitNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "= SGV." + DIColumns.SubgroupVals.SubgroupValNId + " AND D."
                + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId + " AND D."
                + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId + " AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId
                + " AND D." + DIColumns.Data.DataNId + "= N." + DIColumns.Notes_Data.DataNId;


            return RetVal;
        }

        /// <summary>
        /// Gets DISTINCT INdicator_Name, Unit_Name, SubgroupVal for which Data exists in DataBase.
        /// </summary>
        /// <param name="indicatorNids">Comma delimited indicatorNid to filter. (Use blank for no filter)</param>
        /// <param name="unitNids">Comma delimited UnitNid to filter. (Use blank for no filter)</param>
        /// <param name="subgroupvalNids">Comma delimited SubgroupValNid to filter. (Use blank for no filter)</param>
        /// <returns>SQl query.</returns>
        public string GetAutoDistinctIUS(string indicatorNids, string unitNids, string subgroupvalNids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Distinct I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId + ", S." + DIColumns.SubgroupVals.SubgroupValNId +
           " FROM (" + this.TablesName.SubgroupVals + " AS S INNER JOIN (" + this.TablesName.Unit + " AS U INNER JOIN (" + this.TablesName.Indicator + " AS I INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ") ON U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ") ON S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ") INNER JOIN " + this.TablesName.Data + " AS D ON IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId +
           " Where 1 = 1";

            if (!string.IsNullOrEmpty(indicatorNids))
            {
                RetVal += " AND I." + DIColumns.Indicator.IndicatorNId + " in (" + indicatorNids + ")";
            }

            if (!string.IsNullOrEmpty(unitNids))
            {
                RetVal += " AND (U." + DIColumns.Unit.UnitNId + " ) in (" + unitNids + ")";
            }

            //subgroupvalNids

            if (!string.IsNullOrEmpty(subgroupvalNids))
            {
                RetVal += " AND (S." + DIColumns.SubgroupVals.SubgroupValNId + ") in (" + subgroupvalNids + ")";
            }
            return RetVal;

        }

        /// <summary>
        /// Gets DISTINCT INdicator_Name, Unit_Name, SubgroupVal for which Data exists in DataBase.
        /// </summary>
        /// <param name="indicatorNids">Comma delimited indicatorNid to filter. (Use blank for no filter)</param>
        /// <param name="unitNids">Comma delimited UnitNid to filter. (Use blank for no filter)</param>
        /// <param name="subgroupvalNids">Comma delimited SubgroupValNid to filter. (Use blank for no filter)</param>
        /// <returns>SQl query.</returns>
        public string GetDistinctIUS(string indicatorNids, string unitNids, string subgroupvalNids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT Distinct I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId + ", S." + DIColumns.SubgroupVals.SubgroupValNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId +
" FROM " + this.TablesName.SubgroupVals + " S," + this.TablesName.Unit + " U," + this.TablesName.Indicator + " I," + this.TablesName.IndicatorUnitSubgroup + " IUS WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " AND S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId;


            if (!string.IsNullOrEmpty(indicatorNids))
            {
                RetVal += " AND I." + DIColumns.Indicator.IndicatorNId + " in (" + indicatorNids + ")";
            }

            if (!string.IsNullOrEmpty(unitNids))
            {
                RetVal += " AND U." + DIColumns.Unit.UnitNId + " in (" + unitNids + ")";
            }

            //subgroupvalNids

            if (!string.IsNullOrEmpty(subgroupvalNids))
            {
                RetVal += " AND S." + DIColumns.SubgroupVals.SubgroupValNId + " in (" + subgroupvalNids + ")";
            }
            return RetVal;

        }

        /// <summary>
        /// Gets DISTINCT INdicator_Name, Unit_Name, SubgroupVal for which Data exists in DataBase, applying filter of userSelection's I, U, S, A, T, Source.
        /// </summary>
        /// <param name="userSelections">userSelection object</param>
        /// <returns>SQl query.</returns>
        public string GetAutoDistinctIUS(UserSelection.UserSelection userSelections)
        {
            string RetVal = string.Empty;


            RetVal = "SELECT Distinct I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId + ", S." + DIColumns.SubgroupVals.SubgroupValNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId +
" FROM (" + this.TablesName.SubgroupVals + " AS S INNER JOIN (" + this.TablesName.Unit + " AS U INNER JOIN (" + this.TablesName.Indicator + " AS I INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ") ON U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ") ON S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ") INNER JOIN " + this.TablesName.Data + " AS D ON IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId +
" Where 1 = 1";

            // Check if Userselection.ShowIUS is true
            if (!string.IsNullOrEmpty(userSelections.IndicatorNIds) & (userSelections.ShowIUS))
            {
                RetVal += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + userSelections.IndicatorNIds + ")";
            }
            else
            {
                if (!string.IsNullOrEmpty(userSelections.IndicatorNIds))
                {
                    RetVal += " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + userSelections.IndicatorNIds + ")";
                }
                if (!string.IsNullOrEmpty(userSelections.UnitNIds))
                {
                    RetVal += " AND (U." + DIColumns.Unit.UnitNId + " ) IN (" + userSelections.UnitNIds + ")";
                }

                //subgroupvalNids
                if (!string.IsNullOrEmpty(userSelections.SubgroupValNIds))
                {
                    RetVal += " AND (S." + DIColumns.SubgroupVals.SubgroupValNId + ") IN (" + userSelections.SubgroupValNIds + ")";
                }
            }

            // TimePeriod NID filter
            if (!string.IsNullOrEmpty(userSelections.TimePeriodNIds))
            {
                RetVal += " AND (D." + DIColumns.Timeperiods.TimePeriodNId + ") IN (" + userSelections.TimePeriodNIds + ")";
            }

            // AreaNid filter
            if (!string.IsNullOrEmpty(userSelections.AreaNIds))
            {
                RetVal += " AND (D." + DIColumns.Area.AreaNId + ") IN (" + userSelections.AreaNIds + ")";
            }

            // Source NID filter
            if (!string.IsNullOrEmpty(userSelections.SourceNIds))
            {
                RetVal += " AND (D." + DIColumns.Data.SourceNId + ") IN (" + userSelections.SourceNIds + ")";
            }

            return RetVal;

        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUS(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.HighIsGood);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder);

                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod);

            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <param name="includeDI7Columns"></param> 
        /// <returns>Sql query string</returns>
        public string GetIUS(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection, bool includeDI7Columns)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist);

            if (includeDI7Columns)
            {
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod);
            }

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.HighIsGood);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder);

            }

            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            //if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            //{
            //    sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            //}

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUSByOrder(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);

            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            //if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            //{
            //    sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            //}

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUSByParentOrder(FilterFieldType filterFieldType, string filterText, ICType ICType, FieldSelection fieldSelection, string icNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);

            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
                sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);

                if (!string.IsNullOrEmpty(icNIds))
                {
                    sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " IN (" + icNIds + ")");
                }
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUSByOrder(FilterFieldType filterFieldType, string filterText, ICType ICType, FieldSelection fieldSelection, string icNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);

            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
                sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId);
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);

                if (!string.IsNullOrEmpty(icNIds))
                {
                    sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + icNIds + ")");
                }
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for the given  Indicator Nids,unit nids and subgroup nids
        /// </summary>        
        /// <param name="indicatorNIds">Comma separated Indicator Nids which may be blank</param>
        /// <param name="unitNids">Comma separated Unit Nids which may be blank</param>
        /// <param name="subgroupValNids">Comma separated SubgroupVal Nids which may be blank</param>            /// <returns>Sql query string</returns>
        public string GetIUSByI_U_S(string indicatorNIds, string unitNids, string subgroupValNids)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
            sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
            sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue);

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.Unit + " AS U");
            sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);


            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ")");
            }

            if (!string.IsNullOrEmpty(unitNids))
            {
                sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " IN (" + unitNids + ")");
            }

            if (!string.IsNullOrEmpty(subgroupValNids))
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " IN (" + subgroupValNids + ")");
            }



            //  ORDER BY Clause
            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for the given  Indicator Nids,unit nids and subgroup nids
        /// </summary>        
        /// <param name="indicatorNIds">Comma separated Indicator Nids which may be blank</param>
        /// <param name="unitNids">Comma separated Unit Nids which may be blank</param>
        /// <param name="subgroupValNids">Comma separated SubgroupVal Nids which may be blank</param>            /// <returns>Sql query string</returns>
        public string GetIUSByI_U_S(string indicatorNIds, string unitNids, string subgroupValNids, bool includeDI7Columns)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
            sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
            sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue);

            if (includeDI7Columns)
            {
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.Unit + " AS U");
            sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);


            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ")");
            }

            if (!string.IsNullOrEmpty(unitNids))
            {
                sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " IN (" + unitNids + ")");
            }

            if (!string.IsNullOrEmpty(subgroupValNids))
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " IN (" + subgroupValNids + ")");
            }



            //  ORDER BY Clause
            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be -1 if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUSByIC(ICType ICType, int ICNId, FieldSelection fieldSelection)
        {
            if (ICNId == -1)
            {
                return GetIUSByIC(ICType, string.Empty, fieldSelection);
            }
            else
            {
                return GetIUSByIC(ICType, ICNId.ToString(), fieldSelection);
            }

        }

        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType"></param>
        /// <param name="ICNId"></param>
        /// <param name="fieldSelection"></param>
        /// <param name="includeDI7Columns"></param>
        /// <returns></returns>
        public string GetIUSByIC(ICType ICType, int ICNId, FieldSelection fieldSelection, bool includeDI7Columns)
        {
            if (ICNId == -1)
            {
                return GetIUSByIC(ICType, string.Empty, fieldSelection, includeDI7Columns);
            }
            else
            {
                return GetIUSByIC(ICType, ICNId.ToString(), fieldSelection, includeDI7Columns);
            }

        }

        /// <summary>
        /// Get  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassifications.ICNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICNId.Length > 0)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        public string GetIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection, bool includeDI7Columns)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.HighIsGood);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassifications.ICNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            if (includeDI7Columns)
            {
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + ",IUS." + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod);
            }

            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICNId.Length > 0)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        public string GetAllIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassifications.ICNId + ", ICIUS." + DIColumns.IndicatorClassificationsIUS.RecommendedSource);
                sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICNId.Length > 0)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be -1 if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctIUSByIC(ICType ICType, int ICNId, FieldSelection fieldSelection)
        {
            if (ICNId == -1)
            {
                return GetDistinctIUSByIC(ICType, string.Empty, fieldSelection);
            }
            else
            {
                return GetDistinctIUSByIC(ICType, ICNId.ToString(), fieldSelection);
            }
        }

        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type and order on the basis of ICIUS order
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctOrderedIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {
            string sortOrderString = "ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder;
            return this.GetDistinctIUSByIC(ICType, ICNId, fieldSelection, sortOrderString);

        }

        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection)
        {
            string sortOrderString = "I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal;
            return this.GetDistinctIUSByIC(ICType, ICNId, fieldSelection, sortOrderString);

        }

        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctIUSByIC(ICType ICType, string ICNId, FieldSelection fieldSelection, string sortString)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICNId.Length > 0)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                if (string.IsNullOrEmpty(sortString) == false)
                {
                    sbQuery.Append(" ORDER BY " + sortString);
                    //"I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal
                }
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get distinct ordered NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetOrderedDistinctIUSByICParentNId(ICType ICType, int ICParentNId, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICParentNId >= -1)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " = " + ICParentNId);
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        public string GetOrderedDistinctAllIUS(ICType ICType, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            int ICParentNId = -1;

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId); //+ ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (ICParentNId >= -1)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " = " + ICParentNId);
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                // sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get distinct ordered NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table based on Indicator Classification Type
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetOrderedDistinctIUS(ICType ICType, string indicatorNIds, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);
            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ")");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect IUS based on TimePeriod and Area selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="ICNId">Indicator Classification NId of selected item in IC tree which may be -1 for all </param>
        /// <param name="fieldSelection">NId or Light</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByTimePeriodArea(string timePeriodNIds, string areaNIds, int ICNId, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // Set SELECT clause based on fieldSelection type
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",U." + DIColumns.Unit.UnitNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId);    //FieldSelection.NId
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }

            //FROM UT_Data AS D,UT_Indicator_Unit_Subgroup AS IUS,UT_Indicator_en AS I,UT_Unit_en AS U,UT_Subgroup_Vals_en AS SGV,UT_Indicator_Classifications_en AS IC,UT_Indicator_Classifications_IUS AS ICIUS,UT_Area_en AS A ,UT_TimePeriod AS T

            // Set FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I,");
            sbQuery.Append(this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");

            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.TimePeriod + " AS T");
            }

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Area + " AS A");
            }
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

            // Set WHERE clause 
            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);
            sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId);
            if (timePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId + " AND T." + DIColumns.Timeperiods.TimePeriodNId + " IN (" + timePeriodNIds + ")");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + " AND A." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }

            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);

            if (ICNId != -1)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " = " + ICNId);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect IUS based on TimePeriod and Area selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="fieldSelection">NId or Light</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByTimePeriodArea(string timePeriodNIds, string areaNIds, ICType ICType, FieldSelection fieldSelection)
        {
            string RetVal = String.Empty;
            // Get basic Sql clause from overloaded function
            RetVal = GetAutoSelectByTimePeriodArea(timePeriodNIds, areaNIds, -1, fieldSelection);

            // Append additional clause for ICType
            RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType];

            return RetVal;

        }


        /// <summary>
        /// AutoSelect Indiactor,unit,subgroup  based on TimePeriod, Area  and source selection and only for the given indicators nid
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="fieldSelection">NId, Light or Heavy</param>
        /// <param name="indicatorsNId">Comma delimited Indicators NId</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIUSByIndicator(string timePeriodNIds, string areaNIds, string sourceNids, FieldSelection fieldSelection, string indicatorsNId)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal +
                ",U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGId + ", U." + DIColumns.Unit.UnitGlobal +
                ",SG." + DIColumns.SubgroupVals.SubgroupVal + ", SG." + DIColumns.SubgroupVals.SubgroupValNId + ", SG." + DIColumns.SubgroupVals.SubgroupValGId + ", SG." + DIColumns.SubgroupVals.SubgroupValGlobal
                + " FROM " + this.TablesName.Indicator + " AS I, " + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SG , " + this.TablesName.IndicatorUnitSubgroup + " AS IUS "
                + " WHERE I." + DIColumns.Indicator.IndicatorNId + " IN(" + indicatorsNId + ") AND "
                + " I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId
                + " AND U." + DIColumns.Unit.UnitNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId
                + " AND SG." + DIColumns.SubgroupVals.SubgroupValNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId
                + " AND EXISTS (  SELECT *  FROM " + this.TablesName.Data + " as D WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId;

            if (!string.IsNullOrEmpty(timePeriodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timePeriodNIds + " ) ";
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNids))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids + " ) ";
            }


            RetVal += " ) ORDER BY I." + DIColumns.Indicator.IndicatorName + "";

            return RetVal;
        }

        /// <summary>
        /// Get IUSNIds based on IndicatorNIds, UnitNIds , SubgroupValNIds
        /// </summary>
        /// <param name="indicatorNIds">comma delimited IndicatorNIds which may be blank</param>
        /// <param name="unitNIds">comma delimited UnitNIds which may be blank</param>
        /// <param name="subgroupValNIds">comma delimited SubgroupValNIds which may be blank</param>
        /// <returns>If all three parameters are blank then resultent query will return all IUSNIds in IUS table</returns>
        public string GetIUSNIdByI_U_S(string indicatorNIds, string unitNIds, string subgroupValNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "," + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "," + DIColumns.Indicator_Unit_Subgroup.UnitNId + "," + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "," + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + "," + DIColumns.Indicator_Unit_Subgroup.DataExist);
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup);

            if (indicatorNIds == "" && unitNIds == "" && subgroupValNIds == "")
            {

            }
            else
            {

                sbQuery.Append(" WHERE 1=1 ");

                if (indicatorNIds.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNIds + ")");

                if (unitNIds.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.UnitNId + " IN (" + unitNIds + ")");

                if (subgroupValNIds.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " IN (" + subgroupValNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUSNIds based on Indicator, Unit , SubgroupVal
        /// </summary>
        /// <param name="indicatorName">IndicatorName which may be blank</param>
        /// <param name="unitName">UnitName which may be blank</param>
        /// <param name="subgroupVal">SubgroupVal which may be blank</param>
        /// <returns>If all three parameters are blank then resultent query will return all IUSNIds in IUS table</returns>
        public string GetIUSByI_U_S_Name(string indicatorName, string unitName, string subgroupVal, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);


            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");

                //  WHERE Clause
                sbQuery.Append(" WHERE 1=1 ");

                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);

                if (indicatorName.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.Indicator.IndicatorName + " ='" + indicatorName + "'");

                if (unitName.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.Unit.UnitName + " ='" + unitName + "'");

                if (subgroupVal.Trim().Length > 0)
                    sbQuery.Append(" AND " + DIColumns.SubgroupVals.SubgroupVal + " ='" + subgroupVal + "'");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        public string GetIUSGIds(string IUSNIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");

            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + IUSNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUSNID, IndicatorNId, UnitNId and SubgroupValNIds alongwith names against IndicatorGId_UnitGId_SubgroupGIds
        /// </summary>
        /// <param name="IndicatorGId_UnitGId_SubgroupGIds"></param>
        /// <param name="DIServerType"></param>
        /// <returns></returns>
        public string GetIUSNIds(string IndicatorGId_UnitGId_SubgroupGIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal);

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");

            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType) + " IN (" + IndicatorGId_UnitGId_SubgroupGIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUSNID, IndicatorNId, UnitNId and SubgroupValNIds alongwith names against IndicatorGId , UnitGId & SubgroupGId
        /// </summary>
        /// <param name="IndicatorGId_UnitGId_SubgroupGIds"></param>
        /// <param name="DIServerType"></param>
        /// <returns></returns>
        public string GetIUSNIdsByGID(string IndicatorGId, string UnitGId, string SubgroupValGId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",U." + DIColumns.Unit.UnitName + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal);

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");

            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorGId + "='" + DIQueries.RemoveQuotesForSqlQuery(IndicatorGId) + "' AND U." + DIColumns.Unit.UnitGId + "='" + DIQueries.RemoveQuotesForSqlQuery(UnitGId) + "' AND SGV." + DIColumns.SubgroupVals.SubgroupValGId + "='" + DIQueries.RemoveQuotesForSqlQuery(SubgroupValGId) + "' ");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal against which data is present and order by ICIUSOrder.
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctOrderedIUSWithData(int icNId, string IUSNIDs, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append(this.GetGenericSelectClause(fieldSelection));

            sbQuery.Append(",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);

            ////  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId + " ");
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);
            }

            if (!string.IsNullOrEmpty(IUSNIDs))
            {
                sbQuery.Append(" and IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in( " + IUSNIDs + " )");
            }
            else
            {
                sbQuery.Append(" and IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in(0) ");
            }

            if (icNId > 0)
            {
                sbQuery.Append(" and ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = " + icNId);
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get distinct NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal against which data is present.
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctIUSWithData(string IUSNIDs, FieldSelection fieldSelection)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //-- Select clause
            sbQuery.Append(this.GetGenericSelectClause(fieldSelection));

            ////  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId + " ");
            }

            if (!string.IsNullOrEmpty(IUSNIDs))
            {
                sbQuery.Append(" and " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in( " + IUSNIDs + " )");
            }
            else
            {
                sbQuery.Append(" and " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in(0) ");
            }

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns sql query to get distinct IUSNID from data table
        /// </summary>
        /// <returns></returns>
        public string GetDistinctIUSNIdFrmDataTable()
        {
            string RetVal = string.Empty;

            RetVal = " Select DISTINCT " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " FROM " + TablesName.Data;
            return RetVal;
        }

        /// <summary>
        /// Get distinct IUSNId  from Indicator_Classification against which data is present.
        /// </summary>
        /// <param name="ICType">Indicator Classification Type</param>
        /// <param name="ICNId">ICNId which may be blank if only ICType is to be considered</param>
        /// <returns>Sql query string</returns>
        public string GetDistinctIUSNidWithData(ICType ICType, string ICNId)
        {

            string RetVal = string.Empty;

            StringBuilder sbQuery = new StringBuilder();


            //  SELECT Clause
            sbQuery.Append("  SELECT DISTINCT ( ICIUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ") FROM ");
            sbQuery.Append(this.TablesName.IndicatorClassificationsIUS + " AS ICIUS ");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC ");
            sbQuery.Append("," + this.TablesName.Data + " AS D ");

            sbQuery.Append(" WHERE  ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + "= D." + DIColumns.Data.IUSNId + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType]);

            if (ICNId.Length > 0)
            {
                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN (" + ICNId + ")");
            }


            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the IUS NId on the basis of IndicatorNIDs
        /// </summary>
        /// <param name="indicatorNId">comma seprated Indicator NId</param>
        /// <returns>Query</returns>
        public string GetIUSFromIndicator(string indicatorNIds)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();

                //  SELECT Clause
                sbQuery.Append("  SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " FROM ");
                sbQuery.Append(this.TablesName.IndicatorUnitSubgroup + " AS IUS ");

                if (!string.IsNullOrEmpty(indicatorNIds))
                {
                    sbQuery.Append(" WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNIds + ")");
                }

                Retval = sbQuery.ToString();

            }
            catch (Exception ex)
            {
                Retval = string.Empty;
            }
            return Retval;
        }



        /// <summary>
        /// AutoSelect Indiactor,Unit & SubgroupVal based on TimePeriod, Area  and source selection
        /// </summary>
        /// <param name="timePeriodNIds">commma delimited Timeperiod NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">Source Nids which may be blank </param>
        /// <param name="ICParentNId">Set IC_Parent_NID to get records against the given NID otherwise set 0 to get all auto select records</param>
        /// <param name="ICType"></param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIUS(string timePeriodNIds, string areaNIds, string sourceNids, int ICParentNId, ICType ICType)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal
                + ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGId + ", U." + DIColumns.Unit.UnitGlobal
                + ", SG." + DIColumns.SubgroupVals.SubgroupValNId + ", SG." + DIColumns.SubgroupVals.SubgroupVal + ", SG." + DIColumns.SubgroupVals.SubgroupValGId + ", SG." + DIColumns.SubgroupVals.SubgroupValGlobal
               + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId
                + " FROM " + this.TablesName.Indicator
                + " AS I  ," + this.TablesName.Unit + " AS U ," + this.TablesName.SubgroupVals + " AS SG,  " + this.TablesName.IndicatorUnitSubgroup
                + " AS IUS WHERE I." + DIColumns.Indicator.IndicatorNId + "= IUS." + DIColumns.Indicator.IndicatorNId
                + " AND U." + DIColumns.Unit.UnitNId + "=IUS." + DIColumns.Unit.UnitNId
                + " AND SG." + DIColumns.SubgroupVals.SubgroupValNId + "=IUS." + DIColumns.SubgroupVals.SubgroupValNId
                + " AND EXISTS (  SELECT *  FROM " + this.TablesName.Data + " as D WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "= D." + DIColumns.Data.IUSNId;

            if (!string.IsNullOrEmpty(timePeriodNIds))
            {
                RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN( " + timePeriodNIds + " ) ";
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNids))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNids + " ) ";
            }
            RetVal += " )";

            RetVal += " AND EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = ICIUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND  EXISTS ( SELECT * FROM " + this.TablesName.IndicatorClassifications + " IC  WHERE ICIUS." + DIColumns.IndicatorClassifications.ICNId + " =IC." + DIColumns.IndicatorClassifications.ICNId;

            if (ICParentNId > 0)
            {
                RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICParent_NId + " =" + ICParentNId + " ";
            }

            RetVal += " AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType] + " )) ";


            RetVal += " ORDER BY I." + DIColumns.Indicator.IndicatorName + "";

            return RetVal;
        }

        /// <summary>
        /// Query return auto selected IUS
        /// </summary>
        /// <param name="showIUS"></param>
        /// <param name="indicatorNIds"></param>
        /// <param name="areaNIds"></param>
        /// <param name="timePeriodNIds"></param>
        /// <param name="sourceNIds"></param>
        /// <returns></returns>
        public string GetDataWAutoSelectedIUS(bool showIUS, string indicatorNIds, string areaNIds, string timePeriodNIds, string sourceNIds)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IUSNId + ", I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName);
                sbQuery.Append(", SGV." + DIColumns.SubgroupVals.SubgroupValNId + ", SGV." + DIColumns.SubgroupVals.SubgroupVal);

                sbQuery.Append(" FROM " + this.TablesName.Indicator + " I," + this.TablesName.SubgroupVals + " SGV,");
                sbQuery.Append(this.TablesName.Unit + " U," + this.TablesName.Data + " D ");

                sbQuery.Append(" WHERE ");
                sbQuery.Append(" U." + DIColumns.Unit.UnitNId + " = D." + DIColumns.Data.UnitNId + " AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = D." + DIColumns.Data.SubgroupValNId);
                sbQuery.Append(" AND  I." + DIColumns.Indicator.IndicatorNId + " = D." + DIColumns.Data.IndicatorNId);

                if (showIUS && !string.IsNullOrEmpty(indicatorNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + indicatorNIds + ")");
                }
                else if (!string.IsNullOrEmpty(indicatorNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNIds + ")");
                }

                if (!string.IsNullOrEmpty(areaNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");
                }

                if (!string.IsNullOrEmpty(timePeriodNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");
                }

                if (!string.IsNullOrEmpty(sourceNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");
                }

                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal);

                Retval = sbQuery.ToString();
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        public string CheckExistenceOfIUSInData(string iusNIds, string areaNIds, string timePeriodNIds, string sourceNIds)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();
                sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.IUSNId);

                sbQuery.Append(" FROM " + this.TablesName.Data + " D");

                sbQuery.Append(" WHERE 1=1 ");

                if (!string.IsNullOrEmpty(iusNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + iusNIds + ")");
                }

                if (!string.IsNullOrEmpty(areaNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");
                }

                if (!string.IsNullOrEmpty(timePeriodNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNIds + ")");
                }

                if (!string.IsNullOrEmpty(sourceNIds))
                {
                    sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ")");
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
        /// Get Unmatched IUS
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedIUS()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS1 WHERE NOT EXISTS (");

            sbQuery.Append(" SELECT * FROM " + this.TablesName.Indicator + "  AS I ");

            sbQuery.Append(" WHERE  I." + DIColumns.Indicator.IndicatorNId + " = IUS1." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);

            sbQuery.Append(" ) AND NOT EXISTS  ( SELECT * FROM " + this.TablesName.Unit + "  AS U ");

            sbQuery.Append(" WHERE  U.Unit_NId = IUS1." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            sbQuery.Append(" )AND NOT EXISTS (SELECT * FROM " + this.TablesName.SubgroupVals + "  AS S ");

            sbQuery.Append(" WHERE S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS1." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " )");

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// GetDuplicate IUS embedded with Count(*)
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIUS()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName);
            sbQuery.Append(", IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",U." + DIColumns.Unit.UnitName);
            sbQuery.Append(", IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ", S." + DIColumns.SubgroupVals.SubgroupVal);

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Indicator + " I,");
            sbQuery.Append(this.TablesName.Unit + " U," + this.TablesName.SubgroupVals + " S, ");
            sbQuery.Append("(SELECT " + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ", " + DIColumns.Indicator_Unit_Subgroup.UnitNId + ", " + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup);
            sbQuery.Append(" GROUP BY " + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " ,");
            sbQuery.Append(DIColumns.Indicator_Unit_Subgroup.UnitNId + "," + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(" HAVING COUNT(*)>1  ) AS DIUS ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = S." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND DIUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND DIUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND DIUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = S." + DIColumns.SubgroupVals.SubgroupValNId);
            RetVal = sbQuery.ToString();

            return RetVal;
        }


        /// <summary>
        /// Get Unmatched IUS(Where Indicator exists in IUS table but not in Indicator table)
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedIndicator()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE NOT EXISTS ( ");

            sbQuery.Append(" SELECT * FROM " + this.TablesName.Indicator + "  AS I ");

            sbQuery.Append(" WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + ")");

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Where Unit exists in IUS table but not in Unit table
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedUnit()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE NOT EXISTS ( ");

            sbQuery.Append(" SELECT * FROM " + this.TablesName.Unit + "  AS U ");

            sbQuery.Append(" WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId + ")");

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Where SubgroupVal exists in IUS table but not in SubgroupVal table
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedSubgroup()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE NOT EXISTS ( ");

            sbQuery.Append(" SELECT * FROM " + this.TablesName.SubgroupVals + "  AS SG ");

            sbQuery.Append(" WHERE  IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId + ")");

            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Returns the sql query to get auto Indicators with units 
        /// </summary>
        /// <returns></returns>
        public string GetAutoIndicatorNUnit()
        {
            string RetVal = string.Empty;

            RetVal = " SELECT Distinct I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId +
                ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGId + ", U." + DIColumns.Unit.UnitGlobal + ", I." + DIColumns.Indicator.IndicatorGlobal + " FROM (" + this.TablesName.Indicator +
                " AS I INNER JOIN " + this.TablesName.Data +
                " AS D ON I." + DIColumns.Indicator.IndicatorNId + "= D." + DIColumns.Indicator.IndicatorNId + ") INNER JOIN " + this.TablesName.Unit + " AS U ON D." + DIColumns.Unit.UnitNId + " = U." + DIColumns.Unit.UnitNId + " ";

            return RetVal;
        }


        /// <summary>
        /// Returns the sql query to get auto Subgroup vals for the given indicator and unit
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public string GetAutoSubgroupValByIU(int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT distinct SG." + DIColumns.SubgroupVals.SubgroupValNId + ", SG." + DIColumns.SubgroupVals.SubgroupVal + ", SG." + DIColumns.SubgroupVals.SubgroupValGId + ", SG." + DIColumns.SubgroupVals.SubgroupValGlobal +
                " FROM " + this.TablesName.Data + " AS D INNER JOIN " + this.TablesName.SubgroupVals + " AS SG ON D." + DIColumns.SubgroupVals.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId +
                " where D." + DIColumns.Indicator.IndicatorNId + "=" + indicatorNId.ToString() +
                " and D." + DIColumns.Unit.UnitNId + "=" + unitNId.ToString();
            ;
            return RetVal;

        }

        /// <summary>
        /// Returns the sql query to get Subgroup vals for the given indicator and unit
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public string GetSubgroupValByIU(int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ", SG." + DIColumns.SubgroupVals.SubgroupValNId + ", SG." + DIColumns.SubgroupVals.SubgroupVal + ", SG." + DIColumns.SubgroupVals.SubgroupValGId + ", SG." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + ", IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids +
                " FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN " + this.TablesName.SubgroupVals + " AS SG ON IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId +
                " where IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "=" + indicatorNId.ToString() +
                " and IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + "=" + unitNId.ToString();
            ;
            return RetVal;

        }

        /// <summary>
        /// Returns the sql query to get auto indicators with units for the given indicator classification
        /// </summary>
        /// <param name="icNId"></param>
        /// <returns></returns>
        public string GetAutoIndicatorNUnitByIC(int icNId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ", I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGId + ", I." + DIColumns.Indicator.IndicatorGlobal +
                ", U." + DIColumns.Unit.UnitNId + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGId + ", U." + DIColumns.Unit.UnitGlobal +
                " FROM ((" + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS INNER JOIN " +
                this.TablesName.Data + " AS D ON ICIUS." +
                DIColumns.IndicatorClassificationsIUS.IUSNId + " = D." + DIColumns.Data.IUSNId +
                ") INNER JOIN " + this.TablesName.Indicator +
                " AS I ON D." + DIColumns.Indicator.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId + ") INNER JOIN " + this.TablesName.Unit +
                " AS U ON D." + DIColumns.Unit.UnitNId + " = U." + DIColumns.Unit.UnitNId +
                " WHERE (((ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + ") =" + icNId + ")) ";

            return RetVal;
        }

        /// <summary>
        /// Returns the sql query to get subgroup dimension values with subgroup dimension for the given indicator and unit
        /// </summary>
        /// <param name="indicatorNid"></param>
        /// <param name="unitNid"></param>
        /// <returns></returns>
        public string GetAutoSubgroupDetailsByIU(int indicatorNid, int unitNid)
        {
            string RetVal = string.Empty;
            RetVal = " SELECT DISTINCT SGDV." + DIColumns.Subgroup.SubgroupName + ", SGDV." + DIColumns.Subgroup.SubgroupGlobal + ", SGDV." + DIColumns.Subgroup.SubgroupGId + ", SGDV." + DIColumns.Subgroup.SubgroupNId +
                ", SGD." + DIColumns.SubgroupTypes.SubgroupTypeNId + ", SGD." + DIColumns.SubgroupTypes.SubgroupTypeName + ", SGD." + DIColumns.SubgroupTypes.SubgroupTypeOrder + ", SGDV." + DIColumns.Subgroup.SubgroupOrder +
                " FROM (" + this.TablesName.Subgroup + " AS SGDV INNER JOIN ((" + this.TablesName.Data +
                " AS D INNER JOIN " + this.TablesName.SubgroupVals +
                " AS SG ON D." + DIColumns.SubgroupVals.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId +
                ") INNER JOIN " + this.TablesName.SubgroupValsSubgroup + " AS SVS ON SG." + DIColumns.SubgroupVals.SubgroupValNId + " = SVS." + DIColumns.SubgroupVals.SubgroupValNId +
                ") ON SGDV." + DIColumns.Subgroup.SubgroupNId + " = SVS." + DIColumns.Subgroup.SubgroupNId +
                ") INNER JOIN " + this.TablesName.SubgroupType + " AS SGD ON SGDV." +
                DIColumns.Subgroup.SubgroupType + " = SGD." + DIColumns.SubgroupTypes.SubgroupTypeNId +
                " WHERE (((D." + DIColumns.Indicator.IndicatorNId + ")=" + indicatorNid.ToString() + ") AND ((D." + DIColumns.Unit.UnitNId + ")=" + unitNid.ToString() + ")) ";

            return RetVal;
        }

        /// <summary>
        /// Returns the optimized sql query to get subgroup dimension values with subgroup dimension for the given indicator and unit
        /// </summary>
        /// <param name="indicatorNid"></param>
        /// <param name="unitNid"></param>
        /// <returns></returns>
        public string GetAutoSelectSubgroupDetailsByIU(int indicatorNid, int unitNid)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT DISTINCT S." + DIColumns.Subgroup.SubgroupName + ", S." + DIColumns.Subgroup.SubgroupGlobal + ", S." + DIColumns.Subgroup.SubgroupGId + ", S." + DIColumns.Subgroup.SubgroupNId +
                ", ST." + DIColumns.SubgroupTypes.SubgroupTypeNId + ", ST." + DIColumns.SubgroupTypes.SubgroupTypeName + ", ST." + DIColumns.SubgroupTypes.SubgroupTypeOrder + ", S." + DIColumns.Subgroup.SubgroupOrder + ", IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ", IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + ", IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId +
                " FROM (" + this.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN (" + this.TablesName.Subgroup + " AS S INNER JOIN " + this.TablesName.SubgroupValsSubgroup + " AS SVS ON S." + DIColumns.Subgroup.SubgroupNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId +
                " ) ON IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SVS." + DIColumns.SubgroupVals.SubgroupValNId + ") INNER JOIN " + this.TablesName.SubgroupType + " AS ST ON S." + DIColumns.Subgroup.SubgroupType + " = ST." + DIColumns.SubgroupTypes.SubgroupTypeNId +
                " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = " + indicatorNid + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = " + unitNid;

            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of showIUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetIndicatorUnit(string iusNId, bool showIUS)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorGId);
            sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);


            if (showIUS)
            {
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Indicator + " I," + TablesName.Unit + " U");

            if (showIUS)
            {
                sbQuery.Append("," + TablesName.SubgroupVals + " SGV");
            }

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (showIUS)
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            }

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");

            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of showIUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetIndicatorUnit(string iusNId, string ICNId, bool showIUS)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorGId);
            sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);


            if (showIUS)
            {
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            }

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Indicator + " I," + TablesName.Unit + " U");

            if (showIUS)
            {
                sbQuery.Append("," + TablesName.SubgroupVals + " SGV," + TablesName.IndicatorClassificationsIUS + " ICIUS");
            }

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (showIUS)
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);
            }

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");

            if (showIUS)
            {
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " IN (" + ICNId + ")");
            }

            sbQuery.Append(" ORDER BY ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSOrder);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of showIUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetDistinctIndicatorUnit(string iusNId, bool showIUS)
        {

            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT I." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorGId);
            sbQuery.Append(",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist);


            if (showIUS)
            {
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
            }

            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Indicator + " I," + TablesName.Unit + " U");

            if (showIUS)
            {
                sbQuery.Append("," + TablesName.SubgroupVals + " SGV");
            }

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (showIUS)
            {
                sbQuery.Append(" AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            }

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");

            sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from Indicator against which data is present.
        /// </summary>
        /// <param name="fieldSelection">Use heavy for all fields or use light to exclude IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetIndicatorUnitWithData(string IUSNIDs, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();


            //  SELECT Clause
            sbQuery.Append(" SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE 1=1 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + "= U." + DIColumns.Unit.UnitNId + "   and ");
            }

            if (string.IsNullOrEmpty(IUSNIDs))
            {
                IUSNIDs = "0";
            }

            sbQuery.Append(" " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " in( " + IUSNIDs + " )");

            //  ORDER BY Clause
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the IUS NId on the basis of IndicatorNIDs
        /// </summary>
        /// <param name="indicatorNId">comma seprated Indicator NId</param>
        /// <returns>Query</returns>
        public string GetIUSFromIndicatorUnit(string IndicatorNId_UnitNid, DIServerType DIServerType)
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder sbQuery = new StringBuilder();

                //  SELECT Clause
                sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " FROM ");
                sbQuery.Append(this.TablesName.IndicatorUnitSubgroup + " AS IUS ");

                if (!string.IsNullOrEmpty(IndicatorNId_UnitNid))
                {
                    sbQuery.Append(" WHERE ");
                    sbQuery.Append(DIQueries.SQL_GetConcatenatedValues("IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType) + " IN (" + IndicatorNId_UnitNid + ")");
                }

                Retval = sbQuery.ToString();

            }
            catch (Exception ex)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Get Common UnitNid by IndicatorNIds
        /// </summary>
        /// <param name="indicatorNIds"></param>
        /// <param name="indicatorCount"></param>
        /// <returns></returns>
        /// <remarks>//Select unit_name from
        ///(SELECT distinct U.Unit_Name, IUS.Indicator_NId
        ///FROM UT_Unit_en AS U INNER JOIN UT_Indicator_Unit_Subgroup AS IUS ON U.Unit_NId = IUS.Unit_NId
        ///where IUS.Indicator_Nid in (306)) as UnitTable
        ///group by Unit_Name having count(*)=1
        ///</remarks>
        public string GetCommonUnitNIdByIndicators(string indicatorNIds, int indicatorCount)
        {
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT " + DIColumns.Unit.UnitNId + " FROM ");

            Sb.Append(" ( SELECT DISTINCT U." + DIColumns.Unit.UnitNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            Sb.Append(" FROM " + this.TablesName.Unit + " AS U  INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON  U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (!string.IsNullOrEmpty(indicatorNIds))
                Sb.Append(" WHERE ");

            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                Sb.Append(" IUS." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            Sb.Append(" ) as UnitTable GROUP BY " + DIColumns.Unit.UnitNId + " HAVING COUNT(*) = " + indicatorCount);

            return Sb.ToString();
        }

        /// <summary>
        /// Get All UnitNid by IndicatorNIds
        /// </summary>
        /// <param name="indicatorNIds"></param>
        /// <returns></returns>
        /// <remarks>
        /// select unitTable.Unit_Name,count(unitTable.Unit_Name ) from (
        /// select distinct I.Indicator_Name, U.Unit_Name  from ut_indicator_unit_subgroup as IUS ,
        /// ut_indicator_en as I 
        /// ,ut_unit_en as U 
        /// where I.Indicator_Nid = IUS.Indicator_Nid and U.Unit_Nid = IUS.Unit_Nid and IUS.indicator_nid in(23,316,240)
        /// ) as unitTable 
        /// group by unitTable.Unit_Name
        /// having count(unitTable.Unit_Name)>1
        ///</remarks>
        public string GetCommonUnitsByIndicatorNid(string indicatorNIds, int IndicatorCount)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitGId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGlobal + " FROM ");

            RetVal.Append(" ( SELECT DISTINCT IUS." + DIColumns.Indicator.IndicatorNId + ",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal);
            RetVal.Append(" FROM " + this.TablesName.Unit + " AS U  INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON  U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                RetVal.Append(" WHERE IUS." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            RetVal.Append(" ) as UnitTable GROUP BY " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + "," + DIColumns.Unit.UnitGlobal + " HAVING COUNT(*) = " + IndicatorCount);

            return RetVal.ToString();
        }

        /// <summary>
        /// Get Common Subgroup By Indicator and Unit
        /// </summary>
        /// <param name="indicatorNIds"></param>
        /// <param name="unitNids"></param>
        /// <param name="indicatorCount"></param>
        /// <returns></returns>
        public string GetCommonSubgroupValByIndicatorUnit(string indicatorNIds, string unitNids, int indicatorCount)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT " + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupValGlobal + "," + DIColumns.SubgroupVals.SubgroupValOrder + " FROM ");

            Sb.Append(" ( SELECT DISTINCT S." + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.SubgroupVals.SubgroupValNId + ",S." + DIColumns.SubgroupVals.SubgroupValOrder + ",S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            Sb.Append(" FROM " + this.TablesName.SubgroupVals + " AS S  INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON  S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (!string.IsNullOrEmpty(indicatorNIds) && !string.IsNullOrEmpty(unitNids))
                Sb.Append(" WHERE ");

            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                Sb.Append(" IUS." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            if (!string.IsNullOrEmpty(indicatorNIds) && !string.IsNullOrEmpty(unitNids))
                Sb.Append(" AND ");

            if (!string.IsNullOrEmpty(unitNids))
            {
                Sb.Append(" IUS." + DIColumns.Unit.UnitNId + " IN(" + unitNids + " ) ");
            }

            Sb.Append(" ) AS SubgroupTable ");

            Sb.Append(" GROUP BY " + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupValOrder + "," + DIColumns.SubgroupVals.SubgroupValGlobal + " HAVING COUNT(*) = " + indicatorCount);

            Sb.Append(" order by " + DIColumns.SubgroupVals.SubgroupValOrder);
            RetVal = Sb.ToString();
            return RetVal;

        }

        public string GetCommonSubgroupsByIndicatorUnit(string indicatorNIds, string unitNids, int indicatorCount)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGlobal + "," + DIColumns.SubgroupVals.SubgroupValOrder + " FROM ");

            RetVal.Append(" ( SELECT DISTINCT IUS." + DIColumns.Indicator.IndicatorNId + ",S." + DIColumns.SubgroupVals.SubgroupValNId + ",S." + DIColumns.SubgroupVals.SubgroupValGId + ",S." + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.SubgroupVals.SubgroupValOrder + ",S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            RetVal.Append(" FROM " + this.TablesName.SubgroupVals + " AS S  INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON  S." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (!string.IsNullOrEmpty(indicatorNIds) || !string.IsNullOrEmpty(unitNids))
                RetVal.Append(" WHERE ");

            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                RetVal.Append(" IUS." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            if (!string.IsNullOrEmpty(indicatorNIds) && !string.IsNullOrEmpty(unitNids))
                RetVal.Append(" AND ");

            if (!string.IsNullOrEmpty(unitNids))
            {
                RetVal.Append(" IUS." + DIColumns.Unit.UnitNId + " IN(" + unitNids + " ) ");
            }

            RetVal.Append(" ) AS SubgroupTable ");

            RetVal.Append(" GROUP BY " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValOrder + "," + DIColumns.SubgroupVals.SubgroupValGlobal + " HAVING COUNT(*) > " + indicatorCount);

            RetVal.Append(" order by " + DIColumns.SubgroupVals.SubgroupValOrder);
            
            return RetVal.ToString();

        }

        /// <summary>
        /// Returns sql query to get IUSNIds with subgroup nids from IUS table
        /// </summary>
        /// <returns></returns>
        public string GetIUSNidsWSubgroupNIds()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT SVS." + DIColumns.Subgroup.SubgroupNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN " + this.TablesName.SubgroupValsSubgroup + " AS SVS ON IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " ORDER BY  IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " ";

            return RetVal;
        }

        /// <summary>
        /// Get the auto selected  NId, Name, GID, Global from IUS - Indicator - Unit - SubgroupVal table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId</param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection">Use Light for NId + Name + GId + Global fields use heavy to include IndicatorInfo field</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectIUS(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            //  SELECT Clause
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal);
                sbQuery.Append(",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal);
                sbQuery.Append(",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal);
                sbQuery.Append(",IUS." + DIColumns.Indicator_Unit_Subgroup.MinValue + ",IUS." + DIColumns.Indicator_Unit_Subgroup.MaxValue + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder);

            }
            if (fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(",I." + DIColumns.Indicator.IndicatorInfo);
            }

            //  FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I");
                sbQuery.Append("," + this.TablesName.Unit + " AS U");
                sbQuery.Append("," + this.TablesName.SubgroupVals + " AS SGV");
            }

            //  WHERE Clause
            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + " <> 0 ");
            if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            }

            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
            {

                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(" AND " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + filterText + ")");
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
                        sbQuery.Append(filterText);
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

            //  ORDER BY Clause
            //if (fieldSelection == FieldSelection.Light || fieldSelection == FieldSelection.Heavy)
            //{
            //    sbQuery.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName);
            //}

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the auto selected IUS on the basis of indicator, area, timeperiod and source NIDs
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="areaNId"></param>
        /// <param name="timePeriodNId"></param>
        /// <param name="sourceNId"></param>
        /// <returns></returns>
        public string GetAutoSelectIUS(string indicatorNId, string unitNId, string areaNId, string timePeriodNId, string sourceNId)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist;
                RetVal += ",D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.AreaNId;

                RetVal += " FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS INNER JOIN " + this.TablesName.Data + " D ON";
                RetVal += " IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId;

                RetVal += " WHERE 1=1 ";

                if (!string.IsNullOrEmpty(indicatorNId))
                {
                    RetVal += " AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNId + ")";
                }

                if (!string.IsNullOrEmpty(unitNId))
                {
                    RetVal += " AND D." + DIColumns.Data.UnitNId + " IN (" + unitNId + ")";
                }

                if (!string.IsNullOrEmpty(areaNId))
                {
                    RetVal += " AND D." + DIColumns.Data.AreaNId + " IN (" + areaNId + ")";
                }

                if (!string.IsNullOrEmpty(timePeriodNId))
                {
                    RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNId + ")";
                }

                if (!string.IsNullOrEmpty(sourceNId))
                {
                    RetVal += " AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNId + ")";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the IUS on the basis of indicator, area, timeperiod and source NIDs
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="areaNId"></param>
        /// <param name="timePeriodNId"></param>
        /// <param name="sourceNId"></param>
        /// <returns></returns>
        public string GetIUS(string indicatorNId, string unitNId, string areaNId, string timePeriodNId, string sourceNId)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId;
                RetVal += ",I." + DIColumns.Indicator.IndicatorName + ",I." + DIColumns.Indicator.IndicatorGId + ",I." + DIColumns.Indicator.IndicatorGlobal;
                RetVal += ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder;

                RetVal += " FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Data + " D," + this.TablesName.Indicator + " I," + this.TablesName.SubgroupVals + " SGV";

                RetVal += " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId;
                RetVal += " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId;
                RetVal += " AND SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId;

                if (!string.IsNullOrEmpty(indicatorNId))
                {
                    RetVal += " AND D." + DIColumns.Data.IndicatorNId + " IN (" + indicatorNId + ")";
                }

                if (!string.IsNullOrEmpty(unitNId))
                {
                    RetVal += " AND D." + DIColumns.Data.UnitNId + " IN (" + unitNId + ")";
                }

                if (!string.IsNullOrEmpty(areaNId))
                {
                    RetVal += " AND D." + DIColumns.Data.AreaNId + " IN (" + areaNId + ")";
                }

                if (!string.IsNullOrEmpty(timePeriodNId))
                {
                    RetVal += " AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timePeriodNId + ")";
                }

                if (!string.IsNullOrEmpty(sourceNId))
                {
                    RetVal += " AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNId + ")";
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get IUSNIds based on IndicatorNIds, UnitNIds , SubgroupNIds
        /// </summary>
        /// <param name="indicatorNIds">comma delimited IndicatorNIds which may be blank</param>
        /// <param name="unitNIds">comma delimited UnitNIds which may be blank</param>
        /// <param name="subgroupValNIds">comma delimited SubgroupNIds which may be blank</param>
        /// <returns>If all three parameters are blank then resultent query will return all IUSNIds in IUS table</returns>
        public string GetIUSNIdByI_U_SubgroupNIds(string indicatorNIds, string unitNIds, string subgroupNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId);
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupValsSubgroup + " SVS");
            sbQuery.Append(" WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (indicatorNIds == "" && unitNIds == "" && subgroupNIds == "")
            {

            }
            else
            {
                if (indicatorNIds.Trim().Length > 0)
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNIds + ")");

                if (unitNIds.Trim().Length > 0)
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " IN (" + unitNIds + ")");

                if (subgroupNIds.Trim().Length > 0)
                    sbQuery.Append(" AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + subgroupNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUSNIds based on IndicatorNIds, UnitNIds , SubgroupNIds
        /// </summary>
        /// <param name="indicatorNIds">comma delimited IndicatorNIds which may be blank</param>
        /// <param name="unitNIds">comma delimited UnitNIds which may be blank</param>
        /// <param name="subgroupValNIds">comma delimited SubgroupNIds which may be blank</param>
        /// <returns>If all three parameters are blank then resultent query will return all IUSNIds in IUS table</returns>
        public string GetIUSByI_U_SubgroupNIds(string indicatorNIds, string unitNIds, string subgroupNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + ",SV." + DIColumns.SubgroupVals.SubgroupVal);
            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupValsSubgroup + " SVS," + this.TablesName.SubgroupVals + " SV");
            sbQuery.Append(" WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);
            sbQuery.Append(" AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = SV." + DIColumns.SubgroupVals.SubgroupValNId);

            if (indicatorNIds == "" && unitNIds == "" && subgroupNIds == "")
            {

            }
            else
            {
                if (indicatorNIds.Trim().Length > 0)
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNIds + ")");

                if (unitNIds.Trim().Length > 0)
                    sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " IN (" + unitNIds + ")");

                if (subgroupNIds.Trim().Length > 0)
                    sbQuery.Append(" AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + subgroupNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IUS based on IndicatorNIds, UnitNIds , SubgroupValNIds
        /// </summary>
        /// <param name="indicatorNIds">comma delimited IndicatorNIds which may be blank</param>
        /// <param name="unitNIds">comma delimited UnitNIds which may be blank</param>
        /// <param name="subgroupValNIds">comma delimited SubgroupValNIds which may be blank</param>
        /// <returns>If all three parameters are blank then resultent query will return all IUSNIds in IUS table</returns>
        public string GetIUSByI_U_SV(string indicatorNIds, string unitNIds, string subgroupValNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + ",IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist);
            sbQuery.Append(",I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SV." + DIColumns.SubgroupVals.SubgroupVal);

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupVals + " SV," + this.TablesName.Indicator + " I," + this.TablesName.Unit + " U");
            sbQuery.Append(" WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SV." + DIColumns.SubgroupVals.SubgroupValNId);


            if (indicatorNIds == "" && unitNIds == "" && subgroupValNIds == "")
            {

            }
            else
            {
                if (indicatorNIds.Trim().Length > 0)
                    sbQuery.Append(" AND I." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " IN (" + indicatorNIds + ")");

                if (unitNIds.Trim().Length > 0)
                    sbQuery.Append(" AND U." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " IN (" + unitNIds + ")");

                if (subgroupValNIds.Trim().Length > 0)
                    sbQuery.Append(" AND SV." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " IN (" + subgroupValNIds + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Getting Duplicate IUS Records By IUSNid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateIUSByIUSNid()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ",IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

                RetVal.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS");

                RetVal.Append(" WHERE EXISTS ( ");

                RetVal.Append("SELECT IUS1." + DIColumns.Indicator_Unit_Subgroup.IUSNId);

                RetVal.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS1");

                RetVal.Append(" GROUP BY IUS1." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " HAVING COUNT(*)>1 )");
            }
            catch (Exception)
            {
                RetVal.Length = 0;
            }

            return RetVal.ToString();
        }
        public string GetDuplicateICIUSByNid()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + ",ICIUS." + DIColumns.IndicatorClassificationsIUS.ICIUSLabel);

                RetVal.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS");

                RetVal.Append(" WHERE EXISTS ( ");

                RetVal.Append("SELECT ICIUS1." + DIColumns.IndicatorClassificationsIUS.ICIUSNId);

                RetVal.Append(" FROM " + this.TablesName.IndicatorClassificationsIUS + " ICIUS1");

                RetVal.Append(" GROUP BY ICIUS1." + DIColumns.IndicatorClassificationsIUS.ICIUSNId + " HAVING COUNT(*)>1 )");
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
