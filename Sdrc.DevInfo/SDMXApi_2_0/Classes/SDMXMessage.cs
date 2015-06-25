using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Structure;
using SDMXApi_2_0.Common;
using SDMXApi_2_0.Query;
using SDMXApi_2_0.MetadataReport;
using SDMXApi_2_0.CrossSectionalData;
using SDMXApi_2_0.CompactData;
using SDMXApi_2_0.UtilityData;
using SDMXApi_2_0.Registry;

namespace SDMXApi_2_0.Message
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("Structure", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class StructureType : MessageType
    {

        private List<OrganisationSchemeType> organisationSchemesField;

        private List<DataflowType> dataflowsField;

        private List<MetadataflowType> metadataflowsField;

        private List<CategorySchemeType> categorySchemesField;

        private List<CodeListType> codeListsField;

        private List<HierarchicalCodelistType> hierarchicalCodelistsField;

        private ConceptsType conceptsField;

        private List<MetadataStructureDefinitionType> metadataStructureDefinitionsField;

        private List<KeyFamilyType> keyFamiliesField;

        private List<StructureSetType> structureSetsField;

        private List<ReportingTaxonomyType> reportingTaxonomiesField;

        private List<ProcessType> processesField;

        public StructureType()
        {
            this.processesField = new List<ProcessType>();
            this.reportingTaxonomiesField = new List<ReportingTaxonomyType>();
            this.structureSetsField = new List<StructureSetType>();
            this.keyFamiliesField = new List<KeyFamilyType>();
            this.metadataStructureDefinitionsField = new List<MetadataStructureDefinitionType>();
            this.conceptsField = new ConceptsType();
            this.hierarchicalCodelistsField = new List<HierarchicalCodelistType>();
            this.codeListsField = new List<CodeListType>();
            this.categorySchemesField = new List<CategorySchemeType>();
            this.metadataflowsField = new List<MetadataflowType>();
            this.dataflowsField = new List<DataflowType>();
            this.organisationSchemesField = new List<OrganisationSchemeType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OrganisationScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<OrganisationSchemeType> OrganisationSchemes
        {
            get
            {
                return this.organisationSchemesField;
            }
            set
            {
                this.organisationSchemesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Dataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<DataflowType> Dataflows
        {
            get
            {
                return this.dataflowsField;
            }
            set
            {
                this.dataflowsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metadataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<MetadataflowType> Metadataflows
        {
            get
            {
                return this.metadataflowsField;
            }
            set
            {
                this.metadataflowsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<CategorySchemeType> CategorySchemes
        {
            get
            {
                return this.categorySchemesField;
            }
            set
            {
                this.categorySchemesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CodeList", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<CodeListType> CodeLists
        {
            get
            {
                return this.codeListsField;
            }
            set
            {
                this.codeListsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("HierarchicalCodelist", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<HierarchicalCodelistType> HierarchicalCodelists
        {
            get
            {
                return this.hierarchicalCodelistsField;
            }
            set
            {
                this.hierarchicalCodelistsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public ConceptsType Concepts
        {
            get
            {
                return this.conceptsField;
            }
            set
            {
                this.conceptsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataStructureDefinition", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<MetadataStructureDefinitionType> MetadataStructureDefinitions
        {
            get
            {
                return this.metadataStructureDefinitionsField;
            }
            set
            {
                this.metadataStructureDefinitionsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyFamily", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<KeyFamilyType> KeyFamilies
        {
            get
            {
                return this.keyFamiliesField;
            }
            set
            {
                this.keyFamiliesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructureSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<StructureSetType> StructureSets
        {
            get
            {
                return this.structureSetsField;
            }
            set
            {
                this.structureSetsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReportingTaxonomy", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<ReportingTaxonomyType> ReportingTaxonomies
        {
            get
            {
                return this.reportingTaxonomiesField;
            }
            set
            {
                this.reportingTaxonomiesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Process", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<ProcessType> Processes
        {
            get
            {
                return this.processesField;
            }
            set
            {
                this.processesField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = true)]
    public class ContactType
    {

        private List<TextType> nameField;

        private List<TextType> departmentField;

        private List<TextType> roleField;

        private string[] itemsField;

        private ContactChoiceType[] itemsElementNameField;

        public ContactType()
        {
            this.roleField = new List<TextType>();
            this.departmentField = new List<TextType>();
            this.nameField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Order = 0)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Department", Order = 1)]
        public List<TextType> Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 2)]
        public List<TextType> Role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Email", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Fax", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Telephone", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("URI", typeof(string), DataType = "anyURI", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("X400", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 4)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ContactChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IncludeInSchema = false)]
    public enum ContactChoiceType
    {

        /// <remarks/>
        Email,

        /// <remarks/>
        Fax,

        /// <remarks/>
        Telephone,

        /// <remarks/>
        URI,

        /// <remarks/>
        X400,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = true)]
    public class PartyType
    {

        private List<TextType> nameField;

        private List<ContactType> contactField;

        private string idField;

        public PartyType()
        {
            this.contactField = new List<ContactType>();
            this.nameField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Order = 0)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Contact", Order = 1)]
        public List<ContactType> Contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("Header", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class HeaderType
    {

        private string idField;

        private bool testField;

        private bool truncatedField;

        private bool truncatedFieldSpecified;

        private List<TextType> nameField;

        private string preparedField;

        private List<PartyType> senderField;

        private List<PartyType> receiverField;

        private string keyFamilyRefField;

        private string keyFamilyAgencyField;

        private string dataSetAgencyField;

        private string dataSetIDField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private List<TextType> sourceField;

        public HeaderType()
        {
            this.sourceField = new List<TextType>();
            this.receiverField = new List<PartyType>();
            this.senderField = new List<PartyType>();
            this.nameField = new List<TextType>();
            this.testField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool Truncated
        {
            get
            {
                return this.truncatedField;
            }
            set
            {
                this.truncatedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TruncatedSpecified
        {
            get
            {
                return this.truncatedFieldSpecified;
            }
            set
            {
                this.truncatedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Order = 3)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Sender", Order = 5)]
        public List<PartyType> Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 6)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "NMTOKEN", Order = 7)]
        public string KeyFamilyRef
        {
            get
            {
                return this.keyFamilyRefField;
            }
            set
            {
                this.keyFamilyRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "NMTOKEN", Order = 8)]
        public string KeyFamilyAgency
        {
            get
            {
                return this.keyFamilyAgencyField;
            }
            set
            {
                this.keyFamilyAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "NMTOKEN", Order = 9)]
        public string DataSetAgency
        {
            get
            {
                return this.dataSetAgencyField;
            }
            set
            {
                this.dataSetAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "NMTOKEN", Order = 10)]
        public string DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 15)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    //[System.Xml.Serialization.XmlIncludeAttribute(typeof(MessageGroupType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RegistryInterfaceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(QueryMessageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataReportType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericMetadataType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CrossSectionalDataType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompactDataType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UtilityDataType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = true)]
    public abstract class MessageType
    {

        private HeaderType headerField;

        public MessageType()
        {
            this.headerField = new HeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public HeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }
    }

    //[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    //[System.SerializableAttribute()]
    //[System.ComponentModel.DesignerCategoryAttribute("code")]
    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    //[System.Xml.Serialization.XmlRootAttribute("MessageGroup", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    //public class MessageGroupType : MessageType
    //{

    //    private List<object> itemsField;

    //    private string idField;

    //    public MessageGroupType()
    //    {
    //        this.itemsField = new List<object>();
    //    }

    //    [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SDMXApi_2_0.CompactData.DataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/compact", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SDMXApi_2_0.CrossSectionalData.DataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/cross", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SDMXApi_2_0.GenericData.DataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(MetadataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(SDMXApi_2_0.MetadataReport.MetadataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/metadatareport", Order = 0)]
    //    [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SDMXApi_2_0.UtilityData.DataSetType), Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/utility", Order = 0)]
    //    public List<object> Items
    //    {
    //        get
    //        {
    //            return this.itemsField;
    //        }
    //        set
    //        {
    //            this.itemsField = value;
    //        }
    //    }

    //    [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
    //    public string id
    //    {
    //        get
    //        {
    //            return this.idField;
    //        }
    //        set
    //        {
    //            this.idField = value;
    //        }
    //    }
    //}

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("RegistryInterface", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class RegistryInterfaceType : MessageType
    {

        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("NotifyRegistryEvent", typeof(NotifyRegistryEventType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryProvisioningRequest", typeof(QueryProvisioningRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryProvisioningResponse", typeof(QueryProvisioningResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryRegistrationRequest", typeof(QueryRegistrationRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryRegistrationResponse", typeof(QueryRegistrationResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryStructureRequest", typeof(QueryStructureRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryStructureResponse", typeof(QueryStructureResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitProvisioningRequest", typeof(SubmitProvisioningRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitProvisioningResponse", typeof(SubmitProvisioningResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitRegistrationRequest", typeof(SubmitRegistrationRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitRegistrationResponse", typeof(SubmitRegistrationResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitStructureRequest", typeof(SubmitStructureRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitStructureResponse", typeof(SubmitStructureResponseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitSubscriptionRequest", typeof(SubmitSubscriptionRequestType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitSubscriptionResponse", typeof(SubmitSubscriptionResponseType), Order = 0)]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("QueryMessage", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class QueryMessageType : MessageType
    {

        private QueryType queryField;

        public QueryMessageType()
        {
            this.queryField = new QueryType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public QueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataReport", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class MetadataReportType : MessageType
    {

        private SDMXApi_2_0.MetadataReport.MetadataSetType metadataSetField;

        public MetadataReportType()
        {
            this.metadataSetField = new SDMXApi_2_0.MetadataReport.MetadataSetType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/metadatareport", Order = 0)]
        public SDMXApi_2_0.MetadataReport.MetadataSetType MetadataSet
        {
            get
            {
                return this.metadataSetField;
            }
            set
            {
                this.metadataSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericMetadata", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class GenericMetadataType : MessageType
    {

        private SDMXApi_2_0.GenericMetadata.MetadataSetType metadataSetField;

        public GenericMetadataType()
        {
            this.metadataSetField = new SDMXApi_2_0.GenericMetadata.MetadataSetType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", Order = 0)]
        public SDMXApi_2_0.GenericMetadata.MetadataSetType MetadataSet
        {
            get
            {
                return this.metadataSetField;
            }
            set
            {
                this.metadataSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("CrossSectionalData", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class CrossSectionalDataType : MessageType
    {

        private SDMXApi_2_0.CrossSectionalData.DataSetType dataSetField;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/cross", Order = 0)]
        public SDMXApi_2_0.CrossSectionalData.DataSetType DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("CompactData", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class CompactDataType : MessageType
    {

        private SDMXApi_2_0.CompactData.DataSetType dataSetField;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = Constants.Namespaces.URLs.DevInfo, Order = 0)]
        public SDMXApi_2_0.CompactData.DataSetType DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("UtilityData", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class UtilityDataType : MessageType
    {

        private SDMXApi_2_0.UtilityData.DataSetType dataSetField;

        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/utility", Order = 0)]
        public SDMXApi_2_0.UtilityData.DataSetType DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericData", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/message", IsNullable = false)]
    public class GenericDataType : MessageType
    {

        private SDMXApi_2_0.GenericData.DataSetType dataSetField;

        public GenericDataType()
        {
            this.dataSetField = new SDMXApi_2_0.GenericData.DataSetType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public SDMXApi_2_0.GenericData.DataSetType DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }
    }
}
