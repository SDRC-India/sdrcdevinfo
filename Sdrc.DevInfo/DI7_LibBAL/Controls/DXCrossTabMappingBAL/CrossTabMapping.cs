using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Controls.DXCrossTabMappingBAL
{
    /// <summary>
    /// Provides CrossTab table's row/column mapping information
    /// </summary>
    public class CrossTabMapping
    {
        #region "-- Private  --"

        #region "-- Variables --"


        #endregion

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods  --"

        private string GetMappedValuesStringWGID()
        {
            string RetVal = string.Empty;
            string Delimiter = " \n " ;
            Mapping CellMappings = this._Mappings.CellMap;

            RetVal = CellMappings.IndicatorName + Delimiter +
                CellMappings.IndicatorGID + Delimiter + CellMappings.UnitName + Delimiter +
                CellMappings.UnitGID + Delimiter + CellMappings.SubgroupVal + Delimiter +
                CellMappings.SubgroupValGID + Delimiter + CellMappings.Timeperiod + Delimiter +
                CellMappings.Area + Delimiter + CellMappings.AreaID + Delimiter +
                CellMappings.Denominator + Delimiter + CellMappings.Source + Delimiter +
                CellMappings.Footnote;

            return RetVal;
        }

        private string GetMappedValuesString()
        {
            string RetVal = string.Empty;
            StringBuilder Str = new StringBuilder();
            string Delimiter = " \n ";
            string SelectedSubgroups =string.Empty;

            Mapping CellMappings = this._Mappings.CellMap;

            // timeperiod
            if (!string.IsNullOrEmpty(CellMappings.Timeperiod))
            {
                Str.Append(CellMappings.Timeperiod + Delimiter);                
            }

            // area
            if (!string.IsNullOrEmpty(CellMappings.Area))
            {
                Str.Append(CellMappings.Area + Delimiter);
            }

            //indicator
            if (!string.IsNullOrEmpty(CellMappings.IndicatorName))
            {
                Str.Append( CellMappings.IndicatorName + Delimiter);
            }

            //unit
            if (!string.IsNullOrEmpty(CellMappings.UnitName))
            {
                Str.Append(CellMappings.UnitName + Delimiter);
            }

            // subgroup
            if (!string.IsNullOrEmpty(CellMappings.SubgroupVal))
            {
                Str.Append(CellMappings.SubgroupVal + Delimiter);
            }

            if (CellMappings.Subgroups != null)
            {
                // add selected Subgroups
                foreach (DI6SubgroupInfo SelectedSGInfo in CellMappings.Subgroups.Values)
                {
                    if (!string.IsNullOrEmpty(SelectedSubgroups))
                    {
                        SelectedSubgroups += " , ";
                    }

                    SelectedSubgroups += SelectedSGInfo.Name;
                }

                if (!string.IsNullOrEmpty(SelectedSubgroups))
                {
                    Str.Append("[ " + SelectedSubgroups + " ]" + Delimiter);
                }

            }
                          
            // Denominator
            if (!string.IsNullOrEmpty(CellMappings.Denominator))
            {
                Str.Append(CellMappings.Denominator + Delimiter);
            }

            // footnote
            if (!string.IsNullOrEmpty(CellMappings.Footnote))
            {
                Str.Append(CellMappings.Footnote + Delimiter);
            }

            // source
            if (!string.IsNullOrEmpty(CellMappings.Source))
            {
                Str.Append(CellMappings.Source );
            }

            ////// Default Decimal Value
            ////Str.Append(CellMappings.DefaultDecimalValue);

            RetVal = Str.ToString();
            return RetVal;
        }


        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables/Properties --"

        private int _Index = -1;
        /// <summary>
        /// Gets or set column or rows index
        /// </summary>
        public int Index
        {
            get { return this._Index; }
            set { this._Index = value; }
        }

        private string _MappedValues = string.Empty;
        /// <summary>
        /// Gets the mapped values
        /// </summary>
        public string MappedValues
        {
            get
            {
                this._MappedValues = this.GetMappedValuesString();
                return this._MappedValues;
            }
        }

        private string _MappedValuesWGIds = string.Empty;
        /// <summary>
        /// Gets the mapped values with gids
        /// </summary>
        public string MappedValuesWGIds
        {
            get
            {
                this._MappedValuesWGIds = this.GetMappedValuesStringWGID();
                return this._MappedValuesWGIds;
            }
        }

        private DICellMapping _Mappings = new DICellMapping();
        /// <summary>
        /// Gets or sets mapping values
        /// </summary>
        public DICellMapping Mappings
        {
            get { return this._Mappings; }
            set { this._Mappings = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        public CrossTabMapping()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Column or row index </param>
        public CrossTabMapping(int index)
        {
            this._Index = index;
        }

        #endregion

        #region "-- Methods  --"


        #endregion

        #endregion

    }
}
