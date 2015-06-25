using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.QDS.UI.Databases;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.QDS.DI7CacheNSP
{
    public class CacheGenerator
    {
        #region "-- Private --"

        #region "-- Methods --"
        
        /// <summary>
        /// Create chache results for a language
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        private void CreateCacheResults(DIConnection dbConnection, string languageCode)
        {
            DITables TableNames;

            try
            {
                //-- Get all tables by dataset and language basis
                TableNames = new DITables(dbConnection.DIDataSetDefault(), languageCode);

                //-- Create cache table (DI_Search_Results)
                this.CreateCacheResultsForDISearchResults(dbConnection, languageCode, TableNames);
                                
                //-- Block
                this.CreateCacheResultsForBlock(dbConnection, languageCode, TableNames);
                
                //-- Quick Search (level)
                this.CreateCacheResultsForLevel(dbConnection, languageCode, TableNames);
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        /// <summary>
        /// Create cache result for DISearchResults
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        private void CreateCacheResultsForDISearchResults(DIConnection dbConnection, string languageCode, DITables tableNames)
        {
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;            
            int ProgressCount = 24;            

            try
            {
                #region "-- Create cache table (DI_Search_Results) --"

                //-- Create DI_Search_Result table for using in cache generation
                CacheUtility.CreateDISearchResultsTable(dbConnection, languageCode);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Delete all record
                CacheUtility.DeleteTableRecords(dbConnection, QDSConstants.QDSTables.TempMRDRecords.TableName);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);
                
                //-- Get MRD records and create tempMRDRecords table                
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("Insert Into " + QDSConstants.QDSTables.TempMRDRecords.TableName);
                SBQry.Append(" (" + QDSConstants.QDSTables.TempMRDRecords.Columns.IUSNId + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.UnitNId + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.SubgroupValNId + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.AreaNId + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.Timeperiod + ", " + QDSConstants.QDSTables.TempMRDRecords.Columns.DVCount + " )");
                SBQry.Append(" SELECT " + Data.IUSNId + ", D." + Data.IndicatorNId + ", D." + Data.UnitNId + ", D." + Data.SubgroupValNId + ", " + Data.AreaNId + ", MAX(D." + QDSConstants.QDSTables.Data.Columns.TimePeriod + ") AS " + QDSConstants.QDSTables.Data.Columns.TimePeriod + ",");
                SBQry.Append(" Count(*) AS " + QDSConstants.QDSTables.Data.Columns.DVCount);
                SBQry.Append(" FROM " + tableNames.Data + " AS D");
                SBQry.Append(" WHERE D."+ QDSConstants.QDSTables.Data.Columns.ISDefaultSG +" = -1");
                SBQry.Append(" GROUP BY D."+ Data.IUSNId +", D."+ Data.AreaNId +", D."+ Data.IndicatorNId +", D."+ Data.UnitNId +", D." + Data.SubgroupValNId);
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);
                                


                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Update DataValue into TempMRDRecords table using data table
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("UPDATE "+ tableNames.Data +" AS d1 INNER JOIN " + QDSConstants.QDSTables.TempMRDRecords.TableName + " AS t1");
                SBQry.Append(" ON (d1." + QDSConstants.QDSTables.Data.Columns.TimePeriod + "=t1." + QDSConstants.QDSTables.TempMRDRecords.Columns.Timeperiod + ")");
                SBQry.Append(" AND (d1." + Data.IUSNId + "=t1." + QDSConstants.QDSTables.TempMRDRecords.Columns.IUSNId + ") AND (D1." + Data.AreaNId + " = t1." + QDSConstants.QDSTables.TempMRDRecords.Columns.AreaNId + ")");
                SBQry.Append(" SET t1." + QDSConstants.QDSTables.TempMRDRecords.Columns.DV + " = d1." + Data.DataValue);
                SBQry.Append(" WHERE d1.isdefaultSG = -1");
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);

                

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Insert records into DISearchResults table
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("INSERT INTO " + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode);
                SBQry.Append(" (" + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRD + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Unit + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Area + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + ")");
                SBQry.Append(" SELECT '" + languageCode + "', D." + QDSConstants.QDSTables.TempMRDRecords.Columns.IndicatorNId + ", D." + QDSConstants.QDSTables.TempMRDRecords.Columns.UnitNId + ", D." + QDSConstants.QDSTables.TempMRDRecords.Columns.AreaNId + ", -1 AS Expr1, D." + QDSConstants.QDSTables.TempMRDRecords.Columns.Timeperiod + ", t." + Timeperiods.TimePeriodNId + ", D." + QDSConstants.QDSTables.TempMRDRecords.Columns.IUSNId + ",");
                SBQry.Append(" d." + QDSConstants.QDSTables.TempMRDRecords.Columns.SubgroupValNId + " & '[@@@@]' & SG."+ SubgroupVals.SubgroupVal + " AS Expr2, I."+ Indicator.IndicatorName +", U."+ Unit.UnitName +", A."+ Area.AreaName +",");
                SBQry.Append(" A.area_parent_nid, D." + QDSConstants.QDSTables.TempMRDRecords.Columns.DV + ", D." + QDSConstants.QDSTables.TempMRDRecords.Columns.DVCount);
                SBQry.Append(" FROM " + tableNames.TimePeriod + " AS t");
                SBQry.Append(" INNER JOIN (((" + tableNames.Unit + " AS U INNER JOIN (" + tableNames.Area + " AS A");
                SBQry.Append(" INNER JOIN " + QDSConstants.QDSTables.TempMRDRecords.TableName + " AS D ON A."+ Area.AreaNId +" = D." + QDSConstants.QDSTables.TempMRDRecords.Columns.AreaNId + ")");
                SBQry.Append(" ON U."+ Unit.UnitNId +" = D." + QDSConstants.QDSTables.TempMRDRecords.Columns.UnitNId + ") INNER JOIN " + tableNames.SubgroupVals + " AS SG ON D." + QDSConstants.QDSTables.TempMRDRecords.Columns.SubgroupValNId + " = SG." + SubgroupVals.SubgroupValNId + ")");
                SBQry.Append(" INNER JOIN " + tableNames.Indicator + " AS I ON D." + QDSConstants.QDSTables.TempMRDRecords.Columns.IndicatorNId + " = I."+ Indicator.IndicatorNId +")");
                SBQry.Append(" ON t."+ Timeperiods.TimePeriod +" = D." + QDSConstants.QDSTables.TempMRDRecords.Columns.Timeperiod);
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);

                #endregion                               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create cache result of blocks
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        private void CreateCacheResultsForBlock(DIConnection dbConnection, string languageCode, DITables tableNames)
        {
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;            
            int ProgressCount = 29;            

            try
            {                
                #region "-- Block --"

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Area block - get area where block exists
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("SELECT " + Area.AreaNId + ", " + Area.AreaBlock);
                SBQry.Append(" FROM " + tableNames.Area );
                SBQry.Append(" WHERE " + Area.AreaBlock + " is not null and " + Area.AreaBlock + "<>''");
                StrQry = SBQry.ToString();
                DataTable DtAreaBlocks = dbConnection.ExecuteDataTable(StrQry);
                              
                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);
                
                //-- Create BlockAreaResults table
                CacheUtility.CreateBlockAreaResultsTable(dbConnection, languageCode);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                if (DtAreaBlocks.Rows.Count > 0)
                {
                    foreach (DataRow Row in DtAreaBlocks.Rows)
                    {
                        string AreaNId = Convert.ToString(Row[Area.AreaNId]);
                        string AreaBlock = Convert.ToString(Row[Area.AreaBlock]);

                        //-- Area block - insert record into block area results
                        SBQry.Remove(0, SBQry.Length);
                        SBQry.Append("INSERT INTO " + QDSConstants.QDSTables.BlockAreaResults.TableName);
                        SBQry.Append(" ( " + QDSConstants.QDSTables.BlockAreaResults.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.UnitNId + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaNId + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.IndicatorName + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.Unit + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.Area + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.DefaultSG + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.MRDTP + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.MRD + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaCount + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.SGCount + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.SourceCount + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.TPCount + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.DVCount + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaNIds + ", ");
                        SBQry.Append(QDSConstants.QDSTables.BlockAreaResults.Columns.SGNIds + " , " + QDSConstants.QDSTables.BlockAreaResults.Columns.SourceNIds + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.TPNIds + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.DVNIds + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.DVSeries + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.Dimensions + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.IUSNId + ", " + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaParentNId + " )");
                        SBQry.Append(" SELECT D." + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.Unit + ",");
                        SBQry.Append(" D." + QDSConstants.QDSTables.DISearchResult.Columns.Area + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.MRD + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.AreaCount + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.SGCount + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.SourceCount + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.TPCount + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + ",");
                        SBQry.Append(" D." + QDSConstants.QDSTables.DISearchResult.Columns.SGNIds + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.SourceNIds + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.TPNIds + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.DVNIds + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.Dimensions + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.BlockAreaParentNId + ", D." + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + ", " + AreaNId);
                        SBQry.Append(" FROM " + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode + " AS D");
                        SBQry.Append(" WHERE d." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + " in (" + AreaBlock + ")");
                        StrQry = SBQry.ToString();
                        dbConnection.ExecuteNonQuery(StrQry);
                    }
                }

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Area block - Insert into di search results table
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("INSERT INTO " + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode);
                SBQry.Append(" ( " + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Unit + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Area + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + ", " + QDSConstants.QDSTables.DISearchResult.Columns.MRD + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SGCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.SourceCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + ", ");
                SBQry.Append(QDSConstants.QDSTables.DISearchResult.Columns.SGNIds + " , " + QDSConstants.QDSTables.DISearchResult.Columns.SourceNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.TPNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVNIds + ", " + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + ", " + QDSConstants.QDSTables.DISearchResult.Columns.Dimensions + ", " + QDSConstants.QDSTables.DISearchResult.Columns.BlockAreaParentNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId + ", " + QDSConstants.QDSTables.DISearchResult.Columns.IsBlockAreaRecord + " )");
                SBQry.Append(" SELECT D." + QDSConstants.QDSTables.BlockAreaResults.Columns.SearchLanguage + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.IndicatorNId + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.UnitNId + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaNId + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.IsAreaNumeric + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.IndicatorName + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.Unit + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.Area + ",");
                SBQry.Append(" D." + QDSConstants.QDSTables.BlockAreaResults.Columns.DefaultSG + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.MRDTP + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.MRD + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaCount + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.SGCount + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.SourceCount + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.TPCount + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.DVCount + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaNIds + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.SGNIds + ",");
                SBQry.Append(" D." + QDSConstants.QDSTables.BlockAreaResults.Columns.SourceNIds + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.TPNIds + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.DVNIds + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.DVSeries + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.Dimensions + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.BlockAreaParentNId + ", D." + QDSConstants.QDSTables.BlockAreaResults.Columns.IUSNId + ", d." + QDSConstants.QDSTables.BlockAreaResults.Columns.AreaParentNId + ", -1");
                SBQry.Append(" FROM " + QDSConstants.QDSTables.BlockAreaResults.TableName + " AS D");
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);                

                #endregion                                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create cache results for levels
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="languageCode"></param>
        private void CreateCacheResultsForLevel(DIConnection dbConnection, string languageCode, DITables tableNames)
        {
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;
            int AreaLevel = 4;
            int ProgressCount = 33;            

            try
            {
                #region "-- Quick Search (level) --"

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- 1 Add TempColumn column into Area table
                StrQry = "Alter table " + tableNames.Area + " add column " + QDSConstants.QDSTables.Area.Columns.TempColumn + " varchar";                
                dbConnection.ExecuteNonQuery(StrQry);

                //-- 2 Get area levels                
                StrQry = "Select count(*) from " + tableNames.AreaLevel;
                AreaLevel = (int)dbConnection.ExecuteScalarSqlQuery(StrQry);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                if (AreaLevel < 4)
                {
                    AreaLevel = 4;
                }

                //-- 3 Update query and execute it
                for (int i = 2; i <= AreaLevel; i++)
                {
                    StrQry = GetUpdateAreaTempColumnQry(i, languageCode, tableNames);
                    dbConnection.ExecuteNonQuery(StrQry);
                }

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- 4. Update into table DI_Search_Results of AreaNIds column with TempComumn column from ut_Area_xx table
                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("Update " + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode + " as R");
                SBQry.Append(" Inner Join "+ tableNames.Area + " as A");
                SBQry.Append(" On R." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + " = A." + Area.AreaNId);
                SBQry.Append(" And R."+ QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId + " = A." + Area.AreaParentNId);
                SBQry.Append(" Set R." + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + " = A.TempColumn");
                StrQry = SBQry.ToString();
                dbConnection.ExecuteNonQuery(StrQry);
                
                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- 5 Drop TempColumn column from Area table                
                StrQry = "Alter table " + tableNames.Area + " Drop Column " + QDSConstants.QDSTables.Area.Columns.TempColumn;
                dbConnection.ExecuteNonQuery(StrQry);

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Get query for update AreaNIds column on level basis new quick search id
        /// </summary>
        /// <param name="level"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private string GetUpdateAreaTempColumnQry(int level, string languageCode, DITables tableNames)
        {
            string RetVal = string.Empty;
            StringBuilder SBQry = new StringBuilder();
            string ParentIndex = "";
            string CurrentIndex = "";

            try
            {
                SBQry.Append("UPDATE ");

                for (int i = 2; i < level; i++)
                {
                    SBQry.Append(" ( ");
                }

                SBQry.Append( tableNames.Area +" AS a1 ");

                for (int i = 2; i<=level; i++)
                {
                    CurrentIndex = i.ToString();
                    ParentIndex = Convert.ToString(i - 1);
                    SBQry.Append(" INNER JOIN " + tableNames.Area + " AS a" + CurrentIndex + " ON a" + ParentIndex + "."+ Area.AreaNId +" = a" + CurrentIndex + "." + Area.AreaParentNId);
                    if (i >= 2 && i < level)
                    {
                        SBQry.Append(" ) ");
                    }
                }
                
                SBQry.Append(" SET a"+ level.ToString() +"."+ QDSConstants.QDSTables.Area.Columns.TempColumn +" = 'QS_' & a1."+ Area.AreaID +" & '_L"+ level.ToString() + "'");
                
                SBQry.Append(" WHERE a1."+ Area.AreaParentNId +"=-1");

                RetVal = SBQry.ToString();
            }
            catch (Exception ex)
            {                
                throw ex;
            }

            return RetVal;
        }

        /// <summary>
        /// Create and alter tables schema in database
        /// </summary>
        /// <param name="dbConnection"></param>
        private void CreateNAlterSchemas(DIConnection dbConnection)
        {
            string StrQry = string.Empty;
            DITables TableNames;
            string DataPrefix = string.Empty;
            
            try
            {
                //-- Get default data prefix in database
                DataPrefix = dbConnection.DIDataSetDefault();

                //-- Get all table names
                TableNames = new DITables(dbConnection.DIDataSetDefault(), dbConnection.DILanguageCodeDefault(dbConnection.DIDataSetDefault()));

                //-- Add timeperiod column into data                
                StrQry = "Alter table "+ TableNames.Data +" add column " + QDSConstants.QDSTables.Data.Columns.TimePeriod + " varchar(100)";
                dbConnection.ExecuteNonQuery(StrQry);

                //-- Add IsDefaultSG column into data                
                StrQry = "Alter table "+ TableNames.Data +" add column " + QDSConstants.QDSTables.Data.Columns.ISDefaultSG + " bit";
                dbConnection.ExecuteNonQuery(StrQry);

                //-- Create TempMRDRecords table
                CacheUtility.CreateTempMRDRecordsTable(dbConnection);
                
                //-- Update timeperiod values into data table
                StrQry = "UPDATE "+ TableNames.Data +" AS d INNER JOIN "+ TableNames.TimePeriod +" AS t ON d."+ Data.TimePeriodNId +" = t."+ Timeperiods.TimePeriodNId +" SET d."+  QDSConstants.QDSTables.Data.Columns.TimePeriod +" = t." + Timeperiods.TimePeriod;
                dbConnection.ExecuteNonQuery(StrQry);

                //-- Update default sg into data table
                StrQry = "UPDATE "+ TableNames.Data +" AS D INNER JOIN "+ TableNames.IndicatorUnitSubgroup +" AS IUS ON D." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId + " SET D."+ QDSConstants.QDSTables.Data.Columns.ISDefaultSG +" = IUS." + Indicator_Unit_Subgroup.IsDefaultSubgroup;
                dbConnection.ExecuteNonQuery(StrQry);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Events --"

        public event ProgressChangedDelegate ProgressChangedEvent;
        public event EventHandler DisplayProgressFormEvent;

        public void RaiseProgressChangedEvent(int value, string languageCode, string folderName, bool savingDatabase)
        {
            if (this.ProgressChangedEvent != null)
            {
                // todo: language handling for string
                if (!string.IsNullOrEmpty(languageCode))
                {
                    languageCode = "Generating xml files for:" + languageCode;
                }

                if (!string.IsNullOrEmpty(folderName))
                {
                    folderName = "XML Files:" + folderName;
                }
                if (savingDatabase)
                {
                    languageCode = "Saving database details...";
                }

                this.ProgressChangedEvent(value, "Optimizing database", languageCode, folderName, string.Empty);
            }
        }

        public void RaiseDisplayProgressFormEvent()
        {
            if (this.DisplayProgressFormEvent != null)
            {
                this.DisplayProgressFormEvent(this, null);
            }
        }

        #endregion
        
        #region "-- Methods --"
        
        /// <summary>
        /// Generate Cache Result tables on language basis
        /// </summary>
        /// <param name="databasePath"></param>
        public void GenerateCacheResults(string databasePath)
        {
            List<string> DbLangCodes = new List<string>();
            DIConnection DbConnection = null;
            string DataFolerName = string.Empty;
            int ProgressCount = 20;

            try
            {
                //-- raise event to display progress form
                this.RaiseDisplayProgressFormEvent();

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                DbConnection = new DIConnection(DIServerType.MsAccess, "", "", databasePath, "", "");
                
                DbLangCodes = DI7OfflineSPHelper.GetAllDbLangCodes(DbConnection);
                
                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Merge Textual_Data_Value and Data_value column into Data_value column
                DIDataValueHelper.MergeTextualandNumericDataValueColumn(databasePath);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);
                
                //-- Create new temp table and alter table schemas
                this.CreateNAlterSchemas(DbConnection);

                // increment progress bar value                
                this.RaiseProgressChangedEvent(ProgressCount++, string.Empty, string.Empty, false);

                //-- Generate cache on language basis
                foreach (string LangCode in DbLangCodes)
                {                    
                    this.CreateCacheResults(DbConnection, LangCode);
                }                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (DbConnection != null)
                {
                    DbConnection.Dispose();
                    DbConnection = null;
                }
            }
        }        

        #endregion

        #endregion
    }
}
