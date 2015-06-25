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
    /// Provides method for inseting, deleting and updating records in RecommendedSources table
    /// </summary>
    public class RecommendedSourcesBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        DIConnection DBConnection;
        DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// To check existance of RecommendedSource DataNid into database
        /// </summary>
        /// <param name="dataNid"></param>
        /// <returns>Record Count</returns>
        private int CheckDataNidExistsInRecommendedSource(int dataNid)
        {
            int RetVal = 0;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.RecommendedSources.GetRecommendedSources(dataNid.ToString())).Rows.Count;
            }
            catch { }
            return RetVal;
        }

        /// <summary>
        /// Insert RecommendedSource record into database
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="icIUSLabel"></param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        public bool InsertIntoDatabase(int dataNId, string ICIUSLabel)
        {
            bool RetVal = false;
            DITables TablesName;
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string ICIUSLabelForDatabase = string.Empty;
            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                // insert IC_IUS_label in all language tables
                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        ICIUSLabelForDatabase = ICIUSLabel;
                    }
                    else
                    {
                        ICIUSLabelForDatabase = Constants.PrefixForNewValue + ICIUSLabel;
                    }

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Insert.InsertRecommendedSource(TablesName.RecommendedSources, dataNId, ICIUSLabelForDatabase));
                }


                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }

        

        #endregion

        #endregion

        #region "-- public --"

        #region "-- New/Dispose --"

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public RecommendedSourcesBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns true if RecommendedSources table exists otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsRecommendedSourcesTableExists()
        {
            bool RetVal = false;

            try
            {
                //-- Check the existence of the table 
                if (this.DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this.DBQueries.TablesName.RecommendedSources + " WHERE 1=1") != null)
                    RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        public DataTable GetAllRecordsFromRecommendedSources()
        {
            DataTable RetVal = null;

            try
            {
                //-- Get Recommended Sources table 
                RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.RecommendedSources.GetRecommendedSources(string.Empty));

            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Check existance of DataNID record into database if false then insert RecommendedSource else Update
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="ICIUSOrder"></param>    
        /// <param name="ICIUSLabel"></param>
        public int CheckNInsertRecommendedSource(int dataNId, string ICIUSorder, string ICIUSLabel)
        {
            int RetVal = 0;

            try
            {
                // Step1: update/insert IC_IUS_Label
                if (!string.IsNullOrEmpty(ICIUSLabel))
                {
                    // check DataNid exists or not
                    RetVal = this.CheckDataNidExistsInRecommendedSource(dataNId);

                    // if DataNid does not exist then create it.
                    if (RetVal <= 0)
                    {
                        // insert Recommended Source
                        if (this.InsertIntoDatabase(dataNId, ICIUSLabel))
                        {
                            RetVal = this.DBConnection.GetNewId();
                        }

                    }
                    else
                    {
                        this.UpdateRecommendedSources(dataNId, ICIUSLabel);
                    }
                }


                // Step2: update IC_IUS_order in UT_Data table
                this.UpdateICIUSOrderInDataTable(dataNId, ICIUSorder);

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Update Recommended Source Label By DataNid
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="icIUSLabel"></param>
        /// <returns></returns>
        public bool UpdateRecommendedSources(int dataNId, string icIUSLabel)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;
            DITables tables = new DITables(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode);
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Update.UpdateRecommendedSourceValue(tables.RecommendedSources, dataNId, DICommon.RemoveQuotes(icIUSLabel));

                this.DBConnection.ExecuteNonQuery(SqlQuery);
                RetVal = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Deletes rows from RecommendSources table for the given dataNIds
        /// </summary>
        /// <param name="dataNIds"></param>
        public void DeleteRecommendedSources(string dataNIds)
        {
            DITables TableNames;


            try
            {
                // Step1: Delete records from RecommendedSources table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get table name
                    TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                    
                        // delete records
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Delete.DeleteRecommendedSources(TableNames.RecommendedSources, dataNIds));
                   
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        /// <summary>
        /// Deletes rows from RecommendSources table where ICIUS Label is empty
        /// </summary>
        public void DeleteRecordsWhereICIUSIsEmpty()
        {
            DITables TableNames;

            try
            {
                // Step1: Delete records from RecommendedSources table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get table name
                    TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                    // delete records
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Delete.DeleteRecordsWhereICIUSIsEmpty(TableNames.RecommendedSources, TableNames.Data));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        /// <summary>
        /// Updates records in Data table
        /// </summary>
        /// <param name="dataNId"></param>
        /// <param name="ICIUSOrder"></param>
        public void UpdateICIUSOrderInDataTable(int dataNId, string ICIUSOrder)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateICIUSOrder(this.DBQueries.DataPrefix, dataNId, ICIUSOrder));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }



        /// <summary>
        /// Deletes extra rows from RecommendSources table 
        /// </summary>
        public void DeleteExtraRowsFrmRecommendedSources()
        {
            DITables TableNames;
            
            try
            {
                // Step1: Delete records from RecommendedSources table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get table name
                    TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                    // delete records
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Delete.DeleteExtraRecords(TableNames.RecommendedSources, TableNames.Data));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        


        public void UpdateDataNIdDataType(bool forOnilneDB)
        {            
            DITables TableNames;

            try
            {
                // Step1: update data type of data_nid column 
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get table name
                    TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                    // update datatype
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Update.UpdateTypeofDataNIdColumn(TableNames.RecommendedSources, forOnilneDB, this.DBConnection.ConnectionStringParameters.ServerType));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

    }
}
