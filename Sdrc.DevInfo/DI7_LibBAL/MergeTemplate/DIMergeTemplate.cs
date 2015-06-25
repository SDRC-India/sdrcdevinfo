using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Data.OleDb;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.MergeTemplate
{
    public class DIMergeTemplate
    {


        public static bool CreateTempTables(List<string> sourceDatabaseFileWPaths, string targetDatabaseFileWPath)
        {
            bool RetVal = false;
            OleDbConnection Connection = null;
            OleDbCommand Command;
            ImportTableQueries ImportQueries = null;
            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;



            for (int i = 0; i < sourceDatabaseFileWPaths.Count; i++)
            {
                try
                {
                    string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + sourceDatabaseFileWPaths[i] + ";Jet OLEDB:Database Password=" + MergetTemplateConstants.DBPassword + ";Persist Security Info=False;";
                    Connection = new OleDbConnection(ConnectionString);
                    Command = new OleDbCommand();
                    Command.Connection = Connection;
                    Command.CommandTimeout = 0;
                    Connection.Open();


                    SourceDBConnection = new DevInfo.Lib.DI_LibDAL.Connection.DIConnection(DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess, string.Empty, string.Empty, sourceDatabaseFileWPaths[i], String.Empty, MergetTemplateConstants.DBPassword);
                    SourceDBQueries = DevInfo.Lib.DI_LibBAL.Utility.DataExchange.GetDBQueries(SourceDBConnection);

                    ImportQueries = new ImportTableQueries(SourceDBQueries.DataPrefix, SourceDBQueries.LanguageCode, targetDatabaseFileWPath, sourceDatabaseFileWPaths[i]);
                    if (i == 0)
                    {
                        Command.CommandText = ImportQueries.GetTempIndicatorTable();
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.GetTempUnitTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.GetTempSubgroupValsTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.GetTempSubgrouopDimensionsTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = ImportQueries.GetTempSubgroupDimValuesTable();
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.GetTempAreaTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.GetTempICTable());
                        Command.ExecuteNonQuery();
                    }
                    else
                    {
                        Command.CommandText = ImportQueries.InsertIntoIndicatorTable();
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.InsertIntoUnitTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.InsertIntoSubgroupValsTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.InsertIntoSubgrouopDimensionsTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = ImportQueries.InsertIntoSubgroupDimValuesTable();
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.InsertIntoAreaTable());
                        Command.ExecuteNonQuery();
                        Command.CommandText = (ImportQueries.InsertIntoTempICTable());
                        Command.ExecuteNonQuery();
                    }
                    RetVal = true;
                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
                finally
                {
                    if (SourceDBConnection != null)
                    {
                        SourceDBConnection.Dispose();
                        SourceDBQueries = null;
                    }
                    if (ImportQueries != null)
                        ImportQueries = null;

                    if (Connection != null)
                    {
                        Connection.Close();
                        Connection.Dispose();
                    }
                }

            }


            return RetVal;
        }


        public static void DeleteTempTables(DIConnection connection)
        {

            try
            {


                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.Indicator);

                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.Unit);

                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.Subgroup);

                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.SubgroupType);
                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.SubgroupVals);

                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.Area);

                connection.ExecuteNonQuery("DROP TABLE " + MergetTemplateConstants.TempTable.IndicatorClassification);

            }
            catch (Exception)
            { }
           
        }

    }
}
