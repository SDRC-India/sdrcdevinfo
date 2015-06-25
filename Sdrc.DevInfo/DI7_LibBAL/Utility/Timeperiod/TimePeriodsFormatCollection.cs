using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.Utility.Timeperiod
{
    /// <summary>
    /// 
    /// </summary>
    public class TimePeriodsFormatCollection : CollectionBase
    {

        #region "-- Internal --"

        /// <summary>
        /// Adding time period format into the collection
        /// </summary>
        /// <param name="format"></param>
        internal void Add(TimeperiodInfo format)
        {
            this.List.Add(format);
        }

        #endregion

        #region "-- Public --"
        /// <summary>
        /// To Items timeperiodinfo 
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public TimeperiodInfo Items(int Index)
        {

            return (TimeperiodInfo)this.List[Index];
        }

        #endregion

    }
}
