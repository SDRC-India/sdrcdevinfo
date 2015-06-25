using System;
using System.Text;

namespace DevInfo.Lib.DI_LibDATA
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

        public static class Xml
        {
            public const string version = "1.0";
            public const string encoding = "utf-8";
            public const string id = "id";
            public const string Xml_Lang = "xml:lang";
            public const string Root = "Root";
            public const string Data = "Data";
            public const string Observation = "Observation";
            public const string TIME_PERIOD = "TIME_PERIOD";
            public const string OBS_VALUE = "OBS_VALUE";
            public const string AREA = "AREA";
            public const string INDICATOR = "INDICATOR";
            public const string UNIT = "UNIT";
            public const string SOURCE = "SOURCE";
            public const string DENOMINATOR = "DENOMINATOR";
            public const string FOOTNOTES = "FOOTNOTES";
            public const string SUBGROUP = "SUBGROUP";
        }
    }
}