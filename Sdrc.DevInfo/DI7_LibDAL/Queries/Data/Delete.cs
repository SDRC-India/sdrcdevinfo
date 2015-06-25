using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Data
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public class Delete
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New / Dispose --"

        public Delete(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns query to delete records from Data table
        /// </summary>
        /// <param name="nids">Comma separated DataNIds which may be blank </param>
        /// <returns></returns>
        public string DeleteRecords(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.DataNId + " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Data table
        /// </summary>
        /// <param name="nids">Comma separated sourceNIds which may be blank </param>
        /// <returns></returns>
        public string DeleteRecordsBySourceNIds(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.SourceNId + " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Data table
        /// </summary>
        /// <param name="nids">Comma separated subgroupValNId which may be blank </param>
        /// <returns></returns>
        public string DeleteRecordsBySubgroupValNIds(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.SubgroupValNId + " IN (" + nids + ")";
            }
            return RetVal;
        }



        /// <summary>
        /// Returns query to delete records from Data table for the given indicators nid
        /// </summary>
        /// <param name="nids">Comma separated indicatorsNId which may be blank </param>
        /// <returns></returns>
        public string DeleteRecordsByIndicatorsNId(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.IndicatorNId + " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Data table for the given units nid
        /// </summary>
        /// <param name="nids">Comma separated unitsNId which may be blank </param>
        /// <returns></returns>
        public string DeleteRecordsByUnitsNId(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.UnitNId + " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Data table
        /// </summary>
        /// <param name="nids">Comma separated IUSNIds which may be blank </param>
        /// <returns></returns>
        public string DeleteRecordsByIUSNIds(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.Data;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Data.IUSNId + " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete blank datavalue records from Data table
        /// </summary>
        /// <returns></returns>
        public string DeleteBlankData(DIServerType serverType)
        {

            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + this.TablesName.Data;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.MySql:                  
                case DIServerType.SqlServerExpress:
                case DIServerType.Sqlite:
                    RetVal += " WHERE (" + DIColumns.Data.TextualDataValue + " is null or " + DIColumns.Data.TextualDataValue + "= '') AND " + DIColumns.Data.IsTextualData + "=1";
                    break;
                case DIServerType.MsAccess:
                    RetVal += " WHERE " + DIColumns.Data.DataValue + " is null or " + DIColumns.Data.DataValue + "= '' AND " + DIColumns.Data.IsTextualData + "=TRUE";
                    break;  
                default:
                    RetVal += " WHERE " + DIColumns.Data.DataValue + " is null or " + DIColumns.Data.DataValue + "= '' AND " + DIColumns.Data.IsTextualData + "=TRUE";
                    break;
                        
            }
         

            return RetVal;
        }

        #endregion

        #endregion

    }
}
