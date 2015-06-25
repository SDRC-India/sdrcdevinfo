using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Configuration;

/// <summary>
/// Summary description for DI7AppServicePage
/// </summary>
[WebService(Namespace = "http://DIworldwide/DIWWS/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class DI7AppServicePage : System.Web.Services.WebService {

    public DI7AppServicePage () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Get the QDS Result based on What and Where condition
    /// </summary>
    /// <param name="indicatorNId">Indicator NId</param>
    /// <param name="iCNId">IC NId</param>
    /// <param name="areaNId">Area NId</param>
    /// <param name="languageCode">Language Code</param>
    /// <returns></returns>
    [WebMethod]
    public string GetQDSResults(string indicatorNId, string iCNId, string areaNId, string languageCode, string dBNId)
    {   
        string RetVal = string.Empty;
        Callback objCB = new Callback();
        RetVal = objCB.GetQDSResults(indicatorNId + Constants.Delimiters.ParamDelimiter + iCNId + Constants.Delimiters.ParamDelimiter + areaNId + Constants.Delimiters.ParamDelimiter + languageCode + Constants.Delimiters.ParamDelimiter + dBNId);
        return RetVal;
    }

    /// <summary>
    /// Get Default DBNId and Default Language code
    /// </summary>
    /// <returns>XML Document</returns>
    [WebMethod]
    public XmlDocument GetDefaults()
    {
        XmlDocument RetVal = new XmlDocument();
        XmlElement Defaults = RetVal.CreateElement("Defaults");
        RetVal.AppendChild(Defaults);
        XmlElement DBID=RetVal.CreateElement("DBNID");
        DBID.InnerText = Global.GetDefaultDbNId().ToString();
        Defaults.AppendChild(DBID);
        XmlElement Language = RetVal.CreateElement("Language");
        Language.InnerText = Global.GetDefaultLanguageCode().ToString();
        Defaults.AppendChild(Language);
        return RetVal;
    }

    /// <summary>
    /// Get Database Metadata information for GUID
    /// </summary>
    /// <param name="gUID">Unique ID</param>
    /// <returns></returns>
    [WebMethod]
    public XmlNode GetDBMetaData()
    {
        XmlNode RetVal;
        //DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
        //XmlNode xmlNode = CatalogService.GetDBMetaData(gUID);
        //XmlDocument XMLDoc = new XmlDocument();
        //XmlNode declaration = XMLDoc.CreateNode(XmlNodeType.XmlDeclaration, null, null);
        //XMLDoc.AppendChild(declaration);
        //XmlElement DBDetail = XMLDoc.CreateElement("DBDetail");
        //DBDetail.AppendChild(xmlNode);
        //XMLDoc.AppendChild(DBDetail);
        //RetVal = XMLDoc;

        DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
        CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
        RetVal = CatalogService.GetDBMetaData(Global.GetAdaptationGUID());
        return RetVal;
    }
}
