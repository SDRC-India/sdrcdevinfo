using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL
{
    /// <summary>
    /// This class helps in setting time period format and the corresponding masking value
    /// </summary>
   public class TimeperiodInfo
    {

        private string _format = string.Empty;
        /// <summary>
        /// Get and set time period format
        /// </summary>
        public string Format
        {
            get { return _format; }
            set { _format = value; }
        }

        private string _maskValue = string.Empty;

        /// <summary>
        /// Get and set MaskValue
        /// </summary>
        public string MaskValue
        {
            get { return _maskValue; }
            set { _maskValue = value; }
        }

        private TimeperiodInfo()
        {
            // do nothing
        }
        /// <summary>
        /// set time period format and value
        /// </summary>
        /// <param name="format">Format</param>
        /// <param name="maskValue">Time Period value</param>
        public TimeperiodInfo(string format, string maskValue)
        {
            this.Format = format;
            this.MaskValue = maskValue;
        }
    }
 }