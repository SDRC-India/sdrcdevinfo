using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SDMXObjectModel;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using System.Xml;
using System.IO;
using System.Text;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibSDMX;
using SDMXApi_2_0;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Web.Hosting;

public partial class libraries_aspx_SDMX_sdmx_StructuralMetadata : System.Web.UI.Page
{
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
   // protected string hdsdnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string hLoggedInUserName = string.Empty;
    protected string hOriginaldbnid = string.Empty;
    protected string hIsUploadedDSD = string.Empty;
    protected string hDsdChange = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name + " - Registry - " + Constants.Pages.Registry.StructuralMetadata;

            // Read http header or cookie values
            GetPostedData();
            bool IsKeyFound = false;
            string ProtectedValue = string.Empty;
            if (ConfigurationManager.AppSettings["AppProtected"] != null)
            {
                IsKeyFound = true;
                ProtectedValue = ConfigurationManager.AppSettings["AppProtected"];
            }
            if (IsKeyFound && ProtectedValue == "true" && Session["hLoggedInUserNId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                ((libraries_aspx_RegistryMaster)this.Master).Populate_Select_DSD_DropDown(hdbnid);
                ((libraries_aspx_RegistryMaster)this.Master).ShowHide_Select_DSD_DropDown(Global.enableDSDSelection);

                if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(hdbnid)) == true)
                {
                    //Binding Dimensions,Attributes and Measure by reading from the corresponding DSD.xml of schema version 2.0
                    BindDimensionsAttributesMeasure_For_Version_2_0();

                    BindDFDAndConceptScheme_For_Version_2_0();

                    //Binding Codelists(both Hierarchial and Non-Hierarchial) by reading from the corresponding Composite.xml of schema version 2.0
                    BindCodelists_For_Version_2_0();
                }
                else
                {
                    //Binding Dimensions,Attributes and Measure by reading from the corresponding DSD.xml of schema version 2.1
                    BindDimensionsAttributesMeasure_For_Version_2_1();

                    BindDFDAndConceptScheme_For_Version_2_1();

                    //Binding Codelists(both Hierarchial and Non-Hierarchial) by reading from the corresponding Composite.xml of schema version 2.0
                    BindCodelists_For_Version_2_1();
                }

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
        
        //if (!string.IsNullOrEmpty(Request["hdbnid"])) { hdbnid = Request["hdbnid"]; }

        //Set database NId - check in the posetd data
        if (!string.IsNullOrEmpty(Request["hdbnid"]))
        {
            // -- check in the posetd data first
            hdbnid = Request["hdbnid"];
            //if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            //{
            //    hdbnid = Global.GetDefaultDSDNId();
            //}           
        }
        else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value)))
        {
            // then check in the cookie
            hdbnid = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value;

            if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            {
               hdbnid = Global.GetDefaultDSDNId();
            }
        }
        else
        {
            // get default db nid ;
            hdbnid = Global.GetDefaultDbNId();

            if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            {
              hdbnid = Global.GetDefaultDSDNId();
              
            }
        }

        if (!Global.IsDbIdExists(hdbnid))
        {
            // get default db nid ;
            hdbnid = Global.GetDefaultDbNId();

            if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            {
               hdbnid = Global.GetDefaultDSDNId();
            }
        }

        Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId), hdbnid, Page);


        if (!string.IsNullOrEmpty(Request["hselarea"]))
        {
            hselarea = Request["hselarea"];
            Session["hselarea"] = Request["hselarea"];
        }
        else
        {
            hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
        }

        // Set Selected Indicators - check in the posetd data
        if (!string.IsNullOrEmpty(Request["hselind"]))
        {
            hselind = Request["hselind"];
            Session["hselind"] = Request["hselind"];
        }
        else
            hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

        //if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }

        // Set language code   
        if (!string.IsNullOrEmpty(Request["hlngcode"]))
        {
            // -- check in the posetd data first
            hlngcode = Request["hlngcode"];
        }
        else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
        {
            // then check in the cookie
            hlngcode = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
        }
        else
        {
            // get default lng code
            hlngcode = Global.GetDefaultLanguageCode();
        }
        Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode), hlngcode, Page);

        
        //if (!string.IsNullOrEmpty(Request["hlngcodedb"])) { hlngcodedb = Request["hlngcodedb"]; }

        if (!string.IsNullOrEmpty(Request["hlngcodedb"])) 
        { 
            hlngcodedb = Request["hlngcodedb"]; 
        }
        else
        {
            //hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid, hlngcode);
            hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);
        }

        if (!string.IsNullOrEmpty(Request["hselindo"]))
        {
            hselindo = Global.formatQuoteString(Request["hselindo"]);
            Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
        }
        else
            hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

        // Set selected area JSON object
        if (!string.IsNullOrEmpty(Request["hselareao"]))
        {
            hselareao = Global.formatQuoteString(Request["hselareao"]);
            Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
        }
        else
            hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"]))
        {
            hLoggedInUserNId = Request["hLoggedInUserNId"];
            Session["hLoggedInUserNId"] = Request["hLoggedInUserNId"];
        }
        else
            hLoggedInUserNId = Session["hLoggedInUserNId"] != null ? Session["hLoggedInUserNId"].ToString() : null;

        if (!string.IsNullOrEmpty(Request["hLoggedInUserName"]))
        {
            hLoggedInUserName = Request["hLoggedInUserName"];
            Session["hLoggedInUserName"] = Request["hLoggedInUserName"];
        }
        else
            hLoggedInUserName = Session["hLoggedInUserName"] != null ? Session["hLoggedInUserName"].ToString() : null;

        if (Request["hOriginaldbnid"] != null && !string.IsNullOrEmpty(Request["hOriginaldbnid"]))
        {
            hOriginaldbnid = Request["hOriginaldbnid"];
        }
        else
        {
            hOriginaldbnid = Global.GetDefaultDbNId();
        }

        if (Global.GetDefaultDSDNId() != string.Empty && hdbnid == Global.GetDefaultDSDNId())
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(Global.GetDefaultDSDNId())).ToString();
        }
        else
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(hdbnid)).ToString();
        }
    }

    private void BindDimensionsAttributesMeasure_For_Version_2_1()
    {
        XmlDocument SummaryXml;
        SummaryXml = new XmlDocument();
        int i, j, k;
        string DSDViewPath, DSDPath;
       
        StringBuilder sb;
        SDMXObjectModel.Structure.MeasureListType MeasureList;
        SDMXObjectModel.Structure.DimensionType Dimension;
        SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
        SDMXObjectModel.Structure.AttributeType Attribute;
        SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;
        SDMXObjectModel.Structure.AttributeRelationshipType AttributeRelationship;
        SDMXObjectModel.Common.LocalPrimaryMeasureReferenceType LocalPrimaryMeasureReference;
        SDMXObjectModel.Structure.PrimaryMeasureType PrimaryMeasure;

        SDMXObjectModel.Structure.StructuresType ConceptsObj;
         
        SDMXObjectModel.Structure.DataStructureComponentsType DSComponents = new DataStructureComponentsType();

        DSDViewPath = string.Empty;
        DSDPath = string.Empty;
       
       
        SDMXObjectModel.Message.StructureType Summary = new SDMXObjectModel.Message.StructureType();

        DSDViewPath = "../../" + Constants.FolderName.Data + hdbnid + "\\sdmx\\DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
        DSDPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

        //Providing the Path of the Summary.xml
        SummaryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

        //Loading Summary objects from its XML document into 'DataStructuresType' Summary object
        Summary = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), SummaryXml);

        DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(Summary.Structures.DataStructures[0].Item);
        ConceptsObj = Summary.Structures;

        //Binding DSD
        sb = new StringBuilder();
        sb.Append("<div class=\"reg_li_sub_txt\">");
        sb.Append("<b>");
        sb.Append(GetLangSpecificValue_For_Version_2_1(Summary.Structures.DataStructures[0].Name, hlngcodedb));
        sb.Append("</b>");
        sb.Append("<span class=\"reg_li_brac_txt\">(");
        sb.Append(GetLangSpecificValue_For_Version_2_1(Summary.Structures.DataStructures[0].Description, hlngcodedb));
        sb.Append(")</span> ");
        sb.Append("</div>");
        sb.Append("<div>");
        sb.Append("<a href=\" " + DSDViewPath.Replace("\\", "/") + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + DSDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("</div>");
        sb.Append("<br/>");

        //Binding Dimensions  

        sb.Append("<h4 id=\"lang_Dimensions\"></h4>");
        sb.Append("<ul>");
        for (i = 0; i < DSComponents.Items[0].Items.Count; i++)
        {

            if (DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.TimeDimensionType)
            {
                TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSComponents.Items[0].Items[i]);
                ConceptIdentity = TimeDimension.ConceptIdentity;
            }
            else
            {
                Dimension = (SDMXObjectModel.Structure.DimensionType)(DSComponents.Items[0].Items[i]);
                ConceptIdentity = Dimension.ConceptIdentity;
            }

            sb.Append("<li>");
            for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                {

                    for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                    {
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                        {
                            sb.Append("<div>");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Name, hlngcodedb));
                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Description, hlngcodedb));
                            sb.Append(")</span>");
                            sb.Append("<div>");

                            break;
                        }
                    }

                }
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divDimensions.InnerHtml = sb.ToString();

        //Binding Attributes

        sb = new StringBuilder();
        sb.Append("<h4 id=\"lang_Attributes\"></h4>");
        sb.Append("<ul>");
        for (i = 0; i < DSComponents.Items[1].Items.Count; i++)
        {
            Attribute = (SDMXObjectModel.Structure.AttributeType)(DSComponents.Items[1].Items[i]);
            ConceptIdentity = Attribute.ConceptIdentity;
            AttributeRelationship = Attribute.AttributeRelationship;
            LocalPrimaryMeasureReference = (SDMXObjectModel.Common.LocalPrimaryMeasureReferenceType)(AttributeRelationship.Items[0]);
            sb.Append("<li>");
            for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                {
                    for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                    {
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                        {
                            sb.Append("<div>");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Name, hlngcodedb));
                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Description, hlngcodedb));
                            sb.Append(")</span>");
                            sb.Append("</div>");
                            break;
                        }
                    }
                }
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("<span id=\"lang_Attachment_Level" + i + "\" ></span>");
            sb.Append("<span id=\"lang_Obs_Val" + i + "\">");
            sb.Append(((SDMXObjectModel.Common.LocalPrimaryMeasureRefType)(LocalPrimaryMeasureReference.Items[0])).id.ToString());
            sb.Append(" , ");
            sb.Append("</span>");
            sb.Append("<span id=\"lang_Mandatory" + i + "\" ></span>");
            if (((UsageStatusType)(Attribute.assignmentStatus)) == UsageStatusType.Mandatory)
            {
                sb.Append("<span id=\"lang_Yes" + i + "\" ></span>");
            }
            else
            {
                sb.Append("<span id=\"lang_No" + i + "\" ></span>");
            }
            sb.Append("</div>");
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divAttributes.InnerHtml = sb.ToString();

        //Binding Measure

        MeasureList = ((SDMXObjectModel.Structure.MeasureListType)((SDMXObjectModel.Structure.DataStructureComponentsType)(DSComponents)).MeasureList);
        sb = new StringBuilder();
        sb.Append("<h4 id=\"lang_Measure\"></h4>");
        sb.Append("<ul>");
        for (i = 0; i < DSComponents.Items[2].Items.Count; i++)
        {

            PrimaryMeasure = (SDMXObjectModel.Structure.PrimaryMeasureType)(DSComponents.Items[2].Items[i]);
            ConceptIdentity = PrimaryMeasure.ConceptIdentity;
            sb.Append("<li>");
            for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).maintainableParentID.ToString() == ConceptsObj.Concepts[j].id.ToString())
                {

                    for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                    {
                        if (((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                        {
                            sb.Append("<div>");
                            sb.Append("<span style=\"font-weight:normal\">");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Name, hlngcodedb));
                            sb.Append("</span>");

                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Description, hlngcodedb));
                            sb.Append(")</span>");
                            sb.Append("<div>");
                            break;
                        }
                    }

                }
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divMeasure.InnerHtml = sb.ToString();
    }

    private void BindDFDAndConceptScheme_For_Version_2_1()
    {
        string DFDViewPath, DFDPath;
        string ConceptSchemeViewPath, ConceptSchemePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        ConceptSchemeViewPath = string.Empty;
        ConceptSchemePath = string.Empty;

        sb = new StringBuilder();
        
        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(hdbnid) + " AND Type=4;";
        DtDFD = DIConnection.ExecuteDataTable(Query);

        if (DtDFD != null && DtDFD.Rows.Count > 0)
        {
            DFDPath = DtDFD.Rows[0]["FileLocation"].ToString();
            DFDViewPath = "../../" + DFDPath.Substring(DFDPath.LastIndexOf("stock")).Replace("\\", "/");

        }
        else
        {
            DFDPath = string.Empty;
            DFDViewPath = string.Empty;
        }

       
        SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
        DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);

        sb = new StringBuilder();
        sb.Append("<div class=\"reg_li_sub_txt\">");
        sb.Append("<b>");
        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, hlngcodedb));
        sb.Append("</b>");
        sb.Append("<span class=\"reg_li_brac_txt\">(");
        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Description, hlngcodedb));
        sb.Append(")</span> ");
        sb.Append("</div>");
        sb.Append("<div>");
        sb.Append("<a href=\" " + DFDViewPath + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + DFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("</div>");

        divDFD.InnerHtml = sb.ToString();

        ConceptSchemeViewPath = "../../" + Constants.FolderName.Data + hdbnid + "\\sdmx\\Concepts\\DSD_Concepts" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
        ConceptSchemePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Concepts\\DSD_Concepts" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

        SDMXObjectModel.Message.StructureType ConceptScheme = new SDMXObjectModel.Message.StructureType();
        ConceptScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConceptSchemePath);

        sb = new StringBuilder();
        sb.Append("<div class=\"reg_li_sub_txt\">");
        sb.Append("<b>");
        sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptScheme.Structures.Concepts[0].Name, hlngcodedb));
        sb.Append("</b>");
        sb.Append("<span class=\"reg_li_brac_txt\">(");
        sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptScheme.Structures.Concepts[0].Description, hlngcodedb));
        sb.Append(")</span> ");
        sb.Append("</div>");
        sb.Append("<div>");
        sb.Append("<a href=\" " + ConceptSchemeViewPath.Replace("\\", "/") + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + ConceptSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("<br/>");
        sb.Append("<br/>");
        sb.Append("<ul id=\"ulConceptScheme\">");
        for (i = 0; i < ConceptScheme.Structures.Concepts[0].Items.Count; i++)
        {
            sb.Append("<li>");
        
            sb.Append("<span>");
            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptScheme.Structures.Concepts[0].Items[i].Name, hlngcodedb));
            sb.Append("</span>");

            sb.Append("<span class=\"reg_li_brac_txt\">(");
            sb.Append(GetLangSpecificValue_For_Version_2_1(ConceptScheme.Structures.Concepts[0].Items[i].Description, hlngcodedb));
            sb.Append(")</span>");
         
            sb.Append("</li>");
          
        }
        sb.Append("</ul>");
        sb.Append("</div>");

        divConceptScheme.InnerHtml = sb.ToString();
    }

    private void BindCodelists_For_Version_2_1()
    {
        XmlDocument SummaryXml;
        SummaryXml = new XmlDocument();
        string CodelistPath;
        string CodelistViewPath;
        int i;
        StringBuilder sb;

      
        SummaryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

        SDMXObjectModel.Message.StructureType Summary = new SDMXObjectModel.Message.StructureType();

        //Loading Composite Xml into 'StructureType' Composite object
        Summary = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), SummaryXml);

        //Binding Codelists

        sb = new StringBuilder();
        sb.Append("<ul>");
        for (i = 0; i < Summary.Structures.Codelists.Count; i++)
        {
            sb.Append("<li>");
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("<b>");
            sb.Append(GetLangSpecificValue_For_Version_2_1(Summary.Structures.Codelists[i].Name, hlngcodedb));
            sb.Append("</b>");
            sb.Append("<span class=\"reg_li_brac_txt\">(");
            sb.Append(GetLangSpecificValue_For_Version_2_1(Summary.Structures.Codelists[i].Description, hlngcodedb));
            sb.Append(")</span> ");
            sb.Append("</div>");

            switch (Summary.Structures.Codelists[i].id)
            {
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Area.FileName;
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.FileName;
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.IUS.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.IUS.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.IUS.FileName;
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupType.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupType.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupType.FileName;
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.SubgroupVal.FileName;
                    break;
                case DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Id:
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.FileName;
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.FileName;
                    break;
                default:
                    //for subgroups
                    CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + Summary.Structures.Codelists[i].id.Substring(3) + ".xml";
                    CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + Summary.Structures.Codelists[i].id.Substring(3) + ".xml";
                    break;
            }


            sb.Append("<div>");
            sb.Append("<a href=\"javascript:void(0);\" ");
            sb.Append(" onclick=\"ViewCodelist('" + CodelistViewPath + "');\"");
            sb.Append(" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");

            //Download Codelist
            sb.Append("<a class=\"reg_li_link_txt\" href='Download.aspx?fileId=" + CodelistPath + "' name=\"lang_Download\"></a>");
            sb.Append("</div>");
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divCodelists.InnerHtml = sb.ToString();
    }

    private string GetLangSpecificValue_For_Version_2_1(List<SDMXObjectModel.Common.TextType> ListOfValues, string LangCode)
    {
        string Retval = string.Empty;
        foreach (SDMXObjectModel.Common.TextType ObjectValue in ListOfValues)
        {
            if (ObjectValue.lang.ToString() == LangCode)
            {
                Retval = ObjectValue.Value.ToString();
                break;
            }
        }
        if (Retval == string.Empty)
        {
            Retval = ListOfValues[0].Value.ToString();
        }
        return Retval;

    }

    private void BindDimensionsAttributesMeasure_For_Version_2_0()
    {
        XmlDocument SummaryXml;
        string DSDViewPath, DSDPath;
        SummaryXml = new XmlDocument();
        int i, j;
        StringBuilder sb;
        SDMXApi_2_0.Message.StructureType SummaryStructure = new SDMXApi_2_0.Message.StructureType();
        string AttributeImportance = string.Empty;

        //Providing the Path of the Summary.xml
        SummaryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

        //Loading Summary objects from its XML document into 'DataStructuresType' Summary object
        SummaryStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), SummaryXml);

        DSDViewPath = "../../" + Constants.FolderName.Data + hdbnid + "\\sdmx\\DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
        DSDPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\DSD" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

        //Binding DSD
        sb = new StringBuilder();
        sb.Append("<div class=\"reg_li_sub_txt\">");
        sb.Append("<b>");
        sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.KeyFamilies[0].Name, hlngcodedb));
        sb.Append("</b>");
        if (SummaryStructure.KeyFamilies[0].Description.Count > 0)
        {
            sb.Append("<span class=\"reg_li_brac_txt\">(");
            sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.KeyFamilies[0].Description, hlngcodedb));
            sb.Append(")</span> ");
        }
        sb.Append("</div>");
        sb.Append("<div>");
        sb.Append("<a href=\" " + DSDViewPath.Replace("\\", "/") + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + DSDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("</div>");
        sb.Append("<br/>");

        //Binding Dimensions  

        sb.Append("<h4 id=\"lang_Dimensions\"></h4>");
        sb.Append("<ul>");

        for (i = 0; i < SummaryStructure.KeyFamilies[0].Components.Dimension.Count; i++)
        {
            sb.Append("<li>");
            for (j = 0; j < SummaryStructure.Concepts.Concept.Count; j++)
            {
                if (SummaryStructure.Concepts.Concept[j].id == SummaryStructure.KeyFamilies[0].Components.Dimension[i].conceptRef)
                {
                    sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Name, hlngcodedb));
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Description, hlngcodedb));
                    sb.Append(")</span> ");
                    break;
                }
            }
            sb.Append("</li>");

        }

        //Binding Time Dimension  
       
        for (j = 0; j < SummaryStructure.Concepts.Concept.Count; j++)
        {
            if (SummaryStructure.Concepts.Concept[j].id == SummaryStructure.KeyFamilies[0].Components.TimeDimension.conceptRef)
            {
                sb.Append("<li>");
                sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Name, hlngcodedb));
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Description, hlngcodedb));
                sb.Append(")</span> ");
                sb.Append("</li>");
                break;
            }
        }       
        sb.Append("</ul>");
        divDimensions.InnerHtml = sb.ToString();

        //Binding Attributes

        sb = new StringBuilder();
        sb.Append("<h4 id=\"lang_Attributes\"></h4>");
        sb.Append("<ul>");
       


        for (i = 0; i < SummaryStructure.KeyFamilies[0].Components.Attribute.Count; i++)
        {
            sb.Append("<li>");
            for (j = 0; j < SummaryStructure.Concepts.Concept.Count; j++)
            {
                if (SummaryStructure.Concepts.Concept[j].id == SummaryStructure.KeyFamilies[0].Components.Attribute[i].conceptRef)
                {
                    sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Name, hlngcodedb));
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Description, hlngcodedb));
                    sb.Append(")</span>");
                    break;
                }
            }
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("Attachment Level : ");
            sb.Append(SummaryStructure.KeyFamilies[0].Components.Attribute[i].attachmentLevel);
            sb.Append(" , Mandatory : ");
            if (SummaryStructure.KeyFamilies[0].Components.Attribute[i].assignmentStatus == SDMXApi_2_0.Structure.AssignmentStatusType.Mandatory)
            {
                sb.Append("Yes");
            }
            else
            {
                sb.Append("No");
            }
            sb.Append("</div>");
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divAttributes.InnerHtml = sb.ToString();

        //Binding Measure  
        sb = new StringBuilder();
        sb.Append("<h4 id=\"lang_Measure\"></h4>");
        sb.Append("<ul>");
        for (j = 0; j < SummaryStructure.Concepts.Concept.Count; j++)
        {
            if (SummaryStructure.Concepts.Concept[j].id == SummaryStructure.KeyFamilies[0].Components.PrimaryMeasure.conceptRef)
            {
                sb.Append("<li>");
                sb.Append( GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Name, hlngcodedb));
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValue_For_Version_2_0(SummaryStructure.Concepts.Concept[j].Description, hlngcodedb));
                sb.Append(")</span>");
                sb.Append("</li>");
                break;
            }
        }
        sb.Append("</ul>");
        divMeasure.InnerHtml = sb.ToString();

    }

    private void BindDFDAndConceptScheme_For_Version_2_0()
    {
        string DFDViewPath, DFDPath;
        string ConceptSchemeViewPath, ConceptSchemePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        ConceptSchemeViewPath = string.Empty;
        ConceptSchemePath = string.Empty;

        sb = new StringBuilder();

        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(hdbnid) + " AND Type=4;";
        DtDFD = DIConnection.ExecuteDataTable(Query);

        if (DtDFD != null && DtDFD.Rows.Count > 0)
        {
            DFDPath = DtDFD.Rows[0]["FileLocation"].ToString();
            DFDViewPath = "../../" + DFDPath.Substring(DFDPath.LastIndexOf("stock")).Replace("\\", "/");

        }
        else
        {
            DFDPath = string.Empty;
            DFDViewPath = string.Empty;
        }


        SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
        DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);

        sb = new StringBuilder();
        sb.Append("<div class=\"reg_li_sub_txt\">");
        sb.Append("<b>");
        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, hlngcodedb));
        sb.Append("</b>");
        sb.Append("<span class=\"reg_li_brac_txt\">(");
        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Description, hlngcodedb));
        sb.Append(")</span> ");
        sb.Append("</div>");
        sb.Append("<div>");
        sb.Append("<a href=\" " + DFDViewPath + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + DFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("</div>");

        divDFD.InnerHtml = sb.ToString();

        ConceptSchemeViewPath = "../../" + Constants.FolderName.Data + hdbnid + "\\sdmx\\Concepts\\DSD_Concepts" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension;
        ConceptSchemePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Concepts\\DSD_Concepts" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);

        SDMXApi_2_0.Message.StructureType ConceptScheme = new SDMXApi_2_0.Message.StructureType();
        ConceptScheme = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), ConceptSchemePath);

        sb = new StringBuilder();
        sb.Append("<div>");
        sb.Append("<a href=\" " + ConceptSchemeViewPath.Replace("\\", "/") + "\"  ");
        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
        sb.Append("<a href='Download.aspx?fileId=" + ConceptSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
        sb.Append("</div>");
        sb.Append("<br/>");
        sb.Append("<ul id=\"ulConceptScheme\">");
        for (i = 0; i < ConceptScheme.Concepts.Concept.Count; i++)
        {
            sb.Append("<li>");

            sb.Append("<span>");
            sb.Append(GetLangSpecificValue_For_Version_2_0(ConceptScheme.Concepts.Concept[i].Name, hlngcodedb));
            sb.Append("</span>");

            sb.Append("<span class=\"reg_li_brac_txt\">(");
            sb.Append(GetLangSpecificValue_For_Version_2_0(ConceptScheme.Concepts.Concept[i].Description, hlngcodedb));
            sb.Append(")</span>");

            sb.Append("</li>");

        }
        sb.Append("</ul>");
        divConceptScheme.InnerHtml = sb.ToString();
    }

    private void BindCodelists_For_Version_2_0()
    {
        XmlDocument SummaryXml;
        SummaryXml = new XmlDocument();
        string CodelistPath;
        string CodelistViewPath;
        int i;
        string CodelistName;
        StringBuilder sb;

        //Providing the Path of the Summary XML

        SummaryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

        SDMXApi_2_0.Message.StructureType Summary = new SDMXApi_2_0.Message.StructureType();

        //Loading Summary Xml into 'StructureType' Composite object
        Summary = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), SummaryXml);

        //Binding Codelists

        sb = new StringBuilder();
        sb.Append("<ul>");
        for (i = 0; i < Summary.CodeLists.Count; i++)
        {
            sb.Append("<li>");
            CodelistName = GetLangSpecificValue_For_Version_2_0(Summary.CodeLists[i].Name, hlngcodedb);
            sb.Append(CodelistName);
            if (Summary.CodeLists[i].Description.Count > 0)
            {
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValue_For_Version_2_0(Summary.CodeLists[i].Description, hlngcodedb));
                sb.Append(")</span>");
            }
            CodelistViewPath = "/data/" + hdbnid + "/sdmx/Codelists/" + Summary.CodeLists[i].Name[0].Value + ".xml";
            CodelistPath = "stock/data/" + hdbnid + "/sdmx/Codelists/" + Summary.CodeLists[i].Name[0].Value + ".xml";
            sb.Append("<div>");
            sb.Append("<a href=\"javascript:void(0);\" ");
            sb.Append(" onclick=\"ViewCodelist('" + CodelistViewPath + "');\"");
            sb.Append(" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");

            //Download Codelist
            sb.Append("<a class=\"reg_li_link_txt\" href='Download.aspx?fileId=" + CodelistPath + "' name=\"lang_Download\"></a>");
            sb.Append("</div>");
            sb.Append("</li>");
        }
        sb.Append("</ul>");
        divCodelists.InnerHtml = sb.ToString();
    }

    private string GetLangSpecificValue_For_Version_2_0(List<SDMXApi_2_0.Common.TextType> ListOfValues, string LangCode)
    {
        string Retval = string.Empty;
        foreach (SDMXApi_2_0.Common.TextType ObjectValue in ListOfValues)
        {
            if (ObjectValue.lang.ToString() == LangCode)
            {
                Retval = ObjectValue.Value.ToString();
                break;
            }
        }
        if (Retval == string.Empty)
        {
            if (ListOfValues.Count > 0)
            {
                Retval = ListOfValues[0].Value.ToString();
            }
        }
        return Retval;

    }

}
