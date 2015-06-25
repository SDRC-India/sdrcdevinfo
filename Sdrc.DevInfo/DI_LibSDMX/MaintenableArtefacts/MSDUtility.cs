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
using System.Text.RegularExpressions;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class MSDUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private MSDTypes _msdType;

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        #endregion "--Private--"

        #region "--Public--"

        internal MSDTypes MSDType
        {
            get
            {
                return this._msdType;
            }
            set
            {
                this._msdType = value;
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

        internal MSDUtility(MSDTypes msdType, string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
            this._msdType = msdType;
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

        private ArtefactInfo Generate_MSD_Area()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataStructureType MetadataStructure;
            MetadataTargetType MetadataTarget;
            ReportStructureType ReportStructure;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            MetadataStructure = null;
            MetadataTarget = null;
            ReportStructure = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                MetadataStructure = new SDMXObjectModel.Structure.MetadataStructureType(Constants.MSD.Area.Id, this.AgencyId, Constants.MSD.Area.Version, Constants.MSD.Area.Name, Constants.MSD.Area.Description, Constants.DefaultLanguage, null);
                MetadataStructure.Item = new MetadataStructureComponentsType();

                #region "--Forming Metadata Target--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget = new List<MetadataTargetType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget.Add(new MetadataTargetType(Constants.MSD.Area.MetadataTargetId, null));
                MetadataTarget = ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget[0];

                MetadataTarget.Annotations = null;
                MetadataTarget.Items1 = new List<TargetObject>();
                MetadataTarget.Items1.Add(new IdentifiableObjectTargetType(Constants.MSD.Area.IdentifiableObjectTargetId, ObjectTypeCodelistType.Code, null));

                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).Annotations = null;
                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation = new IdentifiableObjectRepresentationType();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items = new List<object>();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items.Add(new ItemSchemeReferenceType());

                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items = new List<object>();
                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items.Add(new ItemSchemeRefType(Constants.CodeList.Area.Id, this.AgencyId, Constants.CodeList.Area.Version, PackageTypeCodelistType.codelist, true, ObjectTypeCodelistType.Codelist, true, false, true));

                #endregion "--Forming Metadata Target--"

                #region "--Forming Report Structure--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure = new List<ReportStructureType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure.Add(new ReportStructureType(Constants.MSD.Area.ReportStructureId, null));
                ReportStructure = ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure[0];

                ReportStructure.Annotations = null;
                ReportStructure.MetadataTarget = new List<LocalMetadataTargetReferenceType>();
                ReportStructure.MetadataTarget.Add(new LocalMetadataTargetReferenceType());

                ReportStructure.MetadataTarget[0].Items = new List<object>();
                ReportStructure.MetadataTarget[0].Items.Add(new LocalMetadataTargetRefType(Constants.MSD.Area.MetadataTargetId, ObjectTypeCodelistType.MetadataTarget, true, PackageTypeCodelistType.metadatastructure, true, true, true));

                #endregion "--Forming Report Structure--"

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'A' ORDER BY CategoryOrder ASC;";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ReportStructure(ReportStructure, DtMetadataCategory, MSDTypes.MSD_Area);

                RetVal = this.Prepare_ArtefactInfo_From_MetadataStructure(MetadataStructure, Constants.MSD.Area.FileName);
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

        private ArtefactInfo Generate_MSD_Indicator()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataStructureType MetadataStructure;
            MetadataTargetType MetadataTarget;
            ReportStructureType ReportStructure;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            MetadataStructure = null;
            MetadataTarget = null;
            ReportStructure = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                MetadataStructure = new SDMXObjectModel.Structure.MetadataStructureType(Constants.MSD.Indicator.Id, this.AgencyId, Constants.MSD.Indicator.Version, Constants.MSD.Indicator.Name, Constants.MSD.Indicator.Description, Constants.DefaultLanguage, null);
                MetadataStructure.Item = new MetadataStructureComponentsType();

                #region "--Forming Metadata Target--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget = new List<MetadataTargetType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget.Add(new MetadataTargetType(Constants.MSD.Indicator.MetadataTargetId, null));
                MetadataTarget = ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget[0];

                MetadataTarget.Annotations = null;
                MetadataTarget.Items1 = new List<TargetObject>();
                MetadataTarget.Items1.Add(new IdentifiableObjectTargetType(Constants.MSD.Indicator.IdentifiableObjectTargetId, ObjectTypeCodelistType.Code, null));

                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).Annotations = null;
                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation = new IdentifiableObjectRepresentationType();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items = new List<object>();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items.Add(new ItemSchemeReferenceType());

                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items = new List<object>();
                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items.Add(new ItemSchemeRefType(Constants.CodeList.Indicator.Id, this.AgencyId, Constants.CodeList.Indicator.Version, PackageTypeCodelistType.codelist, true, ObjectTypeCodelistType.Codelist, true, false, true));

                #endregion "--Forming Metadata Target--"

                #region "--Forming Report Structure--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure = new List<ReportStructureType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure.Add(new ReportStructureType(Constants.MSD.Indicator.ReportStructureId, null));
                ReportStructure = ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure[0];

                ReportStructure.Annotations = null;
                ReportStructure.MetadataTarget = new List<LocalMetadataTargetReferenceType>();
                ReportStructure.MetadataTarget.Add(new LocalMetadataTargetReferenceType());

                ReportStructure.MetadataTarget[0].Items = new List<object>();
                ReportStructure.MetadataTarget[0].Items.Add(new LocalMetadataTargetRefType(Constants.MSD.Indicator.MetadataTargetId, ObjectTypeCodelistType.MetadataTarget, true, PackageTypeCodelistType.metadatastructure, true, true, true));

                #endregion "--Forming Report Structure--"

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'I' ORDER BY CategoryOrder ASC;";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ReportStructure(ReportStructure, DtMetadataCategory, MSDTypes.MSD_Indicator);

                RetVal = this.Prepare_ArtefactInfo_From_MetadataStructure(MetadataStructure, Constants.MSD.Indicator.FileName);
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

        private ArtefactInfo Generate_MSD_Source()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataStructureType MetadataStructure;
            MetadataTargetType MetadataTarget;
            ReportStructureType ReportStructure;
            string Query;
            DataTable DtMetadataCategory;

            RetVal = null;
            MetadataStructure = null;
            MetadataTarget = null;
            ReportStructure = null;
            Query = string.Empty;
            DtMetadataCategory = null;

            try
            {
                MetadataStructure = new SDMXObjectModel.Structure.MetadataStructureType(Constants.MSD.Source.Id, this.AgencyId, Constants.MSD.Source.Version, Constants.MSD.Source.Name, Constants.MSD.Source.Description, Constants.DefaultLanguage, null);
                MetadataStructure.Item = new MetadataStructureComponentsType();

                #region "--Forming Metadata Target--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget = new List<MetadataTargetType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget.Add(new MetadataTargetType(Constants.MSD.Source.MetadataTargetId, null));
                MetadataTarget = ((MetadataStructureComponentsType)MetadataStructure.Item).MetadataTarget[0];

                MetadataTarget.Annotations = null;
                MetadataTarget.Items1 = new List<TargetObject>();
                MetadataTarget.Items1.Add(new IdentifiableObjectTargetType(Constants.MSD.Source.IdentifiableObjectTargetId, ObjectTypeCodelistType.Category, null));

                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).Annotations = null;
                ((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation = new IdentifiableObjectRepresentationType();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items = new List<object>();
                ((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items.Add(new ItemSchemeReferenceType());

                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items = new List<object>();
                ((ItemSchemeReferenceType)((IdentifiableObjectRepresentationType)((IdentifiableObjectTargetType)MetadataTarget.Items1[0]).LocalRepresentation).Items[0]).Items.Add(new ItemSchemeRefType(Constants.CategoryScheme.Source.Id, this.AgencyId, Constants.CategoryScheme.Source.Version, PackageTypeCodelistType.categoryscheme, true, ObjectTypeCodelistType.CategoryScheme, true, false, true));

                #endregion "--Forming Metadata Target--"

                #region "--Forming Report Structure--"

                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure = new List<ReportStructureType>();
                ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure.Add(new ReportStructureType(Constants.MSD.Source.ReportStructureId, null));
                ReportStructure = ((MetadataStructureComponentsType)MetadataStructure.Item).ReportStructure[0];

                ReportStructure.Annotations = null;
                ReportStructure.MetadataTarget = new List<LocalMetadataTargetReferenceType>();
                ReportStructure.MetadataTarget.Add(new LocalMetadataTargetReferenceType());

                ReportStructure.MetadataTarget[0].Items = new List<object>();
                ReportStructure.MetadataTarget[0].Items.Add(new LocalMetadataTargetRefType(Constants.MSD.Source.MetadataTargetId, ObjectTypeCodelistType.MetadataTarget, true, PackageTypeCodelistType.metadatastructure, true, true, true));

                #endregion "--Forming Report Structure--"

                Query = "SELECT * FROM UT_Metadata_Category_" + this.Language + " WHERE CategoryType = 'S' ORDER BY CategoryOrder ASC;";
                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
                this.Fill_ReportStructure(ReportStructure, DtMetadataCategory, MSDTypes.MSD_Source);

                RetVal = this.Prepare_ArtefactInfo_From_MetadataStructure(MetadataStructure, Constants.MSD.Source.FileName);
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

        private void Fill_ReportStructure(ReportStructureType ReportStructure, DataTable DtMetadataCategory, MSDTypes msdType)
        {
            MetadataAttributeType MetadataAttribute;
            DataRow[] ParentRows;
            string CategoryNId, CategoryGId;
            bool IsPresentational;

            ReportStructure.Items = new List<ComponentType>();
            ParentRows = DtMetadataCategory.Select("ParentCategoryNId = -1", "CategoryOrder ASC");

            foreach (DataRow ParentRow in ParentRows)
            {
                CategoryNId = ParentRow["CategoryNId"].ToString();
                CategoryGId = ParentRow["CategoryGId"].ToString();
                IsPresentational = Convert.ToBoolean(ParentRow["IsPresentational"].ToString());

                MetadataAttribute = new MetadataAttributeType(CategoryGId, IsPresentational, null);
                this.Add_Children_Attributes(MetadataAttribute, CategoryNId, DtMetadataCategory, msdType);
                this.Fill_MetadataAttribute(MetadataAttribute, CategoryGId, msdType);
                
                ReportStructure.Items.Add(MetadataAttribute);
            }
        }

        private void Add_Children_Attributes(MetadataAttributeType MetadataAttribute, string CategoryNId, DataTable DtMetadataCategory, MSDTypes msdType)
        {
            MetadataAttributeType ChildMetadataAttribute;
            DataRow[] ChildRows;
            string ChildCategoryNId, ChildCategoryGId;
            bool ChildIsPresentational;

            ChildRows = DtMetadataCategory.Select("ParentCategoryNId = " + CategoryNId, "CategoryOrder ASC");

            if (ChildRows.Length > 0)
            {
                MetadataAttribute.MetadataAttribute = new List<MetadataAttributeType>();

                foreach (DataRow ChildRow in ChildRows)
                {
                    ChildCategoryNId = ChildRow["CategoryNId"].ToString();
                    ChildCategoryGId = ChildRow["CategoryGId"].ToString();
                    ChildIsPresentational = Convert.ToBoolean(ChildRow["IsPresentational"].ToString());

                    ChildMetadataAttribute = new MetadataAttributeType(ChildCategoryGId, ChildIsPresentational, null);
                    this.Add_Children_Attributes(ChildMetadataAttribute, ChildCategoryNId, DtMetadataCategory, msdType);
                    this.Fill_MetadataAttribute(ChildMetadataAttribute, ChildCategoryGId, msdType);

                    MetadataAttribute.MetadataAttribute.Add(ChildMetadataAttribute);
                }
            }
            else
            {
                MetadataAttribute.MetadataAttribute = null;
            }
        }

        private void Fill_MetadataAttribute(MetadataAttributeType MetadataAttribute, string CategoryGId, MSDTypes msdType)
        {
            MetadataAttribute.Annotations = null;
            MetadataAttribute.ConceptIdentity = new ConceptReferenceType();
            MetadataAttribute.ConceptIdentity.Items = new List<object>();

            switch (msdType)
            {
                case MSDTypes.MSD_Area:
                    MetadataAttribute.ConceptIdentity.Items.Add(new ConceptRefType(CategoryGId, this.AgencyId, Constants.ConceptScheme.MSD_Area.Id, Constants.ConceptScheme.MSD_Area.Version));
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).local = false;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).localSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).@class = ObjectTypeCodelistType.Concept;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).classSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).package = PackageTypeCodelistType.conceptscheme;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).packageSpecified = true;
                    break;
                case MSDTypes.MSD_Indicator:
                    MetadataAttribute.ConceptIdentity.Items.Add(new ConceptRefType(CategoryGId, this.AgencyId, Constants.ConceptScheme.MSD_Indicator.Id, Constants.ConceptScheme.MSD_Indicator.Version));
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).local = false;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).localSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).@class = ObjectTypeCodelistType.Concept;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).classSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).package = PackageTypeCodelistType.conceptscheme;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).packageSpecified = true;
                    break;
                case MSDTypes.MSD_Source:
                    MetadataAttribute.ConceptIdentity.Items.Add(new ConceptRefType(CategoryGId, this.AgencyId, Constants.ConceptScheme.MSD_Source.Id, Constants.ConceptScheme.MSD_Source.Version));
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).local = false;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).localSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).@class = ObjectTypeCodelistType.Concept;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).classSpecified = true;

                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).package = PackageTypeCodelistType.conceptscheme;
                    ((ConceptRefType)MetadataAttribute.ConceptIdentity.Items[0]).packageSpecified = true;
                    break;
            }

            MetadataAttribute.LocalRepresentation = new MetadataAttributeRepresentationType();
            MetadataAttribute.LocalRepresentation.Items = new List<object>();
            MetadataAttribute.LocalRepresentation.Items.Add(new BasicComponentTextFormatType());
            ((BasicComponentTextFormatType)MetadataAttribute.LocalRepresentation.Items[0]).textType = SDMXObjectModel.Common.DataType.String;
        }

        private ArtefactInfo Prepare_ArtefactInfo_From_MetadataStructure(SDMXObjectModel.Structure.MetadataStructureType MetadataStructure, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(MetadataStructure);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(MetadataStructure.id, MetadataStructure.agencyID, MetadataStructure.version, string.Empty, ArtefactTypes.MSD, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.MetadataStructureType MetadataStructure)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, MetadataStructure, null, null, null, null, null, null, null, null);
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
                if ((this._msdType & MSDTypes.ALL) == MSDTypes.ALL)
                {
                    Artefact = this.Generate_MSD_Area();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MSD_Indicator();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MSD_Source();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else
                {
                    if ((this._msdType & MSDTypes.MSD_Area) == MSDTypes.MSD_Area)
                    {
                        Artefact = this.Generate_MSD_Area();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._msdType & MSDTypes.MSD_Indicator) == MSDTypes.MSD_Indicator)
                    {
                        Artefact = this.Generate_MSD_Indicator();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._msdType & MSDTypes.MSD_Source) == MSDTypes.MSD_Source)
                    {
                        Artefact = this.Generate_MSD_Source();
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
