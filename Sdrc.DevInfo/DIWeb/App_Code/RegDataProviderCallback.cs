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
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Structure;
using System.Text;
using System.Collections;

using Newtonsoft;
internal enum DataProviderActions
{
    None = 0,
    GenerateSDMXML = 1,
    RegisterSDMXML = 2,
    GenerateMetadata = 3,
    RegisterMetadata = 4
}



public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #region "-- Indicator Popup HTML --"

    private string GetIndicatorDivInnerHTML_For_GenerateSDMXML(string DBOrDSDDBId, bool DBOrDSDFlag)
    {
        string RetVal;
        DataTable DtIndicator;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;
        DataView dv;

        RetVal = string.Empty;
        DtIndicator = null;
        DictIndicator = null;
        DictIndicatorMapping = null;
        dv = null;

        try
        {
            this.GetIndicatorTableAndDictionaries(DBOrDSDDBId, DBOrDSDFlag, null, out DtIndicator, out DictIndicator, out DictIndicatorMapping);
            dv = DtIndicator.DefaultView;
            dv.Sort = "Indicator_Name asc";
            DtIndicator = dv.ToTable();

            if (DictIndicator != null && DictIndicator.Keys.Count > 0)
            {
                DictIndicator = this.Sort_Dictionary(DictIndicator);
            }

            if (DictIndicatorMapping != null && DictIndicatorMapping.Keys.Count > 0)
            {
                //  DictIndicatorMapping = this.Sort_Dictionary(DictIndicatorMapping);
            }
            if (DBOrDSDFlag == true)
            {
                RetVal = this.GetIndicatorDivInnerHTML(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag);
            }
            else
            {
                RetVal = this.GetPublishDataGrid(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag, DBOrDSDDBId);
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetIndicatorDivInnerHTML_For_RegisterSDMXML(string DBOrDSDDBId, bool DBOrDSDFlag)
    {
        string RetVal;
        string SDMXMLFolderPath, IndicatorGId;
        List<string> ListGeneratedIndicatorsGIds;
        DataTable DtIndicator;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;

        RetVal = string.Empty;
        SDMXMLFolderPath = string.Empty;
        IndicatorGId = string.Empty;
        ListGeneratedIndicatorsGIds = new List<string>();
        DtIndicator = null;
        DictIndicator = null;
        DictIndicatorMapping = null;

        try
        {
            SDMXMLFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);

            foreach (string SDMXFolderPerLanguage in Directory.GetDirectories(SDMXMLFolderPath))
            {
                foreach (string FileNameWPath in Directory.GetFiles(SDMXFolderPerLanguage))
                {
                    IndicatorGId = this.Get_IndicatorGId_From_SDMXML(FileNameWPath, DBOrDSDFlag, null);

                    if (!ListGeneratedIndicatorsGIds.Contains(IndicatorGId))
                    {
                        ListGeneratedIndicatorsGIds.Add(IndicatorGId);
                    }
                }
            }

            this.GetIndicatorTableAndDictionaries(DBOrDSDDBId, DBOrDSDFlag, ListGeneratedIndicatorsGIds, out DtIndicator, out DictIndicator, out DictIndicatorMapping);
            RetVal = this.GetIndicatorDivInnerHTML(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag);
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

    private string GetIndicatorDivInnerHTML_For_GenerateMetadata(string DBOrDSDDBId, bool DBOrDSDFlag)
    {
        string RetVal;
        DataTable DtIndicator;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;
        DataView dv;
        RetVal = string.Empty;
        DtIndicator = null;
        DictIndicator = null;
        DictIndicatorMapping = null;
        dv = null;
        try
        {
            this.GetIndicatorTableAndDictionaries(DBOrDSDDBId, DBOrDSDFlag, null, out DtIndicator, out DictIndicator, out DictIndicatorMapping);
            dv = DtIndicator.DefaultView;
            dv.Sort = "Indicator_Name asc";
            DtIndicator = dv.ToTable();

            if (DictIndicator != null && DictIndicator.Keys.Count > 0)
            {
                DictIndicator = this.Sort_Dictionary(DictIndicator);
            }
            if (DBOrDSDFlag == true)
            {
                RetVal = this.GetIndicatorDivInnerHTML(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag);
            }
            else
            {
                RetVal = this.GetPublishMetadataGrid(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag, DBOrDSDDBId);
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetIndicatorDivInnerHTML_For_RegisterMetadata(string DBOrDSDDBId, bool DBOrDSDFlag)
    {
        string RetVal;
        string MetadataFolderPath, FileName;
        List<string> ListGeneratedIndicatorsGIds;
        DataTable DtIndicator;
        Dictionary<string, string> DictIndicator, DictIndicatorMapping;

        RetVal = string.Empty;
        MetadataFolderPath = string.Empty;
        FileName = string.Empty;
        ListGeneratedIndicatorsGIds = new List<string>();
        DtIndicator = null;
        DictIndicator = null;
        DictIndicatorMapping = null;

        try
        {
            if (DBOrDSDFlag == true)
            {
                MetadataFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.IndicatorMetadata);
            }
            else
            {
                MetadataFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.Metadata + "\\" + this.Get_MFDId_From_DBNId(DBOrDSDDBId) + "\\");
            }

            foreach (string FileNameWPath in Directory.GetFiles(MetadataFolderPath))
            {
                FileName = Path.GetFileNameWithoutExtension(FileNameWPath);

                if (!ListGeneratedIndicatorsGIds.Contains(FileName))
                {
                    ListGeneratedIndicatorsGIds.Add(FileName);
                }
            }

            this.GetIndicatorTableAndDictionaries(DBOrDSDDBId, DBOrDSDFlag, ListGeneratedIndicatorsGIds, out DtIndicator, out DictIndicator, out DictIndicatorMapping);
            RetVal = this.GetIndicatorDivInnerHTML(DtIndicator, DictIndicator, DictIndicatorMapping, DBOrDSDFlag);
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

    private void GetIndicatorTableAndDictionaries(string DBOrDSDDBId, bool DBOrDSDFlag, List<string> ListIndicatorsGIds, out DataTable DtIndicator, out Dictionary<string, string> DictIndicator, out Dictionary<string, string> DictIndicatorMapping)
    {
        int DBNId;
        DataTable DtIndicatorRetVal;
        DataRow DrIndicatorRetVal;

        DtIndicator = null;
        DictIndicator = null;
        DictIndicatorMapping = null;
        DBNId = -1;
        DtIndicatorRetVal = null;
        DrIndicatorRetVal = null;

        try
        {
            if (DBOrDSDFlag == true)
            {
                DBNId = Convert.ToInt32(DBOrDSDDBId);

                DictIndicator = null;
                DictIndicatorMapping = null;
                DtIndicator = RegTwoZeroFunctionality.GetIndicatorTable(DBNId, string.Empty);
                DtIndicatorRetVal = DtIndicator.Clone();

                if (ListIndicatorsGIds != null)
                {
                    foreach (DataRow DrIndicator in DtIndicator.Rows)
                    {
                        if (ListIndicatorsGIds.Contains(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                        {
                            DrIndicatorRetVal = DtIndicatorRetVal.NewRow();
                            DrIndicatorRetVal.ItemArray = DrIndicator.ItemArray;

                            DtIndicatorRetVal.Rows.Add(DrIndicatorRetVal);
                        }
                    }

                    DtIndicator = DtIndicatorRetVal;
                }
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);

                DictIndicator = RegTwoZeroFunctionality.Get_Indicator_GIds(Convert.ToInt32(DBOrDSDDBId));
                DictIndicatorMapping = RegTwoZeroFunctionality.Get_Indicator_Mapping_Dict(Convert.ToInt32(DBOrDSDDBId));
                DtIndicator = RegTwoZeroFunctionality.GetIndicatorTable(DBNId, string.Empty);
                DtIndicatorRetVal = DtIndicator.Clone();

                if (ListIndicatorsGIds != null)
                {
                    foreach (DataRow DrIndicator in DtIndicator.Rows)
                    {
                        if (DictIndicatorMapping.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                        {
                            if (ListIndicatorsGIds.Contains(DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()]))
                            {
                                DrIndicatorRetVal = DtIndicatorRetVal.NewRow();
                                DrIndicatorRetVal.ItemArray = DrIndicator.ItemArray;

                                DtIndicatorRetVal.Rows.Add(DrIndicatorRetVal);
                            }
                        }
                    }

                    DtIndicator = DtIndicatorRetVal;
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

    private string GetIndicatorDivInnerHTML(DataTable DtIndicator, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, bool DBOrDSDFlag)
    {
        string RetVal;

        RetVal = string.Empty;

        try
        {
            RetVal = "<table id=\"tblIndicator\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkIndicator_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllIndicators();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanIndicator_0\"></i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            if (DBOrDSDFlag == true)
            {
                foreach (DataRow DrIndicator in DtIndicator.Rows)
                {
                    RetVal += "<tr>";

                    RetVal += "<td style=\"width:2%\">";
                    RetVal += "<input id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                    RetVal += "</td>";

                    RetVal += "<td style=\"width:98%\">";
                    RetVal += "<span id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString() + "</span>";
                    RetVal += "</td>";

                    RetVal += "</tr>";
                }
            }
            else
            {
                foreach (DataRow DrIndicator in DtIndicator.Rows)
                {
                    if (DictIndicatorMapping.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                    {
                        RetVal += "<tr>";

                        RetVal += "<td style=\"width:2%\">";
                        RetVal += "<input id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</td>";

                        RetVal += "<td style=\"width:98%\">";
                        RetVal += "<span id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DictIndicator[DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()].ToString()] + "</span>";
                        RetVal += "</td>";

                        RetVal += "</tr>";
                    }
                }
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "-- Indicator Popup HTML --"

    #region "-- Area Popup HTML --"

    private string GetAreaDivInnerHTML_For_GenerateSDMXML(string DBOrDSDDBId, bool DBOrDSDFlag, string IndicatorNId = null)
    {
        string RetVal;
        DataTable DtAreas;
        Dictionary<string, string> DictAreas, DictAreaMapping;
        DataView dv;
        RetVal = string.Empty;
        DtAreas = null;
        DictAreas = null;
        DictAreaMapping = null;
        dv = null;
        try
        {

            this.GetAreaTableAndDictionaries(DBOrDSDDBId, DBOrDSDFlag, null, out DtAreas, out DictAreas, out DictAreaMapping, IndicatorNId);
            dv = DtAreas.DefaultView;
            dv.Sort = "Area_Name asc";
            DtAreas = dv.ToTable();

            if (DictAreas != null && DictAreas.Keys.Count > 0)
            {
                DictAreas = this.Sort_Dictionary(DictAreas);
            }
            RetVal = this.GetAreaDivInnerHTML(DtAreas, DictAreas, DictAreaMapping, DBOrDSDFlag, IndicatorNId, DBOrDSDDBId);
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetAreaDivInnerHTML_For_GenerateMetadata(string DBOrDSDDBId)
    {
        string RetVal;
        DataTable DtAreas;
        Dictionary<string, string> DictAreas, DictAreaMapping;
        DataView dv;
        RetVal = string.Empty;
        DtAreas = null;
        DictAreas = null;
        DictAreaMapping = null;
        dv = null;
        try
        {
            this.GetAreaTableAndDictionaries(DBOrDSDDBId, true, null, out DtAreas, out DictAreas, out DictAreaMapping);
            dv = DtAreas.DefaultView;
            dv.Sort = "Area_Name asc";
            DtAreas = dv.ToTable();

            if (DictAreas != null && DictAreas.Keys.Count > 0)
            {
                DictAreas = this.Sort_Dictionary(DictAreas);
            }
            RetVal = this.GetAreaDivInnerHTML(DtAreas, DictAreas, DictAreaMapping, true, string.Empty, DBOrDSDDBId);
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetAreaDivInnerHTML_For_RegisterMetadata(string DBOrDSDDBId)
    {
        string RetVal;
        string MetadataFolderPath, FileName;
        List<string> ListGeneratedAreaIds;
        DataTable DtAreas;
        Dictionary<string, string> DictAreas, DictAreaMapping;

        RetVal = string.Empty;
        MetadataFolderPath = string.Empty;
        FileName = string.Empty;
        ListGeneratedAreaIds = new List<string>();
        DtAreas = null;
        DictAreas = null;
        DictAreaMapping = null;

        try
        {
            MetadataFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.AreaMetadata);

            foreach (string FileNameWPath in Directory.GetFiles(MetadataFolderPath))
            {
                FileName = Path.GetFileNameWithoutExtension(FileNameWPath);

                if (!ListGeneratedAreaIds.Contains(FileName))
                {
                    ListGeneratedAreaIds.Add(FileName);
                }
            }

            this.GetAreaTableAndDictionaries(DBOrDSDDBId, true, ListGeneratedAreaIds, out DtAreas, out DictAreas, out DictAreaMapping);
            RetVal = this.GetAreaDivInnerHTML(DtAreas, DictAreas, DictAreaMapping, true, string.Empty, DBOrDSDDBId);
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

    private void GetAreaTableAndDictionaries(string DBOrDSDDBId, bool DBOrDSDFlag, List<string> ListAreaIDs, out DataTable DtAreas, out Dictionary<string, string> DictAreas, out Dictionary<string, string> DictAreaMapping, string IndicatorNId = null)
    {
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtAreasRetVal;
        DataRow DrAreasRetVal;

        DtAreas = null;
        DictAreas = null;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtAreasRetVal = null;
        DrAreasRetVal = null;
        DictAreaMapping = null;
        try
        {
            if (DBOrDSDFlag == true)
            {
                DBNId = Convert.ToInt32(DBOrDSDDBId);
                DIConnection = Global.GetDbConnection(DBNId);
                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

                DictAreas = null;

                DtAreas = DIConnection.ExecuteDataTable(DIQueries.Area.GetArea(FilterFieldType.None, string.Empty));

                DtAreasRetVal = DtAreas.Clone();

                if (ListAreaIDs != null)
                {
                    foreach (DataRow DrAreas in DtAreas.Rows)
                    {
                        if (ListAreaIDs.Contains(DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()))
                        {
                            DrAreasRetVal = DtAreasRetVal.NewRow();
                            DrAreasRetVal.ItemArray = DrAreas.ItemArray;

                            DtAreasRetVal.Rows.Add(DrAreasRetVal);
                        }
                    }

                    DtAreas = DtAreasRetVal;
                }
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
                DIConnection = Global.GetDbConnection(DBNId);
                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
                DictAreaMapping = RegTwoZeroFunctionality.Get_Area_Mapping_Dict(Convert.ToInt32(DBOrDSDDBId));
                DictAreas = this.Get_Area_IDs(DBOrDSDDBId);
                //if (string.IsNullOrEmpty(IndicatorNId))
                //{

                //}
                //else
                //{
                //    DtAreas = DIConnection.ExecuteDataTable(DIQueries.Area.GetAutoSelectAreas(IndicatorNId, false, string.Empty, string.Empty,0));
                //}
                DtAreas = DIConnection.ExecuteDataTable(DIQueries.Area.GetAreaByAreaLevel(Global.registryAreaLevel));
                DtAreasRetVal = DtAreas.Clone();
                foreach (DataRow DrAreas in DtAreas.Rows)
                {
                    if (DictAreaMapping.ContainsKey(DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()))
                    {

                        DrAreasRetVal = DtAreasRetVal.NewRow();
                        DrAreasRetVal.ItemArray = DrAreas.ItemArray;

                        DtAreasRetVal.Rows.Add(DrAreasRetVal);



                    }
                }

                DtAreas = DtAreasRetVal;
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

    private string GetAreaDivInnerHTML(DataTable DtAreas, Dictionary<string, string> DictAreas, Dictionary<string, string> DictAreaMapping, bool DBOrDSDFlag, string IndicatorNId, string DBNId)
    {
        string RetVal;
        string xml = string.Empty;
        RetVal = string.Empty;
        ArrayList ALItemAdded = new ArrayList();
        xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx" + "\\DataPublishedUserSelection.xml");
        if (File.Exists(xml) && string.IsNullOrEmpty(IndicatorNId) == false)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);

            string areaId = string.Empty;
            foreach (XmlElement element in xmlDoc.SelectNodes("/root/Data"))
            {
                if (element.GetAttribute("Ind") == IndicatorNId)
                {
                    areaId = element.GetAttribute("areas");

                }
            }
            if (areaId.Contains(",") && string.IsNullOrEmpty(areaId) == false)
            {
                foreach (string item in areaId.Split(','))
                {
                    ALItemAdded.Add(item);
                }
            }
            else
            {
                ALItemAdded.Add(areaId);
            }
        }
        try
        {
            RetVal = "<table id=\"tblArea\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkArea_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllAreas();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanArea_0\"></i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            if (DBOrDSDFlag == true)
            {
                foreach (DataRow DrAreas in DtAreas.Rows)
                {
                    RetVal += "<tr>";

                    RetVal += "<td style=\"width:2%\">";
                    RetVal += "<input id=\"chkArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";
                    RetVal += "</td>";

                    RetVal += "<td style=\"width:98%\">";
                    RetVal += "<span id=\"spanArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\">" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName].ToString() + "</span>";
                    RetVal += "</td>";

                    RetVal += "</tr>";
                }
            }
            else
            {
                foreach (DataRow DrAreas in DtAreas.Rows)
                {
                    if (DictAreaMapping.ContainsKey(DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()))
                    {
                        RetVal += "<tr>";

                        RetVal += "<td style=\"width:2%\">";
                        if (ALItemAdded.Count > 0)
                        {
                            if (ALItemAdded.Contains(DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString()))
                            {
                                RetVal += "<input checked=\"checked\" id=\"chkArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";

                            }
                            else
                            {
                                RetVal += "<input  id=\"chkArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";
                            }
                        }
                        else
                        {
                            RetVal += "<input id=\"chkArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";
                        }
                        RetVal += "</td>";

                        RetVal += "<td style=\"width:98%\">";
                        RetVal += "<span id=\"spanArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\">" + DictAreas[DictAreaMapping[DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()].ToString()] + "</span>";
                        RetVal += "<input id=\"hdnDBAreaId\" type=\"hidden\" value=\"" + DictAreaMapping[DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()] + "\"></input>";
                        RetVal += "</td>";

                        RetVal += "</tr>";
                    }

                    //if (DictAreas.ContainsKey(DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()))
                    //{
                    //    RetVal += "<tr>";

                    //    RetVal += "<td style=\"width:2%\">";
                    //    RetVal += "<input id=\"chkArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";
                    //    RetVal += "</td>";

                    //    RetVal += "<td style=\"width:98%\">";
                    //    RetVal += "<span id=\"spanArea_" + DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\">" + DictAreas[DrAreas[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()] + "</span>";
                    //    RetVal += "</td>";

                    //    RetVal += "</tr>";
                    //}
                }
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "-- Area Popup HTML --"

    #region "-- Source Popup HTML --"

    private string GetSourceDivInnerHTML_For_GenerateSDMXML(string DBOrDSDDBId, bool DBOrDSDFlag, string IndicatorNIDs = null)
    {
        string RetVal;
        DataTable DtSource;

        RetVal = string.Empty;
        DtSource = null;

        try
        {
            this.GetSourceTable(DBOrDSDDBId, DBOrDSDFlag, null, out DtSource, IndicatorNIDs);
            RetVal = this.GetSourceDivInnerHTML(DtSource, IndicatorNIDs, DBOrDSDDBId);
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetSourceDivInnerHTML_For_GenerateMetadata(string DBOrDSDDBId)
    {
        string RetVal;
        DataTable DtSource;

        RetVal = string.Empty;
        DtSource = null;

        try
        {
            this.GetSourceTable(DBOrDSDDBId, true, null, out DtSource);
            RetVal = this.GetSourceDivInnerHTML(DtSource, string.Empty, DBOrDSDDBId);
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string GetSourceDivInnerHTML_For_RegisterMetadata(string DBOrDSDDBId)
    {
        string RetVal;
        string MetadataFolderPath, FileName;
        List<string> ListGeneratedSourceGIds;
        DataTable DtSource;

        RetVal = string.Empty;
        MetadataFolderPath = string.Empty;
        FileName = string.Empty;
        ListGeneratedSourceGIds = new List<string>();
        DtSource = null;

        try
        {
            MetadataFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.SourceMetadata);

            foreach (string FileNameWPath in Directory.GetFiles(MetadataFolderPath))
            {
                FileName = Path.GetFileNameWithoutExtension(FileNameWPath);

                if (!ListGeneratedSourceGIds.Contains(FileName))
                {
                    ListGeneratedSourceGIds.Add(FileName);
                }
            }

            this.GetSourceTable(DBOrDSDDBId, true, ListGeneratedSourceGIds, out DtSource);
            RetVal = this.GetSourceDivInnerHTML(DtSource, string.Empty, DBOrDSDDBId);
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

    private void GetSourceTable(string DBOrDSDDBId, bool DBOrDSDFlag, List<string> ListSourceGIDs, out DataTable DtSource, string IndicatorNIDs = null)
    {
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtSourceRetVal;
        DataRow DrSourceRetVal;

        DtSource = null;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtSourceRetVal = null;
        DrSourceRetVal = null;

        try
        {
            if (DBOrDSDFlag == true)
            {
                DBNId = Convert.ToInt32(DBOrDSDDBId);
                DIConnection = Global.GetDbConnection(DBNId);
                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
                DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));

                DtSourceRetVal = DtSource.Clone();

                if (ListSourceGIDs != null)
                {
                    foreach (DataRow DrSource in DtSource.Rows)
                    {
                        if (ListSourceGIDs.Contains(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICGId].ToString()))
                        {
                            DrSourceRetVal = DtSourceRetVal.NewRow();
                            DrSourceRetVal.ItemArray = DrSource.ItemArray;

                            DtSourceRetVal.Rows.Add(DrSourceRetVal);
                        }
                    }

                    DtSource = DtSourceRetVal;
                }
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
                DIConnection = Global.GetDbConnection(DBNId);
                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
                if (string.IsNullOrEmpty(IndicatorNIDs))
                {
                    DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));
                }
                else
                {
                    DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetICForIndicators(ICType.Source, IndicatorNIDs, FieldSelection.Light));
                    foreach (DataRow dr in DtSource.Rows)
                    {
                        if (dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICParent_NId].ToString() == "-1")
                            dr.Delete();

                    }
                    DtSource.AcceptChanges();
                }
                // DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));
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
    }

    private string GetSourceDivInnerHTML(DataTable DtSource, string IndicatorNId, string DBNId)
    {
        string RetVal;

        RetVal = string.Empty;
        string xml = string.Empty;
        RetVal = string.Empty;
        ArrayList ALItemAdded = new ArrayList();
        xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx" + "\\DataPublishedUserSelection.xml");
        if (File.Exists(xml) && string.IsNullOrEmpty(IndicatorNId) == false)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);

            string source = string.Empty;
            foreach (XmlElement element in xmlDoc.SelectNodes("/root/Data"))
            {
                if (element.GetAttribute("Ind") == IndicatorNId)
                {
                    source = element.GetAttribute("source");

                }
            }
            if (source.Contains(",") && string.IsNullOrEmpty(source) == false)
            {
                foreach (string item in source.Split(','))
                {
                    ALItemAdded.Add(item);
                }
            }
            else
            {
                ALItemAdded.Add(source);
            }
        }
        try
        {
            RetVal = "<table id=\"tblSource\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkSource_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllSources();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanSource_0\"></i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            foreach (DataRow DrSource in DtSource.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                if (ALItemAdded.Count > 0)
                {
                    if (ALItemAdded.Contains(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString()))
                    {
                        RetVal += "<input checked=\"checked\" id=\"chkSource_" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\" type=\"checkbox\" value=\"" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\"/>";
                    }
                    else
                    {
                        RetVal += "<input id=\"chkSource_" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\" type=\"checkbox\" value=\"" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\"/>";
                    }
                }
                else
                {
                    RetVal += "<input id=\"chkSource_" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\" type=\"checkbox\" value=\"" + DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString() + "\"/>";

                } RetVal += "</td>";

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

        return RetVal;
    }

    #endregion "-- Source Popup HTML --"

    #region "--Generate Data--"

    private void Clean_SDMX_ML_Folder_For_UNSD(int DbNId, string DSDDbId)
    {
        string FolderName;
        string AppPhysicalPath, DbFolder, language;
        DIConnection DIConnection;
        DIQueries DIQueries;
        AppPhysicalPath = string.Empty;
        DbFolder = string.Empty;
        language = string.Empty;


        try
        {
            AppPhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;
            DbFolder = Constants.FolderName.Data + DSDDbId.ToString() + "\\";

            FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML);
            //  Global.DeleteDirectory(FolderName);
            this.Create_Directory_If_Not_Exists(FolderName);

            DIConnection = Global.GetDbConnection(DbNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            foreach (DataRow LanguageRow in DIConnection.DILanguages(DIQueries.DataPrefix).Rows)
            {
                language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();

                FolderName = Path.Combine(AppPhysicalPath, DbFolder + Constants.FolderName.SDMX.SDMX_ML + "\\" + language);
                this.Create_Directory_If_Not_Exists(FolderName);
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
    }

    #endregion "--Generate Data--"

    #region "--Generate Metadata--"

    private string GenerateMetadata_For_UNSD(int AssociatedDbNId, string DBNId, string IndicatorNIds, out string ErrorMessage, out List<string> GeneratedMetadataFiles, string HeaderFilePath)
    {
        string RetVal, xmlMetaFilePath;
        DIConnection DIConnection;
        DIQueries DIQueries;
        string OutputFolder, SummaryFileName, CodelistMappingFileName, MetadataMappingFileName, MFDId;
        bool MetadataGenerated = false;
        ErrorMessage = string.Empty;
        xmlMetaFilePath = string.Empty;
        RetVal = "false";
        DIConnection = Global.GetDbConnection(AssociatedDbNId);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\" + Constants.FolderName.SDMX.Metadata);
        SummaryFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\" + Constants.FolderName.SDMX.sdmx + "Summary.xml");
        CodelistMappingFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
        MetadataMappingFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.ConceptSchemeMap.MetadataMap.FileName);

        // this.Clean_Metadata_Folder_For_UNSD(OutputFolder);//To be done in action
        MFDId = this.Get_MFDId_From_DBNId(DBNId);
        this.Create_Directory_If_Not_Exists(Path.Combine(OutputFolder, MFDId));

        Global.GetAppSetting();

        try
        {
            xmlMetaFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx" + "\\MetadataPublishedUserSelection.xml");
            MetadataGenerated = RegTwoZeroMetadata.Generate_MetadataReport(SummaryFileName, CodelistMappingFileName, MetadataMappingFileName, IndicatorNIds, Global.registryMSDAreaId, DIConnection, DIQueries, Path.Combine(OutputFolder, MFDId), out ErrorMessage, out GeneratedMetadataFiles, HeaderFilePath, xmlMetaFilePath);

            if (MetadataGenerated == true)
            {
                RetVal = "true";
            }
            else
            {

                RetVal = "false" + Constants.Delimiters.ParamDelimiter + ErrorMessage;
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog(ex.StackTrace);
            Global.CreateExceptionString(ex, null);

            throw ex;
        }

        return RetVal;
    }

    private void Clean_Metadata_Folder_For_UNSD(string MetadataFolder)
    {
        foreach (string ChildMetadataFolder in Directory.GetDirectories(MetadataFolder))
        {
            Global.DeleteDirectory(ChildMetadataFolder);
        }
    }

    private string Get_MFDId_From_DBNId(string DbNId)
    {
        string RetVal = string.Empty;

        DataTable DtTable;
        DIConnection DIConnection;

        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
        DtTable = DIConnection.ExecuteDataTable("SELECT Id FROM Artefacts WHERE DBNId = " + DbNId + " AND Type = " +
                                                 Convert.ToInt32(ArtefactTypes.MFD).ToString() + ";");

        if (DtTable != null && DtTable.Rows.Count > 0)
        {
            RetVal = DtTable.Rows[0]["Id"].ToString();
        }

        return RetVal;
    }

    #endregion "--Generate Metadata--"

    #region "--Data/MetadataGridforIndicator_Area_Time_Source--"
    public string GetJSONString(DataTable table)
    {
        StringBuilder headStrBuilder = new StringBuilder(table.Columns.Count * 5); //pre-allocate some space, default is 16 bytes
        for (int i = 0; i < table.Columns.Count; i++)
        {
            headStrBuilder.AppendFormat("\"{0}\" : \"{0}{1}¾\",", table.Columns[i].Caption, i);
        }
        headStrBuilder.Remove(headStrBuilder.Length - 1, 1); // trim away last ,

        StringBuilder sb = new StringBuilder(table.Rows.Count * 5); //pre-allocate some space
        sb.Append("{\"");
        sb.Append(table.TableName);
        sb.Append("\" : [");
        for (int i = 0; i < table.Rows.Count; i++)
        {
            string tempStr = headStrBuilder.ToString();
            sb.Append("{");
            for (int j = 0; j < table.Columns.Count; j++)
            {
                table.Rows[i][j] = table.Rows[i][j].ToString().Replace("'", "");
                tempStr = tempStr.Replace(table.Columns[j] + j.ToString() + "¾", table.Rows[i][j].ToString());
            }
            sb.Append(tempStr + "},");
        }
        sb.Remove(sb.Length - 1, 1); // trim last ,
        sb.Append("]}");
        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="DtIndicator"></param>
    /// <param name="DictIndicator"></param>
    /// <param name="DictIndicatorMapping"></param>
    /// <param name="DBOrDSDFlag"></param>
    /// <param name="DBNId"></param>
    /// <returns></returns>
    private string GetPublishDataGrid(DataTable DtIndicator, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, bool DBOrDSDFlag, string DBNId)
    {
        string RetVal;
        string chkAttribute = "";
        string IndicatorNIdForGID = string.Empty;
        string areas = string.Empty;
        string timeperiods = "";
        string sources = "";
        string selectedState = "false";
        string selectedJson = string.Empty;
        RetVal = string.Empty;
        DataSet ds = new DataSet();
        string xml = string.Empty;
        try
        {
            xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx" + "\\DataPublishedUserSelection.xml");
            if (File.Exists(xml) == false)
            {
                XmlDocument docConfig = new XmlDocument();
                XmlNode xmlNode = docConfig.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                XmlElement rootElement = docConfig.CreateElement("root");
                docConfig.AppendChild(rootElement);
                foreach (DataRow DrowIndicator in DtIndicator.Rows)
                {
                    if (DictIndicatorMapping.ContainsKey(DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                    {
                        // Create <Data> Node
                        XmlElement DataElement = docConfig.CreateElement("Data");
                        DataElement.SetAttribute("Ind", DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString());
                        DataElement.SetAttribute("areas", "");
                        DataElement.SetAttribute("timeperiods", "");
                        DataElement.SetAttribute("source", "");
                        DataElement.SetAttribute("selectedState", "false");
                        rootElement.AppendChild(DataElement);
                    }
                }

                docConfig.Save(xml);
            }
            else
            {
                XmlDocument docConfig = new XmlDocument();
                docConfig.Load(xml);
                string IndNId = string.Empty;
                ArrayList ALItemAdded = new ArrayList();
                XmlNode rootElement = docConfig.SelectSingleNode("/root");
                foreach (XmlElement element in docConfig.SelectNodes("/root/Data"))
                {
                    if (!ALItemAdded.Contains(element.GetAttribute("Ind")))
                    {
                        ALItemAdded.Add(element.GetAttribute("Ind"));
                    }
                }
                foreach (DataRow DrowIndicator in DtIndicator.Rows)
                {
                    foreach (XmlElement element in docConfig.SelectNodes("/root/Data"))
                    {
                        if (DictIndicatorMapping.ContainsKey(DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                        {
                            IndNId = DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString();
                            if (!ALItemAdded.Contains(IndNId))
                            {
                                if (element.GetAttribute("Ind") != DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString())
                                {
                                    XmlElement DataElement = docConfig.CreateElement("Data");
                                    DataElement.SetAttribute("Ind", DrowIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString());
                                    DataElement.SetAttribute("areas", "");
                                    DataElement.SetAttribute("timeperiods", "");
                                    DataElement.SetAttribute("source", "");
                                    DataElement.SetAttribute("selectedState", "false");
                                    rootElement.AppendChild(DataElement);
                                }
                                docConfig.Save(xml);
                                ALItemAdded.Add(IndNId);
                            }

                        }
                    }
                }

            }

            if (File.Exists(xml))
            {
                using (FileStream fs = new FileStream(xml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    ds = new DataSet();
                    ds.ReadXml(fs);
                }
            }

            RetVal = "<table id=\"tblIATSGrid\" class=\"pivot_table\" style=\"width:100%; height:50%; overflow:auto;\">";
            RetVal += "<tr>";
            RetVal += "<th class=\"h1\" style=\"width:5%\"> <input id=\"chkIndicator_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllIndicators();\"/>";
            RetVal += "</th>";
            RetVal += "<th class=\"h1\" style=\"width:35%\"> Indicators";
            RetVal += "</th>";
            RetVal += "<th class=\"h1\" style=\"width:20%\"> Area";
            RetVal += "</th>";
            RetVal += "<th class=\"h1\" style=\"width:20%\"> Time";
            RetVal += "</th>";
            RetVal += "<th class=\"h1\" style=\"width:20%\"> Source";
            RetVal += "</th>";
            RetVal += "</tr>";
            foreach (DataRow DrIndicator in DtIndicator.Rows)
            {

                chkAttribute = "";
                areas = "";
                timeperiods = "";
                sources = "";
                selectedState = "false";
                if (DictIndicatorMapping.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                {
                    IndicatorNIdForGID = DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString();
                    if (File.Exists(xml))
                    {
                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow DSRow in ds.Tables["Data"].Select("Ind='" + IndicatorNIdForGID + "'"))
                            {
                                chkAttribute = "";
                                areas = DSRow["areas"].ToString();
                                timeperiods = DSRow["timeperiods"].ToString();
                                sources = DSRow["source"].ToString();
                                selectedState = DSRow["selectedState"].ToString();
                                if (selectedState == "true")
                                {
                                    chkAttribute = "checked";
                                }
                            }
                        }
                    }

                    RetVal += "<tr>";

                    RetVal += "<th class=\"h2\">";
                    if (chkAttribute == "checked")
                    {
                        RetVal += "<input checked=\"" + chkAttribute + "\" id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</th>";

                        RetVal += "<th class=\"h2\" style=\"padding-left: 10px;\">";
                        RetVal += "<span style=\"float:left\" id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString() + "</span>";
                        RetVal += "</th>";
                        if (string.IsNullOrEmpty(areas))
                        {
                            RetVal += "<th id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"   style=\"padding-left: 10px;\"> Select <a rel=\" " + areas + "\" id=\"lkArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenAreaPopup(this);\">[+]</a> <a rel=\" " + areas + "\" id=\"clrArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveAreas(this);\">[X]</a> <span id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span> ";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"   style=\"padding-left: 10px;\"> <b>Select</b> <a rel=\" " + areas + "\" id=\"lkArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenAreaPopup(this);\">[+]</a> <a rel=\" " + areas + "\" id=\"clrArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveAreas(this);\">[X]</a> <span id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span> ";
                            RetVal += "</th>";
                        }
                        if (string.IsNullOrEmpty(timeperiods))
                        {
                            RetVal += "<th id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  Select <a rel=\" " + timeperiods + "\" id=\"lkTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenTPPopup(this);\">[+]</a> <a rel=\" " + timeperiods + "\" id=\"clrTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveTimeperiods(this);\">[X]</a>";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  <b>Select</b> <a rel=\" " + timeperiods + "\" id=\"lkTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenTPPopup(this);\">[+]</a> <a rel=\" " + timeperiods + "\" id=\"clrTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveTimeperiods(this);\">[X]</a>";
                            RetVal += "</th>";
                        }
                        if (string.IsNullOrEmpty(sources))
                        {
                            RetVal += "<th id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  Select <a rel=\" " + sources + "\" id=\"lkSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenSourcePopup(this);\">[+]</a>  <a rel=\" " + sources + "\" id=\"clrSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveSource(this);\">[X]</a>";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  <b>Select</b> <a rel=\" " + sources + "\" id=\"lkSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenSourcePopup(this);\">[+]</a>  <a rel=\" " + sources + "\" id=\"clrSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveSource(this);\">[X]</a>";
                            RetVal += "</th>";
                        }


                        RetVal += "</tr>";
                    }
                    else
                    {
                        RetVal += "<input id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</th>";
                        //DictIndicator[DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()].ToString()]
                        RetVal += "<th class=\"h2\" style=\"padding-left: 10px;\">";
                        RetVal += "<span style=\"float:left\" id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString() + "</span>";
                        RetVal += "</th>";
                        if (string.IsNullOrEmpty(areas))
                        {
                            RetVal += "<th id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\"> Select <a rel=\" " + areas + "\" id=\"lkArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenAreaPopup(this);\">[+]</a>  <a rel=\" " + areas + "\" id=\"clrArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveAreas(this);\">[X]</a>";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\"> <b>Select</b> <a rel=\" " + areas + "\" id=\"lkArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenAreaPopup(this);\">[+]</a>  <a rel=\" " + areas + "\" id=\"clrArea_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveAreas(this);\">[X]</a>";
                            RetVal += "</th>";
                        }
                        if (string.IsNullOrEmpty(timeperiods))
                        {
                            RetVal += "<th id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  Select <a rel=\" " + timeperiods + "\" id=\"lkTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenTPPopup(this);\">[+]</a> <a rel=\" " + timeperiods + "\" id=\"clrTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveTimeperiods(this);\">[X]</a>  <span id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span>";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  <b>Select</b> <a rel=\" " + timeperiods + "\" id=\"lkTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenTPPopup(this);\">[+]</a> <a rel=\" " + timeperiods + "\" id=\"clrTP_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveTimeperiods(this);\">[X]</a>  <span id=\"spanTime_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span>";
                            RetVal += "</th>";
                        }
                        if (string.IsNullOrEmpty(sources))
                        {
                            RetVal += "<th id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  Select <a  rel=\" " + sources + "\" id=\"lkSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenSourcePopup(this);\">[+]</a> <a rel=\" " + sources + "\" id=\"clrSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveSource(this);\">[X]</a>  <span id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span>";
                            RetVal += "</th>";
                        }
                        else
                        {
                            RetVal += "<th id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" class=\"h2\"  style=\"padding-left: 10px;\">  <b>Select</b> <a  rel=\" " + sources + "\" id=\"lkSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"OpenSourcePopup(this);\">[+]</a> <a rel=\" " + sources + "\" id=\"clrSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" href=\"#\" onclick=\"RemoveSource(this);\">[X]</a>  <span id=\"spanSource_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"></span>";
                            RetVal += "</th>";
                        }
                        RetVal += "</tr>";
                    }

                }

            }


            RetVal += "</table>";
            if (ds.Tables.Contains("Data") == true)
            {
                if (ds.Tables["Data"].Rows.Count > 0)
                {
                    DataTable newTable = ds.Tables["Data"].DefaultView.ToTable(false, "Ind", "areas", "timeperiods", "source");

                    selectedJson = GetJSONString(newTable);

                    RetVal += Constants.Delimiters.EndDelimiter + selectedJson;
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

            ds = null;
            xml = null;
        }

        return RetVal;
    }

    private string GetPublishMetadataGrid(DataTable DtIndicator, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, bool DBOrDSDFlag, string DBNId)
    {
        string RetVal;
        string chkAttribute = "";
        string IndicatorNIdForGID = string.Empty;
        RetVal = string.Empty;
        string selectedJson = string.Empty;
        DataSet ds = new DataSet();
        string xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx" + "\\MetadataPublishedUserSelection.xml");
        try
        {
            if (File.Exists(xml))
            {
                using (FileStream fs = new FileStream(xml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    ds = new DataSet();
                    ds.ReadXml(fs);
                }
            }
            RetVal = "<table id=\"tblIATSGrid\" class=\"pivot_table\" style=\"width:100%; height:50%; overflow:auto;\">";
            RetVal += "<tr>";
            RetVal += "<th class=\"h1\" style=\"width:5%\"> <input id=\"chkIndicator_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllIndicators();\"/>";
            RetVal += "</th>";
            RetVal += "<th class=\"h1\" style=\"width:35%\"> Indicators";
            RetVal += "</th>";
            RetVal += "</tr>";

            foreach (DataRow DrIndicator in DtIndicator.Rows)
            {
                chkAttribute = "";
                if (DictIndicatorMapping.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                {
                    IndicatorNIdForGID = DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString();
                    if (File.Exists(xml))
                    {
                        foreach (DataRow DSRow in ds.Tables["Data"].Select("Ind='" + IndicatorNIdForGID + "'"))
                        {
                            chkAttribute = "checked";

                        }
                    }
                    if (chkAttribute == "checked")
                    {
                        RetVal += "<tr>";

                        RetVal += "<th class=\"h2\">";
                        RetVal += "<input checked=\"" + chkAttribute + "\" id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</th>";

                        RetVal += "<th class=\"h2\" style=\"padding-left: 10px; text-align:left\">";
                        RetVal += "<span id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString() + "</span>";
                        RetVal += "</th>";
                        RetVal += "</tr>";
                    }
                    else
                    {
                        RetVal += "<tr>";

                        RetVal += "<th class=\"h2\" >";
                        RetVal += "<input id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</th>";
                        //+DictIndicator[DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()].ToString()] +
                        RetVal += "<th class=\"h2\" style=\"padding-left: 10px; text-align:left\">";
                        RetVal += "<span id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString() + "</span>";
                        RetVal += "</th>";
                        RetVal += "</tr>";
                    }
                }
            }


            RetVal += "</table>";

            if (ds.Tables.Contains("Data") == true)
            {
                if (ds.Tables["Data"].Rows.Count > 0)
                {
                    DataTable newTable = ds.Tables["Data"].DefaultView.ToTable(false, "Ind", "areas", "timeperiods", "source");

                    selectedJson = GetJSONString(newTable);

                    RetVal += Constants.Delimiters.EndDelimiter + selectedJson;
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "--DataGridforIndicator_Area_Time_Source--"

    #region PublishDataFileForAdaptationandDSDFromAdmin

    private string PublishDataFilesForDB(bool DBOrDSDFlag, string DBOrDSDDBId, string AgencyId, string UserNId)
    {
        string RetVal = string.Empty;
        string IndicatorNIds, AreaNIds, TPNIds, SourceNIds, OutputFolder, AreaId, HeaderFilePath, OriginalDBNId;
        int DBNId;
        int fileCount;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, string> DictQuery;
        Dictionary<string, string> DictMapping;
        XmlDocument Query;
        List<string> GeneratedFiles;
        List<string> ListIndicatorForRegistrations;
        RetVal = "false";
        fileCount = 0;
        AreaId = string.Empty;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        OutputFolder = string.Empty;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DictQuery = new Dictionary<string, string>();
        DictMapping = new Dictionary<string, string>();
        Query = null;
        GeneratedFiles = new List<string>();// Get collection of files 
        ListIndicatorForRegistrations = new List<string>();
        HeaderFilePath = string.Empty;
        OriginalDBNId = string.Empty;
        HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
        DBNId = Convert.ToInt32(DBOrDSDDBId);
        DIConnection = Global.GetDbConnection(DBNId);
        DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
        XmlDocument UploadedHeaderXml = new XmlDocument();
        SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
        SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
        if (File.Exists(HeaderFilePath))
        {
            UploadedHeaderXml.Load(HeaderFilePath);
            UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
        }
        //   this.Clean_SDMX_ML_Folder(DBNId);
        IndicatorNIds = "-1";
        AreaNIds = "-1";
        TPNIds = "-1";
        SourceNIds = "-1";
        AreaId = "";

        this.Add_IUS_To_Dictionary(DictQuery, IndicatorNIds, DIConnection, DIQueries);
        this.Add_Area_To_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);
        this.Add_TP_To_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
        this.Add_Source_To_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

        Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, AgencyId, DIConnection, DIQueries);
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
        if (SDMXUtility.Generate_Data(SDMXSchemaType.Two_One, Query, DataFormats.StructureSpecificTS, DIConnection, DIQueries, OutputFolder, out fileCount, out GeneratedFiles, Header) == true)
        {
            if (fileCount == 0)
            {
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "NDF";
            }
            else
            {
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
                //ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, string.Empty);

                this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, GeneratedFiles, DBNId.ToString());
            }
        }
        else
        {
            RetVal = "false";
        }


        return RetVal;

    }

    private string PublishDataFilesForDSD(bool DBOrDSDFlag, string DBOrDSDDBId, string AgencyId, string UserNId)
    {
        string RetVal;
        string IndicatorNIds, AreaNIds, TPNIds, SourceNIds, OutputFolder, AreaId, HeaderFilePath, OriginalDBNId, DuplicateKey;
        int DBNId;
        int fileCount;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, string> DictQuery;
        Dictionary<string, string> DictMapping;
        XmlDocument Query;
        List<string> GeneratedFiles;
        List<string> ListIndicatorForRegistrations;
        RetVal = "false";
        fileCount = 0;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;

        OutputFolder = string.Empty;

        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DictQuery = new Dictionary<string, string>();
        DictMapping = new Dictionary<string, string>();
        Query = null;
        GeneratedFiles = new List<string>();// Get collection of files 
        ListIndicatorForRegistrations = new List<string>();
        HeaderFilePath = string.Empty;
        OriginalDBNId = string.Empty;
        AreaId = string.Empty;
        DuplicateKey = string.Empty;
        string xml = string.Empty;
        string ErrorLogs = string.Empty;
        List<string> GeneratedIndicatorGIDs = new List<string>();
        try
        {
            DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
            DictMapping = this.Get_IUS_Mapping_Dict(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\" + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml"));

            this.Clean_SDMX_ML_Folder_For_UNSD(DBNId, DBOrDSDDBId);
            OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
            xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\sdmx" + "\\DataPublishedUserSelection.xml");

            if (RegTwoZeroData.Generate_Data(DictMapping, OutputFolder, DIConnection, DIQueries, AreaId, DBOrDSDDBId, HeaderFilePath, xml, null, out fileCount, out GeneratedFiles, out ErrorLogs, out DuplicateKey) == true)
            {
                //MNF-Mapping Not found in case if IUS Mapping doesnot exist.
                //NDF-No data Found if datavalues does not exist corresponding to the indicator(s) selected.
                if (DictMapping.Count == 0)
                {
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + "0" + Constants.Delimiters.ParamDelimiter + "MNF";
                }
                else if (fileCount == 0 && DictMapping.Count == 0)
                {
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";
                }
                else if (fileCount == 0 && DictMapping.Count > 0 && string.IsNullOrEmpty(DuplicateKey) == false)
                {
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "DK" + Constants.Delimiters.ParamDelimiter + DuplicateKey;
                }
                else if (fileCount == 0 && DictMapping.Count > 0)
                {
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "NDF";
                }
                else
                {
                    if (string.IsNullOrEmpty(DuplicateKey) == false)
                    {
                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + DuplicateKey + Constants.Delimiters.ParamDelimiter + "DK";
                    }
                    else
                    {
                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
                    }
                    Session["GeneratedFiles"] = (List<string>)GeneratedFiles;

                    ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

                    this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, GeneratedFiles, DBNId.ToString());
                }
            }
            else
            {
                RetVal = "false";
                if (DictMapping.Count == 0)
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";

                }
            }

        }
        catch (Exception ex)
        {
            RetVal = "false";
        }
        finally
        {
            xml = string.Empty;
        }
        return RetVal;
    }

    #endregion PublishDataFileForAdaptationandDSD

    private string[] AddItemsInSelectedCollection(string[] selectedCollection, string IndicatorNIds)
    {
        string[] SelectedCollection = null;
        string[] currentItem = null;
        string[] selectedField = null;
        string selectedType = string.Empty;
        string selectedValue = string.Empty;
        string selectedIndicator = string.Empty;
        if (selectedCollection != null)
        {
            ArrayList Collection = new ArrayList();
            Collection.AddRange(selectedCollection);
            int count = 0;
            foreach (string IndicatorNId in IndicatorNIds.Split(','))
            {
                count = selectedCollection.Length - 1;
                foreach (string item in selectedCollection)
                {
                    currentItem = item.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None);
                    foreach (string field in currentItem)
                    {
                        if (string.IsNullOrEmpty(field) == false)
                        {
                            selectedField = field.Split(new string[] { Constants.Delimiters.Underscore }, StringSplitOptions.None);
                            selectedType = selectedField[0];
                            selectedValue = selectedField[1];
                            if (selectedType == "Indicator")
                            {
                                if (selectedValue == IndicatorNId)
                                {
                                    continue;
                                }
                                else
                                {
                                    Collection.Add("Indicator_" + IndicatorNId);

                                    // SelectedCollection.SetValue("Indicator_" + IndicatorNId, count);

                                    // SelectedCollection[count] = "Indicator_" + IndicatorNId;
                                    // SelectedCollection = "Indicator_" + IndicatorNId;
                                }

                            }
                        }
                    }
                }

            }
            SelectedCollection = (string[])Collection.ToArray(typeof(string));
        }


        return SelectedCollection;
    }

    #region "--Common--"

    private string Get_AssociatedDB_Name(XmlNodeList DBList, string AssociatedDbNId)
    {
        string RetVal;

        RetVal = string.Empty;

        foreach (XmlNode DB in DBList)
        {
            if (DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value == AssociatedDbNId)
            {
                RetVal = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value;
                break;
            }
        }

        return RetVal;
    }

    public int Get_AssociatedDB_NId(string DSDDBId)
    {
        int RetVal;
        string DBXMLFileName;
        XmlDocument DBXMLDocument;
        XmlNodeList DBList;

        RetVal = -1;
        DBXMLFileName = string.Empty;
        DBXMLDocument = null;
        DBList = null;

        DBXMLFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
        DBXMLDocument = new XmlDocument();
        DBXMLDocument.Load(DBXMLFileName);
        DBList = DBXMLDocument.GetElementsByTagName(Constants.XmlFile.Db.Tags.Database);

        foreach (XmlNode DB in DBList)
        {
            if (DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value == DSDDBId)
            {
                if (DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb] != null)
                {
                    RetVal = Convert.ToInt32(DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value);
                    break;
                }
            }
        }

        return RetVal;
    }

    private void Add_IUS_To_Dictionary(Dictionary<string, string> DictQuery, string IndicatorNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtIUSConcise, DtIUSElaborate;
        string IndicatorGId, UnitGId, SubgroupValGId, IUSNIds, IUSGIds;

        IUSNIds = string.Empty;
        IUSGIds = string.Empty;

        try
        {
            DtIUSConcise = DIConnection.ExecuteDataTable(DIQueries.IUS.GetIUSFromIndicator(IndicatorNIds));

            foreach (DataRow DrIUSConcise in DtIUSConcise.Rows)
            {
                IUSNIds += DrIUSConcise[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId].ToString() + ",";
            }

            if (IUSNIds.Length > 0)
            {
                IUSNIds = IUSNIds.Substring(0, IUSNIds.Length - 1);
            }

            DtIUSElaborate = DIConnection.ExecuteDataTable(DIQueries.IUS.GetIUS(FilterFieldType.NId, IUSNIds, FieldSelection.Light));
            DtIUSElaborate = DtIUSElaborate.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId,
                             DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            foreach (DataRow DrIUSElaborate in DtIUSElaborate.Rows)
            {
                IndicatorGId = DrIUSElaborate[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString();
                UnitGId = DrIUSElaborate[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString();
                SubgroupValGId = DrIUSElaborate[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                IUSGIds += IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.AtTheRate + SubgroupValGId + ",";
            }

            if (IUSGIds.Length > 0)
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.INDICATOR.Id, IUSGIds.Substring(0, IUSGIds.Length - 1));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
    }

    private void Add_Area_To_Dictionary(Dictionary<string, string> DictQuery, string AreaNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtAreas;
        string AreaID, AreaIDs;

        AreaIDs = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(AreaNIds))
            {
                DtAreas = DIConnection.ExecuteDataTable(DIQueries.Area.GetArea(FilterFieldType.NId, AreaNIds));

                foreach (DataRow DrArea in DtAreas.Rows)
                {
                    AreaID = DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString();
                    AreaIDs += AreaID + ",";
                }
            }

            if (AreaIDs.Length > 0)
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.AREA.Id, AreaIDs.Substring(0, AreaIDs.Length - 1));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
    }

    private void Add_TP_To_Dictionary(Dictionary<string, string> DictQuery, string TPNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtTimePeriods;
        string TPText, TPTexts;

        TPTexts = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(TPNIds))
            {
                DtTimePeriods = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, TPNIds));

                foreach (DataRow DrTimePeriod in DtTimePeriods.Rows)
                {
                    TPText = DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString();
                    TPTexts += TPText + ",";
                }
            }

            if (TPTexts.Length > 0)
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.TIME_PERIOD.Id, TPTexts.Substring(0, TPTexts.Length - 1));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
    }

    private void Add_Source_To_Dictionary(Dictionary<string, string> DictQuery, string SourceNIds, DIConnection DIConnection, DIQueries DIQueries)
    {
        DataTable DtSources;
        string Source, SourceTexts;

        SourceTexts = string.Empty;

        try
        {
            if (!string.IsNullOrEmpty(SourceNIds))
            {
                DtSources = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, SourceNIds, ICType.Source, FieldSelection.Light));

                foreach (DataRow DrArea in DtSources.Rows)
                {
                    Source = DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString();
                    SourceTexts += Source + ",";
                }
            }

            if (SourceTexts.Length > 0)
            {
                DictQuery.Add(DevInfo.Lib.DI_LibSDMX.Constants.Concept.SOURCE.Id, SourceTexts.Substring(0, SourceTexts.Length - 1));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
    }

    private Dictionary<string, string> Get_Area_IDs(string DBOrDSDDBId)
    {
        Dictionary<string, string> RetVal;
        string CompleteFileWPath;
        SDMXApi_2_0.Message.StructureType Structure;

        RetVal = new Dictionary<string, string>();
        Global.GetAppSetting();
        CompleteFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\sdmx\\Complete.xml");
        Structure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), CompleteFileWPath);

        foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
        {
            if (Codelist.id == "CL_REF_AREA_MDG")
            {
                foreach (SDMXApi_2_0.Structure.CodeType Code in Codelist.Code)
                {
                    RetVal.Add(Code.value, Code.Description[0].Value);
                }
            }
        }

        return RetVal;
    }

    private Dictionary<string, string> Get_IUS_Mapping_Dict(string IUSMappingFileNameWPath)
    {
        Dictionary<string, string> RetVal;
        XmlDocument IUSMapping;

        RetVal = new Dictionary<string, string>();
        IUSMapping = new XmlDocument();

        try
        {
            if (File.Exists(IUSMappingFileNameWPath))
            {
                IUSMapping.Load(IUSMappingFileNameWPath);

                foreach (XmlNode Mapping in IUSMapping.GetElementsByTagName("Mapping"))
                {
                    RetVal.Add(Mapping.Attributes["IUS"].Value, Mapping.Attributes["Series"].Value + "@__@" + Mapping.Attributes["Unit"].Value + "@__@" + Mapping.Attributes["Age"].Value + "@__@" + Mapping.Attributes["Sex"].Value + "@__@" + Mapping.Attributes["Location"].Value + "@__@" + Mapping.Attributes["Frequency"].Value + "@__@" + Mapping.Attributes["SourceType"].Value + "@__@" + Mapping.Attributes["Nature"].Value + "@__@" + Mapping.Attributes["UnitMult"].Value);
                }
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

    private void Add_Minus_One_To_Selections(ref string IndicatorNIds, ref string AreaNIds, ref string SourceNIds)
    {
        if (string.IsNullOrEmpty(IndicatorNIds))
        {
            IndicatorNIds = "-1";
        }
        else
        {
            IndicatorNIds += ",-1";
        }

        if (string.IsNullOrEmpty(AreaNIds))
        {
            AreaNIds = "-1";
        }
        else
        {
            AreaNIds += ",-1";
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

    private List<string> Get_Indicator_List_For_Registrations(string DBOrDSDDBId, bool DBOrDSDFlag, string IndicatorNIds)
    {
        List<string> RetVal;
        int DBNId;
        DataTable DtIndicator;
        Dictionary<string, string> DictIndicatorMapping;

        RetVal = new List<string>();
        DBNId = -1;
        DtIndicator = null;
        DictIndicatorMapping = null;

        try
        {
            if (DBOrDSDFlag == true)
            {
                DBNId = Convert.ToInt32(DBOrDSDDBId);

                DtIndicator = RegTwoZeroFunctionality.GetIndicatorTable(DBNId, IndicatorNIds);

                foreach (DataRow DrIndicator in DtIndicator.Rows)
                {
                    if (!RetVal.Contains(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                    {
                        RetVal.Add(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString());
                    }
                }
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);

                DtIndicator = RegTwoZeroFunctionality.GetIndicatorTable(DBNId, IndicatorNIds);
                DictIndicatorMapping = RegTwoZeroFunctionality.Get_Indicator_Mapping_Dict(Convert.ToInt32(DBOrDSDDBId));

                foreach (DataRow DrIndicator in DtIndicator.Rows)
                {
                    if (DictIndicatorMapping.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                    {
                        if (!RetVal.Contains(DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()]))
                        {
                            RetVal.Add(DictIndicatorMapping[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()]);
                        }
                    }
                }
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

    private List<string> Get_Area_List_For_Registrations(string DBOrDSDDBId, string AreaNIds)
    {
        List<string> RetVal;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtArea;

        RetVal = new List<string>();
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtArea = null;

        try
        {
            DBNId = Convert.ToInt32(DBOrDSDDBId);

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            DtArea = DIConnection.ExecuteDataTable(DIQueries.Area.GetArea(FilterFieldType.NId, AreaNIds));

            foreach (DataRow DrArea in DtArea.Rows)
            {
                if (!RetVal.Contains(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()))
                {
                    RetVal.Add(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString());
                }
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

    private List<string> Get_Source_List_For_Registrations(string DBOrDSDDBId, string SourceNIds)
    {
        List<string> RetVal;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtSource;

        RetVal = new List<string>();
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtSource = null;

        try
        {
            DBNId = Convert.ToInt32(DBOrDSDDBId);

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, SourceNIds, ICType.Source, FieldSelection.Light));

            foreach (DataRow DrSource in DtSource.Rows)
            {
                if (!RetVal.Contains(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICGId].ToString()))
                {
                    RetVal.Add(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICGId].ToString());
                }
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

    private string Get_IndicatorGId_From_SDMXML(string FileNameWPath, bool DBOrDSDFlag, List<string> GeneratedIndicatorCountryGIDS)
    {
        string RetVal;
        XmlDocument Document;

        RetVal = string.Empty;
        Document = new XmlDocument();

        try
        {
            Document.Load(FileNameWPath);

            if (DBOrDSDFlag == true)
            {
                RetVal = Document.GetElementsByTagName("Series")[0].Attributes["INDICATOR"].Value;
            }
            else
            {
                foreach (XmlNode item in Document.GetElementsByTagName("sts:Series"))
                {
                    if (GeneratedIndicatorCountryGIDS.Contains(item.Attributes["SERIES"].Value))//item.Attributes["SERIES"].Value == IndicatorGID
                    {
                        // RetVal = Document.GetElementsByTagName("sts:Series")[0].Attributes["SERIES"].Value;
                        RetVal = item.Attributes["SERIES"].Value;
                        break;
                    }
                }


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



    #endregion "--Common--"


    #endregion "--Private--"

    #region "--Public--"

    public string DataProviderGetDatabaseDivInnerHTML(string requestParam)
    {
        string RetVal;
        string UserNId;
        string DBXMLFileName;
        string DbNId, Publisher;
        XmlDocument DBXMLDocument;
        XmlNodeList DBList;

        RetVal = string.Empty;
        UserNId = string.Empty;
        DBXMLFileName = string.Empty;
        DbNId = string.Empty;
        Publisher = string.Empty;
        DBXMLDocument = null;
        DBList = null;

        try
        {
            UserNId = requestParam;
            DBXMLFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            DBXMLDocument = new XmlDocument();
            DBXMLDocument.Load(DBXMLFileName);
            DBList = DBXMLDocument.GetElementsByTagName(Constants.XmlFile.Db.Tags.Database);

            RetVal = "<table id=\"tblDatabase\" style=\"width:100%\">";

            if (DBList != null && DBList.Count > 0)
            {
                RetVal += "<tr class=\"HeaderRowStyle\">";
                RetVal += "<td class=\"HeaderColumnStyle\" style=\"width:2%\"></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Database\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Publisher\"></span></td>";
                RetVal += "</tr>";

                foreach (XmlNode DB in DBList)
                {
                    if (Convert.ToBoolean(DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value) == false)
                    {
                        DbNId = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                        Publisher = Global.Get_AgencyId_From_DFD(DbNId);

                        RetVal += "<tr class=\"DataRowStyle\">";

                        RetVal += "<td class=\"DataColumnStyle\">";

                        if (string.IsNullOrEmpty(Publisher) || Publisher == DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId)
                        {
                            RetVal += "<input id=\"radio_" + DbNId + "\" type=\"radio\" name=\"db\" value=\"" + DbNId + "\" />";
                        }

                        RetVal += "</td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Name].Value + "</span></td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + Publisher + "</span></td>";

                        RetVal += "</tr>";
                    }
                }
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
        }

        return RetVal;
    }

    public string DataProviderGetDSDDivInnerHTML(string requestParam)
    {
        string RetVal;
        string UserNId;
        string DBXMLFileName;
        string DbNId, DSDId, DSDAgencyId, DSDVersion, AssociatedDB, Publisher;
        XmlDocument DBXMLDocument;
        XmlNodeList DBList;

        RetVal = string.Empty;
        UserNId = string.Empty;
        DBXMLFileName = string.Empty;
        DbNId = string.Empty;
        DSDId = string.Empty;
        DSDAgencyId = string.Empty;
        DSDVersion = string.Empty;
        AssociatedDB = string.Empty;
        Publisher = string.Empty;
        DBXMLDocument = null;
        DBList = null;

        try
        {
            UserNId = requestParam;
            DBXMLFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            DBXMLDocument = new XmlDocument();
            DBXMLDocument.Load(DBXMLFileName);
            DBList = DBXMLDocument.GetElementsByTagName(Constants.XmlFile.Db.Tags.Database);

            RetVal = "<table id=\"tblDSD\" style=\"width:100%\">";

            if (DBList != null && DBList.Count > 0)
            {
                RetVal += "<tr class=\"HeaderRowStyle\">";
                RetVal += "<td class=\"HeaderColumnStyle\" style=\"width:2%\"></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Id\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_AgencyId\"></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Version\"></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Assosciated_Database\"></span></td>";
                RetVal += "<td class=\"HeaderColumnStyle\"><span  id=\"lang_Publisher_DSD\"></span></td>";
                RetVal += "</tr>";

                foreach (XmlNode DB in DBList)
                {
                    if (Convert.ToBoolean(DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value) == true)
                    {
                        DbNId = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value;
                        DSDId = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDId].Value;
                        DSDAgencyId = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDAgencyId].Value;
                        DSDVersion = DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.DSDVersion].Value;
                        AssociatedDB = this.Get_AssociatedDB_Name(DBList, DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value);
                        Publisher = Global.Get_AgencyId_From_DFD(DbNId);

                        RetVal += "<tr class=\"DataRowStyle\">";

                        RetVal += "<td class=\"DataColumnStyle\">";

                        if (Publisher == DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId)
                        {
                            RetVal += "<input id=\"radio_" + DbNId + "\" type=\"radio\" name=\"db\" value=\"" + DbNId + "\" />";
                        }

                        RetVal += "</td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + DSDId + "</span></td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + DSDAgencyId + "</span></td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + DSDVersion + "</span></td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + AssociatedDB + "</span></td>";

                        RetVal += "<td class=\"DataColumnStyle\"><span>" + Publisher + "</span></td>";

                        RetVal += "</tr>";
                    }
                }
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
        }

        return RetVal;
    }

    public string DataProviderGetIndicatorDivInnerHTML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        string UserNId;
        DataProviderActions Action;

        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        UserNId = string.Empty;
        Action = DataProviderActions.None;

        try
        {
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            Action = (DataProviderActions)Enum.Parse(typeof(DataProviderActions), Params[2].ToString().Trim());

            switch (Action)
            {
                case DataProviderActions.GenerateSDMXML:
                    RetVal = this.GetIndicatorDivInnerHTML_For_GenerateSDMXML(DBOrDSDDBId, DBOrDSDFlag);
                    break;
                case DataProviderActions.RegisterSDMXML:
                    RetVal = this.GetIndicatorDivInnerHTML_For_RegisterSDMXML(DBOrDSDDBId, DBOrDSDFlag);
                    break;
                case DataProviderActions.GenerateMetadata:
                    RetVal = this.GetIndicatorDivInnerHTML_For_GenerateMetadata(DBOrDSDDBId, DBOrDSDFlag);
                    break;
                case DataProviderActions.RegisterMetadata:
                    RetVal = this.GetIndicatorDivInnerHTML_For_RegisterMetadata(DBOrDSDDBId, DBOrDSDFlag);
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

        return RetVal;
    }

    public string DataProviderGetAreaDivInnerHTML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        DataProviderActions Action;
        string IndicatorNIds;
        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        Action = DataProviderActions.None;
        IndicatorNIds = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            Action = (DataProviderActions)Enum.Parse(typeof(DataProviderActions), Params[2].ToString().Trim());
            if (Params.Length > 3)
            {
                IndicatorNIds = Params[3].ToString().Trim();
            }
            switch (Action)
            {
                case DataProviderActions.GenerateSDMXML:
                    RetVal = this.GetAreaDivInnerHTML_For_GenerateSDMXML(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);
                    break;
                case DataProviderActions.GenerateMetadata:
                    RetVal = this.GetAreaDivInnerHTML_For_GenerateMetadata(DBOrDSDDBId);
                    break;
                case DataProviderActions.RegisterMetadata:
                    RetVal = this.GetAreaDivInnerHTML_For_RegisterMetadata(DBOrDSDDBId);
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

        return RetVal;
    }

    public string DataProviderGetTPDivInnerHTML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtTimePeriod;

        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtTimePeriod = null;
        string IndicatorNId = string.Empty;
        ArrayList ALItemAdded = new ArrayList();
        string xml = string.Empty;
        try
        {
            RetVal = string.Empty;

            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            if (Params.Length > 2)
            {
                IndicatorNId = Params[2].ToString().Trim();
            }
            if (DBOrDSDFlag == true)
            {
                DBNId = Convert.ToInt32(DBOrDSDDBId);
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
            }

            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\sdmx" + "\\DataPublishedUserSelection.xml");
            if (File.Exists(xml) && string.IsNullOrEmpty(IndicatorNId) == false)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xml);

                string timeperiods = string.Empty;
                foreach (XmlElement element in xmlDoc.SelectNodes("/root/Data"))
                {
                    if (element.GetAttribute("Ind") == IndicatorNId)
                    {
                        timeperiods = element.GetAttribute("timeperiods");

                    }
                }
                if (timeperiods.Contains(",") && string.IsNullOrEmpty(timeperiods) == false)
                {
                    foreach (string item in timeperiods.Split(','))
                    {
                        ALItemAdded.Add(item);
                    }
                }
                else
                {
                    ALItemAdded.Add(timeperiods);
                }
            }

            RetVal = "<table id=\"tblTP\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkTP_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllTPs();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanTP_0\"></i>";
            RetVal += "</td>";

            RetVal += "</tr>";
            if (string.IsNullOrEmpty(IndicatorNId))
            {
                DtTimePeriod = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
            }
            else
            {
                DtTimePeriod = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetAutoSelectTimeperiod(IndicatorNId, false, string.Empty, string.Empty));
            }
            foreach (DataRow DrTimePeriod in DtTimePeriod.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                if (ALItemAdded.Count > 0)
                {
                    if (ALItemAdded.Contains(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString()))
                    {
                        RetVal += "<input checked=\"checked\" id=\"chkTP_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\" type=\"checkbox\" value=\"" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\"/>";
                    }
                    else
                    {
                        RetVal += "<input id=\"chkTP_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\" type=\"checkbox\" value=\"" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\"/>";
                    }
                }
                else
                {
                    RetVal += "<input id=\"chkTP_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\" type=\"checkbox\" value=\"" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\"/>";
                }
                RetVal += "</td>";

                RetVal += "<td style=\"width:98%\">";
                RetVal += "<span id=\"spanTP_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\">" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString() + "</span>";
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

        return RetVal;
    }

    public string DataProviderGetSourceDivInnerHTML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        string IndicatorNIds;
        DataProviderActions Action;

        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        Action = DataProviderActions.None;
        IndicatorNIds = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            Action = (DataProviderActions)Enum.Parse(typeof(DataProviderActions), Params[2].ToString().Trim());

            if (Params.Length > 3)
            {
                IndicatorNIds = Params[3].ToString().Trim();
            }
            switch (Action)
            {
                case DataProviderActions.GenerateSDMXML:
                    RetVal = this.GetSourceDivInnerHTML_For_GenerateSDMXML(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);
                    break;
                case DataProviderActions.GenerateMetadata:
                    RetVal = this.GetSourceDivInnerHTML_For_GenerateMetadata(DBOrDSDDBId);
                    break;
                case DataProviderActions.RegisterMetadata:
                    RetVal = this.GetSourceDivInnerHTML_For_RegisterMetadata(DBOrDSDDBId);
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

        return RetVal;
    }

    //public string DataProviderGenerateSDMXML(string requestParam)
    //{
    //    string RetVal;
    //    string[] Params;
    //    string DBOrDSDDBId;
    //    bool DBOrDSDFlag;
    //    string IndicatorNIds, AreaNIds, TPNIds, SourceNIds, AgencyId, OutputFolder, AreaId, UserNId, HeaderFilePath, OriginalDBNId, DuplicateKey;
    //    int DBNId;
    //    int fileCount;
    //    string[] output;
    //    string[] SelectedCollection;
    //    DIConnection DIConnection;
    //    DIQueries DIQueries;
    //    Dictionary<string, string> DictQuery;
    //    Dictionary<string, string> DictMapping;
    //    Dictionary<string, string> DBConnections;
    //    XmlDocument Query;
    //    List<string> GeneratedFiles;
    //    List<string> ListIndicatorForRegistrations;
    //    string[] currentItem = null;
    //    string[] selectedField = null;
    //    string selectedType = string.Empty;
    //    string selectedValue = string.Empty;
    //    string selectedIndicator = string.Empty;
    //    RetVal = "false";
    //    fileCount = 0;
    //    Params = null;
    //    DBOrDSDDBId = string.Empty;
    //    DBOrDSDFlag = false;
    //    IndicatorNIds = string.Empty;
    //    AreaNIds = string.Empty;
    //    TPNIds = string.Empty;
    //    SourceNIds = string.Empty;
    //    AgencyId = string.Empty;
    //    OutputFolder = string.Empty;
    //    UserNId = string.Empty;
    //    DBNId = -1;
    //    DIConnection = null;
    //    DIQueries = null;
    //    DictQuery = new Dictionary<string, string>();
    //    DictMapping = new Dictionary<string, string>();
    //    Query = null;
    //    GeneratedFiles = new List<string>();// Get collection of files 
    //    ListIndicatorForRegistrations = new List<string>();
    //    HeaderFilePath = string.Empty;
    //    OriginalDBNId = string.Empty;
    //    DBConnections = new Dictionary<string, string>();
    //    AreaId = string.Empty;
    //    output = null;
    //    DuplicateKey = string.Empty;
    //    SelectedCollection = null;
    //    DataTable dtSelections = null;
    //    DataRow dr;
    //    DataRow[] drSelections = null;
    //    string[] AgencyUserNid = null;
    //    string DevInfoOrCountryData = string.Empty;
    //    string xml = string.Empty;
    //    try
    //    {
    //        Params = Global.SplitString(requestParam, "|");
    //        DBOrDSDDBId = Params[0].ToString().Trim();
    //        DevInfoOrCountryData = Params[1].ToString().Trim();
    //        AgencyUserNid = Session["hLoggedInUserNId"].ToString().Trim().Split(new string[] { Constants.Delimiters.PivotColumnDelimiter }, StringSplitOptions.None);
    //        AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AgencyUserNid[0].ToString();
    //        UserNId = AgencyUserNid[0].ToString().Trim();
    //        if (Params.Length > 2)
    //        {
    //            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
    //            IndicatorNIds = Params[2].ToString().Trim();
    //            AreaNIds = Params[3].ToString().Trim();
    //            TPNIds = Params[4].ToString().Trim();
    //            SourceNIds = Params[5].ToString().Trim();
    //            AreaId = Params[6].ToString().Trim();
    //            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + Params[7].ToString().Trim();
    //            UserNId = Params[7].ToString().Trim();
    //            OriginalDBNId = Params[8].ToString().Trim();
    //            if (Params.Length > 9)
    //            {
    //                SelectedCollection = Params[9].ToString().Trim().Split(new string[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None);
    //            }

    //        }
    //        else
    //        {
    //            Dictionary<string, string> DictConnections = new Dictionary<string, string>();
    //            DBConnections = Global.GetAllConnections("DevInfo");
    //            foreach (var item in DBConnections.Keys)
    //            {
    //                if (Convert.ToString(item) == DBOrDSDDBId && DevInfoOrCountryData == "DevInfo")
    //                {
    //                    RetVal = PublishDataFilesForDB(true, DBOrDSDDBId, AgencyId, UserNId);
    //                    output = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
    //                    RetVal = output[0].ToString();
    //                }
    //                else if (Convert.ToString(item) != DBOrDSDDBId && DevInfoOrCountryData == "CountryData")
    //                {
    //                    RetVal = PublishDataFilesForDSD(false, Convert.ToString(item), AgencyId, UserNId);
    //                    output = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
    //                    RetVal = output[0].ToString();
    //                }

    //            }

    //        }



    //        if (Params.Length > 2)
    //        {
    //            HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
    //            if (DBOrDSDFlag == true)
    //            {
    //                DBNId = Convert.ToInt32(DBOrDSDDBId);
    //                DIConnection = Global.GetDbConnection(DBNId);
    //                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
    //                XmlDocument UploadedHeaderXml = new XmlDocument();
    //                SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
    //                SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
    //                if (File.Exists(HeaderFilePath))
    //                {
    //                    UploadedHeaderXml.Load(HeaderFilePath);
    //                    UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
    //                    Header = UploadedDSDStructure.Header;
    //                }
    //                this.Clean_SDMX_ML_Folder(DBNId);

    //                this.Add_IUS_To_Dictionary(DictQuery, IndicatorNIds, DIConnection, DIQueries);
    //                this.Add_Area_To_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);
    //                this.Add_TP_To_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
    //                this.Add_Source_To_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

    //                Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, AgencyId, DIConnection, DIQueries);
    //                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
    //                if (SDMXUtility.Generate_Data(SDMXSchemaType.Two_One, Query, DataFormats.StructureSpecificTS, DIConnection, DIQueries, OutputFolder, out fileCount, out GeneratedFiles, Header) == true)
    //                {
    //                    if (fileCount == 0)
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "NDF";
    //                    }
    //                    else
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
    //                        ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

    //                        this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, ListIndicatorForRegistrations, GeneratedFiles, DBNId.ToString());
    //                    }
    //                }
    //                else
    //                {
    //                    RetVal = "false";
    //                }
    //            }
    //            else
    //            {
    //                if (SelectedCollection != null)
    //                {
    //                    dtSelections = new DataTable();
    //                    dtSelections.TableName = "Data";
    //                    dtSelections.Columns.Add("Ind", typeof(int));
    //                    dtSelections.Columns.Add("areas", typeof(string));
    //                    dtSelections.Columns.Add("timeperiods", typeof(string));
    //                    dtSelections.Columns.Add("source", typeof(string));
    //                }
    //                else
    //                {
    //                    dtSelections = new DataTable();
    //                    dtSelections.TableName = "Data";
    //                    dtSelections.Columns.Add("Ind", typeof(int));
    //                    dtSelections.Columns.Add("areas", typeof(string));
    //                    dtSelections.Columns.Add("timeperiods", typeof(string));
    //                    dtSelections.Columns.Add("source", typeof(string));

    //                    foreach (string IndicatorNId in IndicatorNIds.Split(','))
    //                    {
    //                        dr = dtSelections.NewRow();
    //                        dr["Ind"] = IndicatorNId;

    //                        dr["areas"] = string.Empty;

    //                        dr["timeperiods"] = string.Empty;

    //                        dr["source"] = string.Empty;

    //                        dtSelections.Rows.Add(dr);
    //                    }

    //                }


    //                bool rowFound = false;
    //                currentItem = null;
    //                SelectedCollection = this.AddItemsInSelectedCollection(SelectedCollection, IndicatorNIds);
    //                if (SelectedCollection != null)

    //                    foreach (string item in SelectedCollection)
    //                    {
    //                        dr = dtSelections.NewRow();
    //                        currentItem = item.Split(new string[] { Constants.Delimiters.ColumnDelimiter }, StringSplitOptions.None);
    //                        foreach (string field in currentItem)
    //                        {
    //                            if (string.IsNullOrEmpty(field) == false)
    //                            {
    //                                selectedField = field.Split(new string[] { Constants.Delimiters.Underscore }, StringSplitOptions.None);
    //                                selectedType = selectedField[0];
    //                                selectedValue = selectedField[1];
    //                                if (selectedType == "Indicator")
    //                                {
    //                                    selectedIndicator = selectedValue;
    //                                }
    //                                if (dtSelections.Rows.Count > 0)
    //                                {
    //                                    foreach (DataRow datarow in dtSelections.Rows)
    //                                    {
    //                                        if (datarow[0].ToString() == selectedIndicator)
    //                                        {
    //                                            switch (selectedType)
    //                                            {
    //                                                case "Indicator":
    //                                                    datarow["Ind"] = selectedValue;
    //                                                    rowFound = true;
    //                                                    break;
    //                                                case "Area":
    //                                                    datarow["areas"] = selectedValue;
    //                                                    rowFound = true;
    //                                                    break;
    //                                                case "Time":
    //                                                    datarow["timeperiods"] = selectedValue;
    //                                                    rowFound = true;
    //                                                    break;
    //                                                case "Source":
    //                                                    datarow["source"] = selectedValue;
    //                                                    rowFound = true;
    //                                                    break;
    //                                                default:
    //                                                    break;

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            switch (selectedType)
    //                                            {
    //                                                case "Indicator":
    //                                                    dr["Ind"] = selectedValue;
    //                                                    break;
    //                                                case "Area":
    //                                                    dr["areas"] = selectedValue;
    //                                                    break;
    //                                                case "Time":
    //                                                    dr["timeperiods"] = selectedValue;
    //                                                    break;
    //                                                case "Source":
    //                                                    dr["source"] = selectedValue;
    //                                                    break;
    //                                                default:
    //                                                    break;
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    switch (selectedType)
    //                                    {
    //                                        case "Indicator":
    //                                            dr["Ind"] = selectedValue;
    //                                            break;
    //                                        case "Area":
    //                                            dr["areas"] = selectedValue;
    //                                            break;
    //                                        case "Time":
    //                                            dr["timeperiods"] = selectedValue;
    //                                            break;
    //                                        case "Source":
    //                                            dr["source"] = selectedValue;
    //                                            break;
    //                                        default:
    //                                            break;
    //                                    }
    //                                }

    //                            }
    //                        }
    //                        if (rowFound == false)
    //                            dtSelections.Rows.Add(dr);
    //                        for (int i = 0; i < dtSelections.Rows.Count; i++)
    //                        {
    //                            if (dtSelections.Rows[i].IsNull("Ind") == true)
    //                            {
    //                                dtSelections.Rows.RemoveAt(i);
    //                            }
    //                        }
    //                    }



    //                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
    //                DIConnection = Global.GetDbConnection(DBNId);
    //                DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
    //                DictMapping = this.Get_IUS_Mapping_Dict(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\" + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml"));

    //                this.Clean_SDMX_ML_Folder_For_UNSD(DBNId, DBOrDSDDBId);
    //                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
    //                xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\sdmx" + "\\DataPublishedUserSelection.xml");
    //                if (RegTwoZeroData.Generate_Data(IndicatorNIds, AreaNIds, TPNIds, SourceNIds, DictMapping, OutputFolder, DIConnection, DIQueries, out fileCount, AreaId, DBOrDSDDBId, out GeneratedFiles, HeaderFilePath, xml, out DuplicateKey, dtSelections) == true)
    //                {
    //                    //MNF-Mapping Not found in case if IUS Mapping doesnot exist.
    //                    //NDF-No data Found if datavalues does not exist corresponding to the indicator(s) selected.
    //                    if (DictMapping.Count == 0)
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + "0" + Constants.Delimiters.ParamDelimiter + "MNF";
    //                    }
    //                    else if (fileCount == 0 && DictMapping.Count == 0)
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";
    //                    }
    //                    else if (fileCount == 0 && DictMapping.Count > 0 && string.IsNullOrEmpty(DuplicateKey) == false)
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "DK" + Constants.Delimiters.ParamDelimiter + DuplicateKey;
    //                    }
    //                    else if (fileCount == 0 && DictMapping.Count > 0)
    //                    {
    //                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "NDF";
    //                    }
    //                    else
    //                    {
    //                        if (string.IsNullOrEmpty(DuplicateKey) == false)
    //                        {
    //                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + DuplicateKey + Constants.Delimiters.ParamDelimiter + "DK";
    //                        }
    //                        else
    //                        {
    //                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
    //                        }
    //                        Session["GeneratedFiles"] = (List<string>)GeneratedFiles;

    //                        ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

    //                        this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, ListIndicatorForRegistrations, GeneratedFiles, DBNId.ToString());
    //                    }
    //                }
    //                else
    //                {
    //                    RetVal = "false";
    //                    if (DictMapping.Count == 0)
    //                    {
    //                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";

    //                    }
    //                }

    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.Message.Contains("The given key was not present in the dictionary"))
    //        {
    //            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "MNF";

    //        }
    //        else
    //        {
    //            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
    //        }

    //        Global.CreateExceptionString(ex, null);

    //    }
    //    finally
    //    {

    //    }

    //    return RetVal;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestParam">Selected Indicator[] Area []</param>
    /// <returns>FileCount and Status of Generated or Not generated files</returns>
    public string DataProviderGenerateSDMXML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        string IndicatorNIds, AreaNIds, TPNIds, SourceNIds, AgencyId, OutputFolder, AreaId, UserNId, HeaderFilePath, OriginalDBNId, DuplicateKey;
        int DBNId;
        int fileCount;
        string[] output;
        string[] SelectedCollection;
        DIConnection DIConnection;
        DIQueries DIQueries;
        Dictionary<string, string> DictQuery;
        Dictionary<string, string> DictMapping;
        Dictionary<string, string> DBConnections;
        XmlDocument Query;
        List<string> GeneratedFiles;
       // List<string> ListIndicatorForRegistrations;
        RetVal = "false";
        fileCount = 0;
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        AgencyId = string.Empty;
        OutputFolder = string.Empty;
        UserNId = string.Empty;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DictQuery = new Dictionary<string, string>();
        DictMapping = new Dictionary<string, string>();
        Query = null;
        GeneratedFiles = new List<string>();// Get collection of files 
     //   ListIndicatorForRegistrations = new List<string>();
        HeaderFilePath = string.Empty;
        OriginalDBNId = string.Empty;
        DBConnections = new Dictionary<string, string>();
        AreaId = string.Empty;
        output = null;
        DuplicateKey = string.Empty;
        SelectedCollection = null;
        DataTable dtSelections = null;
        DataRow dr;

        string[] AgencyUserNid = null;
        string DevInfoOrCountryData = string.Empty;
        string xml = string.Empty;
        string DataPublishUserSelectionFileNameWPath = string.Empty;
        string IndNId = string.Empty;
        string areas = string.Empty;
        string timeperiods = string.Empty;
        string source = string.Empty;
        string selectedState = string.Empty;
        string ErrorLogs = string.Empty;
        try
        {
            // 1. Split the parameter and set the variable
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DevInfoOrCountryData = Params[1].ToString().Trim();
            AgencyUserNid = Session["hLoggedInUserNId"].ToString().Trim().Split(new string[] { Constants.Delimiters.PivotColumnDelimiter }, StringSplitOptions.None);
            AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + AgencyUserNid[0].ToString();
            UserNId = AgencyUserNid[0].ToString().Trim();

            if (Params.Length > 2)
            {
                // 1A. If reaching through Register tab ...
                DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
                IndicatorNIds = Params[2].ToString().Trim();
                AreaNIds = Params[3].ToString().Trim();
                TPNIds = Params[4].ToString().Trim();
                SourceNIds = Params[5].ToString().Trim();
                AreaId = Params[6].ToString().Trim();
                AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + Params[7].ToString().Trim();
                UserNId = Params[7].ToString().Trim();
                OriginalDBNId = Params[8].ToString().Trim();
                if (Params.Length > 9)
                {
                    SelectedCollection = Params[9].ToString().Trim().Split(new string[] { Constants.Delimiters.RowDelimiter }, StringSplitOptions.None);
                }
                HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
                if (DBOrDSDFlag == true)
                {
                    //  1Ai If DevInfo DSD
                    DBNId = Convert.ToInt32(DBOrDSDDBId);
                    DIConnection = Global.GetDbConnection(DBNId);
                    DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
                    XmlDocument UploadedHeaderXml = new XmlDocument();
                    SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
                    SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
                    if (File.Exists(HeaderFilePath))
                    {
                        UploadedHeaderXml.Load(HeaderFilePath);
                        UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                        Header = UploadedDSDStructure.Header;
                    }
                    this.Clean_SDMX_ML_Folder(DBNId);

                    this.Add_IUS_To_Dictionary(DictQuery, IndicatorNIds, DIConnection, DIQueries);
                    this.Add_Area_To_Dictionary(DictQuery, AreaNIds, DIConnection, DIQueries);
                    this.Add_TP_To_Dictionary(DictQuery, TPNIds, DIConnection, DIQueries);
                    this.Add_Source_To_Dictionary(DictQuery, SourceNIds, DIConnection, DIQueries);

                    Query = SDMXUtility.Get_Query(SDMXSchemaType.Two_One, DictQuery, QueryFormats.StructureSpecificTS, DataReturnDetailTypes.Full, AgencyId, DIConnection, DIQueries);
                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
                    if (SDMXUtility.Generate_Data(SDMXSchemaType.Two_One, Query, DataFormats.StructureSpecificTS, DIConnection, DIQueries, OutputFolder, out fileCount, out GeneratedFiles, Header) == true)
                    {
                        if (fileCount == 0)
                        {
                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "NDF";
                        }
                        else
                        {
                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
                            //ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

                            this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, GeneratedFiles, DBNId.ToString());
                        }
                    }
                    else
                    {
                        RetVal = "false";
                    }
                }
                else
                {
                    // 1Aii If Country DSD

                    // Set Datatable to store user selction (Indicator -> Area - Time Period, Source)
                    dtSelections = new DataTable();
                    dtSelections.TableName = "Data";
                    dtSelections.Columns.Add("Ind", typeof(int));
                    dtSelections.Columns.Add("areas", typeof(string));
                    dtSelections.Columns.Add("timeperiods", typeof(string));
                    dtSelections.Columns.Add("source", typeof(string));
                    dtSelections.Columns.Add("selectedState", typeof(string));

                    foreach (string IndicatorNId in IndicatorNIds.Split(','))
                    {
                        dr = dtSelections.NewRow();
                        dr["Ind"] = IndicatorNId;


                        DataPublishUserSelectionFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\sdmx" + "\\DataPublishedUserSelection.xml");
                        XmlDocument doc = new XmlDocument();
                        doc.Load(DataPublishUserSelectionFileNameWPath);
                        foreach (XmlElement element in doc.SelectNodes("/root/Data"))
                        {
                            IndNId = element.GetAttribute("Ind");
                            if (IndicatorNId == IndNId)
                            {

                                areas = element.GetAttribute("areas");
                                dr["areas"] = areas;

                                timeperiods = element.GetAttribute("timeperiods");
                                dr["timeperiods"] = timeperiods;

                                source = element.GetAttribute("source");

                                dr["source"] = source;

                                selectedState = element.GetAttribute("selectedState");

                                dr["selectedState"] = "true";
                            }

                        }

                        dtSelections.Rows.Add(dr);

                    }


                    DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
                    DIConnection = Global.GetDbConnection(DBNId);
                    DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));
                    DictMapping = this.Get_IUS_Mapping_Dict(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\" + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml"));

                    this.Clean_SDMX_ML_Folder_For_UNSD(DBNId, DBOrDSDDBId);
                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.SDMX_ML);
                    xml = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId + "\\sdmx" + "\\DataPublishedUserSelection.xml");

                    // Generate SDMX-mL data files
                    if (RegTwoZeroData.Generate_Data(DictMapping, OutputFolder, DIConnection, DIQueries, AreaId, DBOrDSDDBId, HeaderFilePath, xml, dtSelections, out fileCount, out GeneratedFiles, out ErrorLogs, out DuplicateKey) == true)
                    {

                        // Build Error Messages
                        if (DictMapping.Count == 0)
                        {
                            //MNF-Mapping Not found in case if IUS Mapping doesnot exist.
                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + "0" + Constants.Delimiters.ParamDelimiter + "MNF";
                        }
                        else if (fileCount == 0 && DictMapping.Count == 0)
                        {
                            //MNF-Mapping Not found in case if IUS Mapping doesnot exist.
                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";
                        }
                        else if (fileCount == 0 && DictMapping.Count > 0 && string.IsNullOrEmpty(DuplicateKey) == false)
                        {
                            // Duplicate Key
                            RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + DuplicateKey;
                            if (RetVal.EndsWith(Constants.Delimiters.ParamDelimiter))
                            {
                                RetVal = RetVal.Substring(0, RetVal.Length - 6);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(DuplicateKey) == false)
                            {
                                RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + DuplicateKey;
                            }
                            else
                            {
                                RetVal = "true" + Constants.Delimiters.ParamDelimiter + fileCount.ToString();
                            }
                            //ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

                            // Register SDMX-ML file generated
                          
                            this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, GeneratedFiles, DBNId.ToString());
                        }
                        RetVal = RetVal + Constants.Delimiters.ParamDelimiter + ErrorLogs;
                    }
                    else
                    {
                        RetVal = "false";
                        if (DictMapping.Count == 0)
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + fileCount.ToString() + Constants.Delimiters.ParamDelimiter + "MNF";
                        }
                    }
                }

            }
            else
            {
                // 1B. If reaching through Admin Optimize 
                Dictionary<string, string> DictConnections = new Dictionary<string, string>();
                DBConnections = Global.GetAllConnections("DevInfo");
                foreach (var item in DBConnections.Keys)
                {
                    if (Convert.ToString(item) == DBOrDSDDBId && DevInfoOrCountryData == "DevInfo")
                    {
                        RetVal = PublishDataFilesForDB(true, DBOrDSDDBId, AgencyId, UserNId);
                        output = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                        RetVal = output[0].ToString();
                    }
                    else if (Convert.ToString(item) != DBOrDSDDBId && DevInfoOrCountryData == "CountryData")
                    {
                        RetVal = PublishDataFilesForDSD(false, Convert.ToString(item), AgencyId, UserNId);
                        output = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                        RetVal = output[0].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("The given key was not present in the dictionary"))
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + "MNF";

            }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            }

            Global.CreateExceptionString(ex, null);
           
        }
        finally
        {

        }

        return RetVal;
    }

    /// <summary>
    /// function is obselete
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string DataProviderRegisterSDMXML(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId, IndicatorNIds, UserNId;
        bool DBOrDSDFlag;
        List<string> ListIndicatorForRegistrations;
        List<string> FilesToBeRegistered;
        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        IndicatorNIds = string.Empty;
        UserNId = string.Empty;
        DBOrDSDFlag = false;
        ListIndicatorForRegistrations = new List<string>();
        FilesToBeRegistered = new List<string>();
        try
        {
            if (Session["GeneratedFiles"] != null)
            {
                FilesToBeRegistered = (List<string>)Session["GeneratedFiles"];
            }
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());

            UserNId = Params[2].ToString().Trim();
            this.Register_SDMXFiles(DBOrDSDDBId, DBOrDSDFlag, UserNId, FilesToBeRegistered, null);
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

    public string DataProviderGenerateMetadata(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId, IndicatorNIds, AreaNIds, SourceNIds, UserNId, ErrorMessage, HeaderFilePath;
        bool DBOrDSDFlag;
        int DBNId;
        string OriginalDBNId;
        List<string> GeneratedMetadataFiles, GeneratedIndicatorMetadataFiles, GeneratedAreaMetadataFiles, GeneratedSourceMetadataFiles, ListIndicatorForRegistrations, ListAreaForRegistrations, ListSourceForRegistrations;
        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        SourceNIds = string.Empty;
        UserNId = string.Empty;
        ErrorMessage = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        GeneratedMetadataFiles = new List<string>();
        GeneratedIndicatorMetadataFiles = new List<string>();
        GeneratedAreaMetadataFiles = new List<string>();
        GeneratedSourceMetadataFiles = new List<string>();
        ListIndicatorForRegistrations = new List<string>();
        ListAreaForRegistrations = new List<string>();
        ListSourceForRegistrations = new List<string>();
        HeaderFilePath = string.Empty;
        OriginalDBNId = string.Empty;
        RetVal = "false";
        try
        {
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            IndicatorNIds = Params[2].ToString().Trim();
            AreaNIds = Params[3].ToString().Trim();
            SourceNIds = Params[4].ToString().Trim();
            UserNId = Params[5].ToString().Trim();

            this.Add_Minus_One_To_Selections(ref IndicatorNIds, ref AreaNIds, ref SourceNIds);

            ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

            HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.sdmx + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
            if (DBOrDSDFlag == true)
            {
                XmlDocument UploadedHeaderXml = new XmlDocument();
                SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
                SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();

                if (File.Exists(HeaderFilePath))
                {

                    UploadedHeaderXml.Load(HeaderFilePath);
                    UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                    Header = UploadedDSDStructure.Header;
                }
                DBNId = Convert.ToInt32(DBOrDSDDBId);
                OriginalDBNId = DBNId.ToString();
                RetVal = this.GenerateMetadata(DBNId, UserNId, IndicatorNIds, AreaNIds, SourceNIds, out GeneratedIndicatorMetadataFiles, out GeneratedAreaMetadataFiles, out GeneratedSourceMetadataFiles, Header);
                ListAreaForRegistrations = this.Get_Area_List_For_Registrations(DBOrDSDDBId, AreaNIds);
                ListSourceForRegistrations = this.Get_Source_List_For_Registrations(DBOrDSDDBId, SourceNIds);
                RetVal = "true";
            }
            else
            {
                DBNId = this.Get_AssociatedDB_NId(DBOrDSDDBId);
                OriginalDBNId = DBNId.ToString();
                RetVal = this.GenerateMetadata_For_UNSD(DBNId, DBOrDSDDBId, IndicatorNIds, out ErrorMessage, out GeneratedMetadataFiles, HeaderFilePath);

            }

            this.Register_MetadataReport(DBOrDSDDBId, UserNId, ListIndicatorForRegistrations, ListAreaForRegistrations, ListSourceForRegistrations, GeneratedMetadataFiles, GeneratedIndicatorMetadataFiles, GeneratedAreaMetadataFiles, GeneratedSourceMetadataFiles, OriginalDBNId);
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
    
    //function is obselete 
    public string DataProviderRegisterMetadata(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBOrDSDDBId, IndicatorNIds, AreaNIds, SourceNIds, UserNId;
        bool DBOrDSDFlag;
        List<string> ListIndicatorForRegistrations, ListAreaForRegistrations, ListSourceForRegistrations;

        RetVal = string.Empty;
        Params = null;
        DBOrDSDDBId = string.Empty;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        SourceNIds = string.Empty;
        UserNId = string.Empty;
        DBOrDSDFlag = false;
        ListIndicatorForRegistrations = new List<string>();
        ListAreaForRegistrations = new List<string>();
        ListSourceForRegistrations = new List<string>();

        try
        {
            Params = Global.SplitString(requestParam, "|");
            DBOrDSDDBId = Params[0].ToString().Trim();
            DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
            UserNId = Params[2].ToString().Trim();

            ListIndicatorForRegistrations = this.Get_Indicator_List_For_Registrations(DBOrDSDDBId, DBOrDSDFlag, IndicatorNIds);

            if (DBOrDSDFlag == true)
            {
                ListAreaForRegistrations = this.Get_Area_List_For_Registrations(DBOrDSDDBId, AreaNIds);
                ListSourceForRegistrations = this.Get_Source_List_For_Registrations(DBOrDSDDBId, SourceNIds);
            }

            this.Register_MetadataReport(DBOrDSDDBId, UserNId, ListIndicatorForRegistrations, ListAreaForRegistrations, ListSourceForRegistrations, null, null, null, null, null);//code to add
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

    public string SaveSDMXDataSelection(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        string SelectedIndicatorNId, FilterNIds, AgencyId, OutputFolder, AreaId, UserNId, StrXML, FilterType, IndicatorNId;
        int DBNId;
        RetVal = "false";
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        SelectedIndicatorNId = string.Empty;
        FilterNIds = string.Empty;
        AgencyId = string.Empty;
        OutputFolder = string.Empty;
        UserNId = string.Empty;
        StrXML = string.Empty;
        FilterType = string.Empty;
        IndicatorNId = string.Empty;
        DBNId = -1;
        Params = Global.SplitString(requestParam, "|");
        DBOrDSDDBId = Params[0].ToString().Trim();
        string DataPublishUserSelectionFileNameWPath = string.Empty;
        string obj = string.Empty;
        try
        {

            if (Params.Length > 1)
            {
                DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
                SelectedIndicatorNId = Params[2].ToString().Trim();
                FilterNIds = Params[3].ToString().Trim();
                AreaId = Params[4].ToString().Trim();
                AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + Params[5].ToString().Trim();
                UserNId = Params[5].ToString().Trim();
                FilterType = Params[6].ToString().Trim();
                DBNId = Convert.ToInt32(DBOrDSDDBId);
                DataPublishUserSelectionFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId.ToString() + "\\sdmx" + "\\DataPublishedUserSelection.xml");
                XmlDocument doc = new XmlDocument();
                doc.Load(DataPublishUserSelectionFileNameWPath);
                string IndNId = string.Empty;
                ArrayList ALItemAdded = new ArrayList();
                XmlNode rootElement = doc.SelectSingleNode("/root");
                foreach (string IndNid in SelectedIndicatorNId.Split(','))
                {
                    if (!ALItemAdded.Contains(IndNid))
                    {
                        ALItemAdded.Add(IndNid);
                    }
                }


                foreach (XmlElement element in doc.SelectNodes("/root/Data"))
                {
                    if (FilterType == "Indicators")
                    {
                        IndicatorNId = element.GetAttribute("Ind");
                        if (ALItemAdded.Contains(IndicatorNId))
                        {
                            element.SetAttribute("selectedState", "true");
                        }
                        else
                        {
                            element.SetAttribute("selectedState", "false");
                        }
                    }
                    else
                    {
                        IndicatorNId = element.GetAttribute("Ind");
                        if (IndicatorNId == SelectedIndicatorNId)
                        {
                            if (FilterType == "Areas")
                            {
                                element.SetAttribute("areas", FilterNIds);
                            }
                            else if (FilterType == "TimePeriods")
                            {
                                element.SetAttribute("timeperiods", FilterNIds);
                            }
                            else if (FilterType == "Source")
                            {
                                element.SetAttribute("source", FilterNIds);
                            }

                        }
                    }
                }
                doc.Save(DataPublishUserSelectionFileNameWPath);

                RetVal = "true";
            }


        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, string.Empty);
        }
        return RetVal;
    }

    public string SaveSDMXMetadataSelection(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        string IndicatorNIds, AreaNIds, TPNIds, SourceNIds, AgencyId, OutputFolder, AreaId, UserNId, StrXML;
        int DBNId;
        RetVal = "false";
        Params = null;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        IndicatorNIds = string.Empty;
        AreaNIds = string.Empty;
        TPNIds = string.Empty;
        SourceNIds = string.Empty;
        AgencyId = string.Empty;
        OutputFolder = string.Empty;
        UserNId = string.Empty;
        StrXML = string.Empty;
        DBNId = -1;
        DIConnection = null;
        Params = Global.SplitString(requestParam, "|");
        DBOrDSDDBId = Params[0].ToString().Trim();
        string MetaDataPublishUserSelectionFileNameWPath = string.Empty;
        string obj = string.Empty;
        try
        {

            if (Params.Length > 1)
            {
                DBOrDSDFlag = Convert.ToBoolean(Params[1].ToString().Trim());
                IndicatorNIds = Params[2].ToString().Trim();
                AreaId = Params[3].ToString().Trim();
                AgencyId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + Params[4].ToString().Trim();
                UserNId = Params[4].ToString().Trim();
                obj = Params[5].ToString().Trim();
                DBNId = Convert.ToInt32(DBOrDSDDBId);
                XmlDocument doc = (XmlDocument)Newtonsoft.Json.JsonConvert.DeserializeXmlNode(obj);
                MetaDataPublishUserSelectionFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId.ToString() + "\\sdmx" + "\\MetadataPublishedUserSelection.xml");
                if (File.Exists(MetaDataPublishUserSelectionFileNameWPath) == false)
                {
                    System.IO.FileStream f = System.IO.File.Create(MetaDataPublishUserSelectionFileNameWPath);
                    f.Close();
                }
                else
                {
                    File.Delete(MetaDataPublishUserSelectionFileNameWPath);
                    System.IO.FileStream f = System.IO.File.Create(MetaDataPublishUserSelectionFileNameWPath);
                    f.Close();
                }
                using (FileStream fs = new FileStream(MetaDataPublishUserSelectionFileNameWPath, FileMode.Open))
                {
                    XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8);
                    writer.Formatting = Formatting.Indented;
                    doc.Save(writer);

                    fs.Close();
                }
                RetVal = "true";
            }


        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, string.Empty);
        }
        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
