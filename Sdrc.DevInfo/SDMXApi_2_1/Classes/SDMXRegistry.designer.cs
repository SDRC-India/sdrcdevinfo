using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;
using SDMXObjectModel.Structure;

namespace SDMXObjectModel.Registry
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class RegistrationType
    {
        #region "--Variables--"

        #region "--Private--"

        private ProvisionAgreementReferenceType provisionAgreementField;

        private List<object> datasourceField;

        private string idField;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        private System.DateTime lastUpdatedField;

        private bool lastUpdatedFieldSpecified;

        private bool indexTimeSeriesField;

        private bool indexDataSetField;

        private bool indexAttributesField;

        private bool indexReportingPeriodField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ProvisionAgreementReferenceType ProvisionAgreement
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("QueryableDataSource", typeof(QueryableDataSourceType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SimpleDataSource", typeof(string), DataType = "anyURI", IsNullable = false)]
        public List<object> Datasource
        {
            get
            {
                return this.datasourceField;
            }
            set
            {
                this.datasourceField = value;
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime lastUpdated
        {
            get
            {
                return this.lastUpdatedField;
            }
            set
            {
                this.lastUpdatedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastUpdatedSpecified
        {
            get
            {
                return this.lastUpdatedFieldSpecified;
            }
            set
            {
                this.lastUpdatedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool indexTimeSeries
        {
            get
            {
                return this.indexTimeSeriesField;
            }
            set
            {
                this.indexTimeSeriesField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool indexDataSet
        {
            get
            {
                return this.indexDataSetField;
            }
            set
            {
                this.indexDataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool indexAttributes
        {
            get
            {
                return this.indexAttributesField;
            }
            set
            {
                this.indexAttributesField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool indexReportingPeriod
        {
            get
            {
                return this.indexReportingPeriodField;
            }
            set
            {
                this.indexReportingPeriodField = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        public RegistrationType():this(string.Empty)
        {
        }

        public RegistrationType(string id)
        {
            this.idField = id;
            this.datasourceField = new List<object>();
            this.provisionAgreementField = new ProvisionAgreementReferenceType();
            this.indexTimeSeriesField = false;
            this.indexDataSetField = false;
            this.indexAttributesField = false;
            this.indexReportingPeriodField = false;
        }

        #endregion "--Public--"

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "QueryableDataSourceType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute("QueryableDataSourceType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QueryableDataSourceType : SDMXObjectModel.Common.QueryableDataSourceType
    {
        #region "Variables"

        #region "Private"

        private string tYPEField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public QueryableDataSourceType()
            : this(string.Empty, false, string.Empty, false, string.Empty)
        {
        }

        public QueryableDataSourceType(string dataURL, bool isREST, string wadlURL, bool isSOAP, string wsdlURL)
        {
            this.tYPEField = "QUERY";
            this.DataURL = dataURL;
            this.isRESTDatasource = isREST;
            this.WADLURL = wadlURL;
            this.isWebServiceDatasource = isSOAP;
            this.WSDLURL = wsdlURL;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class DataSourceType
    {

        private List<object> itemsField;

        public DataSourceType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("QueryableDataSource", typeof(QueryableDataSourceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("SimpleDataSource", typeof(string), DataType = "anyURI", Order = 0)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SimpleDataSourceType
    {

        private string tYPEField;

        private string valueField;

        public SimpleDataSourceType()
        {
            this.tYPEField = "SIMPLE";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(VersionableQueryType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableQueryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class IdentifiableQueryType
    {

        private string idField;

        public IdentifiableQueryType()
        {
            this.idField = "%";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("%")]
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

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MaintainableQueryType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class VersionableQueryType : IdentifiableQueryType
    {

        private string versionField;

        public VersionableQueryType()
        {
            this.versionField = "*";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("*")]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class MaintainableQueryType : VersionableQueryType
    {

        private string agencyIDField;

        public MaintainableQueryType()
        {
            this.agencyIDField = "%";
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("%")]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class StatusMessageType
    {
        #region "Variables"

        #region "Private"

        private List<SDMXObjectModel.Common.StatusMessageType> messageTextField;

        private StatusType statusField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("MessageText", Order = 0)]
        public List<SDMXObjectModel.Common.StatusMessageType> MessageText
        {
            get
            {
                return this.messageTextField;
            }
            set
            {
                this.messageTextField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public StatusType status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public StatusMessageType():this(StatusType.Success, string.Empty, string.Empty)
        {
        }

        public StatusMessageType(StatusType status, string message, string langauge)
        {
            this.status = status;
            if (!string.IsNullOrEmpty(message))
            {
                this.messageTextField = new List<SDMXObjectModel.Common.StatusMessageType>();
                this.messageTextField.Add(new SDMXObjectModel.Common.StatusMessageType(null, message, langauge));
            }
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    public enum StatusType
    {

        /// <remarks/>
        Success,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Failure,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitRegistrationsRequestType
    {

        private List<RegistrationRequestType> registrationRequestField;

        public SubmitRegistrationsRequestType()
        {
            this.registrationRequestField = new List<RegistrationRequestType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("RegistrationRequest", Order = 0)]
        public List<RegistrationRequestType> RegistrationRequest
        {
            get
            {
                return this.registrationRequestField;
            }
            set
            {
                this.registrationRequestField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class RegistrationRequestType
    {
        #region "--Variables--"

        #region "--Private--"

        private RegistrationType registrationField;

        private ActionType actionField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RegistrationType Registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        public RegistrationRequestType():this(null, ActionType.Append)
        {
        }

        public RegistrationRequestType(RegistrationType registration, ActionType action)
        {
            this.registrationField = registration;
            this.actionField = action;
        }

        #endregion "--Public--"

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitRegistrationsResponseType
    {

        private List<RegistrationStatusType> registrationStatusField;

        public SubmitRegistrationsResponseType()
        {
            this.registrationStatusField = new List<RegistrationStatusType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("RegistrationStatus", Order = 0)]
        public List<RegistrationStatusType> RegistrationStatus
        {
            get
            {
                return this.registrationStatusField;
            }
            set
            {
                this.registrationStatusField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class RegistrationStatusType
    {

        private RegistrationType registrationField;

        private StatusMessageType statusMessageField;

        public RegistrationStatusType()
        {
            this.statusMessageField = new StatusMessageType();
            this.registrationField = new RegistrationType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RegistrationType Registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public StatusMessageType StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QueryRegistrationRequestType
    {

        private QueryTypeType queryTypeField;

        private object itemField;

        private ReferencePeriodType referencePeriodField;

        private List<object> itemsField;

        private bool returnConstraintsField;

        public QueryRegistrationRequestType()
        {
            this.itemsField = new List<object>();
            this.referencePeriodField = new ReferencePeriodType();
            this.returnConstraintsField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public QueryTypeType QueryType
        {
            get
            {
                return this.queryTypeField;
            }
            set
            {
                this.queryTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("All", typeof(EmptyType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderReferenceType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Dataflow", typeof(DataflowReferenceType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", typeof(MetadataflowReferenceType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementReferenceType), Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("CubeRegion", typeof(CubeRegionType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("DataKeySet", typeof(DataKeySetType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataKeySet", typeof(MetadataKeySetType), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataTargetRegion", typeof(MetadataTargetRegionType), Order = 3)]
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
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool returnConstraints
        {
            get
            {
                return this.returnConstraintsField;
            }
            set
            {
                this.returnConstraintsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    public enum QueryTypeType
    {

        /// <remarks/>
        DataSets,

        /// <remarks/>
        MetadataSets,

        /// <remarks/>
        AllSets,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QueryRegistrationResponseType
    {

        private StatusMessageType statusMessageField;

        private List<QueryResultType> queryResultField;

        public QueryRegistrationResponseType()
        {
            this.queryResultField = new List<QueryResultType>();
            this.statusMessageField = new StatusMessageType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StatusMessageType StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("QueryResult", Order = 1)]
        public List<QueryResultType> QueryResult
        {
            get
            {
                return this.queryResultField;
            }
            set
            {
                this.queryResultField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QueryResultType
    {

        private ResultType itemField;

        private QueryResultChoiceType itemElementNameField;

        private bool timeSeriesMatchField;

        public QueryResultType()
        {
            this.itemField = new ResultType();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataResult", typeof(ResultType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataResult", typeof(ResultType), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public ResultType Item
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
        public QueryResultChoiceType ItemElementName
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
        public bool timeSeriesMatch
        {
            get
            {
                return this.timeSeriesMatchField;
            }
            set
            {
                this.timeSeriesMatchField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class ResultType
    {

        private RegistrationType registrationField;

        private List<ContentConstraintReferenceType> contentConstraintField;

        public ResultType()
        {
            this.contentConstraintField = new List<ContentConstraintReferenceType>();
            this.registrationField = new RegistrationType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RegistrationType Registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ContentConstraint", Order = 1)]
        public List<ContentConstraintReferenceType> ContentConstraint
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum QueryResultChoiceType
    {

        /// <remarks/>
        DataResult,

        /// <remarks/>
        MetadataResult,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitStructureRequestType
    {

        private object itemField;

        private List<SubmittedStructureType> submittedStructureField;

        private ActionType actionField;

        private bool externalDependenciesField;

        public SubmitStructureRequestType()
        {
            this.submittedStructureField = new List<SubmittedStructureType>();
            this.actionField = ActionType.Append;
            this.externalDependenciesField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("StructureLocation", typeof(string), DataType = "anyURI", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Structures", typeof(StructuresType), Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute("SubmittedStructure", Order = 1)]
        public List<SubmittedStructureType> SubmittedStructure
        {
            get
            {
                return this.submittedStructureField;
            }
            set
            {
                this.submittedStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(ActionType.Append)]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool externalDependencies
        {
            get
            {
                return this.externalDependenciesField;
            }
            set
            {
                this.externalDependenciesField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmittedStructureType
    {
        #region "Variables"

        #region "Private"

        private MaintainableReferenceType maintainableObjectField;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private bool externalDependenciesField;

        private bool externalDependenciesFieldSpecified;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public MaintainableReferenceType MaintainableObject
        {
            get
            {
                return this.maintainableObjectField;
            }
            set
            {
                this.maintainableObjectField = value;
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
        public bool externalDependencies
        {
            get
            {
                return this.externalDependenciesField;
            }
            set
            {
                this.externalDependenciesField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool externalDependenciesSpecified
        {
            get
            {
                return this.externalDependenciesFieldSpecified;
            }
            set
            {
                this.externalDependenciesFieldSpecified = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public SubmittedStructureType():this(null, ActionType.Information, false, false, false)
        {
        }

        public SubmittedStructureType(MaintainableReferenceType maintainableReference, ActionType action, bool actionSpecified,
                                      bool externalDependencies, bool externalDependenciesSpecified)
        {
            this.maintainableObjectField = maintainableReference;
            this.actionField = action;
            this.actionFieldSpecified = actionSpecified;
            this.externalDependenciesField = externalDependencies;
            this.externalDependenciesFieldSpecified = externalDependenciesSpecified;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitStructureResponseType
    {

        private List<SubmissionResultType> submissionResultField;

        public SubmitStructureResponseType()
        {
            this.submissionResultField = new List<SubmissionResultType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("SubmissionResult", Order = 0)]
        public List<SubmissionResultType> SubmissionResult
        {
            get
            {
                return this.submissionResultField;
            }
            set
            {
                this.submissionResultField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmissionResultType
    {
        #region "Variables"

        #region "Private"

        private SubmittedStructureType submittedStructureField;

        private StatusMessageType statusMessageField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public SubmittedStructureType SubmittedStructure
        {
            get
            {
                return this.submittedStructureField;
            }
            set
            {
                this.submittedStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public StatusMessageType StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public SubmissionResultType():this(null, null)
        {
        }

        public SubmissionResultType(StatusMessageType statusMessage, SubmittedStructureType submittedStructure)
        {
            this.statusMessageField = statusMessage;
            this.submittedStructureField = submittedStructure;
        }

        #endregion "Constructors"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitSubscriptionsRequestType
    {

        private List<SubscriptionRequestType> subscriptionRequestField;

        public SubmitSubscriptionsRequestType()
        {
            this.subscriptionRequestField = new List<SubscriptionRequestType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("SubscriptionRequest", Order = 0)]
        public List<SubscriptionRequestType> SubscriptionRequest
        {
            get
            {
                return this.subscriptionRequestField;
            }
            set
            {
                this.subscriptionRequestField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubscriptionRequestType
    {
        #region "--Variables--"

        #region "--Private--"

        private SubscriptionType subscriptionField;

        private ActionType actionField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public SubscriptionType Subscription
        {
            get
            {
                return this.subscriptionField;
            }
            set
            {
                this.subscriptionField = value;
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        public SubscriptionRequestType()
            : this(null, ActionType.Append)
        {
        }

        public SubscriptionRequestType(SubscriptionType subscription, ActionType action)
        {
            this.subscriptionField = subscription;
            this.actionField = action;
        }

        #endregion "--Public--"

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubscriptionType
    {

        private OrganisationReferenceType organisationField;

        private string registryURNField;

        private List<NotificationURLType> notificationMailToField;

        private List<NotificationURLType> notificationHTTPField;

        private string subscriberAssignedIDField;

        private ValidityPeriodType validityPeriodField;

        private List<object> eventSelectorField;

        public SubscriptionType()
        {
            this.eventSelectorField = new List<object>();
            this.validityPeriodField = new ValidityPeriodType();
            this.notificationHTTPField = new List<NotificationURLType>();
            this.notificationMailToField = new List<NotificationURLType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public OrganisationReferenceType Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 1)]
        public string RegistryURN
        {
            get
            {
                return this.registryURNField;
            }
            set
            {
                this.registryURNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("NotificationMailTo", Order = 2)]
        public List<NotificationURLType> NotificationMailTo
        {
            get
            {
                return this.notificationMailToField;
            }
            set
            {
                this.notificationMailToField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("NotificationHTTP", Order = 3)]
        public List<NotificationURLType> NotificationHTTP
        {
            get
            {
                return this.notificationHTTPField;
            }
            set
            {
                this.notificationHTTPField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string SubscriberAssignedID
        {
            get
            {
                return this.subscriberAssignedIDField;
            }
            set
            {
                this.subscriberAssignedIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ValidityPeriodType ValidityPeriod
        {
            get
            {
                return this.validityPeriodField;
            }
            set
            {
                this.validityPeriodField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("DataRegistrationEvents", typeof(DataRegistrationEventsType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataRegistrationEvents", typeof(MetadataRegistrationEventsType), IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructuralRepositoryEvents", typeof(StructuralRepositoryEventsType), IsNullable = false)]
        public List<object> EventSelector
        {
            get
            {
                return this.eventSelectorField;
            }
            set
            {
                this.eventSelectorField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class NotificationURLType
    {
        #region "--Variables--"

        #region "--Private--"

        private bool isSOAPField;

        private string valueField;

        #endregion "--Private--"

        #region "--Public--"

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool isSOAP
        {
            get
            {
                return this.isSOAPField;
            }
            set
            {
                this.isSOAPField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
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

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        public NotificationURLType():this(false, string.Empty)
        {
        }

        public NotificationURLType(bool isSOAP, string notificationMailId)
        {
            this.isSOAPField = isSOAP;
            this.Value = notificationMailId;
        }

        #endregion "--Public--"

        #endregion "--Constructors--"
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class ValidityPeriodType
    {

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 0)]
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", Order = 1)]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class DataRegistrationEventsType
    {

        private object[] itemsField;

        private DataRegistrationEventsChoiceType[] itemsElementNameField;

        private string tYPEField;

        public DataRegistrationEventsType()
        {
            //this.itemsElementNameField = new List<DataRegistrationEventsChoiceType>();
            //this.itemsField = new List<object>();
            this.tYPEField = "DATA";
        }

        [System.Xml.Serialization.XmlElementAttribute("AllEvents", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowReference", typeof(MaintainableEventType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyReference", typeof(MaintainableEventType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RegistrationID", typeof(string), Order = 0)]
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
        public DataRegistrationEventsChoiceType[] ItemsElementName
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
        public string TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class MaintainableEventType
    {

        private object itemField;

        [System.Xml.Serialization.XmlElementAttribute("Ref", typeof(MaintainableQueryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("URN", typeof(string), DataType = "anyURI", Order = 0)]
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

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum DataRegistrationEventsChoiceType
    {

        /// <remarks/>
        AllEvents,

        /// <remarks/>
        Category,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        DataflowReference,

        /// <remarks/>
        KeyFamilyReference,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        RegistrationID,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class MetadataRegistrationEventsType
    {

        private object[] itemsField;

        private MetadataRegistrationEventsChoiceType[] itemsElementNameField;

        private string tYPEField;

        public MetadataRegistrationEventsType()
        {
            //this.itemsElementNameField = new List<MetadataRegistrationEventsChoiceType>();
            //this.itemsField = new List<object>();
            this.tYPEField = "METADATA";
        }

        [System.Xml.Serialization.XmlElementAttribute("AllEvents", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(DataProviderReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureDefinitionReference", typeof(MaintainableEventType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowReference", typeof(MaintainableEventType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementReferenceType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("RegistrationID", typeof(string), Order = 0)]
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
        public MetadataRegistrationEventsChoiceType[] ItemsElementName
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
        public string TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum MetadataRegistrationEventsChoiceType
    {

        /// <remarks/>
        AllEvents,

        /// <remarks/>
        Category,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        MetadataStructureDefinitionReference,

        /// <remarks/>
        MetadataflowReference,

        /// <remarks/>
        ProvisionAgreement,

        /// <remarks/>
        RegistrationID,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class StructuralRepositoryEventsType
    {

        private List<string> agencyIDField;

        private object[] itemsField;

        private StructuralRepositoryEventsChoiceType[] itemsElementNameField;

        private string tYPEField;

        public StructuralRepositoryEventsType()
        {
            //this.itemsElementNameField = new List<StructuralRepositoryEventsChoiceType>();
            //this.itemsField = new List<object>();
            this.agencyIDField = new List<string>();
            this.tYPEField = "STRUCTURE";
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", Order = 0)]
        public List<string> AgencyID
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

        [System.Xml.Serialization.XmlElementAttribute("AgencyScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("AllEvents", typeof(EmptyType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("AttachmentConstraint", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Categorisation", typeof(IdentifiableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("CategoryScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ContentConstraint", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataConsmerScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataProviderScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Dataflow", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelist", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureDefinition", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationUnitScheme", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("Process", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomy", typeof(VersionableObjectEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSet", typeof(VersionableObjectEventType), Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 2)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public StructuralRepositoryEventsChoiceType[] ItemsElementName
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
        public string TYPE
        {
            get
            {
                return this.tYPEField;
            }
            set
            {
                this.tYPEField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class VersionableObjectEventType
    {

        private object[] itemsField;

        private VersionableObjectEventChoiceType[] itemsElementNameField;

        public VersionableObjectEventType()
        {
            //this.itemsElementNameField = new List<VersionableObjectEventChoiceType>();
            //this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("All", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("URN", typeof(string), DataType = "anyURI", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
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
        public VersionableObjectEventChoiceType[] ItemsElementName
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum VersionableObjectEventChoiceType
    {

        /// <remarks/>
        All,

        /// <remarks/>
        ID,

        /// <remarks/>
        URN,

        /// <remarks/>
        Version,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class IdentifiableObjectEventType
    {

        private object itemField;

        private IdentifiableObjectEventChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("All", typeof(EmptyType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("URN", typeof(string), DataType = "anyURI", Order = 0)]
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
        public IdentifiableObjectEventChoiceType ItemElementName
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum IdentifiableObjectEventChoiceType
    {

        /// <remarks/>
        All,

        /// <remarks/>
        ID,

        /// <remarks/>
        URN,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum StructuralRepositoryEventsChoiceType
    {

        /// <remarks/>
        AgencyScheme,

        /// <remarks/>
        AllEvents,

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
        ContentConstraint,

        /// <remarks/>
        DataConsmerScheme,

        /// <remarks/>
        DataProviderScheme,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        HierarchicalCodelist,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        MetadataStructureDefinition,

        /// <remarks/>
        Metadataflow,

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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubmitSubscriptionsResponseType
    {

        private List<SubscriptionStatusType> subscriptionStatusField;

        public SubmitSubscriptionsResponseType()
        {
            this.subscriptionStatusField = new List<SubscriptionStatusType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("SubscriptionStatus", Order = 0)]
        public List<SubscriptionStatusType> SubscriptionStatus
        {
            get
            {
                return this.subscriptionStatusField;
            }
            set
            {
                this.subscriptionStatusField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class SubscriptionStatusType
    {

        private string subscriptionURNField;

        private string subscriberAssignedIDField;

        private StatusMessageType statusMessageField;

        public SubscriptionStatusType()
        {
            this.statusMessageField = new StatusMessageType();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string SubscriptionURN
        {
            get
            {
                return this.subscriptionURNField;
            }
            set
            {
                this.subscriptionURNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string SubscriberAssignedID
        {
            get
            {
                return this.subscriberAssignedIDField;
            }
            set
            {
                this.subscriberAssignedIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public StatusMessageType StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QuerySubscriptionRequestType
    {

        private OrganisationReferenceType organisationField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public OrganisationReferenceType Organisation
        {
            get
            {
                return this.organisationField;
            }
            set
            {
                this.organisationField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class QuerySubscriptionResponseType
    {

        private StatusMessageType statusMessageField;

        private List<SubscriptionType> subscriptionField;

        public QuerySubscriptionResponseType()
        {
            this.subscriptionField = new List<SubscriptionType>();
            this.statusMessageField = new StatusMessageType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StatusMessageType StatusMessage
        {
            get
            {
                return this.statusMessageField;
            }
            set
            {
                this.statusMessageField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Subscription", Order = 1)]
        public List<SubscriptionType> Subscription
        {
            get
            {
                return this.subscriptionField;
            }
            set
            {
                this.subscriptionField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class NotifyRegistryEventType
    {

        private System.DateTime eventTimeField;

        private string itemField;

        private NotifyRegistryEventChoiceType itemElementNameField;

        private string subscriptionURNField;

        private ActionType eventActionField;

        private object item1Field;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.DateTime EventTime
        {
            get
            {
                return this.eventTimeField;
            }
            set
            {
                this.eventTimeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ObjectURN", typeof(string), DataType = "anyURI", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("RegistrationID", typeof(string), Order = 1)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public NotifyRegistryEventChoiceType ItemElementName
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

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 3)]
        public string SubscriptionURN
        {
            get
            {
                return this.subscriptionURNField;
            }
            set
            {
                this.subscriptionURNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ActionType EventAction
        {
            get
            {
                return this.eventActionField;
            }
            set
            {
                this.eventActionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("RegistrationEvent", typeof(RegistrationEventType), Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("StructuralEvent", typeof(StructuralEventType), Order = 5)]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IncludeInSchema = false)]
    public enum NotifyRegistryEventChoiceType
    {

        /// <remarks/>
        ObjectURN,

        /// <remarks/>
        RegistrationID,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class RegistrationEventType
    {

        private RegistrationType registrationField;

        public RegistrationEventType()
        {
            this.registrationField = new RegistrationType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public RegistrationType Registration
        {
            get
            {
                return this.registrationField;
            }
            set
            {
                this.registrationField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class StructuralEventType
    {

        private StructuresType itemField;

        [System.Xml.Serialization.XmlElementAttribute("Structures", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure", Order = 0)]
        public StructuresType Item
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = true)]
    public class EventSelectorType
    {

        private List<object> itemsField;

        public EventSelectorType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataRegistrationEvents", typeof(DataRegistrationEventsType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataRegistrationEvents", typeof(MetadataRegistrationEventsType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructuralRepositoryEvents", typeof(StructuralRepositoryEventsType), Order = 0)]
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
}
