using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Xslt
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string XSLTTableName = "XSLT";
        private const string ElementTableName = "Element_XSLT";


        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Returns sql query to insert Xslt into UT_XSLT table
        /// </summary>
        /// <param name="dataprefix"></param>
        /// <param name="XsltFilename"></param>
        /// <returns></returns>
        public static string InsertXSLT(string dataprefix,string XSLTText, string XsltFilename)
        {
            string RetVal = string.Empty;

            
            RetVal = "Insert into " + dataprefix + Insert.XSLTTableName + "(" + DIColumns.XSLT.XSLTText+ ","+DIColumns.XSLT.XSLTFile +") values('" + XSLTText + "','" + XsltFilename + "')";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to insert element's xslt relationship into UT_Element_XSLT table
        /// </summary>
        /// <param name="dataprefix"></param>
        /// <param name="elementNId"></param>
        /// <param name="elementType"></param>
        /// <param name="XsltNid"></param>
        /// <returns></returns>
        public static string InsertElementXSLT(string dataprefix, int elementNId, MetadataElementType elementType, int XsltNid)
        {
            string RetVal = string.Empty;

            RetVal = "insert into " + dataprefix + Insert.ElementTableName + "(" + DIColumns.EelementXSLT.ElementNId + "," + DIColumns.EelementXSLT.ElementType + "," + DIColumns.XSLT.XSLTNId + ") values(" + elementNId + "," + DIQueries.MetadataElementTypeText[elementType] + "," + XsltNid + ")";

         return RetVal;

        }
#endregion

        #endregion

    }
}