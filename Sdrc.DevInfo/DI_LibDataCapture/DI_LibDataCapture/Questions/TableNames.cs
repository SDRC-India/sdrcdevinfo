using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    public class TableNames
    {
        /// <summary>
        /// Returns the Question table name
        /// </summary>
        public const string QuestionTable = "Question";

        /// <summary>
        /// Returns the String table name
        /// </summary>
        public const string StringTable = "StringTbl";

        /// <summary>
        /// Returns the Header table name
        /// </summary>
        public const string HeaderTable = "HeaderTbl";

        /// <summary>
        /// Returns the MultipleOptionVals table name
        /// </summary>
        public const string MultipleOptionValsTable = "MultipleOptionValsTable";

        /// <summary>
        /// Returns the OptionSubgroupMapping table name
        /// </summary>
        public const string OptionSubgroupMappingTable = "OptionSubgroupMappingTable";

        /// <summary>
        /// Returns the Page table name
        /// </summary>
        public const string PagesTable = "Pages";

        /// <summary>
        /// Returns the Grid table name
        /// </summary>
        public const string GridTypeTable = "GridTypeTable";

        /// <summary>
        /// Returns the Interface table name
        /// </summary>
        public const string InterfaceTable = "Interface";

        /// <summary>
        /// Returns the TempGrid table name
        /// </summary>
        public const string TempGridTypeTable = "Temp_GridTypeTable_";

    }
}
