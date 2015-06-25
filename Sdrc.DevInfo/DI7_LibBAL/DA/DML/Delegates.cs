using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// A delegate for BeforeProcess and ProcessInfo events 
    /// </summary>
    /// <param name="recordsFound">For beforePorcess event pass the total numbers of records which are need to be processed. For ProcessInfo event pass the current record number</param>
    public delegate void ProcessInfoDelegate(int number);

    /// <summary>
    /// A delegate for StartProcess and EndProcess event
    /// </summary>
    public delegate void ProcessDelegate();

}
