Imports Twitterizer

Partial Class _Default
    Inherits System.Web.UI.Page

    
    Protected Sub form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles form1.Load

        Dim callbackPath As String = Request.Url.AbsoluteUri.Replace(Request.Url.Segments(Request.Url.Segments.Length - 1), "") + "Callback.aspx"
        callbackPath = callbackPath.Replace(Request.Url.Query, "")

        Dim ConsumerKey As String = "NfcDh2xxvDKQoPxvAA"
        Dim ConsumerSecret As String = "pDDQcdcaKsGcPz6DMo6z4g6opsQFt3FAMqmGR0OQ"
        Dim tokens As OAuthTokenResponse = Twitterizer.OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, callbackPath)
        Session("Tweet") = Request.QueryString("PivotLink")
        Response.Redirect(String.Format("http://twitter.com/oauth/authorize?oauth_token={0}", tokens.Token), True)
        
    End Sub
End Class
