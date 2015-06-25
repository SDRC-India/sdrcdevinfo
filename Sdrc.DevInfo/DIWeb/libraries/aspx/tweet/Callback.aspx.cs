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
public partial class libraries_aspx_tweet_Callback : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string ConsumerKey = Global.twAppID; //
        string ConsumerSecret = Global.twAppSecret; // 
        string userAccessToken = string.Empty;
        string userAccessSecret = string.Empty;
        string userName = string.Empty;
        try
        {
            Global.WriteErrorsInLogFolder("tweet callback page Request.QueryString[oauth_token] is" + Request.QueryString["oauth_token"]);
            try
            {
                OAuthTokenResponse token = OAuthUtility.GetAccessToken(
                    ConsumerKey,
                    ConsumerSecret,
                    Request.QueryString["oauth_token"], "");
                userAccessToken = token.Token;
                userAccessSecret = token.TokenSecret;
                userName = token.ScreenName;
            }
            catch (Exception Ex)
            {
                Global.WriteErrorsInLogFolder("at tweet callback exception in OAuthUtility.GetAccessToken " + Ex.Message + "-----------------" + Ex.InnerException + "-----------------------" + Ex.StackTrace);
            }

            OAuthTokens tokens = new OAuthTokens();
            tokens.AccessToken = userAccessToken; // 
            tokens.AccessTokenSecret = userAccessSecret; // 
            tokens.ConsumerKey = ConsumerKey;
            tokens.ConsumerSecret = ConsumerSecret;


            Global.WriteErrorsInLogFolder("at page load userAccessToken is" + userAccessToken);
            Global.WriteErrorsInLogFolder("at page load userAccessSecret is" + userAccessSecret);
            Global.WriteErrorsInLogFolder("at page load ConsumerSecret is" + ConsumerSecret);
            Global.WriteErrorsInLogFolder("at page load ConsumerKey is" + ConsumerKey);

            string strTweet = Session["Tweet"].ToString();

            TwitterResponse<TwitterStatus> tweetResponse = TwitterStatus.Update(tokens, strTweet);

            Global.WriteErrorsInLogFolder("at page tweet response result is" + tweetResponse.Result);
            if (tweetResponse.Result == RequestResult.Success)
            {
                Response.Write("<script language='javascript'> { self.close() }</script>");
                Session["Tweet"] = "";
                //Response.Write("Dear " + userName + ", Tweet is done! Please check your twitter...");
            }
            else
            {
                //Response.Write("Dear " + userName + ", Tweet failed due to some reasons. Please close this window.");
            }
        }

        catch (Exception ex)
        {
            //Response.Write("ERROR: " + ex.Message + ":" + Request.QueryString["oauth_token"]);
            Response.Write("<script language='javascript'> { self.close() }</script>");
            Session["Tweet"] = "";
            //Response.Redirect("~/libraries/aspx/Home.aspx");
            Global.WriteErrorsInLogFolder("at tweet callback exception is" + ex.Message + "-----------------" + ex.InnerException + "-----------------------" + ex.StackTrace);
        }
    }


}
