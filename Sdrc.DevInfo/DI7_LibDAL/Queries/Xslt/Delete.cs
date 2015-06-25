using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Xslt
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
        /// Returns sql query to delete XsltNid from UT_Element_XSLT table
        /// </summary>
        /// <param name="elementNIds"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public  string DeleteElementXSLT( string elementNIds, MetadataElementType elementType)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + this.TablesName.ElementXSLT + " WHERE  " +  DIColumns.EelementXSLT.ElementNId + " IN (" + elementNIds + " ) AND  " + DIColumns.EelementXSLT.ElementType + "=" + DIQueries.MetadataElementTypeText[elementType];

            return RetVal;
        }

        #endregion

        #endregion
    }
}
