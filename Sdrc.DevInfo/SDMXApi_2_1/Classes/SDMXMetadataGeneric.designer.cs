using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;

namespace SDMXObjectModel.Metadata.Generic
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class MetadataSetType : AnnotableType
    {

        private List<TextType> nameField;

        private DataProviderReferenceType dataProviderField;

        private List<ReportType> reportField;

        private string structureRefField;

        private string setIDField;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private string reportingBeginDateField;

        private string reportingEndDateField;

        private System.DateTime validFromDateField;

        private bool validFromDateFieldSpecified;

        private System.DateTime validToDateField;

        private bool validToDateFieldSpecified;

        private string publicationYearField;

        private string publicationPeriodField;

        public MetadataSetType()
        {
            this.reportField = new List<ReportType>();
            this.dataProviderField = new DataProviderReferenceType();
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

        [System.Xml.Serialization.XmlElementAttribute("Report", Order = 2)]
        public List<ReportType> Report
        {
            get
            {
                return this.reportField;
            }
            set
            {
                this.reportField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "IDREF")]
        public string structureRef
        {
            get
            {
                return this.structureRefField;
            }
            set
            {
                this.structureRefField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string setID
        {
            get
            {
                return this.setIDField;
            }
            set
            {
                this.setIDField = value;
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
        public System.DateTime validFromDate
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validFromDateSpecified
        {
            get
            {
                return this.validFromDateFieldSpecified;
            }
            set
            {
                this.validFromDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime validToDate
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validToDateSpecified
        {
            get
            {
                return this.validToDateFieldSpecified;
            }
            set
            {
                this.validToDateFieldSpecified = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class ReferenceValueType
    {

        private object itemField;

        private string idField;

        [System.Xml.Serialization.XmlElementAttribute("ConstraintContentReference", typeof(AttachmentConstraintReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataKey", typeof(DataKeyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSetReference", typeof(SetReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ObjectReference", typeof(ObjectReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportPeriod", typeof(string), Order = 0)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class TargetType
    {

        private List<ReferenceValueType> referenceValueField;

        private string idField;

        public TargetType()
        {
            this.referenceValueField = new List<ReferenceValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ReferenceValue", Order = 0)]
        public List<ReferenceValueType> ReferenceValue
        {
            get
            {
                return this.referenceValueField;
            }
            set
            {
                this.referenceValueField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class ReportedAttributeType : AnnotableType
    {

        private List<object> itemsField;

        //private List<ReportedAttributeType> attributeSetField;
        private AttributeSetType attributeSetField;

        private string idField;

        private string valueField;

        public ReportedAttributeType()
        {
            //this.attributeSetField = new List<ReportedAttributeType>();
            this.attributeSetField = new AttributeSetType();
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("StructuredText", typeof(XHTMLType), Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Text", typeof(TextType), Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
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

        //[System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        //[System.Xml.Serialization.XmlArrayItemAttribute("ReportedAttribute", IsNullable = false)]
        //public List<ReportedAttributeType> AttributeSet
        //{
        //    get
        //    {
        //        return this.attributeSetField;
        //    }
        //    set
        //    {
        //        this.attributeSetField = value;
        //    }
        //}
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AttributeSetType AttributeSet
        {
            get
            {
                return this.attributeSetField;
            }
            set
            {
                this.attributeSetField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class ReportType : AnnotableType
    {

        private TargetType targetField;

        //private List<ReportedAttributeType> attributeSetField;
        private AttributeSetType attributeSetField;

        private string idField;

        public ReportType()
        {
            //this.attributeSetField = new List<ReportedAttributeType>();
            this.attributeSetField = new AttributeSetType();
            this.targetField = new TargetType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public TargetType Target
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

        //[System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        //[System.Xml.Serialization.XmlArrayItemAttribute("ReportedAttribute", IsNullable = false)]
        //public List<ReportedAttributeType> AttributeSet
        //{
        //    get
        //    {
        //        return this.attributeSetField;
        //    }
        //    set
        //    {
        //        this.attributeSetField = value;
        //    }
        //}
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public AttributeSetType AttributeSet
        {
            get
            {
                return this.attributeSetField;
            }
            set
            {
                this.attributeSetField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "AttributeSetType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic")]
    [System.Xml.Serialization.XmlRootAttribute("AttributeSetType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic", IsNullable = true)]
    public class AttributeSetType
    {

        private List<ReportedAttributeType> reportedAttributeField;

        public AttributeSetType()
        {
            this.reportedAttributeField = new List<ReportedAttributeType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportedAttribute", Order = 0)]
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
}
