using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.QDS.DI7CacheNSP;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.QDS.UI.Databases
{
    public delegate void ProgressChangedDelegate(int currentProgressValue,string label1,string label2,string label3,string label4);

    public  class UIDatabaseHelper
    {
        #region "-- Private --"
        
        #region "-- Methods --"

        private string[] GetDefaultAreasArr(DIConnection diConnection, DITables tableNames)
        {
            string[] RetVal = new string[3];
            StringBuilder TmpAreaJson = new StringBuilder();
            Dictionary<string, List<string>> TmpAreaForJson = new Dictionary<string, List<string>>();
            string CurrentAreaLevel = string.Empty;
            
            try
            {                
                DataTable dtAllDefaultAreas = DI7OfflineSPHelper.GetDefaultAreas(diConnection, tableNames);

                string CurrentAreaID = string.Empty;
                List<string> lstAreas = new List<string>();
                string tmpAreaInfo = string.Empty;

                //TmpAreaJson.Append("{'area' : {");

                //TmpAreaJson.Append("'1' : [");

                //foreach (DataRow dr in dtAllDefaultAreas.Rows)
                //{
                //    tmpAreaInfo += ",'" + dr["Area_NId"].ToString() + "|| " + dr["Area_Name"].ToString() + " (" + dr["Area_ID"].ToString() + ") - Level 1'";
                //}

                //if (!string.IsNullOrEmpty(tmpAreaInfo))
                //{
                //    tmpAreaInfo = tmpAreaInfo.Substring(1);
                //}
                //TmpAreaJson.Append(tmpAreaInfo);
                //TmpAreaJson.Append("],");

                //TmpAreaJson.Append("'2' : [");

                //tmpAreaInfo = "";

                //foreach (DataRow dr in dtAllDefaultAreas.Rows)
                //{
                //    tmpAreaInfo += ",'QS_" + dr["Area_ID"].ToString() + "_L2|| " + dr["Area_Name"].ToString() + " (" + dr["Area_ID"].ToString() + ") - Level 2'";
                //}

                //if (!string.IsNullOrEmpty(tmpAreaInfo))
                //{
                //    tmpAreaInfo = tmpAreaInfo.Substring(1);
                //}
                //TmpAreaJson.Append(tmpAreaInfo);
                //TmpAreaJson.Append("]");


                //TmpAreaJson.Append("}}");

                //TmpAreaJson.Replace("'", "\"");


                string[] AreaCount = this.GetAreasCount(dtAllDefaultAreas);

                RetVal[0] = AreaCount[0];
                RetVal[1] = TmpAreaJson.ToString();
                RetVal[2] = AreaCount[1];
            }
            catch (Exception)
            {                
            }

            return RetVal;
        }

        private string[] GetAreasCount(DataTable dtAllDefaultAreas)
        {
            string[] RetVal = new string[2];
            RetVal[0] = string.Empty;
            RetVal[1] = string.Empty;
            int TmpAreasCount = 0;

            try
            {                
                foreach (DataRow Row in dtAllDefaultAreas.Rows)
                {
                    string ID = "," + Row["Area_NId"].ToString() + ",QS_" + Row["Area_ID"].ToString() + "_L2";

                    if (!RetVal[0].Contains(ID))
                    {
                        RetVal[0] += ID;
                    }
                                        
                    if(!string.IsNullOrEmpty(Convert.ToString(Row["Children"])))
                    {
                        string Children = Row["Children"].ToString();
                        TmpAreasCount += int.Parse(Children);
                    }

                }

                if (RetVal[0].Length > 0)
                {
                    RetVal[0] = RetVal[0].Substring(1);
                }

                RetVal[1] = Convert.ToString(TmpAreasCount);
            }
            catch (Exception)
            {
            }
            
            return RetVal;
        }

        private string[] GetDefaultIndicatorsArr(DIConnection diConnection, DITables tableNames)
        {
            string[] RetVal = new string[2];

            RetVal[0] = string.Empty;
            RetVal[1] = string.Empty;

            try
            {                
                DataTable DtDefaultIndicators = DI7OfflineSPHelper.GetDefaultIndicators(diConnection, tableNames);

                List<string> DistinctIndicators = new List<string>();

                foreach (DataRow dr in DtDefaultIndicators.Rows)
                {
                    string tmpIndNId = "," + dr["IUSNId"].ToString();
                    if (!RetVal[0].Contains(tmpIndNId)) RetVal[0] += tmpIndNId;

                    //string I_U = dr["Indicator_NId"].ToString() + "~" + dr["Unit_NId"].ToString();
                    //string IndName_UnitName = dr["Indicator_Name"].ToString() + "~" + dr["Unit_Name"].ToString();
                    //string jsonIU = ",'" + I_U + "||" + IndName_UnitName.Replace("'", @"\'") + "'";

                    //if (!DistinctIndicators.Contains(jsonIU))
                    //{
                    //    result[1] += jsonIU;
                    //    DistinctIndicators.Add(jsonIU);
                    //}
                }

                if (RetVal[0].Length > 0) RetVal[0] = RetVal[0].Substring(1);
                if (RetVal[1].Length > 0) RetVal[1] = RetVal[1].Substring(1);

                //result[1] = "{'iu':[" + result[1] + "],'sg_dim':{},'sg_dim_val':{},'iusnid':{}}";

            }
            catch (Exception)
            {                
            }

            return RetVal;
        }

        private DataTable AddIndicatorDescCol(DataTable dtQDSResults, DITables tableNames, DIConnection dbConnection)
        {
            DataTable RetVal = dtQDSResults.Copy();
            string Area = string.Empty;
            string AreaLevel = string.Empty;
            //int QSAreaNId = -1;

            try
            {
                RetVal.Columns.Add("IndicatorDescription", typeof(System.String));

                foreach (DataRow Row in RetVal.Rows)
                {
                    Row["IndicatorDescription"] = this.GetIndicatorDescription((int)Row["IndicatorNId"], (string)Row["SearchLanguage"]);

                    if (string.IsNullOrEmpty(Convert.ToString(Row["DVSeries"])))
                    {                        
                        Area = Convert.ToString(Row["Area"]);
                        AreaLevel = DI7OfflineSPHelper.GetAreaLevelName(dbConnection, tableNames, Row["AreaNId"].ToString());
                        Area += " - " + AreaLevel;
                        Row["Area"] = Area;

                        //Row["AreaNId"] = QSAreaNId--;
                    }
                }
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        private string GetIndicatorDescription(int indicatorNId, string searchLanguage)
        {
            string RetVal = string.Empty;
            XmlDocument MetadataDocument = null;
            string XmlFileWithPath = string.Empty;

            try
            {
                MetadataDocument = new XmlDocument();

                XmlFileWithPath = Path.Combine(this._XmlDataFilesPath, searchLanguage + @"\metadata\indicator\" + indicatorNId.ToString() + ".xml");

                MetadataDocument.Load(XmlFileWithPath);
                foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("ReportedAttribute"))
                {
                    //todo - if (Category.Attributes["id"].Value.ToUpper() == "DEFINITION")
                    if (Category.Attributes["name"].Value.ToUpper() == "DEFINITION")                    
                    {
                        RetVal = Category.InnerText;
                        RetVal = RetVal.Replace("{{~}}", "").Replace("\n", "").Replace("\t", "").Replace("\"", "\\\"");
                        break;
                    }

                }

                RetVal = RetVal.Trim();
            }
            catch (Exception)
            {                
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Public --"

        private string _DatabasePath;
        public string DatabasePath
        {
            get { return _DatabasePath; }
            set { _DatabasePath = value; }
        }

        private string _XmlDataFilesPath;
        public string XmlDataFilesPath
        {
            get { return _XmlDataFilesPath; }
            set { _XmlDataFilesPath = value; }
        }
		
        #region "-- Events --"

        public event ProgressChangedDelegate ProgressChangedEvent;
        public event EventHandler DisplayProgressFormEvent;
        
        public void RaiseProgressChangedEvent(int value, string languageCode, string folderName,bool savingDatabase)
        {
            if (this.ProgressChangedEvent != null)
            {
                // todo: language handling for string
                if (!string.IsNullOrEmpty(languageCode))
                {
                    languageCode = "Generating xml files for:" + languageCode;
                }

                if (!string.IsNullOrEmpty(folderName))
                {
                    folderName="XML Files:" + folderName;
                }
                if (savingDatabase)
                {
                    languageCode = "Saving database details...";
                }

                this.ProgressChangedEvent(value, "Optimizing database", languageCode, folderName, string.Empty);
            }
        }

        public void RaiseDisplayProgressFormEvent()
        {
            if (this.DisplayProgressFormEvent != null)
            {
                this.DisplayProgressFormEvent(this,null);
            }
        }
        
        #endregion

        #region "-- Methods --"

        #region "-- XML generation --"

        /// <summary>
        /// Get available database name of selected mdb file
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        public  string GetAvailableDbName(string databasePath)
        {
            string RetVal = string.Empty;
            DIConnection DBConnection = null;

            try
            {
                DBConnection = new DIConnection(DIServerType.MsAccess, "", "", databasePath, "", "");

                RetVal = GetAvailableDbName(DBConnection);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                    DBConnection = null;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get available database name of selected database connection
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public string GetAvailableDbName(DIConnection dbConnection)
        {
            string RetVal = string.Empty;
            DataTable DtResult = null;

            try
            {
                DtResult = dbConnection.DIDataSets();

                foreach (DataRow Row in DtResult.Rows)
                {
                    if (Convert.ToString(Row["AvlDB_Default"]).ToLower() == "true")
                    {
                        RetVal = Convert.ToString(Row["AvlDB_Name"]);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// Generate data xml files and enter db info in xml DatabaseDetails file.
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="tempDatabaseFolderPath"></param>
        /// <param name="dbDetails"></param>
        /// <param name="dbDetailsXmlFileWithPath"></param>
        public bool GenerateXmlFilesNLog(string tempDatabaseFolderPath, UIDatabaseDetails uiDBDetails, string dbDetailsXmlFileWithPath, string xmlDataPath)
        {
            bool RetVal = false;
            string TmpDatabaseFilenameWithPath = string.Empty;
            string DbFileName = string.Empty;
            string XmlDataOutputFolderpath = string.Empty;
            string DataFolerName = string.Empty;
            UIDatabaseInfo CurrentDBInfo = null;
            XMLGenerator ObjXMLGenerator = null;
            CacheGenerator ObjCacheGenerator = null;
            string AvlDbName = string.Empty;
            int ProgressCount = 3;

            try
            {
                //-- raise event to display progress form
                this.RaiseDisplayProgressFormEvent();

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                // check n create temp database folder
                if (!Directory.Exists(tempDatabaseFolderPath))
                {
                    Directory.CreateDirectory(tempDatabaseFolderPath);
                }

                // get database file and copy it into temp
                DbFileName = Path.GetFileName(this._DatabasePath);
                TmpDatabaseFilenameWithPath = Path.Combine(tempDatabaseFolderPath, DbFileName);


                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Get Available database name
                AvlDbName = GetAvailableDbName(this._DatabasePath);

                //-- Get all database details                
                if (uiDBDetails.DatabaseInfo.Count > 0)
                {
                    // get current database info
                    CurrentDBInfo = uiDBDetails.DatabaseInfo.Find(delegate(UIDatabaseInfo p) { return p.DatabaseName == AvlDbName; });
                }

                // increment progress bar value
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                if (CurrentDBInfo == null || CurrentDBInfo.LastModifiedDataNTime != File.GetLastWriteTime(this._DatabasePath).ToString() || CurrentDBInfo.IsXmlFilesGenerated == false)
                {
                    //-- Copy db into temp foler                
                    File.Copy(this._DatabasePath, TmpDatabaseFilenameWithPath, true);

                    // increment progress bar value
                    this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);
                    
                    //-- Delete old database info entry if exists
                    if (CurrentDBInfo != null)
                    {
                        uiDBDetails.DatabaseInfo.Remove(CurrentDBInfo);
                    }

                    //-- Create current database info
                    CurrentDBInfo = new UIDatabaseInfo();
                    CurrentDBInfo.DatabaseName = AvlDbName;
                    CurrentDBInfo.DatabaseFilename = DbFileName;
                    CurrentDBInfo.LastModifiedDataNTime = File.GetLastWriteTime(this._DatabasePath).ToString();

                    //-- Set DatabaseInfo details
                    XmlDataOutputFolderpath = Path.Combine(tempDatabaseFolderPath, "data");
                    ObjXMLGenerator = new XMLGenerator(XmlDataOutputFolderpath, this._DatabasePath);
                    DataFolerName = Path.GetFileNameWithoutExtension(this._DatabasePath);

                    //-- add events
                    ObjXMLGenerator.ProgressChangedEvent += new ProgressChangedDelegate(ObjXMLGenerator_ProgressChangedEvent);

                    //-- Generate XML files
                    ObjXMLGenerator.GenerateDefaultXmlFiles(DataFolerName, "AreaName", "Immediate");

                    ProgressCount = 19;
                    // increment progress bar value
                    this.RaiseProgressChangedEvent(ProgressCount++, "Genetating cache", string.Empty, false);

                    // Genetating cache
                    ObjCacheGenerator = new CacheGenerator();

                    //-- add events
                    ObjCacheGenerator.ProgressChangedEvent += new ProgressChangedDelegate(ObjCacheGenerator_ProgressChangedEvent);                                 
                    //-- Generate Cache in db
                    ObjCacheGenerator.GenerateCacheResults(TmpDatabaseFilenameWithPath);
                    
                    ProgressCount = 37;
                    // increment progress bar value
                    this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                    //-- update xml files generation status into DesktopDatabaseDetails.xml file
                    CurrentDBInfo.IsXmlFilesGenerated = true;                    

                    // add current db info into database details xml file
                    uiDBDetails.DatabaseInfo.Add(CurrentDBInfo);                    

                    // update database details xml file
                    uiDBDetails.Save(dbDetailsXmlFileWithPath);

                    // increment progress bar value
                    this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, true);
                                        
                    RetVal = true;
                }
                else
                {
                    // do nothing                  
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        void ObjXMLGenerator_ProgressChangedEvent(int currentProgressValue, string label1, string label2, string label3, string label4)
        {
            this.RaiseProgressChangedEvent(currentProgressValue, label1, label2, false);
        }

        void ObjCacheGenerator_ProgressChangedEvent(int currentProgressValue, string label1, string label2, string label3, string label4)
        {
            this.RaiseProgressChangedEvent(currentProgressValue, label1, label2, false);
        }

        #endregion
        
        #region "-- QDS result --"

        public DataSet GetQDSResults(DIConnection dbConnection, string searchIndicators, string searchICs, string searchAreas, string searchLanguage, bool isBlockResults)
        {
            DataSet RetVal = new DataSet();
            string CatalogAdaptationUrl = string.Empty;                        
            DataTable DtQDSResults = new DataTable();
            DataTable DtQDSChildResults = new DataTable();
            string NumericSearchAreas = string.Empty;
            string QSSearchAreas = string.Empty;
            DITables TableNames;
           
            try
            {
                //-- Get all tables by dataset and language basis
                TableNames = new DITables(dbConnection.DIDataSetDefault(), searchLanguage);

                if (string.IsNullOrEmpty(searchAreas))
                {                    
                    string[] DefaultAreaArr = this.GetDefaultAreasArr(dbConnection, TableNames);
                    if (DefaultAreaArr.Length > 0)
                    {
                        searchAreas = DefaultAreaArr[0];                       
                    }
                }

                #region "-- Seperate Normal and QS areas  --"

                //-- Seperate Numric and QS area in variable
                foreach (string SptText in DICommon.SplitString(searchAreas, ","))
                {
                    if (DICommon.IsNumeric(SptText))
                    {
                        NumericSearchAreas += "," + SptText;
                    }
                    else
                    {
                        QSSearchAreas += "," + SptText;
                    }
                }

                if (!string.IsNullOrEmpty(NumericSearchAreas))
                {
                    searchAreas = NumericSearchAreas.Substring(1);
                }
                else
                {
                    searchAreas = string.Empty;
                }

                if (!string.IsNullOrEmpty(QSSearchAreas))
                {
                    QSSearchAreas = QSSearchAreas.Substring(1);
                }

                #endregion
                
                if (string.IsNullOrEmpty(searchIndicators))
                {                    
                    string[] DefaultIndArr = this.GetDefaultIndicatorsArr(dbConnection, TableNames);
                    if (DefaultIndArr.Length > 0)
                    {
                        searchIndicators = DefaultIndArr[0];
                    }                    
                }
                else
                {
                    #region "-- Seperate indicator and IC --"

                    string IndNIds = string.Empty;

                    //-- Seperate Numric and QS area in variable
                    foreach (string SptText in DICommon.SplitString(searchIndicators, ","))
                    {
                        if (SptText.IndexOf("ic_") > -1)
                        {
                            IndNIds += "," + DI7OfflineSPHelper.GetIndicatorNIdsFromICNId(dbConnection, TableNames, Convert.ToInt32(SptText.Substring(3)));
                        }
                        else
                        {
                            IndNIds += "," + SptText;
                        }
                    }
                   
                    if (!string.IsNullOrEmpty(IndNIds))
                    {
                        searchIndicators = IndNIds.Substring(1);
                    }

                    #endregion
                }                

                #region "-- Get Parent Data --"

                //-- Create an empty Parent Table, TmpSearchAreaa, TmpSearchIndicators
                CacheUtility.CreateParentTbl(dbConnection, searchLanguage);
                DI7OfflineSPHelper.CreateTmpAreaSearchTbl(dbConnection, searchAreas, false);                
                DI7OfflineSPHelper.CreateTmpIndSearchTbl(dbConnection, TableNames, searchIndicators, false);
                
                //-- Get Record for Normal areas
                if (!string.IsNullOrEmpty(searchAreas))
                {                       
                    DI7OfflineSPHelper.GetSearchResults(dbConnection, searchLanguage, false);
                }                                                             

                #endregion "-- Get Parent Data --"
                
                #region "-- Get Child Data --"

                //-- Create an empty Child Table
                CacheUtility.CreateChildTbl(dbConnection, searchLanguage);               

                //-- Get Record for Normal areas
                if (!string.IsNullOrEmpty(searchAreas))
                {                    
                    DtQDSChildResults = DI7OfflineSPHelper.GetSearchChildResults(dbConnection, searchLanguage, searchIndicators, false);
                }

                //-- Get Record for QS areas
                if (!string.IsNullOrEmpty(QSSearchAreas))
                {
                    //-- Create TmpSearchAreaa for QS areas
                    DI7OfflineSPHelper.CreateTmpAreaSearchTbl(dbConnection, QSSearchAreas, true);

                    DtQDSChildResults = DI7OfflineSPHelper.GetSearchChildResults(dbConnection, searchLanguage, searchIndicators, true);
                }

                #endregion "-- Get Child Data --"

                #region "-- Reprocess Parent records --"

                //-- Create an empty Child Area Table
                CacheUtility.CreateChildAreaTbl(dbConnection, searchLanguage);

                //-- Get distinct Indicator,unit,area … from ChildTable table and insert into TmpChildArea table 
                DI7OfflineSPHelper.InsertIntoTmpChildAreaTable(dbConnection, searchLanguage, TableNames);
                

                //-- Use union (first select – parenttable, second table – tmpchildarea) and create new parent table
                DI7OfflineSPHelper.GetNewParentTable(dbConnection, searchLanguage);

                //-- Delete duplicate record for same IUS
                DtQDSResults = DI7OfflineSPHelper.DeletedDuplicateRecordOfSameIUS(dbConnection);

                DtQDSResults = this.AddIndicatorDescCol(DtQDSResults, TableNames, dbConnection);
                
                #endregion

                //-- Add table into Dataset
                RetVal.Tables.Add(DtQDSResults);
                RetVal.Tables.Add(DtQDSChildResults);
            }
            catch (Exception)
            {
            }

            return RetVal;
        }        

        public DataTable GetQDSSubgroupDetails(DIConnection dbConnection, string languageCode)
        {
            DataTable RetVal = null;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            DITables TableNames;

            try
            {
                //-- Get table names for current dataset and language
                TableNames = new DITables(dbConnection.DIDataSetDefault(), languageCode);

                //-- Get QDSSubgroupDetailsTable                
                SBQry.Append("SELECT SVS." + SubgroupValsSubgroup.SubgroupValNId + ", sdv." + Subgroup.SubgroupName + " AS DimensionValue, SD." + SubgroupTypes.SubgroupTypeName + " AS Dimension,");
                SBQry.Append(" SD." + SubgroupTypes.SubgroupTypeNId + " AS DimensionNId");
                SBQry.Append(" FROM " + TableNames.SubgroupType + " AS SD INNER JOIN (" + TableNames.Subgroup + " AS SDV INNER JOIN "+ TableNames.SubgroupValsSubgroup +" AS SVS");
                SBQry.Append(" ON SDV." + Subgroup.SubgroupNId + " = SVS." + SubgroupValsSubgroup.SubgroupNId + ") ON SD." + SubgroupTypes.SubgroupTypeNId + " = SDV." + Subgroup.SubgroupType);
                StrQry = SBQry.ToString();
                RetVal = dbConnection.ExecuteDataTable(StrQry);
            }
            catch (Exception)
            {                
                throw;
            }

            return RetVal;
        }

        #endregion
        
        #endregion

        #endregion
    }
}
