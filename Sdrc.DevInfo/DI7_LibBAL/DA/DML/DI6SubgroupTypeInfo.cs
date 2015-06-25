using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides information about subgroup type.
    /// </summary>
    public class DI6SubgroupTypeInfo : CommonInfo
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"

        private int _Order= 0;
        /// <summary>
        /// Gets or sets subgroup type order
        /// </summary>
        public int Order
        {
            get { return this._Order; }
            set { this._Order = value; }
        }
           
        #endregion

        #region "-- Methods --"

        #endregion

        #endregion
    }
}
