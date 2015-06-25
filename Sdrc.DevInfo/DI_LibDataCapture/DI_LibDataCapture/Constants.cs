using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture
{
    [Serializable()]
    internal class Constants
    {
        /// <summary>
        /// Returns mark for mandatoryQuestion. Use this mark to display unanswered mandatory questions in jumpbox combobox
        /// </summary>
        internal const string MarkForMandatoryQuestion="*";

        /// <summary>
        /// Returns the Area XML file name
        /// </summary>
        internal const string AreaFileName = "AREA_DATA.xml";

        /// <summary>
        /// Returns the Country ISO Codes XML file name
        /// </summary>
        internal const string SourceFileName = "Country_ISO_Codes.xml";

        /// <summary>
        /// Returns the Jump Next Operator
        /// </summary>
        internal const string JumpNextSeparator = "&";

        /// <summary>
        /// Retruns the separator used to concatenate jump next questions like "1_2 & 3"
        /// </summary>
        internal const string JumpNextQuestionSeparator = " & ";

        /// <summary>
        /// Retruns the separator used to concatenate string IDs in jump next string like "1_2 & 3,2_4"
        /// </summary>
        internal const string JumpNextStringSeparator = ",";


        /// <summary>
        /// Returns the Grid Seprator
        /// </summary>
        internal const string GridSeprator = "~";

        /// <summary>
        /// Returns the Question prefix
        /// </summary>
        internal const string QuestionPrefix = "Q";

        /// <summary>
        /// Returns the grid cell separator
        /// </summary>
        internal const string GridCellSeparator = "{{~}}";

        /// <summary>
        /// Returns the area value separator
        /// </summary>
        internal const string AreaValueSeparator = "{([~])}";

        /// <summary>
        /// Returns the string ID separator
        /// </summary>
        internal const string StringIDSeparator = ",";

        /// <summary>
        /// Returns the jumpQuestionSeprator
        /// </summary>
        internal const string JumpQuestionSeprator = "_";

        /// <summary>
        /// Returns the constants for dependent question
        /// </summary>
        internal const string Dependentquestion = ",";

        /// <summary>
        /// Retruns the page string
        /// </summary>
        internal const string PageText = "Page";

        /// <summary>
        /// Returns the key for getting the total questions 
        /// </summary>
        internal const string TotalQuestionsKey = "TotalQuestions";

        /// <summary>
        /// Returns the source separator.
        /// </summary>
        internal const string SourceSeparator = "_";

    }
}
