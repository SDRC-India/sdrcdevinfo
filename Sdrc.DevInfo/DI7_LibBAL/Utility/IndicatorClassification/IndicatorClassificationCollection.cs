using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Utility.IndicatorClassification
{
    /// <summary>
    /// Provides collection of Indicator Classifications which can be used in datasource property
    /// </summary>
    public class IndicatorClassificationCollection:CollectionBase
    {
        #region "-- Internal --"

        /// <summary>
        /// Adding indicator classification  into the collection
        /// </summary>
        /// <param name="format"></param>
        internal void Add(IndicatorClassificationInfo IC)
        {
            this.List.Add(IC);
        }

        #endregion

        #region "-- Public --"
        /// <summary>
        /// To Items IndicatorClassificationInfo
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public IndicatorClassificationInfo Items(int Index)
        {
            return (IndicatorClassificationInfo)this.List[Index];
        }

        #endregion
    }
}
