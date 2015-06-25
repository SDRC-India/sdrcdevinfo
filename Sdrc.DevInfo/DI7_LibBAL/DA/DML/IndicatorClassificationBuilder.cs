using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Xml;
using System.IO;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Helps in creating and importing indicator classification 
    /// </summary>
    public class IndicatorClassificationBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// To check existance of Indicator classification first into collection then into database
        /// </summary>
        /// <param name="icInfo">object of IndicatorClassificationInfo</param>
        /// <returns>Nid</returns>
        private int CheckIndicatorClassificationExists(IndicatorClassificationInfo icInfo)
        {
            int RetVal = 0;

            //Step 1: check source exists in source collection
            RetVal = this.CheckIndicatorClassificationInCollection(icInfo.GID);

            //Step 2: check indicator exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetIndicatorClassificationNid(icInfo.GID, icInfo.Name, icInfo.Parent.Nid, icInfo.Type);
            }

            return RetVal;
        }


        /// <summary>
        ///To add indicator classification into collection 
        /// </summary>
        /// <param name="icInfo">object of IndicatorClassificationInfo</param>
        private void AddIndicatorIntoCollection(IndicatorClassificationInfo icInfo)
        {
            if (!this.IndicatorClassificationCollection.ContainsKey(icInfo.GID))
            {
                this.IndicatorClassificationCollection.Add(icInfo.GID, icInfo);
            }

        }

        /// <summary>
        /// To  check Indicator classification exists in collection
        /// </summary>
        /// <param name="gid"></param>
        /// <returns>Indicator classification Nid</returns>
        private int CheckIndicatorClassificationInCollection(string gid)
        {
            int RetVal = 0;
            try
            {
                if (this.IndicatorClassificationCollection.ContainsKey(gid))
                {
                    RetVal = this.IndicatorClassificationCollection[gid].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private int GetNidByName(string name, int parentNid, ICType type)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable TempTable;
            DataRow[] Rows;
            try
            {
                SqlQuery = this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, "'" + name + "'", type, FieldSelection.NId);
                TempTable = this.DBConnection.ExecuteDataTable(SqlQuery);
                if (TempTable.Rows.Count > 0)
                {
                    Rows = TempTable.Select(IndicatorClassifications.ICParent_NId + "=" + parentNid);
                    RetVal = Convert.ToInt32(Rows[0][IndicatorClassifications.ICNId]);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if GID exists in the database
        /// </summary>
        /// <returns></returns>
        public int GetNidByGID(string GID, ICType type)
        {
            string SqlQuery = string.Empty;
            int RetVal = 0;
            try
            {
                SqlQuery = this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.GId, "'" + GID + "'", type, FieldSelection.NId);
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        public string GetClassificationsChildren(int parentNid)
        {
            string RetVal = parentNid.ToString();
            DataTable ICTable;
            int NewFoundNId;

            try
            {
                // get all associated child against the given parent nid
                ICTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.ParentNId, parentNid.ToString(), FieldSelection.Light));

                foreach (DataRow Row in ICTable.Rows)
                {
                    NewFoundNId = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal += ",";
                    }
                    RetVal += this.GetClassificationsChildren(NewFoundNId);

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region"--Internal--"
        #region "-- Variables & Propertipes --"

        /// <summary>
        /// Returns indicator classification colleciton in key,pair format. Key is indicator classification gid and value is Object of IndicatorClassificationInfo.
        /// </summary>
        internal Dictionary<string, IndicatorClassificationInfo> IndicatorClassificationCollection = new Dictionary<string, IndicatorClassificationInfo>();

        #endregion
        #endregion

        #region "-- Public --"
        #region "-- New/Dispose --"

        public IndicatorClassificationBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
            DIConnection.ConnectionType = this.DBConnection.ConnectionStringParameters.ServerType;

        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of indicator classification  record into database if false then create indicator record 
        /// </summary>
        /// <param name="indicatorClassificationInfo">object of IndicatorClassificationInfo</param>
        /// <returns>Nid</returns>
        public int CheckNCreateIndicatorClassification(IndicatorClassificationInfo icInfo)
        {
            int RetVal = 0;

            try
            {
                // check indicator classification exists or not
                RetVal = this.CheckIndicatorClassificationExists(icInfo);

                // if indicator classification does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert indicator
                    if (this.InsertIntoDatabase(icInfo))
                    {
                        RetVal = this.GetNidByName(icInfo.Name, icInfo.Parent.Nid, icInfo.Type);
                    }

                }

                // add indicator classification information into collection
                icInfo.Nid = RetVal;
                this.AddIndicatorIntoCollection(icInfo);
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns indicator classification nid. 
        /// </summary>
        /// <param name="Gid">Indicator classification GID </param>
        /// <param name="name">Name of the Indicator classification</param>
        /// <param name="parentNid"></param>
        /// <param name="classificationType">Indicator classification type</param>
        /// <returns></returns>
        public int GetIndicatorClassificationNid(string Gid, string name, int parentNid, ICType classificationType)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            //--step1:Get Nid by GID if GID is not empty
            if (!string.IsNullOrEmpty(Gid))
            {
                RetVal = this.GetNidByGID(Gid, classificationType);
            }

            //--step2:Get Nid by Name if name is not empty
            if (RetVal <= 0)
            {
                if (!string.IsNullOrEmpty(name))
                {
                    RetVal = this.GetNidByName(name, parentNid, classificationType);
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Insert IndicatorClassification record into database
        /// </summary>
        /// <param name="indicatorClassificationInfo">object of IndicatorInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        public bool InsertIntoDatabase(IndicatorClassificationInfo indicatorClassificationInfo)
        {
            bool RetVal = false;
            string ICName = indicatorClassificationInfo.Name;
            string ICGId = Guid.NewGuid().ToString();
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string ICNameForDatabase = string.Empty;
            int ICOrder = 0;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //replace GID only if given gid is not empty or null.
                if (!string.IsNullOrEmpty(indicatorClassificationInfo.GID))
                {
                    ICGId = indicatorClassificationInfo.GID;
                }

                // get max IC order 
                try
                {
                    ICOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.IndicatorClassification.GetMaxICOrder(indicatorClassificationInfo.Parent.Nid)));
                }
                catch (Exception)
                {
                }

                ICOrder += 1;


                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        ICNameForDatabase = ICName;
                    }
                    else
                    {
                        ICNameForDatabase = Constants.PrefixForNewValue + ICName;

                    }
                    this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Insert.InsertIC(this.DBQueries.DataPrefix, "_" + LanguageCode, ICNameForDatabase, ICGId,
                        indicatorClassificationInfo.IsGlobal, indicatorClassificationInfo.Parent.Nid, indicatorClassificationInfo.ClassificationInfo, indicatorClassificationInfo.Type, ICOrder));
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
        /// To Import indicator classification from mapped indicator classification
        /// </summary>
        /// <param name="ICInfo">Instance of IndicatorClassificationInfo</param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidInTrgDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns>new indicator classification nid</returns>
        public int ImportICFrmMappedIC(IndicatorClassificationInfo ICInfo, int NidInSourceDB, int NidInTrgDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;

            try
            {
                // Set RetVal to NidInTrgDB
                RetVal = NidInTrgDB;


                if (RetVal > 0)
                {
                    // if source item is global
                    if (ICInfo.IsGlobal)
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UpdateIC(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode,
                           ICInfo.Name, ICInfo.GID, ICInfo.IsGlobal, ICInfo.Parent.Nid, ICInfo.ClassificationInfo, ICInfo.Type, RetVal));
                    }

                    //update/insert icon 
                    DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.IndicatorClassification, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// To Import indicator classification into database or template
        /// </summary>
        /// <param name="ICInfo">Instance of IndicatorClassificationInfo</param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns>new indicator classification nid</returns>
        public int ImportIndicatorClassification(IndicatorClassificationInfo ICInfo, int NidInSourceDB, DIQueries sourceQurey, DIConnection sourceDBConnection)
        {
            int RetVal = -1;
            bool ISTrgICGlobal = false;
            DataTable TempTable;

            try
            {
                //check item is already exist in database or not
                RetVal = this.GetIndicatorClassificationNid(ICInfo.GID, ICInfo.Name, ICInfo.Parent.Nid, ICInfo.Type);

                if (RetVal > 0)
                {
                    // check target ic is global 
                    TempTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Light));
                    if (TempTable.Rows.Count > 0)
                    {
                        ISTrgICGlobal = Convert.ToBoolean(TempTable.Rows[0][IndicatorClassifications.ICGlobal]);
                    }

                    // if target item is  not global
                    if (!ISTrgICGlobal)
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UpdateIC(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode,
                           ICInfo.Name, ICInfo.GID, ICInfo.IsGlobal, ICInfo.Parent.Nid, ICInfo.ClassificationInfo, ICInfo.Type, RetVal));
                    }

                }
                else
                {
                    if (this.InsertIntoDatabase(ICInfo))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }

                // update UT_CF_FLOWCHART table
                if (ICInfo.Type == ICType.CF)
                {
                    string NewXMLText = "<?xml version=\"1.0\"?><!--AddFlow.net diagram--><AddFlow Nodes=\"1\" Links=\"0\"><Version>1.5.2.0</Version></AddFlow>";

                    DataTable SrcCFTable = null;
                    DataTable TrgCFTable = null;

                    try
                    {
                        // get node where URL ==NidInSourceDB
                        SrcCFTable = sourceDBConnection.ExecuteDataTable(sourceQurey.IndicatorClassification.GetCFFlowCharts());
                        if (SrcCFTable.Rows.Count > 0)
                        {
                            string SrcXMLString = string.Empty;
                            string TrgXMLString = string.Empty;

                            // get xml from src database
                            SrcXMLString = Convert.ToString(SrcCFTable.Rows[0][CFFlowChart.CF_FlowChart]);

                            XmlDocument SrcXmlDoc = new XmlDocument();
                            SrcXmlDoc.LoadXml(SrcXMLString);
                            SrcXmlDoc.PreserveWhitespace = true;

                            //update Nid in src node
                            XmlNodeList SrcNodeList = SrcXmlDoc.SelectNodes("/AddFlow/Node[./Url='" + NidInSourceDB + "']");
                            SrcNodeList.Item(0).LastChild.InnerText = RetVal.ToString();

                            // get target CF table
                            XmlDocument TrgXmlDoc = new XmlDocument();
                            TrgXmlDoc.PreserveWhitespace = true;
                            TrgCFTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetCFFlowCharts());

                            if (TrgCFTable.Rows.Count > 0)
                            {
                                //update 
                                TrgXMLString = Convert.ToString(TrgCFTable.Rows[0][CFFlowChart.CF_FlowChart]);
                                if (string.IsNullOrEmpty(TrgXMLString))
                                {
                                    TrgXMLString = NewXMLText;
                                }
                                TrgXmlDoc.LoadXml(TrgXMLString);

                                XmlNodeList TrgNodeList = TrgXmlDoc.SelectNodes("/AddFlow/Node[./Url='" + RetVal + "']");

                                if (TrgNodeList != null & TrgNodeList.Count > 0)
                                {
                                    TrgXmlDoc.SelectNodes("/AddFlow").Item(0).RemoveChild(TrgNodeList.Item(0));
                                }

                                XmlNode SrcImpNode = SrcXmlDoc.SelectSingleNode("/AddFlow/Node[./Url='" + RetVal + "']");

                                //NewNode

                                XmlNode NewNode = TrgXmlDoc.ImportNode(SrcImpNode, true);

                                TrgXmlDoc.SelectNodes("/AddFlow").Item(0).AppendChild(NewNode);
                                TrgXMLString = DICommon.IndentXMLString(TrgXmlDoc.InnerXml);

                                TrgXMLString = DICommon.RemoveQuotes(TrgXMLString);




                                //TrgXmlDoc.Save("c:\\testtest.xml");
                                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpateCFFlowChart(this.DBQueries.TablesName.CFFlowChart, TrgXMLString));
                            }

                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                //update/insert icon 
                DIIcons.ImportElement(NidInSourceDB, RetVal, IconElementType.IndicatorClassification, sourceQurey, sourceDBConnection, this.DBQueries, this.DBConnection);




            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// To import IC and IUS realtionship into Indicator_Classification_IUS table
        /// </summary>
        /// <param name="NidInSourceDB"></param>
        /// <param name="NidinTargetDB"></param>
        /// <param name="classificationType"></param>
        /// <param name="srcQueries"></param>
        /// <param name="srcDBConnection"></param>
        public void ImportICAndIUSRelations(int NidInSourceDB, int NidinTargetDB, ICType classificationType, DIQueries srcQueries, DIConnection srcDBConnection)
        {
            // -- Create the IUS Links 
            // -- STEP 1: Get the Indicator, Unit and Subgroup Names from the Source Database 
            // -- STEP 2: Get IUSNIds from the Target Database against the I-U-S from the Source Database
            int TempIUSNId;
            int ClassificationNId;
            string IndicatorName;
            string UnitName;
            string SGVal;
            string IndicatorGID;
            string UnitGID;
            string SGValGID;
            bool RecommendedSource;
            DataTable TempTable = null;
            DataTable Table = null;


            // -- STEP 1: Get the Indicator, Unit and Subgroup Names from the Source Database 
            TempTable = srcDBConnection.ExecuteDataTable(srcQueries.IUS.GetIUSByIC(classificationType, NidInSourceDB, FieldSelection.Light));


            for (int Index = 0; Index <= TempTable.Rows.Count - 1; Index++)
            {
                try
                {
                    // -- STEP 2: Get IUSNIds from the Target Database against the I-U-S from the Source Database 
                    IndicatorName = DICommon.RemoveQuotes(TempTable.Rows[Index][Indicator.IndicatorName].ToString());
                    UnitName = DICommon.RemoveQuotes(TempTable.Rows[Index][Unit.UnitName].ToString());
                    SGVal = DICommon.RemoveQuotes(TempTable.Rows[Index][SubgroupVals.SubgroupVal].ToString());
                    RecommendedSource = Convert.ToBoolean(TempTable.Rows[Index][IndicatorClassificationsIUS.RecommendedSource]);


                    IndicatorGID = DICommon.RemoveQuotes(TempTable.Rows[Index][Indicator.IndicatorGId].ToString());
                    UnitGID = DICommon.RemoveQuotes(TempTable.Rows[Index][Unit.UnitGId].ToString());
                    SGValGID = DICommon.RemoveQuotes(TempTable.Rows[Index][SubgroupVals.SubgroupValGId].ToString());

                    // get records it by GID
                    try
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSNIdsByGID(
                            IndicatorGID, UnitGID, SGValGID));

                    }
                    catch (Exception)
                    { }

                    // if records not found then get it by Name
                    if (Table == null || Table.Rows.Count == 0)
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSByI_U_S_Name(IndicatorName, UnitName, SGVal, FieldSelection.Light));
                    }


                    if (Table.Rows.Count > 0)
                    {

                        TempIUSNId = Convert.ToInt32(Table.Rows[0][Indicator_Unit_Subgroup.IUSNId]);


                        //create relationship for parent_IC_NID also 
                        try
                        {
                            ClassificationNId = NidinTargetDB;
                            while (true)
                            {
                                // -- Create IUS Relationship 
                                this.AddNUpdateICIUSRelation(ClassificationNId, TempIUSNId, RecommendedSource);

                                //-- find parent IC_NId 
                                DataTable ICTable;
                                try
                                {
                                    ICTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, ClassificationNId.ToString(), classificationType, FieldSelection.Light));
                                    if (ICTable.Rows.Count > 0)
                                    {
                                        ClassificationNId = Convert.ToInt32(ICTable.Rows[0][IndicatorClassifications.ICParent_NId]);
                                    }
                                    else
                                    {
                                        ClassificationNId = -1;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    throw new ApplicationException(ex.ToString());
                                }
                                finally
                                {
                                    if ((Table != null))
                                        Table.Dispose();

                                    Table = null;
                                }
                                if (ClassificationNId == -1)
                                    break;

                            }
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(ex.ToString());
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
        /// Deletes records from IC_IUS table
        /// </summary>
        /// <param name="ICNIds"></param>
        /// <param name="IUSNids"></param>
        public void DeleteClassificationIUSRelation(string ICNIds, string IUSNids)
        {
            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteICIUS(this.DBQueries.TablesName.IndicatorClassificationsIUS, IUSNids, ICNIds));


        }

        /// <summary>
        /// Delete IC and IUS relation upto root level.
        /// </summary>
        public void DeleteICIUSRelationUptoRootlevel(ICType icType, int ICNid, int IUSNid)
        {
            string SqlQuery = string.Empty;
            DataView ICTable = null;
            int ICParentNid = ICNid;
            int ChildICNId;
            DataView IUSTable = null;
            bool IUSFound = false;

            try
            {
                if ((ICParentNid != -1))
                {
                    // add ic and ius relationship 
                    this.DeleteClassificationIUSRelation(ICNid.ToString(), IUSNid.ToString());

                    SqlQuery = this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, ICParentNid.ToString(), FieldSelection.NId);
                    ICTable = this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                    if (ICTable.Count > 0)
                    {
                        ICParentNid = (int)ICTable[0][IndicatorClassifications.ICParent_NId];
                        SqlQuery = this.DBQueries.IndicatorClassification.GetICForParentNIdAndICType(icType, ICParentNid);
                        ICTable = this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                        foreach (DataRowView Row in ICTable)
                        {
                            ChildICNId = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                            SqlQuery = this.DBQueries.IUS.GetIUSByIC(icType, ChildICNId, FieldSelection.NId);
                            IUSTable = this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;
                            IUSTable.RowFilter = Indicator_Unit_Subgroup.IUSNId + "=" + IUSNid;
                            //-- Delete Relation if IUS not associated with other siblings
                            if (IUSTable.Count > 0)
                            {
                                IUSFound = true;
                            }
                        }
                        if (!IUSFound)
                        {
                            this.DeleteICIUSRelationUptoRootlevel(icType, ICParentNid, IUSNid);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Delete IC & IUS relation by IUS/Indicator
        /// </summary>
        /// <param name="icType"></param>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <param name="isIUS"></param>
        public void DeleteICIUSRelationUptoRootlevel(ICType icType, int ICNid, int IUSNid, bool isIUS)
        {
            string IUSNIds = string.Empty;
            DataTable Table = null;

            try
            {
                if (!isIUS)
                {

                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSByI_U_S(IUSNid.ToString(), string.Empty, string.Empty));
                    //IUSNIds = DIConnection.GetDelimitedValuesFromDataTable(Table, Indicator.IndicatorNId);
                    foreach (DataRow Row in Table.Rows)
                    {
                        this.DeleteICIUSRelationUptoRootlevel(icType, ICNid, Convert.ToInt32(Row[Indicator_Unit_Subgroup.IUSNId]));
                    }
                }
                else
                {
                    this.DeleteICIUSRelationUptoRootlevel(icType, ICNid, IUSNid);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// Deletes records from Indicator_Classification table  and associated records from IC_IUS table. Use this method to delete records against given nids. This methods will not delete the associate child of the given Nids
        /// </summary>
        /// <param name="nids"></param>
        public void DeleteClassification(string nids)
        {
            DITables TableNames;
            if (!string.IsNullOrEmpty(nids))
            {
                try
                {
                    // Step1: Delete records from IndicatorClassification table
                    foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                    {
                        TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteIC(TableNames.IndicatorClassifications, nids));

                    }

                    // Step2: Delete records from IC_IUS table
                    this.DeleteClassificationIUSRelation(nids, string.Empty);

                    // step3: update parent_nid in IC table where IC_Parent_NId in( given nids)
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateIC(IndicatorClassifications.ICParent_NId + " IN(" + nids + ") ", this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, string.Empty, string.Empty, null, -1, string.Empty, null, nids));
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        /// <summary>
        /// Deletes records from Indicator_Classification table  and associated records from IC_IUS table. This methods deletes the all associate child against the given Nid
        /// </summary>
        /// <param name="NId"></param>
        public void DeleteClassificationChain(int NId)
        {
            string AllAssociatedNIds = string.Empty;

            // Get all associated NIds against the given NId
            AllAssociatedNIds = this.GetClassificationsChildren(NId);

            // Delete all found NIds from IC and IC_IUS table
            this.DeleteClassification(AllAssociatedNIds);
        }

        #region "-- Indicator_classification_IUS --"

        /// <summary>
        /// To Add and update IC and IUS relationship in IC_IUS table
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <param name="recommendedSource"></param>
        /// <param name="icOrder"></param>
        public void AddNUpdateICIUSRelation(int ICNid, int IUSNid, bool recommendedSource, int iusOrder, bool showIUS)
        {
            DataTable Table = null;

            try
            {
                if (showIUS)
                {
                    this.AddNUpdateICIUSRelation(ICNid, IUSNid, recommendedSource, iusOrder);
                }
                else
                {
                    //-- Add for Indicator( Get IUS for Indicator
                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSByI_U_S(IUSNid.ToString(), string.Empty, string.Empty));
                    //-- Update for each ISU of Indicator
                    foreach (DataRow Row in Table.Rows)
                    {
                        this.AddNUpdateICIUSRelation(ICNid, Convert.ToInt32(Row[Indicator_Unit_Subgroup.IUSNId]), recommendedSource);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// To Add and update IC and IUS relationship in IC_IUS table
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <param name="recommendedSource"></param>
        public void AddNUpdateICIUSRelation(int ICNid, int IUSNid, bool recommendedSource)
        {
            int SortOrder = 0;

            try
            {
                //-- check relationship already exists or not
                if (this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetICIUSNid(ICNid, IUSNid)).Rows.Count == 0)
                {

                    // get the maximum ICIUSorder 
                    try
                    {
                        SortOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.IndicatorClassification.GetMaxICIUSOrder(ICNid)));
                    }
                    catch (Exception)
                    { }
                    SortOrder += 1;

                    //-- if not then insert record into indicator_classification_IUS table
                    this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Insert.InsertICAndIUSRelation(this.DBQueries.DataPrefix, ICNid, IUSNid, recommendedSource, SortOrder));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Inserts IC and IUS relation upto root level.
        /// </summary>
        public void AddNUpdateICIUSRelationUptoRootlevel(int ICNid, int IUSNid, bool recommendedSource)
        {
            string SqlQuery = string.Empty;
            DataView ICTable = null;
            int ICParentNid = ICNid;

            try
            {
                if ((ICParentNid != -1))
                {
                    // add ic and ius relationship 
                    this.AddNUpdateICIUSRelation(ICNid, IUSNid, recommendedSource);

                    SqlQuery = this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, ICParentNid.ToString(), FieldSelection.NId);
                    ICTable = this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                    if (ICTable.Count > 0)
                    {
                        ICParentNid = (int)ICTable[0][IndicatorClassifications.ICParent_NId];
                        this.AddNUpdateICIUSRelationUptoRootlevel(ICParentNid, IUSNid, recommendedSource);
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Inserts IC and IUS relation upto root level.
        /// </summary>
        public void AddNUpdateICIUSRelationUptoRootlevel(int ICNid, int iusNId, bool recommendedSource, int iusOrder, bool showIUS)
        {
            string SqlQuery = string.Empty;
            DataView ICTable = null;
            int ICParentNid = ICNid;
            DataTable Table = null;

            try
            {
                if ((ICParentNid != -1))
                {
                    // add ic and ius relationship 
                    this.AddNUpdateICIUSRelation(ICNid, iusNId, recommendedSource, iusOrder, showIUS);

                    SqlQuery = this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, ICParentNid.ToString(), FieldSelection.NId);
                    ICTable = this.DBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                    if (!showIUS)
                    {
                        Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUSByI_U_S(iusNId.ToString(), string.Empty, string.Empty));
                    }

                    if (ICTable.Count > 0)
                    {
                        ICParentNid = (int)ICTable[0][IndicatorClassifications.ICParent_NId];

                        if (showIUS)
                        {
                            this.AddNUpdateICIUSRelationUptoRootlevel(ICParentNid, iusNId, recommendedSource);
                        }
                        else
                        {
                            //-- Update for each ISU of Indicator
                            foreach (DataRow Row in Table.Rows)
                            {
                                // todo:for testing
                                this.AddNUpdateICIUSRelationUptoRootlevel(ICParentNid, Convert.ToInt32(Row[Indicator_Unit_Subgroup.IUSNId]), recommendedSource, iusOrder, true);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// To Add and update IC and IUS relationship in IC_IUS table
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <param name="recommendedSource"></param>
        /// <param name="icOrder"></param>
        public void AddNUpdateICIUSRelation(int ICNid, int IUSNid, bool recommendedSource, int iusOrder)
        {
            try
            {
                //-- check relationship already exists or not
                if (this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetICIUSNid(ICNid, IUSNid)).Rows.Count == 0)
                {
                    //-- if not then insert record into indicator_classification_IUS table
                    this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Insert.InsertICAndIUSRelation(this.DBQueries.DataPrefix, ICNid, IUSNid, recommendedSource, iusOrder));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// To update IC and IUS Order in IC_IUS table
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="IUSNid"></param>
        /// <param name="icOrder"></param>
        public void UpdateICIUSOrder(int ICNid, int IUSNid, int iusOrder)
        {
            DataTable ICIUSTable = null;
            int ICIUSNid = 0;
            try
            {
                ICIUSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetICIUSNid(ICNid, IUSNid));
                //-- check relationship already exists or not
                if (ICIUSTable.Rows.Count > 0)
                {
                    ICIUSNid = Convert.ToInt32(ICIUSTable.Rows[0][IndicatorClassificationsIUS.ICIUSNId]);
                    //-- if not then Update record into indicator_classification_IUS table
                    this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UdpateICIUSOrder(this.DBQueries.DataPrefix, iusOrder, ICIUSNid));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Updates publisher, title & year values
        /// </summary>
        public void UpdatePublisherTitleYear()
        {
            DIServerType ServerType = this.DBConnection.ConnectionStringParameters.ServerType;
            DITables TablesName;
            DataTable Table;
            string LanguageCode = string.Empty;
            string PublisherName = string.Empty;
            string ICName = string.Empty;
            int Index = -1;
            int ICNid = 0;

            try
            {
                // 1.Do the following for the default langauge
                // 1.1 update publisher
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdatePublisherColumn(ServerType, this.DBQueries.TablesName.IndicatorClassifications));

                // 1.2 update year
                //this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateYearColumn(ServerType, this.DBQueries.TablesName.IndicatorClassifications));
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetAllSourceColumnsWithoutPublishers());

                foreach (DataRow Row in Table.Rows)
                {
                    ICNid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                    PublisherName = Convert.ToString(Row[IndicatorClassifications.Publisher]) + "_";
                    ICName = Convert.ToString(Row[IndicatorClassifications.ICName]);

                    if (ICName.StartsWith(PublisherName))
                    {
                        ICName = ICName.Substring(PublisherName.Length);
                    }
                    Index = ICName.LastIndexOf("_");

                    if (Index >= 0)
                    {
                        ICName = ICName.Substring(Index + 1);

                        if (ICName.Length > 10)
                        {
                            ICName = string.Empty;
                        }
                    }
                    else
                    {
                        ICName = string.Empty;
                    }

                    // update diyear into database
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateYearColumn(this.DBQueries.TablesName.IndicatorClassifications, ICNid, ICName));

                }


                // 1.3 update title
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateTitleColumn(ServerType, this.DBQueries.TablesName.IndicatorClassifications));

                // 2. update Publisher, title & year in other language tables
                foreach (DataRow LanguageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = Convert.ToString(LanguageRow[Language.LanguageCode]);

                    // update all Language tables except default langauge table
                    if (("_" + LanguageCode) != this.DBQueries.LanguageCode)
                    {
                        TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdatePublisherTitleYearInOtherLanguages(this.DBQueries.TablesName.IndicatorClassifications, TablesName.IndicatorClassifications));
                    }
                }
            }
            catch (Exception ex)
            {
                DevInfo.Lib.DI_LibBAL.ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// To Import indicator classification into database or template
        /// </summary>
        /// <param name="ICInfo">Instance of IndicatorClassificationInfo</param>
        /// <param name="NidInSourceDB"></param>
        /// <param name="sourceQurey"></param>
        /// <param name="sourceDBConnection"></param>
        /// <returns>new indicator classification nid</returns>
        public int ImportIndicatorClassification(ICType icType, string icName, string icGId, string parentICGId, bool isGlobal)
        {
            int RetVal = -1;
            bool ISTrgICGlobal = false;
            DataTable TempTable;
            IndicatorClassificationInfo ICInfo = new IndicatorClassificationInfo(icName, icType, isGlobal, 0);
            ICInfo.GID = icGId;
            string LangCode = this.DBQueries.LanguageCode;

            try
            {
                ICInfo.Parent = new IndicatorClassificationInfo();
                if (parentICGId == "-1")
                {
                    ICInfo.Parent.Nid = -1;
                }
                else
                {
                    ICInfo.Parent.GID = parentICGId;
                    ICInfo.Parent.Nid = this.GetNidByGID(parentICGId, icType);
                }
                //check item is already exist in database or not
                RetVal = this.GetIndicatorClassificationNid(ICInfo.GID, ICInfo.Name, ICInfo.Parent.Nid, ICInfo.Type);

                if (RetVal > 0)
                {
                    if (!this.DBQueries.LanguageCode.StartsWith("_"))
                    {
                        LangCode = "_" + LangCode;
                    }

                    // check target ic is global 
                    TempTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, RetVal.ToString(), FieldSelection.Light));
                    if (TempTable.Rows.Count > 0)
                    {
                        ISTrgICGlobal = Convert.ToBoolean(TempTable.Rows[0][IndicatorClassifications.ICGlobal]);
                    }

                    // if target item is  not global
                    if (!ISTrgICGlobal)
                    {
                        //update the gid,name and global on the basis of nid
                        this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UpdateIC(this.DBQueries.DataPrefix, LangCode,
                           ICInfo.Name, ICInfo.GID, ICInfo.IsGlobal, ICInfo.Parent.Nid, ICInfo.ClassificationInfo, ICInfo.Type, RetVal));
                    }

                }
                else
                {
                    if (this.InsertIntoDatabase(ICInfo))
                    {
                        //get nid
                        RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                    }
                }

                // update UT_CF_FLOWCHART table
                if (ICInfo.Type == ICType.CF)
                {
                    //--
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return RetVal;
        }

        /// <summary>
        /// Updates IC info 
        /// </summary>
        /// <param name="ICNid"></param>
        /// <param name="infoText"></param>
        public void UpdateICInfo(int ICNid, string infoText)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateICInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, infoText, ICNid));
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }            
        }

        #endregion

        #endregion

    }
}
