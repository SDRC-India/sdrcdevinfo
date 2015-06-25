
namespace DevInfo.Lib.DI_LibDAL.Queries
{
    /// <summary>
    /// Constants to define data type
    /// </summary>
    /// <remarks>Used for setting sorting related expression column values</remarks>
    public enum DataType
    {
        /// <summary>
        /// Numeric data type
        /// </summary>
        Numeric = 1,

        /// <summary>
        /// Textual Data Type
        /// </summary>
        Text = 2
    }

    /// <summary>
    /// Constants to define Filter criteria.
    /// </summary>
    public enum FilterFieldType
    {
        /// <summary>
        /// Skip Filter Criterion
        /// </summary>
        None = -1,
        NId = 0,
        ParentNId = 1,
        ID = 2,
        GId = 3,
        Name = 4,
        
        /// <summary>
        /// E.g. SubgroupType, IC_Type
        /// </summary>
        Type = 5,

        /// <summary>
        /// LIKE Clause
        /// </summary>
        Search = 6,
        Global = 7,
        NIdNotIn = 8,
        NameNotIn = 9,
        /// <summary>
        /// for area level and IC level
        /// </summary>
        Level=10,
        /// <summary>
        /// for area layer nid
        /// </summary>
        LayerNid=11
    }

    /// <summary>
    /// Constants to define Indicator Classifications.
    /// </summary>
    public enum ICType
    {
        Sector = 0,
        Goal = 1,
        CF = 2,
        Theme = 3,
        Source = 4,
        Institution = 5,
        Convention = 6
    }

    /// <summary>
    /// Constants to define Heavy or Light Queries.
    /// </summary>
    public enum FieldSelection
    {
        /// <summary>
        /// Only NId field will be returned
        /// </summary>
        NId = 0,
        /// <summary>
        /// Only textual fields (Name) will be returned
        /// </summary>
        Name = 1,
        /// <summary>
        /// Limited set of Fields will be returned
        /// </summary>
        /// <remarks>
        /// Generally Memo fields or blob fields of the table will be discarded
        /// </remarks>
        Light = 2,
        /// <summary>
        /// All applicable Fields will be returned
        /// </summary>
        Heavy = 3
    }

    /// <summary>
    /// I-Indicator; A-Area; S-Source
    /// </summary>
    public enum MetadataElementType
    {
        /// <summary>
        /// I
        /// </summary>
        Indicator,

        /// <summary>
        /// A
        /// </summary>
        Area,

        /// <summary>
        /// S
        /// </summary>
        Source
    }

    /// <summary>
    /// I-Indicator,A-Area,S-SubgroupVals,U-Unit,C-Classification,D-Data,MI-Metadata Indicator, MS-Metadata Source, MA-Metadata Area
    /// </summary>
    public enum IconElementType
    {

        /// <summary>
        /// I
        /// </summary>
        Indicator ,

        /// <summary>
        /// U
        /// </summary>
        Unit,

        /// <summary>
        /// S
        /// </summary>
        SubgroupVals,

        /// <summary>
        /// A
        /// </summary>
        Area,

        /// <summary>
        /// C
        /// </summary>
        IndicatorClassification ,

        /// <summary>
        /// D
        /// </summary>
        Data ,

        /// <summary>
        /// MI
        /// </summary>
        MetadataIndicator,

        /// <summary>
        /// MA
        /// </summary>
        MetadataArea,

        /// <summary>
        /// MS
        /// </summary>
        MetadataSource

    }

	/// <summary>
	/// Specifies the checked value for boolean fields
	/// </summary>
	public enum CheckedStatus
	{
        /// <summary>
		/// All rows to be considered irrespective of check status
		/// </summary>
		All = 0,

		/// <summary>
		/// Only unchecked (false) rows to be considered
		/// </summary>
		False = 1,

		/// <summary>
		/// Only checked (true) rows to be considered
		/// </summary>
		True = 2
	}

    /// <summary>
    /// Returns the subgroup type ( age , sex ,location or others).
    /// </summary>
    public enum SubgroupType
    {
        None=0,
        Sex=1,
        Location=2,
        Age=3,
        Others=4
        
    }

    /// <summary>
    /// Returns the Metadata type 
    /// </summary>
    public enum MetaDataType
    {
        Indicator = 1,
        Map = 2,
        Source = 3,
        Sector = 4,
        Goal = 5,
        CF = 6,
        Theme = 7,
        Institution = 8,
        Convention = 9,
        IndicatorClassification=10
    }


    /// <summary>
    /// constants for Concat and delimiters
    /// </summary>
    public struct Delimiter
    {
        /// <summary>
        /// "{[#]}" Delimiter for textual values like GIds, AreaID, TimePeriods, SourceNames
        /// </summary>
        public const string TEXT_DELIMITER = "{[#]}";

        /// <summary>
        /// "_{#}_"
        /// </summary>
        public const string TEXT_SEPARATOR = "_{#}_";

        /// <summary>
        /// ","
        /// </summary>
        public const string NUMERIC_DELIMITER = ",";

        /// <summary>
        /// "_"
        /// </summary>
        public const string NUMERIC_SEPARATOR = "_";

    }

    public enum SortingType
    {
        None,
        ASC,
        DESC
    }
    }