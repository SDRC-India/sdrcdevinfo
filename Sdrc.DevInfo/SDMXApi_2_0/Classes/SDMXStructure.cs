using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Common;

namespace SDMXApi_2_0.Structure
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationSchemesType
    {

        private List<OrganisationSchemeType> organisationSchemeField;

        public OrganisationSchemesType()
        {
            this.organisationSchemeField = new List<OrganisationSchemeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("OrganisationScheme", Order = 0)]
        public List<OrganisationSchemeType> OrganisationScheme
        {
            get
            {
                return this.organisationSchemeField;
            }
            set
            {
                this.organisationSchemeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationSchemeType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<OrganisationType> agenciesField;

        private List<OrganisationType> dataProvidersField;

        private List<OrganisationType> dataConsumersField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string agencyIDField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public OrganisationSchemeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.dataConsumersField = new List<OrganisationType>();
            this.dataProvidersField = new List<OrganisationType>();
            this.agenciesField = new List<OrganisationType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Agency", typeof(OrganisationType), IsNullable = false)]
        public List<OrganisationType> Agencies
        {
            get
            {
                return this.agenciesField;
            }
            set
            {
                this.agenciesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataProvider", typeof(OrganisationType), IsNullable = false)]
        public List<OrganisationType> DataProviders
        {
            get
            {
                return this.dataProvidersField;
            }
            set
            {
                this.dataProvidersField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataConsumer", typeof(OrganisationType), IsNullable = false)]
        public List<OrganisationType> DataConsumers
        {
            get
            {
                return this.dataConsumersField;
            }
            set
            {
                this.dataConsumersField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ContactType
    {

        private List<TextType> nameField;

        private string idField;

        private List<TextType> departmentField;

        private List<TextType> roleField;

        private string[] itemsField;

        private ContactChoiceType[] itemsElementNameField;

        public ContactType()
        {
            //this.itemsElementNameField = new List<ContactChoiceType>();
            //this.itemsField = new List<string>();
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute("Department", Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 3)]
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

        [System.Xml.Serialization.XmlElementAttribute("Email", typeof(string), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("Fax", typeof(string), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("Telephone", typeof(string), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("URI", typeof(string), DataType = "anyURI", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("X400", typeof(string), Order = 4)]
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 5)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IncludeInSchema = false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private ContactType maintenanceContactField;

        private ContactType collectorContactField;

        private ContactType disseminatorContactField;

        private ContactType reporterContactField;

        private List<ContactType> otherContactField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string parentOrganisationField;

        private string validFromField;

        private string validToField;

        public OrganisationType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.otherContactField = new List<ContactType>();
            this.reporterContactField = new ContactType();
            this.disseminatorContactField = new ContactType();
            this.collectorContactField = new ContactType();
            this.maintenanceContactField = new ContactType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ContactType MaintenanceContact
        {
            get
            {
                return this.maintenanceContactField;
            }
            set
            {
                this.maintenanceContactField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public ContactType CollectorContact
        {
            get
            {
                return this.collectorContactField;
            }
            set
            {
                this.collectorContactField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ContactType DisseminatorContact
        {
            get
            {
                return this.disseminatorContactField;
            }
            set
            {
                this.disseminatorContactField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ContactType ReporterContact
        {
            get
            {
                return this.reporterContactField;
            }
            set
            {
                this.reporterContactField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("OtherContact", Order = 6)]
        public List<ContactType> OtherContact
        {
            get
            {
                return this.otherContactField;
            }
            set
            {
                this.otherContactField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentOrganisation
        {
            get
            {
                return this.parentOrganisationField;
            }
            set
            {
                this.parentOrganisationField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataProvidersType
    {

        private List<OrganisationType> dataProviderField;

        public DataProvidersType()
        {
            this.dataProviderField = new List<OrganisationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", Order = 0)]
        public List<OrganisationType> DataProvider
        {
            get
            {
                return this.dataProviderField;
            }
            set
            {
                this.dataProviderField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataConsumersType
    {

        private List<OrganisationType> dataConsumerField;

        public DataConsumersType()
        {
            this.dataConsumerField = new List<OrganisationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataConsumer", Order = 0)]
        public List<OrganisationType> DataConsumer
        {
            get
            {
                return this.dataConsumerField;
            }
            set
            {
                this.dataConsumerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class AgenciesType
    {

        private List<OrganisationType> agencyField;

        public AgenciesType()
        {
            this.agencyField = new List<OrganisationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Agency", Order = 0)]
        public List<OrganisationType> Agency
        {
            get
            {
                return this.agencyField;
            }
            set
            {
                this.agencyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataflowsType
    {

        private List<DataflowType> dataflowField;

        public DataflowsType()
        {
            this.dataflowField = new List<DataflowType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Dataflow", Order = 0)]
        public List<DataflowType> Dataflow
        {
            get
            {
                return this.dataflowField;
            }
            set
            {
                this.dataflowField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataflowType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private KeyFamilyRefType keyFamilyRefField;

        private List<CategoryRefType> categoryRefField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string urnField;

        private string uriField;

        private string agencyIDField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string validFromField;

        private string validToField;

        public DataflowType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryRefField = new List<CategoryRefType>();
            this.keyFamilyRefField = new KeyFamilyRefType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public KeyFamilyRefType KeyFamilyRef
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

        [System.Xml.Serialization.XmlElementAttribute("CategoryRef", Order = 3)]
        public List<CategoryRefType> CategoryRef
        {
            get
            {
                return this.categoryRefField;
            }
            set
            {
                this.categoryRefField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class KeyFamilyRefType
    {

        private string uRNField;

        private string keyFamilyIDField;

        private string keyFamilyAgencyIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string KeyFamilyID
        {
            get
            {
                return this.keyFamilyIDField;
            }
            set
            {
                this.keyFamilyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string KeyFamilyAgencyID
        {
            get
            {
                return this.keyFamilyAgencyIDField;
            }
            set
            {
                this.keyFamilyAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategoryRefType
    {

        private string uRNField;

        private string categorySchemeIDField;

        private string categorySchemeAgencyIDField;

        private string categorySchemeVersionField;

        private CategoryIDType categoryIDField;

        public CategoryRefType()
        {
            this.categoryIDField = new CategoryIDType();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string CategorySchemeID
        {
            get
            {
                return this.categorySchemeIDField;
            }
            set
            {
                this.categorySchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CategorySchemeAgencyID
        {
            get
            {
                return this.categorySchemeAgencyIDField;
            }
            set
            {
                this.categorySchemeAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string CategorySchemeVersion
        {
            get
            {
                return this.categorySchemeVersionField;
            }
            set
            {
                this.categorySchemeVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public CategoryIDType CategoryID
        {
            get
            {
                return this.categoryIDField;
            }
            set
            {
                this.categoryIDField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategoryIDType
    {

        private string idField;

        private string categoryVersionField;

        private CategoryIDType categoryIDField;

        public CategoryIDType()
        {
            this.categoryIDField = new CategoryIDType();
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
        public string CategoryVersion
        {
            get
            {
                return this.categoryVersionField;
            }
            set
            {
                this.categoryVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public CategoryIDType CategoryID
        {
            get
            {
                return this.categoryIDField;
            }
            set
            {
                this.categoryIDField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataflowsType
    {

        private List<MetadataflowType> metadataflowField;

        public MetadataflowsType()
        {
            this.metadataflowField = new List<MetadataflowType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", Order = 0)]
        public List<MetadataflowType> Metadataflow
        {
            get
            {
                return this.metadataflowField;
            }
            set
            {
                this.metadataflowField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataflowType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private MetadataStructureRefType metadataStructureRefField;

        private List<CategoryRefType> categoryRefField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string urnField;

        private string uriField;

        private string agencyIDField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string validFromField;

        private string validToField;

        public MetadataflowType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryRefField = new List<CategoryRefType>();
            this.metadataStructureRefField = new MetadataStructureRefType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MetadataStructureRefType MetadataStructureRef
        {
            get
            {
                return this.metadataStructureRefField;
            }
            set
            {
                this.metadataStructureRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryRef", Order = 3)]
        public List<CategoryRefType> CategoryRef
        {
            get
            {
                return this.categoryRefField;
            }
            set
            {
                this.categoryRefField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataStructureRefType
    {

        private string uRNField;

        private string metadataStructureIDField;

        private string metadataStructureAgencyIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string MetadataStructureID
        {
            get
            {
                return this.metadataStructureIDField;
            }
            set
            {
                this.metadataStructureIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MetadataStructureAgencyID
        {
            get
            {
                return this.metadataStructureAgencyIDField;
            }
            set
            {
                this.metadataStructureAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategorySchemesType
    {

        private List<CategorySchemeType> categorySchemeField;

        public CategorySchemesType()
        {
            this.categorySchemeField = new List<CategorySchemeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryScheme", Order = 0)]
        public List<CategorySchemeType> CategoryScheme
        {
            get
            {
                return this.categorySchemeField;
            }
            set
            {
                this.categorySchemeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategorySchemeType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<CategoryType> categoryField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public CategorySchemeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryField = new List<CategoryType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 2)]
        public List<CategoryType> Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategoryType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<DataflowRefType> dataflowRefField;

        private List<MetadataflowRefType> metadataflowRefField;

        private List<CategoryType> categoryField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        public CategoryType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryField = new List<CategoryType>();
            this.metadataflowRefField = new List<MetadataflowRefType>();
            this.dataflowRefField = new List<DataflowRefType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", Order = 2)]
        public List<DataflowRefType> DataflowRef
        {
            get
            {
                return this.dataflowRefField;
            }
            set
            {
                this.dataflowRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", Order = 3)]
        public List<MetadataflowRefType> MetadataflowRef
        {
            get
            {
                return this.metadataflowRefField;
            }
            set
            {
                this.metadataflowRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 4)]
        public List<CategoryType> Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
       
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataflowRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string dataflowIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string DataflowID
        {
            get
            {
                return this.dataflowIDField;
            }
            set
            {
                this.dataflowIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataflowRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string metadataflowIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MetadataflowID
        {
            get
            {
                return this.metadataflowIDField;
            }
            set
            {
                this.metadataflowIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodeListsType
    {

        private List<CodeListType> codeListField;

        public CodeListsType()
        {
            this.codeListField = new List<CodeListType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("CodeList", Order = 0)]
        public List<CodeListType> CodeList
        {
            get
            {
                return this.codeListField;
            }
            set
            {
                this.codeListField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodeListType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<CodeType> codeField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public CodeListType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.codeField = new List<CodeType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Code", Order = 2)]
        public List<CodeType> Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        
        public List<AnnotationType> Annotations
        {
            get
            {
               return this.annotationsField == null ? null : this.annotationsField; 
               //return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodeType
    {

        private List<TextType> descriptionField;

        private List<AnnotationType> annotationsField;

        private string valueField;

        private string urnField;

        private string parentCodeField;

        public CodeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.descriptionField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 0)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {               
              return this.annotationsField == null ? null : this.annotationsField;
              //  return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        //public List<AnnotationType> Annotations
        //{
        //    get
        //    {
        //        return this.annotationsField;
        //    }
        //    set
        //    {
        //        this.annotationsField = value;
        //    }
        //}

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentCode
        {
            get
            {
                return this.parentCodeField;
            }
            set
            {
                this.parentCodeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class HierarchicalCodelistsType
    {

        private List<HierarchicalCodelistType> hierarchicalCodelistField;

        public HierarchicalCodelistsType()
        {
            this.hierarchicalCodelistField = new List<HierarchicalCodelistType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelist", Order = 0)]
        public List<HierarchicalCodelistType> HierarchicalCodelist
        {
            get
            {
                return this.hierarchicalCodelistField;
            }
            set
            {
                this.hierarchicalCodelistField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class HierarchicalCodelistType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<CodelistRefType> codelistRefField;

        private List<HierarchyType> hierarchyField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public HierarchicalCodelistType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.hierarchyField = new List<HierarchyType>();
            this.codelistRefField = new List<CodelistRefType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodelistRef", Order = 2)]
        public List<CodelistRefType> CodelistRef
        {
            get
            {
                return this.codelistRefField;
            }
            set
            {
                this.codelistRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Hierarchy", Order = 3)]
        public List<HierarchyType> Hierarchy
        {
            get
            {
                return this.hierarchyField;
            }
            set
            {
                this.hierarchyField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodelistRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string codelistIDField;

        private string versionField;

        private string aliasField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CodelistID
        {
            get
            {
                return this.codelistIDField;
            }
            set
            {
                this.codelistIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Alias
        {
            get
            {
                return this.aliasField;
            }
            set
            {
                this.aliasField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class HierarchyType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<CodeRefType> codeRefField;

        private List<LevelType> levelField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        private string versionField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public HierarchyType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.levelField = new List<LevelType>();
            this.codeRefField = new List<CodeRefType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodeRef", Order = 2)]
        public List<CodeRefType> CodeRef
        {
            get
            {
                return this.codeRefField;
            }
            set
            {
                this.codeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Level", Order = 3)]
        public List<LevelType> Level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodeRefType
    {

        private string uRNField;

        private string codelistAliasRefField;

        private string codeIDField;

        private List<CodeRefType> codeRefField;

        private string levelRefField;

        private string nodeAliasIDField;

        private string versionField;

        private string validFromField;

        private string validToField;

        public CodeRefType()
        {
            this.codeRefField = new List<CodeRefType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string CodelistAliasRef
        {
            get
            {
                return this.codelistAliasRefField;
            }
            set
            {
                this.codelistAliasRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CodeID
        {
            get
            {
                return this.codeIDField;
            }
            set
            {
                this.codeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodeRef", Order = 3)]
        public List<CodeRefType> CodeRef
        {
            get
            {
                return this.codeRefField;
            }
            set
            {
                this.codeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string LevelRef
        {
            get
            {
                return this.levelRefField;
            }
            set
            {
                this.levelRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string NodeAliasID
        {
            get
            {
                return this.nodeAliasIDField;
            }
            set
            {
                this.nodeAliasIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string ValidFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public string ValidTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class LevelType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private string orderField;

        private TextFormatType codingTypeField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        public LevelType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.codingTypeField = new TextFormatType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public TextFormatType CodingType
        {
            get
            {
                return this.codingTypeField;
            }
            set
            {
                this.codingTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class TextFormatType
    {

        private TextTypeType textTypeField;

        private bool textTypeFieldSpecified;

        private bool isSequenceField;

        private bool isSequenceFieldSpecified;

        private string minLengthField;

        private string maxLengthField;

        private double startValueField;

        private bool startValueFieldSpecified;

        private double endValueField;

        private bool endValueFieldSpecified;

        private double intervalField;

        private bool intervalFieldSpecified;

        private string timeIntervalField;

        private string decimalsField;

        private string patternField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TextTypeType textType
        {
            get
            {
                return this.textTypeField;
            }
            set
            {
                this.textTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool textTypeSpecified
        {
            get
            {
                return this.textTypeFieldSpecified;
            }
            set
            {
                this.textTypeFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isSequence
        {
            get
            {
                return this.isSequenceField;
            }
            set
            {
                this.isSequenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isSequenceSpecified
        {
            get
            {
                return this.isSequenceFieldSpecified;
            }
            set
            {
                this.isSequenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string minLength
        {
            get
            {
                return this.minLengthField;
            }
            set
            {
                this.minLengthField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string maxLength
        {
            get
            {
                return this.maxLengthField;
            }
            set
            {
                this.maxLengthField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double startValue
        {
            get
            {
                return this.startValueField;
            }
            set
            {
                this.startValueField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startValueSpecified
        {
            get
            {
                return this.startValueFieldSpecified;
            }
            set
            {
                this.startValueFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double endValue
        {
            get
            {
                return this.endValueField;
            }
            set
            {
                this.endValueField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endValueSpecified
        {
            get
            {
                return this.endValueFieldSpecified;
            }
            set
            {
                this.endValueFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double interval
        {
            get
            {
                return this.intervalField;
            }
            set
            {
                this.intervalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool intervalSpecified
        {
            get
            {
                return this.intervalFieldSpecified;
            }
            set
            {
                this.intervalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "duration")]
        public string timeInterval
        {
            get
            {
                return this.timeIntervalField;
            }
            set
            {
                this.timeIntervalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string decimals
        {
            get
            {
                return this.decimalsField;
            }
            set
            {
                this.decimalsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pattern
        {
            get
            {
                return this.patternField;
            }
            set
            {
                this.patternField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum TextTypeType
    {

        /// <remarks/>
        String,

        /// <remarks/>
        BigInteger,

        /// <remarks/>
        Integer,

        /// <remarks/>
        Long,

        /// <remarks/>
        Short,

        /// <remarks/>
        Decimal,

        /// <remarks/>
        Float,

        /// <remarks/>
        Double,

        /// <remarks/>
        Boolean,

        /// <remarks/>
        DateTime,

        /// <remarks/>
        Date,

        /// <remarks/>
        Time,

        /// <remarks/>
        Year,

        /// <remarks/>
        Month,

        /// <remarks/>
        Day,

        /// <remarks/>
        MonthDay,

        /// <remarks/>
        YearMonth,

        /// <remarks/>
        Duration,

        /// <remarks/>
        URI,

        /// <remarks/>
        Timespan,

        /// <remarks/>
        Count,

        /// <remarks/>
        InclusiveValueRange,

        /// <remarks/>
        ExclusiveValueRange,

        /// <remarks/>
        Incremental,

        /// <remarks/>
        ObservationalTimePeriod,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptsType
    {

        private List<ConceptType> conceptField;

        private List<ConceptSchemeType> conceptSchemeField;

        private List<AnnotationType> annotationsField;

        public ConceptsType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.conceptSchemeField = new List<ConceptSchemeType>();
            this.conceptField = new List<ConceptType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Concept", Order = 0)]
        public List<ConceptType> Concept
        {
            get
            {
                return this.conceptField;
            }
            set
            {
                this.conceptField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptScheme", Order = 1)]
        public List<ConceptSchemeType> ConceptScheme
        {
            get
            {
                return this.conceptSchemeField;
            }
            set
            {
                this.conceptSchemeField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string coreRepresentationField;

        private string coreRepresentationAgencyField;

        private string parentField;

        private string parentAgencyField;

        public ConceptType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string coreRepresentation
        {
            get
            {
                return this.coreRepresentationField;
            }
            set
            {
                this.coreRepresentationField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string coreRepresentationAgency
        {
            get
            {
                return this.coreRepresentationAgencyField;
            }
            set
            {
                this.coreRepresentationAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parent
        {
            get
            {
                return this.parentField;
            }
            set
            {
                this.parentField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string parentAgency
        {
            get
            {
                return this.parentAgencyField;
            }
            set
            {
                this.parentAgencyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptSchemeType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<ConceptType> conceptField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public ConceptSchemeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.conceptField = new List<ConceptType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Concept", Order = 2)]
        public List<ConceptType> Concept
        {
            get
            {
                return this.conceptField;
            }
            set
            {
                this.conceptField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataStructureDefinitionsType
    {

        private List<MetadataStructureDefinitionType> metadataStructureDefinitionField;

        public MetadataStructureDefinitionsType()
        {
            this.metadataStructureDefinitionField = new List<MetadataStructureDefinitionType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureDefinition", Order = 0)]
        public List<MetadataStructureDefinitionType> MetadataStructureDefinition
        {
            get
            {
                return this.metadataStructureDefinitionField;
            }
            set
            {
                this.metadataStructureDefinitionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataStructureDefinitionType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private TargetIdentifiersType targetIdentifiersField;

        private List<ReportStructureType> reportStructureField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public MetadataStructureDefinitionType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.reportStructureField = new List<ReportStructureType>();
            this.targetIdentifiersField = new TargetIdentifiersType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public TargetIdentifiersType TargetIdentifiers
        {
            get
            {
                return this.targetIdentifiersField;
            }
            set
            {
                this.targetIdentifiersField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportStructure", Order = 3)]
        public List<ReportStructureType> ReportStructure
        {
            get
            {
                return this.reportStructureField;
            }
            set
            {
                this.reportStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class TargetIdentifiersType
    {

        private FullTargetIdentifierType fullTargetIdentifierField;

        private List<PartialTargetIdentifierType> partialTargetIdentifierField;

        private List<AnnotationType> annotationsField;

        public TargetIdentifiersType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.partialTargetIdentifierField = new List<PartialTargetIdentifierType>();
            this.fullTargetIdentifierField = new FullTargetIdentifierType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public FullTargetIdentifierType FullTargetIdentifier
        {
            get
            {
                return this.fullTargetIdentifierField;
            }
            set
            {
                this.fullTargetIdentifierField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("PartialTargetIdentifier", Order = 1)]
        public List<PartialTargetIdentifierType> PartialTargetIdentifier
        {
            get
            {
                return this.partialTargetIdentifierField;
            }
            set
            {
                this.partialTargetIdentifierField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class FullTargetIdentifierType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<IdentifierComponentType> identifierComponentField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        private string uriField;

        public FullTargetIdentifierType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.identifierComponentField = new List<IdentifierComponentType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("IdentifierComponent", Order = 2)]
        public List<IdentifierComponentType> IdentifierComponent
        {
            get
            {
                return this.identifierComponentField;
            }
            set
            {
                this.identifierComponentField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class IdentifierComponentType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private ObjectIDType targetObjectClassField;

        private RepresentationSchemeType representationSchemeField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        private string uriField;

        public IdentifierComponentType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.representationSchemeField = new RepresentationSchemeType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ObjectIDType TargetObjectClass
        {
            get
            {
                return this.targetObjectClassField;
            }
            set
            {
                this.targetObjectClassField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public RepresentationSchemeType RepresentationScheme
        {
            get
            {
                return this.representationSchemeField;
            }
            set
            {
                this.representationSchemeField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum ObjectIDType
    {

        /// <remarks/>
        Agency,

        /// <remarks/>
        ConceptScheme,

        /// <remarks/>
        Concept,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Code,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        Component,

        /// <remarks/>
        KeyDescriptor,

        /// <remarks/>
        MeasureDescriptor,

        /// <remarks/>
        AttributeDescriptor,

        /// <remarks/>
        GroupKeyDescriptor,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        Measure,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        CategoryScheme,

        /// <remarks/>
        ReportingTaxonomy,

        /// <remarks/>
        Category,

        /// <remarks/>
        OrganisationScheme,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        FullTargetIdentifier,

        /// <remarks/>
        PartialTargetIdentifier,

        /// <remarks/>
        MetadataAttribute,

        /// <remarks/>
        DataFlow,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        MetadataFlow,

        /// <remarks/>
        ContentConstraint,

        /// <remarks/>
        AttachmentConstraint,

        /// <remarks/>
        DataSet,

        /// <remarks/>
        XSDataSet,

        /// <remarks/>
        MetadataSet,

        /// <remarks/>
        HierarchicalCodelist,

        /// <remarks/>
        Hierarchy,

        /// <remarks/>
        StructureSet,

        /// <remarks/>
        StructureMap,

        /// <remarks/>
        ComponentMap,

        /// <remarks/>
        CodelistMap,

        /// <remarks/>
        CodeMap,

        /// <remarks/>
        CategorySchemeMap,

        /// <remarks/>
        CategoryMap,

        /// <remarks/>
        OrganisationSchemeMap,

        /// <remarks/>
        OrganisationRoleMap,

        /// <remarks/>
        ConceptSchemeMap,

        /// <remarks/>
        ConceptMap,

        /// <remarks/>
        Process,

        /// <remarks/>
        ProcessStep,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class RepresentationSchemeType
    {

        private string representationSchemeField;

        private string representationSchemeAgencyField;

        private RepresentationSchemeTypeType representationSchemeTypeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string representationScheme
        {
            get
            {
                return this.representationSchemeField;
            }
            set
            {
                this.representationSchemeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string representationSchemeAgency
        {
            get
            {
                return this.representationSchemeAgencyField;
            }
            set
            {
                this.representationSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RepresentationSchemeTypeType representationSchemeType
        {
            get
            {
                return this.representationSchemeTypeField;
            }
            set
            {
                this.representationSchemeTypeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum RepresentationSchemeTypeType
    {

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        Category,

        /// <remarks/>
        Organisation,

        /// <remarks/>
        External,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class PartialTargetIdentifierType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<string> identifierComponentRefField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        private string uriField;

        public PartialTargetIdentifierType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.identifierComponentRefField = new List<string>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("IdentifierComponentRef", Order = 2)]
        public List<string> IdentifierComponentRef
        {
            get
            {
                return this.identifierComponentRefField;
            }
            set
            {
                this.identifierComponentRefField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ReportStructureType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<MetadataAttributeType> metadataAttributeField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string urnField;

        private string uriField;

        private string targetField;

        public ReportStructureType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.metadataAttributeField = new List<MetadataAttributeType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttribute", Order = 2)]
        public List<MetadataAttributeType> MetadataAttribute
        {
            get
            {
                return this.metadataAttributeField;
            }
            set
            {
                this.metadataAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string target
        {
            get
            {
                return this.targetField;
            }
            set
            {
                this.targetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class MetadataAttributeType
    {

        private List<MetadataAttributeType> metadataAttributeField;

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string representationSchemeField;

        private string representationSchemeAgencyField;

        private UsageStatusType usageStatusField;

        public MetadataAttributeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
            this.metadataAttributeField = new List<MetadataAttributeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttribute", Order = 0)]
        public List<MetadataAttributeType> MetadataAttribute
        {
            get
            {
                return this.metadataAttributeField;
            }
            set
            {
                this.metadataAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string representationScheme
        {
            get
            {
                return this.representationSchemeField;
            }
            set
            {
                this.representationSchemeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string representationSchemeAgency
        {
            get
            {
                return this.representationSchemeAgencyField;
            }
            set
            {
                this.representationSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public UsageStatusType usageStatus
        {
            get
            {
                return this.usageStatusField;
            }
            set
            {
                this.usageStatusField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum UsageStatusType
    {

        /// <remarks/>
        Mandatory,

        /// <remarks/>
        Conditional,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class KeyFamiliesType
    {

        private List<KeyFamilyType> keyFamilyField;

        public KeyFamiliesType()
        {
            this.keyFamilyField = new List<KeyFamilyType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", Order = 0)]
        public List<KeyFamilyType> KeyFamily
        {
            get
            {
                return this.keyFamilyField;
            }
            set
            {
                this.keyFamilyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class KeyFamilyType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private ComponentsType componentsField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string validFromField;

        private string validToField;

        public KeyFamilyType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.componentsField = new ComponentsType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ComponentsType Components
        {
            get
            {
                return this.componentsField;
            }
            set
            {
                this.componentsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ComponentsType
    {

        private List<DimensionType> dimensionField;

        private TimeDimensionType timeDimensionField;

        private List<GroupType> groupField;

        private PrimaryMeasureType primaryMeasureField;

        private List<CrossSectionalMeasureType> crossSectionalMeasureField;

        private List<AttributeType> attributeField;

        public ComponentsType()
        {
            this.attributeField = new List<AttributeType>();
            this.crossSectionalMeasureField = new List<CrossSectionalMeasureType>();
            this.primaryMeasureField = new PrimaryMeasureType();
            this.groupField = new List<GroupType>();
            this.timeDimensionField = new TimeDimensionType();
            this.dimensionField = new List<DimensionType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Dimension", Order = 0)]
        public List<DimensionType> Dimension
        {
            get
            {
                return this.dimensionField;
            }
            set
            {
                this.dimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TimeDimensionType TimeDimension
        {
            get
            {
                return this.timeDimensionField;
            }
            set
            {
                this.timeDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Group", Order = 2)]
        public List<GroupType> Group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public PrimaryMeasureType PrimaryMeasure
        {
            get
            {
                return this.primaryMeasureField;
            }
            set
            {
                this.primaryMeasureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CrossSectionalMeasure", Order = 4)]
        public List<CrossSectionalMeasureType> CrossSectionalMeasure
        {
            get
            {
                return this.crossSectionalMeasureField;
            }
            set
            {
                this.crossSectionalMeasureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", Order = 5)]
        public List<AttributeType> Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DimensionType
    {

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string codelistField;

        private string codelistVersionField;

        private string codelistAgencyField;

        private bool isMeasureDimensionField;

        private bool isFrequencyDimensionField;

        private bool isEntityDimensionField;

        private bool isCountDimensionField;

        private bool isNonObservationTimeDimensionField;

        private bool isIdentityDimensionField;

        private bool crossSectionalAttachDataSetField;

        private bool crossSectionalAttachDataSetFieldSpecified;

        private bool crossSectionalAttachGroupField;

        private bool crossSectionalAttachGroupFieldSpecified;

        private bool crossSectionalAttachSectionField;

        private bool crossSectionalAttachSectionFieldSpecified;

        private bool crossSectionalAttachObservationField;

        private bool crossSectionalAttachObservationFieldSpecified;

        public DimensionType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
            this.isMeasureDimensionField = false;
            this.isFrequencyDimensionField = false;
            this.isEntityDimensionField = false;
            this.isCountDimensionField = false;
            this.isNonObservationTimeDimensionField = false;
            this.isIdentityDimensionField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistVersion
        {
            get
            {
                return this.codelistVersionField;
            }
            set
            {
                this.codelistVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistAgency
        {
            get
            {
                return this.codelistAgencyField;
            }
            set
            {
                this.codelistAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isMeasureDimension
        {
            get
            {
                return this.isMeasureDimensionField;
            }
            set
            {
                this.isMeasureDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isFrequencyDimension
        {
            get
            {
                return this.isFrequencyDimensionField;
            }
            set
            {
                this.isFrequencyDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isEntityDimension
        {
            get
            {
                return this.isEntityDimensionField;
            }
            set
            {
                this.isEntityDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isCountDimension
        {
            get
            {
                return this.isCountDimensionField;
            }
            set
            {
                this.isCountDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isNonObservationTimeDimension
        {
            get
            {
                return this.isNonObservationTimeDimensionField;
            }
            set
            {
                this.isNonObservationTimeDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isIdentityDimension
        {
            get
            {
                return this.isIdentityDimensionField;
            }
            set
            {
                this.isIdentityDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachDataSet
        {
            get
            {
                return this.crossSectionalAttachDataSetField;
            }
            set
            {
                this.crossSectionalAttachDataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachDataSetSpecified
        {
            get
            {
                return this.crossSectionalAttachDataSetFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachDataSetFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachGroup
        {
            get
            {
                return this.crossSectionalAttachGroupField;
            }
            set
            {
                this.crossSectionalAttachGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachGroupSpecified
        {
            get
            {
                return this.crossSectionalAttachGroupFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachGroupFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachSection
        {
            get
            {
                return this.crossSectionalAttachSectionField;
            }
            set
            {
                this.crossSectionalAttachSectionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachSectionSpecified
        {
            get
            {
                return this.crossSectionalAttachSectionFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachSectionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachObservation
        {
            get
            {
                return this.crossSectionalAttachObservationField;
            }
            set
            {
                this.crossSectionalAttachObservationField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachObservationSpecified
        {
            get
            {
                return this.crossSectionalAttachObservationFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachObservationFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class TimeDimensionType
    {

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string codelistField;

        private string codelistVersionField;

        private string codelistAgencyField;

        private bool crossSectionalAttachDataSetField;

        private bool crossSectionalAttachDataSetFieldSpecified;

        private bool crossSectionalAttachGroupField;

        private bool crossSectionalAttachGroupFieldSpecified;

        private bool crossSectionalAttachSectionField;

        private bool crossSectionalAttachSectionFieldSpecified;

        private bool crossSectionalAttachObservationField;

        private bool crossSectionalAttachObservationFieldSpecified;

        public TimeDimensionType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistVersion
        {
            get
            {
                return this.codelistVersionField;
            }
            set
            {
                this.codelistVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistAgency
        {
            get
            {
                return this.codelistAgencyField;
            }
            set
            {
                this.codelistAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachDataSet
        {
            get
            {
                return this.crossSectionalAttachDataSetField;
            }
            set
            {
                this.crossSectionalAttachDataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachDataSetSpecified
        {
            get
            {
                return this.crossSectionalAttachDataSetFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachDataSetFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachGroup
        {
            get
            {
                return this.crossSectionalAttachGroupField;
            }
            set
            {
                this.crossSectionalAttachGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachGroupSpecified
        {
            get
            {
                return this.crossSectionalAttachGroupFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachGroupFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachSection
        {
            get
            {
                return this.crossSectionalAttachSectionField;
            }
            set
            {
                this.crossSectionalAttachSectionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachSectionSpecified
        {
            get
            {
                return this.crossSectionalAttachSectionFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachSectionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachObservation
        {
            get
            {
                return this.crossSectionalAttachObservationField;
            }
            set
            {
                this.crossSectionalAttachObservationField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachObservationSpecified
        {
            get
            {
                return this.crossSectionalAttachObservationFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachObservationFieldSpecified = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class GroupType
    {

        private string[] itemsField;

        private GroupChoiceType[] itemsElementNameField;

        private List<TextType> descriptionField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public GroupType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.descriptionField = new List<TextType>();
            //this.itemsElementNameField = new List<GroupChoiceType>();
            //this.itemsField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraintRef", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DimensionRef", typeof(string), Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public GroupChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 2)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IncludeInSchema = false)]
    public enum GroupChoiceType
    {

        /// <remarks/>
        AttachmentConstraintRef,

        /// <remarks/>
        DimensionRef,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class PrimaryMeasureType
    {

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string codelistField;

        private string codelistVersionField;

        private string codelistAgencyField;

        public PrimaryMeasureType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistVersion
        {
            get
            {
                return this.codelistVersionField;
            }
            set
            {
                this.codelistVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistAgency
        {
            get
            {
                return this.codelistAgencyField;
            }
            set
            {
                this.codelistAgencyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CrossSectionalMeasureType
    {

        private TextFormatType textFormatField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string codelistField;

        private string codelistVersionField;

        private string codelistAgencyField;

        private string measureDimensionField;

        private string codeField;

        public CrossSectionalMeasureType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.textFormatField = new TextFormatType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistVersion
        {
            get
            {
                return this.codelistVersionField;
            }
            set
            {
                this.codelistVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistAgency
        {
            get
            {
                return this.codelistAgencyField;
            }
            set
            {
                this.codelistAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string measureDimension
        {
            get
            {
                return this.measureDimensionField;
            }
            set
            {
                this.measureDimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class AttributeType
    {

        private TextFormatType textFormatField;

        private List<string> attachmentGroupField;

        private List<string> attachmentMeasureField;

        private List<AnnotationType> annotationsField;

        private string conceptRefField;

        private string conceptVersionField;

        private string conceptAgencyField;

        private string conceptSchemeRefField;

        private string conceptSchemeAgencyField;

        private string codelistField;

        private string codelistVersionField;

        private string codelistAgencyField;

        private AttachmentLevelType attachmentLevelField;

        private AssignmentStatusType assignmentStatusField;

        private bool isTimeFormatField;

        private bool crossSectionalAttachDataSetField;

        private bool crossSectionalAttachDataSetFieldSpecified;

        private bool crossSectionalAttachGroupField;

        private bool crossSectionalAttachGroupFieldSpecified;

        private bool crossSectionalAttachSectionField;

        private bool crossSectionalAttachSectionFieldSpecified;

        private bool crossSectionalAttachObservationField;

        private bool crossSectionalAttachObservationFieldSpecified;

        private bool isEntityAttributeField;

        private bool isNonObservationalTimeAttributeField;

        private bool isCountAttributeField;

        private bool isFrequencyAttributeField;

        private bool isIdentityAttributeField;

        public AttributeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.attachmentMeasureField = new List<string>();
            this.attachmentGroupField = new List<string>();
            this.textFormatField = new TextFormatType();
            this.isTimeFormatField = false;
            this.isEntityAttributeField = false;
            this.isNonObservationalTimeAttributeField = false;
            this.isCountAttributeField = false;
            this.isFrequencyAttributeField = false;
            this.isIdentityAttributeField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TextFormatType TextFormat
        {
            get
            {
                return this.textFormatField;
            }
            set
            {
                this.textFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentGroup", Order = 1)]
        public List<string> AttachmentGroup
        {
            get
            {
                return this.attachmentGroupField;
            }
            set
            {
                this.attachmentGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentMeasure", Order = 2)]
        public List<string> AttachmentMeasure
        {
            get
            {
                return this.attachmentMeasureField;
            }
            set
            {
                this.attachmentMeasureField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptRef
        {
            get
            {
                return this.conceptRefField;
            }
            set
            {
                this.conceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptVersion
        {
            get
            {
                return this.conceptVersionField;
            }
            set
            {
                this.conceptVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAgency
        {
            get
            {
                return this.conceptAgencyField;
            }
            set
            {
                this.conceptAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptSchemeAgency
        {
            get
            {
                return this.conceptSchemeAgencyField;
            }
            set
            {
                this.conceptSchemeAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistVersion
        {
            get
            {
                return this.codelistVersionField;
            }
            set
            {
                this.codelistVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codelistAgency
        {
            get
            {
                return this.codelistAgencyField;
            }
            set
            {
                this.codelistAgencyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AttachmentLevelType attachmentLevel
        {
            get
            {
                return this.attachmentLevelField;
            }
            set
            {
                this.attachmentLevelField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AssignmentStatusType assignmentStatus
        {
            get
            {
                return this.assignmentStatusField;
            }
            set
            {
                this.assignmentStatusField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isTimeFormat
        {
            get
            {
                return this.isTimeFormatField;
            }
            set
            {
                this.isTimeFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachDataSet
        {
            get
            {
                return this.crossSectionalAttachDataSetField;
            }
            set
            {
                this.crossSectionalAttachDataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachDataSetSpecified
        {
            get
            {
                return this.crossSectionalAttachDataSetFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachDataSetFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachGroup
        {
            get
            {
                return this.crossSectionalAttachGroupField;
            }
            set
            {
                this.crossSectionalAttachGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachGroupSpecified
        {
            get
            {
                return this.crossSectionalAttachGroupFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachGroupFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachSection
        {
            get
            {
                return this.crossSectionalAttachSectionField;
            }
            set
            {
                this.crossSectionalAttachSectionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachSectionSpecified
        {
            get
            {
                return this.crossSectionalAttachSectionFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachSectionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool crossSectionalAttachObservation
        {
            get
            {
                return this.crossSectionalAttachObservationField;
            }
            set
            {
                this.crossSectionalAttachObservationField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool crossSectionalAttachObservationSpecified
        {
            get
            {
                return this.crossSectionalAttachObservationFieldSpecified;
            }
            set
            {
                this.crossSectionalAttachObservationFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isEntityAttribute
        {
            get
            {
                return this.isEntityAttributeField;
            }
            set
            {
                this.isEntityAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isNonObservationalTimeAttribute
        {
            get
            {
                return this.isNonObservationalTimeAttributeField;
            }
            set
            {
                this.isNonObservationalTimeAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isCountAttribute
        {
            get
            {
                return this.isCountAttributeField;
            }
            set
            {
                this.isCountAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isFrequencyAttribute
        {
            get
            {
                return this.isFrequencyAttributeField;
            }
            set
            {
                this.isFrequencyAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isIdentityAttribute
        {
            get
            {
                return this.isIdentityAttributeField;
            }
            set
            {
                this.isIdentityAttributeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum AttachmentLevelType
    {

        /// <remarks/>
        DataSet,

        /// <remarks/>
        Group,

        /// <remarks/>
        Series,

        /// <remarks/>
        Observation,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum AssignmentStatusType
    {

        /// <remarks/>
        Mandatory,

        /// <remarks/>
        Conditional,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class AttachmentConstraintRefType
    {

        private object itemField;

        private string constraintRefField;

        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", typeof(DataProviderRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementRef", typeof(ProvisionAgreementRefType), Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ConstraintRef
        {
            get
            {
                return this.constraintRefField;
            }
            set
            {
                this.constraintRefField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class DataProviderRefType
    {

        private string uRNField;

        private string organisationSchemeAgencyIDField;

        private string organisationSchemeIDField;

        private string dataProviderIDField;

        private string versionField;

        private ConstraintType constraintField;

        public DataProviderRefType()
        {
            this.constraintField = new ConstraintType();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string OrganisationSchemeAgencyID
        {
            get
            {
                return this.organisationSchemeAgencyIDField;
            }
            set
            {
                this.organisationSchemeAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string OrganisationSchemeID
        {
            get
            {
                return this.organisationSchemeIDField;
            }
            set
            {
                this.organisationSchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string DataProviderID
        {
            get
            {
                return this.dataProviderIDField;
            }
            set
            {
                this.dataProviderIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ConstraintType Constraint
        {
            get
            {
                return this.constraintField;
            }
            set
            {
                this.constraintField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ProvisionAgreementRefType
    {

        private string uRNField;

        private string organisationSchemeAgencyIDField;

        private string organisationSchemeIDField;

        private string dataProviderIDField;

        private string dataProviderVersionField;

        private string dataflowAgencyIDField;

        private string dataflowIDField;

        private string dataflowVersionField;

        private ConstraintType constraintField;

        public ProvisionAgreementRefType()
        {
            this.constraintField = new ConstraintType();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string OrganisationSchemeAgencyID
        {
            get
            {
                return this.organisationSchemeAgencyIDField;
            }
            set
            {
                this.organisationSchemeAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string OrganisationSchemeID
        {
            get
            {
                return this.organisationSchemeIDField;
            }
            set
            {
                this.organisationSchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string DataProviderID
        {
            get
            {
                return this.dataProviderIDField;
            }
            set
            {
                this.dataProviderIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string DataProviderVersion
        {
            get
            {
                return this.dataProviderVersionField;
            }
            set
            {
                this.dataProviderVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string DataflowAgencyID
        {
            get
            {
                return this.dataflowAgencyIDField;
            }
            set
            {
                this.dataflowAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string DataflowID
        {
            get
            {
                return this.dataflowIDField;
            }
            set
            {
                this.dataflowIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string DataflowVersion
        {
            get
            {
                return this.dataflowVersionField;
            }
            set
            {
                this.dataflowVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ConstraintType Constraint
        {
            get
            {
                return this.constraintField;
            }
            set
            {
                this.constraintField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class StructureSetsType
    {

        private List<StructureSetType> structureSetField;

        public StructureSetsType()
        {
            this.structureSetField = new List<StructureSetType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("StructureSet", Order = 0)]
        public List<StructureSetType> StructureSet
        {
            get
            {
                return this.structureSetField;
            }
            set
            {
                this.structureSetField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class StructureSetType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private RelatedStructuresType relatedStructuresField;

        private StructureMapType structureMapField;

        private CodelistMapType codelistMapField;

        private CategorySchemeMapType categorySchemeMapField;

        private ConceptSchemeMapType conceptSchemeMapField;

        private OrganisationSchemeMapType organisationSchemeMapField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string agencyIDField;

        private string versionField;

        private string urnField;

        private string uriField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string validFromField;

        private string validToField;

        public StructureSetType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.organisationSchemeMapField = new OrganisationSchemeMapType();
            this.conceptSchemeMapField = new ConceptSchemeMapType();
            this.categorySchemeMapField = new CategorySchemeMapType();
            this.codelistMapField = new CodelistMapType();
            this.structureMapField = new StructureMapType();
            this.relatedStructuresField = new RelatedStructuresType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public RelatedStructuresType RelatedStructures
        {
            get
            {
                return this.relatedStructuresField;
            }
            set
            {
                this.relatedStructuresField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public StructureMapType StructureMap
        {
            get
            {
                return this.structureMapField;
            }
            set
            {
                this.structureMapField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public CodelistMapType CodelistMap
        {
            get
            {
                return this.codelistMapField;
            }
            set
            {
                this.codelistMapField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public CategorySchemeMapType CategorySchemeMap
        {
            get
            {
                return this.categorySchemeMapField;
            }
            set
            {
                this.categorySchemeMapField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public ConceptSchemeMapType ConceptSchemeMap
        {
            get
            {
                return this.conceptSchemeMapField;
            }
            set
            {
                this.conceptSchemeMapField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public OrganisationSchemeMapType OrganisationSchemeMap
        {
            get
            {
                return this.organisationSchemeMapField;
            }
            set
            {
                this.organisationSchemeMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class RelatedStructuresType
    {

        private List<KeyFamilyRefType> keyFamilyRefField;

        private List<MetadataStructureRefType> metadataStructureRefField;

        private List<ConceptSchemeRefType> conceptSchemeRefField;

        private List<CategorySchemeRefType> categorySchemeRefField;

        private List<OrganisationSchemeRefType> organisationSchemeRefField;

        private List<HierarchicalCodelistRefType> hierarchicalCodelistRefField;

        public RelatedStructuresType()
        {
            this.hierarchicalCodelistRefField = new List<HierarchicalCodelistRefType>();
            this.organisationSchemeRefField = new List<OrganisationSchemeRefType>();
            this.categorySchemeRefField = new List<CategorySchemeRefType>();
            this.conceptSchemeRefField = new List<ConceptSchemeRefType>();
            this.metadataStructureRefField = new List<MetadataStructureRefType>();
            this.keyFamilyRefField = new List<KeyFamilyRefType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyRef", Order = 0)]
        public List<KeyFamilyRefType> KeyFamilyRef
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureRef", Order = 1)]
        public List<MetadataStructureRefType> MetadataStructureRef
        {
            get
            {
                return this.metadataStructureRefField;
            }
            set
            {
                this.metadataStructureRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeRef", Order = 2)]
        public List<ConceptSchemeRefType> ConceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeRef", Order = 3)]
        public List<CategorySchemeRefType> CategorySchemeRef
        {
            get
            {
                return this.categorySchemeRefField;
            }
            set
            {
                this.categorySchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeRef", Order = 4)]
        public List<OrganisationSchemeRefType> OrganisationSchemeRef
        {
            get
            {
                return this.organisationSchemeRefField;
            }
            set
            {
                this.organisationSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistRef", Order = 5)]
        public List<HierarchicalCodelistRefType> HierarchicalCodelistRef
        {
            get
            {
                return this.hierarchicalCodelistRefField;
            }
            set
            {
                this.hierarchicalCodelistRefField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptSchemeRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string conceptSchemeIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string ConceptSchemeID
        {
            get
            {
                return this.conceptSchemeIDField;
            }
            set
            {
                this.conceptSchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategorySchemeRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string categorySchemeIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string CategorySchemeID
        {
            get
            {
                return this.categorySchemeIDField;
            }
            set
            {
                this.categorySchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationSchemeRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string organisationSchemeIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string OrganisationSchemeID
        {
            get
            {
                return this.organisationSchemeIDField;
            }
            set
            {
                this.organisationSchemeIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class HierarchicalCodelistRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string hierarchicalCodelistIDField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string URN
        {
            get
            {
                return this.uRNField;
            }
            set
            {
                this.uRNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string HierarchicalCodelistID
        {
            get
            {
                return this.hierarchicalCodelistIDField;
            }
            set
            {
                this.hierarchicalCodelistIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class StructureMapType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private object itemField;

        private object item1Field;

        private List<ComponentMapType> componentMapField;

        private List<AnnotationType> annotationsField;

        private bool isExtensionField;

        private bool isExtensionFieldSpecified;

        private string idField;

        public StructureMapType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.componentMapField = new List<ComponentMapType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyRef", typeof(KeyFamilyRefType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureRef", typeof(MetadataStructureRefType), Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("TargetKeyFamilyRef", typeof(KeyFamilyRefType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("TargetMetadataStructureRef", typeof(MetadataStructureRefType), Order = 3)]
        public object Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ComponentMap", Order = 4)]
        public List<ComponentMapType> ComponentMap
        {
            get
            {
                return this.componentMapField;
            }
            set
            {
                this.componentMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExtension
        {
            get
            {
                return this.isExtensionField;
            }
            set
            {
                this.isExtensionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExtensionSpecified
        {
            get
            {
                return this.isExtensionFieldSpecified;
            }
            set
            {
                this.isExtensionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ComponentMapType
    {

        private string mapConceptRefField;

        private string mapTargetConceptRefField;

        private List<object> itemsField;

        private string componentAliasField;

        private string preferredLanguageField;

        public ComponentMapType()
        {
            this.itemsField = new List<object>();
            this.preferredLanguageField = "en";
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MapConceptRef
        {
            get
            {
                return this.mapConceptRefField;
            }
            set
            {
                this.mapConceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string MapTargetConceptRef
        {
            get
            {
                return this.mapTargetConceptRefField;
            }
            set
            {
                this.mapTargetConceptRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("RepresentationMapRef", typeof(RepresentationMapRefType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("ToTextFormat", typeof(TextFormatType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("ToValueType", typeof(ToValueTypeType), Order = 2)]
        public List<object> Items
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string componentAlias
        {
            get
            {
                return this.componentAliasField;
            }
            set
            {
                this.componentAliasField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "language")]
        [System.ComponentModel.DefaultValueAttribute("en")]
        public string preferredLanguage
        {
            get
            {
                return this.preferredLanguageField;
            }
            set
            {
                this.preferredLanguageField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class RepresentationMapRefType
    {

        private string representationMapAgencyIDField;

        private string representationMapIDField;

        private RepresentationTypeType representationTypeField;

        public RepresentationMapRefType()
        {
            this.representationTypeField = RepresentationTypeType.Codelist;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string RepresentationMapAgencyID
        {
            get
            {
                return this.representationMapAgencyIDField;
            }
            set
            {
                this.representationMapAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string RepresentationMapID
        {
            get
            {
                return this.representationMapIDField;
            }
            set
            {
                this.representationMapIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(RepresentationTypeType.Codelist)]
        public RepresentationTypeType representationType
        {
            get
            {
                return this.representationTypeField;
            }
            set
            {
                this.representationTypeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum RepresentationTypeType
    {

        /// <remarks/>
        Codelist,

        /// <remarks/>
        CategoryScheme,

        /// <remarks/>
        OrganisationScheme,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    public enum ToValueTypeType
    {

        /// <remarks/>
        Value,

        /// <remarks/>
        Name,

        /// <remarks/>
        Description,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodelistMapType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private object itemField;

        private object item1Field;

        private List<CodeMapType> codeMapField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public CodelistMapType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.codeMapField = new List<CodeMapType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodelistRef", typeof(CodelistRefType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistRef", typeof(HierarchicalCodelistRefType), Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("TargetCodelistRef", typeof(CodelistRefType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("TargetHierarchicalCodelistRef", typeof(HierarchicalCodelistRefType), Order = 3)]
        public object Item1
        {
            get
            {
                return this.item1Field;
            }
            set
            {
                this.item1Field = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodeMap", Order = 4)]
        public List<CodeMapType> CodeMap
        {
            get
            {
                return this.codeMapField;
            }
            set
            {
                this.codeMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CodeMapType
    {

        private string mapCodeRefField;

        private string mapTargetCodeRefField;

        private string codeAliasField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MapCodeRef
        {
            get
            {
                return this.mapCodeRefField;
            }
            set
            {
                this.mapCodeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string MapTargetCodeRef
        {
            get
            {
                return this.mapTargetCodeRefField;
            }
            set
            {
                this.mapTargetCodeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CodeAlias
        {
            get
            {
                return this.codeAliasField;
            }
            set
            {
                this.codeAliasField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategorySchemeMapType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private CategorySchemeRefType categorySchemeRefField;

        private CategorySchemeRefType targetCategorySchemeRefField;

        private List<CategoryMapType> categoryMapField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public CategorySchemeMapType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryMapField = new List<CategoryMapType>();
            this.targetCategorySchemeRefField = new CategorySchemeRefType();
            this.categorySchemeRefField = new CategorySchemeRefType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public CategorySchemeRefType CategorySchemeRef
        {
            get
            {
                return this.categorySchemeRefField;
            }
            set
            {
                this.categorySchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public CategorySchemeRefType TargetCategorySchemeRef
        {
            get
            {
                return this.targetCategorySchemeRefField;
            }
            set
            {
                this.targetCategorySchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryMap", Order = 4)]
        public List<CategoryMapType> CategoryMap
        {
            get
            {
                return this.categoryMapField;
            }
            set
            {
                this.categoryMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class CategoryMapType
    {

        private CategoryIDType categoryIDField;

        private CategoryIDType targetCategoryIDField;

        private string categoryAliasField;

        public CategoryMapType()
        {
            this.targetCategoryIDField = new CategoryIDType();
            this.categoryIDField = new CategoryIDType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CategoryIDType CategoryID
        {
            get
            {
                return this.categoryIDField;
            }
            set
            {
                this.categoryIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public CategoryIDType TargetCategoryID
        {
            get
            {
                return this.targetCategoryIDField;
            }
            set
            {
                this.targetCategoryIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string categoryAlias
        {
            get
            {
                return this.categoryAliasField;
            }
            set
            {
                this.categoryAliasField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptSchemeMapType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private ConceptSchemeRefType conceptSchemeRefField;

        private ConceptSchemeRefType targetConceptSchemeRefField;

        private List<ConceptMapType> conceptMapField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public ConceptSchemeMapType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.conceptMapField = new List<ConceptMapType>();
            this.targetConceptSchemeRefField = new ConceptSchemeRefType();
            this.conceptSchemeRefField = new ConceptSchemeRefType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ConceptSchemeRefType ConceptSchemeRef
        {
            get
            {
                return this.conceptSchemeRefField;
            }
            set
            {
                this.conceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public ConceptSchemeRefType TargetConceptSchemeRef
        {
            get
            {
                return this.targetConceptSchemeRefField;
            }
            set
            {
                this.targetConceptSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptMap", Order = 4)]
        public List<ConceptMapType> ConceptMap
        {
            get
            {
                return this.conceptMapField;
            }
            set
            {
                this.conceptMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ConceptMapType
    {

        private string conceptIDField;

        private string targetConceptIDField;

        private string conceptAliasField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ConceptID
        {
            get
            {
                return this.conceptIDField;
            }
            set
            {
                this.conceptIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string TargetConceptID
        {
            get
            {
                return this.targetConceptIDField;
            }
            set
            {
                this.targetConceptIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string conceptAlias
        {
            get
            {
                return this.conceptAliasField;
            }
            set
            {
                this.conceptAliasField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationSchemeMapType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private OrganisationSchemeRefType organisationSchemeRefField;

        private OrganisationSchemeRefType targetOrganisationSchemeRefField;

        private List<OrganisationMapType> organisationMapField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public OrganisationSchemeMapType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.organisationMapField = new List<OrganisationMapType>();
            this.targetOrganisationSchemeRefField = new OrganisationSchemeRefType();
            this.organisationSchemeRefField = new OrganisationSchemeRefType();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public OrganisationSchemeRefType OrganisationSchemeRef
        {
            get
            {
                return this.organisationSchemeRefField;
            }
            set
            {
                this.organisationSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public OrganisationSchemeRefType TargetOrganisationSchemeRef
        {
            get
            {
                return this.targetOrganisationSchemeRefField;
            }
            set
            {
                this.targetOrganisationSchemeRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("OrganisationMap", Order = 4)]
        public List<OrganisationMapType> OrganisationMap
        {
            get
            {
                return this.organisationMapField;
            }
            set
            {
                this.organisationMapField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class OrganisationMapType
    {

        private string organisationIDField;

        private string targetOrganisationIDField;

        private string organisationAliasField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string OrganisationID
        {
            get
            {
                return this.organisationIDField;
            }
            set
            {
                this.organisationIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string TargetOrganisationID
        {
            get
            {
                return this.targetOrganisationIDField;
            }
            set
            {
                this.targetOrganisationIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organisationAlias
        {
            get
            {
                return this.organisationAliasField;
            }
            set
            {
                this.organisationAliasField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ReportingTaxonomiesType
    {

        private List<ReportingTaxonomyType> reportingTaxonomyField;

        public ReportingTaxonomiesType()
        {
            this.reportingTaxonomyField = new List<ReportingTaxonomyType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomy", Order = 0)]
        public List<ReportingTaxonomyType> ReportingTaxonomy
        {
            get
            {
                return this.reportingTaxonomyField;
            }
            set
            {
                this.reportingTaxonomyField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ReportingTaxonomyType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<DataflowRefType> dataflowRefField;

        private List<MetadataflowRefType> metadataflowRefField;

        private List<CategoryType> categoryField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string agencyIDField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public ReportingTaxonomyType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.categoryField = new List<CategoryType>();
            this.metadataflowRefField = new List<MetadataflowRefType>();
            this.dataflowRefField = new List<DataflowRefType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", Order = 2)]
        public List<DataflowRefType> DataflowRef
        {
            get
            {
                return this.dataflowRefField;
            }
            set
            {
                this.dataflowRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", Order = 3)]
        public List<MetadataflowRefType> MetadataflowRef
        {
            get
            {
                return this.metadataflowRefField;
            }
            set
            {
                this.metadataflowRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 4)]
        public List<CategoryType> Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ProcessesType
    {

        private List<ProcessType> processField;

        public ProcessesType()
        {
            this.processField = new List<ProcessType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Process", Order = 0)]
        public List<ProcessType> Process
        {
            get
            {
                return this.processField;
            }
            set
            {
                this.processField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ProcessType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<ProcessStepType> processStepField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string versionField;

        private string uriField;

        private string urnField;

        private bool isExternalReferenceField;

        private bool isExternalReferenceFieldSpecified;

        private string agencyIDField;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        private string validFromField;

        private string validToField;

        public ProcessType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.processStepField = new List<ProcessStepType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessStep", Order = 2)]
        public List<ProcessStepType> ProcessStep
        {
            get
            {
                return this.processStepField;
            }
            set
            {
                this.processStepField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string uri
        {
            get
            {
                return this.uriField;
            }
            set
            {
                this.uriField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string urn
        {
            get
            {
                return this.urnField;
            }
            set
            {
                this.urnField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isExternalReference
        {
            get
            {
                return this.isExternalReferenceField;
            }
            set
            {
                this.isExternalReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isExternalReferenceSpecified
        {
            get
            {
                return this.isExternalReferenceFieldSpecified;
            }
            set
            {
                this.isExternalReferenceFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isFinal
        {
            get
            {
                return this.isFinalField;
            }
            set
            {
                this.isFinalField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFinalSpecified
        {
            get
            {
                return this.isFinalFieldSpecified;
            }
            set
            {
                this.isFinalFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFrom
        {
            get
            {
                return this.validFromField;
            }
            set
            {
                this.validFromField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validTo
        {
            get
            {
                return this.validToField;
            }
            set
            {
                this.validToField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class ProcessStepType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private List<ObjectIDType> inputField;

        private List<ObjectIDType> outputField;

        private List<TextType> computationField;

        private List<TransitionType> transitionField;

        private List<ProcessStepType> processStepField;

        private List<AnnotationType> annotationsField;

        private string idField;

        public ProcessStepType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.processStepField = new List<ProcessStepType>();
            this.transitionField = new List<TransitionType>();
            this.computationField = new List<TextType>();
            this.outputField = new List<ObjectIDType>();
            this.inputField = new List<ObjectIDType>();
            this.descriptionField = new List<TextType>();
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Order = 1)]
        public List<TextType> Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Input", Order = 2)]
        public List<ObjectIDType> Input
        {
            get
            {
                return this.inputField;
            }
            set
            {
                this.inputField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Output", Order = 3)]
        public List<ObjectIDType> Output
        {
            get
            {
                return this.outputField;
            }
            set
            {
                this.outputField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Computation", Order = 4)]
        public List<TextType> Computation
        {
            get
            {
                return this.computationField;
            }
            set
            {
                this.computationField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Transition", Order = 5)]
        public List<TransitionType> Transition
        {
            get
            {
                return this.transitionField;
            }
            set
            {
                this.transitionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessStep", Order = 6)]
        public List<ProcessStepType> ProcessStep
        {
            get
            {
                return this.processStepField;
            }
            set
            {
                this.processStepField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = false)]
        public List<AnnotationType> Annotations
        {
            get
            {
                return this.annotationsField;
            }
            set
            {
                this.annotationsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = true)]
    public class TransitionType
    {

        private string targetStepField;

        private TextType conditionField;

        public TransitionType()
        {
            this.conditionField = new TextType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string TargetStep
        {
            get
            {
                return this.targetStepField;
            }
            set
            {
                this.targetStepField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TextType Condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }
    }

}
