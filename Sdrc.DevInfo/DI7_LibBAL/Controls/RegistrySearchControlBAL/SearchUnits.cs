using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility.Search;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Controls.RegistrySearchControlBAL
{
    public class SearchUnits : SearchBase
    {

        #region "-- Private --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"        

        private string GetUnitNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            String AllIndicatorNIdsWithData = String.Empty;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)
            // -- Adding each element of SearchString  into FilterClause
            for (int i = 0; i <= searchString.Length - 1; i++)
            {
                if (FilterClause.Length > 0)
                {
                    FilterClause += " OR ";
                }
                //change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
                // Change on 14-07-08 .Like search instead of equal in case of phrase search.
                // When search text is written inside double quote.
                // Use search text after removing quotes from first and last position in like claues
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += Indicator.IndicatorName + " =" + DICommon.EscapeWildcardChar(searchString[i]) ;
                    FilterClause += Unit.UnitName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += Unit.UnitName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
            }

            //Running Query to get IndicatorNId's    
            sSql = this.DBQueries.Unit.GetUnit(FilterFieldType.Search, FilterClause);
            try
            {
                rd = this.DBConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Unit.UnitNId);
                rd.Close();
            }
            catch (Exception ex)
            {
                //rd.Close();
            }


            return RetVal;
        }

        #endregion

        #endregion


        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- new/dispose --"

        public SearchUnits()
        {
            this._GIdColumnName = Unit.UnitNId;
            this._NameColumnName = Unit.UnitName;
            this._GlobalValueColumnName = Unit.UnitGlobal;

            this._NameColumnHeader = DILanguage.GetLanguageString("UNIT");

        }

        #endregion

        #region "-- Methods --"

        public override string GetNidsForSearchText(string searchText)
        {
            string RetVal = string.Empty;
            string[] keywords;
            keywords = this.GetAbsoluteKeyword(searchText);
            if (keywords.Length >= 1)
            {
                RetVal = this.GetUnitNIds(keywords);
            }

            return RetVal;
        }

        public override DataTable GetTableForSearchText(string searchText)
        {
            DataTable RetVal = null;
            string UnitNids = this.GetNidsForSearchText(searchText);

            if (!string.IsNullOrEmpty(UnitNids))
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Unit.GetUnit(FilterFieldType.NId, UnitNids));
            }

            return RetVal;
        }

        #endregion

        #endregion


    }
}
