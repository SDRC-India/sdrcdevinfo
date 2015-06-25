using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Data.OleDb;

public partial class libraries_aspx_Admin_Language : System.Web.UI.Page
{
    #region "--Private--"

    #region "--Methods--"

    private void FillSourceLanguageDDL()
    {
        string LngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList ObjXmlNodeList;
        int i = 1;

        try
        {
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LngFile);

            ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Language.Tags.Root + "/" + "child::node()");

            ddlSrcLng.Items.Clear();
            ddlSrcLng.Items.Insert(0, new ListItem("-- Select Language --", "-1"));

            foreach (XmlNode data in ObjXmlNodeList)
            {
                ddlSrcLng.Items.Insert(i, new ListItem(data.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value));
                i++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillTargetLanguageDDL()
    {
        string LngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList ObjXmlNodeList;
        int i = 1;

        try
        {
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LngFile);

            ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Language.Tags.Root + "/" + "child::node()");

            ddlTrgLng.Items.Clear();
            ddlTrgLng.Items.Insert(0, new ListItem("-- Select Language --", "-1"));

            foreach (XmlNode data in ObjXmlNodeList)
            {
                ddlTrgLng.Items.Insert(i, new ListItem(data.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value));
                i++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        //string LngFile = string.Empty;
        //XmlDocument XmlDoc;
        //XmlNodeList ObjXmlNodeList;
        //int i = 1;

        //try
        //{
        //    LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DestinationLanguageFile]);
        //    XmlDoc = new XmlDocument();
        //    XmlDoc.Load(LngFile);

        //    ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.DestinationLanguage.Tags.Root + "/" + "child::node()");

        //    ddlTrgLng.Items.Clear();
        //    ddlTrgLng.Items.Insert(0, new ListItem("-- Select Language --", "-1"));

        //    foreach (XmlNode data in ObjXmlNodeList)
        //    {
        //        ddlTrgLng.Items.Insert(i, new ListItem(data.ChildNodes[0].InnerText));
        //        i++;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Global.CreateExceptionString(ex, null);
        //}
    }




    #endregion

    #endregion

    #region "--Public/Protected--"

    #region "--Variagles--"

    protected string RowDelimiter = string.Empty;
    protected string ColumnDelimiter = string.Empty;

    #endregion

    #region "--Methods/Events--"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            Response.Redirect("AdminLogin.aspx");
        }
        else
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name;

            //Fill source language dropdown list
            this.FillSourceLanguageDDL();

            //Fill target language dropdown list
            this.FillTargetLanguageDDL();



            if (!string.IsNullOrEmpty(Request["n"]))
            {
                ddlSrcLng.SelectedValue = Request["n"];
                ddlTrgLng.SelectedValue = Request["n"];
            }

            this.RowDelimiter = Constants.Delimiters.RowDelimiter;
            this.ColumnDelimiter = Constants.Delimiters.ColumnDelimiter;
        }
    }
    protected void btnUploadLangXLS_Click(object sender, EventArgs e)
    {
        string FileName = string.Empty;
        string FileExtension = string.Empty;
        string FileFullPath = string.Empty;
        DataTable RetDataTable = new DataTable();
        LanguageImport LangImport = new LanguageImport();
        string RetResult = string.Empty;
        string[] ImportResult;

        ///Variables for creatin CSVLogfile 
        string UserName = string.Empty;
        string XLSFileMsg = string.Empty;
        string SrcXMLfileName = string.Empty;
        string DestXMLFileName = string.Empty;

        if (FileUpdXLS.HasFile)
        {
            FileName = FileUpdXLS.FileName;// Name of file with extension
            // Get File extension from file name
            FileExtension = Path.GetExtension(FileUpdXLS.PostedFile.FileName).Substring(1);
            // Allow further processing if input file is xls file
            if (FileExtension.ToLower() == "xls")
            {
               
                //delete existing xls file from temp folder
                LangImport.DeleteExistingXLSFile();
                FileFullPath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.TemporaryXLS, FileName); ;
                FileUpdXLS.SaveAs(Path.Combine(FileFullPath));

                // Call method to import xls file
                RetResult = LangImport.ImportLanguageXMLFiles(FileName, FileFullPath);
                ImportResult = Global.SplitString(RetResult, Constants.Delimiters.ParamDelimiter);
                if (ImportResult.Length > 0 && !string.IsNullOrEmpty(ImportResult[0]))
                {
                    if (ImportResult[0] == "true")
                    {//If files imported successfully show success message 
                        ShowMessage(Constants.XLSImportMessage.ImportSuccess.ToString());

                        #region "Call method to write log in csv file"
                        if (!string.IsNullOrEmpty(ImportResult[1]))
                        {
                            XLSFileMsg = ImportResult[1];
                        }
                        WriteLogInCSVFile(Constants.AdminModules.LanguageSettings.ToString(), XLSFileMsg);
                        #endregion
                    }
                    else if (ImportResult[0] == "false")
                    {
                        if (!string.IsNullOrEmpty(ImportResult[1]))
                        {
                            ShowMessage(ImportResult[1]);
                        }
                    }
                    else {
                        ShowMessage(Constants.XLSImportMessage.ImportFalied.ToString());
                    }
                }
                else//else show faliure message
                {
                    ShowMessage(Constants.XLSImportMessage.ImportFalied.ToString());
                }
            }
            else
            { // show message that only xls file is supported
                ShowMessage(Constants.XLSImportMessage.ImportFileExtension.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showUpload", "ShowUploadDiv();", true);
            }
        }
        else
        { // show message to select file               
            ShowMessage(Constants.XLSImportMessage.ImportSelectFIle.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "showUpload1", "ShowUploadDiv();", true);
        }

    }

    private void ShowMessage(string Message)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + Message + "');", true);
    }
    //private DataTable ReadXLSFiles(string XMLFilesFullPath, string fileName)
    //{
    //    DataTable RetVal = new DataTable();
    //    bool hasHeaders = false;
    //    string HDR = hasHeaders ? "Yes" : "No";
    //    string strConn;
    //    //create connection string for oledb connection from CSV File
    //    if (Environment.Is64BitProcess)
    //    {
    //        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
    //    }
    //    else
    //    {
    //        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + XMLFilesFullPath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
    //    }
    //    //DataTable RetVal = new DataTable();
    //    OleDbCommand cmd;
    //    string DataselectionQuery = string.Empty;
    //    using (OleDbConnection conn = new OleDbConnection(strConn))
    //    {
    //        conn.Open();
    //        try
    //        {
    //            cmd = new OleDbCommand("SELECT * FROM [" + XMLFilesFullPath + "]", conn);
    //            cmd.CommandType = CommandType.Text;
    //            RetVal = new DataTable(fileName);
    //            new OleDbDataAdapter(cmd).Fill(RetVal);

    //        }
    //        catch (Exception ex)
    //        {
    //            throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", fileName, fileName), ex);
    //        }
    //        finally
    //        {
    //            conn.Close();
    //        }

    //    }
    //    return RetVal;
    //}

    #region "write log in CSV file"
    private void WriteLogInCSVFile(string ModuleName, string XLSFileMsg)
    {
        string UserName = string.Empty;
        string UserEmailID = string.Empty;
        string OldValue = string.Empty;
        string ClientIpAddress = string.Empty;

        if (!string.IsNullOrEmpty(Session[Constants.SessionNames.LoggedAdminUser].ToString()))
        {
            UserName = Session[Constants.SessionNames.LoggedAdminUser].ToString();
        }
        if (!string.IsNullOrEmpty(Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
        {
            UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
        }
        HttpRequest Request = base.Request;
        ClientIpAddress = Request.UserHostAddress;
        XLSLogGenerator.WriteLogInXLSFile(UserName, ModuleName, XLSFileMsg, UserEmailID, ClientIpAddress);


    }
    #endregion

    #endregion

    #endregion
}
