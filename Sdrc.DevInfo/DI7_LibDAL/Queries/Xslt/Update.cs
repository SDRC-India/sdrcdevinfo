using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Xslt
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
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
        /// Returns sql query to update xslt into UT_XSLT table
        /// </summary>
        /// <param name="dataprefix"></param>
        /// <param name="XSLTText"></param>
        /// <param name="XsltFilename"></param>
        /// <returns></returns>
        public static string UpdateXSLT(string dataprefix, string XSLTText, string XsltFilename)
        {
            string RetVal = string.Empty;
                        

            RetVal = "Update " + dataprefix+ Update.XSLTTableName + " set "+ DIColumns.XSLT.XSLTText +" ='"
                + XSLTText + "' WHERE "+ DIColumns.XSLT.XSLTFile+"='" + XsltFilename + "'";


            return RetVal;
        }

        public static string UpdateXSLT(string dataprefix, string xsltText)
        {
            string RetVal = string.Empty;


            RetVal = "Update " + dataprefix + Update.XSLTTableName + " set " + DIColumns.XSLT.XSLTText + " ='"
                + xsltText + "' WHERE 1=1";


            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update XsltNid into UT_Element_XSLT table
        /// </summary>
        /// <param name="dataprefix"></param>
        /// <param name="XSLTNid"></param>
        /// <param name="elementNId"></param>
        /// <param name="elementType"></param>
        /// <returns></returns>
        public static string UpdateElementXSLT(string dataprefix, int XSLTNid, int elementNId,MetadataElementType elementType)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + dataprefix+ Update.ElementTableName + " set "+ DIColumns.EelementXSLT.XSLTNId +" ="
                + XSLTNid + " WHERE "+ DIColumns.EelementXSLT.ElementNId+"=" + elementNId+ " AND "+DIColumns.EelementXSLT.ElementType +"="+ DIQueries.MetadataElementTypeText[elementType] ;
            
            return RetVal;
        }

        
        #endregion

        #endregion

    }
}