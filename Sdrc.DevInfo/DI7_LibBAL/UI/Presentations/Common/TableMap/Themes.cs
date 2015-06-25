using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap
{
    /// <summary>
    /// Class contains the theme collections
    /// </summary>
    public class Themes : System.Collections.CollectionBase,ICloneable
    {
        #region " -- Private -- "

        #region " -- Methods -- "

        /// <summary>
        /// update the legends on the basis of Equal Count break type
        /// </summary>
        /// <param name="presentationData">Data view</param>
        private void UpdateEqualCountLegends(DataView presentationData,int themeIndex)
        {
            if (this[themeIndex].IsFreequenctTableTheme)
            {
                // -- filter the dataview according to the IUS.
                presentationData.RowFilter = " " + Indicator_Unit_Subgroup.IUSNId + " = " + this[themeIndex].IndicatorNId + "";
            }
            else
            {
                // -- filter the dataview according to the indicator.            
                presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this[themeIndex].IndicatorNId + "";
            }

            // -- Create the data table to store Indicator Nid, Name and Data value
            DataTable RangeDataTable = new DataTable();
            // -- Get the valid numeric value
            RangeDataTable = this[themeIndex].ValidNumericValue(presentationData, this[themeIndex].Decimal);

            DataTable LegendDv = new DataTable();

            LegendDv = DICommon.GenerateEqualCount(RangeDataTable.DefaultView, this[themeIndex].BreakCount, this[themeIndex].Minimum, this[themeIndex].Maximum, this[themeIndex].Decimal);
            int Index = 0;
            foreach (DataRow LegendRow in LegendDv.Rows)
            {
                this[themeIndex].Legends[Index].Range = LegendRow[0].ToString() + " - " + LegendRow[1].ToString();
                this[themeIndex].Legends[Index].SetFromvalue(Convert.ToDecimal(LegendRow[0]));
                this[themeIndex].Legends[Index].SetTovalue(Convert.ToDecimal(LegendRow[1]));
                this[themeIndex].Legends[Index].Count = Convert.ToInt32(LegendRow[2]);
                Index += 1;
            }

            presentationData.RowFilter = string.Empty;

            //decimal IntervalGap = 0;
            //decimal OptimalRangeCount = 0;
            //string FormatString = string.Empty;

            //// -- filter the dataview according to the indicator.            
            //presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this[themeIndex].IndicatorNId + "";

            //// -- Create the data table to store Indicator Nid, Name and Data value
            //DataTable RangeDataTable = new DataTable();
            //// -- Get the valid numeric value
            //RangeDataTable = this[themeIndex].ValidNumericValue(presentationData, this[themeIndex].Decimal);

            //DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

            //// -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
            //IntervalGap = this[themeIndex].SetIntervalGap();

            //System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //// -- Valid range value exists in the dataview
            ////NumericIndicatorDataView.RowFilter = Data.DataValue + ">=" + Convert.ToDouble(DICommon.SetDecimalSperator(this[themeIndex].Minimum.ToString(FormatString))) + " AND " + Data.DataValue + "<=" + Convert.ToDouble(DICommon.SetDecimalSperator(this[themeIndex].Maximum.ToString(FormatString)));
            //NumericIndicatorDataView.RowFilter = Data.DataValue + ">=" + Convert.ToDouble(this[themeIndex].Minimum.ToString(FormatString)) + " AND " + Data.DataValue + "<=" + Convert.ToDouble(this[themeIndex].Maximum.ToString(FormatString));

            //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

            //// -- Set the count on each interval.
            //OptimalRangeCount = NumericIndicatorDataView.Count / this[themeIndex].BreakCount;

            //if (NumericIndicatorDataView.Count > 0 && OptimalRangeCount < 1)
            //{
            //    // -- if Equal count is less then 1 
            //    OptimalRangeCount = 1;
            //}

            //// Handle textual sort 
            //NumericIndicatorDataView.Sort = Data.DataValue + " Asc";
            //// -- Create the Format string on the basis of Decimal places
            //FormatString = this[themeIndex].FormatNumber(this[themeIndex].Decimal);

            //int Index = 0;
            //int DataValueCount = 1;
            //string MinValue = this[themeIndex].Minimum.ToString(FormatString);
            //decimal FromRange = Convert.ToDecimal(this[themeIndex].Minimum.ToString(FormatString));
            //decimal ToRange = 0;
            //int Count = Convert.ToInt32(OptimalRangeCount);
            //int LegendCount = 0;
            //int ItemMoved = 0;

            //// -- Loop to build the legends agsinst the indicator
            //for (Index = 0; Index <= NumericIndicatorDataView.Count - 1; Index++)
            //{
            //    while (DataValueCount <= this[themeIndex].BreakCount)
            //    {
            //        LegendCount++;
                    
            //        if (string.IsNullOrEmpty(MinValue))
            //        {                        
            //            // -- Set the from range, if it is 0
            //            FromRange = this[themeIndex].Legends[LegendCount - 2].RangeTo + IntervalGap;
            //            if (FromRange > this[themeIndex].Maximum)
            //            {
            //                // -- if the FromRange has value greater then the mxaimum value, set it with max value
            //                // -- No DataValue is against that range
            //                FromRange = this[themeIndex].Maximum;
            //            }
            //            MinValue = FromRange.ToString(FormatString);
            //        }

            //        // -- Set the Default legends                    
            //        this[themeIndex].Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //        this[themeIndex].Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //        this[themeIndex].Legends[LegendCount - 1].Count = 0;

            //        while (Index < (Math.Round(OptimalRangeCount * DataValueCount)) + ItemMoved)
            //        {
            //            if (Index <= NumericIndicatorDataView.Count - 1)
            //            {
            //                // -- set the FromRange, ToRange and Count of the legend
            //                this[themeIndex].Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //                ToRange = Convert.ToDecimal(NumericIndicatorDataView[Index][Data.DataValue]);
            //                this[themeIndex].Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString)));
            //                this[themeIndex].Legends[LegendCount - 1].Count = this[themeIndex].Legends[LegendCount - 1].Count + 1;
            //            }
            //            Index += 1;
            //        }

            //        if (this[themeIndex].Legends[LegendCount - 1].RangeTo < this[themeIndex].Legends[LegendCount - 1].RangeFrom)
            //        {
            //            this[themeIndex].Legends[LegendCount - 1].SetTovalue(this[themeIndex].Legends[LegendCount - 1].RangeFrom);
            //        }

            //        if (Index < NumericIndicatorDataView.Count && !string.IsNullOrEmpty(this[themeIndex].Legends[LegendCount - 1].RangeTo.ToString()))
            //        {
            //            // -- Check next rows data for similar digits                        
            //            while (Convert.ToDouble(NumericIndicatorDataView[Index][DI_LibDAL.Queries.DIColumns.Data.DataValue]) == Convert.ToDouble(this[themeIndex].Legends[LegendCount - 1].RangeTo.ToString(FormatString)))
            //            {
            //                // -- if the DataValue is same as the last equal count DataValue
            //                // -- this will add in the current legend
            //                this[themeIndex].Legends[LegendCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //                ToRange = Convert.ToDecimal(NumericIndicatorDataView[Index][Data.DataValue]);
            //                this[themeIndex].Legends[LegendCount - 1].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString)));
            //                this[themeIndex].Legends[LegendCount - 1].Count = this[themeIndex].Legends[LegendCount - 1].Count + 1;

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
            //if (this[themeIndex].Legends[this[themeIndex].BreakCount - 1].RangeTo < this[themeIndex].Maximum)
            //{               
            //    //System.Globalization.CultureInfo oldC = System.Threading.Thread.CurrentThread.CurrentCulture;
            //    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //    NumericIndicatorDataView.RowFilter = Data.DataValue + ">" + this[themeIndex].Legends[this[themeIndex].BreakCount - 1].RangeTo;

            //    System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

            //    this[themeIndex].Legends[this[themeIndex].BreakCount - 1].Count = this[themeIndex].Legends[this[themeIndex].BreakCount - 1].Count + NumericIndicatorDataView.Count;
            //    this[themeIndex].Legends[this[themeIndex].BreakCount - 1].SetTovalue(Convert.ToDecimal(this[themeIndex].Maximum.ToString(FormatString)));
            //}            
            //presentationData.RowFilter = string.Empty;
        }

        /// <summary>
        /// Update the legends on the basis of Equalsize break type
        /// </summary>        
        /// <param name="presentationData"></param>
        /// <param name="themeIndex"></param>
        private void EqualSize(DataView presentationData,int themeIndex)
        {
            if (this[themeIndex].IsFreequenctTableTheme)
            {
                // -- filter the dataview according to the IUS.
                presentationData.RowFilter = " " + Indicator_Unit_Subgroup.IUSNId + " = " + this[themeIndex].IndicatorNId + "";
            }
            else
            {
                // -- filter the dataview according to the indicator.            
                presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this[themeIndex].IndicatorNId + "";
            }            

            // -- Create the data table to store Indicator Nid, Name and Data value
            DataTable RangeDataTable = new DataTable();
            // -- Get the valid numeric value
            RangeDataTable = this[themeIndex].ValidNumericValue(presentationData, this[themeIndex].Decimal);

            DataTable LegendDv = new DataTable();

            LegendDv = DICommon.GenerateEqualSize(RangeDataTable.DefaultView, this[themeIndex].BreakCount, this[themeIndex].Minimum, this[themeIndex].Maximum, this[themeIndex].Decimal);
            int Index = 0;
            foreach (DataRow LegendRow in LegendDv.Rows)
            {
                this[themeIndex].Legends[Index].Range = LegendRow[0].ToString() + " - " + LegendRow[1].ToString();
                this[themeIndex].Legends[Index].SetFromvalue(Convert.ToDecimal(LegendRow[0]));
                this[themeIndex].Legends[Index].SetTovalue(Convert.ToDecimal(LegendRow[1]));
                this[themeIndex].Legends[Index].Count = Convert.ToInt32(LegendRow[2]);
                Index += 1;
            }

            presentationData.RowFilter = string.Empty;

            //decimal IntervalGap = 0;
            //decimal EqualSizeInterval = 0;
            //string FormatString = string.Empty;

            //// -- filter the dataview according to the indicator.

            //presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this[themeIndex].IndicatorNId + "";

            //// -- Create the data table to store Indicator Nid, Name and Data value
            //DataTable RangeDataTable = new DataTable();
            //// -- Get the valid numeric value
            //RangeDataTable = this[themeIndex].ValidNumericValue(presentationData, this[themeIndex].Decimal);

            //DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

            //// -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
            //IntervalGap = this[themeIndex].SetIntervalGap();

            //// -- Set the count on each interval.
            //EqualSizeInterval = Math.Round(((this[themeIndex].Maximum - this[themeIndex].Minimum - (IntervalGap * (this[themeIndex].BreakCount - 1))) / this[themeIndex].BreakCount), this[themeIndex].Decimal);

            //if (EqualSizeInterval <= 0)
            //{
            //    EqualSizeInterval = IntervalGap;
            //}

            //// -- sort the data table on the basis of data value
            //NumericIndicatorDataView.Sort = Data.DataValue + " Asc";

            //// -- Create the Format string on the basis of Decimal places
            //FormatString = this[themeIndex].FormatNumber(this[themeIndex].Decimal);

            //System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            //// -- Valid range value exists in the dataview
            ////NumericIndicatorDataView.RowFilter = Data.DataValue + ">=" + Convert.ToDouble(DICommon.SetDecimalSperator(this[themeIndex].Minimum.ToString(FormatString))) + " AND " + Data.DataValue + "<=" + Convert.ToDouble(DICommon.SetDecimalSperator(this[themeIndex].Maximum.ToString(FormatString)));
            //NumericIndicatorDataView.RowFilter = Data.DataValue + ">=" + Convert.ToDouble(this[themeIndex].Minimum.ToString(FormatString)) + " AND " + Data.DataValue + "<=" + Convert.ToDouble(this[themeIndex].Maximum.ToString(FormatString));

            //System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;

            //int DataValueCount = 0;
            //decimal FromRange = Convert.ToDecimal(this[themeIndex].Minimum.ToString(FormatString));
            //decimal ToRange = 0;
            //int DataCount = 0;
            //int Count = 0;

            //for (DataValueCount = 1; DataValueCount <= this[themeIndex].BreakCount; DataValueCount++)
            //{
            //    if (DataValueCount == 1)
            //    {
            //        // -- For the first legend
            //        FromRange = this[themeIndex].Minimum;
            //        ToRange = FromRange + EqualSizeInterval;
            //    }
            //    else
            //    {
            //        // -- For the rest of the legends.
            //        FromRange = FromRange + EqualSizeInterval + IntervalGap;
            //        if (DataValueCount < this[themeIndex].BreakCount)
            //        {
            //            // -- Set the ToRange but not for the last legend
            //            ToRange = FromRange + EqualSizeInterval;
            //        }
            //        else
            //        {
            //            // -- Set the ToRange for the last legend.
            //            ToRange = this[themeIndex].Maximum;
            //        }
            //    }
            //    // -- set the FromRange and ToRange to Maximum value, if it is greater then the max value
            //    if (FromRange > this[themeIndex].Maximum)
            //    {
            //        FromRange = this[themeIndex].Maximum;
            //    }
            //    if (ToRange > this[themeIndex].Maximum)
            //    {
            //        ToRange = this[themeIndex].Maximum;
            //    }

            //    // -- Datavalue count does not repeat for a range.
            //    if (DataCount < NumericIndicatorDataView.Count)
            //    {
            //        // -- Set the Count value
            //        foreach (DataRowView Row in NumericIndicatorDataView)
            //        {
            //            if (Convert.ToDouble(Row[Data.DataValue]) >= Convert.ToDouble(FromRange.ToString(FormatString)) && Convert.ToDouble(Row[Data.DataValue]) <= Convert.ToDouble(ToRange.ToString(FormatString)))
            //            {
            //                // -- DataValue exist between the from range and to range
            //                Count += 1;
            //            }
            //        }
            //    }
            //    // -- update the legend
            //    this[themeIndex].Legends[DataValueCount - 1].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //    this[themeIndex].Legends[DataValueCount - 1].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString)));
            //    this[themeIndex].Legends[DataValueCount - 1].Count = Count;
            //    DataCount = DataCount + Count;
            //    Count = 0;
            //}
            //presentationData.RowFilter = string.Empty;
        }

        #endregion

        #region "-- Event Handler --"

        /// <summary>
        /// Set the frequency table break count for the selected theme
        /// </summary>
        /// <param name="ThemeIndex"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        private void SetFrequencyTableBreakCount(int ThemeIndex, Color startColor, Color endColor)
        {
            string[] ColorShade = new string[this[ThemeIndex].BreakCount];

            // -- get the old legend count.
            int OldBreakCount = this[ThemeIndex].Legends.Count;

            // -- update the breakcount property of the selected theme.
            this[ThemeIndex].SetBreakCount(this[ThemeIndex].BreakCount);
            this[ThemeIndex].SetHighIsGood(true);


            // -- Dispose the seelcted theme legends
            this[ThemeIndex].Legends = null;

            // -- set all the legends
            switch (this[ThemeIndex].BreakType)
            {
                case BreakTypes.EqualCount:
                    this[ThemeIndex].EqualCount(this[ThemeIndex].PresentationData);
                    break;

                case BreakTypes.EqualSize:
                    this[ThemeIndex].EqualSize(this[ThemeIndex].PresentationData);
                    break;

                case BreakTypes.Continuous:
                    this[ThemeIndex].EqualCount(this[ThemeIndex].PresentationData);
                    break;

                case BreakTypes.Discontinuous:
                    this[ThemeIndex].EqualCount(this[ThemeIndex].PresentationData);
                    break;
            }
            
            // -- Call the BuildRangeColor to get the color shades
            ColorShade = this.BuildRangeColor(startColor, endColor, ThemeIndex);
            for (int j = 0; j < this[ThemeIndex].BreakCount; j++)
            {
                this[ThemeIndex].Legends[j].SetColor(ColorTranslator.FromHtml(ColorShade[j]));
            }
        }

        /// <summary>
        /// Set the table break count for all the themes
        /// </summary>
        /// <param name="ThemeIndex"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        private void SetTableBreakCount(int ThemeIndex, Color startColor, Color endColor)
        {
            string[] ColorShade = new string[this[ThemeIndex].BreakCount];

            // -- get the old legend count.
            int OldBreakCount = this[ThemeIndex].Legends.Count;

            // -- update the breakcount property of all the themes
            for (int i = 0; i < this.Count; i++)
            {
                this[i].SetBreakCount(this[ThemeIndex].BreakCount);
                this[i].SetHighIsGood(true);
            }

            // -- Dispose all the legends
            this.DisposeLegends();

            // -- set all the legends
            for (int i = 0; i < this.Count; i++)
            {
                switch (this[i].BreakType)
                {
                    case BreakTypes.EqualCount:
                        this[i].EqualCount(this[ThemeIndex].PresentationData);
                        break;

                    case BreakTypes.EqualSize:
                        this[i].EqualSize(this[ThemeIndex].PresentationData);
                        break;

                    case BreakTypes.Continuous:
                        this[i].EqualCount(this[ThemeIndex].PresentationData);
                        break;

                    case BreakTypes.Discontinuous:
                        this[i].EqualCount(this[ThemeIndex].PresentationData);
                        break;
                }
            }
            // -- Call the BuildRangeColor to get the color shades
            ColorShade = this.BuildRangeColor(startColor, endColor, ThemeIndex);
            // -- Set the color shade to the legends
            for (int i = 0; i < this.Count; i++)
            {
                for (int j = 0; j < this[i].BreakCount; j++)
                {
                    this[i].Legends[j].SetColor(ColorTranslator.FromHtml(ColorShade[j]));
                }
            }
        }

        /// <summary>
        /// This event will execute only when user update the min, max, decimal and break type of theme
        /// </summary>
        /// <param name="ThemeIndex">ThemeIndex</param>
        /// <param name="startColor">startColor</param>
        /// <param name="endColor">endColor</param>
        private void pTheme_SetBreakCount(int ThemeIndex,Color startColor,Color endColor)
        {
            if (this[ThemeIndex].IsFreequenctTableTheme)
            {
                this.SetFrequencyTableBreakCount(ThemeIndex, startColor, endColor);
            }
            else
            {
                this.SetTableBreakCount(ThemeIndex, startColor, endColor);
            }           
        }

        /// <summary>
        /// Event will dispose all the legends and recreates all the legends.
        /// </summary>
        /// <param name="ThemeIndex">ThemeIndex</param>
        private void pTheme_SetThemeEvent(int ThemeIndex)
        {
            string FormatString = string.Empty;
            FormatString = this[ThemeIndex].FormatNumber(this[ThemeIndex].Decimal);
            this[ThemeIndex].SetMinimum(Convert.ToDecimal(this[ThemeIndex].Minimum.ToString(FormatString)));
            this[ThemeIndex].SetMaximum(Convert.ToDecimal(this[ThemeIndex].Maximum.ToString(FormatString)));
            switch (this[ThemeIndex].BreakType)
            {
                case BreakTypes.EqualCount:
                    this.UpdateEqualCountLegends(this[ThemeIndex].PresentationData, ThemeIndex);
                    break;

                case BreakTypes.EqualSize:
                    this.EqualSize(this[ThemeIndex].PresentationData, ThemeIndex);
                    break;

                case BreakTypes.Continuous:
                    this.UpdateEqualCountLegends(this[ThemeIndex].PresentationData, ThemeIndex);
                    break;

                case BreakTypes.Discontinuous:
                    this.UpdateEqualCountLegends(this[ThemeIndex].PresentationData, ThemeIndex);
                    break;
            }
        }

        /// <summary>
        /// Update the legends caption and color according to HighIsGood property
        /// </summary>
        /// <param name="ThemeIndex">ThemeIndex</param>
        private void pTheme_SetHighIsGoodEvent(int ThemeIndex)
        {
            Color TempColor;
            string TempCaption;
            int i=0;
            while (i < Convert.ToInt32(this[ThemeIndex].BreakCount/2))
            {
                // -- swap the caption using TempCaption
                TempCaption = this[ThemeIndex].Legends[i].Caption;
                this[ThemeIndex].Legends[i].SetCaption(this[ThemeIndex].Legends[this[ThemeIndex].BreakCount - i - 1].Caption);
                this[ThemeIndex].Legends[this[ThemeIndex].BreakCount - i - 1].SetCaption(TempCaption);

                // -- swap the color using TempColor
                TempColor = this[ThemeIndex].Legends[i].Color;
                this[ThemeIndex].Legends[i].SetColor(this[ThemeIndex].Legends[this[ThemeIndex].BreakCount - i - 1].Color);
                this[ThemeIndex].Legends[this[ThemeIndex].BreakCount - i - 1].SetColor(TempColor);
                i += 1;
            }    
        }

        private void DisposeLegends()
        {
            // -- Loop will dispose all the legends
            for (int i = 0; i < this.Count; i++)
            {
              this[i].Legends = null;
            }
        }

        #endregion

          #endregion

        #region " -- Public / Friend -- "
 
        #region " -- Properties -- "

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Add the theme in the list
        /// </summary>
        /// <param name="pTheme">Theme</param>
        public void Add(Theme pTheme)
        {
            this.List.Add(pTheme);
            pTheme.SetThemeEvent += new SetThemeDelegate(pTheme_SetThemeEvent);
            pTheme.SetBreakCountEvent += new SetBreakCountDelegate(pTheme_SetBreakCount);
            pTheme.SetHighIsGoodEvent += new SetHighIsGoodDelegate(pTheme_SetHighIsGoodEvent);
        }

        /// <summary>
        /// Remove the theme in the list
        /// </summary>
        /// <param name="themeIndex">Index</param>
        public void Remove(int themeIndex)
        {
            this.List.RemoveAt(themeIndex);
        }

        /// <summary>
        /// Get the theme on the basis of index
        /// </summary>
        /// <param name="ThemeIndex">ThemeIndex</param>
        /// <returns>theme</returns>
        public Theme this[int themeIndex]
        {
            get
            {
                Theme RetVal;
                try
                {
                    if (themeIndex < 0)
                    {
                        // -- For invalid theme Index
                        RetVal = null;
                    }
                    else
                    {
                        RetVal = (Theme)this.List[themeIndex];
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }
                return RetVal; 
            }
        }

        /// <summary>
        /// Get the theme on the basis of indicator name
        /// </summary>
		/// <param name="name">name</param>
        /// <returns>Theme</returns>
        public Theme this[string name]
        {
            get 
            {
                Theme RetVal = null;
                try
                {
                    foreach (Theme PTheme in this.List)
                    {
                        if (DICommon.RemoveQuotes(PTheme.Indicator) == name)
                        {
                            RetVal = PTheme;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }
                return RetVal;
            }
        }

        /// <summary>
        /// Generate the smooth color shade for the legends
        /// </summary>
        /// <param name="startColor">StartColor</param>
        /// <param name="endColor">EndColor</param>
        /// <returns>String[]</returns>
        internal string[] BuildRangeColor(Color startColor, Color endColor, int themeIndex)
        {
            string[] RetVal = new string[this[themeIndex].BreakCount];
            try
            {
                Int16 R1, R2, G1, G2, B1, B2, TR1, TG1, TB1;
                Single RInterval, GInterval, BInterval;

                R1 = startColor.R;
                G1 = startColor.G;
                B1 = startColor.B;

                R2 = endColor.R;
                G2 = endColor.G;
                B2 = endColor.B;

                // -- get the difference between the R, G, B and divide it by number of breaks to get the interval

                RInterval = Convert.ToSingle(Math.Abs((R1 - R2) / (this[themeIndex].BreakCount - 1)));
                GInterval = Convert.ToSingle(Math.Abs((G1 - G2) / (this[themeIndex].BreakCount - 1)));
                BInterval = Convert.ToSingle(Math.Abs((B1 - B2) / (this[themeIndex].BreakCount - 1)));

                for (int ColorCount = 1; ColorCount <= this[themeIndex].BreakCount; ColorCount++)
                {
                    if (ColorCount == 1)
                    {
                        // -- first color
                        TR1 = startColor.R;
                        TG1 = startColor.G;
                        TB1 = startColor.B;
                    }
                    else if (ColorCount == this[themeIndex].BreakCount)
                    {
                        //-- last color
                        TR1 = endColor.R;
                        TG1 = endColor.G;
                        TB1 = endColor.B;
                    }
                    else
                    {
                        if (R1 > R2)
                        {
                            // -- For dark red shade then previous one
                            TR1 = Convert.ToInt16(R1 - (RInterval * (ColorCount - 1)));
                        }
                        else
                        {
                            // -- For light red shade then previous one
                            TR1 = Convert.ToInt16(R1 + (RInterval * (ColorCount - 1)));
                        }
                        if (G1 > G2)
                        {
                            // -- For dark green shade then previous one
                            TG1 = Convert.ToInt16(G1 - (GInterval * (ColorCount - 1)));
                        }
                        else
                        {
                            // -- For light green shade then previous one
                            TG1 = Convert.ToInt16(G1 + (GInterval * (ColorCount - 1)));
                        }
                        if (B1 > B2)
                        {
                            // -- For dark blue shade then previous one
                            TB1 = Convert.ToInt16(B1 - (BInterval * (ColorCount - 1)));
                        }
                        else
                        {
                            // -- For light blue shade then previous one
                            TB1 = Convert.ToInt16(B1 + (BInterval * (ColorCount - 1)));
                        }
                    }
                    // -- Add the color in the array
                    RetVal[ColorCount - 1] = ColorTranslator.ToHtml(Color.FromArgb(Math.Abs(TR1), Math.Abs(TG1), Math.Abs(TB1)));
                }
            }
            catch (Exception)
            {
                for (int i = 0; i <= this[themeIndex].BreakCount - 1; i++)
                {
                    RetVal[i] = ColorTranslator.ToHtml(Color.FromArgb(255, 255, 255));
                }
            }
            return RetVal;
        }

        internal void IntializeTheme(DataView presentationData, Themes themes,int themeIndex)
        {
            this[themeIndex].PresentationData = presentationData;
            this[themeIndex].Themes = themes;
        }

        #endregion

        #endregion


        #region ICloneable Members      

        public object Clone()
        {
            Themes RetVal;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(Themes));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (Themes)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        #endregion
    }
}