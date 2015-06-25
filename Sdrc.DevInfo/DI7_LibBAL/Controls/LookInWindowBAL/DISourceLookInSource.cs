using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{

    /// <summary>
    /// Helps in getting sources for LookInWindow control
    /// </summary>
    public class DISourceLookInSource : BaseLookInSource
    {
        #region"--Protected--"

        #region"--Method--"


        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;



            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString = " '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.Search, FilterString, ICType.Source, FieldSelection.Heavy);
            }
            else
            {
                RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Heavy);
            }


            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {

            table.DefaultView.RowFilter = IndicatorClassifications.ICParent_NId + ">0";
            //Dont implement this
            table = table.DefaultView.ToTable();
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            DataRow Row;
            SourceInfo SourceRecord = null;
            SourceBuilder SourceBuilderObj = null;
            DI7MetadataCategoryBuilder SourceMetadataCategoryBuilder = null;
            int ProgressBarValue = 0;

            try
            {                
                // import source metadata categories from source dadtabase
                SourceMetadataCategoryBuilder = new DI7MetadataCategoryBuilder(this._TargetDBConnection, this._TargetDBQueries);
                SourceMetadataCategoryBuilder.ImportAllMetadataCategories(this.SourceDBConnection, this.SourceDBQueries, MetadataElementType.Source);

                //import selected sources
                foreach (string Nid in selectedNids)
                {
                    try
                    {
                        Row = this.SourceTable.Select(this.TagValueColumnName + "=" + Nid)[0];

                        //import indicator 
                        SourceRecord = new SourceInfo();
                        SourceRecord.Name = Row[IndicatorClassifications.ICName].ToString();
                        SourceRecord.Info = Row[IndicatorClassifications.ICInfo].ToString();
                        SourceRecord.ISBN = Row[IndicatorClassifications.ISBN].ToString();
                        SourceRecord.Nature = Row[IndicatorClassifications.Nature].ToString();

                        SourceBuilderObj = new SourceBuilder(this._TargetDBConnection, this._TargetDBQueries);
                        SourceBuilderObj.ImportSource(SourceRecord, Convert.ToInt32(Nid), this.SourceDBQueries, this.SourceDBConnection);

                    }
                    catch (Exception ex)
                    {

                        throw new ApplicationException(ex.ToString());
                    }

                    this.RaiseIncrementProgessBarEvent(ProgressBarValue);
                    ProgressBarValue++;

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();
            this.Columns.Add(IndicatorClassifications.ICName, Constants.LanguageKeys.Source);
            this.TagValueColumnName = IndicatorClassifications.ICNId;
        }

        #endregion

        #endregion

        #endregion

    }
}