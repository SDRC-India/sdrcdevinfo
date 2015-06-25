using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibDAL.Queries.AgePeriod
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
        /// Returns sql query to get age periods.
        /// </summary>
        /// <returns></returns>
        public string GetAgePeriods()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + this.TablesName.AgePeriod;

            return RetVal;
        }
        #endregion

        #endregion

    }
}
