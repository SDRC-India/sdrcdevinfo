using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for OptimizeDataBase
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class OptimizeDataBase : System.Web.Services.WebService {

    public OptimizeDataBase()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string OptimizeAdaptationDataBase()
    {
        string RetVal = string.Empty;
        Callback objCallback = new Callback();
        RetVal = objCallback.GenerateCacheResults("1" + Constants.Delimiters.ParamDelimiter + "General");
        return RetVal;
    }
}
