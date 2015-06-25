using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Controls.RegistrySearchControlBAL
{
    public class SearchSubgroups : SearchBase
    {

        #region "-- Private --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"


        private string GetSubgroupValNIds(string[] keywords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- new/dispose --"

        public SearchSubgroups()
        {
            this._GIdColumnName = SubgroupVals.SubgroupValNId;
            this._NameColumnName = SubgroupVals.SubgroupVal;
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
                RetVal = this.GetSubgroupValNIds(keywords);
            }

            return RetVal;
        }

        public override DataTable GetTableForSearchText(string searchText)
        {
            DataTable RetVal = null;

            return RetVal;
        }

        #endregion

        #endregion


    }
}

