using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides Indicator information like name, GId, NId,etc
    /// </summary>
    public class IndicatorInfo:CommonInfo
    {
        #region "-- Variables/Properties --"

        private string _Info= string.Empty;
        /// <summary>
        /// Gets or sets IndicatorInfo
        /// </summary>
        public string Info
        {
            get { return this._Info; }
            set { this._Info= value; }
        }

        private bool _HighIsGood;
        /// <summary>
        /// Get or Set HighIsGood
        /// </summary>
        public bool HighIsGood
        {
            get { return _HighIsGood; }
            set { _HighIsGood = value; }
        }

	
        #endregion

        #region "-- Methods --"

        #endregion
    }
}
