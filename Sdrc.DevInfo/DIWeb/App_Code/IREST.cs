using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IREST" in both code and config file together.
[ServiceContract]
public interface IREST
{
    [OperationContract]
    [WebGet(UriTemplate = "{dbNId}/{language}/SDMX/data/{sdmxFormat}/{key}")]
    string GetDataQueryResponseSDMXML(string dbNId, string language, string sdmxFormat, string key);

    [OperationContract]
    [WebGet(UriTemplate = "{dbNId}/{language}/XML/{key}")]
    string GetDataQueryResponseXML(string dbNId, string language, string key);

    [OperationContract]
    [WebGet(UriTemplate = "{dbNId}/{language}/JSON/{key}")]
    string GetDataQueryResponseJSON(string dbNId, string language, string key);
}
