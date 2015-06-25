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

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class DSDUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private bool _multiLanguageHandlingRequired;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        #endregion "--Private--"

        #region "--Public--"

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

        internal DSDUtility(string agencyId, string language, Header header, string outputFolder, DIConnection DIConnection, DIQueries DIQueries)
            : base(agencyId, language, header, outputFolder)
        {
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

        private ArtefactInfo Prepare_ArtefactInfo_From_DataStructure(SDMXObjectModel.Structure.DataStructureType DataStructure, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(DataStructure);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(DataStructure.id, DataStructure.agencyID, DataStructure.version, string.Empty, ArtefactTypes.DSD, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.DataStructureType DataStructure)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, DataStructure, null, null, null, null, null, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Structure.DataStructureType DataStructure;
            DataTable DtSubgroupType;
            int ItemCounter = 0;

            RetVal = null;

            try
            {
                DataStructure = new SDMXObjectModel.Structure.DataStructureType(Constants.DSD.Id, this.AgencyId, Constants.DSD.Version, Constants.DSD.Name, Constants.DSD.Description, Constants.DefaultLanguage, null);
                DataStructure.Item = new DataStructureComponentsType();

                // Adding TIME_PERIOD Dimension
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Annotations = null; 
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new TimeDimensionType());
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.TIME_PERIOD.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new TimeDimensionRepresentationType();
                ((TimeDimensionRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new TimeTextFormatType());
                ((TimeTextFormatType)((TimeDimensionRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.ObservationalTimePeriod;

                // Adding AREA Dimension
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new DimensionType());
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.AREA.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new CodelistReferenceType());
                ((CodelistReferenceType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).Items.Add(new CodelistRefType(Constants.CodeList.Area.Id, this.AgencyId, Constants.CodeList.Area.Version));

                // Adding INDICATOR Dimension
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new DimensionType());
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.INDICATOR.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new CodelistReferenceType());
                ((CodelistReferenceType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).Items.Add(new CodelistRefType(Constants.CodeList.Indicator.Id, this.AgencyId, Constants.CodeList.Indicator.Version));

                // Adding UNIT Dimension
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new DimensionType());
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.UNIT.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new CodelistReferenceType());
                ((CodelistReferenceType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).Items.Add(new CodelistRefType(Constants.CodeList.Unit.Id, this.AgencyId, Constants.CodeList.Unit.Version));

                // Adding SOURCE Dimension
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new DimensionType());
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.SOURCE.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                DtSubgroupType = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                foreach (DataRow DrSubgroupType in DtSubgroupType.Rows)
                {
                    // Adding Subgroups Dimension
                    ItemCounter++;
                    ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items.Add(new DimensionType());
                    ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].Annotations = null;
                    ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                    ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString(), this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                    ((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                    ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items.Add(new CodelistReferenceType());
                    ((CodelistReferenceType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).DimensionList.Items[ItemCounter].LocalRepresentation).Items[0]).Items.Add(new CodelistRefType(Constants.CodelistPrefix + DrSubgroupType[SubgroupTypes.SubgroupTypeGID].ToString(), this.AgencyId, Constants.CodeList.Subgroups.Version));
                }

                // Adding PERIODICITY Attribute
                ItemCounter = 0;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Mandatory;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.PERIODICITY.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding NATURE Attribute
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Conditional;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.NATURE.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.Decimal;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding DENOMINATOR Attribute
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Conditional;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.DENOMINATOR.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.Decimal;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding FOOTNOTES Attribute
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Conditional;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.FOOTNOTES.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding CONFIDENCE_INTERVAL_UPPER Attribute
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Conditional;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding CONFIDENCE_INTERVAL_LOWER Attribute
                ItemCounter++;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items.Add(new AttributeType());
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).Annotations = null;
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).assignmentStatus = UsageStatusType.Conditional;
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship = new AttributeRelationshipType();
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.ItemsElementName = new AttributeRelationshipChoiceType[] { AttributeRelationshipChoiceType.PrimaryMeasure };
                ((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items = new LocalPrimaryMeasureReferenceType[] { new LocalPrimaryMeasureReferenceType() };
                ((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items.Add(new LocalPrimaryMeasureRefType());
                ((LocalPrimaryMeasureRefType)((LocalPrimaryMeasureReferenceType)((AttributeType)((DataStructureComponentsType)DataStructure.Item).AttributeList.Items[ItemCounter]).AttributeRelationship.Items[0]).Items[0]).id = Constants.Concept.OBS_VALUE.Id;

                // Adding OBS_VAL Primary Measure
                ItemCounter = 0;
                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Annotations = null; 
                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Items.Add(new PrimaryMeasureType());
                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].Annotations = null;
                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].ConceptIdentity = new SDMXObjectModel.Common.ConceptReferenceType();
                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].ConceptIdentity.Items.Add(new ConceptRefType(Constants.Concept.OBS_VALUE.Id, this.AgencyId, Constants.ConceptScheme.DSD.Id, Constants.ConceptScheme.DSD.Version));

                ((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].LocalRepresentation = new SimpleDataStructureRepresentationType();
                ((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].LocalRepresentation).Items.Add(new SimpleComponentTextFormatType());
                ((SimpleComponentTextFormatType)((SimpleDataStructureRepresentationType)((DataStructureComponentsType)DataStructure.Item).MeasureList.Items[ItemCounter].LocalRepresentation).Items[0]).textType = SDMXObjectModel.Common.DataType.String;

                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_DataStructure(DataStructure, Constants.DSD.FileName);
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
