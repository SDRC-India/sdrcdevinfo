using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DevInfo.Lib.DI_LibSDMX;
using System.Xml;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDATA;
using System.ServiceModel.Web;

public class REST : IREST
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

    private XmlDocument Get_Query(string QueryString, string ResponseFormat, string SDMXFormat, string Language)
    {
        XmlDocument RetVal;
        Dictionary<string, string> DictQuery;
        DataTable DtSubgroupTypes;
        string[] QueryStringComponents;

        RetVal = null;
        DictQuery = new Dictionary<string, string>();
        DtSubgroupTypes = null;
        QueryStringComponents = null;

        try
        {
            QueryStringComponents = QueryString.Split(',');

            DtSubgroupTypes = DIConnection.ExecuteDataTable(DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
            DtSubgroupTypes.DefaultView.Sort = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID + " ASC";
            DtSubgroupTypes = DtSubgroupTypes.DefaultView.ToTable();

            if (!string.IsNullOrEmpty(QueryStringComponents[0].Trim()))//Indicator
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, QueryStringComponents[0].Replace(" ", ","));
            }

            if (!string.IsNullOrEmpty(QueryStringComponents[1].Trim()))//Unit
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.UNIT.Id, QueryStringComponents[1].Replace(" ", ","));
            }

            for (int i = 0; i < DtSubgroupTypes.Rows.Count; i++)//Subgroups
            {
                if (!string.IsNullOrEmpty(QueryStringComponents[i + 2].Trim()))
                {
                    DictQuery.Add(DtSubgroupTypes.Rows[i][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString(), QueryStringComponents[i + 2].Replace(" ", ","));
                }
            }

            if (!string.IsNullOrEmpty(QueryStringComponents[DtSubgroupTypes.Rows.Count + 2].Trim()))//Area
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, QueryStringComponents[DtSubgroupTypes.Rows.Count + 2].Replace(" ", ","));
            }

            if (!string.IsNullOrEmpty(QueryStringComponents[DtSubgroupTypes.Rows.Count + 3].Trim()))//TimePeriod
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, QueryStringComponents[DtSubgroupTypes.Rows.Count + 3].Replace(" ", ","));
            }

            if (!string.IsNullOrEmpty(Language))
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.FOOTNOTES.Id, Language);
            }

            switch (ResponseFormat)
            {
                case Constants.WSQueryStrings.ResponseFormatTypes.SDMX:
                    switch (SDMXFormat)
                    {
                        case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS:
                            RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                            break;
                        case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific:
                            RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecific, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                            break;
                        case Constants.WSQueryStrings.SDMXFormatTypes.GenericTS:
                            RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.GenericTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                            break;
                        case Constants.WSQueryStrings.SDMXFormatTypes.Generic:
                            RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.Generic, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                            break;
                        default:
                            break;
                    }
                    break;
                case Constants.WSQueryStrings.ResponseFormatTypes.JSON:
                    RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                    break;
                case Constants.WSQueryStrings.ResponseFormatTypes.XML:
                    RetVal = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, DevInfo.Lib.DI_LibSDMX.Constants.AgencyId, this.DIConnection, this.DIQueries);
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion "--Private--"

    #region "--Public--"

    public string GetDataQueryResponseSDMXML(string dbNId, string language, string flowRef, string key)
    {
        string AgencyId, ResponseString;
        XmlDocument QueryDocument;

        AgencyId = string.Empty;
        ResponseString = string.Empty;
        QueryDocument = null;

        try
        {
            this.DIConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
            this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + dbNId;

            switch (WebOperationContext.Current.IncomingRequest.ContentType)
            {
                case Constants.WSQueryStrings.SDMXContentTypes.Generic:
                    QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.SDMX, Constants.WSQueryStrings.SDMXFormatTypes.Generic, language);
                    ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.Generic, DIConnection, DIQueries).OuterXml;
                    break;
                case Constants.WSQueryStrings.SDMXContentTypes.GenericTS:
                    QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.SDMX, Constants.WSQueryStrings.SDMXFormatTypes.GenericTS, language);
                    ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.GenericTS, DIConnection, DIQueries).OuterXml;
                    break;
                case Constants.WSQueryStrings.SDMXContentTypes.StructureSpecific:
                    QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.SDMX, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific, language);
                    ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecific, DIConnection, DIQueries).OuterXml;
                    break;
                case Constants.WSQueryStrings.SDMXContentTypes.StructureSpecificTS:
                    QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.SDMX, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS, language);
                    ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecificTS, DIConnection, DIQueries).OuterXml;
                    break;
                default:
                    QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.SDMX, Constants.WSQueryStrings.SDMXFormatTypes.Generic, language);
                    ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.Generic, DIConnection, DIQueries).OuterXml;
                    break;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return ResponseString;
    }

    public string GetDataQueryResponseXML(string dbNId, string language, string key)
    {
        string ResponseString;
        XmlDocument QueryDocument;

        ResponseString = string.Empty;
        QueryDocument = null;

        try
        {
            this.DIConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
            this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));

            QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.XML, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS, language);
            ResponseString = DATAUtility.Get_Data(QueryDocument, DataTypes.XML, DIConnection, DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return ResponseString;
    }

    public string GetDataQueryResponseJSON(string dbNId, string language, string key)
    {
        string ResponseString;
        XmlDocument QueryDocument;

        ResponseString = string.Empty;
        QueryDocument = null;

        try
        {
            this.DIConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
            this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));

            QueryDocument = this.Get_Query(key, Constants.WSQueryStrings.ResponseFormatTypes.JSON, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS, language);
            ResponseString = DATAUtility.Get_Data(QueryDocument, DataTypes.JSON, DIConnection, DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return ResponseString;
    }

    #endregion "--Public--"

    #endregion "--Method--"
}
