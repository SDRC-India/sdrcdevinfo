using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibSDMX;
using DevInfo.Lib.DI_LibDATA;
using System.Xml.Xsl;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Data;

public partial class libraries_ws_HTTP : System.Web.UI.Page
{
    #region "--Variables--"

    #region "--Private--"

    private DIConnection DIConnection;
    private DIQueries DIQueries;

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Method--"

    #region "--Private--"

    private XmlDocument GetQueryXML(string ResponseFormat, string SDMXFormat, string Indicator, string Area, string TimePeriod, string Source, string Language)
    {
        XmlDocument RetVal;
        Dictionary<string, string> DictUserSelections;

        RetVal = new XmlDocument();
        DictUserSelections = new Dictionary<string, string>();

        try
        {
            if (!string.IsNullOrEmpty(Indicator))
            {
                DictUserSelections.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, Indicator);
            }
            if (!string.IsNullOrEmpty(Area))
            {
                DictUserSelections.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, Area);
            }
            if (!string.IsNullOrEmpty(TimePeriod))
            {
                DictUserSelections.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, TimePeriod);
            }
            if (!string.IsNullOrEmpty(Source))
            {
                DictUserSelections.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id, Source);
            }
            if (!string.IsNullOrEmpty(Language))
            {
                DictUserSelections.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.FOOTNOTES.Id, Language);
            }

            if (!string.IsNullOrEmpty(ResponseFormat))
            {
                if (ResponseFormat == Constants.WSQueryStrings.ResponseFormatTypes.SDMX)
                {
                    if (!string.IsNullOrEmpty(SDMXFormat))
                    {
                        switch (SDMXFormat)
                        {
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS:
                                RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific:
                                RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.StructureSpecific, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.GenericTS:
                                RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.GenericTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.Generic:
                                RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.Generic, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                    }
                }
                else
                {
                    RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                }
            }
            else
            {
                RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictUserSelections, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return RetVal;
    }

    #endregion "--Private--"

    #region "--Public--"

    protected void Page_Load(object sender, EventArgs e)
    {
        int DBNId;
        string ResponseFormat, SDMXFormat, Indicator, Area, TimePeriod, Source, Language, DataResponse,Title, Footnote;
        XmlDocument QueryDocument, ResponseDocument;
        bool IsGroupBy=false;
        DBNId = 0;
        ResponseFormat = string.Empty;
        SDMXFormat = string.Empty;
        Indicator = string.Empty;
        Area = string.Empty;
        TimePeriod = string.Empty;
        Source = string.Empty;
        Language = string.Empty;
        DataResponse = string.Empty;
        Title = string.Empty;
        Footnote = string.Empty;
        QueryDocument = new XmlDocument();
        ResponseDocument = new XmlDocument();
        

        foreach (string QSComponent in Request.QueryString.Keys)
        {
            switch (QSComponent)
            {
                case Constants.WSQueryStrings.p:
                    if (this.Context.Request.QueryString[Constants.WSQueryStrings.p] != null && !string.IsNullOrEmpty(this.Context.Request.QueryString[Constants.WSQueryStrings.p]) && int.TryParse(this.Context.Request.QueryString[Constants.WSQueryStrings.p], out DBNId))
                    {
                        this.DIConnection = Global.GetDbConnection(DBNId);
                        this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
                    }
                    else
                    {
                        Global.GetAppSetting();
                        DBNId = Convert.ToInt32(Global.GetDefaultDbNId());
                        this.DIConnection = Global.GetDbConnection(Convert.ToInt32(Global.GetDefaultDbNId()));
                        this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
                    }
                    break;
                case Constants.WSQueryStrings.ResponseFormat:
                    if (Request.QueryString[Constants.WSQueryStrings.ResponseFormat] != null && !string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.ResponseFormat].ToString()))
                    {
                        ResponseFormat = Request.QueryString[Constants.WSQueryStrings.ResponseFormat].ToString();
                    }
                    else
                    {
                        ResponseFormat = Constants.WSQueryStrings.ResponseFormatTypes.SDMX;
                    }
                    break;
                case Constants.WSQueryStrings.SDMXFormat:
                    if (Request.QueryString[Constants.WSQueryStrings.SDMXFormat] != null && !string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.SDMXFormat].ToString()))
                    {
                        SDMXFormat = Request.QueryString[Constants.WSQueryStrings.SDMXFormat].ToString();
                    }
                    else
                    {
                        SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS; 
                    }
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id:
                    if (Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id] != null && !string.IsNullOrEmpty(Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id].ToString()))
                    {
                        Indicator = Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id].ToString();
                    }
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id:
                    if (Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id] != null && !string.IsNullOrEmpty(Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id].ToString()))
                    {
                        Area = Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id].ToString();
                    }
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id:
                    if (Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id] != null && !string.IsNullOrEmpty(Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id].ToString()))
                    {
                        TimePeriod = Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id].ToString();
                    }
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id:
                    if (Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id] != null && !string.IsNullOrEmpty(Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id].ToString()))
                    {
                        Source = Request.QueryString[DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id].ToString();
                    }
                    break;
                case Constants.WSQueryStrings.Language:
                    if (Request.QueryString[Constants.WSQueryStrings.Language] != null && !string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.Language].ToString()))
                    {
                        Language = Request.QueryString[Constants.WSQueryStrings.Language].ToString();
                    }
                    break;
                case Constants.WSQueryStrings.GroupByIndicator:
                    if(!string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.GroupByIndicator]))
                    {
                        IsGroupBy = Convert.ToBoolean(Request.QueryString[Constants.WSQueryStrings.GroupByIndicator]);
                    }
                    break;
                case Constants.WSQueryStrings.Title:
                    if (!string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.Title]))
                    {
                        Title = Convert.ToString(Request.QueryString[Constants.WSQueryStrings.Title]);
                    }
                    break;
                case Constants.WSQueryStrings.FootNote:
                    if (!string.IsNullOrEmpty(Request.QueryString[Constants.WSQueryStrings.FootNote]))
                    {
                        Footnote = Convert.ToString(Request.QueryString[Constants.WSQueryStrings.FootNote]);
                    }
                    break;
                default:
                    break;
            }
        }

        if (!string.IsNullOrEmpty(ResponseFormat))
        {
            switch (ResponseFormat)
            {
                case Constants.WSQueryStrings.ResponseFormatTypes.SDMX:
                    QueryDocument = GetQueryXML(ResponseFormat, SDMXFormat, Indicator, Area, TimePeriod, Source, Language);

                    if (!string.IsNullOrEmpty(SDMXFormat))
                    {
                        switch (SDMXFormat)
                        {
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS:
                                ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecificTS, DIConnection, DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific:
                                ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecific, DIConnection, DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.GenericTS:
                                ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.GenericTS, DIConnection, DIQueries);
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.Generic:
                                ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.Generic, DIConnection, DIQueries);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecificTS, DIConnection, DIQueries);
                    }

                    Response.Write(ResponseDocument.OuterXml);
                    break;
                case Constants.WSQueryStrings.ResponseFormatTypes.JSON:
                    QueryDocument = GetQueryXML(ResponseFormat, null, Indicator, Area, TimePeriod, Source, Language);
                    Response.Write(DATAUtility.Get_Data(QueryDocument, DataTypes.JSON, DIConnection, DIQueries));
                    break;
                case Constants.WSQueryStrings.ResponseFormatTypes.XML:
                    QueryDocument = GetQueryXML(ResponseFormat, null, Indicator, Area, TimePeriod, Source, Language);
                    Response.Write(DATAUtility.Get_Data(QueryDocument, DataTypes.XML, DIConnection, DIQueries));
                    break;
                case Constants.WSQueryStrings.ResponseFormatTypes.TABLE:
                    string BaseUrl = HttpContext.Current.Request.Url.OriginalString.Split(new string[] { "?" }, StringSplitOptions.None)[0];
                    int index = BaseUrl.IndexOf("libraries");
                    
                    string XsltUrl = string.Empty;
                    if (IsGroupBy)
                    {
                        XsltUrl = BaseUrl.Substring(0, index) + "stock/xslt/HTTPRequest_XMLResponse - INDGrp.xslt";
                        //XsltUrl = "../../stock/xslt/HTTPRequest_XMLResponse - INDGrp.xslt";
                    }
                    else
                    {
                        XsltUrl = BaseUrl.Substring(0, index) + "stock/xslt/HTTPRequest_XMLResponse.xslt";
                        //XsltUrl = "../../stock/xslt/HTTPRequest_XMLResponse.xslt";
                    }
                    BaseUrl = BaseUrl.Substring(0,index-1);
                    //string PItext = "type='application/xml' href='" + XsltUrl + "'";

                    QueryDocument = GetQueryXML(ResponseFormat, null, Indicator, Area, TimePeriod, Source, Language);
                    

                    string XmlStr = DATAUtility.Get_Data(QueryDocument, DataTypes.XML, DIConnection, DIQueries);
                    
                  //  DataTable DT = new DataTable();
                   // DT.ReadXml(new StringReader(XmlStr));
                    XsltArgumentList ArguList = new XsltArgumentList();
                    ArguList.AddParam("Title_Text","",Title);
                    ArguList.AddParam("Footnote_Text", "", Footnote);
                    ArguList.AddParam("BaseUrl", "", BaseUrl);
                    XmlStr = this.Transform(XmlStr, XsltUrl, ArguList);
                    XmlStr = XmlStr.Substring(XmlStr.IndexOf("<html>"));
                    //XmlDocument FormattedXml = new XmlDocument();
                    //FormattedXml.LoadXml(XmlStr);
                    //XmlProcessingInstruction XSLTTag = FormattedXml.CreateProcessingInstruction("xml-stylesheet", PItext);
                    //FormattedXml.InsertBefore(XSLTTag, FormattedXml.SelectSingleNode("Root"));
                    Response.ClearHeaders();
                    //Response.AddHeader("content-type", "application/xml");
                    //Response.Write(FormattedXml.InnerXml);
                    Response.Write(XmlStr);
                    break;
                default:
                    break;
            }
        }
        else
        {
            QueryDocument = GetQueryXML(ResponseFormat, null, Indicator, Area, TimePeriod, Source, Language);
            ResponseDocument = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecificTS, DIConnection, DIQueries);
        }
    }

    #endregion "--Public--"


    private string Transform(string xml, string xsl, XsltArgumentList argsList)
    {
        XDocument selectedXml = XDocument.Parse(xml);
        XslCompiledTransform xmlTransform = new XslCompiledTransform();

        StringBuilder htmlOutput = new StringBuilder();
        XmlWriter writer = XmlWriter.Create(htmlOutput);

        xmlTransform.Load(xsl);
        xmlTransform.Transform(selectedXml.CreateReader(), argsList, writer);

        return htmlOutput.ToString();
    }
    #endregion "--Method--"
}