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

public partial class libraries_aspx_fb_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string callbackPath = Request.Url.AbsoluteUri.Replace(Request.Url.Segments[Request.Url.Segments.Length - 1], "") + "Callback.aspx";

        if (!string.IsNullOrEmpty(Request.Url.Query)) callbackPath = callbackPath.Replace(Request.Url.Query, "");

        if (!string.IsNullOrEmpty(Request.QueryString["wall_post"]))
        {
            if (Request.QueryString["wall_post"].Contains("/$articles$/"))
            {
                Session["fbWallPost"] = Request.QueryString["wall_post"].Replace("/$articles$/", "/articles/");
                Session["PostArticleForCms"] = true;
            }
            else
            {
                Session["fbWallPost"] = Request.QueryString["wall_post"];
            }
            if (Request.QueryString["picture_url"] != null && !string.IsNullOrEmpty(Request.QueryString["picture_url"].ToString()))
            {
                Session["fbPictureUrl"] = Request.QueryString["picture_url"];
            }
            if (Request.QueryString["title"]!= null && !string.IsNullOrEmpty(Request.QueryString["title"].ToString()))
            {
                Session["fbTitle"] = Request.QueryString["title"];
            }
            if (Request.QueryString["desc"] != null && !string.IsNullOrEmpty(Request.QueryString["desc"].ToString()))
            {
                Session["fbDesc"] = Request.QueryString["desc"].Replace("<br/>", "\n");
            }          
            if (Request.QueryString["valArticleUrl"] != null && !string.IsNullOrEmpty(Request.QueryString["valArticleUrl"].ToString()))
            {
                Session["CmsArticleUrl"] = Request.QueryString["valArticleUrl"].Replace("/$articles$/", "/articles/");
            }
            
            //  Response.Redirect(String.Format("http://twitter.com/oauth/authorize?oauth_token={0}", tk.Token), true);
        }
        Session["fbCallback"] = callbackPath;
        oAuthFacebook oFB = new oAuthFacebook(callbackPath);
        Response.Redirect(oFB.AuthorizationLinkGet());
    }
}
