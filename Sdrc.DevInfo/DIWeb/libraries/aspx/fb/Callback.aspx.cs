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

public partial class libraries_aspx_fb_Callback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        oAuthFacebook oAuth = new oAuthFacebook(Session["fbCallback"].ToString());
        string PostArticleForCms = string.Empty;
        string json = string.Empty;
        string pictrue = string.Empty;
        string title = string.Empty;
        string desc = string.Empty;
        if (Request["code"] == null)
        {
            //Redirect the user back to Facebook for authorization.
            Response.Redirect(oAuth.AuthorizationLinkGet());
        }
        else
        {
            try
            {
                //Get the access token and secret.
                oAuth.AccessTokenGet(Request["code"]);

                if (oAuth.Token.Length > 0)
                {
                    string tmpWallPost = Session["fbWallPost"].ToString();
                    if (Session["fbPictureUrl"] != null && !string.IsNullOrEmpty((Session["fbPictureUrl"].ToString())))
                    {
                        pictrue = Session["fbPictureUrl"].ToString();
                    }
                    if (Session["fbTitle"] != null && !string.IsNullOrEmpty((Session["fbTitle"].ToString())))
                    {
                        title = Session["fbTitle"].ToString();
                    }
                    if (Session["fbDesc"] != null && !string.IsNullOrEmpty((Session["fbDesc"].ToString())))
                    {
                        desc = Session["fbDesc"].ToString();
                    }
                    if (Session["PostArticleForCms"] != null && !string.IsNullOrEmpty(Session["PostArticleForCms"].ToString()))
                    {
                        string ArticleLink = Session["CmsArticleUrl"].ToString();

                        // send image tag with query string only if image exists
                        if (!string.IsNullOrEmpty(pictrue))
                        {
                            //We now have the credentials, so we can start making API calls
                            json = oAuth.WebRequest(oAuthFacebook.Method.POST, "https://graph.facebook.com/me/feed",
                               "access_token=" + oAuth.Token + "&message=" + tmpWallPost + "&picture=" + pictrue + "&link=" + ArticleLink + "&name=" + title + "&caption=Powered By DevInfo&description=" + desc);
                        }
                        else
                        {//We now have the credentials, so we can start making API calls
                            json = oAuth.WebRequest(oAuthFacebook.Method.POST, "https://graph.facebook.com/me/feed",
                               "access_token=" + oAuth.Token + "&message=" + tmpWallPost + "&link=" + ArticleLink + "&name=" + title + "&caption=Powered By DevInfo&description=" + desc);
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(desc) && string.IsNullOrEmpty(title))
                        {
                             json = oAuth.WebRequest(oAuthFacebook.Method.POST, "https://graph.facebook.com/me/feed",
                                "access_token=" + oAuth.Token + "&message=" + tmpWallPost);
                            Response.Write("<script language='javascript'> { self.close() }</script>");
                            Session["fbWallPost"] = "";
                        }
                        else
                        {
                        
                        //We now have the credentials, so we can start making API calls
                        json = oAuth.WebRequest(oAuthFacebook.Method.POST, "https://graph.facebook.com/me/feed",
                           "access_token=" + oAuth.Token + "&message=" + tmpWallPost + "&picture=" + pictrue + "&name=Visuaization" + "&caption=" + title + "&description=" + desc);
                        //picture=http://fbrell.com/f8.jpg&
                        //name=Facebook%20Dialogs&
                        //caption=Reference%20Documentation&
                        //description=Using%20Dialogs%20to%20interact%20with%20users.&
                        }
                    }
                    Response.Write("<script language='javascript'> { self.close() }</script>");
                    Session.Remove("fbWallPost");
                    Session.Remove("fbPictureUrl");
                    Session.Remove("fbTitle");
                    Session.Remove("fbDesc");
                    Session.Remove("PostArticleForCms");
                    Session.Remove("CmsArticleUrl");
                }
            }
            catch (Exception Ex)
            {
                Global.WriteErrorsInLogFolder(Ex.Message + "-------------------------" + Ex.InnerException + "-------------------------" + Ex.StackTrace);
                Response.Write("<script language='javascript'> { self.close() }</script>");
                Session.Remove("fbWallPost");
                Session.Remove("fbPictureUrl");
                Session.Remove("fbTitle");
                Session.Remove("fbDesc");
                Session.Remove("PostArticleForCms");
                Session.Remove("CmsArticleUrl");
            }
        }
    }
}
