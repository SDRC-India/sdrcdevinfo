using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Common;

namespace SDMXApi_2_0.GenericData
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute("DataSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = false)]
    public class DataSetType
    {

        private string keyFamilyRefField;

        private List<ValueType> attributesField;

        private List<object> itemsField;

        private List<AnnotationType> annotationsField;

        private string keyFamilyURIField;

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

        public DataSetType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.itemsField = new List<object>();
            this.attributesField = new List<ValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> Attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Group", typeof(GroupType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("Series", typeof(SeriesType), Order = 2)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string keyFamilyURI
        {
            get
            {
                return this.keyFamilyURIField;
            }
            set
            {
                this.keyFamilyURIField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class ValueType
    {

        private string conceptField;

        private string valueField;

        private System.DateTime startTimeField;

        private bool startTimeFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string concept
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startTimeSpecified
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class ObsValueType
    {

        private double valueField;

        private bool valueFieldSpecified;

        private System.DateTime startTimeField;

        private bool startTimeFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double value
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }

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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startTimeSpecified
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class ObsType
    {

        private string timeField;

        private ObsValueType obsValueField;

        private List<ValueType> attributesField;

        private List<AnnotationType> annotationsField;

        public ObsType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.attributesField = new List<ValueType>();
            this.obsValueField = new ObsValueType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ObsValueType ObsValue
        {
            get
            {
                return this.obsValueField;
            }
            set
            {
                this.obsValueField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> Attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class SeriesType
    {

        private List<ValueType> seriesKeyField;

        private List<ValueType> attributesField;

        private List<ObsType> obsField;

        private List<AnnotationType> annotationsField;

        public SeriesType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.obsField = new List<ObsType>();
            this.attributesField = new List<ValueType>();
            this.seriesKeyField = new List<ValueType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> SeriesKey
        {
            get
            {
                return this.seriesKeyField;
            }
            set
            {
                this.seriesKeyField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> Attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Obs", Order = 2)]
        public List<ObsType> Obs
        {
            get
            {
                return this.obsField;
            }
            set
            {
                this.obsField = value;
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class GroupType
    {

        private List<ValueType> groupKeyField;

        private List<ValueType> attributesField;

        private List<SeriesType> seriesField;

        private List<AnnotationType> annotationsField;

        private string typeField;

        public GroupType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.seriesField = new List<SeriesType>();
            this.attributesField = new List<ValueType>();
            this.groupKeyField = new List<ValueType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> GroupKey
        {
            get
            {
                return this.groupKeyField;
            }
            set
            {
                this.groupKeyField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ValueType> Attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Series", Order = 2)]
        public List<SeriesType> Series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NMTOKEN")]
        public string type
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class SeriesKeyType
    {

        private List<ValueType> valueField;

        public SeriesKeyType()
        {
            this.valueField = new List<ValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Value", Order = 0)]
        public List<ValueType> Value
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
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/generic", IsNullable = true)]
    public class ValuesType
    {

        private List<ValueType> valueField;

        public ValuesType()
        {
            this.valueField = new List<ValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Value", Order = 0)]
        public List<ValueType> Value
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

}
