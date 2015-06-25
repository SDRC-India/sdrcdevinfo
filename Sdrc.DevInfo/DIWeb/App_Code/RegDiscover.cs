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

public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    public string PopulateDFDMFD(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBNId;
        string Language;
        bool DFDOrMFDFlag;
        DIConnection DIConnection;
        DataTable DtTable;
        string FIleNameWPath;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = string.Empty;
        Params = null;
        DBNId = string.Empty;
        Language = string.Empty;
        DFDOrMFDFlag = false;
        DIConnection = null;
        DtTable = null;
        FIleNameWPath = string.Empty;
        Structure = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            DFDOrMFDFlag = Convert.ToBoolean(Params[2].ToString().Trim());

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                      string.Empty, string.Empty);

            if (DFDOrMFDFlag == true)
            {
                DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Type=" + Convert.ToInt32(ArtefactTypes.DFD).ToString() + ";");

                foreach (DataRow Drtable in DtTable.Rows)
                {
                    FIleNameWPath = Drtable["FileLocation"].ToString();
                    Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FIleNameWPath);

                    RetVal += Drtable["Id"].ToString() + Constants.Delimiters.PivotColumnDelimiter + 
                              this.GetLangSpecificValue_For_Version_2_1(Structure.Structures.Dataflows[0].Name, Language) + Constants.Delimiters.PivotRowDelimiter;
                }
            }
            else
            {
                DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Type=" + Convert.ToInt32(ArtefactTypes.MFD).ToString() + ";");

                foreach (DataRow Drtable in DtTable.Rows)
                {
                    FIleNameWPath = Drtable["FileLocation"].ToString();
                    Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FIleNameWPath);

                    RetVal += Drtable["Id"].ToString() + Constants.Delimiters.PivotColumnDelimiter +
                              this.GetLangSpecificValue_For_Version_2_1(Structure.Structures.Metadataflows[0].Name, Language) + Constants.Delimiters.PivotRowDelimiter;
                }
            }

            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Substring(0, RetVal.Length - 1);
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    public string DiscoverGetTPDivInnerHTML(string requestParam)
    {
        string RetVal;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtTimePeriod;

        RetVal = string.Empty;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtTimePeriod = null;

        try
        {
            DBOrDSDDBId = requestParam.ToString().Trim();
            DBOrDSDFlag = !Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBOrDSDDBId));

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

            RetVal = "<table id=\"tblTP\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkTP_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllTPs();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanTP_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            DtTimePeriod = DIConnection.ExecuteDataTable(DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
            foreach (DataRow DrTimePeriod in DtTimePeriod.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                RetVal += "<input id=\"chkTP_" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\" type=\"checkbox\" value=\"" + DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString() + "\"/>";
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
        }

        return RetVal;
    }

    public string DiscoverGetIndicatorDivInnerHTML(string requestParam)
    {
        string RetVal;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtIndicator;
        string CompleteFileWPath;
        Dictionary<string, string> DictIndicators;

        RetVal = string.Empty;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtIndicator = null;
        CompleteFileWPath = string.Empty;
        DictIndicators = null;

        try
        {
            DBOrDSDDBId = requestParam.ToString().Trim();
            DBOrDSDFlag = !Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBOrDSDDBId));

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

            RetVal = "<table id=\"tblIndicator\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkIndicator_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllIndicators();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanIndicator_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            DtIndicator = DIConnection.ExecuteDataTable(DIQueries.Indicators.GetAllIndicatorByIC(ICType.Convention, string.Empty, FieldSelection.Light));
            DtIndicator.DefaultView.Sort = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName;
            DtIndicator = DtIndicator.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId,
                          DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName);

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
                CompleteFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\sdmx\\Complete.xml");
                DictIndicators = RegTwoZeroFunctionality.Get_Indicator_GIds(CompleteFileWPath);

                foreach (DataRow DrIndicator in DtIndicator.Rows)
                {
                    if (DictIndicators.ContainsKey(DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()))
                    {
                        RetVal += "<tr>";

                        RetVal += "<td style=\"width:2%\">";
                        RetVal += "<input id=\"chkIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\" type=\"checkbox\" value=\"" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\"/>";
                        RetVal += "</td>";

                        RetVal += "<td style=\"width:98%\">";
                        RetVal += "<span id=\"spanIndicator_" + DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString() + "\">" + DictIndicators[DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString()].ToString() + "</span>";
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
        }

        return RetVal;
    }

    public string DiscoverGetAreaDivInnerHTML(string requestParam)
    {
        string RetVal;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtArea;

        RetVal = string.Empty;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtArea = null;

        try
        {
            DBOrDSDDBId = requestParam.ToString().Trim();
            DBOrDSDFlag = !Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBOrDSDDBId));

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

            RetVal = "<table id=\"tblArea\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkArea_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllAreas();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanArea_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            Global.GetAppSetting();

            DtArea = DIConnection.ExecuteDataTable(DIQueries.Area.GetArea(FilterFieldType.None, string.Empty));
            DtArea.DefaultView.Sort = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName;
            DtArea = DtArea.DefaultView.ToTable();


            foreach (DataRow DrArea in DtArea.Rows)
            {
                RetVal += "<tr>";

                RetVal += "<td style=\"width:2%\">";
                RetVal += "<input id=\"chkArea_" + DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\" type=\"checkbox\" value=\"" + DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\"/>";
                RetVal += "</td>";

                RetVal += "<td style=\"width:98%\">";
                RetVal += "<span id=\"spanArea_" + DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString() + "\">" + DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName].ToString() + "</span>";
                RetVal += "</td>";

                RetVal += "</tr>";
            }

            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }

        return RetVal;
    }

    public string DiscoverGetSourceDivInnerHTML(string requestParam)
    {
        string RetVal;
        string DBOrDSDDBId;
        bool DBOrDSDFlag;
        int DBNId;
        DIConnection DIConnection;
        DIQueries DIQueries;
        DataTable DtSource;

        RetVal = string.Empty;
        DBOrDSDDBId = string.Empty;
        DBOrDSDFlag = false;
        DBNId = -1;
        DIConnection = null;
        DIQueries = null;
        DtSource = null;

        try
        {
            DBOrDSDDBId = requestParam.ToString().Trim();
            DBOrDSDFlag = !Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBOrDSDDBId));

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

            RetVal = "<table id=\"tblSource\" style=\"width:100%\">";

            RetVal += "<tr>";

            RetVal += "<td style=\"width:2%\">";
            RetVal += "<input id=\"chkSource_0\" type=\"checkbox\" value=\"0\" onclick=\"SelectUnselectAllSources();\"/>";
            RetVal += "</td>";

            RetVal += "<td style=\"width:98%\">";
            RetVal += "<i id=\"spanSource_0\">Select All</i>";
            RetVal += "</td>";

            RetVal += "</tr>";

            Global.GetAppSetting();

            DtSource = DIConnection.ExecuteDataTable(DIQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));
            DtSource.DefaultView.Sort = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName;
            DtSource = DtSource.DefaultView.ToTable();

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
        }

        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
