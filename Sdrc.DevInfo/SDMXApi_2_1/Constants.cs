using System;
using System.Collections.Generic;
using System.Text;

namespace SDMXObjectModel
{
    public static class Constants
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "hh:mm:sszzz";
        public const string DateTimeSeparator = "T";
        public const string LoadingException = "Exception while loading";
        public const string InvalidElement = "Invalid Element:";
        public const string InvalidAttribute = "Invalid Attribute:";
        public const string InvalidObject = "Invalid Object:";

        public static class Elements
        {
            public const string Annotation = "Annotation";
            public const string Annotations = "Annotations";
            public const string AnnotationText = "AnnotationText";
            public const string AnnotationTitle = "AnnotationTitle";
            public const string Attribute = "Attribute";
            public const string AttributeList = "AttributeList";
            public const string AttributeRelationship = "AttributeRelationship";
            public const string Category = "Category";
            public const string Code = "Code";
            public const string CodeID = "CodeID";
            public const string Codelist = "Codelist";
            public const string CodelistAliasRef = "CodelistAliasRef";
            public const string Codelists = "Codelists";
            public const string Concept = "Concept";
            public const string ConceptIdentity = "ConceptIdentity";
            public const string Concepts = "Concepts";
            public const string ConceptScheme = "ConceptScheme";
            public const string Contact = "Contact";
            public const string DataRegistrationEvents = "DataRegistrationEvents";
            public const string DataSet = "DataSet";
            public const string DataSource = "Datasource";
            public const string DataStructure = "DataStructure";
            public const string DataStructureComponents = "DataStructureComponents";
            public const string DataStructures = "DataStructures";
            public const string DataURL = "DataURL";
            public const string Department = "Department";
            public const string Description = "Description";
            public const string Dimension = "Dimension";
            public const string DimensionList = "DimensionList";
            public const string Email = "Email";
            public const string EndDate = "EndDate";
            public const string Enumeration = "Enumeration";
            public const string EventAction = "EventAction";
            public const string EventSelector = "EventSelector";
            public const string EventTime = "EventTime";
            public const string Fax = "Fax";
            public const string Header = "Header";
            public const string HierarchicalCode = "HierarchicalCode";
            public const string HierarchicalCodelist = "HierarchicalCodelist";
            public const string HierarchicalCodelists = "HierarchicalCodelists";
            public const string ID = "ID";
            public const string IncludedCodelistReference = "IncludedCodelistReference";
            public const string LocalRepresentation = "LocalRepresentation";
            public const string MeasureList = "MeasureList";
            public const string MessageText = "MessageText";
            public const string Name = "Name";
            public const string NotificationHTTP = "NotificationHTTP";
            public const string NotificationMailTo = "NotificationMailTo";
            public const string NotifyRegistryEvent = "NotifyRegistryEvent";
            public const string Obs = "Obs";
            public const string Organisation = "Organisation";
            public const string Parent = "Parent";
            public const string Prepared = "Prepared";
            public const string PrimaryMeasure = "PrimaryMeasure";
            public const string ProvisionAgreement = "ProvisionAgreement";
            public const string QueryableDataSource = "QueryableDataSource";
            public const string Receiver = "Receiver";
            public const string Ref = "Ref";
            public const string Registration = "Registration";
            public const string RegistrationEvent = "RegistrationEvent";
            public const string RegistrationID = "RegistrationID";
            public const string RegistryURN = "RegistryURN";
            public const string Role = "Role";
            public const string Sender = "Sender";
            public const string Series = "Series";
            public const string SimpleDataSource = "SimpleDataSource";
            public const string Source = "Source";
            public const string StartDate = "StartDate";
            public const string StatusMessage = "StatusMessage";
            public const string Structure = "Structure";
            public const string Structures = "Structures";
            public const string SubmitSubscriptionRequest = "SubmitSubscriptionRequest";
            public const string SubmitSubscriptionResponse = "SubmitSubscriptionResponse";
            public const string SubscriberAssignedID = "SubscriberAssignedID";
            public const string Subscription = "Subscription";
            public const string SubscriptionStatus = "SubscriptionStatus";
            public const string SubscriptionURN = "SubscriptionURN";
            public const string Telephone = "Telephone";
            public const string Test = "Test";
            public const string Text = "Text";
            public const string TextFormat = "TextFormat";
            public const string ValidityPeriod = "ValidityPeriod";
            public const string ValueHierarchy = "ValueHierarchy";
            public const string WADLURL = "WADLURL";
            public const string WSDLURL = "WSDLURL";
        }

        public static class Namespaces
        {
            public static class URLs
            {
                public const string Common = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common";
                public const string GenericData = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic";
                public const string StructureSpecificData = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/structurespecific";
                public const string GenericMetadata = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/generic";
                public const string StructureSpecificMetadata = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/metadata/structurespecific";
                public const string Message = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message";
                public const string MessageFooter = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer";
                public const string Query = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/query";
                public const string Registry = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry";
                public const string Structure = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/structure";
                public const string XSI = "http://www.w3.org/2001/XMLSchema-instance";
                public const string XS = "http://www.w3.org/2001/XMLSchema";
                public const string DevInfo = "testURI:compact";
            }

            public static class Prefixes
            {
                public const string Common = "common";
                public const string GenericData = "generic";
                public const string StructureSpecificData = "structurespecific";
                public const string GenericMetadata = "generic";
                public const string StructureSpecificMetadata = "structurespecific";
                public const string Message = "message";
                public const string MessageFooter = "footer";
                public const string Query = "query";
                public const string Registry = "registry";
                public const string Structure = "structure";
                public const string XSI = "xsi";
                public const string XS = "xs";
                public const string DevInfo = "sts";
            }
        }

        public static class Types
        {
            public static class Message
            {
                public const string StructureType = "SDMXObjectModel.Message.StructureType";
                public const string GenericDataType = "SDMXObjectModel.Message.GenericDataType";
                public const string GenericTimeSeriesDataType = "SDMXObjectModel.Message.GenericTimeSeriesDataType";
                public const string StructureSpecificDataType = "SDMXObjectModel.Message.StructureSpecificDataType";
                public const string StructureSpecificTimeSeriesDataType = "SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType";
                public const string GenericMetadataType = "SDMXObjectModel.Message.GenericMetadataType";
                public const string StructureSpecificMetadataType = "SDMXObjectModel.Message.StructureSpecificMetadataType";
                public const string RegistryInterfaceType = "SDMXObjectModel.Message.RegistryInterfaceType";
            }
        }
    }
}
