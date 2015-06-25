using System;
//using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations
{
    /// <summary>
    /// Class contains the Legend collections.
    /// </summary>
    [Serializable()]
    public class Legends : System.Collections.CollectionBase, ICloneable
    {
        # region " -- Private -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Private constructor is needed to serailize and deserialize the class
        /// </summary>
        public Legends()
        {
        }

        #endregion

        #region " -- Variables -- "

        private Theme Theme;

        #endregion

        #region " -- Methods -- "


        /// <summary>
        /// Update the legend in case of Continuous break type
        /// </summary>
        /// <param name="legendIndex">Legend Index</param>
        /// <param name="presentationData">Presentation Data</param>
        private void SetContinuousLegend(int legendIndex,DataView presentationData)
        {
            if (this.Theme.IsFreequenctTableTheme)
            {
                // -- filter the data view on the basis of IUS NID 
                presentationData.RowFilter = " " + Indicator_Unit_Subgroup.IUSNId + " = " + this.Theme.IndicatorNId + "";
            }
            else
            {
                // -- filter the data view on the basis of Indicator NID 
                presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this.Theme.IndicatorNId + "";
            }

            // -- Create the data table to store Indicator NId, Name and Data value
            DataTable RangeDataTable = new DataTable();
            // -- Get the records numeric value
            RangeDataTable = this.Theme.ValidNumericValue(presentationData, this.Theme.Decimal);

            // -- Update the range
            this[legendIndex].Range = this[legendIndex].RangeFrom.ToString() + " - " + this[legendIndex].RangeTo.ToString();            
            
            DataTable LegendDv = new DataTable();

            LegendDv = DICommon.GenerateEqualCount(RangeDataTable.DefaultView, this.Theme.BreakCount - (legendIndex + 1), this.Theme.Maximum, this.Theme.Decimal, this[legendIndex].RangeTo);
            int Index = legendIndex+1;
            foreach (DataRow LegendRow in LegendDv.Rows)
            {
                this[Index].Range = Convert.ToDecimal(LegendRow[0]).ToString() + " - " + Convert.ToDecimal(LegendRow[1]).ToString();
                this[Index].SetFromvalue(Convert.ToDecimal(LegendRow[0]));
                this[Index].SetTovalue(Convert.ToDecimal(LegendRow[1]));
                this[Index].Count = Convert.ToInt32(LegendRow[2]);
                Index += 1;
            }

            //-- Update the legend count
            int LegentCount = 0;
            foreach (DataRow Row in RangeDataTable.Rows)
            {
                if (Convert.ToDecimal(Row[DataExpressionColumns.NumericData]) >= this[legendIndex].RangeFrom & Convert.ToDecimal(Row[DataExpressionColumns.NumericData]) <= this[legendIndex].RangeTo)
                {
                    LegentCount++;
                }
            }

            this[legendIndex].Count = LegentCount;  

            presentationData.RowFilter = string.Empty;

            //decimal IntervalGap = 0;
            //decimal EqualSizeInterval = 0;
            //int AdjustedLegend = legendIndex;

            // // -- filter the data view on the basis of Indicator NID 
            //presentationData.RowFilter = " " + Indicator.IndicatorNId.ToString() + " = " + this.Theme.IndicatorNId + "";

            //// -- Create the data table to store Indicator NId, Name and Data value
            //DataTable RangeDataTable = new DataTable();
            //// -- Get the records numeric value
            //RangeDataTable = this.Theme.ValidNumericValue(presentationData,this.Theme.Decimal);

            //DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

            //// -- sort the DataView
            //NumericIndicatorDataView.Sort = Data.DataValue + " Asc";

            //// -- Number of legends to be updated
            //int AdjustLegendCount = this.Theme.BreakCount - legendIndex;
            //string FormatString = this.Theme.FormatNumber(this.Theme.Decimal);

            //// -- Set the next interval gap like the (Difference between the previous "To value" and new "From value"
            //IntervalGap =this.Theme.SetIntervalGap();

            //// -- Get the equal size interval
            //decimal MinValue = Math.Min(this[legendIndex].RangeTo + IntervalGap, this.Theme.Maximum);

            //if (MinValue == this.Theme.Maximum)
            //{
            //    EqualSizeInterval = 0;
            //}
            //else
            //{
            //    EqualSizeInterval = Math.Round((((this.Theme.Maximum - this.Theme.Minimum + IntervalGap) - (AdjustLegendCount * IntervalGap)) / AdjustLegendCount), this.Theme.Decimal);
            //}

            //int DataValueCount = 0;
            //decimal FromRange = Convert.ToDecimal(this[legendIndex].RangeFrom.ToString(FormatString));
            //decimal ToRange = Convert.ToDecimal(this[legendIndex].RangeTo.ToString(FormatString));
            //int Count = 0;            

            //// -- Set the count for the legend whose FROMVALUE was changed
            //foreach (DataRowView Row in NumericIndicatorDataView)
            //{                
            //    if (Convert.ToDecimal(Row[Data.DataValue]) >= FromRange && Convert.ToDecimal(Row[Data.DataValue]) <= ToRange)
            //    {
            //        // -- DataValue exist between the from range and to range
            //        Count += 1;
            //    }
            //}
            //this[legendIndex].Count = Count;
            //FromRange = Convert.ToDecimal(this[legendIndex].RangeFrom.ToString(FormatString));
            
            //legendIndex += 1;            
            //Count = 0;            
            //ToRange = 0;

            //for (DataValueCount = legendIndex; DataValueCount < this.Theme.BreakCount; DataValueCount++)
            //{
            //    //--Set Range-From
            //    if (DataValueCount == AdjustedLegend + 1)
            //    {
            //        //--Set the Start RangeFrom value to Min Value
            //        this[legendIndex].SetFromvalue(Convert.ToDecimal(MinValue.ToString(FormatString)));
            //    }
            //    else
            //    {
            //        // -- set the FromRange legend for rest of the legend
            //        FromRange = Math.Min(this[legendIndex - 1].RangeTo + IntervalGap, this.Theme.Maximum);
            //        this[legendIndex].SetFromvalue(Convert.ToDecimal(FromRange.ToString(FormatString)));
            //    }
            //    if (DataValueCount == this.Theme.BreakCount - 1)
            //    {
            //        // --Set the last RangeTo value to Max Value
            //        ToRange = this.Theme.Maximum;
            //        this[legendIndex].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString)));
            //    }
            //    else
            //    {
            //        // -- set the ToRange legend for rest of the legend
            //        ToRange = Math.Min(this[legendIndex].RangeFrom + EqualSizeInterval, this.Theme.Maximum);
            //        //ToRange = Math.Round(ToRange);
            //        this[legendIndex].SetTovalue(Convert.ToDecimal(ToRange.ToString(FormatString)));
            //    }

            //    if (this[legendIndex].RangeFrom == this.Theme.Maximum && this[legendIndex].RangeTo == this.Theme.Maximum)
            //    {
            //        // -- if both the from value and to value are same, means no datavalue exist for that interval
            //        Count = 0;
            //    }
            //    else
            //    {
            //        // -- Countthe datavalue for the range
            //        foreach (DataRowView Row in NumericIndicatorDataView)
            //        {
            //            if (Convert.ToDecimal(Row[Data.DataValue]) >= this[legendIndex].RangeFrom && Convert.ToDecimal(Row[Data.DataValue]) <= this[legendIndex].RangeTo)
            //            {
            //                // -- DataValue exist between the from range and to range
            //                Count += 1;
            //            }
            //        }
            //    }
            //    this[legendIndex].Count = Count;
            //    legendIndex += 1;
            //    Count = 0;
            //}
            //presentationData.RowFilter = string.Empty;
        }

        /// <summary>
        /// Update the legend in case of Discontinuous break type
        /// </summary>
        /// <param name="legendIndex">Legend Index</param>
        /// <param name="presentationData">Presentation Data</param>
        private void SetDiscontinuousLegend(int legendIndex,DataView presentationData)
        {
            if (this.Theme.IsFreequenctTableTheme)
            {
                // -- filter the data view on the basis of IUS NID 
                presentationData.RowFilter = " " + Indicator_Unit_Subgroup.IUSNId + " = " + this.Theme.IndicatorNId + "";
            }
            else
            {
                // -- filter the data view on the basis of Indicator NID
                presentationData.RowFilter = " " + Indicator.IndicatorNId + " = " + this.Theme.IndicatorNId + "";
            }

            // -- Create the data table to store Indicator Nid, Name and Data value
            DataTable RangeDataTable = new DataTable();
            // -- Get the valid numeric value
            RangeDataTable = this.Theme.ValidNumericValue(presentationData, this.Theme.Decimal);

            DataView NumericIndicatorDataView = RangeDataTable.DefaultView;

            // -- sort the DataView
            NumericIndicatorDataView.Sort = DataExpressionColumns.NumericData + " Asc";

            DataRowView RowView;
            int DataCount = 0;

            // -- Count The Datavalue that comes within the range
            for (int i = 0; i <= NumericIndicatorDataView.Count - 1; i++)
            {
                RowView = NumericIndicatorDataView[i];
                if (Convert.ToDecimal(RowView[DataExpressionColumns.NumericData]) >= this[legendIndex].RangeFrom && Convert.ToDecimal(RowView[DataExpressionColumns.NumericData]) <= this[legendIndex].RangeTo)
                {
                    DataCount++;
                }
            }
            this[legendIndex].Range = this[legendIndex].RangeFrom.ToString() + " - " + this[legendIndex].RangeTo.ToString();
            this[legendIndex].Count = DataCount;
            presentationData.RowFilter = string.Empty;
        }

        #endregion

        #region " -- Event Handler -- "

        
        private void pLegend_SetLegendsEvent(int LegendIndex)
        {
            try
            {
                if (this.Theme.BreakType == BreakTypes.Continuous)
                {
                    // -- for continuous break type
                    this.SetContinuousLegend(LegendIndex, this.Theme.PresentationData);
                }
                else if (this.Theme.BreakType == BreakTypes.Discontinuous)
                {
                    // -- for Discontinuous break type
                    this.SetDiscontinuousLegend(LegendIndex, this.Theme.PresentationData);
                }

            }
            catch (Exception)
            {
            }  
        }       

        private void pLegend_SetCaptionEvent(int LegendIndex)
        {
            if (this.Theme.IsFreequenctTableTheme)
            {
                this.Theme.Legends[LegendIndex].SetCaption(this[LegendIndex].Caption);
            }
            else
            {
                // -- update the caption of legend for all themes
                for (int i = 0; i < this.Theme.Themes.Count; i++)
                {
                    //if (this.Theme.Themes[i].HighIsGood)
                    //{
                    // -- if the HighIsGood is set to be true
                    this.Theme.Themes[i].Legends[LegendIndex].SetCaption(this[LegendIndex].Caption);
                    //}
                    //else
                    //{
                    //    // -- if the HighIsGood is set to be false, We have to set the caption in reversed order
                    //    this.Theme.Themes[this.Theme.Themes[i].BreakCount - 1].Legends[LegendIndex].SetCaption(this[LegendIndex].Caption);
                    //}
                }
            }
        }

        private void pLegend_SetColorEvent(int LegendIndex)
        {
            // -- update the color of legend for all themes
            for (int i = 0; i < this.Theme.Themes.Count ; i++)
            {
                //if (this.Theme.Themes[i].HighIsGood)
                //{
                    // -- if the HighIsGood is set to be true
                    this.Theme.Themes[i].Legends[LegendIndex].SetColor(this[LegendIndex].Color);    
                //}
                //else
                //{
                //    // -- if the HighIsGood is set to be false, We have to set the color in reversed order
                //    this.Theme.Themes[this.Theme.Themes[i].BreakCount - 1].Legends[LegendIndex].SetColor(this[LegendIndex].Color);
                //}                
            }
        }

        #endregion

        # endregion

        #region " -- Public / Friend -- "

        #region " -- Constructor -- "       
        
        /// <summary>
        /// Constructor to intialize the legend.
        /// </summary>
        /// <param name="theme"></param>
        public Legends(Theme theme)
        {
            this.Theme = theme;
        }

        #endregion

        #region " -- Properties -- "

        /// <summary>
        /// Gets the Legend count
        /// </summary>
        public int LegendCount
        {
            get
            {
                return this.List.Count; 
            }
        }

        #endregion

        #region " -- Methods --"

        /// <summary>
        /// Return the LegendItem
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Legend this[int index]
        { 
            get
            {
                Legend RetVal;
                try
                {
                    RetVal = (Legend)this.List[index];
                }
                catch (Exception)
                {
                    RetVal = null;
                }
                return RetVal;
            }
        }      
      
        /// <summary>
        /// add the legend to the list
        /// </summary>
        /// <param name="pLegend">Legend</param>
        public void Add(Legend pLegend)
        {
            this.List.Add(pLegend);
            pLegend.SetLegendsEvent += new SetLegendsDelegate(pLegend_SetLegendsEvent);            
            pLegend.SetCaptionEvent += new SetCaptionDelegate(pLegend_SetCaptionEvent);
            pLegend.SetColorEvent += new SetColorDelegate(pLegend_SetColorEvent);
            //serial();
        }
       
        /// <summary>
        /// Add the range of legends
        /// </summary>
        /// <param name="pLegends"></param>
        public void AddRange(Legend[] pLegends)
        {
            foreach (Legend pLegend in pLegends)
            {
                this.List.Add(pLegend);
            }
        }

        /// <summary>
        /// Remove the legend from the list
        /// </summary>
        /// <param name="pLegend"></param>
        public void Remove(Legend pLegend)
        {
            this.List.Remove(pLegend);
        }

        /// <summary>
        /// Serialize & desrialize the class
        /// </summary>
        /// <returns></returns>
        public Object Clone()
        {
              // -- Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
            object RetVal;
            try
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter BinaryFormatter= new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                System.IO.MemoryStream MemStream = new System.IO.MemoryStream();
                BinaryFormatter.Serialize(MemStream, this);
                MemStream.Position = 0;
                RetVal = BinaryFormatter.Deserialize(MemStream);
                MemStream.Close();
            }
            catch(Exception)
            {
                RetVal= null;
            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}
