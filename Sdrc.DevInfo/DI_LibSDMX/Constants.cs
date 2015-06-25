using System;
using System.Text;

namespace DevInfo.Lib.DI_LibSDMX
{
    /// <summary>
    /// Constants class contains various utility constant string expressions and a hierarchy of other such classes to be used across library.
    /// </summary>
    public static class Constants
    {
        public const string DefaultLanguage = "en";
        public const string DateFormat = "yyyy-MM-dd";
        public const string TimeFormat = "hh:mm:sszzz";
        public const string GIDSeparator = "@__@";

        public const string NA = "NA";
        public const string MinusOne = "-1";
        public const string AND = " AND ";
        public const string OR = " OR ";
        public const string MRD = "MRD";
        public const string AgencyId = "agency";

        public const string Comma = ",";
        public const string Underscore = "_";
        public const string Tilde = "~";
        public const string Apostophe = "'";
        public const string Slash = "/";
        public const string Dash = "-";
        public const string EqualsTo = " = ";
        public const string NotEqualsTo = " <> ";
        public const string Space = " ";
        public const string QuestionMark = "?";
        public const string Ampersand = "&";
        public const string IN = " IN ";
        public const string OpeningParenthesis = "(";
        public const string ClosingParenthesis = ")";
        public const string T = "T";
        public const string CodelistPrefix = "CL_";
        public const string AtTheRate = "@";
        public const string XmlExtension = ".xml";

        public const string defaultNamespace = "http://www.devinfo.info/";

        public static class Header
        {
            public const string Id = "Header_Id";
            public const string Sender = "Sender";
            public const string SenderId = "Sender_Id";
            public const string SenderName = "Sender_Name";
            public const string SenderDepartment = "Sender_Department";
            public const string SenderRole = "Sender_Role";
            public const string SenderTelephone = "Sender_Telephone";
            public const string SenderEmail = "Sender_Email";
            public const string SenderFax = "Sender_Fax";
            public const string Receiver = "Receiver";
            public const string ReceiverId = "Receiver_Id";
            public const string ReceiverName = "Receiver_Name";
            public const string ReceiverDepartment = "Receiver_Department";
            public const string ReceiverRole = "Receiver_Role";
            public const string ReceiverTelephone = "Receiver_Telephone";
            public const string ReceiverEmail = "Receiver_Email";
            public const string ReceiverFax = "Receiver_Fax";
            public const string FileName = "Header.xml";
        }

        public static class Annotations
        {
            public const string IsGlobal = "IsGlobal";
            public const string Breakup = "Breakup";
            public const string HighIsGood = "HighIsGood";
            public const string IsDefault = "IsDefault";
            public const string CategoryType = "CategoryType";

            public const string IC = "IC";
            public const string Indicator = "Indicator";
            public const string Area_Level = "Area_Level";
        }

        public static class Concept
        {
            public static class TIME_PERIOD
            {
                public const string Id = "TIME_PERIOD";
                public const string Name = "TimePeriod";
                public const string Description = "Reference date - or date range - the observed value refers to (usually different from the dates of data production or dissemination). All four format (Gregorian Time Period, Date Time, Reporting Time Period, Time Range) supported by SDMX are allowed.";
            }

            public static class OBS_VALUE
            {
                public const string Id = "OBS_VALUE";
                public const string Name = "Observation value";
                public const string Description = "Actual observation.";
            }

            public static class AREA
            {
                public const string Id = "AREA";
                public const string Name = "Area";
                public const string Description = "Reference Area: Specific areas (e.g. Country, Regional Grouping, etc)  the observed values refer to. Reference areas can be determined according to different criteria (e.g.: geographical, economic, etc.)";
            }

            public static class INDICATOR
            {
                public const string Id = "INDICATOR";
                public const string Name = "Indicator";
                public const string Description = "The phenomenon or phenomena to be measured in the data set. The word INDICATOR is used for consistency with the term used in the DevInfo.";
            }

            public static class UNIT
            {
                public const string Id = "UNIT";
                public const string Name = "Unit";
                public const string Description = "Dimension by which the indicators are described (e.g.: percentage, USD, etc.)";
            }

            public static class PERIODICITY
            {
                public const string Id = "PERIODICITY";
                public const string Name = "Periodicity";
                public const string Description = "Indicates rate of recurrence at which observations occur (e.g. monthly, yearly, biannually, etc.)";
            }

            public static class SOURCE
            {
                public const string Id = "SOURCE";
                public const string Name = "Source";
                public const string Description = "Source of  survey, administrative records, census etc.";
            }

            public static class NATURE
            {
                public const string Id = "NATURE";
                public const string Name = "Nature";
                public const string Description = "Information on the production and dissemination of the data (e.g.: if the figure has been produced and disseminated by the country, estimated by international agencies, etc.)";
            }

            public static class DENOMINATOR
            {
                public const string Id = "DENOMINATOR";
                public const string Name = "Denominator";
                public const string Description = "Base denominator value associated with observation.";
            }

            public static class FOOTNOTES
            {
                public const string Id = "FOOTNOTES";
                public const string Name = "Footnotes";
                public const string Description = "Additional information on specific aspects of each observation, such as how the observation was computed/estimated or details that could affect the comparability of this data point with others in a time series.";
            }

            public static class CONFIDENCE_INTERVAL_UPPER
            {
                public const string Id = "CONFIDENCE_INTERVAL_UPPER";
                public const string Name = "Confidence interval - Upper";
                public const string Description = "Upper measure of the probability that the observation value will fall below this value.";
            }

            public static class CONFIDENCE_INTERVAL_LOWER
            {
                public const string Id = "CONFIDENCE_INTERVAL_LOWER";
                public const string Name = "Confidence interval - Lower";
                public const string Description = "Lower measure of the probability that the observation value will fall above this value.";
            }

            public static class SUBGROUP
            {
                public static class AGE
                {
                    public const string Id = "AGE";
                    public const string Description = "Age - or age range - of the individuals the observation refers to.";
                }

                public static class SEX
                {
                    public const string Id = "SEX";
                    public const string Description = "Gender condition: male or female. This dimension applies only if data can be dissaggregated by sex.";
                }

                public static class LOCATION
                {
                    public const string Id = "LOCATION";
                    public const string Description = "Refers to a disaggregation within the Reference Area the data alludes; normally National (total), Urban or Rural - although additional disaggregations are possible within an area (e.g. subUrban).";
                }

                public const string Description = "A subgroup concept.";
            }
        }

        public static class ConceptScheme
        {
            public static class DSD
            {
                public const string Id = "CS_DSD_DevInfo";
                public const string Version = "7.0";
                public const string Name = "Concept Scheme for DSD";
                public const string Description = "List of Concepts to be used in DSD";
                public const string FileName = "DSD_Concepts.xml";
            }

            public static class MSD_Area
            {
                public const string Id = "CS_MSD_Area_DevInfo";
                public const string Version = "7.0";
                public const string Name = "Concept Scheme for Area MSD";
                public const string Description = "List of Concepts to be used in MSD for Areas.";
                public const string FileName = "MSD_Area_Concepts.xml";
            }

            public static class MSD_Indicator
            {
                public const string Id = "CS_MSD_Indicator_DevInfo";
                public const string Version = "7.0";
                public const string Name = "Concept Scheme for Indicator MSD";
                public const string Description = "List of Concepts to be used in MSD for Indicators.";
                public const string FileName = "MSD_Indicator_Concepts.xml";
            }

            public static class MSD_Source
            {
                public const string Id = "CS_MSD_Source_DevInfo";
                public const string Version = "7.0";
                public const string Name = "Concept Scheme for Source MSD";
                public const string Description = "List of Concepts to be used in MSD for Sources.";
                public const string FileName = "MSD_Source_Concepts.xml";
            }
        }

        public static class DSD
        {
            public const string Id = "DSD_DevInfo";
            public const string Version = "7.0";
            public const string Name = "DevInfo DataStructures";
            public const string Description = "Concept schemes and Data structure may vary for various Devinfo databases to accomodate dimension specific to those databases";
            public const string FileName = "DSD.xml";
        }

        public static class MSD
        {
            public static class Area
            {
                public const string Id = "MSD_Area_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Area Metadata Structure";
                public const string Description = "Concept schemes and Metadata Structure may vary for various Devinfo databases to accomodate metadata attributes specific to those databases";
                public const string FileName = "MSD_Area.xml";

                public const string MetadataTargetId = "Target_Area";
                public const string IdentifiableObjectTargetId = "IO_Target_Area";
                public const string ReportStructureId = "RS_Area";
            }

            public static class Indicator
            {
                public const string Id = "MSD_Indicator_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Indicator Metadata Structure";
                public const string Description = "Concept schemes and Metadata structure may vary for various Devinfo databases to accomodate metadata attributes specific to those databases";
                public const string FileName = "MSD_Indicator.xml";

                public const string MetadataTargetId = "Target_Indicator";
                public const string IdentifiableObjectTargetId = "IO_Target_Indicator";
                public const string ReportStructureId = "RS_Indicator";
            }

            public static class Source
            {
                public const string Id = "MSD_Source_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Source Metadata Structure";
                public const string Description = "Concept schemes and Metadata structure may vary for various Devinfo databases to accomodate metadata attributes specific to those databases";
                public const string FileName = "MSD_Source.xml";

                public const string MetadataTargetId = "Target_Source";
                public const string IdentifiableObjectTargetId = "IO_Target_Source";
                public const string ReportStructureId = "RS_Source";
            }
        }

        public static class DFD
        {
            public const string Id = "DF_DevInfo";
            public const string Version = "7.0";
            public const string Name = "DevInfo DataFlows";
            public const string Description = "Data Flow Definition refers a Data Strucuture definition and is linked to Data Providers through Provision agreements.";
            public const string FileName = "DFD.xml";
        }

        public static class MFD
        {
            public static class Area
            {
                public const string Id = "MF_Area_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Metadata Flow for Area";
                public const string Description = "Metadata Flow Definition refers a Metadata Strucuture definition and is linked to Data Providers through Provision agreements.";
                public const string FileName = "MFD_Area.xml";
            }

            public static class Indicator
            {
                public const string Id = "MF_Indicator_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Metadata Flow for Indicator";
                public const string Description = "Metadata Flow Definition refers a Metadata Strucuture definition and is linked to Data Providers through Provision agreements.";
                public const string FileName = "MFD_Indicator.xml";
            }

            public static class Source
            {
                public const string Id = "MF_Source_DevInfo";
                public const string Version = "7.0";
                public const string Name = "DevInfo Metadata Flow for Source";
                public const string Description = "Metadata Flow Definition refers a Metadata Strucuture definition and is linked to Data Providers through Provision agreements.";
                public const string FileName = "MFD_Source.xml";
            }
        }

        public static class CategoryScheme
        {
            public static class Sector
            {
                public const string Id = "CS_SC";
                public const string Version = "7.0";
                public const string Name = "Sectors";
                public const string Description = "Categories of type Sector";
                public const string FileName = "SC.xml";
            }

            public static class Goal
            {
                public const string Id = "CS_GL";
                public const string Version = "7.0";
                public const string Name = "Goals";
                public const string Description = "Categories of type Goal";
                public const string FileName = "GL.xml";
            }

            public static class Theme
            {
                public const string Id = "CS_TH";
                public const string Version = "7.0";
                public const string Name = "Themes";
                public const string Description = "Categories of type Theme";
                public const string FileName = "TH.xml";
            }

            public static class Source
            {
                public const string Id = "CS_SR";
                public const string Version = "7.0";
                public const string Name = "Sources";
                public const string Description = "Categories of type Source";
                public const string FileName = "SR.xml";
            }

            public static class Convention
            {
                public const string Id = "CS_CV";
                public const string Version = "7.0";
                public const string Name = "Conventions";
                public const string Description = "Categories of type Convention";
                public const string FileName = "CV.xml";
            }

            public static class Framework
            {
                public const string Id = "CS_FR";
                public const string Version = "7.0";
                public const string Name = "Frameworks";
                public const string Description = "Categories of type Framework";
                public const string FileName = "FR.xml";
            }

            public static class Institution
            {
                public const string Id = "CS_IT";
                public const string Version = "7.0";
                public const string Name = "Institutions";
                public const string Description = "Categories of type Institution";
                public const string FileName = "IT.xml";
            }
        }

        public static class Categorization
        {
            public const string Version = "7.0";
            public const string Prefix = "CTGZ_";
            public const string Name = "Categorization Artefact.";
            public const string Description = "This artefact is being used to associate a category to a code.";
        }

        public static class CodeList
        {
            public static class Area
            {
                public const string Id = "CL_AREA";
                public const string Version = "7.0";
                public const string Name = "Area";
                public const string Description = "List of Areas.";
                public const string FileName = "Area.xml";
            }

            public static class Indicator
            {
                public const string Id = "CL_INDICATOR";
                public const string Version = "7.0";
                public const string Name = "Indicator";
                public const string Description = "List of Indicators.";
                public const string FileName = "Indicator.xml";
            }

            public static class Unit
            {
                public const string Id = "CL_UNIT";
                public const string Version = "7.0";
                public const string Name = "Unit";
                public const string Description = "List of Units.";
                public const string FileName = "Unit.xml";
            }

            public static class Subgroups
            {
                public const string Version = "7.0";
                public const string Description = "List of ";
            }

            public static class SubgroupType
            {
                public const string Id = "CL_SUBGROUPTYPES";
                public const string Version = "7.0";
                public const string Name = "Subgroup Type";
                public const string Description = "List of SubgroupTypes.";
                public const string FileName = "SubgroupType.xml";
            }

            public static class SubgroupVal
            {
                public const string Id = "CL_SUBGROUPVALUES";
                public const string Version = "7.0";
                public const string Name = "Subgroup Value";
                public const string Description = "List of Subgroup Values.";
                public const string FileName = "SubgroupVal.xml";
            }

            public static class IUS
            {
                public const string Id = "CL_IUS";
                public const string Version = "7.0";
                public const string Name = "Indicator-Unit-Subgroup";
                public const string Description = "List of IUS.";
                public const string FileName = "IUS.xml";
            }
        }

        public static class Complete_XML
        {
            public const string Id = "Complete";
            public const string Version = "7.0";
            public const string FileName = "Complete.xml";
        }

        public static class Summary_XML
        {
            public const string Id = "Summary";
            public const string Version = "7.0";
            public const string FileName = "Summary.xml";
        }

        public static class Report
        {
            public const string Id = "Report";
            public const string Version = "7.0";
            public const string FileName = "Report.xls";
        }

        public static class MaintenanceAgencyScheme
        {
            public const string Id = "AGENCIES";
            public const string AgencyId = "SDMX";
            public const string Version = "1.0";
            public const string Prefix = "MA_";
            public const string Name = "Maintenance Agency Scheme";
            public const string Description = "Listing of all maintenance agencies.";
            public const string FileName = "MaintenanceAgencies.xml";
        }

        public static class DataConsumerScheme
        {
            public const string Id = "DATA_CONSUMERS";
            public const string AgencyId = "SDMX";
            public const string Version = "1.0";
            public const string Prefix = "DC_";
            public const string Name = "Data Consumer Scheme";
            public const string Description = "Listing of all data consumers.";
            public const string FileName = "DataConsumers.xml";
        }

        public static class DataProviderScheme
        {
            public const string Id = "DATA_PROVIDERS";
            public const string AgencyId = "SDMX";
            public const string Version = "1.0";
            public const string Prefix = "DP_";
            public const string Name = "Data Provider Scheme";
            public const string Description = "Listing of all data providers.";
            public const string FileName = "DataProviders.xml";
        }

        public static class PA
        {
            public const string Version = "7.0";
            public const string Prefix = "PA_";
            public const string Name = "Devinfo Provision Agreement";
            public const string Description = "A provision agreement links a data provider to a data flow definition";
        }

        public static class Constraint
        {
            public const string Version = "7.0";
            public const string Prefix = "CNS_";
            public const string Name = "Constraint For ";
            public const string Description = "Constraints contains a collection of constraint descriptions. This container may contain both attachment and content constraints. The constraints may be detailed in full, or referenced from an external structure document or registry service.";
           
        }

        public static class StructureSet
        {
            public const string id = "StructureSet_Mapping";
            public const string version = "7.0";

            public static class CodelistMap
            {
                public const string prefix = "CodelistMap_";
                public const string name = "Mapping of DevInfo Indicators, Units, Age, Sex and Location with UNSD Indicators, Units, Age, Sex and Location respectively.";

                public static class Indicator
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Indicator.Name;
                    public const string name = "Mapping of DevInfo Indicators with UNSD Indicators.";
                }

                public static class Unit
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + DevInfo.Lib.DI_LibSDMX.Constants.CodeList.Unit.Name;
                    public const string name = "Mapping of DevInfo Units with UNSD Units.";
                }

                public static class Age
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + "Age";
                    public const string name = "Mapping of DevInfo Age with UNSD Age.";
                }

                public static class Sex
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + "Sex";
                    public const string name = "Mapping of DevInfo Sex with UNSD Sex.";
                }

                public static class Location
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + "Location";
                    public const string name = "Mapping of DevInfo Location with UNSD Location.";
                }

                public static class Area
                {
                    public const string id = Constants.StructureSet.CodelistMap.prefix + "Area";
                    public const string name = "Mapping of DevInfo Area with UNSD Area.";
                }

                public const string FileName = "CodelistMapping.xml";
            }

            public static class ConceptSchemeMap
            {
                public const string prefix = "ConceptSchemeMap_";
                public const string name = "Mapping of DevInfo Metadata Concept Scheme with UNSD Metadata Concept Scheme.";

                public static class MetadataMap
                {
                    public const string FileName = "MetadataMapping.xml";
                    public const string id = Constants.StructureSet.ConceptSchemeMap.prefix + DevInfo.Lib.DI_LibSDMX.Constants.ConceptScheme.MSD_Indicator.Id;
                    public const string name = "Mapping of DevInfo Metadata with UNSD Metadata.";
                }
            }
        }
        
        public static class ValidationMessages
        {
            public const string Non_Existing_Mandatory_Dimension = "Non Existing Mandatory Dimension : ";
            public const string Invalid_Dimension = "Invalid Dimension : ";
            public const string Invalid_Attribute = "Invalid Attribute : ";
            public const string Invalid_Primary_Measure = "Invalid Primary Measure : ";
            public const string Invalid_Code = "Invalid Code : ";
            public const string Invalid_MFD_Selected = "Metadata Flow for which Metadata is being registered does not refer to the same DSD as specified in the Metadata File.";
            public const string Invalid_Referred_MSD_Id = "Invalid MSD Referred in the Metadata : ";
            public const string Invalid_Metadata_Report = "Invalid Metadata Report : ";
            public const string Invalid_Metadata_Target = "Invalid Metadata Target : ";
            public const string Invalid_Metadata_Target_Object_Reference = "Invalid Metadata Target Object Reference";
            public const string Invalid_Metadata_Reported_Attribute = "Invalid Metadata Reported Attribute : ";
            public const string Invalid_Report_Structure = "Invalid Report Structure";
        }

        public static class SDMXWebServices
        {
            public const string xmlns = "xmlns";

            public static class SOAPTags
            {
                public const string faultcode = "faultcode";
                public const string faultstring = "faultstring";
                public const string faultactor = "faultactor";
                public const string detail = "detail";
                public const string Fault = "Fault";
                public const string Envelope = "Envelope";
            }

            public static class Namespaces
            {
                public static class Prefixes
                {
                    public const string SOAP = "soap";
                    public const string SDMXError = "sdmxerror";
                    public const string SDMXWebservice = "sdmxws";
                }

                public static class URIs
                {
                    public const string SDMXError = "http://www.SDMX.org/resources/SDMXML/webservice/iso/v_776 2_0_draft/error";
                    public const string SDMXWebservice = "http://www.devinfo.info/";
                }
            }

            public static class Exceptions
            {
                public static class NotImplemented
                {
                    public const string Message = "Functionality not implemented.";
                    public const string Code = "501";
                }

                public static class ServerError
                {
                    public const string Message = "Internal server error.";
                    public const string Code = "500";
                }

                public static class InvalidSyntax
                {
                    public const string Message = "Invalid parameter value passed to web service.";
                    public const string Code = "140";
                }

                public static class NoResults
                {
                    public const string Message = "No results found.";
                    public const string Code = "100";
                }
            }
        }
    }
}