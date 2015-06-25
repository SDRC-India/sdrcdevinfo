using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Registry;

namespace SDMXObjectModel.Common
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = false)]
    public class TextType
    {
        #region "Variables"

        #region "Private"

        private string langField;

        private string valueField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public TextType()
            : this(null, null)
        {
        }

        public TextType(string language, string value)
        {
            this.langField = language;
            this.valueField = value;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute("StructuredText", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = false)]
    public class XHTMLType
    {

        private List<System.Xml.XmlNode> anyField;

        private string langField;

        public XHTMLType()
        {
            this.anyField = new List<System.Xml.XmlNode>();
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.w3.org/1999/xhtml", Order = 0)]
        public List<System.Xml.XmlNode> Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute("Annotations", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = false)]
    public class AnnotationsType
    {

        private List<AnnotationType> annotationField;

        public AnnotationsType()
        {
            this.annotationField = new List<AnnotationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Annotation", Order = 0)]
        public List<AnnotationType> Annotation
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AnnotationType
    {
        #region "Variables"

        #region "Private"

        private string annotationTitleField;

        private string annotationTypeField;

        private string annotationURLField;

        private List<TextType> annotationTextField;

        private string idField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AnnotationTitle
        {
            get
            {
                return this.annotationTitleField;
            }
            set
            {
                this.annotationTitleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AnnotationType", Order = 1)]
        public string AnnotationTypeElement
        {
            get
            {
                return this.annotationTypeField;
            }
            set
            {
                this.annotationTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
        public string AnnotationURL
        {
            get
            {
                return this.annotationURLField;
            }
            set
            {
                this.annotationURLField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AnnotationText", Order = 3)]
        public List<TextType> AnnotationText
        {
            get
            {
                return this.annotationTextField;
            }
            set
            {
                this.annotationTextField = value;
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

        #region "Cosntructors"

        public AnnotationType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public AnnotationType(string title, string text, string language)
        {
            this.annotationTitleField = title;

            if (!string.IsNullOrEmpty(text))
            {
                this.annotationTextField = new List<TextType>();
                this.annotationTextField.Add(new TextType(language, text));
            }
            else
            {
                this.annotationTextField = null;
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute("Any", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = false)]
    public class EmptyType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodedStatusMessageType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StatusMessageType
    {
        #region "Variables"

        #region "Private"

        private List<TextType> textField;

        private string codeField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Text", Order = 0)]
        public List<TextType> Text
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public StatusMessageType()
            : this(null, string.Empty, string.Empty)
        {
        }

        public StatusMessageType(string code, string value, string language)
        {
            this.codeField = code;
            if (!string.IsNullOrEmpty(value))
            {
                this.textField = new List<TextType>();
                this.textField.Add(new TextType(language, value));
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodedStatusMessageType : StatusMessageType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class AnnotableType
    {

        private List<AnnotationType> annotationsField;

        public AnnotableType()
        {
            this.annotationsField = new List<AnnotationType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Annotation", IsNullable = false)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReferencePeriodType
    {

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime startTime
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
        public System.DateTime endTime
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class QueryableDataSourceType
    {

        private string dataURLField;

        private string wSDLURLField;

        private string wADLURLField;

        private bool isRESTDatasourceField;

        private bool isWebServiceDatasourceField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string DataURL
        {
            get
            {
                return this.dataURLField;
            }
            set
            {
                this.dataURLField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 1)]
        public string WSDLURL
        {
            get
            {
                return this.wSDLURLField;
            }
            set
            {
                this.wSDLURLField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
        public string WADLURL
        {
            get
            {
                return this.wADLURLField;
            }
            set
            {
                this.wADLURLField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isRESTDatasource
        {
            get
            {
                return this.isRESTDatasourceField;
            }
            set
            {
                this.isRESTDatasourceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isWebServiceDatasource
        {
            get
            {
                return this.isWebServiceDatasourceField;
            }
            set
            {
                this.isWebServiceDatasourceField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetRegionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CubeRegionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DistinctKeyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataKeyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataKeyType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class RegionType
    {

        private List<ComponentValueSetType> keyValueField;

        private List<ComponentValueSetType> attributeField;

        private bool includeField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public RegionType()
        {
            this.anyAttrField = new List<System.Xml.XmlAttribute>();
            this.attributeField = new List<ComponentValueSetType>();
            this.keyValueField = new List<ComponentValueSetType>();
            this.includeField = true;
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyValue", Order = 0)]
        public List<ComponentValueSetType> KeyValue
        {
            get
            {
                return this.keyValueField;
            }
            set
            {
                this.keyValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", Order = 1)]
        public List<ComponentValueSetType> Attribute
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool include
        {
            get
            {
                return this.includeField;
            }
            set
            {
                this.includeField = value;
            }
        }

        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public List<System.Xml.XmlAttribute> AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeValueSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeValueSetType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetRegionKeyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CubeRegionKeyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DinstinctKeyValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataKeyValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataKeyValueType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ComponentValueSetType
    {

        private List<object> itemsField;

        private string idField;

        private bool includeField;

        public ComponentValueSetType()
        {
            this.itemsField = new List<object>();
            this.includeField = true;
        }

        [System.Xml.Serialization.XmlElementAttribute("DataKey", typeof(DataKeyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Object", typeof(ObjectReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("TimeRange", typeof(TimeRangeValueType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(SimpleValueType), Order = 0)]
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
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool include
        {
            get
            {
                return this.includeField;
            }
            set
            {
                this.includeField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataKeyType : DistinctKeyType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataKeyType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataKeyType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class DistinctKeyType : RegionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataKeyType : DistinctKeyType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class SetReferenceType
    {

        private DataProviderReferenceType dataProviderField;

        private string idField;

        public SetReferenceType()
        {
            this.dataProviderField = new DataProviderReferenceType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataProviderReferenceType : OrganisationReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class OrganisationReferenceBaseType : ItemReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ItemReferenceType : ChildObjectReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ChildObjectReferenceType : ReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodelistMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalLevelReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyLocalCodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataStructureComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataStructureComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalGroupKeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalItemReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportingCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalConceptReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContainerChildObjectReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChildObjectReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalIdentifiableReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalProcessStepReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyCodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(URNReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ReferenceType
    {

        private List<object> itemsField;

        public ReferenceType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Ref", typeof(RefBaseType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("URN", typeof(string), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "anyURI", Order = 0)]
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyLocalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalIdentifiableRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodelistMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalProcessStepRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalLevelRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalGroupKeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalItemRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportingCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContainerChildObjectRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChildObjectRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyCodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObjectRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class RefBaseType
    {

        private string agencyIDField;

        private string maintainableParentIDField;

        private string maintainableParentVersionField;

        private string containerIDField;

        private string idField;

        private string versionField;

        private bool localField;

        private bool localFieldSpecified;

        private ObjectTypeCodelistType classField;

        private bool classFieldSpecified;

        private PackageTypeCodelistType packageField;

        private bool packageFieldSpecified;

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
        public string maintainableParentID
        {
            get
            {
                return this.maintainableParentIDField;
            }
            set
            {
                this.maintainableParentIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string maintainableParentVersion
        {
            get
            {
                return this.maintainableParentVersionField;
            }
            set
            {
                this.maintainableParentVersionField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string containerID
        {
            get
            {
                return this.containerIDField;
            }
            set
            {
                this.containerIDField = value;
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool local
        {
            get
            {
                return this.localField;
            }
            set
            {
                this.localField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool localSpecified
        {
            get
            {
                return this.localFieldSpecified;
            }
            set
            {
                this.localFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ObjectTypeCodelistType @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classSpecified
        {
            get
            {
                return this.classFieldSpecified;
            }
            set
            {
                this.classFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PackageTypeCodelistType package
        {
            get
            {
                return this.packageField;
            }
            set
            {
                this.packageField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool packageSpecified
        {
            get
            {
                return this.packageFieldSpecified;
            }
            set
            {
                this.packageFieldSpecified = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum ObjectTypeCodelistType
    {

        /// <remarks/>
        Any,

        /// <remarks/>
        Agency,

        /// <remarks/>
        AgencyScheme,

        /// <remarks/>
        AttachmentConstraint,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        AttributeDescriptor,

        /// <remarks/>
        Categorisation,

        /// <remarks/>
        Category,

        /// <remarks/>
        CategorySchemeMap,

        /// <remarks/>
        CategoryScheme,

        /// <remarks/>
        Code,

        /// <remarks/>
        CodeMap,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        CodelistMap,

        /// <remarks/>
        ComponentMap,

        /// <remarks/>
        Concept,

        /// <remarks/>
        ConceptMap,

        /// <remarks/>
        ConceptScheme,

        /// <remarks/>
        ConceptSchemeMap,

        /// <remarks/>
        Constraint,

        /// <remarks/>
        ConstraintTarget,

        /// <remarks/>
        ContentConstraint,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        DataConsumer,

        /// <remarks/>
        DataConsumerScheme,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        DataProviderScheme,

        /// <remarks/>
        DataSetTarget,

        /// <remarks/>
        DataStructure,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        DimensionDescriptor,

        /// <remarks/>
        DimensionDescriptorValuesTarget,

        /// <remarks/>
        GroupDimensionDescriptor,

        /// <remarks/>
        HierarchicalCode,

        /// <remarks/>
        HierarchicalCodelist,

        /// <remarks/>
        Hierarchy,

        /// <remarks/>
        HybridCodelistMap,

        /// <remarks/>
        HybridCodeMap,

        /// <remarks/>
        IdentifiableObjectTarget,

        /// <remarks/>
        Level,

        /// <remarks/>
        MeasureDescriptor,

        /// <remarks/>
        MeasureDimension,

        /// <remarks/>
        Metadataflow,

        /// <remarks/>
        MetadataAttribute,

        /// <remarks/>
        MetadataSet,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        MetadataTarget,

        /// <remarks/>
        Organisation,

        /// <remarks/>
        OrganisationMap,

        /// <remarks/>
        OrganisationScheme,

        /// <remarks/>
        OrganisationSchemeMap,

        /// <remarks/>
        OrganisationUnit,

        /// <remarks/>
        OrganisationUnitScheme,

        /// <remarks/>
        PrimaryMeasure,

        /// <remarks/>
        Process,

        /// <remarks/>
        ProcessStep,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        ReportingCategory,

        /// <remarks/>
        ReportingCategoryMap,

        /// <remarks/>
        ReportingTaxonomy,

        /// <remarks/>
        ReportingTaxonomyMap,

        /// <remarks/>
        ReportingYearStartDay,

        /// <remarks/>
        ReportPeriodTarget,

        /// <remarks/>
        ReportStructure,

        /// <remarks/>
        StructureMap,

        /// <remarks/>
        StructureSet,

        /// <remarks/>
        TimeDimension,

        /// <remarks/>
        Transition,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum PackageTypeCodelistType
    {

        /// <remarks/>
        @base,

        /// <remarks/>
        datastructure,

        /// <remarks/>
        metadatastructure,

        /// <remarks/>
        process,

        /// <remarks/>
        registry,

        /// <remarks/>
        mapping,

        /// <remarks/>
        codelist,

        /// <remarks/>
        categoryscheme,

        /// <remarks/>
        conceptscheme,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AnyLocalCodeRefType : RefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodelistMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalProcessStepRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalLevelRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalGroupKeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalItemRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportingCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCategoryRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalIdentifiableRefBaseType : RefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCodelistMapRefType : LocalIdentifiableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalProcessStepRefType : LocalIdentifiableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalLevelRefType : LocalIdentifiableRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataStructureComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentListComponentRefBaseType : LocalIdentifiableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalMetadataStructureComponentRefType : LocalComponentListComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataStructureComponentRefType : LocalComponentListComponentRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentRefBaseType : LocalComponentListComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalTargetObjectRefType : LocalComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalPrimaryMeasureRefType : LocalComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDimensionRefType : LocalComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalComponentRefType : LocalComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalComponentListComponentRefType : LocalComponentListComponentRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalGroupKeyDescriptorRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentListRefBaseType : LocalIdentifiableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalReportStructureRefType : LocalComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalMetadataTargetRefType : LocalComponentListRefBaseType
    {
        #region "Constructors"

        public LocalMetadataTargetRefType()
            : this(string.Empty, ObjectTypeCodelistType.Any, false, PackageTypeCodelistType.@base, false, false, false)
        {
        }

        public LocalMetadataTargetRefType(string id, ObjectTypeCodelistType classType, bool classSpecified,
                                   PackageTypeCodelistType packageType, bool packageSpecified, bool local, bool localSpecified)
        {
            this.id = id;
            this.@class = classType;
            this.classSpecified = classSpecified;
            this.package = packageType;
            this.packageSpecified = packageSpecified;
            this.local = local;
            this.localSpecified = localSpecified;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalGroupKeyDescriptorRefType : LocalComponentListRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportingCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCategoryRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalItemRefBaseType : LocalIdentifiableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalReportingCategoryRefType : LocalItemRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalOrganisationRefBaseType : LocalItemRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataProviderRefType : LocalOrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataConsumerRefType : LocalOrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalAgencyRefType : LocalOrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalOrganisationUnitRefType : LocalOrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalOrganisationRefType : LocalOrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalConceptRefType : LocalItemRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCodeRefType : LocalItemRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCategoryRefType : LocalItemRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ContainerChildObjectRefBaseType : RefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TransitionRefType : ContainerChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchicalCodeRefType : ContainerChildObjectRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ComponentRefBaseType : ContainerChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataAttributeRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class IdentifiableObjectTargetRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportPeriodTargetRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class KeyDescriptorValuesTargetRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataSetTargetRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConstraintTargetRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class PrimaryMeasureRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttributeRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimeDimensionRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MeasureDimensionRefType : ComponentRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DimensionRefType : ComponentRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureMapRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessStepRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LevelRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentListRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ChildObjectRefBaseType : RefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationSchemeMapRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptSchemeMapRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodelistMapRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorySchemeMapRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureMapRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProcessStepRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LevelRefType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchyRefType : ChildObjectRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ComponentListRefBaseType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportStructureRefType : ComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataTargetRefType : ComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GroupKeyDescriptorRefType : ComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MeasureDescriptorRefType : ComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttributeDescriptorRefType : ComponentListRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class KeyDescriptorRefType : ComponentListRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportCategoryRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategoryRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ItemRefBaseType : ChildObjectRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportCategoryRefType : ItemRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class OrganisationRefBaseType : ItemRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataProviderRefType : OrganisationRefBaseType
    {
        #region "--Constructors--"

        public DataProviderRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public DataProviderRefType(string id, string agencyId, string maintenableParentId, string maintenableParentVersion)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.maintainableParentID = maintenableParentId;
            this.maintainableParentVersion = maintenableParentVersion;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataConsumerRefType : OrganisationRefBaseType
    {
        #region "--Constructors--"

        public DataConsumerRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public DataConsumerRefType(string id, string agencyId, string maintenableParentId, string maintenableParentVersion)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.maintainableParentID = maintenableParentId;
            this.maintainableParentVersion = maintenableParentVersion;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AgencyRefType : OrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationUnitRefType : OrganisationRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationRefType : OrganisationRefBaseType
    {
        #region "--Constructors--"

        public OrganisationRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public OrganisationRefType(string id, string agencyId, string maintenableParentId, string maintenableParentVersion)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.maintainableParentID = maintenableParentId;
            this.maintainableParentVersion = maintenableParentVersion;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptRefType : ItemRefBaseType
    {
        #region "--Constructors--"

        public ConceptRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public ConceptRefType(string id, string agencyId, string maintenableParentId, string maintenableParentVersion)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.maintainableParentID = maintenableParentId;
            this.maintainableParentVersion = maintenableParentVersion;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodeRefType : ItemRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategoryRefType : ItemRefBaseType
    {
        #region "--Constructors--"

        public CategoryRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public CategoryRefType(string id, string agencyId, string maintenableParentId, string maintenableParentVersion)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.maintainableParentID = maintenableParentId;
            this.maintainableParentVersion = maintenableParentVersion;
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyCodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class MaintainableRefBaseType : RefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureSetRefType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProcessRefType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProvisionAgreementRefType : MaintainableRefBaseType
    {
        #region "--Constructors--"

        public ProvisionAgreementRefType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public ProvisionAgreementRefType(string id, string agencyId, string version)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ConstraintRefType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ContentConstraintRefType : ConstraintRefType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttachmentConstraintRefType : ConstraintRefType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchicalCodelistRefType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorisationRefType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AnyCodelistRefType : MaintainableRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class StructureOrUsageRefBaseType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureOrUsageRefType : StructureOrUsageRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class StructureUsageRefBaseType : StructureOrUsageRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataflowRefType : StructureUsageRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataflowRefType : StructureUsageRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureUsageRefType : StructureUsageRefBaseType
    {
        #region "--Constructors--"

        public StructureUsageRefType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public StructureUsageRefType(string id, string agencyId, string version)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
        }

        #endregion "--Constructors--"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class StructureRefBaseType : StructureOrUsageRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataStructureRefType : StructureRefBaseType
    {
        #region "--Constructors--"

        public MetadataStructureRefType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public MetadataStructureRefType(string id, string agencyId, string version)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataStructureRefType : StructureRefBaseType
    {
        #region "--Constructors--"

        public DataStructureRefType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public DataStructureRefType(string id, string agencyId, string version)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureRefType : StructureRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ItemSchemeRefBaseType : MaintainableRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportingTaxonomyRefType : ItemSchemeRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeRefType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class OrganisationSchemeRefBaseType : ItemSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataProviderSchemeRefType : OrganisationSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataConsumerSchemeRefType : OrganisationSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AgencySchemeRefType : OrganisationSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationUnitSchemeRefType : OrganisationSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationSchemeRefType : OrganisationSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptSchemeRefType : ItemSchemeRefBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodelistRefType : ItemSchemeRefBaseType
    {
        #region "--Constructors--"

        public CodelistRefType()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }

        public CodelistRefType(string id, string agencyId, string version)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorySchemeRefType : ItemSchemeRefBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeRefType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ItemSchemeRefType : ItemSchemeRefBaseType
    {
        #region "--Constructors--"

        public ItemSchemeRefType()
            : this(string.Empty, string.Empty, string.Empty, PackageTypeCodelistType.@base, false, ObjectTypeCodelistType.Any, false, false, false)
        {
        }

        public ItemSchemeRefType(string id, string agencyId, string version, PackageTypeCodelistType packageType, bool packageTypeSpecified, ObjectTypeCodelistType classType, bool classTypeSpecified, bool local, bool localSpecified)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
            
            this.package = packageType;
            this.packageSpecified = packageTypeSpecified;

            this.@class = classType;
            this.classSpecified = classTypeSpecified;

            this.local = local;
            this.localSpecified = localSpecified;
        }

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataStructureEnumerationSchemeRefType : ItemSchemeRefType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MaintainableRefType : MaintainableRefBaseType
    {
        #region "Constructors"

        public MaintainableRefType()
            : this(string.Empty, string.Empty, string.Empty, ObjectTypeCodelistType.Any, false,
                                     PackageTypeCodelistType.@base, false)
        {
        }

        public MaintainableRefType(string id, string agencyId, string version, ObjectTypeCodelistType classType, bool classSpecified,
                                   PackageTypeCodelistType packageType, bool packageSpecified)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
            this.@class = classType;
            this.classSpecified = classSpecified;
            this.package = packageType;
            this.packageSpecified = packageSpecified;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ObjectRefType : RefBaseType
    {
        #region "Constructors"

        public ObjectRefType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, ObjectTypeCodelistType.Any, false,
                                     PackageTypeCodelistType.@base, false)
        {
        }

        public ObjectRefType(string id, string agencyId, string version, string maintainableParentID, string maintainableParentVersion, ObjectTypeCodelistType classType, bool classSpecified, PackageTypeCodelistType packageType, bool packageSpecified)
        {
            this.id = id;
            this.agencyID = agencyId;
            this.version = version;
            this.maintainableParentID = maintainableParentID;
            this.maintainableParentVersion = maintainableParentVersion;
            this.@class = classType;
            this.classSpecified = classSpecified;
            this.package = packageType;
            this.packageSpecified = packageSpecified;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCodelistMapReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalLevelReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AnyLocalCodeReferenceType : ReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataStructureComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataStructureComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalComponentListComponentReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentListComponentReferenceBaseType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalMetadataStructureComponentReferenceType : LocalComponentListComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataStructureComponentReferenceType : LocalComponentListComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalComponentReferenceType : LocalComponentListComponentReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalTargetObjectReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalPrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDimensionReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentReferenceBaseType : LocalComponentListComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalTargetObjectReferenceType : LocalComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalPrimaryMeasureReferenceType : LocalComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDimensionReferenceType : LocalComponentReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalComponentListComponentReferenceType : LocalComponentListComponentReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalMetadataTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalGroupKeyDescriptorReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalComponentListReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalReportStructureReferenceType : LocalComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalMetadataTargetReferenceType : LocalComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalGroupKeyDescriptorReferenceType : LocalComponentListReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalReportingCategoryReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalConceptReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalCategoryReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalItemReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalReportingCategoryReferenceType : LocalItemReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataProviderReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalDataConsumerReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalAgencyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationUnitReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalOrganisationReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalOrganisationReferenceBaseType : LocalItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataProviderReferenceType : LocalOrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalDataConsumerReferenceType : LocalOrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalAgencyReferenceType : LocalOrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalOrganisationUnitReferenceType : LocalOrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalOrganisationReferenceType : LocalOrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalConceptReferenceType : LocalItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCodeReferenceType : LocalItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalCategoryReferenceType : LocalItemReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransitionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ContainerChildObjectReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TransitionReferenceType : ContainerChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchicalCodeReferenceType : ContainerChildObjectReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataAttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IdentifiableObjectTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportPeriodTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorValuesTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataSetTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PrimaryMeasureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDimensionReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DimensionReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ComponentReferenceType : ContainerChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataAttributeReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class IdentifiableObjectTargetReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportPeriodTargetReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class KeyDescriptorValuesTargetReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataSetTargetReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConstraintTargetReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class PrimaryMeasureReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttributeReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimeDimensionReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MeasureDimensionReferenceType : ComponentReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DimensionReferenceType : ComponentReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LocalProcessStepReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class LocalIdentifiableReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LocalProcessStepReferenceType : LocalIdentifiableReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProcessReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ProvisionAgreementReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HierarchicalCodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AnyCodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorisationReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureOrUsageReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class MaintainableReferenceBaseType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureSetReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProcessReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProvisionAgreementReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContentConstraintReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttachmentConstraintReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConstraintReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ContentConstraintReferenceType : ConstraintReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttachmentConstraintReferenceType : ConstraintReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchicalCodelistReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AnyCodelistReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorisationReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureOrUsageReferenceType : MaintainableReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportingTaxonomyReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceBaseType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConceptSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CodelistReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CategorySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ItemSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ItemSchemeReferenceBaseType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportingTaxonomyReferenceType : ItemSchemeReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataProviderSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataConsumerSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AgencySchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationUnitSchemeReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OrganisationSchemeReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class OrganisationSchemeReferenceBaseType : ItemSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataProviderSchemeReferenceType : OrganisationSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataConsumerSchemeReferenceType : OrganisationSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AgencySchemeReferenceType : OrganisationSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationUnitSchemeReferenceType : OrganisationSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationSchemeReferenceType : OrganisationSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptSchemeReferenceType : ItemSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodelistReferenceType : ItemSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorySchemeReferenceType : ItemSchemeReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureEnumerationSchemeReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ItemSchemeReferenceType : ItemSchemeReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataStructureEnumerationSchemeReferenceType : ItemSchemeReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataflowReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureUsageReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class StructureUsageReferenceBaseType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataflowReferenceType : StructureUsageReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataflowReferenceType : StructureUsageReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureUsageReferenceType : StructureUsageReferenceBaseType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class StructureReferenceBaseType : MaintainableReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataStructureReferenceType : StructureReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataStructureReferenceType : StructureReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureReferenceType : StructureReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MaintainableReferenceType : MaintainableReferenceBaseType
    {
        #region "Constructors"

        public MaintainableReferenceType()
            : this(null)
        {
        }

        public MaintainableReferenceType(MaintainableRefType maintainableRef)
        {
            if (maintainableRef != null)
            {
                this.Items = new List<object>();
                this.Items.Add(maintainableRef);
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class URNReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ObjectReferenceType : ReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationSchemeMapReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptSchemeMapReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodelistMapReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategorySchemeMapReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureMapReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ProcessStepReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class LevelReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class HierarchyReferenceType : ChildObjectReferenceType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReportStructureReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataTargetReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GroupKeyDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MeasureDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttributeDescriptorReferenceType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(KeyDescriptorReferenceType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class ComponentListReferenceType : ChildObjectReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportStructureReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataTargetReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GroupKeyDescriptorReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MeasureDescriptorReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttributeDescriptorReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class KeyDescriptorReferenceType : ComponentListReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ReportingCategoryReferenceType : ItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ConceptReferenceType : ItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CodeReferenceType : ItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CategoryReferenceType : ItemReferenceType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataConsumerReferenceType : OrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AgencyReferenceType : OrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationUnitReferenceType : OrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class OrganisationReferenceType : OrganisationReferenceBaseType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimeRangeValueType
    {

        private TimePeriodRangeType[] itemsField;

        private TimeRangeValueChoiceType[] itemsElementNameField;

        public TimeRangeValueType()
        {
            //this.itemsElementNameField = new List<TimeRangeValueChoiceType>();
            //this.itemsField = new List<TimePeriodRangeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AfterPeriod", typeof(TimePeriodRangeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("BeforePeriod", typeof(TimePeriodRangeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("EndPeriod", typeof(TimePeriodRangeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StartPeriod", typeof(TimePeriodRangeType), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public TimePeriodRangeType[] Items
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
        public TimeRangeValueChoiceType[] ItemsElementName
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimePeriodRangeType
    {

        private bool isInclusiveField;

        private string valueField;

        public TimePeriodRangeType()
        {
            this.isInclusiveField = true;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool isInclusive
        {
            get
            {
                return this.isInclusiveField;
            }
            set
            {
                this.isInclusiveField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IncludeInSchema = false)]
    public enum TimeRangeValueChoiceType
    {

        /// <remarks/>
        AfterPeriod,

        /// <remarks/>
        BeforePeriod,

        /// <remarks/>
        EndPeriod,

        /// <remarks/>
        StartPeriod,
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SimpleKeyValueType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class SimpleValueType
    {

        private bool cascadeValuesField;

        private string valueField;

        public SimpleValueType()
        {
            this.cascadeValuesField = false;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool cascadeValues
        {
            get
            {
                return this.cascadeValuesField;
            }
            set
            {
                this.cascadeValuesField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class SimpleKeyValueType : SimpleValueType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataAttributeValueSetType : ComponentValueSetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class AttributeValueSetType : ComponentValueSetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataTargetRegionKeyType : ComponentValueSetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CubeRegionKeyType : ComponentValueSetType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataKeyValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataKeyValueType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class DinstinctKeyValueType : ComponentValueSetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataKeyValueType : DinstinctKeyValueType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataKeyValueType : DinstinctKeyValueType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MetadataTargetRegionType : RegionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class CubeRegionType : RegionType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericMetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificMetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificDataTimeSeriesStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesGenericDataStructureRequestType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class PayloadStructureType
    {

        private MaintainableReferenceBaseType itemField;

        private string structureIDField;

        private string schemaURLField;

        private string namespaceField;

        private string dimensionAtObservationField;

        private bool explicitMeasuresField;

        private bool explicitMeasuresFieldSpecified;

        private string serviceURLField;

        private string structureURLField;

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgrement", typeof(ProvisionAgreementReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(StructureReferenceBaseType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureUsage", typeof(StructureUsageReferenceBaseType), Order = 0)]
        public MaintainableReferenceBaseType Item
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string structureID
        {
            get
            {
                return this.structureIDField;
            }
            set
            {
                this.structureIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string schemaURL
        {
            get
            {
                return this.schemaURLField;
            }
            set
            {
                this.schemaURLField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string @namespace
        {
            get
            {
                return this.namespaceField;
            }
            set
            {
                this.namespaceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dimensionAtObservation
        {
            get
            {
                return this.dimensionAtObservationField;
            }
            set
            {
                this.dimensionAtObservationField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool explicitMeasures
        {
            get
            {
                return this.explicitMeasuresField;
            }
            set
            {
                this.explicitMeasuresField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool explicitMeasuresSpecified
        {
            get
            {
                return this.explicitMeasuresFieldSpecified;
            }
            set
            {
                this.explicitMeasuresFieldSpecified = value;
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericMetadataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificMetadataStructureType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class MetadataStructureType : PayloadStructureType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GenericMetadataStructureType : MetadataStructureType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureSpecificMetadataStructureType : MetadataStructureType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificDataTimeSeriesStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataStructureType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesGenericDataStructureRequestType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public abstract class DataStructureType : PayloadStructureType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureSpecificDataTimeSeriesStructureType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureSpecificDataStructureType : DataStructureType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class StructureSpecificDataTimeSeriesStructureType : StructureSpecificDataStructureType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTimeSeriesDataStructureType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GenericDataStructureType : DataStructureType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GenericTimeSeriesDataStructureType : GenericDataStructureType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericDataStructureRequestType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesGenericDataStructureRequestType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class DataStructureRequestType : DataStructureType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimeSeriesDataStructureRequestType : DataStructureRequestType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesGenericDataStructureRequestType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class GenericDataStructureRequestType : DataStructureRequestType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class TimeSeriesGenericDataStructureRequestType : GenericDataStructureRequestType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableObjectTypeListType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class ObjectTypeListType
    {

        private EmptyType anyField;

        private EmptyType agencyField;

        private EmptyType agencySchemeField;

        private EmptyType attachmentConstraintField;

        private EmptyType attributeField;

        private EmptyType attributeDescriptorField;

        private EmptyType categorisationField;

        private EmptyType categoryField;

        private EmptyType categorySchemeMapField;

        private EmptyType categorySchemeField;

        private EmptyType codeField;

        private EmptyType codeMapField;

        private EmptyType codelistField;

        private EmptyType codelistMapField;

        private EmptyType componentMapField;

        private EmptyType conceptField;

        private EmptyType conceptMapField;

        private EmptyType conceptSchemeField;

        private EmptyType conceptSchemeMapField;

        private EmptyType contentConstraintField;

        private EmptyType dataflowField;

        private EmptyType dataConsumerField;

        private EmptyType dataConsumerSchemeField;

        private EmptyType dataProviderField;

        private EmptyType dataProviderSchemeField;

        private EmptyType dataSetTargetField;

        private EmptyType dataStructureField;

        private EmptyType dimensionField;

        private EmptyType dimensionDescriptorField;

        private EmptyType dimensionDescriptorValuesTargetField;

        private EmptyType groupDimensionDescriptorField;

        private EmptyType hierarchicalCodeField;

        private EmptyType hierarchicalCodelistField;

        private EmptyType hierarchyField;

        private EmptyType hybridCodelistMapField;

        private EmptyType hybridCodeMapField;

        private EmptyType identifiableObjectTargetField;

        private EmptyType levelField;

        private EmptyType measureDescriptorField;

        private EmptyType measureDimensionField;

        private EmptyType metadataflowField;

        private EmptyType metadataAttributeField;

        private EmptyType metadataSetField;

        private EmptyType metadataStructureField;

        private EmptyType metadataTargetField;

        private EmptyType organisationMapField;

        private EmptyType organisationSchemeMapField;

        private EmptyType organisationUnitField;

        private EmptyType organisationUnitSchemeField;

        private EmptyType primaryMeasureField;

        private EmptyType processField;

        private EmptyType processStepField;

        private EmptyType provisionAgreementField;

        private EmptyType reportingCategoryField;

        private EmptyType reportingCategoryMapField;

        private EmptyType reportingTaxonomyField;

        private EmptyType reportingTaxonomyMapField;

        private EmptyType reportPeriodTargetField;

        private EmptyType reportStructureField;

        private EmptyType structureMapField;

        private EmptyType structureSetField;

        private EmptyType timeDimensionField;

        private EmptyType transitionField;

        public ObjectTypeListType()
        {
            this.transitionField = new EmptyType();
            this.timeDimensionField = new EmptyType();
            this.structureSetField = new EmptyType();
            this.structureMapField = new EmptyType();
            this.reportStructureField = new EmptyType();
            this.reportPeriodTargetField = new EmptyType();
            this.reportingTaxonomyMapField = new EmptyType();
            this.reportingTaxonomyField = new EmptyType();
            this.reportingCategoryMapField = new EmptyType();
            this.reportingCategoryField = new EmptyType();
            this.provisionAgreementField = new EmptyType();
            this.processStepField = new EmptyType();
            this.processField = new EmptyType();
            this.primaryMeasureField = new EmptyType();
            this.organisationUnitSchemeField = new EmptyType();
            this.organisationUnitField = new EmptyType();
            this.organisationSchemeMapField = new EmptyType();
            this.organisationMapField = new EmptyType();
            this.metadataTargetField = new EmptyType();
            this.metadataStructureField = new EmptyType();
            this.metadataSetField = new EmptyType();
            this.metadataAttributeField = new EmptyType();
            this.metadataflowField = new EmptyType();
            this.measureDimensionField = new EmptyType();
            this.measureDescriptorField = new EmptyType();
            this.levelField = new EmptyType();
            this.identifiableObjectTargetField = new EmptyType();
            this.hybridCodeMapField = new EmptyType();
            this.hybridCodelistMapField = new EmptyType();
            this.hierarchyField = new EmptyType();
            this.hierarchicalCodelistField = new EmptyType();
            this.hierarchicalCodeField = new EmptyType();
            this.groupDimensionDescriptorField = new EmptyType();
            this.dimensionDescriptorValuesTargetField = new EmptyType();
            this.dimensionDescriptorField = new EmptyType();
            this.dimensionField = new EmptyType();
            this.dataStructureField = new EmptyType();
            this.dataSetTargetField = new EmptyType();
            this.dataProviderSchemeField = new EmptyType();
            this.dataProviderField = new EmptyType();
            this.dataConsumerSchemeField = new EmptyType();
            this.dataConsumerField = new EmptyType();
            this.dataflowField = new EmptyType();
            this.contentConstraintField = new EmptyType();
            this.conceptSchemeMapField = new EmptyType();
            this.conceptSchemeField = new EmptyType();
            this.conceptMapField = new EmptyType();
            this.conceptField = new EmptyType();
            this.componentMapField = new EmptyType();
            this.codelistMapField = new EmptyType();
            this.codelistField = new EmptyType();
            this.codeMapField = new EmptyType();
            this.codeField = new EmptyType();
            this.categorySchemeField = new EmptyType();
            this.categorySchemeMapField = new EmptyType();
            this.categoryField = new EmptyType();
            this.categorisationField = new EmptyType();
            this.attributeDescriptorField = new EmptyType();
            this.attributeField = new EmptyType();
            this.attachmentConstraintField = new EmptyType();
            this.agencySchemeField = new EmptyType();
            this.agencyField = new EmptyType();
            this.anyField = new EmptyType();
        }

        public EmptyType Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        public EmptyType Agency
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

        public EmptyType AgencyScheme
        {
            get
            {
                return this.agencySchemeField;
            }
            set
            {
                this.agencySchemeField = value;
            }
        }

        public EmptyType AttachmentConstraint
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

        public EmptyType Attribute
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

        public EmptyType AttributeDescriptor
        {
            get
            {
                return this.attributeDescriptorField;
            }
            set
            {
                this.attributeDescriptorField = value;
            }
        }

        public EmptyType Categorisation
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

        public EmptyType Category
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

        public EmptyType CategorySchemeMap
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

        public EmptyType CategoryScheme
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

        public EmptyType Code
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

        public EmptyType CodeMap
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

        public EmptyType Codelist
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

        public EmptyType CodelistMap
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

        public EmptyType ComponentMap
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

        public EmptyType Concept
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

        public EmptyType ConceptMap
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

        public EmptyType ConceptScheme
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

        public EmptyType ConceptSchemeMap
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

        public EmptyType ContentConstraint
        {
            get
            {
                return this.contentConstraintField;
            }
            set
            {
                this.contentConstraintField = value;
            }
        }

        public EmptyType Dataflow
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

        public EmptyType DataConsumer
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

        public EmptyType DataConsumerScheme
        {
            get
            {
                return this.dataConsumerSchemeField;
            }
            set
            {
                this.dataConsumerSchemeField = value;
            }
        }

        public EmptyType DataProvider
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

        public EmptyType DataProviderScheme
        {
            get
            {
                return this.dataProviderSchemeField;
            }
            set
            {
                this.dataProviderSchemeField = value;
            }
        }

        public EmptyType DataSetTarget
        {
            get
            {
                return this.dataSetTargetField;
            }
            set
            {
                this.dataSetTargetField = value;
            }
        }

        public EmptyType DataStructure
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

        public EmptyType Dimension
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

        public EmptyType DimensionDescriptor
        {
            get
            {
                return this.dimensionDescriptorField;
            }
            set
            {
                this.dimensionDescriptorField = value;
            }
        }

        public EmptyType DimensionDescriptorValuesTarget
        {
            get
            {
                return this.dimensionDescriptorValuesTargetField;
            }
            set
            {
                this.dimensionDescriptorValuesTargetField = value;
            }
        }

        public EmptyType GroupDimensionDescriptor
        {
            get
            {
                return this.groupDimensionDescriptorField;
            }
            set
            {
                this.groupDimensionDescriptorField = value;
            }
        }

        public EmptyType HierarchicalCode
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

        public EmptyType HierarchicalCodelist
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

        public EmptyType Hierarchy
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

        public EmptyType HybridCodelistMap
        {
            get
            {
                return this.hybridCodelistMapField;
            }
            set
            {
                this.hybridCodelistMapField = value;
            }
        }

        public EmptyType HybridCodeMap
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

        public EmptyType IdentifiableObjectTarget
        {
            get
            {
                return this.identifiableObjectTargetField;
            }
            set
            {
                this.identifiableObjectTargetField = value;
            }
        }

        public EmptyType Level
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

        public EmptyType MeasureDescriptor
        {
            get
            {
                return this.measureDescriptorField;
            }
            set
            {
                this.measureDescriptorField = value;
            }
        }

        public EmptyType MeasureDimension
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

        public EmptyType Metadataflow
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

        public EmptyType MetadataAttribute
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

        public EmptyType MetadataSet
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

        public EmptyType MetadataStructure
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

        public EmptyType MetadataTarget
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

        public EmptyType OrganisationMap
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

        public EmptyType OrganisationSchemeMap
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

        public EmptyType OrganisationUnit
        {
            get
            {
                return this.organisationUnitField;
            }
            set
            {
                this.organisationUnitField = value;
            }
        }

        public EmptyType OrganisationUnitScheme
        {
            get
            {
                return this.organisationUnitSchemeField;
            }
            set
            {
                this.organisationUnitSchemeField = value;
            }
        }

        public EmptyType PrimaryMeasure
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

        public EmptyType Process
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

        public EmptyType ProcessStep
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

        public EmptyType ProvisionAgreement
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

        public EmptyType ReportingCategory
        {
            get
            {
                return this.reportingCategoryField;
            }
            set
            {
                this.reportingCategoryField = value;
            }
        }

        public EmptyType ReportingCategoryMap
        {
            get
            {
                return this.reportingCategoryMapField;
            }
            set
            {
                this.reportingCategoryMapField = value;
            }
        }

        public EmptyType ReportingTaxonomy
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

        public EmptyType ReportingTaxonomyMap
        {
            get
            {
                return this.reportingTaxonomyMapField;
            }
            set
            {
                this.reportingTaxonomyMapField = value;
            }
        }

        public EmptyType ReportPeriodTarget
        {
            get
            {
                return this.reportPeriodTargetField;
            }
            set
            {
                this.reportPeriodTargetField = value;
            }
        }

        public EmptyType ReportStructure
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

        public EmptyType StructureMap
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

        public EmptyType StructureSet
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

        public EmptyType TimeDimension
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

        public EmptyType Transition
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", IsNullable = true)]
    public class MaintainableObjectTypeListType : ObjectTypeListType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum ActionType
    {

        /// <remarks/>
        Append,

        /// <remarks/>
        Replace,

        /// <remarks/>
        Delete,

        /// <remarks/>
        Information,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum MaintainableTypeCodelistType
    {

        /// <remarks/>
        Any,

        /// <remarks/>
        AgencyScheme,

        /// <remarks/>
        AttachmentConstraint,

        /// <remarks/>
        Categorisation,

        /// <remarks/>
        CategoryScheme,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        ConceptScheme,

        /// <remarks/>
        Constraint,

        /// <remarks/>
        ContentConstraint,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        DataConsumerScheme,

        /// <remarks/>
        DataProviderScheme,

        /// <remarks/>
        DataStructure,

        /// <remarks/>
        HierarchicalCodelist,

        /// <remarks/>
        Metadataflow,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        OrganisationScheme,

        /// <remarks/>
        OrganisationUnitScheme,

        /// <remarks/>
        Process,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        ReportingTaxonomy,

        /// <remarks/>
        StructureSet,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum TargetObjectTypeCodelistType
    {

        /// <remarks/>
        ConstraintTarget,

        /// <remarks/>
        DataSetTarget,

        /// <remarks/>
        IdentifiableObjectTarget,

        /// <remarks/>
        DimensionDescriptorValuesTarget,

        /// <remarks/>
        ReportPeriodTarget,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum TimeOperatorType
    {

        /// <remarks/>
        equal,

        /// <remarks/>
        greaterThanOrEqual,

        /// <remarks/>
        lessThanOrEqual,

        /// <remarks/>
        greaterThan,

        /// <remarks/>
        lessThan,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum SimpleOperatorType
    {

        /// <remarks/>
        notEqual,

        /// <remarks/>
        equal,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum TimeDataType
    {

        /// <remarks/>
        ObservationalTimePeriod,

        /// <remarks/>
        StandardTimePeriod,

        /// <remarks/>
        BasicTimePeriod,

        /// <remarks/>
        GregorianTimePeriod,

        /// <remarks/>
        GregorianYear,

        /// <remarks/>
        GregorianYearMonth,

        /// <remarks/>
        GregorianDay,

        /// <remarks/>
        ReportingTimePeriod,

        /// <remarks/>
        ReportingYear,

        /// <remarks/>
        ReportingSemester,

        /// <remarks/>
        ReportingTrimester,

        /// <remarks/>
        ReportingQuarter,

        /// <remarks/>
        ReportingMonth,

        /// <remarks/>
        ReportingWeek,

        /// <remarks/>
        ReportingDay,

        /// <remarks/>
        DateTime,

        /// <remarks/>
        TimeRange,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum DataType
    {
        String,

        /// <remarks/>
        Alpha,

        /// <remarks/>
        AlphaNumeric,

        /// <remarks/>
        Numeric,

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
        URI,

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

        /// <remarks/>
        StandardTimePeriod,

        /// <remarks/>
        BasicTimePeriod,

        /// <remarks/>
        GregorianTimePeriod,

        /// <remarks/>
        GregorianYear,

        /// <remarks/>
        GregorianYearMonth,

        /// <remarks/>
        GregorianDay,

        /// <remarks/>
        ReportingTimePeriod,

        /// <remarks/>
        ReportingYear,

        /// <remarks/>
        ReportingSemester,

        /// <remarks/>
        ReportingTrimester,

        /// <remarks/>
        ReportingQuarter,

        /// <remarks/>
        ReportingMonth,

        /// <remarks/>
        ReportingWeek,

        /// <remarks/>
        ReportingDay,

        /// <remarks/>
        DateTime,

        /// <remarks/>
        TimeRange,

        /// <remarks/>
        Month,

        /// <remarks/>
        MonthDay,

        /// <remarks/>
        Day,

        /// <remarks/>
        Time,

        /// <remarks/>
        Duration,

        /// <remarks/>
        XHTML,

        /// <remarks/>
        KeyValues,

        /// <remarks/>
        IdentifiableReference,

        /// <remarks/>
        DataSetReference,

        /// <remarks/>
        AttachmentConstraintReference,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum DimensionTypeType
    {

        /// <remarks/>
        Dimension,

        /// <remarks/>
        MeasureDimension,

        /// <remarks/>
        TimeDimension,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common")]
    public enum ContentConstraintTypeCodeType
    {

        /// <remarks/>
        Allowed,

        /// <remarks/>
        Actual,
    }
}
