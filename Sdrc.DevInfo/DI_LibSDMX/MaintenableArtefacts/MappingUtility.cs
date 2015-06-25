using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Xml;
using System.Data;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel;
using System.IO;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class MappingUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private MappingType _mappingType;

        private Dictionary<string, string> _dictMapping;

        private string _sourceId;

        private string _sourceAgencyId;

        private string _sourceVersion;

        private string _targetId;

        private string _targetAgencyId;

        private string _targetVersion;

        private string _fileNameWPath;

        private string _codelistName;
        #endregion "--Private--"

        #region "--Public--"

        internal MappingType MappingType
        {
            get
            {
                return this._mappingType;
            }
            set
            {
                this._mappingType = value;
            }
        }

        internal Dictionary<string, string> DictMapping
        {
            get
            {
                return this._dictMapping;
            }
            set
            {
                this._dictMapping = value;
            }
        }

        internal string SourceId
        {
            get
            {
                return this._sourceId;
            }
            set
            {
                this._sourceId = value;
            }
        }

        internal string SourceAgencyId
        {
            get
            {
                return this._sourceAgencyId;
            }
            set
            {
                this._sourceAgencyId = value;
            }
        }

        internal string SourceVersion
        {
            get
            {
                return this._sourceVersion;
            }
            set
            {
                this._sourceVersion = value;
            }
        }

        internal string TargetId
        {
            get
            {
                return this._targetId;
            }
            set
            {
                this._targetId = value;
            }
        }

        internal string TargetAgencyId
        {
            get
            {
                return this._targetAgencyId;
            }
            set
            {
                this._targetAgencyId = value;
            }
        }

        internal string TargetVersion
        {
            get
            {
                return this._targetVersion;
            }
            set
            {
                this._targetVersion = value;
            }
        }

        internal string FileNameWPath
        {
            get
            {
                return this._fileNameWPath;
            }
            set
            {
                this._fileNameWPath = value;
            }
        }

        internal string CodelistName
        {
            get
            {
                return this._codelistName;
            }
            set
            {
                this._codelistName = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal MappingUtility(MappingType mappingType, Dictionary<string, string> dictMapping, string sourceId, string sourceAgencyId, string sourceVersion, string targetId, string targetAgencyId, string targetVersion, string agencyId, string language, Header header, string fileNameWPath, string outputFolder,string codelistName)
            : base(agencyId, language, header, outputFolder)
        {
            this._mappingType = mappingType;
            this._dictMapping = dictMapping;
            this._sourceId = sourceId;
            this._sourceAgencyId = sourceAgencyId;
            this._sourceVersion = sourceVersion;
            this._targetId = targetId;
            this._targetAgencyId = targetAgencyId;
            this._targetVersion = targetVersion;
            this._fileNameWPath = fileNameWPath;
            this._codelistName = codelistName;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Generate_Codelist_Mapping_Artefact()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            StructureSetType StructureSet;
            CodelistMapType CodelistMap;
            CodeMapType CodeMap;
            string CodelistMapId, CodelistMapName;
            string FileName, OutputFolder;

            RetVal = null;
            StructureSet = null;
            CodelistMap = null;
            CodeMap = null;
            CodelistMapId = string.Empty;
            CodelistMapName = string.Empty;
            FileName = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(this._fileNameWPath) || File.Exists(this._fileNameWPath))
                {
                    Structure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), this._fileNameWPath);

                    if (Structure != null && Structure.Structures != null && Structure.Structures.StructureSets != null &&
                        Structure.Structures.StructureSets.Count > 0)
                    {
                        StructureSet = Structure.Structures.StructureSets[0];
                    }

                    this.Remove_Extra_Annotations(Structure);

                    FileName = Path.GetFileName(this._fileNameWPath);
                    this.OutputFolder = Path.GetDirectoryName(this._fileNameWPath);
                }
                else
                {
                    StructureSet = new StructureSetType();
                    StructureSet.id = Constants.StructureSet.id;
                    StructureSet.agencyID = this.AgencyId;
                    StructureSet.version = Constants.StructureSet.version;

                    StructureSet.Name = new List<TextType>();
                    StructureSet.Name.Add(new TextType(this.Language, Constants.StructureSet.CodelistMap.name));

                    StructureSet.Annotations = null;

                    FileName = Constants.StructureSet.CodelistMap.FileName;
                }

                switch (this._codelistName)//_sourceId
                {
                    case Constants.CodeList.Indicator.Id:
                        CodelistMapId = Constants.StructureSet.CodelistMap.Indicator.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Indicator.name;
                        break;
                    case Constants.CodeList.Unit.Id:
                        CodelistMapId = Constants.StructureSet.CodelistMap.Unit.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Unit.name;
                        break;
                    case Constants.CodelistPrefix + "AGE":
                        CodelistMapId = Constants.StructureSet.CodelistMap.Age.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Age.name;
                        break;
                    case Constants.CodelistPrefix + "SEX":
                        CodelistMapId = Constants.StructureSet.CodelistMap.Sex.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Sex.name;
                        break;
                    case Constants.CodelistPrefix + "LOCATION":
                        CodelistMapId = Constants.StructureSet.CodelistMap.Location.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Location.name;
                        break;
                    case Constants.CodelistPrefix + "AREA":
                        CodelistMapId = Constants.StructureSet.CodelistMap.Area.id;
                        CodelistMapName = Constants.StructureSet.CodelistMap.Area.name;
                        break;
                    default:
                        break;
                }

                CodelistMap = new SDMXObjectModel.Structure.CodelistMapType();
                CodelistMap.id = CodelistMapId;

                CodelistMap.Name = new List<TextType>();
                CodelistMap.Name.Add(new TextType(Language, CodelistMapName));

                CodelistMap.Source = new SDMXObjectModel.Common.CodelistReferenceType();
                CodelistMap.Source.Items = new List<object>();
                CodelistMap.Source.Items.Add(new SDMXObjectModel.Common.CodelistRefType());
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Source.Items[0]).id = this._sourceId;
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Source.Items[0]).agencyID = this._sourceAgencyId;
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Source.Items[0]).version = this._sourceVersion;

                CodelistMap.Target = new SDMXObjectModel.Common.CodelistReferenceType();
                CodelistMap.Target.Items = new List<object>();
                CodelistMap.Target.Items.Add(new SDMXObjectModel.Common.CodelistRefType());
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Target.Items[0]).id = this._targetId;
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Target.Items[0]).agencyID = this._targetAgencyId;
                ((SDMXObjectModel.Common.CodelistRefType)CodelistMap.Target.Items[0]).version = this._targetVersion;

                CodelistMap.Annotations = null;

                if (this._dictMapping != null && this._dictMapping.Keys.Count > 0)
                {
                    if (StructureSet.Items == null || StructureSet.Items.Count == 0)
                    {
                        StructureSet.Items = new List<NameableType>();
                    }
                    
                    CodelistMap.Items = new List<SDMXObjectModel.Structure.ItemAssociationType>();

                    foreach (string SourceGId in this._dictMapping.Keys)
                    {
                        CodeMap = new SDMXObjectModel.Structure.CodeMapType();

                        CodeMap.Source = new SDMXObjectModel.Common.LocalCodeReferenceType();
                        CodeMap.Source.Items = new List<object>();
                        CodeMap.Source.Items.Add(new SDMXObjectModel.Common.LocalCodeRefType());
                        ((LocalCodeRefType)CodeMap.Source.Items[0]).id = SourceGId;

                        CodeMap.Target = new SDMXObjectModel.Common.LocalCodeReferenceType();
                        CodeMap.Target.Items = new List<object>();
                        CodeMap.Target.Items.Add(new SDMXObjectModel.Common.LocalCodeRefType());
                        ((LocalCodeRefType)CodeMap.Target.Items[0]).id = this._dictMapping[SourceGId];

                        CodeMap.Annotations = null;
                        CodelistMap.Items.Add(CodeMap);
                    }

                    StructureSet.Items.Add(CodelistMap);
                }

                RetVal = this.Prepare_ArtefactInfo_From_StructureSet(StructureSet, FileName);
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

        private ArtefactInfo Generate_Metadata_Mapping()
        {
            ArtefactInfo RetVal;
            StructureSetType StructureSet;
            ConceptSchemeMapType ConceptSchemeMap;
            ConceptMapType ConceptMap;

            RetVal = null;
            StructureSet = null;
            ConceptSchemeMap = null;
            ConceptMap = null;

            try
            {
                StructureSet = new StructureSetType();
                StructureSet.id = Constants.StructureSet.id;
                StructureSet.agencyID = this.AgencyId;
                StructureSet.version = Constants.StructureSet.version;

                StructureSet.Name = new List<TextType>();
                StructureSet.Name.Add(new TextType(this.Language, Constants.StructureSet.ConceptSchemeMap.name));

                StructureSet.Annotations = null;

                ConceptSchemeMap = new SDMXObjectModel.Structure.ConceptSchemeMapType();
                ConceptSchemeMap.id = Constants.StructureSet.ConceptSchemeMap.MetadataMap.id;

                ConceptSchemeMap.Name = new List<TextType>();
                ConceptSchemeMap.Name.Add(new TextType(Language, Constants.StructureSet.ConceptSchemeMap.MetadataMap.name));

                ConceptSchemeMap.Source = new SDMXObjectModel.Common.ConceptSchemeReferenceType();
                ConceptSchemeMap.Source.Items = new List<object>();
                ConceptSchemeMap.Source.Items.Add(new SDMXObjectModel.Common.ConceptSchemeRefType());
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Source.Items[0]).id = this._sourceId;
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Source.Items[0]).agencyID = this._sourceAgencyId;
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Source.Items[0]).version = this._sourceVersion;

                ConceptSchemeMap.Target = new SDMXObjectModel.Common.ConceptSchemeReferenceType();
                ConceptSchemeMap.Target.Items = new List<object>();
                ConceptSchemeMap.Target.Items.Add(new SDMXObjectModel.Common.ConceptSchemeRefType());
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Target.Items[0]).id = this._targetId;
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Target.Items[0]).agencyID = this._targetAgencyId;
                ((SDMXObjectModel.Common.ConceptSchemeRefType)ConceptSchemeMap.Target.Items[0]).version = this._targetVersion;

                ConceptSchemeMap.Annotations = null;

                if (this._dictMapping != null && this._dictMapping.Keys.Count > 0)
                {
                    StructureSet.Items = new List<NameableType>();
                    ConceptSchemeMap.Items = new List<SDMXObjectModel.Structure.ItemAssociationType>();

                    foreach (string SourceGId in this._dictMapping.Keys)
                    {
                        ConceptMap = new SDMXObjectModel.Structure.ConceptMapType();

                        ConceptMap.Source = new SDMXObjectModel.Common.LocalConceptReferenceType();
                        ConceptMap.Source.Items = new List<object>();
                        ConceptMap.Source.Items.Add(new SDMXObjectModel.Common.LocalConceptRefType());
                        ((LocalConceptRefType)ConceptMap.Source.Items[0]).id = SourceGId;

                        ConceptMap.Target = new SDMXObjectModel.Common.LocalConceptReferenceType();
                        ConceptMap.Target.Items = new List<object>();
                        ConceptMap.Target.Items.Add(new SDMXObjectModel.Common.LocalConceptRefType());
                        ((LocalConceptRefType)ConceptMap.Target.Items[0]).id = this._dictMapping[SourceGId];

                        ConceptMap.Annotations = null;
                        ConceptSchemeMap.Items.Add(ConceptMap);
                    }

                    StructureSet.Items.Add(ConceptSchemeMap);
                }

                RetVal = this.Prepare_ArtefactInfo_From_StructureSet(StructureSet, Constants.StructureSet.ConceptSchemeMap.MetadataMap.FileName);
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
            if (Structure != null && Structure.Structures != null && Structure.Structures.StructureSets != null &&
                Structure.Structures.StructureSets.Count > 0)
            {
                Structure.Structures.StructureSets[0].Annotations = null;

                if (Structure.Structures.StructureSets[0].Items != null && Structure.Structures.StructureSets[0].Items.Count > 0)
                {
                    foreach (SDMXObjectModel.Structure.NameableType CodelistMap in Structure.Structures.StructureSets[0].Items)
                    {
                        CodelistMap.Annotations = null;

                        if (CodelistMap != null && ((CodelistMapType)CodelistMap).Items != null && ((CodelistMapType)CodelistMap).Items.Count > 0)
                        {
                            foreach (SDMXObjectModel.Structure.ItemAssociationType CodeMap in ((CodelistMapType)CodelistMap).Items)
                            {
                                CodeMap.Annotations = null;
                            }
                        }
                    }
                }
            }
        }

        private ArtefactInfo Prepare_ArtefactInfo_From_StructureSet(SDMXObjectModel.Structure.StructureSetType StructureSet, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_StructureType_Object(StructureSet);
                XmlContent = SDMXObjectModel.Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(StructureSet.id, StructureSet.agencyID, StructureSet.version, string.Empty, ArtefactTypes.Mapping, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_StructureType_Object(SDMXObjectModel.Structure.StructureSetType StructureSet)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, StructureSet, null, null, null, null, null, null, null, null, null, null);
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
                if (MappingType == MappingType.Codelist)
                {
                    Artefact = this.Generate_Codelist_Mapping_Artefact();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else if (MappingType == MappingType.Metadata)
                {
                    Artefact = this.Generate_Metadata_Mapping();
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

        #endregion "--Public--"

        #endregion "--Methods--""
    }
}
