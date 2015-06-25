using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Drawing;
using Ionic.Zip;
using System.Web.Hosting;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Xml.Serialization;
using SDMXObjectModel.Registry;
using SDMXObjectModel;
using System.Net;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel.Common;
using SDMXObjectModel.Structure;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net.Mail;
using SDMXObjectModel.Message;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


/// <summary>
/// Summary description for Global
/// </summary>
public static class Global
{
    #region "--Private--"

    #region "--Methods--"

    private static Hashtable readAllKeyValsInXML(string LangFilePath)
    {
        Hashtable AllKeyVals = new Hashtable();
        XmlTextReader reader = new XmlTextReader(LangFilePath);

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element: // The node is an element.
                    if (reader.Name == "Row")
                    {
                        string tmpKey = string.Empty;
                        string tmpVal = string.Empty;
                        while (reader.MoveToNextAttribute()) // Read the attributes.
                        {
                            if (reader.Name == "key") tmpKey = reader.Value;
                            else if (reader.Name == "value") tmpVal = reader.Value;
                        }
                        if (!AllKeyVals.ContainsKey(tmpKey))
                        {
                            AllKeyVals.Add(tmpKey, tmpVal);
                        }
                    }
                    break;
                case XmlNodeType.Text: //Display the text in each element.
                    break;
                case XmlNodeType.EndElement: //Display the end of the element.
                    break;
            }

        }
        reader.Close();
        return AllKeyVals;
    }

    ///// <summary>
    ///// To append the error message into the error file.
    ///// </summary>
    ///// <param name="fpath"></param>
    ///// <param name="ErrorString"></param>
    //private static void WriteInErrorLogFile(string fpath, string ErrorString)
    //{
    //    StreamWriter wr = null;
    //    try
    //    {
    //        wr = new StreamWriter(fpath, true);
    //        string str = "";
    //        str = System.DateTime.Now.ToString() + " >> " + ErrorString;
    //        str = str + System.Environment.NewLine;
    //        wr.WriteLine(str);
    //        wr.Flush();
    //        wr.Dispose();

    //    }
    //    catch (Exception ex)
    //    {
    //        Global.CreateExceptionString(ex, null);

    //    }

    //}

    #endregion

    #endregion

    #region "--Public--"

    #region "--Variables--"

    // Application Setting variables
    public static string diuilib_url = string.Empty;
    public static string js_version = string.Empty;
    public static string diuilib_version = string.Empty;
    public static string diuilib_theme_css = string.Empty;
    public static string adaptation_name = string.Empty;
    public static string fbAppID = string.Empty;
    public static string fbAppSecret = string.Empty;
    public static string twAppID = string.Empty;
    public static string twAppSecret = string.Empty;
    public static string hlngcodedb = "en";
    public static string CookiePostfixStr = string.Empty;
    public static string dvMrdThreshold = string.Empty;
    public static string dvHideSource = string.Empty;
    public static string enableQDSGallery = string.Empty;
    public static string googleapikey = string.Empty;
    public static string isQdsGeneratedForDensedQS_Areas = string.Empty;
    public static string enableDSDSelection = string.Empty;
    public static string showdisputed = string.Empty;
    public static string registryAreaLevel = string.Empty;
    public static string registryMSDAreaId = string.Empty;
    public static string registryNotifyViaEmail = string.Empty;
    public static string adapted_for = string.Empty;
    public static string area_nid = string.Empty;
    public static string sub_nation = string.Empty;
    public static string VisibleSlidersCount = string.Empty;
    public static string DbAdmName = string.Empty;
    public static string DbAdmInstitution = string.Empty;
    public static string DbAdmEmail = string.Empty;
    public static string UnicefRegion = string.Empty;
    public static string AdaptationYear = string.Empty;

    public static string standalone_registry = string.Empty;
    public static string registryMappingAgeDefaultValue = string.Empty;
    public static string registryMappingSexDefaultValue = string.Empty;
    public static string registryMappingLocationDefaultValue = string.Empty;
    public static string registryMappingFrequencyDefaultValue = string.Empty;
    public static string registryMappingSourceTypeDefaultValue = string.Empty;
    public static string registryMappingNatureDefaultValue = string.Empty;
    public static string registryMappingUnitMultDefaultValue = string.Empty;
    public static string registryPagingRows = string.Empty;

    public static string customParams = string.Empty;
    public static string GalleryPageSize = string.Empty;

    public static string isInnovationsMenuVisible = string.Empty;
    public static string isNewsMenuVisible = string.Empty;
    public static string isContactUsMenuVisible = string.Empty;
    public static string isSupportMenuVisible = string.Empty;
    public static string show_cloud = string.Empty;
    public static string AdapUserPageSize = string.Empty;
    public static string sliders_list = string.Empty;
    public static string isWorkingOnIP = string.Empty;
    public static string application_version = string.Empty;

    public static string isDownloadsLinkVisible = string.Empty;
    public static string isTrainingLinkVisible = string.Empty;
    public static string isMapLibraryLinkVisible = string.Empty;
    public static string islangFAQVisible = string.Empty;
    public static string islangKBVisible = string.Empty;
    public static string isHowToVideoVisible = string.Empty;
    public static string isSitemapLinkVisible = string.Empty;

    public static string isRSSFeedsLinkVisible = string.Empty;
    public static string isDiWorldWideLinkVisible = string.Empty;

    public static string SliderAnimationSpeed = string.Empty;
    public static string gaUniqueID = string.Empty;
    public static string Country = string.Empty;

    public static string CategoryScheme = string.Empty;
    public static string isShowMapServer = string.Empty;
    public static string ShowWebmasterAccount = string.Empty;

    public static string DropNCreateSitemapURLTable = string.Empty;
    public static string CmsDbCreationFile = string.Empty;
    public static string CmsTableDefinition = string.Empty;
    public static string CmsDataBasePath = string.Empty;
    public static string MaxArticlesCount = string.Empty;
    public static string MetaTag_Desc = string.Empty;
    public static string MetaTag_Kw = string.Empty;
    public static string IsSDMXHeaderCreated = string.Empty;
    public static string IsSDMXDataPublished = string.Empty;

    #endregion

    #region "--Methods--"

    #region "-- Encryption --"

    public static string EncryptString(string text)
    {
        string RetVal;
        DESCryptoServiceProvider CryptoProvider;
        MemoryStream MemoryStream;
        CryptoStream CryptoStream;
        StreamWriter Writer;
        byte[] Bytes;

        RetVal = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {
            Bytes = ASCIIEncoding.ASCII.GetBytes(Constants.EncryptionKey);
            CryptoProvider = new DESCryptoServiceProvider();
            MemoryStream = new MemoryStream(Bytes.Length);
            CryptoStream = new CryptoStream(MemoryStream, CryptoProvider.CreateEncryptor(Bytes, Bytes), CryptoStreamMode.Write);
            Writer = new StreamWriter(CryptoStream);
            Writer.Write(text);
            Writer.Flush();
            CryptoStream.FlushFinalBlock();
            Writer.Flush();

            RetVal = Convert.ToBase64String(MemoryStream.GetBuffer(), 0, (int)MemoryStream.Length);
        }

        return RetVal;
    }

    public static string DecryptString(string text)
    {
        string RetVal;
        DESCryptoServiceProvider CryptoProvider;
        MemoryStream MemoryStream;
        CryptoStream CryptoStream;
        StreamReader Reader;
        byte[] Bytes;

        RetVal = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {
            Bytes = ASCIIEncoding.ASCII.GetBytes(Constants.EncryptionKey);
            CryptoProvider = new DESCryptoServiceProvider();
            MemoryStream = new MemoryStream(Convert.FromBase64String(text));
            CryptoStream = new CryptoStream(MemoryStream, CryptoProvider.CreateDecryptor(Bytes, Bytes), CryptoStreamMode.Read);
            Reader = new StreamReader(CryptoStream);
            RetVal = Reader.ReadToEnd();
        }

        return RetVal;
    }

    public static string OneWayEncryption(string text)
    {
        string RetVal;
        byte[] HashValue;
        UnicodeEncoding UE;
        SHA256Managed Hasher;

        RetVal = string.Empty;
        UE = new UnicodeEncoding();
        Hasher = new SHA256Managed();

        try
        {
            HashValue = Hasher.ComputeHash(UE.GetBytes(text));

            foreach (byte HashByte in HashValue)
            {
                RetVal += String.Format("{0:x2}", HashByte);
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

        return RetVal;
    }

    #endregion

    ///// <summary>
    ///// To maintain the errors in the log file. 
    ///// </summary>
    ///// <param name="ErrorString"></param>
    //public static void WriteErrorsInLog(string ErrorString)
    //{
    //    string ErrorLogFolderPath = string.Empty;
    //    string ErrorFileNameWithPath = null;

    //    ErrorLogFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.ErrorLogs);

    //    //for checking that directory exists , if not then create it.
    //    if (!Directory.Exists(ErrorLogFolderPath))
    //    {
    //        Directory.CreateDirectory(ErrorLogFolderPath);
    //    }

    //    //variable: file path
    //    ErrorFileNameWithPath = Path.Combine(ErrorLogFolderPath, string.Format("{0:yyyyMMdd}", DateTime.Today.Date) + ".txt");

    //    //for checking file exists or not . if not then create the file and append the file
    //    if (!File.Exists(ErrorFileNameWithPath))
    //    {
    //        File.WriteAllText(ErrorFileNameWithPath, " ");
    //        WriteInErrorLogFile(ErrorFileNameWithPath, ErrorString);
    //    }
    //    else
    //    {
    //        WriteInErrorLogFile(ErrorFileNameWithPath, ErrorString);
    //    }

    //}

    /// <summary>
    /// Get the cookie name after appending adaptataion name
    /// </summary>
    /// <param name="cookieName"></param>
    /// <returns></returns>
    public static string GetCookieNameByAdapatation(string cookieName)
    {
        return cookieName + "_" + CookiePostfixStr;
    }

    /// <summary>
    /// Delete a directory
    /// </summary>
    /// <param name="dirPath"></param>
    /// <returns></returns>
    public static bool DeleteDirectory(string dirPath, string DSDDelete = null)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                try
                {
                    ClearAttributes(dirPath);
                    // Directory.Delete(dirPath, true);
                    if (string.IsNullOrEmpty(DSDDelete) == false)
                    {
                        DirectoryInfo Folder = new DirectoryInfo(dirPath);
                        DeleteFilesAndFolders(Folder);

                    }
                    else
                    {
                        Directory.Delete(dirPath, true);
                    }



                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                    return false;

                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }


    public static bool DeleteFilesAndFolders(DirectoryInfo DirectoryName)
    {
        bool RetVal = false;
        string dirname = DirectoryName.Name;
        DirectoryName.Attributes = System.IO.FileAttributes.Normal;
        foreach (FileInfo fl in DirectoryName.GetFiles())
        {
            fl.Delete();
        }
        foreach (DirectoryInfo Dir in DirectoryName.GetDirectories())
        {
        
            DeleteFilesAndFolders(Dir);

        }
        DirectoryName.Delete();
        return RetVal;
    }



    public static bool MoveDirectory(string dirPath, string tempPath)
    {
        try
        {
            Directory.Move(dirPath, tempPath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }


    /// <summary>
    /// Clear attributes of files in a directory recusivily
    /// </summary>
    /// <param name="currentDir"></param>
    public static void ClearAttributes(string currentDir)
    {
        if (Directory.Exists(currentDir))
        {
            string[] subDirs = Directory.GetDirectories(currentDir);
            foreach (string dir in subDirs)
                ClearAttributes(dir);
            string[] files = files = Directory.GetFiles(currentDir);
            foreach (string file in files)
                File.SetAttributes(file, FileAttributes.Normal);
        }
    }

    /// <summary>
    /// Copy all files from source directroy to target directory
    /// </summary>
    /// <param name="sourcePath"></param>
    /// <param name="targetPath"></param>
    public static void CopyDirectoryFiles(string sourcePath, string targetPath)
    {
        string SrcFileName = string.Empty;
        string DestFile = string.Empty;

        if (Directory.Exists(sourcePath))
        {
            string[] Files = Directory.GetFiles(sourcePath);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // Copy the files and overwrite destination files if they already exist.
            foreach (string CurrentFile in Files)
            {
                // Use static Path methods to extract only the file name from the path.
                SrcFileName = Path.GetFileName(CurrentFile);
                DestFile = Path.Combine(targetPath, SrcFileName);
                if (!File.Exists(DestFile))
                {
                    File.Copy(CurrentFile, DestFile, true);
                }
            }
        }
    }


    /// <summary>
    /// Set the key values from xml file
    /// </summary>
    /// <param name="AppSettingFileObj"></param>
    /// <param name="KeyName"></param>
    /// <returns></returns>
    public static void GetAppSetting()
    {
        string AppSettingFile = string.Empty;
        string AppVersionFile = string.Empty;
        XmlDocument XmlDoc;
        XmlDocument XmlDocVersion;

        try
        {
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            diuilib_url = GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_url);

            js_version = GetNodeValue(XmlDoc, Constants.AppSettingKeys.js_version);
            diuilib_version = GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_version);
            diuilib_theme_css = GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_theme_css);
            adaptation_name = GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_name);
            fbAppID = GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppID);
            fbAppSecret = GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppSecret);
            twAppID = GetNodeValue(XmlDoc, Constants.AppSettingKeys.twAppID);
            twAppSecret = GetNodeValue(XmlDoc, Constants.AppSettingKeys.twAppSecret);

            CookiePostfixStr = Global.adaptation_name.Replace(" ", "");

            dvHideSource = GetNodeValue(XmlDoc, Constants.AppSettingKeys.dvHideSource);
            dvMrdThreshold = GetNodeValue(XmlDoc, Constants.AppSettingKeys.dvMrdThreshold);
            enableQDSGallery = GetNodeValue(XmlDoc, Constants.AppSettingKeys.enableQDSGallery);
            googleapikey = GetNodeValue(XmlDoc, Constants.AppSettingKeys.googleapikey);


            isQdsGeneratedForDensedQS_Areas = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isQdsGeneratedForDensedQS_Areas);
            enableDSDSelection = GetNodeValue(XmlDoc, Constants.AppSettingKeys.enableDSDSelection);
            showdisputed = GetNodeValue(XmlDoc, Constants.AppSettingKeys.showdisputed);
            registryAreaLevel = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryAreaLevel);
            registryMSDAreaId = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId);
            registryNotifyViaEmail = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryNotifyViaEmail);
            adapted_for = GetNodeValue(XmlDoc, Constants.AppSettingKeys.adapted_for);
            area_nid = GetNodeValue(XmlDoc, Constants.AppSettingKeys.area_nid);
            sub_nation = GetNodeValue(XmlDoc, Constants.AppSettingKeys.sub_nation);
            VisibleSlidersCount = GetNodeValue(XmlDoc, Constants.AppSettingKeys.VisibleSlidersCount);

            DbAdmName = GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmName);
            DbAdmInstitution = GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmInstitution);
            DbAdmEmail = GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmEmail);
            UnicefRegion = GetNodeValue(XmlDoc, Constants.AppSettingKeys.unicef_region);
            AdaptationYear = GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_year);

            standalone_registry = GetNodeValue(XmlDoc, Constants.AppSettingKeys.standalone_registry);
            registryMappingAgeDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingAgeDefaultValue);
            registryMappingSexDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingSexDefaultValue);
            registryMappingLocationDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingLocationDefaultValue);
            registryMappingFrequencyDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingFrequencyDefaultValue);
            registryMappingSourceTypeDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingSourceTypeDefaultValue);
            registryMappingNatureDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingNatureDefaultValue);
            registryMappingUnitMultDefaultValue = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingUnitMultDefaultValue);
            registryPagingRows = GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryPagingRows);

            customParams = GetNodeValue(XmlDoc, Constants.AppSettingKeys.customParams);
            GalleryPageSize = GetNodeValue(XmlDoc, Constants.AppSettingKeys.GalleryPageSize);

            isNewsMenuVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isNewsMenuVisible);
            isInnovationsMenuVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isInnovationsMenuVisible);
            isContactUsMenuVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isContactUsMenuVisible);
            isSupportMenuVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isSupportMenuVisible);
            show_cloud = GetNodeValue(XmlDoc, Constants.AppSettingKeys.show_cloud);
            AdapUserPageSize = GetNodeValue(XmlDoc, Constants.AppSettingKeys.AdapUserPageSize);
            sliders_list = GetNodeValue(XmlDoc, Constants.AppSettingKeys.sliders_list);
            isWorkingOnIP = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isWorkingOnIP);

            isDownloadsLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isDownloadsLinkVisible);
            isTrainingLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isTrainingLinkVisible);
            isMapLibraryLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isMapLibraryLinkVisible);
            islangFAQVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.islangFAQVisible);
            islangKBVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.islangKBVisible);
            isHowToVideoVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isHowToVideoVisible);
            isSitemapLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isSitemapVisible);
            isRSSFeedsLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isRSSFeedsLinkVisible);
            isDiWorldWideLinkVisible = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isDiWorldWideLinkVisible);
            AppVersionFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppVersionFile]);
            XmlDocVersion = new XmlDocument();
            XmlDocVersion.Load(AppVersionFile);
            application_version = GetNodeValue(XmlDocVersion, Constants.AppSettingKeys.app_version);
            SliderAnimationSpeed = GetNodeValue(XmlDoc, Constants.AppSettingKeys.SliderAnimationSpeed);
            gaUniqueID = GetNodeValue(XmlDoc, Constants.AppSettingKeys.gaUniqueID);
            Country = GetNodeValue(XmlDoc, Constants.AppSettingKeys.Country);
            ShowWebmasterAccount = GetNodeValue(XmlDoc, Constants.AppSettingKeys.ShowWebmasterAccount);
            CategoryScheme = GetNodeValue(XmlDoc, Constants.AppSettingKeys.CategoryScheme);
            isShowMapServer = GetNodeValue(XmlDoc, Constants.AppSettingKeys.isShowMapServer);

            DropNCreateSitemapURLTable = GetNodeValue(XmlDoc, Constants.AppSettingKeys.DropNCreateSitemapURLTable);
            CmsDbCreationFile = GetNodeValue(XmlDoc, Constants.AppSettingKeys.CmsDbCreationFile);
            CmsTableDefinition = GetNodeValue(XmlDoc, Constants.AppSettingKeys.CmsTableDefinition);
            CmsDataBasePath = GetNodeValue(XmlDoc, Constants.AppSettingKeys.CmsDataBasePath);
            MaxArticlesCount = GetNodeValue(XmlDoc, Constants.AppSettingKeys.MaxArticlesCount);
            MetaTag_Desc = GetNodeValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Desc);
            MetaTag_Kw = GetNodeValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Kw);
        //    IsSDMXHeaderCreated = GetNodeValue(XmlDoc, Constants.AppSettingKeys.IsSDMXHeaderCreated);
            IsSDMXDataPublished = GetNodeValue(XmlDoc, Constants.AppSettingKeys.IsSDMXDataPublished);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
    }

    /// <summary>
    /// Get the value of specified key
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public static string GetNodeValue(XmlDocument xmlDoc, string keyName)
    {
        string RetVal = string.Empty;

        try
        {
            RetVal = xmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + keyName + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value;
        }
        catch (Exception ex)
        {
            Global.WriteErrorsInLogFolder("key" + keyName + " Not found");
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }
    /// <summary>
    /// To Check if the appsetting already exists or not, if not then add it with suitable value
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public static string CheckAppSetting(XmlDocument xmlDocument, string keyName, string keyValue)
    {
        string RetVal = string.Empty;
        XmlDocument XmlDoc;
        string AppSettingsFile = string.Empty;

        try
        {
            if (xmlDocument.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + keyName + "']") == null)
            {
                AppSettingsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
                XmlDoc = new XmlDocument();
                XmlDoc.Load(AppSettingsFile);
                XmlNode xNode = XmlDoc.CreateNode(XmlNodeType.Element, "item", "");
                XmlAttribute xKey = XmlDoc.CreateAttribute("n");
                XmlAttribute xValue = XmlDoc.CreateAttribute("v");
                xKey.Value = keyName;
                xValue.Value = keyValue;
                xNode.Attributes.Append(xKey);
                xNode.Attributes.Append(xValue);
                XmlDoc.GetElementsByTagName("appsettings")[0].InsertAfter(xNode,
                XmlDoc.GetElementsByTagName("appsettings")[0].LastChild);
                File.SetAttributes(AppSettingsFile, FileAttributes.Normal);
                XmlDoc.Save(AppSettingsFile);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
    /// <summary>
    /// Returns splitted string values
    /// </summary>
    /// <param name="valueString"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static string[] SplitString(string valueString, string delimiter)
    {
        string[] RetVal;
        int Index = 0;
        string Value;
        List<string> SplittedList = new List<string>();

        while (true)
        {
            Index = valueString.IndexOf(delimiter);
            if (Index == -1)
            {
                if (!string.IsNullOrEmpty(valueString))
                {
                    SplittedList.Add(valueString);
                }
                break;
            }
            else
            {
                Value = valueString.Substring(0, Index);
                valueString = valueString.Substring(Index + delimiter.Length);
                SplittedList.Add(Value);

            }

        }

        RetVal = SplittedList.ToArray();

        return RetVal;
    }
    /// <summary>
    /// Replaces single quotes
    /// </summary>
    /// <param name="SingleQuotedString"></param>
    /// <returns></returns>
    public static string formatQuoteString(string SingleQuotedString)
    {
        string result = string.Empty;

        result = SingleQuotedString.Replace(@"'", @"\'");

        while (result.IndexOf(@"\\'") != -1)
        {
            result = result.Replace(@"\\'", @"\'");
        }

        return result;
    }
    /// <summary>
    /// Returns splitted string values
    /// </summary>
    /// <param name="valueString"></param>
    /// <param name="delimiter"></param>
    /// <returns>List of string</returns>
    public static List<string> SplitStringInList(string valueString, string delimiter)
    {
        List<string> RetVal = new List<string>();
        int Index = 0;
        string Value;

        while (true)
        {
            Index = valueString.IndexOf(delimiter);
            if (Index == -1)
            {
                if (!string.IsNullOrEmpty(valueString))
                {
                    RetVal.Add(valueString);
                }
                break;
            }
            else
            {
                Value = valueString.Substring(0, Index);
                valueString = valueString.Substring(Index + delimiter.Length);

                if (!string.IsNullOrEmpty(Value))
                {
                    RetVal.Add(Value);
                }
            }

        }

        return RetVal;
    }

    /// <summary>
    /// Get database connection of selected database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static DIConnection GetDbConnection(int dbNid)
    {
        DIConnection ObjDIConnection = null;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        string ServerType = string.Empty;
        string ServerName = string.Empty;
        string DbName = string.Empty;
        string UserName = string.Empty;
        string Password = string.Empty;
        string PostNo = string.Empty;

        string DBConn = string.Empty;
        string[] DBConnArr;

        try
        {
            DBConnectionsFile = Path.Combine(HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DBConn = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value;

            DBConnArr = SplitString(DBConn, "||"); //DBConn.Split("||".ToCharArray());//

            ServerType = DBConnArr[0];
            ServerName = DBConnArr[1];
            DbName = DBConnArr[2];
            UserName = DBConnArr[3];

            if (DBConnArr.Length > 4)
            {
                Password = Global.DecryptString(DBConnArr[4]);
            }

            ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(ServerType)), ServerName, PostNo, DbName, UserName, Password);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return ObjDIConnection;
    }

    /// <summary>
    /// Get Default Database NID
    /// </summary>
    /// <returns></returns>
    public static string GetDefaultDbNId()
    {
        string RetVal = string.Empty;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);
            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root);
            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.Default].Value;
            //  RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value; 
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Get Default DSD NID
    /// </summary>
    /// <returns></returns>
    public static string GetDefaultDSDNId()
    {
        string RetVal = string.Empty;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);
            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root);
            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get Default DSD NID
    /// </summary>
    /// <returns></returns>
    public static string GetDataBaseConnectionName()
    {
        string RetVal = string.Empty;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);
            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root);
            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.RootAttributes.DefaultDSD].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Save cookie data
    /// </summary>
    /// <param name="cookieName"></param>
    /// <param name="value"></param>
    /// <param name="page"></param>
    public static void SaveCookie(string cookieName, string value, Page page)
    {
        try
        {
            HttpCookie Cookie = new HttpCookie(cookieName);
            Cookie.Value = value;
            page.Response.Cookies.Add(Cookie);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
    }

    /// <summary>
    /// Get Default language code
    /// </summary>
    /// <returns></returns>
    public static string GetDefaultLanguageCode()
    {
        string RetVal = string.Empty;
        string LngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string language = string.Empty;
        string defBrowserLng = string.Empty;
        bool isLngExists = false;
        try
        {
            language = HttpContext.Current.Request.Headers["Accept-Language"].ToString();
            if (language.Length > 1)
            {
                defBrowserLng = language.Substring(0, 2).ToLower();
            }
            else
            {
                defBrowserLng = language.ToLower();
            }
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LngFile);
            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root);
            foreach (XmlNode lngNode in xmlNode.ChildNodes)
            {

                if (lngNode.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Code].Value.ToLower() == defBrowserLng)
                {
                    isLngExists = true;
                    RetVal = defBrowserLng;
                    break;
                }
            }
            if (!isLngExists)
                RetVal = xmlNode.Attributes[Constants.XmlFile.Language.Tags.RootAttributes.Default].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Get Default language code of selected dbnid
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetDefaultLanguageCodeDB(string dbNId, string lngCode)
    {
        string RetVal = string.Empty;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNId + "]");

            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage].Value;

            if (!string.IsNullOrEmpty(lngCode))
            {
                // Get Avilable Language for dbnid
                string avlLng = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AvailableLanguage].Value;
                if (!string.IsNullOrEmpty(avlLng))
                {
                    string[] LanguageArray = SplitString(avlLng, ",");
                    int SearchIndex = Array.IndexOf(LanguageArray, lngCode);
                    if (SearchIndex > -1)
                    {
                        RetVal = lngCode;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }

    /// <summary>
    /// Get default area of selected database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetDefaultArea(string dbNid)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get attribute value of selected dbnid
    /// </summary>
    /// <param name="dbNid"></param>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    public static string GetDbXmlAttributeValue(string dbNid, string attributeName)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            RetVal = xmlNode.Attributes[attributeName].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }


    public static string SetDBXmlAttributes(string dbNId, string attributeName,string attributeValue)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNId + "]");

            xmlNode.Attributes[attributeName].Value = attributeValue;

            File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
            XmlDoc.Save(DBConnectionsFile);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    /// <summary>
    /// Get default Indicator of selected database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetDefaultIndicator(string dbNid)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            string defaultIndJson = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON].Value;

            var jss = new JavaScriptSerializer();
            dynamic JsonData = jss.Deserialize<dynamic>(defaultIndJson);

            foreach (string IU in JsonData["iu"])
            {
                int indexOfSeparator = IU.IndexOf("~");
                RetVal += "," + IU.Substring(0, indexOfSeparator);
            }

            if (!string.IsNullOrEmpty(RetVal)) RetVal = RetVal.Substring(1);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get default Indicator of selected database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetDefaultIusNIds(string dbNid)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            RetVal = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator].Value;

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get selected database details (name, summary, desc)
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string[] GetDbDetails(string dbNid, string lngCode)
    {
        string[] RetVal;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        List<string> DbDetailsList = new List<string>();

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Count].Value);
            DbDetailsList.Add(xmlNode.Attributes["desc_" + lngCode].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LastModified].Value);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        RetVal = DbDetailsList.ToArray();

        return RetVal;
    }

    /// <summary>
    /// Get default area count of selected database NId
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static int GetDefaultAreaCount(string dbNid)
    {
        int RetVal = 0;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            RetVal = int.Parse(XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]").Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaCount].Value);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Get category name
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetSelectedCategoryName(string dbNid)
    {
        string RetVal = string.Empty;

        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            //RetVal = XmlDoc.SelectSingleNode("/dbinfo/category/db[@id=" + dbNid + "]").Attributes["defac"].Value;

            RetVal = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]").ParentNode.Attributes[Constants.XmlFile.Db.Tags.CategoryAttributes.Name].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get selected database connection details
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string[] GetDbConnectionDetails(string dbNid)
    {
        string[] RetVal;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string Deflng = string.Empty;
        List<string> DbDetailsList = new List<string>();

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea].Value);

            Deflng = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultLanguage].Value;
            DbDetailsList.Add(xmlNode.Attributes["desc_" + Deflng].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        RetVal = DbDetailsList.ToArray();

        return RetVal;
    }

    /// <summary>
    /// Get selected database connection name
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string GetDefDBConnName(string dbNid)
    {
        string RetVal = string.Empty;
        string DataBaseConnName = string.Empty;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        string Deflng = string.Empty;
        List<string> DbDetailsList = new List<string>();

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DataBaseConnName = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        RetVal = DataBaseConnName;

        return RetVal;
    }

    /// <summary>
    /// Get all connection name and its id for a category
    /// </summary>
    /// <param name="categoryName"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetAllConnections(string categoryName)
    {
        Dictionary<string, string> ConnDict = new Dictionary<string, string>();
        string DBFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        int i = 0;

        try
        {
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "[@" + Constants.XmlFile.Db.Tags.CategoryAttributes.Name + "='" + categoryName + "']");

            for (i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                ConnDict.Add(xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value, xmlNode.ChildNodes[i].Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return ConnDict;
    }

    /// <summary>
    /// Get selected database details (CategoryName, ConnectinName, ConnectionDetails, description)
    /// </summary>
    /// <param name="dbNid"></param>
    /// <returns></returns>
    public static string[] GetDbNConnectionDetails(string dbNid, string lngCode)
    {
        string[] RetVal;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;
        List<string> DbDetailsList = new List<string>();

        string DBConn = string.Empty;
        string[] DBConnArr;

        try
        {
            DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNid + "]");

            DbDetailsList.Add(xmlNode.ParentNode.Attributes[Constants.XmlFile.Db.Tags.CategoryAttributes.Name].Value);
            DbDetailsList.Add(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value);

            DBConn = xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DatabaseConnection].Value;
            DBConnArr = SplitString(DBConn, "||");

            DbDetailsList.Add(DBConnArr[0]);    //DatabaseType
            DbDetailsList.Add(DBConnArr[1]);    //ServerName
            DbDetailsList.Add(DBConnArr[2]);    //DbName
            DbDetailsList.Add(DBConnArr[3]);    //UserName

            if (DBConnArr.Length > 4)
            {
                DbDetailsList.Add(Global.DecryptString(DBConnArr[4]));    //Password
            }
            else
            {
                DbDetailsList.Add("");
            }

            if (!string.IsNullOrEmpty(lngCode))
            {
                DbDetailsList.Add(xmlNode.Attributes["desc_" + lngCode].Value);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        RetVal = DbDetailsList.ToArray();

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="KeyValsRelativePath">@"~\Stock\language\en\DI_English [en].xml"</param>
    /// <param name="PageMappingRelativePath">@"~\Stock\language\PageKeyMapping_DataView.xml"</param>
    /// <returns></returns>
    public static string getClientLangXML(string KeyValsRelativePath, string PageMappingRelativePath)
    {
        XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document

        Hashtable AllKeyVals = readAllKeyValsInXML(KeyValsRelativePath);
        XmlTextReader reader = new XmlTextReader(PageMappingRelativePath);

        XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
        doc.AppendChild(dec);// Create the root element
        XmlElement root = doc.CreateElement("Language");

        try
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name == "Associate")
                        {
                            XmlElement lng = doc.CreateElement("lng");
                            while (reader.MoveToNextAttribute()) // Read the attributes.
                            {
                                if (reader.Name == "MasterKey")
                                {

                                    if (AllKeyVals.Contains(reader.Value))
                                    {
                                        lng.SetAttribute("val", AllKeyVals[reader.Value].ToString());

                                    }
                                }
                                else if (reader.Name == "ElementID")
                                {
                                    lng.SetAttribute("id", reader.Value);
                                }
                                else if (reader.Name == "ElementProperty")
                                {
                                    lng.SetAttribute("prop", reader.Value);
                                }
                                else
                                {
                                    lng.SetAttribute(reader.Name, reader.Value);
                                }
                            }
                            root.AppendChild(lng);
                        }
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        break;
                }
            }
            reader.Close();
            doc.AppendChild(root);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, "KeyValsRelativePath :" + KeyValsRelativePath + "reader.Name : " + reader.Name + " reader.Value : " + reader.Value);
        }

        return doc.OuterXml;
    }

    /// <summary>
    /// Get show slider value from appsettings.xml file
    /// </summary>
    /// <returns></returns>
    public static string GetShowSlidersValue()
    {
        string RetVal = string.Empty;
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;

        try
        {
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            RetVal = GetNodeValue(XmlDoc, Constants.AppSettingKeys.show_sliders);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public static bool isNumeric(string val, NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    public static string GetHexColorCode(Color colorValue)
    {
        string RetVal = string.Empty;
        RetVal = String.Format("#{0:X2}{1:X2}{2:X2}", colorValue.R, colorValue.G, colorValue.B);
        return RetVal;
    }

    public static MemoryStream GetZipFileStream(MemoryStream tStream, string fileNameWExt)
    {
        MemoryStream RetVal = new MemoryStream();
        try
        {
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                tStream.Position = 0;

                //use image for image type PNG and for files use files for txt files no need to mensiopn
                zip.AddEntry(fileNameWExt, string.Empty, tStream);
                zip.Save(RetVal);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    public static MemoryStream GetZipFileStream(Dictionary<string, MemoryStream> dicStreamFileName, string fileExt)
    {
        MemoryStream RetVal = new MemoryStream();
        try
        {
            using (ZipFile zip = new ZipFile(Encoding.UTF8))
            {
                foreach (string key in dicStreamFileName.Keys)
                {
                    string fileNameWExt = key;
                    MemoryStream tStream = dicStreamFileName[key];
                    tStream.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry(fileNameWExt + "." + fileExt, string.Empty, tStream);
                }
                zip.Save(RetVal);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    public static MemoryStream GetZipFileStream(MemoryStream tStream, string fileNameWExt, string tempPath)
    {
        MemoryStream RetVal = new MemoryStream();
        List<string> FileName = new List<string>();
        string outputFileName = string.Empty;

        try
        {
            outputFileName = Path.Combine(tempPath, Path.GetFileNameWithoutExtension(fileNameWExt));

            Global.SaveMemoryStreamIntoFile(tStream, outputFileName + ".xls");
            FileName.Add(outputFileName + ".xls");

            ZipFileManager.ZipFiles(FileName, outputFileName + ".zip");

            RetVal = Global.GetMemoryStreamFromFile(outputFileName + ".zip");

            //to delete delete both files
            Global.deleteFiles(outputFileName + ".xls" + "," + outputFileName + ".zip");

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);


        }
        return RetVal;
    }

    public static void deleteFiles(string fileNames)
    {
        if (!string.IsNullOrEmpty(fileNames))
        {
            foreach (string fileName in fileNames.Split(','))
            {
                try
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);


                }
            }
        }
    }

    /// <summary>
    /// Save files from memory stream
    /// </summary>
    /// <param name="ms"></param>
    /// <param name="FileName"></param>
    public static bool SaveMemoryStreamIntoFile(MemoryStream ms, string FileNameWPath)
    {
        bool RetVal = false;
        FileStream outStream = null;

        try
        {
            if (File.Exists(FileNameWPath))
            {
                File.Delete(FileNameWPath);
            }

            outStream = File.OpenWrite(FileNameWPath);
            ms.WriteTo(outStream);
            RetVal = true;
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From xml : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (outStream != null)
            {
                outStream.Flush();
                outStream.Close();
            }
        }
        return RetVal;
    }

    public static MemoryStream GetMemoryStreamFromFile(string filename)
    {
        MemoryStream RetVal = null;
        FileStream fs = null;
        byte[] filebytes;

        try
        {
            fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            filebytes = new byte[fs.Length];
            fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
            RetVal = new MemoryStream(filebytes);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (fs != null)
            {
                fs.Flush();
                fs.Close();
            }
        }
        return RetVal;
    }

    #region "-- Serialize deserialize --"

    public static string GetXmlOfSerializeObject(string TempFileWPath, Object objToSerialize)
    {
        string RetVal = string.Empty;
        try
        {
            Global.SerializeObject(TempFileWPath, objToSerialize);
            RetVal = XmlFileReader.ReadXML(TempFileWPath, string.Empty);

            if (File.Exists(TempFileWPath))
            {
                try
                {
                    File.Delete(TempFileWPath);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetXmlOfSerializeObject Object : " + objToSerialize.GetType().ToString() + "->" + " File:" + TempFileWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From GetXmlOfSerializeObject Object : " + objToSerialize.GetType().ToString() + "->" + " File:" + TempFileWPath + " Error: " + ex.Message);

            throw;
        }
        return RetVal;
    }

    public static Object GetSerializeObjectFromXml(string p_FileNameWPath, Type type)
    {
        Object RetVal = null;
        try
        {
            RetVal = Global.DeSerializeObject(p_FileNameWPath, type);

            if (File.Exists(p_FileNameWPath))
            {
                try
                {
                    File.Delete(p_FileNameWPath);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetSerializeObjectFromXml Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From GetSerializeObjectFromXml Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        return RetVal;
    }

    public static void SerializeObject(string p_FileNameWPath, Object serializeObj)
    {
        FileStream _IO = null;
        try
        {
            string DirPath = string.Empty;
            DirPath = Path.GetDirectoryName(p_FileNameWPath);

            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);

            _IO = new FileStream(p_FileNameWPath, FileMode.Create);
            XmlSerializer _SRZFrmt = new XmlSerializer(serializeObj.GetType());
            _SRZFrmt.Serialize(_IO, serializeObj);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
    }

    public static Object DeSerializeObject(string p_FileNameWPath, Type type)
    {
        Object RetVal = null;
        FileStream _IO = null;
        try
        {
            string DirPath = string.Empty;
            DirPath = Path.GetDirectoryName(p_FileNameWPath);

            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);

            _IO = new FileStream(p_FileNameWPath, FileMode.Open);
            XmlSerializer _SRZFrmt = new XmlSerializer(type);
            RetVal = _SRZFrmt.Deserialize(_IO);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From DeSerializeObject Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From DeSerializeObject Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
        return RetVal;
    }

    #endregion

    /// <summary>
    /// Returns True if DSD has been uploaded from Admin
    /// </summary>
    /// <param name="DbNId"></param>
    /// <returns>bool value</returns>
    public static bool IsDSDUploadedFromAdmin(int DbNId)
    {
        bool Retval;
        Retval = false;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");
            if (string.IsNullOrEmpty(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value))
            {
                Retval = false;
            }
            else
            {
                Retval = Convert.ToBoolean(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);


        }

        return Retval;
    }

    public static bool IsCacheResultGenerated(string dbNId)
    {
        bool Retval = false;
        DIConnection DIConnection;
        try
        {
            DIConnection = Global.GetDbConnection(int.Parse(dbNId));

            DataTable dtCacheRowsCount = DIConnection.ExecuteDataTable("SP_GET_CACHE_COUNT", CommandType.StoredProcedure, null);
            if (!(dtCacheRowsCount.Rows[0]["ROWS_COUNT"] is DBNull))
            {
                int CacheRowsCount = int.Parse(dtCacheRowsCount.Rows[0]["ROWS_COUNT"].ToString());

                if (CacheRowsCount > 0)
                {
                    Retval = true;
                }
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);


        }

        return Retval;
    }

    public static bool IsDbIdExists(string dbNId)
    {
        bool RetVal = false;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode = null;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dbNId + "]");

            if (xmlNode != null)
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public static bool IsDSDIdExists(string dsdNId)
    {
        bool RetVal = false;
        string DbFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode = null;

        try
        {
            DbFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DbFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + dsdNId + "]");

            if (xmlNode != null)
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public static string GetJSONString(DataTable Dt)
    {
        string[] StrDc = new string[Dt.Columns.Count];
        string HeadStr = string.Empty;

        for (int i = 0; i < Dt.Columns.Count; i++)
        {

            StrDc[i] = Dt.Columns[i].Caption;

            HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
        }

        HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);

        StringBuilder Sb = new StringBuilder();
        Sb.Append("{\"" + Dt.TableName + "\" : [");

        for (int i = 0; i < Dt.Rows.Count; i++)
        {

            string TempStr = HeadStr;
            Sb.Append("{");

            for (int j = 0; j < Dt.Columns.Count; j++)
            {

                TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
            }

            Sb.Append(TempStr + "},");
        }

        Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
        Sb.Append("]}");

        return Sb.ToString();
    }

    public static string GetCatalogHtml()
    {
        string RetVal = string.Empty;
        try
        {
            string CatalogTemplateFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FileName.CatalogAdaptationsTemplate);

            RetVal = File.ReadAllText(CatalogTemplateFilePath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }

        return RetVal;
    }

    public static string[] GetStartEndYear(DIConnection objDIConnection)
    {
        string[] RetVal = new string[2];
        DataTable DtYears = null;

        try
        {
            DtYears = objDIConnection.ExecuteDataTable("SP_GET_START_END_YEARS", CommandType.StoredProcedure, null);

            if (DtYears.Rows.Count > 0)
            {
                RetVal[0] = Convert.ToString(DtYears.Rows[0][0]);
                RetVal[1] = Convert.ToString(DtYears.Rows[0][1]);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get the URL of running site/adaptation
    /// </summary>
    /// <returns></returns>
    public static string GetAdaptationUrl()
    {
        string RetVal = string.Empty;

        try
        {
            RetVal = Global.SplitString(HttpContext.Current.Request.Url.OriginalString.ToLower(), "/libraries")[0];
            if (Global.isWorkingOnIP == "false")
            {
                if (RetVal.IndexOf("localhost") == -1 && RetVal.IndexOf("dgps") == -1)
                {
                    if (RetVal.IndexOf("www.") == -1)
                    {
                        //RetVal = RetVal.Replace("://", "://www.");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    private static void SaveAppSettingValue(XmlDocument xmlDoc, string keyName, string value)
    {
        //Variables for creating CSV Logfile 
        string CSVFileMsg = string.Empty;
        string NodeOldValue = string.Empty;
        try
        {
            //NodeOldValue = xmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + keyName + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value;


        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    /// <summary>
    /// Get the URL of running site/adaptation
    /// </summary>
    /// <returns></returns>
    public static string GetAdaptationGUID()
    {
        string RetVal = string.Empty;
        string GUID = string.Empty;
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;
        try
        {

            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.GUID, string.Empty);
           GUID = GetNodeValue(XmlDoc, Constants.AppSettingKeys.GUID);
            if (string.IsNullOrEmpty(GUID))
            {
                //Create and Set a new GUID
                GUID = Guid.NewGuid().ToString();
                XmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + Constants.AppSettingKeys.GUID + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value = GUID;
                File.SetAttributes(AppSettingFile, FileAttributes.Normal);
                XmlDoc.Save(AppSettingFile);
                RetVal = GUID;
            }
            else
            {
                //Return the GUID from AppSettings File
                RetVal = GUID;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }

    /// <summary>
    /// Get key value if key exists, if not exists create key with default value and return default value
    /// </summary>
    /// <param name="Key">key whose value is to read</param>
    /// <param name="DefaultValue">default value of keyto update if key not exists</param>
    /// <returns></returns>
    public static string GetNCreateDefaultAppSettingKey(string Key, string DefaultValue)
    {
        string RetVal = string.Empty;
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;
        try
        {

            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            CheckAppSetting(XmlDoc, Key, DefaultValue);

            RetVal = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + Key + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }
    public static string ReadAppSettingValueByKey(string Key)
    {
        string RetVal = string.Empty;
        //  string GUID = string.Empty;
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;
        try
        {

            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            if (string.IsNullOrEmpty(Key))
            {
                RetVal = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.AppSettings.Tags.Root + "/" + Constants.XmlFile.AppSettings.Tags.Item + "[@" + Constants.XmlFile.AppSettings.Tags.ItemAttributes.Name + "='" + Key + "']").Attributes[Constants.XmlFile.AppSettings.Tags.ItemAttributes.Value].Value;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }

    public static Boolean CheckIsGlobalAdaptation()
    {
        Boolean RetVal = false;
        //string AdaptationUrl = string.Empty;

        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            //AdaptationUrl = Global.GetAdaptationUrl();            
            if (CatalogService.CheckIsGlobalAdaptation(Global.GetAdaptationGUID()))
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;

        //Boolean RetVal = false;
        //string AppSettingFile = string.Empty;
        //XmlDocument XmlDoc;        

        //try
        //{

        //    AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
        //    XmlDoc = new XmlDocument();
        //    XmlDoc.Load(AppSettingFile);

        //    if (GetNodeValue(XmlDoc, Constants.AppSettingKeys.area_nid) == "-1")
        //    {
        //        RetVal = true;
        //    }
        //}
        //catch (Exception ex)
        //{
        //}

        //return RetVal;
    }

    public static Boolean CheckIsDI7ORGAdaptation()
    {
        Boolean RetVal = false;
        //string AdaptationUrl = string.Empty;

        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            if (CatalogService.CheckIsDI7ORGAdaptation(Global.GetAdaptationGUID()))
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get valid file name after stripping reserved character if any
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="replaceChar"></param>
    /// <returns></returns>
    public static string GetValidFileNameURLSupport(string fileName, string replaceChar)
    {
        string RetVal = string.Empty;

        if (!string.IsNullOrEmpty(fileName))
        {
            RetVal = fileName;
            RetVal = RetVal.Replace("\\", replaceChar);
            RetVal = RetVal.Replace("/", replaceChar);
            RetVal = RetVal.Replace("*", replaceChar);
            RetVal = RetVal.Replace("?", replaceChar);
            RetVal = RetVal.Replace("|", replaceChar);
            RetVal = RetVal.Replace("<", replaceChar);
            RetVal = RetVal.Replace(">", replaceChar);
            RetVal = RetVal.Replace(":", replaceChar);
            RetVal = RetVal.Replace("#", replaceChar);
            RetVal = RetVal.Replace("@", replaceChar);
            RetVal = RetVal.Replace("%", replaceChar);
            RetVal = RetVal.Replace("[", replaceChar);
            RetVal = RetVal.Replace("]", replaceChar);
            RetVal = RetVal.Replace("&", replaceChar);
            RetVal = RetVal.Replace("'", replaceChar);
            RetVal = RetVal.Replace(".", replaceChar);
            RetVal = RetVal.Replace("\"", replaceChar);
            RetVal = RetVal.Replace(" ", replaceChar);
            //RetVal = RetVal.Replace("[", "(");
            //RetVal = RetVal.Replace("]", ")");
            //RetVal = RetVal.Replace("&", "&amp;");
            //RetVal = RetVal.Replace("<", "&lt;");
            //RetVal = RetVal.Replace(">", "&gt;");
            //RetVal = RetVal.Replace("\"", "&quot;");
            //RetVal = RetVal.Replace("'", "&apos;");
            RetVal = RetVal.Replace("$", replaceChar);


            //-- long file name trimmed to 200
            if ((RetVal != null) && RetVal.Length > 200) RetVal = RetVal.Substring(0, 200);
        }

        return RetVal;
    }

    /// <summary>
    /// Get valid file name after stripping reserved character if any
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns>replace \ / : * ? " < > |</returns>
    public static string GetValidFileName(string fileName)
    {
        string RetVal = string.Empty;

        if (!string.IsNullOrEmpty(fileName))
        {
            RetVal = fileName;
            RetVal = RetVal.Replace("\\", "_");
            RetVal = RetVal.Replace("/", "_");
            RetVal = RetVal.Replace(":", "_");
            RetVal = RetVal.Replace("*", "_");
            RetVal = RetVal.Replace("?", "_");
            RetVal = RetVal.Replace("\"", "_");
            RetVal = RetVal.Replace("<", "_");
            RetVal = RetVal.Replace(">", "_");
            RetVal = RetVal.Replace("|", "_");

            //-- long file name trimmed to 200
            if ((RetVal != null) && RetVal.Length > 200) RetVal = RetVal.Substring(0, 200);
        }

        return RetVal;
    }

    /// <summary>
    /// Get all available language code from database
    /// </summary>
    /// <param name="diConnection"></param>
    /// <returns></returns>
    public static string[] GetAllAvailableLanguageCode(DIConnection diConnection)
    {
        string[] RetVal;
        DataTable DTAvailableLanguages = null;
        List<string> AvlLanguages = new List<string>();

        try
        {
            DTAvailableLanguages = diConnection.DILanguages(diConnection.DIDataSetDefault());

            foreach (DataRow Row in DTAvailableLanguages.Rows)
            {
                AvlLanguages.Add(Row["Language_Code"].ToString());
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        RetVal = AvlLanguages.ToArray();

        return RetVal;
    }

    /// <summary>
    /// Get key value of current set language file
    /// </summary>
    /// <param name="keyName"></param>
    /// <returns></returns>
    public static string GetLanguageKeyValue(string keyName)
    {
        string RetVal = string.Empty;

        string LanguageCode = "en";
        string LanguageFile = "English [en].xml";
        string LanguageFileWithPath = string.Empty;
        XmlDocument XmlDoc;

        try
        {
            try
            {

                //if (HttpContext.Current.Response.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && !string.IsNullOrEmpty(HttpContext.Current.Response.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value))
                //{
                //    LanguageCode = HttpContext.Current.Response.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
                //}
                //else if (HttpContext.Current.Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value))
                //{
                //    LanguageCode = HttpContext.Current.Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
                //}

                LanguageCode = HttpContext.Current.Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
                LanguageFile = Global.GetLanguageFileByLanguageCode(LanguageCode);
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);

            }

            LanguageFileWithPath = Path.Combine(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals), LanguageFile);
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

    /// <summary>
    /// Get language file name by language code
    /// </summary>
    /// <param name="languageCode"></param>
    /// <returns></returns>
    public static string GetLanguageFileByLanguageCode(string languageCode)
    {
        string RetVal = string.Empty;
        string LngFile = string.Empty;
        XmlDocument XmlDocLng;

        try
        {
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDocLng = new XmlDocument();
            XmlDocLng.Load(LngFile);

            RetVal = XmlDocLng.SelectSingleNode("/" + Constants.XmlFile.Language.Tags.Root + "/" + Constants.XmlFile.Language.Tags.Language + "[@" + Constants.XmlFile.Language.Tags.LanguageAttributes.Code + "='" + languageCode + "']").Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value;

            if (!string.IsNullOrEmpty(RetVal))
            {
                RetVal += ".xml";
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public static string GetSearchedKeywords(string KeywordNIds, List<string> SearchWords, string databaseURL)
    {
        string RetVal;
        string GetKeywordsQuery, KeyWord;
        DataTable DtKeywords;
        DIConnection DIConnection;

        RetVal = string.Empty;
        GetKeywordsQuery = "SELECT * FROM keywords WHERE keyword_nid IN (" + KeywordNIds + ") And keyword_type='UDK'";
        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, databaseURL,
                       string.Empty, string.Empty);
        try
        {
            DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
            if (DtKeywords.Rows.Count > 0)
            {
                foreach (DataRow DrKeyWord in DtKeywords.Rows)
                {
                    KeyWord = DrKeyWord["keyword"].ToString();
                    RetVal += KeyWord + "||";
                }
            }
            else
            {
                GetKeywordsQuery = "SELECT * FROM keywords WHERE keyword_nid IN (" + KeywordNIds + ")";
                DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
                if (DtKeywords.Rows.Count > 0)
                {
                    foreach (DataRow DrKeyWord in DtKeywords.Rows)
                    {
                        KeyWord = DrKeyWord["keyword"].ToString();
                        RetVal += KeyWord + "||";
                    }
                }
            }

            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Substring(0, RetVal.Length - 2);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    #region "-- Registry --"

    #region "-- Public --"

    public static void Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(string UserNId, string Language)
    {
        string ProviderFileName, UserFolder, UserFullName;
        bool IsAlreadyExistingProvider;

        IsAlreadyExistingProvider = false;

        try
        {
            ProviderFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
            UserFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
            UserFullName = Get_User_Full_Name(UserNId);
            IsAlreadyExistingProvider = Is_Already_Existing_Provider(UserNId);

            if (IsAlreadyExistingProvider == false)
            {
                if (File.Exists(ProviderFileName))
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, ProviderFileName, UserTypes.Provider, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, UserFullName, Language, string.Empty);
                }
                else
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserTypes.Provider, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, UserFullName, Language, UserFolder);
                }

                Create_Other_Artefacts_And_Update_Folder_Structures_For_Provider_Per_Database(UserNId);
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

    public static void Create_Consumer_In_DCScheme_And_Update_Folder_Structures_Per_Database(string UserNId, string Language)
    {
        string ConsumerFileName, UserFolder, UserFullName;
        bool IsAlreadyExistingConsumer;

        IsAlreadyExistingConsumer = false;

        try
        {
            ConsumerFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
            UserFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
            UserFullName = Get_User_Full_Name(UserNId);
            IsAlreadyExistingConsumer = Is_Already_Existing_Consumer(UserNId);

            if (IsAlreadyExistingConsumer == false)
            {
                if (File.Exists(ConsumerFileName))
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, ConsumerFileName, UserTypes.Consumer, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId, UserFullName, Language, string.Empty);
                }
                else
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserTypes.Consumer, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId, UserFullName, Language, UserFolder);
                }

                Create_Other_Artefacts_And_Update_Folder_Structures_For_Consumer_Per_Database(UserNId);
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

    public static void Create_Other_Artefacts_And_Update_Folder_Structures_For_Provider_Per_Database(string UserNId)
    {
        DataTable DtRegisteredDatabases, DtTable;
        DIConnection DIConnection;
        string AgencyId, OutputFolder;
        string Query, InsertQuery;
        List<ArtefactInfo> PAArtefacts;

        DtRegisteredDatabases = null;
        DtTable = null;
        DIConnection = null;
        AgencyId = string.Empty;
        OutputFolder = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
            DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

            foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
            {
                #region "--PA--"

                AgencyId = Get_AgencyId_From_DFD(DrRegisteredDatabases["DBNId"].ToString());
                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Provisioning Metadata\\PAs");
                PAArtefacts = SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, true, DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, AgencyId, null, OutputFolder);

                if (!Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DrRegisteredDatabases["DBNId"].ToString())))
                {
                    PAArtefacts.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, AgencyId, null, OutputFolder));
                    PAArtefacts.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Indicator.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, AgencyId, null, OutputFolder));
                    PAArtefacts.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DevInfo.Lib.DI_LibSDMX.Constants.MFD.Source.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, AgencyId, null, OutputFolder));
                }
                else
                {
                    Query = "SELECT DISTINCT Id FROM Artefacts WHERE DBNId = " + DrRegisteredDatabases["DBNId"].ToString() + " AND Type = " + Convert.ToInt32(ArtefactTypes.MFD).ToString();
                    DtTable = DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrTable in DtTable.Rows)
                    {
                        PAArtefacts.AddRange(SDMXUtility.Generate_PA(SDMXSchemaType.Two_One, false, DrTable["Id"].ToString(),
                                             DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, AgencyId, null, OutputFolder));
                    }
                }

                foreach (ArtefactInfo PAArtefact in PAArtefacts)
                {
                    InsertQuery = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation) VALUES(" + DrRegisteredDatabases["DBNId"].ToString() + ",'" + PAArtefact.Id + "','" + PAArtefact.AgencyId + "','" + PAArtefact.Version + "','" + string.Empty + "'," + Convert.ToInt32(PAArtefact.Type) + ",'" + Path.Combine(OutputFolder, PAArtefact.FileName) + "');";
                    DIConnection.ExecuteDataTable(InsertQuery);
                }

                #endregion "--PA--"

                #region "--Registration--"

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Registrations\\" + UserNId);
                Create_Directory_If_Not_Exists(OutputFolder);

                #endregion "--Registration--"

                #region "--Constraint--"

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Constraints\\" + UserNId);
                Create_Directory_If_Not_Exists(OutputFolder);

                #endregion "--Constraint--"

                #region "--Subscription--"

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                Create_Directory_If_Not_Exists(OutputFolder);

                #endregion "--Subscription--"
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    public static void Create_Constraint_Artefact_For_Version_2_0_SDMLMLFile(string RegistrationId, string DbNId, string UserNId, string AgencyId, string FileURL)
    {
        string InsertQuery, OutputFolder;
        DIConnection DIConnection;
        XmlDocument SimpleDataFileXML;
        XmlNodeList ObjXmlNodeList;
        SDMXObjectModel.Message.StructureType ConstraintStructure;
        SDMXObjectModel.Structure.ContentConstraintType ContentConstraint;
        DataKeySetType DataKeySet;
        DataKeyValueType DataKeyValue;
        int KeyIndex;

        string SimpleDataFileUrl = string.Empty;
        string ConstraintFileName = string.Empty;
        string ConstraintFileLocation = string.Empty;

        InsertQuery = string.Empty;
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Constraints\\" + UserNId);
        ConstraintFileName = DevInfo.Lib.DI_LibSDMX.Constants.Constraint.Prefix + RegistrationId + ".xml";
        ConstraintFileLocation = OutputFolder + "\\" + ConstraintFileName;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            SimpleDataFileXML = new XmlDocument();
            if (!String.IsNullOrEmpty(FileURL))
            {
                SimpleDataFileXML.Load(FileURL);
            }
            ObjXmlNodeList = SimpleDataFileXML.GetElementsByTagName("sts:Series");
            ConstraintStructure = new SDMXObjectModel.Message.StructureType();
            ConstraintStructure.Structures = new StructuresType();
            ConstraintStructure.Structures.Constraints = new List<ConstraintType>();

            ContentConstraint = new SDMXObjectModel.Structure.ContentConstraintType();
            ContentConstraint.id = DevInfo.Lib.DI_LibSDMX.Constants.Constraint.Prefix + RegistrationId;
            ContentConstraint.Name.Add(new TextType(null, DevInfo.Lib.DI_LibSDMX.Constants.Constraint.Name + RegistrationId));
            ContentConstraint.agencyID = AgencyId;
            ContentConstraint.version = DevInfo.Lib.DI_LibSDMX.Constants.Constraint.Version;
            ContentConstraint.Description.Add(new TextType(null, DevInfo.Lib.DI_LibSDMX.Constants.Constraint.Description));
            ContentConstraint.Annotations = null;
            ContentConstraint.ReleaseCalendar = null;
            ContentConstraint.ConstraintAttachment = new ContentConstraintAttachmentType();

            if (!String.IsNullOrEmpty(FileURL))
            {
                ContentConstraint.ConstraintAttachment.Items = new object[1];
                ContentConstraint.ConstraintAttachment.Items[0] = FileURL;
                ContentConstraint.ConstraintAttachment.ItemsElementName = new ConstraintAttachmentChoiceType[] { ConstraintAttachmentChoiceType.SimpleDataSource };
            }

            DataKeySet = new DataKeySetType();
            DataKeySet.isIncluded = true;
            ContentConstraint.Items.Add(DataKeySet);
            KeyIndex = 0;
            foreach (XmlNode SeriesNode in ObjXmlNodeList)
            {
                ((DataKeySetType)(ContentConstraint.Items[0])).Key.Add(new DataKeyType());
                foreach (XmlAttribute SeriesAttribute in SeriesNode.Attributes)
                {
                    DataKeyValue = new DataKeyValueType();
                    DataKeyValue.id = SeriesAttribute.Name;
                    DataKeyValue.Items.Add(new SimpleKeyValueType());
                    ((SimpleKeyValueType)(DataKeyValue.Items[0])).Value = SeriesAttribute.Value;
                    ((DataKeySetType)(ContentConstraint.Items[0])).Key[KeyIndex].KeyValue.Add(DataKeyValue);
                }
                KeyIndex = KeyIndex + 1;
            }

            ConstraintStructure.Structures.Constraints.Add(ContentConstraint);

            SDMXObjectModel.Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.StructureType), ConstraintStructure, ConstraintFileLocation);
            InsertQuery = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                " VALUES(" + DbNId + ",'" + ContentConstraint.id + "','" + ContentConstraint.agencyID + "','" + ContentConstraint.version + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Constraint).ToString() + ",'" + ConstraintFileLocation + "');";
            DIConnection.ExecuteDataTable(InsertQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    public static bool Is_Already_Existing_Provider(string UserNId)
    {
        bool RetVal;
        string ProviderFileName;
        SDMXObjectModel.Message.StructureType Structure;


        RetVal = false;
        ProviderFileName = string.Empty;
        Structure = null;

        try
        {
            ProviderFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);

            if (File.Exists(ProviderFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ProviderFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null &&
                    Structure.Structures.OrganisationSchemes.Count > 0 &&
                    Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataProviderSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null &&
                    Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataProviderType DataProvider in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        if (DataProvider.id == DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId)
                        {
                            RetVal = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                RetVal = false;
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

        return RetVal;
    }

    public static void Delete_Registration_Artefact(string DbNId, string UserNId, string RegistrationId)
    {
        string Query, FileNameWPath;
        DIConnection DIConnection;

        Query = string.Empty;
        FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Registrations\\" + UserNId + "\\" + RegistrationId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            DIConnection.ExecuteDataTable("DELETE FROM Artefacts WHERE Id = '" + RegistrationId + "' AND DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.Registration).ToString() + ";");

            if (File.Exists(FileNameWPath))
            {
                File.Delete(FileNameWPath);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    public static void Delete_Constraint_Artefact(string DbNId, string UserNId, string RegistrationId)
    {
        string Query, FileNameWPath, ConstraintId;
        DIConnection DIConnection;

        Query = string.Empty;
        ConstraintId = string.Empty;
        ConstraintId = "CNS_" + RegistrationId;
        FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Constraints\\" + UserNId + "\\" + ConstraintId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            DIConnection.ExecuteDataTable("DELETE FROM Artefacts WHERE Id = '" + ConstraintId + "' AND DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.Constraint).ToString() + ";");

            if (File.Exists(FileNameWPath))
            {
                File.Delete(FileNameWPath);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    // Modifying to handle the subscription id for preferred language notification
    public static void Send_Notifications_For_Subscriptions(string DbNId, string UserNId, string RegistrationId, bool IsMetadata, string isDeleteEvent = null)
    {
        Dictionary<string, SubscriptionType> DictSubscriptions;
        List<string> ListRegistrationsOverlap;
        List<string> ListSubscriptionsOverlap;
        bool IsAdminUploadedDSD;
        string OriginalDBNId = string.Empty;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Registry.RegistrationType Registration;
        string RegistrationFileNameWPath;
        string currentRegFileLangCode = String.Empty;
        string preferredLang = String.Empty;
        string langPrefNid = string.Empty;
        IsAdminUploadedDSD = false;
        ListRegistrationsOverlap = new List<string>();
        ListSubscriptionsOverlap = new List<string>();
        string HeaderFilePath = string.Empty;
        try
        {
            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
            OriginalDBNId = Global.GetDefaultDbNId();
            if (OriginalDBNId != DbNId)
            {
                HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            }
            RegistrationFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Registrations\\" + UserNId + "\\" + RegistrationId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

            RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistrationFileNameWPath);
            Registration = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterface.Item).RegistrationRequest[0].Registration;

            // Extract registration file language from DataSource file path.
            currentRegFileLangCode = Registration.Datasource[0].ToString().Substring((Registration.Datasource[0].ToString().IndexOf("SDMX-ML/")) + 8, 2);

            DictSubscriptions = Get_Subscriptions_Dictionary(DbNId, IsMetadata);

            ListRegistrationsOverlap = Get_Registrations_OverlapList(Registration, IsMetadata, IsAdminUploadedDSD);

            foreach (string RegistryURN in DictSubscriptions.Keys)
            {
                ListSubscriptionsOverlap = Get_Subscriptions_OverlapList(DictSubscriptions[RegistryURN], IsMetadata, IsAdminUploadedDSD, DbNId);

                // Send notification if subscriber's preferred language matches registration file language.
                langPrefNid = GetPreferredLanguageFromSubscriptionId(RegistryURN);
                preferredLang = GetLanguageNameFromNid(langPrefNid);

                if ((Are_OverLapping_Lists(ListRegistrationsOverlap, ListSubscriptionsOverlap)) && preferredLang.TrimEnd().EndsWith("[" + currentRegFileLangCode + "]") && IsMetadata == false)
                {
                    if (isDeleteEvent != null)
                    {
                        Send_Delete_Notifications_For_Current_Subscription(DictSubscriptions[RegistryURN], Registration, HeaderFilePath);
                    }
                    else
                    {
                        Send_Notifications_For_Current_Subscription(DictSubscriptions[RegistryURN], Registration, HeaderFilePath);
                    }
                }
                else
                {
                    if ((Are_OverLapping_Lists(ListRegistrationsOverlap, ListSubscriptionsOverlap)) && IsMetadata == true)
                    {

                        if (isDeleteEvent != null)
                        {
                            Send_Delete_Notifications_For_Current_Subscription(DictSubscriptions[RegistryURN], Registration, HeaderFilePath);
                        }
                        else
                        {
                            Send_Notifications_For_Current_Subscription(DictSubscriptions[RegistryURN], Registration, HeaderFilePath);
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

    public static void Send_Notification_By_Email(string MessageContent, string NotificationMailId)
    {
        Send_Email(ConfigurationManager.AppSettings["NotificationSender"].ToString(), ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), NotificationMailId, ConfigurationManager.AppSettings["NotificationSubject"].ToString(), MessageContent);
    }

    public static void Send_Notification_By_HTTP_Post(string MessageContent, string NotificationHTTP)
    {
        WebRequest Request;
        byte[] Message;
        Stream DataStream;

        DataStream = null;

        try
        {
            Request = WebRequest.Create(NotificationHTTP);
            Message = System.Text.Encoding.UTF8.GetBytes(MessageContent);

            Request.Method = "POST";
            Request.ContentType = "text/xml";
            Request.ContentLength = Message.Length;
            //WebResponse Response1 = Request.GetResponse();

            DataStream = Request.GetRequestStream();
            DataStream.Write(Message, 0, Message.Length);
            DataStream.Close();
        }
        catch (Exception ex)
        {
            if (DataStream != null)
            {
                DataStream.Close();
            }
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }
    }

    public static string GetSoapWrappedXml(string content)
    {
        string RetVal;
        SoapFormatter SoapFormatter;
        MemoryStream Stream;
        StreamReader Reader;

        SoapFormatter = new SoapFormatter();
        Stream = new MemoryStream();
        Reader = new StreamReader(Stream);

        SoapFormatter.Serialize(Stream, content);
        Stream.Position = 0;
        RetVal = Reader.ReadToEnd();

        return RetVal;
    }

    public static string Get_User_Full_Name(string UserNId)
    {
        string RetVal;
        string UserDetails;
        string[] UserDetailsParams;

        RetVal = string.Empty;
        UserDetails = string.Empty;
        UserDetailsParams = null;

        try
        {
            UserDetails = GetUserDetails(UserNId);
            UserDetailsParams = Global.SplitString(UserDetails, Constants.Delimiters.ParamDelimiter);

            if (UserDetailsParams != null && UserDetailsParams.Length > 3)
            {
                RetVal = UserDetailsParams[2] + " " + UserDetailsParams[3];
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

        return RetVal;
    }

    public static string Get_User_EmailId(string UserNId)
    {
        string RetVal;
        //string UserDetails;
        //string[] UserDetailsParams;

        RetVal = string.Empty;
        //UserDetails = string.Empty;
        //UserDetailsParams = null;

        try
        {
            RetVal = Get_User_EmailId_ByAdaptationURL(UserNId);
            //UserDetails = GetUserDetails(UserNId);
            //UserDetailsParams = Global.SplitString(UserDetails, Constants.Delimiters.ParamDelimiter);

            //if (UserDetailsParams != null && UserDetailsParams.Length > 0)
            //{
            //    RetVal = UserDetailsParams[0];
            //}
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

    public static string Get_User_EmailId_ByAdaptationURL(string UserNId)
    {
        string RetVal;
        string UserDetails;

        RetVal = string.Empty;
        UserDetails = string.Empty;

        try
        {
            UserDetails = GetUserEmailByAdaptationUrl(UserNId);
            if (!string.IsNullOrEmpty(UserDetails))
            {
                RetVal = UserDetails;

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

        return RetVal;
    }


    private static string GetUserEmailByAdaptationUrl(string requestParam)
    {
        string RetVal;
        string[] Params, UserDetails;
        DataTable DtUser;
        string UserNId, UserEmailId;
        diworldwide_userinfo.UserLoginInformation Service;

        try
        {
            RetVal = string.Empty;
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = Params[0].ToString().Trim();
            Service = new diworldwide_userinfo.UserLoginInformation();
            Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;

            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                RetVal = Service.GetUserDetails(UserNId, Global.GetAdaptationGUID());
                if (!string.IsNullOrEmpty(RetVal))
                {
                    UserDetails = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                    UserEmailId = UserDetails[0];
                    RetVal = UserEmailId;
                }
            }
            else
            {
                DtUser = Get_User(Convert.ToInt32(UserNId));

                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    RetVal = DtUser.Rows[0]["User_Email_id"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    public static string Get_AgencyId_From_DFD(string DbNId)
    {
        string RetVal = string.Empty;

        DataTable DtTable;
        DIConnection DIConnection;

        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
        DtTable = DIConnection.ExecuteDataTable("SELECT AgencyId FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " +
                                                 Convert.ToInt32(ArtefactTypes.DFD).ToString() + ";");

        if (DtTable != null && DtTable.Rows.Count > 0)
        {
            RetVal = DtTable.Rows[0]["AgencyId"].ToString();
        }

        return RetVal;
    }

    public static SDMXObjectModel.Message.BasicHeaderType Get_Appropriate_Header()
    {
        SDMXObjectModel.Message.BasicHeaderType RetVal;
        SenderType Sender;
        PartyType Receiver;

        Sender = new SenderType(DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderId, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderName, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Sender, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderDepartment, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderRole, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage));
        Sender.Contact[0].Items = new string[] { DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderTelephone, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderEmail, DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderFax };
        Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        Receiver = new PartyType(DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverId, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverName, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Receiver, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverDepartment, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverRole, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage));
        Receiver.Contact[0].Items = new string[] { DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverTelephone, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverEmail, DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverFax };
        Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        RetVal = new BasicHeaderType(DevInfo.Lib.DI_LibSDMX.Constants.Header.Id, true, DateTime.Now, Sender, Receiver);

        return RetVal;
    }

    public static bool Validate_DataSource(List<object> DataSource, string CompleteFileNameWPath, int DbNId, bool IsMetadata, string DFDMFDId, out string ErrorMessage, out string DataMetadataFile)
    {
        bool RetVal;
        string SimpleDataSource;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;
        Dictionary<string, string> DictValidation;

        ErrorMessage = string.Empty;
        DataMetadataFile = string.Empty;
        RetVal = true;
        SimpleDataSource = string.Empty;
        QueryableDataSource = null;
        DictValidation = null;

        try
        {
            if (DataSource != null && DataSource.Count > 0 && DataSource.Count < 3)
            {
                if (DataSource.Count == 1)
                {
                    if (DataSource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                    {
                        QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)DataSource[0];
                    }
                    else if (DataSource[0] is string)
                    {
                        SimpleDataSource = (string)DataSource[0];
                    }
                    else
                    {
                        RetVal = false;
                        ErrorMessage = "Invalid data source";
                    }
                }
                else if (DataSource.Count == 2)
                {
                    if (DataSource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                    {
                        QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)DataSource[0];

                        if (DataSource[1] is string)
                        {
                            SimpleDataSource = (string)DataSource[1];
                        }
                        else
                        {
                            RetVal = false;
                            ErrorMessage = "Invalid data source";
                        }
                    }
                    else if (DataSource[0] is string)
                    {
                        SimpleDataSource = (string)DataSource[0];

                        if (DataSource[1] is SDMXObjectModel.Registry.QueryableDataSourceType)
                        {
                            QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)DataSource[1];
                        }
                        else
                        {
                            RetVal = false;
                            ErrorMessage = "Invalid data source";
                        }
                    }
                    else
                    {
                        RetVal = false;
                        ErrorMessage = "Invalid data source";
                    }
                }
                else
                {
                    RetVal = false;
                    ErrorMessage = "Invalid data source";
                }
            }
            else
            {
                RetVal = false;
                ErrorMessage = "Invalid data source";
            }


            if (RetVal != false)
            {
                if (!string.IsNullOrEmpty(SimpleDataSource))
                {
                    if (IsMetadata == true)
                    {
                        if (Global.IsDSDUploadedFromAdmin(DbNId))
                        {
                            Global.GetAppSetting();
                            DictValidation = RegTwoZeroFunctionality.Validate_MetadataReport_For_Version_2_0(SimpleDataSource, CompleteFileNameWPath, Global.registryMSDAreaId);
                        }
                        else
                        {
                            DictValidation = SDMXUtility.Validate_MetadataReport(SDMXSchemaType.Two_One, SimpleDataSource, CompleteFileNameWPath, DFDMFDId);
                        }

                        if (!(DictValidation != null && DictValidation.Keys.Count == 1 && DictValidation.ContainsKey(MetadataValidationStatus.Valid.ToString())))
                        {
                            RetVal = false;
                            ErrorMessage = "Invalid simple data source";
                        }
                    }
                    else
                    {
                        if (Global.IsDSDUploadedFromAdmin(DbNId))
                        {
                            DictValidation = RegTwoZeroFunctionality.Validate_SDMXML_File_For_Version_2_0(SimpleDataSource, CompleteFileNameWPath);
                        }
                        else
                        {
                            DictValidation = SDMXUtility.Validate_SDMXML(SDMXSchemaType.Two_One, SimpleDataSource, CompleteFileNameWPath);
                        }

                        if (!(DictValidation != null && DictValidation.Keys.Count == 1 && DictValidation.ContainsKey(SDMXValidationStatus.Valid.ToString())))
                        {
                            RetVal = false;
                            ErrorMessage = "Invalid simple data source";
                        }
                    }
                }

                if (RetVal != false && QueryableDataSource != null)
                {
                    try
                    {
                        HttpWebRequest Request = WebRequest.Create(QueryableDataSource.DataURL) as HttpWebRequest;

                        using (HttpWebResponse Response = Request.GetResponse() as HttpWebResponse)
                        {
                            if (Response.StatusCode != HttpStatusCode.OK)
                            {
                                RetVal = false;
                                ErrorMessage = "Invalid queryable data source";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RetVal = false;
                        ErrorMessage = "Invalid queryable data source";
                        Global.CreateExceptionString(ex, null);

                    }
                }
            }

            DataMetadataFile = SimpleDataSource;
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

    public static void Retrieve_SimpleAndQueryableDataSource_FromRegistration(RegistrationType Registration, out string SimpleDataSource, out SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource)
    {
        SimpleDataSource = string.Empty;
        QueryableDataSource = null;

        if (Registration.Datasource != null && Registration.Datasource.Count > 0 && Registration.Datasource.Count < 3)
        {
            if (Registration.Datasource.Count == 1)
            {
                if (Registration.Datasource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                {
                    QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[0];
                }
                else if (Registration.Datasource[0] is string)
                {
                    SimpleDataSource = (string)Registration.Datasource[0];
                }
            }
            else if (Registration.Datasource.Count == 2)
            {
                if (Registration.Datasource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                {
                    QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[0];

                    if (Registration.Datasource[1] is string)
                    {
                        SimpleDataSource = (string)Registration.Datasource[1];
                    }
                }
                else if (Registration.Datasource[0] is string)
                {
                    SimpleDataSource = (string)Registration.Datasource[0];

                    if (Registration.Datasource[1] is SDMXObjectModel.Registry.QueryableDataSourceType)
                    {
                        QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[1];
                    }
                }
            }
        }
    }

    #endregion "-- Public --"

    #region "-- Private --"

    private static void Create_Other_Artefacts_And_Update_Folder_Structures_For_Consumer_Per_Database(string UserNId)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string OutputFolder;
        string Query;

        DtRegisteredDatabases = null;
        DIConnection = null;
        OutputFolder = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
            DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

            foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
            {
                #region "--Subscription--"

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                Create_Directory_If_Not_Exists(OutputFolder);

                #endregion "--Subscription--"
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private static bool Is_Already_Existing_Consumer(string UserNId)
    {
        bool RetVal;
        string ConsumerFileName;
        SDMXObjectModel.Message.StructureType Structure;


        RetVal = false;
        ConsumerFileName = string.Empty;
        Structure = null;

        try
        {
            ConsumerFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);

            if (File.Exists(ConsumerFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConsumerFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null &&
                    Structure.Structures.OrganisationSchemes.Count > 0 &&
                    Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataConsumerSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null &&
                    Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataConsumerType DataConsumer in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        if (DataConsumer.id == DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId)
                        {
                            RetVal = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                RetVal = false;
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

        return RetVal;
    }

    private static string GetUserDetails(string requestParam)
    {
        string RetVal;
        string[] Params, UserDetails;
        DataTable DtUser;
        string UserNId, UserEmailId, UserPwd, UserFirstName, UserLastName, AreaNid, IsProvider, IsAdmin;
        diworldwide_userinfo.UserLoginInformation Service;

        try
        {
            RetVal = string.Empty;
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = Params[0].ToString().Trim();
            Service = new diworldwide_userinfo.UserLoginInformation();
            Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                RetVal = Service.GetUserDetails(UserNId, Global.GetAdaptationGUID());
                if (!string.IsNullOrEmpty(RetVal))
                {
                    UserDetails = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                    UserEmailId = UserDetails[0];
                    UserPwd = UserDetails[1];
                    UserFirstName = UserDetails[2];
                    UserLastName = UserDetails[3];
                    AreaNid = UserDetails[4];
                    IsProvider = UserDetails[5];
                    IsAdmin = UserDetails[6];
                    RetVal = UserEmailId + Constants.Delimiters.ParamDelimiter + UserPwd + Constants.Delimiters.ParamDelimiter +
                    UserFirstName + Constants.Delimiters.ParamDelimiter + UserLastName + Constants.Delimiters.ParamDelimiter + AreaNid + Constants.Delimiters.ParamDelimiter + IsProvider + Constants.Delimiters.ParamDelimiter + IsAdmin;
                }
            }
            else
            {
                DtUser = Get_User(Convert.ToInt32(UserNId));

                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    RetVal = DtUser.Rows[0]["User_Email_id"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Password"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_First_Name"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Last_Name"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Country"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Is_Provider"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Is_Admin"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    private static DataTable Get_User(int UserNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        string Query;

        RetVal = null;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT * FROM Users WHERE NId = " + UserNId.ToString() + ";";
            RetVal = DIConnection.ExecuteDataTable(Query);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private static string GetBaseURL()
    {
        string BaseUrl = string.Empty;
        string Url = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        int index = Url.IndexOf("libraries");
        BaseUrl = Url.Substring(0, index - 1);
        BaseUrl = HttpContext.Current.Server.UrlDecode(BaseUrl);
        return BaseUrl;
    }

    private static void Create_Directory_If_Not_Exists(string RetVal)
    {
        if (!Directory.Exists(RetVal))
        {
            Directory.CreateDirectory(RetVal);
        }
    }

    private static Dictionary<string, SubscriptionType> Get_Subscriptions_Dictionary(string DbNId, bool IsMetadata)
    {
        Dictionary<string, SubscriptionType> RetVal;
        string Query, FileNameWPath;
        DIConnection DIConnection;
        DataTable DtSubscriptions;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Registry.SubscriptionType Subscription;

        RetVal = new Dictionary<string, SubscriptionType>();
        Query = string.Empty;
        FileNameWPath = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + ";";
            DtSubscriptions = DIConnection.ExecuteDataTable(Query);
            foreach (DataRow DrSubscriptions in DtSubscriptions.Rows)
            {
                FileNameWPath = DrSubscriptions["FileLocation"].ToString();
                RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), FileNameWPath);
                Subscription = ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)RegistryInterface.Item).SubscriptionRequest[0].Subscription;

                if (Subscription.EventSelector != null && Subscription.EventSelector.Count > 0)
                {
                    if (IsMetadata == false)
                    {
                        if (Subscription.EventSelector[0] is DataRegistrationEventsType)
                        {
                            RetVal.Add(Subscription.RegistryURN, Subscription);
                        }
                    }
                    else
                    {
                        if (Subscription.EventSelector[0] is MetadataRegistrationEventsType)
                        {
                            RetVal.Add(Subscription.RegistryURN, Subscription);
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
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private static List<string> Get_Registrations_OverlapList(RegistrationType Registration, bool IsMetadata, bool IsAdminUploadedDSD)
    {
        List<string> RetVal;
        string FileURL, WSURL, Content, ElementName, AttributeName, PAIdPrefixRemoved, MFDId;
        WebRequest Request;
        WebResponse Response;
        StreamReader DataReader;
        Stream DataStream;
        XmlElement QueryElement;
        Registry.RegistryService Service;
        XmlDocument FileXML;

        RetVal = new List<string>();
        FileURL = string.Empty;
        WSURL = string.Empty;
        Content = string.Empty;
        ElementName = string.Empty;
        AttributeName = string.Empty;
        PAIdPrefixRemoved = string.Empty;
        MFDId = string.Empty;
        Request = null;
        Response = null;
        DataReader = null;
        DataStream = null;
        QueryElement = null;
        Service = new Registry.RegistryService();
        FileXML = new XmlDocument();

        if (IsMetadata == false)
        {
            if (Registration.Datasource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
            {
                WSURL = ((SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[0]).DataURL;

                if (Registration.Datasource.Count == 2)
                {
                    FileURL = Registration.Datasource[1].ToString();
                }
            }
            else
            {
                FileURL = Registration.Datasource[0].ToString();

                if (Registration.Datasource.Count == 2)
                {
                    WSURL = ((SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[1]).DataURL;
                }
            }

            if (IsAdminUploadedDSD == false)
            {
                if (!string.IsNullOrEmpty(FileURL))
                {
                    Request = WebRequest.Create(FileURL);
                    Response = Request.GetResponse();

                    DataStream = Response.GetResponseStream();
                    DataReader = new StreamReader(DataStream);
                    Content = DataReader.ReadToEnd();
                }
                else if (!string.IsNullOrEmpty(WSURL))
                {
                    QueryElement = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, null, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.SeriesKeyOnly, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, null, null).DocumentElement;
                    Service.Url = WSURL.ToString();
                    Service.GetStructureSpecificTimeSeriesData(ref QueryElement);
                    Content = QueryElement.OuterXml;
                }

                ElementName = "Series";
                AttributeName = DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id;
            }
            else
            {
                if (!string.IsNullOrEmpty(FileURL))
                {
                    Request = WebRequest.Create(FileURL);
                    Response = Request.GetResponse();

                    DataStream = Response.GetResponseStream();
                    DataReader = new StreamReader(DataStream);
                    Content = DataReader.ReadToEnd();
                }

                ElementName = SDMXApi_2_0.Constants.Namespaces.Prefixes.DevInfo + ":" + "Series";
                AttributeName = Constants.UNSD.Concept.Indicator.Id;
            }

            FileXML.LoadXml(Content);

            foreach (XmlNode Series in FileXML.GetElementsByTagName(ElementName))
            {
                if (!RetVal.Contains(Series.Attributes[AttributeName].Value))
                {
                    RetVal.Add(Series.Attributes[AttributeName].Value);
                }
            }
        }
        else
        {
            if (Registration.ProvisionAgreement != null && Registration.ProvisionAgreement.Items != null && Registration.ProvisionAgreement.Items.Count > 0)
            {
                PAIdPrefixRemoved = ((ProvisionAgreementRefType)Registration.ProvisionAgreement.Items[0]).id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix, string.Empty);
                MFDId = PAIdPrefixRemoved.Substring(PAIdPrefixRemoved.IndexOf('_') + 1);
                RetVal.Add(MFDId);
            }
        }

        return RetVal;
    }

    private static List<string> Get_Subscriptions_OverlapList(SubscriptionType Subscription, bool IsMetadata, bool IsAdminUploadedDSD, string DbNId)
    {
        List<string> RetVal;
        string CategoryId, CategorySchemeId, OutputFolder, FileName;
        Dictionary<string, List<string>> DictCategories;
        List<string> IndicatorGIds;

        RetVal = new List<string>();
        CategoryId = string.Empty;
        CategorySchemeId = string.Empty;
        OutputFolder = string.Empty;
        FileName = string.Empty;
        DictCategories = new Dictionary<string, List<string>>();
        IndicatorGIds = null;

        if (IsMetadata == false)
        {
            if (Subscription.EventSelector[0] is DataRegistrationEventsType)
            {
                foreach (CategoryReferenceType Category in ((DataRegistrationEventsType)Subscription.EventSelector[0]).Items)
                {
                    if (Category.Items != null && Category.Items.Count > 0)
                    {
                        CategoryId = ((CategoryRefType)Category.Items[0]).id;
                        CategorySchemeId = ((CategoryRefType)Category.Items[0]).maintainableParentID;

                        if (!DictCategories.ContainsKey(CategorySchemeId))
                        {
                            DictCategories.Add(CategorySchemeId, null);
                            DictCategories[CategorySchemeId] = new List<string>();
                            DictCategories[CategorySchemeId].Add(CategoryId);
                        }
                        else
                        {
                            DictCategories[CategorySchemeId].Add(CategoryId);
                        }
                    }
                }

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\" + Constants.FolderName.SDMX.Categories);

                foreach (string CategoryScheme in DictCategories.Keys)
                {
                    switch (CategoryScheme)
                    {
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Sector.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Sector.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Goal.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Goal.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Source.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Source.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Framework.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Framework.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Convention.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Convention.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Theme.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Theme.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Institution.Id:
                            FileName = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Institution.FileName);
                            break;
                        default:
                            break;
                    }

                    IndicatorGIds = Get_IndicatorGIds_From_CategoryScheme(FileName, DictCategories[CategoryScheme]);

                    foreach (string IndicatorGId in IndicatorGIds)
                    {
                        if (!RetVal.Contains(IndicatorGId))
                        {
                            RetVal.Add(IndicatorGId);
                        }
                    }
                }
            }
        }
        else
        {
            if (Subscription.EventSelector[0] is MetadataRegistrationEventsType)
            {
                foreach (MaintainableEventType MaintainableEvent in ((MetadataRegistrationEventsType)Subscription.EventSelector[0]).Items)
                {
                    RetVal.Add(((MaintainableQueryType)MaintainableEvent.Item).id);
                }
            }
        }

        return RetVal;
    }

    private static List<string> Get_IndicatorGIds_From_CategoryScheme(string FileName, List<string> ListCategoryGIds)
    {
        List<string> RetVal;
        SDMXObjectModel.Message.StructureType CategoryScheme;

        RetVal = new List<string>();

        try
        {
            CategoryScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

            foreach (string CategoryGId in ListCategoryGIds)
            {
                foreach (CategoryType Category in CategoryScheme.Structures.CategorySchemes[0].Items)
                {
                    if (Category.Items == null || Category.Items.Count == 0)
                    {
                        if (Category.Annotations != null && Category.Annotations.Count > 0 && Category.Annotations[0].AnnotationTitle == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.CategoryType && Category.Annotations[0].AnnotationText.Count > 0 && Category.Annotations[0].AnnotationText[0].Value == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.Indicator)
                        {
                            if (Category.id == CategoryGId)
                            {
                                if (!RetVal.Contains(CategoryGId))
                                {
                                    RetVal.Add(CategoryGId);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Category.id == CategoryGId)
                        {
                            Add_IndicatorGIds_For_Category(RetVal, CategoryGId, Category.Items, true);
                        }
                        else
                        {
                            Add_IndicatorGIds_For_Category(RetVal, CategoryGId, Category.Items, false);
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

        return RetVal;
    }

    private static void Add_IndicatorGIds_For_Category(List<string> RetVal, string CategoryGId, List<object> Categories, bool CategoryFoundFlag)
    {
        if (CategoryFoundFlag == false)
        {
            foreach (CategoryType Category in Categories)
            {
                if (Category.Items == null || Category.Items.Count == 0)
                {
                    if (Category.Annotations != null && Category.Annotations.Count > 0 && Category.Annotations[0].AnnotationTitle == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.CategoryType && Category.Annotations[0].AnnotationText.Count > 0 && Category.Annotations[0].AnnotationText[0].Value == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.Indicator)
                    {
                        if (Category.id == CategoryGId)
                        {
                            if (!RetVal.Contains(CategoryGId))
                            {
                                RetVal.Add(CategoryGId);
                            }
                        }
                    }
                }
                else
                {
                    if (Category.id == CategoryGId)
                    {
                        Add_IndicatorGIds_For_Category(RetVal, CategoryGId, Category.Items, true);
                    }
                    else
                    {
                        Add_IndicatorGIds_For_Category(RetVal, CategoryGId, Category.Items, false);
                    }
                }
            }
        }
        else
        {
            foreach (CategoryType Category in Categories)
            {
                if (Category.Items == null || Category.Items.Count == 0)
                {
                    if (Category.Annotations != null && Category.Annotations.Count > 0 && Category.Annotations[0].AnnotationTitle == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.CategoryType && Category.Annotations[0].AnnotationText.Count > 0 && Category.Annotations[0].AnnotationText[0].Value == DevInfo.Lib.DI_LibSDMX.Constants.Annotations.Indicator)
                    {
                        if (!RetVal.Contains(Category.id))
                        {
                            RetVal.Add(Category.id);
                        }
                    }
                }
                else
                {
                    Add_IndicatorGIds_For_Category(RetVal, CategoryGId, Category.Items, true);
                }
            }
        }
    }

    private static bool Are_OverLapping_Lists(List<string> ListRegistrationsIUS, List<string> ListSubscriptionsIUS)
    {
        bool RetVal;


        RetVal = false;

        foreach (string registrationIUS in ListRegistrationsIUS)
        {
            if (ListSubscriptionsIUS.Contains(registrationIUS))
            {
                RetVal = true;
                break;
            }
        }

        return RetVal;
    }

    private static void Send_Notifications_For_Current_Subscription(SubscriptionType Subscription, SDMXObjectModel.Registry.RegistrationType Registration, string HeaderFilePath = null)
    {
        string MessageContent;
        List<ArtefactInfo> Artefacts;
        DateTime CurrentDate;
        DateTime ValidityPeriodStartDate;
        DateTime ValidityPeriodEndDate;

        Header Header = new Header();
        XmlDocument UploadedHeaderXml = new XmlDocument();
        SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
        SDMXApi_2_0.Message.HeaderType Header_2_0 = new SDMXApi_2_0.Message.HeaderType();


        if (File.Exists(HeaderFilePath))
        {
            UploadedHeaderXml.Load(HeaderFilePath);
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
            Header_2_0 = UploadedDSDStructure.Header;
        }
        try
        {
            if (string.IsNullOrEmpty(HeaderFilePath) == false)
            {
                Header.ID = Header_2_0.ID;
                Header.Name = Header_2_0.Name[0].Value.ToString();
                foreach (SDMXApi_2_0.Message.PartyType Senders in Header_2_0.Sender)
                {
                    Header.Sender.ID = Senders.id;
                    Header.Sender.Name = !string.IsNullOrEmpty(Senders.Name[0].Value.ToString()) ? Senders.Name[0].Value.ToString() : string.Empty;
                    foreach (SDMXApi_2_0.Message.ContactType Contacts in Senders.Contact)
                    {
                        Header.Sender.Contact.Department = !string.IsNullOrEmpty(Contacts.Department[0].Value.ToString()) ? Contacts.Department[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Email = !string.IsNullOrEmpty(Contacts.Items[1].ToString()) ? Contacts.Items[1].ToString() : string.Empty;
                        Header.Sender.Contact.Fax = !string.IsNullOrEmpty(Contacts.Items[2].ToString()) ? Contacts.Items[2].ToString() : string.Empty;
                        Header.Sender.Contact.Name = !string.IsNullOrEmpty(Contacts.Name[0].Value.ToString()) ? Contacts.Name[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Role = !string.IsNullOrEmpty(Contacts.Role[0].Value.ToString()) ? Contacts.Role[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Telephone = !string.IsNullOrEmpty(Contacts.Items[0].ToString()) ? Contacts.Items[0].ToString() : string.Empty;

                    }

                }

                foreach (SDMXApi_2_0.Message.PartyType Receiver in Header_2_0.Receiver)
                {
                    Header.Receiver.ID = Receiver.id;
                    if (string.IsNullOrEmpty(Convert.ToString(Receiver.Name[0].Value)) == false)
                    {
                        Header.Receiver.Name = !string.IsNullOrEmpty(Receiver.Name[0].Value.ToString()) ? Receiver.Name[0].Value.ToString() : string.Empty;
                        foreach (SDMXApi_2_0.Message.ContactType Contacts in Receiver.Contact)
                        {
                            Header.Receiver.Contact.Department = !string.IsNullOrEmpty(Convert.ToString(Contacts.Department[0].Value)) ? Convert.ToString(Contacts.Department[0].Value) : string.Empty;
                            if (Contacts.Items.Length > 2)
                            {
                                Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[1])) ? Convert.ToString(Contacts.Items[1]) : string.Empty;
                                Header.Receiver.Contact.Fax = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[2])) ? Convert.ToString(Contacts.Items[2]) : string.Empty;
                            }
                            else
                            {
                                Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[1])) ? Convert.ToString(Contacts.Items[1]) : string.Empty;
                            }
                            //Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[1])) ? Convert.ToString(Contacts.Items[1]) : string.Empty;
                            //Header.Receiver.Contact.Fax = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[2])) ? Convert.ToString(Contacts.Items[2]) : string.Empty;
                            Header.Receiver.Contact.Name = !string.IsNullOrEmpty(Convert.ToString(Contacts.Name[0].Value)) ? Convert.ToString(Contacts.Name[0].Value) : string.Empty;
                            Header.Receiver.Contact.Role = !string.IsNullOrEmpty(Convert.ToString(Contacts.Role[0].Value)) ? Convert.ToString(Contacts.Role[0].Value) : string.Empty;
                            Header.Receiver.Contact.Telephone = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[0])) ? Convert.ToString(Contacts.Items[0]) : string.Empty;
                            //Header.Receiver.Contact.Department = !string.IsNullOrEmpty(Contacts.Department[0].Value.ToString()) ? Contacts.Department[0].Value.ToString() : string.Empty;
                            //Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Contacts.Items[2].ToString()) ? Contacts.Items[2].ToString() : string.Empty;
                            //Header.Receiver.Contact.Fax = !string.IsNullOrEmpty(Contacts.Items[1].ToString()) ? Contacts.Items[1].ToString() : string.Empty;
                            //Header.Receiver.Contact.Name = !string.IsNullOrEmpty(Contacts.Name[0].Value.ToString()) ? Contacts.Name[0].Value.ToString() : string.Empty;
                            //Header.Receiver.Contact.Role = !string.IsNullOrEmpty(Contacts.Role[0].Value.ToString()) ? Contacts.Role[0].Value.ToString() : string.Empty;
                            //Header.Receiver.Contact.Telephone = !string.IsNullOrEmpty(Contacts.Items[0].ToString()) ? Contacts.Items[0].ToString() : string.Empty;
                        }
                    }

                }

            }
            CurrentDate = DateTime.Now;
            ValidityPeriodStartDate = Subscription.ValidityPeriod.StartDate;
            ValidityPeriodEndDate = Subscription.ValidityPeriod.EndDate;
            if ((CurrentDate >= ValidityPeriodStartDate) && (CurrentDate <= ValidityPeriodEndDate))
            {
                if (string.IsNullOrEmpty(HeaderFilePath) == false)
                {
                    Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, Registration.id, Subscription.RegistryURN, ActionType.Append, Registration, Header);
                }
                else
                {
                    Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, Registration.id, Subscription.RegistryURN, ActionType.Append, Registration, null);
                }
                foreach (NotificationURLType NotificationMailId in Subscription.NotificationMailTo)
                {
                    if (NotificationMailId.isSOAP == true)
                    {
                        MessageContent = GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Send_Notification_By_Email(MessageContent, NotificationMailId.Value);
                }

                foreach (NotificationURLType NotificationHTTP in Subscription.NotificationHTTP)
                {
                    if (NotificationHTTP.isSOAP == true)
                    {
                        MessageContent = GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Send_Notification_By_HTTP_Post(MessageContent, NotificationHTTP.Value);
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


    private static void Send_Delete_Notifications_For_Current_Subscription(SubscriptionType Subscription, SDMXObjectModel.Registry.RegistrationType Registration, string HeaderFilePath = null)
    {
        string MessageContent;
        List<ArtefactInfo> Artefacts;
        DateTime CurrentDate;
        DateTime ValidityPeriodStartDate;
        DateTime ValidityPeriodEndDate;


        Header Header = new Header();
        XmlDocument UploadedHeaderXml = new XmlDocument();
        SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
        SDMXApi_2_0.Message.HeaderType Header_2_0 = new SDMXApi_2_0.Message.HeaderType();


        if (File.Exists(HeaderFilePath))
        {
            UploadedHeaderXml.Load(HeaderFilePath);
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
            Header_2_0 = UploadedDSDStructure.Header;
        }
        try
        {
            if (string.IsNullOrEmpty(HeaderFilePath) == false)
            {
                Header.ID = Header_2_0.ID;
                Header.Name = Header_2_0.Name[0].Value.ToString();
                foreach (SDMXApi_2_0.Message.PartyType Senders in Header_2_0.Sender)
                {
                    Header.Sender.ID = Senders.id;
                    Header.Sender.Name = !string.IsNullOrEmpty(Senders.Name[0].Value.ToString()) ? Senders.Name[0].Value.ToString() : string.Empty;
                    foreach (SDMXApi_2_0.Message.ContactType Contacts in Senders.Contact)
                    {
                        Header.Sender.Contact.Department = !string.IsNullOrEmpty(Contacts.Department[0].Value.ToString()) ? Contacts.Department[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Email = !string.IsNullOrEmpty(Contacts.Items[1].ToString()) ? Contacts.Items[1].ToString() : string.Empty;
                        Header.Sender.Contact.Fax = !string.IsNullOrEmpty(Contacts.Items[2].ToString()) ? Contacts.Items[2].ToString() : string.Empty;
                        Header.Sender.Contact.Name = !string.IsNullOrEmpty(Contacts.Name[0].Value.ToString()) ? Contacts.Name[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Role = !string.IsNullOrEmpty(Contacts.Role[0].Value.ToString()) ? Contacts.Role[0].Value.ToString() : string.Empty;
                        Header.Sender.Contact.Telephone = !string.IsNullOrEmpty(Contacts.Items[0].ToString()) ? Contacts.Items[0].ToString() : string.Empty;

                    }

                }

                foreach (SDMXApi_2_0.Message.PartyType Receiver in Header_2_0.Receiver)
                {
                    Header.Receiver.ID = Receiver.id;
                    if (string.IsNullOrEmpty(Convert.ToString(Receiver.Name[0].Value)) == false)
                    {
                        Header.Receiver.Name = !string.IsNullOrEmpty(Receiver.Name[0].Value.ToString()) ? Receiver.Name[0].Value.ToString() : string.Empty;
                        foreach (SDMXApi_2_0.Message.ContactType Contacts in Receiver.Contact)
                        {

                            if (Contacts.Items.Length > 2)
                            {
                                Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[1])) ? Convert.ToString(Contacts.Items[1]) : string.Empty;
                                Header.Receiver.Contact.Fax = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[2])) ? Convert.ToString(Contacts.Items[2]) : string.Empty;
                            }
                            else
                            {
                                Header.Receiver.Contact.Email = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[1])) ? Convert.ToString(Contacts.Items[1]) : string.Empty;
                            }
                            Header.Receiver.Contact.Department = !string.IsNullOrEmpty(Convert.ToString(Contacts.Department[0].Value)) ? Convert.ToString(Contacts.Department[0].Value) : string.Empty;
                            Header.Receiver.Contact.Name = !string.IsNullOrEmpty(Convert.ToString(Contacts.Name[0].Value)) ? Convert.ToString(Contacts.Name[0].Value) : string.Empty;
                            Header.Receiver.Contact.Role = !string.IsNullOrEmpty(Convert.ToString(Contacts.Role[0].Value)) ? Convert.ToString(Contacts.Role[0].Value) : string.Empty;
                            Header.Receiver.Contact.Telephone = !string.IsNullOrEmpty(Convert.ToString(Contacts.Items[0])) ? Convert.ToString(Contacts.Items[0]) : string.Empty;
                        }
                    }
                }

            }
            CurrentDate = DateTime.Now;
            ValidityPeriodStartDate = Subscription.ValidityPeriod.StartDate;
            ValidityPeriodEndDate = Subscription.ValidityPeriod.EndDate;
            if ((CurrentDate >= ValidityPeriodStartDate) && (CurrentDate <= ValidityPeriodEndDate))
            {
                //Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, Registration.id, Subscription.RegistryURN, ActionType.Delete, Registration, null);
                if (string.IsNullOrEmpty(HeaderFilePath) == false)
                {
                    Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, Registration.id, Subscription.RegistryURN, ActionType.Delete, Registration, Header);
                }
                else
                {
                    Artefacts = SDMXUtility.Get_Notification(SDMXSchemaType.Two_One, DateTime.Now, Registration.id, Subscription.RegistryURN, ActionType.Delete, Registration, null);
                }
                foreach (NotificationURLType NotificationMailId in Subscription.NotificationMailTo)
                {
                    if (NotificationMailId.isSOAP == true)
                    {
                        MessageContent = GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Send_Notification_By_Email(MessageContent, NotificationMailId.Value);
                }

                foreach (NotificationURLType NotificationHTTP in Subscription.NotificationHTTP)
                {
                    if (NotificationHTTP.isSOAP == true)
                    {
                        MessageContent = GetSoapWrappedXml(Artefacts[0].Content.OuterXml);
                    }
                    else
                    {
                        MessageContent = Artefacts[0].Content.OuterXml;
                    }

                    Send_Notification_By_HTTP_Post(MessageContent, NotificationHTTP.Value);
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

    private static void Send_Email(string SenderName, string SenderEmailId, string ReceiverEmailId, string Subject, string Content, bool IsHTMLBody = false)
    {
        MailMessage MailMessage;
        SmtpClient SmtpClient;

        try
        {
            MailMessage = new MailMessage();
            SmtpClient = new SmtpClient();

            MailMessage.From = new MailAddress(SenderEmailId, SenderName);
            MailMessage.To.Add(ReceiverEmailId);
            MailMessage.IsBodyHtml = IsHTMLBody;
            MailMessage.Priority = MailPriority.Normal;
            MailMessage.Subject = Subject;
            MailMessage.Body = Content;

            SmtpClient.ServicePoint.MaxIdleTime = 2;
            SmtpClient.Send(MailMessage);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }
    }

    #endregion "-- Private --"

    #endregion "-- Registry --"

    /// <summary>
    /// To maintain the errors in the log file. 
    /// </summary>
    /// <param name="ErrorString"></param>
    public static void WriteErrorsInLogFolder(string ErrorString)
    {
        string ErrorLogFolderPath = string.Empty;
        string ErrorFileNameWithPath = null;
        ErrorLogFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.ErrorLogs);
        ErrorLogFolderPath = Path.Combine(ErrorLogFolderPath, DateTime.Today.Month.ToString());

        //for checking that directory exists , if not then create it.
        if (!Directory.Exists(ErrorLogFolderPath))
        {
            Directory.CreateDirectory(ErrorLogFolderPath);
        }

        //variable: file path
        ErrorFileNameWithPath = Path.Combine(ErrorLogFolderPath, string.Format("{0:yyyyMMdd}", DateTime.Today.Date) + ".txt");

        //for checking file exists or not . if not then create the file and append the file
        if (!File.Exists(ErrorFileNameWithPath))
        {
            File.WriteAllText(ErrorFileNameWithPath, " ");
            WriteInErrorLogFile(ErrorFileNameWithPath, ErrorString);
        }
        else
        {
            WriteInErrorLogFile(ErrorFileNameWithPath, ErrorString);
        }

        //For Deleting folders other than current and previous month
        for (int i = 1; i < 12; i++)
        {
            int currentMonth = DateTime.Today.Month;
            int previousMonth = (DateTime.Now.AddMonths(-1)).Month;
            if (i == currentMonth || i == previousMonth)
                continue;
            else
            {
                string FNamePathToBeDeleted = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.ErrorLogs);
                FNamePathToBeDeleted = Path.Combine(FNamePathToBeDeleted, i.ToString());
                DeleteDirectory(FNamePathToBeDeleted);
                //if (Directory.Exists(FNamePathToBeDeleted))
                //{
                //    Directory.Delete(FNamePathToBeDeleted);
                //}
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ExceptionObject"></param>
    public static void CreateExceptionString(Exception ex, string indent)
    {
        StringBuilder sb = new StringBuilder();
        string Delemiter = Constants.Delimiters.ParamDelimiter;
        if (indent == null)
        {
            indent = string.Empty;
        }
        else if (indent.Length > 0)
        {
            sb.AppendFormat("{0}Inner ", indent);
        }

        sb.AppendFormat("Exception Found:\n{0}Type: {1}", Delemiter, ex.GetType().FullName);
        sb.AppendFormat("\n{0}Message: {1}", Delemiter, ex.Message);
        sb.AppendFormat("\n{0}Source: {1}", Delemiter, ex.Source);
        sb.AppendFormat("\n{0}Stacktrace: {1}", Delemiter, ex.StackTrace);
        sb.AppendFormat("\n{0}Exception Time: {1}", Delemiter, DateTime.Now);

        if (ex.InnerException != null)
        {
            sb.Append("\n");
            CreateExceptionString(ex.InnerException, indent);
        }

        Global.WriteErrorsInLogFolder(sb.ToString());
    }

    /// <summary>
    /// To append the error message into the error file.
    /// </summary>
    /// <param name="fpath"></param>
    /// <param name="ErrorString"></param>
    private static void WriteInErrorLogFile(string fpath, string ErrorString)
    {
        StreamWriter wr = null;
        try
        {
            wr = new StreamWriter(fpath, true);
            string str = "";
            str = System.DateTime.Now.ToString() + " >> " + ErrorString;
            str = str + System.Environment.NewLine;
            str = str + "=====================================================================================================================================================";
            str = str + System.Environment.NewLine;
            wr.WriteLine(str);
            wr.Flush();
            wr.Dispose();

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

    }

    public static string CheckIfMasterAccount(string UserNId)
    {
        string RetVal = string.Empty;
        try
        {
            if (isMasterAccount(UserNId) == true)
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
            //  RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public static bool isMasterAccount(string usernid)
    {
        bool isAdmin = false;
        string Query = string.Empty;
        diworldwide_userinfo.UserLoginInformation Service;

        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
        {
            Service = new diworldwide_userinfo.UserLoginInformation();
            Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            isAdmin = Service.IsMasterAccount(usernid);
        }
        return isAdmin;
    }

    public static DIConnectionDetails GetMapServerConnectionDetails()
    {
        //ServerType||ServerName||Database||User||Pasword

        DIConnectionDetails RetVal = new DIConnectionDetails();
        List<string> connectionDetailsWS;

        //GetConnection from web service
        connectionDetailsWS = new List<string>(DIMapServer.WebServiceInstance.GetMapServerConnection().Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

        RetVal.ServerType = (DIServerType)Convert.ToInt32(connectionDetailsWS[0]);
        //RetVal.ServerName = "localhost";
        RetVal.ServerName = connectionDetailsWS[1];
        RetVal.DbName = connectionDetailsWS[2];
        RetVal.UserName = connectionDetailsWS[3];
        RetVal.Password = Global.DecryptString(connectionDetailsWS[4]);

        //RetVal.ServerName = "dgps2";
        //RetVal.DbName = "DI7_MDG_r12";
        //RetVal.UserName = "sa";
        //RetVal.Password = "l9ce130";

        return RetVal;
    }

    /// <summary>
    /// http://www.devinfo.org/Mapserver/stock/data/1/ds
    /// </summary>
    /// <param name="UILanguageCode"></param>
    /// <returns></returns>
    public static string GetMapServerURL(string UILanguageCode)
    {
        string RetVal = string.Empty;
        try
        {
            UILanguageCode = GetMapServerLangCode(UILanguageCode);
            string MapServerDsURL = DIMapServer.WebServiceInstance.GetMapServerURL();
            RetVal = Path.Combine(MapServerDsURL, UILanguageCode);
            //DIConnection MapServerDBConnection = new DIConnection(Global.GetMapServerConnectionDetails());
            //if (MapServerDBConnection.ExecuteDataTable(DIQueries.CheckLanguageExists(MapServerDBConnection.DIDataSetDefault(), UILanguageCode)).row > 0)
            //{
            //    RetVal = UILanguageCode;
            //}
            //else
            //{
            //    DataTable table = MapServerDBConnection.ExecuteDataTable(DIQueries.GetDefaultLangauge(MapServerDBConnection.DIDataSetDefault()));
            //    if (table.Rows.Count != 0)
            //    {
            //        string mapServerDefaultLangCode = table.Rows[0][2].ToString();
            //        
            //    }
            //}
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal + "/";
    }
    public static string GetMapServerLangCode(string UILanguageCode)
    {
        string RetVal = string.Empty;
        string MapServerDsURL = DIMapServer.WebServiceInstance.GetMapServerURL();
        DIConnection MapServerDBConnection = new DIConnection(Global.GetMapServerConnectionDetails());
        if (MapServerDBConnection.ExecuteDataTable(DIQueries.CheckLanguageExists(MapServerDBConnection.DIDataSetDefault(), UILanguageCode)).Rows[0][0].ToString().Trim() != "0")
        {
            RetVal = UILanguageCode;
        }
        else
        {
            DataTable table = MapServerDBConnection.ExecuteDataTable(DIQueries.GetDefaultLangauge(MapServerDBConnection.DIDataSetDefault()));
            if (table.Rows.Count != 0)
            {
                string mapServerDefaultLangCode = table.Rows[0][2].ToString();
                RetVal = mapServerDefaultLangCode;
            }
        }
        return RetVal;
    }
    public static string GetMapServerDirectory()
    {
        string RetVal = string.Empty;
        try
        {
            RetVal = DIMapServer.WebServiceInstance.GetMapServerDirectory();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }
    public static string GetMapserverDbNid(string dbNid)
    {
        string RetVal = string.Empty;

        return RetVal;
    }

    public static DIQueries GetMapServerQueries(string DataPrefix_UI, string LanguageCode_UI, DIConnection connection)
    {
        //TO Do GetConnection from web service
        DIQueries RetVal = null;

        try
        {
            if (connection.DIDataSets().Select(DBAvailableDatabases.AvlDBPrefix + "='" + DataPrefix_UI.Trim('_') + "'").Length == 0)
            {
                DataPrefix_UI = connection.DIDataSetDefault();
            }

            if (connection.DILanguages(DataPrefix_UI).Select(Language.LanguageCode + "='" + LanguageCode_UI + "'").Length == 0)
            {
                LanguageCode_UI = connection.DILanguageCodeDefault(DataPrefix_UI);
            }

            RetVal = new DIQueries(DataPrefix_UI, LanguageCode_UI);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return RetVal;
    }

    /// <summary>
    /// Add langPrefNid (INT) table to the Artefacts table in database.mdb
    /// </summary>
    /// <returns>bool</returns>
    public static bool BaselineAccessDbSchema()
    {
        DIConnection diConnection;
        bool retVal = false;
        bool IsColumnExists = false;
        string query = string.Empty;

        using (diConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
        {
            try
            {
                // Check if field exists to avoid exception
                try
                {
                    query = @" ALTER TABLE Artefacts ADD COLUMN LangPrefNid INT";
                    diConnection.ExecuteNonQuery(query);
                }
                catch
                {
                    IsColumnExists = true;
                }

                //if (IsColumnExists == false)
                //{
                //    query = @" ALTER TABLE Artefacts ADD COLUMN LangPrefNid INT";
                //    diConnection.ExecuteNonQuery(query);
                //}

                retVal = true;
            }
            catch (Exception ex) //catch specific exception by refering to the DIConnectino source.
            {
                Global.CreateExceptionString(ex, null);
                throw ex;
            }
        }
        return retVal;
    }

    /// <summary>
    /// Gets preferred language Nid for subscription id (URN) from access DB
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns>string lngNid</returns>
    public static string GetPreferredLanguageFromSubscriptionId(string subscriptionId)
    {
        string lngNid = string.Empty;
        string lngNidQuery = string.Empty;
        DataTable dtLanguage = null;
        using (DIConnection diCon = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
        {
            dtLanguage = new DataTable();

            lngNidQuery = "SELECT LangPrefNId FROM Artefacts WHERE Id = '" + subscriptionId + "'";
            dtLanguage = diCon.ExecuteDataTable(lngNidQuery);
            foreach (DataRow row in dtLanguage.Rows)
            {
                lngNid = row[0].ToString();
            }
        }
        return lngNid;
    }

    /// <summary>
    /// Resolves the language name using the passed Nid. Uses SQL DB(ut_languages)
    /// </summary>
    /// <param name="langNid"></param>
    /// <returns>string</returns>
    public static string GetLanguageNameFromNid(string langNid)
    {
        string langName;
        LanguageModel lngModel;

        langName = String.Empty;
        lngModel = new LanguageModel();
        lngModel = JsonHelper.JsonDeserialize<LanguageModel>(Global.GetLangFromDB());
        // Resolve preferred language from langNid to pass to OAT grid
        foreach (int id in lngModel.LanguagesFromDB.Keys)
        {
            if (!string.IsNullOrEmpty(langNid))
            {
                if (id == Int32.Parse(langNid))
                {
                    if (lngModel.LanguagesFromDB[id].Contains("DI"))
                    {
                        langName = lngModel.LanguagesFromDB[id].Split(new string[] { "_" }, StringSplitOptions.None)[1];
                    }
                    else
                    {
                        langName = lngModel.LanguagesFromDB[id];
                    }
                    break;
                }
            }
        }
        return langName;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static DataTable GetAllDBLangaugeCodes()
    {
        DataTable RetVal = null;
        DIConnection DIConnection;
        try
        {
            DIConnection = Global.GetDbConnection(Int32.Parse(Global.GetDefaultDbNId()));
            RetVal = DIConnection.DILanguages(DIConnection.DIDataSetDefault());
        }
        catch (Exception)
        {
        }
        return RetVal;
    }

    public static DataTable GetAllDBLangaugeCodesByDbNid(int DBNid, DIConnection diConnection)
    {
        DataTable RetVal = null;
        //DIConnection DIConnection;
        if (DBNid != 0)
        {
            //DIConnection = Global.GetDbConnection(DBNid);
            RetVal = diConnection.DILanguages(diConnection.DIDataSetDefault());
        }
        return RetVal;
    }

    /// <summary>
    /// Returns list of languages from the Country Database ut_language table.
    /// </summary>
    /// <returns>JSON string (LanguageModel)</returns>
    public static string GetLangFromDB()
    {
        LanguageModel dbLanguages = new LanguageModel();
        DataTable dtLanguages;
        DIConnection DIConnection;
        string retVal = string.Empty;
        bool defaultLang;
        try
        {
            using (DIConnection = Global.GetDbConnection(Int32.Parse(Global.GetDefaultDbNId())))
            {
                retVal = "false";
                dtLanguages = DIConnection.DILanguages(DIConnection.DIDataSetDefault());
                int langNid;
                string langName;
                defaultLang = false;

                foreach (DataRow row in dtLanguages.Rows)
                {
                    langNid = Int32.Parse(row[Language.LanguageNId].ToString());
                    langName = row[Language.LanguageName].ToString();
                    defaultLang = Convert.ToBoolean(row[Language.LanguageDefault]);//.ToString();
                    dbLanguages.LanguagesFromDB.Add(langNid, langName);

                    // Check if default lang
                    if (defaultLang)
                    {
                        dbLanguages.DefaultLang = langName;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        retVal = JsonHelper.JsonSerializer<LanguageModel>(dbLanguages);
        return retVal;
    }


    /// <summary>
    /// Returns language code from the Country Database ut_language table.
    /// </summary>
    /// <returns>JSON string (LanguageModel)</returns>
    public static string GetLangCodeFromDB(string languageNid)
    {
        LanguageModel dbLanguages = new LanguageModel();
        DataTable dtLanguages;
        DIConnection DIConnection;
        string retVal = string.Empty;

        try
        {
            using (DIConnection = Global.GetDbConnection(Int32.Parse(Global.GetDefaultDbNId())))
            {
                retVal = "false";
                dtLanguages = DIConnection.DILanguages(DIConnection.DIDataSetDefault());
                string langCode;


                foreach (DataRow row in dtLanguages.Select("Language_NId=" + languageNid))
                {
                    langCode = Convert.ToString(row[Language.LanguageCode].ToString());
                    retVal = Convert.ToString(langCode);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return retVal;
    }


    /// <summary>
    /// Returns list of languages from the Country Database ut_language table.
    /// </summary>
    /// <returns>JSON string (LanguageModel)</returns>
    public static string GetLangNidFromlangCode(string languageCode)
    {
        LanguageModel dbLanguages = new LanguageModel();
        DataTable dtLanguages;
        DIConnection DIConnection;
        string retVal = "0";

        try
        {
            using (DIConnection = Global.GetDbConnection(Int32.Parse(Global.GetDefaultDbNId())))
            {
                retVal = "0";
                dtLanguages = DIConnection.DILanguages(DIConnection.DIDataSetDefault());
                string langNid;


                foreach (DataRow row in dtLanguages.Select("Language_Code='" + languageCode + "'"))
                {
                    langNid = Convert.ToString(row[Language.LanguageNId].ToString());
                    retVal = langNid;

                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return retVal;
    }


    public static string GenerateMetaValues(string QueryString)
    {
        string RetVal = string.Empty;
        string[] QueryStringArray = QueryString.Split(new string[] { "," }, StringSplitOptions.None);
        string Indicator = QueryStringArray[1].ToString().Split(new string[] { ":" }, StringSplitOptions.None)[1].ToString();
        Indicator = Indicator.Replace("[****]", " - ");
        string IndicatorNId = QueryStringArray[2].ToString().Split(new string[] { ":" }, StringSplitOptions.None)[1].ToString();
        string Area = QueryStringArray[3].ToString().Split(new string[] { ":" }, StringSplitOptions.None)[1].ToString();
        Area = Area.Replace("[****]", "  ");
        IndicatorNId = IndicatorNId.Replace("\"", "");
        string[] IndicatorNIdArray = IndicatorNId.Split(new string[] { "[****]" }, StringSplitOptions.RemoveEmptyEntries);
        string IndicatorDescription = GetIndicatorDescription(Convert.ToInt32(IndicatorNIdArray[0].ToString()), GetDefaultLanguageCode());
        RetVal = Global.adaptation_name + " - " + Area.Replace("\"", "") + " - " + Indicator.Replace("\"", "");
        RetVal = RetVal + "[**]" + Global.adaptation_name + "," + Indicator.Replace("\"", "") + "," + Area.Replace("\"", "");
        IndicatorDescription = Global.adaptation_name + ". " + Indicator.Replace("\"", "") + ". " + Area.Replace("\"", "") + ". " + IndicatorDescription;
        RetVal = RetVal + "[**]" + IndicatorDescription;
        return RetVal;
    }

    /// <summary>
    /// Returns the Definiton of Indicator, whose Nid has been passed
    /// </summary>
    /// <param name="indicatorNId"></param>
    /// <param name="searchLanguage"></param>
    /// <returns></returns>
    private static string GetIndicatorDescription(int indicatorNId, string searchLanguage)
    {
        string RetVal = string.Empty;
        XmlDocument MetadataDocument = null;
        string XmlFileWithPath = string.Empty;
        string dataPath = string.Empty;
        try
        {
            MetadataDocument = new XmlDocument();
            dataPath = "stock/data/" + Global.GetDefaultDbNId() + "/ds/";
            XmlFileWithPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, dataPath, searchLanguage + @"\metadata\indicator\" + indicatorNId.ToString() + ".xml");

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



    /// <summary>
    /// Add PublishedFileName(Text) table to the Artefacts table in database.mdb
    /// </summary>
    /// <returns>bool</returns>
    public static bool ExistenceofColumnAccessDbSchema()
    {
        DIConnection diConnection;
        bool retVal = false;
        bool IsColumnExists = false;
        string query = string.Empty;

        using (diConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
        {
            try
            {
                // Check if field exists to avoid exception
                try
                {   
                    //// MOD LOG
                    //// Issue Id:BUG 7    Issue Date: 07-May-2014
                    //// Issue Fixed by:vedprakash@sdrc.co.in
                    //// Description: Exception capturing was used to figure out database colmun existance. Fixed to check for column exist before running the query to create column PublishedFileName.
                    query = @" SELECT Top 1 * FROM Artefacts";
                    DataTable table = diConnection.ExecuteDataTable(query);
                    if (table.Columns.IndexOf("PublishedFileName") < 0)
                    {
                        query = @" ALTER TABLE Artefacts ADD COLUMN PublishedFileName Text(225)";
                        var result = diConnection.ExecuteScalarSqlQuery(query);
                    }
                    IsColumnExists = true;
                }
                catch
                {
                    IsColumnExists = true;
                }
                retVal = true;
            }
            catch (Exception ex) //catch specific exception by refering to the DIConnectino source.
            {
                Global.CreateExceptionString(ex, null);
                throw ex;
            }
        }
        return retVal;
    }



    /// <summary>
    /// Get filename
    /// </summary>
    /// <param name="subscriptionId"></param>
    /// <returns>string lngNid</returns>
    public static string GetFileName(string Registrationid, string DbNId)
    {
        string filename = string.Empty;
        string query = string.Empty;
        DataTable dtregistrations = null;
        using (DIConnection diCon = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, @"stock\Database.mdb"), string.Empty, string.Empty))
        {
            dtregistrations = new DataTable();

            query = "SELECT * FROM Artefacts WHERE DBNId = " + DbNId + "  AND id='" + Registrationid + "' AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ";";
            dtregistrations = diCon.ExecuteDataTable(query);
            foreach (DataRow row in dtregistrations.Rows)
            {
                filename = row["PublishedFileName"].ToString();
            }
        }
        return filename;
    }

    #endregion

    #endregion
}
