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
using System.Text;
using System.IO;
using SDMXObjectModel;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using System.Data.OleDb;
using System.Collections.Generic;

public partial class libraries_aspx_RegMapping : System.Web.UI.Page
{
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string hLoggedInUserName = string.Empty;
    protected string hOriginaldbnid = string.Empty;
    protected string hIsUploadedDSD = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set pate title
            Page.Title = Global.adaptation_name + " - Registry - " + Constants.Pages.Registry.Mapping;

            // Read http header or cookie values
            GetPostedData();
            bool IsKeyFound = false;
            string ProtectedValue = string.Empty;
            if (ConfigurationManager.AppSettings["AppProtected"] != null)
            {
                IsKeyFound = true;
                ProtectedValue = ConfigurationManager.AppSettings["AppProtected"];
            }
            if (IsKeyFound && ProtectedValue == "true" && Session["hLoggedInUserNId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                ((libraries_aspx_RegistryMaster)this.Master).Populate_Select_DSD_DropDown(hdbnid);
                ((libraries_aspx_RegistryMaster)this.Master).ShowHide_Select_DSD_DropDown(Global.enableDSDSelection);
            }
            
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
        if (!string.IsNullOrEmpty(Request["hdbnid"]))
        {

            hdbnid = Request["hdbnid"];
            //if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            //{
            //    hdbnid = Global.GetDefaultDSDNId();
            //}
            //else
            //{
            //    hdbnid=Request["hdbnid"];
            //}
        }
        if (!string.IsNullOrEmpty(Request["hselarea"])) { hselarea = Request["hselarea"]; }
        if (!string.IsNullOrEmpty(Request["hselind"])) { hselind = Request["hselind"]; }
        if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }
        if (!string.IsNullOrEmpty(Request["hlngcodedb"])) { hlngcodedb = Request["hlngcodedb"]; }
        if (!string.IsNullOrEmpty(Request["hselindo"])) { hselindo = Request["hselindo"]; }
        if (!string.IsNullOrEmpty(Request["hselareao"])) { hselareao = Request["hselareao"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserName"])) { hLoggedInUserName = Request["hLoggedInUserName"]; }

        if (Request["hOriginaldbnid"] != null && !string.IsNullOrEmpty(Request["hOriginaldbnid"]))
        {
            hOriginaldbnid = Request["hOriginaldbnid"];
        }
        else
        {
            hOriginaldbnid = Global.GetDefaultDbNId();
        }

        if (Global.GetDefaultDSDNId() != string.Empty && hdbnid==Global.GetDefaultDSDNId())
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(Global.GetDefaultDSDNId())).ToString();
        }
        else
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(hdbnid)).ToString();
        }
    }


    //protected void btnUploadXLS_Click(object sender, EventArgs e)
    //{
    //    string FileName = string.Empty;
    //    string FileExtension = string.Empty;
    //    string FileFullPath = string.Empty;
    
    //    string RetResult = string.Empty;
       

    //    ///Variables for creatin CSVLogfile 
    //    string UserName = string.Empty;
    //    string XLSFileMsg = string.Empty;
    //    string SrcXMLfileName = string.Empty;
    //    string DestXMLFileName = string.Empty;

    //    if (FileUpdXLS.HasFile)
    //    {
    //        FileName = FileUpdXLS.FileName;// Name of file with extension
    //        // Get File extension from file name
    //        FileExtension = Path.GetExtension(FileUpdXLS.PostedFile.FileName).Substring(1);
    //        // Allow further processing if input file is xls file
    //        if (FileExtension.ToLower() == "xls")
    //        {
    //            FileFullPath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.TemporaryXLS, FileName); ;
    //            FileUpdXLS.SaveAs(Path.Combine(FileFullPath));
    //            RetResult = "true";
    //        }
    //        else
    //        { // show message that only xls file is supported
    //            ShowMessage(Constants.XLSImportMessage.ImportFileExtension.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showUpload", "ShowUploadDiv();", true);
    //        }
    //    }
    //    else
    //    { // show message to select file               
    //        ShowMessage(Constants.XLSImportMessage.ImportSelectFIle.ToString());
    //        ScriptManager.RegisterStartupScript(this, GetType(), "showUpload1", "ShowUploadDiv();", true);
    //    }

    //}

    //private void ShowMessage(string Message)
    //{
    //    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('" + Message + "');", true);
    //}
                
}