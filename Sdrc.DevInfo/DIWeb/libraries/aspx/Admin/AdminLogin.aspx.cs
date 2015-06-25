using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class libraries_aspx_Admin_Login : System.Web.UI.Page
{

    protected string hlngcode = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        //Session[Constants.SessionNames.LoggedAdminUser] = null;
        if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
        {
            hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
        }
        // Read AppSettings
        Global.GetAppSetting();

        // Set page title
        Page.Title = Global.adaptation_name;

        this.GetPostedData();
    }

    private void GetPostedData()
    {
        try
        {
            // Set language code 
            string LanguageCode = string.Empty;
            DataTable DTLanguage = null;
            string RegionalLanguage = string.Empty;
            string[] languages = Request.UserLanguages;
            if (!(languages == null || languages.Length == 0))
            {
                RegionalLanguage = languages[0].ToString().Substring(0, 2);
            }
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
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
            // Get Posted Data - will be passed to the Javascript            

            if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }

            //if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
            if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
            {
                hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
}