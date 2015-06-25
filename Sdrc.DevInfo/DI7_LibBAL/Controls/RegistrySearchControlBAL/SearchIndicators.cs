using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility.Search;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Controls.RegistrySearchControlBAL
{
    public class SearchIndicators : SearchBase
    {
        #region "-- Private --"

        #region "-- Variables --"

        private FreeText FreeTextObj = null;

        #endregion

        #region "-- Methods --"

        #endregion

        #endregion
        
        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- new/dispose --"

        public SearchIndicators(DIConnection dbConnection,DIQueries dbQueries)
        {
            this._GIdColumnName = Indicator.IndicatorNId;
            this._NameColumnName = Indicator.IndicatorName;
            this._GlobalValueColumnName = Indicator.IndicatorGlobal;
            
            this._NameColumnHeader = DILanguage.GetLanguageString("INDICATOR");
            
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;

            FreeTextObj = new FreeText(dbConnection, dbQueries);
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Get Nids for search text
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public override string GetNidsForSearchText(string searchText)
        {
            string RetVal = string.Empty;

            // Set Indicator_IUS_option.
            this.FreeTextObj.ShowIUS = false;
            // Set GetIndicatorWithName
            this.FreeTextObj.GetIndicatorByDataOnly = false;

            // Call simple search
            this.FreeTextObj.SimpleSearch(searchText, FreeText.SearchType.Indicator);

            RetVal = FreeTextObj.Indicator_IUS_NId;

            return RetVal;
        }

        public override DataTable GetTableForSearchText(string searchText)
        {
            DataTable RetVal = null;
            string IndicatorNids = this.GetNidsForSearchText(searchText);

            try
            {
                
                if (!string.IsNullOrEmpty(IndicatorNids))
                {
                    RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.NId, IndicatorNids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light));

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        #endregion

        #endregion




    }
}
