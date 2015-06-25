using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DES
{
    public class DataEntrySpreadsheet
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"

        private string _SheetName=string.Empty ;
        /// <summary>
        /// Gets or sets sheet name
        /// </summary>
        public string SheetName
        {
            get 
            {
                return this._SheetName;
            }
            set 
            {
                this._SheetName = value;
            }
        }	    

        private List<DESValue> _DESValues = new List<DESValue>();
        /// <summary>
        /// Gets or sets DES values
        /// </summary>
        public List<DESValue> DESValues
        {
            get { return this._DESValues; }
            set { this._DESValues = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        public  DataEntrySpreadsheet()
        {
          
        }

        #endregion

        #region "-- Methods  --"

        public DataTable GetDESDataTable()
        {
            DataTable RetVal = new DataTable();
            DataRow Row;
                       
            // add columns
            // indicator, indicator gid, unit, unit gid, Time,	Area Id,	Area Name,	Datavalue,	Subgroup,	Source,	Footnote,	Denominator	, subgroup gid
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Indicator.IndicatorGId);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(Unit.UnitGId);
            RetVal.Columns.Add(Timeperiods.TimePeriod);
            RetVal.Columns.Add(Area.AreaID);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Data.DataValue);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(SubgroupVals.SubgroupValGId);
            RetVal.Columns.Add(IndicatorClassifications.ICName);
            RetVal.Columns.Add(FootNotes.FootNote);
            RetVal.Columns.Add(Data.DataDenominator);
            RetVal.AcceptChanges();

            // add values

            foreach (DESValue RowValue in this._DESValues)
            {
                Row = RetVal.NewRow();

                Row[Indicator.IndicatorName] = RowValue.IndicatorName;
                Row[Indicator.IndicatorGId] = RowValue.IndicatorGID;
                Row[Unit.UnitName] = RowValue.UnitName;
                Row[Unit.UnitGId] = RowValue.UnitGID; 

                Row[Timeperiods.TimePeriod] = RowValue.Timeperiod;
                Row[Area.AreaID] = RowValue.AreaID;
                Row[Area.AreaName] = RowValue.AreaName;
                Row[Data.DataValue] = RowValue.DataValue;

                Row[SubgroupVals.SubgroupVal] = RowValue.SubgroupVal;
                Row[SubgroupVals.SubgroupValGId] = RowValue.SubgroupValGID;

                Row[IndicatorClassifications.ICName] = RowValue.Source;
                Row[FootNotes.FootNote] = RowValue.Footnote;
                Row[Data.DataDenominator] = RowValue.Denominator;

                RetVal.Rows.Add(Row);
            }

            return RetVal;
        }

        #endregion

        #endregion

    }


    public class DESValue
    {
        #region "-- Public --"

        #region "-- Variables/Properties --"

        private string _IndicatorGID = string.Empty;
        /// <summary>
        /// Gets or sets Indicator GID
        /// </summary>
        public string IndicatorGID
        {
            get { return this._IndicatorGID; }
            set { this._IndicatorGID = value; }
        }

        private string _IndicatorName = string.Empty;
        /// <summary>
        /// Gets or sets indicator name
        /// </summary>
        public string IndicatorName
        {
            get { return this._IndicatorName; }
            set { this._IndicatorName = value; }
        }

        private string _UnitGID = string.Empty;
        /// <summary>
        /// Gets or sets unit GID
        /// </summary>
        public string UnitGID
        {
            get { return this._UnitGID; }
            set { this._UnitGID = value; }
        }

        private string _UnitName = string.Empty;
        /// <summary>
        /// Gets or sets unit name
        /// </summary>
        public string UnitName
        {
            get { return this._UnitName; }
            set { this._UnitName = value; }
        }

        private string _AreaID = string.Empty;
        /// <summary>
        /// Gets or sets AreaID
        /// </summary>
        public string AreaID
        {
            get { return this._AreaID; }
            set { this._AreaID = value; }
        }

        private string _AreaName = string.Empty;
        /// <summary>
        /// Gets or sets AreaName
        /// </summary>
        public string AreaName
        {
            get { return this._AreaName; }
            set { this._AreaName = value; }
        }

        private string _Source = string.Empty;
        /// <summary>
        /// Gets or sets source
        /// </summary>
        public string Source
        {
            get { return this._Source; }
            set { this._Source = value; }
        }

        private string _Timeperiod = string.Empty;
        /// <summary>
        /// Gets or sets Timeperiod
        /// </summary>
        public string Timeperiod
        {
            get { return this._Timeperiod; }
            set { this._Timeperiod = value; }
        }

        private string _DataValue = string.Empty;
        /// <summary>
        /// Gets or sets data value
        /// </summary>
        public string DataValue
        {
            get { return this._DataValue; }
            set { this._DataValue = value; }
        }

        private string _SubgroupValGID = string.Empty;
        /// <summary>
        /// Gets or sets subgroup val GID
        /// </summary>
        public string SubgroupValGID
        {
            get { return this._SubgroupValGID; }
            set { this._SubgroupValGID = value; }
        }

        private string _SubgroupVal = string.Empty;
        /// <summary>
        /// Gets or sets subgroup val
        /// </summary>
        public string SubgroupVal
        {
            get { return this._SubgroupVal; }
            set { this._SubgroupVal = value; }
        }

        private string _Footnote= string.Empty;
        /// <summary>
        /// Gets or sets footnote
        /// </summary>
        public string Footnote
        {
            get { return this._Footnote; }
            set { this._Footnote = value; }
        }
        private string _Denominator = string.Empty;
        /// <summary>
        /// Gets or sets denominator
        /// </summary>
        public string Denominator
        {
            get { return this._Denominator; }
            set { this._Denominator = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        public DESValue()
        {
            // do nothing
        }

        #endregion

        #region "-- Methods  --"


        #endregion

        #endregion
    }


}