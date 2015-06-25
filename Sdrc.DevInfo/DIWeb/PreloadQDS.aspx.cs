using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreloadQDS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Check for Valid Query String Values 
        //Modify the Query String\
        //Call the Home page with these modified values
        string QueryString = string.Empty;
        try
        {
            if (Request.QueryString.Count > 0)
            {
                try
                {
                    //QueryString = Request.QueryString.ToString().Replace("&amp;", "&").Replace("&amp%3b", "&");
                    if (Request.QueryString["jsonAreasTopics"] != null)
                    {
                        QueryString = Request.QueryString["jsonAreasTopics"].ToString();
                        //Modify the Query string
                        //QueryString = "catalog&jsonAreasTopics=%7Bapn:55,i:Population density,i_n:237,a:India,a_n:18274%7D";
                        QueryString = QueryString.Replace("1234-", "{").Replace("-4321", "}").Replace("&amp;", "&");
                        string MetaValues = Global.GenerateMetaValues(QueryString);
                        Page.Title = MetaValues.Split(new string[] { "[****]" }, StringSplitOptions.None)[0].ToString();
                        Page.MetaKeywords = MetaValues.Split(new string[] { "[****]" }, StringSplitOptions.None)[1].ToString();
                        Page.MetaDescription = MetaValues.Split(new string[] { "[****]" }, StringSplitOptions.None)[2].ToString();
                        Response.Redirect("libraries/aspx/Home.aspx?refer_url=catalog&jsonAreasTopics=" + QueryString, false);
                    }
                    else if (Request.QueryString["lngCode"] != null)
                    {
                        Response.Redirect("libraries/aspx/Home.aspx?lngCode=" + Request.QueryString["lngCode"].ToString(), false);
                    }
                    else
                    {
                        Response.Redirect("libraries/aspx/Home.aspx");
                    }
                }
                catch (Exception ex)
                {
                    Response.Redirect("libraries/aspx/Home.aspx");
                }
            }
            else
            {
                Response.Redirect("libraries/aspx/Home.aspx");
            }
        }
        catch (Exception)
        {
            Response.Redirect("libraries/aspx/Home.aspx");
        }        
    }
}