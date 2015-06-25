using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Resources;
using System.Reflection;
using DevInfo.Lib.DI_LibDAL.Resources;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Converter.Database;
using ADOX;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// This class allows to create DevInfo database and provides methods to insert data value into database. 
    /// </summary>   
    /// <example>
    /// <code>
    /// 
    /// //Step 1: Create the instance of DIDatabase to work with database.It will create DevInfo database file only if does not exist.
    ///   DIDatabase Database = new DIDatabase("c:\\MDG Info 2006.mdb");
    /// 
    /// //Step 2:Do the following steps to insert data value into DevInfo Database.
    ///     //Step a: Create instance of AreaInfo and provide area name , area id, parent area name and parent area id. 
    ///     // if do not know the area id then use string.Empty.If area id is missing then area name will be treated as area id for both area and parent area.
    ///    AreaInfo Area1 = new AreaInfo("India", string.Empty, "Asia", string.Empty);
    /// 
    ///    //Step b: Create instance of IUSInfo and provide indicator name , unit name and subgroup name ,etc.
    ///        IUSInfo IUS = new IUSInfo();
    ///        IUS.IndicatorInfo.Name = "Population size";
    ///        IUS.UnitInfo.Name = "Percentage";
    ///        IUS.SubgroupValInfo.Name = "Rural";
    /// 
    ///    //Step c: Invoke AddDataPoint() method to insert data value .
    ///        Database.AddDataPoint(Area1, IUS, timeperiod,Source,dataValue);
    ///        
    /// // After inserting records, dispose the DIDatabase object.
    ///    Database.Dispose();
    ///</code>
    /// </example>
    public class DIDatabase : IDIDatabase, IDisposable
    {
        #region "-- Private --"

        #region "-- constants --"

        const string MINTIME = "MinTime";
        const string MINDATA = "MinData";
        const string MAXDATA = "MaxData";
        const string MAXTIME = "MaxTime";

        #endregion

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;
        private string FileNameWPath = string.Empty;
        private string LanguageCode = string.Empty;
        private string DataPrefix = string.Empty;
        private string BlankTemplateFileNameWPath = string.Empty;
        private static bool SeperateDataValueColumn = true;

        #endregion

        #region "-- New / Dispose --"

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Check existance of indicatorclassification nid and ius nid in database.
        /// </summary>
        /// <param name="ICNid">IndicatorClassification Nid </param>
        /// <param name="IUSNid">Ius Nid</param>
        /// <returns>IC_IUS Nid</returns>
        private int CheckICAndIUSRelation(int ICNid, int IUSNid)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable IndclassificationTable = new DataTable();
            try
            {
                SqlQuery = this.DBQueries.IndicatorClassification.GetICIUSNid(ICNid, IUSNid);
                IndclassificationTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                if (IndclassificationTable.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(IndclassificationTable.Rows[0][IndicatorClassificationsIUS.ICIUSNId]);
                }

            }
            catch (Exception)
            {

                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert indicatorclassification_IUS record into database.  
        /// </summary>
        /// <param name="ICNid">IC Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <returns></returns>
        private bool InsertICAndIUSRelation(int ICNid, int IUSNid)
        {
            bool RetVal = false;
            try
            {
                if (this.CheckICAndIUSRelation(ICNid, IUSNid) <= 0)
                {
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertICAndIUSRelation(this.DBQueries.DataPrefix, ICNid, IUSNid, false));
                }
            }
            catch (Exception)
            {
                return RetVal;
            }

            return false;
        }
        /// <summary>
        /// Check devinfo record in database .
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid</param>
        /// <returns>Data nid</returns>
        private int CheckDataExists(int areaNid, int IUSNid, int sourceNid, int timeperiodNid)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Data.GetDataByIUSTimePeriodAreaSource(IUSNid.ToString(), timeperiodNid.ToString(), areaNid.ToString(), sourceNid.ToString(), FieldSelection.NId);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Update data value into data table
        /// </summary>
        /// <param name="dataNid"></param>
        /// <param name="dataValue"></param>
        /// <returns>True/False</returns>
        public bool UpdateDataValue(int dataNid, string dataValue)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;
            try
            {

                // insert data value into data table 
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateDataValue(this.DBQueries.DataPrefix, dataNid, dataValue);
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //get datanid
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert data record into Data table
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid </param>
        /// <param name="dataValue">Data Value</param>
        /// <param name="footNoteNId"></param>
        /// <returns>Data Nid</returns>
        public int InsertDataValue(int areaNid, int IUSNid, int sourceNid, int timeperiodNid, string dataValue, int footNoteNId)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            try
            {
                // set footnote Nid to -1 if it is zero
                if (footNoteNId == 0)
                {
                    footNoteNId = -1;
                }

                // insert data value into data table 
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDataValue(this.DBQueries.DataPrefix, IUSNid, timeperiodNid, areaNid, dataValue, footNoteNId, sourceNid);
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //get datanid
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetNewNID()));

                //this.CheckDataExists(areaNid, IUSNid, sourceNid, timeperiodNid);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// set Database object.
        /// </summary>
        /// <param name="fileNameWPath">File Path of Database</param>
        /// <param name="dataset"></param>
        /// <param name="languageCode">language code like _en</param>
        private void SetDatabaseObjects(string fileNameWPath, string dataset, string languageCode)
        {
            //save filename with path
            this.FileNameWPath = fileNameWPath;

            //create database connection
            this.DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, String.Empty, String.Empty, fileNameWPath, string.Empty, string.Empty));

            //set language code and dataprefix
            this.SetLangaugeCodeNDataPrefix(dataset, languageCode);

            //set queries object to work with queries
            this.DBQueries = new DIQueries(this.DataPrefix, this.LanguageCode);

        }

        /// <summary>
        /// Set Dataprifix and languagecode  
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="langaugeCode">language code like _en</param>
        private void SetLangaugeCodeNDataPrefix(string dataPrefix, string langaugeCode)
        {
            try
            {
                //check dataprefix exists in the database
                if (!string.IsNullOrEmpty(dataPrefix))
                {
                    foreach (DataRow Row in this.DBConnection.DIDataSets().Rows)
                    {
                        if (Row[DBAvailableDatabases.AvlDBPrefix].ToString().ToUpper() == dataPrefix)
                        {
                            this.DataPrefix = dataPrefix;
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(this.DataPrefix))
                {
                    //get default dataset from database
                    this.DataPrefix = this.DBConnection.DIDataSetDefault();
                }

                //check languagecode exists in the database
                if (!string.IsNullOrEmpty(langaugeCode))
                {
                    if (this.DBConnection.IsValidDILanguage(this.DataPrefix, langaugeCode))
                    {
                        this.LanguageCode = langaugeCode;
                    }
                    else
                    {
                        //If language code doesnt exist then create langauge tables.
                        LanguageBuilder NewLanguage = new LanguageBuilder(this.DBConnection, this.DBQueries);
                        NewLanguage.CreateNewLanguageTables(langaugeCode, langaugeCode, this.DataPrefix);
                        this.LanguageCode = "_" + langaugeCode;
                    }
                }

                if (string.IsNullOrEmpty(this.LanguageCode))
                {
                    // get default language code 
                    this.LanguageCode = this.DBConnection.DILanguageCodeDefault(this.DataPrefix);
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }




        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/ Properties --"

        public AreaBuilder DIArea;
        public IUSBuilder DIIUS;
        public TimeperiodBuilder DITimeperiod;
        public SourceBuilder DISource;
        public FootnoteBuilder DIFootnote;

        #endregion

        #region "-- New / Dispose --"

        /// <summary>
        /// Returns instance of DIDatabase.
        /// </summary>
        public DIDatabase()
        {
            //dont implement this
        }

        /// <summary>
        /// Returns instance of DIDatabase.
        /// </summary>
        /// <param name="fileNameWPath">Databse file name with path</param>
        public DIDatabase(string fileNameWPath)
            : this(fileNameWPath, string.Empty)
        {
            //dont implement this
        }

        /// <summary>
        /// Returns instance of DIDatabase.
        /// </summary>
        /// <param name="fileNameWPath">Database file name with path</param>
        /// <param name="datasetPrefix">Dataset prefix like "UT_"</param>
        public DIDatabase(string fileNameWPath, string datasetPrefix)
            : this(fileNameWPath, datasetPrefix, string.Empty)
        {
            //dont implement this
        }

        /// <summary>
        /// Returns instance of DIDatabase.
        /// </summary>
        /// <param name="fileNameWPath">Database file name with path</param>
        /// <param name="datasetPrefix">Dataset prefix like "UT_"</param>
        /// <param name="langaugeCode">Language code like "_en"</param>
        public DIDatabase(string fileNameWPath, string datasetPrefix, string langaugeCode)
        {
            this.OpenDatabase(fileNameWPath, datasetPrefix, langaugeCode);

        }

        /// <summary>
        /// Returns instance of DIDatabase.
        /// </summary>
        /// <param name="connection">Instance of DIConnection which is already opened</param>
        /// <param name="queries">Instance of DIQueries which is already opened</param>
        public DIDatabase(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
            this.DIArea = new AreaBuilder(this.DBConnection, this.DBQueries);
            this.DIIUS = new IUSBuilder(this.DBConnection, this.DBQueries);
            this.DITimeperiod = new TimeperiodBuilder(this.DBConnection, this.DBQueries);
            this.DISource = new SourceBuilder(this.DBConnection, this.DBQueries);
            this.DIFootnote = new FootnoteBuilder(this.DBConnection, this.DBQueries);
        }

        #region IDisposable Members

        //public void Dispose()
        //{
        //    if (this.DBConnection != null)
        //    {
        //        this.DBConnection.Dispose();
        //    }

        //}

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            DBMetadataTableBuilder DBMetadataTable;

            try
            {
                if (this.DBConnection != null)
                {
                    // update counts in DBMetadata table( only if database/template is in DI6 format)        
                    DBMetadataTable = new DBMetadataTableBuilder(this.DBConnection, this.DBQueries);
                    if (DBMetadataTable.IsDBMetadataTableExists())
                    {
                        DBMetadataTable.GetNUpdateCounts();
                    }

                    // update database name in DB_Available table
                    this.DBConnection.InsertNewDBFileName(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(System.IO.Path.GetFileName(this.FileNameWPath)));


                    //update indicator unit and subgroup nids in Data table
                    this.UpdateIndicatorUnitSubgroupNIDsInData();


                    // dispose source database connection                    
                    this.DBConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Create and add record into database.
        /// </summary>
        /// <param name="areaInfo">object of AreaInfo</param>
        /// <param name="iusInfo">object of IUSInfo</param>
        /// <param name="timeperiod">Timeperiod</param>
        /// <param name="source">Source Name</param>
        /// <param name="dataValue">Data Value</param>
        /// <returns></returns>
        public int AddDataPoint(AreaInfo areaInfo, IUSInfo iusInfo, string timeperiod, string source, string dataValue)
        {
            int RetVal = 0;


            TimeperiodInfo TimeperiodObject = new TimeperiodInfo();
            SourceInfo SourceObject = new SourceInfo();

            if (areaInfo == null | string.IsNullOrEmpty(iusInfo.IndicatorInfo.Name) |
                string.IsNullOrEmpty(iusInfo.UnitInfo.Name) |
                string.IsNullOrEmpty(iusInfo.SubgroupValInfo.Name) |
                string.IsNullOrEmpty(timeperiod) | string.IsNullOrEmpty(source))
            {
                //do nothing

            }
            else
            {
                //set timeperiod info
                TimeperiodObject.Nid = 0;
                TimeperiodObject.TimeperiodValue = timeperiod;

                //set source info
                SourceObject.Nid = 0;
                SourceObject.Name = source;

                //insert data
                RetVal = this.AddDataPoint(areaInfo, iusInfo, TimeperiodObject, SourceObject, dataValue);
            }

            return RetVal;
        }

        /// <summary>
        /// Create and add record into database.
        /// </summary>
        /// <param name="areaInfo"></param>
        /// <param name="iusInfo"></param>
        /// <param name="timeperiodInfo"></param>
        /// <param name="sourceInfo"></param>
        /// <returns></returns>
        public int AddDataPoint(AreaInfo areaInfo, IUSInfo iusInfo, TimeperiodInfo timeperiodInfo, SourceInfo sourceInfo, string dataValue)
        {
            int RetVal = 0;
            int AreaNid = 0;
            int IUSNid = 0;
            int TimeperiodNid = 0;
            int SourceNid = 0;

            // Step 1: Area
            if (areaInfo.Nid <= 0)
            {
                AreaNid = this.DIArea.CheckNCreateArea(areaInfo);
            }
            else
            {
                AreaNid = areaInfo.Nid;
            }


            // Step 2: IUS, check for IUS only if IUSNid is zero.
            if (iusInfo.Nid <= 0)
            {
                IUSNid = this.DIIUS.CheckNCreateIUS(iusInfo);
            }
            else
            {
                IUSNid = iusInfo.Nid;
            }

            // Step 3: Timeperiod
            if (timeperiodInfo.Nid <= 0)
            {
                TimeperiodNid = this.DITimeperiod.CheckNCreateTimeperiod(timeperiodInfo.TimeperiodValue);
            }
            else
            {
                TimeperiodNid = timeperiodInfo.Nid;
            }

            // Step 4: Source
            if (sourceInfo.Nid <= 0)
            {
                SourceNid = this.DISource.CheckNCreateSource(sourceInfo.Name);
            }
            else
            {
                SourceNid = sourceInfo.Nid;
            }


            // Step 5: Insert Data and source , IUSNId & sourceparent ,IUSNID relationship into IC_IUS table
            RetVal = this.CheckNCreateData(AreaNid, IUSNid, SourceNid, TimeperiodNid, dataValue);
            return RetVal;
        }

        /// <summary>
        /// Insert DevInfo data value  into database.
        /// </summary>
        /// <param name="areaInfo">Object of AreaInfo</param>
        /// <param name="indicatorName">Indicator name</param>
        /// <param name="unitName">unit name</param>
        /// <param name="subgroupValName">SubgroupVal Name</param>
        /// <param name="timeperiod">Time Period </param>
        /// <param name="source">Source Name </param>
        /// <param name="dataValue">data value</param>
        /// <returns></returns>
        public int AddDataPoint(AreaInfo areaInfo, string indicatorName, string unitName, string subgroupValName, string timeperiod, string source, string dataValue)
        {
            int RetVal = 0;
            IUSInfo IUSInfo = new IUSInfo();

            // check and create IUS value
            IUSInfo.IndicatorInfo.Name = indicatorName;
            IUSInfo.UnitInfo.Name = unitName;
            IUSInfo.SubgroupValInfo.Name = subgroupValName;

            //check and create other elements
            RetVal = this.AddDataPoint(areaInfo, IUSInfo, timeperiod, source, dataValue);

            return RetVal;
        }


        /// <summary>
        /// To open or create DevInfo Database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void OpenDatabase(string fileNameWPath)
        {
            this.OpenDatabase(fileNameWPath, string.Empty);
        }

        /// <summary>
        /// To open or create DevInfo Database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="datasetPrefix"></param>
        public void OpenDatabase(string fileNameWPath, string datasetPrefix)
        {
            this.OpenDatabase(fileNameWPath, datasetPrefix, string.Empty);
        }


        /// <summary>
        /// To open or create DevInfo database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="datasetPrefix"></param>        
        /// <param name="langaugeCode"></param>
        public void OpenDatabase(string fileNameWPath, string datasetPrefix, string langaugeCode)
        {
            DBConverterDecorator DBConverter;

            //check file exists or not. If not exists then create it.
            if (!File.Exists(fileNameWPath))
            {
                DIDatabase.CreateDevInfoDBFile(fileNameWPath);

                // update the db schema
                DBConverter = new DBConverterDecorator(fileNameWPath);
                DBConverter.DoConversion(false);
                DBConverter.Dispose();
            }

            this.SetDatabaseObjects(fileNameWPath, datasetPrefix, langaugeCode);

            this.DIArea = new AreaBuilder(this.DBConnection, this.DBQueries);
            this.DIIUS = new IUSBuilder(this.DBConnection, this.DBQueries);
            this.DITimeperiod = new TimeperiodBuilder(this.DBConnection, this.DBQueries);
            this.DISource = new SourceBuilder(this.DBConnection, this.DBQueries);
            this.DIFootnote = new FootnoteBuilder(this.DBConnection, this.DBQueries);
        }

        /// <summary>
        /// Convets Database into template
        /// </summary>
        public void ConvertDatabaseToTemplate()
        {
            SourceBuilder SrcBuilder;
            FootnoteBuilder FoonotesBuilder;
            IndicatorClassificationBuilder ICBuilder;
            RecommendedSourcesBuilder RecommendedSrcBuilder;
            DataTable ICTable;
            string ICNIds = string.Empty;

            try
            {
                // STEP 1: Remove records from DATA table
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Data.DeleteRecords(string.Empty));

                // STEP 2: Remove records from TIME table
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Timeperiod.DeleteRecords(string.Empty));

                // STEP 3: Remove records from SOURCE table
                SrcBuilder = new SourceBuilder(this.DBConnection, this.DBQueries);
                SrcBuilder.DeleteSources(string.Empty);

                // STEP 4: Remove source records from Indicator_classification tables
                ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);

                ICTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.Type, "'SR'", FieldSelection.NId));
                if (ICTable != null && ICTable.Rows.Count > 0)
                {
                    ICNIds = DIConnection.GetDelimitedValuesFromDataTable(ICTable, IndicatorClassifications.ICNId);
                }
                ICBuilder.DeleteClassification(ICNIds);


                // STEP 5: Remove records from Footnotes table
                FoonotesBuilder = new FootnoteBuilder(this.DBConnection, this.DBQueries);
                FoonotesBuilder.DeleteFootnote(string.Empty);

                // STEP 6: Creates DevInfoSP2 tables if missing.
                DICommon.CheckNCreateDevInfoSP2Database(this.DBConnection, this.DBQueries, false);

                // STEP 7: remove records from notes table
                this.ClearNotesTables();


                // STEP 8: remove records from RecommendedSources table
                RecommendedSrcBuilder = new RecommendedSourcesBuilder(this.DBConnection, this.DBQueries);
                RecommendedSrcBuilder.DeleteRecommendedSources(string.Empty);


                // STEP 9:Remove Source metadata reports
                new DI7MetaDataBuilder(this.DBConnection, this.DBQueries).DeleteMetadataReports(MetadataElementType.Source);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Clears Notes tables
        /// </summary>
        public void ClearNotesTables()
        {
            DITables TableNames;
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesProfile(this.DBQueries.TablesName.NotesProfile, string.Empty));

                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesData(this.DBQueries.TablesName.NotesProfile, string.Empty));
            }
            catch (Exception)
            { }


            foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
            {
                try
                {
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
                    // Delete records from Notes
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotes(TableNames.Notes, string.Empty));

                    // Delete records from NotesClassification
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesClassification(TableNames.NotesClassification, string.Empty));
                }
                catch (Exception)
                { }
            }

        }


        /// <summary>
        /// Check existance of Data into database if is not found then create data otherwise update the datavalue
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid</param>
        /// <param name="dataValue">Data Value</param>
        /// <returns>Data Nid</returns>
        public int CheckNCreateData(int areaNid, int IUSNid, int sourceNid, int timeperiodNid, string dataValue)
        {
            return this.CheckNCreateData(areaNid, IUSNid, sourceNid, timeperiodNid, dataValue, -1);
        }


        /// <summary>
        /// Check existance of Data into database if is not found then create data otherwise update the datavalue
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid</param>
        /// <param name="dataValue">Data Value</param>
        /// <param name="footNoteNId">Footnote nid</param>
        /// <returns>Data Nid</returns>
        public int CheckNCreateData(int areaNid, int IUSNid, int sourceNid, int timeperiodNid, string dataValue, int footNoteNId, int denominatorValue)
        {
            int RetVal = 0;

            RetVal = this.CheckNCreateData(areaNid, IUSNid, sourceNid, timeperiodNid, dataValue, footNoteNId, true);

            // update denominator value
            if (RetVal > 0)
            {
                this.UpdateDenominatorValue(RetVal, denominatorValue);
            }

            return RetVal;
        }


        /// <summary>
        /// Check existance of Data into database if is not found then create data otherwise update the datavalue
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid</param>
        /// <param name="dataValue">Data Value</param>
        /// <param name="footNoteNId">Footnote nid</param>
        /// <returns>Data Nid</returns>
        public int CheckNCreateData(int areaNid, int IUSNid, int sourceNid, int timeperiodNid, string dataValue, int footNoteNId)
        {
            int RetVal = 0;

            RetVal = this.CheckNCreateData(areaNid, IUSNid, sourceNid, timeperiodNid, dataValue, footNoteNId, true);

            return RetVal;
        }


        /// <summary>
        /// Check existance of Data into database if is not found then create data otherwise update the datavalue
        /// </summary>
        /// <param name="areaNid">Area Nid</param>
        /// <param name="IUSNid">IUS Nid</param>
        /// <param name="sourceNid">Source Nid</param>
        /// <param name="timeperiodNid">Timeperiod Nid</param>
        /// <param name="dataValue">Data Value</param>
        /// <param name="footNoteNId">Footnote nid</param>
        /// <param name="updateSourceNIUSRelationshipInICIUSTable">True to update source and IUSNID relationship in ICIUSTable</param>
        /// <returns>Data Nid</returns>
        public int CheckNCreateData(int areaNid, int IUSNid, int sourceNid, int timeperiodNid, string dataValue, int footNoteNId, bool updateSourceNIUSRelationshipInICIUSTable)
        {
            int RetVal = 0;
            int SourceParentNId = -1;
            DataTable Table;

            // check data value already exists or not
            RetVal = this.CheckDataExists(areaNid, IUSNid, sourceNid, timeperiodNid);

            if (RetVal <= 0)
            {
                // insert data value into data table & return data nid
                RetVal = this.InsertDataValue(areaNid, IUSNid, sourceNid, timeperiodNid, dataValue, footNoteNId);
            }
            else
            {
                // update data value into data table & return data nid
                this.UpdateDataValue(RetVal, dataValue);

                // update footnote nid Into data table
                this.UpdateFootnoteNId(footNoteNId, RetVal.ToString());
            }


            if (updateSourceNIUSRelationshipInICIUSTable)
            {
                // check and update source and iusnid relationship in IC_IUS table
                this.InsertICAndIUSRelation(sourceNid, IUSNid);

                // get parent nid for the given sourceNId and insert source_Parent_nid relationship with IUSNId in IC_IUSNId table
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Source.GetSource(FilterFieldType.NId, sourceNid.ToString(), FieldSelection.Light, false));
                if (Table != null && Table.Rows.Count > 0)
                {
                    SourceParentNId = Convert.ToInt32(Table.Rows[0][IndicatorClassifications.ICParent_NId]);
                    this.InsertICAndIUSRelation(SourceParentNId, IUSNid);
                }

            }

            return RetVal;
        }


        /// <summary>
        /// Updates Source and IUS relation in IC_IUS table
        /// </summary>
        /// <param name="sourceNid"></param>
        /// <param name="IUSNid"></param>
        public void InsertSourceNIUSRelation(int sourceNid, int IUSNid)
        {
            DataTable Table;
            int SourceParentNId;

            // check and update source and iusnid relationship in IC_IUS table
            this.InsertICAndIUSRelation(sourceNid, IUSNid);

            // get parent nid for the given sourceNId and insert source_Parent_nid relationship with IUSNId in IC_IUSNId table
            Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Source.GetSource(FilterFieldType.NId, sourceNid.ToString(), FieldSelection.Light, false));
            if (Table != null && Table.Rows.Count > 0)
            {
                SourceParentNId = Convert.ToInt32(Table.Rows[0][IndicatorClassifications.ICParent_NId]);
                this.InsertICAndIUSRelation(SourceParentNId, IUSNid);
            }
        }



        /// <summary>
        /// Updates FootnoteNId for the given data NIds
        /// </summary>
        /// <param name="footnoteNId"></param>
        /// <param name="dataNIds">Comma separated data NID</param>
        public void UpdateFootnoteNId(int footnoteNId, string dataNIds)
        {
            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateFootnoteNId(this.DBQueries.DataPrefix, footnoteNId.ToString(), dataNIds));
        }


        /// <summary>
        /// Updates denominator value for the given data NId
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="denominator"></param>
        public void UpdateDenominatorValue(int dataNId, int denominator)
        {
            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateDenominatorValue(this.DBQueries.DataPrefix, dataNId, denominator));
        }

        /// <summary>
        /// Deletes records from  Data table
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteByIUSNIds(string nids)
        {
            this.DeleteDataValue(nids, string.Empty, string.Empty, string.Empty);
        }


        /// <summary>
        /// Deletes records from  Data table
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteByAreaNIds(string nids)
        {
            this.DeleteDataValue(string.Empty, string.Empty, nids, string.Empty);
        }

        /// <summary>
        /// Deletes records from  Data table
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteDataValue(string IUSNIDs, string timperiodNIds, string areaNIds, string sourceNIDs)
        {
            string SqlQuery = string.Empty;
            DataTable Table;
            string DataNIds = string.Empty;
            int ChunkCount = 300;
            string TempNids = string.Empty;
            string[] NidsArray;
            if (!string.IsNullOrEmpty(IUSNIDs) || !string.IsNullOrEmpty(timperiodNIds) || !string.IsNullOrEmpty(areaNIds) || !string.IsNullOrEmpty(sourceNIDs))
            {

                try
                {
                    // delete records from Data table

                    // step a: get datanids for the given IUSNIDs
                    SqlQuery = this.DBQueries.Data.GetDataByIUSTimePeriodAreaSource(IUSNIDs, timperiodNIds, areaNIds, sourceNIDs, FieldSelection.NId);
                    Table = this.DBConnection.ExecuteDataTable(SqlQuery);
                    if (Table != null && Table.Rows.Count > 0)
                    {
                        DataNIds = DIConnection.GetDelimitedValuesFromDataTable(Table, Data.DataNId);

                        if (DataNIds.Length > 0)
                        {
                            NidsArray = DataNIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            TempNids = string.Empty;

                            for (int Index = 0; Index < NidsArray.Length; Index += 300)
                            {
                                ChunkCount = (NidsArray.Length - Index >= 300) ? 300 : NidsArray.Length - Index;
                                TempNids = string.Join(",", NidsArray, Index, ChunkCount);

                                if (!string.IsNullOrEmpty(TempNids))
                                {
                                    // step b: delete records from Data table 
                                    SqlQuery = this.DBQueries.Delete.Data.DeleteRecords(TempNids);
                                    this.DBConnection.ExecuteNonQuery(SqlQuery);

                                    // step c: delete records from notes_data 
                                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesDataByDataNIds(this.DBQueries.TablesName.NotesData, TempNids);
                                    this.DBConnection.ExecuteNonQuery(SqlQuery);

                                    //delete records from recommendedSources from all language tables
                                    new RecommendedSourcesBuilder(this.DBConnection, this.DBQueries).DeleteRecommendedSources(TempNids);
                                }
                            }

                            //// step d: delete records from recommendedSources from all language tables
                            //new RecommendedSourcesBuilder(this.DBConnection, this.DBQueries).DeleteRecommendedSources(DataNIds);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Updates the NIDs of Indicator, unit and subgroup into Data table
        /// </summary>
        public void UpdateIndicatorUnitSubgroupNIDsInData()
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateIndicatorUnitSubgroupVAlNids(this.DBConnection.ConnectionStringParameters.ServerType, this.DBQueries.TablesName);

                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //Update iunids also
                this.UpdateIUNIDsInData();
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }


        }

        /// <summary>
        /// Updates the NIDs of Indicator, unit and subgroup into Data table
        /// </summary>
        public void UpdateIUNIDsInData()
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateIUNIds(this.DBQueries.TablesName, this.DBConnection.ConnectionStringParameters.ServerType);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region "-- Static --"

        /// <summary>
        /// Create blank devinfo database file.
        /// </summary>
        /// <param name="fileNameWPath">Database file path</param>
        /// <returns></returns>
        public static bool CreateDevInfoDBFile(string fileNameWPath)
        {

            return Resource.GetBlankDevInfoDBFile(fileNameWPath);
        }

        /// <summary>
        /// Updates footnote nids to -1 where footnote nid is 0 or empty 
        /// </summary>
        public void RemoveFootnoteNIdsInconsistencyInData()
        {
            //- It replaces 0 or null FootnoteNId by -1 in UT_data table.
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = "Update " + this.DBQueries.TablesName.Data + " Set " + Data.FootNoteNId + " = -1 WHERE " + Data.FootNoteNId + " is NULL or " + Data.FootNoteNId + " = 0";

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }



        public static bool CompactDataBase(ref DIConnection sourceDBConnection, DIQueries dbQueries, string destFilePath, bool disposeConnection, bool SeperateDataValueColumn)
        {
            bool RetVal = false;

            DIDatabase.SeperateDataValueColumn = SeperateDataValueColumn;
            RetVal = DIDatabase.CompactDataBase(ref sourceDBConnection, dbQueries, destFilePath, disposeConnection);

            //set true as default
            DIDatabase.SeperateDataValueColumn = true;

            return RetVal;
        }

        public static bool CompactNUpdateOnlineDataBase(ref DIConnection sourceDBConnection, DIQueries dbQueries)
        {
            bool RetVal = false;
            DIConnectionDetails SourceDBConnectionDetails;
            DBMetadataTableBuilder DBMetadataTable;

            DIDatabase DBDatabase;
            string DataPrefix = string.Empty;


            try
            {
                if (sourceDBConnection != null)
                {

                    // update counts in DBMetadata table( only if database/template is in DI6 format)
                    if (dbQueries == null)
                    {
                        DataPrefix = sourceDBConnection.DIDataSetDefault();

                        dbQueries = new DIQueries(DataPrefix, sourceDBConnection.DILanguageCodeDefault(DataPrefix));
                    }

                    DBMetadataTable = new DBMetadataTableBuilder(sourceDBConnection, dbQueries);
                    if (DBMetadataTable.IsDBMetadataTableExists())
                    {
                        DBMetadataTable.GetNUpdateCounts();
                    }


                    if (DIDatabase.SeperateDataValueColumn)
                    {
                        // Check orgTextual_Data_value exists or not. If column exists then move textual & numeric values into their respective column.
                        DIDataValueHelper.SeparateTextualandNemericData(sourceDBConnection, dbQueries);
                    }

                    // update indicator unit and subgroup nids in Data table
                    DBDatabase = new DIDatabase(sourceDBConnection, dbQueries);
                    DBDatabase.UpdateIndicatorUnitSubgroupNIDsInData();

                    // remove FootnoteNId inconsistency. (replace 0 or null FootnoteNId by -1 in UT_data table).
                    DBDatabase.RemoveFootnoteNIdsInconsistencyInData();



                    RetVal = true;
                }

            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Compact the database mainly Access database(.mdb) and save as specified destination file.
        /// </summary>
        ///<param name="sourceDBConnection"> Source database connection</param>
        /// <param name="destFilePath">destination file name with path.</param>
        /// <param name="disposeConnection"> Set true when calling this methods for finish process otherwise false.For only saving the database, pass False and for finish , pass true.  </param>
        /// <returns>true, if success.</returns>
        /// <remarks>Before calling this function,dont dispose source database connection </remarks>
        public static bool CompactDataBase(ref DIConnection sourceDBConnection, DIQueries dbQueries, string destFilePath, bool disposeConnection)
        {
            bool RetVal = false;
            bool IsFileOpen = false;
            DIConnectionDetails SourceDBConnectionDetails;
            DBMetadataTableBuilder DBMetadataTable;
            DIDatabase DBDatabase;
            string SourceDBNameWPath;
            JRO.JetEngine je;
            string DataPrefix = string.Empty;
            DI7MetadataCategoryBuilder DI7MetadataCategory;

            // -- NOTE: USE sDestFile only when sDestFile <> sSourceDB
            try
            {
                if (sourceDBConnection != null && !string.IsNullOrEmpty(destFilePath))
                {
                    //Drop index for ut_data table for IUSNID, areanid column
                    DIDatabase.DropDefaultIndex(sourceDBConnection);

                    // update counts in DBMetadata table( only if database/template is in DI6 format)
                    if (dbQueries == null)
                    {
                        DataPrefix = sourceDBConnection.DIDataSetDefault();

                        dbQueries = new DIQueries(DataPrefix, sourceDBConnection.DILanguageCodeDefault(DataPrefix));
                    }

                    DBMetadataTable = new DBMetadataTableBuilder(sourceDBConnection, dbQueries);
                    if (DBMetadataTable.IsDBMetadataTableExists())
                    {
                        DBMetadataTable.GetNUpdateCounts();
                    }
                    //Updating XSLT from Resourse File into database.
                    DI7MetadataCategory = new DI7MetadataCategoryBuilder(sourceDBConnection, dbQueries);
                    DI7MetadataCategory.UpdateXSLT(dbQueries.DataPrefix);

                    // update database name in DB_Available table
                    sourceDBConnection.InsertNewDBFileName(dbQueries.DataPrefix, DICommon.RemoveQuotes(System.IO.Path.GetFileName(destFilePath)));

                    if (DIDatabase.SeperateDataValueColumn)
                    {
                        // Check orgTextual_Data_value exists or not. If column exists then move textual & numeric values into their respective column.
                        DIDataValueHelper.SeparateTextualandNemericData(sourceDBConnection, dbQueries);
                    }

                    // update indicator unit and subgroup nids in Data table
                    DBDatabase = new DIDatabase(sourceDBConnection, dbQueries);
                    DBDatabase.UpdateIndicatorUnitSubgroupNIDsInData();

                    // remove FootnoteNId inconsistency. (replace 0 or null FootnoteNId by -1 in UT_data table).
                    DBDatabase.RemoveFootnoteNIdsInconsistencyInData();

                    // Update auto calculated columns ( IC table - Publisher, Year & title ,Indicator table- Data_Exists, area table - data_exist, IUS table - subgroup_nids & data_exist) into the database/template
                    DBDatabase.UpdateAutoCalculatedFieldsInTables();

                    // Update auto calculated column of DI7
                    DBDatabase.UpdateAutoCalculatedDI7FieldsInTables();

                    //Update those subgroupVals which is not associated with any subgroup type.Then insert association with others for those subgroup
                    DBDatabase.UpdateSubgroupValsInOthersSGDimensionInTables();

                    //Create index for ut_data table for IUSNID, areanid column
                    DIDatabase.CreateDefaultIndex(sourceDBConnection);

                    // dispose source database connection
                    SourceDBConnectionDetails = sourceDBConnection.ConnectionStringParameters;
                    SourceDBNameWPath = SourceDBConnectionDetails.DbName;
                    sourceDBConnection.Dispose();
                    System.Threading.Thread.Sleep(10);
                    try
                    {
                        if (File.Exists(destFilePath))
                        {
                            File.SetAttributes(destFilePath, FileAttributes.Normal);
                            File.Delete(destFilePath);
                        }
                    }
                    catch { }

                    //-- Copy SourceFile to temp file so that any existing connection on database shall not stop compact database process.
                    //string TempFile = DICommon.GetValidFileName(DateTime.Now.ToString()) + Path.GetExtension(SourceDBNameWPath);
                    //File.Copy(SourceDBNameWPath, TempFile, true);

                    try
                    {
                        // compacting the database
                        je = new JRO.JetEngine();
                        je.CompactDatabase("Data Source=\"" + SourceDBNameWPath + "\";Jet OLEDB:Database Password=" + SourceDBConnectionDetails.Password, "Data Source=\"" + destFilePath + "\";Jet OLEDB:Database Password=" + SourceDBConnectionDetails.Password);
                        je = null;

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Database already exists"))
                            IsFileOpen = true;
                    }

                    // reconnect to source database
                    if (!disposeConnection)
                    {
                        sourceDBConnection = new DIConnection(SourceDBConnectionDetails);
                    }

                    if (IsFileOpen == false)
                        RetVal = true;
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        #region "-- Index --"

        /// <summary>
        /// Create index for ut_data table for IUSNID, areanid column
        /// </summary>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public static void CreateDefaultIndex(DIConnection sourceDBConnection)
        {
            string DataPrefix = string.Empty;

            //Create index "Index_IUSNid_<DataPrefix>data"
            foreach (DataRow row in sourceDBConnection.DIDataSets().Rows)
            {
                DataPrefix = Convert.ToString(row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.DBAvailableDatabases.AvlDBPrefix]).TrimEnd("_".ToCharArray());
                sourceDBConnection.CreateIndex("Index_" + Data.IUSNId + "_" + DataPrefix + "Data", DataPrefix + "_data", Data.IUSNId);
                sourceDBConnection.CreateIndex("Index_" + Data.AreaNId + "_" + DataPrefix + "Data", DataPrefix + "_data", Data.AreaNId);
            }
        }

        /// <summary>
        /// Drop index for ut_data table for IUSNID, areanid column
        /// </summary>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public static void DropDefaultIndex(DIConnection sourceDBConnection)
        {
            string DataPrefix = string.Empty;

            //Drop index "Index_IUSNid_<DataPrefix>data"
            foreach (DataRow row in sourceDBConnection.DIDataSets().Rows)
            {
                DataPrefix = Convert.ToString(row[DBAvailableDatabases.AvlDBPrefix]).TrimEnd("_".ToCharArray());
                sourceDBConnection.DropIndex("Index_" + Data.IUSNId + "_" + DataPrefix + "Data", DataPrefix + "_data");
                sourceDBConnection.DropIndex("Index_" + Data.AreaNId + "_" + DataPrefix + "Data", DataPrefix + "_data");
            }
        }

        #endregion

        private void UpdateISMRDAndMultipleSource()
        {

            string Query = string.Empty;
            string TempTableName = "Temp1";

            try
            {
                DITables TableNames = this.DBQueries.TablesName;

                // 1. drop temp1 table 
                try
                {
                    this.DBConnection.ExecuteNonQuery("Drop table " + TempTableName);
                }
                catch (Exception)
                {

                }


                // 2. Create Temp1 table for IsMRD calculation
                Query = "Select MRDTable.*, T2.Timeperiod_nid into " + TempTableName + " from  ( SELECT  d.IUSNId, d.Area_NId, MAX(t.TimePeriod) AS timeperiod  FROM " + TableNames.Data + " d," + TableNames.TimePeriod + " t WHERE d.TimePeriod_NId= t.TimePeriod_NId GROUP BY d.IUSNId,d.Area_NId) AS MRDTable , " + TableNames.TimePeriod + " T2 where MRDTable.timeperiod=T2.Timeperiod";

                this.DBConnection.ExecuteNonQuery(Query);


                // 3. update IsMrd in DataTable
                Query = "UPDATE  " + TableNames.Data + "  AS d INNER JOIN  " + TempTableName + " AS t ON (d.Timeperiod_nid = t.TimePeriod_NId) AND (d.Area_NId = t.Area_NId) AND (d.IUSNId = t.IUSNId) SET d.IsMRD=1, d.MultipleSource=1";

                this.DBConnection.ExecuteNonQuery(Query);

                //4. drop table
                this.DBConnection.ExecuteNonQuery("Drop table " + TempTableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool UpdateAutoCalculatedDI7FieldsInTables()
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;


            try
            {
                //  update IsMRD UT_Dataa and Multiple source
                /////this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateISMRD(this.DBQueries.TablesName));
                this.UpdateISMRDAndMultipleSource();

                //  IUNId UT_Data	
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateIUNIds(this.DBQueries.TablesName, this.DBConnection.ConnectionStringParameters.ServerType));

                //  MultipleSource	UT_Data 
                //this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateISMultipleSource(this.DBQueries.TablesName));

                SqlQuery = "SELECT IUS." + Indicator_Unit_Subgroup.IUSNId + ", Min(D." + Data.DataValue + ") AS " + MINDATA + ", Max(D." + Data.DataValue + ") AS " + MAXDATA + ", Min(T." + Timeperiods.TimePeriod + ") as " + MINTIME + ",Max(T." + Timeperiods.TimePeriod + ") as " + MAXTIME + " FROM " + this.DBQueries.TablesName.TimePeriod + " AS T INNER JOIN (" + this.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN " + this.DBQueries.TablesName.Data + " AS D ON IUS." + Indicator_Unit_Subgroup.IUSNId + " = D." + Data.IUSNId + ") ON T." + Timeperiods.TimePeriodNId + " = D." + Data.TimePeriodNId + " GROUP BY IUS." + Indicator_Unit_Subgroup.IUSNId;

                DataTable IUSTable = this.DBConnection.ExecuteDataTable(SqlQuery);

                foreach (DataRow IUSRow in IUSTable.Rows)
                {
                    int IUSNId = Convert.ToInt32(IUSRow[Indicator_Unit_Subgroup.IUSNId]);
                    string MinDataValue = string.IsNullOrEmpty(Convert.ToString(IUSRow[MINDATA])) ? "0" : Convert.ToString(IUSRow[MINDATA]);
                    string MaxDataValue = string.IsNullOrEmpty(Convert.ToString(IUSRow[MAXDATA])) ? "0" : Convert.ToString(IUSRow[MAXDATA]);
                    //  Update AvlMinDataValue	value in   UT_Indicator_Unit_Subgroup	table
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.IUS.UpdateMinMaxDataAndTimeperiodValue(IUSNId, MinDataValue, MaxDataValue, Convert.ToString(IUSRow[MINTIME]), Convert.ToString(IUSRow[MAXTIME])));


                }

                DataTable TPTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, ""));
                //-- A-Annual, M-Monthly,Q-Quarterly,D-Daily
                foreach (DataRow Row in TPTable.Rows)
                {
                    string TimePeriod = Convert.ToString(Row[Timeperiods.TimePeriod]);
                    string Periodicity = "A";
                    //  StartDate UT_TimePeriod	Date/Time	
                    DateTime StartDate = default(DateTime);
                    //  EndDate UT_TimePeriod	Date/Time
                    DateTime EndDate = default(DateTime);

                    TimePeriodFacade.SetStartDateEndDate(TimePeriod, ref StartDate, ref EndDate);

                    //  Periodicity	    UT_TimePeriod	
                    Periodicity = this.GetPeriodicity(TimePeriod);

                    //-- Update Start date, End date and periodicity
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Update.UpdateTimeperiod(this.DBConnection.DIDataSetDefault(), Convert.ToString(Row[Timeperiods.TimePeriodNId]), TimePeriod, StartDate.ToShortDateString(), EndDate.ToShortDateString(), Periodicity));
                }


                //  CategoryGId	UT_Metadata_Category_en	Text	System derived as well as user input

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        public bool UpdateSubgroupValsInOthersSGDimensionInTables()
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;
            DataTable SubgroupTable = null;
            int SubgroupNid = 0;
            int Others_SubgroupTypeNid = -1;
            DI6SubgroupBuilder subgroupBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupInfo subgroupInfo = null;           

            try
            {

                subgroupBuilder = new DI6SubgroupBuilder(this.DBConnection,this.DBQueries);

                Others_SubgroupTypeNid = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.GId, "'OTHERS'")));

                if (Others_SubgroupTypeNid > 0)
                {
                    SqlQuery = "select * from " + this.DBQueries.TablesName.SubgroupVals + " sv where " + SubgroupVals.SubgroupValNId + " not in(select " + SubgroupVals.SubgroupValNId + " from " + this.DBQueries.TablesName.SubgroupValsSubgroup + " svs where svs." + SubgroupVals.SubgroupValNId + "=sv." + SubgroupVals.SubgroupValNId + ")";

                    //Get those subgroupVals which is not exists in ut_subgroup_val_subgroup
                    DataTable SubgroupValsTable = this.DBConnection.ExecuteDataTable(SqlQuery);

                    foreach (DataRow SGRow in SubgroupValsTable.Rows)
                    {
                        SubgroupTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.Name, "'" + Convert.ToString(SGRow[SubgroupVals.SubgroupVal]) + "'"));

                        if (SubgroupTable.Rows.Count == 0)
                        {
                            subgroupInfo = new DI6SubgroupInfo();
                            subgroupInfo.Name = Convert.ToString(SGRow[SubgroupVals.SubgroupVal]);
                            subgroupInfo.GID = Convert.ToString(SGRow[SubgroupVals.SubgroupValGId]);
                            subgroupInfo.Global = Convert.ToBoolean(SGRow[SubgroupVals.SubgroupValGlobal]);
                            subgroupInfo.Type = Others_SubgroupTypeNid;

                            SubgroupNid = subgroupBuilder.CheckNCreateSubgroup(subgroupInfo);
                        }
                        else
                        {
                            SubgroupNid = Convert.ToInt32(SubgroupTable.Rows[0][Subgroup.SubgroupNId]);
                        }

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup.Insert.InsertSubgroupValRelation(this.DBQueries.DataPrefix, Convert.ToInt32(SGRow[SubgroupVals.SubgroupValNId]), SubgroupNid));

                    }

                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        private string GetPeriodicity(string timeperiod)
        {

            string RetVal = "A";

            //-- A-Annual, M-Monthly,Q-Quarterly,D-Daily
            if (timeperiod.Length == 4 || timeperiod.Length == 9)	//-- YYYY/yyyy-yyyy
            {
                RetVal = "A";	//	A-Annual
            }
            else if (timeperiod.Length == 7 || timeperiod.Length == 6 || timeperiod.Length == 15 || timeperiod.Length == 13)            // -- YYYY.MM		
            {
                if (timeperiod.StartsWith("Q"))
                {
                    RetVal = "Q";   // Q-Quarterly
                }
                else
                {
                    //  M - Monthly
                    RetVal = "M";
                }
            }                //-- yyyy.mm.dd / yyyy.m.d
            else if (timeperiod.Length == 10 || timeperiod.Length == 8 || timeperiod.Length == 19
           || timeperiod.Length == 21 || timeperiod.Length == 17 || timeperiod.Length == 18 || timeperiod.Length == 20)
            {
                //    D- Daily
                RetVal = "D";
            }
            else
            {
                RetVal = "D";
            }

            return RetVal;

        }

        /// <summary>
        /// Updates GID for the given table 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="NID"></param>
        /// <param name="GID"></param>
        /// <returns></returns>
        public bool UpdateGID(string tableName, int NID, string GID)
        {
            bool RetVal = false;
            string LanguageCode = string.Empty;
            DITables TableName;
            string Query = string.Empty;

            string GivenTableName = string.Empty;
            string NIDColumn = string.Empty;
            string GIDColumn = string.Empty;

            try
            {
                TableName = new DITables(this.DBQueries.DataPrefix, string.Empty);
                if (tableName == this.DBQueries.TablesName.Area)
                {

                    GivenTableName = TableName.Area;
                    NIDColumn = Area.AreaNId;
                    GIDColumn = Area.AreaGId;
                }
                else if (tableName == this.DBQueries.TablesName.Indicator)
                {
                    GivenTableName = TableName.Indicator;
                    NIDColumn = Indicator.IndicatorNId;
                    GIDColumn = Indicator.IndicatorGId;
                }
                else if (tableName == this.DBQueries.TablesName.Unit)
                {
                    GivenTableName = TableName.Unit;
                    NIDColumn = Unit.UnitNId;
                    GIDColumn = Unit.UnitGId;
                }
                else if (tableName == this.DBQueries.TablesName.SubgroupType)
                {
                    GivenTableName = TableName.SubgroupType;
                    NIDColumn = SubgroupTypes.SubgroupTypeNId;
                    GIDColumn = SubgroupTypes.SubgroupTypeGID;
                }
                else if (tableName == this.DBQueries.TablesName.Subgroup)
                {
                    GivenTableName = TableName.Subgroup;
                    NIDColumn = Subgroup.SubgroupNId;
                    GIDColumn = Subgroup.SubgroupGId;
                }
                else if (tableName == this.DBQueries.TablesName.IndicatorClassifications)
                {
                    GivenTableName = TableName.IndicatorClassifications;
                    NIDColumn = IndicatorClassifications.ICNId;
                    GIDColumn = IndicatorClassifications.ICGId;
                }
                else if (tableName == this.DBQueries.TablesName.SubgroupVals)
                {
                    GivenTableName = TableName.SubgroupVals;
                    NIDColumn = SubgroupVals.SubgroupValNId;
                    GIDColumn = SubgroupVals.SubgroupValGId;
                }

                // update GID in language tables
                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();

                    // update GID
                    Query = "UPDATE " + GivenTableName + LanguageCode + " SET " + GIDColumn + "='" + DICommon.RemoveQuotes(GID) + "' WHERE " + NIDColumn + "=" + NID;
                    this.DBConnection.ExecuteNonQuery(Query);
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Update auto calculated columns ( IC table - Publisher, Year & title ,Indicator table- Data_Exists, area table - data_exist, IUS table - subgroup_nids & data_exist) into the database/template
        /// </summary>
        /// <returns></returns>
        public bool UpdateAutoCalculatedFieldsInTables()
        {
            bool RetVal = false;
            IndicatorClassificationBuilder ICBuilder;
            IndicatorBuilder IndBuilder;
            IUSBuilder IUSBuilderObj;
            AreaBuilder AreaBuilderObj;
            try
            {
                // 1. Indicator classification table - Publisher, year & title columns
                ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
                ICBuilder.UpdatePublisherTitleYear();

                // 2. Indicator table - Data_exist column
                IndBuilder = new IndicatorBuilder(this.DBConnection, this.DBQueries);
                IndBuilder.UpdateDataExistValues();

                // 3. IUS table- Subgroup_Nids & Data_Exist columns
                IUSBuilderObj = new IUSBuilder(this.DBConnection, this.DBQueries);
                IUSBuilderObj.UpdateSubgroupNids();
                IUSBuilderObj.UpdateDataExistValues();

                // 4. Area table- Data_Exist column
                AreaBuilderObj = new AreaBuilder(this.DBConnection, this.DBQueries);
                AreaBuilderObj.UpdateDataExistValues();

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Checks GID is already available in the given table or not.
        /// </summary>
        /// <param name="tableName">like DIQueriesObject.TableNames.Indicator</param>
        /// <param name="NID"></param>
        /// <param name="GID"></param>
        /// <returns></returns>
        public bool IsGIDExists(string tableName, int NID, string GID)
        {
            bool RetVal = false;
            string Query = string.Empty;
            string GIDColumn = string.Empty;
            string NIDColumn = string.Empty;

            try
            {
                // Get GID & NID column name
                if (tableName == this.DBQueries.TablesName.Area)
                {
                    NIDColumn = Area.AreaNId;
                    GIDColumn = Area.AreaGId;
                }
                else if (tableName == this.DBQueries.TablesName.Indicator)
                {
                    NIDColumn = Indicator.IndicatorNId;
                    GIDColumn = Indicator.IndicatorGId;
                }
                else if (tableName == this.DBQueries.TablesName.Unit)
                {
                    NIDColumn = Unit.UnitNId;
                    GIDColumn = Unit.UnitGId;
                }
                else if (tableName == this.DBQueries.TablesName.SubgroupType)
                {
                    NIDColumn = SubgroupTypes.SubgroupTypeNId;
                    GIDColumn = SubgroupTypes.SubgroupTypeGID;
                }
                else if (tableName == this.DBQueries.TablesName.Subgroup)
                {
                    NIDColumn = Subgroup.SubgroupNId;
                    GIDColumn = Subgroup.SubgroupGId;
                }
                else if (tableName == this.DBQueries.TablesName.SubgroupVals)
                {
                    NIDColumn = SubgroupVals.SubgroupValNId;
                    GIDColumn = SubgroupVals.SubgroupValGId;
                }
                else if (tableName == this.DBQueries.TablesName.IndicatorClassifications)
                {
                    NIDColumn = IndicatorClassifications.ICNId;
                    GIDColumn = IndicatorClassifications.ICGId;
                }

                // check GID already exists or not

                Query = "Select Count(*) From " + tableName + " WHERE " + GIDColumn + "='" + DICommon.RemoveQuotes(GID) + "' and  " + NIDColumn + " NOT IN(" + NID + ")";

                if (Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(Query)) > 0)
                {
                    RetVal = true;
                }

            }
            catch (Exception)
            {
                RetVal = true;
            }

            return RetVal;
        }

        #endregion
    }

}
