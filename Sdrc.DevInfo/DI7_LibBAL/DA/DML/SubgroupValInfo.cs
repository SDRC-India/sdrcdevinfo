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
    public class SubgroupValInfo : CommonInfo, ISubgroupValInfo
    {
        #region "-- Public --"

        #region "-- Variables/ Properties --"

        private SubgroupInfo _Age;
        /// <summary>
        /// Gets or sets values for age subgroup
        /// </summary>
        public SubgroupInfo Age
        {
            get { return this._Age; }
            set 
            {
                this._Age = value;
                this._Age.Type = SubgroupType.Age;
            }
        }

        private SubgroupInfo _Sex;
        /// <summary>
        /// Gets or sets values for sex subgroup
        /// </summary>
        public SubgroupInfo Sex
        {
            get { return this._Sex; }
            set 
            {
                this._Sex = value;
                this._Sex.Type = SubgroupType.Sex;
            }
        }

        private SubgroupInfo _Location;
        /// <summary>
        /// Gets or sets values for location subgroup
        /// </summary>
        public SubgroupInfo Location
        {
            get { return this._Location; }
            set 
            {
                this._Location = value;
                this._Location.Type = SubgroupType.Location;
            }
        }

        private SubgroupInfo _Others;
        /// <summary>
        /// Gets or sets values for others subgroup
        /// </summary>
        public SubgroupInfo Others
        {
            get { return _Others; }
            set
            {
                this._Others = value;
                this._Others.Type = SubgroupType.Others;
            }
        }

      #endregion

        #region "-- Methods --"
        #region "-- New/Dispose --"
        public SubgroupValInfo()
        {
            this._Age = new SubgroupInfo();
            this._Location = new SubgroupInfo();
            this._Others = new SubgroupInfo();
            this._Sex = new SubgroupInfo();
        }
        #endregion
        #endregion

        #endregion
    }
}
