using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Converter.Database;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting DevInfo database into DevInfo 6_0_0_4 format
    /// </summary>
    public class DI6_0_0_4DBConverter : DI6_0_0_3DBConverter
    {

        #region --  Private --

        #region --  Methods --

        #region -- version --


        private void UpdateDataNIdTypeForRecommendedSourcesTable(bool forOnlineDB)
        {
            
            RecommendedSourcesBuilder RecommendedSourcesTblBuilder;
           

            try
            {
                RecommendedSourcesTblBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);
                RecommendedSourcesTblBuilder.UpdateDataNIdDataType(forOnlineDB);
               
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void RemoveEmptyRecordsFrmRecommendedSources(bool forOnlineDB)
        {           
           RecommendedSourcesBuilder RecommendedSourcesTblBuilder;

            try
            {
                RecommendedSourcesTblBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);
                RecommendedSourcesTblBuilder.DeleteRecordsWhereICIUSIsEmpty();       

            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.ToString());
            }
        }


        private void DeleteDuplicateRecordsFrmDBVersion()
        {
            DBVersionBuilder DBVersion;
            try
            {
                DBVersion = new DBVersionBuilder(this._DBConnection,this._DBQueries );
                DBVersion.DeleteDuplicateRecords();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

        #endregion

        #region -- internal/public --


        #region -- New/Dispose --

        public DI6_0_0_4DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //donothing
        }

        #endregion


        #region -- Methods --
        /// <summary>
        /// Returns true/false. True if Database is in valid format otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                // check 6.0.1.4 version exists in dbVersion table
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.DBVersion.GetRecords(Constants.Versions.DI6_0_0_4)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {                
            }
            
            return RetVal;
        }

        /// <summary>
        /// Converts DevInfo Database into DevInfo6.0.0.4 format
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 4;
            DBVersionBuilder VersionBuilder;

            //do the conversion only if database has different shcema
            try
            {
                if (!this.IsValidDB(forOnlineDB))
                {
                    if (!base.IsValidDB(forOnlineDB))
                    {
                        RetVal = base.DoConversion(forOnlineDB);
                    }

                    // Step1: insert version info into database
                    VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                    VersionBuilder.InsertVersionInfo(Constants.Versions.DI6_0_0_4, Constants.VersionsChangedDates.DI6_0_0_4, Constants.VersionComments.DI6_0_0_4);

                    this.RaiseProcessStartedEvent(TotalSteps);


                    // Step2: remove empty records from recommended sources
                    this.RemoveEmptyRecordsFrmRecommendedSources(forOnlineDB);
                    this.RaiseProcessInfoEvent(1);


                    // Step3: change datatype of Data_NID column to Long Integer under RecommendedSources table  table
                    this.UpdateDataNIdTypeForRecommendedSourcesTable(forOnlineDB);
                    this.RaiseProcessInfoEvent(2);
                    

                    // Step4: remove duplicate records from dbVersion table
                    this.DeleteDuplicateRecordsFrmDBVersion();
                    this.RaiseProcessInfoEvent(3);

                    RetVal = true;

                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #endregion
    }

}
