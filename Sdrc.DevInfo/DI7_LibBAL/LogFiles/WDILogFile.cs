using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
   public class WDILogFile:DXLogFile
    {
        #region "--Private--"

        #region "--Variables--"


      

        #endregion


        #region "--Methods


       /// <summary>
       /// Write unmatched series and area
       /// </summary>
       private void WriteSkippedRows()
       {
           if (this._UnmatchedAreas.Count > 0 || this._UnmatchedSeries.Count > 0)
           {
               this.InsertHorizontalRegion(string.Empty, "2");

           }

            this.Writer.WriteBreak();

            if (this._UnmatchedSeries.Count > 0)
            {
                // start unmatched series table
                this.InsertTable("Unmatched Series:");
                this.InsertStartTableTag();

                // add heading
                this.InsertStartRowTag();
                this.InsertHeading("Series Code");
                this.InsertHeading("Series Name");
                this.InsertHeading("Indicator");
                this.InsertHeading("Indicator GID");
                this.InsertHeading("Unit");
                this.InsertHeading("Unit GID");
                this.InsertHeading("Subgroup");
                this.InsertHeading("Subgroup GID");
                this.InsertEndRowTag();       
       
                // add values
                foreach (UnmatchedSeriesInfo SeriesInfo in this._UnmatchedSeries.Values)
                {
                    this.InsertStartRowTag();
                    this.InsertHeadingValue(SeriesInfo.SeriesCode);
                    this.InsertHeadingValue(SeriesInfo.SeriesName);
                    this.InsertHeadingValue(SeriesInfo.MappedIndicatorName);
                    this.InsertHeadingValue(SeriesInfo.MappedIndicatorGID);
                    this.InsertHeadingValue(SeriesInfo.MappedUnit);
                    this.InsertHeadingValue(SeriesInfo.MappedUnitGId);
                    this.InsertHeadingValue(SeriesInfo.MappedSG);
                    this.InsertHeadingValue(SeriesInfo.MappedSGGId);
                    this.InsertEndRowTag();
                }

                // end unmatched series table
                this.InsertEndTableTag();
            }
     

           if (this._UnmatchedAreas.Count > 0)
           {
               //Start unmapped area table
              this.InsertTable("Unmapped Areas:");
              this.InsertStartTableTag();
              this.InsertStartRowTag();
             //add Heading
              this.InsertHeading("Country Code");
              this.InsertHeading("Country Name");
               this.InsertHeading("Area ID");
              this.InsertHeading("Area Name");
              this.InsertEndRowTag();

               //add values
                      foreach (UnMatchedAreaInfo AreaInfo in this._UnmatchedAreas.Values)
                      {           
                               this.InsertStartRowTag();
                               this.InsertHeadingValue(AreaInfo.CountryCode);
                               this.InsertHeadingValue(AreaInfo.CountryName);
                               this.InsertHeadingValue(AreaInfo.MappedAreaID);
                               this.InsertHeadingValue(AreaInfo.MappedArea);               
                               this.InsertEndRowTag();
                       }
               //end table
              this.InsertEndTableTag();
         }
       }          
    
       private void InsertTable(string header)
       {
           this.InsertStartTableTag();
           this.InsertStartRowTag();
           this.InsertMainHeading(header);
           this.InsertEndRowTag();
           this.InsertEndTableTag();
       }
       
       #endregion

        #endregion

        #region "--Public--"

       #region "-- Variables/Properties --"

       private Dictionary<string, UnmatchedSeriesInfo> _UnmatchedSeries;
           /// <summary>
           /// Gets or sets unmatched series information
           /// </summary>
       public Dictionary<string,UnmatchedSeriesInfo> UnmatchedSeries
       {
           get { return this._UnmatchedSeries; }
           set { this._UnmatchedSeries = value; }
       }

       private Dictionary<string, UnMatchedAreaInfo> _UnmatchedAreas;
       /// <summary>
       /// Gets or sets unmatched area information
       /// </summary>
       public Dictionary<string, UnMatchedAreaInfo> UnmatchedAreas
       {
           get { return this._UnmatchedAreas; }
           set { this._UnmatchedAreas = value; }
       }	
       #endregion

       #region"--New/dispose--"


       public WDILogFile()
       {
           this._UnmatchedAreas = new Dictionary<string, UnMatchedAreaInfo>();
           this._UnmatchedSeries = new Dictionary<string, UnmatchedSeriesInfo>();
       }

       #endregion

       #region "--Methods--"

       public override void Close()
       {
           this.WriteSkippedRows();

           base.Close();
       }
       
       


       #endregion

        #endregion

   }
}
