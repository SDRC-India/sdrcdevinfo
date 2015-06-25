using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.DES;
using System.Data;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibSDMX;
//using DevInfo.Lib.DI_LibSDMX;

namespace DevInfo.Lib.DI_LibBAL.Import.Template
{
    public class DITemplate
    {
        #region -- Private --

        #region -- Variables --

        private DataEntrySpreadsheets Spreadsheets;
        private const string IndicatorString = "Indicator";
        private const string AreaIdString = "AreaId";
        private const string AreaGIdString = "AreaGId";
        private const string AreaLevelString = "AreaLevel";
        private const string AreaNameString = "AreaName";
        private const string AreaParentGIdString = "Parent AreaId";

        private Dictionary<AreaSheetColumnType, AreaSheetColumnInfo> AreaSheetColumns = new Dictionary<AreaSheetColumnType, AreaSheetColumnInfo>();

        #endregion

        #region -- Methods --

        private void UpdateTablesForTargetLanguage(string languageName, DIConnection dbConnection, DIQueries dbQueries, string dataPrefix, string languageCode)
        {
            LanguageBuilder DILanguageBuilder;

            // create langauge dependent tables if not exists in the new template
            if (!dbConnection.IsValidDILanguage(dataPrefix, languageCode))
            {
                DILanguageBuilder = new LanguageBuilder(dbConnection, dbQueries);
                DILanguageBuilder.CreateNewLanguageTables(languageCode, languageName, dataPrefix);

                // delete "_en" tables & delete langauge code from language table
                DILanguageBuilder.DropLanguageDependentTables(dataPrefix, "_en");

                // set default language
                dbConnection.ExecuteNonQuery(dbQueries.SetDefaultLanguageCode(languageCode));
            }
        }

        private IUSInfo GetIUSInfo(DataRow row)
        {
            IUSInfo RetVal = new IUSInfo();

            //indicator
            RetVal.IndicatorInfo = new IndicatorInfo();
            RetVal.IndicatorInfo.Name = row[Indicator.IndicatorName].ToString();
            RetVal.IndicatorInfo.GID = row[Indicator.IndicatorGId].ToString();

            //unit
            RetVal.UnitInfo = new UnitInfo();
            RetVal.UnitInfo.Name = row[Unit.UnitName].ToString();
            RetVal.UnitInfo.GID = row[Unit.UnitGId].ToString();

            //subgroup
            RetVal.SubgroupValInfo = new DI6SubgroupValInfo();
            RetVal.SubgroupValInfo.Name = row[SubgroupVals.SubgroupVal].ToString();
            RetVal.SubgroupValInfo.GID = row[SubgroupVals.SubgroupValGId].ToString();

            return RetVal;
        }

        #endregion

        #endregion

        #region -- Public --

        #region -- Variables --

        #endregion

        #region -- New/Dispose --

        public DITemplate()
        {

            //set area columns info
            this.AreaSheetColumns.Add(AreaSheetColumnType.AreaID, new AreaSheetColumnInfo(0, AreaIdString));
            this.AreaSheetColumns.Add(AreaSheetColumnType.AreaName, new AreaSheetColumnInfo(1, AreaNameString));
            this.AreaSheetColumns.Add(AreaSheetColumnType.AreaLevel, new AreaSheetColumnInfo(2, AreaLevelString));
            this.AreaSheetColumns.Add(AreaSheetColumnType.AreaGID, new AreaSheetColumnInfo(3, AreaGIdString));
            this.AreaSheetColumns.Add(AreaSheetColumnType.PareaGID, new AreaSheetColumnInfo(4, AreaParentGIdString));

        }

        #endregion

        #region -- Methods --

        public bool CreateTemplateFrmSDMX(string templateFileName, List<string> SDMXFilenames, string tempFolderPath, string languageCode, bool importMetadata,string sdmxMetadataWebservice)
        {
            bool RetVal = true;
            DIConnection DBConnection = null;
            DIQueries DBQueries;
            try
            {
                //create temp template file
                DIDatabase TempTemplateFile = new DIDatabase(templateFileName, "UT_", languageCode);

                // create database object
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, templateFileName, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), languageCode);

                // delete default categories from blank template
                DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Delete.DeleteMetadataCategory(DBQueries.TablesName.MetadataCategory, string.Empty));

                // import data from SDMX
                foreach (string DSDFilenameWPath in SDMXFilenames)
                {
                    //if (SDMXUtility.Validate_DSDFile(SDMXSchemaType.Two_One, DSDFilenameWPath))
                    //{
                        SDMXHelper.Import_DSD(SDMXSchemaType.Two_One, DSDFilenameWPath, DataFormats.StructureSpecificTS, "TempTemp", DBConnection, DBQueries, true);

                        if (importMetadata)
                        {
                            SDMXHelper.Import_Metadata(SDMXSchemaType.Two_One, "", sdmxMetadataWebservice, DBConnection, DBQueries);
                        }
                    //}
                    //else
                    //{
                    //    RetVal = false;
                    //    break;
                    //}
                }              

            }
            catch (Exception ex)
            {
                RetVal = false;

            }
            finally
            {
                // dispose database object
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }

            }

            return RetVal;
        }

        public bool CreateTemplateFrmSDMX(string templateFileName, List<string> SDMXFilenames, string tempFolderPath, string languageCode)
        {
           return this.CreateTemplateFrmSDMX(templateFileName, SDMXFilenames, tempFolderPath, languageCode, false,string.Empty); 
        }

        public bool CreateMetadataFromSDMX(string templateFileName,string tempFolderPath, string languageCode,string sdmxRegistryUrl)
        {
            bool RetVal = true;
            DIConnection DBConnection = null;
            DIQueries DBQueries;
            try
            {
                //create temp template file
                DIDatabase TempTemplateFile = new DIDatabase(templateFileName, "UT_", languageCode);

                // create database object
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, templateFileName, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), languageCode);

                // delete default categories from blank template
                DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Delete.DeleteMetadataCategory(DBQueries.TablesName.MetadataCategory, string.Empty));

                // import data from SDMX
                BaseSDMXHelper HelperObj = new BaseSDMXHelper(DBConnection, DBQueries);
                string TempXmlFile = tempFolderPath + "\\IMPORT_MSDXML_" + this.GetDateTimeStamp() + DICommon.FileExtension.XML;

                HelperObj.GenerateMetadataXMLFromSDMXWebservice(sdmxRegistryUrl, TempXmlFile);

            }
            catch (Exception ex)
            {
                RetVal = false;

            }
            finally
            {
                // dispose database object
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }

            }

            return RetVal;
        }

        private string GetDateTimeStamp()
        {
            DateTime dDate = DateTime.Now;
            return (dDate.Month.ToString("00") + dDate.Day.ToString("00") + dDate.Year.ToString("0000") + dDate.Hour.ToString("00") + dDate.Minute.ToString("00") + dDate.Second.ToString("00"));
        }


        public bool CreateTemplateFrmDES(string templateFileName, List<string> DESFilenames, string tempFolderPath)
        {
            bool RetVal;

            //create temp template file
            DIDatabase TempTemplateFile = new DIDatabase(templateFileName);

            //import data from DES 
            this.Spreadsheets = new DataEntrySpreadsheets(DESFilenames, templateFileName, tempFolderPath, string.Empty);
            RetVal = this.Spreadsheets.StartImportProcess();

            if (RetVal)
            {
                //import only ius 
                DataTable TempDataTable = this.Spreadsheets.DBConnection.ExecuteDataTable("Select * from " + DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempDataTableName);
                foreach (DataRow Row in TempDataTable.Rows)
                {
                    TempTemplateFile.DIIUS.CheckNCreateIUS(this.GetIUSInfo(Row));
                }

                //delete temp tables
                this.Spreadsheets.DBConnection.ExecuteNonQuery("DROP Table " + DAImportCommon.Constants.TempDataTableName);
                this.Spreadsheets.DBConnection.ExecuteNonQuery("DROP Table " + DAImportCommon.Constants.TempUnmatchedIUSTable);
                this.Spreadsheets.DBConnection.ExecuteNonQuery("DROP Table " + DAImportCommon.Constants.TempBlankIUSTable);

                //dispose objects
                this.Spreadsheets.Dispose();
                TempTemplateFile.Dispose();

                RetVal = true;
            }

            return RetVal;
        }



        /// <summary>
        /// DevInfo_5_0 Area Spreadsheet (should have 5 columns only staring the value from 6th Row - AreaID, AreaName, AreaLevel, AreaGID, ParentGID)
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="xlsFilenames"></param>
        /// <param name="tempFolderPath"></param>
        /// <param name="trgDBQueries">Instance of target queries object</param>
        /// <param name="languageName">languageName</param>
        /// <returns></returns>
        public bool CreateTemplateFrmAreaSpreadsheet(string templateFileName, List<string> xlsFilenames, string tempFolderPath, DIQueries trgDBQueries, string languageName)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries;
            AreaBuilder AreaBuilderObj;
            DIDatabase TempTemplateFile;
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            LanguageBuilder DILanguageBuilder;
            try
            {
                //create temp template file
                TempTemplateFile = new DIDatabase(templateFileName);
                TempTemplateFile.Dispose();

                //create DIConnection, queries and area objects
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, templateFileName, string.Empty, string.Empty);

                if (trgDBQueries != null)
                {
                    DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), trgDBQueries.LanguageCode);
                    DataPrefix = DBQueries.DataPrefix;
                    LanguageCode = trgDBQueries.LanguageCode.Replace("_", "");

                    // create langauge dependent tables if not exists in the new template
                    this.UpdateTablesForTargetLanguage(languageName, DBConnection, DBQueries, DataPrefix, LanguageCode);
                }
                else
                {

                    DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));
                }

                AreaBuilderObj = new AreaBuilder(DBConnection, DBQueries);

                //get areas from excel files
                foreach (string XlsFileName in xlsFilenames)
                {
                    try
                    {
                        // insert area into template
                        DIExcel AreaXlsFile = new DIExcel(XlsFileName);
                        AreaInfo NewArea;
                        DataTable TempTable = AreaXlsFile.GetDataTableFromSheet(AreaXlsFile.GetSheetName(0));

                        //check it is a valid area excel file or not
                        if (TempTable.Rows.Count > 5 & TempTable.Columns.Count > 1)
                        {
                            if (TempTable.Rows[2][0].ToString() == DITemplate.AreaIdString
                                & TempTable.Rows[2][1].ToString() == DITemplate.AreaNameString
                                & TempTable.Rows[2][2].ToString() == DITemplate.AreaLevelString
                                & TempTable.Rows[2][3].ToString() == DITemplate.AreaGIdString
                                & TempTable.Rows[2][4].ToString() == DITemplate.AreaParentGIdString)
                            {

                                // delete empty rows
                                for (int i = 0; i < 4; i++)
                                {
                                    TempTable.Rows[0].Delete();
                                }

                                TempTable.AcceptChanges();

                                // sort the table by level
                                TempTable.DefaultView.Sort = TempTable.Columns[this.AreaSheetColumns[AreaSheetColumnType.AreaLevel].ColumnIndex].ColumnName;


                                foreach (DataRowView Row in TempTable.DefaultView)
                                {
                                    try
                                    {
                                        // get area info from temp table
                                        NewArea = new AreaInfo();
                                        NewArea.ID = DICommon.RemoveQuotes(Row[this.AreaSheetColumns[AreaSheetColumnType.AreaID].ColumnIndex].ToString());
                                        NewArea.Name = DICommon.RemoveQuotes(Row[this.AreaSheetColumns[AreaSheetColumnType.AreaName].ColumnIndex].ToString());
                                        NewArea.Level = Convert.ToInt32(Row[this.AreaSheetColumns[AreaSheetColumnType.AreaLevel].ColumnIndex]);
                                        NewArea.GID = DICommon.RemoveQuotes(Row[this.AreaSheetColumns[AreaSheetColumnType.AreaGID].ColumnIndex].ToString());
                                        NewArea.Parent = new AreaInfo();
                                        NewArea.Parent.ID = DICommon.RemoveQuotes(Row[this.AreaSheetColumns[AreaSheetColumnType.PareaGID].ColumnIndex].ToString());


                                        //insert area into template
                                        // Step 1: If Area_Parent_ID is blank then set Area_Parent_NID to -1 and Area_Level=1
                                        if (string.IsNullOrEmpty(NewArea.Parent.ID))
                                        {
                                            NewArea.Parent.Nid = -1;
                                            NewArea.Level = 1;
                                        }
                                        else
                                        {
                                            // Step 2: If Area_Parent_ID is not blank then get Area_Parent_NID
                                            NewArea.Parent.Nid = AreaBuilderObj.GetAreaNidByAreaID(NewArea.Parent.ID);

                                            if (NewArea.Parent.Nid <= 0)
                                            { // Step 2.1: If Area_Parent_NID <=0 then set Area_Parent_NID to -1 and Area_Level=1
                                                NewArea.Parent.Nid = -1;
                                                NewArea.Level = 1;
                                            }
                                            else
                                            { // Step 2.1: If Area_Parent_NID >0 then Area_Level=Area_Parent_Level+1

                                                //get parent area level
                                                NewArea.Parent.Level = AreaBuilderObj.GetAreaLevelByAreaID(NewArea.Parent.ID);
                                                NewArea.Level = NewArea.Parent.Level + 1;
                                            }
                                        }

                                        // insert area into template
                                        AreaBuilderObj.InsertIntoDatabase(NewArea.Name, NewArea.ID, NewArea.GID, NewArea.Level, NewArea.Parent.Nid);

                                    }
                                    catch (Exception ex)
                                    {
                                        // do nothing
                                    }


                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
                RetVal = true;

            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }
            }
            return RetVal;

        }


        /// <summary>
        /// DevInfo_5_0 Area Spreadsheet (should have 5 columns only staring the value from 6th Row - AreaID, AreaName, AreaLevel, AreaGID, ParentGID)
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="xlsFilenames"></param>
        /// <param name="tempFolderPath"></param>
        /// <returns></returns>
        public bool CreateTemplateFrmAreaSpreadsheet(string templateFileName, List<string> xlsFilenames, string tempFolderPath)
        {
            return this.CreateTemplateFrmAreaSpreadsheet(templateFileName, xlsFilenames, tempFolderPath, null, string.Empty);
        }




        /// <summary>
        /// DevInfo_5_0 Indicator Spreadsheet (should have 2 columns only staring the value from 6th Row - Indicator,Indicator_GId)
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="xlsFilenames"></param>
        /// <param name="tempFolderPath"></param>
        /// <returns></returns>
        public bool CreateTemplateFrmIndicatorSpreadsheet(string templateFileName, List<string> xlsFilenames, string tempFolderPath)
        {
            return this.CreateTemplateFrmIndicatorSpreadsheet(templateFileName, xlsFilenames, tempFolderPath, null, string.Empty);
        }



        /// <summary>
        /// DevInfo_5_0 Indicator Spreadsheet (should have 2 columns only staring the value from 6th Row - Indicator,Indicator_GId)
        /// </summary>
        /// <param name="templateFileName"></param>
        /// <param name="xlsFilenames"></param>
        /// <param name="tempFolderPath"></param>
        /// <returns></returns>
        public bool CreateTemplateFrmIndicatorSpreadsheet(string templateFileName, List<string> xlsFilenames, string tempFolderPath, DIQueries trgQueries, string languageName)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries;
            IndicatorBuilder IndicatorBuilderObj;
            DIDatabase TempTemplateFile;
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;

            try
            {
                //create temp template file
                TempTemplateFile = new DIDatabase(templateFileName);
                TempTemplateFile.Dispose();

                //create DIConnection, queries and indicator objects
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, templateFileName, string.Empty, string.Empty);

                if (trgQueries == null)
                {
                    DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));
                }
                else
                {
                    DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), trgQueries.LanguageCode);
                    DataPrefix = DBQueries.DataPrefix;
                    LanguageCode = trgQueries.LanguageCode.Replace("_", "");

                    this.UpdateTablesForTargetLanguage(languageName, DBConnection, DBQueries, DataPrefix, LanguageCode);
                }

                IndicatorBuilderObj = new IndicatorBuilder(DBConnection, DBQueries);

                //get indicators from excel files
                foreach (string XlsFileName in xlsFilenames)
                {
                    try
                    {
                        // insert indicators into template
                        DIExcel IndicatorXlsFile = new DIExcel(XlsFileName);
                        IndicatorInfo NewIndicator;
                        DataTable TempTable = IndicatorXlsFile.GetDataTableFromSheet(IndicatorXlsFile.GetSheetName(0));
                        //check it is a valid indicator excel file or not
                        if (TempTable.Rows.Count > 5 & TempTable.Columns.Count > 1)
                        {
                            if (TempTable.Rows[2][0].ToString() == DITemplate.IndicatorString)
                            {
                                //starting index should be 5
                                for (int i = 5; i < TempTable.Rows.Count; i++)
                                {
                                    NewIndicator = new IndicatorInfo();

                                    //indicator Name
                                    NewIndicator.Name = DICommon.RemoveQuotes(TempTable.Rows[i][0].ToString());
                                    //indicator GId
                                    NewIndicator.GID = DICommon.RemoveQuotes(TempTable.Rows[i][1].ToString());
                                    //insert indicator into template
                                    IndicatorBuilderObj.CheckNCreateIndicator(NewIndicator);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
                RetVal = true;

            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }
            }
            return RetVal;
        }


        #endregion

        #endregion


    }

    internal enum AreaSheetColumnType
    {
        AreaID,
        AreaName,
        AreaLevel,
        AreaGID,
        PareaGID
    }

    internal class AreaSheetColumnInfo
    {
        #region "-- Private --"

        #region "-- New/Dispose --"

        private AreaSheetColumnInfo()
        {
            // do nothing
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables --"

        private int _ColumnIndex;
        /// <summary>
        /// Gets or sets column index
        /// </summary>
        internal int ColumnIndex
        {
            get { return this._ColumnIndex; }
            set { this._ColumnIndex = value; }
        }


        private string _ColumnName;
        /// <summary>
        /// Gets or sets column name
        /// </summary>
        internal string ColumnName
        {
            get { return this._ColumnName; }
            set { this._ColumnName = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        internal AreaSheetColumnInfo(int columnIndex, string columnName)
        {
            this._ColumnIndex = columnIndex;
            this._ColumnName = columnName;
        }

        #endregion

        #endregion
    }

}
