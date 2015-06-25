using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Xml;

/// <summary>
/// Summary description for LanguageImport
/// </summary>
public class LanguageImport
{
    public LanguageImport()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Delets all existing XLS files
    /// </summary>       
    /// <returns> true if XLS files have been deleted successfully else false</returns>
    public bool DeleteExistingXLSFile() 
    {
        bool RetVal = false;
        string CSVFolderPath = string.Empty;
        try
        {
            CSVFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.TemporaryXLS);
            System.IO.DirectoryInfo XMlDirectoryInfo = new System.IO.DirectoryInfo(CSVFolderPath);

            foreach (System.IO.FileInfo file in XMlDirectoryInfo.GetFiles())
            {
                file.Delete();
            }
            RetVal = true;
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Used to Import Language Xml files from src XLS file
    /// </summary>
    /// <param name="FileFullName">name of input XlS file</param>
    /// <param name="FileFullPath">Path of input XLS file</param>
    /// <returns>true if language xml file imported successfully, else return false</returns>
    public string ImportLanguageXMLFiles(string FileFullName, string FileFullPath)
    {
        string RetVal = string.Empty;
        DataTable ReurnVal = new DataTable();
        string[] ReturnParams;

        ///Variables for creatin CSVLogfile 
        string SrcXMLfileName = string.Empty;
        string DestXMLFileName = string.Empty;
        string XLSFileMsg = string.Empty;
        try
        {
            FileFullName = Path.GetFileName(FileFullName);
            System.Collections.Generic.List<Constants.CreateXmlFor> listCreateXmlFor = new System.Collections.Generic.List<Constants.CreateXmlFor>();

            //Read XLS file to create xml file
            ReurnVal = ReadXLSFile(FileFullPath, FileFullName, false);
            if (ReurnVal != null)
            {
                listCreateXmlFor.Add(Constants.CreateXmlFor.SrcXmlFile);
                listCreateXmlFor.Add(Constants.CreateXmlFor.DestXMLFile);
                foreach (Constants.CreateXmlFor XmlFileType in listCreateXmlFor)
                {
                    RetVal = GenerateLanguageXMLFile(ReurnVal, XmlFileType);
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        ReturnParams = Global.SplitString(RetVal, Constants.Delimiters.ParamDelimiter);
                        if (ReturnParams.Length > 0 && !string.IsNullOrEmpty(ReturnParams[0]))
                        {
                            if (ReturnParams[0] == "false")
                            {
                                if (!string.IsNullOrEmpty(ReturnParams[1]))
                                {
                                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + ReturnParams[1];
                                }
                                return RetVal;
                            }
                        }
                        else
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.ImportFalied;
                            return RetVal;
                        }
                    }
                    else
                    {
                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.ImportFalied;
                        return RetVal;
                    }
                }
                SrcXMLfileName = ReurnVal.Columns[1].ColumnName.Replace("(", "[").Replace(")", "]");
                DestXMLFileName = ReurnVal.Columns[2].ColumnName.Replace("(", "[").Replace(")", "]");
                XLSFileMsg = string.Format(Constants.CSVLogMessage.ImportLanguage, SrcXMLfileName, DestXMLFileName);
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + XLSFileMsg;
            }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.ErrorinReadXLSFile.ToString();
            }
        }
        catch (Exception Ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.ImportFalied;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Used to Generate XML files
    /// </summary>
    /// <param name="DtXMlData">datatable containing data for creating xml file</param>
    /// <param name="XmlFileType">Type of XML file--srec of destination</param>
    private string GenerateLanguageXMLFile(DataTable DtXMlData, Constants.CreateXmlFor XMLFileType)
    {
        XmlDocument XMLDoc = new XmlDocument();
        string FileName = string.Empty;
        string XmlFilePath = string.Empty;
        XmlElement ChildNode;
        string TemplateFilePath = string.Empty;
        string RetVal = string.Empty;
        bool IsTemplateCreated = false;
        if (XMLFileType.Equals(Constants.CreateXmlFor.SrcXmlFile))
        {
            FileName = DtXMlData.Columns[1].ColumnName;
        }
        else
        {
            FileName = DtXMlData.Columns[2].ColumnName;
        }
        FileName = FileName.Replace("(", "[").Replace(")", "]");
        XmlFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,
                        Constants.FolderName.MasterKeyVals, FileName) + ".xml";

        if (File.Exists(XmlFilePath))
        {
            CreateBackUpFile(XmlFilePath, FileName);

            // Check if template xml file exist
            TemplateFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.LanguageTemplate, Constants.FileName.LanguageXMLTemplate);
            if (File.Exists(TemplateFilePath))
            {
                // Create XML file template
                IsTemplateCreated = CreateXMLFromTemplate(FileName);

                // If XML template created
                if (IsTemplateCreated)
                {
                    // Create Doc Type
                    XMLDoc.Load(XmlFilePath);
                    for (int Jcount = 1; Jcount < DtXMlData.Rows.Count; Jcount++)
                    {
                        //create child noods
                        ChildNode = XMLDoc.CreateElement("Row");

                        //Set attribute name and value!
                        ChildNode.SetAttribute("key", DtXMlData.Rows[Jcount][0].ToString());
                        if (XMLFileType.Equals(Constants.CreateXmlFor.SrcXmlFile))
                        {
                            ChildNode.SetAttribute("value", DtXMlData.Rows[Jcount][1].ToString());
                        }
                        else
                        {
                            ChildNode.SetAttribute("value", DtXMlData.Rows[Jcount][2].ToString());
                        }
                        XMLDoc.DocumentElement.AppendChild(ChildNode);
                    }
                    XMLDoc.Save(XmlFilePath);
                    RetVal = "true";
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.ErrorCreateXMLTemplate.ToString();
                }
            }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.XMLTempNotExist.ToString();
            }
        }
        else
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + Constants.XLSImportMessage.LanguageNotExist.ToString();
        }
        return RetVal;

    }


    private bool CreateXMLFromTemplate(string DestXMLFileName)
    {
        string XMLTemplatePath = string.Empty;
        string XMLFolderPath = string.Empty;
        string SrcXMLFileName = string.Empty;

        bool RetVal = false;
        bool IsFileCreated = false;
        try
        {
            XMLTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.LanguageTemplate);
            XMLFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.MasterKeyVals);
            SrcXMLFileName = Constants.FileName.LanguageXMLTemplate.ToString();
            IsFileCreated = CreateXMLTemplateCopy(XMLTemplatePath, XMLFolderPath, SrcXMLFileName, DestXMLFileName);
            if (IsFileCreated)
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }

        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;

    }

    private bool CreateXMLTemplateCopy(string XMLTemplatePath, string XMLFolderPath, string SrcFileName, string DestFileName)
    {
        bool RetVal = false;
        string SourceKMLFilePath = string.Empty;
        string DestKMLFilePath = string.Empty;
        try
        {
            if (!Directory.Exists(XMLTemplatePath))
            {
                RetVal = false;
            }
            else
            {
                SourceKMLFilePath = Path.Combine(XMLTemplatePath, SrcFileName);
                DestKMLFilePath = Path.Combine(XMLFolderPath, DestFileName) + ".xml";
                if (File.Exists(SourceKMLFilePath))
                {
                    if (File.Exists(DestKMLFilePath))
                    {
                        File.Delete(DestKMLFilePath);
                        File.Copy(SourceKMLFilePath, DestKMLFilePath, true);
                        RetVal = true;
                    }
                    else
                    {
                        File.Copy(SourceKMLFilePath, DestKMLFilePath, true);
                        RetVal = true;
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    private DataTable ReadXLSFile(string XMLFilesFullPath, string fileName, bool hasHeaders)
    {
        DataTable RetVal = new DataTable();
        //string HDR = hasHeaders ? "Yes" : "No";
        string strConn;
        //create connection string for oledb connection from CSV File
        if (Environment.Is64BitProcess)
        {
           // strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=0;\"";
        }
        else
        {
          // strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
           strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1;MAXSCANROWS=0;\"";
        }
        //DataTable RetVal = new DataTable();
        OleDbCommand cmd;
        string DataselectionQuery = string.Empty;
        using (OleDbConnection conn = new OleDbConnection(strConn))
        {
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(
                OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string sheet = schemaRow["TABLE_NAME"].ToString();
                if (!sheet.EndsWith("_"))
                {
                    try
                    {
                        cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                        cmd.CommandType = CommandType.Text;
                        RetVal = new DataTable(sheet);
                        new OleDbDataAdapter(cmd).Fill(RetVal);

                    }
                    catch (Exception ex)
                    {
                        Global.CreateExceptionString(ex, null);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        return RetVal;
    }


    private bool CreateBackUpFile(string FilePath, string FileName)
    {
        bool RetVal = false;
        string OldPath = string.Empty;
        string Newpath = string.Empty;
        string NewFileName = string.Empty;
        string NewFilePath = string.Empty;
        string Date = string.Empty;
        string Time = string.Empty;
        try
        {
            
            OldPath = FilePath;           
            FileInfo f1 = new FileInfo(OldPath);
            if (f1.Exists)
            {
                Date = DateTime.Now.Date.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                Time = DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString();

                Newpath =Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,
                        Constants.FolderName.LanguageBackUp);
                NewFileName = FileName + "_" + Date + "_"+Time;
                if (!Directory.Exists(Newpath))
                {
                    Directory.CreateDirectory(Newpath);
                }
                NewFilePath = Path.Combine(Newpath, NewFileName) + f1.Extension;
                f1.CopyTo(NewFilePath);
                RetVal = true;
            }

        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }




}