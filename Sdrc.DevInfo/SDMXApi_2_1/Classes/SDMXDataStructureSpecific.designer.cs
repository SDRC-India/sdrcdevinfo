using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;
using System.Xml;

namespace SDMXObjectModel.Data.StructureSpecific
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesDataSetType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class DataSetType : AnnotableType
    {

        private DataProviderReferenceType dataProviderField;

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

        private DataScopeType dataScopeField;

        private string rEPORTING_YEAR_START_DAYField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public DataSetType()
        {
            this.anyAttrField = new List<System.Xml.XmlAttribute>();
            this.itemsField = new List<AnnotableType>();
            this.groupField = new List<GroupType>();
            this.dataProviderField = new DataProviderReferenceType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute("Group", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute("Obs", typeof(ObsType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("Series", typeof(SeriesType), Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "IDREF")]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, DataType = "gYear")]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
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

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified)]
        public DataScopeType dataScope
        {
            get
            {
                return this.dataScopeField;
            }
            set
            {
                this.dataScopeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gMonthDay")]
        public string REPORTING_YEAR_START_DAY
        {
            get
            {
                return this.rEPORTING_YEAR_START_DAYField;
            }
            set
            {
                this.rEPORTING_YEAR_START_DAYField = value;
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesObsType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class ObsType : AnnotableType
    {

        private string typeField;

        private string tIME_PERIODField;

        private string rEPORTING_YEAR_START_DAYField;

        private string oBS_VALUEField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public ObsType(Dictionary<string, string> dictAttributes)
        {
            XmlDocument AttributesDoc;
            XmlAttribute Attribute;

            if (dictAttributes != null && dictAttributes.Keys.Count > 0)
            {
                AttributesDoc = new XmlDocument();
                this.anyAttrField = new List<XmlAttribute>();
                foreach (string AttributeName in dictAttributes.Keys)
                {
                    Attribute = AttributesDoc.CreateAttribute(AttributeName);
                    Attribute.Value = dictAttributes[AttributeName].ToString();
                    this.anyAttrField.Add(Attribute);
                }
            }
        }

        public ObsType()
            : this(null)
        {
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TIME_PERIOD
        {
            get
            {
                return this.tIME_PERIODField;
            }
            set
            {
                this.tIME_PERIODField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gMonthDay")]
        public string REPORTING_YEAR_START_DAY
        {
            get
            {
                return this.rEPORTING_YEAR_START_DAYField;
            }
            set
            {
                this.rEPORTING_YEAR_START_DAYField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OBS_VALUE
        {
            get
            {
                return this.oBS_VALUEField;
            }
            set
            {
                this.oBS_VALUEField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class TimeSeriesObsType : ObsType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeSeriesType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class SeriesType : AnnotableType
    {
        public SeriesType(Dictionary<string, string> dictAttributes, List<ObsType> Obses)
        {
            XmlDocument AttributesDoc;
            XmlAttribute Attribute;

            if (dictAttributes != null && dictAttributes.Keys.Count > 0)
            {
                AttributesDoc = new XmlDocument();
                this.anyAttrField = new List<XmlAttribute>();
                foreach (string AttributeName in dictAttributes.Keys)
                {
                    Attribute = AttributesDoc.CreateAttribute(AttributeName);
                    Attribute.Value = dictAttributes[AttributeName].ToString();
                    this.anyAttrField.Add(Attribute);
                }
            }

            if (Obses != null && Obses.Count > 0)
            {
                this.obsField = Obses;
            }
        }

        public SeriesType()
            : this(null, null)
        {
        }

        private List<ObsType> obsField;

        private string tIME_PERIODField;

        private string rEPORTING_YEAR_START_DAYField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        [System.Xml.Serialization.XmlElementAttribute("Obs", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TIME_PERIOD
        {
            get
            {
                return this.tIME_PERIODField;
            }
            set
            {
                this.tIME_PERIODField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gMonthDay")]
        public string REPORTING_YEAR_START_DAY
        {
            get
            {
                return this.rEPORTING_YEAR_START_DAYField;
            }
            set
            {
                this.rEPORTING_YEAR_START_DAYField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class TimeSeriesType : SeriesType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class GroupType : AnnotableType
    {

        private string typeField;

        private string rEPORTING_YEAR_START_DAYField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public GroupType()
        {
            this.anyAttrField = new List<System.Xml.XmlAttribute>();
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gMonthDay")]
        public string REPORTING_YEAR_START_DAY
        {
            get
            {
                return this.rEPORTING_YEAR_START_DAYField;
            }
            set
            {
                this.rEPORTING_YEAR_START_DAYField = value;
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    public enum DataScopeType
    {

        /// <remarks/>
        DataStructure,

        /// <remarks/>
        ConstrainedDataStructure,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        ProvisionAgreement,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific", IsNullable = true)]
    public class TimeSeriesDataSetType : DataSetType
    {
    }
}
