using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory
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
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.Metadata_Category.CategoryNId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.Metadata_Category.CategoryNId + ")," +
                        DIColumns.Metadata_Category.CategoryName + " varchar(255), " +
                        DIColumns.Metadata_Category.CategoryType + " varchar(2), " +
                        DIColumns.Metadata_Category.CategoryOrder + " int(4))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.Metadata_Category.CategoryNId + "  int Identity(1,1) primary key," +
                        DIColumns.Metadata_Category.CategoryName + " varchar(255), " +
                        DIColumns.Metadata_Category.CategoryType + " varchar(2))," +
                        DIColumns.Metadata_Category.CategoryOrder + " int)";
                }

            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.Metadata_Category.CategoryNId + " counter primary key, " +
                       DIColumns.Metadata_Category.CategoryName + " text(255), " +
                       DIColumns.Metadata_Category.CategoryType + " text(2), " +
                       DIColumns.Metadata_Category.CategoryOrder + " Long)";
            }

            return RetVal;
        }

        /// <summary>
        /// Insert new record Into MatadataCategory table
        /// </summary>
        /// <param name="tableName">e.g. UT_Matadata_Category_en</param>
        /// <param name="metadataName"></param>
        /// <param name="metadataType"></param>
        /// <param name="metadataOrder"></param>
        /// <returns></returns>
        public static string InsertMetadataCategory(string tableName, string metadataName, string metadataType, int metadataOrder)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tableName + " (" + DIColumns.Metadata_Category.CategoryName + "," + DIColumns.Metadata_Category.CategoryType + "," + DIColumns.Metadata_Category.CategoryOrder + ") "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(metadataName) + "','" + DIQueries.RemoveQuotesForSqlQuery(metadataType) + "'," + metadataOrder + ")";

            return RetVal;
        }


        public static string InsertMetadataCategory(string tableName, string categoryName,string categoryGID,string categoryDescription, string categoryType, int categoryOrder,string parentNid,bool isPresentational,bool isMandatory)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tableName + " (" + DIColumns.Metadata_Category.CategoryName + "," + 
              DIColumns.Metadata_Category.CategoryGId +","+
              DIColumns.Metadata_Category.CategoryDescription + "," +
              DIColumns.Metadata_Category.CategoryType + "," +
              DIColumns.Metadata_Category.CategoryOrder +","+
              DIColumns.Metadata_Category.ParentCategoryNId+","+
              DIColumns.Metadata_Category.IsPresentational +","+
              DIColumns.Metadata_Category.IsMandatory+") "
            + " VALUES('" + DIQueries.RemoveQuotesForSqlQuery(categoryName) + "','" +
            DIQueries.RemoveQuotesForSqlQuery(categoryGID) + "','" +
            DIQueries.RemoveQuotesForSqlQuery(categoryDescription)+"','"+
            categoryType.Replace("'","") + "'," + categoryOrder +","+ parentNid +","+
            Convert.ToInt32(isPresentational).ToString()+","+ Convert.ToInt32(isMandatory).ToString() +")";
             
            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert ParentCategoryNId column into MetadataCategory table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertParentCategoryNIdColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.MetadataCategory + " ADD COLUMN  " + DIColumns.Metadata_Category.ParentCategoryNId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " number ";
            }

            return RetVal;
        }

        /// <summary>
        /// CategoryGId
        /// </summary>
        /// <param name="tablesName"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertCategoryGIdColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.MetadataCategory + " ADD COLUMN  " + DIColumns.Metadata_Category.CategoryGId + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(255) ";
                }
                else
                {
                    RetVal += " nvarchar(255) ";
                }
            }
            else
            {
                RetVal += " Text ";
            }

            return RetVal;
        }

        /// <summary>
        /// Add column CategoryDescription
        /// </summary>
        /// <param name="tablesName"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertCategoryDescriptionColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.MetadataCategory + " ADD COLUMN  " + DIColumns.Metadata_Category.CategoryDescription + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(4000) ";
                }
                else
                {
                    RetVal += " nvarchar(4000) ";
                }
            }
            else
            {
                RetVal += " Memo ";
            }

            return RetVal;
        }

        /// <summary>
        /// Add column IsPresentational in MetadataCategory
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIsPresentationalColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.MetadataCategory + " ADD COLUMN  " + DIColumns.Metadata_Category.IsPresentational + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }

        
        /// <summary>
        /// Add column IsMandatory in UT_Metadata_Category
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertIsMandatoryColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + tablesName.MetadataCategory + " ADD COLUMN  " + DIColumns.Metadata_Category.IsMandatory + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }

        #endregion
        #endregion

    }
}
