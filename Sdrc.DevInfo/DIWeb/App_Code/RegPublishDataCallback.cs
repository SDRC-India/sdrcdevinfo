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

    #region "--GetRegistrationsSummary--"


    public string GetIndicatorName(string DbNId, string Language, string RegistrationId)
    {
        string RetVal;
        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;

        RetVal = string.Empty;
        SourceCodelistStructure = null;
        TargetCodelistStructure = null;
        MappingCodelistStructure = null;

        try
        {
            string FileName = Global.GetFileName(RegistrationId, DbNId);
            this.Get_Codelist_Source_Target_Mapping_Structure(DbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

            RetVal = this.GetIUSMappingList(DbNId, Language, SourceCodelistStructure, TargetCodelistStructure, MappingCodelistStructure, FileName);
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



    private string GetIUSMappingList(string DbNId, string Language, SDMXObjectModel.Message.StructureType SourceCodelistStructure, SDMXApi_2_0.Message.StructureType TargetCodelistStructure, SDMXObjectModel.Message.StructureType MappingCodelistStructure, string FileName)
    {
        string RetVal;
        string IndicatorGId;
        string IndicatorCodelistId;
        Dictionary<string, string> DictTargetIndicator, DictSourceIndicator;
        StringBuilder Builder;

        RetVal = string.Empty;
        IndicatorGId = string.Empty;

        DictTargetIndicator = null;
        IndicatorCodelistId = string.Empty;

        Builder = new StringBuilder(RetVal);
        string TargetIndicatorGid = string.Empty;

        string SourceIndicatorGid = string.Empty;
        string[] FileNameArray = null;
        string[] DI_IUSFileNameArray = null;
        string TargetIndicatorPart = string.Empty;
        string SourceIndicatorPart = string.Empty;
        string SourceIndicatorName = string.Empty;
        try
        {
            Global.GetAppSetting();

            if (TargetCodelistStructure != null)
            {
                foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
                {
                    if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
                    {
                        IndicatorCodelistId = Dimensions.codelist;
                    }


                }


                DictTargetIndicator = this.Get_DictTargetCodelist(Language, IndicatorCodelistId, TargetCodelistStructure);
            }
            DictSourceIndicator = GetSourceIndicatorCodeListMapping(Language, DbNId);
            if (string.IsNullOrEmpty(FileName) == false)
            {
                FileName = FileName.Replace(".xml", "");
                if (FileName.Contains("_DI_") || FileName.Contains("_DIMD_"))
                {
                    if (FileName.Contains("_DI_"))
                    {
                        FileNameArray = Global.SplitString(FileName, "_DI_");
                    }
                    else
                    {
                        FileNameArray = Global.SplitString(FileName, "_DIMD_");
                    }
                    TargetIndicatorPart = FileNameArray[0];
                    SourceIndicatorPart = FileNameArray[1];
                }
                else
                {
                    FileNameArray = Global.SplitString(FileName, "_" + Language + "_");
                    TargetIndicatorPart = FileNameArray[1];
                    DI_IUSFileNameArray = Global.SplitString(FileNameArray[0], "_");
                    SourceIndicatorPart = DI_IUSFileNameArray[0];
                    //  SourceIndicatorPart = FileName;
                }
                foreach (KeyValuePair<string, string> SourceIndicator in DictSourceIndicator)
                {

                    if (SourceIndicator.Key.Contains(SourceIndicatorPart))
                    {
                        SourceIndicatorName = SourceIndicator.Value;
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

        RetVal = SourceIndicatorName.ToString();
        return RetVal;
    }


    private string GetRegistrationsSummaryHTML(string DbNId, string UserNId, string LanguageCode, int StartIndex, int NumberPagingRows)
    {
        string RetVal;
        int TotalRows, Counter, TotalRowCount = 0, TempRowCount = 0;
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        Registry.RegistryService Service;
        XmlDocument Request;
        XmlElement Element;

        RetVal = string.Empty;
        TotalRows = 0;
        Counter = 0;
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        Service = null;
        Request = new XmlDocument();
        Element = null;

        try
        {


            RegistryInterfaceRequest = new RegistryInterfaceType();
            RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            RegistryInterfaceRequest.Footer = null;

            RegistryInterfaceRequest.Item = new SDMXObjectModel.Registry.QueryRegistrationRequestType();
            ((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item = new DataProviderReferenceType();
            ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items = new List<object>();
            ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items.Add(new DataProviderRefType(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version));

            Service = new Registry.RegistryService();
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Request = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceRequest);
            Element = Request.DocumentElement;
            string languageNid = Global.GetLangNidFromlangCode(LanguageCode);
            Service.QueryRegistration(ref Element, languageNid);
            RegistryInterfaceResponse = (RegistryInterfaceType)Deserializer.LoadFromText(typeof(RegistryInterfaceType), Element.OuterXml);
            TotalRows = ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RegistryInterfaceResponse.Item).QueryResult.Count;

            foreach (QueryResultType QueryResult in ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RegistryInterfaceResponse.Item).QueryResult)
            {
                if (StartIndex + NumberPagingRows <= TotalRows)
                {
                    if ((Counter >= StartIndex) && (Counter < StartIndex + NumberPagingRows))
                    {
                        TempRowCount = TotalRowCount;
                        RetVal += this.Get_InnerHtml_For_Single_Registration(DbNId, UserNId, LanguageCode, QueryResult.Item.Registration, TempRowCount, out TotalRowCount);
                        RetVal += Constants.Delimiters.PivotRowDelimiter;
                    }
                }
                else
                {
                    if ((Counter >= StartIndex) && (Counter < TotalRows))
                    {
                        TempRowCount = TotalRowCount;
                        RetVal += this.Get_InnerHtml_For_Single_Registration(DbNId, UserNId, LanguageCode, QueryResult.Item.Registration, TempRowCount, out TotalRowCount);
                        RetVal += Constants.Delimiters.PivotRowDelimiter;
                    }
                }
                if (RetVal.Contains("#"))
                {

                    RetVal = RetVal.Replace("##########", "#").Replace("#########", "#").Replace("########", "#").Replace("#######", "#").Replace("######", "#").Replace("#####", "#").Replace("####", "#").Replace("###", "#").Replace("##", "#");
                    if (RetVal.StartsWith("#"))
                    {
                        RetVal = RetVal.Substring(1);    
                    }                    
                }
                Counter++;
            }

            RetVal = RetVal.Remove(RetVal.Length - 1, 1);
            // TotalRows =TotalRowCount;

            RetVal += Constants.Delimiters.ParamDelimiter + StartIndex.ToString() + Constants.Delimiters.ParamDelimiter + NumberPagingRows.ToString() + Constants.Delimiters.ParamDelimiter + TotalRows.ToString();
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

    private string Get_InnerHtml_For_Single_Registration(string DbNId, string UserNId, string LanguageCode, RegistrationType Registration, int TempRowCount, out int TotalRows)
    {
        string RetVal = string.Empty;
        string DataUrlOriginal = string.Empty;
        string DataUrl = string.Empty;
        string WADLUrlOriginal = string.Empty;
        string WADLUrl = string.Empty;
        string WSDLUrlOriginal = string.Empty;
        string WSDLUrl = string.Empty;
        string SimpleDataFileUrlOriginal = string.Empty;
        string SimpleDataFileUrl = string.Empty;
        string ConstraintPath = string.Empty;
        string ConstraintViewPath = string.Empty;
        string DataMetadata = string.Empty;
        string DFDMFDId = string.Empty;
        string DFDMFDUrl = string.Empty;
        string PAId = string.Empty;
        string IndicatorName = string.Empty;
        string LanguageId = string.Empty;
        string LanguageName = string.Empty;
        string SimpleDataSource;
        string[] LanguageInSimpleDataSource = null;
        string language = string.Empty;
        string DbFolder = string.Empty;
        TotalRows = 0;
        string languageCode = string.Empty;

        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource;

        SimpleDataSource = string.Empty;
        QueryableDataSource = null;

        try
        {

            Global.Retrieve_SimpleAndQueryableDataSource_FromRegistration(Registration, out SimpleDataSource, out QueryableDataSource);
            DbFolder = Constants.FolderName.Data + DbNId + "\\";
            IndicatorName = GetIndicatorName(DbNId, LanguageCode, Registration.id);
            LanguageId = Global.GetLangNidFromlangCode(LanguageCode);
            LanguageName = Global.GetLanguageNameFromNid(LanguageId);
            LanguageInSimpleDataSource = SimpleDataSource.Split(new char[] { '/' }, StringSplitOptions.None);
            languageCode = LanguageInSimpleDataSource[LanguageInSimpleDataSource.Length - 2].ToString();
            if (TempRowCount != 0)
            {
                TotalRows = TempRowCount;
            }
            
            if (languageCode.Trim() == LanguageCode.Trim() && languageCode.Length == 2)
            {
                TotalRows += 1;
                RetVal += "" + Registration.id + "";
                RetVal += "~";

                ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + Registration.id + ".xml";
                ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + Registration.id + ".xml";

                if (!string.IsNullOrEmpty(SimpleDataSource))
                {
                    SimpleDataFileUrlOriginal = SimpleDataSource;
                    if (SimpleDataSource.Length > 25)
                    {
                        SimpleDataFileUrl = SimpleDataSource.Substring(0, 25) + "...";
                    }
                    else
                    {
                        SimpleDataFileUrl = SimpleDataSource;
                    }
                }

                if (QueryableDataSource != null)
                {
                    DataUrlOriginal = QueryableDataSource.DataURL;
                    if (QueryableDataSource.DataURL.Length > 25)
                    {
                        DataUrl = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                    }
                    else
                    {
                        DataUrl = QueryableDataSource.DataURL;
                    }

                    WADLUrlOriginal = QueryableDataSource.WADLURL;
                    if (QueryableDataSource.WADLURL.Length > 25)
                    {
                        WADLUrl = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                    }
                    else
                    {
                        WADLUrl = QueryableDataSource.WADLURL;
                    }

                    WSDLUrlOriginal = QueryableDataSource.WSDLURL;
                    if (QueryableDataSource.WSDLURL.Length > 25)
                    {
                        WSDLUrl = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                    }
                    else
                    {
                        WSDLUrl = QueryableDataSource.WSDLURL;
                    }
                }

                RetVal += "<span  style=\"padding-left: 10px;\" id=\"spanIndicatorName\">" + IndicatorName + "</span>";
                RetVal += "~";
                //RetVal += "<span id=\"spanLanguage\">" + LanguageName + "</span>";
                //RetVal += "~";
                RetVal += "" + "<a href=\"" + DataUrlOriginal + "\" title=\"" + DataUrlOriginal + "\" target=\"_blank \" >" + DataUrl + "</a>";
                RetVal += "~";
                RetVal += "" + "<a href=\"" + WADLUrlOriginal + "\" title=\"" + WADLUrlOriginal + "\" target=\"_blank \" >" + WADLUrl + "</a>";
                RetVal += "~";
                RetVal += "" + "<a href=\"" + WSDLUrlOriginal + "\" title=\"" + WSDLUrlOriginal + "\" target=\"_blank \" >" + WSDLUrl + "</a>";
                RetVal += "~";
                RetVal += "" + "<a href=\"" + SimpleDataFileUrlOriginal + "\" title=\"" + SimpleDataFileUrlOriginal + "\" target=\"_blank \" >" + SimpleDataFileUrl + "</a>";
                RetVal += "~";

                PAId = "PA_" + UserNId;

                DFDMFDId = Global.SplitString(((ProvisionAgreementRefType)(Registration.ProvisionAgreement.Items[0])).id, PAId + "_")[1].ToString();
                if (Global.SplitString(DFDMFDId, "_")[0].ToString() == "DF")
                {
                    DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/DFD.xml";
                    RetVal += "<span id=\"spanData\">Data</span>";

                    RetVal += "~";
                    RetVal += "<a href=\" " + DFDMFDUrl + "\"  ";
                    RetVal += " target=\"_blank\">" + GetDFDMFDNameById(DbNId, "DFD", DFDMFDId, LanguageCode) + "</a>";


                    RetVal += "~";
                    RetVal += "<a id=\"aView\" href=\" " + ConstraintViewPath + "\"  ";
                    RetVal += " target=\"_blank\">View</a> | ";
                    RetVal += "<a id=\"aDownload\" style=\"cursor:pointer;\" href='Download.aspx?fileId=" + ConstraintPath + "'>Download</a>";

                    RetVal += "~";
                    RetVal += "<a id=\"aView\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('V','" + Registration.id + "');\">View</a> | <a id=\"aEdit\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('U','" + Registration.id + "');\">Edit</a> | <a id=\"aDelete\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('D','" + Registration.id + "');\">Delete</a>";

                }
                else
                {
                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId)))
                    {
                        DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/" + DFDMFDId + ".xml";
                    }
                    else
                    {
                        if (DFDMFDId.Contains("Area"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Area.xml";
                        }
                        else if (DFDMFDId.Contains("Indicator"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Indicator.xml";
                        }
                        else if (DFDMFDId.Contains("Source"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Source.xml";
                        }
                    }
                    RetVal += "<span id=\"spanMetadata\">Metadata</span>";
                    RetVal += "~";
                    RetVal += "<a href=\" " + DFDMFDUrl + "\"  ";
                    RetVal += " target=\"_blank\">" + GetDFDMFDNameById(DbNId, "MFD", DFDMFDId, LanguageCode) + "</a>";


                    RetVal += "~";
                    RetVal += " ";
                    RetVal += " ";
                    RetVal += " ";

                    RetVal += "~";
                    RetVal += "<a id=\"aView\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('V','" + Registration.id + "');\">View</a> | <a id=\"aEdit\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('U','" + Registration.id + "');\">Edit</a> | <a id=\"aDelete\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('D','" + Registration.id + "');\">Delete</a>";

                    RetVal += "~";
                    RetVal += string.Empty;
                }
            }
            else
            {
                //Metadata
                if (SimpleDataSource.Contains("/Metadata"))
                {
                    TotalRows += 1;
                    RetVal += "" + Registration.id + "";
                    RetVal += "~";

                    ConstraintPath = "stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + Registration.id + ".xml";
                    ConstraintViewPath = "../../stock/data/" + DbNId + "/sdmx/Constraints/" + UserNId + "/" + "CNS_" + Registration.id + ".xml";

                    if (!string.IsNullOrEmpty(SimpleDataSource))
                    {
                        SimpleDataFileUrlOriginal = SimpleDataSource;
                        if (SimpleDataSource.Length > 25)
                        {
                            SimpleDataFileUrl = SimpleDataSource.Substring(0, 25) + "...";
                        }
                        else
                        {
                            SimpleDataFileUrl = SimpleDataSource;
                        }
                    }

                    if (QueryableDataSource != null)
                    {
                        DataUrlOriginal = QueryableDataSource.DataURL;
                        if (QueryableDataSource.DataURL.Length > 25)
                        {
                            DataUrl = QueryableDataSource.DataURL.Substring(0, 25) + "...";
                        }
                        else
                        {
                            DataUrl = QueryableDataSource.DataURL;
                        }

                        WADLUrlOriginal = QueryableDataSource.WADLURL;
                        if (QueryableDataSource.WADLURL.Length > 25)
                        {
                            WADLUrl = QueryableDataSource.WADLURL.Substring(0, 25) + "...";
                        }
                        else
                        {
                            WADLUrl = QueryableDataSource.WADLURL;
                        }

                        WSDLUrlOriginal = QueryableDataSource.WSDLURL;
                        if (QueryableDataSource.WSDLURL.Length > 25)
                        {
                            WSDLUrl = QueryableDataSource.WSDLURL.Substring(0, 25) + "...";
                        }
                        else
                        {
                            WSDLUrl = QueryableDataSource.WSDLURL;
                        }
                    }

                    RetVal += "<span  style=\"padding-left: 10px;\" id=\"spanIndicatorName\">" + IndicatorName + "</span>";
                    RetVal += "~";
                    //RetVal += "<span id=\"spanLanguage\">" + LanguageName + "</span>";
                    //RetVal += "~";
                    RetVal += "" + "<a href=\"" + DataUrlOriginal + "\" title=\"" + DataUrlOriginal + "\" target=\"_blank \" >" + DataUrl + "</a>";
                    RetVal += "~";
                    RetVal += "" + "<a href=\"" + WADLUrlOriginal + "\" title=\"" + WADLUrlOriginal + "\" target=\"_blank \" >" + WADLUrl + "</a>";
                    RetVal += "~";
                    RetVal += "" + "<a href=\"" + WSDLUrlOriginal + "\" title=\"" + WSDLUrlOriginal + "\" target=\"_blank \" >" + WSDLUrl + "</a>";
                    RetVal += "~";
                    RetVal += "" + "<a href=\"" + SimpleDataFileUrlOriginal + "\" title=\"" + SimpleDataFileUrlOriginal + "\" target=\"_blank \" >" + SimpleDataFileUrl + "</a>";
                    RetVal += "~";

                    PAId = "PA_" + UserNId;

                    DFDMFDId = Global.SplitString(((ProvisionAgreementRefType)(Registration.ProvisionAgreement.Items[0])).id, PAId + "_")[1].ToString();


                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId)))
                    {
                        DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/" + DFDMFDId + ".xml";
                    }
                    else
                    {
                        if (DFDMFDId.Contains("Area"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Area.xml";
                        }
                        else if (DFDMFDId.Contains("Indicator"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Indicator.xml";
                        }
                        else if (DFDMFDId.Contains("Source"))
                        {
                            DFDMFDUrl = "../../stock/data/" + DbNId + "/sdmx/Provisioning Metadata/MFD_Source.xml";
                        }
                    }
                    RetVal += "<span id=\"spanMetadata\">Metadata</span>";
                    RetVal += "~";
                    RetVal += "<a href=\" " + DFDMFDUrl + "\"  ";
                    RetVal += " target=\"_blank\">" + GetDFDMFDNameById(DbNId, "MFD", DFDMFDId, LanguageCode) + "</a>";


                    RetVal += "~";
                    RetVal += " ";
                    RetVal += " ";
                    RetVal += " ";

                    RetVal += "~";
                    RetVal += "<a id=\"aView\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('V','" + Registration.id + "');\">View</a> | <a id=\"aEdit\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('U','" + Registration.id + "');\">Edit</a> | <a id=\"aDelete\" style=\"cursor:pointer;\" onclick=\"OpenRegistrationDetailsPopup('D','" + Registration.id + "');\">Delete</a>";

                    RetVal += "~";
                    RetVal += string.Empty;
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

    private string GetDFDMFDNameById(string DBNId, string MFDOrDFD, string DFDMFDId, string lngcodedb)
    {
        string RetVal = string.Empty;
        DIConnection DIConnection = null;
        bool IsAdminUploadedDSD = false;
        string Query = string.Empty;
        string DFDPath = string.Empty;
        string MFDPath = string.Empty;
        DataTable DtDFD;
        DataTable DtMFDs;


        int i;
        try
        {

            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId));


            if (IsAdminUploadedDSD == false)
            {
                SDMXObjectModel.Message.StructureType SummaryStructure = new SDMXObjectModel.Message.StructureType();
                SummaryStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                if (MFDOrDFD == "DFD")
                {
                    for (i = 0; i < SummaryStructure.Structures.Dataflows.Count; i++)
                    {
                        if (SummaryStructure.Structures.Dataflows[i].id == DFDMFDId)
                        {
                            RetVal = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Dataflows[i].Name, lngcodedb);
                            RetVal = "DFD";
                            break;
                        }

                    }

                }
                else if (MFDOrDFD == "MFD")
                {
                    for (i = 0; i < SummaryStructure.Structures.Metadataflows.Count; i++)
                    {
                        if (SummaryStructure.Structures.Metadataflows[i].id == DFDMFDId)
                        {
                            RetVal = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Metadataflows[i].Name, lngcodedb);
                            RetVal = "MFD";
                            break;
                        }
                    }
                }
            }

            else
            {
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                    string.Empty, string.Empty);
                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Id='" + DFDMFDId + "' AND Type=" + Convert.ToInt32(ArtefactTypes.DFD).ToString() + ";";
                DtDFD = DIConnection.ExecuteDataTable(Query);


                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Id='" + DFDMFDId + "' AND Type=" + Convert.ToInt32(ArtefactTypes.MFD).ToString() + ";";
                DtMFDs = DIConnection.ExecuteDataTable(Query);

                if (MFDOrDFD == "DFD")
                {
                    if (DtDFD != null && DtDFD.Rows.Count > 0)
                    {
                        DFDPath = DtDFD.Rows[0]["FileLocation"].ToString();
                        SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
                        DFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                        RetVal = GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, lngcodedb);
                        RetVal = "DFD";
                    }
                }
                else if (MFDOrDFD == "MFD")
                {
                    if (DtMFDs != null && DtMFDs.Rows.Count > 0)
                    {
                        for (i = 0; i < DtMFDs.Rows.Count; i++)
                        {
                            MFDPath = DtMFDs.Rows[i]["FileLocation"].ToString();
                            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
                            MFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                            RetVal = GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[i].Name, lngcodedb);
                            RetVal = "MFD";
                        }
                    }
                }
            }


            //RetVal = RetVal.Remove(RetVal.Length - 1);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "--GetRegistrationsSummary--"

    #region "-- Send Mail For Registration --"

    //private void Frame_Message_And_Send_Registration_Mail(string UserNId, string RegistrationId)
    //{
    //    string MessageContent, FullName, EmailId;

    //    Global.GetAppSetting();

    //    if (Global.registryNotifyViaEmail == "true")
    //    {
    //        FullName = Global.Get_User_Full_Name(UserNId);
    //        EmailId = Global.Get_User_EmailId(UserNId);

    //        MessageContent = "Dear " + FullName + ",\n\nYou have successfully registered data/metadata at " + Global.adaptation_name +
    //                                 "(" + this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")) + ").";
    //        MessageContent += "\n\nRegistration Id: " + RegistrationId;

    //        MessageContent += "\n\nThank You.";
    //        MessageContent += "\n\nWith Best Regards,";
    //        MessageContent += "\n\nAdmin";
    //        MessageContent += "\n\n" + Global.adaptation_name;

    //        this.Send_Email(ConfigurationManager.AppSettings["NotificationSender"].ToString(), ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, "Data/Metadata Registration Details", MessageContent);
    //    }


    //}

    private void Frame_Message_And_Send_Registration_Mail(string UserNId, string RegistrationId, string Language, string IsDeleted = null)
    {
        string MessageContent = string.Empty;
        string FirstName, EmailId;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        Global.GetAppSetting();

        if (Global.registryNotifyViaEmail == "true")
        {
            FirstName = Global.Get_User_Full_Name(UserNId);
            EmailId = Global.Get_User_EmailId(UserNId);
            TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
            if (IsDeleted != null)
            {
                TamplatePath += Language + "\\" + Constants.FileName.RegistrationData_DeletedSDMX;
            }
            else
            {
                TamplatePath += Language + "\\" + Constants.FileName.RegistrationData_MetaDataSDMX;
            }
            MessageContent = GetEmailTamplate(TamplatePath);
            Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
            Subject = Subject.Replace("[^^^^]", "");
            Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
            Body = Body.Replace("[****]USER_NAME[****]", FirstName);
            Body = Body.Replace("[****]REGISTRATION_ID[****]", RegistrationId);
            Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
            Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
            this.Send_Email(Global.adaptation_name + " - WebMaster", ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, Subject, Body, true, FirstName, RegistrationId, "Registration");

        }
    }

    #endregion "-- Send Mail For Registration --"

    #endregion "--Private--"

    #region "--Public--"

    public string GetRegistrationsSummary(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DbNId, UserNId, LanguageCode, OriginalDBNId;
        int StartIndex, NumberPagingRows;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            UserNId = Params[1].ToString().Trim();
            LanguageCode = Params[2].ToString().Trim();

            if (!int.TryParse(Params[3].ToString().Trim(), out StartIndex))
            {
                StartIndex = 0;
            }

            Global.GetAppSetting();

            if (!int.TryParse(Global.registryPagingRows, out NumberPagingRows))
            {
                NumberPagingRows = 0;
            }

            //    OriginalDBNId = Params[4].ToString().Trim();
            RetVal = this.GetRegistrationsSummaryHTML(DbNId, UserNId, LanguageCode, StartIndex, NumberPagingRows);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string GetRegistrationDetails(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DbNId, UserNId, RegistrationId, DFDOrMFDId, LanguageCode;
        bool IsMetadata;
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        RegistrationType Registration;
        Registry.RegistryService Service;
        XmlDocument Request;
        XmlElement Element;
        SDMXObjectModel.Registry.QueryableDataSourceType QueryableDataSource = null;
        string SimpleDataSource = string.Empty;
        string DataUrl = string.Empty;
        string WADLUrl = string.Empty;
        string WSDLUrl = string.Empty;
        string SimpleDataFileUrl = string.Empty;
        bool IsRest = false;
        bool IsSoap = false;

        RetVal = string.Empty;
        DbNId = string.Empty;
        UserNId = string.Empty;
        RegistrationId = string.Empty;
        DFDOrMFDId = string.Empty;
        LanguageCode = string.Empty;
        IsMetadata = false;

        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        Registration = null;
        Service = null;
        Request = new XmlDocument();
        Element = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            UserNId = Params[1].ToString().Trim();
            RegistrationId = Params[2].ToString().Trim();
            LanguageCode = Params[3].ToString().Trim();
            RegistryInterfaceRequest = new RegistryInterfaceType();
            RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            RegistryInterfaceRequest.Footer = null;

            RegistryInterfaceRequest.Item = new SDMXObjectModel.Registry.QueryRegistrationRequestType();
            ((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item = new DataProviderReferenceType();
            ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items = new List<object>();
            ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items.Add(new DataProviderRefType
            (DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId,
             DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version));

            Service = new Registry.RegistryService();
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Request = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceRequest);
            Element = Request.DocumentElement;
            string languageNid = Global.GetLangNidFromlangCode(LanguageCode);
            Service.QueryRegistration(ref Element, languageNid);
            RegistryInterfaceResponse = (RegistryInterfaceType)Deserializer.LoadFromText(typeof(RegistryInterfaceType), Element.OuterXml);

            foreach (QueryResultType QueryResult in ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RegistryInterfaceResponse.Item).QueryResult)
            {
                if (QueryResult.Item.Registration.id == RegistrationId)
                {
                    Registration = QueryResult.Item.Registration;
                    break;
                }
            }

            DFDOrMFDId = ((ProvisionAgreementRefType)(Registration.ProvisionAgreement.Items[0])).id.Replace(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_", "");

            if (Registration.Datasource != null && Registration.Datasource.Count > 0 && Registration.Datasource.Count < 3)
            {
                if (Registration.Datasource.Count == 1)
                {
                    if (Registration.Datasource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                    {
                        QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[0];
                    }
                    else if (Registration.Datasource[0] is string)
                    {
                        SimpleDataSource = (string)Registration.Datasource[0];
                    }
                }
                else if (Registration.Datasource.Count == 2)
                {
                    if (Registration.Datasource[0] is SDMXObjectModel.Registry.QueryableDataSourceType)
                    {
                        QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[0];

                        if (Registration.Datasource[1] is string)
                        {
                            SimpleDataSource = (string)Registration.Datasource[1];
                        }
                    }
                    else if (Registration.Datasource[0] is string)
                    {
                        SimpleDataSource = (string)Registration.Datasource[0];

                        if (Registration.Datasource[1] is SDMXObjectModel.Registry.QueryableDataSourceType)
                        {
                            QueryableDataSource = (SDMXObjectModel.Registry.QueryableDataSourceType)Registration.Datasource[1];
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(SimpleDataSource))
            {
                SimpleDataFileUrl = SimpleDataSource;
            }

            if (QueryableDataSource != null)
            {
                DataUrl = QueryableDataSource.DataURL;
                WADLUrl = QueryableDataSource.WADLURL;
                WSDLUrl = QueryableDataSource.WSDLURL;
                IsRest = QueryableDataSource.isRESTDatasource;
                IsSoap = QueryableDataSource.isWebServiceDatasource;
            }

            RetVal = Registration.id + Constants.Delimiters.ParamDelimiter + DataUrl + Constants.Delimiters.ParamDelimiter + IsRest.ToString() +
                     Constants.Delimiters.ParamDelimiter + WADLUrl + Constants.Delimiters.ParamDelimiter + IsSoap.ToString() + Constants.Delimiters.ParamDelimiter +
                     WSDLUrl + Constants.Delimiters.ParamDelimiter + SimpleDataFileUrl;

            if (DFDOrMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
            {
                IsMetadata = false;
            }
            else
            {
                IsMetadata = true;
            }

            RetVal += Constants.Delimiters.ParamDelimiter + IsMetadata;
            RetVal += Constants.Delimiters.ParamDelimiter + DFDOrMFDId;
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string AddRegistration(string requestParam)
    {
        string RetVal;
        string[] Params;
        string[] DBDetails;
        string DbNId, UserNId, DFDOrMFDId, WebServiceURL, WADLURL, WSDLURL, FileURL, Language, RegistrationId, UploadedHeaderFileWPath, UploadedHeaderFolderPath, OriginalDBNId, SDMXFileName;
        bool IsREST, IsSOAP, IsMetadata;
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        Registry.RegistryService Service;
        XmlDocument Request;
        XmlElement Element;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.StructureHeaderType Header;
        RetVal = string.Empty;
        RegistrationId = string.Empty;
        UploadedHeaderFileWPath = string.Empty;
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        Service = null;
        Request = new XmlDocument();
        Element = null;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        OriginalDBNId = string.Empty;
        DBDetails = null;
        SDMXFileName = string.Empty;
        int index = 0;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
     
            DbNId = Params[0].ToString().Trim();
        
            UserNId = Params[1].ToString().Trim();
          
            IsMetadata = bool.Parse(Params[2].ToString().Trim());
            DFDOrMFDId = Params[3].ToString().Trim();
            WebServiceURL = Params[4].ToString().Trim();
            IsREST = bool.Parse(Params[5].ToString().Trim());
            WADLURL = Params[6].ToString().Trim();
            IsSOAP = bool.Parse(Params[7].ToString().Trim());
            WSDLURL = Params[8].ToString().Trim();
            FileURL = Params[9].ToString().Trim();
            Language = Params[10].ToString().Trim();
            OriginalDBNId = Params[11].ToString().Trim();
            if (Params.Length > 12)
            {

                SDMXFileName = Params[12].ToString().Trim();
            }
            else
            {
                
                SDMXFileName = Path.GetFileNameWithoutExtension(FileURL);

            }
             DBDetails = Global.GetDbConnectionDetails(DbNId.ToString());
            if (DbNId != OriginalDBNId && DBDetails[4] == "true")
            {
                UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + OriginalDBNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;


            }
            else
            {
                UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            }
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();

            if (File.Exists(UploadedHeaderFileWPath))
            {
                UploadedHeaderXml.Load(UploadedHeaderFileWPath);
                UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                Header = UploadedDSDStructure.Header;
            }

            RegistryInterfaceRequest = new RegistryInterfaceType();
            if (Header == null)
            {
                RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            }
            else
            {

                RegistryInterfaceRequest.Header.ID = Header.ID.ToString();
                RegistryInterfaceRequest.Header.Prepared = Header.Prepared.ToString();
                foreach (PartyType receiver in Header.Receiver)
                {
                    RegistryInterfaceRequest.Header.Receiver = new PartyType();
                    RegistryInterfaceRequest.Header.Receiver.Contact = receiver.Contact;
                    RegistryInterfaceRequest.Header.Receiver.id = receiver.id;
                    RegistryInterfaceRequest.Header.Receiver.Name = receiver.Name;
                }
                RegistryInterfaceRequest.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                RegistryInterfaceRequest.Header.Test = Header.Test;

            }
            RegistryInterfaceRequest.Footer = null;

            RegistryInterfaceRequest.Item = new SDMXObjectModel.Registry.SubmitRegistrationsRequestType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest = new List<RegistrationRequestType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest.Add(new RegistrationRequestType(null, ActionType.Append));
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration = new RegistrationType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement = new ProvisionAgreementReferenceType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items = new List<object>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items.Add(new ProvisionAgreementRefType(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_" + DFDOrMFDId, Global.Get_AgencyId_From_DFD(DbNId), DevInfo.Lib.DI_LibSDMX.Constants.PA.Version));

            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource = new List<object>();

            if (!string.IsNullOrEmpty(WebServiceURL))
            {
                ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource.Add(new SDMXObjectModel.Registry.QueryableDataSourceType(WebServiceURL, IsREST, WADLURL, IsSOAP, WSDLURL));
            }

            if (!string.IsNullOrEmpty(FileURL))
            {
                ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource.Add(FileURL);
            }

            Service = new Registry.RegistryService();
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Request = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceRequest);
            Element = Request.DocumentElement;
            string languageNid = Global.GetLangNidFromlangCode(Language);
            Service.SubmitRegistration(ref Element, SDMXFileName, languageNid);
            RegistryInterfaceResponse = (RegistryInterfaceType)Deserializer.LoadFromText(typeof(RegistryInterfaceType), Element.OuterXml);

            if (((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.status == StatusType.Success)
            {
                this.Frame_Message_And_Send_Registration_Mail(UserNId, ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].Registration.id, Language);
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].Registration.id;
             }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.MessageText[0].Text;
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

    public string UpdateRegistration(string requestParam)
    {
        string RetVal;
        string[] Params;
        string[] DBDetails;
        string DbNId, UserNId, DFDOrMFDId, RegistrationId, WebServiceURL, WADLURL, WSDLURL, FileURL, Language, UploadedHeaderFileWPath, UploadedHeaderFolderPath, OriginalDBNId, SDMXFileName;
        bool IsREST, IsSOAP, IsMetadata;
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        Registry.RegistryService Service;
        XmlDocument Request;
        XmlElement Element;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.StructureHeaderType Header;
        RetVal = string.Empty;
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        Service = null;
        Request = new XmlDocument();
        Element = null;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        DBDetails = null;
        OriginalDBNId = string.Empty;
        SDMXFileName = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            UserNId = Params[1].ToString().Trim();
            IsMetadata = bool.Parse(Params[2].ToString().Trim());
            DFDOrMFDId = Params[3].ToString().Trim();
            RegistrationId = Params[4].ToString().Trim();
            WebServiceURL = Params[5].ToString().Trim();
            IsREST = bool.Parse(Params[6].ToString().Trim());
            WADLURL = Params[7].ToString().Trim();
            IsSOAP = bool.Parse(Params[8].ToString().Trim());
            WSDLURL = Params[9].ToString().Trim();
            if (Params.Length > 10)
            {
                FileURL = Params[10].ToString().Trim();
            }
            else
            {
                FileURL = string.Empty;
            }
            if (Params.Length > 11)
            {
                Language = Params[11].ToString().Trim();
            }
            else
            {
                Language = Global.GetDefaultLanguageCode();
            }
            if (Params.Length > 12)
            {
                SDMXFileName = Params[12].ToString().Trim();
            }
            else
            {
                SDMXFileName = string.Empty;
            }
            RegistryInterfaceRequest = new RegistryInterfaceType();
            //RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            DBDetails = Global.GetDbConnectionDetails(DbNId);
            if (DBDetails[4] == "true")
            {
                OriginalDBNId = Convert.ToString(this.Get_AssociatedDB_NId(DbNId));
            }
            else
            {
                OriginalDBNId = DbNId;
            }
            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + OriginalDBNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();

            if (File.Exists(UploadedHeaderFileWPath))
            {
                UploadedHeaderXml.Load(UploadedHeaderFileWPath);
                UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                Header = UploadedDSDStructure.Header;
            }


            RegistryInterfaceRequest = new RegistryInterfaceType();
            if (Header == null)
            {
                RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            }
            else
            {

                RegistryInterfaceRequest.Header.ID = Header.ID.ToString();
                RegistryInterfaceRequest.Header.Prepared = Header.Prepared.ToString();
                foreach (PartyType receiver in Header.Receiver)
                {
                    RegistryInterfaceRequest.Header.Receiver = new PartyType();
                    RegistryInterfaceRequest.Header.Receiver.Contact = receiver.Contact;
                    RegistryInterfaceRequest.Header.Receiver.id = receiver.id;
                    RegistryInterfaceRequest.Header.Receiver.Name = receiver.Name;
                }
                RegistryInterfaceRequest.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                RegistryInterfaceRequest.Header.Test = Header.Test;

            }
            RegistryInterfaceRequest.Footer = null;

            RegistryInterfaceRequest.Item = new SDMXObjectModel.Registry.SubmitRegistrationsRequestType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest = new List<RegistrationRequestType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest.Add(new RegistrationRequestType(null, ActionType.Replace));
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration = new RegistrationType(RegistrationId);
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement = new ProvisionAgreementReferenceType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items = new List<object>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items.Add(new ProvisionAgreementRefType(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_" + DFDOrMFDId, Global.Get_AgencyId_From_DFD(DbNId), DevInfo.Lib.DI_LibSDMX.Constants.PA.Version));

            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource = new List<object>();
            if (!string.IsNullOrEmpty(WebServiceURL))
            {
                ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource.Add(new SDMXObjectModel.Registry.QueryableDataSourceType(WebServiceURL, IsREST, WADLURL, IsSOAP, WSDLURL));
            }

            if (!string.IsNullOrEmpty(FileURL))
            {
                ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.Datasource.Add(FileURL);
            }

            Service = new Registry.RegistryService();
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Request = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceRequest);
            Element = Request.DocumentElement;
            string languageNid = Global.GetLangNidFromlangCode(Language);
            Service.SubmitRegistration(ref Element, SDMXFileName, languageNid);
            RegistryInterfaceResponse = (RegistryInterfaceType)Deserializer.LoadFromText(typeof(RegistryInterfaceType), Element.OuterXml);

            if (((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.status == StatusType.Success)
            {
                this.Frame_Message_And_Send_Registration_Mail(UserNId, RegistrationId, Language);
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].Registration.id;
            }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.MessageText[0].Text;
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

    public string DeleteRegistration(string requestParam)
    {
        string RetVal;
        string[] Params;
        string[] DBDetails;
        string DbNId, UserNId, RegistrationId, DFDMFDId, Language, UploadedHeaderFileWPath, UploadedHeaderFolderPath, OriginalDBNId, SDMXFileName;
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        Registry.RegistryService Service;
        XmlDocument Request;
        XmlElement Element;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.StructureHeaderType Header;
        RetVal = string.Empty;
        DbNId = string.Empty;
        UserNId = string.Empty;
        RegistrationId = string.Empty;
        DFDMFDId = string.Empty;
        Language = string.Empty;
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        Service = null;
        Request = new XmlDocument();
        Element = null;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        DBDetails = null;
        OriginalDBNId = string.Empty;
        SDMXFileName = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            UserNId = Params[1].ToString().Trim();
            RegistrationId = Params[2].ToString().Trim();
            DFDMFDId = Params[3].ToString().Trim();
            if (Params.Length > 4)
            {
                Language = Params[4].ToString().Trim();
            }
            else
            {
                Language = Global.GetDefaultLanguageCode();
            }
            DBDetails = Global.GetDbConnectionDetails(DbNId);
            if (DBDetails[4] == "true")
            {
                OriginalDBNId = Convert.ToString(this.Get_AssociatedDB_NId(DbNId));
            }
            else
            {
                OriginalDBNId = DbNId;
            }

            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + OriginalDBNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();

            if (File.Exists(UploadedHeaderFileWPath))
            {
                UploadedHeaderXml.Load(UploadedHeaderFileWPath);
                UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                Header = UploadedDSDStructure.Header;
            }
            RegistryInterfaceRequest = new RegistryInterfaceType();
            // RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            if (Header == null)
            {
                RegistryInterfaceRequest.Header = Global.Get_Appropriate_Header();
            }
            else
            {

                RegistryInterfaceRequest.Header.ID = Header.ID.ToString();
                RegistryInterfaceRequest.Header.Prepared = Header.Prepared.ToString();
                foreach (PartyType receiver in Header.Receiver)
                {
                    RegistryInterfaceRequest.Header.Receiver = new PartyType();
                    RegistryInterfaceRequest.Header.Receiver.Contact = receiver.Contact;
                    RegistryInterfaceRequest.Header.Receiver.id = receiver.id;
                    RegistryInterfaceRequest.Header.Receiver.Name = receiver.Name;
                }
                RegistryInterfaceRequest.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                RegistryInterfaceRequest.Header.Test = Header.Test;

            }
            RegistryInterfaceRequest.Footer = null;

            RegistryInterfaceRequest.Item = new SDMXObjectModel.Registry.SubmitRegistrationsRequestType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest = new List<RegistrationRequestType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest.Add(new RegistrationRequestType(null, ActionType.Delete));
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration = new RegistrationType(RegistrationId);
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement = new ProvisionAgreementReferenceType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items = new List<object>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0].Registration.ProvisionAgreement.Items.Add(new ProvisionAgreementRefType(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_" + DFDMFDId, Global.Get_AgencyId_From_DFD(DbNId), DevInfo.Lib.DI_LibSDMX.Constants.PA.Version));

            Service = new Registry.RegistryService();
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Request = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceRequest);
            Element = Request.DocumentElement;
            string languageNid = Global.GetLangNidFromlangCode(Language);
            Service.SubmitRegistration(ref Element, SDMXFileName, languageNid);
            RegistryInterfaceResponse = (RegistryInterfaceType)Deserializer.LoadFromText(typeof(RegistryInterfaceType), Element.OuterXml);

            if (((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.status == StatusType.Success)
            {
                this.Frame_Message_And_Send_Registration_Mail(UserNId, RegistrationId, Language, "true");
                RetVal = "true" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].Registration.id;
            }
            else
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RegistryInterfaceResponse.Item).RegistrationStatus[0].StatusMessage.MessageText[0].Text;
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

    public string SendNotifications(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DbNId, UserNId, RegistrationId;
        bool IsMetadata;

        RetVal = "true";

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNId = Params[0].ToString().Trim();
            UserNId = Params[1].ToString().Trim();
            RegistrationId = Params[2].ToString().Trim();
            IsMetadata = bool.Parse(Params[3].ToString().Trim());

            Global.Send_Notifications_For_Subscriptions(DbNId, UserNId, RegistrationId, IsMetadata);
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

    public string GetDFDAndMFDList(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        string lngcodedb = string.Empty;
        string MFDOrDFD = string.Empty;
        DIConnection DIConnection = null;
        bool IsAdminUploadedDSD = false;
        string Query = string.Empty;
        string DFDPath = string.Empty;
        string MFDPath = string.Empty;
        DataTable DtDFD;
        DataTable DtMFDs;


        int i;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        DBNId = Params[0];
        lngcodedb = Params[1];
        MFDOrDFD = Params[2];
        try
        {

            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId));
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                      string.Empty, string.Empty);
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Type=" + Convert.ToInt32(ArtefactTypes.DFD).ToString() + ";";
            DtDFD = DIConnection.ExecuteDataTable(Query);


            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Type=" + Convert.ToInt32(ArtefactTypes.MFD).ToString() + ";";
            DtMFDs = DIConnection.ExecuteDataTable(Query);

            if (IsAdminUploadedDSD == false)
            {
                SDMXObjectModel.Message.StructureType SummaryStructure = new SDMXObjectModel.Message.StructureType();
                SummaryStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                if (MFDOrDFD == "DFD")
                {
                    for (i = 0; i < SummaryStructure.Structures.Dataflows.Count; i++)
                    {
                        RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Dataflows[i].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + SummaryStructure.Structures.Dataflows[i].id + Constants.Delimiters.Comma;
                    }

                }
                else if (MFDOrDFD == "MFD")
                {
                    for (i = 0; i < SummaryStructure.Structures.Metadataflows.Count; i++)
                    {
                        RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Metadataflows[i].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + SummaryStructure.Structures.Metadataflows[i].id + Constants.Delimiters.Comma;
                    }
                }
            }

            else
            {
                if (MFDOrDFD == "DFD")
                {
                    if (DtDFD != null && DtDFD.Rows.Count > 0)
                    {
                        DFDPath = DtDFD.Rows[0]["FileLocation"].ToString();
                        SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
                        DFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                        RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + DFD.Structures.Dataflows[0].id + Constants.Delimiters.Comma;
                    }

                }
                else if (MFDOrDFD == "MFD")
                {
                    if (DtMFDs != null && DtMFDs.Rows.Count > 0)
                    {
                        for (i = 0; i < DtMFDs.Rows.Count; i++)
                        {
                            MFDPath = DtMFDs.Rows[i]["FileLocation"].ToString();
                            SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
                            MFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                            RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[i].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + MFD.Structures.Metadataflows[i].id + Constants.Delimiters.Comma;

                        }
                    }
                }
            }


            RetVal = RetVal.Remove(RetVal.Length - 1);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
