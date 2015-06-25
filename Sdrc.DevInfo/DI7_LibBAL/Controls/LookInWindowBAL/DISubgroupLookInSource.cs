using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Subgroup for LookInWindow control
    /// </summary>
    public class DISubgroupLookInSource : BaseLookInSource
    {
        #region"--Protected--"

        #region"--Method--"

        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString =  Subgroup.SubgroupName + " like '%" + searchString + "%' ";
            }

            RetVal = this.SourceDBQueries.Subgroup.GetSubgroup(FilterFieldType.Search, FilterString);

            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //Dont implement this
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
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            DI6SubgroupBuilder SGBuilderObj = new DI6SubgroupBuilder(this._TargetDBConnection, this._TargetDBQueries);
            DI6SubgroupInfo SourceDBSubgroup;
            DataRow Row;
            int ProgressBarValue = 0;


            foreach (string Nid in selectedNids)
            {
                try
                {
                    //get subgroup from source table
                    Row = this.SourceTable.Select(Subgroup.SubgroupNId + "=" + Nid)[0];
                    SourceDBSubgroup = new DI6SubgroupInfo();
                    SourceDBSubgroup.Name = DICommon.RemoveQuotes(Row[Subgroup.SubgroupName].ToString());
                    SourceDBSubgroup.GID = Row[Subgroup.SubgroupGId].ToString();
                    SourceDBSubgroup.Global = Convert.ToBoolean(Row[Subgroup.SubgroupGlobal]);
                    SourceDBSubgroup.Nid = Convert.ToInt32(Row[Subgroup.SubgroupNId]);
                    SourceDBSubgroup.Type = Convert.ToInt32(Row[Subgroup.SubgroupType]);

                    //import into target database
                    SGBuilderObj.ImportSubgroup(SourceDBSubgroup, SourceDBSubgroup.Nid, this.SourceDBQueries, this.SourceDBConnection);

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
            this.Columns.Add(Subgroup.SubgroupName, DILanguage.GetLanguageString(Constants.LanguageKeys.SubgroupDimension));
            this.TagValueColumnName = Subgroup.SubgroupNId;
            this.GlobalValueColumnName1 = Subgroup.SubgroupGlobal;
        }

        #endregion

        #endregion

        #endregion
    }

}
