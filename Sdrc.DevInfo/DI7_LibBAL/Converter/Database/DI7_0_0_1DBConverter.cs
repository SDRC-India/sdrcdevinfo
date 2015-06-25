using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    public class DI7_0_0_1DBConverter : DI7_0_0_0DBConverter
    {

        #region "-- private --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        private bool UpdateDBSchema(bool forOnlineDB)
        {
            bool RetVal = false;
            DITables TablesName = this.DBQueries.TablesName;
            string dataType = string.Empty;

            try
            {
                //1) For timeperiod update column periodicity column datatype from Number to Text.
                if (forOnlineDB)
                    dataType = "nvarchar(255)";
                else
                    dataType = "Text(50)";
                this.AlterColumn(Timeperiods.Periodicity, TablesName.TimePeriod, dataType);
                this.RaiseProcessInfoEvent(1);

                ////2) For Data table update column data_value column datatype from Text to Number.
                //dataType = "int";
                //this.AlterColumn(Data.DataValue, TablesName.Data, dataType);
                this.RaiseProcessInfoEvent(2);

                //3) Check for table Indicator_Classifications_IUS then update name as ic_ius 
                this.RenameIndicatorClassificationIUSTable(forOnlineDB, TablesName, this._DBConnection.ConnectionStringParameters.ServerType);
                this.RaiseProcessInfoEvent(3);

                RetVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private void RenameIndicatorClassificationIUSTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (!this.ISColumnExist(" top 1 * ", this._DBQueries.DataPrefix + "ic_ius"))
                {
                    //-- Carate New IC_IUS table from existing table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.CreateNewICIUSTableFromExisting(this._DBQueries.DataPrefix));

                    //-- Delete old table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteOldICIUSTable(this._DBQueries.DataPrefix));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool AlterColumn(string columnName, string tableName,string dataType)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;

            SqlQuery = "ALTER TABLE " + tableName + " ALTER COLUMN " + columnName + " " + dataType;

            try
            {
                this._DBConnection.ExecuteDataTable(SqlQuery);

                RetVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private bool ISColumnExist(string columnName, string tableName)
        {
            bool RetVal = true;
            string SqlQuery = string.Empty;

            SqlQuery = "SELECT " + columnName + " FROM " + tableName;

            try
            {
                this._DBConnection.ExecuteDataTable(SqlQuery);
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

        #region "-- new/dispose --"

        public DI7_0_0_1DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //do nothing
        }

        #endregion

        #region "-- Methods --"

        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                // check DI7_0_0_1 version exists in dbVersion table
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.DBVersion.GetRecords(Constants.Versions.DI7_0_0_1)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
            }

            if (!RetVal)
            {
                // set file post fix if given file is not in latest format
                this._DBFilePostfix = Constants.DBFilePostFix.DI7_0_0_0;
            }

            return RetVal;
        }

        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 6;
            DBVersionBuilder VersionBuilder;

            // Do the conversion only if database has different Schema
            try
            {
                if (!this.IsValidDB(forOnlineDB))
                {
                    this._DBQueries = new DIQueries(this._DBQueries.DataPrefix, this._DBQueries.LanguageCode);

                    if (!base.IsValidDB(forOnlineDB))
                    {
                        RetVal = base.DoConversion(forOnlineDB);
                    }

                    if (this._ConvertDatabase)
                    {
                        this.RaiseProcessStartedEvent(TotalSteps);

                        this.RaiseProcessInfoEvent(0);

                        if (this.UpdateDBSchema(forOnlineDB))
                        {
                            this.RaiseProcessInfoEvent(4);

                            // Insert version info into database after conversion
                            VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                            VersionBuilder.InsertVersionInfo(Constants.Versions.DI7_0_0_1, Constants.VersionsChangedDates.DI7_0_0_1, Constants.VersionComments.DI7_0_0_1);

                            this.RaiseProcessInfoEvent(5);

                            RetVal = true;
                        }
                    }
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
