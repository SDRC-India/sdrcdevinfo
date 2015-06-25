using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
  public interface ITimeperiodInfo
    {
      /// <summary>
      /// Get or set timeperiod nid.
      /// </summary>
      int Nid
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set timeperiod value. 
      /// </summary>
      string TimeperiodValue
      {
          get;
          set;
      }

    }
}
