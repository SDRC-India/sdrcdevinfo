using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap
{

    #region " -- Delegates --"

    /// <summary>
    ///  Declare the delegate to update the theme and legends.
    /// </summary>
    /// <param name="ThemeIndex">ThemeIndex</param>    
    public delegate void SetThemeDelegate(int ThemeIndex);

    /// <summary>
    /// Declare the delegate to update break count of the theme and recreate the legends.
    /// </summary>
    /// <param name="ThemeIndex">ThemeIndex</param>
    /// <param name="StartColor">StartColor</param>
    /// <param name="EndColor">EndColor</param>
    public delegate void SetBreakCountDelegate(int ThemeIndex,Color StartColor, Color EndColor);

    /// <summary>
    /// Declare the delegate to update the legends according to "High is good"
    /// </summary>
    /// <param name="ThemeIndex">ThemeIndex</param>
    public delegate void SetHighIsGoodDelegate(int ThemeIndex);
 
    #endregion   

    /// <summary>
    /// Class contain the properties of theme, and the methods to prepare the legends
    /// </summary>
    public class Theme
    {
        #region " -- Private -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Private default constructor is needed to serailize and deserialize the class
        /// </summary>
        private Theme()
        {
        }

        #endregion

        #region " -- Variable -- "

        private Color[] LegendColor=new Color[4];
        private int ThemeIndex = 0;

        #endregion

        #region " -- Methods -- "     
        
        #endregion

        #region " -- Raise Event -- "

        /// <summary>
        /// Raise the event to set the themes and update the legends
        /// </summary>
        /// <param name="themeIndex"></param>        
        private void RaiseSetThemeEvent(int themeIndex)
        {
            if (this.SetThemeEvent != null)
            {
                this.SetThemeEvent(themeIndex);
            }
        }

        /// <summary>
        /// Raise the event to recreate all the legends
        /// </summary>
        /// <param name="themeIndex">themeIndex</param>
        /// <param name="startColor">startColor</param>
        /// <param name="endColor">endColor</param>
        private void RaiseSetBreakCountEvent(int themeIndex, Color startColor, Color endColor)
        {
            if (this.SetBreakCountEvent != null)
            {
                this.SetBreakCountEvent(themeIndex,startColor,endColor);
            }
        }

        /// <summary>
        /// R%aise the event to set the order of legends according to "High is good"
        /// </summary>
        /// <param name="themeIndex"></param>
        public void RaiseSetHighIsGoodEvent(int themeIndex)
        {
            if (this.SetHighIsGoodEvent != null)
            {
                this.SetHighIsGoodEvent(themeIndex);
            }
        }
      

        #endregion

        #endregion

        #region " -- Public / Friend -- "

        #region " -- Variables  -- "

        // -- internal as it will also be used by legend class
        internal DataView PresentationData;

        // -- This will used to set the color, when hosting application change the color of a legend.         
        internal Themes Themes;	

        #endregion

        #region " -- New / Dispose -- "

        public Theme(string indicator, string id, decimal min, decimal max, int decimalPoint, DataView presentationData, Color[] legendColor, Themes themes, int themeIndex, bool isFreequenctTableTheme)
        {
            this._Indicator = indicator;
            this._IndicatorNId = id;
            string FormatString = this.FormatNumber(decimalPoint);
            this._Minimum = Convert.ToDecimal(min.ToString(FormatString));
            this._Maximum = Convert.ToDecimal(max.ToString(FormatString));
            this._Decimal = decimalPoint;
            this.LegendColor = legendColor;
            this.PresentationData = presentationData;
            this.ThemeIndex = themeIndex;
            this._IsFreequenctTableTheme = isFreequenctTableTheme;

            if (this.Themes == null)
            {
                this.Themes = new Themes();
            }
            this.Themes = themes;

            if (isFreequenctTableTheme)
            {
                this._BreakType = BreakTypes.EqualSize;

                // -- set the default legends for frequency table
                this.EqualSize(this.PresentationData);
            }
            else
            {
                // -- set the default legends 
                this.EqualCount(this.PresentationData);
            }
        }

        #endregion

        #region " -- Properties -- "

        private string _Indicator=string.Empty;
        /// <summary>
        /// Get or sets the indicator
        /// </summary>
        public string Indicator
        {
            get
            {
                return this._Indicator;
            }

            set 
            {
                this._Indicator= value; 
            }
        }

        private string _IndicatorNId;
        /// <summary>
        /// Gets or sets the indicator ID
        /// </summary>
        public string IndicatorNId
        {
            get
            {
                return this._IndicatorNId; 
            }
            set
            {
                this._IndicatorNId = value; 
            }
        }
	

        private int _BreakCount=4;
        /// <summary>
        /// Gets or sets the Break count
        /// </summary>
        /// <remarks>By default, Table wizard uses 4 break count.</remarks>
        public int BreakCount
        {
            get
            {
                return this._BreakCount; 
            }
            set
            {
                Color FirstColor = Color.Blue;
                Color LastColor = Color.DarkBlue;
                if (this._Legends[0] != null)
                {
                    // -- if the legend instance is not null
                    // -- Store the first & last color to get the color shade
                    FirstColor = this._Legends[0].Color;
                    LastColor = this.Legends[this._BreakCount - 1].Color;
                }
                else
                {
                    FirstColor = Color.Blue;
                    LastColor = Color.DarkBlue;
                }
                
                this._BreakCount= value;
                
                this.RaiseSetBreakCountEvent(this.ThemeIndex, FirstColor, LastColor);
            }
        }

        private BreakTypes _BreakType=BreakTypes.Continuous;
        /// <summary>
        /// Get or sets the break type
        /// </summary>
        /// <remarks> </remarks>
        public BreakTypes BreakType
        {
            get
            {
                return this._BreakType; 
            }
            set
            {
                this._BreakType = value;
                this.RaiseSetThemeEvent(this.ThemeIndex);
            }
        }

        private decimal _Minimum;
        /// <summary>
        /// gets or sets the minimum value
        /// <para>Hosting application will display this value according to the regional setting</para>
        /// </summary>
        public decimal Minimum
        {
            get
            {
                return this._Minimum; 
            }
            set
            {
                this._Minimum = value;
                this.RaiseSetThemeEvent(this.ThemeIndex);
            }
        }

        private decimal _Maximum;
        /// <summary>
        /// gets or sets the maximum value
        /// <para>Hosting application will display this value according to the regional setting</para>
        /// </summary>
        public decimal Maximum
        {
            get
            {
                return this._Maximum;
            }
            set
            {
                this._Maximum = value;
                this.RaiseSetThemeEvent(this.ThemeIndex);
            }
        }

        private int _Decimal=0;
        /// <summary>
        /// Gets or sets the decimal
        /// </summary>
        public int Decimal
        {
            get 
            {
                return this._Decimal; 
            }
            set
            {
                this._Decimal = value;
                this.RaiseSetThemeEvent(this.ThemeIndex);
            }
        }

        private bool _HighIsGood=true;
        /// <summary>
        /// Gets or sets the high is good
        /// </summary>
        /// <remarks>
        /// Applicable only for table presentation
        /// <para>For few indicators high data value might be good (Literacy Rate) and for others high value may be bad (IMR)</para>
        /// <para>By default High is good is set to true. When user toogles its value, the order of legend caption and color are reversed</para>
        /// </remarks>
        public bool HighIsGood
        {
            get
            {
                return this._HighIsGood; 
            }
            set
            {
                bool OldHighIsGood = this._HighIsGood;
                this._HighIsGood = value;
                //If there is change in value
                if (OldHighIsGood != this._HighIsGood)
                {
                    this.RaiseSetHighIsGoodEvent(this.ThemeIndex);
                }
            }
        }

        private Legends _Legends;
        /// <summary>
        /// Gets or sets the Legend
        /// </summary>        
        public Legends Legends
        {
            get
            {
                if (this._Legends == null)
                {
                    this._Legends = new Legends(this);
                }
                return _Legends; 
            }
            set
            {                
                this._Legends = value;
            }
        }

        private bool _IsFreequenctTableTheme = false;
        /// <summary>
        /// Gets or sets the IsFreequenctTableTheme
        /// </summary>
        public bool IsFreequenctTableTheme
        {
            get 
            {
                return this._IsFreequenctTableTheme; 
            }
            set 
            {
                this._IsFreequenctTableTheme = value; 
            }
        }
	

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Set the legends on the basis of Equal Count break type
        /// </summary>
        /// <param name="presentationData">Data view</param>
        internal void EqualCount(DataView presentationData)
        {
            try
            {
                if (this._IsFreequenctTableTheme)
                {
                    // -- filter the dataview according to the IUS.
                    presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + this.IndicatorNId + "";
                }
                else
                {
                    // -- filter the dataview according to the indicator.            
                    presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId + " = " + this.IndicatorNId + "";
                }

                // -- Create the data table to store Indicator Nid, Name and Data value
                DataTable RangeDataTable = new DataTable();
                // -- Get the valid numeric value
                RangeDataTable = this.ValidNumericValue(presentationData, this.Decimal);

                // Redeclare the color array, in case of increase in breakcount
                if (this.LegendColor.Length < this._BreakCount)
                {
                    Color[] TempColor = new Color[this._BreakCount];
                    Array.Copy(this.LegendColor, TempColor, this.LegendColor.Length - 1);
                    this.LegendColor = new Color[this._BreakCount];
                    Array.Copy(TempColor, this.LegendColor, this._BreakCount);
                }

                DataTable LegendDv = new DataTable();

                LegendDv = DICommon.GenerateEqualCount(RangeDataTable.DefaultView, this.BreakCount, this.Minimum, this.Maximum, this.Decimal);
                int Index = 0;
                foreach (DataRow LegendRow in LegendDv.Rows)
                {
                    this.Legends.Add(new Legend("Label " + (Index + 1).ToString(), Convert.ToDecimal(LegendRow[0]), Convert.ToDecimal(LegendRow[1]), Convert.ToInt32(LegendRow[2]), this.LegendColor[Index], Index));
                    Index += 1;
                }
                presentationData.RowFilter = string.Empty;
            }
            catch (Exception ex)
            {
                presentationData.RowFilter = string.Empty;
            }

            //decimal IntervalGap = 0;
            //decimal OptimalRangeCount = 0;            
            //string FormatString = string.Empty;
            //string Caption = "Label";
            //string[] LegendColor = new string[this.BreakCount];

            //// -- filter the dataview according to the indicator.            
            //presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId + " = " + this.IndicatorNId + "";

            //// -- Create the data table to store Indicator Nid, Name and Data value
            //DataTable RangeDataTable = new DataTable();
            //// -- Get the valid numeric value
            //RangeDataTable = this.ValidNumericValue(presentationData, this.Decimal);

            //DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

            //// -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
            //IntervalGap = this.SetIntervalGap();

            //System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //// -- Valid range value exists in the dataview            
            //NumericIndicatorDataView.RowFilter = Data.DataValue + ">=" + Convert.ToDouble(this.Minimum.ToString(FormatString)) + " AND " + Data.DataValue + "<=" + Convert.ToDouble(this.Maximum.ToString(FormatString));

            //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;


            //// -- Set the count on each interval.
            //OptimalRangeCount = NumericIndicatorDataView.Count / this.BreakCount;
            
            //if (NumericIndicatorDataView.Count > 0 && OptimalRangeCount < 1)
            //{
            //    // -- if Equal count is less then 1 
            //    OptimalRangeCount = 1;
            //}

            //// Handle textual sort 
            //NumericIndicatorDataView.Sort = DI_LibDAL.Queries.DIColumns.Data.DataValue + " Asc";
            //// -- Create the Format string on the basis of Decimal places
            //FormatString = this.FormatNumber(this._Decimal);

            //// Redeclare the color array, in case of increase in breakcount
            //if (this.LegendColor.Length < this._BreakCount)
            //{
            //    Color[] TempColor = new Color[this._BreakCount];
            //    Array.Copy(this.LegendColor, TempColor, this.LegendColor.Length - 1);                
            //    this.LegendColor = new Color[this._BreakCount];
            //    Array.Copy(TempColor, this.LegendColor, this._BreakCount);
            //}

            //int Index = 0;
            //int DataValueCount = 1;
            //string MinValue = this.Minimum.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator);
            //decimal FromRange = Convert.ToDecimal(this.Minimum.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator));
            //decimal ToRange = 0;
            //int Count = Convert.ToInt32(OptimalRangeCount);
            //int LegendCount = 0;
            //int ItemMoved = 0;            

            //// -- Loop to build the legends agsinst the indicator
            //for (Index = 0; Index <= NumericIndicatorDataView.Count - 1; Index++)
            //{
            //    while (DataValueCount <= this.BreakCount)
            //    {
            //        LegendCount++;

            //        if (LegendCount - 1 < this.LegendColor.Length)
            //        {
            //            // -- if any color is send by the hosting application
            //            if (this.LegendColor[LegendCount - 1] != null)
            //            {
            //                // -- if the color is passed by the hosting applocation for the legend
            //                this.Legends.Add(new Legend(Caption + " " + LegendCount.ToString(), 0, 0, 0, this.LegendColor[LegendCount - 1], LegendCount - 1));
            //            }
            //            else
            //            {
            //                // -- if the null color is passed by the hosting applocation for the legend
            //                this.Legends.Add(new Legend(Caption + " " + LegendCount.ToString(), 0, 0, 0, Color.Gray, LegendCount - 1));
            //            }
            //        }
            //        else
            //        {
            //            // -- if the color is not passed by the hosting applocation for the legend
            //            this.Legends.Add(new Legend(Caption + " " + LegendCount.ToString(), 0, 0, 0, Color.Gray, LegendCount - 1));
            //        }


            //        if (string.IsNullOrEmpty(MinValue))
            //        {
            //            // -- Set the from range, if it is 0
            //            FromRange = this.Legends[LegendCount - 2].RangeTo + IntervalGap;
            //            if (FromRange > this.Maximum)
            //            {
            //                // -- if the FromRange has value greater then the mxaimum value, set it with max value
            //                // -- No DataValue is against that range
            //                FromRange = this.Maximum;
            //            }
            //            MinValue = FromRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator);
            //        }

            //        // -- Set the Default legends
            //        this.Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //        this.Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(FromRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //        this.Legends[LegendCount - 1].Count = 0;                    

            //        while (Index < (Math.Round(OptimalRangeCount * DataValueCount)) + ItemMoved)
            //        {
            //            if (Index <= NumericIndicatorDataView.Count - 1)
            //            {
            //                // -- set the FromRange, ToRange and Count of the legend
            //                this.Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //                ToRange = Convert.ToDecimal(NumericIndicatorDataView[Index][DI_LibDAL.Queries.DIColumns.Data.DataValue]);
            //                this.Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //                this.Legends[LegendCount - 1].Count = this.Legends[LegendCount - 1].Count + 1;                                                       
            //            }
            //            Index += 1;                        
            //        }
            //        if (Index < NumericIndicatorDataView.Count && this.Legends[LegendCount - 1].RangeTo != 0)
            //        {
            //            // -- Check next rows data for similar digits                        
            //            while (Convert.ToDecimal(NumericIndicatorDataView[Index][DI_LibDAL.Queries.DIColumns.Data.DataValue]) == this.Legends[LegendCount - 1].RangeTo)
            //            {
            //                // -- if the DataValue is same as the last equal count DataValue
            //                // -- this will add in the current legend
            //                this.Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //                ToRange = Convert.ToDecimal(NumericIndicatorDataView[Index][DI_LibDAL.Queries.DIColumns.Data.DataValue]);
            //                this.Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString).Replace(".", DICommon.NumberDecimalSeparator)));
            //                this.Legends[LegendCount - 1].Count = this.Legends[LegendCount - 1].Count + 1;                            

            //                Index += 1;
            //                if (Index > NumericIndicatorDataView.Count - 1)
            //                {
            //                    // -- if no more Indicator left in the DataView
            //                    break;
            //                }
            //                ItemMoved += 1;
            //            }                      
            //        }
            //        MinValue = string.Empty;
            //        DataValueCount += 1;
            //    }             
            //}            
            //if (this.Legends[this.BreakCount - 1].RangeTo < this.Maximum)
            //{
            //    System.Globalization.CultureInfo oldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //    NumericIndicatorDataView.RowFilter = Data.DataValue + ">" + this.Legends[this.BreakCount - 1].RangeTo;

            //    System.Threading.Thread.CurrentThread.CurrentCulture = oldCulture;

            //    this.Legends[this.BreakCount - 1].Count = this.Legends[this.BreakCount - 1].Count + NumericIndicatorDataView.Count;
            //    this.Legends[this.BreakCount - 1].SetTovalue(this.Maximum);
            //}
           
        }

        /// <summary>
        /// Set the legends on the basis of Equalsize break type
        /// </summary>
        /// <param name="presentationData">Data View</param>
        internal void EqualSize(DataView presentationData)
        {
            if (this._IsFreequenctTableTheme)
            {
                // -- filter the dataview according to the IUS.
                presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId + " = " + this.IndicatorNId + "";
            }
            else
            {
                // -- filter the dataview according to the indicator.            
                presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId + " = " + this.IndicatorNId + "";
            }

            // -- Create the data table to store Indicator Nid, Name and Data value
            DataTable RangeDataTable = new DataTable();
            // -- Get the valid numeric value
            RangeDataTable = this.ValidNumericValue(presentationData, this.Decimal);

            // Redeclare the color array, in case of increase in breakcount
            if (this.LegendColor.Length < this._BreakCount)
            {
                Color[] TempColor = new Color[this._BreakCount];
                Array.Copy(this.LegendColor, TempColor, this.LegendColor.Length - 1);
                this.LegendColor = new Color[this._BreakCount];
                Array.Copy(TempColor, this.LegendColor, this._BreakCount);
            }

            DataTable LegendDv = new DataTable();

            LegendDv = DICommon.GenerateEqualSize(RangeDataTable.DefaultView, this.BreakCount, this.Minimum, this.Maximum, this.Decimal);
            int Index = 0;
            foreach (DataRow LegendRow in LegendDv.Rows)
            {
                this.Legends.Add(new Legend("Label " + (Index + 1).ToString(), Convert.ToDecimal(LegendRow[0]), Convert.ToDecimal(LegendRow[1]), Convert.ToInt32(LegendRow[2]), this.LegendColor[Index], Index));
                Index += 1;
            }

            presentationData.RowFilter = string.Empty;
        //    decimal IntervalGap = 0;
        //    decimal EqualSizeInterval = 0;
        //    string FormatString = string.Empty;
        //    string Caption = "Label";
        //    string[] LegendColor = new string[this.BreakCount];

        //    // -- filter the dataview according to the indicator.

        //    presentationData.RowFilter = " " + DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId + " = " + this.IndicatorNId + "";

        //    // -- Create the data table to store Indicator Nid, Name and Data value
        //    DataTable RangeDataTable = new DataTable();
        //    // -- Get the valid numeric value
        //    RangeDataTable = this.ValidNumericValue(presentationData, this.Decimal);

        //    DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

        //    // -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
        //    IntervalGap = this.SetIntervalGap();

        //    // -- Set the count on each interval.
        //    EqualSizeInterval = Math.Round((((this.Maximum - this.Minimum) / this.BreakCount)), this.Decimal);

        //    // -- sort the data table on the basis of data value
        //    NumericIndicatorDataView.Sort = DI_LibDAL.Queries.DIColumns.Data.DataValue + " Asc";

        //    // -- Create the Format string on the basis of Decimal places
        //    FormatString = this.FormatNumber(this._Decimal);

        //    // Redeclare the color array, in case of increase in breakcount
        //    if (this.LegendColor.Length < this._BreakCount)
        //    {
        //        Color[] TempColor = new Color[this._BreakCount];
        //        Array.Copy(this.LegendColor, TempColor, this.LegendColor.Length - 1);
        //        this.LegendColor = new Color[this._BreakCount];
        //        Array.Copy(TempColor, this.LegendColor, this._BreakCount);
        //    }

        //    int DataValueCount = 0;
        //    decimal FromRange = Convert.ToDecimal(this.Minimum.ToString(FormatString));
        //    decimal ToRange = 0;
        //    int Count = 0;           

        //    for (DataValueCount = 1; DataValueCount <= this.BreakCount; DataValueCount++)
        //    {
        //        if (DataValueCount == 1)
        //        {
        //            // -- For the first legend
        //            FromRange = this.Minimum;
        //            ToRange = FromRange + EqualSizeInterval ;
        //        }
        //        else
        //        {
        //            // -- For the rest of the legends.
        //            FromRange = FromRange + EqualSizeInterval + IntervalGap;
        //            if (DataValueCount < this.BreakCount)
        //            {
        //                // -- Set the ToRange but not for the last legend
        //                ToRange = FromRange + EqualSizeInterval;
        //            }
        //            else
        //            {
        //                // -- Set the ToRange for the last legend.
        //                ToRange = this.Maximum;
        //            }
        //        }
        //        // -- set the FromRange and ToRange to Maximum value, if it is greater then the max value
        //        if (FromRange > this.Maximum)
        //        {
        //            FromRange = this.Maximum;
        //        }
        //        if (ToRange > this.Maximum)
        //        {
        //            ToRange = this.Maximum;
        //        }

        //        // -- Set the Count value
        //        foreach (DataRowView Row in NumericIndicatorDataView)
        //        {                    
        //            if (Convert.ToDecimal(Row[DI_LibDAL.Queries.DIColumns.Data.DataValue]) >= FromRange && Convert.ToDecimal(Row[DI_LibDAL.Queries.DIColumns.Data.DataValue]) <= ToRange)
        //            {
        //                // -- DataValue exist between the from range and to range
        //                Count += 1;
        //            }
        //        }
        //        // -- set the legend
        //        if (DataValueCount < LegendColor.Length)
        //        {
        //            if (this.LegendColor[DataValueCount - 1] != null)
        //            {
        //                // -- if the color is passed by the hosting applocation for the legend
        //                this.Legends.Add(new Legend((Caption + " " + DataValueCount).ToString(), Convert.ToDecimal(FromRange.ToString(FormatString)), Convert.ToDecimal(ToRange.ToString(FormatString)), Count, this.LegendColor[DataValueCount - 1], DataValueCount - 1));
        //            }
        //            else
        //            {
        //                // -- if the null color is passed by the hosting applocation for the legend
        //                this.Legends.Add(new Legend((Caption + " " + DataValueCount).ToString(), Convert.ToDecimal(FromRange.ToString(FormatString)), Convert.ToDecimal(ToRange.ToString(FormatString)), Count, Color.Gray, DataValueCount - 1));
        //            }
        //        }                
        //        else
        //        {
        //            // -- if the color is not passed by the hosting applocation for the legend
        //            this.Legends.Add(new Legend((Caption + " " + DataValueCount).ToString(), Convert.ToDecimal(FromRange.ToString(FormatString)), Convert.ToDecimal(ToRange.ToString(FormatString)), Count, Color.Gray, DataValueCount - 1));
        //        }
        //        Count = 0;
        //    }
        //    presentationData.RowFilter = string.Empty;
        }      

        /// <summary>
        /// Get those records from data view which contains numeric DataValue
        /// </summary>
        /// <param name="themeData">theme Data</param>
        /// <returns></returns>
        internal DataTable ValidNumericValue(DataView themeData,int decimalPoint)
        {
            DataTable RetVal = new DataTable();
            try
            {
                string FormatString = this.FormatNumber(decimalPoint);
                
                RetVal.Columns.Add(DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId, typeof(String));
                RetVal.Columns.Add(DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName,typeof(String));                
                RetVal.Columns.Add(DI_LibDAL.Queries.DIColumns.DataExpressionColumns.NumericData,typeof(Double));
                

                DataView DV = new DataView();

                // -- Store the valid numeric value on to the Data set
                foreach (DataRowView UserRowView in themeData)
                {

                    if (DICommon.IsNumeric(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString())))
                    {
                        // -- If the data value is numeric.
                        DataRow Row;
                        Row = RetVal.NewRow();
                        Row[0] = this.IndicatorNId;
                        Row[1] = this.Indicator;
                        double DataValue = Convert.ToDouble(UserRowView[DataExpressionColumns.NumericData].ToString().Replace(".", DICommon.NumberDecimalSeparator));
                        Row[2] = DataValue.ToString(FormatString);
                        RetVal.Rows.Add(Row);
                    }
                }

                RetVal.AcceptChanges();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Set the Interval Gap between ranges
        /// </summary>
        /// <returns></returns>
        internal decimal SetIntervalGap()
        {
            decimal RetVal;
            try
            {
                // -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
                if (this._Decimal > 0)
                {
                    RetVal =Convert.ToDecimal(1 / Math.Pow(10, this._Decimal));
                }
                else
                {
                    // -- if Decimal value=0
                    // it will set the next legend interval to 1.
                    RetVal = 1;
                }
            }
            catch (Exception)
            {
                RetVal = 0;

            }
            return RetVal;
        }

        /// <summary>
        /// Prepare the string on the basis of decimal places to format the number 
        /// </summary>
        /// <returns>String</returns>
        internal string FormatNumber(int decimalCount)
        {            
            string RetVal = "0.";
            try
            {
                for (int i = 0; i < decimalCount; i++)
                {
                    RetVal = RetVal + "0";
                }
            }
            catch (Exception)
            {
                RetVal = "0";
            }
            return RetVal;
        }

        /// <summary>
        /// Set the Break Count
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="breakCountValue"></param>
        internal void SetBreakCount(int breakCountValue)
        {
            this._BreakCount = breakCountValue;
        }

        /// <summary>
        /// Set the High is Good
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="breakCountValue"></param>
        internal void SetHighIsGood(bool value)
        {
            this._HighIsGood = value;
        }

        /// <summary>
        /// Set the maximum value
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="breakCountValue"></param>
        internal void SetMaximum(decimal value)
        {
            this._Maximum = value;
        }

        /// <summary>
        /// Set the minimum value
        /// <para>This method is used to avoid the raise event to successive update </para>
        /// </summary>
        /// <param name="breakCountValue"></param>
        internal void SetMinimum(decimal value)
        {
            this._Minimum = value;
        }

        public void SmoothRangeColor()
        {
            string[] Retval = new string[0];
            Retval = this.Themes.BuildRangeColor(this.Themes[this.ThemeIndex].Legends[0].Color, this.Themes[this.ThemeIndex].Legends[this.Themes[this.ThemeIndex].BreakCount - 1].Color, this.ThemeIndex);
            int Index = 0;
            foreach (Legend Legend in this.Legends)
            {
                Legend.Color = ColorTranslator.FromHtml(Retval[Index]);
                Index += 1;
            }
        }  

        #endregion

        #region " -- Events -- "

        /// <summary>
        /// Declare the event to update the themes and legends.
        /// </summary>
        public event SetThemeDelegate SetThemeEvent;

        /// <summary>
        /// Declare the event to update the breakcount & recreates the legends.
        /// </summary>
        public event SetBreakCountDelegate SetBreakCountEvent;

        /// <summary>
        /// Declare the event to set the order of legends according to "High is good".
        /// </summary>
        public event SetHighIsGoodDelegate SetHighIsGoodEvent;

        #endregion

        #endregion

    }
}

