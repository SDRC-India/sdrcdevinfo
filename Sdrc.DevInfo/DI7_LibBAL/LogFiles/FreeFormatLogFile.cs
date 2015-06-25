using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    public class FreeFormatLogFile : DXLogFile
    {
        #region "--Private--"

        #region "--Variables--"

        /// <summary>
        /// Key: Processed filename and value is collection of data value with error description (value= Key:data value, value: description)
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> SkippedRows;
        private string CurrentFileName = string.Empty;

        #endregion

        #region "--Methods--"

        private void WriteSkippedRows()
        {
            this.InsertHorizontalRegion(string.Empty, "2");

            foreach (string fileName in this.SkippedRows.Keys)
            {
                this.Writer.WriteBreak();
                this.Writer.WriteBreak();

                this.InsertStartTableTag();
                this.InsertStartRowTag();
                this.InsertMainHeading("ProcessedFileName: ", "30%");
                this.InsertMainHeadingValue(fileName);
                this.InsertEndRowTag();

                this.InsertBlankRows();
                this.InsertEndTableTag();

                this.InsertStartTableTagWAttr();
                this.InsertStartRowTag();
                this.InsertHeading("Skipped Data Rows");
                this.InsertEndRowTag();

                Dictionary<string, string> SkippedVal = this.SkippedRows[fileName];

                foreach (string val in SkippedVal.Keys)
                {
                    this.InsertStartRowTag();
                    this.InsertHeadingValue(val, "50%");
                    this.InsertHeadingValue(SkippedVal[val], "50%");
                    this.InsertEndRowTag();
                }
                this.InsertEndTableTag();
            }
        }
        
        #endregion

        #endregion

        #region "--Public--"
        
        #region "--Methods--"

        public void StartInputFileLog(string fileNameWPath)
        {
            if (!this.SkippedRows.ContainsKey(fileNameWPath))
            {
                SkippedRows.Add(fileNameWPath, new Dictionary<string, string>());
            }

            this.CurrentFileName = fileNameWPath;
        }

        public void AddSkippedDataValue(string skippedDataValue, string errorDescription)
        {
            if (!this.SkippedRows[this.CurrentFileName].ContainsKey(skippedDataValue))
            {
                SkippedRows[this.CurrentFileName].Add(skippedDataValue, errorDescription);
            }
        }

        public void EndInputFileLog()
        {
            this.WriteSkippedRows();
            // base.Close(logfilenamewPath);           
        }
        
        #endregion

        #region "--New/Dispose--"

        public FreeFormatLogFile()
        {
            SkippedRows = new Dictionary<string, Dictionary<string, string>>();
        }

        #endregion

        #endregion
    }
}
