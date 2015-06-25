using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// provide information about timeperiod.
    /// </summary>
    public class TimeperiodInfo : ITimeperiodInfo
    {
        #region  "-- Public --"

        #region "-- Variables / Properties --"

        private int _Nid = 0;
        /// <summary>
        /// Gets or sets timeperiod nid
        /// </summary>
        public int Nid
        {
            get { return this._Nid; }
            set { this._Nid = value; }
        }

        private string _TimeperiodValue = string.Empty;
        /// <summary>
        /// Gets or sets timeperiod value.
        /// </summary>
        public string TimeperiodValue
        {
            get { return this._TimeperiodValue; }
            set { this._TimeperiodValue = value; }
        }

        #endregion

        #endregion

    }
}
