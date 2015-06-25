using System;
namespace DevInfo.Lib.DI_LibSDMX
{
    /// <summary>
    /// Enumeration listing different artefact types.
    /// </summary>
    public enum ArtefactTypes
    {
        None = -1,
        // Maintenance Agency
        MA = 0, 
        //Data Provider Scheme
        DP = 1, 
        // Data Consumer Scheme
        DC = 2, 
        // Data Structure Definition
        DSD = 3, 
        // Data Flow Definition
        DFD = 4, 
        // Concept Scheme
        ConceptS = 5, 
        // Category Scheme
        CategoryS = 6, 
        // Codelist
        CL = 7, 
        // Provision Agreement
        PA = 8, 
        // Constraint
        Constraint = 9, 
        // Registration Request
        Registration = 10, 
        // Subscription Request
        Subscription = 11,
        // An xml file with DSD, Concept Schemes and complete codelists
        Complete = 12,
        // An xml file with DSD, Concept Schemes and empty codelists
        Summary = 13,
        // An excel file with DSD, Concept Schemes and codelists information
        Report = 14,
        // Metadata Structure definition.
        MSD = 15,
        // Metadata Flow definition.
        MFD = 16,
        // Categorisation.
        Categorisation = 17,
        // Mapping.
        Mapping = 18,
        //Header
        Header=19

    }

    /// <summary>
    /// Enumeration listing different formats of SDMX-ML data.
    /// </summary>
    public enum DataFormats
    {
        Generic,
        GenericTS,
        StructureSpecific,
        StructureSpecificTS
    }

    /// <summary>
    /// Enumeration listing different formats of SDMX Queries.
    /// </summary>
    public enum QueryFormats
    {
        Generic,
        GenericTS,
        StructureSpecific,
        StructureSpecificTS
    }

    /// <summary>
    /// Bitwise Enum for listing different combination of codelist types.
    /// </summary>
    [Flags]
    public enum CodelistTypes
    {
        ALL = 1,
        Area = 2,
        Indicator = 4,
        Unit = 8,
        Subgroups = 16,
        SubgroupType = 32,
        SubgroupVal = 64,
        IUS = 128
    }

    /// <summary>
    /// Bitwise Enum for listing different combination of category scheme types.
    /// </summary>
    ///<remarks>For selecting Sector and Goal  CategorySchemeTypes.Sector | CategorySchemeTypes.Goal</remarks>
    [Flags]
    public enum CategorySchemeTypes
    {
        ALL = 1,
        Sector = 2,
        Goal = 4,
        Theme = 8,
        Source = 16,
        Convention = 32,
        Framework = 64,
        Institution = 128
    }

    /// <summary>
    /// Bitwise Enum for listing different combination of Concept Scheme Types.
    /// </summary>
    ///<remarks>For selecting DSD and MSD_Indicator  CategorySchemeTypes.DSD | CategorySchemeTypes.MSD_Indicator</remarks>
    [Flags]
    public enum ConceptSchemeTypes
    {
        ALL = 1,
        DSD = 2,
        MSD_Indicator = 4,
        MSD_Area = 8,
        MSD_Source = 16
    }

    /// <summary>
    /// Bitwise Enum for listing different combination of MSD.
    /// </summary>
    ///<remarks>For selecting MSD_Indicator and MSD_Area  MSDTypes.MSD_Indicator | MSDTypes.MSD_Area</remarks>
    [Flags]
    public enum MSDTypes
    {
        ALL = 1,
        MSD_Indicator = 2,
        MSD_Area = 4,
        MSD_Source = 8
    }

    /// <summary>
    /// Enum for listing different types of Metadata in DI database.
    /// </summary>
    public enum MetadataTypes
    {
        //-- Already declared in DI_LibDAL
        Indicator = DevInfo.Lib.DI_LibDAL.Queries.MetaDataType.Indicator,
        Area = DevInfo.Lib.DI_LibDAL.Queries.MetaDataType.Map,
        Source = DevInfo.Lib.DI_LibDAL.Queries.MetaDataType.Source,
        Layer = 4
    }

    /// <summary>
    /// Enumeration listing different kinds of users.
    /// </summary>
    public enum UserTypes
    {
        Agency = 0,
        Consumer = 1,
        Provider = 2
    }

    /// <summary>
    /// Enumeration listing the levels of validity/invalidity of a SDMX Data file.
    /// </summary>
    public enum SDMXValidationStatus
    {
        /// <summary>
        /// SDMX data file doesnot have a valid xml structure.
        /// </summary>
        Xml_Invalid,

        /// <summary>
        /// SDMX data file is not compliant to appropriate sdmx schemas - version 2.1.
        /// </summary>
        SDMX_Invalid,

        /// <summary>
        /// SDMX data file contains dimensions not compliant to the DSD(data structure definition).
        /// </summary>
        Dimension_Invalid,

        /// <summary>
        /// Code specified against the dimensions does not exist in their respective Codelists.
        /// </summary>
        Code_Invalid,

        /// <summary>
        /// SDMX data file is valid.
        /// </summary>
        Valid
    }

    /// <summary>
    /// Enumeration listing the levels of validity/invalidity of a DSD file.
    /// </summary>
    public enum DSDValidationStatus
    {
        /// <summary>
        /// DSD file doesnot have a valid xml structure.
        /// </summary>
        Xml_Invalid,

        /// <summary>
        /// DSD is not compliant to appropriate sdmx schemas - version 2.1.
        /// </summary>
        SDMX_Invalid,

        /// <summary>
        /// Dimensions in the DSD is not compliant to the Dev Info Master DSD(data structure definition).
        /// </summary>
        DSD_Dimensions_Invalid,

        /// <summary>
        /// DSD does not contains all the mandatory dimensions(Indicator,Unit,Area,Time Period and Obs_Val) that exist in Dev Info Master DSD(data structure definition).
        /// </summary>
        DSD_MandatoryDimensions_Invalid,

        /// <summary>
        /// Attribute in the DSD is not compliant to the Dev Info Master DSD(data structure definition).
        /// </summary>
        DSD_Attribute_Invalid,

        /// <summary>
        /// Primary Measure in the DSD is not compliant to the Dev Info Master DSD(data structure definition).
        /// </summary>
        DSD_Primary_Measure_Invalid,

        /// <summary>
        /// DSD is valid.
        /// </summary>
        Valid
    }

    /// <summary>
    /// Enumeration listing the levels of validity/invalidity of a Metadata file.
    /// </summary>
    public enum MetadataValidationStatus
    {
        /// <summary>
        /// Metadata file doesnot have a valid xml structure.
        /// </summary>
        Xml_Invalid,

        /// <summary>
        /// Metadata is not compliant to appropriate sdmx schemas - version 2.1.
        /// </summary>
        SDMX_Invalid,

        /// <summary>
        /// MFD related to the Referred MSD in the Metadata File is invalid.
        /// </summary>
        MFD_Selection_Invalid,

        /// <summary>
        /// MSD referred in the Metadata File is invalid.
        /// </summary>
        Referred_MSD_Invalid,

        /// <summary>
        /// Report specified in the Metadata File is invalid.
        /// </summary>
        Metadata_Report_Invalid,

        /// <summary>
        /// Target specified in the Metadata File is invalid.
        /// </summary>
        Metadata_Target_Invalid,

        /// <summary>
        /// Target Object Reference specified in the Metadata File is invalid.
        /// </summary>
        Metadata_Target_Object_Reference_Invalid,

        /// <summary>
        /// Report Structure specified in the Metadata File is invalid.
        /// </summary>
        Metadata_Report_Structure_Invalid,

        /// <summary>
        /// Metadata File is valid.
        /// </summary>
        Valid
    }

    /// <summary>
    /// Enumeration listing the detail type in data queries.
    /// </summary>
    public enum DataReturnDetailTypes
    {
        Full = 0,
        DataOnly = 1,
        SeriesKeyOnly = 2,
        NoData = 3
    }

    /// <summary>
    /// Enumeration listing the different sdmx schemas.
    /// </summary>
    public enum SDMXSchemaType
    {
        Two_One = 1
    }

     /// <summary>
    /// Enumeration listing the different Mappings to be generated.
    /// </summary>
    public enum MappingType
    {
        Codelist = 0,
        Metadata = 1
    }
}