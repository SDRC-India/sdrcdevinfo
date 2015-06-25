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
    /// Build subgroup record and insert it into database.
    /// </summary>
    public class DI6SubgroupBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of Subgroup first in collection Then In database
        /// </summary>
        /// <param name="subgroupInfo">object of UnitInfo</param>
        /// <returns> Nid</returns>
        private int CheckSubgroupExists(DI6SubgroupInfo subgroupInfo)
        {
            int RetVal = 0;

            //Step 1: check subgroup exists in Unit collection
            RetVal = this.CheckSubgroupInCollection(subgroupInfo.Name);

            //Step 2: check unit exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetSubgroupNid(subgroupInfo.GID, subgroupInfo.Name);
            }

            return RetVal;
        }
        /// <summary>
        /// Add subgroup record into collection
        /// </summary>
        /// <param name="subgroupInfo">object of subgroupInfo</param>

        private void AddSubgroupIntoCollection(DI6SubgroupInfo subgroupInfo)
        {
            if (!this.SubgroupCollection.ContainsKey(subgroupInfo.Name))
            {
                this.SubgroupCollection.Add(subgroupInfo.Name, subgroupInfo);
            }

        }
        /// <summary>
        /// Check subgroup in collection
        /// </summary>
        /// <param name="subgroupName"> Name</param>
        /// <returns>Nid</returns>
        private int CheckSubgroupInCollection(string subgroupName)
        {
            int RetVal = 0;
            try
            {
                if (this.SubgroupCollection.ContainsKey(subgroupName))
                {
                    RetVal = this.SubgroupCollection[subgroupName].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        /// <summary>
        /// Get subgroup Nid by  Name
        /// </summary>
        /// <returns> Nid</returns>
        private int GetSubgroupByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery( name) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Get subgroup Nid from subgroup by  GId.
        /// </summary>
        /// <returns></returns>
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.GId, "'" + GID + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert  record into subgroup table
        /// </summary>
        /// <param name="subgroupInfo">object of SubgroupInfo </param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>

        private bool InsertIntoDatabase(DI6SubgroupInfo subgroupInfo)
        {
            bool RetVal = false;
            string SubgroupName = subgroupInfo.Name;
            string subgroupGID = subgroupInfo.GID;
            string SubgroupGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string SubgroupForDatabase = string.Empty;
            int LastOrder = 0;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(subgroupGID))
                {
                    SubgroupGId = subgroupGID;
                }

                // if subgroup order <= 0 then set the subgroup order
                if (subgroupInfo.SubgroupOrder <= 0)
                {
                    try
                    {
                        LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Subgroup.GetMaxSubgroupOrder(subgroupInfo.Type)));
                    }
                    catch (Exception)
                    {
                    }

                    // set subgroup order
                    subgroupInfo.SubgroupOrder = LastOrder + 1;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        SubgroupForDatabase = SubgroupName;
                    }
                    else
                    {
                        SubgroupForDatabase = Constants.PrefixForNewValue + SubgroupName;
                    }
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Insert.InsertSubgroup(this.DBQueries.DataPrefix, "_" + LanguageCode, SubgroupForDatabase, SubgroupGId, subgroupInfo.Global, subgroupInfo.Type, DBConnection.ConnectionStringParameters.ServerType,subgroupInfo.SubgroupOrder ));
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }


        #endregion

        #endregion

        #region"--Internal--"

        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns subgroup colleciton in key,pair format. Key is subgroup name and value is Object of SubgroupInfo.
        /// </summary>
        internal Dictionary<string, DI6SubgroupInfo> SubgroupCollection = new Dictionary<string, DI6SubgroupInfo>();

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public DI6SubgroupBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Check and create Subgroup record
        /// </summary>
        /// <param name="subgroupInfo">object of DI6SubgroupInfo </param>
        /// <returns>Nid</returns>
        public int CheckNCreateSubgroup(DI6SubgroupInfo subgroupInfo)
        {
            int RetVal = 0;

            try
            {
                // check Subgroup exists or not
                RetVal = this.CheckSubgroupExists(subgroupInfo);

                // if Subgroup does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert subgroup
                    if (this.InsertIntoDatabase(subgroupInfo))
                    {
                        RetVal = this.GetSubgroupByName(subgroupInfo.Name);
                    }

                }

                // add Subgroup information into collection
                subgroupInfo.Nid = RetVal;
                this.AddSubgroupIntoCollection(subgroupInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get Subgroup nid.
        /// </summary>
        /// <param name="subgroupGid">Subgroup GID </param>
        /// <param name="subgroupName">Name of the Subgroup</param>
        /// <returns> Nid</returns>
        public int GetSubgroupNid(string subgroupGid, string subgroupName)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(subgroupGid))
            {
                RetVal = this.GetNidByGID(subgroupGid);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(subgroupName))
                {
                    RetVal = this.GetSubgroupByName(subgroupName);
                }
            }

            return RetVal;
        }


        /// <summary>
        /// Returns the instance of SubgroupInfo on the basis of Subgroup Nid
        /// </summary>
        /// <param name="sourceQuery"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="subgroupNid"></param>
        /// <returns></returns>
        public static DI6SubgroupInfo GetSubgroupInfoByNid(DIQueries sourceQuery, DIConnection sourceDBConnection, int subgroupNid)
        {
            DI6SubgroupInfo RetVal = new DI6SubgroupInfo();
            RetVal = DI6SubgroupBuilder.GetSubgroupInfo(sourceQuery, sourceDBConnection, FilterFieldType.NId, subgroupNid.ToString());
            return RetVal;
        }


        /// <summary>
        /// Returns the instance of SubgroupInfo on the basis of Subgroup Nid
        /// </summary>
        /// <param name="sourceQuery"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="subgroupNid"></param>
        /// <returns></returns>
        public static DI6SubgroupInfo GetSubgroupInfo(DIQueries queries, DIConnection dbConnection, FilterFieldType filterClause, string filterText)
        {

            string Query = string.Empty;
            DI6SubgroupInfo RetVal = new DI6SubgroupInfo();
            DataTable SubgroupTable;
            DI6SubgroupTypeBuilder SGTypeBuilder;
            try
            {
                Query = queries.Subgroup.GetSubgroup(filterClause, filterText);
                SubgroupTable = dbConnection.ExecuteDataTable(Query);

                //set Subgroup info
                if (SubgroupTable != null)
                {
                    if (SubgroupTable.Rows.Count > 0)
                    {
                        RetVal.GID = SubgroupTable.Rows[0][Subgroup.SubgroupGId].ToString();
                        RetVal.Global = Convert.ToBoolean(SubgroupTable.Rows[0][Subgroup.SubgroupGlobal]);
                        RetVal.Name = SubgroupTable.Rows[0][Subgroup.SubgroupName].ToString();
                        RetVal.Nid = Convert.ToInt32(SubgroupTable.Rows[0][Subgroup.SubgroupNId].ToString());
                        RetVal.Type = Convert.ToInt32(SubgroupTable.Rows[0][Subgroup.SubgroupType].ToString());

                        // Get subgrouptype info
                        if (RetVal.Type > 0)
                        {
                            SGTypeBuilder = new DI6SubgroupTypeBuilder(dbConnection, queries);
                            RetVal.DISubgroupType = SGTypeBuilder.GetSubgroupTypeInfoByNid(RetVal.Type);

                        }

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
        /// Returns instance of SubgrouopInfo
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public DI6SubgroupInfo GetSubgroupInfo(FilterFieldType filterClause, string filterText)
        {

            return DI6SubgroupBuilder.GetSubgroupInfo(this.DBQueries, this.DBConnection, filterClause, filterText);

        }

        /// <summary>
        /// Returns all associated SubgroupNIds from Subgroup table on the basis of SubgroupTypeNId
        /// </summary>
        /// <param name="subgroupTypeNIds">Comma separated NIds</param>
        /// <returns></returns>
        public string GetAllAssociatedSubgroupNIds(string subgroupTypeNIds)
        {
            string RetVal = string.Empty;
            DataTable Table;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroup(FilterFieldType.Type, subgroupTypeNIds));

                foreach (DataRow Row in Table.Rows)
                {
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal += ",";
                    }

                    RetVal += Row[Subgroup.SubgroupNId].ToString();
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the list of subgroup dimensions which are associated with the given subgroupval NId
        /// </summary>
        /// <param name="subgroupValNId"></param>
        /// <returns></returns>
        public List<DI6SubgroupInfo> GetAssocaitedSubgroupsInfo(int subgroupValNId)
        {
            List<DI6SubgroupInfo> RetVal = new List<DI6SubgroupInfo>();
            DataTable SVSTable; // Subgroup_Val_Subgroup
            int SubgroupNId = 0;

            try
            {
                SVSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsSubgroup(subgroupValNId.ToString(), string.Empty));

                foreach (DataRow SVSRow in SVSTable.Rows)
                {
                    SubgroupNId = Convert.ToInt32(SVSRow[SubgroupValsSubgroup.SubgroupNId]);
                    RetVal.Add(this.GetSubgroupInfo(FilterFieldType.NId, SubgroupNId.ToString()));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }


            return RetVal;
        }

        /// <summary>
        /// Updates subgroup
        /// </summary>
        /// <param name="NId"></param>
        /// <param name="subgroup"></param>
        /// <param name="isGlobal"></param>
        /// <param name="GId"></param>
        /// <param name="type"></param>
        public void UpdateSubgroup(int NId, string subgroup, bool isGlobal, string GId, int type)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroup, GId, isGlobal, type, NId);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Updates subgroup with subgroup order
        /// </summary>
        /// <param name="NId"></param>
        /// <param name="subgroup"></param>
        /// <param name="isGlobal"></param>
        /// <param name="GId"></param>
        /// <param name="type"></param>
        /// <param name="order"></param>
        public void UpdateSubgroup(int NId, string subgroup, bool isGlobal, string GId, int type,int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroup, GId, isGlobal, type, NId, order);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Updates Subgroup Order into subgroup_order column
        /// </summary>
        /// <param name="NId"></param>      
        /// <param name="order"></param>
        public void UpdateSubgroupByOrder(int NId,int order)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Update.UpdateSubgroupOrderByNId(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode,NId, order);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        /// <summary>
        /// Deletes subgroup and associated records from SubgroupVal, SubgroupValSubgroup, Indicator_Unit_Subgroup and 
        /// </summary>
        /// <param name="NIds"></param>
        public void DeleteSubgroup(string NIds)
        {
            string SqlQuery = string.Empty;
            DITables TablesName;
            DI6SubgroupValBuilder SGValBuilder;
            string AssociatedSubgroupValNIds = string.Empty;

            try
            {

                // Step 1: Delete subgroup
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TablesName = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Delete.DeleteSubgroups(TablesName.Subgroup, NIds);
                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }

                // Step 2: Delete associated SubgroupVal which will automatically delete associated records from SubgroupValSubgroup, IUS and IC_IUS
                SGValBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
                AssociatedSubgroupValNIds = SGValBuilder.GetAllAssociatedSubgroupValNIds(NIds);

                if (!string.IsNullOrEmpty(AssociatedSubgroupValNIds))
                {
                    SGValBuilder.DeleteSubgroupVals(AssociatedSubgroupValNIds);
                }

            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// To Import a Subgroup into template or database
        /// </summary>
        /// <param name="subgroupInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroup(DI6SubgroupInfo subgroupInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            int NIDByName = 0;
            int SGTypeNIdInTargetDB = 0;
            bool SkipRecord = false;
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupTypeInfo SourceSGTypeInfo = new DI6SubgroupTypeInfo();
            DI6SubgroupInfo TargetSGInfo;

            try
            {
                //check  already exists in database or not

                //Get NId by Name
                if (!string.IsNullOrEmpty(subgroupInfo.GID))
                {
                    //first check by gid and then by name
                    RetVal = this.GetNidByGID(subgroupInfo.GID);

                    if (RetVal > 0)
                    {
                        // check for the duplicacy by name
                        NIDByName = this.GetSubgroupByName(subgroupInfo.Name);
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

                    RetVal = this.GetSubgroupByName(subgroupInfo.Name);
                }


                if (!SkipRecord)
                {

                    // get subgroupTypeInfo from source database
                    SourceSGTypeInfo = (new DI6SubgroupTypeBuilder(sourceDBConnection, sourceQurey)).GetSubgroupTypeInfoByNid(subgroupInfo.Type);


                    // import SubgroupType
                    SGTypeNIdInTargetDB = SGTypeBuilder.ImportSubgroupType(SourceSGTypeInfo, SourceSGTypeInfo.Nid, sourceQurey, sourceDBConnection);


                    //insert or update subgroup record
                    if (RetVal > 0)
                    {
                        // get target subgroup info
                        TargetSGInfo = this.GetSubgroupInfo(FilterFieldType.NId, RetVal.ToString());
                        // Dont update if target is global and source is local
                        if (!(subgroupInfo.Global == false & TargetSGInfo.Global))
                        {
                            try
                            {
                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupInfo.Name, subgroupInfo.GID, subgroupInfo.Global, SGTypeNIdInTargetDB, RetVal));
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
                        subgroupInfo.Type = SGTypeNIdInTargetDB;

                        if (this.InsertIntoDatabase(subgroupInfo))
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
        /// To Import  Subgroup information from mapped subgroup
        /// </summary>
        /// <param name="subgroupInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidInTrgDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportSubgroupFrmMappedSubgroup(DI6SubgroupInfo subgroupInfo, int NidInSourceDB, int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            int SGTypeNIdInTargetDB = 0;
            bool SkipRecord = false;
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupTypeInfo SourceSGTypeInfo = new DI6SubgroupTypeInfo();
            DI6SubgroupInfo TargetSGInfo;

            try
            {

                // set RetVal to NidInTrgDB
                RetVal = NidInTrgDB;



                if (!SkipRecord)
                {

                    // get subgroupTypeInfo from source database
                    SourceSGTypeInfo = (new DI6SubgroupTypeBuilder(sourceDBConnection, sourceQurey)).GetSubgroupTypeInfoByNid(subgroupInfo.Type);


                    // import SubgroupType
                    SGTypeNIdInTargetDB = SGTypeBuilder.ImportSubgroupType(SourceSGTypeInfo, SourceSGTypeInfo.Nid, sourceQurey, sourceDBConnection);


                    //insert or update subgroup record
                    if (RetVal > 0)
                    {
                        // get target subgroup info
                        TargetSGInfo = this.GetSubgroupInfo(FilterFieldType.NId, RetVal.ToString());

                        // Dont update if target is global and source is local
                        if (!(subgroupInfo.Global == false & TargetSGInfo.Global))
                        {
                            try
                            {
                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupInfo.Name, subgroupInfo.GID, subgroupInfo.Global, SGTypeNIdInTargetDB, RetVal));
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
                        subgroupInfo.Type = SGTypeNIdInTargetDB;

                        if (this.InsertIntoDatabase(subgroupInfo))
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
        /// Returns true/false. Ture if the given type is associated with other subgroup types.
        /// </summary>
        /// <param name="subgroupNId"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public bool IsSGTypeAssociatedWOtherSubgroups(int subgroupNId, int newType)
        {
            bool RetVal = true;

            try
            {
                if (Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Subgroup.IsSGTypeAssociatedWOtherSubgroups(subgroupNId, newType))) > 0)
                {
                    RetVal = true;
                }
                else
                {
                    RetVal = false;
                }
            }
            catch (Exception ex)
            {
                RetVal = true;
                throw new ApplicationException(ex.Message);
            }
            return RetVal;

        }

        /// <summary>
        /// Import Subgroup: by SubgroupGID and Name
        /// </summary>
        /// <param name="subgroupName"></param>
        /// <param name="subgroupGID"></param>
        /// <param name="subgroupType"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public int ImportSubgroup(string subgroupName,string subgroupGID,string subgroupTypeName,bool isGlobal)
        {
            int RetVal = -1;
            int NIDByName = 0;
            int SGTypeNIdInTargetDB = 0;
            bool SkipRecord = false;
            DI6SubgroupTypeBuilder SGTypeBuilder = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupTypeInfo SourceSGTypeInfo = new DI6SubgroupTypeInfo();
            DI6SubgroupInfo TargetSGInfo;
            DI6SubgroupInfo SubgroupInfoObj=new DI6SubgroupInfo();

            try
            {
                SubgroupInfoObj.Name = subgroupName;
                SubgroupInfoObj.GID = subgroupGID;
                SubgroupInfoObj.Global = isGlobal;

                //check  already exists in database or not

                //Get NId by Name
                if (!string.IsNullOrEmpty(SubgroupInfoObj.GID))
                {
                    //first check by gid and then by name
                    RetVal = this.GetNidByGID(SubgroupInfoObj.GID);

                    if (RetVal > 0)
                    {
                        // check for the duplicacy by name
                        NIDByName = this.GetSubgroupByName(SubgroupInfoObj.Name);
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

                    RetVal = this.GetSubgroupByName(SubgroupInfoObj.Name);
                }

                if (!SkipRecord)
                {
                    //-- PreRequisite to Import SubgroupType First
                    // get subgroupTypeInfo from source database
                    SourceSGTypeInfo.Name = subgroupTypeName;
                    SGTypeNIdInTargetDB = (new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries)).CheckNCreateSubgroupType(SourceSGTypeInfo);
                                       
                    //insert or update subgroup record
                    if (RetVal > 0)
                    {
                        // get target subgroup info
                        TargetSGInfo = this.GetSubgroupInfo(FilterFieldType.NId, RetVal.ToString());
                        // Dont update if target is global and source is local
                        if (!(SubgroupInfoObj.Global == false & TargetSGInfo.Global))
                        {
                            try
                            {
                                //update the gid,name and global on the basis of nid
                                this.DBConnection.ExecuteNonQuery(DALQueries.Subgroup.Update.UpdateSubgroupByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, SubgroupInfoObj.Name, SubgroupInfoObj.GID, SubgroupInfoObj.Global, SGTypeNIdInTargetDB, RetVal));
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
                        SubgroupInfoObj.Type = SGTypeNIdInTargetDB;

                        if (this.InsertIntoDatabase(SubgroupInfoObj))
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
