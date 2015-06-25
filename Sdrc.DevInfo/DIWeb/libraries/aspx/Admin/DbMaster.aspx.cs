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
using DI7.Lib.BAL.HTMLGenerator;
using DevInfo.Lib.DI_LibDAL.Connection;


public partial class libraries_aspx_Admin_DbList : System.Web.UI.Page
{
    protected string hlngcode = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    #region "--Private--"
    protected string adminId = string.Empty;
    #region "--Methods--"
        
    private void BindDatabaseGrid()
    {
        Callback objCallback;

        try
        {
            objCallback = new Callback(Page);                
            this.divDbList.InnerHtml = objCallback.AdminGetAllDbConnections("DevInfo");
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #endregion

    #region "--Public/Protected--"

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
            adminId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            // Set page title
            Page.Title = Global.adaptation_name;
                 
            this.BindDatabaseGrid();
            this.GetPostedData();
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
    #endregion

    #endregion
}
