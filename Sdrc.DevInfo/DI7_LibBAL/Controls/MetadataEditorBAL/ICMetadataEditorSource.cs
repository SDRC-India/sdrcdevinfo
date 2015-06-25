using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL
{
    public class ICMetadataEditorSource : MetadataEditorSource
    {

        #region "-- Public --"

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"

        #endregion

        #region "-- Methods --"


        /// <summary>
        /// Updates ElementXslt table
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public override bool UpdateElementXsltTable(int elementNid)
        {
            // do nothing, Not applicable for IC
            return true;
        }


        /// <summary>
        /// Returns Category table 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetCategoryDataTable()
        {
            // do nothing. Not Applicable for IC

            return null;
        }

        /// <summary>
        /// Returns xml text for the given element nid
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public override string GetMetadataTextFrmDB(int elementNid)
        {
            string RetVal = string.Empty;
            DataTable Table=null;

            Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, elementNid.ToString(), FieldSelection.Heavy));
            foreach (DataRow Row in Table.Rows)
            {
                RetVal=Convert.ToString(Row[IndicatorClassifications.ICInfo]);
                break;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns metadata reports for the given element nid
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public override DataTable GetMetadataReportsTableFrmDB(int elementNid)
        {
            // not required for IC, so  return null
            return null;
        }


        /// <summary>
        /// Returns xslt text from UT_Xslt table
        /// </summary>
        /// <returns></returns>
        protected override string GetXSLTTextFrmDB()
        {
            // do nothing . Not applicable for indicator classifications         
            
            return string.Empty;
        }

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        public override bool SaveXsltTextIntoDB(string xsltText,bool removeQuotes)
        {
             // do nothing . Not applicable for indicator classifications         
            
            return false;
        }


        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        public override bool SaveXsltTextIntoDB(string xsltText)
        {

            return this.SaveXsltTextIntoDB(xsltText, true);
        }


        #endregion

        #endregion

    }
}
