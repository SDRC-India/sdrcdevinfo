using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;


using System.Reflection;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Resources;
using DevInfo.Lib.DI_LibBAL.LogFiles;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    public class CrossTabLogFile : DXLogFile
    {

        #region "--Public--"

        #region"--METHODS--"

        /// <summary>
        /// Start Input File Log with passing Filename which is in processing
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void StartInputFileLog(string fileNameWPath)
        {
            this.InsertHorizontalRegion(string.Empty, "2");
            this.Writer.WriteBreak();
            this.Writer.WriteBreak();

            this.InsertStartTableTagWAttr();
            this.InsertTableRow(Constants.ProcessedFile, fileNameWPath);
            
        }

        /// <summary>
        /// Create UnmatchedElememts if it's key is not present
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        private void CreateUnmatchedElements(string tableCaptionOrIndex)
        {
            if (!this.UnmatchedElements.ContainsKey(tableCaptionOrIndex))
            {
                this.UnmatchedElements.Add(tableCaptionOrIndex, new UnMatchedElementInfo());
            }
        }

        /// <summary>
        /// Add Unmatched Indicator in unmatched Elements Dictionary
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        /// <param name="indicatorName"></param>
        /// <param name="indicatorGID"></param>
        public void AddUnmatchedIndicator(string tableCaptionOrIndex, string indicatorName, string indicatorGID)
        {
            this.CreateUnmatchedElements(tableCaptionOrIndex);

            if (!this.UnmatchedElements[tableCaptionOrIndex].Indicators.ContainsKey(indicatorGID))
            { this.UnmatchedElements[tableCaptionOrIndex].Indicators.Add(indicatorGID, indicatorName); }

        }

        /// <summary>
        /// Add Unmatched Unit in unmatched Elements Dictionary
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        /// <param name="unitName"></param>
        /// <param name="unitGID"></param>
        public void AddUnmatchedUnit(string tableCaptionOrIndex, string unitName, string unitGID)
        {
            this.CreateUnmatchedElements(tableCaptionOrIndex);

            if (!this.UnmatchedElements[tableCaptionOrIndex].Units.ContainsKey(unitGID))
            {
                this.UnmatchedElements[tableCaptionOrIndex].Units.Add(unitGID, unitName);
            }
        }

        /// <summary>
        /// Add Unmatched Subgroup in unmatched Elements Dictionary
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        /// <param name="subgroupName"></param>
        /// <param name="subgroupGId"></param>
        public void AddUnmatchedSubgroup(string tableCaptionOrIndex, string subgroupName, string subgroupGId)
        {
            this.CreateUnmatchedElements(tableCaptionOrIndex);

            if (!this.UnmatchedElements[tableCaptionOrIndex].Subgroups.ContainsKey(subgroupGId))
            {
                this.UnmatchedElements[tableCaptionOrIndex].Subgroups.Add(subgroupGId, subgroupName);
            }
        }

        /// <summary>
        /// Add Unmatched Area in unmatched Elements Dictionary
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        /// <param name="areaID"></param>
        /// <param name="areaName"></param>
        public void AddUnmatchedArea(string tableCaptionOrIndex, string areaID, string areaName)
        {
            this.CreateUnmatchedElements(tableCaptionOrIndex);


            if (!this.UnmatchedElements[tableCaptionOrIndex].Areas.ContainsKey(areaID))
            {
                this.UnmatchedElements[tableCaptionOrIndex].Areas.Add(areaID, areaName);
            }


        }


        /// <summary>
        /// Add Unmapped Cells  in unmapped Cells Dictionary 
        /// </summary>
        /// <param name="tableCaptionOrIndex"></param>
        /// <param name="cellValue"></param>
        /// <param name="indicator"></param>
        /// <param name="unit"></param>
        /// <param name="subgroup"></param>
        /// <param name="area"></param>
        /// <param name="time"></param>
        /// <param name="source"></param>
        public void AddUnMappedCells(string tableCaptionOrIndex, string cellValue, bool indicator, bool unit, bool subgroup, bool area, bool time, bool source)
        {
            UnMappedCellInfo CellInfo;

            if (!this.UnmappedCells.ContainsKey(tableCaptionOrIndex))
            {
                this.UnmappedCells.Add(tableCaptionOrIndex, new List<UnMappedCellInfo>());
            }

            // add unmapped cell value

            CellInfo = new UnMappedCellInfo();
            CellInfo.Area = area;
            CellInfo.Unit = unit;
            CellInfo.Source = source;
            CellInfo.Time = time;
            CellInfo.Subgroup = subgroup;
            CellInfo.Indicator = indicator;
            CellInfo.CellValue = cellValue;

            this.UnmappedCells[tableCaptionOrIndex].Add(CellInfo);
        }

        /// <summary>
        /// End Input File and write unmatchedElements and unmapped Cells of processing file
        /// </summary>
        public void EndInputFileLog()
        {
            int TableIndex = 0;
            List<string> TableCaptions;

            try
            {
                // write table info
                if (this.UnmatchedElements.Count > 0 || this.UnmappedCells.Keys.Count > 0)
                {
                    TableCaptions = this.GetTablesCaption();

                    foreach (string TableCaptionOrIndex in TableCaptions)
                    {
                        if (TableIndex > 0)
                        {
                            this.InsertStartTableTagWAttr();
                        }

                        TableIndex++;

                        this.InsertTableRow(Constants.TableName, TableCaptionOrIndex);
                        this.InsertEndTableTag();

                        this.Writer.WriteBreak();
                        this.Writer.WriteBreak();

                        // write unmatched elements                            
                        if (this.UnmatchedElements.Count > 0)
                        {
                            if (this.UnmatchedElements.ContainsKey(TableCaptionOrIndex))
                            {
                                this.WriteUnmatchedElements(TableCaptionOrIndex);
                            }
                        }

                        this.Writer.WriteBreak();


                        // write unmapped cells
                        if (this.UnmappedCells.Keys.Count > 0)
                        {
                            if (this.UnmappedCells.ContainsKey(TableCaptionOrIndex))
                            {
                                this.WriteUnmappedCells(TableCaptionOrIndex);
                            }
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }



        public CrossTabLogFile()
        {
            this.UnmatchedElements = new Dictionary<string, UnMatchedElementInfo>();
            this.UnmappedCells = new Dictionary<string, List<UnMappedCellInfo>>();
            this.CheckNCreateImageFile();
        }

        #endregion

        #endregion


        #region "--Private--"




        #region "--Variables--"
     
        private Dictionary<string, UnMatchedElementInfo> UnmatchedElements;
        private Dictionary<string, List<UnMappedCellInfo>> UnmappedCells;


        #endregion

        #region"--METHODS--"

        private void CheckNCreateImageFile()
        {
            string ImageFilePath =DICommon.DefaultFolder.DefaultSpreadSheetsFolder + Constants.ImageName;

            // create yes image from resources
            if (!File.Exists(ImageFilePath))
            {
                try
                {
                    Resource1.yesImage.Save(ImageFilePath );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                   // throw new ApplicationException(ex.ToString());
                }
                
            }
        }

        private void WriteUnmatchedElements(string tableCaptionOrIndex)
        {
            UnMatchedElementInfo ElementInfo;

            try
            {
                ElementInfo = this.UnmatchedElements[tableCaptionOrIndex];

                // write unmatched element caption
                this.InsertSpan(Constants.UnmatchedElements, Constants.LogFileFontTypes[LogFileFontType.H1]);
                this.Writer.WriteBreak();

                // write unmatched indicator, unit, subgroup & area
                //indicator
                this.WriteUnmatchedElementsDetail(Constants.UnmatchedIndicators, Constants.IndicatorName, Constants.IndicatorGID, ElementInfo.Indicators);
                // Unit

                this.WriteUnmatchedElementsDetail(Constants.UnmatchedUnits, Constants.UnitName, Constants.UnitGID, ElementInfo.Units);

                // subgroup
                this.WriteUnmatchedElementsDetail(Constants.UnmatchedSubgroups, Constants.Subgroup, Constants.SubgroupGID, ElementInfo.Subgroups);


                // Area
                this.WriteUnmatchedElementsDetail(Constants.UnmatchedAreas, Constants.AreaName, Constants.AreaID, ElementInfo.Areas);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
           
        }



        private void WriteUnmatchedElementsDetail(string unmatchedElementHeader,string column1,string column2, Dictionary<string,string> unmatchedElements)
        {
            // Indicator
            // write Unmatched Indicators caption
            this.Writer.WriteBreak();
            this.InsertSpan(unmatchedElementHeader, Constants.LogFileFontTypes[LogFileFontType.H2]);


            this.InsertStartTableTag();

            // write header : Indicator name ,Indicator GId                                
            this.InsertStartRowTag();
            this.InsertHeading(column1,"50%");
            this.InsertHeading(column2,"50%");
            this.InsertEndRowTag();            

            foreach (string GID in unmatchedElements.Keys)
            {
                this.InsertStartRowTag();

                this.InsertHeadingValue(unmatchedElements[GID]);
                this.InsertHeadingValue(GID);

                this.InsertEndRowTag();
            }

            this.InsertEndTableTag();
        }

        private void WriteUnmappedCells(string tableCaptionOrIndex)
        {
           List<UnMappedCellInfo> CellInfo ;

           try
           {

               CellInfo = this.UnmappedCells[tableCaptionOrIndex];

               // write unmatched element caption
               this.InsertSpan(Constants.UnmappedElements, Constants.LogFileFontTypes[LogFileFontType.H1]);
               this.Writer.WriteBreak();

               this.InsertStartTableTag();

               this.WriteUnmappedHeaders();




               foreach (UnMappedCellInfo cInfo in CellInfo)
               {
                   this.InsertStartRowTag();
                   this.InsertHeadingValue(cInfo.CellValue);


                   this.InsertImageRow(cInfo.Indicator);
                   this.InsertImageRow(cInfo.Unit);
                   this.InsertImageRow(cInfo.Subgroup);
                   this.InsertImageRow(cInfo.Area);
                   this.InsertImageRow(cInfo.Time);
                   this.InsertImageRow(cInfo.Source);

                   this.InsertEndRowTag();

               }

               this.InsertEndTableTag();
               this.Writer.WriteBreak();
               this.Writer.WriteBreak();
           }
           catch (Exception ex)
           {
               throw new ApplicationException(ex.ToString());
           }
        }

        private void WriteUnmappedHeaders()
        {
            this.InsertStartRowTag();
            this.InsertHeading(Constants.CellValue);
            this.InsertHeading(Constants.Indicator);
            this.InsertHeading(Constants.Unit);
            this.InsertHeading(Constants.Subgroup);
            this.InsertHeading(Constants.Area);
            this.InsertHeading(Constants.Time);
            this.InsertHeading(Constants.Source);
            this.InsertEndRowTag();
            
        }

        private List<string> GetTablesCaption()
        {
            List<string> RetVal = new List<string>();

            if (this.UnmatchedElements.Count > this.UnmappedCells.Keys.Count)
            {                
                foreach (string TableCaptionOrIndex in this.UnmatchedElements.Keys)
                {
                    RetVal.Add(TableCaptionOrIndex);
                }
            }
            else
            {
                foreach (string TableCaptionOrIndex in this.UnmappedCells.Keys)
                {
                    RetVal.Add(TableCaptionOrIndex);
                }
            }

            return RetVal;
        }


        private void InsertImageRow(bool IsUnmapped)
        {
            string ImageFileNameWPath = DICommon.DefaultFolder.DefaultSpreadSheetsFolder + "\\" + Constants.ImageName;
            this.InsertStartCellTagWAttr(Constants.LogFileFontTypes[LogFileFontType.H1]);
          
            if (IsUnmapped == true)
            {
                Writer.WriteBeginTag("IMG ");


                Writer.WriteAttribute("src", ImageFileNameWPath );
                Writer.WriteAttribute("style", "text-align:center;");
                Writer.Write(HtmlTextWriter.TagRightChar);

                Writer.WriteEndTag("IMG ");
            }
            else
            {
                Writer.Write("-"); 
            }

            this.InsertEndCellTag();
            
        }
        #endregion


        #endregion



    }
}
