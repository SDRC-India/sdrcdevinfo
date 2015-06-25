using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL
{
    /// <summary>
    /// Provides methods to read/write xml and xsl into database
    /// </summary>
    public abstract class MetadataEditorSource
    {


        #region "-- Public/Protected --"

        #region "-- Variables --"

        private DIConnection _DBConnection;
        /// <summary>
        /// Gets or sets instance of DIConnection
        /// </summary>
        public DIConnection DBConnection
        {
            get { return this._DBConnection; }
            set { this._DBConnection = value; }
        }

        private DIQueries _DBQueries;
        /// <summary>
        /// Gets or sets instance of DIQueries
        /// </summary>
        public DIQueries DBQueries
        {
            get { return this._DBQueries; }
            set { this._DBQueries = value; }
        }

        internal string _ImageElementType = string.Empty;
        /// <summary>
        /// Gets Image element type like IM, 
        /// </summary>
        public string ImageElementType
        {
            get { return this._ImageElementType; }
        }

        internal bool _IsRtfMetadata = false;
        /// <summary>
        /// Gets true or false. True if metadata is of RTF type means for indicator classifications
        /// </summary>
        public bool IsRtfMetadata
        {
            get { return this._IsRtfMetadata; }
        }


        #endregion

        #region "-- New/Dispose --"

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns Xslt text 
        /// </summary>
        /// <returns></returns>
        public string GetXSLTText()
        {
            string RetVal = string.Empty;

            // 1. Get Xslt text from DB
            RetVal = this.GetXSLTTextFrmDB();

            // 2. If Xslt text is not available in the DB then get it from BAL.Resources
            if (string.IsNullOrEmpty(RetVal))
            {
                RetVal = DevInfo.Lib.DI_LibBAL.Resource1.XSLTFile2;
            }

            return RetVal;
        }

        #region "-- Abstract --"
        public virtual string GetMetadataTextFrmDB(int elementNid)
        {
            return string.Empty;
        }

        /// <summary>
        /// Returns Category table 
        /// </summary>
        /// <returns></returns>
        public abstract DataTable GetCategoryDataTable();

        /// <summary>
        /// Returns xml text for the given element nid
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public abstract DataTable GetMetadataReportsTableFrmDB(int elementNid);

        /// <summary>
        /// Returns xslt text from UT_Xslt table
        /// </summary>
        /// <returns></returns>
        protected abstract string GetXSLTTextFrmDB();

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        public abstract bool SaveXsltTextIntoDB(string xsltText);

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <param name="removeQuotes"></param>
        /// <returns></returns>
        public abstract bool SaveXsltTextIntoDB(string xsltText,bool removeQuotes);

        /// <summary>
        /// Updates ElementXslt table
        /// </summary>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public abstract bool UpdateElementXsltTable(int elementNid);

        ///// <summary>
        ///// Returns true/false.True after successfully updation of xml text into database for the given element nid
        ///// </summary>
        ///// <param name="xmlText"></param>
        ///// <param name="elementNid"></param>
        ///// <returns></returns>
        //public abstract bool SaveMetadataTextIntoDB(string metadataText, int elementNid);

        #endregion

        protected string GetXslTextFrmDB(string metadataXsltType)
        {
            string RetVal = string.Empty;

            foreach (DataRow Row in this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, metadataXsltType)).Rows)
            {
                RetVal = Convert.ToString(Row[XSLT.XSLTText]);
                break;
            }

            return RetVal;
        }

        protected int GetXslNidFrmXSLTTable(string metadataXsltType)
        {
            int RetVal = -1;

            foreach (DataRow Row in this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, metadataXsltType)).Rows)
            {
                RetVal = Convert.ToInt32(Row[XSLT.XSLTNId]);
                break;
            }

            return RetVal;
        }

        protected int GetElementXsltNidFrmDB(MetadataElementType mdElementtype, int elementNid)
        {
            int RetVal = -1;


            foreach (DataRow Row in this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetElementXSLTTable(elementNid.ToString(),mdElementtype)).Rows)
            {
                RetVal = Convert.ToInt32(Row[EelementXSLT.ElementXSLTNId]);
                break;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        protected bool  SaveXsltTextIntoDB(string xsltText, string metadataXsltType)
        {
            return this.SaveXsltTextIntoDB(xsltText, metadataXsltType, true);
        }

        /// <summary>
        /// Returns true/false.True after successfully insertion/updation of xslt text into database
        /// </summary>
        /// <param name="xsltText"></param>
        /// <returns></returns>
        protected bool SaveXsltTextIntoDB(string xsltText, string metadataXsltType,bool removeQuotes)
        {
            bool RetVal = false;


            try
            {
                if (removeQuotes)
                {
                    xsltText = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(xsltText);
                    //xsltText = DIQueries.RemoveQuotesForSqlQuery(xsltText);
                }

                // 1. Check xslt exists in UT_Xslt table or not
                if (this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, metadataXsltType)).Rows.Count == 0)
                {
                    // 2. If not then insert XLST text into all UT_XSLT tables
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Insert.InsertXSLT(this.DBQueries.DataPrefix, xsltText, metadataXsltType));

                }
                else
                {
                    // 3. Else, Update XLST text into current UT_XLST language table
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, xsltText, metadataXsltType));

                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }


            return RetVal;
        }


        /// <summary>
        /// Updates ElementXslt table
        /// </summary>
        /// <param name="elementNid"></param>
        /// <param name="mdElementType"></param>
        /// <returns></returns>
        protected  bool UpdateElementXsltTable(int elementNid,MetadataElementType mdElementType,string mdXslTtype)
        {
            bool RetVal = false;
            int XsltNid = 0;
            int ElementXsltNid = 0;

            try
            {
                

                // 1. get xslt nid from XSLT table 
                XsltNid = this.GetXslNidFrmXSLTTable(mdXslTtype);

                if (XsltNid > 0)
                {
                    // 2. check record exists in Element_Xslt table or not
                    ElementXsltNid = this.GetElementXsltNidFrmDB(mdElementType, elementNid);

                    // 3. If not then insert record 
                    if (ElementXsltNid <= 0)
                    {
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Insert.InsertElementXSLT(this.DBQueries.DataPrefix, elementNid, mdElementType, XsltNid));
                    }
                    else
                    {
                        // 4. if exists then update record
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateElementXSLT(this.DBQueries.DataPrefix, XsltNid, elementNid, mdElementType));
                    }
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Returns true/false.True after successfully updation of xml text into database for the given element nid
        /// </summary>
        /// <param name="xmlText"></param>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public bool SaveMetadataTextIntoDB(string xmlText, int elementNid,MetaDataType elementType)
        {
            bool RetVal = false;
            MetaDataBuilder MetadataBuilderObj = new MetaDataBuilder(this.DBConnection, this.DBQueries);

            try
            {
                // update xml text into database
                MetadataBuilderObj.UpdateMetadataInfo(elementType, string.Empty, elementNid, xmlText);

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }


        #endregion

        #endregion

    }
}
