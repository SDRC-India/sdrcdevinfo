using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.Controls.RegistrySearchControlBAL
{

    public abstract class SearchBase
    {
        #region "-- public --"

        #region "-- Variables/properties --"

        private DIConnection _DBConnection = null;
        /// <summary>
        /// Get or Set DIConnection object
        /// </summary>
        public DIConnection DBConnection
        {
            get { return this._DBConnection; }
            set { this._DBConnection = value; }
        }

        private DIQueries _DBQueries = null;
        /// <summary>
        /// Get or Set DIQueries object
        /// </summary>
        public DIQueries DBQueries
        {
            get { return this._DBQueries; }
            set { this._DBQueries = value; }
        }


        protected string _GIdColumnName = string.Empty;

        /// <summary>
        /// Get or Set GId ColumnName
        /// </summary>
        public string GIdColumnName
        {
            get { return this._GIdColumnName; }
        }

        protected string _NameColumnName = string.Empty;

        /// <summary>
        /// Get or Set Name ColumnName
        /// </summary>
        public string NameColumnName
        {
            get { return this._NameColumnName; }
        }

        protected string _GlobalValueColumnName;

        /// <summary>
        /// Gets global value column name
        /// </summary>
        public string GlobalValueColumnName
        {
            get
            {
                return this._GlobalValueColumnName;
            }
        }

        protected string _NameColumnHeader;
        /// <summary>
        /// Gets or Sets first column header;
        /// </summary>
        public string NameColumnNameHeader
        {
            get
            {
                return this._NameColumnHeader;
            }
        }


        #endregion

        #region "-- Methods --"

        public abstract string GetNidsForSearchText(string searchText);

        public abstract DataTable GetTableForSearchText(string searchText);

        protected string[] GetAbsoluteKeyword(string keyword)
        {
            string[] RetVal;
            string[] TempSearchPattern;

            // -- Split keywords  at quotes  and get it in SearchPattern"
            // Empty entries are required as part of logic
            string[] SearchPattern = keyword.Split("\"".ToCharArray());
            // string[] SearchPattern = keyword.Split("\"".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
            if (IsEvenNumber(SearchPattern.Length))
            {
                //--- Quotes to be ignored
                //SearchPattern = keyword.Split(' ');
                SearchPattern = keyword.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                //--- Quotes to be considered
                string[] TempSearch;
                //  string[] TempStorage;
                List<string> TempStorage = new List<string>();
                for (int i = 0; i <= SearchPattern.Length - 1; i++)
                {
                    if (SearchPattern[i].Length > 0)
                    {
                        if (IsEvenNumber(i))
                        {
                            //--- is string that is not withing ""  
                            //TempSearch = SearchPattern[i].Split(' ');
                            TempSearch = SearchPattern[i].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                            for (int j = 0; j <= TempSearch.Length - 1; j++)
                            {
                                if (TempSearch[j].Length > 0)
                                {
                                    TempStorage.Add(TempSearch[j]);
                                }
                            }
                        }
                        else
                        {
                            if (SearchPattern[i].Length > 0)
                            {
                                // Change on 18-03-08 . values inside quotes will be matched exactly
                                //TempStorage.Add(SearchPattern[i]);
                                TempStorage.Add("'" + SearchPattern[i] + "'");
                                //-- end of Change on 18-03-08 . values inside quotes will be matched exactly

                            }

                        }
                    }
                }
                TempSearchPattern = TempStorage.ToArray();
                SearchPattern = new string[TempSearchPattern.Length];
                TempSearchPattern.CopyTo(SearchPattern, 0);
            }
            RetVal = SearchPattern;
            return RetVal;
        }

        private Boolean IsEvenNumber(Int32 number)
        {
            int result = 0;
            Math.DivRem(number, 2, out result);
            if (result == 0)
            {
                return true;
            }
            else
                return false;
        }

        #endregion

        #endregion

    }
}