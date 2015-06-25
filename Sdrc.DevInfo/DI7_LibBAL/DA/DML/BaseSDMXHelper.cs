using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using SDMXConstants = DevInfo.Lib.DI_LibSDMX.Constants;
using DevInfo.Lib.DI_LibSDMX;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Export;
using SDMXObjectModel.Data.StructureSpecific;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.IO;
using DevInfo.Lib.DI_LibBAL.DA.DML;



namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class BaseSDMXHelper
    {

        private DIConnection DBConnection = null;

        private DIQueries DBQueries = null;

        #region "-- Private --"


        private string GetSubgroupValGId(Dictionary<string, string> DictSubgroupBreakup, DataTable DtSubgroupBreakup)
        {
            string RetVal;
            bool IsSetFlag;
            string SubgroupValGId, SubgroupTypeGId, SubgroupGId;
            List<string> ProcessedSubgroupValGIds;
            DataRow[] SubgroupValRows;

            RetVal = string.Empty;
            IsSetFlag = false;
            SubgroupValGId = string.Empty;
            SubgroupTypeGId = string.Empty;
            SubgroupGId = string.Empty;
            ProcessedSubgroupValGIds = new List<string>();
            SubgroupValRows = null;

            try
            {
                foreach (DataRow DrSubgroupBreakup in DtSubgroupBreakup.Rows)
                {
                    SubgroupValGId = DrSubgroupBreakup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                    if (!ProcessedSubgroupValGIds.Contains(SubgroupValGId))
                    {
                        ProcessedSubgroupValGIds.Add(SubgroupValGId);

                        SubgroupValRows = DtSubgroupBreakup.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + SDMXConstants.EqualsTo + SDMXConstants.Apostophe + SubgroupValGId + SDMXConstants.Apostophe);

                        if (SubgroupValRows.Length == DictSubgroupBreakup.Count)
                        {
                            foreach (DataRow DrSubgroupVal in SubgroupValRows)
                            {
                                SubgroupTypeGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString();
                                SubgroupGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString();

                                if (DictSubgroupBreakup.ContainsKey(SubgroupTypeGId) && DictSubgroupBreakup[SubgroupTypeGId] == SubgroupGId)
                                {
                                    IsSetFlag = true;
                                }
                                else
                                {
                                    IsSetFlag = false;
                                    break;
                                }
                            }

                            if (IsSetFlag == true)
                            {
                                RetVal = SubgroupValGId;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        private DataTable GetSubgroupValTable(Dictionary<string, SubgroupValInfo> subgroupValDetails)
        {
            DataTable RetVal = null;
            SubgroupValInfo SGInfo = new SubgroupValInfo();

            try
            {
                RetVal = new DataTable();
                RetVal.Columns.Add(SubgroupVals.SubgroupValGId);
                RetVal.Columns.Add(SubgroupVals.SubgroupVal);
                RetVal.Columns.Add(SubgroupTypes.SubgroupTypeGID);
                RetVal.Columns.Add(Subgroup.SubgroupGId);

                foreach (string SGVGId in subgroupValDetails.Keys)
                {
                    SGInfo = subgroupValDetails[SGVGId];

                    DataRow DrRow = RetVal.NewRow();

                    DrRow[SubgroupVals.SubgroupValGId] = SGInfo.GID;
                    DrRow[SubgroupVals.SubgroupVal] = SGInfo.Name;

                    if (!string.IsNullOrEmpty(SGInfo.Sex.GID))
                    {
                        DrRow[SubgroupTypes.SubgroupTypeGID] = SubgroupType.Sex.ToString().ToUpper();
                        DrRow[Subgroup.SubgroupGId] = SGInfo.Sex.GID;
                    }
                    if (!string.IsNullOrEmpty(SGInfo.Age.GID))
                    {
                        DrRow[SubgroupTypes.SubgroupTypeGID] = SubgroupType.Age.ToString().ToUpper();
                        DrRow[Subgroup.SubgroupGId] = SGInfo.Age.GID;
                    }
                    if (!string.IsNullOrEmpty(SGInfo.Location.GID))
                    {
                        DrRow[SubgroupTypes.SubgroupTypeGID] = SubgroupType.Location.ToString().ToUpper();
                        DrRow[Subgroup.SubgroupGId] = SGInfo.Location.GID;
                    }
                    if (!string.IsNullOrEmpty(SGInfo.Others.GID))
                    {
                        DrRow[SubgroupTypes.SubgroupTypeGID] = SubgroupType.Others.ToString().ToUpper();
                        DrRow[Subgroup.SubgroupGId] = SGInfo.Others.GID;
                    }
                    RetVal.Rows.Add(DrRow);
                }

                RetVal.AcceptChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        #endregion

        public BaseSDMXHelper()
        {

        }

        public BaseSDMXHelper(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        /// <summary>
        /// IMport DSD file into database
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="dsdfileNameWPath"></param>
        /// <returns></returns>
        public bool Import_DSD(SDMXSchemaType schemaType, string dsdfileNameWPath)
        {
            bool RetVal = false;

            RetVal = Import_DSD(schemaType, dsdfileNameWPath, false);

            return RetVal;
        }

        public bool Import_DSD(SDMXSchemaType schemaType, string dsdfileNameWPath, bool includeSource)
        {
            bool RetVal = false;
            SDMXObjectModel.Message.StructureType DSDStructureObj = null;
            string IUSGID = string.Empty;
            string ICGId = string.Empty;
            string IndicatorGId = string.Empty;
            string UnitGId = string.Empty;
            string SGVGId = string.Empty;
            int IncrementCounter = 0;
            int TotalCount = 0;
            Dictionary<string, int> IndList = null;
            Dictionary<string, int> UnitList = null;
            Dictionary<string, int> SGVList = null;
            Dictionary<string, int> AreaList = null;
            Dictionary<string, int> ICList = null;
            Dictionary<string, int> SourceList = null;
            Dictionary<string, int> IUSList = null;
            Dictionary<string, int> MDCatList = null;
            Dictionary<string, Dictionary<string, string>> ICWParentList = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> ICIUSList = new Dictionary<string, string>();
            Dictionary<string, List<string>> ICTypeICList = new Dictionary<string, List<string>>();


            try
            {
                //  SDMXHelper.DBConnection =

                //-- Load DSD File
                DSDStructureObj = new SDMXObjectModel.Message.StructureType();

                DSDStructureObj = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), dsdfileNameWPath);

                //-- Initialize Process
                //SDMXHelper.Raise_Initilize_Process_Event("Importing DSD...", 1, 10);

                MDCatList = this.Import_MetdataCategory(DSDStructureObj);

                //     //-- Add Indicator Into Database
                IndList = this.Import_Indicators(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //-- Add Unit Into Database
                UnitList = this.Import_Units(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //     //-- Add SubgroupTypes Into Database
                this.Import_SubgroupType(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //     //-- Add Subgroup Into Database
                this.Import_Subgroup(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //     //-- Add SubgroupVal Into Database
                SGVList = this.Import_SubgroupVal(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //     //-- Add Areas Into Database
                AreaList = this.Import_Area(DSDStructureObj);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                // -- Add IndicatorClassification Into Database
                ICList = this.Import_IC(DSDStructureObj);

                if (includeSource)
                {
                    SourceList = this.Import_Source(DSDStructureObj);
                }

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //-- Get ISU
                IUSList = this.Import_IUS(DSDStructureObj, IndList, UnitList, SGVList);

                //SDMXHelper.Raise_Notify_Progress_Event(IncrementCounter += 1);
                //     //SDMXHelper.Raise_ProgressBar_Increment(IncrementCounter += 5);
                ICIUSList = this.Import_IC_IUS(DSDStructureObj, IUSList, ICList);


                //SDMXHelper.Raise_Notify_Progress_Event(10);
                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }

            return RetVal;

        }
        /// <summary>
        /// Generate MSD
        /// </summary>
        /// <param name="schemaType"></param>
        /// <param name="metadataElementType"></param>
        /// <param name="outPutFolderPath"></param>
        /// <returns></returns>
        public bool Generate_MSD(SDMXSchemaType schemaType, MetadataElementType metadataElementType, string outPutFolderPath)
        {
            bool RetVal = false;
            string LanguageCode = string.Empty;

            MSDTypes MSDType = MSDTypes.ALL;

            if (metadataElementType == MetadataElementType.Indicator)
            {
                MSDType = MSDTypes.MSD_Indicator;
            }
            else if (metadataElementType == MetadataElementType.Area)
            {
                MSDType = MSDTypes.MSD_Area;
            }
            else if (metadataElementType == MetadataElementType.Source)
            {
                MSDType = MSDTypes.MSD_Source;
            }

            //List<ArtefactInfo> MSDArtifacts = SDMXUtility.Generate_MSD(schemaType, MSDType, "agency", LanguageCode, new DevInfo.Lib.DI_LibSDMX.Header(), outPutFolderPath, this.DBConnection, this.DBQueries);

            LanguageCode = this.DBQueries.LanguageCode.Replace("_", "");
            List<ArtefactInfo> MSDArtifacts = SDMXUtility.Generate_MSD(schemaType, MSDType, "agency", LanguageCode, new DevInfo.Lib.DI_LibSDMX.Header(), outPutFolderPath, this.DBConnection, this.DBQueries);

            if (MSDArtifacts.Count > 0)
            {
                RetVal = true;
            }

            return RetVal;
        }

        #region "Import DSD"

        /// <summary>
        /// Get_Indicator_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_Indicators(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            IndicatorBuilder IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            string IndicatorName = string.Empty;
            string IndicatorGID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int IndicatorNID = 0;
            bool IndicatorGlobal = false;
            bool HighIsGood = false;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.Indicator.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        IndicatorGID = IndCode.id;
                        IndicatorName = string.Empty;
                        IndicatorGlobal = false;
                        foreach (TextType IndTextType in IndCode.Name)
                        {
                            if (IndTextType.lang.Trim('_') == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                IndicatorName = IndTextType.Value;
                                break;
                            }
                        }

                        foreach (AnnotationType AnnType in IndCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    IndicatorGlobal = Convert.ToBoolean(AnnTextType.Value);
                                }
                            }
                            else if (AnnType.AnnotationTitle == SDMXConstants.Annotations.HighIsGood)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    HighIsGood = Convert.ToBoolean(AnnTextType.Value);
                                }
                            }
                        }

                        //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                        IndicatorNID = IndicatorBuilderObj.ImportIndicator(IndicatorName, IndicatorGID, IndicatorGlobal, HighIsGood);

                        if (!RetVal.ContainsKey(IndicatorGID))
                        {
                            RetVal.Add(IndicatorGID, IndicatorNID);
                        }
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get_Unit_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_Units(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            UnitBuilder BuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            bool UnitGlobal = false;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.Unit.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        GID = IndCode.id;
                        Name = string.Empty;
                        UnitGlobal = false;
                        foreach (TextType IndTextType in IndCode.Name)
                        {
                            if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                Name = IndTextType.Value;
                                break;
                            }
                        }

                        foreach (AnnotationType AnnType in IndCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    UnitGlobal = Convert.ToBoolean(AnnTextType.Value);
                                    break;
                                }
                            }
                        }


                        //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                        NID = BuilderObj.ImportUnit(GID, Name, UnitGlobal);

                        if (!RetVal.ContainsKey(GID))
                        {
                            RetVal.Add(GID, NID);
                        }
                    }
                }
            }



            return RetVal;
        }

        /// <summary>
        /// Get_SubgroupVal_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_SubgroupVal(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            DI6SubgroupValBuilder BuilderObj = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupBuilder SGBuilderObj = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            bool ISGlobal = false;
            string SubgroupDimensions = string.Empty;

            try
            {
                foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
                {
                    if (CodeListObj.id == SDMXConstants.CodeList.SubgroupVal.Id)
                    {
                        foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                        {
                            GID = IndCode.id;
                            Name = string.Empty;
                            ISGlobal = false;
                            foreach (TextType IndTextType in IndCode.Name)
                            {
                                if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                                {
                                    Name = IndTextType.Value;
                                    break;
                                }
                            }

                            foreach (AnnotationType AnnType in IndCode.Annotations)
                            {
                                if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                                {
                                    foreach (TextType AnnTextType in AnnType.AnnotationText)
                                    {
                                        ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                                    }
                                }
                                else if (AnnType.AnnotationTitle == SDMXConstants.Annotations.Breakup)
                                {
                                    foreach (TextType AnnTextType in AnnType.AnnotationText)
                                    {
                                        SubgroupDimensions = AnnTextType.Value;
                                    }
                                }
                            }

                            //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                            NID = BuilderObj.ImportSubgroupVal(Name, GID, ISGlobal);

                            int SGNID = 0;
                            if (SubgroupDimensions != null)
                            {
                                foreach (string SgText in SubgroupDimensions.Split(','))
                                {
                                    string SGType = SgText.Split('=')[0].Trim();
                                    string SGGID = SgText.Split('=')[1].Trim();

                                    //-- Get Subgroup Dimension value NID
                                    SGNID = SGBuilderObj.GetSubgroupNid(SGGID, "");

                                    if (SGNID > 0)
                                    {
                                        BuilderObj.InsertSubgroupValRelations(NID, SGNID);
                                    }
                                }
                            }

                            if (!RetVal.ContainsKey(GID))
                            {
                                RetVal.Add(GID, NID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }            

            return RetVal;
        }

        /// <summary>
        /// Get_Subgroup_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_Subgroup(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            DI6SubgroupBuilder BuilderObj = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string SubgroupTypeGID = string.Empty;
            string SubgroupGID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            bool ISGlobal = false;
            string SubgroupDimID = string.Empty;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.SubgroupType.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        SubgroupTypeGID = IndCode.id;
                        SubgroupDimID = "CL_" + SubgroupTypeGID;

                        foreach (SDMXObjectModel.Structure.CodelistType SCodeListObj in dsdStructure.Structures.Codelists)
                        {
                            if (SCodeListObj.id == SubgroupDimID)
                            {
                                foreach (SDMXObjectModel.Structure.CodeType SGCode in SCodeListObj.Items)
                                {
                                    foreach (TextType IndTextType in SGCode.Name)
                                    {
                                        SubgroupGID = SGCode.id;
                                        Name = string.Empty;
                                        ISGlobal = false;
                                        if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                                        {
                                            Name = IndTextType.Value;
                                            break;
                                        }
                                    }

                                    foreach (AnnotationType AnnType in SGCode.Annotations)
                                    {
                                        if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                                        {
                                            foreach (TextType AnnTextType in AnnType.AnnotationText)
                                            {
                                                ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                                                break;
                                            }
                                        }
                                    }

                                    //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                                    NID = BuilderObj.ImportSubgroup(Name, SubgroupGID, SubgroupTypeGID, ISGlobal);

                                    if (!RetVal.ContainsKey(SubgroupGID))
                                    {
                                        RetVal.Add(SubgroupGID, NID);
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return RetVal;
        }

        /// <summary>
        /// Get_SubgroupType_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_SubgroupType(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            DI6SubgroupTypeBuilder BuilderObj = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            bool ISGlobal = false;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.SubgroupType.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        GID = IndCode.id;
                        Name = string.Empty;
                        ISGlobal = false;
                        foreach (TextType IndTextType in IndCode.Name)
                        {
                            if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                Name = IndTextType.Value;
                                break;
                            }
                        }

                        foreach (AnnotationType AnnType in IndCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                                    break;
                                }
                            }
                        }

                        //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                        NID = BuilderObj.ImportSubgroupType(Name, GID, ISGlobal);

                        if (!RetVal.ContainsKey(GID))
                        {
                            RetVal.Add(GID, NID);
                        }
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get_Area_Collection method..
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_Area(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            AreaBuilder BuilderObj = new AreaBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            string ParentID = string.Empty;
            bool ISGlobal = false;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in dsdStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.Area.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        GID = IndCode.id;
                        Name = string.Empty;
                        ISGlobal = false;
                        foreach (TextType IndTextType in IndCode.Name)
                        {
                            if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                Name = IndTextType.Value;
                                break;
                            }
                        }


                        foreach (AnnotationType AnnType in IndCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                                    // break;
                                }
                            }
                        }

                        foreach (LocalCodeReferenceType RefType in IndCode.Items)
                        {
                            ParentID = ((SDMXObjectModel.Common.RefBaseType)(RefType.Items[0])).id;
                        }

                        //  //SDMXHelper.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                        NID = BuilderObj.ImportArea(Name, GID, ParentID, ISGlobal);

                        if (!RetVal.ContainsKey(GID))
                        {
                            RetVal.Add(GID, NID);
                        }
                    }
                }
            }


            return RetVal;
        }


        private Dictionary<string, int> Import_IUS(SDMXObjectModel.Message.StructureType DSDStructureObj, Dictionary<string, int> indicatorList, Dictionary<string, int> unitList, Dictionary<string, int> subgroupValList)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            string IUSGID = string.Empty;
            string IndicatorGId = string.Empty;
            string UnitGId = string.Empty;
            string SGVGId = string.Empty;
            int IndicatorNId = 0;
            int UnitNId = 0;
            int SubgroupValNId = 0;
            bool IsDefault = false;

            IUSBuilder IUSBuilderObj = new IUSBuilder(this.DBConnection, this.DBQueries);
            IndicatorBuilder IndBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            UnitBuilder UnitBuilderObj = new UnitBuilder(this.DBConnection, this.DBQueries);
            DI6SubgroupValBuilder SGVBuilderObj = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);

            //-- Initialize Process
            //SDMXHelper.Raise_Initilize_Process_Event("Generating Complete DSD...", 1, 1);


            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in DSDStructureObj.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.IUS.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IUSCode in CodeListObj.Items)
                    {
                        IUSGID = IUSCode.id;
                        string[] GIDs;
                        GIDs = IUSGID.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        IndicatorGId = GIDs[0];
                        UnitGId = GIDs[1];
                        SGVGId = GIDs[2];
                        //-- Get IndicatorNid from GID
                        if (indicatorList == null || !indicatorList.ContainsKey(IndicatorGId))
                        {
                            IndicatorNId = IndBuilderObj.GetIndicatorNid(IndicatorGId, string.Empty);
                        }
                        else
                        {
                            IndicatorNId = indicatorList[IndicatorGId];
                        }
                        //-- Get UnitNId from GID
                        if (unitList == null || !unitList.ContainsKey(UnitGId))
                        {
                            UnitNId = UnitBuilderObj.GetUnitNid(UnitGId, string.Empty);
                        }
                        else
                        {
                            UnitNId = unitList[UnitGId];
                        }

                        //-- Get SubgroupVal Nid from GID
                        if (subgroupValList == null || !subgroupValList.ContainsKey(SGVGId))
                        {
                            SubgroupValNId = SGVBuilderObj.GetSubgroupValNid(SGVGId, string.Empty);
                        }
                        else
                        {
                            SubgroupValNId = subgroupValList[SGVGId];
                        }

                        foreach (AnnotationType AnnType in IUSCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsDefault)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    IsDefault = Convert.ToBoolean(AnnTextType.Value);
                                }
                            }
                        }

                        if (IndicatorNId > 0 && UnitNId > 0 && SubgroupValNId > 0)
                        {
                            int IUSNId = IUSBuilderObj.GetIUSNid(IndicatorNId, UnitNId, SubgroupValNId);
                            if (IUSNId <= 0)
                            {
                                IUSNId = IUSBuilderObj.InsertIUS(IndicatorNId, UnitNId, SubgroupValNId, 0, 0);
                            }
                            if (IUSNId > 0)
                            {
                                IUSBuilderObj.UpdateIUSISDefaultSubgroup(IUSNId.ToString(), IsDefault);
                            }

                            //-- Add IUSGID with IUSNId into collection
                            if (!RetVal.ContainsKey(IUSGID))
                            {
                                RetVal.Add(IUSGID, IUSNId);
                            }
                        }
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Get_IC_Collection method.. 
        /// </summary>
        /// <param name="dsdStructure"></param>
        /// <returns></returns>
        private Dictionary<string, int> Import_IC(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            IndicatorClassificationBuilder BuilderObj = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
            SourceBuilder SrcBuilderObj = new SourceBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            string ParentID = string.Empty;
            ICType icICType = ICType.Sector;
            Dictionary<string, string> CParent = new Dictionary<string, string>();

            bool ISGlobal = false;

            foreach (SDMXObjectModel.Structure.CategorySchemeType CodeListObj in dsdStructure.Structures.CategorySchemes)
            {
                icICType = (ICType)DIQueries.ICTypeText.Values.IndexOf("'" + CodeListObj.id.Substring(3) + "'");

                if (icICType != ICType.Source)
                {
                    foreach (SDMXObjectModel.Structure.CategoryType CatTypeCode in CodeListObj.Items)
                    {
                        GID = CatTypeCode.id;
                        Name = string.Empty;
                        ISGlobal = false;

                        foreach (TextType IndTextType in CatTypeCode.Name)
                        {
                            if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                Name = IndTextType.Value;
                                break;
                            }
                        }

                        foreach (AnnotationType AnnType in CatTypeCode.Annotations)
                        {
                            if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                            {
                                foreach (TextType AnnTextType in AnnType.AnnotationText)
                                {
                                    ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                                    break;
                                }
                            }
                        }

                        ParentID = "-1";


                        NID = BuilderObj.ImportIndicatorClassification(icICType, Name, GID, ParentID, ISGlobal);

                        if (!RetVal.ContainsKey(GID) && NID > 0)
                        {
                            RetVal.Add(GID, NID);
                        }

                        // Import each child IC for current IC
                        Import_ICChain(RetVal, BuilderObj, GID, icICType, CatTypeCode);
                    }
                }
            }

            return RetVal;
        }

        private Dictionary<string, int> Import_Source(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            SourceBuilder SrcBuilderObj = null;
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            string ParentID = string.Empty;
            ICType icICType = ICType.Sector;

            SrcBuilderObj = new SourceBuilder(this.DBConnection, this.DBQueries);

            foreach (SDMXObjectModel.Structure.CategorySchemeType CodeListObj in dsdStructure.Structures.CategorySchemes)
            {
                icICType = (ICType)DIQueries.ICTypeText.Values.IndexOf("'" + CodeListObj.id.Substring(3) + "'");

                if (icICType == ICType.Source)
                {
                    foreach (SDMXObjectModel.Structure.CategoryType CatTypeCode in CodeListObj.Items)
                    {
                        GID = CatTypeCode.id;
                        string ParentName = string.Empty;

                        foreach (TextType IndTextType in CatTypeCode.Name)
                        {
                            if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                ParentName = IndTextType.Value;
                            }

                        }

                        foreach (SDMXObjectModel.Structure.CategoryType SubCatTypeCode in CatTypeCode.Items)
                        {
                            GID = CatTypeCode.id;
                            Name = string.Empty;

                            foreach (TextType IndTextType in SubCatTypeCode.Name)
                            {
                                if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                                {
                                    Name = IndTextType.Value;
                                    break;
                                }
                            }
                        }
                        ParentID = "-1";

                        NID = SrcBuilderObj.CheckNCreateSource(Name);

                        if (!RetVal.ContainsKey(GID) && NID > 0)
                        {
                            RetVal.Add(GID, NID);
                        }
                    }
                }
            }

            return RetVal;
        }

        private void Import_ICChain(Dictionary<string, int> RetVal, IndicatorClassificationBuilder BuilderObj, string parentICGID, ICType icICType, SDMXObjectModel.Structure.CategoryType CatTypeCode)
        {
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            int NID = 0;
            bool ISGlobal = false;

            // Read each child IC for current IC
            foreach (SDMXObjectModel.Structure.CategoryType SubCateType in CatTypeCode.Items)
            {
                GID = SubCateType.id;
                Name = string.Empty;
                ISGlobal = false;

                foreach (TextType IndTextType in SubCateType.Name)
                {
                    if (IndTextType.lang == this.DBQueries.LanguageCode.Trim('_'))
                    {
                        Name = IndTextType.Value;
                        break;
                    }
                }

                foreach (AnnotationType AnnType in SubCateType.Annotations)
                {
                    if (AnnType.AnnotationTitle == SDMXConstants.Annotations.IsGlobal)
                    {
                        foreach (TextType AnnTextType in AnnType.AnnotationText)
                        {
                            ISGlobal = Convert.ToBoolean(AnnTextType.Value);
                            break;
                        }
                    }
                }

                NID = BuilderObj.ImportIndicatorClassification(icICType, Name, GID, parentICGID, ISGlobal);

                if (!RetVal.ContainsKey(GID) && NID > 0)
                {
                    RetVal.Add(GID, NID);
                }

                this.Import_ICChain(RetVal, BuilderObj, GID, icICType, SubCateType);

            }
        }

        /// <summary>
        /// Get_IC_IUS_List method..
        /// </summary>
        /// <param name="DSDStructure"></param>
        /// <returns></returns>
        private Dictionary<string, string> Import_IC_IUS(SDMXObjectModel.Message.StructureType DSDStructure, Dictionary<string, int> iusList, Dictionary<string, int> icList)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();
            IndicatorClassificationBuilder ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
            string IUSGID = string.Empty;
            string ICGID = string.Empty;
            int ICNId = 0;
            int IUSNId = 0;

            foreach (SDMXObjectModel.Structure.CategorisationType CodeListObj in DSDStructure.Structures.Categorisations)
            {
                if (CodeListObj.id.StartsWith("CTGZ_"))
                {
                    IUSGID = CodeListObj.id.Substring(5);
                }

                foreach (SDMXObjectModel.Common.RefBaseType TargetObj in CodeListObj.Target.Items)
                {
                    ICGID = TargetObj.id;
                    break;
                }

                IUSGID = IUSGID.Replace(ICGID + "@", "");

                ICNId = 0;
                IUSNId = 0;
                if (iusList.ContainsKey(IUSGID))
                {
                    IUSNId = iusList[IUSGID];
                }
                if (icList.ContainsKey(ICGID))
                {
                    ICNId = icList[ICGID];
                }
                if (ICNId > 0 && IUSNId > 0)
                {
                    ICBuilder.AddNUpdateICIUSRelation(ICNId, IUSNId, false);
                }
                if (!RetVal.ContainsKey(IUSGID))
                {
                    RetVal.Add(IUSGID, ICGID);
                }
            }

            return RetVal;
        }

        private Dictionary<string, int> Import_MetdataCategory(SDMXObjectModel.Message.StructureType dsdStructure)
        {
            Dictionary<string, int> RetVal = new Dictionary<string, int>();
            IndicatorBuilder IndicatorBuilderObj = new IndicatorBuilder(this.DBConnection, this.DBQueries);
            string IndicatorName = string.Empty;
            string IndicatorGID = string.Empty;
            string LanguageCode = this.DBQueries.LanguageCode.Trim('_');
            Dictionary<string, string> MDIndicators = new Dictionary<string, string>();
            Dictionary<string, string> MDArea = new Dictionary<string, string>();
            Dictionary<string, string> MDSource = new Dictionary<string, string>();
            try
            {
                MDIndicators = GetMetdataCategoryList(dsdStructure.Structures.Concepts, SDMXConstants.ConceptScheme.MSD_Indicator.Id);
                MDArea = GetMetdataCategoryList(dsdStructure.Structures.Concepts, SDMXConstants.ConceptScheme.MSD_Area.Id);
                MDSource = GetMetdataCategoryList(dsdStructure.Structures.Concepts, SDMXConstants.ConceptScheme.MSD_Source.Id);

                if (MDIndicators.Count > 0)
                    this.CreateMetadataCategoriesFromDSDStructure(dsdStructure, MDIndicators, SDMXConstants.MSD.Indicator.Id, MetadataElementType.Indicator);

                if (MDArea.Count > 0)
                    this.CreateMetadataCategoriesFromDSDStructure(dsdStructure, MDArea, SDMXConstants.MSD.Area.Id, MetadataElementType.Area);

                if (MDSource.Count > 0)
                    this.CreateMetadataCategoriesFromDSDStructure(dsdStructure, MDSource, SDMXConstants.MSD.Source.Id, MetadataElementType.Source);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }

        private void CreateMetadataCategoriesFromDSDStructure(SDMXObjectModel.Message.StructureType dsdStructure, Dictionary<string, string> MDCateList, string strictureID, MetadataElementType mdElementType)
        {
            foreach (SDMXObjectModel.Structure.MetadataStructureType MDStructObj in dsdStructure.Structures.MetadataStructures)
            {
                if (MDStructObj.id == strictureID)
                {
                    foreach (SDMXObjectModel.Structure.ComponentListType ComponentObj in ((SDMXObjectModel.Structure.StructureType)(MDStructObj)).Item.Items)
                    {
                        if (ComponentObj.GetType().ToString() == "SDMXObjectModel.Structure.ReportStructureType")
                        {
                            foreach (SDMXObjectModel.Structure.MetadataAttributeType MDCategoryObj in ComponentObj.Items)
                            {
                                this.CheckAndCreateMetedataCategory(MDCateList, mdElementType, MDCategoryObj, -1);

                            }
                        }
                    }
                }
            }
        }

        private void CheckAndCreateMetedataCategory(Dictionary<string, string> MDCateList, MetadataElementType mdElementType, SDMXObjectModel.Structure.MetadataAttributeType MDCategoryObj, int parentCategoryNid)
        {
            DI7MetadataCategoryBuilder MDCatBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
            DI7MetadataCategoryInfo CatInfo = new DI7MetadataCategoryInfo();
            CatInfo.CategoryGID = MDCategoryObj.id;
            CatInfo.CategoryName = MDCateList[MDCategoryObj.id];
            CatInfo.CategoryType = DIQueries.MetadataElementTypeText[mdElementType];
            CatInfo.ParentNid = parentCategoryNid;
            CatInfo.CategoryNId = MDCatBuilder.CheckNInsertCategory(CatInfo);

            foreach (SDMXObjectModel.Structure.MetadataAttributeType ChildMDCategory in MDCategoryObj.MetadataAttribute)
            {
                CheckAndCreateMetedataCategory(MDCateList, mdElementType, ChildMDCategory, CatInfo.CategoryNId);
            }
        }

        private Dictionary<string, string> GetMetdataCategoryList(List<SDMXObjectModel.Structure.ConceptSchemeType> dsdConcepts, string mdConceptID)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();

            foreach (SDMXObjectModel.Structure.ConceptSchemeType MDConcepts in dsdConcepts)
            {
                if (MDConcepts.id == mdConceptID)
                {
                    foreach (SDMXObjectModel.Structure.ItemType MDItem in MDConcepts.Items)
                    {
                        foreach (TextType MDValue in MDItem.Name)
                        {
                            if (MDValue.lang.Trim('_') == this.DBQueries.LanguageCode.Trim('_'))
                            {
                                if (!RetVal.ContainsKey(MDItem.id))
                                {
                                    RetVal.Add(MDItem.id, MDValue.Value);
                                }
                            }
                        }
                    }
                }
            }

            return RetVal;
        }

        #endregion "Import DSD"


        #region "-- DES --"

        /// <summary>
        /// Indicator_Name, Unit_Name, Subgroup_Val, TimePeriod,Area_ID,Area_Name,Data_Value,FootNote,Data_Denominator, Source As IC_Name, Indicator_GId, Unit_GId, Subgroup_Val_GId 
        /// </summary>
        /// <returns></returns>
        private DataTable Create_Table_For_DES()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibBAL.Export.Constants.SectorColumnName);
            RetVal.Columns.Add(DevInfo.Lib.DI_LibBAL.Export.Constants.ClassColumnName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(Timeperiods.TimePeriod);
            RetVal.Columns.Add(Area.AreaID);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Data.DataValue);
            RetVal.Columns.Add(FootNotes.FootNote);
            RetVal.Columns.Add(Data.DataDenominator);
            RetVal.Columns.Add(IndicatorClassifications.ICName);
            RetVal.Columns.Add(Indicator.IndicatorGId);
            RetVal.Columns.Add(Unit.UnitGId);
            RetVal.Columns.Add(SubgroupVals.SubgroupValGId);

            RetVal.AcceptChanges();

            return RetVal;
        }

        internal bool Generate_DES(SDMXSchemaType schemaType, string DESOutputFileNameWPath, string dataFileNameWithPath)
        {
            bool RetVal = false;

            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType DataObj = null;
            DataTable DTForDESGeneration = null;
            bool AddRow = true;
            DataTable IUSTable = null;
            DataTable ICTable = null;
            DataTable AreaTable = null;
            DataTable ICIUSTable = null;
            DIExport DIExportObj = new DIExport();
            int TotalCount = 0;
            int ProgressCount = 0;

            try
            {
                DataObj = new SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType();
                DataObj = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), dataFileNameWithPath);

                //-- Load SDMX-Ml Data file
                foreach (TimeSeriesDataSetType DataSetObj in DataObj.DataSet)
                {
                    TotalCount += DataSetObj.Items.Count;
                }

                //((SDMXObjectModel.Data.StructureSpecific.SeriesType)(DataSetObj.Items[0])).Obs

                ////-- Initialize Progress
                SDMXHelper.Raise_Initilize_Process_Event("Generating DES ...", TotalCount, TotalCount + 10);
                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                IUSTable = DBConnection.ExecuteDataTable(DBQueries.IUS.GetIUS(FilterFieldType.None, "", FieldSelection.Light));

                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);
                AreaTable = DBConnection.ExecuteDataTable(DBQueries.Area.GetArea(FilterFieldType.None, ""));

                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);
                ICTable = DBConnection.ExecuteDataTable(DBQueries.IndicatorClassification.GetIC(FilterFieldType.None, "", FieldSelection.Light));

                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);
                ICIUSTable = DBConnection.ExecuteDataTable(DBQueries.IUS.GetIUSByIC(ICType.Sector, "", FieldSelection.Light));

                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);
                //-- Create Blank Table For DES DataTable
                DTForDESGeneration = this.Create_Table_For_DES();

                DataTable SubgroupBreakupTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                SubgroupBreakupTable = SubgroupBreakupTable.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId);

                //TimeSeriesDataSetType DataSetObj in DataObj.DataSet
                foreach (SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType DataSetObj in DataObj.DataSet)
                {
                    foreach (SDMXObjectModel.Data.StructureSpecific.SeriesType SeriesObj in DataSetObj.Items)
                    {
                        string IndicatorGID = string.Empty;
                        string UnitGID = string.Empty;
                        string AreaID = string.Empty;
                        string SubgroupValGID = string.Empty;
                        string CombinedSubgroupGID = string.Empty;

                        Dictionary<string, string> SubgroupBreakup = new Dictionary<string, string>();
                        string Source = string.Empty;
                        foreach (XmlAttribute XMLAtrr in SeriesObj.AnyAttr)
                        {
                            if (XMLAtrr.Name == SDMXConstants.Concept.INDICATOR.Id)
                            {
                                IndicatorGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.UNIT.Id)
                            {
                                UnitGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.AREA.Id)
                            {
                                AreaID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.SOURCE.Id)
                            {
                                Source = XMLAtrr.Value;
                            }
                            else
                            {
                                if (!SubgroupBreakup.ContainsKey(XMLAtrr.Name))
                                {
                                    if (XMLAtrr.Value != "NA")
                                    {
                                        SubgroupBreakup.Add(XMLAtrr.Name, XMLAtrr.Value);
                                        CombinedSubgroupGID += XMLAtrr.Value;
                                    }
                                }
                            }
                        }

                        SubgroupValGID = GetSubgroupValGId(SubgroupBreakup, SubgroupBreakupTable);

                        foreach (SDMXObjectModel.Data.StructureSpecific.ObsType ObsObj in SeriesObj.Obs)
                        {
                            string FootnoteValue = string.Empty;
                            string Denominator = string.Empty;


                            foreach (XmlAttribute XMLAtrr in ObsObj.AnyAttr)
                            {
                                if (XMLAtrr.Name == SDMXConstants.Concept.FOOTNOTES.Id)
                                {
                                    FootnoteValue = Convert.ToString(XMLAtrr.Value);
                                }
                                else if (XMLAtrr.Name == SDMXConstants.Concept.DENOMINATOR.Id)
                                {
                                    Denominator = Convert.ToString(XMLAtrr.Value);
                                }
                                else if (XMLAtrr.Name == SDMXConstants.Concept.SOURCE.Id)
                                {
                                    Source = Convert.ToString(XMLAtrr.Value);
                                }
                            }

                            AddRow = true;
                            DataRow NewRow = DTForDESGeneration.NewRow();

                            IUSTable.DefaultView.RowFilter = Indicator.IndicatorGId + "='" + DICommon.RemoveQuotes(IndicatorGID) + "' AND " + Unit.UnitGId + "='" + DICommon.RemoveQuotes(UnitGID) + "' AND " + SubgroupVals.SubgroupValGId + "='" + DICommon.RemoveQuotes(SubgroupValGID) + "'";

                            //            //-- Get IndicatorName,Unit, Subgroups
                            if (IUSTable.DefaultView.Count > 0)
                            {
                                NewRow[Indicator.IndicatorName] = IUSTable.DefaultView[0][Indicator.IndicatorName];
                                NewRow[Unit.UnitName] = IUSTable.DefaultView[0][Unit.UnitName];
                                NewRow[SubgroupVals.SubgroupVal] = IUSTable.DefaultView[0][SubgroupVals.SubgroupVal];

                            }
                            else
                            {
                                NewRow[Indicator.IndicatorName] = DICommon.RemoveQuotes(IndicatorGID);
                                NewRow[Unit.UnitName] = DICommon.RemoveQuotes(UnitGID);

                                if (string.IsNullOrEmpty(SubgroupValGID))
                                {
                                    NewRow[SubgroupVals.SubgroupVal] = DICommon.RemoveQuotes(CombinedSubgroupGID);
                                }
                                else
                                {
                                    NewRow[SubgroupVals.SubgroupVal] = DICommon.RemoveQuotes(SubgroupValGID);
                                }

                            }

                            AreaTable.DefaultView.RowFilter = Area.AreaID + "='" + DICommon.RemoveQuotes(AreaID) + "'";
                            if (AreaTable.DefaultView.Count > 0)
                            {
                                NewRow[Area.AreaName] = AreaTable.DefaultView[0][Area.AreaName];
                            }
                            else
                            {
                                //-- Add AreaID for DES if missing in database
                                NewRow[Area.AreaName] = AreaID;
                            }

                            if (AddRow)
                            {
                                NewRow[Timeperiods.TimePeriod] = ObsObj.TIME_PERIOD;
                                NewRow[Area.AreaID] = DICommon.RemoveQuotes(AreaID);
                                NewRow[Data.DataValue] = DICommon.RemoveQuotes(ObsObj.OBS_VALUE);

                                NewRow[FootNotes.FootNote] = DICommon.RemoveQuotes(FootnoteValue);
                                NewRow[Data.DataDenominator] = Denominator;
                                NewRow[IndicatorClassifications.ICName] = DICommon.RemoveQuotes(Source);

                                NewRow[Indicator.IndicatorGId] = DICommon.RemoveQuotes(IndicatorGID);
                                NewRow[Unit.UnitGId] = DICommon.RemoveQuotes(UnitGID);
                                NewRow[SubgroupVals.SubgroupValGId] = DICommon.RemoveQuotes(SubgroupValGID);

                                ICIUSTable.DefaultView.RowFilter = Indicator.IndicatorGId + "='" + DICommon.RemoveQuotes(IndicatorGID) + "' AND " + Unit.UnitGId + "='" + DICommon.RemoveQuotes(UnitGID) + "' AND " + SubgroupVals.SubgroupValGId + "='" + DICommon.RemoveQuotes(SubgroupValGID) + "'";

                                if (ICIUSTable.DefaultView.Count > 0)
                                {
                                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICNId + "=" + ICIUSTable.DefaultView[0][IndicatorClassifications.ICNId];

                                    NewRow[Lib.DI_LibBAL.Export.Constants.SectorColumnName] = ICTable.DefaultView[0][IndicatorClassifications.ICName];
                                }
                                else
                                {
                                    NewRow[Lib.DI_LibBAL.Export.Constants.SectorColumnName] = string.Empty;
                                }

                                //-- Add Row Into Table
                                DTForDESGeneration.Rows.Add(NewRow);
                            }
                        }
                        //-- Raise Progress event
                        SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                    }
                }

                DTForDESGeneration.AcceptChanges();

                DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph.Fields Flds = null;

                ////-- Export DataTable to DES
                DIExportObj.ExportDataEntrySpreadsheet(true, DTForDESGeneration.DefaultView, true, Flds, DESOutputFileNameWPath, DICommon.LangaugeFileNameWithPath);

                //-- Raise Progress event
                SDMXHelper.Raise_Notify_File_Name_Event(DESOutputFileNameWPath);

                SDMXHelper.Raise_Notify_Progress_Event(TotalCount + 10);

                RetVal = true;
            }
            catch (Exception ex)
            {
                //ExceptionFacade.throwException(ex); 
            }


            return RetVal;
        }

        internal bool Generate_DES(SDMXSchemaType schemaType, string DESOutputFileNameWPath, string dataFileNameWithPath, string DSDFileNameWPath, string languagcode)
        {
            bool RetVal = false;

            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType DataStructureObj = null;
            DataTable DTForDESGeneration = null;
            bool AddRow = true;
            DIExport DIExportObj = new DIExport();
            int TotalCount = 0;
            int ProgressCount = 0;
            DSDHelper DSDHelperObj = null;
            string IUSGID = string.Empty;
            DataTable SubgroupBreakupTable = null;

            try
            {
                //-- Load SDMX Data File
                DataStructureObj = new SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType();
                DataStructureObj = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), dataFileNameWithPath);

                SDMXHelper.Raise_Initilize_Process_Event("Generating DES ...", TotalCount, TotalCount + 10);
                ////-- Initialize Progress
                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                //-- Load DSD file and Get Details
                DSDHelperObj = new DSDHelper(languagcode);
                DSDHelperObj.LoadDSD(DSDFileNameWPath);

                ////-- Initialize Progress
                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                //-- Load SDMX-Ml Data file
                foreach (TimeSeriesDataSetType DataSetObj in DataStructureObj.DataSet)
                {
                    TotalCount += DataSetObj.Items.Count;
                }

                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);
                //-- Create Blank Table For DES DataTable
                DTForDESGeneration = this.Create_Table_For_DES();

                SubgroupBreakupTable = this.GetSubgroupValTable(DSDHelperObj.SubgroupValDetails);

                //TimeSeriesDataSetType DataSetObj in DataObj.DataSet
                foreach (SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType DataSetObj in DataStructureObj.DataSet)
                {
                    foreach (SDMXObjectModel.Data.StructureSpecific.SeriesType SeriesObj in DataSetObj.Items)
                    {
                        string IndicatorGID = string.Empty;
                        string UnitGID = string.Empty;
                        string AreaID = string.Empty;
                        string SubgroupValGID = string.Empty;
                        string CombinedSubgroupGID = string.Empty;

                        Dictionary<string, string> SubgroupBreakup = new Dictionary<string, string>();

                        foreach (XmlAttribute XMLAtrr in SeriesObj.AnyAttr)
                        {
                            if (XMLAtrr.Name == SDMXConstants.Concept.INDICATOR.Id)
                            {
                                IndicatorGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.UNIT.Id)
                            {
                                UnitGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.AREA.Id)
                            {
                                AreaID = XMLAtrr.Value;
                            }
                            else
                            {
                                if (!SubgroupBreakup.ContainsKey(XMLAtrr.Name))
                                {
                                    if (XMLAtrr.Value != "NA")
                                    {
                                        SubgroupBreakup.Add(XMLAtrr.Name, XMLAtrr.Value);
                                        CombinedSubgroupGID += XMLAtrr.Value;
                                    }
                                }
                            }
                        }

                        //--
                        SubgroupValGID = this.GetSubgroupValGId(SubgroupBreakup, SubgroupBreakupTable);

                        foreach (SDMXObjectModel.Data.StructureSpecific.ObsType ObsObj in SeriesObj.Obs)
                        {
                            string FootnoteValue = string.Empty;
                            string Denominator = string.Empty;
                            string Source = string.Empty;

                            foreach (XmlAttribute XMLAtrr in ObsObj.AnyAttr)
                            {
                                if (XMLAtrr.Name == SDMXConstants.Concept.FOOTNOTES.Id)
                                {
                                    FootnoteValue = Convert.ToString(XMLAtrr.Value);
                                }
                                else if (XMLAtrr.Name == SDMXConstants.Concept.DENOMINATOR.Id)
                                {
                                    Denominator = Convert.ToString(XMLAtrr.Value);
                                }
                                else if (XMLAtrr.Name == SDMXConstants.Concept.SOURCE.Id)
                                {
                                    Source = Convert.ToString(XMLAtrr.Value);
                                }
                            }

                            AddRow = true;
                            DataRow NewRow = DTForDESGeneration.NewRow();

                            //IUSTable.DefaultView.RowFilter = Indicator.IndicatorGId + "='" + DICommon.RemoveQuotes(IndicatorGID) + "' AND " + Unit.UnitGId + "='" + DICommon.RemoveQuotes(UnitGID) + "' AND " + SubgroupVals.SubgroupValGId + "='" + DICommon.RemoveQuotes(SubgroupValGID) + "'";
                            IUSGID = IndicatorGID + "@" + UnitGID + "@" + SubgroupValGID;

                            if (DSDHelperObj.IUSDetails.ContainsKey(IUSGID))
                            {
                                NewRow[Indicator.IndicatorName] = DSDHelperObj.IndicatorDetails[IndicatorGID].Name;
                                NewRow[Unit.UnitName] = DSDHelperObj.UnitDetails[UnitGID].Name;
                                NewRow[SubgroupVals.SubgroupVal] = DSDHelperObj.SubgroupValDetails[SubgroupValGID].Name;
                            }
                            else
                            {
                                if (DSDHelperObj.IndicatorDetails.ContainsKey(IndicatorGID))
                                {
                                    NewRow[Indicator.IndicatorName] = DSDHelperObj.IndicatorDetails[IndicatorGID].Name;
                                }
                                else
                                {
                                    NewRow[Indicator.IndicatorName] = DICommon.RemoveQuotes(IndicatorGID);
                                }

                                if (DSDHelperObj.UnitDetails.ContainsKey(UnitGID))
                                {
                                    NewRow[Unit.UnitName] = DSDHelperObj.UnitDetails[UnitGID].Name;
                                }
                                else
                                {
                                    NewRow[Unit.UnitName] = DICommon.RemoveQuotes(UnitGID);
                                }

                                if (DSDHelperObj.SubgroupValDetails.ContainsKey(SubgroupValGID))
                                {
                                    NewRow[SubgroupVals.SubgroupVal] = DSDHelperObj.SubgroupValDetails[SubgroupValGID].Name;
                                }
                                else
                                {
                                    NewRow[SubgroupVals.SubgroupVal] = DICommon.RemoveQuotes(CombinedSubgroupGID);
                                }
                            }

                            if (DSDHelperObj.AreaDetails.ContainsKey(AreaID))
                            {
                                NewRow[Area.AreaName] = DSDHelperObj.AreaDetails[AreaID].Name;
                            }
                            else
                            {
                                //-- Add AreaID for DES if missing in database
                                NewRow[Area.AreaName] = DICommon.RemoveQuotes(AreaID);
                            }

                            if (AddRow)
                            {
                                NewRow[Timeperiods.TimePeriod] = ObsObj.TIME_PERIOD;
                                NewRow[Area.AreaID] = DICommon.RemoveQuotes(AreaID);
                                NewRow[Data.DataValue] = DICommon.RemoveQuotes(ObsObj.OBS_VALUE);

                                NewRow[FootNotes.FootNote] = DICommon.RemoveQuotes(FootnoteValue);
                                NewRow[Data.DataDenominator] = Denominator;
                                NewRow[IndicatorClassifications.ICName] = DICommon.RemoveQuotes(Source);

                                NewRow[Indicator.IndicatorGId] = DICommon.RemoveQuotes(IndicatorGID);
                                NewRow[Unit.UnitGId] = DICommon.RemoveQuotes(UnitGID);
                                NewRow[SubgroupVals.SubgroupValGId] = DICommon.RemoveQuotes(SubgroupValGID);

                                //-- Get Source
                                if (DSDHelperObj.ICIUSDetails.ContainsKey(IUSGID))
                                {
                                    NewRow[Lib.DI_LibBAL.Export.Constants.SectorColumnName] = DSDHelperObj.ICDetails[DSDHelperObj.ICIUSDetails[IUSGID].GID].Name;

                                    if (DSDHelperObj.ICDetails[DSDHelperObj.ICIUSDetails[IUSGID].GID].Parent != null && DSDHelperObj.ICDetails[DSDHelperObj.ICIUSDetails[IUSGID].GID].Parent.GID != "-1")
                                    {
                                        NewRow[Lib.DI_LibBAL.Export.Constants.ClassColumnName] = Convert.ToString(DSDHelperObj.ICDetails[DSDHelperObj.ICDetails[DSDHelperObj.ICIUSDetails[IUSGID].GID].Parent.GID].Name);
                                    }
                                }
                                else
                                {
                                    NewRow[Lib.DI_LibBAL.Export.Constants.SectorColumnName] = string.Empty;
                                }

                                //-- Add Row Into Table
                                DTForDESGeneration.Rows.Add(NewRow);
                            }
                        }
                        //-- Raise Progress event
                        SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                    }
                }

                DTForDESGeneration.AcceptChanges();

                DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph.Fields Flds = null;

                ////-- Export DataTable to DES
                DIExportObj.ExportDataEntrySpreadsheet(true, DTForDESGeneration.DefaultView, true, Flds, DESOutputFileNameWPath, DICommon.LangaugeFileNameWithPath);

                //-- Raise Progress event
                SDMXHelper.Raise_Notify_File_Name_Event(DESOutputFileNameWPath);

                SDMXHelper.Raise_Notify_Progress_Event(TotalCount + 10);

                RetVal = true;
            }
            catch (Exception ex)
            {
                //ExceptionFacade.throwException(ex); 
            }

            //////DIConnection DBConnection = null;
            //////DIQueries DBQueries = null;
            //////string TempDBFileName = Path.Combine(Path.GetDirectoryName(DESOutputFileNameWPath), Path.GetFileNameWithoutExtension(DESOutputFileNameWPath) + DICommon.FileExtension.Template);

            //////DevInfo.Lib.DI_LibDAL.Resources.Resource.GetBlankDevInfoDBFile(TempDBFileName);
            //////DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TempDBFileName, string.Empty, string.Empty);
            //////DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), DBConnection.DILanguageCodeDefault(DBConnection.DIDataSetDefault()));

            //////this.DBConnection = DBConnection;
            //////this.DBQueries = DBQueries;
            //////this.Import_DSD(schemaType, DSDFileNameWPath);

            ////////-- Generate DES File
            //////RetVal = this.Generate_DES(schemaType, DESOutputFileNameWPath, dataFileNameWithPath);
            //////DBConnection.Dispose();
            //////DBConnection = null;
            //////DBQueries.Dispose();
            //////DBQueries = null;
            //////try
            //////{
            //////    File.Delete(TempDBFileName);
            //////}
            //////catch
            //////{
            //////}




            return RetVal;
        }

        internal bool Import_SDMXML_Data(string SDMXFileNameWithPath)
        {
            bool RetVal = false;

            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType DataObj = null;
            DataTable DTForDESGeneration = null;
            bool AddRow = true;
            DataTable IUSTable = null;
            DataTable ICTable = null;
            DataTable AreaTable = null;
            DataTable ICIUSTable = null;
            DIExport DIExportObj = new DIExport();
            int TotalCount = 0;
            int ProgressCount = 0;

            try
            {
                DataObj = new SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType();
                DataObj = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), SDMXFileNameWithPath);

                //-- Load SDMX-Ml Data file
                foreach (TimeSeriesDataSetType DataSetObj in DataObj.DataSet)
                {
                    TotalCount += DataSetObj.Items.Count;
                }

                //((SDMXObjectModel.Data.StructureSpecific.SeriesType)(DataSetObj.Items[0])).Obs

                ////-- Initialize Progress
                SDMXHelper.Raise_Initilize_Process_Event("Importing Data...", TotalCount, TotalCount + 10);
                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);



                DataTable SubgroupBreakupTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                SubgroupBreakupTable = SubgroupBreakupTable.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId);

                Dictionary<string, int> IndicatorList = new Dictionary<string, int>();
                Dictionary<string, int> UnitList = new Dictionary<string, int>();
                Dictionary<string, int> AreaList = new Dictionary<string, int>();
                Dictionary<string, int> SgValList = new Dictionary<string, int>();
                Dictionary<string, int> TimepeirodList = new Dictionary<string, int>();
                Dictionary<string, int> SourceList = new Dictionary<string, int>();

                IndicatorBuilder IndBuilder = new IndicatorBuilder(DBConnection, DBQueries);
                UnitBuilder UBuilder = new UnitBuilder(DBConnection, DBQueries);
                DI6SubgroupValBuilder SGBuilder = new DI6SubgroupValBuilder(DBConnection, DBQueries);
                AreaBuilder AreaBuilderObj = new AreaBuilder(DBConnection, DBQueries);
                IndicatorClassificationBuilder ICBuilder = new IndicatorClassificationBuilder(DBConnection, DBQueries);
                TimeperiodBuilder TBuilder = new TimeperiodBuilder(DBConnection, DBQueries);
                SourceBuilder SrcBuilder = new SourceBuilder(DBConnection, DBQueries);
                DIDatabase DatabaseObj = new DIDatabase(DBConnection, DBQueries);
                IUSBuilder IUSBuilderObj = new IUSBuilder(DBConnection, DBQueries);
                string Source = string.Empty;


                //TimeSeriesDataSetType DataSetObj in DataObj.DataSet
                foreach (SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType DataSetObj in DataObj.DataSet)
                {
                    foreach (SDMXObjectModel.Data.StructureSpecific.SeriesType SeriesObj in DataSetObj.Items)
                    {
                        string IndicatorGID = string.Empty;
                        string UnitGID = string.Empty;
                        string AreaID = string.Empty;
                        string SubgroupValGID = string.Empty;
                        Dictionary<string, string> SubgroupBreakup = new Dictionary<string, string>();

                        foreach (XmlAttribute XMLAtrr in SeriesObj.AnyAttr)
                        {
                            Source = string.Empty;

                            if (XMLAtrr.Name == SDMXConstants.Concept.INDICATOR.Id)
                            {
                                IndicatorGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.UNIT.Id)
                            {
                                UnitGID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.AREA.Id)
                            {
                                AreaID = XMLAtrr.Value;
                            }
                            else if (XMLAtrr.Name == SDMXConstants.Concept.SOURCE.Id)
                            {
                                Source = Convert.ToString(XMLAtrr.Value);
                            }
                            else
                            {
                                if (!SubgroupBreakup.ContainsKey(XMLAtrr.Name))
                                {
                                    if (XMLAtrr.Value != "NA")
                                    {
                                        SubgroupBreakup.Add(XMLAtrr.Name, XMLAtrr.Value);
                                    }
                                }
                            }
                        }

                        SubgroupValGID = this.GetSubgroupValGId(SubgroupBreakup, SubgroupBreakupTable);
                        if (!string.IsNullOrEmpty(SubgroupValGID))
                        {

                            foreach (SDMXObjectModel.Data.StructureSpecific.ObsType ObsObj in SeriesObj.Obs)
                            {
                                string FootnoteValue = string.Empty;
                                string Denominator = string.Empty;


                                foreach (XmlAttribute XMLAtrr in ObsObj.AnyAttr)
                                {
                                    if (XMLAtrr.Name == SDMXConstants.Concept.FOOTNOTES.Id)
                                    {
                                        FootnoteValue = Convert.ToString(XMLAtrr.Value);
                                    }
                                    else if (XMLAtrr.Name == SDMXConstants.Concept.DENOMINATOR.Id)
                                    {
                                        Denominator = Convert.ToString(XMLAtrr.Value);
                                    }
                                    else if (XMLAtrr.Name == SDMXConstants.Concept.SOURCE.Id)
                                    {
                                        Source = Convert.ToString(XMLAtrr.Value);
                                    }
                                }
                                string Timeperiod = ObsObj.TIME_PERIOD;
                                string DataValue = ObsObj.OBS_VALUE;

                                int IndicatorNId = 0;
                                int UnitNId = 0;
                                int SGValNId = 0;
                                int AreaNId = 0;
                                int TimeperiodNId = 0;
                                int SourceNid = 0;
                                int IUSNID = 0;

                                //-- Import Indicator
                                if (IndicatorList.ContainsKey(IndicatorGID))
                                {
                                    IndicatorNId = IndicatorList[IndicatorGID];
                                }
                                else
                                {
                                    IndicatorNId = IndBuilder.GetIndicatorNid(IndicatorGID, string.Empty);
                                    if (IndicatorNId > 0)
                                    {
                                        IndicatorList.Add(IndicatorGID, IndicatorNId);
                                    }
                                }

                                //-- Import Unit
                                if (UnitList.ContainsKey(UnitGID))
                                {
                                    UnitNId = UnitList[UnitGID];
                                }
                                else
                                {
                                    UnitNId = UBuilder.GetUnitNid(UnitGID, "");
                                    if (UnitNId > 0)
                                    {
                                        UnitList.Add(UnitGID, UnitNId);
                                    }
                                }

                                //-- Import SubgroupVal
                                if (!string.IsNullOrEmpty(SubgroupValGID))
                                {
                                    if (SgValList.ContainsKey(SubgroupValGID))
                                    {
                                        SGValNId = SgValList[SubgroupValGID];
                                    }
                                    else
                                    {
                                        SGValNId = SGBuilder.GetSubgroupValNid(SubgroupValGID, "");
                                        if (SGValNId > 0)
                                        {
                                            SgValList.Add(SubgroupValGID, SGValNId);
                                        }
                                    }
                                }

                                //-- Import Area
                                if (AreaList.ContainsKey(AreaID))
                                {
                                    AreaNId = AreaList[AreaID];
                                }
                                else
                                {
                                    AreaNId = AreaBuilderObj.GetAreaNidByAreaID(AreaID);
                                    AreaList.Add(AreaID, AreaNId);
                                }

                                //-- Import Timeperiod
                                if (TimepeirodList.ContainsKey(Timeperiod))
                                {
                                    TimeperiodNId = TimepeirodList[Timeperiod];
                                }
                                else
                                {
                                    TimeperiodNId = TBuilder.CheckNCreateTimeperiod(Timeperiod);
                                    TimepeirodList.Add(Timeperiod, TimeperiodNId);
                                }

                                //-- Import Source
                                if (SourceList.ContainsKey(Source))
                                {
                                    SourceNid = SourceList[Source];
                                }
                                else
                                {
                                    SourceNid = SrcBuilder.CheckNCreateSource(Source);
                                    SourceList.Add(Source, SourceNid);
                                }

                                IUSNID = IUSBuilderObj.ImportIUS(IndicatorNId, UnitNId, SGValNId, 0, 0, DBQueries, DBConnection);

                                if (IUSNID > 0 && AreaNId > 0 && TimeperiodNId > 0 && !string.IsNullOrEmpty(DataValue))
                                {
                                    DatabaseObj.CheckNCreateData(AreaNId, IUSNID, SourceNid, TimeperiodNId, DataValue);
                                }
                            }
                        }
                    }
                }
                DatabaseObj.UpdateIndicatorUnitSubgroupNIDsInData();
                //-- Raise Progress event
                SDMXHelper.Raise_Notify_Progress_Event(ProgressCount++);

                RetVal = true;
            }
            catch (Exception ex)
            {
                //ExceptionFacade.throwException(ex); 
            }
            return RetVal;
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>

        public bool GenerateXMLFromSDMXWebservice(string registryServiceUrl, string TempXMLFileNameWPath)
        {
            bool RetVal = false;

            XmlDocument Document = new XmlDocument();
            XmlDeclaration Declaration = Document.CreateXmlDeclaration("1.0", "utf-8", null);
            Document.AppendChild(Declaration);
            XmlElement DocumentElement = Document.CreateElement("Root");
            Document.AppendChild(DocumentElement);
            XmlElement Element = Document.DocumentElement;

            try
            {

                SDMXRegistry.RegistryService SDMXService = new SDMXRegistry.RegistryService();

                //Running URl is http://118.102.190.94/di7/SDMXRegistry/libraries/ws/RegistryService.asmx;
                //New URL http://www.devinfo.org/libraries/ws/Registryservice.asmx
                SDMXService.Url = registryServiceUrl;

                SDMXService.GetStructures(ref Element);

                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(Element.OuterXml);
                XmlDoc.Save(TempXMLFileNameWPath);

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
            }


            return RetVal;

        }


        public bool GenerateMetadataXMLFromSDMXWebservice(string registryServiceUrl, string TempXMLFileNameWPath)
        {
            bool RetVal = false;

            XmlDocument Document = new XmlDocument();
            XmlDeclaration Declaration = Document.CreateXmlDeclaration("1.0", "utf-8", null);
            Document.AppendChild(Declaration);
            XmlElement DocumentElement = Document.CreateElement("Root");
            Document.AppendChild(DocumentElement);
            XmlElement Element = Document.DocumentElement;
            Dictionary<string, string> DicUserSeletion = new Dictionary<string, string>();
            DataTable Table = null;
            string IndicatorGIDs = string.Empty;
            DI7MetaDataBuilder MDBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);

            try
            {

                //New URL http://www.devinfo.org/libraries/ws/Registryservice.asmx
                SDMXRegistry.RegistryService SDMXService = new DevInfo.Lib.DI_LibBAL.SDMXRegistry.RegistryService();// SDMXWebServices.RegistryService();

                ////Running URl is http://118.102.190.94/di7/SDMXRegistry/libraries/ws/RegistryService.asmx;
                //SDMXService.Url = registryServiceUrl.Replace("RegistryService.asmx", "SDMXService.asmx");

                SDMXService.Timeout = int.MaxValue;

                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.None, "", FieldSelection.Light));

                foreach (DataRow Row in Table.Rows)
                {
                    XmlDocument MDXmlDoc = SDMXUtility.Get_MetadataQuery(SDMXSchemaType.Two_One, MetadataTypes.Indicator, Convert.ToString(Row[Indicator.IndicatorGId]), "MDAGENCYID");

                    XmlElement Element1 = MDXmlDoc.DocumentElement;
                    SDMXService.GetGenericMetadata(ref Element1);

                    MDXmlDoc.LoadXml(Element1.OuterXml);
                    //MDXmlDoc.Save(TempXMLFileNameWPath);
                    MDBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                    MDBuilder.ImportMetadataFromXML(Element1.OuterXml, MetadataElementType.Indicator, Convert.ToInt32(Row[Indicator.IndicatorNId]), TempXMLFileNameWPath);

                }

                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadata(FilterFieldType.None, "", FieldSelection.Light));

                foreach (DataRow Row in Table.Rows)
                {
                    XmlDocument MDXmlDoc = SDMXUtility.Get_MetadataQuery(SDMXSchemaType.Two_One, MetadataTypes.Layer, Convert.ToString(Row[Area_Map_Metadata.LayerNId]), "MDAGENCYAREAID");

                    XmlElement Element1 = MDXmlDoc.DocumentElement;
                    SDMXService.GetGenericMetadata(ref Element1);

                    MDXmlDoc.LoadXml(Element1.OuterXml);
                    //MDXmlDoc.Save(TempXMLFileNameWPath);
                    MDBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                    MDBuilder.ImportMetadataFromXML(Element1.OuterXml, MetadataElementType.Area, Convert.ToInt32(Row[Area_Map_Metadata.LayerNId]), TempXMLFileNameWPath);

                }


                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Source.GetSource(FilterFieldType.None, "", FieldSelection.Light, false));

                foreach (DataRow Row in Table.Rows)
                {
                    XmlDocument MDXmlDoc = SDMXUtility.Get_MetadataQuery(SDMXSchemaType.Two_One, MetadataTypes.Source, Convert.ToString(Row[IndicatorClassifications.ICGId]), "MDAGENCYID");

                    XmlElement Element1 = MDXmlDoc.DocumentElement;
                    SDMXService.GetGenericMetadata(ref Element1);

                    MDXmlDoc.LoadXml(Element1.OuterXml);
                    //MDXmlDoc.Save(TempXMLFileNameWPath);
                    MDBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);
                    MDBuilder.ImportMetadataFromXML(Element1.OuterXml, MetadataElementType.Source, Convert.ToInt32(Row[IndicatorClassifications.ICNId]), TempXMLFileNameWPath);

                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
            }


            return RetVal;

        }
    }
}
