using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides information about subgroupval.  
    /// </summary>
    public class DI6SubgroupValInfo : CommonInfo
    {
        #region "-- Public --"

        #region "-- Variables/ Properties --"

        private List<DI6SubgroupInfo> _Dimensions;
        /// <summary>
        ///  Gets or sets associated subgroup dimension
        /// </summary>
        public List<DI6SubgroupInfo> Dimensions
        {
            get
            {
                return this._Dimensions; 
            }
            set
            {
                this._Dimensions = value; 
            }
        }

        private int _SubgroupValOrder;
        /// <summary>
        /// Gets or Sets SubgroupVals Sort Order
        /// </summary>
        public int SubgroupValOrder
        {

            get { return this._SubgroupValOrder; }
            set { this._SubgroupValOrder = value; }
        }
	
         #endregion

        #region "-- Methods --"

        #region "-- New/Dispose --"

        public DI6SubgroupValInfo()
        {            
            // do nothing
        }

        #endregion
        #endregion

        #endregion
    }
}
