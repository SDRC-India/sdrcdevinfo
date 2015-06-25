using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Common;
using SDMXConstants = DevInfo.Lib.DI_LibSDMX.Constants;
using SDMXObjectModel.Message;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class DSDHelper
    {

        #region "-- Propertes --"

        private SDMXObjectModel.Message.StructureType _DSDStructure = null;
        /// <summary>
        /// 
        /// </summary>
        public SDMXObjectModel.Message.StructureType DSDStructure
        {
            get { return this._DSDStructure; }
            set { this._DSDStructure = value; }
        }

        private string _LanguageCode = "en";

        /// <summary>
        /// Set or get Language code
        /// </summary>
        public string LanguageCode
        {
            get { return this._LanguageCode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._LanguageCode = value.Trim('_');
                }
            }
        }

        //private DIConnection DBConnection = null;
        //private DIQueries DBQueries = null;

        internal Dictionary<string, CommonInfo> IndicatorDetails = new Dictionary<string, CommonInfo>();
        internal Dictionary<string, CommonInfo> UnitDetails = new Dictionary<string, CommonInfo>();
        internal Dictionary<string, SubgroupValInfo> SubgroupValDetails = new Dictionary<string, SubgroupValInfo>();
        internal Dictionary<string, CommonInfo> SubgroupTypeDetails = new Dictionary<string, CommonInfo>();
        internal Dictionary<string, SubgroupInfo> SubgroupDetails = new Dictionary<string, SubgroupInfo>();
        internal Dictionary<string, AreaInfo> AreaDetails = new Dictionary<string, AreaInfo>();
        internal Dictionary<string, IndicatorClassificationInfo> ICDetails = new Dictionary<string, IndicatorClassificationInfo>();
        internal Dictionary<string, IUSInfo> IUSDetails = new Dictionary<string, IUSInfo>();
        internal Dictionary<string, CommonInfo> SourceDetails = new Dictionary<string, CommonInfo>();

        /// <summary>
        /// Key IUSGID Value NID:ICNId and Name:ICGID
        /// </summary>
        internal Dictionary<string, IndicatorClassificationInfo> ICIUSDetails = new Dictionary<string, IndicatorClassificationInfo>();
        internal SDMXObjectModel.Message.StructureType DSDStructureObj = null;


        #endregion


        #region "-- New/Dispose --"

        public DSDHelper(string languageCode)
        {
            this._LanguageCode = languageCode;
        }

        #endregion

        #region "Import DSD"

        /// <summary>
        /// Get_Indicator_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_Indicators()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            string IndicatorName = string.Empty;
            string IndicatorGID = string.Empty;
            //string LanguageCode = this._LanguageCode;
            int IndicatorNID = 0;
            bool IndicatorGlobal = false;

            IndicatorDetails = new Dictionary<string, CommonInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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
                                    break;
                                }
                            }
                        }

                        if (!IndicatorDetails.ContainsKey(IndicatorGID))
                        {
                            CommonInfo TableInfoObj = new CommonInfo();
                            TableInfoObj.GID = IndicatorGID;
                            TableInfoObj.Name = IndicatorName;
                            TableInfoObj.Global = IndicatorGlobal;
                            IndicatorDetails.Add(IndicatorGID, TableInfoObj);
                        }
                    }
                }
            }

            // return RetVal;
        }

        /// <summary>
        /// Get_Unit_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_Units()
        {
            //Dictionary<string, int> RetVal = new Dictionary<string, int>();

            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            bool UnitGlobal = false;

            this.UnitDetails = new Dictionary<string, CommonInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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


                        //  SDMXUtility.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);


                        if (!UnitDetails.ContainsKey(GID))
                        {
                            CommonInfo TableInfoObj = new CommonInfo();
                            TableInfoObj.GID = GID;
                            TableInfoObj.Name = Name;
                            TableInfoObj.Global = UnitGlobal;
                            UnitDetails.Add(GID, TableInfoObj);
                        }
                    }
                }
            }



            //return RetVal;
        }

        /// <summary>
        /// Get_SubgroupVal_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_SubgroupVal()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            // DI6SubgroupValBuilder BuilderObj = new DI6SubgroupValBuilder(this.DBConnection, this.DBQueries);
            //  DI6SubgroupBuilder SGBuilderObj = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            // string LanguageCode = this._LanguageCode;
            int NID = 0;
            bool ISGlobal = false;
            string SubgroupDimensions = string.Empty;

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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

                        //  SDMXUtility.Raise_Set_Process_Name_Event(IndicatorName, IndicatorGID, 0);

                        // NID = BuilderObj.ImportSubgroupVal(Name, GID, ISGlobal);
                        SubgroupValInfo TableInfoObj = new SubgroupValInfo();
                        TableInfoObj.Name = Name;
                        TableInfoObj.GID = GID;
                        TableInfoObj.Global = ISGlobal;

                        foreach (string SgText in SubgroupDimensions.Split(','))
                        {
                            string SGType = SgText.Split('=')[0].Trim();
                            string SGGID = SgText.Split('=')[1].Trim();

                            if (SGType.Length > 3)
                            {
                                SGType = SGType.Substring(3);
                            }
                            if (SGType == "LOCATION")
                            {
                                TableInfoObj.Location.GID = SGGID;
                            }
                            else if (SGType == "AGE")
                            {
                                TableInfoObj.Age.GID = SGGID;
                            }
                            else if (SGType == "SEX")
                            {
                                TableInfoObj.Sex.GID = SGGID;
                            }
                            else if (SGType == "OTHERS")
                            {
                                TableInfoObj.Others.GID = SGGID;
                            }

                        }

                        if (!string.IsNullOrEmpty(GID))
                        {
                            if (!SubgroupValDetails.ContainsKey(GID))
                            {
                                SubgroupValDetails.Add(GID, TableInfoObj);
                            }
                        }
                    }
                }
            }

            // return RetVal;
        }

        /// <summary>
        /// Get_Subgroup_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_Subgroup()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            //DI6SubgroupBuilder BuilderObj = new DI6SubgroupBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string SubgroupTypeGID = string.Empty;
            string SubgroupGID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            bool ISGlobal = false;
            string SubgroupDimID = string.Empty;
            this.SubgroupDetails = new Dictionary<string, SubgroupInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.SubgroupType.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        SubgroupTypeGID = IndCode.id;
                        SubgroupDimID = "CL_" + SubgroupTypeGID;

                        foreach (SDMXObjectModel.Structure.CodelistType SCodeListObj in this._DSDStructure.Structures.Codelists)
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
                                        if (IndTextType.lang.Trim('_') == this._LanguageCode)
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

                                    if (!this.SubgroupDetails.ContainsKey(SubgroupGID))
                                    {
                                        SubgroupInfo TableInfoObj = new SubgroupInfo();
                                        TableInfoObj.GID = SubgroupGID;
                                        TableInfoObj.Name = Name;
                                        TableInfoObj.Type = GetSubgroupTypeByName(SubgroupTypeGID);
                                        TableInfoObj.Global = ISGlobal;
                                        this.SubgroupDetails.Add(SubgroupGID, TableInfoObj);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private SubgroupType GetSubgroupTypeByName(string SubgroupTypeGID)
        {
            SubgroupType RetVal = SubgroupType.Others;

            if (SubgroupTypeGID == SubgroupType.Age.ToString().ToUpper())
            {
                RetVal = SubgroupType.Age;
            }
            else if (SubgroupTypeGID.ToString().ToUpper() == SubgroupType.Sex.ToString().ToUpper())
            {
                RetVal = SubgroupType.Sex;
            }
            else if (SubgroupTypeGID.ToString().ToUpper() == SubgroupType.Location.ToString().ToUpper())
            {
                RetVal = SubgroupType.Location;
            }
            else if (SubgroupTypeGID == SubgroupType.Others.ToString().ToUpper())
            {
                RetVal = SubgroupType.Others;
            }

            return RetVal;
        }

        /// <summary>
        /// Get_SubgroupType_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_SubgroupType()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            //  DI6SubgroupTypeBuilder BuilderObj = new DI6SubgroupTypeBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            bool ISGlobal = false;
            this.SubgroupTypeDetails = new Dictionary<string, CommonInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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

                        if (!this.SubgroupTypeDetails.ContainsKey(GID))
                        {
                            CommonInfo TableInfoObj = new CommonInfo();
                            TableInfoObj.GID = GID;
                            TableInfoObj.Name = Name;
                            TableInfoObj.Global = ISGlobal;
                            this.SubgroupTypeDetails.Add(GID, TableInfoObj);
                        }
                    }
                }
            }

            //  return RetVal;
        }

        /// <summary>
        /// Get_Area_Collection method..
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_Area()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            //AreaBuilder BuilderObj = new AreaBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string AreaID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            string ParentID = string.Empty;
            bool ISGlobal = false;
            this.AreaDetails = new Dictionary<string, AreaInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this._DSDStructure.Structures.Codelists)
            {
                if (CodeListObj.id == SDMXConstants.CodeList.Area.Id)
                {
                    foreach (SDMXObjectModel.Structure.CodeType IndCode in CodeListObj.Items)
                    {
                        AreaID = IndCode.id;
                        Name = string.Empty;
                        ISGlobal = false;
                        foreach (TextType IndTextType in IndCode.Name)
                        {
                            if (IndTextType.lang.Trim('_') == this._LanguageCode.Trim('_'))
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

                        if (!this.AreaDetails.ContainsKey(AreaID))
                        {
                            AreaInfo TableInfoObj = new AreaInfo();
                            TableInfoObj.ID = AreaID;
                            TableInfoObj.Name = Name;
                            TableInfoObj.IsGlobal = ISGlobal;
                            TableInfoObj.Parent = new AreaInfo();
                            TableInfoObj.Parent.ID = ParentID;
                            this.AreaDetails.Add(AreaID, TableInfoObj);
                        }

                    }


                }
            }
        }


        private void Get_IUS()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            string IUSGID = string.Empty;
            string IndicatorGId = string.Empty;
            string UnitGId = string.Empty;
            string SGVGId = string.Empty;
            int IndicatorNId = 0;
            int UnitNId = 0;
            int SubgroupValNId = 0;

            //-- Initialize Process
            //  SDMXUtility.Raise_Initilize_Process_Event("Generating Complete DSD...", 1, 1);
            this.IUSDetails = new Dictionary<string, IUSInfo>();

            foreach (SDMXObjectModel.Structure.CodelistType CodeListObj in this.DSDStructure.Structures.Codelists)
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

                        IUSInfo TableInfoObj = new IUSInfo();
                        TableInfoObj.IndicatorInfo = new IndicatorInfo();
                        TableInfoObj.UnitInfo = new UnitInfo();
                        TableInfoObj.SubgroupValInfo = new DI6SubgroupValInfo();

                        TableInfoObj.IndicatorInfo.GID = IndicatorGId;
                        TableInfoObj.UnitInfo.GID = UnitGId;
                        TableInfoObj.SubgroupValInfo.GID = SGVGId;

                        if (!this.IUSDetails.ContainsKey(IUSGID))
                        {
                            this.IUSDetails.Add(IUSGID, TableInfoObj);
                        }
                    }
                }
            }

            // return RetVal;
        }

        /// <summary>
        /// Get_IC_Collection method.. 
        /// </summary>
        /// <param name="this._DSDStructure"></param>
        /// <returns></returns>
        private void Get_IC()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            // IndicatorClassificationBuilder BuilderObj = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
            //SourceBuilder SrcBuilderObj = new SourceBuilder(this.DBConnection, this.DBQueries);
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            string ParentID = string.Empty;
            ICType icICType = ICType.Sector;
            //Dictionary<string, string> CParent = new Dictionary<string, string>();

            bool ISGlobal = false;

            this.ICDetails = new Dictionary<string, IndicatorClassificationInfo>();

            foreach (SDMXObjectModel.Structure.CategorySchemeType CodeListObj in this._DSDStructure.Structures.CategorySchemes)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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

                        if (!this.ICDetails.ContainsKey(GID))
                        {
                            IndicatorClassificationInfo TableInfoObj = new IndicatorClassificationInfo();
                            TableInfoObj.GID = GID;
                            TableInfoObj.Name = Name;
                            TableInfoObj.Type = icICType;
                            TableInfoObj.Parent = new IndicatorClassificationInfo();
                            TableInfoObj.Parent.GID = ParentID;
                            this.ICDetails.Add(GID, TableInfoObj);
                        }
                        // Read each child IC for current IC
                        this.Get_ICChain(GID, icICType, CatTypeCode);
                    }
                }
            }

        }

        private void Get_Source()
        {
            //'Dictionary<string, int> RetVal = new Dictionary<string, int>();
            SourceBuilder SrcBuilderObj = null;
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
            int NID = 0;
            string ParentID = string.Empty;
            ICType icICType = ICType.Sector;
            this.SourceDetails = new Dictionary<string, CommonInfo>();
            //SrcBuilderObj = new SourceBuilder(this.DBConnection, this.DBQueries);

            foreach (SDMXObjectModel.Structure.CategorySchemeType CodeListObj in this._DSDStructure.Structures.CategorySchemes)
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
                            if (IndTextType.lang.Trim('_') == this._LanguageCode)
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
                                if (IndTextType.lang.Trim('_') == this._LanguageCode)
                                {
                                    Name = IndTextType.Value;
                                    break;
                                }
                            }
                        }
                        ParentID = "-1";

                        //NID = SrcBuilderObj.CheckNCreateSource(Name);

                        //if (!RetVal.ContainsKey(GID) && NID > 0)
                        //{
                        //    RetVal.Add(GID, NID);
                        //}

                        if (!this.SourceDetails.ContainsKey(Name))
                        {
                            CommonInfo TableInfoObj = new CommonInfo();
                            TableInfoObj.GID = GID;
                            TableInfoObj.Name = Name;
                            this.SourceDetails.Add(Name, TableInfoObj);
                        }
                    }
                }
            }
        }

        private void Get_ICChain(string parentICGID, ICType icICType, SDMXObjectModel.Structure.CategoryType CatTypeCode)
        {
            string Name = string.Empty;
            string GID = string.Empty;
            string LanguageCode = this._LanguageCode;
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
                    if (IndTextType.lang.Trim('_') == this._LanguageCode)
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

                if (!this.ICDetails.ContainsKey(GID))
                {
                    IndicatorClassificationInfo TableInfoObj = new IndicatorClassificationInfo();
                    TableInfoObj.GID = GID;
                    TableInfoObj.Name = Name;
                    TableInfoObj.Type = icICType;
                    TableInfoObj.Parent = new IndicatorClassificationInfo();
                    TableInfoObj.Parent.GID = parentICGID;
                    this.ICDetails.Add(GID, TableInfoObj);
                }

                this.Get_ICChain(GID, icICType, SubCateType);

            }
        }

        /// <summary>
        /// Get_IC_IUS_List method..
        /// </summary>
        /// <param name="DSDStructure"></param>
        /// <returns></returns>
        private void Get_IC_IUS()
        {
            //  Dictionary<string, string> RetVal = new Dictionary<string, string>();
            // IndicatorClassificationBuilder ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);
            string IUSGID = string.Empty;
            string ICGID = string.Empty;
            //int ICNId = 0;
            //int IUSNId = 0;
            this.ICIUSDetails = new Dictionary<string, IndicatorClassificationInfo>();

            foreach (SDMXObjectModel.Structure.CategorisationType CodeListObj in this.DSDStructure.Structures.Categorisations)
            {
                if (CodeListObj.id.StartsWith("CTGZ_"))
                {
                    IUSGID = CodeListObj.id.Substring(5);
                    IUSGID = IUSGID.Substring(IUSGID.IndexOf("@") + 1);
                }

                foreach (SDMXObjectModel.Common.RefBaseType TargetObj in CodeListObj.Target.Items)
                {
                    ICGID = TargetObj.id;
                    break;
                }

                if (!this.ICIUSDetails.ContainsKey(IUSGID))
                {
                    IndicatorClassificationInfo TableInfoObj = new IndicatorClassificationInfo();
                    if (this.ICDetails.ContainsKey(ICGID))
                    {
                        TableInfoObj = this.ICDetails[ICGID];
                    }
                    else
                    {
                        TableInfoObj.GID = ICGID;
                        TableInfoObj.Name = ICGID;
                    }

                    this.ICIUSDetails.Add(IUSGID, TableInfoObj);
                }
                else
                {
                    if (this.ICDetails.ContainsKey(ICGID))
                    {
                        if (this.ICDetails[ICGID].Parent != null)
                        {
                            if (this.ICDetails[ICGID].Parent.GID != "-1" && !string.IsNullOrEmpty(this.ICDetails[ICGID].Parent.GID))
                            {
                                IndicatorClassificationInfo TableInfoObj = new IndicatorClassificationInfo();
                                if (this.ICDetails.ContainsKey(ICGID))
                                {
                                    TableInfoObj = this.ICDetails[ICGID];
                                }
                                else
                                {
                                    TableInfoObj.GID = ICGID;
                                    TableInfoObj.Name = ICGID;
                                }
                                this.ICIUSDetails[IUSGID] = TableInfoObj;
                            }
                        }
                    }
                }
            }

            //  return RetVal;
        }

        #endregion "Read DSD"

        public bool LoadDSD(string DSDFileNameWithPath)
        {
            bool RetVal = false;
            this._DSDStructure = new SDMXObjectModel.Message.StructureType();

            try
            {
                this._DSDStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DSDFileNameWithPath);

                this.Get_Indicators();
                this.Get_Units();
                this.Get_SubgroupType();
                this.Get_Subgroup();
                this.Get_SubgroupVal();

                this.Get_Area();
                this.Get_IUS();

                this.Get_IC();
                this.Get_IC_IUS();
            }
            catch (Exception ex)
            {
                throw ex;
            }



            return RetVal;

        }

    }
}
