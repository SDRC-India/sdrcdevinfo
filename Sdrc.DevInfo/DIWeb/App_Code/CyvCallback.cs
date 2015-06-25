using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

/// <summary>
/// Summary description for CyvCallback
/// </summary>
public partial class Callback : System.Web.UI.Page
{
    #region Public Functions

    #region Get unmatched list in my data flow
    /// <summary>
    /// Get unmatched list in my data flow
    /// </summary>
    /// <param name="type">aid/aname</param>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <param name="lngCode"></param>
    /// <param name="dbNid"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public string GetUnmatchList(string type, string key, string data, string lngCode, string dbNid, string filePath)
    {
        if (Session["IsMapServer"] != null && Convert.ToBoolean(Session["IsMapServer"]))
        {
            bool usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
            if (usingMapServer)
            {
                lngCode = Global.GetMapServerLangCode(lngCode);
            }
        }
        if (String.IsNullOrEmpty(key))
            return string.Empty;
        DataTable dataTable = new DataTable();
        string retVal = string.Empty;
        int DbNid = int.Parse(dbNid);
        if (type.ToLower() == "csv")
        {
            int columnIndex = int.Parse(data);
            string areaList = getListFromCSV(filePath, columnIndex);
            dataTable = GetDataTable(DbNid, areaList, key.ToLower(), lngCode);
            retVal = filterRecords(dataTable, areaList);
        }
        else if (type.ToLower() == "dataentry")
        {
            dataTable = GetDataTable(DbNid, data, key.ToLower(), lngCode);
            retVal = filterRecords(dataTable, data);
        }


        return retVal;
    }
    #endregion

    #region make Unique item into list
    /// <summary>
    /// Get Unique items
    /// </summary>
    /// <param name="listString">item list sperated by {}</param>
    /// <returns>unique item list sperated by {}</returns>
    public string uniqueListItem(string listString)
    {
        string uniqueList = string.Empty;
        if (!String.IsNullOrEmpty(listString))
        {
            string[] list = listString.Split(new String[] { "{}" }, StringSplitOptions.None);
            System.Collections.ArrayList newList = new System.Collections.ArrayList();
            foreach (string str in list)
                if (!newList.Contains(str))
                    newList.Add(str);
            string[] tempArray = (string[])newList.ToArray(typeof(string));
            foreach (string str in tempArray)
                uniqueList += "{}" + str;
        }
        if (uniqueList.Length > 0)
        {
            uniqueList = uniqueList.Substring(2);
        }
        return uniqueList;
    }
    #endregion

    #region Store mapping information
    /// <summary>
    /// Store mapping information
    /// </summary>
    /// <param name="mappingInfo">string containing unmatched list & mapping ids</param>
    /// <param name="type">which type of list(aid or aname)</param>
    public string storeMappingDetails(string mappingInfo, string type)
    {
        DIConnection dIConnection = null;
        try
        {
            string delemeter1 = "{||}";
            string delemeter2 = "{}";
            string[] mappingAreaInfo = mappingInfo.Split(new string[] { delemeter1 }, StringSplitOptions.None);
            string mapping_nid = string.Empty;
            string areaName = string.Empty;
            DataTable dtKeyword = null;
            string query = string.Empty;
            string UserDefinedKey = string.Empty;
            string MappingKey = string.Empty;

            dIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                               string.Empty, string.Empty);
            bool usingMapServer = false;//Convert.ToBoolean(isMapServer.Trim());
            if (Session["IsMapServer"] != null)
            {
                usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
            }
            foreach (string mappingArea in mappingAreaInfo)
            {
                if (mappingArea.IndexOf(delemeter2) > -1)
                {
                    string[] areaData = mappingArea.Split(new string[] { delemeter2 }, StringSplitOptions.None);
                    UserDefinedKey = MappingKey = "";
                    if (areaData != null)
                    {
                        UserDefinedKey = areaData[0].Trim();
                        MappingKey = areaData[1].Trim();
                    }
                    if (UserDefinedKey.IndexOf("'") > -1)
                        UserDefinedKey = UserDefinedKey.Replace("'", "''");

                    query = "select * from mappinginformation where user_key = '" + UserDefinedKey + "' and mapping_key='" + MappingKey + "' and type ='" + type + "' and mapserver_used ='" + usingMapServer.ToString() + "'";
                    dtKeyword = dIConnection.ExecuteDataTable(query);
                    if (dtKeyword.Rows.Count == 0) // If record doesn't exist into table
                    {
                        if (!string.IsNullOrEmpty(MappingKey))
                        {
                            query = "insert into mappinginformation(user_key,mapping_key,type,item_count,mapserver_used) values('" + UserDefinedKey + "','" + MappingKey + "','" + type + "',0,'" + usingMapServer.ToString() + "')";
                            dIConnection.ExecuteNonQuery(query);
                        }
                    }
                    query = "select * from mappinginformation where user_key = '" + UserDefinedKey + "' and mapping_key='" + MappingKey + "' and type ='" + type + "'";
                    dtKeyword = dIConnection.ExecuteDataTable(query);
                    foreach (DataRow dr in dtKeyword.Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["mapping_key"].ToString()))
                        {
                            int count = Int32.Parse(dr["item_count"].ToString());
                            count++;
                            query = "update mappinginformation set item_count=" + count + " where user_key = '" + UserDefinedKey + "' and mapping_key='" + MappingKey + "' and type ='" + type + "' and mapserver_used ='" + usingMapServer.ToString() + "'";
                            dIConnection.ExecuteNonQuery(query);
                        }
                    }
                }
            }
            dIConnection.Dispose();
        }
        catch (Exception ex)
        {
            dIConnection.Dispose();
            Global.CreateExceptionString(ex, null);


        }
        return string.Empty;
    }
    /// <summary>
    /// Get mapping information
    /// </summary>
    /// <param name="unmatchedList">unmatched list</param>
    /// <param name="type">aid/aname</param>
    /// <param name="dbNid">database nid</param>
    /// <param name="lngCode">language code</param>
    /// <returns>list of unmatched list + matching area id +matching area name</returns>
    public string getMappingDetails(string unmatchedList, string type, string dbNid, string lngCode)
    {
        if (Session["IsMapServer"] != null && Convert.ToBoolean(Session["IsMapServer"]))
        {
            bool usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
            if (usingMapServer)
            {
                lngCode = Global.GetMapServerLangCode(lngCode);
            }
        }
        string result = string.Empty;
        DIConnection dIConnection = null;
        try
        {
            DataTable dtRecords = null;
            DataTable dtAreas = null;
            string query = string.Empty;
            string delemeter = "{}";
            string mapping_Key = string.Empty;
            string user_key = string.Empty;
            string area_name = string.Empty;
            dIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                              string.Empty, string.Empty);
            string[] keys = unmatchedList.Split(new string[] { delemeter }, StringSplitOptions.None);
            dtAreas = getAllAreaName(Int32.Parse(dbNid), lngCode);
            foreach (string key in keys)
            {
                user_key = key;
                mapping_Key = "";
                area_name = "";

                if (!string.IsNullOrEmpty(key))
                {
                    bool usingMapServer = false;//Convert.ToBoolean(isMapServer.Trim());
                    if (Session["IsMapServer"] != null)
                    {
                        usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
                    }
                    if (key.IndexOf("'") > -1)
                    {

                        query = "select * from mappinginformation where user_key = '" + key.Replace("'", "''").Trim() + "' and type = '" + type + "' and mapserver_used ='" + usingMapServer.ToString() + "'";
                    }
                    else
                        query = "select * from mappinginformation where user_key = '" + key.Trim() + "' and type = '" + type + "' and mapserver_used ='" + usingMapServer.ToString() + "'";
                }
                dtRecords = dIConnection.ExecuteDataTable(query);
                if (dtRecords.Rows.Count == 1)
                {
                    if (!string.IsNullOrEmpty(dtRecords.Rows[0]["mapping_key"].ToString()))
                    {
                        mapping_Key = dtRecords.Rows[0]["mapping_key"].ToString().Trim();
                        user_key = dtRecords.Rows[0]["user_key"].ToString().Trim();
                        try
                        {
                            DataRow dr = dtAreas.Select("area_id='" + mapping_Key.Trim() + "'")[0];
                            area_name = dr["area_name"].ToString().Trim();
                        }
                        catch (Exception innerEx)
                        {
                            Global.CreateExceptionString(innerEx, null);

                        }
                    }
                }
                if (dtRecords.Rows.Count > 1)
                {
                    int max = getMaximunValue(dtRecords);
                    if (!string.IsNullOrEmpty(dtRecords.Rows[0]["mapping_key"].ToString()))
                    {
                        foreach (DataRow dr in dtRecords.Rows)
                        {
                            if (Int32.Parse(dr["item_count"].ToString()) == max)
                            {
                                mapping_Key = dr["mapping_key"].ToString().Trim();
                                user_key = dr["user_key"].ToString().Trim();
                                try
                                {
                                    DataRow drArea = dtAreas.Select("area_id='" + mapping_Key.Trim() + "'")[0];
                                    area_name = drArea["area_name"].ToString().Trim();
                                }
                                catch (Exception innerEx)
                                {
                                    Global.CreateExceptionString(innerEx, null);

                                }

                                break;
                            }
                        }
                    }
                }
                result += "{@@}" + user_key + delemeter + mapping_Key + delemeter + area_name;
            }
            dIConnection.Dispose();
            if (result.Length > 0)
                result = result.Substring(4);
        }
        catch (Exception ex)
        {
            dIConnection.Dispose();
            Global.CreateExceptionString(ex, null);

        }
        return result;
    }


    /// <summary>
    /// Get matching area information
    /// </summary>
    /// <param name="unmatchedList">unmatched list</param>
    /// <param name="type">aid/aname</param>
    /// <param name="dbNid">database nid</param>
    /// <param name="lngCode">language code</param>
    /// <returns>list of unmatched list + mapping area id +mapping area name</returns>
    public string getMatchingArea(string unmatchedList, string type, string dbNid, string lngCode)
    {
        if (Session["IsMapServer"] != null && Convert.ToBoolean(Session["IsMapServer"]))
        {
            bool usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
            if (usingMapServer)
            {
                lngCode = Global.GetMapServerLangCode(lngCode);
            }
        }
        string result = string.Empty;
        DIConnection dIConnection = null;
        List<System.Data.Common.DbParameter> DbParams = null;
        System.Data.Common.DbParameter Param1, Param2 = null;
        try
        {
            string query = string.Empty;
            string delemeter = "{}";
            string mapping_Key = string.Empty;
            string user_key = string.Empty;
            string area_name = string.Empty;

            DataTable areaTable = null;


            /*Comment By nishit Garg*/

            /*      dIConnection = Global.GetDbConnection(Int32.Parse(dbNid));
             * The Code stated above in comment has been commented and replaced To use a single instance of
             * DIConnection class.*/

            //   dIConnection = Global.GetDbConnection(Int32.Parse(dbNid));
            /*-------nishit--------*/
            dIConnection = this.GetDbConnection(Convert.ToInt32(dbNid));
            /*--------nishit-------*/


            string[] keys = unmatchedList.Split(new string[] { delemeter }, StringSplitOptions.None);
            foreach (string key in keys)
            {
                user_key = key.Trim();
                if (!string.IsNullOrEmpty(user_key))
                {
                    user_key = user_key.Replace("'", "''");

                    //if (dIConnection != null)
                    //{
                    //    DbParams = new List<System.Data.Common.DbParameter>();
                    //    Param1 = dIConnection.CreateDBParameter();
                    //    Param1.ParameterName = "type";
                    //    Param1.DbType = DbType.String;
                    //    Param1.Value = type.ToLower();
                    //    DbParams.Add(Param1);

                    //    Param2 = dIConnection.CreateDBParameter();
                    //    Param2.ParameterName = "userkey";
                    //    Param2.DbType = DbType.String;
                    //    Param2.Value = user_key;
                    //    DbParams.Add(Param2);
                    //    //Get all data with MRD, Sources & Time period filter. If needed then DataValue filters would be applied over this set of data.
                    //    areaTable = dIConnection.ExecuteDataTable("sp_getAreaListByNidOrName_" + lngCode, CommandType.StoredProcedure, DbParams);
                    //}
                    if (type.ToLower() == "aid")
                        query = "select area_id,area_name from ut_area_" + lngCode + " where (SOUNDEX(area_id) = SOUNDEX('" + user_key + "')) ORDER BY area_name DESC";
                    else
                        query = "select area_id,area_name,(select 1 where area_name='"+user_key+"') as Match  from ut_area_" + lngCode + " where (SOUNDEX(area_name) = SOUNDEX('" + user_key + "')) ORDER BY Match,area_name DESC";
                    areaTable = dIConnection.ExecuteDataTable(query);
                    string matchingArea = string.Empty;
                    if (areaTable != null)
                    {
                        foreach (DataRow dr in areaTable.Rows)
                            matchingArea += "||" + dr[0].ToString().Trim() + delemeter + dr[1].ToString().Trim();
                        if (matchingArea.Length > 0)
                            matchingArea = matchingArea.Substring(2);
                        else
                            matchingArea = "no";
                    }
                    else
                    {
                        matchingArea = "no";
                    }
                    result += "{@@}" + key + "{@@@@}" + matchingArea;
                }
            }
            dIConnection.Dispose();
            if (result.Length > 0)
                result = result.Substring(4);
        }
        catch (Exception ex)
        {
            dIConnection.Dispose();
            Global.CreateExceptionString(ex, null);

        }
        return result;
    }
    #endregion
    #endregion

    #region Private Functions

    private int getMaximunValue(DataTable dtRecords)
    {
        int max = Int32.Parse(dtRecords.Rows[0]["item_count"].ToString());
        foreach (DataRow dr in dtRecords.Rows)
        {
            int count = Int32.Parse(dr["item_count"].ToString());
            if (max < count)
                max = count;
        }
        return max;
    }
    private DataTable getAllAreaName(int dbNid, string langCode)
    {
        DataTable areaTable = null;       


        /*Comment By nishit Garg*/

        /*      DIConnection dIConnection = Global.GetDbConnection(dbNid);
         * The Code stated above in comment has been commented and replaced To use a single instance of
         * DIConnection class.*/


        //  DIConnection dIConnection = Global.GetDbConnection(dbNid);
        /*-------nishit--------*/
        DIConnection dIConnection = this.GetDbConnection(dbNid);
        /*--------nishit-------*/

        areaTable = dIConnection.ExecuteDataTable("sp_GetAllAreaNidName_" + langCode, CommandType.StoredProcedure, null);
        return areaTable;
    }
    /// <summary>
    /// Get DataTable
    /// </summary>
    /// <param name="DBId">Database Id</param>
    /// <param name="arealist">Area Id/Name List</param>
    /// <param name="key">aid/aname</param>
    /// <param name="lngCode">Language code</param>
    /// <returns>Matched records</returns>
    private DataTable GetDataTable(int DBId, string arealist, string key, string lngCode)
    {
        DataTable dtMatchList = new DataTable();
        StringBuilder sbResult = new StringBuilder();

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string AreaList = string.Empty;
        #region Prepare parameters & stored procedure execution.

        try
        {

            if (!String.IsNullOrEmpty(arealist))
            {
                AreaList = SqlParameterFormatString(arealist, "{}");
            }

            if (_DBCon == null || _DBCon.GetConnection().State == System.Data.ConnectionState.Closed)
            {
                _DBCon = this.GetDbConnection(DBId);
            }

            if (_DBCon != null)
            {

                System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
                Param1.ParameterName = "type";
                Param1.DbType = DbType.String;
                Param1.Value = key;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = _DBCon.CreateDBParameter();
                Param2.ParameterName = "arealist";
                Param2.DbType = DbType.String;
                Param2.Value = AreaList;
                DbParams.Add(Param2);

                //Get all data with MRD, Sources & Time period filter. If needed then DataValue filters would be applied over this set of data.
                dtMatchList = _DBCon.ExecuteDataTable("sp_get_matcharealist_" + lngCode, CommandType.StoredProcedure, DbParams);

                return dtMatchList;

            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            return dtMatchList;

        }

        finally
        {
            //if (_DBCon != null)
            //{
            //    _DBCon.Dispose();
            //}

        }
        #endregion
        return dtMatchList;
    }

    /// <summary>
    /// Filter records with matched records
    /// </summary>
    /// <param name="dt">Matched Datatable</param>
    /// <param name="list">List</param>
    /// <returns>Filterd records</returns>
    private string filterRecords(DataTable dt, string list)
    {
        string result = string.Empty;
        int RecordCount = 0;
        if (dt.Rows.Count > 0)
        {
            string[] allList = list.Split(new string[] { "{}" }, StringSplitOptions.None);
            foreach (string str in allList)
            {
                if (!String.IsNullOrEmpty(str))
                {
                    string tempStr1 = str.Trim().ToLower();
                    bool isFound = false;
                    RecordCount = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        string tempStr2 = row[0].ToString().Trim().ToLower();
                        if (tempStr1.Equals(tempStr2))
                        {
                            isFound = true;
                            RecordCount++;
                            if (RecordCount >= 2)
                            {
                                break;
                            }
                        }
                    }
                    if (!isFound || RecordCount >= 2)
                    {
                        result += str + "{}";
                    }
                }
            }
            if (!String.IsNullOrEmpty(result))
            {
                result = result.Substring(0, result.Length - 2);
            }
        }
        else
        {
            result = list;
        }
        result = uniqueListItem(result);
        return result;
    }


    /// <summary>
    /// Prepair parameter string for procedure
    /// </summary>
    /// <param name="unformatedString">Unformated string</param>
    /// <param name="delemeter">delemeter in unformated string</param>
    /// <returns>Formated string</returns>
    private string SqlParameterFormatString(string unformatedString, string delemeter)
    {
        string formatedString = string.Empty;
        string[] strList = null;
        strList = unformatedString.Split(new String[] { delemeter }, StringSplitOptions.None);
        foreach (string str in strList)
        {
            if (!String.IsNullOrEmpty(str))
            {
                formatedString += Constants.Delimiters.SingleQuote + str.Replace("'", "''") + Constants.Delimiters.SingleQuote + Constants.Delimiters.Comma;
            }
        }
        if (!String.IsNullOrEmpty(formatedString))
        {
            formatedString = formatedString.Substring(0, formatedString.Length - 1);
        }
        return formatedString;
    }


    private string getListFromCSV(string filePath, int columnIndex)
    {
        string list = string.Empty;
        StreamReader CsvFile = new StreamReader(filePath, Encoding.UTF7);
        int count = -1;
        #region Convert CSV to DataTable
        string[] columns = null;
        List<string> wordList = new List<string>();
        while (CsvFile.Peek() != -1)
        {
            string line = CsvFile.ReadLine();
            count++;
            if (count != 0)
            {
                if (line.IndexOf('"') > -1)
                {
                    wordList = getDoubleQuoteString(line);
                    foreach (string word in wordList)
                    {
                        string temp = word.Replace(Constants.Delimiters.Comma, "{}");
                        line = line.Replace('"' + word + '"', temp);
                    }
                }
                columns = line.Split(new string[] { Constants.Delimiters.Comma }, StringSplitOptions.None);
                list += columns[columnIndex].Replace("{}", Constants.Delimiters.Comma) + "{}";
            }
        }
        if (!String.IsNullOrEmpty(list))
        {
            list = list.Substring(0, list.Length - 2);
        }
        #endregion

        CsvFile.Close();
        CsvFile.Dispose();
        return list;
    }
    #endregion
}
