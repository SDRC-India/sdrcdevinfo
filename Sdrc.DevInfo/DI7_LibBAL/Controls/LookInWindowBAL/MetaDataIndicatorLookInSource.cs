using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;



namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Indicator Metadata for LookInWindow control
    /// </summary>
    public class MetaDataIndicatorLookInSource: BaseLookInSource
    {

        #region"-- Private --"

        #region"-- Methods --"

        private void MetaData_BeforeProcess(int recordsFound)
        {
            this.RaiseInitializeProgessBarEvent(recordsFound);
        }

        private void MetaData_ProcessInfo(int currentRowIndex)
        {
            this.RaiseIncrementProgessBarEvent(currentRowIndex);
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region"-- Methods --"

   
        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

           
                if (!string.IsNullOrEmpty(searchString))
                {
                    FilterString = Indicator.IndicatorName + " like '%" + searchString + "%' ";
                    RetVal = this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.Search, "" + FilterString + "", FieldSelection.Light);
                    
                }
                else
                {
    RetVal = this.SourceDBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light);
                  }
            return RetVal;
        }
        
        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //do nothing
        }

        #endregion


        #endregion

        #region "-- public  --"

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> SelectedNids, bool allSelected)
        {
            int SelectedCount;           
            try
            {
                DI7MetaDataBuilder MetaData = new DI7MetaDataBuilder(this._TargetDBConnection, this._TargetDBQueries);

                if (this.InputFileType != DataSourceType.Database && this.InputFileType != DataSourceType.Template)
                {

                    // Import data from xml or rtf file 
                    this.RaiseInitializeProgessBarEvent(1);

                    if (System.IO.Path.GetExtension(this._SelectedFiles[0]).ToUpper() == DICommon.FileExtension.Excel.ToUpper())
                    {
                        // import from excel
                        MetaData.ImportMetataFromExcel(MetadataElementType.Indicator,MetaDataType.Indicator, this.SelectedNidInTrgDatabase, this._SelectedFiles[0], this.XsltFolderPath);
                                            
                    }
                    else
                    {
                        // import from xml 
                        MetaData.ImportMetadataFromXML(this.GetFileText(this._SelectedFiles[0]),MetadataElementType.Indicator, this.SelectedNidInTrgDatabase, this.XsltFolderPath);
                    }

                    this.RaiseIncrementProgessBarEvent(1);
                }
                else
                {
                    MetaData.ProcessInfo += new ProcessInfoDelegate(MetaData_ProcessInfo);
                    MetaData.BeforeProcess += new ProcessInfoDelegate(MetaData_BeforeProcess);
                    //improt data from database/template
                    SelectedCount = SelectedNids.Count;
                    if (allSelected)
                        SelectedCount = -1;

                    MetaData.ImportIndicatorMetadata(this._TargetDBQueries.DataPrefix, this.SourceDBConnection, this.SourceDBQueries, string.Join(",", SelectedNids.ToArray()), SelectedCount, MetaDataType.Indicator);
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();

            this.Columns.Add(Indicator.IndicatorName,DILanguage.GetLanguageString("Indicator"));
            this.TagValueColumnName = Indicator.IndicatorNId;
            this.GlobalValueColumnName1 = Indicator.IndicatorGlobal;
        }

        #endregion

        #endregion

        #endregion
       
    }
}
