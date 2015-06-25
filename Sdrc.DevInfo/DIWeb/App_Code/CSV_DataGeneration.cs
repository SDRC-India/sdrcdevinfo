using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using Ionic.Zip;
using System.Configuration;
using System.Xml;

/// <summary>
/// Summary description for CSV_DataGeneration
/// </summary>
public partial class Callback : System.Web.UI.Page
{


    #region "Variables"
       DIConnection DIConnection= null;
      
    #endregion "Variables"



    #region "Private Methods"

    /// <summary>
    /// This perticular method is used for executing database query for selecting information from multiple database tables using data reader.
    /// We have used datareader because no of records are quite high so loading data in any data object is not possible.
    /// </summary>
    /// <param name="ConStr">This parameter is used for holding query string.</param>
    /// <param name="Language"></param>
    /// <returns></returns>
       private SqlDataReader ExecuteSqlQuery(string ConStr, string Language)
       {
           SqlCommand cmd = new SqlCommand();
           SqlDataReader dr = null;
           try
           {
               SqlConnection con = new SqlConnection(ConStr.ToString());
               con.Open();
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandText = "sp_SelectMultipleData_" + Language;
               cmd.Connection = con;
               dr = cmd.ExecuteReader();
               return dr;
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
               return null;
           }
       }

    /// <summary>
    /// This Perticular method is used for generating CSV files for all languages in that database
    /// </summary>
    /// <param name="DataReader">data reader object containg output of sql query</param>
    /// <param name="FilePath">Path where to create CSV file</param>
    /// <returns></returns>
       private bool CreateCsvFile(SqlDataReader DataReader, string FilePath)
       {
           DataTable dt = DataReader.GetSchemaTable();
           StringBuilder StrBld = new StringBuilder();
           try
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   StrBld.Append(dt.Rows[i][0].ToString());
                   if (i < dt.Rows.Count - 1)
                       StrBld.Append(",");
               }
               WriteToCsvFile(StrBld, FilePath);
               while (DataReader.Read())
               {
                   StringBuilder StrBldObj = new StringBuilder();
                   for (int i = 0; i < dt.Rows.Count; i++)
                   {
                       StrBldObj.Append("\"" + DataReader[i] + "\"");
                       if (i < dt.Rows.Count - 1)
                           StrBldObj.Append(",");
                   }
                   WriteToCsvFile(StrBldObj, FilePath);
               }
               return true;
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
               return false;
           }
       }

    /// <summary>
    /// This method writes in csv file
    /// </summary>
    /// <param name="sb">contains input data to write in csv file</param>
    /// <param name="FilePath">Path where to create CSV file</param>
       private void WriteToCsvFile(StringBuilder sb, string FilePath)
       {
           try
           {
               string fileName=Path.GetFileName(FilePath);
               string folderPath = FilePath.Replace("\\" + fileName, "");
               if (!Directory.Exists(folderPath))
               {
                   Directory.CreateDirectory(folderPath);
               }
               sb.Append("\r\n");
               File.AppendAllText(FilePath, sb.ToString());
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
           }

       }

    /// <summary>
    /// After zip file is created csv file is deleted from folder
    /// </summary>
    /// <param name="FilePath">Path of created csv file</param>
    /// <returns>return true if deletion isperformed successfully otherwise return false</returns>
       private bool DeleteCsvFile(string FilePath)
       {
           bool Result = false;
           try
           {
               if (!System.IO.File.Exists(FilePath))
               {
                   Result = false;
               }
               else
               {
                   System.IO.File.Delete(FilePath);
                   Result = true;
               }
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
               Result = false;
           }
           return Result;
       }

    /// <summary>
    /// This perticular method is used for creating zip file of created CSV file
    /// </summary>
    /// <param name="FilePath">path where csv file exist</param>
    /// <returns>return true if deletion isperformed successfully otherwise return false</returns>
       private bool CreateZipFile(string FilePath)
       {
           ZipFile createZipFile = new ZipFile();
           bool Result = false;
           try
           {
               
               createZipFile.AddFile(FilePath, string.Empty);
               string OutPutFileName = FilePath.Replace("csv", "zip");
               createZipFile.Save(OutPutFileName);
               Result = true;
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
               Result = false;
           }
           return Result;
       }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="KeyValue"></param>
    /// <returns></returns>
       private bool AddKeyToAppSetting(string KeyValue)
       {
           bool Result = false;
           string AppSettingFile = string.Empty;
           XmlDocument XmlDoc;
           XmlDoc = new XmlDocument();
           try
           {
               AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
               XmlDoc.Load(AppSettingFile);
               this.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.DownloadCSV, KeyValue);
               File.SetAttributes(AppSettingFile, FileAttributes.Normal);
               XmlDoc.Save(AppSettingFile);
               Result = true;
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
               Result = false;
           }           
           return Result;
       }

       /// <summary>
       /// This perticular method is used for adding created CSV files names entry in database
       /// </summary>
       /// <param name="CsvFileNames">name of all csv files present in CSV_DataFiles folers seperated from ','</param>
       private void AddCsv_EntryTo_WebService(string CsvFileNames)
       {
           //string AdaptationURL = Global.GetAdaptationUrl();
           DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
           CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
           CatalogService.Update_LangCode_CSVFiles_Catalog(Global.GetAdaptationGUID(), CsvFileNames);
       }

       /// <summary>
       ///  This perticular method is used for adding created CSV files names in database
       /// </summary>
       /// <param name="CsvFileNames">name of all csv files present in CSV_DataFiles folers seperated from ','</param>
       /// <param name="DbNid">Id of data base</param>
       private void AddCsv_EntryTo_DBXML(string CsvFileNames, int DbNid)
       {
           XmlDocument XmlDoc;
           XmlNode xmlNode;
           string DBConnectionsFile;
           XmlDoc = new XmlDocument();
           try
           {
               DBConnectionsFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
               XmlDoc.Load(DBConnectionsFile);

               xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNid.ToString() + "]");
               xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.LanguageCodeCSVFiles].Value = CsvFileNames;

               File.SetAttributes(DBConnectionsFile, FileAttributes.Normal);
               XmlDoc.Save(DBConnectionsFile);
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
           }
       }

       private void DeleteExistingFiles()
       {
           string FolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\CSV_DataFiles");
           try
           {
               System.IO.DirectoryInfo CsvDirInfo = new DirectoryInfo(FolderPath);

               foreach (FileInfo file in CsvDirInfo.GetFiles())
               {
                   file.Delete();
               }

               //if (Directory.Exists(FolderPath))
               //{
               //    Directory.Delete(FolderPath, true);
               //}
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
           }           
       }
  
    #endregion"private Methods"



    #region "Public Methods"

    /// <summary>
    /// This method is used for generating csv files
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
       public string GenerateCsvFile()
       {
           string CsvFileNames = string.Empty;
           string RetVal = string.Empty;
           string ConStr = string.Empty;
           int DBNId =Convert.ToInt32(Global.GetDefaultDbNId());
           string FileBasePath = string.Empty;
           string DestFilePath=string.Empty;
           string CSVFileName = string.Empty;
           //string DtatabaseConnectionname=string.Empty;
           string CSVFileStartName = string.Empty;
           
           FileBasePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\CSV_DataFiles\\");
           try
           {
               DeleteExistingFiles();
               DIConnection = Global.GetDbConnection(DBNId);
               List<string> RetAllLang = getAllDbLangCodes(DBNId);
               string AdaptationName = string.Empty;

               AdaptationName = Global.adaptation_name;
               CSVFileStartName = AdaptationName + "_" + Global.AdaptationYear + "_";
               foreach (string lang in RetAllLang)
               {
                   CSVFileName = CSVFileStartName + lang + ".csv";
                   DestFilePath = FileBasePath + CSVFileName;
                   ConStr = DIConnection.GetConnection().ConnectionString.ToString() + "Password='" + DIConnection.ConnectionStringParameters.Password + "';";
                   SqlDataReader DataReader = ExecuteSqlQuery(ConStr, lang);
                   if (CreateCsvFile(DataReader, DestFilePath))
                   {
                       if (CreateZipFile(DestFilePath))
                       {
                           CsvFileNames = CsvFileNames + Path.GetFileName(DestFilePath).Replace(".csv", "").Replace(CSVFileStartName, "");
                           CsvFileNames = CsvFileNames + ",";
                           DeleteCsvFile(DestFilePath);
                       }                       
                   }
               }
               CsvFileNames = CsvFileNames.Remove(CsvFileNames.Length - 1);
               AddCsv_EntryTo_DBXML(CsvFileNames, DBNId);
               AddCsv_EntryTo_WebService(CsvFileNames);

               RetVal = "true";
           }
           catch (Exception Ex)
           {
               Global.CreateExceptionString(Ex, null);
           }
           return RetVal;

       }

    /// <summary>
    /// This method reads name of all csv files present in folder CSV_DataFile(physical location)
    /// and append names to a string seperated from ',' and return that final string.
    /// </summary>
    /// <returns></returns>
       public string GetCSVFiles()
       {
           string Result = string.Empty;
           string FileNames = string.Empty;
           string DBNId = Global.GetDefaultDbNId();
           string FilePathInfo = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\CSV_DataFiles");          
           try
           {
               if (Directory.Exists(FilePathInfo))
               {
                   string[] files = Directory.GetFiles(FilePathInfo);
                   foreach (string s in files)
                   {
                       FileNames = FileNames + Path.GetFileName(s) + ",";
                   }
                   FileNames = FileNames.Remove(FileNames.Length - 1);
                   Result = FileNames;
               }
           }

           catch (Exception EX)
           {
               Global.CreateExceptionString(EX, null);
               Result = "";
           }
           return Result;
       }    

    #endregion "Public Methods"


    


}