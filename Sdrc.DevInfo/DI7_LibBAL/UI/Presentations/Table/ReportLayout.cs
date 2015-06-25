using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Table
{
    /// <summary>
    /// Provides the function which executes Query and Retuns data.
    /// </summary>
    public static class ReportLayout
    {
        //Returns the requires area levels  
        public static DataTable GetAreaLevels(DIConnection dBConnection, DIQueries dBQueries)
        {
            DataTable dtAreaLevel = null;
            try
            {
                dtAreaLevel = dBConnection.ExecuteDataTable(dBQueries.Area.GetAreaLevel(FilterFieldType.None, string.Empty));
            }
            catch (Exception)
            {
            }
            return dtAreaLevel;
        }

        //Returns the all time periods  
        public static DataTable GetAllTimePeriods(DIConnection dBConnection, DIQueries dBQueries)
        {
            DataTable dtTimePeriod=null;
            try
            {
                dtTimePeriod = dBConnection.ExecuteDataTable(dBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty, Timeperiods.TimePeriod + " DESC"));
            }
            catch (Exception)
            {
              
            }
            return dtTimePeriod;
            
        }
    }
}
