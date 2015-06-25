using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Common;
using SDMXApi_2_0.Structure;

namespace SDMXApi_2_0.Registry
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitSubscriptionRequestType
    {

        private List<SubscriptionType> subscriptionField;

        public SubmitSubscriptionRequestType()
        {
            this.subscriptionField = new List<SubscriptionType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Subscription", Order = 0)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubscriptionType
    {

        private ActionType actionField;

        private string registryURNField;

        private string notificationMailToField;

        private string notificationHTTPField;

        private string subscriberAssignedIDField;

        private ValidityPeriodType validityPeriodField;

        private EventSelectorType eventSelectorField;

        public SubscriptionType()
        {
            this.eventSelectorField = new EventSelectorType();
            this.validityPeriodField = new ValidityPeriodType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ActionType Action
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

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
        public string NotificationMailTo
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

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 3)]
        public string NotificationHTTP
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public EventSelectorType EventSelector
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class MetadataRegistrationEventsType
    {

        private List<string> allEventsIDField;

        private List<string> dataProviderIDField;

        private List<string> provisionAgreementIDField;

        private List<string> metadataflowIDField;

        private List<string> metadatastructureIDField;

        private List<string> categoryIDField;

        public MetadataRegistrationEventsType()
        {
            this.categoryIDField = new List<string>();
            this.metadatastructureIDField = new List<string>();
            this.metadataflowIDField = new List<string>();
            this.provisionAgreementIDField = new List<string>();
            this.dataProviderIDField = new List<string>();
            this.allEventsIDField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AllEventsID", Order = 0)]
        public List<string> AllEventsID
        {
            get
            {
                return this.allEventsIDField;
            }
            set
            {
                this.allEventsIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProviderID", Order = 1)]
        public List<string> DataProviderID
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

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementID", Order = 2)]
        public List<string> ProvisionAgreementID
        {
            get
            {
                return this.provisionAgreementIDField;
            }
            set
            {
                this.provisionAgreementIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowID", Order = 3)]
        public List<string> MetadataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("MetadatastructureID", Order = 4)]
        public List<string> MetadatastructureID
        {
            get
            {
                return this.metadatastructureIDField;
            }
            set
            {
                this.metadatastructureIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategoryID", Order = 5)]
        public List<string> CategoryID
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class DataRegistrationEventsType
    {

        private List<string> allEventsIDField;

        private List<string> dataProviderIDField;

        private List<string> provisionAgreementIDField;

        private List<string> dataflowIDField;

        private List<string> keyFamilyIDField;

        private List<string> categoryIDField;

        private List<string> categorySchemeIDField;

        private List<string> categorySchemeAgencyIDField;

        public DataRegistrationEventsType()
        {
            this.categorySchemeAgencyIDField = new List<string>();
            this.categorySchemeIDField = new List<string>();
            this.categoryIDField = new List<string>();
            this.keyFamilyIDField = new List<string>();
            this.dataflowIDField = new List<string>();
            this.provisionAgreementIDField = new List<string>();
            this.dataProviderIDField = new List<string>();
            this.allEventsIDField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AllEventsID", Order = 0)]
        public List<string> AllEventsID
        {
            get
            {
                return this.allEventsIDField;
            }
            set
            {
                this.allEventsIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProviderID", Order = 1)]
        public List<string> DataProviderID
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

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementID", Order = 2)]
        public List<string> ProvisionAgreementID
        {
            get
            {
                return this.provisionAgreementIDField;
            }
            set
            {
                this.provisionAgreementIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataflowID", Order = 3)]
        public List<string> DataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyID", Order = 4)]
        public List<string> KeyFamilyID
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

        [System.Xml.Serialization.XmlElementAttribute("CategoryID", Order = 5)]
        public List<string> CategoryID
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

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeID", Order = 6)]
        public List<string> CategorySchemeID
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

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeAgencyID", Order = 7)]
        public List<string> CategorySchemeAgencyID
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ProvisioningRepositoryEventsType
    {

        private List<string> provisionAgreementIDField;

        private List<string> dataProviderIDField;

        private List<string> dataflowIDField;

        private List<string> metadataflowIDField;

        private List<string> allEventsIDField;

        public ProvisioningRepositoryEventsType()
        {
            this.allEventsIDField = new List<string>();
            this.metadataflowIDField = new List<string>();
            this.dataflowIDField = new List<string>();
            this.dataProviderIDField = new List<string>();
            this.provisionAgreementIDField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementID", Order = 0)]
        public List<string> ProvisionAgreementID
        {
            get
            {
                return this.provisionAgreementIDField;
            }
            set
            {
                this.provisionAgreementIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProviderID", Order = 1)]
        public List<string> DataProviderID
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

        [System.Xml.Serialization.XmlElementAttribute("DataflowID", Order = 2)]
        public List<string> DataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowID", Order = 3)]
        public List<string> MetadataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("AllEventsID", Order = 4)]
        public List<string> AllEventsID
        {
            get
            {
                return this.allEventsIDField;
            }
            set
            {
                this.allEventsIDField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class StructuralRepositoryEventsType
    {

        private List<string> agencyIDField;

        private List<string> allEventsIDField;

        private List<string> keyFamilyIDField;

        private List<string> conceptSchemeIDField;

        private List<string> codeListIDField;

        private List<string> metadataStructureIDField;

        private List<string> categorySchemeIDField;

        private List<string> dataflowIDField;

        private List<string> metadataflowIDField;

        private List<string> organisationSchemeIDField;

        private List<string> hierarchicalCodelistIDField;

        private List<string> structureSetIDField;

        private List<string> reportingTaxonomyIDField;

        private List<string> processIDField;

        public StructuralRepositoryEventsType()
        {
            this.processIDField = new List<string>();
            this.reportingTaxonomyIDField = new List<string>();
            this.structureSetIDField = new List<string>();
            this.hierarchicalCodelistIDField = new List<string>();
            this.organisationSchemeIDField = new List<string>();
            this.metadataflowIDField = new List<string>();
            this.dataflowIDField = new List<string>();
            this.categorySchemeIDField = new List<string>();
            this.metadataStructureIDField = new List<string>();
            this.codeListIDField = new List<string>();
            this.conceptSchemeIDField = new List<string>();
            this.keyFamilyIDField = new List<string>();
            this.allEventsIDField = new List<string>();
            this.agencyIDField = new List<string>();
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

        [System.Xml.Serialization.XmlElementAttribute("AllEventsID", Order = 1)]
        public List<string> AllEventsID
        {
            get
            {
                return this.allEventsIDField;
            }
            set
            {
                this.allEventsIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyID", Order = 2)]
        public List<string> KeyFamilyID
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

        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeID", Order = 3)]
        public List<string> ConceptSchemeID
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

        [System.Xml.Serialization.XmlElementAttribute("CodeListID", Order = 4)]
        public List<string> CodeListID
        {
            get
            {
                return this.codeListIDField;
            }
            set
            {
                this.codeListIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureID", Order = 5)]
        public List<string> MetadataStructureID
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

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeID", Order = 6)]
        public List<string> CategorySchemeID
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

        [System.Xml.Serialization.XmlElementAttribute("DataflowID", Order = 7)]
        public List<string> DataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowID", Order = 8)]
        public List<string> MetadataflowID
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

        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeID", Order = 9)]
        public List<string> OrganisationSchemeID
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

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistID", Order = 10)]
        public List<string> HierarchicalCodelistID
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

        [System.Xml.Serialization.XmlElementAttribute("StructureSetID", Order = 11)]
        public List<string> StructureSetID
        {
            get
            {
                return this.structureSetIDField;
            }
            set
            {
                this.structureSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyID", Order = 12)]
        public List<string> ReportingTaxonomyID
        {
            get
            {
                return this.reportingTaxonomyIDField;
            }
            set
            {
                this.reportingTaxonomyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessID", Order = 13)]
        public List<string> ProcessID
        {
            get
            {
                return this.processIDField;
            }
            set
            {
                this.processIDField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class EventSelectorType
    {

        private StructuralRepositoryEventsType structuralRepositoryEventsField;

        private ProvisioningRepositoryEventsType provisioningRepositoryEventsField;

        private DataRegistrationEventsType dataRegistrationEventsField;

        private MetadataRegistrationEventsType metadataRegistrationEventsField;

        public EventSelectorType()
        {
            this.metadataRegistrationEventsField = new MetadataRegistrationEventsType();
            this.dataRegistrationEventsField = new DataRegistrationEventsType();
            this.provisioningRepositoryEventsField = new ProvisioningRepositoryEventsType();
            this.structuralRepositoryEventsField = new StructuralRepositoryEventsType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructuralRepositoryEventsType StructuralRepositoryEvents
        {
            get
            {
                return this.structuralRepositoryEventsField;
            }
            set
            {
                this.structuralRepositoryEventsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public ProvisioningRepositoryEventsType ProvisioningRepositoryEvents
        {
            get
            {
                return this.provisioningRepositoryEventsField;
            }
            set
            {
                this.provisioningRepositoryEventsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public DataRegistrationEventsType DataRegistrationEvents
        {
            get
            {
                return this.dataRegistrationEventsField;
            }
            set
            {
                this.dataRegistrationEventsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public MetadataRegistrationEventsType MetadataRegistrationEvents
        {
            get
            {
                return this.metadataRegistrationEventsField;
            }
            set
            {
                this.metadataRegistrationEventsField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitSubscriptionResponseType
    {

        private string subscriptionURNField;

        private string subscriberAssignedIDField;

        private StatusMessageType subscriptionStatusField;

        public SubmitSubscriptionResponseType()
        {
            this.subscriptionStatusField = new StatusMessageType();
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
        public StatusMessageType SubscriptionStatus
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class StatusMessageType
    {

        private List<TextType> messageTextField;

        private StatusType statusField;

        public StatusMessageType()
        {
            this.messageTextField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("MessageText", Order = 0)]
        public List<TextType> MessageText
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    public enum StatusType
    {

        /// <remarks/>
        Success,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Failure,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class NotifyRegistryEventType
    {

        private System.DateTime eventTimeField;

        private string objectURNField;

        private string subscriptionURNField;

        private ActionType eventActionField;

        private object itemField;

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

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 1)]
        public string ObjectURN
        {
            get
            {
                return this.objectURNField;
            }
            set
            {
                this.objectURNField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
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

        [System.Xml.Serialization.XmlElementAttribute("ProvisioningEvent", typeof(ProvisioningEventType), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("RegistrationEvent", typeof(RegistrationEventType), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("StructuralEvent", typeof(StructuralEventType), Order = 4)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ProvisioningEventType
    {

        private DataProviderRefType dataProviderRefField;

        private DataflowRefType dataflowRefField;

        private MetadataflowRefType metadataflowRefField;

        private ProvisionAgreementType provisionAgreementField;

        public ProvisioningEventType()
        {
            this.provisionAgreementField = new ProvisionAgreementType();
            this.metadataflowRefField = new MetadataflowRefType();
            this.dataflowRefField = new DataflowRefType();
            this.dataProviderRefField = new DataProviderRefType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DataProviderRefType DataProviderRef
        {
            get
            {
                return this.dataProviderRefField;
            }
            set
            {
                this.dataProviderRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DataflowRefType DataflowRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MetadataflowRefType MetadataflowRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public ProvisionAgreementType ProvisionAgreement
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class DataProviderRefType
    {

        private string uRNField;

        private string organisationSchemeAgencyIDField;

        private string organisationSchemeIDField;

        private string dataProviderIDField;

        private string versionField;

        private DatasourceType datasourceField;

        private ConstraintType constraintField;

        public DataProviderRefType()
        {
            this.constraintField = new ConstraintType();
            this.datasourceField = new DatasourceType();
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
        public DatasourceType Datasource
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class DatasourceType
    {

        private string simpleDatasourceField;

        private QueryableDatasourceType queryableDatasourceField;

        public DatasourceType()
        {
            this.queryableDatasourceField = new QueryableDatasourceType();
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string SimpleDatasource
        {
            get
            {
                return this.simpleDatasourceField;
            }
            set
            {
                this.simpleDatasourceField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public QueryableDatasourceType QueryableDatasource
        {
            get
            {
                return this.queryableDatasourceField;
            }
            set
            {
                this.queryableDatasourceField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryableDatasourceType
    {

        private string dataUrlField;

        private string wSDLUrlField;

        private bool isRESTDatasourceField;

        private bool isWebServiceDatasourceField;

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 0)]
        public string DataUrl
        {
            get
            {
                return this.dataUrlField;
            }
            set
            {
                this.dataUrlField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 1)]
        public string WSDLUrl
        {
            get
            {
                return this.wSDLUrlField;
            }
            set
            {
                this.wSDLUrlField = value;
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DataflowRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("DataflowRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class DataflowRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string dataflowIDField;

        private string versionField;

        private DatasourceType datasourceField;

        private ConstraintType constraintField;

        public DataflowRefType()
        {
            this.constraintField = new ConstraintType();
            this.datasourceField = new DatasourceType();
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public DatasourceType Datasource
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataflowRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataflowRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class MetadataflowRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string metadataflowIDField;

        private string versionField;

        private DatasourceType datasourceField;

        private ConstraintType constraintField;

        public MetadataflowRefType()
        {
            this.constraintField = new ConstraintType();
            this.datasourceField = new DatasourceType();
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public DatasourceType Datasource
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ProvisionAgreementType
    {

        private List<TextType> nameField;

        private List<TextType> descriptionField;

        private object itemField;

        private DataProviderRefType dataProviderRefField;

        private QueryableDatasourceType datasourceField;

        private ConstraintType constraintField;

        private List<AnnotationType> annotationsField;

        private string idField;

        private string uriField;

        private string urnField;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private bool indexTimeSeriesField;

        private bool indexTimeSeriesFieldSpecified;

        private bool indexDataSetField;

        private bool indexDataSetFieldSpecified;

        private bool indexReportingPeriodField;

        private bool indexReportingPeriodFieldSpecified;

        private string validFromField;

        private string validToField;

        public ProvisionAgreementType()
        {
            this.annotationsField = new List<AnnotationType>();
            this.constraintField = new ConstraintType();
            this.datasourceField = new QueryableDatasourceType();
            this.dataProviderRefField = new DataProviderRefType();
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

        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public DataProviderRefType DataProviderRef
        {
            get
            {
                return this.dataProviderRefField;
            }
            set
            {
                this.dataProviderRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public QueryableDatasourceType Datasource
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexTimeSeriesSpecified
        {
            get
            {
                return this.indexTimeSeriesFieldSpecified;
            }
            set
            {
                this.indexTimeSeriesFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexDataSetSpecified
        {
            get
            {
                return this.indexDataSetFieldSpecified;
            }
            set
            {
                this.indexDataSetFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool indexReportingPeriodSpecified
        {
            get
            {
                return this.indexReportingPeriodFieldSpecified;
            }
            set
            {
                this.indexReportingPeriodFieldSpecified = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class RegistrationType
    {

        private System.DateTime lastUpdatedField;

        private bool lastUpdatedFieldSpecified;

        private System.DateTime validFromField;

        private bool validFromFieldSpecified;

        private System.DateTime validToField;

        private bool validToFieldSpecified;

        private ActionType actionField;

        private DatasourceType datasourceField;

        private object itemField;

        public RegistrationType()
        {
            this.datasourceField = new DatasourceType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public System.DateTime LastUpdated
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
        public bool LastUpdatedSpecified
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime ValidFrom
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
        public bool ValidFromSpecified
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public System.DateTime ValidTo
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
        public bool ValidToSpecified
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public ActionType Action
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public DatasourceType Datasource
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

        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementRef", typeof(ProvisionAgreementRefType), Order = 5)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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

        private DatasourceType datasourceField;

        private ConstraintType constraintField;

        public ProvisionAgreementRefType()
        {
            this.constraintField = new ConstraintType();
            this.datasourceField = new DatasourceType();
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
        public DatasourceType Datasource
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class StructuralEventType
    {

        private List<OrganisationSchemeType> organisationSchemesField;

        private List<DataflowType> dataflowsField;

        private List<MetadataflowType> metadataflowsField;

        private List<CategorySchemeType> categorySchemesField;

        private List<CodeListType> codeListsField;

        private List<HierarchicalCodelistType> hierarchicalCodelistsField;

        private ConceptsType conceptsField;

        private List<MetadataStructureDefinitionType> metadataStructureDefinitionsField;

        private List<KeyFamilyType> keyFamiliesField;

        private List<StructureSetType> structureSetsField;

        private List<ProcessType> processesField;

        private List<ReportingTaxonomyType> reportingTaxonomiesField;

        public StructuralEventType()
        {
            this.reportingTaxonomiesField = new List<ReportingTaxonomyType>();
            this.processesField = new List<ProcessType>();
            this.structureSetsField = new List<StructureSetType>();
            this.keyFamiliesField = new List<KeyFamilyType>();
            this.metadataStructureDefinitionsField = new List<MetadataStructureDefinitionType>();
            this.conceptsField = new ConceptsType();
            this.hierarchicalCodelistsField = new List<HierarchicalCodelistType>();
            this.codeListsField = new List<CodeListType>();
            this.categorySchemesField = new List<CategorySchemeType>();
            this.metadataflowsField = new List<MetadataflowType>();
            this.dataflowsField = new List<DataflowType>();
            this.organisationSchemesField = new List<OrganisationSchemeType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OrganisationScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Dataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Metadataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CodeList", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<CodeListType> CodeLists
        {
            get
            {
                return this.codeListsField;
            }
            set
            {
                this.codeListsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("HierarchicalCodelist", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public ConceptsType Concepts
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataStructureDefinition", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<MetadataStructureDefinitionType> MetadataStructureDefinitions
        {
            get
            {
                return this.metadataStructureDefinitionsField;
            }
            set
            {
                this.metadataStructureDefinitionsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyFamily", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<KeyFamilyType> KeyFamilies
        {
            get
            {
                return this.keyFamiliesField;
            }
            set
            {
                this.keyFamiliesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructureSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Process", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReportingTaxonomy", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitRegistrationRequestType
    {

        private List<RegistrationType> registrationField;

        public SubmitRegistrationRequestType()
        {
            this.registrationField = new List<RegistrationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Registration", Order = 0)]
        public List<RegistrationType> Registration
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitRegistrationResponseType
    {

        private List<RegistrationStatusType> registrationStatusField;

        public SubmitRegistrationResponseType()
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class RegistrationStatusType
    {

        private StatusMessageType statusMessageField;

        private DatasourceType datasourceField;

        private DataProviderRefType dataProviderRefField;

        private DataflowRefType dataflowRefField;

        private MetadataflowRefType metadaflowRefField;

        private ProvisionAgreementRefType provisionAgreementRefField;

        public RegistrationStatusType()
        {
            this.provisionAgreementRefField = new ProvisionAgreementRefType();
            this.metadaflowRefField = new MetadataflowRefType();
            this.dataflowRefField = new DataflowRefType();
            this.dataProviderRefField = new DataProviderRefType();
            this.datasourceField = new DatasourceType();
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DatasourceType Datasource
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public DataProviderRefType DataProviderRef
        {
            get
            {
                return this.dataProviderRefField;
            }
            set
            {
                this.dataProviderRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public DataflowRefType DataflowRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public MetadataflowRefType MetadaflowRef
        {
            get
            {
                return this.metadaflowRefField;
            }
            set
            {
                this.metadaflowRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ProvisionAgreementRefType ProvisionAgreementRef
        {
            get
            {
                return this.provisionAgreementRefField;
            }
            set
            {
                this.provisionAgreementRefField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryRegistrationRequestType
    {

        private QueryTypeType queryTypeField;

        private object itemField;

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

        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", typeof(DataProviderRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementRef", typeof(ProvisionAgreementRefType), Order = 1)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    public enum QueryTypeType
    {

        /// <remarks/>
        DataSets,

        /// <remarks/>
        MetadataSets,

        /// <remarks/>
        AllSets,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryRegistrationResponseType
    {

        private List<QueryResultType> queryResultField;

        public QueryRegistrationResponseType()
        {
            this.queryResultField = new List<QueryResultType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("QueryResult", Order = 0)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryResultType
    {

        private object itemField;

        private QueryResultChoiceType itemElementNameField;

        private bool timeSeriesMatchField;

        [System.Xml.Serialization.XmlElementAttribute("DataResult", typeof(ResultType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataResult", typeof(ResultType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StatusMessage", typeof(StatusMessageType), Order = 0)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ResultType
    {

        private DatasourceType datasourceField;

        private object itemField;

        private ResultChoiceType itemElementNameField;

        public ResultType()
        {
            this.datasourceField = new DatasourceType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public DatasourceType Datasource
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

        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", typeof(DataProviderRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(DataflowRefType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreementRef", typeof(ProvisionAgreementRefType), Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ResultChoiceType ItemElementName
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IncludeInSchema = false)]
    public enum ResultChoiceType
    {

        /// <remarks/>
        DataProviderRef,

        /// <remarks/>
        DataflowRef,

        /// <remarks/>
        MetadataflowRef,

        /// <remarks/>
        ProvisionAgreementRef,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitStructureRequestType
    {

        private object itemField;

        private List<SubmittedStructureType> submittedStructureField;

        public SubmitStructureRequestType()
        {
            this.submittedStructureField = new List<SubmittedStructureType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", typeof(StructureType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureLocation", typeof(string), DataType = "anyURI", Order = 0)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class StructureType
    {

        private List<OrganisationSchemeType> organisationSchemesField;

        private List<DataflowType> dataflowsField;

        private List<MetadataflowType> metadataflowsField;

        private List<CategorySchemeType> categorySchemesField;

        private List<CodeListType> codeListsField;

        private List<HierarchicalCodelistType> hierarchicalCodelistsField;

        private ConceptsType conceptsField;

        private List<MetadataStructureDefinitionType> metadataStructureDefinitionsField;

        private List<KeyFamilyType> keyFamiliesField;

        private List<StructureSetType> structureSetsField;

        private List<ProcessType> processesField;

        private List<ReportingTaxonomyType> reportingTaxonomiesField;

        public StructureType()
        {
            this.reportingTaxonomiesField = new List<ReportingTaxonomyType>();
            this.processesField = new List<ProcessType>();
            this.structureSetsField = new List<StructureSetType>();
            this.keyFamiliesField = new List<KeyFamilyType>();
            this.metadataStructureDefinitionsField = new List<MetadataStructureDefinitionType>();
            this.conceptsField = new ConceptsType();
            this.hierarchicalCodelistsField = new List<HierarchicalCodelistType>();
            this.codeListsField = new List<CodeListType>();
            this.categorySchemesField = new List<CategorySchemeType>();
            this.metadataflowsField = new List<MetadataflowType>();
            this.dataflowsField = new List<DataflowType>();
            this.organisationSchemesField = new List<OrganisationSchemeType>();
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 0)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OrganisationScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Dataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Metadataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("CodeList", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<CodeListType> CodeLists
        {
            get
            {
                return this.codeListsField;
            }
            set
            {
                this.codeListsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("HierarchicalCodelist", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public ConceptsType Concepts
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 7)]
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataStructureDefinition", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<MetadataStructureDefinitionType> MetadataStructureDefinitions
        {
            get
            {
                return this.metadataStructureDefinitionsField;
            }
            set
            {
                this.metadataStructureDefinitionsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 8)]
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyFamily", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<KeyFamilyType> KeyFamilies
        {
            get
            {
                return this.keyFamiliesField;
            }
            set
            {
                this.keyFamiliesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructureSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Process", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 11)]
        [System.Xml.Serialization.XmlArrayItemAttribute("ReportingTaxonomy", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmittedStructureType
    {

        private object itemField;

        private bool externalDependenciesField;

        private bool externalDependenciesFieldSpecified;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private bool isFinalField;

        private bool isFinalFieldSpecified;

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeRef", typeof(CategorySchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodelistRef", typeof(CodelistRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeRef", typeof(ConceptSchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistRef", typeof(HierarchicalCodelistRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyRef", typeof(KeyFamilyRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureRef", typeof(MetadataStructureRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeRef", typeof(OrganisationSchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProcessRef", typeof(ProcessRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyRef", typeof(ReportingTaxonomyRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSetRef", typeof(StructureSetRefType), Order = 0)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CategorySchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("CategorySchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CodelistRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("CodelistRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class CodelistRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string codelistIDField;

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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConceptSchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("ConceptSchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "HierarchicalCodelistRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("HierarchicalCodelistRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "KeyFamilyRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("KeyFamilyRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class KeyFamilyRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string keyFamilyIDField;

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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataStructureRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataStructureRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class MetadataStructureRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string metadataStructureIDField;

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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "OrganisationSchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationSchemeRefType", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ProcessRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string processIDField;

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
        public string ProcessID
        {
            get
            {
                return this.processIDField;
            }
            set
            {
                this.processIDField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ReportingTaxonomyRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string reportingTaxonomyIDField;

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
        public string ReportingTaxonomyID
        {
            get
            {
                return this.reportingTaxonomyIDField;
            }
            set
            {
                this.reportingTaxonomyIDField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class StructureSetRefType
    {

        private string uRNField;

        private string agencyIDField;

        private string structureSetIDField;

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
        public string StructureSetID
        {
            get
            {
                return this.structureSetIDField;
            }
            set
            {
                this.structureSetIDField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmissionResultType
    {

        private SubmittedStructureType submittedStructureField;

        private StatusMessageType statusMessageField;

        public SubmissionResultType()
        {
            this.statusMessageField = new StatusMessageType();
            this.submittedStructureField = new SubmittedStructureType();
        }

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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryStructureRequestType
    {

        private List<object> itemsField;

        private bool resolveReferencesField;

        public QueryStructureRequestType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyRef", typeof(AgencyRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeRef", typeof(CategorySchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("CodelistRef", typeof(CodelistRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeRef", typeof(ConceptSchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", typeof(DataProviderRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistRef", typeof(HierarchicalCodelistRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyRef", typeof(KeyFamilyRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureRef", typeof(MetadataStructureRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", typeof(MetadataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeRef", typeof(OrganisationSchemeRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProcessRef", typeof(ProcessRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyRef", typeof(ReportingTaxonomyRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSetRef", typeof(StructureSetRefType), Order = 0)]
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
        public bool resolveReferences
        {
            get
            {
                return this.resolveReferencesField;
            }
            set
            {
                this.resolveReferencesField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class AgencyRefType
    {

        private string uRNField;

        private string organisationSchemeAgencyIDField;

        private string organisationSchemeIDField;

        private string agencyIDField;

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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryStructureResponseType
    {

        private StatusMessageType statusMessageField;

        private List<OrganisationSchemeType> organisationSchemesField;

        private List<DataflowType> dataflowsField;

        private List<MetadataflowType> metadataflowsField;

        private List<CategorySchemeType> categorySchemesField;

        private List<CodeListType> codeListsField;

        private List<HierarchicalCodelistType> hierarchicalCodelistsField;

        private ConceptsType conceptsField;

        private List<MetadataStructureDefinitionType> metadataStructureDefinitionsField;

        private List<KeyFamilyType> keyFamiliesField;

        private List<StructureSetType> structureSetsField;

        private List<ReportingTaxonomyType> reportingTaxonomiesField;

        private List<ProcessType> processesField;

        public QueryStructureResponseType()
        {
            this.processesField = new List<ProcessType>();
            this.reportingTaxonomiesField = new List<ReportingTaxonomyType>();
            this.structureSetsField = new List<StructureSetType>();
            this.keyFamiliesField = new List<KeyFamilyType>();
            this.metadataStructureDefinitionsField = new List<MetadataStructureDefinitionType>();
            this.conceptsField = new ConceptsType();
            this.hierarchicalCodelistsField = new List<HierarchicalCodelistType>();
            this.codeListsField = new List<CodeListType>();
            this.categorySchemesField = new List<CategorySchemeType>();
            this.metadataflowsField = new List<MetadataflowType>();
            this.dataflowsField = new List<DataflowType>();
            this.organisationSchemesField = new List<OrganisationSchemeType>();
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("OrganisationScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Dataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 3)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Metadataflow", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 4)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CategoryScheme", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("CodeList", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<CodeListType> CodeLists
        {
            get
            {
                return this.codeListsField;
            }
            set
            {
                this.codeListsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 6)]
        [System.Xml.Serialization.XmlArrayItemAttribute("HierarchicalCodelist", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public ConceptsType Concepts
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
        [System.Xml.Serialization.XmlArrayItemAttribute("MetadataStructureDefinition", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<MetadataStructureDefinitionType> MetadataStructureDefinitions
        {
            get
            {
                return this.metadataStructureDefinitionsField;
            }
            set
            {
                this.metadataStructureDefinitionsField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 9)]
        [System.Xml.Serialization.XmlArrayItemAttribute("KeyFamily", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
        public List<KeyFamilyType> KeyFamilies
        {
            get
            {
                return this.keyFamiliesField;
            }
            set
            {
                this.keyFamiliesField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("StructureSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("ReportingTaxonomy", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Process", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/structure", IsNullable = false)]
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
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitProvisioningRequestType
    {

        private List<object> itemsField;

        public SubmitProvisioningRequestType()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", typeof(DataProviderRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", typeof(DataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadatataflowRef", typeof(MetadataflowRefType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("ProvisionAgreement", typeof(ProvisionAgreementType), Order = 0)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class SubmitProvisioningResponseType
    {

        private List<ProvisioningStatusType> provisioningStatusField;

        public SubmitProvisioningResponseType()
        {
            this.provisioningStatusField = new List<ProvisioningStatusType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ProvisioningStatus", Order = 0)]
        public List<ProvisioningStatusType> ProvisioningStatus
        {
            get
            {
                return this.provisioningStatusField;
            }
            set
            {
                this.provisioningStatusField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class ProvisioningStatusType
    {

        private object itemField;

        private StatusMessageType statusMessageField;

        public ProvisioningStatusType()
        {
            this.statusMessageField = new StatusMessageType();
        }

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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryProvisioningRequestType
    {

        private ProvisionAgreementRefType provisionAgreementRefField;

        private DataflowRefType dataflowRefField;

        private MetadataflowRefType metadataflowRefField;

        private DataProviderRefType dataProviderRefField;

        public QueryProvisioningRequestType()
        {
            this.dataProviderRefField = new DataProviderRefType();
            this.metadataflowRefField = new MetadataflowRefType();
            this.dataflowRefField = new DataflowRefType();
            this.provisionAgreementRefField = new ProvisionAgreementRefType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public ProvisionAgreementRefType ProvisionAgreementRef
        {
            get
            {
                return this.provisionAgreementRefField;
            }
            set
            {
                this.provisionAgreementRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public DataflowRefType DataflowRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MetadataflowRefType MetadataflowRef
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public DataProviderRefType DataProviderRef
        {
            get
            {
                return this.dataProviderRefField;
            }
            set
            {
                this.dataProviderRefField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IsNullable = true)]
    public class QueryProvisioningResponseType
    {

        private List<ProvisionAgreementType> provisionAgreementField;

        private List<DataflowRefType> dataflowRefField;

        private List<MetadataflowRefType> metadataflowRefField;

        private List<DataProviderRefType> dataProviderRefField;

        private StatusMessageType statusMessageField;

        public QueryProvisioningResponseType()
        {
            this.statusMessageField = new StatusMessageType();
            this.dataProviderRefField = new List<DataProviderRefType>();
            this.metadataflowRefField = new List<MetadataflowRefType>();
            this.dataflowRefField = new List<DataflowRefType>();
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

        [System.Xml.Serialization.XmlElementAttribute("DataflowRef", Order = 1)]
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

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowRef", Order = 2)]
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

        [System.Xml.Serialization.XmlElementAttribute("DataProviderRef", Order = 3)]
        public List<DataProviderRefType> DataProviderRef
        {
            get
            {
                return this.dataProviderRefField;
            }
            set
            {
                this.dataProviderRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
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

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/registry", IncludeInSchema = false)]
    public enum QueryResultChoiceType
    {

        /// <remarks/>
        DataResult,

        /// <remarks/>
        MetadataResult,

        /// <remarks/>
        StatusMessage,
    }
}
