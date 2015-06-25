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
using System.Data;

public partial class libraries_ws_REST : System.Web.UI.Page
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

    private XmlDocument Get_Query(string PathInfo, string ResponseFormat, string SDMXFormat, string Language)
    {
        XmlDocument RetVal;
        Dictionary<string, string> DictQuery;
        string[] PathInfoComponents;
        DataTable DtIUS;
        DataRow[] IUSRows;
        string WhereClause;
        List<string> IUSGIds;

        RetVal = null;
        DictQuery = new Dictionary<string, string>();
        PathInfoComponents = null;
        DtIUS = null;
        IUSRows = new DataRow[] { };
        WhereClause = string.Empty;
        IUSGIds = new List<string>();

        try
        {
            PathInfoComponents = PathInfo.Split('/');

            DtIUS = this.DIConnection.ExecuteDataTable(this.DIQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            if (PathInfoComponents.Length > 1)
            {
                if (!string.IsNullOrEmpty(PathInfoComponents[1]) && PathInfoComponents[1] != "ALL")
                {
                    DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, PathInfoComponents[1]);
                }

                if (PathInfoComponents.Length > 2)
                {
                    if (!string.IsNullOrEmpty(PathInfoComponents[2]) && PathInfoComponents[2] != "ALL")
                    {
                        DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, PathInfoComponents[2]);
                    }

                    if (PathInfoComponents.Length > 3)
                    {
                        if (!string.IsNullOrEmpty(PathInfoComponents[3]) && PathInfoComponents[3] != "ALL")
                        {
                            DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id, PathInfoComponents[3]);
                        }

                        if (PathInfoComponents.Length > 4)
                        {
                            if (!string.IsNullOrEmpty(PathInfoComponents[4]))
                            {
                                WhereClause = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId + "='" + PathInfoComponents[4] + "'";
                            }

                            if (PathInfoComponents.Length > 5)
                            {
                                if (!string.IsNullOrEmpty(PathInfoComponents[5]))
                                {
                                    WhereClause += " AND " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId + "='" + PathInfoComponents[5] + "'";
                                }

                                if (PathInfoComponents.Length > 6)
                                {
                                    if (!string.IsNullOrEmpty(PathInfoComponents[6]))
                                    {
                                        WhereClause += " AND " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + "='" + PathInfoComponents[6].Replace("~~", string.Empty) + "'";
                                    }
                                }
                            }

                            IUSRows = DtIUS.Select(WhereClause);

                            foreach (DataRow IUSRow in IUSRows)
                            {
                                IUSGIds.Add(IUSRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString() + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + IUSRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString() + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate +
                                            IUSRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString());
                            }

                            DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, String.Join(",", IUSGIds.ToArray()));
                        }
                    }
                }
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
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion "--Private--"

    #region "--Public--"

    protected void Page_Load(object sender, EventArgs e)
    {
        int DBNId;
        string PathInfo, Language, ResponseFormat, SDMXFormat, ResponseString;
        XmlDocument QueryDocument;

        DBNId = 0;
        PathInfo = string.Empty;
        Language = string.Empty;
        ResponseFormat = string.Empty;
        SDMXFormat = string.Empty;
        ResponseString = string.Empty;
        QueryDocument = null;

        try
        {
            //if (!string.IsNullOrEmpty(this.Request.PathInfo))
            if (this.Request.QueryString["info"] != null)
            {
                //PathInfo = this.Request.PathInfo.Substring(1);
                PathInfo = this.Request.QueryString["info"];
                DBNId = Convert.ToInt32(PathInfo.Split('/')[0]);
                Language = PathInfo.Split('/')[1];
                ResponseFormat = PathInfo.Split('/')[2];

                this.DIConnection = Global.GetDbConnection(DBNId);
                this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));

                switch (ResponseFormat)
                {
                    case Constants.WSQueryStrings.ResponseFormatTypes.SDMX:
                        PathInfo = PathInfo.Substring(PathInfo.IndexOf("/SDMX") + "/SDMX".Length);

                        switch (Request.ContentType)
                        {
                            case Constants.WSQueryStrings.SDMXContentTypes.Generic:
                                SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.Generic;
                                break;
                            case Constants.WSQueryStrings.SDMXContentTypes.GenericTS:
                                SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.GenericTS;
                                break;
                            case Constants.WSQueryStrings.SDMXContentTypes.StructureSpecific:
                                SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific;
                                break;
                            case Constants.WSQueryStrings.SDMXContentTypes.StructureSpecificTS:
                                SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS;
                                break;
                            default:
                                SDMXFormat = Constants.WSQueryStrings.SDMXFormatTypes.Generic;
                                break;
                        }

                        QueryDocument = this.Get_Query(PathInfo, ResponseFormat, SDMXFormat, Language);

                        switch (SDMXFormat)
                        {
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS:
                                ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecificTS, DIConnection, DIQueries).OuterXml;
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecific:
                                ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.StructureSpecific, DIConnection, DIQueries).OuterXml;
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.GenericTS:
                                ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.GenericTS, DIConnection, DIQueries).OuterXml;
                                break;
                            case Constants.WSQueryStrings.SDMXFormatTypes.Generic:
                                ResponseString = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, QueryDocument, DataFormats.Generic, DIConnection, DIQueries).OuterXml;
                                break;
                            default:
                                break;
                        }

                        Response.Write(ResponseString);
                        break;
                    case Constants.WSQueryStrings.ResponseFormatTypes.JSON:
                        PathInfo = PathInfo.Substring(PathInfo.IndexOf("/JSON") + "/JSON".Length);
                        QueryDocument = this.Get_Query(PathInfo, ResponseFormat, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS, Language);

                        ResponseString = DATAUtility.Get_Data(QueryDocument, DataTypes.JSON, DIConnection, DIQueries);
                        Response.Write(ResponseString);
                        break;
                    case Constants.WSQueryStrings.ResponseFormatTypes.XML:
                        PathInfo = PathInfo.Substring(PathInfo.IndexOf("/XML") + "/XML".Length);
                        QueryDocument = this.Get_Query(PathInfo, ResponseFormat, Constants.WSQueryStrings.SDMXFormatTypes.StructureSpecificTS, Language);

                        ResponseString = DATAUtility.Get_Data(QueryDocument, DataTypes.XML, DIConnection, DIQueries);
                        Response.Write(ResponseString);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }

    #endregion "--Public--"

    #endregion "--Method--"
}