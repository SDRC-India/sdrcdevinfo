using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.RecommendedSourcesBAL
{
    public class RecommendedSourceArea : RecommendedSourceBase
    {
        public RecommendedSourceArea()
        {
            this._TagColumnName = Area.AreaNId;
            this._DisplayColumnName = Area.AreaName;
            this._GlobalColumnName = Area.AreaGlobal;
        }

        public override DataTable GetDefaultElementsTable()
        {
            DataTable RetVal=null;
            DataView TableView=null;
            string Query = string.Empty;
            try
            {
                Query = this._DBQueries.AutoFill.GetAutoDistinctArea(this.SeletectedICType , this.UserSelections);
                RetVal = this._DBConnection.ExecuteDataTable(Query);
                ////TableView.Sort = Area.AreaName + " Desc";
                ////RetVal = TableView.ToTable();
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

                Query = this._DBQueries.RecommendedSources.GetAllRecordsByIUSTimeperiodAreaNSource(UserSelections.IndicatorNIds, NId.ToString(), UserSelections.TimePeriodNIds, UserSelections.SourceNIds, UserSelections.ICNIds, this.SeletectedICType);
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
