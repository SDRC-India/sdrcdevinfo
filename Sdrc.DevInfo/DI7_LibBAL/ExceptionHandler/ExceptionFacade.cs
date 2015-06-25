using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DevInfo.Lib.DI_LibBAL.ExceptionHandler
{
    public class ExceptionFacade
    {
        public static void LogException(Exception ex)
        {
            //TODO: Implement Exception Logging
            Trace.WriteLine(ex.Message);
            //Instrumentation.InstrumentationFacade.LogMethodCalls();
        }

        public static void ThrowException(Exception ex)
        {
            //throw execption
           throw new ApplicationException(ex.ToString());
          
            //TODO : log exception 

        }
    }
}
