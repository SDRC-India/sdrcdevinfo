using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Data.Generic;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class CodelistUtility : ArtefactUtility
    {
        #region "Properties"

        #region "Private"

        private bool _completeOrSummaryFlag;

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        private CodelistTypes _codelistType;

        #endregion "Private"

        #region "Public"

        internal bool CompleteOrSummaryFlag
        {
            get
            {
                return this._completeOrSummaryFlag;
            }
            set
            {
                this._completeOrSummaryFlag = value;
            }
        }

        internal bool MultiLanguageHandlingRequired
        {
            get
            {
                return this._multiLanguageHandlingRequired;
            }
            set
            {
                this._multiLanguageHandlingRequired = value;
            }
        }

        internal DIConnection DIConnection
        {
            get
            {
                return this._diConnection;
            }
            set
            {
                this._diConnection = value;
            }
        }

        internal DIQueries DIQueries
        {
            get
            {
                return this._diQueries;
            }
            set
            {
                this._diQueries = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal CodelistUtility(CodelistTypes codelistType, bool completeOrSummaryFlag, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
            this._codelistType = codelistType;
            this._completeOrSummaryFlag = completeOrSummaryFlag;
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;

            if (string.IsNullOrEmpty(language))
            {
                this.Language = this._diQueries.LanguageCode.Substring(1);
                this._multiLanguageHandlingRequired = true;
            }
            else
            {
                if (this._diConnection.IsValidDILanguage(this._diQueries.DataPrefix, language))
                {
                    this.Language = language;
                    this._multiLanguageHandlingRequired = false;
                }
                else
                {
                    this.Language = this._diQueries.LanguageCode.Substring(1);
                    this._multiLanguageHandlingRequired = false;
                }
            }
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private ArtefactInfo Generate_Area_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            DataTable DtArea;
            string ParentID;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtArea = null;
            ParentID = string.Empty;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.Area.Id, this.AgencyId, Constants.CodeList.Area.Version,
                           Constants.CodeList.Area.Name, Constants.CodeList.Area.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.Area,FilterFieldType.None, string.Empty, this.Language);
                    DtArea = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrArea in DtArea.Rows)
                    {
                        ParentID = this.Get_ParentID(CodelistTypes.Area, DrArea[Area.AreaParentNId].ToString(), Area.AreaID, this.Language);

                        Code = this.Create_Code(DrArea[Area.AreaID].ToString(), DrArea[Area.AreaName].ToString(), string.Empty, this.Language,
                               ParentID, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.Area, FilterFieldType.ID, DrArea[Area.AreaID].ToString(), Area.AreaName);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrArea[Area.AreaGlobal].ToString());
                        this.Add_Annotation(Code, Area_Level.AreaLevel, DrArea[Area_Level.AreaLevel].ToString());
                        this.Add_Code(Codelist, Code);
                    }
                }
                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.Area));
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

        //This method will return codelist of area according to the passed area levels + the levels below given level.
        private ArtefactInfo Generate_Area_Codelist(string AreaLevel)
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            DataTable DtArea;
            DataTable DTAreaAccordingToLevel;
            string ParentID;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtArea = null;
            ParentID = string.Empty;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.Area.Id, this.AgencyId, Constants.CodeList.Area.Version,
                           Constants.CodeList.Area.Name, Constants.CodeList.Area.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.Area, FilterFieldType.None, string.Empty, this.Language);
                    DtArea = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrArea in DtArea.Select(Area.AreaLevel + " <= '" + AreaLevel + "'"))
                    {
                        ParentID = this.Get_ParentID(CodelistTypes.Area, DrArea[Area.AreaParentNId].ToString(), Area.AreaID, this.Language);

                        Code = this.Create_Code(DrArea[Area.AreaID].ToString(), DrArea[Area.AreaName].ToString(), string.Empty, this.Language,
                               ParentID, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.Area, FilterFieldType.ID, DrArea[Area.AreaID].ToString(), Area.AreaName);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrArea[Area.AreaGlobal].ToString());
                        this.Add_Annotation(Code, Area_Level.AreaLevel, DrArea[Area_Level.AreaLevel].ToString());
                        this.Add_Code(Codelist, Code);
                    }
                }
                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.Area));
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

        private ArtefactInfo Generate_Indicator_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            DataTable DtIndicator;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtIndicator = null;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.Indicator.Id, this.AgencyId, Constants.CodeList.Indicator.Version, Constants.CodeList.Indicator.Name, Constants.CodeList.Indicator.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.Indicator, FilterFieldType.None, string.Empty, this.Language);
                    DtIndicator = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrIndicator in DtIndicator.Rows)
                    {
                        Code = this.Create_Code(DrIndicator[Indicator.IndicatorGId].ToString(),
                               DrIndicator[Indicator.IndicatorName].ToString(), string.Empty, this.Language, string.Empty, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.Indicator, FilterFieldType.GId,
                                 DrIndicator[Indicator.IndicatorGId].ToString(), Indicator.IndicatorName);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrIndicator[Indicator.IndicatorGlobal].ToString());
                        if (DrIndicator[Indicator.HighIsGood] != null)
                        {
                            this.Add_Annotation(Code, Constants.Annotations.HighIsGood, DrIndicator[Indicator.HighIsGood].ToString());
                        }
                        this.Add_Code(Codelist, Code);
                    }
                }

                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.Indicator));
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

        private ArtefactInfo Generate_Unit_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            DataTable DtUnit;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtUnit = null;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.Unit.Id, this.AgencyId, Constants.CodeList.Unit.Version, Constants.CodeList.Unit.Name, Constants.CodeList.Unit.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.Unit, FilterFieldType.None, string.Empty, this.Language);
                    DtUnit = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrUnit in DtUnit.Rows)
                    {
                        Code = this.Create_Code(DrUnit[Unit.UnitGId].ToString(), DrUnit[Unit.UnitName].ToString(), string.Empty, this.Language,
                               string.Empty, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.Unit, FilterFieldType.GId, DrUnit[Unit.UnitGId].ToString(),
                                 Unit.UnitName);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrUnit[Unit.UnitGlobal].ToString());
                        this.Add_Code(Codelist, Code);
                    }
                }

                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.Unit));
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

        private List<ArtefactInfo> Generate_Subgroup_Codelist()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            string CodelistId, CodelistName, CodelistDescription;
            DataTable DtSubgroupType;
            DataTable DtSubgroup;

            RetVal = null;
            Artefact = null;
            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtSubgroupType = null;
            DtSubgroup = null;

            try
            {
                DtSubgroupType = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                foreach (DataRow DrSubgroupType in DtSubgroupType.Rows)
                {
                    CodelistId = Constants.CodelistPrefix + DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString();
                    CodelistName = DrSubgroupType[SubgroupTypes.SubgroupTypeName].ToString();
                    CodelistDescription = Constants.CodeList.Subgroups.Description + DrSubgroupType[SubgroupTypes.SubgroupTypeName].ToString();

                    Codelist = new CodelistType(CodelistId, this.AgencyId, Constants.CodeList.Subgroups.Version, CodelistName, CodelistDescription, Constants.DefaultLanguage, null);

                    if (this._completeOrSummaryFlag == true)
                    {
                        Query = this.Get_Language_Specific_Query(CodelistTypes.Subgroups, FilterFieldType.Type, DrSubgroupType[SubgroupTypes.SubgroupTypeNId].ToString(), this.Language);
                        DtSubgroup = this.DIConnection.ExecuteDataTable(Query);

                        foreach (DataRow DrSubgroup in DtSubgroup.Rows)
                        {
                            Code = this.Create_Code(DrSubgroup[Subgroup.SubgroupGId].ToString(), DrSubgroup[Subgroup.SubgroupName].ToString(), string.Empty, this.Language, string.Empty, null);

                            if (this.MultiLanguageHandlingRequired)
                            {
                                this.Handle_All_Languages(Code, CodelistTypes.Subgroups, FilterFieldType.GId, DrSubgroup[Subgroup.SubgroupGId].ToString(), Subgroup.SubgroupName);
                            }

                            this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrSubgroup[Subgroup.SubgroupGlobal].ToString());
                            this.Add_Code(Codelist, Code);
                        }
                    }

                    Artefact = this.Prepare_ArtefactInfo_From_Codelist(Codelist, DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString() + Constants.XmlExtension);
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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

        private ArtefactInfo Generate_SubgroupType_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            DataTable DtSubgroupType;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            DtSubgroupType = null;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.SubgroupType.Id, this.AgencyId, Constants.CodeList.SubgroupType.Version, Constants.CodeList.SubgroupType.Name, Constants.CodeList.SubgroupType.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.SubgroupType, FilterFieldType.None, string.Empty, this.Language);
                    DtSubgroupType = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrSubgroupType in DtSubgroupType.Rows)
                    {
                        Code = this.Create_Code(DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString(), DrSubgroupType[SubgroupTypes.SubgroupTypeName].ToString(), string.Empty, this.Language, string.Empty, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.SubgroupType, FilterFieldType.GId, DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString(), SubgroupTypes.SubgroupTypeName);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrSubgroupType[SubgroupTypes.SubgroupTypeGlobal].ToString());
                        this.Add_Code(Codelist, Code);
                    }
                }

                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.SubgroupType));
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

        private ArtefactInfo Generate_SubgroupVal_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string Query;
            string BreakupInfo;
            DataTable DtSubgroupVal;
            DataTable DtBreakupInfo;

            Codelist = null;
            Code = null;
            Query = string.Empty;
            BreakupInfo = string.Empty;
            DtSubgroupVal = null;
            DtBreakupInfo = null;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.SubgroupVal.Id, this.AgencyId, Constants.CodeList.SubgroupVal.Version, Constants.CodeList.SubgroupVal.Name, Constants.CodeList.SubgroupVal.Description, Constants.DefaultLanguage, null);

                DtBreakupInfo = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());

                if (this._completeOrSummaryFlag == true)
                {
                    Query = this.Get_Language_Specific_Query(CodelistTypes.SubgroupVal, FilterFieldType.None, string.Empty, this.Language);
                    DtSubgroupVal = this.DIConnection.ExecuteDataTable(Query);

                    foreach (DataRow DrSubgroupVal in DtSubgroupVal.Rows)
                    {
                        Code = this.Create_Code(DrSubgroupVal[SubgroupVals.SubgroupValGId].ToString(), DrSubgroupVal[SubgroupVals.SubgroupVal].ToString(), string.Empty, this.Language, string.Empty, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.SubgroupVal, FilterFieldType.GId, DrSubgroupVal[SubgroupVals.SubgroupValGId].ToString(), SubgroupVals.SubgroupVal);
                        }

                        this.Add_Annotation(Code, Constants.Annotations.IsGlobal, DrSubgroupVal[SubgroupVals.SubgroupValGlobal].ToString());

                        BreakupInfo = this.Get_SubgroupVal_Breakup_Info(DtBreakupInfo, DrSubgroupVal[SubgroupVals.SubgroupValGId].ToString());
                        this.Add_Annotation(Code, Constants.Annotations.Breakup, BreakupInfo);

                        this.Add_Code(Codelist, Code);
                    }
                }

                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.SubgroupVal));
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

        private ArtefactInfo Generate_IUS_Codelist()
        {
            ArtefactInfo RetVal;
            CodelistType Codelist;
            CodeType Code;
            string BreakupInfo;
            DataTable DtIUS;

            Codelist = null;
            Code = null;
            BreakupInfo = string.Empty;
            DtIUS = null;

            try
            {
                Codelist = new CodelistType(Constants.CodeList.IUS.Id, this.AgencyId, Constants.CodeList.IUS.Version, Constants.CodeList.IUS.Name, Constants.CodeList.IUS.Description, Constants.DefaultLanguage, null);

                if (this._completeOrSummaryFlag == true)
                {
                    DtIUS = this.DIConnection.ExecuteDataTable(this.DIQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light,true));

                    foreach (DataRow DrIUS in DtIUS.Rows)
                    {
                        Code = this.Create_Code(DrIUS[Indicator.IndicatorGId].ToString() + Constants.AtTheRate + DrIUS[Unit.UnitGId].ToString() + Constants.AtTheRate + DrIUS[SubgroupVals.SubgroupValGId].ToString(), DrIUS[Indicator.IndicatorName].ToString() + Constants.Dash + DrIUS[Unit.UnitName].ToString() + Constants.Dash + DrIUS[SubgroupVals.SubgroupVal].ToString(), string.Empty, this.Language, string.Empty, null);

                        if (this.MultiLanguageHandlingRequired)
                        {
                            this.Handle_All_Languages(Code, CodelistTypes.IUS, FilterFieldType.GId, DrIUS[Indicator.IndicatorGId].ToString() + Constants.AtTheRate + DrIUS[Unit.UnitGId].ToString() + Constants.AtTheRate + DrIUS[SubgroupVals.SubgroupValGId].ToString(), Indicator.IndicatorName + Constants.AtTheRate + Unit.UnitName + Constants.AtTheRate + SubgroupVals.SubgroupVal);
                        }
                        if (DrIUS[Indicator_Unit_Subgroup.IsDefaultSubgroup] != null)
                        {
                            this.Add_Annotation(Code, Constants.Annotations.IsDefault, DrIUS[Indicator_Unit_Subgroup.IsDefaultSubgroup].ToString());
                        }
                        this.Add_Code(Codelist, Code);
                    }
                }

                RetVal = this.Prepare_ArtefactInfo_From_Codelist(Codelist, this.Get_File_Name(CodelistTypes.IUS));
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

        private string Get_SubgroupVal_Breakup_Info(DataTable DtBreakupInfo, string SubgroupValGId)
        {
            string RetVal;
            string SubgroupTypeGId, SubgroupGId;

            RetVal = string.Empty;

            try
            {
                
                DtBreakupInfo.DefaultView.RowFilter = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe;
                DtBreakupInfo = DtBreakupInfo.DefaultView.ToTable();

                foreach (DataRow DrBreakupInfo in DtBreakupInfo.Rows)
                {
                    SubgroupTypeGId = DrBreakupInfo[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString();
                    SubgroupGId = DrBreakupInfo[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString();
                    RetVal += Constants.CodelistPrefix + SubgroupTypeGId + Constants.EqualsTo + SubgroupGId + Constants.Comma;
                }

                if (RetVal.Length > 0)
                {
                    RetVal = RetVal.Substring(0, RetVal.Length - 1);
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

        private CodeType Create_Code(string id, string name, string description, string language, string parentId, AnnotationType annotation)
        {
            CodeType RetVal;

            RetVal = new CodeType(id, name, description, language, parentId, annotation);

            return RetVal;
        }

        private void Add_Code(CodelistType Codelist, CodeType Code)
        {
            if (Codelist != null && Code != null)
            {
                Codelist.Items.Add(Code);
            }
        }

        private void Add_Annotation(CodeType Code, string title, string text)
        {
            if (title != string.Empty || text != string.Empty)
            {
                if (Code.Annotations == null)
                {
                    Code.Annotations = new List<AnnotationType>();
                }

                AnnotationType Annotation = new AnnotationType();
                Annotation.AnnotationTitle = title;
                Annotation.AnnotationText = new List<TextType>();
                Annotation.AnnotationText.Add(new TextType(null, text));
                Code.Annotations.Add(Annotation);
            }
        }

        private void Add_Language_Specific_Details(CodeType Code, CodelistTypes codelistType, DataTable DtTable, string nameColumn, string language)
        {
            string[] IUSSplits;

            if (DtTable.Rows.Count > 0)
            {
                if (codelistType != CodelistTypes.IUS)
                {
                    Code.Name.Add(new TextType(language, DtTable.Rows[0][nameColumn].ToString()));
                }
                else
                {
                    IUSSplits = this.SplitString(nameColumn, Constants.AtTheRate);
                    Code.Name.Add(new TextType(language, DtTable.Rows[0][IUSSplits[0]].ToString() + Constants.Dash + DtTable.Rows[0][IUSSplits[1]].ToString() + Constants.Dash + DtTable.Rows[0][IUSSplits[2]].ToString()));
                }
            }
        }

        private void Handle_All_Languages(CodeType Code, CodelistTypes codelistType, FilterFieldType filterField, string filterText, string nameColumn)
        {
            DataTable DtTable;
            string Query, Language;

            Query = string.Empty;

            if (this.MultiLanguageHandlingRequired)
            {
                foreach (DataRow LanguageRow in this.DIConnection.DILanguages(this.DIQueries.DataPrefix).Rows)
                {
                    Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                    if (Language != this.DIQueries.LanguageCode.Substring(1))
                    {
                        Query = this.Get_Language_Specific_Query(codelistType, filterField, filterText, Language);
                        DtTable = this.DIConnection.ExecuteDataTable(Query);
                        Add_Language_Specific_Details(Code, codelistType, DtTable, nameColumn, Language);
                    }
                }
            }
        }

        private string Get_Language_Specific_Query(CodelistTypes codelistType, FilterFieldType filterField, string filterText, string language)
        {
            string RetVal;
            string[] IUSSplits;
            DIQueries DIQueriesLanguage;

            RetVal = string.Empty;
            if ((filterField == FilterFieldType.GId || filterField == FilterFieldType.ID) && codelistType != CodelistTypes.IUS)
            {
                filterText = Constants.Apostophe + filterText + Constants.Apostophe;
            }
            // 1. Creating language specific DIQueries object.
            DIQueriesLanguage = new DIQueries(this.DIQueries.DataPrefix, language);

            switch (codelistType)
            {
                case CodelistTypes.Area:
                    RetVal = DIQueriesLanguage.Area.GetArea(filterField, filterText);
                    break;
                case CodelistTypes.Indicator:
                    RetVal = DIQueriesLanguage.Indicators.GetIndicator(filterField, filterText, FieldSelection.Heavy);
                    break;
                case CodelistTypes.Subgroups:
                    RetVal = DIQueriesLanguage.Subgroup.GetSubgroup(filterField, filterText);
                    break;
                case CodelistTypes.SubgroupType:
                    RetVal = DIQueriesLanguage.SubgroupTypes.GetSubgroupTypes(filterField, filterText);
                    break;
                case CodelistTypes.SubgroupVal:
                    RetVal = DIQueriesLanguage.SubgroupVals.GetSubgroupVals(filterField, filterText);
                    break;
                case CodelistTypes.Unit:
                    RetVal = DIQueriesLanguage.Unit.GetUnit(filterField, filterText);
                    break;
                case CodelistTypes.IUS:
                    IUSSplits = this.SplitString(filterText, Constants.AtTheRate);
                    RetVal = DIQueriesLanguage.IUS.GetIUSNIdsByGID(IUSSplits[0], IUSSplits[1], IUSSplits[2]);
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        private string Get_ParentID(CodelistTypes codelistType, string parentNid, string idColumn, string language)
        {
            string RetVal;
            string Query;
            DataTable DtParent;

            try
            {
                Query = this.Get_Language_Specific_Query(codelistType, FilterFieldType.NId, parentNid, language);
                DtParent = this.DIConnection.ExecuteDataTable(Query);
                if (DtParent.Rows.Count > 0)
                {
                    RetVal = DtParent.Rows[0][idColumn].ToString();
                }
                else
                {
                    RetVal = Constants.MinusOne;
                }
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                throw ex;
            }

            return RetVal;
        }

        private ArtefactInfo Prepare_ArtefactInfo_From_Codelist(CodelistType Codelist, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(Codelist);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(Codelist.id, Codelist.agencyID, Codelist.version, string.Empty, ArtefactTypes.CL, FileName, XmlContent);
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

        private string Get_File_Name(CodelistTypes codelistType)
        {
            string RetVal;

            RetVal = string.Empty;

            switch (codelistType)
            {
                case CodelistTypes.Area:
                    RetVal = Constants.CodeList.Area.FileName;
                    break;
                case CodelistTypes.Indicator:
                    RetVal = Constants.CodeList.Indicator.FileName;
                    break;
                case CodelistTypes.Unit:
                    RetVal = Constants.CodeList.Unit.FileName;
                    break;
                case CodelistTypes.SubgroupType:
                    RetVal = Constants.CodeList.SubgroupType.FileName;
                    break;
                case CodelistTypes.SubgroupVal:
                    RetVal = Constants.CodeList.SubgroupVal.FileName;
                    break;
                case CodelistTypes.IUS:
                    RetVal = Constants.CodeList.IUS.FileName;
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(CodelistType Codelist)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, Codelist, null, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "Private"

        #region "Public"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;

            RetVal = null;

            try
            {
                if ((this._codelistType & CodelistTypes.ALL) == CodelistTypes.ALL)
                {
                    Artefact = this.Generate_Area_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Indicator_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_Unit_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    RetVal.AddRange(this.Generate_Subgroup_Codelist());

                    Artefact = this.Generate_SubgroupType_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_SubgroupVal_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_IUS_Codelist();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else
                {
                    if ((this._codelistType & CodelistTypes.Area) == CodelistTypes.Area)
                    {
                        Artefact = this.Generate_Area_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._codelistType & CodelistTypes.Indicator) == CodelistTypes.Indicator)
                    {
                        Artefact = this.Generate_Indicator_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._codelistType & CodelistTypes.Unit) == CodelistTypes.Unit)
                    {
                        Artefact = this.Generate_Unit_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._codelistType & CodelistTypes.Subgroups) == CodelistTypes.Subgroups)
                    {
                        RetVal = new List<ArtefactInfo>();
                        RetVal.AddRange(this.Generate_Subgroup_Codelist());
                    }
                    if ((this._codelistType & CodelistTypes.SubgroupType) == CodelistTypes.SubgroupType)
                    {
                        Artefact = this.Generate_SubgroupType_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._codelistType & CodelistTypes.SubgroupVal) == CodelistTypes.SubgroupVal)
                    {
                        Artefact = this.Generate_SubgroupVal_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._codelistType & CodelistTypes.IUS) == CodelistTypes.IUS)
                    {
                        Artefact = this.Generate_IUS_Codelist();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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

        #endregion "Public"

        #endregion "Methods"
    }
}
