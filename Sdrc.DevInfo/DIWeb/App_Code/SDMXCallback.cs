using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using SDMXObjectModel;
using SDMXObjectModel.Registry;
using System.Web.Configuration;
using System.Xml.Serialization;
using System.Text;
using DevInfo.Lib.DI_LibSDMX;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using SDMXObjectModel.Common;
using System.Web.Hosting;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Message;
using SDMXApi_2_0.Structure;

/// <summary>
/// Summary description for SDMXCallback
/// </summary>

public partial class Callback : System.Web.UI.Page
{
    #region "--SDMX Callbacks--"

    # region "--Public functions--"

    /// <summary>
    /// Function Apply the XSLT on the file and show in the page
    /// </summary>
    /// <param name="CodelistXMLPath">path of the file to be displayed</param>
    /// <remarks>Transformation base on XSLT</remarks>
    public string GetTransformedCodelist(string requestParam)
    {
        string RetVal;
        string CodelistXMLPath;
        string lngcode;
        string dbnid;
        string XSLTPath;
        RetVal = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

        try
        {
            // ::-> Create XMLDOC
            XmlDocument docXml = new XmlDocument();

            CodelistXMLPath = Params[0];
            lngcode = Params[1];
            dbnid = Params[2];

            // ::-> Load the Document

            docXml.Load(Server.MapPath(@"~/stock" + CodelistXMLPath));

            // ::-> Create XML Transformation doc
            XslCompiledTransform docXsl = new XslCompiledTransform();

            // ::-> Load the XSLT

            if (IsDSDUploadedFromAdmin(Convert.ToInt32(dbnid)) == true)
            {
                XSLTPath = "stock\\XSLT\\CodeLists_" + lngcode + "-v_2_0" + ".xsl";
            }
            else
            {
                XSLTPath = "stock\\XSLT\\CodeLists_" + lngcode + ".xsl";

            }

            docXsl.Load(Server.MapPath("~\\" + XSLTPath));

            // ::-> Set the Html to the page control to display
            System.IO.StringWriter writer = new System.IO.StringWriter();
            docXsl.Transform(docXml, null, writer);

            // ::-> Returning the Transformed XML       
            return writer.ToString();

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return string.Empty;
        }
    }

    private bool IsDSDUploadedFromAdmin(int DbNId)
    {
        bool Retval;
        Retval = false;
        string DBConnectionsFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNode xmlNode;

        try
        {
            DBConnectionsFile = Path.Combine(HostingEnvironment.MapPath("~"), ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBConnectionsFile);

            xmlNode = XmlDoc.SelectSingleNode("/" + Constants.XmlFile.Db.Tags.Root + "/" + Constants.XmlFile.Db.Tags.Category + "/" + Constants.XmlFile.Db.Tags.Database + "[@" + Constants.XmlFile.Db.Tags.DatabaseAttributes.Id + "=" + DbNId + "]");
            if (string.IsNullOrEmpty(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value))
            {
                Retval = false;
            }
            else
            {
                Retval = Convert.ToBoolean(xmlNode.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.SDMXDb].Value);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return Retval;
    }

    public string GetDPArtefact(string ArtefactPath)
    {
        try
        {
            string Retval;
            Retval = string.Empty;
            XmlDocument ArtefactXml;
            ArtefactXml = new XmlDocument();
            ArtefactXml.Load(ArtefactPath.Replace('$', '\\'));
            Retval = this.Get_Formatted_XML(ArtefactXml);
            return Retval;



        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return string.Empty;
        }
    }

    public string AddSubscription(string requestParam)
    {
        string RetVal;
        string[] Params;
        string Action;
        string NotificationMail;
        string NotificationHTTP;
        string SubscriberAssignedID;
        string StartDate;
        string EndDate;
        string EventSelector;
        string CategoriesGIDAndSchemeIds;
        string MFDId;
        string DbNId;
        string CodelistId;
        string SubscriberIdAndType;
        string Language;
        string UserNId;
        string AgencyId;
        string CategoryGID;
        string CategorySchemeId;
        string langPrefNid;
        string UploadedHeaderFileWPath, UploadedHeaderFolderPath;
        string OriginalDBNId;
        Registry.RegistryService Service;
        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.RegistryInterfaceType SubRegistryInterface;
        SDMXObjectModel.Message.StructureHeaderType Header;
        int IsSOAP;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        DateTime startDate;
        DateTime endDate;
        Dictionary<string, string> dictCategories;
        string[] DBDetails = null;
        RetVal = "false";
        Service = new Registry.RegistryService();
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;
        SubRegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        Action = string.Empty;
        NotificationMail = string.Empty;
        NotificationHTTP = string.Empty;
        IsSOAP = 0;
        SubscriberAssignedID = string.Empty;
        StartDate = string.Empty;
        EndDate = string.Empty;
        EventSelector = string.Empty;
        CategoriesGIDAndSchemeIds = string.Empty;
        MFDId = string.Empty;
        DbNId = string.Empty;
        CodelistId = string.Empty;
        SubscriberIdAndType = string.Empty;
        CategoryGID = string.Empty;
        CategorySchemeId = string.Empty;
        langPrefNid = string.Empty;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        DBDetails = null;
        UploadedHeaderFileWPath = string.Empty;
        SDMXObjectModel.Message.StructureType UploadedDSDStructure;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            Action = Params[0];
            NotificationMail = Params[1];
            NotificationHTTP = Params[2];
            IsSOAP = Convert.ToInt16(Params[3]);
            StartDate = Params[4];
            EndDate = Params[5];
            EventSelector = Params[6];
            CategoriesGIDAndSchemeIds = Params[7];
            MFDId = Params[8];
            DbNId = Params[9];
            SubscriberIdAndType = Params[10];
            Language = Params[11];
            langPrefNid = Params[12];
            OriginalDBNId = Params[13];
            AgencyId = Global.Get_AgencyId_From_DFD(DbNId.ToString());
            UserNId = SubscriberIdAndType.Split('|')[0];
            DBDetails = Global.GetDbConnectionDetails(DbNId);

            if (DbNId != OriginalDBNId && DBDetails[4] == "true")
            {
                UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + OriginalDBNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;


            }
            else
            {
                UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DbNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            }
            if (File.Exists(UploadedHeaderFileWPath))
            {
                UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
                UploadedHeaderXml.Load(UploadedHeaderFileWPath);
                UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                Header = UploadedDSDStructure.Header;
            }

            if (IsSOAP == 0)
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(false);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(false);
            }
            else
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(true);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(true);
            }
            notificationMailIds = new List<string>();
            notificationMailIds.Add(NotificationMail);
            notificationHTTPs = new List<string>();
            notificationHTTPs.Add(NotificationHTTP);

            startDate = DateTime.ParseExact(StartDate.ToString().Trim(), "dd-MM-yyyy", null);
            endDate = DateTime.ParseExact(EndDate.ToString().Trim(), "dd-MM-yyyy", null);
            dictCategories = new Dictionary<string, string>();
            if (EventSelector == "Data Registration")
            {
                foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
                {
                    CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                    CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                    dictCategories.Add(CategoryGID, CategorySchemeId);
                }
            }

            Query = GetQueryXmlDocumentForSubmitSubscription(ActionType.Append, SubscriberIdAndType, AgencyId, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, startDate, endDate, EventSelector, dictCategories, MFDId, string.Empty,Header);
            Element = Query.DocumentElement;
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();
            Service.SubmitSubscription(ref Element, langPrefNid);


            Response.LoadXml(Element.OuterXml);
            SubRegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)(SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Response));

            if (((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)(SubRegistryInterface.Item)).SubscriptionStatus[0].StatusMessage.status == SDMXObjectModel.Registry.StatusType.Success) // also update subscriber lang preference
            {
                RetVal = "true";
                if (Global.registryNotifyViaEmail == "true")
                {
                    this.Frame_Message_And_Send_Subscription_Mail(UserNId, ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)(SubRegistryInterface.Item)).SubscriptionStatus[0].SubscriptionURN, Language);
                }
            }
            else
            {
                RetVal = "false";
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

    public string UpdateSubscription(string requestParam)
    {
        string RetVal;
        string[] Params;
        string Action;
        string NotificationMail;
        string NotificationHTTP;
        int IsSOAP;
        string SubscriberAssignedID;
        string StartDate;
        string EndDate;
        string EventSelector;
        string CategoriesGIDAndSchemeIds;
        string MFDId;
        string DbNId;
        string CodelistId;
        string SubscriberIdAndType;
        string SubscriptionId;
        string CategoryGID;
        string CategorySchemeId;
        string UploadedHeaderFileWPath, UploadedHeaderFolderPath;
        string OriginalDBNId;
        string[] DBDetails;
        Registry.RegistryService Service;


        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.RegistryInterfaceType SubRegistryInterface;
        SDMXObjectModel.Message.StructureHeaderType Header;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        DateTime startDate;
        DateTime endDate;
        Dictionary<string, string> dictCategories;
        string AgencyId;
        string Language;
        string langPrefNid;

        RetVal = "true";
        Service = new Registry.RegistryService();
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;
        SubRegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        Header = new StructureHeaderType();
        Action = string.Empty;
        NotificationMail = string.Empty;
        NotificationHTTP = string.Empty;
        IsSOAP = 0;
        SubscriberAssignedID = string.Empty;
        StartDate = string.Empty;
        EndDate = string.Empty;
        EventSelector = string.Empty;
        CategoriesGIDAndSchemeIds = string.Empty;
        MFDId = string.Empty;
        DbNId = string.Empty;
        CodelistId = string.Empty;
        SubscriberIdAndType = string.Empty;
        CategoryGID = string.Empty;
        CategorySchemeId = string.Empty;
        isSOAPMailIds = new List<bool>();
        notificationMailIds = new List<string>();
        isSOAPHTTPs = new List<bool>();
        notificationHTTPs = new List<string>();
        dictCategories = new Dictionary<string, string>();
        AgencyId = string.Empty;
        langPrefNid = string.Empty;
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        DBDetails = null;
        OriginalDBNId = string.Empty;
        UploadedHeaderFileWPath = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            Action = Params[0];
            NotificationMail = Params[1];
            NotificationHTTP = Params[2];
            IsSOAP = Convert.ToInt16(Params[3]);
            StartDate = Params[4];
            EndDate = Params[5];
            EventSelector = Params[6];
            CategoriesGIDAndSchemeIds = Params[7];
            MFDId = Params[8];
            DbNId = Params[9];
            SubscriberIdAndType = Params[10];
            SubscriptionId = Params[11];
            Language = Params[12];
            langPrefNid = Params[13];
            OriginalDBNId = Params[14];
            AgencyId = Global.Get_AgencyId_From_DFD(DbNId.ToString());

            DBDetails = Global.GetDbConnectionDetails(DbNId);

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
            if (IsSOAP == 0)
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(false);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(false);
            }
            else
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(true);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(true);
            }
            notificationMailIds = new List<string>();
            notificationMailIds.Add(NotificationMail);
            notificationHTTPs = new List<string>();
            notificationHTTPs.Add(NotificationHTTP);

            startDate = DateTime.ParseExact(StartDate.ToString().Trim(), "dd-MM-yyyy", null);
            endDate = DateTime.ParseExact(EndDate.ToString().Trim(), "dd-MM-yyyy", null);
            dictCategories = new Dictionary<string, string>();
            if (EventSelector == "Data Registration")
            {
                foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
                {
                    CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                    CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                    dictCategories.Add(CategoryGID, CategorySchemeId);
                }
            }
            Query = GetQueryXmlDocumentForSubmitSubscription(ActionType.Replace, SubscriberIdAndType, AgencyId, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, startDate, endDate, EventSelector, dictCategories, MFDId, SubscriptionId,Header);
            Element = Query.DocumentElement;

            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();
            Service.SubmitSubscription(ref Element, langPrefNid);
            Response.LoadXml(Element.OuterXml);
            SubRegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)(SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Response));
            if (((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)(SubRegistryInterface.Item)).SubscriptionStatus[0].StatusMessage.status == SDMXObjectModel.Registry.StatusType.Success)
            {
                RetVal = "true";

                if (Global.registryNotifyViaEmail == "true")
                {
                    this.Frame_Message_And_Send_Subscription_Mail(SubscriberIdAndType.Split('|')[0], SubscriptionId, Language);
                }
            }
            else
            {
                RetVal = "false";
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

    public string DeleteSubscription(string requestParam)
    {
        string RetVal;
        string[] Params;
        string Action;
        string NotificationMail;
        string NotificationHTTP;
        int IsSOAP;
        string SubscriberAssignedID;
        string StartDate;
        string EndDate;
        string EventSelector;
        string CategoriesGIDAndSchemeIds;
        string MFDId;
        string DbNId;
        string CodelistId;
        string SubscriberIdAndType;
        string SubscriptionId;
        string CategoryGID;
        string CategorySchemeId;
        string UploadedHeaderFileWPath, UploadedHeaderFolderPath;
        string OriginalDBNId;
        string[] DBDetails;
        Registry.RegistryService Service;
        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        XmlDocument UploadedHeaderXml;
        SDMXObjectModel.Message.RegistryInterfaceType SubRegistryInterface;
        SDMXObjectModel.Message.StructureHeaderType Header;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        DateTime startDate;
        DateTime endDate;
        Dictionary<string, string> dictCategories;
        string AgencyId;

        RetVal = "true";
        Service = new Registry.RegistryService();
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;
        SubRegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        Action = string.Empty;
        NotificationMail = string.Empty;
        NotificationHTTP = string.Empty;
        IsSOAP = 0;
        SubscriberAssignedID = string.Empty;
        StartDate = string.Empty;
        EndDate = string.Empty;
        EventSelector = string.Empty;
        CategoriesGIDAndSchemeIds = string.Empty;
        MFDId = string.Empty;
        DbNId = string.Empty;
        CodelistId = string.Empty;
        SubscriberIdAndType = string.Empty;
        CategoryGID = string.Empty;
        CategorySchemeId = string.Empty;
        isSOAPMailIds = new List<bool>();
        notificationMailIds = new List<string>();
        isSOAPHTTPs = new List<bool>();
        notificationHTTPs = new List<string>();
        dictCategories = new Dictionary<string, string>();
        AgencyId = string.Empty;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        OriginalDBNId = string.Empty;
        DBDetails = null;
        UploadedHeaderFileWPath = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            Action = Params[0];
            NotificationMail = Params[1];
            NotificationHTTP = Params[2];
            IsSOAP = Convert.ToInt16(Params[3]);
            StartDate = Params[4];
            EndDate = Params[5];
            EventSelector = Params[6];
            CategoriesGIDAndSchemeIds = Params[7];
            MFDId = Params[8];
            DbNId = Params[9];
            SubscriberIdAndType = Params[10];
            SubscriptionId = Params[11];
            OriginalDBNId = Params[12];
            AgencyId = Global.Get_AgencyId_From_DFD(DbNId.ToString());

            DBDetails = Global.GetDbConnectionDetails(DbNId);

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

            if (IsSOAP == 0)
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(false);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(false);
            }
            else
            {
                isSOAPMailIds = new List<bool>();
                isSOAPMailIds.Add(true);
                isSOAPHTTPs = new List<bool>();
                isSOAPHTTPs.Add(true);
            }
            notificationMailIds = new List<string>();
            notificationMailIds.Add(NotificationMail);
            notificationHTTPs = new List<string>();
            notificationHTTPs.Add(NotificationHTTP);

            startDate = DateTime.ParseExact(StartDate.ToString().Trim(), "dd-MM-yyyy", null);
            endDate = DateTime.ParseExact(EndDate.ToString().Trim(), "dd-MM-yyyy", null);
            dictCategories = new Dictionary<string, string>();
            if (EventSelector == "Data Registration")
            {
                foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
                {
                    CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                    CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                    dictCategories.Add(CategoryGID, CategorySchemeId);
                }
            }
            Query = GetQueryXmlDocumentForSubmitSubscription(ActionType.Delete, SubscriberIdAndType, AgencyId, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, startDate, endDate, EventSelector, dictCategories, MFDId, SubscriptionId,Header);
            Element = Query.DocumentElement;

            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();

            Service.SubmitSubscription(ref Element, string.Empty);
            Response.LoadXml(Element.OuterXml);
            SubRegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)(SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Response));
            if (((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)(SubRegistryInterface.Item)).SubscriptionStatus[0].StatusMessage.status == SDMXObjectModel.Registry.StatusType.Success)
            {
                RetVal = "true";
            }
            else
            {
                RetVal = "false";
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

    public string GetSubscriptionDetails(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        string DbNId, UserIdAndType, SubscriptionId;
        XmlDocument SubscriptionDetailsXml = new XmlDocument();
        string CategoriesGIDAndSchemeIds = string.Empty;
        string CategoriesGIDAndSchemeId = string.Empty;
        string CategoryGId = string.Empty;
        string CategorySchemeId = string.Empty;
        string Email = string.Empty;
        string WebServiceAddress = string.Empty;
        bool IsSOAP = false;
        string StartDate = string.Empty;
        string EndDate = string.Empty;
        string EventType = string.Empty;
        string hlngcode = string.Empty;
        Registry.RegistryService Service;
        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        SDMXObjectModel.Message.RegistryInterfaceType SubRegistryInterface;
        int i;

        Service = new Registry.RegistryService();
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;
        SubRegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();

        try
        {
            SubscriptionId = Params[0];
            DbNId = Params[1].ToString().Trim();
            UserIdAndType = Params[2].ToString();
            hlngcode = Params[3].ToString().Trim();
            Query = GetQueryXmlDocumentOnTypeBasis(12, string.Empty, string.Empty, string.Empty, UserIdAndType);
            Element = Query.DocumentElement;
            Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
            Service.Url += "?p=" + DbNId.ToString();
            Service.QuerySubscription(ref Element);
            Response.LoadXml(Element.OuterXml);
            SubRegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)(SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Response));
            foreach (SDMXObjectModel.Registry.SubscriptionType Subscription in ((SDMXObjectModel.Registry.QuerySubscriptionResponseType)(SubRegistryInterface.Item)).Subscription)
            {
                if (SubscriptionId == Subscription.RegistryURN)
                {
                    Email = Subscription.NotificationMailTo[0].Value.ToString();
                    WebServiceAddress = Subscription.NotificationHTTP[0].Value.ToString();
                    IsSOAP = Subscription.NotificationHTTP[0].isSOAP;
                    StartDate = Subscription.ValidityPeriod.StartDate.ToString("dd-MM-yyyy");
                    EndDate = Subscription.ValidityPeriod.EndDate.ToString("dd-MM-yyyy");

                    RetVal = Email;
                    RetVal += Constants.Delimiters.ValuesDelimiter + WebServiceAddress;
                    RetVal += Constants.Delimiters.ValuesDelimiter + IsSOAP;
                    RetVal += Constants.Delimiters.ValuesDelimiter + StartDate;
                    RetVal += Constants.Delimiters.ValuesDelimiter + EndDate;

                    if (Subscription.EventSelector[0] is DataRegistrationEventsType)
                    {
                        EventType = GetlanguageBasedValueOfKey("lang_Data_Registration", hlngcode, "RegSubscription.xml");
                        for (i = 0; i < ((DataRegistrationEventsType)(Subscription.EventSelector[0])).Items.Length; i++)
                        {
                            CategoryGId = ((SDMXObjectModel.Common.CategoryRefType)(((CategoryReferenceType)(((DataRegistrationEventsType)(Subscription.EventSelector[0])).Items[i])).Items[0])).id;
                            CategorySchemeId = ((SDMXObjectModel.Common.CategoryRefType)(((CategoryReferenceType)(((DataRegistrationEventsType)(Subscription.EventSelector[0])).Items[i])).Items[0])).maintainableParentID;
                            CategoriesGIDAndSchemeIds = CategoriesGIDAndSchemeIds + CategoryGId + Constants.Delimiters.PivotColumnDelimiter + CategorySchemeId + ",";
                        }
                        CategoriesGIDAndSchemeIds = CategoriesGIDAndSchemeIds.Remove(CategoriesGIDAndSchemeIds.Length - 1, 1);
                        RetVal += Constants.Delimiters.ValuesDelimiter + EventType;
                        RetVal += Constants.Delimiters.ValuesDelimiter + CategoriesGIDAndSchemeIds;
                    }
                    else if (Subscription.EventSelector[0] is MetadataRegistrationEventsType)
                    {
                        EventType = GetlanguageBasedValueOfKey("lang_Metadata_Registration", hlngcode, "RegSubscription.xml");
                        RetVal += Constants.Delimiters.ValuesDelimiter + EventType;
                        RetVal += Constants.Delimiters.ValuesDelimiter + ((MaintainableQueryType)((MaintainableEventType)(((MetadataRegistrationEventsType)(Subscription.EventSelector[0])).Items[0])).Item).id;
                    }
                    else if (Subscription.EventSelector[0] is StructuralRepositoryEventsType)
                    {
                        EventType = GetlanguageBasedValueOfKey("lang_Structural_Metadata_Registration", hlngcode,
"RegSubscription.xml");
                        RetVal += Constants.Delimiters.ValuesDelimiter + EventType;
                    }

                    // get preferred language

                    RetVal += Constants.Delimiters.ValuesDelimiter + Global.GetPreferredLanguageFromSubscriptionId(SubscriptionId);
                    break;
                }

            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetICTypes(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        string SubscriptionSelectedCatGIDAndScheme = string.Empty;

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        DBNId = Params[0];
        if (Params.Length > 1)
        {
            SubscriptionSelectedCatGIDAndScheme = Params[1];
        }


        try
        {

            string Path = "";
            StringBuilder sb;
            string[] CategoryFiles;
            string[] Categories;
            int i;
            int FileNameLength;
            string Category;

            Path = System.IO.Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\data\\" + DBNId + "\\sdmx\\Categories\\");
            //Global.WriteErrorsInLog(Path);
            //Global.WriteErrorsInLog(System.IO.Path.Combine(Server.MapPath("~"), @"stock\\data\\" + DBNId + "\\sdmx\\Categories"));
            CategoryFiles = System.IO.Directory.GetFiles(Path);
            Categories = new string[7];
            for (i = 0; i < CategoryFiles.Length; i++)
            {
                FileNameLength = CategoryFiles[i].Length;
                Category = CategoryFiles[i].Substring(FileNameLength - 6, 2);
                if (Category == "SC")
                {
                    Categories[0] = Category;
                }

                if (Category == "GL")
                {
                    Categories[1] = Category;
                }


                if (Category == "SR")
                {
                    Categories[2] = Category;
                }


                if (Category == "IT")
                {
                    Categories[3] = Category;
                }


                if (Category == "TH")
                {
                    Categories[4] = Category;
                }


                if (Category == "CV")
                {
                    Categories[5] = Category;
                }

                if (Category == "FR")
                {
                    Categories[6] = Category;

                }

            }
            sb = new StringBuilder();
            sb.Append("<table class=content style=width:100%>");
            //Adding links for each IC Types
            sb.Append("<tr><td>");
            for (i = 0; i < Categories.Length; i++)
            {
                if (Categories[i] == "SC")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");
                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('SC','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('SC','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Sector</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "GL")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('GL','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('GL','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Goal</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "SR")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('SR','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('SR','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Source</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "IT")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('IT','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('IT','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }
                    if (i != (CategoryFiles.Length - 1))
                    {
                        sb.Append("   |   ");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Institution</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "TH")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('TH','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('TH','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Theme</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "CV")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('CV','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('CV','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Convention</a>");
                    sb.Append("   |   ");
                }
                else if (Categories[i] == "FR")
                {
                    sb.Append("<a href=\"javascript:void(0);\" ");


                    if (SubscriptionSelectedCatGIDAndScheme == "")
                    {
                        sb.Append(" onclick=\"BindCatScheme('FR','');\"");
                    }
                    else
                    {
                        sb.Append(" onclick=\"BindCatScheme('FR','" + SubscriptionSelectedCatGIDAndScheme + "');\"");
                    }

                    sb.Append(" style=\"color:#1e90ff;\">Framework</a>");
                    sb.Append("   |   ");
                }


            }
            sb.Remove(sb.Length - 8, 4);
            sb.Append("</td></tr>");
            sb.Append("</table>");
            RetVal = sb.ToString();

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            //Global.WriteErrorsInLog(ex.Message);
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetLoggedInUserSubscriptions(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBNId;
        string UserIdAndType;
        string hlngcode;
        StringBuilder sb;
        Registry.RegistryService Service;
        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        SDMXObjectModel.Message.RegistryInterfaceType SubRegistryInterface;
        string RegistryURN;
        string Email;
        string WebServiceAddress;
        string StartDate;
        string EndDate;
        string EventType;
        string Action;
        string prefLang;

        RetVal = string.Empty;
        Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        DBNId = Params[0];
        UserIdAndType = Params[1];
        hlngcode = Params[2];

        sb = new StringBuilder();
        Service = new Registry.RegistryService();
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;
        SubRegistryInterface = new SDMXObjectModel.Message.RegistryInterfaceType();
        RegistryURN = string.Empty;
        Email = string.Empty;
        WebServiceAddress = string.Empty;
        StartDate = string.Empty;
        EndDate = string.Empty;
        EventType = string.Empty;
        Action = string.Empty;
        prefLang = string.Empty;

        Query = GetQueryXmlDocumentOnTypeBasis(12, string.Empty, string.Empty, string.Empty, UserIdAndType);
        Element = Query.DocumentElement;

        Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
        Service.Url += "?p=" + DBNId.ToString();
        Service.QuerySubscription(ref Element);
        Response.LoadXml(Element.OuterXml);
        SubRegistryInterface = (SDMXObjectModel.Message.RegistryInterfaceType)(SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), Response));
        foreach (SDMXObjectModel.Registry.SubscriptionType Subscription in ((SDMXObjectModel.Registry.QuerySubscriptionResponseType)(SubRegistryInterface.Item)).Subscription)
        {
            RegistryURN = Subscription.RegistryURN;
            Email = Subscription.NotificationMailTo[0].Value.ToString();
            WebServiceAddress = Subscription.NotificationHTTP[0].Value.ToString();
            StartDate = Subscription.ValidityPeriod.StartDate.ToString("dd-MM-yyyy");
            EndDate = Subscription.ValidityPeriod.EndDate.ToString("dd-MM-yyyy");
            if (Subscription.EventSelector[0] is DataRegistrationEventsType)
            {
                EventType = GetlanguageBasedValueOfKey("lang_Data_Registration", hlngcode, "RegSubscription.xml");
            }
            else if (Subscription.EventSelector[0] is MetadataRegistrationEventsType)
            {
                EventType = GetlanguageBasedValueOfKey("lang_Metadata_Registration", hlngcode, "RegSubscription.xml");
            }
            else if (Subscription.EventSelector[0] is StructuralRepositoryEventsType)
            {
                EventType = GetlanguageBasedValueOfKey("lang_Structural_Metadata_Registration", hlngcode, "RegSubscription.xml");
            }

            // Get preferred language
            prefLang = Global.GetLanguageNameFromNid(Global.GetPreferredLanguageFromSubscriptionId(RegistryURN));

            sb.Append(RegistryURN);
            sb.Append("~");
            sb.Append(Email);
            sb.Append("~");
            sb.Append(WebServiceAddress);
            sb.Append("~");
            sb.Append(StartDate);
            sb.Append("~");
            sb.Append(EndDate);
            sb.Append("~");
            sb.Append(EventType);
            sb.Append("~");
            // Append preferred language
            sb.Append(prefLang);
            sb.Append("~");
            sb.Append("<a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenSubscriptionDetailsPopup('V','" + Subscription.RegistryURN + "' );\" name=\"lang_View\"></a> | ");
            sb.Append("<a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenSubscriptionDetailsPopup('U','" + Subscription.RegistryURN + "');\" name=\"lang_Edit\"></a> | ");
            sb.Append("<a style=\"cursor:pointer;\" href=\"javascript:void(0);\" onclick=\"OpenSubscriptionDetailsPopup('D','" + Subscription.RegistryURN + "');\" name=\"lang_Delete\"></a>");
            sb.Append(Constants.Delimiters.PivotRowDelimiter);
        }

        sb.Remove(sb.Length - 1, 1);
        RetVal = sb.ToString();
        return RetVal;
    }

    public string GetCategoryScheme(string requestParam)
    {
        StringBuilder sb;
        string RetVal = string.Empty;
        XmlDocument CategoryXml;
        string DbNId;
        string ICType;
        string SubscriptionSelectedCatGIDAndScheme = string.Empty;
        SDMXObjectModel.Structure.CategoryType Category;
        string chkboxvalue = string.Empty;
        int i = 0;

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        DbNId = Params[0];
        ICType = Params[1];
        if (Params.Length > 2)
        {
            SubscriptionSelectedCatGIDAndScheme = Params[2];
        }

        CategoryXml = new XmlDocument();
        CategoryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\data\\" + DbNId + "\\sdmx\\Categories\\" + ICType + ".xml"));
        SDMXObjectModel.Message.StructureType CatScheme = new SDMXObjectModel.Message.StructureType();
        CatScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), CategoryXml);
        sb = new StringBuilder();
        sb.Append("<ul id=" + ICType + ">");
        for (i = 0; i < CatScheme.Structures.CategorySchemes[0].Items.Count; i++)
        {
            Category = (SDMXObjectModel.Structure.CategoryType)(CatScheme.Structures.CategorySchemes[0].Items[i]);
            chkboxvalue = Category.id + "|" + CatScheme.Structures.CategorySchemes[0].id;

            if ((SubscriptionSelectedCatGIDAndScheme != "") && (CategoryAlreadySelected(chkboxvalue, SubscriptionSelectedCatGIDAndScheme)))
            {
                sb.Append("<li><input type=checkbox value=" + chkboxvalue + " checked=checked onclick=\"AddCategoryGId('" + chkboxvalue + "',event)\" /><label>" + Category.Name[0].Value + "</label>");
            }
            else
            {
                sb.Append("<li><input type=checkbox value=" + chkboxvalue + " onclick=\"AddCategoryGId('" + chkboxvalue + "',event)\" /><label>" + Category.Name[0].Value + "</label>");
            }
            sb.Append(Add_ChildCategory_To_Category(Category, CatScheme.Structures.CategorySchemes[0].id, SubscriptionSelectedCatGIDAndScheme));
            sb.Append("</li>");
        }
        sb.Append("</ul>");

        RetVal = sb.ToString();
        return RetVal;
    }

    public string GetEmailIdOfLoggedInUser(string UserNId)
    {
        string RetVal;
        DIConnection DIConnection;
        string Query;
        DataTable DtUser;

        RetVal = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT User_Email_Id FROM Users WHERE NId = " + Convert.ToInt32(UserNId.Split('|')[0]) + ";";
            DtUser = DIConnection.ExecuteDataTable(Query);

            if (DtUser != null && DtUser.Rows.Count > 0)
            {
                RetVal = DtUser.Rows[0]["User_Email_Id"].ToString();
            }
            else
            {
                RetVal = string.Empty;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string BindDPSchemeAndDFD(string requestParam)
    {
        string RetVal;
        string hdbnid, hlngcodedb;
        string DPSchemePath, MASchemePath, DPSchemeViewPath, MASchemeViewPath, DPSchemeName, DPSchemeDescription, MASchemeName, MASchemeDescription;
        string[] Params;
        StringBuilder sb;
        SDMXObjectModel.Message.StructureType DPScheme, MAScheme;

        RetVal = string.Empty;
        hdbnid = string.Empty;
        hlngcodedb = string.Empty;
        DPSchemePath = string.Empty;
        MASchemePath = string.Empty;
        DPSchemeViewPath = string.Empty;
        MASchemeViewPath = string.Empty;
        DPSchemeName = string.Empty;
        DPSchemeDescription = string.Empty;
        MASchemeName = string.Empty;
        MASchemeDescription = string.Empty;
        Params = null;
        sb = new StringBuilder();
        DPScheme = null;
        MAScheme = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            hdbnid = Params[0];
            hlngcodedb = Params[1];

            sb.Append("<table class=content style=width:100%>");
            sb.Append("<tr><td><ul>");

            DPSchemePath = Server.MapPath(Path.Combine("~", @"stock\\Users\\DataProviders.xml"));

            if (File.Exists(DPSchemePath))
            {
                DPScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DPSchemePath);
                DPSchemeName = GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Name, hlngcodedb);
                DPSchemeDescription = GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Description, hlngcodedb);
                DPSchemeViewPath = "../../" + DPSchemePath.Substring(DPSchemePath.LastIndexOf("stock")).Replace("\\", "/");

                sb.Append("<li>");
                sb.Append(DPSchemeName);

                if (!(string.IsNullOrEmpty(DPSchemeDescription)))
                {
                    sb.Append(" <span style=color:Gray><i>(");
                    sb.Append(DPSchemeDescription);
                    sb.Append(")</i></span> ");
                }

                sb.Append("&nbsp;<img id=\"imghelpDPScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpDPScheme')\" style=\"vertical-align:top; margin-top:-5px;cursor:pointer;\"  onmouseout=\"HideCallout('divCallout')\";/>");

                sb.Append("<br/>");

                sb.Append("<a href='" + DPSchemeViewPath + "'  ");
                sb.Append(" style=\"color:#1e90ff;\" target=\"_blank\" name=\"lang_View\"></a> | ");

                sb.Append("<a href='Download.aspx?fileId=" + DPSchemePath + "' style=color:#1e90ff;  name=\"lang_Download\"></a>");
                sb.Append("</li>");

                sb.Append("<br/>");
            }

            //MASchemePath = Server.MapPath(Path.Combine("~", @"stock\\Users\\MaintenanceAgencies.xml"));

            //if (File.Exists(MASchemePath))
            //{
            //    MAScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MASchemePath);
            //    MASchemeName = GetLangSpecificValue_For_Version_2_1(MAScheme.Structures.OrganisationSchemes[0].Name, hlngcodedb);
            //    MASchemeDescription = GetLangSpecificValue_For_Version_2_1(MAScheme.Structures.OrganisationSchemes[0].Description, hlngcodedb);
            //    MASchemeViewPath = "../../" + MASchemePath.Substring(MASchemePath.LastIndexOf("stock")).Replace("\\", "/");

            //    sb.Append("<li>");
            //    sb.Append(MASchemeName);

            //    if (!(string.IsNullOrEmpty(MASchemeDescription)))
            //    {
            //        sb.Append(" <span style=color:Gray><i>(");
            //        sb.Append(MASchemeDescription);
            //        sb.Append(")</i></span> ");
            //    }

            //    sb.Append("&nbsp;<img id=\"imghelpMAScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpMAScheme')\" style=\"vertical-align:top; margin-top:-5px;cursor:pointer;\"  onmouseout=\"HideCallout('divCallout')\";/>");

            //    sb.Append("<br/>");

            //    sb.Append("<a href='" + MASchemeViewPath + "'  ");
            //    sb.Append(" style=\"color:#1e90ff;\" target=\"_blank\" name=\"lang_View\"></a> | ");

            //    sb.Append("<a href='Download.aspx?fileId=" + MASchemePath + "' style=color:#1e90ff; name=\"lang_Download\"></a>");
            //    sb.Append("</li>");
            //}

            sb.Append("</ul></td>");
            sb.Append("</tr></table>");

            RetVal = sb.ToString();
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

    public string BindMASchemeAndDFD(string requestParam)
    {
        string RetVal;
        string hdbnid, hlngcodedb;
        string DPSchemePath, MASchemePath, DPSchemeViewPath, MASchemeViewPath, DPSchemeName, DPSchemeDescription, MASchemeName, MASchemeDescription;
        string[] Params;
        StringBuilder sb;
        SDMXObjectModel.Message.StructureType DPScheme, MAScheme;

        RetVal = string.Empty;
        hdbnid = string.Empty;
        hlngcodedb = string.Empty;
        DPSchemePath = string.Empty;
        MASchemePath = string.Empty;
        DPSchemeViewPath = string.Empty;
        MASchemeViewPath = string.Empty;
        DPSchemeName = string.Empty;
        DPSchemeDescription = string.Empty;
        MASchemeName = string.Empty;
        MASchemeDescription = string.Empty;
        Params = null;
        sb = new StringBuilder();
        DPScheme = null;
        MAScheme = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            hdbnid = Params[0];
            hlngcodedb = Params[1];

            sb.Append("<table class=content style=width:100%>");
            sb.Append("<tr><td><ul>");

            //DPSchemePath = Server.MapPath(Path.Combine("~", @"stock\\Users\\DataProviders.xml"));

            //if (File.Exists(DPSchemePath))
            //{
            //    DPScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DPSchemePath);
            //    DPSchemeName = GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Name, hlngcodedb);
            //    DPSchemeDescription = GetLangSpecificValue_For_Version_2_1(DPScheme.Structures.OrganisationSchemes[0].Description, hlngcodedb);
            //    DPSchemeViewPath = "../../" + DPSchemePath.Substring(DPSchemePath.LastIndexOf("stock")).Replace("\\", "/");

            //    sb.Append("<li>");
            //    sb.Append(DPSchemeName);

            //    if (!(string.IsNullOrEmpty(DPSchemeDescription)))
            //    {
            //        sb.Append(" <span style=color:Gray><i>(");
            //        sb.Append(DPSchemeDescription);
            //        sb.Append(")</i></span> ");
            //    }

            //    sb.Append("&nbsp;<img id=\"imghelpDPScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpDPScheme')\" style=\"vertical-align:top; margin-top:-5px;cursor:pointer;\"  onmouseout=\"HideCallout('divCallout')\";/>");

            //    sb.Append("<br/>");

            //    sb.Append("<a href='" + DPSchemeViewPath + "'  ");
            //    sb.Append(" style=\"color:#1e90ff;\" target=\"_blank\" name=\"lang_View\"></a> | ");

            //    sb.Append("<a href='Download.aspx?fileId=" + DPSchemePath + "' style=color:#1e90ff;  name=\"lang_Download\"></a>");
            //    sb.Append("</li>");

            //    sb.Append("<br/>");
            //}

            MASchemePath = Server.MapPath(Path.Combine("~", @"stock\\Users\\MaintenanceAgencies.xml"));

            if (File.Exists(MASchemePath))
            {
                MAScheme = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MASchemePath);
                MASchemeName = GetLangSpecificValue_For_Version_2_1(MAScheme.Structures.OrganisationSchemes[0].Name, hlngcodedb);
                MASchemeDescription = GetLangSpecificValue_For_Version_2_1(MAScheme.Structures.OrganisationSchemes[0].Description, hlngcodedb);
                MASchemeViewPath = "../../" + MASchemePath.Substring(MASchemePath.LastIndexOf("stock")).Replace("\\", "/");

                sb.Append("<li>");
                sb.Append(MASchemeName);

                if (!(string.IsNullOrEmpty(MASchemeDescription)))
                {
                    sb.Append(" <span style=color:Gray><i>(");
                    sb.Append(MASchemeDescription);
                    sb.Append(")</i></span> ");
                }

                sb.Append("&nbsp;<img id=\"imghelpMAScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpMAScheme')\" style=\"vertical-align:top; margin-top:-5px;cursor:pointer;\"  onmouseout=\"HideCallout('divCallout')\";/>");

                sb.Append("<br/>");

                sb.Append("<a href='" + MASchemeViewPath + "'  ");
                sb.Append(" style=\"color:#1e90ff;\" target=\"_blank\" name=\"lang_View\"></a> | ");

                sb.Append("<a href='Download.aspx?fileId=" + MASchemePath + "' style=color:#1e90ff; name=\"lang_Download\"></a>");
                sb.Append("</li>");
            }

            sb.Append("</ul></td>");
            sb.Append("</tr></table>");

            RetVal = sb.ToString();
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

    public string BindDataProviders(string requestParam)
    {
        string RetVal;
        DIConnection DIConnection;
        int NoOfDPs;
        StringBuilder sb;
        string PAPath = string.Empty;
        string PAViewPath = string.Empty;
        DataTable dt = new DataTable();
        bool IsAdminUploadedDSD;
        SDMXApi_2_0.Message.StructureType SummaryStructureFor2_0 = new SDMXApi_2_0.Message.StructureType();
        SDMXObjectModel.Message.StructureType SummaryStructureFor2_1 = new SDMXObjectModel.Message.StructureType();
        Dictionary<string, bool> DictColumns;

        DataTable DtDFD;
        DataTable DtMFDs;
        string Query = string.Empty;
        string DFDPath = string.Empty;
        string MFDPath = string.Empty;

        DIConnection = null;
        RetVal = string.Empty;
        string hdbnid = string.Empty;
        string hlngcodedb = string.Empty;
        string hlngcode = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        int i;

        hdbnid = Params[0];
        hlngcodedb = Params[1];
        hlngcode = Params[2];
        IsAdminUploadedDSD = false;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                        string.Empty, string.Empty);
            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(hdbnid) + " AND Type=4;";
            DtDFD = DIConnection.ExecuteDataTable(Query);


            Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(hdbnid) + " AND Type=16;";
            DtMFDs = DIConnection.ExecuteDataTable(Query);

            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(hdbnid));

            if (IsAdminUploadedDSD == false)
            {
                SummaryStructureFor2_1 = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));

            }

            sb = new StringBuilder();
            sb.Append(GetlanguageBasedValueOfKey("langFirstName", hlngcode, "RegProviders.xml"));
            sb.Append("~");
            sb.Append(GetlanguageBasedValueOfKey("langLastName", hlngcode, "RegProviders.xml"));
            sb.Append("~");
            sb.Append(GetlanguageBasedValueOfKey("langCountry", hlngcode, "RegProviders.xml"));
            sb.Append("~");
            sb.Append(GetlanguageBasedValueOfKey("langEmail", hlngcode, "RegProviders.xml"));
            sb.Append("~");

            DictColumns = new Dictionary<string, bool>();
            if (IsAdminUploadedDSD == true)
            {

                if (DtDFD != null && DtDFD.Rows.Count > 0)
                {
                    DFDPath = DtDFD.Rows[0]["FileLocation"].ToString();
                    SDMXObjectModel.Message.StructureType DFD = new SDMXObjectModel.Message.StructureType();
                    DFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DFDPath);
                    DictColumns.Add(DFD.Structures.Dataflows[0].id, false);
                    sb.Append(GetlanguageBasedValueOfKey("langProvisionAgreementFor", hlngcode, "RegProviders.xml") + GetLangSpecificValue_For_Version_2_1(DFD.Structures.Dataflows[0].Name, hlngcodedb));
                    sb.Append("~");

                }

                if (DtMFDs != null && DtMFDs.Rows.Count > 0)
                {
                    for (i = 0; i < DtMFDs.Rows.Count; i++)
                    {
                        MFDPath = DtMFDs.Rows[i]["FileLocation"].ToString();
                        SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
                        MFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                        DictColumns.Add(MFD.Structures.Metadataflows[i].id, false);
                        sb.Append(GetlanguageBasedValueOfKey("langProvisionAgreementFor", hlngcode, "RegProviders.xml") + GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[i].Name, hlngcodedb));
                        sb.Append("~");
                    }
                }


            }
            else
            {
                foreach (SDMXObjectModel.Structure.DataflowType DFD in SummaryStructureFor2_1.Structures.Dataflows)
                {
                    DictColumns.Add(DFD.id, false);
                    sb.Append(GetlanguageBasedValueOfKey("langProvisionAgreementFor", hlngcode, "RegProviders.xml") + GetLangSpecificValue_For_Version_2_1(DFD.Name, hlngcodedb));
                    sb.Append("~");
                }

                foreach (SDMXObjectModel.Structure.MetadataflowType MFD in SummaryStructureFor2_1.Structures.Metadataflows)
                {
                    DictColumns.Add(MFD.id, true);
                    sb.Append(GetlanguageBasedValueOfKey("langProvisionAgreementFor", hlngcode, "RegProviders.xml") + GetLangSpecificValue_For_Version_2_1(MFD.Name, hlngcodedb));
                    sb.Append("~");
                }

            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(Constants.Delimiters.ParamDelimiter);

            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                diworldwide_userinfo.UserLoginInformation Service = new diworldwide_userinfo.UserLoginInformation();
                Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                dt = Service.GetAllDataProviders(Global.GetAdaptationGUID());
            }
            else
            {
                Query = "Select NId,User_First_Name,User_Last_Name,User_Country as [User Country],User_Email_Id from Users where User_Is_Provider='True';";
                dt = DIConnection.ExecuteDataTable(Query);
            }

            dt = this.Replace_AreaNIds_With_Names(dt);


            NoOfDPs = dt.Rows.Count;

            // Binding Data Providers
            for (i = 0; i < NoOfDPs; i++)
            {
                if (Global.Is_Already_Existing_Provider(dt.Rows[i]["NId"].ToString()))
                {
                    sb.Append("" + dt.Rows[i]["User_First_Name"] + "");
                    sb.Append("~");
                    sb.Append("" + dt.Rows[i]["User_Last_Name"] + "");
                    sb.Append("~");
                    sb.Append("" + dt.Rows[i]["User Country"] + "");
                    sb.Append("~");
                    sb.Append("" + dt.Rows[i]["User_Email_Id"] + "");
                    sb.Append("~");

                    foreach (string Id in DictColumns.Keys)
                    {
                        PAPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + hdbnid + "\\sdmx\\Provisioning Metadata\\PAs\\" + "PA_" + dt.Rows[i]["NId"] + "_" + Id + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
                        PAViewPath = "../../" + PAPath.Substring(PAPath.LastIndexOf("stock")).Replace("\\", "/");

                        sb.Append("<a href=\" " + PAViewPath + "\"  ");
                        sb.Append(" target=\"_blank\" name=\"lang_View\"></a> | ");
                        sb.Append("<a href='Download.aspx?fileId=" + PAPath + "' name=\"lang_Download\"></a>");
                        sb.Append("~");
                    }

                    sb.Remove(sb.Length - 1, 1);

                    sb.Append(Constants.Delimiters.PivotRowDelimiter);
                }
            }

            sb.Remove(sb.Length - 1, 1);

            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;

        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;

    }

    public string ValidateSDMXML(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string SDMXMLFileName = string.Empty;
        string ValidationStatus = string.Empty;
        string ErrorDetails = string.Empty;
        string dataFileNameWPath;
        string completeFileNameWPath;
        Dictionary<string, string> dictValidationResponse;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;


        DbNId = Params[0];
        dataFileNameWPath = Params[1];
        dictValidationResponse = new Dictionary<string, string>();

        try
        {

            completeFileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\Complete.xml"));
            if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId)) == true)
            {
                dictValidationResponse = RegTwoZeroFunctionality.Validate_SDMXML_File_For_Version_2_0(dataFileNameWPath, completeFileNameWPath);
            }
            else
            {
                dictValidationResponse = SDMXUtility.Validate_SDMXML(SDMXSchemaType.Two_One, dataFileNameWPath, completeFileNameWPath);
            }

            foreach (string key in dictValidationResponse.Keys)
            {
                ValidationStatus = key;
                ErrorDetails = dictValidationResponse[key];

            }
            RetVal = BindSDMXMLValidation(ValidationStatus, ErrorDetails);
            File.Delete(dataFileNameWPath);
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;


        }
        finally
        {

        }

        return RetVal;

    }

    private string BindSDMXMLValidation(string ValidationStatus, string ValidationMessage)
    {

        StringBuilder RetVal;

        try
        {

            RetVal = new StringBuilder();

            if (ValidationStatus == SDMXValidationStatus.Xml_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ValidationMessage.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidCodesForDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >" + "Details" + "</a>");

            }
            else if (ValidationStatus == SDMXValidationStatus.SDMX_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ValidationMessage.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidCodesForDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == SDMXValidationStatus.Dimension_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ValidationMessage.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidCodesForDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == SDMXValidationStatus.Code_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidCodesForDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ValidationMessage.Replace("'", "\\'") + "');\" >Details</a>");

            }

            else
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");

                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");

                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidDimensionsMessage + "');\" >Details</a>");

                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidCodesForDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidCodesForDimensionsMessage + "');\" >Details</a>");

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

        return RetVal.ToString();

    }

    public string ValidateDSD(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string dsdFileNameWPath;
        string devinfodsdFileNameWPath;
        string ValidationStatus = string.Empty;
        string ErrorDetails = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        Dictionary<string, string> dictValidationResponse;
        DbNId = Params[0];
        dsdFileNameWPath = Params[1];
        dictValidationResponse = new Dictionary<string, string>();
        try
        {
            devinfodsdFileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\DSD.xml"));
            dictValidationResponse = SDMXUtility.Validate_DSDAgainstMasterDSD(SDMXSchemaType.Two_One, dsdFileNameWPath, devinfodsdFileNameWPath);
            foreach (string key in dictValidationResponse.Keys)
            {
                ValidationStatus = key;
                ErrorDetails = dictValidationResponse[key];

            }
            RetVal = BindDSDValidation(ValidationStatus, ErrorDetails);
            File.Delete(dsdFileNameWPath);
        }
        catch (Exception ex)
        {

            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {

        }

        return RetVal;

    }

    private string BindDSDValidation(string ValidationStatus, string ErrorDetails)
    {

        StringBuilder RetVal;

        try
        {

            RetVal = new StringBuilder();

            if (ValidationStatus == DSDValidationStatus.Xml_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");



            }
            else if (ValidationStatus == DSDValidationStatus.SDMX_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == DSDValidationStatus.DSD_Dimensions_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMandatoryDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == DSDValidationStatus.DSD_Attribute_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMandatoryDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidAllDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == DSDValidationStatus.DSD_Primary_Measure_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMandatoryDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidAllDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidAttributesMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");

            }

            else
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMandatoryDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMandatoryDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAllDimensions + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidAllDimensionsMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidAttributes + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidAttributesMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidPrimaryMeasure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidPrimaryMeasureMessage + "');\" >Details</a>");

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

        return RetVal.ToString();

    }

    public string ValidateMetadataReport(string requestParam)
    {
        string RetVal;
        string DbNId = string.Empty;
        string metadataFileNameWPath;
        string completeFileNameWPath;
        string ValidationStatus = string.Empty;
        string ErrorDetails = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        RetVal = string.Empty;
        Dictionary<string, string> dictValidationResponse;
        DbNId = Params[0];
        metadataFileNameWPath = Params[1];
        dictValidationResponse = new Dictionary<string, string>();
        string TargetAreaId = string.Empty;
        try
        {

            completeFileNameWPath = Server.MapPath(Path.Combine("~", @"stock\\data\\" + DbNId + "\\sdmx\\Complete.xml"));
            if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DbNId)) == true)
            {

                Global.GetAppSetting();
                TargetAreaId = Global.registryMSDAreaId;
                dictValidationResponse = RegTwoZeroFunctionality.Validate_MetadataReport_For_Version_2_0(metadataFileNameWPath, completeFileNameWPath, TargetAreaId);
            }
            else
            {
                dictValidationResponse = SDMXUtility.Validate_MetadataReport(SDMXSchemaType.Two_One, metadataFileNameWPath, completeFileNameWPath, string.Empty);
            }

            foreach (string key in dictValidationResponse.Keys)
            {
                ValidationStatus = key;
                ErrorDetails = dictValidationResponse[key];
            }
            RetVal = BindMetadataReportValidation(ValidationStatus, ErrorDetails);
            File.Delete(metadataFileNameWPath);
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

    private string BindMetadataReportValidation(string ValidationStatus, string ErrorDetails)
    {

        StringBuilder RetVal;

        try
        {

            RetVal = new StringBuilder();

            if (ValidationStatus == MetadataValidationStatus.Xml_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == MetadataValidationStatus.SDMX_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == MetadataValidationStatus.Referred_MSD_Invalid.ToString())
            {

                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
            }
            else if (ValidationStatus == MetadataValidationStatus.Metadata_Report_Invalid.ToString())
            {

                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReferredMSDMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
            }
            else if (ValidationStatus == MetadataValidationStatus.Metadata_Target_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReferredMSDMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataReportMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");
            }
            else if (ValidationStatus == MetadataValidationStatus.Metadata_Target_Object_Reference_Invalid.ToString())
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReferredMSDMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataReportMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataTargetMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTickGray\" alt='' src=\"../../stock/themes/default/images/tickmark_grey.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" style=\"color:Gray;\" onclick=\"this.removeAttribute('href');\" >Details</a>");

            }
            else if (ValidationStatus == MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString())
            {

                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReferredMSDMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataReportMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataTargetMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReferenceMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXError\" alt='' src=\"../../stock/themes/default/images/error.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + ErrorDetails.Replace("'", "\\'") + "');\" >Details</a>");
            }
            else
            {
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidXML + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidXMLMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidSDMXFile + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidSDMXFileMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReferredMSD + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReferredMSDMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataReport + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataReportMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTarget + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataTargetMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReference + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidMetadataTargetObjectReferenceMessage + "');\" >Details</a>");
                RetVal.Append(Constants.Delimiters.PivotRowDelimiter);
                RetVal.Append("" + Constants.SDMXValidationMessages.ValidReportStructure + "");
                RetVal.Append("~");
                RetVal.Append("<img id=\"imgSDMXTick\" alt='' src=\"../../stock/themes/default/images/tickmark.png\" />");
                RetVal.Append("~");
                RetVal.Append("<a href=\"javascript:void(0);\" onclick=\"ViewErrorDetails('" + Constants.SDMXValidationMessages.ValidReportStructureMessage + "');\" >Details</a>");
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

        return RetVal.ToString();

    }

    public string GetMSDList(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        string URL = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

        DBNId = Params[0];
        URL = Params[1];
        try
        {

            string Path = "";
            string[] MSDFiles;
            string MSDs;
            string MSDFileName;
            int i;
            this.EntryIntoGlobalCatalog(URL);
            Path = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\MSD"));
            MSDFiles = System.IO.Directory.GetFiles(Path);
            MSDs = string.Empty;
            for (i = 0; i < MSDFiles.Length; i++)
            {
                MSDFileName = System.IO.Path.GetFileNameWithoutExtension(MSDFiles[i]);
                MSDs = MSDs + MSDFileName + Constants.Delimiters.Comma;
            }
            if (MSDs.Length > 1)
            {
                MSDs = MSDs.Remove(MSDs.Length - 1);
            }
            RetVal = MSDs;
           
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetMSDAttributes(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        string MSDId = string.Empty;
        string lngcodedb = string.Empty;
        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        bool IsAdminUploadedDSD;

        DBNId = Params[0];
        lngcodedb = Params[1];
        MSDId = Params[2];


        try
        {
            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId));
            if (IsAdminUploadedDSD)
            {
                RetVal = BindMSDAttributes_For_MSD_2_0(DBNId, MSDId, lngcodedb);
            }
            else
            {
                RetVal = BindMSDAttributes_For_MSD_2_1(DBNId, MSDId, lngcodedb);
            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetMFDList(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        string lngcodedb = string.Empty;
        bool IsAdminUploadedDSD;
        int i, j;
        DIConnection DIConnection;
        string Query;
        DataTable DtMFDs;

        string MFDPath = string.Empty;
        string MFDName = string.Empty;
        string MFDDescription = string.Empty;

        string[] Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
        DBNId = Params[0];
        lngcodedb = Params[1];
        try
        {

            IsAdminUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId));
            if (IsAdminUploadedDSD)
            {
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                         string.Empty, string.Empty);
                Query = "SELECT * FROM Artefacts WHERE DBNId = " + Convert.ToInt32(DBNId) + " AND Type=16;";
                DtMFDs = DIConnection.ExecuteDataTable(Query);
                if (DtMFDs != null && DtMFDs.Rows.Count > 0)
                {
                    for (i = 0; i < DtMFDs.Rows.Count; i++)
                    {
                        MFDPath = DtMFDs.Rows[i]["FileLocation"].ToString();
                        SDMXObjectModel.Message.StructureType MFD = new SDMXObjectModel.Message.StructureType();
                        MFD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
                        RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(MFD.Structures.Metadataflows[0].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + MFD.Structures.Metadataflows[0].id + Constants.Delimiters.Comma;
                    }
                    if (RetVal.Length > 0)
                    {
                        RetVal = RetVal.Remove(RetVal.Length - 1);
                    }
                }
            }
            else
            {
                SDMXObjectModel.Message.StructureType SummaryStructure = new SDMXObjectModel.Message.StructureType();
                SummaryStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                for (i = 0; i < SummaryStructure.Structures.Metadataflows.Count; i++)
                {
                    RetVal = RetVal + GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Metadataflows[i].Name, lngcodedb) + Constants.Delimiters.ValuesDelimiter + SummaryStructure.Structures.Metadataflows[i].id + Constants.Delimiters.Comma;
                }
                if (RetVal.Length > 0)
                {
                    RetVal = RetVal.Remove(RetVal.Length - 1);
                }
            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string CheckIfUploadedDSD(string requestParam)
    {
        string RetVal = string.Empty;
        string DBNId = string.Empty;
        DBNId = requestParam;
        try
        {

            RetVal = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)).ToString();
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string BindHelpText(string requestParam)
    {
        string RetVal = string.Empty;
        string ArtefactType = string.Empty;
        ArtefactType = requestParam;
        XmlDocument XmlDoc;
        XmlNodeList xmlNodeList;
        XmlNode xmlNode;
        try
        {

            XmlDoc = new XmlDocument();
            XmlDoc.Load(Server.MapPath("~//stock//Help.xml"));

            xmlNodeList = XmlDoc.GetElementsByTagName(ArtefactType);
            if (xmlNodeList.Count > 0)
            {
                xmlNode = xmlNodeList[0];
                RetVal = xmlNode.InnerXml;
            }
            else
            {
                RetVal = string.Empty;
            }

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string CheckIfAdminLoggedIn(string requestParam)
    {
        string RetVal = string.Empty;
        string UserNId = string.Empty;
        string Query = string.Empty;
        UserNId = requestParam;
        RetVal = "false";

        try
        {
            if (this.isUserAdmin(UserNId) == true)
            {
                RetVal = "true";
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    # endregion "--Public functions--"

    # region "--Private functions--"

    private string BindMSDAttributes_For_MSD_2_1(string DBNId, string MSDId, string lngcodedb)
    {
        string RetVal = string.Empty;
        string ConceptName = string.Empty;
        string ConceptDescription = string.Empty;
        string CategorySchemeName = string.Empty;
        string CategorySchemeDescription = string.Empty;
        string CodelistName = string.Empty;
        string CodelistDescription = string.Empty;
        string ObjectType = string.Empty;
        XmlDocument MSDXml;
        XmlDocument SummaryXml;
        StringBuilder sb;
        StringBuilder sbChild;
        int i, j, k;
        SDMXObjectModel.Structure.MetadataAttributeType MetadataAttribute;
        SDMXObjectModel.Structure.IdentifiableObjectTargetType IdentifiableObjectTarget;
        SDMXObjectModel.Structure.IdentifiableObjectRepresentationType LocalRepresentation;
        SDMXObjectModel.Common.ItemSchemeReferenceType Enumeration;
        SDMXObjectModel.Structure.MetadataTargetType MetadataTarget;
        SDMXObjectModel.Structure.ReportStructureType ReportStructure;
        SDMXObjectModel.Common.ConceptReferenceType ConceptReference;
        SDMXObjectModel.Common.ConceptRefType ConceptRef;

        try
        {
            string MSDPath = "";
            string MSDViewPath = "";
            string MFDPath = "";
            string MFDViewPath = "";
            string ConceptSchemePath = "";
            string ConceptSchemeViewPath = "";
            string MFDId = "";
            string ConceptSchemeId = "";

            MSDPath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\MSD\\" + MSDId + ".xml"));
            MSDViewPath = "../../stock/data/" + DBNId + "/sdmx/MSD/" + MSDId + ".xml";
            MSDXml = new XmlDocument();
            MSDXml.Load(MSDPath);

            MFDId = MSDId.Replace("MSD", "MFD");
            MFDPath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\Provisioning Metadata\\" + MFDId + ".xml"));
            MFDViewPath = "../../stock/data/" + DBNId + "/sdmx/Provisioning Metadata/" + MFDId + ".xml";

            ConceptSchemeId = MSDId + "_Concepts";
            ConceptSchemePath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\Concepts\\" + ConceptSchemeId + ".xml"));
            ConceptSchemeViewPath = "../../stock/data/" + DBNId + "/sdmx/Concepts/" + ConceptSchemeId + ".xml";

            SummaryXml = new XmlDocument();

            SDMXObjectModel.Structure.StructuresType ConceptsObj;

            SDMXObjectModel.Structure.DataStructureComponentsType DSComponents = new DataStructureComponentsType();


            SDMXObjectModel.Message.StructureType SummaryStructure = new SDMXObjectModel.Message.StructureType();
            SummaryXml.Load(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
            SummaryStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), SummaryXml);

            DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(SummaryStructure.Structures.DataStructures[0].Item);
            ConceptsObj = SummaryStructure.Structures;

            SDMXObjectModel.Message.StructureType MSDStructure = new SDMXObjectModel.Message.StructureType();
            MSDStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), MSDXml);
            MetadataTarget = new MetadataTargetType();
            MetadataTarget = (MetadataTargetType)(((SDMXObjectModel.Structure.MetadataStructureComponentsType)MSDStructure.Structures.MetadataStructures[0].Item).Items[0]);
            ReportStructure = new SDMXObjectModel.Structure.ReportStructureType();
            ReportStructure = (SDMXObjectModel.Structure.ReportStructureType)(((SDMXObjectModel.Structure.MetadataStructureComponentsType)MSDStructure.Structures.MetadataStructures[0].Item).Items[1]);
            IdentifiableObjectTarget = ((SDMXObjectModel.Structure.IdentifiableObjectTargetType)(MetadataTarget.Items[0]));
            LocalRepresentation = ((SDMXObjectModel.Structure.IdentifiableObjectRepresentationType)(IdentifiableObjectTarget.LocalRepresentation));
            Enumeration = ((SDMXObjectModel.Common.ItemSchemeReferenceType)(LocalRepresentation.Items[0]));

            sb = new StringBuilder();
            //sb.Append("<div>");
            //sb.Append("<img id=\"imgdivCS\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivCS','divCS')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            //sb.Append("<h3 id=\"lang_Concept_Scheme_MSD\" class=\"flt_lft\"></h3>");
            //sb.Append("&nbsp;<img id=\"imghelpConceptScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\"  onclick=\"ToggleCallout('divCallout', event,  'divHelpCS')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            //sb.Append("</div>");
            //sb.Append("<br/>");
            //sb.Append("<div id=\"divCS\" style=\"display:none\">");
            //for (i = 0; i < SummaryStructure.Structures.Concepts.Count; i++)
            //{
            //    if (SummaryStructure.Structures.Concepts[i].id == ConceptSchemeId)
            //    {
            //        sb.Append("<div class=\"reg_li_sub_txt\">");
            //        sb.Append("<b>");
            //        sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Name, lngcodedb));
            //        sb.Append("</b>");
            //        sb.Append("<span class=\"reg_li_brac_txt\">(");
            //        sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Description, lngcodedb));
            //        sb.Append(")</span> ");
            //        sb.Append("</div>");
            //        sb.Append("<div>");
            //        sb.Append("<a href=\" " + ConceptSchemeViewPath + "\"  ");
            //        sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
            //        sb.Append("<a href='Download.aspx?fileId=" + ConceptSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
            //        sb.Append("</div>");
            //        sb.Append("<br/>");
            //        sb.Append("<ul>");
            //        for (j = 0; j < SummaryStructure.Structures.Concepts[i].Items.Count; j++)
            //        {
            //            sb.Append("<li>");

            //            sb.Append("<span>");
            //            sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Name, lngcodedb));
            //            sb.Append("</span>");
            //            if (!string.IsNullOrEmpty(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Description, lngcodedb)))
            //            {
            //                sb.Append("<span class=\"reg_li_brac_txt\">(");
            //                sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Description, lngcodedb));
            //                sb.Append(")</span>");
            //            }
            //            sb.Append("</li>");

            //        }
            //        sb.Append("</ul>");
            //        break;
            //    }
            //}
            //sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<img id=\"imgdivMSD\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMSD','divMSD')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Metadata_Structure_Definition\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpMSD\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpMSD')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div id=\"divMSD\" style=\"display:none\">");
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("<b>");
            sb.Append(GetLangSpecificValue_For_Version_2_1(MSDStructure.Structures.MetadataStructures[0].Name, lngcodedb));
            sb.Append("</b>");
            sb.Append("<span class=\"reg_li_brac_txt\">(");
            sb.Append(GetLangSpecificValue_For_Version_2_1(MSDStructure.Structures.MetadataStructures[0].Description, lngcodedb));
            sb.Append(")</span> ");
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<a href=\" " + MSDViewPath + "\"  ");
            sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
            sb.Append("<a href='Download.aspx?fileId=" + MSDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<h4 id=\"lang_Target\"></h4>");
            if (((SDMXObjectModel.Common.ItemSchemeRefType)(Enumeration.Items[0])).@class == ObjectTypeCodelistType.Codelist)
            {
                for (i = 0; i < SummaryStructure.Structures.Codelists.Count; i++)
                {
                    if (((SDMXObjectModel.Common.ItemSchemeRefType)(Enumeration.Items[0])).id == SummaryStructure.Structures.Codelists[i].id)
                    {
                        sb.Append("<div id=\"divTarget\" class=\"reg_li_sub_txt\">");
                        sb.Append("<ul class=\"reg_nonlst_txt\"><li>");
                        CodelistName = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Codelists[i].Name, lngcodedb).ToUpper();
                        CodelistDescription = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Codelists[i].Description, lngcodedb);
                        if (CodelistName != string.Empty)
                        {
                            sb.Append(CodelistName);
                        }
                        if (CodelistDescription != string.Empty)
                        {
                            sb.Append("<span class=\"reg_li_brac_txt\" >(");
                            sb.Append(CodelistDescription);
                            sb.Append(")</span>");
                        }
                        sb.Append("  -  ");
                        sb.Append("<span id=\"lang_Object_Type" + i + "\" ></span>");
                        sb.Append(IdentifiableObjectTarget.objectType.ToString());
                        sb.Append("</li>");
                        sb.Append("</ul>");
                        sb.Append("</div>");
                        break;
                    }
                }
            }
            else if (((SDMXObjectModel.Common.ItemSchemeRefType)(Enumeration.Items[0])).@class == ObjectTypeCodelistType.CategoryScheme)
            {
                for (i = 0; i < SummaryStructure.Structures.CategorySchemes.Count; i++)
                {
                    if (((SDMXObjectModel.Common.ItemSchemeRefType)(Enumeration.Items[0])).id == SummaryStructure.Structures.CategorySchemes[i].id)
                    {
                        sb.Append("<div id=\"divTarget\">");
                        sb.Append("<ul class=\"reg_nonlst_txt\"><li>");
                        CategorySchemeName = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.CategorySchemes[i].Name, lngcodedb).ToUpper();
                        CategorySchemeDescription = GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.CategorySchemes[i].Description, lngcodedb);
                        if (CategorySchemeName != string.Empty)
                        {
                            sb.Append(CategorySchemeName);
                        }
                        if (CategorySchemeDescription != string.Empty)
                        {
                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                            sb.Append(CategorySchemeDescription);
                            sb.Append(")</span>");
                        }

                        sb.Append("  -  ");
                        sb.Append("<span id=\"lang_Object_Type" + i + "\" ></span>");
                        sb.Append(IdentifiableObjectTarget.objectType.ToString());
                        sb.Append("</li>");
                        sb.Append("</ul>");
                        sb.Append("</div>");
                        break;
                    }
                }
            }

            sb.Append("<h4 id=\"lang_Report_Structure\"></h4>");
            sb.Append("<ul id=\"ulRS\">");
            for (i = 0; i < ReportStructure.Items.Count; i++)
            {
                sb.Append("<li>");
                MetadataAttribute = ((SDMXObjectModel.Structure.MetadataAttributeType)(ReportStructure.Items[i]));
                ConceptReference = MetadataAttribute.ConceptIdentity;
                ConceptRef = ((ConceptRefType)(ConceptReference.Items[0]));
                for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                {
                    if (ConceptRef.maintainableParentID == ConceptsObj.Concepts[j].id)
                    {
                        ConceptSchemeId = ConceptRef.maintainableParentID;
                        for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                        {
                            if (ConceptRef.id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                            {
                                ConceptName = GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Name, lngcodedb).ToUpper();
                                ConceptDescription = GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Description, lngcodedb);
                                if (ConceptName != string.Empty)
                                {
                                    sb.Append(ConceptName);
                                }
                                if (ConceptDescription != string.Empty)
                                {
                                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                                    sb.Append(ConceptDescription);
                                    sb.Append(")</span> ");
                                }
                                break;
                            }
                        }
                    }
                }
                sb.Append("<div class=\"reg_li_sub_txt\">");
                sb.Append("<span id=\"lang_Presentational" + i + "\" ></span>");
                if (MetadataAttribute.isPresentational == true)
                {
                    sb.Append("<span id=\"lang_Yes" + i + "\" ></span>");
                }
                else
                {
                    sb.Append("<span id=\"lang_No" + i + "\" ></span>");
                }
                sb.Append("</div>");
                sbChild = new StringBuilder();
                sb.Append(BindChildMetadatAttributes_For_Version_2_1(sbChild, MetadataAttribute, ConceptsObj, lngcodedb));
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<img id=\"imgdivMFD\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMFD','divMFD')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Metadata_Flow_Definition\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpMFD\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\"  onclick=\"ToggleCallout('divCallout', event,  'divHelpMFD')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div id=\"divMFD\" style=\"display:none\">");
            for (i = 0; i < SummaryStructure.Structures.Metadataflows.Count; i++)
            {
                if (MSDStructure.Structures.MetadataStructures[0].id == ((SDMXObjectModel.Common.MetadataStructureRefType)(SummaryStructure.Structures.Metadataflows[i].Structure.Items[0])).id)
                {
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Metadataflows[i].Name, lngcodedb));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Metadataflows[i].Description, lngcodedb));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + MFDViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + MFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");

                    break;
                }

            }
            //lang_Concept_Scheme changed to lang_Concept_Scheme_MSD
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<img id=\"imgdivCS\" src=\"../../stock/themes/default/images/expand.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivCS','divCS')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Concept_Scheme_MSD\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpConceptScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\"  onclick=\"ToggleCallout('divCallout', event,  'divHelpCS')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div id=\"divCS\" style=\"display:none\">");
            for (i = 0; i < SummaryStructure.Structures.Concepts.Count; i++)
            {
                if (SummaryStructure.Structures.Concepts[i].id == ConceptSchemeId)
                {
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Name, lngcodedb));
                    sb.Append("</b>");
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Description, lngcodedb));
                    sb.Append(")</span> ");
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + ConceptSchemeViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + ConceptSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<ul>");
                    for (j = 0; j < SummaryStructure.Structures.Concepts[i].Items.Count; j++)
                    {
                        sb.Append("<li>");

                        sb.Append("<span>");
                        sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Name, lngcodedb));
                        sb.Append("</span>");
                        if (!string.IsNullOrEmpty(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Description, lngcodedb)))
                        {
                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                            sb.Append(GetLangSpecificValue_For_Version_2_1(SummaryStructure.Structures.Concepts[i].Items[j].Description, lngcodedb));
                            sb.Append(")</span>");
                        }
                        sb.Append("</li>");

                    }
                    sb.Append("</ul>");
                    break;
                }
            }
            sb.Append("<div id=\"divMFD\">");
            RetVal = sb.ToString();

        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string BindMSDAttributes_For_MSD_2_0(string DBNId, string MSDId, string lngcodedb)
    {
        string RetVal = string.Empty;
        string ConceptName = string.Empty;
        string ConceptDescription = string.Empty;
        string CategorySchemeName = string.Empty;
        string CategorySchemeDescription = string.Empty;
        string CodelistName = string.Empty;
        string CodelistDescription = string.Empty;
        string ObjectType = string.Empty;
        XmlDocument MSDXml;
        StringBuilder sb;
        StringBuilder sbChild;
        int i, j, k, l;

        try
        {
            string MSDPath = "";
            string MSDViewPath = "";
            string MFDPath = "";
            string MFDViewPath = "";
            string ConceptSchemePath = "";
            string ConceptSchemeViewPath = "";
            string MFDId = "";
            string ConceptSchemeId = "";



            MSDPath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\MSD\\" + MSDId + ".xml"));
            MSDViewPath = "../../stock/data/" + DBNId + "/sdmx/MSD/" + MSDId + ".xml";
            MSDXml = new XmlDocument();
            MSDXml.Load(MSDPath);


            MFDId = "MF_" + MSDId;
            MFDPath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\Provisioning Metadata\\" + MFDId + ".xml"));
            MFDViewPath = "../../stock/data/" + DBNId + "/sdmx/Provisioning Metadata/" + MFDId + ".xml";



            SDMXApi_2_0.Message.StructureType SummaryStructure = new SDMXApi_2_0.Message.StructureType();

            SummaryStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBNId + "\\sdmx\\Summary" + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
            sb = new StringBuilder();
            sb.Append("<div>");
            sb.Append("<img id=\"imgdivMSD\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMSD','divMSD')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Metadata_Structure_Definition\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpMSD\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\" onclick=\"ToggleCallout('divCallout', event, 'divHelpMSD')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<div id=\"divMSD\" style=\"display:block\">");
            for (i = 0; i < SummaryStructure.MetadataStructureDefinitions.Count; i++)
            {
                if (SummaryStructure.MetadataStructureDefinitions[i].id == MSDId)
                {
                    sb.Append("<br/>");
                    sb.Append("<div class=\"reg_li_sub_txt\">");
                    sb.Append("<b>");
                    sb.Append(GetLangSpecificValueFor_Version_2_0(SummaryStructure.MetadataStructureDefinitions[i].Name, lngcodedb));
                    sb.Append("</b>");
                    if (SummaryStructure.MetadataStructureDefinitions[i].Description.Count > 0)
                    {
                        sb.Append("<span class=\"reg_li_brac_txt\">(");
                        sb.Append(GetLangSpecificValueFor_Version_2_0(SummaryStructure.MetadataStructureDefinitions[i].Description, lngcodedb));
                        sb.Append(")</span> ");
                    }
                    sb.Append("</div>");
                    sb.Append("<div>");
                    sb.Append("<a href=\" " + MSDViewPath + "\"  ");
                    sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
                    sb.Append("<a href='Download.aspx?fileId=" + MSDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
                    sb.Append("</div>");
                    sb.Append("<br/>");
                    sb.Append("<h4 id=\"lang_Target\"></h4>");
                    sb.Append("<div id=\"divTarget\" class=\"reg_li_sub_txt\">");
                    sb.Append("<ul class=\"reg_nonlst_txt\">");
                    foreach (IdentifierComponentType IdentifierComponent in SummaryStructure.MetadataStructureDefinitions[i].TargetIdentifiers.FullTargetIdentifier.IdentifierComponent)
                    {
                        if (IdentifierComponent.RepresentationScheme.representationSchemeType == RepresentationSchemeTypeType.Codelist)
                        {
                            sb.Append("<li>");
                            for (j = 0; j < SummaryStructure.KeyFamilies[0].Components.Dimension.Count; j++)
                            {
                                if (IdentifierComponent.id == SummaryStructure.KeyFamilies[0].Components.Dimension[j].conceptRef)
                                {
                                    for (k = 0; k < SummaryStructure.CodeLists.Count; k++)
                                    {
                                        if (SummaryStructure.KeyFamilies[0].Components.Dimension[j].codelist == SummaryStructure.CodeLists[k].id)
                                        {

                                            CodelistName = GetLangSpecificValueFor_Version_2_0(SummaryStructure.CodeLists[k].Name, lngcodedb).ToUpper();
                                            CodelistDescription = GetLangSpecificValueFor_Version_2_0(SummaryStructure.CodeLists[k].Description, lngcodedb);
                                            if (CodelistName != string.Empty)
                                            {
                                                sb.Append(CodelistName);
                                            }
                                            if (CodelistDescription != string.Empty)
                                            {
                                                sb.Append("<span class=\"reg_li_brac_txt\" >(");
                                                sb.Append(CodelistDescription);
                                                sb.Append(")</span> ");
                                            }
                                            sb.Append("  -  ");
                                            sb.Append("<span id=\"lang_Object_Type" + i + "\" ></span>");
                                            sb.Append("<span class=\"reg_li_brac_txt\" >");
                                            sb.Append(IdentifierComponent.RepresentationScheme.representationSchemeType.ToString());
                                            sb.Append("</span> ");
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            sb.Append("</li>");
                        }
                        else if (IdentifierComponent.RepresentationScheme.representationSchemeType == RepresentationSchemeTypeType.Category)
                        {
                            sb.Append("<li>");
                            for (j = 0; j < SummaryStructure.KeyFamilies[0].Components.Dimension.Count; j++)
                            {
                                if (IdentifierComponent.id == SummaryStructure.KeyFamilies[0].Components.Dimension[j].conceptRef)
                                {
                                    for (k = 0; k < SummaryStructure.CategorySchemes.Count; k++)
                                    {
                                        if (SummaryStructure.KeyFamilies[0].Components.Dimension[j].conceptSchemeRef == SummaryStructure.CategorySchemes[k].id)
                                        {

                                            CategorySchemeName = GetLangSpecificValueFor_Version_2_0(SummaryStructure.CategorySchemes[k].Name, lngcodedb).ToUpper();
                                            CategorySchemeDescription = GetLangSpecificValueFor_Version_2_0(SummaryStructure.CategorySchemes[k].Description, lngcodedb);
                                            if (CategorySchemeName != string.Empty)
                                            {
                                                sb.Append(CategorySchemeName);
                                            }
                                            if (CategorySchemeDescription != string.Empty)
                                            {
                                                sb.Append("<span class=\"reg_li_brac_txt\" >(");
                                                sb.Append(CategorySchemeDescription);
                                                sb.Append(")</span> ");
                                            }
                                            sb.Append("  -  ");
                                            sb.Append("<span id=\"lang_Object_Type" + i + "\" ></span>");
                                            sb.Append("<span class=\"reg_li_brac_txt\" >");
                                            sb.Append(IdentifierComponent.RepresentationScheme.representationSchemeType.ToString());
                                            sb.Append("</span> ");
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                            sb.Append("</li>");
                        }
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>");
                    break;
                }
            }

            for (i = 0; i < SummaryStructure.MetadataStructureDefinitions.Count; i++)
            {
                if (SummaryStructure.MetadataStructureDefinitions[i].id == MSDId)
                {
                    sb.Append("<h4 id=\"lang_Report_Structure\"></h4>");
                    sb.Append("<ul id=\"ulRS\">");
                    for (j = 0; j < SummaryStructure.MetadataStructureDefinitions[i].ReportStructure[0].MetadataAttribute.Count; j++)
                    {
                        sb.Append("<li>");
                        for (k = 0; k < SummaryStructure.Concepts.ConceptScheme.Count; k++)
                        {
                            if (SummaryStructure.MetadataStructureDefinitions[i].ReportStructure[0].MetadataAttribute[j].conceptSchemeRef == SummaryStructure.Concepts.ConceptScheme[k].id)
                            {
                                ConceptSchemeId = SummaryStructure.MetadataStructureDefinitions[i].ReportStructure[0].MetadataAttribute[j].conceptSchemeRef;
                                for (l = 0; l < SummaryStructure.Concepts.ConceptScheme[k].Concept.Count; l++)
                                {
                                    if (SummaryStructure.MetadataStructureDefinitions[i].ReportStructure[0].MetadataAttribute[j].conceptRef == SummaryStructure.Concepts.ConceptScheme[k].Concept[l].id)
                                    {
                                        ConceptName = GetLangSpecificValueFor_Version_2_0(SummaryStructure.Concepts.ConceptScheme[k].Concept[l].Name, lngcodedb).ToUpper();
                                        ConceptDescription = GetLangSpecificValueFor_Version_2_0(SummaryStructure.Concepts.ConceptScheme[k].Concept[l].Description, lngcodedb);
                                        if (ConceptName != string.Empty)
                                        {
                                            sb.Append(ConceptName);
                                        }
                                        if (ConceptDescription != string.Empty)
                                        {
                                            sb.Append("<span class=\"reg_li_brac_txt\">(");
                                            sb.Append(ConceptDescription);
                                            sb.Append(")</span> ");
                                        }
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        sbChild = new StringBuilder();
                        sb.Append(BindChildMetadatAttributes_For_Version_2_0(sbChild, SummaryStructure.MetadataStructureDefinitions[i].ReportStructure[0].MetadataAttribute[j], SummaryStructure, lngcodedb));
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                }
            }
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div>");
            sb.Append("<img id=\"imgdivMFD\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivMFD','divMFD')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Metadata_Flow_Definition\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpMFD\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\"  onclick=\"ToggleCallout('divCallout', event,  'divHelpMFD')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<br/>");
            SDMXObjectModel.Message.StructureType MFDStructure = new SDMXObjectModel.Message.StructureType();

            MFDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MFDPath);
            sb.Append("<div id=\"divMFD\" style=\"display:block\">");
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("<b>");
            sb.Append(GetLangSpecificValue_For_Version_2_1(MFDStructure.Structures.Metadataflows[0].Name, lngcodedb));
            sb.Append("</b>");
            if (MFDStructure.Structures.Metadataflows[0].Description.Count > 0)
            {
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValue_For_Version_2_1(MFDStructure.Structures.Metadataflows[0].Description, lngcodedb));
                sb.Append(")</span> ");
            }
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<a href=\" " + MFDViewPath + "\"  ");
            sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
            sb.Append("<a href='Download.aspx?fileId=" + MFDPath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
            sb.Append("</div>");
            sb.Append("</div>");

            ConceptSchemePath = Server.MapPath(System.IO.Path.Combine("~", @"stock\\data\\" + DBNId + "\\sdmx\\Concepts\\" + ConceptSchemeId + ".xml"));
            ConceptSchemeViewPath = "../../stock/data/" + DBNId + "/sdmx/Concepts/" + ConceptSchemeId + ".xml";

            SDMXApi_2_0.Message.StructureType ConceptSchemeStructure = new SDMXApi_2_0.Message.StructureType();
            ConceptSchemeStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), ConceptSchemePath);

            sb.Append("<div>");
            sb.Append("<img id=\"imgdivCS\" src=\"../../stock/themes/default/images/collapse.png\" alt=\"Expand and Collapse\" onclick=\"ExpandCollapseList('imgdivCS','divCS')\" style=\"margin-top: 10px;margin-right: 4px\" class=\"flt_lft\"/>");
            sb.Append("<h3 id=\"lang_Concept_Scheme_MSD\" class=\"flt_lft\"></h3>");
            sb.Append("&nbsp;<img id=\"imghelpConceptScheme\" src=\"../../stock/themes/default/images/help.gif\" alt=\"Help\"  onclick=\"ToggleCallout('divCallout', event,  'divHelpCS')\" style=\"margin-top:10px;cursor:pointer;\" onmouseout=\"HideCallout('divCallout')\";/>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<div id=\"divCS\" style=\"display:block\">");
            sb.Append("<div class=\"reg_li_sub_txt\">");
            sb.Append("<b>");
            sb.Append(GetLangSpecificValueFor_Version_2_0(ConceptSchemeStructure.Concepts.ConceptScheme[0].Name, lngcodedb));
            sb.Append("</b>");
            if (ConceptSchemeStructure.Concepts.ConceptScheme[0].Description.Count > 0)
            {
                sb.Append("<span class=\"reg_li_brac_txt\">(");
                sb.Append(GetLangSpecificValueFor_Version_2_0(ConceptSchemeStructure.Concepts.ConceptScheme[0].Description, lngcodedb));
                sb.Append(")</span> ");
            }
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append("<a href=\" " + ConceptSchemeViewPath + "\"  ");
            sb.Append(" target=\"_blank\" class=\"reg_li_link_txt\" name=\"lang_View\"></a> | ");
            sb.Append("<a href='Download.aspx?fileId=" + ConceptSchemePath + "' class=\"reg_li_link_txt\" name=\"lang_Download\"></a>");
            sb.Append("</div>");
            sb.Append("<br/>");
            sb.Append("<ul>");
            for (i = 0; i < ConceptSchemeStructure.Concepts.ConceptScheme[0].Concept.Count; i++)
            {
                sb.Append("<li>");

                sb.Append("<span>");
                sb.Append(GetLangSpecificValueFor_Version_2_0(ConceptSchemeStructure.Concepts.ConceptScheme[0].Concept[i].Name, lngcodedb));
                sb.Append("</span>");
                if (!string.IsNullOrEmpty(GetLangSpecificValueFor_Version_2_0(ConceptSchemeStructure.Concepts.ConceptScheme[0].Concept[i].Description, lngcodedb)))
                {
                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                    sb.Append(GetLangSpecificValueFor_Version_2_0(ConceptSchemeStructure.Concepts.ConceptScheme[0].Concept[i].Description, lngcodedb));
                    sb.Append(")</span>");
                }
                sb.Append("</li>");

            }
            sb.Append("</ul>");
            sb.Append("</div>");
            RetVal = sb.ToString();
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    private string BindChildMetadatAttributes_For_Version_2_1(StringBuilder sb, SDMXObjectModel.Structure.MetadataAttributeType ParentMetadataAttribute, SDMXObjectModel.Structure.StructuresType ConceptsObj, string lngcodedb)
    {
        string RetVal = string.Empty;
        string ConceptName = string.Empty;
        string ConceptDescription = string.Empty;
        SDMXObjectModel.Structure.MetadataAttributeType ChildMetadataAttribute;
        SDMXObjectModel.Common.ConceptReferenceType ConceptReference;
        SDMXObjectModel.Common.ConceptRefType ConceptRef;
        int i, j, k;
        if (ParentMetadataAttribute.MetadataAttribute != null && ParentMetadataAttribute.MetadataAttribute.Count > 0)
        {
            sb.Append("<ul>");
            for (i = 0; i < ParentMetadataAttribute.MetadataAttribute.Count; i++)
            {
                sb.Append("<li>");
                ChildMetadataAttribute = ParentMetadataAttribute.MetadataAttribute[i];
                ConceptReference = ChildMetadataAttribute.ConceptIdentity;
                ConceptRef = ((ConceptRefType)(ConceptReference.Items[0]));
                for (j = 0; j < ConceptsObj.Concepts.Count; j++)
                {
                    if (ConceptRef.maintainableParentID == ConceptsObj.Concepts[j].id)
                    {
                        for (k = 0; k < ConceptsObj.Concepts[j].Items.Count; k++)
                        {
                            if (ConceptRef.id.ToString() == ConceptsObj.Concepts[j].Items[k].id.ToString())
                            {
                                ConceptName = GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Name, lngcodedb).ToUpper();
                                ConceptDescription = GetLangSpecificValue_For_Version_2_1(ConceptsObj.Concepts[j].Items[k].Description, lngcodedb);
                                if (ConceptName != string.Empty)
                                {
                                    sb.Append(ConceptName);
                                }
                                if (ConceptDescription != string.Empty)
                                {
                                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                                    sb.Append(ConceptDescription);
                                    sb.Append(")</span> ");
                                }
                                break;
                            }
                        }
                    }
                }
                sb.Append("<div class=\"reg_li_sub_txt\">");
                sb.Append("<span id=\"lang_Presentational" + i + "\" ></span>");
                if (ChildMetadataAttribute.isPresentational == true)
                {
                    sb.Append("<span id=\"lang_Yes" + i + "\" ></span>");
                }
                else
                {
                    sb.Append("<span id=\"lang_No" + i + "\" ></span>");
                }
                sb.Append("</div>");
                if (ChildMetadataAttribute.MetadataAttribute != null && ChildMetadataAttribute.MetadataAttribute.Count > 0)
                {
                    BindChildMetadatAttributes_For_Version_2_1(sb, ChildMetadataAttribute, ConceptsObj, lngcodedb);
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
        }
        RetVal = sb.ToString();

        return RetVal;
    }

    private string BindChildMetadatAttributes_For_Version_2_0(StringBuilder sb, SDMXApi_2_0.Structure.MetadataAttributeType ParentMetadataAttribute, SDMXApi_2_0.Message.StructureType SummaryStructure, string lngcodedb)
    {
        string RetVal = string.Empty;
        string ConceptName = string.Empty;
        string ConceptDescription = string.Empty;
        SDMXApi_2_0.Structure.MetadataAttributeType ChildMetadataAttribute;

        int i, j, k;
        if (ParentMetadataAttribute.MetadataAttribute != null && ParentMetadataAttribute.MetadataAttribute.Count > 0)
        {
            sb.Append("<ul>");
            for (i = 0; i < ParentMetadataAttribute.MetadataAttribute.Count; i++)
            {
                sb.Append("<li>");
                ChildMetadataAttribute = ParentMetadataAttribute.MetadataAttribute[i];
                for (j = 0; j < SummaryStructure.Concepts.ConceptScheme.Count; j++)
                {
                    if (ChildMetadataAttribute.conceptSchemeRef == SummaryStructure.Concepts.ConceptScheme[j].id)
                    {
                        for (k = 0; k < SummaryStructure.Concepts.ConceptScheme[j].Concept.Count; k++)
                        {
                            if (ChildMetadataAttribute.conceptRef == SummaryStructure.Concepts.ConceptScheme[j].Concept[k].id)
                            {

                                ConceptName = GetLangSpecificValueFor_Version_2_0(SummaryStructure.Concepts.ConceptScheme[j].Concept[k].Name, lngcodedb).ToUpper();
                                ConceptDescription = GetLangSpecificValueFor_Version_2_0(SummaryStructure.Concepts.ConceptScheme[j].Concept[k].Description, lngcodedb);
                                if (ConceptName != string.Empty)
                                {
                                    sb.Append(ConceptName);
                                }
                                if (ConceptDescription != string.Empty)
                                {
                                    sb.Append("<span class=\"reg_li_brac_txt\">(");
                                    sb.Append(ConceptDescription);
                                    sb.Append(")</span> ");
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
                if (ChildMetadataAttribute.MetadataAttribute != null && ChildMetadataAttribute.MetadataAttribute.Count > 0)
                {
                    BindChildMetadatAttributes_For_Version_2_0(sb, ChildMetadataAttribute, SummaryStructure, lngcodedb);
                }
                sb.Append("</li>");
            }
            sb.Append("</ul>");
        }
        RetVal = sb.ToString();

        return RetVal;
    }

    private string GetLangSpecificValue_For_Version_2_1(List<SDMXObjectModel.Common.TextType> ListOfValues, string LangCode)
    {
        string Retval = string.Empty;
        if (ListOfValues != null && ListOfValues.Count > 0)
        {
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
        }
        return Retval;
    }

    #region "--AddSubscription--"

    private void Create_Subscription_Artefact(string Action, string NotificationMail, string NotificationHTTP, int IsSOAP, string StartDate, string EndDate, string EventSelector, string CategoriesGIDAndSchemeIds, string MFDId, string DbNId, string SubscriberIdAndType)
    {
        string InsertQuery, OutputFolder;
        DIConnection DIConnection;
        List<ArtefactInfo> Artefacts;


        string userId, UserNId;
        UserTypes userType;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        string subscriberAssignedId;
        DateTime startDate, endDate;
        Dictionary<string, string> dictCategories;
        string agencyId, CategoryGID, CategorySchemeId, HeaderFilePath;
        DevInfo.Lib.DI_LibSDMX.Header libHeader = new DevInfo.Lib.DI_LibSDMX.Header();

        //----------- Data to be passed to SDMXUtility-----------------------------------------------------------------


        UserNId = SubscriberIdAndType.Split('|')[0];
        if (SubscriberIdAndType.Split('|')[1] == "True")
        {
            userId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;
            userType = UserTypes.Provider;
        }
        else
        {
            userId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId;
            userType = UserTypes.Consumer;
        }

        isSOAPMailIds = new List<bool>();
        isSOAPMailIds.Add(Convert.ToBoolean(IsSOAP));

        notificationMailIds = new List<string>();
        notificationMailIds.Add(NotificationMail);

        isSOAPHTTPs = new List<bool>();
        isSOAPHTTPs.Add(Convert.ToBoolean(IsSOAP));

        notificationHTTPs = new List<string>();
        notificationHTTPs.Add(NotificationHTTP);

        subscriberAssignedId = Guid.NewGuid().ToString();

        startDate = DateTime.ParseExact(StartDate, "dd-MM-yyyy", null);
        endDate = DateTime.ParseExact(EndDate, "dd-MM-yyyy", null);

        dictCategories = new Dictionary<string, string>();
        if (EventSelector == "Data Registration")
        {
            foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
            {

                CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                dictCategories.Add(CategoryGID, CategorySchemeId);
            }

        }
        agencyId = Global.Get_AgencyId_From_DFD(DbNId);
        //------------------------------------------------------------------------------------------------------------
        InsertQuery = string.Empty;
        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Subscriptions\\" + UserNId);
        DIConnection = null;
        Artefacts = new List<ArtefactInfo>();
        HeaderFilePath = string.Empty;
        HeaderFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId.ToString() + "\\"+ Constants.FolderName.SDMX.sdmx+ DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName);
        //XmlDocument UploadedHeaderXml = new XmlDocument();
        //SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
        //SDMXObjectModel.Message.StructureHeaderType Header = new SDMXObjectModel.Message.StructureHeaderType();
        ////if (File.Exists(HeaderFilePath))
        //{
        //    UploadedHeaderXml.Load(HeaderFilePath);
        //    UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
        //    Header = UploadedDSDStructure.Header;
        //}

        //libHeader.ID = Header.ID;
        //libHeader.Name = Header.Name.ToString();
        //foreach (PartyType receiver in Header.Receiver)
        //{
        //    libHeader.Receiver.ID = receiver.id;
        //    libHeader.Receiver.Name = receiver.Name.ToString();
           
        //    //foreach (Contact contact in (Contact)receiver.Contact)
        //    //{
        //    //    libHeader.Receiver.Contact.Name = contact.Name;
        //    //    libHeader.Receiver.Contact.Department = contact.Department;
        //    //    libHeader.Receiver.Contact.Email = contact.Email;
        //    //    libHeader.Receiver.Contact.Fax = contact.Fax;
        //    //    libHeader.Receiver.Contact.Telephone = contact.Telephone;
        //    //    libHeader.Receiver.Contact.Role = contact.Role;
        //    //}
        //}
        //libHeader.Sender = Header.Sender;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            if (EventSelector == "Data Registration")
            {
                Artefacts = SDMXUtility.Subscribe_User(SDMXSchemaType.Two_One, Guid.NewGuid().ToString(), userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, dictCategories, string.Empty, agencyId,null, OutputFolder);

            }
            else if (EventSelector == "Metadata Registration")
            {
                Artefacts = SDMXUtility.Subscribe_User(SDMXSchemaType.Two_One, Guid.NewGuid().ToString(), userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, null, MFDId, agencyId, null, OutputFolder);
            }
            else if (EventSelector == "Structural Metadata Registration")
            {
                Artefacts = SDMXUtility.Subscribe_User(SDMXSchemaType.Two_One, Guid.NewGuid().ToString(), userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, null, null, agencyId, null, OutputFolder);
            }


            if (Artefacts.Count > 0)
            {
                InsertQuery = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                              " VALUES(" + DbNId + ",'" + Artefacts[0].Id + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + ",'" + Path.Combine(OutputFolder, Artefacts[0].FileName) + "');";

                DIConnection.ExecuteDataTable(InsertQuery);
            }

            Global.GetAppSetting();

            if (Global.registryNotifyViaEmail == "true")
            {
                //this.Frame_Message_And_Send_Subscription_Mail(UserNId, Artefacts[0].Id);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Create_Consumer_In_DCScheme_And_Update_Folder_Structures_Per_Database(string UserNId, string Language)
    {
        string ConsumerFileName, UserFolder, UserFullName;
        bool IsAlreadyExistingConsumer;

        IsAlreadyExistingConsumer = false;

        try
        {
            ConsumerFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
            UserFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
            UserFullName = Global.Get_User_Full_Name(UserNId);
            IsAlreadyExistingConsumer = this.Is_Already_Existing_Consumer(UserNId);

            if (IsAlreadyExistingConsumer == false)
            {
                if (File.Exists(ConsumerFileName))
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, ConsumerFileName, UserTypes.Consumer, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId, UserFullName, Language, string.Empty);
                }
                else
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserTypes.Consumer, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId, UserFullName, Language, UserFolder);
                }

                this.Create_Other_Artefacts_And_Update_Folder_Structures_For_Consumer_Per_Database(UserNId);
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

    private void Create_Other_Artefacts_And_Update_Folder_Structures_For_Consumer_Per_Database(string UserNId)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string OutputFolder;
        string Query;

        DtRegisteredDatabases = null;
        DIConnection = null;
        OutputFolder = string.Empty;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
            DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

            foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
            {
                #region "--Subscription--"

                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                this.Create_Directory_If_Not_Exists(OutputFolder);

                #endregion "--Subscription--"
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private bool Is_Already_Existing_Consumer(string UserNId)
    {
        bool RetVal;
        string ConsumerFileName;
        SDMXObjectModel.Message.StructureType Structure;


        RetVal = false;
        ConsumerFileName = string.Empty;
        Structure = null;

        try
        {
            ConsumerFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);

            if (File.Exists(ConsumerFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), ConsumerFileName);

                if (Structure != null && Structure.Structures != null && Structure.Structures.OrganisationSchemes != null &&
                    Structure.Structures.OrganisationSchemes.Count > 0 &&
                    Structure.Structures.OrganisationSchemes[0] is SDMXObjectModel.Structure.DataConsumerSchemeType &&
                    Structure.Structures.OrganisationSchemes[0].Organisation != null &&
                    Structure.Structures.OrganisationSchemes[0].Organisation.Count > 0)
                {
                    foreach (DataConsumerType DataConsumer in Structure.Structures.OrganisationSchemes[0].Organisation)
                    {
                        if (DataConsumer.id == DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId)
                        {
                            RetVal = true;
                            break;
                        }
                    }
                }

            }
            else
            {
                RetVal = false;
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

    #endregion "--AddSubscription--"

    #region "--UpdateSubscription--"

    private void Update_Subscription_Artefact(string Action, string NotificationMail, string NotificationHTTP, int IsSOAP, string StartDate, string EndDate, string EventSelector, string CategoriesGIDAndSchemeIds, string MFDId, string DbNId, string SubscriberIdAndType, string SubscriptionId)
    {
        string OutputFolder;
        List<ArtefactInfo> Artefacts;
        string userId, UserNId;
        UserTypes userType;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        string subscriberAssignedId;
        DateTime startDate, endDate;
        Dictionary<string, string> dictCategories;
        string agencyId, CategoryGID, CategorySchemeId;

        //----------- Data to be passed to SDMXUtility-----------------------------------------------------------------

        userId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + SubscriberIdAndType.Split('|')[0];
        UserNId = SubscriberIdAndType.Split('|')[0];
        if (SubscriberIdAndType.Split('|')[1] == "True")
        {
            userType = UserTypes.Provider;
        }
        else
        {
            userType = UserTypes.Consumer;
        }

        isSOAPMailIds = new List<bool>();
        isSOAPMailIds.Add(Convert.ToBoolean(IsSOAP));

        notificationMailIds = new List<string>();
        notificationMailIds.Add(NotificationMail);

        isSOAPHTTPs = new List<bool>();
        isSOAPHTTPs.Add(Convert.ToBoolean(IsSOAP));

        notificationHTTPs = new List<string>();
        notificationHTTPs.Add(NotificationHTTP);

        subscriberAssignedId = Guid.NewGuid().ToString();

        startDate = DateTime.ParseExact(StartDate, "dd-MM-yyyy", null);
        endDate = DateTime.ParseExact(EndDate, "dd-MM-yyyy", null);

        dictCategories = new Dictionary<string, string>();

        if (EventSelector == "Data Registration")
        {
            foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
            {

                CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                dictCategories.Add(CategoryGID, CategorySchemeId);
            }
        }

        agencyId = Global.Get_AgencyId_From_DFD(DbNId);
        //------------------------------------------------------------------------------------------------------------

        OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Subscriptions\\" + UserNId);

        try
        {

            if (EventSelector == "Data Registration")
            {
                Artefacts = SDMXUtility.Subscribe_User(SDMXSchemaType.Two_One, SubscriptionId, userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, dictCategories, string.Empty, agencyId, null, OutputFolder);
            }
            else if (EventSelector == "Metadata Registration")
            {
                Artefacts = SDMXUtility.Subscribe_User(SDMXSchemaType.Two_One, SubscriptionId, userId, userType, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, subscriberAssignedId, startDate, endDate, EventSelector, null, MFDId, agencyId, null, OutputFolder);
            }

            Global.GetAppSetting();

            if (Global.registryNotifyViaEmail == "true")
            {
                //this.Frame_Message_And_Send_Subscription_Mail(UserNId, SubscriptionId);
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

    #endregion "--UpdateSubscription--"

    #region "--DeleteSubscription--"

    private void Delete_Subscription_Artefact(string DbNId, string UserNId, string SubscriptionId)
    {
        string Query, FileNameWPath;
        DIConnection DIConnection;

        Query = string.Empty;
        FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbNId + "\\sdmx\\Subscriptions\\" + UserNId + "\\" + SubscriptionId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension);
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DIConnection.ExecuteDataTable("DELETE FROM Artefacts WHERE Id = '" + SubscriptionId + "' AND DBNId = " + DbNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.Subscription).ToString() + ";");

            if (File.Exists(FileNameWPath))
            {
                File.Delete(FileNameWPath);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    #endregion "--DeleteSubscription--"

    //private void Frame_Message_And_Send_Subscription_Mail(string UserNId, string SubscriptionURN)
    //{
    //    string MessageContent, FullName, EmailId;

    //    FullName = Global.Get_User_Full_Name(UserNId);
    //    EmailId = Global.Get_User_EmailId(UserNId);

    //    MessageContent = "Dear " + FullName + ",\n\nYou have successfully subscribed at " + Global.adaptation_name +
    //                             "(" + this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")) + ").";
    //    MessageContent += "\n\nRegistry URN: " + SubscriptionURN;

    //    MessageContent += "\n\nThank You.";
    //    MessageContent += "\n\nWith Best Regards,";
    //    MessageContent += "\n\nAdmin";
    //    MessageContent += "\n\n" + Global.adaptation_name;

    //    this.Send_Email(ConfigurationManager.AppSettings["NotificationSender"].ToString(), ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, "Subscription Details", MessageContent);
    //}

    private void Frame_Message_And_Send_Subscription_Mail(string UserNId, string SubscriptionURN, string Language)
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
            TamplatePath += Language + "\\" + Constants.FileName.SubscriptionSDMX;
            MessageContent = GetEmailTamplate(TamplatePath);
            Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
            Subject = Subject.Replace("[^^^^]", "");
            Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
            Body = Body.Replace("[****]USER_NAME[****]", FirstName);
            Body = Body.Replace("[****]REGISTRY_URN[****]", SubscriptionURN);
            Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
            Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
            //this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName, SubscriptionURN, "Subscription");
            this.Send_Email(Global.adaptation_name + " - WebMaster", ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, Subject, Body, true, FirstName, SubscriptionURN, "Subscription");
        }
    }

    private string Add_ChildCategory_To_Category(SDMXObjectModel.Structure.CategoryType Category, string CategorySchemeId, string SubscriptionSelectedCatGIDAndScheme)
    {
        StringBuilder sb = new StringBuilder();
        string chkboxvalue = string.Empty;
        foreach (SDMXObjectModel.Structure.CategoryType ChildCategory in Category.Items)
        {
            chkboxvalue = ChildCategory.id + "|" + CategorySchemeId;
            sb.Append("<ul>");


            if ((SubscriptionSelectedCatGIDAndScheme != "") && (CategoryAlreadySelected(chkboxvalue, SubscriptionSelectedCatGIDAndScheme)))
            {
                sb.Append("<li><input type=checkbox value=" + chkboxvalue + " checked=checked onclick=AddCategoryGId('" + chkboxvalue + "',event) /><label>" + ChildCategory.Name[0].Value + "</label>");
            }
            else
            {
                sb.Append("<li><input type=checkbox value=" + chkboxvalue + " onclick=AddCategoryGId('" + chkboxvalue + "',event) /><label>" + ChildCategory.Name[0].Value + "</label>");
            }

            sb.Append(this.Add_ChildCategory_To_Category(ChildCategory, CategorySchemeId, SubscriptionSelectedCatGIDAndScheme));
            sb.Append("</li></ul>");
        }
        return sb.ToString();
    }

    private Boolean CategoryAlreadySelected(string CategoryChkBoxValue, string SubscriptionSelectedCatGIDAndScheme)
    {
        Boolean retval;
        retval = false;


        foreach (string CategoryGIDAndSchemeId in Global.SplitString(SubscriptionSelectedCatGIDAndScheme, ","))
        {

            if (CategoryGIDAndSchemeId == CategoryChkBoxValue)
            {
                retval = true;
                break;
            }
            retval = false;
        }
        return retval;

    }

    private string GetLangSpecificValue(List<SDMXObjectModel.Common.TextType> ListOfValues, string LangCode)
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

    private string GetlanguageBasedValueOfKey(string key, string languageCode, string pageName)
    {
        string RetVal;
        string LanguageFolderPath;
        string LanguageFile;
        XmlFileReader objxmlFilereader;

        RetVal = string.Empty;
        objxmlFilereader = null;


        try
        {
            LanguageFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language);
            LanguageFile = Path.Combine(LanguageFolderPath, languageCode) + "\\" + pageName;
            objxmlFilereader = new XmlFileReader(LanguageFile);
            if (objxmlFilereader != null)
            {
                RetVal = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, key), Constants.Map.XMLValueAtributeName);

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

    # endregion "--Private functions--"

    #endregion
}
