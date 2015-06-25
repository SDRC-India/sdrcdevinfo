using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.RecommendedSourcesBAL
{
    public class RecommendedSourceTimeperiod : RecommendedSourceBase
    {
        public RecommendedSourceTimeperiod()
        {
            this._TagColumnName = Timeperiods.TimePeriodNId;
            this._DisplayColumnName = Timeperiods.TimePeriod;
            this._GlobalColumnName = string.Empty;
        }

        public override DataTable GetDefaultElementsTable()
        {
            DataTable RetVal = null;
            DataView TableView = null;
            string Query = string.Empty;
            try
            {
                Query = this._DBQueries.AutoFill.GetAutoDistinctTimeperiod(this.SeletectedICType,this.UserSelections);
                TableView = this._DBConnection.ExecuteDataTable(Query).DefaultView;
                TableView.Sort = Timeperiods.TimePeriod + " Desc";
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

                Query = this._DBQueries.RecommendedSources.GetAllRecordsByIUSTimeperiodAreaNSource(UserSelections.IndicatorNIds, UserSelections.AreaNIds, NId.ToString(), UserSelections.SourceNIds, UserSelections.ICNIds, this.SeletectedICType);

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
