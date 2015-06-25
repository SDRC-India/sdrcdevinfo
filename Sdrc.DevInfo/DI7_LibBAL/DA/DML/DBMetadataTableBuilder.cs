using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides method for inseting, deleting and updating records in DBMetadata table
    /// </summary>
    public class DBMetadataTableBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        DIConnection DBConnection;
        DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        

        #endregion

        #endregion

        #region "-- public --"

        #region "-- New/Dispose --"

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public DBMetadataTableBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"


        /// <summary>
        /// Returns true if DBMetadata table exists otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsDBMetadataTableExists()
        {
            bool RetVal = false;

            try
            {
                //-- Check the existence of the table 
                if (this.DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this.DBQueries.TablesName.DBMetadata + " WHERE 1=1") != null)
                    RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert DBMetadata information
        /// </summary>
        /// <param name="description"></param>
        /// <param name="publisherName"></param>
        /// <param name="publisherDate"></param>
        /// <param name="publisherCountry"></param>
        /// <param name="publisherRegion"></param>
        /// <param name="publisherOffice"></param>
        /// <param name="areaCount"></param>
        /// <param name="indicatorCount"></param>
        /// <param name="IUSCount"></param>
        /// <param name="timeperiodCount"></param>
        /// <param name="sourceCount"></param>
        /// <param name="dataCount"></param>        
        public  void InsertRecord(string description, string publisherName, string publisherDate,
            string publisherCountry, string publisherRegion, string publisherOffice,
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount, string sourceCount, string dataCount)
        {
            string SqlQuery = string.Empty;
            DITables TablesName;
            string DataPrefix = this.DBConnection.DIDataSetDefault();


            try
            {
                if (string.IsNullOrEmpty(areaCount))
                {
                    areaCount = "0";
                }
                if (string.IsNullOrEmpty(indicatorCount))
                {
                    indicatorCount = "0";
                }
                if (string.IsNullOrEmpty(IUSCount))
                {
                    IUSCount = "0";
                }
                if (string.IsNullOrEmpty(timeperiodCount))
                {
                    timeperiodCount = "0";
                }
                if (string.IsNullOrEmpty(sourceCount))
                {
                    sourceCount = "0";
                }
                if (string.IsNullOrEmpty(dataCount))
                {
                    dataCount = "0";
                }

                foreach (DataRow Row in this.DBConnection.DILanguages(DataPrefix).Rows)
                {
                    TablesName = new DITables(DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.DBMetadata.Insert.InsertRecord(TablesName.DBMetadata, DICommon.RemoveQuotes(description),
                         DICommon.RemoveQuotes(publisherName), publisherDate, DICommon.RemoveQuotes(publisherCountry), DICommon.RemoveQuotes(publisherRegion),
                         DICommon.RemoveQuotes(publisherOffice), areaCount, indicatorCount, IUSCount, timeperiodCount, sourceCount, dataCount);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }



        /// <summary>
        /// Updates count information 
        /// </summary>
        /// <param name="NID"></param>
        /// <param name="areaCount"></param>
        /// <param name="indicatorCount"></param>
        /// <param name="IUSCount"></param>
        /// <param name="timeperiodCount"></param>
        /// <param name="sourceCount"></param>
        /// <param name="dataCount"></param>        
        public  void UpdateCounts(int NID,
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount, string sourceCount, string dataCount)
        {
            string PublishedOn=System.DateTime.Now.ToString( "yyyy-MM-dd");
            string SqlQuery = string.Empty;
            DITables TablesName;
            string DataPrefix = this.DBConnection.DIDataSetDefault();
            bool UseUpdateQuery = true;
            
            try
            {
                // check record exists in DB_Metadata table or not
                if (Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.DBMetadata, string.Empty))) == 0)
                {
                    UseUpdateQuery = false;
                }

                foreach (DataRow Row in this.DBConnection.DILanguages(DataPrefix).Rows)
                {
                    TablesName = new DITables(DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    if (UseUpdateQuery)
                    {
                        // update query
                        SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.DBMetadata.Update.UpdateCounts(TablesName.DBMetadata, NID,
                            areaCount, indicatorCount, IUSCount, timeperiodCount, sourceCount, dataCount);

                    }
                    else
                    {
                        //insert query
                        SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.DBMetadata.Insert.InsertRecord(TablesName.DBMetadata, string.Empty, string.Empty, PublishedOn, string.Empty, string.Empty, string.Empty,
                        areaCount, indicatorCount, IUSCount, timeperiodCount, sourceCount, dataCount);

                    }
                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// Gets the count from database/template and update into DBMetadata Table
        /// </summary>
        public void GetNUpdateCounts()
        {
             string AreaCount=string.Empty;
            string IndicatorCount=string.Empty;
            string IUSCount=string.Empty;
            string TimeperiodCount=string.Empty;
            string SourceCount=string.Empty;
            string DataCount = string.Empty;
            string SearchString = string.Empty;
            DataTable SourceTable;

            // Get NId from DBMetadata Table
            
            // Get area count
            AreaCount = this.DBConnection.ExecuteScalarSqlQuery( DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.Area, string.Empty)).ToString();
            
            // Get Indicator count
            IndicatorCount = this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.Indicator, string.Empty)).ToString();
            // Get IUS count
            IUSCount = this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.IndicatorUnitSubgroup, string.Empty)).ToString();

            // Get timeperiod count
            TimeperiodCount = this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.TimePeriod, string.Empty)).ToString();

            // Get source count
            SearchString = IndicatorClassifications.ICParent_NId + ">0";
            SourceTable= this.DBConnection.ExecuteDataTable(this.DBQueries.Source.GetSource(FilterFieldType.Search, SearchString, FieldSelection.NId, false));
            if (SourceTable.Rows.Count > 0)
            {
                SourceCount = Convert.ToString(SourceTable.Select().Length);
            }
            else
            {
                SourceCount = "0";
            }
           

           // Get data count
            DataCount = this.DBConnection.ExecuteScalarSqlQuery(DIQueries.GetTableRecordsCount(this.DBQueries.TablesName.Data, string.Empty)).ToString();

           this.UpdateCounts(-1, AreaCount, IndicatorCount, IUSCount, TimeperiodCount, SourceCount, DataCount);

        }

        /// <summary>
        /// Updates  DBMetadata information into current language table.
        /// </summary>
        /// <param name="NID"></param>
        /// <param name="description"></param>
        /// <param name="publisherName"></param>
        /// <param name="publisherDate"></param>
        /// <param name="publisherCountry"></param>
        /// <param name="publisherRegion"></param>
        /// <param name="publisherOffice"></param>
        /// <param name="areaCount"></param>
        /// <param name="indicatorCount"></param>
        /// <param name="IUSCount"></param>
        /// <param name="timeperiodCount"></param>
        /// <param name="sourceCount"></param>
        /// <param name="dataCount"></param>        
        public void UpdateRecord(int NID,string description, string publisherName, string publisherDate,
            string publisherCountry, string publisherRegion, string publisherOffice,
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount, string sourceCount, string dataCount)
        {
            string SqlQuery = string.Empty;
            DITables TablesName;
            string DataPrefix = this.DBConnection.DIDataSetDefault();


            try
            {
                SqlQuery=DevInfo.Lib.DI_LibDAL.Queries.DBMetadata.Update.UpdateRecord(this.DBQueries.TablesName.DBMetadata,NID,
                    DICommon.RemoveQuotes(description),DICommon.RemoveQuotes(publisherName),publisherDate,
                    DICommon.RemoveQuotes(publisherCountry),DICommon.RemoveQuotes(publisherRegion),DICommon.RemoveQuotes(publisherOffice),
                    areaCount,indicatorCount,IUSCount,timeperiodCount,sourceCount,dataCount);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);
              }
            
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }


        #endregion

        #endregion

    }
}
