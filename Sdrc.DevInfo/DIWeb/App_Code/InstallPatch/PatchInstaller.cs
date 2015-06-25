using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Xml;
using System.Collections;
using System.Xml.Linq;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data.SqlClient;
using System.Web.Hosting;
using System.Configuration;
using DevInfo.Lib.DI_LibSDMX;

/// <summary>
/// Methods for installing patch, updating language  xml, updating app setting file
/// </summary>
public class PatchInstaller
{
    #region --Internal--
    internal static string StatusSucess = ReadKeysForPatch("StatusPassed").ToString();
    internal static string StatusFailed = ReadKeysForPatch("StatusFail").ToString();
    #endregion

    #region -- Public --



    /// <summary>
    /// Install all patches from 7.0.0.3 to 7.0.0.9
    /// If installation fails at any stop further execution of code
    /// </summary>
    /// <returns>True if patches installed from 7.0.0.3 to 7.0.0.9 sucessfully, else return false</returns>
    public bool InstallPatch()
    {
        bool RetVal = false;
        string DbConnectionstring = string.Empty;
        int DbNid = -1;

        try
        {
            //// Get Database NID of Default Database
            //try
            //{

            //}
            //catch (Exception){}
            //if (DbNid > 0)
            //{                
            //    // Get database connection object by dbnid
            //    try
            //    {
            //        ;
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}
            //else
            //{
            //    RetVal = true;
            //}
            DbNid = Convert.ToInt32(Global.GetDefaultDbNId());
            DbConnectionstring = this.GetConnectionString(DbNid);
            //if (DbConnectionstring != null && DbConnectionstring.Trim() != string.Empty)
            //{
            // check if patch 7.0.0.3 installed successfully
            if (this.InstallPatch7_0_0_3())
            { // check if patch 7.0.0.4 installed successfully
                if (this.InstallPatch7_0_0_4())
                { // check if patch 7.0.0.5 installed successfully
                    if (this.InstallPatch7_0_0_5())
                    { // check if patch 7.0.0.6 installed successfully
                        if (this.InstallPatch7_0_0_6())
                        { // check if patch 7.0.0.7 installed successfully
                            if (this.InstallPatch7_0_0_7(DbConnectionstring))
                            { // check if patch 7.0.0.8 installed successfully
                                if (this.InstallPatch7_0_0_8())
                                { // check if patch 7.0.0.9 installed successfully
                                    if (this.InstallPatch7_0_0_9(DbConnectionstring))
                                    {
                                        if (this.InstallPatch7_0_0_10(DbConnectionstring))
                                        {
                                            RetVal = true;
                                            return RetVal;
                                        }
                                        else
                                        {
                                            RetVal = false;
                                            return RetVal;
                                        }
                                    }
                                    else
                                    {
                                        RetVal = false;
                                        return RetVal;
                                    }
                                }
                                else
                                {
                                    RetVal = false;
                                    return RetVal;
                                }
                            }
                            else
                            {
                                RetVal = false;
                                return RetVal;
                            }
                        }
                        else
                        {
                            RetVal = false;
                            return RetVal;
                        }
                    }
                    else
                    {
                        RetVal = false;
                        return RetVal;
                    }
                }
                else
                {
                    RetVal = false;
                    return RetVal;
                }
            }
            else
            {
                RetVal = false;
                return RetVal;
            }
            //}
            //else
            //{
            //    RetVal = true;
            //}
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Get all the new keys
    /// Add new keys to master language files
    /// Genarate page xml for all languages
    /// </summary>
    /// <returns>True if key aadded and language files updated successfully, else return false</returns>
    public bool UpdateLanguageFiles()
    {
        bool RetVal = false;
        Callback Callback = null;
        bool IsGeneratelog = false;

        // Check if keys have been added to language file
        if (this.AddKeysToLangFile())
        {
            Callback = new Callback();
            // IsGeneratelog is a optional parameter, for checking whether to make an entry in patch log file
            // by default its value is false
            // While generating log file, at the patch installation step need to make entry in patch log file
            // so setvalue of IsGeneratelog to true,

            IsGeneratelog = true;
            // call method to optimize language file
            if (Callback.GenerateAllPagesXML(IsGeneratelog) == "true")
            {// language files generated successfully return true
                RetVal = true;
            }
            else
            {// If language files generation failed return true
                RetVal = false;
            }
        }// else set return value to false
        else
        {
            RetVal = false;
        }
        return RetVal;
    }
    /// <summary>
    /// get all the new keys
    /// Add new keys to master app setting file with default values
    /// </summary>
    /// <returns>return true if keys added sussessfully, elase return false</returns>
    public bool UpdateAppSettingFile()
    {
        bool RetVal = false;
        if (this.AddKeysToAppSettingFile())
        {
            RetVal = true;
        }
        else
        {
            RetVal = false;
        }
        return RetVal;
    }

    #endregion

    #region -- Private --

    #region -- InstallPatches --



    #region -- Patch 7.0.0.3

    private bool InstallPatch7_0_0_3()
    {
        bool RetVal = true;
        return RetVal;
    }

    #endregion

    private bool InstallPatch7_0_0_4()
    {
        bool RetVal = true;
        return RetVal;
    }

    private bool InstallPatch7_0_0_5()
    {
        bool RetVal = true;
        return RetVal;
    }

    /// <summary>
    /// Changes for language preference for the notifications while subscription
    /// </summary>
    /// <returns> True if changes done susscss fuly else retyrn false</returns>
    private bool InstallPatch7_0_0_6()
    {
        bool RetVal = false;

        string Query = string.Empty;
        string DefaultLanguage = string.Empty;
        string LogMessage = string.Empty;
        string PatchMessagesFile = string.Empty;
        DIConnection diConnection;
        DataTable DTLanguages = null;
        try
        {
            DataTable DTLangauge = Global.GetAllDBLangaugeCodes();
            DataRow[] DefaultLangaugeCodeRow = DTLangauge.Select("Language_Default = '" + true + "'");
            int DefaultLanguageNId = 0;
            if (DefaultLangaugeCodeRow.Length > 0)
            {
                DefaultLanguageNId = Convert.ToInt32(DefaultLangaugeCodeRow[0]["Language_NId"].ToString());
            }

            using (diConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
            {
                if (Global.BaselineAccessDbSchema())
                {
                    Query = @" SELECT * FROM Artefacts";
                    DTLanguages = diConnection.ExecuteDataTable(Query);
                    foreach (DataRow dr in DTLanguages.Rows)
                    {
                        if (dr["LangPrefNid"].ToString().Trim() == string.Empty)
                        {
                            Query = @" UPDATE Artefacts Set LangPrefNid =" + DefaultLanguageNId;
                            diConnection.ExecuteNonQuery(Query);
                        }
                    }
                    RetVal = true;
                    // Add entry in xls log file for patch, for Success
                    LogMessage = ReadKeysForPatch("ChangeLangPref").ToString();
                    XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, string.Empty);
                }
            }
        }
        catch (Exception Ex)
        {
            // Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("ChangeLangPref").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString());

            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;

    }


    /// <summary>
    /// Make all the necessary changes in database, that were done in patch 7.0.0.7
    /// </summary>
    /// <param name="DbConnectionstring">database connection string</param>
    /// <returns>Returns true, if patch installed successfully, else return false</returns>
    private bool InstallPatch7_0_0_7(string DbConnectionstring)
    {
        bool RetVal = false;
        SqlConnection DbCon = null;
        SqlCommand SqlCmd = null;
        string Query = string.Empty;
        SqlTransaction tran = null;
        string LogMessage = string.Empty;
        string PatchMessagesFile = string.Empty;
        try
        {
            DbCon = new SqlConnection(DbConnectionstring);
            SqlCmd = new SqlCommand();
            DbCon.Open();// Open database connection
            tran = DbCon.BeginTransaction();// Begin Transaction
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        try
        {
            Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GET_SITEMAP_DATA]') AND type in (N'P', N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N' CREATE PROC [dbo].[SP_GET_SITEMAP_DATA] AS BEGIN SELECT * FROM DBO.DI_SEARCH_RESULTS END ' END ELSE BEGIN EXEC dbo.sp_executesql @statement = N' ALTER PROC [dbo].[SP_GET_SITEMAP_DATA] AS BEGIN SELECT * FROM DBO.DI_SEARCH_RESULTS END ' END";
            SqlCmd.CommandText = Query;
            SqlCmd.Connection = DbCon;
            SqlCmd.Transaction = tran;
            SqlCmd.ExecuteNonQuery();  // Execute query to create procedure             
            // If stored procudeure for generating sitemap data updated\created, successfully commit transaction
            tran.Commit();
            RetVal = true;
            // Add entry in xls log file for patch, for Success
            LogMessage = ReadKeysForPatch("SiteMapGen").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, string.Empty);
        }
        catch (Exception Ex)
        {
            // Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("SiteMapGen").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString());
            // if any error occured duringtransaction
            tran.Rollback();
            LogMessage = String.Format(ReadKeysForPatch("RollBack").ToString(), Ex.Message.ToString(), "7.0.0.7");
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString());
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {// Dispose connection  Object
            DbCon.Close();
        }
        return RetVal;
    }

    /// <summary>
    /// Make all the necessary changes in database, that were done in patch 7.0.0.8
    /// </summary>
    /// <returns>Returns true, if patch installed successfully, else return false</returns>
    private bool InstallPatch7_0_0_8()
    {
        bool RetVal = false;
        bool IsnewsMenuEnabled = false;
        bool IsCMSDatabaseCreated = false;
        string OutCmsDatabaseName = string.Empty;
        string LogMessage = string.Empty;
        try
        {
            // Get news menu enable value
            IsnewsMenuEnabled = Convert.ToBoolean(Global.isNewsMenuVisible.ToString());

            // If user enables news menu then call method to geneate database for cms
            if (IsnewsMenuEnabled)
            {
                // Create cms database, if database is not already existing,
                // Create datatables if tables are not already existing,
                // Create Procedures, if procedures are not already existing,
                // Alter procedure, if procedure are already exists
                IsCMSDatabaseCreated = Callback.CheckNCreateCMSDatabase(out OutCmsDatabaseName);
                // IF CMS database created successfull, call method to add entry in Patch log file
                if (IsCMSDatabaseCreated)
                {
                    // If name of creted CMS database is not null or empty
                    if (!string.IsNullOrEmpty(OutCmsDatabaseName))
                    {// Add entry in xls log file for patch, for success
                        LogMessage = string.Format(ReadKeysForPatch("CMSDBCreationPassed").ToString(), OutCmsDatabaseName);
                        XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, string.Empty);
                    }
                    RetVal = true;
                }
                else // if databse is not created
                {// Add entry in xls log file for patch, for faliure
                    RetVal = false;
                    LogMessage = ReadKeysForPatch("CMSDBCreationFailed").ToString();
                    XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, string.Empty);
                }
            }
            else
            {
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {// Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("CSVFileGen").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, Ex.Message.ToString());
            // RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        // retun true in any case either cms database created or not
        return true;
    }

    /// <summary>
    /// Make all the necessary changes in database, that were done in patch 7.0.0.9
    /// </summary>
    /// <param name="DbConnectionstring">Database connection string</param>  
    /// <returns>Returns true, if patch installed successfully, else return false</returns>
    private bool InstallPatch7_0_0_9(string DbConnectionstring)
    {
        bool RetVal = false;
        string LngCode = string.Empty;
        DataTable DatabaseLanguages = null;
        string LogMessage = string.Empty;
        SqlConnection DbCon = null;
        SqlCommand SqlCmd = null;
        string Query = string.Empty;
        SqlTransaction tran = null;
        try
        {
            DbCon = new SqlConnection(DbConnectionstring);
            SqlCmd = new SqlCommand();
            DbCon.Open();// Open database connection
            tran = DbCon.BeginTransaction();// Begin Transaction
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        try
        {
            // Get all languages from database
            DatabaseLanguages = Global.GetAllDBLangaugeCodes();
            // itterrate loop for database languages
            // get languaage code
            // create procedure for each language present in database
            // by replacing "_XX" with"_"+ lang code, to create procedure for all available languages
            foreach (DataRow langRow in DatabaseLanguages.Rows)
            {
                Query = "IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_SelectMultipleData_XX]') AND type in (N'P', N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[sp_SelectMultipleData_XX] AS BEGIN select Indicator_Name as Indicator,Unit_Name as Unit, Subgroup_Val as Subgroup,Area_Name as Area,ut_data.area_nid as ''Area ID'', TimePeriod as ''Time Period'',IC_Name as Source, Data_Value as ''Data Value'',FootNote as Footnotes from ut_indicator_XX,ut_unit_XX,ut_subgroup_vals_XX,ut_area_XX, ut_timeperiod,ut_indicator_classifications_XX,ut_data,ut_footnote_XX where ut_data.indicator_nid=ut_indicator_XX.indicator_nid and ut_data.Unit_Nid=ut_unit_XX.Unit_Nid and ut_data.subgroup_val_nid=ut_subgroup_vals_XX.subgroup_val_nid and ut_data.area_nid=ut_area_XX.area_nid and ut_data.timeperiod_nid=ut_timeperiod.timeperiod_nid and ut_data.footnote_nid=ut_footnote_XX.footnote_nid and ut_data.source_nid=ut_indicator_classifications_XX.ic_nid and ut_indicator_classifications_XX.ic_type=''sr'' END ' END ELSE BEGIN EXEC dbo.sp_executesql @statement = N'ALTER PROCEDURE [dbo].[sp_SelectMultipleData_XX] AS BEGIN select Indicator_Name as Indicator,Unit_Name as Unit, Subgroup_Val as Subgroup,Area_Name as Area,ut_data.area_nid as ''Area ID'', TimePeriod as ''Time Period'',IC_Name as Source, Data_Value as ''Data Value'',FootNote as Footnotes from ut_indicator_XX,ut_unit_XX,ut_subgroup_vals_XX,ut_area_XX, ut_timeperiod,ut_indicator_classifications_XX,ut_data,ut_footnote_XX where ut_data.indicator_nid=ut_indicator_XX.indicator_nid and ut_data.Unit_Nid=ut_unit_XX.Unit_Nid and ut_data.subgroup_val_nid=ut_subgroup_vals_XX.subgroup_val_nid and ut_data.area_nid=ut_area_XX.area_nid and ut_data.timeperiod_nid=ut_timeperiod.timeperiod_nid and ut_data.footnote_nid=ut_footnote_XX.footnote_nid and ut_data.source_nid=ut_indicator_classifications_XX.ic_nid and ut_indicator_classifications_XX.ic_type=''sr'' END ' END ";
                Query = Query.Replace("_XX", "_" + langRow["Language_Code"]);

                SqlCmd.CommandText = Query;
                SqlCmd.Connection = DbCon;
                SqlCmd.Transaction = tran;
                SqlCmd.ExecuteNonQuery();  // Execute query to create procedure             
            }
            // If all the stored procudeure for creating csv file, created success fully commit transaction
            tran.Commit();
            RetVal = true;
            // Add entry in xls log file for patch, for success
            LogMessage = ReadKeysForPatch("CSVFileGen").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusSucess, string.Empty);
        }
        catch (Exception Ex)
        {
            // Add entry in xls log file for patch, for faliure
            LogMessage = ReadKeysForPatch("CSVFileGen").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, string.Empty);
            RetVal = false;

            tran.Rollback();
            LogMessage = String.Format(ReadKeysForPatch("RollBack").ToString(), Ex.Message.ToString(), "7.0.0.9");
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, Ex.Message.ToString());
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {// Dispose connection  Object
            DbCon.Close();
        }

        return RetVal;
    }

    private bool InstallPatch7_0_0_10(string DbConnectionstring)
    {
        bool RetVal = false;
        //Step 1.	Iterate all dbnids in db.xml
        //Step 2.	Check existence of Header.xml in each \dbnid\sdmx
        //Step 3.	If Header.xml does not exists then create Header.xml using “GenerateHeader()” function (use appropriate function for 2.0 (dbnid > 1)and 2.1 (dbnid=1))
        //Step 4.	Generate “DataPublishedUserSelection.xml” based on existing mapping using function “()” for each dbnid
        //Step 5.	GenerateandRegister SDMX-ML data file using function “()”
        //Step 6.	Set   <item n="IsSDMXHeaderCreated" v="true" /> in appsettings.xml
        try
        {
            Dictionary<string, string> DictConnections = new Dictionary<string, string>();
            Dictionary<string, string> DBConnections = Global.GetAllConnections("DevInfo");
            string DBConnectionsFile = HttpContext.Current.Server.MapPath("..\\..\\" + ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);
            XmlNode xmlNode;
            XmlDocument UploadedDSDXml = new XmlDocument();
            string UploadedDSDFileWPath = string.Empty;
            bool delCondition = false;
            SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
            Callback objCallback = new Callback();


            foreach (var item in DBConnections.Keys)
            {
                // Remove all the artifacts of type=10 regardless of its DBNid . IF two columns 'publishedfilename and LangPrefNid' exists

                using (var diConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
                {
                    var dataTable = diConnection.ExecuteDataTable(@"SELECT * FROM Artefacts");
                    delCondition = dataTable.Columns.Contains("PublishedFileName");
                    if (!delCondition)
                    {
                        diConnection.ExecuteNonQuery(string.Format(@"DELETE FROM Artefacts where DBNId = {0} and Type={1}", item, 10));
                        foreach (var folder in System.IO.Directory.GetDirectories(HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item + "\\" + Constants.FolderName.SDMX.Registrations)))
                        {
                            foreach (var file in System.IO.Directory.GetFiles(folder))
                                System.IO.File.Delete(file);
                        }
                    }
                }

                //check folder
                if (Directory.Exists(HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item)))
                {
                    //check file
                    if (!File.Exists(HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item + "\\sdmx\\" + Constants.FileName.HeaderFileName)))
                    {
                        //create file 
                        xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + item + "]");
                        if (Convert.ToBoolean(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value) == true)
                        {
                            UploadedDSDFileWPath = HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item) + "\\sdmx\\DSD.xml";

                            UploadedDSDXml.Load(UploadedDSDFileWPath);
                            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedDSDXml);
                            objCallback.CreateHeaderFileForUploadedDSD(UploadedDSDStructure, HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item) + "\\" + "sdmx", Convert.ToInt32(item), string.Empty).ToString();
                            Global.SetDBXmlAttributes(item, Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated, "true");
                        }
                        else
                        {
                            SDMXUtility.Generate_Header(string.Empty, string.Empty, null, HttpContext.Current.Server.MapPath("..\\..\\stock\\data\\" + item + "\\sdmx"));
                            Global.SetDBXmlAttributes(item, Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated, "true");
                        }


                    }
                }


            }

            RetVal = true;
        }
        catch (Exception) { }
        return RetVal;
    }

    #endregion

    #region -- Update Language N App SettingFiles --

    /// <summary>
    /// Get New Keys to be added in each language master  file
    /// Add Keys to language file
    /// Generate language XML 
    /// </summary>
    /// <returns>True if key added successfully, else return false</returns>
    private bool AddKeysToLangFile()
    {
        bool RetVal = false;
        string MasterKeyValsDir = string.Empty;
        string SourceLangFileToMatchKeys = string.Empty;
        string LangFileNameForLog = string.Empty;
        string LogMessage = string.Empty;
        try
        {

            // Get Directory of language master files
            MasterKeyValsDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            // Get Path of source file containing new keys to add in master files
            SourceLangFileToMatchKeys = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.PatchConstaints.PatchLanguageXML);

            // For adding keys in each master file get all file from Directory of language master files, with extension "xml"
            foreach (string MasterKeyFileName in Directory.GetFiles(MasterKeyValsDir, "*.xml"))
            {
                LangFileNameForLog = Path.GetFileName(MasterKeyFileName);
                // call method to add keys in language files
                // if all keys added successfully retuen true, else return false
                if (AddKeysInLangNAppSettingFile(SourceLangFileToMatchKeys, MasterKeyFileName, FileType.Language))
                {
                    RetVal = true;
                    // Add entry in xls log file for patch, for success
                    LogMessage = string.Format(ReadKeysForPatch("GenPagXMlPassed").ToString(), LangFileNameForLog);
                    XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, ReadKeysForPatch("StatusPassed").ToString(), string.Empty);
                }
                else// else if error occured during adding key add entry to log file,
                {
                    RetVal = false;
                    // Add entry in xls log file for patch, for failure
                    LogMessage = string.Format(ReadKeysForPatch("GenPagXMlPassed").ToString(), LangFileNameForLog);
                    XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, ReadKeysForPatch("StatusFail").ToString(), string.Empty);
                }


            }
            // Add entry in xls log file for patch, for Success      
            LogMessage = string.Format(ReadKeysForPatch("AddKeyInLangXMlPassed").ToString(), LangFileNameForLog);
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, ReadKeysForPatch("StatusPassed").ToString(), string.Empty);
        }
        catch (Exception ex)
        {
            RetVal = false;
            // Add entry in xls log file for patch, for failure
            LogMessage = string.Format(ReadKeysForPatch("AddKeyInLangXMlFail").ToString(), LangFileNameForLog);
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, ReadKeysForPatch("StatusFail").ToString(), ex.Message.ToString());
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }


    /// <summary>
    /// Get Keys that are not present in existing AppSetting file,
    /// Add keys to app setting file
    /// </summary>
    /// <returns>True if key added successfully, else return false</returns>
    private bool AddKeysToAppSettingFile()
    {
        bool RetVal = false;
        string AppSettingFile = string.Empty;
        string PatchAppsettingFile = string.Empty;
        try
        {
            // Get full path of App Setting File
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, System.Configuration.ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            // Get Path of source app setting file, containing new keys to add in existing app setting file
            PatchAppsettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.PatchConstaints.PatchAppSetting);

            // call method to add keys in language files
            // if all keys added successfully retuen true, else return false
            if (AddKeysInLangNAppSettingFile(PatchAppsettingFile, AppSettingFile, FileType.AppSetting))
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Gets All Unmatched key value pait and add to Laguage xml files present inside master key val folder
    /// </summary>
    /// <param name="sourceXMLFile">File From where to get keys</param>
    /// <param name="destXMLFile">File to add Keys</param>
    /// <returns></returns>
    private bool AddKeysInLangNAppSettingFile(string sourceXMLFile, string destXMLFile, FileType fileType)
    {
        bool Retval = false;
        List<KeyValXml> KeysToAdd = new List<KeyValXml>();

        // Get all the keys which are not present in existing language file
        KeysToAdd = this.GetUnMatchedKeys(sourceXMLFile, destXMLFile, fileType);

        // Checke if list is containing atleast 1 key
        if (KeysToAdd.Count > 0)
        {
            // call method to add keys to the language file
            if (this.AddKeysToXMLFile(KeysToAdd, destXMLFile, fileType))
            {
                Retval = true;
            }
            else
            {
                Retval = false;
            }
        }
        else
        {
            Retval = true;
        }

        return Retval;
    }

    /// <summary>
    /// Gets List of all keys which are not present in existing language filek
    /// </summary>
    /// <param name="sourceXMLFilePath">Path of source xml file,conatining new kaye</param>
    /// <param name="destXMLFilePath">path of destination xml file, existing xml file</param>
    /// <returns>All new keys that are present in new xml file, and not in existing xml file</returns>
    private List<KeyValXml> GetUnMatchedKeys(string sourceXMLFilePath, string destXMLFilePath, FileType fileType)
    {
        List<KeyValXml> listOfNewKeys = null; new List<KeyValXml>();
        List<KeyValXml> listOfSourceKeys = null;
        List<KeyValXml> listOfDestKeys = null;


        // initlize list objects
        listOfSourceKeys = new List<KeyValXml>();
        listOfDestKeys = new List<KeyValXml>();
        listOfNewKeys = new List<KeyValXml>();

        //call method to read keys of source xml file
        listOfSourceKeys = ReadKeysFromXMLFile(sourceXMLFilePath, fileType);

        // call method to read keys of destination xml file
        listOfDestKeys = ReadKeysFromXMLFile(destXMLFilePath, fileType);

        // Get all unmatched keys that are not present in destination xml file 
        listOfNewKeys = (from SourceKeyVal in listOfSourceKeys
                         where !(listOfDestKeys.Any(DestKeyVal => DestKeyVal.Key.ToLower().Trim() == SourceKeyVal.Key.ToLower().Trim()))
                         select new KeyValXml
                         {
                             Key = SourceKeyVal.Key,
                             Value = SourceKeyVal.Value,
                         }).ToList<KeyValXml>();
        // return all new keys of source file
        return listOfNewKeys;
    }

    /// <summary>
    /// Read Keys value pair from  file
    /// </summary>
    /// <param name="XMLFilePath"></param>
    /// <returns>list of keys and values of input xml file</returns>
    private List<KeyValXml> ReadKeysFromXMLFile(string XMLFilePath, FileType fileType)
    {
        List<KeyValXml> RetVal = null;
        string RootName = string.Empty;
        string ChildNode = string.Empty;
        XDocument SourceDoc = null;

        string KeyName = string.Empty;
        string ValueName = string.Empty;
        // load xml file
        SourceDoc = XDocument.Load(XMLFilePath);

        // check file type, if file is app settings file,
        // initlize name of root node name and child name for app setting file
        if (fileType == FileType.AppSetting)
        {
            RootName = Constants.XmlFile.AppSettings.Tags.Root.ToString();
            ChildNode = Constants.XmlFile.AppSettings.Tags.Item.ToString();
            KeyName = Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name;
            ValueName = Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value;
        }
        // check file type, if file is language file
        // initlize name of root node name and child name for language file
        else if (fileType == FileType.Language)
        {
            RootName = Constants.XmlFile.MasterLanguage.Tags.Root.ToString();
            ChildNode = Constants.XmlFile.MasterLanguage.Tags.Row.ToString();
            KeyName = Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Key;
            ValueName = Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Value;
        }
        // initlize retval with a empty collection
        RetVal = new List<KeyValXml>();
        // read keys of xml file
        RetVal = (from Lang in SourceDoc.Elements(RootName).Elements(ChildNode)
                  select new KeyValXml
                  {

                      Key = Lang.FirstAttribute.Name.ToString() == KeyName ? Lang.FirstAttribute.Value : Lang.LastAttribute.Value,
                      Value = Lang.LastAttribute.Name.ToString() == ValueName ? Lang.LastAttribute.Value : Lang.FirstAttribute.Value,
                  }
                   ).ToList<KeyValXml>();
        return RetVal;
    }

    /// <summary>
    /// Add Keys to the destination xml file
    /// </summary>
    /// <param name="listOfKeys">list of keys to be add in xml file</param>
    /// <param name="destXMLFilePath">Path of destination file in which keys are to be added</param>
    /// <returns>True if keys added successfully, else return false</returns>
    private bool AddKeysToXMLFile(List<KeyValXml> listOfKeys, string destXMLFilePath, FileType fileType)
    {
        bool Retval = false;
        XmlDocument DocLanguageXMLFile;
        XmlElement ChildNode;
        string NodeName = string.Empty;
        string KeyName = string.Empty;
        string ValueName = string.Empty;
        try
        {
            // check file type, if file is app settings file
            // initlize name of node, key and value for app setting file
            if (fileType == FileType.AppSetting)
            {
                NodeName = Constants.XmlFile.AppSettings.Tags.Item.ToString();
                KeyName = Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name.ToString();
                ValueName = Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value.ToString();
            }
            // check file type, if file is Language file
            // initlize name of node, key and value for Language file
            else if (fileType == FileType.Language)
            {
                NodeName = Constants.XmlFile.MasterLanguage.Tags.Row.ToString();
                KeyName = Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Key.ToString();
                ValueName = Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Value.ToString();

            }
            //Initlixe xml document
            DocLanguageXMLFile = new XmlDocument();
            // Load XML Document
            DocLanguageXMLFile.Load(destXMLFilePath);
            // add all new keys to xml document
            foreach (KeyValXml KeyVAlPair in listOfKeys)
            {
                //create child node
                ChildNode = DocLanguageXMLFile.CreateElement(NodeName);
                //Set attribute key and value
                ChildNode.SetAttribute(KeyName, KeyVAlPair.Key);
                ChildNode.SetAttribute(ValueName, KeyVAlPair.Value);
                DocLanguageXMLFile.DocumentElement.AppendChild(ChildNode);
            }
            // save xml document
            DocLanguageXMLFile.Save(destXMLFilePath);
            Retval = true;
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            throw;
        }

        return Retval;
    }



    /// <summary>
    /// Class for language xml file
    /// </summary>
    private class KeyValXml
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// enum For file type
    /// </summary>
    enum FileType
    {
        Language,
        AppSetting
    };

    #endregion

    #region --Common Methods--
    /// <summary>
    /// Get database connection string, by database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    private string GetConnectionString(int dbNid)
    {
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string ServerName = string.Empty;
        string DbName = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;

        string DBConn = string.Empty;
        string[] DBConnArr;
        string RetConnectionstring = string.Empty;

        try
        {
            DBConnectionsFile = Path.Combine(HostingEnvironment.MapPath("~"), System.Configuration.ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DBConn = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value;

            DBConnArr = Global.SplitString(DBConn, "||");

            //  ServerType = DBConnArr[0];
            ServerName = DBConnArr[1];
            DbName = DBConnArr[2];
            UserName = DBConnArr[3];

            if (DBConnArr.Length > 4)
            {
                Password = Global.DecryptString(DBConnArr[4]);
            }
            RetConnectionstring = "Data Source=" + ServerName + "; Initial Catalog=" + DbName + "; User Id=" + UserName + "; Password=" + Password + ";";
        }
        catch (Exception ex)
        {
            string LogMessage = string.Format(ReadKeysForPatch("ReadConnectionString").ToString(), ex.Message.ToString());
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, StatusFailed, ex.Message.ToString());
            Global.CreateExceptionString(ex, null);
        }

        return RetConnectionstring;
    }
    /// <summary>
    /// Get value of keys by key name
    /// </summary>
    /// <param name="keyName">Name of key, for which value is to retrieve</param>
    /// <returns></returns>
    public static string ReadKeysForPatch(string keyName)
    {
        string RetVal = string.Empty;
        string LanguageFileWithPath = string.Empty;
        string PatchMessagesFile = string.Empty;

        XmlDocument XmlDoc;

        try
        {
            // Path of file containing messages for patch installation
            PatchMessagesFile = Constants.PatchConstaints.PatchInstalMessages;
            LanguageFileWithPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, PatchMessagesFile);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LanguageFileWithPath);

            RetVal = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.MasterLanguage.Tags.Root + "/" + Constants.XmlFile.MasterLanguage.Tags.Row + "[@" + Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Key + "='" + keyName + "']").Attributes[Constants.XmlFile.MasterLanguage.Tags.RowAttributes.Value].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }
    #endregion

    #endregion
}