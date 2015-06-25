using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class libraries_aspx_Article : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        // check if admin is not logged in, then redirect user to login page
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            Response.Redirect("login.aspx");
        }

        GetPostedData();
    }
    private void GetPostedData()
    {
        try
        {
            // Get Posted Data - will be passed to the Javascript

            // Set Data Search by
            if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
            else
            {
                hdsby = "ArticleImport";
            }

            //Set database NId - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hdbnid"]))
            {
                // -- check in the posetd data first
                hdbnid = Request["hdbnid"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value)))
            {
                // then check in the cookie
                hdbnid = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value;
            }
            else
            {
                // get default db nid ;
                hdbnid = Global.GetDefaultDbNId();
            }
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId), hdbnid, Page);

            // Set Selected Areas - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselarea"]))
            {
                hselarea = Global.formatQuoteString(Request["hselarea"]);
                Session["hselarea"] = Request["hselarea"];
            }
            else
            {
                hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
            }

            // Set Selected Indicators - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselind"]))
            {
                hselind = Global.formatQuoteString(Request["hselind"]);
                Session["hselind"] = Request["hselind"];
            }
            else
                hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

            // Set selected Indicator JSON object
            if (!string.IsNullOrEmpty(Request["hselindo"]))
            {
                hselindo = Global.formatQuoteString(Request["hselindo"]);
                Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
            }
            else
                hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

            // Set selected area JSON object
            if (!string.IsNullOrEmpty(Request["hselareao"]))
            {
                hselareao = Global.formatQuoteString(Request["hselareao"]);
                Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
            }
            else
                hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;

            // Set language code   
            if (!string.IsNullOrEmpty(Request["hlngcode"]))
            {
                // -- check in the posetd data first
                hlngcode = Request["hlngcode"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
            {
                // then check in the cookie
                hlngcode = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
            }
            else
            {
                // get default lng code
                hlngcode = Global.GetDefaultLanguageCode();
            }
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode), hlngcode, Page);


            //Set Database language code

            // get default database lng code
            hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode), hlngcodedb, Page);
            if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"]))
            {
                hLoggedInUserNId = Request["hLoggedInUserNId"];
                Session["hLoggedInUserNId"] = Request["hLoggedInUserNId"];
            }
            else
                hLoggedInUserNId = Session["hLoggedInUserNId"] != null ? Session["hLoggedInUserNId"].ToString() : null;
            if (!string.IsNullOrEmpty(Request["hLoggedInUserName"]))
            {
                hLoggedInUserName = Request["hLoggedInUserName"];
                Session["hLoggedInUserName"] = Request["hLoggedInUserName"];
            }
            else
                hLoggedInUserName = Session["hLoggedInUserName"] != null ? Session["hLoggedInUserName"].ToString() : null;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    public void BtnImportContent_Click(object sender, EventArgs e)
    {
        bool IsDataImported = false;
        List<DataContent> NewsHtmlContentList = null;
        List<DataContent> ActionHtmlContentList = null;
        List<DataContent> AllContentList = null;
        string OperationStatusMessage = string.Empty;
        // check if admin is not logged in then redirect user to login page
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            Response.Redirect("login.aspx");
        }
        else
        {
            // create instance of class HtmlContentReader
            HtmlContentReader ContentReader = new HtmlContentReader();

            //get list containing all news content
            NewsHtmlContentList = ContentReader.ReadNewsHtmlContent();
            //get list containing all action content
            ActionHtmlContentList = ContentReader.ReadActionHtmlContent();

            AllContentList = NewsHtmlContentList;
            AllContentList.AddRange(ActionHtmlContentList);

            // create instance of class HtmlContentImporter
            HtmlContentMigrator HtmlImport = new HtmlContentMigrator();
            // call method to insert values in database        
            IsDataImported = HtmlImport.MigratorHtmlContentToDb(AllContentList, out OperationStatusMessage);
            if (IsDataImported)
            {
                LblImportSuccessMsg.Style["display"] = "block";
                LblImportFailureMsg.Style["display"] = "none";
            }
            else
            {
                LblImportFailureMsg.Text = OperationStatusMessage;
                LblImportSuccessMsg.Style["display"] = "none";
                LblImportFailureMsg.Style["display"] = "block";
            }
        }

    }
}