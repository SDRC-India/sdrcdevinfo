using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;



namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build source record according to source information and insert it into database.
    /// </summary>
    public class SourceBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Add source info into sourcecollection. 
        /// </summary>
        /// <param name="sourceRecord">object of SourceInfo</param>
        private void AddSourceIntoCollection(SourceInfo sourceInfo)
        {
            if (!this.Sources.ContainsKey(sourceInfo.Name))
            {
                this.Sources.Add(sourceInfo.Name, sourceInfo);
            }

        }
        /// <summary>
        /// Check existance of source into sourcollection.
        /// </summary>
        /// <param name="sourceName">Source  Name</param>
        /// <returns>Source Nid</returns>
        private int CheckSourceInCollection(string sourceName)
        {
            int RetVal = 0;
            try
            {
                if (this.Sources.ContainsKey(sourceName))
                {
                    RetVal = this.Sources[sourceName].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }
        /// <summary>
        /// Get source nid
        /// </summary>
        /// <param name="sourceName">source Name</param>
        /// <returns>Source Nid</returns>
        private int GetSourceNid(string sourceName)
        {
            int RetVal = 0;

            try
            {
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Source.GetSource(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery(sourceName) + "'", FieldSelection.NId, false)));

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }
        /// <summary>
        /// Get source parent nid;
        /// </summary>
        /// <param name="publisher">publisher</param>
        /// <returns>Source Parent Nid</returns>
        private int GetSourceParentNid(string publisher)
        {
            int RetVal = 0;

            try
            {
                // check publisher exists or not
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Source.GetSource(FilterFieldType.Name, "'" + publisher + "'", FieldSelection.NId, true)));
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }
        /// <summary>
        /// Insert source parent
        /// </summary>
        /// <param name="sourceName">Source Name</param>
        private void InsertSourceParent(string sourceName)
        {
            this.InsertSource(sourceName, -1);
        }
        /// <summary>
        /// Insert source into database.
        /// </summary>
        /// <param name="sourceName">Source Name</param>
        /// <param name="parentNid">Parent Nid</param>
        private int InsertSource(string sourceName, int parentNid)
        {
            return this.InsertSource(sourceName, parentNid, string.Empty, string.Empty);
        }

        /// <summary>
        /// Insert source into database.
        /// </summary>
        /// <param name="sourceName">Source Name</param>
        /// <param name="parentNid">Parent Nid</param>
        private int InsertSource(string sourceName, int parentNid, string isbn, string nature)
        {
            int RetVal = 0;
            string sourceGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string SourceNameForDatabase = string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    SourceNameForDatabase = sourceName;
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Source.Insert.InsertSource(this.DBQueries.DataPrefix, "_" + LanguageCode, SourceNameForDatabase, parentNid, sourceGId, string.Empty, false, isbn, nature));
                    RetVal=this.GetSourceNid(sourceName);
                }
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        private string GetSourceParentFrmSourceName(string sourceName)
        {
            string RetVal = string.Empty;

            if (sourceName.IndexOf(Constants.SourceSeparator) > 0)
            {
                RetVal = sourceName.Substring(0, sourceName.IndexOf(Constants.SourceSeparator));
            }
            else
            {
                RetVal = Constants.DefaultPublisherName;
            }
            return RetVal;
        }

        private bool UpdateSources(IndicatorClassificationInfo icInfo, int parentNid)
        {
            bool RetVal = false;
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Source.Update.UpdateSource(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, icInfo.Name, parentNid, icInfo.ISBN, icInfo.Nature));

                RetVal = true;
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

       

        private int CheckPublisherExistsInDB(string publisher)
        {
            int RetVal = 0;
            DataTable Table = null;
            try
            {
                // -- Check if the Publisher exists or not
                string squery = this.DBQueries.IndicatorClassification.GetICTopParents(FilterFieldType.Name, "'" + publisher + "'", ICType.Source);

                Table = this.DBConnection.ExecuteDataTable(squery);

                {
                    //-- Set Top Publisher If record Not found
                    if (Table.Rows.Count == 0)
                    {
                        RetVal = -1;
                    }
                    else
                    {
                        RetVal = Convert.ToInt32(Table.Rows[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId]);
                    }

                    Table.Dispose();
                }
            }
            catch { }

            return RetVal;

        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns source colleciton in key,pair format. Key is source name and value is Object of SourceInfo.
        /// </summary>
        internal Dictionary<string, SourceInfo> Sources = new Dictionary<string, SourceInfo>();

        #endregion
        #endregion

        #region "-- Public --"
        #region "-- New/Dispose --"

        public SourceBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of  source first in sourcecollection Then.
        /// </summary>
        /// <param name="sourceName">Source Name</param>
        /// <returns>Source Nid</returns>
        public int CheckSourceExists(string sourceName)
        {
            int RetVal = 0;

            //Step 1: check source exists in source collection
            RetVal = this.CheckSourceInCollection(sourceName);

            //Step 2: check source exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetSourceNid(sourceName);
            }

            return RetVal;
        }
         /// <summary>
        /// Check existance of source in database if not found then create source
        /// </summary>
        /// <param name="sourceName">Source Name</param>
        /// <returns>Source Nid</returns>
        public int CheckNCreateSource(string sourceName)
        {
            return this.CheckNCreateSource(sourceName, string.Empty, string.Empty);
        }

        /// <summary>
        /// Checks and creates source
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="isbn"></param>
        /// <param name="nature"></param>
        /// <returns></returns>
        public int CheckNCreateSource(string sourceName, string isbn,string nature)
        {
            int RetVal = 0;
            string Publisher = string.Empty;
            int ParentNid = 0;
            SourceInfo sourceInfo = new SourceInfo();

            try
            {
                
                // check source exists or not
                RetVal = this.CheckSourceExists(sourceName);

                // if source does not exist then create it.
                if (RetVal <= 0)
                {
                    //Step1: Get publisher from source.
                    Publisher = this.GetSourceParentFrmSourceName(sourceName);

                    //Step2: check publisher exists or not
                    ParentNid = this.GetSourceParentNid(Publisher);

                    //if not exists then create it
                    if (ParentNid <= 0)
                    {
                        // create parent source
                        this.InsertSourceParent(Publisher.ToString());

                        // get parent nid
                        ParentNid = this.GetSourceParentNid(Publisher);
                    }

                    if (ParentNid > 0)
                    {
                        // create  source
                        this.InsertSource(sourceName, ParentNid, isbn, nature);
                        RetVal = this.GetSourceNid(sourceName);
                    }
                }

                // add source information into source collection
                sourceInfo.Name = sourceName;
                sourceInfo.Nid = RetVal;
                sourceInfo.ParentNid = ParentNid;
                sourceInfo.ISBN = isbn;
                sourceInfo.Nature = nature;
                this.AddSourceIntoCollection(sourceInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Update Sources
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="sourceNid"></param>
        /// <returns></returns>
        public bool UpdateSources(string sourceName, int nid, int parentNid)
        {
            bool RetVal = false;
            int SourceParentNid;
            if (this.CheckSourceExists(sourceName) <= 0)
            {
                IndicatorClassificationInfo ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();

                ICInfo.Parent.Name = GetSourceParentFrmSourceName(sourceName);
                ICInfo.Name = sourceName;
                ICInfo.Type = ICType.Source;

                //-- Update Publisher
                this.UpdateSources(ICInfo.Parent, parentNid);

                SourceParentNid = this.CheckPublisherExistsInDB(sourceName);
                if (SourceParentNid == -1)
                {
                    //-- Update Source
                    this.UpdateSources(ICInfo, nid);

                }
                RetVal = true;
            }
            return RetVal;
        }

        /// <summary>
        /// Update Sources
        /// </summary>
        /// <param name="sourceName"></param>
        /// <param name="sourceNid"></param>
        /// <returns></returns>
        public bool UpdateSources(string sourceName, int nid, int parentNid,string ISBN,string nature)
        {
            bool RetVal = false;
            int SourceParentNid;

            if (this.CheckSourceExists(sourceName) <= 0)
            {
                IndicatorClassificationInfo ICInfo = new IndicatorClassificationInfo();
                ICInfo.Parent = new IndicatorClassificationInfo();

                ICInfo.Parent.Name = GetSourceParentFrmSourceName(sourceName);
                ICInfo.Name = sourceName;
                ICInfo.Type = ICType.Source;
                ICInfo.ISBN = ISBN;
                ICInfo.Nature = nature;

                //-- Update Publisher
                this.UpdateSources(ICInfo.Parent, parentNid);

                SourceParentNid = this.CheckPublisherExistsInDB(sourceName);
                if (SourceParentNid == -1)
                {
                    //-- Update Source
                    this.UpdateSources(ICInfo, nid);

                }
                RetVal = true;
            }
            else
            {
                //-- Update Publisher               
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Source.Update.UpdateSource(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, sourceName, nid, ISBN, nature));
            }
            return RetVal;
        }

        /// <summary>
        /// To import source into template or database
        /// </summary>
        /// <param name="sourceInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSource(SourceInfo sourceInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            string MetadataInfo = string.Empty;
            string SqlString = string.Empty;
            string Publisher = string.Empty;
            int ParentNid = -1;
            DataRow Row;
            DataTable TempTable;
            Dictionary<String, String> OldIconNId_NewIconNId = new Dictionary<string, string>();
            DI7MetaDataBuilder SourceMetadataBuilder = null;

            try
            {

                //check source already exists in database or not
                RetVal = this.GetSourceNid(sourceInfo.Name);

                if (RetVal > 0)
                {
                    // if source indicator is global
                    if (sourceInfo.Global)
                    {
                        // do nothing
                    }
                }
                else
                {
                    RetVal = this.CheckNCreateSource(sourceInfo.Name, sourceInfo.ISBN, sourceInfo.Nature);
                }

                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.IndicatorClassification, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

                // import metadata reports from source database into current database
                SourceMetadataBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                SourceMetadataBuilder.ImportMetadata(sourceDBConnection, sourceQurey, NidInSourceDB, RetVal, MetadataElementType.Source, MetaDataType.Source, IconElementType.MetadataSource);

                //OldIconNId_NewIconNId = DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.MetadataSource, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

                //// get metadata info.
                //MetadataInfo = sourceInfo.Info;

                //// Update IconNids in xml if exists
                //foreach (string OldIconName in OldIconNId_NewIconNId.Keys)
                //{
                //    MetadataInfo = MetadataInfo.Replace(OldIconName, OldIconNId_NewIconNId[OldIconName].ToString());
                //}
                //MetadataInfo = DICommon.CheckNConvertMetadataXml(MetadataInfo);
                //// Update Metadata
                //this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UpdateICInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, DICommon.RemoveQuotes(MetadataInfo), ICType.Source, RetVal));

                //// -- insert records in xslt tables 
                //SqlString = sourceQurey.Xslt.GetXSLT(NidInSourceDB.ToString(), MetadataElementType.Source);
                //TempTable = sourceDBConnection.ExecuteDataTable(SqlString);


                //if (TempTable.Rows.Count > 0)
                //{
                //    Row = TempTable.Rows[0];
                //    MetaDataBuilderObj = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                //    MetaDataBuilderObj.ImportTransformInfo(Row[XSLT.XSLTText].ToString(), Row[XSLT.XSLTFile].ToString(), RetVal.ToString(), MetadataElementType.Source);
                //}

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// Deletes sources from Indicator_Classification table  and associated records from IC_IUS table
        /// </summary>
        /// <param name="nids">Comma separated nids which may be blank</param>
        public void DeleteSources(string nids)
        {
            DITables TableNames;
            IndicatorClassificationBuilder ICBuilder;
            MetaDataBuilder MetadataBuilderObject;
            if (!string.IsNullOrEmpty(nids))
            {
                try
                {
                    // Step1: Delete records from IndicatorClassification table
                    foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                    {
                        TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteSources(TableNames.IndicatorClassifications, nids));

                    }

                    if (!string.IsNullOrEmpty(nids))
                    {
                        // Step2: Delete records from IC_IUS table
                        ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
                        ICBuilder.DeleteClassificationIUSRelation(nids, string.Empty);

                        // delete records data table
                        //this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Data.DeleteRecordsBySourceNIds(nids));
                        new DIDatabase(this.DBConnection, this.DBQueries).DeleteDataValue(string.Empty, string.Empty, string.Empty, nids);

                        // delete metadata 
                        MetadataBuilderObject = new MetaDataBuilder(this.DBConnection, this.DBQueries);
                        MetadataBuilderObject.DeleteMetadata(nids, MetadataElementType.Source);
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        #endregion

        #endregion


    }
}
