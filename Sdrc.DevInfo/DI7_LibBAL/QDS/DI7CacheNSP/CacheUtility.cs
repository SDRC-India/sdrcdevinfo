using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibBAL.QDS.DI7CacheNSP
{
    public static class CacheUtility
    {
        #region "-- Private --"

        #region "-- Methods --"

        /// <summary>
        /// Create new table schema of DI_Search_Result table
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="tableName"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        private static int CreateSearchResultCopyTable(DIConnection dbConnection, string tableName, string languageCode)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string StrQry = string.Empty;

            try
            {
                dbConnection.DropTable(tableName);

                SBQry.Remove(0, SBQry.Length);
                SBQry.Append("SELECT * INTO " + tableName);
                SBQry.Append(" FROM " + QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode);
                SBQry.Append(" WHERE 1=2");
                StrQry = SBQry.ToString();
                RetVal = dbConnection.ExecuteNonQuery(StrQry);
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        public static int DeleteTableRecords(DIConnection dbConnection, string tableName)
        {
            int RetVal = -1;
            string StrQry = string.Empty;

            try
            {
                StrQry = "Delete * from " + tableName;
                RetVal = dbConnection.ExecuteNonQuery(StrQry);                
            }
            catch (Exception)
            {
            }

            return RetVal;
        }       

        #region "-- Create table schema --"

        public static int CreateSplittedListTable(DIConnection dbConnection, bool isNumericValues)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;
            string ColumnDataType = "Text(255)";

            try
            {
                TblName = QDSConstants.QDSTables.SplittedList.TableName;
                                
                dbConnection.DropTable(TblName);

                if (isNumericValues)
                {
                    ColumnDataType = "Long";
                }

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("[" + QDSConstants.QDSTables.SplittedList.Columns.Value + "] " + ColumnDataType);
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        #region "-- Search Araea --"

        public static int CreateSearchAreasTable(DIConnection dbConnection, bool isNumericValues)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;
            string ColumnDataType = "Text(255)";

            try
            {
                TblName = QDSConstants.QDSTables.SearchAreas.TableName;

                dbConnection.DropTable(TblName);

                if (isNumericValues)
                {
                    ColumnDataType = "Long";
                }

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("["+ QDSConstants.QDSTables.SearchAreas.Columns.AreaNId +"] " + ColumnDataType);
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        public static int CreateSearchAreasTable1(DIConnection dbConnection)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;

            try
            {
                TblName = QDSConstants.QDSTables.SearchAreas.TableName;

                dbConnection.DropTable(TblName);

                SBQry.Append("Create Table " + TblName);
                SBQry.Append(" (");
                SBQry.Append(" [" + QDSConstants.QDSTables.SearchAreas.Columns.Id + "] COUNTER CONSTRAINT PKey PRIMARY KEY,");
                SBQry.Append(" [" + QDSConstants.QDSTables.SearchAreas.Columns.AreaNId + "] Long DEFAULT 0,");
                SBQry.Append(" [" + QDSConstants.QDSTables.SearchAreas.Columns.Area + "] Varchar(60) NOT NULL DEFAULT '' ");
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        #endregion

        #region "-- Search Indicators --"

        public static int CreateSearchIndicatorsTable(DIConnection dbConnection)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;

            try
            {
                TblName = QDSConstants.QDSTables.SearchIndicators.TableName;

                dbConnection.DropTable(TblName);

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("[" + QDSConstants.QDSTables.SearchIndicators.Columns.IndicatorNId + "] Long");
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        public static int CreateSearchIndicatorsTable1(DIConnection dbConnection)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;

            try
            {
                TblName = QDSConstants.QDSTables.SearchIndicators.TableName;

                dbConnection.DropTable(TblName);

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("[" + QDSConstants.QDSTables.SearchIndicators.Columns.Id + "] COUNTER CONSTRAINT PKey PRIMARY KEY, ");
                SBQry.Append("[" + QDSConstants.QDSTables.SearchIndicators.Columns.IndicatorNId + "] Long DEFAULT 0, ");
                SBQry.Append("[" + QDSConstants.QDSTables.SearchIndicators.Columns.IndicatorName + "] Varchar(255) DEFAULT '' NOT NULL , ");
                SBQry.Append("[" + QDSConstants.QDSTables.SearchIndicators.Columns.ICName + "] Varchar DEFAULT '' NOT NULL ");
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }

        #endregion

        public static int CreateDISearchResultsTable(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;

            try
            {
                TblName = QDSConstants.QDSTables.DISearchResult.TableName + "_" + languageCode;

                dbConnection.DropTable(TblName);

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.NId + "] COUNTER, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.SearchLanguage + "] Text(50) NOT NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.UnitNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.AreaNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.IsAreaNumeric + "] YESNO NOT NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.IndicatorName + "] Text(255) NOT NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.Unit + "] Text(128) NOT NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.Area + "] Text(255) NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.DefaultSG + "] Text(255) NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.MRDTP + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.MRD + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.AreaCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.SGCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.SourceCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.TPCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.DVCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.AreaNIds + "] Text(255) NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.SGNIds + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.SourceNIds + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.TPNIds + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.DVNIds + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.DVSeries + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.Dimensions + "] Text NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.BlockAreaParentNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.IUSNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.AreaParentNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.DISearchResult.Columns.IsBlockAreaRecord + "] YESNO");
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }
        
        public static int CreateParentTbl(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;            

            RetVal = CreateSearchResultCopyTable(dbConnection, QDSConstants.QDSTables.ParentTable.TableName, languageCode);

            return RetVal;
        }

        public static int CreateChildTbl(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;            

            RetVal = CreateSearchResultCopyTable(dbConnection, QDSConstants.QDSTables.ChildTable.TableName, languageCode);

            return RetVal;
        }

        public static int CreateChildAreaTbl(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;            

            RetVal = CreateSearchResultCopyTable(dbConnection, QDSConstants.QDSTables.ChildAreaTable.TableName, languageCode);

            return RetVal;
        }

        public static int CreateNewParentTbl(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;            

            RetVal = CreateSearchResultCopyTable(dbConnection, QDSConstants.QDSTables.NewParentTable.TableName, languageCode);

            return RetVal;
        }

        public static int CreateBlockAreaResultsTable(DIConnection dbConnection, string languageCode)
        {
            int RetVal = -1;            

            RetVal = CreateSearchResultCopyTable(dbConnection, QDSConstants.QDSTables.BlockAreaResults.TableName, languageCode);

            return RetVal;
        }

        public static int CreateTempMRDRecordsTable(DIConnection dbConnection)
        {
            int RetVal = -1;
            StringBuilder SBQry = new StringBuilder();
            string TblName = string.Empty;

            try
            {
                TblName = QDSConstants.QDSTables.TempMRDRecords.TableName;

                dbConnection.DropTable(TblName);

                SBQry.Append("Create Table " + TblName);
                SBQry.Append("(");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.IUSNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.IndicatorNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.UnitNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.SubgroupValNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.AreaNId + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.Timeperiod + "] Text(255) NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.DVCount + "] Long NULL, ");
                SBQry.Append("[" + QDSConstants.QDSTables.TempMRDRecords.Columns.DV + "] Text(255) NULL ");
                SBQry.Append(")");

                RetVal = dbConnection.ExecuteNonQuery(SBQry.ToString());
            }
            catch (Exception)
            {
                throw;
            }

            return RetVal;
        }
        
        #endregion
        
        #region "-- DB functions  --"

        public static void GetSplittedList(DIConnection dbConnection, string strDelimitedText, string delimiter, bool isNumericValues)
        {
            //-- FN_GET_SPLITTED_LIST

            string[] TxtArr;
            string StrQry = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(strDelimitedText))
                {
                    CreateSplittedListTable(dbConnection, isNumericValues);

                    //-- Split delimited text into array
                    TxtArr = DICommon.SplitString(strDelimitedText, delimiter);

                    //-- Insert each text into table
                    foreach (string Txt in TxtArr)
                    {
                        if (!string.IsNullOrEmpty(Txt))
                        {
                            StrQry = "Insert into " + QDSConstants.QDSTables.SplittedList.TableName;

                            if (isNumericValues)
                            {
                                StrQry += " values (" + Txt + ")";
                            }
                            else
                            {
                                StrQry += " values ('" + Txt + "')";
                            }

                            dbConnection.ExecuteNonQuery(StrQry);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion

        #endregion

        #endregion
    }
}
