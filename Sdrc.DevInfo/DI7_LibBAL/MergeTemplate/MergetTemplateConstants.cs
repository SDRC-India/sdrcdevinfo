using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
   public class MergetTemplateConstants
    {
       public const string GIDSplitter ="{[~]}";
       public const string DBPassword = "unitednations2000";
       public class TempTable
       {
           public const string Indicator = "Temp_Indicator";
           public const string Unit = "Temp_Unit";
           public const string Subgroup = "Temp_Subgroup";
           public const string SubgroupType = "Temp_SubgroupType";
           public const string SubgroupVals = "Temp_SubgroupVals";
           public const string Area = "Temp_Area";
           public const string IndicatorClassification = "Temp_IndicatorClassification";
          
       }

       public static class Columns
       {
           public const string Level = "Level";
           public const string COLUMN_UNMATCHED = "UnMatched";
           public const string COLUMN_SELECT = "SelectRow";
           public const string COLUMN_SOURCEFILENAME = "SourceFileName";
           public const string COLUMN_SRCNID = "SrcNId";
           public const string COLUMNS_NEWICNID = "NewICNId";
           public const string UNMATCHED_COL_Prefix = "Unmatched_";
           public const string AVAILABLE_COL_Prefix = "Available_";
           
       }
    }
}
