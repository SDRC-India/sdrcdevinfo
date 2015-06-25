using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
  public interface IIUSInfo
    {
      /// <summary>
      /// Get or set ius nid.
      /// </summary>
      int Nid
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set minimum value of IUS.
      /// </summary>
       int Minimum
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set maximum value of IUS.
      /// </summary>
      int Maximum
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set instance of IndicatorInfo.
      /// </summary>
      IndicatorInfo IndicatorInfo
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set instance of UnitInfo.
      /// </summary>

      UnitInfo UnitInfo
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set instance of DI6SubgroupValInfo.
      /// </summary>
      DI6SubgroupValInfo SubgroupValInfo
      {
          get;
          set;
      }
	

    }
}
