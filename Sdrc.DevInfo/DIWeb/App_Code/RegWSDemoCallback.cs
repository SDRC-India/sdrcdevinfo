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
using System.Text;
using DevInfo.Lib.DI_LibDATA;
using System.Web.Services.Protocols;
using SDMXObjectModel.Query;

public partial class Callback : System.Web.UI.Page
{
    
    #region "--Methods--"

    #region "--Private--"

    private XmlDocument GetQueryXmlDocumentOnTypeBasis(int Type, string Id, string AgencyId, string Version, string UserIdAndType)
    {
        XmlDocument RetVal;
        UserTypes UserType;
        string UserID;
        string parentAgencyID;
        string maintainableParentID;
        string maintainableParentVersion;

        parentAgencyID=string.Empty;
        UserID=string.Empty;
        maintainableParentID=string.Empty;
        maintainableParentVersion=string.Empty;
        RetVal = null;

        try
        {
            switch (Type)
            {
                case 0:
                    SDMXObjectModel.Message.DataflowQueryType Dataflow = new SDMXObjectModel.Message.DataflowQueryType();
                    Dataflow.Header = Global.Get_Appropriate_Header();
                    Dataflow.Query = new SDMXObjectModel.Query.DataflowQueryType();

                    Dataflow.Query.ReturnDetails = new StructureReturnDetailsType();
                    Dataflow.Query.ReturnDetails.References = new ReferencesType();
                    Dataflow.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Dataflow.Query.ReturnDetails.References.Item = new EmptyType();

                    Dataflow.Query.Item = new DataflowWhereType();
                    Dataflow.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Dataflow;
                    Dataflow.Query.Item.typeSpecified = true;

                    Dataflow.Query.Item.ID = new QueryIDType();
                    Dataflow.Query.Item.ID.Value = Id;

                    Dataflow.Query.Item.AgencyID = new QueryNestedIDType();
                    Dataflow.Query.Item.AgencyID.Value = AgencyId;

                    Dataflow.Query.Item.Version = Version;

                    Dataflow.Query.Item.Annotation = null;
                    Dataflow.Query.Item.Name = null;
                    Dataflow.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.DataflowQueryType), Dataflow);

                    break;
                case 1:
                    SDMXObjectModel.Message.MetadataflowQueryType Metadataflow = new SDMXObjectModel.Message.MetadataflowQueryType();
                    Metadataflow.Header = Global.Get_Appropriate_Header();
                    Metadataflow.Query = new SDMXObjectModel.Query.MetadataflowQueryType();

                    Metadataflow.Query.ReturnDetails = new StructureReturnDetailsType();
                    Metadataflow.Query.ReturnDetails.References = new ReferencesType();
                    Metadataflow.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Metadataflow.Query.ReturnDetails.References.Item = new EmptyType();

                    Metadataflow.Query.Item = new MetadataflowWhereType();
                    Metadataflow.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Metadataflow;
                    Metadataflow.Query.Item.typeSpecified = true;

                    Metadataflow.Query.Item.ID = new QueryIDType();
                    Metadataflow.Query.Item.ID.Value = Id;

                    Metadataflow.Query.Item.AgencyID = new QueryNestedIDType();
                    Metadataflow.Query.Item.AgencyID.Value = AgencyId;

                    Metadataflow.Query.Item.Version = Version;

                    Metadataflow.Query.Item.Annotation = null;
                    Metadataflow.Query.Item.Name = null;
                    Metadataflow.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.MetadataflowQueryType), Metadataflow);

                    break;
                case 2:
                    SDMXObjectModel.Message.DataStructureQueryType DataStructure = new SDMXObjectModel.Message.DataStructureQueryType();
                    DataStructure.Header = Global.Get_Appropriate_Header();
                    DataStructure.Query = new SDMXObjectModel.Query.DataStructureQueryType();

                    DataStructure.Query.ReturnDetails = new StructureReturnDetailsType();
                    DataStructure.Query.ReturnDetails.References = new ReferencesType();
                    DataStructure.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    DataStructure.Query.ReturnDetails.References.Item = new EmptyType();

                    DataStructure.Query.Item = new DataStructureWhereType();
                    DataStructure.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.DataStructure;
                    DataStructure.Query.Item.typeSpecified = true;

                    DataStructure.Query.Item.ID = new QueryIDType();
                    DataStructure.Query.Item.ID.Value = Id;

                    DataStructure.Query.Item.AgencyID = new QueryNestedIDType();
                    DataStructure.Query.Item.AgencyID.Value = AgencyId;

                    DataStructure.Query.Item.Version = Version;

                    DataStructure.Query.Item.Annotation = null;
                    DataStructure.Query.Item.Name = null;
                    DataStructure.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.DataStructureQueryType), DataStructure);

                    break;
                case 3:
                    SDMXObjectModel.Message.MetadataStructureQueryType MetadataStructure = new SDMXObjectModel.Message.MetadataStructureQueryType();
                    MetadataStructure.Header = Global.Get_Appropriate_Header();
                    MetadataStructure.Query = new SDMXObjectModel.Query.MetadataStructureQueryType();

                    MetadataStructure.Query.ReturnDetails = new StructureReturnDetailsType();
                    MetadataStructure.Query.ReturnDetails.References = new ReferencesType();
                    MetadataStructure.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    MetadataStructure.Query.ReturnDetails.References.Item = new EmptyType();

                    MetadataStructure.Query.Item = new MetadataStructureWhereType();
                    MetadataStructure.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.MetadataStructure;
                    MetadataStructure.Query.Item.typeSpecified = true;

                    MetadataStructure.Query.Item.ID = new QueryIDType();
                    MetadataStructure.Query.Item.ID.Value = Id;

                    MetadataStructure.Query.Item.AgencyID = new QueryNestedIDType();
                    MetadataStructure.Query.Item.AgencyID.Value = AgencyId;

                    MetadataStructure.Query.Item.Version = Version;

                    MetadataStructure.Query.Item.Annotation = null;
                    MetadataStructure.Query.Item.Name = null;
                    MetadataStructure.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.MetadataStructureQueryType), MetadataStructure);

                    break;
                case 4:
                    SDMXObjectModel.Message.CategorySchemeQueryType CategoryScheme = new SDMXObjectModel.Message.CategorySchemeQueryType();
                    CategoryScheme.Header = Global.Get_Appropriate_Header();
                    CategoryScheme.Query = new SDMXObjectModel.Query.CategorySchemeQueryType();

                    CategoryScheme.Query.ReturnDetails = new StructureReturnDetailsType();
                    CategoryScheme.Query.ReturnDetails.References = new ReferencesType();
                    CategoryScheme.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    CategoryScheme.Query.ReturnDetails.References.Item = new EmptyType();

                    CategoryScheme.Query.Item = new CategorySchemeWhereType();
                    CategoryScheme.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.CategoryScheme;
                    CategoryScheme.Query.Item.typeSpecified = true;

                    CategoryScheme.Query.Item.ID = new QueryIDType();
                    CategoryScheme.Query.Item.ID.Value = Id;

                    CategoryScheme.Query.Item.AgencyID = new QueryNestedIDType();
                    CategoryScheme.Query.Item.AgencyID.Value = AgencyId;

                    CategoryScheme.Query.Item.Version = Version;

                    CategoryScheme.Query.Item.Annotation = null;
                    CategoryScheme.Query.Item.Name = null;
                    CategoryScheme.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.CategorySchemeQueryType), CategoryScheme);

                    break;
                case 5:
                    SDMXObjectModel.Message.CategorisationQueryType Categorisation = new SDMXObjectModel.Message.CategorisationQueryType();
                    Categorisation.Header = Global.Get_Appropriate_Header();
                    Categorisation.Query = new SDMXObjectModel.Query.CategorisationQueryType();

                    Categorisation.Query.ReturnDetails = new StructureReturnDetailsType();
                    Categorisation.Query.ReturnDetails.References = new ReferencesType();
                    Categorisation.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Categorisation.Query.ReturnDetails.References.Item = new EmptyType();

                    Categorisation.Query.Item = new CategorisationWhereType();
                    Categorisation.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Categorisation;
                    Categorisation.Query.Item.typeSpecified = true;

                    Categorisation.Query.Item.ID = new QueryIDType();
                    Categorisation.Query.Item.ID.Value = Id;

                    Categorisation.Query.Item.AgencyID = new QueryNestedIDType();
                    Categorisation.Query.Item.AgencyID.Value = AgencyId;

                    Categorisation.Query.Item.Version = Version;

                    Categorisation.Query.Item.Annotation = null;
                    Categorisation.Query.Item.Name = null;
                    Categorisation.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.CategorisationQueryType), Categorisation);

                    break;
                case 6:
                    SDMXObjectModel.Message.ConceptSchemeQueryType ConceptScheme = new SDMXObjectModel.Message.ConceptSchemeQueryType();
                    ConceptScheme.Header = Global.Get_Appropriate_Header();
                    ConceptScheme.Query = new SDMXObjectModel.Query.ConceptSchemeQueryType();

                    ConceptScheme.Query.ReturnDetails = new StructureReturnDetailsType();
                    ConceptScheme.Query.ReturnDetails.References = new ReferencesType();
                    ConceptScheme.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    ConceptScheme.Query.ReturnDetails.References.Item = new EmptyType();

                    ConceptScheme.Query.Item = new ConceptSchemeWhereType();
                    ConceptScheme.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.ConceptScheme;
                    ConceptScheme.Query.Item.typeSpecified = true;

                    ConceptScheme.Query.Item.ID = new QueryIDType();
                    ConceptScheme.Query.Item.ID.Value = Id;

                    ConceptScheme.Query.Item.AgencyID = new QueryNestedIDType();
                    ConceptScheme.Query.Item.AgencyID.Value = AgencyId;

                    ConceptScheme.Query.Item.Version = Version;

                    ConceptScheme.Query.Item.Annotation = null;
                    ConceptScheme.Query.Item.Name = null;
                    ConceptScheme.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.ConceptSchemeQueryType), ConceptScheme);

                    break;
                case 7:
                    SDMXObjectModel.Message.CodelistQueryType Codelist = new SDMXObjectModel.Message.CodelistQueryType();
                    Codelist.Header = Global.Get_Appropriate_Header();
                    Codelist.Query = new SDMXObjectModel.Query.CodelistQueryType();

                    Codelist.Query.ReturnDetails = new StructureReturnDetailsType();
                    Codelist.Query.ReturnDetails.References = new ReferencesType();
                    Codelist.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Codelist.Query.ReturnDetails.References.Item = new EmptyType();

                    Codelist.Query.Item = new CodelistWhereType();
                    Codelist.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Codelist;
                    Codelist.Query.Item.typeSpecified = true;

                    Codelist.Query.Item.ID = new QueryIDType();
                    Codelist.Query.Item.ID.Value = Id;

                    Codelist.Query.Item.AgencyID = new QueryNestedIDType();
                    Codelist.Query.Item.AgencyID.Value = AgencyId;

                    Codelist.Query.Item.Version = Version;

                    Codelist.Query.Item.Annotation = null;
                    Codelist.Query.Item.Name = null;
                    Codelist.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.CodelistQueryType), Codelist);

                    break;
                case 8:
                    SDMXObjectModel.Message.OrganisationSchemeQueryType OrganisationScheme = new SDMXObjectModel.Message.OrganisationSchemeQueryType();
                    OrganisationScheme.Header = Global.Get_Appropriate_Header();
                    OrganisationScheme.Query = new SDMXObjectModel.Query.OrganisationSchemeQueryType();

                    OrganisationScheme.Query.ReturnDetails = new StructureReturnDetailsType();
                    OrganisationScheme.Query.ReturnDetails.References = new ReferencesType();
                    OrganisationScheme.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    OrganisationScheme.Query.ReturnDetails.References.Item = new EmptyType();

                    OrganisationScheme.Query.Item = new OrganisationSchemeWhereType();
                    OrganisationScheme.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.OrganisationScheme;
                    OrganisationScheme.Query.Item.typeSpecified = true;

                    OrganisationScheme.Query.Item.ID = new QueryIDType();
                    OrganisationScheme.Query.Item.ID.Value = Id;

                    OrganisationScheme.Query.Item.AgencyID = new QueryNestedIDType();
                    OrganisationScheme.Query.Item.AgencyID.Value = AgencyId;

                    OrganisationScheme.Query.Item.Version = Version;

                    OrganisationScheme.Query.Item.Annotation = null;
                    OrganisationScheme.Query.Item.Name = null;
                    OrganisationScheme.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.OrganisationSchemeQueryType), OrganisationScheme);

                    break;
                case 9:
                    SDMXObjectModel.Message.ProvisionAgreementQueryType ProvisionAgreement = new SDMXObjectModel.Message.ProvisionAgreementQueryType();
                    ProvisionAgreement.Header = Global.Get_Appropriate_Header();
                    ProvisionAgreement.Query = new SDMXObjectModel.Query.ProvisionAgreementQueryType();

                    ProvisionAgreement.Query.ReturnDetails = new StructureReturnDetailsType();
                    ProvisionAgreement.Query.ReturnDetails.References = new ReferencesType();
                    ProvisionAgreement.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    ProvisionAgreement.Query.ReturnDetails.References.Item = new EmptyType();

                    ProvisionAgreement.Query.Item = new ProvisionAgreementWhereType();
                    ProvisionAgreement.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.ProvisionAgreement;
                    ProvisionAgreement.Query.Item.typeSpecified = true;

                    ProvisionAgreement.Query.Item.ID = new QueryIDType();
                    ProvisionAgreement.Query.Item.ID.Value = Id;

                    ProvisionAgreement.Query.Item.AgencyID = new QueryNestedIDType();
                    ProvisionAgreement.Query.Item.AgencyID.Value = AgencyId;

                    ProvisionAgreement.Query.Item.Version = Version;

                    ProvisionAgreement.Query.Item.Annotation = null;
                    ProvisionAgreement.Query.Item.Name = null;
                    ProvisionAgreement.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.ProvisionAgreementQueryType), ProvisionAgreement);

                    break;
                case 10:
                    SDMXObjectModel.Message.ConstraintQueryType Constraint = new SDMXObjectModel.Message.ConstraintQueryType();
                    Constraint.Header = Global.Get_Appropriate_Header();
                    Constraint.Query = new SDMXObjectModel.Query.ConstraintQueryType();

                    Constraint.Query.ReturnDetails = new StructureReturnDetailsType();
                    Constraint.Query.ReturnDetails.References = new ReferencesType();
                    Constraint.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Constraint.Query.ReturnDetails.References.Item = new EmptyType();

                    Constraint.Query.Item = new ConstraintWhereType();
                    Constraint.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Constraint;
                    Constraint.Query.Item.typeSpecified = true;

                    Constraint.Query.Item.ID = new QueryIDType();
                    Constraint.Query.Item.ID.Value = Id;

                    Constraint.Query.Item.AgencyID = new QueryNestedIDType();
                    Constraint.Query.Item.AgencyID.Value = AgencyId;

                    Constraint.Query.Item.Version = Version;

                    Constraint.Query.Item.Annotation = null;
                    Constraint.Query.Item.Name = null;
                    Constraint.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.ConstraintQueryType), Constraint);

                    break;

                case 11:
                    SDMXObjectModel.Message.StructuresQueryType Structures = new SDMXObjectModel.Message.StructuresQueryType();
                    Structures.Header = Global.Get_Appropriate_Header();
                    Structures.Query = new SDMXObjectModel.Query.StructuresQueryType();

                    Structures.Query.ReturnDetails = new StructureReturnDetailsType();
                    Structures.Query.ReturnDetails.References = new ReferencesType();
                    Structures.Query.ReturnDetails.References.ItemElementName = ReferencesChoiceType.None;
                    Structures.Query.ReturnDetails.References.Item = new EmptyType();

                    Structures.Query.Item = new StructuresWhereType();
                    Structures.Query.Item.type = SDMXObjectModel.Common.MaintainableTypeCodelistType.Any;
                    Structures.Query.Item.typeSpecified = true;

                    Structures.Query.Item.ID = new QueryIDType();
                    Structures.Query.Item.ID.Value = Id;

                    Structures.Query.Item.AgencyID = new QueryNestedIDType();
                    Structures.Query.Item.AgencyID.Value = AgencyId;

                    Structures.Query.Item.Version = Version;

                    Structures.Query.Item.Annotation = null;
                    Structures.Query.Item.Name = null;
                    Structures.Query.Item.Description = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructuresQueryType), Structures);
                    break;
                case 12:
                    UserID = UserIdAndType.Split('|')[0];
                    if (UserIdAndType.Split('|')[1] == "True")
                    {
                        UserType=UserTypes.Provider;
                    }
                    else
                    {
                        UserType=UserTypes.Consumer;
                    }

                    SDMXObjectModel.Message.RegistryInterfaceType QuerySubscription = new RegistryInterfaceType();
                    QuerySubscription.Header = Global.Get_Appropriate_Header();
                    QuerySubscription.Item = new SDMXObjectModel.Registry.QuerySubscriptionRequestType();
                    ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation = new OrganisationReferenceType();
                    ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items = new List<object>();

                    if (UserType == UserTypes.Consumer)
                    {
                        maintainableParentID = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Id;
                        parentAgencyID = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.AgencyId;
                        maintainableParentVersion = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Version;

                        ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items.Add(new SDMXObjectModel.Common.DataConsumerRefType());
                        ((SDMXObjectModel.Common.DataConsumerRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).id = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserID;
                        ((SDMXObjectModel.Common.DataConsumerRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).agencyID = parentAgencyID;
                        ((SDMXObjectModel.Common.DataConsumerRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).maintainableParentID = maintainableParentID;
                        ((SDMXObjectModel.Common.DataConsumerRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).maintainableParentVersion = maintainableParentVersion;
                    }
                    else
                    {
                        maintainableParentID = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id;
                        parentAgencyID = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId;
                        maintainableParentVersion = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version;

                        ((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items.Add(new SDMXObjectModel.Common.DataProviderRefType());
                        ((SDMXObjectModel.Common.DataProviderRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).id = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserID; 
                        ((SDMXObjectModel.Common.DataProviderRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).agencyID = parentAgencyID;
                        ((SDMXObjectModel.Common.DataProviderRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).maintainableParentID = maintainableParentID;
                        ((SDMXObjectModel.Common.DataProviderRefType)(((SDMXObjectModel.Registry.QuerySubscriptionRequestType)QuerySubscription.Item).Organisation.Items[0])).maintainableParentVersion = maintainableParentVersion;
                    }

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), QuerySubscription);
                    break;
                case 13:
                    UserID = UserIdAndType.Split('|')[0];
                    maintainableParentID = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id;
                    parentAgencyID = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId;
                    maintainableParentVersion = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version;

                    SDMXObjectModel.Message.RegistryInterfaceType QueryRegistration = new RegistryInterfaceType();
                    QueryRegistration.Header = Global.Get_Appropriate_Header();
                    QueryRegistration.Item= new SDMXObjectModel.Registry.QueryRegistrationRequestType();

                    ((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).QueryType = QueryTypeType.AllSets;
                    ((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item = new DataProviderReferenceType();
                    ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items = new List<object>();
                    ((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items.Add(new SDMXObjectModel.Common.DataProviderRefType());

                    ((DataProviderRefType)((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items[0]).id = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserID;

                    ((DataProviderRefType)((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items[0]).agencyID = parentAgencyID;
                    ((DataProviderRefType)((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items[0]).maintainableParentID = maintainableParentID;
                    ((DataProviderRefType)((DataProviderReferenceType)((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).Item).Items[0]).maintainableParentVersion = maintainableParentVersion;
                    ((SDMXObjectModel.Registry.QueryRegistrationRequestType)(QueryRegistration.Item)).ReferencePeriod = null;

                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), QueryRegistration);
                   
                    break;
                case 14:


                default:
                    RetVal = new XmlDocument();
                    XmlDeclaration Declaration = RetVal.CreateXmlDeclaration("1.0", null, null);
                    RetVal.AppendChild(Declaration);

                    XmlElement Element = RetVal.CreateElement("Root");
                    RetVal.AppendChild(Element);

                    break;

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private XmlDocument GetQueryXmlDocumentForSubmitSubscription(ActionType Action, string UserIdAndType, string agencyId, List<bool> isSOAPMailIds, List<string> notificationMailIds, List<bool> isSOAPHTTPs, List<string> notificationHTTPs, DateTime startDate, DateTime endDate, string eventSelector, Dictionary<string, string> dictCategories, string mfdId, string RegistryURN,SDMXObjectModel.Message.StructureHeaderType Header)
    {
        XmlDocument RetVal;
        string id;
        string userId;
        UserTypes userType;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        int counter = 0;
        DIConnection DIConnection;
      
        RetVal = null;
        id = string.Empty;
        userId = string.Empty;
        userType = UserTypes.Consumer;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            if (string.IsNullOrEmpty(RegistryURN))
            {
                id = Guid.NewGuid().ToString();
            }
            else
            {
                id = RegistryURN;
            }
           
            userId = UserIdAndType.Split('|')[0];
            if (UserIdAndType.Split('|')[1] == "True")
            {
                userType = UserTypes.Provider;
            }
            else
            {
                userType = UserTypes.Consumer;
            }

            RegistryInterface = new RegistryInterfaceType();
            if (Header == null)
            {
                RegistryInterface.Header = Global.Get_Appropriate_Header();
            }
            else
            {
                RegistryInterface.Header.ID = Header.ID.ToString();
                RegistryInterface.Header.Prepared = Header.Prepared.ToString();
                foreach (PartyType receiver in Header.Receiver)
                {
                    RegistryInterface.Header.Receiver = new PartyType();
                    RegistryInterface.Header.Receiver.Contact = receiver.Contact;
                    RegistryInterface.Header.Receiver.id = receiver.id;
                    RegistryInterface.Header.Receiver.Name = receiver.Name;
                }
                RegistryInterface.Header.Sender = (SDMXObjectModel.Message.SenderType)Header.Sender;
                RegistryInterface.Header.Test = Header.Test;
            }
            RegistryInterface.Item = new SDMXObjectModel.Registry.SubmitSubscriptionsRequestType();

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest = new List<SubscriptionRequestType>();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest.Add(new SubscriptionRequestType());

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription = new SDMXObjectModel.Registry.SubscriptionType();

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].action = Action;
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.Organisation = new OrganisationReferenceType();

            if (userType == UserTypes.Consumer)
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.Organisation.Items.Add(new DataConsumerRefType(DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + userId, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Version));
            }
            else
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.Organisation.Items.Add(new DataProviderRefType(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + userId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version));
            }

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.RegistryURN = id;

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.NotificationMailTo = new List<SDMXObjectModel.Registry.NotificationURLType>();
            foreach (string notificationMailId in notificationMailIds)
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.NotificationMailTo.Add(new SDMXObjectModel.Registry.NotificationURLType(isSOAPMailIds[counter], notificationMailIds[counter]));
                counter++;
            }


            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.NotificationHTTP = new List<SDMXObjectModel.Registry.NotificationURLType>();

            counter = 0;
            foreach (string notificationHTTP in notificationHTTPs)
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.NotificationHTTP.Add(new SDMXObjectModel.Registry.NotificationURLType(isSOAPHTTPs[counter], notificationHTTPs[counter]));
                counter++;
            }

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.SubscriberAssignedID = Guid.NewGuid().ToString();

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.ValidityPeriod = new SDMXObjectModel.Registry.ValidityPeriodType();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.ValidityPeriod.StartDate = startDate;
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.ValidityPeriod.EndDate = endDate;

            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector = new List<object>();

            if (eventSelector == "Data Registration")
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector.Add(new DataRegistrationEventsType());
                ((DataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items = new CategoryReferenceType[dictCategories.Keys.Count];
                ((DataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName = new SDMXObjectModel.Registry.DataRegistrationEventsChoiceType[dictCategories.Keys.Count];

                counter = 0;
                foreach (string categoryId in dictCategories.Keys)
                {
                    ((DataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[counter] = new CategoryReferenceType();
                    ((DataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName[counter] = SDMXObjectModel.Registry.DataRegistrationEventsChoiceType.Category;

                    ((CategoryReferenceType)((DataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[counter]).Items.Add(new CategoryRefType(categoryId, agencyId, dictCategories[categoryId], DevInfo.Lib.DI_LibSDMX.Constants.CategoryScheme.Sector.Version));

                    counter++;
                }
            }
            else if (eventSelector == "Metadata Registration")
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector.Add(new MetadataRegistrationEventsType());
                ((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items = new MaintainableEventType[1];

                ((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0] = new MaintainableEventType();
                ((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName = new MetadataRegistrationEventsChoiceType[1];
                ((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName[0] = SDMXObjectModel.Registry.MetadataRegistrationEventsChoiceType.MetadataflowReference;
                ((MaintainableEventType)((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0]).Item = new MaintainableQueryType();
                ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0]).Item)).id = mfdId;
                ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0]).Item)).agencyID = agencyId;
                ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0]).Item)).version = DevInfo.Lib.DI_LibSDMX.Constants.MFD.Area.Version;
            }
            else if (eventSelector == "Structural Metadata Registration")
            {
                ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector.Add(new StructuralRepositoryEventsType());
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).AgencyID = new List<string>();
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).AgencyID.Add(agencyId);
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items = new EmptyType[1];
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).Items[0] = new EmptyType();
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName = new StructuralRepositoryEventsChoiceType[1];
                ((StructuralRepositoryEventsType)((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)(RegistryInterface.Item)).SubscriptionRequest[0].Subscription.EventSelector[0]).ItemsElementName[0] = SDMXObjectModel.Registry.StructuralRepositoryEventsChoiceType.AllEvents;
            }
            RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistryInterface);
         
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private XmlDocument GetQueryXmlDocumentForSubmitRegistration(string UserIdAndType, string AgencyId, string DFDOrMFDId, string WebServiceURL, bool IsREST, string WADLURL, bool IsSOAP, string WSDLURL, string FileURL)
    {
        XmlDocument RetVal;
        string id;
        string paId;
        SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
        DIConnection DIConnection;
        string MaxNId;
        string UserId;

        RetVal = null;
        id = string.Empty;
        paId = string.Empty;
        MaxNId = string.Empty;
        UserId = string.Empty;

        try
        {
            UserId = UserIdAndType.Split('|')[0];
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            MaxNId = ((int)(Convert.ToInt32(DIConnection.ExecuteDataTable("SELECT MAX(ArtefactsNId) AS MaxNId FROM Artefacts;").Rows[0]["MaxNId"].ToString()) + 1)).ToString();
            id = Guid.NewGuid().ToString();
            paId = DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserId + "_" + DFDOrMFDId;

            RegistryInterface = new RegistryInterfaceType();
            RegistryInterface.Header = Global.Get_Appropriate_Header();
            RegistryInterface.Item = new SDMXObjectModel.Registry.SubmitRegistrationsRequestType();

            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest = new List<RegistrationRequestType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest.Add(new RegistrationRequestType());
            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration = new RegistrationType(id);


            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.indexTimeSeries = true;

            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.ProvisionAgreement = new ProvisionAgreementReferenceType();
            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.ProvisionAgreement.Items.Add(new ProvisionAgreementRefType(paId, AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.PA.Version));

            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.Datasource = new List<object>();
            //((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.Datasource.Add(new SDMXObjectModel.Registry.QueryableDataSourceType(WebServiceURL, IsREST, WADLURL, IsSOAP, WSDLURL));
            ((RegistrationRequestType)(((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)(RegistryInterface.Item)).RegistrationRequest[0])).Registration.Datasource.Add(FileURL);
            
            RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistryInterface);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private string SaveContentTemporarilyandDeleteOtherFiles(string content)
    {
        string RetVal;
        string TempPath;
        string FileNameWPath;
        XmlDocument XmlTempViewXml;

        RetVal = string.Empty;
        TempPath = Server.MapPath("../../stock/tempSDMXFiles");
        FileNameWPath = string.Empty;
        XmlTempViewXml = new XmlDocument();

        try
        {
            foreach (string ViewFile in Directory.GetFiles(TempPath))
            {
                if (Path.GetFileName(ViewFile).Contains("WSDemo_") && (DateTime.Now > File.GetCreationTime(ViewFile).AddHours(1)))
                {
                    File.Delete(ViewFile);
                }
            }

            XmlTempViewXml.LoadXml(this.Page.Server.HtmlDecode(content));

            this.Create_Directory_If_Not_Exists(TempPath);
            FileNameWPath = Path.Combine(TempPath, "WSDemo_" + Environment.TickCount.ToString() + ".xml");
            XmlTempViewXml.Save(FileNameWPath);

            RetVal = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries/")) + "stock/tempSDMXFiles/" + Path.GetFileName(FileNameWPath);
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

    #endregion "--Private--"

    #region "--Public--"

    public string GetDataForGenerateApiCall(string requestParam)
    {
        string RetVal;
        string[] Params;
        int DBNId;
        string SelectedFunction;
        string Id;
        string AgencyId;
        string Version;
        string UserIdAndType;
        string UploadedHeaderFileWPath, UploadedHeaderFolderPath;
        Registry.RegistryService Service;
        SDMXObjectModel.Message.StructureHeaderType Header;
        SDMXApi_2_0.Message.HeaderType Header_2_0;
        XmlDocument Query;
        XmlElement Element;
        XmlDocument Response;
        XmlDocument UploadedHeaderXml;
      

        string agencyId;
        List<bool> isSOAPMailIds;
        List<string> notificationMailIds;
        List<bool> isSOAPHTTPs;
        List<string> notificationHTTPs;
        DateTime startDate;
        DateTime endDate;
        string eventSelector;
        Dictionary<string, string> dictCategories;
        string mfdId;
        string CategoriesGIDAndSchemeIds;
        string CategoryGID;
        string CategorySchemeId;
        string DFDOrMFDId; 
        string WebServiceURL;
        bool IsREST;
        string WADLURL;
        bool IsSOAP;
        string WSDLURL;
        string FileURL;
        string RequestURL;
        string ResponseURL;
        string preferredLangNid;
        string[] DBDetails;
        string checkIfSDMXDB;
        RetVal = string.Empty;
        Params = null;
        DBNId = -1;
        SelectedFunction = string.Empty;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;
        UserIdAndType = string.Empty;
        Service = new Registry.RegistryService();

        Service.Url = HttpContext.Current.Request.Url.OriginalString.Substring(0, HttpContext.Current.Request.Url.OriginalString.IndexOf("libraries")) + Constants.FolderName.SDMX.RegistryServicePath;
        Query = new XmlDocument();
        Response = new XmlDocument();
        Element = null;

        agencyId = string.Empty;
        isSOAPMailIds = new List<bool>();
        notificationMailIds = new List<string>();
        isSOAPHTTPs = new List<bool>();
        notificationHTTPs = new List<string>();
        startDate = new DateTime();
        endDate = new DateTime();
        eventSelector = string.Empty;
        dictCategories = new Dictionary<string, string>();
        mfdId = string.Empty;
        CategoriesGIDAndSchemeIds = string.Empty;
        CategoryGID = string.Empty;
        CategorySchemeId = string.Empty;

        DFDOrMFDId = string.Empty;
        WebServiceURL = string.Empty;
        IsREST = false;
        WADLURL = string.Empty;
        IsSOAP = false;
        WSDLURL = string.Empty;
        FileURL = string.Empty;
        RequestURL = string.Empty;
        ResponseURL = string.Empty;
        preferredLangNid = string.Empty;
        DBDetails = null;
        checkIfSDMXDB = string.Empty;
        Header = new SDMXObjectModel.Message.StructureHeaderType();
        Header_2_0 = new SDMXApi_2_0.Message.HeaderType();
        UploadedHeaderXml = new XmlDocument();
        UploadedHeaderFolderPath = Server.MapPath("../../stock/data");
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Convert.ToInt32(Params[0].ToString().Trim());
            SelectedFunction = Params[1].ToString().Trim();

            DBDetails = Global.GetDbConnectionDetails(Convert.ToString(DBNId));
           checkIfSDMXDB = DBDetails[4].ToString();

            UploadedHeaderFileWPath = UploadedHeaderFolderPath + "/" + DBNId + "/" + "sdmx" + "/" + DevInfo.Lib.DI_LibSDMX.Constants.Header.FileName;
            if (File.Exists(UploadedHeaderFileWPath))
            {
                UploadedHeaderXml.Load(UploadedHeaderFileWPath);
                if (checkIfSDMXDB == "true")
                {
                    SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
                    UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
                    Header_2_0 = UploadedDSDStructure.Header;
                }
                else
                {
                    SDMXObjectModel.Message.StructureType UploadedDSDStructure = new SDMXObjectModel.Message.StructureType();
                    UploadedDSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UploadedHeaderXml);
                    Header = UploadedDSDStructure.Header;
                }
              
            }

            if ((SelectedFunction == "QuerySubscription") || (SelectedFunction == "QueryRegistration"))
            {
                UserIdAndType = Params[2].ToString().Trim();
            }
            else if (SelectedFunction == "SubmitSubscription")
            {
                UserIdAndType = Params[2].ToString().Trim();
               
                if (Params[3].ToString().Trim() == "0")
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
                notificationMailIds.Add(Params[4].ToString().Trim());
                notificationHTTPs = new List<string>();
                notificationHTTPs.Add(Params[5].ToString().Trim());
                startDate = DateTime.ParseExact(Params[6].ToString().Trim(), "dd-MM-yyyy", null);
                endDate = DateTime.ParseExact(Params[7].ToString().Trim(), "dd-MM-yyyy", null);
                eventSelector = Params[8].ToString().Trim();
                CategoriesGIDAndSchemeIds = Params[9].ToString().Trim();
                dictCategories = new Dictionary<string, string>();
                if (eventSelector == "Data Registration")
                {
                    foreach (string CategoryGIDAndSchemeId in Global.SplitString(CategoriesGIDAndSchemeIds, ","))
                    {
                        CategoryGID = CategoryGIDAndSchemeId.Split('|')[0];
                        CategorySchemeId = CategoryGIDAndSchemeId.Split('|')[1];
                        dictCategories.Add(CategoryGID, CategorySchemeId);
                    }
                }
                agencyId = Global.Get_AgencyId_From_DFD(DBNId.ToString());
                mfdId = Params[10].ToString().Trim();
                preferredLangNid = Params[11].ToString().Trim();
            }
            else if (SelectedFunction == "SubmitRegistration")
            {
                UserIdAndType = Params[2].ToString().Trim();
                agencyId = Global.Get_AgencyId_From_DFD(DBNId.ToString());
                DFDOrMFDId = Params[3].ToString().Trim();
                FileURL = Params[4].ToString().Trim();
            }
            else
            {
                Id = Params[2].ToString().Trim();
                AgencyId = Params[3].ToString().Trim();
                Version = Params[4].ToString().Trim();
            }

            Service.Url += "?p=" + DBNId.ToString();
            RetVal = Service.Url;
            RetVal += Constants.Delimiters.ParamDelimiter + SelectedFunction;
            switch (SelectedFunction)
            {
                case "GetDataflow":
                    Query = GetQueryXmlDocumentOnTypeBasis(0, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetDataflow(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetMetadataflow":
                    Query = GetQueryXmlDocumentOnTypeBasis(1, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetMetadataflow(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetDataStructure":
                    Query = GetQueryXmlDocumentOnTypeBasis(2, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetDataStructure(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetMetadataStructure":
                    Query = GetQueryXmlDocumentOnTypeBasis(3, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetMetadataStructure(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetCategoryScheme":
                    Query = GetQueryXmlDocumentOnTypeBasis(4, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetCategoryScheme(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetCategorisation":
                    Query = GetQueryXmlDocumentOnTypeBasis(5, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetCategorisation(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetConceptScheme":
                    Query = GetQueryXmlDocumentOnTypeBasis(6, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetConceptScheme(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetCodelist":
                    Query = GetQueryXmlDocumentOnTypeBasis(7, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetCodelist(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetOrganisationScheme":
                    Query = GetQueryXmlDocumentOnTypeBasis(8, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetOrganisationScheme(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetProvisionAgreement":
                    Query = GetQueryXmlDocumentOnTypeBasis(9, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetProvisionAgreement(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetConstraint":
                    Query = GetQueryXmlDocumentOnTypeBasis(10, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetConstraint(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetStructures":
                    Query = GetQueryXmlDocumentOnTypeBasis(11, Id, AgencyId, Version, string.Empty);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.GetStructures(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "QuerySubscription":
                    Query = GetQueryXmlDocumentOnTypeBasis(12, Id, AgencyId, Version, UserIdAndType);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.QuerySubscription(ref Element);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "QueryRegistration":
                    Query = GetQueryXmlDocumentOnTypeBasis(13, Id, AgencyId, Version, UserIdAndType);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.QueryRegistration(ref Element,preferredLangNid);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "SubmitSubscription":
                    Query = GetQueryXmlDocumentForSubmitSubscription(ActionType.Append, UserIdAndType, agencyId, isSOAPMailIds, notificationMailIds, isSOAPHTTPs, notificationHTTPs, startDate, endDate, eventSelector, dictCategories, mfdId, string.Empty,Header);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.SubmitSubscription(ref Element, preferredLangNid);//language code
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "SubmitRegistration":
                    Query = GetQueryXmlDocumentForSubmitRegistration(UserIdAndType, agencyId, DFDOrMFDId, WebServiceURL, IsREST, WADLURL, IsSOAP, WSDLURL, FileURL); 
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Query);
                    Element = Query.DocumentElement;
                    Service.SubmitRegistration(ref Element, string.Empty, preferredLangNid);
                    Response.LoadXml(Element.OuterXml);
                    RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
                    RequestURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Query));
                    ResponseURL = SaveContentTemporarilyandDeleteOtherFiles(this.Get_Formatted_XML(Response));
                    RetVal += Constants.Delimiters.ParamDelimiter + RequestURL;
                    RetVal += Constants.Delimiters.ParamDelimiter + ResponseURL;
                    break;
                case "GetGenericData":

                    break;
                case "GetGenericTimeSeriesData":

                    break;
                case "GetStructureSpecificData":

                    break;
                case "GetStructureSpecificTimeSeriesData":

                    break;
                case "GetGenericMetadata":

                    break;
                case "GetStructureSpecificMetadata":

                    break;
                case "SubmitStructure":

                    break;
                case "GetReportingTaxonomy":

                    break;
                case "GetStructureSet":

                    break;
                case "GetProcess":

                    break;
                case "GetHierarchicalCodelist":

                    break;
                case "GetDataSchema":

                    break;
                case "GetMetadataSchema":

                    break;
                default:
                    break;
            }
        }
        catch (SoapException SOAPex)
        {
            Response.LoadXml(SOAPex.Detail.InnerText);
            RetVal += Constants.Delimiters.ParamDelimiter + this.Get_Formatted_XML(Response);
            Global.CreateExceptionString(SOAPex, null);
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

    public string GetIndicativeIdAgencyIdVersionValues(string requestParam)
    {
        string RetVal;
        string DBNId, SelectedFunction, FileName;
        string Id, AgencyId, Version;
        string[] Params;
        SDMXObjectModel.Message.StructureType Structure_Two_One;
        SDMXApi_2_0.Message.StructureType Structure_Two_Zero;
        DIConnection DIConnection;
        DataTable DtTable;

        RetVal = string.Empty;

        DBNId = string.Empty;
        SelectedFunction = string.Empty;
        FileName = string.Empty;

        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;

        Params = null;
        Structure_Two_One = null;
        Structure_Two_Zero = null;
        DIConnection = null;
        DtTable = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DBNId = Params[0].ToString().Trim();
            SelectedFunction = Params[1].ToString().Trim();
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);

            switch (SelectedFunction)
            {
                case "GetDataflow":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.DFD).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Dataflows != null && Structure_Two_One.Structures.Dataflows.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.Dataflows[0].id;
                        AgencyId = Structure_Two_One.Structures.Dataflows[0].agencyID;
                        Version = Structure_Two_One.Structures.Dataflows[0].version;
                    }
                    break;
                case "GetMetadataflow":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.MFD).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Metadataflows != null && Structure_Two_One.Structures.Metadataflows.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.Metadataflows[0].id;
                        AgencyId = Structure_Two_One.Structures.Metadataflows[0].agencyID;
                        Version = Structure_Two_One.Structures.Metadataflows[0].version;
                    }
                    break;
                case "GetDataStructure":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.DSD).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)) == false)
                    {
                        Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                        if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.DataStructures != null && Structure_Two_One.Structures.DataStructures.Count > 0)
                        {
                            Id = Structure_Two_One.Structures.DataStructures[0].id;
                            AgencyId = Structure_Two_One.Structures.DataStructures[0].agencyID;
                            Version = Structure_Two_One.Structures.DataStructures[0].version;
                        }
                    }
                    else
                    {
                        Structure_Two_Zero = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), FileName);

                        if (Structure_Two_Zero != null && Structure_Two_Zero.KeyFamilies != null && Structure_Two_Zero.KeyFamilies.Count > 0)
                        {
                            Id = Structure_Two_Zero.KeyFamilies[0].id;
                            AgencyId = Structure_Two_Zero.KeyFamilies[0].agencyID;
                            Version = Structure_Two_Zero.KeyFamilies[0].version;
                        }
                    }
                    break;
                case "GetMetadataStructure":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.MSD).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)) == false)
                    {
                        Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                        if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.MetadataStructures != null && Structure_Two_One.Structures.MetadataStructures.Count > 0)
                        {
                            Id = Structure_Two_One.Structures.MetadataStructures[0].id;
                            AgencyId = Structure_Two_One.Structures.MetadataStructures[0].agencyID;
                            Version = Structure_Two_One.Structures.MetadataStructures[0].version;
                        }
                    }
                    else
                    {
                        Structure_Two_Zero = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), FileName);

                        if (Structure_Two_Zero != null && Structure_Two_Zero.MetadataStructureDefinitions != null && Structure_Two_Zero.MetadataStructureDefinitions.Count > 0)
                        {
                            Id = Structure_Two_Zero.MetadataStructureDefinitions[0].id;
                            AgencyId = Structure_Two_Zero.MetadataStructureDefinitions[0].agencyID;
                            Version = Structure_Two_Zero.MetadataStructureDefinitions[0].version;
                        }
                    }
                    break;
                case "GetCategoryScheme":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.CategoryS).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.CategorySchemes != null && Structure_Two_One.Structures.CategorySchemes.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.CategorySchemes[0].id;
                        AgencyId = Structure_Two_One.Structures.CategorySchemes[0].agencyID;
                        Version = Structure_Two_One.Structures.CategorySchemes[0].version;
                    }
                    break;
                case "GetCategorisation":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.Categorisation).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Categorisations != null && Structure_Two_One.Structures.Categorisations.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.Categorisations[0].id;
                        AgencyId = Structure_Two_One.Structures.Categorisations[0].agencyID;
                        Version = Structure_Two_One.Structures.Categorisations[0].version;
                    }
                    break;
                case "GetConceptScheme":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.ConceptS).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)) == false)
                    {
                        Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                        if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Concepts != null && Structure_Two_One.Structures.Concepts.Count > 0)
                        {
                            Id = Structure_Two_One.Structures.Concepts[0].id;
                            AgencyId = Structure_Two_One.Structures.Concepts[0].agencyID;
                            Version = Structure_Two_One.Structures.Concepts[0].version;
                        }
                    }
                    else
                    {
                        Structure_Two_Zero = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), FileName);

                        if (Structure_Two_Zero != null && Structure_Two_Zero.Concepts != null && Structure_Two_Zero.Concepts.ConceptScheme != null && Structure_Two_Zero.Concepts.ConceptScheme.Count > 0)
                        {
                            Id = Structure_Two_Zero.Concepts.ConceptScheme[0].id;
                            AgencyId = Structure_Two_Zero.Concepts.ConceptScheme[0].agencyID;
                            Version = Structure_Two_Zero.Concepts.ConceptScheme[0].version;
                        }
                    }
                    break;
                case "GetCodelist":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.CL).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    if (Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)) == false)
                    {
                        Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                        if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Codelists != null && Structure_Two_One.Structures.Codelists.Count > 0)
                        {
                            Id = Structure_Two_One.Structures.Codelists[0].id;
                            AgencyId = Structure_Two_One.Structures.Codelists[0].agencyID;
                            Version = Structure_Two_One.Structures.Codelists[0].version;
                        }
                    }
                    else
                    {
                        Structure_Two_Zero = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), FileName);

                        if (Structure_Two_Zero != null && Structure_Two_Zero.CodeLists != null && Structure_Two_Zero.CodeLists.Count > 0)
                        {
                            Id = Structure_Two_Zero.CodeLists[0].id;
                            AgencyId = Structure_Two_Zero.CodeLists[0].agencyID;
                            Version = Structure_Two_Zero.CodeLists[0].version;
                        }
                    }
                    break;
                case "GetOrganisationScheme":
                    FileName = Path.Combine(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users), DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.OrganisationSchemes != null && Structure_Two_One.Structures.OrganisationSchemes.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.OrganisationSchemes[0].id;
                        AgencyId = Structure_Two_One.Structures.OrganisationSchemes[0].agencyID;
                        Version = Structure_Two_One.Structures.OrganisationSchemes[0].version;
                    }
                    break;
                case "GetProvisionAgreement":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.PA).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.ProvisionAgreements != null && Structure_Two_One.Structures.ProvisionAgreements.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.ProvisionAgreements[0].id;
                        AgencyId = Structure_Two_One.Structures.ProvisionAgreements[0].agencyID;
                        Version = Structure_Two_One.Structures.ProvisionAgreements[0].version;
                    }
                    break;
                case "GetConstraint":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.Constraint).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Constraints != null && Structure_Two_One.Structures.Constraints.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.Constraints[0].id;
                        AgencyId = Structure_Two_One.Structures.Constraints[0].agencyID;
                        Version = Structure_Two_One.Structures.Constraints[0].version;
                    }
                    break;
                case "GetStructures":
                    DtTable = DIConnection.ExecuteDataTable("SELECT FileLocation FROM Artefacts Where DBNId = " + DBNId + " AND Type = " + Convert.ToInt32(DevInfo.Lib.DI_LibSDMX.ArtefactTypes.DFD).ToString());

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        FileName = DtTable.Rows[0]["FileLocation"].ToString();
                    }

                    Structure_Two_One = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), FileName);

                    if (Structure_Two_One != null && Structure_Two_One.Structures != null && Structure_Two_One.Structures.Dataflows != null && Structure_Two_One.Structures.Dataflows.Count > 0)
                    {
                        Id = Structure_Two_One.Structures.Dataflows[0].id;
                        AgencyId = Structure_Two_One.Structures.Dataflows[0].agencyID;
                        Version = Structure_Two_One.Structures.Dataflows[0].version;
                    }
                    break;
                default:
                    break;
            }

            RetVal = Id + "," + AgencyId + "," + Version;
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

    #endregion "--Public--"

    #endregion "--Methods--"

}
