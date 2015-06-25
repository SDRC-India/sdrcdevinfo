using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provids combined information about indicator ,unit, subgroupval.
    /// </summary>
    public class IUSInfo : IIUSInfo
    {
        #region "-- Private --"

        #region "-- New / Dispose --"

         #endregion

        #endregion

        #region "-- public  --"

        #region "-- Variables / Properties --"

        private int _Nid = 0;
        /// <summary>
        /// Gets or sets IUS nid.
        /// </summary>
        public int Nid
        {
            get { return this._Nid; }
            set { this._Nid = value; }
        }

        private int _Minimum = 0;
        /// <summary>
        /// Gets or sets IUS minimum value.Default is zero.
        /// </summary>
        public int Minimum
        {
            get { return this._Minimum; }
            set { this._Minimum = value; }
        }

        private int _Maximum = 0;
        /// <summary>
        /// Gets or sets IUS maximum value.Default is zero.
        /// </summary>
        public int Maximum
        {
            get { return this._Maximum; }
            set { this._Maximum = value; }
        }

        private IndicatorInfo _IndicatorInfo;
        /// <summary>
        /// Gets or sets values for indicator.
        /// </summary>
        public IndicatorInfo IndicatorInfo
        {
            get { return this._IndicatorInfo; }
            set { this._IndicatorInfo= value; }
        }
        
        private UnitInfo _UnitInfo;
        /// <summary>
        /// Gets or sets values for unit.
        /// </summary>
        public UnitInfo UnitInfo
        {
            get { return this._UnitInfo; }
            set { this._UnitInfo = value; }
        }

        //private SubgroupValInfo _SubgroupValInfo;
        ///// <summary>
        ///// Gets or sets values for subgroupVal.
        ///// </summary>
        //public SubgroupValInfo SubgroupValInfo
        //{
        //    get { return this._SubgroupValInfo; }
        //    set { this._SubgroupValInfo = value; }
        //}

        private DI6SubgroupValInfo _SubgroupValInfo;
        /// <summary>
        /// Gets or sets values for Subgroup Val.
        /// </summary>
        public DI6SubgroupValInfo SubgroupValInfo
        {
            get { return this._SubgroupValInfo; }
            set { this._SubgroupValInfo = value; }
        }
	

        #endregion

        #region "-- New/Dispose --"

        public IUSInfo()
        {
            this._IndicatorInfo = new IndicatorInfo();
            this._UnitInfo = new UnitInfo();
            this._SubgroupValInfo = new DI6SubgroupValInfo();
        }

        #endregion

        #endregion


       
    }
}
