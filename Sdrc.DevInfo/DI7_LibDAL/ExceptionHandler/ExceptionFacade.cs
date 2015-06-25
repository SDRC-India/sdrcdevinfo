// ***********************Copy Right Notice*****************************
// 
// **********************************************************************
// Program Name:									       
// Developed By: DG6
// Creation date: 2007-06-21							
// Program Comments: Store all mapping information and provide mapping
// **********************************************************************
// **********************Change history*********************************
// No.	Mod: Date	      Mod: By	       Change Description		        
//
//											          
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DevInfo.Lib.DI_LibDAL.ExceptionHandler
    {
            internal class ExceptionFacade
            {

                #region "-- Private --"


                #region "-- Methods --"
                private static void LogException(Exception ex)
                {
                    
                    //TODO: Implement Exception Logging
                    Trace.WriteLine(ex.Message);
                    
                }
                #endregion

                #endregion

                #region "-- Public / Internal --"


                #region "-- Methods --"

                /// <summary>
                /// To throw exception.
                /// </summary>
                /// <param name="ex">Exception</param>
                internal static void ThrowException(Exception ex)
                {
                throw new ApplicationException(ex.Message, ex.InnerException);
                    //LogException(ex);
                }



                #endregion

                #endregion

            }

        }
