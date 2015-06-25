
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL
{
    /// <summary>
    /// This class add different time format and the corresponding values into the collection
    /// </summary>
    public class TimePeriodFacade
    {
        #region "-- Public Static --"

        #region "-- Variables --"

        /// <summary>
        /// Create instance of TimePeriodsFormatCollection class
        /// </summary>
        public static TimePeriodsFormatCollection FormatCollection = new TimePeriodsFormatCollection();

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns mask value only if the timperiod is in DevInfo format.
        /// </summary>
        /// <param name="timeperiod"></param>
        /// <returns></returns>
        public static string GetMaskValue(string timeperiod)
        {
            string RetVal = string.Empty;

            if (!string.IsNullOrEmpty(timeperiod))
            {
                switch (timeperiod.Length)
                {
                    case 4: // "yyyy"

                        RetVal = "0000";
                        break;

                    case 7: // "yyyy.mm" 
                        if (timeperiod.StartsWith("Q"))
                        {
                            RetVal = "Q0.0000";
                        }
                        else
                        {
                            RetVal = "0000.00";
                        }
                        break;

                    case 10: // "yyyy.mm.dd"
                    case 8:
                        RetVal = "0000.00.00";
                        break;

                    case 5:
                        if (timeperiod.Contains("-"))
                        {
                            RetVal = "0000-0000";
                        }
                        else
                        {
                            RetVal = "0000.00";
                        }

                        break;

                    case 9: // "yyyy-yyyy"

                        RetVal = "0000-0000";
                        break;

                    case 15: // "yyyy.mm-yyyy.mm"
                    case 13:
                        RetVal = "0000.00-0000.00";
                        break;

                    case 21: //"yyyy.mm.dd-yyyy.mm.dd"
                    case 19:
                        RetVal = "0000.00.00-0000.00.00";
                        break;

                    default:
                        break;
                }

                if (string.IsNullOrEmpty(RetVal))
                {
                    if (TimePeriodFacade.ValidateDate(RetVal, timeperiod) == false)
                    {
                        RetVal = string.Empty;
                    }
                }

            }



            return RetVal;
        }

        /// <summary>
        /// Set start date and end date based on timeperiod string with DI compatible formats
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public static void SetStartDateEndDate(string timePeriod, ref System.DateTime startDate, ref System.DateTime endDate)
        {
            string[] sDate;
            //-- Handle Problem with different Reginal Settings - Thai, Arabic etc
            System.Globalization.CultureInfo ociEnUS = new System.Globalization.CultureInfo("en-US", false);
            try
            {
                switch (timePeriod.Length)
                {
                    case 4:
                        //yyyy
                        startDate = DateTime.Parse("1/1/" + timePeriod, ociEnUS);
                        endDate = DateTime.Parse("12/31/" + timePeriod, ociEnUS);
                        break;
                    case 7:
                        if (timePeriod.StartsWith("Q"))
                        {
                            GetQuaterStartEndDate(timePeriod, ref startDate, ref endDate, ociEnUS);
                        }
                        else
                        {
                            //yyyy.mm
                            sDate = timePeriod.Split('.');
                            startDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                            endDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(int.Parse(sDate[0]), int.Parse(sDate[1])) + "/" + sDate[0], ociEnUS);
                        }
                        break;
                    case 9:
                        //yyyy-yyyy
                        if (timePeriod.IndexOf(".") == -1)
                        {
                            sDate = timePeriod.Split('-');
                            startDate = DateTime.Parse("1/1/" + sDate[0], ociEnUS);
                            endDate = DateTime.Parse("12/31/" + sDate[1], ociEnUS);
                        }
                        else
                        {
                            sDate = timePeriod.Split('.');
                            startDate = DateTime.Parse("1/1/" + sDate[0], ociEnUS);
                            endDate = DateTime.Parse("12/31/" + sDate[1], ociEnUS);
                        }

                        break;
                    case 10:
                        //yyyy.mm.dd
                        sDate = timePeriod.Split('.');
                        startDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        endDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        break;
                    case 15:
                        //yyyy.mm-yyyy.mm
                        string[] sTempDate;
                        sTempDate = timePeriod.Split('-');
                        sDate = sTempDate[0].Split('.');
                        startDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                        sDate = sTempDate[1].Split('.');
                        endDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(int.Parse(sDate[0]), int.Parse(sDate[1])) + "/" + sDate[0], ociEnUS);
                        break;
                    case 21:
                        //yyyy.mm.dd-yyyy.mm.dd
                        string[] sTempDate2;
                        sTempDate2 = timePeriod.Split('-');
                        sDate = sTempDate2[0].Split('.');
                        startDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        sDate = sTempDate2[1].Split('.');
                        endDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        break;
                }
            }
            catch (Exception ex)
            {
                //startDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS);
                //endDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS);
            }
        }

        private static void GetQuaterStartEndDate(string timePeriod, ref System.DateTime startDate, ref System.DateTime endDate, System.Globalization.CultureInfo ociEnUS)
        {
            string StartMonth = "1";
            string EndMonth = "3";
            string EndDate = "31";

            if (!string.IsNullOrEmpty(timePeriod.Substring(1, 1).Trim()))
            {
                int Quarter = Convert.ToInt32(timePeriod.Substring(1, 1));

                switch (Quarter)
                {
                    case 1:
                        StartMonth = "1";
                        EndMonth = "3";
                        EndDate = "31";
                        break;
                    case 2:
                        StartMonth = "4";
                        EndMonth = "6";
                        EndDate = "30";
                        break;
                    case 3:
                        StartMonth = "7";
                        EndMonth = "9";
                        EndDate = "30";
                        break;
                    case 4:
                        StartMonth = "10";
                        EndMonth = "12";
                        EndDate = "31";
                        break;
                    default:
                        break;
                }
            }

            startDate = DateTime.Parse(StartMonth + "/1/" + timePeriod.Substring(3, 4), ociEnUS);
            endDate = DateTime.Parse(EndMonth + "/" + EndDate + "/" + timePeriod.Substring(3, 4), ociEnUS);
        }


        /// <summary>
        /// Validate date and returns true/false. 
        /// </summary>
        /// <param name="maskValue">MaskValue</param>
        /// <param name="value">date</param>
        /// <returns>true or false</returns>
        public static bool ValidateDate(string maskValue, string value)
        {
            bool RetVal = true;



            //TimePeriod format yyyy.mm
            if (RetVal && maskValue == "0000.00" && value.Length == 7)
            {
                RetVal = (TimePeriodFacade.ValidateMonth(7, value));
            }
            if (RetVal && maskValue == "Q0.0000" && value.Length == 7)
            {
                if (!string.IsNullOrEmpty(value.Substring(1, 1).Trim()) && (Convert.ToInt32(value.Substring(1, 1)) > 0 && Convert.ToInt32(value.Substring(1, 1)) < 5))
                {
                    RetVal = true;
                }
                else
                {
                    RetVal = false;
                }
            }

            //TimePeriod format yyyy.mm-yyyy.mm
            if (RetVal && maskValue == "0000.00-0000.00" && value.Length == 15)
            {
                RetVal = (TimePeriodFacade.ValidateMonth(15, value) && TimePeriodFacade.ValidateMonth(7, value));
            }

            //TimePeriod format yyyy.mm.dd
            if (RetVal && maskValue == "0000.00.00" && value.Length == 10)
            {
                if (TimePeriodFacade.ValidateMonth(7, value))
                {
                    RetVal = TimePeriodFacade.ValidateDay(Convert.ToInt32(value.Substring(0, 4)), Convert.ToInt32(value.Substring(5, 2)), Convert.ToInt32(value.Substring(8, 2)));
                }
                else
                {
                    RetVal = false;
                }
            }
            //TimePeriod format yyyy.mm.dd-yyyy.mm.dd
            if (RetVal && maskValue == "0000.00.00-0000.00.00" && value.Length == 21)
            {
                if (TimePeriodFacade.ValidateMonth(7, value) && TimePeriodFacade.ValidateMonth(18, value))
                {
                    RetVal = (TimePeriodFacade.ValidateDay(Convert.ToInt32(value.Substring(0, 4)), Convert.ToInt32(value.Substring(5, 2)), Convert.ToInt32(value.Substring(8, 2))) && TimePeriodFacade.ValidateDay(Convert.ToInt32(value.Substring(11, 4)), Convert.ToInt32(value.Substring(16, 2)), Convert.ToInt32(value.Substring(19, 2))));
                }
                else
                {
                    RetVal = false;
                }

            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Private Static --"

        #region "-- New/Dispose --"

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
            FormatCollection.Add(new TimeperiodInfo("Qn:yyyy", "Q0.0000"));
            //C1 End Comment
        }

        #endregion


        #region "-- Methods --"

        private static bool ValidateMonth(int length, string value)
        {
            int i;
            i = Convert.ToInt32(value.Substring(length - 2, 2));
            if (i >= 1 && i <= 12)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Invalid month");
                return false;
            }

        }

        private static bool ValidateDay(int year, int month, int day)
        {
            if (day < 1)
            {
                MessageBox.Show("Invalid Day");
                return false;
            }

            if ((month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && day > 31)
            {
                MessageBox.Show("Invalid Day");
                return false;
            }
            else if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
            {
                MessageBox.Show("Invalid Day");
                return false;
            }
            else if ((month == 2))
            {

                //Leap year condition
                if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
                {
                    if (day > 29)
                    {
                        MessageBox.Show("Invalid Day");
                        return false;
                    }
                    else
                        return true;
                }
                else if (day > 28)
                {
                    MessageBox.Show("Invalid Day");
                    return false;
                }

                return true;
            }
            else
            {
                return true;
            }

        }

        #endregion

        #endregion
    }

}