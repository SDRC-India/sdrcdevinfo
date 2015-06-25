using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Classifications metadata for LookInWindow control
    /// </summary>
    public class MetaDataICLookInSource : BaseLookInSource
    {
        #region "-- Private --"

        #region "-- Methods --"

        private void MetaData_ProcessInfo(int currentRowIndex)
        {
            this.RaiseIncrementProgessBarEvent(currentRowIndex);
        }

        private void MetaData_BeforeProcess(int recordsFound)
        {
            this.RaiseInitializeProgessBarEvent(recordsFound);
        }

        #endregion

        #endregion

        #region"--Protected--"

        #region"--Method--"

        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, searchString, this._IndicatorClassificationType, FieldSelection.Light);
            }
            else
            {
                RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, this._IndicatorClassificationType, FieldSelection.Light);
            }

            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //do nothing
        }

        #endregion


        #endregion

        #region "-- public --"

        #region "-- New/Dispose --"

        public MetaDataICLookInSource(ICType indicatorClassificationType)
        {
            this._IndicatorClassificationType = indicatorClassificationType;
            this.ShowDESButton = true;
        }

        #endregion



        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> SelectedNids, bool allSelected)
        {
            DI7MetaDataBuilder MetaData = new DI7MetaDataBuilder(this._TargetDBConnection, this._TargetDBQueries);
            MetaDataType MetaDataType = MetaDataType.Sector;
            switch (this._IndicatorClassificationType)
            {
                case ICType.Sector:
                    MetaDataType = MetaDataType.Sector;
                    break;
                case ICType.Goal:
                    MetaDataType = MetaDataType.Goal;
                    break;
                case ICType.CF:
                    MetaDataType = MetaDataType.CF;
                    break;
                case ICType.Theme:
                    MetaDataType = MetaDataType.Theme;
                    break;
                case ICType.Source:
                    MetaDataType = MetaDataType.Source;
                    break;
                case ICType.Institution:
                    MetaDataType = MetaDataType.Institution;
                    break;
                case ICType.Convention:
                    MetaDataType = MetaDataType.Convention;
                    break;
                default:
                    break;
            }

            if (this.InputFileType != DataSourceType.Database && this.InputFileType != DataSourceType.Template)
            {
                this.RaiseInitializeProgessBarEvent(1);

                if (MetaDataType == MetaDataType.Source)
                {
                    if (System.IO.Path.GetExtension(this._SelectedFiles[0]).ToUpper() == DICommon.FileExtension.Excel.ToUpper())
                    {
                        // import from excel                        
                        MetaData.ImportMetataFromExcel(MetadataElementType.Source,MetaDataType.Source, this.SelectedNidInTrgDatabase, this._SelectedFiles[0].ToString(), this.XsltFolderPath);
                    }
                    else
                    {
                        // Import data from xml file 
                        ////MetaData.ImportMetadataFromXML(this.GetFileText(this._SelectedFiles[0]), MetaDataType, this.SelectedNidInTrgDatabase, this.XsltFolderPath);
                        MetaData.ImportMetadataFromXML(this.GetFileText(this._SelectedFiles[0]), MetadataElementType.Source, this.SelectedNidInTrgDatabase, this.XsltFolderPath);
                    }
                }
                else
                {

                    // Import data from rtf file 
                    MetaData.ImportMetadataFromRTFFile(this.GetFileText(this._SelectedFiles[0]), MetaDataType, this.SelectedNidInTrgDatabase);
                }

                this.RaiseIncrementProgessBarEvent(1);
            }
            else
            {
                //improt data from database/template
                MetaData.BeforeProcess += new ProcessInfoDelegate(MetaData_BeforeProcess);
                MetaData.ProcessInfo += new ProcessInfoDelegate(MetaData_ProcessInfo);

                if (MetaDataType == MetaDataType.Source)
                {
                    MetaData.ImportSourceMetadata(this._TargetDBQueries.DataPrefix, this.SourceDBConnection, this.SourceDBQueries, string.Join(",", SelectedNids.ToArray()), SelectedNids.Count, MetaDataType);
                }
                else
                {
                    MetaData.ImportICMetadata(this._TargetDBQueries.DataPrefix, this.SourceDBConnection, this.SourceDBQueries, string.Join(",", SelectedNids.ToArray()), SelectedNids.Count, MetaDataType,this._IndicatorClassificationType);
                }
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            string ColumnHeader = string.Empty;

            this.Columns.Clear();
            switch (this._IndicatorClassificationType)
            {
                case ICType.CF:
                    ColumnHeader = Constants.LanguageKeys.Framework;
                    break;
                case ICType.Convention:
                    ColumnHeader = Constants.LanguageKeys.Convention;
                    break;
                case ICType.Goal:
                    ColumnHeader = Constants.LanguageKeys.Goal;
                    break;
                case ICType.Institution:
                    ColumnHeader = Constants.LanguageKeys.Institution;
                    break;
                case ICType.Sector:
                    ColumnHeader = Constants.LanguageKeys.Sector;
                    break;
                case ICType.Source:
                    ColumnHeader = Constants.LanguageKeys.Source;
                    break;
                case ICType.Theme:
                    ColumnHeader = Constants.LanguageKeys.Theme;
                    break;
                default:
                    break;
            }

            this.Columns.Add(IndicatorClassifications.ICName,DILanguage.GetLanguageString( ColumnHeader));
            this.TagValueColumnName = IndicatorClassifications.ICNId;
            this.GlobalValueColumnName1 = string.Empty;

        }

        #endregion

        #endregion

        #endregion
    }
}
