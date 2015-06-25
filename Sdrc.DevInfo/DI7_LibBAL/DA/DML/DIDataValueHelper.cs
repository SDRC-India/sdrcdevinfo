using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using ADOX;
using System.Data;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{

    public static class DIDataValueHelper
    {
        
        #region "-- private --"
              
        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        private static void RemoveDataValueAndRenameOrgColumns(DIConnection dbConnection, DIQueries dbQueries)
        {
            string SqlQuery = string.Empty;
            //2. Remove Data_Value  column and rename orgTextual_Data_Value and orgData_Value to Textual_Data_Value and Data_value"
            SqlQuery = "ALTER TABLE " + dbQueries.TablesName.Data + " DROP COLUMN " + Data.DataValue;
            dbConnection.ExecuteNonQuery(SqlQuery);

            DIDataValueHelper.RenameOriganalDataTableColumnInDatabase(dbConnection, dbQueries, Constants.Data.Orginal_Textual_Data_valueColumn, Data.TextualDataValue, " Memo");
            DIDataValueHelper.RenameOriganalDataTableColumnInDatabase(dbConnection, dbQueries, Constants.Data.Orginal_Data_valueColumn, Data.DataValue, " Double ");

        }

        private static bool ISColumnExists(DIConnection dbConnection, string columnName, string tableName)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;
            //-- Check orgTextual_Data_value exists or not. 
            try
            {
                SqlQuery = "Select " + columnName + " FROM " + tableName;
                DataTable Table = dbConnection.ExecuteDataTable(SqlQuery);
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }



        /// <summary>
        /// Rename Textual_Data_Value & Data_value TO orgTextual_Data_value & orgData_Value respectively
        /// </summary>
        /// <returns></returns>
        private static bool RenameOriganalDataTableColumnInDatabase(DIConnection dbConnection, DIQueries dbQueries, string oldColumn, string newColumn, string newColumnDataType)
        {
            bool RetVal = true;
            string SqlQuery = string.Empty;
            try
            {
                //CatalogClass CatalogClassObj = new CatalogClass();
                //-- cat.ActiveConnection = connString    <-- Error here -- Cannot assign to cat.ActiveConnection directly.// Use code at below to assign connection string in C#.
                //CatalogClassObj.let_ActiveConnection(dbConnection.ConnectionStringParameters.GetConnectionString());
                //CatalogClassObj.Tables[dbQueries.TablesName.Data].Columns[oldColumn].Name = newColumn;
                //CatalogClassObj = null;

                //-- RENAME COLUMN
                SqlQuery = "ALTER TABLE " + dbQueries.TablesName.Data + " ADD COLUMN " + newColumn + " " + newColumnDataType;
                dbConnection.ExecuteNonQuery(SqlQuery);

                SqlQuery = "UPDATE " + dbQueries.TablesName.Data + " SET " + newColumn + "=" + oldColumn;
                dbConnection.ExecuteNonQuery(SqlQuery);

                SqlQuery = "ALTER TABLE " + dbQueries.TablesName.Data + " DROP COLUMN " + oldColumn;
                dbConnection.ExecuteNonQuery(SqlQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }





        #endregion

        #endregion

        #region "-- public --"
               
        #region "-- Variables --"

        #endregion
               
        #region "-- Methods --"

        /// <summary>
        /// Rename Data_value and TextualValue column into OrgData_value and OrgTextualValue 
        /// and merge both column value into new column Data_Value
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public static void MergeTextualandNumericDataValueColumn(string databaseFileNameWPath)
        {
            try
            {
                using (DIConnection DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, databaseFileNameWPath, string.Empty, string.Empty))
                {
                    DIQueries DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

                    MergeTextualandNumericDataValueColumn(DBConnection, DBQueries);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// Rename Data_value and TextualValue column into OrgData_value and OrgTextualValue 
        /// and merge both column value into new column Data_Value
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public static void MergeTextualandNumericDataValueColumn(DIConnection dbConnection, DIQueries dbQueries)
        {
            try
            {
                if (!ISColumnExists(dbConnection, Constants.Data.Orginal_Data_valueColumn, dbQueries.TablesName.Data))
                {

                    //1. Rename Textual_Data_Value & Data_value  to orgTextual_Data_value & orgData_Value respectively           
                    RenameOriganalDataTableColumnInDatabase(dbConnection, dbQueries, Data.TextualDataValue, Constants.Data.Orginal_Textual_Data_valueColumn, " Memo");
                    RenameOriganalDataTableColumnInDatabase(dbConnection, dbQueries, Data.DataValue, Constants.Data.Orginal_Data_valueColumn, " Double ");
                    System.Threading.Thread.Sleep(100);
                    //2. Create new column Data_Value of memo data type
                    string SqlQuery = "ALTER Table " + dbQueries.TablesName.Data + " Add Column " + Data.DataValue + " Memo NULL";
                    dbConnection.ExecuteNonQuery(SqlQuery);
                    System.Threading.Thread.Sleep(10);
                    //3. Merge all data values into Data_Value column
                    SqlQuery = "UPDATE " + dbQueries.TablesName.Data + " SET " + Data.DataValue + "=" + Constants.Data.Orginal_Data_valueColumn;
                    dbConnection.ExecuteNonQuery(SqlQuery);

                    SqlQuery = "UPDATE " + dbQueries.TablesName.Data + " SET " + Data.DataValue + "=" + Constants.Data.Orginal_Textual_Data_valueColumn + " WHERE " + Data.IsTextualData + "=" + true;
                    dbConnection.ExecuteNonQuery(SqlQuery);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //-- Check orgTextual_Data_value exists or not. If column exists then move textual & numeric values into their respective column.
        public static void SeparateTextualandNemericData(DIConnection dbConnection, DIQueries dbQueries)
        {
            string SqlQuery = string.Empty;
            try
            {
                if (ISColumnExists(dbConnection, Constants.Data.Orginal_Data_valueColumn, dbQueries.TablesName.Data))
                {

                    //--Update Textual value true if data value is not Numeric
                    SqlQuery = "UPDATE " + dbQueries.TablesName.Data + " SET " + Data.IsTextualData + " = 1 WHERE NOT ISNUMERIC(" + Data.DataValue + ")";
                    dbConnection.ExecuteNonQuery(SqlQuery);

                    //-- move orgData_Value values into Data_Value
                    SqlQuery = "UPDATE " + dbQueries.TablesName.Data + "	Set " + Constants.Data.Orginal_Data_valueColumn + "=" + Data.DataValue + " WHERE " + Data.IsTextualData + " = 0";
                    dbConnection.ExecuteNonQuery(SqlQuery);

                    //--move orgTextual_Data_value into Textual_Data_value if ISTextual is true      
                    SqlQuery = "UPDATE " + dbQueries.TablesName.Data + " Set " + Constants.Data.Orginal_Textual_Data_valueColumn + " = " + Data.DataValue + " WHERE " + Data.IsTextualData + "<>0";
                    dbConnection.ExecuteNonQuery(SqlQuery);

                    //2. Remove Data_Value  column and rename orgTextual_Data_Value and orgData_Value to Textual_Data_Value and Data_value"
                    DIDataValueHelper.RemoveDataValueAndRenameOrgColumns(dbConnection, dbQueries);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        #endregion

        #endregion
		     


    }
}
