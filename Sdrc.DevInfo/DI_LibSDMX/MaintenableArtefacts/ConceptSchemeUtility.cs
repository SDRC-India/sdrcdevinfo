using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml;
using SDMXObjectModel;
using System.Text.RegularExpressions;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class ConceptSchemeUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private ConceptSchemeTypes _conceptSchemeType;

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        #endregion "--Private--"

        #region "--Public--"

        internal ConceptSchemeTypes ConceptSchemeType
        {
            get
            {
                return this._conceptSchemeType;
            }
            set
            {
                this._conceptSchemeType = value;
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

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal ConceptSchemeUtility(ConceptSchemeTypes conceptSchemeType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
            this._conceptSchemeType = conceptSchemeType;
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

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Generate_DSD_ConceptScheme()
        {
            ArtefactInfo RetVal;
            ConceptSchemeType ConceptScheme;
            ConceptType Concept;
            string ConceptId, ConceptName, ConceptDescription;
            DataTable DtSubgroupType;
            DIQueries DIQueriesLanguage;

            RetVal = null;

            try
            {
                ConceptScheme = new ConceptSchemeType(Constants.ConceptScheme.DSD.Id, this.AgencyId, Constants.ConceptScheme.DSD.Version, Constants.ConceptScheme.DSD.Name, Constants.ConceptScheme.DSD.Description, Constants.DefaultLanguage, null);

                Concept = new ConceptType(Constants.Concept.TIME_PERIOD.Id, Constants.Concept.TIME_PERIOD.Name, Constants.Concept.TIME_PERIOD.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.OBS_VALUE.Id, Constants.Concept.OBS_VALUE.Name, Constants.Concept.OBS_VALUE.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.AREA.Id, Constants.Concept.AREA.Name, Constants.Concept.AREA.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.INDICATOR.Id, Constants.Concept.INDICATOR.Name, Constants.Concept.INDICATOR.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.UNIT.Id, Constants.Concept.UNIT.Name, Constants.Concept.UNIT.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.PERIODICITY.Id, Constants.Concept.PERIODICITY.Name, Constants.Concept.PERIODICITY.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.SOURCE.Id, Constants.Concept.SOURCE.Name, Constants.Concept.SOURCE.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.NATURE.Id, Constants.Concept.NATURE.Name, Constants.Concept.NATURE.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.DENOMINATOR.Id, Constants.Concept.DENOMINATOR.Name, Constants.Concept.DENOMINATOR.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.FOOTNOTES.Id, Constants.Concept.FOOTNOTES.Name, Constants.Concept.FOOTNOTES.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Name, Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                Concept = new ConceptType(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Name, Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Description, Constants.DefaultLanguage, string.Empty, null);
                ConceptScheme.Items.Add(Concept);

                DIQueriesLanguage = new DIQueries(this.DIQueries.DataPrefix, this.Language);
                DtSubgroupType = this.DIConnection.ExecuteDataTable(DIQueriesLanguage.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                foreach (DataRow DrSubgroupType in DtSubgroupType.Rows)
                {
                    ConceptId = DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString();
                    ConceptName = DrSubgroupType[SubgroupTypes.SubgroupTypeName].ToString();

                    switch (ConceptId)
                    {
                        case Constants.Concept.SUBGROUP.AGE.Id:
                            ConceptDescription = Constants.Concept.SUBGROUP.AGE.Description;
                            break;
                        case Constants.Concept.SUBGROUP.SEX.Id:
                            ConceptDescription = Constants.Concept.SUBGROUP.SEX.Description;
                            break;
                        case Constants.Concept.SUBGROUP.LOCATION.Id:
                            ConceptDescription = Constants.Concept.SUBGROUP.LOCATION.Description;
                            break;
                        default:
                            ConceptDescription = Constants.Concept.SUBGROUP.Description;
                            break;
                    }

                    Concept = new ConceptType(ConceptId, ConceptName, ConceptDescription, this.Language, string.Empty, null);
                    ConceptScheme.Items.Add(Concept);
                }

                RetVal = this.Prepare_ArtefactInfo_From_ConceptScheme(ConceptScheme, Constants.ConceptScheme.DSD.FileName);
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

        private ArtefactInfo Generate_MSD_Area_ConceptScheme()
        {
            ArtefactInfo RetVal;
            ConceptSchemeType ConceptScheme;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            ConceptScheme = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                ConceptScheme = new ConceptSchemeType(Constants.ConceptScheme.MSD_Area.Id, this.AgencyId, Constants.ConceptScheme.MSD_Area.Version, Constants.ConceptScheme.MSD_Area.Name, Constants.ConceptScheme.MSD_Area.Description, Constants.DefaultLanguage, null);

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'A'";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ConceptScheme(ConceptScheme, DtMetadataCategory);

                RetVal = this.Prepare_ArtefactInfo_From_ConceptScheme(ConceptScheme, Constants.ConceptScheme.MSD_Area.FileName);
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

        private ArtefactInfo Generate_MSD_Indicator_ConceptScheme()
        {
            ArtefactInfo RetVal;
            ConceptSchemeType ConceptScheme;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            ConceptScheme = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                ConceptScheme = new ConceptSchemeType(Constants.ConceptScheme.MSD_Indicator.Id, this.AgencyId, Constants.ConceptScheme.MSD_Indicator.Version, Constants.ConceptScheme.MSD_Indicator.Name, Constants.ConceptScheme.MSD_Indicator.Description, Constants.DefaultLanguage, null);

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'I'";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ConceptScheme(ConceptScheme, DtMetadataCategory);

                RetVal = this.Prepare_ArtefactInfo_From_ConceptScheme(ConceptScheme, Constants.ConceptScheme.MSD_Indicator.FileName);
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

        private ArtefactInfo Generate_MSD_Source_ConceptScheme()
        {
            ArtefactInfo RetVal;
            ConceptSchemeType ConceptScheme;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            ConceptScheme = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                ConceptScheme = new ConceptSchemeType(Constants.ConceptScheme.MSD_Source.Id, this.AgencyId, Constants.ConceptScheme.MSD_Source.Version, Constants.ConceptScheme.MSD_Source.Name, Constants.ConceptScheme.MSD_Source.Description, Constants.DefaultLanguage,null);

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'S'";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ConceptScheme(ConceptScheme, DtMetadataCategory);

                RetVal = this.Prepare_ArtefactInfo_From_ConceptScheme(ConceptScheme, Constants.ConceptScheme.MSD_Source.FileName);
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

        private void Fill_ConceptScheme(ConceptSchemeType ConceptScheme, DataTable DtMetadataCategory)
        {
            ConceptType Concept;
            string CategoryGId, CategoryName, CategoryDescription, ParentCategoryNId, ParentCategoryGId;

            Concept = null;
            CategoryGId = string.Empty;
            CategoryName = string.Empty;
            CategoryDescription = string.Empty;
            ParentCategoryNId = string.Empty;
            ParentCategoryGId = string.Empty;

            foreach (DataRow DrMetadataCategory in DtMetadataCategory.Rows)
            {
                CategoryGId = DrMetadataCategory["CategoryGId"].ToString();
                CategoryName = DrMetadataCategory["CategoryName"].ToString();
                CategoryDescription = DrMetadataCategory["CategoryDescription"].ToString();
                ParentCategoryNId = DrMetadataCategory["ParentCategoryNId"].ToString();
                ParentCategoryGId = this.Get_ParentCategoryGId(ParentCategoryNId, DtMetadataCategory);

                Concept = new ConceptType(CategoryGId, CategoryName, CategoryDescription, this.Language, ParentCategoryGId, null);
                this.Handle_All_Languages(Concept, CategoryGId);
                ConceptScheme.Items.Add(Concept);
            }
        }

        private void Handle_All_Languages(ConceptType Concept, string CategoryGId)
        {
            DataTable DtTable;
            string Query, Language;

            Query = string.Empty;

            if (this._multiLanguageHandlingRequired)
            {
                foreach (DataRow LanguageRow in this.DIConnection.DILanguages(this.DIQueries.DataPrefix).Rows)
                {
                    Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                    if (Language != this.Language)
                    {
                        if (!string.IsNullOrEmpty(CategoryGId))
                        {
                            Query = "SELECT * FROM UT_Metadata_Category_" + Language + " WHERE CategoryGId = '" + CategoryGId + "'";
                            DtTable = this.DIConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                            if (DtTable != null && DtTable.Rows.Count > 0)
                            {
                                Concept.Name.Add(new SDMXObjectModel.Common.TextType(Language, DtTable.Rows[0]["CategoryName"].ToString()));
                                Concept.Description.Add(new SDMXObjectModel.Common.TextType(Language, DtTable.Rows[0]["CategoryDescription"].ToString()));
                            }
                        }
                    }
                }
            }
        }

        private string Get_ParentCategoryGId(string ParentCategoryNId, DataTable DtMetadataCategory)
        {
            string RetVal;
            DataRow[] MetadataCategories;

            RetVal = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(ParentCategoryNId))
                {
                    MetadataCategories = DtMetadataCategory.Select("CategoryNId = " + ParentCategoryNId);

                    if (MetadataCategories.Length > 0)
                    {
                        RetVal = MetadataCategories[0]["CategoryGId"].ToString();
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

        private ArtefactInfo Prepare_ArtefactInfo_From_ConceptScheme(ConceptSchemeType ConceptScheme, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(ConceptScheme);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(ConceptScheme.id, ConceptScheme.agencyID, ConceptScheme.version, string.Empty, ArtefactTypes.ConceptS, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(ConceptSchemeType ConceptScheme)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, ConceptScheme, null, null, null, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;

            RetVal = null;

            try
            {
                if ((this._conceptSchemeType & ConceptSchemeTypes.ALL) == ConceptSchemeTypes.ALL)
                {
                    Artefact = this.Generate_DSD_ConceptScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MSD_Area_ConceptScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MSD_Indicator_ConceptScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MSD_Source_ConceptScheme();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else
                {
                    if ((this._conceptSchemeType & ConceptSchemeTypes.DSD) == ConceptSchemeTypes.DSD)
                    {
                        Artefact = this.Generate_DSD_ConceptScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._conceptSchemeType & ConceptSchemeTypes.MSD_Area) == ConceptSchemeTypes.MSD_Area)
                    {
                        Artefact = this.Generate_MSD_Area_ConceptScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._conceptSchemeType & ConceptSchemeTypes.MSD_Indicator) == ConceptSchemeTypes.MSD_Indicator)
                    {
                        Artefact = this.Generate_MSD_Indicator_ConceptScheme();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._conceptSchemeType & ConceptSchemeTypes.MSD_Source) == ConceptSchemeTypes.MSD_Source)
                    {
                        Artefact = this.Generate_MSD_Source_ConceptScheme();
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

        #endregion "--Public--"

        #endregion "--Methods--""
    }
}
