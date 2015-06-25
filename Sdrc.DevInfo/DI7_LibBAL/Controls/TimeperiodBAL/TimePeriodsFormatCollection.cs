using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL
{
    /// <summary>
    /// This class helps in adding time period into the collection
    /// </summary>
    public class TimePeriodsFormatCollection : CollectionBase
    {
        /// <summary>
        /// Adding time period format into the collection
        /// </summary>
        /// <param name="format"></param>
        internal void Add(TimeperiodInfo format)
        {
            this.List.Add(format);
        }

    }
}