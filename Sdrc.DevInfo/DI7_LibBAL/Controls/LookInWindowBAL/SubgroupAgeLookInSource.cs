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
    //public class SubgroupAgeLookInSource : BaseLookInSource
    //{
    //    #region"--Protected--"

    //    #region"--Method--"
       

    //    protected override string GetSqlQuery(string searchString)
    //    {
    //        string RetVal = string.Empty;
    //        string FilterString = string.Empty;

    //        FilterString = Subgroup.SubgroupType + " = " + Convert.ToInt32(SubgroupType.Age);
    //        if (!string.IsNullOrEmpty(searchString))
    //        {

    //            FilterString = FilterString + " and " + Subgroup.SubgroupName + " like '%" + searchString + "%' ";                
    //        }

    //        RetVal = this.SourceDBQueries.Subgroup.GetSubgroup(FilterFieldType.Search, FilterString);
    //        return RetVal;
    //    }

    //    protected override void ProcessDataTable(ref System.Data.DataTable table)
    //    {
    //        //Dont implement this
    //    }

    //    #endregion

    //    #endregion

    //    #region "-- Internal --"

    //    #region "-- Methods --"

    //    #region "-- Import  --"

    //    public override void ImportValues(List<string> selectedNids, bool allSelected)
    //    {
    //        SubgroupBuilder SubgroupBuilderObj = new SubgroupBuilder(this._TargetDBConnection, this._TargetDBQueries);
    //        SubgroupInfo SourceDBSubgroup;
    //        DataRow Row;
    //        int ProgressBarValue = 0;

    //        foreach (string Nid in selectedNids)
    //        {
    //            try
    //            {
    //                //get subgroup from source table
    //                Row = this.SourceTable.Select(Subgroup.SubgroupNId + "=" + Nid)[0];
    //                SourceDBSubgroup = new SubgroupInfo();
    //                SourceDBSubgroup.Name = DICommon.RemoveQuotes(Row[Subgroup.SubgroupName].ToString());
    //                SourceDBSubgroup.GID = Row[Subgroup.SubgroupGId].ToString();
    //                SourceDBSubgroup.Global = Convert.ToBoolean(Row[Subgroup.SubgroupGlobal]);
    //                SourceDBSubgroup.Nid = Convert.ToInt32(Row[Subgroup.SubgroupNId]);
    //                SourceDBSubgroup.Type = SubgroupType.Age;
    //                //import into target database
    //                SubgroupBuilderObj.ImportSubgroup(SourceDBSubgroup, SourceDBSubgroup.Nid, this.SourceDBQueries, this.SourceDBConnection);
    //            }
    //            catch (Exception ex)
    //            {
    //                ExceptionFacade.ThrowException(ex);
    //            }
    //            this.RaiseIncrementProgessBarEvent(ProgressBarValue);
    //            ProgressBarValue++;
    //        }
    //    }

    //    public override void SetColumnsInfo()
    //    {
    //        this.Columns.Clear();
    //        this.Columns.Add(Subgroup.SubgroupName, DILanguage.GetLanguageString(Constants.LanguageKeys.SubgroupAge));
    //        this.TagValueColumnName = Subgroup.SubgroupNId;
    //        this.GlobalValueColumnName1 = Subgroup.SubgroupGlobal;
    //    }
    //    #endregion

    //    #endregion

    //    #endregion
    //}

}
