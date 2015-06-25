using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.Controls.DIWizardBAL
{
    public class CacheInfo
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "


        #endregion

        #region " -- Properties -- "

        private int _DataCount = 0;
        /// <summary>
        /// Gets or sets the Data count
        /// </summary>
        public int DataCount
        {
            get 
            { 
                return this._DataCount; 
            }
            set 
            {
                this._DataCount = value; 
            }
        }

        private int _IndicatorCount = 0;
        /// <summary>
        /// Gets or sets the Indicator count
        /// </summary>
        public int IndicatorCount
        {
            get 
            { 
                return this._IndicatorCount; 
            }
            set 
            {
                this._IndicatorCount = value; 
            }
        }

        private int _AreaCount = 0;
        /// <summary>
        /// Gets or sets the Area count
        /// </summary>
        public int AreaCount
        {
            get 
            { 
                return this._AreaCount; 
            }
            set 
            { 
                this._AreaCount = value; 
            }
        }

        private int _TimeperiodCount = 0;
        /// <summary>
        /// Gets or sets the timeperiod count
        /// </summary>
        public int TimeperiodCount
        {
            get 
            { 
                return this._TimeperiodCount; 
            }
            set 
            { 
                this._TimeperiodCount = value; 
            }
        }

        private int _SourceCount = 0;
        /// <summary>
        /// Gets or sets the source count
        /// </summary>
        public int SourceCount
        {
            get 
            { 
                return this._SourceCount; 
            }
            set 
            { 
                this._SourceCount = value; 
            }
        }
	
        private string _IndicatorNIds = string.Empty;
        /// <summary>
        /// Gets or sets the IndicatorNIds
        /// </summary>
        public string IndicatorNIds
        {
            get 
            { 
                return this._IndicatorNIds; 
            }
            set 
            {
                this._IndicatorNIds = value; 
            }
        }

        private string _AreaNIds = string.Empty;
        /// <summary>
        /// Gets or sets the AreaNIds
        /// </summary>
        public string AreaNIds
        {
            get 
            { 
                return this._AreaNIds; 
            }
            set 
            {
                this._AreaNIds = value; 
            }
        }

        private string _TimeperiodNIds = string.Empty;
        /// <summary>
        /// Gets or sets the TimeperiodNIds
        /// </summary>
        public string TimeperiodNIds
        {
            get 
            { 
                return this._TimeperiodNIds; 
            }
            set 
            {
                this._TimeperiodNIds = value; 
            }
        }

        private string _SourceNIds = string.Empty;
        /// <summary>
        /// Gets or sets the SourceNIds
        /// </summary>
        public string SourceNIds
        {
            get 
            { 
                return this._SourceNIds; 
            }
            set 
            { 
                this._SourceNIds = value; 
            }
        }

        private string _Description;
        /// <summary>
        /// Gets or sets the database description.
        /// </summary>
        public string Description
        {
            get 
            {
                return this._Description; 
            }
            set 
            {
                this._Description = value; 
            }
        }
	


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Save the Cache in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void Save(string fileNameWPath)
        {
            XmlSerializer CacheSerialize = new XmlSerializer(typeof(CacheInfo));
            StreamWriter CacheWriter = new StreamWriter(fileNameWPath);
            CacheSerialize.Serialize(CacheWriter, this);
            CacheWriter.Close();
        }

        /// <summary>
        /// Load the deserialize XML file
        /// </summary>
        /// <param name="fileNameWPath">File of cache file</param>
        /// <returns></returns>
        /// <remarks> Convention used for auto selected cache file : First letter as prefix of Indicator, Area, Timeperiod, & source and their selected NIds in the ascending order.
        /// For Available Nids cache file : DIWizard.xml 
        /// </remarks>
        public static CacheInfo Load(string fileNameWPath)
        {
            CacheInfo RetVal;
            try
            {
                XmlSerializer CacheSerialize = new XmlSerializer(typeof(CacheInfo));
                TextReader CacheReader = new StreamReader(fileNameWPath);
                RetVal = (CacheInfo)CacheSerialize.Deserialize(CacheReader);                
                CacheReader.Close();
                if (!RetVal.IsValidCache())
                {
                    // -- If the cache contain invalid or incomplete information.
                    RetVal = null;
                }
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }


        #endregion

        #endregion

        #region " -- Private -- "

        private bool IsValidCache()
        {
            bool Retval = true;
            try
            {
                if (this._DataCount == 0 || this._IndicatorCount == 0 || this._AreaCount == 0 || this._TimeperiodCount == 0 || this._SourceCount == 0)
                {
                    Retval = false;
                }
                if (string.IsNullOrEmpty(this._IndicatorNIds) || string.IsNullOrEmpty(this._AreaNIds) || string.IsNullOrEmpty(this._TimeperiodNIds) || string.IsNullOrEmpty(this._SourceNIds))
                {
                    Retval = false;
                }                
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }

        #endregion
    }
}
