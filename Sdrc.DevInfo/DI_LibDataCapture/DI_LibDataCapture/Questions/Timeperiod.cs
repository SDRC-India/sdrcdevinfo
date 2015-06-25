using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

using  DevInfo.Lib;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    //////public class Timeperiod 
    //////{
    //////    #region "-- Private --"

    //////    private Timeperiod()
    //////    {
    //////        // donot implement this.
    //////    }

    //////    #endregion

    //////    #region "-- Static --"

    //////    private static DI_LibUtility.Timeperiod.TimePeriodsFormatCollection _TimePeriodFormat;
    //////    /// <summary>
    //////    /// Gets timeperiod formats with mask value.
    //////    /// </summary>
    //////    /// <remarks>
    //////    ///  //code to bind with control
    //////    ///  cmbTimePeriod.DisplayMember = "Format";
    //////    ///  cmbTimePeriod.ValueMember = "MaskValue";
    //////    ///  cmbTimePeriod.DataSource = TimePeriodFacade.FormatCollection;
    //////    /// 
    //////    ///  //code to get the mask value 
    //////    ///   msktxtTime.Mask = cmbTimePeriod.SelectedValue.ToString();
    //////    ///   
    //////    /// </remarks>
    //////     public static DI_LibUtility.Timeperiod.TimePeriodsFormatCollection TimePeriodFormat
    //////    {
    //////        get 
    //////        {
    //////            Timeperiod._TimePeriodFormat=DI_LibUtility.Timeperiod.TimePeriodFacade.FormatCollection;
    //////            return _TimePeriodFormat;
    //////        }
    //////    }

    //////    #endregion

    //////    #region "-- Public/Internal --"

    //////    #region "-- Variables / Properties --"

    //////    private Question _TimeperiodCaption;
    //////    /// <summary>
    //////    /// Gets caption for timeperiod.
    //////    /// </summary>

    //////    private Question _Value;
    //////    /// <summary>
    //////    /// Gets the timeperiod value.
    //////    /// </summary>
    //////    public Question Value
    //////    {
    //////        get
    //////        {
    //////            return this._Value;
    //////        }
    //////    }

    //////    #endregion

    //////    #region "-- New/Dispose --"

    //////    internal Timeperiod(DataSet xmlDataSet)
    //////    {
    //////        this._Value = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.TimePeriod);
    //////    }

    //////    #endregion

    //////    #endregion
    //////}
}
