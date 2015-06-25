using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.Common;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Gallery.Search.DBBuild;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Gallery.Search.Search
{
    /// <summary>
    /// Class for searhing in database Gallery. For searching call StartGallerySearch Method
    /// Use by making object by passing gallery Database in constructor .
    /// or
    /// Create object using default constructor and then set GalleryDatabase Property
    /// </summary>
    /// <example>
    /// Dim GalleryDatabashPath As String = IO.Path.Combine(gsSystemFolder, "GalleryPresentation.mdb")
    /// Dim BALGallerySearch As GallerySearch= New GallerySearch(GalleryDatabashPath)
    /// /// 
    /// </example>
   public class GallerySearch
    {
        #region " -- Private -- "

        #region " -- Variable -- "
        private DIConnection DBConnection;
        private string PresType = PRES_TYPE_ALL;
        string[] keywords;
        //Store Comma Delimited list of search result in forms of Presentaion Nids
       private String ResultNIds = String.Empty;
      
       bool IsDisposed = false;

        #endregion

        #region " --Constants -- "

        const string WEIGHTAGE_COL1 = "Weightage1";
        const string WEIGHTAGE_COL2 = "Weightage2";
        //const string PRES_TITLES = "Pres_titles";
        const string PRES_FILENAME = "Pres_FileName";
        const string PRES_TYPE = "Pres_Type";
        const string PRES_UKEYWORDS = "Pres_UKeywords";
        const string PRES_KEYWORDS = "Pres_Keywords";
       
        const string PRES_TYPE_ALL = "A";
        const string PRES_TYPE_TABLE = "T";
        const string PRES_TYPE_GRAPH = "G";
        const string PRES_TYPE_MAP = "M";
        const string PRES_TYPE_PROFILE = "P";

        const string Id = "Id";
        const string File = "File";
        const string Type = "Type";
        //const string GalleryId= "GalleryId";                  
        //const string Path = "Path";
        const string WordCount = "WordCount";
        const string SearchKeyword = "SearchKeyword";
        #endregion

        #region " -- Methods -- "


        /// <summary>
        /// Match Keyword with PDS_keyword in PreSearch Table
        /// </summary>
        private Boolean PreSearch()
        {
            Boolean RetVal = false;
            string sSql = "";
            System.Data.IDataReader rd;
            //  sSql Represent query for search in Presearches table
            sSql = "SELECT " +PreSearches.PDS_Presentation_NIds+" FROM "+DBTable.UT_PreSearches+" where "+PreSearches.PDS_Keyword+" = ' " + SearchString + "' ";
            rd = this.DBConnection.ExecuteReader(sSql);
            ResultNIds = DIConnection.GetDelimitedValuesFromReader(rd, PreSearches.PDS_Presentation_NIds);
            // Return true if keyword found in presearches table
            if (!string.IsNullOrEmpty(ResultNIds))
            {
                RetVal = true;
            }
            return RetVal;
        }

        ///// <summary>
        ///// Main Search Method (For New Search)
        ///// </summary>
        //private void StartNewSearch()
        //{
        //    String SortedPresNIds = String.Empty;

        //    //Get Sorted Presentation NIDs
        //    SortedPresNIds = SetRankedResultNIds();

        //    // If ResultNIDs is not Blank,Gett  search result( Nid,PresFilename from PresMaster)
        //    if (!string.IsNullOrEmpty(SortedPresNIds))
        //    {
        //        _SearchResult = GetResultInXML(SortedPresNIds);
        //    }
        //    //If No record Found return a blank Xml 
        //    else
        //    {
        //        _SearchResult = GetXMLDoc("root");
        //    }
        //}

        /// <summary>
        /// Main Search Method (For New Search)
        /// </summary>
        private void StartNewSearch(int pageNo,int pageSize)
        {
            String SortedPresNIds = String.Empty;

            //Get Sorted Presentation NIDs
            SortedPresNIds = SetRankedResultNIds();

            // If ResultNIDs is not Blank,Gett  search result( Nid,PresFilename from PresMaster)
            if (!string.IsNullOrEmpty(SortedPresNIds))
            {
                this._SearchResult = GetResultInXML(SortedPresNIds,pageNo,pageSize);
            }
            //If No record Found return a blank Xml 
            else
            {
                this._SearchResult = GetXMLDoc("root");
            }
        }
     
       private string GetAllSearchResultNIds()
       {
           String RetVal = string.Empty;
           try
           {
               string sSql = string.Empty;
               string FilterClause = string.Empty;
               System.Data.IDataReader rd;


               // Common Search clause
               sSql = " SELECT DISTINCT " + PresentationKeywords.Pres_NId + " FROM " + DBTable.UT_PresKeyword + " WHERE 1=1 ";

               // FILTER - Search Type (T, G, M or All)
               if (_SearchFor != SearchType.All)
               {
                   sSql += " AND " + PresentationKeywords.Pres_Type + "='" + PresType + "' ";
               }
               

               // Get Search results into a readr
               rd = this.DBConnection.ExecuteReader(sSql);
               // Get all PResetation NIDs comma separated
               RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
               // close the reader
               rd.Close();

               // FILTER - Gallery ID
               if (RetVal.Length > 0 && this._GalleryIdForSearch != -1)
               {
                   sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE "
                   + PresentationMaster.Pres_NId + " IN (" + RetVal + ") AND " + PresentationMaster.GalleryId + " = " + this._GalleryIdForSearch;

                   // Get the resuls
                   rd = this.DBConnection.ExecuteReader(sSql);
                   RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return RetVal;
       }

        /// <summary>
        ///  By performing new Search return  unsorted result as comma seperated list of Presentaion Nids  
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        /// <returns></returns>
        private string GetResultNIds(string[] searchString)
        {
            String RetVal = string.Empty;
            string sSql = string.Empty;
            string FilterClause = string.Empty;
            System.Data.IDataReader rd;
            int StartIndex = 0;
            int EndIndex = 0;
            if (searchString != null)
            {
                 //If search string is very large then there will be an error : Query too complex.So
                // At a time process 25 keywords only
                while (EndIndex < searchString.Length)
                {
                    FilterClause = string.Empty;
                    sSql = string.Empty;
                    // Reset Start and end index for filter clause
                    StartIndex = EndIndex;
                    EndIndex += 25;
                    if (EndIndex > searchString.Length)
                    {
                        EndIndex = searchString.Length;
                    }

                    try
                    {
                        // Common Search clause
                        sSql = " SELECT  DISTINCT " + PresentationKeywords.Pres_NId + " FROM " + DBTable.UT_PresKeyword + " WHERE 1=1 ";

                        // FILTER - Search Type (T, G, M or All)
                        if (_SearchFor != SearchType.All)
                        {
                            sSql += " AND " + PresentationKeywords.Pres_Type + "='" + PresType + "' ";
                        }
                        // FILTER - Keywords
                        //---------------------
                        //------------------------
                        if (searchString != null && searchString.Length > 0)
                        {

                            // -- Adding each element of SearchString  into FilterClause
                            for (int i = StartIndex; i <= EndIndex - 1; i++)
                            {
                                if (searchString[i].ToString() != "&")
                                {
                                    if (FilterClause.Length > 0)
                                    {
                                        FilterClause += " OR ";
                                    }

                                    FilterClause += " Pres_Titles LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%' OR " +
                                                  " Pres_Ukeywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%' OR " +
                                                  " Pres_keywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";

                                }
                            }

                            //sSql Represent query for search in presKeyword table
                            sSql = sSql + " AND (" + FilterClause + ") ";
                        }

                        // Get Search results into a readr
                        rd = this.DBConnection.ExecuteReader(sSql);
                        // Get all PResetation NIDs comma separated
                        if (string.IsNullOrEmpty(RetVal))
                        {
                            RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);

                        }
                        // If RetVal already have nid then check is this result nid already exist in Retval
                        // If not in RetVal then add it
                        else
                        {
                            string ThisSearchResultNids = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                            if (!string.IsNullOrEmpty(ThisSearchResultNids))
                            {
                                // there may be more than one result nid so split  and
                                // match each nid in retval
                                string[] SearchResultArray = ThisSearchResultNids.Split(',');
                                foreach (String resultNId in SearchResultArray)
                                {
                                    if (!RetVal.Contains(resultNId))
                                    {
                                        //RetVal += "," + ThisSearchResultNids;
                                        RetVal += "," + resultNId;
                                    }
                                }
                            }
                        }

                        // close the reader
                        rd.Close();

                        // FILTER - Gallery ID
                        if (RetVal.Length > 0 && this._GalleryIdForSearch != -1)
                        {
                            sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE "
                            + PresentationMaster.Pres_NId + " IN (" + RetVal + ") AND " + PresentationMaster.GalleryId + " = " + this._GalleryIdForSearch;

                            // Get the resuls
                            rd = this.DBConnection.ExecuteReader(sSql);
                            RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            else
            {
                RetVal = this.GetAllSearchResultNIds();
            }
            return RetVal;
        }

        /// <summary>
        ///  By performing new Search return  unsorted result as comma seperated list of Presentaion Nids  
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        /// <returns></returns>
        private void GetResultNIdsUsingRecursiveSearch(string searchKeywords)
        {
            searchKeywords = Utility.Search.FreeText.RemoveDoubleSpace(searchKeywords);
            string[] searchString = this.GetAbsoluteKeyword(searchKeywords);

            String SearchResults = string.Empty;
            Int32 iWordCount = 0;
            //*****************************************Search Logic****************************
            // STEP 1: Get All Possible search combinations
            // will return the DataTable containing the Keywords and the wod count in separate columns
            // string[,] PossibleKeywordCombinations = GetAllSearchCombinations(keywords);                
            // STEP 2: Sort the Data Table by Word count Descending

            // STEP 3: For each item in the Data table, perform the search
            // STEP 3.1 If search succeeded and iWordCount=WordCount of this row 
            // THEN do searching 
            // ELSE CONTINUE

            // STEP 3.2 If search succeeded and iWordCount<>WordCount of this row 
            // THEN TRIM mainSearh string and do RECURSSION
            // ELSE CONTINUE
            //*****************************************Search Logic****************************
            try
            {
                if (searchString != null)
                {
                    iWordCount = searchString.Length;
                    // STEP 1 & 2              
                    //Get All Possible search combinations. Sort the Data Table by Word count Descending
                    DataTable PossibleKeywordCombinationsDT =Utility.Search.FreeText.GetAllSearchCombinationsDataTable(searchString);
                    // STEP 3: For each item in the Data table, perform the search
                    foreach (DataRow DRow in PossibleKeywordCombinationsDT.Rows)
                    {
                        // Get CurrentSearchString
                        string CurrentSearchString = DRow[SearchKeyword].ToString();

                        SearchResults = this.GetResultNIds(CurrentSearchString);

                        // Chk For Sucessful Search
                        if (!string.IsNullOrEmpty(SearchResults))
                        {
                            // STEP 3.1 If search succeeded and iWordCount=WordCount of this row 
                            if (Convert.ToInt32(DRow[WordCount]) == iWordCount)
                            {
                                ResultNIds = Utility.Search.FreeText.CheckAndUpdateNewNIds(ResultNIds, SearchResults);
                            }
                            else
                            {
                                //STEP 3.2 If search succeeded and iWordCount<>WordCount of this row 
                                ResultNIds =Utility.Search.FreeText.CheckAndUpdateNewNIds(ResultNIds, SearchResults);

                                // STEP 3.3 : Trim current keyword from SearchString
                                string Tempkeyword = searchKeywords.Replace(DRow[SearchKeyword].ToString().Trim(), "").Trim();

                                // Recursive Call
                                this.GetResultNIdsUsingRecursiveSearch(Tempkeyword);
                                break;
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///  By performing new Search return  unsorted result as comma seperated list of Presentaion Nids  
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        /// <returns></returns>
        private string GetResultNIds(string searchString)
        {
            String RetVal = string.Empty;
            string sSql = string.Empty;
            string FilterClause = string.Empty;
            System.Data.IDataReader rd;
           
            if (!string.IsNullOrEmpty(searchString) != null)
            {   
                    FilterClause = string.Empty;
                    sSql = string.Empty;
                   
                    try
                    {
                        // Common Search clause
                        sSql = " SELECT  DISTINCT " + PresentationKeywords.Pres_NId + " FROM " + DBTable.UT_PresKeyword + " WHERE 1=1 ";

                        // FILTER - Search Type (T, G, M or All)
                        if (_SearchFor != SearchType.All)
                        {
                            sSql += " AND " + PresentationKeywords.Pres_Type + "='" + PresType + "' ";
                        }
                        // FILTER - Keywords
                        //---------------------
                        //------------------------
                        if (!string.IsNullOrEmpty(searchString))
                        {
                            // -- Adding each element of SearchString  into FilterClause                            
                            if (searchString != "&")
                            {
                                    if (FilterClause.Length > 0)
                                    {
                                        FilterClause += " OR ";
                                    }

                                    FilterClause += " Pres_Titles LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%' OR " +
                                                  " Pres_Ukeywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%' OR " +
                                                  " Pres_keywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

                            }
                            

                            //sSql Represent query for search in presKeyword table
                            sSql = sSql + " AND (" + FilterClause + ") ";
                        }

                        // Get Search results into a readr
                        rd = this.DBConnection.ExecuteReader(sSql);
                        // Get all PResetation NIDs comma separated
                        if (string.IsNullOrEmpty(RetVal))
                        {
                            RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);

                        }
                        // If RetVal already have nid then check is this result nid already exist in Retval
                        // If not in RetVal then add it
                        else
                        {
                            string ThisSearchResultNids = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                            if (!string.IsNullOrEmpty(ThisSearchResultNids))
                            {
                                // there may be more than one result nid so split  and
                                // match each nid in retval
                                string[] SearchResultArray = ThisSearchResultNids.Split(',');
                                foreach (String resultNId in SearchResultArray)
                                {
                                    if (!RetVal.Contains(resultNId))
                                    {
                                        //RetVal += "," + ThisSearchResultNids;
                                        RetVal += "," + resultNId;
                                    }
                                }
                            }
                        }

                        // close the reader
                        rd.Close();

                        // FILTER - Gallery ID
                        if (RetVal.Length > 0 && this._GalleryIdForSearch != -1)
                        {
                            sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE "
                            + PresentationMaster.Pres_NId + " IN (" + RetVal + ") AND " + PresentationMaster.GalleryId + " = " + this._GalleryIdForSearch;

                            // Get the resuls
                            rd = this.DBConnection.ExecuteReader(sSql);
                            RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                //}
            }
            else
            {
                RetVal = this.GetAllSearchResultNIds();
            }
            return RetVal;
        }


        ///// <summary>
        /////  Get All Possible search combinations in a data table
        ///// </summary>
        ///// <param name="keywords"></param>
        ///// <returns></returns>
        //private DataTable GetAllSearchCombinationsDataTable(string[] keywords)
        //{
        //    DataTable RetVal = new DataTable();
        //    // Add Word cout and SearchString columns
        //    RetVal.Columns.Add(WordCount);
        //    RetVal.Columns.Add(SearchKeyword);

        //    string PrevWord = string.Empty;

        //    int index = 0;
        //    for (int i = 0; i < keywords.Length; i++)
        //    {
        //        PrevWord = string.Empty;
        //        int wordCount = 0;
        //        for (int j = i; j < keywords.Length; j++)
        //        {
        //            DataRow TempRow = RetVal.NewRow();
        //            if (i == j)
        //            {
        //                TempRow[SearchKeyword] = keywords[j].ToString();
        //            }
        //            else
        //            {
        //                TempRow[SearchKeyword] = PrevWord + " " + keywords[j].ToString();
        //            }
        //            PrevWord = TempRow[SearchKeyword].ToString();
        //            wordCount++;
        //            TempRow[WordCount] = wordCount.ToString();
        //            index++;
        //            RetVal.Rows.Add(TempRow);
        //        }
        //    }
        //    // Sort Result Table
        //    if (RetVal != null && RetVal.Rows.Count > 0)
        //    {
        //        DataView Dv = RetVal.DefaultView;
        //        Dv.Sort = WordCount + " DESC";
        //        RetVal = Dv.ToTable();
        //    }
        //    return RetVal;
        //}

        ///// <summary>
        ///// Get Maximum Lenght Search String
        ///// </summary>
        ///// <param name="PossibleKeywordCombinations"></param>
        //private string GetMaxLenghtSearchString(string[,] PossibleKeywordCombinations)
        //{
        //    string Retval = string.Empty;
        //    int MaxLenghSeachStringIndex = -1;
        //    int TempMaxlenghString = 0;
        //    for (int i = 0; i < PossibleKeywordCombinations.GetLength(0); i++)
        //    {
        //        if (Convert.ToInt32(PossibleKeywordCombinations[i, 0]) > TempMaxlenghString)
        //        {
        //            TempMaxlenghString = Convert.ToInt32(PossibleKeywordCombinations[i, 0]);
        //            MaxLenghSeachStringIndex = i;
        //        }
        //    }
        //    if (MaxLenghSeachStringIndex != -1)
        //    {
        //        Retval = PossibleKeywordCombinations[MaxLenghSeachStringIndex, 1].ToString();
        //    }
        //    return Retval;
        //}

        ///// <summary>
        ///// Update Original string with values of New string If this is not found in original string
        ///// </summary>
        ///// <param name="OriginalString"></param>
        ///// <param name="NewValue"></param>
        ///// <returns></returns>
        //private string CheckAndUpdateNewNIds(String OriginalString, String NewValue)
        //{
        //    string Retval = OriginalString;
        //    string[] OriginalValues = OriginalString.Split(',');
        //    string[] NewValues = DICommon.SplitString(NewValue, ",");
        //    for (int i = 0; i < NewValues.Length; i++)
        //    {
        //        bool UpdateValue = true;
        //        if (string.IsNullOrEmpty(Retval))
        //        {
        //            Retval = NewValues[i].ToString();
        //        }
        //        else
        //        {
        //            //------------------------New Code
        //            for (int j = 0; j < OriginalValues.Length; j++)
        //            {
        //                if (OriginalValues[j].ToString() == NewValues[i].ToString())
        //                {
        //                    UpdateValue = false;
        //                    break;
        //                }
        //            }
        //            // If Update value
        //            if (UpdateValue)
        //            {
        //                Retval += "," + NewValues[i].ToString();
        //                // Update Original Values
        //                OriginalValues = Retval.Split(',');
        //            }                 
        //        }
        //    }
        //    return Retval;
        //}


        /// <summary>
        ///  By performing new Advance Search (using AND clause) return  unsorted result as comma seperated list of Presentaion Nids  
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        /// <returns></returns>
        private string GetResultNIdsForAdvSearch(string[] searchString)
        {
            String RetVal = string.Empty;
            try
            {
                string sSql = string.Empty;
                string FilterClause = string.Empty;
                System.Data.IDataReader rd;

                //Fixed Part of SQL Query
                // If search Type is ALL No Pres_type will be matched
                if (_SearchFor == SearchType.All)
                {
                    sSql = " SELECT DISTINCT " + PresentationMaster.Pres_NId + " from "+ DBTable.UT_PresKeyword +" where (";
                }
                // If search Type is other then all 
                else
                {
                    sSql = " SELECT DISTINCT " + PresentationMaster.Pres_NId + " from " + DBTable.UT_PresKeyword + " where " + PRES_TYPE + " = '" + PresType + " '  And (";
                }
                //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)

                // -- Adding each element of SearchString  into FilterClause
                for (int i = 0; i <= searchString.Length - 1; i++)
                {
                    if (FilterClause.Length > 0)
                    {
                        FilterClause += " AND ";
                    }
                    //Exect phrase search will match the word exectly
                    //FilterClause += "( Pres_Titles LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' OR " +
                    //              " Pres_Ukeywords LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' OR " +
                    //              " Pres_keywords LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' )";

                    //-- Handling for case where search keyword is in the starting or end of the Title, Keyword or User Keyword
                    FilterClause += "( Pres_Titles LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%' OR " +
                          " Pres_Titles LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' OR " +
                          " Pres_Ukeywords LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%' OR " +
                          " Pres_Ukeywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' OR " +
                          " Pres_keywords LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + " " + "%' OR " +
                          " Pres_keywords LIKE '%" + " " + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%' )";
                
                }
                FilterClause = FilterClause + ")";
                //sSql Represent query for search in presKeyword table
                sSql += FilterClause;
                rd =this.DBConnection.ExecuteReader(sSql);
                RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                rd.Close();

                // FiterNids using GalleryId
                if (RetVal.Length > 0 && this._GalleryIdForSearch != -1)
                {
                    sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " where "
                    + PresentationMaster.Pres_NId + " IN (" + RetVal + ") AND " + PresentationMaster.GalleryId + " = " + this._GalleryIdForSearch;

                    rd = this.DBConnection.ExecuteReader(sSql);
                    RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                }   
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        /// <summary>
        /// Set Sorted resultNIds in case of new search 
        /// </summary>
        /// <returns>comma seperated list of sorted Presentation NIDs</returns>
        private string SetRankedResultNIds()
        {
            //Step1: Get Presentaion NId's list as searchResult
            //Step2: Rank Result NIds,Update PreDefined Search
            string RetVal = string.Empty;
            string RankedPres_NIDs = string.Empty;
            DataTable ResultDataTable = new DataTable();

            //Call GetResultNids to Get Search Result  as presentation Nids

            // -- Added for Advanced Search
            //If Advance search (And Clause will be used in Query while searching)
            if (_IsAdvanceSearch)
            {
                ResultNIds = GetResultNIdsForAdvSearch(keywords);
            }
            else      //Normal Search (OR Clause will be used in query while searching)
            {
                //if (!string.IsNullOrEmpty(this._SearchString))
                //{
                //     this.GetResultNIdsUsingRecursiveSearch(this._SearchString);
                //}
                //else   //older Logic of searching
                //{
                     ResultNIds = GetResultNIds(keywords);
                //}
              
               
            }
            // -- end of Addition for Advanced Search

            // If Search is sucessfull get ranked NIDs
            if (ResultNIds.Length > 0)
            {
                RankedPres_NIDs = GetRankedResult(ResultNIds);

                // Update table PreDefined Search
                UpdatePreSearchTable(RankedPres_NIDs);
            }
            RetVal = RankedPres_NIDs;
            return RetVal;
        }

        /// <summary>
        /// Get the Ranked Presentation NIds .
        /// </summary>
        /// <param name="resultIds">Comma seperated List of ResultNids  as Search Result </param>
        /// <returns></returns>
        private string GetRankedResult(string resultIds)
        {
            //Step1: Get Relevent Raw Data into DataTable which is useful for Ranking
            //Step2: Call RankResult function for ranking 
            string RetVal = string.Empty;
            System.Data.DataTable BeforeRankingDataTbl;
            String sSql = String.Empty;

            // Spliting resultNid string into an array
            String[] ResultNIdArray = resultIds.Split(',');

            // Get sorted result Nids
            if (ResultNIdArray.Length > 0)
            {
                // Retrieve data set through Query             
                //RetVal = DIConnection.ExecuteDataTable(sSql);

                sSql = "SELECT  " + PresentationMaster.Pres_NId + "  , " + PresentationKeywords.Pres_Titles + " ,Pres_Keywords,Pres_UKeywords from " + DBTable.UT_PresKeyword + " where  ";

                // // Include Gallery Id
                //if (this._LastUsedGalleryId !=0)
                //{
                //    sSql += PresentationMaster.GalleryId + " = " + this._LastUsedGalleryId; 
                //}
                //if (ResultNIdArray.Length>0)
                //{
                //    sSql += " AND ( ";
                

                for (int i = 0; i <= ResultNIdArray.Length - 1; i++)
                {
                    if (i > 0)
                    {
                        sSql += " OR ";
                    }
                    sSql += PresentationMaster.Pres_NId + "  = " + ResultNIdArray[i];
                }
                //sSql += " )";
            //}
               
                BeforeRankingDataTbl =this.DBConnection.ExecuteDataTable(sSql);

                // Returning PresentaionNId's after Ranking
                RetVal = RankResult(BeforeRankingDataTbl);
            }
            return RetVal;
        }

        /// <summary>
        /// Rank results table
        /// </summary>
        /// <param name="BeforeRankingDataTbl">Data table containing presentation Title and Presentation Keywords & UkeyWord for Ranking</param>
        /// <returns></returns>
        private string RankResult(DataTable BeforeRankingDataTbl)
        {
            string RetVal = string.Empty;
            bool KeywordAlreadyMatched = false;                  //This will conform that a keyword is match only once in result phrase
            DataTable RankedTable = new DataTable();
            BeforeRankingDataTbl.Columns.Add(WEIGHTAGE_COL1, typeof(System.Double));
            BeforeRankingDataTbl.Columns.Add(WEIGHTAGE_COL2, typeof(System.Double));
            try
            {
                if (keywords !=null && keywords.Length>0 )
                {
                    float SearchKeywordCount = keywords.Length;     // total no. of search keyword
                    float ResultPhraseWordCount = 0;                // total no. of words in result phrase
                    float SearchKeywordOccurance = 0;               // Number of search keywords found in result phrase
                    string ResultPhrase = string.Empty;             //Hold Words in result Phrase 
                    string[] ResultPhraseArray;                     //Hold each word of result as an element of an array

                    float Criteria1Weightage = 0;   //1. Number of search keywords found in result phrase / total no. of search keyword
                    float Criteria2Weightage = 0;   //2. Number of search keywords found in result phrase / total no. of words in result phrase 
                    for (int cnt = 0; cnt < BeforeRankingDataTbl.Rows.Count; cnt++)
                    {
                        SearchKeywordOccurance = 0;
                        Criteria1Weightage = 0;
                        Criteria2Weightage = 0;

                        // Step 1 Get the number of search keywords found in result phrase
                        // Creating result phrase considering Presentation title,Presentation keywords and presentationU_keywords
                        ResultPhrase = BeforeRankingDataTbl.Rows[cnt][PresentationKeywords.Pres_Titles].ToString() + " " + BeforeRankingDataTbl.Rows[cnt][PRES_KEYWORDS].ToString()
                          + " " + BeforeRankingDataTbl.Rows[cnt][PRES_UKEYWORDS].ToString();

                        ResultPhraseArray = ResultPhrase.Split(' ');
                        ResultPhraseWordCount = ResultPhraseArray.Length;

                        // loop no. of times equal to keyword lenght
                        for (int i = 0; i <= SearchKeywordCount - 1; i++)
                        {
                            //KeywordAlreadyMatched will conform that a keyword is match only once in result phrase, and value of
                            //searchKeyword is not incremented if the same keyword is found more than once in resultPhrase array. 
                            KeywordAlreadyMatched = false;

                            // loop no. of times equal to lenght of data
                            for (int j = 0; j <= ResultPhraseWordCount - 1; j++)
                                if (keywords[i].ToLower() == ResultPhraseArray[j].ToLower())
                                {
                                    // Increment the value of SearchKeywordOccurance for first time match in resultPhraseArray
                                    if (!KeywordAlreadyMatched)
                                    {
                                        SearchKeywordOccurance += 1;
                                        KeywordAlreadyMatched = true;   //Now 
                                    }
                                }
                        }

                        // Step 2 Get Criteria1Weightage
                        // Number of search keywords found in result phrase / total no. of search keyword * 100
                        Criteria1Weightage = (SearchKeywordOccurance / SearchKeywordCount) * 100;

                        // Step 3 Get Criteria2Weightage
                        // Number of search keywords found in result phrase / total no. of words in result phrase * 100
                        Criteria2Weightage = (SearchKeywordOccurance / ResultPhraseWordCount) * 100;

                        // Step 4 Set Weightage Column's value in datatable
                        BeforeRankingDataTbl.Rows[cnt][WEIGHTAGE_COL1] = Criteria1Weightage;
                        BeforeRankingDataTbl.Rows[cnt][WEIGHTAGE_COL2] = Criteria2Weightage;
                    }
                    BeforeRankingDataTbl.AcceptChanges();

                    // Step 5 Sort data on the basis of weightage and Title
                    DataView DV = BeforeRankingDataTbl.DefaultView;
                    //  RetVal.Sort = WEIGHTAGE_COL1 + " DESC, " + WEIGHTAGE_COL2 + " DESC, " + Indicator.IndicatorName + " DESC, " + Timeperiods.TimePeriod + " DESC";                     
                    DV.Sort = WEIGHTAGE_COL1 + " Desc, " + WEIGHTAGE_COL2 + " Desc, " + PresentationKeywords.Pres_Titles + " Asc ";
                    RankedTable = DV.ToTable();

                }
                else
                {
                    RankedTable = BeforeRankingDataTbl;
                }
               

                for (int j = 0; j < RankedTable.Rows.Count; j++)
                {
                    if (RetVal.Length > 0)
                    {
                        RetVal = RetVal + "," + RankedTable.Rows[j][PresentationMaster.Pres_NId].ToString();
                    }
                    else
                    {
                        RetVal = RankedTable.Rows[j][PresentationMaster.Pres_NId].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        /// <summary>
        /// Update PreSearch Table with new SearchResult
        /// </summary>
        /// <param name="Presentaion_NIds"></param>
        private void UpdatePreSearchTable(string presentaion_NIds)
        {
            string sSql = string.Empty;
            // If Simple search then Check for search type and 
            // add that search type in Condition            
            if (this._SearchString.Length > 0)
            {
                if (_Condition == "B")
                {
                    switch (SearchFor)
                    {
                        case SearchType.All:
                            break;
                        case SearchType.Table:
                            _Condition += "T";
                            break;
                        case SearchType.Graph:
                            _Condition += "G";
                            break;
                        case SearchType.Map:
                            _Condition += "M";
                            break;
                        case SearchType.Report:
                            _Condition += "R";
                            break;
                        default:
                            break;
                    }
                }
                try
                {

                    if (this._GalleryIdForSearch != -1)
                    {
                        sSql = "Insert into " + DBTable.UT_PreSearches + " (" + PreSearches.PDS_Keyword + ","
                       + PreSearches.PDS_Presentation_NIds + "," + PreSearches.PDS_Condition + "," + PreSearches.PDS_GalleryId + ") values"
                       + " ('" + _SearchString + "','" + presentaion_NIds + "','" + _Condition + "'," + _GalleryIdForSearch + ")";
                    }
                    else
                    {
                        sSql = "Insert into " + DBTable.UT_PreSearches + " (" + PreSearches.PDS_Keyword + ","
                        + PreSearches.PDS_Presentation_NIds + "," + PreSearches.PDS_Condition + ") values"
                        + " ('" + _SearchString + "','" + presentaion_NIds + "','" + _Condition + "')";
                    }

                    this.DBConnection.ExecuteNonQuery(sSql);
                }
                catch (Exception ex)
                {
                    
                }

            }
        }

        /// <summary>
        /// Get Data for xml Generation
        /// </summary>
        /// <param name="resultIds"> Sorted List of result NIDs</param>
        /// <returns></returns>
        private DataTable GetResultTableForXMLolD(string resultIds)
        {
            //Step 1: create a datatable(Retval) with columns Pres_NID,PRES_FILENAME and PRES_TYPE
            //Step 2: create another table by getting all result for resultIDs  by using IN clause 
            // step3: Use Relations based on Pres_NID to update first datatable
            IDataReader rd;
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(PresentationMaster.Pres_NId, typeof(System.Int32));
            RetVal.Columns.Add(PresentationMaster.Pres_FileName, typeof(System.String));
            RetVal.Columns.Add(PresentationMaster.Pres_Type, typeof(System.String));

            String sSql = String.Empty;
            try
            {

                // Spliting resultNid string into an array
                String[] ResultNIdArray = resultIds.Split(',');

                // Get sorted result Nids
                if (ResultNIdArray.Length > 0)
                {
                    for (int i = 0; i < ResultNIdArray.Length; i++)
                    {
                        DataRow DR = RetVal.NewRow();
                        sSql = "SELECT PM.pres_Nid, PM." + PresentationMaster.Pres_FileName + ", PM." + PresentationMaster.Pres_Type + " FROM  " + DBTable.UT_PresMst + " AS PM WHERE PM."+PresentationMaster.Pres_NId+" = " + ResultNIdArray[i];
                        
                        rd = this.DBConnection.ExecuteReader(sSql);
                        rd.Read();
                        DR[PresentationMaster.Pres_NId] = ResultNIdArray[i];
                        DR[PRES_FILENAME] = rd[PRES_FILENAME].ToString();
                        DR[PRES_TYPE] = rd[PRES_TYPE].ToString();
                        RetVal.Rows.Add(DR);

                        rd.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        /// <summary>
        /// Get Data for xml Generation
        /// </summary>
        /// <param name="resultIds"> Sorted List of result NIDs</param>
        /// <returns></returns>
        private DataTable GetResultTableForXML(string resultIds)
        {
            //Step 1: create a datatable(Retval) with columns Pres_NID,PRES_FILENAME and PRES_TYPE
            //Step 2: create another table by getting all result for resultIDs  by using IN clause 
            // step3: Use Relations based on Pres_NID to update first datatable
            DataSet DS = new DataSet();
            DataTable RetVal = new DataTable();
            DataTable UnSortedDT = new DataTable();
            DataRelation Relation;

            //  Adding columns to dataTable
            RetVal.Columns.Add(PresentationMaster.Pres_NId, typeof(System.Int32));
            RetVal.Columns.Add(PRES_FILENAME, typeof(System.String));
            RetVal.Columns.Add(PRES_TYPE, typeof(System.String));
            String sSql = String.Empty;
            try
            {
                // Spliting resultNid string into an array
                String[] ResultNIdArray = resultIds.Split(',');

                if (ResultNIdArray.Length > 0)
                {
                    for (int i = 0; i < ResultNIdArray.Length; i++)
                    {
                        // Inserting  Nid's into DataTable (Step1)
                        DataRow DRow = RetVal.Rows.Add();
                        DRow[0] = ResultNIdArray[i].ToString();
                    }
                    // Getting all result in second Datatable(step2)
                    sSql = " Select Pres_Nid,Pres_FileName,Pres_Type from " + DBTable.UT_PresMst + " where Pres_NId in ( " + resultIds + " )";
                    UnSortedDT = this.DBConnection.ExecuteDataTable(sSql);

                    // Creating Relationship between two datatable on PRES_NIds  (step3)
                    DS.Tables.Add(RetVal);
                    DS.Tables.Add(UnSortedDT);
                    Relation = new DataRelation("NIdRelation", DS.Tables[0].Columns[0], DS.Tables[1].Columns[0]);
                    DS.Relations.Add(Relation);
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        // Get child Row corrosponding to parent table row's NId 
                        // Update Parent Table
                        DataRow ChildRow = row.GetChildRows("NIdRelation")[0];
                        row[1] = ChildRow[1].ToString();
                        row[2] = ChildRow[2].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        /// <summary>
        /// Get Data for xml Generation
        /// </summary>
        /// <param name="resultIds"> Sorted List of result NIDs</param>
        /// <returns></returns>
        private DataSet GetResultDataSetForXML(string resultIds)
        {
            //Step 1: create a datatable(Retval) with columns Pres_NID,PRES_FILENAME and PRES_TYPE
            //Step 2: create another table by getting all result for resultIDs  by using IN clause 
            // step3: Use Relations based on Pres_NID to update first datatable
            DataSet RetVal = new DataSet("SearchResultDataSet");
            DataTable ResultDT = new DataTable("Result");
            DataTable UnSortedDT = new DataTable("UnsortedDT");
            DataTable GalleryDT = new DataTable("Gallery");
            DataRelation Relation;

            //  Adding columns to dataTable
            ResultDT.Columns.Add(PresentationMaster.Pres_NId, typeof(System.Int32));
            ResultDT.Columns.Add(PRES_FILENAME, typeof(System.String));
            ResultDT.Columns.Add(PRES_TYPE, typeof(System.String));
            ResultDT.Columns.Add(GalleryMaster.GalleryFolderNId, typeof(System.Int32));

            // adding columns to Gallery Datatable
            GalleryDT.Columns.Add(GalleryMaster.GalleryFolderNId, typeof(System.Int32));
            GalleryDT.Columns.Add(GalleryMaster.GalleryFolder, typeof(System.String));
            

            String sSql = String.Empty;
            try
            {
                // Spliting resultNid string into an array
                String[] ResultNIdArray = resultIds.Split(',');

                if (ResultNIdArray.Length > 0)
                {
                    for (int i = 0; i < ResultNIdArray.Length; i++)
                    {
                        // Inserting  Nid's into DataTable (Step1)
                        DataRow DRow = ResultDT.Rows.Add();
                        DRow[0] = ResultNIdArray[i].ToString();
                    }
                    // Getting all result in second Datatable(step2)
                   ////sSql = " Select Pres_Nid,Pres_FileName,Pres_Type,GalleryId from UT_PresMst where Pres_NId in ( " + resultIds + " )";
                   
                    sSql = "SELECT P.Pres_NId, P.Pres_FileName, P.Pres_Type, P.GalleryId, G.GalleryFolder FROM " + DBTable.UT_PresMst + " AS P, UT_GalleryMst AS G  where  P.Pres_NId in ( " + resultIds + " ) and  P.GalleryId= G.GalleryFolderNId ";

                    UnSortedDT = this.DBConnection.ExecuteDataTable(sSql);
                    // Bug Fixed : unable to read newly  updated data when called quickly after database update                    
                    if (UnSortedDT.Rows.Count == 0)
                    {
                        //close and  Recreate Connection to get newly updated/Inserted record
                        try
                        {
                            DIConnectionDetails ConnDetails = this.DBConnection.ConnectionStringParameters;
                            this.DBConnection.Dispose();
                            this.DBConnection = null;
                            this.DBConnection = new DIConnection(ConnDetails);
                            UnSortedDT = this.DBConnection.ExecuteDataTable(sSql);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    UnSortedDT.TableName = "UnsortedDT";
                    // Creating Relationship between two datatable on PRES_NIds  (step3)
                    RetVal.Tables.Add(ResultDT);
                    RetVal.Tables.Add(UnSortedDT);                    
                    Relation = new DataRelation("NIdRelation", RetVal.Tables[0].Columns[0], RetVal.Tables[1].Columns[0]);
                    RetVal.Relations.Add(Relation);
                    foreach (DataRow row in RetVal.Tables[0].Rows)
                    {
                        // Get child Row corrosponding to parent table row's NId 
                        // Update Parent Table
                        DataRow ChildRow = row.GetChildRows("NIdRelation")[0];
                        row[1] = ChildRow[1].ToString();
                        row[2] = ChildRow[2].ToString();
                        row[3] = ChildRow[3].ToString(); //Gallery NId 

                        //TODo Add Records in Gallery DataTable .ChildRow[3].ToString() and ChildRow[4].ToString()
                        DataView dv = GalleryDT.DefaultView;
                        dv.RowFilter = GalleryMaster.GalleryFolderNId + " = '" + ChildRow[3].ToString()+"'";
                        if (dv.Count==0)
                        {
                         // Add new row
                            DataRow GalleryDTRow =GalleryDT.NewRow();
                            GalleryDTRow[0] = ChildRow[3].ToString();
                            GalleryDTRow[1] = ChildRow[4].ToString();
                            GalleryDT.Rows.Add(GalleryDTRow);
                        }
                    }
                    
                    // Add Gallery DataTable
                    RetVal.Tables.Add(GalleryDT);
                    RetVal.Relations.Remove(Relation);
                    UnSortedDT.Constraints.Remove(UnSortedDT.Constraints[0]);
                    RetVal.Tables.Remove(UnSortedDT);

                    // Rename DataTableColName
                    ResultDT.Columns[0].ColumnName = Id;
                    ResultDT.Columns[1].ColumnName = File;
                    ResultDT.Columns[2].ColumnName = Type;
                    ResultDT.Columns[3].ColumnName = SearchResultFields.GalleryId;
                    GalleryDT.Columns[0].ColumnName = SearchResultFields.GalleryId;
                    GalleryDT.Columns[1].ColumnName = SearchResultFields.Path;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
   
        /// <summary>
        /// Get Xml result(Which is returned as SearchResult)
        /// </summary>
        /// <param name="resultTable"></param>
        /// <returns></returns>
        private XmlDocument GetXml(System.Data.DataTable resultTable)
        {
            String FILE_NODE = "file";      // To Conform : may be Constants
            String XML_ROOT = "root";
            String ID_ATTRIBUTE = "id";
            String NAME_ATTRIBUTE = "name";
            String TYPE_ATTRIBUTE = "type";
            // Create a xml Doc with Gallery as Root Element
            XmlDocument _XmlDoc = GetXMLDoc(XML_ROOT);
            XmlNode XmlResultSetNode;
            XmlAttribute ResultCnt;
            Int32 ResultCount = resultTable.Rows.Count;

            //Preparing Result String for result node
            for (int i = 0; i < ResultCount; i++)
            {
                // Creating File Element node
                XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element, FILE_NODE, "");

                //Creating ID Attrbute
                ResultCnt = _XmlDoc.CreateAttribute(ID_ATTRIBUTE);
                ResultCnt.Value = resultTable.Rows[i][PresentationMaster.Pres_NId].ToString();
                XmlResultSetNode.Attributes.Append(ResultCnt);

                //Creating File Name Attrbute
                ResultCnt = _XmlDoc.CreateAttribute(NAME_ATTRIBUTE);
                ResultCnt.Value = resultTable.Rows[i][PRES_FILENAME].ToString();
                XmlResultSetNode.Attributes.Append(ResultCnt);

                //Creating Type Attrbute
                ResultCnt = _XmlDoc.CreateAttribute(TYPE_ATTRIBUTE);
                ResultCnt.Value = resultTable.Rows[i][PRES_TYPE].ToString();
                XmlResultSetNode.Attributes.Append(ResultCnt);
                _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);

            }
            return _XmlDoc;
        }

        ///// <summary>
        ///// Get Xml result(Which is returned as SearchResult)
        ///// </summary>
        ///// <param name="resultTable"></param>
        ///// <returns></returns>
        //private XmlDocument GetXml(DataSet resultDataSet)
        //{
        //    String FILE_NODE =SearchResultFields.file;    
        //    String Gallery_NODE = SearchResultFields.gallery;   

        //    String XML_ROOT = "root";
        //    String ID_ATTRIBUTE = SearchResultFields.id;
        //    String NAME_ATTRIBUTE = SearchResultFields.name;
        //    String TYPE_ATTRIBUTE = SearchResultFields.type;

        //    // Create a xml Doc with Gallery as Root Element
        //    XmlDocument _XmlDoc = GetXMLDoc(XML_ROOT);
        //    XmlNode XmlResultSetNode;
        //    XmlAttribute ResultAttribute;
        //    DataTable ResultTable = resultDataSet.Tables[0];
        //    Int32 ResultCount = ResultTable.Rows.Count;

        //    //Preparing Result String for result node
        //    for (int i = 0; i < ResultCount; i++)
        //    {
        //        // Creating File Element node
        //        XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element, FILE_NODE, "");

        //        //Creating ID Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(ID_ATTRIBUTE);
        //        ResultAttribute.Value = ResultTable.Rows[i][Id].ToString();
        //        XmlResultSetNode.Attributes.Append(ResultAttribute);

        //        //Creating File Name Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(NAME_ATTRIBUTE);
              
        //        // Get File Name
  
        //        // In case of DiBook We need to break file name like
        //        //diBook1{[]}C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
        //        // so split this string  on  "{[]}" and then get path 
        //        if (ResultTable.Rows[i][TYPE_ATTRIBUTE].ToString().ToLower() == "b" || ResultTable.Rows[i][TYPE_ATTRIBUTE].ToString().ToLower() == "v")
        //        {
        //            String FileNameSeperator = "{[]}";
        //            string[] diBookFilePath=ResultTable.Rows[i][File].ToString().Split(FileNameSeperator.ToCharArray());
        //            ResultAttribute.Value = Path.Combine(diBookFilePath[diBookFilePath.Length-1], diBookFilePath[0]);
        //        }
        //        // If it is  not the case of diBook
        //        // -- Name will be the file name without extention 
        //        else
        //        {
        //            ResultAttribute.Value = System.IO.Path.GetFileNameWithoutExtension(ResultTable.Rows[i][File].ToString());
        //        }
              

        //        XmlResultSetNode.Attributes.Append(ResultAttribute);

        //        //Creating Type Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(TYPE_ATTRIBUTE);
        //        ResultAttribute.Value = ResultTable.Rows[i][Type].ToString();
        //        XmlResultSetNode.Attributes.Append(ResultAttribute);

        //        //Creating Gallery ID Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.GalleryId);
        //        ResultAttribute.Value = ResultTable.Rows[i][SearchResultFields.GalleryId].ToString();
        //        XmlResultSetNode.Attributes.Append(ResultAttribute);
        //        _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);

        //    }
        //    DataTable GalleryTable = resultDataSet.Tables[1];
        //    ResultCount = GalleryTable.Rows.Count;
        //    for (int j = 0; j < ResultCount; j++)
        //    {
        //        XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element, Gallery_NODE, "");
        //        //Creating ID Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.GalleryId);
        //        ResultAttribute.Value = GalleryTable.Rows[j][SearchResultFields.GalleryId].ToString();
        //        XmlResultSetNode.Attributes.Append(ResultAttribute);

        //        //Creating Path Attrbute
        //        ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.Path);
        //        ResultAttribute.Value = GalleryTable.Rows[j][SearchResultFields.Path].ToString();
        //        XmlResultSetNode.Attributes.Append(ResultAttribute);
        //        _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);


        //    }
        //    return _XmlDoc;
        //}

        /// <summary>
        /// Get Xml result(Which is returned as SearchResult)
        /// </summary>
        /// <param name="resultTable"></param>
        /// <returns></returns>
        private XmlDocument GetXml(DataSet resultDataSet,int pageNo,int pageSize)
        {
            String FILE_NODE = SearchResultFields.file;
            String Gallery_NODE = SearchResultFields.gallery;
          
            String XML_ROOT = "root";
            String TOTAL_NoOfPages = "TotPg";
            String PAGES = "Pages";
            String CurrentPageNo= "CurrPg";
            String ResultFileCount = "ResultFileCount";
            String ID_ATTRIBUTE = SearchResultFields.id;
            String NAME_ATTRIBUTE = SearchResultFields.name;
            String TYPE_ATTRIBUTE = SearchResultFields.type;

            // Create a xml Doc with Gallery as Root Element
            XmlDocument _XmlDoc = GetXMLDoc(XML_ROOT);
            XmlNode XmlResultSetNode;
            XmlAttribute ResultAttribute;
            DataTable ResultTable = resultDataSet.Tables[0];
            Int32 ResultCount = ResultTable.Rows.Count;

            //+++++++++++++++++++++++++++++++++

            int PageStartRecordIndex = 0;
            int PageEndRecordIndex = ResultCount;
            int TotalNoOfPages = -1;
            if (pageSize > 0)
            {
                if (pageNo != -1 && pageSize != -1)
                {
                    TotalNoOfPages = this.GetTotalNoPages(pageNo, pageSize, ResultCount);
                    // Get Start and end index  for current page
                    // StartIndex= pageNo -1* pageSize, So if Page 2 data sought and page size is 20
                    //StartIndex will be (2-1)* 20 =20
                    PageStartRecordIndex = ((pageNo - 1) * pageSize);
                    PageEndRecordIndex = PageStartRecordIndex + pageSize;
                    // IF End Index Is greater then ResultCount Reset End index to  ResultCount 
                    if (PageEndRecordIndex > ResultCount)
                    {
                        PageEndRecordIndex = ResultCount;
                    }
                }    
            }            

            //+++++++++++++++++++++++++++++++++

            //Preparing Result String for result node
            for (int i = PageStartRecordIndex; i < PageEndRecordIndex; i++)
            {
                // Creating File Element node
                XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element, FILE_NODE, "");

                //Creating ID Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(ID_ATTRIBUTE);
                ResultAttribute.Value = ResultTable.Rows[i][Id].ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);

                //Creating File Name Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(NAME_ATTRIBUTE);

                // Get File Name

                // In case of DiBook We need to break file name like
                //diBook1{[]}C:\-- Projects --\DevInfo 6.0\User Interface - Desktop\bin\DI Book\diBooks
                // so split this string  on  "{[]}" and then get path 
                if (ResultTable.Rows[i][TYPE_ATTRIBUTE].ToString().ToLower() == "b" || ResultTable.Rows[i][TYPE_ATTRIBUTE].ToString().ToLower() == "v")
                {
                    String FileNameSeperator = "{[]}";
                    string[] diBookFilePath = ResultTable.Rows[i][File].ToString().Split(FileNameSeperator.ToCharArray());
                    ResultAttribute.Value = Path.Combine(diBookFilePath[diBookFilePath.Length - 1], diBookFilePath[0]);
                }
                // If it is  not the case of diBook
                // -- Name will be the file name without extention 
                else
                {
                    ResultAttribute.Value = ResultTable.Rows[i][File].ToString(); //System.IO.Path.GetFileNameWithoutExtension
                }


                XmlResultSetNode.Attributes.Append(ResultAttribute);

                //Creating Type Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(TYPE_ATTRIBUTE);
                ResultAttribute.Value = ResultTable.Rows[i][Type].ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);

                //Creating Gallery ID Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.GalleryId);
                ResultAttribute.Value = ResultTable.Rows[i][SearchResultFields.GalleryId].ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);
                _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);

            }
            DataTable GalleryTable = resultDataSet.Tables[1];
            ResultCount = GalleryTable.Rows.Count;
            for (int j = 0; j < ResultCount; j++)
            {
                XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element, Gallery_NODE, "");
                //Creating ID Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.GalleryId);
                ResultAttribute.Value = GalleryTable.Rows[j][SearchResultFields.GalleryId].ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);

                //Creating Path Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(SearchResultFields.Path);
                ResultAttribute.Value = GalleryTable.Rows[j][SearchResultFields.Path].ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);
                _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);

            }
            // Add Page Count,Current Page and result count (for online Gallery)
            if (ResultCount > 0 && pageNo != -1 && pageSize != -1)
            {
                XmlResultSetNode = _XmlDoc.CreateNode(System.Xml.XmlNodeType.Element,PAGES, "");
                //Creating TotalPg Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(TOTAL_NoOfPages);
                ResultAttribute.Value = TotalNoOfPages.ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);
                
                //Current Page Attrbute
                ResultAttribute = _XmlDoc.CreateAttribute(CurrentPageNo);
                ResultAttribute.Value = pageNo.ToString();
                XmlResultSetNode.Attributes.Append(ResultAttribute);

                // Result Count                
                if (ResultTable != null && ResultTable.Rows.Count>0 )
                {
                    ResultAttribute = _XmlDoc.CreateAttribute(ResultFileCount);
                    ResultAttribute.Value = ResultTable.Rows.Count.ToString();
                    XmlResultSetNode.Attributes.Append(ResultAttribute);
                }
              
                // Add page node having Tot No of Pages ,current Page and  ResultCount
                _XmlDoc.DocumentElement.AppendChild(XmlResultSetNode);               
                
            }
            return _XmlDoc;
        }

       /// <summary>
       /// Get TotalNo of Pages .using PageSize and resultCount
       /// </summary>
       /// <param name="pageNo"></param>
       /// <param name="pageSize"></param>
       /// <param name="ResultCount"></param>
       /// <returns></returns>
       private int GetTotalNoPages(int pageNo, int pageSize, Int32 ResultCount)
       {
           int RetVal = -1;
           // Get total No of pages
           if (pageNo != -1 && pageSize != -1 && (ResultCount % pageSize) == 0)
           {
               RetVal = Convert.ToInt32(ResultCount / pageSize);
           }
           else
           {
               RetVal = Convert.ToInt32(ResultCount / pageSize) + 1;
           }
           return RetVal;
       }


        /// <summary>
        /// Create new Xml Document
        /// </summary>
        /// <param name="p_ROOTNAME"> Root Elemet for Xml Document</param>
        /// <returns></returns>
        private XmlDocument GetXMLDoc(String p_ROOTNAME)
        {
            XmlDocument XMLDoc1 = new XmlDocument();
            XMLDoc1.LoadXml("<?xml version='1.0'?><" + p_ROOTNAME + "></" + p_ROOTNAME + ">");
            return XMLDoc1;
        }

        /// <summary>
        /// Break search keyword at space and quotes to get absolute keyword.Which will to be used in database Query.
        /// </summary>
        /// <returns>String array containg each part of keyword as element </returns>
        /// <remarks>If Search String Contains Space or Quotes then string will be splited in parts.Later each part will be used seperately for search in Database.</remarks>
        private string[] GetAbsoluteKeyword(string keyword)
        {
            string[] RetVal;
            string[] TempSearchPattern;

            // -- Split keywords  at quotes  and get it in SearchPattern"
            //string[] SearchPattern = keyword.Split((char)"\"");
            string[] SearchPattern = keyword.Split('"');

            if (IsEvenNumber(SearchPattern.Length))
            {
                //--- Quotes to be ignored
                SearchPattern = keyword.Split(' ');
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
                            TempSearch = SearchPattern[i].Split(' ');
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
                            TempStorage.Add(SearchPattern[i]);
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

        /// <summary>
        /// Check whether Even Number
        /// Used by method GetAbsoluteKeyword
        /// </summary>
        /// <param name="number">No to be Checked Whether Even Or not</param>
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

        ///// <summary>
        ///// This will  search in predefined table and return resultNIds
        ///// </summary>
        ///// <param name="searchText"></param>
        ///// <param name="Condition"></param>
        ///// <returns></returns>
        //private string PreDefindSearch(string searchText, String Condition, SearchType searchIn)
        //{
        //    string RetVal = string.Empty;
        //    IDataReader rd;
        //    string PreSearchResult = string.Empty;
        //    // If Simple search then Check for search type and 
        //    // add that search type in Condition            
        //    if (Condition == "B")
        //    {
        //        switch (searchIn)
        //        {
        //            case SearchType.All:
        //                break;
        //            case SearchType.Table:
        //                Condition += "T";
        //                break;
        //            case SearchType.Graph:
        //                Condition += "G";
        //                break;
        //            case SearchType.Map:
        //                Condition += "M";
        //                break;
        //            case SearchType.Report:
        //                Condition += "R";
        //                break;
        //            default:
        //                break;
        //        }
        //    }


        //    // Calling Stored Procedure for search in Predefined Table
        //    List<DbParameter> paramerters = new List<DbParameter>();
        //    DbParameter param = this.DBConnection.CreateDBParameter();
        //    DbParameter param1 = this.DBConnection.CreateDBParameter();

        //    // Setting parameter for stored procedure
        //    param.ParameterName = "keyword";
        //    param.Value = searchText;
        //    paramerters.Add(param);
        //    param1.ParameterName = "condition";
        //    param1.Value = Condition;
        //    paramerters.Add(param1);

           
        //    try
        //    {
        //        if (this._GalleryIdForSearch==-1)
        //        {
        //            // Getting result of SP LookupPreDefinedSearch
        //            PreSearchResult = this.DBConnection.ExecuteScalarSqlQuery("SP_LookupPreDefinedSearch", CommandType.StoredProcedure, paramerters).ToString();                    
        //        }
        //        else
        //        {

        //            // get Preseachresult using GalleryId

        //            // Create Parameter
        //            DbParameter param2 = this.DBConnection.CreateDBParameter();
        //            param2.ParameterName = "galleryId";
        //            param2.Value = this._GalleryIdForSearch;
        //            paramerters.Add(param2);

        //            PreSearchResult = this.DBConnection.ExecuteScalarSqlQuery("SP_LookupPreDefinedSearchUsingGalleryId", CommandType.StoredProcedure, paramerters).ToString();                    
        //        }
        //    }
        //    catch (NullReferenceException ex)
        //    {
        //    }

        //    // If  keyword was found in Presearch Table
        //    if (PreSearchResult.Length > 0)
        //    {
        //        // Check Presentation type
        //        if (PresType == PRES_TYPE_ALL)
        //        {
        //            RetVal = GetResultInXML(PreSearchResult).InnerXml.ToString();
        //        }

        //        else // IF Presentation type is other than all
        //        {
        //            string sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PRES_TYPE + " = '" + PresType + "' and ( " + PresentationMaster.Pres_NId + " in ( " + PreSearchResult + "))";
        //            rd = this.DBConnection.ExecuteReader(sSql);
        //            string PreSearchNIds = DIConnection.GetDelimitedValuesFromReader(rd,PresentationMaster.Pres_NId);
        //            if (!string.IsNullOrEmpty(PreSearchNIds))
        //            {
        //                RetVal = GetResultInXML(PreSearchNIds).InnerXml.ToString();
        //            }
        //        }
        //    }

        //    return RetVal;
        //}

        /// <summary>
        /// This will  search in predefined table and return resultNIds
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="Condition"></param>
        /// <returns></returns>
        private string PreDefindSearch(string searchText, String Condition, SearchType searchIn,int pageNo, int pageSize)
        {
            string RetVal = string.Empty;
            IDataReader rd;
            string PreSearchResult = string.Empty;
            // If Simple search then Check for search type and 
            // add that search type in Condition            
            if (Condition == "B")
            {
                switch (searchIn)
                {
                    case SearchType.All:
                        break;
                    case SearchType.Table:
                        Condition += "T";
                        break;
                    case SearchType.Graph:
                        Condition += "G";
                        break;
                    case SearchType.Map:
                        Condition += "M";
                        break;
                    case SearchType.Report:
                        Condition += "R";
                        break;
                    case SearchType.Profile:
                        Condition += "P";
                        break;
                    default:
                        break;
                }
            }


            // Calling Stored Procedure for search in Predefined Table
            List<DbParameter> paramerters = new List<DbParameter>();
            DbParameter param = this.DBConnection.CreateDBParameter();
            DbParameter param1 = this.DBConnection.CreateDBParameter();

            // Setting parameter for stored procedure
            param.ParameterName = "keyword";
            param.Value = searchText;
            paramerters.Add(param);
            param1.ParameterName = "condition";
            param1.Value = Condition;
            paramerters.Add(param1);


            try
            {
                if (this._GalleryIdForSearch == -1)
                {
                    // Getting result of SP LookupPreDefinedSearch
                    PreSearchResult = this.DBConnection.ExecuteScalarSqlQuery("SP_LookupPreDefinedSearch", CommandType.StoredProcedure, paramerters).ToString();
                }
                else
                {

                    // get Preseachresult using GalleryId

                    // Create Parameter
                    DbParameter param2 = this.DBConnection.CreateDBParameter();
                    param2.ParameterName = "galleryId";
                    param2.Value = this._GalleryIdForSearch;
                    paramerters.Add(param2);

                    PreSearchResult = this.DBConnection.ExecuteScalarSqlQuery("SP_LookupPreDefinedSearchUsingGalleryId", CommandType.StoredProcedure, paramerters).ToString();
                }
            }
            catch (NullReferenceException ex)
            {
            }

            // If  keyword was found in Presearch Table
            if (PreSearchResult.Length > 0)
            {
                // Check Presentation type
                if (PresType == PRES_TYPE_ALL)
                {
                    RetVal = GetResultInXML(PreSearchResult, pageNo, pageSize).InnerXml.ToString();
                }

                else // IF Presentation type is other than all
                {
                    string sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PRES_TYPE + " = '" + PresType + "' and ( " + PresentationMaster.Pres_NId + " in ( " + PreSearchResult + "))";
                    rd = this.DBConnection.ExecuteReader(sSql);
                    string PreSearchNIds = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
                    if (!string.IsNullOrEmpty(PreSearchNIds))
                    {
                        RetVal = GetResultInXML(PreSearchNIds,pageNo,pageSize).InnerXml.ToString();
                    }
                }
            }

            return RetVal;
        }

       /// <summary>
       /// Get all Pres NId from Presentation master Table. where Presentation type is as given in parameter
       /// </summary>
       /// <param name="searchType"> Values may be ALL,MAP ,TABLE,GRAPH</param>
       /// <returns></returns>
       private string GetAllPresNId(SearchType searchType)
       {
           string RetVal = string.Empty;
           IDataReader rd;
           string sSql = string.Empty;
           string Pres_Type = string.Empty;

           // Check Presentation type
           // If ALL then get all presentations from Pres_Mst table
           if (searchType ==SearchType.All)
           {
               if (this._GalleryIdForSearch != -1)
               {
                   sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + 
                       " WHERE " + PresentationMaster.GalleryId + " = " + this._GalleryIdForSearch  ;
               }
               else
               {
                   sSql = "SELECT  DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst;
               }              
               rd = this.DBConnection.ExecuteReader(sSql);
               RetVal = DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);
           }

           else // IF Presentation type is other than all. Get all Pres from Pres_mst table of this search type only
           {
               //Set Pres_type
               switch (searchType)
               {                   
                   case SearchType.Table:
                       Pres_Type = PRES_TYPE_TABLE;
                       break;
                   case SearchType.Graph:
                       Pres_Type = PRES_TYPE_GRAPH;
                       break;
                   case SearchType.Map:
                       Pres_Type = PRES_TYPE_MAP;
                       break;                
                   default:
                       break;
               }
               if (this._GalleryIdForSearch !=-1)
               {
                   // Sql Query for getting pres NId
                   sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PRES_TYPE + " = '" + Pres_Type + "' AND " + 
                       PresentationMaster.GalleryId +" = " + this._GalleryIdForSearch ;
               }
               else
               {
                   // Sql Query for getting pres NId
                   sSql = "SELECT DISTINCT " + PresentationMaster.Pres_NId + " FROM " + DBTable.UT_PresMst + " WHERE " + PRES_TYPE + " = '" + Pres_Type + "'";
               }
               
               rd = this.DBConnection.ExecuteReader(sSql);
              RetVal=  DIConnection.GetDelimitedValuesFromReader(rd, PresentationMaster.Pres_NId);              
           }
           return RetVal;

       }

       #region " -- Dispose -- "
       ///// <summary>
       ///// Dispose DIconnection
       ///// </summary>
       ///// <param name="suppress"></param>
       //private void Dispose(bool suppress)
       //{
       //    if (this.DBConnection != null)
       //    {
       //        this.DBConnection.Dispose();
       //    }
       //    if (suppress)
       //    {
       //        GC.SuppressFinalize(this);
       //    }
       //}
       #endregion
        #endregion

        #endregion

       #region " -- Public -- "

       #region " -- Variable -- "


       #endregion

       #region " -- Enum -- "

       /// <summary>
        /// Different Search Type
        /// This will for deciding which type of preseentation will be searched
        /// All denotes that Presention may be of any type (Table,Graph or Map)
        /// </summary>
        public enum SearchType
        {
            All,
            Table,
            Graph,
            Map,
            Profile,
            Report // Yet not implemented
        }

        #endregion

       #region " -- Properties -- "


        private SearchType _SearchFor = SearchType.All;
        /// <summary>
        /// What to look for Table, Graph ,Map or All
        /// Default All
        /// </summary>
        public SearchType SearchFor
        {
            get
            {
                return this._SearchFor; 
            }
            set
            {
               this._SearchFor = value;
                switch (_SearchFor)
                {
                    case SearchType.All:
                        PresType = PRES_TYPE_ALL;
                        break;
                    case SearchType.Table:
                        PresType = PRES_TYPE_TABLE;
                        break;
                    case SearchType.Graph:
                        PresType = PRES_TYPE_GRAPH;
                        break;
                    case SearchType.Map:
                        PresType = PRES_TYPE_MAP;
                        break;
                    case SearchType.Profile:
                        PresType = PRES_TYPE_PROFILE;
                        break;
                    default:
                        break;
                }
            }
        }

        private string _GalleryDatabase=string.Empty;
        /// <summary>
        /// Get or set the path and the file name of the Gallery database
        /// </summary>
        public string GalleryDatabase
        {
            get
            {
                return this._GalleryDatabase;
            }
            set
            {
                this._GalleryDatabase = value;
                try
                {
                    // Creating Db connection with database
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
                }
                catch (Exception ex)
                {
                    this.DBConnection = null;
                }
            }
        }

        private String _SearchString = String.Empty;
        /// <summary>
        /// 
        /// Gets or Sets Search string: Input for Search
        /// </summary>
        public String SearchString
        {
            get 
            {
                return this._SearchString; 
            }
            set
            {
                this._SearchString = value;

                // Initialize Search
                if (_SearchString.Length > 0)
                {
                    // Get all keyword in a array splited at space
                    keywords = GetAbsoluteKeyword(_SearchString);
                   
                }
            }
        }

        private Boolean _IsAdvanceSearch = false;
        /// <summary>
        /// This Indicate whether SearchType is Advance or not
        /// In case of Advance search "And" clause will be used in Query
        /// </summary>
        public Boolean IsAdvanceSearch
        {
            get
            {
                return _IsAdvanceSearch;
            }
            set 
            {
                _IsAdvanceSearch = value; 
            }
        }

        private string _Condition = "B";
        /// <summary>
        /// Gets/sets search Conditon.Values may be "B :SimpleSearch, A :Advanced all Words only, AX : Advanced with All and exact words and X: Exact only"
        /// </summary>
        public String Condition
        {
            get
            { 
                return this._Condition; 
            }
            set
            {
               this._Condition = value; 
            }
        }

        private XmlDocument _SearchResult = new XmlDocument();
        /// <summary>
        /// Gets Search result 
        /// </summary>
        public XmlDocument SearchResult
        {
            get
            {
                return _SearchResult; 
            }
        }

       private int _GalleryIdForSearch = -1;
       /// <summary>
       /// Gets or Sets LastUsedGalleryId for search
       /// </summary>
       public int GalleryIdForSearch
        {
            get 
            {
                return this._GalleryIdForSearch; 
            }
            set
            {
                this._GalleryIdForSearch = value; 
            }
        }	
           
       #endregion

       #region " -- Methods -- "

       #region " -- Constructor -- "

       // Default Constructor
       public GallerySearch()
       {
       }

       /// <summary>
       /// Constructor the Database location
       /// </summary>
       /// <param name="galleryDatabasePath"></param>
       public GallerySearch(string galleryDatabasePath)
       {
            if (!string.IsNullOrEmpty(galleryDatabasePath))
            {
                this._GalleryDatabase = galleryDatabasePath;                
            }            
       }

       /// <summary>
        /// Constructor using DIconnection
        /// </summary>
        /// <param name="dBConnection"></param>
       public GallerySearch(DIConnection dBConnection)
        {
            //Set DBConnection
            this.DBConnection = dBConnection;        
        }
       
#endregion

       /// <summary>
        /// Return Xml Content for Search result 
        /// </summary>
        /// <param name="resultNIds">Comma seperated list of ResultNids </param>
        /// <returns></returns>
       public XmlDocument GetResultInXML(String resultNIds)
        {
            XmlDocument Retval = new XmlDocument();

            //////Create Xml form Result Informations            
            ////if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
            ////{
            ////    //Get DBConnection
            ////    try
            ////    {
            ////        // Creating Db connection with database
            ////        this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
            ////    }
            ////    catch (Exception ex)
            ////    {
            ////        this.DBConnection = null;
            ////    }
            ////}

            ////// Get Result Informations
            ////DataSet ResultDS = GetResultDataSetForXML(resultNIds);
            ////Retval = GetXml(ResultDS);

            Retval = GetResultInXML(resultNIds,-1,-1);
            return Retval;

        }

        /// <summary>
        /// Return Xml Content for Search result 
        /// </summary>
        /// <param name="resultNIds">Comma seperated list of ResultNids </param>
        /// <returns></returns>
        public XmlDocument GetResultInXML(String resultNIds,int pageNo,int pageSize)
        {
            XmlDocument Retval = new XmlDocument();

            //Create Xml form Result Informations            
            if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
            {
                //Get DBConnection
                try
                {
                    // Creating Db connection with database
                    this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
                }
                catch (Exception ex)
                {
                    this.DBConnection = null;
                }
            }

            // Get Result Informations
            DataSet ResultDS = GetResultDataSetForXML(resultNIds);
            Retval = GetXml(ResultDS,pageNo,pageSize);
            return Retval;

        }
       
       /// <summary>
       /// Start search in database. Pass search condition as blank if it is not an advance search
       /// </summary>
       /// <param name="searchString">Search string to be searched in database</param>
        /// <param name="SearchCondition">In case of simple search, It will be B or in case of Adv search it may be  "A" = All Key Words, "" = Simple Search, "X" = Exact Phrase only, "AX" - All and Exact Phrase</param>
       /// <param name="searchType">Search Type: Values from All,Table,Graph or Map</param>
       /// <returns></returns>
       public string StartGallerySearch(String searchString,string SearchCondition,SearchType searchType)
       {
           string RetVal = string.Empty;          
           RetVal = this.StartGallerySearch(searchString, SearchCondition, searchType, -1);
           return RetVal;
       }

       /// <summary>
       /// Start search in database. Pass search condition as blank if it is not an advance search
       /// </summary>
       /// <param name="searchString">Search string to be searched in database</param>
       /// <param name="SearchCondition">In case of simple search, It will be B or in case of Adv search it may be  "A" = All Key Words, "" = Simple Search, "X" = Exact Phrase only, "AX" - All and Exact Phrase</param>
       /// <param name="searchType">Search Type: Values from All,Table,Graph or Map</param>
       /// <param name="galleryId">gallery Id for which search will be performed .if Gallery id is 0 search in all gallery</param>
       /// <returns></returns>
       public string StartGallerySearch(String Keywords, string AdvanceSearchCondition, SearchType searchType,int galleryId)
       {
           string RetVal = string.Empty;
           
          // // -- Check Advance SearchString
          // // -- If search String is empty then it is Simple Search and set search Condition to B Blank (Simple)Search
          // if (string.IsNullOrEmpty(AdvanceSearchCondition))
          // {
          //     AdvanceSearchCondition = "B";
          // }

          // //Set Last Used Gallery Id used for search only
          //this._GalleryIdForSearch = galleryId;

          // // First Call Predefined Search
          // try
          // {
          //     //Get DBConnection
          //     if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
          //     {
          //         //Get DBConnection
          //         try
          //         {
          //             // Creating Db connection with database
          //             this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
          //         }
          //         catch (Exception ex)
          //         {
          //             this.DBConnection = null;
          //         }
          //     }

          //     //Set Search Condition
          //     this.Condition = AdvanceSearchCondition;
          //     this.SearchFor = searchType;
          //     try
          //     {
          //         RetVal = this.PreDefindSearch(Keywords, AdvanceSearchCondition, searchType);
          //     }
          //     catch (Exception ex)
          //     {
          //         RetVal = null;
          //     }
               

          //     if (string.IsNullOrEmpty(RetVal))
          //     {
          //         // Set Properties For search
          //         if (AdvanceSearchCondition == "B")
          //         {
          //             this.IsAdvanceSearch = false;
          //         }
          //         else
          //         {
          //             this.IsAdvanceSearch =true;
          //         }
          //         // Set search string . 
          //         this._SearchString = Keywords;
          //         if (_SearchString.Length > 0)
          //         {
          //             // Get all keyword in a array splited at space
          //             keywords = GetAbsoluteKeyword(_SearchString);

          //         }
          //         //This will initiate new search
          //         this.StartNewSearch();
          //         // Get Search Result
          //         RetVal = this.SearchResult.InnerXml.ToString();
          //     }
          // }
          // catch (Exception ex)
          // {
          //     if (this.DBConnection != null)
          //     {
          //         this.DBConnection.Dispose();
          //     }
          // }

          // //Dispose Connection
          // if (this.DBConnection != null)
          // {
          //     this.DBConnection.Dispose();
          // }
           RetVal = this.StartGallerySearch(Keywords, AdvanceSearchCondition, searchType, galleryId, -1, -1);
          return RetVal;
       }


       public string StartGallerySearch(String Keywords, string AdvanceSearchCondition, SearchType searchType, int galleryId,int pageNo, int pageSize)
       {
           string RetVal = string.Empty;     
           // -- Check Advance SearchString
           // -- If search String is empty then it is Simple Search and set search Condition to B Blank (Simple)Search
           if (string.IsNullOrEmpty(AdvanceSearchCondition))
           {
               AdvanceSearchCondition = "B";
           }

           //Set Last Used Gallery Id used for search only
           this._GalleryIdForSearch = galleryId;

           // First Call Predefined Search
           try
           {
               //Get DBConnection
               if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
               {
                   //Get DBConnection
                   try
                   {
                       // Creating Db connection with database
                       this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
                   }
                   catch (Exception ex)
                   {
                       this.DBConnection = null;
                   }
               }

               //Set Search Condition
               this.Condition = AdvanceSearchCondition;
               this.SearchFor = searchType;
               try
               {
                   RetVal = this.PreDefindSearch(Keywords, AdvanceSearchCondition, searchType,pageNo,pageSize);
               }
               catch (Exception ex)
               {
                   RetVal = null;
               }


               if (string.IsNullOrEmpty(RetVal))
               {
                   // Set Properties For search
                   if (AdvanceSearchCondition == "B")
                   {
                       this.IsAdvanceSearch = false;
                   }
                   else
                   {
                       this.IsAdvanceSearch = true;
                   }
                   // Set search string . 
                   this._SearchString = Keywords;
                   if (_SearchString.Length > 0)
                   {
                       // Get all keyword in a array splited at space
                       keywords = GetAbsoluteKeyword(_SearchString);

                   }
                   //This will initiate new search
                   this.StartNewSearch(pageNo,pageSize);
                   // Get Search Result
                   RetVal = this.SearchResult.InnerXml.ToString();
               }
           }
           catch (Exception ex)
           {
               if (this.DBConnection != null)
               {
                   this.DBConnection.Dispose();
               }
           }

           //Dispose Connection
           if (this.DBConnection != null)
           {
               this.DBConnection.Dispose();
           }
           return RetVal;
       }


       /// <summary>
       /// Get All Presentations of Type
       /// </summary>
       /// <param name="searchType">Search Type: All,Table,Graph or Map</param>
       /// <returns></returns>
       public String GetAllPresentaions(SearchType searchType,int GalleryId)
       {

           string RetVal = string.Empty;
           String PresentationNids = string.Empty;
           this._GalleryIdForSearch = GalleryId;
           try
           {
               if (this.DBConnection == null || ((System.Data.OleDb.OleDbConnection)this.DBConnection.GetConnection()).State == ConnectionState.Closed)
               {
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }

               // Get all PresentationNid from PresMst
               PresentationNids = this.GetAllPresNId(searchType);

               // If  presentaionNId found then get xml in string.
               if (!string.IsNullOrEmpty(PresentationNids))
               {
                   // Get result xml from these PresNids
                   RetVal = this.GetResultInXML(PresentationNids).InnerXml.ToString();
               }
               if (this.DBConnection != null)
               {
                   this.DBConnection.Dispose();
                   this.DBConnection = null;
               }
           }
           catch (Exception)
           {
           }

           return RetVal;
       }

       /// <summary>
       /// Get All Presentations of Type
       /// </summary>
       /// <param name="searchType">Search Type: All,Table,Graph or Map</param>
       /// <returns></returns>
       public String GetAllPresentaions(SearchType searchType)
       {
           string RetVal = string.Empty;
           String PresentationNids = string.Empty;
           try
           {
               if (this.DBConnection == null || ((System.Data.OleDb.OleDbConnection)this.DBConnection.GetConnection()).State==ConnectionState.Closed)
               {
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }
               
               // Get all PresentationNid from PresMst
               PresentationNids = this.GetAllPresNId(searchType);

               // If  presentaionNId found then get xml in string.
               if (!string.IsNullOrEmpty(PresentationNids))
               {
                   // Get result xml from these PresNids
                   RetVal = this.GetResultInXML(PresentationNids).InnerXml.ToString();
               }
               //if (this.DBConnection !=null)
               //{
               //    this.DBConnection.Dispose();
               //    this.DBConnection = null;
               //}
           }
           catch (Exception)
           {   
           }
           
           return RetVal;
       }

       ///// <summary>
       ///// Get All Presentations NId, Name and FolderPath
       ///// </summary>       
       ///// <returns></returns>
       public DataTable GetAllPresentaions()
       {
           DataTable RetVal = null;

           try
           {
               if (this.DBConnection == null || ((System.Data.OleDb.OleDbConnection)this.DBConnection.GetConnection()).State == ConnectionState.Closed)
               {
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }

               string sSql = "SELECT P." + PresentationMaster.Pres_NId + ", P." + PresentationMaster.Pres_FileName   + " AS " + SearchResultFields.file+" , G." + GalleryMaster.GalleryFolder +
               " AS "+ SearchResultFields.gallery +"  FROM " + DBTable.UT_PresMst + " AS P, " + DBTable.UT_GalleryMst + " AS G  WHERE G." + GalleryMaster.GalleryFolderNId + " =P." + PresentationMaster.GalleryId;

               RetVal = this.DBConnection.ExecuteDataTable(sSql);
           }
           catch (Exception)
           {
           }
           return RetVal;
       }


       //// Can be used when all properties are set
       //public string StartGallerySearch()
       //{
       //    this.PerformSearch(this._SearchString, this._Condition, this._SearchFor, this._LastUsedGalleryId);
       //}
       //// Can be used without setting the properties
       //public string StartGallerySearch(String searchString, string SearchCondition, SearchType searchType, int GalleryId)
       //{
       //    this.PerformSearch(searchString, SearchCondition, searchType, GalleryId);
       //}
       //// Can be used without setting the properties and when all Galleries need to be searched
       //public string StartGallerySearch(String searchString, string SearchCondition, SearchType searchType)
       //{
       //    this.PerformSearch(searchString, SearchCondition, searchType, -1);
       //}

       //private string PerformSearch(String searchString, string SearchCondition, SearchType searchType, int GalleryId)
       //{
       //    String SortedPresNIds = String.Empty;

       //    //Get Sorted Presentation NIDs
       //    SortedPresNIds = SetRankedResultNIds();

       //    // If ResultNIDs is not Blank,Gett  search result( Nid,PresFilename from PresMaster)
       //    if (!string.IsNullOrEmpty(SortedPresNIds))
       //    {
       //        _SearchResult = GetResultInXML(SortedPresNIds);
       //    }
       //    //If No record Found return a blank Xml 
       //    else
       //    {
       //        _SearchResult = GetXMLDoc("root");
       //    }
       //}


       /// <summary>
       /// Get All Available  Gallery details from Database
       /// </summary>
       /// <returns></returns>
       public DataTable GetAllGalleryFromDB()
       {
           DataTable Retval = new DataTable();
           this.ChkAndCreateConnection();
           try
           {
               string sSql = "SELECT * FROM " + DBTable.UT_GalleryMst;
               Retval = this.DBConnection.ExecuteDataTable(sSql);
           }
           catch (Exception)
           {               
           }
           ////Dispose Connection
           //if (this.DBConnection != null)
           //{
           //    this.DBConnection.Dispose();
           //}
           return Retval;

       }

       /// <summary>
       /// Check connection if do not exist create new one
       /// </summary>
       private void ChkAndCreateConnection()
       {
            if(this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
             {              
               //Get DBConnection
               try
               {
                   // Creating Db connection with database
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }
               catch (Exception ex)
               {
                   this.DBConnection = null;
               }
             }

             // if Connection is closed .create new connection
             if (this.DBConnection.GetConnection().State == ConnectionState.Closed)
             {
                 try
                 {
                     // Creating Db connection with database
                     this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
                 }
                 catch (Exception ex)
                 {
                     this.DBConnection = null;
                 }
             }
       }

      
      /// <summary>
       ///  Get count of all presentaions in database for last Gallery
      /// </summary>
      /// <param name="galleryId"> Id of the gallery for which  count is required 
      /// -1  for all gallery </param>
      /// <returns></returns>
       public int GetPresentaionCount(int galleryId)
       {
           int RetVal = 0;
           string sSql = string.Empty;
           this._GalleryIdForSearch = galleryId;
           try
           {
               //Chk conection
               if (this.DBConnection == null || this.DBConnection.GetConnection().State == ConnectionState.Closed)
               {
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }

               if (this._GalleryIdForSearch==-1)
               {
                   sSql = "SELECT count(*) FROM " + DBTable.UT_PresMst;                   
               }
               else
               {
                   sSql = "SELECT count(*) FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.GalleryId+ " = " + this._GalleryIdForSearch;
               }
               //Get Pres count
               
               RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(sSql));
           }
           catch (Exception ex)
           {   
           }
           //if (this.DBConnection !=null)
           //{
           //    this.DBConnection.Dispose();
           //}
           return RetVal;
       }

       public static int GetPresentaionCountByType(string galleryDBPath,Presentation.PresentationType presType)
       {
           int RetVal = 0;
           string sSql = string.Empty;
           DIConnection DBConn=null;
           try
           {
               if (!string.IsNullOrEmpty(galleryDBPath))
               {
                   //Chk conection 

                   DBConn = new DIConnection(DIServerType.MsAccess, "", "", galleryDBPath, "", "");
              
               }
               switch (presType)
               {
                   case Presentation.PresentationType.Table:
                       sSql = "SELECT count(*) FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_Type+ " = 'T'" ;
                       break;
                   case Presentation.PresentationType.Graph:
                       sSql = "SELECT count(*) FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_Type + " = 'G'";
                       break;
                   case Presentation.PresentationType.Map:
                       sSql = "SELECT count(*) FROM " + DBTable.UT_PresMst + " WHERE " + PresentationMaster.Pres_Type + " = 'M'";
                       break;
                  
                   default:
                       break;
               }

               //If sql string is not null
               if (!string.IsNullOrEmpty(sSql))
               {
                   RetVal = Convert.ToInt32(DBConn.ExecuteScalarSqlQuery(sSql));
               }

           }
           catch (Exception ex)
           {

           }
           DBConn.Dispose();
           return RetVal;

       }

       /// <summary>
       /// Get Search Result Count
       /// </summary>
       /// <param name="searchKeyword">Keyword to be searched</param>
       /// <param name="searchCondition">search condition.This  may be "" or "B" in case of simple search </param>
       /// <param name="searchType">ALL ,Table ,Graph,Map</param>
       /// <param name="galleryIdForSearch">Gallery in which searched -1 for search in all Gallery</param>
       /// <returns>Search Result count</returns>
       public int GetSearchResultCount(String searchKeyword, String searchCondition,SearchType searchType,int galleryIdForSearch)
       {
           int RetVal = 0;
           this.ResultNIds = string.Empty;

           // -- Check Advance SearchString
           // -- If search condition is empty then it is Simple Search and set search Condition to B Blank (Simple)Search
           if (string.IsNullOrEmpty(searchCondition))
           {
               searchCondition = "B";
           }

           //Set Last Used Gallery Id used for search only
           this._GalleryIdForSearch = galleryIdForSearch;
           //Set Search Condition
           this.Condition = searchCondition;
           this.SearchFor = searchType;

           // Set search string . 
           this._SearchString = searchKeyword;
           if (this._SearchString.Length > 0)
           {
               // Get all keyword in a array splited at space
               this.keywords = this.GetAbsoluteKeyword(this._SearchString);
           }

           //Get DBConnection
           if (this.DBConnection == null && !string.IsNullOrEmpty(this._GalleryDatabase))
           {
               //Get DBConnection
               try
               {
                   // Creating Db connection with database
                   this.DBConnection = new DIConnection(DIServerType.MsAccess, "", "", this._GalleryDatabase, "", "");
               }
               catch (Exception ex)
               {
                   this.DBConnection = null;
               }
           }

           //  Get Search Result
           if (this.Condition != "B")
           {
               ResultNIds = this.GetResultNIdsForAdvSearch(keywords);
           }
           else      //Normal Search (OR Clause will be used in query while searching)
           {
               //if (!String.IsNullOrEmpty(this._SearchString))
               //{
               //    this.GetResultNIdsUsingRecursiveSearch(this._SearchString);
               //}
               //else  //earlier logic
               //{
                   ResultNIds = this.GetResultNIds(keywords);
               //}
               
           }
          
          // string searchResult= this.StartGallerySearch(searchKeyword, searchCondition, searchType, galleryIdForSearch);
           if (!string.IsNullOrEmpty(this.ResultNIds))
           {
             //string[] SearchResults()  
               // Spliting resultNid string into an array
               String[] ResultNIdArray = this.ResultNIds.Split(',');
               RetVal = ResultNIdArray.Length;
           }
           return RetVal;
       }

       /// <summary>
       /// Close database connection
       /// </summary>
       public void CloseConnection()
       {
           if (this.DBConnection != null)
           {
               this.DBConnection.Dispose();
               this.DBConnection = null;
           }
       }

       /// <summary>
       /// Get Last Used Gallery Location .
       /// </summary>
       /// <returns></returns>
       public string GetLastUsedGalleryLocation()
       {
           String Retval = string.Empty;           
           try
           {              
               this.ChkAndCreateConnection();            
               string sSql = "SELECT  " + GalleryMaster.GalleryFolder +" FROM " + DBTable.UT_GalleryMst + " WHERE " + GalleryMaster.LastUsed + " = true";
               Retval = this.DBConnection.ExecuteScalarSqlQuery(sSql).ToString();
                
           }
           catch (Exception ex)
           {
               Retval = string.Empty;
           }
           return Retval;
       }
       
        #endregion

        #endregion
    }

    /// <summary>
    /// Class holding Constants for search result Field
    /// </summary>
    public class SearchResultFields
    {
        /// <summary>
        /// file
        /// </summary>
        public const String file = "file";
        /// <summary>
        /// id
        /// </summary>
        public const String id = "id";
        /// <summary>
        /// name
        /// </summary>
        public const String name = "name";
        /// <summary>
        /// type
        /// </summary>
        public const String type = "type";
        /// <summary>
        /// gallery
        /// </summary>
        public const String gallery = "gallery";
        public const String GalleryId = "GalleryId";
        public const String Path = "Path";

      
    }
}
