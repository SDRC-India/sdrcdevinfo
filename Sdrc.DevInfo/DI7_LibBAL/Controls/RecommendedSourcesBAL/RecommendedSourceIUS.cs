using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.RecommendedSourcesBAL
{
    
    public class RecommendedSourceIUS : RecommendedSourceBase
    {

        public RecommendedSourceIUS()
        {
            this._TagColumnName = Indicator.IndicatorNId;
            this._DisplayColumnName = Indicator.IndicatorName;
            this._GlobalColumnName = Indicator.IndicatorGlobal;
        }

        public override DataTable GetDefaultElementsTable()
        {
            DataTable RetVal = null;
            DataView TableView = null;
            string Query = string.Empty;
            try
            {
                Query = this._DBQueries.AutoFill.GetAutoDistinctIUS(this.SeletectedICType, this.UserSelections);

                TableView = this._DBConnection.ExecuteDataTable(Query).DefaultView;
                TableView.Sort = Indicator.IndicatorName + " DESC," + Unit.UnitName + " DESC," + SubgroupVals.SubgroupVal + " DESC";
                RetVal = TableView.ToTable();

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        public override DataTable GetAllRecordsFrmData(int NId)
        {
            DataTable RetVal = null;
            string Query = string.Empty;

            try
            {

                //GetAllRecordsByIUSTimeperiodAreaNSource(UserSelections.IndicatorNIds,UserSelections.AreaNIds,NId, UserSelections.SourceNIds)
                Query = this._DBQueries.RecommendedSources.GetAllRecordsByIUSTimeperiodAreaNSource(NId.ToString(), UserSelections.AreaNIds, UserSelections.TimePeriodNIds, UserSelections.SourceNIds,UserSelections.ICNIds,this.SeletectedICType);
                RetVal = this._DBConnection.ExecuteDataTable(Query);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }
    }
}
