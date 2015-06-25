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

    private string GetRegistrationsAsPerDFD(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string DFDViewPath, DFDPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerDFD;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        CompleteFilePath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        RegistrationsPerDFD = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                for (i = 0; i < Complete.Structures.Dataflows.Count; i++)
                {
                    Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 AND Id='" + Complete.Structures.Dataflows[i].id + "';";
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

                    sb.Append("<div>");
                    sb.Append("<img id=\"imgdivDFD_" + Complete.Structures.Dataflows[i].id + "\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivDFD_" + Complete.Structures.Dataflows[i].id + "' ,'divDFD_" + Complete.Structures.Dataflows[i].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");//expand.png
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[i].Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[i].Description, LanguageCode));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + DFDViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + DFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<div id=\"divDFD_" + Complete.Structures.Dataflows[i].id + "\" style=\"margin-left: 20px;overflow:auto;height:auto;max-height:200px;\" >");//display:none;
                    RegistrationsPerDFD = BindRegistrationsPerDFD(DbNId, LanguageCode, Complete.Structures.Dataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerDFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerDFD);
                        sb.Append("</table>");
                    }

                    sb.Append("</div>");
                }
            }           
            
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerDFDFor2_0(string DbNId, string LanguageCode) 
    {
        string RetVal;
        RetVal = string.Empty;

        string DFDViewPath, DFDPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerDFD;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        RegistrationsPerDFD = string.Empty;

        try
        {
            SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
              Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 ;";
              DtDFD = DIConnection.ExecuteDataTable(Query);

                if (DtDFD != null && DtDFD.Rows.Count > 0)
                {
                    for (i = 0; i < DtDFD.Rows.Count; i++)
                    {
                      
                        DFDPath = DtDFD.Rows[i]["FileLocation"].ToString();
                        DFDViewPath = "../../" + DFDPath.Substring(DFDPath.LastIndexOf("stock")).Replace("\\", "/");
                        DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                        sb.Append("<div>");
                        sb.Append("<img id=\"imgdivDFD_" + DFD.Structures.Dataflows[0].id + "\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivDFD_" + DFD.Structures.Dataflows[0].id + "' ,'divDFD_" + DFD.Structures.Dataflows[0].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");//expand.png
                        sb.Append("<div class=\"reg_li_sub_txt\">");
                        sb.Append("<b>");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, LanguageCode));
                        sb.Append("</b>");
                        sb.Append("<span class=\"reg_li_brac_txt\">(");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Description, LanguageCode));
                        sb.Append(")</span> ");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        sb.Append("<div>");
                        sb.Append("<a href=\" " + DFDViewPath + "\"  ");
                        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                        sb.Append("<a href='Download.aspx?fileId=" + DFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                        sb.Append("</div>");
                        sb.Append("<br/>");
                        sb.Append("<div id=\"divDFD_" + DFD.Structures.Dataflows[0].id + "\" style=\"margin-left: 20px;overflow:auto;height:auto;max-height:200px;\" >");//display:none;
                        RegistrationsPerDFD = BindRegistrationsPerDFD(DbNId, LanguageCode, DFD.Structures.Dataflows[0].id);
                        if (string.IsNullOrEmpty(RegistrationsPerDFD))
                        {
                            sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                        }
                        else
                        {
                            sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                            sb.Append("<tr class=\"HeaderRowStyle \">");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                            sb.Append("</tr>");
                            sb.Append(RegistrationsPerDFD);
                            sb.Append("</table>");
                        }

                        sb.Append("</div>");
                       
                    }
                }
           

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string BindRegistrationsPerDFD(string DbNId, string LanguageCode, string DFDId)
    {
        string RetVal;
        string PAId;
        string RegDFDOrMFDId;
        string Id;
        string QueryableData;
        string WADL;
        string WSDL;
        string SimpleData;
        string QueryableDataUrl;
        string WADLUrl;
        string WSDLUrl;
        string SimpleDataUrl;
        string DataMetadata;
        string DFDMFD;
        string Constraints;
        string PA;
        string DPId;
        string Provider;
        string UserNId;
        string ConstraintPath;
        string ConstraintViewPath;
        string PAPath;
        string PAViewPath;
        string RegPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtReg;
        DataTable DtPA;
        StringBuilder sb;
        int i,j,RowCounter;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Message.StructureType DPScheme;
        string SimpleDataSource;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;

        RetVal = string.Empty;
        PAId=string.Empty;
        RegDFDOrMFDId = string.Empty;
        Id = string.Empty;
        QueryableData = string.Empty;
        WADL = string.Empty;
        WSDL = string.Empty;
        SimpleData = string.Empty;
        QueryableDataUrl = string.Empty;
        WADLUrl = string.Empty;
        WSDLUrl = string.Empty;
        SimpleDataUrl = string.Empty;
        DataMetadata = string.Empty;
        DFDMFD = string.Empty;
        Constraints = string.Empty;
        PA = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;
        UserNId = string.Empty;
        ConstraintPath = string.Empty;
        ConstraintViewPath = string.Empty;
        PAPath = string.Empty;
        PAViewPath = string.Empty;    
        DIConnection = null;
        RegPath = string.Empty;       
        sb = new StringBuilder();
        i = 0;
        j = 0;
        RowCounter = 0;
        RegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        DPScheme = new SDMXObjectModel.Message.StructureType();

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
           
           
                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=10;";
                DtReg = DIConnection.ExecuteDataTable(Query);

                if (DtReg != null && DtReg.Rows.Count > 0)
                {
                    for (i = 0; i < DtReg.Rows.Count; i++)
                    {
                        RegPath = DtReg.Rows[i]["FileLocation"].ToString();
                        RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegPath);
                        PAId = ((ProvisionAgreementRefType)((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.ProvisionAgreement.Items[0]).id;
                        if (PAId.Contains("DF_"))
                        {
                            RegDFDOrMFDId = "DF_" + Global.SplitString(PAId, "DF_")[1].ToString();
                        }
                        else if (PAId.Contains("MF_"))
                        {
                            RegDFDOrMFDId = "MF_" + Global.SplitString(PAId, "MF_")[1].ToString();
                        }
                        if (RegDFDOrMFDId == DFDId)
                        {
                            RowCounter += 1;
                            
                            Id = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id;
                            Global.Retrieve_SimpleAndQueryableDataSource_FromRegistration(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration, out SimpleDataSource, out QueryableDataSource);
                                                        
                            if (!string.IsNullOrEmpty(SimpleDataSource))
                            {
                                SimpleDataUrl = SimpleDataSource;
                                if (SimpleDataSource.Length > 25)
                                {
                                    SimpleData = SimpleDataSource.Substring(0, 25) + "...";
                                }
                                else
                                {
                                    SimpleData = SimpleDataSource;
                                }
                            }

                            if (QueryableDataSource != null)
                            {
                                QueryableDataUrl = QueryableDataSource.DataURL;
                                if (QueryableDataSource.DataURL.Length > 25)
                                {
                                    QueryableData = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                                }
                                else
                                {
                                    QueryableData = QueryableDataSource.DataURL;
                                }

                                WADLUrl = QueryableDataSource.WADLURL;
                                if (QueryableDataSource.WADLURL.Length > 25)
                                {
                                    WADL = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                                }
                                else
                                {
                                    WADL = QueryableDataSource.WADLURL;
                                }

                                WSDLUrl = QueryableDataSource.WSDLURL;
                                if (QueryableDataSource.WSDLURL.Length > 25)
                                {
                                    WSDL = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                                }
                                else
                                {
                                    WSDL = QueryableDataSource.WSDLURL;
                                }
                            }
                            DataMetadata = "Data";
                           
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + PAId +"' AND Type=8 ;";
                            DtPA = DIConnection.ExecuteDataTable(Query);

                            if (DtPA != null && DtPA.Rows.Count > 0)
                            {

                                PAPath = DtPA.Rows[0]["FileLocation"].ToString();
                                PAViewPath = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/PAs/" + PAId + ".xml";
                                UserNId = Global.SplitString(PAId, "_")[1].ToString();
                                ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                                ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                                DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;
                                DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                                if (DPScheme.Structures.OrganisationSchemes.Count > 0)
                                {
                                    for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                                    {
                                        if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                                        {

                                            Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                                        }
                                    }
                                }
                            }
                            if (RowCounter % 2 == 0)
                            {
                                sb.Append("<tr class=\"DataRowStyle \" style=\"background-color:#D8D8DC \">");
                            }
                            else
                            {
                                sb.Append("<tr class=\"DataRowStyle \">");
                            }
                            sb.Append("<td class=\"DataColumnStyle \">" + Id +"</td>");
                            sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + QueryableDataUrl + "\" title=\"" + QueryableDataUrl + "\" target=\"_blank \" >" + QueryableData + "</a></td>");
                            sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WADLUrl + "\" title=\"" + WADLUrl + "\" target=\"_blank \" >" + WADL + "</a></td>");
                            sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WSDLUrl + "\" title=\"" + WSDLUrl + "\" target=\"_blank \" >" + WSDL + "</a></td>");
                            sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + SimpleDataUrl + "\" title=\"" + SimpleDataUrl + "\" target=\"_blank \" >" + SimpleData + "</a></td>");
                     //       sb.Append("<td class=\"DataColumnStyle \">" + DataMetadata + "</td>");
                            sb.Append("<td class=\"DataColumnStyle \">");
                            if (DataMetadata == "Data")
                            {
                                sb.Append("<a id=\"aView\" href=\" " + ConstraintViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                                sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + ConstraintPath + "' name=\"lang_Download\"></a>");
                            }
                            sb.Append("</td>");
                            sb.Append("<td class=\"DataColumnStyle \"><a id=\"aView\" href=\" " + PAViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                            sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + PAPath + "' name=\"lang_Download\"></a>");
                            sb.Append("</td>");
                            sb.Append("<td class=\"DataColumnStyle \">" + Provider + "</td>");
                            sb.Append("</tr>");
                        }
                    }

                }
               
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerMFD(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string MFDViewPath, MFDPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtMFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerMFD;


        DIConnection = null;
        MFDViewPath = string.Empty;
        MFDPath = string.Empty;
        CompleteFilePath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        RegistrationsPerMFD = string.Empty;
        i = 0;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                sb = new StringBuilder();
                for (i = 0; i < Complete.Structures.Metadataflows.Count; i++)
                {
                    Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 AND Id='" + Complete.Structures.Metadataflows[i].id + "';";
                    DtMFD = DIConnection.ExecuteDataTable(Query);

                    if (DtMFD != null && DtMFD.Rows.Count > 0)
                    {
                        MFDPath = DtMFD.Rows[0]["FileLocation"].ToString();
                        MFDViewPath = "../../" + MFDPath.Substring(MFDPath.LastIndexOf("stock")).Replace("\\", "/");

                    }
                    else
                    {
                        MFDPath = string.Empty;
                        MFDViewPath = string.Empty;
                    }
                    sb.Append("<div>");
                    sb.Append("<img id=\"imgdivMFD_" + Complete.Structures.Metadataflows[i].id + "\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMFD_" + Complete.Structures.Metadataflows[i].id + "' ,'divMFD_" + Complete.Structures.Metadataflows[i].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[i].Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[i].Description, LanguageCode));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + MFDViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + MFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                    sb.Append("<div id=\"divMFD_" + Complete.Structures.Metadataflows[i].id + "\" style=\"margin-left: 20px;display:block;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerMFD = BindRegistrationsPerMFD(DbNId, LanguageCode, Complete.Structures.Metadataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerMFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerMFD);
                        sb.Append("</table>");
                    }
                    //sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                }
            }           
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerMFDFor2_0(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string MFDViewPath, MFDPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtMFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerMFD;


        DIConnection = null;
        MFDViewPath = string.Empty;
        MFDPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        RegistrationsPerMFD = string.Empty;
        i = 0;

        try
        {
            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
           
            sb = new StringBuilder();
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 ;";
            DtMFD = DIConnection.ExecuteDataTable(Query);

            if (DtMFD != null && DtMFD.Rows.Count > 0)
            {
                for (i = 0; i < DtMFD.Rows.Count; i++)
                {
                    MFDPath = DtMFD.Rows[i]["FileLocation"].ToString();
                    MFDViewPath = "../../" + MFDPath.Substring(MFDPath.LastIndexOf("stock")).Replace("\\", "/");
                    MFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);

                    sb.Append("<div>");
                    sb.Append("<img id=\"imgdivMFD_" + MFD.Structures.Metadataflows[i].id + "\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMFD_" + MFD.Structures.Metadataflows[i].id + "' ,'divMFD_" + MFD.Structures.Metadataflows[i].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[i].Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[i].Description, LanguageCode));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + MFDViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + MFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                    sb.Append("<div id=\"divMFD_" + MFD.Structures.Metadataflows[i].id + "\" style=\"margin-left: 20px;display:block;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerMFD = BindRegistrationsPerMFD(DbNId, LanguageCode, MFD.Structures.Metadataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerMFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerMFD);
                        sb.Append("</table>");
                    }
                    //sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                }

            }
           

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string BindRegistrationsPerMFD(string DbNId, string LanguageCode, string MFDId)
    {
        string RetVal;
        string PAId;
        string RegDFDOrMFDId;
        string Id;
        string QueryableData;
        string WADL;
        string WSDL;
        string SimpleData;
        string QueryableDataUrl;
        string WADLUrl;
        string WSDLUrl;
        string SimpleDataUrl;
        string DataMetadata;
        string DFDMFD;
        string Constraints;
        string PA;
        string DPId;
        string Provider;
        string UserNId;
        string ConstraintPath;
        string ConstraintViewPath;
        string PAPath;
        string PAViewPath;
        string RegPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtReg;
        DataTable DtPA;
        StringBuilder sb;
        int i, j;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Message.StructureType DPScheme;
        int RowCounter;
        string SimpleDataSource;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;

        RetVal = string.Empty;
        PAId = string.Empty;
        RegDFDOrMFDId = string.Empty;
        Id = string.Empty;
        QueryableData = string.Empty;
        WADL = string.Empty;
        WSDL = string.Empty;
        SimpleData = string.Empty;
        QueryableDataUrl = string.Empty;
        WADLUrl = string.Empty;
        WSDLUrl = string.Empty;
        SimpleDataUrl = string.Empty;
        DataMetadata = string.Empty;
        DFDMFD = string.Empty;
        Constraints = string.Empty;
        PA = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;
        UserNId = string.Empty;
        ConstraintPath = string.Empty;
        ConstraintViewPath = string.Empty;
        PAPath = string.Empty;
        PAViewPath = string.Empty;
        DIConnection = null;
        RegPath = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        RegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        DPScheme = new SDMXObjectModel.Message.StructureType();
        RowCounter = 0;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);


            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=10;";
            DtReg = DIConnection.ExecuteDataTable(Query);

            if (DtReg != null && DtReg.Rows.Count > 0)
            {
                for (i = 0; i < DtReg.Rows.Count; i++)
                {
                    RegPath = DtReg.Rows[i]["FileLocation"].ToString();
                    RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegPath);
                    PAId = ((ProvisionAgreementRefType)((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.ProvisionAgreement.Items[0]).id;
                    if (PAId.Contains("DF_"))
                    {
                        RegDFDOrMFDId = "DF_" + Global.SplitString(PAId, "DF_")[1].ToString();
                    }
                    else if (PAId.Contains("MF_"))
                    {
                        RegDFDOrMFDId = "MF_" + Global.SplitString(PAId, "MF_")[1].ToString();
                    }
                    if (RegDFDOrMFDId == MFDId)
                    {
                        RowCounter += 1;
                        Id = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id;
                        Global.Retrieve_SimpleAndQueryableDataSource_FromRegistration(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration, out SimpleDataSource, out QueryableDataSource);

                        if (!string.IsNullOrEmpty(SimpleDataSource))
                        {
                            SimpleDataUrl = SimpleDataSource;
                            if (SimpleDataSource.Length > 25)
                            {
                                SimpleData = SimpleDataSource.Substring(0, 25) + "...";
                            }
                            else
                            {
                                SimpleData = SimpleDataSource;
                            }
                        }

                        if (QueryableDataSource != null)
                        {
                            QueryableDataUrl = QueryableDataSource.DataURL;
                            if (QueryableDataSource.DataURL.Length > 25)
                            {
                                QueryableData = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                QueryableData = QueryableDataSource.DataURL;
                            }

                            WADLUrl = QueryableDataSource.WADLURL;
                            if (QueryableDataSource.WADLURL.Length > 25)
                            {
                                WADL = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WADL = QueryableDataSource.WADLURL;
                            }

                            WSDLUrl = QueryableDataSource.WSDLURL;
                            if (QueryableDataSource.WSDLURL.Length > 25)
                            {
                                WSDL = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WSDL = QueryableDataSource.WSDLURL;
                            }
                        }                        
                        DataMetadata = "Metadata";

                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + PAId + "' AND Type=8 ;";
                        DtPA = DIConnection.ExecuteDataTable(Query);

                        if (DtPA != null && DtPA.Rows.Count > 0)
                        {

                            PAPath = DtPA.Rows[0]["FileLocation"].ToString();
                            PAViewPath = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/PAs/" + PAId + ".xml";
                            UserNId = Global.SplitString(PAId, "_")[1].ToString();
                            ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                            ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                            DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;
                            DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                            if (DPScheme.Structures.OrganisationSchemes.Count > 0)
                            {
                                for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                                {
                                    if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                                    {

                                        Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                                    }
                                }
                            }
                        }
                        if (RowCounter % 2 == 0)
                        {
                            sb.Append("<tr class=\"DataRowStyle \" style=\"background-color:#D8D8DC \">");
                        }
                        else
                        {
                            sb.Append("<tr class=\"DataRowStyle \">");
                        }

                        sb.Append("<td class=\"DataColumnStyle \" width=\"10%\">" + Id + "</td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"13%\">" + "<a href=\"" + QueryableDataUrl + "\" title=\"" + QueryableDataUrl + "\" target=\"_blank \" >" + QueryableData + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"13%\">" + "<a href=\"" + WADLUrl + "\" title=\"" + WADLUrl + "\" target=\"_blank \" >" + WADL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"13%\">" + "<a href=\"" + WSDLUrl + "\" title=\"" + WSDLUrl + "\" target=\"_blank \" >" + WSDL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"13%\">" + "<a href=\"" + SimpleDataUrl + "\" title=\"" + SimpleDataUrl + "\" target=\"_blank \" >" + SimpleData + "</a></td>");
                      //  sb.Append("<td class=\"DataColumnStyle \" width=\"10%\">" + DataMetadata + "</td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"10%\">");
                        if (DataMetadata == "Data")
                        {
                            sb.Append("<a id=\"aView\" href=\" " + ConstraintViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                            sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + ConstraintPath + "' name=\"lang_Download\"></a>");
                        }
                        sb.Append("</td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"10%\"><a id=\"aView\" href=\" " + PAViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                        sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + PAPath + "' name=\"lang_Download\"></a>");
                        sb.Append("</td>");
                        sb.Append("<td class=\"DataColumnStyle \" width=\"8%\">" + Provider + "</td>");
                        sb.Append("</tr>");
                    }
                }

            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }



    private string GetAgreementsAsPerPA(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string PAViewPath, PAPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtPA;
        StringBuilder sb;
        int i,j;
        SDMXObjectModel.Message.StructureType PAStructure;
        string RegistrationsPerPA;
        DataTable DtDFDMFD;
        SDMXObjectModel.Message.StructureType DPScheme;
        string DFDMFDId;
        string DFDMFDName;
        string DFDMFDDesc;
        string DFDMFDPath;
        string DFDMFDViewPath;
        string DPId;
        string Provider;

        DIConnection = null;
        PAViewPath = string.Empty;
        PAPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        PAStructure = new SDMXObjectModel.Message.StructureType();
        DtDFDMFD = new DataTable();
        DPScheme = new SDMXObjectModel.Message.StructureType();
        DFDMFDId = string.Empty;
        DFDMFDName = string.Empty;
        DFDMFDDesc = string.Empty;
        DFDMFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        CompleteFilePath = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=8;";
                DtPA = DIConnection.ExecuteDataTable(Query);
                if (DtPA != null && DtPA.Rows.Count > 0)
                {
                    for (i = 0; i < DtPA.Rows.Count; i++)
                    {
                        PAPath = DtPA.Rows[i]["FileLocation"].ToString();
                        PAViewPath = "../../" + PAPath.Substring(PAPath.LastIndexOf("stock")).Replace("\\", "/");

                        PAStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), PAPath);
                        sb.Append("<div>");
                     //   sb.Append("<img id=\"imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "' ,'divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                        sb.Append("<div class=\"reg_li_sub_txt\">");
                        sb.Append("<b>");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Name, LanguageCode));
                        sb.Append("</b>");
                        sb.Append("<span class=\"reg_li_brac_txt\">(");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Description, LanguageCode));
                        sb.Append(")</span> ");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("DF_"))
                        {
                            DFDMFDId = "DF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "DF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=4 ;";
                            for (j = 0; j < Complete.Structures.Dataflows.Count; j++)
                            {
                                if (DFDMFDId == Complete.Structures.Dataflows[j].id)
                                {

                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[j].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[j].Description, LanguageCode);
                                    break;
                                }
                            }
                        }
                        else if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("MF_"))
                        {
                            DFDMFDId = "MF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "MF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=16 ;";
                            for (j = 0; j < Complete.Structures.Metadataflows.Count; j++)
                            {
                                if (DFDMFDId == Complete.Structures.Metadataflows[j].id)
                                {

                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[j].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[j].Description, LanguageCode);
                                    break;
                                }
                            }
                        }

                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {

                            DFDMFDPath = DtDFDMFD.Rows[0]["FileLocation"].ToString();
                            DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                        }
                        DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "_")[1].ToString();

                        DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                        for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                        {
                            if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                            {

                                Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                            }
                        }
                        sb.Append("<div style=\"margin-left: 20px;\">");
                        sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_Provider_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + Provider);
                        sb.Append("</span>");
                        sb.Append("<br/>");
                        sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_DFD_MFD_Short_Form" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + "<a id=\"aDFDMFD_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\">" + DFDMFDName + "</a>" + "(" + DFDMFDDesc + ")");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("<div style=\"margin-left: 20px;\">");
                        sb.Append("<a href=\" " + PAViewPath + "\"  ");
                        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                        sb.Append("<a href='Download.aspx?fileId=" + PAPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                        sb.Append("</div>");
                        sb.Append("<br/>");
                        sb.Append("<div id=\"divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" style=\"margin-left: 20px;display:none;overflow:auto;height:auto;max-height:200px;\" >");
                        sb.Append("</div>");
                        sb.Append("<br/>");
                    }
                }
            } 
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetAgreementsAsPerPAFor2_0(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string PAViewPath, PAPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtPA;
        StringBuilder sb;
        int i, j;
        SDMXObjectModel.Message.StructureType PAStructure;
       // string RegistrationsPerPA;
        DataTable DtDFDMFD;
        DataTable DtDFD;
        DataTable DtMFD;
        SDMXObjectModel.Message.StructureType DPScheme;
        string DFDMFDId;
        string DFDMFDName;
        string DFDMFDDesc;
        string DFDMFDPath;
        string DFDPath;
        string MFDPath;
        string DFDMFDViewPath;
        string DPId;
        string Provider;

        DIConnection = null;
        PAViewPath = string.Empty;
        PAPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        PAStructure = new SDMXObjectModel.Message.StructureType();
     //   RegistrationsPerPA = string.Empty;
        DtDFDMFD = new DataTable();
        DtDFD = new DataTable();
        DtMFD = new DataTable();
        DPScheme = new SDMXObjectModel.Message.StructureType();
        DFDMFDId = string.Empty;
        DFDMFDName = string.Empty;
        DFDMFDDesc = string.Empty;
        DFDMFDPath = string.Empty;
        DFDPath = string.Empty;
        MFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;

        try
        {
            SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=8;";
            DtPA = DIConnection.ExecuteDataTable(Query);
            if (DtPA != null && DtPA.Rows.Count > 0)
            {
                for (i = 0; i < DtPA.Rows.Count; i++)
                {
                    PAPath = DtPA.Rows[i]["FileLocation"].ToString();
                    PAViewPath = "../../" + PAPath.Substring(PAPath.LastIndexOf("stock")).Replace("\\", "/");

                    PAStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), PAPath);
                    sb.Append("<div>");
                  //  sb.Append("<img id=\"imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "' ,'divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Description, LanguageCode));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("DF_"))
                    {
                        DFDMFDId = "DF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "DF_")[1].ToString();
                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 ;";
                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            for (j = 0; j < DtDFDMFD.Rows.Count; j++)
                            {
                                DFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                                if (DFDMFDId == DFD.Structures.Dataflows[0].id)
                                {
                                    DFDMFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                    DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Description, LanguageCode);
                                    break;
                                }
                            }
                           
                           

                        }
                       
                    }
                    else if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("MF_"))
                    {
                        DFDMFDId = "MF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "MF_")[1].ToString();
                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 ;";
                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            for (j = 0; j < DtDFDMFD.Rows.Count; j++)
                            {
                                MFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                MFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                                if (DFDMFDId == MFD.Structures.Metadataflows[0].id)
                                {
                                    DFDMFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                    DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[0].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[0].Description, LanguageCode);
                                    break;
                                }
                            }

                        }
                    }
                   
                    DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "_")[1].ToString();

                    DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                    for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                    {
                        if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                        {

                            Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                        }
                    }
                    sb.Append("<div style=\"margin-left: 20px;\">");
                    sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_Provider_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + Provider);
                    sb.Append("</span>");
                    sb.Append("<br/>");
                    sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_DFD_MFD_Short_Form" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + "<a id=\"aDFDMFD_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\">" + DFDMFDName + "</a>" + "(" + DFDMFDDesc + ")");
                    sb.Append("</span>");
                    sb.Append("</div>");
                    sb.Append("<div style=\"margin-left: 20px;\">");
                    sb.Append("<a href=\" " + PAViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + PAPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<div id=\"divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" style=\"margin-left: 20px;display:none;overflow:auto;height:auto;max-height:200px;\" >");
                  
                    sb.Append("</div>");
                    sb.Append("<br/>");
                }
            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }
 
    private string GetRegistrationsAsPerPA(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string PAViewPath, PAPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtPA;
        StringBuilder sb;
        int i,j;
        SDMXObjectModel.Message.StructureType PAStructure;
        string RegistrationsPerPA;
        DataTable DtDFDMFD;
        SDMXObjectModel.Message.StructureType DPScheme;
        string DFDMFDId;
        string DFDMFDName;
        string DFDMFDDesc;
        string DFDMFDPath;
        string DFDMFDViewPath;
        string DPId;
        string Provider;

        DIConnection = null;
        PAViewPath = string.Empty;
        PAPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        PAStructure = new SDMXObjectModel.Message.StructureType();
        RegistrationsPerPA = string.Empty;
        DtDFDMFD = new DataTable();
        DPScheme = new SDMXObjectModel.Message.StructureType();
        DFDMFDId = string.Empty;
        DFDMFDName = string.Empty;
        DFDMFDDesc = string.Empty;
        DFDMFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        CompleteFilePath = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=8;";
                DtPA = DIConnection.ExecuteDataTable(Query);
                if (DtPA != null && DtPA.Rows.Count > 0)
                {
                    for (i = 0; i < DtPA.Rows.Count; i++)
                    {
                        PAPath = DtPA.Rows[i]["FileLocation"].ToString();
                        PAViewPath = "../../" + PAPath.Substring(PAPath.LastIndexOf("stock")).Replace("\\", "/");

                        PAStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), PAPath);
                        sb.Append("<div>");
                        sb.Append("<img id=\"imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "' ,'divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                        sb.Append("<div class=\"reg_li_sub_txt\">");
                        sb.Append("<b>");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Name, LanguageCode));
                        sb.Append("</b>");
                        sb.Append("<span class=\"reg_li_brac_txt\">(");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Description, LanguageCode));
                        sb.Append(")</span> ");
                        sb.Append("</div>");
                        sb.Append("</div>");
                        if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("DF_"))
                        {
                            DFDMFDId = "DF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "DF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=4 ;";
                            for (j = 0; j < Complete.Structures.Dataflows.Count; j++)
                            {
                                if (DFDMFDId == Complete.Structures.Dataflows[j].id)
                                {

                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[j].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Dataflows[j].Description, LanguageCode);
                                    break;
                                }
                            }
                        }
                        else if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("MF_"))
                        {
                            DFDMFDId = "MF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "MF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=16 ;";
                            for (j = 0; j < Complete.Structures.Metadataflows.Count; j++)
                            {
                                if (DFDMFDId == Complete.Structures.Metadataflows[j].id)
                                {

                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[j].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(Complete.Structures.Metadataflows[j].Description, LanguageCode);
                                    break;
                                }
                            }
                        }

                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {

                            DFDMFDPath = DtDFDMFD.Rows[0]["FileLocation"].ToString();
                            DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                        }
                        DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "_")[1].ToString();

                        DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                        for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                        {
                            if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                            {

                                Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                            }
                        }
                        sb.Append("<div style=\"margin-left: 20px;\">");
                        sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_Provider_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + Provider);
                        sb.Append("</span>");
                        sb.Append("<br/>");
                        sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_DFD_MFD_Short_Form" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + "<a id=\"aDFDMFD_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\">" + DFDMFDName + "</a>" + "(" + DFDMFDDesc + ")");
                        sb.Append("</span>");
                        sb.Append("</div>");
                        sb.Append("<div style=\"margin-left: 20px;\">");
                        sb.Append("<a href=\" " + PAViewPath + "\"  ");
                        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                        sb.Append("<a href='Download.aspx?fileId=" + PAPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                        sb.Append("</div>");
                        sb.Append("<br/>");
                        sb.Append("<div id=\"divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" style=\"margin-left: 20px;display:none;overflow:auto;height:auto;max-height:200px;\" >");
                        RegistrationsPerPA = BindRegistrationsPerPA(DbNId, LanguageCode, PAStructure.Structures.ProvisionAgreements[0].id);
                        if (string.IsNullOrEmpty(RegistrationsPerPA))
                        {
                            sb.Append("<span  id=\"lang_No_Registration_Found_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"><i>No Registration Found</i></span>");
                        }
                        else
                        {

                            sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                            sb.Append("<tr class=\"HeaderRowStyle \">");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                            //sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_DFDMFD\">Data/Metadata Flow Definition</span></td>");
                            sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");

                            //sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_DataProvider\">Data Provider</span></td>");
                            sb.Append("</tr>");
                            sb.Append(RegistrationsPerPA);
                            sb.Append("</table>");
                        }
                        sb.Append("</div>");
                        sb.Append("<br/>");
                    }
                }
            } 
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerPAFor2_0(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string PAViewPath, PAPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtPA;
        StringBuilder sb;
        int i, j;
        SDMXObjectModel.Message.StructureType PAStructure;
        string RegistrationsPerPA;
        DataTable DtDFDMFD;
        DataTable DtDFD;
        DataTable DtMFD;
        SDMXObjectModel.Message.StructureType DPScheme;
        string DFDMFDId;
        string DFDMFDName;
        string DFDMFDDesc;
        string DFDMFDPath;
        string DFDPath;
        string MFDPath;
        string DFDMFDViewPath;
        string DPId;
        string Provider;

        DIConnection = null;
        PAViewPath = string.Empty;
        PAPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        PAStructure = new SDMXObjectModel.Message.StructureType();
        RegistrationsPerPA = string.Empty;
        DtDFDMFD = new DataTable();
        DtDFD = new DataTable();
        DtMFD = new DataTable();
        DPScheme = new SDMXObjectModel.Message.StructureType();
        DFDMFDId = string.Empty;
        DFDMFDName = string.Empty;
        DFDMFDDesc = string.Empty;
        DFDMFDPath = string.Empty;
        DFDPath = string.Empty;
        MFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;

        try
        {
            SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=8;";
            DtPA = DIConnection.ExecuteDataTable(Query);
            if (DtPA != null && DtPA.Rows.Count > 0)
            {
                for (i = 0; i < DtPA.Rows.Count; i++)
                {
                    PAPath = DtPA.Rows[i]["FileLocation"].ToString();
                    PAViewPath = "../../" + PAPath.Substring(PAPath.LastIndexOf("stock")).Replace("\\", "/");

                    PAStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), PAPath);
                    sb.Append("<div>");
                    sb.Append("<img id=\"imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "' ,'divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(PAStructure.Structures.ProvisionAgreements[0].Description, LanguageCode));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("DF_"))
                    {
                        DFDMFDId = "DF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "DF_")[1].ToString();
                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 ;";
                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            for (j = 0; j < DtDFDMFD.Rows.Count; j++)
                            {
                                DFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                                if (DFDMFDId == DFD.Structures.Dataflows[0].id)
                                {
                                    DFDMFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                    DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Description, LanguageCode);
                                    break;
                                }
                            }
                           
                           

                        }
                       
                    }
                    else if (PAStructure.Structures.ProvisionAgreements[0].id.Contains("MF_"))
                    {
                        DFDMFDId = "MF_" + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "MF_")[1].ToString();
                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 ;";
                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            for (j = 0; j < DtDFDMFD.Rows.Count; j++)
                            {
                                MFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                MFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                                if (DFDMFDId == MFD.Structures.Metadataflows[0].id)
                                {
                                    DFDMFDPath = DtDFDMFD.Rows[j]["FileLocation"].ToString();
                                    DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                                    DFDMFDName = GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[0].Name, LanguageCode);
                                    DFDMFDDesc = GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[0].Description, LanguageCode);
                                    break;
                                }
                            }

                        }
                    }
                   
                    DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + Global.SplitString(PAStructure.Structures.ProvisionAgreements[0].id, "_")[1].ToString();

                    DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                    for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                    {
                        if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                        {

                            Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                        }
                    }
                    sb.Append("<div style=\"margin-left: 20px;\">");
                    sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_Provider_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + Provider);
                    sb.Append("</span>");
                    sb.Append("<br/>");
                    sb.Append("<span class=\"reg_li_brac_txt\">" + "<span id=\"lang_DFD_MFD_Short_Form" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span>" + "<a id=\"aDFDMFD_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\">" + DFDMFDName + "</a>" + "(" + DFDMFDDesc + ")");
                    sb.Append("</span>");
                    sb.Append("</div>");
                    sb.Append("<div style=\"margin-left: 20px;\">");
                    sb.Append("<a href=\" " + PAViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + PAPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<div id=\"divPA_" + PAStructure.Structures.ProvisionAgreements[0].id + "\" style=\"margin-left: 20px;display:none;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerPA = BindRegistrationsPerPA(DbNId, LanguageCode, PAStructure.Structures.ProvisionAgreements[0].id);
                    if (string.IsNullOrEmpty(RegistrationsPerPA))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"><i>No Registration Found</i></span>");
                    }
                    else
                    {

                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");
                        //sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_DFDMFD\">Data/Metadata Flow Definition</span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + PAStructure.Structures.ProvisionAgreements[0].id + "\"></span></td>");

                        //sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_DataProvider\">Data Provider</span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerPA);
                        sb.Append("</table>");
                    }
                    sb.Append("</div>");
                    sb.Append("<br/>");
                }
            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string BindRegistrationsPerPA(string DbNId, string LanguageCode, string PAId)
    {
        string RetVal;
        string DFDMFDId;
        string RegPAId;
        string Id;
        string QueryableData;
        string WADL;
        string WSDL;
        string SimpleData;
        string QueryableDataUrl;
        string WADLUrl;
        string WSDLUrl;
        string SimpleDataUrl;
        string DataMetadata;
        string DFDMFD;
        string Constraints;
        string PA;
        string DPId;
        string Provider;
        string UserNId;
        string ConstraintPath;
        string ConstraintViewPath;
        string DFDMFDPath;
        string DFDMFDViewPath;
        string RegPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtReg;
        DataTable DtDFDMFD;
        StringBuilder sb;
        int i, j, RowCounter;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Message.StructureType DPScheme;
        string SimpleDataSource;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;

        RetVal = string.Empty;
        DFDMFDId = string.Empty;
        RegPAId = string.Empty;
        Id = string.Empty;
        QueryableData = string.Empty;
        WADL = string.Empty;
        WSDL = string.Empty;
        SimpleData = string.Empty;
        QueryableDataUrl = string.Empty;
        WADLUrl = string.Empty;
        WSDLUrl = string.Empty;
        SimpleDataUrl = string.Empty;
        DataMetadata = string.Empty;
        DFDMFD = string.Empty;
        Constraints = string.Empty;
        PA = string.Empty;
        DPId = string.Empty;
        Provider = string.Empty;
        UserNId = string.Empty;
        ConstraintPath = string.Empty;
        ConstraintViewPath = string.Empty;
        DFDMFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        DIConnection = null;
        RegPath = string.Empty;
        sb = new StringBuilder();
        i = 0;
        j = 0;
        RowCounter = 0;
        RegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        DPScheme = new SDMXObjectModel.Message.StructureType();

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);

            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=10;";
            DtReg = DIConnection.ExecuteDataTable(Query);

            if (DtReg != null && DtReg.Rows.Count > 0)
            {
                for (i = 0; i < DtReg.Rows.Count; i++)
                {
                    RegPath = DtReg.Rows[i]["FileLocation"].ToString();
                    RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegPath);
                    RegPAId = ((ProvisionAgreementRefType)((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.ProvisionAgreement.Items[0]).id;

                    if (RegPAId == PAId)
                    {

                        RowCounter += 1;
                        Id = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id;
                        Global.Retrieve_SimpleAndQueryableDataSource_FromRegistration(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration, out SimpleDataSource, out QueryableDataSource);

                        if (!string.IsNullOrEmpty(SimpleDataSource))
                        {
                            SimpleDataUrl = SimpleDataSource;
                            if (SimpleDataSource.Length > 25)
                            {
                                SimpleData = SimpleDataSource.Substring(0, 25) + "...";
                            }
                            else
                            {
                                SimpleData = SimpleDataSource;
                            }
                        }

                        if (QueryableDataSource != null)
                        {
                            QueryableDataUrl = QueryableDataSource.DataURL;
                            if (QueryableDataSource.DataURL.Length > 25)
                            {
                                QueryableData = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                QueryableData = QueryableDataSource.DataURL;
                            }

                            WADLUrl = QueryableDataSource.WADLURL;
                            if (QueryableDataSource.WADLURL.Length > 25)
                            {
                                WADL = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WADL = QueryableDataSource.WADLURL;
                            }

                            WSDLUrl = QueryableDataSource.WSDLURL;
                            if (QueryableDataSource.WSDLURL.Length > 25)
                            {
                                WSDL = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WSDL = QueryableDataSource.WSDLURL;
                            }
                        }
                        if (RegPAId.Contains("DF_"))
                        {
                            DataMetadata = "Data";
                            DFDMFDId = "DF_" + Global.SplitString(PAId, "DF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=4 ;";
                           
                        }
                        else if (RegPAId.Contains("MF_"))
                        {
                            DataMetadata = "Metadata";
                            DFDMFDId = "MF_" + Global.SplitString(PAId, "MF_")[1].ToString();
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=16 ;";
                        }

                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);

                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            DFDMFDPath = DtDFDMFD.Rows[0]["FileLocation"].ToString();
                            DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                            UserNId = Global.SplitString(PAId, "_")[1].ToString();
                            ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                            ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                            DPId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;
                            DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName));
                            if (DPScheme.Structures.OrganisationSchemes.Count > 0)
                            {
                                for (j = 0; j < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; j++)
                                {
                                    if (((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).id == DPId)
                                    {

                                        Provider = GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[j])).Name, LanguageCode);

                                    }
                                }
                            }
                        }
                        if (RowCounter % 2 == 0)
                        {
                            sb.Append("<tr class=\"DataRowStyle \" style=\"background-color:#D8D8DC \">");
                        }
                        else
                        {
                            sb.Append("<tr class=\"DataRowStyle \">");
                        }
                        sb.Append("<td class=\"DataColumnStyle \">" + Id + "</td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + QueryableDataUrl + "\" title=\"" + QueryableDataUrl + "\" target=\"_blank \" >" + QueryableData + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WADLUrl + "\" title=\"" + WADLUrl + "\" target=\"_blank \" >" + WADL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WSDLUrl + "\" title=\"" + WSDLUrl + "\" target=\"_blank \" >" + WSDL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + SimpleDataUrl + "\" title=\"" + SimpleDataUrl + "\" target=\"_blank \" >" + SimpleData + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + DataMetadata + "</td>");
                        //sb.Append("<td class=\"DataColumnStyle \"><a id=\"aView\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                        //sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + DFDMFDPath + "' name=\"lang_Download\"></a>");
                        //sb.Append("</td>");
                        sb.Append("<td class=\"DataColumnStyle \">");
                        if (DataMetadata == "Data")
                        {
                            sb.Append("<a id=\"aView\" href=\" " + ConstraintViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                            sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + ConstraintPath + "' name=\"lang_Download\"></a>");
                        }
                        sb.Append("</td>");
                        //sb.Append("<td class=\"DataColumnStyle \">" + Provider + "</td>");
                        sb.Append("</tr>");
                    }
                }

            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerDP(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string DPSchemeViewPath, DPSchemePath;
        StringBuilder sb;
        int i;
        SDMXObjectModel.Message.StructureType DPScheme;
        string RegistrationsPerDP;

        DPSchemeViewPath = string.Empty;
        DPSchemePath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        DPScheme = new SDMXObjectModel.Message.StructureType();
        RegistrationsPerDP = string.Empty;

        try
        {
            DPSchemePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
            DPSchemeViewPath = "../../" + DPSchemePath.Substring(DPSchemePath.LastIndexOf("stock")).Replace("\\", "/");
            if (File.Exists(DPSchemePath))
            {
                DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DPSchemePath);
                sb.Append("<div>");
                sb.Append("<b>");
                sb.Append(GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Name, LanguageCode));
                sb.Append("</b>");
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Description, LanguageCode));
                sb.Append(")</span> ");
                sb.Append("</div>");
                sb.Append("<div>");
                sb.Append("<a href=\" " + DPSchemeViewPath + "\"  ");
                sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                sb.Append("<a href='Download.aspx?fileId=" + DPSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                sb.Append("</div>");
                sb.Append("<br/>");
                for (i = 0; i < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; i++)
                {
                    sb.Append("<div>");
                    sb.Append("<img id=\"imgdiv" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdiv" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "' ,'div" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).Name, LanguageCode));
                    sb.Append("</b>");
                    sb.Append("</div>");
                    sb.Append("</div>");

                    sb.Append("<br/>");
                    sb.Append("<div id=\"div" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" style=\"margin-left: 20px;display:none;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerDP = BindRegistrationsPerDP(DbNId, LanguageCode, ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id);
                    if (string.IsNullOrEmpty(RegistrationsPerDP))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_DFD_MFD_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");

                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Provision_Agreement_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerDP);
                        sb.Append("</table>");
                    }

                    sb.Append("</div>");
                    sb.Append("<br/>");

                }
            }           
           

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    //private string GetRegistrationsAsPerDP(string DbNId, string LanguageCode)
    //{
    //    string RetVal;
    //    RetVal = string.Empty;

    //    string DPSchemeViewPath, DPSchemePath;
    //    StringBuilder sb;
    //    int i;
    //    SDMXObjectModel.Message.StructureType DPScheme;
    //    string RegistrationsPerDP;

    //    DPSchemeViewPath = string.Empty;
    //    DPSchemePath = string.Empty;
    //    RetVal = string.Empty;
    //    sb = new StringBuilder();
    //    i = 0;
    //    DPScheme = new SDMXObjectModel.Message.StructureType();
    //    RegistrationsPerDP = string.Empty;

    //    try
    //    {
    //        DPSchemePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
    //        DPSchemeViewPath = "../../" + DPSchemePath.Substring(DPSchemePath.LastIndexOf("stock")).Replace("\\", "/");
    //        DPScheme = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DPSchemePath);
    //        sb.Append("<div>");
    //        sb.Append("<b>");
    //        sb.Append(GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Name, LanguageCode));
    //        sb.Append("</b>");
    //        sb.Append("<span class=\"reg_li_brac_txt\">(");
    //        sb.Append(GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Description, LanguageCode));
    //        sb.Append(")</span> ");
    //        sb.Append("</div>");
    //        sb.Append("<div>");
    //        sb.Append("<a href=\" " + DPSchemeViewPath + "\"  ");
    //        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
    //        sb.Append("<a href='Download.aspx?fileId=" + DPSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
    //        sb.Append("</div>");
    //        sb.Append("<br/>");
    //        for (i = 0; i < DPScheme.Structures.OrganisationSchemes[0].Organisation.Count; i++)
    //        {
    //            sb.Append("<div>");
    //            sb.Append("<img id=\"imgdiv" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdiv" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "' ,'div" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");
    //            sb.Append("<div class=\"reg_li_sub_txt\">");
    //            sb.Append("<b>");
    //            sb.Append(GetLangSpecificValue_For_Version_2_1(((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).Name, LanguageCode));
    //            sb.Append("</b>");
    //            sb.Append("</div>");
    //            sb.Append("</div>");

    //            sb.Append("<br/>");

    //            sb.Append("<div id=\"div" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" align=\"left\">");
    //            sb.Append("<div style=\"overflow: hidden;\" id=\"DivHeaderRow_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\">");
    //             sb.Append("</div>");
    //             sb.Append("<div style=\"overflow:scroll;\" onscroll=\"OnScrollDiv(this, DivHeaderRow_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + ",DivFooterRow_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + ")\" id=\"DivMainContent" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\">");
    //             RegistrationsPerDP = BindRegistrationsPerDP(DbNId, LanguageCode, ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id);
    //             if (string.IsNullOrEmpty(RegistrationsPerDP))
    //             {
    //                 sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
    //             }
    //             else
    //             {
    //                 sb.Append("<table id=\"tblMainContent_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\">");
    //                 sb.Append("<tr class=\"HeaderRowStyle \">");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_DFD_MFD_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");

    //                 sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Provision_Agreement_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //                 sb.Append("</tr>");
    //                 sb.Append(RegistrationsPerDP);
    //                 sb.Append("</table>");
    //             }
    //             sb.Append("</div>");
    //             sb.Append("<div id=\"DivFooterRow_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" style=\"overflow:hidden\">");
    //             sb.Append("</div>");
    //             sb.Append("</div>");






    //            //sb.Append("<div id=\"div" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\" style=\"margin-left: 20px;display:none;overflow:scroll;height:auto;max-height:200px;overflow-x:hidden;\" >");
    //            //RegistrationsPerDP = BindRegistrationsPerDP(DbNId, LanguageCode, ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id);
    //            //if (string.IsNullOrEmpty(RegistrationsPerDP))
    //            //{
    //            //    sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
    //            //}
    //            //else
    //            //{
    //            //    sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\">");
    //            //    sb.Append("<tr class=\"HeaderRowStyle \">");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"lang_Id_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_Metadata_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_DFD_MFD_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");

    //            //    sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Provision_Agreement_" + ((SDMXObjectModel.Structure.DataProviderType)(DPScheme.Structures.OrganisationSchemes[0].Organisation[i])).id + "\"></span></td>");
    //            //    sb.Append("</tr>");
    //            //    sb.Append(RegistrationsPerDP);
    //            //    sb.Append("</table>");
    //            //}

    //            //sb.Append("</div>");



    //            sb.Append("<br/>");

    //        }

    //        RetVal = sb.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
    //    }
    //    finally
    //    {
    //    }

    //    return RetVal;
    //}

    private string BindRegistrationsPerDP(string DbNId, string LanguageCode, string DPId)
    {
        string RetVal;
        string PAId;
        string RegDPId;
        string Id;
        string QueryableData;
        string WADL;
        string WSDL;
        string SimpleData;
        string QueryableDataUrl;
        string WADLUrl;
        string WSDLUrl;
        string SimpleDataUrl;
        string DataMetadata;
        string DFDMFD;
        string Constraints;
        string PA;
        string DFDMFDId;
        string Provider;
        string UserNId;
        string DFDMFDPath;
        string DFDMFDViewPath;
        string ConstraintPath;
        string ConstraintViewPath;
        string PAPath;
        string PAViewPath;
        string RegPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtReg;
        DataTable DtPA;
        DataTable DtDFDMFD;
        StringBuilder sb;
        int i, RowCounter;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        SDMXObjectModel.Message.StructureType DPScheme;
        string SimpleDataSource;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;

        RetVal = string.Empty;
        PAId = string.Empty;
        RegDPId = string.Empty;
        Id = string.Empty;
        QueryableData = string.Empty;
        WADL = string.Empty;
        WSDL = string.Empty;
        SimpleData = string.Empty;
        QueryableDataUrl = string.Empty;
        WADLUrl = string.Empty;
        WSDLUrl = string.Empty;
        SimpleDataUrl = string.Empty;
        DataMetadata = string.Empty;
        DFDMFD = string.Empty;
        Constraints = string.Empty;
        PA = string.Empty;
        DFDMFDId = string.Empty;
        Provider = string.Empty;
        UserNId = string.Empty;
        DFDMFDPath = string.Empty;
        DFDMFDViewPath = string.Empty;
        ConstraintPath = string.Empty;
        ConstraintViewPath = string.Empty;
        PAPath = string.Empty;
        PAViewPath = string.Empty;
        DIConnection = null;
        RegPath = string.Empty;
        DtReg = new DataTable();
        DtPA = new DataTable();
        DtDFDMFD = new DataTable();
        sb = new StringBuilder();
        i = 0;
        RowCounter = 0;
        RegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        DPScheme = new SDMXObjectModel.Message.StructureType();

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);


            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=10;";
            DtReg = DIConnection.ExecuteDataTable(Query);

            if (DtReg != null && DtReg.Rows.Count > 0)
            {
                for (i = 0; i < DtReg.Rows.Count; i++)
                {
                    RegPath = DtReg.Rows[i]["FileLocation"].ToString();
                    RegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegPath);
                    PAId = ((ProvisionAgreementRefType)((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.ProvisionAgreement.Items[0]).id;
                    if (PAId.Contains("DF_"))
                    {
                        DFDMFDId = "DF_" + Global.SplitString(PAId, "DF_")[1].ToString();
                        DataMetadata = "Data";
                    }
                    else if (PAId.Contains("MF_"))
                    {
                        DFDMFDId = "MF_" + Global.SplitString(PAId, "MF_")[1].ToString();
                        DataMetadata = "Metadata";
                    }
                    RegDPId = "DP_" + Global.SplitString(PAId, "_")[1].ToString();
                    if (RegDPId == DPId)
                    {
                        RowCounter += 1;
                        Id = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id;
                        Global.Retrieve_SimpleAndQueryableDataSource_FromRegistration(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration, out SimpleDataSource, out QueryableDataSource);

                        if (!string.IsNullOrEmpty(SimpleDataSource))
                        {
                            SimpleDataUrl = SimpleDataSource;
                            if (SimpleDataSource.Length > 25)
                            {
                                SimpleData = SimpleDataSource.Substring(0, 25) + "...";
                            }
                            else
                            {
                                SimpleData = SimpleDataSource;
                            }
                        }

                        if (QueryableDataSource != null)
                        {
                            QueryableDataUrl = QueryableDataSource.DataURL;
                            if (QueryableDataSource.DataURL.Length > 25)
                            {
                                QueryableData = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                QueryableData = QueryableDataSource.DataURL;
                            }

                            WADLUrl = QueryableDataSource.WADLURL;
                            if (QueryableDataSource.WADLURL.Length > 25)
                            {
                                WADL = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WADL = QueryableDataSource.WADLURL;
                            }

                            WSDLUrl = QueryableDataSource.WSDLURL;
                            if (QueryableDataSource.WSDLURL.Length > 25)
                            {
                                WSDL = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                            }
                            else
                            {
                                WSDL = QueryableDataSource.WSDLURL;
                            }
                        }
                        Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + PAId + "' AND Type=8 ;";
                        DtPA = DIConnection.ExecuteDataTable(Query);

                        if (DtPA != null && DtPA.Rows.Count > 0)
                        {

                            PAPath = DtPA.Rows[0]["FileLocation"].ToString();
                            PAViewPath = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/PAs/" + PAId + ".xml";
                            UserNId = Global.SplitString(PAId, "_")[1].ToString();
                            ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                            ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0].Registration.id + ".xml";
                           
                        }
                        if (DataMetadata == "Data")
                        {
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=4 ;";
                        }
                        else if (DataMetadata == "Metadata")
                        {
                            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Id='" + DFDMFDId + "' AND Type=16 ;";
                        }
                        DtDFDMFD = DIConnection.ExecuteDataTable(Query);
                        if (DtDFDMFD != null && DtDFDMFD.Rows.Count > 0)
                        {
                            DFDMFDPath = DtDFDMFD.Rows[0]["FileLocation"].ToString();
                            DFDMFDViewPath = "../../" + DFDMFDPath.Substring(DFDMFDPath.LastIndexOf("stock")).Replace("\\", "/");
                        }
                        if (RowCounter % 2 == 0)
                        {
                            sb.Append("<tr class=\"DataRowStyle \" style=\"background-color:#D8D8DC \">");
                        }
                        else
                        {
                            sb.Append("<tr class=\"DataRowStyle \">");
                        }
                        sb.Append("<td class=\"DataColumnStyle \">" + Id + "</td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + QueryableDataUrl + "\" title=\"" + QueryableDataUrl + "\" target=\"_blank \" >" + QueryableData + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WADLUrl + "\" title=\"" + WADLUrl + "\" target=\"_blank \" >" + WADL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + WSDLUrl + "\" title=\"" + WSDLUrl + "\" target=\"_blank \" >" + WSDL + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + "<a href=\"" + SimpleDataUrl + "\" title=\"" + SimpleDataUrl + "\" target=\"_blank \" >" + SimpleData + "</a></td>");
                        sb.Append("<td class=\"DataColumnStyle \">" + DataMetadata + "</td>");
                        sb.Append("<td class=\"DataColumnStyle \"><a id=\"aView\" href=\" " + DFDMFDViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                        sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + DFDMFDPath + "' name=\"lang_Download\"></a>");
                        sb.Append("</td>");
                        sb.Append("<td class=\"DataColumnStyle \">");
                        if (DataMetadata == "Data")
                        {
                            sb.Append("<a id=\"aView\" href=\" " + ConstraintViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                            sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + ConstraintPath + "' name=\"lang_Download\"></a>");
                        }
                        sb.Append("</td>");
                        sb.Append("<td class=\"DataColumnStyle \"><a id=\"aView\" href=\" " + PAViewPath + "\" target=\"_blank\" name=\"lang_View\"></a> | ");
                        sb.Append("<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + PAPath + "' name=\"lang_Download\"></a>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                }

            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }


    //Data Metadata Grid for Data Tab starts

    private string GetRegistrationsAsPerData(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string DFDViewPath, DFDPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerDFD;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        CompleteFilePath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        RegistrationsPerDFD = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                for (i = 0; i < Complete.Structures.Dataflows.Count; i++)
                {
                    Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 AND Id='" + Complete.Structures.Dataflows[i].id + "';";
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

                    sb.Append("<div>");
                   // sb.Append("<img id=\"imgdivDFD_" + Complete.Structures.Dataflows[i].id + "\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivDFD_" + Complete.Structures.Dataflows[i].id + "' ,'divDFD_" + Complete.Structures.Dataflows[i].id + "')\" style=\"margin-right: 4px\" class=\"flt_lft\"/>");//expand.png
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append("<span  id=\"lang_data_Registration\"><b>Data Registrations</b></span>");
                    sb.Append("</b>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                 
                    sb.Append("<br/>");
                    sb.Append("<div id=\"divDFD_" + Complete.Structures.Dataflows[i].id + "\" style=\"margin-left: 20px;overflow:auto;height:auto;max-height:200px;\" >");//display:none;
                    RegistrationsPerDFD = BindRegistrationsPerDFD(DbNId, LanguageCode, Complete.Structures.Dataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerDFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"langReg_Id_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                       // sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + Complete.Structures.Dataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerDFD);
                        sb.Append("</table>");
                    }

                    sb.Append("</div>");
                    sb.Append("<br/>");
                }
            }

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerDataFor2_0(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string DFDViewPath, DFDPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtDFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerDFD;

        DIConnection = null;
        DFDViewPath = string.Empty;
        DFDPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        i = 0;
        RegistrationsPerDFD = string.Empty;

        try
        {
            SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=4 ;";
            DtDFD = DIConnection.ExecuteDataTable(Query);

            if (DtDFD != null && DtDFD.Rows.Count > 0)
            {
                for (i = 0; i < DtDFD.Rows.Count; i++)
                {

                    DFDPath = DtDFD.Rows[i]["FileLocation"].ToString();
                    DFDViewPath = "../../" + DFDPath.Substring(DFDPath.LastIndexOf("stock")).Replace("\\", "/");
                    DFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                    sb.Append("<div>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append("<span  id=\"lang_data_Registration\"><b>Data Registrations</b></span>");
                    sb.Append("</b>");
                    sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<div id=\"divDFD_" + DFD.Structures.Dataflows[0].id + "\" style=\"margin-left: 20px;overflow:auto;height:auto;max-height:200px;\" >");//display:none;
                    RegistrationsPerDFD = BindRegistrationsPerDFD(DbNId, LanguageCode, DFD.Structures.Dataflows[0].id);
                    if (string.IsNullOrEmpty(RegistrationsPerDFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"langReg_Id_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                     //   sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Data_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + DFD.Structures.Dataflows[0].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerDFD);
                        sb.Append("</table>");
                    }

                    sb.Append("</div>");
                    sb.Append("<br/>");

                }
            }


            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }
    

    private string GetRegistrationsAsPerMetadata(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string MFDViewPath, MFDPath, CompleteFilePath;
        DIConnection DIConnection;
        string Query;
        DataTable DtMFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerMFD;


        DIConnection = null;
        MFDViewPath = string.Empty;
        MFDPath = string.Empty;
        CompleteFilePath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        RegistrationsPerMFD = string.Empty;
        i = 0;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            CompleteFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Complete" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
            SDMXObjectModel.Message.StructureType Complete = new SDMXObjectModel.Message.StructureType();
            if (File.Exists(CompleteFilePath))
            {
                Complete = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CompleteFilePath);
                sb = new StringBuilder();
                for (i = 0; i < Complete.Structures.Metadataflows.Count; i++)
                {
                    Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 AND Id='" + Complete.Structures.Metadataflows[i].id + "';";
                    DtMFD = DIConnection.ExecuteDataTable(Query);

                    if (DtMFD != null && DtMFD.Rows.Count > 0)
                    {
                        MFDPath = DtMFD.Rows[0]["FileLocation"].ToString();
                        MFDViewPath = "../../" + MFDPath.Substring(MFDPath.LastIndexOf("stock")).Replace("\\", "/");

                    }
                    else
                    {
                        MFDPath = string.Empty;
                        MFDViewPath = string.Empty;
                    }
                    sb.Append("<div>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    if(Complete.Structures.Metadataflows[i].id.Contains("Indicator"))
                    {
                        sb.Append("<span  id=\"lang_metadata_Registration_Indicator\"><b>Metadata Registrations for Indicator</b></span>");
                    }
                    else if (Complete.Structures.Metadataflows[i].id.Contains("Area"))
                    {
                        sb.Append("<span  id=\"lang_metadata_Registration_Area\"><b>Metadata Registrations for Area</b></span>");
                    }
                 
                    else if (Complete.Structures.Metadataflows[i].id.Contains("Source"))
                    {
                        sb.Append("<span  id=\"lang_metadata_Registration_Source\"><b>Metadata Registrations for Source</b></span>");
                    }

                    sb.Append("</b>");
                  
                    sb.Append("</div>");
                    sb.Append("</div>");
                  
                    sb.Append("<br/>");

                    sb.Append("<div id=\"divMFD_" + Complete.Structures.Metadataflows[i].id + "\" style=\"margin-left: 20px;display:block;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerMFD = BindRegistrationsPerMFD(DbNId, LanguageCode, Complete.Structures.Metadataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerMFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"langReg_Id_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                      //  sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Metadata_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + Complete.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerMFD);
                        sb.Append("</table>");
                    }
                    //sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                }
            }
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string GetRegistrationsAsPerMetadataFor2_0(string DbNId, string LanguageCode)
    {
        string RetVal;
        RetVal = string.Empty;

        string MFDViewPath, MFDPath;
        DIConnection DIConnection;
        string Query;
        DataTable DtMFD;
        StringBuilder sb;
        int i;
        string RegistrationsPerMFD;


        DIConnection = null;
        MFDViewPath = string.Empty;
        MFDPath = string.Empty;
        RetVal = string.Empty;
        sb = new StringBuilder();
        RegistrationsPerMFD = string.Empty;
        i = 0;

        try
        {
            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);

            sb = new StringBuilder();
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DbNId) + " AND Type=16 ;";
            DtMFD = DIConnection.ExecuteDataTable(Query);

            if (DtMFD != null && DtMFD.Rows.Count > 0)
            {
                for (i = 0; i < DtMFD.Rows.Count; i++)
                {
                    MFDPath = DtMFD.Rows[i]["FileLocation"].ToString();
                    MFDViewPath = "../../" + MFDPath.Substring(MFDPath.LastIndexOf("stock")).Replace("\\", "/");
                    MFD = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);

                    sb.Append("<div>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append("<span  id=\"lang_metadata_Registration\"><b>Metadata Registrations</b></span>");
                    sb.Append("</b>");
                  
                    sb.Append("</div>");
                    sb.Append("</div>");
                   
                    sb.Append("<br/>");

                    sb.Append("<div id=\"divMFD_" + MFD.Structures.Metadataflows[i].id + "\" style=\"margin-left: 20px;display:block;overflow:auto;height:auto;max-height:200px;\" >");
                    RegistrationsPerMFD = BindRegistrationsPerMFD(DbNId, LanguageCode, MFD.Structures.Metadataflows[i].id);
                    if (string.IsNullOrEmpty(RegistrationsPerMFD))
                    {
                        sb.Append("<span  id=\"lang_No_Registration_Found\"><i>No Registration Found</i></span>");
                    }
                    else
                    {
                        sb.Append("<table style=\"width:100%; \" border=\"0\" cellSpacing=\"0\" cellSpacing=\"0\" class=\"roundedcorners\">");
                        sb.Append("<tr class=\"HeaderRowStyle \">");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span id=\"langReg_Id_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Queryable_Data_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WADL_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_WSDL_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"13%\"><span  id=\"lang_Simple_Data_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                     //   sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Metadata_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Constraints_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"10%\"><span  id=\"lang_Provision_Agreement_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("<td class=\"HeaderColumnStyle \" width=\"8%\"><span  id=\"lang_Data_Provider_" + MFD.Structures.Metadataflows[i].id + "\"></span></td>");
                        sb.Append("</tr>");
                        sb.Append(RegistrationsPerMFD);
                        sb.Append("</table>");
                    }
                    //sb.Append("</div>");
                    sb.Append("</div>");
                    sb.Append("<br/>");

                }

            }


            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }


    //Data Metadata Grid for Data Tab ends
    #endregion "--Private--"

    #region "--Public--"

    public string BindRegistrations(string requestParam)
    {
        string RetVal;
        string DbNId, Language, ArtefactType;
        string[] Params;
        bool DSDUploadedFromAdmin;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        ArtefactType = string.Empty;
        Params = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            ArtefactType = Params[2].ToString().Trim();
            DSDUploadedFromAdmin=IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
            if (ArtefactType == ArtefactTypes.DFD.ToString())
            {
                if (DSDUploadedFromAdmin)
                {
                    RetVal = this.GetRegistrationsAsPerDFDFor2_0(DbNId, Language);
                }
                else
                {
                    RetVal = this.GetRegistrationsAsPerDFD(DbNId, Language);
                }
            }
            else if (ArtefactType == ArtefactTypes.MFD.ToString())
            {
                if (DSDUploadedFromAdmin)
                {
                    RetVal = this.GetRegistrationsAsPerMFDFor2_0(DbNId, Language);
                }
                else
                {
                    RetVal = this.GetRegistrationsAsPerMFD(DbNId, Language);
                }
            }
            else if (ArtefactType == ArtefactTypes.PA.ToString())
            {
                if (DSDUploadedFromAdmin)
                {
                    RetVal = this.GetRegistrationsAsPerPAFor2_0(DbNId, Language);
                }
                else
                {
                    RetVal = this.GetRegistrationsAsPerPA(DbNId, Language);
                }
            }
            else if (ArtefactType == ArtefactTypes.DP.ToString())
            {
                RetVal = this.GetRegistrationsAsPerDP(DbNId, Language);
            }
           
        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string BindProvisionAgreements(string requestParam)
    {
        string RetVal;
        string DbNId, Language, ArtefactType;
        string[] Params;
        bool DSDUploadedFromAdmin;

        RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        ArtefactType = string.Empty;
        Params = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            ArtefactType = Params[2].ToString().Trim();
            DSDUploadedFromAdmin = IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));
            if (ArtefactType == ArtefactTypes.PA.ToString())
            {
                if (DSDUploadedFromAdmin)
                {
                    //RetVal = this.GetRegistrationsAsPerPAFor2_0(DbNId, Language);
                    RetVal = this.GetAgreementsAsPerPAFor2_0(DbNId, Language);
                }
                else
                {
                  //  RetVal = this.GetRegistrationsAsPerPA(DbNId, Language);
                    RetVal = this.GetAgreementsAsPerPA(DbNId, Language);
                    
                }
            }
          

        }
        catch (Exception ex)
        {
            RetVal += "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    
    }

    public string BindDataAndMetadataRegistration(string requestParam)
    {
        StringBuilder RetVal;
        string DbNId, Language, ArtefactType;
        string[] Params;
        bool DSDUploadedFromAdmin;

        //RetVal = string.Empty;
        DbNId = string.Empty;
        Language = string.Empty;
        ArtefactType = string.Empty;
        Params = null;
        RetVal = new StringBuilder();
        try
        {
            
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            Language = Params[1].ToString().Trim();
            ArtefactType = Params[2].ToString().Trim();
            DSDUploadedFromAdmin = IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId));

            if (DSDUploadedFromAdmin)
            {
                RetVal.Append(this.GetRegistrationsAsPerDataFor2_0(DbNId, Language));
                RetVal.Append(this.GetRegistrationsAsPerMetadataFor2_0(DbNId, Language));
            }
            else
            {
                  RetVal.Append(this.GetRegistrationsAsPerData(DbNId, Language));
                  RetVal.Append(this.GetRegistrationsAsPerMetadata(DbNId, Language));
            }

        }
        catch (Exception ex)
        {
            RetVal.Clear();
            RetVal.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal.ToString();
    }
   
    #endregion "--Public--"

    #endregion "--Methods--"


}
