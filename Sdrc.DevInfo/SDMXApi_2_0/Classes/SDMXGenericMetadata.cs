using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Common;

namespace SDMXApi_2_0.GenericMetadata
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", IsNullable = false)]
    public class MetadataSetType
    {

        private List<TextType> nameField;

        private string metadataStructureRefField;

        private string metadataStructureAgencyRefField;

        private string reportRefField;

        private List<AttributeValueSetType> attributeValueSetField;

        private List<AnnotationType> annotationsField;

        private string metadataStructureURIField;

        private string datasetIDField;

        private string dataProviderSchemeAgencyIdField;

        private string dataProviderSchemeIdField;

        private string dataProviderIDField;

        private string dataflowAgencyIDField;

        private string dataflowIDField;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private string reportingBeginDateField;

        private string reportingEndDateField;

        private string validFromDateField;

        private string validToDateField;

        private string publicationYearField;

        private string publicationPeriodField;

        public MetadataSetType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.attributeValueSetField = new List<AttributeValueSetType>();
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
        public string MetadataStructureRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string MetadataStructureAgencyRef
        {
            get
            {
                return this.metadataStructureAgencyRefField;
            }
            set
            {
                this.metadataStructureAgencyRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string ReportRef
        {
            get
            {
                return this.reportRefField;
            }
            set
            {
                this.reportRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttributeValueSet", Order = 4)]
        public List<AttributeValueSetType> AttributeValueSet
        {
            get
            {
                return this.attributeValueSetField;
            }
            set
            {
                this.attributeValueSetField = value;
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string metadataStructureURI
        {
            get
            {
                return this.metadataStructureURIField;
            }
            set
            {
                this.metadataStructureURIField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datasetID
        {
            get
            {
                return this.datasetIDField;
            }
            set
            {
                this.datasetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderSchemeAgencyId
        {
            get
            {
                return this.dataProviderSchemeAgencyIdField;
            }
            set
            {
                this.dataProviderSchemeAgencyIdField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderSchemeId
        {
            get
            {
                return this.dataProviderSchemeIdField;
            }
            set
            {
                this.dataProviderSchemeIdField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderID
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataflowAgencyID
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataflowID
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActionType action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reportingBeginDate
        {
            get
            {
                return this.reportingBeginDateField;
            }
            set
            {
                this.reportingBeginDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reportingEndDate
        {
            get
            {
                return this.reportingEndDateField;
            }
            set
            {
                this.reportingEndDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFromDate
        {
            get
            {
                return this.validFromDateField;
            }
            set
            {
                this.validFromDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validToDate
        {
            get
            {
                return this.validToDateField;
            }
            set
            {
                this.validToDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gYear")]
        public string publicationYear
        {
            get
            {
                return this.publicationYearField;
            }
            set
            {
                this.publicationYearField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string publicationPeriod
        {
            get
            {
                return this.publicationPeriodField;
            }
            set
            {
                this.publicationPeriodField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", IsNullable = true)]
    public class ReportedAttributeType
    {

        private List<TextType> valueField;

        private System.DateTime startTimeField;

        private bool startTimeFieldSpecified;

        private List<ReportedAttributeType> reportedAttributeField;

        private List<AnnotationType> annotationsField;

        private string conceptIDField;

        public ReportedAttributeType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.reportedAttributeField = new List<ReportedAttributeType>();
            this.valueField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Value", Order = 0)]
        public List<TextType> Value
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime StartTime
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool StartTimeSpecified
        {
            get
            {
                return this.startTimeFieldSpecified;
            }
            set
            {
                this.startTimeFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportedAttribute", Order = 2)]
        public List<ReportedAttributeType> ReportedAttribute
        {
            get
            {
                return this.reportedAttributeField;
            }
            set
            {
                this.reportedAttributeField = value;
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
        public string conceptID
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", IsNullable = true)]
    public class ComponentValueType
    {

        private ObjectIDType objectField;

        private string componentField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ObjectIDType @object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string component
        {
            get
            {
                return this.componentField;
            }
            set
            {
                this.componentField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute(DataType = "NMTOKEN")]
        public string Value
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", IsNullable = true)]
    public class AttributeValueSetType
    {

        private string targetRefField;

        private List<ComponentValueType> targetValuesField;

        private List<ReportedAttributeType> reportedAttributeField;

        public AttributeValueSetType()
        {
            this.reportedAttributeField = new List<ReportedAttributeType>();
            this.targetValuesField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string TargetRef
        {
            get
            {
                return this.targetRefField;
            }
            set
            {
                this.targetRefField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ComponentValue", IsNullable = false)]
        public List<ComponentValueType> TargetValues
        {
            get
            {
                return this.targetValuesField;
            }
            set
            {
                this.targetValuesField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportedAttribute", Order = 2)]
        public List<ReportedAttributeType> ReportedAttribute
        {
            get
            {
                return this.reportedAttributeField;
            }
            set
            {
                this.reportedAttributeField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/genericmetadata", IsNullable = true)]
    public class TargetValuesType
    {

        private List<ComponentValueType> componentValueField;

        public TargetValuesType()
        {
            this.componentValueField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ComponentValue", Order = 0)]
        public List<ComponentValueType> ComponentValue
        {
            get
            {
                return this.componentValueField;
            }
            set
            {
                this.componentValueField = value;
            }
        }
    }

}
