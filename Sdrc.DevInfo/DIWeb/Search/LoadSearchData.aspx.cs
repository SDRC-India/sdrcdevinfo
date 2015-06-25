using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections;

public partial class search_LoadSearchData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string QueryString = string.Empty;
        string MetaValues = string.Empty;      
        string IndicatorName = string.Empty;
        DIConnection ObjDIConnection = null;
        string AreaName = string.Empty;
        string IndicatorNID = "";
        string AreaNId = "";
        string LanguageCode = "en";
        DataTable DTIndicatorDetails = null;
        DataTable DTAreaDetails = null;
        try
        {
            if (Request.QueryString.Count > 0)
            {
                try
                {                   
                    if (Request.QueryString["category"] != null)
                    {
                        string[] QueryStringArr = Request.QueryString["category"].ToString().Split(new string[] { "/" }, StringSplitOptions.None);
                        //string[] QueryStringArr = Request.QueryString["category"].ToString().Split(new string[] { "/" }, StringSplitOptions.None);
                        if (QueryStringArr[1].ToString().Trim().ToLower() == "s")
                        {
                            try
                            {
                                IndicatorName = QueryStringArr[3].ToString();
                                AreaName = QueryStringArr[4].ToString();
                                LanguageCode = QueryStringArr[0].ToString();

                                MakeResponseURL(ref QueryString, ref MetaValues, IndicatorName, ref ObjDIConnection, AreaName, ref IndicatorNID, ref AreaNId, LanguageCode, ref DTIndicatorDetails, ref DTAreaDetails);
                            }
                            catch (Exception Ex)
                            {
                                Global.CreateExceptionString(Ex, null);
                                Response.Redirect("../libraries/aspx/Home.aspx", false);
                            }
                        }
                        else if (QueryStringArr.Length == 3)
                        {
                            IndicatorName = QueryStringArr[1].ToString();
                            AreaName = QueryStringArr[2].ToString();
                            LanguageCode = QueryStringArr[0].ToString();
                            MakeResponseURL(ref QueryString, ref MetaValues, IndicatorName, ref ObjDIConnection, AreaName, ref IndicatorNID, ref AreaNId, LanguageCode, ref DTIndicatorDetails, ref DTAreaDetails);
                        }
                        else
                        {
                            Global.WriteErrorsInLogFolder("SiteMap Link : QueryString is not in correct format");
                            Response.Redirect("../libraries/aspx/Home.aspx", false);
                        }
                    }
                    else if (Request.QueryString["lngCode"] != null)
                    {
                        Response.Redirect("../libraries/aspx/Home.aspx?lngCode=" + Request.QueryString["lngCode"].ToString(), false);
                    }
                    else
                    {
                        Global.WriteErrorsInLogFolder("SiteMap Link : Required QueryString not found");
                        Response.Redirect("../libraries/aspx/Home.aspx", false);
                    }
                }
                catch (Exception Ex)
                {
                    Global.CreateExceptionString(Ex, null);
                    Response.Redirect("../libraries/aspx/Home.aspx", false);
                }
            }
            else
            {
                Global.WriteErrorsInLogFolder("SiteMap Link : QueryString not found");
                Response.Redirect("../libraries/aspx/Home.aspx", false);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            Response.Redirect("libraries/aspx/Home.aspx", false);            
        }
    }

    private void MakeResponseURL(ref string QueryString, ref string MetaValues, string IndicatorName, ref DIConnection ObjDIConnection, string AreaName, ref string IndicatorNID, ref string AreaNId, string LanguageCode, ref DataTable DTIndicatorDetails, ref DataTable DTAreaDetails)
    {
        //Get Application Database Connection object
        ObjDIConnection = Global.GetDbConnection(Convert.ToInt32(Global.GetDefaultDbNId()));
        ArrayList ALIndicator = new ArrayList();
        ArrayList ALArea = new ArrayList();
        string[] IndicatorNameArray = IndicatorName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        IndicatorName = string.Empty;
        int IndCount = 1;
        bool LinkMade = false;
        for (int i = 0; i < IndicatorNameArray.Length; i++)
        {
            //Get the NIds of Indicator from Database
            DTIndicatorDetails = ObjDIConnection.ExecuteDataTable("SELECT Indicator_NId,Indicator_Name from " + ObjDIConnection.DIDataSetDefault() + "Indicator_" + LanguageCode + " WHERE Indicator_Name =N'" + IndicatorNameArray[i].ToString() + "'");
            if (DTIndicatorDetails.Rows.Count > 0)
            {                                                   
                MakeIndicatorString(ref IndicatorName, ref IndicatorNID, DTIndicatorDetails, ALIndicator,ref IndCount);
                LinkMade = true;
            }
            else
            {
                DTIndicatorDetails = ObjDIConnection.ExecuteDataTable("SELECT Indicator_NId,Indicator_Name from " + ObjDIConnection.DIDataSetDefault() + "Indicator_" + LanguageCode + " WHERE Indicator_Name like N'%" + IndicatorNameArray[i].ToString() + "%'");
                if (DTIndicatorDetails.Rows.Count > 0)
                {
                    MakeIndicatorString(ref IndicatorName, ref IndicatorNID, DTIndicatorDetails, ALIndicator, ref IndCount);
                    LinkMade = true;
                }
                else
                {
                    DTIndicatorDetails = ObjDIConnection.ExecuteDataTable("SELECT Indicator_NId,Indicator_Name from " + ObjDIConnection.DIDataSetDefault() + "Indicator_" + LanguageCode + " WHERE SOUNDEX(Indicator_Name) = SOUNDEX(N'" + IndicatorNameArray[i].ToString() + "')");
                    if (DTIndicatorDetails.Rows.Count > 0)
                    {
                        MakeIndicatorString(ref IndicatorName, ref IndicatorNID, DTIndicatorDetails, ALIndicator, ref IndCount);
                        LinkMade = true;
                    }
                }
            }
        }
        if (LinkMade == false)
        {
            Global.WriteErrorsInLogFolder("SiteMap Link : Searched Indicator is not found in database: IndicatorName = " + IndicatorName);
            Response.Redirect("../libraries/aspx/Home.aspx", false);
        }

        string[] AreaNameArray = AreaName.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
        AreaName = string.Empty;
        int AreaCount = 1;
        LinkMade = false;
        for (int i = 0; i < AreaNameArray.Length; i++)
        {
            //Get the NIds of Area from Database
            DTAreaDetails = ObjDIConnection.ExecuteDataTable("SELECT Area_NId,Area_Name from " + ObjDIConnection.DIDataSetDefault() + "Area_" + LanguageCode + " WHERE Area_Name =N'" + AreaNameArray[i].ToString() + "'");
            if (DTAreaDetails.Rows.Count > 0)
            {                                
                MakeAreaString(ref AreaName, ref AreaNId, DTAreaDetails, ALArea,ref AreaCount);
                LinkMade = true;
            }
            else
            {
                DTAreaDetails = ObjDIConnection.ExecuteDataTable("SELECT Area_NId,Area_Name from " + ObjDIConnection.DIDataSetDefault() + "Area_" + LanguageCode + " WHERE Area_Name like N'%" + AreaNameArray[i].ToString() + "%'");
                if (DTAreaDetails.Rows.Count > 0)
                {
                    MakeAreaString(ref AreaName, ref AreaNId, DTAreaDetails, ALArea, ref AreaCount);
                    LinkMade = true;
                }
                else
                {
                    DTAreaDetails = ObjDIConnection.ExecuteDataTable("SELECT Area_NId,Area_Name from " + ObjDIConnection.DIDataSetDefault() + "Area_" + LanguageCode + " WHERE SOUNDEX(Area_Name) = SOUNDEX(N'" + AreaNameArray[i].ToString() + "')");
                    if (DTAreaDetails.Rows.Count > 0)
                    {
                        MakeAreaString(ref AreaName, ref AreaNId, DTAreaDetails, ALArea, ref AreaCount);
                        LinkMade = true;
                    }                   
                }
            } 
        }
        if (LinkMade == false)
        {
            Global.WriteErrorsInLogFolder("SiteMap Link : Searched Area is not found in database: AreaName = " + AreaName);
            Response.Redirect("../libraries/aspx/Home.aspx", false);
        }

        //Make the QueryString
        QueryString = "refer_url=catalog&jsonAreasTopics={%22apn%22:%22" + "1" + "%22,%22i%22:%22" + IndicatorName + "%22,%22i_n%22:%22" + IndicatorNID + "%22,%22a%22:%22" + AreaName + "%22,%22a_n%22:%22" + AreaNId + "%22,%22lngCode%22:%22" + LanguageCode + "%22}";

        //Generate MetaData Values and redirect the page
        MetaValues = Global.GenerateMetaValues(QueryString.Replace("%22", "\"").Replace("%20", " "));
        Page.Title = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[0].ToString();
        Page.MetaKeywords = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[1].ToString();
        Page.MetaDescription = MetaValues.Split(new string[] { "[**]" }, StringSplitOptions.None)[2].ToString();
        Response.Redirect("../libraries/aspx/Home.aspx?" + QueryString, false);
    }

    private static void MakeAreaString(ref string AreaName, ref string AreaNId, DataTable DTAreaDetails, ArrayList ALArea, ref int Count)
    {        
        foreach (DataRow dr in DTAreaDetails.Rows)
        {
            if (Count <= 5)
            {
                if (!ALArea.Contains(dr["Area_NId"].ToString()))
                {
                    ALArea.Add(dr["Area_NId"].ToString());
                    if (string.IsNullOrEmpty(AreaNId.Trim()))
                    {
                        AreaNId = dr["Area_NId"].ToString();
                        AreaName = dr["Area_Name"].ToString();
                    }
                    else
                    {
                        AreaNId = AreaNId + "[****]" + dr["Area_NId"].ToString();
                        AreaName = AreaName + "[****]" + dr["Area_Name"].ToString();
                    }
                    Count++;
                }                
            }
            else
            {
                break;
            }
        }
    }

    private static void MakeIndicatorString(ref string IndicatorName, ref string IndicatorNID, DataTable DTIndicatorDetails, ArrayList ALIndicator, ref int Count)
    {        
        foreach (DataRow dr in DTIndicatorDetails.Rows)
        {
            if (Count <= 5)
            {
                if (!ALIndicator.Contains(dr["Indicator_NId"].ToString()))
                {
                    ALIndicator.Add(dr["Indicator_NId"].ToString());
                    if (string.IsNullOrEmpty(IndicatorNID.Trim()))
                    {
                        IndicatorNID = dr["Indicator_NId"].ToString();
                        IndicatorName = dr["Indicator_Name"].ToString();
                    }
                    else
                    {
                        IndicatorNID = IndicatorNID + "[****]" + dr["Indicator_NId"].ToString();
                        IndicatorName = IndicatorName + "[****]" + dr["Indicator_Name"].ToString();
                    }
                    Count++;
                }                
            }
            else
            {
                break;
            }
        }
    }
}