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
    public class MetaDataCategoryAreaLookInSource : BaseLookInSource
    {

        #region"-- Private --"

        private MetadataElementType MDElementType = MetadataElementType.Area;

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
                FilterString = Metadata_Category.CategoryName + " like '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Search, "" + FilterString + "");

            }
            else
            {
                RetVal = this.SourceDBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, DIQueries.MetadataElementTypeText[this.MDElementType]);
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
                DI7MetadataCategoryBuilder TargetMetaData = new DI7MetadataCategoryBuilder(this._TargetDBConnection, this._TargetDBQueries);
                DI7MetadataCategoryBuilder SrcMetaDataBuilder = new DI7MetadataCategoryBuilder(this._TargetDBConnection, this._TargetDBQueries);
                if (this.InputFileType != DataSourceType.Database && this.InputFileType != DataSourceType.Template)
                {
                    // Import data from xml or rtf file 
                    this.RaiseInitializeProgessBarEvent(1);
                    //if (this.InputFileType == DataSourceType.SDMXWebService)
                    //{
                    // import from database
                    TargetMetaData.ImportAllMetadataCategories(this.SourceDBConnection, this.SourceDBQueries, this.MDElementType);

                    //}
                    this.RaiseIncrementProgessBarEvent(1);
                }
                else
                {
                    //TargetMetaData.ProcessInfo += new ProcessInfoDelegate(MetaData_ProcessInfo);
                    //TargetMetaData.BeforeProcess += new ProcessInfoDelegate(MetaData_BeforeProcess);
                    //improt data from database/template
                    SelectedCount = SelectedNids.Count;

                    if (allSelected)
                    {
                        TargetMetaData.ImportAllMetadataCategories(this.SourceDBConnection, this.SourceDBQueries, this.MDElementType);
                    }
                    else
                    {
                        SrcMetaDataBuilder = new DI7MetadataCategoryBuilder(this.SourceDBConnection, this.SourceDBQueries);
                        foreach (string CategoryNId in SelectedNids)
                        {
                            DI7MetadataCategoryInfo MDCatInfo = SrcMetaDataBuilder.GetMedataCategoryInfo(Convert.ToInt32(CategoryNId));
                            TargetMetaData.CheckNInsertCategory(MDCatInfo);
                        }
                    }
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

            this.Columns.Add(Metadata_Category.CategoryName, DILanguage.GetLanguageString("METADATA_CATEGORY"));
            this.TagValueColumnName = Metadata_Category.CategoryNId;
            this.GlobalValueColumnName1 = string.Empty;
        }

        #endregion

        #endregion

        #endregion

    }
}
