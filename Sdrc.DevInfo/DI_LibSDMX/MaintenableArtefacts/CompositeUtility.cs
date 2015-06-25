using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Xml;
using SDMXObjectModel;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class CompositeUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private bool _completeOrSummaryFlag;

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        private Dictionary<string, string> _dictIndicator;

        private Dictionary<string, string> _dictIndicatorMapping;

        #endregion "--Private--"

        #region "--Public--"

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

        internal Dictionary<string, string> DictIndicator
        {
            get
            {
                return this._dictIndicator;
            }
            set
            {
                this._dictIndicator = value;
            }
        }

        internal Dictionary<string, string> DictIndicatorMapping
        {
            get
            {
                return this._dictIndicatorMapping;
            }
            set
            {
                this._dictIndicatorMapping = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal CompositeUtility(bool completeOrSummaryFlag, string agencyId, string language, Header header, string outputFolder, Dictionary<string, string> DictIndicator, Dictionary<string, string> DictIndicatorMapping, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
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

            this._dictIndicator = DictIndicator;
            this._dictIndicatorMapping = DictIndicatorMapping;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_Artefacts(List<ArtefactInfo> Artefacts)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            SDMXObjectModel.Message.StructureType LoadStructure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = new SDMXObjectModel.Message.StructureType();
                Structure.Header = this.Get_Appropriate_Header();
                Structure.Structures = new StructuresType();
                Structure.Structures.Codelists = new List<SDMXObjectModel.Structure.CodelistType>();
                Structure.Structures.CategorySchemes = new List<CategorySchemeType>();
                Structure.Structures.Categorisations = new List<CategorisationType>();
                Structure.Structures.Concepts = new List<SDMXObjectModel.Structure.ConceptSchemeType>();
                Structure.Structures.DataStructures = new List<SDMXObjectModel.Structure.DataStructureType>();
                Structure.Structures.MetadataStructures = new List<SDMXObjectModel.Structure.MetadataStructureType>();
                Structure.Structures.Dataflows = new List<DataflowType>();
                Structure.Structures.Metadataflows = new List<MetadataflowType>();
                Structure.Footer = null;

                foreach (ArtefactInfo Artefact in Artefacts)
                {
                    LoadStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Artefact.Content);

                    switch (Artefact.Type)
                    {
                        case ArtefactTypes.CL:
                            Structure.Structures.Codelists.AddRange(LoadStructure.Structures.Codelists);
                            break;
                        case ArtefactTypes.CategoryS:
                            Structure.Structures.CategorySchemes.AddRange(LoadStructure.Structures.CategorySchemes);
                            break;
                        case ArtefactTypes.Categorisation:
                            Structure.Structures.Categorisations.AddRange(LoadStructure.Structures.Categorisations);
                            break;
                        case ArtefactTypes.ConceptS:
                            Structure.Structures.Concepts.AddRange(LoadStructure.Structures.Concepts);
                            break;
                        case ArtefactTypes.DSD:
                            Structure.Structures.DataStructures.AddRange(LoadStructure.Structures.DataStructures);
                            break;
                        case ArtefactTypes.MSD:
                            Structure.Structures.MetadataStructures.AddRange(LoadStructure.Structures.MetadataStructures);
                            break;
                        case ArtefactTypes.DFD:
                            Structure.Structures.Dataflows.AddRange(LoadStructure.Structures.Dataflows);
                            break;
                        case ArtefactTypes.MFD:
                            Structure.Structures.Metadataflows.AddRange(LoadStructure.Structures.Metadataflows);
                            break;

                    }
                }

                this.Remove_Extra_Annotations(Structure);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);

                if (this._completeOrSummaryFlag == true)
                {
                    RetVal = new ArtefactInfo(Constants.Complete_XML.Id, this.AgencyId, Constants.Complete_XML.Version, string.Empty, ArtefactTypes.Complete, Constants.Complete_XML.FileName, XmlContent);
                }
                else
                {
                    RetVal = new ArtefactInfo(Constants.Summary_XML.Id, this.AgencyId, Constants.Summary_XML.Version, string.Empty, ArtefactTypes.Summary, Constants.Summary_XML.FileName, XmlContent);
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

        private void Remove_Extra_Annotations(SDMXObjectModel.Message.StructureType Structure)
        {
            foreach (CodelistType Codelist in Structure.Structures.Codelists)
            {
                Codelist.Annotations = null;
            }

            foreach (CategorySchemeType CategoryScheme in Structure.Structures.CategorySchemes)
            {
                CategoryScheme.Annotations = null;

                foreach (CategoryType Category in CategoryScheme.Items)
                {
                    Category.Annotations = null;
                    this.Remove_Extra_Annotations_From_Child_Categories(Category);
                }
            }

            foreach (CategorisationType Categorisation in Structure.Structures.Categorisations)
            {
                Categorisation.Annotations = null;

                ((ObjectRefType)Categorisation.Source.Items[0]).version = null;
            }

            // Removing annotation at ConceptScheme level
            foreach (ConceptSchemeType ConceptScheme in Structure.Structures.Concepts)
            {
                ConceptScheme.Annotations = null;

                foreach (ConceptType Concept in ConceptScheme.Items)
                {
                    Concept.Annotations = null;
                }
            }


            // Removing annotation at DataStructure level
            Structure.Structures.DataStructures[0].Annotations = null;

            // Removing annotation at Dimensionlist level
            ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).DimensionList = null;
            Structure.Structures.DataStructures[0].Item.Items[0].Annotations = null;
            foreach (BaseDimensionType BaseDimension in Structure.Structures.DataStructures[0].Item.Items[0].Items)
            {
                BaseDimension.Annotations = null;
            }

            // Removing annotation at Attributelist level
            ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).AttributeList = null;
            Structure.Structures.DataStructures[0].Item.Items[1].Annotations = null;
            foreach (AttributeType Attribute in Structure.Structures.DataStructures[0].Item.Items[1].Items)
            {
                Attribute.Annotations = null;
            }

            // Removing annotation at Measurelist level
            ((DataStructureComponentsType)Structure.Structures.DataStructures[0].Item).MeasureList = null;
            Structure.Structures.DataStructures[0].Item.Items[2].Annotations = null;
            foreach (PrimaryMeasureType PrimaryMeasure in Structure.Structures.DataStructures[0].Item.Items[2].Items)
            {
                PrimaryMeasure.Annotations = null;
            }

            // Removing annotation at MetadataStructure level
            foreach (SDMXObjectModel.Structure.MetadataStructureType MetadataStructure in Structure.Structures.MetadataStructures)
            {
                MetadataStructure.Annotations = null;

                // Removing annotation at MetadataTarget level
                MetadataStructure.Item.Items[0].Annotations = null;

                // Removing annotation at IdentifiableObjectTarget level
                MetadataStructure.Item.Items[0].Items[0].Annotations = null;

                // Removing annotation at ReportStructure level
                MetadataStructure.Item.Items[1].Annotations = null;

                // Removing annotation at MetadataAttribute level
                foreach (MetadataAttributeType MetadataAttribute in MetadataStructure.Item.Items[1].Items)
                {
                    MetadataAttribute.Annotations = null;
                }
            }

            // Removing annotation at DataFlow level
            Structure.Structures.Dataflows[0].Annotations = null;

            // Removing annotation at MetadataFlow level
            foreach (MetadataflowType MetadataFlow in Structure.Structures.Metadataflows)
            {
                MetadataFlow.Annotations = null;
            }
        }

        private void Remove_Extra_Annotations_From_Child_Categories(CategoryType ParentCategory)
        {
            foreach (CategoryType ChildCategory in ParentCategory.Items)
            {
                ChildCategory.Annotations = null;
                this.Remove_Extra_Annotations_From_Child_Categories(ChildCategory);
            }
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            List<ArtefactInfo> Artefacts;
            CodelistUtility CodelistUtility;
            CategorySchemeUtility CategorySchemeUtility;
            CategorizationUtility CategorizationUtility;
            ConceptSchemeUtility ConceptSchemeUtility;
            DSDUtility DSDUtility;
            MSDUtility MSDUtility;
            DFDUtility DFDUtility;
            MFDUtility MFDUtility;

            RetVal = null;
            Artefact = null;
            Artefacts = null;
            CodelistUtility = null;
            CategorySchemeUtility = null;
            CategorizationUtility = null;
            ConceptSchemeUtility = null;
            DSDUtility = null;
            MSDUtility = null;
            DFDUtility = null;
            MFDUtility = null;

            try
            {
                //-- Initialize Process
                SDMXUtility.Raise_Initilize_Process_Event("Generating Complete DSD...", 1, 1);

                CodelistUtility = new CodelistUtility(CodelistTypes.ALL, this._completeOrSummaryFlag, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts = CodelistUtility.Generate_Artefact();

                CategorySchemeUtility = new CategorySchemeUtility(CategorySchemeTypes.ALL, this._completeOrSummaryFlag, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._dictIndicator, this._dictIndicatorMapping, this._diConnection, this._diQueries);
                Artefacts.AddRange(CategorySchemeUtility.Generate_Artefact());

                if (this._completeOrSummaryFlag == true)
                {
                    CategorizationUtility = new CategorizationUtility(this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                     Artefacts.AddRange(CategorizationUtility.Generate_Artefact());
                    //Artefacts.AddRange(CategorizationUtility.Generate_CompleteArtefact(IcType));
                }

                ConceptSchemeUtility = new ConceptSchemeUtility(ConceptSchemeTypes.ALL, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(ConceptSchemeUtility.Generate_Artefact());

                DSDUtility = new DSDUtility(this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(DSDUtility.Generate_Artefact());

                MSDUtility = new MSDUtility(MSDTypes.ALL, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(MSDUtility.Generate_Artefact());

                DFDUtility = new DFDUtility(Constants.DSD.Id, this.AgencyId, this.Header, this.OutputFolder);
                Artefacts.AddRange(DFDUtility.Generate_Artefact());

                MFDUtility = new MFDUtility(MSDTypes.ALL, this.AgencyId, this.Header, this.OutputFolder);
                Artefacts.AddRange(MFDUtility.Generate_Artefact());

                Artefact = this.Prepare_ArtefactInfo_From_Artefacts(Artefacts);
                this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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

        public List<ArtefactInfo> Generate_Artefact(string IcType)
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            List<ArtefactInfo> Artefacts;
            CodelistUtility CodelistUtility;
            CategorySchemeUtility CategorySchemeUtility;
            CategorizationUtility CategorizationUtility;
            ConceptSchemeUtility ConceptSchemeUtility;
            DSDUtility DSDUtility;
            MSDUtility MSDUtility;
            DFDUtility DFDUtility;
            MFDUtility MFDUtility;

            RetVal = null;
            Artefact = null;
            Artefacts = null;
            CodelistUtility = null;
            CategorySchemeUtility = null;
            CategorizationUtility = null;
            ConceptSchemeUtility = null;
            DSDUtility = null;
            MSDUtility = null;
            DFDUtility = null;
            MFDUtility = null;

            try
            {
                //-- Initialize Process
                SDMXUtility.Raise_Initilize_Process_Event("Generating Complete DSD...", 1, 1);

                CodelistUtility = new CodelistUtility(CodelistTypes.ALL, this._completeOrSummaryFlag, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts = CodelistUtility.Generate_Artefact();

                CategorySchemeUtility = new CategorySchemeUtility(CategorySchemeTypes.ALL, this._completeOrSummaryFlag, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._dictIndicator, this._dictIndicatorMapping, this._diConnection, this._diQueries);
                Artefacts.AddRange(CategorySchemeUtility.Generate_Artefact());

                if (this._completeOrSummaryFlag == true)
                {
                    CategorizationUtility = new CategorizationUtility(this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                   // Artefacts.AddRange(CategorizationUtility.Generate_Artefact());
                    Artefacts.AddRange(CategorizationUtility.Generate_CompleteArtefact(IcType));
                }

                ConceptSchemeUtility = new ConceptSchemeUtility(ConceptSchemeTypes.ALL, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(ConceptSchemeUtility.Generate_Artefact());

                DSDUtility = new DSDUtility(this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(DSDUtility.Generate_Artefact());

                MSDUtility = new MSDUtility(MSDTypes.ALL, this.AgencyId, this.Language, this.Header, this.OutputFolder, this._diConnection, this._diQueries);
                Artefacts.AddRange(MSDUtility.Generate_Artefact());

                DFDUtility = new DFDUtility(Constants.DSD.Id, this.AgencyId, this.Header, this.OutputFolder);
                Artefacts.AddRange(DFDUtility.Generate_Artefact());

                MFDUtility = new MFDUtility(MSDTypes.ALL, this.AgencyId, this.Header, this.OutputFolder);
                Artefacts.AddRange(MFDUtility.Generate_Artefact());

                Artefact = this.Prepare_ArtefactInfo_From_Artefacts(Artefacts);
                this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
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
