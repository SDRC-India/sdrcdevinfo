using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data.Common;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.QDS.DI7CacheNSP
{
    public static class DI7OfflineSPHelper
    {
        #region "-- Public --"

        #region "-- Methods --"
        
        #region "-- QDS Results --"

        /// <summary>
        /// Get default indicators
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static DataTable GetDefaultIndicators(DIConnection dbConnection, DITables tableNames)
        {
            //-- SP_GET_DEFAULT_INDICATORS_XX

            DataTable RetVal = null;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;

            try
            {
                SBQry.Append("SELECT IUS."+ Indicator_Unit_Subgroup.IUSNId +", IUS."+ Indicator_Unit_Subgroup.IndicatorNId +", IUS."+ Indicator_Unit_Subgroup.UnitNId +", I."+ Indicator.IndicatorName +", U." + Unit.UnitName);
                SBQry.Append(" FROM ("+ tableNames.IndicatorUnitSubgroup +" IUS");
                SBQry.Append(" INNER JOIN "+ tableNames.Indicator +" I ON IUS."+ Indicator_Unit_Subgroup.IndicatorNId +" = I."+ Indicator.IndicatorNId +") ");
                SBQry.Append(" INNER JOIN "+ tableNames.Unit +" U ON IUS."+ Indicator_Unit_Subgroup.UnitNId +" = U." + Unit.UnitNId);
                SBQry.Append(" WHERE IUS."+ Indicator_Unit_Subgroup.IUSNId +" IN");
                SBQry.Append(" (");
                SBQry.Append(" SELECT DISTINCT "+ IndicatorClassificationsIUS.IUSNId +" FROM " + tableNames.IndicatorClassificationsIUS);
                SBQry.Append(" WHERE "+ IndicatorClassificationsIUS.ICNId +" IN");
                SBQry.Append(" (");
                SBQry.Append(" SELECT TOP 1 "+ IndicatorClassifications.ICNId +" FROM " + tableNames.IndicatorClassifications);
                SBQry.Append(" WHERE "+ IndicatorClassifications.ICParent_NId +" = -1 AND "+ IndicatorClassifications.ICOrder +" IS NOT NULL");
                SBQry.Append(" ORDER BY " + IndicatorClassifications.ICOrder + " ASC");
                SBQry.Append(")");
                SBQry.Append(")");
                StrQry = SBQry.ToString();                
                RetVal = dbConnection.ExecuteDataTable(StrQry);                
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// Get default areas
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static DataTable GetDefaultAreas(DIConnection dbConnection, DITables tableNames)
        {
            //-- SP_GET_DEFAULT_AREAS_XX

            DataTable RetVal = null;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;

            try
            {
                SBQry.Append("SELECT "+ Area.AreaNId +", "+ Area.AreaID +", "+ Area.AreaName +",");
                SBQry.Append(" (SELECT COUNT(" + Area.AreaNId + ") FROM "+ tableNames.Area +" WHERE "+ Area.AreaParentNId +" = A."+Area.AreaNId +") AS Children");
                SBQry.Append(" FROM "+ tableNames.Area +" A");
                SBQry.Append(" WHERE "+ Area.AreaLevel +" = 1");
                StrQry = SBQry.ToString();                
                RetVal = dbConnection.ExecuteDataTable(StrQry);               
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// Create and insert into TmpDI7SearchAreas table
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="searchAreas"></param>
        /// <param name="isSearchForQS"></param>
        public static void CreateTmpAreaSearchTbl(DIConnection dbConnection, string searchAreas, bool isSearchForQS)
        {
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;

            string SearchLanguage = string.Empty;

            try
            {
                #region "-- Create table schema --"

                if (isSearchForQS)
                {
                    CacheUtility.CreateSearchAreasTable(dbConnection, false);
                }
                else
                {
                    CacheUtility.CreateSearchAreasTable(dbConnection, true);
                }                

                #endregion

                if (!string.IsNullOrEmpty(searchAreas))
                {
                    CacheUtility.GetSplittedList(dbConnection, searchAreas, ",", false);

                    SBQry.Remove(0, SBQry.Length);

                    SBQry.Append("INSERT INTO " + QDSConstants.QDSTables.SearchAreas.TableName);
                    SBQry.Append(" SELECT List." + QDSConstants.QDSTables.SplittedList.Columns.Value + " as AreaNId");
                    SBQry.Append(" FROM " + QDSConstants.QDSTables.SplittedList.TableName + " List");

                    StrQry = SBQry.ToString();

                    dbConnection.ExecuteNonQuery(StrQry);
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create and insert into TmpDI7SearchIndicators table
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableNames"></param>
        /// <param name="searchIndicators"></param>
        /// <param name="isSearchForQS"></param>
        public static void CreateTmpIndSearchTbl(DIConnection dbConnection, DITables tableNames, string searchIndicators, bool isSearchForQS)
        {
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;

            string SearchLanguage = string.Empty;

            try
            {
                CacheUtility.CreateSearchIndicatorsTable(dbConnection);

                if (!string.IsNullOrEmpty(searchIndicators))
                {
                    CacheUtility.GetSplittedList(dbConnection, searchIndicators, ",", true);

                    SBQry.Remove(0, SBQry.Length);
                    SBQry.Append("INSERT INTO " + QDSConstants.QDSTables.SearchIndicators.TableName);
                    SBQry.Append(" (" + QDSConstants.QDSTables.SearchIndicators.Columns.IndicatorNId + ")");
                    SBQry.Append(" SELECT Ind."+ Indicator.IndicatorNId +" As IndicatorNId FROM "+ tableNames.Indicator +" As Ind");
                    SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SplittedList.TableName + " L");
                    SBQry.Append(" ON Ind." + Indicator.IndicatorNId + " = L." + QDSConstants.QDSTables.SplittedList.Columns.Value);
                    StrQry = SBQry.ToString();                    
                    dbConnection.ExecuteNonQuery(StrQry);                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }           

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        /// <param name="isSearchForQS"></param>
        public static void GetSearchResults(DIConnection dbConnection, string languageCode, bool isSearchForQS)
        {
            //-- SP_GET_SEARCH_RESULTS_XX
                        
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            string SearchLanguage = string.Empty;

            try
            {
                SearchLanguage = languageCode;

                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("Insert into " + QDSConstants.QDSTables.ParentTable.TableName);
                SBQry.Append(" (" + QDSConstants.QDSTables.ParentTable.Columns.NId + ", " + QDSConstants.QDSTables.ParentTable.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.ParentTable.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.ParentTable.Columns.UnitNId + ", " + QDSConstants.QDSTables.ParentTable.Columns.AreaNId + ", " + QDSConstants.QDSTables.ParentTable.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.ParentTable.Columns.IndicatorName + ", " + QDSConstants.QDSTables.ParentTable.Columns.Unit + ", " + QDSConstants.QDSTables.ParentTable.Columns.Area + ", " + QDSConstants.QDSTables.ParentTable.Columns.DefaultSG + ", " + QDSConstants.QDSTables.ParentTable.Columns.MRDTP + ", " + QDSConstants.QDSTables.ParentTable.Columns.MRD + ", " + QDSConstants.QDSTables.ParentTable.Columns.AreaCount + ", " + QDSConstants.QDSTables.ParentTable.Columns.SGCount + ", " + QDSConstants.QDSTables.ParentTable.Columns.SourceCount + ", " + QDSConstants.QDSTables.ParentTable.Columns.TPCount + ", " + QDSConstants.QDSTables.ParentTable.Columns.DVCount + ", " + QDSConstants.QDSTables.ParentTable.Columns.AreaNIds + ", " + QDSConstants.QDSTables.ParentTable.Columns.SGNIds + ", " + QDSConstants.QDSTables.ParentTable.Columns.SourceNIds + ", " + QDSConstants.QDSTables.ParentTable.Columns.TPNIds + ", " + QDSConstants.QDSTables.ParentTable.Columns.DVSeries + ", " + QDSConstants.QDSTables.ParentTable.Columns.Dimensions + ", " + QDSConstants.QDSTables.ParentTable.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.ParentTable.Columns.IUSNId + ", " + QDSConstants.QDSTables.ParentTable.Columns.AreaParentNId + ")");
                SBQry.Append(" SELECT DISTINCT " + QDSConstants.QDSTables.DISearchResult.Columns.NId + ", '" + SearchLanguage + "' As " + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + " As [Indicator], " + QDSConstants.QDSTables.DISearchResult.Columns.Unit + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Area + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRD + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SGCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SourceCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SGNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SourceNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Dimensions + ", " + QDSConstants.QDSTables.DISearchResult.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId);
                SBQry.Append(" FROM ("+ QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode +" C");

                if (isSearchForQS)
                {
                    SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchAreas.TableName + " A ON C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + " = cstr(A." + QDSConstants.QDSTables.SearchAreas.Columns.AreaNId + ") ) ");
                }
                else
                {
                    SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchAreas.TableName + " A ON C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + " = A." + QDSConstants.QDSTables.SearchAreas.Columns.AreaNId + " ) ");
                }

                SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchIndicators.TableName + " I ON C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + " = I." + QDSConstants.QDSTables.SearchIndicators.Columns.IndicatorNId);
                SBQry.Append(" WHERE '" + SearchLanguage + "' = C." + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + " and " + QDSConstants.QDSTables.DISearchResult.Columns.IsBlockAreaRecord + " <> -1 ");
                SBQry.Append(" ORDER BY C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.Area + " ASC");
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);                  
            }
            catch (Exception)
            {
                throw;
            }            
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        /// <param name="searchIndicators"></param>
        /// <param name="isSearchForQS"></param>
        /// <returns></returns>
        public static DataTable GetSearchChildResults(DIConnection dbConnection, string languageCode, string searchIndicators, bool isSearchForQS)
        {
            DataTable RetVal = null;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            string SearchLanguage = languageCode;
            int MaxAreaLength = -1;
            int MaxDVSeriesLength = -1;

            try
            {   
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("Insert into " + QDSConstants.QDSTables.ChildTable.TableName);
                SBQry.Append(" (" + QDSConstants.QDSTables.ChildTable.Columns.NId + ", " + QDSConstants.QDSTables.ChildTable.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.ChildTable.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.UnitNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.AreaNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.ChildTable.Columns.IndicatorName + ", " + QDSConstants.QDSTables.ChildTable.Columns.Unit + ", " + QDSConstants.QDSTables.ChildTable.Columns.Area + ", " + QDSConstants.QDSTables.ChildTable.Columns.DefaultSG + ", " + QDSConstants.QDSTables.ChildTable.Columns.MRDTP + ", " + QDSConstants.QDSTables.ChildTable.Columns.MRD + ", " + QDSConstants.QDSTables.ChildTable.Columns.AreaCount + ", " + QDSConstants.QDSTables.ChildTable.Columns.SGCount + ", " + QDSConstants.QDSTables.ChildTable.Columns.SourceCount + ", " + QDSConstants.QDSTables.ChildTable.Columns.TPCount + ", " + QDSConstants.QDSTables.ChildTable.Columns.DVCount + ", " + QDSConstants.QDSTables.ChildTable.Columns.AreaNIds + ", " + QDSConstants.QDSTables.ChildTable.Columns.SGNIds + ", " + QDSConstants.QDSTables.ChildTable.Columns.SourceNIds + ", " + QDSConstants.QDSTables.ChildTable.Columns.TPNIds + ", " + QDSConstants.QDSTables.ChildTable.Columns.DVSeries + ", " + QDSConstants.QDSTables.ChildTable.Columns.Dimensions + ", " + QDSConstants.QDSTables.ChildTable.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.IUSNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.AreaParentNId + ")");
                SBQry.Append(" SELECT DISTINCT " + QDSConstants.QDSTables.DISearchResult.Columns.NId + ", '" + SearchLanguage + "' As " + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + " As [Indicator], " + QDSConstants.QDSTables.DISearchResult.Columns.Unit + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Area + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRD + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SGCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SourceCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SGNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SourceNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Dimensions + ", " + QDSConstants.QDSTables.DISearchResult.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId);
                SBQry.Append(" FROM (" + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode + " C");

                if (isSearchForQS)
                {
                    SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchAreas.TableName + " A ON C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + " = cstr(A.AreaNId) ) ");
                }
                else
                {
                    SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchAreas.TableName + " A ON C." + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId + " = A.AreaNId ) ");
                }

                SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.SearchIndicators.TableName + " I ON C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + " = I.IndicatorNId");
                SBQry.Append(" WHERE '" + SearchLanguage + "' = C.SearchLanguage ");
                SBQry.Append(" ORDER BY C." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + ", C." + QDSConstants.QDSTables.DISearchResult.Columns.Area + " ASC");
                StrQry = SBQry.ToString();                
                dbConnection.ExecuteNonQuery(StrQry);


                //-- Get max area length
                StrQry = "SELECT Max(Len(" + QDSConstants.QDSTables.ChildTable.Columns.Area + ")) FROM " + QDSConstants.QDSTables.ChildTable.TableName;
                Int32.TryParse(Convert.ToString(dbConnection.ExecuteScalarSqlQuery(StrQry)),out MaxAreaLength);
                

                //-- Get max data value length
                StrQry = "SELECT Max(Len(" + QDSConstants.QDSTables.ChildTable.Columns.DVSeries + ")) FROM " + QDSConstants.QDSTables.ChildTable.TableName;
                Int32.TryParse(Convert.ToString(dbConnection.ExecuteScalarSqlQuery(StrQry)), out MaxDVSeriesLength);
                
                
                                
                //-- Get data with padded length
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("SELECT Distinct *, ");
                SBQry.Append(QDSConstants.QDSTables.ChildTable.Columns.Area + " & Space(" + MaxAreaLength + " - Len(" + QDSConstants.QDSTables.ChildTable.Columns.Area + ")) as PaddedArea,");
                SBQry.Append(" Space(" + MaxDVSeriesLength + " - Len(" + QDSConstants.QDSTables.ChildTable.Columns.DVSeries + ")) & " + QDSConstants.QDSTables.ChildTable.Columns.DVSeries + " as PaddedDVSeries");
                SBQry.Append(" FROM " + QDSConstants.QDSTables.ChildTable.TableName);
                StrQry = SBQry.ToString();
                RetVal = dbConnection.ExecuteDataTable(StrQry);
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        /// <param name="tableNames"></param>
        public static void InsertIntoTmpChildAreaTable(DIConnection dbConnection, string languageCode, DITables tableNames)
        {            
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            string SearchLanguage = languageCode;

            try
            {
                SBQry.Append("Insert into " + QDSConstants.QDSTables.ChildAreaTable.TableName );
                SBQry.Append(" (" + QDSConstants.QDSTables.ChildAreaTable.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.UnitNId + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.AreaNId + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.IndicatorName + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.Unit + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.Area + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.DefaultSG + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.Dimensions + ", " + QDSConstants.QDSTables.ChildAreaTable.Columns.IUSNId + " )");
                SBQry.Append(" SELECT DISTINCT '" + SearchLanguage + "' As " + QDSConstants.QDSTables.ChildTable.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.ChildTable.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.UnitNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.AreaParentNId + " As " + QDSConstants.QDSTables.ChildTable.Columns.AreaNId + ", " + QDSConstants.QDSTables.ChildTable.Columns.IndicatorName + ", " + QDSConstants.QDSTables.ChildTable.Columns.Unit + ", A." + Area.AreaName + ", " + QDSConstants.QDSTables.ChildTable.Columns.DefaultSG + ", " + QDSConstants.QDSTables.ChildTable.Columns.Dimensions + ", " + QDSConstants.QDSTables.ChildTable.Columns.IUSNId);
                SBQry.Append(" FROM " + QDSConstants.QDSTables.ChildTable.TableName + " C");
                SBQry.Append(" INNER JOIN " + tableNames.Area + " A ON A."+ Area.AreaNId +"=C." + QDSConstants.QDSTables.ChildTable.Columns.AreaParentNId);
                StrQry = SBQry.ToString();                
                dbConnection.ExecuteNonQuery(StrQry);                 
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        public static void GetNewParentTable(DIConnection dbConnection, string languageCode)
        {            
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            string SearchLanguage = string.Empty;

            try
            {
                //-- Create an empty New Parent Table
                CacheUtility.CreateNewParentTbl(dbConnection, languageCode);

                SBQry.Append(" Insert into " + QDSConstants.QDSTables.NewParentTable.TableName);
                SBQry.Append(" Select * from (");
                SBQry.Append(" Select * from " + QDSConstants.QDSTables.ParentTable.TableName);
                SBQry.Append(" Union");
                SBQry.Append(" Select * from " + QDSConstants.QDSTables.ChildAreaTable.TableName);
                SBQry.Append(" ) A");
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);                
            }
            catch (Exception)
            {
                throw;
            }            
        }

        /// <summary>
        /// Delete duplicate records of same IUS
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static DataTable DeletedDuplicateRecordOfSameIUS(DIConnection dbConnection)
        {
            DataTable RetVal = null;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;           

            try
            {
                SBQry.Append("Delete FROM " + QDSConstants.QDSTables.NewParentTable.TableName + " AS t");
                SBQry.Append(" where (" + QDSConstants.QDSTables.NewParentTable.Columns.DVSeries + " is null or " + QDSConstants.QDSTables.NewParentTable.Columns.DVSeries + " ='') and exists");
                SBQry.Append(" (SELECT " + QDSConstants.QDSTables.NewParentTable.Columns.IUSNId + ", " + QDSConstants.QDSTables.NewParentTable.Columns.AreaNId);
                SBQry.Append(" FROM " + QDSConstants.QDSTables.NewParentTable.TableName + " AS t1");
                SBQry.Append(" Where t." + QDSConstants.QDSTables.NewParentTable.Columns.IUSNId + "=t1." + QDSConstants.QDSTables.NewParentTable.Columns.IUSNId + " and t." + QDSConstants.QDSTables.NewParentTable.Columns.AreaNId + "=t1." + QDSConstants.QDSTables.NewParentTable.Columns.AreaNId);
                SBQry.Append(" group by " + QDSConstants.QDSTables.NewParentTable.Columns.IUSNId + ", " + QDSConstants.QDSTables.NewParentTable.Columns.AreaNId);
                SBQry.Append(" Having count(*)>1)");
                StrQry = SBQry.ToString();                
                dbConnection.ExecuteNonQuery(StrQry);

                StrQry = "Select * from " + QDSConstants.QDSTables.NewParentTable.TableName;
                RetVal = dbConnection.ExecuteDataTable(StrQry);   
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// Get the area level name
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableNames"></param>
        /// <param name="AreaNId"></param>
        /// <returns></returns>
        public static string GetAreaLevelName(DIConnection dbConnection, DITables tableNames, string AreaNId)
        {
            string RetVal = string.Empty;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;            

            try
            {
                SBQry.Append("SELECT L." + Area_Level.AreaLevelName);
                SBQry.Append(" FROM "+ QDSConstants.QDSTables.NewParentTable.TableName +" AS P ");
                SBQry.Append(" INNER JOIN ("+ tableNames.AreaLevel +" AS L INNER JOIN "+ tableNames.Area +" AS A ON L."+ Area_Level.AreaLevel +" = A."+Area.AreaLevel + "+1) ");
                SBQry.Append(" ON P." + QDSConstants.QDSTables.NewParentTable.Columns.AreaNId + " = A." + Area.AreaNId);
                SBQry.Append(" where cstr(A." + Area.AreaNId + ") = " + AreaNId);
                StrQry = SBQry.ToString();                
                RetVal = Convert.ToString(dbConnection.ExecuteScalarSqlQuery(StrQry));                
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        #endregion

        #region "-- Cache Generation --"

        /// <summary>
        /// Get all languge codes exists in database
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static List<string> GetAllDbLangCodes(DIConnection dbConnection)
        {
            List<string> RetVal = new List<string>();
            string StrQry = string.Empty;
            DataTable DtAllLanguages = null;
            DITables TableNames;
            string DataPrefix = string.Empty;

            try
            {
                //-- Get default data prefix in database
                DataPrefix = dbConnection.DIDataSetDefault();

                //-- Get all table names
                TableNames = new DITables(DataPrefix, dbConnection.DILanguageCodeDefault(DataPrefix));
                
                StrQry = "SELECT " + Language.LanguageCode + " FROM " + TableNames.Language;

                DtAllLanguages = dbConnection.ExecuteDataTable(StrQry);

                foreach (DataRow Row in DtAllLanguages.Rows)
                {
                    RetVal.Add(Convert.ToString(Row[0]));
                }               
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        /// <summary>
        /// Get indicator NIds from ICNId
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableNames"></param>
        /// <param name="icNId"></param>
        /// <returns></returns>
        public static string GetIndicatorNIdsFromICNId(DIConnection dbConnection, DITables tableNames, int icNId)
        {
            string RetVal = string.Empty;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            DataTable DtResult = null;
            string IndNIds = string.Empty;

            try
            {
                SBQry.Append("SELECT IUS." + Indicator_Unit_Subgroup.IndicatorNId);
                SBQry.Append(" FROM " + tableNames.IndicatorClassificationsIUS + " As ICIUS");
                SBQry.Append(" INNER JOIN "+ tableNames.IndicatorUnitSubgroup +" As IUS");
                SBQry.Append(" ON ICIUS."+ IndicatorClassificationsIUS.IUSNId +" = IUS." + Indicator_Unit_Subgroup.IUSNId);
                SBQry.Append(" Where ICIUS." + IndicatorClassificationsIUS.ICNId + " = " + icNId);
                StrQry = SBQry.ToString();

                DtResult = dbConnection.ExecuteDataTable(StrQry);

                if (DtResult.Rows.Count > 0)
                {
                    foreach (DataRow Row in DtResult.Rows)
                    {
                        IndNIds += "," + Convert.ToString(Row["Indicator_NId"]);
                    }

                    if (!string.IsNullOrEmpty(IndNIds))
                    {
                        RetVal = IndNIds.Substring(1);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }
        
        #endregion

        #endregion

        #endregion
    }
}
