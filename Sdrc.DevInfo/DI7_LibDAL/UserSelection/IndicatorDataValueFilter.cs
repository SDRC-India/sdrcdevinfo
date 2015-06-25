using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    public class IndicatorDataValueFilter: DataValueFilter
    {
        #region " -- Private -- "
        private Boolean _ShowIUS = true; //Set through constructor

        #endregion

        #region " -- Constructor -- "
        /// <summary>Internal Constructor</summary>
        internal IndicatorDataValueFilter(bool showIUS, int indicatorNId, string indicatorGId, OpertorType opertorType, double fromDataValue, double toDataValue)
        {
            this._ShowIUS = showIUS;
            this._IndicatorNId = indicatorNId;
            this._IndicatorGId = indicatorGId;
            base.OpertorType = opertorType;
            base.FromDataValue = fromDataValue;
            base.ToDataValue = toDataValue;
        }

        /// <summary>
        /// Constructor only for the serialization purpose
        /// </summary>
        public IndicatorDataValueFilter()
        { 
        }

        #endregion

        #region " -- Public -- "

        #region " -- Properties -- "
        


        private int _IndicatorNId;
        /// <summary>
        /// Indicator / IUS NId. 
        /// </summary>
        /// <remarks>
        /// <para>Generally set during the selection process on the Indicator page.</para>
        /// <para>Will store IUSNIds if the property _ShowIUS = true</para>
        /// <para>Will store IndicatorNIds if the property ShowIUS = false</para>
        /// </remarks>
        public int IndicatorNId
        {
            get { return _IndicatorNId; }
            set { _IndicatorNId = value; }
        }

        private string _IndicatorGId = string.Empty;
        /// <summary>
        /// Indicator / IUS GId. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set through filter dialog on the dataview page.</para>
        /// <para>Will store IndicatorGId_UnitGId_SubgroupGId if the property ShowIUS = true</para>
        /// <para>Will store IndicatorGId if the property ShowIUS = false</para>
        /// </remarks>
        public string IndicatorGId
        {
            get { return _IndicatorGId; }
            set { _IndicatorGId = value; }
        }

       
        #endregion

        #region " -- Methods -- "
        internal override string SQL_GetDataValueFilter(DIServerType DIServerType)
        {
            string RetVal = String.Empty;
            string sDataValueFilter = base.SQL_GetDataValueFilter(DIServerType);
            if (sDataValueFilter.Length > 0)   // Filter string might be empty in case OpertorType.None
            {
                if (this._ShowIUS==true)
                {
                    // Sample (D.IUSNId = 93 AND (D.Data_value BETWEEN 0 AND 100))
                    RetVal = "(D." + Data.IUSNId;
                }
                else
                {
                    // Sample (IUS.Indicator_NId = 74 AND (D.Data_value BETWEEN 0 AND 100))
                    RetVal = "(D." + Data.IndicatorNId;
                }

                RetVal += "=" + this._IndicatorNId + sDataValueFilter + ")";
            }

            return RetVal;
        }
        #endregion
        
        #endregion
    }
}
