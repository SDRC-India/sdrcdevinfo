using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;

/// <summary>
/// Summary description for MenuCategory
/// </summary>
public class MenuCategories
{
    public string MenuCategory { get; set; }
    public string LinkText { get; set; }
    public string HeaderText { get; set; }
    public string HeaderDesc { get; set; }
    public string IsVisible { get; set; }
    public string PageName { get; set; }

    public bool DeleteMenuCategory(string CategoryName)
    {
        bool RetVal = false;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        int DeleteMenuCate = -1;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@MenuCategory";
                Param1.DbType = DbType.String;
                Param1.Value = CategoryName;
                DbParams.Add(Param1);
                // Execute stored procedure to delete menu category from database

                DeleteMenuCate = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_DeleteMenuCategory", CommandType.StoredProcedure, DbParams));
                if (DeleteMenuCate >= 0)
                {
                    RetVal = true;
                }

            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;


    }

    public bool AddMenuCategory(string CategoryName, string LinkText, string HeaderText, string HeaderDesc,string PageName)
    {
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        bool RetVal = false;
        int AddMenuCategory = -1;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@MenuCategory";
                Param1.DbType = DbType.String;
                Param1.Value = CategoryName;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param2.ParameterName = "@LinkText";
                Param2.DbType = DbType.String;
                Param2.Value = LinkText;
                DbParams.Add(Param2);
                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param3.ParameterName = "@HeaderText";
                Param3.DbType = DbType.String;
                Param3.Value = HeaderText;
                DbParams.Add(Param3);
                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param4.ParameterName = "@HeaderDesc";
                Param4.DbType = DbType.String;
                Param4.Value = HeaderDesc;
                DbParams.Add(Param4);

                System.Data.Common.DbParameter Param5 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param5.ParameterName = "@IsVisible";
                Param5.DbType = DbType.String;
                Param5.Value = true;
                DbParams.Add(Param5);

                System.Data.Common.DbParameter Param6 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param6.ParameterName = "@PageName";
                Param6.DbType = DbType.String;
                Param6.Value = PageName;
                DbParams.Add(Param6);

                // Execute stored procedure to add menu category from database
                AddMenuCategory = Convert.ToInt32(ObjDIConnection.ExecuteNonQuery("sp_AddMenuCategory", CommandType.StoredProcedure, DbParams));
                // Check if return datatable is not null and having atleast 1 record
                if (AddMenuCategory > 0)
                {
                    RetVal = true;
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;


    }

    public bool EditMenuCategory(string CategoryName, string LinkText, string HeaderText, string HeaderDesc, string PageName)
    {
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        bool RetVal = false;
        int AddMenuCategory = -1;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create Category name parameter
                Param1.ParameterName = "@MenuCategory";
                Param1.DbType = DbType.String;
                Param1.Value = CategoryName;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();// create Link Test parameter
                Param2.ParameterName = "@LinkText";
                Param2.DbType = DbType.String;
                Param2.Value = LinkText;
                DbParams.Add(Param2);
                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();// create Header Text parameter
                Param3.ParameterName = "@HeaderText";
                Param3.DbType = DbType.String;
                Param3.Value = HeaderText;
                DbParams.Add(Param3);
                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter();// create Header Description parameter
                Param4.ParameterName = "@HeaderDesc";
                Param4.DbType = DbType.String;
                Param4.Value = HeaderDesc;
                DbParams.Add(Param4);
                System.Data.Common.DbParameter Param5 = ObjDIConnection.CreateDBParameter();
                Param5.ParameterName = "@IsVisible";
                Param5.DbType = DbType.String;
                Param5.Value = true;
                DbParams.Add(Param5);
                System.Data.Common.DbParameter Param6 = ObjDIConnection.CreateDBParameter();
                Param6.ParameterName = "@PageName";
                Param6.DbType = DbType.String;
                Param6.Value = PageName;
                DbParams.Add(Param6);
                // Execute stored procedure to add menu category from database
                AddMenuCategory = Convert.ToInt32(ObjDIConnection.ExecuteNonQuery("sp_EditMenuCategory", CommandType.StoredProcedure, DbParams));
                if (AddMenuCategory > 0)
                {
                    RetVal = true;
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;


    }

    public string GetMenuCategoriesListJson(bool IsCategoryVisible,string PageName)
    {
        System.Web.Script.Serialization.JavaScriptSerializer jSearializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<MenuCategories> RetMenuCat = null;
        string RetVal = string.Empty;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        DataTable DtRetData = null;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@IsVisible";
                Param1.DbType = DbType.Int32;
                Param1.Value = IsCategoryVisible;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param2.ParameterName = "@PageName";
                Param2.DbType = DbType.String;
                Param2.Value = PageName;
                DbParams.Add(Param2);
                // Execute stored procedure to get Tags From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetMenuCategoriesByPageName", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    RetMenuCat = new List<MenuCategories>();
                    // Itterate through loop
                    for (int Icount = 0; Icount < DtRetData.Rows.Count; Icount++)
                    {
                        MenuCategories ObjMenuCat = new MenuCategories();
                        ObjMenuCat.MenuCategory = DtRetData.Rows[Icount]["MenuCategory"].ToString();
                        ObjMenuCat.LinkText = DtRetData.Rows[Icount]["LinkText"].ToString();
                        ObjMenuCat.HeaderText = DtRetData.Rows[Icount]["HeaderText"].ToString();
                        ObjMenuCat.HeaderDesc = DtRetData.Rows[Icount]["HeaderDesc"].ToString();
                        ObjMenuCat.PageName = DtRetData.Rows[Icount]["PageName"].ToString();
                        // assign tag value to list RetVal
                        RetMenuCat.Add(ObjMenuCat);
                    }
                }
                RetVal = jSearializer.Serialize(RetMenuCat);
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;


    }

    public List<MenuCategories> GetMenuCategoriesList(bool IsCategoryVisible)
    {
        List<MenuCategories> RetMenuCat = null;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        DataTable DtRetData = null;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetMenuCat; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@IsVisible";
                Param1.DbType = DbType.Int32;
                Param1.Value = IsCategoryVisible;
                DbParams.Add(Param1);
                // Execute stored procedure to get Tags From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetMenuCategories", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    RetMenuCat = new List<MenuCategories>();
                    // Itterate through loop
                    for (int Icount = 0; Icount < DtRetData.Rows.Count; Icount++)
                    {
                        MenuCategories ObjMenuCat = new MenuCategories();
                        ObjMenuCat.MenuCategory = DtRetData.Rows[Icount]["MenuCategory"].ToString();
                        ObjMenuCat.LinkText = DtRetData.Rows[Icount]["LinkText"].ToString();
                        ObjMenuCat.HeaderText = DtRetData.Rows[Icount]["HeaderText"].ToString();
                        ObjMenuCat.HeaderDesc = DtRetData.Rows[Icount]["HeaderDesc"].ToString();
                        // assign tag value to list RetVal
                        RetMenuCat.Add(ObjMenuCat);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            RetMenuCat = null;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetMenuCat;


    }

    public bool MoveUpNDownMenuCat(string MenuCategory, bool MoveUp, bool MoveDown,string PageName)
    {
        bool RetVal = false;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        int MenuCateMoved = -1;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                DbParams = new List<System.Data.Common.DbParameter>();

                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@MenuCategory";
                Param1.DbType = DbType.String;
                Param1.Value = MenuCategory;
                DbParams.Add(Param1);
                // create database parameters
                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param2.ParameterName = "@MoveUp";
                Param2.DbType = DbType.Int16;
                Param2.Value = MoveUp;
                DbParams.Add(Param2);
                // create database parameters
                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param3.ParameterName = "@MoveDown";
                Param3.DbType = DbType.Int16;
                Param3.Value = MoveDown;
                DbParams.Add(Param3);

                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param4.ParameterName = "@PageName";
                Param4.DbType = DbType.String;
                Param4.Value = PageName;
                DbParams.Add(Param4);
                // Execute stored procedure to delete menu category from database

                MenuCateMoved = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_MoveUpNDownMenuCat", CommandType.StoredProcedure, DbParams));
                if (MenuCateMoved >= 0)
                {
                    RetVal = true;
                }

            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;
    }
}