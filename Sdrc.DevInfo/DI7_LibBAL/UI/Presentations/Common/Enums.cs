using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations
{
    /// <summary>
    /// Specifies Fields type
    /// </summary>
	//public enum FieldType
	//{
	//    /// <summary>
	//    /// Enum for indicator
	//    /// </summary>
	//    Indicator = 0,
	//    /// <summary>
	//    /// Enum for indicator
	//    /// </summary>
	//    Unit = 1,
	//    /// <summary>
	//    ///	Enum for subgroup
	//    /// </summary>
	//    Subgroup = 2,
	//    /// <summary>
	//    /// Enum for location
	//    /// </summary>
	//    Location = 3,
	//    /// <summary>
	//    /// Enum for timeperiod
	//    /// </summary>
	//    TimePeriod = 4,
	//    /// <summary>
	//    /// Enum for source
	//    /// </summary>
	//    Source = 5,
	//    /// <summary>
	//    /// Enum for sex
	//    /// </summary>
	//    Sex = 6,
	//    /// <summary>
	//    ///	Enum for areaid
	//    /// </summary>
	//    AreaID = 7,
	//    /// <summary>
	//    /// Enum for areaname
	//    /// </summary>
	//    AreaName = 8,
	//    /// <summary>
	//    /// Enum for agegroup
	//    /// </summary>
	//    AgeGroup = 9,
	//    /// <summary>
	//    /// Enum for others
	//    /// </summary>
	//    Others = 10,
	//    /// <summary>
	//    /// Indicator Metadata type field
	//    /// </summary>
	//    MetadataIndicator = 11,

	//    /// <summary>
	//    /// Area Metadata type field
	//    /// </summary>
	//    MetadataArea = 12,

	//    /// <summary>
	//    /// Source Metadata type field
	//    /// </summary>
	//    MetadataSource = 13,

	//    /// <summary>
	//    /// NONE type field
	//    /// </summary>
	//    None=14,

	//    /// <summary>
	//    /// Denominator type field
	//    /// </summary>
	//    Denominator=15,

	//    /// <summary>
	//    /// DataValue type field
	//    /// </summary>
	//    DataValue=16,

	//     /// <summary>
	//    /// Source IC type field
	//    /// </summary>
	//    ICSource = 17,

	//     /// <summary>
	//    /// Sector IC type field
	//    /// </summary>
	//    ICSector = 18,

	//    /// <summary>
	//    /// Goal IC type field
	//    /// </summary>
	//    ICGoal = 19,

	//    /// <summary>
	//    /// Theame IC type field
	//    /// </summary>
	//    ICTheame = 20,

	//    /// <summary>
	//    /// Institute IC type field
	//    /// </summary>
	//    ICInstitute = 21,

	//    /// <summary>
	//    /// Convention IC type field
	//    /// </summary>
	//    ICConvention = 22
	//}

    /// <summary>
    /// Specify the enum for break type
    /// </summary>
    public enum BreakTypes
    { 
         /// <summary>
        /// Equal count break type 
        /// </summary>
        /// <remarks>Breaks are created in such a way so that count are equal for every legend.
        /// <para>In certain cases data value may be such that count may not be exactly equal</para>
        /// </remarks>
        EqualCount=0,

        /// <summary>
        /// Equal size break type
        /// </summary>
        /// <remarks>Breaks are created in such a way that difference between ranges are equal
        /// <para>In some cases, data value may be such that count may not be exactly equal</para>
        /// </remarks>
        EqualSize=1,

        /// <summary>
        /// Continuous break type
        /// </summary>
        /// <remarks> By default breaks are created with the logic of equal count
        /// <para> User can edit the to range and all other range are adjusted acordingly.</para>
        /// </remarks>
        Continuous=2,

  
        /// <summary>
        /// discontinuous break type
        /// </summary>
        /// <remarks>breaks are created with the logic of equal count
        /// <para>User can edit the from and to range according to their own choice </para>
        /// </remarks>        
        Discontinuous=3
    }

	public enum FieldSource
	{
		/// <summary>
		/// Rows FieldSource
		/// </summary>
		Rows=0,
		/// <summary>
		/// Column FieldSource
		/// </summary>
		Column=1,
		/// <summary>
		/// Available FieldSource
		/// </summary>
		Available=2,
		/// <summary>
		/// All FieldSource
		/// </summary>
		All=3
	}

	/// <summary>
	/// Specifies the Footnote display style.
	/// </summary>
	/// <remarks>
	/// Inline with data will get set in case of graph wizard.
	/// </remarks>
	public enum FootNoteDisplayStyle
	{
		/// <summary>
		/// Footnote Inline Display Style
		/// </summary>
		Inline=0,
		/// <summary>
		/// Footnote InlineWithData Display Style
		/// </summary>
		InlineWithData=1,
		/// <summary>
		/// Footnote Seprate Display Style
		/// </summary>
		Separate=2		
	}

    /// <summary>
    /// Specify the Chart orientation
    /// </summary>
    public enum TextOrientation
    {
        Horizontal,
        /// <summary>
        /// Downward
        /// </summary>
        VerticalLeftFacing,
        /// <summary>
        /// Upward
        /// </summary>
        VerticalRightFacing,
        Custom
    }

    /// <summary>
    /// Specify the Line draw style
    /// </summary>
    /// <remarks> To keep the backward compatibility, we are created this enum. Spreadhseet gear enum does not contain Enum value for solid </remarks>
    public enum LineDrawStyle
    {
        Solid,
        Dash,
        DashDot,
        DashDotDot,
        Dot
    }
}
