using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// A delegate for SubgroupValGIDChanged event
    /// </summary>
    /// <param name="subgroupValNID"></param>
    /// <param name="newGID"></param>
    /// <param name="isUpdated"></param>
    public delegate void SubgroupValGIdChangedDelegate(string subgroupValNID, string newGID, bool isUpdated);


    /// <summary>
    /// Build subgroupval record according to subgroupval  and insert it into database.   
    /// </summary>
    public class DI6SubgroupValBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;
        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Check existance of subgroup_val first in collection then in database. 
        /// </summary>
        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
        /// <returns>SubgroupVal Nid</returns>
        private int ChecksubgroupValExists(DI6SubgroupValInfo subgroupValInfo)
        {
            int RetVal = 0;

            //Step 1: check subgroupVal exists in subgroupVal collection
            RetVal = this.CheckSubgroupValInCollection(subgroupValInfo.Name);

            //Step 2: check subgroupVal exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetSubgroupValNid(subgroupValInfo.GID, subgroupValInfo.Name);
            }

            return RetVal;
        }


        /// <summary>
        /// Add subgroupval in collection. 
        /// </summary>
        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
        private void AddSubgroupValIntoCollection(DI6SubgroupValInfo subgroupValInfo)
        {
            if (!this.SubgroupValCollection.ContainsKey(subgroupValInfo.Name))
            {
                this.SubgroupValCollection.Add(subgroupValInfo.Name, subgroupValInfo);
            }
        }
        /// <summary>
        /// check existance of subgroupval in collection 
        /// </summary>
        /// <param name="name">name </param>
        /// <returns>SubgroupVal Nid</returns>
        private int CheckSubgroupValInCollection(string name)
        {
            int RetVal = 0;
            try
            {
                if (this.SubgroupValCollection.ContainsKey(name))
                {
                    RetVal = this.SubgroupValCollection[name].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        /// <summary>
        /// Get subgroupval Nid by name 
        /// </summary>
        ///<param name="name">SubgroupVal Name</param>
        /// <returns>SubgroupVal Nid</returns>
        private int GetNidByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.Name, "'" +
                    DIQueries.RemoveQuotesForSqlQuery(name) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Get nid only if GID exists in the database
        /// </summary>
        ///<param GID="GID">SubgroupVal GId</param>
        /// <returns>SubgroupVal Nid</returns>
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.GId, "'" + DIQueries.RemoveQuotesForSqlQuery(GID) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert subgroupval record into database. 
        /// </summary>
        /// <param name="subgroupVal">Subgroupval Name</param>
        /// <param name="subgroupValGId">Subgroupval GID</param>
        /// <param name="isGlobal">True/false</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(string subgroupVal, string subgroupValGId, bool isGlobal)
        {
            bool RetVal = false;
            string SubgroupValGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string SubgroupValForDatabase = string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace gid only if given
                if (!string.IsNullOrEmpty(subgroupValGId))
                {
                    SubgroupValGId = subgroupValGId;
                }


                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        SubgroupValForDatabase = subgroupVal;
                    }
                    else
                    {
                        SubgroupValForDatabase = Constants.PrefixForNewValue + subgroupVal;
                    }

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Insert.InsertSubgroupVal(this.DBQueries.DataPrefix, "_"
                        + LanguageCode, SubgroupValForDatabase,
                        SubgroupValGId, isGlobal, this.DBConnection.ConnectionStringParameters.ServerType));
                }
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert subgroupval record into database. 
        /// </summary>
        /// <param name="subgroupVal">Subgroupval Name</param>
        /// <param name="subgroupValGId">Subgroupval GID</param>
        /// <param name="isGlobal">True/false</param>
        /// <param name="order"></param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(string subgroupVal, string subgroupValGId, bool isGlobal, int order)
        {
            bool RetVal = false;
            string SubgroupValGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string SubgroupValForDatabase = string.Empty;
            int LastOrder = 0;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace gid only if given
                if (!string.IsNullOrEmpty(subgroupValGId))
                {
                    SubgroupValGId = subgroupValGId;
                }

                // if subgroup order <= 0 then set the subgroup  order
                if (order <= 0)
                {
                    try
                    {
                        LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.SubgroupVals.GetMaxSubgroupValOrder()));
                    }
                    catch (Exception)
                    {
                    }

                    // set subgroup order
                    order = LastOrder + 1;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        SubgroupValForDatabase = subgroupVal;
                    }
                    else
                    {
                        SubgroupValForDatabase = Constants.PrefixForNewValue + subgroupVal;
                    }

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Insert.InsertSubgroupVal(this.DBQueries.DataPrefix, "_"
                        + LanguageCode, SubgroupValForDatabase,
                        SubgroupValGId, isGlobal, this.DBConnection.ConnectionStringParameters.ServerType, order));
                }
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert subgroupval record into database 
        /// </summary>
        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
        /// <returns></returns>
        private bool InsertIntoDatabase(DI6SubgroupValInfo subgroupValInfo)
        {
            return this.InsertIntoDatabase(subgroupValInfo.Name, subgroupValInfo.GID, subgroupValInfo.Global, subgroupValInfo.SubgroupValOrder);
        }

        private bool CompareDimensions(DI6SubgroupValInfo sourceSGValInfo, DI6SubgroupValInfo targetSGValInfo)
        {
            bool RetVal = true;

            if (sourceSGValInfo.Dimensions.Count != targetSGValInfo.Dimensions.Count)
            {
                RetVal = false;
            }
            else
            {
                // check each dimension by name and GID
                for (int Index = 0; Index < sourceSGValInfo.Dimensions.Count; Index++)
                {
                    if (sourceSGValInfo.Dimensions[Index].GID != targetSGValInfo.Dimensions[Index].GID && sourceSGValInfo.Dimensions[Index].Name != sourceSGValInfo.Dimensions[Index].Name)
                    {
                        RetVal = false;
                        break;
                    }
                }

            }

            return RetVal;
        }

        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns Subgroup Val colleciton in key,pair format. Key is subgroupVal name and value is Object of SubgroupValInfo.
        /// </summary>
        internal Dictionary<string, DI6SubgroupValInfo> SubgroupValCollection = new Dictionary<string, DI6SubgroupValInfo>();

        #endregion
        #endregion

        #region "-- Public --"

        #region "-- Event --"

        /// <summary>
        /// Fires when a subgroupval GID is Updated by subgroupNIds
        /// </summary>
        public event SubgroupValGIdChangedDelegate SubgroupValGIDChanged;

        #endregion

        #region "-- New/Dispose --"

        public DI6SubgroupValBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Retruns subgroup val nid where combination of subgroup nids is exactly same as given in the subgroup list
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public int GetSubgroupValNIdBySugbroups(List<string> subgroupNIds)
        {
            int RetVal = -1;
            string Query = string.Empty;

            try
            {
                Query = this.DBQueries.SubgroupVals.GetSubgroupValNIdBySubgroups(subgroupNIds);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(Query));

                if (RetVal > 0)
                {

                    DataTable TempTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(RetVal.ToString(), string.Empty));

                    if (TempTable.Rows.Count != subgroupNIds.Count)
                    {
                        RetVal = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Check subgroupval record into database if false create subgroupval record  
        /// </summary>
        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
        /// <returns>SubgroupVal Nid</returns>
        public int CheckNCreateSubgroupVal(DI6SubgroupValInfo subgroupValInfo)
        {
            int RetVal = 0;

            try
            {
                // check subgroupVal exists or not
                RetVal = this.ChecksubgroupValExists(subgroupValInfo);

                // if subgroupVal does not exist then create it.
                if (RetVal <= 0)
                {
                    RetVal = this.CreateSubgroupVal(subgroupValInfo);
                    //// insert subgroupVal
                    //if (this.InsertIntoDatabase(subgroupValInfo))
                    //{
                    //    RetVal = this.GetNidByName(subgroupValInfo.Name);
                    //}
                }

                // add subgroupVal information into collection
                subgroupValInfo.Nid = RetVal;
                this.AddSubgroupValIntoCollection(subgroupValInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        public int CreateSubgroupVal(DI6SubgroupValInfo subgroupValInfo)
        {
            int RetVal = 0;

            try
            {                
                    // insert subgroupVal
                    if (this.InsertIntoDatabase(subgroupValInfo))
                    {
                        RetVal = this.GetNidByName(subgroupValInfo.Name);
                    }              
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }


        /// <summary>
        /// Get subgroupval Nid. Pass either GId or name and ofcourse you can pass both.   
        /// </summary>
        /// <param name="GID">Subgroup Val GID </param>
        /// <param name="name">SubgroupVal Name</param>
        /// <returns>SubgroupVal Nid</returns>
        public int GetSubgroupValNid(string GID, string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(GID))
            {
                RetVal = this.GetNidByGID(GID);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    RetVal = this.GetNidByName(name);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Returns instance of SubgrouopValInfo
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public DI6SubgroupValInfo GetSubgroupValInfo(FilterFieldType filterClause, string filterText)
        {
            string Query = string.Empty;
            DI6SubgroupValInfo RetVal = new DI6SubgroupValInfo();
            DataTable SubgroupTable;
            DI6SubgroupBuilder SGBuilder;
            try
            {

                Query = this.DBQueries.SubgroupVals.GetSubgroupVals(filterClause, filterText);
                SubgroupTable = this.DBConnection.ExecuteDataTable(Query);

                // Step 1: set SubgroupVal info
                if (SubgroupTable != null)
                {
                    if (SubgroupTable.Rows.Count > 0)
                    {
                        RetVal.GID = SubgroupTable.Rows[0][SubgroupVals.SubgroupValGId].ToString();
                        RetVal.Global = Convert.ToBoolean(Convert.ToInt32(SubgroupTable.Rows[0][SubgroupVals.SubgroupValGlobal]));
                        RetVal.Name = SubgroupTable.Rows[0][SubgroupVals.SubgroupVal].ToString();
                        RetVal.Nid = Convert.ToInt32(SubgroupTable.Rows[0][SubgroupVals.SubgroupValNId].ToString());
                        // Step 2: Get associated subgroup dimensions
                        SGBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
                        RetVal.Dimensions = SGBuilder.GetAssocaitedSubgroupsInfo(RetVal.Nid);

                    }
                }
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Call this function to update subgroupVals 
        /// </summary>
        /// <param name="subgroupNid"></param>
        /// <param name="subgroupVal"></param>
        /// <param name="subgroupValGlobal"></param>
        /// <param name="subgroupValGId"></param>
        public void UpdateSubgroupVals(int subgroupNid, string subgroupVal, bool subgroupValGlobal, string subgroupValGId)
        {
            string SqlQuery = string.Empty;
            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
            SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupVal, subgroupValGId, subgroupValGlobal, subgroupNid);

            try
            {
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Call this function to update subgroupVals with SortOrder
        /// </summary>
        /// <param name="subgroupNid"></param>
        /// <param name="subgroupVal"></param>
        /// <param name="subgroupValGlobal"></param>
        /// <param name="subgroupValGId"></param>
        /// <param name="order"></param>
        public void UpdateSubgroupVals(int subgroupNid, string subgroupVal, bool subgroupValGlobal, string subgroupValGId, int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupVal, subgroupValGId, subgroupValGlobal, subgroupNid, order);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// Call this function to update only SortOrder of subgroupVal
        /// </summary>
        /// <param name="subgroupValNid"></param>
        /// <param name="order"></param>
        public void UpdateSubgroupValsOrder(int subgroupValNid, int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Update.UpdateSubgroupValOrder(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupValNid, order);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// Deletes subgroupVals and associated records from SubgroupValSubgroup, IUS and IC_IUS tables
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteSubgroupVals(string nids)
        {
            DITables TableNames;
            IUSBuilder IUSBuilder;
            string AssocicatedIUSNIds = string.Empty;
            try
            {

                IUSBuilder = new IUSBuilder(this.DBConnection, this.DBQueries);

                // Step 1: Delete records from subgroup val table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Delete.DeleteSubgroupVals(TableNames.SubgroupVals, nids));
                }

                // Step 2: Delete records from SubgroupValSubgroup table
                foreach (string NId in DICommon.SplitString(nids, ","))
                {
                    this.DeleteSubgroupValRelations(Convert.ToInt32(NId));
                }

                // Step 3: Delete records from IUS table

                // Step 3(a): Get all associated IUSNIds
                AssocicatedIUSNIds = IUSBuilder.GetAllAssociatedIUSNids(string.Empty, string.Empty, nids);

                // Step 3(b): Delete all associated IUSNIds
                IUSBuilder.DeleteIUS(AssocicatedIUSNIds);


            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }


        }

        /// <summary>
        /// Creates the Subgroup Val text by using the given subgroup nids
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public string CreateSubgroupValTextBySubgroupNids(string subgroupNIds)
        {
            string RetVal = string.Empty;
            DataTable Table;

            try
            {
                if (!string.IsNullOrEmpty(subgroupNIds))
                {
                    // get subgroup and subgroup type details for the selected nids from database
                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroupInfoWithTypeNOrder(subgroupNIds));

                    foreach (DataRow Row in Table.Rows)
                    {
                        if (!string.IsNullOrEmpty(RetVal))
                        {
                            RetVal += " ";
                        }

                        RetVal += Row[Subgroup.SubgroupName].ToString();


                    }
                }
            }
            catch (Exception)
            {
               
            }

            return RetVal;
        }

        /// <summary>
        /// Creates the Subgroup Val GID by using the given subgroup nids. 
        /// This method concatenate the GID of all subgroups and check the lenght of final GID. IF new GID length is morethan 50 then string.emptry will be returned.
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public string CreateSubgroupValGIDBySubgroupNids(string subgroupNIds)
        {
            string RetVal = string.Empty;
            DataTable Table;

            // get subgroup and subgroup type details for the selected nids from database
            Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroupInfoWithTypeNOrder(subgroupNIds));

            foreach (DataRow Row in Table.Rows)
            {
                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal += " ";
                }

                RetVal += Row[Subgroup.SubgroupGId].ToString();


            }


            // Check the length of new GID . IF it is morethan 50 characters then replace it with string .empty
            if (RetVal.Length > 50)
            {
                RetVal = string.Empty;
            }

            return RetVal;
        }

        /// <summary>
        /// Insert the subgroupValRelations into SubgroupValSubgroup table
        /// </summary>
        /// <param name="subgroupValNId"></param>
        /// <param name="subgroupNId"></param>
        /// <returns></returns>
        public int InsertSubgroupValRelations(int subgroupValNId, int subgroupNId)
        {
            int RetVal = 0;
            DataTable TempTable;
            if (subgroupNId > 0 & subgroupValNId > 0)
            {
                try
                {
                    // check already exists or not
                    TempTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(subgroupValNId.ToString(), subgroupNId.ToString()));

                    if (TempTable.Rows.Count == 0)
                    {
                        // insert into database
                        this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupValSubgroup.Insert.InsertSubgroupValRelation(this.DBQueries.DataPrefix, subgroupValNId, subgroupNId));

                        RetVal = this.DBConnection.ExecuteNonQuery("SELECT @@IDENTITY");
                    }
                    else
                    {
                        RetVal = Convert.ToInt32(TempTable.Rows[0][SubgroupValsSubgroup.SubgroupValSubgroupNId]);
                    }

                }
                catch (Exception ex)
                {
                    RetVal = 0;
                    throw new ApplicationException(ex.ToString());
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Deletes subgroup val relations from Subgroup_Val_Subgroup table
        /// </summary>
        /// <param name="subgroupValNId"></param>
        public void DeleteSubgroupValRelations(int subgroupValNId)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup.Delete.DeleteSubgroupValRelations(this.DBQueries.TablesName.SubgroupValsSubgroup, subgroupValNId.ToString()));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Deletes subgroup val relations from Subgroup_Val_Subgroup table
        /// </summary>
        /// <param name="subgroupValNId"></param>
        /// <param name="subgroupNIds"></param>
        public void DeleteSubgroupValRelations(int subgroupValNId, string subgroupNIds)
        {
            try
            {

                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup.Delete.DeleteSubgroupRelationsFrmSubgroupVal(this.DBQueries.TablesName.SubgroupValsSubgroup, subgroupValNId, subgroupNIds));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// Returns all associated SubgroupValNIds from SubgroupValSubgroup table on the basis of SubgroupNId
        /// </summary>
        /// <param name="subgroupNIds">Comma separated NIds</param>
        /// <returns></returns>
        public string GetAllAssociatedSubgroupValNIds(string subgroupNIds)
        {
            string RetVal = string.Empty;
            DataTable Table;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(string.Empty, subgroupNIds));

                foreach (DataRow Row in Table.Rows)
                {
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal += ",";
                    }

                    RetVal += Row[SubgroupValsSubgroup.SubgroupValNId].ToString();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the list of subgroup dimensions NId which are associated with the given subgroupval NId
        /// </summary>
        /// <param name="subgroupValNId"></param>
        /// <returns></returns>
        public List<string> GetAssocaitedSubgroupsNId(int subgroupValNId)
        {
            List<string> RetVal = new List<string>();
            DataTable SVSTable; // Subgroup_Val_Subgroup
            int SubgroupNId = 0;

            try
            {
                SVSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(subgroupValNId.ToString(), string.Empty));

                foreach (DataRow SVSRow in SVSTable.Rows)
                {
                    SubgroupNId = Convert.ToInt32(SVSRow[SubgroupValsSubgroup.SubgroupNId]);
                    RetVal.Add(SubgroupNId.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }


            return RetVal;
        }

        /// <summary>
        /// Updates the SubgroupVal text for the given SubgroupValNIds
        /// </summary>
        /// <param name="SubgroupValNIds"></param>
        public void UpdateSubgroupValText(string SubgroupValNIds)
        {
            string SqlQuery = string.Empty;
            string SubgroupNids = string.Empty;
            string NewSubgroupValText = string.Empty;
            DI6SubgroupValInfo SGValInfo;
            DataTable Table;

            foreach (string NID in DICommon.SplitString(SubgroupValNIds, ","))
            {
                try
                {
                    // Step1: get SubgroupVal info
                    SGValInfo = this.GetSubgroupValInfo(FilterFieldType.NId, NID);

                    // Step2: Get all subgroup NID 
                    SqlQuery = this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(NID, string.Empty);
                    Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                    foreach (DataRow Row in Table.Rows)
                    {
                        if (!string.IsNullOrEmpty(SubgroupNids))
                        {
                            SubgroupNids += ",";
                        }
                        SubgroupNids += Row[Subgroup.SubgroupNId].ToString();
                    }

                    // Step3: Create SubgroupVal text 
                    if (!string.IsNullOrEmpty(SubgroupNids))
                    {
                        DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                        NewSubgroupValText = DICommon.RemoveQuotes(this.CreateSubgroupValTextBySubgroupNids(SubgroupNids));
                        // Step4: Update Subgroup Val text
                        this.UpdateSubgroupVals(Convert.ToInt32(NID), NewSubgroupValText, SGValInfo.Global, SGValInfo.GID);
                    }
                }
                catch (Exception ex)
                {

                    throw new ApplicationException(ex.ToString());
                }

            }

        }


        /// <summary>
        /// Updates the SubgroupVal GID for the given SubgroupNids
        /// </summary>
        /// <param name="SubgroupValNIds"></param>
        public void UpdateSubgroupValGID(string SubgroupNIds)
        {
            string SqlQuery = string.Empty;
            DataTable Table;
            string SubgroupValNIDs = string.Empty;
            string FoundSubgroupNIDs = string.Empty;
            string NewSubgroupValGID = string.Empty;
            string LanguageCode = string.Empty;

            foreach (string NID in DICommon.SplitString(SubgroupNIds, ","))
            {
                try
                {
                    // Step 1: Get all subgroup Val NIds
                    SqlQuery = this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(string.Empty, NID);
                    Table = this.DBConnection.ExecuteDataTable(SqlQuery);
                    SubgroupValNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, SubgroupVals.SubgroupValNId);

                    // Step 2: update each subgroupval GID
                    foreach (string SGNId in DICommon.SplitString(SubgroupValNIDs, ","))
                    {
                        // Step 2(a): Get all associated subgroup nids 
                        SqlQuery = this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(SGNId, string.Empty);
                        Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                        // step 2(b): update subgroupval gid
                        FoundSubgroupNIDs = DIConnection.GetDelimitedValuesFromDataTable(Table, Subgroup.SubgroupNId);
                        NewSubgroupValGID = this.CreateSubgroupValGIDBySubgroupNids(FoundSubgroupNIDs);

                        if (string.IsNullOrEmpty(NewSubgroupValGID))
                        {
                            // raise an event to tell newSubgroupValGId's length is morethan 50
                            if (this.SubgroupValGIDChanged != null)
                            {
                                this.SubgroupValGIDChanged(SGNId, NewSubgroupValGID, false);
                            }
                        }
                        else
                        {
                            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                            // update in langauge tables
                            foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                            {
                                LanguageCode = "_" + Row[Language.LanguageCode].ToString();

                                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Update.UpdateSubgroupValGID(this.DBQueries.DataPrefix, LanguageCode, DICommon.RemoveQuotes(NewSubgroupValGID), Convert.ToInt32(SGNId));
                                this.DBConnection.ExecuteNonQuery(SqlQuery);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw new ApplicationException(ex.ToString());
                }

            }

        }

        /// <summary>
        /// Set Auto Order of SubgroupVal By SubgroupType_Order and Subgroup_Order
        /// </summary>
        public void SetSubgroupValOrderBySubgroupTypeNOrder()
        {
            DataView TableView = null;
            int Order = 1;
            List<int> SubgroupsList = new List<int>();

            try
            {
                TableView = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupVals.GetSubgroupsValsWithDimensionNDimValues()).DefaultView;
                TableView.Sort = Subgroup.SubgroupType + " Asc," + Subgroup.SubgroupOrder + " Asc";

                foreach (DataRowView Row in TableView)
                {
                    if (!SubgroupsList.Contains(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId])))
                    {
                        SubgroupsList.Add(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]));
                        this.UpdateSubgroupValsOrder(Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]), Order++);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #region "-- Old import --"
        ///// <summary>
        ///// To Import a SubgroupVal into template or database
        ///// </summary>
        ///// <param name="subgroupValInfo"></param>
        ///// <param name="NidInSourceDB"></param>
        ///// <param name="sourceQurey"></param>
        ///// <param name="sourceDBConnection"></param>
        ///// <returns></returns>
        //public int ImportSubgroupVal(DI6SubgroupValInfo subgroupValInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        //{
        //    int RetVal = -1;
        //    int NIDByName = 0;
        //    int SGNIdInTargetDB=0;
        //    bool SkipRecord = false;
        //    DataTable SGValSubgroupTable;

        //    DI6SubgroupBuilder SGBuilder ;
        //    DI6SubgroupInfo SourceSGInfo ;

        //    try
        //    {
        //        // Step1: check  already exists in database or not

        //        // Step1(a): Get NId by Name
        //        if (!string.IsNullOrEmpty(subgroupValInfo.GID))
        //        {
        //            // Step1(a.1): first check by gid and then by name
        //            RetVal = this.GetNidByGID(subgroupValInfo.GID);

        //            if (RetVal > 0)
        //            {
        //                // check for the duplicacy by name
        //                NIDByName = this.GetNidByName(subgroupValInfo.Name);
        //                if (RetVal != NIDByName & NIDByName > 0)
        //                {
        //                    //skip records 
        //                    SkipRecord = true;
        //                    RetVal = -1;
        //                }
        //            }
        //        }

        //        // Step2: if GID is empty or GID doesnt match  then get NId by name
        //        if (RetVal <= 0 & SkipRecord == false)
        //        {

        //            RetVal = this.GetNidByName(subgroupValInfo.Name);
        //        }


        //        if (!SkipRecord)
        //        {
        //            // Step3: insert or update SubgroupVal record
        //            if (RetVal > 0)
        //            {
        //                // Step3(a): update only if source item is global
        //                if (subgroupValInfo.Global)
        //                {
        //                    try
        //                    {

        //                        // Step3(a.1): update the gid,name and global on the basis of nid
        //                        this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupValInfo.Name, subgroupValInfo.GID, subgroupValInfo.Global, RetVal));

        //                        // Step3(a.2): delete all relationship from target database
        //                        this.DeleteSubgroupValRelations(RetVal);

        //                    }
        //                    catch (Exception)
        //                    {
        //                        RetVal = -1;
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                // Step3(b): Insert SubgroupVal into target database and get NID
        //                if (this.InsertIntoDatabase(subgroupValInfo))
        //                {
        //                    //get nid
        //                    RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
        //                }
        //            }

        //            //  Step4: update SubgroupVals relationship in Subgroup_Vals_Subgroup table
        //            if (RetVal > 0)
        //            {
        //                //  Step4(a): Get all associated records from source database
        //                SGValSubgroupTable = sourceDBConnection.ExecuteDataTable(sourceQurey.SubgroupValSubgroup.GetSubgroupValsSubgroup(NidInSourceDB.ToString(), string.Empty));

        //                SGBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
        //                foreach (DataRow Row in SGValSubgroupTable.Rows)
        //                {

        //                    // Step4(a.1): Get subgroupInfo from source database
        //                    SourceSGInfo = (new DI6SubgroupBuilder(sourceDBConnection, sourceQurey)).GetSubgroupInfo(FilterFieldType.NId, Row[SubgroupValsSubgroup.SubgroupNId].ToString());

        //                    // Step4(a.2): import subgroup and associated subgroupType into target database
        //                    SGNIdInTargetDB = SGBuilder.ImportSubgroup(SourceSGInfo, SourceSGInfo.Nid, sourceQurey, sourceDBConnection);

        //                    if (SGNIdInTargetDB > 0)
        //                    {
        //                        // Step4(a.3): import subgroup and associated subgroupType
        //                        this.InsertSubgroupValRelations(RetVal, SGNIdInTargetDB);
        //                    }
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(ex.Message);
        //    }

        //    return RetVal;
        //}

        #endregion

        #region "-- Import --"


        /// <summary>
        /// To Import a SubgroupVal into template or database
        /// </summary>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <remarks>
        ///     // Note 1: Always create subgroupVal text on the basis of subgroup dimension for both insertion and updation
        ///     // Note 2: Import process will not the import the order of subgoup type and use target subgroup type's order to generate subgroup val text
        /// 
        ///     // Case 1: If GIDs and Names are same then do nothing
        /// 
        ///     // Case 2: if GIDs and Names both are not found in target then import subgroupval with associated subgroup and subgroup type
        /// 
        ///     // Case 3: If GIDs or Names are different then always check for subgroup dimensions
        ///     // Case 3.1: If atleast one of the dimension doesn't exist in the target then create that subgruop val with associated subgroup dimension and subgroup type.
        ///     // Case 3.2: If all dimension matches then generate subgroup val and overwrite target subgroup val
        /// 
        ///     // Case 3.2.1: Check for global and local: Do import only if
        ///             // (1) target is local and source is global
        ///             // (2) target is global and source is global
        ///             // & not import if target is global and source is local
        /// 
        ///     // Case 4: if source GID is matched with one subgroupVal in target and source subgroupVal text is matched with another subgroupval available in target DB then do nothing
        /// 
        /// </remarks>
        /// <returns></returns>
        public int ImportSubgroupVal(int NIdInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            int NIDFoundInTargetByGId = 0;
            int NIDFoundInTargetByName = 0;
            int SGNIdInTargetDB = 0;

            string AssociatedSGNIds = string.Empty;
            string SGValText = string.Empty;

            bool SkipRecord = false;

            DI6SubgroupValBuilder SourceSGValBuilder;
            DI6SubgroupValInfo SourceSGValInfo;
            DI6SubgroupValInfo TargetSGValInfo;
            DI6SubgroupBuilder SGBuilder;

            try
            {
                // set builder
                SourceSGValBuilder = new DI6SubgroupValBuilder(sourceDBConnection, sourceQurey);

                // Get source subgroupval info 
                SourceSGValInfo = SourceSGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, NIdInSourceDB.ToString());
                // Get NID by name and GID                
                NIDFoundInTargetByGId = this.GetNidByGID(DICommon.RemoveQuotes(SourceSGValInfo.GID));
                NIDFoundInTargetByName = this.GetNidByName(DICommon.RemoveQuotes(SourceSGValInfo.Name));

                // Case 1: If GIDs and Names are same then do nothing
                if (NIDFoundInTargetByGId == NIDFoundInTargetByName & NIDFoundInTargetByGId > 0)
                {
                    // do nothing
                    SkipRecord = true;
                    RetVal = NIDFoundInTargetByGId;
                }
                else if (NIDFoundInTargetByGId == 0 || NIDFoundInTargetByName == 0)
                {

                    // Case 2: if GIDs and Names both are not found in target then import subgroupval with associated subgroup and subgroup type
                    if (NIDFoundInTargetByGId == 0 & NIDFoundInTargetByName == 0)
                    {
                        // Insert SubgroupVal into target database and get NID
                        SourceSGValInfo.Name = DICommon.RemoveQuotes(SourceSGValInfo.Name);
                        if (this.InsertIntoDatabase(SourceSGValInfo))
                        {
                            //get nid
                            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                        }

                    }
                    else
                    {
                        // Case 3: If GIDs or Names are different then always check for subgroup dimensions
                        // Get NID available in target DB
                        if (NIDFoundInTargetByName > 0)
                        {
                            RetVal = NIDFoundInTargetByName;
                        }
                        else
                        {
                            RetVal = NIDFoundInTargetByGId;
                        }

                        // Get SubgroupValInfo from target DB
                        TargetSGValInfo = this.GetSubgroupValInfo(FilterFieldType.NId, RetVal.ToString());

                        ////// Case 3.1: If atleast one of the dimension doesn't exist in the target then create that subgruop val with associated subgroup dimension and subgroup type.
                        ////if (this.CompareDimensions(SourceSGValInfo, TargetSGValInfo)==false)
                        ////{
                        ////    ImportDimensionsNType = true;
                        ////}
                        ////else
                        ////{
                        ////    // Case 3.2: If all dimension matches then generate subgroup val and overwrite target subgroup val
                        ////    ImportDimensionsNType = false;                            
                        ////}

                        // Case 3.2.1: Check for global and local: Do import only if
                        // (1) target is local and source is global
                        // (2) target is global and source is global
                        // & not import if target is global and source is local
                        if (TargetSGValInfo.Global & SourceSGValInfo.Global == false)
                        {
                            SkipRecord = true;
                        }

                    }

                }
                else
                {
                    // Case 4: if source GID is matched with one subgroupVal in target and source subgroupVal text is matched with another subgroupval available in target DB then do nothing

                    // do nothing
                    SkipRecord = true;
                }


                if (RetVal > 0)
                {
                    // Import SubgroupVal relationship in Subgroup_Vals_Subgroup table and also import subgroup & subgroup type
                    if (SkipRecord == false)
                    {
                        SGBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);

                        foreach (DI6SubgroupInfo SourceSGInfo in SourceSGValInfo.Dimensions)
                        {
                            // import subgroup and associated subgroupType into target database
                            SGNIdInTargetDB = SGBuilder.ImportSubgroup(SourceSGInfo, SourceSGInfo.Nid, sourceQurey, sourceDBConnection);

                            if (SGNIdInTargetDB > 0)
                            {
                                // import subgroup and associated subgroupType
                                this.InsertSubgroupValRelations(RetVal, SGNIdInTargetDB);


                                // add NId into AssociatedSGNIds 
                                if (!string.IsNullOrEmpty(AssociatedSGNIds))
                                {
                                    AssociatedSGNIds += ",";
                                }
                                AssociatedSGNIds += SGNIdInTargetDB;
                            }
                        }

                        if (string.IsNullOrEmpty(AssociatedSGNIds))
                        {

                        }
                        else
                        {
                            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                            // update the gid,name and global on the basis of nid
                            SGValText = DICommon.RemoveQuotes(this.CreateSubgroupValTextBySubgroupNids(AssociatedSGNIds));
                            this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, SGValText, SourceSGValInfo.GID, SourceSGValInfo.Global, RetVal));
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
        /// To Import subgroup val information from mapped subgroupval
        /// </summary>
        /// <param name="NIdInSourceDB"></param>
        /// <param name="NIdInTargetDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroupValFrmMappedSubgroupVal(int NIdInSourceDB, int NIdInTargetDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;

            string AssociatedSGNIds = string.Empty;
            string SGValText = string.Empty;

            bool SkipRecord = false;

            DI6SubgroupValBuilder SourceSGValBuilder;
            DI6SubgroupValInfo SourceSGValInfo;
            DI6SubgroupValInfo TargetSGValInfo;

            try
            {
                // set builder
                SourceSGValBuilder = new DI6SubgroupValBuilder(sourceDBConnection, sourceQurey);

                // Get source subgroupval info 
                SourceSGValInfo = SourceSGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, NIdInSourceDB.ToString());


                // Set RetVal= NIdInTrgDB
                RetVal = NIdInTargetDB;

                // Get SubgroupValInfo from target DB
                TargetSGValInfo = this.GetSubgroupValInfo(FilterFieldType.NId, RetVal.ToString());


                // Check for global and local: Do import only if
                // (1) target is local and source is global
                // (2) target is global and source is global
                // & not import if target is global and source is local
                if (TargetSGValInfo.Global & SourceSGValInfo.Global == false)
                {
                    SkipRecord = true;
                }

                if (RetVal > 0)
                {
                    // Import SubgroupVal relationship in Subgroup_Vals_Subgroup table and also import subgroup & subgroup type
                    if (SkipRecord == false)
                    {
                        DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                        // update the gid,name and global on the basis of nid
                        SGValText = DICommon.RemoveQuotes(SourceSGValInfo.Name);
                        this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, SGValText, SourceSGValInfo.GID, SourceSGValInfo.Global, RetVal));

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
        /// IMport SubgroupVal
        /// </summary>
        /// <param name="subgroupVal"></param>
        /// <param name="subgroupValGID"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public int ImportSubgroupVal(string subgroupVal, string subgroupValGID, bool isGlobal)
        {
            int RetVal = -1;
            int NIDFoundInTargetByGId = 0;
            int NIDFoundInTargetByName = 0;
            int SGNIdInTargetDB = 0;

            string AssociatedSGNIds = string.Empty;
            string SGValText = string.Empty;

            bool SkipRecord = false;

            DI6SubgroupValBuilder SourceSGValBuilder;
            DI6SubgroupValInfo SourceSGValInfo;
            DI6SubgroupValInfo TargetSGValInfo;
            DI6SubgroupBuilder SGBuilder;

            try
            {
                SourceSGValInfo = new DI6SubgroupValInfo();
                SourceSGValInfo.Name = subgroupVal;
                SourceSGValInfo.GID = subgroupValGID;
                SourceSGValInfo.Global = isGlobal;

                // set builder
                SourceSGValBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

                // Get NID by name and GID                
                NIDFoundInTargetByGId = this.GetNidByGID(DICommon.RemoveQuotes(SourceSGValInfo.GID));
                NIDFoundInTargetByName = this.GetNidByName(DICommon.RemoveQuotes(SourceSGValInfo.Name));

                // Case 1: If GIDs and Names are same then do nothing
                if (NIDFoundInTargetByGId == NIDFoundInTargetByName & NIDFoundInTargetByGId > 0)
                {
                    // do nothing
                    SkipRecord = true;
                    RetVal = NIDFoundInTargetByGId;
                }
                else if (NIDFoundInTargetByGId == 0 || NIDFoundInTargetByName == 0)
                {

                    // Case 2: if GIDs and Names both are not found in target then import subgroupval with associated subgroup and subgroup type
                    if (NIDFoundInTargetByGId == 0 & NIDFoundInTargetByName == 0)
                    {
                        // Insert SubgroupVal into target database and get NID
                        SourceSGValInfo.Name = DICommon.RemoveQuotes(SourceSGValInfo.Name);
                        if (this.InsertIntoDatabase(SourceSGValInfo))
                        {
                            //get nid
                            RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                        }

                    }
                    else
                    {
                        // Case 3: If GIDs or Names are different then always check for subgroup dimensions
                        // Get NID available in target DB
                        if (NIDFoundInTargetByName > 0)
                        {
                            RetVal = NIDFoundInTargetByName;
                        }
                        else
                        {
                            RetVal = NIDFoundInTargetByGId;
                        }

                        // Get SubgroupValInfo from target DB
                        TargetSGValInfo = this.GetSubgroupValInfo(FilterFieldType.NId, RetVal.ToString());
                        // Case 3.2.1: Check for global and local: Do import only if
                        // (1) target is local and source is global
                        // (2) target is global and source is global
                        // & not import if target is global and source is local
                        if (TargetSGValInfo.Global & SourceSGValInfo.Global == false)
                        {
                            SkipRecord = true;
                        }

                    }

                }
                else
                {
                    // Case 4: if source GID is matched with one subgroupVal in target and source subgroupVal text is matched with another subgroupval available in target DB then do nothing

                    // do nothing
                    SkipRecord = true;
                }


                if (RetVal > 0)
                {
                    // Import SubgroupVal relationship in Subgroup_Vals_Subgroup table and also import subgroup & subgroup type
                    if (SkipRecord == false)
                    {
                        SGBuilder = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
                        if (SourceSGValInfo.Dimensions != null)
                        {
                            foreach (DI6SubgroupInfo SourceSGInfo in SourceSGValInfo.Dimensions)
                            {
                                // import subgroup and associated subgroupType into target database
                                SGBuilder.GetSubgroupNid(SourceSGInfo.GID, SourceSGInfo.Name);
                                if (SGNIdInTargetDB > 0)
                                {
                                    // import subgroup and associated subgroupType
                                    this.InsertSubgroupValRelations(RetVal, SGNIdInTargetDB);

                                    // add NId into AssociatedSGNIds 
                                    if (!string.IsNullOrEmpty(AssociatedSGNIds))
                                    {
                                        AssociatedSGNIds += ",";
                                    }
                                    AssociatedSGNIds += SGNIdInTargetDB;
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(AssociatedSGNIds))
                        {

                        }
                        else
                        {
                            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
                            // update the gid,name and global on the basis of nid
                            SGValText = DICommon.RemoveQuotes(this.CreateSubgroupValTextBySubgroupNids(AssociatedSGNIds));
                            this.DBConnection.ExecuteNonQuery(DALQueries.SubgroupVal.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, SGValText, SourceSGValInfo.GID, SourceSGValInfo.Global, RetVal));
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

        #endregion


    }
}
