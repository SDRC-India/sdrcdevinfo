using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provide Metadata Category information like CategoryNId,CategoryName, type and order.
    /// </summary>
    public class MetadataCategoryInfo
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"
       
        private int _CategoryNId;
        /// <summary>
        /// get or set CategoryNId
        /// </summary>
        public int CategoryNId
        {
            get { return this._CategoryNId; }
            set { this._CategoryNId = value; }
        }

        private string _CategoryName = string.Empty;
        /// <summary>
        /// get or set CategoryName
        /// </summary>
        public string CategoryName
        {
            get { return this._CategoryName; }
            set { this._CategoryName = value; }
        }

        private string _CategoryType = string.Empty;
        /// <summary>
        /// get or set CategoryType  – [I-Indicator; A-Area; S-Source]
        /// </summary>
        public string CategoryType
        {
            get { return _CategoryType; }
            set { _CategoryType = value; }
        }

        private int _CategoryOrder;
        /// <summary>
        /// get or set CategoryOrder
        /// </summary>
        public int CategoryOrder
        {
            get { return this._CategoryOrder; }
            set { this._CategoryOrder = value; }
        }
	
	
        #endregion

        #endregion

    }
}
