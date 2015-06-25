using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Export;

namespace DevInfo.Lib.DI_LibBAL.DES
{
    /// <summary>
    /// Generates Data entry spreadsheet file
    /// </summary>
    public class DESGenerator
    {
        private List<DataEntrySpreadsheet> _DESSheets;
        /// <summary>
        /// Gets or sets DES sheets
        /// </summary>
        public List<DataEntrySpreadsheet> DESSheets
        {
            get
            {
                return this._DESSheets; 
            }
            set
            {
                this._DESSheets = value; 
            }
        }

        public DESGenerator()
        {
            this._DESSheets = new List<DataEntrySpreadsheet>();
        }

        /// <summary>
        /// Returns true after generating a DES file. 
        /// </summary>
        /// <param name="filenameWPath"></param>
        /// <returns></returns>
        public bool GenerateDESFile(string filenameWPath)
        {
            bool RetVal = false;
            DataTable Table=null;
            DIExport DIExportObj = new DIExport();

            try
            {
                // get data table for workbook
                foreach (DataEntrySpreadsheet DESSheet in this._DESSheets)
                {
                    if (Table == null)
                    {
                        Table = DESSheet.GetDESDataTable();
                    }
                    else
                    {
                        Table.Merge(DESSheet.GetDESDataTable());
                    }
                }

              RetVal=  DIExportObj.ExportDataEntrySpreadsheet(true, Table.DefaultView, true, null, filenameWPath, DICommon.LangaugeFileNameWithPath);    


            }
            catch (Exception ex)
            {
                RetVal = false;
                throw  new ApplicationException(ex.ToString());

            }

            return RetVal;
        }

    }
}
