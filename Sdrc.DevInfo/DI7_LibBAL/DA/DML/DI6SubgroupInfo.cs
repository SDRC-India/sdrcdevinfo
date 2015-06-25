using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides information about subgroup .
    /// </summary>
    public class DI6SubgroupInfo : CommonInfo
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"

        private int _Type= 0;
        /// <summary>
        /// Gets or sets subgroup type
        /// </summary>
        public int Type
        {
            get { return this._Type; }
            set { this._Type = value; }
        }

        private bool _IsGlobal=false;
        /// <summary>
        /// Gets or sets ture or false. True if subgroup is global otherwise false.Default is false.
        /// </summary>
        public bool Global
        {
            get { return this._IsGlobal; }
            set { this._IsGlobal = value; }
        }

        
        private DI6SubgroupTypeInfo _DISubgroupType;
        /// <summary>
        /// Gets or sets associated subgroup type information
        /// </summary>
        public DI6SubgroupTypeInfo DISubgroupType
        {
            get 
            {
                return this._DISubgroupType; 
            }
            set 
            {
                this._DISubgroupType = value; 
            }
        }

        private int _SubgroupOrder;
        /// <summary>
        /// Gets or Sets Subgroup_order
        /// </summary>
        public int SubgroupOrder
        {
            get { return this._SubgroupOrder; }
            set { this._SubgroupOrder = value; }
        }
	           
        #endregion

        #region "-- Methods --"

        #endregion

        #endregion
    }
}
