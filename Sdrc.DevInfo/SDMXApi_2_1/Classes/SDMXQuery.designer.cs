using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;

namespace SDMXObjectModel.Query
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("StructuralMetadataWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public abstract class MaintainableWhereType : VersionableWhereType
    {

        private QueryNestedIDType agencyIDField;

        private MaintainableTypeCodelistType typeField;

        private bool typeFieldSpecified;

        public MaintainableWhereType()
        {
            this.agencyIDField = new QueryNestedIDType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public QueryNestedIDType AgencyID
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
        public MaintainableTypeCodelistType type
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class QueryNestedIDType
    {

        private string operatorField;

        private string valueField;

        public QueryNestedIDType()
        {
            this.operatorField = "equal";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("equal")]
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class InputOrOutputObjectType
    {

        private ObjectReferenceType objectReferenceField;

        private InputOutputTypeCodeType typeField;

        public InputOrOutputObjectType()
        {
            this.objectReferenceField = new ObjectReferenceType();
            this.typeField = InputOutputTypeCodeType.Any;
        }

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
        [System.ComponentModel.DefaultValueAttribute(InputOutputTypeCodeType.Any)]
        public InputOutputTypeCodeType type
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MappedObjectRefType : MaintainableRefType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MappedObjectType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MappedObjectReferenceType : MaintainableReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MappedObjectType : MappedObjectReferenceType
    {

        private SourceTargetType typeField;

        public MappedObjectType()
        {
            this.typeField = SourceTargetType.Any;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SourceTargetType.Any)]
        public SourceTargetType type
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    public enum SourceTargetType
    {

        /// <remarks/>
        Any,

        /// <remarks/>
        Source,

        /// <remarks/>
        Target,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    public enum InputOutputTypeCodeType
    {

        /// <remarks/>
        Input,

        /// <remarks/>
        Output,

        /// <remarks/>
        Any,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ConstraintAttachmentWhereType
    {

        private object[] itemsField;

        private ConstraintAttachmentWhereChoiceType[] itemsElementNameField;

        public ConstraintAttachmentWhereType()
        {
            //this.itemsElementNameField = new List<ConstraintAttachmentWhereChoiceType>();
            //this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSourceURL", typeof(string), DataType = "anyURI", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataStructure", typeof(DataStructureReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dataflow", typeof(DataflowReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(MetadataStructureReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", typeof(MetadataflowReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementReferenceType), Order = 0)]
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
        public ConstraintAttachmentWhereChoiceType[] ItemsElementName
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IncludeInSchema = false)]
    public enum ConstraintAttachmentWhereChoiceType
    {

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        DataSet,

        /// <remarks/>
        DataSourceURL,

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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class QueryIDType
    {

        private string operatorField;

        private string valueField;

        public QueryIDType()
        {
            this.operatorField = "equal";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("equal")]
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("TextValue", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class QueryTextType : TextType
    {

        private string operatorField;

        public QueryTextType()
        {
            this.operatorField = "equal";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("equal")]
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class QueryStringType
    {

        private string operatorField;

        private string valueField;

        public QueryStringType()
        {
            this.operatorField = "equal";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("equal")]
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class AnnotationWhereType
    {

        private QueryStringType typeField;

        private QueryStringType titleField;

        private QueryTextType textField;

        public AnnotationWhereType()
        {
            this.textField = new QueryTextType();
            this.titleField = new QueryStringType();
            this.typeField = new QueryStringType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public QueryStringType Type
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public QueryStringType Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public QueryTextType Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NameableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class AnnotableWhereType
    {

        private AnnotationWhereType annotationField;

        public AnnotableWhereType()
        {
            this.annotationField = new AnnotationWhereType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AnnotationWhereType Annotation
        {
            get
            {
                return this.annotationField;
            }
            set
            {
                this.annotationField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NameableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class IdentifiableWhereType : AnnotableWhereType
    {

        private string uRNField;

        private QueryIDType idField;

        public IdentifiableWhereType()
        {
            this.idField = new QueryIDType();
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
        public QueryIDType ID
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ProcessStepWhereType : IdentifiableWhereType
    {

        private List<InputOrOutputObjectType> inputOrOutputObjectField;

        private List<ProcessStepWhereType> processStepWhereField;

        public ProcessStepWhereType()
        {
            this.processStepWhereField = new List<ProcessStepWhereType>();
            this.inputOrOutputObjectField = new List<InputOrOutputObjectType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("InputOrOutputObject", Order = 0)]
        public List<InputOrOutputObjectType> InputOrOutputObject
        {
            get
            {
                return this.inputOrOutputObjectField;
            }
            set
            {
                this.inputOrOutputObjectField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessStepWhere", Order = 1)]
        public List<ProcessStepWhereType> ProcessStepWhere
        {
            get
            {
                return this.processStepWhereField;
            }
            set
            {
                this.processStepWhereField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureComponentWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ComponentWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public abstract class ComponentWhereType : IdentifiableWhereType
    {

        private ConceptReferenceType conceptIdentityField;

        private ItemSchemeReferenceBaseType enumerationField;

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
        public ItemSchemeReferenceBaseType Enumeration
        {
            get
            {
                return this.enumerationField;
            }
            set
            {
                this.enumerationField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class MetadataAttributeWhereBaseType : ComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataAttributeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class MetadataAttributeWhereType : MetadataAttributeWhereBaseType
    {

        private List<MetadataAttributeWhereType> metadataAttributeWhereField;

        public MetadataAttributeWhereType()
        {
            this.metadataAttributeWhereField = new List<MetadataAttributeWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeWhere", Order = 0)]
        public List<MetadataAttributeWhereType> MetadataAttributeWhere
        {
            get
            {
                return this.metadataAttributeWhereField;
            }
            set
            {
                this.metadataAttributeWhereField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TargetObjectWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class TargetObjectWhereBaseType : ComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("TargetObjectWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class TargetObjectWhereType : TargetObjectWhereBaseType
    {

        private TargetObjectTypeCodelistType typeField;

        private bool typeFieldSpecified;

        private ObjectTypeCodelistType targetClassField;

        private bool targetClassFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TargetObjectTypeCodelistType type
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ObjectTypeCodelistType targetClass
        {
            get
            {
                return this.targetClassField;
            }
            set
            {
                this.targetClassField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool targetClassSpecified
        {
            get
            {
                return this.targetClassFieldSpecified;
            }
            set
            {
                this.targetClassFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class MeasureDimensionWhereBaseType : ComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("MeasureDimensionWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class MeasureDimensionWhereType : MeasureDimensionWhereBaseType
    {

        private List<ConceptReferenceType> roleField;

        public MeasureDimensionWhereType()
        {
            this.roleField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 0)]
        public List<ConceptReferenceType> Role
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataStructureComponentWhereType : ComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("PrimaryMeasureWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class PrimaryMeasureWhereType : DataStructureComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("TimeDimensionWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class TimeDimensionWhereType : DataStructureComponentWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("GroupDimensionWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class DimensionWhereType : DataStructureComponentWhereType
    {

        private List<ConceptReferenceType> roleField;

        public DimensionWhereType()
        {
            this.roleField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 0)]
        public List<ConceptReferenceType> Role
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("AttributeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class AttributeWhereType : DataStructureComponentWhereType
    {

        private List<ConceptReferenceType> roleField;

        public AttributeWhereType()
        {
            this.roleField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 0)]
        public List<ConceptReferenceType> Role
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ComponentListWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public abstract class ComponentListWhereType : IdentifiableWhereType
    {

        private ComponentWhereType[] itemsField;

        private ComponentListWhereChoiceType[] itemsElementNameField;

        public ComponentListWhereType()
        {
            //this.itemsElementNameField = new List<ComponentListWhereChoiceType>();
            //this.itemsField = new List<ComponentWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttributeWhere", typeof(AttributeWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DimensionWhere", typeof(DimensionWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("GroupDimensionWhere", typeof(DimensionWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MeasureDimensionWhere", typeof(MeasureDimensionWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeWhere", typeof(MetadataAttributeWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("PrimaryMeasureWhere", typeof(PrimaryMeasureWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TargetObjectWhere", typeof(TargetObjectWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TimeDimensionWhere", typeof(TimeDimensionWhereType), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public ComponentWhereType[] Items
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
        public ComponentListWhereChoiceType[] ItemsElementName
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IncludeInSchema = false)]
    public enum ComponentListWhereChoiceType
    {

        /// <remarks/>
        AttributeWhere,

        /// <remarks/>
        DimensionWhere,

        /// <remarks/>
        GroupDimensionWhere,

        /// <remarks/>
        MeasureDimensionWhere,

        /// <remarks/>
        MetadataAttributeWhere,

        /// <remarks/>
        PrimaryMeasureWhere,

        /// <remarks/>
        TargetObjectWhere,

        /// <remarks/>
        TimeDimensionWhere,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ReportStructureWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ReportStructureWhereType : ComponentListWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataTargetWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class MetadataTargetWhereType : ComponentListWhereType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class GroupWhereBaseType : ComponentListWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("GroupWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class GroupWhereType : GroupWhereBaseType
    {

        private AttachmentConstraintReferenceType attachmentConstraintField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public AttachmentConstraintReferenceType AttachmentConstraint
        {
            get
            {
                return this.attachmentConstraintField;
            }
            set
            {
                this.attachmentConstraintField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class NameableWhereType : IdentifiableWhereType
    {

        private QueryTextType nameField;

        private QueryTextType descriptionField;

        public NameableWhereType()
        {
            this.descriptionField = new QueryTextType();
            this.nameField = new QueryTextType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public QueryTextType Name
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
        public QueryTextType Description
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ItemWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public abstract class ItemWhereType : NameableWhereType
    {

        private List<object> itemsField;

        public ItemWhereType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryWhere", typeof(CategoryWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodeWhere", typeof(CodeWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptWhere", typeof(ConceptWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ItemWhere", typeof(ItemWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationWhere", typeof(OrganisationWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Parent", typeof(LocalItemReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingCategoryWhere", typeof(ReportingCategoryWhereType), Order = 0)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("CategoryWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class CategoryWhereType : ItemWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("CodeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class CodeWhereType : ItemWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ConceptWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ConceptWhereType : ConceptWhereBaseType
    {

        private CodelistReferenceType enumerationField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CodelistReferenceType Enumeration
        {
            get
            {
                return this.enumerationField;
            }
            set
            {
                this.enumerationField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ConceptWhereBaseType : ItemWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class OrganisationWhereType : ItemWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingCategoryWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ReportingCategoryWhereType : ReportingCategoryWhereBaseType
    {

        //private List<MaintainableReferenceBaseType> itemsField;

        //public ReportingCategoryWhereType()
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ReportingCategoryWhereBaseType : ItemWhereType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class VersionableWhereType : NameableWhereType
    {

        private string versionField;

        private TimeRangeValueType versionToField;

        private TimeRangeValueType versionFromField;

        private bool versionActiveField;

        private bool versionActiveFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TimeRangeValueType VersionTo
        {
            get
            {
                return this.versionToField;
            }
            set
            {
                this.versionToField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public TimeRangeValueType VersionFrom
        {
            get
            {
                return this.versionFromField;
            }
            set
            {
                this.versionFromField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool VersionActive
        {
            get
            {
                return this.versionActiveField;
            }
            set
            {
                this.versionActiveField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VersionActiveSpecified
        {
            get
            {
                return this.versionActiveFieldSpecified;
            }
            set
            {
                this.versionActiveFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class StructureSetWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSetWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class StructureSetWhereType : StructureSetWhereBaseType
    {

        private List<StructureOrUsageReferenceType> relatedStructuresField;

        private List<MappedObjectType> mappedObjectField;

        public StructureSetWhereType()
        {
            this.mappedObjectField = new List<MappedObjectType>();
            this.relatedStructuresField = new List<StructureOrUsageReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("RelatedStructures", Order = 0)]
        public List<StructureOrUsageReferenceType> RelatedStructures
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

        [System.Xml.Serialization.XmlElementAttribute("MappedObject", Order = 1)]
        public List<MappedObjectType> MappedObject
        {
            get
            {
                return this.mappedObjectField;
            }
            set
            {
                this.mappedObjectField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("StructuresWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class StructuresWhereType : MaintainableWhereType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ProvisionAgreementWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ProvisionAgreementWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ProvisionAgreementWhereType : ProvisionAgreementWhereBaseType
    {

        private StructureUsageReferenceType structureUsageField;

        private DataProviderReferenceType dataProviderField;

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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ProcessWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ProcessWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ProcessWhereType : ProcessWhereBaseType
    {

        private List<ProcessStepWhereType> processStepWhereField;

        public ProcessWhereType()
        {
            this.processStepWhereField = new List<ProcessStepWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessStepWhere", Order = 0)]
        public List<ProcessStepWhereType> ProcessStepWhere
        {
            get
            {
                return this.processStepWhereField;
            }
            set
            {
                this.processStepWhereField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class HierarchicalCodelistWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("HierarchicalCodelistWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class HierarchicalCodelistWhereType : HierarchicalCodelistWhereBaseType
    {

        private List<CodelistReferenceType> includedCodelistField;

        public HierarchicalCodelistWhereType()
        {
            this.includedCodelistField = new List<CodelistReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("IncludedCodelist", Order = 0)]
        public List<CodelistReferenceType> IncludedCodelist
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ConstraintWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ConstraintWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ConstraintWhereType : ConstraintWhereBaseType
    {

        private ConstraintAttachmentWhereType constraintAttachmentWhereField;

        private bool allowableField;

        private bool allowableFieldSpecified;

        public ConstraintWhereType()
        {
            this.constraintAttachmentWhereField = new ConstraintAttachmentWhereType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ConstraintAttachmentWhereType ConstraintAttachmentWhere
        {
            get
            {
                return this.constraintAttachmentWhereField;
            }
            set
            {
                this.constraintAttachmentWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool allowable
        {
            get
            {
                return this.allowableField;
            }
            set
            {
                this.allowableField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool allowableSpecified
        {
            get
            {
                return this.allowableFieldSpecified;
            }
            set
            {
                this.allowableFieldSpecified = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class CategorisationWhereBaseType : MaintainableWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("CategorisationWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class CategorisationWhereType : CategorisationWhereBaseType
    {

        private CategorySchemeReferenceType categorySchemeField;

        private CategoryReferenceType targetCategoryField;

        private ObjectReferenceType objectReferenceField;

        private List<ObjectTypeCodelistType> categorisedObjectTypeField;

        public CategorisationWhereType()
        {
            this.categorisedObjectTypeField = new List<ObjectTypeCodelistType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CategorySchemeReferenceType CategoryScheme
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public CategoryReferenceType TargetCategory
        {
            get
            {
                return this.targetCategoryField;
            }
            set
            {
                this.targetCategoryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("CategorisedObjectType", Order = 3)]
        public List<ObjectTypeCodelistType> CategorisedObjectType
        {
            get
            {
                return this.categorisedObjectTypeField;
            }
            set
            {
                this.categorisedObjectTypeField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class StructureUsageWhereType : MaintainableWhereType
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataflowWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class MetadataflowWhereType : StructureUsageWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("DataflowWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class DataflowWhereType : StructureUsageWhereType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class StructureWhereType : MaintainableWhereType
    {

        private List<ConceptReferenceType> usedConceptField;

        private List<ItemSchemeReferenceBaseType> usedRepresentationField;

        private List<ComponentListWhereType> itemsField;

        private ComponentWhereType[] items1Field;

        private StructureWhereChoiceType[] items1ElementNameField;

        public StructureWhereType()
        {
            //this.items1ElementNameField = new List<StructureWhereChoiceType>();
            //this.items1Field = new List<ComponentWhereType>();
            this.itemsField = new List<ComponentListWhereType>();
            this.usedRepresentationField = new List<ItemSchemeReferenceBaseType>();
            this.usedConceptField = new List<ConceptReferenceType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("UsedConcept", Order = 0)]
        public List<ConceptReferenceType> UsedConcept
        {
            get
            {
                return this.usedConceptField;
            }
            set
            {
                this.usedConceptField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("UsedRepresentation", Order = 1)]
        public List<ItemSchemeReferenceBaseType> UsedRepresentation
        {
            get
            {
                return this.usedRepresentationField;
            }
            set
            {
                this.usedRepresentationField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("GroupWhere", typeof(GroupWhereType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataTargetWhere", typeof(MetadataTargetWhereType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("ReportStructureWhere", typeof(ReportStructureWhereType), Order = 2)]
        public List<ComponentListWhereType> Items
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

        [System.Xml.Serialization.XmlElementAttribute("AttributeWhere", typeof(AttributeWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("DimensionWhere", typeof(DimensionWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("GroupDimensionWhere", typeof(DimensionWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("MeasureDimensionWhere", typeof(MeasureDimensionWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeWhere", typeof(MetadataAttributeWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("PrimaryMeasureWhere", typeof(PrimaryMeasureWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("TargetObjectWhere", typeof(TargetObjectWhereType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("TimeDimensionWhere", typeof(TimeDimensionWhereType), Order = 3)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("Items1ElementName")]
        public ComponentWhereType[] Items1
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

        [System.Xml.Serialization.XmlElementAttribute("Items1ElementName", Order = 4)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public StructureWhereChoiceType[] Items1ElementName
        {
            get
            {
                return this.items1ElementNameField;
            }
            set
            {
                this.items1ElementNameField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IncludeInSchema = false)]
    public enum StructureWhereChoiceType
    {

        /// <remarks/>
        AttributeWhere,

        /// <remarks/>
        DimensionWhere,

        /// <remarks/>
        GroupDimensionWhere,

        /// <remarks/>
        MeasureDimensionWhere,

        /// <remarks/>
        MetadataAttributeWhere,

        /// <remarks/>
        PrimaryMeasureWhere,

        /// <remarks/>
        TargetObjectWhere,

        /// <remarks/>
        TimeDimensionWhere,
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class MetadataStructureWhereBaseType : StructureWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataStructureWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class MetadataStructureWhereType : MetadataStructureWhereBaseType
    {

        private List<MetadataTargetWhereType> metadataTargetWhereField;

        private List<TargetObjectWhereType> targetObjectWhereField;

        private List<ReportStructureWhereType> reportStructureWhereField;

        private List<MetadataAttributeWhereType> metadataAttributeWhereField;

        public MetadataStructureWhereType()
        {
            this.metadataAttributeWhereField = new List<MetadataAttributeWhereType>();
            this.reportStructureWhereField = new List<ReportStructureWhereType>();
            this.targetObjectWhereField = new List<TargetObjectWhereType>();
            this.metadataTargetWhereField = new List<MetadataTargetWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataTargetWhere", Order = 0)]
        public List<MetadataTargetWhereType> MetadataTargetWhere
        {
            get
            {
                return this.metadataTargetWhereField;
            }
            set
            {
                this.metadataTargetWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("TargetObjectWhere", Order = 1)]
        public List<TargetObjectWhereType> TargetObjectWhere
        {
            get
            {
                return this.targetObjectWhereField;
            }
            set
            {
                this.targetObjectWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportStructureWhere", Order = 2)]
        public List<ReportStructureWhereType> ReportStructureWhere
        {
            get
            {
                return this.reportStructureWhereField;
            }
            set
            {
                this.reportStructureWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeWhere", Order = 3)]
        public List<MetadataAttributeWhereType> MetadataAttributeWhere
        {
            get
            {
                return this.metadataAttributeWhereField;
            }
            set
            {
                this.metadataAttributeWhereField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class DataStructureWhereBaseType : StructureWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("DataStructureWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class DataStructureWhereType : DataStructureWhereBaseType
    {

        private List<AttributeWhereType> attributeWhereField;

        private List<DimensionWhereType> dimensionWhereField;

        private MeasureDimensionWhereType measureDimensionWhereField;

        private TimeDimensionWhereType timeDimensionWhereField;

        private PrimaryMeasureWhereType primaryMeasureWhereField;

        public DataStructureWhereType()
        {
            this.primaryMeasureWhereField = new PrimaryMeasureWhereType();
            this.timeDimensionWhereField = new TimeDimensionWhereType();
            this.measureDimensionWhereField = new MeasureDimensionWhereType();
            this.dimensionWhereField = new List<DimensionWhereType>();
            this.attributeWhereField = new List<AttributeWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AttributeWhere", Order = 0)]
        public List<AttributeWhereType> AttributeWhere
        {
            get
            {
                return this.attributeWhereField;
            }
            set
            {
                this.attributeWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DimensionWhere", Order = 1)]
        public List<DimensionWhereType> DimensionWhere
        {
            get
            {
                return this.dimensionWhereField;
            }
            set
            {
                this.dimensionWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MeasureDimensionWhereType MeasureDimensionWhere
        {
            get
            {
                return this.measureDimensionWhereField;
            }
            set
            {
                this.measureDimensionWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public TimeDimensionWhereType TimeDimensionWhere
        {
            get
            {
                return this.timeDimensionWhereField;
            }
            set
            {
                this.timeDimensionWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public PrimaryMeasureWhereType PrimaryMeasureWhere
        {
            get
            {
                return this.primaryMeasureWhereField;
            }
            set
            {
                this.primaryMeasureWhereField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistWhereType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeWhereType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ItemSchemeWhereType : MaintainableWhereType
    {

        private List<ItemWhereType> itemsField;

        public ItemSchemeWhereType()
        {
            this.itemsField = new List<ItemWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryWhere", typeof(CategoryWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodeWhere", typeof(CodeWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptWhere", typeof(ConceptWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationWhere", typeof(OrganisationWhereType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingCategoryWhere", typeof(ReportingCategoryWhereType), Order = 0)]
        public List<ItemWhereType> Items
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingTaxonomyWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ReportingTaxonomyWhereType : ItemSchemeWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationSchemeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class OrganisationSchemeWhereType : ItemSchemeWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("ConceptSchemeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class ConceptSchemeWhereType : ItemSchemeWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("CodelistWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class CodelistWhereType : ItemSchemeWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("CategorySchemeWhere", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class CategorySchemeWhereType : ItemSchemeWhereType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("NumericValue", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class NumericValueType
    {

        private string operatorField;

        private decimal valueField;

        public NumericValueType()
        {
            this.operatorField = "equal";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("equal")]
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("TimeValue", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class TimePeriodValueType
    {

        private TimeOperatorType operatorField;

        private string reportingYearStartDayField;

        private string valueField;

        public TimePeriodValueType()
        {
            this.operatorField = TimeOperatorType.equal;
            this.reportingYearStartDayField = "Any";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(TimeOperatorType.equal)]
        public TimeOperatorType @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("Any")]
        public string reportingYearStartDay
        {
            get
            {
                return this.reportingYearStartDayField;
            }
            set
            {
                this.reportingYearStartDayField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("Value", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = false)]
    public class SimpleValueType
    {

        private SimpleOperatorType operatorField;

        private string valueField;

        public SimpleValueType()
        {
            this.operatorField = SimpleOperatorType.equal;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(SimpleOperatorType.equal)]
        public SimpleOperatorType @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataReturnDetailsBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReturnDetailsBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class ReturnDetailsBaseType
    {

        private string defaultLimitField;

        private string detailField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string defaultLimit
        {
            get
            {
                return this.defaultLimitField;
            }
            set
            {
                this.defaultLimitField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataReturnDetailsType : ReturnDetailsBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class DataReturnDetailsBaseType : ReturnDetailsBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataReturnDetailsType : DataReturnDetailsBaseType
    {

        private int firstNObservationsField;

        private bool firstNObservationsFieldSpecified;

        private int lastNObservationsField;

        private bool lastNObservationsFieldSpecified;

        private List<DataStructureRequestType> structureField;

        private ObservationActionCodeType observationActionField;

        public DataReturnDetailsType()
        {
            this.structureField = new List<DataStructureRequestType>();
            this.observationActionField = ObservationActionCodeType.Active;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int FirstNObservations
        {
            get
            {
                return this.firstNObservationsField;
            }
            set
            {
                this.firstNObservationsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FirstNObservationsSpecified
        {
            get
            {
                return this.firstNObservationsFieldSpecified;
            }
            set
            {
                this.firstNObservationsFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int LastNObservations
        {
            get
            {
                return this.lastNObservationsField;
            }
            set
            {
                this.lastNObservationsField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LastNObservationsSpecified
        {
            get
            {
                return this.lastNObservationsFieldSpecified;
            }
            set
            {
                this.lastNObservationsFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 2)]
        public List<DataStructureRequestType> Structure
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ObservationActionCodeType.Active)]
        public ObservationActionCodeType observationAction
        {
            get
            {
                return this.observationActionField;
            }
            set
            {
                this.observationActionField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DataStructureRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute("DataStructureRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataStructureRequestType : SDMXObjectModel.Common.DataStructureRequestType
    {

        private bool timeSeriesField;

        private bool processConstraintsField;

        public DataStructureRequestType()
        {
            this.timeSeriesField = false;
            this.processConstraintsField = false;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool timeSeries
        {
            get
            {
                return this.timeSeriesField;
            }
            set
            {
                this.timeSeriesField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool processConstraints
        {
            get
            {
                return this.processConstraintsField;
            }
            set
            {
                this.processConstraintsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    public enum ObservationActionCodeType
    {

        /// <remarks/>
        Active,

        /// <remarks/>
        Added,

        /// <remarks/>
        Updated,

        /// <remarks/>
        Deleted,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class TimeSeriesDataReturnDetailsType : DataReturnDetailsType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class GenericDataReturnDetailsType : DataReturnDetailsType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class GenericTimeSeriesDataReturnDetailsType : GenericDataReturnDetailsType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReturnDetailsType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class StructureReturnDetailsBaseType : ReturnDetailsBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReturnDetailsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class StructureReturnDetailsType : StructureReturnDetailsBaseType
    {

        private ReferencesType referencesField;

        private bool returnMatchedArtefactField;

        public StructureReturnDetailsType()
        {
            this.referencesField = new ReferencesType();
            this.returnMatchedArtefactField = true;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ReferencesType References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool returnMatchedArtefact
        {
            get
            {
                return this.returnMatchedArtefactField;
            }
            set
            {
                this.returnMatchedArtefactField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ReferencesType
    {

        private object itemField;

        private ReferencesChoiceType itemElementNameField;

        private bool processConstraintsField;

        private MaintainableReturnDetailType detailField;

        public ReferencesType()
        {
            this.processConstraintsField = false;
            this.detailField = MaintainableReturnDetailType.Full;
        }

        [System.Xml.Serialization.XmlElementAttribute("All", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Children", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Descendants", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("None", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Parents", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ParentsAndSiblings", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SpecificObjects", typeof(MaintainableObjectTypeListType), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ReferencesChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool processConstraints
        {
            get
            {
                return this.processConstraintsField;
            }
            set
            {
                this.processConstraintsField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(MaintainableReturnDetailType.Full)]
        public MaintainableReturnDetailType detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IncludeInSchema = false)]
    public enum ReferencesChoiceType
    {

        /// <remarks/>
        All,

        /// <remarks/>
        Children,

        /// <remarks/>
        Descendants,

        /// <remarks/>
        None,

        /// <remarks/>
        Parents,

        /// <remarks/>
        ParentsAndSiblings,

        /// <remarks/>
        SpecificObjects,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    public enum MaintainableReturnDetailType
    {

        /// <remarks/>
        Stub,

        /// <remarks/>
        CompleteStub,

        /// <remarks/>
        Full,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MaintainableReturnDetailsType : StructureReturnDetailsType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructuresQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationQueryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class StructuralMetadataQueryType
    {

        private StructureReturnDetailsType returnDetailsField;

        private MaintainableWhereType itemField;

        public StructuralMetadataQueryType()
        {
            this.returnDetailsField = new StructureReturnDetailsType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureReturnDetailsType ReturnDetails
        {
            get
            {
                return this.returnDetailsField;
            }
            set
            {
                this.returnDetailsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategorisationWhere", typeof(CategorisationWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeWhere", typeof(CategorySchemeWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CodelistWhere", typeof(CodelistWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeWhere", typeof(ConceptSchemeWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ConstraintWhere", typeof(ConstraintWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataStructureWhere", typeof(DataStructureWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowWhere", typeof(DataflowWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistWhere", typeof(HierarchicalCodelistWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureWhere", typeof(MetadataStructureWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowWhere", typeof(MetadataflowWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeWhere", typeof(OrganisationSchemeWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProcessWhere", typeof(ProcessWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementWhere", typeof(ProvisionAgreementWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyWhere", typeof(ReportingTaxonomyWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSetWhere", typeof(StructureSetWhereType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("StructuresWhere", typeof(StructuresWhereType), Order = 1)]
        public MaintainableWhereType Item
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class StructureSetQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class StructuresQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ReportingTaxonomyQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ProvisionAgreementQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ProcessQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class OrganisationSchemeQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataStructureQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataflowQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class HierarchicalCodelistQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataStructureQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataflowQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ConstraintQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ConceptSchemeQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class CodelistQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class CategorySchemeQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class CategorisationQueryType : StructuralMetadataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ConceptValueType
    {

        private ConceptReferenceType conceptField;

        private List<object> itemsField;

        public ConceptValueType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ConceptReferenceType Concept
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

        [System.Xml.Serialization.XmlElementAttribute("NumericValue", typeof(NumericValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TextValue", typeof(QueryTextType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TimeValue", typeof(TimePeriodValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(SimpleValueType), Order = 1)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class CodeValueType
    {

        private CodelistReferenceType codelistField;

        private string valueField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public CodelistReferenceType Codelist
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataQueryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataQueryType
    {

        private DataReturnDetailsType returnDetailsField;

        private DataParametersAndType dataWhereField;

        public DataQueryType()
        {
            this.dataWhereField = new DataParametersAndType();
            this.returnDetailsField = new DataReturnDetailsType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DataReturnDetailsType ReturnDetails
        {
            get
            {
                return this.returnDetailsField;
            }
            set
            {
                this.returnDetailsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DataParametersAndType DataWhere
        {
            get
            {
                return this.dataWhereField;
            }
            set
            {
                this.dataWhereField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataParametersAndType : DataParametersType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataParametersAndType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataParametersOrType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class DataParametersType
    {

        private List<QueryIDType> dataSetIDField;

        private List<DataProviderReferenceType> dataProviderField;

        private List<DataStructureReferenceType> dataStructureField;

        private List<DataflowReferenceType> dataflowField;

        private List<ProvisionAgreementReferenceType> provisionAgreementField;

        private List<CategoryReferenceType> categoryField;

        private List<TimeRangeValueType> updatedField;

        private List<ConceptValueType> conceptValueField;

        private List<CodeValueType> representationValueField;

        private List<DimensionValueType> dimensionValueField;

        private List<TimeDimensionValueType> timeDimensionValueField;

        private List<AttributeValueType> attributeValueField;

        private List<PrimaryMeasureValueType> primaryMeasureValueField;

        private List<AttachmentConstraintReferenceType> attachmentConstraintField;

        private List<TimeDataType> timeFormatField;

        private List<DataParametersOrType> orField;

        private List<DataParametersAndType> andField;

        public DataParametersType()
        {
            this.andField = new List<DataParametersAndType>();
            this.orField = new List<DataParametersOrType>();
            this.timeFormatField = new List<TimeDataType>();
            this.attachmentConstraintField = new List<AttachmentConstraintReferenceType>();
            this.primaryMeasureValueField = new List<PrimaryMeasureValueType>();
            this.attributeValueField = new List<AttributeValueType>();
            this.timeDimensionValueField = new List<TimeDimensionValueType>();
            this.dimensionValueField = new List<DimensionValueType>();
            this.representationValueField = new List<CodeValueType>();
            this.conceptValueField = new List<ConceptValueType>();
            this.updatedField = new List<TimeRangeValueType>();
            this.categoryField = new List<CategoryReferenceType>();
            this.provisionAgreementField = new List<ProvisionAgreementReferenceType>();
            this.dataflowField = new List<DataflowReferenceType>();
            this.dataStructureField = new List<DataStructureReferenceType>();
            this.dataProviderField = new List<DataProviderReferenceType>();
            this.dataSetIDField = new List<QueryIDType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 0)]
        public List<QueryIDType> DataSetID
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

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", Order = 1)]
        public List<DataProviderReferenceType> DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute("DataStructure", Order = 2)]
        public List<DataStructureReferenceType> DataStructure
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

        [System.Xml.Serialization.XmlElementAttribute("Dataflow", Order = 3)]
        public List<DataflowReferenceType> Dataflow
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

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", Order = 4)]
        public List<ProvisionAgreementReferenceType> ProvisionAgreement
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

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 5)]
        public List<CategoryReferenceType> Category
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

        [System.Xml.Serialization.XmlElementAttribute("Updated", Order = 6)]
        public List<TimeRangeValueType> Updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptValue", Order = 7)]
        public List<ConceptValueType> ConceptValue
        {
            get
            {
                return this.conceptValueField;
            }
            set
            {
                this.conceptValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("RepresentationValue", Order = 8)]
        public List<CodeValueType> RepresentationValue
        {
            get
            {
                return this.representationValueField;
            }
            set
            {
                this.representationValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DimensionValue", Order = 9)]
        public List<DimensionValueType> DimensionValue
        {
            get
            {
                return this.dimensionValueField;
            }
            set
            {
                this.dimensionValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("TimeDimensionValue", Order = 10)]
        public List<TimeDimensionValueType> TimeDimensionValue
        {
            get
            {
                return this.timeDimensionValueField;
            }
            set
            {
                this.timeDimensionValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttributeValue", Order = 11)]
        public List<AttributeValueType> AttributeValue
        {
            get
            {
                return this.attributeValueField;
            }
            set
            {
                this.attributeValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("PrimaryMeasureValue", Order = 12)]
        public List<PrimaryMeasureValueType> PrimaryMeasureValue
        {
            get
            {
                return this.primaryMeasureValueField;
            }
            set
            {
                this.primaryMeasureValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraint", Order = 13)]
        public List<AttachmentConstraintReferenceType> AttachmentConstraint
        {
            get
            {
                return this.attachmentConstraintField;
            }
            set
            {
                this.attachmentConstraintField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("TimeFormat", Order = 14)]
        public List<TimeDataType> TimeFormat
        {
            get
            {
                return this.timeFormatField;
            }
            set
            {
                this.timeFormatField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Or", Order = 15)]
        public List<DataParametersOrType> Or
        {
            get
            {
                return this.orField;
            }
            set
            {
                this.orField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("And", Order = 16)]
        public List<DataParametersAndType> And
        {
            get
            {
                return this.andField;
            }
            set
            {
                this.andField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DimensionValueType : DataStructureComponentValueQueryType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionValueType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class DataStructureComponentValueQueryType
    {

        private string idField;

        private List<object> itemsField;

        public DataStructureComponentValueQueryType()
        {
            this.itemsField = new List<object>();
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

        [System.Xml.Serialization.XmlElementAttribute("NumericValue", typeof(NumericValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TextValue", typeof(QueryTextType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TimeValue", typeof(TimePeriodValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(SimpleValueType), Order = 1)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class TimeDimensionValueType : DataStructureComponentValueQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class PrimaryMeasureValueType : DataStructureComponentValueQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class AttributeValueType : DataStructureComponentValueQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataParametersOrType : DataParametersType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataQueryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class GenericDataQueryType : DataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class GenericTimeSeriesDataQueryType : GenericDataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class TimeSeriesDataQueryType : DataQueryType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataQueryType
    {

        private MetadataReturnDetailsType returnDetailsField;

        private MetadataParametersAndType metadataParametersField;

        public MetadataQueryType()
        {
            this.metadataParametersField = new MetadataParametersAndType();
            this.returnDetailsField = new MetadataReturnDetailsType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public MetadataReturnDetailsType ReturnDetails
        {
            get
            {
                return this.returnDetailsField;
            }
            set
            {
                this.returnDetailsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public MetadataParametersAndType MetadataParameters
        {
            get
            {
                return this.metadataParametersField;
            }
            set
            {
                this.metadataParametersField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataParametersAndType : MetadataParametersType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataParametersAndType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataParametersOrType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public abstract class MetadataParametersType
    {

        private List<QueryIDType> metadataSetIDField;

        private List<DataProviderReferenceType> dataProviderField;

        private List<MetadataStructureReferenceType> metadataStructureField;

        private List<MetadataflowReferenceType> metadataflowField;

        private List<ProvisionAgreementReferenceType> provisionAgreementField;

        private List<CategoryReferenceType> categoryField;

        private List<TimeRangeValueType> updatedField;

        private List<ConceptValueType> conceptValueField;

        private List<CodeValueType> representationValueField;

        private List<MetadataTargetValueType> metadataTargetValueField;

        private List<ReportStructureValueType> reportStructureValueField;

        private List<AttachmentConstraintReferenceType> attachmentConstraintField;

        private List<ObjectReferenceType> attachedObjectField;

        private List<DataKeyType> attachedDataKeyField;

        private List<SetReferenceType> attachedDataSetField;

        private List<TimeRangeValueType> attachedReportingPeriodField;

        private List<MetadataParametersOrType> orField;

        private List<MetadataParametersAndType> andField;

        public MetadataParametersType()
        {
            this.andField = new List<MetadataParametersAndType>();
            this.orField = new List<MetadataParametersOrType>();
            this.attachedReportingPeriodField = new List<TimeRangeValueType>();
            this.attachedDataSetField = new List<SetReferenceType>();
            this.attachedDataKeyField = new List<DataKeyType>();
            this.attachedObjectField = new List<ObjectReferenceType>();
            this.attachmentConstraintField = new List<AttachmentConstraintReferenceType>();
            this.reportStructureValueField = new List<ReportStructureValueType>();
            this.metadataTargetValueField = new List<MetadataTargetValueType>();
            this.representationValueField = new List<CodeValueType>();
            this.conceptValueField = new List<ConceptValueType>();
            this.updatedField = new List<TimeRangeValueType>();
            this.categoryField = new List<CategoryReferenceType>();
            this.provisionAgreementField = new List<ProvisionAgreementReferenceType>();
            this.metadataflowField = new List<MetadataflowReferenceType>();
            this.metadataStructureField = new List<MetadataStructureReferenceType>();
            this.dataProviderField = new List<DataProviderReferenceType>();
            this.metadataSetIDField = new List<QueryIDType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataSetID", Order = 0)]
        public List<QueryIDType> MetadataSetID
        {
            get
            {
                return this.metadataSetIDField;
            }
            set
            {
                this.metadataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", Order = 1)]
        public List<DataProviderReferenceType> DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", Order = 2)]
        public List<MetadataStructureReferenceType> MetadataStructure
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

        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", Order = 3)]
        public List<MetadataflowReferenceType> Metadataflow
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

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", Order = 4)]
        public List<ProvisionAgreementReferenceType> ProvisionAgreement
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

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 5)]
        public List<CategoryReferenceType> Category
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

        [System.Xml.Serialization.XmlElementAttribute("Updated", Order = 6)]
        public List<TimeRangeValueType> Updated
        {
            get
            {
                return this.updatedField;
            }
            set
            {
                this.updatedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptValue", Order = 7)]
        public List<ConceptValueType> ConceptValue
        {
            get
            {
                return this.conceptValueField;
            }
            set
            {
                this.conceptValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("RepresentationValue", Order = 8)]
        public List<CodeValueType> RepresentationValue
        {
            get
            {
                return this.representationValueField;
            }
            set
            {
                this.representationValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataTargetValue", Order = 9)]
        public List<MetadataTargetValueType> MetadataTargetValue
        {
            get
            {
                return this.metadataTargetValueField;
            }
            set
            {
                this.metadataTargetValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportStructureValue", Order = 10)]
        public List<ReportStructureValueType> ReportStructureValue
        {
            get
            {
                return this.reportStructureValueField;
            }
            set
            {
                this.reportStructureValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraint", Order = 11)]
        public List<AttachmentConstraintReferenceType> AttachmentConstraint
        {
            get
            {
                return this.attachmentConstraintField;
            }
            set
            {
                this.attachmentConstraintField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachedObject", Order = 12)]
        public List<ObjectReferenceType> AttachedObject
        {
            get
            {
                return this.attachedObjectField;
            }
            set
            {
                this.attachedObjectField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachedDataKey", Order = 13)]
        public List<DataKeyType> AttachedDataKey
        {
            get
            {
                return this.attachedDataKeyField;
            }
            set
            {
                this.attachedDataKeyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachedDataSet", Order = 14)]
        public List<SetReferenceType> AttachedDataSet
        {
            get
            {
                return this.attachedDataSetField;
            }
            set
            {
                this.attachedDataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AttachedReportingPeriod", Order = 15)]
        public List<TimeRangeValueType> AttachedReportingPeriod
        {
            get
            {
                return this.attachedReportingPeriodField;
            }
            set
            {
                this.attachedReportingPeriodField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Or", Order = 16)]
        public List<MetadataParametersOrType> Or
        {
            get
            {
                return this.orField;
            }
            set
            {
                this.orField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("And", Order = 17)]
        public List<MetadataParametersAndType> And
        {
            get
            {
                return this.andField;
            }
            set
            {
                this.andField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataTargetValueType
    {

        private string idField;

        private List<TargetObjectValueType> targetObjectValueField;

        public MetadataTargetValueType()
        {
            this.targetObjectValueField = new List<TargetObjectValueType>();
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

        [System.Xml.Serialization.XmlElementAttribute("TargetObjectValue", Order = 1)]
        public List<TargetObjectValueType> TargetObjectValue
        {
            get
            {
                return this.targetObjectValueField;
            }
            set
            {
                this.targetObjectValueField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class TargetObjectValueType
    {

        private string idField;

        private List<object> itemsField;

        public TargetObjectValueType()
        {
            this.itemsField = new List<object>();
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

        [System.Xml.Serialization.XmlElementAttribute("DataKey", typeof(DataKeyType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SetReferenceType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Object", typeof(ObjectReferenceType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TimeValue", typeof(TimePeriodValueType), Order = 1)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class ReportStructureValueType
    {

        private string idField;

        private List<MetadataAttributeValueType> metadataAttributeValueField;

        public ReportStructureValueType()
        {
            this.metadataAttributeValueField = new List<MetadataAttributeValueType>();
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeValue", Order = 1)]
        public List<MetadataAttributeValueType> MetadataAttributeValue
        {
            get
            {
                return this.metadataAttributeValueField;
            }
            set
            {
                this.metadataAttributeValueField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataAttributeValueType
    {

        private string idField;

        private List<object> itemsField;

        private List<MetadataAttributeValueType> metadataAttributeValueField;

        public MetadataAttributeValueType()
        {
            this.metadataAttributeValueField = new List<MetadataAttributeValueType>();
            this.itemsField = new List<object>();
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

        [System.Xml.Serialization.XmlElementAttribute("NumericValue", typeof(NumericValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TextValue", typeof(QueryTextType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("TimeValue", typeof(TimePeriodValueType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(SimpleValueType), Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataAttributeValue", Order = 2)]
        public List<MetadataAttributeValueType> MetadataAttributeValue
        {
            get
            {
                return this.metadataAttributeValueField;
            }
            set
            {
                this.metadataAttributeValueField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataParametersOrType : MetadataParametersType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class DataSchemaQueryType
    {

        private DataStructureRequestType dataStructureField;

        public DataSchemaQueryType()
        {
            this.dataStructureField = new DataStructureRequestType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DataStructureRequestType DataStructure
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query", IsNullable = true)]
    public class MetadataSchemaQueryType
    {

        private GenericMetadataStructureType metadataStructureField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GenericMetadataStructureType MetadataStructure
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
}
