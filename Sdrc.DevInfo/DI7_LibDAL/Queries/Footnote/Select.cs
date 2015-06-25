using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Footnote
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
        /// Get  NId, Name, GID, Global from Unit table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId,GId,Name,Search,NIdNotIn,NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be FootNotes.FootNote LIKE '%Sample FootNote%'</param>
        /// <returns></returns>
        public string GetFootnote(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.FootNotes.FootNoteNId + "," + DIColumns.FootNotes.FootNote + "," + DIColumns.FootNotes.FootNoteGId);

            sbQuery.Append(" FROM " + this.TablesName.FootNote);

            if (filterFieldType != FilterFieldType.None)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.FootNotes.FootNoteNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.FootNotes.FootNoteGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.FootNotes.FootNote + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Type:
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.FootNotes.FootNoteNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.FootNotes.FootNote + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the footnotes on the basis of DataNIds
        /// </summary>
        /// <param name="dataNIds">Comma seprated Data NIds </param>
        /// <returns></returns>
        public string GetFootnoteFromDataNId(string dataNIds)
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append(" SELECT F." + DIColumns.FootNotes.FootNote + ", F." + DIColumns.FootNotes.FootNoteNId + ", F." + DIColumns.FootNotes.FootNoteGId);

                RetVal.Append(" FROM " + TablesName.Data + " D," + TablesName.FootNote + " F");
                RetVal.Append(" WHERE D." + DIColumns.Data.FootNoteNId + " = F." + DIColumns.FootNotes.FootNoteNId);

                if (!string.IsNullOrEmpty(dataNIds))
                {
                    RetVal.Append(" AND D." + DIColumns.Data.DataNId + " IN (" + dataNIds + ")");
                }
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }


        /// <summary>
        /// Returns  a query to get unmatched footnote nids from data table
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedFootnoteNidFormData()
        {
            string RetVal = string.Empty;

            RetVal = " Select  " + DIColumns.FootNotes.FootNoteNId + " FROM " + this.TablesName.FootNote
                + "  where " + DIColumns.FootNotes.FootNoteNId + "  not in ( SELECT distinct D." + DIColumns.FootNotes.FootNoteNId
                + " FROM " + this.TablesName.Data + " AS D INNER JOIN " + this.TablesName.FootNote + "  AS F ON F." + DIColumns.FootNotes.FootNoteNId + " =D." + DIColumns.FootNotes.FootNoteNId + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns the query to get footnotes  with indicator,unit, subgroupVal, area, timeperiod, source and data value.
        /// </summary>
        /// <returns></returns>
        public string GetFootNotesWithDataValue()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ", T." + DIColumns.Timeperiods.TimePeriod + ", A." + DIColumns.Area.AreaName + ", D." + DIColumns.Data.DataValue + ", IC." + DIColumns.IndicatorClassifications.ICName + ", F." + DIColumns.FootNotes.FootNote
                + " FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS, "
                + this.TablesName.Indicator + " AS I, "
                + this.TablesName.Unit + " AS U,"
                + this.TablesName.SubgroupVals + " AS SGV, "
                + this.TablesName.TimePeriod + " AS T, "
                + this.TablesName.Area + " AS A,"
                + this.TablesName.IndicatorClassifications + " AS IC, "
                + this.TablesName.Data + " AS D, "
                + this.TablesName.FootNote + " AS F "
                + " WHERE  D." + DIColumns.Data.IUSNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.UnitNId + "= U." + DIColumns.Unit.UnitNId + " AND IUS."
                + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + "= SGV." + DIColumns.SubgroupVals.SubgroupValNId + " AND D."
                + DIColumns.Data.FootNoteNId + " = F." + DIColumns.FootNotes.FootNoteNId + " AND D."
                + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId + " AND D."
                + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId + " AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId
                + " AND D." + DIColumns.Data.FootNoteNId + " > 0 " + " ORDER BY I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", SGV." + DIColumns.SubgroupVals.SubgroupVal + ", D." + DIColumns.Data.DataValue;


            return RetVal;
        }

        /// <summary>
        /// Returns the footnotes with time periodNID
        /// </summary>
        /// <returns></returns>
        public string GetFootnoteWithTimePeriod()
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = " SELECT F." + DIColumns.FootNotes.FootNoteNId + ",F." + DIColumns.FootNotes.FootNote + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.DataNId;
                RetVal += " FROM " + this.TablesName.FootNote + " F," + this.TablesName.Data + " D ";
                RetVal += " WHERE F." + DIColumns.FootNotes.FootNoteNId + " = D." + DIColumns.Data.FootNoteNId;
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

   

        #endregion

        #endregion

    }
}
