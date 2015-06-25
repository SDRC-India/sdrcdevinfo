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
using System.IO;

public partial class libraries_aspx_Admin_DbEdit3 : System.Web.UI.Page
{
    #region "--Public/Protected--"

    #region "--Variables--"

    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string ShowSiteMap = string.Empty;
    protected string IsSiteMapGenerated = string.Empty;

    #endregion

    #region "--Events/Methods--"

    protected void Page_Load(object sender, EventArgs e)
    {
        string DbId = string.Empty;
        string DefaultIndicators = string.Empty;
        string DefaultAreas = string.Empty;

        try
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


                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    //Get DbId from query string
                    DbId = Request.QueryString["id"].ToString();

                    DefaultIndicators = Global.GetDbXmlAttributeValue(DbId, Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicator);
                    if (!string.IsNullOrEmpty(DefaultIndicators))
                    {
                        //this.lblIndCounts.Text = Global.SplitString(DefaultIndicators, ",").Length.ToString();
                        this.lblIndCounts.InnerHtml = Global.SplitString(DefaultIndicators, ",").Length.ToString();
                    }

                    DefaultAreas = Global.GetDbXmlAttributeValue(DbId, Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultArea);
                    if (!string.IsNullOrEmpty(DefaultAreas))
                    {
                        this.lblAreaCounts.InnerHtml = Global.SplitString(DefaultAreas, ",").Length.ToString();
                    }


                    //Assign default indicator json data into hidden field control
                    this.hdnSelIndO.Value = Global.GetDbXmlAttributeValue(DbId, Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultIndicatorJSON);

                    //Assign default area json data into hidden field control
                    this.hdnSelAreaO.Value = Global.GetDbXmlAttributeValue(DbId, Constants.XmlFile.Db.Tags.DatabaseAttributes.DefaultAreaJSON);
                    
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


                    // get default database lng code
                    hlngcodedb = Global.GetDefaultLanguageCodeDB(DbId, hlngcode);
                    Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode), hlngcodedb, Page);

                    if (File.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "sitemap.xml")))
                    { IsSiteMapGenerated = "true"; }
                    else { IsSiteMapGenerated = "false"; }

                    if (Global.isSitemapLinkVisible == "false")
                    {ShowSiteMap = "false"; }
                    else
                    { ShowSiteMap = "true";}
                }
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
