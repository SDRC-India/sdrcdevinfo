using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Web.Script.Serialization;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


public partial class Callback : System.Web.UI.Page
{
    int tmpDBNId = 0;
    string Dimensions;
    string header;

    Hashtable Areas;
    Hashtable Indicators;
    Hashtable Units;
    Hashtable Subgroup_Vals;
    Hashtable Sources;
    Hashtable allSG = new Hashtable();

    public string GetLangDBcode(string requestParam)
    {
        try
        {
            string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            string UILanguage, hlngcodedb = string.Empty;
            int dbnid = Int32.Parse(Params[0]);
            //hlngcodedb = UILanguage = Params[1];
            UILanguage = Params[1];
            hlngcodedb = Global.GetDefaultLanguageCodeDB(dbnid.ToString(), UILanguage);
            return hlngcodedb;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return "false";
        }
    }

    /// <summary>
    /// ?callback=3&param1=QS_ASI_L2,QS_ASI_L3[****]1764[****]103[****]isMRD:1||dvStart:46||dvEnd:484
    /// requestParam = "[****]16,78,155,314,112,30[****]201[****]isMRD:1||IUS_NId:16||dvStart:0||dvEnd:50";
    /// requestParam = "30[****]199,200,213[****]201";
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string GetDataView(string requestParam)
    {
        string Result = string.Empty;
        string UILanguage = string.Empty;
        StringBuilder sbResult = new StringBuilder();

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        //Params after DBNID in this order : Source_NId, TimePeriod_NId, isMRD, dvStart, dvEnd
        string AreaNids = Params[0];
        string IUSNids = Params[1];

        int DbNid = int.Parse(Params[2]);
        UILanguage = Params[3];
        string AreaO = Params[4];
        string IndO = Params[5];
        Session["hselindo"] = IndO;
        Session["hselareao"] = AreaO;
        Session["hselind"] = IUSNids;
        Session["hselarea"] = AreaNids;
        Session["hdsby"] = "dataview.aspx";

        tmpDBNId = DbNid;
        int filterIUS_NId = 0;
        int filterDvStart = 0;
        int filterDvEnd = 0;

        // 1. Separate Data Value Filter String from param
        // 2. Fetch datavaiew based on Area IUS , Time Period Filter, Source filter
        // 3. Get rows satisfying Datavalue filter
        // 4. Get unique AreaNId,.... for from above rows

        #region Prepare parameters from requestParam for stored procedure execution.

        try
        {

            if (String.IsNullOrEmpty(AreaNids))
            {
                AreaNids = Global.GetDefaultArea(DbNid.ToString());
            }
            if (String.IsNullOrEmpty(IUSNids))
            {
                IUSNids = Global.GetDefaultIusNIds(DbNid.ToString());
            }

            AreaNids = formatCommaLimitedString(AreaNids);
            IUSNids = formatCommaLimitedString(IUSNids);

            Session["CurrentSelectedAreaNids"] = AreaNids;

            if (_DBCon == null)
            {
                this.GetDbConnection(tmpDBNId);
            }

            if (_DBCon != null)
            {
                System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
                Param1.ParameterName = "strAllAreaQSIds_Nids";
                Param1.DbType = DbType.String;
                Param1.Value = AreaNids;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = _DBCon.CreateDBParameter();
                Param2.ParameterName = "strIUS_Nids";
                Param2.DbType = DbType.String;
                Param2.Value = IUSNids;
                DbParams.Add(Param2);

                //try //Code for capturing optional parameters for filtering
                //{
                //    string[] OptionalParams = Global.SplitString(Params[4], "||");

                //    foreach (string spParamName in OptionalParams)
                //    {
                //        string[] KeyVal = spParamName.Split(':');
                //        switch (KeyVal[0])
                //        {

                //            case "IUS_NId":
                //                {
                //                    filterIUS_NId = Convert.ToInt32(KeyVal[1]);
                //                    break;
                //                }
                //            case "dvStart":
                //                {
                //                    filterDvStart = Convert.ToInt32(KeyVal[1]);
                //                    break;
                //                }
                //            case "dvEnd":
                //                {
                //                    filterDvEnd = Convert.ToInt32(KeyVal[1]);
                //                    break;
                //                }
                //            default:
                //                {
                //                    System.Data.Common.DbParameter dbParam = _DBCon.CreateDBParameter();
                //                    dbParam.ParameterName = KeyVal[0];
                //                    dbParam.DbType = DbType.String;
                //                    dbParam.Value = KeyVal[1];
                //                    DbParams.Add(dbParam);
                //                    break;
                //                }

                //        }
                //    }
                //}
                //catch (Exception) { }

        #endregion

                string CurrentLanguage = Global.GetDefaultLanguageCodeDB(DbNid.ToString(), UILanguage);
                //Get all data with MRD, Sources & Time period filter. If needed then DataValue filters would be applied over this set of data.
                DataTable dtDataviewWithoutDataValueFilter = _DBCon.ExecuteDataTable("sp_get_dataview_" + CurrentLanguage, CommandType.StoredProcedure, DbParams);

                //Handling of creating map visualization after session time out
                SessionDetails sdetails = new SessionDetails();
                if (HttpContext.Current.Request.Cookies["SessionID"] == null)
                {
                    Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
                }
                sdetails.IsMyData = false;
                sdetails.DataViewNonQDS = dtDataviewWithoutDataValueFilter;
                sdetails.CurrentSelectedAreaNids = Session["CurrentSelectedAreaNids"].ToString();
                Global.SerializeObject(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml", sdetails);

                Session["IsMyData"] = false;
                Session["DataViewNonQDS"] = dtDataviewWithoutDataValueFilter;

                if (Session["DIMap"] != null)
                {
                    ((DevInfo.Lib.DI_LibMap.Map)Session["DIMap"]).Dispose();
                    Session["DIMap"] = null;
                }
                Result = PrepareDataViewString(dtDataviewWithoutDataValueFilter, CurrentLanguage, filterIUS_NId, filterDvStart, filterDvEnd);
                return Result;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return "false";
        }

        finally
        {
            if (_DBCon != null)
            {
                _DBCon.Dispose();
            }

        }

        return Result;

    }

    /// <summary>
    /// ?callback=13&param1=FULL_HTML_FOR_OAT        
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string GetDataViewDataNids(string requestParam)
    {
        string Result = string.Empty;

        StringBuilder sbResult = new StringBuilder();

        string DataNids = string.Empty;

        string SelectedDimension = string.Empty;
        string UILanguage = string.Empty;

        List<string> DefaultSubgroups = new List<string>();

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        UILanguage = Params[2]; // UI language code
        string[] paramFromQds = Params[0].Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        if (paramFromQds.Length == 2) // QDS Visualize feature is used with a selected dimension
        {
            DataNids = paramFromQds[0];
            SelectedDimension = paramFromQds[1];
        }
        else if (paramFromQds.Length == 1) // QDS 'add to cart' feature is used
        {
            string[] paramsDataNIds_DefaultSGs = paramFromQds[0].Split(',');

            foreach (string paramVal in paramsDataNIds_DefaultSGs)
            {
                string[] paramDV_SG = paramVal.Split('-');
                DataNids += "," + paramDV_SG[0];

                if (!DefaultSubgroups.Contains(paramDV_SG[1])) DefaultSubgroups.Add(paramDV_SG[1]);
            }
            if (DataNids.Length > 0) DataNids = DataNids.Substring(1);
        }


        //Params after DBNID in this order : Source_NId, TimePeriod_NId, isMRD, dvStart, dvEnd

        //DataNids = Params[0].Split(':')[0];
        int DbNid = int.Parse(Params[1]);
        tmpDBNId = DbNid;

        int filterIUS_NId = 0;
        int filterDvStart = 0;
        int filterDvEnd = 0;
        string AreaNids = string.Empty;

        try
        {
            if (_DBCon == null)
            {
                this.GetDbConnection(tmpDBNId);
            }

            if (String.IsNullOrEmpty(AreaNids))
            {
                AreaNids = Global.GetDefaultArea(DbNid.ToString());
            }

            if (_DBCon != null)
            {
                System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
                Param1.ParameterName = "strAllData_Nids";
                Param1.DbType = DbType.String;
                Param1.Value = DataNids;
                DbParams.Add(Param1);

                try //Code for capturing optional parameters for filtering
                {
                    if (Params.Length > 3)
                    {
                        string[] OptionalParams = Global.SplitString(Params[3], "||");
                        foreach (string spParamName in OptionalParams)
                        {
                            string[] KeyVal = spParamName.Split(':');
                            switch (KeyVal[0])
                            {
                                case "IUS_NId":
                                    {
                                        filterIUS_NId = Convert.ToInt32(KeyVal[1]);
                                        break;
                                    }
                                case "dvStart":
                                    {
                                        filterDvStart = Convert.ToInt32(KeyVal[1]);
                                        break;
                                    }
                                case "dvEnd":
                                    {
                                        filterDvEnd = Convert.ToInt32(KeyVal[1]);
                                        break;
                                    }
                                default:
                                    {
                                        System.Data.Common.DbParameter dbParam = _DBCon.CreateDBParameter();
                                        dbParam.ParameterName = KeyVal[0];
                                        dbParam.DbType = DbType.String;
                                        dbParam.Value = KeyVal[1];
                                        DbParams.Add(dbParam);
                                        break;
                                    }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                }

                string CurrentLanguage = CurrentLanguage = Global.GetDefaultLanguageCodeDB(DbNid.ToString(), UILanguage);

                DataTable dtDataviewWithoutDataValueFilter = _DBCon.ExecuteDataTable("sp_get_dataview_datanid_" + CurrentLanguage, CommandType.StoredProcedure, DbParams);

                //Handling of creating map visualization after session time out
                SessionDetails sdetails = new SessionDetails();
                if (HttpContext.Current.Request.Cookies["SessionID"] == null)
                {
                    Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
                }
                sdetails.IsMyData = false;
                sdetails.DataViewNonQDS = dtDataviewWithoutDataValueFilter;
                Global.SerializeObject(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml", sdetails);

                Session["IsMyData"] = false;
                Session["DataViewNonQDS"] = dtDataviewWithoutDataValueFilter;

                if (Session["DIMap"] != null)
                {
                    ((DevInfo.Lib.DI_LibMap.Map)Session["DIMap"]).Dispose();
                    Session["DIMap"] = null;
                }
                Session["CurrentSelectedAreaNids"] = null;
                Result = PrepareDataViewString(dtDataviewWithoutDataValueFilter, CurrentLanguage, filterIUS_NId, filterDvStart, filterDvEnd, SelectedDimension, DefaultSubgroups);
                return Result;

            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return "false";

        }

        finally
        {
            if (_DBCon != null)
            {
                _DBCon.Dispose();
            }

        }

        return Result;
    }

    public string WriteHTMLtoFile(string requestParam, string filePath)
    {
        string HTMLcontent, RelativeFileName;
        RelativeFileName = "PivotData_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_"
                            + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Ticks.ToString();
        string Result = "";
        try
        {
            // Title, SubTitle, Header array & Data array is passed in same sequence separated by ParamDelimiter
            string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            string DI_UI_LIB = Global.diuilib_url; //http://dgps/di7poc/diuilib/

            string Title = Params[0];
            string SubTitle = Params[1];
            string Headers = Params[2];
            string allData = Params[3];
            string rowConditions = Params[4];
            string colConditions = Params[5];
            string dbNID = Params[6];

            // OAT template is being read for replacing the variable with input data
            if (string.IsNullOrEmpty(filePath))
            {
                HTMLcontent = File.ReadAllText(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\shared\\dv\\OAT_Template.htm"));
                //HTMLcontent = File.ReadAllText(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\OAT_Template.aspx"));
            }
            else
            {
                HTMLcontent = File.ReadAllText(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\OAT_Template.htm"));                
            }

            HTMLcontent = HTMLcontent.Replace("STR_DI_UI_LIB", DI_UI_LIB);
            HTMLcontent = HTMLcontent.Replace("STR_TITLE", Title);
            HTMLcontent = HTMLcontent.Replace("STR_SUB_TITLE", SubTitle);
            HTMLcontent = HTMLcontent.Replace("STR_HEADER", "'" + Headers + "'");
            HTMLcontent = HTMLcontent.Replace("STR_DATA", "'" + allData.Replace("'", @"\'") + "'");
            HTMLcontent = HTMLcontent.Replace("STR_ROW_CONDITION", "'" + rowConditions + "'");
            HTMLcontent = HTMLcontent.Replace("STR_COL_CONDITION", "'" + colConditions + "'");
            HTMLcontent = HTMLcontent.Replace("STR_DB_NID", dbNID);
            //HTMLcontent = HTMLcontent.Replace("stock_templates_OAT_Template", "stock_shared_dv_" + RelativeFileName);
            //HTMLcontent = HTMLcontent.Replace("OAT_Template", RelativeFileName);

            if (string.IsNullOrEmpty(filePath))
            {
                // New created HTML file is written in Shared folder and then File Name is returned back to ajax callback 
                File.WriteAllText(Server.MapPath("../../stock/shared/dv/") + RelativeFileName + ".htm", HTMLcontent);
                //File.Copy(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\OAT_Template.aspx.cs"), Server.MapPath("../../stock/shared/dv/") + RelativeFileName + ".aspx.cs");
                //string CsFileContent = File.ReadAllText(Server.MapPath("../../stock/shared/dv/") + RelativeFileName + ".aspx.cs");
                //CsFileContent = CsFileContent.Replace("stock_templates_OAT_Template", "stock_shared_dv_" + RelativeFileName);
                //File.WriteAllText(Server.MapPath("../../stock/shared/dv/") + RelativeFileName + ".aspx.cs", CsFileContent);
            }
            else
            {
                File.WriteAllText(filePath, HTMLcontent);
            }
            Result = RelativeFileName;

        }
        catch (Exception ex)
        {
            Result = "";
            Global.CreateExceptionString(ex, null);

        }

        return Result;

    }


    public string ShareByEmail(string requestParam)
    {
        string Result = "";
        try
        {
            // Title, SubTitle, Header array & Data array is passed in same sequence separated by ParamDelimiter
            string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            string EmailAddresses = Params[0];
            string ShareLink = Params[1];

            string allTemplates = System.IO.File.ReadAllText(Server.MapPath("~/stock/templates/emailtemplates.xml"));
            int initPos = allTemplates.IndexOf("<![CDATA[") + "<![CDATA[".Length;
            int endPos = allTemplates.IndexOf("]]>");

            string MsgBody = allTemplates.Substring(initPos, endPos - initPos);

            initPos = allTemplates.IndexOf("<subject>") + "<subject>".Length;
            endPos = allTemplates.IndexOf("</subject>");

            string MsgSubject = allTemplates.Substring(initPos, endPos - initPos);

            EmailShare es = new EmailShare(MsgSubject, MsgBody);

            Result = es.SendTemplateEmail(EmailAddresses, Global.adaptation_name, ShareLink).ToString();

        }
        catch (Exception ex)
        {
            Result = "false";
            Global.CreateExceptionString(ex, null);

        }

        return Result;

    }


    public string GetDataViewDataNidsMulipleDbs(string requestParam)
    {
        string Result = string.Empty;

        //StringBuilder sbResult = new StringBuilder();

        //List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        //string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

        ////Params after DBNID in this order : Source_NId, TimePeriod_NId, isMRD, dvStart, dvEnd

        //string[] DataNids = Params[0].Split(";");
        //string[] DbNids = Params[1].Split(",");
        //tmpDBNId = DbNid;

        //int filterIUS_NId = 0;
        //int filterDvStart = 0;
        //int filterDvEnd = 0;

        //try
        //{
        //    if (_DBCon == null)
        //    {
        //        this.GetDbConnection(tmpDBNId);
        //    }

        //    if (_DBCon != null)
        //    {
        //        System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
        //        Param1.ParameterName = "strAllData_Nids";
        //        Param1.DbType = DbType.String;
        //        Param1.Value = DataNids;
        //        DbParams.Add(Param1);

        //        try //Code for capturing optional parameters for filtering
        //        {
        //            string[] OptionalParams = Global.SplitString(Params[2], "||");

        //            foreach (string spParamName in OptionalParams)
        //            {
        //                string[] KeyVal = spParamName.Split(':');
        //                switch (KeyVal[0])
        //                {

        //                    case "IUS_NId":
        //                        {
        //                            filterIUS_NId = Convert.ToInt32(KeyVal[1]);
        //                            break;
        //                        }
        //                    case "dvStart":
        //                        {
        //                            filterDvStart = Convert.ToInt32(KeyVal[1]);
        //                            break;
        //                        }
        //                    case "dvEnd":
        //                        {
        //                            filterDvEnd = Convert.ToInt32(KeyVal[1]);
        //                            break;
        //                        }
        //                    default:
        //                        {
        //                            System.Data.Common.DbParameter dbParam = _DBCon.CreateDBParameter();
        //                            dbParam.ParameterName = KeyVal[0];
        //                            dbParam.DbType = DbType.String;
        //                            dbParam.Value = KeyVal[1];
        //                            DbParams.Add(dbParam);
        //                            break;
        //                        }

        //                }
        //            }
        //        }
        //        catch (Exception) { }


        //        DataTable dtDataviewWithoutDataValueFilter = _DBCon.ExecuteDataTable("sp_get_dataview_datanid", CommandType.StoredProcedure, DbParams);

        //        Result = PrepareDataViewString(dtDataviewWithoutDataValueFilter, filterIUS_NId, filterDvStart, filterDvEnd);
        //        return Result;

        //    }

        //}
        //catch (Exception ex)
        //{

        //    return "false";
        //}

        //finally
        //{
        //    if (_DBCon != null)
        //    {
        //        _DBCon.Dispose();
        //    }

        //}

        return Result;
    }


    #region "Helper functions for GetDataView functions"


    private string PrepareDataViewString(DataTable dtDataviewWithoutDataValueFilter, string Language,
        int filterIUS_NId, int filterDvStart, int filterDvEnd, string optionalSelectedDimension = "", List<string> DefaultSubGroups = null)
    {
        string Result = string.Empty;
        try
        {
            SessionDetails sdetails = null;

            StringBuilder sbResult = new StringBuilder();

            #region "Filter by DataValue range specified"


            DataRow[] FilteredByIUS_NId = dtDataviewWithoutDataValueFilter.Select("IUSNId=" + filterIUS_NId.ToString()
                + " and Convert(Data_Value,'System.Single') >= " + filterDvStart.ToString()
                + " and Convert(Data_Value,'System.Single') <= " + filterDvEnd.ToString());
            DataTable dtFilteredByIUS_NId = dtDataviewWithoutDataValueFilter.Clone();

            List<int> filteredAreas = new List<int>();
            List<string> filteredTimePeriods = new List<string>();
            List<int> filteredSources = new List<int>();
            foreach (DataRow dr in FilteredByIUS_NId)
            {
                dtFilteredByIUS_NId.ImportRow(dr);
                int tmpArea = Convert.ToInt32(dr["Area_NId"].ToString());
                string tmpTimePeriod = dr["TimePeriod"].ToString();
                int tmpSource = Convert.ToInt32(dr["IC_NId"].ToString());
                if (!filteredAreas.Contains(tmpArea)) filteredAreas.Add(tmpArea);
                if (!filteredTimePeriods.Contains(tmpTimePeriod)) filteredTimePeriods.Add(tmpTimePeriod);
                if (!filteredSources.Contains(tmpSource)) filteredSources.Add(tmpSource);

            }


            foreach (DataRow dr in dtDataviewWithoutDataValueFilter.Rows)
            {
                int tmpArea = Convert.ToInt32(dr["Area_NId"].ToString());
                string tmpTimePeriod = dr["TimePeriod"].ToString();
                int tmpSource = Convert.ToInt32(dr["IC_NId"].ToString());
                int tmpIUSNId = Convert.ToInt32(dr["IUSNId"].ToString());

                if ((tmpIUSNId != filterIUS_NId
                    && filteredAreas.Contains(tmpArea))
                    && filteredTimePeriods.Contains(tmpTimePeriod)
                    && filteredSources.Contains(tmpSource))
                {
                    dtFilteredByIUS_NId.ImportRow(dr);
                }
            }

            if (dtFilteredByIUS_NId.Rows.Count > 0) dtDataviewWithoutDataValueFilter = dtFilteredByIUS_NId;
            else
            {
                if (!(filterDvEnd == 0 && filterDvStart == 0 && filterIUS_NId == 0)) return "";
            }

            #endregion

            #region "Evaluate Distinct NIds for all fields and then fetch their respective Names value into Hashtables"


            // Finding distincts for all fields and saving them in separate DataTables
            DataTable dtDistinctAreaNIds = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "Area_NId");
            DataTable dtDistinctIndicatorNIds = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "IUNId");
            DataTable dtDistinctSubgroup_Val_NIds = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "Subgroup_Val_NId");
            DataTable dtDistinctSourceNIds = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "IC_NId");
            DataTable dtDistinctTimePeriods = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "TimePeriod");
            DataTable dtDistinctIUSs = dtDataviewWithoutDataValueFilter.DefaultView.ToTable(true, "IUSNId");

            string strSelectedAreas = "";

            foreach (DataRow dr in dtDistinctAreaNIds.Rows) strSelectedAreas += "," + dr["Area_NId"].ToString();

            if (strSelectedAreas.Length > 0) strSelectedAreas = strSelectedAreas.Substring(1);

            if (Session["CurrentSelectedAreaNids"] == null)
            {
                //Handling of creating map visualization after session time out
                sdetails = SessionDetails.Load(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml");
                if (sdetails != null)
                {
                    sdetails.CurrentSelectedAreaNids = strSelectedAreas;
                    Global.SerializeObject(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml", sdetails);
                }

                Session["CurrentSelectedAreaNids"] = strSelectedAreas;
            }

            // Getting respective names for corresponding Nids
            Areas = getNIdNamePair(dtDistinctAreaNIds, getDistinctNames(dtDistinctAreaNIds, "ut_area_" + Language, "Area_Name"));
            Sources = getNIdNamePair(dtDistinctSourceNIds, getDistinctNames(dtDistinctSourceNIds, "ut_indicator_classifications_" + Language, "IC_Name"));
            Indicators = new Hashtable();
            Hashtable IUSs = getIUSNames(dtDistinctIUSs, Language);

            foreach (DataRow dr in dtDistinctIndicatorNIds.Rows)
            {
                Indicators.Add(dr["IUNId"].ToString(), getIUNidName(dr["IUNId"].ToString(), Language));
            }
            // For distinct SG_Vals store all dimensions in Hashtable 
            Subgroup_Vals = getSGName_DimensionPair(dtDistinctSubgroup_Val_NIds, Language);

            // For each SG save its vals and get dimension
            Dimensions = getDimensions(dtDistinctSubgroup_Val_NIds, Language);

            #endregion


            #region "Prepare informative NID:Name keypairs, title/sibtitles & column headers for OAT DataGrid"
            //This region also handles the singularity of Area, TimePeriod, Sources, Indicators & SubGroupVals
            string Captions = "";
            header = "";
            if (Areas.Count > 1 || dtDataviewWithoutDataValueFilter.Rows.Count == 1)
            {
                header += "Area,";
            }
            else
            {
                Captions += Constants.Delimiters.PivotColumnDelimiter + "Area:" + Areas[dtDistinctAreaNIds.Rows[0]["Area_NId"].ToString()].ToString();
                header += "Area,";
            }
            if (Indicators.Count > 1 || dtDataviewWithoutDataValueFilter.Rows.Count == 1)
            {
                header += "Indicator,";
            }
            else
            {
                Captions += Constants.Delimiters.PivotColumnDelimiter + "Indicator:" + Indicators[dtDistinctIndicatorNIds.Rows[0]["IUNId"].ToString()].ToString();
                header += "Indicator,";
            }

            if (dtDistinctTimePeriods.Rows.Count > 1 || dtDataviewWithoutDataValueFilter.Rows.Count == 1)
            {
                header += "Time Period,";
            }
            else
            {
                Captions += Constants.Delimiters.PivotColumnDelimiter + "Time Period:" + dtDistinctTimePeriods.Rows[0]["TimePeriod"].ToString();
            }

            if (dtDistinctSubgroup_Val_NIds.Rows.Count > 1 || dtDataviewWithoutDataValueFilter.Rows.Count == 1)
            {
                header += Dimensions.Replace(Constants.Delimiters.PivotColumnDelimiter, ",");
                if (header.LastIndexOf(',') != header.Length - 1) header += ",";
            }
            else
            {
                Captions += Constants.Delimiters.PivotColumnDelimiter + "Sub Group:" + getFirstKey(Subgroup_Vals);
            }

            if (Captions.IndexOf(Constants.Delimiters.PivotColumnDelimiter) == 0)
            {
                Captions = Captions.Substring(1);
            }

            if (Sources.Count > 1 || dtDataviewWithoutDataValueFilter.Rows.Count == 1)
            {
                header += "Source,";
            }
            else
            {
                Captions += Constants.Delimiters.PivotColumnDelimiter + "Source:" + Sources[dtDistinctSourceNIds.Rows[0]["IC_NId"].ToString()].ToString();
            }
            #endregion

            #region Dimension Items handling(if required)
            string strDimensions = string.Empty;
            if (optionalSelectedDimension != "" && !string.IsNullOrEmpty(optionalSelectedDimension))
            {
                strDimensions = getDimensionItems_SelectedDimension(optionalSelectedDimension, Subgroup_Vals, Dimensions);
            }

            if (DefaultSubGroups != null)
            {
                if (DefaultSubGroups.Count != 0)
                {
                    strDimensions = getDimensionItems_DefaultSubgroup(DefaultSubGroups, Subgroup_Vals, Dimensions);
                }
            }

            #endregion

            #region "Append all above informative data in final string which client has to receive"

            StringBuilder strJSonRepeaters = new StringBuilder();
            strJSonRepeaters.Append(getJsonFromHash(Areas));
            strJSonRepeaters.Append(getJsonFromHash(Indicators));
            strJSonRepeaters.Append(getJsonFromHash(Units));
            strJSonRepeaters.Append(getJsonFromHash(Sources));
            strJSonRepeaters.Append(getJsonFromHash(IUSs));
            strJSonRepeaters.Append("{" + strDimensions + "}");

            strJSonRepeaters.Append(Constants.Delimiters.PivotRowDelimiter);
            Captions = Captions + Constants.Delimiters.PivotRowDelimiter;

            if (header.LastIndexOf(',') != header.Length - 1) header += ",";
            header += "Area_ID,isMRD,FootNote_NId,Data Value"; //,isMRD

            //Create header string
            sbResult.Append(strJSonRepeaters.ToString());
            sbResult.Append(Captions);
            sbResult.Append(header.Replace(",", Constants.Delimiters.PivotColumnDelimiter) + Constants.Delimiters.PivotRowDelimiter);
            #endregion

            //Append data for each row, after processing, into final string
            foreach (DataRow dr in dtDataviewWithoutDataValueFilter.Rows)
            {
                sbResult.Append(getProcessedRow(dr) + Constants.Delimiters.PivotRowDelimiter);
            }


            //
            Result = sbResult.ToString().Substring(0, sbResult.Length - 1);

            //if (sbResult.ToString().Substring(0, 1) == "{")
            //{
            //    if (sbResult.ToString().Substring(0, sbResult.Length - 1) != "}")
            //    {
            //        Result = sbResult.ToString().Substring(0, sbResult.Length - 1) + "}";
            //    }
            //    else
            //    {
            //        Result = sbResult.ToString();
            //    }
            //}
            //else
            //{
            //    Result = sbResult.ToString().Substring(0, sbResult.Length - 1);
            //}


            return Result;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return Result;
        }
    }

    /// <summary>
    /// When Visualize/Add to cart is used, then only default subgroups are checked ON in filters panel
    /// </summary>
    /// <param name="dtDataviewWithoutDataValueFilter"></param>
    /// <param name="htSubgroups_Dimension"></param>
    /// <param name="strDimensions"></param>
    /// <returns></returns>
    private string getDimensionItems_DefaultSubgroup(List<string> DefaultSubGroupsVals, Hashtable htSubgroups_Dimension, string strDimensions)
    {
        string RetVal = string.Empty;

        List<string> DefaultSubgroups = new List<string>();

        foreach (string SubGroupVal in DefaultSubGroupsVals)
        {
            List<string> Subgroups = (List<string>)allSG[SubGroupVal];

            foreach (string SubGroup in Subgroups)
            {
                if (!DefaultSubgroups.Contains(SubGroup)) DefaultSubgroups.Add(SubGroup);
            }
        }

        RetVal = getStringForDimensionsSubgroupsPair(strDimensions.Split('|'), DefaultSubgroups, htSubgroups_Dimension);

        return RetVal;
    }
    /// <summary>
    /// When selected Dimension is clicked in QDS results, then few items in filters panel are checked OFF
    /// </summary>
    /// <param name="dtDataviewWithoutDataValueFilter"></param>
    /// <param name="htSubgroups_Dimension"></param>
    /// <param name="strDimensions"></param>
    /// <returns></returns>
    private string getDimensionItems_SelectedDimension(string SelectedDimension, Hashtable htSubgroups_Dimension, string strDimensions)
    {
        string RetVal = string.Empty;
        try
        {
            List<string> SelectedDimensionItems = new List<string>();

            // For storing only those subgroups which qualifies for final display in filters :
            List<string> ValidSubgroups = new List<string>();
            // Get subgroups of selected dimension
            foreach (string SubGroup in htSubgroups_Dimension.Keys)
            {
                string tmpDimension = (string)htSubgroups_Dimension[SubGroup];
                if (SelectedDimension == tmpDimension)
                {
                    SelectedDimensionItems.Add(SubGroup);
                }
            }

            // Firstly, check whether selected dimension's subgroups exists individually or not? If any of them exists alone, then don't find dependent subgroups. Go directly to GetDimensionsSG_Pair label.

            bool IsSelectedSubGroupExistsIndividually = false;

            foreach (string SelectedDimensionItem in SelectedDimensionItems)
            {
                foreach (string SubGroupValKey in allSG.Keys)
                {
                    List<string> SubGroups = (List<string>)allSG[SubGroupValKey];

                    // If subgroup found then add all dependent also 
                    if (SubGroups.Count == 1 && SubGroups[0] == SelectedDimensionItem && !ValidSubgroups.Contains(SelectedDimensionItem))
                    {
                        // Add this subgroup to result subgroups
                        ValidSubgroups.Add(SelectedDimensionItem);
                        IsSelectedSubGroupExistsIndividually = true;
                        break;
                    }
                }
            }

            // 
            if (IsSelectedSubGroupExistsIndividually) goto GetDimensionsSG_Pair;

            // For each selected dimension's subgroup, traverse all subgroups for finding its associated/dependent subgroups(if present)
            foreach (string SelectedDimensionItem in SelectedDimensionItems)
            {
                foreach (string SubGroupValKey in allSG.Keys)
                {
                    List<string> SubGroups = (List<string>)allSG[SubGroupValKey];

                    // If subgroup found then add all dependent also 
                    if (SubGroups.Contains(SelectedDimensionItem))
                    {
                        // Add NEWER friends only(not selected dimension's subroup)
                        foreach (string tmpSubGroup in SubGroups)
                        {
                            if (!ValidSubgroups.Contains(tmpSubGroup))
                                ValidSubgroups.Add(tmpSubGroup);
                        }
                    }
                }
            }

        GetDimensionsSG_Pair:
            // Get dimensions-Subgroups pair        

            RetVal = getStringForDimensionsSubgroupsPair(strDimensions.Split('|'), ValidSubgroups, htSubgroups_Dimension);

            return RetVal;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return RetVal;
        }

    }
    /// <summary>
    /// Helper function for getDimensionItems_DefaultSubgroup, getDimensionItems_SelectedDimension
    /// </summary>
    /// <param name="arrDimensions"></param>
    /// <param name="ValidSubgroups"></param>
    /// <param name="htSubgroups_Dimension"></param>
    /// <returns></returns>
    private string getStringForDimensionsSubgroupsPair(string[] arrDimensions, List<string> ValidSubgroups, Hashtable htSubgroups_Dimension)
    {
        string RetVal = string.Empty;
        try
        {
            char consComma = ',';
            char consColon = ':';
            char consPipe = '|';

            Hashtable htDimensions_Items = new Hashtable();

            foreach (string Dimension in arrDimensions) htDimensions_Items.Add(Dimension, string.Empty);

            foreach (string DefaultSubgroup in ValidSubgroups)
            {
                string DimensionName = (string)htSubgroups_Dimension[DefaultSubgroup];
                string tmpItems = (string)htDimensions_Items[DimensionName];
                if (tmpItems == string.Empty) tmpItems = DefaultSubgroup;
                else tmpItems += consComma + DefaultSubgroup;

                htDimensions_Items[DimensionName] = tmpItems;
            }

            foreach (string htDimensionsKey in htDimensions_Items.Keys)
            {
                string tmpItem = (string)htDimensions_Items[htDimensionsKey];

                if (tmpItem != string.Empty)
                {
                    RetVal += htDimensionsKey + consColon;
                    RetVal += tmpItem + consPipe;
                }

            }

            return RetVal;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return RetVal;
        }
    }

    private string getJsonFromHash(Hashtable inputHash)
    {
        StringBuilder Result = new StringBuilder();
        Result.Append("{");
        try
        {
            if (inputHash != null)
            {
                ICollection tmpKeys = inputHash.Keys;
                bool isFirst = true;
                foreach (object key in tmpKeys)
                {
                    if (!isFirst)
                    {
                        Result.Append("|" + key + ":" + inputHash[key]);
                    }
                    else
                    {
                        Result.Append(key + ":" + inputHash[key]);
                        isFirst = false;
                    }
                }    
            }            
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        Result.Append("}");
        return Result.ToString();
    }

    private string getFirstKey(Hashtable inputHash)
    {
        string Result = "";
        ICollection tmpKeys = inputHash.Keys;
        foreach (object key in tmpKeys)
        {
            Result = key.ToString();
            break;
        }
        return Result;
    }

    private string formatCommaLimitedString(string inputString)
    {
        inputString = inputString.Replace(",,", ",");
        if (inputString.LastIndexOf(",") == inputString.Length - 1) inputString = inputString.Substring(0, inputString.Length - 1);
        return inputString;

    }

    private string getDimensions(DataTable DistinctSubgroup_Val_NIds, string Language)
    {
        string Result = "";

        DataTable RetVal;
        string strCommaSeparatedNids = "";
        foreach (DataRow dr in DistinctSubgroup_Val_NIds.Rows)
        {
            List<string> tmpSGs = getSubgroup_Name(dr[0].ToString(), Language);
            allSG.Add(dr[0].ToString(), tmpSGs);
            strCommaSeparatedNids += dr[0].ToString() + ","; //if(tmpSGs.Count>1) 
        }

        if (!string.IsNullOrEmpty(strCommaSeparatedNids)) strCommaSeparatedNids = strCommaSeparatedNids.Substring(0, strCommaSeparatedNids.Length - 1);

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
        Param1.ParameterName = "DistinctSubgroup_Val_NIds";
        Param1.DbType = DbType.String;
        Param1.Value = strCommaSeparatedNids;
        DbParams.Add(Param1);

        string strSQL = string.Empty; //"select Subgroup_Type_Name from dbo.ut_subgroup_type_en where Subgroup_Type_NId in (select Subgroup_Type from dbo.ut_subgroup_en where Subgroup_NId in" +
        //"(select Subgroup_Nid from dbo.ut_subgroup_vals_subgroup where Subgroup_Val_NId in (" + strCommaSeparatedNids + "))"
        //+ "or Subgroup_NId in (" + strCommaSeparatedNids + ")" 
        //+ ")"
        // ;
        try
        {


            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable("sp_getDimensions_" + Language, CommandType.StoredProcedure, DbParams);
                strCommaSeparatedNids = "";
                foreach (DataRow dr in RetVal.Rows) strCommaSeparatedNids += dr[0].ToString() + Constants.Delimiters.PivotColumnDelimiter;
                if (!string.IsNullOrEmpty(strCommaSeparatedNids)) strCommaSeparatedNids = strCommaSeparatedNids.Substring(0, strCommaSeparatedNids.Length - 1);

                return strCommaSeparatedNids;
            }

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("getDimensions ERROR : " + ex.Message);
            Global.CreateExceptionString(ex, null);

        }


        return Result;


    }

    private string getIUNidName(string IUNid, string Language)
    {
        string Result = "";
        DataTable RetVal;

        string[] IU = IUNid.Split('_');

        string strSQL = string.Empty;
        //"Select " +
        //                "(select top 1 Indicator_Name from dbo.ut_indicator_en where Indicator_NId = " + IU[0] + ") + ' - ' + " +
        //                "(select top 1 unit_name from dbo.ut_unit_en where Unit_NId = " + IU[1] + ")";

        try
        {
            List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
            System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
            Param1.ParameterName = "IndicatorNId";
            Param1.DbType = DbType.Int32;
            Param1.Value = int.Parse(IU[0]);
            DbParams.Add(Param1);

            System.Data.Common.DbParameter Param2 = _DBCon.CreateDBParameter();
            Param2.ParameterName = "UnitNId";
            Param2.DbType = DbType.Int32;
            Param2.Value = int.Parse(IU[1]);
            DbParams.Add(Param2);

            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable("sp_getIUNidName_" + Language, CommandType.StoredProcedure, DbParams);
                return RetVal.Rows[0][0].ToString();
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return Result;
    }

    private DataTable getDistinctNames(DataTable DistinctNids, string TargetDbTable, string TargetField)
    {
        DataTable Result = new DataTable();
        DataTable RetVal;
        string strCommaSeparatedNids = "";
        foreach (DataRow dr in DistinctNids.Rows) strCommaSeparatedNids += dr[0].ToString() + ",";

        if (!string.IsNullOrEmpty(strCommaSeparatedNids)) strCommaSeparatedNids = strCommaSeparatedNids.Substring(0, strCommaSeparatedNids.Length - 1);

        string NIdColumnName = DistinctNids.Columns[0].ColumnName;

        string strSQL = "select " + NIdColumnName + "," + TargetField +
            " from dbo." + TargetDbTable +
            " where " + NIdColumnName +
            " in (" + strCommaSeparatedNids + ")";
        //" in ( select items from dbo.split('" + strCommaSeparatedNids + "',','))";
        try
        {


            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable(strSQL);
                return RetVal;
            }

        }
        catch (Exception ex)
        {
            //Console.Write(ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        return Result;
    }

    private Hashtable getSGName_DimensionPair(DataTable distinctSGNids, string Language)
    {
        Hashtable Result = new Hashtable();

        DataTable RetVal;
        string strCommaSeparatedNids = "";
        foreach (DataRow dr in distinctSGNids.Rows)
        {
            strCommaSeparatedNids += dr[0].ToString() + ",";
        }

        if (!string.IsNullOrEmpty(strCommaSeparatedNids))
        {
            strCommaSeparatedNids = strCommaSeparatedNids.Substring(0, strCommaSeparatedNids.Length - 1);
        }
        string strSQL = string.Empty;
        //"select SubGroup_Name, (select top 1 Subgroup_Type_Name from dbo.ut_subgroup_type_en where " +
        //    "Subgroup_Type_NId = SubGroup_Type) as Dimension  from dbo.ut_subgroup_en where Subgroup_NId in " +
        //    "(select Subgroup_Nid from dbo.ut_subgroup_vals_subgroup where Subgroup_Val_NId in (" + strCommaSeparatedNids + ")) "
        //    //+ "or Subgroup_NId in (" + strCommaSeparatedNids + ")";
        //;

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
        Param1.ParameterName = "distinctSGNids";
        Param1.DbType = DbType.String;
        Param1.Value = strCommaSeparatedNids;
        DbParams.Add(Param1);

        try
        {


            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable("sp_getSGName_DimensionPair_" + Language, CommandType.StoredProcedure, DbParams);
                foreach (DataRow dr in RetVal.Rows)
                {
                    Result.Add(dr["SubGroup_Name"].ToString(), dr["Dimension"].ToString());
                }
                return Result;
            }

        }
        catch (Exception ex)
        {
            //Console.Write(ex.Message);
            Global.CreateExceptionString(ex, null);

        }


        return Result;

    }

    private List<string> getSubgroup_Name(string Subgroup_Val_NId, string Language)
    {
        List<string> Result = new List<string>();

        DataTable RetVal;


        string strSQL = string.Empty;
        //"select CAST((select top 1 Subgroup_Name from dbo.ut_subgroup_en where Subgroup_Nid = svs.Subgroup_Nid)" +
        //     " AS varchar(50)) as Subgroup_Name from dbo.ut_subgroup_vals_subgroup as svs where Subgroup_Val_NId = " + Subgroup_Val_NId;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
        Param1.ParameterName = "Subgroup_Val_NId";
        Param1.DbType = DbType.Int32;
        Param1.Value = int.Parse(Subgroup_Val_NId);
        DbParams.Add(Param1);

        try
        {


            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable("sp_getSubgroup_Name_" + Language, CommandType.StoredProcedure, DbParams);
                foreach (DataRow dr in RetVal.Rows) Result.Add(dr["SubGroup_Name"].ToString());

                return Result;
            }

        }
        catch (Exception ex)
        {
            //Console.Write(ex.Message);
            Global.CreateExceptionString(ex, null);

        }


        return Result;

    }

    private Hashtable getNIdNamePair(DataTable NIdsTable, DataTable NamesTable)
    {
        Hashtable Result = new Hashtable();

        for (int i = 0; i < NIdsTable.Rows.Count; i++)
        {
            Result.Add(NamesTable.Rows[i][0].ToString(), NamesTable.Rows[i][1].ToString());
        }
        return Result;

    }

    private Hashtable getIUSNames(DataTable DistinctNids, string Language)
    {
        Hashtable Result = new Hashtable();

        DataTable RetVal;
        string strCommaSeparatedNids = "";
        foreach (DataRow dr in DistinctNids.Rows) strCommaSeparatedNids += dr[0].ToString() + ",";

        if (!string.IsNullOrEmpty(strCommaSeparatedNids)) strCommaSeparatedNids = strCommaSeparatedNids.Substring(0, strCommaSeparatedNids.Length - 1);

        string strSQL = string.Empty;
        //"select IUSNId, (Select top 1 Indicator_Name from dbo.ut_indicator_en where Indicator_NId = dtIUS.Indicator_NId) + ', ' + (Select top 1 Unit_Name from dbo.ut_unit_en where Unit_NId = dtIUS.Unit_NId) + ', ' + (Select top 1 Subgroup_Val from dbo.ut_subgroup_vals_en where Subgroup_Val_NId = dtIUS.Subgroup_Val_NId) as IUSName from dbo.ut_indicator_unit_subgroup as dtIUS where IUSNId in ("
        //    + strCommaSeparatedNids + ")";
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
        Param1.ParameterName = "DistinctNids";
        Param1.DbType = DbType.String;
        Param1.Value = strCommaSeparatedNids;
        DbParams.Add(Param1);
        try
        {


            if (_DBCon != null)
            {
                RetVal = _DBCon.ExecuteDataTable("sp_getIUSNames_" + Language, CommandType.StoredProcedure, DbParams);
                foreach (DataRow dr in RetVal.Rows) Result.Add(dr["IUSNId"].ToString(), dr["IUSName"].ToString());
                return Result;
            }

        }
        catch (Exception ex)
        {
            //Console.Write(ex.Message);
            Global.CreateExceptionString(ex, null);

        }


        return Result;

    }

    private string getProcessedRow(DataRow dr)
    {
        string Result = "";
        try
        {
            string[] ColumnSequence = header.Split(",".ToCharArray());
            int TotalColumns = ColumnSequence.Length;
            for (int i = 0; i < TotalColumns; i++)
            {
                int isMRD = 0;
                switch (ColumnSequence[i])
                {
                    case "Area_ID":
                        Result += dr["Area_ID"].ToString();
                        break;
                    case "isMRD":
                        if (!string.IsNullOrEmpty(dr["isMRD"].ToString()) && ((bool)dr["isMRD"])) isMRD = 1;
                        Result += isMRD.ToString();
                        break;
                    case "Area":
                        Result += dr["Area_NId"].ToString(); //Areas[dr["Area_NId"].ToString()].ToString();
                        break;
                    case "Indicator":
                        Result += dr["IUNId"].ToString();// Indicators[dr["Indicator_NId"].ToString()].ToString();
                        break;
                    case "Unit_NId":
                        Result += dr["Unit_NId"].ToString();// Units[dr["Unit_NId"].ToString()].ToString();
                        break;
                    case "Subgroup_Val_NId":
                        Result += dr["Subgroup_Val_NId"].ToString();// Subgroup_Vals[dr["Subgroup_Val_NId"].ToString()].ToString();
                        break;
                    case "Source":
                        Result += dr["IC_NId"].ToString();// Sources[dr["IC_NId"].ToString()].ToString();
                        break;
                    case "Time Period":
                        Result += dr["TimePeriod"].ToString();
                        break;
                    case "Data Value":
                        Result += dr["Data_Value"].ToString();
                        break;
                    case "FootNote_NId":
                        Result += dr["FootNote_NId"].ToString(); //678
                        break;


                    default:
                        {
                            string HeaderName = ColumnSequence[i];
                            string SG_Val_Nid = dr["Subgroup_Val_NId"].ToString();
                            List<string> lstVals = (List<string>)allSG[SG_Val_Nid];
                            string DataForRow = "";
                            foreach (string tmpStr in lstVals)
                            {
                                if (Subgroup_Vals[tmpStr].ToString() == HeaderName)
                                {
                                    DataForRow = tmpStr;
                                    break;
                                }
                            }
                            Result += DataForRow;
                            break;

                        }
                }
                if (i != TotalColumns - 1) Result += Constants.Delimiters.PivotColumnDelimiter;

            }
            return Result;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return Result;
        }
    }

    #endregion




    public string getOATfromFile(string MetaFilePath)
    {
        string CallbackResult = string.Empty;

        string tempPath = Server.MapPath("../../stock/tempCYV");

        var jss = new JavaScriptSerializer();

        dynamic JsonData = jss.Deserialize<dynamic>(File.ReadAllText(MetaFilePath));

        string FullFilePath = JsonData["FilePathOnServer"];
        Session["IsMyData"] = true;

        if (FullFilePath.EndsWith(".csv") || FullFilePath.EndsWith(".xgp")) // CSV or XGP Format
        {
            StreamReader csvStream = new StreamReader(FullFilePath, Encoding.Default, true);

            CallbackResult = GenerateOAT_String(csvStream.BaseStream, JsonData);
            csvStream.Close();
            csvStream.Dispose();
            // Delete files after done task once.

        }
        else if (FullFilePath.EndsWith(".xls")) // XLS Format
        {
            #region XLS file handling
            string xlsFilePath = FullFilePath;
            string csvFilePath = tempPath + "\\" + Environment.TickCount.ToString() + ".csv";

            if (convertXLSToCSV(xlsFilePath, csvFilePath)) // Convert xls to csv
            {
                StreamReader csvStream = new StreamReader(csvFilePath, Encoding.Default, true);
                JsonData["arIDColIndex"] = "2";
                JsonData["tPeriosCol"] = "1";
                JsonData["arNameColIndex"] = "3";
                CallbackResult = GenerateOAT_String(csvStream.BaseStream, JsonData);
                csvStream.Close();
                csvStream.Dispose();
                // Delete files after done task once.
                File.Delete(csvFilePath);
            }

            #endregion
        }

        try
        {
            File.Delete(MetaFilePath);
            File.Delete(FullFilePath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return CallbackResult;

    }



    #region Helper functions for getOAT_string


    private string GenerateOAT_String(Stream fileInputStream, Dictionary<string, object> MapMatchingMetaData)
    {
        try
        {
            StringBuilder finalOatInput = new StringBuilder();

            StreamReader CsvFile = new StreamReader(fileInputStream, Encoding.UTF8, true);
            DataTable dtFromCsv = null;
            //bool IsFirstRow = true;
            //char separator = ',';
            List<string> wordList = new List<string>();
            List<string> NIdValuePair = new List<string>();

            if (Session["CurrentSelectedAreaNids"] != null)
            {
                Session["CurrentSelectedAreaNids"] = null;
            }

            #region Convert CSV to DataTable
            if (MapMatchingMetaData["tPeriosCol"].ToString().LastIndexOf(",") > -1)
            {
                dtFromCsv = getDTForAllColumnTimePeriod(CsvFile, wordList, MapMatchingMetaData);
                MapMatchingMetaData["tPeriosCol"] = getTimePeriodIndex(MapMatchingMetaData); ;
            }
            else
            {
                dtFromCsv = getDTForSelectedTimePeriod(CsvFile, wordList, MapMatchingMetaData);
            }
            #endregion


            #region Getting those Column Indexes which do not qualify for Numeric DataValues
            List<int> NonNumericColumns = new List<int>();

            bool checkForMoreNumericCols = true;
            int firstNumericCol = -1;

            for (int i = dtFromCsv.Columns.Count - 1; i > -1; i--)
            {

                if (!checkForMoreNumericCols) NonNumericColumns.Add(i);
                else
                {
                    foreach (DataRow dr in dtFromCsv.Rows)
                    {
                        float tmpVal = 0;

                        if (!float.TryParse(dr[i].ToString(), out tmpVal)) // If Non Numeric Found 
                        {
                            if (dr[i].ToString() != string.Empty && dr[i].ToString() != "-" && dr[i].ToString() != " ")
                            {
                                NonNumericColumns.Add(i);
                                break;
                            }
                        }
                        else // Figuring out consecutive numerics columns otherwise consider all as No-Numeric columns
                        {
                            if (firstNumericCol == -1) firstNumericCol = i;
                            else
                            {
                                if (firstNumericCol == i + 1 || firstNumericCol == i) firstNumericCol = i;
                                else
                                {
                                    checkForMoreNumericCols = false;
                                    NonNumericColumns.Add(i);
                                    break;
                                }
                            }

                        }

                    }
                }

                NIdValuePair.Add(dtFromCsv.Columns[i].ColumnName);
            }

            if (!string.IsNullOrEmpty(Convert.ToString(MapMatchingMetaData["tPeriosCol"])))
            {
                dtFromCsv = getMrdUpdatedTable(dtFromCsv, MapMatchingMetaData, NonNumericColumns);
                NonNumericColumns.Add(Convert.ToInt32(MapMatchingMetaData["tPeriosCol"]));
                NonNumericColumns.Add(dtFromCsv.Columns.Count - 1);
                NonNumericColumns.Sort();
                NonNumericColumns.Reverse();

                dtFromCsv.Columns[Convert.ToInt32(MapMatchingMetaData["tPeriosCol"])].ColumnName = "Time Period";
            }


            DataTable dtForMap = new System.Data.DataTable();



            dtForMap.Load(dtFromCsv.CreateDataReader(), System.Data.LoadOption.OverwriteChanges);


            for (int i = 0; i < dtFromCsv.Columns.Count; i++)
            {
                if (!NonNumericColumns.Contains(i)) // Numeric column found
                {
                    //dtForMap.Columns.Add("[**Data_Value**]" + i.ToString(), typeof(decimal));
                    dtForMap.Columns[i].Caption = "[**" + Data.DataValue + "**]" + i.ToString();
                    foreach (DataRow dr in dtForMap.Rows)
                    {
                        string CurrentDataValue = dr[i].ToString();
                        if (CurrentDataValue == "-") dr[i] = string.Empty;
                    }
                }
            }

            #endregion



            #region Utilize additional information for "map" creation


            if (MapMatchingMetaData["arIDColIndex"].ToString() != "-1") // Area ID information is provided by meta data
            {
                int AreaIdColIndex = Convert.ToInt16(MapMatchingMetaData["arIDColIndex"]);
                //AreaIdColIndex += dtFromCsv.Columns.Count - NonNumericColumns.Count;

                dtForMap.Columns[AreaIdColIndex].Caption = Area.AreaID;

                #region User selection matching is utilized


                if (!string.IsNullOrEmpty(MapMatchingMetaData["mappingStr"].ToString())) // Matching areas is done manually
                {
                    string[] ManuallyMatchedAreas = MapMatchingMetaData["mappingStr"].ToString().Replace("{||}", "#").Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string matchedArea in ManuallyMatchedAreas)
                    {
                        string[] AreaName_AreaID = matchedArea.Replace("{}", "|").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        foreach (DataRow dr in dtForMap.Rows)
                        {
                            if (dr[AreaIdColIndex].ToString() == AreaName_AreaID[0]) // Faulty Area ID is found, now replace it
                            {
                                if (AreaName_AreaID.Length == 2) dr[AreaIdColIndex] = AreaName_AreaID[1];
                                else dr[AreaIdColIndex] = "";
                            }
                        }
                    }
                }


                #endregion


                StringBuilder sbAreaIdsForSQL = new StringBuilder();

                foreach (DataRow dr in dtForMap.Rows) sbAreaIdsForSQL.Append("{}" + dr[AreaIdColIndex].ToString());
                if (sbAreaIdsForSQL.Length > 0) sbAreaIdsForSQL.Remove(0, 2);

                string nowDBnid = Global.GetDefaultDbNId();
                DataTable dtMatchedAreaIds = GetDataTable(int.Parse(nowDBnid), sbAreaIdsForSQL.ToString(), "aid", "en");

                string UnMatchedAreaIDs = filterRecords(dtMatchedAreaIds, sbAreaIdsForSQL.ToString());

                string[] arrUnmatchedAreaIDs = UnMatchedAreaIDs.Split(new string[] { "{}" }, StringSplitOptions.None);
                foreach (string UnMatchedAreaID in arrUnmatchedAreaIDs)
                {
                    foreach (DataRow dr in dtForMap.Rows)
                    {
                        if (dr[AreaIdColIndex].ToString() == UnMatchedAreaID)
                        {
                            dr[AreaIdColIndex] = "";
                        }
                    }
                }
            }

            if (MapMatchingMetaData["arNameColIndex"].ToString() != "-1") // Area Name is provided
            {
                int AreaNameColIndex = Convert.ToInt16(MapMatchingMetaData["arNameColIndex"]);
                //AreaNameColIndex += dtFromCsv.Columns.Count - NonNumericColumns.Count;

                dtForMap.Columns[AreaNameColIndex].Caption = Area.AreaName;


                #region Area ID is fetched & updated from DB


                StringBuilder sbAreaNamesForSQL = new StringBuilder();

                foreach (DataRow dr in dtForMap.Rows) sbAreaNamesForSQL.Append("{}" + dr[AreaNameColIndex].ToString().Trim());
                if (sbAreaNamesForSQL.Length > 0) sbAreaNamesForSQL.Remove(0, 2);
                string nowDBnid = Global.GetDefaultDbNId();
                string langCode = "";
                if (Session["hlngcode"] != null)
                {
                    langCode = Session["hlngcode"].ToString().Trim();
                }
                else
                {
                    langCode = Global.GetDefaultLanguageCode();
                }

                DataTable dtMatchedAreaNames = GetDataTable(int.Parse(nowDBnid), sbAreaNamesForSQL.ToString(), "aname", langCode);

                #region User selection matching is utilized


                if (!string.IsNullOrEmpty(MapMatchingMetaData["mappingStr"].ToString())) // Matching areas is done manually
                {
                    string[] ManuallyMatchedAreas = MapMatchingMetaData["mappingStr"].ToString().Replace("{||}", "#").Split("#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    foreach (string matchedArea in ManuallyMatchedAreas)
                    {
                        string[] AreaName_AreaID = matchedArea.Replace("{}", "|").Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                        // To be fixed: Column names should be fixed
                        if (AreaName_AreaID.Length == 2)
                        {
                            string areaName = AreaName_AreaID[0].Trim().ToLower();
                            string areaId = AreaName_AreaID[1].Trim().ToLower();
                            for (int i = 0; i < dtMatchedAreaNames.Rows.Count; i++)
                            {                                
                                string dtAreaName = dtMatchedAreaNames.Rows[i][0].ToString().Trim().ToLower();
                                string dtAreaId = dtMatchedAreaNames.Rows[i][1].ToString().Trim().ToLower();
                                if (areaName == dtAreaName)
                                {
                                    dtMatchedAreaNames.Rows.RemoveAt(i);
                                    i--;
                                }
                            }
                            DataRow drAreaID_Name = dtMatchedAreaNames.NewRow();
                            drAreaID_Name["Area_name"] = AreaName_AreaID[0];
                            drAreaID_Name["Area_id"] = AreaName_AreaID[1];
                            dtMatchedAreaNames.Rows.Add(drAreaID_Name);
                        }
                    }
                }


                #endregion


                int indexOfAreaID = -1;

                //if (!dtForMap.Columns.Contains("Area_ID")) 
                if (MapMatchingMetaData["arIDColIndex"].ToString() == "-1")
                {
                    dtForMap.Columns.Add("Area_ID");
                    indexOfAreaID = dtForMap.Columns.Count - 1;
                }
                else
                {
                    indexOfAreaID = int.Parse(MapMatchingMetaData["arIDColIndex"].ToString());
                }

                foreach (DataRow dr in dtForMap.Rows)
                {
                    string AreaIdFromDB = string.Empty;

                    foreach (DataRow drAreaIdName in dtMatchedAreaNames.Rows)
                    {
                        if (drAreaIdName["Area_name"].ToString().Trim().ToLower() == dr[AreaNameColIndex].ToString().Trim().ToLower())
                        {
                            AreaIdFromDB = drAreaIdName["Area_ID"].ToString();
                            break;
                        }
                    }

                    if (!string.IsNullOrEmpty(AreaIdFromDB)) dr[indexOfAreaID] = AreaIdFromDB;
                }

                #endregion

            }

            if (!string.IsNullOrEmpty(Convert.ToString(MapMatchingMetaData["tPeriosCol"])))
            {
                dtForMap.Columns[Convert.ToInt32(MapMatchingMetaData["tPeriosCol"])].Caption = Timeperiods.TimePeriod;
            }

            #endregion


            #region Prepare Headers & KeyValue pairs for all columns


            string Headers = string.Empty;
            string Titles = string.Empty;

            for (int i = NonNumericColumns.Count - 1; i > -1; i--)
            //for (int i = 0; i < NonNumericColumns.Count; i++)
            {
                DataTable dtDistinctVals = dtFromCsv.DefaultView.ToTable(true, dtFromCsv.Columns[NonNumericColumns[i]].ColumnName);
                if (dtDistinctVals.Rows.Count == 1) // If Column has same data then send it to Titles
                {
                    if (dtFromCsv.Columns[NonNumericColumns[i]].ColumnName != "isMRD")
                        Titles += dtDistinctVals.Rows[0][0].ToString() + " - ";
                    //else
                    //    Headers += "isMRD|";
                }
                else // Make NId:Value pair for them and append them in headers
                {
                    Headers += dtFromCsv.Columns[NonNumericColumns[i]].ColumnName + "|";
                    foreach (DataRow dr in dtDistinctVals.Rows)
                    {
                        NIdValuePair.Add(dr[dtFromCsv.Columns[NonNumericColumns[i]].ColumnName].ToString()); //.Replace("'","")
                    }
                }
            }

            Headers += "Data Column|Data Value";
            if (Titles.EndsWith("Data Column")) Titles = Titles.Substring(0, Titles.Length - 3);

            #endregion

            foreach (string ValueOfKey in NIdValuePair)
            {
                int indexOfValue = NIdValuePair.IndexOf(ValueOfKey);
                finalOatInput.Append(indexOfValue + ":" + ValueOfKey + "|");
            }
            finalOatInput.Append(Constants.Delimiters.PivotRowDelimiter);
            finalOatInput.Append(Titles + Constants.Delimiters.PivotRowDelimiter);

            finalOatInput.Append(Headers + Constants.Delimiters.PivotRowDelimiter);

            #region OAT data rows are prepared in string format


            foreach (DataRow dr in dtFromCsv.Rows)
            {
                string tmpRow = string.Empty;
                for (int i = 0; i < dtFromCsv.Columns.Count; i++)// If Non-Numeric distinct columns then append their NIds
                {
                    if (Headers.Contains(dtFromCsv.Columns[i].ColumnName + "|")
                        && NonNumericColumns.Contains(i))
                    {
                        tmpRow += NIdValuePair.IndexOf(dr[i].ToString()).ToString() + "|";
                    }
                }
                for (int i = 0; i < dtFromCsv.Columns.Count; i++)
                {
                    if (!Headers.Contains(dtFromCsv.Columns[i].ColumnName + "|")
                        && !NonNumericColumns.Contains(i)) // If Non-Numeric distinct columns then append their NIds
                    {
                        finalOatInput.Append(tmpRow
                            + NIdValuePair.IndexOf(dtFromCsv.Columns[i].ColumnName).ToString()
                            + "|" + dr[i].ToString() + Constants.Delimiters.PivotRowDelimiter);
                    }
                }
            }

            #endregion

            //Handling of creating map visualization after session time out
            SessionDetails sdetails = new SessionDetails();
            if (HttpContext.Current.Request.Cookies["SessionID"] == null)
            {
                Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
            }
            sdetails.DataViewNonQDS = dtForMap;
            sdetails.IsMyData = true;
            Global.SerializeObject(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml", sdetails);

            Session["DataViewNonQDS"] = dtForMap;
            if (Session["DIMap"] != null)
            {
                ((DevInfo.Lib.DI_LibMap.Map)Session["DIMap"]).Dispose();
                Session["DIMap"] = null;
            }

            return finalOatInput.ToString();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return "false";
        }
    }

    private DataTable getDTForSelectedTimePeriod(StreamReader CsvFile, List<string> wordList, Dictionary<string, object> MapMatchingMetaData)
    {
        DataTable dtResult = new DataTable("CSV");

        try
        {
            char separator = ',';
            bool IsFirstRow = true;
            while (CsvFile.Peek() != -1)
            {
                string line = CsvFile.ReadLine();
                if (line.IndexOf('"') > -1)
                {
                    wordList = getDoubleQuoteString(line);
                    foreach (string word in wordList)
                    {
                        string temp = word.Replace(",", "{}");
                        line = line.Replace('"' + word + '"', temp);
                    }
                }

                if (MapMatchingMetaData["FilePathOnServer"].ToString().EndsWith(".xgp") && IsFirstRow) // if XGP format
                    separator = (char)9;
                else if (IsFirstRow) // for CSV format // line.IndexOf(",") != -1 && 
                    separator = ',';

                string[] nowCsvLineVals = { };
                nowCsvLineVals = line.Split(separator);

                for (int i = 0; i < nowCsvLineVals.Length; i++)
                {
                    string tempString = nowCsvLineVals[i].Replace("{}", ",");
                    nowCsvLineVals[i] = tempString;
                }
                if (!IsFirstRow) //Get new row for Datatable if its not the first line of CSV
                {
                    bool areAllValuesBlank = true;
                    foreach (string nowVal in nowCsvLineVals)
                    {
                        if (!string.IsNullOrEmpty(nowVal.Trim())) areAllValuesBlank = false;
                    }
                    if (!areAllValuesBlank) dtResult.Rows.Add(nowCsvLineVals);
                }
                else //If its CSV's first line then use that for creating ColumnNames of data table
                {
                    int tmpSameColExistsCounter = 0;
                    foreach (string ColumnName in nowCsvLineVals)
                    {
                        string ColName = ColumnName;
                        if (dtResult.Columns.Contains(ColumnName))
                        {
                            ColName += tmpSameColExistsCounter.ToString();
                            tmpSameColExistsCounter++;
                        }
                        dtResult.Columns.Add(ColName.Trim());
                    }
                    IsFirstRow = false;
                }
            }
            return dtResult;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return dtResult;
        }
    }
    /// <summary>
    /// Get DataTable when all column are considered as time period except 1st column
    /// </summary>
    /// <param name="CsvFile">CSC File </param>
    /// <param name="wordList"></param>
    /// <param name="MapMatchingMetaData"></param>
    /// <returns></returns>
    private DataTable getDTForAllColumnTimePeriod(StreamReader CsvFile, List<string> wordList, Dictionary<string, object> MapMatchingMetaData)
    {
        DataTable dtResult = new DataTable("CSV");
        try
        {
            char separator = ',';
            bool IsFirstRow = true;
            string[] timeperiods = null;
            string[] headersIndexList = MapMatchingMetaData["tPeriosCol"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
            int totalStringCol = 0;
            int indexVal = -1;
            while (CsvFile.Peek() != -1)
            {
                string line = CsvFile.ReadLine();
                if (line.IndexOf('"') > -1)
                {
                    wordList = getDoubleQuoteString(line);
                    foreach (string word in wordList)
                    {
                        string temp = word.Replace(",", "{}");
                        line = line.Replace('"' + word + '"', temp);
                    }
                }

                if (MapMatchingMetaData["FilePathOnServer"].ToString().EndsWith(".xgp") && IsFirstRow) // if XGP format
                    separator = (char)9;
                else if (IsFirstRow) // for CSV format // line.IndexOf(",") != -1 && 
                    separator = ',';

                string[] nowCsvLineVals = { };
                nowCsvLineVals = line.Split(separator);

                for (int i = 0; i < nowCsvLineVals.Length; i++)
                {
                    string tempString = nowCsvLineVals[i].Replace("{}", ",");
                    nowCsvLineVals[i] = tempString;
                }

                if (!IsFirstRow) //Get new row for Datatable if its not the first line of CSV
                {
                    bool areAllValuesBlank = true;
                    string[] newCsvLineVals = new string[totalStringCol + 2];
                    int count = -1;
                    foreach (string tpIndex in headersIndexList)// string nowVal in nowCsvLineVals)
                    {
                        if (!string.IsNullOrEmpty(tpIndex) && tpIndex != "-1")
                        {
                            count++;
                            if (!string.IsNullOrEmpty(nowCsvLineVals[Int32.Parse(tpIndex)].Trim()))
                            {
                                areAllValuesBlank = false;
                                indexVal = -1;
                                int lastIndex = -1;
                                foreach (string index in headersIndexList)
                                {
                                    if (!string.IsNullOrEmpty(index))
                                    {
                                        indexVal++;
                                        if (index == "-1")
                                        {
                                            lastIndex++;
                                            newCsvLineVals[lastIndex] = nowCsvLineVals[indexVal];
                                        }
                                    }
                                }
                                lastIndex++;
                                newCsvLineVals[lastIndex] = timeperiods[count];
                                lastIndex++;
                                newCsvLineVals[lastIndex] = nowCsvLineVals[Int32.Parse(tpIndex)];
                            }
                            if (!areAllValuesBlank) dtResult.Rows.Add(newCsvLineVals);
                        }
                    }
                }
                else //If its CSV's first line then use that for creating ColumnNames of data table
                {
                    // Column Header                
                    indexVal = -1;
                    foreach (string index in headersIndexList)
                    {
                        if (!string.IsNullOrEmpty(index))
                        {
                            indexVal++;
                            if (index == "-1")
                            {
                                dtResult.Columns.Add(nowCsvLineVals[indexVal].Trim());
                                totalStringCol++;
                            }
                        }
                    }
                    dtResult.Columns.Add("Time Period");
                    dtResult.Columns.Add("Data Value");
                    timeperiods = new string[nowCsvLineVals.Length - totalStringCol];
                    indexVal = -1;
                    int timeperiodIndex = -1;
                    foreach (string index in headersIndexList)
                    {
                        if (!string.IsNullOrEmpty(index))
                        {
                            if (index != "-1")
                            {
                                timeperiodIndex++;
                                timeperiods[timeperiodIndex] = nowCsvLineVals[Int32.Parse(index)];
                            }
                        }
                    }
                    IsFirstRow = false;
                }
            }
            return dtResult;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return dtResult;
        }
    }
    private int getTimePeriodIndex(Dictionary<string, object> MapMatchingMetaData)
    {
        int tpIndex = 0;
        try
        {
            string[] headersIndexList = MapMatchingMetaData["tPeriosCol"].ToString().Split(new string[] { "," }, StringSplitOptions.None);
            foreach (string index in headersIndexList)
            {
                if (!string.IsNullOrEmpty(index))
                {
                    if (index == "-1")
                    {
                        tpIndex++;
                    }
                }
            }
            return tpIndex;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return tpIndex;
        }
    }
    private DataTable getMrdUpdatedTable(DataTable dtFromCsv, Dictionary<string, object> MapMatchingMetaData, List<int> NonNumericColumns)
    {
        DataTable dtResults;

        #region Sort the whole datatable to identify MRD of each subgroup (Sort TimePeriod column in last)

        List<int> GroupableColumns = NonNumericColumns;

        int indexOfTimePeriod = Convert.ToInt32(MapMatchingMetaData["tPeriosCol"]);
        GroupableColumns.Remove(indexOfTimePeriod);
        GroupableColumns.Sort();

        string SortedColumns = string.Empty;

        for (int i = 0; i < GroupableColumns.Count; i++)
        {
            SortedColumns += "," + dtFromCsv.Columns[GroupableColumns[i]].ColumnName;
        }

        if (!string.IsNullOrEmpty(SortedColumns)) SortedColumns = SortedColumns.Substring(1) +
            "," + dtFromCsv.Columns[indexOfTimePeriod].ColumnName;

        SortedColumns += " ASC";

        dtFromCsv.DefaultView.Sort = SortedColumns;

        List<DataTable> lstSortedGroups = new List<DataTable>();

        DataTable dtSortedAllGroups = dtFromCsv.DefaultView.ToTable();

        #endregion

        // String array to store temp values of each row of sorted table. It is used to identify starting of new group of records.
        string[] tmpTableValsHolder = new string[GroupableColumns.Count];

        for (int i = 0; i < tmpTableValsHolder.Length; i++) tmpTableValsHolder[i] = string.Empty;

        dtSortedAllGroups.Columns.Add("isMRD", typeof(bool));

        dtResults = dtSortedAllGroups.Clone();

        #region Break into sorted groups of datatables

        for (int currentRow = 0; currentRow < dtSortedAllGroups.Rows.Count; currentRow++)
        {
            DataRow nowRow = dtSortedAllGroups.Rows[currentRow];

            if (currentRow != 0)
            {
                bool isNewGroupRow = false;
                // Figure out whether new group's row has arrived or not
                for (int i = 0; i < tmpTableValsHolder.Length; i++)
                {
                    int tmpColIndex = GroupableColumns[i];
                    if (tmpTableValsHolder[i].ToLower() != dtSortedAllGroups.Rows[currentRow][tmpColIndex].ToString().ToLower()) // Different group's row
                    {
                        isNewGroupRow = true;
                        break;
                    }
                }


                if (isNewGroupRow) // Add new datatable with its first row as current row of master
                {
                    for (int i = 0; i < tmpTableValsHolder.Length; i++)
                    {
                        int tmpColIndex = GroupableColumns[i];
                        tmpTableValsHolder[i] = dtSortedAllGroups.Rows[currentRow][tmpColIndex].ToString();
                    }
                    DataTable dtNewGroup = dtSortedAllGroups.Clone();
                    dtNewGroup.TableName = "GroupOfRow" + currentRow.ToString();

                    dtNewGroup.ImportRow(nowRow);
                    lstSortedGroups.Add(dtNewGroup);

                }
                else // Keep appending current rows to Current DataTable object
                {
                    DataTable CurrentDataTable = lstSortedGroups[lstSortedGroups.Count - 1];
                    CurrentDataTable.ImportRow(nowRow);
                }

            }
            else // For first row of master table, update temp string array with first row value (without comparision)
            {
                for (int i = 0; i < tmpTableValsHolder.Length; i++)
                {
                    int tmpColIndex = GroupableColumns[i];
                    tmpTableValsHolder[i] = dtSortedAllGroups.Rows[currentRow][tmpColIndex].ToString();
                }
                DataTable dtNewGroup = dtSortedAllGroups.Clone();
                dtNewGroup.TableName = "GroupOfRow" + currentRow.ToString();
                dtNewGroup.ImportRow(nowRow);
                lstSortedGroups.Add(dtNewGroup);
            }
        }
        #endregion

        // Update MRD columns of each groups with appropiate flag and then append it to results table

        for (int i = 0; i < lstSortedGroups.Count; i++)
        {
            lstSortedGroups[i] = UpdateMRD(lstSortedGroups[i], indexOfTimePeriod);
            dtResults.Load(lstSortedGroups[i].CreateDataReader(), LoadOption.PreserveChanges);
        }

        return dtResults;

    }


    private DataTable UpdateMRD(DataTable dtBlankMRD, int indexOfTimePeriodCol)
    {
        string MRD = dtBlankMRD.Rows[dtBlankMRD.Rows.Count - 1][indexOfTimePeriodCol].ToString();

        foreach (DataRow dr in dtBlankMRD.Rows)
            dr["isMRD"] = (dr[indexOfTimePeriodCol].ToString() == MRD) ? true : false;

        return dtBlankMRD;
    }

    /// <summary>
    /// Return double quutes words list
    /// </summary>
    /// <param name="str">String</param>
    /// <returns>list of double quotes string</returns>
    private List<string> getDoubleQuoteString(string str)
    {
        string[] wordsArray = str.Split('"');
        List<string> correctWordList = new List<string>();
        int count = -1;
        foreach (string word in wordsArray)
        {
            count++;
            if (count % 2 != 0)
            {
                correctWordList.Add(word);
            }
        }
        return correctWordList;
    }



    /// <summary>
    /// Save xls file to temp location in server
    /// </summary>
    /// <param name="fileInputStream">Stream Object</param>
    /// <param name="xlsFilePath">xls file's path</param>
    /// <returns>true if successful else false</returns>
    private bool SaveXlsFileToTempLocation(Stream fileInputStream, string xlsFilePath)
    {
        bool isSaved = true;
        try
        {

            int length = 256;
            int bytesRead = 0;
            Byte[] buffer = new Byte[length];

            // write the required bytes
            using (FileStream fs = new FileStream(xlsFilePath, FileMode.Create))
            {
                do
                {
                    bytesRead = fileInputStream.Read(buffer, 0, length);
                    fs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead == length);
            }

            fileInputStream.Dispose();
        }
        catch (Exception ex)
        {
            isSaved = false;
            Global.CreateExceptionString(ex, null);

        }
        return isSaved;
    }


    /// <summary>
    /// convert xls format to csv format for OAT component
    /// </summary>
    /// <param name="xlsFilePath">xls file's path</param>
    /// <param name="csvFilePath">csv file's path</param>
    ///  <returns>true if successful else false</returns>
    private bool convertXLSToCSV(string xlsFilePath, string csvFilePath)
    {
        bool isConverted = true;
        xlsFilePath = xlsFilePath.Replace(@"\\", @"\");
        try
        {
            IWorkbook workBook = SpreadsheetGear.Factory.GetWorkbook(xlsFilePath);
            IWorksheet workSheet = workBook.Worksheets[0];
            string indicator = string.Empty;
            if (workSheet.Cells["B5"].Value != null)
            {
                indicator = workSheet.Cells["B5"].Value.ToString();
            }
            string unit = string.Empty;
            if (workSheet.Cells["B7"].Value != null)
            {
                unit = workSheet.Cells["B7"].Value.ToString();
            }
            if (workSheet.Cells["D9"].Value != null)
            {
                workSheet.Cells["D9"].Value = "Values";
            }
            // Delete rows
            workSheet.Cells.Range["1:8"].Delete();
            workSheet.Cells.Range["2:2"].Delete();

            // Delete area id column
            //workSheet.Cells.Range["B:B"].UnMerge();
            //workSheet.Cells.Range["B:B"].Delete();

            // Delete footnotes & Denominator columns
            workSheet.Cells.Range["F:X"].UnMerge();
            workSheet.Cells.Range["F:X"].Delete();

            // Add Column in worksheet
            IRange Range = workSheet.Range[0, 0, workSheet.Cells.Rows.Count - 1, 0];
            Range.Insert(InsertShiftDirection.Right);
            workSheet.Cells["A1"].Value = "Indicator";
            string source = workSheet.Cells["F2"].Value.ToString();
            bool isSingleSource = true;
            //indicator = indicator.Insert(indicator.Length, ", ");
            string newValue = indicator + ", " + unit;
            for (int rowNo = 2; rowNo <= workSheet.UsedRange.Rows.Count; rowNo++)
            {

                workSheet.Cells["A" + rowNo].NumberFormat = "@";
                workSheet.Cells["A" + rowNo].Value = newValue;
                if (rowNo != 2 && source != workSheet.Cells["F" + rowNo].Value.ToString() && isSingleSource == true)
                {
                    isSingleSource = false;
                }
                workSheet.Cells["B" + rowNo].NumberFormat = "@";
                workSheet.Cells["B" + rowNo].Value = workSheet.Cells["B" + rowNo].Text;

                //workSheet.Cells["B" + rowNo].Value = "'" + workSheet.Cells["B" + rowNo].Text + "'";
            }

            // Delete source column if more than 1 source
            if (!isSingleSource)
            {
                workSheet.Cells.Range["F:F"].UnMerge();
                workSheet.Cells.Range["F:F"].Delete();
            }
            workBook.SaveAs(csvFilePath, FileFormat.CSV);

            File.Delete(xlsFilePath);
        }
        catch (Exception ex)
        {
            isConverted = false;
            Global.CreateExceptionString(ex, null);

        }
        return isConverted;
    }


    #endregion
}

