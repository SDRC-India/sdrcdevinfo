using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build indicator,unit,subgroup combined record according to indicator,unit,subgroupval information and insert it into database.   
    /// </summary>
    public class IUSBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private IndicatorBuilder DIIndicator;
        private UnitBuilder DIUnit;
        private DI6SubgroupValBuilder DISubgroupVal;

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        #region "-- IUS --"

        /// <summary>
        /// Check existance of  IUS record in database 
        /// </summary>
        /// <param name="IUSRecord">object of IUSInfo</param>
        /// <returns>IUS Nid</returns>
        private int CheckIUSExists(IUSInfo IUSInfo)
        {
            int RetVal = 0;
            int IndicatorNid = 0;
            int UnitNid = 0;
            int SubgroupValNid = 0;

            //get nids 
            IndicatorNid = this.DIIndicator.CheckNCreateIndicator(IUSInfo.IndicatorInfo);
            UnitNid = this.DIUnit.CheckNCreateUnit(IUSInfo.UnitInfo);            
            SubgroupValNid = this.DISubgroupVal.CheckNCreateSubgroupVal(IUSInfo.SubgroupValInfo);

            //udpate  object of IUSInfo
            IUSInfo.IndicatorInfo.Nid = IndicatorNid;
            IUSInfo.UnitInfo.Nid = UnitNid;
            IUSInfo.SubgroupValInfo.Nid = SubgroupValNid;

            //check combination of indicator, unit and subgroup exists in the database or not
            if (IndicatorNid > 0 & UnitNid > 0 & SubgroupValNid > 0)
            {
                RetVal = this.GetIUSNid(IndicatorNid, UnitNid, SubgroupValNid);
            }

            return RetVal;
        }

        /// <summary>
        /// Insert IUS in database
        /// </summary>
        /// <param name="IUSRecord">object of IUSInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIUS(IUSInfo IUSInfo)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertIUS(this.DBQueries.DataPrefix, IUSInfo.IndicatorInfo.Nid,
                    IUSInfo.UnitInfo.Nid, IUSInfo.SubgroupValInfo.Nid,
                    IUSInfo.Maximum, IUSInfo.Minimum);
                this.DBConnection.ExecuteNonQuery(SqlQuery);
                RetVal = true;
            }
            catch (Exception)
            {
                return RetVal;
            }

            return RetVal;
        }
        /// <summary>
        /// Add IUS record into collection
        /// </summary>
        /// <param name="IUSRecord">object of IUSInfo </param>
        private void AddIUSIntoCollection(IUSInfo IUSInfo)
        {
            if (!this.IUSCollection.ContainsKey(IUSInfo.Nid.ToString()))
            {
                this.IUSCollection.Add(IUSInfo.Nid.ToString(), IUSInfo);
            }
        }


        #endregion

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables & Properties --"
        /// <summary>
        /// Returns ius colleciton in key,pair format. Key is ius nid and value is object of iusinfo.
        /// </summary>
        internal Dictionary<string, IUSInfo> IUSCollection = new Dictionary<string, IUSInfo>();

        #endregion
        #endregion

        #region "-- Public --"
        #region "-- New/Dispose --"

        public IUSBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
            this.DIIndicator = new IndicatorBuilder(connection, queries);
            this.DIUnit = new UnitBuilder(connection, queries);
            this.DISubgroupVal = new DI6SubgroupValBuilder(connection, queries);
            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check and create IUS
        /// </summary>
        /// <param name="IUSRecord">object of IUS</param>
        /// <returns>IUS Nid</returns>
        public int CheckNCreateIUS(IUSInfo IUSInfo)
        {
            int RetVal = 0;
            try
            {
                //Step 1: check the existence of IUS 
                RetVal = this.CheckIUSExists(IUSInfo);

                //Step 2: if doesnt exist , then insert into database
                if (RetVal <= 0)
                {
                    if (this.InsertIUS(IUSInfo))
                    {
                        RetVal = this.GetIUSNid(IUSInfo.IndicatorInfo.Nid, IUSInfo.UnitInfo.Nid, IUSInfo.SubgroupValInfo.Nid);
                    }
                }

                //Step 3: add ius info into collection

                this.AddIUSIntoCollection(IUSInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get IUS Nid.
        /// </summary>
        /// <param name="indicatorNid">Indicator Nid</param>
        /// <param name="unitNid">Unit Nid</param>
        /// <param name="subgroupValNid">Subgroup val Nid </param>
        /// <returns>IUS Nid</returns>
        public int GetIUSNid(int indicatorNid, int unitNid, int subgroupValNid)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable IUSTable = new DataTable();
            try
            {
                SqlQuery = this.DBQueries.IUS.GetIUSNIdByI_U_S(indicatorNid.ToString(), unitNid.ToString(), subgroupValNid.ToString());
                IUSTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                if (IUSTable.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(IUSTable.Rows[0][Indicator_Unit_Subgroup.IUSNId]);
                }

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }


        /// <summary>
        /// Get all associated IUSNIds.
        /// </summary>
        /// <param name="indicatorNids">Comma separated Indicator Nids</param>
        /// <param name="unitNids">Comma separated Unit Nids</param>
        /// <param name="subgroupValNids">Comma separated Subgroup val Nids </param>
        /// <returns>Comma separated IUSNids</returns>
        public string GetAllAssociatedIUSNids(string indicatorNids, string unitNids, string subgroupValNids)
        {
            string RetVal = string.Empty;
            string SqlQuery = string.Empty;
            DataTable IUSTable = new DataTable();
            try
            {
                SqlQuery = this.DBQueries.IUS.GetIUSNIdByI_U_S(indicatorNids, unitNids, subgroupValNids);
                IUSTable = this.DBConnection.ExecuteDataTable(SqlQuery);

                foreach (DataRow Row in IUSTable.Rows)
                {
                    if (string.IsNullOrEmpty(RetVal))
                    {
                        RetVal = Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                    }
                    else
                    {
                        RetVal = RetVal + "," + Row[Indicator_Unit_Subgroup.IUSNId].ToString();
                    }
                }

            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }

            return RetVal;
        }

        /// <summary>
        /// Get IUS Nid.
        /// </summary>
        /// <param name="indicatorGId">Indicator GId</param>
        /// <param name="unitGId">Unit GId</param>
        /// <param name="subgroupValGId">Subgroup val GId </param>
        /// <returns>IUS Nid</returns>
        public int GetIUSNid(string indicatorGId, string unitGId, string subgroupValGId)
        {
            int RetVal = 0;
            int IndicatorNid = 0;
            int UnitNid = 0;
            int SubgroupValNid = 0;

            try
            {
                IndicatorNid = this.DIIndicator.GetIndicatorNid(indicatorGId, string.Empty);
                UnitNid = this.DIUnit.GetUnitNid(unitGId, string.Empty);
                SubgroupValNid = this.DISubgroupVal.GetSubgroupValNid(subgroupValGId, string.Empty);
                if (IndicatorNid > 0 & UnitNid > 0 & SubgroupValNid > 0)
                {
                    RetVal = this.GetIUSNid(IndicatorNid, UnitNid, SubgroupValNid);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get IUS Nid.
        /// </summary>
        /// <param name="indicatorGId">Indicator GId</param>
        /// <param name="unitGId">Unit GId</param>
        /// <param name="subgroupValGId">Subgroup val GId </param>
        /// <returns>IUS Nid</returns>
        public int GetIUSNid(string indicatorGId, string unitGId, string subgroupValGId, string indicatorName, string unitName, string subgroupval)
        {
            int RetVal = 0;
            int IndicatorNid = 0;
            int UnitNid = 0;
            int SubgroupValNid = 0;

            try
            {
                IndicatorNid = this.DIIndicator.GetIndicatorNid(indicatorGId, indicatorName);
                UnitNid = this.DIUnit.GetUnitNid(unitGId, unitName);
                SubgroupValNid = this.DISubgroupVal.GetSubgroupValNid(subgroupValGId, subgroupval);
                if (IndicatorNid > 0 & UnitNid > 0 & SubgroupValNid > 0)
                {
                    RetVal = this.GetIUSNid(IndicatorNid, UnitNid, SubgroupValNid);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Get IUSNId from IUSInfo and also get the IndicatorNid, UnitNid and SubgroupValNid
        /// </summary>
        /// <param name="IUSInfoObj"></param>
        /// <returns></returns>
        public int GetIUSNid(IUSInfo IUSInfoObj)
        {
            int RetVal = 0;

            try
            {
                IUSInfoObj.IndicatorInfo.Nid = this.DIIndicator.GetIndicatorNid(IUSInfoObj.IndicatorInfo.GID, IUSInfoObj.IndicatorInfo.Name);
                IUSInfoObj.UnitInfo.Nid = this.DIUnit.GetUnitNid(IUSInfoObj.UnitInfo.GID, IUSInfoObj.UnitInfo.Name);
                IUSInfoObj.SubgroupValInfo.Nid = this.DISubgroupVal.GetSubgroupValNid(IUSInfoObj.SubgroupValInfo.GID, IUSInfoObj.SubgroupValInfo.Name);

                if (IUSInfoObj.IndicatorInfo.Nid > 0 & IUSInfoObj.UnitInfo.Nid > 0 & IUSInfoObj.SubgroupValInfo.Nid > 0)
                {

                    RetVal = this.GetIUSNid(IUSInfoObj.IndicatorInfo.Nid, IUSInfoObj.UnitInfo.Nid, IUSInfoObj.SubgroupValInfo.Nid);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Import indicator, unit and subgroupval. 
        /// </summary>
        /// <param name="iusInfo"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportIUS(IUSInfo iusInfo, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = 0;
            int NewIndicatorNid = 0;
            int NewUnitNid = 0;
            int NewSubgroupValNid = 0;


            try
            {
                //import indicator
                NewIndicatorNid = this.DIIndicator.ImportIndicator(iusInfo.IndicatorInfo, iusInfo.IndicatorInfo.Nid, sourceQurey, sourceDBConnection);

                //import unit
                NewUnitNid = this.DIUnit.ImportUnit(iusInfo.UnitInfo, iusInfo.UnitInfo.Nid, sourceQurey, sourceDBConnection);

                //import subgroupval
                NewSubgroupValNid = this.DISubgroupVal.ImportSubgroupVal(iusInfo.SubgroupValInfo.Nid, sourceQurey, sourceDBConnection);

                //check ius exists or not
                if (NewIndicatorNid > 0 & NewUnitNid > 0 & NewSubgroupValNid > 0)
                {
                    RetVal = this.GetIUSNid(NewIndicatorNid, NewUnitNid, NewSubgroupValNid);

                    //update nids
                    iusInfo.IndicatorInfo.Nid = NewIndicatorNid;
                    iusInfo.UnitInfo.Nid = NewUnitNid;
                    iusInfo.SubgroupValInfo.Nid = NewSubgroupValNid;

                    if (RetVal <= 0)
                    {
                        //insert ius combination
                        this.InsertIUS(iusInfo);
                        RetVal = this.GetIUSNid(NewIndicatorNid, NewUnitNid, NewSubgroupValNid);
                    }
                }

            }
            catch (Exception ex)
            {
                RetVal = 0;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }


        /// <summary>
        /// Import indicator, unit and subgroupval. 
        /// </summary>
        /// <param name="iusInfo"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns></returns>
        public int ImportIUS(int indicatorNId, int unitNId, int subgroupValNId, int minValue, int maxValue, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = 0;


            try
            {

                //check ius exists or not
                if (indicatorNId > 0 & unitNId > 0 & subgroupValNId > 0)
                {
                    RetVal = this.GetIUSNid(indicatorNId, unitNId, subgroupValNId);


                    if (RetVal <= 0)
                    {
                        //insert ius combination
                        RetVal = this.InsertIUS(indicatorNId, unitNId, subgroupValNId, maxValue, minValue);
                    }
                }

            }
            catch (Exception ex)
            {
                RetVal = 0;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }


        /// <summary>
        /// Deletes records from IUS table  and associated records from IC_IUS table
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteIUS(string nids)
        {
            string SqlQuery = string.Empty;
            DataTable Table;
            string DataNIds = string.Empty;

            if (!string.IsNullOrEmpty(nids))
            {
                try
                {
                    // Step1: Delete records from IUS table
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.IUS.DeleteIUS(nids));

                    // Step2: Delete records from IC_IUS table
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteICIUS(this.DBQueries.TablesName.IndicatorClassificationsIUS, nids));

                    // Step3: delete records from Data table
                    new DIDatabase(this.DBConnection, this.DBQueries).DeleteByIUSNIds(nids);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Inserts IUS into database and returns IUSNID.
        /// </summary>
        /// <param name="IndicatorNid"></param>
        /// <param name="UnitNid"></param>
        /// <param name="SubgroupValNid"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <returns></returns>
        public int InsertIUS(int IndicatorNid, int UnitNid, int SubgroupValNid, long maximum, long minimum)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertIUS(this.DBQueries.DataPrefix, IndicatorNid, UnitNid, SubgroupValNid, maximum, minimum);
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Inserts IUS into database and returns IUSNID.
        /// </summary>
        /// <param name="IndicatorNid"></param>
        /// <param name="UnitNid"></param>
        /// <param name="SubgroupValNid"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <param name="isDefaultSubgroup"></param>
        /// <returns></returns>
        public int InsertIUS(int IndicatorNid, int UnitNid, int SubgroupValNid, long maximum, long minimum, bool isDefaultSubgroup)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertIUS(this.DBQueries.DataPrefix, IndicatorNid, UnitNid, SubgroupValNid, maximum, minimum, isDefaultSubgroup);
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }


        /// <summary>
        /// Inserts IUS into database 
        /// </summary>
        /// <param name="indicatorGId"></param>
        /// <param name="unitGId"></param>
        /// <param name="subgroupGId"></param>
        /// <returns></returns>
        public int InsertIUSIntoDB(string indicatorGId, string unitGId, string subgroupGId)
        {
            int RetVal = -1;

            IndicatorBuilder IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            UnitBuilder UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupValBuilder SGBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
            IUSBuilder IUSObj = new IUSBuilder(this.DBConnection, this.DBQueries);
            int IndicatorNID = -1;
            int UnitNID = -1;
            int SubgroupValNID = -1;

            try
            {
                IndicatorNID = IndicatorBuilderObj.GetIndicatorNid(indicatorGId, string.Empty);
                UnitNID = UnitBuilderObj.GetUnitNid(unitGId, string.Empty);
                SubgroupValNID = SGBuilder.GetSubgroupValNid(subgroupGId, string.Empty);

                if (IndicatorNID > 0 & UnitNID > 0 & SubgroupValNID > 0)
                {
                    RetVal = IUSObj.InsertIUS(IndicatorNID, UnitNID, SubgroupValNID, 0, 0);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Inserts or updates IUS into database 
        /// </summary>
        /// <param name="indicatorGId"></param>
        /// <param name="unitGId"></param>
        /// <param name="subgroupGId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int InsertOrUpdateIUSIntoDB(string indicatorGId, string unitGId, string subgroupGId, int minValue, int maxValue)
        {
            int RetVal = -1;

            IndicatorBuilder IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            UnitBuilder UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupValBuilder SGBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
            IUSBuilder IUSObj = new IUSBuilder(this.DBConnection, this.DBQueries);
            int IndicatorNID = -1;
            int UnitNID = -1;
            int SubgroupValNID = -1;

            try
            {
                IndicatorNID = IndicatorBuilderObj.GetIndicatorNid(indicatorGId, string.Empty);
                UnitNID = UnitBuilderObj.GetUnitNid(unitGId, string.Empty);
                SubgroupValNID = SGBuilder.GetSubgroupValNid(subgroupGId, string.Empty);

                if (IndicatorNID > 0 & UnitNID > 0 & SubgroupValNID > 0)
                {
                    // check IUS combination already exists or not
                    RetVal = this.GetIUSNid(IndicatorNID, UnitNID, SubgroupValNID);
                    if (RetVal > 0)
                    {
                        //update record
                        this.UpdateIUS(IndicatorNID, UnitNID, SubgroupValNID, maxValue.ToString(), minValue.ToString(), RetVal);
                    }
                    else
                    {
                        // insert record
                        RetVal = IUSObj.InsertIUS(IndicatorNID, UnitNID, SubgroupValNID, minValue, maxValue);
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
        /// Inserts or updates IUS into database 
        /// </summary>
        /// <param name="indicatorGId"></param>
        /// <param name="unitGId"></param>
        /// <param name="subgroupGId"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public int InsertOrUpdateIUSIntoDBByName(string indicatorName, string unitName, string subgroupval, int minValue, int maxValue)
        {
            int RetVal = -1;

            IndicatorBuilder IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            UnitBuilder UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupValBuilder SGBuilder = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
            IUSBuilder IUSObj = new IUSBuilder(this.DBConnection, this.DBQueries);
            int IndicatorNID = -1;
            int UnitNID = -1;
            int SubgroupValNID = -1;

            try
            {
                IndicatorNID = IndicatorBuilderObj.GetIndicatorNid(string.Empty, indicatorName);
                UnitNID = UnitBuilderObj.GetUnitNid(string.Empty, unitName);
                SubgroupValNID = SGBuilder.GetSubgroupValNid(string.Empty, subgroupval);

                if (IndicatorNID > 0 & UnitNID > 0 & SubgroupValNID > 0)
                {
                    // check IUS combination already exists or not
                    RetVal = this.GetIUSNid(IndicatorNID, UnitNID, SubgroupValNID);
                    if (RetVal > 0)
                    {
                        //update record
                        this.UpdateIUS(IndicatorNID, UnitNID, SubgroupValNID, maxValue.ToString(), minValue.ToString(), RetVal);
                    }
                    else
                    {
                        // insert record
                        RetVal = IUSObj.InsertIUS(IndicatorNID, UnitNID, SubgroupValNID, minValue, maxValue);
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
        /// Updates IUS into database on the basis of IUSNId.
        /// </summary>
        /// <param name="IndicatorNid"></param>
        /// <param name="UnitNid"></param>
        /// <param name="SubgroupValNid"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <param name="IUSNId"></param>
        /// <returns></returns>
        public void UpdateIUS(int IndicatorNid, int UnitNid, int SubgroupValNid, string maximum, string minimum, int IUSNId)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = this.DBQueries.Update.IUS.UpdateIUS(IUSNId, IndicatorNid, UnitNid, SubgroupValNid, maximum, minimum);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Updates IUS into database on the basis of IUSNId.
        /// </summary>
        /// <param name="IndicatorNid"></param>
        /// <param name="UnitNid"></param>
        /// <param name="SubgroupValNid"></param>
        /// <param name="maximum"></param>
        /// <param name="minimum"></param>
        /// <param name="IUSNId"></param>
        /// <param name="isDefaultSubgroup"></param>
        public void UpdateIUS(int IndicatorNid, int UnitNid, int SubgroupValNid, string maximum, string minimum, int IUSNId, bool isDefaultSubgroup)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = this.DBQueries.Update.IUS.UpdateIUSWithDefaultSubgroup(IUSNId, IndicatorNid, UnitNid, SubgroupValNid, maximum, minimum, isDefaultSubgroup);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Returns instance of IUSInfo.
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <param name="selectionType"></param>
        /// <returns></returns>
        public IUSInfo GetIUSInfo(FilterFieldType filterClause, string filterText, FieldSelection selectionType)
        {
            string Query = string.Empty;

            IUSInfo RetVal = new IUSInfo();
            IndicatorInfo IndicatorObject = new IndicatorInfo();
            UnitInfo UnitObject = new UnitInfo();
            IndicatorBuilder IndicatorBuilderObj = null;
            UnitBuilder UnitBuilderObj = null;
            DI6SubgroupValBuilder SubgroupValBuilderObj = null;
            DI6SubgroupValInfo SubgroupValObject = new DI6SubgroupValInfo();

            int IndicatorNid = 0;
            int UnitNid = 0;
            int SGNid = 0;
            int MinVal = 0;
            int MaxVal = 0;
            DataTable Table = null;
            try
            {


                //get IUS information
                Query = this.DBQueries.IUS.GetIUS(filterClause, filterText, selectionType);
                Table = this.DBConnection.ExecuteDataTable(Query);

                //set IUS info
                if (Table != null)
                {
                    if (Table.Rows.Count > 0)
                    {
                        MinVal = 0;
                        MaxVal = 0;

                        // initialize builder objects
                        IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
                        UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
                        SubgroupValBuilderObj = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

                        // set IUS properties 
                        //-- set maximum value
                        if (!string.IsNullOrEmpty(Convert.ToString(Table.Rows[0][Indicator_Unit_Subgroup.MaxValue])))
                        {
                            MaxVal = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.MaxValue]);
                        }
                        //-- Set Minmum Value
                        if (!string.IsNullOrEmpty(Convert.ToString(Table.Rows[0][Indicator_Unit_Subgroup.MinValue])))
                        {
                            MinVal = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.MinValue]);
                        }

                        RetVal.Maximum = MaxVal;    // Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.MaxValue]);
                        RetVal.Minimum = MinVal;    // Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.MinValue]);

                        RetVal.Nid = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.IUSNId]);

                        // set indicator, unit and subgroup info
                        IndicatorNid = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.IndicatorNId]);
                        UnitNid = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.UnitNId]);
                        SGNid = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.SubgroupValNId]);

                        RetVal.IndicatorInfo = IndicatorBuilderObj.GetIndicatorInfo(FilterFieldType.NId, IndicatorNid.ToString(), FieldSelection.Light);
                        RetVal.UnitInfo = UnitBuilderObj.GetUnitInfo(FilterFieldType.NId, UnitNid.ToString());
                        RetVal.SubgroupValInfo = SubgroupValBuilderObj.GetSubgroupValInfo(FilterFieldType.NId, SGNid.ToString());

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
        /// Updates data exists values
        /// </summary>
        public void UpdateDataExistValues()
        {
            try
            {
                // 1. set all IUS's data_exist value to false in default language table
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.IUS.UpdateDataExistsToFalse(this.DBConnection.ConnectionStringParameters.ServerType));

                // 2. set data_exist to true but where data exists
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.IUS.UpdateDataExistsValues());

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Updates subgroup nids for every IUS
        /// </summary>
        public void UpdateSubgroupNids()
        {
            DataTable Table = null;
            string IUSNid = string.Empty;
            string SubgroupNids = string.Empty;

            try
            {
                // get IUSNIds with subgroup nids
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSNidsWSubgroupNIds());

                foreach (DataRow Row in Table.Rows)
                {
                    if (Convert.ToString(Row[Indicator_Unit_Subgroup.IUSNId]) == IUSNid)
                    {
                        // add subgroup nid
                        SubgroupNids += "," + Convert.ToString(Row[SubgroupValsSubgroup.SubgroupNId]);
                    }
                    else
                    {
                        // update subgruop nids where IUSNid=<iusnid>
                        if (!string.IsNullOrEmpty(IUSNid))
                        {
                            this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.IUS.UpdateSubgroupNids(IUSNid, SubgroupNids));
                        }

                        // update IUSNID & subgroup nid variables
                        IUSNid = Convert.ToString(Row[Indicator_Unit_Subgroup.IUSNId]);
                        SubgroupNids = Convert.ToString(Row[SubgroupValsSubgroup.SubgroupNId]);
                    }
                }

                // update subgruop nids where IUSNid=<iusnid>
                if (!string.IsNullOrEmpty(IUSNid))
                {
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.IUS.UpdateSubgroupNids(IUSNid, SubgroupNids));
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Update Default Subgroup for IUS
        /// Set Total as default subgroup else Set subgroup start with 'Total' else Set first subgroup in ius
        /// </summary>
        public void UpdateISDefaultSubgroups()
        {
            bool ColumnExist = true;
            DataTable IUSTable = null;
            DataTable IUTable = null;
            DataView IUSTableView = null;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = this.DBQueries.Update.IUS.UpdateISDefaultSubgroup(true, "'Total'");
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                IUSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));
                IUTable = IUSTable.DefaultView.ToTable(true, Indicator.IndicatorNId, Indicator.IndicatorName, Unit.UnitNId, Unit.UnitName);

                foreach (DataRow IURow in IUTable.Rows)
                {
                    DataRow[] Rows = IUSTable.Select(Indicator.IndicatorNId + "=" + Convert.ToString(IURow[Indicator.IndicatorNId]) + " AND " + Unit.UnitNId + "=" + Convert.ToString(IURow[Unit.UnitNId]));

                    if (Rows.Length == 1)
                    {
                        this.UpdateIUSISDefaultSubgroup(Convert.ToString(Rows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                    }
                    else if (Rows.Length > 1)
                    {
                        DataRow[] NewRows = IUSTable.Select(Indicator.IndicatorNId + "=" + Convert.ToString(IURow[Indicator.IndicatorNId]) + " AND " + Unit.UnitNId + "=" + Convert.ToString(IURow[Unit.UnitNId]) + " AND " + SubgroupVals.SubgroupVal + " = 'Total'");
                        //-- Update IUS if Default Subgroup Not exists
                        if (NewRows.Length == 0)
                        {
                            DataRow[] SGIURows = IUSTable.Select(Indicator.IndicatorNId + "=" + Convert.ToString(IURow[Indicator.IndicatorNId]) + " AND " + Unit.UnitNId + "=" + Convert.ToString(IURow[Unit.UnitNId]) + " AND " + SubgroupVals.SubgroupVal + " like 'Total%'");

                            //-- Set Default Subgroup for start with Total
                            if (SGIURows.Length > 0)
                            {
                                this.UpdateIUSISDefaultSubgroup(Convert.ToString(SGIURows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                            }
                            else
                            {
                                //-- Set Default Subgroup for first subgroup if not find for total
                                this.UpdateIUSISDefaultSubgroup(Convert.ToString(Rows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// Update DefaultSubgroup for IUSNid
        /// </summary>
        /// <param name="iusNIds"></param>
        /// <param name="isDefaultSG"></param>
        public void UpdateIUSISDefaultSubgroup(string IUSNIds, bool isDefaultSG)
        {
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = this.DBQueries.Update.IUS.UpdateIUSISDefaultSubgroup(IUSNIds, isDefaultSG);
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Import ISDefaultSubgroup from database
        /// </summary>
        /// <param name="sourceDatabase"></param>
        /// <param name="sourceDBQueries"></param>
        public void ImportDefaultSubgroups(DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {
            string IUSNids = string.Empty;

            DataTable IUSTable = null;
            DataTable IUTable = null;
            IUSInfo IUSInfoObj = new IUSInfo();

            //-- Get IUS from Source Database
            IUSTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light, true));

            //-- Get Default IUS Values
            DataRow[] IUSRows = IUSTable.Select(Indicator_Unit_Subgroup.IsDefaultSubgroup + "=1");

            foreach (DataRow Row in IUSRows)
            {
                IUSInfoObj = new IUSInfo();
                IUSInfoObj.IndicatorInfo = new IndicatorInfo();
                IUSInfoObj.UnitInfo = new UnitInfo();
                IUSInfoObj.SubgroupValInfo = new DI6SubgroupValInfo();

                IUSInfoObj.IndicatorInfo.Name = Convert.ToString(Row[Indicator.IndicatorName]);
                IUSInfoObj.IndicatorInfo.GID = Convert.ToString(Row[Indicator.IndicatorGId]);

                IUSInfoObj.UnitInfo.Name = Convert.ToString(Row[Unit.UnitName]);
                IUSInfoObj.UnitInfo.GID = Convert.ToString(Row[Unit.UnitGId]);

                IUSInfoObj.SubgroupValInfo.Name = Convert.ToString(Row[SubgroupVals.SubgroupVal]);
                IUSInfoObj.SubgroupValInfo.GID = Convert.ToString(Row[SubgroupVals.SubgroupValGId]);

                int IUSNid = this.GetIUSNid(IUSInfoObj);

                //-- Set Default Subgroup if IUS exists
                if (IUSNid > 0)
                {
                    IUTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSByI_U_S(IUSInfoObj.IndicatorInfo.Nid.ToString(), IUSInfoObj.UnitInfo.Nid.ToString(), string.Empty));

                    IUSNids = DIConnection.GetDelimitedValuesFromDataTable(IUTable, Indicator_Unit_Subgroup.IUSNId);

                    //-- Make ISDefault false for other subgroupval for current I-U
                    this.UpdateIUSISDefaultSubgroup(IUSNids, false);

                    //-- Set DefaultSubgroupVal for IUSNid
                    this.UpdateIUSISDefaultSubgroup(IUSNid.ToString(), true);
                }
            }
        }

        /// <summary>
        /// Set Default Subgroup for I-U
        /// </summary>
        /// <param name="IndicatorNId"></param>
        /// <param name="UnitNId"></param>
        /// <returns>SubgroupVal Nid which is set default</returns>
        public int UpdateISDefaultSubgroup(int IndicatorNId, int UnitNId)
        {
            string SqlQuery = string.Empty;
            DataTable Table = null;
            int RetVal = 0;

            try
            {
                SqlQuery = this.DBQueries.IUS.GetIUSByI_U_S(IndicatorNId.ToString(), UnitNId.ToString(), "", true);
                Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                if (Table.Select(Indicator_Unit_Subgroup.IsDefaultSubgroup + "=1").Length == 0)
                {
                    //  IUSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light, true));

                    DataRow[] Rows = Table.Select(Indicator.IndicatorNId + "=" + Convert.ToString(IndicatorNId) + " AND " + Unit.UnitNId + "=" + Convert.ToString(UnitNId) + " AND " + SubgroupVals.SubgroupVal + " = 'Total'");

                    //-- Update IUS if Default Subgroup Not exists
                    if (Rows.Length == 0)
                    {
                        DataRow[] SGIURows = Table.Select(Indicator.IndicatorNId + "=" + Convert.ToString(IndicatorNId) + " AND " + Unit.UnitNId + "=" + Convert.ToString(UnitNId) + " AND " + SubgroupVals.SubgroupVal + " like 'Total%'");

                        //-- Set Default Subgroup for start with Total
                        if (SGIURows.Length > 0)
                        {
                            this.UpdateIUSISDefaultSubgroup(Convert.ToString(SGIURows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                            RetVal = Convert.ToInt32(SGIURows[0][Indicator_Unit_Subgroup.SubgroupValNId]);
                        }
                        else
                        {
                            DataRow[] IUSRows = Table.Select("1=1", Indicator_Unit_Subgroup.IUSNId + " Asc");
                            //-- Set Default Subgroup for first subgroup if not find for total
                            if (IUSRows.Length > 0)
                            {
                                this.UpdateIUSISDefaultSubgroup(Convert.ToString(IUSRows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                                RetVal = Convert.ToInt32(IUSRows[0][Indicator_Unit_Subgroup.SubgroupValNId]);
                            }
                        }
                    }
                    else
                    {
                        this.UpdateIUSISDefaultSubgroup(Convert.ToString(Rows[0][Indicator_Unit_Subgroup.IUSNId]), true);
                        RetVal = Convert.ToInt32(Rows[0][Indicator_Unit_Subgroup.SubgroupValNId]);
                    }
                }
                else
                {
                    RetVal = Convert.ToInt32(Table.Select(Indicator_Unit_Subgroup.IsDefaultSubgroup + "=1")[0][Indicator_Unit_Subgroup.SubgroupValNId].ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        #endregion

        #endregion
    }
}
