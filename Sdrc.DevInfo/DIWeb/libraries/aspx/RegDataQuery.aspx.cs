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

public partial class libraries_aspx_RegDataQuery : System.Web.UI.Page
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
        try
        {
            // Set page title
            Page.Title = Global.adaptation_name + " - " + Constants.Pages.Registry.DataQuery;

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
                // Read AppSettings
                Global.GetAppSetting();
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
        if (!string.IsNullOrEmpty(Request["hdsby"]))
        {
            hdsby = Request["hdsby"];
            Session["hdsby"] = Request["hdsby"];
        }
        else
            hdsby = Session["hdsby"] != null ? Session["hdsby"].ToString() : null;
        
        if (!string.IsNullOrEmpty(Request["hdbnid"]))
        {
            hdbnid = Request["hdbnid"];
            Session["hdbnid"] = Request["hdbnid"];
        }
        else if (Session["hdbnid"] != null)
        {
            hdbnid = Session["hdbnid"].ToString();
        }
        else
        {
            hdbnid = Global.GetDefaultDbNId();
            Session["hdbnid"] = Global.GetDefaultDbNId();
        }


        if (!string.IsNullOrEmpty(Request["hselarea"]))
        {
            hselarea = Request["hselarea"];
            Session["hselarea"] = Request["hselarea"];
        }
        else
            hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
       
        if (!string.IsNullOrEmpty(Request["hselind"]))
        {
            hselind = Request["hselind"];
            Session["hselind"] = Request["hselind"];
        }
        else
            hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

        if (!string.IsNullOrEmpty(Request["hlngcode"]))
        {
            hlngcode = Request["hlngcode"];
            Session["hlngcode"] = Request["hlngcode"];
        }
        else if (Session["hlngcode"] != null)
        {
            hlngcode = Session["hlngcode"].ToString();
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


        if (!string.IsNullOrEmpty(Request["hlngcodedb"]))
        {
            hlngcodedb = Request["hlngcodedb"];
            Session["hlngcodedb"] = Request["hlngcodedb"];
        }
        else if (Session["hlngcodedb"] != null)
        {
            hlngcodedb = Session["hlngcodedb"].ToString();
        }
        else
        {
            hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);
        }

        if (!string.IsNullOrEmpty(Request["hselindo"]))
        {
            hselindo = Global.formatQuoteString(Request["hselindo"]);
            Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
        }
        else
            hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;
       
        if (!string.IsNullOrEmpty(Request["hselareao"]))
        {
            hselareao = Global.formatQuoteString(Request["hselareao"]);
            Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
        }
        else
            hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;
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
}
