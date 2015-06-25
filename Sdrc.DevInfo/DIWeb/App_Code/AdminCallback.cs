using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using DI7_XMLGenerator_LibBAL.Classes;
using DI7_XMLGenerator_LibBAL.BAL;
using DI7.Lib.BAL.HTMLGenerator;
using DevInfo.Lib.DI_LibSDMX;
using DevInfo.Lib.DI_LibDAL.Queries;
using iTextSharp.text;
using SDMXObjectModel.Message;
using SDMXObjectModel;
using System.Data.Common;
using DevInfo.Lib.DI_LibMap;
using System.Threading;
using SDMXApi_2_0;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using SpreadsheetGear;
using SDMXObjectModel.Structure;
using System.Text;
using System.Collections;
using DevInfo.Lib.DI_LibBAL.Converter.Database;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using System.Data.OleDb;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using HtmlAgilityPack;
/// <summary>
/// Summary description for AdminCallback
/// </summary>
public partial class Callback : System.Web.UI.Page
{
    #region "--Private--"

    private string DataBaseDescription = string.Empty;

    #region "--Methods--"

    #region "--Application Settings--"

    /// <summary>
    /// Save value of a key in appsettings.xml file
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="keyName"></param>
    /// <param name="value"></param>
    public void SaveAppSettingValue(XmlDocument xmlDoc, string keyName, string value)
    {
        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;
        string NodeOldValue = string.Empty;
        try
        {
            NodeOldValue = xmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + keyName + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value;
            if (NodeOldValue != value)
            {
                string UserEmailID = string.Empty;

                XLSFileMsg = string.Format(Constants.CSVLogMessage.AppSettingSelectiveUpdate, keyName, NodeOldValue, value);
                WriteLogInXLSFile(Constants.AdminModules.AppSettings.ToString(), XLSFileMsg);
            }
            xmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + keyName + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value = value;

        }
        catch (Exception ex)
        {
            Global.WriteErrorsInLogFolder("key" + keyName + " Not found");
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #region "--Database Settings--"

    /// <summary>
    /// Method to test the connection 
    /// </summary>
    /// <param name="serverType">Server Type</param>
    /// <param name="serverName">Server Name</param>
    /// <param name="portNo">Port No</param>
    /// <param name="databaseName">Database Name</param>
    /// <param name="userName">UserName</param>
    /// <param name="password">Password</param> 
    /// <returns>True if Success / False if Failed </returns>
    private bool TestConnection(DIServerType serverType, string serverName, string portNo, string databaseName, string userName, string password)
    {
        bool RetVal;
        try
        {
            // -- Set the DiConnection object
            DevInfo.Lib.DI_LibDAL.Connection.DIConnection diConnection = new DIConnection(serverType, serverName, portNo, databaseName, userName, password);
            RetVal = true;
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    ///// <summary>
    ///// Get all available language code from database
    ///// </summary>
    ///// <param name="diConnection"></param>
    ///// <returns></returns>
    //private string[] GetAllAvailableLanguageCode(DIConnection diConnection)
    //{
    //    string[] RetVal;
    //    DataTable DTAvailableLanguages = null;
    //    List<string> AvlLanguages = new List<string>();

    //    try
    //    {
    //        DTAvailableLanguages = diConnection.DILanguages(diConnection.DIDataSetDefault());

    //        foreach (DataRow Row in DTAvailableLanguages.Rows)
    //        {
    //            AvlLanguages.Add(Row["Language_Code"].ToString());
    //        }
    //    }
    //    catch (Exception)
    //    {
    //    }

    //    RetVal = AvlLanguages.ToArray();

    //    return RetVal;
    //}

    /// <summary>
    /// Run dbscripts in database from script files
    /// </summary>
    /// <param name="ObjDIConnection"></param>
    /// <returns></returns>
    private Boolean RunDBScripts(DIConnection ObjDIConnection, string DBNid, string DbDefaultLanguage)
    {
        Boolean RetVal = false;
        FileInfo ScriptFile;
        string DbScripts = string.Empty;
        string LngFile = string.Empty;
        string LngCode = string.Empty;
        string LngDbScripts = string.Empty;
        int DefaultDBNid = 0;
        DataTable DatabaseLanguages = null;
        try
        {
            //Create and Execute table definition scripts
            ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.TableDefinitionsFile]));
            DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("GO", "");
            ObjDIConnection.ExecuteNonQuery(DbScripts);
            ScriptFile.OpenText().Close();


            //Create and Execute db language scrpts
            ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DbScriptsCreationFile]));
            DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("GO", "");

            if (DBNid.Trim() != "")
            {
                DefaultDBNid = Convert.ToInt32(DBNid);
            }
            else
            {
                DefaultDBNid = Convert.ToInt32(Global.GetDefaultDbNId());
            }
            DatabaseLanguages = Global.GetAllDBLangaugeCodesByDbNid(DefaultDBNid, ObjDIConnection);
            foreach (DataRow langRow in DatabaseLanguages.Rows)
            {
                LngDbScripts = DbScripts;
                LngDbScripts = LngDbScripts.Replace("_XX", "_" + langRow["Language_Code"]);
                foreach (string LngDbScriptBlock in Global.SplitString(LngDbScripts, "--SPSEPARATOR--"))
                {
                    try
                    {
                        ObjDIConnection.ExecuteNonQuery(LngDbScriptBlock);
                    }
                    catch (Exception ex)
                    {
                        Global.CreateExceptionString(ex, null);
                    }
                }
            }
            ScriptFile.OpenText().Close();

            ////Create and Execute db language scrpts
            //DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("GO", "");
            //foreach (string lang in getAllDbLangCodes(Convert.ToInt32(Global.GetDefaultDbNId())))
            //{
            //    LngDbScripts = DbScripts;
            //    LngDbScripts = LngDbScripts.Replace("_XX", "_" + lang);
            //    foreach (string LngDbScriptBlock in Global.SplitString(LngDbScripts, "--SPSEPARATOR--"))
            //    {
            //        try
            //        {
            //            ObjDIConnection.ExecuteNonQuery(LngDbScriptBlock);
            //        }
            //        catch (Exception ex)
            //        {
            //            Global.CreateExceptionString(ex, null);
            //        }
            //    }
            //}
            //ScriptFile.OpenText().Close();

            //Create and Execute table indexs scripts
            ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DbTableIndexes]));
            DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("GO", "");
            ObjDIConnection.ExecuteNonQuery(DbScripts);
            ScriptFile.OpenText().Close();

            RetVal = true;

            // Execute method to synchronize language files from ut_language
            this.SynchronizeLanguage(ObjDIConnection, DefaultDBNid, DbDefaultLanguage);
            this.ReverseSynchronizeLanguage(ObjDIConnection, DefaultDBNid);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }
    /// <summary>
    /// Synchronize Language.xml with ut_language table in the database
    /// </summary>
    /// <param name="ObjDIConnection"></param>
    /// <returns></returns>
    private void SynchronizeLanguage(DIConnection DIConnection, int DBNId, string DbDefaultLanguage)
    {

        //int DBNId = Convert.ToInt32(Global.GetDefaultDbNId());
        string TargetLngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        XmlElement NewNode;
        string SrcSlidersDirPath = string.Empty;
        string NewSlidersDirPath = string.Empty;
        string SrcEmailDirPath = string.Empty;
        string NewEmailDirPath = string.Empty;
        string LanguageName = string.Empty;
        string LanguageCode = string.Empty;
        string LanguageDirection = string.Empty;
        string LngNCodeName = string.Empty;
        // string EnglishLanguageCode = "en";
        string defaultLanguageCode = string.Empty;
        string defaultLanguageCodeInDB = string.Empty;
        string DefaultLangAtributeName = string.Empty;

        try
        {
            // Execute stored procedure to get all language codes in ut_language table
            defaultLanguageCode = DbDefaultLanguage;
            DataTable AllCodes = DIConnection.ExecuteDataTable("SP_GET_ALL_LANGUAGE_CODES_NAMES", CommandType.StoredProcedure, null);
            foreach (DataRow dr in AllCodes.Rows)
            {
                TargetLngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
                XmlDoc = new XmlDocument();
                XmlDoc.Load(TargetLngFile);
                LanguageCode = dr["Language_Code"].ToString();
                LngNCodeName = dr["Language_Name"].ToString();
                LanguageDirection = "ltr";

                //get Default Language code in case English is not present as a language.
                if (defaultLanguageCode.Trim() == "")
                {
                    defaultLanguageCode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value;
                }


                if (XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + LanguageCode + "']") != null)
                {
                    ////// if langusge file is containing language, check if default language is set 
                    //if (!string.IsNullOrEmpty(Global.GetDefaultLanguageCodeDB(DBNId.ToString(), string.Empty)))
                    //{
                    //    // get default language code from database
                    //    defaultLanguageCodeInDB = Global.GetDefaultLanguageCodeDB(DBNId.ToString(), string.Empty);
                    //}
                    if (LanguageCode == defaultLanguageCode)
                    {
                        // check if default node is not null or empty
                        if (!string.IsNullOrEmpty(XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value))
                        {
                            // check if default value is not set
                            if (XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value != defaultLanguageCode)
                            {
                                // set default node 
                                XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value = defaultLanguageCode;
                                File.SetAttributes(TargetLngFile, FileAttributes.Normal);
                                // save xml file
                                XmlDoc.Save(TargetLngFile);
                            }
                        }
                    }
                }
                else
                {
                    xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root);
                    NewNode = XmlDoc.CreateElement(Constants.XmlFile.Language.Tags.Language);
                    NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.Code, LanguageCode);
                    NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.Name, LngNCodeName);
                    NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.RTLDirection, (LanguageDirection == "rtl" ? "T" : "F"));
                    xmlNode.AppendChild(NewNode);
                    //Save xml file
                    File.SetAttributes(TargetLngFile, FileAttributes.Normal);
                    XmlDoc.Save(TargetLngFile);
                    //Create slide html pages for new language
                    SrcSlidersDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, defaultLanguageCode));
                    NewSlidersDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, LanguageCode));
                    Global.CopyDirectoryFiles(SrcSlidersDirPath, NewSlidersDirPath);
                    //Create Email template pages for new language
                    SrcEmailDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.EmailTemplates, defaultLanguageCode));
                    NewEmailDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.EmailTemplates, LanguageCode));
                    Global.CopyDirectoryFiles(SrcEmailDirPath, NewEmailDirPath);
                    //Create target language xml file
                    CreateNewTargetLngFile(LngNCodeName, defaultLanguageCode);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

    }

    /// <summary>
    /// Reverse Synchronize Language.xml with ut_language table in the database
    /// </summary>
    /// <param name="ObjDIConnection"></param>
    /// <returns></returns>
    private void ReverseSynchronizeLanguage(DIConnection DIConnection, int DBNId)
    {

        //int DBNId = Convert.ToInt32(Global.GetDefaultDbNId());
        string TargetLngFile = string.Empty;
        XmlDocument XmlDoc;
        string SrcSlidersDirPath = string.Empty;
        string NewSlidersDirPath = string.Empty;
        string SrcEmailDirPath = string.Empty;
        string NewEmailDirPath = string.Empty;
        string LanguageCode = string.Empty;
        string LanguageName = string.Empty;
        string LngFolderName = string.Empty;
        string LngFileForDelete = string.Empty;
        string SliderPathForDelete = string.Empty;
        string EmailTemplatePathForDelete = string.Empty;

        XmlNodeList LngCodeList;
        try
        {
            // Execute stored procedure to get all language codes in ut_language table


            DataTable AllCodes = DIConnection.ExecuteDataTable("SP_GET_ALL_LANGUAGE_CODES_NAMES", CommandType.StoredProcedure, null);

            TargetLngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(TargetLngFile);
            LngCodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Language.Tags.Root + "/child::node()");
            foreach (XmlNode xNode in LngCodeList)
            {
                LanguageCode = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Code].Value;
                LanguageName = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value;
                var result = from p in AllCodes.AsEnumerable()
                             where p.Field<string>("Language_Code") == LanguageCode
                             select p.Field<string>("Language_Name");
                if (result.Any())
                {
                    //do your work 
                }
                else
                {
                    xNode.ParentNode.RemoveChild(xNode);
                    //Get Data folder name
                    LngFolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language + LanguageCode);
                    //Delete data folder
                    ////////// if (Global.DeleteDirectory(LngFolderName))
                    // {
                    //////////////Delete language file from MasterKeyVals
                    ////////////LngFileForDelete = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals + LanguageName + ".xml");
                    ////////////if (File.Exists(LngFileForDelete))
                    ////////////{
                    ////////////    File.Delete(LngFileForDelete);
                    ////////////}

                    //////////////Slider html path
                    ////////////SliderPathForDelete = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, LanguageCode));
                    ////////////Global.DeleteDirectory(SliderPathForDelete);

                    //////////////Email Template path
                    //////////////Slider html path
                    ////////////EmailTemplatePathForDelete = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.EmailTemplates, LanguageCode));
                    ////////////Global.DeleteDirectory(EmailTemplatePathForDelete);

                    //save xml file
                    File.SetAttributes(TargetLngFile, FileAttributes.Normal);
                    XmlDoc.Save(TargetLngFile);
                    //}
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

    }

    /// <summary>
    /// Generate all codelists
    /// </summary>
    /// <param name="ObjDIConnection"></param>
    /// <param name="dataFolderName"></param>
    private string GenerateAllCodelists(DIConnection ObjDIConnection, string dataFolderName, string selectedDataTypes, string areaOrderBy, string quickSelectionType)
    {
        string RetVal = "false";
        string LanguageBasedOutputFolder = string.Empty;
        SDMXMLGenerator SDMXMLFileGenerator = null;
        ArrayList XmlDataList = new ArrayList();
        AreaQuickSelectionType QuickSelectionType = AreaQuickSelectionType.Immediate;

        foreach (string XmlDataType in Global.SplitString(selectedDataTypes, Constants.Delimiters.Comma))
        {
            XmlDataList.Add(XmlDataType);
        }

        try
        {
            SDMXMLFileGenerator = new SDMXMLGenerator(ObjDIConnection.ConnectionStringParameters.GetConnectionString(), Convert.ToInt32(ObjDIConnection.ConnectionStringParameters.ServerType));

            LanguageBasedOutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dataFolderName + "\\" + Constants.FolderName.DataFiles);

            if (XmlDataList.Contains(Constants.FolderName.Codelists.Area))
            {
                //-- Generate Area     
                switch (areaOrderBy)
                {
                    case "AreaName":
                        SDMXMLFileGenerator.SortByAreaName = true;
                        break;
                    case "AreaId":
                        SDMXMLFileGenerator.SortByAreaName = false;
                        break;
                    default:
                        break;
                }

                SDMXMLFileGenerator.AreaLevel = "_";
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.DI7Area, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Area, true);

                //-- Generate Area level            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Arealevel, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Area, true);
            }

            if (XmlDataList.Contains("quicksearch"))
            {
                switch (quickSelectionType)
                {
                    case "All":
                        QuickSelectionType = AreaQuickSelectionType.All;
                        break;
                    case "Immediate":
                        QuickSelectionType = AreaQuickSelectionType.Immediate;
                        break;
                    case "None":
                        QuickSelectionType = AreaQuickSelectionType.None;
                        break;
                    default:
                        break;
                }

                SDMXMLFileGenerator.SelectedAreaQuickSelectionType = QuickSelectionType;

                //-- Generate Area search            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.AreaSearch, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Area, true);

                //-- Generate Quick search                   
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.QuickSearch, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Area, true);
            }

            if (XmlDataList.Contains(Constants.FolderName.Codelists.Footnotes))
            {
                //-- Generate Footnotes            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Footnotes, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Footnotes, true);
            }

            if (XmlDataList.Contains(Constants.FolderName.Codelists.IC))
            {
                //-- Generate ic xml files e.g avl_ic_types.xml,sc.xml,sc_l1.xml,sc_l1_icNId.xml and for other ictypes etc.
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.IC, LanguageBasedOutputFolder, Constants.FolderName.Codelists.IC, true);
            }

            if (XmlDataList.Contains(Constants.FolderName.Codelists.IC_IUS))
            {
                //-- Generate ic-ius files iu_ICNId.xml            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.ic_ius, LanguageBasedOutputFolder, Constants.FolderName.Codelists.IC_IUS, true);
            }

            if (XmlDataList.Contains(Constants.FolderName.Codelists.IUS))
            {
                //-- Generate IUS codelist e.g. _iu_.xml, ius_IndicatorNID_UnitNID.xml and IUSSearch xml files            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.ius, LanguageBasedOutputFolder, Constants.FolderName.Codelists.IUS, true);
            }

            if (XmlDataList.Contains(Constants.FolderName.Codelists.Metadata))
            {
                //-- Generate Metadata            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.Metadata, LanguageBasedOutputFolder, Constants.FolderName.Codelists.Metadata, true);
            }


            if (XmlDataList.Contains(Constants.FolderName.Codelists.TP))
            {
                //-- Generate Time Period            
                SDMXMLFileGenerator.GenerateXmlCodeList(XMLCodeListType.tp, LanguageBasedOutputFolder, Constants.FolderName.Codelists.TP, true);
            }

            RetVal = "true";
        }
        catch (Exception ex)
        {
            if (Directory.Exists(LanguageBasedOutputFolder))
            {
                //Global.DeleteDirectory(LanguageBasedOutputFolder);
            }

            //Global.WriteErrorsInLog(ex.StackTrace + "--" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #region "--Cache Automatic Generation--"

    private static DataTable GetQsAreas(string SearchLanguage, int DBNId, Page OPage)
    {
        DataTable RetVal = new DataTable();

        RetVal.Columns.Add("Area_NId", typeof(string));

        RetVal.Columns.Add("Children_Count", typeof(int));

        XmlDocument MetadataDocument;

        MetadataDocument = null;

        try
        {
            MetadataDocument = new XmlDocument();

            MetadataDocument.Load(OPage.Server.MapPath(@"~/stock/data/" + DBNId.ToString() + @"/ds/" + SearchLanguage + @"/area/qscodelist.xml"));
            foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("a"))
            {

                DataRow dr = RetVal.NewRow();
                dr["Area_NId"] = Category.Attributes["id"].Value;
                dr["Children_Count"] = Category.Attributes["childrencount"].Value;

                RetVal.Rows.Add(dr);
            }

        }
        catch (Exception ex)
        {
            RetVal = new DataTable();
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion

    private bool InsertIntoCatalog(DIConnection ObjDIConnection, DataRow dataCountsRow, string dbAvailableLanguages)
    {
        bool RetVal = false;
        string[] YearsArr;

        string AdaptationName = string.Empty;
        string Description = string.Empty;
        string Version = "-1";
        bool IsDesktop = false;
        bool IsWeb = true;
        string WebURL = string.Empty;
        string AreaCount = string.Empty;
        string IUSCount = string.Empty;
        string TimePeriodsCount = string.Empty;
        string DataValuesCount = string.Empty;
        string StartYear = string.Empty;
        string EndYear = string.Empty;
        string LastModifiedOn = string.Empty;
        string AreaNId = string.Empty;
        string SubNation = string.Empty;
        string CatalogImage = string.Empty;
        string LangCode_CSVFiles = string.Empty;
        string ParamStr = string.Empty;

        try
        {
            AdaptationName = Global.adaptation_name;
            Description = this.DataBaseDescription;
            WebURL = Global.GetAdaptationUrl();

            //DBMtd_AreaCnt, DBMtd_IndCnt, DBMtd_SrcCnt, DBMtd_DataCnt
            AreaCount = dataCountsRow[0].ToString();
            IUSCount = dataCountsRow[1].ToString();
            TimePeriodsCount = dataCountsRow[2].ToString();
            DataValuesCount = dataCountsRow[3].ToString();

            YearsArr = Global.GetStartEndYear(ObjDIConnection);
            StartYear = YearsArr[0];
            EndYear = YearsArr[1];

            LastModifiedOn = string.Format("{0:dd-MM-yyyy}", DateTime.Today.Date);

            AreaNId = Global.area_nid;
            SubNation = Global.sub_nation;
            CatalogImage = WebURL + "/stock/themes/default/images/cust/logo.png";

            ParamStr = AdaptationName;
            ParamStr += Constants.Delimiters.ParamDelimiter + Description;
            ParamStr += Constants.Delimiters.ParamDelimiter + Version;
            ParamStr += Constants.Delimiters.ParamDelimiter + IsDesktop;
            ParamStr += Constants.Delimiters.ParamDelimiter + IsWeb;
            ParamStr += Constants.Delimiters.ParamDelimiter + WebURL;
            ParamStr += Constants.Delimiters.ParamDelimiter + AreaCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + IUSCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + TimePeriodsCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + DataValuesCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + StartYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + EndYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + LastModifiedOn;
            ParamStr += Constants.Delimiters.ParamDelimiter + AreaNId;
            ParamStr += Constants.Delimiters.ParamDelimiter + SubNation;
            ParamStr += Constants.Delimiters.ParamDelimiter + CatalogImage;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmName;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmInstitution;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmEmail;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.UnicefRegion;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.AdaptationYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + dbAvailableLanguages;
            ParamStr += Constants.Delimiters.ParamDelimiter + LangCode_CSVFiles;

            if (AdminSaveAdaptation(ParamStr) == "true")
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {

            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private bool GetAndUpdateIndexedAreas(string[] dbAvailableLanguage, DIConnection objDIConnection)
    {
        bool RetVal = false;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DataSet DSAreas = new DataSet();
        string TableName = string.Empty;
        // string AdaptationURL = string.Empty;

        try
        {
            //Get all area tables from db
            foreach (string LanguageCode in dbAvailableLanguage)
            {
                TableName = "ut_area_" + LanguageCode;

                DbParams.Clear();

                System.Data.Common.DbParameter Param1 = objDIConnection.CreateDBParameter();
                Param1.ParameterName = "TAB_NAME";
                Param1.DbType = DbType.String;
                Param1.Value = TableName;
                DbParams.Add(Param1);

                DataTable DtArea = objDIConnection.ExecuteDataTable("SP_GET_TABLE_DATA", CommandType.StoredProcedure, DbParams);
                DtArea.TableName = TableName;
                DSAreas.Tables.Add(DtArea);
            }

            if (DSAreas.Tables.Count > 0)
            {
                //AdaptationURL = Global.GetAdaptationUrl();

                //call webservice method UpdateIndexedAreas
                DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
                CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
                RetVal = CatalogService.UpdateIndexedAreas(DSAreas, Global.GetAdaptationGUID());

                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private bool GetAndUpdateIndexedIndicators(string[] dbAvailableLanguage, DIConnection objDIConnection)
    {
        bool RetVal = false;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DataSet DSIndicators = new DataSet();
        string TableName = string.Empty;
        //string AdaptationURL = string.Empty;

        try
        {
            //Get all area tables from db
            foreach (string LanguageCode in dbAvailableLanguage)
            {
                TableName = "ut_indicator_" + LanguageCode;

                DbParams.Clear();

                System.Data.Common.DbParameter Param1 = objDIConnection.CreateDBParameter();
                Param1.ParameterName = "TAB_NAME";
                Param1.DbType = DbType.String;
                Param1.Value = TableName;
                DbParams.Add(Param1);

                DataTable DtIndicator = objDIConnection.ExecuteDataTable("SP_GET_TABLE_DATA", CommandType.StoredProcedure, DbParams);
                DtIndicator.TableName = TableName;
                DtIndicator.Columns.Remove("Indicator_Info");
                DSIndicators.Tables.Add(DtIndicator);
            }

            if (DSIndicators.Tables.Count > 0)
            {
                //AdaptationURL = Global.GetAdaptationUrl();

                //call webservice method UpdateIndexedIndicators
                DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
                CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
                RetVal = CatalogService.UpdateIndexedIndicators(DSIndicators, Global.GetAdaptationGUID());

                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private bool UpdateAdaptations(DIConnection objDIConnection)
    {
        bool RetVal = false;
        string[] YearsArr;
        string AdaptationName = string.Empty;
        string Description = string.Empty;
        string Version = "-1";
        bool IsDesktop = false;
        bool IsWeb = true;
        string WebURL = string.Empty;
        string AreaCount = string.Empty;
        string IUSCount = string.Empty;
        string TimePeriodsCount = string.Empty;
        string DataValuesCount = string.Empty;
        string StartYear = string.Empty;
        string EndYear = string.Empty;
        string LastModifiedOn = string.Empty;
        string AreaNId = string.Empty;
        string SubNation = string.Empty;
        string CatalogImage = string.Empty;
        string DbAvailableLanguageStr = string.Empty;
        string[] DbAvailableLanguage = null;
        string LangCode_CSVFiles = string.Empty;
        string QryStr = string.Empty;
        string strDefaultLanguage = string.Empty;
        DataTable DTCounts;
        string ParamStr = string.Empty;

        try
        {
            AdaptationName = Global.adaptation_name;
            WebURL = Global.GetAdaptationUrl();
            //DBMtd_AreaCnt, DBMtd_IndCnt, DBMtd_SrcCnt, DBMtd_DataCnt
            QryStr = "SELECT Language_Code FROM ut_language WHERE Language_Default = 1";
            DTCounts = objDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", objDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
            if (DTCounts.Rows.Count == 1) strDefaultLanguage = DTCounts.Rows[0]["Language_Code"].ToString();
            else strDefaultLanguage = "en";

            // Get counts from db
            QryStr = "select DBMtd_AreaCnt, DBMtd_IndCnt, DBMtd_SrcCnt, DBMtd_DataCnt from ut_dbmetadata_" + strDefaultLanguage;
            DTCounts = objDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", objDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
            DataRow dataCountsRow = DTCounts.Rows[0];
            AreaCount = dataCountsRow[0].ToString();
            IUSCount = dataCountsRow[1].ToString();
            TimePeriodsCount = dataCountsRow[2].ToString();
            DataValuesCount = dataCountsRow[3].ToString();

            YearsArr = Global.GetStartEndYear(objDIConnection);
            StartYear = YearsArr[0];
            EndYear = YearsArr[1];
            LastModifiedOn = string.Format("{0:dd-MM-yyyy}", DateTime.Today.Date);
            AreaNId = Global.area_nid;
            SubNation = Global.sub_nation;
            Description = this.DataBaseDescription;
            CatalogImage = WebURL + "/stock/themes/default/images/cust/logo.png";

            DbAvailableLanguage = Global.GetAllAvailableLanguageCode(objDIConnection);

            if (DbAvailableLanguage.Length > 0)
            {
                DbAvailableLanguageStr = string.Join(",", DbAvailableLanguage);
            }

            ParamStr = AdaptationName;
            ParamStr += Constants.Delimiters.ParamDelimiter + Description;
            ParamStr += Constants.Delimiters.ParamDelimiter + Version;
            ParamStr += Constants.Delimiters.ParamDelimiter + IsDesktop;
            ParamStr += Constants.Delimiters.ParamDelimiter + IsWeb;
            ParamStr += Constants.Delimiters.ParamDelimiter + WebURL;
            ParamStr += Constants.Delimiters.ParamDelimiter + AreaCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + IUSCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + TimePeriodsCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + DataValuesCount;
            ParamStr += Constants.Delimiters.ParamDelimiter + StartYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + EndYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + LastModifiedOn;
            ParamStr += Constants.Delimiters.ParamDelimiter + AreaNId;
            ParamStr += Constants.Delimiters.ParamDelimiter + SubNation;
            ParamStr += Constants.Delimiters.ParamDelimiter + CatalogImage;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmName;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmInstitution;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.DbAdmEmail;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.UnicefRegion;
            ParamStr += Constants.Delimiters.ParamDelimiter + Global.AdaptationYear;
            ParamStr += Constants.Delimiters.ParamDelimiter + DbAvailableLanguageStr;
            ParamStr += Constants.Delimiters.ParamDelimiter + LangCode_CSVFiles;
            if (AdminSaveAdaptation(ParamStr) == "true")
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {

            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;

    }

    #endregion

    #region "--Language Settings--"

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetLngName"></param>
    /// <returns></returns>
    private string CreateNewTargetLngFile(string targetLngName, string defaultNewLanguageCode)
    {
        string RetVal = "false";
        string BaseLanguageFile = string.Empty;
        string TargetLanguageFile = string.Empty;
        XmlDocument XmlDoc;
        string LngFile = string.Empty;
        XmlDocument XmlDocLangFile;
        string EnglishLanguageCode = "en";
        string defaultLanguageCode = string.Empty;
        string LanguageName = string.Empty;
        XmlNode xmlNode;


        try
        {
            //get Default Language code in case Engliash is not present as a language
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDocLangFile = new XmlDocument();
            XmlDocLangFile.Load(LngFile);
            //if (XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + EnglishLanguageCode + "']") == null)
            //{
            //    defaultLanguageCode = Global.GetDefaultLanguageCodeDB(Global.GetDefaultDbNId().ToString(), string.Empty);
            //    xmlNode = XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + defaultLanguageCode + "']");
            //    LanguageName = xmlNode.Attributes["n"].Value;

            //}
            //else
            //{
            //    defaultLanguageCode = EnglishLanguageCode;
            //    LanguageName = "English [en]";

            //}
            defaultLanguageCode = XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value;

            xmlNode = XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + defaultLanguageCode + "']");
            LanguageName = xmlNode.Attributes["n"].Value;

            if (defaultNewLanguageCode == string.Empty)
            {
                XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value = Global.GetDefaultLanguageCodeDB(Global.GetDefaultDbNId().ToString(), string.Empty);
            }
            else
            {
                XmlDocLangFile.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root).Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value = defaultNewLanguageCode;
            }

            XmlDocLangFile.Save(LngFile);
            BaseLanguageFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals + LanguageName + ".xml");
            //BaseLanguageFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.BaseLanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(BaseLanguageFile);

            TargetLanguageFile = Path.Combine(Path.GetDirectoryName(BaseLanguageFile), targetLngName + ".xml");
            if (!File.Exists(TargetLanguageFile))
            {
                File.Copy(BaseLanguageFile, TargetLanguageFile);
            }

            //Read new language file and prepend # in each value
            //XmlDoc = new XmlDocument();
            //XmlDoc.Load(TargetLanguageFile);
            //xmlNodeList = XmlDoc.SelectNodes("root/child::node()");
            //foreach (XmlNode xNode in xmlNodeList)
            //{
            //    try
            //    {
            //        xNode.Attributes["value"].Value = "#" + xNode.Attributes["value"].Value;
            //    }
            //    catch (Exception ex)
            //    {
            //        //Global.WriteErrorsInLog(ex.StackTrace + "--" + ex.Message);
            //        //Global.WriteErrorsInLog("Key : " + xNode.Attributes["key"].Value);
            //        Global.CreateExceptionString(ex, null);
            //    }
            //}

            //File.SetAttributes(TargetLanguageFile, FileAttributes.Normal);
            //XmlDoc.Save(TargetLanguageFile);

            RetVal = "true";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private List<string> getAllDbLangCodes(int DBNId)
    {
        List<string> allLanguages = new List<string>();

        DIConnection DIConnection;
        DIConnection = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);

            DataTable AllCodes = DIConnection.ExecuteDataTable("SP_GET_ALL_LANGUAGE_CODES", CommandType.StoredProcedure, null);

            foreach (DataRow dr in AllCodes.Rows) allLanguages.Add(dr[0].ToString());
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return allLanguages;
    }

    #endregion

    #endregion
    #endregion

    #region "--Public--"

    #region "--Methods--"

    #region "--Application Settings--"

    /// <summary>
    /// Update configuration setting in appsettings.xml file
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminUpdateConfiguration(string requestParam)
    {
        string RetVal = string.Empty; ;
        string[] Params;
        string AdapatationName = string.Empty;
        string DefaultLng = string.Empty;
        string ShowDbSelection = string.Empty;
        string Version = string.Empty;
        string DiUiLibUrl = string.Empty;
        string DiUiLibThemCss = string.Empty;
        string FBAppID = string.Empty;
        string FBAppSecret = string.Empty;
        string TwAppID = string.Empty;
        string TwAppSecret = string.Empty;
        string ShowSliders = string.Empty;
        string MrdThreshold = string.Empty;
        string HideSourceColumn = string.Empty;
        string DSDSelection = string.Empty;
        string QDSCache = string.Empty;
        string ShowDIB = string.Empty;
        string StandaloneRegistry = string.Empty;
        string ApplicationVersion = string.Empty;
        string AdaptedFor = string.Empty;
        string AreaNId = string.Empty;
        string SubNation = string.Empty;
        string SlideCount = string.Empty;
        string QDSGallery = string.Empty;
        string RegistryAreaLevel = string.Empty;
        string RegistryMSDAreaId = string.Empty;
        string RegistryNotifyViaEmail = string.Empty;
        string RegistryMappingAgeDefaultValue = string.Empty;
        string RegistryMappingSexDefaultValue = string.Empty;
        string RegistryMappingLocationDefaultValue = string.Empty;
        string RegistryMappingFrequencyDefaultValue = string.Empty;
        string RegistryMappingSourceDefaultValue = string.Empty;
        string RegistryMappingNatureDefaultValue = string.Empty;
        string RegistryMappingUnitMultDefaultValue = string.Empty;

        string AdaptationYear = string.Empty;
        string UnicefRegion = string.Empty;
        string IsDesktopVersionAvailable = string.Empty;
        string DbAdmName = string.Empty;
        string DbAdmInstitution = string.Empty;
        string DbAdmEmail = string.Empty;

        string NewsMenuEnabled = string.Empty;
        string InnovationsMenuEnabled = string.Empty;
        string QDSCloud = string.Empty;
        string SupportEnabled = string.Empty;
        string ContactUsEnabled = string.Empty;
        string AppSettingFile = string.Empty;
        string JSVersion = string.Empty;
        string DownloadsLinkEnabled = string.Empty;
        string TrainingLinkEnabled = string.Empty;
        string MapLibraryLinkEnabled = string.Empty;
        string RSSFeedsLinkEnabled = string.Empty;
        string DiWorldWideLinkEnabled = string.Empty;
        string GoogleAnalyticsId = string.Empty;
        string Country = string.Empty;
        string typeOfEmail = string.Empty;
        string DateCreated = string.Empty;
        string CategoryScheme = string.Empty;
        string SliderAnimationSpeed = string.Empty;
        string FAQ = string.Empty;
        string SiteMap = string.Empty;
        string HowToVideos = string.Empty;
        XmlDocument XmlDoc;
        bool IsCMSDatabaseCreated = false;
        string XLSFileMsg = string.Empty;
        string HowToVideo = string.Empty;
        string[] HowToVideoArr = null;
        string[] HowToVideoIndivisualArr = null;
        string HowToVideoJSONString = string.Empty;
        string OutCmsDatabaseName = string.Empty;// This variable is used for storing Name of database created for CMS
        string MetaTag_Desc = string.Empty;
        string MetaTag_Kw = string.Empty;
        string KB = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            AdapatationName = Params[0];
            DefaultLng = Params[1];
            Version = Params[2];
            DiUiLibUrl = Params[3];
            DiUiLibThemCss = Params[4];
            FBAppID = Params[5];
            FBAppSecret = Params[6];
            TwAppID = Params[7];
            TwAppSecret = Params[8];
            ShowSliders = Params[9];
            MrdThreshold = Params[10];
            HideSourceColumn = Params[11];
            DSDSelection = Params[12];
            QDSCache = Params[13];
            ShowDIB = Params[14];
            StandaloneRegistry = Params[15];
            JSVersion = Params[16];
            AdaptedFor = Params[17];
            AreaNId = Params[18];
            SubNation = Params[19];
            SlideCount = Params[20];
            AdaptationYear = Params[21];
            UnicefRegion = Params[22];
            IsDesktopVersionAvailable = Params[23];
            DbAdmName = Params[24];
            DbAdmInstitution = Params[25];
            DbAdmEmail = Params[26];
            QDSGallery = Params[27];
            RegistryAreaLevel = Params[28];
            RegistryMSDAreaId = Params[29];
            RegistryNotifyViaEmail = Params[30];
            RegistryMappingAgeDefaultValue = Params[31];
            RegistryMappingSexDefaultValue = Params[32];
            RegistryMappingLocationDefaultValue = Params[33];
            RegistryMappingFrequencyDefaultValue = Params[34];
            RegistryMappingSourceDefaultValue = Params[35];
            RegistryMappingNatureDefaultValue = Params[36];
            RegistryMappingUnitMultDefaultValue = Params[37];

            NewsMenuEnabled = Params[38];
            InnovationsMenuEnabled = Params[39];
            QDSCloud = Params[40];
            ContactUsEnabled = Params[41];
            SupportEnabled = Params[42];
            ApplicationVersion = Params[43];
            DownloadsLinkEnabled = Params[44];
            TrainingLinkEnabled = Params[45];
            MapLibraryLinkEnabled = Params[46];
            RSSFeedsLinkEnabled = Params[47];
            DiWorldWideLinkEnabled = Params[48];
            GoogleAnalyticsId = Params[49];
            Country = Params[50];
            CategoryScheme = Params[51];
            SliderAnimationSpeed = Params[52];
            FAQ = Params[53];
            SiteMap = Params[54];
            MetaTag_Desc = Params[55];
            MetaTag_Kw = Params[56];
            KB = Params[57];
            HowToVideos = Params[58];
            try
            {
                HowToVideo = Params[59];
                HowToVideoArr = HowToVideo.Split(new string[] { "[**]" }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);
            }


            // If user enables news menu then call method to geneate database for news if CMS databese is not already existing
            if (NewsMenuEnabled == "true")
            {
                IsCMSDatabaseCreated = CheckNCreateCMSDatabase(out OutCmsDatabaseName);
                // IF CMS database created successfull call method to add entry in csv log file
                if (IsCMSDatabaseCreated)
                {
                    // If name op creted CMS database is not null or empty
                    if (!string.IsNullOrEmpty(OutCmsDatabaseName))
                    {
                        // Create Message to write in CSV file
                        XLSFileMsg = string.Format(Constants.CSVLogMessage.CmsDatabaseCreated, AdapatationName, OutCmsDatabaseName);
                        // Call method to write CSV log For CMS databse creation
                        WriteLogInXLSFile(Constants.AdminModules.AppSettings.ToString(), XLSFileMsg);
                    }
                }
            }

            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);

            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.Country, string.Empty);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.CategoryScheme, "SC");

            // check if key islangFAQVisible exist in appsetting.xml file, else create key
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.islangFAQVisible, string.Empty);

            XmlDoc.Load(AppSettingFile);
            //Application
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.adaptation_name, AdapatationName);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.adapted_for, AdaptedFor);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.area_nid, AreaNId);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.sub_nation, SubNation);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.default_lng, DefaultLng);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.diuilib_theme_css, DiUiLibThemCss);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.show_sliders, ShowSliders);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.slider_count, SlideCount);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.dvMrdThreshold, MrdThreshold);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.dvHideSource, HideSourceColumn);

            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isQdsGeneratedForDensedQS_Areas, QDSCache);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.showdisputed, ShowDIB);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.adaptation_mode, StandaloneRegistry);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.js_version, JSVersion);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.adaptation_year, AdaptationYear);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.unicef_region, UnicefRegion);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isDesktopVersionAvailable, IsDesktopVersionAvailable);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.enableQDSGallery, QDSGallery);

            //Database Administrator
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmName, DbAdmName);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmInstitution, DbAdmInstitution);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmEmail, DbAdmEmail);


            //Web Components
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.diuilib_version, Version);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.diuilib_url, DiUiLibUrl);

            //Share
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.fbAppID, FBAppID);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.fbAppSecret, FBAppSecret);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.twAppID, TwAppID);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.twAppSecret, TwAppSecret);

            //Registry
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.enableDSDSelection, DSDSelection);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryAreaLevel, RegistryAreaLevel);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId, RegistryMSDAreaId);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryNotifyViaEmail, RegistryNotifyViaEmail);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingAgeDefaultValue, RegistryMappingAgeDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingSexDefaultValue, RegistryMappingSexDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingLocationDefaultValue, RegistryMappingLocationDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingFrequencyDefaultValue, RegistryMappingFrequencyDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingSourceTypeDefaultValue, RegistryMappingSourceDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingNatureDefaultValue, RegistryMappingNatureDefaultValue);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMappingUnitMultDefaultValue, RegistryMappingUnitMultDefaultValue);

            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isNewsMenuVisible, NewsMenuEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isInnovationsMenuVisible, InnovationsMenuEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.show_cloud, QDSCloud);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isContactUsMenuVisible, ContactUsEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isSupportMenuVisible, SupportEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.app_version, ApplicationVersion);

            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isDownloadsLinkVisible, DownloadsLinkEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isTrainingLinkVisible, TrainingLinkEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isMapLibraryLinkVisible, MapLibraryLinkEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isRSSFeedsLinkVisible, RSSFeedsLinkEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isDiWorldWideLinkVisible, DiWorldWideLinkEnabled);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.gaUniqueID, GoogleAnalyticsId);

            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.Country, Country);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.CategoryScheme, CategoryScheme);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.SliderAnimationSpeed, SliderAnimationSpeed);

            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.islangFAQVisible, FAQ);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isSitemapVisible, SiteMap);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.isHowToVideoVisible, HowToVideos);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Desc, MetaTag_Desc);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Kw, MetaTag_Kw);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.islangKBVisible, KB);

            //Save How to Video
            if (HowToVideoArr != null)
            {
                dynamic dynObj = JsonConvert.DeserializeObject(Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.HowToVideoData).ToString());
                for (int i = 0; i < HowToVideoArr.Length; i++)
                {
                    foreach (var data in dynObj)
                    {
                        if ((data).Name == HowToVideoArr[i].ToString().Substring(0, 2))
                        {
                            HowToVideoIndivisualArr = HowToVideoArr[i].ToString().Split(new string[] { "[*]" }, StringSplitOptions.None);
                            foreach (JArray item in data)
                            {
                                foreach (JToken dataItem in item.Children())
                                {
                                    dataItem["HomeVisible"] = HowToVideoIndivisualArr[1];
                                    dataItem["VisualizerVisible"] = HowToVideoIndivisualArr[2];
                                    dataItem["Homelink"] = HowToVideoIndivisualArr[3];
                                    dataItem["Visualizerlink"] = HowToVideoIndivisualArr[4];
                                }
                            }
                        }
                    }
                }
                HowToVideoJSONString = JsonConvert.SerializeObject(dynObj);
                HowToVideoJSONString = HowToVideoJSONString.Replace("\"", "'");
                this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.HowToVideoData, HowToVideoJSONString);
            }

            File.SetAttributes(AppSettingFile, FileAttributes.Normal);
            XmlDoc.Save(AppSettingFile);

            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            CatalogService.UpdateCatalogInfo(AdapatationName, "", Global.GetAdaptationUrl(), DateTime.Now.ToString(), Convert.ToInt32(AreaNId), SubNation, DbAdmName, DbAdmInstitution, DbAdmEmail, UnicefRegion, AdaptationYear, AdaptedFor, Country, Global.GetAdaptationGUID());
            DataSet dsCatalogAdaptation = new DataSet();
            dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationGUID());
            string Visible = string.Empty;
            if (dsCatalogAdaptation.Tables[0].Rows.Count == 0)
            {
                typeOfEmail = "New";
                DateCreated = String.Format("{0:r}", DateTime.Now);
            }
            else
            {
                typeOfEmail = "Updated";
                if (!string.IsNullOrEmpty(dsCatalogAdaptation.Tables[0].Rows[0][1].ToString()))
                {
                    DateCreated = String.Format("{0:r}", Convert.ToDateTime(dsCatalogAdaptation.Tables[0].Rows[0][1]));
                }
                Visible = dsCatalogAdaptation.Tables[0].Rows[0][2].ToString();

            }
            Frame_Message_And_Send_Catalog_Mail(AdapatationName, Global.GetAdaptationUrl(), Visible, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, SubNation, DateCreated, String.Format("{0:r}", DateTime.Now), typeOfEmail);
            RetVal = "true";

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion

    #region "--Database Settings--"

    public string GetDataDescription(string requestParam)
    {
        string RetVal = string.Empty;
        DataTable DTMetadata = null;
        string[] DBDetails = requestParam.Split(new string[] { "[~]" }, StringSplitOptions.None);
        DIConnection ObjDIConnection = null;
        ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(0)), DBDetails[0].ToString(), string.Empty, DBDetails[1].ToString(), DBDetails[2].ToString(), DBDetails[3].ToString());
        DTMetadata = ObjDIConnection.ExecuteDataTable("SELECT DBMtd_Desc FROM " + ObjDIConnection.DIDataSetDefault() + "dbmetadata" + ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault()).ToString());
        if (DTMetadata.Rows.Count > 0)
        {
            RetVal = DTMetadata.Rows[0]["DBMtd_Desc"].ToString();
        }
        return RetVal;
    }


    /// <summary>
    /// Get available connections by selected category name
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminGetAvlConnections(string requestParam)
    {
        string RetVal = string.Empty;
        Dictionary<string, string> ConnDetails = new Dictionary<string, string>();


        try
        {
            ConnDetails = Global.GetAllConnections(requestParam);

            foreach (KeyValuePair<string, string> Data in ConnDetails)
            {
                RetVal += "," + Data.Key + Constants.Delimiters.ValuesDelimiter + Data.Value;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        if (!string.IsNullOrEmpty(RetVal))
        {
            RetVal = RetVal.Substring(1);
        }

        return RetVal;
    }

    /// <summary>
    /// Get connection details data by dbNId
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminGetConnectionDetailsData(string requestParam)
    {
        string RetVal = string.Empty;
        string[] DBDetails;

        try
        {
            //Get connection details of database
            DBDetails = Global.GetDbConnectionDetails(requestParam);

            if (DBDetails.Length > 0)
            {
                RetVal = DBDetails[0];
                RetVal += Constants.Delimiters.ValuesDelimiter + DBDetails[1];
                RetVal += Constants.Delimiters.ValuesDelimiter + DBDetails[2];
                RetVal += Constants.Delimiters.ValuesDelimiter + DBDetails[3];
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }


    /// Update connection details by selected db
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminUpdateDbConnection(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string DbNId = string.Empty;
        string ConnName = string.Empty;
        string DbConn = string.Empty;
        string Password = string.Empty;
        string DefArea = string.Empty;
        string Description = string.Empty;
        string IsDefDb = string.Empty;
        string CategoryName = string.Empty;

        XmlElement NewCategoryElement;
        XmlNode NewXmlNode;
        XmlNode CategoryNode;

        DIConnection ObjDIConnection = null;
        string[] DbAvailableLanguage;

        string strDefaultLanguage = string.Empty;
        string CountStr = string.Empty;
        DataTable DTCounts;
        string QryStr = string.Empty;
        string DbDefaultLanguage = string.Empty;
        string DbAvailableLanguageStr = string.Empty;
        string DefAreaJSon = string.Empty;
        string DefAreaCount = "0";
        string[] DefAreaStr;
        string[] DefIndStr;
        string DefInd = string.Empty;
        string DefIndJSon = string.Empty;

        DBConverterDecorator objDBConverterDecorator = null;
        DBVersionBuilder VersionBuilder = null;
        ///Variables for creatin XLSLogfile 
        string[] DatabaseParams;
        string XLSFileMsg = string.Empty;
        string Server_HostName = string.Empty;
        string DatabaseName = string.Empty;
        string DataBaseUserName = string.Empty;
        string[] DBConnArr;
        try
        {
            requestParam = HttpUtility.UrlDecode(requestParam);
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNId = Params[0];
            ConnName = Params[1];
            DbConn = Params[2];
            Password = Params[3];
            Description = Params[4];
            IsDefDb = Params[5];
            CategoryName = Params[6];

            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            DBConnArr = DbConn.Split(new string[] { "||" }, StringSplitOptions.None);
            ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(DBConnArr[0])), DBConnArr[1].ToString(), string.Empty, DBConnArr[2].ToString(), DBConnArr[3].ToString(), Password);

            if (!string.IsNullOrEmpty(Password))
            {
                Password = Global.EncryptString(Password);
                DbConn = DbConn + Password;
            }
            QryStr = "SELECT Language_Code FROM ut_language WHERE Language_Default = 1";
            DTCounts = ObjDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", ObjDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
            if (DTCounts.Rows.Count == 1) strDefaultLanguage = DTCounts.Rows[0]["Language_Code"].ToString();
            else strDefaultLanguage = "en";

            // Get counts from db
            QryStr = "select DBMtd_AreaCnt, DBMtd_IndCnt, DBMtd_SrcCnt, DBMtd_DataCnt from ut_dbmetadata_" + strDefaultLanguage;
            DTCounts = ObjDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", ObjDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
            DataRow Row = DTCounts.Rows[0];
            CountStr = string.Format("{0:0,0}", Row[0]) + "_" + string.Format("{0:0,0}", Row[1]) + "_" + string.Format("{0:0,0}", Row[2]) + "_" + string.Format("{0:0,0}", Row[3]);
            //get default language code in database
            DbDefaultLanguage = ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault());

            //get all avalilable language code in database
            DbAvailableLanguage = Global.GetAllAvailableLanguageCode(ObjDIConnection);
            if (DbAvailableLanguage.Length > 0)
            {
                DbAvailableLanguageStr = string.Join(",", DbAvailableLanguage);
            }

            #region -- get default indicator and their Json data --

            DefIndStr = GetDefaultIndicators(ObjDIConnection, strDefaultLanguage);
            DefInd = DefIndStr[0];
            DefIndJSon = DefIndStr[1];

            #endregion

            #region --get default area of level 1 and 2 (L1 + L2) by stored procedure with their json and counts --

            DefAreaStr = GetDefaultAreas(ObjDIConnection, strDefaultLanguage);
            DefArea = DefAreaStr[0];
            DefAreaJSon = DefAreaStr[1];
            DefAreaCount = DefAreaStr[2];

            #endregion

            //Check connection name already exists or not
            if (XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Name + "='" + ConnName + "']") != null && XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Name + "='" + ConnName + "']").Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value != DbNId.ToString())
            {
                RetVal = "exists";
            }
            else
            {
                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");
                if (xmlNode.Attributes["langcode_csvfiles"] == null)
                {
                    XmlAttribute attrCSV = XmlDoc.CreateAttribute("langcode_csvfiles");
                    attrCSV.Value = string.Empty;
                    xmlNode.Attributes.Append(attrCSV);
                }
                else
                {
                    xmlNode.Attributes["langcode_csvfiles"].Value = string.Empty;
                }
                if (xmlNode.ParentNode.Attributes[Constants.XmlFile.Db.Tags.CategoryAttributes.Name].Value == CategoryName)
                {
                    //Update for same category name
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value = ConnName;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value = DbConn;
                    if (xmlNode.Attributes["desc" + DbDefaultLanguage] == null)
                    {
                        XmlAttribute attrDesc = XmlDoc.CreateAttribute("desc" + DbDefaultLanguage);
                        attrDesc.Value = Description;
                        xmlNode.Attributes.Append(attrDesc);
                    }
                    else
                    {
                        xmlNode.Attributes["desc" + DbDefaultLanguage].Value = Description;
                    }

                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Count].Value = CountStr;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value = string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date);
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AvailableLanguage].Value = DbAvailableLanguageStr;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage].Value = DbDefaultLanguage.Substring(1);
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator].Value = DefInd;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON].Value = DefIndJSon;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea].Value = DefArea;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON].Value = DefAreaJSon;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount].Value = DefAreaCount;

                    //Update default dbNId
                    if (IsDefDb == "true")
                    {
                        XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root).Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value = DbNId;
                    }

                    File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
                    XmlDoc.Save(DBConnectionsFile);

                    //Update Metadata Description in the database                   
                    ObjDIConnection.ExecuteNonQuery("UPDATE " + ObjDIConnection.DIDataSetDefault() + "dbmetadata" + ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault()).ToString() + " SET DBMtd_Desc = '" + Description.Replace("'", "''") + "'");
                    RetVal = "true";
                }
                else if (xmlNode.ParentNode.Attributes[Constants.XmlFile.Db.Tags.CategoryAttributes.Name].Value != CategoryName)
                {
                    //Update when category name has changed for same id
                    //Update values in node
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value = ConnName;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value = DbConn;
                    xmlNode.Attributes["desc_en"].Value = Description;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Count].Value = CountStr;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value = string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date);
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AvailableLanguage].Value = DbAvailableLanguageStr;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage].Value = DbDefaultLanguage.Substring(1);
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator].Value = DefInd;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON].Value = DefIndJSon;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea].Value = DefArea;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON].Value = DefAreaJSon;
                    xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount].Value = DefAreaCount;
                    //Update default dbNId
                    if (IsDefDb == "true")
                    {
                        XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root).Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value = DbNId;
                    }
                    //Copy updated node values in new node
                    NewXmlNode = xmlNode;

                    //Remove current node
                    xmlNode.ParentNode.RemoveChild(xmlNode);

                    //Read the category node
                    CategoryNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + CategoryName + "']");

                    //Create a category node if not exists
                    if (CategoryNode == null)
                    {
                        NewCategoryElement = XmlDoc.CreateElement(Constants.XmlFile.Db.Tags.Category);
                        NewCategoryElement.SetAttribute(Constants.XmlFile.Db.Tags.CategoryAttributes.Name, CategoryName);
                        CategoryNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root).AppendChild(NewCategoryElement);
                    }

                    //Append the new node in category
                    CategoryNode.AppendChild(NewXmlNode);
                    File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
                    XmlDoc.Save(DBConnectionsFile);
                    RetVal = "true";
                    //Update Metadata Description in the database                   
                    ObjDIConnection.ExecuteNonQuery("UPDATE " + ObjDIConnection.DIDataSetDefault() + "dbmetadata" + ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault()).ToString() + " SET DBMtd_Desc = '" + Description.Replace("'", "''") + "'");
                }
                if (Global.standalone_registry != "true")
                {

                    this.RunDBScripts(ObjDIConnection, DbNId, DbDefaultLanguage.Substring(1));
                    // Generate language file because RunDBScripts method deletes existing language file, so user will not be able to execute furter functionality. 
                    GenerateAllPagesXML();
                    VersionBuilder = new DBVersionBuilder(ObjDIConnection, new DIQueries(ObjDIConnection.DIDataSetDefault(), ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault())));
                    VersionBuilder.InsertVersionInfo(Constants.DBVersion.DI7_0_0_0, Constants.DBVersion.VersionsChangedDatesDI7_0_0_0, Constants.DBVersion.VersionCommentsDI7_0_0_0);
                }
                // Set description filed to update in service database 
                this.DataBaseDescription = Description.Replace("'", "''").Replace("\n", "<br />").Replace(@"""", @"\""");

                GetAndUpdateIndexedAreas(DbAvailableLanguage, ObjDIConnection);
                GetAndUpdateIndexedIndicators(DbAvailableLanguage, ObjDIConnection);
                UpdateAdaptations(ObjDIConnection);
                DeleteSitemapFiles();

            }
            #region "Call method to write log in XLS file"
            if (RetVal == "true")
            {
                DatabaseParams = DbConn.Split(new string[] { "||" }, StringSplitOptions.None);
                if (!string.IsNullOrEmpty(DatabaseParams[1].ToString()))
                {
                    Server_HostName = DatabaseParams[1].ToString();
                }
                if (!string.IsNullOrEmpty(DatabaseParams[2].ToString()))
                {
                    DatabaseName = DatabaseParams[2].ToString();
                }
                if (!string.IsNullOrEmpty(DatabaseParams[3].ToString()))
                {
                    DataBaseUserName = DatabaseParams[3].ToString();
                }
                //"Connection Name:{0}, Server/Host Name:{1}, Database Name:{2}, User Name:{3}, Description:{4}";
                XLSFileMsg = string.Format(Constants.CSVLogMessage.UpdateConnection, ConnName, Server_HostName, DatabaseName, DataBaseUserName, Description);
                WriteLogInXLSFile(Constants.AdminModules.DatabaseSettings.ToString(), XLSFileMsg);
            }
            #endregion
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Delete all the sitemap located in the root directory
    /// </summary>
    private void DeleteSitemapFiles()
    {
        string FolderPath = HttpContext.Current.Request.PhysicalApplicationPath;
        string FilesToDelete = @"*sitemap.xml";
        string[] FileList = System.IO.Directory.GetFiles(FolderPath, FilesToDelete);
        foreach (string file in FileList)
        {
            File.Delete(file);
        }
    }

    private void UpdateMRDTable(DIConnection ObjDIConnection)
    {
        string Query = string.Empty;
        string TempTableName = "temp1";
        string DTData = ObjDIConnection.DIDataSetDefault() + "Data";
        // 1. drop temp1 table 
        try
        {
            ObjDIConnection.ExecuteNonQuery("Drop table " + TempTableName);
        }
        catch (Exception)
        {

        }

        // 2. Create Temp1 table for IsMRD calculation
        Query = "Select MRDTable.*, T2.Timeperiod_nid into " + TempTableName + " from  ( SELECT  d.IUSNId, d.Area_NId, MAX(t.TimePeriod) AS timeperiod  FROM " + ObjDIConnection.DIDataSetDefault() + "Data" + " d," + ObjDIConnection.DIDataSetDefault() + "TimePeriod" + " t WHERE d.TimePeriod_NId= t.TimePeriod_NId GROUP BY d.IUSNId,d.Area_NId) AS MRDTable , " + ObjDIConnection.DIDataSetDefault() + "TimePeriod" + " T2 where MRDTable.timeperiod=T2.Timeperiod";
        ObjDIConnection.ExecuteNonQuery(Query);

        // 3. set IsMrd to false in data table
        Query = "UPDATE  " + ObjDIConnection.DIDataSetDefault() + "Data" + " SET IsMRD=0, MultipleSource=0";
        ObjDIConnection.ExecuteNonQuery(Query);


        // 4. update IsMrd in DataTable
        Query = "UPDATE " + DTData + " SET " + DTData + ".IsMRD=1, " + DTData + ".MultipleSource=1 FROM temp1 INNER JOIN " + DTData + " ON temp1.IUSNId = " + DTData + ".IUSNId AND temp1.Timeperiod_nid = " + DTData + ".TimePeriod_NId AND temp1.Area_NId = " + DTData + ".Area_NId";
        ObjDIConnection.ExecuteNonQuery(Query);

        //4. drop table
        ObjDIConnection.ExecuteNonQuery("Drop table " + TempTableName);
    }


    /// <summary>
    /// Test to connection details
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminTestConnection(string requestParam)
    {
        string RetVal = "false";

        string[] Params;
        string ServerType = string.Empty;
        string ServerName = string.Empty;
        string DatabaseName = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            ServerType = Params[0];
            ServerName = Params[1];
            DatabaseName = Params[2];
            UserName = Params[3];

            if (Params.Length > 4)
            {
                Password = Params[4];
            }


            if (TestConnection((DIServerType)int.Parse(ServerType), ServerName, "", DatabaseName, UserName, Password))
            {
                RetVal = "true";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Delete connection details from db.xml file
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminDeleteConnection(string requestParam)
    {
        string RetVal = "false";
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        XmlNode defaultDb;
        string DbNId = string.Empty;
        string[] Params;
        string DbFolderName = string.Empty;
        string AssociatedDBNids = string.Empty;
        XmlNodeList associatedDBNodes;
        XmlNode AssociatedDB;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNId = Params[0];

            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");
            xmlNode.ParentNode.RemoveChild(xmlNode);

            associatedDBNodes = XmlDoc.SelectNodes("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb + "=" + DbNId + "]");
            foreach (XmlNode node in associatedDBNodes)
            {
                AssociatedDBNids = node.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value.ToString();
                // AssociatedDBNids = node.Attributes["id"].ToString();
                node.ParentNode.RemoveChild(node);

                //Get Data folder name
                DbFolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + AssociatedDBNids);

                // Delete SDMX entries in database.
                if (Directory.Exists(DbFolderName))
                {
                    this.Delete_Artefacts_Details_In_Database(AssociatedDBNids);
                }

                //Delete data folder
                if (Global.DeleteDirectory(DbFolderName))
                {
                    //save xml file
                    File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);

                    XmlDoc.Save(DBConnectionsFile);

                    RetVal = "true";
                }
            }
            AssociatedDB = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb + "=" + DbNId + "]");
            //Get Data folder name
            DbFolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId);

            // Delete SDMX entries in database.
            if (Directory.Exists(DbFolderName))
            {
                this.Delete_Artefacts_Details_In_Database(DbNId);
            }

            //Delete data folder
            if (Global.DeleteDirectory(DbFolderName, "true"))
            {
                //save xml file
                File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
                defaultDb = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "[@" + Constants.XmlFile.Db.Tags.RootAttributes.Default.ToString() + "]");
                if (defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default.ToString()].Value.ToString() == DbNId)
                {
                    defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default.ToString()].Value = "";
                    if (defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD.ToString()] != null)
                    {
                        defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD.ToString()].Value = "";
                    }

                }
                else
                {
                    if (defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD] != null)
                    {
                        if (defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value == DbNId)
                        {
                            defaultDb.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value = "";
                        }
                    }
                }
                XmlDoc.Save(DBConnectionsFile);
                RetVal = "true";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Register database in db.xml file
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminRegisterDatabase(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string CategoryName = string.Empty;
        string ConnName = string.Empty;
        string DbConn = string.Empty;
        string DefArea = string.Empty;
        string Description = string.Empty;
        string IsDefDb = string.Empty;
        string IsRegCatalog = string.Empty;

        XmlElement NewNode;
        int NewId = 0;
        string[] DBConnArr;
        DIConnection ObjDIConnection = null;
        string ServerType = string.Empty;
        string ServerName = string.Empty;
        string DbName = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;
        string QryStr = string.Empty;
        DataTable DTCounts;
        string CountStr = string.Empty;
        XmlElement NewCategoryNode;
        XmlNodeList ObjXmlNodeList;
        int CategoryId = 0;
        string DbConnWithEncryptPassword = string.Empty;
        string DefAreaJSon = string.Empty;
        string DefAreaCount = "0";
        string[] DefAreaStr;
        string[] DefIndStr;
        string DefInd = string.Empty;
        string DefIndJSon = string.Empty;
        string strDefaultLanguage = string.Empty;

        DBConverterDecorator objDBConverterDecorator = null;
        DBVersionBuilder VersionBuilder = null;
        string DbDefaultLanguage = string.Empty;
        string[] DbAvailableLanguage = null;
        string DbAvailableLanguageStr = string.Empty;
        string isGlobalAllow = ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow];
        try
        {
            requestParam = HttpUtility.UrlDecode(requestParam);
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            CategoryName = Params[0];
            ConnName = Params[1];
            DbConn = Params[2];
            if (Params.Length > 3)
            {
                DefArea = Params[3];
            }
            if (Params.Length > 4)
            {
                Description = Params[4];
            }
            if (Params.Length > 6)
            {
                IsRegCatalog = Params[6];
            }
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);
            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Name + "='" + ConnName + "']");
            if (xmlNode != null)
            {
                RetVal = "exists";
            }
            else
            {
                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + CategoryName + "']");
                if (xmlNode == null)
                {
                    NewCategoryNode = XmlDoc.CreateElement(Constants.XmlFile.Db.Tags.Category);
                    NewCategoryNode.SetAttribute(Constants.XmlFile.Db.Tags.CategoryAttributes.Name, CategoryName);
                    xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root).AppendChild(NewCategoryNode);
                }
                // Get old higher id                   
                ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + "child::node()");
                foreach (XmlNode data in ObjXmlNodeList)
                {
                    CategoryId = int.Parse(data.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value);
                    if (NewId < CategoryId)
                    {
                        NewId = CategoryId;
                    }
                }
                NewId++; // Increase 1 for new id
                // Split connection details in variables
                DBConnArr = Global.SplitString(DbConn, "||");
                ServerType = DBConnArr[0];
                ServerName = DBConnArr[1];
                DbName = DBConnArr[2];
                UserName = DBConnArr[3];
                if (DBConnArr.Length > 4)
                {
                    Password = DBConnArr[4];
                }
                ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(ServerType)), ServerName, "", DbName, UserName, Password);
                // Get default language for that db
                QryStr = "SELECT Language_Code FROM ut_language WHERE Language_Default = 1";

                DTCounts = ObjDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", ObjDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                if (DTCounts.Rows.Count == 1) strDefaultLanguage = DTCounts.Rows[0]["Language_Code"].ToString();
                else strDefaultLanguage = "en";

                // Get counts from db
                QryStr = "select DBMtd_AreaCnt, DBMtd_IndCnt, DBMtd_SrcCnt, DBMtd_DataCnt from ut_dbmetadata_" + strDefaultLanguage;
                DTCounts = ObjDIConnection.ExecuteDataTable(Regex.Replace(QryStr, "UT_", ObjDIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                DataRow Row = DTCounts.Rows[0];
                CountStr = string.Format("{0:0,0}", Row[0]) + "_" + string.Format("{0:0,0}", Row[1]) + "_" + string.Format("{0:0,0}", Row[2]) + "_" + string.Format("{0:0,0}", Row[3]);

                DbConnWithEncryptPassword = ServerType + "||" + ServerName + "||" + DbName + "||" + UserName;
                if (!string.IsNullOrEmpty(Password))
                {
                    DbConnWithEncryptPassword += "||" + Global.EncryptString(Password);
                }
                else
                {
                    DbConnWithEncryptPassword += "||";
                }

                //get default language code in database
                DbDefaultLanguage = ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault());

                //get all avalilable language code in database
                DbAvailableLanguage = Global.GetAllAvailableLanguageCode(ObjDIConnection);

                if (DbAvailableLanguage.Length > 0)
                {
                    DbAvailableLanguageStr = string.Join(",", DbAvailableLanguage);
                }
                if (Global.standalone_registry != "true")
                {
                    this.RunDBScripts(ObjDIConnection, NewId.ToString(), DbDefaultLanguage.Substring(1));

                    VersionBuilder = new DBVersionBuilder(ObjDIConnection, new DIQueries(ObjDIConnection.DIDataSetDefault(), ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault())));

                    VersionBuilder.InsertVersionInfo(Constants.DBVersion.DI7_0_0_0, Constants.DBVersion.VersionsChangedDatesDI7_0_0_0, Constants.DBVersion.VersionCommentsDI7_0_0_0);

                    #region -- Get default values of indicator, area and language  --

                    #region -- get default indicator and their Json data --

                    DefIndStr = GetDefaultIndicators(ObjDIConnection, strDefaultLanguage);
                    DefInd = DefIndStr[0];
                    DefIndJSon = DefIndStr[1];

                    #endregion

                    #region --get default area of level 1 and 2 (L1 + L2) by stored procedure with their json and counts --

                    DefAreaStr = GetDefaultAreas(ObjDIConnection, strDefaultLanguage);
                    DefArea = DefAreaStr[0];
                    DefAreaJSon = DefAreaStr[1];
                    DefAreaCount = DefAreaStr[2];

                    #endregion

                    #endregion

                    #region "Catalog"

                    if (IsRegCatalog == "true")
                    {
                        // Set description filed to update in service database 
                        this.DataBaseDescription = Description.Replace("'", "''").Replace("\n", "<br />").Replace(@"""", @"\""");
                        if (InsertIntoCatalog(ObjDIConnection, Row, DbAvailableLanguageStr))
                        {
                            GetAndUpdateIndexedAreas(DbAvailableLanguage, ObjDIConnection);
                            GetAndUpdateIndexedIndicators(DbAvailableLanguage, ObjDIConnection);
                        }
                    }

                    #endregion "Catalog"
                }
                #region -- Set xml tag attribute values and save it in db.xml file  --

                //Create new element node and set its attributes
                NewNode = XmlDoc.CreateElement(Constants.XmlFile.Db.Tags.Database);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Id, NewId.ToString());
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Name, ConnName);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb, "false");
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.Count, CountStr);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage, DbDefaultLanguage.Substring(1));
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator, DefInd);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON, DefIndJSon);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea, DefArea);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON, DefAreaJSon);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount, DefAreaCount);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection, DbConnWithEncryptPassword);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.AvailableLanguage, DbAvailableLanguageStr);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified, string.Format("{0:yyyy-MM-dd}", DateTime.Today.Date));
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.LanguageCodeCSVFiles, string.Empty);
                NewNode.SetAttribute(Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated, "false");

                if (DbAvailableLanguage != null)
                {
                    foreach (string LanguageCode in DbAvailableLanguage)
                    {
                        NewNode.SetAttribute("desc_" + LanguageCode, Description);
                    }
                }

                xmlNode.AppendChild(NewNode);

                XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root).Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value = NewId.ToString();

                //Save xml file
                File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
                XmlDoc.Save(DBConnectionsFile);

                #endregion
                GenerateAllPagesXML();
                DeleteSitemapFiles();
                RetVal = NewId.ToString();
            }
        }
        catch (Exception ex)
        {
            Global.WriteErrorsInLogFolder("error in Registering new database");
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (ObjDIConnection != null)
            {
                ObjDIConnection.Dispose();
                ObjDIConnection = null;
            }
        }
        return RetVal;
    }

    /// <summary>
    /// Get required database details for editing
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminGetAllDbConnections(string requestParam)
    {
        string RetVal = string.Empty;
        string CategoryName = string.Empty;

        string DBFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        DataTable DtDatabase;
        HTMLTableGenerator TableGenerator;
        int i = 0;
        string DefDbNId = string.Empty;
        string ConnStr = string.Empty;
        string DbType = string.Empty;
        string DbTypeStr = string.Empty;
        //  string DefalutStr = string.Empty;

        try
        {
            CategoryName = requestParam;

            DefDbNId = Global.GetDefaultDbNId();

            DtDatabase = new DataTable();
            DtDatabase.Columns.Add("DbId");
            DtDatabase.Columns.Add("Connection Name");
            DtDatabase.Columns.Add("Database NId");
            DtDatabase.Columns.Add("Database Type");
            DtDatabase.Columns.Add("Created On");
            //    DtDatabase.Columns.Add("Default");

            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + CategoryName + "']");

            for (i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                if (xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value.ToLower() == "false")
                {
                    DataRow Row;
                    Row = DtDatabase.NewRow();
                    Row[0] = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                    Row[1] = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value;
                    Row[2] = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;

                    ConnStr = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value;

                    if (!string.IsNullOrEmpty(ConnStr))
                    {
                        DbType = Global.SplitString(ConnStr, "||")[0];
                    }
                    if (DbType == "0")
                    {
                        DbTypeStr = "Sql Server";
                    }
                    else if (DbType == "3")
                    {
                        DbTypeStr = "My Sql";
                    }
                    else if (DbType == "8")
                    {
                        DbTypeStr = "Firebird";
                    }
                    else
                    {
                        DbTypeStr = "";
                    }

                    Row[3] = DbTypeStr;
                    Row[4] = xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value;
                    DtDatabase.Rows.Add(Row);
                }
            }

            TableGenerator = new HTMLTableGenerator();
            TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
            RetVal = TableGenerator.GetTableHmtl(DtDatabase, "DbId", "db", true);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("Getting all database connection");
            //Global.WriteErrorsInLog(ex.StackTrace);
            //Global.WriteErrorsInLog(ex.Message);
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Generate all codelists xml files for new registered database 
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminXMLGeneration(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        int DbNid = -1;
        string SelectedDataTypes = string.Empty;
        string AreaOrderBy = string.Empty;
        string QuickSelectionType = string.Empty;
        DIConnection ObjDIConnection = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNid = int.Parse(Params[0]);
            SelectedDataTypes = Params[1];

            if (Params.Length > 2)
            {
                AreaOrderBy = Params[2];
            }

            if (Params.Length > 3)
            {
                QuickSelectionType = Params[3];
            }

            ObjDIConnection = Global.GetDbConnection(DbNid);

            //Generate all codelists xml files for new registered database
            RetVal = GenerateAllCodelists(ObjDIConnection, DbNid.ToString(), SelectedDataTypes, AreaOrderBy, QuickSelectionType);
        }

        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace + "--" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Generate all Map files
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminMapFilesGeneration(string requestParam)
    {
        string RetVal = string.Empty;
        int DbNid = -1;
        DIConnection ObjDIConnection = null;
        DIQueries DIQueries;
        string MapFilesFolder = string.Empty;

        try
        {
            DbNid = int.Parse(requestParam);
            ObjDIConnection = Global.GetDbConnection(DbNid);
            DIQueries = new DIQueries(ObjDIConnection.DIDataSetDefault(), ObjDIConnection.DILanguageCodeDefault(ObjDIConnection.DIDataSetDefault()));

            MapFilesFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\" + Constants.FolderName.Maps);

            if (!Directory.Exists(MapFilesFolder))
            {
                Directory.CreateDirectory(MapFilesFolder);
            }

            Map.ExtractAllShapeFiles(ObjDIConnection, DIQueries, MapFilesFolder);

            RetVal = "true";
        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GenerateCacheResults(string requestParam)
    {
        string[] Params;
        string RetVal = string.Empty;
        string Additionalparams = string.Empty;
        int DbNid = -1;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            //DbNid = int.Parse(requestParam);
            DbNid = int.Parse(Params[0]);
            Additionalparams = (Params[1]).ToString();
            string AppSettingFile = string.Empty;
            XmlDocument XmlDoc;
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.customParams, Additionalparams);

            XmlDoc.Save(AppSettingFile);
            //lang = param[1];
            DIConnection ObjDIConnection;
            ObjDIConnection = Global.GetDbConnection(DbNid);
            //Update Most recent Data
            UpdateMRDTable(ObjDIConnection);

            foreach (string lang in getAllDbLangCodes(DbNid))
            {
                CreateCacheResults(lang, DbNid, this.Page, Additionalparams);
            }

            RetVal = "true";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Run dbscript in database
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminDBScriptGeneration(string requestParam)
    {

        string RetVal = string.Empty;
        int DbNid = -1;
        DIConnection ObjDIConnection = null;

        try
        {

            DbNid = int.Parse(requestParam);
            ObjDIConnection = Global.GetDbConnection(DbNid);

            if (this.RunDBScripts(ObjDIConnection, "", ""))
            {
                RetVal = "true";
            }
        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Save default area for created new database
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminSaveDefaultArea(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        int DbNid = -1;
        string DefArea = string.Empty;
        string DefAreaJson = string.Empty;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        int DefAreaCount = 0;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DIConnection ObjDIConnection = null;
        DataTable dtAreaNames;

        //Variables for creating CSVLogfile 
        string XLSFileMsg = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNid = int.Parse(Params[0]);
            DefArea = Params[1];
            DefAreaJson = Params[2];

            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNid.ToString() + "]");


            //Fetch default area cound from database
            ObjDIConnection = this.GetDbConnection(DbNid);

            if (ObjDIConnection != null)
            {
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "AreaNids";
                Param1.DbType = DbType.String;
                Param1.Value = DefArea;
                DbParams.Add(Param1);

                dtAreaNames = ObjDIConnection.ExecuteDataTable("sp_get_AreaNames_en", CommandType.StoredProcedure, DbParams);

                DefAreaCount = dtAreaNames.Rows.Count;
            }

            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea].Value = DefArea;
            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON].Value = DefAreaJson;
            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount].Value = DefAreaCount.ToString();

            File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
            XmlDoc.Save(DBConnectionsFile);

            #region "Call method to write log in XLS file"
            XLSFileMsg = Constants.CSVLogMessage.DefaultAreaUpdated;
            WriteLogInXLSFile(Constants.AdminModules.DatabaseSettings.ToString(), XLSFileMsg);
            #endregion

            RetVal = "true";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Save default indicator for created new database
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminSaveDefaultIndcator(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        int DbNid = -1;
        string DefIndicator = string.Empty;
        string DefIndicatorJson = string.Empty;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNid = int.Parse(Params[0]);
            DefIndicator = Params[1];
            DefIndicatorJson = Params[2];

            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNid.ToString() + "]");

            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator].Value = DefIndicator;
            xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON].Value = DefIndicatorJson;

            File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
            XmlDoc.Save(DBConnectionsFile);

            #region "Call method to write log in XLS file"

            XLSFileMsg = Constants.CSVLogMessage.DefaultIndicatorUpdated;
            WriteLogInXLSFile(Constants.AdminModules.DatabaseSettings.ToString(), XLSFileMsg);
            #endregion

            RetVal = "true";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion

    #region "--Language Settings--"

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminAddNewLanguage(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string LanguageName = string.Empty;
        string LanguageCode = string.Empty;
        string LanguageDirection = string.Empty;
        string LngNCodeName = string.Empty;

        string TargetLngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        XmlElement NewDestNode;
        XmlElement NewNode;

        string SrcSlidersDirPath = string.Empty;
        string NewSlidersDirPath = string.Empty;
        string SrcEmailDirPath = string.Empty;
        string NewEmailDirPath = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            LanguageName = Params[0];
            LanguageCode = Params[1];
            LanguageDirection = Params[2];

            LngNCodeName = LanguageName + " [" + LanguageCode + "]";

            //--Begin Save node in Destination_Language.xml file

            TargetLngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DestinationLanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(TargetLngFile);

            if (XmlDoc.SelectSingleNode("/" + Constants.XmlFile.DestinationLanguage.Tags.Root + "/" + Constants.XmlFile.DestinationLanguage.Tags.Destination + "/" + Constants.XmlFile.DestinationLanguage.Tags.LanguageName + "[text()='" + LngNCodeName + "']") == null)
            {

                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.DestinationLanguage.Tags.Root);

                //Create new destination node 
                NewDestNode = XmlDoc.CreateElement(Constants.XmlFile.DestinationLanguage.Tags.Destination);

                NewNode = XmlDoc.CreateElement(Constants.XmlFile.DestinationLanguage.Tags.LanguageName);
                NewNode.InnerText = LngNCodeName;
                NewDestNode.AppendChild(NewNode);

                NewNode = XmlDoc.CreateElement(Constants.XmlFile.DestinationLanguage.Tags.LanguageCode);
                NewNode.InnerText = LanguageCode;
                NewDestNode.AppendChild(NewNode);

                NewNode = XmlDoc.CreateElement(Constants.XmlFile.DestinationLanguage.Tags.PageDirection);
                NewNode.InnerText = LanguageDirection;
                NewDestNode.AppendChild(NewNode);

                NewNode = XmlDoc.CreateElement(Constants.XmlFile.DestinationLanguage.Tags.FileVersion);
                NewNode.InnerText = "1.0";
                NewDestNode.AppendChild(NewNode);

                xmlNode.AppendChild(NewDestNode);

                //Save xml file
                File.SetAttributes(TargetLngFile, FileAttributes.Normal);
                XmlDoc.Save(TargetLngFile);
            }

            //--End Save node in Destination_Language.xml file



            //--Begin Save node in Language.xml file

            TargetLngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(TargetLngFile);

            if (XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Name + "='" + LngNCodeName + "']") != null)
            {
                RetVal = "exists";
            }
            else
            {
                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root);

                NewNode = XmlDoc.CreateElement(Constants.XmlFile.Language.Tags.Language);
                NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.Code, LanguageCode);
                NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.Name, LngNCodeName);
                NewNode.SetAttribute(Constants.XmlFile.Language.Tags.LanguageAttributes.RTLDirection, (LanguageDirection == "rtl" ? "T" : "F"));
                xmlNode.AppendChild(NewNode);

                //Save xml file
                File.SetAttributes(TargetLngFile, FileAttributes.Normal);
                XmlDoc.Save(TargetLngFile);


                //Create slide html pages for new language
                SrcSlidersDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, "en"));
                NewSlidersDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, LanguageCode));
                Global.CopyDirectoryFiles(SrcSlidersDirPath, NewSlidersDirPath);
                //Create Email template pages for new language
                SrcEmailDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.EmailTemplates, "en"));
                NewEmailDirPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.EmailTemplates, LanguageCode));
                Global.CopyDirectoryFiles(SrcEmailDirPath, NewEmailDirPath);
                //Create target language xml file
                RetVal = CreateNewTargetLngFile(LngNCodeName, string.Empty);
            }

            //--End Save node in Language.xml file            
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Get language list
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminGetAllLanguageList()
    {
        string RetVal = string.Empty;

        string LanguageFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList LngCodeList;
        DataTable DtLanguage;
        HTMLTableGenerator TableGenerator;

        try
        {
            DtLanguage = new DataTable();
            DtLanguage.Columns.Add("LngId");
            DtLanguage.Columns.Add("Language Name");
            DtLanguage.Columns.Add("Language Code");
            DtLanguage.Columns.Add("Page Direction");

            LanguageFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LanguageFile);

            LngCodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Language.Tags.Root + "/child::node()");

            foreach (XmlNode xNode in LngCodeList)
            {
                DataRow Row;
                Row = DtLanguage.NewRow();
                Row[0] = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Code].Value;
                Row[1] = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value;
                Row[2] = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Code].Value;
                Row[3] = xNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.RTLDirection].Value == "T" ? "rtl" : "ltr";

                DtLanguage.Rows.Add(Row);
            }

            TableGenerator = new HTMLTableGenerator();
            TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
            RetVal = TableGenerator.GetTableHmtl(DtLanguage, "LngId", "Lng", true);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Delete language
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string AdminDeleteLanguage(string requestParam)
    {
        string RetVal = "false";
        string[] Params;
        string LanguageCode = string.Empty;
        string LanguageName = string.Empty;
        string LanguageFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode = null;
        string LngFolderName = string.Empty;
        string LngFileForDelete = string.Empty;
        string SliderPathForDelete = string.Empty;

        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            LanguageCode = Params[0];
            LanguageName = Params[1];

            LanguageFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LanguageFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + LanguageCode + "']");

            xmlNode.ParentNode.RemoveChild(xmlNode);


            //Get Data folder name
            LngFolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language + LanguageCode);

            //Delete data folder
            if (Global.DeleteDirectory(LngFolderName))
            {
                //Delete language file from MasterKeyVals
                LngFileForDelete = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals + LanguageName + ".xml");
                if (File.Exists(LngFileForDelete))
                {
                    File.Delete(LngFileForDelete);
                }

                //Slider html path
                SliderPathForDelete = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Path.Combine(Constants.FolderName.AdaptationSliderHTML, LanguageCode));
                Global.DeleteDirectory(SliderPathForDelete);

                //save xml file
                File.SetAttributes(LanguageFile, FileAttributes.Normal);
                XmlDoc.Save(LanguageFile);

                RetVal = "true";
            }
            #region "Call method to write log in XLS file"
            if (RetVal == "true")
            {
                XLSFileMsg = string.Format(Constants.CSVLogMessage.LanguageDeleted, LanguageName);
                WriteLogInXLSFile(Constants.AdminModules.LanguageSettings.ToString(), XLSFileMsg);
            }
            #endregion
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Generate language based XML files for all pages
    /// </summary>
    /// <returns></returns>
    public string GenerateAllPagesXML(bool IsWriteLogForPatchInst = false)
    {
        string RetVal = string.Empty;
        string MasterKeyValsDir = string.Empty;
        string PageKeysMappingsDir = string.Empty;
        string LanguageCode = string.Empty;
        string PageLanguageDir = string.Empty;
        string FullClientXML = string.Empty;
        //Variables for creating XLS Logfile 
        DataTable AllDbLanguageCodes = null;
        string XLSFileMsg = string.Empty;
        bool IsLanguageExist = false;
        string LangFileNameForLog = string.Empty;
        try
        {
            MasterKeyValsDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            PageKeysMappingsDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.PageKeysMappings);

            AllDbLanguageCodes = Global.GetAllDBLangaugeCodes();

            //Check for available Language Codes in the Database
            //If not langauge find or in null case, dont perform any action further
            if (AllDbLanguageCodes != null)
            {
                foreach (string MasterKeyFileName in Directory.GetFiles(MasterKeyValsDir, "*.xml"))
                {
                    LangFileNameForLog = Path.GetFileName(MasterKeyFileName);
                    LanguageCode = Global.SplitString(Path.GetFileNameWithoutExtension(MasterKeyFileName), "[")[1].Substring(0, 2);
                    IsLanguageExist = AllDbLanguageCodes.AsEnumerable().Any(row => LanguageCode == row.Field<String>("Language_Code"));

                    //If language exist in database generate GenerateAllPagesXML
                    if (IsLanguageExist)
                    {
                        PageLanguageDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language + LanguageCode);

                        foreach (string PageKeyFileName in Directory.GetFiles(PageKeysMappingsDir, "*.xml"))
                        {
                            FullClientXML = Global.getClientLangXML(MasterKeyFileName, PageKeyFileName);

                            if (!Directory.Exists(PageLanguageDir))
                            {
                                Directory.CreateDirectory(PageLanguageDir);
                            }
                            if (File.Exists(Path.Combine(PageLanguageDir, Path.GetFileName(PageKeyFileName))))
                            {
                                File.Delete(Path.Combine(PageLanguageDir, Path.GetFileName(PageKeyFileName)));
                            }
                            File.WriteAllText(Path.Combine(PageLanguageDir, Path.GetFileName(PageKeyFileName)), FullClientXML);
                        }
                    }
                    //Write Success Log
                    if (IsWriteLogForPatchInst)
                    {
                        string LogMessage = string.Format(PatchInstaller.ReadKeysForPatch("GenPagXMlPassed").ToString(), LangFileNameForLog);
                        XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, PatchInstaller.ReadKeysForPatch("StatusPassed").ToString(), string.Empty);
                    }
                }
                #region "Call method to write log in XLS file"
                XLSFileMsg = Constants.CSVLogMessage.GeneratedPageXMLs;
                WriteLogInXLSFile(Constants.AdminModules.LanguageSettings.ToString(), XLSFileMsg);
                #endregion

                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Write Logic for Faliure
            if (IsWriteLogForPatchInst)
            {
                string LogMessage = string.Format(PatchInstaller.ReadKeysForPatch("GenPagXMlFailed").ToString(), LangFileNameForLog);
                XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, PatchInstaller.ReadKeysForPatch("StatusFail").ToString(), ex.Message.ToString());
            }
            RetVal = "false";
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminGetLanguageGridHTML(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string SourceLanguage = string.Empty;
        string TargetLanguage = string.Empty;
        string LanguageFilesPath = string.Empty;
        DataTable LanguageTbl = new DataTable();
        InterfaceStringTranslator interfaceStringTranslator = new InterfaceStringTranslator();
        string Available = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SourceLanguage = Params[0];
            TargetLanguage = Params[1];

            LanguageFilesPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            LanguageTbl = interfaceStringTranslator.GetTranslationTable(LanguageFilesPath, SourceLanguage, TargetLanguage);
            Available = LanguageTbl.Rows.Count.ToString();

            LanguageTbl.Columns[1].Caption = SourceLanguage + " Available(" + Available + ")";
            LanguageTbl.Columns[2].Caption = TargetLanguage + " Available(" + Available + ")";

            TranslationHTMLTableGenerator translationHTMLTableGenerator = new TranslationHTMLTableGenerator();
            RetVal = translationHTMLTableGenerator.GetTableHmtl(LanguageTbl, "Key", "Lng", true);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminSaveLanguageChanges(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string KeyValueStr = string.Empty;
        string TargetLanguageFileName = string.Empty;
        string Key = string.Empty;
        string TargetValue = string.Empty;
        string LanguageFilesPath = string.Empty;
        string TargetLanguageFilePath = string.Empty;
        XmlDocument xmlDoc = new XmlDocument();
        string[] KeyValueRowsArr;
        string[] KeyValueArr;

        //Variables for creating XLS Logfile 
        string SourceLanguageFileName = string.Empty;
        string XLSFileMsg = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            KeyValueStr = Params[0];
            TargetLanguageFileName = Params[1];
            SourceLanguageFileName = Params[2];

            LanguageFilesPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            TargetLanguageFilePath = Path.Combine(LanguageFilesPath, (TargetLanguageFileName + ".xml"));

            xmlDoc.Load(TargetLanguageFilePath);

            KeyValueRowsArr = KeyValueStr.Split(new String[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None);

            for (int i = 0; i < KeyValueRowsArr.Length; i++)
            {
                KeyValueArr = KeyValueRowsArr[i].Split(new String[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None);

                Key = KeyValueArr[0];
                TargetValue = KeyValueArr[1];

                this.SetNodeValue(xmlDoc.SelectNodes(Constants.LanguageKeyValueTag), Key, TargetValue);
            }

            xmlDoc.Save(TargetLanguageFilePath);

            RetVal = "true";

            #region "Call method to write log in XLS file"
            XLSFileMsg = string.Format(Constants.CSVLogMessage.EditLanguage, TargetLanguageFileName, SourceLanguageFileName);
            WriteLogInXLSFile(Constants.AdminModules.LanguageSettings.ToString(), XLSFileMsg);
            #endregion
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private void SetNodeValue(XmlNodeList xmlNodeList, string nodeKey, string nodeValue)
    {
        try
        {
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes[0].Value == nodeKey)
                {
                    xmlNode.Attributes[1].Value = nodeValue;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #endregion

    #endregion

    #region "--SDMX Registry Functionality--"

    #region "--SDMX-ML Generation--"

    public string AdminSDMXMLGeneration(string requestParam)
    {
        string RetVal = string.Empty;
        int DbNid = -1;

        try
        {
            DbNid = int.Parse(requestParam);
            RetVal = GenerateSDMXML(DbNid, Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GenerateSDMXML(int DbNid, string LoggedInUserNId)
    {
        string RetVal;
        DIConnection DIConnection;
        DIQueries DIQueries;
        string OutputFolder, AgencyId, HeaderFilePath;
        bool SDMXMLGenerated = false;
        XmlDocument query = new XmlDocument();
        Dictionary<string, string> DictQuery;
        int fileCount;
        List<string> GeneratedFiles;
        RetVal = "false";
        DIConnection = Global.GetDbConnection(DbNid);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\sdmx");
        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + LoggedInUserNId;
        this.Clean_SDMX_ML_Folder(DbNid);
        DictQuery = new Dictionary<string, string>();
        fileCount = 0;
        GeneratedFiles = new List<string>();
        HeaderFilePath = string.Empty;
        try
        {
            HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            XmlDocument UploadedHeaderXml = new XmlDocument();
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
            if (File.Exists(HeaderFilePath))
            {
                UploadedHeaderXml.Load(HeaderFilePath);
                UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                Header = UploadedDSDStructure.Header;
            }
            this.Add_All_IUS_To_Query_Dictionary(DictQuery, DIConnection, DIQueries);
            //Generation of SDMX-ML files
            query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, AgencyId, DIConnection, DIQueries);

            SDMXMLGenerated = SDMXUtility.Generate_Data(SDMXSchemaType.Two_One, query, DataFormats.StructureSpecificTS, DIConnection, DIQueries, Path.Combine(OutputFolder, "SDMX-ML"), out fileCount, out  GeneratedFiles, Header);
            if (SDMXMLGenerated == true)
            {
                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace);
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private void Clean_SDMX_ML_Folder(int DbNId)
    {
        string FolderName;
        string AppPhysicalPath, DbFolder, language;
        DIConnection DIConnection;
        DIQueries DIQueries;
        AppPhysicalPath = string.Empty;
        DbFolder = string.Empty;
        language = string.Empty;


        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML);
            Global.DeleteDirectory(FolderName);
            this.Create_Directory_If_Not_Exists(FolderName);

            DIConnection = Global.GetDbConnection(DbNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            foreach (DataRow LanguageRow in DIConnection.DILanguages(DIQueries.DataPrefix).Rows)
            {
                language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML + "\\" + language);
                this.Create_Directory_If_Not_Exists(FolderName);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;

        }
        finally
        {

        }
    }

    private void Add_All_IUS_To_Query_Dictionary(Dictionary<string, string> DictQuery, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtIUS;
        string IndicatorGId, UnitGId, SubgroupValGId;

        try
        {
            DtIUS = DIConnection.ExecuteDataTable(DIQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            foreach (DataRow DrIUS in DtIUS.Rows)
            {
                IndicatorGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString();
                UnitGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString();
                SubgroupValGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                if (!DictQuery.ContainsKey(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id))
                {
                    DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + SubgroupValGId);
                }
                else
                {
                    DictQuery[DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id] += "," + IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + SubgroupValGId;
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    #endregion "--SDMX-ML Generation--"

    #region "--SDMX-ML Registration--"

    public string AdminSDMXMLRegistration(string requestParam)
    {
        string RetVal;
        int DbNid = -1;

        try
        {
            DbNid = int.Parse(requestParam);

            RetVal = "false";

            this.Register_SDMXFiles(DbNid.ToString(), false, Session[Constants.SessionNames.LoggedAdminUserNId].ToString(), null, DbNid.ToString());

            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace);
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    //private void Register_SDMXFiles(string DbNId, bool DBOrDSDFlag, string LoggedInUserNId, List<string> ListIndicatorForRegistrations, List<string> FilesToRegister,string OriginalDBNId=null)
    //{
    //    string AgencyId = string.Empty;
    //    string AdminNId = string.Empty;
    //    string AppPhysicalPath = string.Empty;
    //    string DbFolder = string.Empty;
    //    string RegistrationId = string.Empty;
    //  //  List<string> SDMXMLFiles;
    //    string[] SDMXMLFiles;
    //    string SDMXMLFileName;
    //    string SDMXMLFileURL;
    //    string WebServiceURL = string.Empty;
    //    string WSDLURL = string.Empty;
    //    string DFDId = string.Empty;
    //    string language = string.Empty;
    //    int NoOfSDMXMLFiles, i;
    //    int index = 0;
    //    bool IsAdminUploadedDSD;
    //    //string[] SDMXMLFilename;
    //    IsAdminUploadedDSD = false;
    //    SDMXMLFiles = null;

    //    try
    //    {
    //        AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
    //        DbFolder = Constants.FolderName.Data + DbNId + "\\";
    //        AdminNId = LoggedInUserNId;
    //        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AdminNId;
    //        IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
    //       this.Clean_Registrations_And_Constraints_By_Admin(DbNId, AdminNId, true);//uncommented to clean old registrations

    //        DFDId = this.Get_DFDId_From_DBNId(DbNId);

    //        foreach (string LanguageFolder in Directory.GetDirectories(Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML)))
    //        {
    //            SDMXMLFiles = Directory.GetFiles(LanguageFolder);
    //            language = LanguageFolder.Substring(LanguageFolder.IndexOf("SDMX-ML" + Path.DirectorySeparatorChar) + ("SDMX-ML" + Path.DirectorySeparatorChar).Length);
    //            NoOfSDMXMLFiles = SDMXMLFiles.Length;

    //            for (i = 0; i < NoOfSDMXMLFiles; i++)
    //            {
    //                try
    //                {
    //                    index = SDMXMLFiles[i].Split('\\').Length - 1;
    //                    SDMXMLFileName = SDMXMLFiles[i].Split('\\')[index];
    //                    SDMXMLFileURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/SDMX-ML/" + language + '/' + SDMXMLFileName;

    //                    if (ListIndicatorForRegistrations != null)
    //                    {
    //                        if (ListIndicatorForRegistrations.Contains(this.Get_IndicatorGId_From_SDMXML(SDMXMLFiles[i], DBOrDSDFlag)))
    //                        {
    //                            this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + false +
    //                        Constants.Delimiters.ParamDelimiter + DFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + SDMXMLFileURL + Constants.Delimiters.ParamDelimiter + language + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + SDMXMLFileName);
    //                        }
    //                    }
    //                    else
    //                    {
    //                        this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + false +
    //                        Constants.Delimiters.ParamDelimiter + DFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
    //                        false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + SDMXMLFileURL + Constants.Delimiters.ParamDelimiter + language + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + SDMXMLFileName);
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    Global.CreateExceptionString(ex, null);
    //                }
    //            }
    //        }

    //        if (IsAdminUploadedDSD == false)
    //        {
    //            WebServiceURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?p=" +
    //                        DbNId.ToString();
    //            WSDLURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?WSDL";

    //            this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + false +
    //                    Constants.Delimiters.ParamDelimiter + DFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
    //                    false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + true + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + language + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter +string.Empty);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Global.CreateExceptionString(ex, null);
    //        throw ex;
    //    }
    //    finally
    //    {
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DbNId">DBOrDSDFlag = True -> DbNId- DeVInfo DBNId;  DBOrDSDFlag = False - DSD NId</param>
    /// <param name="DBOrDSDFlag">True = DI DB flow; False = DSD Flow</param>
    /// <param name="LoggedInUserNId"></param>
    /// <param name="ListIndicatorForRegistrations">Country Indicator GUID for which datafile has been generated</param>
    /// <param name="FilesToRegister"></param>
    /// <param name="GeneratedIndicatorCountryGIDS"></param>
    /// <param name="OriginalDBNId">DevInfo DBNId</param>
    private void Register_SDMXFiles(string DbNId, bool DBOrDSDFlag, string LoggedInUserNId, List<string> FilesToRegister, string OriginalDBNId = null)
    {
        string AgencyId = string.Empty;
        string AdminNId = string.Empty;
        string AppPhysicalPath = string.Empty;
        string DbFolder = string.Empty;
        string RegistrationId = string.Empty;
        List<string> SDMXMLFiles;
        string SDMXMLFileName;
        string SDMXMLFileURL;
        string WebServiceURL = string.Empty;
        string WSDLURL = string.Empty;
        string DFDId = string.Empty;
        string language = string.Empty;
        int NoOfSDMXMLFiles, i;
        int index = 0;
        bool IsAdminUploadedDSD;
        string[] SDMXMLFilename;
        //   List<string> SDMXMLFiles;
        IsAdminUploadedDSD = false;
        SDMXMLFiles = null;
        SDMXMLFilename = null;
        string UNSDGID = string.Empty;
        string[] GIDArray = null;
        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId + "\\";
            AdminNId = LoggedInUserNId;
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AdminNId;
            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
         
            // this.Clean_Registrations_And_Constraints_By_Admin(DbNId, AdminNId, true);//uncommented to clean old registrations

            DFDId = this.Get_DFDId_From_DBNId(DbNId);

            foreach (string LanguageFolder in Directory.GetDirectories(Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML)))
            {
               
                SDMXMLFiles = new List<string>();
                language = LanguageFolder.Substring(LanguageFolder.IndexOf("SDMX-ML" + Path.DirectorySeparatorChar) + ("SDMX-ML" + Path.DirectorySeparatorChar).Length);
                SDMXMLFilename = Directory.GetFiles(LanguageFolder);
              
                foreach (string files in FilesToRegister.Distinct())
                {
                    foreach (string FileFullPath in SDMXMLFilename)
                    {
                        if (FileFullPath.IndexOf(files) > -1)
                        {
                            SDMXMLFiles.Add(FileFullPath);
                            break;
                        }
                    }
                  
                }

            
                NoOfSDMXMLFiles = SDMXMLFiles.Count;

                for (i = 0; i < NoOfSDMXMLFiles; i++)
                {
                    try
                    {
                        index = SDMXMLFiles[i].Split('\\').Length - 1;
                        SDMXMLFileName = SDMXMLFiles[i].Split('\\')[index];
                        GIDArray = Global.SplitString(SDMXMLFileName, "_DI_");
                        UNSDGID = GIDArray[0];
                        SDMXMLFileURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/SDMX-ML/" + language + '/' + SDMXMLFileName;
                        this.Clean_Registrations_And_Constraints_By_Admin(DbNId, AdminNId, true, SDMXMLFileName, language);
                     
                        this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + false +
                            Constants.Delimiters.ParamDelimiter + DFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
                            false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false +
                            Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + SDMXMLFileURL + Constants.Delimiters.ParamDelimiter +
                            language + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + SDMXMLFileName);
                        
                    }
                    catch (Exception ex)
                    {
                        Global.CreateExceptionString(ex, null);
                      
                    }
                }
            }

            if (IsAdminUploadedDSD == false)
            {
                WebServiceURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?p=" +
                            DbNId.ToString();
                WSDLURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?WSDL";

                this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + false +
                        Constants.Delimiters.ParamDelimiter + DFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
                        false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + true + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + language + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + string.Empty);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
          
            throw ex;
        }
        finally
        {
        }
    }

    private string Get_DFDId_From_DBNId(string DbNId)
    {
        string RetVal = string.Empty;

        DataTable DtTable;
        DIConnection DIConnection;

        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
        DtTable = DIConnection.ExecuteDataTable("SELECT Id FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " +
                                                 Convert.ToInt32(ArtefactTypes.DFD).ToString() + ";");

        if (DtTable != null && DtTable.Rows.Count > 0)
        {
            RetVal = DtTable.Rows[0]["Id"].ToString();
        }

        return RetVal;
    }

    #endregion "--SDMX-ML Registration--"

    #region "--Metadata Generation--"

    public string AdminMetadataGeneration(string requestParam)
    {
        string RetVal = string.Empty;
        int DbNid = -1;
        List<string> GeneratedIndicatorMetadataFiles = new List<string>();
        List<string> GeneratedAreaMetadataFiles = new List<string>();
        List<string> GeneratedSourceMetadataFiles = new List<string>();
        try
        {
            DbNid = int.Parse(requestParam);
            RetVal = GenerateMetadata(DbNid, Session[Constants.SessionNames.LoggedAdminUserNId].ToString(), string.Empty, string.Empty, string.Empty, out GeneratedIndicatorMetadataFiles, out GeneratedAreaMetadataFiles, out GeneratedSourceMetadataFiles, null);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GenerateMetadata(int DbNid, string LoggedInUserNId, string IndicatorNIds, string AreaNIds, string SourceNIds, out List<string> GeneratedIndicatorMetadataFiles, out List<string> GeneratedAreaMetadataFiles, out List<string> GeneratedSourceMetadataFiles, SDMXObjectModel.Message.StructureHeaderType Header)
    {
        string RetVal;
        DIConnection DIConnection;
        DIQueries DIQueries;
        string OutputFolder, AgencyId;
        bool MetadataGenerated = false;

        RetVal = "false";
        DIConnection = Global.GetDbConnection(DbNid);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNid.ToString());
        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + LoggedInUserNId;
        //this.Clean_Metadata_Folder(DbNid);
        //    GeneratedMetadataFiles=new List<string>();
        GeneratedIndicatorMetadataFiles = new List<string>();
        GeneratedAreaMetadataFiles = new List<string>();
        GeneratedSourceMetadataFiles = new List<string>();
        try
        {
            MetadataGenerated = SDMXUtility.Generate_MetadataReport(SDMXSchemaType.Two_One, MetadataTypes.Area, AreaNIds, AgencyId, string.Empty, null, DIConnection, DIQueries, Path.Combine(OutputFolder, Constants.FolderName.SDMX.AreaMetadata), out GeneratedAreaMetadataFiles, Header);

            if (MetadataGenerated == true)
            {
                MetadataGenerated = SDMXUtility.Generate_MetadataReport(SDMXSchemaType.Two_One, MetadataTypes.Indicator, IndicatorNIds, AgencyId, string.Empty, null, DIConnection, DIQueries, Path.Combine(OutputFolder, Constants.FolderName.SDMX.IndicatorMetadata), out GeneratedIndicatorMetadataFiles, Header);

                if (MetadataGenerated == true)
                {
                    MetadataGenerated = SDMXUtility.Generate_MetadataReport(SDMXSchemaType.Two_One, MetadataTypes.Source, SourceNIds, AgencyId, string.Empty, null, DIConnection, DIQueries, Path.Combine(OutputFolder, Constants.FolderName.SDMX.SourceMetadata), out GeneratedSourceMetadataFiles, Header);

                    if (MetadataGenerated == true)
                    {
                        RetVal = "true";
                    }
                    else
                    {
                        RetVal = "false";
                    }
                }
                else
                {
                    RetVal = "false";
                }
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace);
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private void Clean_Metadata_Folder(int DbNId)
    {
        string FolderName;
        string AppPhysicalPath, DbFolder, language;
        AppPhysicalPath = string.Empty;
        DbFolder = string.Empty;
        language = string.Empty;


        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DbNId.ToString() + "\\";

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.AreaMetadata);
            Global.DeleteDirectory(FolderName);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.IndicatorMetadata);
            Global.DeleteDirectory(FolderName);
            this.Create_Directory_If_Not_Exists(FolderName);

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SourceMetadata);
            Global.DeleteDirectory(FolderName);
            this.Create_Directory_If_Not_Exists(FolderName);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {

        }
    }

    #endregion "--Metadata Generation--"

    #region "--Metadata Registration--"

    public string AdminMetadataRegistration(string requestParam)
    {
        string RetVal;
        int DbNid = -1;

        try
        {
            DbNid = int.Parse(requestParam);

            RetVal = "false";

            this.Register_MetadataReport(DbNid.ToString(), Session[Constants.SessionNames.LoggedAdminUserNId].ToString(), null, null, null, null, null, null, null, DbNid.ToString());

            RetVal = "true";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private void Register_MetadataReport(string DbNId, string LoggedInUserNId, List<string> ListIndicatorForRegistrations, List<string> ListAreaForRegistrations, List<string> ListSourceForRegistrations, List<string> GeneratedMetadataFiles, List<string> GeneratedIndicatorMetadataFiles, List<string> GeneratedAreaMetadataFiles, List<string> GeneratedSourceMetadataFiles, string OriginalDBNId = null)
    {
        string AdminNId, FolderName, FolderURL;
        bool IsAdminUploadedDSD;

        AdminNId = string.Empty;
        FolderName = string.Empty;
        FolderURL = string.Empty;
        IsAdminUploadedDSD = false;

        try
        {
            AdminNId = LoggedInUserNId;
            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));

            //  this.Clean_Registrations_And_Constraints_By_Admin(DbNId, AdminNId, false);

            if (IsAdminUploadedDSD == false)
            {
                FolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" +
                             Constants.FolderName.SDMX.AreaMetadata);
                FolderURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/Metadata/Area/";
                this.Register_MetadataReport_Specific_Type(DbNId, AdminNId, FolderName, FolderURL, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Id, IsAdminUploadedDSD, ListAreaForRegistrations, GeneratedAreaMetadataFiles, OriginalDBNId);

                FolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" +
                             Constants.FolderName.SDMX.IndicatorMetadata);
                FolderURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/Metadata/Indicator/";
                this.Register_MetadataReport_Specific_Type(DbNId, AdminNId, FolderName, FolderURL, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Indicator.Id, IsAdminUploadedDSD, ListIndicatorForRegistrations, GeneratedIndicatorMetadataFiles, OriginalDBNId);

                FolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" +
                             Constants.FolderName.SDMX.SourceMetadata);
                FolderURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/Metadata/Source/";
                this.Register_MetadataReport_Specific_Type(DbNId, AdminNId, FolderName, FolderURL, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Source.Id, IsAdminUploadedDSD, ListSourceForRegistrations, GeneratedSourceMetadataFiles, OriginalDBNId);
            }
            else
            {
                FolderName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" +
                             Constants.FolderName.SDMX.Metadata);
                FolderURL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, HttpContext.Current.Request.Url.AbsoluteUri.IndexOf("/libraries/")) + "/stock/data/" + DbNId + "/sdmx/Metadata/";

                foreach (string MetadataFolderName in Directory.GetDirectories(FolderName))
                {
                    FolderURL += MetadataFolderName.Substring(MetadataFolderName.IndexOf("\\sdmx\\Metadata\\") + "\\sdmx\\Metadata\\".Length) + "/";
                    this.Register_MetadataReport_Specific_Type(DbNId, AdminNId, MetadataFolderName, FolderURL,
                         MetadataFolderName.Substring(MetadataFolderName.IndexOf("\\sdmx\\Metadata\\") + "\\sdmx\\Metadata\\".Length), IsAdminUploadedDSD, ListIndicatorForRegistrations, GeneratedMetadataFiles, OriginalDBNId);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }
    }

    //private void Register_MetadataReport_Specific_Type(string DbNId, string AdminNId, string FolderName, string FolderURL, string MFDId, bool IsAdminUploadedDSD, List<string> ListFilter, List<string> GeneratedMetadataFiles,string  OriginalDBNId)
    //{
    //    string AgencyId, MetadataFileName, MetadataFileUrl, RegistrationId, WebServiceURL, WSDLURL;

    //    AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AdminNId;
    //    MetadataFileName = string.Empty;
    //    MetadataFileUrl = string.Empty;
    //    RegistrationId = string.Empty;
    //    WebServiceURL = string.Empty;
    //    WSDLURL = string.Empty;

    //        foreach (string FileNameWpath in Directory.GetFiles(FolderName))
    //        {
    //              MetadataFileName = Path.GetFileName(FileNameWpath);
    //              MetadataFileUrl = FolderURL + MetadataFileName;
    //               if (ListFilter != null)
    //                {
    //                    foreach (string item in ListFilter)
    //                    {
    //                        MetadataFileName = MetadataFileName.Replace(".xml", string.Empty);
    //                        if (MetadataFileName.Contains(item))//ListFilter.Contains(MetadataFileName.Replace(".xml", string.Empty))
    //                        {

    //                            this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
    //                                        Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
    //                                        false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + MetadataFileUrl + Constants.Delimiters.ParamDelimiter + DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage+ Constants.Delimiters.ParamDelimiter +OriginalDBNId);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
    //                                Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
    //                                false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + MetadataFileUrl + Constants.Delimiters.ParamDelimiter + DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage + Constants.Delimiters.ParamDelimiter + OriginalDBNId);
    //                }

    //        }

    //    if (IsAdminUploadedDSD == false)
    //    {
    //        WebServiceURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?p=" +
    //                        DbNId.ToString();
    //        WSDLURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?WSDL";

    //        this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
    //                    Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
    //                    false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + true + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage + Constants.Delimiters.ParamDelimiter + OriginalDBNId);
    //    }
    //}



    private void Register_MetadataReport_Specific_Type(string DbNId, string AdminNId, string FolderName, string FolderURL, string MFDId, bool IsAdminUploadedDSD, List<string> ListFilter, List<string> GeneratedMetadataFiles, string OriginalDBNId)
    {
        string AgencyId, MetadataFileName, MetadataFileUrl, RegistrationId, WebServiceURL, WSDLURL;

        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AdminNId;
        MetadataFileName = string.Empty;
        MetadataFileUrl = string.Empty;
        RegistrationId = string.Empty;
        WebServiceURL = string.Empty;
        WSDLURL = string.Empty;

        //Get Database Language
        string LanguageCode = string.Empty;
        DataTable DTLanguage = null;
        string RegionalLanguage = string.Empty;
        //Get Posted Data - will be passed to the Javascript

        LanguageCode = string.Empty;

        DTLanguage = Global.GetAllDBLangaugeCodes();
        if (DTLanguage != null)
        {
            foreach (DataRow dr in DTLanguage.Rows)
            {
                if (dr["Language_Default"].ToString() == "True")
                {
                    LanguageCode = dr["Language_Code"].ToString();
                    break;
                }
            }
        }



        foreach (string files in GeneratedMetadataFiles)
        {
            foreach (string FileNameWpath in Directory.GetFiles(FolderName))
            {
                if (FileNameWpath.IndexOf(files) > -1)
                {
                    MetadataFileName = Path.GetFileName(FileNameWpath);
                    MetadataFileUrl = FolderURL + MetadataFileName;

                    MetadataFileName = MetadataFileName.Replace(".xml", string.Empty);
                    this.Clean_Registrations_And_Constraints_By_Admin(DbNId, AdminNId, false, MetadataFileName, string.Empty);
                    //MetadataFileName = Path.GetFileName(FileNameWpath);
                    //MetadataFileUrl = FolderURL + MetadataFileName;

                    if (ListFilter != null)
                    {
                        foreach (string item in ListFilter)
                        {
                            MetadataFileName = MetadataFileName.Replace(".xml", string.Empty);
                            if (MetadataFileName.Contains(item + "_DIMD_"))//ListFilter.Contains(MetadataFileName.Replace(".xml", string.Empty))
                            {

                                this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
                                            Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
                                            false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + MetadataFileUrl + Constants.Delimiters.ParamDelimiter + LanguageCode + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + MetadataFileName);//DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage 
                            }
                        }
                    }
                    else
                    {
                        this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
                                    Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
                                    false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + false + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + MetadataFileUrl + Constants.Delimiters.ParamDelimiter + LanguageCode + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + MetadataFileName);
                    }
                }
            }
        }
        if (IsAdminUploadedDSD == false)
        {
            WebServiceURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?p=" +
                            DbNId.ToString();
            WSDLURL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("/aspx/")) + "/ws/RegistryService.asmx?WSDL";

            this.AddRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + true +
                        Constants.Delimiters.ParamDelimiter + MFDId + Constants.Delimiters.ParamDelimiter + WebServiceURL + Constants.Delimiters.ParamDelimiter +
                        false + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + true + Constants.Delimiters.ParamDelimiter + WSDLURL + Constants.Delimiters.ParamDelimiter + string.Empty + Constants.Delimiters.ParamDelimiter + LanguageCode + Constants.Delimiters.ParamDelimiter + OriginalDBNId + Constants.Delimiters.ParamDelimiter + string.Empty);
        }
    }

    #endregion "--Metadata Registration--"

    #region "--Common--"

    private void Delete_Artefacts_Details_In_Database(string DbNId)
    {
        string DeleteQuery;
        DIConnection DIConnection;

        DeleteQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DeleteQuery = "DELETE FROM Artefacts WHERE DBNId = " + DbNId + ";";
            DIConnection.ExecuteDataTable(DeleteQuery);
        }
        catch (Exception ex)
        {
            //throw ex;
            Global.CreateExceptionString(ex, null);
        }
    }

    private void Clean_Registrations_And_Constraints_By_Admin(string DbNId, string AdminNId, bool DataMetadataFlag)
    {
        DIConnection DIConnection;
        DataTable DtTable;
        string DFDMFDId;

        DIConnection = null;
        DtTable = null;
        DFDMFDId = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ";");
            foreach (DataRow DrTable in DtTable.Rows)
            {
                if (Path.GetDirectoryName(DrTable["FileLocation"].ToString()).EndsWith(Constants.FolderName.SDMX.Registrations + AdminNId))
                {
                    DFDMFDId = this.Get_DFDMFDId(DbNId, AdminNId, DrTable["Id"].ToString());

                    if (DataMetadataFlag == true)
                    {
                        if (DFDMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                        {
                            this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
                        }
                    }
                    else
                    {
                        if (DFDMFDId != DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                        {
                            this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {

        }
    }


    private void Clean_Registrations_And_Constraints_By_Admin(string DbNId, string AdminNId, bool DataMetadataFlag, string PublishedFileName, string Language)
    {
        DIConnection DIConnection;
        DataTable DtTable;
        string DFDMFDId;
        string languageId = string.Empty;
        string languageCode = string.Empty;
        DIConnection = null;
        DtTable = null;
        DFDMFDId = string.Empty;

        try
        {
            Global.ExistenceofColumnAccessDbSchema();

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            languageId = Global.GetLangNidFromlangCode(Language);
           
            if (DataMetadataFlag == true)
            {
                DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + DbNId + "  AND LangPrefNId=" + languageId + " AND PublishedFileName='" + PublishedFileName + "' AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ";");
            }
            else
            {
                DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + DbNId + "  AND PublishedFileName='" + PublishedFileName + "' AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ";");
            }
            foreach (DataRow DrTable in DtTable.Rows)
            {
                if (Path.GetDirectoryName(DrTable["FileLocation"].ToString()).EndsWith(Constants.FolderName.SDMX.Registrations + AdminNId))
                {
                    DFDMFDId = this.Get_DFDMFDId(DbNId, AdminNId, DrTable["Id"].ToString());

                    if (DataMetadataFlag == true)
                    {
                        if (DFDMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                        {
                            this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
                            XLSLogGenerator.WriteCSVLogForMailStatus("Clean_Registrations_And_Constraints_By_Admin", Convert.ToString(PublishedFileName), "register", Convert.ToString(PublishedFileName));
                        }
                    }
                    else
                    {
                        if (DFDMFDId != DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                        {
                            this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
                        }
                    }
                }

            }

            //else
            //{
            //    foreach (DataRow DrTable in DtTable.Rows)
            //    {
            //        languageId = Convert.ToString(DrTable["LangPrefNid"].ToString());
            //        languageCode = Global.GetLangCodeFromDB(languageId);
            //        if (languageCode == Language)
            //        {
            //            if (Path.GetDirectoryName(DrTable["FileLocation"].ToString()).EndsWith(Constants.FolderName.SDMX.Registrations + AdminNId))
            //            {
            //                DFDMFDId = this.Get_DFDMFDId(DbNId, AdminNId, DrTable["Id"].ToString());

            //                if (DataMetadataFlag == true)
            //                {
            //                    if (DFDMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
            //                    {
            //                        this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
            //                    }
            //                }
            //                else
            //                {
            //                    if (DFDMFDId != DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
            //                    {
            //                        this.DeleteRegistration(DbNId + Constants.Delimiters.ParamDelimiter + AdminNId + Constants.Delimiters.ParamDelimiter + DrTable["Id"].ToString() + Constants.Delimiters.ParamDelimiter + DFDMFDId);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception ex)
        {
            XLSLogGenerator.WriteCSVLogForMailStatus("EX-CLEAN REGISTREregister", Convert.ToString(ex), "register", Convert.ToString(ex));
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {

        }
    }

    private string Get_DFDMFDId(string DbNId, string AdminNId, string RegistrationId)
    {
        string RetVal;
        string FileNameWPath;
        RegistryInterfaceType RegistryInterfaceRequest;

        RetVal = string.Empty;
        FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Registrations\\" + AdminNId + "\\" + RegistrationId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
        RegistryInterfaceRequest = null;

        try
        {
            RegistryInterfaceRequest = (RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(RegistryInterfaceType), FileNameWPath);
            RetVal = ((ProvisionAgreementRefType)((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items[0]).id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + AdminNId + DevInfo.Lib.DI_LibSDMX.Constants.Underscore, string.Empty);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion "--Common--"

    #endregion "--SDMX Registry Functionality--"

    #region "Adaptation User"

    public string GetAllAdaptations()
    {
        string RetVal = string.Empty;
        try
        {

            AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
            UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
            RetVal = UserLoginService.GetAllAdaptations();
        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    ///// <summary>
    ///// Get all users of current Adaptation
    ///// </summary>
    ///// <param name="requestParam"></param>
    ///// <returns></returns>
    //public string GetCurentAdaptationsUser(string requestParam)
    //{
    //    string RetVal = string.Empty;
    //    int AdapNId;
    //    string[] Params;
    //    string sortExp = "";
    //    HTMLTableGenerator TableGenerator;
    //    //string AdaptationURl = string.Empty;
    //    try
    //    {
    //        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
    //        AdapNId = int.Parse(Params[0]); // Not in use now
    //        int CurrentPage = int.Parse(Params[1]);
    //        if (Params.Length > 1)
    //        {
    //            sortExp = Params[3].ToString() + " " + Params[2].ToString();
    //        }
    //        // Get Adaptation URL
    //        //AdaptationURl = Global.GetAdaptationUrl();
    //        DataSet dsUsers = new DataSet();
    //        AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
    //        UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
    //        // Get ALL the users of current Adaptation by Adaptation URL
    //        dsUsers = UserLoginService.GetUsersByAdaptationURL(Global.GetAdaptationGUID());
    //        DataTable dtUsers = dsUsers.Tables[0].Clone();
    //        // Set Paging and creatwe data table
    //        for (int i = ((CurrentPage - 1) * Convert.ToInt32(Global.AdapUserPageSize)) + 1; i <= (CurrentPage * Convert.ToInt32(Global.AdapUserPageSize)); i++)
    //        {
    //            if (i <= dsUsers.Tables[0].Rows.Count)
    //            {
    //                DataRow newRow = dsUsers.Tables[0].Rows[i - 1];
    //                dtUsers.ImportRow(newRow);
    //            }
    //            else
    //            {
    //                break;
    //            }
    //        }
    //        // Sort Result
    //        if (sortExp != "")
    //        {
    //            dtUsers.DefaultView.Sort = sortExp;
    //        }
    //        TableGenerator = new HTMLTableGenerator();
    //        TableGenerator.ShowSorting = true;
    //        TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
    //        RetVal = TableGenerator.GetTableHmtl(dtUsers.DefaultView.ToTable(), "NId", "AdapUser", true) + Constants.Delimiters.ParamDelimiter + CalPageCount((dsUsers.Tables[0].Rows.Count), Convert.ToInt32(Global.AdapUserPageSize));

    //    }

    //    catch (Exception ex)
    //    {
    //        Global.CreateExceptionString(ex, null);
    //    }
    //    return RetVal;
    //}

    public string GetCurentAdaptationsUser(string requestParam)
    {
        //////string RetVal = string.Empty;
        //////string[] Params;
        //////string sortExp = "";
        //////HTMLTableGenerator TableGenerator;
        //////string SearchStr = string.Empty;
        //////DataView DefDataView = null;
        //////try
        //////{
        //////    Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        //////    int CurrentPage = int.Parse(Params[1]);
        //////    if (Params.Length > 1)
        //////    {
        //////        sortExp = Params[3].ToString() + " " + Params[2].ToString();
        //////        if (Params.Length > 4)
        //////        {
        //////            SearchStr = Params[4].ToString();
        //////        }
        //////    }
        //////    DataSet dsUsers = new DataSet();
        //////    AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
        //////    UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
        //////    // Get ALL the users of current Adaptation Guid
        //////    dsUsers = UserLoginService.GetUsersByAdaptationURL(Global.GetAdaptationGUID());
        //////    DataTable DTFullTable = dsUsers.Tables[0];
        //////    DataTable DTFullTableClone = null;
        //////    if (Convert.ToBoolean(Global.ShowWebmasterAccount) == false)
        //////    {
        //////        DTFullTableClone = dsUsers.Tables[0].Clone();
        //////        DataRow[] Drows = DTFullTable.Select("IsMasterAccount <> 'true'");
        //////        foreach (DataRow dr in Drows)
        //////        {
        //////            DTFullTableClone.ImportRow(dr);
        //////        }
        //////    }
        //////    else
        //////    {
        //////        DTFullTableClone = DTFullTable;
        //////    }
        //////    DTFullTableClone.Columns.Remove("IsMasterAccount");
        //////    //dsUsers.Tables[0].Columns.Remove("IsMasterAccount"); 
        //////    //dsUsers.Tables[0] = DTFullTable;
        //////    DataTable dtUsers = dsUsers.Tables[0].Clone();
        //////    DataTable dtTempUsers = dsUsers.Tables[0].Clone();
        //////    DefDataView = new DataView();


        //////    if (!string.IsNullOrEmpty(SearchStr))
        //////    {
        //////        var FilteredTable = DTFullTableClone.Select(@"UserName like '%" + SearchStr + "%' or EmailId like '%" + SearchStr + "%'", sortExp);
        //////        foreach (DataRow Dr in FilteredTable)
        //////        {
        //////            dtTempUsers.ImportRow(Dr);
        //////        }

        //////        if (dtTempUsers.Rows.Count > 0)
        //////        {
        //////            DefDataView = dtTempUsers.DefaultView;
        //////        }
        //////    }
        //////    else
        //////    {
        //////        DefDataView = DTFullTableClone.DefaultView;
        //////    }
        //////    if (!string.IsNullOrEmpty(sortExp) && DefDataView.Table != null && DefDataView.Table.Rows != null && DefDataView.Table.Rows.Count > 0)
        //////    {
        //////        DefDataView.Sort = sortExp;
        //////        dtTempUsers = DefDataView.ToTable();
        //////    }

        //////    // Set Paging and creatwe data table
        //////    for (int i = ((CurrentPage - 1) * Convert.ToInt32(Global.AdapUserPageSize)) + 1; i <= (CurrentPage * Convert.ToInt32(Global.AdapUserPageSize)); i++)
        //////    {
        //////        if (i <= dtTempUsers.Rows.Count)
        //////        {
        //////            DataRow newRow = dtTempUsers.Rows[i - 1];
        //////            dtUsers.ImportRow(newRow);
        //////        }
        //////        else
        //////        {
        //////            break;
        //////        }
        //////    }

        //////    TableGenerator = new HTMLTableGenerator();
        //////    TableGenerator.ShowSorting = true;
        //////    TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
        //////    RetVal = TableGenerator.GetTableHmtl(dtUsers.DefaultView.ToTable(), "NId", "AdapUser", true) + Constants.Delimiters.ParamDelimiter + CalPageCount((dtTempUsers.Rows.Count), Convert.ToInt32(Global.AdapUserPageSize));
        //////}

        //////catch (Exception ex)
        //////{
        //////    Global.CreateExceptionString(ex, null);
        //////}
        //////return RetVal;

        string RetVal = string.Empty;
        string[] Params;
        string sortExp = "";
        HTMLTableGenerator TableGenerator;
        string SearchStr = string.Empty;
        DataView DefDataView = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            int CurrentPage = int.Parse(Params[1]);
            if (Params.Length > 1)
            {
                sortExp = Params[3].ToString() + " " + Params[2].ToString();
                if (Params.Length > 4)
                {
                    SearchStr = Params[4].ToString();
                }
            }
            DataSet dsUsers = new DataSet();
            AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
            UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
            // Get ALL the users of current Adaptation Guid
            //dsUsers = UserLoginService.GetUsersByAdaptationURL(Global.GetAdaptationGUID());
            DataTable dtUsers = dsUsers.Tables[0].Clone();
            DataTable dtTempUsers = dsUsers.Tables[0].Clone();
            DefDataView = new DataView();
            if (!string.IsNullOrEmpty(SearchStr))
            {
                var FilteredTable = dsUsers.Tables[0].Select(@"UserName like '%" + SearchStr + "%' or EmailId like '%" + SearchStr + "%'", sortExp);
                foreach (DataRow Dr in FilteredTable)
                {
                    dtTempUsers.ImportRow(Dr);
                }

                if (dtTempUsers.Rows.Count > 0)
                {
                    DefDataView = dtTempUsers.DefaultView;
                }
            }
            else
            {
                DefDataView = dsUsers.Tables[0].DefaultView;
            }
            if (!string.IsNullOrEmpty(sortExp) && DefDataView.Table != null && DefDataView.Table.Rows != null && DefDataView.Table.Rows.Count > 0)
            {
                DefDataView.Sort = sortExp;
                dtTempUsers = DefDataView.ToTable();
            }

            // Set Paging and creatwe data table
            for (int i = ((CurrentPage - 1) * Convert.ToInt32(Global.AdapUserPageSize)) + 1; i <= (CurrentPage * Convert.ToInt32(Global.AdapUserPageSize)); i++)
            {
                if (i <= dtTempUsers.Rows.Count)
                {
                    DataRow newRow = dtTempUsers.Rows[i - 1];
                    dtUsers.ImportRow(newRow);
                }
                else
                {
                    break;
                }
            }

            TableGenerator = new HTMLTableGenerator();
            TableGenerator.ShowSorting = true;
            TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
            RetVal = TableGenerator.GetTableHmtl(dtUsers.DefaultView.ToTable(), "NId", "AdapUser", true) + Constants.Delimiters.ParamDelimiter + CalPageCount((dtTempUsers.Rows.Count), Convert.ToInt32(Global.AdapUserPageSize));
        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Set User As Admin for Current Adaptation
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string SetUserAsAdmin(string requestParam)
    {
        string RetVal = string.Empty;
        //string AdptationUrl = string.Empty;
        int UserNId;
        string Language, UserName, EmailId;
        string[] Params;

        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;
        try
        {
            //AdptationUrl = Global.GetAdaptationUrl();
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = int.Parse(Params[0]);
            Language = Params[1].ToString();
            UserName = Params[2].ToString();
            EmailId = Params[3].ToString();
            AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
            UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
            RetVal = UserLoginService.SetUserAsAdmin(UserNId, Global.GetAdaptationGUID()).ToString();

            #region "Call method to write log in XLS file"
            if (RetVal.ToUpper() == "TRUE")
            {

                XLSFileMsg = string.Format(Constants.CSVLogMessage.UserSetAsAdmin, EmailId);
                WriteLogInXLSFile(Constants.AdminModules.UserManagement.ToString(), XLSFileMsg);
            #endregion

                Session[Constants.SessionNames.LoggedAdminUser] = null;
                Session[Constants.SessionNames.LoggedAdminUserNId] = null;
                Frame_Message_And_Send_Mail(UserName, EmailId, UserNId.ToString(), true, true, Language);
                this.Create_MaintenanceAgency_ForAdmin(UserNId.ToString(), UserName, Language);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }
    public string GetAllUsers(string requestParam)
    {
        string RetVal = string.Empty;
        int AdapNId;
        string[] Params;
        string sortExp = "";
        HTMLTableGenerator TableGenerator;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            AdapNId = int.Parse(Params[0]);
            int CurrentPage = int.Parse(Params[1]);
            if (Params.Length > 1)
            {
                sortExp = Params[3].ToString() + " " + Params[2].ToString();
            }
            DataSet dsUsers = new DataSet();
            AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
            UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
            dsUsers = UserLoginService.GetGlobalUsers(AdapNId);
            DataTable dtUsers = dsUsers.Tables[0].Clone();
            for (int i = ((CurrentPage - 1) * Convert.ToInt32(Global.AdapUserPageSize)) + 1; i <= (CurrentPage * Convert.ToInt32(Global.AdapUserPageSize)); i++)
            {
                if (i <= dsUsers.Tables[0].Rows.Count)
                {
                    DataRow newRow = dsUsers.Tables[0].Rows[i - 1];
                    dtUsers.ImportRow(newRow);
                }
                else
                {
                    break;
                }
            }
            if (sortExp != "")
            {
                dtUsers.DefaultView.Sort = sortExp;
            }
            TableGenerator = new HTMLTableGenerator();
            TableGenerator.ShowSorting = true;
            TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
            RetVal = TableGenerator.GetTableHmtl(dtUsers.DefaultView.ToTable(), "NId", "AdapUser", true) + Constants.Delimiters.ParamDelimiter + CalPageCount((dsUsers.Tables[0].Rows.Count), Convert.ToInt32(Global.AdapUserPageSize));

        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    public string UpdatePassword(string requestParam)
    {
        string RetVal = string.Empty;
        int UserNId;
        string Password, Language, UserName, EmailId;
        string[] Params;
        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = int.Parse(Params[0]);
            Password = Params[1].ToString();
            Language = Params[2].ToString();
            UserName = Params[3].ToString();
            EmailId = Params[4].ToString();
            AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
            UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
            RetVal = UserLoginService.UpdateUserPassword(UserNId, Global.OneWayEncryption(Password)).ToString();
            if (RetVal.ToUpper() == "TRUE")
            {
                Frame_ChangePassword_And_Send_Mail(UserName, EmailId, Language, Password);

                #region "Call method to write log in XLS file"
                XLSFileMsg = string.Format(Constants.CSVLogMessage.ChangePassword, EmailId);
                WriteLogInXLSFile(Constants.AdminModules.UserManagement.ToString(), XLSFileMsg);
                #endregion
            }

        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }
    public string CalPageCount(int RowsCount, int PageSize)
    {

        int totalPage = RowsCount / PageSize;

        //add the last page
        if (RowsCount % PageSize != 0) totalPage++;

        return totalPage.ToString();
    }

    public void Frame_ChangePassword_And_Send_Mail(string FirstName, string EmailId, string Language, string Password)
    {
        string MessageContent = string.Empty;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
        TamplatePath += Language + "\\" + Constants.FileName.UpdatePasswordAdmin;
        MessageContent = GetEmailTamplate(TamplatePath);
        Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
        Subject = Subject.Replace("[^^^^]", "");
        Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
        Body = Body.Replace("[****]USER_NAME[****]", FirstName);
        Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
        Body = Body.Replace("[****]PASSWORD[****]", Password);
        Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
        Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName);
    }



    #endregion

    /// <summary>
    /// Used to Export language xml files
    /// </summary>
    /// <param name="requestParam">source and target xml files name seperated with delemeter</param>
    /// <returns></returns>
    public string ExportLanguageXML(string requestParam)
    {
        string RetVal = string.Empty;
        string XMLFilesDir = string.Empty;
        string SrcLanguageName = string.Empty;
        string TrgLanguageName = string.Empty;
        string FileName = string.Empty;
        string SrcLngXML = string.Empty;
        string TrgLngXML = string.Empty;
        DataSet dsSrcLng = new DataSet();
        DataSet dsTrgLng = new DataSet();
        DataTable DtSrcLng = new DataTable();
        DataTable DtTrgLng = new DataTable();
        System.IO.FileStream SrcReadXml = null;
        System.IO.FileStream TrgReadXml = null;
        string OutputXmlFilePath = string.Empty;
        List<KeyValues> lstKeyValue = new List<KeyValues>();
        KeyValues KeyVal = null;
        ArrayList ALTargetKeys = new ArrayList();
        DataRow[] drArray;
        string[] Params;

        //Variables for creating XLS Logfile 
        string XLSFileMsg = string.Empty;
        try
        {
            // Get Source Path of XML files
            XMLFilesDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            OutputXmlFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.LanguageXMLFiles);

            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            // Get source and target language xml file names from input param
            SrcLanguageName = Params[0].ToString();
            TrgLanguageName = Params[1].ToString();

            // Get Complete path of source language XML file
            SrcLngXML = XMLFilesDir + SrcLanguageName + ".XML";
            TrgLngXML = XMLFilesDir + TrgLanguageName + ".XML";
            FileName = SrcLanguageName.Split(new string[] { "[" }, StringSplitOptions.None)[0].ToString() + "- " + TrgLanguageName.Split(new string[] { " [" }, StringSplitOptions.None)[0].ToString();

            // Create new FileStream with which to read the schema.
            SrcReadXml = new FileStream(SrcLngXML, FileMode.Open);
            TrgReadXml = new FileStream(TrgLngXML, FileMode.Open);

            // Read language xml files as data set
            dsSrcLng.ReadXml(SrcReadXml);
            dsTrgLng.ReadXml(TrgReadXml);

            // Get data table from dataset
            DtSrcLng = dsSrcLng.Tables[0];
            DtTrgLng = dsTrgLng.Tables[0];
            foreach (DataRow row in DtSrcLng.Rows)
            {
                // List<KeyValues> listTest = new List<KeyValues>();
                drArray = null;
                KeyVal = new KeyValues();
                if (!ALTargetKeys.Contains(row["Key"].ToString().Trim()))
                {
                    ALTargetKeys.Add(row["Key"].ToString().Trim());
                    KeyVal.Key = row["Key"].ToString();
                    KeyVal.ValueSource = row["Value"].ToString();
                    drArray = DtTrgLng.Select("Key = '" + row["Key"].ToString() + "'");
                    if (drArray.Length > 0)
                    {
                        KeyVal.ValueTarget = drArray[0]["Value"].ToString();
                    }
                    lstKeyValue.Add(KeyVal);
                }
            }

            //foreach (DataRow row in DtTrgLng.Rows)
            //{
            //    if (!ALTargetKeys.Contains(row["Key"].ToString().ToUpper().Trim()))
            //    {
            //        KeyVal = new KeyValues();
            //        KeyVal.Key = row["Key"].ToString();
            //        KeyVal.ValueTarget = row["Value"].ToString();
            //        lstKeyValue.Add(KeyVal);
            //    }
            //}

            IWorksheet ExcelSheet = (IWorksheet)Factory.GetWorkbook().Sheets[0];   //(IWorksheet)ExcelWorkbook.Sheets[0];
            int RowIndex = 0;
            //Set the hidden fields for langaugue file names
            ExcelSheet.Cells[RowIndex, 1].Value = SrcLanguageName;
            ExcelSheet.Cells[RowIndex, 2].Value = TrgLanguageName;
            // hide first row as it consist of xml file names
            SpreadsheetGear.IRange Range = ExcelSheet.Range[0, 0, 0, 2];
            Range.RowHeight = 0.0;
            //Create Header row for XLS file
            RowIndex++;
            CreateHeader(ref ExcelSheet, RowIndex, FileName);
            foreach (KeyValues KeyValues in lstKeyValue)
            {
                RowIndex++;
                ExcelSheet.Cells[RowIndex, 0].Value = KeyValues.Key;
                ExcelSheet.Cells[RowIndex, 1].Value = KeyValues.ValueSource;
                ExcelSheet.Cells[RowIndex, 2].Value = KeyValues.ValueTarget;
                ExcelSheet.Cells[RowIndex, 0].Borders.Color = System.Drawing.Color.Black;
                ExcelSheet.Cells[RowIndex, 1].Borders.Color = System.Drawing.Color.Black;
                ExcelSheet.Cells[RowIndex, 2].Borders.Color = System.Drawing.Color.Black;
                if (KeyValues.Key.ToString() == "FILE_VERSION")
                {
                    SpreadsheetGear.IRange RngFileVersion = ExcelSheet.Range[RowIndex, 0, RowIndex + 1, 2];
                    RngFileVersion.NumberFormat = "0.0";
                }
            }

            // hide columns containing keys
            SpreadsheetGear.IRange Rng = ExcelSheet.Range[0, 0, RowIndex - 1, 0];
            Rng.ColumnWidth = 0.0;

            // Increase width of rows containing data values
            SpreadsheetGear.IRange RangeData = ExcelSheet.Range[0, 1, RowIndex - 1, 2];
            RangeData.ColumnWidth = 50.0;

            // hide first four rows
            SpreadsheetGear.IRange Rng1 = ExcelSheet.Range[2, 0, 6, 2];
            Rng1.RowHeight = 0.0;
            ExcelSheet.Name = FileName;
            ExcelSheet.SaveAs(OutputXmlFilePath + FileName + ".xls", SpreadsheetGear.FileFormat.Excel8);
            SrcReadXml.Close();
            TrgReadXml.Close();
            RetVal = "true" + Constants.Delimiters.ParamDelimiter + FileName;


            #region "Call method to write log in XLS file"
            if (RetVal.Contains("true"))
            {
                XLSFileMsg = string.Format(Constants.CSVLogMessage.ExportLanguage, SrcLanguageName, TrgLanguageName);
                WriteLogInXLSFile(Constants.AdminModules.LanguageSettings.ToString(), XLSFileMsg);
            }
            #endregion

        }
        catch (Exception ex)
        {
            RetVal = "false";
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            SrcReadXml.Dispose();
            TrgReadXml.Dispose();
        }
        return RetVal;
    }

    /// <summary>
    /// This method is used fior creating header of xls files
    /// </summary>
    /// <param name="ExcelSheet"></param>
    /// <param name="RowIndex"></param>
    /// <param name="SrcLanguageName"></param>
    /// <param name="TrgLanguageName"></param>
    private void CreateHeader(ref IWorksheet excelSheet, int rowIndex, string fileName)
    {
        string ColumnNameText = string.Empty;
        string[] FileName = fileName.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
        try
        {
            int ColumnIndex = 0;
            for (int Icount = 0; Icount < 3; Icount++)
            {
                ColumnIndex = Icount;
                if (Icount == 0)
                {
                    ColumnNameText = "Key";
                }
                else if (Icount == 1)
                {
                    ColumnNameText = FileName[0].ToString();
                }
                else if (Icount == 2)
                {
                    ColumnNameText = FileName[1].ToString();
                }
                excelSheet.Cells[rowIndex, ColumnIndex].Value = ColumnNameText;
                excelSheet.Cells[rowIndex, ColumnIndex].Borders.Color = System.Drawing.Color.Black; // Sets The Border Color For Header
                excelSheet.Cells[rowIndex, ColumnIndex].Interior.Color = System.Drawing.Color.Gray; // Sets The Background Color For Header
                excelSheet.Cells[rowIndex, ColumnIndex].Font.Bold = true; // Sets Header Font To Bold
                excelSheet.Cells[rowIndex, ColumnIndex].Font.Color = System.Drawing.Color.White; // Sets Header Font Color To White
                excelSheet.Cells[rowIndex, ColumnIndex].Font.Size = 12;
                excelSheet.Cells[rowIndex, ColumnIndex].VerticalAlignment = VAlign.Top;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    // Write logs for optimize databse
    public string GenerateLogsForOptimiseDB(string DataBaseOptmisedFor)
    {

        string RetVal = string.Empty;
        string XLSFileMsg = string.Empty;
        try
        {
            XLSFileMsg = string.Format(Constants.CSVLogMessage.OptimizeDatabsae, DataBaseOptmisedFor);
            WriteLogInXLSFile(Constants.AdminModules.DatabaseSettings, XLSFileMsg);
        }

        catch (Exception ex)
        {
            RetVal = "false";
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Returns latest writen file from the specified directory.
    /// If the directory does not exist or doesn't contain any file, DateTime.MinValue is returned.
    /// </summary>
    /// <param name="directoryInfo">Path of the directory that needs to be scanned</param>
    /// <returns></returns>
    private static DateTime GetLatestWriteTimeFromFileInDirectory(DirectoryInfo directoryInfo)
    {
        if (directoryInfo == null || !directoryInfo.Exists)
            return DateTime.MinValue;

        FileInfo[] files = directoryInfo.GetFiles();
        DateTime lastWrite = DateTime.MinValue;

        foreach (FileInfo file in files)
        {
            if (file.LastWriteTime > lastWrite)
            {
                lastWrite = file.LastWriteTime;
            }
        }

        return lastWrite;
    }

    /// <summary>
    /// Returns file's latest writen timestamp from the specified directory.
    /// If the directory does not exist or doesn't contain any file, null is returned.
    /// </summary>
    /// <param name="directoryInfo">Path of the directory that needs to be scanned</param>
    /// <returns></returns>
    private static List<FileInfo> GetLatestWritenFileFileInDirectory(DirectoryInfo directoryInfo)
    {
        if (directoryInfo == null || !directoryInfo.Exists)
            return null;

        FileInfo[] files = directoryInfo.GetFiles();
        TimeSpan Hours = new TimeSpan(2, 0, 0);
        DateTime LatestDateModified = Convert.ToDateTime(directoryInfo.LastWriteTime);
        DateTime FilesNottotake = LatestDateModified.Subtract(Hours);
        DateTime lastWrite = DateTime.MinValue;
        List<FileInfo> lastWritenFiles = new List<FileInfo>();

        foreach (FileInfo file in files)
        {
            // lastWrite = file.LastWriteTime;
            if (file.LastWriteTime > FilesNottotake)
            {
                lastWritenFiles.Add(file);
            }
        }
        return lastWritenFiles;
    }

    #region "write log in XLS file"
    // Call method to write log in csv file
    public void WriteLogInXLSFile(string ModuleName, string XLSFileMsg)
    {
        string UserName = string.Empty;
        string UserEmailID = string.Empty;
        string OldValue = string.Empty;
        string ClientIpAddress = string.Empty;
        try
        {
            // get user name from session
            if (!string.IsNullOrEmpty(Session[Constants.SessionNames.LoggedAdminUser].ToString()))
            {
                UserName = Session[Constants.SessionNames.LoggedAdminUser].ToString();
            }
            // get user email id from session
            if (!string.IsNullOrEmpty(Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
            {
                UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
            }
            ClientIpAddress = GetClientIpAddress();
            XLSLogGenerator.WriteLogInXLSFile(UserName, ModuleName, XLSFileMsg, UserEmailID, ClientIpAddress);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    //Get Visitor IP address method
    public string GetClientIpAddress()
    {
        string stringIpAddress = string.Empty;
        try
        {
            stringIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (stringIpAddress == null) //may be the HTTP_X_FORWARDED_FOR is null
            {
                stringIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];//we can use REMOTE_ADDR
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        if (!stringIpAddress.Contains('.'))
        {
            if (stringIpAddress.Split('.').Length < 3)
            {
                stringIpAddress = GetLanIPAddress();
            }
        }
        return stringIpAddress;
    }

    //Get Lan Connected IP address method
    public string GetLanIPAddress()
    {
        //Get the Host Name
        string stringHostName = Dns.GetHostName();
        //Get The Ip Host Entry
        IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
        //Get The Ip Address From The Ip Host Entry Address List
        IPAddress[] arrIpAddress = ipHostEntries.AddressList;
        return arrIpAddress[arrIpAddress.Length - 2].ToString();
    }
    #endregion

    /// <summary>
    /// Generate Site Map based on Cache Result of the database.
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string GenerateSiteMap(string requestParam)
    {
        int DbNid = -1;
        string RetVal = "false";
        string DefaultLanguageCode = string.Empty;
        DataRow[] drArray = null;
        DataRow[] DataRows = null;
        DataRow[] DataRowsInd = null;
        string SiteMapName = string.Empty;
        string SiteMapData = string.Empty;
        string[] SiteMapDataArray = null;
        string Query = string.Empty;
        bool FirstSitemap = true;
        ArrayList ALAreas = new ArrayList();
        string SectorIndicators = string.Empty;
        string BlockAreaNIds = string.Empty;
        DbNid = int.Parse(requestParam);
        DIConnection ObjDIConnection = null;
        string SiteMapHtmlPagePath;
        string FinalAreaNids = string.Empty;
        HtmlDocument HtmDocument = null;
        StringBuilder SbSiteMapSectors = new StringBuilder();
        ObjDIConnection = Global.GetDbConnection(DbNid);
        DataTable DTAreas = null;
        DataTable DTBlockAreasParent = null;
        DataTable DTBlockAreas = null;
        DataTable DTCacheData = null;//ObjDIConnection.ExecuteDataTable("SP_GET_SITEMAP_DATA", CommandType.StoredProcedure, new List<DbParameter>());

        //Get Default Indicators and Areas
        //DataTable DefaultIndicators = Global.GetDefaultIndicator(Global.GetDefaultDbNId());
        DefaultLanguageCode = Global.GetDefaultLanguageCode();

        DataTable DefaultIndicators = GetDefaultIndicators(DefaultLanguageCode, Query, ObjDIConnection);
        DataTable DefaultAreas = GetDefaultAreas(ref DefaultLanguageCode, ref Query, ALAreas, ref BlockAreaNIds, ObjDIConnection, ref DTAreas, ref DTBlockAreasParent, ref DTBlockAreas);
        DataTable DTIndArea = MakeIndAreaTable(DefaultIndicators, DefaultAreas);


        Query = "SELECT DISTINCT IC.IC_Name,IC.IC_NId,IUS.Indicator_NId from ut_indicator_classifications_" + DefaultLanguageCode + " IC,ut_indicator_unit_subgroup IUS,ut_ic_ius ICIUS WHERE IC.IC_NId = ICIUS.IC_NId AND ICIUS.IUSNId = IUS.IUSNId AND IC.IC_Type='SC' AND IC.IC_Parent_NId ='-1'";
        DataTable DTIndicatorSector = ObjDIConnection.ExecuteDataTable(Query);
        if (DTIndicatorSector.Rows.Count == 1)
        {
            Query = "SELECT DISTINCT IC.IC_Name,IC.IC_NId,IUS.Indicator_NId from ut_indicator_classifications_" + DefaultLanguageCode + " IC,ut_indicator_unit_subgroup IUS,ut_ic_ius ICIUS WHERE IC.IC_NId = ICIUS.IC_NId AND ICIUS.IUSNId = IUS.IUSNId AND IC.IC_Type='SC' AND IC.IC_Parent_NId IN(SELECT IC_NId from ut_indicator_classifications_en where IC_Parent_NId='-1') ";
            DTIndicatorSector = ObjDIConnection.ExecuteDataTable(Query);
        }
        //Get Sectors List.
        DataTable DTSector = DTIndicatorSector.DefaultView.ToTable(true, "IC_NId", "IC_Name");

        FileInfo ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Global.DropNCreateSitemapURLTable));
        string DbScripts = ScriptFile.OpenText().ReadToEnd();
        // Execute script file to check and create database schema
        ObjDIConnection.ExecuteNonQuery(DbScripts);

        XmlDocument XMLPDoc = new XmlDocument();
        XmlNode declaration = XMLPDoc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
        XMLPDoc.AppendChild(declaration);
        XmlElement URLSet = XMLPDoc.CreateElement("sitemapindex");
        XMLPDoc.AppendChild(URLSet);

        URLSet.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
        URLSet.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
        URLSet.SetAttribute("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");


        #region "-- SiteMapHtml"

        // Create Html Document object
        HtmDocument = new HtmlDocument();
        // Get Site Map Html File Path
        SiteMapHtmlPagePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FileName.SitemapHtmlPage);

        if (File.Exists(SiteMapHtmlPagePath))
        {
            File.Delete(SiteMapHtmlPagePath);
        }

        // Load Html Document
        HtmDocument.LoadHtml("");
        // Create Main Div For Data

        // Crate Main Div for Data
        HtmlNode DivMain;
        HtmlNode DivSectorContainer;
        DivMain = HtmDocument.CreateElement("div");
        DivMain.SetAttributeValue("id", "DivMain_Container");
        //  DivMain.ChildNodes.Append(.Add("innerHtml", "Data");

        DivSectorContainer = HtmDocument.CreateElement("div");
        DivSectorContainer.SetAttributeValue("id", "DivSectorContainer");
        DivSectorContainer.SetAttributeValue("class", "sitemap");

        #endregion
        StringBuilder SbSectorInnerLinks = new StringBuilder();
        try
        {
            //Add IUS data Links            
            foreach (DataRow dr in DTSector.Rows)
            {
                SectorIndicators = string.Empty;
                drArray = null;
                DataRows = null;
                drArray = DTIndicatorSector.Select("IC_NId = '" + dr["IC_NId"].ToString() + "'");
                for (int i = 0; i < drArray.Length; i++)
                {
                    if (SectorIndicators == string.Empty)
                    {
                        SectorIndicators = drArray[i]["Indicator_NId"].ToString();
                    }
                    else
                    {
                        SectorIndicators = SectorIndicators + "," + drArray[i]["Indicator_NId"].ToString();
                    }
                }
                DataRowsInd = DefaultIndicators.Select("Indicator_NId IN(" + SectorIndicators + ")");
                if (DataRowsInd.Length > 0)
                {
                    SbSectorInnerLinks.Append("<div class='clear' /><div><h2 class='lnk_dspy' title='" + dr["IC_Name"].ToString() + "'><img id='img" + dr["IC_Nid"].ToString() + "' src='../../stock/themes/default/images/plus.png' style='cursor:pointer' onclick='scolexp(this);' /><span class='sitemap_sc' id='sph" + dr["IC_Nid"].ToString() + "' onclick='scolexpdv(this);'>" + dr["IC_Name"].ToString() + "</span></h2><div style='padding-left:25px;display:none' id='dv" + dr["IC_Nid"].ToString() + "'>");
                    FirstSitemap = true;
                    foreach (DataRow drInd in DataRowsInd)
                    {
                        DataRows = DTIndArea.Select("Indicator_NId IN(" + drInd["Indicator_NId"].ToString() + ")");
                        if (DataRows.Length > 0)
                        {
                            SbSectorInnerLinks.Append("<div class='clear' /><div title='" + drInd["Indicator_Name"].ToString() + "' class='navls_top_pg'><img id='img" + dr["IC_Nid"].ToString() + "-" + drInd["Indicator_NId"].ToString() + "' src='../../stock/themes/default/images/st_arrow.png' style='cursor:pointer' onclick='scolexpinner(this);' />&nbsp;&nbsp;&nbsp;&nbsp;<span id='spn" + dr["IC_Nid"].ToString() + "-" + drInd["Indicator_NId"].ToString() + "' onclick='scolexdvinner(this);' class='lnk_dspy'>" + drInd["Indicator_Name"].ToString() + "</span><br /><div style='padding-left:50px;display:none;padding-top:20px' id='dv" + dr["IC_Nid"].ToString() + "-" + drInd["Indicator_NId"].ToString() + "'>");

                            SiteMapData = GenerateIndivisualSitemap(DataRows, -1, -1, "-1", ObjDIConnection, dr["IC_NId"].ToString(), dr["IC_Name"].ToString(), DefaultLanguageCode, FirstSitemap);

                            SiteMapDataArray = SiteMapData.Split(new string[] { "[~]" }, StringSplitOptions.None);
                            SbSectorInnerLinks.Append(SiteMapDataArray[1].ToString());
                            SbSectorInnerLinks.Append("</div></div>");
                            if (FirstSitemap)
                            {
                                XmlElement URL = XMLPDoc.CreateElement("sitemap");
                                URLSet.AppendChild(URL);
                                XmlElement loc = XMLPDoc.CreateElement("loc");
                                loc.InnerText = Global.GetAdaptationUrl().Replace(":80", "") + "/" + SiteMapDataArray[0].ToString();
                                URL.AppendChild(loc);
                            }
                            FirstSitemap = false;
                        }
                    }
                    SbSectorInnerLinks.Append("</div></div>");
                }
            }
            RetVal = "true";
        }
        catch (Exception Ex)
        {
            string msg = Ex.Message;
        }
        // HtmlNode body = HtmDocument.DocumentNode.ChildNodes.Append(DivSectorContainer).;
        DivSectorContainer.InnerHtml = SbSectorInnerLinks.ToString();
        DivMain.ChildNodes.Append(DivSectorContainer);
        HtmDocument.DocumentNode.ChildNodes.Append(DivMain);
        HtmDocument.Save(SiteMapHtmlPagePath);
        XMLPDoc.Save(Server.MapPath("../../sitemap.xml"));
        return RetVal;
    }

    private static DataTable MakeIndAreaTable(DataTable DefaultIndicators, DataTable DefaultAreas)
    {
        DataTable RetVal = new DataTable();
        try
        {
            DataTable DTIndArea = new DataTable();
            DTIndArea.Columns.Add("Area_NId");
            DTIndArea.Columns.Add("Indicator_NId");
            DTIndArea.Columns.Add("Area_Name");
            DTIndArea.Columns.Add("Indicator_Name");
            DTIndArea.Columns.Add("Area_ID");

            DataRow dr = null;
            foreach (DataRow drArea in DefaultAreas.Rows)
            {
                foreach (DataRow drIndicator in DefaultIndicators.Rows)
                {
                    dr = DTIndArea.NewRow();
                    dr["Area_NId"] = drArea["Area_NId"].ToString();
                    dr["Area_Name"] = drArea["Area_Name"].ToString();
                    dr["Area_ID"] = drArea["Area_ID"].ToString();
                    dr["Indicator_NId"] = drIndicator["Indicator_NId"].ToString();
                    dr["Indicator_Name"] = drIndicator["Indicator_Name"].ToString();
                    DTIndArea.Rows.Add(dr);
                }
            }
            DataView DVIndArea = DTIndArea.DefaultView;
            DVIndArea.Sort = "Area_Name";
            DTIndArea = DVIndArea.ToTable();
            RetVal = DTIndArea;
        }
        catch (Exception)
        {
        }
        return RetVal;
    }

    private static DataTable GetDefaultIndicators(string DefaultLanguageCode, string Query, DIConnection ObjDIConnection)
    {
        DataTable RetVal = new DataTable();
        try
        {
            string DefaultIndicators = Global.GetDefaultIndicator(Global.GetDefaultDbNId());
            Query = "SELECT * FROM ut_indicator_" + DefaultLanguageCode + " WHERE Indicator_NId IN(" + DefaultIndicators + ")";
            DataTable DTIndicators = ObjDIConnection.ExecuteDataTable(Query);
            RetVal = DTIndicators;
        }
        catch (Exception)
        {

        }
        return RetVal;
    }

    private static DataTable GetDefaultAreas(ref string DefaultLanguageCode, ref string Query, ArrayList ALAreas, ref string BlockAreaNIds, DIConnection ObjDIConnection, ref DataTable DTAreas, ref DataTable DTBlockAreasParent, ref DataTable DTBlockAreas)
    {
        DataTable RetVal = new DataTable();
        try
        {
            string DefaultAreas = Global.GetDefaultArea(Global.GetDefaultDbNId());
            DefaultLanguageCode = Global.GetDefaultLanguageCode();

            string[] AreaArray = DefaultAreas.Split(new string[] { "," }, StringSplitOptions.None);
            string AreaNIds = string.Empty;
            for (int i = 0; i < AreaArray.Length - 1; i++)
            {
                if (Global.isNumeric(AreaArray[i].ToString(), System.Globalization.NumberStyles.Number))
                {
                    if (AreaNIds == string.Empty)
                    {
                        AreaNIds = AreaArray[i].ToString();
                    }
                    else
                    {
                        AreaNIds = AreaNIds + "," + AreaArray[i].ToString();
                    }
                    if (!ALAreas.Contains(AreaArray[i].ToString()))
                    {
                        ALAreas.Add(AreaArray[i].ToString());
                    }
                }
            }

            if (AreaNIds.Length > 0)
            {
                //Get Area Data
                Query = "SELECT * from UT_Area_" + DefaultLanguageCode + " WHERE AREA_NID IN(" + AreaNIds + ")";
                DTBlockAreasParent = ObjDIConnection.ExecuteDataTable(Query);
                foreach (DataRow dr in DTBlockAreasParent.Rows)
                {
                    try
                    {
                        if (dr["Area_Block"].ToString().Trim() != string.Empty)
                        {
                            BlockAreaNIds = dr["Area_Block"].ToString();
                            Query = "SELECT * from UT_Area_" + DefaultLanguageCode + " WHERE AREA_NID IN(" + BlockAreaNIds + ")";
                            DTBlockAreas = ObjDIConnection.ExecuteDataTable(Query);
                            foreach (DataRow brow in DTBlockAreas.Rows)
                            {
                                if (!ALAreas.Contains(brow["Area_NId"].ToString()))
                                {
                                    ALAreas.Add(brow["Area_NId"].ToString());
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            AreaNIds = string.Empty;
            for (int i = 0; i < ALAreas.Count; i++)
            {
                if (AreaNIds == string.Empty)
                {
                    AreaNIds = ALAreas[i].ToString();
                }
                else
                {
                    AreaNIds = AreaNIds + "," + ALAreas[i].ToString();
                }
            }

            //Get the Final Area Datatable
            Query = "SELECT * from UT_Area_" + DefaultLanguageCode + " WHERE AREA_NID IN(" + AreaNIds + ")";
            DTAreas = ObjDIConnection.ExecuteDataTable(Query);
            RetVal = DTAreas;
        }
        catch (Exception)
        {

        }
        return RetVal;
    }

    private string GenerateIndivisualSitemap(DataRow[] cacheDataRows, int startRow, int LastRow, string siteMapNumber, DIConnection diConnection, string siteMapName, string sectorName, string defaultLanguageCode, bool firstSitemap)
    {
        string RetVal = string.Empty;
        XmlDocument XMLDoc = new XmlDocument();
        XmlNode declaration = null;
        XmlElement URLSet = null;
        StringBuilder ObjSb = new System.Text.StringBuilder();
        int RowCount = 1;
        string SitemapTagData = string.Empty;
        if (firstSitemap)
        {
            declaration = XMLDoc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
            XMLDoc.AppendChild(declaration);
            URLSet = XMLDoc.CreateElement("urlset");
            XMLDoc.AppendChild(URLSet);
            URLSet.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            URLSet.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            URLSet.SetAttribute("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9" + "  " + "http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
        }
        else
        {
            XMLDoc.Load(Server.MapPath("../../" + siteMapName + "-sitemap.xml"));
            URLSet = XMLDoc.DocumentElement;
        }


        //Generate SiteMap for Each row of Cached Data  
        foreach (DataRow row in cacheDataRows)
        {
            if (startRow == -1)
            {
                SitemapTagData = AddSitemapTags(XMLDoc, URLSet, row, diConnection, sectorName, defaultLanguageCode);
                if (SitemapTagData != string.Empty)
                {
                    ObjSb.Append(SitemapTagData);
                }
            }
            else
            {
                if (RowCount >= startRow && RowCount < LastRow)
                {
                    AddSitemapTags(XMLDoc, URLSet, row, diConnection, sectorName, defaultLanguageCode);
                }
            }
            RowCount++;
        }


        if (startRow == -1)
        {
            RetVal = siteMapName + "-sitemap.xml";
            XMLDoc.Save(Server.MapPath("../../" + siteMapName + "-sitemap.xml"));
        }
        else
        {
            RetVal = siteMapName + "-sitemap" + siteMapNumber + ".xml";
            XMLDoc.Save(Server.MapPath("../../" + siteMapName + "-sitemap" + siteMapNumber + ".xml"));
        }
        RetVal = RetVal + "[~]" + ObjSb.ToString();
        return RetVal;
    }


    private static string AddSitemapTags(XmlDocument XMLDoc, XmlElement URLSet, DataRow row, DIConnection diConnection, string sectorName, string defaultLanguageCode)
    {
        string RetVal = string.Empty;
        string AnchorURL = string.Empty;
        if (Global.isNumeric(row["Area_NId"].ToString(), System.Globalization.NumberStyles.Number))
        {
            XmlElement URL = XMLDoc.CreateElement("url");
            URLSet.AppendChild(URL);
            XmlElement loc = XMLDoc.CreateElement("loc");
            Random rnd = new Random();
            string URLString = defaultLanguageCode + "/S/" + sectorName + "/" + row["Indicator_Name"].ToString() + "/" + row["Area_Name"].ToString();
            //string URLString = row["Area_Name"].ToString().Replace(" ", "-") + "-" + row["Indicator_Name"].ToString().Replace(" ", "-") + "-" + defaultLanguageCode;
            //string QueryString = "refer_url=catalog&jsonAreasTopics={%22apn%22:%22" + "1" + "%22,%22i%22:%22" + row["Indicator_Name"].ToString() + "%22,%22i_n%22:%22" + row["Indicator_NId"].ToString() + "%22,%22a%22:%22" + row["Area_Name"].ToString() + "%22,%22a_n%22:%22" + row["Area_NId"].ToString() + "%22,%22lngCode%22:%22" + defaultLanguageCode + "%22}";
            //string Query = "INSERT INTO SitemapUrl(URL,QueryString) Values (N'" + URLString + "',N'" + QueryString + "')";
            //try
            //{
            //    diConnection.ExecuteNonQuery(Query);
            //}
            //catch (Exception ex)
            //{

            //}
            AnchorURL = Global.GetAdaptationUrl().Replace(":80", "") + "/Search/" + URLString;
            loc.InnerText = AnchorURL;
            URL.AppendChild(loc);
            XmlElement lastmod = XMLDoc.CreateElement("lastmod");
            lastmod.InnerText = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            URL.AppendChild(lastmod);
            XmlElement changefreq = XMLDoc.CreateElement("changefreq");
            changefreq.InnerText = "daily";
            URL.AppendChild(changefreq);
            XmlElement priority = XMLDoc.CreateElement("priority");
            priority.InnerText = "0.8";
            URL.AppendChild(priority);
            RetVal = "<div style='float:left;width:270px'><a title='" + row["Indicator_Name"].ToString() + " - " + row["Area_Name"].ToString() + " (" + row["Area_ID"].ToString() + "' href='" + AnchorURL.Trim() + "'>" + row["Area_Name"].ToString() + " (" + row["Area_ID"].ToString() + ")</a></div>";
        }
        return RetVal;
    }


    public string ChangeDatabasePassword(string param)
    {
        string RetVal = string.Empty;
        string[] ParamArray = param.Split(new string[] { "[****]" }, StringSplitOptions.None);
        string OldPassword = ParamArray[0].ToString();
        string NewPassword = ParamArray[1].ToString();
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string DbConn = string.Empty;
        string[] OldDBCon = null;

        try
        {
            //Check for old password existance
            //string[] OldConnectionDetails = Global.GetDbConnectionDetails(Global.GetDefaultDbNId());
            string[] ConnectionDetails = Global.GetDbConnectionDetails(Global.GetDefaultDbNId());
            OldDBCon = ConnectionDetails[1].Split(new string[] { "||" }, StringSplitOptions.None);
            if (OldPassword == Global.DecryptString(OldDBCon[4].ToString()))
            {
                for (int i = 0; i < OldDBCon.Length; i++)
                {
                    if (i == 4)
                    {
                        DbConn += "||" + Global.EncryptString(NewPassword);
                    }
                    else
                    {
                        if (DbConn == string.Empty)
                        {
                            DbConn = OldDBCon[i].ToString();
                        }
                        else
                        {
                            DbConn += "||" + OldDBCon[i].ToString();
                        }
                    }
                }
                //Update the new password
                DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
                XmlDoc = new XmlDocument();
                XmlDoc.Load(DBConnectionsFile);
                xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + Global.GetDefaultDbNId() + "]");
                xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value = DbConn;
                XmlDoc.Save(DBConnectionsFile);
                RetVal = "1";
            }
            else
            {
                RetVal = "0";
            }
        }
        catch (Exception)
        {
            RetVal = "-1";
        }

        return RetVal;
    }

    // this method is used for creating cms database if database not exist
    /// <summary>
    /// Create CMS database for adaptation If Database Not exise
    /// </summary>
    /// <param name="CMSDatabaseName">Name of the created Cms Database</param>
    /// <returns>True if database created or already exist</returns>
    public static bool CheckNCreateCMSDatabase(out string CMSDatabaseName)
    {
        bool RetVal = false;
        DIConnection ObjDIConnection = null;
        FileInfo ScriptFile;
        string DbScripts = string.Empty;
        string LngDbScripts = string.Empty;
        string DBConn = string.Empty;
        string[] DBConnArr;
        string DbNId = string.Empty;
        string Password = string.Empty;
        string DatabaseName = string.Empty; // Name of Adaptation Database
        CMSDatabaseName = string.Empty; // Name of CMS Database 
        string DatabasePath = string.Empty;
        try
        {
            // Get Database NID of Default Database
            DbNId = Global.GetDefaultDbNId();
            // If Database NID is null or empty break further execution of code 
            if (string.IsNullOrEmpty(DbNId))
            {
                RetVal = false;
                return RetVal;
            }
            //Get connection details of database
            DBConnArr = Global.GetDbNConnectionDetails(DbNId, string.Empty);
            // If Database returned database details are less than 6 break further execution of code 
            if (DBConnArr.Length < 6)
            {
                RetVal = false;
                return RetVal;
            }
            //Get Decrypted password
            Password = DBConnArr[6].ToString();
            //Get Adaptation Database name
            DatabaseName = DBConnArr[4].ToString();
            //Name of CMS Database
            CMSDatabaseName = DatabaseName + "_CMS";
            // Create Database connection
            ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(DBConnArr[2])), DBConnArr[3].ToString(), string.Empty, DatabaseName, DBConnArr[5].ToString(), Password);

            //Create and Execute database creation script
            ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Global.CmsDbCreationFile));
            // If Script File not exist in the folder break further execution of code  
            if (!ScriptFile.Exists)
            {
                RetVal = false;
                return RetVal;
            }
            // get path of database file from web config file
            DatabasePath = Global.CmsDataBasePath;// ConfigurationManager.AppSettings[Constants.WebConfigKey.CmsDataBasePath].ToString();

            // This Script file contains sql syntax to creates database, if database is not already existing in database
            // Replace DBName with new database name
            DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("DBName", CMSDatabaseName).Replace("GO", "");
            DbScripts = DbScripts.Replace("CmsDataBasePath", DatabasePath);
            // Execute script file to check and create database schema
            ObjDIConnection.ExecuteNonQuery(DbScripts);
            // Close file object
            ScriptFile.OpenText().Close();
            //Create and Execute database tables creation script
            ScriptFile = new FileInfo(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Global.CmsTableDefinition));
            // If Script File not exist in the folder break further execution of code  
            if (!ScriptFile.Exists)
            {
                RetVal = false;
                return RetVal;
            }
            // This Script file contains sql syntax to creates datatables, if datatables are not already existing in database
            DbScripts = ScriptFile.OpenText().ReadToEnd().Replace("DBName", CMSDatabaseName);//.Replace("GO", "\nGO");
            if (DbScripts.Contains("--GO"))
            {
                List<string> ListQuery = new List<string>();
                ListQuery.AddRange(Global.SplitString(DbScripts, "--GO"));
                foreach (string StrQuery in ListQuery)
                {
                    // Execute script file to create database Tables
                    ObjDIConnection.ExecuteNonQuery(StrQuery);
                }
            }
            else
            {
                ObjDIConnection.ExecuteNonQuery(DbScripts);
            }
            // Close file object
            ScriptFile.OpenText().Close();
            // Set Retval to true
            RetVal = true;
            // Dispose connection object
            ObjDIConnection.Dispose();
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);
        }

        finally
        { // dispose connection object
            ObjDIConnection = null;
            ScriptFile = null;
        }
        return RetVal;
    }

}
