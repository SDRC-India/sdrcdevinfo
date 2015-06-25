using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    public class UltraWinGridAutoFilter
    {
    
        #region " -- Constructor -- "
        /// <summary>Internal Constructor</summary>
        internal UltraWinGridAutoFilter(string filterColumn, string filterString)
        {
            this._FilterColumn = filterColumn;
            this._FilterString = filterString;
        }

        /// <summary>
        /// Constructor only for the serialization purpose
        /// </summary>
        public UltraWinGridAutoFilter()
        { 
        }

        #endregion

        #region " -- Public -- "

        #region " -- Properties -- "
        


        private string _FilterColumn = string.Empty;
        /// <summary>
        /// Indicator / IUS NId. 
        /// </summary>
        /// <remarks>
        /// <para>Generally set during the selection process on the Indicator page.</para>
        /// <para>Will store IUSNIds if the property _ShowIUS = true</para>
        /// <para>Will store IndicatorNIds if the property ShowIUS = false</para>
        /// </remarks>
        public string FilterColumn
        {
            get { return _FilterColumn; }
            set { _FilterColumn = value; }
        }

        private string _FilterString = string.Empty;
        /// <summary>
        /// Indicator / IUS GId. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set through filter dialog on the dataview page.</para>
        /// <para>Will store IndicatorGId_UnitGId_SubgroupGId if the property ShowIUS = true</para>
        /// <para>Will store IndicatorGId if the property ShowIUS = false</para>
        /// </remarks>
        public string FilterString
        {
            get { return _FilterString; }
            set { _FilterString = value; }
        }

       
        #endregion

    
        #endregion
    }
}
