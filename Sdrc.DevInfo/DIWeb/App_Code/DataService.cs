using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDATA;
using System.Xml.Serialization;
using System.Xml;

[WebService(Namespace = "http://www.devinfo.info/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class DataService : System.Web.Services.WebService
{
    #region "--Variables--"

    #region "--Private--"

    private DIConnection DIConnection;
    private DIQueries DIQueries;

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Constructors--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    public DataService()
    {
        int DBNId;

        if (this.Context.Request.QueryString["p"] != null && !string.IsNullOrEmpty(this.Context.Request.QueryString["p"]) &&
            int.TryParse(this.Context.Request.QueryString["p"], out DBNId))
        {
            this.DIConnection = Global.GetDbConnection(DBNId);
            this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
        }
        else
        {
            Global.GetAppSetting();
            this.DIConnection = Global.GetDbConnection(Convert.ToInt32(Global.GetDefaultDbNId()));
            this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
        }
    }

    #endregion "--Public--"

    #endregion "--Constructors--"

    #region "--Methods--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    [WebMethod]
    public string GetJSONData([XmlAnyElement] XmlDocument Input)
    {
        string RetVal;

        RetVal = DATAUtility.Get_Data(Input, DataTypes.JSON, DIConnection, DIQueries);

        return RetVal;
    }

    [WebMethod]
    public string GetXmlData([XmlAnyElement] XmlDocument Input)
    {
        string RetVal;

        RetVal = DATAUtility.Get_Data(Input, DataTypes.XML, DIConnection, DIQueries);

        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
