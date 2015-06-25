using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;


namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting subgroup val for LookInWindow control
    /// </summary>
    public class SubgroupValLookInSource : BaseLookInSource
   {
       #region"--Protected--"

       #region"--Method--"

       
        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString = SubgroupVals.SubgroupVal + " like '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.Search, FilterString);
            }
            else
            {
                RetVal = this.SourceDBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None,string.Empty);
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
           DI6SubgroupValBuilder SubgroupValBuilderObj = new DI6SubgroupValBuilder(this._TargetDBConnection, this._TargetDBQueries);
            int ProgressBarValue = 0;

           foreach (string Nid in selectedNids)
           {
               try
               {
                   //import into target database
                   SubgroupValBuilderObj.ImportSubgroupVal(Convert.ToInt32(Nid),this.SourceDBQueries, this.SourceDBConnection);
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
            this.Columns.Add(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString(Constants.LanguageKeys.SubgroupVal));
            this.TagValueColumnName = SubgroupVals.SubgroupValNId;
            this.GlobalValueColumnName1 = SubgroupVals.SubgroupValGlobal;
        }

       #endregion

       #endregion

       #endregion
    }
}
