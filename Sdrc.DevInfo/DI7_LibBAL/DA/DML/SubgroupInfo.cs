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
    public class SubgroupInfo : CommonInfo, ISubgroupInfo
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"

        private SubgroupType _Type= SubgroupType.Age;
        /// <summary>
        /// Gets or sets subgroup type.Default is Age.
        /// </summary>
        public SubgroupType Type
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

        #endregion

        #region "-- Methods --"

        #endregion

        #endregion
    }
}
