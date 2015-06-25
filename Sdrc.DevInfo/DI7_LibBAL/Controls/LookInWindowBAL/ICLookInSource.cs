using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting classifications for LookInWindow control
    /// </summary>
    public class ICLookInSource : BaseLookInSource
    {
        #region"-- Private --"

        #region "-- Variables --"

        private int MaxLevel = 0;
        private string SearchString = string.Empty;

        #endregion

        #region"--Method--"

        private void SetIndicatorClassificationLevelName(DataTable table)
        {
            DataRow[] Rows;
            for (int Level = 2; Level <= this.MaxLevel; Level++)
            {
                foreach (DataRow Row in table.Select(Constants.LanguageKeys.Level + "=" + Level))
                {
                    Rows = table.Select(IndicatorClassifications.ICNId + "=" + Row[IndicatorClassifications.ICParent_NId].ToString());
                    for (int i = 1; i <= Level - 1; i++)
                    {
                        Row[Constants.LanguageKeys.Level + " " + i] = Rows[0][Constants.LanguageKeys.Level + " " + i];
                        table.AcceptChanges();
                    }

                }
            }
        }

        private void SetIndicatorClassificationLevel(DataTable table, int parentNid, int parentLevel)
        {
            foreach (DataRow Row in table.Select(IndicatorClassifications.ICParent_NId + "=" + parentNid))
            {
                Row[Constants.LanguageKeys.Level] = parentLevel + 1;
                this.SetIndicatorClassificationLevel(table,
                    Convert.ToInt32(Row[IndicatorClassifications.ICNId]),
                    Convert.ToInt32(Row[Constants.LanguageKeys.Level]));

            }
        }



        #endregion

        #endregion

        #region"--Protected--"

        #region"--Method--"

     
        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;

            //save the search string and do the filtering of records in processDataTable() method
            this.SearchString = searchString;

                RetVal = this.SourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, this._IndicatorClassificationType, FieldSelection.Heavy);
        
            return RetVal;
        }
          
        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            DataTable NewTable;
            DataRow NewRow;
            DataRow[] Rows;
            int Level = 0;

            try
            {

                //Step 1: Set indicator classification levels
                table.Columns.Add(Constants.LanguageKeys.Level, System.Type.GetType("System.Int32"));
                this.SetIndicatorClassificationLevel(table, -1, 0);

                //Step 2: Get maximum level from table 
                table.DefaultView.Sort = Constants.LanguageKeys.Level + " DESC";
                if (table.Rows.Count > 0)
                {
                    this.MaxLevel = Convert.ToInt32(table.DefaultView.ToTable().Rows[0][Constants.LanguageKeys.Level]);
                }
                else
                {
                    this.MaxLevel = 1;
                }

                //Setp 2: Set Columns Info accordingly
                this.SetColumnsInfo();

                //Step 3: Create New Data Table to fill the list view
                NewTable = new DataTable();
                NewTable.Columns.Add(IndicatorClassifications.ICNId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICParent_NId, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(Constants.LanguageKeys.Level, System.Type.GetType("System.Int32"));
                NewTable.Columns.Add(IndicatorClassifications.ICGlobal, System.Type.GetType("System.Boolean"));
                NewTable.Columns.Add(IndicatorClassifications.ICGId);
                NewTable.Columns.Add(IndicatorClassifications.ICInfo);
                NewTable.Columns.Add(IndicatorClassifications.ICName);

                //insert columns for area level info.
                for (Level = 1; Level <= this.MaxLevel; Level++)
                {
                    NewTable.Columns.Add(Constants.LanguageKeys.Level + " " + Level);
                }

                //insert rows into new table
                foreach (DataRow Row in table.Rows)
                {
                    NewRow = NewTable.NewRow();
                    NewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                    NewRow[IndicatorClassifications.ICParent_NId] = Row[IndicatorClassifications.ICParent_NId];
                    NewRow[IndicatorClassifications.ICGlobal] = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    NewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];
                    NewRow[IndicatorClassifications.ICInfo] = Row[IndicatorClassifications.ICInfo];
                    NewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName];
                    NewRow[Constants.LanguageKeys.Level] = Row[Constants.LanguageKeys.Level];

                    NewRow[Constants.LanguageKeys.Level + " " + Convert.ToInt32(Row[Constants.LanguageKeys.Level])] = Row[IndicatorClassifications.ICName].ToString();
                    NewTable.Rows.Add(NewRow);
                }

                NewTable.AcceptChanges();
                this.SetIndicatorClassificationLevelName(NewTable);
                NewTable.DefaultView.Sort = IndicatorClassifications.ICNId;

                //Step 4: If search string is not empty then filter data table 
                if (!string.IsNullOrEmpty(this.SearchString))
                {
                                     NewTable.DefaultView.RowFilter = IndicatorClassifications.ICName + " LIKE '%" + this.SearchString + "%'";

                    NewTable=NewTable.DefaultView.ToTable();
                    
                }

                //Step 5: do the sorting on the basis of available levels
                string SortString = string.Empty;
                for (int i = 1; i <= MaxLevel; i++)
                {
                    if (!string.IsNullOrEmpty( SortString))
                    {
                        SortString += " ,";
                    }
                    SortString += Constants.LanguageKeys.Level + " " + i;
                }

                NewTable.DefaultView.Sort = SortString;
                

                //Step 6: Replace the referenced table with the new data table
                table =  NewTable.DefaultView.ToTable(); 
            }
            catch (Exception ex)
            {   
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion


        #endregion

        #region "-- public --"

        #region "-- New/Dispose --"

        public ICLookInSource(ICType indicatorClassificationType)
        {
            this._IndicatorClassificationType = indicatorClassificationType;
        }

        #endregion

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            int ProgressBarValue = 0;
            IndicatorClassificationInfo SrcClassification;
            DataRow Row;

            foreach (string Nid in selectedNids)
            {
                try
                {
                    //get ic from source table
                    Row = this.SourceTable.Select(IndicatorClassifications.ICNId + "=" + Nid)[0];
                    SrcClassification = new IndicatorClassificationInfo();
                    SrcClassification.Name = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICName].ToString());
                    SrcClassification.GID = Row[IndicatorClassifications.ICGId].ToString();
                    SrcClassification.IsGlobal = Convert.ToBoolean(Row[IndicatorClassifications.ICGlobal]);
                    SrcClassification.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                    if (!Information.IsDBNull(Row[IndicatorClassifications.ICInfo]))
                    {
                        SrcClassification.ClassificationInfo = DICommon.RemoveQuotes(Row[IndicatorClassifications.ICInfo].ToString());
                    }

                    SrcClassification.Parent = new IndicatorClassificationInfo();
                    SrcClassification.Parent.Nid = Convert.ToInt32(Row[IndicatorClassifications.ICParent_NId]);
                    SrcClassification.Type = this._IndicatorClassificationType;

                    //import into target database
                    Utility.CreateClassificationChainFromExtDB(
                        SrcClassification.Nid,
                        SrcClassification.Parent.Nid,
                        SrcClassification.GID,
                        SrcClassification.Name,
                        SrcClassification.Type,
                        SrcClassification.ClassificationInfo,
                        SrcClassification.IsGlobal,
                        this.SourceDBQueries, this.SourceDBConnection, this._TargetDBQueries, this._TargetDBConnection);

                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
                this.RaiseIncrementProgessBarEvent(ProgressBarValue);
                ProgressBarValue++;
            }
        }


        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();
            for (int Level = 1; Level <= this.MaxLevel; Level++)
            {
                this.Columns.Add(Constants.LanguageKeys.Level + " " + Level, DILanguage.GetLanguageString(Constants.LanguageKeys.Level) + " " + Level);
            }
            this.TagValueColumnName = IndicatorClassifications.ICNId;
            this.GlobalValueColumnName1 = IndicatorClassifications.ICGlobal;
        }

        #endregion

        #endregion

        #endregion

    }
}
