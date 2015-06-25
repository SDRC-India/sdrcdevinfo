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
    /// <summary>
    /// Build subgroup type and insert it into database.
    /// </summary>
    public class DI6SubgroupTypeBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of Subgroup type first in collection Then In database
        /// </summary>
        /// <param name="subgroupInfo">object of DI6SubgroupTypeInfo</param>
        /// <returns> Nid</returns>
        private int CheckSubgroupTypeExists(DI6SubgroupTypeInfo subgroupTypeInfo)
        {
            int RetVal = 0;

            //Step 1: check subgroup type exists in  collection
            RetVal = this.CheckSubgroupTypeInCollection(subgroupTypeInfo.Name);

            //Step 2: check it  exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetSubgroupTypeNid(subgroupTypeInfo.GID, subgroupTypeInfo.Name);
            }

            return RetVal;
        }

        /// <summary>
        /// Add subgroup type record into collection
        /// </summary>
        /// <param name="subgroupInfo">object of DI6SubgroupTypeInfo</param>

        private void AddSubgroupTypeIntoCollection(DI6SubgroupTypeInfo subgroupTypeInfo)
        {
            if (!this.SubgroupCollection.ContainsKey(subgroupTypeInfo.Name))
            {
                this.SubgroupCollection.Add(subgroupTypeInfo.Name, subgroupTypeInfo);
            }

        }
        /// <summary>
        /// Check subgroup type in collection
        /// </summary>
        /// <param name="subgroupType"> Type</param>
        /// <returns>Nid</returns>
        private int CheckSubgroupTypeInCollection(string subgroupType)
        {
            int RetVal = 0;
            try
            {
                if (this.SubgroupCollection.ContainsKey(subgroupType))
                {
                    RetVal = this.SubgroupCollection[subgroupType].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        /// <summary>
        /// Get subgroup Type Nid by  Name
        /// </summary>
        /// <returns> Nid</returns>
        private int GetSubgroupTypeByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.Name, "'" +
                    DIQueries.RemoveQuotesForSqlQuery( name) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Get subgroup type Nid from subgroup type by  GId.
        /// </summary>
        /// <returns></returns>
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.GId, "'" + GID + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert  record into subgroup type table
        /// </summary>
        /// <param name="subgroupTypeInfo">object of DI6SubgroupTypeInfo </param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>

        private bool InsertIntoDatabase(DI6SubgroupTypeInfo subgroupTypeInfo)
        {
            bool RetVal = false;
            string Name = subgroupTypeInfo.Name;
            string subgroupTypeGID = subgroupTypeInfo.GID;
            string SubgroupTypeGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string SubgroupTypeForDatabase = string.Empty;
            DITables TablesName;
            int LastOrder = 0;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(subgroupTypeGID))
                {
                    SubgroupTypeGId = subgroupTypeGID;
                }

                // if subgroup type  order <= 0 then set the subgroup type order
                if (subgroupTypeInfo.Order <= 0)
                {
                    try
                    {
                        LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.SubgroupTypes.GetMaxSubgroupTypeOrder()));
                    }
                    catch (Exception)
                    {
                    }

                    // set subgroup order
                    subgroupTypeInfo.Order = LastOrder + 1;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();

                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        SubgroupTypeForDatabase = Name;
                    }
                    else
                    {
                        SubgroupTypeForDatabase = Constants.PrefixForNewValue + Name;
                    }

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes.Insert.InsertSubgroupType(TablesName.SubgroupType, SubgroupTypeForDatabase, SubgroupTypeGId, subgroupTypeInfo.Order, subgroupTypeInfo.Global));
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private int GetLastSubgroupTypeOrder()
        {
            int RetVal = 0;
            DataTable SubgroupTypeTable;
            SubgroupTypeTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));

            if (SubgroupTypeTable.Rows.Count > 0)
            {
                RetVal=Convert.ToInt32(SubgroupTypeTable.Rows[0][SubgroupTypes.SubgroupTypeOrder].ToString());
            }

            
            return RetVal;
        }

        #endregion

        #endregion

        #region"--Internal--"

        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns subgroup type colleciton in key,pair format. Key is subgroup type and value is Object of SubgroupTypeInfo.
        /// </summary>
        internal Dictionary<string, DI6SubgroupTypeInfo> SubgroupCollection = new Dictionary<string, DI6SubgroupTypeInfo>();

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public DI6SubgroupTypeBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check and create Subgroup Type record
        /// </summary>
        /// <param name="subgroupTypeInfo">object of DI6SubgroupTypeInfo </param>
        /// <returns>Nid</returns>
        public int CheckNCreateSubgroupType(DI6SubgroupTypeInfo subgroupTypeInfo)
        {
            int RetVal = 0;

            try
            {
                // check Subgroup type exists or not
                RetVal = this.CheckSubgroupTypeExists(subgroupTypeInfo);

                // if Subgroup type does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert subgroup
                    if (this.InsertIntoDatabase(subgroupTypeInfo))
                    {
                        RetVal = this.GetSubgroupTypeByName(subgroupTypeInfo.Name);
                    }

                }

                // add Subgroup information into collection
                subgroupTypeInfo.Nid = RetVal;
                this.AddSubgroupTypeIntoCollection(subgroupTypeInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get Subgroup Type nid.
        /// </summary>
        /// <param name="subgroupTypeGid">Subgroup GID </param>
        /// <param name="subgroupTypeName">Name of the Subgroup</param>
        /// <returns> Nid</returns>
        public int GetSubgroupTypeNid(string subgroupTypeGid, string subgroupTypeName)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(subgroupTypeGid))
            {
                RetVal = this.GetNidByGID(subgroupTypeGid);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(subgroupTypeName))
                {
                    RetVal = this.GetSubgroupTypeByName(subgroupTypeName);
                }
            }

            return RetVal;
        }


        /// <summary>
        /// Returns the instance of SubgroupTypeInfo on the basis of Subgroup Nid
        /// </summary>
        /// <param name="subgroupTypeNid"></param>
        /// <returns></returns>
        public DI6SubgroupTypeInfo GetSubgroupTypeInfoByNid(int subgroupTypeNid)
        {
            DI6SubgroupTypeInfo RetVal = new DI6SubgroupTypeInfo();
            string Query = string.Empty;
            DataTable Table;

            //get subgroup type info from source database
            try
            {
                if (subgroupTypeNid > 0)
                {
                    Query = this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.NId, subgroupTypeNid.ToString());
                    Table = this.DBConnection.ExecuteDataTable(Query);

                    if (Table.Rows.Count > 0)
                    {
                        RetVal.Nid = subgroupTypeNid;
                        RetVal.GID = Table.Rows[0][SubgroupTypes.SubgroupTypeGID].ToString();
                        RetVal.Name = Table.Rows[0][SubgroupTypes.SubgroupTypeName].ToString();
                        RetVal.Global = Convert.ToBoolean(Table.Rows[0][SubgroupTypes.SubgroupTypeGlobal]);
                        RetVal.Order = Convert.ToInt32(Table.Rows[0][SubgroupTypes.SubgroupTypeOrder]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return RetVal;
        }



        /// <summary>
        /// Call this function to update subgroup
        /// </summary>
        /// <param name="NId"></param>
        /// <param name="name"></param>
        /// <param name="isGlobal"></param>
        /// <param name="GId"></param>
        /// <param name="order"></param>
        public void UpdateSubgroupType(int NId, string name, bool isGlobal, string GId, int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes.Update.UpdateSubgroupTypeByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, name, GId, isGlobal, order, NId);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// return query to update SubgroupType Order
        /// </summary>
        /// <param name="NId"></param>
        /// <param name="order"></param>
        public void UpdateSubgroupTypeOrder(int NId,int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes.Update.UpdateSubgroupTypeOrderByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, NId,order);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Deletes subgroup type and associated Subgroup, SubgroupVal, SubgroupValSubgroup,  Indicator_Uni_Subgroup and IC_IUS
        /// </summary>
        /// <param name="NIDs">Comma separated NIDs</param>
        public void DeleteSubgroupType(string NIDs)
        {
            string SqlQuery = string.Empty;
            string AssociatedSubgroupNIds = string.Empty;
            DITables TablesName;
            DI6SubgroupBuilder SGBuilder;

            try
            {

                  // Step 1:Delete subgroup type
                    foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                    {
                        TablesName = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                        SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes.Delete.DeleteSubgroupType(TablesName.SubgroupType, NIDs);

                        this.DBConnection.ExecuteNonQuery(SqlQuery);
                    }

                // Step 2: Delete associated records from Subgroup and Subgroup Builder will automatically  delete records from Subgroup, SubgroupValSubgroup, Indicator_Unit_Subgroup and IC_IUS.
                SGBuilder=new DI6SubgroupBuilder(this.DBConnection,this.DBQueries);

                // Get all associated SubgroupNIds
                AssociatedSubgroupNIds =SGBuilder.GetAllAssociatedSubgroupNIds(NIDs);
                if(!string.IsNullOrEmpty(AssociatedSubgroupNIds))
                {
                SGBuilder.DeleteSubgroup(AssociatedSubgroupNIds);
                }                
                
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }


        }

        /// <summary>
        /// To Import a SubgroupType into template or database
        /// </summary>
        /// <param name="subgroupTypeInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroupType(DI6SubgroupTypeInfo subgroupTypeInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            int NIDByName = 0;
            bool SkipRecord = false;
            try
            {
                //check  already exists in database or not

                //Get NId by Name
                if (!string.IsNullOrEmpty(subgroupTypeInfo.GID))
                {
                    //first check by gid and then by name
                    RetVal = this.GetNidByGID(subgroupTypeInfo.GID);

                    if (RetVal > 0)
                    {
                        // check for the duplicacy by name
                        NIDByName = this.GetSubgroupTypeByName(subgroupTypeInfo.Name);
                        if (RetVal != NIDByName & NIDByName>0)
                        {
                            //skip records 
                            SkipRecord = true;
                            RetVal = -1;
                        }
                    }
                }

                //if GID is empty or GID doesnt match  then get NId by name
                if(RetVal<=0 & SkipRecord==false)
                {
                    
                    RetVal = this.GetSubgroupTypeByName(subgroupTypeInfo.Name);
                }


                if (!SkipRecord)
                {
                    //insert or update record
                    if (RetVal > 0)
                    {
                        // update only if source item is global
                        if (subgroupTypeInfo.Global)
                        {
                            try
                            {
                                // dont change the order into target  database
                                

                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupTypes.Update.UpdateSubgroupTypeByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupTypeInfo.Name, subgroupTypeInfo.GID, subgroupTypeInfo.Global, this.GetSubgroupTypeInfoByNid(RetVal).Order, RetVal));
                            }
                            catch (Exception)
                            {
                                RetVal = -1;
                            }
                        }

                    }
                    else
                    {
                        // get the last subgroup Type order and update order in source subgrouptype
                       subgroupTypeInfo.Order= this.GetLastSubgroupTypeOrder()+1;

                        if (this.InsertIntoDatabase(subgroupTypeInfo))
                        {
                            //get nid
                            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }



        /// <summary>
        /// To Import SubgroupType information  from mapped subgroup type
        /// </summary>
        /// <param name="subgroupTypeInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidInTrgDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroupTypeFrmMappedSubgroupType(DI6SubgroupTypeInfo subgroupTypeInfo, int NidInSourceDB,int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;           
            bool SkipRecord = false;
            try
            {
                // set RetVal to NidInTrgDB
                RetVal = NidInTrgDB;

                if (!SkipRecord)
                {
                    //insert or update record
                    if (RetVal > 0)
                    {
                        // update only if source item is global
                        if (subgroupTypeInfo.Global)
                        {
                            try
                            {
                                // dont change the order into target  database


                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupTypes.Update.UpdateSubgroupTypeByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupTypeInfo.Name, subgroupTypeInfo.GID, subgroupTypeInfo.Global, this.GetSubgroupTypeInfoByNid(RetVal).Order, RetVal));
                            }
                            catch (Exception)
                            {
                                RetVal = -1;
                            }
                        }

                    }
                    else
                    {
                        // get the last subgroup Type order and update order in source subgrouptype
                        subgroupTypeInfo.Order = this.GetLastSubgroupTypeOrder() + 1;

                        if (this.InsertIntoDatabase(subgroupTypeInfo))
                        {
                            //get nid
                            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }
        
        /// <summary>
        /// Import Subgroup Type
        /// </summary>
        /// <param name="subgroupTypeInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroupType(string subgroupTypeName,string subgroupTypeGID,bool isGlobal)
        {
            int RetVal = -1;
            int NIDByName = 0;
            bool SkipRecord = false;
            DI6SubgroupTypeInfo SubgroupTypeInfo=new DI6SubgroupTypeInfo();

            try
            {
                SubgroupTypeInfo.Name = subgroupTypeName;
                SubgroupTypeInfo.GID = subgroupTypeGID;
                SubgroupTypeInfo.Global = isGlobal;

                //check  already exists in database or not

                //Get NId by Name
                if (!string.IsNullOrEmpty(SubgroupTypeInfo.GID))
                {
                    //first check by gid and then by name
                    RetVal = this.GetNidByGID(SubgroupTypeInfo.GID);

                    if (RetVal > 0)
                    {
                        // check for the duplicacy by name
                        NIDByName = this.GetSubgroupTypeByName(SubgroupTypeInfo.Name);
                        if (RetVal != NIDByName & NIDByName > 0)
                        {
                            //skip records 
                            SkipRecord = true;
                            RetVal = -1;
                        }
                    }
                }

                //if GID is empty or GID doesnt match  then get NId by name
                if (RetVal <= 0 & SkipRecord == false)
                {

                    RetVal = this.GetSubgroupTypeByName(SubgroupTypeInfo.Name);
                }


                if (!SkipRecord)
                {
                    //insert or update record
                    if (RetVal > 0)
                    {
                        // update only if source item is global
                        if (SubgroupTypeInfo.Global)
                        {
                            try
                            {
                                // dont change the order into target  database
                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupTypes.Update.UpdateSubgroupTypeByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, SubgroupTypeInfo.Name, SubgroupTypeInfo.GID, SubgroupTypeInfo.Global, this.GetSubgroupTypeInfoByNid(RetVal).Order, RetVal));
                            }
                            catch (Exception)
                            {
                                RetVal = -1;
                            }
                        }

                    }
                    else
                    {
                        // get the last subgroup Type order and update order in source subgrouptype
                        SubgroupTypeInfo.Order = this.GetLastSubgroupTypeOrder() + 1;

                        if (this.InsertIntoDatabase(SubgroupTypeInfo))
                        {
                            //get nid
                            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }


        #endregion

        #endregion

    }
}
