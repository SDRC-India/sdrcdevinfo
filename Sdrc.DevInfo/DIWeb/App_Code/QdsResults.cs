using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for QdsResults
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class QdsResults : System.Web.Services.WebService {

    public QdsResults () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string getQdsResults(string SearchIndicators, string SearchAreas, string LanguageCode, bool IsBlockResultsNeeded) {
        string RetVal = string.Empty;

        Callback objCallback = new Callback(new System.Web.UI.Page());
        RetVal = objCallback.GetCatalogQdsResults(SearchAreas, SearchIndicators, LanguageCode, IsBlockResultsNeeded);


        return RetVal;
    }

    [WebMethod]
    public string getShareCatalogResults(string requestParam)
    {
        string RetVal = string.Empty;
        Callback objCallback = new Callback(new System.Web.UI.Page());
        RetVal = objCallback.ShareSearchResult(requestParam);
        return RetVal;
    }
    
}
