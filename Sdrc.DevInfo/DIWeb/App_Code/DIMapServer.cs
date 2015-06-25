using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Summary description for DIMapServer
/// </summary>
public static class DIMapServer
{
    public readonly static DIMapServerWS.Utility WebServiceInstance = DIMapServer.GetInstance();

    private static DIMapServerWS.Utility GetInstance()
    {
        DIMapServerWS.Utility RetVal = null;
        if (WebServiceInstance == null)
        {
            RetVal = new DIMapServerWS.Utility();
            RetVal.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UtilityWebService;
        }
        return RetVal;
    }
}