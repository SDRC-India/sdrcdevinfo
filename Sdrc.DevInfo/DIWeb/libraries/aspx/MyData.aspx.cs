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

public partial class libraries_aspx_MyData : System.Web.UI.Page
{
	// manage posting data
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
    protected string dbname = string.Empty;
    protected string dbsummary = string.Empty;
    protected string dbdesc = string.Empty;
    protected int DefAreaCount = 0;
    protected string ShowSliders = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        // Read AppSettings
        Global.GetAppSetting();

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
                Page.Title = Global.adaptation_name;
            }
        
    }

	private void GetPostedData()
    {
        try
        {
            // Get Posted Data - will be passed to the Javascript
            string LanguageCode = string.Empty;
            DataTable DTLanguage = null;
            string RegionalLanguage = string.Empty;
            string[] languages = Request.UserLanguages;
            if (!(languages == null || languages.Length == 0))
            {
                RegionalLanguage = languages[0].ToString().Substring(0, 2);
            }
            // Set Data Search by
            if (!string.IsNullOrEmpty(Request["hdsby"])) 
            { 
                hdsby = Request["hdsby"];
                Session["hdsby"] = Request["hdsby"];
            }
            else
                hdsby = Session["hdsby"] != null ? Session["hdsby"].ToString() : null;

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
                hselarea = Request["hselarea"];
                Session["hselarea"] = Request["hselarea"];
            }
            else
            {
                hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
            }
            
            // Set Selected Indicators - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselind"])) 
            { 
                hselind = Request["hselind"];
                Session["hselind"] = Request["hselind"];
            }
            else
                hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

            // Set selected Indicator JSON object
            if (!string.IsNullOrEmpty(Request["hselindo"])) 
            { 
                hselindo = Request["hselindo"];
                Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
            }
            else
                hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

            // Set selected area JSON object
            if (!string.IsNullOrEmpty(Request["hselareao"])) 
            { 
                hselareao = Request["hselareao"];
                Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
            }
            else
                hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;

                        
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
                LanguageCode = string.Empty;
                if (!string.IsNullOrEmpty(RegionalLanguage))
                {
                    DTLanguage = Global.GetAllDBLangaugeCodes();
                    if (DTLanguage != null)
                    {
                        foreach (DataRow dr in DTLanguage.Rows)
                        {
                            if (dr["Language_Code"].ToString() == RegionalLanguage)
                            {
                                LanguageCode = RegionalLanguage;
                                break;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(LanguageCode))
                {
                    hlngcode = LanguageCode;
                }
                else
                {
                    hlngcode = Global.GetDefaultLanguageCode();
                }
            }            
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode), hlngcode, Page);

            
            //Get Database language code                        
            LanguageCode = string.Empty;
            if (!string.IsNullOrEmpty(RegionalLanguage))
            {
                DTLanguage = Global.GetAllDBLangaugeCodes();
                if (DTLanguage != null)
                {
                    foreach (DataRow dr in DTLanguage.Rows)
                    {
                        if (dr["Language_Code"].ToString() == RegionalLanguage)
                        {
                            LanguageCode = RegionalLanguage;
                            break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(LanguageCode))
            {
                hlngcodedb = LanguageCode;
            }
            else
            {
                hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);
            }
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
}
