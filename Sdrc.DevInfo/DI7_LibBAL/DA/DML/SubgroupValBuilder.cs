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
////    /// <summary>
////    /// Build subgroupval record according to subgroupval and subgroup information and insert it into dadabase.   
////    /// </summary>
////    public class SubgroupValBuilder
////    {
////        #region "-- Private --"

////        #region "-- Variables --"

////        private DIConnection DBConnection;
////        private DIQueries DBQueries;
////        #endregion
////        #region "-- Methods --"
////        /// <summary>
////        /// Check existance of subgroup_val first in collection then in database. 
////        /// </summary>
////        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
////        /// <returns>SubgroupVal Nid</returns>
////        private int ChecksubgroupValExists(SubgroupValInfo subgroupValInfo)
////        {
////            int RetVal = 0;

////            //Step 1: check subgroupVal exists in subgroupVal collection
////            RetVal = this.CheckSubgroupValInCollection(subgroupValInfo.Name);

////            //Step 2: check subgroupVal exists in database.
////            if (RetVal <= 0)
////            {
////                RetVal = this.GetSubgroupValNid(subgroupValInfo.GID, subgroupValInfo.Name);
////            }

////            return RetVal;
////        }
////        /// <summary>
////        /// Add subgroupval in collection. 
////        /// </summary>
////        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
////        private void AddSubgroupValIntoCollection(SubgroupValInfo subgroupValInfo)
////        {
////            if (!this.SubgroupValCollection.ContainsKey(subgroupValInfo.Name))
////            {
////                this.SubgroupValCollection.Add(subgroupValInfo.Name, subgroupValInfo);
////            }
////        }
////        /// <summary>
////        /// check existance of subgroupval in collection 
////        /// </summary>
////        /// <param name="name">name </param>
////        /// <returns>SubgroupVal Nid</returns>
////        private int CheckSubgroupValInCollection(string name)
////        {
////            int RetVal = 0;
////            try
////            {
////                if (this.SubgroupValCollection.ContainsKey(name))
////                {
////                    RetVal = this.SubgroupValCollection[name].Nid;
////                }
////            }
////            catch (Exception)
////            {
////                RetVal = 0;
////            }
////            return RetVal;
////        }

////        private int CheckNImportSubgroup(SubgroupInfo subgroupInfo, DIQueries queries, DIConnection dbConnection)
////        {
////            int RetVal = 0;
////            SubgroupBuilder SubgroupDBBuilder;

////            //import subgroup and get new nid
////            if (subgroupInfo.Nid > 0)
////            {
////                SubgroupDBBuilder = new SubgroupBuilder(this.DBConnection, this.DBQueries);
////                RetVal = SubgroupDBBuilder.ImportSubgroup(subgroupInfo, subgroupInfo.Nid, queries, dbConnection);
////            }

////            return RetVal;
////        }

////        /// <summary>
////        /// Get subgroupval Nid by name 
////        /// </summary>
////        ///<param name="name">SubgroupVal Name</param>
////        /// <returns>SubgroupVal Nid</returns>
////        private int GetNidByName(string name)
////        {
////            int RetVal = 0;
////            string SqlQuery = string.Empty;
////            try
////            {
////                SqlQuery = this.DBQueries.Subgroup.GetSubgroupVals(FilterFieldType.Name, "'" + name + "'");
////                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
////            }
////            catch (Exception)
////            {
////                RetVal = 0;
////            }
////            return RetVal;
////        }

////        /// <summary>
////        /// Get nid only if GID exists in the database
////        /// </summary>
////        ///<param GID="GID">SubgroupVal GId</param>
////        /// <returns>SubgroupVal Nid</returns>
////        private int GetNidByGID(string GID)
////        {
////            string SqlQuery = string.Empty;
////            int RetVal = 0;
////            try
////            {
////                SqlQuery = this.DBQueries.Subgroup.GetSubgroupVals(FilterFieldType.GId, "'" + GID + "'");
////                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
////            }
////            catch (Exception)
////            {
////                RetVal = 0;
////            }
////            return RetVal;
////        }
////        /// <summary>
////        /// Insert subgroupval record into database. 
////        /// </summary>
////        /// <param name="subgroupVal">Subgroupval Name</param>
////        /// <param name="subgroupValGId">Subgroupval GID</param>
////        /// <param name="AgeNId">Age Nid</param>
////        /// <param name="SexNId">Sex Nid</param>
////        /// <param name="LocationNid">Location Nid</param>
////        /// <param name="OthersNid">Other Nid</param>
////        /// <param name="isGlobal">True/false</param>
////        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
////        private bool InsertIntoDatabase(string subgroupVal, string subgroupValGId, int AgeNId, int SexNId, int LocationNid, int OthersNid, bool isGlobal)
////        {
////            bool RetVal = false;
////            string SubgroupValGId = Guid.NewGuid().ToString();
////            string LanguageCode = string.Empty;
////            string DefaultLanguageCode = string.Empty;
////            string SubgroupValForDatabase = string.Empty;

////            try
////            {
////                DefaultLanguageCode = this.DBQueries.LanguageCode;

////                //replace gid only if given
////                if (!string.IsNullOrEmpty(subgroupValGId))
////                {
////                    SubgroupValGId = subgroupValGId;
////                }


////                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
////                {
////                    LanguageCode = languageRow[Language.LanguageCode].ToString();
////                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
////                    {
////                        SubgroupValForDatabase = subgroupVal;
////                    }
////                    else
////                    {
////                        SubgroupValForDatabase = Constants.PrefixForNewValue + subgroupVal;
////                    }
////                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Insert.InsertSubgroupVal(this.DBQueries.DataPrefix, "_" 
////                        + LanguageCode, SubgroupValForDatabase, 
////                        SubgroupValGId,isGlobal , AgeNId, SexNId, LocationNid, OthersNid));     
////                    }
////                RetVal = true;
////            }
////            catch (Exception)
////            {
////                RetVal = false;
////            }

////            return RetVal;
////        }

////        /// <summary>
////        /// Insert subgroupval record into database 
////        /// </summary>
////        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
////        /// <returns></returns>
////        private bool InsertIntoDatabase(SubgroupValInfo subgroupValInfo)
////        {
////            return this.InsertIntoDatabase(subgroupValInfo.Name, subgroupValInfo.GID,
////                subgroupValInfo.Age.Nid, subgroupValInfo.Sex.Nid,
////                subgroupValInfo.Location.Nid, subgroupValInfo.Others.Nid,subgroupValInfo.Global);
////        }


////        #endregion

////        #endregion

////        #region"--Internal--"
////        #region "-- Variables & Properties --"

////        /// <summary>
////        /// Returns Subgroup Val colleciton in key,pair format. Key is subgroupVal name and value is Object of SubgroupValInfo.
////        /// </summary>
////        internal Dictionary<string, SubgroupValInfo> SubgroupValCollection = new Dictionary<string, SubgroupValInfo>();

////         #endregion
////        #endregion
        
////        #region "-- Public --"

////        #region "-- New/Dispose --"

////        public SubgroupValBuilder(DIConnection connection, DIQueries queries)
////        {
////            this.DBConnection = connection;
////            this.DBQueries = queries;
////        }

////        #endregion

////        #region "-- Methods --"
////        /// <summary>
////        /// Check subgroupval record into database if false create subgroupval record  
////        /// </summary>
////        /// <param name="subgroupValRecord">object of SubgroupValInfo</param>
////        /// <returns>SubgroupVal Nid</returns>
////        public int CheckNCreateSubgroupVal(SubgroupValInfo subgroupValInfo)
////        {
////            int RetVal = 0;
            
////            try
////            {
////                // check subgroupVal exists or not
////                RetVal = this.ChecksubgroupValExists(subgroupValInfo);

////                // if subgroupVal does not exist then create it.
////                if (RetVal <= 0)
////                {
////                     // insert subgroupVal
////                    if (this.InsertIntoDatabase(subgroupValInfo))
////                    {
////                        RetVal = this.GetNidByName(subgroupValInfo.Name);
////                    }
////                 }

////                // add subgroupVal information into collection
////                subgroupValInfo.Nid = RetVal;
////                this.AddSubgroupValIntoCollection(subgroupValInfo);
////            }
////            catch (Exception)
////            {
////                RetVal = 0;
////            }
                        
////            return RetVal;
////        }


////        /// <summary>
////        /// To Import a SubgroupVal into template or database. 
////        /// </summary>
////        /// <param name="subgroupValInfo"></param>
////        /// <param name="NidInSourceDB"></param>
////        /// <param name="sourceQurey"></param>
////        /// <param name="sourceDBConnection"></param>
////        /// <returns></returns>
////        public int ImportSubgroupVal(SubgroupValInfo subgroupValInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
////        {
////            int RetVal = -1;
                     

////            try
////            {
////                //check already exists in database or not
////                RetVal = this.GetSubgroupValNid(subgroupValInfo.GID, subgroupValInfo.Name);

////                if (RetVal > 0)
////                {
////                    // if source item is global
////                    if (subgroupValInfo.Global)
////                    {
////                        //update the gid,name and global on the basis of nid
////                        DALQueries.Subgroup.Update.UpdateSubgroupVal(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, subgroupValInfo.Name, subgroupValInfo.GID, subgroupValInfo.Global, RetVal);
////                    }

////                }
////                else
////                {
////                    //get subgroups : age, sex, location and others
////                    subgroupValInfo.Age = SubgroupBuilder.GetSubgroupInfoByNid(sourceQurey, sourceDBConnection, subgroupValInfo.Age.Nid);
////                    subgroupValInfo.Sex = SubgroupBuilder.GetSubgroupInfoByNid(sourceQurey, sourceDBConnection, subgroupValInfo.Sex.Nid);
////                    subgroupValInfo.Others = SubgroupBuilder.GetSubgroupInfoByNid(sourceQurey, sourceDBConnection, subgroupValInfo.Others.Nid);
////                    subgroupValInfo.Location = SubgroupBuilder.GetSubgroupInfoByNid(sourceQurey, sourceDBConnection, subgroupValInfo.Location.Nid);

////                    //reset subgroup nids
////                    subgroupValInfo.Age.Nid= this.CheckNImportSubgroup(subgroupValInfo.Age,sourceQurey,sourceDBConnection);
////                    subgroupValInfo.Sex.Nid = this.CheckNImportSubgroup(subgroupValInfo.Sex, sourceQurey, sourceDBConnection);
////                    subgroupValInfo.Location.Nid = this.CheckNImportSubgroup(subgroupValInfo.Location, sourceQurey, sourceDBConnection);
////                    subgroupValInfo.Others.Nid = this.CheckNImportSubgroup(subgroupValInfo.Others,sourceQurey,sourceDBConnection);


////                    //insert new subgroupval
////                    if (this.InsertIntoDatabase(subgroupValInfo))
////                    {
////                        //get nid
////                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
////                    }
////                }


////            }
////            catch (Exception ex)
////            {
////                throw new ApplicationException(ex.Message);
////            }

////            return RetVal;
////        }
        
////        /// <summary>
////        /// Get subgroupval Nid. Pass either GId or name and ofcourse you can pass both.   
////        /// </summary>
////        /// <param name="GID">Subgroup Val GID </param>
////        /// <param name="name">SubgroupVal Name</param>
////        /// <returns>SubgroupVal Nid</returns>
////        public int GetSubgroupValNid(string GID, string name)
////        {
////            int RetVal = 0;
////            string SqlQuery = string.Empty;

////            //--step1:Get Nid by GID if GID is not empty
////            if (!string.IsNullOrEmpty(GID))
////            {
////                RetVal = this.GetNidByGID(GID);
////            }

////            //--step2:Get Nid by Name if name is not empty
////            if (RetVal <= 0)
////            {
////                if (!string.IsNullOrEmpty(name))
////                {
////                    RetVal = this.GetNidByName(name);
////                }
////            }

////            return RetVal;
////        }

////        /// <summary>
////        /// Returns instance of SubgrouopValInfo
////        /// </summary>
////        /// <param name="filterClause"></param>
////        /// <param name="filterText"></param>
////        /// <param name="selectionType"></param>
////        /// <returns></returns>
////        public SubgroupValInfo GetSubgroupValInfo(FilterFieldType filterClause, string filterText)
////        {
////            string Query = string.Empty;
////            SubgroupValInfo RetVal = new SubgroupValInfo();
////            DataTable SubgroupTable;
////            try
////            {
                
////                Query = this.DBQueries.Subgroup.GetSubgroupVals(filterClause, filterText);
////                SubgroupTable = this.DBConnection.ExecuteDataTable(Query);

////                // set SubgroupVal info
////                if (SubgroupTable != null)
////                {
////                    if (SubgroupTable.Rows.Count > 0)
////                    {
////                        RetVal.GID = SubgroupTable.Rows[0][SubgroupVals.SubgroupValGId].ToString();
////                        RetVal.Global = Convert.ToBoolean(SubgroupTable.Rows[0][SubgroupVals.SubgroupValGlobal].ToString());
////                        RetVal.Name = SubgroupTable.Rows[0][SubgroupVals.SubgroupVal].ToString();
////                        RetVal.Nid = Convert.ToInt32(SubgroupTable.Rows[0][SubgroupVals.SubgroupValNId].ToString());
////                    }
////                }
////            }
////            catch (Exception)
////            {
////                RetVal = null;
////            }
////            return RetVal;
////        }

////        /// <summary>
////        /// Returns all associated subgroupValNId  
////        /// </summary>
////        /// <param name="subgroupNids"></param>
////        public string GetAllAssociatedSubgroupValNids(string subgroupNids)
////        {
////            string RetVal = string.Empty;
////            DataTable SubgroupValsTable;
////            string FilterString = string.Empty;
            
////            if (!string.IsNullOrEmpty(subgroupNids))
////            {
////                try
////                {
                    
////                    // select rows from subgroup_vals where [subgroup_type] = iSubgroup_nid 
////                    FilterString = SubgroupVals.SubgroupValSex + " in (" + subgroupNids + ")" + " or " + SubgroupVals.SubgroupValAge + " in (" + subgroupNids + ") " + " or " + SubgroupVals.SubgroupValOthers + " in (" + subgroupNids + ")" + " or " + SubgroupVals.SubgroupValLocation + " in (" + subgroupNids + ") ";

////                    SubgroupValsTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroupVals(FilterFieldType.Search, FilterString));


////                    //get SubgroupValNIds from Subgroup table 
////                    foreach (DataRow Row in SubgroupValsTable.Rows)
////                    {
////                        if (string.IsNullOrEmpty(RetVal))
////                        {
////                            RetVal = Row[SubgroupVals.SubgroupValNId].ToString();
////                        }
////                        else
////                        {
////                            RetVal = RetVal + "," + Row[SubgroupVals.SubgroupValNId].ToString();
////                        }                        
////                    }
////                }
////                catch (Exception ex)
////                {
////                    RetVal = string.Empty;
////                    throw new ApplicationException(ex.ToString());                    
////                }
////            }

////            return RetVal;
////        }

////        /// <summary>
////        /// Call this function to update subgroupVals 
////        /// </summary>
////        /// <param name="subgroupNid"></param>
////        /// <param name="oldSubgroupVal"></param>
////        /// <param name="type"></param>
////        /// <param name="totalSubgroupText">Language value of "Total" for total subgroup </param>
////        public void UpdateSubgroupVals(string subgroupNid, string oldSubgroupVal, SubgroupType type,string totalSubgroupText)
////        {
////            //update subgroup_Vals whenever subgroup is changed 
////            DataTable SubgroupValsTable;
////            DataRow[] rows;
////            int SexNid;
////            int AgeNid;
////            int LocationNid;
////            int OthersNid;
////            string Sex = string.Empty;
////            string Age = string.Empty;
////            string Location = string.Empty;
////            string Others = string.Empty;
////            string OldSubgroupVals = string.Empty;
////            string FilterString=string.Empty;
////          string SqlString=string.Empty;
////            SubgroupBuilder Subgroups;
            

////            if (!string.IsNullOrEmpty(subgroupNid))
////            {
////                try
////                {
////                    Subgroups = new SubgroupBuilder(this.DBConnection, this.DBQueries);

////                    //-- Step1: select rows from subgroup_vals where [subgroup_type] = iSubgroup_nid 
////                    FilterString = SubgroupVals.SubgroupValSex + " in (" + subgroupNid + ")" + " or " + SubgroupValRemovedColumns.SubgroupValAge + " in (" + subgroupNid + ") " + " or " + SubgroupValRemovedColumns.SubgroupValOthers + " in (" + subgroupNid + ")" + " or " + SubgroupValRemovedColumns.SubgroupValLocation + " in (" + subgroupNid + ") ";

////                    SubgroupValsTable = this.DBConnection.ExecuteDataTable( this.DBQueries.Subgroup.GetSubgroupVals(FilterFieldType.Search, FilterString));
                        

////                    //-- Step2: get values from Subgroup table 
////                    foreach (DataRow Row in SubgroupValsTable.Rows)
////                    {
////                        SexNid = Convert.ToInt32(Row[SubgroupValRemovedColumns.SubgroupValSex]);
////                        AgeNid = Convert.ToInt32(Row[SubgroupValRemovedColumns.SubgroupValAge]);
////                        OthersNid = Convert.ToInt32(Row[SubgroupValRemovedColumns.SubgroupValOthers]);
////                        LocationNid = Convert.ToInt32(Row[SubgroupValRemovedColumns.SubgroupValLocation]); ;
////                        Sex = string.Empty;
////                        Age = string.Empty;
////                        Location = string.Empty;
////                        Others = string.Empty;

////                        if (SexNid > 0)
////                        {
////                            Sex =Subgroups.GetSubgroupInfo(FilterFieldType.NId, SexNid.ToString()).Name;
////                        }

////                        if (AgeNid > 0)
////                        {
////                            Age = Subgroups.GetSubgroupInfo(FilterFieldType.NId, AgeNid.ToString()).Name;
////                        }

////                        if (LocationNid > 0)
////                        {
////                            Location = Subgroups.GetSubgroupInfo(FilterFieldType.NId, LocationNid.ToString()).Name;
////                         }

////                        if (OthersNid > 0)
////                        {
////                            Others = Subgroups.GetSubgroupInfo(FilterFieldType.NId, OthersNid.ToString()).Name;
////                        }

////                        //-- order: TOTAL +( Location & Sex & Age & other) 
                                            

////                        //-- Step3: check subgroup_vals created with "TOTAL" or not.If created, then add total 
////                        OldSubgroupVals = Row[SubgroupVals.SubgroupVal].ToString();
////                        string str = SubgroupValBuilder.CreateSubgroupValsText(Sex, Others, Location, Age, type, oldSubgroupVal);
////                        if (str.Length == 0)
////                        {
////                            str = totalSubgroupText;
////                        }
////                        else
////                        {
////                            str = totalSubgroupText + " " + str;
////                        }



////                        //-- new subgroup_vals 
////                        string sSubgroup_Vals = SubgroupValBuilder.CreateSubgroupValsText(Sex, Others, Location, Age, SubgroupType.None, string.Empty);

////                        if (OldSubgroupVals.ToUpper() == str.ToUpper())
////                        {
////                            //-- check "Total" exists in subgroup_vals 
////                            sSubgroup_Vals = totalSubgroupText + " " + sSubgroup_Vals;
////                        }

////                        //-- Step4: update subgroup_Vals in subgroup_Vals table 

////                        sSubgroup_Vals = sSubgroup_Vals.Trim();
////                       SqlString=DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Update.UpdateSubgroupVal(
////                           this.DBQueries.DataPrefix,
////                           this.DBQueries.LanguageCode,DICommon.RemoveQuotes(sSubgroup_Vals),
////                           Row[SubgroupVals.SubgroupValGId].ToString(),
////                           Convert.ToBoolean(Row[SubgroupVals.SubgroupValGlobal]),
////                           Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]));
                   
////                        this.DBConnection.ExecuteNonQuery(SqlString);
                            
////                    }
////                }
////                catch (Exception ex)
////                {
////                    throw new ApplicationException(ex.ToString());
////                }
////            }
////        }


////        /// <summary>
////        /// Deletes subgroupVals and associated records from IUS and IC_IUS tables
////        /// </summary>
////        /// <param name="nids"></param>
////        public void DeleteSubgroupVals(string nids)
////        {
////            DITables TableNames;
////            IUSBuilder IUSBuilder;
////            string AssocicatedIUSNIds = string.Empty;
////            try
////            {

////                IUSBuilder = new IUSBuilder(this.DBConnection, this.DBQueries);

////                // Step 1: Delete records from subgroup val table
////                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
////                {
////                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());

////                  this.DBConnection.ExecuteNonQuery(  DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Delete.DeleteSubgroupVals(TableNames.SubgroupVals, nids));
////                }

////                // Step 2: Delete records from IUS table

////                // Step2(a): Get all associated IUSNIds
////                AssocicatedIUSNIds = IUSBuilder.GetAllAssociatedIUSNids(string.Empty, string.Empty, nids);

////                // Step2(b): Delete all associated IUSNIds
////                IUSBuilder.DeleteIUS(AssocicatedIUSNIds);


////            }
////            catch (Exception ex)
////            {

////                throw new ApplicationException(ex.ToString());
////            }


////        }      


////        #endregion
        
////        #endregion

////        #region "-- Static --"

////        public static string CreateSubgroupValsText(string sex, string others, string location, string age, SubgroupType type, string oldSubgroupVal)
////        {
////            //-- order: TOTAL +( Location & Sex & Age & other) 
////            string RetVal = string.Empty;

////            if (!string.IsNullOrEmpty(location))
////            {
////                if (type== SubgroupType.Location)
////                {
////                    RetVal =SubgroupValBuilder.AddValue(RetVal, oldSubgroupVal);
////                }
////                else
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, location);
////                }
////            }

////            if (!string.IsNullOrEmpty(sex))
////            {
////                if (type== SubgroupType.Sex)
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, oldSubgroupVal);
////                }
////                else
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, sex);
////                }
////            }

////            if (!string.IsNullOrEmpty( age))
////            {
////                if (type== SubgroupType.Age)
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, oldSubgroupVal);
////                }
////                else
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, age);
////                }
////            }

////            if ( !string.IsNullOrEmpty( others))
////            {
////                if (type== SubgroupType.Others)
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, oldSubgroupVal);
////                }
////                else
////                {
////                    RetVal = SubgroupValBuilder.AddValue(RetVal, others);
////                }
////            }
////            return RetVal;
////        } 

////        private static string AddValue(string oldValue, string concatenateValue) 
////{ 
////    string RetVal=oldValue;
    
////    if (string.IsNullOrEmpty( oldValue))
////    { 
////        RetVal= concatenateValue; 
////    } 
////    else
////    {
////        RetVal += " " + concatenateValue;
////    } 

////    return RetVal; 
////} 
////        #endregion
////    }
}
