using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL
{
    public class DISourceMetadataEditorSource : MetadataEditorSource
    {

        #region "-- Public --"

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"

        #endregion

        #region "-- Methods --"
        public override string GetMetadataTextFrmDB(int elementNid)
        {
            string RetVal = string.Empty;
            string ElementGID = string.Empty;
            DataTable Table = null;
            DA.DML.DI7MetaDataBuilder MDBuilder = null;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, elementNid.ToString(), FieldSelection.Heavy));
                foreach (DataRow Row in Table.Rows)
                {
                    ElementGID = Convert.ToString(Row[IndicatorClassifications.ICGId]);

                    // get xml file from SDMX library
                    RetVal = DevInfo.Lib.DI_LibSDMX.SDMXUtility.Get_MetadataReport(DI_LibSDMX.SDMXSchemaType.Two_One, ElementGID, DevInfo.Lib.DI_LibSDMX.MetadataTypes.Source, "MDAgency", this.DBQueries.LanguageCode.Replace("_", ""), this.DBConnection, this.DBQueries).InnerXml;

                    MDBuilder = new DA.DML.DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                    RetVal = MDBuilder.GetMetadataReportWCategoryName(RetVal, MetadataElementType.Source).InnerXml;

                    break;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }

            return RetVal;

        }

        /// <summary>
        /// Updates ElementXslt table
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public override bool UpdateElementXsltTable(int elementNid)
        {
            bool RetVal = false;

            RetVal = this.UpdateElementXsltTable(elementNid, MetadataElementType.Source, DIQueries.MetadataXsltType.Source);

            return RetVal;
        }

        /// <summary>
        /// Returns Category table 
        /// </summary>
        /// <returns></returns>
        public override DataTable GetCategoryDataTable()
        {
            DataTable RetVal = null;

            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategoriesForEditor(FilterFieldType.Type, DIQueries.MetadataElementTypeText[MetadataElementType.Source]));

            return RetVal;
        }

        /// <summary>
        /// Returns metadata reports for the given element nid
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public override DataTable GetMetadataReportsTableFrmDB(int elementNid)
        {
            DataTable RetVal = null;

            // get indicator metadata from metadatareport table
            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.MetadataReport.GetMetadataReportsByTargetNid(elementNid.ToString(), MetadataElementType.Source));


            return RetVal;
        }


        /// <summary>
        /// Returns xslt text from UT_Xslt table
        /// </summary>
        /// <returns></returns>
        protected override string GetXSLTTextFrmDB()
        {
            string RetVal = string.Empty;

            RetVal = RetVal = this.GetXslTextFrmDB(DIQueries.MetadataXsltType.Source);

            return RetVal;
        }

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        public override bool SaveXsltTextIntoDB(string xsltText,bool removeQuotes)
        {
            bool RetVal = false;

            RetVal = this.SaveXsltTextIntoDB(xsltText, DIQueries.MetadataXsltType.Source, removeQuotes);

            return RetVal;
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
