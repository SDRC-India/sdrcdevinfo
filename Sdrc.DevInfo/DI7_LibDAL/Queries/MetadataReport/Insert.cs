using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataReport
{
    public class Insert
    {

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns a query to create Metadata_Category table
        /// </summary>
        /// <param name="tableName"><DataPrefix>_Metadata_Category_<LanguageCode></param> 
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateTable(string tableName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;


            if (forOnlineDB)
            {

                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.MetadataReport.MetadataReportNid + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.MetadataReport.MetadataReportNid + ")," +
                        DIColumns.MetadataReport.TargetNid + " int(4), " +
                        DIColumns.MetadataReport.CategoryNid + " int(4), " +
                        DIColumns.MetadataReport.Metadata + " longtext )";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.MetadataReport.MetadataReportNid + "  int Identity(1,1) primary key," +
                        DIColumns.MetadataReport.TargetNid + " int , " +
                        DIColumns.MetadataReport.CategoryNid + " int ," +
                        DIColumns.MetadataReport.Metadata + " ntext )";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.MetadataReport.MetadataReportNid + " counter primary key, " +
                       DIColumns.MetadataReport.TargetNid + " long , " +
                       DIColumns.MetadataReport.CategoryNid + " long , " +
                       DIColumns.MetadataReport.Metadata + " Memo)";
            }

            return RetVal;
        }

        /// <summary>
        /// Insert new record Into MatadataCategory table
        /// </summary>
        /// <param name="tableName">e.g. UT_Matadata_Category_en</param>
        /// <param name="targetNid"></param>
        /// <param name="categoryNId"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        public static string InsertMetadataReport(string tableName, string targetNid, string categoryNId, string metadata)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tableName + " (" + DIColumns.MetadataReport.TargetNid + "," + DIColumns.MetadataReport.CategoryNid + "," + DIColumns.MetadataReport.Metadata + ") "
            + " VALUES(" + targetNid + "," + categoryNId + ",'" + DIQueries.RemoveQuotesForSqlQuery(metadata) + "')";

            return RetVal;
        }

    
        
        #endregion
        #endregion

    }
}
