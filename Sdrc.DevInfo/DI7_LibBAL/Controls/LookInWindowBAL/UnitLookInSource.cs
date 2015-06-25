using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Unit for LookInWindow control
    /// </summary>
    public class UnitLookInSource : BaseLookInSource
    {

       #region"--Protected--"

       #region"--Method--"
              
        protected override string GetSqlQuery(string searchString)
        { 
            string RetVal = string.Empty;
            string FilterString = string.Empty;
            if (!string.IsNullOrEmpty(searchString))
            {
                FilterString = Unit.UnitName + " like '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.Unit.GetUnit(FilterFieldType.Search, "" + FilterString + "");
            }
            else
            {
                RetVal = this.SourceDBQueries.Unit.GetUnit(FilterFieldType.None, string.Empty);
            }
            
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
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            UnitBuilder UnitBuilderObj = new UnitBuilder(this._TargetDBConnection, this._TargetDBQueries);
            UnitInfo SourceDBUnit;
            DataRow Row;
            int ProgressBarValue = 0;

            foreach (string Nid in selectedNids)
            {
                try
                {
                    //get unit from source table
                    Row = this.SourceTable.Select(Unit.UnitNId + "=" + Nid)[0];
                    SourceDBUnit = new UnitInfo();
                    SourceDBUnit.Name = DICommon.RemoveQuotes( Row[Unit.UnitName].ToString());
                    SourceDBUnit.GID = Row[Unit.UnitGId].ToString();
                   SourceDBUnit.Global = Convert.ToBoolean(Row[Unit.UnitGlobal]);
                    SourceDBUnit.Nid=Convert.ToInt32(Row[Unit.UnitNId]);
                    //import into target database
                   UnitBuilderObj.ImportUnit(SourceDBUnit, SourceDBUnit.Nid, this.SourceDBQueries, this.SourceDBConnection);
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
            this.Columns.Add(Unit.UnitName, DILanguage.GetLanguageString(Constants.LanguageKeys.Unit));
            this.TagValueColumnName = Unit.UnitNId;
            this.GlobalValueColumnName1 = Unit.UnitGlobal;
        }


        #endregion

        #endregion

        #endregion
    }
}
