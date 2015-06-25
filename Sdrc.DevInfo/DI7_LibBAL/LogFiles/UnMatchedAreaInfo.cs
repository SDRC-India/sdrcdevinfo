using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    public class UnMatchedAreaInfo
    {
        private string _MappedAreaID;
        /// <summary>
        /// Gets or sets MappedareaId
        /// </summary>
        public string MappedAreaID
        {
            get { return this._MappedAreaID; }
            set { this._MappedAreaID = value; }
        }
        private string _MappedArea;
        /// <summary>
        /// GEts or sets Mapped areaName
        /// </summary>
        public string MappedArea
        {
            get { return this._MappedArea; }
            set { this._MappedArea = value; }
        }

        private string _CountryCode;
/// <summary>
/// Gets or sets Country Code
/// </summary>
        public string CountryCode
        {
            get { return this._CountryCode; }
            set { this._CountryCode = value; }
        }
        private string _CountryName;
/// <summary>
/// Gets or sets Countryname
/// </summary>
        public string CountryName
        {
            get { return this._CountryName; }
            set { this._CountryName = value; }
        }


        public UnMatchedAreaInfo(string countryCode, string countryName)
        {
            this._CountryCode = countryCode;
            this._CountryName = countryName;
        }

	
    }
}
