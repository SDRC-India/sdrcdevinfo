using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /////// <summary>
    /////// Build subgroup record and insert it into database.
    /////// </summary>
    ////public class SubgroupBuilder
    ////{
    ////    #region "-- Private --"

    ////    #region "-- Variables --"

    ////    private DIConnection DBConnection;
    ////    private DIQueries DBQueries;
        
    ////    #endregion

    ////    #region "-- Methods --"
    ////    /// <summary>
    ////    /// Check existance of Subgroup first in collection Then In database
    ////    /// </summary>
    ////    /// <param name="subgroupInfo">object of UnitInfo</param>
    ////    /// <returns> Nid</returns>
    ////    private int CheckSubgroupExists(SubgroupInfo subgroupInfo)
    ////    {
    ////        int RetVal = 0;

    ////        //Step 1: check subgroup exists in Unit collection
    ////        RetVal = this.CheckSubgroupInCollection(subgroupInfo.Name);

    ////        //Step 2: check unit exists in database.
    ////        if (RetVal <= 0)
    ////        {
    ////            RetVal = this.GetSubgroupNid(subgroupInfo.GID, subgroupInfo.Name);
    ////        }

    ////        return RetVal;
    ////    }
    ////    /// <summary>
    ////    /// Add subgroup record into collection
    ////    /// </summary>
    ////    /// <param name="subgroupInfo">object of subgroupInfo</param>

    ////    private void AddSubgroupIntoCollection(SubgroupInfo subgroupInfo)
    ////    {
    ////        if (!this.SubgroupCollection.ContainsKey(subgroupInfo.Name))
    ////        {
    ////            this.SubgroupCollection.Add(subgroupInfo.Name, subgroupInfo);
    ////        }

    ////    }
    ////    /// <summary>
    ////    /// Check subgroup in collection
    ////    /// </summary>
    ////    /// <param name="subgroupName"> Name</param>
    ////    /// <returns>Nid</returns>
    ////    private int CheckSubgroupInCollection(string subgroupName)
    ////    {
    ////        int RetVal = 0;
    ////        try
    ////        {
    ////            if (this.SubgroupCollection.ContainsKey(subgroupName))
    ////            {
    ////                RetVal = this.SubgroupCollection[subgroupName].Nid;
    ////            }
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = 0;
    ////        }
    ////        return RetVal;
    ////    }


    ////    /// <summary>
    ////    /// Get subgroup Nid by  Name
    ////    /// </summary>
    ////    /// <returns> Nid</returns>
    ////    private int GetSubgroupByName(string name)
    ////    {
    ////        int RetVal = 0;
    ////        string SqlQuery = string.Empty;
    ////        try
    ////        {
    ////            SqlQuery = this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.Name, "'" + name + "'");
    ////            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = 0;
    ////        }
    ////        return RetVal;
    ////    }

    ////    /// <summary>
    ////    /// Get subgroup Nid from subgroup by  GId.
    ////    /// </summary>
    ////    /// <returns></returns>
    ////    private int GetNidByGID(string GID)
    ////    {
    ////        string SqlQuery = string.Empty;
    ////        int RetVal = 0;
    ////        try
    ////        {
    ////            SqlQuery = this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.GId, "'" + GID + "'");
    ////            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = 0;
    ////        }
    ////        return RetVal;
    ////    }

    ////    /// <summary>
    ////    /// Insert  record into subgroup table
    ////    /// </summary>
    ////    /// <param name="subgroupInfo">object of SubgroupInfo </param>
    ////    /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>

    ////    private bool InsertIntoDatabase(SubgroupInfo  subgroupInfo)
    ////    {
    ////        bool RetVal = false;
    ////        string SubgroupName=subgroupInfo.Name;
    ////        string subgroupGID = subgroupInfo.GID;
    ////        string SubgroupGId = Guid.NewGuid().ToString();
    ////        string LanguageCode = string.Empty;
    ////        string DefaultLanguageCode = string.Empty;
    ////        string SubgroupForDatabase = string.Empty;

    ////        try
    ////        {
    ////            DefaultLanguageCode = this.DBQueries.LanguageCode;

    ////            //replace GID only if given gid is not empty or null.
    ////            if (!string.IsNullOrEmpty(subgroupGID))
    ////            {
    ////                SubgroupGId = subgroupGID;
    ////            }

    ////            foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
    ////            {
    ////                LanguageCode = languageRow[Language.LanguageCode].ToString();
    ////                if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
    ////                {
    ////                    SubgroupForDatabase = SubgroupName;
    ////                }
    ////                else
    ////                {
    ////                    SubgroupForDatabase = Constants.PrefixForNewValue + SubgroupName;
    ////                }
    ////                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Insert.InsertSubgroup(this.DBQueries.DataPrefix,"_"+LanguageCode,SubgroupForDatabase,SubgroupGId,subgroupInfo.Global,subgroupInfo.Type));  
    ////            }

    ////            RetVal = true;
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = false;
    ////        }

    ////        return RetVal;      
    ////    }


    ////    #endregion

    ////    #endregion

    ////    #region"--Internal--"

    ////    #region "-- Variables & Properties --"

    ////    /// <summary>
    ////    /// Returns subgroup colleciton in key,pair format. Key is subgroup name and value is Object of SubgroupInfo.
    ////    /// </summary>
    ////    internal Dictionary<string, SubgroupInfo> SubgroupCollection = new Dictionary<string, SubgroupInfo>();

    ////    #endregion

    ////    #endregion

    ////    #region "-- Public --"
        
    ////    #region "-- New/Dispose --"

    ////    public SubgroupBuilder(DIConnection connection, DIQueries queries)
    ////    {
    ////        this.DBConnection = connection;
    ////        this.DBQueries = queries;
    ////    }

    ////    #endregion

    ////    #region "-- Methods --"
    ////    /// <summary>
    ////    /// Check and create Subgroup record
    ////    /// </summary>
    ////    /// <param name="subgroupInfo">object of SubgroupInfo </param>
    ////    /// <returns>Nid</returns>
    ////    public int CheckNCreateSubgroup(SubgroupInfo subgroupInfo)
    ////    {
    ////        int RetVal = 0;
            
    ////        try
    ////        {
    ////            // check Subgroup exists or not
    ////            RetVal = this.CheckSubgroupExists(subgroupInfo);

    ////            // if Subgroup does not exist then create it.
    ////            if (RetVal <= 0)
    ////            {
    ////                 // insert unit
    ////                if (this.InsertIntoDatabase(subgroupInfo))
    ////                {
    ////                    RetVal = this.GetSubgroupByName(subgroupInfo.Name);
    ////                }
                    
    ////            }

    ////            // add Subgroup information into collection
    ////            subgroupInfo.Nid = RetVal;
    ////            this.AddSubgroupIntoCollection(subgroupInfo);
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = 0;
    ////        }
                        
    ////        return RetVal;
    ////    }
        
    ////    /// <summary>
    ////    /// Get Subgroup nid.
    ////    /// </summary>
    ////    /// <param name="subgroupGid">Subgroup GID </param>
    ////    /// <param name="subgroupName">Name of the Subgroup</param>
    ////    /// <returns> Nid</returns>
    ////    public int GetSubgroupNid(string subgroupGid, string subgroupName)
    ////    {
    ////        int RetVal = 0;
    ////        string SqlQuery = string.Empty;

    ////        //--step1:Get Nid by GID if GID is not empty
    ////        if (!string.IsNullOrEmpty(subgroupGid))
    ////        {
    ////            RetVal = this.GetNidByGID(subgroupGid);
    ////        }

    ////        //--step2:Get Nid by Name if name is not empty
    ////        if (RetVal <= 0)
    ////        {
    ////            if (!string.IsNullOrEmpty(subgroupName))
    ////            {
    ////                RetVal = this.GetSubgroupByName(subgroupName);
    ////            }
    ////        }

    ////        return RetVal;
    ////    }

    ////    /// <summary>
    ////    /// To Import a Subgroup into template or database
    ////    /// </summary>
    ////    /// <param name="subgroupInfo"></param>
    ////    /// <param name="NidInSourceDB"></param>
    ////    /// <param name="sourceQurey"></param>
    ////    /// <param name="sourceDBConnection"></param>
    ////    /// <returns></returns>
    ////    public int ImportSubgroup(SubgroupInfo subgroupInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection) 
    ////    {
    ////        int RetVal = -1;

    ////        try
    ////        {
    ////            //check  already exists in database or not
    ////            RetVal = this.GetSubgroupNid(subgroupInfo.GID, subgroupInfo.Name);

    ////            if (RetVal > 0)
    ////            {
    ////                // if source  is global
    ////                if (subgroupInfo.Global)
    ////                {
    ////                    try
    ////                    {
    ////                        //update the gid,name and global on the basis of nid
    ////                       this.DBConnection.ExecuteNonQuery( DALQueries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupInfo.Name, subgroupInfo.GID, subgroupInfo.Global, subgroupInfo.Type, RetVal));
    ////                    }
    ////                    catch (Exception)
    ////                    {
    ////                        RetVal = -1;
    ////                    }
    ////                }

    ////            }
    ////            else
    ////            {
    ////                if (this.InsertIntoDatabase(subgroupInfo))
    ////                {
    ////                    //get nid
    ////                    RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
    ////                }
    ////            }
                              

    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            throw new ApplicationException(ex.Message);
    ////        }

    ////        return RetVal;
    ////    }


    ////    /// <summary>
    ////    /// Returns the instance of SubgroupInfo on the basis of Subgroup Nid
    ////    /// </summary>
    ////    /// <param name="sourceQuery"></param>
    ////    /// <param name="sourceDBConnection"></param>
    ////    /// <param name="subgroupNid"></param>
    ////    /// <returns></returns>
    ////    public static SubgroupInfo GetSubgroupInfoByNid(DIQueries sourceQuery, DIConnection sourceDBConnection, int subgroupNid)
    ////    {
    ////        SubgroupInfo RetVal = new SubgroupInfo();
    ////        string Query = string.Empty;
    ////        DataTable Table;            

    ////        //get subgroup info from source database
    ////        try
    ////        {
    ////            if(subgroupNid>0)
    ////            {
    ////            Query = sourceQuery.Subgroup.GetSubgroup(FilterFieldType.NId, subgroupNid.ToString());
    ////            Table = sourceDBConnection.ExecuteDataTable(Query);                    
    ////            if (Table.Rows.Count > 0)
    ////            {
    ////                    RetVal.Nid = subgroupNid;
    ////                    RetVal.GID = Table.Rows[0][Subgroup.SubgroupGId].ToString();
    ////                    RetVal.Name = Table.Rows[0][Subgroup.SubgroupName].ToString();
    ////                    RetVal.Global = Convert.ToBoolean(Table.Rows[0][Subgroup.SubgroupGlobal]);
    ////                    RetVal.Type = (SubgroupType)(Convert.ToInt32(Table.Rows[0][Subgroup.SubgroupType]));
    ////                }
    ////            }
    ////        }
    ////        catch (Exception ex)
    ////        {
    ////            throw new ApplicationException(ex.Message);
    ////        }
    ////        return RetVal;
    ////    }



    ////    /// <summary>
    ////    /// Returns instance of SubgrouopInfo
    ////    /// </summary>
    ////    /// <param name="filterClause"></param>
    ////    /// <param name="filterText"></param>
    ////    /// <param name="selectionType"></param>
    ////    /// <returns></returns>
    ////    public SubgroupInfo GetSubgroupInfo(FilterFieldType filterClause, string filterText)
    ////    {
    ////        string Query = string.Empty;
    ////        SubgroupInfo RetVal = new SubgroupInfo();
    ////        DataTable SubgroupTable;
    ////        try
    ////        {
				
    ////            Query = this.DBQueries.Subgroup.GetSubgroup(filterClause, filterText);
    ////            SubgroupTable = this.DBConnection.ExecuteDataTable(Query);

    ////            //set Subgroup info
    ////            if (SubgroupTable != null)
    ////            {
    ////                if (SubgroupTable.Rows.Count > 0)
    ////                {
    ////                    RetVal.GID = SubgroupTable.Rows[0][Subgroup.SubgroupGId].ToString();
    ////                    RetVal.Global =Convert.ToBoolean(SubgroupTable.Rows[0][Subgroup.SubgroupGlobal].ToString());
    ////                    RetVal.Name = SubgroupTable.Rows[0][Subgroup.SubgroupName].ToString();
    ////                    RetVal.Nid = Convert.ToInt32(SubgroupTable.Rows[0][Subgroup.SubgroupNId].ToString());
    ////                }
    ////            }
    ////        }
    ////        catch (Exception)
    ////        {
    ////            RetVal = null;
    ////        }
    ////        return RetVal;
    ////    }

    ////    /// <summary>
    ////    /// Deletes subgroups and associated records from subgroupVals, IUS and IC_IUS tables
    ////    /// </summary>
    ////    /// <param name="nids"></param>
    ////    public void DeleteSubgroups(string nids)
    ////    {
    ////        DITables TableNames;
    ////        DI6SubgroupValBuilder SubgroupValsBuilder;
    ////        string AssocicatedSubgroupValNIds = string.Empty;
            
    ////        try
    ////        {
                
    ////            SubgroupValsBuilder= new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

    ////            // Step 1: Get subgroupValsNid of all  associated subgroupVals 
    ////            AssocicatedSubgroupValNIds= SubgroupValsBuilder.GetAllAssociatedSubgroupValNids(nids);
    ////            if (!string.IsNullOrEmpty(AssocicatedSubgroupValNIds))
    ////            {
    ////                // Step 2: Delete records from subgroup val table
    ////                //subgroupValBuilder.Delete deletes the records from subgroupVal, IUS and IC_IUS
    ////                SubgroupValsBuilder.DeleteSubgroupVals(AssocicatedSubgroupValNIds);
    ////            }

    ////                //Step 3: Delete records from subgroup table
    ////                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
    ////                {
    ////                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
    ////                  this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Delete.DeleteSubgroups(TableNames.Subgroup, nids));
    ////                }
               

    ////        }
    ////        catch (Exception ex)
    ////        {

    ////            throw new ApplicationException(ex.ToString());
    ////        }


    ////    }      

    ////    #endregion

    ////    #endregion

    ////}
}
