using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;

namespace SDMXObjectModel.Data.Generic
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataSetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class DataSetType : AnnotableType
    {

        private DataProviderReferenceType dataProviderField;

        private List<ComponentValueType> attributesField;

        private List<GroupType> groupField;

        private List<AnnotableType> itemsField;

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

        public DataSetType()
        {
            this.itemsField = new List<AnnotableType>();
            this.groupField = new List<GroupType>();
            this.attributesField = new List<ComponentValueType>();
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ComponentValueType> Attributes
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

        [System.Xml.Serialization.XmlElementAttribute("Obs", typeof(ObsOnlyType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Series", typeof(SeriesType), Order = 3)]
        public List<AnnotableType> Items
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ComponentValueType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ObsValueType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class BaseValueType
    {

        private string idField;

        private string valueField;

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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class TimeValueType : BaseValueType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class ComponentValueType : BaseValueType
    {
        public ComponentValueType()
        {
            this.id = string.Empty;
            this.value = string.Empty;
        }
        public ComponentValueType( string Id,string Value)
        {
            this.id = Id;
            this.value = Value;
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class ObsValueType : BaseValueType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class ObsOnlyType : AnnotableType
    {

        private List<ComponentValueType> obsKeyField;

        private ObsValueType obsValueField;

        private List<ComponentValueType> attributesField;

        public ObsOnlyType()
        {
            this.attributesField = new List<ComponentValueType>();
            this.obsValueField = new ObsValueType();
            this.obsKeyField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ComponentValueType> ObsKey
        {
            get
            {
                return this.obsKeyField;
            }
            set
            {
                this.obsKeyField = value;
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
        public List<ComponentValueType> Attributes
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesObsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class ObsType : AnnotableType
    {

        private BaseValueType obsDimensionField;

        private ObsValueType obsValueField;

        private List<ComponentValueType> attributesField;

        public ObsType()
        {
            this.attributesField = new List<ComponentValueType>();
            this.obsValueField = new ObsValueType();
            this.obsDimensionField = new BaseValueType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BaseValueType ObsDimension
        {
            get
            {
                return this.obsDimensionField;
            }
            set
            {
                this.obsDimensionField = value;
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
        public List<ComponentValueType> Attributes
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class TimeSeriesObsType : ObsType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class SeriesType : AnnotableType
    {

        private List<ComponentValueType> seriesKeyField;

        private List<ComponentValueType> attributesField;

        private List<ObsType> obsField;

        public SeriesType()
        {
            this.obsField = new List<ObsType>();
            this.attributesField = new List<ComponentValueType>();
            this.seriesKeyField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ComponentValueType> SeriesKey
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
        public List<ComponentValueType> Attributes
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class TimeSeriesType : SeriesType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class GroupType : AnnotableType
    {

        private List<ComponentValueType> groupKeyField;

        private List<ComponentValueType> attributesField;

        private string typeField;

        public GroupType()
        {
            this.attributesField = new List<ComponentValueType>();
            this.groupKeyField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Value", IsNullable = false)]
        public List<ComponentValueType> GroupKey
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
        public List<ComponentValueType> Attributes
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class TimeSeriesDataSetType : DataSetType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic", IsNullable = true)]
    public class ValuesType
    {

        private List<ComponentValueType> valueField;

        public ValuesType()
        {
            this.valueField = new List<ComponentValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Value", Order = 0)]
        public List<ComponentValueType> Value
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
