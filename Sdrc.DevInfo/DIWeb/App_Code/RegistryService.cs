using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel;
using SDMXObjectModel.Message;
using SDMXObjectModel.Query;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Soap;
using System.Web.Mail;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using SDMXObjectModel.Structure;
using System.Runtime.Serialization.Formatters;


[WebService(Namespace = "http://www.devinfo.info/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class RegistryService : System.Web.Services.WebService
{
    #region "--Constructors--"

    #region "--Private--"

    private int DBNId;
    private DIConnection DIConnection;
    private DIQueries DIQueries;

    #endregion "--Private--"

    #region "--Public--"

    public RegistryService()
    {
        int DBNIdOut;

        if (this.Context.Request.QueryString["p"] != null && !string.IsNullOrEmpty(this.Context.Request.QueryString["p"]) &&
            int.TryParse(this.Context.Request.QueryString["p"], out DBNIdOut))
        {
            DBNId = DBNIdOut;
        }
        else
        {
            Global.GetAppSetting();
            DBNId = Convert.ToInt32(Global.GetDefaultDbNId());
        }

        if (Global.IsDSDUploadedFromAdmin(DBNId))
        {
            this.DIConnection = Global.GetDbConnection(this.Get_AssociatedDB_NId(DBNId.ToString()));
        }
        else
        {
            this.DIConnection = Global.GetDbConnection(DBNId);
        }

        this.DIQueries = new DIQueries(this.DIConnection.DIDataSetDefault(), this.DIConnection.DILanguageCodeDefault(this.DIConnection.DIDataSetDefault()));
    }

    #endregion "--Public--"

    #endregion "--Constructors--"

    #region "--Methods--"

    #region "--Private--"

    private int Get_AssociatedDB_NId(string DSDDBId)
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

    #region User is admin or not
    /// <summary>
    /// Is user admin
    /// </summary>
    /// <param name="usernid">user nid</param>
    /// <returns>true if user is admin else false</returns>    
    private bool isUserAdmin(string usernid)
    {
        bool isAdmin = false;
        string Query = string.Empty;
        DataTable dtUsers = null;
        DIConnection DIConnection = null;
        diworldwide_userinfo.UserLoginInformation Service;

        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
        {
            Service = new diworldwide_userinfo.UserLoginInformation();
            Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            isAdmin = Service.IsUserAdmin(usernid, Global.GetAdaptationGUID());
        }
        else
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "Select user_is_admin from Users where nid =" + usernid + ";";
            dtUsers = DIConnection.ExecuteDataTable(Query);
            if (dtUsers.Rows.Count > 0)
            {
                if (dtUsers.Rows[0][0].ToString().ToLower() == "true")
                {
                    isAdmin = true;
                }
            }
        }

        return isAdmin;
    }
    #endregion
    private void Retrieve_Id_AgencyId_Version(ref string Id, ref string AgencyId, ref string Version, MaintainableWhereType ArtefactWhere)
    {
        if (ArtefactWhere != null)
        {
            if (ArtefactWhere.ID != null)
            {
                Id = ArtefactWhere.ID.Value;
            }

            if (ArtefactWhere.AgencyID != null)
            {
                AgencyId = ArtefactWhere.AgencyID.Value;
            }

            Version = ArtefactWhere.Version;
        }
    }

    private XmlDocument Retrieve_Artefact(string Id, string AgencyId, string Version, string DbNId)
    {
        XmlDocument RetVal;
        string Query, FileNameWPath;
        DIConnection DIConnection;
        DataTable DtTable;

        RetVal = new XmlDocument();
        Query = string.Empty;
        FileNameWPath = string.Empty;
        DIConnection = null;
        DtTable = null;

        try
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Version))
            {
                if (Id != DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Id &&
                    Id != DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id &&
                    Id != DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Id)
                {
                    DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty,
                                                    string.Empty);

                    if ((AgencyId == DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.AgencyId) || (AgencyId == DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.AgencyId) || (AgencyId == DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId))
                    {
                        Query = "SELECT FileLocation FROM Artefacts WHERE Id = '" + Id;
                    }
                    else
                    {
                        Query = "SELECT FileLocation FROM Artefacts WHERE DBNId = " + DbNId + " AND Id = '" + Id;
                    }


                    if (!string.IsNullOrEmpty(AgencyId))
                    {
                        Query += "' AND AgencyId = '" + AgencyId;
                    }

                    if (!string.IsNullOrEmpty(Version))
                    {
                        Query += "' AND Version = '" + Version + "';";
                    }

                    DtTable = DIConnection.ExecuteDataTable(Query);

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileNameWPath = DtTable.Rows[0]["FileLocation"].ToString();
                        RetVal.Load(FileNameWPath);
                    }
                }
                else
                {
                    switch (Id)
                    {
                        case DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Id:
                            FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id:
                            FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
                            break;
                        case DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Id:
                            FileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
                            break;
                        default:
                            break;
                    }

                    RetVal.Load(FileNameWPath);
                }
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
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

    private RegistryInterfaceType Get_SubmitSubscriptions_Reponse(string SubscriberAssignedId, string RegistryURN, StatusType StatusType, string StatusMessage, string StatusLanguage)
    {
        RegistryInterfaceType RetVal;

        RetVal = null;

        try
        {
            RetVal = new RegistryInterfaceType();
            RetVal.Header = Global.Get_Appropriate_Header();
            RetVal.Footer = null;
            RetVal.Item = new SDMXObjectModel.Registry.SubmitSubscriptionsResponseType();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)RetVal.Item).SubscriptionStatus = new System.Collections.Generic.List<SubscriptionStatusType>();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)RetVal.Item).SubscriptionStatus.Add(new SubscriptionStatusType());
            ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)RetVal.Item).SubscriptionStatus[0].SubscriberAssignedID = SubscriberAssignedId;
            ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)RetVal.Item).SubscriptionStatus[0].SubscriptionURN = RegistryURN;
            ((SDMXObjectModel.Registry.SubmitSubscriptionsResponseType)RetVal.Item).SubscriptionStatus[0].StatusMessage = new SDMXObjectModel.Registry.StatusMessageType(StatusType, StatusMessage, StatusLanguage);
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

    private RegistryInterfaceType Get_QuerySubscriptions_Reponse(List<SubscriptionType> Subscriptions, StatusType StatusType, string StatusMessage, string StatusLanguage)
    {
        RegistryInterfaceType RetVal;

        RetVal = null;

        try
        {
            RetVal = new RegistryInterfaceType();
            RetVal.Header = Global.Get_Appropriate_Header();
            RetVal.Footer = null;
            RetVal.Item = new SDMXObjectModel.Registry.QuerySubscriptionResponseType();
            ((SDMXObjectModel.Registry.QuerySubscriptionResponseType)RetVal.Item).Subscription = Subscriptions;
            ((SDMXObjectModel.Registry.QuerySubscriptionResponseType)RetVal.Item).StatusMessage = new SDMXObjectModel.Registry.StatusMessageType(StatusType, StatusMessage, StatusLanguage);
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

    private RegistryInterfaceType Get_SubmitRegistrations_Reponse(RegistrationType Registration, StatusType StatusType, string StatusMessage, string StatusLanguage)
    {
        RegistryInterfaceType RetVal;

        RetVal = null;

        try
        {
            RetVal = new RegistryInterfaceType();
            RetVal.Header = Global.Get_Appropriate_Header();
            RetVal.Footer = null;
            RetVal.Item = new SDMXObjectModel.Registry.SubmitRegistrationsResponseType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RetVal.Item).RegistrationStatus = new List<RegistrationStatusType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RetVal.Item).RegistrationStatus.Add(new RegistrationStatusType());
            ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RetVal.Item).RegistrationStatus[0].Registration = Registration;
            ((SDMXObjectModel.Registry.SubmitRegistrationsResponseType)RetVal.Item).RegistrationStatus[0].StatusMessage = new SDMXObjectModel.Registry.StatusMessageType(StatusType, StatusMessage, StatusLanguage);
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

    private RegistryInterfaceType Get_QueryRegistrations_Reponse(List<RegistrationType> Registrations, StatusType StatusType, string StatusMessage, string StatusLanguage)
    {
        RegistryInterfaceType RetVal;
        QueryResultType QueryResult;

        RetVal = null;
        QueryResult = null;

        try
        {
            RetVal = new RegistryInterfaceType();
            RetVal.Header = Global.Get_Appropriate_Header();
            RetVal.Footer = null;
            RetVal.Item = new SDMXObjectModel.Registry.QueryRegistrationResponseType();
            ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RetVal.Item).QueryResult = new List<QueryResultType>();

            foreach (RegistrationType Registration in Registrations)
            {
                QueryResult = new QueryResultType();
                QueryResult.timeSeriesMatch = false;

                if (((ProvisionAgreementRefType)Registration.ProvisionAgreement.Items[0]).id.Contains(DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id))
                {
                    QueryResult.ItemElementName = QueryResultChoiceType.DataResult;
                }
                else
                {
                    QueryResult.ItemElementName = QueryResultChoiceType.MetadataResult;
                }

                QueryResult.Item = new ResultType();
                QueryResult.Item.Registration = Registration;

                ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RetVal.Item).QueryResult.Add(QueryResult);
            }

            ((SDMXObjectModel.Registry.QueryRegistrationResponseType)RetVal.Item).StatusMessage = new SDMXObjectModel.Registry.StatusMessageType(StatusType, StatusMessage, StatusLanguage);
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

    private void Handle_Exception(Exception ex)
    {
        if (!ex.Message.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message) &&
            !ex.Message.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message) &&
            !ex.Message.Contains(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message))
        {
            throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.ServerError.Message);
        }
        else
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    #endregion "--Private--"

    #region "--Public--"

    #region "--Not To Be Implemented--"

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetStructureSpecificMetadata([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument SubmitStructure([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetReportingTaxonomy([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetStructureSet([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetProcess([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetHierarchicalCodelist([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetDataSchema([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetMetadataSchema([XmlAnyElement] XmlDocument Input)
    {
        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NotImplemented.Message);
    }

    #endregion "--Not To Be Implemented--"

    #region "--To Be Implemented--"

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetDataflow([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.DataflowQueryType DataflowQuery;
        string Id, AgencyId, Version;

        DataflowQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                DataflowQuery = (SDMXObjectModel.Message.DataflowQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.DataflowQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (DataflowQuery != null && DataflowQuery.Query != null && DataflowQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, DataflowQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetMetadataflow([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.MetadataflowQueryType MetadataflowQuery;
        string Id, AgencyId, Version;

        MetadataflowQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                MetadataflowQuery = (SDMXObjectModel.Message.MetadataflowQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.MetadataflowQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (MetadataflowQuery != null && MetadataflowQuery.Query != null && MetadataflowQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, MetadataflowQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetDataStructure([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.DataStructureQueryType DataStructureQuery;
        string Id, AgencyId, Version;

        DataStructureQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                DataStructureQuery = (SDMXObjectModel.Message.DataStructureQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.DataStructureQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (DataStructureQuery != null && DataStructureQuery.Query != null && DataStructureQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, DataStructureQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetMetadataStructure([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.MetadataStructureQueryType MetadataStructureQuery;
        string Id, AgencyId, Version;

        MetadataStructureQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                MetadataStructureQuery = (SDMXObjectModel.Message.MetadataStructureQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.MetadataStructureQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (MetadataStructureQuery != null && MetadataStructureQuery.Query != null && MetadataStructureQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, MetadataStructureQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetCategoryScheme([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.CategorySchemeQueryType CategorySchemeQuery;
        string Id, AgencyId, Version;

        CategorySchemeQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                CategorySchemeQuery = (SDMXObjectModel.Message.CategorySchemeQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.CategorySchemeQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (CategorySchemeQuery != null && CategorySchemeQuery.Query != null && CategorySchemeQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, CategorySchemeQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetCategorisation([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.CategorisationQueryType CategorisationQuery;
        string Id, AgencyId, Version;

        CategorisationQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                CategorisationQuery = (SDMXObjectModel.Message.CategorisationQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.CategorisationQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (CategorisationQuery != null && CategorisationQuery.Query != null && CategorisationQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, CategorisationQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetConceptScheme([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.ConceptSchemeQueryType ConceptSchemeQuery;
        string Id, AgencyId, Version;

        ConceptSchemeQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                ConceptSchemeQuery = (SDMXObjectModel.Message.ConceptSchemeQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.ConceptSchemeQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (ConceptSchemeQuery != null && ConceptSchemeQuery.Query != null && ConceptSchemeQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, ConceptSchemeQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetCodelist([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.CodelistQueryType CodelistQuery;
        string Id, AgencyId, Version;

        CodelistQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                CodelistQuery = (SDMXObjectModel.Message.CodelistQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.CodelistQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (CodelistQuery != null && CodelistQuery.Query != null && CodelistQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, CodelistQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetOrganisationScheme([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.OrganisationSchemeQueryType OrganisationSchemeQuery;
        string Id, AgencyId, Version;

        OrganisationSchemeQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                OrganisationSchemeQuery = (SDMXObjectModel.Message.OrganisationSchemeQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.OrganisationSchemeQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (OrganisationSchemeQuery != null && OrganisationSchemeQuery.Query != null && OrganisationSchemeQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, OrganisationSchemeQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetProvisionAgreement([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.ProvisionAgreementQueryType ProvisionAgreementQuery;
        string Id, AgencyId, Version;

        ProvisionAgreementQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                ProvisionAgreementQuery = (SDMXObjectModel.Message.ProvisionAgreementQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.ProvisionAgreementQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (ProvisionAgreementQuery != null && ProvisionAgreementQuery.Query != null && ProvisionAgreementQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, ProvisionAgreementQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetConstraint([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.ConstraintQueryType ConstraintQuery;
        string Id, AgencyId, Version;

        ConstraintQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            try
            {
                ConstraintQuery = (SDMXObjectModel.Message.ConstraintQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.ConstraintQueryType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            if (ConstraintQuery != null && ConstraintQuery.Query != null && ConstraintQuery.Query.Item != null)
            {
                this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, ConstraintQuery.Query.Item);
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

            if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetStructures([XmlAnyElement] XmlDocument Input)
    {
        SDMXObjectModel.Message.StructuresQueryType StructuresQuery;
        string Id, AgencyId, Version;

        StructuresQuery = null;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        try
        {
            if (Input.ChildNodes.Count == 0 || (Input.ChildNodes.Count > 0 && Input.ChildNodes[0].ChildNodes.Count == 0))
            {
                Input = this.Retrieve_Artefact(DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.Id, string.Empty, DevInfo.Lib.DI_LibSDMX.Constants.Complete_XML.Version, DBNId.ToString());

                if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
                }
            }
            else
            {
                try
                {
                    StructuresQuery = (SDMXObjectModel.Message.StructuresQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructuresQueryType), Input);
                }
                catch (Exception)
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                if (StructuresQuery != null && StructuresQuery.Query != null && StructuresQuery.Query.Item != null)
                {
                    this.Retrieve_Id_AgencyId_Version(ref Id, ref AgencyId, ref Version, StructuresQuery.Query.Item);
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                Input = this.Retrieve_Artefact(Id, AgencyId, Version, DBNId.ToString());

                if (Input == null || string.IsNullOrEmpty(Input.InnerXml))
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument SubmitSubscription([XmlAnyElement] XmlDocument Input, string langPrefNid)
    {
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        SubscriptionRequestType SubscriptionRequest;
        ActionType Action;
        DIConnection DIConnection;
        DataTable DtTable;
        string UserId, UserNId;
        string MaxNId, RegistryURN, SubscriberAssignedId;
        string FileNameWPath, Query;

        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        SubscriptionRequest = null;
        DIConnection = null;
        DtTable = null;
        UserId = string.Empty;
        UserNId = string.Empty;
        MaxNId = string.Empty;
        RegistryURN = string.Empty;
        SubscriberAssignedId = string.Empty;
        FileNameWPath = string.Empty;
        Query = string.Empty;

        try
        {
            try
            {
                RegistryInterfaceRequest = (RegistryInterfaceType)Deserializer.LoadFromXmlDocument(typeof(RegistryInterfaceType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            if (RegistryInterfaceRequest.Item != null)
            {
                if (((SubmitSubscriptionsRequestType)RegistryInterfaceRequest.Item).SubscriptionRequest != null && ((SubmitSubscriptionsRequestType)RegistryInterfaceRequest.Item).SubscriptionRequest.Count > 0)
                {
                    SubscriptionRequest = ((SubmitSubscriptionsRequestType)RegistryInterfaceRequest.Item).SubscriptionRequest[0];

                    if (SubscriptionRequest.Subscription != null)
                    {
                        if (SubscriptionRequest.Subscription.Organisation != null && SubscriptionRequest.Subscription.Organisation.Items != null && SubscriptionRequest.Subscription.Organisation.Items.Count > 0)
                        {
                            if (SubscriptionRequest.Subscription.Organisation.Items[0] is DataProviderRefType)
                            {
                                UserId = ((DataProviderRefType)SubscriptionRequest.Subscription.Organisation.Items[0]).id;
                            }
                            else
                            {
                                UserId = ((DataConsumerRefType)SubscriptionRequest.Subscription.Organisation.Items[0]).id;
                            }
                            UserNId = UserId.Split('_')[1];

                            Action = SubscriptionRequest.action;
                            SubscriberAssignedId = SubscriptionRequest.Subscription.SubscriberAssignedID;
                            RegistryInterfaceRequest.Footer = null;
                            if (!string.IsNullOrEmpty(SubscriberAssignedId))
                            {
                                switch (Action)
                                {
                                    case ActionType.Append:
                                        if (UserId.Contains(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix))
                                        {
                                            Global.Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                        }
                                        else
                                        {
                                            Global.Create_Consumer_In_DCScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                        }

                                        Query = "SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;";
                                        DtTable = DIConnection.ExecuteDataTable(Query);

                                        MaxNId = (Convert.ToInt32(DtTable.Rows[0]["MaxNId"].ToString()) + 1).ToString();
                                        RegistryURN = Guid.NewGuid().ToString();
                                        SubscriptionRequest.Subscription.RegistryURN = RegistryURN;
                                        FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Subscriptions/" + UserNId + "/" + RegistryURN + ".xml");
                                        Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                        // validate Artefact schema for langPrefNid
                                        Global.BaselineAccessDbSchema();

                                        Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation, LangPrefNid)" +
                                                " VALUES(" + DBNId.ToString() + ",'" + RegistryURN + "','" + string.Empty + "','" +
                                                string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Subscription).ToString() +
                                                ",'" + FileNameWPath + "', '" + Int32.Parse(langPrefNid) + "');";
                                        DIConnection.ExecuteDataTable(Query);


                                        RegistryInterfaceResponse = this.Get_SubmitSubscriptions_Reponse(SubscriberAssignedId, RegistryURN, StatusType.Success, string.Empty, string.Empty);
                                        Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                        break;

                                    case ActionType.Delete:
                                        RegistryURN = SubscriptionRequest.Subscription.RegistryURN;
                                        FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Subscriptions/" + UserNId + "/" + RegistryURN + ".xml");
                                        File.Delete(FileNameWPath);

                                        Query = "DELETE FROM Artefacts WHERE Id = '" + RegistryURN + "' AND Type = " + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + ";";
                                        DIConnection.ExecuteDataTable(Query);

                                        RegistryInterfaceResponse = this.Get_SubmitSubscriptions_Reponse(SubscriberAssignedId, RegistryURN, StatusType.Success, string.Empty, string.Empty);
                                        Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                        break;

                                    case ActionType.Replace:
                                        RegistryURN = SubscriptionRequest.Subscription.RegistryURN;
                                        FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Subscriptions/" + UserNId + "/" + RegistryURN + ".xml");
                                        File.Delete(FileNameWPath);
                                        Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                        Query = "DELETE FROM Artefacts WHERE Id = '" + RegistryURN + "' AND Type = " + Convert.ToInt32(ArtefactTypes.Subscription).ToString() + ";";
                                        DIConnection.ExecuteDataTable(Query);

                                        // validate Artefact schema for langPrefNid
                                        Global.BaselineAccessDbSchema();

                                        Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation, LangPrefNid)" +
                                                " VALUES(" + DBNId.ToString() + ",'" + RegistryURN + "','" + string.Empty + "','" +
                                                string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Subscription).ToString() +
                                                ",'" + FileNameWPath + "', '" + Int32.Parse(langPrefNid) + "');";
                                        DIConnection.ExecuteDataTable(Query);

                                        RegistryInterfaceResponse = this.Get_SubmitSubscriptions_Reponse(SubscriberAssignedId, RegistryURN, StatusType.Success, string.Empty, string.Empty);
                                        Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                            }
                        }
                        else
                        {
                            throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                        }
                    }
                    else
                    {
                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument QuerySubscription([XmlAnyElement] XmlDocument Input)
    {
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse, RegistryInterfaceSubscription;
        List<SubscriptionType> Subscriptions;
        Dictionary<string, string> subscriptionPrefLangDict;
        string UserId, UserNId;
        string FolderName;
        string registryUrn;
        bool IsAdminUser;
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        RegistryInterfaceSubscription = null;
        Subscriptions = null;
        UserId = string.Empty;
        UserNId = string.Empty;
        FolderName = string.Empty;
        registryUrn = string.Empty;
        subscriptionPrefLangDict = new Dictionary<string, string>();
        IsAdminUser = false;
        DirectoryInfo DirSubscription;
        FileInfo[] FilesSubscription;
        try
        {
            try
            {
                RegistryInterfaceRequest = (RegistryInterfaceType)Deserializer.LoadFromXmlDocument(typeof(RegistryInterfaceType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            FolderName = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Subscriptions/");

            if (RegistryInterfaceRequest.Item != null)
            {
                if (((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation != null &&
                    ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation.Items != null &&
                    ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation.Items.Count > 0)
                {
                    if (((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation.Items[0] is DataProviderRefType)
                    {
                        UserId = ((DataProviderRefType)((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation.Items[0]).id;
                    }
                    else
                    {
                        UserId = ((DataConsumerRefType)((SDMXObjectModel.Registry.QuerySubscriptionRequestType)RegistryInterfaceRequest.Item).Organisation.Items[0]).id;
                    }



                    if (!string.IsNullOrEmpty(UserId) && UserId.Split('_').Length == 2)
                    {

                        UserNId = UserId.Split('_')[1];
                        IsAdminUser = this.isUserAdmin(UserNId);
                        if (IsAdminUser)
                        {
                            DirSubscription = new DirectoryInfo(Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Subscriptions/"));
                            Subscriptions = new List<SubscriptionType>();
                            foreach (DirectoryInfo dirSubs in DirSubscription.GetDirectories())
                            {
                                FilesSubscription = null;
                                FilesSubscription = dirSubs.GetFiles();
                                foreach (FileInfo subsFile in FilesSubscription)
                                {
                                    RegistryInterfaceSubscription = (RegistryInterfaceType)Deserializer.LoadFromFile(typeof(RegistryInterfaceType), subsFile.DirectoryName.ToString() + "\\" + subsFile.ToString());

                                    if (RegistryInterfaceSubscription != null && RegistryInterfaceSubscription.Item != null && ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest.Count > 0)
                                    {
                                        Subscriptions.Add(((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription);
                                    }


                                    RegistryInterfaceResponse = this.Get_QuerySubscriptions_Reponse(Subscriptions, StatusType.Success, string.Empty, string.Empty);
                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                }

                            }
                        }
                        else
                        {
                            FolderName += UserNId + "/";
                            Subscriptions = new List<SubscriptionType>();

                            if (Directory.Exists(FolderName))
                            {
                                foreach (string File in Directory.GetFiles(FolderName))
                                {
                                    RegistryInterfaceSubscription = (RegistryInterfaceType)Deserializer.LoadFromFile(typeof(RegistryInterfaceType), File);

                                    if (RegistryInterfaceSubscription != null && RegistryInterfaceSubscription.Item != null && ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest.Count > 0)
                                    {
                                        Subscriptions.Add(((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription);
                                    }
                                }

                                RegistryInterfaceResponse = this.Get_QuerySubscriptions_Reponse(Subscriptions, StatusType.Success, string.Empty, string.Empty);
                                Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                            }
                            else
                            {
                                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
                            }
                        }
                        //FolderName += UserNId + "/";
                        //Subscriptions = new List<SubscriptionType>();

                        //if (Directory.Exists(FolderName))
                        //{
                        //    foreach (string File in Directory.GetFiles(FolderName))
                        //    {
                        //        RegistryInterfaceSubscription = (RegistryInterfaceType)Deserializer.LoadFromFile(typeof(RegistryInterfaceType), File);

                        //        if (RegistryInterfaceSubscription != null && RegistryInterfaceSubscription.Item != null && ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest.Count > 0)
                        //        {
                        //            Subscriptions.Add(((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription);
                        //        }
                        //    }

                        //    RegistryInterfaceResponse = this.Get_QuerySubscriptions_Reponse(Subscriptions, StatusType.Success, string.Empty, string.Empty);
                        //    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                        //}
                        //else
                        //{
                        //    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
                        //}
                    }
                    else
                    {
                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            if (!(ex.Message == "No results found."))
            {
                this.Handle_Exception(ex);
            }
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument SubmitRegistration([XmlAnyElement] XmlDocument Input, string SDMXFileName, string langPrefNid)
    {
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse;
        RegistrationRequestType RegistrationRequest;
        ActionType Action;
        DIConnection DIConnection;
        DataTable DtTable;
        string PAId, UserNId, DFDMFDId, AgencyId;
        string MaxNId, RegistrationId;
        string FileNameWPath, Query;
        string DataMetadataFile, CompleteFile;
        string ErrorMessage;
        XmlDocument DataDocument;
        List<ArtefactInfo> Artefacts;

        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        RegistrationRequest = null;
        DIConnection = null;
        DtTable = null;
        PAId = string.Empty;
        UserNId = string.Empty;
        DFDMFDId = string.Empty;
        AgencyId = string.Empty;
        MaxNId = string.Empty;
        RegistrationId = string.Empty;
        FileNameWPath = string.Empty;
        Query = string.Empty;
        DataMetadataFile = string.Empty;
        CompleteFile = string.Empty;
        ErrorMessage = string.Empty;
        DataDocument = new XmlDocument();
        Artefacts = null;

        try
        {
            try
            {
                RegistryInterfaceRequest = (RegistryInterfaceType)Deserializer.LoadFromXmlDocument(typeof(RegistryInterfaceType), Input);
                RegistryInterfaceRequest.Footer = null;
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            AgencyId = Global.Get_AgencyId_From_DFD(DBNId.ToString());

            if (!Global.IsDSDUploadedFromAdmin(DBNId))
            {
                #region "--Schema 2.1--"

                if (RegistryInterfaceRequest.Item != null)
                {
                    if (((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest != null && ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest.Count > 0)
                    {
                        RegistrationRequest = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0];

                        if (RegistrationRequest.Registration != null)
                        {
                            if (RegistrationRequest.Registration.ProvisionAgreement != null && RegistrationRequest.Registration.ProvisionAgreement.Items != null && RegistrationRequest.Registration.ProvisionAgreement.Items.Count > 0)
                            {
                                PAId = ((ProvisionAgreementRefType)RegistrationRequest.Registration.ProvisionAgreement.Items[0]).id;
                                UserNId = PAId.Split('_')[1];
                                DFDMFDId = PAId.Replace(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_", "");

                                if (DFDMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                                {
                                    #region "--Data--"

                                    if (RegistrationRequest.Registration.Datasource != null)
                                    {
                                        CompleteFile = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Complete.xml");
                                        Action = RegistrationRequest.action;

                                        switch (Action)
                                        {
                                            case ActionType.Append:
                                                Global.Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);

                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, false, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Query = "SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;";
                                                    DtTable = DIConnection.ExecuteDataTable(Query);

                                                    MaxNId = (Convert.ToInt32(DtTable.Rows[0]["MaxNId"].ToString()) + 1).ToString();
                                                    RegistrationId = Guid.NewGuid().ToString();
                                                    RegistrationRequest.Registration.id = RegistrationId;

                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);
                                                    Global.ExistenceofColumnAccessDbSchema();
                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation,PublishedFileName,LangPrefNid)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "','" + SDMXFileName + "','" + Int32.Parse(langPrefNid) + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    if (!string.IsNullOrEmpty(DataMetadataFile))
                                                    {
                                                        DataDocument = new XmlDocument();
                                                        DataDocument.Load(DataMetadataFile);

                                                        Artefacts = SDMXUtility.Create_Constraint(SDMXSchemaType.Two_One, DataDocument, RegistrationId, DataMetadataFile, AgencyId, null, Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Constraints/" + UserNId));

                                                        if (Artefacts.Count > 0)
                                                        {
                                                            Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                                                    " VALUES(" + DBNId.ToString() + ",'" + Artefacts[0].Id + "','" +
                                                                    Artefacts[0].AgencyId + "','" + Artefacts[0].Version + "','" + string.Empty + "'," +
                                                                    Convert.ToInt32(ArtefactTypes.Constraint).ToString() + ",'" + Path.Combine(
                                                                    Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Constraints/" + UserNId),
                                                                    Artefacts[0].FileName) + "');";

                                                            DIConnection.ExecuteDataTable(Query);
                                                        }
                                                    }

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, false);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            case ActionType.Delete:
                                                Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id, false, "true");
                                                Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                Global.Delete_Constraint_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);

                                                RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                break;
                                            case ActionType.Replace:
                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, false, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    Global.Delete_Constraint_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    RegistrationId = RegistrationRequest.Registration.id;
                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    if (!string.IsNullOrEmpty(DataMetadataFile))
                                                    {
                                                        DataDocument = new XmlDocument();
                                                        DataDocument.Load(DataMetadataFile);

                                                        Artefacts = SDMXUtility.Create_Constraint(SDMXSchemaType.Two_One, DataDocument, RegistrationId, DataMetadataFile, AgencyId, null, Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Constraints/" + UserNId));

                                                        if (Artefacts.Count > 0)
                                                        {
                                                            Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                                                    " VALUES(" + DBNId.ToString() + ",'" + Artefacts[0].Id + "','" +
                                                                    Artefacts[0].AgencyId + "','" + Artefacts[0].Version + "','" + string.Empty + "'," +
                                                                    Convert.ToInt32(ArtefactTypes.Constraint).ToString() + ",'" + Path.Combine(
                                                                    Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Constraints/" + UserNId),
                                                                    Artefacts[0].FileName) + "');";

                                                            DIConnection.ExecuteDataTable(Query);
                                                        }
                                                    }

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, false);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                                    }

                                    #endregion "--Data--"
                                }
                                else
                                {
                                    #region "--Metadata--"

                                    if (RegistrationRequest.Registration.Datasource != null)
                                    {
                                        CompleteFile = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Complete.xml");
                                        Action = RegistrationRequest.action;

                                        switch (Action)
                                        {
                                            case ActionType.Append:
                                                Global.Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);

                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, true, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Query = "SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;";
                                                    DtTable = DIConnection.ExecuteDataTable(Query);

                                                    MaxNId = (Convert.ToInt32(DtTable.Rows[0]["MaxNId"].ToString()) + 1).ToString();
                                                    RegistrationId = Guid.NewGuid().ToString();
                                                    RegistrationRequest.Registration.id = RegistrationId;

                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Global.ExistenceofColumnAccessDbSchema();
                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation,PublishedFileName,LangPrefNid)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "','" + SDMXFileName + "','" + Int32.Parse(langPrefNid) + "');";



                                                    DIConnection.ExecuteDataTable(Query);

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, true);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            case ActionType.Delete:
                                                Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id, false, "true");
                                                Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);

                                                RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                break;
                                            case ActionType.Replace:
                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, true, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    RegistrationId = RegistrationRequest.Registration.id;
                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, true);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                                    }

                                    #endregion "--Metadata--"
                                }
                            }
                            else
                            {
                                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                            }
                        }
                        else
                        {
                            throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                        }
                    }
                    else
                    {
                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                #endregion "--Schema 2.1--"
            }
            else
            {
                #region "--Schema 2.0--"

                if (RegistryInterfaceRequest.Item != null)
                {
                    if (((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest != null && ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest.Count > 0)
                    {
                        RegistrationRequest = ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRequest.Item).RegistrationRequest[0];

                        if (RegistrationRequest.Registration != null)
                        {
                            if (RegistrationRequest.Registration.ProvisionAgreement != null && RegistrationRequest.Registration.ProvisionAgreement.Items != null && RegistrationRequest.Registration.ProvisionAgreement.Items.Count > 0)
                            {
                                PAId = ((ProvisionAgreementRefType)RegistrationRequest.Registration.ProvisionAgreement.Items[0]).id;
                                UserNId = PAId.Split('_')[1];
                                DFDMFDId = PAId.Replace(DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + "_", "");

                                if (DFDMFDId == DevInfo.Lib.DI_LibSDMX.Constants.DFD.Id)
                                {
                                    #region "--Data--"

                                    if (RegistrationRequest.Registration.Datasource != null)
                                    {
                                        CompleteFile = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Complete.xml");
                                        Action = RegistrationRequest.action;

                                        switch (Action)
                                        {
                                            case ActionType.Append:
                                                Global.Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);

                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, false, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Query = "SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;";
                                                    DtTable = DIConnection.ExecuteDataTable(Query);

                                                    MaxNId = (Convert.ToInt32(DtTable.Rows[0]["MaxNId"].ToString()) + 1).ToString();
                                                    RegistrationId = Guid.NewGuid().ToString();
                                                    RegistrationRequest.Registration.id = RegistrationId;

                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Global.ExistenceofColumnAccessDbSchema();

                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation,PublishedFileName,LangPrefNid)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "','" + SDMXFileName + "','" + Int32.Parse(langPrefNid) + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    if (!string.IsNullOrEmpty(DataMetadataFile))
                                                    {
                                                        Global.Create_Constraint_Artefact_For_Version_2_0_SDMLMLFile(RegistrationId, DBNId.ToString(), UserNId, AgencyId, DataMetadataFile);
                                                    }

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, false);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            case ActionType.Delete:

                                                Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id, false, "true");
                                                Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                Global.Delete_Constraint_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);

                                                RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                break;
                                            case ActionType.Replace:
                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, false, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    Global.Delete_Constraint_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    RegistrationId = RegistrationRequest.Registration.id;
                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    if (!string.IsNullOrEmpty(DataMetadataFile))
                                                    {
                                                        Global.Create_Constraint_Artefact_For_Version_2_0_SDMLMLFile(RegistrationId, DBNId.ToString(), UserNId, AgencyId, DataMetadataFile);
                                                    }

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, false);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                                    }

                                    #endregion "--Data--"
                                }
                                else
                                {
                                    #region "--Metadata--"

                                    if (RegistrationRequest.Registration.Datasource != null)
                                    {
                                        CompleteFile = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Complete.xml");
                                        Action = RegistrationRequest.action;

                                        switch (Action)
                                        {
                                            case ActionType.Append:
                                                Global.Create_Provider_In_DPScheme_And_Update_Folder_Structures_Per_Database(UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);

                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, true, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Query = "SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;";
                                                    DtTable = DIConnection.ExecuteDataTable(Query);

                                                    MaxNId = (Convert.ToInt32(DtTable.Rows[0]["MaxNId"].ToString()) + 1).ToString();
                                                    RegistrationId = Guid.NewGuid().ToString();
                                                    RegistrationRequest.Registration.id = RegistrationId;

                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);
                                                    Global.ExistenceofColumnAccessDbSchema();
                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation,PublishedFileName,LangPrefNid)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "','" + SDMXFileName + "','" + Int32.Parse(langPrefNid) + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, true);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            case ActionType.Delete:
                                                Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id, true, "true");
                                                Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);

                                                RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                break;
                                            case ActionType.Replace:
                                                if (Global.Validate_DataSource(RegistrationRequest.Registration.Datasource, CompleteFile, DBNId, true, DFDMFDId, out ErrorMessage, out DataMetadataFile))
                                                {
                                                    Global.Delete_Registration_Artefact(DBNId.ToString(), UserNId, RegistrationRequest.Registration.id);
                                                    RegistrationId = RegistrationRequest.Registration.id;
                                                    FileNameWPath = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/" + UserNId + "/" + RegistrationId + ".xml");
                                                    Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceRequest, FileNameWPath);

                                                    Query = "INSERT INTO Artefacts (DBNId, Id, AgencyId, Version, URN, Type, FileLocation)" +
                                              " VALUES(" + DBNId.ToString() + ",'" + RegistrationId + "','" + string.Empty + "','" + string.Empty + "','" + string.Empty + "'," + Convert.ToInt32(ArtefactTypes.Registration).ToString() + ",'" + FileNameWPath + "');";

                                                    DIConnection.ExecuteDataTable(Query);

                                                    Global.Send_Notifications_For_Subscriptions(DBNId.ToString(), UserNId, RegistrationId, true);

                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(RegistrationRequest.Registration, StatusType.Success, string.Empty, string.Empty);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                else
                                                {
                                                    RegistryInterfaceResponse = this.Get_SubmitRegistrations_Reponse(null, StatusType.Failure, ErrorMessage, DevInfo.Lib.DI_LibSDMX.Constants.DefaultLanguage);
                                                    Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                                                }
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                                    }

                                    #endregion "--Metadata--"
                                }
                            }
                            else
                            {
                                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                            }
                        }
                        else
                        {
                            throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                        }
                    }
                    else
                    {
                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                #endregion "--Schema 2.0--"
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument QueryRegistration([XmlAnyElement] XmlDocument Input, string languageNid)
    {
        RegistryInterfaceType RegistryInterfaceRequest, RegistryInterfaceResponse, RegistryInterfaceRegistration;
        List<RegistrationType> Registrations;
        List<string> SortedFileNames;
        DIConnection DIConnection;
        DataTable DtTable;
        string UserId, UserNId;
        string FolderName;
        ArrayList AdFiles = new ArrayList();
        RegistryInterfaceRequest = null;
        RegistryInterfaceResponse = null;
        RegistryInterfaceRegistration = null;
        Registrations = null;
        SortedFileNames = new List<string>();
        UserId = string.Empty;
        UserNId = string.Empty;
        FolderName = string.Empty;

        try
        {
            try
            {
                RegistryInterfaceRequest = (RegistryInterfaceType)Deserializer.LoadFromXmlDocument(typeof(RegistryInterfaceType), Input);
            }
            catch (Exception)
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }

            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            FolderName = Path.Combine(Server.MapPath("~"), "stock/data/" + DBNId.ToString() + "/sdmx/Registrations/");



            DtTable = new DataTable();
            DtTable = DIConnection.ExecuteDataTable("SELECT * FROM Artefacts WHERE DBNId = " + DBNId.ToString() + " AND LangPrefNid=" + Int32.Parse(languageNid) + " AND Type = " + Convert.ToInt32(ArtefactTypes.Registration).ToString() + "  ORDER BY PublishedFileName   ;");
            if (DtTable != null && DtTable.Rows.Count > 0)
            {
                foreach (DataRow dr in DtTable.Rows)
                {
                    AdFiles.Add(Convert.ToString(dr["FileLocation"].ToString().Replace("<div>", "").Replace("</div>", "")));
                }
            }

            if (RegistryInterfaceRequest.Item != null)
            {
                if (((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item != null &&
                    ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items != null &&
                    ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items.Count > 0)
                {
                    UserId = ((DataProviderRefType)((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)RegistryInterfaceRequest.Item).Item).Items[0]).id;

                    if (!string.IsNullOrEmpty(UserId) && UserId.Split('_').Length == 2)
                    {
                        UserNId = UserId.Split('_')[1];
                        FolderName += UserNId + "/";
                        Registrations = new List<RegistrationType>();

                        if (Directory.Exists(FolderName))
                        {
                            SortedFileNames = new List<string>(Directory.GetFiles(FolderName));
                            SortedFileNames.Sort();


                            foreach (string File in AdFiles)
                            {

                                if (SortedFileNames.Contains(File))
                                {
                                    RegistryInterfaceRegistration = (RegistryInterfaceType)Deserializer.LoadFromFile(typeof(RegistryInterfaceType), File);

                                    if (RegistryInterfaceRegistration != null && RegistryInterfaceRegistration.Item != null && ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRegistration.Item).RegistrationRequest.Count > 0)
                                    {
                                        Registrations.Add(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RegistryInterfaceRegistration.Item).RegistrationRequest[0].Registration);
                                    }
                                }

                            }


                            RegistryInterfaceResponse = this.Get_QueryRegistrations_Reponse(Registrations, StatusType.Success, string.Empty, string.Empty);
                            Input = Serializer.SerializeToXmlDocument(typeof(RegistryInterfaceType), RegistryInterfaceResponse);
                        }
                        else
                        {
                            throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.NoResults.Message);
                        }
                    }
                    else
                    {
                        throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }
            }
            else
            {
                throw new Exception(DevInfo.Lib.DI_LibSDMX.Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }
        finally
        {
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetGenericData([XmlAnyElement] XmlDocument Input)
    {
        try
        {
            Input = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, Input, DevInfo.Lib.DI_LibSDMX.DataFormats.Generic, this.DIConnection, this.DIQueries);
        }
        catch (Exception ex)
        {
            this.Handle_Exception(ex);
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetGenericTimeSeriesData([XmlAnyElement] XmlDocument Input)
    {
        try
        {
            Input = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, Input, DevInfo.Lib.DI_LibSDMX.DataFormats.GenericTS, this.DIConnection, this.DIQueries);
        }
        catch (Exception ex)
        {
            this.Handle_Exception(ex);
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetStructureSpecificData([XmlAnyElement] XmlDocument Input)
    {
        try
        {
            Input = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, Input, DevInfo.Lib.DI_LibSDMX.DataFormats.StructureSpecific, this.DIConnection, this.DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetStructureSpecificTimeSeriesData([XmlAnyElement] XmlDocument Input)
    {
        try
        {
            Input = SDMXUtility.Get_Data(SDMXSchemaType.Two_One, Input, DevInfo.Lib.DI_LibSDMX.DataFormats.StructureSpecificTS, this.DIConnection, this.DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }

        return Input;
    }

    [WebMethod]
    [RegistryExtensionAttribute]
    [return: XmlAnyElement]
    public XmlDocument GetGenericMetadata([XmlAnyElement] XmlDocument Input)
    {
        try
        {
            Input = SDMXUtility.Get_MetadataReport(SDMXSchemaType.Two_One, Input, string.Empty, null, this.DIConnection, this.DIQueries);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            this.Handle_Exception(ex);
        }

        return Input;
    }

    #endregion "--To Be Implemented--"

    #endregion "--Public--"

    #endregion "--Methods--"
}

