using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Metadata;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build indicator according to indicator information and insert it into database.
    /// </summary>
    public class IndicatorBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        ///To add indicatorInfo into collection 
        /// </summary>
        /// <param name="indicatorRecord">object of IndicatorInfo</param>
        private void AddIndicatorIntoCollection(IndicatorInfo indicatorInfo)
        {
            if (!this.IndicatorCollection.ContainsKey(indicatorInfo.Name))
            {
                this.IndicatorCollection.Add(indicatorInfo.Name, indicatorInfo);
            }

        }
        /// <summary>
        /// To  check Indicato exists in collection
        /// </summary>
        /// <param name="indicatorName"></param>
        /// <returns>Indicator Nid</returns>
        private int CheckIndicatorInCollection(string indicatorName)
        {
            int RetVal = 0;
            try
            {
                if (this.IndicatorCollection.ContainsKey(indicatorName))
                {
                    RetVal = this.IndicatorCollection[indicatorName].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        private void CreateIndicatorTransformInfo(string XSLTText, string XsltFilename, string elementNId, MetadataElementType elementType)
        {
            // -- this function insert the values into RT_XSLT table 
            string SqlString = string.Empty;
            try
            {
                int XsltNid = 0;

                // -- step1 : check xslt info already exists or not 
                SqlString = this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, XsltFilename);

                if (this.DBConnection.ExecuteDataTable(SqlString).Rows.Count == 0)
                {
                    //-- step 2: insert into xslt table 
                    SqlString = DALQueries.Xslt.Insert.InsertXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(XSLTText), DICommon.RemoveQuotes(XsltFilename));

                    this.DBConnection.ExecuteNonQuery(SqlString);
                }
                else
                {
                    //-- step 2: Update xslt table 
                    SqlString = DALQueries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(XSLTText), XsltFilename);
                    this.DBConnection.ExecuteNonQuery(SqlString);
                }
                //-- step 3: find the xslt_nid against xslFilename 
                SqlString = this.DBQueries.Xslt.GetXSLT(FilterFieldType.Name, XsltFilename);

                // -- step 4: insert into Element_xslt table 
                XsltNid = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlString));

                SqlString = DALQueries.Xslt.Insert.InsertElementXSLT(this.DBQueries.DataPrefix, Convert.ToInt32(elementNId), elementType, Convert.ToInt32(XsltNid));

                this.DBConnection.ExecuteNonQuery(SqlString);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }



        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Indicators.GetIndicator(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery(name) + "'", FieldSelection.NId);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if GID exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.Indicators.GetIndicator(FilterFieldType.GId, "'" + DIQueries.RemoveQuotesForSqlQuery(GID) + "'", FieldSelection.NId);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));

            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert Indicator record into database
        /// </summary>
        /// <param name="indicatorInfo">object of IndicatorInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(IndicatorInfo indicatorInfo)
        {
            bool RetVal = false;
            string IndicatorName = indicatorInfo.Name;
            string IndicatorGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string IndicatorForDatabase = string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(indicatorInfo.GID))
                {
                    IndicatorGId = indicatorInfo.GID;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        IndicatorForDatabase = IndicatorName;
                    }
                    else
                    {
                        IndicatorForDatabase = Constants.PrefixForNewValue + IndicatorName;

                    }
                    //--
                    indicatorInfo.Info = DICommon.CheckNConvertMetadataXml(indicatorInfo.Info);

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertIndicator(this.DBQueries.DataPrefix, "_" + LanguageCode, IndicatorForDatabase, IndicatorGId, indicatorInfo.Info, indicatorInfo.Global, indicatorInfo.HighIsGood, this.DBConnection.ConnectionStringParameters.ServerType));
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Propertipes --"

        /// <summary>
        /// Returns indicator colleciton in key,pair format. Key is indicator name and value is Object of IndicatorInfo.
        /// </summary>
        internal Dictionary<string, IndicatorInfo> IndicatorCollection = new Dictionary<string, IndicatorInfo>();

        #endregion
        #endregion

        #region "-- Public --"
        #region "-- New/Dispose --"

        public IndicatorBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// To check existance of Indicator first into collection then into database
        /// </summary>
        /// <param name="indicatorRecord">object of IndicatorInfo</param>
        /// <returns>Indicator Nid</returns>
        public int CheckIndicatorExists(IndicatorInfo indicatorInfo)
        {
            int RetVal = 0;

            //Step 1: check source exists in source collection
            RetVal = this.CheckIndicatorInCollection(indicatorInfo.Name);

            //Step 2: check indicator exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetIndicatorNid(indicatorInfo.GID, indicatorInfo.Name);
            }

            return RetVal;
        }

        /// <summary>
        /// Check existance of indicator record into database if false then create indicator record 
        /// </summary>
        /// <param name="indicatorInfo">object of IndicatorInfo</param>
        /// <returns>Indicator Nid</returns>
        public int CheckNCreateIndicator(IndicatorInfo indicatorInfo)
        {
            int RetVal = 0;

            try
            {
                // check indicator exists or not
                RetVal = this.CheckIndicatorExists(indicatorInfo);

                // if indicator does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert indicator
                    if (this.InsertIntoDatabase(indicatorInfo))
                    {
                        RetVal = this.GetNidByName(indicatorInfo.Name);
                    }

                }

                // add indicator information into collection
                indicatorInfo.Nid = RetVal;
                this.AddIndicatorIntoCollection(indicatorInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns indicator nid. 
        /// </summary>
        /// <param name="indicatorGid">Indicator GID </param>
        /// <param name="indicatorName">Name of the Indicator.It can be empty</param>
        /// <returns></returns>
        public int GetIndicatorNid(string indicatorGid, string indicatorName)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(indicatorGid))
            {
                RetVal = this.GetNidByGID(indicatorGid);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(indicatorName))
                {
                    RetVal = this.GetNidByName(indicatorName);
                }
            }

            return RetVal;
        }


        /// <summary>
        /// To import indicator into template or database
        /// </summary>
        /// <param name="indicatorInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <reeturns></returns>
        public int ImportIndicator(IndicatorInfo indicatorInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            return this.ImportIndicator(indicatorInfo, NidInSourceDB, sourceQurey, sourceDBConnection, true);
        }

        public int ImportIndicatorMetadata(IndicatorInfo indicatorInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            return this.ImportIndicator(indicatorInfo, NidInSourceDB, sourceQurey, sourceDBConnection, false);
        }


        /// <summary>
        /// To import indicator into template or database
        /// </summary>
        /// <param name="indicatorInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportIndicator(IndicatorInfo sourceIndicatorInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection, bool importIndicatorInfoAlso)
        {
            int RetVal = -1;
            string metadataInfo = string.Empty;
            string SqlString = string.Empty;
            DataRow Row;
            DataTable TempTable;
            //Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();
            //MetaDataBuilder MetaDataBuilderObj;
            DI7MetaDataBuilder MetadataBuilderObj = null;
            IndicatorInfo TrgIndicatorInfo;

            try
            {
                //check Indicator already exists in database or not

                RetVal = this.GetIndicatorNid(sourceIndicatorInfo.GID, sourceIndicatorInfo.Name);

                if (RetVal > 0)
                {
                    TrgIndicatorInfo = this.GetIndicatorInfo(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Heavy);

                    // dont import if trg indicator is global but source indicator is local
                    if (TrgIndicatorInfo.Global & sourceIndicatorInfo.Global == false)
                    {
                        // dont import if trg indicator is global but source indicator is local
                    }
                    else
                    {
                        sourceIndicatorInfo.Info = DICommon.CheckNConvertMetadataXml(sourceIndicatorInfo.Info);
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, sourceIndicatorInfo.Name, sourceIndicatorInfo.GID, sourceIndicatorInfo.Global, sourceIndicatorInfo.Info, RetVal, sourceIndicatorInfo.HighIsGood));
                    }

                }
                else if (importIndicatorInfoAlso)
                {
                    if (this.InsertIntoDatabase(sourceIndicatorInfo))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }

                if (RetVal > 0)
                {
                    //update/insert icon 
                    DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Indicator, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);


                    // import metadata
                    MetadataBuilderObj = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                    MetadataBuilderObj.ImportMetadata(sourceDBConnection, sourceQurey, NidInSourceDB, RetVal, MetadataElementType.Indicator, MetaDataType.Indicator, IconElementType.MetadataIndicator);

                    //////// -- insert records in xslt tables 
                    //////SqlString = sourceQurey.Xslt.GetXSLT(NidInSourceDB.ToString(), MetadataElementType.Indicator);
                    //////TempTable = sourceDBConnection.ExecuteDataTable(SqlString);


                    //////if (TempTable.Rows.Count > 0)
                    //////{
                    //////    Row = TempTable.Rows[0];
                    //////    MetaDataBuilderObj = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                    //////    MetaDataBuilderObj.ImportTransformInfo(Row[XSLT.XSLTText].ToString(), Row[XSLT.XSLTFile].ToString(), RetVal.ToString(), MetadataElementType.Indicator);
                    //////}
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// To import indicator information from mapped indicator
        /// </summary>
        /// <param name="indicatorInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidInTrgDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportIndicatorFrmMappedIndicator(IndicatorInfo indicatorInfo, int NidInSourceDB, int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            string metadataInfo = string.Empty;
            string SqlString = string.Empty;
            DataRow Row;
            DataTable TempTable;
            Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();
            MetaDataBuilder MetaDataBuilderObj;
            IndicatorInfo TrgIndicatorInfo;

            try
            {
                // set RetVal to targetNID
                RetVal = NidInTrgDB;

                if (RetVal > 0)
                {
                    TrgIndicatorInfo = this.GetIndicatorInfo(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Light);

                    // dont import if trg indicator is global but source indicator is local
                    if (TrgIndicatorInfo.Global & indicatorInfo.Global == false)
                    {
                        // dont import if trg indicator is global but source indicator is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, indicatorInfo.Name, indicatorInfo.GID, indicatorInfo.Global, indicatorInfo.Info, RetVal));
                    }

                }


                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Indicator, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

                OldIconNId_NewIconNId = DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.MetadataIndicator, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

                // get metadata info.
                metadataInfo = indicatorInfo.Info;

                // Update IconNids in xml if exists
                foreach (string OldIconName in OldIconNId_NewIconNId.Keys)
                {
                    metadataInfo = metadataInfo.Replace(OldIconName, OldIconNId_NewIconNId[OldIconName].ToString());
                }

                metadataInfo = DICommon.CheckNConvertMetadataXml(metadataInfo);
                // Update Metadata
                this.DBConnection.ExecuteNonQuery(DALQueries.Indicator.Update.UpdateIndicatorInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, DICommon.RemoveQuotes(metadataInfo), FilterFieldType.GId, indicatorInfo.GID));


                // -- insert records in xslt tables 

                SqlString = sourceQurey.Xslt.GetXSLT(NidInSourceDB.ToString(), MetadataElementType.Indicator);
                TempTable = sourceDBConnection.ExecuteDataTable(SqlString);


                if (TempTable.Rows.Count > 0)
                {
                    Row = TempTable.Rows[0];
                    MetaDataBuilderObj = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                    MetaDataBuilderObj.ImportTransformInfo(Row[XSLT.XSLTText].ToString(), Row[XSLT.XSLTFile].ToString(), RetVal.ToString(), MetadataElementType.Indicator);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }


        /// <summary>
        /// Updates the Indicator information into database on the basis of NId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GId"></param>
        /// <param name="isGlobal"></param>
        /// <param name="info"></param>
        /// <param name="NId"></param>
        public void UpdateIndicator(string name, string GId, bool isGlobal, string info, int NId)
        {
            string SqlQuery = string.Empty;
            SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, name, GId, isGlobal, info, NId);

            try
            {
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Updates the Indicator information into database on the basis of NId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GId"></param>
        /// <param name="isGlobal"></param>
        /// <param name="info"></param>
        /// <param name="NId"></param>
        public void UpdateIndicator(string name, string GId, bool isGlobal, int NId)
        {
            string SqlQuery = string.Empty;
            SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, name, GId, isGlobal, NId);

            try
            {
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Returns instance of IndicatorInfo.
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public IndicatorInfo GetIndicatorInfo(FilterFieldType filterClause, string filterText, FieldSelection selectionType)
        {
            string Query = string.Empty;
            IndicatorInfo RetVal = new IndicatorInfo();
            DataTable IndicatorTable;
            try
            {
                //get indicator information
                Query = this.DBQueries.Indicators.GetIndicator(filterClause, filterText, selectionType);
                IndicatorTable = this.DBConnection.ExecuteDataTable(Query);

                //set indicator info
                if (IndicatorTable != null)
                {
                    if (IndicatorTable.Rows.Count > 0)
                    {
                        RetVal.GID = IndicatorTable.Rows[0][Indicator.IndicatorGId].ToString();
                        RetVal.Global = Convert.ToBoolean(IndicatorTable.Rows[0][Indicator.IndicatorGlobal]);
                        RetVal.Name = IndicatorTable.Rows[0][Indicator.IndicatorName].ToString();
                        RetVal.Nid = Convert.ToInt32(IndicatorTable.Rows[0][Indicator.IndicatorNId]);
                        if (selectionType == FieldSelection.Heavy)
                        {
                            RetVal.Info = IndicatorTable.Rows[0][Indicator.IndicatorInfo].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Deletes indicators and associated records from IUS and IC_IUS table
        /// </summary>
        /// <param name="indicatorNids"></param>
        public void DeleteIndicator(string indicatorNids)
        {
            DITables TableNames;
            IUSBuilder IUSBuilder;
            MetaDataBuilder MetadataBuilderObject;
            string AssocicatedIUSNIds = string.Empty;
            try
            {

                IUSBuilder = new IUSBuilder(this.DBConnection, this.DBQueries);

                // Step 1: Delete records from Indicator table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Delete.DeleteIndicator(TableNames.Indicator, indicatorNids));
                }

                // Step 2: Delete records from IUS table

                // Step2(a): Get all associated IUSNIds
                AssocicatedIUSNIds = IUSBuilder.GetAllAssociatedIUSNids(indicatorNids, string.Empty, string.Empty);

                // Step2(b): Delete all associated IUSNIds
                IUSBuilder.DeleteIUS(AssocicatedIUSNIds);


                // delete metadata 
                MetadataBuilderObject = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                MetadataBuilderObject.DeleteMetadata(indicatorNids, MetadataElementType.Indicator);
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }


        }

        /// <summary>
        /// Returns true if indicator already exists otherwise false. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nid">send -99 if indicator is new otherwise send indicator nid </param>
        /// <returns></returns>
        public bool IsIndicatorAlreadyExists(string name, int nid)
        {
            bool RetVal = false;
            int FetchedNid = 0;

            // get nid from database
            FetchedNid = this.GetNidByName(name);


            if (nid > 0)
            {
                // check if nid is given 
                if (FetchedNid == nid)
                {
                    RetVal = false;
                }
                else if (FetchedNid > 0)
                {
                    RetVal = true;
                }
            }
            else if (FetchedNid > 0)
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true if indicator GID already exists otherwise false. 
        /// </summary>
        /// <param name="GID"></param>
        /// <param name="nid">send -99 if indicator is new otherwise send indicator nid </param>
        /// <returns></returns>
        public bool IsIndicatorGIDAlreadyExists(string GID, int nid)
        {
            bool RetVal = false;
            int FetchedNid = 0;

            // get nid from database
            FetchedNid = this.GetNidByGID(GID);


            if (nid > 0)
            {
                // check if nid is given 
                if (FetchedNid == nid)
                {
                    RetVal = false;
                }
                else if (FetchedNid > 0)
                {
                    RetVal = true;
                }
            }
            else if (FetchedNid > 0)
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Updates data exists values
        /// </summary>
        public void UpdateDataExistValues()
        {
            DIServerType ServerType = this.DBConnection.ConnectionStringParameters.ServerType;
            DITables TablesName;
            string LanguageCode = string.Empty;

            try
            {
                // 1. set all indicators' data_exist value to false in default language table
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateDataExistToFalse(ServerType, this.DBQueries.TablesName.Indicator));

                // 2. set data_exist to true but where data exists
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateDataExistValues(ServerType, this.DBQueries.TablesName));

                // 3. update data_exist in  other language tables 
                foreach (DataRow LanguageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = Convert.ToString(LanguageRow[Language.LanguageCode]);

                    // update all Language tables except default language table
                    if (("_" + LanguageCode) != this.DBQueries.LanguageCode)
                    {
                        TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateDataExistValuesInOtherLanguage(this.DBQueries.TablesName.Indicator, TablesName.Indicator));
                    }
                }
            }
            catch (Exception ex)
            {
                DevInfo.Lib.DI_LibBAL.ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        ////public void UddateIndicatorOrder()
        ////{
        ////    //// 1. Set indicator_order to empty for all indicators


        ////    //// 2. Update indicator order by IC_order & IC_IUS_Order
        ////    //this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicatorByICNIUSOrder());

        ////}

        /// <summary>
        /// Import Indicator
        /// </summary>
        /// <param name="indicatorName"></param>
        /// <param name="indicatorGID"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public int ImportIndicator(string indicatorName, string indicatorGID, bool isGlobal)
        {
            int RetVal = 0;
            IndicatorInfo IndicatorInfoObj = null;
            IndicatorInfo TrgIndicatorInfo = null;

            IndicatorInfoObj = new IndicatorInfo();
            IndicatorInfoObj.Global = isGlobal;
            IndicatorInfoObj.Name = indicatorName;
            IndicatorInfoObj.GID = indicatorGID;
            try
            {
                RetVal = this.GetIndicatorNid(indicatorGID, indicatorName);

                if (RetVal > 0)
                {
                    TrgIndicatorInfo = this.GetIndicatorInfo(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Light);

                    // dont import if trg indicator is global but source indicator is local
                    if (TrgIndicatorInfo.Global & isGlobal == false)
                    {
                        // dont import if trg indicator is global but source indicator is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, indicatorName, indicatorGID, isGlobal, TrgIndicatorInfo.Info, RetVal));
                    }
                }
                else
                {
                    if (this.InsertIntoDatabase(IndicatorInfoObj))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        public int ImportIndicator(string indicatorName, string indicatorGID, bool isGlobal, bool highIsGood)
        {
            int RetVal = 0;
            IndicatorInfo IndicatorInfoObj = null;
            IndicatorInfo TrgIndicatorInfo = null;
            string LangCode = this.DBQueries.LanguageCode;

            IndicatorInfoObj = new IndicatorInfo();
            IndicatorInfoObj.Global = isGlobal;
            IndicatorInfoObj.Name = indicatorName;
            IndicatorInfoObj.GID = indicatorGID;
            IndicatorInfoObj.HighIsGood = highIsGood;
            try
            {

                RetVal = this.GetIndicatorNid(indicatorGID, indicatorName);

                if (RetVal > 0)
                {
                    if (!this.DBQueries.LanguageCode.StartsWith("_"))
                    {
                        LangCode = "_" + LangCode;
                    }

                    TrgIndicatorInfo = this.GetIndicatorInfo(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Light);

                    // dont import if trg indicator is global but source indicator is local
                    if (TrgIndicatorInfo.Global & isGlobal == false)
                    {
                        // dont import if trg indicator is global but source indicator is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateByNid(this.DBQueries.DataPrefix, LangCode, indicatorName, indicatorGID, isGlobal, TrgIndicatorInfo.Info, RetVal, IndicatorInfoObj.HighIsGood));
                    }
                }
                else
                {
                    if (this.InsertIntoDatabase(IndicatorInfoObj))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// Import HighISGood from Source database
        /// </summary>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        public void ImportHighISGood(DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {
            string IUSNids = string.Empty;

            DataTable IndTable = null;

            //-- Get IUS from Source Database
            IndTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light));

            foreach (DataRow Row in IndTable.Rows)
            {
                bool HighISGood = false;
                int IndNid = this.GetIndicatorNid(Convert.ToString(Row[Indicator.IndicatorGId]), Convert.ToString(Row[Indicator.IndicatorName]));

                if (IndNid > 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Row[Indicator.HighIsGood])))
                    {
                        HighISGood = Convert.ToBoolean(Row[Indicator.HighIsGood]);
                    }

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateISHighGood(this.DBConnection.ConnectionStringParameters.ServerType, this.DBQueries.TablesName.Indicator, IndNid, HighISGood));
                }
            }
        }

        #endregion

        #endregion

    }
}
