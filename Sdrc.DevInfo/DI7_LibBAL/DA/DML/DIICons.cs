using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides methods to do selection and manipulation in DIIcon table
    /// </summary>
    public static class DIIcons
    {
        #region"-- Public --"

        #region"-- Variable/Properties --"

        //-- Image reference is stored as __DIIMG__1.gif inside metadata xml 
        public const string IMG_PREFIX = "__DIIMG__";
        private static string sOnline_TablePrefix;
        private static SortedList<IconElementType, string> Elements = new SortedList<IconElementType, string>();

        #endregion

        #region"-- New/Dispose --"

         static DIIcons()
        {
            DIIcons.Elements = DIQueries.IconElementTypeText;
        }
        
        #endregion

        #region"-- Methods --"

        /// <summary>
        /// Returns true if ICON table exists otherwise false.
        /// </summary>
        /// <param name="iconTableName"></param>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static bool IsIconsTblExists(string iconTableName, DIConnection dbConnection)
        {
            bool RetVal = false;
            string sqlString = "Select count(*) from " + iconTableName + " where 1=2";
            try
            {
                if ((dbConnection.ExecuteScalarSqlQuery(sqlString) != null))
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Creates Icon tables for all available language
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="forOnlineDB"></param>
        public static void CreateIconsTblsForAllLngs(DIConnection dbConnection, DIQueries dbQueries, bool forOnlineDB)
        {

            if (DIIcons.IsIconsTblExists(dbQueries.TablesName.Icons, dbConnection) == false)
            {
                try
                {
                    //-- create Icon table 
                    dbConnection.ExecuteNonQuery(DIIcons.CreateIconsTbl(dbQueries.TablesName.Icons, forOnlineDB));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.Message);
                }
            }
        }        

        /// <summary> 
        /// Dictionary containing old IconNId as key and new IconNId as value. 
        /// This information shall be utilised to update metadata xml being inserted / updated 
        /// </summary> 
        /// <param name="NidInSourceDB"></param> 
        /// <param name="NidInTargetDB"></param> 
        /// <param name="elementType"></param> 
        /// <param name="sourceQurey"></param> 
        /// <param name="SourceDBConnection"></param> 
        /// <param name="targetQurey"></param> 
        /// <param name="TargetDBConnection"></param> 
        /// <returns> 
        /// </returns> 
        /// <remarks></remarks> 
        public static Dictionary<string, string> ImportElement(int NidInSourceDB, int NidInTargetDB, IconElementType elementType, DIQueries sourceQurey, DIConnection sourceDBConnection, DIQueries targetQurey, DIConnection targetDBConnection) 
        {             
            Dictionary<string, string> RetVal = new Dictionary<string, string>();

            string ElementValue = DIIcons.Elements[elementType]; 
            string SqlQuery = string.Empty; 
            DataTable IconsDatatable; 
            string OldIconNId = string.Empty; 
            string NewIconNId = string.Empty; 
            try { 
                if ((sourceDBConnection != null)) { 
                    
                    if (DIIcons.IsIconsTblExists(sourceQurey.TablesName.Icons, sourceDBConnection)) { 
                        //-- In Target Database: delete records from UT_Icon table if Icon is already associated with given Element Type 
                        SqlQuery=DevInfo.Lib.DI_LibDAL.Queries.Icon.Delete.DeleteIcon(targetQurey.DataPrefix,ElementValue, NidInTargetDB.ToString()); 
                        targetDBConnection.ExecuteNonQuery(SqlQuery); 
                      
  
                        //-- In Source Database: check Icon is associated with the given Element type in UT_ICon table                         
                        SqlQuery = sourceQurey.Icon.GetIcon(NidInSourceDB.ToString(), ElementValue); 
                        IconsDatatable = sourceDBConnection.ExecuteDataTable(SqlQuery); 
                        
                        //-- If associated, then copy it it from Source database into target database 
                        foreach(DataRow Row in IconsDatatable.Rows)
                        {
                        //-- Insert Icon and get new IconNId 
                            NewIconNId = DIIcons.InsertIcon(targetDBConnection, targetQurey, (byte[])(Row["Element_Icon"]), Row["Icon_Type"].ToString(), Convert.ToInt32(Row["Icon_Dim_W"]), Convert.ToInt32(Row["Icon_Dim_H"]), ElementValue,Convert.ToString( NidInTargetDB)).ToString(); 
                            
                            //-- Add Item to Dictionary with New IconNId as Key and Old IconNId as Value 
                            RetVal.Add(IMG_PREFIX + Row["Icon_NId"].ToString() + ".", IMG_PREFIX + NewIconNId + "."); 
                        } 
                    } 
                } 
            } 
            catch (Exception ex) {
                throw new ApplicationException(ex.Message);
            } 
            return RetVal; 
        }

        /// <summary>
        /// Inserts Icon into ICON table
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="buffer"></param>
        /// <param name="iconType"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="elementType"></param>
        /// <param name="elementNid"></param>
        /// <returns></returns>
        public static int InsertIcon(DIConnection dbConnection, DIQueries dbQueries, byte[] buffer, string iconType, int width, int height, string elementType, string elementNid)
        {
            int RetVal = 0;
            //System.Data.OleDb.OleDbCommand cmd;
            //OleDbParameter prmPic = new OleDbParameter();

            DbCommand Command = dbConnection.GetCurrentDBProvider().CreateCommand(); 
            DbParameter Parameter;

            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Icon.Insert.InsertIcon(dbQueries.DataPrefix, iconType, width, height, elementType, elementNid);
                
                //-- Change for Online Database
                SqlQuery = SqlQuery.Replace("?", "@Element_Icon");

                //SqlQuery = dbQueries.FetchAndUpdateIcons(1, clsIcons.TableName, iconType, width, height, elementType, elementNid);

                // -- Get New NID generated 
                //cmd = new OleDbCommand(SqlQuery, (OleDbConnection)dbConnection.GetConnection);
                //cmd.CommandType = CommandType.Text;

               
                Command.Connection = dbConnection.GetConnection();
                Command.CommandText = SqlQuery;
                Command.CommandType = CommandType.Text;
                Parameter = dbConnection.GetCurrentDBProvider().CreateParameter();
                
                {
                    //prmPic.ParameterName = "@Element_Icon";
                    ////the name used in the query for the parameter 
                    //prmPic.OleDbType = OleDbType.Binary;
                    ////set the database type 
                    //prmPic.Value = buffer;
                    ////assign the contents of the buffer to the value of the parameter 

                    Parameter.ParameterName = "@Element_Icon";
                    //the name used in the query for the parameter 
                    Parameter.DbType= DbType.Binary;

                    //set the database type 
                    Parameter.Value = buffer;
                    //assign the contents of the buffer to the value of the parameter 
                }

                //cmd.Parameters.Add(prmPic);
                Command.Parameters.Add(Parameter);
                //-- add the parameter to the command 
                //cmd.ExecuteNonQuery();
                Command.ExecuteNonQuery();

                //-- this saves the image to the database 
                RetVal =Convert.ToInt32( dbConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                if ((Command != null))
                {
                    Command.Dispose();
                }
            }
            return RetVal;
        }

        /// <summary>
        /// To create ICON Table
        /// </summary>
        /// <param name="iconTableName"></param>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public static string CreateIconsTbl(string iconTableName, bool forOnlineDB)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
                RetVal = "CREATE TABLE " + iconTableName + " (" + Icons.IconNId + " int Identity (1,1) primary key," + Icons.IconType + " varchar(3)," + Icons.IconDimW + " int," + Icons.IconDimH + "   int," + Icons.ElementType + " varchar(2)," + Icons.ElementNId + " int," + Icons.ElementIcon + "   Image)";
            }
            else
            {
                RetVal = "CREATE TABLE " + iconTableName + " (" + Icons.IconNId + " counter primary key," + Icons.IconType + " Text(3)," + Icons.IconDimW + " number," + Icons.IconDimH + " number," + Icons.ElementType + " Text(2)," + Icons.ElementNId + " number," + Icons.ElementIcon + "  OLEObject)";
            }
            return RetVal;
        }

        /// <summary>
        /// Deletes records from ICON table on the basis of element type and element NId.
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="elementNid"></param>
        /// <param name="elementType"></param>
        public static void DeleteIcon(DIConnection dbConnection, string dataPrefix, string elementNIds, IconElementType elementType)
        {
            try
            {
                dbConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Icon.Delete.DeleteIcon(dataPrefix, DIIcons.Elements[elementType].ToString(), elementNIds));

            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.ToString());
            }
			
        }

        public static void ClearIcon(DIConnection dbConnection, string dataPrefix, IconElementType elementType)
        {
            try
            {
                dbConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Icon.Delete.ClearIcon(dataPrefix, DIIcons.Elements[elementType].ToString()));

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