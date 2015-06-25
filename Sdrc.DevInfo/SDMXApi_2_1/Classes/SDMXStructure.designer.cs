using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;

namespace SDMXObjectModel.Structure
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Structures", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class StructuresType
    {
        #region "Variables"

        #region "Private"

        private List<OrganisationSchemeType> organisationSchemesField;

        private List<DataflowType> dataflowsField;

        private List<MetadataflowType> metadataflowsField;

        private List<CategorySchemeType> categorySchemesField;

        private List<CategorisationType> categorisationsField;

        private List<CodelistType> codelistsField;

        private List<HierarchicalCodelistType> hierarchicalCodelistsField;

        private List<ConceptSchemeType> conceptsField;

        private List<MetadataStructureType> metadataStructuresField;

        private List<DataStructureType> dataStructuresField;

        private List<StructureSetType> structureSetsField;

        private List<ReportingTaxonomyType> reportingTaxonomiesField;

        private List<ProcessType> processesField;

        private List<ConstraintType> constraintsField;

        private List<ProvisionAgreementType> provisionAgreementsField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("AgencyScheme", typeof(AgencySchemeType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataConsumerScheme", typeof(DataConsumerSchemeType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataProviderScheme", typeof(DataProviderSchemeType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OrganisationUnitScheme", typeof(OrganisationUnitSchemeType), IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Dataflow", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Metadataflow", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryScheme", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Categorisation", IsNullable = false)]
        public List<CategorisationType> Categorisations
        {
            get
            {
                return this.categorisationsField;
            }
            set
            {
                this.categorisationsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Codelist", IsNullable = false)]
        public List<CodelistType> Codelists
        {
            get
            {
                return this.codelistsField;
            }
            set
            {
                this.codelistsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("HierarchicalCodelist", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ConceptScheme", IsNullable = false)]
        public List<ConceptSchemeType> Concepts
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataStructure", IsNullable = false)]
        public List<MetadataStructureType> MetadataStructures
        {
            get
            {
                return this.metadataStructuresField;
            }
            set
            {
                this.metadataStructuresField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataStructure", IsNullable = false)]
        public List<DataStructureType> DataStructures
        {
            get
            {
                return this.dataStructuresField;
            }
            set
            {
                this.dataStructuresField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructureSet", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReportingTaxonomy", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 12)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Process", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 13)]
        [System.Xml.Serialization.XmlArrayItemAttribute("AttachmentConstraint", typeof(AttachmentConstraintType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ContentConstraint", typeof(ContentConstraintType), IsNullable = false)]
        public List<ConstraintType> Constraints
        {
            get
            {
                return this.constraintsField;
            }
            set
            {
                this.constraintsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 14)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ProvisionAgreement", IsNullable = false)]
        public List<ProvisionAgreementType> ProvisionAgreements
        {
            get
            {
                return this.provisionAgreementsField;
            }
            set
            {
                this.provisionAgreementsField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public StructuresType():this(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null)
        {
        }

        public StructuresType(ProvisionAgreementType provisionAgreement, ConstraintType constraint, ProcessType process, 
                              ReportingTaxonomyType reportingTaxonomoy, StructureSetType structureSet, DataStructureType dataStructure, 
                              MetadataStructureType metadataStructure, ConceptSchemeType conceptScheme, 
                              HierarchicalCodelistType hierarchicalCodelist, CodelistType codelist, CategorisationType categorisation, 
                              CategorySchemeType categoryScheme, MetadataflowType metadataFlow, DataflowType dataFlow, 
                              OrganisationSchemeType organisationScheme)
        {
            if (provisionAgreement != null)
            {
                this.provisionAgreementsField = new List<ProvisionAgreementType>();
                this.provisionAgreementsField.Add(provisionAgreement);
            }

            if (constraint != null)
            {
                this.constraintsField = new List<ConstraintType>();
                this.constraintsField.Add(constraint);
            }

            if (process != null)
            {
                this.processesField = new List<ProcessType>();
                this.processesField.Add(process);
            }

            if (reportingTaxonomoy != null)
            {
                this.reportingTaxonomiesField = new List<ReportingTaxonomyType>();
                this.reportingTaxonomiesField.Add(reportingTaxonomoy);
            }

            if (structureSet != null)
            {
                this.structureSetsField = new List<StructureSetType>();
                this.structureSetsField.Add(structureSet);
            }

            if (dataStructure != null)
            {
                this.dataStructuresField = new List<DataStructureType>();
                this.dataStructuresField.Add(dataStructure);
            }

            if (metadataStructure != null)
            {
                this.metadataStructuresField = new List<MetadataStructureType>();
                this.metadataStructuresField.Add(metadataStructure);
            }

            if (conceptScheme != null)
            {
                this.conceptsField = new List<ConceptSchemeType>();
                this.conceptsField.Add(conceptScheme);
            }

            if (hierarchicalCodelist != null)
            {
                this.hierarchicalCodelistsField = new List<HierarchicalCodelistType>();
                this.hierarchicalCodelistsField.Add(hierarchicalCodelist);
            }

            if (codelist != null)
            {
                this.codelistsField = new List<CodelistType>();
                this.codelistsField.Add(codelist);
            }

            if (categorisation != null)
            {
                this.categorisationsField = new List<CategorisationType>();
                this.categorisationsField.Add(categorisation);
            }

            if (categoryScheme != null)
            {
                this.categorySchemesField = new List<CategorySchemeType>();
                this.categorySchemesField.Add(categoryScheme);
            }

            if (metadataFlow != null)
            {
                this.metadataflowsField = new List<MetadataflowType>();
                this.metadataflowsField.Add(metadataFlow);
            }

            if (dataFlow != null)
            {
                this.dataflowsField = new List<DataflowType>();
                this.dataflowsField.Add(dataFlow);
            }

            if (organisationScheme != null)
            {
                this.organisationSchemesField = new List<OrganisationSchemeType>();
                this.organisationSchemesField.Add(organisationScheme);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class AgencySchemeType : OrganisationSchemeType
    {
        #region "Constructors"

        public AgencySchemeType():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public AgencySchemeType(string id, string agencyId, string version, string name, string description, string language, OrganisationType organisation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (organisation != null)
            {
                this.Organisation = new List<OrganisationType>();
                this.Organisation.Add(organisation);
            }
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class OrganisationSchemeType : OrganisationSchemeBaseType
    {
        #region "Variables"

        #region "Private"

        private List<OrganisationType> itemsField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Agency", typeof(AgencyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataConsumer", typeof(DataConsumerType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationUnit", typeof(OrganisationUnitType), Order = 0)]
        public List<OrganisationType> Organisation
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public OrganisationSchemeType()
        {
            this.itemsField = new List<OrganisationType>();
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Agency", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class AgencyType : OrganisationType
    {
        #region "Constructors"

        public AgencyType():this(string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public AgencyType(string id, string name, string description, string language, ContactType contact)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (contact != null)
            {
                this.Contact = new List<ContactType>();
                this.Contact.Add(contact);
            }
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Organisation", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class OrganisationType : BaseOrganisationType
    {
        #region "Variables"

        #region "Private"

        private List<ContactType> contactField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Contact", Order = 0)]
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public OrganisationType()
        {
            this.contactField = new List<ContactType>();
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ContactType
    {
        #region "Variables"

        #region "Private"

        private List<TextType> nameField;

        private List<TextType> departmentField;

        private List<TextType> roleField;

        private string[] itemsField;

        private ContactChoiceType[] itemsElementNameField;

        private string idField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public ContactType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public ContactType(string id, string name, string department, string role, string language)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.nameField = new List<TextType>();
                this.nameField.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(department))
            {
                this.departmentField = new List<TextType>();
                this.departmentField.Add(new TextType(language, department));
            }

            if (!string.IsNullOrEmpty(role))
            {
                this.roleField = new List<TextType>();
                this.roleField.Add(new TextType(language, role));
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ValueMapType
    {

        private List<ValueMappingType> valueMappingField;

        public ValueMapType()
        {
            this.valueMappingField = new List<ValueMappingType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ValueMapping", Order = 0)]
        public List<ValueMappingType> ValueMapping
        {
            get
            {
                return this.valueMappingField;
            }
            set
            {
                this.valueMappingField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ValueMappingType
    {

        private string sourceField;

        private string targetField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class RepresentationMapType
    {

        private List<object> itemsField;

        public RepresentationMapType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("CodelistMap", typeof(LocalCodelistMapReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ToTextFormat", typeof(TextFormatType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ToValueType", typeof(ToValueTypeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ValueMap", typeof(ValueMapType), Order = 0)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class IncludedCodelistReferenceType : CodelistReferenceType
    {

        private string aliasField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string alias
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BasicComponentTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleComponentTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodingTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NonFacetedTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodededTextFormatType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class TextFormatType
    {

        private DataType textTypeField;

        private bool isSequenceField;

        private bool isSequenceFieldSpecified;

        private decimal intervalField;

        private bool intervalFieldSpecified;

        private decimal startValueField;

        private bool startValueFieldSpecified;

        private decimal endValueField;

        private bool endValueFieldSpecified;

        private string timeIntervalField;

        private string startTimeField;

        private string endTimeField;

        private string minLengthField;

        private string maxLengthField;

        private decimal minValueField;

        private bool minValueFieldSpecified;

        private decimal maxValueField;

        private bool maxValueFieldSpecified;

        private string decimalsField;

        private string patternField;

        private bool isMultiLingualField;

        public TextFormatType()
        {
            this.textTypeField = DataType.String;
            this.isMultiLingualField = true;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        //[System.ComponentModel.DefaultValueAttribute(DataType.StringValue)]
        public DataType textType
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal interval
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal startValue
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
        public decimal endValue
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
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
        public decimal minValue
        {
            get
            {
                return this.minValueField;
            }
            set
            {
                this.minValueField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minValueSpecified
        {
            get
            {
                return this.minValueFieldSpecified;
            }
            set
            {
                this.minValueFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal maxValue
        {
            get
            {
                return this.maxValueField;
            }
            set
            {
                this.maxValueField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxValueSpecified
        {
            get
            {
                return this.maxValueFieldSpecified;
            }
            set
            {
                this.maxValueFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "positiveInteger")]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool isMultiLingual
        {
            get
            {
                return this.isMultiLingualField;
            }
            set
            {
                this.isMultiLingualField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTextFormatType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class TargetObjectTextFormatType : TextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class IdentifiableObjectTextFormatType : TargetObjectTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConstraintTextFormatType : TargetObjectTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataSetTextFormatType : TargetObjectTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class KeyDescriptorValuesTextFormatType : TargetObjectTextFormatType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleComponentTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodingTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NonFacetedTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodededTextFormatType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class BasicComponentTextFormatType : TextFormatType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodingTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NonFacetedTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayTextFormatType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodededTextFormatType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class SimpleComponentTextFormatType : BasicComponentTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CodingTextFormatType : SimpleComponentTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class TimeTextFormatType : SimpleComponentTextFormatType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayTextFormatType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class NonFacetedTextFormatType : SimpleComponentTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReportingYearStartDayTextFormatType : NonFacetedTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CodededTextFormatType : SimpleComponentTextFormatType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    public enum ToValueTypeType
    {

        /// <remarks/>
        Value,

        /// <remarks/>
        Name,

        /// <remarks/>
        Description,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReleaseCalendarType
    {

        private string periodicityField;

        private string offsetField;

        private string toleranceField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Periodicity
        {
            get
            {
                return this.periodicityField;
            }
            set
            {
                this.periodicityField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Tolerance
        {
            get
            {
                return this.toleranceField;
            }
            set
            {
                this.toleranceField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataKeySetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataKeySetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class KeySetType
    {

        private List<DistinctKeyType> keyField;

        private bool isIncludedField;

        public KeySetType()
        {
            this.keyField = new List<DistinctKeyType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Key", Order = 0)]
        public List<DistinctKeyType> Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataKeySetType : KeySetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataKeySetType : KeySetType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintAttachmentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintAttachmentType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ConstraintAttachmentType
    {

        private object[] itemsField;

        private ConstraintAttachmentChoiceType[] itemsElementNameField;

        public ConstraintAttachmentType()
        {
            //this.itemsElementNameField = new List<ConstraintAttachmentChoiceType>();
            //this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataStructure", typeof(DataStructureReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dataflow", typeof(DataflowReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(MetadataStructureReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", typeof(MetadataflowReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("QueryableDataSource", typeof(QueryableDataSourceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SimpleDataSource", typeof(string), DataType = "anyURI", Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
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
        public ConstraintAttachmentChoiceType[] ItemsElementName
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IncludeInSchema = false)]
    public enum ConstraintAttachmentChoiceType
    {

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        DataSet,

        /// <remarks/>
        DataStructure,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        MetadataSet,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        Metadataflow,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        QueryableDataSource,

        /// <remarks/>
        SimpleDataSource,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ContentConstraintAttachmentType : ConstraintAttachmentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class AttachmentConstraintAttachmentType : ConstraintAttachmentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class AttributeRelationshipType
    {

        private  object[] itemsField;

        private AttributeRelationshipChoiceType[] itemsElementNameField;

        public AttributeRelationshipType()
        {
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentGroup", typeof(LocalGroupKeyDescriptorReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(LocalDimensionReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Group", typeof(LocalGroupKeyDescriptorReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("None", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PrimaryMeasure", typeof(LocalPrimaryMeasureReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
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
        public AttributeRelationshipChoiceType[] ItemsElementName
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IncludeInSchema = false)]
    public enum AttributeRelationshipChoiceType
    {

        /// <remarks/>
        AttachmentGroup,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        Group,

        /// <remarks/>
        None,

        /// <remarks/>
        PrimaryMeasure,
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureComponentsBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureComponentsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentsBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Grouping", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class GroupingType
    {

        private List<ComponentListType> itemsField;

        public GroupingType()
        {
            this.itemsField = new List<ComponentListType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttributeList", typeof(AttributeListType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DimensionList", typeof(DimensionListType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Group", typeof(GroupType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MeasureList", typeof(MeasureListType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataTarget", typeof(MetadataTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportStructure", typeof(ReportStructureType), Order = 0)]
        public List<ComponentListType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("AttributeList", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class AttributeListType : AttributeListBaseType
    {

        private List<AttributeType> items1Field;

        public AttributeListType()
        {
            this.items1Field = new List<AttributeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingYearStartDay", typeof(ReportingYearStartDayType), Order = 0)]
        public List<AttributeType> Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Attribute", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class AttributeType : AttributeBaseType
    {

        private List<ConceptReferenceType> conceptRoleField;

        private AttributeRelationshipType attributeRelationshipField;

        private UsageStatusType assignmentStatusField;

        public AttributeType()
        {
            this.attributeRelationshipField = new AttributeRelationshipType();
            this.conceptRoleField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptRole", Order = 0)]
        public List<ConceptReferenceType> ConceptRole
        {
            get
            {
                return this.conceptRoleField;
            }
            set
            {
                this.conceptRoleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AttributeRelationshipType AttributeRelationship
        {
            get
            {
                return this.attributeRelationshipField;
            }
            set
            {
                this.attributeRelationshipField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public UsageStatusType assignmentStatus
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    public enum UsageStatusType
    {

        /// <remarks/>
        Mandatory,

        /// <remarks/>
        Conditional,
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class AttributeBaseType : ComponentType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintContentTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Component", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class ComponentType : ComponentBaseType
    {

        private ConceptReferenceType conceptIdentityField;

        private RepresentationType localRepresentationField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ConceptReferenceType ConceptIdentity
        {
            get
            {
                return this.conceptIdentityField;
            }
            set
            {
                this.conceptIdentityField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public RepresentationType LocalRepresentation
        {
            get
            {
                return this.localRepresentationField;
            }
            set
            {
                this.localRepresentationField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleDataStructureRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptRepresentation))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class RepresentationType
    {

        private List<object> itemsField;

        public RepresentationType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Enumeration", typeof(ItemSchemeReferenceBaseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("EnumerationFormat", typeof(CodededTextFormatType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TextFormat", typeof(TextFormatType), Order = 0)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataAttributeRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class IdentifiableObjectRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReportPeriodRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConstraintRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataSetRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class KeyDescriptorValuesRepresentationType : RepresentationType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleDataStructureRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayRepresentationType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class DataStructureRepresentationType : RepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MeasureDimensionRepresentationType : DataStructureRepresentationType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRepresentationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayRepresentationType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class SimpleDataStructureRepresentationType : DataStructureRepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class TimeDimensionRepresentationType : SimpleDataStructureRepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReportingYearStartDayRepresentationType : SimpleDataStructureRepresentationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConceptRepresentation : RepresentationType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintContentTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ComponentBaseType : IdentifiableType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObject))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintContentTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingYearStartDayType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionListBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeListBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NameableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HybridCodelistMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HybridCodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseOrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class IdentifiableType : AnnotableType
    {

        private string idField;

        private string urnField;

        private string uriField;

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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ComponentMapType : AnnotableType
    {

        private LocalComponentListComponentReferenceType sourceField;

        private LocalComponentListComponentReferenceType targetField;

        private RepresentationMapType representationMappingField;

        public ComponentMapType()
        {
            this.representationMappingField = new RepresentationMapType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public LocalComponentListComponentReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LocalComponentListComponentReferenceType Target
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public RepresentationMapType RepresentationMapping
        {
            get
            {
                return this.representationMappingField;
            }
            set
            {
                this.representationMappingField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class HybridCodeMapType : AnnotableType
    {

        private AnyLocalCodeReferenceType sourceField;

        private AnyLocalCodeReferenceType targetField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AnyLocalCodeReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AnyLocalCodeReferenceType Target
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationMapType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ItemAssociation", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class ItemAssociationType : AnnotableType
    {

        private LocalItemReferenceType sourceField;

        private LocalItemReferenceType targetField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public LocalItemReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LocalItemReferenceType Target
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingCategoryMap", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ReportingCategoryMapType : ItemAssociationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ConceptMap", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ConceptMapType : ItemAssociationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("CodeMap", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class CodeMapType : ItemAssociationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("CategoryMap", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class CategoryMapType : ItemAssociationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationMap", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class OrganisationMapType : ItemAssociationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ComputationType : AnnotableType
    {

        private List<TextType> descriptionField;

        private string localIDField;

        private string softwarePackageField;

        private string softwareLanguageField;

        private string softwareVersionField;

        public ComputationType()
        {
            this.descriptionField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localID
        {
            get
            {
                return this.localIDField;
            }
            set
            {
                this.localIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string softwarePackage
        {
            get
            {
                return this.softwarePackageField;
            }
            set
            {
                this.softwarePackageField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string softwareLanguage
        {
            get
            {
                return this.softwareLanguageField;
            }
            set
            {
                this.softwareLanguageField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string softwareVersion
        {
            get
            {
                return this.softwareVersionField;
            }
            set
            {
                this.softwareVersionField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class InputOutputType : AnnotableType
    {

        private ObjectReferenceType objectReferenceField;

        private string localIDField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ObjectReferenceType ObjectReference
        {
            get
            {
                return this.objectReferenceField;
            }
            set
            {
                this.objectReferenceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localID
        {
            get
            {
                return this.localIDField;
            }
            set
            {
                this.localIDField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class TransitionType : IdentifiableType
    {

        private LocalProcessStepReferenceType targetStepField;

        private List<TextType> conditionField;

        private string localIDField;

        public TransitionType()
        {
            this.conditionField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public LocalProcessStepReferenceType TargetStep
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

        [System.Xml.Serialization.XmlElementAttribute("Condition", Order = 1)]
        public List<TextType> Condition
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localID
        {
            get
            {
                return this.localIDField;
            }
            set
            {
                this.localIDField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class HierarchicalCodeBaseType : IdentifiableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class HierarchicalCodeType : HierarchicalCodeBaseType
    {

        private List<object> itemsField;

        private List<HierarchicalCodeType> hierarchicalCodeField;

        private LocalLevelReferenceType levelField;

        private string versionField;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        public HierarchicalCodeType()
        {
            this.hierarchicalCodeField = new List<HierarchicalCodeType>();
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Code", typeof(CodeReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodeID", typeof(LocalCodeReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodelistAliasRef", typeof(string), Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCode", Order = 1)]
        public List<HierarchicalCodeType> HierarchicalCode
        {
            get
            {
                return this.hierarchicalCodeField;
            }
            set
            {
                this.hierarchicalCodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public LocalLevelReferenceType Level
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
        public System.DateTime validFrom
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validFromSpecified
        {
            get
            {
                return this.validFromFieldSpecified;
            }
            set
            {
                this.validFromFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime validTo
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validToSpecified
        {
            get
            {
                return this.validToFieldSpecified;
            }
            set
            {
                this.validToFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionListBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionListType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeListBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeListType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ComponentList", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class ComponentListType : IdentifiableType
    {

        private List<ComponentType> itemsField;

        public ComponentListType()
        {
            this.itemsField = new List<ComponentType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ConstraintContentTarget", typeof(ConstraintContentTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSetTarget", typeof(DataSetTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(DimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("GroupDimension", typeof(GroupDimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("IdentifiableObjectTarget", typeof(IdentifiableObjectTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyDescriptorValuesTarget", typeof(KeyDescriptorValuesTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MeasureDimension", typeof(MeasureDimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataAttribute", typeof(MetadataAttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PrimaryMeasure", typeof(PrimaryMeasureType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportPeriodTarget", typeof(ReportPeriodTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingYearStartDay", typeof(ReportingYearStartDayType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TimeDimension", typeof(TimeDimensionType), Order = 0)]
        public List<ComponentType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ConstraintContentTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ConstraintContentTargetType : TargetObject
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintContentTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class TargetObject : ComponentType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class IdentifiableObjectTargetBaseType : TargetObject
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("IdentifiableObjectTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class IdentifiableObjectTargetType : IdentifiableObjectTargetBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private ObjectTypeCodelistType objectTypeField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ObjectTypeCodelistType objectType
        {
            get
            {
                return this.objectTypeField;
            }
            set
            {
                this.objectTypeField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public IdentifiableObjectTargetType()
        {
        }

        public IdentifiableObjectTargetType(string id, ObjectTypeCodelistType objectType, AnnotationType annotation)
        {
            this.id = id;
            this.objectType = objectType;

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ReportPeriodTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ReportPeriodTargetType : TargetObject
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("DataSetTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DataSetTargetType : TargetObject
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("KeyDescriptorValuesTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class KeyDescriptorValuesTargetType : TargetObject
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Dimension", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DimensionType : BaseDimensionType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class BaseDimensionType : BaseDimensionBaseType
    {

        private List<ConceptReferenceType> conceptRoleField;

        private int positionField;

        private bool positionFieldSpecified;

        private DimensionTypeType typeField;

        private bool typeFieldSpecified;

        public BaseDimensionType()
        {
            this.conceptRoleField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptRole", Order = 0)]
        public List<ConceptReferenceType> ConceptRole
        {
            get
            {
                return this.conceptRoleField;
            }
            set
            {
                this.conceptRoleField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool positionSpecified
        {
            get
            {
                return this.positionFieldSpecified;
            }
            set
            {
                this.positionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DimensionTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class BaseDimensionBaseType : ComponentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("MeasureDimension", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class MeasureDimensionType : BaseDimensionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("TimeDimension", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class TimeDimensionType : BaseDimensionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("GroupDimension", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class GroupDimensionType : GroupDimensionBaseType
    {

        private LocalDimensionReferenceType dimensionReferenceField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public LocalDimensionReferenceType DimensionReference
        {
            get
            {
                return this.dimensionReferenceField;
            }
            set
            {
                this.dimensionReferenceField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupDimensionType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class GroupDimensionBaseType : ComponentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataAttribute", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class MetadataAttributeType : MetadataAttributeBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private List<MetadataAttributeType> metadataAttributeField;

        private string minOccursField;

        private string maxOccursField;

        private bool isPresentationalField;

        #endregion "--Private--"

        #region "--Public--"

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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string minOccurs
        {
            get
            {
                return this.minOccursField;
            }
            set
            {
                this.minOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string maxOccurs
        {
            get
            {
                return this.maxOccursField;
            }
            set
            {
                this.maxOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isPresentational
        {
            get
            {
                return this.isPresentationalField;
            }
            set
            {
                this.isPresentationalField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public MetadataAttributeType():this(string.Empty, false, null)
        {
        }

        public MetadataAttributeType(string id, bool isPresentationField, AnnotationType annotation)
        {
            this.id = id;
            this.isPresentationalField = isPresentationField;

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }

            this.metadataAttributeField = new List<MetadataAttributeType>();
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class MetadataAttributeBaseType : ComponentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("PrimaryMeasure", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class PrimaryMeasureType : ComponentType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingYearStartDay", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ReportingYearStartDayType : AttributeType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ReportStructureBaseType : ComponentListType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ReportStructure", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ReportStructureType : ReportStructureBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private List<LocalMetadataTargetReferenceType> metadataTargetField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute("MetadataTarget", Order = 0)]
        public List<LocalMetadataTargetReferenceType> MetadataTarget
        {
            get
            {
                return this.metadataTargetField;
            }
            set
            {
                this.metadataTargetField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public ReportStructureType()
            : this(string.Empty, null)
        {
        }

        public ReportStructureType(string id, AnnotationType annotation)
        {
            this.id = id;

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }

            this.metadataTargetField = new List<LocalMetadataTargetReferenceType>();
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class MetadataTargetBaseType : ComponentListType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataTarget", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class MetadataTargetType : MetadataTargetBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private List<TargetObject> itemsField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute("ConstraintContentTarget", typeof(ConstraintContentTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSetTarget", typeof(DataSetTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("IdentifiableObjectTarget", typeof(IdentifiableObjectTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyDescriptorValuesTarget", typeof(KeyDescriptorValuesTargetType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportPeriodTarget", typeof(ReportPeriodTargetType), Order = 0)]
        public List<TargetObject> Items1
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public MetadataTargetType():this(string.Empty, null)
        {
        }

        public MetadataTargetType(string id, AnnotationType annotation)
        {
            this.id = id;

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("MeasureList", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class MeasureListType : ComponentListType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class GroupBaseType : ComponentListType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Group", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class GroupType : GroupBaseType
    {

        private List<object> items1Field;

        public GroupType()
        {
            this.items1Field = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraint", typeof(AttachmentConstraintReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("GroupDimension", typeof(GroupDimensionType), Order = 0)]
        public List<object> Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionListType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class DimensionListBaseType : ComponentListType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("DimensionList", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DimensionListType : DimensionListBaseType
    {

        private List<BaseDimensionType> items1Field;

        public DimensionListType()
        {
            this.items1Field = new List<BaseDimensionType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(DimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MeasureDimension", typeof(MeasureDimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TimeDimension", typeof(TimeDimensionType), Order = 0)]
        public List<BaseDimensionType> Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeListType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class AttributeListBaseType : ComponentListType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HybridCodelistMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HybridCodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeMapBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseOrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class NameableType : IdentifiableType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        public NameableType()
        {
            this.descriptionField = new List<TextType>();
            this.nameField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute("Description", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 1)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class StructureMapBaseType : NameableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class StructureMapType : StructureMapBaseType
    {

        private StructureOrUsageReferenceType sourceField;

        private StructureOrUsageReferenceType targetField;

        private ComponentMapType componentMapField;

        private bool isExtensionField;

        public StructureMapType()
        {
            this.componentMapField = new ComponentMapType();
            this.isExtensionField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureOrUsageReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public StructureOrUsageReferenceType Target
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ComponentMapType ComponentMap
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HybridCodelistMapType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class HybridCodelistMapBaseType : NameableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class HybridCodelistMapType : HybridCodelistMapBaseType
    {

        private AnyCodelistReferenceType sourceField;

        private AnyCodelistReferenceType targetField;

        private List<HybridCodeMapType> hybridCodeMapField;

        public HybridCodelistMapType()
        {
            this.hybridCodeMapField = new List<HybridCodeMapType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AnyCodelistReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AnyCodelistReferenceType Target
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

        [System.Xml.Serialization.XmlElementAttribute("HybridCodeMap", Order = 2)]
        public List<HybridCodeMapType> HybridCodeMap
        {
            get
            {
                return this.hybridCodeMapField;
            }
            set
            {
                this.hybridCodeMapField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ItemSchemeMapBaseType : NameableType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ItemSchemeMapType : ItemSchemeMapBaseType
    {

        private ItemSchemeReferenceBaseType sourceField;

        private ItemSchemeReferenceBaseType targetField;

        private List<ItemAssociationType> itemsField;

        public ItemSchemeMapType()
        {
            this.itemsField = new List<ItemAssociationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ItemSchemeReferenceBaseType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ItemSchemeReferenceBaseType Target
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

        [System.Xml.Serialization.XmlElementAttribute("CategoryMap", typeof(CategoryMapType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("CodeMap", typeof(CodeMapType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptMap", typeof(ConceptMapType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationMap", typeof(OrganisationMapType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingCategoryMap", typeof(ReportingCategoryMapType), Order = 2)]
        public List<ItemAssociationType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReportingTaxonomyMapType : ItemSchemeMapType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConceptSchemeMapType : ItemSchemeMapType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CodelistMapType : ItemSchemeMapType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CategorySchemeMapType : ItemSchemeMapType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class OrganisationSchemeMapType : ItemSchemeMapType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ProcessStepBaseType : NameableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ProcessStepType : ProcessStepBaseType
    {

        private List<InputOutputType> inputField;

        private List<InputOutputType> outputField;

        private ComputationType computationField;

        private List<TransitionType> transitionField;

        private List<ProcessStepType> processStepField;

        public ProcessStepType()
        {
            this.processStepField = new List<ProcessStepType>();
            this.transitionField = new List<TransitionType>();
            this.computationField = new ComputationType();
            this.outputField = new List<InputOutputType>();
            this.inputField = new List<InputOutputType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Input", Order = 0)]
        public List<InputOutputType> Input
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

        [System.Xml.Serialization.XmlElementAttribute("Output", Order = 1)]
        public List<InputOutputType> Output
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public ComputationType Computation
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

        [System.Xml.Serialization.XmlElementAttribute("Transition", Order = 3)]
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

        [System.Xml.Serialization.XmlElementAttribute("ProcessStep", Order = 4)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class LevelBaseType : NameableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class LevelType : LevelBaseType
    {

        private CodingTextFormatType codingFormatField;

        private LevelType levelField;

        public LevelType()
        {
            //this.levelField = new LevelType();
            this.codingFormatField = new CodingTextFormatType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CodingTextFormatType CodingFormat
        {
            get
            {
                return this.codingFormatField;
            }
            set
            {
                this.codingFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LevelType Level
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class HierarchyBaseType : NameableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class HierarchyType : HierarchyBaseType
    {

        private List<HierarchicalCodeType> hierarchicalCodeField;

        private LevelType levelField;

        private bool leveledField;

        public HierarchyType()
        {
            this.levelField = new LevelType();
            this.hierarchicalCodeField = new List<HierarchicalCodeType>();
            this.leveledField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCode", Order = 0)]
        public List<HierarchicalCodeType> HierarchicalCode
        {
            get
            {
                return this.hierarchicalCodeField;
            }
            set
            {
                this.hierarchicalCodeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public LevelType Level
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool leveled
        {
            get
            {
                return this.leveledField;
            }
            set
            {
                this.leveledField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseOrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ItemBaseType : NameableType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BaseOrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Item", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public abstract class ItemType : ItemBaseType
    {

        private List<object> itemsField;

        public ItemType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Code", typeof(CodeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(ConceptType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Item", typeof(ItemType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Parent", typeof(LocalItemReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingCategory", typeof(ReportingCategoryType), Order = 0)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Category", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class CategoryType : ItemType
    {
        #region "Constructors"

        public CategoryType():this(string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public CategoryType(string id, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Code", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class CodeType : ItemType
    {
        #region "Constructors"

        public CodeType():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public CodeType(string id, string name, string description, string language, string parentId, AnnotationType annotation)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }

            if (!string.IsNullOrEmpty(parentId))
            {
                this.Items = new List<object>();
                this.Items.Add(new LocalCodeReferenceType());
                ((LocalCodeReferenceType)this.Items[0]).Items = new List<object>();
                ((LocalCodeReferenceType)this.Items[0]).Items.Add(new LocalCodeRefType());
                ((LocalCodeRefType)((LocalCodeReferenceType)this.Items[0]).Items[0]).id = parentId;
            }
            else
            {
                this.Items = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("Concept", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ConceptType : ConceptBaseType
    {
        #region "--Varibles--"

        #region "--Private--"

        private ConceptRepresentation coreRepresentationField;

        private ISOConceptReferenceType iSOConceptReferenceField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ConceptRepresentation CoreRepresentation
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ISOConceptReferenceType ISOConceptReference
        {
            get
            {
                return this.iSOConceptReferenceField;
            }
            set
            {
                this.iSOConceptReferenceField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public ConceptType():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public ConceptType(string id, string name, string description, string language, string parentId, AnnotationType annotation)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }

            if (!string.IsNullOrEmpty(parentId))
            {
                this.Items = new List<object>();
                this.Items.Add(new LocalCodeReferenceType());
                ((LocalCodeReferenceType)this.Items[0]).Items = new List<object>();
                ((LocalCodeReferenceType)this.Items[0]).Items.Add(new LocalCodeRefType());
                ((LocalCodeRefType)((LocalCodeReferenceType)this.Items[0]).Items[0]).id = parentId;
            }
            else
            {
                this.Items = null;
            }
        }

        #endregion "--Constrcutors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ISOConceptReferenceType
    {

        private string conceptAgencyField;

        private string conceptSchemeIDField;

        private string conceptIDField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ConceptAgency
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ConceptBaseType : ItemType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingCategory", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class ReportingCategoryType : ReportingCategoryBaseType
    {

        //private List<MaintainableReferenceBaseType> itemsField;

        //public ReportingCategoryType()
        //{
        //    this.itemsField = new List<MaintainableReferenceBaseType>();
        //}

        //[System.Xml.Serialization.XmlElementAttribute("ProvisioningMetadata", typeof(StructureUsageReferenceType), Order = 0)]
        //[System.Xml.Serialization.XmlElementAttribute("StructuralMetadata", typeof(StructureReferenceType), Order = 0)]
        //public List<MaintainableReferenceBaseType> Items
        //{
        //    get
        //    {
        //        return this.itemsField;
        //    }
        //    set
        //    {
        //        this.itemsField = value;
        //    }
        //}
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ReportingCategoryBaseType : ItemType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class BaseOrganisationType : ItemType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class VersionableType : NameableType
    {

        private string versionField;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        public VersionableType()
        {
            this.versionField = "1.0";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        //[System.ComponentModel.DefaultValueAttribute("1.0")]
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
        public System.DateTime validFrom
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validFromSpecified
        {
            get
            {
                return this.validFromFieldSpecified;
            }
            set
            {
                this.validFromFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime validTo
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validToSpecified
        {
            get
            {
                return this.validToFieldSpecified;
            }
            set
            {
                this.validToFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class MaintainableBaseType : VersionableType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class MaintainableType : MaintainableBaseType
    {

        private string agencyIDField;

        private bool isFinalField;

        private bool isExternalReferenceField;

        private string serviceURLField;

        private string structureURLField;

        public MaintainableType()
        {
            this.isFinalField = false;
            this.isExternalReferenceField = false;
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
        [System.ComponentModel.DefaultValueAttribute(false)]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string serviceURL
        {
            get
            {
                return this.serviceURLField;
            }
            set
            {
                this.serviceURLField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string structureURL
        {
            get
            {
                return this.structureURLField;
            }
            set
            {
                this.structureURLField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class StructureSetBaseType : MaintainableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class StructureSetType : StructureSetBaseType
    {

        private List<StructureOrUsageReferenceType> relatedStructureField;

        private List<NameableType> itemsField;

        public StructureSetType()
        {
            this.itemsField = new List<NameableType>();
            this.relatedStructureField = new List<StructureOrUsageReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("RelatedStructure", Order = 0)]
        public List<StructureOrUsageReferenceType> RelatedStructure
        {
            get
            {
                return this.relatedStructureField;
            }
            set
            {
                this.relatedStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeMap", typeof(CategorySchemeMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CodelistMap", typeof(CodelistMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeMap", typeof(ConceptSchemeMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("HybridCodelistMap", typeof(HybridCodelistMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeMap", typeof(OrganisationSchemeMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyMap", typeof(ReportingTaxonomyMapType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("StructureMap", typeof(StructureMapType), Order = 1)]
        public List<NameableType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ProvisionAgreementType : MaintainableType
    {
        #region "--Variables--"

        #region "--Private--"

        private StructureUsageReferenceType structureUsageField;

        private DataProviderReferenceType dataProviderField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureUsageReferenceType StructureUsage
        {
            get
            {
                return this.structureUsageField;
            }
            set
            {
                this.structureUsageField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DataProviderReferenceType DataProvider
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "Constructors"

        public ProvisionAgreementType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public ProvisionAgreementType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ProcessType : MaintainableType
    {

        private List<ProcessStepType> processStepField;

        public ProcessType()
        {
            this.processStepField = new List<ProcessStepType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessStep", Order = 0)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class HierarchicalCodelistBaseType : MaintainableType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class HierarchicalCodelistType : HierarchicalCodelistBaseType
    {

        private List<IncludedCodelistReferenceType> includedCodelistField;

        private List<HierarchyType> hierarchyField;

        public HierarchicalCodelistType()
        {
            this.hierarchyField = new List<HierarchyType>();
            this.includedCodelistField = new List<IncludedCodelistReferenceType>();
        }
        public HierarchicalCodelistType(string ID, string AgencyId, string Version, bool IsExternalReference, string Uri, string Name, string Description, AnnotationType Annotation)
        {
            this.id = ID;
            this.agencyID = AgencyId;
            this.version = Version;
            this.isExternalReference = IsExternalReference;
            this.uri = Uri;
            if (!string.IsNullOrEmpty(Name))
            {
                this.Name = new List<SDMXObjectModel.Common.TextType>();
                this.Name.Add(new SDMXObjectModel.Common.TextType());
                this.Name[0].Value = Name;
            }
            else
            {
                this.Name = null;
            }
            if (!string.IsNullOrEmpty(Description))
            {
                this.Description = new List<SDMXObjectModel.Common.TextType>();
                this.Description.Add(new SDMXObjectModel.Common.TextType());
                this.Description[0].Value = Description;
            }
            else
            {
                this.Description = null;
            }
            if (Annotation == null)
            {
                this.Annotations = null;
            }
            else
            {
                this.Annotations = new List<SDMXObjectModel.Common.AnnotationType>();
                this.Annotations.Add(new SDMXObjectModel.Common.AnnotationType());
                this.Annotations[0].AnnotationText = new List<SDMXObjectModel.Common.TextType>();
                this.Annotations[0].AnnotationText.Add(new SDMXObjectModel.Common.TextType());
                this.Annotations[0].AnnotationText[0].Value = Annotation.AnnotationText[0].Value;
                this.Annotations[0].AnnotationTitle = Annotation.AnnotationTitle;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("IncludedCodelist", Order = 0)]
        public List<IncludedCodelistReferenceType> IncludedCodelist
        {
            get
            {
                return this.includedCodelistField;
            }
            set
            {
                this.includedCodelistField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Hierarchy", Order = 1)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ConstraintBaseType : MaintainableType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ConstraintType : ConstraintBaseType
    {

        private ConstraintAttachmentType constraintAttachmentField;

        private List<object> itemsField;

        public ConstraintType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ConstraintAttachmentType ConstraintAttachment
        {
            get
            {
                return this.constraintAttachmentField;
            }
            set
            {
                this.constraintAttachmentField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CubeRegion", typeof(CubeRegionType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataKeySet", typeof(DataKeySetType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataKeySet", typeof(MetadataKeySetType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataTargetRegion", typeof(MetadataTargetRegionType), Order = 1)]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ContentConstraintBaseType : ConstraintType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ContentConstraintType : ContentConstraintBaseType
    {

        private ReleaseCalendarType releaseCalendarField;

        private ReferencePeriodType referencePeriodField;

        private ContentConstraintTypeCodeType typeField;

        public ContentConstraintType()
        {
            this.releaseCalendarField = new ReleaseCalendarType();
            this.typeField = ContentConstraintTypeCodeType.Actual;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ReleaseCalendarType ReleaseCalendar
        {
            get
            {
                return this.releaseCalendarField;
            }
            set
            {
                this.releaseCalendarField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ReferencePeriodType ReferencePeriod
        {
            get
            {
                return this.referencePeriodField;
            }
            set
            {
                this.referencePeriodField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ContentConstraintTypeCodeType.Actual)]
        public ContentConstraintTypeCodeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class AttachmentConstraintType : ConstraintType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CategorisationType : MaintainableType
    {
        #region "--Variables--"

        #region "--Private--"

        private ObjectReferenceType sourceField;

        private CategoryReferenceType targetField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ObjectReferenceType Source
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public CategoryReferenceType Target
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "Constructors"

        public CategorisationType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public CategorisationType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class StructureUsageType : MaintainableType
    {

        private StructureReferenceBaseType structureField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureReferenceBaseType Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataflowType : StructureUsageType
    {
         #region "Constructors"

        public MetadataflowType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public MetadataflowType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataflowType : StructureUsageType
    {
        #region "Constructors"

        public DataflowType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataflowType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class StructureType : MaintainableType
    {

        private GroupingType itemField;

        [System.Xml.Serialization.XmlElementAttribute("DataStructureComponents", typeof(DataStructureComponentsType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureComponents", typeof(MetadataStructureComponentsType), Order = 0)]
        public GroupingType Item
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("DataStructureComponents", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DataStructureComponentsType : DataStructureComponentsBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private DimensionListType dimensionListField;

        private List<GroupType> groupField;

        private AttributeListType attributeListField;

        private MeasureListType measureListField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DimensionListType DimensionList
        {
            get
            {
                return this.dimensionListField;
            }
            set
            {
                this.dimensionListField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Group", Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public AttributeListType AttributeList
        {
            get
            {
                return this.attributeListField;
            }
            set
            {
                this.attributeListField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public MeasureListType MeasureList
        {
            get
            {
                return this.measureListField;
            }
            set
            {
                this.measureListField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public DataStructureComponentsType()
        {
            this.measureListField = new MeasureListType();
            this.attributeListField = new AttributeListType();
            this.groupField = new List<GroupType>();
            this.dimensionListField = new DimensionListType();
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class DataStructureComponentsBaseType : GroupingType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataStructureComponents", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class MetadataStructureComponentsType : MetadataStructureComponentsBaseType
    {
        #region "--Variables--"

        #region "--Private--"

        private List<MetadataTargetType> metadataTargetField;

        private List<ReportStructureType> reportStructureField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute("MetadataTarget", Order = 0)]
        public List<MetadataTargetType> MetadataTarget
        {
            get
            {
                return this.metadataTargetField;
            }
            set
            {
                this.metadataTargetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportStructure", Order = 1)]
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        public MetadataStructureComponentsType()
        {
            this.reportStructureField = new List<ReportStructureType>();
            this.metadataTargetField = new List<MetadataTargetType>();
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureComponentsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataStructureComponentsBaseType : GroupingType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataStructureType : StructureType
    {
        #region "Constructors"

        public MetadataStructureType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public MetadataStructureType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataStructureType : StructureType
    {
        #region "Constructors"

        public DataStructureType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataStructureType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class ItemSchemeType : MaintainableType
    {

        private List<ItemType> itemsField;

        private bool isPartialField;

        public ItemSchemeType()
        {
            this.itemsField = new List<ItemType>();
            this.isPartialField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Code", typeof(CodeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(ConceptType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingCategory", typeof(ReportingCategoryType), Order = 0)]
        public List<ItemType> Items
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
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isPartial
        {
            get
            {
                return this.isPartialField;
            }
            set
            {
                this.isPartialField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ReportingTaxonomyType : ItemSchemeType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public abstract class OrganisationSchemeBaseType : ItemSchemeType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConceptSchemeType : ItemSchemeType
    {
        #region "Constructors"

        public ConceptSchemeType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public ConceptSchemeType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CodelistType : ItemSchemeType
    {
        #region "Constructors"

        public CodelistType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public CodelistType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CategorySchemeType : ItemSchemeType
    {
        #region "Constructors"

        public CategorySchemeType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public CategorySchemeType(string id, string agencyId, string version, string name, string description, string language, AnnotationType annotation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }
            else
            {
                this.Name = null;
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }
            else
            {
                this.Description = null;
            }

            if (annotation != null)
            {
                this.Annotations = new List<AnnotationType>();
                this.Annotations.Add(annotation);
            }
            else
            {
                this.Annotations = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IncludeInSchema = false)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationUnit", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class OrganisationUnitType : OrganisationType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("DataProvider", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DataProviderType : OrganisationType
    {
        #region "Constructors"

        public DataProviderType():this(string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataProviderType(string id, string name, string description, string language, ContactType contact)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (contact != null)
            {
                this.Contact = new List<ContactType>();
                this.Contact.Add(contact);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute("DataConsumer", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = false)]
    public class DataConsumerType : OrganisationType
    {
        #region "Constructors"

        public DataConsumerType():this(string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataConsumerType(string id, string name, string description, string language, ContactType contact)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (contact != null)
            {
                this.Contact = new List<ContactType>();
                this.Contact.Add(contact);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class OrganisationUnitSchemeType : OrganisationSchemeType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataProviderSchemeType : OrganisationSchemeType
    {
        #region "Constructors"

        public DataProviderSchemeType():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataProviderSchemeType(string id, string agencyId, string version, string name, string description, string language, OrganisationType organisation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (organisation != null)
            {
                this.Organisation = new List<OrganisationType>();
                this.Organisation.Add(organisation);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataConsumerSchemeType : OrganisationSchemeType
    {
        #region "Constructors"

        public DataConsumerSchemeType():this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null)
        {
        }

        public DataConsumerSchemeType(string id, string agencyId, string version, string name, string description, string language, OrganisationType organisation)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;

            if (!string.IsNullOrEmpty(name))
            {
                this.Name = new List<TextType>();
                this.Name.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                this.Description = new List<TextType>();
                this.Description.Add(new TextType(language, description));
            }

            if (organisation != null)
            {
                this.Organisation = new List<OrganisationType>();
                this.Organisation.Add(organisation);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class OrganisationSchemesType
    {

        private List<OrganisationSchemeType> itemsField;

        public OrganisationSchemesType()
        {
            this.itemsField = new List<OrganisationSchemeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyScheme", typeof(AgencySchemeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataConsumerScheme", typeof(DataConsumerSchemeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProviderScheme", typeof(DataProviderSchemeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationUnitScheme", typeof(OrganisationUnitSchemeType), Order = 0)]
        public List<OrganisationSchemeType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CategorisationsType
    {

        private List<CategorisationType> categorisationField;

        public CategorisationsType()
        {
            this.categorisationField = new List<CategorisationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Categorisation", Order = 0)]
        public List<CategorisationType> Categorisation
        {
            get
            {
                return this.categorisationField;
            }
            set
            {
                this.categorisationField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class CodelistsType
    {

        private List<CodelistType> codelistField;

        public CodelistsType()
        {
            this.codelistField = new List<CodelistType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Codelist", Order = 0)]
        public List<CodelistType> Codelist
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConceptsType
    {

        private List<ConceptSchemeType> conceptSchemeField;

        public ConceptsType()
        {
            this.conceptSchemeField = new List<ConceptSchemeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptScheme", Order = 0)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class MetadataStructuresType
    {

        private List<MetadataStructureType> metadataStructureField;

        public MetadataStructuresType()
        {
            this.metadataStructureField = new List<MetadataStructureType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", Order = 0)]
        public List<MetadataStructureType> MetadataStructure
        {
            get
            {
                return this.metadataStructureField;
            }
            set
            {
                this.metadataStructureField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class DataStructuresType
    {

        private List<DataStructureType> dataStructureField;

        public DataStructuresType()
        {
            this.dataStructureField = new List<DataStructureType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataStructure", Order = 0)]
        public List<DataStructureType> DataStructure
        {
            get
            {
                return this.dataStructureField;
            }
            set
            {
                this.dataStructureField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ConstraintsType
    {

        private List<ConstraintType> itemsField;

        public ConstraintsType()
        {
            this.itemsField = new List<ConstraintType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraint", typeof(AttachmentConstraintType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ContentConstraint", typeof(ContentConstraintType), Order = 0)]
        public List<ConstraintType> Items
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", IsNullable = true)]
    public class ProvisionAgreementsType
    {

        private List<ProvisionAgreementType> provisionAgreementField;

        public ProvisionAgreementsType()
        {
            this.provisionAgreementField = new List<ProvisionAgreementType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", Order = 0)]
        public List<ProvisionAgreementType> ProvisionAgreement
        {
            get
            {
                return this.provisionAgreementField;
            }
            set
            {
                this.provisionAgreementField = value;
            }
        }
    }
}
