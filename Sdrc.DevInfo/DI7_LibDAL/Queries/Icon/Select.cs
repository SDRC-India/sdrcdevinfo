using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Icon
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
        /// Get Icon  details from Icons table for a given  elementNId and ElementType
        /// </summary>
        /// <param name="elementNId">elementNId for which icon is to be extracted</param>
        /// <param name="IconElementType">For FilterFieldType "Search" FilterText should be FootNotes.FootNote LIKE '%Sample FootNote%'</param>
        /// <returns></returns>
        public string GetIcon(string elementNId, IconElementType IconElementType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Icons.IconNId + "," + DIColumns.Icons.IconType + "," + DIColumns.Icons.IconDimW );
            sbQuery.Append("," + DIColumns.Icons.IconDimH + "," + DIColumns.Icons.ElementType + "," + DIColumns.Icons.ElementNId + "," + DIColumns.Icons.ElementIcon);

            sbQuery.Append(" FROM " + this.TablesName.Icons);
                     
            sbQuery.Append(" WHERE ");
            sbQuery.Append(DIColumns.Icons.ElementType + "=" + DIQueries.IconElementTypeText[IconElementType]);

            if (elementNId.Trim().Length > 0)
            {
                sbQuery.Append(" AND " + DIColumns.Icons.ElementNId + " IN (" + elementNId + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get Icon  details from Icons table for a given  elementNId and ElementType
        /// </summary>
        /// <param name="elementNId">elementNId for which icon is to be extracted</param>
        /// <param name="IconElementType">element type</param>
        /// <returns></returns>
        public string GetIcon(string elementNId, string elementType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Icons.IconNId + "," + DIColumns.Icons.IconType + "," + DIColumns.Icons.IconDimW);
            sbQuery.Append("," + DIColumns.Icons.IconDimH + "," + DIColumns.Icons.ElementType + "," + DIColumns.Icons.ElementNId + "," + DIColumns.Icons.ElementIcon);

            sbQuery.Append(" FROM " + this.TablesName.Icons);

            sbQuery.Append(" WHERE ");
            sbQuery.Append(DIColumns.Icons.ElementType + "=" + elementType +"");

            if (elementNId.Trim().Length > 0)
            {
                sbQuery.Append(" AND " + DIColumns.Icons.ElementNId + " IN (" + elementNId + ")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the total number of records in Icon Table.
        /// </summary>
        /// <returns></returns>
        public string GetRecordCount()
        {
            StringBuilder RetVal = new StringBuilder();
            try
            {
                RetVal.Append("SELECT COUNT(*) FROM " + TablesName.Icons);
            }
            catch (Exception ex)
            {
                RetVal.Length = 0;
            }
            return RetVal.ToString();
        }
        /// <summary>
        /// Get all Icon Nid's from Icon table
        /// </summary>
        /// <returns></returns>
        public string GetIconNids()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Icons.IconNId);
            SqlQuery.Append(" FROM " + TablesName.Icons);
            return SqlQuery.ToString();
        }


        #endregion

        #endregion

    }
}
