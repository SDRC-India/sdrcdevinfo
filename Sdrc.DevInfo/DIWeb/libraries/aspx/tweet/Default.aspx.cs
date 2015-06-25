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

using Twitterizer;
public partial class libraries_aspx_tweet_d : System.Web.UI.Page
{
    string ConsumerKey = Global.twAppID;
    string ConsumerSecret = Global.twAppSecret;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            Global.WriteErrorsInLogFolder("ConsumerKey is: " + ConsumerKey);
            Global.WriteErrorsInLogFolder("ConsumerSecret key is: " + ConsumerSecret);

            string callbackPath = Request.Url.AbsoluteUri.Replace(Request.Url.Segments[Request.Url.Segments.Length - 1], "") + "Callback.aspx";
            Global.WriteErrorsInLogFolder("callbackPath " + callbackPath);

            if (!string.IsNullOrEmpty(Request.Url.Query)) callbackPath = callbackPath.Replace(Request.Url.Query, "");

            Global.WriteErrorsInLogFolder("create object of OAuthUtility.GetRequestToken ");
            Global.WriteErrorsInLogFolder("default page callbackPath path passed in OAuthUtility.GetRequestToken " + callbackPath);
            OAuthTokenResponse tk = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, callbackPath);
            Global.WriteErrorsInLogFolder("atrer create object of OAuthUtility.GetRequestToken token is:" + tk.Token);
            if (!string.IsNullOrEmpty(Request.QueryString["tweet"]))
            {
                if (Request.QueryString["tweet"].Contains("/$articles$/"))
                {
                    Session["Tweet"] = Request.QueryString["tweet"].Replace("/$articles$/", "/articles/");
                }
                else
                {
                    Session["Tweet"] = Request.QueryString["tweet"];

                }
                Response.Redirect(String.Format("https://api.twitter.com/oauth/authorize?oauth_token={0}", tk.Token), true);
            }

        }

        catch (Exception ex)
        {
            Global.WriteErrorsInLogFolder("ERROR: " + ex.Message);
            Response.Write("ERROR: " + ex.Message);
        }
    }

}
