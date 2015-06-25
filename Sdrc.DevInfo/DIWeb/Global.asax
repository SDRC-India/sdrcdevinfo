<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }

    void Application_BeginRequest(object sender, EventArgs e)
    {
        string OriginalURL;
        
        OriginalURL = HttpContext.Current.Request.Url.AbsoluteUri;
        
        if (OriginalURL.Contains("/REST/"))
        {
            HttpContext.Current.RewritePath("~/libraries/ws/REST.aspx?info=" + OriginalURL.Substring(OriginalURL.IndexOf("/REST/") + "/REST/".Length));
            //HttpContext.Current.RewritePath("~/libraries/ws/REST.aspx");
        }
        else if (OriginalURL.Contains("RegistryService.asmx?WSDL"))
        {
            HttpContext.Current.RewritePath("~/libraries/ws/SDMX.wsdl");
        }       
    }
    
    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Global.WriteErrorsInLogFolder("Application_Error");
    }

    void Session_Start(object sender, EventArgs e)
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e)
    {        
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }
       
</script>
