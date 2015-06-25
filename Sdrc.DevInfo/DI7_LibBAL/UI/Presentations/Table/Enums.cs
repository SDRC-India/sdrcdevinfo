using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Table
{
	public enum TablePresentationMessages
	{
		None=0,
		ColumnsExceeded=1
	}

	/// <summary>
	/// Specifies aggregate type
	/// </summary>
	public enum AggregateType
    {
		/// <summary>
		/// Sum type field
		/// </summary>
		Sum=0,
		/// <summary>
		/// Count type field
		/// </summary>
		Count=1,
		/// <summary>
		/// Mean type field
		/// </summary>
		Mean = 2,
		/// <summary>
		/// Minimum type field
		/// </summary>
		Minimum = 3,
		/// <summary>
		/// Maximum type field
		/// </summary>
		Maximum = 4
	}

	/// <summary>
	/// Describes the display of Notes and Footnote on the .XLS file of Step 6.
	/// </summary>
	/// <remarks>
	/// If Notes or Footnote is selected then "One" column gets inserted after each datacolumn in the .XLS file
	/// If Both is selected then "Two" columns gets inserted after each datacolumn in the .XLS file
	/// </remarks>
	public enum FootnoteCommentType
	{
		/// <summary>
		/// None type field.
		/// </summary>
		None=0,
		/// <summary>
		/// Notes type field.
		/// </summary>
		Comment=1,
		/// <summary>
		/// FootNote type field.
		/// </summary>
		Footnote=2,
		/// <summary>
		/// Both type field.
		/// </summary>
		Both=3
	}

	/// <summary>
	/// Describe RowType and is used for applying colors based on row type.
	/// </summary>
	public enum RowType
	{
		/// <summary>
		/// ICAggregate type field
		/// </summary>
		ICAggregate=0,
		
		/// <summary>
		/// SubAggregate type field
		/// </summary>
		SubAggregate=1,
		
		/// <summary>
		/// GroupAggregate type field
		/// </summary>
		GroupAggregate=2,

		/// <summary>
		/// EmptyRow type field
		/// </summary>
		EmptyRow=3,

		/// <summary>
		/// RDataRow type field.
		/// </summary>
		DataRow=4,
		/// <summary>
		/// Row type specifying header of table
		/// </summary>
		TableHeaderRow=5,
		/// <summary>
		/// Row type for Suppress rows
		/// </summary>
		SupressRow=6,
        /// <summary>
        /// Row type for IUS - Used for frequency table
        /// </summary>
        IUSRow=7
	}

	/// <summary>
	/// Defines what should be the output of TablePresentation
	/// </summary>
	/// <remarks>
	/// MHT will be used in case of web application.
	/// </remarks>
	public enum PresentationOutputType
	{	
		/// <summary>
		/// ExcellSheet Type field
		/// </summary>
		ExcellSheet=0,

		/// <summary>
		/// MHT Type field
		/// </summary>
		MHT=1
	}

	/// <summary>
	/// Defines the type of column.
	/// </summary>
	public enum TableColumnType
	{
		/// <summary>
		/// DataValue type field
		/// </summary>
		DataValue = 0,
		/// <summary>
		/// Footnote type
		/// </summary>
		FootNote = 1,
		/// <summary>
		/// Comment type
		/// </summary>
		Comment = 2,
		/// <summary>
		/// Denominator type
		/// </summary>
		Denominator = 3,
		/// <summary>
		/// RowHeader Type
		/// </summary>
		RowHeader = 4,
		/// <summary>
		/// Others type
		/// </summary>
		Others = 5
	}

    /// <summary>
    /// Enum to define the orientation while printing.
    /// </summary>
    public enum Orientation
    {
        Horizontal = 0,
        vertical = 1
    }
}
