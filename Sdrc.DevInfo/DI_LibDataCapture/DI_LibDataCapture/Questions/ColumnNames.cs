using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions.ColumnNames
{
    [Serializable()]
    /// <summary>
    /// Returns  question table columns 
    /// </summary>
    public class QuestionTableColumns
    {
        /// <summary>
        /// Returns QuestionKey column
        /// </summary>
        public const string QuestionKey = "Key";

        /// <summary>
        /// Returns the question test column like "How many bed are there in hospital"
        /// </summary>
        public const string QuestionText = "IUS";

        /// <summary>
        /// Returns the indicator Global column
        /// </summary>
        public const string IndicatorGID = "I_Gid";

        /// <summary>
        /// Returns the Unit Global column
        /// </summary>
        public const string UnitGid = "U_Gid";

        /// <summary>
        /// Returns the subgroup GID column
        /// </summary>
        public const string SubGroupGid = "S_Gid";

        /// <summary>
        /// Returns the answer Type column like "TBN".....
        /// </summary>
        public const string AnswerType = "AnsType";

        /// <summary>
        /// Returns the String ID column like yes,No....
        /// </summary>
        public const string StringID = "Str_ID";

        /// <summary>
        /// Returns the dataValue column
        /// </summary>
        public const string DataValue = "DATA_VALUE";

        /// <summary>
        /// Returns question text  column like Q1.a, Q1.b ...
        /// </summary>
        public const string QuestionNo = "Q_Txt";

        /// <summary>
        /// Returns the Question Header column
        /// </summary>
        public const string QuestionHeader = "Q_Header";

        /// <summary>
        /// Returns the source column
        /// </summary>
        public const string Source = "SOURCE";

        /// <summary>
        /// Returns the timeperiod column
        /// </summary>
        public const string TimePeriod = "TIMEPERIOD";

        /// <summary>
        /// Returns the jumpNext column
        /// </summary>
        public const string JumpNext = "JumpNext";

        /// <summary>
        /// Returns the Jump Previous column
        /// </summary>
        public const string JumpPrevious = "JumpPrevious";

        /// <summary>
        /// Returns the numeric Value column
        /// </summary>
        public const string NumericValue = "Numeric_Value";

        /// <summary>
        /// Returns the required column.
        /// </summary>
        public const string Required = "Required";

        /// <summary>
        /// Returns the minimum column
        /// </summary>
        public const string Minimum = "Min";

        /// <summary>
        /// Returns the maximum column
        /// </summary>
        public const string Maximum = "Max";

        /// <summary>
        /// Returns the default value column
        /// </summary>
        public const string Default = "Default_Value";

        /// <summary>
        /// Returns the visibile column
        /// </summary>
        public const string Visible = "Visible";

        /// <summary>
        /// Returns Grid Id column
        /// </summary>
        public const string GridID = "Grid_ID";

        /// <summary>
        /// Return the Font
        /// </summary>
        public const string Font = "FONT_FAMILY";
    }

    [Serializable()]
    /// <summary>
    /// Returns  string table columns 
    /// </summary>
    public class StringTableColumns
    {
        /// <summary>
        /// Returns the string ID like 1,2
        /// </summary>
        public const string StringId = "ID";

        /// <summary>
        /// Returns the string like yes or no
        /// </summary>
        public const string DisplayString = "Display_String";

        /// <summary>
        /// Retruns subgroupval gid column
        /// </summary>
        public const string SubgroupValGId = "Subgroup_VAL_GID";
    }

    [Serializable()]
    /// <summary>
    /// Returns OptionSubgroupMapping table columns
    /// </summary>
    public class OptionSubgroupMappingTableColumns
    {
        /// <summary>
        /// Returns the string ID like 1,2
        /// </summary>
        public const string StringId = "ID";

        /// <summary>
        /// Retruns subgroupval gid column
        /// </summary>
        public const string SubgroupValGId = "Subgroup_VAL_GID";

        /// <summary>
        /// Returns QuestionKey column
        /// </summary>
        public const string QuestionKey = "Key";
    }

    [Serializable()]
    /// <summary>
    /// Returns  header table columns 
    /// </summary>
    public class HeaderTableColumns
    {
        /// <summary>
        /// Returns the question header id column
        /// </summary>
        public const string HeaderId = "ID";

        /// <summary>
        /// returns the question heading column
        /// </summary>
        public const string Header = "Q_Heading";
    }

    [Serializable()]
    /// <summary>
    /// Returns  multipleoption value table columns .
    /// </summary>
    public class MultipleOptionValueTableColumns
    {
        /// <summary>
        /// Returns the item Values
        /// </summary>
        public const string ItemValue = "Item";

        /// <summary>
        /// Returns the value column
        /// </summary>
        public const string Value = "Value";
    }

    [Serializable()]
    /// <summary>
    /// Returns  pages table columns 
    /// </summary>
    public class PagesTableColumns
    {
        /// <summary>
        /// Returns the pageNumber
        /// </summary>
        public const string PageNumber = "Page_No";

        /// <summary>
        /// Returns the questions
        /// </summary>
        public const string Questions = "Questions";
    }

    [Serializable()]
    /// <summary>
    /// Returns  Grid table columns 
    /// </summary>
    public class GridTableColumns
    {
        /// <summary>
        /// Returns the Grid Id column
        /// </summary>
        public const string GridId = "Grid_ID";

        /// <summary>
        /// Returns the grid header column. Grid header column contains header Id.
        /// </summary>
        public const string GridHeader = "Header";

        /// <summary>
        /// Returns the gris question text column
        /// </summary>
        public const string GridQuestionText = "Text";

        /// <summary>
        /// Returns the number of rows in grid
        /// </summary>
        public const string Rows = "Rows";

        /// <summary>
        /// Returns the number of column in grid
        /// </summary>
        public const string Columns = "Columns";
    }

    [Serializable()]
    /// <summary>
    /// Returns  interface table columns 
    /// </summary>
    public class InterfaceColumns
    {
        /// <summary>
        /// Return the key column
        /// </summary>
        public const string InterfaceKey = "Key";

        /// <summary>
        /// Return the source column
        /// </summary>
        public const string Source = "Source";

        /// <summary>
        /// return the translate column
        /// </summary>
        public const string Translate = "Translate";
    }

    [Serializable()]
    public class AreaXmlColumns
    {
        /// <summary>
        /// returns the Area Nid column
        /// </summary>
        public const string AreaNid = "Area_Nid";

        /// <summary>
        /// returns the parent Nid column
        /// </summary>
        public const string ParentNid = "Area_Parent_Nid";

        /// <summary>
        /// Returns the AreaId column
        /// </summary>
        public const string AreaId = "Area_ID";

        /// <summary> 
        /// Returns the area name column
        /// </summary>
        public const string AreaName = "Area_Name";

        /// <summary>
        /// Returns the area Global ID column
        /// </summary>        
        public const string AreaGid = "Area_GID";

        /// <summary>
        /// returns the area level column
        /// </summary>
        public const string AreaLevel = "Area_Level";

        /// <summary>
        /// Returns the area map column
        /// </summary>
        public const string AreaMap = "Area_Map";

        /// <summary>
        /// Returns the Area Block column
        /// </summary>
        public const string AreaBlock = "Area_Block";

        /// <summary>
        /// returns the area global column.
        /// </summary>
        public const string AreaGlobal = "Area_Global";
    }

}
