using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    internal static class Constants
    {
        internal static Dictionary<LogFileFontType, string> LogFileFontTypes;

        static Constants()
        {
            Constants.LogFileFontTypes = new Dictionary<LogFileFontType, string>();
            Constants.LogFileFontTypes.Add(LogFileFontType.ApplicationName, Constants.LogFontSize.ApplicationName);
            Constants.LogFileFontTypes.Add(LogFileFontType.H1, Constants.LogFontSize.H1 );
            Constants.LogFileFontTypes.Add(LogFileFontType.H2, Constants.LogFontSize.H2);
            Constants.LogFileFontTypes.Add(LogFileFontType.H3, Constants.LogFontSize.H3);
            Constants.LogFileFontTypes.Add(LogFileFontType.H4, Constants.LogFontSize.H4);
        }

        internal static class LogFontSize
        {
            internal const string ApplicationName = "style='font-size=48px;'";// For application
            internal const string H1 = "style='font-size=21px;'"; // main heading
            internal const string H2 = "style='font-size=18px;'"; // main heading value
            internal const string H3 = "style='font-size=18px; font-weight:bold;'"; // sub heading
            internal const string H4 = "style='font-size=15px;'"; // sub heading value
           
            
        }

        internal  const string EndDateTimeTag = "End Date & Time:";
        internal const string OutputFileWPathTag = "Output File Name & Path";

        internal const string ChangedInLast = Constants.EndDateTimeTag + "Update";
        internal const string ImageName ="yes.jpg";

        internal const string ProcessedFile = "Processed filename:";

        internal const string TableName = "Table Name :"; 

         internal const string UnmatchedIndicators = "Unmatched Indicators";

         internal const string UnmatchedUnits = "Unmatched Units";

         internal const string UnmatchedElements = "Unmatched Elements";

         internal const string UnmatchedSubgroups = "Unmatched Subgroups";

         internal const string UnmatchedAreas = "Unmatched Areas";

                 internal const string IndicatorName = "Indicator Name";
                 internal const string UnitName = "Unit Name";
                 internal const string Subgroup = "Subgroup";
                 internal const string AreaName = "Area Name";

                 internal const string IndicatorGID = "Indicator GID";
                 internal const string UnitGID = "Unit GID";
                 internal const string SubgroupGID = "Subgroup GID";
                 internal const string AreaID = "Area ID";
                 internal const string UnmappedElements = "Unmapped Elements";


           internal const string CellValue="CellValue";
           internal const string Indicator="Indicator";
           internal const string IndicatorClassification = "IndicatorClassification";
           internal const string Unit="Unit";
           internal const string IUS = "IUS";
           internal const string Area="Area";
           internal const string Time="Time";
           internal const string Source="Source";

        internal const string Data = "Data";
        
       }

    internal enum LogFileFontType
    {
        ApplicationName,
        H1,
        H2,
        H3,
        H4
    }

}


