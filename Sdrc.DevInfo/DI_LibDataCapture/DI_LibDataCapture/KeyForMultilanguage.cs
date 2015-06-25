using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture
{
    [Serializable()]
    public class KeyForMultiLanguage
    {
        /// <summary>
        /// Returns the Interviewer key
        /// </summary>
        public const string Interviewer = "Name of Interviewer";

        /// <summary>
        /// Returns the AreaId key
        /// </summary>
        public const string AreaId = "Area Id";

        /// <summary>
        /// Returns the Area Name key
        /// </summary>
        public const string AreaName = "Area Name";

        /// <summary>
        /// Returns the Latitude key
        /// </summary>
        public const string Latitude = "Latitude";

        /// <summary>
        /// Returns the Longitude key
        /// </summary>
        public const string Longitude = "Longitude";

        /// <summary>
        /// Returns the FixQuality key
        /// </summary>
        public const string FixQuality = "Fix Quality";

        /// <summary>
        /// Returns the SatelliteTime key
        /// </summary>
        public const string SatelliteTime = "Satellite Time";

        /// <summary>
        /// Returns the LocalTime key
        /// </summary>
        public const string LocalTime = "Local Time";

        /// <summary>
        /// Returns the InputLocation key
        /// </summary>
        public const string InputLocation = "Input Location";

        /// <summary>
        /// Returns the GPS key
        /// </summary>
        public const string GPS = "GPS";

        /// <summary>
        /// Returns the TimePeriod key
        /// </summary>
        public const string TimePeriod = "Time period";

        /// <summary>
        /// Returns the SourcePublisher key
        /// </summary>
        public const string SourcePublisher = "Source publisher";

        /// <summary>
        /// Returns the SourceTitle key
        /// </summary>
        public const string SourceTitle = "Source title";

        /// <summary>
        /// Returns the SourceYear key
        /// </summary>
        public const string SourceYear = "Source year";

        /// <summary>
        /// Returns the Source key
        /// </summary>
        public const string Source = "Source";

        /// <summary>
        /// Returns the ValueRequired key
        /// </summary>
        public const string ValueRequired = "Data value required for";

        /// <summary>
        /// Returns the Save key
        /// </summary>
        public const string Save = "Do you still want to save it";

        /// <summary>
        /// Returns the end of survey string
        /// </summary>
        public const string EndOfSurvey = "End of Survey";
    }
}
