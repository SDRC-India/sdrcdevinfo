using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_Admin_RegHeaderDetail : System.Web.UI.Page
{
    protected string hlngcode = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string IsSDMXHeaderCreated = string.Empty;
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
            this.GetPostedData();
            string dbNId=Global.GetDefaultDbNId();
         //  IsSDMXHeaderCreated = Global.GetNCreateDefaultAppSettingKey(Constants.AppSettingKeys.IsSDMXHeaderCreated, "false");

            IsSDMXHeaderCreated = Global.GetDbXmlAttributeValue(dbNId, Constants.XmlFile.Db.Tags.DatabaseAttributes.IsSDMXHeaderCreated);
        }
    }

    private void GetPostedData()
    {
        try
        {
            // Set language code   
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
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