using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build unit record and insert it into database.
    /// </summary>
    public class UnitBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Add unit record into collection
        /// </summary>
        /// <param name="unitRecord">object of UnitInfo</param>

        private void AddUnitIntoCollection(UnitInfo unitInfo)
        {
            if (!this.UnitCollection.ContainsKey(unitInfo.Name))
            {
                this.UnitCollection.Add(unitInfo.Name, unitInfo);
            }

        }
        /// <summary>
        /// Check Unit in collection
        /// </summary>
        /// <param name="unitName">Unit Name</param>
        /// <returns>Unit Nid</returns>
        private int CheckUnitInCollection(string unitName)
        {
            int RetVal = 0;
            try
            {
                if (this.UnitCollection.ContainsKey(unitName))
                {
                    RetVal = this.UnitCollection[unitName].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        /// <summary>
        /// Get unit Nid by unit Name
        /// </summary>
        /// <returns>Unit Nid</returns>
        private int GetNidByName(string name)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Unit.GetUnit(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery(name) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Get unit Nid from unit by unit GId.
        /// </summary>
        /// <returns></returns>
        private int GetNidByGID(string GID)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.Unit.GetUnit(FilterFieldType.GId, "'" + DIQueries.RemoveQuotesForSqlQuery(GID) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Properties --"

        /// <summary>
        /// Returns unit colleciton in key,pair format. Key is unit name and value is Object of UnitInfo.
        /// </summary>
        internal Dictionary<string, UnitInfo> UnitCollection = new Dictionary<string, UnitInfo>();

        #endregion
        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public UnitBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of unit first in collection Then In database
        /// </summary>
        /// <param name="unitRecord">object of UnitInfo</param>
        /// <returns>Unit Nid</returns>
        public int CheckUnitExists(UnitInfo unitInfo)
        {
            int RetVal = 0;

            //Step 1: check unit exists in Unit collection
            RetVal = this.CheckUnitInCollection(unitInfo.Name);

            //Step 2: check unit exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetUnitNid(unitInfo.GID, unitInfo.Name);
            }

            return RetVal;
        }

        /// <summary>
        /// Check and create Unit record
        /// </summary>
        /// <param name="unitRecord">object of UnitInfo </param>
        /// <returns>Unit Nid</returns>
        public int CheckNCreateUnit(UnitInfo unitInfo)
        {
            int RetVal = 0;

            try
            {
                // check unit exists or not
                RetVal = this.CheckUnitExists(unitInfo);

                // if unit does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert unit
                    if (this.InsertIntoDatabase(unitInfo))
                    {
                        RetVal = this.GetNidByName(unitInfo.Name);
                    }

                }

                // add indicator information into collection
                unitInfo.Nid = RetVal;
                this.AddUnitIntoCollection(unitInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get unit nid. Pass either GId or name and ofcourse you can pass both.
        /// </summary>
        /// <param name="unitGid">Unit GID </param>
        /// <param name="unitName">Name of the unit. </param>
        /// <returns>Unit Nid</returns>
        public int GetUnitNid(string unitGid, string unitName)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(unitGid))
            {
                RetVal = this.GetNidByGID(unitGid);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(unitName))
                {
                    RetVal = this.GetNidByName(unitName);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// To Import a unit into template or database
        /// </summary>
        /// <param name="unitInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportUnit(UnitInfo unitInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            UnitInfo TrgUnitInfo;

            try
            {
                //check unit already exists in database or not
                RetVal = this.GetUnitNid(unitInfo.GID, unitInfo.Name);

                if (RetVal > 0)
                {
                    TrgUnitInfo = this.GetUnitInfo(FilterFieldType.NId, RetVal.ToString());

                    // dont import if target is global but source is local
                    if (TrgUnitInfo.Global & unitInfo.Global == false)
                    {
                        // dont import if target is global but source is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.Unit.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, unitInfo.Name, unitInfo.GID, unitInfo.Global, RetVal));
                    }

                }
                else
                {
                    if (this.InsertIntoDatabase(unitInfo))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }

                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Unit, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// To import unit information from mapped unit 
        /// </summary>
        /// <param name="unitInfo"></param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidInTrgDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportMappedUnitInformation(UnitInfo unitInfo, int NidInSourceDB, int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            UnitInfo TrgUnitInfo;

            try
            {
                //set RetVal to NidInTrgDB
                RetVal = NidInTrgDB;

                if (RetVal > 0)
                {
                    TrgUnitInfo = this.GetUnitInfo(FilterFieldType.NId, RetVal.ToString());

                    // dont import if target is global but source is local
                    if (TrgUnitInfo.Global & unitInfo.Global == false)
                    {
                        // dont import if target is global but source is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.Unit.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, unitInfo.Name, unitInfo.GID, unitInfo.Global, RetVal));
                    }

                }

                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.Unit, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// Import Unit: check and update UnitName,UnitGID
        /// </summary>
        /// <param name="unitGid"></param>
        /// <param name="unitName"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public int ImportUnit(string unitGid, string unitName, bool isGlobal)
        {
            int RetVal = -1;
            UnitInfo TrgUnitInfo = null;
            UnitInfo UnitInfoObj = new UnitInfo();
            string LangCode = this.DBQueries.LanguageCode;

            try
            {
                UnitInfoObj.GID = unitGid;
                UnitInfoObj.Name = unitName;
                UnitInfoObj.Global = isGlobal;

                //check unit already exists in database or not
                RetVal = this.GetUnitNid(UnitInfoObj.GID, UnitInfoObj.Name);

                if (RetVal > 0)
                {
                    if (!this.DBQueries.LanguageCode.StartsWith("_"))
                    {
                        LangCode = "_" + LangCode;
                    }

                    TrgUnitInfo = this.GetUnitInfo(FilterFieldType.NId, RetVal.ToString());

                    // dont import if target is global but source is local
                    if (TrgUnitInfo.Global & UnitInfoObj.Global == false)
                    {
                        // dont import if target is global but source is local
                    }
                    else
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.Unit.Update.UpdateByNid(this.DBQueries.DataPrefix, LangCode, UnitInfoObj.Name, UnitInfoObj.GID, UnitInfoObj.Global, RetVal));
                    }

                }
                else
                {
                    if (this.InsertIntoDatabase(UnitInfoObj))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
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
        /// Returns instance of UnitInfo.
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public UnitInfo GetUnitInfo(FilterFieldType filterClause, string filterText)
        {
            string Query = string.Empty;
            UnitInfo RetVal = new UnitInfo();
            DataTable UnitTable;
            try
            {
                Query = this.DBQueries.Unit.GetUnit(filterClause, filterText);
                UnitTable = this.DBConnection.ExecuteDataTable(Query);

                //set unit info
                if (UnitTable != null)
                {
                    if (UnitTable.Rows.Count > 0)
                    {
                        RetVal.GID = UnitTable.Rows[0][Unit.UnitGId].ToString();
                        RetVal.Name = UnitTable.Rows[0][Unit.UnitName].ToString();
                        RetVal.Nid = Convert.ToInt32(UnitTable.Rows[0][Unit.UnitNId].ToString());
                        RetVal.Global = Convert.ToBoolean(UnitTable.Rows[0][Unit.UnitGlobal]);
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
        /// Deletes units and associated records from IUS a IC_IUS table table
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteUnit(string nids)
        {
            DITables TableNames;
            IUSBuilder IUSBuilder;
            string AssocicatedIUSNIds = string.Empty;
            try
            {

                IUSBuilder = new IUSBuilder(this.DBConnection, this.DBQueries);

                // Step 1: Delete records from Unit table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Unit.Delete.DeleteUnits(TableNames.Unit, nids));
                }

                // Step 2: Delete records from IUS table

                // Step2(a): Get all associated IUSNIds
                AssocicatedIUSNIds = IUSBuilder.GetAllAssociatedIUSNids(string.Empty, nids, string.Empty);

                // Step2(b): Delete all associated IUSNIds
                IUSBuilder.DeleteIUS(AssocicatedIUSNIds);


            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }


        }

        /// <summary>
        /// Insert unit record into Unit table
        /// </summary>
        /// <param name="unitRecord">object of UnitInfo </param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        public bool InsertIntoDatabase(UnitInfo unitInfo)
        {
            bool RetVal = false;
            string unitName = unitInfo.Name;
            string unitGID = unitInfo.GID;
            string UnitGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string UnitForDatabase = string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(unitGID))
                {
                    UnitGId = unitGID;
                }

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        UnitForDatabase = unitName;
                    }
                    else
                    {
                        UnitForDatabase = Constants.PrefixForNewValue + unitName;
                    }
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Unit.Insert.InsertUnit(this.DBQueries.DataPrefix, "_" + LanguageCode, UnitForDatabase, UnitGId, unitInfo.Global, this.DBConnection.ConnectionStringParameters.ServerType));

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
        /// Updates the unit information into database on the basis of NId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="GId"></param>
        /// <param name="isGlobal"></param>
        /// <param name="NId"></param>
        public void UpdateUnit(string name, string GId, bool isGlobal,int NId)
        {
            string SqlQuery = string.Empty;
            SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Unit.Update.UpdateByNid(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, name, GId, isGlobal, NId);

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
        /// Returns true if unit already exists otherwise false. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nid">send -99 if unit is new otherwise send unit nid </param>
        /// <returns></returns>
        public bool IsUnitAlreadyExists(string name, int nid)
        {
            bool RetVal = false;
            int FetchedNid = 0;

            // get nid from database
            FetchedNid = this.GetNidByName(name);


            if (nid > 0)
            {
                // check if nid is given 
                if (FetchedNid == nid)
                {
                    RetVal = false;
                }
                else if (FetchedNid > 0)
                {
                    RetVal = true;
                }
            }
            else if (FetchedNid > 0)
            {
                RetVal = true;
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}
