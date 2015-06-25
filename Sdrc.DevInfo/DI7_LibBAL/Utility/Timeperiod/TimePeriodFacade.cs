using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Utility.Timeperiod
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <code>
    /// 
    ///  //code to bind with control
    ///  cmbTimePeriod.DisplayMember = "Format";
    ///  cmbTimePeriod.ValueMember = "MaskValue";
    ///  cmbTimePeriod.DataSource = TimePeriodFacade.FormatCollection;
    /// 
    ///  //code to get the mask value 
    ///   msktxtTime.Mask = cmbTimePeriod.SelectedValue.ToString();
    ///   
    /// </code>
    /// </example>
    public class TimePeriodFacade
    {
        /// <summary>
        /// Create instance of TimePeriodsFormatCollection class
        /// </summary>
        public static TimePeriodsFormatCollection FormatCollection = new TimePeriodsFormatCollection();

        /// <summary>
        ///Add TimePeriod format and corressponding values 
        /// </summary>
        static TimePeriodFacade()
        {
            // insert timeperiod format 
            FormatCollection.Add(new TimeperiodInfo("yyyy", "0000"));
            FormatCollection.Add(new TimeperiodInfo("yyyy.mm", "0000.00"));
            FormatCollection.Add(new TimeperiodInfo("yyyy.mm.dd", "0000.00.00"));
            //C1 Start comment
            FormatCollection.Add(new TimeperiodInfo("yyyy-yyyy", "0000-0000"));
            FormatCollection.Add(new TimeperiodInfo("yyyy.mm-yyyy.mm", "0000.00-0000.00"));
            FormatCollection.Add(new TimeperiodInfo("yyyy.mm.dd-yyyy.mm.dd", "0000.00.00-0000.00.00"));
            //C1 End Comment
        }


    }
}
