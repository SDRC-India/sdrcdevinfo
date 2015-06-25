using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Icon
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "icons";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Returns query to insert ICON
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="iconType"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="elementType"></param>
        /// <param name="elementNid"></param>
        /// <returns></returns>
    public static string InsertIcon(string dataPrefix, string iconType,  int width,  int height,  string elementType,  string elementNid) 
    {
        string RetVal=string.Empty;

        RetVal= "INSERT INTO " + dataPrefix + Insert.TableName  + " ("+DIColumns.Icons.IconType  +" ,"+DIColumns.Icons.IconDimW+" ,"+DIColumns.Icons.IconDimH+" ,"+DIColumns.Icons.ElementType+" ,"+DIColumns.Icons.ElementNId+" ,"+DIColumns.Icons.ElementIcon +" ) VALUES ('" + iconType + "'," + width + "," + height + "," + elementType + "," + elementNid + ",?)"; 

        return RetVal;
    } 
    
        /// <summary>
        /// Returns query to create Icons table 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
    public static string CreateIconsTbl(string dataPrefix,bool forOnlineDB) 
    {
        string RetVal=string.Empty;

     if(forOnlineDB)
     {
            RetVal= "CREATE TABLE " + dataPrefix+ Insert.TableName  + " (Icon_NId int Identity   (1,1) primary key,Icon_Type varchar(3),Icon_Dim_W int,Icon_Dim_H int,Element_Type varchar(2),Element_NId int,Element_Icon Image)";
     }
     else{
         RetVal = "CREATE TABLE " + dataPrefix + Insert.TableName + " (Icon_NId counter primary key,Icon_Type Text(3),Icon_Dim_W number,Icon_Dim_H number,Element_Type Text(2),Element_NId number,Element_Icon OLEObject)";
     }
      return RetVal;
    }

        #endregion

        #endregion
    }
}