using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    public class UnmatchedSeriesInfo
    {
        #region"--PUBLIC--"

        #region"-- Variables/Properties --"

        private string _SeriesCode;
        /// <summary>
        /// Gets or sets SeriesCode
        /// </summary>
        public string SeriesCode
        {
            get { return this._SeriesCode; }
            set { this._SeriesCode = value; }
        }

        private string _SeriesName;
        /// <summary>
        /// Gets or sets Series Name
        /// </summary>
        public string SeriesName
        {
            get { return this._SeriesName; }
            set { this._SeriesName = value; }
        }

        private string _MappedIndicatorName;
        /// <summary>
        /// Gets or sets mapped indicatorName
        /// </summary>
        public string MappedIndicatorName
        {
            get { return this._MappedIndicatorName; }
            set { this._MappedIndicatorName = value; }
        }

        private string _MappedIndicatorGID;
        /// <summary>
        /// Gets or sets mapped indicator GID
        /// </summary>
        public string MappedIndicatorGID
        {
            get { return this._MappedIndicatorGID; }
            set { this._MappedIndicatorGID = value; }
        }


        private string _MappedUnit;

        public string MappedUnit
        {
            get { return this._MappedUnit; }
            set { this._MappedUnit = value; }
        }

        private string _MappedUnitGId;

        public string MappedUnitGId
        {
            get { return this._MappedUnitGId; }
            set { this._MappedUnitGId = value; }
        }

        private string _MappedSG;

        public string MappedSG
        {
            get { return this._MappedSG; }
            set { this._MappedSG = value; }
        }

        private string _MappedSGGId;

        public string MappedSGGId
        {
            get { return this._MappedSGGId; }
            set { this._MappedSGGId = value; }
        }




        #endregion

        #region "-- New/Dispsoe --"

        public UnmatchedSeriesInfo(string seriesCode, string seriesName)
        {
            this._SeriesCode = seriesCode;
            this._SeriesName = seriesName;
        }

        #endregion

        #endregion

    }

}
