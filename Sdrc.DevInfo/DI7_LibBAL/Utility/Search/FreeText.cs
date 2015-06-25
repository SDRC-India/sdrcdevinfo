using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Utility.Search
{
    /// <summary>
    ///  This Class meant for getting search result on the basis of keyword 
    ///  and search creteria specified.
    /// </summary> 
    ///     
    /// These examples will demonstrate how to use this class
    /// <example >
    /// 
    /// This example demonstrate How to Use this class for SMS Application
    /// <code lang ="VB">
    ///  DataView Dv = new DataView();
    ///  Dim FreeText as New FreeText(goDIConnection, goDIQueries,"")
    ///  FreeSearch.SMSPageSize = 3
    ///  FreeSearch.SimpleSearch(str, FreeText.SearchType.SMS);    
    ///  Dv = FreeSearch.SMSData;   
    /// </code>
    /// 
    /// This example demonstrate How to Use this class for Simple Search
    /// <code lang ="C#">
    ///   FreeSearch = new FreeText(0, "dgps", "", "test", "sa", "", "C:\\-- Projects --\\DevInfo 5.0 - VS 2005\\Language\\DI_English [en].xml");
    ///                                 or
    ///   Freetext FreeSearch = new FreeText(goDIConnection, goDIQueries, gsFldrLng & gsLngName & ".xml");
    ///   FreeSearch.DatabasePrefix = "SN_";
    ///   FreeSearch.LanguageCode = "_en";
    ///   FreeSearch.SimpleSearch(SearchText.txt, Freetext.SearchType.All);
    ///   goUserSelection.Indicators = goFreeText.IUSNId
    ///   goUserSelection.Areas = goFreeText.AreaNId
    ///   goUserSelection.Time = goFreeText.TimePeriodNId
    /// </code> 
    /// </example>
    /// <remarks> 
    ///This class contains logic for simple as well as Advanced Search.
    /// this will be Consumed by different pages of Devinfo UserInterface/web/SMS project to ficilate
    /// Search option. This also aims to bring consistancy of search options for all pages
    ///  </remarks>        
    public class FreeText
    {
        #region "-- Private --"

        #region "-- Constants --"
        const string WEIGHTAGE_COL1 = "Weightage1";
        const string WEIGHTAGE_COL2 = "Weightage2";

        // Constants for Advance Search Field Name
        public const string INDICATOR = "indicator";
        public const string UNIT = "unit";
        public const string SUBGROUP = "subgroup";
        public const string AREA_NAME = "areaname";
        public const string AREAID = "areaid";
        public const string SOURCE = "source";
        public const string HIDDEN = "hidden";
        public const string TIMEPERIOD = "timeperiod";

        #endregion

        #region "-- Variable --"
        private DIConnection DIConnection;
        private DIQueries DIQueries;
        private Boolean AutoSelectIndicatorForAdvSearch = true;
        private Boolean AutoSelectTimePeriodForAdvSearch = true;
        private Boolean AutoSelectAreaForAdvSearch = true;
        const string WordCount = "WordCount";
        const string SearchKeyword = "SearchKeyword";
        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Return search result in form of comma seperated list of IndicatorNId
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetIndicatorNIds(string[] searchString)
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
                    FilterClause += Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //FilterClause += Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //end of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }

            //Running Query to get IndicatorNId's    
            sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.Search, FilterClause, FieldSelection.Light);
            try
            {
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator.IndicatorNId);
                rd.Close();
            }
            catch (Exception ex)
            {
                //rd.Close();
            }

            // If there are some indicator NIds 
            if (RetVal.Length > 0)
            {
                // Only those IndicatorNIds are required, those have data values against them.
                // RetrieveIndicator_IUS==Indicator_IUS_Option.IndicatorNId
                if (_GetIndicatorByDataOnly && !this._ShowIUS)
                {
                    // --Check if Data exist against these indicatorNIds
                    // Step1: Split NId string in an array
                    //string[] TempIndNIds = RetVal.Split(',');

                    //Step2:  Clear retval. Only those NIds will be returned those have data
                    ////RetVal = string.Empty;

                    // Step3. Get All indicatorNids those have data against them

                    // IndicatorNid is used to get ius NIds from IUS Table
                    // IUS is checked into data table 
                    //TODO: QUERY TO BE IN DAL
                    sSql = "SELECT DISTINCT I." + Indicator.IndicatorNId + "  FROM " + this.DIQueries.TablesName.Data + " Data, " +
                    DIQueries.TablesName.Indicator + " I ," + this.DIQueries.TablesName.IndicatorUnitSubgroup +
                    "  IUS WHERE Data." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId + " AND IUS." +
                    Indicator_Unit_Subgroup.IndicatorNId + " = I." + Indicator.IndicatorNId
                    + " AND I." + Indicator.IndicatorNId + " IN (" + RetVal + ")";

                    // Execute command
                    rd = DIConnection.ExecuteReader(sSql);

                    // Get all indicator Nids with data in a string
                    AllIndicatorNIdsWithData = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator.IndicatorNId);
                    rd.Close();

                    // Update Retval With all IndicatorNid with data
                    RetVal = AllIndicatorNIdsWithData;


                }
            }

            return RetVal;
        }

        /// <summary>
        /// Return autoselected comma seperated list of IndicatorNId
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string AutoSelectIndicatorNIds(string areaNId, string timePeriodNId)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            //Running Query to get IndicatorNId's  
            sSql = DIQueries.Indicators.GetAutoSelectByTimePeriodArea(timePeriodNId, areaNId, -1, FieldSelection.NId);

            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator.IndicatorNId);
            rd.Close();
            return RetVal;
        }



        /// <summary>
        /// Return autoselected comma seperated list of IUSNId
        /// </summary>
        /// <param name="areaNId"></param>
        /// <param name="timePeriodNId"></param>
        /// <returns></returns>
        private string AutoSelectIUSNIds(string areaNId, string timePeriodNId)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            //Running Query to get IUSNId's               
            sSql = DIQueries.IUS.GetAutoSelectByTimePeriodArea(timePeriodNId, areaNId, -1, FieldSelection.NId);
            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IUSNId);
            rd.Close();
            return RetVal;
        }
        /// <summary>
        /// Return  comma seperated list of  autoselected timeperiodNId
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string AutoSelectTimeperiodNIds(string areaNId, string indicatorNId)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;

            //Running Query to get TimeperiodNId's on the basis of indicator and areaNIds
            // Either AreaNid or Indicator Nid may be Blank  
            if (this._ShowIUS)
            {
                sSql = DIQueries.Timeperiod.GetAutoSelectByIUSArea(indicatorNId, areaNId);
            }
            else
            {
                sSql = DIQueries.Timeperiod.GetAutoSelectByIndicatorArea(indicatorNId, areaNId);
            }

            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Timeperiods.TimePeriodNId);
            rd.Close();
            return RetVal;
        }


        /// <summary>
        /// Return comma seperated list of AreaNid
        /// </summary>
        /// <param name="searchString">Keyword to be searched </param>
        private string GetAreaNIdsByAreaName(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
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
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += Area.AreaName + " =" + DICommon.EscapeWildcardChar(searchString[i]) ;
                    FilterClause += Area.AreaName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += Area.AreaName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //FilterClause += Area.AreaName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }
            try
            {
                //Running Query to get AreaNId's    
                sSql = DIQueries.Area.GetArea(FilterFieldType.Search, FilterClause);
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Area.AreaNId);
                rd.Close();
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Return search result in form of comma seperated list of AreaNid
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetAreaNIdsByAreaID(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
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
                //change on 14-07-08. in case of phrase search use like instead of equal to.
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += Area.AreaID + " = " +  DICommon.EscapeWildcardChar(searchString[i])  ;
                    FilterClause += Area.AreaID + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length))) + "%'";
                }
                else
                {
                    FilterClause += Area.AreaID + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //FilterClause += Area.AreaID + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //end of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }

            //Running Query to get AreaNId's    
            sSql = DIQueries.Area.GetArea(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Area.AreaNId);
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// Return  comma seperated list of autoselected AreaNId
        /// </summary>      
        private string AutoSelectAreaNIds(string indicatorNId, string timePeriodNId)
        {
            string RetVal = "";
            string sSql = "";
            string AreaBlocks = string.Empty;
            IDataReader rd;

            //-- Get the Area block on the basis of area parent NId
            sSql = DIQueries.Area.GetParentData("-1");
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                AreaBlocks = rd[Area.AreaBlock].ToString();
            }
            // Close Datareader
            rd.Close();

            //Running Query to get AreaNId's on the basis of indicator and timeperiod     
            if (this._ShowIUS)
            {
                sSql = DIQueries.Area.GetAutoSelectByIUSTimePeriod(-1, 0, 0, indicatorNId, timePeriodNId, AreaBlocks);
            }
            else
            {
                sSql = DIQueries.Area.GetAutoSelectByIndicatorTimePeriod(-1, 0, 0, indicatorNId, timePeriodNId, AreaBlocks);
            }

            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Area.AreaNId);
            rd.Close();
            return RetVal;
        }


        /// <summary>
        /// Return search result in form of comma seperated list of TimeNid
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetTimeNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)
            // -- Adding each element of SearchString  into FilterClause
            for (int i = 0; i <= searchString.Length - 1; i++)
            {
                if (FilterClause.Length > 0)
                {
                    FilterClause += " OR ";
                }

                //change on 14-07-08. In case of phrase search(inside double quote) use like instead of equal 
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += Timeperiods.TimePeriod + " = " + DICommon.EscapeWildcardChar(searchString[i]) ;
                    FilterClause += Timeperiods.TimePeriod + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += Timeperiods.TimePeriod + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //FilterClause += Timeperiods.TimePeriod + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }
            try
            {
                //Running Query to get TimePeriodNId's    
                sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.Search, FilterClause);
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Timeperiods.TimePeriodNId);
                rd.Close();
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Return search result in form of comma seperated list of UnitNid
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetUnitNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
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
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    FilterClause += Unit.UnitName + " =" + DICommon.EscapeWildcardChar(searchString[i]);
                }
                else
                {
                    FilterClause += Unit.UnitName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //                FilterClause += Unit.UnitName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                // --End of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }
            try
            {
                //Running Query to get UnitNId's    
                sSql = DIQueries.Unit.GetUnit(FilterFieldType.Search, FilterClause);
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Unit.UnitNId);
                rd.Close();
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Return search result in form of comma seperated list of SubGroupNid
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetSubGroupNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
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
                //change on 14-07-08.If searchString[i] is in quote use like in query instead of equal to (=) 
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += SubgroupVals.SubgroupVal + " = " + DICommon.EscapeWildcardChar(searchString[i]) ;
                    FilterClause += SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString.Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //                FilterClause += SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //end of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }
            //Running Query to get SubGroupNId's    
            sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, SubgroupVals.SubgroupValNId);
            rd.Close();
            return RetVal;
        }


        /// <summary>
        /// Return search result in form of comma seperated list of SourceNId
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetSourceNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)
            // -- Adding each element of SearchString  into FilterClause
            for (int i = 0; i <= searchString.Length - 1; i++)
            {
                if (FilterClause.Length > 0)
                {
                    FilterClause += " OR ";
                }


                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    FilterClause += IndicatorClassifications.ICName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += IndicatorClassifications.ICName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
            }
            try
            {
                //Running Query to get SourceNId 

                if (!string.IsNullOrEmpty(FilterClause))
                {
                    sSql = "SELECT DISTINCT " + IndicatorClassifications.ICNId
                    + " FROM " + DIQueries.TablesName.IndicatorClassifications + " AS IC WHERE " + IndicatorClassifications.ICType + "='SR' "
                    + " AND " + FilterClause;
                    rd = DIConnection.ExecuteReader(sSql);
                    RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, IndicatorClassifications.ICNId);
                    rd.Close();
                }

            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }


        /// <summary>
        /// Return comma seperated list of IUSNId 
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string GetIUSNIds(string[] searchString, string indicatorIUSNId)
        {
            String RetVal = string.Empty;
            string sSql = "";
            IDataReader rd;
            string AllIUSNIdWithData = string.Empty;

            // --get IndicatorNIds inside _Indicator_IUS_NId  in order to get IUSNids
            indicatorIUSNId = GetIndicatorNIds(searchString);

            //if  Indicator has a value  get IUSNid
            if (indicatorIUSNId.Length > 0)//|| UnitNIds.Length > 0 || SubgroupNIds.Length > 0
            {
                // -- Getting IUSNids by using IndicatorId(used " " for Unit and Subgroup Nids) in query
                sSql = DIQueries.IUS.GetIUSNIdByI_U_S(indicatorIUSNId, "", "");
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IUSNId);
                rd.Close();

                // Only those IUSNIds are required, those have data values against them
                if (_GetIndicatorByDataOnly)
                {
                    //// --Check if Data exist against these indicatorNIds
                    //string[] TempIUSNIds = RetVal.Split(',');


                    // Query for checking is data is in for this IUS Nid.                        
                    // IUS is checked into data table .
                    //TODO: move this query for getting all IUS with Data to DAL
                    sSql = "SELECT DISTINCT IUS." + Indicator_Unit_Subgroup.IUSNId + "  FROM " + DIQueries.TablesName.Data + " Data, " +
                    DIQueries.TablesName.IndicatorUnitSubgroup +
                    "  IUS WHERE Data." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId + " AND IUS." +
                    Indicator_Unit_Subgroup.IUSNId + " IN (" + RetVal + ")";

                    //execute command
                    rd = DIConnection.ExecuteReader(sSql);
                    AllIUSNIdWithData = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IUSNId);
                    rd.Close();

                    // Update Retval With all IUS with data
                    RetVal = AllIUSNIdWithData;

                }
            }
            return RetVal;
        }

        /// <summary>
        /// Return whether keyword is found in indicator table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private Boolean IsIndicator(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get IndicatorNId's    
            sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.Search, FilterClause, FieldSelection.NId);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// Return whether keyword is found in Unit table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private Boolean IsUnit(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = Unit.UnitName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get UnitNId's    
            sSql = DIQueries.Unit.GetUnit(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// Return whether keyword is found in SubgroupVal table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private Boolean IsSubgroup(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get subgroupNId's    
            sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// Return whether keyword is found in Area table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched for area name</param>
        private Boolean IsArea(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = Area.AreaName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get AreaNId's    
            sSql = DIQueries.Area.GetArea(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// Return whether keyword is found in Area table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched for area Id</param>
        private Boolean IsAreaId(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = Area.AreaID + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get AreaNId's    
            sSql = DIQueries.Area.GetArea(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }


        /// <summary>
        /// Return whether keyword is found in timeperiod table or not
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private Boolean IsTimePeriod(string searchString)
        {
            Boolean RetVal = false;
            string sSql = "";
            IDataReader rd;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause        
            FilterClause = Timeperiods.TimePeriod + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString)) + "%'";

            //Running Query to get IndicatorNId's    
            sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.Search, FilterClause);
            rd = DIConnection.ExecuteReader(sSql);
            if (rd.Read())
            {
                RetVal = true;
            }
            rd.Close();
            return RetVal;
        }

        /// <summary>
        /// To Get IUS NId/IndicatorNId on the basis of parameter getIUSNId
        /// </summary>
        /// <param name="getIUSNId">True value indicate function will return IUSNId ,
        ///   False  indicate function will return IndicatorNids   </param>
        /// <returns> Comma delimited list of IUSNIds/IndicatorNIds</returns>
        private string GetIndicator_IUSNIdFromAdvanceSearchFileds(Boolean getIUSNId)
        {
            string RetVal = "";
            string[] IndicatorKeywords = null;
            string[] UnitKeywords = null;
            string[] SubgroupKeywords = null;
            string UnitNIds = string.Empty;
            string SubgroupNIds = string.Empty;

            // Loop through Advancedsearchfield collection
            foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
            {
                //If FieldName =indicator and selected=true then get indicator keyword in an array

                if ((AdvanceSearchField.Name.ToLower() == INDICATOR) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                {
                    // -- Get Absolute keyword  for indicator(Without Spaces and Quotes)
                    IndicatorKeywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);
                    for (int i = 0; i < IndicatorKeywords.Length; i++)
                    {
                        // Remove quotes from position other then start and end
                        // Get substring except first and last position
                        if (IndicatorKeywords[i].ToString().StartsWith("'") && IndicatorKeywords[i].ToString().EndsWith("'"))
                        {
                            IndicatorKeywords[i] = IndicatorKeywords[i].Substring(1, IndicatorKeywords[i].Length - 2);
                        }
                        //Remove quotes
                        IndicatorKeywords[i] = DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(IndicatorKeywords[i].ToString()));

                    }
                    AutoSelectIndicatorForAdvSearch = false;
                }
                if ((AdvanceSearchField.Name.ToLower() == UNIT) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                {
                    // -- Get Absolute keyword  for Unit(Without Spaces and Quotes)
                    UnitKeywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);
                    // Remove quotes from position other then start and end
                    for (int i = 0; i < UnitKeywords.Length; i++)
                    {
                        if (UnitKeywords[i].ToString().StartsWith("'") && UnitKeywords[i].ToString().EndsWith("'"))
                        {
                            IndicatorKeywords[i] = UnitKeywords[i].Substring(1, UnitKeywords[i].Length - 2);
                        }
                        UnitKeywords[i] = DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(UnitKeywords[i].ToString()));
                    }
                }

                if ((AdvanceSearchField.Name.ToLower() == SUBGROUP) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                {
                    // -- Get Absolute keyword  for Subgroup(Without Spaces and Quotes)
                    SubgroupKeywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);
                    // Remove quotes from position other then start and end
                    for (int i = 0; i < SubgroupKeywords.Length; i++)
                    {
                        if (SubgroupKeywords[i].ToString().StartsWith("'") && SubgroupKeywords[i].ToString().EndsWith("'"))
                        {
                            SubgroupKeywords[i] = SubgroupKeywords[i].Substring(1, SubgroupKeywords[i].Length - 2);
                        }
                        SubgroupKeywords[i] = DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(SubgroupKeywords[i].ToString()));
                    }
                }
            }
            // If any of three (IndicatorKeywords ,UnitKeywords,SubgroupKeywords) has value use DAL Query to get
            //IndicatorNId or IUSNId
            if (IndicatorKeywords != null || UnitKeywords != null || SubgroupKeywords != null)
            {
                try
                {
                    // Using DAL Query to get Indicator or IUSNid
                    string sSql = DIQueries.Indicators.GetIndicatorNId_IUSNIdForFreeTextSearch(IndicatorKeywords, UnitKeywords, SubgroupKeywords, getIUSNId);
                    IDataReader rd = DIConnection.ExecuteReader(sSql);

                    // Get IUSNId
                    if (getIUSNId)
                    {
                        RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IUSNId);
                    }
                    else          // Get IndicatorNId
                    {
                        RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IndicatorNId);
                    }
                    rd.Close();

                }
                catch (Exception)
                {
                }

            }
            return RetVal;
        }

        ///// <summary>
        ///// To GetIndicator Nid for Advance Search 
        ///// </summary>
        ///// <returns>Comma delimited list of IUSNIds</returns>
        //private string GetIndicatorNIdFromAdvanceSearchFileds()
        //{
        //    string RetVal = "";
        //    string[] keywords;

        //    foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
        //    {
        //        if ((AdvanceSearchField.Name == "Indicator") && (AdvanceSearchField.Selected == true) && ((AdvanceSearchField.SearchText).Length > 0))
        //        {
        //            // -- Get Absolute keyword (Without Spaces and Quotes)
        //            keywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);

        //            // --get IndicatorNIds
        //            RetVal= GetIndicatorNIds(keywords);                    
        //        }         

        //    }
        //    return RetVal;
        //}


        /// <summary>
        /// To Get AreaNid for Advance Search
        /// </summary>
        /// <returns> Comma delimited list of AreaNIds</returns>
        private string GetAreaNIdFromAdvanceSearchFileds()
        {
            string RetVal = "";
            string[] keywords;
            try
            {
                foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
                {
                    if ((AdvanceSearchField.Name.ToLower() == AREA_NAME) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                    {
                        //-- In case user has typed some text for Area name , then supress autoselect for Areas
                        this.AutoSelectAreaForAdvSearch = false;
                        keywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);

                        // --get AreaNIds
                        RetVal = GetAreaNIdsByAreaName(keywords);
                    }
                    if ((AdvanceSearchField.Name.ToLower() == AREAID) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                    {
                        //-- In case user has typed some text for AreaId, then supress autoselect for Areas
                        this.AutoSelectAreaForAdvSearch = false;
                        keywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);

                        // --get AreaNIds 
                        if (RetVal.Length > 0)
                        {
                            RetVal = RetVal + ",";
                            RetVal += GetAreaNIdsByAreaID(keywords);
                        }
                        else
                        {
                            RetVal = GetAreaNIdsByAreaID(keywords);
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;

        }
        /// <summary>
        /// Ruturn Search string after concating search text of each advance search field
        /// This search string will be shown in search text box of host application
        /// </summary>
        /// <returns>String containing search text</returns>
        /// <remarks></remarks>
        private string GetSearchText()
        {
            string RetVal = "";         //Search String to be returned
            //loop through  AdvancedSearchFields List to get SearchText
            foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
            {
                // If AdvancedSearchFields search text is not blank
                if (AdvanceSearchField.SearchText != "")
                {
                    // If search string do not contain any word then  search string =  AdvanceSearchfield search text
                    if (RetVal.Length == 0)
                    {
                        RetVal = AdvanceSearchField.SearchText;
                    }
                    else           // If search string(Retval) already have  one or more word
                    {
                        // step1 :Check is search string(Retval) already have advanceSearchField.SearchText

                        // If Retval do not contain AdvanceSearchField.SearchText                      
                        if (RetVal.Contains(AdvanceSearchField.SearchText) == false)
                        {
                            // If Even AdvanceSearchField.SearchText do not contain  retval
                            // Add  field's Search text in retval
                            if ((AdvanceSearchField.SearchText).Contains(RetVal) == false)
                            {
                                RetVal = RetVal + " " + AdvanceSearchField.SearchText;
                            }
                            // If AdvanceSearchField.SearchText contain retval and lenght of 
                            // AdvanceSearchField.SearchText is  more then retval
                            //Replace retval With AdvanceSearchField.SearchText

                            else if (((AdvanceSearchField.SearchText).Contains(RetVal) == true) && ((AdvanceSearchField.SearchText.Trim()).Length > RetVal.Length))
                            {
                                RetVal = AdvanceSearchField.SearchText;
                            }
                        }

                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Auto Select for Simple Search
        /// </summary>
        private void AutoSelectIUSAreaTimeperiod()
        {
            // -- Autoselect Indicator / IUS
            if (this._Indicator_IUS_NId.Length == 0 && (this._TimePeriodNId.Length > 0 || this._AreaNId.Length > 0))
            {
                if (this._ShowIUS)
                {
                    this._Indicator_IUS_NId = AutoSelectIUSNIds(this._AreaNId, this._TimePeriodNId);
                }
                else
                {
                    this._Indicator_IUS_NId = AutoSelectIndicatorNIds(this._AreaNId, this._TimePeriodNId);
                }

            }
            // -- Autoselect Timeperiod 
            if (this._TimePeriodNId.Length == 0 && (this._AreaNId.Length > 0 || this._Indicator_IUS_NId.Length > 0))
            {
                this._TimePeriodNId = AutoSelectTimeperiodNIds(this._AreaNId, this._Indicator_IUS_NId);
            }

            // -- Autoselect Area 
            if (this._AreaNId.Length == 0 && (this._Indicator_IUS_NId.Length > 0 || this._TimePeriodNId.Length > 0))
            {
                this._AreaNId = AutoSelectAreaNIds(this._Indicator_IUS_NId, this._TimePeriodNId);
            }

        }

        /// <summary>
        /// Auto Select for  Search
        /// </summary>
        private void AutoSelectAdvanceSearchIUSAreaTimeperiod_old()
        {
            if (this._Indicator_IUS_NId.Length == 0 && (this._TimePeriodNId.Length > 0 || this._AreaNId.Length > 0) && AutoSelectIndicatorForAdvSearch)
            {
                if (this._ShowIUS)
                {
                    this._Indicator_IUS_NId = AutoSelectIUSNIds(this._AreaNId, this._TimePeriodNId);
                }
                else
                {
                    this._Indicator_IUS_NId = AutoSelectIndicatorNIds(this._AreaNId, this._TimePeriodNId);
                }
            }

            // -- Autoselect Timeperiod 
            // -- Autoselect only when search field is blank not in case when search text not found
            if (this._TimePeriodNId.Length == 0 && this._Indicator_IUS_NId.Length > 0 && AutoSelectTimePeriodForAdvSearch)
            {
                this._TimePeriodNId = AutoSelectTimeperiodNIds(this._AreaNId, this._Indicator_IUS_NId);
            }

            // -- Autoselect Area 
            if (this._AreaNId.Length == 0 && (this._Indicator_IUS_NId.Length > 0 || this._TimePeriodNId.Length > 0) && AutoSelectAreaForAdvSearch)
            {
                this._AreaNId = AutoSelectAreaNIds(this._Indicator_IUS_NId, this._TimePeriodNId);
            }

            // Reset autoselectIndicator,area,timeperiod to true
            AutoSelectIndicatorForAdvSearch = true;
            AutoSelectTimePeriodForAdvSearch = true;
            AutoSelectAreaForAdvSearch = true;


        }

        /// <summary>
        /// Auto Select IUS Area TimePeriod
        /// </summary>
        private void AutoSelectAdvanceSearchIUSAreaTimeperiod()
        {
            if (this._Indicator_IUS_NId.Length == 0 && (this._TimePeriodNId.Length > 0 || this._AreaNId.Length > 0 || this._SourceNId.Length > 0) && AutoSelectIndicatorForAdvSearch)
            {
                this._Indicator_IUS_NId = this.AutoSelectIndicator_IUSNIdsForAdvSearch();
            }

            // -- Autoselect Timeperiod 
            // -- Autoselect only when search field is blank not in case when search text not found
            if (this._TimePeriodNId.Length == 0 && (this._Indicator_IUS_NId.Length > 0 || this._SourceNId.Length > 0) && AutoSelectTimePeriodForAdvSearch)
            {
                //this._TimePeriodNId = AutoSelectTimeperiodNIds(this._AreaNId, this._Indicator_IUS_NId);
                this._TimePeriodNId = this.AutoSelectTimeperiodNIdsForAdvSearch();
            }

            // -- Autoselect Area 
            if (this._AreaNId.Length == 0 && (this._Indicator_IUS_NId.Length > 0 || this._TimePeriodNId.Length > 0 || this._SourceNId.Length > 0) && AutoSelectAreaForAdvSearch)
            {
                //this._AreaNId = AutoSelectAreaNIds(this._Indicator_IUS_NId, this._TimePeriodNId);
                this._AreaNId = this.AutoSelectAreaNIdsForAdvSearch();
            }

            // Reset autoselectIndicator,area,timeperiod to true
            AutoSelectIndicatorForAdvSearch = true;
            AutoSelectTimePeriodForAdvSearch = true;
            AutoSelectAreaForAdvSearch = true;
        }

        /// <summary>
        /// Return autoselected comma seperated list of IndicatorNId
        /// </summary>
        /// <param name="searchString">Keyword to be searched</param>
        private string AutoSelectIndicator_IUSNIdsForAdvSearch()
        {
            string RetVal = "";
            string AutoSelectdIUSNIds = string.Empty;
            string sSql = "";
            IDataReader rd;
            //Running Query to get IndicatorNId's  

            sSql = DIQueries.Indicators.GetAutoSelectByTimePeriodAreaSource(this._TimePeriodNId, this._AreaNId, string.Empty, this._SourceNId);
            try
            {
                rd = DIConnection.ExecuteReader(sSql);
                AutoSelectdIUSNIds = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator_Unit_Subgroup.IUSNId);
                rd.Close();
            }
            catch (Exception ex)
            {


            }

            //If Show IUS is off Get Indicator Id 
            if (this._ShowIUS)
            {
                RetVal = AutoSelectdIUSNIds;
            }
            else
            {
                if (!string.IsNullOrEmpty(AutoSelectdIUSNIds))
                {
                    sSql = DIQueries.Indicators.GetIndicators(AutoSelectdIUSNIds, this._ShowIUS);

                    rd = DIConnection.ExecuteReader(sSql);
                    RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Indicator.IndicatorNId);
                    rd.Close();
                }

            }
            return RetVal;
        }

        /// <summary>
        /// Return  comma seperated list of  autoselected timeperiodNId
        /// </summary>       
        private string AutoSelectTimeperiodNIdsForAdvSearch()
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;

            //Running Query to get TimeperiodNId's on the basis of indicator and areaNIds and source                       
            sSql = DIQueries.Timeperiod.GetAutoSelectTimeperiod(this._Indicator_IUS_NId, this._ShowIUS, this._AreaNId, this._SourceNId);

            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Timeperiods.TimePeriodNId);
            rd.Close();
            return RetVal;
        }


        /// <summary>
        /// Return  comma seperated list of autoselected AreaNId
        /// </summary>      
        private string AutoSelectAreaNIdsForAdvSearch()
        {
            string RetVal = "";
            string sSql = "";
            string AreaBlocks = string.Empty;
            IDataReader rd;

            sSql = DIQueries.Area.GetAutoSelectAreas(this._Indicator_IUS_NId, this._ShowIUS, this._TimePeriodNId, this._SourceNId, 0);

            rd = DIConnection.ExecuteReader(sSql);
            RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Area.AreaNId);
            rd.Close();
            return RetVal;
        }



        /// <summary>
        ///  Rank result database on the basis of weightage of keyword
        /// </summary>
        /// <param name="keywords"> string array containing search keywords as element</param>
        /// <param name="SmsDataTable">Datatable containing result of sms query</param>
        /// <returns></returns>
        private DataView RankSmsResult(string[] keywords, DataTable SmsDataTable)
        {
            DataView RetVal = new DataView();
            try
            {
                // Ranking depends on two factors
                // 1. Number of search keywords found in result phrase / total no. of search keyword  as Criteria1Weightage
                // 2. Number of search keywords found in result phrase / total no. of words in result phrase  as Criteria2Weightage
                // Where Result Pharase means Indicator + Area + Timeperiod (Excluding Unit, Subgroup and Source)
                // Final ranking is defined by sorting data on the basis of sorting data on the basis
                // of Criteria1Weightage,Creteria2Weightage,IndicatorName and Timeperiod

                float SearchKeywordCount = keywords.Length;     // total no. of search keyword
                float ResultPhraseWordCount = 0;                // total no. of words in result phrase
                float SearchKeywordOccurance = 0;               // Number of search keywords found in result phrase
                string ResultPhrase = string.Empty;             //Hold Words in result Phrase 
                string[] ResultPhraseArray;                     //Hold each word of result as an element of an array


                // float AverageWeightage = 0;                  
                float Criteria1Weightage = 0;   //1. Number of search keywords found in result phrase / total no. of search keyword
                float Criteria2Weightage = 0;   //2. Number of search keywords found in result phrase / total no. of words in result phrase 

                for (int cnt = 0; cnt < SmsDataTable.Rows.Count; cnt++)
                {
                    SearchKeywordOccurance = 0;
                    //  AverageWeightage = 0;           
                    Criteria1Weightage = 0;
                    Criteria2Weightage = 0;

                    // Step 1 Get the number of search keywords found in result phrase
                    // Creating result phrase considering Indicator,area and Timeperiod 
                    ResultPhrase = SmsDataTable.Rows[cnt][Indicator.IndicatorName].ToString() + " " + SmsDataTable.Rows[cnt][Area.AreaName].ToString()
                      + " " + SmsDataTable.Rows[cnt][Timeperiods.TimePeriod].ToString();

                    ResultPhraseArray = ResultPhrase.Split(' ');

                    ResultPhraseWordCount = ResultPhraseArray.Length;
                    // loop no. of times equal to keyword lenght
                    for (int i = 0; i <= SearchKeywordCount - 1; i++)
                    {
                        // loop no. of times equal to lenght of data
                        for (int j = 0; j <= ResultPhraseWordCount - 1; j++)
                            if (keywords[i].ToLower() == ResultPhraseArray[j].ToLower())
                            {
                                SearchKeywordOccurance += 1;
                            }
                    }

                    // Step 2 Get Criteria1Weightage
                    // Number of search keywords found in result phrase / total no. of search keyword * 100
                    Criteria1Weightage = (SearchKeywordOccurance / SearchKeywordCount) * 100;

                    // Step 3 Get Criteria2Weightage
                    // Number of search keywords found in result phrase / total no. of words in result phrase * 100
                    Criteria2Weightage = (SearchKeywordOccurance / ResultPhraseWordCount) * 100;

                    // Step 4 Set Weightage Column's value in datatable
                    SmsDataTable.Rows[cnt][WEIGHTAGE_COL1] = Criteria1Weightage;
                    SmsDataTable.Rows[cnt][WEIGHTAGE_COL2] = Criteria2Weightage;
                }
                SmsDataTable.AcceptChanges();

                // Step 5 Sort data on the basis of weightage and timeperiod
                RetVal = SmsDataTable.DefaultView;
                //-- 11-12-2009 Sort Order fixed 
                //RetVal.Sort = WEIGHTAGE_COL1 + " , " + WEIGHTAGE_COL2 + " Desc, " + Indicator.IndicatorName + " Desc, " + Timeperiods.TimePeriod + " Desc";
                RetVal.Sort = WEIGHTAGE_COL1 + " DESC, " + WEIGHTAGE_COL2 + " DESC, " + Indicator.IndicatorName + " DESC, " + Timeperiods.TimePeriod + " DESC";
            }
            catch (Exception ex)
            {

            }
            return RetVal;
        }

        /// <summary>
        /// Break search keyword at space and quotes to get absolute keyword.Which will to be used in database Query.
        /// </summary>
        /// <returns>String array containg each part of keyword as element </returns>
        /// <remarks>If SimpleSearch String Contains Space or Quotes then string will be splited in parts.Later each part will be used seperately for search in Database.</remarks>
        private string[] GetAbsoluteKeyword(string keyword)
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

        /// <summary>
        /// Check whether Even Number
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
        /// <summary>
        /// This will break search string and return a array where elements have Double Quote If Required
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private string[] GetAbsoluteKeywordToInitializeAdvSearch(string keyword)
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
                            //Add double Quote 
                            TempStorage.Add("\"" + SearchPattern[i] + "\"");
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
        /// Set Advance Search fields
        /// </summary>
        private void SetAdvanceSearchFields()
        {


            // Add fields to _AdvanceSearchFields collection in desired order
            //_AdvanceSearchFields.Add(new AdvanceSearchField(INDICATOR, DILanguage.GetLanguageString("INDICATOR"),true));
            //_AdvanceSearchFields.Add(new AdvanceSearchField(UNIT, DILanguage.GetLanguageString("UNIT"), true));
            // _AdvanceSearchFields.Add(new AdvanceSearchField(SUBGROUP, DILanguage.GetLanguageString("SUBGROUP"),true));
            //_AdvanceSearchFields.Add(new AdvanceSearchField(AREA_NAME, DILanguage.GetLanguageString("AREANAME"),true));
            //_AdvanceSearchFields.Add(new AdvanceSearchField(AREAID, DILanguage.GetLanguageString("AREAID"), true));
            //_AdvanceSearchFields.Add(new AdvanceSearchField(TIMEPERIOD, DILanguage.GetLanguageString("TIMEPERIOD"), true));
            // Language handlining of caption will be done by host application
            _AdvanceSearchFields.Add(new AdvanceSearchField(INDICATOR, "INDICATOR", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(UNIT, "UNIT", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(SUBGROUP, "SUBGROUP", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(AREA_NAME, "AREANAME", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(AREAID, "AREAID", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(TIMEPERIOD, "TIMEPERIOD", true));
            _AdvanceSearchFields.Add(new AdvanceSearchField(SOURCE, "SOURCE", true));

            // This Filed shall hold search text which does not fit into any of the above field type. e.g. "xyz"
            _AdvanceSearchFields.Add(new AdvanceSearchField(HIDDEN, "", true));

            //    // if language file do not exist
            //else
            //{
            //    // Add fields to _AdvanceSearchFields collection in desired order
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(INDICATOR, "Indicator", true));
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(UNIT, "Unit", true));
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(SUBGROUP, "Subgroup",  true));
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(AREA_NAME, "AreaName",  true));
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(AREAID, "AreaID",  true));
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(TIMEPERIOD, "TimePeriod", true));
            //    // This Filed shall hold search text which does not fit into any of the above field type. e.g. "xyz"
            //    _AdvanceSearchFields.Add(new AdvanceSearchField(HIDDEN, "",true));
            //}

        }

        private void GetIUSAreaTimeperiodNIdsBySource(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            string FilterClause = string.Empty;

            //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)
            // -- Adding each element of SearchString  into FilterClause
            for (int i = 0; i <= searchString.Length - 1; i++)
            {
                if (FilterClause.Length > 0)
                {
                    FilterClause += " OR ";
                }
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    FilterClause += IndicatorClassifications.ICName + " =" + DICommon.EscapeWildcardChar(searchString[i]);
                }
                else
                {
                    FilterClause += IndicatorClassifications.ICName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
            }
            try
            {
                //Running Query to get AreaNId's    
                if (FilterClause.Trim().Length > 0)
                {
                    sSql = "SELECT DISTINCT Data.IUSNId,Data.Area_NId,Data.TimePeriod_NId,IC.IC_Name FROM UT_Indicator_Classifications_en AS IC INNER JOIN UT_Data AS Data ON IC.IC_NId = Data.Source_NId where  IC.IC_Type='SR' and (" + FilterClause + ")";
                    DataTable TempSourcebasedTable = DIConnection.ExecuteDataTable(sSql);
                    // If these NIDs is not already in _ius or _area or _timeperiod list then add them                   
                    UpdateNIdsToList(TempSourcebasedTable);
                }

            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }

        }


        private void UpdateNIdsToList(DataTable datatable)
        {
            try
            {
                if (datatable.Rows.Count > 0)
                {
                    foreach (DataRow dr in datatable.Rows)
                    {
                        if (!this._Indicator_IUS_NId.Contains(dr[Indicator_Unit_Subgroup.IUSNId].ToString()))
                        {
                            if (this._Indicator_IUS_NId.Trim().Length > 0)
                            {
                                this._Indicator_IUS_NId = this._Indicator_IUS_NId + "," + dr[Indicator_Unit_Subgroup.IUSNId].ToString();
                            }
                            else
                            {
                                this._Indicator_IUS_NId = dr[Indicator_Unit_Subgroup.IUSNId].ToString();
                            }
                        }
                        if (!this._TimePeriodNId.Contains(dr[Timeperiods.TimePeriodNId].ToString()))
                        {
                            if (this._TimePeriodNId.Trim().Length > 0)
                            {
                                this._TimePeriodNId = this._TimePeriodNId + "," + dr[Timeperiods.TimePeriodNId].ToString();
                            }
                            else
                            {
                                this._TimePeriodNId = dr[Timeperiods.TimePeriodNId].ToString();
                            }
                        }
                        if (!this._AreaNId.Contains(dr[Area.AreaNId].ToString()))
                        {
                            if (this._AreaNId.Trim().Length > 0)
                            {
                                this._AreaNId = this._AreaNId + "," + dr[Area.AreaNId].ToString();
                            }
                            else
                            {
                                this._AreaNId = dr[Area.AreaNId].ToString();
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
        /// Removes Double Quotes From the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveDoubleQuotes(string value)
        {
            return (value.Replace("\"", " ")).Trim();
        }

        /// <summary>
        /// Simple search method using recursive approach
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="searchType"></param>
        private void SimpleRecursiveSearch(string keyword, SearchType searchType)
        {
            // Replace Double space to single space
            keyword = RemoveDoubleSpace(keyword);
            string TempIndicatorIUSNIds = String.Empty;
            string TempAreaNIds = String.Empty;
            string TempTimePeriodNIds = String.Empty;
            string[] keywords = null;
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

                // Break keyword typed into words and get Word count
                keywords = GetAbsoluteKeyword(keyword);
                iWordCount = keywords.Length;

                // STEP 1 & 2              
                //Get All Possible search combinations. Sort the Data Table by Word count Descending
                DataTable PossibleKeywordCombinationsDT = GetAllSearchCombinationsDataTable(keywords);

                // STEP 3: For each item in the Data table, perform the search
                foreach (DataRow DRow in PossibleKeywordCombinationsDT.Rows)
                {

                    // Perform search in I ,A and T table
                    string CurrentSearchString = DRow[SearchKeyword].ToString();
                    this.PerformSimpleSearch(searchType, out TempIndicatorIUSNIds, out TempAreaNIds, out TempTimePeriodNIds, CurrentSearchString);

                    // Chk For Sucessful Search
                    if (!string.IsNullOrEmpty(TempIndicatorIUSNIds) || !string.IsNullOrEmpty(TempAreaNIds) || !string.IsNullOrEmpty(TempTimePeriodNIds))
                    {
                        // STEP 3.1 If search succeeded and iWordCount=WordCount of this row 
                        if (Convert.ToInt32(DRow[WordCount]) == iWordCount)
                        {
                            this._Indicator_IUS_NId = CheckAndUpdateNewNIds(this._Indicator_IUS_NId, TempIndicatorIUSNIds);
                            this._AreaNId = CheckAndUpdateNewNIds(this._AreaNId, TempAreaNIds);
                            this._TimePeriodNId = CheckAndUpdateNewNIds(this._TimePeriodNId, TempTimePeriodNIds);
                            break;
                        }
                        else
                        {
                            //STEP 3.2 If search succeeded and iWordCount<>WordCount of this row 

                            //Update I A T NIds
                            this._Indicator_IUS_NId = CheckAndUpdateNewNIds(this._Indicator_IUS_NId, TempIndicatorIUSNIds);
                            this._AreaNId = CheckAndUpdateNewNIds(this._AreaNId, TempAreaNIds);
                            this._TimePeriodNId = CheckAndUpdateNewNIds(this._TimePeriodNId, TempTimePeriodNIds); ;

                            // STEP 3.3 : Trim current keyword from SearchString
                            string Tempkeyword = keyword.Replace(DRow[SearchKeyword].ToString().Trim(), "").Trim();

                            // Recursive Call
                            this.SimpleRecursiveSearch(Tempkeyword, searchType);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// Removes Quotes From the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RemoveDoubleSpace(string value)
        {
            string RetVal = string.Empty;
            RetVal = value.Replace("  ", " ");
            if (RetVal.Contains("  "))
            {
                RetVal = RemoveDoubleSpace(RetVal);
            }
            return RetVal;

        }

        /// <summary>
        /// Update Original string with values of New string If this is not found in original string
        /// </summary>
        /// <param name="OriginalString"></param>
        /// <param name="NewValue"></param>
        /// <returns></returns>
        public static string CheckAndUpdateNewNIds(String OriginalString, String NewValue)
        {
            string Retval = OriginalString;
            string[] OriginalValues = OriginalString.Split(',');
            string[] NewValues = DICommon.SplitString(NewValue, ",");
            for (int i = 0; i < NewValues.Length; i++)
            {
                bool UpdateValue = true;
                if (string.IsNullOrEmpty(Retval))
                {
                    Retval = NewValues[i].ToString();
                }
                else
                {
                    //------------------------New Code
                    for (int j = 0; j < OriginalValues.Length; j++)
                    {
                        if (OriginalValues[j].ToString() == NewValues[i].ToString())
                        {
                            UpdateValue = false;
                            break;
                        }
                    }
                    // If Update value
                    if (UpdateValue)
                    {
                        Retval += "," + NewValues[i].ToString();
                        // Update Original Values
                        OriginalValues = Retval.Split(',');
                    }
                    //----------------------
                    ////if (Retval.Contains( NewValues[i].ToString()))
                    ////{                        
                    ////}
                    ////else
                    ////{
                    ////    Retval += "," + NewValues[i].ToString();
                    ////}
                }
            }
            return Retval;
        }

        /// <summary>
        /// Perform Simple search using Breakup search string and update Temp Variables
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="TempIndicatorIUSNIds"></param>
        /// <param name="TempAreaNIds"></param>
        /// <param name="TempTimePeriodNIds"></param>
        /// <param name="searchString"></param>
        private void PerformSimpleSearch(SearchType searchType, out string TempIndicatorIUSNIds, out string TempAreaNIds, out string TempTimePeriodNIds, string searchkeyword)
        {
            TempIndicatorIUSNIds = string.Empty;
            TempAreaNIds = string.Empty;
            TempTimePeriodNIds = string.Empty;
            string[] searchString = new string[1];
            searchString[0] = searchkeyword.ToString();

            // Try to Get Search Result using MaxLenghtSearchString
            switch (searchType)
            {
                case SearchType.Indicator:
                    if (this._ShowIUS)
                    {
                        //Get IUSNIds  
                        TempIndicatorIUSNIds = GetIUSNIds(searchString, TempIndicatorIUSNIds);
                    }
                    else
                    {
                        TempIndicatorIUSNIds = GetIndicatorNIds(searchString);
                    }

                    // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                    TempAreaNIds = string.Empty;
                    TempTimePeriodNIds = string.Empty;
                    break;

                // If searchCreteria is  Area (SimpleSearch is from frmArea)
                case SearchType.Area:
                    TempAreaNIds = GetAreaNIdsByAreaName(searchString);
                    TempIndicatorIUSNIds = string.Empty;
                    TempTimePeriodNIds = string.Empty;
                    break;

                //--If searchCreteria is  All (SimpleSearch is from frmHome or frmData)
                case SearchType.Data:
                case SearchType.SMS:

                    if (this._ShowIUS)
                    {
                        TempIndicatorIUSNIds = GetIUSNIds(searchString, TempIndicatorIUSNIds);
                    }
                    //If RetrieveIndicator_IUS is set for  Indicator or search type is sms
                    else
                    {
                        //Get Indicator Nids
                        TempIndicatorIUSNIds = GetIndicatorNIds(searchString);
                    }

                    //Get TimePeriodNId and AreaNId
                    TempTimePeriodNIds = GetTimeNIds(searchString);
                    TempAreaNIds = GetAreaNIdsByAreaName(searchString);

                    // if Search string is in source
                    //GetIUSAreaTimeperiodNIdsBySource(keywords);

                    //Auto Select IUS Area Timeperiod
                    //AutoSelectIUSAreaTimeperiod();
                    //------------------------
                    // -- Autoselect Indicator / IUS
                    // this.AutoselectIUSAreaTimeperiod(ref TempIndicatorIUSNIds, ref TempAreaNIds, ref TempTimePeriodNIds);
                    //-----------------------
                    //////if (searchType == SearchType.SMS)
                    //////{
                    //////    SetSMSData(searchString, TempIndicatorIUSNIds, TempAreaNIds, TempTimePeriodNIds);
                    //////}
                    break;
                case SearchType.Unit:

                    this._Unit_NId = GetSearchUnitNIds(searchString);
                    // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                    this._AreaNId = "";
                    this._TimePeriodNId = "";
                    break;
                case SearchType.Subgroups:
                    this._SubgroupVal_NId = GetSubgroupValNIds(searchString);
                    // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                    this._AreaNId = "";
                    this._TimePeriodNId = "";
                    break;
            }
        }

        private void AutoselectIUSAreaTimeperiod(ref string TempIndicatorIUSNIds, ref string TempAreaNIds, ref string TempTimePeriodNIds)
        {
            if (TempIndicatorIUSNIds.Length == 0 && (TempTimePeriodNIds.Length > 0 || TempAreaNIds.Length > 0))
            {
                if (this._ShowIUS)
                {
                    TempIndicatorIUSNIds = AutoSelectIUSNIds(TempAreaNIds, TempTimePeriodNIds);
                }
                else
                {
                    TempIndicatorIUSNIds = AutoSelectIndicatorNIds(TempAreaNIds, TempTimePeriodNIds);
                }

            }
            // -- Autoselect Timeperiod 
            if (TempTimePeriodNIds.Length == 0 && (TempAreaNIds.Length > 0 || TempIndicatorIUSNIds.Length > 0))
            {
                TempTimePeriodNIds = AutoSelectTimeperiodNIds(TempAreaNIds, TempIndicatorIUSNIds);
            }

            // -- Autoselect Area 
            if (TempAreaNIds.Length == 0 && (TempIndicatorIUSNIds.Length > 0 || TempTimePeriodNIds.Length > 0))
            {
                TempAreaNIds = AutoSelectAreaNIds(TempIndicatorIUSNIds, TempTimePeriodNIds);
            }
        }

        /// <summary>
        ///  Get All Possible search combinations in a data table
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static DataTable GetAllSearchCombinationsDataTable(string[] keywords)
        {
            DataTable RetVal = new DataTable();
            // Add Word cout and SearchString columns
            RetVal.Columns.Add(WordCount);
            RetVal.Columns.Add(SearchKeyword);

            string PrevWord = string.Empty;

            int index = 0;
            for (int i = 0; i < keywords.Length; i++)
            {
                PrevWord = string.Empty;
                int wordCount = 0;
                for (int j = i; j < keywords.Length; j++)
                {
                    DataRow TempRow = RetVal.NewRow();
                    if (i == j)
                    {
                        TempRow[SearchKeyword] = keywords[j].ToString();
                    }
                    else
                    {
                        TempRow[SearchKeyword] = PrevWord + " " + keywords[j].ToString();
                    }
                    PrevWord = TempRow[SearchKeyword].ToString();
                    wordCount++;
                    TempRow[WordCount] = wordCount.ToString();
                    index++;
                    RetVal.Rows.Add(TempRow);
                }
            }
            // Sort Result Table
            if (RetVal != null && RetVal.Rows.Count > 0)
            {
                DataView Dv = RetVal.DefaultView;
                Dv.Sort = WordCount + " DESC";
                RetVal = Dv.ToTable();
            }
            return RetVal;
        }

        /// <summary>
        /// Get Maximum Lenght Search String
        /// </summary>
        /// <param name="PossibleKeywordCombinations"></param>
        private string GetMaxLenghtSearchString(string[,] PossibleKeywordCombinations)
        {
            string Retval = string.Empty;
            int MaxLenghSeachStringIndex = -1;
            int TempMaxlenghString = 0;
            for (int i = 0; i < PossibleKeywordCombinations.GetLength(0); i++)
            {
                if (Convert.ToInt32(PossibleKeywordCombinations[i, 0]) > TempMaxlenghString)
                {
                    TempMaxlenghString = Convert.ToInt32(PossibleKeywordCombinations[i, 0]);
                    MaxLenghSeachStringIndex = i;
                }
            }
            if (MaxLenghSeachStringIndex != -1)
            {
                Retval = PossibleKeywordCombinations[MaxLenghSeachStringIndex, 1].ToString();
            }
            return Retval;
        }

        #region "-- New FreeText Search --"

        private string GetSearchUnitNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            String AllUnitNIdsWithData = String.Empty;
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
                //FilterClause += Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //end of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }

            //Running Query to get IndicatorNId's    
            sSql = DIQueries.Unit.GetUnit(FilterFieldType.Search, FilterClause);
            try
            {
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Unit.UnitNId);
                rd.Close();
            }
            catch (Exception ex)
            {
                //rd.Close();
            }

            // If there are some indicator NIds 
            if (RetVal.Length > 0)
            {
                // Only those IndicatorNIds are required, those have data values against them.
                // RetrieveIndicator_IUS==Indicator_IUS_Option.IndicatorNId
                if (_GetIndicatorByDataOnly && !this._ShowIUS)
                {
                    // --Check if Data exist against these indicatorNIds
                    // Step1: Split NId string in an array
                    //string[] TempIndNIds = RetVal.Split(',');

                    //Step2:  Clear retval. Only those NIds will be returned those have data
                    ////RetVal = string.Empty;

                    // Step3. Get All indicatorNids those have data against them

                    // IndicatorNid is used to get ius NIds from IUS Table
                    // IUS is checked into data table 
                    //TODO: QUERY TO BE IN DAL
                    sSql = "SELECT DISTINCT U." + Unit.UnitNId + "  FROM " + this.DIQueries.TablesName.Data + " Data, " +
                    DIQueries.TablesName.Unit + " U ," + this.DIQueries.TablesName.IndicatorUnitSubgroup +
                    "  IUS WHERE Data." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId + " AND IUS." +
                    Indicator_Unit_Subgroup.UnitNId + " = U." + Unit.UnitNId
                    + " AND U." + Unit.UnitNId + " IN (" + RetVal + ")";

                    // Execute command
                    rd = DIConnection.ExecuteReader(sSql);

                    // Get all indicator Nids with data in a string
                    AllUnitNIdsWithData = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, Unit.UnitNId);
                    rd.Close();

                    // Update Retval With all IndicatorNid with data
                    RetVal = AllUnitNIdsWithData;
                }
            }

            return RetVal;
        }

        private string GetSubgroupValNIds(string[] searchString)
        {
            string RetVal = "";
            string sSql = "";
            IDataReader rd;
            String AllSgValNIdsWithData = String.Empty;
            string FilterClause = string.Empty;

            //-- Creating Filter Clause (FilterClause may have Multiple like Clauses)
            // -- Adding each element of SearchString  into FilterClause
            for (int i = 0; i <= searchString.Length - 1; i++)
            {
                if (FilterClause.Length > 0)
                {
                    FilterClause += " OR ";
                }
                // change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
                // Change on 14-07-08 .Like search instead of equal in case of phrase search.
                // When search text is written inside double quote.
                // Use search text after removing quotes from first and last position in like claues
                if (searchString[i].ToString().StartsWith("'") && searchString[i].ToString().EndsWith("'") && searchString[i].ToString() != "'")
                {
                    //FilterClause += Indicator.IndicatorName + " =" + DICommon.EscapeWildcardChar(searchString[i]) ;
                    FilterClause += SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i].Substring(1, searchString[i].Length - 2))) + "%'";
                }
                else
                {
                    FilterClause += SubgroupVals.SubgroupVal + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                }
                //FilterClause += Indicator.IndicatorName + " LIKE '%" + DICommon.EscapeWildcardChar(DICommon.RemoveQuotes(searchString[i])) + "%'";
                //end of change on 18-03-08.If searchString[i] is in quote use  equal to (=) in query instead of like
            }

            //Running Query to get IndicatorNId's    
            sSql = DIQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.Search, FilterClause);
            try
            {
                rd = DIConnection.ExecuteReader(sSql);
                RetVal = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, SubgroupVals.SubgroupValNId);
                rd.Close();
            }
            catch (Exception ex)
            {
                //rd.Close();
            }

            // If there are some indicator NIds 
            if (RetVal.Length > 0)
            {
                // Only those IndicatorNIds are required, those have data values against them.
                // RetrieveIndicator_IUS==Indicator_IUS_Option.IndicatorNId
                if (_GetIndicatorByDataOnly && !this._ShowIUS)
                {
                    // --Check if Data exist against these indicatorNIds
                    // Step1: Split NId string in an array
                    // Step2:  Clear retval. Only those NIds will be returned those have data
                    // Step3. Get All indicatorNids those have data against them
                    // IndicatorNid is used to get ius NIds from IUS Table
                    // IUS is checked into data table 
                    sSql = "SELECT DISTINCT S." + SubgroupVals.SubgroupValNId + "  FROM " + this.DIQueries.TablesName.Data + " Data, " +
                    DIQueries.TablesName.SubgroupVals + " S ," + this.DIQueries.TablesName.IndicatorUnitSubgroup +
                    "  IUS WHERE Data." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId + " AND IUS." +
                    Indicator_Unit_Subgroup.UnitNId + " = S." + SubgroupVals.SubgroupValNId
                    + " AND S." + SubgroupVals.SubgroupValNId + " IN (" + RetVal + ")";

                    // Execute command
                    rd = DIConnection.ExecuteReader(sSql);

                    // Get all indicator Nids with data in a string
                    AllSgValNIdsWithData = DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromReader(rd, SubgroupVals.SubgroupValNId);
                    rd.Close();

                    // Update Retval With all IndicatorNid with data
                    RetVal = AllSgValNIdsWithData;
                }
            }

            return RetVal;
        }

        #endregion
        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Constructor--"

        /// <summary>
        /// Initializes a new instance of the Freetext class with the DIConnection and DIQueries instances.
        /// </summary>
        /// <remarks>
        /// Use this constructor when hosting application is itself using an instance of DIConnection and DIQueries
        /// Hosting application do not need to additionally set DatabasePrefix and LanguageCode properties
        /// </remarks>
        public FreeText(DIConnection DIConnection, DIQueries DIQueries)
        {
            this.DIConnection = DIConnection;
            this.DIQueries = DIQueries;
            this._DatabasePrefix = DIQueries.DataPrefix;
            this._LanguageCode = DIQueries.LanguageCode;
            //add  Advance search fields (Indicator,Unit,Subgroup,AreaName,AreaId and Timeperiod)to collection (_AdvanceSearchFields)             
            SetAdvanceSearchFields();

        }

        /// <summary>
        /// Initializes a new instance of the Freetext class with the specified database connection details.
        /// </summary>
        /// <param name="serverType"></param>
        /// <param name="serverName"></param>
        /// <param name="portNo"></param>
        /// <param name="databaseName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="languageFilePath"></param>
        /// <remarks>
        /// Use this constructor when hosting application is itself not using an instance of DAL
        /// Hosting application needs to additionally set DatabasePrefix and LanguageCode properties
        /// </remarks>
        public FreeText(DIServerType serverType, string serverName, string portNo, string databaseName, string userName, string password)
        {

            try
            {
                DIConnection = new DIConnection(serverType, serverName, portNo, databaseName, userName, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Get Default DataSet Prefix for database.
            this._DatabasePrefix = DIConnection.DIDataSetDefault();

            // Get Default Language Code for Default DataSet.
            this._LanguageCode = DIConnection.DILanguageCodeDefault(_DatabasePrefix);

            DIQueries = new DIQueries(_DatabasePrefix, _LanguageCode);
            //add  Advance search fields (Indicator,Unit,Subgroup,AreaName,AreaId and Timeperiod)to collection (_AdvanceSearchFields)
            SetAdvanceSearchFields();
        }
        #endregion

        #region "-- Properties --"

        private string _DatabasePrefix = string.Empty;
        /// <summary>
        /// Gets or sets dataset prefix used to identefy a specific dataset among multiple datasets residing in a single database. e.g. "UT_" "RT_" 
        /// </summary>
        public string DatabasePrefix
        {
            get
            {
                return _DatabasePrefix;
            }
            set
            {
                _DatabasePrefix = value;
                //Replace existing instance of DIQueries with a new instance of DIQueries based on new DatabasePrefix
                this.DIQueries = new DIQueries(_DatabasePrefix, _LanguageCode);
            }

        }

        private string _LanguageCode = string.Empty;
        /// <summary>
        /// Gets or sets Language suffix for language tables. e.g. "_en" "_fr"
        /// </summary>
        public string LanguageCode
        {
            get
            {
                return _LanguageCode;
            }
            set
            {
                _LanguageCode = value;
                //Replace existing instance of DIQueries with a new instance of DIQueries based on new LanguageCode
                this.DIQueries = new DIQueries(_DatabasePrefix, _LanguageCode);
            }
        }

        //private string _LanguageFile = string.Empty;
        ///// <summary>
        ///// Gets or sets Language File Path 
        ///// </summary>
        //public string LanguageFile
        //{
        //    get
        //    {
        //        return _LanguageFile;
        //    }
        //    set
        //    {
        //     _LanguageFile = value;
        //     DILanguage.Open(_LanguageFile);

        //     //Reset caption of Advance Search Fields
        //     foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
        //    {
        //       switch (AdvanceSearchField.Name.ToLower())
        //            {
        //                case  INDICATOR:
        //                   AdvanceSearchField.Caption = DILanguage.GetLanguageString("INDICATOR");
        //                break;

        //                case UNIT:
        //                AdvanceSearchField.Caption = DILanguage.GetLanguageString("UNIT");
        //                break;

        //                 case SUBGROUP:
        //                AdvanceSearchField.Caption = DILanguage.GetLanguageString("SUBGROUP");
        //                break;

        //                case AREA_NAME:
        //                AdvanceSearchField.Caption = DILanguage.GetLanguageString("AREANAME");
        //                break;

        //                 case AREAID:
        //                AdvanceSearchField.Caption = DILanguage.GetLanguageString("AREAID");
        //                break;

        //                 case TIMEPERIOD:
        //                AdvanceSearchField.Caption = DILanguage.GetLanguageString("TIMEPERIOD");
        //                break;

        //                default:
        //                    break;                        
        //            }
        //   }               

        //    }
        //}

        private string _SearchText = string.Empty;
        /// <summary>
        /// Get the last search text
        /// </summary>
        /// <remarks>
        /// In case of simple search it will be same as search  string set by user.
        /// In case of advance search it will be concatinated text formed by search string of all AdvancedSearchFields and delimited by space 
        /// </remarks>
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; }
        }


        private bool _ShowIUS = true;
        /// <summary>
        /// This will determine whethere Indicator or IUS  Nid will be retrieved
        /// </summary>
        /// <remarks>earlier this property was exposed as type Indicator_IUS_Option</remarks>
        public bool ShowIUS
        {
            get
            {
                return this._ShowIUS;
            }
            set
            {
                this._ShowIUS = value;
            }
        }

        private string _Indicator_IUS_NId = string.Empty;
        /// <summary>
        /// Get IndicatorNIds/IUSNIds based on search text
        /// </summary>
        public string Indicator_IUS_NId
        {
            get
            { return _Indicator_IUS_NId; }
        }

        //private string _IUSNId = string.Empty;
        ///// <summary>
        ///// Get IUSNIds based on search text
        ///// </summary>
        //public string IUSNId
        //{
        //    get
        //    {   return _IUSNId;  }
        //}

        private string _TimePeriodNId = string.Empty;
        /// <summary>
        /// Get TimeNIds based on search text
        /// </summary>
        public string TimePeriodNId
        {
            get
            { return _TimePeriodNId; }
        }

        private string _AreaNId = string.Empty;
        /// <summary>
        /// Get AreaNIds based on search text
        /// </summary>
        public string AreaNId
        {
            get
            { return _AreaNId; }
        }


        private string _SourceNId = string.Empty;
        /// <summary>
        /// Get SourceNId based on search text
        /// </summary>
        public string SourceNId
        {
            get
            {
                return this._SourceNId;
            }
        }

        private DataTable _SMSData;

        public DataTable SMSData
        {
            get { return _SMSData; }

        }

        private string _Unit_NId = string.Empty;
        /// <summary>
        /// Get UnitNId based on search text
        /// </summary>
        public string Unit_NId
        {
            get
            { return this._Unit_NId; }
        }

        private string _SubgroupVal_NId = string.Empty;
        /// <summary>
        /// Get SubgroupVal_NId based on search text
        /// </summary>
        public string SubgroupVal_NId
        {
            get
            { return this._SubgroupVal_NId; }
        }


        private Int32 _SMSPageSize = 10;
        /// <summary>
        /// For setting the No of Records in dataset .
        /// </summary>
        /// <remarks>set in case of SMS application only</remarks>
        public Int32 SMSPageSize
        {
            get { return _SMSPageSize; }
            set { _SMSPageSize = (Int32)value; }
        }

        private List<AdvanceSearchField> _AdvanceSearchFields = new List<AdvanceSearchField>();
        /// <summary>
        /// Gets or sets collection of Advance search fields
        /// </summary>
        public List<AdvanceSearchField> AdvanceSearchFields
        {
            get
            { return _AdvanceSearchFields; }
        }

        private Boolean _GetIndicatorByDataOnly = false;
        /// <summary>
        /// Is indicator with data will be selected 
        /// </summary>
        public Boolean GetIndicatorByDataOnly
        {
            get { return _GetIndicatorByDataOnly; }
            set { _GetIndicatorByDataOnly = value; }
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Main function for initiating simple search. 
        ///</summary>
        /// <param name="keyword">Space delimited search text</param>
        /// <param name="searchType">This determine where to search(Indicater,area or Data)</param> 
        public void SimpleSearch(string keyword, SearchType searchType)
        {
            this._SearchText = keyword;         //Store search string                      
            string[] keywords;
            try
            {
                // If search string Contrain double quote earlier logic will applicable
                if (keyword.Contains('"'.ToString()))
                {
                    // -- Get Absolute keyWord to SimpleSearch (Without Spaces and Quotes)
                    keywords = GetAbsoluteKeyword(keyword);

                    // Process if Keywords was found
                    if (keywords.Length >= 1)
                    {
                        //-- Set Idicator/Ius, Area Nid and Timeperiod Nid to blank  before starting search
                        this._Indicator_IUS_NId = "";
                        this._AreaNId = "";
                        this._TimePeriodNId = "";

                        // If searchCreteria is  Indicator (SimpleSearch is from frmIndicator)
                        switch (searchType)
                        {
                            case SearchType.Indicator:
                                if (this._ShowIUS)
                                {
                                    //Get IUSNIds  
                                    this._Indicator_IUS_NId = GetIUSNIds(keywords, this._Indicator_IUS_NId);
                                }
                                else
                                {
                                    this._Indicator_IUS_NId = GetIndicatorNIds(keywords);
                                }

                                // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                                this._AreaNId = "";
                                this._TimePeriodNId = "";
                                break;

                            // If searchCreteria is  Area (SimpleSearch is from frmArea)
                            case SearchType.Area:
                                this._AreaNId = GetAreaNIdsByAreaName(keywords);
                                this._Indicator_IUS_NId = "";
                                this._TimePeriodNId = "";
                                break;

                            //--If searchCreteria is  All (SimpleSearch is from frmHome or frmData)
                            case SearchType.Data:
                            case SearchType.SMS:

                                if (this._ShowIUS)
                                {
                                    this._Indicator_IUS_NId = GetIUSNIds(keywords, this._Indicator_IUS_NId);
                                }
                                //If RetrieveIndicator_IUS is set for  Indicator or search type is sms
                                else
                                {
                                    //Get Indicator Nids
                                    this._Indicator_IUS_NId = GetIndicatorNIds(keywords);
                                }

                                //Get TimePeriodNId and AreaNId
                                this._TimePeriodNId = GetTimeNIds(keywords);
                                this._AreaNId = GetAreaNIdsByAreaName(keywords);

                                // if Search string is in source
                                //GetIUSAreaTimeperiodNIdsBySource(keywords);

                                //Auto Select IUS Area Timeperiod
                                AutoSelectIUSAreaTimeperiod();


                                if (searchType == SearchType.SMS)
                                {
                                    SetSMSData(keywords, this._Indicator_IUS_NId, this._AreaNId, this._TimePeriodNId);
                                }
                                break;
                            case SearchType.Unit:

                                this._Unit_NId = GetSearchUnitNIds(keywords);
                                // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                                this._AreaNId = "";
                                this._TimePeriodNId = "";
                                break;
                            case SearchType.Subgroups:
                                this._SubgroupVal_NId = GetSubgroupValNIds(keywords);
                                // Set AreaNid and TimePeriodNId to blank if searchType is indicator
                                this._AreaNId = "";
                                this._TimePeriodNId = "";
                                break;
                        }
                    }
                }
                // If no quote found in search string Use Recursive search for better result
                else
                {
                    this._Indicator_IUS_NId = string.Empty;
                    this._AreaNId = string.Empty;
                    this._TimePeriodNId = string.Empty;

                    // Call Recrsive search
                    SimpleRecursiveSearch(keyword, searchType);
                    //Perform  Autoselect()
                    if ((searchType == SearchType.Data) || (searchType == SearchType.SMS))
                    {
                        this.AutoSelectIUSAreaTimeperiod();
                        if (searchType == SearchType.SMS)
                        {
                            //Set Sms data using Ranking 
                            SetSMSData(this.GetAbsoluteKeyword(keyword), this._Indicator_IUS_NId, this._AreaNId, this._TimePeriodNId);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void SetSMSData(string[] keywords, string indicatorIUSNId, string areaNId, string timeperiodNId)
        {
            DataTable SmsDataTable;         //Store Datatable returned as result of DAL query
            DataView SortedData = new DataView();

            // Data will be retrieved if any of three (_Indicator_IUSNId,_TimePeriodNId,_AreaNId) has value
            if (indicatorIUSNId.Length > 0 || timeperiodNId.Length > 0 || areaNId.Length > 0)
            {
                // Retrieve data set through DAL  
                string sSql = DIQueries.Data.GetDataByIUSTimePeriodAreaSource(indicatorIUSNId, timeperiodNId, areaNId, "", FieldSelection.Name);
                SmsDataTable = DIConnection.ExecuteDataTable(sSql);


                // Add Weightage column
                SmsDataTable.Columns.Add(WEIGHTAGE_COL1);
                SmsDataTable.Columns.Add(WEIGHTAGE_COL2);
                //---Get Sized DataTable with Same Structure as smsDataTable
                DataTable SizedDataTable = new DataTable();
                SizedDataTable = SmsDataTable.Clone();

                // Apply ranking logic            
                SortedData = RankSmsResult(keywords, SmsDataTable);

                DataTable SortedTable = new DataTable();
                SortedTable = SortedData.ToTable();
                // Remove records in excess to SMSPageSize      

                for (int i = 0; i <= System.Math.Min(SMSPageSize, SortedData.Count) - 1; i++)
                {

                    DataRow dr;
                    dr = SizedDataTable.NewRow();
                    for (int cnt = 0; cnt < SortedTable.Columns.Count - 1; cnt++)
                    {
                        dr[cnt] = SortedTable.Rows[i][cnt];
                    }
                    SizedDataTable.Rows.Add(dr);
                }


                // Removing weightage column's from dataview

                SizedDataTable.Columns.Remove(WEIGHTAGE_COL1);
                SizedDataTable.Columns.Remove(WEIGHTAGE_COL2);

                // Add Data_NId column of type autonumber
                SizedDataTable.Columns.Add("Data_NId", typeof(System.Int32));
                for (int j = 1; j <= SizedDataTable.Rows.Count; j++)
                {

                    SizedDataTable.Rows[j - 1]["Data_NId"] = j;
                }
                SizedDataTable.AcceptChanges();

                // Set SMSData property 
                _SMSData = SizedDataTable;
            }
            else
            {
                // if _IUSNId and _TimePeriodNId and _AreaNId are "" then set blank dataview
                _SMSData = new DataTable();
            }
        }

        /// <summary>
        /// Initialize advance search
        /// </summary>
        /// <param name="Keyword"> Search String</param>
        public void InitializeAdvanceSearch(string keyword, SearchType searchType)
        {
            string[] keywords;
            this._SearchText = keyword;
            string indicator = "";
            string unit = "";
            string subgroup = "";
            string areaName = "";
            string areaId = "";
            string timePeriod = "";
            string hidden = "";
            // boolean Flag to identify that whether any search text doesnt fit in any of the advance search field
            bool IsHidden = true;

            //Get Absolute Keyword after Breaking keyword on spaces
            //-- Start of Change C1: Adv Search Removes Double Quote
            //keywords = GetAbsoluteKeyword(keyword); //this statement is changed to keywords = GetAbsoluteKeywordToInitializeAdvSearch(keyword); on 21 sept 2007
            keywords = GetAbsoluteKeywordToInitializeAdvSearch(keyword);
            //-- End of Change C1: Adv Search Removes Double Quote
            //Check whether keyword is Indicator or area or Timperiod           
            for (Int32 Counter = 0; Counter < keywords.Length; Counter++)
            {
                IsHidden = true;
                //Check for Indicator
                // If keyword was found in indicator Table
                if (IsIndicator(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (indicator.Length == 0)
                    { indicator = keywords[Counter]; }
                    else
                    { indicator = indicator + " " + keywords[Counter]; }
                }

                // -- Check for Unit
                // If keyword was found in Unit Table
                if (IsUnit(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (unit.Length == 0)
                    { unit = keywords[Counter]; }
                    else
                    { unit = unit + " " + keywords[Counter]; }
                }
                // -- Check for subgroup
                // If keyword was found in subgroup Val Table
                if (IsSubgroup(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (unit.Length == 0)
                    { subgroup = keywords[Counter]; }
                    else
                    { subgroup = subgroup + " " + keywords[Counter]; }
                }

                //Check for AreaName
                if (IsArea(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (areaName.Length == 0)
                    { areaName = keywords[Counter]; }
                    else
                    { areaName = areaName + " " + keywords[Counter]; }
                }
                //Check for AreaId            
                if (IsAreaId(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (areaId.Length == 0)
                    { areaId = keywords[Counter]; }
                    else
                    { areaId = areaId + " " + keywords[Counter]; }
                }

                //Check For TimePeriod
                if (IsTimePeriod(RemoveDoubleQuotes(keywords[Counter])))
                {
                    IsHidden = false;
                    if (timePeriod.Length == 0)
                    { timePeriod = keywords[Counter]; }
                    else
                    { timePeriod = timePeriod + " " + keywords[Counter]; }
                }

                //If the keyword does lies in any of the above criteria then set it for hidden field
                if (IsHidden == true)
                {
                    //Is Text do not  already exist in hidden field
                    if (hidden.Contains(keywords[Counter]) == false)
                    {
                        hidden = hidden + " " + keywords[Counter];
                    }
                }

            }

            //Set the Search text of AdvanceSearchFields
            foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
            {
                switch (AdvanceSearchField.Name.ToLower())
                {
                    case INDICATOR:

                        //-- Indicator will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;

                        }
                        else
                        {
                            AdvanceSearchField.Enabled = true;

                        }

                        if (indicator.Length > 0)
                        {
                            AdvanceSearchField.SearchText = indicator;
                        }
                        break;

                    case UNIT:
                        //-- unit will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (unit.Length > 0)
                        {
                            AdvanceSearchField.SearchText = unit;
                        }
                        break;

                    case SUBGROUP:

                        //-- subgroup will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (subgroup.Length > 0)
                        {
                            AdvanceSearchField.SearchText = subgroup;
                        }
                        break;

                    case AREA_NAME:
                        //--  AreaName will be disabled for advance search from Indicator Page
                        if (searchType == SearchType.Indicator)
                        {

                            AdvanceSearchField.Enabled = false;
                        }

                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }
                        if (areaName.Length > 0)
                        {
                            AdvanceSearchField.SearchText = areaName;
                        }
                        break;

                    case AREAID:

                        //--  AreaId will be disabled for advance search from Indicator Page
                        if (searchType == SearchType.Indicator)
                        {

                            AdvanceSearchField.Enabled = false;

                        }

                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (areaId.Length > 0)
                        {
                            AdvanceSearchField.SearchText = areaId;
                        }
                        break;

                    case TIMEPERIOD:

                        //-- Timeperiod will be disabled for advance search from Indicator or Area  Page
                        if ((searchType == SearchType.Indicator) || (searchType == SearchType.Area))
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (timePeriod.Length > 0)
                        {
                            AdvanceSearchField.SearchText = timePeriod;
                        }
                        break;
                    case HIDDEN:
                        if (hidden.Length > 0)
                        {
                            AdvanceSearchField.SearchText = hidden;
                        }
                        break;
                    default:
                        break;
                }
            }

        }


        /// <summary>
        /// Initialize fields for Advance search
        /// </summary>
        /// <param name="Keyword"> Search String</param>
        public void InitializeAdvanceSearchFields(Dictionary<string, string> keyword, SearchType searchType)
        {
            string Indicator = "";
            string Unit = "";
            string Subgroup = "";
            string AreaName = "";
            string AreaId = "";
            string TimePeriod = "";
            string Source = "";
            string hidden = "";


            foreach (String key in keyword.Keys)
            {
                switch (key.ToLower())
                {
                    case INDICATOR:
                        Indicator = keyword[key];
                        break;
                    case UNIT:
                        Unit = keyword[key];
                        break;
                    case SUBGROUP:
                        Subgroup = keyword[key];
                        break;
                    case AREA_NAME:
                        AreaName = keyword[key];
                        break;
                    case AREAID:
                        AreaId = keyword[key];
                        break;
                    case TIMEPERIOD:
                        TimePeriod = keyword[key];
                        break;
                    case SOURCE:
                        Source = keyword[key];
                        break;
                    default:
                        break;
                }
            }


            //Set the Search text of AdvanceSearchFields
            foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
            {
                switch (AdvanceSearchField.Name.ToLower())
                {
                    case INDICATOR:

                        //-- Indicator will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;

                        }
                        else
                        {
                            AdvanceSearchField.Enabled = true;

                        }

                        if (Indicator.Length > 0)
                        {
                            AdvanceSearchField.SearchText = Indicator;
                        }
                        break;

                    case UNIT:
                        //-- unit will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (Unit.Length > 0)
                        {
                            AdvanceSearchField.SearchText = Unit;
                        }
                        break;

                    case SUBGROUP:

                        //-- subgroup will be disabled for advance search from Area Page
                        if (searchType == SearchType.Area)
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (Subgroup.Length > 0)
                        {
                            AdvanceSearchField.SearchText = Subgroup;
                        }
                        break;

                    case AREA_NAME:
                        //--  AreaName will be disabled for advance search from Indicator Page
                        if (searchType == SearchType.Indicator)
                        {

                            AdvanceSearchField.Enabled = false;
                        }

                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }
                        if (AreaName.Length > 0)
                        {
                            AdvanceSearchField.SearchText = AreaName;
                        }
                        break;

                    case AREAID:

                        //--  AreaId will be disabled for advance search from Indicator Page
                        if (searchType == SearchType.Indicator)
                        {

                            AdvanceSearchField.Enabled = false;

                        }

                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (AreaId.Length > 0)
                        {
                            AdvanceSearchField.SearchText = AreaId;
                        }
                        break;

                    case TIMEPERIOD:

                        //-- Timeperiod will be disabled for advance search from Indicator or Area  Page
                        if ((searchType == SearchType.Indicator) || (searchType == SearchType.Area))
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (TimePeriod.Length > 0)
                        {
                            AdvanceSearchField.SearchText = TimePeriod;
                        }
                        break;

                    case SOURCE:

                        //-- Timeperiod will be disabled for advance search from Indicator or Area  Page
                        if ((searchType == SearchType.Indicator) || (searchType == SearchType.Area))
                        {

                            AdvanceSearchField.Enabled = false;
                        }
                        else
                        {

                            AdvanceSearchField.Enabled = true;
                        }

                        if (Source.Length > 0)
                        {
                            AdvanceSearchField.SearchText = Source;
                        }
                        break;
                    case HIDDEN:
                        if (hidden.Length > 0)
                        {
                            AdvanceSearchField.SearchText = hidden;
                        }
                        break;
                    default:
                        break;
                }
            }

        }


        /// <summary>
        /// Function for initiating Advance search. 
        /// </summary>
        /// <param name="searchType">This determine where to search(Indicater,area or Data)</param>
        /// <remarks>Hosting application should update AdvanceSearchFields collection before calling this function</remarks>
        public void AdvanceSearch(SearchType searchType)
        {
            string[] keywords;
            try
            {
                // set searchText from AdvanceSearchFileds collection
                // _SearchText = GetSearchText();
                switch (searchType)
                {
                    case SearchType.Indicator:
                        this._Indicator_IUS_NId = "";
                        this._TimePeriodNId = "";
                        this._AreaNId = "";

                        // if Indicator_IUS_Option is set for IUSNId
                        if (this.ShowIUS)
                        {
                            // Get IUSNId by Setting functions argument GetIUSNid to True
                            this._Indicator_IUS_NId = GetIndicator_IUSNIdFromAdvanceSearchFileds(true);

                        }
                        // if Indicator_IUS_Option is set for IndicatorNId
                        else
                        {
                            // Get IndicatorNId by Setting functions Parameter GetIUSNid to False
                            this._Indicator_IUS_NId = GetIndicator_IUSNIdFromAdvanceSearchFileds(false); //GetIndicatorNIdFromAdvanceSearchFileds();
                        }

                        // Set AreaNId and TimeperiodNId to Blank as search is  for indicator
                        this._AreaNId = "";
                        this._TimePeriodNId = "";
                        break;

                    case SearchType.Area:
                        this._Indicator_IUS_NId = "";
                        this._TimePeriodNId = "";
                        this._AreaNId = "";
                        // Get AreaId
                        this._AreaNId = GetAreaNIdFromAdvanceSearchFileds();

                        // Set _Indicator_IUS_NId and TimeperiodNId to Blank as search is  for Area
                        this._Indicator_IUS_NId = "";
                        this._TimePeriodNId = "";
                        break;
                    case SearchType.Data:
                        // Include Search in Adv search only when something is typed in source search text box
                        bool IncludeSourceInAdvSearch = false;
                        this._Indicator_IUS_NId = "";
                        this._TimePeriodNId = "";
                        this._AreaNId = "";
                        this._SourceNId = string.Empty;

                        // if Indicator_IUS_Option is set for IUSNId
                        if (this._ShowIUS)
                        {
                            // Get IUSNId by Setting functions argument GetIUSNid to True
                            this._Indicator_IUS_NId = GetIndicator_IUSNIdFromAdvanceSearchFileds(true);
                        }
                        else
                        {
                            this._Indicator_IUS_NId = GetIndicator_IUSNIdFromAdvanceSearchFileds(false);
                        }


                        //Get TimePeriodId
                        foreach (AdvanceSearchField AdvanceSearchField in _AdvanceSearchFields)
                        {
                            if ((AdvanceSearchField.Name == TIMEPERIOD) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                            {
                                //-- In case user has typed some text for Timeperiod, then supress autoselect for timeperiod
                                this.AutoSelectTimePeriodForAdvSearch = false;

                                keywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);

                                // --get TimePeriodNIds
                                this._TimePeriodNId = GetTimeNIds(keywords);

                            }

                            // change on 16-03-09. Adding Handling for search using  source 
                            if ((AdvanceSearchField.Name == SOURCE) && (((AdvanceSearchField.SearchText).Trim()).Length > 0))
                            {
                                //Include Source in Search process
                                IncludeSourceInAdvSearch = true;
                                keywords = GetAbsoluteKeyword(AdvanceSearchField.SearchText);

                                // --get SourceNId
                                this._SourceNId = this.GetSourceNIds(keywords);

                            }

                        }

                        // Get AreaId
                        this._AreaNId = GetAreaNIdFromAdvanceSearchFileds();

                        //Check that IUSNId, AreaNId and TimePeriodNIds are set else set them using autoselect clause
                        // Autoselect IUS,Area or Timeperiod when source is not included ,as per earlier logic
                        if (!IncludeSourceInAdvSearch)
                        {
                            AutoSelectAdvanceSearchIUSAreaTimeperiod();
                        }
                        else
                        {
                            //If Source text box is not blank but source text fail to get any result do not autoselect IUS,Area or Timeperiod 
                            //Case: abc is typed in source text box , which do not bring any source nid after search
                            if (!string.IsNullOrEmpty(this._SourceNId))
                            {
                                AutoSelectAdvanceSearchIUSAreaTimeperiod();
                            }
                        }
                        //AutoSelectAdvanceSearchIUSAreaTimeperiod();
                        break;

                    case SearchType.SMS:
                        // SMS will work on simple search only
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        #region "-- Enum--"
        /// <summary>
        /// Store Different Type of SimpleSearch Options Available
        /// </summary>
        public enum SearchType
        {
            /// <summary>
            ///  No search will be performed
            /// </summary>
            None = 0,

            /// <summary>
            /// SimpleSearch will be only for IndicatorNId's
            /// </summary>
            Indicator = 1,

            /// <summary>
            /// SimpleSearch will be only for AreaNId's
            /// </summary>
            Area = 2,

            /// <summary>
            /// SimpleSearch will be for IndicatorNId's, AreaNId's and TimePeriodNId's 
            /// </summary>
            /// <remarks>Used from Home page and Dataview page</remarks>
            Data = 3,

            /// <summary>
            ///SimpleSearch For SMS Application and Google SimpleSearch (Online Gallery)
            /// </summary>
            /// <remarks>In this case instead of setting NIds a DataSet with ranking shall be generated</remarks>
            SMS = 4,

            /// <summary>
            /// SimpleSearch will be only for UNITNId's
            /// </summary>
            Unit = 5,

            /// <summary>
            /// SimpleSearch will be only for SubgroupValNId's
            /// </summary>
            Subgroups = 6
        }

        /// <summary>
        /// Store What to return  after Search (IndiacatorNid or IUSNid)
        /// </summary>
        public enum Indicator_IUS_Option
        {
            /// <summary>
            /// IndicatorNId  will Returned
            /// </summary>
            IndicatorNId = 0,
            /// <summary>
            ///  IUSNId  will Returned
            /// </summary>
            IUSNId = 1
        }
    }
        #endregion

        #endregion

    /// <summary>
    /// Class for holding Advance search field information
    /// </summary>
    public class AdvanceSearchField
    {
        #region " --Public --"

        #region " --Properties --"

        private string _Name = string.Empty;
        /// <summary>
        /// Gets or sets name of the advance search field
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _Caption = string.Empty;
        /// <summary>
        /// Gets or sets the language based caption of the advance search field
        /// </summary>
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }


        private string _SearchText = string.Empty;
        /// <summary>
        /// Gets or sets the search text of the advance search field
        /// </summary>
        public string SearchText
        {
            get { return _SearchText; }
            set { _SearchText = value; }
        }

        private Boolean _Enabled = false;
        /// <summary>
        ///  Gets or sets Enable status of the advance search field
        /// </summary>
        public Boolean Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; }
        }

        #endregion

        #region "-- Constructor --"

        /// <summary>
        /// Set Name, Caption and selection status for search field 
        /// </summary>
        /// <param name="name">Name of the advance search field</param>
        /// <param name="caption">Caption of the advance search field</param>
        /// <param name="enabled">Enable/Disable status of the advance search field</param>
        public AdvanceSearchField(string name, String caption, Boolean enabled)
        {
            this._Name = name;
            this._Caption = caption;
            this._Enabled = enabled;
        }
        # endregion

        #endregion
    }

}

