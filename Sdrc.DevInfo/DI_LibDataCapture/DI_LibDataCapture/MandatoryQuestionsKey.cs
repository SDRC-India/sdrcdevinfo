using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture
{
    /// <summary>
    /// Helps in getting mandatory questions key
    /// </summary>
    [Serializable()]
    public class MandatoryQuestionsKey
    {
        #region "-- Private --"

        #region "--New/Dispose --"

        private MandatoryQuestionsKey()
        {
            //do not implement this
        }

        #endregion

        #endregion

        #region "--Public --"

        /// <summary>
        /// Returns the INTERVIEWER Key
        /// </summary>
        public const string INTERVIEWER = "NAME_INTERVIEWER";

        /// <summary>
        /// Return the Area key 
        /// </summary>
        public const string Area = "AREA";

        /// <summary>
        /// Return the Time Period key
        /// </summary>
        public const string TimePeriod = "TIMEPERIOD";

        /// <summary>
        /// Return the Source Publisher key
        /// </summary>
        public const string SorucePublisher = "SRC_PUBLISHER";

        /// <summary>
        /// Returns the Source Title
        /// </summary>
        public const string SourceTitle = "SRC_TITLE";

        /// <summary>
        /// Return the source Year key
        /// </summary>
        public const string SourceYear = "SRC_YEAR";

        /// <summary>
        /// Returns the GPS
        /// </summary>
        public const string GPS = "GPS_LOCATION";

        /// <summary>
        /// Returns the FONT_FAMILY
        /// </summary>
        public const string FONT_FAMILY = "FONT_FAMILY";

        /// <summary>
        /// Returns the FONT_SIZE
        /// </summary>
        public const string FONT_SIZE= "FONT_SIZE";

        /// <summary>
        /// Returns the latitude
        /// </summary>
        public const string Latitude = "LATITUDE";

        /// <summary>
        /// Returns the longitude 
        /// </summary>
        public const string Longitude = "LONGITUDE";

        /// <summary>
        /// Returns the TotalQuestions
        /// </summary>
        public const string TotalQuestions = "TotalQuestions";

        /// <summary>
        /// Returns the Start time of the survey
        /// </summary>
        public const string StartTime = "StartTime";

        /// <summary>
        /// Returns the End time of the survey
        /// </summary>
        public const string EndTime = "EndTime";

        #endregion

    }
}
