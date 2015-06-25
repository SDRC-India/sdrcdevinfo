using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting footnote for LookInWindow control
    /// </summary>
    public class FootnoteLookInSource : BaseLookInSource
    {
        #region"--Protected--"

        #region"--Method--"
        
        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                FilterString = FootNotes.FootNote + " like '%" + searchString + "%' ";                                
                RetVal = this.SourceDBQueries.Footnote.GetFootnote(FilterFieldType.Search, FilterString);
            }
            else
            {
                FilterString = FootNotes.FootNoteNId + ">0 ";
                RetVal = this.SourceDBQueries.Footnote.GetFootnote(FilterFieldType.Search, FilterString);
            }

            RetVal += " Order by " + FootNotes.FootNote;

            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //Dont implement this
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
            DataRow Row;            
            FootnoteInfo FootnoteRecord = null;
            FootnoteBuilder FootnoteBuilderObj = null;           
            int ProgressBarValue = 0;

            try
            {   
                //import selected footnotes
                foreach (string Nid in SelectedNids)
                {
                    try
                    {
                        Row = this.SourceTable.Select(this.TagValueColumnName + "=" + Nid)[0];

                        //import footnote
                        FootnoteRecord = new FootnoteInfo();
                        FootnoteRecord.Name = Convert.ToString(Row[FootNotes.FootNote]);
                        FootnoteRecord.Nid = Convert.ToInt32(Row[FootNotes.FootNoteNId]);
                        FootnoteRecord.GID = Convert.ToString(Row[FootNotes.FootNoteGId]);

                        FootnoteBuilderObj = new FootnoteBuilder(this._TargetDBConnection, this._TargetDBQueries);
                        FootnoteBuilderObj.CheckNCreateFoonote(FootnoteRecord);

                        //FootnoteBuilderObj.ImportFootnote(FootnoteRecord, Convert.ToInt32(Nid), this.SourceDBQueries, this.SourceDBConnection);
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
            this.Columns.Add(FootNotes.FootNote, DevInfo.Lib.DI_LibBAL.Utility.DILanguage.GetLanguageString(Constants.LanguageKeys.Footnote));
            this.TagValueColumnName = FootNotes.FootNoteNId;
            this.GlobalValueColumnName1 = string.Empty;
        }

        #endregion

        #endregion

        #endregion
    }
}
