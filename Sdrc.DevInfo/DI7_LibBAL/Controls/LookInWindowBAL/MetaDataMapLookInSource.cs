using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Area Map for LookInWindow control
    /// </summary>
    internal class MetaDataMapLookInSource : BaseLookInSource
    {
        #region "-- Private --"

        #region "-- Methods  --"

        private void MetaData_ProcessInfo(int recordsFound)
        {
            this.RaiseIncrementProgessBarEvent(recordsFound);
        }

        private void MetaData_BeforeProcess(int recordsFound)
        {
            this.RaiseInitializeProgessBarEvent(recordsFound);
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region"-- Methods --"


        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;

            RetVal = this.SourceDBQueries.Area.GetAreaMapMetadata(String.Empty);

            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //do nothing
        }

        #endregion


        #endregion

        #region "-- public --"

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

            MetaData.BeforeProcess += new ProcessInfoDelegate(MetaData_BeforeProcess);
            MetaData.ProcessInfo += new ProcessInfoDelegate(MetaData_ProcessInfo);

            if (this.InputFileType != DataSourceType.Database && this.InputFileType != DataSourceType.Template)
            {
                // Import data from xml file 
                this.RaiseInitializeProgessBarEvent(1);
                if (System.IO.Path.GetExtension(this._SelectedFiles[0]).ToUpper() == DICommon.FileExtension.Excel.ToUpper())
                {
                    // import from excel
                    MetaData.ImportMetataFromExcel(MetadataElementType.Area, MetaDataType.Map, this.SelectedNidInTrgDatabase, this._SelectedFiles[0].ToString(), this.XsltFolderPath);
                }
                else
                {
                    ////MetaData.ImportMetadataFromXML(this.GetFileText(this._SelectedFiles[0]), MetaDataType.Map, this.SelectedNidInTrgDatabase, this.XsltFolderPath);
                    MetaData.ImportMetadataFromXML(this.GetFileText(this._SelectedFiles[0]), MetadataElementType.Area, this.SelectedNidInTrgDatabase, this.XsltFolderPath);
                }
                this.RaiseIncrementProgessBarEvent(1);

            }
            else
            {
                int SelectedCount = SelectedNids.Count;
                if (allSelected)
                    SelectedCount = -1;

                MetaData.ImportAreaMetadata(this._TargetDBQueries.DataPrefix, this.SourceDBConnection, this.SourceDBQueries, string.Join(",", SelectedNids.ToArray()), SelectedCount, MetaDataType.Map);
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();

            this.Columns.Add(Area_Map_Metadata.LayerName, DILanguage.GetLanguageString(Constants.LanguageKeys.Layer));
            this.TagValueColumnName = Area_Map_Metadata.MetadataNId;
            this.GlobalValueColumnName1 = string.Empty;
        }

        #endregion

        #endregion

        #endregion
    }
}
