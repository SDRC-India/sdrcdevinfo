using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// Class used to store Mapping of Unmatched element DataRow and available element DataRow.
    /// </summary>
    public class MappedElementInfo
    {
        #region "-- Public --"

        # region " -- Variables/Properties-- "

        private DataRow _UnMatchedRow;
        public DataRow UnMatchedRow
        {
            get
            {
                return this._UnMatchedRow;
            }
            set
            {
                this._UnMatchedRow = value;
            }
        }

        private DataRow _MatchedRow;
        /// <summary>
        /// 
        /// </summary>
        public DataRow MatchedRow
        {
            get
            {
                return this._MatchedRow;
            }
            set
            {
                this._MatchedRow = value;
            }
        }

        # endregion

        # region "-- New/Dispose --"

        public MappedElementInfo(DataRow unMatchedRow, DataRow matchedRow)
        {
            this._UnMatchedRow = unMatchedRow;
            this._MatchedRow = matchedRow;
        }

        # endregion

        #endregion
    }
}
