using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using iTextSharp.text;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net;
using System.Net.Mail;
using SDMXObjectModel.Message;
using System.Text;
using DevInfo.Lib.DI_LibDATA;
using System.Web.Services.Protocols;
using System.Xml.Xsl; 

public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    private void Add_PlaceHolders_To_Query_Dictionary(Dictionary<string, List<string>> DictQuery)
    {
        try
        {
            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, null);
            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, null);
            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, null);
            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id, null);
            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.FOOTNOTES.Id, null);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private void Add_Indicator_Unit_Subgroups_To_Query_Dictionary(Dictionary<string, List<string>> DictQuery, string IUSNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtIUS;
        string IndicatorGId, UnitGId, SubgroupValGId;

        try
        {
            DtIUS = DIConnection.ExecuteDataTable(DIQueries.IUS.GetIUS(FilterFieldType.NId, IUSNIds, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            foreach (DataRow DrIUS in DtIUS.Rows)
            {
                IndicatorGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString();
                UnitGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString();
                SubgroupValGId = DrIUS[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + SubgroupValGId);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private string Get_IUS_For_REST_From_NIds(string IUSNId, DataTable DtIUS)
    {
        string RetVal;
        DataRow[] RowsIUS;
        string IndicatorGId, UnitGId, SubgroupValGId;

        RetVal = string.Empty;
        RowsIUS = null;
        IndicatorGId = string.Empty;
        UnitGId = string.Empty;
        SubgroupValGId = string.Empty;

        try
        {
            RowsIUS = DtIUS.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo + IUSNId);
            if (RowsIUS.Length > 0)
            {
                IndicatorGId = RowsIUS[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString();
                UnitGId = RowsIUS[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString();
                SubgroupValGId = RowsIUS[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                RetVal = IndicatorGId + "/" + UnitGId + "/" + SubgroupValGId;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private void Add_Areas_To_Query_Dictionary(Dictionary<string, List<string>> DictQuery, string AreaNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtAreas;
        string AreaID;

        try
        {
            DtAreas = DIConnection.ExecuteDataTable(DIQueries.Area.GetArea(FilterFieldType.NId, AreaNIds));

            foreach (DataRow DrArea in DtAreas.Rows)
            {
                AreaID = DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString();
                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, AreaID);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private void Add_TPs_To_Query_Dictionary(Dictionary<string, List<string>> DictQuery, string TPNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtTimePeriods;
        string TimePeriodText;

        try
        {
            if (!string.IsNullOrEmpty(TPNIds))
            {
                DtTimePeriods = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, TPNIds));

                foreach (DataRow DrTimePeriod in DtTimePeriods.Rows)
                {
                    TimePeriodText = DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString();
                    this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, TimePeriodText);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private void Add_Sources_To_Query_Dictionary(Dictionary<string, List<string>> DictQuery, string SourceNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtSources;
        string Source;

        try
        {
            DtSources = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, SourceNIds, ICType.Source, FieldSelection.Light));

            foreach (DataRow DrSource in DtSources.Rows)
            {
                Source = DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString();
                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id, Source);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private void Add_Minus_One_To_Selections(ref string IUSNIds, ref string AreaNIds, ref string TPNIds, ref string SourceNIds)
    {
        if (string.IsNullOrEmpty(IUSNIds))
        {
            IUSNIds = "-1";
        }
        else
        {
            IUSNIds += ",-1";
        }

        if (string.IsNullOrEmpty(AreaNIds))
        {
            AreaNIds = "-1";
        }
        else
        {
            AreaNIds += ",-1";
        }

        if (string.IsNullOrEmpty(TPNIds))
        {
            TPNIds = "-1";
        }
        else
        {
            TPNIds += ",-1";
        }

        if (string.IsNullOrEmpty(SourceNIds))
        {
            SourceNIds = "-1";
        }
        else
        {
            SourceNIds += ",-1";
        }
    }

    private void Add_Item_To_Dictionary(Dictionary<string, List<string>> DictQuery, string key, string value)
    {
        if (DictQuery.ContainsKey(key))
        {
            if (!DictQuery[key].Contains(value))
            {
                DictQuery[key].Add(value);
            }
        }
        else
        {
            DictQuery.Add(key, null);
            DictQuery[key] = new List<string>();
            DictQuery[key].Add(value);
        }
    }

    private string Get_Formatted_XML(XmlDocument Query)
    {
        string RetVal;
        StringBuilder XmlContent;
        StringWriter StringWriter;
        XmlTextWriter XmlTextWriter;

        XmlContent = new StringBuilder();
        StringWriter = new StringWriter(XmlContent);
        XmlTextWriter = new XmlTextWriter(StringWriter);
        
        XmlTextWriter.Formatting = Formatting.Indented;
        Query.WriteTo(XmlTextWriter);
        RetVal = this.Server.HtmlEncode(XmlContent.ToString());

        return RetVal;
    }

    private string Join(List<string> List, string separator)
    {
        string RetVal;

        RetVal = string.Empty;

        if (List != null && List.Count > 0)
        {
            foreach (string Item in List)
            {
                if (!string.IsNullOrEmpty(Item))
                {
                    RetVal += Item + separator;
                }
            }
        }

        if (RetVal.Length > 0)
        {
            RetVal = RetVal.Substring(0, RetVal.Length - 1);
        }

        return RetVal;
    }

    #endregion "--Private--"

    #region "--Public--"

    public string GetDataQueryRequestSOAP(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId, ResponseType, SDMXFormat;
        string Language, IUSNIds, AreaNIds, TPNIds, SourceNIds;
        bool IsMRD;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, List<string>> DictQuery;
        Dictionary<string, string> DictQueryParam;
        XmlDocument Query;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        Language = string.Empty;
        IUSNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        IsMRD = false;
        DIConnection = null;
        DIQueries = null;
        DictQuery = new Dictionary<string, List<string>>();
        DictQueryParam = new Dictionary<string, string>();
        Query = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            Language = Params[1].ToString().Trim();
            IUSNIds = Params[2].ToString().Trim();

            if (!string.IsNullOrEmpty(Params[3]) && Params[3].Length > 0)
            {
                AreaNIds = Params[3].ToString().TrimEnd(',');
            }

            IsMRD = Convert.ToBoolean(Params[4]);
            TPNIds = Params[5];

            if (!string.IsNullOrEmpty(Params[6]) && Params[6].Length > 0)
            {
                SourceNIds = Params[6].ToString().TrimEnd(',');
            }

            ResponseType = Convert.ToInt32(Params[7].ToString().Trim());
            SDMXFormat = Convert.ToInt32(Params[8].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            this.Add_Minus_One_To_Selections(ref IUSNIds, ref AreaNIds, ref TPNIds, ref SourceNIds);
            this.Add_PlaceHolders_To_Query_Dictionary(DictQuery);
            this.Add_Indicator_Unit_Subgroups_To_Query_Dictionary(DictQuery, IUSNIds, DIConnection, DIQueries);
            this.Add_Areas_To_Query_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);

            if (IsMRD == false)
            {
                this.Add_TPs_To_Query_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
            }
            else
            {
                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, DevInfo.Lib.DI_LibSDMX.Constants.MRD);
            }

            this.Add_Sources_To_Query_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

            this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.FOOTNOTES.Id, Language);

            foreach (string key in DictQuery.Keys)
            {
                if (DictQuery[key] != null && !string.IsNullOrEmpty(this.Join(DictQuery[key], ",")))
                {
                    DictQueryParam.Add(key, this.Join(DictQuery[key], ","));
                }
            }

            switch (ResponseType)
            {
                case 0://SDMX
                    Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQueryParam, (QueryFormats)SDMXFormat, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, DIConnection, DIQueries);
                    break;
                case 1://JSON
                    Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQueryParam, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, DIConnection, DIQueries);
                    break;
                case 2://XML
                    Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQueryParam, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, DIConnection, DIQueries);
                    break;
            }

            RetVal = this.Get_Formatted_XML(Query);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string GetDataQueryRequestHTTP(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId, ResponseType, SDMXFormat;
        string Language, IUSNIds, AreaNIds, TPNIds, SourceNIds, Title, Footnote, GroupByIndi;
        bool IsMRD;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, List<string>> DictQuery;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        Language = string.Empty;
        IUSNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        IsMRD = false;
        DIConnection = null;
        DIQueries = null;
        DictQuery = new Dictionary<string, List<string>>();
        Title = string.Empty;
        Footnote = string.Empty;
        GroupByIndi = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            Language = Params[1].ToString().Trim();
            IUSNIds = Params[2].ToString().Trim();

            if (!string.IsNullOrEmpty(Params[3]) && Params[3].Length > 0)
            {
                AreaNIds = Params[3].ToString().TrimEnd(',');
            }

            IsMRD = Convert.ToBoolean(Params[4]);
            TPNIds = Params[5];

            if (!string.IsNullOrEmpty(Params[6]) && Params[6].Length > 0)
            {
                SourceNIds = Params[6].ToString().TrimEnd(',');
            }

            ResponseType = Convert.ToInt32(Params[7].ToString().Trim());
            SDMXFormat = Convert.ToInt32(Params[8].ToString().Trim());
            Title = Convert.ToString(Params[9].ToString().Trim());
            Footnote = Convert.ToString(Params[10].ToString().Trim());
            GroupByIndi = Convert.ToString(Params[11].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            this.Add_Minus_One_To_Selections(ref IUSNIds, ref AreaNIds, ref TPNIds, ref SourceNIds);
            this.Add_PlaceHolders_To_Query_Dictionary(DictQuery);
            this.Add_Indicator_Unit_Subgroups_To_Query_Dictionary(DictQuery, IUSNIds, DIConnection, DIQueries);
            this.Add_Areas_To_Query_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);

            if (IsMRD == false)
            {
                this.Add_TPs_To_Query_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
            }
            else
            {
                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, DevInfo.Lib.DI_LibSDMX.Constants.MRD);
            }

            this.Add_Sources_To_Query_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.Language, Language);

            RetVal = this.Page.Request.Url.AbsoluteUri.Replace("aspx/CallBack.aspx", "ws/HTTP.aspx") + "?" + Constants.WSQueryStrings.p + "=" + DBNId.ToString();

            switch (ResponseType)
            {
                case 0://SDMX
                    this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.ResponseFormat, Constants.WSQueryStrings.ResponseFormatTypes.SDMX);

                    switch ((DataFormats)SDMXFormat)
                    {
                        case DataFormats.Generic:
                            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.SDMXFormat, Constants.WSQueryStrings.SDMXFormatTypes.Generic);
                            break;
                        case DataFormats.GenericTS:
                            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.SDMXFormat, Constants.WSQueryStrings.SDMXFormatTypes.GenericTS);
                            break;
                        case DataFormats.StructureSpecific:
                            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.SDMXFormat, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific);
                            break;
                        case DataFormats.StructureSpecificTS:
                            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.SDMXFormat, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS);
                            break;
                    }
                    break;
                case 1://JSON
                    this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.ResponseFormat, Constants.WSQueryStrings.ResponseFormatTypes.JSON);
                    break;
                case 2://XML
                    this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.ResponseFormat, Constants.WSQueryStrings.ResponseFormatTypes.XML);
                    break;
                case 3://TABLE
                    this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.ResponseFormat, Constants.WSQueryStrings.ResponseFormatTypes.TABLE);
                    break;
                default:
                    break;
            }

            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.Title, Title);
            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.FootNote, Footnote);
            this.Add_Item_To_Dictionary(DictQuery, Constants.WSQueryStrings.GroupByIndicator, GroupByIndi);
            foreach (string key in DictQuery.Keys)
            {
                if (DictQuery[key] != null && !string.IsNullOrEmpty(this.Join(DictQuery[key], ",")))
                {
                    RetVal += "&" + key + "=" + this.Join(DictQuery[key], ",");
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string GetDataQueryRequestREST(string requestParam)
    {
        string RetVal;
        string[] Params;
        string RESTUrl;
        int DBNId, ResponseType, SDMXFormat;
        string Language, IUSNIds, AreaNIds, TPNIds, SourceNIds;
        bool IsMRD;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtIUS;
        Dictionary<string, List<string>> DictQuery;

        RetVal = string.Empty;
        Params = null;
        RESTUrl = string.Empty;
        DBNId = -1;
        Language = string.Empty;
        IUSNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        IsMRD = false;
        DIConnection = null;
        DIQueries = null;
        DtIUS = null;
        DictQuery = new Dictionary<string, List<string>>();

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            Language = Params[1].ToString().Trim();
            IUSNIds = Params[2].ToString().Trim();

            if (!string.IsNullOrEmpty(Params[3]) && Params[3].Length > 0)
            {
                AreaNIds = Params[3].ToString().Trim(',');
            }

            IsMRD = Convert.ToBoolean(Params[4]);
            TPNIds = Params[5];

            if (!string.IsNullOrEmpty(Params[6]) && Params[6].Length > 0)
            {
                SourceNIds = Params[6].ToString().TrimEnd(',');
            }

            ResponseType = Convert.ToInt32(Params[7].ToString().Trim());
            SDMXFormat = Convert.ToInt32(Params[8].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            DtIUS = DIConnection.ExecuteDataTable(DIQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            this.Add_Minus_One_To_Selections(ref IUSNIds, ref AreaNIds, ref TPNIds, ref SourceNIds);
            this.Add_PlaceHolders_To_Query_Dictionary(DictQuery);
            this.Add_Areas_To_Query_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);

            if (IsMRD == false)
            {
                this.Add_TPs_To_Query_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
            }
            else
            {
                this.Add_Item_To_Dictionary(DictQuery, DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, DevInfo.Lib.DI_LibSDMX.Constants.MRD);
            }

            this.Add_Sources_To_Query_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

            RESTUrl = this.Page.Request.Url.AbsoluteUri.Replace("aspx/CallBack.aspx", "ws/REST/") + DBNId.ToString() + "/" + Language + "/";

            switch (ResponseType)
            {
                case 0://SDMX
                    RESTUrl += "SDMX/";
                    break;
                case 1://JSON
                    RESTUrl += "JSON/";
                    break;
                case 2://XML
                    RESTUrl += "XML/";
                    break;
                default:
                    break;
            }

            foreach (string key in DictQuery.Keys)
            {
                if ((key == DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id || key == DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id || key == DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id) && DictQuery[key] != null)
                {
                    if (!string.IsNullOrEmpty(this.Join(DictQuery[key], ",")))
                    {
                        RESTUrl += this.Join(DictQuery[key], ",") + "/";
                    }
                    else
                    {
                        RESTUrl += "ALL/";
                    }
                }
            }

            foreach (string IUSNId in IUSNIds.Split(','))
            {
                if (!string.IsNullOrEmpty(IUSNId) && IUSNId != "-1")
                {
                    RetVal += RESTUrl + this.Get_IUS_For_REST_From_NIds(IUSNId, DtIUS) + Constants.Delimiters.ValuesDelimiter;
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string GetDataQueryResponseSDMXML(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId, SDMXFormat, RequestType;
        string Request;
        XmlDocument Query, SDMXML;
        XmlElement Element;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Registry.RegistryService Service;
        string SOAPMethodName;
        WebRequest WebRequest;
        StreamReader ResponseReader;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        Request = string.Empty;
        Query = new XmlDocument();
        SDMXML = new XmlDocument();
        Element = null;
        DIConnection = null;
        DIQueries = null;
        Service = new Registry.RegistryService();
        SOAPMethodName = string.Empty;
        WebRequest = null;
        ResponseReader = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            RequestType = Convert.ToInt32(Params[1].ToString().Trim());
            Request = this.Server.HtmlDecode(Params[2].ToString().Trim());
            SDMXFormat = Convert.ToInt32(Params[3].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            switch (RequestType)
            {
                case 0://SOAP
                    Query.LoadXml(Request);
                    Element = Query.DocumentElement;
                     //HttpContext.Current.RewritePath("~/libraries/ws/RegistryService.asmx");
                     //Service.Url = HttpContext.Current.Request.Url.ToString();
                    Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
                    Service.Url += "?p=" + DBNId.ToString();

                    switch ((DataFormats)SDMXFormat)
                    {
                        case DataFormats.Generic:
                            SOAPMethodName = "GetGenericData";

                            try
                            {
                                Service.GetGenericData(ref Element);
                                SDMXML.LoadXml(Element.OuterXml);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                            }
                            catch (SoapException SOAPex)
                            {
                                SDMXML.LoadXml(SOAPex.Detail.InnerText);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                                Global.CreateExceptionString(SOAPex, null);
                            }
                            break;
                        case DataFormats.GenericTS:
                            SOAPMethodName = "GetGenericTimeSeriesData";

                            try
                            {
                                Service.GetGenericTimeSeriesData(ref Element);
                                SDMXML.LoadXml(Element.OuterXml);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                            }
                            catch (SoapException SOAPex)
                            {
                                SDMXML.LoadXml(SOAPex.Detail.InnerText);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                                Global.CreateExceptionString(SOAPex, null);
                            }
                            break;
                        case DataFormats.StructureSpecific:
                            SOAPMethodName = "GetStructureSpecificData";
                            
                            try
                            {
                                Service.GetStructureSpecificData(ref Element);
                                SDMXML.LoadXml(Element.OuterXml);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                            }
                            catch (SoapException SOAPex)
                            {
                                SDMXML.LoadXml(SOAPex.Detail.InnerText);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                                Global.CreateExceptionString(SOAPex, null);
                            }
                            break;
                        case DataFormats.StructureSpecificTS:
                            SOAPMethodName = "GetStructureSpecificTimeSeriesData";
                            
                            try
                            {
                                Service.GetStructureSpecificTimeSeriesData(ref Element);
                                SDMXML.LoadXml(Element.OuterXml);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                            }
                            catch (SoapException SOAPex)
                            {
                                SDMXML.LoadXml(SOAPex.Detail.InnerText);
                                RetVal = this.Get_Formatted_XML(SDMXML);
                                Global.CreateExceptionString(SOAPex, null);
                            }
                            break;
                    }
                    
                    RetVal += Constants.Delimiters.ParamDelimiter + Service.Url;
                    RetVal += Constants.Delimiters.ParamDelimiter + SOAPMethodName;
                    break;
                case 1://REST
                    WebRequest = WebRequest.Create(Request);

                    switch ((DataFormats)SDMXFormat)
                    {
                        case DataFormats.Generic:
                            WebRequest.ContentType = Constants.WSQueryStrings.SDMXContentTypes.Generic;
                            break;
                        case DataFormats.GenericTS:
                            WebRequest.ContentType = Constants.WSQueryStrings.SDMXContentTypes.GenericTS;
                            break;
                        case DataFormats.StructureSpecific:
                            WebRequest.ContentType = Constants.WSQueryStrings.SDMXContentTypes.StructureSpecific;
                            break;
                        case DataFormats.StructureSpecificTS:
                            WebRequest.ContentType = Constants.WSQueryStrings.SDMXContentTypes.StructureSpecificTS;
                            break;
                    }

                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());

                    SDMXML.LoadXml(ResponseReader.ReadToEnd());
                    RetVal = this.Get_Formatted_XML(SDMXML);
                    break;
                case 2://HTTP
                    WebRequest = WebRequest.Create(Request);
                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());

                    SDMXML.LoadXml(ResponseReader.ReadToEnd());
                    RetVal = this.Get_Formatted_XML(SDMXML);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDataQueryResponseJSON(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId, RequestType;
        string Request;
        XmlDocument Query;
        DIConnection DIConnection;
        DIQueries DIQueries;
        JSONXML.DataService Service;
        WebRequest WebRequest;
        StreamReader ResponseReader;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        Request = string.Empty;
        Query = new XmlDocument();
        DIConnection = null;
        DIQueries = null;
        Service = new JSONXML.DataService();
        WebRequest = null;
        ResponseReader = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            RequestType = Convert.ToInt32(Params[1].ToString().Trim());
            Request = this.Server.HtmlDecode(Params[2].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            switch (RequestType)
            {
                case 0://SOAP
                    Query.LoadXml(Request);
                    // HttpContext.Current.RewritePath("~/libraries/ws/DataServices.asmx");
                    //Service.Url = HttpContext.Current.Request.Url.ToString();
                    Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.DataServicePath;
                    Service.Url += "?p=" + DBNId.ToString();

                    RetVal = Service.GetJSONData(Query.DocumentElement);
                    RetVal += Constants.Delimiters.ParamDelimiter + Service.Url;
                    RetVal += Constants.Delimiters.ParamDelimiter + "GetJSONData";
                    break;
                case 1://REST
                    WebRequest = WebRequest.Create(Request);
                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());
                    RetVal = ResponseReader.ReadToEnd();
                    break;
                case 2://HTTP
                    WebRequest = WebRequest.Create(Request);
                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());
                    RetVal = ResponseReader.ReadToEnd();
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDataQueryResponseXML(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId, RequestType;
        string Request;
        XmlDocument Query, XML;
        DIConnection DIConnection;
        DIQueries DIQueries;
        JSONXML.DataService Service;
        WebRequest WebRequest;
        StreamReader ResponseReader;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        Request = string.Empty;
        Query = new XmlDocument();
        XML = new XmlDocument();
        DIConnection = null;
        DIQueries = null;
        Service = new JSONXML.DataService();
        WebRequest = null;
        ResponseReader = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            RequestType = Convert.ToInt32(Params[1].ToString().Trim());
            Request = this.Server.HtmlDecode(Params[2].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            switch (RequestType)
            {
                case 0://SOAP
                    Query.LoadXml(Request);
                     //HttpContext.Current.RewritePath("~/libraries/ws/DataServices.asmx");
                     //Service.Url = HttpContext.Current.Request.Url.ToString();
                    Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.DataServicePath;
                    Service.Url += "?p=" + DBNId.ToString();

                    XML.LoadXml(Service.GetXmlData(Query.DocumentElement));
                    RetVal = this.Get_Formatted_XML(XML);
                    RetVal += Constants.Delimiters.ParamDelimiter + Service.Url;
                    RetVal += Constants.Delimiters.ParamDelimiter + "GetXmlData";
                    break;
                case 1://REST
                    WebRequest = WebRequest.Create(Request);
                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());

                    XML.LoadXml(ResponseReader.ReadToEnd());
                    RetVal = this.Get_Formatted_XML(XML);
                    break;
                case 2://HTTP
                    WebRequest = WebRequest.Create(Request);
                    ResponseReader = new StreamReader(WebRequest.GetResponse().GetResponseStream());
                    XML.LoadXml(ResponseReader.ReadToEnd());
                    RetVal = this.Get_Formatted_XML(XML);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string GetTimePeriods(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtTimePeriod;

        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtTimePeriod = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            RetVal = "<table id=\"tblTimeRange\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkTimeRange_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllTPs();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanTimeRange_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            DtTimePeriod = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
            foreach (DataRow DrTimePeriod in DtTimePeriod.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                RetVal += "<input id=\"chkTimeRange_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\" type=\"checkbox\" value=\"" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\"/>";
                RetVal += "</td>";

                RetVal += "<td style=\"width:98%\">";
                RetVal += "<span id=\"spanTimeRange_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\">" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString() + "</span>";
                RetVal += "</td>";

                RetVal += "</tr>";
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string GetDataQuerySources(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId;
        string Language;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtSource;

        RetVal = string.Empty;
        Language = string.Empty;
        Params = null;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtSource = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            Language = Params[1].ToString().Trim();

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            if (DIConnection.IsValidDILanguage(DIQueries.DataPrefix, Language))
            {
                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), Language);
            }

            RetVal = "<table id=\"tblSource\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkSource_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllSources();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanSource_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.None,string.Empty, ICType.Source, FieldSelection.Light));
            foreach (DataRow DrSource in DtSource.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                RetVal += "<input id=\"chkSource_" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\" type=\"checkbox\" value=\"" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\"/>";
                RetVal += "</td>";

                RetVal += "<td style=\"width:98%\">";
                RetVal += "<span id=\"spanSource_" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\">" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString() + "</span>";
                RetVal += "</td>";

                RetVal += "</tr>";
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
