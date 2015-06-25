using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using System.Data;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting Subgroup types for LookInWindow control
    /// </summary>
    public class DISubgroupTypeLookInSource : BaseLookInSource
    {
        #region"--Protected--"

        #region"--Method--"
       

        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {

                FilterString = " AND " + SubgroupTypes.SubgroupTypeName + " like '%" + searchString + "%' ";
            }
            
            RetVal = this.SourceDBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.Search, FilterString);

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
            DI6SubgroupTypeBuilder SGTypeBuilderObj = new DI6SubgroupTypeBuilder(this._TargetDBConnection, this._TargetDBQueries);
            DI6SubgroupTypeInfo SourceDBSubgroupType;
          
            DataRow Row;
            int ProgressBarValue = 0;
                   

            foreach (string Nid in selectedNids)
            {
                try
                {
                    //get subgroup Type from source table
                    Row = this.SourceTable.Select(SubgroupTypes.SubgroupTypeNId + "=" + Nid)[0];
                    SourceDBSubgroupType = new DI6SubgroupTypeInfo();
                    SourceDBSubgroupType.Name = DICommon.RemoveQuotes(Row[SubgroupTypes.SubgroupTypeName].ToString());
                    SourceDBSubgroupType.GID = Row[SubgroupTypes.SubgroupTypeGID].ToString();
                    SourceDBSubgroupType.Global = Convert.ToBoolean(Row[SubgroupTypes.SubgroupTypeGlobal]);
                    SourceDBSubgroupType.Nid = Convert.ToInt32(Row[SubgroupTypes.SubgroupTypeNId]);
                    SourceDBSubgroupType.Order = Convert.ToInt32(Row[SubgroupTypes.SubgroupTypeOrder]);

                    //import into target database
                    SGTypeBuilderObj.ImportSubgroupType(SourceDBSubgroupType, SourceDBSubgroupType.Nid, this.SourceDBQueries, this.SourceDBConnection);
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
            this.Columns.Add(SubgroupTypes.SubgroupTypeName, DILanguage.GetLanguageString(Constants.LanguageKeys.SubgroupType));
            this.TagValueColumnName = SubgroupTypes.SubgroupTypeNId;
            this.GlobalValueColumnName1 = SubgroupTypes.SubgroupTypeGlobal;
        }
        #endregion

        #endregion

        #endregion
    }

}
