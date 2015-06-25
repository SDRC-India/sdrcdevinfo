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
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;

public partial class libraries_aspx_Home : System.Web.UI.Page
{
    #region "-- Private --"

    #region "-- Methods --"

    private void GetDbAdminitratorDetails()
    {
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;

        try
        {
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);

            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            DbAdmName = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmName);
            DbAdmInstitution = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmInstitution);
            DbAdmEmail = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmEmail);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void GetPostedData()
    {
        try
        {
            string LanguageCode = string.Empty;
            DataTable DTLanguage = null;
            string RegionalLanguage = string.Empty;
            //Get Posted Data - will be passed to the Javascript
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
                hdbnid = Global.GetDefaultDbNId(); //Request["hdbnid"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value)))
            {
                // then check in the cookie
                //  hdbnid = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value;
                hdbnid = Global.GetDefaultDbNId();
            }
            else
            {
                // get default db nid ;
                hdbnid = Global.GetDefaultDbNId();
            }

            if (!Global.IsDbIdExists(hdbnid))
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
                hselindo = formatQuoteString(Request["hselindo"]);
                Session["hselindo"] = formatQuoteString(Request["hselindo"]);
            }
            else
                hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

            // Set selected area JSON object
            if (!string.IsNullOrEmpty(Request["hselareao"]))
            {
                hselareao = formatQuoteString(Request["hselareao"]);
                Session["hselareao"] = formatQuoteString(Request["hselareao"]);
            }
            else
                hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;

            // Set selected Cache NIds object
            if (!string.IsNullOrEmpty(Request["hdvnids"]))
            {
                hdvnids = formatQuoteString(Request["hdvnids"]);
                Session["hdvnids"] = formatQuoteString(Request["hdvnids"]);
            }
            else
                hdvnids = Session["hdvnids"] != null ? Session["hdvnids"].ToString() : null;
            //
            // Set language code  
            if (Request.QueryString["lngCode"] != null)
            {
                DTLanguage = Global.GetAllDBLangaugeCodes();
                if (DTLanguage != null)
                {
                    foreach (DataRow dr in DTLanguage.Rows)
                    {
                        if (dr["Language_Code"].ToString() == Request.QueryString["lngCode"].ToString())
                        {
                            LanguageCode = Request.QueryString["lngCode"].ToString();
                            break;
                        }
                    }
                }
            }
            else if (Request.QueryString["jsonAreasTopics"] != null)
            {
                try
                {
                    string[] QSArray = Request.QueryString["jsonAreasTopics"].Split(new string[] { "," }, StringSplitOptions.None);
                    if (QSArray.Length >= 6)
                    {
                        string QALngCode = QSArray[5].Split(new string[] { ":" }, StringSplitOptions.None)[1].Replace("\"", "").Replace("}", "");
                        DTLanguage = Global.GetAllDBLangaugeCodes();
                        if (DTLanguage != null)
                        {
                            foreach (DataRow dr in DTLanguage.Rows)
                            {
                                if (dr["Language_Code"].ToString() == QALngCode)
                                {
                                    LanguageCode = QALngCode;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (!string.IsNullOrEmpty(LanguageCode))
            {
                hlngcode = LanguageCode;
            }
            else if (!string.IsNullOrEmpty(Request["hlngcode"]))
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


            //Get Database Language
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

            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode)].Value)))
            {
                // then check in the cookie
                hlngcodedb = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode)].Value;
            }
            else if (!string.IsNullOrEmpty(LanguageCode))
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
            else if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
            {
                hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }
            else
                hLoggedInUserNId = Session["hLoggedInUserNId"] != null ? Session["hLoggedInUserNId"].ToString() : null;

            if (!string.IsNullOrEmpty(Request["hLoggedInUserName"]))
            {
                hLoggedInUserName = Request["hLoggedInUserName"];
                Session["hLoggedInUserName"] = Request["hLoggedInUserName"];
            }
            else if (Session[Constants.SessionNames.LoggedAdminUser] != null && Session[Constants.SessionNames.LoggedAdminUser].ToString() != "")
            {
                hLoggedInUserName = Session[Constants.SessionNames.LoggedAdminUser].ToString();
            }
            else
                hLoggedInUserName = Session["hLoggedInUserName"] != null ? Session["hLoggedInUserName"].ToString() : null;


            // Get show sliders status in variable
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)].Value)))
            {
                // then check in the cookie
                ShowSliders = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)].Value;
            }
            else
            {
                // get show sliders value from appsettings.xml
                ShowSliders = Global.GetShowSlidersValue();
                Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders), ShowSliders, Page);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private string formatQuoteString(string SingleQuotedString)
    {
        string result = string.Empty;

        result = SingleQuotedString.Replace(@"'", @"\'");

        while (result.IndexOf(@"\\'") != -1)
        {
            result = result.Replace(@"\\'", @"\'");
        }

        return result;
    }

    #endregion

    #endregion

    #region "-- Public/Protected --"

    #region "-- Variables --"

    // manage posting data
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hdvnids = string.Empty;
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
    protected string isGalleryEnabled = "false";
    protected string isQdsToBeRendered = "false";
    protected string DbAdmName = string.Empty;
    protected string DbAdmInstitution = string.Empty;
    protected string DbAdmEmail = string.Empty;
    protected string hShowCloud = "false";
    protected string gaUniqueID = string.Empty;

    #endregion

    #region "-- Methods/Events --"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            // Read AppSettings
            Global.GetAppSetting();
            string QueryString = string.Empty;
            string QueryStrRefUrl = string.Empty;
            if (Request.QueryString.Count > 0)
            {
                try
                {                    
                    QueryString = Request.QueryString["jsonAreasTopics"].ToString();
                    if (!string.IsNullOrEmpty(QueryString))
                    {
                        string MetaValues = Global.GenerateMetaValues(QueryString);
                        Page.Title = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[0].ToString();
                        Page.MetaKeywords = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[1].ToString();
                        Page.MetaDescription = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[2].ToString();
                    }
                    // read query string refer_url
                    QueryStrRefUrl = Request.QueryString["refer_url"].ToString();
                    if (!string.IsNullOrEmpty(QueryStrRefUrl))
                    {
                        // if request is comming from site map hide silder
                        Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders), "false", Page);
                    }
                }
                catch (Exception)
                {
                    Page.Title = Global.adaptation_name;
                }
            }
            else
            {
                Page.Title = Global.adaptation_name;
            }
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
                // Set page title
                isGalleryEnabled = Global.enableQDSGallery;
                hShowCloud = Global.show_cloud;
                // Set default area count of selected database            
                DefAreaCount = Global.GetDefaultAreaCount(hdbnid);

                this.GetDbAdminitratorDetails();

                // Persist QDS search results/Cart or not?

                if (Request.UrlReferrer != null
                    && Request.Url.Authority == Request.UrlReferrer.Authority
                    && !Request.UrlReferrer.AbsoluteUri.ToString().ToLower().EndsWith("index.htm")
                    )
                    isQdsToBeRendered = "true";

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #endregion
}
