using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries.Notes;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// Provides collection of SQL queries used for importing RecommendedSourceQueries from database or template
    /// </summary>
    internal class RecommendedSourceQueries
    {
        # region " -- Private -- "

        # region " -- Variables -- "

        private DIQueries DBQueries;

        # endregion

        # endregion

        # region " -- Internal -- "

        # region " -- New /Dispose -- "

        internal RecommendedSourceQueries(DIQueries DBQueries)
        {
            this.DBQueries = DBQueries;
        }


        # endregion

        #region "-- Methods --"

        internal string GetDistinctSourceFileFromTempDataTable()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Constants.Log.SkippedSourceFileColumnName + " from " + Constants.TempDataTableName; ;
            return RetVal;
        }

        # endregion

        # endregion
    }

}
