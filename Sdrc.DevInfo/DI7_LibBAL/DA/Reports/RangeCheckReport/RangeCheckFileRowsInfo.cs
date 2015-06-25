using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.RangeCheckReport
{
    /// <summary>
    /// Contains RangeCheck Report WorkSheet Fixed Cell Address
    /// </summary>
  internal static class RangeCheckFileRowsInfo
    {
          internal const int SheetStartRowIndex = 0;
          internal const int DataStartingRowIndex = 5;

          internal const int SheetHeaderRowIndex = 0;
          internal const int SheetHeaderColIndex = 0;

          internal const int SourceHeaderRowIndex = 2;
          internal const int SourceHeaderColIndex = 0;
          internal const int SourceDataColIndex = 1;

          internal const int DateTimeHeaderColIndex = 0;
          internal const int DateTimeHeaderRowIndex = 3;
          internal const int DateTimeDataColIndex = 1;

          internal const int DetailHeaderColIndex = 0;
          internal const int DetailDataColIndex = 1;

          internal const int ColWidithStartIndex = 2;
          internal const int ColWidthLastIndex = 4;
          internal const int HeaderRowCount = 5;            
    }

}
