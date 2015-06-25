using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Data;
using System.Collections.Generic;

using Microsoft.VisualBasic;
using Microsoft.Win32;

using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Converter.Database;
using DevInfo.Lib.DI_LibBAL.Utility.MRU;
using DevInfo.Lib.DI_LibBAL.UI.Presentations;

using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.Metadata;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;



namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Provides common methods, filenames , application name and application path.
    /// </summary>
    public static class DICommon
    {

        #region "-- Private --"

        #region "-- Variables --"
        // -- FileNo used for Generating Uniqne FileName
        private static int FileNo;
        private static string ReservedFileNames = "con,prn,aux,nul,com1,com2,com3,com4,com5,com6,com7,com8,com9,lpt1,lpt2,lpt3,lpt4,lpt5,lpt6,lpt7,lpt18,lpt9,clock$";

        #endregion

        #region "-- Methods --"

        private static bool ISValidDESSheet(DIExcel desFile, int sheetIndex)
        {
            bool RetVal = false;
            DataTable SheetTable = null;

            try
            {

                SheetTable = desFile.GetDataTableFromSheet(desFile.GetSheetName(sheetIndex));

                // check rows count ( it should be greater than 9) & columns count should be grtthan 7
                if (SheetTable != null && SheetTable.Rows.Count > 9 && SheetTable.Columns.Count > 7)
                {
                    // check 4th row [ indicator =<indicator>]
                    if (!string.IsNullOrEmpty(Convert.ToString(SheetTable.Rows[3][0])) &&
                        !string.IsNullOrEmpty(Convert.ToString(SheetTable.Rows[3][1])))
                    {
                        // check 6th row [ unit =<unit>]
                        if (!string.IsNullOrEmpty(Convert.ToString(SheetTable.Rows[5][0])) &&
                        !string.IsNullOrEmpty(Convert.ToString(SheetTable.Rows[5][1])))
                        {
                            RetVal = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Get distinct IUSNIDs from the data table
        /// </summary>
        /// <param name="recSourceDt"></param>
        /// <returns></returns>
        private static string GetDistinctTimePeriods(DataTable recSourceDt)
        {
            string RetVal = string.Empty;
            try
            {
                string[] DistinctArray = new string[1];
                DistinctArray[0] = Timeperiods.TimePeriodNId;
                DataView dvTimePeriod = recSourceDt.DefaultView;
                DataTable dtTimePeriods = dvTimePeriod.ToTable(true, DistinctArray);
                foreach (DataRow Row in dtTimePeriods.Rows)
                {
                    RetVal += "," + Row[Timeperiods.TimePeriodNId].ToString();
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get distinct IUSNIDs from the data table
        /// </summary>
        /// <param name="recSourceDt"></param>
        /// <returns></returns>
        private static string GetDistinctIUS(DataTable recSourceDt)
        {
            string RetVal = string.Empty;
            try
            {
                string[] DistinctArray = new string[1];
                DistinctArray[0] = Indicator_Unit_Subgroup.IUSNId;
                DataView dvIUS = recSourceDt.DefaultView;
                DataTable dtIUS = dvIUS.ToTable(true, DistinctArray);
                foreach (DataRow Row in dtIUS.Rows)
                {
                    RetVal += "," + Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get distinct AreaNIDs from the data table
        /// </summary>
        /// <param name="recSourceDt"></param>
        /// <returns></returns>
        private static string GetDistinctArea(DataTable recSourceDt)
        {
            string RetVal = string.Empty;
            try
            {
                string[] DistinctArray = new string[1];
                DistinctArray[0] = Area.AreaNId;
                DataView dvArea = recSourceDt.DefaultView;
                DataTable dtArea = dvArea.ToTable(true, DistinctArray);
                foreach (DataRow Row in dtArea.Rows)
                {
                    RetVal += "," + Row[Area.AreaNId].ToString();
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        private static List<char> GetValidSpecialCharacterInGUId()
        {
            List<char> RetVal = new List<char>();

            //check for only english characters a-z A-Z 0-9 @-_$

            //RetVal.Add('@');
            //RetVal.Add('-');
            //RetVal.Add('_');
            //RetVal.Add('$');
            //abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789

            RetVal.AddRange(new char[] { '@', '-', '_', '$', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables --"

        // -- Get the decimal seprator on the basis of regional setting of current thread.
        public static string NumberDecimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public static string NumberGroupSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        public const string DecimalSperator = ".";
        public const string ValidSdmxRegularExpression = "^$A-Za-z@0-9_-";

        private static string _Applicationpath = string.Empty;
        /// <summary>
        /// Gets or sets The Application's Execution path
        /// </summary>
        public static string Applicationpath
        {
            get
            {
                return DICommon._Applicationpath;
            }
            set
            {
                DICommon._Applicationpath = value;
            }
        }


        /// <summary>
        ///  Gets the language file path
        /// </summary>
        public static string LangaugeFileNameWithPath = string.Empty;

        /// <summary>
        /// Gets country iso code file path for sources
        /// </summary>
        public static string CountryISoCodeFilename = string.Empty;

        /// <summary>
        /// Gets application startup path
        /// </summary>
        public static string ApplicationName = string.Empty;

        /// <summary>
        ///  Gets registry file path
        /// </summary>
        public static string RegistryFilePath = string.Empty;

        public static string DAEmergencyInfoFolderName = "EmergencyInfo";
        public static string DAExchangeFolderName = "Exchange";
        public static string DAStandardsFolderName = "Standards";

        /// <summary>
        /// Gets BlankDataValueSymbol for DES Import i.e. Symbol replacable by blank
        /// </summary>
        public static string DESImportBlankDataValueSymbol = string.Empty;

        /// <summary>
        /// System reserved files: con,prn,aux,nul,com1,com2,com3,com4,com5,com6,com7,com8,com9,lpt1,lpt2,lpt3,lpt4,lpt5,lpt6,lpt7,lpt18,lpt9,clock$
        /// </summary>
        public static List<string> ReservedFileOrFolderNames = new List<string>(ReservedFileNames.Split(",".ToCharArray()));

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Determine if the path file name is reserved.
        /// </summary>
        public static bool IsReservedFileOrFolderName(string FileName)
        {
            bool RetVal = false;

            if (ReservedFileOrFolderNames.Contains(FileName.ToLower()))
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Calculates the Screen Dots Per Inch of a Display Monitor
        /// </summary>
        /// <param name="monitorSize">Size, in inches</param>
        /// <param name="resolutionWidth">width resolution, in pixels</param>
        /// <param name="resolutionHeight">height resolution, in pixels</param>
        /// <returns>double presision value indicating the Screen Dots Per Inch</returns>
        public static double ScreenDPI(int monitorSize, int resolutionWidth, int resolutionHeight)
        {
            //int resolutionWidth = 1600;
            //int resolutionHeight = 1200;
            //int monitorSize = 19;
            if (0 < monitorSize)
            {
                double screenDpi = Math.Sqrt(Math.Pow(resolutionWidth, 2) + Math.Pow(resolutionHeight, 2)) / monitorSize;
                return screenDpi;
            }
            return 0;
        }


        /// <summary>
        /// Copies complete directory into target location
        /// </summary>
        /// <param name="TargetDirPath"></param>
        /// <param name="SrcDirPath"></param>
        public static void CopyCompleteDirectory(string TargetDirPath, string SrcDirPath)
        {
            DirectoryInfo TargetDirInfo = new DirectoryInfo(TargetDirPath);
            DirectoryInfo SourceDirInfo = new DirectoryInfo(SrcDirPath);
            string TargetSubDirPath = string.Empty;
            string SourceSubDirPath = string.Empty;

            try
            {
                // copies  subdirectories and their files
                foreach (DirectoryInfo SrcDirectory in SourceDirInfo.GetDirectories())
                {
                    TargetSubDirPath = TargetDirInfo + "\\" + SrcDirectory.Name;
                    SourceSubDirPath = SrcDirectory.FullName;

                    Directory.CreateDirectory(TargetSubDirPath);

                    //foreach (FileInfo srcFile in SrcDirectory.GetFiles())
                    //{
                    //    File.Copy(srcFile.FullName, TargetSubDirPath + "\\" + Path.GetFileName(srcFile.Name),true);
                    //}

                    DICommon.CopyCompleteDirectory(TargetSubDirPath, SourceSubDirPath);
                }

                // copies files source directory int target directory
                foreach (FileInfo srcFile in SourceDirInfo.GetFiles())
                {
                    File.Copy(srcFile.FullName, TargetDirPath + "\\" + Path.GetFileName(srcFile.Name), true);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }


        /// <summary>
        /// Get formatted compilation date time of file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns></returns>
        public static string GetCompileDateTime(string filePath)
        {
            string RetVal = string.Empty;
            try
            {
                //TODO it gives the datetime of file when it was copied from installation CD to client system ratather than the date the binary was created
                DateTime dt = File.GetLastWriteTime(filePath); //GetCreationTime
                RetVal = GetFormattedDateTime(dt);
            }
            catch (Exception)
            {
                //  UnauthorizedAccessException, ArgumentException, ArgumentNullException, PathTooLongException, IOException, NotSupportedException
            }
            return RetVal;
        }

        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static string GetFormattedDateTime(DateTime dateTime)
        {
            return (dateTime.Year.ToString("0000") + dateTime.Month.ToString("00") + dateTime.Day.ToString("00") + " - " + dateTime.Hour.ToString("00") + dateTime.Minute.ToString("00"));
        }

        /// <summary>
        /// It generates equal counts. Applicable for EqualCount, Continuous, Discontinuous DataValue ranges.
        /// </summary>
        /// <param name="_DV">DataView must have Numeric DataValue column</param>
        /// <param name="breakCount">No. of breaks count</param>
        /// <param name="minValue">minimun value of first Range.</param>
        /// <param name="maxValue">maximum value of last range.</param>
        /// <param name="decimals">no. of decimal places to be cons</param>
        /// <returns>Datatable with three columns (Range_From  ,  Range_To  ,  Count).</returns>
        public static DataTable GenerateEqualCount(DataView _DV, int breakCount, decimal minValue, decimal maxValue, int decimals)
        {

            DataTable RetVal = null;
            DataView _DVCopy = null;
            if (_DV != null)
            {
                RetVal = new DataTable();

                // Add Three columns in DataTable
                // Structure of table will be :
                // Range_From  |  Range_To  |  Count
                RetVal.Columns.Add("Range_From", typeof(string));
                RetVal.Columns.Add("Range_To", typeof(string));
                RetVal.Columns.Add("Count", typeof(int));


                //Assign DataView's original rowfilter
                string _ActualFilter = _DV.RowFilter;

                _DV.Sort = DataExpressionColumns.NumericData + " ASC";
                Int32 i;

                decimal _IntervalGap = (decimal)(1 / Math.Pow(10, decimals));

                //*** Differnce between To value and From Value of next item
                string _RangeFrom = "";
                string _RangeTo = "";
                int _StartIndex = 0;
                int _EndIndex = 0;
                bool _RedundantRange = false;
                int AreaCount = -1;
                int _ItemCount;

                if (_DV.RowFilter.Length > 0)
                {
                    _DV.RowFilter += " AND " + DataExpressionColumns.NumericData + " >= " + minValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + maxValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    _DV.RowFilter = DataExpressionColumns.NumericData + " >= " + minValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + maxValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                }

                //Copy DV into DVCopy
                _DVCopy = _DV.ToTable().DefaultView;

                //Counts to be handled for one break

                //-- Bugfix 2007-07-23 Equal count breaks for 53/4(floor) vs 35/4(ceiling)
                _ItemCount = (int)Math.Round(((decimal)_DVCopy.Count / breakCount), 0);

                if (_ItemCount > 1)
                {
                    _EndIndex = _ItemCount - 1;
                }
                else if (_ItemCount == 1)
                {
                    _EndIndex = _StartIndex;
                }

                // loop for each breaks, and get RangeFrom, RangeTo, AreaCount.
                for (i = 1; i <= breakCount; i++)
                {

                    if (i == 1)
                    {
                        _RangeFrom = Strings.FormatNumber(minValue, decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                    }
                    else
                    {
                        if (Convert.ToDecimal(_RangeTo) + _IntervalGap > maxValue)
                        {
                            _RangeFrom = Strings.FormatNumber(maxValue, decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                        }
                        else
                        {
                            _RangeFrom = Strings.FormatNumber(Convert.ToDecimal(_RangeTo) + _IntervalGap, decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                        }
                    }

                    if (i == breakCount)
                    {
                        _RangeTo = Strings.FormatNumber(maxValue, decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                    }
                    else if (_DVCopy.Count == 0 || Conversion.Val(_DVCopy[_EndIndex][DataExpressionColumns.NumericData]) < Conversion.Val(_RangeFrom))
                    {
                        _RangeTo = Strings.FormatNumber(maxValue, decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                    }
                    else
                    {
                        _RangeTo = Strings.FormatNumber(_DVCopy[_EndIndex][DataExpressionColumns.NumericData], decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);

                    }


                    do
                    {
                        if (Convert.ToDecimal(_RangeTo) < Convert.ToDecimal(_RangeFrom))
                        {
                            _EndIndex += 1;
                            if (_EndIndex > _DVCopy.Count - 1)
                            {
                                _EndIndex = _DVCopy.Count - 1;
                                _RangeTo = maxValue.ToString();

                                //-- break while loop, If rangeTo > RangeFrom and EndIndex = dv.rows.Count - 1
                                break;
                            }
                            else
                            {
                                _RangeTo = Strings.FormatNumber(_DVCopy[_EndIndex][DataExpressionColumns.NumericData], decimals, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);

                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true);

                    if (_DVCopy.Count > 0)
                    {
                        _DVCopy.RowFilter = DataExpressionColumns.NumericData + " >= " + (decimal.Parse(_RangeFrom)).ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + (decimal.Parse(_RangeTo).ToString(System.Globalization.CultureInfo.InvariantCulture));
                        //*** Bugfix 05 July 2006 Set the end index on the basis of record count in previous range rather than fixed _ItemCount

                        _EndIndex = _StartIndex + _DVCopy.Count - 1;

                        if (_EndIndex < 0)
                        {
                            _EndIndex = 0;
                        }
                    }


                    if (_RedundantRange)
                    {
                        AreaCount = 0;
                    }
                    else
                    {
                        AreaCount = _DVCopy.Count;
                    }

                    if (Convert.ToDecimal(_RangeTo) >= maxValue)
                    {
                        _RedundantRange = true;

                    }

                    //Add new Row in Datatable with required Values (RangeFrom, RangeTo, AreaCount)
                    RetVal.Rows.Add(_RangeFrom, _RangeTo, AreaCount);

                    _StartIndex = _EndIndex + 1;
                    _DVCopy.RowFilter = _ActualFilter;
                    if (_StartIndex > _DVCopy.Count - 1)
                        _StartIndex = _DVCopy.Count - 1;

                    if (_ItemCount > 1)
                    {
                        _EndIndex += _ItemCount;
                    }
                    else if (_ItemCount == 1)
                    {
                        _EndIndex += 1;
                    }

                    if (_EndIndex > _DVCopy.Count - 1)
                        _EndIndex = _DVCopy.Count - 1;

                }

            }
            return RetVal;
        }

        /// <summary>
        /// It generates equal counts. Applicable for EqualCount, Continuous, Discontinuous DataValue ranges.
        /// </summary>
        /// <param name="_DV">DataView must have Numeric DataValue column</param>
        /// <param name="breakCount">No. of breaks count</param>
        /// <param name="maxValue">maximum value of last range.</param>
        /// <param name="decimals">no. of decimal places to be cons</param>
        /// <param name="editedLegendRangeToValue">RangeTo value of specified edited Legend. this value is set by Client program explicitly. (Generally used when user edited any cell value)</param>
        /// <returns>Datatable with three columns (Range_From  ,  Range_To  ,  Count). and rows equal to breakCount.</returns>
        public static DataTable GenerateEqualCount(DataView _DV, int breakCount, decimal maxValue, int decimals, decimal editedLegendRangeToValue)
        {
            decimal _IntervalGap = (decimal)(1 / Math.Pow(10, decimals));

            //-Calculate new minValue for rest of Legend ranges.
            decimal newMinValue = editedLegendRangeToValue + _IntervalGap;

            DataTable RetVal = DICommon.GenerateEqualCount(_DV, breakCount, newMinValue, maxValue, decimals);


            return RetVal;
        }


        /// <summary>
        /// It generates equal Sized dataValue ranges.
        /// </summary>
        /// <param name="_DV">DataView must have Numeric DataValue column</param>
        /// <param name="m_BreakCount">No. of breaks count</param>
        /// <param name="m_Minimum">minimun value of first Range.</param>
        /// <param name="m_Maximum">maximum value of last range.</param>
        /// <param name="m_Decimals">no. of decimal places to be cons</param>
        /// <returns>Datatable with three columns (Range_From  ,  Range_To  ,  Count).</returns>
        public static DataTable GenerateEqualSize(DataView _DV, int breakCount, decimal minValue, decimal maxValue, int decimals)
        {

            DataTable RetVal = null;
            decimal _IntervalGap;
            decimal _Interval;
            string _ActualFilter = string.Empty;
            Int32 i;

            decimal _RangeFrom = 0;
            decimal _RangeTo = 0;
            bool RedundantRange = false;
            int AreaCount = -1;

            try
            {
                if (_DV != null)
                {
                    RetVal = new DataTable();

                    // Add Three columns in DataTable
                    // Structure of table will be :
                    // Range_From  |  Range_To  |  Count
                    RetVal.Columns.Add("Range_From", typeof(string));
                    RetVal.Columns.Add("Range_To", typeof(string));
                    RetVal.Columns.Add("Count", typeof(int));


                    if (decimals > 0)
                    {
                        _IntervalGap = (decimal)(1 / Math.Pow(10, decimals));
                    }
                    else
                    {
                        _IntervalGap = 1;
                    }


                    //Class Range

                    _Interval = Math.Round((maxValue - minValue + _IntervalGap) / breakCount, decimals);

                    _DV.Sort = DataExpressionColumns.NumericData + " ASC ";

                    //*** BugFix 10 May 2006 Handling for French Settings
                    if (_DV.RowFilter.Length > 0)
                    {
                        _DV.RowFilter += " AND " + DataExpressionColumns.NumericData + " >= " + minValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + maxValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        _DV.RowFilter = DataExpressionColumns.NumericData + " >= " + minValue.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + maxValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    }

                    _ActualFilter = _DV.RowFilter;

                    for (i = 1; i <= breakCount; i++)
                    {

                        if (i == 1)
                        {
                            _RangeFrom = Math.Round(minValue, decimals);
                            if (_Interval == 0)
                            {
                                _RangeTo = _RangeFrom;
                            }
                            else
                            {
                                _RangeTo = Math.Round(_RangeFrom + _Interval - _IntervalGap, decimals);
                            }

                        }
                        else
                        {
                            _RangeFrom = Math.Round(_RangeTo + _IntervalGap, decimals);
                            if (i < breakCount)
                            {
                                if (_Interval == 0)
                                {
                                    _RangeTo = _RangeFrom;
                                }
                                else
                                {
                                    _RangeTo = Math.Round(_RangeTo + _Interval, decimals);
                                }
                            }
                            else
                            {
                                _RangeTo = Math.Round(maxValue, decimals);
                            }
                        }

                        if (_RangeFrom > maxValue)
                        {
                            _RangeFrom = Math.Round(maxValue, decimals);
                        }

                        if (_RangeTo > maxValue)
                        {
                            _RangeTo = Math.Round(maxValue, decimals);
                        }
                        //*** BugFix 10 May 2006 Handling for French Settings
                        _DV.RowFilter = DataExpressionColumns.NumericData + " >= " + _RangeFrom.ToString(System.Globalization.CultureInfo.InvariantCulture) + " AND " + DataExpressionColumns.NumericData + " <= " + _RangeTo.ToString(System.Globalization.CultureInfo.InvariantCulture);



                        if (RedundantRange)
                        {
                            AreaCount = 0;
                        }
                        else
                        {
                            AreaCount = _DV.Count;
                        }

                        if (Convert.ToDecimal(_RangeTo) >= maxValue)
                        {
                            RedundantRange = true;

                        }

                        //Add new Row in Datatable with required Values (RangeFrom, RangeTo, AreaCount)
                        RetVal.Rows.Add(_RangeFrom, _RangeTo, AreaCount);

                        _DV.RowFilter = _ActualFilter;
                    }
                    _DV.RowFilter = _ActualFilter;

                }
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// It rounds the decimal value to nearest value towards zero.
        /// </summary>
        /// <param name="decimalDataValue"></param>
        /// <param name="roundDecimalPlaces"></param>
        /// <returns></returns>
        public static string RoundDecimalValueTowardsZero(string decimalDataValue, int roundDecimalPlaces)
        {
            string RetVal = decimalDataValue;
            string DecimalSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            try
            {
                if (decimalDataValue.Contains(DecimalSeperator))
                {
                    //- If +ve number, Round value towards Zero.
                    if (decimal.Parse(decimalDataValue) > 0)
                    {
                        string IntegerPart = decimalDataValue.Substring(0, decimalDataValue.IndexOf(DecimalSeperator));
                        string DecimalPart = decimalDataValue.Substring(decimalDataValue.IndexOf(DecimalSeperator) + 1);
                        string RoundedValue = string.Empty;

                        if (string.IsNullOrEmpty(DecimalPart) == false)
                        {
                            //- Get Rounded value to specified decimal Places
                            RoundedValue = DecimalPart.Substring(0, Math.Min(roundDecimalPlaces, DecimalPart.Length));
                        }
                        if (string.IsNullOrEmpty(RoundedValue) == false)
                        {
                            RetVal = IntegerPart + DecimalSeperator + RoundedValue;
                        }
                        else
                        {
                            RetVal = IntegerPart;
                        }
                    }
                    else
                    {
                        RetVal = Convert.ToString(Math.Round(decimal.Parse(decimalDataValue), roundDecimalPlaces, MidpointRounding.AwayFromZero));
                    }
                }
            }
            catch
            {
            }

            return RetVal;
        }

        /// <summary>
        ///  It rounds the decimal value to nearest value Away From zero.
        /// </summary>
        /// <param name="dataValue"></param>
        /// <param name="roundDecimalPlaces"></param>
        /// <returns></returns>
        public static string RoundDecimalValueAwayFromZero(string dataValue, int roundDecimalPlaces)
        {
            string RetVal = dataValue;
            string DecimalSeperator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            decimal DecimalValue;

            try
            {
                if (dataValue.Contains(DecimalSeperator))
                {
                    DecimalValue = decimal.Parse(dataValue);

                    //- If +ve number, Round value Away from Zero.
                    if (DecimalValue > 0)
                    {
                        string IntegerPart = string.Empty;
                        string DecimalPart = dataValue.Substring(dataValue.IndexOf(DecimalSeperator) + 1);

                        if (roundDecimalPlaces < DecimalPart.Length)
                        {
                            IntegerPart = dataValue.Replace(DecimalSeperator, "");
                            decimal Remainder = decimal.Parse(IntegerPart) % 10;
                            decimal Quotent = decimal.Parse(IntegerPart) / 10;

                            if (Remainder > 0)
                            {
                                IntegerPart = dataValue.Substring(0, dataValue.IndexOf(DecimalSeperator) + 1 + roundDecimalPlaces);
                                if (IntegerPart.EndsWith(DecimalSeperator))
                                {
                                    IntegerPart = IntegerPart.Replace(DecimalSeperator, "");
                                }
                                DecimalValue = decimal.Parse(IntegerPart) + (decimal)(1 / Math.Pow(10, roundDecimalPlaces));
                            }
                        }

                        RetVal = DecimalValue.ToString();
                    }
                    else
                    {
                        RetVal = Convert.ToString(Math.Round(decimal.Parse(dataValue), roundDecimalPlaces, MidpointRounding.AwayFromZero));
                    }
                }
            }
            catch
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Returns splitted string values
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] SplitStringNIncludeEmpyValue(string valueString, string delimiter)
        {
            string[] RetVal;

            //replace delimiter
            valueString = valueString.Replace(delimiter, "\n");
            RetVal = valueString.Split("\n".ToCharArray());

            return RetVal;
        }

        /// <summary>
        /// Gets a value that indicates whether a specified string value is numeric. 
        /// </summary>
        /// <param name="val">string value</param>
        /// <returns>boolean value specifieing whether a numeric value</returns>
        /// <remarks>
        /// Based on assumption that numeric values will be culture invariant, using "." as decimal separater
        /// http://www.codeproject.com/useritems/IsNumeric.asp
        /// http://msdn2.microsoft.com/en-us/library/system.globalization.numberstyles(vs.71).aspx 
        /// http://msdn2.microsoft.com/en-us/library/system.globalization.cultureinfo(vs.71).aspx 
        /// </remarks>
        public static bool IsNumeric(string val)
        {
            Decimal result;
            return Decimal.TryParse(val, System.Globalization.NumberStyles.Any, new System.Globalization.CultureInfo("", false), out result);
        }

        public static bool IsNumeric(string val, CultureInfo CultureInfo)
        {
            Double result;
            return Double.TryParse(val, System.Globalization.NumberStyles.Any, CultureInfo, out result);
        }

        /// <summary>
        /// Parse a  string value to double based on Inavariant culture
        /// </summary>
        /// <param name="DataValue"></param>
        /// <returns></returns>
        public static double ParseStringToDouble(string DataValue)
        {
            Double RetVal;
            Double.TryParse(DataValue, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("", false), out RetVal);
            return RetVal;
            // Variable to handle globalization / localization issues
            //private static CultureInfo Invc = CultureInfo.InvariantCulture;  //InvariantCulture is the neutral culture
            //private static NumberFormatInfo nfi = (NumberFormatInfo)Invc.GetFormat(typeof(System.Globalization.NumberFormatInfo));
            //private static DateTimeFormatInfo dfi = (DateTimeFormatInfo)Invc.GetFormat(typeof(System.Globalization.DateTimeFormatInfo));
            //double.Parse(DataValue,nfi)

        }

        /// <summary>
        /// Parse a  string value to double based on Inavariant culture
        /// </summary>
        /// <param name="DataValue"></param>
        /// <returns></returns>
        public static double ParseStringToDouble(string DataValue, CultureInfo currentCulture)
        {
            Double RetVal;
            Double.TryParse(DataValue, System.Globalization.NumberStyles.Float, currentCulture, out RetVal);
            return RetVal;
            // Variable to handle globalization / localization issues
            //private static CultureInfo Invc = CultureInfo.InvariantCulture;  //InvariantCulture is the neutral culture
            //private static NumberFormatInfo nfi = (NumberFormatInfo)Invc.GetFormat(typeof(System.Globalization.NumberFormatInfo));
            //private static DateTimeFormatInfo dfi = (DateTimeFormatInfo)Invc.GetFormat(typeof(System.Globalization.DateTimeFormatInfo));
            //double.Parse(DataValue,nfi)

        }

        /// <summary>
        /// Set the decimal sperator, Decimal seprator for the regional settings set by the user
        /// </summary>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public static string SetDecimalSperator(string dataValue)
        {
            string RetVal;
            try
            {
                RetVal = dataValue.Replace(NumberDecimalSeparator, DecimalSperator);
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns valid string value which can be used in database operations. 
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>string</returns>
        public static string MakeString(string value)
        {

            string RetVal = value;
            RetVal = RetVal.Replace("&", "&&");
            RetVal = RetVal.Replace("'", "''");
            return RetVal;
        }

        /// <summary>
        /// Removes Quotes From the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveQuotes(string value)
        {
            string RetVal = string.Empty;


            RetVal = value.Replace("'", "''");
            //-- Côte d'Ivoire issue. Filter clause works if "’" is traeted as normal text
            //RetVal = RetVal.Replace("’", "''");

            return RetVal;
        }


        /// <summary>
        /// Escapes sql reserved characters in like clause as it does not works with  "*" "%"
        /// </summary>
        /// <param name="value">Original Search characters</param>
        /// <returns>escaped string</returns>
        public static string EscapeWildcardChar(string value)
        {
            //sString = Replace(sString, "[", "[[]")
            //sString = Replace(sString, "]", "[]]")
            value = value.Replace("*", "[*]");
            value = value.Replace("%", "[%]");
            return value;
        }

        /// <summary>
        /// Checks the Existence of Folder, if folder does not exist then create folder.
        /// </summary>
        /// <param name="FolderName">Folder name</param>
        /// <returns>Boolean</returns>
        public static bool CheckNCreateFolder(string FolderName)
        {
            Boolean RetVal = true;

            try
            {
                if (!System.IO.Directory.Exists(FolderName))
                {
                    System.IO.Directory.CreateDirectory(FolderName);
                    //System.IO.Directory.Delete(FolderName, true);
                }


                //  System.IO.Directory.CreateDirectory(DICommon.DefaultFolder.DefaultExchangeTempFolder.Replace("\\", @"\"));

            }
            catch (Exception ex)
            {
                RetVal = false;

            }

            return RetVal;

        }

        /// <summary>
        /// Sets default folders path
        /// </summary>
        public static void SetDefaultFoldersPath()
        {

            // set other default folder

            // Data Exchange
            DICommon.DefaultFolder.DefaultExchangeTempFolder = DICommon.Applicationpath + "\\Bin\\Temp";
            DICommon.DefaultFolder.DefaultExchangeTemplateFolder = DICommon.Applicationpath + "\\Bin\\Template";

            // DataAdmin
            DICommon.DefaultFolder.DefaultLanguageFolder = DataAdmin.DAApplicationPath + "\\Language";
            DICommon.DefaultFolder.DefaultDAIconFolder = DataAdmin.DAApplicationPath + "\\Shortcut Icon";

        }

        /// <summary>
        /// Returns splitted string values
        /// </summary>
        /// <param name="value">string value</param>
        /// <param name="delimiter">delimiter</param>
        /// <returns>string[]</returns>
        public static string[] GetSplittedValues(string value, string delimiter)
        {
            string[] RetVal;
            string[] Arr = new string[1];       //To get splitted values
            Arr[0] = delimiter;
            RetVal = value.Split(Arr, StringSplitOptions.None);
            return RetVal;
        }

        /// <summary>
        /// Use UTF8 encoding to deserailize the text
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        public static byte[] StringToUTF8ByteArray(string SerializeText)
        {
            byte[] RetVal;
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                RetVal = encoding.GetBytes(SerializeText);
            }
            catch (Exception)
            {

                throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns splitted string values
        /// </summary>
        /// <param name="valueString"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] SplitString(string valueString, string delimiter)
        {
            string[] RetVal;
            int Index = 0;
            string Value;
            List<string> SplittedList = new List<string>();

            while (true)
            {
                Index = valueString.IndexOf(delimiter);
                if (Index == -1)
                {
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        SplittedList.Add(valueString);
                    }
                    break;
                }
                else
                {
                    Value = valueString.Substring(0, Index);
                    valueString = valueString.Substring(Index + delimiter.Length);
                    SplittedList.Add(Value);
                }

            }

            RetVal = SplittedList.ToArray();

            return RetVal;
        }

        /// <summary>
        /// We would be using UTF-8 encoding for the creating the XML stream for the custom object as it supports a wide range of Unicode character values and surrogates. 
        /// For this purpose, we will make use of the UTF8Encoding class provided by the .Net framework
        /// </summary>
        /// <param name="characters"></param>
        /// <returns>String</returns>
        public static string UTF8ByteArrayToString(byte[] characters)
        {
            string RetVal = string.Empty;
            try
            {
                UTF8Encoding Encoding = new UTF8Encoding();
                RetVal = Encoding.GetString(characters).Trim();
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns true if template or database is in DevInfo SP2 format
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        /// <remarks>For MsAccess</remarks>
        public static bool ISDevInfoSP2Database(string fileNameWPath)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries = null;

            try
            {
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                RetVal = DICommon.ISDevInfoSP2Database(DBConnection, DBQueries, false);
            }
            catch (Exception)
            {
                RetVal = false;
                // Do NOT throw any exception.
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }

            }
            return RetVal;
        }


        /// <summary>
        /// Returns true if template or database is in DevInfo 6.0 format
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        /// <remarks>For MsAccess</remarks>
        public static bool ISDevInfo6Database(string fileNameWPath)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries = null;
            DI6DBConverter DI6Converter = null;
            DBConverterDecorator DBConverter;
            try
            {
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));


                DI6Converter = new DI6DBConverter(DBConnection, DBQueries);
                RetVal = DI6Converter.IsValidDB(false);


                if (RetVal)
                {
                    // if database/tempalte already in DI6 format then convert it into latest format
                    DBConverter = new DBConverterDecorator(DBConnection, DBQueries);
                    DBConverter.DoConversion(false);
                }
            }
            catch (Exception)
            {
                RetVal = false;
                // Do NOT throw any exception.
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }

            }
            return RetVal;
        }

        public static bool ISDevInfoLatestDatabase(string fileNameWPath)
        {
            string DBFilePostFix = string.Empty;

            return DICommon.ISDevInfoLatestDatabase(fileNameWPath, ref DBFilePostFix);

        }


        /// <summary>
        /// Returns true if template or database is in current DevInfo 6.0 format
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        /// <remarks>For MsAccess</remarks>
        public static bool ISDevInfoLatestDatabase(string fileNameWPath, ref string dbFilePostFix)
        {
            bool RetVal = false;
            DIConnection DBConnection = null;
            DIQueries DBQueries = null;
            DBConverterDecorator DBConverterObject;
            DI7_0_0_0DBConverter DI7_DatabaseConvertor;
            try
            {
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);
                DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                DI7_DatabaseConvertor = new DI7_0_0_0DBConverter(DBConnection, DBQueries);
                RetVal = DI7_DatabaseConvertor.IsValidDB(false);

                dbFilePostFix = DI7_DatabaseConvertor.DBFilePostfix;

                if (RetVal)
                {
                    // if database/tempalte already in DI7 format then convert it into latest format
                    DBConverterObject = new DBConverterDecorator(DBConnection, DBQueries);
                    DBConverterObject.DoConversion(false);
                }
            }
            catch (Exception)
            {
                RetVal = false;
                // Do NOT throw any exception.
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }

            }
            return RetVal;
        }

        /// <summary>
        /// Returns true if template or database is in current DevInfo 6.0 format
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        /// <remarks>For Online db</remarks>
        public static bool ISDevInfoLatestDatabase(DIConnection dBConnection, DIQueries dBQueries)
        {
            bool RetVal = false;
            DBConverterDecorator DBConverterObject;

            try
            {
                DBConverterObject = new DBConverterDecorator(dBConnection, dBQueries);
                RetVal = DBConverterObject.IsValidDB(false);

            }
            catch (Exception)
            {
                RetVal = false;
                // Do NOT throw any exception.
            }
            return RetVal;
        }


        /// <summary>
        /// Returns true if template or database is in DevInfo SP2 format
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="isForOnlineDB"></param>
        /// <returns></returns>
        public static bool ISDevInfoSP2Database(DIConnection dbConnection, DIQueries dbQueries, bool isForOnlineDB)
        {
            bool RetVal = false;
            DI5SP2DBConverter SP2Converter = null;
            try
            {
                SP2Converter = new DI5SP2DBConverter(dbConnection, dbQueries);
                RetVal = SP2Converter.IsValidDB(isForOnlineDB);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }


        /// <summary>
        /// Creates DevInfoSP2 tables only if they are missing.
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="isForOnlineDB"></param>
        /// <returns></returns>
        public static bool CheckNCreateDevInfoSP2Database(DIConnection dbConnection, DIQueries dbQueries, bool isForOnlineDB)
        {
            bool RetVal = false;
            DI5SP2DBConverter SP2Converter = null;
            try
            {
                SP2Converter = new DI5SP2DBConverter(dbConnection, dbQueries);
                if (SP2Converter.IsValidDB(isForOnlineDB) == false)
                {
                    SP2Converter.DoConversion(isForOnlineDB);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true if template or database is DevInfo compatible
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="isForOnlineDB"></param>
        /// <returns></returns>
        /// <remarks>It will validate on the basis of default langauge and dataset</remarks>
        public static bool ISVaildDevInfoDatabase(DIConnectionDetails connectionDetails)
        {
            bool RetVal = false;
            bool IsForOnlineDB;
            DIConnection DBConnection = null;
            DIQueries DbQueries;
            DI5SP2DBConverter SP2Converter = null;
            try
            {
                DBConnection = new DIConnection(connectionDetails);
                //AvailableDB 
                DbQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                // check DB_Available_Databases table exists
                if (!string.IsNullOrEmpty(DBConnection.DIDataSetDefault()))
                {
                    RetVal = true;


                    ////// if exists then check for SP2 database
                    ////SP2Converter = new DI5SP2DBConverter(DBConnection, DbQueries);

                    ////switch (connectionDetails.ServerType)
                    ////{
                    ////    case DIServerType.Excel:
                    ////    case DIServerType.MsAccess:
                    ////        IsForOnlineDB = false;
                    ////        break;
                    ////    default:
                    ////        IsForOnlineDB = true;
                    ////        break;
                    ////}
                    ////RetVal = SP2Converter.IsValidDB(IsForOnlineDB);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                RetVal = false;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }
            }

            return RetVal;
        }


        /// <summary>
        /// Returns true if template or database is DevInfo compatible
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        /// <remarks>It will validate on the basis of default langauge and dataset</remarks>
        public static bool ISVaildDevInfoDatabase(string fileNameWPath)
        {
            bool RetVal = false;
            bool IsForOnlineDB;
            DIConnection DBConnection = null;
            DIQueries DbQueries;

            try
            {
                DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);
                //AvailableDB 
                DbQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                // check DB_Available_Databases table exists
                if (!string.IsNullOrEmpty(DBConnection.DIDataSetDefault()))
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
                RetVal = false;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Dispose();
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Check the application existence.
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns></returns>
        public static bool ISApplicationExist(string applicationFileName)
        {
            bool Retval = false;
            try
            {
                System.Diagnostics.Process Process;
                System.Diagnostics.ProcessStartInfo ProcessInfo = new System.Diagnostics.ProcessStartInfo(applicationFileName);
                //-- start the process.
                Process = System.Diagnostics.Process.Start(ProcessInfo);
                //-- kill the process
                Process.Kill();

                Retval = true;
            }
            catch (Exception ex)
            {
                Retval = false;
            }
            return Retval;
        }

        /// <summary>
        /// Check the existence of Office application.
        /// </summary>
        /// <param name="officeApplicationName">Office application name.</param>
        /// <returns>True, if installe din the system otherwise false.</returns>
        public static bool IsOfficeApplicationExist(string officeApplicationName)
        {
            bool Retval = false;
            try
            {
                string ComponentsKeyName = "SOFTWARE\\Microsoft\\Office";
                RegistryKey ComponentsKey = Registry.LocalMachine.OpenSubKey(ComponentsKeyName);
                string[] InstalledComponents = ComponentsKey.GetSubKeyNames();
                // -- Iterate the sub keys of Office key
                foreach (string Application in InstalledComponents)
                {
                    // -- Get the sub keys of office sub key.
                    RegistryKey Key = ComponentsKey.OpenSubKey(Application);
                    bool Success = OfficeExistence(Key, officeApplicationName);
                    // -- Check the key and sub key for the existence of office application.
                    if (!Success && Application.ToLower().Contains(officeApplicationName.ToLower()))
                    {
                        Retval = true;
                        break;
                    }
                    else if (Success)
                    {
                        Retval = true;
                        break;
                    }
                }

                if (!Retval)
                {
                    // Check whether Microsoft office application is installed on this computer,
                    // by searching the HKEY_CLASSES_ROOT\<Application Name>.Application key.
                    using (RegistryKey RegOfficeApp = Registry.ClassesRoot.OpenSubKey(officeApplicationName + ".Application"))
                    {
                        if (RegOfficeApp != null)
                        {
                            Retval = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Retval = false;
            }
            return Retval;
        }

        /// <summary>
        /// Check for existence of office application in its sub key.
        /// </summary>
        /// <param name="key">Registry key </param>
        /// <param name="officeApplicationName">Office application name</param>
        /// <returns></returns>
        private static bool OfficeExistence(RegistryKey key, string officeApplicationName)
        {
            bool Retval = false;
            try
            {
                string[] InstalledComponents = key.GetSubKeyNames();
                // -- Iterate the sub keys for the existence of office application.
                foreach (string Application in InstalledComponents)
                {
                    if (Application.ToLower().Contains(officeApplicationName.ToLower()))
                    {
                        Retval = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Retval = false;
            }
            return Retval;
        }

        /// <summary>
        /// Return the filter string which can be used in "Save File AS" or "File Types" box in the file dialog box
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static string GetFilterString(MRUKey fileType)
        {
            string RetVal = string.Empty;

            switch (fileType)
            {
                case MRUKey.MRU_DATABASES:
                    RetVal = FilterType.Database;
                    break;

                case MRUKey.MRU_ACCDB_DATABASES:
                    RetVal = FilterType.MSAccessDatabase;
                    break;

                case MRUKey.MRU_TEMPLATES:
                    RetVal = FilterType.MS_TEMPLATE_TPL;
                    break;

                case MRUKey.MRU_SPREADSHEETS:
                    RetVal = FilterType.MS_EXCEL_XLS;
                    break;

                case MRUKey.MRU_DI4_DATABASES:
                    RetVal = FilterType.Database;
                    break;

                case MRUKey.MRU_LANGUAGE:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_REPORTS:
                    RetVal = FilterType.MS_EXCEL_XLS;
                    break;

                case MRUKey.MRU_SPS:
                    RetVal = FilterType.SPSS;
                    break;

                case MRUKey.MRU_SPO:
                    RetVal = FilterType.SPO;
                    break;

                case MRUKey.MRU_DX_LANGUAGE:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_IMPORT_COMMENTS:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_WDI:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_DX_FREE_FORMAT:
                    //-- Excel and Excel 2007
                    RetVal = FilterType.MS_EXCEL_XLS + "|" + FilterType.MS_EXCEL_XLSX;
                    break;

                case MRUKey.MRU_DX_SMS:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_DX_DESKTOP_DATACAPTURE:
                    RetVal = FilterType.MS_XML_XML;
                    break;
                case MRUKey.MRU_PDA_FORMAT:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_DX_STATA:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_STATA_SMCL:
                    RetVal = FilterType.STATA_SMCL_FILES;
                    break;
                case MRUKey.MRU_PRESENTATIONS:
                    RetVal = FilterType.MS_EXCEL_XLS;
                    break;
                case MRUKey.MRU_HTML:
                    RetVal = FilterType.WEB_HTML_HTM + "|" + FilterType.WEB_HTML_HTML;
                    break;
                case MRUKey.MRU_METADATAXML:
                    RetVal = FilterType.MS_XML_XML;
                    break;
                case MRUKey.MRU_MetaDataRTF:
                    RetVal = FilterType.RTF;
                    break;
                case MRUKey.MRU_CRIS:
                    RetVal = FilterType.CRIS_FILES;
                    break;
                case MRUKey.MRU_SAS_XLS:
                    RetVal = FilterType.MS_EXCEL_XLS;
                    break;
                case MRUKey.MRU_STATA_DAT:
                    RetVal = FilterType.MS_XML_XML;
                    break;

                case MRUKey.MRU_CSPRO:
                    RetVal = FilterType.CSPRO_FILES;
                    break;

                case MRUKey.MRU_DIPROFILE:
                    RetVal = FilterType.MS_EXCEL_XLS + "|" + FilterType.MS_EXCEL_XLSX;
                    break;
                case MRUKey.MRU_IMPORT_CSV_CENSUS:
                    //-- for CSV
                    RetVal = "Comma Separated Values (*.csv files)|*.csv";
                    break;
                case MRUKey.MRU_DIDataFiles:
                    //-- DI Datahub Organizer
                    RetVal = FilterType.DI_DATA_FILES;
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Return Unique File Name
        /// </summary>
        /// <param name="fileNameWPath"> FileName With Path </param>
        /// <param name="identifier">File Identifier </param>
        /// <returns>(string) File Name With Path</returns>
        public static string GenerateUniqueFileName(string fileNameWPath, string identifier)
        {
            string RetVal = string.Empty;
            string FileName = string.Empty;
            string FileExtension = string.Empty;
            string FileDirectoryName;
            bool Found = true;

            FileExtension = Path.GetExtension(fileNameWPath);
            FileName = Path.GetFileNameWithoutExtension(fileNameWPath);

            if (FileName.Length > 254)
                FileName = Path.GetFileNameWithoutExtension(fileNameWPath).Substring(0, 254);


            FileDirectoryName = Path.GetDirectoryName(fileNameWPath);

            DICommon.FileNo = 1;

            if (string.IsNullOrEmpty(identifier))
            {
                // -- Search File Until FileName Exist
                while (Found)
                {
                    if (File.Exists(Path.Combine(FileDirectoryName, FileName + DICommon.FileNo.ToString() + "." + FileExtension)))
                    {
                        DICommon.FileNo++;
                    }
                    else
                    {
                        RetVal = Path.Combine(FileDirectoryName, FileName + DICommon.FileNo.ToString() + "." + FileExtension);
                        Found = false;
                    }
                }
            }
            else
            {
                RetVal = Path.Combine(FileDirectoryName, FileName + identifier + "." + FileExtension);
            }

            return RetVal;

        }

        /// <summary>
        /// Get the installation path of google earth installed on the local machine, from Windows Registry.
        /// </summary>
        /// <returns>path location of Google Earth.</returns>
        public static string GetGoogleEarthInstallPath()
        {
            string RetVal = string.Empty;

            try
            {
                string GoogleEarthKey = "Software\\Google\\Google Earth Plus";

                //RetVal = RegistryInfo.GetInstallLocationofParentKey(GoogleEarthKey);
                RegistryKey oRegKey = Registry.LocalMachine.OpenSubKey(GoogleEarthKey);
                if (oRegKey != null)
                {
                    if (oRegKey.GetValue("InstallDir") != null)
                    {
                        RetVal = (string)oRegKey.GetValue("InstallDir");

                        if (File.Exists(Path.Combine(RetVal, "googleearth.exe")) == false)
                        {
                            RetVal = string.Empty;
                        }
                    }
                    oRegKey.Close();
                }

                //-- If RetVal is null, try second possible Key in Registry
                if (string.IsNullOrEmpty(RetVal))
                {
                    GoogleEarthKey = @"Software\Classes\Applications\googleearth.exe\shell\open\command";
                    oRegKey = Registry.LocalMachine.OpenSubKey(GoogleEarthKey);
                    if (oRegKey != null)
                    {
                        if (oRegKey.GetValue("InstallDir") != null)
                        {
                            RetVal = (string)oRegKey.GetValue("InstallDir");
                        }
                        else
                        {
                            RetVal = (string)oRegKey.GetValue("");  //-- Default Key

                            RetVal = RetVal.Substring(0, RetVal.IndexOf("googleearth.exe"));
                        }

                        //-- remove invalid characters from path
                        string rgInvalid = @"[*?""<>|]";
                        System.Text.RegularExpressions.Regex oRegex = new System.Text.RegularExpressions.Regex(rgInvalid);
                        RetVal = oRegex.Replace(RetVal, "");
                        oRegKey.Close();

                        if (File.Exists(Path.Combine(RetVal, "googleearth.exe")) == false)
                        {
                            RetVal = string.Empty;
                        }
                    }
                }

            }
            catch
            {

            }

            try
            {
                //-- If RetVal is still blank, then Check if googleearth.exe is present in its default location "C:\Program Files\Google\Google Earth
                if (string.IsNullOrEmpty(RetVal))
                {
                    //-- Search File "GoogleEarth.exe" in default folder Location & its nested sub-folders too. 
                    RetVal = DICommon.SearchFile(@"C:\Program Files\Google\Google Earth", "googleearth.exe");
                }
            }
            catch
            {
            }

            return RetVal;
        }

        /// <summary>
        /// It searches the file in a given folder location & its all sub Folders. Return Folderpath if found.
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="fileNameToSearch"></param>
        /// <returns>return Folderpath if file found.</returns>
        public static string SearchFile(string folderPath, string fileNameToSearch)
        {
            string RetVal = string.Empty;

            if (Directory.Exists(folderPath))
            {
                // Loop each file in this specified folder 
                foreach (string file in Directory.GetFiles(folderPath))
                {
                    if (string.Compare(Path.GetFileName(file), fileNameToSearch, true) == 0)
                    {
                        RetVal = folderPath;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(RetVal))
                {
                    foreach (string subfolder in Directory.GetDirectories(folderPath))
                    {
                        // search subFolder. (using recursion technique)
                        RetVal = DICommon.SearchFile(subfolder, fileNameToSearch);

                        if (string.IsNullOrEmpty(RetVal) == false)
                        {
                            break;
                        }
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get valid file name after stripping reserved character if any
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetValidFileName(string fileName)
        {
            string RetVal = string.Empty;

            if (!string.IsNullOrEmpty(fileName))
            {
                RetVal = fileName;
                RetVal = RetVal.Replace("\\", "");
                RetVal = RetVal.Replace("/", "");
                RetVal = RetVal.Replace("*", "");
                RetVal = RetVal.Replace("\"", "");
                RetVal = RetVal.Replace("?", "");
                RetVal = RetVal.Replace("|", "");
                RetVal = RetVal.Replace("<", "");
                RetVal = RetVal.Replace(">", "");
                RetVal = RetVal.Replace(":", "");
                RetVal = RetVal.Replace("#", "");
                RetVal = RetVal.Replace("[", "(");
                RetVal = RetVal.Replace("]", ")");

                //-- long file name trimmed to 200
                if ((RetVal != null) && RetVal.Length > 200) RetVal = RetVal.Substring(0, 200);
            }

            return RetVal;
        }

        /// <summary>
        /// Get valid length file name
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="filenameWithoutExtension"></param>
        /// <returns></returns>
        public static string GetValidLengthFileName(string filepath, string filenameWithoutExtension, Presentation.PresentationType presentationType)
        {
            string RetVal = string.Empty;
            try
            {
                if (filepath.Length + filenameWithoutExtension.Length > 205)
                {
                    string FileName = filenameWithoutExtension.Replace(presentationType.ToString(), "");

                    RetVal = FileName.Substring(0, 205 - (FileName.Length + presentationType.ToString().Length));

                    RetVal = RetVal + " - " + presentationType.ToString();
                }
                else
                {
                    RetVal = filenameWithoutExtension;
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the valid FontStyle which are compatible with specified FontName.
        /// </summary>
        /// <param name="fontName">font Name</param>
        /// <returns>DataTable having 2 Columns (StyleName, StyleID). dataRow sample-: Bold, 1</returns>
        public static DataTable GetFontStyle(string fontName)
        {
            DataTable RetVal = new DataTable();
            RetVal.Columns.Add("StyleName");
            RetVal.Columns.Add("StyleID");

            try
            {
                FontFamily oFontFamily = new FontFamily(fontName);
                if (oFontFamily.IsStyleAvailable(FontStyle.Regular))
                {
                    RetVal.Rows.Add(new object[] { "Regular", 0 });
                }
                if (oFontFamily.IsStyleAvailable(FontStyle.Bold))
                {
                    RetVal.Rows.Add(new object[] { "Bold", 1 });
                }
                if (oFontFamily.IsStyleAvailable(FontStyle.Italic))
                {
                    RetVal.Rows.Add(new object[] { "Italic", 2 });
                }
                if (oFontFamily.IsStyleAvailable(FontStyle.Underline))
                {
                    RetVal.Rows.Add(new object[] { "Underline", 4 });
                }
                if (oFontFamily.IsStyleAvailable(FontStyle.Strikeout))
                {
                    RetVal.Rows.Add(new object[] { "Strikeout", 8 });
                }

            }
            catch
            {

            }

            return RetVal;
        }

        /// <summary>
        /// Updates database schema into latest format
        /// </summary>
        /// <returns></returns>
        public static bool UpdateDBSchemaToLatestDI6(string fileNameWPath)
        {
            bool RetVal = false;
            DBConverterDecorator DBConverter = null;

            try
            {
                DBConverter = new DBConverterDecorator(fileNameWPath);
                RetVal = DBConverter.DoConversion(false);

            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                if (DBConverter != null)
                {
                    DBConverter.Dispose();
                }
            }

            return RetVal;
        }

        public static List<string> GetOffLineDBList(string adaptationFolder)
        {
            List<string> RetVal = new List<string>();
            Adaptation Adp = null;
            string DataFolder = string.Empty;


            // If valid Data Folder
            if (string.IsNullOrEmpty(adaptationFolder) == false && Directory.Exists(adaptationFolder))
            {
                //-- Load Adaptation.xml present in Adaptation root folder
                Adp = Adaptation.Load(Path.Combine(adaptationFolder, Adaptation.ADAPTATION_FILE_NAME));
                DataFolder = Path.Combine(adaptationFolder, Adp.DirectoryLocation.Data);

                //Handling for mdb files
                if (Directory.Exists(DataFolder))
                {
                    foreach (FileInfo sFile in new DirectoryInfo(DataFolder).GetFiles("*" + FileExtension.Database))
                    {
                        RetVal.Add(sFile.FullName);
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the metadata Description of specified database along with IUS, Area, TimePeriod & Data Count.
        /// </summary>
        public static string GetDBMetaData(DIConnection dbConnection, DIQueries dbQueries)
        {
            string RetVal = string.Empty;

            string MaxAreaLevel = string.Empty;
            try
            {
                DataTable DTMeta = dbConnection.ExecuteDataTable(dbQueries.DBMetadata.GetDBMetadata());


                //-- Get Maximum Area Level in Area Tables
                String AreaLevel = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetMaxValue(dbQueries.TablesName.Area, Area.AreaLevel, string.Empty)).ToString();
                if (AreaLevel.Length > 0)
                {
                    MaxAreaLevel = System.Int32.Parse(AreaLevel).ToString();
                }

                if (DTMeta.Rows.Count > 0)
                {

                    RetVal = DTMeta.Rows[0][DBMetaData.Description].ToString();

                    // IUS count
                    if (DTMeta.Rows[0][DBMetaData.IUSCount].ToString() != null)
                    {
                        RetVal += "\n\n" + DILanguage.GetLanguageString("IUS") + " " + string.Format("{0:##,###}", DTMeta.Rows[0][DBMetaData.IUSCount]);
                    }

                    // Indicator
                    RetVal += "  " + DILanguage.GetLanguageString("INDICATOR") + " " + DTMeta.Rows[0][DBMetaData.IndicatorCount].ToString();

                    // Area
                    if (DTMeta.Rows[0][DBMetaData.AreaCount].ToString() != null)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("AREA") + " " + string.Format("{0:##,###}", DTMeta.Rows[0][DBMetaData.AreaCount]);
                    }

                    //-- AreaLevel
                    if (MaxAreaLevel.Length > 0)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("AREA_LEVEL") + " " + MaxAreaLevel;
                    }

                    // Time
                    RetVal += "  " + DILanguage.GetLanguageString("TIMEPERIOD") + " " + DTMeta.Rows[0][DBMetaData.TimeperiodCount].ToString();

                    // Source
                    RetVal += "  " + DILanguage.GetLanguageString("SOURCE") + " " + DTMeta.Rows[0][DBMetaData.SourceCount].ToString();

                    // Data count
                    if (DTMeta.Rows[0][DBMetaData.DataCount].ToString() != null)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("DATA") + " " + string.Format("{0:##,###}", DTMeta.Rows[0][DBMetaData.DataCount]);
                    }

                    //-- Get Last updated date from database table. (Column is "Pub_date")
                    if (DTMeta.Rows[0][DBMetaData.PublisherDate] != null)
                    {
                        if (string.IsNullOrEmpty(DTMeta.Rows[0][DBMetaData.PublisherDate].ToString()) == false)
                        {
                            RetVal += Environment.NewLine + DILanguage.GetLanguageString("LAST_UPDATED") + " " + ((DateTime)(DTMeta.Rows[0][DBMetaData.PublisherDate])).ToShortDateString();
                        }
                    }
                }
                else
                {
                    // Get area count
                    string AreaCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.Area, string.Empty)).ToString();

                    //// Get IUS count
                    String IUSCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.IndicatorUnitSubgroup, string.Empty)).ToString();
                    if (IUSCount.Length > 0)
                    {
                        RetVal += DILanguage.GetLanguageString("IUS") + " " + string.Format("{0:##,###}", System.Int32.Parse(IUSCount));
                    }

                    // Get Indicator count
                    string IndicatorCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.Indicator, string.Empty)).ToString();
                    RetVal += "  " + DILanguage.GetLanguageString("INDICATOR") + " " + IndicatorCount;

                    // Area Count
                    if (AreaCount.Length > 0)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("AREA") + " " + string.Format("{0:##,###}", System.Int32.Parse(AreaCount));
                    }

                    //-- AreaLevel
                    if (MaxAreaLevel.Length > 0)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("AREA_LEVEL") + " " + MaxAreaLevel;
                    }

                    // Get timeperiod count
                    String TimeperiodCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.TimePeriod, string.Empty)).ToString();
                    RetVal += "  " + DILanguage.GetLanguageString("TIMEPERIOD") + " " + TimeperiodCount;

                    //// Get source count
                    String SourceCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.IndicatorClassifications, IndicatorClassifications.ICType + " =" + DIQueries.ICTypeText[ICType.Source])).ToString();
                    RetVal += "  " + DILanguage.GetLanguageString("SOURCE") + " " + SourceCount;

                    //// Get data count
                    String DataCount = dbConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(dbQueries.TablesName.Data, string.Empty)).ToString();
                    if (DataCount.Length > 0)
                    {
                        RetVal += "  " + DILanguage.GetLanguageString("DATA") + " " + string.Format("{0:##,###}", System.Int32.Parse(DataCount));
                    }
                }

                RetVal = RetVal.Trim();
            }
            catch
            {

            }

            return RetVal;
        }

        /// <summary>
        /// Get the deleted source NIds
        /// </summary>
        /// <param name="iusNIds">distinct comma seprated IUSNIds</param>
        /// <param name="sourceDt">Source data table</param>
        /// <returns></returns>
        public static string GetDeletedSourceNIds(string iusNIds, DataTable sourceDt)
        {
            string Retval = string.Empty;
            try
            {
                string[] DistinctIUSNIds = new string[0];
                DataRow[] Rows = new DataRow[0];
                DataRow[] NonRecommnededRows = new DataRow[0];

                DistinctIUSNIds = SplitString(iusNIds, ",");

                //-- Loop thru the distinct IUSNIds
                foreach (string IUSNId in DistinctIUSNIds)
                {
                    //-- Get the recommended sources of the IUs NIds
                    Rows = sourceDt.Select(IndicatorClassificationsIUS.ICIUSOrder + " = 1 and " + Indicator_Unit_Subgroup.IUSNId + " = " + Convert.ToInt32(IUSNId));
                    if (Rows.Length > 0)
                    {
                        //-- If there are some recommended sources against the IUS, get the non-recommended source and move it to the deleted source NIds.

                        //NonRecommnededRows = sourceDt.Select(IndicatorClassificationsIUS.ICIUSOrder + " <> 1 or " + IndicatorClassificationsIUS.ICIUSOrder + " is null and " + Indicator_Unit_Subgroup.IUSNId + " = " + Convert.ToInt32(IUSNId));
                        NonRecommnededRows = sourceDt.Select("(" + IndicatorClassificationsIUS.ICIUSOrder + " <> 1 or " + IndicatorClassificationsIUS.ICIUSOrder + " is null) and " + Indicator_Unit_Subgroup.IUSNId + " = " + Convert.ToInt32(IUSNId));
                        if (NonRecommnededRows.Length > 0)
                        {
                            //-- Comma seprated string of recommended sources
                            foreach (DataRow Row in NonRecommnededRows)
                            {
                                if (string.IsNullOrEmpty(Retval))
                                {
                                    Retval = "'" + Row[Indicator_Unit_Subgroup.IUSNId].ToString() + Delimiter.NUMERIC_SEPARATOR + Row[IndicatorClassifications.ICNId].ToString() + "'";
                                }
                                else
                                {
                                    Retval += "," + "'" + Row[Indicator_Unit_Subgroup.IUSNId].ToString() + Delimiter.NUMERIC_SEPARATOR + Row[IndicatorClassifications.ICNId].ToString() + "'";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the deleted source NIds
        /// </summary>
        /// <param name="userselection">user selection</param>
        /// <param name="recSourceDt">Source data table</param>
        /// <returns></returns>
        public static string GetDeletedSourceNIds(UserSelection userselection, DataTable recSourceDt)
        {
            string Retval = string.Empty;
            try
            {
                string[] DistinctIUSNIds = new string[0];
                string[] DistinctTimePeriodNIds = new string[0];
                string[] DistinctAreaNIds = new string[0];
                DataRow[] Rows = new DataRow[0];
                DataRow[] NonRecommnededRows = new DataRow[0];

                //-- Get the distinct IUS
                if (string.IsNullOrEmpty(userselection.IndicatorNIds.Trim()))
                {
                    DistinctIUSNIds = SplitString(GetDistinctIUS(recSourceDt), ",");
                }
                else
                {
                    DistinctIUSNIds = SplitString(userselection.IndicatorNIds, ",");
                }

                //-- Get the distinct Areas
                if (string.IsNullOrEmpty(userselection.AreaNIds.Trim()))
                {
                    DistinctAreaNIds = SplitString(GetDistinctArea(recSourceDt), ",");
                }
                else
                {
                    DistinctAreaNIds = SplitString(userselection.AreaNIds, ",");
                }

                //-- Get the distinct TimePeriods
                if (string.IsNullOrEmpty(userselection.TimePeriodNIds.Trim()))
                {
                    DistinctTimePeriodNIds = SplitString(GetDistinctTimePeriods(recSourceDt), ",");
                }
                else
                {
                    DistinctTimePeriodNIds = SplitString(userselection.TimePeriodNIds, ",");
                }

                //-- Loop thru the distinct IUSNIds, TimePeriod and Area NId
                foreach (string IUSNId in DistinctIUSNIds)
                {
                    foreach (string TimePeriodNId in DistinctTimePeriodNIds)
                    {
                        foreach (string AreaNId in DistinctAreaNIds)
                        {
                            //-- Get the recommended sources of the IUS, Area and TimePeriod NIds
                            Rows = recSourceDt.Select(Data.ICIUSOrder + " = 1 and " + Data.IUSNId + " = " + Convert.ToInt32(IUSNId) + " and " + Data.TimePeriodNId + " = " + Convert.ToInt32(TimePeriodNId) + " and " + Data.AreaNId + " = " + Convert.ToInt32(AreaNId));
                            if (Rows.Length > 0)
                            {
                                //-- If there are some recommended sources against the IUS, Area and Time period, move that non-recommended source into the deleted source NIds.
                                NonRecommnededRows = recSourceDt.Select("(" + Data.ICIUSOrder + " <> 1 or " + Data.ICIUSOrder + " is null) and " + Data.IUSNId + " = " + Convert.ToInt32(IUSNId) + " and " + Data.TimePeriodNId + " = " + Convert.ToInt32(TimePeriodNId) + " and " + Data.AreaNId + " = " + Convert.ToInt32(AreaNId));
                                if (NonRecommnededRows.Length > 0)
                                {
                                    //-- Comma seprated string of recommended sources
                                    foreach (DataRow Row in NonRecommnededRows)
                                    {
                                        if (string.IsNullOrEmpty(Retval))
                                        {
                                            Retval = Row[Data.DataNId].ToString();
                                        }
                                        else
                                        {
                                            Retval += "," + Row[Data.DataNId].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the most recent time periods.
        /// </summary>
        /// <param name="userSelection"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static string GetMostRecentTimePeriods(UserSelection userSelection, DIConnection dbConnection, DIQueries dbQueries)
        {
            string RetVal = string.Empty;
            try
            {
                string AreaNId = string.Empty;
                string IUSNId = string.Empty;
                StringBuilder sbTimePeriodNIDs = new StringBuilder();

                DataTable dtData = dbConnection.ExecuteDataTable(dbQueries.Data.GetAutoSelectedIUSTimePeriodArea(userSelection));
                DataView dvData = dtData.DefaultView;

                //sort dataview in decending order of timeperiod so that latest record can be obtained
                dvData.Sort = Indicator_Unit_Subgroup.IUSNId + "," + Area.AreaNId + "," + Timeperiods.TimePeriod + " Desc";

                foreach (DataRow DRowParentTable in dvData.ToTable().Rows)
                {
                    // Get the record for latest timeperiod.
                    if (AreaNId != DRowParentTable[Area.AreaNId].ToString() || IUSNId != DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString())
                    {
                        AreaNId = DRowParentTable[Area.AreaNId].ToString();
                        IUSNId = DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString();
                        sbTimePeriodNIDs.Append("," + DRowParentTable[Data.TimePeriodNId].ToString());
                    }
                }
                if (sbTimePeriodNIDs.Length > 0)
                {
                    RetVal = sbTimePeriodNIDs.ToString().Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Returns image size
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        public static Size GetImageSize(string fileNameWPath)
        {
            Size RetVal = new Size();
            Image ImageFile = null;

            try
            {
                if (File.Exists(fileNameWPath))
                {
                    ImageFile = Image.FromFile(fileNameWPath);
                    RetVal = ImageFile.Size;
                    ImageFile.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Resize image
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static bool ResizeImage(string fileNameWithPath, int width, int height)
        {
            bool RetVal = false;

            try
            {
                Image img = null;
                if (string.IsNullOrEmpty(fileNameWithPath) || File.Exists(fileNameWithPath) == false)
                {
                    RetVal = false;
                }
                else
                {
                    FileStream fs = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
                    try
                    {
                        img = Image.FromStream(fs);
                        fs.Dispose();
                        fs = null;
                        RetVal = true;
                    }
                    catch (Exception ex)
                    {
                        //-- If the file does not have a valid image format or if GDI+ does not support the pixel format
                        //-- of the file, Image.FromFile method throws an OutOfMemoryException exception. 
                        fs.Dispose();
                        fs = null;
                        img = null;
                        RetVal = false;
                    }
                }

                if (RetVal)
                {
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Math.Min(img.Width, width), Math.Min(img.Height, height));
                    Graphics g = Graphics.FromImage(bmp);
                    MemoryStream tempStream = new MemoryStream();

                    g.Clear(Color.Transparent);
                    g.DrawImage(img, 0, 0, Math.Min(img.Width, width), Math.Min(img.Height, height));
                    bmp.Save(fileNameWithPath, System.Drawing.Imaging.ImageFormat.Jpeg);


                    bmp.Dispose();
                    bmp = null;
                    g.Dispose();
                    g = null;
                    tempStream.Dispose();
                    tempStream = null;
                    img.Dispose();
                    img = null;

                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }



        /// <summary>
        /// Get the image object.
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Image GetImage(string fileNameWithPath, int width, int height)
        {
            Image Retval = null;
            try
            {
                Image img = null;
                if (string.IsNullOrEmpty(fileNameWithPath) || File.Exists(fileNameWithPath) == false)
                {
                    return null;
                }
                else
                {
                    FileStream fs = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
                    try
                    {
                        img = Image.FromStream(fs);
                        fs.Dispose();
                        fs = null;
                    }
                    catch (Exception ex)
                    {
                        //-- If the file does not have a valid image format or if GDI+ does not support the pixel format
                        //-- of the file, Image.FromFile method throws an OutOfMemoryException exception. 
                        fs.Dispose();
                        fs = null;
                        img = null;
                        return null;
                    }
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Math.Min(img.Width, width), Math.Min(img.Height, height));
                Graphics g = Graphics.FromImage(bmp);
                MemoryStream tempStream = new MemoryStream();
                Retval = null;
                g.Clear(Color.Transparent);
                g.DrawImage(img, 0, 0, Math.Min(img.Width, width), Math.Min(img.Height, height));
                bmp.Save(tempStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                Retval = Image.FromStream(tempStream);

                bmp.Dispose();
                bmp = null;
                g.Dispose();
                g = null;
                tempStream.Dispose();
                tempStream = null;
                img.Dispose();
                img = null;
            }
            catch (Exception)
            {

            }
            return Retval;
        }

        public static bool CheckInternetConnectivity()
        {
            bool RetVal = false;
            System.Net.WebRequest WebReq;
            try
            {
                //System.Net.IPHostEntry objIPHE = System.Net.Dns.GetHostByName("www.google.com");
                //RetVal = true;
                System.Uri Url = new System.Uri("http://www.google.com");


                System.Net.WebResponse Resp;
                WebReq = System.Net.WebRequest.Create(Url);


                Resp = WebReq.GetResponse();
                Resp.Close();
                WebReq = null;
                RetVal = true;

            }
            catch
            {
                WebReq = null;
                RetVal = false; // host not reachable.
            }

            return RetVal;
        }

        /// <summary>
        ///  Returns true if file  is in Devinfo Data Entry Spreadsheet format
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="checkAllSheets">true to check all sheets otherwise set false</param>
        /// <returns></returns>
        public static bool ISValidDIDataEntrySphreadsheet(string fileNameWPath, bool checkAllSheets)
        {
            bool RetVal = false;
            DICulture DICultureInfo = new DICulture();
            DIExcel DESFile = null;

            try
            {
                if (System.IO.Path.GetExtension(fileNameWPath) == DICommon.FileExtension.Excel | System.IO.Path.GetExtension(fileNameWPath) == DICommon.FileExtension.Excel2007)
                {
                    try
                    {
                        DICultureInfo.SetInvariantCulture();
                        DESFile = new UI.Presentations.DIExcelWrapper.DIExcel(fileNameWPath);

                        //  check sheet/s
                        for (int SheetIndex = 0; SheetIndex < DESFile.AvailableWorksheetsCount; SheetIndex++)
                        {
                            if (DICommon.ISValidDESSheet(DESFile, SheetIndex))
                            {
                                RetVal = true;
                            }
                            else
                            {
                                RetVal = false;
                                break;
                            }

                            if (!checkAllSheets)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        DICultureInfo.RestoreOriginalCulture();
                        if (DESFile != null)
                        {
                            DESFile.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }


        /// <summary>
        /// Indents the given xml string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string IndentXMLString(string xml)
        {
            string RetVal = string.Empty;
            string outXml = string.Empty;
            MemoryStream ms = new MemoryStream();
            // Create a XMLTextWriter that will send its output to a memory stream (file)
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.Unicode);
            XmlDocument doc = new XmlDocument();

            try
            {
                // Load the unformatted XML text string into an instance
                // of the XML Document Object Model (DOM)
                doc.LoadXml(xml);

                // Set the formatting property of the XML Text Writer to indented
                // the text writer is where the indenting will be performed
                xtw.Formatting = Formatting.Indented;

                // write dom xml to the xmltextwriter
                doc.WriteContentTo(xtw);
                // Flush the contents of the text writer
                // to the memory stream, which is simply a memory file
                xtw.Flush();

                // set to start of the memory stream (file)
                ms.Seek(0, SeekOrigin.Begin);
                // create a reader to read the contents of
                // the memory stream (file)
                StreamReader sr = new StreamReader(ms);
                // return the formatted string to caller
                RetVal = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Returns the path of excel template folder.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Applicable only for Office 2007. This will get the excel template foder path.</remarks>
        public static string GetTemplatePath()
        {
            string Retval = string.Empty;
            try
            {
                string TemplatePath = @"AppData\Roaming\Microsoft\Templates\Charts";
                RegistryKey ShellFolder = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders");
                Retval = Path.Combine(ShellFolder.GetValue("Templates").ToString(), "Charts");
                string[] Paths = DICommon.SplitString(Retval, @"\");
                Retval = Paths[0] + @"\";
                for (int index = 1; index <= 2; index++)
                {
                    Retval = Path.Combine(Retval, Paths[index]);
                }
                Retval = Path.Combine(Retval, TemplatePath);
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        public static Size GetImageSizeByAspectRatio(int originalWidth, int originalHeight, int pageWidth, int pageHeight, bool onlyResizeIfWider)
        {
            Size RetVal = new Size();

            int NewHeight = 0;

            // resize only if wider
            if (onlyResizeIfWider)
            {
                if (originalWidth <= pageWidth)
                {
                    pageWidth = originalWidth;
                }
            }

            NewHeight = originalHeight * pageWidth / originalWidth;
            if (NewHeight > pageHeight)
            {
                // Resize with height instead
                pageWidth = originalWidth * pageHeight / originalHeight;
                NewHeight = pageHeight;
            }

            RetVal.Height = NewHeight;
            RetVal.Width = pageWidth;

            return RetVal;
        }

        /// <summary>
        /// true, if specified file is locked (in use) by another program or process.
        /// </summary>
        /// <param name="fileNamePath"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string fileNamePath)
        {
            bool RetVal = false;


            System.IO.FileStream fs = null;
            try
            {
                if (File.Exists(fileNamePath))
                {
                    fs = System.IO.File.Open(fileNamePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
            }
            catch (System.IO.IOException ex)
            {
                RetVal = true;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get the bottom panel button adaptation based Text
        /// </summary>
        /// <param name="adaptation"></param>
        /// <param name="bottomPanelButtonID"></param>
        /// <returns></returns>
        public static string GetBottomPanelAdaptationBasedString(Adaptation adaptation, string bottomPanelButtonID)
        {
            string RetVal = string.Empty;
            try
            {
                //-- Get the adaptation based text
                foreach (Adaptation.BottomPanelButton bpButton in adaptation.BottomPanel.Buttons)
                {
                    if (bpButton.ID.ToLower() == bottomPanelButtonID.ToLower())
                    {
                        RetVal = bpButton.Text;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }


        /// <summary>
        /// Remove specail character from the XML
        /// </summary>
        /// <param name="icName"></param>
        /// <returns></returns>
        public static string RemoveXMLSpecialCharacter(string xmlString)
        {
            string Retval = string.Empty;
            try
            {
                xmlString = xmlString.Replace("&", "&amp;");
                xmlString = xmlString.Replace("<", "&lt;");
                xmlString = xmlString.Replace(">", "&gt;");
                xmlString = xmlString.Replace("\"", "&quot;");
                xmlString = xmlString.Replace("'", "&#39;");
                Retval = xmlString;
            }
            catch (Exception)
            {
                Retval = xmlString;
            }
            return Retval;
        }

        /// <summary>
        /// Convert old metadata into latest Metadata Format
        /// </summary>
        /// <param name="metadataXml"></param>
        /// <returns></returns>
        public static string CheckNConvertMetadataXml(string metadataXml)
        {
            string RetVal = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            try
            {
                if (!string.IsNullOrEmpty(metadataXml))
                {
                    RetVal = metadataXml;

                    XmlDoc.LoadXml(metadataXml);
                    if (XmlDoc.SelectNodes(DA.DML.Constants.DIMetedataRootNode).Count > 0)
                    {
                        RetVal = MetadataConverter.ConvertXml(metadataXml);
                    }
                }
            }
            catch { }

            return RetVal;
        }

        /// <summary>
        /// Check url is valid
        /// </summary>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public static bool ISValidUrl(string urlPath)
        {
            bool RetVal = false;
            WebRequest WRequest = null;
            WebResponse WResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(urlPath))
                {
                    WRequest = WebRequest.Create(urlPath);
                    WResponse = WRequest.GetResponse();
                    if (WResponse != null)
                    {
                        RetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Check web service url is valid
        /// </summary>
        /// <param name="WebServiceURLPath"></param>
        /// <returns></returns>
        public static bool ISValidWSUrl(string WebServiceURLPath)
        {
            bool RetVal = false;
            WebRequest WRequest = null;
            WebResponse WResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(WebServiceURLPath) && WebServiceURLPath.EndsWith(".asmx"))
                {
                    WRequest = WebRequest.Create(WebServiceURLPath);
                    WResponse = WRequest.GetResponse();
                    if (WResponse != null && WResponse.ResponseUri != null && !WResponse.ResponseUri.IsFile)
                    {
                        RetVal = true;
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Valid charactres: A-Z, a-z, @, 0-9, _, -, $
        /// Regular Expression for sdmx [^$A-Za-z@0-9_-] [SDMX: 2.1 [a-zA-Z0-9_@$\-]+]
        /// http://opensdmx.wikispaces.com/SDMX+Code+Convention
        /// </summary>
        /// <param name="MetadataGid"></param>
        /// <returns></returns>
        public static bool IsValidGIdForSDMXRule(string Gid)
        {
            bool RetVal = true;
            Regex regex;
            List<char> ValidChars = DICommon.GetValidSpecialCharacterInGUId();

            //check for valid character in sdmx
            if (!string.IsNullOrEmpty(Gid))
            {
                foreach (char gidCh in Gid)
                {
                    if (!ValidChars.Contains(gidCh))
                    {
                        RetVal = false;
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Valid charactres: A-Z, a-z, @, 0-9, _, -, $
        /// Regular Expression for sdmx [^$A-Za-z@0-9_-] [SDMX: 2.1 [a-zA-Z0-9_@$\-]+]
        /// http://opensdmx.wikispaces.com/SDMX+Code+Convention
        /// </summary>
        /// <param name="MetadataGid"></param>
        /// <returns></returns>
        public static string GetValidGIdForSDMXRule(string Gid)
        {
            StringBuilder RetVal = new StringBuilder();
            List<char> ValidChars = DICommon.GetValidSpecialCharacterInGUId();
            bool IsInvalidChar = false;

            //check for valid character in sdmx
            if (!string.IsNullOrEmpty(Gid))
            {
                //replace spaces with underscore
                Gid = Gid.Replace(" ", "_");

                foreach (char gidCh in Gid)
                {
                    if (!ValidChars.Contains(gidCh))
                    {
                        IsInvalidChar = true;
                    }

                    RetVal.Append((IsInvalidChar) ? '_' : gidCh);

                    IsInvalidChar = false;
                }
            }

            return RetVal.ToString();
        }


        /// <summary>
        /// check is DI7 database
        /// </summary>
        /// <returns></returns>
        public static bool IsDI7Database(DIConnection diConnection, DIQueries diQueries)
        {
            bool RetVal = false;

            if (diConnection != null)
            {
                //check for any version from database
                if (diConnection.ExecuteDataTable(diQueries.DBVersion.GetRecords(DevInfo.Lib.DI_LibBAL.Converter.Database.Constants.Versions.DI7_0_0_0)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// check is DI7 database
        /// </summary>
        /// <returns></returns>
        public static bool IsDI7Database(string databaseFileNameWPath)
        {
            bool RetVal = false;
            string DataPrefix = string.Empty;

            DIConnection diConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, databaseFileNameWPath, string.Empty, "unitednations2000");
            DIQueries diQueries;

            if (diConnection != null)
            {
                DataPrefix = diConnection.DIDataSetDefault();
                diQueries = new DIQueries(DataPrefix, diConnection.DILanguageCodeDefault(DataPrefix));

                if (diConnection.ExecuteDataTable(diQueries.DBVersion.GetRecords(DevInfo.Lib.DI_LibBAL.Converter.Database.Constants.Versions.DI7_0_0_0)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }

            return RetVal;
        }

        public static List<string> GetCommaSeperatedListOfGivenColumn(DataTable dataTable, string columnName, bool isSortByGivenColumn, string filterText)
        {
            List<string> RetVal = new List<string>();

            try
            {
                dataTable.DefaultView.RowFilter = filterText;
                dataTable = dataTable.DefaultView.ToTable();

                if (dataTable.Columns.Contains(columnName))
                {
                    if (isSortByGivenColumn)
                        dataTable.DefaultView.Sort = columnName;

                    foreach (DataRow dvRow in dataTable.DefaultView.ToTable(true, columnName).Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dvRow[columnName])))
                            RetVal.Add(Convert.ToString(dvRow[columnName]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }

        public static List<string> GetCommaSeperatedListOfGivenColumn(DataView dataView, string columnName)
        {
            List<string> RetVal = new List<string>();

            try
            {
                if (dataView.Table.Columns.Contains(columnName))
                {
                    foreach (DataRowView dvRow in dataView)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dvRow[columnName])))
                        {
                            if (!RetVal.Contains(Convert.ToString(dvRow[columnName])))
                                RetVal.Add(Convert.ToString(dvRow[columnName]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }

        public static List<string> GetAllUnmatchedGIDs(DataTable table, string gidColumnName)
        {
            List<string> RetVal = new List<string>();

            foreach (string gid in DICommon.GetCommaSeperatedListOfGivenColumn(table, gidColumnName, false, string.Empty))
            {
                if (!DICommon.IsValidGIdForSDMXRule(gid))
                {
                    RetVal.Add(gid);
                }
            }

            return RetVal;

        }

        public static string GetDIVersionDetailsHTMLStringForHTMLLog(string databaseFileName)
        {
            string RetVal = string.Empty;
            DIConnection TempDBConnection = null;

            try
            {
                if (File.Exists(databaseFileName))
                {
                    TempDBConnection = new DIConnection(DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess, string.Empty, string.Empty, databaseFileName, string.Empty, string.Empty);
                    RetVal = "<span style=\"color:gray;font-size:10px;\"> (v." + Convert.ToString(TempDBConnection.ExecuteScalarSqlQuery(DIQueries.GetDatabaseVersion())) + ") </span>";
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (TempDBConnection != null)
                {
                    TempDBConnection.Dispose();
                }
            }

            return RetVal;
        }

        public static object GetResourceByResourceName(string ResourceNameWithoutExt)
        {
            object RetaVal;

            RetaVal = DevInfo.Lib.DI_LibBAL.Resource1.ResourceManager.GetObject(ResourceNameWithoutExt);

            return RetaVal;
        }

        #endregion

        #region "-- InnerClasses --"

        /// <summary>
        /// Defines image file type
        /// </summary>
        public enum ImageFileType
        {
            Icon,
            GIF,
            JPG,
            PNG
        }

        /// <summary>
        /// Represents constant values for File dialog filter types.
        /// </summary>
        public class FilterType
        {
            /// <summary>
            /// Microsoft Access(*.mdb)
            /// </summary>
            public const string MS_ACCESS_MDB = "Microsoft Access(*.mdb)|*.mdb";
            /// <summary>
            /// DevInfo 5.0 Indicator(*.xls)
            /// </summary>
            public const string MS_EXCEL_IND_DI5 = "DevInfo 5.0 Indicator(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Indicator(*.xls)
            /// </summary>
            public const string MS_EXCEL_IND_DI4 = "DevInfo 4.0 Indicator(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 5.0 Sector(*.xls)
            /// </summary>
            public const string MS_EXCEL_SEC_DI5 = "DevInfo 5.0 Sector(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Indicator(*.xls)
            /// </summary>
            public const string MS_EXCEL_SEC_DI4 = "DevInfo 4.0 Indicator(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 5.0 Goal(*.xls)
            /// </summary>
            public const string MS_EXCEL_GL_DI5 = "DevInfo 5.0 Goal(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Goal(*.xls)
            /// </summary>
            public const string MS_EXCEL_GL_DI4 = "DevInfo 4.0 Goal(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 5.0 Convention(*.xls)
            /// </summary>
            public const string MS_EXCEL_CV_DI5 = "DevInfo 5.0 Convention(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Convention(*.xls)
            /// </summary>
            public const string MS_EXCEL_CV_DI4 = "DevInfo 4.0 Convention(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 5.0 Theme(*.xls)
            /// </summary>
            public const string MS_EXCEL_TH_DI5 = "DevInfo 5.0 Theme(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Theme(*.xls)
            /// </summary>
            public const string MS_EXCEL_TH_DI4 = "DevInfo 4.0 Theme(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 5.0 Institution(*.xls)
            /// </summary>
            public const string MS_EXCEL_INST_DI5 = "DevInfo 5.0 Institution(*.xls)|*.xls";
            /// <summary>
            /// DevInfo 4.0 Institution(*.xls)
            /// </summary>
            public const string MS_EXCEL_INST_DI4 = "DevInfo 4.0 Institution(*.xls)|*.xls";

            /// <summary>
            /// Microsoft Excel Files(*.xls)
            /// </summary>
            public const string MS_EXCEL_XLS = "Microsoft Excel Files(*.xls)|*.xls";

            /// <summary>
            /// Microsoft Excel Files(*.xlsx)
            /// </summary>
            public const string MS_EXCEL_XLSX = "Microsoft Excel Files(*.xlsx)|*.xlsx";

            /// <summary>
            /// Microsoft PowerPoint Presentations(*.ppt)
            /// </summary>
            public const string MS_POWERPOINT_PPT = "Microsoft PowerPoint Presentations(*.ppt)|*.ppt";

            /// <summary>
            /// Microsoft Excel Files(*.csv files)
            /// </summary>
            public const string MS_EXCEL_CSV = "Microsoft Excel Files(*.csv files)|*.csv";
            /// <summary>
            /// Icon Files(*.ico)
            /// </summary>
            public const string MS_ICON_ICO = "Icon Files(*.ico)|*.ico";
            /// <summary>
            /// DevInfo Template(*.tpl)
            /// </summary>
            public const string MS_TEMPLATE_TPL = "DevInfo Template(*.tpl)|*.tpl";
            /// <summary>
            /// Adodbe PDF(*.pdf)
            /// </summary>
            public const string ADOBE_EXT_PDF = "Adobe PDF(*.pdf)|*.pdf";

            /// <summary>
            /// Microsoft Help Files(*.hlp)
            /// </summary>
            public const string MS_HELP_HLP = "Microsoft Help Files(*.hlp)|*.hlp";

            /// <summary>
            /// XML Files(*.xml)
            /// </summary>
            public const string MS_XML_XML = "XML Files(*.xml)|*.xml";

            /// <summary>
            /// XSL Files(*.xsl)
            /// </summary>
            public const string MS_XSL_XSL = "XSL Files(*.xsl)|*.xsl";

            /// <summary>
            /// Shape Files(*.shp)
            /// </summary>
            public const string ESRI_SHAPE_SHP = "Shape Files(*.shp)|*.shp";

            /// <summary>
            /// Html Files(*.html)
            /// </summary>
            public const string WEB_HTML_HTML = "Html Files(*.html)|*.html";

            /// <summary>
            /// Html Files(*.htm)
            /// </summary>
            public const string WEB_HTML_HTM = "Html Files(*.htm)|*.htm";

            /// <summary>
            /// Presentation Files(*.ppt)
            /// </summary>
            public const string WEB_HTML_PPT = "Presentation Files(*.ppt)|*.ppt";
            /// <summary>
            /// Tour Files(*.pps)
            /// </summary>
            public const string WEB_HTML_PPS = "Tour Files(*.pps)|*.pps";
            /// <summary>
            /// Presentation Files(*.xls)
            /// </summary>
            public const string WEB_HTML_PRE = "Presentation Files(*.xls)|*.xls";
            /// <summary>
            /// Sound Files(*.wav)
            /// </summary>
            public const string WEB_HTML_SUN = "Sound Files(*.wav)|*.wav";
            /// <summary>
            /// Picture files(*.gif)
            /// </summary>
            public const string PIC_IMAGE_GIF = "Picture files(*.gif)|*.gif";
            /// <summary>
            /// Picture files(*.jpg)
            /// </summary>
            public const string PIC_IMAGE_JPG = "Picture files(*.jpg)|*.jpg";
            /// <summary>
            /// Picture files(*.png)
            /// </summary>
            public const string PIC_IMAGE_PNG = "Picture files(*.png)|*.png";
            /// <summary>
            /// SDMX(*.xml)
            /// </summary>
            public const string SDMX_XML = "SDMX(*.xml)|*.xml";

            /// <summary>
            /// Microsoft Access(*.mdb)|*.mdb
            /// </summary>
            public const string Database = "Microsoft Access(*.mdb)|*.mdb";

            /// <summary>
            /// Microsoft Access(*.accdb)|*.accdb
            /// </summary>
            public const string MSAccessDatabase = "Microsoft Access(*.accdb)|*.accdb";

            /// <summary>
            /// MSSQL database files(*.mdf)|*.mdf
            /// </summary>
            public const string MSSQLDatabase = "MSSQL database files(*.mdf)|*.mdf";


            /// <summary>
            /// SPSS files(*.spss)|*.spss
            /// </summary>
            public const string SPSS = "SPSS files(*.sps)|*.sps";

            /// <summary>
            /// SPO files(*.spo)|*.spo
            /// </summary>
            public const string SPO = "SPO files(*.spo)|*.spo";

            /// <summary>
            /// RTF files(*.rtf)|*.rtf
            /// </summary>
            public const string RTF = "RTF files(*.rtf)|*.rtf";

            /// <summary>
            /// Shockwave files (*.swf)|*.swf
            /// </summary>
            public const string SHOCKWAVE_FILES = "ShockWave files(*.swf)|*.swf";

            /// <summary>
            /// Wave files(*.wmv)|*.wmv
            /// </summary>
            public const string WMV_FILES = "WMV files(*.wmv)|*.wmv";

            /// <summary>
            /// AVI files(*.avi)|*.avi
            /// </summary>
            public const string AVI_FILES = "AVI files(*.avi)|*.avi";

            /// <summary>
            /// Media files(*.mp3)|*.mp3
            /// </summary>
            public const string MP3_FILES = "Media files(*.mp3)|*.mp3";

            /// <summary>
            /// Audio files(*.wma)|*.wma
            /// </summary>
            public const string MEDIA_FILES = "Audio files(*.wma)|*.wma";

            /// <summary>
            /// DX CRIS files (*.cris|*.cris)
            /// </summary>
            public const string CRIS_FILES = "CRIS files(*.cris)|*.cris";

            /// <summary>
            /// DX Cspro files (*.tbw|*.tbw)
            /// </summary>
            public const string CSPRO_FILES = "CSPRO files(*.tbw)|*.tbw";

            /// <summary>
            /// DXM export/import files
            /// </summary>
            public const string DXM_EXPIMP_FILES = "Mapping Files(*.dxm)|*.dxm";

            /// <summary>
            /// DXS export/import files
            /// </summary>
            public const string DXS_EXPIMP_FILES = "Mapping Files(*.dxs)|*.dxs";

            /// <summary>
            /// STATA_SMCL (*.smcl) files
            /// </summary>
            public const string STATA_SMCL_FILES = "STATA SMCL Files(*.smcl)|*.smcl";

            /// <summary>
            /// Google MAP(*.kmz)|*.kmz
            /// </summary>
            public const string GOOGLE_MAP_KMZ_FILES = "Google MAP(*.kmz)|*.kmz";

            /// <summary>
            /// Google MAP With Legend(*.kmz)|*.kmz
            /// </summary>
            public const string GOOGLE_MAPLEGEND_KMZ_FILES = "Google MAP With Legend(*.kmz)|*.kmz";

            /// <summary>
            /// DI data files for database,SDMX and spreadsheet
            /// </summary>
            public const string DI_DATA_FILES = "DI Data files (*.mdb;*.xml;*.xls;*.xlsx)|*.mdb;*.xml;*.xls;*.xlsx";
        }

        /// <summary>
        /// Represents constant values for file extension types.
        /// </summary>
        public class FileExtension
        {
            /// <summary>
            /// .mdb
            /// </summary>
            public const string Database = ".mdb";
            /// <summary>
            /// .accdb
            /// </summary>
            public const string MSAccessDatabase = ".accdb";

            /// <summary>
            /// .mdf
            /// </summary>
            public const string MDFDatabase = ".mdf";

            /// <summary>
            /// .tpl
            /// </summary>
            public const string Template = ".tpl";


            /// <summary>
            /// .xls
            /// </summary>
            public const string Excel = ".xls";

            /// <summary>
            /// .xlsx
            /// </summary>
            public const string Excel2007 = ".xlsx";

            /// <summary>
            /// .xml
            /// </summary>
            public const string XML = ".xml";

            /// <summary>
            /// .manifest. 
            /// </summary>
            public const string Manifest = ".manifest";

            /// <summary>
            /// .exe
            /// </summary>
            public const string Exe = ".exe";

            /// <summary>
            /// .dll
            /// </summary>
            public const string Dll = ".dll";

            /// <summary>
            /// .doc
            /// </summary>
            public const string WordApplication = ".doc";

            /// <summary>
            /// .swf
            /// </summary>
            public const string ShockWaveFiles = ".swf";

            /// <summary>
            /// .pdf
            /// </summary>
            public const string PDF = ".pdf";

            /// <summary>
            /// *.spss files
            /// </summary>
            public const string SPSS = ".sps";

            /// <summary>
            /// *.SPO files
            /// </summary>
            public const string SPO = ".spo";

            /// <summary>
            /// *.SAV files
            /// </summary>
            public const string SAV = ".sav";

            /// <summary>
            /// *.HTM files
            /// </summary>
            public const string HTM = ".htm";

            /// <summary>
            /// *.HTML files
            /// </summary>
            public const string HTML = ".html";

            /// <summary>
            /// *.MPG files
            /// </summary>
            public const string MPG = ".mpg";

            /// <summary>
            /// *.WMV files
            /// </summary>
            public const string WMV = ".wmv";

            /// <summary>
            /// *.AVI files
            /// </summary>
            public const string AVI = ".avi";

            /// <summary>
            /// *.dxs files
            /// </summary>
            public const string DXS = ".dxs";

            /// <summary>
            /// *.dxm files
            /// </summary>
            public const string DXM = ".dxm";

            /// <summary>
            /// SMCL file extension
            /// </summary>
            public const string SMCL = ".smcl";

            /// <summary>
            /// *.CRIS files 
            /// </summary>
            public const string CIRS = ".cris";

            /// <summary>
            /// *.KML files (google earth)
            /// </summary>
            public const string KML = ".kml";

            /// <summary>
            /// *.KMZ files (google earth)
            /// </summary>
            public const string KMZ = ".kmz";

            /// <summary>
            /// *.tbw files (CSPRO table view )
            /// </summary>
            public const string CSPRO = ".tbw";

            /// <summary>
            /// *.JPG files 
            /// </summary>
            public const string JPEG = ".jpg";

        }

        /// <summary>
        /// Represents constant values for Country ISO Code Filenames.
        /// </summary>
        public class Others
        {
            /// <summary>
            /// Gets the name of file containing  ISO codes for the Countries and Default Language Code
            /// </summary>
            public static string CountryISOCodeFilename = "Country_ISO_Codes.xml";              //-- ISO Code filename 


            /// <summary>
            /// Default Langauge for the File
            /// </summary>
            public const string DefaultLanguageCode = "en";                         //--Default LangaugeCode

            /// <summary>
            /// Gallery database name.
            /// </summary>
            public const string GalleryDatabase = "GalleryPresentation.mdb";


        }

        /// <summary>
        /// Represents default folders path.
        /// </summary>
        public class DefaultFolder
        {

            ///
            public static string DefaultDevInfo6Folder = @"C:\DevInfo\DevInfo 6.0";

            /// <summary>
            /// Gets the Default Data Folder
            /// </summary>
            public static string DefaultDataFolder = string.Empty;

            /// <summary>
            /// Gets the Ecxchange Default Folder
            /// </summary>
            public static string DefaultExchangeTempFolder = Applicationpath + @"\bin\temp\";

            /// <summary>
            ///  Gets the default folder for spreadsheets
            /// </summary>
            public static string DefaultSpreadSheetsFolder = string.Empty;


            /// <summary>
            /// Gets the Default Langauge Folder
            /// </summary>
            public static string DefaultLanguageFolder = DataAdmin.DAApplicationPath + @"\Language\";

            /// <summary>
            /// Gets the Default presentation folder
            /// </summary>
            public static string DefaultPresentationFolder = DataAdmin.DAApplicationPath + @"\Gallery\Presentations\";


            /// <summary>
            /// Gets the default map folder
            /// </summary>
            public static string DefaultMapFolder = string.Empty;

            /// <summary>
            /// Gets  default template folder of data exchange 
            /// </summary>
            public static string DefaultExchangeTemplateFolder = string.Empty;

            /// <summary>
            /// Gets the ISOFolder Path
            /// </summary>
            public static string DefaultISOFldrpath = string.Empty;                          //- ISO Code file location



            private static string _DefaultDXIconFolder = DICommon.Applicationpath + @"\bin\ICONS\";
            /// <summary>
            /// Gets DX Icons folder path
            /// </summary>
            public static string DefaultDXIconFolder
            {
                get
                {
                    DefaultFolder._DefaultDXIconFolder = DICommon.Applicationpath + @"\bin\ICONS\";
                    return DefaultFolder._DefaultDXIconFolder;
                }
                set
                {
                    DefaultFolder._DefaultDXIconFolder = value;
                }
            }

            /// <summary>
            /// Gets DA Icons folder path
            /// </summary>



            private static string _DefaultDAIconFolder = DataAdmin.DAApplicationPath + @"\Shortcut Icon\";
            /// <summary>
            /// Gets DA Icons folder path
            /// </summary>            
            public static string DefaultDAIconFolder
            {
                get
                {
                    DefaultFolder._DefaultDAIconFolder = DataAdmin.DAApplicationPath + @"\Shortcut Icon\";
                    return DefaultFolder._DefaultDAIconFolder; ;
                }
                set
                {
                    DefaultFolder._DefaultDAIconFolder = value;
                }
            }


            public static string DefaultDAEmergencyInfoFolder = DataAdmin.DAApplicationPath + @"\EmergencyInfo\";
            public static string DefaultDAExchangeFolder = DataAdmin.DAApplicationPath + @"\Exchange\";
            public static string DefaultDAStandardsFolder = DataAdmin.DAApplicationPath + @"\Standards\";

            /// <summary>
            /// Disputed international Boundaries Folder
            /// </summary>
            public static string DIBMapFolder = @"Stock\Map\DIB\";

            /// <summary>
            /// Mapping file for Import process
            /// </summary>
            public static string DefaultImportMappingFolder = DataAdmin.DAApplicationPath + @"\Bin\Stock\ImportMapping\";

        }

        public static bool ISColumnExistInTable(DIConnection DBConnection, string columnName, string tableName)
        {
            bool RetVal = true;
            string SqlQuery = string.Empty;

            SqlQuery = "SELECT " + columnName + " FROM " + tableName;

            try
            {
                DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        #endregion

        #endregion
    }
}
