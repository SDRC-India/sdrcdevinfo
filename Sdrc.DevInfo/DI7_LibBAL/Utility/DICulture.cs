using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    public class DICulture
    {
        #region "-- Private --"

        #region "-- Variables --"

        private CultureInfo OriginalCultureInfo;

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"

        /// <summary>
        /// Sets invariant culture
        /// </summary>
        public void SetInvariantCulture()
        {
            this.OriginalCultureInfo = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        /// <summary>
        /// Restores original culture
        /// </summary>
        public void RestoreOriginalCulture()
        {
            Thread.CurrentThread.CurrentCulture = this.OriginalCultureInfo;

        }
        #endregion

        #endregion

    }
}
